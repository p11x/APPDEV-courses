# JWT Authentication Middleware

## What You'll Learn

- Creating auth middleware
- Protecting routes
- Extracting tokens

## Auth Middleware

```javascript
// auth.js - JWT authentication middleware

import jwt from 'jsonwebtoken';

const SECRET = 'my-secret-key';

function authenticate(req, res, next) {
  // Get token from header
  const authHeader = req.headers.authorization;
  
  if (!authHeader) {
    return res.status(401).json({ error: 'No token' });
  }
  
  const token = authHeader.split(' ')[1];
  
  try {
    const decoded = jwt.verify(token, SECRET);
    req.user = decoded;
    next();
  } catch (error) {
    return res.status(401).json({ error: 'Invalid token' });
  }
}

export default authenticate;
```

## Protecting Routes

```javascript
// routes.js - Protected routes

import express from 'express';
import authenticate from './auth.js';

const app = express();

// Public route
app.get('/public', (req, res) => {
  res.json({ message: 'Public' });
});

// Protected route
app.get('/protected', authenticate, (req, res) => {
  res.json({ 
    message: 'Protected',
    user: req.user 
  });
});
```

## Code Example

```javascript
// middleware-demo.js - Complete auth middleware

import express from 'express';
import jwt from 'jsonwebtoken';

const app = express();
app.use(express.json());

const SECRET = 'secret';

// Auth middleware
function auth(req, res, next) {
  const token = req.headers.authorization?.split(' ')[1];
  
  if (!token) {
    return res.status(401).json({ error: 'No token' });
  }
  
  try {
    req.user = jwt.verify(token, SECRET);
    next();
  } catch {
    res.status(401).json({ error: 'Invalid token' });
  }
}

// Login route
app.post('/login', (req, res) => {
  const { email } = req.body;
  const token = jwt.sign({ email }, SECRET, { expiresIn: '1h' });
  res.json({ token });
});

// Protected route
app.get('/me', auth, (req, res) => {
  res.json({ user: req.user });
});

app.listen(3000);
```

## Try It Yourself

### Exercise 1: Create Auth Middleware
Create authentication middleware for Express.

### Exercise 2: Protect Routes
Protect routes using the middleware.
