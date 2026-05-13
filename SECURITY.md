# Security policy

## Supported versions

| Version | Supported |
|---------|-----------|
| 0.2.x   | Yes |
| &lt; 0.2 | Best effort (upgrade recommended) |

## Reporting a vulnerability

Please **do not** open a public GitHub issue for security vulnerabilities.

Instead, report details privately so they can be addressed before wider disclosure:

1. Open a **[GitHub Security Advisory](https://github.com/o-shabi/odex-clarc-domain-aspnetcore/security/advisories/new)** for this repository (preferred if the feature is enabled), or  
2. Contact the maintainers through a private channel if you use another process.

Include:

- Affected package version(s) and .NET runtime
- Steps to reproduce or a minimal proof of concept
- Potential impact (confidentiality, integrity, availability)

You should receive an initial response within a reasonable time frame. We may coordinate a fix, release, and public advisory following responsible disclosure.

## Scope

This repository ships a **.NET class library** consumed as a dependency. Vulnerabilities in **your** application stack (host, database, transitive packages) are outside this project’s direct control; still, reports that affect **this** package’s code or published artifacts are in scope.
