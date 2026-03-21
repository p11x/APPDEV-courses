# HTTP Methods in Route Handlers

## What You'll Learn
- Understanding GET, POST, PUT, PATCH, DELETE methods
- When to use each HTTP method
- Implementing multiple methods in one route handler
- Proper status codes for each method

## Prerequisites
- Basic understanding of REST APIs
- Knowledge of route handler structure
- Familiarity with CRUD operations

## Concept Explained Simply

HTTP methods are like different types of requests you might make at a restaurant:
- **GET** = "What do you have on the menu?" (just looking, no changes)
- **POST** = "I'd like to order this new dish" (create something new)
- **PUT** = "Replace my entire order" (replace something entirely)
- **PATCH** = "Can I just add a drink to my order?" (modify part of something)
- **DELETE** = "Cancel that order" (remove something)

Each method has a specific purpose, and using them correctly makes your API intuitive and standard-compliant.

## Complete Code Example

```typescript
// app/api/users/[id]/route.ts
import { NextResponse } from "next/server";
import { db } from "@/lib/db";

// GET - Fetch a single user by ID
export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  
  const user = await db.user.findUnique({
    where: { id },
  });
  
  if (!user) {
    return NextResponse.json(
      { error: "User not found" },
      { status: 404 }
    );
  }
  
  return NextResponse.json(user);
}

// PUT - Replace an entire user
export async function PUT(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const body = await request.json();
  
  // Validate required fields
  if (!body.name || !body.email) {
    return NextResponse.json(
      { error: "Name and email are required" },
      { status: 400 }
    );
  }
  
  const user = await db.user.update({
    where: { id },
    data: {
      name: body.name,
      email: body.email,
    },
  });
  
  return NextResponse.json(user);
}

// PATCH - Partially update a user
export async function PATCH(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const body = await request.json();
  
  const user = await db.user.update({
    where: { id },
    data: body, // Only update provided fields
  });
  
  return NextResponse.json(user);
}

// DELETE - Remove a user
export async function DELETE(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  
  await db.user.delete({
    where: { id },
  });
  
  return NextResponse.json(
    { message: "User deleted successfully" },
    { status: 200 }
  );
}
```

### Collection Endpoint (GET all, POST new)

```typescript
// app/api/users/route.ts
import { NextResponse } from "next/server";
import { db } from "@/lib/db";

// GET /api/users - Fetch all users
export async function GET() {
  const users = await db.user.findMany();
  return NextResponse.json(users);
}

// POST /api/users - Create a new user
export async function POST(request: Request) {
  const body = await request.json();
  
  const user = await db.user.create({
    data: {
      name: body.name,
      email: body.email,
    },
  });
  
  return NextResponse.json(user, { status: 201 });
}
```

## HTTP Method Reference

| Method | Purpose | Status Codes | Example |
|--------|---------|--------------|---------|
| GET | Read/Fetch | 200 (OK), 404 (Not Found) | `GET /api/users` |
| POST | Create | 201 (Created), 400 (Bad Request) | `POST /api/users` |
| PUT | Replace | 200 (OK), 404 (Not Found) | `PUT /api/users/1` |
| PATCH | Modify | 200 (OK), 404 (Not Found) | `PATCH /api/users/1` |
| DELETE | Remove | 200 (OK), 404 (Not Found) | `DELETE /api/users/1` |

## Common Mistakes

### Mistake 1: Using POST for Everything

```typescript
// WRONG - Using POST for all operations
export async function POST(request: Request) {
  const body = await request.json();
  
  if (body.action === "delete") {
    // Don't do this! Use proper DELETE method
    await deleteUser(body.id);
  }
}

// CORRECT - Use proper HTTP methods
export async function DELETE(...) {
  await deleteUser(id);
}
```

### Mistake 2: Not Handling Missing Data

```typescript
// WRONG - Not validating required data
export async function POST(request: Request) {
  const body = await request.json();
  
  // What if body.name or body.email is undefined?
  const user = await db.user.create({
    data: { name: body.name, email: body.email }
  });
}

// CORRECT - Always validate
export async function POST(request: Request) {
  const body = await request.json();
  
  if (!body.name || !body.email) {
    return NextResponse.json(
      { error: "Name and email required" },
      { status: 400 }
    );
  }
  
  const user = await db.user.create({
    data: { name: body.name, email: body.email }
  });
}
```

### Mistake 3: Forgetting params is a Promise (Next.js 15)

```typescript
// WRONG - Not awaiting params (Next.js 15)
export async function GET({ params }: { params: { id: string } }) {
  const user = await db.user.findUnique({
    where: { id: params.id } // Might not work!
  });
}

// CORRECT - Await params (Next.js 15)
export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const user = await db.user.findUnique({
    where: { id }
  });
}
```

## Summary

- GET = fetch data, POST = create new, PUT = replace, PATCH = modify, DELETE = remove
- Use the right method for the job — it's more than just convention
- Always return appropriate status codes (200, 201, 400, 404, 500)
- In Next.js 15, params must be awaited (it's a Promise)
- Validate all input data before processing
- Use separate route files for collection endpoints vs individual item endpoints

## Next Steps

- [query-params-and-headers.md](../02-request-and-response/query-params-and-headers.md) - Reading URL parameters
- [webhooks.md](../03-advanced-handlers/webhooks.md) - Handling webhook callbacks
