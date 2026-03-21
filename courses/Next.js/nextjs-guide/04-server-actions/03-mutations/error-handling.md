# Error Handling in Server Actions

## What You'll Learn
- Handling errors in Server Actions
- Returning error states
- Showing errors to users

## Complete Code Example

```typescript
// src/app/actions.ts
"use server";

export async function createPost(formData: FormData) {
  try {
    const title = formData.get("title") as string;
    
    if (!title || title.length < 3) {
      return { error: "Title must be at least 3 characters" };
    }
    
    // await prisma.post.create({ data: { title } });
    
    return { success: true };
  } catch (error) {
    return { error: "Failed to create post" };
  }
}
```

```typescript
// src/components/PostForm.tsx
"use client";

import { useState } from "react";
import { createPost } from "@/app/actions";

export function PostForm() {
  const [error, setError] = useState("");
  
  return (
    <form action={async (formData) => {
      const result = await createPost(formData);
      if (result.error) {
        setError(result.error);
      }
    }}>
      <input name="title" />
      <button type="submit">Create</button>
      {error && <p>{error}</p>}
    </form>
  );
}
```

## Summary

- Always return error states instead of throwing
- Catch blocks handle unexpected errors
- Display errors in UI
