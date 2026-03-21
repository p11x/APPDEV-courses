# Protecting Routes

## In Server Components

```typescript
// src/app/dashboard/page.tsx
import { getSession } from "@/lib/auth";
import { redirect } from "next/navigation";

export default async function DashboardPage() {
  const session = await getSession();
  
  if (!session) {
    redirect("/login");
  }
  
  return <div>Welcome, {session.user.name}</div>;
}
```

## With Server Actions

```typescript
// src/app/actions.ts
"use server";

import { getSession } from "@/lib/auth";
import { redirect } from "next/navigation";

export async function protectedAction() {
  const session = await getSession();
  
  if (!session) {
    redirect("/login");
  }
  
  // Do protected work
}
```
