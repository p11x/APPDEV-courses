# Server Action Errors

## What You'll Learn
- Capture errors from Server Actions
- Handle validation errors
- Return meaningful error messages

## Prerequisites
- Understanding of Server Actions
- Sentry configured

## Do I Need This Right Now?
Server Actions are how you handle mutations in Next.js 15. Knowing how to capture and handle errors from them is crucial for production apps.

## Concept Explained Simply

Server Actions are like kitchen staff taking orders. When something goes wrong in the kitchen, you need to let the customer (UI) know what happened. Errors in Server Actions need special handling because they're called from the client.

## Basic Error Handling

```typescript
// app/actions.ts
'use server';

import * as Sentry from '@sentry/nextjs';

export async function createUser(formData: FormData) {
  const name = formData.get('name');
  const email = formData.get('email');
  
  try {
    // Validate inputs
    if (!name || !email) {
      throw new Error('Name and email are required');
    }
    
    // Simulate database operation
    const user = await createUserInDatabase(name.toString(), email.toString());
    
    return { success: true, user };
  } catch (error) {
    // Capture the error
    Sentry.captureException(error, {
      tags: {
        action: 'createUser',
      },
      extra: {
        name,
        email,
      },
    });
    
    // Return error to client
    return { 
      success: false, 
      error: error instanceof Error ? error.message : 'Failed to create user' 
    };
  }
}

async function createUserInDatabase(name: string, email: string) {
  // Simulate database
  if (email.toString().includes('error')) {
    throw new Error('Database error');
  }
  
  return { id: '123', name, email };
}
```

## Client-Side Error Handling

```typescript
// components/CreateUserForm.tsx
'use client';

import { createUser } from '@/app/actions';

export function CreateUserForm() {
  async function handleSubmit(formData: FormData) {
    const result = await createUser(formData);
    
    if (!result.success) {
      alert(`Error: ${result.error}`);
      return;
    }
    
    alert('User created successfully!');
  }

  return (
    <form action={handleSubmit} className="space-y-4">
      <div>
        <label>Name</label>
        <input name="name" type="text" className="border p-2 block w-full" />
      </div>
      <div>
        <label>Email</label>
        <input name="email" type="email" className="border p-2 block w-full" />
      </div>
      <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">
        Create User
      </button>
    </form>
  );
}
```

## Using useFormState for Better Errors

```typescript
// app/actions.ts
'use server';

import { z } from 'zod';

const schema = z.object({
  name: z.string().min(2, 'Name must be at least 2 characters'),
  email: z.string().email('Invalid email address'),
});

export async function createUserWithValidation(
  prevState: { errors?: Record<string, string[]> } | null,
  formData: FormData
) {
  const data = {
    name: formData.get('name'),
    email: formData.get('email'),
  };
  
  // Validate
  const result = schema.safeParse(data);
  
  if (!result.success) {
    const errors: Record<string, string[]> = {};
    result.error.issues.forEach((issue) => {
      const field = issue.path[0];
      if (!errors[field]) {
        errors[field] = [];
      }
      errors[field].push(issue.message);
    });
    
    return { errors };
  }
  
  try {
    // Create user in database
    await createUserInDatabase(result.data);
    
    return { success: true };
  } catch (error) {
    return { 
      errors: { _form: ['Failed to create user. Please try again.'] } 
    };
  }
}
```

```typescript
// components/FormWithValidation.tsx
'use client';

import { useFormState } from 'react-dom';
import { createUserWithValidation } from '@/app/actions';

const initialState = {
  errors: undefined,
  success: false,
};

export function FormWithValidation() {
  const [state, formAction] = useFormState(createUserWithValidation, initialState);

  return (
    <form action={formAction} className="space-y-4">
      {state.errors?._form && (
        <div className="text-red-500">
          {state.errors._form.map((error) => (
            <p key={error}>{error}</p>
          ))}
        </div>
      )}
      
      <div>
        <input name="name" placeholder="Name" className="border p-2" />
        {state.errors?.name && (
          <p className="text-red-500 text-sm">{state.errors.name[0]}</p>
        )}
      </div>
      
      <div>
        <input name="email" placeholder="Email" className="border p-2" />
        {state.errors?.email && (
          <p className="text-red-500 text-sm">{state.errors.email[0]}</p>
        )}
      </div>
      
      <button type="submit" className="bg-blue-500 text-white px-4 py-2">
        Submit
      </button>
    </form>
  );
}
```

## Throwing vs Returning Errors

```typescript
// Option 1: Throw errors (Sentry captures automatically)
'use server';

export async function actionWithThrow() {
  throw new Error('This will be captured by Sentry');
}

// Option 2: Return error objects (manual capture needed)
'use server';

export async function actionWithReturn() {
  try {
    // Do something
  } catch (error) {
    // Must manually capture
    return { error: 'Failed' };
  }
}
```

## Common Mistakes

### Mistake #1: Not Capturing Validation Errors
```typescript
// Wrong: Validation errors not captured
export async function createUser(formData: FormData) {
  if (!formData.get('name')) {
    // Just return, Sentry doesn't know!
    return { error: 'Name required' };
  }
}
```

```typescript
// Correct: Capture validation errors too
import * as Sentry from '@sentry/nextjs';

export async function createUser(formData: FormData) {
  if (!formData.get('name')) {
    // Capture for debugging
    Sentry.captureMessage('Validation: Name required', 'warning');
    return { error: 'Name required' };
  }
}
```

### Mistake #2: Exposing Internal Errors
```typescript
// Wrong: Exposing internal error details
catch (error) {
  return { error: error.message }; // Could expose sensitive info!
}
```

```typescript
// Correct: Generic error message
catch (error) {
  // Log internally
  Sentry.captureException(error);
  
  // Return safe message
  return { error: 'An error occurred. Please try again.' };
}
```

### Mistake #3: Not Handling Redirect Errors
```typescript
// Wrong: Redirect doesn't work after error
catch (error) {
  Sentry.captureException(error);
  redirect('/error'); // This won't work after response started!
}
```

```typescript
// Correct: Handle properly
import { redirect } from 'next/navigation';

async function safeAction(formData: FormData) {
  try {
    await doSomething();
    redirect('/success');
  } catch (error) {
    Sentry.captureException(error);
    // Can't redirect after error, return state instead
    return { error: 'Failed' };
  }
}
```

## Summary
- Server Action errors need to be captured and returned to client
- Use try/catch to capture with Sentry
- Return error objects, not thrown errors (for form state)
- Use useFormState/useFormAction for better UX
- Don't expose internal error details to clients
- Always capture validation errors for debugging

## Next Steps
- [performance-monitoring.md](../03-sentry-advanced/performance-monitoring.md) — Performance tracking
