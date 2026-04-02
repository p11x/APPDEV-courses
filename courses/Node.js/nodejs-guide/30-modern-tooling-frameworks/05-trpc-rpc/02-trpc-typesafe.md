# tRPC Type-Safe Client

## What You'll Learn

- How to create a tRPC client
- How types flow from server to client automatically
- How to use tRPC with React
- How to handle errors on the client

## Client Setup

```ts
// client/trpc.ts

import { createTRPCClient, httpBatchLink } from '@trpc/client';
import type { AppRouter } from '../server/app.js';

// Create the client — types are inferred from AppRouter
export const trpc = createTRPCClient<AppRouter>({
  links: [
    httpBatchLink({
      url: 'http://localhost:3000/trpc',
      headers() {
        const token = localStorage.getItem('token');
        return token ? { authorization: `Bearer ${token}` } : {};
      },
    }),
  ],
});

// Usage — full type safety!
const users = await trpc.user.list.query();
// users is typed as { id: string; name: string; email: string }[]

const user = await trpc.user.getById.query({ id: '1' });
// user is typed as { id: string; name: string; email: string }

const newUser = await trpc.user.create.mutate({ name: 'Charlie', email: 'charlie@example.com' });
// newUser is typed as { id: string; name: string; email: string }
```

## React Integration

```tsx
// providers/TrpcProvider.tsx

import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { createTRPCReact } from '@trpc/react-query';
import { httpBatchLink } from '@trpc/client';
import { useState } from 'react';
import type { AppRouter } from '../../server/app.js';

export const trpc = createTRPCReact<AppRouter>();

export function TrpcProvider({ children }: { children: React.ReactNode }) {
  const [queryClient] = useState(() => new QueryClient());
  const [trpcClient] = useState(() =>
    trpc.createClient({
      links: [
        httpBatchLink({
          url: 'http://localhost:3000/trpc',
        }),
      ],
    })
  );

  return (
    <trpc.Provider client={trpcClient} queryClient={queryClient}>
      <QueryClientProvider client={queryClient}>
        {children}
      </QueryClientProvider>
    </trpc.Provider>
  );
}
```

```tsx
// components/UserList.tsx

import { trpc } from '../providers/TrpcProvider.js';

export function UserList() {
  // Auto-generated hook — types come from the server
  const { data: users, isLoading, error } = trpc.user.list.useQuery();

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  return (
    <ul>
      {users?.map((user) => (
        <li key={user.id}>{user.name} ({user.email})</li>
      ))}
    </ul>
  );
}

export function CreateUser() {
  const utils = trpc.useUtils();
  const mutation = trpc.user.create.useMutation({
    onSuccess: () => {
      utils.user.list.invalidate();  // Refetch the user list
    },
  });

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const form = new FormData(e.currentTarget);
    mutation.mutate({
      name: form.get('name') as string,
      email: form.get('email') as string,
    });
  };

  return (
    <form onSubmit={handleSubmit}>
      <input name="name" placeholder="Name" />
      <input name="email" placeholder="Email" />
      <button type="submit" disabled={mutation.isPending}>
        {mutation.isPending ? 'Creating...' : 'Create'}
      </button>
    </form>
  );
}
```

## Error Handling

```tsx
function UserPage({ id }: { id: string }) {
  const { data: user, error } = trpc.user.getById.useQuery({ id });

  if (error) {
    // error is typed as TRPCClientError<AppRouter>
    if (error.data?.code === 'NOT_FOUND') {
      return <div>User not found</div>;
    }
    if (error.data?.code === 'UNAUTHORIZED') {
      return <div>Please log in</div>;
    }
    return <div>Error: {error.message}</div>;
  }

  return <div>{user?.name}</div>;
}
```

## Next Steps

For query optimization, continue to [tRPC Query Optimization](./03-trpc-query-optimization.md).
