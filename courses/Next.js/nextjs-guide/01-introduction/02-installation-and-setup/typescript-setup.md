# TypeScript Setup in Next.js

## What You'll Learn
- What TypeScript is and why Next.js uses it by default
- How TypeScript helps you write better code
- Basic TypeScript types you'll use in Next.js

## Prerequisites
- A Next.js project created with TypeScript (the default)
- Basic JavaScript knowledge

## Concept Explained Simply

**TypeScript** is like JavaScript with a superpower: it catches mistakes before you run your code. Think of it like spell-check for your code. When you're writing, it highlights typos and suggests corrections. This prevents bugs from reaching your users.

JavaScript is like driving without a seatbelt — you can do it, but it's risky. TypeScript is like having all the safety features: airbags, seatbelts, and collision warnings. It adds a small amount of extra work upfront but saves massive headaches later.

Next.js chose TypeScript as the default because it makes your code more reliable and helps you understand what data flows through your app. When you see `string` or `number` in code, you know exactly what type of data to expect.

## How TypeScript Works in Next.js

When you create a Next.js app with TypeScript, you get:

1. **Type checking** — Catches errors before runtime
2. **Auto-completion** — Your editor suggests what you might want to type
3. **Documentation** — Types serve as documentation for other developers
4. **Refactoring safety** — Change code with confidence

## Complete Code Example

Here's a typical Next.js page with TypeScript:

```typescript
// src/app/page.tsx
import Image from "next/image";

// Define the shape of our product data
interface Product {
  id: number;
  name: string;
  price: number;
  inStock: boolean;
}

// This is an async Server Component
async function getProducts(): Promise<Product[]> {
  // Simulating an API call
  const res = await fetch('https://api.example.com/products', {
    cache: 'no-store'
  });
  
  if (!res.ok) {
    throw new Error('Failed to fetch products');
  }
  
  return res.json();
}

export default async function Home() {
  // Fetch data on the server
  const products = await getProducts();

  return (
    <main className="p-8">
      <h1 className="text-3xl font-bold mb-6">Our Products</h1>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {products.map((product: Product) => (
          <div key={product.id} className="border p-4 rounded-lg">
            <h2 className="text-xl font-semibold">{product.name}</h2>
            <p className="text-gray-600">${product.price}</p>
            <p className={product.inStock ? 'text-green-600' : 'text-red-600'}>
              {product.inStock ? 'In Stock' : 'Out of Stock'}
            </p>
          </div>
        ))}
      </div>
    </main>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `interface Product` | Defines a TypeScript interface | Describes the shape of your data |
| `id: number` | Number type property | Ensures id is always a number |
| `name: string` | String type property | Ensures name is always text |
| `async function getProducts()` | Async function that returns a Promise | Required for server-side data fetching |
| `: Promise<Product[]>` | Return type annotation | Tells TypeScript this returns an array of Products |
| `await fetch(...)` | Fetches data from an API | Standard web API for making requests |
| `{ cache: 'no-store' }` | Cache option | Tells Next.js not to cache this request |
| `throw new Error(...)` | Throws an error | Server Components can throw errors |
| `async function Home()` | Async page component | Server Components can be async |
| `{products.map((product: Product) => ...)}` | Maps over products with type | TypeScript knows the shape of each product |

## Common TypeScript Types You'll Use

```typescript
// Primitives
let name: string = "John";        // Text
let age: number = 25;             // Numbers
let isActive: boolean = true;     // true/false

// Arrays
let items: string[] = ["a", "b"]; // Array of strings
let nums: number[] = [1, 2, 3];   // Array of numbers

// Objects
interface User {
  id: number;
  name: string;
  email: string;
}

let user: User = {
  id: 1,
  name: "John",
  email: "john@example.com"
};

// Nullable types
let maybe: string | null = null;  // Can be string or null
```

## Common Mistakes

### Mistake #1: Ignoring Type Errors

Never ignore TypeScript errors. They're there to help you:

```typescript
// ✗ Wrong: Ignoring the error
const name: any = "John";  // "any" defeats the purpose

// ✓ Correct: Use proper types
const name: string = "John";
```

### Mistake #2: Not Defining Return Types

Always define return types for functions:

```typescript
// ✗ Wrong: No return type
function getData() {
  return fetchData();
}

// ✓ Correct: Return type defined
function getData(): Promise<Data[]> {
  return fetchData();
}
```

### Mistake #3: Using 'any' Type

Avoid `any` — it disables TypeScript's checking:

```typescript
// ✗ Wrong: Using any defeats TypeScript
const something: any = getWhatever();

// ✓ Correct: Define the actual type
const something: Product = getWhatever();
```

## Summary

- TypeScript adds type checking to JavaScript
- It catches errors before your code runs
- Next.js uses TypeScript by default (choose "Yes" when creating your app)
- Use `interface` to define object shapes
- Avoid `any` — it defeats the purpose of TypeScript

## Next Steps

Now that you understand the project structure and TypeScript, let's build your first actual page:

- [Your First Page →](../03-your-first-page/hello-world.md)
