# Bun Package Manager and Performance Comparison

## What You'll Learn

- Bun as a package manager
- Performance benchmarks
- Migration strategies
- Multi-manager setups

## Bun Package Manager

```bash
# Install Bun
curl -fsSL https://bun.sh/install | bash

# Install dependencies
bun install
bun add express
bun add -D typescript

# Run scripts
bun run build
bun run test
```

## Performance Benchmarks

```
Install Speed (1000 dependencies):
─────────────────────────────────────────────
bun    ████████████████  2.5s
pnpm   ████████████████████████  5.0s
yarn   ██████████████████████████████  8.0s
npm    ██████████████████████████████████  10.0s

Disk Usage (same project):
─────────────────────────────────────────────
pnpm   ████████████  200MB
yarn   ████████████████████  350MB
bun    ████████████████████  360MB
npm    ████████████████████████████  500MB
```

## Migration Between Managers

```bash
# npm → pnpm
rm -rf node_modules package-lock.json
pnpm install

# npm → yarn
rm -rf node_modules package-lock.json
yarn install

# npm → bun
rm -rf node_modules package-lock.json
bun install
```

## Best Practices Checklist

- [ ] Evaluate based on project needs
- [ ] Benchmark for your specific workload
- [ ] Test compatibility with your dependencies
- [ ] Use corepack for manager versioning
- [ ] Document manager choice in README

## Cross-References

- See [Yarn/pnpm](./01-yarn-pnpm.md) for Yarn and pnpm
- See [NPM Architecture](../01-npm-architecture/01-registry-structure.md) for npm
- See [Monorepo](../07-monorepo-management/01-workspaces.md) for monorepos

## Next Steps

Continue to [Package Development](../09-package-development/01-creating-packages.md) for development.
