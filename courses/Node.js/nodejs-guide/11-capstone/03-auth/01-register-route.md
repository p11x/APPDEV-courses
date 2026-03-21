# User Registration Route

## What You'll Build In This File

The POST /auth/register endpoint that creates new user accounts with bcrypt password hashing and returns a JWT token.

## Complete Registration Route

Create `src/routes/auth.js`:

```javascript
// src/routes/auth.js - Authentication routes
// POST /auth/register - Create new user account
// POST /auth/login - Authenticate and get JWT

import { Router } from 'express';
import bcrypt from 'bcrypt';
import jwt from 'jsonwebtoken';
import { query } from '../db/index.js';
import { config } from '../config/index.js';
import { registerSchema, loginSchema } from '../schemas/auth.js';

// Create new router instance
const router = Router();

/**
 * POST /auth/register
 * Create a new user account
 * 
 * Request body:
 * {
 *   "email": "user@example.com",
 *   "password": "securePassword123"
 * }
 * 
 * Response (201):
 * {
 *   "message": "User created successfully",
 *   "user": { "id": 1, "email": "user@example.com" },
 *   "token": "eyJhbGciOiJIUzI1NiIs..."
 * }
 */
router.post('/register', async (req, res) => {
  try {
    // 1. Validate request body using Zod schema
    // This ensures email is valid and password meets requirements
    const parsed = registerSchema.parse(req.body);
    
    const { email, password } = parsed;
    
    // 2. Check if user already exists
    // Use parameterized query to prevent SQL injection
    const existingUser = await query(
      'SELECT id FROM users WHERE email = $1',
      [email]
    );
    
    if (existingUser.rows.length > 0) {
      // Return 409 Conflict - user already exists
      return res.status(409).json({
        error: 'Conflict',
        message: 'A user with this email already exists'
      });
    }
    
    // 3. Hash the password using bcrypt
    // 10 rounds is a good balance between security and performance
    const saltRounds = 10;
    const passwordHash = await bcrypt.hash(password, saltRounds);
    
    // 4. Insert the new user into the database
    const result = await query(
      `INSERT INTO users (email, password_hash) 
       VALUES ($1, $2) 
       RETURNING id, email, created_at`,
      [email, passwordHash]
    );
    
    const user = result.rows[0];
    
    // 5. Generate JWT token for immediate login
    const token = jwt.sign(
      { userId: user.id, email: user.email },  // Payload - user data in token
      config.jwt.secret,                        // Secret key from config
      { expiresIn: config.jwt.expiresIn }      // Token expiration
    );
    
    // 6. Return success response with token
    // Status 201 = Created
    res.status(201).json({
      message: 'User created successfully',
      user: {
        id: user.id,
        email: user.email,
        createdAt: user.created_at
      },
      token  // Client can use this token immediately
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
    console.error('Register error:', error);
    res.status(500).json({
      error: 'Internal Server Error',
      message: 'Failed to create user'
    });
  }
});

/**
 * POST /auth/login
 * Authenticate user and return JWT
 * (Implemented in next file)
 */
router.post('/login', async (req, res) => {
  // See login-route.md for implementation
});

export default router;
```

## Zod Schema for Registration

Create `src/schemas/auth.js`:

```javascript
// src/schemas/auth.js - Validation schemas for authentication
// Uses Zod for schema validation (see 04-npm-and-packages/useful-packages/03-zod.md)

import { z } from 'zod';

/**
 * Registration validation schema
 * - Email must be valid format
 * - Password must be at least 8 characters
 */
export const registerSchema = z.object({
  email: z.string()
    .min(1, 'Email is required')
    .email('Invalid email format'),
  password: z.string()
    .min(8, 'Password must be at least 8 characters')
    .max(100, 'Password must be less than 100 characters')
});

/**
 * Login validation schema
 * - Same as register but no password length requirements on input
 */
export const loginSchema = z.object({
  email: z.string()
    .min(1, 'Email is required')
    .email('Invalid email format'),
  password: z.string()
    .min(1, 'Password is required')
});

/**
 * TypeScript-like inference (for documentation)
 * These types are inferred from the schemas:
 * 
 * type RegisterInput = z.infer<typeof registerSchema>;
 * // { email: string; password: string }
 */
```

## How It Connects

This route connects to concepts from:
- [05-express-framework/getting-started/02-routing.md](../../../05-express-framework/getting-started/02-routing.md) - Express routing
- [08-authentication/bcrypt/01-hashing.md](../../../08-authentication/bcrypt/01-hashing.md) - Password hashing with bcrypt
- [08-authentication/jwt/02-sign-verify.md](../../../08-authentication/jwt/02-sign-verify.md) - JWT signing
- [04-npm-and-packages/useful-packages/03-zod.md](../../../04-npm-and-packages/useful-packages/03-zod.md) - Zod validation

## Common Mistakes

- Not validating input (security vulnerability)
- Not checking if user exists before inserting
- Using plaintext passwords (huge security risk)
- Not handling unique constraint violations
- Not returning proper HTTP status codes

## Try It Yourself

### Exercise 1: Test Registration
Use curl or Postman to register a new user.

### Exercise 2: Add Email Validation
Add more email validation (e.g., reject disposable emails).

### Exercise 3: Handle Duplicate Error
Add proper handling for unique constraint violations at the database level.

## Next Steps

Continue to [02-login-route.md](./02-login-route.md) to implement the login endpoint.
