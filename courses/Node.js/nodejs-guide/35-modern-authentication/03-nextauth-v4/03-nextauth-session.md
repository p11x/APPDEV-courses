# NextAuth Session

## What You'll Learn

- How sessions work in NextAuth
- How to use JWT vs database sessions
- How to access session data
- How to extend session types

## JWT Strategy

```ts
// app/api/auth/[...nextauth]/route.ts

export default NextAuth({
  session: {
    strategy: 'jwt',
    maxAge: 30 * 24 * 60 * 60,  // 30 days
  },
  callbacks: {
    async jwt({ token, user }) {
      if (user) {
        token.id = user.id;
        token.role = user.role;
      }
      return token;
    },
    async session({ session, token }) {
      session.user.id = token.id as string;
      session.user.role = token.role as string;
      return session;
    },
  },
});
```

## Using Session

```tsx
// app/dashboard/page.tsx

import { getServerSession } from 'next-auth';

export default async function Dashboard() {
  const session = await getServerSession();

  if (!session) {
    redirect('/api/auth/signin');
  }

  return <div>Welcome, {session.user?.name}</div>;
}
```

## Next Steps

For security, continue to [NextAuth Security](./04-nextauth-security.md).
