# Feature-Based Folder Structure

## Overview
Organizing React projects by feature rather than by file type (components, hooks, utils) improves maintainability and makes it easier to navigate large codebases. This guide covers feature-based folder structures and best practices for organizing React applications.

## Prerequisites
- React project experience
- Understanding of component architecture

## Core Concepts

### Traditional vs Feature-Based Structure

```bash
# ❌ WRONG - File-type organization
src/
├── components/
│   ├── Button.tsx
│   ├── Card.tsx
│   └── Modal.tsx
├── hooks/
│   ├── useAuth.ts
│   └── useFetch.ts
├── utils/
│   ├── api.ts
│   └── helpers.ts
└── pages/
    ├── Home.tsx
    └── About.tsx

# ✅ CORRECT - Feature-based organization
src/
├── features/
│   ├── auth/
│   │   ├── components/
│   │   │   ├── LoginForm.tsx
│   │   │   └── ProtectedRoute.tsx
│   │   ├── hooks/
│   │   │   └── useAuth.ts
│   │   ├── api/
│   │   │   └── authApi.ts
│   │   └── types/
│   │       └── index.ts
│   ├── products/
│   │   ├── components/
│   │   ├── hooks/
│   │   ├── api/
│   │   └── types/
│   └── cart/
│       ├── components/
│       ├── hooks/
│       ├── api/
│       └── types/
├── shared/
│   ├── components/
│   ├── hooks/
│   └── utils/
└── app/
```

### Feature Structure

```typescript
// [File: src/features/auth/types/index.ts]
export interface User {
  id: string;
  email: string;
  name: string;
}

export interface LoginCredentials {
  email: string;
  password: string;
}
```

```typescript
// [File: src/features/auth/api/authApi.ts]
import { User, LoginCredentials } from '../types';

export async function login(credentials: LoginCredentials): Promise<User> {
  const response = await fetch('/api/auth/login', {
    method: 'POST',
    body: JSON.stringify(credentials),
  });
  return response.json();
}
```

## Key Takeaways
- Group by feature, not file type
- Each feature has its own components, hooks, api, types
- Shared code goes in shared/ or common/
- Features should be self-contained

## What's Next
Continue to [Barrel Exports](02-barrel-exports-and-index-files.md) to learn about organizing exports.