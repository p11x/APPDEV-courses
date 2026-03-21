# Barrel Exports and Index Files

## Overview
Barrel exports (index files) allow you to export multiple modules from a single entry point, creating cleaner import statements. This guide covers when to use barrel exports and how to avoid their pitfalls.

## Prerequisites
- JavaScript modules
- Import/export syntax

## Core Concepts

### Creating Barrel Exports

```typescript
// [File: src/components/index.ts]
// Barrel export - single entry point
export { Button } from './Button';
export { Card } from './Card';
export { Modal } from './Modal';
```

```typescript
// [File: src/features/auth/index.ts]
export { useAuth } from './hooks/useAuth';
export { LoginForm } from './components/LoginForm';
export type { User, LoginCredentials } from './types';
```

### Using Barrel Exports

```typescript
// ❌ WITHOUT barrel - verbose imports
import Button from '../../components/Button';
import Card from '../../components/Card';
import Modal from '../../components/Modal';

// ✅ WITH barrel - clean imports
import { Button, Card, Modal } from '@/components';
```

## Common Mistakes

### Circular Dependencies
```typescript
// ❌ WRONG - Can cause circular dependencies
// a.ts
export { b } from './b';
// b.ts
export { a } from './a';

// ✅ CORRECT - Avoid circular imports through careful structure
```

## Key Takeaways
- Use barrel exports for cleaner imports
- Be aware of circular dependency risks
- Use eslint-plugin-import to detect issues

## What's Next
Continue to [Dependency Inversion](03-dependency-inversion-in-react.md) to learn about SOLID principles in React.