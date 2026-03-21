# Using Drizzle ORM with Next.js

## What You'll Learn
- Setting up Drizzle with Next.js
- Type-safe database queries

```typescript
// src/lib/db.ts
import { drizzle } from "drizzle-orm/postgres-js";
import postgres from "postgres";
import * as schema from "./schema";

const client = postgres(process.env.DATABASE_URL!);
export const db = drizzle(client, { schema });
```

```typescript
// src/app/users/page.tsx
import { db } from "@/lib/db";
import { users } from "@/lib/schema";

export default async function UsersPage() {
  const allUsers = await db.select().from(users);
  
  return (
    <ul>
      {allUsers.map((user) => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

## Summary

- Drizzle provides type-safe SQL queries
- Works great with Server Components
- Lightweight and fast
