# Authentication in Express.js

## What is Authentication?

**Authentication** verifies who a user is. Think of it like a login — proving you are who you claim to be.

Common authentication methods:
- **Password-based** - Email and password
- **JWT (JSON Web Tokens)** - Token-based
- **OAuth** - Third-party login (Google, GitHub, etc.)

## JSON Web Tokens (JWT)

**JWT** is a popular stateless authentication method. The server creates a signed token that the client stores and sends with each request.

### How JWT Works

1. User logs in with credentials
2. Server validates and sends a token
3. Client stores the token
4. Client sends token with each request
5. Server verifies token signature

### Install Dependencies

```bash
npm install jsonwebtoken bcryptjs dotenv
```

### Create JWT Utilities

```javascript
// utils/jwt.js
import jwt from 'jsonwebtoken';

// JWT secrets should be in environment variables!
// process.env.JWT_SECRET is the key used to sign tokens
const JWT_SECRET = process.env.JWT_SECRET || 'your-secret-key';
const JWT_EXPIRES_IN = '7d'; // Token expires in 7 days

// Generate a JWT token for a user
export const generateToken = (userId) => {
    // jwt.sign() creates a new token
    // First param: payload (data to store in token)
    // Second param: secret key
    // Third param: options
    return jwt.sign(
        { userId },
        JWT_SECRET,
        { expiresIn: JWT_EXPIRES_IN }
    );
};

// Verify a JWT token
export const verifyToken = (token) => {
    try {
        // jwt.verify() checks if token is valid
        // Returns the decoded payload if valid
        return jwt.verify(token, JWT_SECRET);
    } catch (error) {
        // Token is invalid or expired
        return null;
    }
};
```

### Hash Passwords

```javascript
// utils/password.js
import bcrypt from 'bcryptjs';

// Hash a password
// We hash passwords so even if the database is compromised,
// the actual passwords aren't exposed
export const hashPassword = async (password) => {
    // Generate salt (random data added to password)
    const salt = await bcrypt.genSalt(10);
    // Hash the password with the salt
    return bcrypt.hash(password, salt);
};

// Compare password with hash
export const comparePassword = async (password, hash) => {
    // bcrypt.compare() checks if password matches hash
    return bcrypt.compare(password, hash);
};
```

### Authentication Middleware

```javascript
// middleware/auth.js
import { verifyToken } from '../utils/jwt.js';

// Middleware to protect routes - only logged in users can access
export const authenticate = (req, res, next) => {
    // Get token from header
    // Headers are case-insensitive in Express
    const authHeader = req.headers.authorization;
    
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
        return res.status(401).json({
            error: 'No token provided'
        });
    }
    
    // Extract token from "Bearer <token>"
    const token = authHeader.split(' ')[1];
    
    // Verify the token
    const decoded = verifyToken(token);
    
    if (!decoded) {
        return res.status(401).json({
            error: 'Invalid or expired token'
        });
    }
    
    // Add user ID to request object for use in routes
    req.userId = decoded.userId;
    
    // Call next middleware
    next();
};
```

### Complete Authentication Example

```javascript
// server.js
import express from 'express';
import bcrypt from 'bcryptjs';
import { generateToken, verifyToken } from './utils/jwt.js';
import { authenticate } from './middleware/auth.js';

const app = express();
app.use(express.json());

// Mock user database
const users = [];

// ============================================
// Auth Routes Table
// ============================================
// | Method | URL          | Handler      | Description           |
// |--------|--------------|--------------|-----------------------|
// | POST   | /register    | register    | Create new account    |
// | POST   | /login       | login       | Login to account      |
// | GET    | /profile     | getProfile  | Get user profile      |
// ============================================

// POST /register - Create new user
export const register = async (req, res) => {
    try {
        const { name, email, password } = req.body;
        
        // Check if user exists
        const existingUser = users.find(u => u.email === email);
        if (existingUser) {
            return res.status(400).json({
                error: 'Email already registered'
            });
        }
        
        // Hash password (NEVER store plain text passwords!)
        const hashedPassword = await bcrypt.hash(password, 10);
        
        // Create user
        const user = {
            id: users.length + 1,
            name,
            email,
            password: hashedPassword
        };
        
        users.push(user);
        
        // Generate token
        const token = generateToken(user.id);
        
        res.status(201).json({
            message: 'User created',
            token
        });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
};

// POST /login - Login user
export const login = async (req, res) => {
    try {
        const { email, password } = req.body;
        
        // Find user
        const user = users.find(u => u.email === email);
        if (!user) {
            return res.status(401).json({
                error: 'Invalid credentials'
            });
        }
        
        // Check password
        const isMatch = await bcrypt.compare(password, user.password);
        if (!isMatch) {
            return res.status(401).json({
                error: 'Invalid credentials'
            });
        }
        
        // Generate token
        const token = generateToken(user.id);
        
        res.json({
            message: 'Login successful',
            token
        });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
};

// GET /profile - Get user profile (protected route)
// authenticate middleware protects this route
export const getProfile = async (req, res) => {
    try {
        // req.userId was set by authenticate middleware
        const user = users.find(u => u.id === req.userId);
        
        if (!user) {
            return res.status(404).json({ error: 'User not found' });
        }
        
        // Don't send password!
        res.json({
            id: user.id,
            name: user.name,
            email: user.email
        });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
};

// Register routes
app.post('/register', register);
app.post('/login', login);
app.get('/profile', authenticate, getProfile);

// Error handler
app.use((err, req, res, next) => {
    console.error(err.message);
    res.status(500).json({ error: 'Server error' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Using Authentication in Routes

```javascript
// Routes that require authentication
import { authenticate } from '../middleware/auth.js';

// All routes below require authentication
app.use('/api', authenticate);

// Now these routes are protected
app.get('/api/users', getUsers);
app.post('/api/posts', createPost);
app.delete('/api/posts/:id', deletePost);
```

## OAuth (Third-Party Login)

For Google, GitHub, etc., use Passport.js:

```bash
npm install passport passport-google-oauth20
```

```javascript
import passport from 'passport';
import { Strategy as GoogleStrategy } from 'passport-google-oauth20';

passport.use(new GoogleStrategy({
    clientID: process.env.GOOGLE_CLIENT_ID,
    clientSecret: process.env.GOOGLE_CLIENT_SECRET,
    callbackURL: '/auth/google/callback'
}, (accessToken, refreshToken, profile, done) => {
    // Find or create user with profile.id
    return done(null, user);
}));

app.get('/auth/google', passport.authenticate('google', {
    scope: ['profile', 'email']
}));

app.get('/auth/google/callback',
    passport.authenticate('google', { failureRedirect: '/login' }),
    (req, res) => res.redirect('/dashboard')
);
```

## Security Best Practices

| Practice | Why |
|----------|-----|
| Hash passwords | Protects passwords if database is compromised |
| Use HTTPS | Encrypts data in transit |
| Use environment variables for secrets | Keeps secrets out of code |
| Validate input | Prevents injection attacks |
| Rate limit login attempts | Prevents brute force attacks |
| Use HttpOnly cookies | Prevents XSS attacks on tokens |

## What's Next?

- **[Authorization](./02_authorization.md)** — Controlling what users can do
- **[Security Best Practices](./03_best_practices.md)** — Comprehensive security
