# Alternative Package Managers: Yarn and pnpm

## What You'll Learn

- Yarn Berry features and setup
- pnpm performance and disk efficiency
- Package manager comparison matrix
- Migration strategies between managers

## Yarn Berry (v4)

```bash
# Install Yarn
corepack enable
corepack prepare yarn@stable --activate

# Initialize project
yarn init

# Install dependencies
yarn install
yarn add express
yarn add -D typescript

# Scripts
yarn build
yarn test
```

```yaml
# .yarnrc.yml
nodeLinker: node-modules
yarnPath: .yarn/releases/yarn-4.x.x.cjs
```

## pnpm

```bash
# Install pnpm
corepack enable
corepack prepare pnpm@latest --activate

# Initialize project
pnpm init

# Install dependencies
pnpm install
pnpm add express
pnpm add -D typescript

# Scripts
pnpm build
pnpm test
```

```yaml
# pnpm-workspace.yaml
packages:
  - 'packages/*'
  - 'apps/*'
```

## Comparison Matrix

```
Feature          npm        Yarn Berry    pnpm
─────────────────────────────────────────────────
Speed            Good       Good          Excellent
Disk usage       High       Medium        Low
Monorepo         Workspaces Workspaces    Workspaces
Lock file        package-   yarn.lock     pnpm-lock.
                 lock.json                yaml
Node modules     Flat       PnP/flat      Strict
Ecosystem        Largest    Large         Growing
Learning curve   Low        Medium        Low
```

## Best Practices Checklist

- [ ] Choose one package manager per project
- [ ] Commit lock file to version control
- [ ] Use corepack for version management
- [ ] Test package manager in CI/CD
- [ ] Document package manager choice

## Cross-References

- See [Workspaces](../07-monorepo-management/01-workspaces.md) for monorepos
- See [NPM Architecture](../01-npm-architecture/01-registry-structure.md) for npm
- See [Enterprise](../14-enterprise-management/01-enterprise-registry.md) for enterprise

## Next Steps

Continue to [Package Development](../09-package-development/01-creating-packages.md) for development.
