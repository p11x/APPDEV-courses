# Optimistic Updates with Server Actions

## What You'll Learn
- What optimistic updates are
- How to implement them with useOptimistic
- Improving perceived performance

## Prerequisites
- Understanding of Server Actions
- Basic React hooks knowledge

## Concept Explained Simply

**Optimistic updates** make your app feel instant by updating the UI immediately, before the server actually processes the request. If the server fails, you show an error and roll back.

Think of it like sending a text message. Your phone shows "sent" immediately, even though it might take a second to actually reach the server. If it fails, it shows "failed" and lets you retry.

## Complete Code Example

```typescript
// src/app/actions.ts - Server Actions
"use server";

import { revalidatePath } from "next/cache";

export async function toggleLike(postId: string) {
  // Simulate database update
  // await db.like.toggle({ postId });
  
  revalidatePath("/posts");
  return { liked: true };
}

export async function addComment(postId: string, comment: string) {
  // await db.comment.create({ postId, text: comment });
  
  revalidatePath("/posts");
  return { id: Date.now().toString(), text: comment };
}
```

```typescript
// src/components/Post.tsx - With optimistic updates
"use client";

import { useOptimistic } from "react";
import { toggleLike, addComment } from "@/app/actions";

interface Comment {
  id: string;
  text: string;
}

interface PostProps {
  post: {
    id: string;
    content: string;
    likes: number;
    comments: Comment[];
  };
}

export function Post({ post }: PostProps) {
  // Optimistic state for likes
  const [optimisticLikes, setOptimisticLikes] = useOptimistic(
    post.likes,
    (state, newLike: boolean) => (newLike ? state + 1 : state - 1)
  );

  // Optimistic state for comments
  const [optimisticComments, setOptimisticComments] = useOptimistic(
    post.comments,
    (state, newComment: Comment) => [...state, newComment]
  );

  const handleLike = async () => {
    setOptimisticLikes(true); // Update UI immediately
    await toggleLike(post.id); // Then call server
  };

  const handleAddComment = async (comment: string) => {
    const tempComment = { id: "temp", text: comment };
    setOptimisticComments(tempComment); // Add immediately
    await addComment(post.id, comment); // Then call server
  };

  return (
    <article style={{ padding: "1rem", border: "1px solid #ddd", marginBottom: "1rem" }}>
      <p>{post.content}</p>
      
      <button onClick={handleLike}>
        ❤️ {optimisticLikes} Likes
      </button>
      
      <div style={{ marginTop: "1rem" }}>
        <h4>Comments</h4>
        {optimisticComments.map((c) => (
          <p key={c.id} style={{ fontSize: "0.9rem" }}>{c.text}</p>
        ))}
      </div>
    </article>
  );
}
```

## How Optimistic Updates Work

1. User clicks button
2. `useOptimistic` updates UI immediately
3. Server Action runs in background
4. If successful, final state matches
5. If failed, would need manual rollback

## Summary

- Use `useOptimistic` hook for instant UI updates
- Update state before server responds
- Works great for likes, comments, toggles

## Next Steps

Learn about validation with Zod:

- [Server Action with Zod →](./server-action-with-zod.md)
