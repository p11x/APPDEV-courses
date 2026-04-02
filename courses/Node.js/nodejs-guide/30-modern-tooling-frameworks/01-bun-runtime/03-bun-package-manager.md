# Bun Package Manager

## What You'll Learn

- How to use Bun as a drop-in replacement for npm
- Bun's lock file and resolution strategy
- How to manage workspaces with Bun
- How to publish packages with Bun

## Basic Commands

```bash
# Initialize a project
bun init -y

# Install all dependencies
bun install

# Add a package
bun add express

# Add dev dependency
bun add -d typescript @types/node

# Remove a package
bun remove express

# Update packages
bun update

# Run scripts
bun run dev
bun run build
bun test

# Run a package directly (like npx)
bunx create-next-app my-app
```

## Lock File

Bun uses `bun.lockb` — a **binary lock file** that is 10-100x faster to parse than `package-lock.json`:

```bash
# bun.lockb is created automatically on bun install
# It's binary, not human-readable (by design for speed)

# If you need to inspect it:
bun install --yarn  # Generates yarn.lock instead (for compatibility)
```

## Workspaces

```json
// package.json (root)
{
  "name": "monorepo",
  "private": true,
  "workspaces": ["packages/*", "apps/*"]
}
```

```bash
# Install all workspace dependencies
bun install

# Add a dependency to a specific workspace
bun add react --filter @myapp/web

# Run scripts across workspaces
bun run --filter '*' build
```

## Package.json Scripts

```json
{
  "scripts": {
    "dev": "bun --watch server.ts",
    "build": "bun build server.ts --outdir dist --target node",
    "start": "bun dist/server.js",
    "test": "bun test",
    "lint": "bunx eslint .",
    "format": "bunx prettier --write ."
  }
}
```

## Publishing

```bash
# Login to npm
bunx npm login

# Publish a package
bun publish

# Publish with access
bun publish --access public
```

## Speed Comparison

| Operation | npm | yarn | pnpm | bun |
|-----------|-----|------|------|-----|
| Clean install | 60s | 30s | 15s | 3s |
| Add 1 package | 8s | 4s | 2s | 0.3s |
| Lock file parse | 2s | 1s | 0.5s | 0.01s |

## Common Mistakes

### Mistake 1: Committing bun.lockb Without Team Agreement

```bash
# WRONG — team uses npm, you commit bun.lockb
git add bun.lockb
# Team gets confused by binary lock file

# CORRECT — align on package manager
# Either everyone uses bun, or add bun.lockb to .gitignore
```

### Mistake 2: Mixing Package Managers

```bash
# WRONG — conflicting lock files
npm install        # Creates package-lock.json
bun install        # Creates bun.lockb
yarn install       # Creates yarn.lock

# CORRECT — pick one and stick with it
bun install        # Only use bun
```

## Next Steps

For Bun's test runner, continue to [Bun Testing Framework](./04-bun-testing-framework.md).
