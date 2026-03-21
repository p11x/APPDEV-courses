# The "use client" Directive Explained

## What You'll Learn
- What "use client" does
- How to use it correctly
- Understanding the boundary concept

## Prerequisites
- Understanding of Server and Client Components
- Basic React knowledge

## Concept Explained Simply

The `"use client"` directive is a special instruction that tells Next.js a component should run on the client (browser) instead of the server. It's placed at the very top of your file, before any imports.

Think of it like a border gate between two countries. Server Components are in one country (the server), and Client Components are in another (the browser). The "use client" directive is like opening a gate — it tells React, "this component needs to cross over to the client side."

Once you add "use client" to a component, that component and all its children become client components. This is called the "client boundary."

## Why It Exists

In traditional React, all components run on the client (in the browser). With Server Components, most components run on the server by default. But you need a way to opt into client-side behavior when you need:
- Interactivity (clicks, forms)
- React hooks (useState, useEffect)
- Browser APIs

That's what "use client" does — it opts a component into client-side rendering.

## Complete Code Example

Let's see how "use client" works in different scenarios:

```typescript
// src/components/Counter.tsx - A Client Component
"use client";  // ← Must be at the very top!

import { useState } from "react";

export function Counter() {
  const [count, setCount] = useState(0);

  return (
    <div style={{ padding: "1rem" }}>
      <p style={{ fontSize: "1.5rem", marginBottom: "1rem" }}>
        Count: {count}
      </p>
      <button
        onClick={() => setCount(count + 1)}
        style={{
          padding: "0.5rem 1rem",
          backgroundColor: "#0070f3",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: "pointer",
        }}
      >
        Increment
      </button>
    </div>
  );
}
```

```typescript
// src/components/InteractiveCard.tsx - Contains both types
"use client";  // This whole file is a Client Component

import { useState } from "react";

export function InteractiveCard({ children }: { children: React.ReactNode }) {
  const [isExpanded, setIsExpanded] = useState(false);

  return (
    <div
      onClick={() => setIsExpanded(!isExpanded)}
      style={{
        padding: "1rem",
        border: "1px solid #ddd",
        borderRadius: "8px",
        cursor: "pointer",
        transition: "all 0.3s ease",
        backgroundColor: isExpanded ? "#f0f0f0" : "white",
      }}
    >
      <div style={{ transform: isExpanded ? "rotate(180deg)" : "rotate(0)" }}>
        ▼
      </div>
      {children}
    </div>
  );
}
```

```typescript
// src/app/page.tsx - Server Component (no "use client")
import { Counter } from "@/components/Counter";
import { InteractiveCard } from "@/components/InteractiveCard";

export default function HomePage() {
  return (
    <main style={{ padding: "2rem" }}>
      <h1>Server vs Client Demo</h1>
      
      {/* Server Component - renders on server, no JS sent */}
      <section style={{ marginBottom: "2rem" }}>
        <h2>Server Component</h2>
        <p>This page runs on the server!</p>
        <p>No JavaScript is sent to the browser for this content.</p>
      </section>

      {/* Client Component - interactive, JS sent */}
      <section style={{ marginBottom: "2rem" }}>
        <h2>Client Component</h2>
        <Counter />
      </section>

      {/* Client Component with children */}
      <section>
        <h2>Interactive Card</h2>
        <InteractiveCard>
          <p>Click to expand!</p>
        </InteractiveCard>
      </section>
    </main>
  );
}
```

## The Boundary Concept

Once you add "use client", you've created a boundary:

```
┌─────────────────────────────────────────┐
│           Server Component              │
│                                         │
│  ┌─────────────────────────────────┐    │
│  │   "use client" Component       │    │
│  │   (and all its children)       │    │
│  │                                 │    │
│  │   - Runs in browser            │    │
│  │   - Has JavaScript            │    │
│  │   - Interactive               │    │
│  └─────────────────────────────────┘    │
│                                         │
└─────────────────────────────────────────┘
```

## Where to Put "use client"

You only need "use client" on components that directly need:
- React hooks (useState, useEffect, useContext, etc.)
- Event handlers (onClick, onChange, etc.)
- Browser APIs (window, document)

```typescript
// ✗ Wrong: Adding to component that doesn't need it
"use client";  // Unnecessary!

export function DisplayOnly({ text }) {
  return <p>{text}</p>;  // Just displays, no interactivity
}

// ✓ Correct: Server Component is fine
export function DisplayOnly({ text }) {
  return <p>{text}</p>;
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `"use client"` | Directive | Marks file as Client Component |
| `import { useState }` | Hook import | Only available in client |
| `onClick` handler | Event | Only works in client |
| `useEffect` | Side effect | Only runs in client |

## Common Mistakes

### Mistake #1: Not Adding "use client" When Needed

```typescript
// ✗ Wrong: Using hooks without "use client"
import { useState } from "react";

export function Counter() {
  const [count, setCount] = useState(0); // Error!
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}

// ✓ Correct: Add "use client"
"use client";

import { useState } from "react";

export function Counter() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}
```

### Mistake #2: Adding "use client" to Wrong File

```typescript
// ✗ Wrong: Adding to the page instead of the component
// src/app/page.tsx
"use client";  // Wrong place!

import { Counter } from "@/components/Counter";

export default function Page() {
  return <Counter />;
}

// ✓ Correct: Add to the component that needs it
// src/components/Counter.tsx
"use client";

import { useState } from "react";

export function Counter() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}
```

### Mistake #3: Forgetting It's Contagious

```typescript
// ✗ Wrong: All children become client components
"use client";

export function Parent() {
  return (
    <>
      <ServerChild />  // This also becomes client!
      <AnotherChild /> // And this!
    </>
  );
}

// ✓ Correct: Keep server/display components separate
"use client";

export function InteractiveParent({ children }) {
  return <div onClick={() => alert("hi")}>{children}</div>;
}

// In a different file - Server Component
export function TextDisplay({ text }) {
  return <p>{text}</p>;
}

// Usage - Server Component passes server child to client parent
export default function Page() {
  return (
    <InteractiveParent>
      <TextDisplay text="This stays server!" />
    </InteractiveParent>
  );
}
```

## Summary

- "use client" marks a component to run on the browser
- Must be at the very top of the file
- Only needed for components with hooks or event handlers
- Once added, all children become client components
- Keep server-only components in separate files

## Next Steps

Now let's learn about async Server Components:

- [Async Server Components →](../../03-react-server-components/02-data-fetching-in-rsc/async-server-components.md)
