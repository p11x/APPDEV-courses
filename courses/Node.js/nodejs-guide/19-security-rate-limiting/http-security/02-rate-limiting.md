# Rate Limiting

## What You'll Learn

- How to rate-limit API endpoints with express-rate-limit
- How to configure time windows and request limits
- How to create different limits for different routes
- How to use a custom key generator (e.g., by user ID)
- How to use Redis as a rate limit store for distributed systems

## Why Rate Limit?

Without rate limiting, a single client can make thousands of requests per second, overwhelming your server. Rate limiting caps the number of requests a client can make in a time window.

> See: ../../16-caching-redis/advanced-redis/02-rate-limiting-redis.md for a Redis-based sliding window approach.

## Project Setup

```bash
npm install express express-rate-limit
```

## Basic Rate Limiter

```js
// basic-limit.js — Simple rate limiting with express-rate-limit

import express from 'express';
import rateLimit from 'express-rate-limit';

const app = express();

// Create a rate limiter middleware
const limiter = rateLimit({
  windowMs: 60 * 1000,  // 1-minute window
  max: 100,              // Max 100 requests per window per IP

  // Custom response when limit is exceeded
  handler: (req, res) => {
    res.status(429).json({
      error: 'Too many requests',
      retryAfter: Math.ceil(req.rateLimit.resetTime / 1000),
    });
  },

  // Standard headers to communicate rate limit status
  standardHeaders: true,   // Return rate limit info in RateLimit-* headers
  legacyHeaders: false,    // Disable X-RateLimit-* headers (deprecated)
});

// Apply to all routes
app.use(limiter);

app.get('/api/data', (req, res) => {
  res.json({ message: 'Here is your data' });
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Route-Specific Limits

```js
// route-limits.js — Different limits for different routes

import express from 'express';
import rateLimit from 'express-rate-limit';

const app = express();
app.use(express.json());

// Lenient limit for general API (100 per minute)
const apiLimiter = rateLimit({
  windowMs: 60 * 1000,
  max: 100,
  message: { error: 'Too many API requests' },
});

// Strict limit for authentication (5 per 15 minutes)
const authLimiter = rateLimit({
  windowMs: 15 * 60 * 1000,
  max: 5,
  message: { error: 'Too many login attempts — try again later' },
  // Skip successful requests — only count failed logins
  skipSuccessfulRequests: true,
});

// Very strict limit for expensive operations (10 per hour)
const expensiveLimiter = rateLimit({
  windowMs: 60 * 60 * 1000,
  max: 10,
  message: { error: 'Rate limit exceeded for this operation' },
});

// Apply limiters to specific routes
app.use('/api', apiLimiter);
app.post('/auth/login', authLimiter, loginHandler);
app.post('/export', expensiveLimiter, exportHandler);

function loginHandler(req, res) {
  const { username, password } = req.body;
  if (username === 'admin' && password === 'secret') {
    return res.json({ token: 'jwt-token' });
  }
  res.status(401).json({ error: 'Invalid credentials' });
}

function exportHandler(req, res) {
  // Simulate expensive operation
  res.json({ downloadUrl: '/files/export.csv' });
}

app.get('/api/health', (req, res) => {
  res.json({ status: 'ok' });
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Custom Key Generator

```js
// custom-key.js — Rate limit by user ID instead of IP

import express from 'express';
import rateLimit from 'express-rate-limit';

const app = express();

// Extract user ID from JWT or API key
function getUserKey(req) {
  // If authenticated, use user ID for per-user limits
  if (req.user?.id) {
    return `user:${req.user.id}`;
  }
  // Fall back to IP for unauthenticated requests
  return `ip:${req.ip}`;
}

const limiter = rateLimit({
  windowMs: 60 * 1000,
  max: 100,

  // Custom key generator — default is req.ip
  keyGenerator: getUserKey,
});

app.use(limiter);

// Simulated auth middleware
app.use((req, res, next) => {
  const token = req.headers.authorization?.split(' ')[1];
  if (token === 'admin-token') req.user = { id: 'admin', role: 'admin' };
  else if (token === 'user-token') req.user = { id: 'user1', role: 'user' };
  next();
});

app.get('/api/data', (req, res) => {
  res.json({
    message: 'Data',
    user: req.user?.id || 'anonymous',
    remaining: req.rateLimit.remaining,
  });
});

app.listen(3000);
```

## How It Works

### Rate Limit Headers

Express-rate-limit adds headers to every response:

```
RateLimit-Policy: 100;w=60
RateLimit-Limit: 100
RateLimit-Remaining: 97
RateLimit-Reset: 45
```

| Header | Meaning |
|--------|---------|
| `RateLimit-Limit` | Maximum requests in the window |
| `RateLimit-Remaining` | Requests left in the current window |
| `RateLimit-Reset` | Seconds until the window resets |

### The 429 Response

When the limit is exceeded, the server returns HTTP 429 (Too Many Requests). Well-behaved clients read the `Retry-After` or `RateLimit-Reset` header and wait before retrying.

## Common Mistakes

### Mistake 1: Rate Limiting Behind a Proxy

```js
// WRONG — behind nginx/CloudFlare, req.ip is always 127.0.0.1
// All clients share the same rate limit
app.use(rateLimit({ windowMs: 60000, max: 100 }));

// CORRECT — trust the proxy to set X-Forwarded-For
app.set('trust proxy', 1);
app.use(rateLimit({ windowMs: 60000, max: 100 }));
```

### Mistake 2: No Rate Limit on Auth Endpoints

```js
// WRONG — login endpoint without rate limiting
// Attacker can brute-force passwords at 1000/sec
app.post('/auth/login', loginHandler);

// CORRECT — strict limit on auth endpoints
app.post('/auth/login', rateLimit({ windowMs: 900000, max: 5 }), loginHandler);
```

### Mistake 3: Rate Limiting Static Assets

```js
// WRONG — rate limiting CSS, images, and JS files
app.use(rateLimit({ windowMs: 60000, max: 100 }));
app.use(express.static('public'));  // Static files are limited too

// CORRECT — only rate limit API routes
app.use('/api', rateLimit({ windowMs: 60000, max: 100 }));
app.use(express.static('public'));
```

## Try It Yourself

### Exercise 1: Test Rate Limits

Start the server. Use `curl` in a loop to hit `/api/data` 101 times. Verify that the 101st request gets a 429 response.

### Exercise 2: Different Limits by Role

Implement rate limiting where free users get 10 requests/minute and premium users get 1000 requests/minute.

### Exercise 3: Sliding Window

Research `express-rate-limit` with the `@upstash/ratelimit` store for a sliding window algorithm.

## Next Steps

You can rate limit APIs. For input validation and sanitisation, continue to [Input Sanitisation](./03-input-sanitisation.md).
