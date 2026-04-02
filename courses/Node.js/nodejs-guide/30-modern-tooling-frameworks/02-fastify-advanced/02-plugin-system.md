# Fastify Plugin System

## What You'll Learn

- How Fastify's encapsulated plugin system works
- How to create reusable plugins
- How plugin scopes prevent naming conflicts
- How to register plugins with options

## Plugin Basics

Fastify plugins are functions that receive the Fastify instance. They provide encapsulation — each plugin has its own scope.

```ts
// plugins/greeting.ts — A simple plugin

import { FastifyInstance } from 'fastify';

// Plugin function — receives the Fastify instance
export async function greetingPlugin(app: FastifyInstance) {
  // Add a decorator — extends the Fastify instance
  app.decorate('greet', (name: string) => `Hello, ${name}!`);

  // Add a route inside the plugin
  app.get('/greet/:name', async (request) => {
    const { name } = request.params as { name: string };
    return { message: app.greet(name) };
  });
}
```

```ts
// server.ts — Register the plugin

import Fastify from 'fastify';
import { greetingPlugin } from './plugins/greeting.js';

const app = Fastify({ logger: true });

// Register the plugin
app.register(greetingPlugin);

// The greet decorator is available after registration
app.listen({ port: 3000 });
```

## Plugin with Options

```ts
// plugins/auth.ts — Plugin with configuration

import { FastifyInstance, FastifyRequest, FastifyReply } from 'fastify';

interface AuthOptions {
  secret: string;
  exclude?: string[];
}

export async function authPlugin(app: FastifyInstance, options: AuthOptions) {
  const { secret, exclude = [] } = options;

  // Add a hook that runs before every request
  app.addHook('preHandler', async (request: FastifyRequest, reply: FastifyReply) => {
    // Skip auth for excluded routes
    if (exclude.includes(request.url)) return;

    const token = request.headers.authorization?.replace('Bearer ', '');

    if (!token) {
      reply.status(401).send({ error: 'Missing token' });
      return;
    }

    try {
      // Verify token (simplified)
      const user = verifyToken(token, secret);
      request.user = user;
    } catch {
      reply.status(401).send({ error: 'Invalid token' });
    }
  });
}
```

```ts
// server.ts — Register with options

app.register(authPlugin, {
  secret: process.env.JWT_SECRET!,
  exclude: ['/health', '/login'],
});
```

## Encapsulated Plugins

Plugins create isolated scopes — routes and decorators registered inside a plugin are only visible within that plugin unless explicitly shared:

```ts
// plugins/users.ts — Encapsulated routes

import { FastifyInstance } from 'fastify';

export async function usersPlugin(app: FastifyInstance) {
  // These routes are prefixed with /users
  app.get('/', async () => {
    return [{ id: 1, name: 'Alice' }];
  });

  app.get('/:id', async (request) => {
    const { id } = request.params as { id: string };
    return { id: Number(id), name: 'Alice' };
  });
}
```

```ts
// server.ts — Register with prefix
app.register(usersPlugin, { prefix: '/users' });

// Routes available at:
// GET /users/
// GET /users/:id
```

## Chaining Plugins

```ts
// server.ts — Plugin composition

import Fastify from 'fastify';
import cors from '@fastify/cors';
import helmet from '@fastify/helmet';
import jwt from '@fastify/jwt';

const app = Fastify({ logger: true });

// Plugins are registered in order — each can depend on previous ones
await app.register(cors, { origin: '*' });
await app.register(helmet);
await app.register(jwt, { secret: process.env.JWT_SECRET! });

// After jwt plugin is registered, app.jwt is available
app.post('/login', async (request, reply) => {
  const token = app.jwt.sign({ userId: '1' });
  return { token };
});
```

## Lifecycle Hooks

```ts
// Hooks run at specific points in the request lifecycle

app.addHook('onRequest', async (request, reply) => {
  // Runs when request is received
  request.startTime = Date.now();
});

app.addHook('preHandler', async (request, reply) => {
  // Runs before the route handler
  // Good place for auth, validation
});

app.addHook('onSend', async (request, reply, payload) => {
  // Runs before response is sent
  // Can modify the payload
  return payload;
});

app.addHook('onResponse', async (request, reply) => {
  // Runs after response is sent
  const duration = Date.now() - request.startTime;
  console.log(`${request.method} ${request.url} - ${reply.statusCode} (${duration}ms)`);
});
```

## Next Steps

For schema validation, continue to [Schema Validation](./03-schema-validation.md).
