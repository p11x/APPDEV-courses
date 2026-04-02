# Lockfile Management and Dependency Updates

## What You'll Learn

- Lockfile structure and behavior
- Automated dependency updates
- Dependency pruning strategies
- Lockfile conflict resolution

## Lockfile Behavior

```bash
# npm install respects lockfile
npm install  # Uses exact versions from lockfile

# npm ci is stricter
npm ci       # Fails if package.json ≠ lockfile
             # Deletes node_modules first
             # Recommended for CI/CD

# Update lockfile after package.json changes
npm install  # Updates lockfile

# Regenerate lockfile
rm package-lock.json
npm install
```

## Automated Dependency Updates

```json
// renovate.json — Renovate bot configuration
{
    "extends": ["config:base"],
    "packageRules": [
        {
            "matchUpdateTypes": ["minor", "patch"],
            "automerge": true
        },
        {
            "matchUpdateTypes": ["major"],
            "labels": ["major-update"],
            "automerge": false
        },
        {
            "matchDepTypes": ["devDependencies"],
            "automerge": true
        }
    ],
    "schedule": ["before 5am on monday"],
    "timezone": "America/New_York"
}
```

```yaml
# .github/dependabot.yml
version: 2
updates:
  - package-ecosystem: "npm"
    directory: "/"
    schedule:
      interval: "weekly"
    open-pull-requests-limit: 10
    reviewers:
      - "team-lead"
    labels:
      - "dependencies"
```

## Dependency Pruning

```bash
# Check for unused dependencies
npx depcheck

# Remove unused packages
npm uninstall <unused-package>

# Check for duplicate dependencies
npm dedupe

# Clean install (removes extraneous packages)
npm ci
```

## Best Practices Checklist

- [ ] Use `npm ci` in CI/CD
- [ ] Set up Renovate or Dependabot
- [ ] Review major updates manually
- [ ] Run `npm dedupe` periodically
- [ ] Use `depcheck` to find unused deps

## Cross-References

- See [Tree Analysis](./01-tree-analysis.md) for dependency analysis
- See [Security](../11-package-security/01-supply-chain.md) for security
- See [Ecosystem](../15-ecosystem-integration/01-github-actions.md) for CI/CD

## Next Steps

Continue to [Private Registry](../05-private-registry/01-setup-guide.md) for private registries.
