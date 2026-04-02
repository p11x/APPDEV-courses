# Fastify Setup

## What You'll Learn

- What Fastify is and how it compares to Express
- How to create a Fastify server with TypeScript
- How Fastify's plugin system works
- How schema validation improves performance

## What Is Fastify?

Fastify is a web framework focused on **performance** and **developer experience**. It uses a schema-based approach to validate and serialize requests/responses, achieving 2x the throughput of Express.

| Feature | Express | Fastify |
|---------|---------|---------|
| Requests/sec | ~15,000 | ~35,000 |
| Validation | Manual (middleware) | Built-in (schema) |
| TypeScript | @types/express | Built-in types |
| Plugin system | Middleware | Encapsulated plugins |
| Logging | Manual | Built-in (Pino) |

## Setup

```bash
mkdir fastify-app && cd fastify-app
bun init -y
bun add fastify @fastify/cors @fastify/helmet
```

## Basic Server

```ts
// server.ts — Fastify server

import Fastify from 'fastify';

const app = Fastify({
  logger: true,  // Built-in Pino logger
});

// Health check
app.get('/health', async () => {
  return { status: 'ok', uptime: process.uptime() };
});

// Simple route
app.get('/users', async (request, reply) => {
  return [
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' },
  ];
});

// Route with parameters
app.get('/users/:id', async (request, reply) => {
  const { id } = request.params as { id: string };

  return {
    id: Number(id),
    name: 'Alice',
  };
});

// POST route
app.post('/users', async (request, reply) => {
  const body = request.body as { name: string; email: string };

  // In production, save to database
  reply.status(201);
  return { id: 3, ...body };
});

// Start server
const start = async () => {
  try {
    await app.listen({ port: 3000, host: '0.0.0.0' });
    console.log('Server running on http://localhost:3000');
  } catch (err) {
    app.log.error(err);
    process.exit(1);
  }
};

start();
```

```bash
# Run with Bun
bun run server.ts

# Run with Node.js
npx tsx server.ts
```

## Typed Routes

```ts
// typed-routes.ts — Full type safety

import Fastify from 'fastify';

const app = Fastify({ logger: true });

// Define types for request/response
interface CreateUserBody {
  name: string;
  email: string;
}

interface UserParams {
  id: string;
}

interface UserResponse {
  id: number;
  name: string;
  email: string;
}

// Typed route
app.post<{ Body: CreateUserBody; Reply: UserResponse }>(
  '/users',
  async (request, reply) => {
    const { name, email } = request.body;  // Fully typed!

    return {
      id: 1,
      name,
      email,
    };
  }
);

// Typed params
app.get<{ Params: UserParams }>(
  '/users/:id',
  async (request) => {
    const { id } = request.params;  // Fully typed as string
    return { id: Number(id), name: 'Alice', email: 'alice@example.com' };
  }
);
```

## Performance Comparison

| Benchmark | Express | Fastify | Difference |
|-----------|---------|---------|-----------|
| JSON response | 15,000 req/s | 35,000 req/s | 2.3x |
| Latency p99 | 12ms | 5ms | 2.4x |
| Memory (idle) | 45MB | 35MB | 22% less |
| Startup time | 80ms | 45ms | 44% less |

## Common Mistakes

### Mistake 1: Not Using reply.send()

```ts
// WRONG — returning from async handler works, but not for all cases
app.get('/users', async () => {
  return { users: [] };  // Works, but Fastify wraps it
});

// CORRECT — use reply.send() for explicit control
app.get('/users', async (request, reply) => {
  reply.send({ users: [] });
});
```

### Mistake 2: Adding Middleware Like Express

```ts
// WRONG — Fastify plugins are different from Express middleware
app.use(cors());  // This does not work

// CORRECT — use Fastify plugins
import cors from '@fastify/cors';
app.register(cors);
```

## Next Steps

For the plugin system, continue to [Plugin System](./02-plugin-system.md).
