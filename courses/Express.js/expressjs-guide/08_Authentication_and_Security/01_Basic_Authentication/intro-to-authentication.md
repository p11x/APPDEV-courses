# Introduction to Authentication

## 📌 What You'll Learn
- What authentication means in web applications
- The difference between authentication and authorization
- Common authentication methods
- How to implement basic authentication in Express

## 🧠 Concept Explained (Plain English)

**Authentication** is the process of verifying who someone is. It's like showing your ID at the door of a building - you're proving that you are who you claim to be.

**Authorization** is what happens after you're verified - determining what you're allowed to do. Once you're inside, authorization decides if you can enter certain rooms or access certain files.

Think of it like a hotel:
- **Authentication**: The front desk checks your ID and confirms you have a reservation - they verify WHO you are
- **Authorization**: Based on your room type, you get a keycard that only opens YOUR room - they determine WHAT you can access

In web applications:
- You "prove" who you are by providing credentials (username/password, token, etc.)
- The server validates these credentials
- If valid, you're "logged in" and can access protected resources

## 💻 Code Example

```javascript
// ES Module - Basic Authentication in Express

import express from 'express';

const app = express();

// Middleware to parse JSON bodies
app.use(express.json());

// ========================================
// UNDERSTANDING req, res, AND next
// ========================================

// In Express, every route handler receives these three parameters:
// - req: The request object (what the client sent)
// - res: The response object (what we'll send back)
// - next: Function to pass control to the next middleware

// ========================================
// MOCK USER DATABASE
// ========================================

const users = [
    { 
        id: 1, 
        username: 'alice', 
        password: 'password123', // In real app, this would be hashed!
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
// SIMPLE AUTHENTICATION MIDDLEWARE
// ========================================

// This middleware checks if the user is authenticated
// It's a function that takes req, res, and next
const authenticate = (req, res, next) => {
    // Check for Authorization header
    const authHeader = req.headers.authorization;
    
    if (!authHeader) {
        return res.status(401).json({ 
            error: 'No authorization header provided' 
        });
    }
    
    // Parse "Basic base64(username:password)" header
    const base64Credentials = authHeader.split(' ')[1];
    const credentials = Buffer.from(base64Credentials, 'base64').toString('utf-8');
    const [username, password] = credentials.split(':');
    
    // Find user with matching username and password
    const user = users.find(u => u.username === username && u.password === password);
    
    if (!user) {
        return res.status(401).json({ 
            error: 'Invalid credentials' 
        });
    }
    
    // Attach user to request object for use in routes
    // This is common pattern - storing authenticated user for later use
    req.user = {
        id: user.id,
        username: user.username,
        role: user.role
    };
    
    // Call next to pass control to the route handler
    next();
};

// ========================================
// AUTHORIZATION MIDDLEWARE
// ========================================

// Check if user has specific role
const authorize = (roles = []) => {
    // Return a middleware function
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
// PUBLIC ROUTES (NO AUTH REQUIRED)
// ========================================

app.get('/', (req, res) => {
    res.json({ message: 'Welcome to our API!' });
});

app.get('/public', (req, res) => {
    res.json({ message: 'This is a public endpoint' });
});

// ========================================
// PROTECTED ROUTES (AUTHENTICATION REQUIRED)
// ========================================

// Using the authenticate middleware
app.get('/protected', authenticate, (req, res) => {
    // req.user is available because authenticate middleware set it
    res.json({ 
        message: 'You have accessed a protected route',
        user: req.user 
    });
});

// ========================================
// PROTECTED ROUTES WITH SPECIFIC ROLES
// ========================================

// Using both authenticate and authorize middleware
app.get('/admin-only', authenticate, authorize(['admin']), (req, res) => {
    res.json({ 
        message: 'Welcome, Admin!',
        secret: 'This is admin-only information'
    });
});

// User's own profile - accessible to authenticated users
app.get('/profile', authenticate, (req, res) => {
    res.json({
        username: req.user.username,
        role: req.user.role,
        message: 'This is your profile'
    });
});

// Update profile - only the user themselves can do this
app.put('/profile', authenticate, (req, res) => {
    // In real app, you'd update the database
    res.json({
        message: 'Profile updated successfully',
        user: req.user
    });
});

// ========================================
// LOGIN ROUTE (FOR OBTAINING CREDENTIALS)
// ========================================

app.post('/login', (req, res) => {
    const { username, password } = req.body;
    
    const user = users.find(u => u.username === username && u.password === password);
    
    if (!user) {
        return res.status(401).json({ error: 'Invalid credentials' });
    }
    
    // In a real app, you'd generate and return a JWT token here
    // For now, just return success
    res.json({
        message: 'Login successful',
        user: {
            id: user.id,
            username: user.username,
            role: user.role
        }
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
| 23 | `const authenticate = (req, res, next) => {...}` | Define authentication middleware |
| 28 | `const authHeader = req.headers.authorization;` | Get Authorization header from request |
| 35 | `Buffer.from(base64Credentials, 'base64').toString()` | Decode base64 credentials |
| 51 | `req.user = {...}` | Store authenticated user on request object |
| 52 | `next();` | Pass control to the next middleware |
| 62 | `const authorize = (roles = []) => {...}` | Define authorization middleware |
| 67 | `if (!req.user)` | Check if user is authenticated |
| 73 | `if (roles.length && !roles.includes(req.user.role))` | Check if user has required role |
| 93 | `app.get('/protected', authenticate, ...)` | Apply authenticate middleware to route |

## ⚠️ Common Mistakes

**1. Confusing authentication with authorization**
Authentication = proving who you are. Authorization = proving what you can do. They're different!

**2. Storing passwords in plain text**
NEVER store passwords as plain text. Always hash them (using bcrypt or similar).

**3. Not using HTTPS**
Authentication data sent over HTTP can be intercepted. Always use HTTPS in production.

**4. Forgetting to call next()**
Middleware must call `next()` or send a response. If it does neither, the request will hang.

**5. Not validating user input**
Always validate and sanitize user input, especially authentication credentials.

## ✅ Quick Recap

- **Authentication** verifies WHO a user is
- **Authorization** determines WHAT a user can do
- Use middleware to protect routes
- Store authenticated user info in `req.user`
- Create role-based authorization middleware
- Always hash passwords in production
- Use HTTPS in production

## 🔗 What's Next

Let's look at implementing JWT (JSON Web Tokens) for authentication, which is a modern and popular way to handle authentication in APIs.
