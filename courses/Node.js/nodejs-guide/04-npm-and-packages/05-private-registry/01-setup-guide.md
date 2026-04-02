# Private Registry Setup and Configuration

## What You'll Learn

- Setting up private registries (Verdaccio, GitHub Packages)
- Registry authentication and access control
- Publishing to private registries
- Registry security best practices

## Verdaccio Setup

```bash
# Install Verdaccio
npm install -g verdaccio

# Start registry
verdaccio  # http://localhost:4873

# Configure npm to use it
npm set registry http://localhost:4873

# Publish to local registry
npm publish --registry http://localhost:4873
```

```yaml
# verdaccio.yaml
storage: ./storage
plugins: ./plugins

web:
    title: Private Registry

auth:
    htpasswd:
        file: ./htpasswd
        max_users: 100

uplinks:
    npmjs:
        url: https://registry.npmjs.org/
        timeout: 30s

packages:
    '@myorg/*':
        access: $authenticated
        publish: $authenticated
        proxy: npmjs
    '**':
        access: $all
        publish: $authenticated
        proxy: npmjs

server:
    keepAliveTimeout: 60

logs:
    - { type: stdout, format: pretty, level: http }
```

## GitHub Packages

```bash
# .npmrc for GitHub Packages
@myorg:registry=https://npm.pkg.github.com
//npm.pkg.github.com/:_authToken=${GITHUB_TOKEN}

# Publish to GitHub Packages
npm publish

# Install from GitHub Packages
npm install @myorg/my-package
```

## Best Practices Checklist

- [ ] Use scoped packages for private packages
- [ ] Store auth tokens in environment variables
- [ ] Set up access control by team/role
- [ ] Configure uplinks for public packages
- [ ] Monitor registry health and storage

## Cross-References

- See [Registry Structure](../01-npm-architecture/01-registry-structure.md) for registry internals
- See [Security](../11-package-security/01-supply-chain.md) for security
- See [Publishing](../06-package-publishing/01-publishing-workflow.md) for publishing

## Next Steps

Continue to [Registry Security](./02-registry-security.md) for security practices.
