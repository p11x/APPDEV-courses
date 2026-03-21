# User Login Route

## What You'll Build In This File

The POST /auth/login endpoint that authenticates users with bcrypt and returns a JWT token.

## Complete Login Route

Update `src/routes/auth.js` to add the login handler:

```javascript
// ... (register route from previous file)

// Add to auth.js - Login route

/**
 * POST /auth/login
 * Authenticate user with email and password
 * 
 * Request body:
 * {
 *   "email": "user@example.com",
 *   "password": "securePassword123"
 * }
 * 
 * Response (200):
 * {
 *   "message": "Login successful",
 *   "user": { "id": 1, "email": "user@example.com" },
 *   "token": "eyJhbGciOiJIUzI1NiIs..."
 * }
 */
router.post('/login', async (req, res) => {
  try {
    // 1. Validate request body using Zod schema
    const parsed = loginSchema.parse(req.body);
    
    const { email, password } = parsed;
    
    // 2. Find user by email
    // Use parameterized query to prevent SQL injection
    const result = await query(
      'SELECT id, email, password_hash, created_at FROM users WHERE email = $1',
      [email]
    );
    
    // 3. Check if user exists
    if (result.rows.length === 0) {
      // Use generic error message to prevent user enumeration
      // Don't reveal whether email or password is wrong
      return res.status(401).json({
        error: 'Unauthorized',
        message: 'Invalid email or password'
      });
    }
    
    const user = result.rows[0];
    
    // 4. Compare provided password with stored hash
    // bcrypt.compare is constant-time - prevents timing attacks
    const isValidPassword = await bcrypt.compare(password, user.password_hash);
    
    if (!isValidPassword) {
      // Same error message as above - don't reveal which was wrong
      return res.status(401).json({
        error: 'Unauthorized',
        message: 'Invalid email or password'
      });
    }
    
    // 5. Generate JWT token
    const token = jwt.sign(
      { userId: user.id, email: user.email },
      config.jwt.secret,
      { expiresIn: config.jwt.expiresIn }
    );
    
    // 6. Return success response with token
    res.status(200).json({
      message: 'Login successful',
      user: {
        id: user.id,
        email: user.email,
        createdAt: user.created_at
      },
      token
    });
    
  } catch (error) {
    // Handle Zod validation errors (400 Bad Request)
    if (error.name === 'ZodError') {
      return res.status(400).json({
        error: 'Validation Error',
        issues: error.issues
      });
    }
    
    // Handle other errors (500 Internal Server Error)
    console.error('Login error:', error);
    res.status(500).json({
      error: 'Internal Server Error',
      message: 'Failed to login'
    });
  }
});

/**
 * GET /auth/me
 * Get current user's profile (protected route example)
 * Requires valid JWT token
 */
router.get('/me', async (req, res) => {
  // This would use the auth middleware - see next file
  // For now, just return a message
  res.json({
    message: 'This endpoint requires authentication middleware'
  });
});

export default router;
```

## Security: Why Generic Error Messages?

Notice that we return the same error message for:
- User doesn't exist
- Wrong password

This prevents **user enumeration attacks** where attackers try many emails to see which ones exist.

## Complete auth.js Router

The full `src/routes/auth.js` should look like:

```javascript
// src/routes/auth.js - Complete authentication routes
import { Router } from 'express';
import bcrypt from 'bcrypt';
import jwt from 'jsonwebtoken';
import { query } from '../db/index.js';
import { config } from '../config/index.js';
import { registerSchema, loginSchema } from '../schemas/auth.js';

const router = Router();

router.post('/register', async (req, res) => {
  // Implementation from previous file
});

router.post('/login', async (req, res) => {
  // Implementation above
});

export default router;
```

## How It Connects

This route connects to concepts from:
- [08-authentication/bcrypt/02-compare.md](../../../08-authentication/bcrypt/02-compare.md) - Password comparison
- [08-authentication/jwt/02-sign-verify.md](../../../08-authentication/jwt/02-sign-verify.md) - JWT verification
- [05-express-framework/request-response/01-req-object.md](../../../05-express-framework/request-response/01-req-object.md) - Reading request body

## Common Mistakes

- Returning different errors for "user not found" vs "wrong password"
- Not using bcrypt.compare (vulnerable to timing attacks)
- Not handling database errors
- Leaking user existence through error messages

## Try It Yourself

### Exercise 1: Test Login
Register a user, then try logging in with wrong credentials.

### Exercise 2: Add Rate Limiting
Add rate limiting to prevent brute force attacks.

### Exercise 3: Token Refresh
Add a /auth/refresh endpoint to renew expiring tokens.

## Next Steps

Continue to [03-auth-middleware.md](./03-auth-middleware.md) to create the JWT authentication middleware.
