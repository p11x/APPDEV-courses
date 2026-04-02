# Prisma Migrations

## What You'll Learn

- What database migrations are and why they matter
- How to create and apply migrations with `prisma migrate dev`
- How to deploy migrations in production with `prisma migrate deploy`
- What the shadow database is and why it exists
- How to reset and seed a database

## What Are Migrations?

When you change your Prisma schema (add a field, rename a table), the database must change too. **Migrations** are versioned SQL scripts that evolve the database schema safely.

```
schema.prisma (v1) → migration 001 (CREATE TABLE users...)
schema.prisma (v2) → migration 002 (ALTER TABLE users ADD COLUMN bio...)
schema.prisma (v3) → migration 003 (CREATE TABLE posts...)
```

## Creating Migrations

### Initial Migration

```bash
npx prisma migrate dev --name init
```

This:
1. Compares the schema to the current database state
2. Generates SQL in `prisma/migrations/0001_init/migration.sql`
3. Applies the migration to the database
4. Regenerates Prisma Client

### Adding a Field

```prisma
model User {
  id    Int    @id @default(autoincrement())
  name  String
  email String @unique
  bio   String?  // New field
}
```

```bash
npx prisma migrate dev --name add-user-bio
```

Generated SQL (PostgreSQL):

```sql
ALTER TABLE "users" ADD COLUMN "bio" TEXT;
```

### Adding a New Model

```prisma
model Comment {
  id       Int    @id @default(autoincrement())
  text     String
  authorId Int
  postId   Int
}
```

```bash
npx prisma migrate dev --name add-comments
```

## Migration Commands

```bash
# Development: create and apply a migration
npx prisma migrate dev --name <description>

# Production: apply pending migrations (no schema creation)
npx prisma migrate deploy

# Check migration status
npx prisma migrate status

# Reset the database (delete all data and reapply all migrations)
npx prisma migrate reset

# Generate Prisma Client without running migrations
npx prisma generate
```

## Seeding

```js
// prisma/seed.js — Seed the database with initial data

import { PrismaClient } from '@prisma/client';

const prisma = new PrismaClient();

async function main() {
  // Clear existing data
  await prisma.comment.deleteMany();
  await prisma.post.deleteMany();
  await prisma.user.deleteMany();

  // Create users
  const alice = await prisma.user.create({
    data: {
      email: 'alice@example.com',
      name: 'Alice',
      bio: 'Full-stack developer',
    },
  });

  const bob = await prisma.user.create({
    data: {
      email: 'bob@example.com',
      name: 'Bob',
      bio: 'DevOps engineer',
    },
  });

  // Create posts
  await prisma.post.createMany({
    data: [
      { title: 'Getting Started with Prisma', content: 'Prisma is...', authorId: alice.id },
      { title: 'Node.js Best Practices', content: 'Always...', authorId: alice.id },
      { title: 'Docker for Beginners', content: 'Containers...', authorId: bob.id },
    ],
  });

  console.log('Database seeded!');
}

main()
  .catch(console.error)
  .finally(() => prisma.$disconnect());
```

Add to `package.json`:

```json
{
  "prisma": {
    "seed": "node prisma/seed.js"
  }
}
```

Run the seed:

```bash
npx prisma db seed
```

Or reset and seed in one command:

```bash
npx prisma migrate reset  # Drops DB, runs all migrations, runs seed
```

## Shadow Database

The **shadow database** is a temporary database Prisma creates during `prisma migrate dev` to detect schema drift. It:
1. Creates a temporary database
2. Applies all existing migrations
3. Generates the new migration by comparing to your schema
4. Deletes the temporary database

For PostgreSQL, add a second database URL:

```env
DATABASE_URL="postgresql://user:pass@localhost:5432/mydb"
SHADOW_DATABASE_URL="postgresql://user:pass@localhost:5432/mydb_shadow"
```

## How It Works

### Migration Files

```
prisma/
└── migrations/
    ├── 20240115100000_init/
    │   └── migration.sql
    ├── 20240115110000_add_user_bio/
    │   └── migration.sql
    └── migration_lock.toml
```

Each migration is a timestamped folder containing the SQL that was generated.

### Dev vs Deploy

| Command | Use | Creates Migrations | Applies Migrations | Resets Data |
|---------|-----|-------------------|-------------------|-------------|
| `migrate dev` | Development | Yes | Yes | Yes (if schema changed) |
| `migrate deploy` | Production | No | Pending only | No |
| `migrate reset` | Development | No | All | Yes |

## Common Mistakes

### Mistake 1: Editing Migration Files After Apply

```bash
# WRONG — editing the SQL after it was applied
# Prisma's migration history no longer matches the database
vim prisma/migrations/20240115_init/migration.sql

# CORRECT — create a new migration for changes
npx prisma migrate dev --name fix-users
```

### Mistake 2: Running migrate dev in Production

```bash
# WRONG — migrate dev can reset the database!
npx prisma migrate dev  # May delete production data!

# CORRECT — use migrate deploy in production
npx prisma migrate deploy  # Only applies pending migrations
```

### Mistake 3: Not Running Migrations on Deploy

```bash
# WRONG — deploying new code without running migrations
# The code expects new columns that don't exist
docker build -t myapp .
docker run myapp  # Crashes: "column bio does not exist"

# CORRECT — run migrations as part of deploy
npx prisma migrate deploy && node server.js
```

## Try It Yourself

### Exercise 1: Add a Model

Add a `Category` model with `id` and `name`. Add a `categoryId` relation to `Post`. Create and apply the migration.

### Exercise 2: Seed Data

Write a seed script that creates 5 users, 10 posts, and 3 categories. Run `prisma db seed`.

### Exercise 3: Reset and Verify

Run `prisma migrate reset`. Verify the seed data is present. Add a new field, run `migrate dev`, and verify the field exists.

## Next Steps

You can manage schema migrations. For advanced queries, continue to [CRUD Queries](../prisma-queries/01-crud-queries.md).
