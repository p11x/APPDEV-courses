# TypeScript Setup with Vite

## Overview
TypeScript is JavaScript with syntax for types. It helps developers catch errors early through type checking and provides better tooling with intelligent autocomplete. When combined with Vite's lightning-fast dev server, you get a development experience that's both safe and incredibly fast. This guide covers setting up TypeScript in a React project using Vite, with strict mode enabled for maximum bug prevention.

## Prerequisites
- Basic understanding of JavaScript (ES6+ syntax)
- Node.js installed (v18+ recommended)
- Familiarity with React components and JSX
- Code editor (VS Code recommended)

## Core Concepts

### Setting Up TypeScript with Vite
Vite provides first-class support for TypeScript with zero configuration needed. When you create a new project with Vite and the React-TS template, it automatically configures everything you need.

```typescript
// [File: src/App.tsx]
// This is a simple React component written in TypeScript
// Notice how we can specify types for props and see autocomplete in action

// Define the shape of our component's props using an interface
// Interfaces are perfect for defining object shapes in TypeScript
interface AppProps {
  // The title prop is a required string
  title: string;
  // The user prop is optional - notice the ? mark
  user?: {
    name: string;
    email: string;
  };
}

// TypeScript will now warn us if we forget to pass title
// and provide autocomplete for the user object properties
function App({ title, user }: AppProps) {
  return (
    <div>
      <h1>{title}</h1>
      {user && <p>Welcome, {user.name}!</p>}
    </div>
  );
}

export default App;
```

### Understanding tsconfig.json
The `tsconfig.json` file is the heart of TypeScript configuration. It tells the compiler how to transform your TypeScript code into JavaScript and what rules to enforce.

```json
// [File: tsconfig.json]
{
  // This extends Vite's recommended TypeScript configuration
  // which provides smart defaults for React projects
  "extends": "@tsconfig/vite/tsconfig.json",

  // Compiler options control how TypeScript behaves
  "compilerOptions": {
    // Target defines which JavaScript version to output
    // "ES2020" is a good balance of modern features and browser support
    "target": "ES2020",

    // Use ESNext to use the latest ECMAScript features
    // This allows optional chaining (?.) and nullish coalescing (??)
    "useDefineForClassFields": true,

    // Library defines which built-in types are available
    // DOM includes types for browser APIs (document, window, etc.)
    // ESNext includes types for modern JavaScript features
    "lib": ["ES2020", "DOM", "DOM.Iterable"],

    // Module system - Vite uses ES modules by default
    "module": "ESNext",

    // Skip type checking for .d.ts files - speeds up compilation
    "skipLibCheck": true,

    // Module resolution strategy - "bundler" is recommended for Vite
    // It allows path aliases and resolves node_modules correctly
    "moduleResolution": "bundler",

    // Allow importing .ts files
    // This is required for the new JSX transform
    "allowImportingTsExtensions": true,

    // How to handle JSX code - "react-jsx" is for the new JSX transform
    // This is the recommended setting for React 18+
    "jsx": "react-jsx",

    // Strict mode enables ALL strict type checking options
    // This is CRITICAL for catching bugs early
    // We'll explain each strict option below
    "strict": true,

    // Report errors on unused local variables
    // Helps keep code clean
    "noUnusedLocals": true,

    // Report errors on unused function parameters
    // Catches mistakes like typing parameters you don't use
    "noUnusedParameters": true,

    // Report errors when code returns implicitly (no return statement)
    // Useful for catching functions that should return something
    "noImplicitReturns": true,

    // Report errors on fallthrough in switch statements
    // Prevents accidental missing break statements
    "noFallthroughCasesInSwitch": true,

    // Path aliases - allows @/* to map to src/*
    // This makes imports cleaner and more maintainable
    "baseUrl": ".",
    "paths": {
      "@/*": ["./src/*"]
    }
  },

  // Include tells TypeScript which files to check
  // This pattern includes all TypeScript and React files in src
  "include": ["src"],

  // References to other tsconfig files (for monorepos)
  "references": [{ "path": "./tsconfig.node.json" }]
}
```

### Strict Mode Explained
The `strict: true` option is a umbrella setting that enables multiple strict type-checking features. Here's why each one matters:

```typescript
// [File: src/examples/strictModeExamples.ts]

// 1. noImplicitAny - Prevents 'any' type inference
// ❌ WRONG - TypeScript will infer 'any' and complain
function add(a, b) {
  return a + b;
}

// ✅ CORRECT - Always specify types for parameters
function add(a: number, b: number): number {
  return a + b;
}

// 2. strictNullChecks - Catches null/undefined errors
// ❌ WRONG - Accessing property of potentially null object
function getUserName(user: { name: string }) {
  return user.name.toUpperCase(); // What if user is undefined?
}

// ✅ CORRECT - Handle null/undefined explicitly
function getUserName(user?: { name: string }) {
  if (!user) return 'Guest';
  return user.name.toUpperCase();
}

// 3. strictPropertyInitialization - Ensures class properties are initialized
// ❌ WRONG - Property might be undefined
class UserProfile {
  name: string; // Error: Property 'name' has no initializer
  age: number;
}

// ✅ CORRECT - Initialize in constructor or use definite assignment
class UserProfile {
  name: string;
  age: number;
  
  constructor(name: string, age: number) {
    this.name = name;
    this.age = age;
  }
}

// 4. noImplicitThis - Catches 'this' type errors
// ❌ WRONG - 'this' is implicitly 'any'
function greet() {
  return `Hello, ${this.name}`;
}

// ✅ CORRECT - Explicitly type 'this'
function greet(this: { name: string }) {
  return `Hello, ${this.name}`;
}
```

### Path Aliases with Vite
Path aliases make imports cleaner by allowing you to use `@/` instead of `../` or `../../`. This is especially helpful in large projects with deep folder structures.

```typescript
// [File: vite.config.ts]
import { defineConfig } from 'vite';
// Import the React plugin - handles JSX transformation and Fast Refresh
import react from '@vitejs/plugin-react';
// Import path module for resolving aliases
import path from 'path';

export default defineConfig({
  // Configure the React plugin with its options
  plugins: [react()],
  
  // Resolve path aliases - tells Vite how to handle @/*
  resolve: {
    alias: {
      // @ maps to the src folder
      // This allows: import Button from '@/components/Button'
      '@': path.resolve(__dirname, './src'),
    },
  },
});
```

Now update tsconfig.json to match:

```json
// [File: tsconfig.json] - relevant section
{
  "compilerOptions": {
    "baseUrl": ".",
    "paths": {
      "@/*": ["./src/*"]
    }
  }
}
```

Now you can import like this:

```typescript
// [File: src/components/Button.tsx]
// Instead of: import { utils } from '../../utils/helpers'
// You can use: import { utils } from '@/utils/helpers'

import { formatDate } from '@/utils/date';
import { Button } from '@/components/ui';
import { useAuth } from '@/hooks/useAuth';

export function Dashboard() {
  const formattedDate = formatDate(new Date());
  return <Button>Today: {formattedDate}</Button>;
}
```

### Ambient Declarations (.d.ts)
TypeScript uses `.d.ts` files to provide type information for JavaScript libraries that don't have TypeScript types. These are called "ambient declarations".

```typescript
// [File: src/vite-env.d.ts]
/// <reference types="vite/client" />

// This tells TypeScript about Vite's client-side API
// It includes types for:
// - import.meta.env - environment variables
// - import.meta.hot - HMR API
// - Vite-specific CSS imports

// You can also declare global types here
declare module '*.svg' {
  // When importing SVG files as React components
  const content: React.FC<React.SVGProps<SVGElement>>;
  export default content;
}

declare module '*.module.css' {
  // For CSS Modules - allows type-safe class name access
  const classes: { [key: string]: string };
  export default classes;
}
```

### Running TypeScript with ts-node vs tsx
For running TypeScript scripts (not in the browser), you have two main options:

```bash
# Option 1: tsx - Fast, modern alternative to ts-node
# Install: npm install -D tsx
# Run: npx tsx script.ts

# Option 2: ts-node - Traditional choice
# Install: npm install -D ts-node @types/node
# Run: npx ts-node script.ts
```

```typescript
// [File: scripts/build-report.ts]
// Example script to demonstrate TypeScript execution
// Run with: npx tsx scripts/build-report.ts

// Define interfaces for our data
interface BuildMetric {
  timestamp: Date;
  duration: number;
  bundleSize: number;
}

// Parse command line arguments
const args = process.argv.slice(2);
const outputPath = args[0] ?? './build-report.json';

// Generate some mock data
const metrics: BuildMetric[] = [
  { timestamp: new Date(), duration: 1500, bundleSize: 250000 },
  { timestamp: new Date(), duration: 1800, bundleSize: 275000 },
];

// Log the results
console.log(`Generated build report at ${outputPath}`);
console.log(`Total builds: ${metrics.length}`);

// This shows TypeScript working with:
// - Interfaces
// - Type inference
// - Nullish coalescing operator (??)
// - Array types
```

## Common Mistakes

### Mistake 1: Disabling Strict Mode
```typescript
// ❌ WRONG - Disabling strict mode defeats the purpose of TypeScript
// tsconfig.json
{
  "compilerOptions": {
    "strict": false // This allows all sorts of bugs!
  }
}

// ✅ CORRECT - Keep strict mode enabled
{
  "compilerOptions": {
    "strict": true
  }
}
```

### Mistake 2: Using `any` Type
```typescript
// ❌ WRONG - 'any' defeats TypeScript's type checking
function processData(data: any): any {
  return data.map((x: any) => x.value); // No type safety!
}

// ✅ CORRECT - Use proper types
interface DataItem {
  value: string;
}

function processData(data: DataItem[]): string[] {
  return data.map(item => item.value);
}
```

### Mistake 3: Forgetting to Install @types Packages
```bash
# ❌ WRONG - Missing type definitions
npm install lodash

# ✅ CORRECT - Install both the library and its types
npm install lodash
npm install -D @types/lodash

# For React-specific types:
npm install -D @types/react @types/react-dom
```

### Mistake 4: Not Using Path Aliases Consistently
```typescript
// ❌ WRONG - Mixed import styles are confusing
import { utils } from '../../../../utils';
import { Button } from '@/components/Button';

// ✅ CORRECT - Use aliases consistently
import { utils } from '@/utils';
import { Button } from '@/components/Button';
```

## Real-World Example

Here's a complete example of a typed React component with proper TypeScript setup:

```typescript
// [File: src/components/UserCard.tsx]
// A fully typed UserCard component demonstrating best practices

// Import React with types - use React.FC for functional components
import React from 'react';

// Define the User type - interfaces are great for object shapes
interface User {
  id: string;
  name: string;
  email: string;
  avatar?: string; // Optional - users might not have avatars
  role: 'admin' | 'user' | 'guest'; // Union type - limited options
  createdAt: Date;
}

// Define component props using an interface
interface UserCardProps {
  // User is required - must be provided
  user: User;
  // Optional: callback function when user is clicked
  onClick?: (user: User) => void;
  // Optional: custom className for styling
  className?: string;
  // Optional: show more details
  showDetails?: boolean;
}

// Use React.FC (Functional Component) with generic props type
// This provides built-in children typing and prop types
const UserCard: React.FC<UserCardProps> = ({
  user,
  onClick,
  className = '', // Default value with proper typing
  showDetails = false,
}) => {
  // Event handler with proper typing
  const handleClick = () => {
    // onClick might be undefined, so use optional chaining
    onClick?.(user);
  };

  // Format date with proper type handling
  const formattedDate = user.createdAt.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  });

  return (
    <div
      // Combine className conditionally
      className={`user-card ${className}`}
      // Type the click event properly
      onClick={handleClick}
      // Role-based styling with data attribute
      data-role={user.role}
    >
      {/* Optional chaining for avatar */}
      {user.avatar && (
        <img
          src={user.avatar}
          alt={`${user.name}'s avatar`}
          className="user-avatar"
        />
      )}
      
      <div className="user-info">
        <h3 className="user-name">{user.name}</h3>
        <p className="user-email">{user.email}</p>
        
        {/* Conditional rendering with showDetails */}
        {showDetails && (
          <div className="user-details">
            <p>Role: {user.role}</p>
            <p>Joined: {formattedDate}</p>
            <p>ID: {user.id}</p>
          </div>
        )}
      </div>
    </div>
  );
};

// Export as default with explicit type annotation
export default UserCard;

// Also export a typed version for direct use
export type { User, UserCardProps };
```

## Key Takeaways
- Use `npm create vite@latest my-app -- --template react-ts` for instant TypeScript setup
- Always keep `strict: true` enabled in tsconfig.json - it catches real bugs
- Path aliases (`@/`) make imports cleaner in large projects
- Install `@types/*` packages for JavaScript libraries
- Use interfaces for object shapes and types for unions/primitives
- `tsx` is the fastest way to run TypeScript scripts

## What's Next
Continue to [Types vs Interfaces in React](02-types-vs-interfaces-in-react.md) to understand when to use each type definition syntax and how they work with React's prop system.