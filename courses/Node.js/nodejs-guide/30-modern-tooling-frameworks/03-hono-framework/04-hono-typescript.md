# Hono with TypeScript

## What You'll Learn

- How to get full type inference with Hono
- Defining typed route groups
- Using Zod for runtime validation with type safety
- Generating OpenAPI specs from Hono routes

## Typed App

```ts
// types.ts

export type Env = {
  Bindings: {
    DATABASE_URL: string;
    JWT_SECRET: string;
  };
  Variables: {
    user: { id: string; role: string };
  };
};
```

```ts
// app.ts

import { Hono } from 'hono';
import type { Env } from './types.js';

const app = new Hono<Env>();

// c.env.DATABASE_URL is typed as string
// c.get('user') is typed as { id: string; role: string }
app.get('/', (c) => {
  return c.json({ dbUrl: c.env.DATABASE_URL });
});
```

## Zod Validation

```ts
// schemas/user.ts

import { z } from 'zod';

export const createUserSchema = z.object({
  name: z.string().min(2).max(100),
  email: z.string().email(),
  age: z.number().int().min(13).max(120).optional(),
});

export type CreateUserInput = z.infer<typeof createUserSchema>;
```

```ts
// middleware/validate.ts

import { Context, Next } from 'hono';
import { ZodSchema } from 'zod';

export function validate(schema: ZodSchema) {
  return async (c: Context, next: Next) => {
    const body = await c.req.json();
    const result = schema.safeParse(body);

    if (!result.success) {
      return c.json({
        error: 'Validation failed',
        details: result.error.flatten().fieldErrors,
      }, 400);
    }

    c.set('validatedBody', result.data);
    await next();
  };
}
```

```ts
// routes/users.ts

import { Hono } from 'hono';
import { validate } from '../middleware/validate.js';
import { createUserSchema, type CreateUserInput } from '../schemas/user.js';

const users = new Hono();

users.post('/', validate(createUserSchema), async (c) => {
  // c.get('validatedBody') is typed as CreateUserInput
  const body = c.get('validatedBody') as CreateUserInput;

  return c.json({ id: 1, ...body }, 201);
});
```

## OpenAPI Generation

```ts
// With @hono/zod-openapi

import { OpenAPIHono, createRoute, z } from '@hono/zod-openapi';

const app = new OpenAPIHono();

const userSchema = z.object({
  id: z.number(),
  name: z.string(),
  email: z.string().email(),
});

const getUserRoute = createRoute({
  method: 'get',
  path: '/users/{id}',
  request: {
    params: z.object({ id: z.string() }),
  },
  responses: {
    200: {
      content: { 'application/json': { schema: userSchema } },
      description: 'User found',
    },
  },
});

app.openapi(getUserRoute, (c) => {
  const { id } = c.req.valid('param');
  return c.json({ id: Number(id), name: 'Alice', email: 'alice@example.com' });
});

// Serve OpenAPI spec
app.doc('/openapi.json', {
  openapi: '3.0.0',
  info: { title: 'Hono API', version: '1.0.0' },
});
```

## Next Steps

For tRPC, continue to [tRPC Setup](../05-trpc-rpc/01-trpc-setup.md).
