# Dependency Tree Analysis and Management

## What You'll Learn

- Dependency tree visualization
- Conflict resolution strategies
- Lockfile management
- Dependency auditing

## Dependency Tree Analysis

```bash
# Show dependency tree
npm ls

# Show specific package
npm ls express

# Show all versions of a package
npm ls lodash

# Show why a package is installed
npm explain lodash

# Show only top-level
npm ls --depth=0

# Show production dependencies only
npm ls --prod

# Show dev dependencies only
npm ls --dev

# Show outdated packages
npm outdated

# JSON output for automation
npm ls --json
```

## Lockfile Management

```json
// package-lock.json structure
{
    "name": "my-app",
    "version": "1.0.0",
    "lockfileVersion": 3,
    "requires": true,
    "packages": {
        "": {
            "name": "my-app",
            "version": "1.0.0",
            "dependencies": { "express": "^4.21.0" }
        },
        "node_modules/express": {
            "version": "4.21.0",
            "resolved": "https://registry.npmjs.org/express/-/express-4.21.0.tgz",
            "integrity": "sha512-...",
            "dependencies": { "accepts": "~1.3.8" }
        }
    }
}
```

```bash
# Lockfile best practices:
# 1. Always commit package-lock.json
# 2. Use npm ci in CI/CD (strict install from lock)
# 3. Don't manually edit package-lock.json
# 4. Regenerate if corrupted:
rm package-lock.json && npm install
```

## Security Auditing

```bash
# Check for vulnerabilities
npm audit

# Auto-fix vulnerabilities
npm audit fix

# Force fix (may introduce breaking changes)
npm audit fix --force

# Only show high/critical
npm audit --audit-level=high

# JSON output
npm audit --json > audit-report.json

# Verify package signatures
npm audit signatures
```

## Dependency Conflict Resolution

```json
{
    "overrides": {
        "semver": "^7.5.0",
        "lodash": "4.17.21"
    },
    "resolutions": {
        "**/lodash": "4.17.21"
    }
}
```

```bash
# Force resolution of peer dependencies
npm install --legacy-peer-deps

# Force reinstall
npm install --force
```

## Best Practices Checklist

- [ ] Commit package-lock.json to version control
- [ ] Use `npm ci` in CI/CD pipelines
- [ ] Run `npm audit` regularly
- [ ] Use `overrides` for forced resolutions
- [ ] Keep dependencies up to date

## Cross-References

- See [Security](../11-package-security/01-supply-chain.md) for security practices
- See [Version Ranges](../03-semantic-versioning/01-version-ranges.md) for versioning
- See [Lockfile Management](./02-lockfile-management.md) for lockfiles

## Next Steps

Continue to [Lockfile Management](./02-lockfile-management.md) for lockfile details.
