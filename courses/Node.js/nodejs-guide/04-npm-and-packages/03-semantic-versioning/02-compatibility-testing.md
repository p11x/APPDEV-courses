# Version Compatibility Testing and Migration

## What You'll Learn

- Testing packages across versions
- Automated version bumping
- Version migration strategies
- Breaking change communication

## Automated Version Bumping

```bash
# npm version command
npm version patch   # 1.0.0 → 1.0.1
npm version minor   # 1.0.1 → 1.1.0
npm version major   # 1.1.0 → 2.0.0
npm version prerelease --preid=beta  # 2.0.0 → 2.0.1-beta.0

# With git integration
npm version patch -m "Release %s"
# Creates git tag, commits changes
```

## CI Matrix Testing

```yaml
# .github/workflows/test.yml
name: Test
on: [push, pull_request]
jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        node-version: [18, 20, 22]
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ matrix.node-version }}
          cache: 'npm'
      - run: npm ci
      - run: npm test
```

## Breaking Change Communication

```markdown
# CHANGELOG.md

## [2.0.0] - 2024-01-15

### BREAKING CHANGES
- Removed deprecated `getUserById()` function
  - Use `getUser({ id })` instead
- Changed `createUser()` signature
  - Now requires `email` parameter
- Dropped Node.js 16 support

### Migration Guide
\`\`\`javascript
// Before (1.x)
const user = getUserById(123);

// After (2.x)
const user = await getUser({ id: 123 });
\`\`\`
```

## Best Practices Checklist

- [ ] Test against all supported Node.js versions
- [ ] Document all breaking changes
- [ ] Provide migration guides
- [ ] Use prerelease versions for testing
- [ ] Communicate breaking changes in advance

## Cross-References

- See [Version Ranges](./01-version-ranges.md) for version syntax
- See [Dependency Management](../04-dependency-management/01-tree-analysis.md) for dependencies
- See [CI/CD Integration](../15-ecosystem-integration/01-github-actions.md) for automation

## Next Steps

Continue to [Dependency Management](../04-dependency-management/01-tree-analysis.md) for dependencies.
