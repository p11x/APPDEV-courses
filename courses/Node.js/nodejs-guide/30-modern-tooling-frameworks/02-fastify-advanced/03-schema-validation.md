# Fastify Schema Validation

## What You'll Learn

- How to use JSON Schema for request/response validation
- How schema validation improves performance
- How to define schemas for different HTTP parts
- How to reuse schemas across routes

## Why Schemas?

Fastify uses **JSON Schema** to validate incoming requests and serialize outgoing responses. This provides:

1. **Automatic validation** — invalid requests are rejected before reaching your handler
2. **Faster serialization** — Fastify pre-compiles serializers using schemas
3. **Auto-generated documentation** — schemas can generate OpenAPI specs
4. **Type safety** — schemas generate TypeScript types

## Schema Definition

```ts
// schemas/user.ts

// Schema for creating a user
export const createUserSchema = {
  body: {
    type: 'object',
    required: ['name', 'email'],
    properties: {
      name: { type: 'string', minLength: 2, maxLength: 100 },
      email: { type: 'string', format: 'email' },
      age: { type: 'integer', minimum: 13, maximum: 120 },
    },
    additionalProperties: false,
  },

  response: {
    201: {
      type: 'object',
      properties: {
        id: { type: 'integer' },
        name: { type: 'string' },
        email: { type: 'string' },
        createdAt: { type: 'string', format: 'date-time' },
      },
    },
  },
};

// Schema for getting a user
export const getUserSchema = {
  params: {
    type: 'object',
    required: ['id'],
    properties: {
      id: { type: 'string', pattern: '^[0-9]+$' },  // Numeric string
    },
  },

  response: {
    200: {
      type: 'object',
      properties: {
        id: { type: 'integer' },
        name: { type: 'string' },
        email: { type: 'string' },
      },
    },
  },
};

// Schema for query parameters
export const listUsersSchema = {
  querystring: {
    type: 'object',
    properties: {
      page: { type: 'integer', minimum: 1, default: 1 },
      limit: { type: 'integer', minimum: 1, maximum: 100, default: 20 },
      sort: { type: 'string', enum: ['name', 'email', 'createdAt'] },
    },
  },
};
```

## Using Schemas in Routes

```ts
// routes/users.ts

import { FastifyInstance } from 'fastify';
import { createUserSchema, getUserSchema, listUsersSchema } from '../schemas/user.js';

export async function userRoutes(app: FastifyInstance) {
  app.post('/', { schema: createUserSchema }, async (request, reply) => {
    const { name, email } = request.body as { name: string; email: string };

    // request.body is validated against the schema
    // If validation fails, Fastify returns 400 automatically

    reply.status(201);
    return {
      id: 1,
      name,
      email,
      createdAt: new Date().toISOString(),
    };
  });

  app.get('/:id', { schema: getUserSchema }, async (request) => {
    const { id } = request.params as { id: string };

    return {
      id: Number(id),
      name: 'Alice',
      email: 'alice@example.com',
    };
  });

  app.get('/', { schema: listUsersSchema }, async (request) => {
    const { page, limit, sort } = request.query as {
      page: number;
      limit: number;
      sort: string;
    };

    return {
      page,
      limit,
      sort,
      data: [{ id: 1, name: 'Alice' }],
    };
  });
}
```

## Schema Composition

```ts
// schemas/shared.ts — Reusable schema parts

export const paginationSchema = {
  type: 'object',
  properties: {
    page: { type: 'integer', minimum: 1, default: 1 },
    limit: { type: 'integer', minimum: 1, maximum: 100, default: 20 },
  },
};

export const timestampSchema = {
  createdAt: { type: 'string', format: 'date-time' },
  updatedAt: { type: 'string', format: 'date-time' },
};

export const errorSchema = {
  type: 'object',
  properties: {
    error: { type: 'string' },
    message: { type: 'string' },
    statusCode: { type: 'integer' },
  },
};

// Use $ref to reuse schemas
export const userListSchema = {
  querystring: paginationSchema,
  response: {
    200: {
      type: 'object',
      properties: {
        data: {
          type: 'array',
          items: {
            type: 'object',
            properties: {
              id: { type: 'integer' },
              name: { type: 'string' },
              ...timestampSchema,
            },
          },
        },
        total: { type: 'integer' },
        page: { type: 'integer' },
      },
    },
  },
};
```

## Custom Validators

```ts
import Fastify from 'fastify';
import Ajv from 'ajv';
import addFormats from 'ajv-formats';

const app = Fastify({
  ajv: {
    customOptions: {
      removeAdditional: 'all',     // Remove unknown properties
      coerceTypes: true,           // Convert "1" to 1
      allErrors: true,             // Return all errors, not just the first
    },
  },
});

// Add custom formats
const ajv = new Ajv();
addFormats(ajv);

// Custom format: phone number
ajv.addFormat('phone', /^\+?[1-9]\d{1,14}$/);
```

## Performance Impact

| Operation | Without Schema | With Schema | Improvement |
|-----------|---------------|-------------|-------------|
| JSON serialization | 12ms | 4ms | 3x faster |
| Request validation | 2ms (manual) | 0.5ms | 4x faster |
| Response validation | None | 0.3ms | Safety net |

## Next Steps

For decorators, continue to [Fastify Decorators](./04-fastify-decorators.md).
