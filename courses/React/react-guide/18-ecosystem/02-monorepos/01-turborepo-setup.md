# Turborepo Setup

## Overview
Turborepo is a high-performance build system for JavaScript and TypeScript monorepos. It accelerates builds with intelligent caching and parallel execution.

## Prerequisites
- Node.js knowledge
- Monorepo concepts

## Core Concepts

### Project Structure

```
my-monorepo/
├── apps/
│   ├── web/          # Next.js app
│   ├── mobile/       # React Native app
│   └── docs/         # Documentation site
├── packages/
│   ├── ui/           # Shared UI components
│   ├── utils/        # Shared utilities
│   └── config/       # Shared configurations
├── turbo.json       # Turborepo configuration
├── package.json     # Root package.json
└── pnpm-workspace.yaml
```

### Configuration

```json
// [File: turbo.json]
{
  "$schema": "https://turbo.build/schema.json",
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**", ".next/**"]
    },
    "lint": {},
    "test": {
      "dependsOn": ["build"]
    },
    "dev": {
      "cache": false,
      "persistent": true
    }
  }
}
```

### Package Setup

```json
// [File: packages/ui/package.json]
{
  "name": "@my-org/ui",
  "version": "1.0.0",
  "main": "./src/index.ts",
  "types": "./dist/index.d.ts",
  "exports": {
    ".": "./src/index.ts",
    "./button": "./src/Button/index.ts"
  },
  "scripts": {
    "build": "tsup src/index.ts --dts",
    "dev": "tsup src/index.ts --watch"
  }
}
```

### Installing Dependencies

```bash
# [File: Terminal]
# Install all dependencies
pnpm install

# Add dependency to specific package
pnpm add react --filter @my-org/ui

# Build all packages
pnpm build
```

## Key Takeaways
- Turborepo uses pipeline configuration
- Dependencies between packages are tracked
- Outputs are cached intelligently

## What's Next
Continue to [Shared Packages](02-shared-packages.md) to learn about sharing code in monorepos.