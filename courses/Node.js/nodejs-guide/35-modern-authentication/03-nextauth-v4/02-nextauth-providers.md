# NextAuth Providers

## What You'll Learn

- How to configure OAuth providers
- How to use email/password with NextAuth
- How to add custom providers
- How to handle provider callbacks

## OAuth Providers

```ts
// app/api/auth/[...nextauth]/route.ts

import NextAuth from 'next-auth';
import GitHubProvider from 'next-auth/providers/github';
import GoogleProvider from 'next-auth/providers/google';
import DiscordProvider from 'next-auth/providers/discord';
import AppleProvider from 'next-auth/providers/apple';

export default NextAuth({
  providers: [
    GitHubProvider({
      clientId: process.env.GITHUB_ID!,
      clientSecret: process.env.GITHUB_SECRET!,
    }),
    GoogleProvider({
      clientId: process.env.GOOGLE_ID!,
      clientSecret: process.env.GOOGLE_SECRET!,
    }),
    DiscordProvider({
      clientId: process.env.DISCORD_ID!,
      clientSecret: process.env.DISCORD_SECRET!,
    }),
    AppleProvider({
      clientId: process.env.APPLE_ID!,
      clientSecret: process.env.APPLE_SECRET!,
    }),
  ],
});
```

## Next Steps

For sessions, continue to [NextAuth Session](./03-nextauth-session.md).
