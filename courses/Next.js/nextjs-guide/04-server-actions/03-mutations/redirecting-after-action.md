# Redirecting After Server Actions

## What You'll Learn
- Using redirect after mutations
- Navigating users after actions

## Prerequisites
- Understanding of Server Actions

## Complete Code Example

```typescript
// src/app/actions.ts
"use server";

import { redirect } from "next/navigation";

export async function createPost(formData: FormData) {
  // Create post...
  // await prisma.post.create({ data: { title: "New Post" } });
  
  // Redirect to the new post
  redirect("/blog/new-post");
}

export async function login(formData: FormData) {
  // Authenticate...
  
  // Redirect to dashboard
  redirect("/dashboard");
}
```

## Summary

- Use `redirect()` from "next/navigation"
- Call after successful mutations
- Works in Server Actions only
