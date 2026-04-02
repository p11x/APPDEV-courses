# Clerk Setup

## What You'll Learn

- What Clerk is and why it's popular
- How to set up Clerk with Next.js
- How to configure Clerk dashboard
- How Clerk compares to other auth providers

## What Is Clerk?

Clerk is a **complete authentication and user management** solution. It provides pre-built UI components, handles OAuth, MFA, and session management — so you don't build auth from scratch.

| Feature | Clerk | NextAuth | Supabase Auth |
|---------|-------|----------|---------------|
| Pre-built UI | Yes (React components) | No | Partial |
| OAuth providers | 20+ | 80+ | Google, GitHub, etc. |
| MFA | Built-in | Manual | Built-in |
| User management | Dashboard | No | Dashboard |
| Pricing | Free up to 10K MAU | Free (self-hosted) | Free up to 50K MAU |
| Best for | Fastest auth setup | Custom auth flows | Full backend |

## Setup

```bash
npm install @clerk/nextjs
```

## Dashboard Configuration

1. Go to [clerk.com](https://clerk.com) and create an account
2. Create a new application
3. Configure sign-in methods (email, Google, GitHub, etc.)
4. Copy your API keys

## Environment Variables

```bash
# .env.local
NEXT_PUBLIC_CLERK_PUBLISHABLE_KEY=pk_test_...
CLERK_SECRET_KEY=sk_test_...
NEXT_PUBLIC_CLERK_SIGN_IN_URL=/sign-in
NEXT_PUBLIC_CLERK_SIGN_UP_URL=/sign-up
NEXT_PUBLIC_CLERK_AFTER_SIGN_IN_URL=/dashboard
NEXT_PUBLIC_CLERK_AFTER_SIGN_UP_URL=/dashboard
```

## Next.js Integration

```ts
// middleware.ts — Clerk middleware

import { clerkMiddleware } from '@clerk/nextjs/server';

export default clerkMiddleware();

export const config = {
  matcher: ['/((?!.*\\..*|_next).*)', '/', '/(api|trpc)(.*)'],
};
```

```tsx
// app/layout.tsx — Wrap with ClerkProvider

import { ClerkProvider } from '@clerk/nextjs';

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <ClerkProvider>
      <html lang="en">
        <body>{children}</body>
      </html>
    </ClerkProvider>
  );
}
```

## Sign In / Sign Up Pages

```tsx
// app/sign-in/[[...sign-in]]/page.tsx

import { SignIn } from '@clerk/nextjs';

export default function SignInPage() {
  return (
    <div className="flex items-center justify-center min-h-screen">
      <SignIn />
    </div>
  );
}
```

```tsx
// app/sign-up/[[...sign-up]]/page.tsx

import { SignUp } from '@clerk/nextjs';

export default function SignUpPage() {
  return (
    <div className="flex items-center justify-center min-h-screen">
      <SignUp />
    </div>
  );
}
```

## Next Steps

For React components, continue to [Clerk React](./02-clerk-react.md).
