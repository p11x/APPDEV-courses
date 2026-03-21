# Server vs Client State

## What You'll Learn
- Understanding different types of state
- When to use server state vs client state
- Managing state in App Router
- Avoiding hydration mismatches

## Prerequisites
- Understanding of React state
- Knowledge of Server Components
- Familiarity with data fetching

## Concept Explained Simply

Not all state is the same. Imagine a restaurant: the menu (server state) is printed and shared with everyone, while the specific items on your table (client state) are unique to you. Understanding this difference helps you choose the right tools.

**Server State** — Data that comes from your database or API. It's shared, can become stale, and typically needs to be refreshed.

**Client State** — Data that exists only in the user's browser. Their theme preference, form inputs, or shopping cart.

## Types of State

| Type | Source | Example | Tool |
|------|--------|---------|------|
| Server State | Database/API | User data, posts | fetch, React Query |
| Client State | Browser only | Theme, form inputs | useState, Zustand |
| URL State | URL params | Filters, page | useSearchParams |
| Static State | Code | App config | const values |

## Complete Code Example

### Server State Patterns

```typescript
// app/posts/page.tsx - Server Component fetches data
import { db } from "@/lib/db";

export default async function PostsPage() {
  // This is server state - fetched on the server
  const posts = await db.post.findMany({
    include: { author: true },
  });
  
  return (
    <div>
      {posts.map((post) => (
        <PostCard key={post.id} post={post} />
      ))}
    </div>
  );
}
```

```typescript
// components/PostList.tsx - Client component for interactivity
"use client";

import { useState } from "react";

export default function PostList({ initialPosts }) {
  // Client state - just for this component
  const [filter, setFilter] = useState("all");
  
  // Filter initial posts (server state)
  const filteredPosts = filter === "all" 
    ? initialPosts 
    : initialPosts.filter(p => p.category === filter);
  
  return (
    <div>
      <select value={filter} onChange={(e) => setFilter(e.target.value)}>
        <option value="all">All</option>
        <option value="tech">Tech</option>
      </select>
      {filteredPosts.map(post => (
        <PostCard key={post.id} post={post} />
      ))}
    </div>
  );
}
```

### Client State Patterns

```typescript
// components/ThemeToggle.tsx
"use client";

import { useState, useEffect } from "react";

export default function ThemeToggle() {
  // Client-only state
  const [theme, setTheme] = useState("light");
  const [mounted, setMounted] = useState(false);
  
  // Only show after hydration to avoid mismatch
  useEffect(() => {
    setMounted(true);
    setTheme(localStorage.getItem("theme") || "light");
  }, []);
  
  if (!mounted) return null;
  
  return (
    <button onClick={() => {
      const newTheme = theme === "light" ? "dark" : "light";
      setTheme(newTheme);
      localStorage.setItem("theme", newTheme);
    }}>
      Current: {theme}
    </button>
  );
}
```

### Avoiding Hydration Mismatch

```typescript
// WRONG - Causes hydration error
"use client";

import { useState, useEffect } from "react";

export default function Component() {
  const [value, setValue] = useState("");
  
  useEffect(() => {
    setValue(localStorage.getItem("key") || "");
  }, []);
  
  // Render differs on server vs client!
  return <div>{value}</div>;
}

// CORRECT - Handle mounting properly
"use client";

import { useState, useEffect } from "react";

export default function Component() {
  const [value, setValue] = useState("");
  const [mounted, setMounted] = useState(false);
  
  useEffect(() => {
    setValue(localStorage.getItem("key") || "");
    setMounted(true);
  }, []);
  
  // Don't render anything until mounted
  if (!mounted) {
    return null; // or a loading skeleton
  }
  
  return <div>{value}</div>;
}
```

### Mixing Server and Client State

```typescript
// app/dashboard/page.tsx - Server Component
import { db } from "@/lib/db";
import DashboardContent from "./DashboardContent";

export default async function DashboardPage() {
  // Fetch server state
  const user = await db.user.findUnique({ /* ... */ });
  const stats = await db.stat.findMany();
  
  return (
    // Pass to client component for interactivity
    <DashboardContent user={user} stats={stats} />
  );
}
```

```typescript
// app/dashboard/DashboardContent.tsx - Client Component
"use client";

import { useState } from "react";

export default function DashboardContent({ user, stats }) {
  // user and stats are INITIAL server state
  // This component adds client interactivity
  
  const [view, setView] = useState("grid");
  
  return (
    <div>
      <h1>Welcome, {user.name}</h1>
      <StatsChart stats={stats} />
      <ViewToggle value={view} onChange={setView} />
    </div>
  );
}
```

## State Management Decision Tree

```
Do you need this data to be shared?
│
├─ YES - Is it from a database/API?
│   │
│   ├─ YES → Server Component + fetch (or React Query)
│   │
│   └─ NO → URL state (if shareable) or global client state
│
└─ NO - Is it specific to this user/session?
    │
    ├─ YES → Is it simple (theme, toggle)?
    │   └─ YES → useState / useReducer
    │
    └─ NO → Is it complex (multiple components)?
        └─ YES → Zustand / Context
```

## Common Mistakes

### Mistake 1: Fetching in Client Components

```typescript
// WRONG - Client component fetching
"use client";

import { useEffect, useState } from "react";

export default function Posts() {
  const [posts, setPosts] = useState([]);
  
  useEffect(() => {
    fetch("/api/posts").then(res => res.json()).then(setPosts);
  }, []);
  
  return <div>{/* ... */}</div>;
}

// CORRECT - Fetch in Server Component
// app/posts/page.tsx
export default async function PostsPage() {
  const posts = await fetchPosts(); // Server-side
  return <PostsList posts={posts} />;
}
```

### Mistake 2: Not Handling Hydration

```typescript
// WRONG - Hydration mismatch
"use client";

export default function Component() {
  const value = typeof window !== "undefined" 
    ? window.localStorage.getItem("key") 
    : "default";
  
  return <div>{value}</div>; // Server: "default", Client: something else!
}

// CORRECT - Wait for mount
"use client";

import { useState, useEffect } from "react";

export default function Component() {
  const [value, setValue] = useState("default");
  const [mounted, setMounted] = useState(false);
  
  useEffect(() => {
    setValue(localStorage.getItem("key") || "default");
    setMounted(true);
  }, []);
  
  if (!mounted) return null;
  
  return <div>{value}</div>;
}
```

### Mistake 3: Using Client State When Server State Is Better

```typescript
// WRONG - Client-side filtering
"use client";

export default function FilterableList({ items }) {
  const [filter, setFilter] = useState("all");
  
  // Pass everything to client, filter there
  const filtered = items.filter(i => i.category === filter);
  
  return (
    <>
      <Filter value={filter} onChange={setFilter} />
      {filtered.map(item => <Item key={item.id} item={item} />)}
    </>
  );
}

// CORRECT - Server-side filtering via URL
// page.tsx?category=tech
export default async function Page({ searchParams }) {
  const category = searchParams.category;
  const items = await fetchItems(category); // Filter on server
  
  return <ItemList items={items} />;
}
```

## Summary

- Server Components handle server state natively
- Client Components handle user-specific state
- Always handle hydration mismatches with useState + useEffect
- Prefer URL state for shareable filters/sorting
- Use client state only for truly local data
- Mix server and client state by passing data to client components

## Next Steps

- [when-not-to-use-redux.md](./when-not-to-use-redux.md) - Avoiding over-engineering
- [zustand-setup.md](../02-zustand/zustand-setup.md) - Client-side state management
