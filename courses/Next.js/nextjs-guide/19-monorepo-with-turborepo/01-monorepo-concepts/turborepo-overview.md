# Turborepo Overview

## What You'll Learn
- What Turborepo is and how it works
- Core concepts and terminology
- Why use Turborepo with Next.js

## Prerequisites
- Understanding of JavaScript/TypeScript tooling
- Basic knowledge of monorepos

## Do I Need This Right Now?
Turborepo is essential for managing large Next.js projects with multiple packages. If you're building a small app (1-2 pages), you don't need it. Come back here when your project grows or you want to share code between projects.

## Concept Explained Simply

Turborepo is like a smart project manager for your code. Imagine you have multiple projects that share some code. Without Turborepo, when you update shared code, you have to manually update each project. With Turborepo, it automatically figures out what needs to be rebuilt and does it in parallel — saving huge amounts of time.

## Key Features

### 1. Smart Caching
- Remembers what you've built
- Only rebuilds what changed
- Shares cache across your team

### 2. Parallel Execution
- Runs tasks in parallel when possible
- Optimizes build times automatically

### 3. Remote Caching
- Share builds with your team
- CI/CD builds become much faster
- Works with Vercel, AWS S3, or custom

### 4. Task Orchestration
- Define complex workflows
- Dependencies between tasks
- Visualize your pipeline

## Complete Code Example

### Setting Up a Basic Turborepo

```json
// package.json (root)
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
    "test": "turbo run test"
  },
  "devDependencies": {
    "turbo": "^2.0.0"
  }
}
```

```json
// turbo.json
{
  "$schema": "https://turbo.build/schema.json",
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": [".next/**", "!.next/cache/**", "dist/**"]
    },
    "lint": {},
    "dev": {
      "cache": false,
      "persistent": true
    },
    "test": {
      "dependsOn": ["build"]
    },
    "deploy": {
      "dependsOn": ["build", "test", "lint"]
    }
  }
}
```

### Project Structure

```
my-turborepo/
├── apps/
│   ├── web/              # Next.js app
│   │   ├── src/
│   │   │   └── app/
│   │   ├── package.json
│   │   └── next.config.js
│   └── docs/             # Another Next.js app
│       ├── src/
│       ├── package.json
│       └── next.config.js
├── packages/
│   ├── ui/               # Shared UI components
│   │   ├── src/
│   │   │   └── Button.tsx
│   │   └── package.json
│   ├── config/           # Shared config
│   │   ├── eslint/
│   │   └── package.json
│   └── utils/           # Shared utilities
│       ├── src/
│       │   └── format.ts
│       └── package.json
├── package.json
└── turbo.json
```

### Shared Package Example

```typescript
// packages/ui/src/Button.tsx
import React from 'react';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
}

export function Button({ 
  children, 
  variant = 'primary',
  className = '',
  ...props 
}: ButtonProps) {
  const baseStyles = 'px-4 py-2 rounded font-medium transition-colors';
  const variantStyles = {
    primary: 'bg-blue-600 text-white hover:bg-blue-700',
    secondary: 'bg-gray-200 text-gray-900 hover:bg-gray-300',
  };

  return (
    <button
      className={`${baseStyles} ${variantStyles[variant]} ${className}`}
      {...props}
    >
      {children}
    </button>
  );
}
```

```json
// packages/ui/package.json
{
  "name": "@my-turborepo/ui",
  "version": "0.0.0",
  "main": "./src/index.ts",
  "types": "./src/index.ts",
  "exports": {
    ".": "./src/index.ts",
    "./button": "./src/Button.tsx"
  },
  "scripts": {
    "lint": "eslint src/",
    "test": "jest"
  }
}
```

### Using Shared Package in App

```typescript
// apps/web/src/app/page.tsx
import { Button } from '@my-turborepo/ui';

export default function HomePage() {
  return (
    <main className="p-8">
      <h1 className="text-2xl font-bold mb-4">Welcome to My App</h1>
      
      <Button variant="primary" onClick={() => console.log('Clicked!')}>
        Click Me
      </Button>
      
      <Button variant="secondary" className="ml-2">
        Cancel
      </Button>
    </main>
  );
}
```

```json
// apps/web/package.json
{
  "name": "web",
  "version": "0.0.0",
  "dependencies": {
    "@my-turborepo/ui": "*",
    "next": "latest",
    "react": "latest",
    "react-dom": "latest"
  }
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `"workspaces": ["apps/*", "packages/*"]` | Defines monorepo structure | npm/yarn knows where to find packages |
| `"dependsOn": ["^build"]` | Builds dependencies first | Ensures shared packages are built before apps |
| `"outputs": [".next/**"]` | Marks build outputs | Turborepo knows what to cache |
| `"cache": false` | Disables caching | Needed for dev and persistent tasks |
| `"@my-turborepo/ui": "*"` | Uses shared package | Version * means "use whatever is in workspace" |

## Common Mistakes

### Mistake #1: Not Using Workspaces
```typescript
// Wrong: Using separate package.json files without workspaces
// packages/ui/package.json
{ "name": "ui" }

// apps/web/package.json  
{ "dependencies": { "ui": "*" } } // Won't find the local package!
```

```typescript
// Correct: Set up workspaces in root package.json
{
  "workspaces": ["apps/*", "packages/*"]
}
```

### Mistake #2: Missing Dependencies in Pipeline
```typescript
// Wrong: Tasks run without waiting for dependencies
{
  "build": {
    // Missing dependsOn - might fail!
  }
}
```

```typescript
// Correct: Define dependency order
{
  "build": {
    "dependsOn": ["^build"] // Wait for dependencies to build first
  }
}
```

### Mistake #3: Not Excluding Cache
```typescript
// Wrong: Caching dev server breaks hot reload
{
  "dev": {
    "cache": true // Bad! Cache will interfere with dev
  }
}
```

```typescript
// Correct: Disable cache for dev and persistent tasks
{
  "dev": {
    "cache": false,
    "persistent": true
  }
}
```

## Summary
- Turborepo optimizes builds with smart caching
- Use workspaces to share code between apps
- Define pipeline in turbo.json for task orchestration
- Use `dependsOn` to ensure proper build order
- Use `outputs` to mark what should be cached
- Remote caching makes CI/CD much faster
- Great for teams with multiple Next.js apps or shared packages

## Next Steps
- [when-to-use-monorepo.md](./when-to-use-monorepo.md) — When to choose monorepo
