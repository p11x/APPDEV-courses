# JWT Refresh Tokens, Security, and Production Patterns

## What You'll Learn

- JWT refresh token implementation
- JWT security best practices
- JWT token revocation strategies
- JWT performance optimization
- JWT production deployment patterns

## JWT Refresh Token Flow

```
Refresh Token Flow:
─────────────────────────────────────────────
Client                    Server
  │                         │
  │── POST /login ─────────►│
  │   {email, password}     │
  │                         │── Verify credentials
  │                         │── Generate access token (15min)
  │                         │── Generate refresh token (7d)
  │◄── {accessToken,        │── Store refresh token hash
  │     refreshToken}       │
  │                         │
  │── GET /api/data ───────►│
  │   Authorization: Bearer │── Verify access token
  │◄── {data} ─────────────│
  │                         │
  │   (access token expired)│
  │                         │
  │── POST /refresh ───────►│
  │   {refreshToken}        │── Verify refresh token
  │                         │── Generate new access token
  │◄── {accessToken} ──────│
```

```javascript
import jwt from 'jsonwebtoken';
import { randomBytes, createHash } from 'node:crypto';

class TokenService {
    constructor(pool, redis) {
        this.pool = pool;
        this.redis = redis;
        this.accessTokenSecret = process.env.JWT_ACCESS_SECRET;
        this.refreshTokenSecret = process.env.JWT_REFRESH_SECRET;
        this.accessTokenExpiry = '15m';
        this.refreshTokenExpiry = '7d';
    }

    generateAccessToken(user) {
        return jwt.sign(
            {
                sub: user.id,
                email: user.email,
                role: user.role,
                type: 'access',
            },
            this.accessTokenSecret,
            { expiresIn: this.accessTokenExpiry, issuer: 'myapp' }
        );
    }

    async generateRefreshToken(user) {
        const tokenId = randomBytes(16).toString('hex');
        const token = jwt.sign(
            {
                sub: user.id,
                jti: tokenId,
                type: 'refresh',
            },
            this.refreshTokenSecret,
            { expiresIn: this.refreshTokenExpiry, issuer: 'myapp' }
        );

        // Store hash of refresh token for revocation
        const hash = createHash('sha256').update(token).digest('hex');
        await this.redis.set(`refresh:${tokenId}`, hash, {
            EX: 7 * 24 * 60 * 60, // 7 days
        });

        return token;
    }

    async refreshAccessToken(refreshToken) {
        try {
            const decoded = jwt.verify(refreshToken, this.refreshTokenSecret, {
                issuer: 'myapp',
            });

            if (decoded.type !== 'refresh') {
                throw new Error('Invalid token type');
            }

            // Check if token is revoked
            const storedHash = await this.redis.get(`refresh:${decoded.jti}`);
            const tokenHash = createHash('sha256').update(refreshToken).digest('hex');

            if (!storedHash || storedHash !== tokenHash) {
                throw new Error('Token revoked');
            }

            // Get user
            const { rows } = await this.pool.query(
                'SELECT id, email, role FROM users WHERE id = $1',
                [decoded.sub]
            );

            if (rows.length === 0) {
                throw new Error('User not found');
            }

            // Rotate refresh token (invalidate old, issue new)
            await this.redis.del(`refresh:${decoded.jti}`);
            const newAccessToken = this.generateAccessToken(rows[0]);
            const newRefreshToken = await this.generateRefreshToken(rows[0]);

            return {
                accessToken: newAccessToken,
                refreshToken: newRefreshToken,
            };
        } catch (err) {
            throw new AuthError('Invalid refresh token');
        }
    }

    async revokeRefreshToken(token) {
        const decoded = jwt.decode(token);
        if (decoded?.jti) {
            await this.redis.del(`refresh:${decoded.jti}`);
        }
    }

    async revokeAllUserTokens(userId) {
        // Scan and delete all refresh tokens for user
        let cursor = 0;
        do {
            const result = await this.redis.scan(cursor, { MATCH: 'refresh:*', COUNT: 100 });
            cursor = result.cursor;
            for (const key of result.keys) {
                const hash = await this.redis.get(key);
                if (hash) {
                    // Check if token belongs to user
                    try {
                        const token = jwt.verify(hash, this.refreshTokenSecret);
                        if (token.sub === userId) {
                            await this.redis.del(key);
                        }
                    } catch { /* skip invalid */ }
                }
            }
        } while (cursor !== 0);
    }
}
```

## JWT Security Best Practices

```javascript
// 1. Use asymmetric keys for distributed systems
import { generateKeyPairSync } from 'node:crypto';

const { publicKey, privateKey } = generateKeyPairSync('rsa', {
    modulusLength: 2048,
});

// Sign with private key
const token = jwt.sign(payload, privateKey, { algorithm: 'RS256' });

// Verify with public key (can be shared with other services)
const decoded = jwt.verify(token, publicKey, { algorithms: ['RS256'] });

// 2. Validate all claims
function verifyAccessToken(token) {
    return jwt.verify(token, process.env.JWT_SECRET, {
        algorithms: ['HS256'],           // Only allow expected algorithm
        issuer: 'myapp',                 // Validate issuer
        audience: 'myapp-api',           // Validate audience
        clockTolerance: 30,              // 30s clock skew tolerance
    });
}

// 3. Token denylist for immediate revocation
class TokenDenylist {
    constructor(redis) {
        this.redis = redis;
    }

    async add(token) {
        const decoded = jwt.decode(token);
        const ttl = decoded.exp - Math.floor(Date.now() / 1000);
        if (ttl > 0) {
            await this.redis.set(`denylist:${token}`, '1', { EX: ttl });
        }
    }

    async isRevoked(token) {
        return !!(await this.redis.get(`denylist:${token}`));
    }
}

// 4. Middleware with denylist check
function authMiddleware(tokenService, denylist) {
    return async (req, res, next) => {
        const authHeader = req.headers.authorization;
        if (!authHeader?.startsWith('Bearer ')) {
            return res.status(401).json({ error: 'No token provided' });
        }

        const token = authHeader.slice(7);

        try {
            if (await denylist.isRevoked(token)) {
                return res.status(401).json({ error: 'Token revoked' });
            }

            const decoded = tokenService.verifyAccessToken(token);
            req.user = decoded;
            next();
        } catch (err) {
            return res.status(401).json({ error: 'Invalid token' });
        }
    };
}
```

## JWT Performance Optimization

```
JWT Performance Benchmarks:
─────────────────────────────────────────────
Algorithm    Sign(ms)  Verify(ms)  Token Size
─────────────────────────────────────────────
HS256        0.05      0.05        ~200 bytes
HS384        0.06      0.06        ~280 bytes
HS512        0.06      0.06        ~340 bytes
RS256        1.2       0.08        ~400 bytes
RS384        1.3       0.09        ~500 bytes
RS512        1.3       0.09        ~550 bytes
ES256        0.8       0.4         ~250 bytes
EdDSA        0.3       0.2         ~200 bytes
```

```javascript
// Cache public key verification results
class CachedTokenVerifier {
    constructor(secret, redis) {
        this.secret = secret;
        this.redis = redis;
    }

    async verify(token) {
        // Check denylist
        const denied = await this.redis.get(`deny:${token}`);
        if (denied) throw new Error('Token denied');

        // Verify signature (CPU-bound, no cache needed — fast)
        return jwt.verify(token, this.secret, {
            algorithms: ['HS256'],
            issuer: 'myapp',
        });
    }
}
```

## JWT Debugging

```javascript
function debugToken(token) {
    const [headerB64, payloadB64, signatureB64] = token.split('.');

    const header = JSON.parse(Buffer.from(headerB64, 'base64url').toString());
    const payload = JSON.parse(Buffer.from(payloadB64, 'base64url').toString());

    const now = Math.floor(Date.now() / 1000);

    return {
        header,
        payload,
        signature: signatureB64.slice(0, 16) + '...',
        size: token.length,
        isExpired: payload.exp ? payload.exp < now : false,
        expiresIn: payload.exp ? `${payload.exp - now}s` : 'never',
        issuedAt: payload.iat ? new Date(payload.iat * 1000).toISOString() : null,
        expiresAt: payload.exp ? new Date(payload.exp * 1000).toISOString() : null,
    };
}

// Common JWT errors and solutions:
// TokenExpiredError → Use refresh token flow
// JsonWebTokenError → Check secret, algorithm, token format
// NotBeforeError → Token not yet valid (nbf claim)
// invalid signature → Secret mismatch or token tampered
```

## Best Practices Checklist

- [ ] Use short-lived access tokens (15 minutes)
- [ ] Use long-lived refresh tokens with rotation
- [ ] Store refresh token hashes, not tokens
- [ ] Implement token denylist for immediate revocation
- [ ] Use RS256/ES256 for microservices, HS256 for monolith
- [ ] Validate all claims (iss, aud, exp, nbf)
- [ ] Use asymmetric keys when services verify independently
- [ ] Rotate refresh tokens on each use

## Cross-References

- See [JWT Basics](./01-jwt-basics.md) for JWT fundamentals
- See [Security](../06-authentication-security/01-security-headers.md) for hardening
- See [OAuth2](../04-oauth2-oidc/01-authorization-code-flow.md) for OAuth flows

## Next Steps

Continue to [OAuth 2.0 and OpenID Connect](../04-oauth2-oidc/01-authorization-code-flow.md).
