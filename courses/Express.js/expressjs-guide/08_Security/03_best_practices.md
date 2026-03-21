# Security Best Practices in Express.js

## Comprehensive Security Guide

Building secure web applications requires attention to multiple attack vectors. This guide covers the essential security practices for Express.js applications.

## Essential Security Middleware

### Install Security Packages

```bash
npm install helmet cors express-rate-limit
```

### Configure Security Middleware

```javascript
// server.js
import express from 'express';
import helmet from 'helmet';
import cors from 'cors';
import rateLimit from 'express-rate-limit';

const app = express();

// ============================================
// Security Middleware Table
// ============================================
// | Middleware         | Purpose                              |
// |--------------------|--------------------------------------|
// | helmet            | Sets security HTTP headers           |
// | cors              | Controls cross-origin requests       |
// | rateLimit         | Prevents brute force attacks         |
// | express.json      | Parse JSON safely                    |
// | express.urlencoded| Parse form data safely               |
// ============================================

// 1. Helmet - Sets various HTTP headers for security
// - X-Content-Type-Options: nosniff
// - X-Frame-Options: SAMEORIGIN
// - X-XSS-Protection
// - Strict-Transport-Security
app.use(helmet());

// 2. CORS - Control who can access your API
app.use(cors({
    origin: process.env.ALLOWED_ORIGIN || 'https://yourdomain.com',
    methods: ['GET', 'POST', 'PUT', 'DELETE'],
    allowedHeaders: ['Content-Type', 'Authorization']
}));

// 3. Rate Limiting - Prevent abuse
const limiter = rateLimit({
    // Time window: 15 minutes
    windowMs: 15 * 60 * 1000,
    // Limit each IP to 100 requests per window
    max: 100,
    message: {
        error: 'Too many requests, please try again later'
    }
});
app.use(limiter);

// 4. Body Parsing - Limit request body size
app.use(express.json({ limit: '10kb' }));  // Max 10KB JSON
app.use(express.urlencoded({ extended: true, limit: '10kb' }));

// Routes
app.get('/', (req, res) => res.json({ message: 'Secure API!' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Common Security Vulnerabilities

### 1. SQL Injection Prevention

**SQL Injection** occurs when malicious SQL is inserted into queries.

```javascript
// ❌ DANGEROUS - Never do this!
const query = `SELECT * FROM users WHERE email = '${req.body.email}'`;

// ✅ SAFE - Use parameterized queries
// Database libraries escape input automatically
const result = await pool.query(
    'SELECT * FROM users WHERE email = $1',
    [req.body.email]
);
```

### 2. NoSQL Injection Prevention

```javascript
// ❌ DANGEROUS - Pass user input directly to query
const user = await User.find({ email: req.body.email });

// ✅ SAFE - Validate and sanitize input
import { body, validationResult } from 'express-validator';

app.post('/login', [
    body('email').isEmail().normalizeEmail(),
    body('password').isLength({ min: 6 })
], async (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
        return res.status(400).json({ errors: errors.array() });
    }
    
    // Now safe to use
    const user = await User.findOne({ email: req.body.email });
});
```

### 3. XSS (Cross-Site Scripting) Prevention

**XSS** injects malicious scripts into web pages.

```javascript
// Escape output in templates
// EJS - use <%= %> (escapes by default)
<p><%= userInput %></p>

// NOT <%- %> which outputs raw HTML
```

### 4. CSRF (Cross-Site Request Forgery) Prevention

**CSRF** tricks users into performing unwanted actions.

```bash
npm install csurf cookie-parser
```

```javascript
import cookieParser from 'cookie-parser';
import csrf from 'csurf';

app.use(cookieParser());

// Create CSRF token middleware
const csrfProtection = csrf({ cookie: true });

// Generate CSRF token for forms
app.get('/form', csrfProtection, (req, res) => {
    res.json({ csrfToken: req.csrfToken() });
});

// Protect POST/PUT/DELETE routes
app.post('/submit', csrfProtection, (req, res) => {
    // CSRF token is validated automatically
    res.json({ message: 'Form submitted!' });
});
```

### 5. Secure Headers

```javascript
// Custom security headers with helmet
app.use(helmet.contentSecurityPolicy({
    directives: {
        defaultSrc: ["'self'"],
        scriptSrc: ["'self'", "'trusted-cdn.com'"],
        styleSrc: ["'self'", "'unsafe-inline'"],
        imgSrc: ["'self'", "data:", "trusted-images.com"]
    }
}));

app.use(helmet.hsts({
    maxAge: 31536000, // 1 year
    includeSubDomains: true,
    preload: true
}));
```

### 6. Secure Password Storage

```javascript
// ALWAYS hash passwords!
import bcrypt from 'bcryptjs';

// Hash password with salt
const salt = await bcrypt.genSalt(12);
const hashedPassword = await bcrypt.hash(password, salt);

// Verify password
const isMatch = await bcrypt.compare(password, hashedPassword);

// NEVER store plain text passwords!
```

### 7. JWT Security

```javascript
import jwt from 'jsonwebtoken';

// Use strong secret from environment
const JWT_SECRET = process.env.JWT_SECRET; // Must be long and random!

// Set short expiration
const token = jwt.sign({ userId: user.id }, JWT_SECRET, {
    expiresIn: '15m' // Short-lived access tokens
});

// Use refresh tokens for longer sessions
// Store refresh token in HttpOnly cookie
res.cookie('refreshToken', refreshToken, {
    httpOnly: true,
    secure: true, // HTTPS only
    sameSite: 'strict'
});
```

## Environment Variables

Never hardcode secrets!

```bash
# .env file - keep out of version control!
PORT=3000
DB_HOST=localhost
DB_USER=myuser
DB_PASSWORD=supersecretpassword
JWT_SECRET=verylongrandomstring123456789
SESSION_SECRET=anothersecret
```

```javascript
// Access in code
const PORT = process.env.PORT;
const JWT_SECRET = process.env.JWT_SECRET;

// process.env keeps secrets out of source code
// Different values for dev vs production
// Easy to rotate secrets without code changes
```

## Security Checklist

| Practice | Status |
|----------|--------|
| Use HTTPS in production | ☐ |
| Hash all passwords with bcrypt | ☐ |
| Use parameterized queries | ☐ |
| Validate and sanitize input | ☐ |
| Use rate limiting | ☐ |
| Use Helmet for security headers | ☐ |
| Implement CORS properly | ☐ |
| Store secrets in environment variables | ☐ |
| Use short-lived JWT tokens | ☐ |
| Log security events | ☐ |
| Keep dependencies updated | ☐ |
| Use HttpOnly cookies for tokens | ☐ |

## Logging Security Events

```javascript
// middleware/securityLogger.js
export const securityLogger = (req, res, next) => {
    // Log suspicious requests
    const suspiciousPatterns = ['../', '..\\', '<script>', 'UNION SELECT'];
    const isSuspicious = suspiciousPatterns.some(
        pattern => req.url.includes(pattern) || 
                   JSON.stringify(req.body).includes(pattern)
    );
    
    if (isSuspicious) {
        console.warn(`[SECURITY] Suspicious request from ${req.ip}:`, {
            method: req.method,
            path: req.path,
            body: req.body
        });
    }
    
    next();
};

app.use(securityLogger);
```

## What's Next?

- **[Testing](../09_Testing/01_unit_testing.md)** — Testing your Express app
- **[Deployment](../10_Advanced_Topics/01_deployment.md)** — Deploying to production
