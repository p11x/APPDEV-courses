# Filtering & Sorting

## What You'll Learn

- How to filter records with where clauses
- How to use AND, OR, NOT operators
- How to sort results with orderBy
- How to paginate with take/skip and cursor-based pagination
- How to use select and include to shape responses

## Setup

```js
import prisma from './lib/prisma.js';
```

## Filtering with where

```js
// Exact match
const users = await prisma.user.findMany({
  where: { role: 'USER' },
});

// Multiple conditions (implicit AND)
const activeUsers = await prisma.user.findMany({
  where: {
    role: 'USER',
    isActive: true,
  },
});

// String filters
const search = await prisma.user.findMany({
  where: {
    name: {
      startsWith: 'Ali',       // LIKE 'Ali%'
      mode: 'insensitive',     // Case-insensitive
    },
  },
});

// Numeric filters
const recentPosts = await prisma.post.findMany({
  where: {
    viewCount: { gte: 100 },  // viewCount >= 100
    createdAt: { gte: new Date('2024-01-01') },
  },
});

// IN operator
const users = await prisma.user.findMany({
  where: {
    id: { in: [1, 2, 3] },
  },
});
```

## AND, OR, NOT

```js
// Explicit AND
const posts = await prisma.post.findMany({
  where: {
    AND: [
      { published: true },
      { viewCount: { gte: 10 } },
    ],
  },
});

// OR
const results = await prisma.user.findMany({
  where: {
    OR: [
      { email: { contains: 'gmail' } },
      { role: 'ADMIN' },
    ],
  },
});

// NOT
const nonDrafts = await prisma.post.findMany({
  where: {
    NOT: { published: false },
  },
});

// Combined
const complex = await prisma.post.findMany({
  where: {
    AND: [
      { published: true },
      { OR: [
        { viewCount: { gte: 100 } },
        { author: { role: 'ADMIN' } },
      ]},
    ],
  },
});
```

## Sorting

```js
// Sort by one field
const posts = await prisma.post.findMany({
  orderBy: { createdAt: 'desc' },
});

// Sort by multiple fields
const users = await prisma.user.findMany({
  orderBy: [
    { role: 'asc' },     // First by role
    { name: 'asc' },     // Then by name
  ],
});

// Sort by related field
const posts = await prisma.post.findMany({
  orderBy: { author: { name: 'asc' } },
  include: { author: true },
});
```

## Pagination

### Offset-Based (take/skip)

```js
const page = 2;
const pageSize = 10;

const posts = await prisma.post.findMany({
  skip: (page - 1) * pageSize,  // Skip first 10
  take: pageSize,                 // Take 10
  orderBy: { createdAt: 'desc' },
});

// Get total count for page info
const total = await prisma.post.count();
const totalPages = Math.ceil(total / pageSize);
```

### Cursor-Based (for infinite scroll)

```js
// First page
const firstPage = await prisma.post.findMany({
  take: 10,
  orderBy: { id: 'asc' },
});

// Next page — use the last item's ID as cursor
const lastId = firstPage[firstPage.length - 1].id;
const nextPage = await prisma.post.findMany({
  take: 10,
  skip: 1,             // Skip the cursor itself
  cursor: { id: lastId },  // Start from this ID
  orderBy: { id: 'asc' },
});
```

## Shaping Responses

```js
// select — return only specific fields
const users = await prisma.user.findMany({
  select: {
    id: true,
    name: true,
    email: true,
    _count: {
      select: { posts: true },
    },
  },
});
// Result: { id, name, email, _count: { posts: 5 } }

// include — return all fields plus relations
const users = await prisma.user.findMany({
  include: {
    posts: {
      where: { published: true },
      select: { title: true, createdAt: true },
    },
  },
});

// Nested filtering
const users = await prisma.user.findMany({
  where: {
    posts: {
      some: { published: true },  // User has at least one published post
    },
  },
  include: {
    posts: {
      where: { published: true },
      orderBy: { createdAt: 'desc' },
      take: 5,
    },
  },
});
```

## How It Works

### Relation Filters

| Operator | Meaning |
|----------|---------|
| `some` | At least one related record matches |
| `every` | All related records match |
| `none` | No related records match |

```js
// Users who have ANY published post
const authors = await prisma.user.findMany({
  where: { posts: { some: { published: true } } },
});

// Users who have NO posts
const inactive = await prisma.user.findMany({
  where: { posts: { none: {} } },
});
```

## Common Mistakes

### Mistake 1: Offset Pagination for Large Datasets

```js
// WRONG — skip(100000) is slow because the DB must scan 100K rows
const page10001 = await prisma.post.findMany({
  skip: 100000,
  take: 10,
});

// CORRECT — use cursor-based pagination for large offsets
const page = await prisma.post.findMany({
  cursor: { id: lastSeenId },
  take: 10,
});
```

### Mistake 2: Forgetting mode: 'insensitive' for SQLite

```js
// WRONG — SQLite string comparisons are case-sensitive by default
const users = await prisma.user.findMany({
  where: { name: { contains: 'alice' } },
});
// Does not match "Alice" on SQLite

// CORRECT — add mode
const users = await prisma.user.findMany({
  where: { name: { contains: 'alice', mode: 'insensitive' } },
});
```

### Mistake 3: Over-Fetching Data

```js
// WRONG — fetching all fields and relations when you only need 2 fields
const users = await prisma.user.findMany({
  include: { posts: true, comments: true },
});
// Returns potentially megabytes of data

// CORRECT — select only what you need
const users = await prisma.user.findMany({
  select: { id: true, name: true },
});
```

## Try It Yourself

### Exercise 1: Search API

Build a search endpoint: `GET /users?name=ali&role=USER&page=1&pageSize=10`. Use Prisma's where, orderBy, skip, and take.

### Exercise 2: Dashboard Query

Write a query that returns users with: their post count, their latest post title, and whether they are admins. Use select and _count.

### Exercise 3: Cursor Pagination

Implement a cursor-based API: `GET /posts?after=<postId>&limit=20`. Return the next cursor in the response.

## Next Steps

You can filter and paginate. For atomic operations, continue to [Transactions](./03-transactions.md).
