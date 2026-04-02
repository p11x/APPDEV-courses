# NextAuth Custom Pages

## What You'll Learn

- How to customize sign-in/sign-out pages
- How to add custom error pages
- How to style NextAuth pages
- How to add custom fields to sign-up

## Custom Pages

```ts
export default NextAuth({
  pages: {
    signIn: '/auth/signin',
    signOut: '/auth/signout',
    error: '/auth/error',
    verifyRequest: '/auth/verify-request',
    newUser: '/auth/new-user',
  },
});
```

```tsx
// app/auth/signin/page.tsx

import { getProviders } from 'next-auth/react';
import { SignInButton } from './SignInButton';

export default async function SignInPage() {
  const providers = await getProviders();

  return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="bg-white p-8 rounded-lg shadow">
        <h1 className="text-2xl mb-6">Sign In</h1>
        {Object.values(providers!).map((provider) => (
          <SignInButton key={provider.id} provider={provider} />
        ))}
      </div>
    </div>
  );
}
```

## Next Steps

For Firebase, continue to [Firebase Auth](../04-firebase-auth/02-firebase-auth.md).
