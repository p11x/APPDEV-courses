# JWT Security, Production Patterns, and Debugging

## What You'll Learn

- JWT algorithm selection and security
- JWT storage strategies (httpOnly cookies vs localStorage)
- JWT debugging and troubleshooting
- JWT production deployment considerations
- JWT vs opaque token comparison

## JWT Algorithm Selection

```
JWT Algorithm Security Matrix:
─────────────────────────────────────────────
Algorithm    Type         Key Size   Security    Use Case
─────────────────────────────────────────────
HS256        Symmetric    256-bit    Good        Single service
HS384        Symmetric    384-bit    Better      Single service (higher)
HS512        Symmetric    512-bit    Best        Single service (highest)
RS256        Asymmetric   2048-bit   Good        Microservices
RS384        Asymmetric   3072-bit   Better      Microservices (higher)
RS512        Asymmetric   4096-bit   Best        Microservices (highest)
ES256        Asymmetric   256-bit    Good        Mobile/IoT (small keys)
ES384        Asymmetric   384-bit    Better      Mobile/IoT (higher)
EdDSA        Asymmetric   256-bit    Best        Modern choice
─────────────────────────────────────────────

NEVER use: none, HS1, RS1 (insecure)
```

```javascript
import jwt from 'jsonwebtoken';
import { generateKeyPairSync } from 'node:crypto';

// Symmetric (HS256) — single service, simple setup
const symmetricToken = jwt.sign(payload, process.env.JWT_SECRET, {
    algorithm: 'HS256',
    expiresIn: '15m',
    issuer: 'myapp',
});

// Asymmetric (RS256) — microservices, public key verification
const { publicKey, privateKey } = generateKeyPairSync('rsa', {
    modulusLength: 2048,
    publicKeyEncoding: { type: 'spki', format: 'pem' },
    privateKeyEncoding: { type: 'pkcs8', format: 'pem' },
});

const asymmetricToken = jwt.sign(payload, privateKey, {
    algorithm: 'RS256',
    expiresIn: '15m',
    issuer: 'myapp',
});

// Any service can verify with just the public key
const decoded = jwt.verify(asymmetricToken, publicKey, {
    algorithms: ['RS256'],
    issuer: 'myapp',
});
```

## JWT Storage Strategies

```
Storage Strategy Comparison:
─────────────────────────────────────────────
Storage         XSS Safe  CSRF Risk  Size Limit  Mobile
─────────────────────────────────────────────
httpOnly Cookie Yes       Need guard None        Yes
localStorage    No        Low        5-10MB      Yes
sessionStorage  No        Low        5-10MB      No
Memory (JS var) Yes       Low        None        Yes
─────────────────────────────────────────────
```

```javascript
// STRATEGY 1: httpOnly Cookie (Recommended for web apps)
app.post('/auth/login', (req, res) => {
    const { accessToken, refreshToken } = generateTokens(user);

    // Access token in httpOnly cookie
    res.cookie('accessToken', accessToken, {
        httpOnly: true,       // No JavaScript access (XSS protection)
        secure: true,         // HTTPS only
        sameSite: 'strict',   // CSRF protection
        maxAge: 15 * 60 * 1000, // 15 minutes
        path: '/',
    });

    // Refresh token in separate httpOnly cookie
    res.cookie('refreshToken', refreshToken, {
        httpOnly: true,
        secure: true,
        sameSite: 'strict',
        maxAge: 7 * 24 * 60 * 60 * 1000, // 7 days
        path: '/auth/refresh', // Only sent to refresh endpoint
    });

    res.json({ user: { id: user.id, email: user.email } });
});

// Read token from cookie
app.use((req, res, next) => {
    const token = req.cookies?.accessToken;
    if (token) {
        try {
            req.user = jwt.verify(token, process.env.JWT_SECRET);
        } catch { /* invalid token */ }
    }
    next();
});

// STRATEGY 2: Bearer header (API-first, mobile apps)
app.use((req, res, next) => {
    const auth = req.headers.authorization;
    if (auth?.startsWith('Bearer ')) {
        const token = auth.slice(7);
        try {
            req.user = jwt.verify(token, process.env.JWT_SECRET);
        } catch { /* invalid */ }
    }
    next();
});

// STRATEGY 3: Split token (maximum security)
// Access token: short-lived, in memory only
// Refresh token: long-lived, in httpOnly cookie
// Neither stored in localStorage
```

## JWT Debugging Utilities

```javascript
function debugJWT(token) {
    const [headerB64, payloadB64, signatureB64] = token.split('.');

    const decode = (str) => JSON.parse(
        Buffer.from(str, 'base64url').toString()
    );

    const header = decode(headerB64);
    const payload = decode(payloadB64);
    const now = Math.floor(Date.now() / 1000);

    return {
        header,
        payload,
        signaturePreview: signatureB64.slice(0, 20) + '...',
        size: {
            total: token.length,
            header: headerB64.length,
            payload: payloadB64.length,
            signature: signatureB64.length,
        },
        timing: {
            issuedAt: payload.iat
                ? new Date(payload.iat * 1000).toISOString()
                : null,
            expiresAt: payload.exp
                ? new Date(payload.exp * 1000).toISOString()
                : null,
            notBefore: payload.nbf
                ? new Date(payload.nbf * 1000).toISOString()
                : null,
            isExpired: payload.exp ? payload.exp < now : false,
            expiresIn: payload.exp
                ? `${Math.max(0, payload.exp - now)}s`
                : 'never',
        },
        claims: {
            issuer: payload.iss,
            subject: payload.sub,
            audience: payload.aud,
            jwtId: payload.jti,
        },
    };
}

// Common JWT errors and solutions:
const jwtErrorGuide = {
    'TokenExpiredError': {
        cause: 'Token has expired',
        solution: 'Use refresh token to get new access token',
        prevention: 'Set appropriate expiration time',
    },
    'JsonWebTokenError': {
        cause: 'Invalid signature or malformed token',
        solution: 'Check secret key matches, verify token format',
        prevention: 'Use strong secrets, validate token structure',
    },
    'NotBeforeError': {
        cause: 'Token used before nbf claim time',
        solution: 'Wait until nbf time or remove nbf claim',
        prevention: 'Set nbf to current time or omit',
    },
    'invalid signature': {
        cause: 'Secret key mismatch or token tampered',
        solution: 'Verify JWT_SECRET environment variable',
        prevention: 'Use secrets manager, rotate keys properly',
    },
    'invalid algorithm': {
        cause: 'Token signed with unexpected algorithm',
        solution: 'Specify allowed algorithms in verify()',
        prevention: 'Always use algorithms option: { algorithms: ["HS256"] }',
    },
};
```

## JWT vs Opaque Tokens

```
JWT vs Opaque Token Comparison:
─────────────────────────────────────────────
Feature          JWT                Opaque Token
─────────────────────────────────────────────
Self-contained   Yes (has claims)   No (reference only)
Size             200-2000 bytes     32-64 bytes
Revocation       Needs denylist     Instant (delete from DB)
Validation       Local (fast)       Remote lookup (slow)
State            Stateless          Stateful
Scalability      Horizontal easy    Needs shared state
Security         Depends on algo    Depends on storage
─────────────────────────────────────────────

Use JWT when:
├── Microservices (no shared state needed)
├── High read throughput (local validation)
└── Mobile apps (offline validation possible)

Use Opaque tokens when:
├── Need instant revocation
├── Small token size matters
└── Single service with Redis
```

## Common Mistakes

- Not specifying algorithms in verify() (algorithm confusion attack)
- Storing JWT in localStorage (XSS vulnerable)
- Using 'none' algorithm (anyone can forge tokens)
- Not validating issuer/audience claims
- Making access tokens too long-lived (> 15 minutes)

## Cross-References

- See [JWT Basics](./01-jwt-basics.md) for fundamentals
- See [Security](../06-authentication-security/01-security-headers.md) for hardening
- See [Performance](../08-authentication-performance/01-performance-optimization.md) for optimization

## Next Steps

Continue to [OAuth 2.0 PKCE](../04-oauth2-oidc/02-pkce-social-providers.md).
