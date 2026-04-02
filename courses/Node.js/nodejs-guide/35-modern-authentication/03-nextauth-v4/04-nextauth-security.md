# NextAuth Security

## What You'll Learn

- How to secure NextAuth
- How to handle CSRF protection
- How to configure secure cookies
- How to prevent common attacks

## Configuration

```ts
export default NextAuth({
  // Use secure cookies in production
  useSecureCookies: process.env.NODE_ENV === 'production',

  cookies: {
    sessionToken: {
      name: process.env.NODE_ENV === 'production'
        ? '__Secure-next-auth.session-token'
        : 'next-auth.session-token',
      options: {
        httpOnly: true,
        sameSite: 'lax',
        path: '/',
        secure: process.env.NODE_ENV === 'production',
      },
    },
  },

  // CSRF protection is built-in
  // NextAuth generates a csrfToken for each session
});
```

## Next Steps

For customization, continue to [NextAuth Custom](./05-nextauth-custom.md).
