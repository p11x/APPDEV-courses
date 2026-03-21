# Revalidating Data After Server Actions

## What You'll Learn
- How to refresh data after mutations
- Using revalidatePath and revalidateTag
- When to use each method

## Prerequisites
- Understanding of Server Actions
- Basic fetch caching knowledge

## Concept Explained Simply

After a Server Action modifies data (create, update, delete), you need to tell Next.js to refresh its cached data. Otherwise, users will see old data until they manually refresh the page.

## Revalidation Methods

### revalidatePath

Refreshes all data in a specific path:

```typescript
import { revalidatePath } from "next/cache";

revalidatePath("/blog");
revalidatePath("/blog/[slug]");
```

### revalidateTag

Refreshes all data with a specific tag:

```typescript
import { revalidateTag } from "next/cache";

revalidateTag("posts");
revalidateTag("comments");
```

## Complete Code Example

```typescript
// src/app/actions.ts
"use server";

import { revalidatePath, revalidateTag } from "next/cache";

// Using revalidatePath
export async function createPost(formData: FormData) {
  // Create post...
  
  // Refresh the blog page
  revalidatePath("/blog");
  revalidatePath("/");
}

// Using revalidateTag
export async function updatePost(id: string, data: any) {
  // Update post...
  
  // Refresh all cached data with 'posts' tag
  revalidateTag("posts");
}
```

```typescript
// src/app/blog/page.tsx
export default async function BlogPage() {
  // This fetch is cached with tag 'posts'
  const posts = await fetch("https://api.example.com/posts", {
    next: { tags: ["posts"] },
  }).then((res) => res.json());

  return <div>{posts.map((p: any) => p.title).join(", ")}</div>;
}
```

## Summary

- Use `revalidatePath` to refresh specific routes
- Use `revalidateTag` for more granular control
- Call after mutations to keep UI in sync
