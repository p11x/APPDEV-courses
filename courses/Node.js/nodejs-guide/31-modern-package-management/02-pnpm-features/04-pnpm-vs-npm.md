# pnpm vs npm

## What You'll Learn

- Detailed comparison of pnpm and npm
- When to choose each
- How to migrate from npm to pnpm

## Comparison

| Feature | npm 10 | pnpm 9 |
|---------|--------|--------|
| Lock file | package-lock.json | pnpm-lock.yaml |
| node_modules | Flat | Strict (non-flat) |
| Disk usage | High | Low (70% less) |
| Install speed | Slower | Faster |
| Phantom deps | Possible | Prevented |
| Workspaces | Basic | Advanced |
| Strict mode | No | Yes (default) |
| Monorepo support | Basic | Excellent |

## Migration from npm

```bash
# 1. Install pnpm
corepack prepare pnpm@latest --activate

# 2. Import from npm
pnpm import  # Converts package-lock.json to pnpm-lock.yaml

# 3. Remove npm artifacts
rm package-lock.json
rm -rf node_modules

# 4. Install with pnpm
pnpm install

# 5. Update scripts (if needed)
# Most npm commands work the same in pnpm
```

## Command Mapping

| npm | pnpm |
|-----|------|
| `npm install` | `pnpm install` |
| `npm install express` | `pnpm add express` |
| `npm install -D typescript` | `pnpm add -D typescript` |
| `npm uninstall express` | `pnpm remove express` |
| `npm run dev` | `pnpm dev` or `pnpm run dev` |
| `npm test` | `pnpm test` |
| `npm ls` | `pnpm ls` |

## Next Steps

For monorepo tools, continue to [Lerna Setup](../03-monorepo-setup/01-lerna-setup.md).
