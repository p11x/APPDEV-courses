# 🔒 JavaScript Security Deep Dive

## Comprehensive Security Guide

---

## Table of Contents

1. [XSS Prevention](#xss-prevention)
2. [CSRF Protection](#csrf-protection)
3. [SQL Injection](#sql-injection)
4. [Authentication](#authentication)
5. [Secure Headers](#secure-headers)

---

## XSS Prevention

### Types of XSS

```javascript
// Reflected XSS
// URL: https://example.com/search?q=<script>alert(1)</script>

// Stored XSS
// Malicious user stores: <script>stealCookies()</script>

// DOM-based XSS
// JavaScript: element.innerHTML = userInput;
```

### Prevention Techniques

```javascript
// Encode HTML
function escapeHtml(text) {
  const map = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
    '"': '&quot;',
    "'": '&#x27;',
    '/': '&#x2F;'
  };
  return text.replace(/[&<>"'/]/g, char => map[char]);
}

// Use textContent instead of innerHTML
element.textContent = userInput; // Safe
element.innerHTML = userInput;   // Dangerous
```

### Content Security Policy

```html
<meta http-equiv="Content-Security-Policy" 
      content="default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline';">
```

---

## CSRF Protection

### CSRF Tokens

```javascript
// Generate token
function generateCSRFToken() {
  return crypto.randomBytes(32).toString('hex');
}

// Verify token
function verifyCSRFToken(token, sessionToken) {
  return token === sessionToken && token.length === 64;
}
```

### SameSite Cookies

```javascript
// Set cookie with SameSite
document.cookie = 'session=abc123; SameSite=Strict; Secure; HttpOnly';
```

---

## SQL Injection

### Prevention

```javascript
// ❌ Dangerous - SQL injection vulnerable
const query = `SELECT * FROM users WHERE name = '${userInput}'`;

// ✅ Safe - Parameterized query
const query = 'SELECT * FROM users WHERE name = ?';
database.prepare(query).get(userInput);

// ✅ Safe - ORM
const user = await User.findOne({ where: { name: userInput } });
```

---

## Authentication

### Password Hashing

```javascript
import bcrypt from 'bcrypt';

// Hash password
async function hashPassword(password) {
  return await bcrypt.hash(password, 12);
}

// Verify password
async function verifyPassword(password, hash) {
  return await bcrypt.compare(password, hash);
}
```

### JWT Security

```javascript
import jwt from 'jsonwebtoken';

function createToken(user) {
  return jwt.sign(
    { userId: user.id, email: user.email },
    process.env.JWT_SECRET,
    { expiresIn: '1h' }
  );
}

function verifyToken(token) {
  return jwt.verify(token, process.env.JWT_SECRET);
}
```

---

## Secure Headers

### Required Headers

```javascript
const securityHeaders = {
  'X-Content-Type-Options': 'nosniff',
  'X-Frame-Options': 'DENY',
  'X-XSS-Protection': '1; mode=block',
  'Strict-Transport-Security': 'max-age=31536000; includeSubDomains',
  'Content-Security-Policy': "default-src 'self'",
  'Referrer-Policy': 'strict-origin-when-cross-origin'
};
```

---

## Summary

### Key Takeaways

1. **XSS**: Escape output
2. **CSRF**: Use tokens
3. **SQL**: Parameterize queries

### Security Checklist

- Input validation
- Output encoding
- Secure headers
- Authentication
- HTTPS only

---

*Last updated: 2024*