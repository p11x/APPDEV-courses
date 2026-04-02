# Clerk React Components

## What You'll Learn

- How to use Clerk's pre-built React components
- How to protect routes
- How to access user data
- How to customize Clerk UI

## Protecting Routes

```tsx
// app/dashboard/page.tsx

import { auth } from '@clerk/nextjs/server';
import { redirect } from 'next/navigation';

export default async function DashboardPage() {
  const { userId } = await auth();

  if (!userId) {
    redirect('/sign-in');
  }

  return <div>Welcome to the dashboard!</div>;
}
```

## Using User Data

```tsx
// components/UserProfile.tsx

import { currentUser } from '@clerk/nextjs/server';

export async function UserProfile() {
  const user = await currentUser();

  if (!user) return null;

  return (
    <div>
      <img src={user.imageUrl} alt={user.firstName || 'User'} />
      <h2>{user.firstName} {user.lastName}</h2>
      <p>{user.emailAddresses[0]?.emailAddress}</p>
    </div>
  );
}
```

## Client-Side Components

```tsx
// components/UserButton.tsx

import { UserButton, useUser } from '@clerk/nextjs';

export function Header() {
  const { user, isLoaded } = useUser();

  if (!isLoaded) return <div>Loading...</div>;

  return (
    <header className="flex justify-between p-4">
      <h1>My App</h1>
      <UserButton afterSignOutUrl="/" />
    </header>
  );
}
```

## Customization

```tsx
// Custom sign-in appearance
<SignIn
  appearance={{
    elements: {
      formButtonPrimary: 'bg-blue-500 hover:bg-blue-600',
      card: 'shadow-lg rounded-lg',
    },
  }}
/>
```

## Next Steps

For backend auth, continue to [Clerk Backend](./03-clerk-backend.md).
