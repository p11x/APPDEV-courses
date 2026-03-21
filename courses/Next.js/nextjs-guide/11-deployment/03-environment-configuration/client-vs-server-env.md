# Client vs Server Environment Variables

## What You'll Learn
- Understand which environment variables are exposed to the browser
- Learn how to properly prefix server-only variables
- Secure your sensitive keys and secrets

## Prerequisites
- Understanding of what environment variables are
- Basic knowledge of Next.js App Router structure
- Familiarity with how client and server components differ

## Concept Explained Simply

Think of environment variables like different types of mail:
- **Server-only variables** are like sealed letters that only you (the server) can open
- **Client-exposed variables** are like postcards — anyone can read what's written on them

In Next.js, any environment variable that doesn't start with `NEXT_PUBLIC_` is kept secret on the server. This is incredibly important for security. Your database passwords, API keys for backend services, and encryption keys should NEVER be exposed to the client.

Here's the golden rule: If your frontend code needs to use it, it will be sent to the browser. That means hackers can see it too. So keep your secrets on the server side, and only expose what you intentionally want the public to see.

## Complete Code Example

### Setting Up Environment Variables

```typescript
// .env.local file
# Server-only (never sent to client)
DATABASE_URL="postgres://user:password@localhost:5432/mydb"
STRIPE_SECRET_KEY="sk_test_12345"
JWT_SECRET="super-secret-key-123"

# Client-safe (exposed to browser)
NEXT_PUBLIC_APP_NAME="My Awesome App"
NEXT_PUBLIC_API_URL="https://api.example.com"
NEXT_PUBLIC_FEATURE_FLAG="true"
```

```typescript
// Using in Server Components (safe for secrets)
import { db } from "@/lib/db";

export default async function ServerPage() {
  // This runs only on server - DATABASE_URL is safe here
  const users = await db.user.findMany();
  return <div>Found {users.length} users</div>;
}
```

```typescript
// Using in Client Components
"use client";

export default function ClientComponent() {
  // NEXT_PUBLIC_ variables are exposed to client
  const appName = process.env.NEXT_PUBLIC_APP_NAME;
  const apiUrl = process.env.NEXT_PUBLIC_API_URL;
  
  return (
    <div>
      <h1>Welcome to {appName}</h1>
      <p>API: {apiUrl}</p>
    </div>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `DATABASE_URL=` | Defines a server-only environment variable | Variables without `NEXT_PUBLIC_` prefix stay on server |
| `NEXT_PUBLIC_APP_NAME=` | Exposes variable to client | The `NEXT_PUBLIC_` prefix makes it accessible in browser |
| `"use client"` | Marks this as a Client Component | Required when using environment variables in interactive components |
| `process.env.VAR_NAME` | Accesses environment variable in code | Works in both server and client contexts |

## Common Mistakes

### Mistake 1: Exposing Secrets to Client

```typescript
// WRONG - Secret key exposed to browser!
const stripeKey = process.env.STRIPE_SECRET_KEY;
// This will fail because STRIPE_SECRET_KEY is sent to browser

// CORRECT - Use server-side only
async function createPaymentIntent() {
  "use server";
  const stripeKey = process.env.STRIPE_SECRET_KEY; // Safe on server
  // ... stripe logic
}
```

### Mistake 2: Forgetting the Prefix

```typescript
// WRONG - Variable won't be accessible in client
const apiUrl = process.env.API_URL; // Returns undefined in browser

// CORRECT - Add NEXT_PUBLIC_ prefix
const apiUrl = process.env.NEXT_PUBLIC_API_URL; // Works in client
```

### Mistake 3: Using Server Variables in Client Components

```typescript
// WRONG - database URL exposed to client
"use client";
import { db } from "@/lib/db"; // This might expose server-side code

export default function Page() {
  const users = db.user.findMany(); // DON'T do this
  return <div>{users.length}</div>;
}
```

## Summary

- Always prefix client-safe variables with `NEXT_PUBLIC_`
- Keep sensitive keys (database passwords, API secrets) server-only
- Use Server Actions to safely access secrets from client code
- Test your app in production to ensure secrets aren't leaked
- Environment variables are replaced at build time in static exports

## Next Steps

- [secrets-management.md](./secrets-management.md) - Learn best practices for managing secrets in production
- [deploying-to-vercel.md](../01-vercel/deploying-to-vercel.md) - See how Vercel handles environment variables
