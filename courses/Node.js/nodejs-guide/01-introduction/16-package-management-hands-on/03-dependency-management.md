# Dependency Management Strategies

## What You'll Learn

- Managing project dependencies effectively
- Security auditing and vulnerability management
- Monorepo dependency management
- Dependency update strategies

## Dependency Security

### npm audit

```bash
# Check for vulnerabilities
npm audit

# Auto-fix vulnerabilities
npm audit fix

# Force fix (may introduce breaking changes)
npm audit fix --force

# Generate audit report
npm audit --json > audit-report.json

# Only show high/critical vulnerabilities
npm audit --audit-level=high
```

### Automated Security

```json
// .npmrc — Security configuration
audit-level=high
save-exact=true
```

```yaml
# .github/workflows/security.yml
name: Security Audit
on: [push, pull_request]
jobs:
  audit:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: 22 }
      - run: npm ci
      - run: npm audit --audit-level=high
```

## Dependency Update Strategy

### Update Workflow

```bash
# Check outdated packages
npm outdated

# Update to latest compatible versions
npm update

# Update specific package
npm update express

# Interactive update (using npm-check-updates)
npx npm-check-updates -i
```

### Automated Updates with Renovate/Dependabot

```json
// renovate.json
{
  "extends": ["config:base"],
  "packageRules": [
    {
      "matchUpdateTypes": ["minor", "patch"],
      "automerge": true
    },
    {
      "matchUpdateTypes": ["major"],
      "automerge": false,
      "labels": ["major-update"]
    }
  ],
  "schedule": ["before 5am on monday"]
}
```

## Monorepo with Workspaces

### npm Workspaces

```json
// Root package.json
{
  "name": "my-monorepo",
  "private": true,
  "workspaces": [
    "packages/*",
    "apps/*"
  ]
}
```

```bash
# Install dependencies for all workspaces
npm install

# Add dependency to specific workspace
npm install express --workspace=packages/api

# Run script in specific workspace
npm run test --workspace=packages/utils

# Run script in all workspaces
npm run test --workspaces

# Add workspace as dependency
npm install @myrepo/utils --workspace=packages/api
```

### Directory Structure

```
my-monorepo/
├── package.json
├── packages/
│   ├── utils/
│   │   ├── package.json  (name: @myrepo/utils)
│   │   └── src/
│   └── types/
│       ├── package.json  (name: @myrepo/types)
│       └── src/
├── apps/
│   ├── api/
│   │   ├── package.json  (depends on @myrepo/utils)
│   │   └── src/
│   └── web/
│       ├── package.json  (depends on @myrepo/utils)
│       └── src/
```

## .npmrc Configuration

```ini
# .npmrc — Project-level configuration

# Use exact versions (no ^ or ~)
save-exact=true

# Set audit level
audit-level=high

# Registry (for private packages)
registry=https://registry.npmjs.org/

# Scoped registry
@myorg:registry=https://npm.myorg.com/

# Authentication token (CI/CD)
//registry.npmjs.org/:_authToken=${NPM_TOKEN}

# Engine strict
engine-strict=true
```

## Dependency Analysis

```bash
# Show dependency tree
npm list

# Show specific package tree
npm list express

# Show why a package is installed
npm explain express

# Find duplicate dependencies
npm dedupe

# Check package size
npx package-size express
```

## Best Practices Checklist

- [ ] Run `npm audit` in CI/CD pipeline
- [ ] Use `npm ci` for reproducible builds
- [ ] Set up automated dependency updates (Renovate/Dependabot)
- [ ] Use npm workspaces for monorepos
- [ ] Pin exact versions for critical dependencies
- [ ] Review major version updates manually
- [ ] Keep dependencies to minimum (reduce attack surface)
- [ ] Use `.npmrc` for project-level configuration

## Cross-References

- See [npm Hands-On](./01-npm-hands-on.md) for npm commands
- See [Creating and Publishing](./02-creating-publishing.md) for package creation
- See [Security Best Practices](../21-security-modern/02-auth-rate-limiting.md) for security
- See [CI/CD Integration](../../../26-cicd-github-actions/) for automation

## Next Steps

Continue to [Performance Deep Dive](../09-performance-deep-dive/02-cpu-scalability.md) for optimization strategies.
