# Server Actions in Next.js 14

## Overview
Server Actions are async functions that run on the server but can be called directly from Client Components. They eliminate the need for API routes for many use cases while maintaining security and enabling progressive enhancement.

## Prerequisites
- Next.js 14+ fundamentals
- Server and Client Components
- Form handling basics

## Core Concepts

### What Are Server Actions?

Server Actions are functions defined with `'use server'` that run exclusively on the server. They're called like regular functions from Client Components but execute server-side.

```tsx
// [File: app/actions.ts]
'use server';

/**
 * This function runs ONLY on the server.
 * It can access databases, environment variables, and other server resources.
 * It's secure by default — not exposed as an API endpoint.
 */
export async function createTodo(formData: FormData) {
  'use server';
  
  const title = formData.get('title') as string;
  
  // Direct database access - no API needed!
  await db.todo.create({
    data: { title, completed: false }
  });
  
  // Revalidate the cache to update UI
  revalidatePath('/todos');
}
```

### Using 'use server' Directive

The `'use server'` directive can be placed at the top of a file or within a function.

```tsx
// [File: app/actions.ts]
// File-level: All functions in this file are Server Actions
'use server';

export async function createTodo(data: TodoData) { /* ... */ }
export async function deleteTodo(id: string) { /* ... */ }
export async function updateTodo(id: string, data: Partial<TodoData>) { /* ... */ }
```

```tsx
// [File: app/components/Form.tsx]
// Inline: Only this specific function is a Server Action
'use client';

import { createTodo } from '@/app/actions';

export function TodoForm() {
  async function handleSubmit(formData: FormData) {
    'use server';
    // This runs on the server
    await createTodo(formData);
  }
  
  return (
    <form action={handleSubmit}>
      <input name="title" />
      <button type="submit">Add</button>
    </form>
  );
}
```

### useFormState Hook

Connect Server Actions to forms with useFormState for proper state management.

```tsx
// [File: app/components/ContactForm.tsx]
'use client';

import { useFormState } from 'react-dom';
import { submitContactForm } from '@/app/actions';

const initialState = {
  message: '',
  errors: null,
};

export function ContactForm() {
  // useFormState binds the Server Action to the form
  // Returns [state, formAction] tuple
  const [state, formAction] = useFormState(submitContactForm, initialState);

  return (
    <form action={formAction}>
      <div>
        <label htmlFor="email">Email</label>
        <input type="email" name="email" id="email" required />
      </div>
      
      <div>
        <label htmlFor="message">Message</label>
        <textarea name="message" id="message" required />
      </div>
      
      {state?.errors && (
        <ul>
          {state.errors.map(error => (
            <li key={error}>{error}</li>
          ))}
        </ul>
      )}
      
      {state?.message && (
        <p className="success">{state.message}</p>
      )}
      
      <button type="submit">Send</button>
    </form>
  );
}
```

### useFormStatus Hook

Get pending state during form submission without passing props.

```tsx
// [File: app/components/SubmitButton.tsx]
'use client';

import { useFormStatus } from 'react-dom';

export function SubmitButton() {
  const { pending, data, method } = useFormStatus();
  
  // pending is true while the form is submitting
  return (
    <button disabled={pending}>
      {pending ? 'Submitting...' : 'Submit'}
    </button>
  );
}
```

### Revalidation in Server Actions

Server Actions can invalidate cached data to update the UI.

```tsx
// [File: app/actions.ts]
'use server';

import { revalidatePath, revalidateTag } from 'next/cache';

export async function createTodo(data: TodoData) {
  await db.todo.create({ data });
  
  // Revalidate a specific path
  revalidatePath('/todos');
  
  // OR: Revalidate by cache tag
  revalidateTag('todos');
}

export async function deleteTodo(id: string) {
  await db.todo.delete({ where: { id } });
  
  // This will refresh all data fetching tagged with 'todos'
  revalidateTag('todos');
}
```

## Common Mistakes

### ❌ Using fetch instead of Server Actions / ✅ Fix

```tsx
// ❌ WRONG - Using API route for simple mutations
const handleSubmit = async () => {
  await fetch('/api/todos', {
    method: 'POST',
    body: JSON.stringify(data),
  });
  reFetchTodos();
};

// ✅ CORRECT - Using Server Action directly
const handleSubmit = async (formData: FormData) => {
  'use server';
  await createTodo(formData);
};

<form action={handleSubmit}>...</form>
```

### ❌ Not Handling Errors / ✅ Fix

```tsx
// ❌ WRONG - No error handling
export async function createTodo(data: TodoData) {
  await db.todo.create({ data });
}

// ✅ CORRECT - Return error state
export async function createTodo(prevState: State, formData: FormData) {
  try {
    await db.todo.create({ data: { title: formData.get('title') } });
    revalidatePath('/todos');
    return { success: true, message: 'Created!' };
  } catch (e) {
    return { success: false, message: 'Failed to create' };
  }
}
```

### ❌ Exposing Sensitive Data / ✅ Fix

```tsx
// ❌ WRONG - Returning sensitive data
export async function getUser(id: string) {
  const user = await db.user.findUnique({ where: { id } });
  return user; // Could leak password hash, etc!
}

// ✅ CORRECT - Only return needed fields
export async function getUser(id: string) {
  const user = await db.user.findUnique({ 
    where: { id },
    select: { id: true, name: true, email: true } // Only public fields
  });
  return user;
}
```

## Real-World Example: Complete Contact Form

This example shows a complete contact form with Server Actions, Zod validation, and proper error handling.

```tsx
// [File: app/actions/contact.ts]
'use server';

import { z } from 'zod';
import { revalidatePath } from 'next/cache';

// Zod schema for validation
const ContactSchema = z.object({
  name: z.string().min(2, 'Name must be at least 2 characters'),
  email: z.string().email('Invalid email address'),
  message: z.string().min(10, 'Message must be at least 10 characters'),
});

type ContactState = {
  errors?: {
    name?: string[];
    email?: string[];
    message?: string[];
  };
  message?: string;
};

/**
 * Server Action for contact form submission.
 * Validates with Zod and saves to database.
 */
export async function submitContactForm(
  prevState: ContactState,
  formData: FormData
): Promise<ContactState> {
  // 1. Validate form data
  const validatedFields = ContactSchema.safeParse({
    name: formData.get('name'),
    email: formData.get('email'),
    message: formData.get('message'),
  });

  if (!validatedFields.success) {
    return {
      errors: validatedFields.error.flatten().fieldErrors,
    };
  }

  // 2. Save to database (simulated)
  try {
    // await db.contact.create({ data: validatedFields.data });
    console.log('Saving to database:', validatedFields.data);
    
    // Simulate delay
    await new Promise(resolve => setTimeout(resolve, 1000));
    
  } catch (error) {
    return {
      message: 'Failed to send message. Please try again.',
    };
  }

  // 3. Revalidate the page to show new data
  revalidatePath('/contact');
  
  // 4. Return success
  return {
    message: 'Message sent successfully!',
  };
}
```

```tsx
// [File: app/components/ContactForm.tsx]
'use client';

import { useFormState } from 'react-dom';
import { submitContactForm } from '@/app/actions/contact';

const initialState = {
  message: '',
  errors: undefined,
};

export function ContactForm() {
  const [state, formAction] = useFormState(submitContactForm, initialState);

  return (
    <form action={formAction} className="space-y-4">
      {/* Name field */}
      <div>
        <label htmlFor="name" className="block text-sm font-medium">
          Name
        </label>
        <input
          type="text"
          name="name"
          id="name"
          className="mt-1 block w-full rounded border p-2"
          aria-describedby="name-error"
        />
        {state?.errors?.name && (
          <p id="name-error" className="text-red-500 text-sm">
            {state.errors.name[0]}
          </p>
        )}
      </div>

      {/* Email field */}
      <div>
        <label htmlFor="email" className="block text-sm font-medium">
          Email
        </label>
        <input
          type="email"
          name="email"
          id="email"
          className="mt-1 block w-full rounded border p-2"
        />
        {state?.errors?.email && (
          <p className="text-red-500 text-sm">
            {state.errors.email[0]}
          </p>
        )}
      </div>

      {/* Message field */}
      <div>
        <label htmlFor="message" className="block text-sm font-medium">
          Message
        </label>
        <textarea
          name="message"
          id="message"
          rows={4}
          className="mt-1 block w-full rounded border p-2"
        />
        {state?.errors?.message && (
          <p className="text-red-500 text-sm">
            {state.errors.message[0]}
          </p>
        )}
      </div>

      {/* Success message */}
      {state?.message && !state.errors && (
        <p className="text-green-600">{state.message}</p>
      )}

      {/* Submit button */}
      <button
        type="submit"
        className="rounded bg-blue-600 px-4 py-2 text-white hover:bg-blue-700"
      >
        Send Message
      </button>
    </form>
  );
}
```

## Key Takeaways

- Server Actions are defined with `'use server'` directive
- They run only on the server but can be called from Client Components
- They support progressive enhancement — forms work without JavaScript
- They're not exposed as public API endpoints (more secure)
- Use revalidatePath or revalidateTag to update cached data
- useFormState binds Server Actions to form state
- useFormStatus provides pending state during submission

## What's Next

Continue to [Next.js Caching Explained](04-nextjs-advanced/01-nextjs-caching-explained.md) to understand the four caching layers in Next.js.