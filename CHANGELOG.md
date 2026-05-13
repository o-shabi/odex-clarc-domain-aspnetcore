# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- GitHub Actions **CI** workflow (build, test, pack verification, artifact upload).
- **Dependabot** configuration for NuGet and GitHub Actions.
- Community docs: **CONTRIBUTING.md**, **CODE_OF_CONDUCT.md** (Contributor Covenant 2.1), **SECURITY.md**.
- Issue templates (bug report, feature request) and **pull request template**.
- **Directory.Build.props** and **.editorconfig** for consistent SDK and C# style defaults.
- **Microsoft.SourceLink.GitHub** and repository metadata on the package (`PublishRepositoryUrl`, `RepositoryType`, `PackageReleaseNotes`, `Copyright`, `Title`).

### Changed

- **API documentation:** Full **XML Doc Comment** coverage for all **public** types and members in `Odex.AspNetCore.Clarc.Domain` (standard `///` blocks: summaries plus `param`, `typeparam`, `returns`, and `exception` where applicable). The shipped **`Odex.AspNetCore.Clarc.Domain.xml`** now matches the public surface, clearing **CS1591** when strict documentation warnings are enabled.

## [0.2.0] - 2026-05-12

### Added

- Optional `CancellationToken` parameter on `IAggregateRepository` read overloads that accept an `includeBuilder` (`FindByIdAsync`, `FindByIdsAsync`, `FirstOrDefaultAsync`, `ListAsync`).
- Unit tests for `BasePolicy` guard behavior (`Odex.AspNetCore.Clarc.Domain.Tests`).

### Changed

- **Breaking:** `IAggregateRepository<TEntity, TId>` no longer declares `DeleteByAccessIdAsync`. Use `DeleteAsync` with a predicate, a dedicated repository interface, or an infrastructure-level extension for access-id semantics.
- `BaseService` now exposes a `protected IBaseRepository Repository { get; }` for derived services; `AggregateService` delegates through it (no duplicate repository field).
- Package description and documentation emphasize a **framework-agnostic** domain layer; ASP.NET Core is not a dependency of this assembly.
- `RequireNotNull` / `RequireNotNull<TU>` failure messages use a stable, null-safe description.
- `PackageTags` updated to reflect clean architecture and CLARC (`ddd`, `clean-architecture`, `aggregates`, `specification`, `domain-events`, `clarc`).

### Fixed

- `RequireNullOrEmpty` now correctly allows `null` or empty values and throws when the value is non-empty (previously the control flow was invalid and could throw unexpectedly).
- `RequireNotNullNorEmpty<TU>` implementation now matches `IBasePolicy<T>` nullability (`TU?`).

## [0.1.2], [0.1.1], [0.1.0]

The **0.1.x** line introduced the core library surface: aggregates (`BaseAggregate`, `StatefulAggregate`), aggregate events, composable specifications, guard policies, repository and unit-of-work-style transaction abstractions, typed domain exceptions, and shared request/response primitives (including pagination helpers).

---

[Unreleased]: https://github.com/o-shabi/odex-clarc-domain-aspnetcore/compare/v0.2.0...HEAD
[0.2.0]: https://github.com/o-shabi/odex-clarc-domain-aspnetcore/compare/v0.1.2...v0.2.0
