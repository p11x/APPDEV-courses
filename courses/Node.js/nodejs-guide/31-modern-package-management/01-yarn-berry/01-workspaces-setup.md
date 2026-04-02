# Yarn Berry Workspaces

## What You'll Learn

- What Yarn Berry (v2+) is and how it differs from Yarn Classic
- How to set up workspaces for monorepos
- How workspace protocols work
- How to manage dependencies across workspaces

## What Is Yarn Berry?

Yarn Berry is the codename for Yarn v2+. It was rewritten from scratch with features like **Plug'n'Play** (PnP), **zero-installs**, and **constraints**. It produces a `yarn.lock` file and uses `.yarnrc.yml` for configuration.

| Feature | Yarn Classic (v1) | Yarn Berry (v4) |
|---------|-------------------|-----------------|
| Install speed | Fast | Faster |
| Node modules | Yes (node_modules/) | Optional (PnP) |
| Zero installs | No | Yes |
| Lock file | yarn.lock | yarn.lock |
| Config | .yarnrc | .yarnrc.yml |

## Setup

```bash
# Install Yarn Berry globally (or use corepack)
corepack enable
corepack prepare yarn@stable --activate

# Or install via npm
npm install -g yarn

# Verify version
yarn --version
# Should show 4.x.x

# Create a new project
mkdir my-monorepo && cd my-monorepo
yarn init -2

# This creates:
# .yarn/           → Yarn binary and cache
# .yarnrc.yml      → Configuration
# package.json     → Root package.json
# yarn.lock        → Lock file
```

## Project Structure

```
my-monorepo/
├── .yarn/
├── .yarnrc.yml
├── package.json           → Root (private: true, workspaces)
├── yarn.lock
├── packages/
│   ├── shared/
│   │   ├── package.json   → @myorg/shared
│   │   └── src/
│   ├── api/
│   │   ├── package.json   → @myorg/api
│   │   └── src/
│   └── web/
│       ├── package.json   → @myorg/web
│       └── src/
└── apps/
    └── admin/
        ├── package.json   → @myorg/admin
        └── src/
```

## Root package.json

```json
{
  "name": "my-monorepo",
  "private": true,
  "workspaces": [
    "packages/*",
    "apps/*"
  ],
  "scripts": {
    "build": "yarn workspaces foreach -A run build",
    "test": "yarn workspaces foreach -A run test",
    "lint": "yarn workspaces foreach -A run lint"
  },
  "packageManager": "yarn@4.1.0"
}
```

## Workspace Package

```json
// packages/shared/package.json

{
  "name": "@myorg/shared",
  "version": "1.0.0",
  "type": "module",
  "main": "./src/index.js",
  "scripts": {
    "build": "tsc",
    "test": "node --test"
  },
  "dependencies": {
    "zod": "^3.22.0"
  }
}
```

```json
// packages/api/package.json

{
  "name": "@myorg/api",
  "version": "1.0.0",
  "type": "module",
  "scripts": {
    "dev": "node --watch src/server.js",
    "build": "tsc"
  },
  "dependencies": {
    "@myorg/shared": "workspace:*",
    "express": "^4.18.0"
  }
}
```

## Workspace Protocol

```bash
# Add a dependency on another workspace
yarn workspace @myorg/api add @myorg/shared

# In package.json, this creates:
# "@myorg/shared": "workspace:*"
```

| Protocol | Meaning |
|----------|---------|
| `workspace:*` | Any version from the workspace |
| `workspace:^` | Semver-compatible version |
| `workspace:~` | Patch-compatible version |

## Running Commands

```bash
# Run script in specific workspace
yarn workspace @myorg/api dev

# Run script in all workspaces
yarn workspaces foreach -A run build

# Run in parallel
yarn workspaces foreach -Ap run build

# Run in topological order (dependencies first)
yarn workspaces foreach -Apt run build

# Add dependency to specific workspace
yarn workspace @myorg/api add express

# Add dev dependency to root
yarn add -D typescript
```

## Common Mistakes

### Mistake 1: Missing private: true

```json
// WRONG — workspaces require the root to be private
{
  "name": "my-monorepo",
  "workspaces": ["packages/*"]
  // Missing "private": true!
}

// CORRECT
{
  "name": "my-monorepo",
  "private": true,
  "workspaces": ["packages/*"]
}
```

### Mistake 2: Installing in Workspace Directly

```bash
# WRONG — this installs in the root, not the workspace
cd packages/api
yarn add express

# CORRECT — use workspace command from root
yarn workspace @myorg/api add express
```

## Next Steps

For Plug'n'Play, continue to [Plug'n'Play](./02-plug-and-play.md).
