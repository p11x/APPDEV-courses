# JWT Authentication

## 📌 What You'll Learn
- What JSON Web Tokens (JWT) are
- How JWT authentication works
- How to implement JWT in Express
- Best practices for JWT security

## 🧠 Concept Explained (Plain English)

JWT (JSON Web Token) is a way to securely transmit information between parties. Think of it like an ID card:
- When you log in, the server gives you a JWT (like an ID card)
- You present this JWT with every subsequent request (like showing your ID)
- The server validates the JWT to know who you are

JWTs are especially popular for REST APIs because they're:
- **Stateless**: The server doesn't need to store session data
- **Compact**: They're relatively small and can be sent in headers
- **Secure**: They can be signed so they can't be tampered with
- **Cross-platform**: They're just JSON, so work with any language

A JWT has three parts separated by dots:
```
xxxxx.yyyyy.zzzzz
```
- Header: Describes the token type and algorithm
- Payload: Contains the claims (data) you want to store
- Signature: Verifies the token hasn't been tampered with

## 💻 Code Example

```javascript
// ES Module - JWT Authentication in Express

import express from 'express';
import crypto from 'crypto';

const app = express();

// Middleware to parse JSON bodies
app.use(express.json());

// ========================================
// JWT SECRET - WHY USE ENVIRONMENT VARIABLES?
// ========================================

/*
In production, NEVER hardcode secrets in your code!
Environment variables (process.env.JWT_SECRET) keep sensitive
data separate from your code. This allows you to:
- Have different secrets for development vs production
- Change secrets without modifying code
- Keep secrets out of version control
*/

// For this example, we'll use a default (but in production, use env var!)
const JWT_SECRET = process.env.JWT_SECRET || 'your-secret-key-change-in-production';

// ========================================
// MOCK USER DATABASE
// ========================================

const users = [
    { 
        id: 1, 
        username: 'alice', 
        password: 'password123', // In production, this would be hashed!
        role: 'user' 
    },
    { 
        id: 2, 
        username: 'admin', 
        password: 'admin123', 
        role: 'admin' 
    }
];

// ========================================
// HELPER FUNCTIONS
// ========================================

// Simple JWT creation (for demonstration - in production, use jsonwebtoken library!)
function createJWT(payload, secret) {
    // Header
    const header = {
        alg: 'HS256',
        typ: 'JWT'
    };
    
    // Encode header and payload
    const base64Header = Buffer.from(JSON.stringify(header)).toString('base64url');
    const base64Payload = Buffer.from(JSON.stringify(payload)).toString('base64url');
    
    // Create signature
    const signature = crypto
        .createHmac('sha256', secret)
        .update(`${base64Header}.${base64Payload}`)
        .digest('base64url');
    
    // Return token
    return `${base64Header}.${base64Payload}.${signature}`;
}

// Simple JWT verification
function verifyJWT(token, secret) {
    try {
        const [base64Header, base64Payload, signature] = token.split('.');
        
        // Verify signature
        const expectedSignature = crypto
            .createHmac('sha256', secret)
            .update(`${base64Header}.${base64Payload}`)
            .digest('base64url');
        
        if (signature !== expectedSignature) {
            return null; // Invalid signature
        }
        
        // Decode payload
        const payload = JSON.parse(Buffer.from(base64Payload, 'base64url').toString());
        
        // Check expiration
        if (payload.exp && Date.now() > payload.exp) {
            return null; // Token expired
        }
        
        return payload;
    } catch (error) {
        return null;
    }
}

// ========================================
// JWT AUTHENTICATION MIDDLEWARE
// ========================================

const authenticateJWT = (req, res, next) => {
    // Get token from Authorization header
    const authHeader = req.headers.authorization;
    
    if (!authHeader) {
        return res.status(401).json({ error: 'No token provided' });
    }
    
    // Format: "Bearer <token>"
    const token = authHeader.split(' ')[1];
    
    // Verify the token
    const payload = verifyJWT(token, JWT_SECRET);
    
    if (!payload) {
        return res.status(401).json({ error: 'Invalid or expired token' });
    }
    
    // Attach user info to request
    req.user = payload;
    
    next();
};

// ========================================
// AUTHORIZATION MIDDLEWARE
// ========================================

const authorize = (roles = []) => {
    return (req, res, next) => {
        if (!req.user) {
            return res.status(401).json({ error: 'Not authenticated' });
        }
        
        if (roles.length && !roles.includes(req.user.role)) {
            return res.status(403).json({ 
                error: 'Not authorized for this action' 
            });
        }
        
        next();
    };
};

// ========================================
// AUTHENTICATION ROUTES
// ========================================

// Login - generate JWT
app.post('/login', (req, res) => {
    const { username, password } = req.body;
    
    // Find user
    const user = users.find(u => u.username === username && u.password === password);
    
    if (!user) {
        return res.status(401).json({ error: 'Invalid credentials' });
    }
    
    // Create token payload
    const payload = {
        id: user.id,
        username: user.username,
        role: user.role,
        // Token expires in 1 hour
        exp: Date.now() + (60 * 60 * 1000)
    };
    
    // Generate JWT
    const token = createJWT(payload, JWT_SECRET);
    
    res.json({
        message: 'Login successful',
        token: token,
        expiresIn: 3600 // seconds
    });
});

// ========================================
// PROTECTED ROUTES
// ========================================

// Public route
app.get('/', (req, res) => {
    res.json({ message: 'Welcome to the API!' });
});

// Protected route - requires authentication
app.get('/profile', authenticateJWT, (req, res) => {
    // req.user is set by the authenticateJWT middleware
    res.json({
        user: req.user,
        message: 'This is your protected profile'
    });
});

// Admin-only route
app.get('/admin', authenticateJWT, authorize(['admin']), (req, res) => {
    res.json({
        message: 'Welcome, Admin!',
        secret: 'Admin-only data'
    });
});

// ========================================
// REFRESH TOKEN (Optional)
// ========================================

app.post('/refresh', authenticateJWT, (req, res) => {
    // Create new token with new expiration
    const payload = {
        id: req.user.id,
        username: req.user.username,
        role: req.user.role,
        exp: Date.now() + (60 * 60 * 1000) // Another hour
    };
    
    const newToken = createJWT(payload, JWT_SECRET);
    
    res.json({
        token: newToken,
        expiresIn: 3600
    });
});

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 27 | `const JWT_SECRET = process.env.JWT_SECRET \|\| ...` | Get secret from environment (with fallback for dev) |
| 54 | `createJWT(payload, secret)` | Function to create a JWT token |
| 60 | `Buffer.from(JSON.stringify(header)).toString('base64url')` | Encode header to base64 |
| 70 | `crypto.createHmac('sha256', secret).update(...)` | Create cryptographic signature |
| 95 | `const token = authHeader.split(' ')[1];` | Extract token from "Bearer <token>" |
| 100 | `verifyJWT(token, JWT_SECRET)` | Verify token is valid and not expired |
| 105 | `req.user = payload;` | Store decoded payload for use in routes |

## ⚠️ Common Mistakes

**1. Not using environment variables for secrets**
Never hardcode JWT secrets in your code. Use process.env.JWT_SECRET.

**2. Not setting token expiration**
Always set an expiration time for tokens. Without it, tokens last forever!

**3. Storing sensitive data in JWT payload**
JWT payload is encoded, not encrypted. Don't put passwords or sensitive data in it.

**4. Not handling token expiration**
Your frontend needs to handle 401 errors and potentially refresh the token.

**5. Using HTTP instead of HTTPS**
Tokens can be intercepted over HTTP. Always use HTTPS in production!

## ✅ Quick Recap

- JWT (JSON Web Token) is a compact, URL-safe way to transmit claims
- JWTs have three parts: header, payload, and signature
- The signature ensures tokens can't be tampered with
- Store JWT in Authorization header: "Bearer <token>"
- Always set token expiration
- Use environment variables for JWT secrets
- In production, use HTTPS to protect tokens

## 🔗 What's Next

Now let's look at securing Express applications with common security practices and middleware.
