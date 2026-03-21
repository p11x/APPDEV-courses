# Create a Turborepo

## What You'll Learn
- Initialize a new Turborepo project
- Configure package manager
- Set up initial structure

## Prerequisites
- Node.js 18+ installed
- Basic terminal knowledge

## Do I Need This Right Now?
This is a practical guide for setting up Turborepo. If you've decided a monorepo is right for you, this shows exactly how to set it up.

## Concept Explained Simply

Setting up Turborepo is like building the foundation for your house. Get it right, and everything else follows smoothly. We'll use create-turbo to scaffold everything.

## Complete Code Example

### Step 1: Create the Project

```bash
# Create a new Turborepo
npx create-turbo@latest my-turborepo

# Or use npm/yarn/pnpm
npm create turbo@latest my-turborepo -- --package-manager npm
cd my-turborepo
```

### Step 2: Project Structure

After creation, you'll see:

```
my-turborepo/
├── apps/
│   └── web/           # Example Next.js app
│       ├── src/
│       ├── package.json
│       ├── next.config.js
│       └── tsconfig.json
├── packages/
│   └── ui/            # Example shared package
│       ├── src/
│       ├── package.json
│       └── tsconfig.json
├── package.json       # Root package.json
├── turbo.json         # Turborepo config
├── tsconfig.json      # Root TypeScript config
└── package-lock.json
```

### Step 3: Root package.json

```json
{
  "name": "my-turborepo",
  "private": true,
  "workspaces": [
    "apps/*",
    "packages/*"
  ],
  "scripts": {
    "build": "turbo run build",
    "dev": "turbo run dev",
    "lint": "turbo run lint",
    "test": "turbo run test",
    "clean": "turbo run clean"
  },
  "devDependencies": {
    "turbo": "^2.0.0"
  },
  "packageManager": "npm@10.0.0"
}
```

### Step 4: turbo.json Configuration

```json
{
  "$schema": "https://turbo.build/schema.json",
  "globalDependencies": [
    "**/.env.*local"
  ],
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": [".next/**", "!.next/cache/**", "dist/**"]
    },
    "lint": {
      "outputs": []
    },
    "test": {
      "dependsOn": ["build"],
      "outputs": ["coverage/**"],
      "inputs": ["src/**/*.tsx", "src/**/*.ts", "test/**/*.ts", "*.config.ts"]
    },
    "dev": {
      "cache": false,
      "persistent": true
    },
    "clean": {
      "cache": false
    }
  }
}
```

### Step 5: Add a New App

```bash
# Add a new Next.js app to apps/
cd apps
npx create-next-app@latest docs --typescript --eslint --tailwind --src-dir --app --import-alias "@/*"
cd ..
```

### Step 6: Add a Shared Package

```bash
# Create a new package in packages/
mkdir -p packages/utils
cd packages/utils

# Create package.json
cat > package.json << 'EOF'
{
  "name": "@my-turborepo/utils",
  "version": "0.0.0",
  "main": "./src/index.ts",
  "types": "./src/index.ts",
  "exports": {
    ".": "./src/index.ts"
  },
  "scripts": {
    "lint": "eslint src/",
    "test": "jest"
  }
}
EOF

# Create TypeScript config
cat > tsconfig.json << 'EOF'
{
  "extends": "../../tsconfig.json",
  "compilerOptions": {
    "jsx": "react-jsx"
  },
  "include": ["src"]
}
EOF

mkdir src
```

## Using Different Package Managers

### npm

```json
// package.json
{
  "packageManager": "npm@10.0.0"
}
```

### yarn

```json
// package.json  
{
  "packageManager": "yarn@1.22.0"
}
```

### pnpm (Recommended for monorepos)

```json
// package.json
{
  "packageManager": "pnpm@8.0.0"
}
```

```bash
# Install pnpm first
npm install -g pnpm
```

## Common Mistakes

### Mistake #1: Not Using Package Manager Field
```typescript
// Wrong: No package manager specified
{
  "name": "my-turborepo"
  // Will use whatever is installed globally!
}
```

```typescript
// Correct: Pin the package manager
{
  "packageManager": "pnpm@8.0.0"
}
```

### Mistake #2: Wrong Workspace Paths
```typescript
// Wrong: Paths don't match actual structure
{
  "workspaces": [
    "apps/",           // Missing *
    "packages/"
  ]
}
```

```typescript
// Correct: Use wildcards
{
  "workspaces": [
    "apps/*",
    "packages/*"
  ]
}
```

### Mistake #3: Forgetting turbo.json
```typescript
// Without turbo.json, nothing is cached or optimized
// Just regular npm workspaces!

// Add turbo.json for the magic!
```

## Summary
- Use `npx create-turbo@latest` to scaffold
- Configure workspaces in root package.json
- Create turbo.json for task orchestration
- Use packageManager field to lock package manager version
- pnpm is recommended for monorepos
- Add apps with create-next-app
- Create packages manually in packages/ folder

## Next Steps
- [workspace-structure.md](./workspace-structure.md) — Organizing your workspace
