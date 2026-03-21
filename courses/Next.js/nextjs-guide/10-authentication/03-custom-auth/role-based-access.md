# Role-Based Access Control

## User Roles

```typescript
// src/types/auth.ts
export type Role = "admin" | "user" | "guest";

export interface Session {
  userId: string;
  role: Role;
}
```

## Protecting Routes by Role

```typescript
// src/lib/auth.ts
export async function requireRole(requiredRole: Role) {
  const session = await getSession();
  
  if (!session) {
    throw new Error("Unauthorized");
  }
  
  if (session.role !== requiredRole && session.role !== "admin") {
    throw new Error("Forbidden");
  }
  
  return session;
}
```

```typescript
// src/app/admin/page.tsx
import { requireRole } from "@/lib/auth";

export default async function AdminPage() {
  const session = await requireRole("admin");
  
  return <h1>Admin Dashboard</h1>;
}
```
