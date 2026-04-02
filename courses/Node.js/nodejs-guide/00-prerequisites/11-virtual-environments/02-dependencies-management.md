# Dependencies Management Basics

## What You'll Learn

- Managing project dependencies effectively
- Local vs global package installation
- Environment variable setup
- Best practices for dependency management

## Understanding Dependencies

### Package.json Dependencies

```json
{
    "dependencies": {
        "express": "^4.18.0",
        "mongoose": "^7.0.0",
        "dotenv": "^16.0.0"
    },
    "devDependencies": {
        "jest": "^29.0.0",
        "nodemon": "^3.0.0",
        "eslint": "^8.0.0"
    },
    "peerDependencies": {
        "react": ">=16.8.0"
    },
    "optionalDependencies": {
        "fsevents": "^2.3.0"
    }
}
```

### Dependency Types

| Type | Purpose | Example |
|------|---------|---------|
| dependencies | Production runtime | express, mongoose |
| devDependencies | Development only | jest, eslint |
| peerDependencies | Required peer package | react |
| optionalDependencies | Optional features | fsevents |
| bundledDependencies | Bundled with package | - |

## Version Management

### Semantic Versioning

```json
{
    "dependencies": {
        "exact": "1.0.0",
        "patch": "~1.0.0",
        "minor": "^1.0.0",
        "major": "*"
    }
}
```

### Version Ranges

```bash
# Exact version
"express": "4.18.0"

# Patch updates (1.0.x)
"express": "~4.18.0"

# Minor updates (1.x.x)
"express": "^4.18.0"

# Any version
"express": "*"

# Range
"express": ">=4.0.0 <5.0.0"
```

## Installing Dependencies

### Production Dependencies

```bash
# Install and save to dependencies
npm install express
yarn add express
pnpm add express

# Install specific version
npm install express@4.18.0

# Install from GitHub
npm install github:username/repo

# Install from tarball
npm install ./package.tgz
```

### Development Dependencies

```bash
# Install as dev dependency
npm install --save-dev jest
yarn add --dev jest
pnpm add -D jest

# Common dev dependencies
npm install --save-dev typescript @types/node
npm install --save-dev eslint prettier
npm install --save-dev jest @types/jest
```

## Updating Dependencies

### Check Outdated Packages

```bash
# npm
npm outdated

# yarn
yarn outdated

# pnpm
pnpm outdated
```

### Update Packages

```bash
# Update all packages
npm update
yarn upgrade
pnpm update

# Update specific package
npm update express
yarn upgrade express
pnpm update express

# Update to latest major version
npm install express@latest
yarn add express@latest
pnpm add express@latest
```

### Interactive Update

```bash
# npm-check-updates
npx npm-check-updates

# Update package.json
npx npm-check-updates -u

# Then install
npm install
```

## Removing Unused Dependencies

### Detect Unused Dependencies

```bash
# depcheck
npx depcheck

# npm-check
npx npm-check -u
```

### Remove Unused Dependencies

```bash
# Remove dependency
npm uninstall express
yarn remove express
pnpm remove express

# Remove dev dependency
npm uninstall --save-dev jest
yarn remove --dev jest
pnpm remove -D jest
```

## Lock Files

### Understanding Lock Files

```bash
# npm: package-lock.json
# yarn: yarn.lock
# pnpm: pnpm-lock.yaml

# Lock files ensure:
# 1. Reproducible builds
# 2. Consistent versions
# 3. Faster installs
```

### Best Practices

```bash
# Always commit lock files
git add package-lock.json

# Use frozen lockfile in CI
npm ci
yarn install --frozen-lockfile
pnpm install --frozen-lockfile

# Update lock file after changes
npm install
yarn install
pnpm install
```

## Security

### Audit Dependencies

```bash
# npm audit
npm audit

# Fix vulnerabilities
npm audit fix

# Force fix (breaking changes)
npm audit fix --force

# yarn audit
yarn audit

# pnpm audit
pnpm audit
```

### Security Best Practices

```bash
# Keep dependencies updated
npm update

# Use exact versions for critical packages
"express": "4.18.0"

# Check package before installing
npm info express

# Use npm audit in CI
npm audit --production
```

## Environment Variables

### Creating .env Files

```bash
# .env
NODE_ENV=development
PORT=3000
DATABASE_URL=postgresql://localhost:5432/mydb

# .env.production
NODE_ENV=production
PORT=8080
DATABASE_URL=postgresql://prod-server:5432/mydb
```

### Loading Environment Variables

```javascript
// Using dotenv
require('dotenv').config();

// Or ES modules
import 'dotenv/config';

// Access variables
console.log(process.env.PORT);
console.log(process.env.DATABASE_URL);
```

### .gitignore for .env

```gitignore
# .gitignore
.env
.env.local
.env.*.local
```

## Monorepo Dependencies

### Workspace Dependencies

```json
// package.json
{
    "workspaces": [
        "packages/*"
    ]
}
```

```bash
# Add dependency to workspace
npm install express --workspace=packages/api
yarn workspace @myproject/api add express
pnpm add express --filter @myproject/api

# Add as dev dependency
npm install jest --save-dev --workspace=packages/api
```

## Troubleshooting Common Issues

### Dependency Conflicts

```bash
# Problem: Peer dependency conflicts
# Solution: Use --legacy-peer-deps

npm install --legacy-peer-deps
# Or
npm install --force
```

### Version Conflicts

```bash
# Problem: Different versions required
# Solution: Use overrides/resolutions

// package.json
{
    "overrides": {
        "lodash": "4.17.21"
    }
}
```

### Lock File Conflicts

```bash
# Problem: Merge conflicts in lock file
# Solution: Regenerate lock file

rm package-lock.json
npm install
```

## Best Practices Checklist

- [ ] Use semantic versioning correctly
- [ ] Keep dependencies up to date
- [ ] Run security audits regularly
- [ ] Use lock files for reproducible builds
- [ ] Separate dev and production dependencies
- [ ] Use .env for environment variables
- [ ] Never commit .env files
- [ ] Document dependency requirements
- [ ] Use workspace for monorepos
- [ ] Clean unused dependencies

## Performance Optimization Tips

- Use npm ci for faster installs
- Enable npm caching
- Use pnpm for disk efficiency
- Remove unused dependencies
- Use exact versions for critical packages
- Use .npmrc for default configuration
- Use npx for one-off tools

## Cross-References

- See [Project Isolation](./01-project-isolation.md) for environment setup
- See [Package Managers](../10-package-managers/) for npm/yarn/pnpm
- See [Virtual Environments](../11-virtual-environments/) for project isolation
- See [Development Tools](../12-dev-tools-integration/) for IDE integration

## Next Steps

Now that dependencies management is understood, let's integrate development tools. Continue to [Development Tools Integration](../12-dev-tools-integration/).