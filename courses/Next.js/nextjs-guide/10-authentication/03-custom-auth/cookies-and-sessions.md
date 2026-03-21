# Cookies and Sessions

## Setting Cookies

```typescript
// src/app/actions.ts
"use server";

import { cookies } from "next/headers";
import { createToken } from "@/lib/auth";

export async function login(formData: FormData) {
  const email = formData.get("email");
  
  // Validate and get user
  const token = await createToken(user.id);
  
  cookies().set("token", token, {
    httpOnly: true,
    secure: process.env.NODE_ENV === "production",
    sameSite: "strict",
    maxAge: 60 * 60 * 24 * 7, // 1 week
  });
}
```

## Reading Cookies

```typescript
// src/lib/auth.ts
import { cookies } from "next/headers";

export function getSession() {
  const token = cookies().get("token")?.value;
  // Verify and return session
}
```
