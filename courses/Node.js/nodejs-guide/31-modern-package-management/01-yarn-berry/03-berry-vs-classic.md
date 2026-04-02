# Yarn Berry vs Classic

## What You'll Learn

- Key differences between Yarn Classic (v1) and Berry (v4)
- How to migrate from Classic to Berry
- When to use each version
- Performance comparisons

## Comparison Table

| Feature | Yarn Classic (v1) | Yarn Berry (v4) |
|---------|-------------------|-----------------|
| Lock file format | yarn.lock v1 | yarn.lock v2 |
| Node modules | Always | Optional (PnP) |
| Zero-installs | No | Yes |
| Offline mirror | Yes | Yes (cache) |
| Constraints | No | Yes |
| Plugin system | Limited | Full |
| TypeScript support | External | Built-in SDK |
| Workspace support | Basic | Advanced |
| Resolution | node_modules | PnP (default) |
| Install speed | ~30s | ~5s |
| Disk usage | High | Low |

## Migration from Classic

```bash
# 1. Update Yarn to Berry
corepack prepare yarn@stable --activate

# 2. Set version in project
yarn set version stable

# 3. Update .gitignore
# Remove: node_modules/
# Add: .yarn/*
# Keep: !.yarn/cache (for zero-installs)

# 4. Install with Berry
yarn install

# 5. If PnP breaks things, use node_modules linker
# .yarnrc.yml
nodeLinker: node-modules
```

## When to Use Classic

- **Legacy projects** that cannot migrate
- **Libraries** that need widest compatibility
- **CI pipelines** that rely on node_modules structure

## When to Use Berry

- **New projects** — better defaults
- **Monorepos** — advanced workspace features
- **TypeScript projects** — built-in SDK
- **Disk-constrained environments** — PnP saves space

## Next Steps

For Yarn commands, continue to [Yarn Commands](./04-yarn-commands.md).
