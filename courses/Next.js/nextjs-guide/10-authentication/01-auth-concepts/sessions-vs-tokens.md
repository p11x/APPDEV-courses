# Sessions vs Tokens

## Sessions

Server-side storage:
- Session ID stored in cookie
- Server validates session
- Good for server-rendered apps

## Tokens (JWT)

Client-side storage:
- Token stored in localStorage/cookies
- Server validates token signature
- Good for SPAs and mobile apps

## In Next.js

- Use cookies for session management
- HttpOnly cookies for security
- Consider NextAuth.js for easy setup
