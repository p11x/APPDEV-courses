# Authentication Fundamentals and Architecture

## What You'll Learn

- Authentication vs Authorization concepts
- Stateless vs stateful authentication
- Authentication flow design
- Token types and their characteristics
- Authentication security principles

## Authentication vs Authorization

```
Authentication vs Authorization:
─────────────────────────────────────────────
Authentication (AuthN)          Authorization (AuthZ)
"Who are you?"                  "What can you do?"
Verifies identity               Verifies permissions
Happens first                   Happens after AuthN
Login, tokens, biometrics       Roles, permissions, ACLs
```

```javascript
// Authentication: Verify identity
async function authenticate(credentials) {
    const user = await User.findByEmail(credentials.email);
    if (!user) throw new AuthError('User not found');

    const valid = await bcrypt.compare(credentials.password, user.passwordHash);
    if (!valid) throw new AuthError('Invalid password');

    return user;
}

// Authorization: Check permissions
function authorize(user, resource, action) {
    const permission = `${resource}:${action}`;
    if (!user.permissions.includes(permission)) {
        throw new AuthError('Insufficient permissions', 403);
    }
    return true;
}

// Usage in middleware
app.post('/api/posts',
    authenticateMiddleware,   // AuthN: Who is this user?
    authorizeMiddleware('posts', 'create'),  // AuthZ: Can they create posts?
    createPostHandler
);
```

## Stateless vs Stateful Authentication

```
Stateless (JWT)                    Stateful (Sessions)
──────────────────────────────────────────────────────
Token carries all data             Server stores session data
No server-side storage             Redis/DB session store
Scales horizontally easily         Shared session store needed
Cannot revoke instantly             Instant revocation
Larger token size                  Small cookie size
Good for APIs/microservices        Good for traditional web apps
```

```javascript
// Stateless: JWT-based
const token = jwt.sign({ userId: user.id, role: user.role }, SECRET, {
    expiresIn: '1h',
});
// Client sends token with each request — server just verifies signature

// Stateful: Session-based
req.session.userId = user.id;
req.session.role = user.role;
// Server looks up session data on each request
```

## Authentication Flow Design

```
Standard Authentication Flow:
─────────────────────────────────────────────
Client                    Server                  Database
  │                         │                        │
  │── POST /login ─────────►│                        │
  │   {email, password}     │── Find user ──────────►│
  │                         │◄── user record ────────│
  │                         │── Verify password      │
  │                         │── Generate token       │
  │◄── {token, user} ──────│                        │
  │                         │                        │
  │── GET /api/data ───────►│                        │
  │   Authorization: Bearer │── Verify token         │
  │                         │── Check permissions    │
  │◄── {data} ─────────────│                        │
```

## Token Types

```
Token Types Comparison:
─────────────────────────────────────────────
Type         Size    Revocable  Storage    Use Case
─────────────────────────────────────────────
JWT          Medium  No*        Client     API auth
Session ID   Small   Yes        Server     Web apps
Opaque       Small   Yes        Server     Reference tokens
API Key      Small   Yes        Server     Service auth

* JWT revocation requires denylist/blocklist
```

```javascript
// JWT: Self-contained token
const jwt = jwt.sign({ userId: 1, role: 'admin' }, SECRET);
// Contains all claims — server verifies signature only

// Opaque: Reference token
const token = crypto.randomBytes(32).toString('hex');
await redis.set(`token:${token}`, JSON.stringify({ userId: 1, role: 'admin' }), { EX: 3600 });
// Token is a key — server looks up data in store

// Session ID: Server-side session
const sessionId = crypto.randomBytes(16).toString('hex');
sessions[sessionId] = { userId: 1, role: 'admin' };
// Cookie contains only session ID
```

## Authentication Security Principles

```
Security Principles:
─────────────────────────────────────────────
1. Never store passwords in plaintext
2. Use parameterized queries for user lookups
3. Implement rate limiting on login endpoints
4. Use HTTPS for all authentication flows
5. Set secure cookie flags (httpOnly, secure, sameSite)
6. Implement account lockout after failed attempts
7. Use timing-safe comparison for secrets
8. Log authentication events for audit
9. Implement proper session invalidation
10. Use MFA for sensitive operations
```

## Best Practices Checklist

- [ ] Choose stateless (JWT) for APIs, stateful (sessions) for web apps
- [ ] Implement both authentication and authorization
- [ ] Use appropriate token types for your use case
- [ ] Follow OWASP authentication guidelines
- [ ] Log all authentication events
- [ ] Implement rate limiting on auth endpoints
- [ ] Use HTTPS exclusively for auth flows

## Cross-References

- See [Traditional Auth](./01-traditional-auth.md) for password-based auth
- See [JWT Guide](../jwt/04-jwt-refresh-tokens.md) for token management
- See [Security](../06-authentication-security/01-security-headers.md) for hardening

## Next Steps

Continue to [Traditional Authentication Methods](./01-traditional-auth.md).
