# tRPC Setup

## What You'll Learn

- What tRPC is and why it exists
- How to set up a tRPC server with Express/Fastify
- How to create type-safe procedures
- How tRPC eliminates the need for REST documentation

## What Is tRPC?

tRPC (TypeScript Remote Procedure Call) lets you call backend functions from the frontend **with full type safety** — no code generation, no schema files, no REST endpoints. Types flow automatically from server to client.

```
REST: Frontend → HTTP → Server → Parse response → Hope types match
tRPC: Frontend → tRPC → Server → Types guaranteed to match
```

## Setup

```bash
mkdir trpc-app && cd trpc-app
bun init -y
bun add @trpc/server @trpc/client @trpc/react-query @tanstack/react-query zod
```

## Server Setup

```ts
// server/trpc.ts — Create tRPC instance

import { initTRPC, TRPCError } from '@trpc/server';
import { z } from 'zod';

// Context — passed to every procedure
export type Context = {
  userId?: string;
};

// Initialize tRPC with context type
const t = initTRPC.context<Context>().create();

// Export router and procedure helpers
export const router = t.router;
export const publicProcedure = t.procedure;

// Protected procedure — requires authentication
export const protectedProcedure = t.procedure.use(({ ctx, next }) => {
  if (!ctx.userId) {
    throw new TRPCError({ code: 'UNAUTHORIZED' });
  }
  return next({ ctx: { userId: ctx.userId } });
});
```

```ts
// server/routers/user.ts

import { router, publicProcedure, protectedProcedure } from '../trpc.js';
import { z } from 'zod';

export const userRouter = router({
  // List users
  list: publicProcedure.query(async () => {
    return [
      { id: '1', name: 'Alice', email: 'alice@example.com' },
      { id: '2', name: 'Bob', email: 'bob@example.com' },
    ];
  }),

  // Get user by ID
  getById: publicProcedure
    .input(z.object({ id: z.string() }))
    .query(async ({ input }) => {
      return { id: input.id, name: 'Alice', email: 'alice@example.com' };
    }),

  // Create user (protected)
  create: protectedProcedure
    .input(z.object({
      name: z.string().min(2),
      email: z.string().email(),
    }))
    .mutation(async ({ input }) => {
      return { id: '3', ...input };
    }),

  // Update user (protected)
  update: protectedProcedure
    .input(z.object({
      id: z.string(),
      name: z.string().min(2).optional(),
      email: z.string().email().optional(),
    }))
    .mutation(async ({ input }) => {
      return input;
    }),
});
```

```ts
// server/app.ts — Create the app router

import { router } from './trpc.js';
import { userRouter } from './routers/user.js';

export const appRouter = router({
  user: userRouter,
});

// Export the router type — the client needs this for type inference
export type AppRouter = typeof appRouter;
```

## HTTP Server Integration

```ts
// server/index.ts — Express integration

import express from 'express';
import { createExpressMiddleware } from '@trpc/server/adapters/express';
import { appRouter } from './app.js';

const app = express();

app.use('/trpc', createExpressMiddleware({
  router: appRouter,
  createContext: ({ req }) => {
    // Extract user from JWT token
    const token = req.headers.authorization?.replace('Bearer ', '');
    const userId = token ? verifyToken(token) : undefined;
    return { userId };
  },
}));

app.listen(3000, () => {
  console.log('tRPC server on http://localhost:3000/trpc');
});
```

## Next Steps

For type-safe client, continue to [tRPC Type-Safe Client](./02-trpc-typesafe.md).
