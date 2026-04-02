# CRUD Queries

## What You'll Learn

- How to create, read, update, and delete records with Prisma
- How to use `findUnique`, `findFirst`, and `findMany`
- How to use `upsert` (create or update)
- How to use `deleteMany` for bulk deletion
- How to handle errors with Prisma's error types

## Setup

```js
// lib/prisma.js — Singleton Prisma client

import { PrismaClient } from '@prisma/client';

// Global variable prevents multiple instances in development
const globalForPrisma = globalThis;

const prisma = globalForPrisma.prisma || new PrismaClient({
  log: ['query'],  // Log all SQL queries (useful for learning)
});

if (process.env.NODE_ENV !== 'production') {
  globalForPrisma.prisma = prisma;
}

export default prisma;
```

## Create

```js
import prisma from './lib/prisma.js';

// Create a single record
const user = await prisma.user.create({
  data: {
    email: 'alice@example.com',
    name: 'Alice',
    role: 'USER',
  },
  select: {  // Return only these fields
    id: true,
    name: true,
    email: true,
  },
});

// Create with nested relations
const userWithPost = await prisma.user.create({
  data: {
    email: 'bob@example.com',
    name: 'Bob',
    posts: {
      create: [  // Create related posts at the same time
        { title: 'First Post', published: true },
        { title: 'Draft Post' },
      ],
    },
  },
  include: { posts: true },  // Return the posts too
});

// Create many records at once
const count = await prisma.user.createMany({
  data: [
    { email: 'charlie@example.com', name: 'Charlie' },
    { email: 'diana@example.com', name: 'Diana' },
  ],
  skipDuplicates: true,  // Skip if email already exists
});
console.log(`Created ${count.count} users`);
```

## Read

```js
// Find a unique record (by unique field)
const user = await prisma.user.findUnique({
  where: { email: 'alice@example.com' },
});

// Find by composite unique
const member = await prisma.projectMember.findUnique({
  where: {
    userId_projectId: { userId: 1, projectId: 1 },  // Composite key
  },
});

// Find the first matching record
const firstPost = await prisma.post.findFirst({
  where: { published: true },
  orderBy: { createdAt: 'desc' },  // Latest published post
});

// Find many with filtering
const users = await prisma.user.findMany({
  where: {
    role: 'USER',
    name: { contains: 'Ali', mode: 'insensitive' },
  },
  orderBy: { createdAt: 'desc' },
  take: 10,   // Limit
  skip: 0,    // Offset
  select: {
    id: true,
    name: true,
    _count: {  // Count related records
      select: { posts: true },
    },
  },
});

// Count records
const totalUsers = await prisma.user.count();
const publishedPosts = await prisma.post.count({
  where: { published: true },
});
```

## Update

```js
// Update a single record
const updated = await prisma.user.update({
  where: { id: 1 },
  data: {
    name: 'Alice Updated',
    bio: 'Senior developer',
  },
});

// Update or create (upsert)
const user = await prisma.user.upsert({
  where: { email: 'alice@example.com' },
  update: { name: 'Alice Updated' },  // If exists, update
  create: { email: 'alice@example.com', name: 'Alice' },  // If not, create
});

// Update many records
const { count } = await prisma.post.updateMany({
  where: { authorId: 1, published: false },
  data: { published: true },
});
console.log(`Published ${count} posts`);

// Increment a field
const post = await prisma.post.update({
  where: { id: 1 },
  data: { viewCount: { increment: 1 } },
});
```

## Delete

```js
// Delete a single record
const deleted = await prisma.user.delete({
  where: { id: 1 },
});

// Delete many records
const { count } = await prisma.post.deleteMany({
  where: { published: false },
});
console.log(`Deleted ${count} unpublished posts`);

// Delete with cascade (if configured in schema)
await prisma.user.delete({
  where: { id: 1 },
  // Related posts are deleted automatically if onDelete: Cascade
});
```

## Error Handling

```js
import { Prisma } from '@prisma/client';

try {
  await prisma.user.create({
    data: { email: 'alice@example.com', name: 'Alice' },
  });
} catch (err) {
  if (err instanceof Prisma.PrismaClientKnownRequestError) {
    // P2002 = unique constraint violation
    if (err.code === 'P2002') {
      console.error('Email already exists:', err.meta.target);
    }
    // P2025 = record not found
    if (err.code === 'P2025') {
      console.error('Record not found');
    }
  } else if (err instanceof Prisma.PrismaClientValidationError) {
    // Invalid query (e.g., wrong field type)
    console.error('Validation error:', err.message);
  } else {
    throw err;  // Unknown error
  }
}
```

### Common Error Codes

| Code | Meaning |
|------|---------|
| `P2002` | Unique constraint failed |
| `P2003` | Foreign key constraint failed |
| `P2025` | Record not found |
| `P2016` | Query interpretation error |
| `P2014` | Required relation violation |

## How It Works

### Prisma to SQL Mapping

```js
// Prisma
prisma.user.findMany({
  where: { role: 'USER' },
  orderBy: { name: 'asc' },
  take: 10,
});

// Generated SQL
SELECT * FROM users WHERE role = 'USER' ORDER BY name ASC LIMIT 10;
```

### select vs include

```js
// select — choose EXACTLY which fields to return
prisma.user.findMany({
  select: { id: true, name: true },  // Only id and name
});

// include — get all fields PLUS related records
prisma.user.findMany({
  include: { posts: true },  // All user fields + posts
});
```

## Common Mistakes

### Mistake 1: Using findFirst When findUnique Suffices

```js
// WRONG — findFirst does a full scan even for unique fields
const user = await prisma.user.findFirst({ where: { email: 'a@b.com' } });

// CORRECT — findUnique uses the unique index
const user = await prisma.user.findUnique({ where: { email: 'a@b.com' } });
```

### Mistake 2: Not Handling Not Found

```js
// WRONG — findUnique returns null if not found
const user = await prisma.user.findUnique({ where: { id: 999 } });
console.log(user.name);  // TypeError: Cannot read property 'name' of null

// CORRECT — check for null
const user = await prisma.user.findUnique({ where: { id: 999 } });
if (!user) throw new Error('User not found');
```

### Mistake 3: N+1 Queries in a Loop

```js
// WRONG — one query per user
for (const user of users) {
  const posts = await prisma.post.findMany({ where: { authorId: user.id } });
}

// CORRECT — include in a single query
const users = await prisma.user.findMany({
  include: { posts: true },
});
```

## Try It Yourself

### Exercise 1: Full CRUD

Create a user, read it by email, update its name, then delete it. Verify each step.

### Exercise 2: Upsert

Write a loop that upserts 10 users. Run it twice and verify no duplicates are created.

### Exercise 3: Error Handling

Try to create a user with a duplicate email. Catch the P2002 error and return a user-friendly message.

## Next Steps

You can perform CRUD operations. For filtering and pagination, continue to [Filtering & Sorting](./02-filtering-sorting.md).
