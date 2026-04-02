# Lerna Setup

## What You'll Learn

- What Lerna is and how it works
- How to set up Lerna with modern package managers
- How to version and publish packages
- How Lerna compares to Turborepo and Nx

## What Is Lerna?

Lerna is a tool for managing monorepos. Modern Lerna (v7+) delegates package management to npm/yarn/pnpm and focuses on **versioning** and **publishing**.

## Setup

```bash
# Install Lerna
npm install -g lerna

# Initialize
npx lerna init
```

```json
// lerna.json
{
  "$schema": "node_modules/lerna/schemas/lerna-schema.json",
  "version": "independent",
  "npmClient": "pnpm",
  "packages": ["packages/*"]
}
```

## Commands

```bash
# Run script in all packages
lerna run build
lerna run test

# Run in topological order
lerna run build --sort

# Version packages
lerna version          # Prompt for version bumps
lerna version patch    # Bump all to next patch
lerna version --conventional-commits  # Auto from commits

# Publish
lerna publish
lerna publish from-git
lerna publish from-package
```

## Version Strategy

```json
// lerna.json
{
  "version": "independent"  // Each package has its own version
}
// OR
{
  "version": "1.0.0"  // All packages share the same version
}
```

## Next Steps

For Nx, continue to [Nx Monorepo](./02-nx-monorepo.md).
