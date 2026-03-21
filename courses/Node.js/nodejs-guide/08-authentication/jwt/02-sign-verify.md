# Signing and Verifying JWT

## What You'll Learn

- Creating signed JWTs
- Verifying JWTs
- JWT options

## Installing jsonwebtoken

```bash
npm install jsonwebtoken
```

## Signing JWTs

```javascript
// sign.js - Creating signed JWTs

import jwt from 'jsonwebtoken';

const secret = process.env.JWT_SECRET;

// Simple sign
const token = jwt.sign(
  { userId: 1 },
  secret
);

// With options
const tokenWithOptions = jwt.sign(
  { userId: 1, role: 'admin' },
  secret,
  {
    expiresIn: '1h',  // 1 hour
    issuer: 'my-app',
    subject: 'auth'
  }
);
```

## Verifying JWTs

```javascript
// verify.js - Verifying JWTs

import jwt from 'jsonwebtoken';

const token = 'eyJhbGciOiJIUzI1NiIs...';

try {
  const decoded = jwt.verify(token, secret);
  console.log('Valid!', decoded);
} catch (error) {
  console.log('Invalid:', error.message);
}
```

## Code Example

```javascript
// jwt-complete.js - Complete JWT example

import jwt from 'jsonwebtoken';

const SECRET = 'my-secret-key';

// Create token
function createToken(payload) {
  return jwt.sign(payload, SECRET, {
    expiresIn: '1h'
  });
}

// Verify token
function verifyToken(token) {
  try {
    return jwt.verify(token, SECRET);
  } catch (error) {
    return null;
  }
}

// Example
const user = { id: 1, name: 'Alice' };
const token = createToken(user);
console.log('Created:', token);

const decoded = verifyToken(token);
console.log('Verified:', decoded);
```

## Try It Yourself

### Exercise 1: Create Token with Claims
Create a JWT with custom claims.

### Exercise 2: Handle Errors
Implement proper error handling for invalid tokens.
