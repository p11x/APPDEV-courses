# NPM Configuration and Security Best Practices

## What You'll Learn

- npm configuration for different environments
- Security configuration and practices
- Authentication and authorization
- Troubleshooting common configuration issues

## Environment Configuration

```bash
# Development .npmrc
registry=https://registry.npmjs.org/
save-exact=true
progress=true
fund=false

# Production .npmrc
registry=https://registry.npmjs.org/
audit-level=high
engine-strict=true
ignore-scripts=true
package-lock=true

# CI/CD .npmrc
registry=https://registry.npmjs.org/
//registry.npmjs.org/:_authToken=${NPM_TOKEN}
audit-level=critical
fund=false
progress=false
loglevel=error
```

## Security Configuration

```bash
# Enable package signature verification
npm config set verify-signatures true

# Set audit level
npm config set audit-level high

# Disable lifecycle scripts (security risk mitigation)
npm config set ignore-scripts true

# Enable provenance (publish with build provenance)
npm publish --provenance

# Verify package integrity
npm audit signatures
```

## Authentication Setup

```bash
# Token-based authentication (recommended)
export NPM_TOKEN=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
echo "//registry.npmjs.org/:_authToken=${NPM_TOKEN}" >> ~/.npmrc

# GitHub Packages
echo "//npm.pkg.github.com/:_authToken=${GITHUB_TOKEN}" >> ~/.npmrc

# Azure Artifacts
npx vsts-npm-auth -config .npmrc

# Multiple registries
echo "@myorg:registry=https://npm.myorg.com/" >> ~/.npmrc
echo "//npm.myorg.com/:_authToken=${MYORG_TOKEN}" >> ~/.npmrc
```

## Troubleshooting Common Issues

```bash
# Permission errors
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH="$HOME/.npm-global/bin:$PATH"' >> ~/.bashrc

# Cache corruption
npm cache clean --force
npm cache verify

# node_modules corruption
rm -rf node_modules package-lock.json
npm install

# Network issues
npm config set proxy http://proxy:8080
npm config set https-proxy http://proxy:8080
npm config set registry https://registry.npmjs.org/

# Peer dependency conflicts
npm install --legacy-peer-deps

# Lock file conflicts
rm package-lock.json
npm install
git add package-lock.json
```

## Best Practices Checklist

- [ ] Never commit `.npmrc` with tokens to version control
- [ ] Use environment variables for authentication tokens
- [ ] Set `audit-level=high` for production
- [ ] Use `engine-strict=true` to enforce Node.js version
- [ ] Use `save-exact=true` for reproducible builds
- [ ] Run `npm audit` regularly
- [ ] Use `npm ci` in CI/CD

## Cross-References

- See [Registry Structure](./01-registry-structure.md) for registry internals
- See [CLI Reference](./02-npm-cli-reference.md) for commands
- See [Package Security](../11-package-security/01-supply-chain.md) for security

## Next Steps

Continue to [Package.json Mastery](../02-package-json/01-fields-reference.md) for package.json.
