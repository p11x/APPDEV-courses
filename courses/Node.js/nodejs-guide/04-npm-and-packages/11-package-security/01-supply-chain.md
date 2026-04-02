# Package Security: Supply Chain Protection

## What You'll Learn

- Supply chain security practices
- Package integrity verification
- Vulnerability scanning
- Dependency policy enforcement

## Supply Chain Security

```bash
# Verify package integrity
npm audit signatures

# Check for known vulnerabilities
npm audit

# Auto-fix vulnerabilities
npm audit fix

# Force fix (may break things)
npm audit fix --force

# Use npm provenance (for published packages)
npm publish --provenance
```

## Dependency Policy

```json
// .npmpolicy.json
{
    "allowedPackages": [
        "express",
        "lodash",
        "zod"
    ],
    "blockedPackages": [
        "event-stream"
    ],
    "allowedRegistries": [
        "https://registry.npmjs.org/"
    ],
    "requireSignatures": true
}
```

## Security Scanning in CI/CD

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
      - name: Snyk scan
        uses: snyk/actions/node@master
        env:
          SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}
```

## Best Practices Checklist

- [ ] Run `npm audit` in CI/CD
- [ ] Enable Dependabot or Renovate
- [ ] Use `--provenance` when publishing
- [ ] Review new dependencies before adding
- [ ] Keep dependencies minimal

## Cross-References

- See [Dependency Management](../04-dependency-management/01-tree-analysis.md) for dependencies
- See [Publishing](../06-package-publishing/01-publishing-workflow.md) for publishing
- See [Enterprise](../14-enterprise-management/01-enterprise-registry.md) for enterprise

## Next Steps

Continue to [Package Optimization](../12-package-optimization/01-tree-shaking.md) for optimization.
