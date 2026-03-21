# Forms with Server Actions

## What You'll Learn
- How to use forms with Server Actions
- Progressive enhancement
- Form validation

## Prerequisites
- Understanding of Server Actions
- Basic HTML form knowledge

## Concept Explained Simply

Forms and Server Actions work beautifully together in Next.js. You can connect a form directly to a Server Action, and it will work even if JavaScript hasn't loaded yet (progressive enhancement).

Think of it like a magic mail slot. You drop your letter (form data) in, and somehow it appears on the server processed and sorted. You don't need to know how the sorting works — you just drop it in and it happens.

## How It Works

1. Create a Server Action that accepts `FormData`
2. Set the form's `action` prop to the Server Action
3. When submitted, the Server Action runs on the server
4. The form is automatically reset after successful submission

## Complete Code Example

### Basic Form

```typescript
// src/app/actions.ts - Server Action
"use server";

import { revalidatePath } from "next/cache";

export async function subscribeToNewsletter(formData: FormData) {
  const email = formData.get("email") as string;
  
  // Validate email
  if (!email || !email.includes("@")) {
    return { error: "Please enter a valid email" };
  }
  
  // Simulate saving to database
  console.log("Subscribing:", email);
  
  // Revalidate the home page
  revalidatePath("/");
  
  return { success: true, message: "Thanks for subscribing!" };
}
```

```typescript
// src/components/NewsletterForm.tsx - Client Component with form
"use client";

import { useRef, useState } from "react";
import { subscribeToNewsletter } from "@/app/actions";

export function NewsletterForm() {
  const [message, setMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const formRef = useRef<HTMLFormElement>(null);

  return (
    <form
      ref={formRef}
      action={async (formData) => {
        setIsLoading(true);
        const result = await subscribeToNewsletter(formData);
        setIsLoading(false);
        
        if (result.error) {
          setMessage(result.error);
        } else {
          setMessage(result.message);
          formRef.current?.reset();
        }
      }}
      style={{ display: "flex", gap: "0.5rem" }}
    >
      <input
        type="email"
        name="email"
        placeholder="Enter your email"
        required
        style={{ padding: "0.5rem", flex: 1 }}
      />
      <button
        type="submit"
        disabled={isLoading}
        style={{
          padding: "0.5rem 1rem",
          backgroundColor: "#0070f3",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: isLoading ? "not-allowed" : "pointer",
        }}
      >
        {isLoading ? "Subscribing..." : "Subscribe"}
      </button>
      {message && <p style={{ margin: 0 }}>{message}</p>}
    </form>
  );
}
```

### Contact Form with Validation

```typescript
// src/app/contact/actions.ts
"use server";

import { z } from "zod";

// Define validation schema
const contactSchema = z.object({
  name: z.string().min(2, "Name must be at least 2 characters"),
  email: z.string().email("Please enter a valid email"),
  message: z.string().min(10, "Message must be at least 10 characters"),
});

export async function sendContactEmail(prevState: any, formData: FormData) {
  const rawData = {
    name: formData.get("name"),
    email: formData.get("email"),
    message: formData.get("message"),
  };
  
  // Validate the data
  const result = contactSchema.safeParse(rawData);
  
  if (!result.success) {
    return {
      errors: result.error.flatten().fieldErrors,
      message: "Please fix the errors above",
    };
  }
  
  // Send email (simulated)
  console.log("Sending email:", result.data);
  
  return { success: true, message: "Message sent successfully!" };
}
```

```typescript
// src/app/contact/page.tsx - Using useFormState
"use client";

"use client";

import { useFormState } from "react-dom";
import { sendContactEmail } from "../actions";

const initialState = {
  message: "",
  errors: undefined,
};

export function ContactForm() {
  const [state, formAction] = useFormState(sendContactEmail, initialState);

  return (
    <form action={formAction} style={{ display: "flex", flexDirection: "column", gap: "1rem", maxWidth: "400px" }}>
      <div>
        <label htmlFor="name" style={{ display: "block", marginBottom: "0.25rem" }}>
          Name
        </label>
        <input
          id="name"
          name="name"
          type="text"
          style={{ width: "100%", padding: "0.5rem" }}
        />
        {state?.errors?.name && (
          <p style={{ color: "red", fontSize: "0.875rem" }}>{state.errors.name[0]}</p>
        )}
      </div>
      
      <div>
        <label htmlFor="email" style={{ display: "block", marginBottom: "0.25rem" }}>
          Email
        </label>
        <input
          id="email"
          name="email"
          type="email"
          style={{ width: "100%", padding: "0.5rem" }}
        />
        {state?.errors?.email && (
          <p style={{ color: "red", fontSize: "0.875rem" }}>{state.errors.email[0]}</p>
        )}
      </div>
      
      <div>
        <label htmlFor="message" style={{ display: "block", marginBottom: "0.25rem" }}>
          Message
        </label>
        <textarea
          id="message"
          name="message"
          rows={4}
          style={{ width: "100%", padding: "0.5rem" }}
        />
        {state?.errors?.message && (
          <p style={{ color: "red", fontSize: "0.875rem" }}>{state.errors.message[0]}</p>
        )}
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
        Send Message
      </button>
      
      {state?.message && (
        <p style={{ color: state.success ? "green" : "red" }}>{state.message}</p>
      )}
    </form>
  );
}
```

## Progressive Enhancement

Forms with Server Actions work even without JavaScript:

1. User submits form
2. Browser sends POST request to server
3. Server Action runs and processes data
4. Page revalidates and updates
5. User sees the result

This is called **progressive enhancement** — the form works with or without JavaScript.

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `formData.get("email")` | Get form field value | Access submitted data |
| `z.object({...})` | Validation schema | Validate input data |
| `useFormState` | Hook for form state | Handle form errors/success |
| `formRef.current?.reset()` | Clear form after submit | Better UX |

## Common Mistakes

### Mistake #1: Not Validating Input

```typescript
// ✗ Wrong: No validation
export async function createPost(formData: FormData) {
  const title = formData.get("title");
  // No check if title is valid!
  await db.post.create({ title });
}

// ✓ Correct: Validate before processing
export async function createPost(formData: FormData) {
  const title = formData.get("title") as string;
  
  if (!title || title.length < 3) {
    throw new Error("Title must be at least 3 characters");
  }
  
  await db.post.create({ title });
}
```

### Mistake #2: Not Handling Errors

```typescript
// ✗ Wrong: Errors not shown to user
export async function submitForm(formData: FormData) {
  await db.create(formData);
  return { success: true };
}

// Return error information to show user
export async function submitForm(formData: FormData) {
  try {
    await db.create(formData);
    return { success: true };
  } catch (error) {
    return { success: false, error: "Failed to submit" };
  }
}
```

### Mistake #3: Forgetting to Reset Form

```typescript
// ✗ Wrong: Form keeps old data after submit
<form action={action}>
  <input name="email" />
</form>

// Reset the form after successful submission
<form action={async (formData) => {
  await action(formData);
  formRef.current?.reset();
}}>
  <input name="email" />
</form>
```

## Summary

- Connect forms directly to Server Actions via `action` prop
- Use `useFormState` for better error handling
- Always validate form input on the server
- Progressive enhancement: forms work without JavaScript

## Next Steps

Now let's learn about optimistic updates:

- [Optimistic Updates →](../../04-server-actions/02-advanced-patterns/optimistic-updates.md)
