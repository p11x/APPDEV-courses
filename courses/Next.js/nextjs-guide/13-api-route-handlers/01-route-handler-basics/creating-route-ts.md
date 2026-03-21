# Creating Your First Route Handler

## What You'll Learn
- Setting up a route handler file
- Naming conventions for route handlers
- Testing your route handler
- Understanding the file system routing

## Prerequisites
- A Next.js project with TypeScript
- Understanding of basic HTTP requests
- Familiarity with the App Router folder structure

## Concept Explained Simply

Creating a route handler is like setting up a new email address for a specific department. Just as `sales@company.com` handles sales inquiries, `app/api/users/route.ts` handles requests to `/api/users`. The file path directly corresponds to the URL — it's that simple!

You create these files inside your `app` directory, and Next.js automatically turns them into API endpoints. No extra configuration needed — the file system does all the work.

## Complete Code Example

### Step 1: Create the Folder Structure

```
your-nextjs-app/
├── app/
│   ├── api/
│   │   └── hello/
│   │       └── route.ts    ← Creates /api/hello endpoint
│   └── page.tsx
├── package.json
└── tsconfig.json
```

### Step 2: Create the Route Handler

```typescript
// app/api/hello/route.ts
import { NextResponse } from "next/server";

// Handle GET requests to /api/hello
export async function GET() {
  return NextResponse.json({
    message: "Hello, World!",
    version: "1.0",
  });
}
```

### Step 3: Test It

```bash
# In your terminal
curl http://localhost:3000/api/hello

# Or visit in browser: http://localhost:3000/api/hello
```

Expected response:
```json
{
  "message": "Hello, World!",
  "version": "1.0"
}
```

### A More Complex Example

```typescript
// app/api/users/route.ts
import { NextResponse } from "next/server";
import { z } from "zod";

// Validation schema
const userSchema = z.object({
  name: z.string().min(1),
  email: z.string().email(),
});

// GET handler
export async function GET() {
  // In a real app, fetch from database
  const users = [
    { id: 1, name: "Alice", email: "alice@example.com" },
    { id: 2, name: "Bob", email: "bob@example.com" },
  ];
  
  return NextResponse.json(users);
}

// POST handler
export async function POST(request: Request) {
  try {
    // Parse and validate request body
    const body = await request.json();
    const result = userSchema.safeParse(body);
    
    if (!result.success) {
      return NextResponse.json(
        { error: "Invalid input", details: result.error.issues },
        { status: 400 }
      );
    }
    
    // Create user (mock)
    const newUser = {
      id: Date.now(),
      ...result.data,
    };
    
    return NextResponse.json(newUser, { status: 201 });
  } catch (error) {
    return NextResponse.json(
      { error: "Internal server error" },
      { status: 500 }
    );
  }
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `app/api/hello/route.ts` | File path determines URL | `/api/hello` becomes the endpoint |
| `import { NextResponse }` | Import Next.js response | Standard way to return data |
| `export async function GET()` | Export named GET function | Handles GET requests automatically |
| `NextResponse.json({...})` | Return JSON response | Creates response with JSON body |

## Common Mistakes

### Mistake 1: Wrong File Location

```typescript
// WRONG - File in wrong place
// app/routes/hello/route.ts → won't work!

// CORRECT - Must be in app directory with api folder
// app/api/hello/route.ts → works!
```

### Mistake 2: Wrong File Name

```typescript
// WRONG - Using handler.ts or index.ts
// app/api/hello/handler.ts → won't work!

// CORRECT - Must be named route.ts
// app/api/hello/route.ts → works!
```

### Mistake 3: Not Exporting Function Correctly

```typescript
// WRONG - Exporting default
export default async function handler() {
  return NextResponse.json({ ok: true });
}

// CORRECT - Export named function
export async function GET() {
  return NextResponse.json({ ok: true });
}
```

## Summary

- Route handlers live in `app/api/[endpoint]/route.ts`
- The file path directly maps to the URL
- Export functions named after HTTP methods (GET, POST, PUT, DELETE)
- Use `NextResponse.json()` to return JSON
- Test with browser or curl
- Can use any Node.js or Web APIs inside route handlers

## Next Steps

- [http-methods.md](./http-methods.md) - Learn about different HTTP methods
- [reading-request-body.md](../02-request-and-response/reading-request-body.md) - Handling request data
