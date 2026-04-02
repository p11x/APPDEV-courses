# GitHub Actions for Package Management

## What You'll Learn

- CI/CD pipeline for packages
- Automated testing and publishing
- Package documentation automation
- Package monitoring and analytics

## CI/CD Pipeline

```yaml
name: Package CI/CD
on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  release:
    types: [created]

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
      - run: npm run lint

  publish:
    needs: test
    if: github.event_name == 'release'
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
      - run: npm publish --provenance
        env:
          NODE_AUTH_TOKEN: ${{ secrets.NPM_TOKEN }}
```

## Automated Version Bumping

```yaml
name: Auto Version Bump
on:
  push:
    branches: [main]
    paths:
      - 'src/**'

jobs:
  bump:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0
      - uses: actions/setup-node@v4
        with: { node-version: 22 }
      - run: npm ci
      - name: Bump version
        run: |
          npm version patch -m "chore: release %s [skip ci]"
          git push && git push --tags
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

## Best Practices Checklist

- [ ] Test across multiple Node.js versions
- [ ] Use `npm ci` for deterministic installs
- [ ] Use `--provenance` for supply chain security
- [ ] Automate version bumping
- [ ] Cache npm dependencies in CI

## Cross-References

- See [Publishing](../06-package-publishing/01-publishing-workflow.md) for publishing
- See [Testing](../13-package-testing/01-unit-testing.md) for testing
- See [Security](../11-package-security/01-supply-chain.md) for security

## Next Steps

This completes Chapter 4 of the Node.js guide. Proceed to [Chapter 5: Express Framework](../../05-express-framework/) for web development.
