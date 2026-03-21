# Authentication Middleware

## What You'll Build In This File

A reusable JWT authentication middleware that protects routes by verifying the token and attaching user info to the request.

## Complete Auth Middleware

Create `src/middleware/auth.js`:

```javascript
// src/middleware/auth.js - JWT authentication middleware
// Protects routes by verifying JWT tokens
// Adds req.user to downstream handlers

import jwt from 'jsonwebtoken';
import { config } from '../config/index.js';

/**
 * Express middleware to authenticate JWT tokens
 * 
 * Usage:
 * router.get('/protected', authenticate, (req, res) => {
 *   // req.user is available here
 *   res.json({ user: req.user });
 * });
 * 
 * If authentication fails, returns 401 with error message
 */
export function authenticate(req, res, next) {
  // 1. Get the Authorization header
  const authHeader = req.headers.authorization;
  
  // Check if header exists and starts with "Bearer "
  if (!authHeader || !authHeader.startsWith('Bearer ')) {
    return res.status(401).json({
      error: 'Unauthorized',
      message: 'Missing or invalid authorization header'
    });
  }
  
  // 2. Extract the token from "Bearer <token>"
  const token = authHeader.substring(7);  // Remove "Bearer " prefix
  
  try {
    // 3. Verify and decode the JWT
    // This throws if token is invalid or expired
    const decoded = jwt.verify(token, config.jwt.secret);
    
    // 4. Attach user info to request object
    // Downstream handlers can access req.user
    req.user = {
      userId: decoded.userId,
      email: decoded.email
    };
    
    // 5. Continue to the next middleware/handler
    next();
    
  } catch (error) {
    // 6. Handle different JWT errors
    if (error.name === 'TokenExpiredError') {
      return res.status(401).json({
        error: 'Unauthorized',
        message: 'Token has expired'
      });
    }
    
    if (error.name === 'JsonWebTokenError') {
      return res.status(401).json({
        error: 'Unauthorized',
        message: 'Invalid token'
      });
    }
    
    // Unexpected error
    console.error('Auth middleware error:', error);
    return res.status(500).json({
      error: 'Internal Server Error',
      message: 'Authentication failed'
    });
  }
}

/**
 * Optional authentication middleware
 * Attaches user if token is valid, but doesn't require it
 * 
 * Usage:
 * router.get('/public', optionalAuth, (req, res) => {
 *   // req.user may or may not be present
 *   // Handle both authenticated and unauthenticated cases
 * });
 */
export function optionalAuth(req, res, next) {
  const authHeader = req.headers.authorization;
  
  if (!authHeader || !authHeader.startsWith('Bearer ')) {
    // No token - that's okay, continue without user
    return next();
  }
  
  const token = authHeader.substring(7);
  
  try {
    const decoded = jwt.verify(token, config.jwt.secret);
    req.user = {
      userId: decoded.userId,
      email: decoded.email
    };
  } catch (error) {
    // Token invalid - that's okay, continue without user
    // Don't return error for optional auth
  }
  
  next();
}

export default authenticate;
```

## Using the Middleware

```javascript
// Example: Protecting routes with the middleware
import express from 'express';
import { authenticate } from '../middleware/auth.js';
import { query } from '../db/index.js';

const router = express.Router();

// Public route - no auth required
router.get('/public', (req, res) => {
  res.json({ message: 'This is public' });
});

// Protected route - requires valid JWT
router.get('/profile', authenticate, async (req, res) => {
  // req.user is available here from the middleware
  const result = await query(
    'SELECT id, email, created_at FROM users WHERE id = $1',
    [req.user.userId]
  );
  
  res.json({ user: result.rows[0] });
});

// Protected route with parameter
router.get('/secret-data', authenticate, (req, res) => {
  res.json({
    message: 'Only authenticated users can see this',
    user: req.user
  });
});
```

## How It Connects

This middleware connects to concepts from:
- [08-authentication/jwt/03-auth-middleware.md](../../../08-authentication/jwt/03-auth-middleware.md) - JWT middleware pattern
- [05-express-framework/getting-started/03-middleware.md](../../../05-express-framework/getting-started/03-middleware.md) - Express middleware pattern

## Common Mistakes

- Forgetting to call next() after successful authentication
- Not handling all JWT error types
- Not checking the Authorization header format
- Attaching sensitive data to req.user

## Try It Yourself

### Exercise 1: Test Protected Route
Register, get a token, then access a protected route with and without the token.

### Exercise 2: Add Role Checking
Create a middleware that checks for specific user roles.

### Exercise 3: Add Token Refresh
Create a refresh endpoint that issues a new token.

## Next Steps

Continue to [../../04-bookmarks-api/01-create-bookmark.md](../../04-bookmarks-api/01-create-bookmark.md) to build the bookmarks API.
