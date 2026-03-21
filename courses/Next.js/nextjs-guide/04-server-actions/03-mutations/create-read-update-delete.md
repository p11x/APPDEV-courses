# CRUD Operations with Server Actions

## What You'll Learn
- Implementing Create, Read, Update, Delete
- Complete data mutation patterns

## Prerequisites
- Understanding of Server Actions

## Complete Code Example

```typescript
// src/app/actions.ts
"use server";

import { revalidatePath, revalidateTag } from "next/cache";

// CREATE
export async function createPost(formData: FormData) {
  const title = formData.get("title") as string;
  const content = formData.get("content") as string;
  
  // await prisma.post.create({ data: { title, content } });
  
  revalidateTag("posts");
  return { success: true };
}

// READ
export async function getPost(id: string) {
  // return await prisma.post.findUnique({ where: { id } });
  return { id, title: "Sample", content: "Content" };
}

// UPDATE
export async function updatePost(id: string, formData: FormData) {
  const title = formData.get("title") as string;
  
  // await prisma.post.update({ where: { id }, data: { title } });
  
  revalidateTag("posts");
  return { success: true };
}

// DELETE
export async function deletePost(id: string) {
  // await prisma.post.delete({ where: { id } });
  
  revalidateTag("posts");
  return { success: true };
}
```

## Summary

- All mutations follow the same pattern
- Always revalidate after changes
- Return success/error states
