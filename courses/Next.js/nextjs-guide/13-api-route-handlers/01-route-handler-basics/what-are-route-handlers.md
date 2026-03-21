# What Are Route Handlers

## What You'll Learn
- Understanding route handlers in Next.js
- How they differ from API routes in the Pages Router
- When to use route handlers vs Server Actions
- Basic route handler structure

## Prerequisites
- Understanding of REST APIs
- Knowledge of HTTP methods (GET, POST, PUT, DELETE)
- Familiarity with Next.js App Router structure

## Concept Explained Simply

Route handlers are like the reception desk at a company. When someone calls (makes a request), the reception desk figures out what they want and connects them to the right person. In web terms, route handlers are functions that receive HTTP requests and send back responses.

In the old Next.js way (Pages Router), you created API routes with files like `pages/api/users.js`. In the new App Router, you create route handlers with files like `app/api/users/route.ts`. The new way is more intuitive — you write regular functions that return data instead of special API functions.

## Complete Code Example

```typescript
// app/api/hello/route.ts
import { NextResponse } from "next/server";

// GET request handler
export async function GET() {
  return NextResponse.json({
    message: "Hello from the API!",
    timestamp: new Date().toISOString(),
  });
}

// POST request handler
export async function POST(request: Request) {
  const body = await request.json();
  
  return NextResponse.json({
    received: body,
    message: "Data received successfully!",
  }, { status: 201 });
}
```

### A More Complete Example

```typescript
// app/api/users/route.ts
import { NextResponse } from "next/server";
import { db } from "@/lib/db";

// GET - Fetch all users
export async function GET() {
  try {
    const users = await db.user.findMany({
      select: {
        id: true,
        name: true,
        email: true,
        createdAt: true,
      },
    });
    
    return NextResponse.json(users);
  } catch (error) {
    return NextResponse.json(
      { error: "Failed to fetch users" },
      { status: 500 }
    );
  }
}

// POST - Create a new user
export async function POST(request: Request) {
  try {
    const body = await request.json();
    
    // Validate required fields
    if (!body.email || !body.name) {
      return NextResponse.json(
        { error: "Name and email are required" },
        { status: 400 }
      );
    }
    
    const user = await db.user.create({
      data: {
        name: body.name,
        email: body.email,
      },
    });
    
    return NextResponse.json(user, { status: 201 });
  } catch (error) {
    return NextResponse.json(
      { error: "Failed to create user" },
      { status: 500 }
    );
  }
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `import { NextResponse } from "next/server"` | Import response helper | Used to return data from route handlers |
| `export async function GET()` | Handle GET requests | Default HTTP method handlers |
| `NextResponse.json(data)` | Return JSON response | Easiest way to send JSON back |
| `export async function POST(request: Request)` | Handle POST requests | Receives the request object |
| `await request.json()` | Parse request body | Gets the JSON data sent with POST |
| `{ status: 201 }` | Set HTTP status code | 201 = Created, important for APIs |

## Common Mistakes

### Mistake 1: Using Wrong HTTP Method Names

```typescript
// WRONG - Method names must be uppercase
export async function get() { // lowercase doesn't work!
  return NextResponse.json({ ok: true });
}

// CORRECT - Use uppercase HTTP method names
export async function GET() {
  return NextResponse.json({ ok: true });
}
```

### Mistake 2: Not Handling Request Body Correctly

```typescript
// WRONG - Trying to read body from wrong source
export async function POST() {
  const body = request.body; // This doesn't work!
}

// CORRECT - Use request.json() for JSON body
export async function POST(request: Request) {
  const body = await request.json();
}
```

### Mistake 3: Returning Plain Objects Instead of NextResponse

```typescript
// WRONG - Just returning an object
export async function GET() {
  return { message: "Hello" }; // Not a proper response!
}

// CORRECT - Wrap in NextResponse
export async function GET() {
  return NextResponse.json({ message: "Hello" });
}
```

## Summary

- Route handlers are the App Router's way of creating API endpoints
- Export functions named after HTTP methods (GET, POST, PUT, DELETE, PATCH)
- Use `NextResponse.json()` to return JSON data
- Access request body with `request.json()`
- Set HTTP status codes with the second argument to NextResponse
- Route handlers are server-side only — code never ships to the client

## Next Steps

- [creating-route-ts.md](./creating-route-ts.md) - Creating your first route handler
- [http-methods.md](./http-methods.md) - Deep dive into different HTTP methods
