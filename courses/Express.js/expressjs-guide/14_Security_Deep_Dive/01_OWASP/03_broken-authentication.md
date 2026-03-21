# Broken Authentication

## 📌 What You'll Learn

- Session fixation
- Weak JWT secrets
- Brute force protection

## 💻 Code Example

```js
import rateLimit from 'express-rate-limit';

const loginLimiter = rateLimit({
  windowMs: 15 * 60 * 1000,
  max: 5
});

// Use strong JWT secrets
const JWT_SECRET = process.env.JWT_SECRET; // Must be long and random

// Session configuration
app.use(session({
  secret: process.env.SESSION_SECRET,
  resave: false,
  saveUninitialized: false,
  cookie: {
    secure: true,
    httpOnly: true,
    sameSite: 'strict',
    maxAge: 3600000
  }
}));
```
