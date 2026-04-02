# Supabase Setup

## What You'll Learn

- How to set up Supabase Auth
- How Supabase Auth works with Row Level Security
- How to configure auth providers
- How Supabase compares to other auth solutions

## Setup

```bash
npm install @supabase/supabase-js
```

```ts
// lib/supabase.ts

import { createClient } from '@supabase/supabase-js';

const supabase = createClient(
  process.env.NEXT_PUBLIC_SUPABASE_URL!,
  process.env.NEXT_PUBLIC_SUPABASE_ANON_KEY!
);

export { supabase };
```

## Auth Methods

```ts
// Email/password sign up
const { data, error } = await supabase.auth.signUp({
  email: 'alice@example.com',
  password: 'secure-password',
});

// Email/password sign in
const { data, error } = await supabase.auth.signInWithPassword({
  email: 'alice@example.com',
  password: 'secure-password',
});

// OAuth (Google, GitHub, etc.)
const { data, error } = await supabase.auth.signInWithOAuth({
  provider: 'google',
});

// Magic link
const { data, error } = await supabase.auth.signInWithOtp({
  email: 'alice@example.com',
});

// Sign out
await supabase.auth.signOut();
```

## Next Steps

For auth features, continue to [Supabase Auth](./02-supabase-auth.md).
