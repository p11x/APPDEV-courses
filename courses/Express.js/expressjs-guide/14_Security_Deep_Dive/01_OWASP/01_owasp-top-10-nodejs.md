# OWASP Top 10 for Node.js

## 📌 What You'll Learn

- The OWASP Top 10 security vulnerabilities
- How each applies to Node.js/Express applications
- Prevention strategies

## 🧠 Concept Explained

The OWASP Top 10 is a standard awareness document about web application security. It lists the most critical security risks.

## 💻 Code Example

```js
import express from 'express';
import helmet from 'helmet';
import rateLimit from 'express-rate-limit';

const app = express();

// 1. Security Headers with Helmet
app.use(helmet());

// 2. Rate Limiting
app.use(rateLimit({
  windowMs: 15 * 60 * 1000,
  max: 100
}));

// 3. Input Validation
import { z } from 'zod';

const userSchema = z.object({
  email: z.string().email(),
  password: z.string().min(8)
});

app.post('/api/users', express.json(), (req, res) => {
  try {
    const user = userSchema.parse(req.body);
    // Process user
    res.json({ ok: true });
  } catch (e) {
    res.status(400).json({ error: 'Invalid input' });
  }
});

// 4. SQL Injection Prevention - Use parameterized queries
// const user = await db.query('SELECT * FROM users WHERE id = $1', [userId]);

// 5. XSS Prevention - Escape output
// Use template engines with auto-escaping

app.get('/health', (req, res) => res.json({ status: 'ok' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```

## ✅ Quick Recap

- Use Helmet for security headers
- Implement rate limiting
- Validate all inputs
- Use parameterized queries
- Escape outputs
