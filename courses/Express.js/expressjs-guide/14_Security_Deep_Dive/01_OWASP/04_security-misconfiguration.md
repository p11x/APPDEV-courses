# Security Misconfiguration

## 📌 What You'll Learn

- Default credentials
- Verbose errors in production
- Directory listing

## 💻 Code Example

```js
import helmet from 'helmet';

// Security headers
app.use(helmet());

// Don't expose stack traces in production
app.use((err, req, res, next) => {
  if (process.env.NODE_ENV === 'production') {
    res.status(500).json({ error: 'Internal server error' });
  } else {
    res.status(500).json({ error: err.message, stack: err.stack });
  }
});

// Disable x-powered-by
app.disable('x-powered-by');

// CORS configuration
import cors from 'cors';
app.use(cors({
  origin: process.env.ALLOWED_ORIGINS?.split(',') || []
}));
```
