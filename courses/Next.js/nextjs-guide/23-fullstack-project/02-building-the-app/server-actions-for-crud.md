# Server Actions for CRUD

## What You'll Learn
- Create, read, update, delete
- Server Actions
- Revalidation

## Prerequisites
- Auth and schema ready

## Do I Need This Right Now?
Server Actions handle all task mutations.

## CRUD Actions

This references **Section 04 (Server Actions)** — mutations and revalidation.

```typescript
// app/actions/tasks.ts
'use server';

import { prisma } from '@/lib/db';
import { auth } from '@/lib/auth';
import { revalidatePath } from 'next/cache';

export async function createTask(formData: FormData) {
  const session = await auth();
  if (!session?.user?.id) throw new Error('Unauthorized');
  
  const title = formData.get('title') as string;
  
  await prisma.task.create({
    data: {
      title,
      userId: session.user.id,
    }
  });
  
  revalidatePath('/dashboard');
}

export async function toggleTask(id: string, completed: boolean) {
  const session = await auth();
  if (!session?.user?.id) throw new Error('Unauthorized');
  
  await prisma.task.update({
    where: { id, userId: session.user.id },
    data: { completed }
  });
  
  revalidatePath('/dashboard');
}

export async function deleteTask(id: string) {
  const session = await auth();
  if (!session?.user?.id) throw new Error('Unauthorized');
  
  await prisma.task.delete({
    where: { id, userId: session.user.id }
  });
  
  revalidatePath('/dashboard');
}
```

## Summary
- Use Server Actions for mutations
- Always check authorization
- Revalidate after changes
