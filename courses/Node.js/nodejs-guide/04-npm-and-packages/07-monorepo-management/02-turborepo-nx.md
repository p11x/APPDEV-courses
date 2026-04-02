# Monorepo Tools: Lerna, Nx, Turborepo

## What You'll Learn

- Lerna for monorepo management
- Nx for build system orchestration
- Turborepo for task running
- Monorepo performance optimization

## Turborepo Setup

```bash
npm install -D turbo
```

```json
// turbo.json
{
    "$schema": "https://turbo.build/schema.json",
    "tasks": {
        "build": {
            "dependsOn": ["^build"],
            "outputs": ["dist/**"]
        },
        "test": {
            "dependsOn": ["build"],
            "outputs": []
        },
        "lint": {
            "outputs": []
        },
        "dev": {
            "cache": false,
            "persistent": true
        }
    }
}
```

```bash
# Run tasks with Turborepo
npx turbo run build        # Build all packages
npx turbo run test         # Test all packages
npx turbo run build --filter=@myorg/api  # Build specific
```

## Nx Setup

```bash
npx create-nx-workspace@latest my-monorepo
```

```bash
# Nx commands
nx build api               # Build specific project
nx test api                # Test specific project
nx affected:build          # Build only affected
nx affected:test           # Test only affected
nx graph                   # Visualize dependency graph
```

## Best Practices Checklist

- [ ] Use Turborepo for task caching
- [ ] Configure build outputs for caching
- [ ] Use `--filter` for targeted builds
- [ ] Leverage remote caching for CI/CD
- [ ] Monitor build times and optimize

## Cross-References

- See [Workspaces](./01-workspaces.md) for npm workspaces
- See [Alternative Managers](../08-alternative-package-managers/01-yarn-pnpm.md) for alternatives
- See [CI/CD](../15-ecosystem-integration/01-github-actions.md) for automation

## Next Steps

Continue to [Alternative Package Managers](../08-alternative-package-managers/01-yarn-pnpm.md).
