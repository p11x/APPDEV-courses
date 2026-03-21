# The "use server" Directive

## What You'll Learn
- How "use server" works
- Where to place it
- Differences from "use client"

## Prerequisites
- Understanding of Server Actions
- Basic React knowledge

## Concept Explained Simply

The `"use server"` directive tells Next.js that a function should run on the server instead of the client. It's the opposite of "use client" — where "use client" marks components to run in the browser, "use server" marks functions to run on the server.

Think of it like a pass card:
- "use client" = Access to the client side (browser)
- "use server" = Access to the server side (database, secrets)

## Two Ways to Use "use server"

### 1. At the Top of a File

When you add `"use server"` at the top of a file, ALL exported functions become Server Actions:

```typescript
// src/app/actions.ts
"use server";

// All functions in this file run on the server
export async function createPost(formData: FormData) {
  // This runs on the server
}

export async function updatePost(id: string, data: any) {
  // This also runs on the server
}

export async function deletePost(id: string) {
  // This runs on the server too
}
```

### 2. Inside a Function (Inline)

You can also add it inside a function to create an inline Server Action:

```typescript
// Inside a client component file
"use client";

import { useState } from "react";

export function MyComponent() {
  async function serverAction() {
    "use server";  // This specific function runs on server
    // Server-side code here
  }
  
  // Component code
}
```

## Complete Code Examples

### File-Level Directive

```typescript
// src/app/api/posts/actions.ts
"use server";

import { revalidatePath } from "next/cache";

interface PostData {
  title: string;
  content: string;
}

export async function createPost(data: PostData) {
  // Validate input
  if (!data.title || data.title.length < 3) {
    throw new Error("Title must be at least 3 characters");
  }
  
  // Save to database (example with Prisma)
  // const post = await prisma.post.create({
  //   data: { title: data.title, content: data.content }
  // });
  
  // Simulate database save
  const post = {
    id: Date.now().toString(),
    ...data,
    createdAt: new Date(),
  };
  
  // Revalidate the blog page
  revalidatePath("/blog");
  
  return { success: true, post };
}

export async function deletePost(id: string) {
  // Delete from database
  // await prisma.post.delete({ where: { id } });
  
  revalidatePath("/blog");
  
  return { success: true };
}
```

### Inline Directive

```typescript
// src/components/InteractiveForm.tsx
"use client";

import { useState } from "react";

export function InteractiveForm() {
  const [message, setMessage] = useState("");

  async function submitForm(data: FormData) {
    "use server";
    
    const name = data.get("name");
    
    if (!name) {
      throw new Error("Name is required");
    }
    
    // Process on server
    return { message: `Hello, ${name}!` };
  }

  return (
    <form action={async (formData) => {
      const result = await submitForm(formData);
      setMessage(result.message);
    }}>
      <input type="text" name="name" placeholder="Your name" />
      <button type="submit">Submit</button>
      {message && <p>{message}</p>}
    </form>
  );
}
```

## use server vs use client

| Feature | "use server" | "use client" |
|---------|--------------|--------------|
| What it marks | Functions | Components + hooks |
| Where code runs | Server | Browser |
| Can access DB | ✅ Yes | ❌ No |
| Can use hooks | ❌ No | ✅ Yes |
| Default in App Router | Functions | Components |

## When to Use Each

### Use "use server" when:
- Writing Server Actions
- Need database access
- Working with secrets/API keys
- Processing form data

### Use "use client" when:
- Building interactive components
- Using React hooks
- Using browser APIs

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `"use server"` at file top | All exports are Server Actions | Simplifies large files |
| `"use server"` inline | Single function runs on server | Useful in client files |
| `revalidatePath` | Refresh data after mutation | Keep UI in sync |

## Common Mistakes

### Mistake #1: Confusing with "use client"

```typescript
// ✗ Wrong: Adding to client component
"use server";

export function ClientComponent() {
  // This doesn't make sense - components need "use client"
  return <button>Click</button>;
}

// ✓ Correct: Use the right directive
"use client";

export function ClientComponent() {
  return <button>Click</button>;
}
```

### Mistake #2: Missing the Directive

```typescript
// ✗ Wrong: No directive = runs on client
export async function createPost(data) {
  // This would fail trying to access database from client!
}

// ✓ Correct: Add the directive
"use server";

export async function createPost(data) {
  // Now runs on server, can access database
}
```

### Mistake #3: Putting in Wrong Location

```typescript
// ✗ Wrong: Inside client component, not a function
"use client";

"use server"  // Wrong location!

export function Component() {
  return <div>Hello</div>;
}

// ✓ Correct: At function level or file level
"use client";

export function Component() {
  async function handleClick() {
    "use server";  // Correct - inside function
    // Server logic
  }
  
  return <button onClick={handleClick}>Click</button>;
}
```

## Summary

- Add `"use server"` at the top of a file or inside a function
- All exports become Server Actions when at file top
- Inline `"use server"` creates server functions in client files
- Use `"use client"` for components, `"use server"` for actions

## Next Steps

Now let's learn about forms with Server Actions:

- [Forms with Server Actions →](./forms-with-server-actions.md)
