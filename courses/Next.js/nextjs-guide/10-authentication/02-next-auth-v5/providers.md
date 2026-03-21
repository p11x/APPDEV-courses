# NextAuth Providers

## OAuth Providers

```typescript
// src/auth.ts
import NextAuth from "next-auth";
import GitHub from "next-auth/providers/github";
import Google from "next-auth/providers/google";

export const { handlers } = NextAuth({
  providers: [
    GitHub({
      clientId: process.env.GITHUB_ID,
      clientSecret: process.env.GITHUB_SECRET,
    }),
    Google({
      clientId: process.env.GOOGLE_ID,
      clientSecret: process.env.GOOGLE_SECRET,
    }),
  ],
});
```

## Credentials Provider

```typescript
import Credentials from "next-auth/providers/credentials";

export const { handlers } = NextAuth({
  providers: [
    Credentials({
      async authorize(credentials) {
        // Validate credentials
        return user;
      },
    }),
  ],
});
```
