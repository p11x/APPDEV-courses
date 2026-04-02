# npm Setup and Configuration

## What You'll Learn

- npm installation and configuration
- npm commands and workflows
- Package management best practices
- npm scripts and automation

## npm Overview

npm (Node Package Manager) is the default package manager for Node.js. It comes bundled with Node.js installation.

### Verify Installation

```bash
# Check npm version
npm --version

# Check npm location
which npm

# Check npm configuration
npm config list
```

## npm Configuration

### Basic Configuration

```bash
# Set registry (default: https://registry.npmjs.org/)
npm config set registry https://registry.npmjs.org/

# Set default author
npm config set init-author-name "Your Name"
npm config set init-author-email "your.email@example.com"
npm config set init-author-url "https://example.com"

# Set default license
npm config set init-license "MIT"

# Set default version
npm config set init-version "1.0.0"
```

### Advanced Configuration

```bash
# Set proxy (corporate networks)
npm config set proxy http://proxy.company.com:8080
npm config set https-proxy http://proxy.company.com:8080

# Set cache location
npm config set cache ~/.npm-cache

# Set global packages location
npm config set prefix ~/.npm-global

# Set log level
npm config set loglevel warn

# Save exact versions by default
npm config set save-exact true

# Save peer dependencies
npm config set save-peer true
```

### View Configuration

```bash
# View all configuration
npm config list

# View specific config
npm config get registry
npm config get prefix

# Edit configuration file
npm config edit

# View configuration file location
npm config list -l | grep config
```

## Package Management

### Initializing Projects

```bash
# Initialize new project
npm init

# Quick initialization with defaults
npm init -y

# Initialize with specific values
npm init --yes --name "my-project" --version "1.0.0"
```

### Installing Packages

```bash
# Install package (adds to dependencies)
npm install express

# Install specific version
npm install express@4.18.0

# Install latest version
npm install express@latest

# Install as dev dependency
npm install --save-dev jest

# Install as peer dependency
npm install --save-peer react

# Install global package
npm install -g nodemon

# Install from GitHub
npm install username/repo

# Install from tarball
npm install ./package.tgz
```

### Updating Packages

```bash
# Update all packages
npm update

# Update specific package
npm update express

# Update to latest major version
npm install express@latest

# Check outdated packages
npm outdated

# Update npm itself
npm install -g npm@latest
```

### Removing Packages

```bash
# Remove package
npm uninstall express

# Remove from dev dependencies
npm uninstall --save-dev jest

# Remove global package
npm uninstall -g nodemon
```

## npm Scripts

### Basic Scripts

```json
{
    "name": "my-project",
    "version": "1.0.0",
    "scripts": {
        "start": "node src/index.js",
        "dev": "nodemon src/index.js",
        "test": "jest",
        "build": "tsc",
        "lint": "eslint src/",
        "format": "prettier --write src/"
    }
}
```

### Running Scripts

```bash
# Run script
npm run start
npm start  # 'start' doesn't need 'run'

# Run multiple scripts
npm run lint && npm run test

# Run script with arguments
npm run test -- --coverage

# List available scripts
npm run
```

### Pre and Post Scripts

```json
{
    "scripts": {
        "prestart": "npm run build",
        "start": "node dist/index.js",
        "poststart": "echo 'Server started'",
        
        "pretest": "npm run lint",
        "test": "jest",
        "posttest": "echo 'Tests completed'",
        
        "prebuild": "npm run clean",
        "build": "tsc",
        "postbuild": "echo 'Build completed'",
        
        "clean": "rm -rf dist/"
    }
}
```

## Package.json Configuration

### Basic Structure

```json
{
    "name": "my-nodejs-project",
    "version": "1.0.0",
    "description": "A Node.js project",
    "main": "dist/index.js",
    "types": "dist/index.d.ts",
    "scripts": {
        "start": "node dist/index.js",
        "dev": "nodemon src/index.ts",
        "build": "tsc",
        "test": "jest"
    },
    "keywords": ["nodejs", "typescript"],
    "author": "Your Name <your.email@example.com>",
    "license": "MIT",
    "dependencies": {
        "express": "^4.18.0"
    },
    "devDependencies": {
        "typescript": "^5.0.0",
        "@types/express": "^4.17.0"
    },
    "engines": {
        "node": ">=18.0.0",
        "npm": ">=9.0.0"
    }
}
```

### Version Ranges

```json
{
    "dependencies": {
        "exact": "1.0.0",
        "patch": "~1.0.0",
        "minor": "^1.0.0",
        "major": "*",
        "range": ">=1.0.0 <2.0.0",
        "latest": "latest"
    }
}
```

## Lock Files

### package-lock.json

```bash
# Generate lock file
npm install

# Install from lock file
npm ci

# Update lock file
npm install --package-lock-only

# Ignore lock file (not recommended)
echo "package-lock.json" >> .gitignore
```

### Lock File Best Practices

```bash
# Always commit lock file
git add package-lock.json

# Use npm ci in CI/CD
npm ci

# Update lock file when dependencies change
npm install
```

## npm Workspaces

### Monorepo Setup

```json
{
    "name": "my-monorepo",
    "version": "1.0.0",
    "private": true,
    "workspaces": [
        "packages/*",
        "apps/*"
    ]
}
```

```bash
# Install dependencies for all workspaces
npm install

# Add dependency to specific workspace
npm install express --workspace=packages/api

# Run script in specific workspace
npm run test --workspace=packages/api

# Run script in all workspaces
npm run test --workspaces
```

## npm Audit

### Security Auditing

```bash
# Run security audit
npm audit

# Fix vulnerabilities automatically
npm audit fix

# Fix with breaking changes
npm audit fix --force

# View audit report
npm audit --json

# Audit production dependencies only
npm audit --production
```

## npm Cache

### Cache Management

```bash
# View cache location
npm config get cache

# List cached packages
npm cache ls

# Verify cache integrity
npm cache verify

# Clean cache
npm cache clean --force

# Add package to cache
npm cache add express@4.18.0
```

## Troubleshooting Common Issues

### Permission Errors

```bash
# Problem: EACCES permission denied
# Solution: Fix npm permissions

# Option 1: Change npm's default directory
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH=~/.npm-global/bin:$PATH' >> ~/.bashrc
source ~/.bashrc

# Option 2: Use npx instead of global install
npx create-react-app my-app
```

### Network Issues

```bash
# Problem: ETIMEDOUT or ECONNREFUSED
# Solution: Check network and proxy

# Test registry connectivity
curl https://registry.npmjs.org/

# Clear npm cache
npm cache clean --force

# Use different registry
npm config set registry https://registry.npmmirror.com
```

### Dependency Conflicts

```bash
# Problem: Peer dependency conflicts
# Solution: Use --legacy-peer-deps

npm install --legacy-peer-deps

# Or install with force
npm install --force
```

### Lock File Issues

```bash
# Problem: Lock file conflicts
# Solution: Regenerate lock file

# Delete lock file and node_modules
rm -rf node_modules package-lock.json

# Reinstall
npm install
```

## Best Practices Checklist

- [ ] Use package-lock.json for reproducible builds
- [ ] Use npm ci in CI/CD pipelines
- [ ] Keep dependencies up to date
- [ ] Use semantic versioning
- [ ] Run npm audit regularly
- [ ] Use npm scripts for automation
- [ ] Configure .npmrc for team settings
- [ ] Use workspaces for monorepos
- [ ] Document npm scripts in README
- [ ] Use npx for one-off commands

## Performance Optimization Tips

- Use npm ci for faster installs
- Enable npm cache
- Use --prefer-offline for cached packages
- Use workspaces for monorepos
- Clean npm cache regularly
- Use npm prune to remove unused packages
- Configure registry for faster downloads

## Cross-References

- See [Yarn Setup](./02-yarn-pnpm.md) for alternative package managers
- See [Virtual Environments](../11-virtual-environments/) for project isolation
- See [Development Tools](../12-dev-tools-integration/) for IDE integration
- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for npm scripts

## Next Steps

Now that npm is configured, let's explore alternative package managers. Continue to [Yarn and pnpm Setup](./02-yarn-pnpm.md).