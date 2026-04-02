# Supabase Security

## What You'll Learn

- How to secure Supabase Auth
- How to configure RLS policies
- How to handle token refresh
- How to prevent common attacks

## Token Refresh

```ts
// Supabase automatically refreshes tokens
// But you can manually refresh:

const { data, error } = await supabase.auth.refreshSession();
```

## Security Best Practices

- Enable email confirmation
- Set password requirements in Dashboard
- Use RLS on all tables
- Never expose service_role key in client code
- Enable CAPTCHA for sign-up
- Set session timeout in Dashboard

## Next Steps

For MFA, continue to [Supabase MFA](./05-supabase-multi-factor.md).
