# Dependency Conflict Resolution and Categorization

## What You'll Learn

- Peer dependency conflict resolution
- Dependency categorization strategies
- Duplicate dependency management
- Version pinning strategies

## Peer Dependency Resolution

```bash
# Strict (default in npm 7+)
npm install  # Fails on peer dep conflicts

# Legacy (npm 6 behavior)
npm install --legacy-peer-deps

# Force
npm install --force
```

```json
// package.json — Declare peer dependencies
{
    "peerDependencies": {
        "react": ">=18.0.0",
        "react-dom": ">=18.0.0"
    },
    "peerDependenciesMeta": {
        "react-dom": {
            "optional": true
        }
    }
}
```

## Dependency Categorization

```json
{
    "dependencies": {
        "express": "^4.21.0",
        "pg": "^8.12.0"
    },
    "devDependencies": {
        "eslint": "^9.0.0",
        "vitest": "^3.0.0",
        "typescript": "^5.4.0",
        "prettier": "^3.2.0"
    },
    "peerDependencies": {
        "react": ">=18.0.0"
    },
    "optionalDependencies": {
        "fsevents": "^2.3.0"
    }
}
```

## Version Pinning Strategy

```
Version Pinning Decision Matrix:
─────────────────────────────────────────────
Use ^ (caret) for:
├── Application dependencies
├── Libraries with good semver adherence
└── Security-sensitive packages

Use ~ (tilde) for:
├── Packages with frequent breaking minors
└── Critical business logic dependencies

Use exact version for:
├── Core framework versions
├── Security-critical packages
└── Packages with poor semver history
```

## Best Practices Checklist

- [ ] Use `peerDependencies` for framework dependencies
- [ ] Categorize dependencies correctly
- [ ] Use `^` for most dependencies
- [ ] Use exact versions for critical packages
- [ ] Resolve peer dependency conflicts explicitly

## Cross-References

- See [Tree Analysis](./01-tree-analysis.md) for dependency analysis
- See [Version Ranges](../03-semantic-versioning/01-version-ranges.md) for versioning
- See [Security](../11-package-security/01-supply-chain.md) for security

## Next Steps

Continue to [Private Registry](../05-private-registry/01-setup-guide.md) for private registries.
