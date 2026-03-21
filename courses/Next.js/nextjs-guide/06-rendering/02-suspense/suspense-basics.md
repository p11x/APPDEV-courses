# Suspense Basics in Next.js

## What You'll Learn
- Using Suspense for loading states
- How Suspense works with streaming

## Complete Example

```typescript
// src/app/blog/page.tsx
import { Suspense } from "react";
import { BlogList } from "@/components/BlogList";
import { BlogListSkeleton } from "@/components/BlogListSkeleton";

export default function BlogPage() {
  return (
    <main>
      <h1>Blog</h1>
      <Suspense fallback={<BlogListSkeleton />}>
        <BlogList />
      </Suspense>
    </main>
  );
}
```

```typescript
// src/components/BlogList.tsx
async function BlogList() {
  const posts = await fetchPosts(); // This can be slow
  return (
    <ul>
      {posts.map((p) => <li key={p.id}>{p.title}</li>)}
    </ul>
  );
}
```
