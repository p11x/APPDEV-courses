# Jotai with Suspense

## What You'll Learn
- Using Jotai async atoms with Suspense
- Handling loading and error states
- Best practices for async Jotai
- Suspense boundaries

## Prerequisites
- Understanding of Jotai atoms
- Knowledge of React Suspense
- Familiarity with async/await

## Concept Explained Simply

Jotai async atoms are powerful because they integrate perfectly with React Suspense. When an async atom is loading, Suspense shows your fallback. When it resolves, the component renders. This is much cleaner than manually managing loading states.

Think of it like ordering food at a restaurant: while your food is being prepared (loading), you see a spinner or "cooking" message (Suspense fallback). When it's ready, you get your food (data resolves).

## Complete Code Example

### Async Atom with Suspense

```typescript
// atoms/userAtoms.ts
import { atom } from "jotai";

export const userIdAtom = atom("");

export const userAtom = atom(async (get) => {
  const userId = get(userIdAtom);
  
  if (!userId) return null;
  
  const response = await fetch(`/api/users/${userId}`);
  
  if (!response.ok) {
    throw new Error("Failed to fetch user");
  }
  
  return response.json();
});
```

```typescript
// components/UserProfile.tsx
"use client";

import { Suspense } from "react";
import { useAtom } from "jotai";
import { userAtom, userIdAtom } from "@/atoms/userAtoms";

function UserContent() {
  const [user] = useAtom(userAtom);
  
  if (!user) return <p>No user selected</p>;
  
  return (
    <div>
      <h1>{user.name}</h1>
      <p>{user.email}</p>
    </div>
  );
}

function UserLoading() {
  return <div>Loading user...</div>;
}

export default function UserProfile() {
  const [, setUserId] = useAtom(userIdAtom);
  
  return (
    <div>
      <select onChange={(e) => setUserId(e.target.value)}>
        <option value="">Select user</option>
        <option value="1">User 1</option>
        <option value="2">User 2</option>
      </select>
      
      <Suspense fallback={<UserLoading />}>
        <UserContent />
      </Suspense>
    </div>
  );
}
```

### Error Handling

```typescript
// atoms/productsAtoms.ts
import { atom } from "jotai";

export const productsAtom = atom(async (get) => {
  const response = await fetch("/api/products");
  
  if (!response.ok) {
    throw new Error("Failed to fetch products");
  }
  
  return response.json();
});
```

```typescript
// components/ProductList.tsx
"use client";

import { useAtomValue } from "jotai";
import { productsAtom } from "@/atoms/productsAtoms";

function Products() {
  const products = useAtomValue(productsAtom);
  
  return (
    <ul>
      {products.map((p) => (
        <li key={p.id}>{p.name}</li>
      ))}
    </ul>
  );
}

function ErrorBoundary({ children }: { children: React.ReactNode }) {
  return (
    <ErrorBoundaryInner>
      {children}
    </ErrorBoundaryInner>
  );
}

function ErrorBoundaryInner({ children }: { children: React.ReactNode }) {
  // This uses Jotai's error handling
  try {
    return <>{children}</>;
  } catch (error) {
    return <div>Error loading products</div>;
  }
}

export default function ProductPage() {
  return (
    <ErrorBoundary>
      <Suspense fallback={<div>Loading products...</div>}>
        <Products />
      </Suspense>
    </ErrorBoundary>
  );
}
```

### Multiple Async Atoms

```typescript
// atoms/dashboardAtoms.ts
import { atom } from "jotai";

export const userAtom = atom(async (get) => {
  const response = await fetch("/api/me");
  return response.json();
});

export const statsAtom = atom(async (get) => {
  const response = await fetch("/api/stats");
  return response.json();
});

export const notificationsAtom = atom(async (get) => {
  const response = await fetch("/api/notifications");
  return response.json();
});
```

```typescript
// components/Dashboard.tsx
"use client";

import { Suspense } from "react";
import { useAtomValue } from "jotai";
import { userAtom, statsAtom, notificationsAtom } from "@/atoms/dashboardAtoms";

function UserWidget() {
  const user = useAtomValue(userAtom);
  return <div>Welcome, {user.name}</div>;
}

function StatsWidget() {
  const stats = useAtomValue(statsAtom);
  return <div>Active: {stats.active}</div>;
}

function NotificationsWidget() {
  const notifications = useAtomValue(notificationsAtom);
  return <div>{notifications.length} new notifications</div>;
}

export default function Dashboard() {
  return (
    <div className="dashboard">
      <Suspense fallback={<div>Loading...</div>}>
        <UserWidget />
      </Suspense>
      
      <Suspense fallback={<div>Loading stats...</div>}>
        <StatsWidget />
      </Suspense>
      
      <Suspense fallback={<div>Loading notifications...</div>}>
        <NotificationsWidget />
      </Suspense>
    </div>
  );
}
```

### Suspense for Data Fetching

```typescript
// components/PageData.tsx
"use client";

import { Suspense } from "react";
import { useAtom } from "jotai";

const dataAtom = atom(async () => {
  await new Promise((resolve) => setTimeout(resolve, 1000));
  return { message: "Data loaded!" };
});

function DataDisplay() {
  const [data] = useAtom(dataAtom);
  return <div>{data.message}</div>;
}

export default function Page() {
  return (
    <main>
      <h1>My Page</h1>
      
      <Suspense fallback={<p>Loading data...</p>}>
        <DataDisplay />
      </Suspense>
    </main>
  );
}
```

## Common Mistakes

### Mistake 1: Not Using Suspense

```typescript
// WRONG - No Suspense, errors out while loading
function User() {
  const [user] = useAtom(userAtom); // Still loading!
  return <div>{user.name}</div>; // Crashes!
}

// CORRECT - Wrap in Suspense
function User() {
  const [user] = useAtom(userAtom);
  return <div>{user.name}</div>;
}

function Page() {
  return (
    <Suspense fallback={<Loading />}>
      <User />
    </Suspense>
  );
}
```

### Mistake 2: Catching Errors Incorrectly

```typescript
// WRONG - Try/catch doesn't work with Suspense
async function User() {
  try {
    const [user] = useAtom(userAtom);
    return <div>{user.name}</div>;
  } catch (e) {
    return <div>Error!</div>;
  }
}

// CORRECT - Use Error Boundary component
function ErrorFallback({ error }) {
  return <div>Error: {error.message}</div>;
}

function Page() {
  return (
    <ErrorBoundary>
      <Suspense fallback={<Loading />}>
        <User />
      </Suspense>
    </ErrorBoundary>
  );
}
```

### Mistake 3: Not Handling null Initial State

```typescript
// WRONG - Atom returns null initially
export const userAtom = atom(async (get) => {
  const id = get(userIdAtom);
  if (!id) return null; // null while loading!
  return fetchUser(id);
});

// In component - need to handle three states: loading, null, data
const [user] = useAtom(userAtom);
// user can be: Promise (loading), null, or actual data

// CORRECT - Don't return null, throw error or return empty
export const userAtom = atom(async (get) => {
  const id = get(userIdAtom);
  if (!id) throw new Error("No user ID"); // Or return []
  return fetchUser(id);
});
```

## Summary

- Async atoms work with React Suspense automatically
- Wrap atoms in Suspense to show fallback while loading
- Use ErrorBoundary for error handling
- Each async atom can have its own Suspense boundary
- Loading states are automatic - no manual isLoading needed

## Next Steps

- [atoms-and-derived-state.md](./atoms-and-derived-state.md) - Advanced atom patterns
- [zustand-with-server-components.md](../02-zustand/zustand-with-server-components.md) - Zustand with server data
