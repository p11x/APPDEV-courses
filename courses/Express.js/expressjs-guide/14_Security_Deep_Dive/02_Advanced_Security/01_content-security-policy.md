# Content Security Policy

## 📌 What You'll Learn

- Implementing CSP headers
- Nonces for scripts
- Report-only mode

## 💻 Code Example

```js
import helmet from 'helmet';

app.use(helmet.contentSecurityPolicy({
  directives: {
    defaultSrc: ["'self'"],
    scriptSrc: ["'self'", "'nonce-abc123'"],
    styleSrc: ["'self'", "'unsafe-inline'"],
    imgSrc: ["'self'", "data:", "https:"],
    connectSrc: ["'self'", "https://api.example.com"],
    fontSrc: ["'self'"]
  },
  reportOnly: false
}));
```
