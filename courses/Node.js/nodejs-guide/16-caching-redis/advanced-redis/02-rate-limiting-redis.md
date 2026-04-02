# Rate Limiting with Redis

## What You'll Learn

- What rate limiting is and why it matters
- How to implement a sliding window rate limiter with Redis sorted sets
- How to use Lua scripts for atomic rate limit checks
- How to return rate limit headers to clients
- How to integrate rate limiting with Express middleware

## What Is Rate Limiting?

Rate limiting restricts how many requests a client can make in a given time window. It protects your API from:

- **Abuse** — malicious clients sending thousands of requests
- **Accidents** — a bug causing a loop of requests
- **Cost** — expensive operations (database, AI, email) called too often

```
Client ──→ Rate Limit Check ──→ Allowed? ──→ Process Request
                               ──→ Denied (429 Too Many Requests)
```

## Sliding Window with Sorted Sets

The most accurate rate limiting algorithm uses Redis **sorted sets**. Each request is stored with its timestamp as the score. We count requests in the window and remove expired ones.

```js
// rate-limiter.js — Sliding window rate limiter using Redis sorted sets

import Redis from 'ioredis';

const redis = new Redis();

/**
 * Check if a request is allowed under the rate limit.
 * @param {string} key - Unique identifier (e.g., user ID or IP address)
 * @param {number} maxRequests - Maximum requests allowed in the window
 * @param {number} windowMs - Time window in milliseconds
 * @returns {Promise<{ allowed: boolean, remaining: number, resetMs: number }>}
 */
async function checkRateLimit(key, maxRequests, windowMs) {
  const now = Date.now();
  const windowStart = now - windowMs;
  const redisKey = `ratelimit:${key}`;

  // Use a Redis pipeline for efficiency — sends multiple commands at once
  const pipeline = redis.pipeline();

  // 1. Remove expired entries (scores older than the window start)
  pipeline.zremrangebyscore(redisKey, 0, windowStart);

  // 2. Add the current request with the timestamp as the score
  pipeline.zadd(redisKey, now, `${now}:${Math.random()}`);

  // 3. Count the number of requests in the current window
  pipeline.zcard(redisKey);

  // 4. Set expiry on the key to auto-clean after the window passes
  pipeline.pexpire(redisKey, windowMs);

  // Execute all 4 commands atomically
  const results = await pipeline.exec();

  // results[2] is the zcard result — [error, count]
  const requestCount = results[2][1];

  const allowed = requestCount <= maxRequests;
  const remaining = Math.max(0, maxRequests - requestCount);

  // The window resets when the oldest entry expires
  const resetMs = windowStart + windowMs;

  return {
    allowed,
    remaining,
    resetMs,
    total: requestCount,
  };
}

// Test
async function main() {
  const key = 'user:1';
  const maxRequests = 5;
  const windowMs = 10_000;  // 10-second window

  for (let i = 0; i < 8; i++) {
    const result = await checkRateLimit(key, maxRequests, windowMs);
    console.log(
      `Request ${i + 1}: ${result.allowed ? 'ALLOWED' : 'DENIED'} ` +
      `(${result.remaining} remaining, ${result.total} total)`
    );
  }
}

main().finally(() => redis.disconnect());
```

### Output

```
Request 1: ALLOWED (4 remaining, 1 total)
Request 2: ALLOWED (3 remaining, 2 total)
Request 3: ALLOWED (2 remaining, 3 total)
Request 4: ALLOWED (1 remaining, 4 total)
Request 5: ALLOWED (0 remaining, 5 total)
Request 6: DENIED (0 remaining, 6 total)
Request 7: DENIED (0 remaining, 7 total)
Request 8: DENIED (0 remaining, 8 total)
```

## Express Middleware

```js
// server.js — Express API with Redis rate limiting

import express from 'express';
import Redis from 'ioredis';

const redis = new Redis();
const app = express();

// Rate limit configuration per route or tier
const RATE_LIMITS = {
  default: { max: 100, windowMs: 60_000 },    // 100 requests per minute
  api: { max: 30, windowMs: 60_000 },          // 30 requests per minute
  auth: { max: 5, windowMs: 60_000 },          // 5 login attempts per minute
};

/**
 * Express middleware that enforces rate limits.
 * @param {string} limitType - Key in RATE_LIMITS config
 */
function rateLimiter(limitType = 'default') {
  return async (req, res, next) => {
    const { max, windowMs } = RATE_LIMITS[limitType];

    // Identify the client — use IP address or authenticated user ID
    const clientId = req.ip || 'unknown';
    const key = `${limitType}:${clientId}`;

    const now = Date.now();
    const windowStart = now - windowMs;
    const redisKey = `ratelimit:${key}`;

    // Atomic rate limit check using a Lua script
    // Lua scripts run as a single atomic operation in Redis
    const result = await redis.eval(
      `
      local key = KEYS[1]
      local now = tonumber(ARGV[1])
      local windowStart = tonumber(ARGV[2])
      local maxRequests = tonumber(ARGV[3])
      local windowMs = tonumber(ARGV[4])

      -- Remove expired entries
      redis.call('ZREMRANGEBYSCORE', key, 0, windowStart)

      -- Count current requests
      local count = redis.call('ZCARD', key)

      if count < maxRequests then
        -- Allow: add this request
        redis.call('ZADD', key, now, now .. ':' .. math.random())
        redis.call('PEXPIRE', key, windowMs)
        return {1, count + 1, maxRequests - count - 1}
      else
        -- Deny: do not add, return current count
        return {0, count, 0}
      end
      `,
      1,                    // Number of KEYS arguments
      redisKey,             // KEYS[1]
      String(now),          // ARGV[1]
      String(windowStart),  // ARGV[2]
      String(max),          // ARGV[3]
      String(windowMs)      // ARGV[4]
    );

    const [allowed, total, remaining] = result;

    // Set standard rate limit headers (draft-ietf-httpapi-ratelimit-headers)
    res.set('X-RateLimit-Limit', String(max));
    res.set('X-RateLimit-Remaining', String(remaining));
    res.set('X-RateLimit-Reset', String(Math.ceil((now + windowMs) / 1000)));

    if (allowed === 1) {
      next();  // Request is allowed
    } else {
      // 429 Too Many Requests
      res.set('Retry-After', String(Math.ceil(windowMs / 1000)));
      res.status(429).json({
        error: 'Too many requests',
        retryAfter: Math.ceil(windowMs / 1000),
      });
    }
  };
}

// Apply rate limiting to routes
app.get('/api/data', rateLimiter('api'), (req, res) => {
  res.json({ data: 'Here is your data' });
});

app.post('/auth/login', rateLimiter('auth'), (req, res) => {
  res.json({ token: 'jwt-token-here' });
});

// Default rate limit for all other routes
app.use(rateLimiter('default'));

app.get('/', (req, res) => {
  res.json({ message: 'Hello' });
});

app.listen(3000, () => {
  console.log('Server with rate limiting on http://localhost:3000');
});
```

## How It Works

### The Sorted Set Approach

```
Key: ratelimit:api:127.0.0.1
Members (timestamp:random):          Scores:
  "1705312200000:0.123"              1705312200000
  "1705312201500:0.456"              1705312201500
  "1705312203000:0.789"              1705312203000
```

1. **ZREMRANGEBYSCORE** removes entries older than the window
2. **ZCARD** counts remaining entries
3. If count < max, **ZADD** adds the current request
4. **PEXPIRE** auto-deletes the key after the window

### Why Lua Scripts?

Without Lua, the check and add are separate commands. Between them, another request could sneak in. Lua scripts run **atomically** — no other Redis command can interleave.

## Common Mistakes

### Mistake 1: Using Fixed Windows Instead of Sliding

```js
// WRONG — fixed window allows bursts at the boundary
// Window: [0:00 - 1:00] allows 100 requests
// At 0:59, client sends 100 requests
// At 1:01, client sends 100 more — total 200 in 2 seconds!

// CORRECT — sliding window uses sorted sets for per-request timestamps
// Each request is individually tracked — no boundary burst problem
```

### Mistake 2: Not Returning Retry-After Header

```js
// WRONG — client does not know when to retry
res.status(429).json({ error: 'Too many requests' });

// CORRECT — tell the client when to retry
res.set('Retry-After', String(Math.ceil(windowMs / 1000)));
res.status(429).json({ error: 'Too many requests', retryAfter: seconds });
```

### Mistake 3: Using req.ip Without Trust Proxy

```js
// WRONG — behind a reverse proxy, req.ip is always '127.0.0.1'
// All users share the same rate limit!

// CORRECT — trust the proxy and use X-Forwarded-For
app.set('trust proxy', 1);  // Trust first proxy
const clientId = req.ip;     // Now correctly reflects the client IP
```

## Try It Yourself

### Exercise 1: Per-User Rate Limiting

Modify the middleware to use the authenticated user ID instead of IP. Unauthenticated requests fall back to IP-based limiting.

### Exercise 2: Tiered Limits

Implement three tiers: free (10/min), basic (100/min), premium (1000/min). The tier is read from `req.user.tier`.

### Exercise 3: Distributed Test

Start 4 instances of the server with cluster (Chapter 12). Send requests from multiple terminals. Verify the rate limit is shared across all instances via Redis.

## Next Steps

You understand Redis rate limiting. For message queuing, continue to [Chapter 17: Message Queues](../../17-message-queues/bullmq/01-bullmq-setup.md).
