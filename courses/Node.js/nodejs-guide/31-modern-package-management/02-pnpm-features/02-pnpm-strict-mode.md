# pnpm Strict Mode

## What You'll Learn

- What pnpm's strict dependency resolution is
- How phantom dependencies are prevented
- How to configure hoisting
- How to handle compatibility issues

## Strict by Default

pnpm creates a **non-flat** `node_modules/` by default. Packages can only access their direct dependencies, not transitive ones.

```
npm (flat):
  node_modules/
  ├── express/        ← Can import lodash (transitive dep)
  └── lodash/         ← Available to ALL packages

pnpm (strict):
  node_modules/
  ├── express/
  │   └── node_modules/
  │       └── qs/     ← Only express can import qs
  └── .pnpm/
      ├── express@4.18.0/
      │   └── node_modules/
      │       ├── express/ → symlink
      │       └── qs/      → symlink to store
      └── lodash@4.17.21/
          └── node_modules/
              └── lodash/ → symlink to store
```

## Hoisting Options

```ini
# .npmrc

# Strict (default) — no hoisting
shamefully-hoist=false

# Hoist everything (like npm)
shamefully-hoist=true

# Selective hoisting
public-hoist-pattern[]=*eslint*
public-hoist-pattern[]=*prettier*
public-hoist-pattern[]=*jest*
```

## Compatibility Fixes

```ini
# .npmrc — Common fixes for tools that assume flat node_modules

# ESLint (needs hoisted plugins)
public-hoist-pattern[]=@eslint/*

# Jest (needs hoisted modules)
public-hoist-pattern[]=*jest*

# Storybook
shamefully-hoist=true

# React Native
shamefully-hoist=true
```

## Benefits of Strict Mode

| Benefit | Explanation |
|---------|-------------|
| No phantom deps | Cannot import unlisted packages |
| Faster installs | Less disk I/O (symlinks, not copies) |
| Less disk space | Shared store across projects |
| Deterministic | Same install on every machine |

## Next Steps

For workspaces, continue to [pnpm Workspace](./03-pnpm-workspace.md).
