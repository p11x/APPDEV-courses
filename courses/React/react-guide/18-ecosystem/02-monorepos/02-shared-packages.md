# Shared Packages

## Overview
Learn how to create and manage shared packages in a monorepo for code reuse across applications.

## Prerequisites
- Turborepo or monorepo knowledge
- TypeScript

## Core Concepts

### Creating a UI Package

```typescript
// [File: packages/ui/src/Button/Button.tsx]
import React from 'react';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'danger';
  size?: 'sm' | 'md' | 'lg';
}

export function Button({ 
  variant = 'primary', 
  size = 'md', 
  children,
  className,
  ...props 
}: ButtonProps) {
  return (
    <button 
      className={`btn btn-${variant} btn-${size} ${className ?? ''}`}
      {...props}
    >
      {children}
    </button>
  );
}
```

### Exporting from Package

```typescript
// [File: packages/ui/src/index.ts]
// Barrel exports for cleaner imports
export { Button } from './Button';
export { Input } from './Input';
export { Card } from './Card';
export type { ButtonProps } from './Button';
export type { InputProps } from './Input';
```

### Using Shared Package in App

```tsx
// [File: apps/web/src/App.tsx]
// Import from shared package
import { Button, Card } from '@my-org/ui';

function App() {
  return (
    <Card>
      <h1>Welcome</h1>
      <Button variant="primary">Get Started</Button>
    </Card>
  );
}

export default App;
```

### Package Configuration

```json
// [File: packages/ui/tsconfig.json]
{
  "compilerOptions": {
    "target": "ES2020",
    "module": "ESNext",
    "declaration": true,
    "declarationMap": true,
    "jsx": "react-jsx",
    "outDir": "./dist",
    "rootDir": "./src"
  },
  "include": ["src/**/*"],
  "exclude": ["node_modules", "dist"]
}
```

## Key Takeaways
- Use barrel exports for clean imports
- Build packages as ESM and CJS
- TypeScript declarations enable autocomplete

## What's Next
Continue to [Nx Setup](03-nx-setup.md) for an alternative monorepo tool.