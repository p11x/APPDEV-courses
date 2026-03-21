# Security Best Practices

## 📌 What You'll Learn
- Essential security middleware for Express
- How to protect against common attacks
- Security headers with Helmet
- Input validation and sanitization
- Rate limiting to prevent abuse

## 🧠 Concept Explained (Plain English)

Building a web application is like building a house. You need to lock the doors and windows, install security cameras, and be careful about who you let in. Similarly, web applications need security measures to protect against attackers.

The most common threats include:
- **XSS (Cross-Site Scripting)**: Attackers inject malicious scripts into your pages
- **CSRF (Cross-Site Request Forgery)**: Trick users into making unwanted requests
- **SQL Injection**: Attackers manipulate database queries
- **Brute Force**: Automated guessing of passwords
- **DDoS**: Overwhelming your server with requests

Express applications need several layers of protection:
- **Helmet**: Sets security HTTP headers
- **CORS**: Controls who can access your API
- **Rate Limiting**: Limits how many requests one user can make
- **Input Validation**: Ensures data is safe to process
- **HTTPS**: Encrypts data in transit

## 💻 Code Example

```javascript
// ES Module - Security Best Practices in Express

import express from 'express';
import helmet from 'helmet';
import cors from 'cors';
import expressRateLimit from 'express-rate-limit';

// ========================================
// WHY USE process.env?
// ========================================

/*
Environment variables are used for sensitive configuration because:
1. They keep secrets out of your source code
2. Different environments can have different values (dev vs production)
3. They don't get committed to version control
4. They can be changed without modifying code

Common examples:
- Database connection strings
- API keys and secrets
- Port numbers
- Feature flags
*/

const app = express();

// ========================================
// 1. HELMET - SECURITY HEADERS
// ========================================

/*
Helmet sets various HTTP headers to protect against common web vulnerabilities.
It doesn't secure everything, but it helps with many attacks.
*/
app.use(helmet());

// ========================================
// 2. CORS - CROSS-ORIGIN RESOURCE SHARING
// ========================================

/*
CORS controls which websites can access your API.
Without CORS, browsers block requests from different domains.
*/

// Simple CORS - allow all (for development)
app.use(cors());

// More restrictive CORS (for production):
/*
app.use(cors({
    origin: 'https://your-frontend.com', // Only allow this domain
    methods: ['GET', 'POST'], // Only allow these methods
    allowedHeaders: ['Content-Type', 'Authorization'] // Only allow these headers
}));
*/

// ========================================
// 3. RATE LIMITING - PREVENT ABUSE
// ========================================

/*
Rate limiting prevents users from making too many requests.
This protects against brute force attacks and DDoS.
*/

const limiter = expressRateLimit({
    // Time window (in milliseconds)
    windowMs: 15 * 60 * 1000, // 15 minutes
    
    // Maximum requests per window
    max: 100, // 100 requests per 15 minutes
    
    // Message when limit exceeded
    message: {
        error: 'Too many requests, please try again later.'
    },
    
    // Standard headers
    standardHeaders: true,
    legacyHeaders: false
});

// Apply rate limiting to all routes
app.use(limiter);

// Different limits for different routes:
const strictLimiter = expressRateLimit({
    windowMs: 15 * 60 * 1000,
    max: 5, // Only 5 attempts
    message: { error: 'Too many login attempts' }
});

// ========================================
// 4. INPUT VALIDATION
// ========================================

/*
Always validate and sanitize user input!
Never trust data from the client.
*/

app.use(express.json({ limit: '10kb' })); // Limit body size

// Simple validation example
const validateUserInput = (req, res, next) => {
    const { username, email, age } = req.body;
    
    const errors = [];
    
    // Validate username
    if (!username || username.length < 3) {
        errors.push('Username must be at least 3 characters');
    }
    
    // Validate email
    if (!email || !email.includes('@')) {
        errors.push('Valid email is required');
    }
    
    // Validate age (optional field)
    if (age !== undefined && (isNaN(age) || age < 0 || age > 150)) {
        errors.push('Age must be a valid number');
    }
    
    if (errors.length > 0) {
        return res.status(400).json({ errors });
    }
    
    next();
};

// ========================================
// 5. SANITIZE INPUT
// ========================================

// In a real app, use a library like express-validator or Joi
// to properly sanitize and validate input

// ========================================
// EXAMPLE ROUTES WITH SECURITY
// ========================================

app.get('/', (req, res) => {
    res.json({ message: 'Welcome!' });
});

// Route with rate limiting (strict)
app.post('/login', strictLimiter, (req, res) => {
    // Rate limited - prevents brute force
    const { username, password } = req.body;
    // ... authentication logic
    res.json({ message: 'Login endpoint' });
});

// Route with input validation
app.post('/register', validateUserInput, (req, res) => {
    // Input already validated
    const { username, email, age } = req.body;
    res.json({ message: 'User registered!' });
});

// ========================================
// 6. SECURITY HEADERS (MANUAL)
// ========================================

// You can also set custom headers manually
app.use((req, res, next) => {
    // Prevent clickjacking
    res.setHeader('X-Frame-Options', 'DENY');
    
    // Prevent XSS
    res.setHeader('X-XSS-Protection', '1; mode=block');
    
    // Force HTTPS
    res.setHeader('Strict-Transport-Security', 'max-age=31536000; includeSubDomains');
    
    // Content Type sniffing protection
    res.setHeader('X-Content-Type-Options', 'nosniff');
    
    next();
});

// ========================================
// ERROR HANDLING
// ========================================

// eslint-disable-next-line no-unused-vars
app.use((err, req, res, next) => {
    // Don't leak error details in production!
    console.error(err.stack);
    
    res.status(500).json({
        error: process.env.NODE_ENV === 'production' 
            ? 'Internal server error' 
            : err.message
    });
});

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Secure server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 25 | `import helmet from 'helmet';` | Import Helmet for security headers |
| 26 | `import cors from 'cors';` | Import CORS for cross-origin requests |
| 27 | `import expressRateLimit from 'express-rate-limit';` | Import rate limiter |
| 58 | `app.use(helmet());` | Apply Helmet middleware to all requests |
| 79 | `app.use(cors());` | Enable CORS for all origins |
| 98 | `app.use(limiter);` | Apply rate limiting to all routes |
| 110 | `const strictLimiter = expressRateLimit({...});` | Create stricter rate limiter |
| 124 | `app.use(express.json({ limit: '10kb' }));` | Limit request body size |
| 129 | `const validateUserInput = (req, res, next) => {...}` | Create validation middleware |
| 183 | `res.setHeader('X-Frame-Options', 'DENY');` | Set security header manually |

## ⚠️ Common Mistakes

**1. Not using HTTPS**
Always use HTTPS in production. It encrypts all data between client and server.

**2. Exposing error details in production**
Don't show stack traces or error details to users in production - it helps attackers!

**3. Not validating user input**
Always validate and sanitize input. Never trust the client!

**4. No rate limiting**
Without rate limiting, attackers can flood your server or guess passwords easily.

**5. Using default configurations**
Security middleware often has default settings for development. Review and adjust for production.

## ✅ Quick Recap

- Use **Helmet** to set security HTTP headers
- Use **CORS** to control who can access your API
- Use **Rate Limiting** to prevent abuse
- Always **validate and sanitize** user input
- Use **HTTPS** in production
- Don't expose error details in production
- Set **request size limits** to prevent large attacks

## 🔗 What's Next

Now let's look at integrating databases with Express. We'll explore different database options and how to connect to them.
