# Package Publishing Workflow

## What You'll Learn

- Publishing best practices and checklist
- Version management and release strategies
- Package pre-publishing validation
- Publishing automation with CI/CD

## Publishing Checklist

```
Before Publishing:
─────────────────────────────────────────────
□ Package name available on npm
□ package.json complete (name, version, description, license)
□ README.md with usage examples
□ LICENSE file included
□ "files" field controls published content
□ No secrets or credentials in package
□ Tests pass
□ Build succeeds
□ npm pack --dry-run shows correct files
□ Version follows semver
```

## Publishing Commands

```bash
# Check what would be published
npm pack --dry-run

# Create tarball (test locally)
npm pack
# Creates: my-package-1.0.0.tgz

# Publish to npm
npm publish

# Publish scoped package publicly
npm publish --access public

# Publish with tag
npm publish --tag beta
npm publish --tag next

# Publish with build provenance (CI)
npm publish --provenance
```

## Release Workflow

```bash
# 1. Ensure tests pass
npm test

# 2. Bump version
npm version patch   # 1.0.0 → 1.0.1
npm version minor   # 1.0.1 → 1.1.0
npm version major   # 1.1.0 → 2.0.0

# 3. Push tags
git push && git push --tags

# 4. Publish
npm publish
```

## Automated Publishing with GitHub Actions

```yaml
name: Publish to npm
on:
  release:
    types: [created]

jobs:
  publish:
    runs-on: ubuntu-latest
    permissions:
      contents: read
      id-token: write
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: 22
          registry-url: 'https://registry.npmjs.org'
      - run: npm ci
      - run: npm test
      - run: npm publish --provenance
        env:
          NODE_AUTH_TOKEN: ${{ secrets.NPM_TOKEN }}
```

## Best Practices Checklist

- [ ] Run `npm pack --dry-run` before publish
- [ ] Use `--provenance` for supply chain security
- [ ] Use tags for prerelease versions
- [ ] Automate publishing with CI/CD
- [ ] Document release process in README

## Cross-References

- See [Package Development](../09-package-development/01-creating-packages.md) for development
- See [Semantic Versioning](../03-semantic-versioning/01-version-ranges.md) for versioning
- See [Security](../11-package-security/01-supply-chain.md) for security

## Next Steps

Continue to [Publishing Best Practices](./02-publishing-best-practices.md) for guidelines.
