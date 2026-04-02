# Fastify Decorators

## What You'll Learn

- How to extend the Fastify instance with custom properties
- How to create typed decorators
- How decorators interact with plugins and hooks
- Practical decorator patterns

## What Are Decorators?

Decorators add custom properties or methods to the Fastify instance. They are attached during plugin registration and available in all routes within that scope.

```ts
// decorators/database.ts — Database decorator

import { FastifyInstance } from 'fastify';
import fp from 'fastify-plugin';  // npm install fastify-plugin

// Without fp(), decorators are scoped to the plugin
// With fp(), decorators are available globally
async function databasePlugin(app: FastifyInstance) {
  // Create a database connection
  const db = {
    users: [
      { id: 1, name: 'Alice' },
      { id: 2, name: 'Bob' },
    ],

    async findUser(id: number) {
      return this.users.find((u) => u.id === id);
    },

    async createUser(data: { name: string }) {
      const user = { id: this.users.length + 1, ...data };
      this.users.push(user);
      return user;
    },
  };

  // Decorate the Fastify instance with 'db'
  // Now app.db is available in all routes
  app.decorate('db', db);
}

// Wrap with fp() to make decorators available globally
export default fp(databasePlugin, { name: 'database' });
```

```ts
// routes/users.ts — Using the decorator

import { FastifyInstance } from 'fastify';

export async function userRoutes(app: FastifyInstance) {
  app.get('/users/:id', async (request) => {
    const { id } = request.params as { id: string };

    // app.db is available because of the decorator
    const user = await app.db.findUser(Number(id));

    if (!user) {
      throw new Error('User not found');
    }

    return user;
  });

  app.post('/users', async (request, reply) => {
    const body = request.body as { name: string };
    const user = await app.db.createUser(body);
    reply.status(201);
    return user;
  });
}
```

```ts
// server.ts — Register plugins

import Fastify from 'fastify';
import databasePlugin from './decorators/database.js';
import { userRoutes } from './routes/users.js';

const app = Fastify({ logger: true });

// Database decorator is registered first
await app.register(databasePlugin);

// Routes can now use app.db
await app.register(userRoutes);

app.listen({ port: 3000 });
```

## Typed Decorators

```ts
// types/fastify.d.ts — Extend Fastify types

import { FastifyInstance } from 'fastify';

// Define the shape of your decorator
interface Database {
  findUser(id: number): Promise<{ id: number; name: string }>;
  createUser(data: { name: string }): Promise<{ id: number; name: string }>;
}

// Extend the FastifyInstance type
declare module 'fastify' {
  interface FastifyInstance {
    db: Database;
  }
}
```

Now `app.db` is fully typed in all routes.

## Common Decorator Patterns

### Logger Decorator

```ts
import fp from 'fastify-plugin';

async function loggerPlugin(app: FastifyInstance) {
  app.decorate('log2', (msg: string) => {
    console.log(`[${new Date().toISOString()}] ${msg}`);
  });
}

export default fp(loggerPlugin);
```

### Config Decorator

```ts
import fp from 'fastify-plugin';

interface Config {
  port: number;
  dbUrl: string;
  jwtSecret: string;
}

async function configPlugin(app: FastifyInstance) {
  const config: Config = {
    port: Number(process.env.PORT) || 3000,
    dbUrl: process.env.DATABASE_URL!,
    jwtSecret: process.env.JWT_SECRET!,
  };

  app.decorate('config', config);
}

export default fp(configPlugin);
```

### Service Decorator

```ts
import fp from 'fastify-plugin';

async function servicesPlugin(app: FastifyInstance) {
  app.decorate('services', {
    email: { send: async (to: string, subject: string) => { /* ... */ } },
    cache: {
      get: async (key: string) => { /* ... */ },
      set: async (key: string, value: unknown, ttl: number) => { /* ... */ },
    },
  });
}

export default fp(servicesPlugin);
```

## Decorator Lifecycle

```
1. Plugin registers → app.decorate('name', value)
2. Plugin completes → 'name' available on app
3. Route handler → app.name is fully typed and available
4. All routes in scope can access the decorator
```

## Next Steps

For Hono framework, continue to [Hono Basics](../03-hono-framework/01-hono-basics.md).
