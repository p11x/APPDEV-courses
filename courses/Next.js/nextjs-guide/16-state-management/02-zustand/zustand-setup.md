# Zustand Setup

## What You'll Learn
- Installing Zustand
- Creating your first store
- Using the store in components
- Understanding Zustand basics

## Prerequisites
- Understanding of React hooks
- Basic TypeScript knowledge
- Familiarity with Client Components

## Concept Explained Simply

Zustand is a small state management library that's simpler than Redux but more powerful than useState. It's like having a shared notepad that any component can write to or read from. The key benefit: you don't need to wrap your app in providers.

Think of Zustand like a whiteboard in your office. Anyone can walk up to it, read what's written, or write something new. It's simple, direct, and doesn't require any setup ceremonies.

## Complete Code Example

### Installation

```bash
npm install zustand
# or
yarn add zustand
# or
pnpm add zustand
```

### Creating a Store

```typescript
// store/useCartStore.ts
import { create } from "zustand";

interface CartItem {
  id: string;
  name: string;
  price: number;
  quantity: number;
}

interface CartState {
  items: CartItem[];
  isOpen: boolean;
  addItem: (item: Omit<CartItem, "quantity">) => void;
  removeItem: (id: string) => void;
  updateQuantity: (id: string, quantity: number) => void;
  clearCart: () => void;
  toggleCart: () => void;
}

export const useCartStore = create<CartState>((set) => ({
  items: [],
  isOpen: false,
  
  addItem: (item) => set((state) => {
    const existing = state.items.find((i) => i.id === item.id);
    
    if (existing) {
      return {
        items: state.items.map((i) =>
          i.id === item.id
            ? { ...i, quantity: i.quantity + 1 }
            : i
        ),
      };
    }
    
    return {
      items: [...state.items, { ...item, quantity: 1 }],
    };
  }),
  
  removeItem: (id) => set((state) => ({
    items: state.items.filter((i) => i.id !== id),
  })),
  
  updateQuantity: (id, quantity) => set((state) => ({
    items: state.items.map((i) =>
      i.id === id ? { ...i, quantity } : i
    ),
  })),
  
  clearCart: () => set({ items: [] }),
  
  toggleCart: () => set((state) => ({ isOpen: !state.isOpen })),
}));
```

### Using the Store

```typescript
// components/CartButton.tsx
"use client";

import { useCartStore } from "@/store/useCartStore";

export default function CartButton() {
  const items = useCartStore((state) => state.items);
  const toggleCart = useCartStore((state) => state.toggleCart);
  
  const itemCount = items.reduce((sum, item) => sum + item.quantity, 0);
  
  return (
    <button onClick={toggleCart}>
      Cart ({itemCount})
    </button>
  );
}
```

```typescript
// components/CartDrawer.tsx
"use client";

import { useCartStore } from "@/store/useCartStore";

export default function CartDrawer() {
  const { items, isOpen, removeItem, clearCart } = useCartStore();
  
  if (!isOpen) return null;
  
  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);
  
  return (
    <div className="cart-drawer">
      <h2>Shopping Cart</h2>
      
      {items.length === 0 ? (
        <p>Your cart is empty</p>
      ) : (
        <ul>
          {items.map((item) => (
            <li key={item.id}>
              <span>{item.name}</span>
              <span>x{item.quantity}</span>
              <span>${item.price * item.quantity}</span>
              <button onClick={() => removeItem(item.id)}>Remove</button>
            </li>
          ))}
        </ul>
      )}
      
      <div>Total: ${total}</div>
      <button onClick={clearCart}>Clear Cart</button>
    </div>
  );
}
```

### Store with Persistence

```typescript
// store/usePreferencesStore.ts
import { create } from "zustand";
import { persist } from "zustand/middleware";

interface PreferencesState {
  theme: "light" | "dark";
  language: string;
  setTheme: (theme: "light" | "dark") => void;
  setLanguage: (language: string) => void;
}

export const usePreferencesStore = create<PreferencesState>()(
  persist(
    (set) => ({
      theme: "light",
      language: "en",
      
      setTheme: (theme) => set({ theme }),
      setLanguage: (language) => set({ language }),
    }),
    {
      name: "preferences-storage", // localStorage key
    }
  )
);
```

### Complete Store Example

```typescript
// store/useUserStore.ts
import { create } from "zustand";

interface User {
  id: string;
  name: string;
  email: string;
}

interface UserState {
  user: User | null;
  isLoading: boolean;
  setUser: (user: User | null) => void;
  setLoading: (loading: boolean) => void;
  logout: () => void;
}

export const useUserStore = create<UserState>((set) => ({
  user: null,
  isLoading: true,
  
  setUser: (user) => set({ user }),
  setLoading: (isLoading) => set({ isLoading }),
  logout: () => set({ user: null }),
}));
```

## Using with TypeScript

```typescript
// Full TypeScript example
import { create } from "zustand";

interface CounterState {
  count: number;
  increment: () => void;
  decrement: () => void;
  reset: () => void;
}

export const useCounterStore = create<CounterState>((set) => ({
  count: 0,
  
  increment: () => set((state) => ({ count: state.count + 1 })),
  decrement: () => set((state) => ({ count: state.count - 1 })),
  reset: () => set({ count: 0 }),
}));
```

## Common Mistakes

### Mistake 1: Selecting Too Much State

```typescript
// WRONG - Re-renders on any store change
const { items, isOpen } = useCartStore();

// CORRECT - Select only what you need
const items = useCartStore((state) => state.items);
const isOpen = useCartStore((state) => state.isOpen);
```

### Mistake 2: Not Using Client Components

```typescript
// WRONG - Using store in Server Component
// app/page.tsx
import { useCartStore } from "@/store";

// This won't work in Server Component!

// CORRECT - Use in Client Components only
// app/page.tsx
import CartButton from "@/components/CartButton";

export default function Page() {
  return <CartButton />; // CartButton is "use client"
}
```

### Mistake 3: Mutating State Directly

```typescript
// WRONG - Direct mutation
addItem: (item) => {
  state.items.push(item); // Don't do this!
},

// CORRECT - Return new state
addItem: (item) => set((state) => ({
  items: [...state.items, item]
})),
```

## Summary

- Install Zustand with `npm install zustand`
- Create stores with `create<State>((set) => ({...}))`
- Use `set()` to update state
- Always return new objects/arrays, never mutate
- Use selectors to prevent unnecessary re-renders
- Use persist middleware for localStorage
- Only use in Client Components ("use client")

## Next Steps

- [zustand-with-server-components.md](./zustand-with-server-components.md) - Combining with server data
- [persisting-state.md](./persisting-state.md) - Persisting state to localStorage
