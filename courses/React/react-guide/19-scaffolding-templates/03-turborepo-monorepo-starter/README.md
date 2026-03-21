# Turborepo Monorepo Production Template

## Overview
A complete Turborepo monorepo setup for managing multiple React applications and shared packages. Enables code sharing, faster builds, and efficient caching.

## Project Structure

```
my-monorepo/
├── apps/
│   ├── web/                        # Next.js web application
│   │   ├── src/
│   │   ├── package.json
│   │   ├── next.config.js
│   │   └── tsconfig.json
│   │
│   ├── docs/                      # Documentation site
│   │   ├── src/
│   │   ├── package.json
│   │   └── next.config.js
│   │
│   └── mobile/                    # React Native (optional)
│       ├── src/
│       └── package.json
│
├── packages/
│   ├── ui/                        # Shared UI component library
│   │   ├── src/
│   │   │   ├── Button/
│   │   │   │   ├── Button.tsx
│   │   │   │   ├── index.ts
│   │   │   │   └── types.ts
│   │   │   └── index.ts
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   └── tsup.config.ts        # Build config
│   │
│   ├── utils/                     # Shared utilities
│   │   ├── src/
│   │   │   ├── format.ts
│   │   │   └── index.ts
│   │   ├── package.json
│   │   └── tsconfig.json
│   │
│   ├── hooks/                     # Shared hooks
│   │   ├── src/
│   │   │   ├── useLocalStorage.ts
│   │   │   └── index.ts
│   │   ├── package.json
│   │   └── tsconfig.json
│   │
│   ├── config/                    # Shared ESLint/TypeScript configs
│   │   ├── eslint-config-custom/
│   │   │   └── index.js
│   │   ├── tsconfig/
│   │   │   ├── base.json
│   │   │   ├── nextjs.json
│   │   │   └── react-library.json
│   │   └── package.json
│   │
│   └── eslint-config/             # Shared ESLint
│       └── package.json
│
├── turbo.json                     # Turborepo config
├── package.json                   # Root package.json
├── pnpm-workspace.yaml            # PNPM workspaces
├── tsconfig.json                  # Root TypeScript config
└── .eslintrc.js                   # Root ESLint
```

## Configuration Files

### turbo.json

```json
{
  "$schema": "https://turbo.build/schema.json",
  "globalDependencies": ["**/.env.*local"],
  
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**", ".next/**", "!.next/cache/**"]
    },
    
    "lint": {},
    
    "test": {
      "dependsOn": ["build"],
      "outputs": ["coverage/**"],
      "inputs": ["src/**/*.tsx", "src/**/*.ts", "test/**/*.ts", "vitest.config.ts"]
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

### pnpm-workspace.yaml

```yaml
packages:
  - 'apps/*'
  - 'packages/*'
```

### Root package.json

```json
{
  "name": "my-monorepo",
  "private": true,
  "workspaces": [
    "apps/*",
    "packages/*"
  ],
  "scripts": {
    "build": "turbo build",
    "dev": "turbo dev",
    "lint": "turbo lint",
    "test": "turbo test",
    "clean": "turbo clean"
  },
  "devDependencies": {
    "turbo": "latest",
    "eslint": "^8.0.0",
    "prettier": "^3.0.0"
  }
}
```

### packages/ui/package.json

```json
{
  "name": "@my-org/ui",
  "version": "0.0.0",
  "main": "./dist/index.js",
  "module": "./dist/index.mjs",
  "types": "./dist/index.d.ts",
  "exports": {
    ".": {
      "import": "./dist/index.mjs",
      "require": "./dist/index.js",
      "types": "./dist/index.d.ts"
    },
    "./button": {
      "import": "./dist/button.mjs",
      "require": "./dist/button.js",
      "types": "./dist/button.d.ts"
    }
  },
  "scripts": {
    "build": "tsup",
    "dev": "tsup --watch",
    "lint": "eslint src/",
    "test": "vitest run"
  }
}
```

## How to Add New Apps

### Adding a New Application

```bash
# Create a Next.js app in the apps folder
cd apps
npx create-next-app@latest new-app --typescript --tailwind
cd ..
```

Update the new app's `package.json` to use local packages:

```json
{
  "dependencies": {
    "@my-org/ui": "workspace:*",
    "@my-org/utils": "workspace:*"
  }
}
```

### Adding a New Package

```bash
# Create a new shared package
mkdir -p packages/my-new-package/src
cd packages/my-new-package

# Initialize package.json
npm init -y

# Add to workspace dependencies in other apps
cd apps/web
npm install @my-org/my-new-package@workspace:*
```

## Shared UI Package Example

```tsx
// [File: packages/ui/src/Button/Button.tsx]
import React from 'react';
import { cva, type VariantProps } from 'class-variance-authority';
import styles from './Button.module.css';

/**
 * Button component with variants using class-variance-authority.
 * Built to be consumed by all apps in the monorepo.
 */
const buttonVariants = cva(styles.button, {
  variants: {
    variant: {
      primary: styles.primary,
      secondary: styles.secondary,
      danger: styles.danger,
    },
    size: {
      sm: styles.small,
      md: styles.medium,
      lg: styles.large,
    },
  },
  defaultVariants: {
    variant: 'primary',
    size: 'md',
  },
});

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof buttonVariants> {
  isLoading?: boolean;
}

export function Button({
  className,
  variant,
  size,
  isLoading,
  children,
  disabled,
  ...props
}: ButtonProps) {
  return (
    <button
      className={buttonVariants({ variant, size, className })}
      disabled={disabled || isLoading}
      {...props}
    >
      {isLoading ? <span className={styles.spinner} /> : null}
      {children}
    </button>
  );
}
```

```typescript
// [File: packages/ui/src/Button/index.ts]
export { Button, type ButtonProps } from './Button';
```

```typescript
// [File: packages/ui/src/index.ts]
export { Button, type ButtonProps } from './Button';
// Add more exports as you create components
```

## Consuming Shared Package in App

```tsx
// [File: apps/web/src/app/page.tsx]
import { Button } from '@my-org/ui';
import { formatCurrency } from '@my-org/utils';

export default function HomePage() {
  return (
    <main>
      <h1>Welcome</h1>
      <p>{formatCurrency(99.99)}</p>
      
      <Button variant="primary" size="lg">
        Get Started
      </Button>
      
      <Button variant="secondary" size="sm">
        Learn More
      </Button>
    </main>
  );
}
```

## Running Commands

```bash
# Install all dependencies
pnpm install

# Build all packages and apps
pnpm build

# Run development for all apps
pnpm dev

# Run tests across the monorepo
pnpm test

# Lint all packages
pnpm lint

# Clean all build outputs
pnpm clean

# Build specific app
pnpm --filter web build

# Build specific package
pnpm --filter @my-org/ui build

# Dev mode for specific app
pnpm --filter web dev

# Run tests for specific package
pnpm --filter @my-org/ui test
```

## Turborepo Benefits

1. **Intelligent Caching** — Turborepo caches build outputs across machines
2. **Parallel Execution** — Tasks run in parallel when possible
3. **Remote Caching** — Share cache with your team via Vercel
4. **Workspace Dependencies** — Share code without publishing to npm
5. **Pipeline Orchestration** — Define task dependencies clearly

## Why This Scales

1. **Shared Code** — UI components, utilities, and hooks are shared
2. **Consistent Config** — TypeScript and ESLint configs are centralized
3. **Independent Deployments** — Each app can be deployed separately
4. **Efficient Builds** — Only rebuilds changed packages
5. **Developer Experience** — Single `pnpm install` for entire codebase

## Next Steps

1. Run `pnpm install` to install all dependencies
2. Run `pnpm build` to build all packages
3. Run `pnpm dev` to start development
4. Add your first shared component in `packages/ui/`

For more details, see:
- [Turborepo Setup](../../18-ecosystem/02-monorepos/01-turborepo-setup.md)
- [Shared Packages](../../18-ecosystem/02-monorepos/02-shared-packages.md)
