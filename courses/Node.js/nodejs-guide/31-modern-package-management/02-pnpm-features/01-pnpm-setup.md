# pnpm Setup

## What You'll Learn

- What pnpm is and why it's popular
- How to install and configure pnpm
- How pnpm's content-addressable storage works
- How pnpm compares to npm and Yarn

## What Is pnpm?

pnpm (performant npm) is a fast, disk-efficient package manager. It uses a **content-addressable storage** (CAS) where each package version is stored once on disk, and projects use hard links to the store.

```
npm/yarn:
  project-a/node_modules/lodash/ → 500KB copied
  project-b/node_modules/lodash/ → 500KB copied (duplicate!)

pnpm:
  ~/.pnpm-store/lodash@4.17.21/  → 500KB (stored once)
  project-a/node_modules/lodash/ → hard link to store
  project-b/node_modules/lodash/ → hard link to store
```

## Installation

```bash
# Via corepack (recommended)
corepack enable
corepack prepare pnpm@latest --activate

# Or via npm
npm install -g pnpm

# Or via curl
curl -fsSL https://get.pnpm.io/install.sh | sh -

# Verify
pnpm --version
```

## Basic Commands

```bash
# Initialize project
pnpm init

# Install dependencies
pnpm install

# Add a package
pnpm add express
pnpm add -D typescript
pnpm add -g nodemon

# Remove a package
pnpm remove express

# Update packages
pnpm update

# Run scripts
pnpm dev
pnpm build
pnpm test
```

## Configuration

```yaml
# .npmrc (pnpm reads this file)

# Registry
registry=https://registry.npmjs.org/

# Strict peer dependencies (default in pnpm)
strict-peer-dependencies=false

# Auto-install peers
auto-install-peers=true

# Shamefully hoist — flatten node_modules like npm
shamefully-hoist=true

# Public hoist pattern — packages that are always hoisted
public-hoist-pattern[]=*eslint*
public-hoist-pattern[]=*prettier*
```

## Performance Comparison

| Operation | npm 10 | yarn 4 | pnpm 9 |
|-----------|--------|--------|--------|
| Clean install (0 deps) | 25s | 12s | 8s |
| Add 1 package | 5s | 2s | 1s |
| Disk usage (500 deps) | 180MB | 150MB | 45MB |
| Install (cached) | 15s | 5s | 3s |

## Common Mistakes

### Mistake 1: Mixing Package Managers

```bash
# WRONG — conflicting lock files
npm install
pnpm install  # Creates pnpm-lock.yaml alongside package-lock.json

# CORRECT — use one manager
pnpm install
rm package-lock.json  # Remove npm lock file
```

### Mistake 2: Not Understanding Phantom Dependencies

```js
// WRONG — pnpm does not hoist by default
import qs from 'qs';  // qs is a dependency of express, not yours
// Error: Cannot find module 'qs'

// CORRECT — add qs as a direct dependency
pnpm add qs
```

## Next Steps

For strict mode, continue to [pnpm Strict Mode](./02-pnpm-strict-mode.md).
