# Yarn and pnpm Setup

## What You'll Learn

- Installing and configuring Yarn
- Setting up pnpm
- Comparing package managers
- Choosing the right tool for your project

## Yarn Installation and Setup

### Installing Yarn

```bash
# Using npm (recommended for Node.js users)
npm install -g yarn

# Using Homebrew (macOS)
brew install yarn

# Using Chocolatey (Windows)
choco install yarn

# Using Scoop (Windows)
scoop install yarn

# Verify installation
yarn --version
```

### Yarn Configuration

```bash
# Set registry
yarn config set registry https://registry.yarnpkg.com

# Set proxy
yarn config set proxy http://proxy.company.com:8080
yarn config set https-proxy http://proxy.company.com:8080

# Set default license
yarn config set init-license MIT

# Set default author
yarn config set init-author-name "Your Name"

# View configuration
yarn config list

# Edit configuration file
yarn config edit
```

### Basic Yarn Commands

```bash
# Initialize project
yarn init

# Install dependencies
yarn install
yarn                    # Shorthand

# Add dependency
yarn add express

# Add dev dependency
yarn add --dev jest
yarn add -D jest        # Shorthand

# Add global package
yarn global add nodemon

# Remove dependency
yarn remove express

# Update dependencies
yarn upgrade
yarn upgrade express

# Run scripts
yarn start
yarn test
yarn build

# List dependencies
yarn list

# Check outdated
yarn outdated
```

### Yarn Workspaces

```json
// package.json
{
    "private": true,
    "workspaces": [
        "packages/*",
        "apps/*"
    ]
}
```

```bash
# Install all workspaces
yarn install

# Add dependency to workspace
yarn workspace @myproject/api add express

# Run script in workspace
yarn workspace @myproject/api test

# Run script in all workspaces
yarn workspaces run test
```

### Yarn Plug'n'Play (PnP)

```bash
# Enable PnP (no node_modules)
yarn config set nodeLinker pnp

# Or in .yarnrc.yml
nodeLinker: pnp

# Install with PnP
yarn install

# PnP creates .pnp.cjs file instead of node_modules
```

## pnpm Installation and Setup

### Installing pnpm

```bash
# Using npm
npm install -g pnpm

# Using Homebrew (macOS)
brew install pnpm

# Using Chocolatey (Windows)
choco install pnpm

# Using Scoop (Windows)
scoop install pnpm

# Using standalone script
curl -fsSL https://get.pnpm.io/install.sh | sh -

# Verify installation
pnpm --version
```

### pnpm Configuration

```bash
# Set registry
pnpm config set registry https://registry.npmmirror.com

# Set proxy
pnpm config set proxy http://proxy.company.com:8080
pnpm config set https-proxy http://proxy.company.com:8080

# Set store location
pnpm config set store-dir ~/.pnpm-store

# Set global bin location
pnpm config set global-bin-dir ~/.local/bin

# View configuration
pnpm config list

# Edit configuration
pnpm config edit
```

### Basic pnpm Commands

```bash
# Initialize project
pnpm init

# Install dependencies
pnpm install
pnpm i                 # Shorthand

# Add dependency
pnpm add express

# Add dev dependency
pnpm add -D jest

# Add global package
pnpm add -g nodemon

# Remove dependency
pnpm remove express
pnpm rm express        # Shorthand

# Update dependencies
pnpm update
pnpm up                # Shorthand

# Run scripts
pnpm start
pnpm test
pnpm build

# List dependencies
pnpm list
pnpm ls                # Shorthand

# Check outdated
pnpm outdated
```

### pnpm Workspaces

```yaml
# pnpm-workspace.yaml
packages:
  - 'packages/*'
  - 'apps/*'
```

```bash
# Install all workspaces
pnpm install

# Add dependency to workspace
pnpm add express --filter @myproject/api

# Run script in workspace
pnpm test --filter @myproject/api

# Run script in all workspaces
pnpm -r test
```

### pnpm Store

```bash
# View store location
pnpm store path

# Prune unused packages
pnpm store prune

# Verify store integrity
pnpm store status

# Add package to store
pnpm store add express@4.18.0
```

## Package Manager Comparison

### Feature Comparison

| Feature | npm | Yarn | pnpm |
|---------|-----|------|------|
| Speed | Medium | Fast | Fastest |
| Disk Usage | High | Medium | Low |
| Lock File | package-lock.json | yarn.lock | pnpm-lock.yaml |
| Workspaces | Yes | Yes | Yes |
| PnP | No | Yes | No |
| Strict Mode | No | Yes | Yes |
| Caching | Good | Good | Excellent |
| Monorepo | Good | Good | Excellent |

### Speed Comparison

```bash
# Benchmark different package managers
# Clean install (no cache)
npm install      # ~30-60 seconds
yarn install     # ~20-40 seconds
pnpm install     # ~10-20 seconds

# With cache
npm install      # ~15-30 seconds
yarn install     # ~10-20 seconds
pnpm install     # ~5-10 seconds
```

### Disk Usage Comparison

```bash
# node_modules size for typical project
npm:     ~150-300 MB
yarn:    ~120-250 MB
pnpm:    ~50-150 MB (with hard links)

# pnpm uses a global store with hard links
# Much more efficient for multiple projects
```

## Migration Between Package Managers

### npm to Yarn

```bash
# Remove node_modules and lock file
rm -rf node_modules package-lock.json

# Install Yarn
npm install -g yarn

# Install dependencies with Yarn
yarn install

# Verify
yarn test
```

### npm to pnpm

```bash
# Remove node_modules and lock file
rm -rf node_modules package-lock.json

# Install pnpm
npm install -g pnpm

# Install dependencies with pnpm
pnpm install

# Or use import command
pnpm import

# Verify
pnpm test
```

### Yarn to pnpm

```bash
# Remove node_modules and lock file
rm -rf node_modules yarn.lock

# Install dependencies with pnpm
pnpm install

# Verify
pnpm test
```

## Advanced Configuration

### Yarn Berry (Yarn 2+)

```bash
# Enable Yarn Berry
yarn set version berry

# Configuration in .yarnrc.yml
nodeLinker: node-modules
plugins:
  - path: .yarn/plugins/@yarnpkg/plugin-interactive-tools.cjs
    spec: "@yarnpkg/plugin-interactive-tools"
```

### pnpm Configuration File

```yaml
# .npmrc
registry=https://registry.npmmirror.com
strict-peer-dependencies=false
auto-install-peers=true
```

```yaml
# pnpm-workspace.yaml
packages:
  - 'packages/*'
  - 'apps/*'
```

## Troubleshooting Common Issues

### Yarn Issues

```bash
# Problem: Yarn global packages not found
# Solution: Add to PATH

# Find Yarn global bin
yarn global bin

# Add to PATH
export PATH="$(yarn global bin):$PATH"

# Problem: Yarn cache issues
# Solution: Clean cache
yarn cache clean
```

### pnpm Issues

```bash
# Problem: pnpm store corruption
# Solution: Verify and repair store
pnpm store prune
pnpm store verify

# Problem: Hard links not working
# Solution: Check filesystem support
# pnpm requires filesystem that supports hard links

# Problem: Scripts not finding packages
# Solution: Use shamefully-hoist
# .npmrc
shamefully-hoist=true
```

### Permission Issues

```bash
# Problem: EACCES errors
# Solution: Fix permissions

# Yarn
yarn config set prefix ~/.yarn
export PATH=~/.yarn/bin:$PATH

# pnpm
pnpm config set global-bin-dir ~/.local/bin
export PATH=~/.local/bin:$PATH
```

## Best Practices Checklist

- [ ] Choose appropriate package manager for project
- [ ] Use lock files for reproducible builds
- [ ] Configure registry for faster downloads
- [ ] Use workspaces for monorepos
- [ ] Keep package manager updated
- [ ] Use appropriate scripts in package.json
- [ ] Document package manager choice
- [ ] Use .npmrc or .yarnrc for configuration
- [ ] Clean cache regularly
- [ ] Audit dependencies for vulnerabilities

## Performance Optimization Tips

- Use pnpm for best performance and disk usage
- Enable caching for faster installs
- Use workspaces for monorepo efficiency
- Use --frozen-lockfile in CI/CD
- Clean unused packages regularly
- Use offline mode when possible
- Configure registry for faster downloads

## Cross-References

- See [npm Setup](./01-npm-setup.md) for npm configuration
- See [Virtual Environments](../11-virtual-environments/) for project isolation
- See [Development Tools](../12-dev-tools-integration/) for IDE integration
- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for automation

## Next Steps

Now that package managers are configured, let's set up virtual environments. Continue to [Virtual Environment Creation](../11-virtual-environments/).