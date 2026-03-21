# Jotai Setup

## What You'll Learn
- Installing Jotai
- Creating atoms
- Using atoms in components
- Understanding Jotai basics

## Prerequisites
- Understanding of React hooks
- Basic TypeScript knowledge
- Familiarity with state concepts

## Concept Explained Simply

Jotai is another state management library, but it takes a different approach than Zustand. Instead of one big store, Jotai uses "atoms" — small, individual pieces of state that can be composed together. It's like having many small boxes instead of one big trunk. Each atom is independent, and components subscribe only to the atoms they need.

Think of it like LEGO: you have individual blocks (atoms) that you can combine in different ways. Need a counter? Grab the count atom. Need a theme? Grab the theme atom. Mix and match as needed.

## Complete Code Example

### Installation

```bash
npm install jotai
# or
yarn add jotai
```

### Creating Atoms

```typescript
// atoms/countAtom.ts
import { atom } from "jotai";

// Simple atom
export const countAtom = atom(0);

// Atom with read function
export const doubleCountAtom = atom((get) => get(countAtom) * 2);

// Writeable atom with getter and setter
export const incrementAtom = atom(
  (get) => get(countAtom), // read
  (get, set) => set(countAtom, get(countAtom) + 1) // write
);
```

```typescript
// atoms/themeAtom.ts
import { atom } from "jotai";

export type Theme = "light" | "dark";

export const themeAtom = atom<Theme>("light");
```

```typescript
// atoms/userAtoms.ts
import { atom } from "jotai";

export interface User {
  id: string;
  name: string;
  email: string;
}

// Async atom for user data
export const userAtom = atom<User | null>(null);

export const userLoadingAtom = atom(false);
```

### Using Atoms in Components

```typescript
// components/Counter.tsx
"use client";

import { useAtom } from "jotai";
import { countAtom } from "@/atoms/countAtom";

export default function Counter() {
  const [count, setCount] = useAtom(countAtom);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(count + 1)}>Increment</button>
    </div>
  );
}
```

```typescript
// components/ThemeToggle.tsx
"use client";

import { useAtom } from "jotai";
import { themeAtom } from "@/atoms/themeAtom";

export default function ThemeToggle() {
  const [theme, setTheme] = useAtom(themeAtom);
  
  return (
    <button onClick={() => setTheme(theme === "light" ? "dark" : "light")}>
      Current: {theme}
    </button>
  );
}
```

### Derived Atoms

```typescript
// atoms/cartAtoms.ts
import { atom } from "jotai";

export interface CartItem {
  id: string;
  name: string;
  price: number;
  quantity: number;
}

// Base atoms
export const cartItemsAtom = atom<CartItem[]>([]);

// Derived atoms (computed from base)
export const cartCountAtom = atom((get) => 
  get(cartItemsAtom).reduce((sum, item) => sum + item.quantity, 0)
);

export const cartTotalAtom = atom((get) => 
  get(cartItemsAtom).reduce((sum, item) => sum + item.price * item.quantity, 0)
);

export const isCartEmptyAtom = atom((get) => 
  get(cartItemsAtom).length === 0
);
```

```typescript
// components/CartSummary.tsx
"use client";

import { useAtom } from "jotai";
import { cartCountAtom, cartTotalAtom } from "@/atoms/cartAtoms";

export default function CartSummary() {
  const [count] = useAtom(cartCountAtom);
  const [total] = useAtom(cartTotalAtom);
  
  return (
    <div>
      <p>Items: {count}</p>
      <p>Total: ${total.toFixed(2)}</p>
    </div>
  );
}
```

### Split Atoms

```typescript
// Using split atoms for performance
"use client";

import { useAtom } from "jotai";
import { cartItemsAtom } from "@/atoms/cartAtoms";

export default function CartList() {
  // Only re-renders when items change
  const [items] = useAtom(cartItemsAtom);
  
  return (
    <ul>
      {items.map((item) => (
        <li key={item.id}>{item.name}</li>
      ))}
    </ul>
  );
}
```

## Common Mistakes

### Mistake 1: Not Using Client Components

```typescript
// WRONG - Using atom in Server Component
// app/page.tsx
import { useAtom } from "jotai";
import { countAtom } from "@/atoms";

// Won't work - atoms need client context!

// CORRECT - Client component
// components/Counter.tsx
"use client";
import { useAtom } from "jotai";
import { countAtom } from "@/atoms";
```

### Mistake 2: Creating Atoms Inside Components

```typescript
// WRONG - Creates new atom on every render
function Counter() {
  const countAtom = atom(0); // New atom each render!
  const [count, setCount] = useAtom(countAtom);
}

// CORRECT - Define atoms outside component
// atoms/count.ts
export const countAtom = atom(0);

// components/Counter.tsx
function Counter() {
  const [count, setCount] = useAtom(countAtom);
}
```

### Mistake 3: Overusing Derived Atoms

```typescript
// WRONG - Deriving everything
const derivedAtom = atom((get) => get(base) * 2);

// CORRECT - Compute in component if simple
function Component() {
  const [value] = useAtom(baseAtom);
  const doubled = value * 2; // Simple derivation
  // ...
}
```

## Summary

- Install Jotai with `npm install jotai`
- Define atoms outside components with `atom(initialValue)`
- Use `useAtom(atom)` to read/write atoms
- Derived atoms auto-compute from base atoms
- Jotai is well-suited for React Server Components compatibility
- Use Client Components when using useAtom

## Next Steps

- [atoms-and-derived-state.md](./atoms-and-derived-state.md) - Advanced atom patterns
- [jotai-with-suspense.md](./jotai-with-suspense.md) - Using Jotai with Suspense
