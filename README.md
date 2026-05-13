# Odex.AspNetCore.Clarc.Domain

[![NuGet](https://img.shields.io/nuget/v/Odex.AspNetCore.Clarc.Domain)](https://www.nuget.org/packages/Odex.AspNetCore.Clarc.Domain)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Odex.AspNetCore.Clarc.Domain)](https://www.nuget.org/packages/Odex.AspNetCore.Clarc.Domain)
[![CI](https://github.com/o-shabi/odex-clarc-domain-aspnetcore/actions/workflows/ci.yml/badge.svg)](https://github.com/o-shabi/odex-clarc-domain-aspnetcore/actions/workflows/ci.yml)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Reusable **domain-layer** building blocks for **.NET**: aggregates, domain events, specifications, guard policies, persistence-oriented repository contracts, and transaction abstractions. The **public API is fully documented** with XML comments for IntelliSense and NuGet. **No ASP.NET Core or EF Core dependency**—this assembly targets plain `net9.0` so you can use it from any host or stack.

---

## Table of contents

- [About](#about)
- [Installation](#installation)
- [Requirements](#requirements)
- [How it fits in CLARC / clean architecture](#how-it-fits-in-clarc--clean-architecture)
- [Capabilities](#capabilities)
- [API documentation](#api-documentation)
- [Examples](#examples)
- [Namespaces](#namespaces)
- [Upgrading](#upgrading)
- [Contributing](#contributing)
- [Community](#community)
- [License](#license)
- [Links](#links)

---

## About

**Odex.AspNetCore.Clarc.Domain** is part of the **CLARC** family of packages. It gives you shared types and interfaces so domain code stays consistent across solutions: typed aggregate roots, optional lifecycle state, composable query predicates (`Specification<T>`), lightweight **aggregate events** (raised in the domain, **dispatched** in your infrastructure), guard-style checks (`BasePolicy<T>`), and async repository contracts you implement against your chosen store.

The package is **opinionated about structure, not about frameworks**. You wire **dependency injection**, **ORMs**, **outboxes**, and **mediators** in **Application** and **Infrastructure**—not in this library.

---

## Installation

```bash
dotnet add package Odex.AspNetCore.Clarc.Domain
```

Pin a version when you need a reproducible build:

```bash
dotnet add package Odex.AspNetCore.Clarc.Domain --version 0.2.0
```

---

## Requirements

| Item | Version |
|------|---------|
| **Target framework** | `net9.0` |
| **.NET SDK** | 9.x (for development and consumption aligned with the target) |

This library has **no NuGet dependencies** at runtime beyond the .NET runtime. The package build references **Microsoft.SourceLink.GitHub** (private asset) so consumers get GitHub-backed **source debugging** in supported IDEs. The same build emits **XML API documentation** for all public types and members (see [API documentation](#api-documentation)). Your **Infrastructure** project references EF Core, Dapper, or other stacks as needed.

To build and test this repository locally, see **[CONTRIBUTING.md](CONTRIBUTING.md)**.

---

## How it fits in CLARC / clean architecture

| Layer | Responsibility | Typical CLARC package |
|-------|----------------|------------------------|
| **Domain** | Entities, value objects, domain services, invariants, aggregate events (raised, not dispatched) | **This package** |
| **Application** | Use cases, commands/queries, handlers, orchestration | *Odex.AspNetCore.Clarc.Application* (your solution) |
| **Infrastructure** | Persistence, external APIs, event dispatch, DI registrations | *Odex.AspNetCore.Clarc.Infrastructure* (your solution) |

**Repository interfaces** (`IAggregateRepository<,>`) use `IQueryable` / `Expression<>` so implementations can translate predicates to SQL (for example with EF Core). That is a deliberate trade-off: the contract stays **persistence-oriented** while remaining **store-agnostic** at compile time.

---

## Capabilities

| Area | Types (summary) |
|------|------------------|
| **Aggregates** | `BaseAggregate<TId>` — identity, audit timestamps, in-memory domain event list. `StatefulAggregate<TId>` — activation and soft-delete flags with timestamps. |
| **Events** | `IAggregateEvent`, `AggregateEvent` — correlation id and `OccurredOn`; collect with `AddEvent`, read with `ListEvents`, clear after dispatch in infrastructure. |
| **Specifications** | `Specification<T>` with `And` / `Or` / `Not`; `ToExpression()` for providers that compile LINQ expressions. |
| **Policies** | `IBasePolicy<T>`, `BasePolicy<T>` — `RequireNotNull`, `RequireNotNullNorEmpty`, `RequireNullOrEmpty`, `RequireTrue` / `RequireFalse`, etc. |
| **Repositories** | `IBaseRepository` — `SaveChangesAsync`, `ExecuteInTransactionAsync`. `IAggregateRepository<TEntity,TId>` — async reads/writes, includes, attach helpers. |
| **Transactions** | `ITransactionContext` — cooperative rollback signalling for unit-of-work implementations. |
| **Exceptions** | `DomainException`, `PolicyViolationException`, `EntityNotFoundException`, `ConcurrencyException`, `InvalidEntityStateException` with `ExceptionType`. |
| **Value objects / DTO markers** | `BaseValueObject`, `BaseRequest`, `BaseResponse`, `PagedRequest`, `PagedResponse<T>`, `BaseData`. |

---

## API documentation

The **public API** is documented with **XML Doc Comments** (`///` summaries, parameters, type parameters, return values, and documented exceptions where relevant). The project enables **`GenerateDocumentationFile`**, so each NuGet release ships **`Odex.AspNetCore.Clarc.Domain.xml`** next to the assembly. That file powers:

- **IDE IntelliSense** (Visual Studio, Rider, VS Code with C# Dev Kit)
- **NuGet.org** API reference and tooltips for consumers

If you extend or change public types, keep documentation in sync; see **[CONTRIBUTING.md](CONTRIBUTING.md#xml-documentation-public-api)**.

---

## Examples

### Aggregate and policy

```csharp
public class Product : StatefulAggregate<Guid>
{
    public string Name { get; private set; } = string.Empty;

    public void Rename(string newName)
    {
        var policy = new BasePolicy<string>();
        policy.RequireNotNullNorEmpty(newName);

        Name = newName;
        MarkModified();
        AddEvent(new ProductRenamedEvent(Id, newName));
    }
}
```

Define `ProductRenamedEvent` as a type implementing `IAggregateEvent` (or inheriting `AggregateEvent` with extra payload properties).

### Specification

```csharp
public sealed class ActiveUsersSpec : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
        => u => u.IsActive;
}

// Compose and pass the expression to your repository implementation
var spec = new ActiveUsersSpec().And(new UserNameContainsSpec("alex"));
await repository.ListAsync(spec.ToExpression(), cancellationToken);
```

### Application service with `AggregateService`

`AggregateService` depends on `IBaseRepository`. Your aggregate repository implementation should implement both `IAggregateRepository<T, TId>` and the same unit-of-work boundary as `IBaseRepository`.

```csharp
public sealed class ProductService(IAggregateRepository<Product, Guid> products)
    : AggregateService(products)
{
    public async Task<Product> CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var product = new Product(name);
        await products.AddAsync(product, cancellationToken);
        await SaveRepositoryChangesAsync(cancellationToken);
        return product;
    }
}
```

### Transaction boundary

```csharp
await ExecuteInRepositoryTransactionAsync(async () =>
{
    await repositoryA.AddAsync(entity, cancellationToken);
    await repositoryB.UpdateAsync(other, cancellationToken);
    return true;
}, cancellationToken);
```

Commit vs rollback semantics are defined by **your** `IBaseRepository` / infrastructure implementation.

### Composition root (dependency injection)

Register your implementations where the application starts (ASP.NET Core minimal API host, generic host, test fixture, etc.):

```csharp
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
```

---

## Namespaces

| Namespace | Role |
|-----------|------|
| `Odex.AspNetCore.Clarc.Domain.Aggregates` | `BaseAggregate<>`, `StatefulAggregate<>` |
| `Odex.AspNetCore.Clarc.Domain.Constants` | `ExceptionType` |
| `Odex.AspNetCore.Clarc.Domain.Contexts` | `ITransactionContext` |
| `Odex.AspNetCore.Clarc.Domain.DTOs` | `BaseData` marker |
| `Odex.AspNetCore.Clarc.Domain.Events` | `IAggregateEvent`, `AggregateEvent` |
| `Odex.AspNetCore.Clarc.Domain.Exceptions` | Domain exception hierarchy |
| `Odex.AspNetCore.Clarc.Domain.Policies` | `IBasePolicy<>`, `BasePolicy<>` |
| `Odex.AspNetCore.Clarc.Domain.Repositories` | `IBaseRepository`, `IAggregateRepository<,>` |
| `Odex.AspNetCore.Clarc.Domain.Services` | `IBaseService`, `BaseService`, `IAggregateService`, `AggregateService` |
| `Odex.AspNetCore.Clarc.Domain.Specifications` | `Specification<>`, combinators, `ReplaceParameterVisitor` |
| `Odex.AspNetCore.Clarc.Domain.ValueObjects` | `BaseValueObject`; `Requests` / `Responses` (e.g. `PagedRequest`, `PagedResponse<>`) |

---

## Upgrading

See the **[changelog](https://github.com/o-shabi/odex-clarc-domain-aspnetcore/blob/main/CHANGELOG.md)** for version history and **breaking changes**. A copy also lives at the repository root as `CHANGELOG.md` for forks and offline docs.

**0.1.x → 0.2.0 (summary):**

- `DeleteByAccessIdAsync` was removed from `IAggregateRepository<,>`.
- Read methods that accept `includeBuilder` gained an optional trailing `CancellationToken`.

---

## Contributing

See **[CONTRIBUTING.md](CONTRIBUTING.md)** for setup, pull-request expectations, and versioning notes.

---

## Community

- **[Code of Conduct](CODE_OF_CONDUCT.md)** — expected behavior in issues and pull requests.
- **[Security policy](SECURITY.md)** — how to report vulnerabilities responsibly.

---

## License

This project is released under the [MIT License](LICENSE).

Copyright (c) Asen O'Shabi.

---

## Links

| Resource | URL |
|----------|-----|
| NuGet Gallery | https://www.nuget.org/packages/Odex.AspNetCore.Clarc.Domain |
| Changelog | https://github.com/o-shabi/odex-clarc-domain-aspnetcore/blob/main/CHANGELOG.md |
| Contributing | [CONTRIBUTING.md](CONTRIBUTING.md) (includes [XML documentation for contributors](CONTRIBUTING.md#xml-documentation-public-api)) |
| Security | [SECURITY.md](SECURITY.md) |
| Source / issues | https://github.com/o-shabi/odex-clarc-domain-aspnetcore |
