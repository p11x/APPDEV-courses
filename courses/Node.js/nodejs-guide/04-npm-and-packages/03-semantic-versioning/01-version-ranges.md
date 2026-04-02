# Semantic Versioning: Version Ranges and Patterns

## What You'll Learn

- Semantic versioning rules (major.minor.patch)
- Version range syntax and patterns
- Compatibility and breaking changes
- Version resolution behavior

## Semantic Versioning Rules

```
Version: MAJOR.MINOR.PATCH
─────────────────────────────────────────────
MAJOR  │ Breaking API changes (incompatible)
MINOR  │ New features (backward compatible)
PATCH  │ Bug fixes (backward compatible)

Examples:
1.0.0  │ Initial release
1.0.1  │ Bug fix
1.1.0  │ New feature
2.0.0  │ Breaking change
```

## Version Range Syntax

```json
{
    "dependencies": {
        "express": "4.21.0",       // Exact version
        "lodash": "^4.17.0",       // Caret: >=4.17.0 <5.0.0
        "axios": "~1.6.0",         // Tilde: >=1.6.0 <1.7.0
        "cors": "*",               // Any version
        "helmet": ">=7.0.0",       // Greater than or equal
        "dotenv": ">=16.0.0 <17.0.0", // Range
        "debug": "4.x",            // Major 4, any minor/patch
        "chalk": "latest"          // Latest published
    }
}
```

### Range Examples

```
Caret (^): Allow minor + patch updates
─────────────────────────────────────────────
^1.2.3  →  >=1.2.3  <2.0.0
^0.2.3  →  >=0.2.3  <0.3.0  (special: 0.x)
^0.0.3  →  >=0.0.3  <0.0.4  (special: 0.0.x)

Tilde (~): Allow patch updates only
─────────────────────────────────────────────
~1.2.3  →  >=1.2.3  <1.3.0
~1.2    →  >=1.2.0  <1.3.0
~1      →  >=1.0.0  <2.0.0

Wildcards:
─────────────────────────────────────────────
*       →  Any version
1.*     →  >=1.0.0 <2.0.0
1.2.*   →  >=1.2.0 <1.3.0

Hyphen ranges:
─────────────────────────────────────────────
1.2.3 - 2.3.4  →  >=1.2.3 <=2.3.4
1.2.3 - 2.3    →  >=1.2.3 <2.4.0
1.2 - 2.3.4    →  >=1.2.0 <=2.3.4

X-Ranges:
─────────────────────────────────────────────
*       →  >=0.0.0
1.x     →  >=1.0.0 <2.0.0
1.2.x   →  >=1.2.0 <1.3.0
```

## Breaking Changes Guide

```
When to bump MAJOR version:
─────────────────────────────────────────────
✗ Remove a function or export
✗ Change function signature (add required param)
✗ Change return type
✗ Change default behavior
✗ Drop Node.js version support

When to bump MINOR version:
─────────────────────────────────────────────
✓ Add new function or export
✓ Add optional parameter
✓ Add new feature
✓ Deprecate existing API (don't remove)

When to bump PATCH version:
─────────────────────────────────────────────
✓ Fix a bug
✓ Improve performance
✓ Update documentation
✓ Internal refactoring
```

## Best Practices Checklist

- [ ] Use `^` for dependencies (allows security patches)
- [ ] Use exact versions for critical dependencies
- [ ] Document breaking changes in CHANGELOG
- [ ] Test against version ranges in CI
- [ ] Use `npm outdated` to check for updates

## Cross-References

- See [Compatibility Testing](./02-compatibility-testing.md) for testing strategies
- See [Dependency Management](../04-dependency-management/01-tree-analysis.md) for dependencies
- See [Package.json](../02-package-json/01-fields-reference.md) for package.json

## Next Steps

Continue to [Compatibility Testing](./02-compatibility-testing.md) for testing strategies.
