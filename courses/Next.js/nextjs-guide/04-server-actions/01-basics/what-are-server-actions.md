# What Are Server Actions

## What You'll Learn
- What Server Actions are
- Why they're useful
- How they differ from API routes

## Prerequisites
- Understanding of Server Components
- Basic React knowledge

## Concept Explained Simply

**Server Actions** are a way to run code on the server directly from your client components. Instead of creating an API endpoint (route.ts) and then calling it with fetch, you can define a function that runs on the server and call it like a regular function.

Think of it like ordering food at a restaurant. Before Server Actions, you'd have to:
1. Walk up to the counter (create API route)
2. Place your order (call the API with fetch)
3. Wait for the kitchen to prepare (wait for response)

With Server Actions, it's like having a direct line to the kitchen — you just say what you want and they bring it to you. Much simpler!

## Why Server Actions?

| API Routes | Server Actions |
|------------|----------------|
| Need to create separate files | Define functions directly |
| Need to handle HTTP manually | Just call the function |
| More boilerplate code | Less code to write |
| Can be called from anywhere | Type-safe from your components |

## Complete Code Example

Let's create a simple form with Server Actions:

```typescript
// src/app/actions.ts - Define Server Actions
"use server";

import { revalidatePath } from "next/cache";

// This function runs ONLY on the server
export async function createPost(formData: FormData) {
  const title = formData.get("title") as string;
  const content = formData.get("content") as string;
  
  // Validate the data
  if (!title || !content) {
    throw new Error("Title and content are required");
  }
  
  // Save to database (example)
  const post = {
    id: Date.now().toString(),
    title,
    content,
    createdAt: new Date(),
  };
  
  // In a real app, save to database:
  // await prisma.post.create({ data: { title, content } });
  
  // Revalidate the blog page to show new post
  revalidatePath("/blog");
  
  return { success: true, post };
}
```

```typescript
// src/app/blog/page.tsx - Server Component displays posts
import { createPost } from "@/app/actions";
import { PostForm } from "@/components/PostForm";

export default async function BlogPage() {
  // In a real app, fetch from database
  const posts = [
    { id: "1", title: "First Post", content: "Hello world!" },
  ];

  return (
    <main style={{ padding: "2rem", maxWidth: "800px", margin: "0 auto" }}>
      <h1>Blog</h1>
      
      <section style={{ marginBottom: "3rem" }}>
        <h2>Create New Post</h2>
        <PostForm action={createPost} />
      </section>
      
      <section>
        <h2>Posts</h2>
        {posts.map((post) => (
          <article
            key={post.id}
            style={{ padding: "1rem", border: "1px solid #ddd", marginBottom: "1rem" }}
          >
            <h3>{post.title}</h3>
            <p>{post.content}</p>
          </article>
        ))}
      </section>
    </main>
  );
}
```

```typescript
// src/components/PostForm.tsx - Client Component uses Server Action
"use client";

import { useRef } from "react";

interface PostFormProps {
  action: (formData: FormData) => Promise<{ success: boolean }>;
}

export function PostForm({ action }: PostFormProps) {
  const formRef = useRef<HTMLFormElement>(null);

  return (
    <form
      ref={formRef}
      action={async (formData) => {
        try {
          await action(formData);
          formRef.current?.reset(); // Clear form on success
        } catch (error) {
          alert("Failed to create post");
        }
      }}
      style={{ display: "flex", flexDirection: "column", gap: "1rem", maxWidth: "400px" }}
    >
      <div>
        <label htmlFor="title" style={{ display: "block", marginBottom: "0.5rem" }}>
          Title
        </label>
        <input
          id="title"
          name="title"
          type="text"
          required
          style={{ width: "100%", padding: "0.5rem" }}
        />
      </div>
      
      <div>
        <label htmlFor="content" style={{ display: "block", marginBottom: "0.5rem" }}>
          Content
        </label>
        <textarea
          id="content"
          name="content"
          required
          rows={4}
          style={{ width: "100%", padding: "0.5rem" }}
        />
      </div>
      
      <button
        type="submit"
        style={{
          padding: "0.75rem",
          backgroundColor: "#0070f3",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: "pointer",
        }}
      >
        Create Post
      </button>
    </form>
  );
}
```

## How It Works

1. Server Action is defined with `"use server"`
2. Client Component receives the action as a prop
3. Form's `action` attribute calls the Server Action
4. Server Action runs on the server, processes the data
5. `revalidatePath` refreshes the page data automatically
6. UI updates to show new data

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `"use server"` | Marks function for server execution | Function runs on server only |
| `formData.get("title")` | Get form input | Access form data |
| `revalidatePath("/blog")` | Refresh cached data | Update UI after mutation |
| `action={action}` | Pass Server Action to client | Connects server logic to client |

## Common Mistakes

### Mistake #1: Not Adding "use server"

```typescript
// ✗ Wrong: Missing directive
export async function createPost(formData: FormData) {
  // This would run on client!
}

// ✓ Correct: Add directive
"use server";

export async function createPost(formData: FormData) {
  // Now runs on server
}
```

### Mistake #2: Not Handling Errors

```typescript
// ✗ Wrong: No error handling
export async function createPost(formData: FormData) {
  const result = await db.post.create({ /* ... */ });
  return result; // If it fails, the app crashes!
}

// ✓ Correct: Try/catch and validation
export async function createPost(formData: FormData) {
  const title = formData.get("title");
  
  if (!title) {
    throw new Error("Title is required");
  }
  
  try {
    return await db.post.create({ data: { title } });
  } catch (error) {
    throw new Error("Failed to create post");
  }
}
```

### Mistake #3: Forgetting to Revalidate

```typescript
// ✗ Wrong: Data changes but UI doesn't update
export async function createPost(formData: FormData) {
  await db.post.create({ /* ... */ });
  // Missing revalidatePath!
  // User won't see the new post
}

// ✓ Correct: Revalidate after mutation
export async function createPost(formData: FormData) {
  await db.post.create({ /* ... */ });
  revalidatePath("/blog");
}
```

## Summary

- Server Actions let you run server code from client components
- Add `"use server"` at the top of the function
- Use `revalidatePath` to refresh data after mutations
- They simplify data mutations compared to API routes

## Next Steps

Now let's learn about the use server directive:

- [use server Directive →](./use-server-directive.md)
