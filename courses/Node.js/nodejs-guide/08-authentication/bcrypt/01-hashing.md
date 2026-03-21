# Password Hashing with bcrypt

## What You'll Learn

- Installing bcrypt
- Hashing passwords
- Understanding salt rounds

## Installing bcrypt

```bash
npm install bcrypt
```

## Hashing Passwords

```javascript
// hash.js - Hashing passwords

import bcrypt from 'bcrypt';

const saltRounds = 10;

async function hashPassword(password) {
  const hash = await bcrypt.hash(password, saltRounds);
  return hash;
}

// Usage
const hash = await hashPassword('myPassword');
console.log('Hash:', hash);
```

## Salt Rounds

```javascript
// salt-rounds.js - Understanding salt rounds

// Higher = more secure but slower
// 10 is a good default

const hash1 = await bcrypt.hash('pass', 10);  // Default
const hash2 = await bcrypt.hash('pass', 12);  // Slower, more secure
```

## Code Example

```javascript
// bcrypt-demo.js - Complete bcrypt example

import bcrypt from 'bcrypt';

const SALT_ROUNDS = 10;

// Hash password
async function createHash(password) {
  const hash = await bcrypt.hash(password, SALT_ROUNDS);
  return hash;
}

// Example
const password = 'mySecurePassword123';
const hash = await createHash(password);

console.log('Password:', password);
console.log('Hash:', hash);
console.log('Hash length:', hash.length);
```

## Try It Yourself

### Exercise 1: Hash Password
Hash a password using bcrypt.

### Exercise 2: Experiment with Salt
Compare hashes with different salt rounds.
