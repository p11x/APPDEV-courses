# Session Store with Redis

## What You'll Learn

- What server-side sessions are and how they work
- How to use `express-session` with `connect-redis`
- How to configure cookie options (secure, httpOnly, sameSite)
- How to implement rolling session TTL
- How to read and write session data

## What Are Sessions?

HTTP is stateless — each request knows nothing about previous requests. **Sessions** add state by storing data on the server (in Redis) and giving the browser a cookie containing a session ID.

```
Browser                    Server
  │                          │
  │── POST /login ──────────→│  (create session, set cookie)
  │←── Set-Cookie: sid=abc ──│
  │                          │
  │── GET /dashboard ───────→│  (cookie sends sid=abc)
  │     Cookie: sid=abc      │  (server looks up session in Redis)
  │←── User: Alice ──────────│
```

## Project Setup

```bash
npm install express express-session connect-redis ioredis
```

## Session Server

```js
// server.js — Express server with Redis-backed sessions

import express from 'express';
import session from 'express-session';
import { createClient } from 'redis';  // npm install redis
import RedisStore from 'connect-redis';

// Create a Redis client for sessions
const redisClient = createClient({
  url: 'redis://localhost:6379',
});

redisClient.on('error', (err) => {
  console.error('Redis session store error:', err.message);
});

await redisClient.connect();
console.log('Connected to Redis for sessions');

const app = express();

// Parse JSON request bodies
app.use(express.json());

// Configure express-session with Redis store
app.use(
  session({
    // Use Redis as the session store instead of the default in-memory store
    store: new RedisStore({
      client: redisClient,
      prefix: 'app:sess:',  // Keys will be app:sess:<session-id>
    }),

    // Secret used to sign the session cookie (prevents tampering)
    // Use a long random string in production, loaded from env variable
    secret: process.env.SESSION_SECRET || 'my-secret-key',

    // resave: false = do not save session if it was not modified during the request
    resave: false,

    // saveUninitialized: false = do not create a session until data is stored
    // This prevents empty sessions from being stored in Redis
    saveUninitialized: false,

    // Cookie configuration
    cookie: {
      secure: false,       // true = only send over HTTPS (set to true in production)
      httpOnly: true,      // true = JavaScript cannot access the cookie (prevents XSS)
      maxAge: 24 * 60 * 60 * 1000,  // 24 hours in milliseconds
      sameSite: 'lax',     // CSRF protection: 'strict', 'lax', or 'none'
    },

    // Name of the session cookie (default is 'connect.sid')
    name: 'sessionId',
  })
);

// === Routes ===

// Login — create a session
app.post('/login', (req, res) => {
  const { username, password } = req.body;

  // Simple authentication (use bcrypt in production)
  if (username === 'alice' && password === 'secret') {
    // Store data in the session — this is saved to Redis automatically
    req.session.user = {
      id: '1',
      username: 'alice',
      role: 'admin',
      loginAt: new Date().toISOString(),
    };

    res.json({ message: 'Logged in', user: req.session.user });
  } else {
    res.status(401).json({ error: 'Invalid credentials' });
  }
});

// Protected route — check session
app.get('/dashboard', (req, res) => {
  if (!req.session.user) {
    return res.status(401).json({ error: 'Not authenticated' });
  }

  res.json({
    message: `Welcome, ${req.session.user.username}!`,
    user: req.session.user,
  });
});

// Update session data
app.post('/preferences', (req, res) => {
  if (!req.session.user) {
    return res.status(401).json({ error: 'Not authenticated' });
  }

  // Add preferences to the session
  req.session.preferences = {
    theme: req.body.theme || 'dark',
    language: req.body.language || 'en',
  };

  res.json({ preferences: req.session.preferences });
});

// Get session info
app.get('/session', (req, res) => {
  res.json({
    id: req.sessionID,
    user: req.session.user || null,
    preferences: req.session.preferences || null,
    cookie: req.session.cookie,
  });
});

// Logout — destroy the session
app.post('/logout', (req, res) => {
  // destroy() removes the session from Redis and clears the cookie
  req.session.destroy((err) => {
    if (err) {
      return res.status(500).json({ error: 'Failed to logout' });
    }

    // Clear the cookie on the client side
    res.clearCookie('sessionId');
    res.json({ message: 'Logged out' });
  });
});

// Start the server
app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Testing with curl

```bash
# Login — saves cookie to cookie-jar
curl -X POST http://localhost:3000/login \
  -H "Content-Type: application/json" \
  -d '{"username":"alice","password":"secret"}' \
  -c cookies.txt

# Access protected route — sends cookie from cookie-jar
curl http://localhost:3000/dashboard -b cookies.txt

# Save preferences
curl -X POST http://localhost:3000/preferences \
  -H "Content-Type: application/json" \
  -d '{"theme":"light","language":"fr"}' \
  -b cookies.txt -c cookies.txt

# View session
curl http://localhost:3000/session -b cookies.txt

# Logout
curl -X POST http://localhost:3000/logout -b cookies.txt -c cookies.txt

# After logout — no longer authenticated
curl http://localhost:3000/dashboard -b cookies.txt
```

## Rolling Sessions

With **rolling** enabled, the session TTL resets on every request. An active user's session never expires, but an idle session expires after `maxAge`.

```js
// Add to session config
app.use(
  session({
    // ... other options
    rolling: true,  // Reset cookie maxAge on every response
    cookie: {
      maxAge: 30 * 60 * 1000,  // 30 minutes — resets with each request
    },
  })
);
```

## How It Works

### Session Storage in Redis

```
Key: app:sess:s%3Aabc123.xyz
Value: {"cookie":{"originalMaxAge":86400000,"expires":"...","httpOnly":true},
        "user":{"id":"1","username":"alice"}}
TTL: 86400 (24 hours)
```

The session data is stored as a JSON string in Redis. The cookie only contains the session ID — no user data is exposed to the browser.

### Cookie Security Options

| Option | Meaning |
|--------|---------|
| `secure: true` | Cookie only sent over HTTPS |
| `httpOnly: true` | JavaScript (`document.cookie`) cannot read the cookie |
| `sameSite: 'strict'` | Cookie not sent on cross-site requests (CSRF protection) |
| `maxAge` | Cookie lifetime in milliseconds |

## Common Mistakes

### Mistake 1: Storing Large Objects in Session

```js
// WRONG — sessions are stored in Redis (RAM); large objects waste memory
req.session.largeData = veryLargeArray;  // 50MB array stored for every request

// CORRECT — store only essential data; fetch large data from DB as needed
req.session.user = { id: '1', name: 'Alice' };
// Fetch the large data in the route handler, not in the session
```

### Mistake 2: Not Setting saveUninitialized to false

```js
// WRONG — creates a session for every visitor, even unauthenticated ones
session({ saveUninitialized: true });
// Every page visit creates a Redis key — wastes memory

// CORRECT — only create sessions when you actually store data
session({ saveUninitialized: false });
```

### Mistake 3: Hardcoding the Session Secret

```js
// WRONG — secret in source code; anyone with access to the repo can forge sessions
session({ secret: 'my-secret-key' });

// CORRECT — load from environment variable
session({ secret: process.env.SESSION_SECRET });
```

## Try It Yourself

### Exercise 1: Login Counter

Add a `loginCount` field to the session. Increment it on every login. Display it in the `/dashboard` response.

### Exercise 2: Session Expiry Warning

Add a route `GET /session-remaining` that returns the remaining TTL of the session in seconds.

### Exercise 3: Multi-Device Sessions

Login from two different browser profiles. Verify that each has a separate session ID in Redis but share the same user data.

## Next Steps

You have session management. For pub/sub messaging with Redis, continue to [Pub/Sub](../advanced-redis/01-pub-sub.md).
