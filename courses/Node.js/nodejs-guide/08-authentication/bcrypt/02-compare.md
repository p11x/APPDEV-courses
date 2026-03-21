# Comparing Passwords with bcrypt

## What You'll Learn

- Comparing hashed passwords
- Login flow implementation

## Comparing Passwords

```javascript
// compare.js - Comparing passwords

import bcrypt from 'bcrypt';

// Hash stored in database
const storedHash = '$2b$10$...';

// User input
const userPassword = 'myPassword';

async function verifyPassword(password, hash) {
  const match = await bcrypt.compare(password, hash);
  return match;
}

// Usage
const isValid = await verifyPassword(userPassword, storedHash);
console.log('Valid:', isValid);
```

## Login Flow

```javascript
// login-flow.js - Complete login example

import bcrypt from 'bcrypt';

const SALT_ROUNDS = 10;

// Simulated database
const users = [];

// Register
async function register(email, password) {
  const hash = await bcrypt.hash(password, SALT_ROUNDS);
  const user = { email, passwordHash: hash };
  users.push(user);
  return user;
}

// Login
async function login(email, password) {
  const user = users.find(u => u.email === email);
  
  if (!user) {
    return null;
  }
  
  const match = await bcrypt.compare(password, user.passwordHash);
  
  if (match) {
    return user;
  }
  
  return null;
}

// Example
await register('alice@example.com', 'password123');
const user = await login('alice@example.com', 'password123');
console.log('Logged in:', user);
```

## Code Example

```javascript
// auth-flow.js - Complete auth demonstration

import bcrypt from 'bcrypt';

const SALT_ROUNDS = 10;

// Hash and verify
async function testAuth() {
  const password = 'secret123';
  
  // Hash
  const hash = await bcrypt.hash(password, SALT_ROUNDS);
  console.log('Hash:', hash);
  
  // Verify correct password
  const match1 = await bcrypt.compare(password, hash);
  console.log('Correct password:', match1);
  
  // Verify wrong password
  const match2 = await bcrypt.compare('wrong', hash);
  console.log('Wrong password:', match2);
}

testAuth();
```

## Try It Yourself

### Exercise 1: Verify Password
Verify a password against a hash.

### Exercise 2: Login Flow
Implement complete registration and login flow.
