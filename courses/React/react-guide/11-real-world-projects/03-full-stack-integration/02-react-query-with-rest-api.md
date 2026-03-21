# React Query with REST API

## Overview

TanStack Query (React Query) provides powerful data fetching with caching, background updates, and optimistic updates. This guide covers integrating React Query with REST APIs for CRUD operations.

## Prerequisites

- React Query installation
- REST API understanding

## Core Concepts

### QueryClient Setup

```tsx
// File: src/App.tsx

import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 5 * 60 * 1000, // 5 minutes
      retry: 2,
      refetchOnWindowFocus: false,
    },
  },
});

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <YourComponents />
      <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  );
}
```

### Fetching Data with useQuery

```tsx
// File: src/hooks/useUsers.ts

import { useQuery } from '@tanstack/react-query';
import api from '../services/api';

export function useUsers() {
  return useQuery({
    queryKey: ['users'],
    queryFn: async () => {
      const response = await api.get('/users');
      return response.data;
    },
  });
}

export function useUser(id: string) {
  return useQuery({
    queryKey: ['user', id],
    queryFn: async () => {
      const response = await api.get(`/users/${id}`);
      return response.data;
    },
    enabled: !!id, // Only run if id exists
  });
}
```

### Mutations with useMutation

```tsx
// File: src/hooks/useCreateUser.ts

import { useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';

export function useCreateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (newUser: { name: string; email: string }) => {
      const response = await api.post('/users', newUser);
      return response.data;
    },
    onSuccess: () => {
      // Invalidate and refetch users list
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
  });
}
```

### Optimistic Updates

```tsx
// File: src/hooks/useUpdateUser.ts

import { useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';

export function useUpdateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ id, ...data }: { id: string; name: string }) => {
      const response = await api.put(`/users/${id}`, data);
      return response.data;
    },
    onMutate: async (newUser) => {
      // Cancel outgoing queries
      await queryClient.cancelQueries({ queryKey: ['users'] });

      // Snapshot previous value
      const previousUsers = queryClient.getQueryData(['users']);

      // Optimistically update
      queryClient.setQueryData(['users'], (old: any[]) => 
        old?.map(user => 
          user.id === newUser.id ? { ...user, ...newUser } : user
        )
      );

      return { previousUsers };
    },
    onError: (err, newUser, context) => {
      // Rollback on error
      queryClient.setQueryData(['users'], context?.previousUsers);
    },
    onSettled: () => {
      // Refetch after error or success
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
  });
}
```

## Key Takeaways

- useQuery for data fetching
- useMutation for data modifications
- Use onSuccess to invalidate queries
- Implement optimistic updates for better UX

## What's Next

Continue to [Deploying React to Vercel](/11-real-world-projects/03-full-stack-integration/03-deploying-react-to-vercel.md) to learn about deployment.