# 🧱 Odex.AspNetCore.Clarc.Domain

[![NuGet Version](https://img.shields.io/nuget/v/Odex.AspNetCore.Clarc.Domain)](https://www.nuget.org/packages/Odex.AspNetCore.Clarc.Domain)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Odex.AspNetCore.Clarc.Domain)](https://www.nuget.org/packages/Odex.AspNetCore.Clarc.Domain)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**A robust DDD foundation library for ASP.NET Core applications**  
*Build clean, maintainable domain layers with aggregates, specifications, policies, and CQRS‑ready building blocks.*

---

## 📦 Overview

`Odex.AspNetCore.Clarc.Domain` provides a set of base classes, interfaces, and utilities that implement common *
*Domain‑Driven Design** (DDD) patterns. It is designed to reduce boilerplate code while encouraging consistency,
testability, and separation of concerns.

The library focuses on:

- **Aggregates & Entities** – Base classes for root aggregates and stateful entities.
- **Domain Events** – Lightweight event recording and dispatching support.
- **Specifications** – Reusable, composable query logic.
- **Policies** – Guard clauses and invariant validation.
- **Repository Abstractions** – Async CRUD operations with transactions.
- **Transactional Context** – Explicit rollback and commit control.
- **Value Objects** – Immutable data containers.

---

## 🚀 Getting Started

### Prerequisites

- .NET 6.0 or later (support for .NET Standard 2.1)
- ASP.NET Core (for dependency injection integration)
- Entity Framework Core (for repository implementations – not included)

### Installation

The library is published as a NuGet package (example – replace with actual feed):

```bash
dotnet add package Odex.AspNetCore.Clarc.Domain
```

Or use the Package Manager Console:

```bash
Install-Package Odex.AspNetCore.Clarc.Domain
```

---

## ✨ Features

| Feature                  | Description                                                                                       |
|--------------------------|---------------------------------------------------------------------------------------------------|
| 🧩 **BaseAggregate**     | `Id`, `CreatedAt`, `LastModifiedAt`, event collection (`ListEvents()`, `AddEvent()`).             |
| 🔁 **StatefulAggregate** | Adds `IsActive` / `IsSoftDeleted` with `Activate()`, `Deactivate()`, `SoftDelete()`, `Restore()`. |
| 📜 **Domain Events**     | `IDomainEvent` – auto‑generated GUID and timestamp.                                               |
| ✅ **Policies**           | `RequireNotNull`, `RequireNotNullNorEmpty`, `RequireTrue/False`, etc.                             |
| 📐 **Specifications**    | `And`, `Or`, `Not` combinators – works with EF Core.                                              |
| 🗄️ **Repository**       | `IAggregateRepository<TEntity, TId>` with `Add`, `Update`, `Delete`, `FindById`, `ListAsync`.     |
| 🔁 **Transactions**      | `ITransactionContext` + `ExecuteInTransactionAsync` – opt‑in rollback.                            |
| ⚠️ **Domain Exceptions** | `DomainException` with `ExceptionType` enum (Concurrency, NotFound, etc.).                        |
| 📦 **Value Objects**     | `BaseValueObject` + typed request/response records (`PagedRequest`, `PagedResponse`).             |

---

## 🏗️ Core Components

### 1. Aggregates

```csharp
public class MyAggregate : StatefulAggregate<Guid>
{
    public string Name { get; private set; }

    public void UpdateName(string newName)
    {
        // Use a policy to guard invariants
        var policy = new BasePolicy<string>();
        policy.RequireNotNullNorEmpty(newName);
        
        Name = newName;
        MarkModified();   // Updates LastModifiedAt
        AddEvent(new NameChangedEvent(Id, newName));
    }
}
```

### 2. Policies (Guard Clauses)

```csharp
var policy = new BasePolicy<MyEntity>();
policy.RequireNotNull(entity);
policy.RequireTrue(entity.IsActive);
policy.RequireNotNullNorEmpty(entity.Code);
```

### 3. Specifications

```csharp
public class ActiveUsersSpec : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression() 
        => u => u.IsActive;
}

// Usage
var spec = new ActiveUsersSpec().And(new UserNameContainsSpec("john"));
var users = await repository.ListAsync(spec.ToExpression());
```

### 4. Repository Usage

```csharp
public class ProductService : BaseService
{
    private readonly IAggregateRepository<Product, Guid> _productRepo;

    public async Task<Product> CreateProduct(string name)
    {
        var product = new Product(name);
        await _productRepo.AddAsync(product);
        await SaveRepositoryChangesAsync();
        return product;
    }
}
```

### 5. Transactions

```csharp
await ExecuteInRepositoryTransactionAsync(async () =>
{
    await repo1.AddAsync(entityA);
    await repo2.UpdateAsync(entityB);
    return true;   // commits on success
});
```

---

### Basic Configuration (DI)

Register your repository and services in the `Program.cs` or `Infrastructure` layer:

```csharp
builder.Services.AddScoped<IMyRepository, MyRepository>();
builder.Services.AddScoped<IMyService, MyService>();
```

---

## 📂 Namespace Map

| Namespace                                     | Purpose                                                                                                                                                                   |
|-----------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Odex.AspNetCore.Clarc.Domain.Aggregates`     | `BaseAggregate`, `StatefulAggregate`                                                                                                                                      |
| `Odex.AspNetCore.Clarc.Domain.Constants`      | `ExceptionType` enum                                                                                                                                                      |
| `Odex.AspNetCore.Clarc.Domain.Contexts`       | `ITransactionContext`                                                                                                                                                     |
| `Odex.AspNetCore.Clarc.Domain.DTOs`           | `BaseData` marker                                                                                                                                                         |
| `Odex.AspNetCore.Clarc.Domain.Events`         | `IAggregateEvent`, `AggregateEvent`                                                                                                                                       |
| `Odex.AspNetCore.Clarc.Domain.Exceptions`     | `DomainException` (Base exception class), `ConcurrencyException`, `EntityNotFoundException`, `InvalidEntityStateException`, `PolicyViolationException`                    |
| `Odex.AspNetCore.Clarc.Domain.Policies`       | `IBasePolicy<T>`, `BasePolicy<T>`                                                                                                                                         |
| `Odex.AspNetCore.Clarc.Domain.Repositories`   | `IBaseRepository`, `IAggregateRepository`                                                                                                                                 |
| `Odex.AspNetCore.Clarc.Domain.Services`       | `IBaseService`, `BaseService`, `IAggregateService` , `AggregateService`                                                                                                   |
| `Odex.AspNetCore.Clarc.Domain.Specifications` | `Specification` (Base specification class), `AndSpecification`, `NotSpecification`, `OrSpecification`, `ReplaceParameterVisitor`                                          |
| `Odex.AspNetCore.Clarc.Domain.ValueObjects`   | `BaseValueObject` (Base value object class. Mostly used as marker), `Requests.BaseRequests`, `Requests.PagedRequest`, `Responses.BaseResponse`, `Responses.PagedResponse` |

---

## 🧪 Example: Soft‑Delete & Restore

```csharp
public class Customer : StatefulAggregate<int>
{
    public string Email { get; private set; }

    public void DeleteCustomer()
    {
        SoftDelete();   // sets IsSoftDeleted = true, SoftDeletedAt = now
        AddEvent(new CustomerSoftDeletedEvent(Id));
    }

    public void Reinstate()
    {
        Restore();      // sets IsSoftDeleted = false, RestoredAt = now
        AddEvent(new CustomerRestoredEvent(Id));
    }
}
```

---

## 🤝 Contributing

Contributions are welcome! Please follow the standard GitHub flow:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Make sure to include tests for new functionality.

---

## 📄 License

This project is licensed under the **MIT License** – see the [LICENSE](LICENSE) file for details.

---

*Built with ❤️ for clean DDD architectures on ASP.NET Core*
