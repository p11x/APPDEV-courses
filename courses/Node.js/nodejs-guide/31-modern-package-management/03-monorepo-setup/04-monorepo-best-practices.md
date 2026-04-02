# Monorepo Best Practices

## What You'll Learn

- How to structure a monorepo
- Dependency management strategies
- Versioning and publishing strategies
- CI/CD for monorepos

## Project Structure

```
monorepo/
├── packages/
│   ├── shared/           ← Shared utilities
│   ├── ui/               ← UI components
│   └── config/           ← Shared configs (eslint, tsconfig)
├── apps/
│   ├── web/              ← Frontend application
│   ├── api/              ← Backend API
│   └── admin/            ← Admin dashboard
├── tools/
│   ├── scripts/          ← Build scripts
│   └── generators/       ← Code generators
├── package.json          ← Root (private)
├── turbo.json            ← Build config
└── pnpm-workspace.yaml  ← Workspace config
```

## Dependency Rules

```
apps/* → packages/*  ← Apps can depend on packages
packages/* → packages/*  ← Packages can depend on other packages
packages/* → apps/*  ← NEVER (packages should not depend on apps)
```

## Shared Configuration

```json
// packages/config/tsconfig/base.json
{
  "compilerOptions": {
    "strict": true,
    "esModuleInterop": true,
    "moduleResolution": "bundler",
    "declaration": true
  }
}
```

```json
// packages/api/tsconfig.json
{
  "extends": "../config/tsconfig/base.json",
  "compilerOptions": {
    "outDir": "./dist"
  }
}
```

## Versioning Strategy

| Strategy | Use When |
|----------|----------|
| **Independent** | Packages are released independently |
| **Fixed/Synchronized** | All packages release together |
| **Changesets** | GitHub-integrated, PR-based versioning |

## Changesets

```bash
# Install
pnpm add -Dw @changesets/cli

# Initialize
pnpm changeset init

# Add a changeset (after making changes)
pnpm changeset

# Version packages
pnpm changeset version

# Publish
pnpm changeset publish
```

## CI Best Practices

```yaml
# Only run affected tasks
- run: turbo build --filter='...[origin/main]'
- run: turbo test --filter='...[origin/main]'

# Cache remote
- run: turbo build
  env:
    TURBO_TOKEN: ${{ secrets.TURBO_TOKEN }}
```

## Next Steps

For bundle analysis, continue to [Bundle Analysis](../04-package-optimization/01-bundle-analysis.md).
