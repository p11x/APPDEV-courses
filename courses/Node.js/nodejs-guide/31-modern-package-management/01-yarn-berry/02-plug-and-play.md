# Yarn Plug'n'Play

## What You'll Learn

- What Plug'n'Play (PnP) is and how it works
- How PnP differs from node_modules
- How to configure PnP
- How to handle PnP compatibility issues

## What Is Plug'n'Play?

PnP replaces the `node_modules/` directory. Instead of copying packages into `node_modules`, Yarn stores them in a central cache and generates a `.pnp.cjs` file that tells Node.js where to find each package.

```
Traditional (node_modules):
  node_modules/
  ├── express/
  │   ├── package.json
  │   └── index.js
  └── lodash/
      ├── package.json
      └── index.js

Plug'n'Play:
  .pnp.cjs          → Maps package names to cache locations
  .yarn/cache/      → Compressed package files
  (no node_modules)
```

## Benefits

| Aspect | node_modules | PnP |
|--------|-------------|-----|
| Install speed | Slow (copy files) | Fast (no copying) |
| Disk usage | High (duplicates) | Low (compressed cache) |
| Dependency resolution | Slow (file system walks) | Fast (in-memory map) |
| Phantom dependencies | Possible | Impossible |
| Disk space | 500MB-2GB | 50-200MB |

## Configuration

```yaml
# .yarnrc.yml

# Enable PnP (default in Yarn Berry)
nodeLinker: pnp

# Or use node-modules for compatibility
# nodeLinker: node-modules

# Enable zero-installs (commit cache to git)
enableGlobalCache: false
```

## Zero-Installs

```yaml
# .yarnrc.yml
enableGlobalCache: false
```

```gitignore
# .gitignore — commit the cache for zero-installs
.yarn/*
!.yarn/patches
!.yarn/plugins
!.yarn/releases
!.yarn/sdks
!.yarn/versions
# Do NOT ignore .yarn/cache for zero-installs
```

With zero-installs, `git clone && yarn` installs instantly — no network requests.

## Compatibility Issues

Some tools don't work with PnP out of the box:

```bash
# Fix: install the PnP compatibility plugin
yarn plugin import @yarnpkg/plugin-compat

# Or switch to node_modules linker for problematic projects
# .yarnrc.yml
nodeLinker: node-modules
```

## Common Mistakes

### Mistake 1: Running npm install After yarn install

```bash
# WRONG — creates node_modules, breaking PnP
yarn install
npm install  # Breaks PnP setup!

# CORRECT — use only yarn
yarn install
```

### Mistake 2: Importing Phantom Dependencies

```js
// WRONG — express depends on qs, but you did not add qs as a dependency
import qs from 'qs';  // PnP blocks this — qs is not in your package.json

// CORRECT — add qs as a direct dependency
// yarn add qs
import qs from 'qs';
```

## Next Steps

For Yarn Classic comparison, continue to [Berry vs Classic](./03-berry-vs-classic.md).
