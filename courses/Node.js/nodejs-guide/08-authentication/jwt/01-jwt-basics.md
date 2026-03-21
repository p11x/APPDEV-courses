# JWT Basics

## What You'll Learn

- What JWT is
- JWT structure
- When to use JWT

## What is JWT?

**JWT** (JSON Web Token) is a compact, URL-safe way to securely transmit information between parties as a JSON object.

## JWT Structure

A JWT has three parts separated by dots:
```
xxxxx.yyyyy.zzzzz
header.payload.signature
```

### Header
```json
{
  "alg": "HS256",
  "typ": "JWT"
}
```

### Payload
```json
{
  "sub": "1234567890",
  "name": "John Doe",
  "admin": true
}
```

## When to Use JWT

- **Authentication**: After login, send JWT with requests
- **Data exchange**: Securely transmit data between services

## Code Example

```javascript
// jwt-demo.js - JWT demonstration

import jwt from 'jsonwebtoken';

const secret = 'my-secret-key';

// Create token
const token = jwt.sign(
  { userId: 1, name: 'Alice' },
  secret,
  { expiresIn: '1h' }
);

console.log('Token:', token);

// Verify token
try {
  const decoded = jwt.verify(token, secret);
  console.log('Decoded:', decoded);
} catch (error) {
  console.log('Invalid token');
}

// Decode without verifying
const decoded = jwt.decode(token);
console.log('Decoded (no verify):', decoded);
```

## Try It Yourself

### Exercise 1: Create JWT
Create a JWT with user information.

### Exercise 2: Verify JWT
Verify a JWT signature.
