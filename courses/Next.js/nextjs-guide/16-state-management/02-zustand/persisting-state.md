# Persisting State

## What You'll Learn
- Persisting Zustand state to localStorage
- Handling SSR with persisted state
- Custom storage solutions
- Best practices

## Prerequisites
- Understanding of Zustand
- Knowledge of localStorage
- Familiarity with Client Components

## Concept Explained Simply

Sometimes you need state to survive page refreshes — like user preferences or shopping cart contents. Zustand's persist middleware makes this easy by automatically syncing your store to localStorage (or any storage you want).

Think of it like saving a game: you can close the app and when you come back, your progress is still there. The middleware handles all the saving and loading automatically.

## Complete Code Example

### Basic Persistence

```typescript
// store/useCartStore.ts
import { create } from "zustand";
import { persist } from "zustand/middleware";

interface CartItem {
  id: string;
  name: string;
  price: number;
  quantity: number;
}

interface CartState {
  items: CartItem[];
  addItem: (item: CartItem) => void;
  removeItem: (id: string) => void;
  clearCart: () => void;
}

export const useCartStore = create<CartState>()(
  persist(
    (set) => ({
      items: [],
      
      addItem: (item) => set((state) => {
        const existing = state.items.find((i) => i.id === item.id);
        
        if (existing) {
          return {
            items: state.items.map((i) =>
              i.id === item.id
                ? { ...i, quantity: i.quantity + item.quantity }
                : i
            ),
          };
        }
        
        return { items: [...state.items, item] };
      }),
      
      removeItem: (id) => set((state) => ({
        items: state.items.filter((i) => i.id !== id),
      })),
      
      clearCart: () => set({ items: [] }),
    }),
    {
      name: "cart-storage", // localStorage key
    }
  )
);
```

### Persisting with TypeScript

```typescript
// store/usePreferencesStore.ts
import { create } from "zustand";
import { persist, createJSONStorage } from "zustand/middleware";

interface PreferencesState {
  theme: "light" | "dark";
  language: string;
  notifications: boolean;
  setTheme: (theme: "light" | "dark") => void;
  setLanguage: (language: string) => void;
  toggleNotifications: () => void;
}

export const usePreferencesStore = create<PreferencesState>()(
  persist(
    (set) => ({
      theme: "light",
      language: "en",
      notifications: true,
      
      setTheme: (theme) => set({ theme }),
      setLanguage: (language) => set({ language }),
      toggleNotifications: () => set((state) => ({ 
        notifications: !state.notifications 
      })),
    }),
    {
      name: "preferences-storage",
      storage: createJSONStorage(() => localStorage),
      // Only persist certain keys
      partialize: (state) => ({
        theme: state.theme,
        language: state.language,
      }),
    }
  )
);
```

### Handling SSR with Persistence

```typescript
// components/ThemeToggle.tsx
"use client";

import { useState, useEffect } from "react";
import { usePreferencesStore } from "@/store/usePreferencesStore";

export default function ThemeToggle() {
  const theme = usePreferencesStore((state) => state.theme);
  const setTheme = usePreferencesStore((state) => state.setTheme);
  const [mounted, setMounted] = useState(false);
  
  // Only render after hydration
  useEffect(() => {
    setMounted(true);
  }, []);
  
  if (!mounted) {
    return null; // Prevents hydration mismatch
  }
  
  return (
    <button onClick={() => setTheme(theme === "light" ? "dark" : "light")}>
      Switch to {theme === "light" ? "dark" : "light"}
    </button>
  );
}
```

### Custom Storage

```typescript
// Using sessionStorage instead of localStorage
import { create } from "zustand";
import { persist, createJSONStorage } from "zustand/middleware";

export const useSessionStore = create<any>()(
  persist(
    (set) => ({ /* ... */ }),
    {
      name: "session-storage",
      storage: createJSONStorage(() => sessionStorage),
    }
  )
);
```

```typescript
// Using cookies
import { create } from "zustand";
import { persist } from "zustand/middleware";
import Cookies from "js-cookie";

const cookieStorage = {
  getItem: (name: string) => {
    const value = Cookies.get(name);
    return value ? JSON.parse(value) : null;
  },
  setItem: (name: string, value: any) => {
    Cookies.set(name, JSON.stringify(value), { expires: 365 });
  },
  removeItem: (name: string) => {
    Cookies.remove(name);
  },
};

export const useCookieStore = create<any>()(
  persist(
    (set) => ({ /* ... */ }),
    {
      name: "cookie-storage",
      storage: cookieStorage,
    }
  )
);
```

## Common Mistakes

### Mistake 1: Hydration Mismatch

```typescript
// WRONG - Server renders with default, client with persisted
// Server: theme = "light"
// Client: theme = "dark" (from localStorage)
// React throws hydration error!

// CORRECT - Handle mounting
function ThemeToggle() {
  const [mounted, setMounted] = useState(false);
  
  useEffect(() => setMounted(true), []);
  
  if (!mounted) return null;
  
  // Now safe to render
  return <ThemeButton />;
}
```

### Mistake 2: Storing Non-Serializable Data

```typescript
// WRONG - Can't persist functions or classes
const useStore = create(persist(
  (set) => ({
    // Functions can't be serialized!
    doSomething: () => console.log("action"),
  }),
  { name: "store" }
));

// CORRECT - Store only serializable data
const useStore = create(persist(
  (set) => ({
    actionType: "none", // Store string instead of function
  }),
  { name: "store" }
));
```

### Mistake 3: Persisting Too Much

```typescript
// WRONG - Persisting large data
{
  name: "cart-storage",
  // Stores entire product database!
}

// CORRECT - Only persist necessary data
{
  name: "cart-storage",
  partialize: (state) => ({
    // Only store item IDs and quantities
    items: state.items.map(i => ({ id: i.id, quantity: i.quantity })),
  }),
}
```

## Summary

- Use persist middleware for automatic localStorage sync
- Handle hydration with useState + useEffect
- Only persist serializable data (no functions)
- Use partialize to persist only what's needed
- Consider custom storage (cookies, sessionStorage)
- Always handle the "not mounted" state to prevent errors

## Next Steps

- [atoms-and-derived-state.md](../03-jotai/atoms-and-derived-state.md) - Jotai for atomic state
- [zustand-setup.md](./zustand-setup.md) - Zustand basics review
