# Returning JSON Responses

## What You'll Learn
- Sending JSON responses
- Setting HTTP status codes
- Setting response headers
- Handling errors properly

## Prerequisites
- Understanding of route handlers
- Basic knowledge of HTTP responses
- Familiarity with NextResponse

## Concept Explained Simply

When your API receives a request, it needs to send back a response — like a waiter bringing back your ordered food. The response should tell the caller whether their request succeeded, and if it did, include the requested data. JSON is the standard format because it's easy for both humans and computers to read.

Think of the response like a report card: it tells you the result (pass/fail) and includes details about what happened. Status codes are like grades — 200 means "everything went well," 404 means "couldn't find what you asked for," and 500 means "something went wrong on our end."

## Complete Code Example

### Basic JSON Responses

```typescript
// app/api/users/route.ts
import { NextResponse } from "next/server";

// Success response
export async function GET() {
  const users = [
    { id: 1, name: "Alice", email: "alice@example.com" },
    { id: 2, name: "Bob", email: "bob@example.com" },
  ];
  
  return NextResponse.json(users);
}

// Created response (201)
export async function POST(request: Request) {
  const body = await request.json();
  
  const newUser = {
    id: Date.now(),
    ...body,
    createdAt: new Date().toISOString(),
  };
  
  return NextResponse.json(newUser, { status: 201 });
}
```

### Error Responses

```typescript
// app/api/users/[id]/route.ts
import { NextResponse } from "next/server";

export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  
  // 404 - Not Found
  const user = await findUser(id);
  
  if (!user) {
    return NextResponse.json(
      { error: "User not found", code: "USER_NOT_FOUND" },
      { status: 404 }
    );
  }
  
  // 200 - Success
  return NextResponse.json(user);
}

// 400 - Bad Request
export async function PUT(request: Request) {
  try {
    const body = await request.json();
    
    if (!body.email || !body.name) {
      return NextResponse.json(
        { 
          error: "Validation failed",
          fields: { email: "Required", name: "Required" }
        },
        { status: 400 }
      );
    }
    
    return NextResponse.json({ success: true });
    
  } catch {
    // 400 - Invalid JSON
    return NextResponse.json(
      { error: "Invalid JSON body" },
      { status: 400 }
    );
  }
}

// 401 - Unauthorized
export async function DELETE(request: Request) {
  const authHeader = request.headers.get("authorization");
  
  if (!authHeader) {
    return NextResponse.json(
      { error: "Authentication required" },
      { status: 401 }
    );
  }
  
  // 403 - Forbidden
  const token = authHeader.replace("Bearer ", "");
  const user = await verifyToken(token);
  
  if (!user) {
    return NextResponse.json(
      { error: "Invalid or expired token" },
      { status: 403 }
    );
  }
  
  // 204 - No Content (successful deletion with no response body)
  await deleteUser(user.id);
  return new NextResponse(null, { status: 204 });
}
```

### Setting Response Headers

```typescript
// app/api/download/route.ts
import { NextResponse } from "next/server";

export async function GET() {
  const data = await generateReport();
  
  const response = NextResponse.json(data);
  
  // Set custom headers
  response.headers.set("X-Report-Generated", new Date().toISOString());
  response.headers.set("X-Report-Version", "1.0");
  
  // Cache control
  response.headers.set(
    "Cache-Control", 
    "public, max-age=3600, s-maxage=86400"
  );
  
  return response;
}
```

### Setting Cookies

```typescript
// app/api/login/route.ts
import { NextResponse } from "next/server";
import { verifyPassword, createToken } from "@/lib/auth";

export async function POST(request: Request) {
  const body = await request.json();
  
  const user = await verifyPassword(body.email, body.password);
  
  if (!user) {
    return NextResponse.json(
      { error: "Invalid credentials" },
      { status: 401 }
    );
  }
  
  const token = await createToken(user);
  
  const response = NextResponse.json(
    { message: "Login successful", user: { id: user.id, email: user.email } },
    { status: 200 }
  );
  
  // Set cookie
  response.cookies.set("auth-token", token, {
    httpOnly: true,
    secure: process.env.NODE_ENV === "production",
    sameSite: "lax",
    maxAge: 60 * 60 * 24 * 7, // 7 days
    path: "/",
  });
  
  return response;
}
```

## HTTP Status Code Reference

| Code | Name | When to Use |
|------|------|-------------|
| 200 | OK | Successful GET, PUT, PATCH, DELETE |
| 201 | Created | Successful POST that creates something |
| 204 | No Content | Successful DELETE with no response body |
| 400 | Bad Request | Invalid input, malformed JSON |
| 401 | Unauthorized | Missing authentication |
| 403 | Forbidden | Authenticated but not allowed |
| 404 | Not Found | Resource doesn't exist |
| 500 | Internal Server Error | Unexpected server error |

## Common Mistakes

### Mistake 1: Not Setting Proper Status Codes

```typescript
// WRONG - Always returning 200
export async function POST(request: Request) {
  const user = await createUser(await request.json());
  return NextResponse.json(user); // Should be 201!
}

// CORRECT - Return 201 for creation
export async function POST(request: Request) {
  const user = await createUser(await request.json());
  return NextResponse.json(user, { status: 201 });
}
```

### Mistake 2: Not Handling Errors

```typescript
// WRONG - No error handling at all
export async function POST(request: Request) {
  const body = await request.json();
  await db.user.create({ data: body });
  return NextResponse.json({ success: true });
  // What if database fails?
}

// CORRECT - Always handle potential errors
export async function POST(request: Request) {
  try {
    const body = await request.json();
    const user = await db.user.create({ data: body });
    return NextResponse.json(user, { status: 201 });
  } catch (error) {
    return NextResponse.json(
      { error: "Failed to create user" },
      { status: 500 }
    );
  }
}
```

### Mistake 3: Returning Non-JSON to JSON Endpoint

```typescript
// WRONG - Returning string instead of JSON
export async function GET() {
  return NextResponse.json("hello"); // Gets wrapped in object
}

// CORRECT - Return proper object
export async function GET() {
  return NextResponse.json({ message: "hello" });
}
```

## Summary

- Use `NextResponse.json(data, { status })` for JSON responses
- Always set appropriate status codes (201 for create, 404 for not found, etc.)
- Return errors with descriptive messages
- Set custom headers with `response.headers.set()`
- Use cookies via `response.cookies.set()`
- Always wrap database operations in try/catch

## Next Steps

- [dynamic-route-handlers.md](../03-advanced-handlers/dynamic-route-handlers.md) - Using URL parameters in route handlers
- [webhooks.md](../03-advanced-handlers/webhooks.md) - Handling incoming webhook requests
