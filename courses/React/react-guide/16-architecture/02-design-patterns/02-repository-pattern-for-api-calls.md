# Repository Pattern for API Calls

## Overview
The Repository Pattern creates an abstraction layer between data access and business logic. This pattern makes it easy to swap implementations (e.g., between real API and mock data) and improves testability.

## Prerequisites
- TypeScript basics
- API fetching

## Core Concepts

### Repository Interface

```typescript
// [File: src/repositories/UserRepository.ts]
export interface User {
  id: string;
  name: string;
  email: string;
}

export interface UserRepository {
  getAll(): Promise<User[]>;
  getById(id: string): Promise<User | null>;
  create(user: Omit<User, 'id'>): Promise<User>;
  update(id: string, data: Partial<User>): Promise<User>;
  delete(id: string): Promise<void>;
}
```

### Implementation

```typescript
// [File: src/repositories/HttpUserRepository.ts]
export class HttpUserRepository implements UserRepository {
  async getAll(): Promise<User[]> {
    const res = await fetch('/api/users');
    return res.json();
  }
  
  async getById(id: string): Promise<User | null> {
    const res = await fetch(`/api/users/${id}`);
    if (!res.ok) return null;
    return res.json();
  }
  
  async create(user: Omit<User, 'id'>): Promise<User> {
    const res = await fetch('/api/users', {
      method: 'POST',
      body: JSON.stringify(user),
    });
    return res.json();
  }
  
  async update(id: string, data: Partial<User>): Promise<User> {
    const res = await fetch(`/api/users/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(data),
    });
    return res.json();
  }
  
  async delete(id: string): Promise<void> {
    await fetch(`/api/users/${id}`, { method: 'DELETE' });
  }
}
```

## Key Takeaways
- Define repository interface
- Create implementations for different data sources
- Swap implementations easily for testing

## What's Next
Continue to [Observer Pattern](03-observer-pattern-with-events.md) to learn about event-based patterns.