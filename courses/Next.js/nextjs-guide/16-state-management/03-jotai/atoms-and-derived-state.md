# Atoms and Derived State

## What You'll Learn
- Creating derived atoms
- Composing atoms together
- Atom effects and async atoms
- Advanced patterns

## Prerequisites
- Understanding of Jotai basics
- Knowledge of async/await
- Familiarity with TypeScript

## Concept Explained Simply

Derived state in Jotai is incredibly powerful. Just like Excel formulas that automatically update when you change a cell, derived atoms automatically recalculate when their dependencies change. You never have to manually "sync" state — the atoms figure it out themselves.

Think of it like a chain reaction: when you update one atom, all atoms that depend on it update automatically. No need to manually trigger anything.

## Complete Code Example

### Basic Derived Atoms

```typescript
// atoms/formAtoms.ts
import { atom } from "jotai";

// Base atoms
export const emailAtom = atom("");
export const passwordAtom = atom("");
export const confirmPasswordAtom = atom("");

// Derived atoms
export const emailValidAtom = atom((get) => {
  const email = get(emailAtom);
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
});

export const passwordValidAtom = atom((get) => {
  const password = get(passwordAtom);
  return password.length >= 8;
});

export const passwordsMatchAtom = atom((get) => {
  const password = get(passwordAtom);
  const confirm = get(confirmPasswordAtom);
  return password === confirm;
});

export const formValidAtom = atom((get) => {
  const emailValid = get(emailValidAtom);
  const passwordValid = get(passwordValidAtom);
  const passwordsMatch = get(passwordsMatchAtom);
  
  return emailValid && passwordValid && passwordsMatch;
});
```

### Using in Components

```typescript
// components/SignupForm.tsx
"use client";

import { useAtom } from "jotai";
import { 
  emailAtom, 
  passwordAtom, 
  confirmPasswordAtom,
  emailValidAtom,
  passwordValidAtom,
  passwordsMatchAtom,
  formValidAtom 
} from "@/atoms/formAtoms";

export default function SignupForm() {
  const [email, setEmail] = useAtom(emailAtom);
  const [password, setPassword] = useAtom(passwordAtom);
  const [confirmPassword, setConfirmPassword] = useAtom(confirmPasswordAtom);
  
  const [emailValid] = useAtom(emailValidAtom);
  const [passwordValid] = useAtom(passwordValidAtom);
  const [passwordsMatch] = useAtom(passwordsMatchAtom);
  const [formValid] = useAtom(formValidAtom);
  
  return (
    <form>
      <div>
        <input
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          type="email"
          placeholder="Email"
        />
        {!emailValid && email && <span>Invalid email</span>}
      </div>
      
      <div>
        <input
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          type="password"
          placeholder="Password"
        />
        {!passwordValid && password && <span>Password too short</span>}
      </div>
      
      <div>
        <input
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          type="password"
          placeholder="Confirm password"
        />
        {!passwordsMatch && confirmPassword && <span>Passwords don't match</span>}
      </div>
      
      <button type="submit" disabled={!formValid}>
        Sign Up
      </button>
    </form>
  );
}
```

### Async Atoms

```typescript
// atoms/dataAtoms.ts
import { atom } from "jotai";

// Async atom for fetching data
export const userIdAtom = atom("");

export const userDataAtom = atom(async (get) => {
  const userId = get(userIdAtom);
  
  if (!userId) return null;
  
  const response = await fetch(`/api/users/${userId}`);
  return response.json();
});

// Atom with error handling
export const userDataWithErrorAtom = atom(
  async (get) => {
    const userId = get(userIdAtom);
    const response = await fetch(`/api/users/${userId}`);
    
    if (!response.ok) {
      throw new Error("Failed to fetch user");
    }
    
    return response.json();
  }
);
```

### Atom Effects

```typescript
// atoms/effects.ts
import { atom } from "jotai";
import { atomEffect } from "jotai/utils";

export const themeAtom = atom("light");

// Side effect: sync to localStorage
export const themeEffect = atomEffect((get) => {
  const theme = get(themeAtom);
  localStorage.setItem("theme", theme);
});

// Side effect: sync to server
export const analyticsEffect = atomEffect((get) => {
  const action = get(actionAtom);
  
  // Send to analytics
  fetch("/api/analytics", {
    method: "POST",
    body: JSON.stringify({ action }),
  });
});
```

### Composing Derived Atoms

```typescript
// atoms/shoppingAtoms.ts
import { atom } from "jotai";

export interface Product {
  id: string;
  name: string;
  price: number;
  category: string;
}

export const productsAtom = atom<Product[]>([]);
export const categoryFilterAtom = atom("all");
export const sortByAtom = atom<"name" | "price">("name");

// Filtered products
export const filteredProductsAtom = atom((get) => {
  const products = get(productsAtom);
  const category = get(categoryFilterAtom);
  
  if (category === "all") return products;
  
  return products.filter((p) => p.category === category);
});

// Sorted products
export const sortedProductsAtom = atom((get) => {
  const products = get(filteredProductsAtom);
  const sortBy = get(sortByAtom);
  
  return [...products].sort((a, b) => {
    if (sortBy === "name") {
      return a.name.localeCompare(b.name);
    }
    return a.price - b.price;
  });
});
```

## Common Mistakes

### Mistake 1: Not Handling Async Loading

```typescript
// WRONG - No loading state
export const userAtom = atom(async (get) => {
  const data = await fetchData();
  return data;
});

// In component - will error while loading!
const [user] = useAtom(userAtom);
console.log(user.name); // Error if still loading!

// CORRECT - Handle loading with Suspense
export const userAtom = atom(async (get) => {
  const data = await fetchData();
  return data;
});

// In component wrapped in Suspense
function User() {
  const [user] = useAtom(userAtom);
  return <div>{user.name}</div>;
}
```

### Mistake 2: Circular Dependencies

```typescript
// WRONG - Atoms depend on each other
const atomA = atom((get) => get(atomB));
const atomB = = atom((get) => get(atomA));

// CORRECT - No circular dependencies
const baseAtom = atom(0);
const derivedA = atom((get) => get(baseAtom) * 2);
const derivedB = atom((get) => get(baseAtom) * 3);
```

### Mistake 3: Expensive Computations

```typescript
// WRONG - Heavy computation in derived atom
const expensiveAtom = atom((get) => {
  const data = get(dataAtom);
  return data.sort(() => Math.random() - 0.5).slice(0, 5); // Random every time!
});

// CORRECT - Use useMemo in component or refactor
```

## Summary

- Derived atoms automatically recalculate when dependencies change
- Use async atoms for data fetching with Suspense
- Compose atoms together for complex derived state
- Use atomEffect for side effects
- Handle loading and error states appropriately

## Next Steps

- [jotai-with-suspense.md](./jotai-with-suspense.md) - Using Jotai with Suspense
- [jotai-setup.md](./jotai-setup.md) - Review Jotai basics
