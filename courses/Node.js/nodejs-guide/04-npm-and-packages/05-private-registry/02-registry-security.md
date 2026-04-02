# Registry Security and Access Control

## What You'll Learn

- Registry authentication methods
- Access control configuration
- Package signing and verification
- Registry monitoring and backup

## Authentication Methods

```bash
# Token authentication (recommended)
npm token create --read-only
npm token create --cidr=10.0.0.0/8  # IP-restricted

# Use token in CI/CD
echo "//registry.npmjs.org/:_authToken=${NPM_TOKEN}" > ~/.npmrc
```

## Access Control

```yaml
# Verdaccio package access control
packages:
    '@myorg/private-*':
        access: $authenticated
        publish: team-leads
        unpublish: admins
        proxy: npmjs

    '@myorg/public-*':
        access: $all
        publish: $authenticated
        proxy: npmjs
```

## Best Practices Checklist

- [ ] Use token authentication (not passwords)
- [ ] Set up role-based access control
- [ ] Enable package signature verification
- [ ] Monitor registry access logs
- [ ] Back up registry storage regularly

## Cross-References

- See [Setup Guide](./01-setup-guide.md) for registry setup
- See [Package Security](../11-package-security/01-supply-chain.md) for security
- See [Enterprise](../14-enterprise-management/01-enterprise-registry.md) for enterprise

## Next Steps

Continue to [Package Publishing](../06-package-publishing/01-publishing-workflow.md) for publishing.
