# Turborepo

## What You'll Learn

- What Turborepo is and how it compares to Nx
- How to set up Turborepo
- How Turbo caching and pipelines work
- How to configure Turborepo for CI

## What Is Turborepo?

Turborepo is a **high-performance build system** for JavaScript/TypeScript monorepos. It focuses on speed through caching and parallelization.

| Feature | Turborepo | Nx |
|---------|-----------|-----|
| Configuration | turbo.json | nx.json + plugins |
| Complexity | Low | Medium-High |
| Caching | Local + Remote | Local + Remote |
| Plugin system | No | Yes (extensive) |
| Best for | Simple monorepos | Large enterprise monorepos |

## Setup

```bash
# Create new project
npx create-turbo@latest my-turbo-repo

# Or add to existing pnpm workspace
pnpm add -Dw turbo
```

## Configuration

```json
// turbo.json

{
  "$schema": "https://turbo.build/schema.json",
  "globalDependencies": [".env"],
  "tasks": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**"]
    },
    "dev": {
      "cache": false,
      "persistent": true
    },
    "test": {
      "dependsOn": ["build"],
      "outputs": []
    },
    "lint": {
      "outputs": []
    }
  }
}
```

## Commands

```bash
# Run tasks
turbo build              # Build all packages
turbo test               # Test all packages
turbo dev                # Dev mode (no cache)
turbo lint               # Lint all

# Filter
turbo build --filter=@myorg/api
turbo build --filter='./packages/*'
turbo build --filter='...[HEAD]'  # Only changed packages

# Remote cache
turbo login
turbo link
```

## Pipeline Definition

```json
// turbo.json — Task dependency graph
{
  "tasks": {
    "build": {
      "dependsOn": ["^build"],     // Build dependencies first
      "outputs": ["dist/**"],      // Cache these outputs
      "inputs": ["src/**"]         // Invalidate cache when these change
    },
    "test": {
      "dependsOn": ["build"],      // Tests run after build
      "outputs": []                // No outputs to cache
    },
    "deploy": {
      "dependsOn": ["build", "test"],
      "cache": false               // Never cache deployments
    }
  }
}
```

## CI Configuration

```yaml
# .github/workflows/ci.yml
name: CI
on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: pnpm/action-setup@v2
      - uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: 'pnpm'
      - run: pnpm install
      - run: turbo build test lint
        env:
          TURBO_TOKEN: ${{ secrets.TURBO_TOKEN }}
          TURBO_TEAM: ${{ secrets.TURBO_TEAM }}
```

## Next Steps

For monorepo best practices, continue to [Monorepo Best Practices](./04-monorepo-best-practices.md).
