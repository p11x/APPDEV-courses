# Session Management

## What You'll Learn

- How server-side sessions work
- Session storage with Redis
- Session security best practices

## Session Setup

```bash
npm install express-session connect-redis redis
```

```js
// session.js — Session configuration

import session from 'express-session';
import { createClient } from 'redis';
import RedisStore from 'connect-redis';

const redisClient = createClient({ url: process.env.REDIS_URL });
await redisClient.connect();

app.use(session({
  store: new RedisStore({ client: redisClient, prefix: 'sess:' }),
  secret: process.env.SESSION_SECRET,
  resave: false,
  saveUninitialized: false,
  cookie: {
    secure: process.env.NODE_ENV === 'production',
    httpOnly: true,
    maxAge: 24 * 60 * 60 * 1000,  // 24 hours
    sameSite: 'lax',
  },
}));
```

## Next Steps

For multi-factor authentication, continue to [Multi-Factor Auth](../mfa/06-multi-factor-auth.md).
