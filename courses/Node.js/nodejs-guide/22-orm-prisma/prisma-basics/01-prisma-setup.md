# Prisma Setup

## What You'll Learn

- What Prisma is and how it compares to raw SQL
- How to initialize a Prisma project
- How to configure the datasource and generator
- How the Prisma schema works
- How to create and use the Prisma client

## What Is Prisma?

Prisma is an **ORM** (Object-Relational Mapper) — it maps database tables to JavaScript objects. Instead of writing raw SQL, you use Prisma's type-safe API:

```js
// Raw SQL
const users = await db.query('SELECT * FROM users WHERE email = $1', [email]);

// Prisma
const user = await prisma.user.findUnique({ where: { email } });
```

Prisma provides:
- **Schema-first** data modeling
- **Auto-generated** client with TypeScript types
- **Migrations** to evolve the database schema
- **Query builder** with relations, filtering, and pagination

## Project Setup

```bash
mkdir prisma-demo && cd prisma-demo
npm init -y
npm install prisma @prisma/client
```

### Initialize Prisma

```bash
npx prisma init
```

This creates:
- `prisma/schema.prisma` — the data model and configuration
- `.env` — environment variables (DATABASE_URL)

## Configure the Schema

```prisma
// prisma/schema.prisma

// Generator — defines which Prisma client to generate
generator client {
  provider = "prisma-client-js"
}

// Datasource — which database to connect to
datasource db {
  provider = "sqlite"          // sqlite, postgresql, mysql
  url      = env("DATABASE_URL")  // Loaded from .env file
}
```

```env
# .env
DATABASE_URL="file:./dev.db"
```

## Define Models

```prisma
// prisma/schema.prisma

generator client {
  provider = "prisma-client-js"
}

datasource db {
  provider = "sqlite"
  url      = env("DATABASE_URL")
}

// User model — maps to a "users" table
model User {
  id        Int      @id @default(autoincrement())
  email     String   @unique
  name      String
  createdAt DateTime @default(now())
  updatedAt DateTime @updatedAt

  // One-to-many: a user has many posts
  posts Post[]
}

// Post model — maps to a "posts" table
model Post {
  id        Int      @id @default(autoincrement())
  title     String
  content   String?
  published Boolean  @default(false)
  createdAt DateTime @default(now())

  // Foreign key to User
  authorId Int
  author   User   @relation(fields: [authorId], references: [id])
}
```

## Run the First Migration

```bash
npx prisma migrate dev --name init
```

This:
1. Creates a migration file in `prisma/migrations/`
2. Applies the migration to the database (creates tables)
3. Generates the Prisma Client

## Use Prisma Client

```js
// index.js — Using Prisma Client

import { PrismaClient } from '@prisma/client';

// Create a single PrismaClient instance
// It manages the database connection pool
const prisma = new PrismaClient();

async function main() {
  // Create a user
  const user = await prisma.user.create({
    data: {
      email: 'alice@example.com',
      name: 'Alice',
    },
  });
  console.log('Created user:', user);

  // Create a post for the user
  const post = await prisma.post.create({
    data: {
      title: 'Hello Prisma',
      content: 'Prisma is great!',
      published: true,
      authorId: user.id,  // Link to the user
    },
  });
  console.log('Created post:', post);

  // Query users with their posts (relation)
  const users = await prisma.user.findMany({
    include: { posts: true },  // Include related posts
  });
  console.log('Users with posts:', JSON.stringify(users, null, 2));
}

main()
  .catch(console.error)
  .finally(() => prisma.$disconnect());  // Always disconnect
```

## How It Works

### The Prisma Workflow

```
1. Define models in schema.prisma
2. Run prisma migrate dev → generates SQL migration + applies it
3. Run prisma generate → generates the Prisma Client
4. Use Prisma Client in your code
```

### The Prisma Client

The client is auto-generated from your schema. It knows your models, fields, and relations. If you change the schema and regenerate, the client updates.

```js
// prisma.user has these methods (auto-generated):
prisma.user.create({ data: { ... } })
prisma.user.findUnique({ where: { ... } })
prisma.user.findMany({ where: { ... } })
prisma.user.update({ where: { ... }, data: { ... } })
prisma.user.delete({ where: { ... } })
```

### Datasource Providers

| Provider | Connection String |
|----------|------------------|
| SQLite | `file:./dev.db` |
| PostgreSQL | `postgresql://user:pass@host:5432/db` |
| MySQL | `mysql://user:pass@host:3306/db` |

## Common Mistakes

### Mistake 1: Multiple PrismaClient Instances

```js
// WRONG — creating a new client on every import (exhausts connection pool)
function getClient() {
  return new PrismaClient();  // Each instance opens new connections
}

// CORRECT — singleton pattern
// lib/prisma.js
import { PrismaClient } from '@prisma/client';
const prisma = new PrismaClient();
export default prisma;
```

### Mistake 2: Not Running Migrations

```js
// WRONG — changed schema.prisma but did not run migrate
// The database tables are out of sync with the schema
const user = await prisma.user.create({ data: { name: 'Alice' } });
// Throws: "column 'name' does not exist"

// CORRECT — always run migrate after schema changes
// npx prisma migrate dev --name add-name-field
```

### Mistake 3: Forgetting $disconnect()

```js
// WRONG — connections leak, eventually the pool is exhausted
const prisma = new PrismaClient();
await prisma.user.findMany();
// Process hangs because connections are still open

// CORRECT — disconnect when done (or use SIGINT handler)
await prisma.$disconnect();
// Or in Express:
process.on('SIGINT', async () => {
  await prisma.$disconnect();
  process.exit(0);
});
```

## Try It Yourself

### Exercise 1: Create and Query

Create a User with two Posts. Query the user and include their posts. Print the result.

### Exercise 2: Update and Delete

Update the user's name. Delete one of their posts. Query again to verify.

### Exercise 3: Browse the Database

Run `npx prisma studio` to open the visual database browser. Add, edit, and delete records through the UI.

## Next Steps

You can define models and query data. For relations and advanced schema design, continue to [Models & Relations](./02-models-relations.md).
