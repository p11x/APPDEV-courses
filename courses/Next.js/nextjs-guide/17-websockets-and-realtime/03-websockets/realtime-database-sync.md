# Real-Time Database Sync

## What You'll Learn
- Subscribe to database changes in real-time
- Use Supabase Realtime with Next.js
- Build collaborative features with live updates

## Prerequisites
- Understanding of WebSockets or SSE
- Basic knowledge of databases (PostgreSQL)

## Do I Need This Right Now?
This is essential for building collaborative applications like Google Docs, multiplayer games, or any app where multiple users need to see the same data update instantly. If you're building a simple CRUD app with occasional updates, regular Server Actions with revalidation are sufficient.

## Concept Explained Simply

Real-time database sync is like having a shared whiteboard where everyone sees drawings appear instantly, no matter who draws them. Instead of users refreshing the page to see new data, the database itself notifies your app when something changes. This is much more efficient than repeatedly asking "did anything change?" (polling).

Services like Supabase, Firebase, and Pusher provide this functionality. We'll look at Supabase as an example.

## Complete Code Example

First, let's set up Supabase client:

```typescript
// lib/supabase/client.ts
import { createClient } from '@supabase/supabase-js';

const supabaseUrl = process.env.NEXT_PUBLIC_SUPABASE_URL!;
const supabaseAnonKey = process.env.NEXT_PUBLIC_SUPABASE_ANON_KEY!;

export const supabase = createClient(supabaseUrl, supabaseAnonKey);
```

Now let's create a real-time todo list component:

```typescript
// app/components/RealtimeTodos.tsx
'use client';

import { useEffect, useState } from 'react';
import { supabase } from '@/lib/supabase/client';

interface Todo {
  id: string;
  title: string;
  completed: boolean;
  created_at: string;
}

export default function RealtimeTodos() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [newTodo, setNewTodo] = useState('');
  const [loading, setLoading] = useState(true);

  // Fetch initial todos
  useEffect(() => {
    async function fetchTodos() {
      const { data, error } = await supabase
        .from('todos')
        .select('*')
        .order('created_at', { ascending: false });

      if (!error && data) {
        setTodos(data);
      }
      setLoading(false);
    }

    fetchTodos();

    // Subscribe to real-time changes
    const channel = supabase
      .channel('todos')
      .on(
        'postgres_changes',
        {
          event: 'INSERT',
          schema: 'public',
          table: 'todos',
        },
        (payload) => {
          console.log('New todo added:', payload.new);
          setTodos(prev => [payload.new as Todo, ...prev]);
        }
      )
      .on(
        'postgres_changes',
        {
          event: 'UPDATE',
          schema: 'public',
          table: 'todos',
        },
        (payload) => {
          console.log('Todo updated:', payload.new);
          setTodos(prev =>
            prev.map(todo =>
              todo.id === payload.new.id
                ? { ...todo, ...(payload.new as Todo) }
                : todo
            )
          );
        }
      )
      .on(
        'postgres_changes',
        {
          event: 'DELETE',
          schema: 'public',
          table: 'todos',
        },
        (payload) => {
          console.log('Todo deleted:', payload.old);
          setTodos(prev =>
            prev.filter(todo => todo.id !== (payload.old as Todo).id)
          );
        }
      )
      .subscribe();

    // Cleanup subscription on unmount
    return () => {
      supabase.removeChannel(channel);
    };
  }, []);

  // Add new todo
  async function addTodo(e: React.FormEvent) {
    e.preventDefault();
    if (!newTodo.trim()) return;

    const { error } = await supabase
      .from('todos')
      .insert({ title: newTodo.trim(), completed: false });

    if (!error) {
      setNewTodo('');
    }
  }

  // Toggle todo completion
  async function toggleTodo(todo: Todo) {
    const { error } = await supabase
      .from('todos')
      .update({ completed: !todo.completed })
      .eq('id', todo.id);

    if (error) {
      console.error('Error updating todo:', error);
    }
  }

  // Delete todo
  async function deleteTodo(id: string) {
    const { error } = await supabase
      .from('todos')
      .delete()
      .eq('id', id);

    if (error) {
      console.error('Error deleting todo:', error);
    }
  }

  if (loading) {
    return <div className="p-4">Loading todos...</div>;
  }

  return (
    <div className="max-w-md mx-auto p-4">
      <h2 className="text-xl font-bold mb-4">Real-Time Todos</h2>
      <p className="text-sm text-gray-600 mb-4">
        Changes sync instantly across all connected users
      </p>

      <form onSubmit={addTodo} className="flex gap-2 mb-4">
        <input
          type="text"
          value={newTodo}
          onChange={(e) => setNewTodo(e.target.value)}
          placeholder="Add a new todo..."
          className="flex-1 px-3 py-2 border rounded-lg"
        />
        <button
          type="submit"
          className="px-4 py-2 bg-blue-500 text-white rounded-lg"
        >
          Add
        </button>
      </form>

      <ul className="space-y-2">
        {todos.map((todo) => (
          <li
            key={todo.id}
            className="flex items-center gap-3 p-3 bg-white border rounded-lg"
          >
            <input
              type="checkbox"
              checked={todo.completed}
              onChange={() => toggleTodo(todo)}
              className="w-5 h-5"
            />
            <span className={`flex-1 ${todo.completed ? 'line-through text-gray-400' : ''}`}>
              {todo.title}
            </span>
            <button
              onClick={() => deleteTodo(todo.id)}
              className="text-red-500 hover:text-red-700"
            >
              Delete
            </button>
          </li>
        ))}
      </ul>

      {todos.length === 0 && (
        <p className="text-center text-gray-500 py-8">No todos yet</p>
      )}
    </div>
  );
}
```

## Server Action for Initial Data (Optional Enhancement)

You can also combine real-time subscriptions with Server Actions for mutations:

```typescript
// app/actions/todos.ts
'use server';

import { supabase } from '@/lib/supabase/server';
import { revalidatePath } from 'next/cache';

export async function addTodo(formData: FormData) {
  const title = formData.get('title') as string;
  
  const { error } = await supabase
    .from('todos')
    .insert({ title, completed: false });

  if (error) {
    return { error: error.message };
  }

  // Revalidate for non-subscribed clients
  revalidatePath('/');
  
  return { success: true };
}

export async function toggleTodo(id: string, completed: boolean) {
  const { error } = await supabase
    .from('todos')
    .update({ completed })
    .eq('id', id);

  if (error) {
    return { error: error.message };
  }

  revalidatePath('/');
  return { success: true };
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `supabase.channel('todos')` | Creates a subscription channel | Enables real-time listening |
| `.on('postgres_changes', ...)` | Listens for DB changes | Watches INSERT, UPDATE, DELETE |
| `payload.new` | Contains new row data | The inserted/updated record |
| `.subscribe()` | Activates the subscription | Must call to start receiving events |
| `supabase.removeChannel()` | Cleans up subscription | Prevents memory leaks |
| `revalidatePath()` | Refreshes server data | Updates non-subscribed clients |

## Common Mistakes

### Mistake #1: Not Enabling Realtime in Supabase Dashboard
```typescript
// Wrong: Subscription won't work if not configured
const channel = supabase.channel('todos').on(...)
```

You must enable realtime on the table in Supabase dashboard:
1. Go to Database → Tables → Select table
2. Click "Edit table"
3. Enable "Enable Realtime" toggle

### Mistake #2: Forgetting to Cleanup Subscriptions
```typescript
// Wrong: Memory leak - subscriptions pile up
useEffect(() => {
  const channel = supabase.channel('test').subscribe();
  // Missing cleanup!
}, []);
```

```typescript
// Correct: Clean up on unmount
useEffect(() => {
  const channel = supabase.channel('test').subscribe();
  
  return () => {
    supabase.removeChannel(channel);
  };
}, []);
```

### Mistake #3: Not Handling Initial Load Separately
```typescript
// Wrong: Duplicating data - subscription adds it too
useEffect(() => {
  // This might cause duplicates
  const channel = supabase.channel('todos')
    .on('postgres_changes', { event: 'INSERT' }, (payload) => {
      setTodos(prev => [...prev, payload.new as Todo]);
    })
    .subscribe();
}, []);
```

```typescript
// Correct: Fetch initial data, then subscribe
useEffect(() => {
  // First fetch existing data
  const { data } = await supabase.from('todos').select('*');
  setTodos(data || []);
  
  // Then subscribe to changes
  const channel = supabase.channel('todos')
    .on('postgres_changes', { event: 'INSERT' }, (payload) => {
      // Only add new items, not duplicates
    })
    .subscribe();
}, []);
```

## Summary
- Real-time database sync updates UI automatically when data changes
- Supabase provides PostgreSQL Realtime out of the box
- Subscribe to INSERT, UPDATE, and DELETE events separately
- Always clean up subscriptions to prevent memory leaks
- Combine with Server Actions for mutations that need server-side logic
- Enable realtime on each table you want to watch in Supabase dashboard

## Next Steps
- [what-is-edge-runtime.md](../18-edge-runtime/01-edge-basics/what-is-edge-runtime.md) — Edge computing for low-latency features
