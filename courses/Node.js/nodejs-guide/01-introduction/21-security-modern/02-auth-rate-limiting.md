# Authentication, Authorization, and Rate Limiting

## What You'll Learn

- JWT authentication patterns
- Rate limiting strategies
- DDoS protection
- Session management

## JWT Authentication

```javascript
// auth/jwt.js — JWT authentication middleware

import jwt from 'jsonwebtoken';

const JWT_SECRET = process.env.JWT_SECRET;
const JWT_EXPIRES = '15m';
const REFRESH_EXPIRES = '7d';

export function generateTokens(user) {
    const accessToken = jwt.sign(
        { sub: user.id, role: user.role },
        JWT_SECRET,
        { expiresIn: JWT_EXPIRES }
    );
    
    const refreshToken = jwt.sign(
        { sub: user.id, type: 'refresh' },
        JWT_SECRET,
        { expiresIn: REFRESH_EXPIRES }
    );
    
    return { accessToken, refreshToken };
}

export function authenticate(req, res, next) {
    const auth = req.headers.authorization;
    if (!auth?.startsWith('Bearer ')) {
        return res.status(401).json({ error: 'No token' });
    }
    
    try {
        const payload = jwt.verify(auth.slice(7), JWT_SECRET);
        req.user = payload;
        next();
    } catch (err) {
        if (err.name === 'TokenExpiredError') {
            return res.status(401).json({ error: 'Token expired', code: 'TOKEN_EXPIRED' });
        }
        return res.status(401).json({ error: 'Invalid token' });
    }
}

export function authorize(...roles) {
    return (req, res, next) => {
        if (!roles.includes(req.user.role)) {
            return res.status(403).json({ error: 'Forbidden' });
        }
        next();
    };
}
```

## Rate Limiting

```javascript
// middleware/rateLimit.js — Token bucket rate limiter

class TokenBucket {
    constructor(capacity, refillRate) {
        this.capacity = capacity;
        this.tokens = capacity;
        this.refillRate = refillRate; // tokens per second
        this.lastRefill = Date.now();
    }
    
    consume(tokens = 1) {
        this.refill();
        if (this.tokens >= tokens) {
            this.tokens -= tokens;
            return true;
        }
        return false;
    }
    
    refill() {
        const now = Date.now();
        const elapsed = (now - this.lastRefill) / 1000;
        this.tokens = Math.min(this.capacity, this.tokens + elapsed * this.refillRate);
        this.lastRefill = now;
    }
}

const buckets = new Map();

export function rateLimit({ capacity = 100, refillRate = 10, keyFn } = {}) {
    return (req, res, next) => {
        const key = keyFn ? keyFn(req) : req.ip;
        
        if (!buckets.has(key)) {
            buckets.set(key, new TokenBucket(capacity, refillRate));
        }
        
        const bucket = buckets.get(key);
        
        if (!bucket.consume()) {
            return res.status(429).json({
                error: 'Too many requests',
                retryAfter: Math.ceil(1 / refillRate),
            });
        }
        
        next();
    };
}

// Usage
app.use('/api/', rateLimit({ capacity: 100, refillRate: 10 }));
app.use('/api/auth/', rateLimit({ capacity: 5, refillRate: 0.1 }));
```

## Best Practices Checklist

- [ ] Use short-lived JWT access tokens (15min)
- [ ] Implement refresh token rotation
- [ ] Rate limit all endpoints
- [ ] Stricter rate limit on auth endpoints
- [ ] Use bcrypt for password hashing (cost factor 12+)
- [ ] Implement account lockout after failed attempts

## Cross-References

- See [Security Headers](./01-security-headers-deps.md) for HTTP headers
- See [OWASP Node.js](./03-owasp-nodejs.md) for vulnerability patterns
- See [Authentication](../../../08-authentication/) for comprehensive auth guide

## Next Steps

Continue to [OWASP Node.js](./03-owasp-nodejs.md) for vulnerability patterns.
