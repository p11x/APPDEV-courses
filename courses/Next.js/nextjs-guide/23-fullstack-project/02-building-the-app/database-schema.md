# Database Schema

## What You'll Learn
- Prisma schema design
- User and Task models
- Relationships

## Prerequisites
- Prisma installed

## Do I Need This Right Now?
Database schema is the foundation of the app.

## Prisma Schema

This references **Section 05 (Data Fetching)** — using Prisma as the data fetching layer.

```prisma
// prisma/schema.prisma

generator client {
  provider = "prisma-client-js"
}

datasource db {
  provider = "postgresql"
  url      = env("DATABASE_URL")
}

model User {
  id        String   @id @default(cuid())
  email     String   @unique
  name      String?
  password  String
  createdAt DateTime @default(now())
  updatedAt DateTime @updatedAt
  
  tasks     Task[]
}

model Task {
  id          String   @id @default(cuid())
  title       String
  description String?
  completed   Boolean  @default(false)
  createdAt   DateTime @default(now())
  updatedAt   DateTime @updatedAt
  
  userId      String
  user        User     @relation(fields: [userId], references: [id], onDelete: Cascade)
}
```

## Summary
- User model with auth fields
- Task model with user relation
- Cascade delete for cleanup

## Next Steps
- [auth-integration.md](./auth-integration.md) — Authentication setup
