# Server Action with Zod Validation

## What You'll Learn
- Using Zod for form validation
- Type-safe Server Actions
- Better error handling

## Prerequisites
- Understanding of Server Actions
- Basic TypeScript knowledge

## Concept Explained Simply

**Zod** is a TypeScript library for schema validation. It lets you define what your data should look like, then validates against that schema. Combined with Server Actions, you get type-safe, validated form submissions.

## Complete Code Example

```typescript
// src/lib/schemas.ts
import { z } from "zod";

export const CreatePostSchema = z.object({
  title: z.string().min(3, "Title must be at least 3 characters"),
  content: z.string().min(10, "Content must be at least 10 characters"),
  published: z.boolean().optional(),
});

export const ContactFormSchema = z.object({
  name: z.string().min(2, "Name is required"),
  email: z.string().email("Invalid email address"),
  message: z.string().min(10, "Message must be at least 10 characters"),
});

export type CreatePostInput = z.infer<typeof CreatePostSchema>;
```

```typescript
// src/app/actions.ts
"use server";

import { revalidatePath } from "next/cache";
import { CreatePostSchema, CreatePostInput } from "@/lib/schemas";

export async function createPost(prevState: any, formData: FormData) {
  // Extract raw data
  const rawData = {
    title: formData.get("title"),
    content: formData.get("content"),
    published: formData.get("published") === "on",
  };

  // Validate with Zod
  const result = CreatePostSchema.safeParse(rawData);

  if (!result.success) {
    return {
      errors: result.error.flatten().fieldErrors,
      message: "Please fix the validation errors",
    };
  }

  // TypeScript now knows the shape of data
  const validatedData: CreatePostInput = result.data;

  try {
    // Save to database
    // await prisma.post.create({ data: validatedData });
    console.log("Creating post:", validatedData);

    revalidatePath("/blog");
    return { success: true, message: "Post created!" };
  } catch (error) {
    return { success: false, message: "Failed to create post" };
  }
}
```

## Summary

- Zod provides runtime validation with TypeScript types
- Use `safeParse()` for validation
- Return formatted errors to the client
