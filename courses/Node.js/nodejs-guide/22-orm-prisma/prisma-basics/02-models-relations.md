# Models & Relations

## What You'll Learn

- How to define Prisma models with field types and attributes
- How to create one-to-one, one-to-many, and many-to-many relations
- How to use @id, @unique, @default, and @updatedAt
- How to define enums
- How relation fields work in queries

## Field Types

| Prisma Type | Database | JavaScript |
|-------------|----------|------------|
| `String` | VARCHAR/TEXT | `string` |
| `Int` | INTEGER | `number` |
| `Float` | FLOAT | `number` |
| `Boolean` | BOOLEAN | `boolean` |
| `DateTime` | TIMESTAMP | `Date` |
| `Json` | JSON | `object` |
| `Bytes` | BYTEA | `Buffer` |

## Model Attributes

```prisma
model User {
  id        Int      @id @default(autoincrement())  // Primary key, auto-increment
  email     String   @unique                         // Unique constraint
  name      String                                   // Required (no ?)
  bio       String?                                  // Optional (nullable)
  role      Role     @default(USER)                  // Enum with default
  createdAt DateTime @default(now())                 // Set on create
  updatedAt DateTime @updatedAt                      // Set on create and update

  @@index([email])                                    // Index for fast lookups
  @@map("users")                                      // Map to different table name
}

enum Role {
  USER
  ADMIN
  MODERATOR
}
```

## Relations

### One-to-Many

```prisma
model User {
  id    Int    @id @default(autoincrement())
  name  String
  posts Post[]   // One user has many posts
}

model Post {
  id       Int    @id @default(autoincrement())
  title    String
  authorId Int                    // Foreign key column
  author   User   @relation(fields: [authorId], references: [id])
}
```

### One-to-One

```prisma
model User {
  id      Int      @id @default(autoincrement())
  name    String
  profile Profile?  // One user has one optional profile
}

model Profile {
  id     Int    @id @default(autoincrement())
  bio    String
  avatar String
  userId Int    @unique            // Unique = one-to-one
  user   User   @relation(fields: [userId], references: [id])
}
```

### Many-to-Many (Implicit)

```prisma
model Post {
  id     Int        @id @default(autoincrement())
  title  String
  tags   Tag[]      // A post can have many tags
}

model Tag {
  id    Int    @id @default(autoincrement())
  name  String @unique
  posts Post[] // A tag can be on many posts
}

// Prisma automatically creates a _PostToTag join table
```

### Many-to-Many (Explicit)

```prisma
model User {
  id      Int         @id @default(autoincrement())
  name    String
  members ProjectMember[]
}

model Project {
  id      Int         @id @default(autoincrement())
  name    String
  members ProjectMember[]
}

model ProjectMember {
  userId    Int
  projectId Int
  role      String    @default("member")
  joinedAt  DateTime  @default(now())

  user    User    @relation(fields: [userId], references: [id])
  project Project @relation(fields: [projectId], references: [id])

  @@id([userId, projectId])  // Composite primary key
}
```

## Full Schema Example

```prisma
// prisma/schema.prisma

generator client {
  provider = "prisma-client-js"
}

datasource db {
  provider = "sqlite"
  url      = env("DATABASE_URL")
}

enum Role {
  USER
  ADMIN
}

model User {
  id        Int      @id @default(autoincrement())
  email     String   @unique
  name      String
  role      Role     @default(USER)
  createdAt DateTime @default(now())
  updatedAt DateTime @updatedAt

  posts    Post[]
  comments Comment[]

  @@map("users")
}

model Post {
  id        Int      @id @default(autoincrement())
  title     String
  content   String?
  published Boolean  @default(false)
  createdAt DateTime @default(now())

  authorId Int
  author   User      @relation(fields: [authorId], references: [id], onDelete: Cascade)

  comments Comment[]
  tags     Tag[]

  @@map("posts")
}

model Comment {
  id        Int      @id @default(autoincrement())
  text      String
  createdAt DateTime @default(now())

  authorId Int
  author   User @relation(fields: [authorId], references: [id])

  postId Int
  post   Post @relation(fields: [postId], references: [id], onDelete: Cascade)

  @@map("comments")
}

model Tag {
  id    Int    @id @default(autoincrement())
  name  String @unique
  posts Post[]

  @@map("tags")
}
```

## Querying Relations

```js
// relations.js — Query and manipulate relations

import { PrismaClient } from '@prisma/client';
const prisma = new PrismaClient();

// Create a user with nested post creation
const user = await prisma.user.create({
  data: {
    email: 'alice@example.com',
    name: 'Alice',
    // Create related posts at the same time
    posts: {
      create: [
        { title: 'First Post', content: 'Hello world!' },
        { title: 'Second Post', published: true },
      ],
    },
  },
  // Include the posts in the response
  include: { posts: true },
});
console.log('User with posts:', user);

// Query with nested filtering
const posts = await prisma.post.findMany({
  where: {
    author: { name: 'Alice' },  // Filter by related model
    published: true,
  },
  include: {
    author: { select: { name: true } },  // Include author name only
    comments: true,
    tags: true,
  },
});

// Add a tag to a post
await prisma.post.update({
  where: { id: 1 },
  data: {
    tags: {
      connect: { name: 'javascript' },  // Connect to existing tag
      // create: { name: 'javascript' },  // Or create and connect
    },
  },
});

// Remove a tag from a post
await prisma.post.update({
  where: { id: 1 },
  data: {
    tags: {
      disconnect: { name: 'javascript' },  // Remove the relation
    },
  },
});

await prisma.$disconnect();
```

## How It Works

### Relation Fields vs Foreign Keys

```prisma
authorId Int    // ← Foreign key column (exists in database)
author   User   @relation(fields: [authorId], references: [id])
//  ↑ Virtual field (does not exist in database — Prisma resolves it via JOIN)
```

### Cascade Deletes

```prisma
author User @relation(fields: [authorId], references: [id], onDelete: Cascade)
// When the user is deleted, all their posts are also deleted
```

Options: `Cascade`, `Restrict` (prevent delete), `SetNull` (set FK to null).

## Common Mistakes

### Mistake 1: Forgetting the Foreign Key Field

```prisma
// WRONG — no foreign key column
model Post {
  author User @relation(fields: [authorId], references: [id])
  // Prisma needs authorId to exist!
}

// CORRECT — include the foreign key
model Post {
  authorId Int
  author   User @relation(fields: [authorId], references: [id])
}
```

### Mistake 2: Not Using include or select

```js
// WRONG — relations are NOT included by default
const user = await prisma.user.findUnique({ where: { id: 1 } });
console.log(user.posts);  // undefined!

// CORRECT — explicitly include relations
const user = await prisma.user.findUnique({
  where: { id: 1 },
  include: { posts: true },
});
console.log(user.posts);  // [{ id: 1, title: '...' }]
```

### Mistake 3: Querying Through the Wrong Side

```js
// WRONG — get all users who have a specific post (inefficient)
const users = await prisma.user.findMany({
  where: { posts: { some: { id: 1 } } },
});

// CORRECT — get the post's author directly
const post = await prisma.post.findUnique({
  where: { id: 1 },
  include: { author: true },
});
```

## Try It Yourself

### Exercise 1: Add a Profile

Add a one-to-one Profile model to User. Create a user with a profile and query it.

### Exercise 2: Many-to-Many Tags

Create 3 tags. Add them to a post. Query the post with all its tags.

### Exercise 3: Cascade Delete

Delete a user and verify that their posts are also deleted (with `onDelete: Cascade`).

## Next Steps

You can design schemas with relations. For database migrations, continue to [Migrations](./03-migrations.md).
