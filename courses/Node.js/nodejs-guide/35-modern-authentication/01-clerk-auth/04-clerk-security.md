# Clerk Security

## What You'll Learn

- How Clerk handles security
- How to configure MFA
- How to set up session management
- How to handle security events

## Multi-Factor Authentication

Enable MFA in the Clerk Dashboard:

1. Go to User & Authentication → Multi-factor
2. Enable TOTP (authenticator app)
3. Optionally enable SMS verification
4. Configure backup codes

## Session Configuration

```ts
// clerk-dashboard settings:
// Sessions → Configure
// - Session lifetime: 7 days (default)
// - Session inactivity timeout: 1 day
// - Multi-session handling: revoke or allow multiple
```

## Security Best Practices

- Enable MFA for admin users
- Set session inactivity timeout to 1 hour for sensitive apps
- Use Clerk's `afterSignOutUrl` to redirect properly
- Monitor sign-in attempts in Clerk Dashboard → Users → Activity
- Enable bot detection and rate limiting in Clerk Dashboard

## Next Steps

For Supabase Auth, continue to [Supabase Setup](../02-supabase-auth/01-supabase-setup.md).
