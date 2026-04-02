# Gateway Security

## What You'll Learn

- How to secure an API gateway
- How to implement JWT validation at the gateway
- How to handle CORS at the gateway
- How to prevent common gateway attacks

## JWT Validation

```ts
// gateway.ts — Validate JWT at the gateway level

import express from 'express';
import jwt from 'jsonwebtoken';

const app = express();

app.use('/api', (req, res, next) => {
  const token = req.headers.authorization?.replace('Bearer ', '');

  if (!token) {
    return res.status(401).json({ error: 'Missing token' });
  }

  try {
    const decoded = jwt.verify(token, process.env.JWT_SECRET!);
    req.user = decoded;

    // Forward user info to downstream services
    req.headers['x-user-id'] = decoded.userId;
    req.headers['x-user-role'] = decoded.role;

    next();
  } catch (err) {
    return res.status(401).json({ error: 'Invalid token' });
  }
});
```

## CORS at Gateway

```ts
app.use(cors({
  origin: process.env.ALLOWED_ORIGINS?.split(',') || [],
  methods: ['GET', 'POST', 'PUT', 'DELETE'],
  allowedHeaders: ['Content-Type', 'Authorization'],
  credentials: true,
  maxAge: 86400,
}));
```

## Next Steps

For event-driven architecture, continue to [Event Sourcing](../03-event-driven-architecture/01-event-sourcing.md).
