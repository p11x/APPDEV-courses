# Session in Server Components

## Getting Session

```typescript
// src/app/page.tsx
import { auth } from "@/auth";
import { redirect } from "next/navigation";

export default async function HomePage() {
  const session = await auth();
  
  if (!session) {
    redirect("/api/auth/signin");
  }
  
  return (
    <main>
      <h1>Welcome, {session.user?.name}</h1>
    </main>
  );
}
```

## Using Session in Client

```typescript
// src/components/UserMenu.tsx
"use client";

import { signOut, useSession } from "next-auth/react";

export function UserMenu() {
  const { data: session } = useSession();
  
  if (!session) return null;
  
  return (
    <button onClick={() => signOut()}>
      Sign out
    </button>
  );
}
```
