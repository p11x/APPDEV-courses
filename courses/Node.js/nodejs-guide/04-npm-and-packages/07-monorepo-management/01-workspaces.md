# Monorepo Management with npm Workspaces

## What You'll Learn

- npm workspaces setup and configuration
- Package interdependencies in monorepos
- Monorepo scripts and automation
- Shared package management

## npm Workspaces Setup

```json
// Root package.json
{
    "name": "my-monorepo",
    "private": true,
    "workspaces": [
        "packages/*",
        "apps/*"
    ],
    "scripts": {
        "build": "npm run build --workspaces",
        "test": "npm run test --workspaces",
        "lint": "npm run lint --workspaces"
    }
}
```

```
monorepo/
├── package.json          (root)
├── packages/
│   ├── utils/
│   │   └── package.json  (@myorg/utils)
│   └── types/
│       └── package.json  (@myorg/types)
├── apps/
│   ├── api/
│   │   └── package.json  (@myorg/api)
│   └── web/
│       └── package.json  (@myorg/web)
```

## Workspace Commands

```bash
# Install all workspace dependencies
npm install

# Add dependency to specific workspace
npm install express -w packages/api
npm install -D typescript -w packages/api

# Run script in specific workspace
npm run test -w packages/utils
npm run build -w apps/api

# Run script in all workspaces
npm run build --workspaces
npm run test --workspaces

# List workspace packages
npm ls --workspaces --depth=0

# Publish all workspaces
npm publish --workspaces
```

## Cross-Workspace Dependencies

```json
// packages/api/package.json
{
    "name": "@myorg/api",
    "dependencies": {
        "@myorg/utils": "*",
        "@myorg/types": "*",
        "express": "^4.21.0"
    }
}
```

## Best Practices Checklist

- [ ] Set `private: true` on root package
- [ ] Use scoped package names
- [ ] Define shared scripts in root
- [ ] Use workspace protocol for internal deps
- [ ] Keep workspace dependencies in sync

## Cross-References

- See [Alternative Managers](../08-alternative-package-managers/01-yarn-pnpm.md) for Yarn/pnpm
- See [Package Development](../09-package-development/01-creating-packages.md) for development
- See [Enterprise](../14-enterprise-management/01-enterprise-registry.md) for enterprise

## Next Steps

Continue to [Alternative Package Managers](../08-alternative-package-managers/01-yarn-pnpm.md) for Yarn/pnpm.
