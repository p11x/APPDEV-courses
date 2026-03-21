# Creating Shared Packages

## What You'll Learn
- Create TypeScript packages in monorepo
- Configure exports properly
- Use shared packages in apps

## Prerequisites
- Understanding of workspace structure

## Do I Need This Right Now?
This is practical knowledge for sharing code. If you want to share components, utilities, or configs between apps, you'll need to create and configure packages.

## Concept Explained Simply

Shared packages are like a library in a university — one copy of each book (code) is available for everyone to use. When you update a book, everyone automatically gets the update. This keeps all apps in sync.

## Complete Code Example

### Creating a UI Package

```typescript
// packages/ui/package.json
{
  "name": "@my-turborepo/ui",
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
      "types": "./dist/button.d.ts"
    }
  },
  "scripts": {
    "build": "tsup",
    "lint": "eslint src/",
    "test": "jest",
    "typecheck": "tsc --noEmit"
  },
  "peerDependencies": {
    "react": ">=18",
    "react-dom": ">=18"
  }
}
```

### Creating the Package Code

```typescript
// packages/ui/src/index.ts
export { Button } from './Button';
export { Card } from './Card';
export { Input } from './Input';
export type { ButtonProps } from './Button';
export type { CardProps } from './Card';
export type { InputProps } from './Input';
```

```typescript
// packages/ui/src/Button.tsx
import React from 'react';

export interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'ghost';
  size?: 'sm' | 'md' | 'lg';
  isLoading?: boolean;
}

export function Button({
  children,
  variant = 'primary',
  size = 'md',
  isLoading = false,
  className = '',
  disabled,
  ...props
}: ButtonProps) {
  const baseStyles = 'inline-flex items-center justify-center font-medium rounded-lg transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2';
  
  const variantStyles = {
    primary: 'bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-500',
    secondary: 'bg-gray-100 text-gray-900 hover:bg-gray-200 focus:ring-gray-500',
    ghost: 'bg-transparent hover:bg-gray-100 focus:ring-gray-500',
  };
  
  const sizeStyles = {
    sm: 'px-3 py-1.5 text-sm',
    md: 'px-4 py-2 text-base',
    lg: 'px-6 py-3 text-lg',
  };

  return (
    <button
      className={`${baseStyles} ${variantStyles[variant]} ${sizeStyles[size]} ${className}`}
      disabled={disabled || isLoading}
      {...props}
    >
      {isLoading ? (
        <span className="mr-2">Loading...</span>
      ) : null}
      {children}
    </button>
  );
}
```

### TypeScript Configuration

```typescript
// packages/ui/tsconfig.json
{
  "extends": "../../tsconfig.json",
  "compilerOptions": {
    "jsx": "react-jsx",
    "outDir": "./dist",
    "rootDir": "./src"
  },
  "include": ["src"],
  "exclude": ["node_modules", "dist", "**/*.test.ts"]
}
```

### Build Configuration (tsup)

```typescript
// packages/ui/tsup.config.ts
import { defineConfig } from 'tsup';

export default defineConfig({
  entry: {
    index: './src/index.ts',
  },
  format: ['esm', 'cjs'],
  dts: true,
  splitting: false,
  sourcemap: true,
  clean: true,
  external: ['react', 'react-dom'],
});
```

### Using the Package in an App

```typescript
// apps/web/package.json
{
  "name": "web",
  "dependencies": {
    "@my-turborepo/ui": "*",
    "react": "latest",
    "react-dom": "latest"
  }
}
```

```typescript
// apps/web/src/app/page.tsx
import { Button, Card, Input } from '@my-turborepo/ui';

export default function HomePage() {
  return (
    <main className="p-8">
      <Card title="Welcome">
        <p className="mb-4">This is a shared component!</p>
        
        <Input 
          placeholder="Enter your email"
          className="mb-4"
        />
        
        <Button variant="primary">
          Submit
        </Button>
      </Card>
    </main>
  );
}
```

## Configuration Package

```typescript
// packages/config/eslint/next.js
module.exports = {
  extends: ['next/core-web-vitals'],
  rules: {
    '@typescript-eslint/no-unused-vars': 'error',
    'prefer-const': 'error',
  },
};
```

```typescript
// packages/config/typescript/nextjs.json
{
  "compilerOptions": {
    "jsx": "preserve",
    "plugins": [{ "name": "next" }]
  }
}
```

## Common Mistakes

### Mistake #1: Not Building Packages
```typescript
// Wrong: Importing from source, not dist
// This causes issues!
import { Button } from '@my-turborepo/ui/src';
```

```typescript
// Correct: Always build first, then import from package
// package.json should have "build" script
// Then import from the package name
import { Button } from '@my-turborepo/ui';
```

### Mistake #2: Wrong Exports
```typescript
// Wrong: No exports defined
{
  "name": "@my-turborepo/ui"
  // Can't import anything!
}
```

```typescript
// Correct: Define exports properly
{
  "name": "@my-turborepo/ui",
  "exports": {
    ".": "./dist/index.js"
  }
}
```

### Mistake #3: Peer Dependencies
```typescript
// Wrong: Bundling React, causing version conflicts!
{
  "dependencies": {
    "react": "latest"  // Will cause issues with Next.js React!
  }
}
```

```typescript
// Correct: Use peerDependencies
{
  "peerDependencies": {
    "react": ">=18",
    "react-dom": ">=18"
  }
}
```

## Summary
- Create packages in `packages/` folder
- Use proper package.json with exports
- Build with tsup or similar
- Use peerDependencies for React
- Import using the scoped name
- Always build packages before using apps

## Next Steps
- [caching-builds.md](../03-turborepo-advanced/caching-builds.md) — Build caching strategies
