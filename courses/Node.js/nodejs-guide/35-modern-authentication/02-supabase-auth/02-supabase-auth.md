# Supabase Auth

## What You'll Learn

- How Supabase Auth works
- How to implement email/password auth
- How to use magic links
- How to handle auth state

## Email/Password Auth

```ts
// lib/auth.ts

import { supabase } from './supabase.js';

export async function signUp(email: string, password: string) {
  const { data, error } = await supabase.auth.signUp({
    email,
    password,
    options: {
      emailRedirectTo: `${window.location.origin}/auth/callback`,
    },
  });

  if (error) throw error;
  return data;
}

export async function signIn(email: string, password: string) {
  const { data, error } = await supabase.auth.signInWithPassword({
    email,
    password,
  });

  if (error) throw error;
  return data;
}

export async function signOut() {
  const { error } = await supabase.auth.signOut();
  if (error) throw error;
}
```

## Magic Link

```ts
export async function sendMagicLink(email: string) {
  const { error } = await supabase.auth.signInWithOtp({
    email,
    options: {
      emailRedirectTo: `${window.location.origin}/auth/callback`,
    },
  });

  if (error) throw error;
}
```

## Auth State

```ts
// Listen for auth changes
supabase.auth.onAuthStateChange((event, session) => {
  console.log(event, session);

  if (event === 'SIGNED_IN') {
    // User signed in
    console.log('User:', session?.user);
  }

  if (event === 'SIGNED_OUT') {
    // User signed out
  }
});

// Get current session
const { data: { session } } = await supabase.auth.getSession();

// Get current user
const { data: { user } } = await supabase.auth.getUser();
```

## Next Steps

For roles, continue to [Supabase Roles](./03-supabase-roles.md).
