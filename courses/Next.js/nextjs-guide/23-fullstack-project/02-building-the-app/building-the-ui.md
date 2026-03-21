# Building the UI

## What You'll Learn
- Dashboard page
- Task components
- Forms

## Prerequisites
- Server actions ready

## Do I Need This Right Now?
The UI is what users see and interact with.

## Dashboard Page

This references **Section 07 (Styling)** and **Section 06 (Rendering/Suspense)**.

```typescript
// app/dashboard/page.tsx
import { auth } from '@/lib/auth';
import { prisma } from '@/lib/db';
import { TaskList } from '@/components/TaskList';
import { TaskForm } from '@/components/TaskForm';

export default async function DashboardPage() {
  const session = await auth();
  
  const tasks = await prisma.task.findMany({
    where: { userId: session?.user?.id },
    orderBy: { createdAt: 'desc' }
  });
  
  return (
    <div className="max-w-2xl mx-auto p-8">
      <h1 className="text-2xl font-bold mb-6">My Tasks</h1>
      <TaskForm />
      <TaskList tasks={tasks} />
    </div>
  );
}
```

## Task List Component

```typescript
// components/TaskList.tsx
'use client';

import { toggleTask, deleteTask } from '@/app/actions/tasks';

export function TaskList({ tasks }) {
  return (
    <ul className="space-y-2 mt-6">
      {tasks.map(task => (
        <li key={task.id} className="flex items-center gap-3 p-3 bg-white rounded border">
          <input
            type="checkbox"
            checked={task.completed}
            onChange={() => toggleTask(task.id, !task.completed)}
            className="w-5 h-5"
          />
          <span className={task.completed ? 'line-through text-gray-400' : ''}>
            {task.title}
          </span>
          <button
            onClick={() => deleteTask(task.id)}
            className="ml-auto text-red-500"
          >
            Delete
          </button>
        </li>
      ))}
    </ul>
  );
}
```

## Summary
- Dashboard shows all tasks
- Client components for interactivity
- Server Actions for mutations
