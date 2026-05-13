# Contributing

Thank you for helping improve **Odex.AspNetCore.Clarc.Domain**. This project follows common practices for .NET libraries on GitHub and NuGet.

## Before you start

- Read [README.md](README.md) for scope: this package is **domain-layer primitives and contracts** only.
- Check [CHANGELOG.md](CHANGELOG.md) and existing [issues](https://github.com/o-shabi/odex-clarc-domain-aspnetcore/issues) to avoid duplicate work.
- For security-sensitive reports, use [SECURITY.md](SECURITY.md) instead of a public issue.

## Development setup

Requirements:

- [.NET SDK 9.0](https://dotnet.microsoft.com/download) (see [global.json](global.json) for the preferred feature band).

Commands:

```bash
git clone https://github.com/o-shabi/odex-clarc-domain-aspnetcore.git
cd odex-clarc-domain-aspnetcore
dotnet restore Odex.AspNetCore.Clarc.Domain.sln
dotnet build Odex.AspNetCore.Clarc.Domain.sln -c Release
dotnet test Odex.AspNetCore.Clarc.Domain.sln -c Release
```

To verify the NuGet layout locally:

```bash
dotnet pack Odex.AspNetCore.Clarc.Domain/Odex.AspNetCore.Clarc.Domain.csproj -c Release -o ./artifacts
```

The produced folder includes **`Odex.AspNetCore.Clarc.Domain.xml`** (API documentation) alongside the `.dll` and optional `.snupkg`.

## XML documentation (public API)

The library sets **`GenerateDocumentationFile`** to `true`. Every **public** type and member must have appropriate **`///` XML documentation** so consumers get IntelliSense and NuGet does not emit **CS1591** (missing XML comment).

When you add or change public API:

1. Add or update **`/// <summary>`** (required for discoverability).
2. Use **`<param>`**, **`<typeparam>`**, **`<returns>`**, and **`<exception cref="...">`** when they clarify contracts (especially on interfaces and throwing helpers like **`BasePolicy<T>`**).
3. Verify locally:

```bash
dotnet build Odex.AspNetCore.Clarc.Domain/Odex.AspNetCore.Clarc.Domain.csproj -warnaserror:CS1591
```

Private implementation details do not require public XML comments.

## Pull requests

1. Fork the repository and create a focused branch from `main`.
2. Keep changes scoped to a single concern when possible.
3. Match existing code style (C# conventions, nullable reference types, file layout).
4. Add or update **unit tests** for behavior changes.
5. For **public API** changes, add or update **`///` XML documentation** so `GenerateDocumentationFile` stays clean (see [XML documentation (public API)](#xml-documentation-public-api)).
6. Document **breaking API** changes in `CHANGELOG.md` under **Unreleased** (Added / Changed / Deprecated / Removed / Fixed).
7. Open a PR using the template; CI must pass.

## API and versioning

This project uses [Semantic Versioning](https://semver.org/). Breaking changes belong in a **major** bump (or in **0.x** as coordinated minor bumps per your policy). Prefer additive changes and obsolescence with a migration period when practical.

## Code of conduct

Participation is governed by the [Code of Conduct](CODE_OF_CONDUCT.md). Be respectful and constructive.

## Licensing

By contributing, you agree that your contributions are licensed under the same terms as the project ([MIT License](LICENSE)).
