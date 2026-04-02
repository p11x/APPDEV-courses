# tRPC Query Optimization

## What You'll Learn

- How to optimize tRPC queries with React Query
- How to implement infinite scrolling
- How to prefetch and cache data
- How to use tRPC with server components

## Prefetching

```tsx
// Prefetch data before the component mounts

import { trpc } from '../providers/TrpcProvider.js';

export function UserPage({ id }: { id: string }) {
  // Prefetch the user data
  trpc.user.getById.usePrefetchQuery({ id });

  // The component renders with pre-fetched data (no loading state)
  return <UserDetails id={id} />;
}
```

## Infinite Scrolling

```tsx
import { trpc } from '../providers/TrpcProvider.js';

export function InfiniteUserList() {
  const {
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
  } = trpc.user.infinite.useInfiniteQuery(
    { limit: 20 },
    {
      getNextPageParam: (lastPage) => lastPage.nextCursor,
    }
  );

  const users = data?.pages.flatMap((page) => page.items) ?? [];

  return (
    <div>
      {users.map((user) => (
        <div key={user.id}>{user.name}</div>
      ))}

      {hasNextPage && (
        <button onClick={() => fetchNextPage()} disabled={isFetchingNextPage}>
          {isFetchingNextPage ? 'Loading...' : 'Load More'}
        </button>
      )}
    </div>
  );
}
```

```ts
// server/routers/user.ts — Infinite query procedure

export const userRouter = router({
  infinite: publicProcedure
    .input(z.object({
      cursor: z.string().optional(),
      limit: z.number().min(1).max(100).default(20),
    }))
    .query(async ({ input }) => {
      const users = await db.user.findMany({
        take: input.limit + 1,  // Fetch one extra to check if there are more
        ...(input.cursor && { cursor: { id: input.cursor }, skip: 1 }),
        orderBy: { createdAt: 'desc' },
      });

      const hasMore = users.length > input.limit;
      const items = hasMore ? users.slice(0, -1) : users;

      return {
        items,
        nextCursor: hasMore ? items[items.length - 1].id : null,
      };
    }),
});
```

## Optimistic Updates

```tsx
export function CreateUser() {
  const utils = trpc.useUtils();

  const mutation = trpc.user.create.useMutation({
    // Optimistic update — show the new user immediately
    onMutate: async (newUser) => {
      await utils.user.list.cancel();

      const previous = utils.user.list.getData();

      // Optimistically add the user
      utils.user.list.setData(undefined, (old) => [
        ...(old ?? []),
        { id: 'temp', ...newUser },
      ]);

      return { previous };
    },

    // Rollback on error
    onError: (_err, _newUser, context) => {
      utils.user.list.setData(undefined, context?.previous);
    },

    // Refetch on success
    onSettled: () => {
      utils.user.list.invalidate();
    },
  });
}
```

## Next Steps

For comparison with GraphQL, continue to [tRPC vs GraphQL](./04-trpc-vs-graphql.md).
