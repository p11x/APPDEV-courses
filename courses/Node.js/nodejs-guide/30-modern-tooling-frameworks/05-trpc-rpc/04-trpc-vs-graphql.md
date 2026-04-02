# tRPC vs GraphQL

## What You'll Learn

- How tRPC compares to GraphQL
- When to use each
- How to migrate from GraphQL to tRPC
- How to combine both in one project

## Comparison

| Feature | tRPC | GraphQL |
|---------|------|---------|
| Type safety | Automatic | Requires codegen |
| Setup complexity | Low | Medium-High |
| Learning curve | Low (if you know TypeScript) | Medium (SDL, resolvers) |
| Schema file | None (inferred from TS) | .graphql file required |
| Query flexibility | Fixed procedures | Client chooses fields |
| Real-time | Subscriptions | Subscriptions |
| Ecosystem | Smaller | Massive |
| Best for | TypeScript-only teams | Public APIs, multi-language |
| Bundle size | ~5KB | ~30KB+ |

## When to Use tRPC

- **Internal APIs** — frontend and backend are both TypeScript
- **TypeScript-only teams** — everyone writes TypeScript
- **Rapid development** — no schema to maintain
- **Monorepos** — types shared between client and server

## When to Use GraphQL

- **Public APIs** — clients in multiple languages (Python, Swift, etc.)
- **Complex queries** — clients need flexible field selection
- **Existing GraphQL** — already have a GraphQL server
- **Subscriptions** — real-time data with fine-grained subscriptions

## Side-by-Side

### REST

```ts
// Server
app.get('/api/users/:id', (req, res) => {
  res.json({ id: req.params.id, name: 'Alice' });
});

// Client
const res = await fetch('/api/users/1');
const user = await res.json();  // Type: any
```

### GraphQL

```graphql
# Schema
type User { id: ID!, name: String! }
type Query { user(id: ID!): User! }

# Client
const { data } = useQuery(gql`
  query GetUser($id: ID!) { user(id: $id) { id name } }
`, { variables: { id: '1' } });
```

### tRPC

```ts
// Server
getUser: publicProcedure
  .input(z.object({ id: z.string() }))
  .query(({ input }) => ({ id: input.id, name: 'Alice' }));

// Client
const user = trpc.user.getUser.useQuery({ id: '1' });
// user.data is typed as { id: string; name: string }
```

## Migration Path

```ts
// If you have GraphQL and want to try tRPC:
// 1. Create tRPC procedures that wrap existing resolvers
// 2. Migrate client code one route at a time
// 3. Keep GraphQL for public APIs, use tRPC for internal

export const userRouter = router({
  getById: publicProcedure
    .input(z.object({ id: z.string() }))
    .query(({ input }) => {
      // Reuse existing GraphQL resolver
      return resolvers.Query.user(null, { id: input.id });
    }),
});
```

## Hybrid Approach

```ts
// Use both in one project
// GraphQL for public API
// tRPC for internal admin dashboard

// app.ts
app.use('/graphql', graphqlMiddleware);  // Public API
app.use('/trpc', trpcMiddleware);         // Internal dashboard
```

## Next Steps

For Fresh framework, continue to [Fresh Setup](../06-fresh-framework/01-fresh-setup.md).
