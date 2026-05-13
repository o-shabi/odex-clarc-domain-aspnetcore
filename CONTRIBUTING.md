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

## CI/CD (GitHub Actions)

This repository uses two workflows under [`.github/workflows/`](.github/workflows/).

### CI — `ci.yml` (continuous integration)

**When it runs:** every **push** and **pull request** targeting `main`.

**What it does:**

1. **`dotnet restore`** on the solution  
2. **`dotnet build`** — `Release`, `ContinuousIntegrationBuild=true`, **`TreatWarningsAsErrors=true`** (same bar as a strict local build)  
3. **`dotnet test`** — same configuration, `--no-build`  
4. **`dotnet pack`** — verifies the NuGet layout; uploads **`*.nupkg`** and **`*.snupkg`** as workflow artifacts (download from the Actions run)

**Concurrency:** new runs for the same PR/branch cancel older in-progress runs so you do not queue redundant work.

### CD — `release.yml` (continuous deployment to NuGet)

**When it runs:**

| Trigger | How |
|--------|-----|
| **Git tag** | Push a tag matching `v1.2.3` (leading `v`, then SemVer). Example: `git tag v0.3.0 && git push origin v0.3.0` |
| **Manual** | GitHub → **Actions** → **Release** → **Run workflow** → enter version **`0.3.0`** (no `v` prefix) |

The workflow sets MSBuild **`Version`** and **`PackageVersion`** from that value, then build → test → pack → **`dotnet nuget push`** to **https://api.nuget.org/v3/index.json** with `--skip-duplicate`.

**One-time setup**

1. On [NuGet.org](https://www.nuget.org/), create an **API key** with permission to **push** this package ID.  
2. In the GitHub repo: **Settings → Secrets and variables → Actions → New repository secret**  
   - Name: **`NUGET_API_KEY`**  
   - Value: the key from NuGet  

Forks will not have your secret; the push step fails fast if the secret is missing.

**Optional (later):** add a GitHub **Environment** named `nuget` with required reviewers, and set `environment: nuget` on the publish job in `release.yml` for a manual approval gate before push.

## Code of conduct

Participation is governed by the [Code of Conduct](CODE_OF_CONDUCT.md). Be respectful and constructive.

## Licensing

By contributing, you agree that your contributions are licensed under the same terms as the project ([MIT License](LICENSE)).
