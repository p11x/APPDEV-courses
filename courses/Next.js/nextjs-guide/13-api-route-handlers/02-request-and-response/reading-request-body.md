# Reading Request Body

## What You'll Learn
- Parsing JSON request bodies
- Handling form data
- Working with different content types
- Validating incoming data

## Prerequisites
- Understanding of route handlers
- Basic knowledge of JSON and forms
- Familiarity with TypeScript

## Concept Explained Simply

When someone sends data to your API, it's packed into the "body" of the request — like putting a letter inside an envelope. Your route handler needs to open that envelope and read what's inside. The format could be JSON, form data, or even plain text, and you need to handle each properly.

Think of it like a postal worker: they need to know if they're handling a letter, a package, or a padded envelope, and open each one correctly.

## Complete Code Example

### Reading JSON Body

```typescript
// app/api/users/route.ts
import { NextResponse } from "next/server";

// POST handler with JSON body
export async function POST(request: Request) {
  try {
    // Read the JSON body
    const body = await request.json();
    
    // Use the data
    console.log("Received:", body);
    
    return NextResponse.json({
      received: body,
      message: "User created",
    }, { status: 201 });
    
  } catch (error) {
    return NextResponse.json(
      { error: "Invalid JSON in request body" },
      { status: 400 }
    );
  }
}
```

### Reading Form Data

```typescript
// app/api/contact/route.ts
import { NextResponse } from "next/server";

export async function POST(request: Request) {
  // Get the content type
  const contentType = request.headers.get("content-type");
  
  if (contentType?.includes("application/json")) {
    // Handle JSON
    const body = await request.json();
    console.log("JSON:", body);
    
  } else if (contentType?.includes("multipart/form-data")) {
    // Handle form data
    const formData = await request.formData();
    const name = formData.get("name");
    const email = formData.get("email");
    const file = formData.get("attachment");
    
    console.log("Form:", { name, email, file });
    
  } else if (contentType?.includes("application/x-www-form-urlencoded")) {
    // Handle URL-encoded form
    const formData = await request.formData();
    console.log("URL-encoded:", Object.fromEntries(formData));
  }
  
  return NextResponse.json({ success: true });
}
```

### Handling Different Content Types

```typescript
// app/api/data/route.ts
import { NextResponse } from "next/server";

export async function POST(request: Request) {
  const contentType = request.headers.get("content-type") || "";
  
  let data;
  
  if (contentType.includes("application/json")) {
    // JSON
    data = await request.json();
    
  } else if (contentType.includes("text/plain")) {
    // Plain text
    const text = await request.text();
    data = { text };
    
  } else if (contentType.includes("application/octet-stream")) {
    // Binary/file
    const arrayBuffer = await request.arrayBuffer();
    const buffer = Buffer.from(arrayBuffer);
    data = { size: buffer.length, type: "binary" };
    
  } else {
    return NextResponse.json(
      { error: "Unsupported content type" },
      { status: 415 }
    );
  }
  
  return NextResponse.json({ received: data });
}
```

### With Validation Using Zod

```typescript
// app/api/users/route.ts
import { NextResponse } from "next/server";
import { z } from "zod";

// Define expected shape
const createUserSchema = z.object({
  name: z.string().min(1, "Name is required"),
  email: z.string().email("Invalid email format"),
  age: z.number().min(18).optional(),
});

export async function POST(request: Request) {
  try {
    const body = await request.json();
    
    // Validate against schema
    const result = createUserSchema.safeParse(body);
    
    if (!result.success) {
      // Return validation errors
      return NextResponse.json(
        { 
          error: "Validation failed",
          details: result.error.flatten().fieldErrors 
        },
        { status: 400 }
      );
    }
    
    // Use validated data
    const { name, email, age } = result.data;
    
    // Create user in database...
    
    return NextResponse.json(
      { message: "User created", user: { name, email, age } },
      { status: 201 }
    );
    
  } catch (error) {
    return NextResponse.json(
      { error: "Invalid request body" },
      { status: 400 }
    );
  }
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `await request.json()` | Parse JSON body | Returns parsed JavaScript object |
| `await request.text()` | Parse as plain text | Useful for non-JSON data |
| `await request.formData()` | Parse form data | For form submissions |
| `await request.arrayBuffer()` | Get raw binary | For file uploads |
| `request.headers.get()` | Read content type | Knows how to parse the body |
| `z.object({...})` | Define validation schema | Ensures data is correct shape |

## Common Mistakes

### Mistake 1: Not Awaiting the Body Read

```typescript
// WRONG - Forgot to await
export async function POST(request: Request) {
  const body = request.json(); // Returns a Promise!
  console.log(body); // Shows "Promise pending"
}

// CORRECT - Await the promise
export async function POST(request: Request) {
  const body = await request.json();
  console.log(body); // Shows actual data
}
```

### Mistake 2: Reading Body Twice

```typescript
// WRONG - Can only read body once!
export async function POST(request: Request) {
  const body1 = await request.json();
  const body2 = await request.json(); // Error: body already consumed
  
  return NextResponse.json({ body1, body2 });
}

// CORRECT - Save to variable
export async function POST(request: Request) {
  const body = await request.json();
  // Use body as many times as needed
  return NextResponse.json({ received: body });
}
```

### Mistake 3: Not Handling Invalid JSON

```typescript
// WRONG - No error handling for bad JSON
export async function POST(request: Request) {
  const body = await request.json(); // Crashes if invalid JSON!
  return NextResponse.json({ body });
}

// CORRECT - Try/catch
export async function POST(request: Request) {
  try {
    const body = await request.json();
    return NextResponse.json({ body });
  } catch {
    return NextResponse.json(
      { error: "Invalid JSON" },
      { status: 400 }
    );
  }
}
```

## Summary

- Use `await request.json()` for JSON bodies
- Use `await request.formData()` for form submissions
- Use `await request.text()` for plain text
- A request body can only be read once — store it in a variable
- Always wrap body parsing in try/catch for invalid data
- Validate incoming data with Zod or similar libraries
- Check `content-type` header to know how to parse

## Next Steps

- [query-params-and-headers.md](./query-params-and-headers.md) - Reading URL parameters and headers
- [returning-json.md](./returning-json.md) - Sending responses back
