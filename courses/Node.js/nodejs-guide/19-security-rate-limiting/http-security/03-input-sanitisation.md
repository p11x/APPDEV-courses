# Input Sanitisation

## What You'll Learn

- Why input validation and sanitisation are essential
- How to use express-validator to validate request data
- How to sanitize inputs to prevent XSS and injection attacks
- How to validate different data types (email, URL, integer)
- How to return structured validation errors to the client

## Why Sanitise Input?

User input is the #1 attack vector. Attackers send:
- **SQL injection** — `'; DROP TABLE users;--`
- **XSS** — `<script>alert('xss')</script>`
- **Command injection** — `file.txt; rm -rf /`
- **Path traversal** — `../../../etc/passwd`

**Sanitisation** cleans input by removing or escaping dangerous characters. **Validation** rejects input that does not meet requirements.

> See: ../../06-databases/postgres/02-parameterized.md for parameterized queries that prevent SQL injection.

## Project Setup

```bash
npm install express express-validator
```

## Validation Middleware

```js
// sanitise.js — Input validation with express-validator

import express from 'express';
import { body, param, query, validationResult } from 'express-validator';

const app = express();
app.use(express.json());

// Middleware to check validation results and return errors
function validate(req, res, next) {
  const errors = validationResult(req);
  if (!errors.isEmpty()) {
    // Return structured errors — each with field, message, and value
    return res.status(400).json({
      error: 'Validation failed',
      details: errors.array().map((err) => ({
        field: err.path,
        message: err.msg,
        value: err.value,
      })),
    });
  }
  next();  // No errors — continue to the route handler
}

// POST /users — create a user with validated input
app.post(
  '/users',
  [
    // body() validates fields in req.body
    body('name')
      .trim()                          // Remove leading/trailing whitespace
      .isLength({ min: 2, max: 100 })  // Must be 2-100 characters
      .withMessage('Name must be 2-100 characters')
      .escape(),                       // Escape HTML entities (< → &lt;)

    body('email')
      .trim()
      .isEmail()                       // Must be a valid email format
      .withMessage('Invalid email address')
      .normalizeEmail(),               // Lowercase, remove dots in Gmail, etc.

    body('age')
      .optional()                      // Field is optional
      .isInt({ min: 13, max: 120 })   // Must be an integer between 13 and 120
      .withMessage('Age must be between 13 and 120')
      .toInt(),                        // Convert string to integer

    body('website')
      .optional()
      .trim()
      .isURL({ protocols: ['http', 'https'] })  // Must be a valid URL
      .withMessage('Must be a valid HTTP(S) URL'),

    body('bio')
      .optional()
      .trim()
      .isLength({ max: 500 })
      .withMessage('Bio must be under 500 characters')
      .escape(),
  ],
  validate,  // Check validation results
  (req, res) => {
    // At this point, all input is validated and sanitized
    const { name, email, age, website, bio } = req.body;

    // Safe to use — all values are validated and escaped
    res.status(201).json({
      message: 'User created',
      user: { name, email, age, website, bio },
    });
  }
);

// GET /search?q=... — validate query parameters
app.get(
  '/search',
  [
    query('q')
      .trim()
      .isLength({ min: 1, max: 200 })
      .withMessage('Search query must be 1-200 characters')
      .escape(),

    query('page')
      .optional()
      .isInt({ min: 1 })
      .withMessage('Page must be a positive integer')
      .toInt(),

    query('limit')
      .optional()
      .isInt({ min: 1, max: 100 })
      .withMessage('Limit must be between 1 and 100')
      .toInt(),
  ],
  validate,
  (req, res) => {
    res.json({
      query: req.query.q,
      page: req.query.page || 1,
      limit: req.query.limit || 20,
    });
  }
);

// GET /users/:id — validate URL parameters
app.get(
  '/users/:id',
  [
    param('id')
      .isInt({ min: 1 })
      .withMessage('User ID must be a positive integer')
      .toInt(),
  ],
  validate,
  (req, res) => {
    res.json({ userId: req.params.id });
  }
);

// Error responses for invalid input
// POST /users with bad data:
// {
//   "name": "A",
//   "email": "not-an-email",
//   "age": 5
// }
// Returns:
// {
//   "error": "Validation failed",
//   "details": [
//     { "field": "name", "message": "Name must be 2-100 characters", "value": "A" },
//     { "field": "email", "message": "Invalid email address", "value": "not-an-email" },
//     { "field": "age", "message": "Age must be between 13 and 120", "value": 5 }
//   ]
// }

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Preventing XSS

```js
// xss-prevention.js — Escape user input to prevent XSS

import express from 'express';
import { body, validationResult } from 'express-validator';

const app = express();
app.use(express.json());

app.post(
  '/comments',
  [
    body('text')
      .trim()
      .isLength({ min: 1, max: 1000 })
      .withMessage('Comment must be 1-1000 characters')
      .escape(),  // Converts < → &lt;, > → &gt;, etc.
  ],
  (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
      return res.status(400).json({ errors: errors.array() });
    }

    // req.body.text is now safe — HTML entities are escaped
    // "<script>alert('xss')</script>" becomes "&lt;script&gt;alert('xss')&lt;/script&gt;"
    const comment = req.body.text;

    // Return as JSON (safe) or render in HTML
    res.json({ comment });
  }
);

app.listen(3000);
```

## How It Works

### The Validation Chain

Each `body()`, `query()`, or `param()` call returns a chainable validator:

```
body('email')
  .trim()           → Remove whitespace
  .isEmail()        → Check format
  .normalizeEmail()  → Standardize
  .escape()         → Escape HTML
```

These run in order. If any step fails, the error is collected and the route handler is not called (because `validate` middleware returns 400).

### Sanitisation vs Validation

| Action | What It Does | Example |
|--------|-------------|---------|
| `trim()` | Remove whitespace | `" hello " → "hello"` |
| `escape()` | HTML entity encoding | `"<script>" → "&lt;script&gt;"` |
| `normalizeEmail()` | Standardize email | `"User@GMAIL.com" → "user@gmail.com"` |
| `toInt()` | Convert to integer | `"42" → 42` |
| `isEmail()` | Validate (does not modify) | Returns error if invalid |

## Common Mistakes

### Mistake 1: Escaping Output Instead of Input

```js
// WRONG — escaping in the route handler is error-prone
const comment = req.body.text.replace(/</g, '&lt;');
// You might forget to escape in some routes

// CORRECT — sanitize in the validation middleware
body('text').escape();
// Now ALL route handlers receive safe input
```

### Mistake 2: Not Normalizing Email

```js
// WRONG — "alice@gmail.com" and "alice+test@gmail.com" are treated as different users
body('email').isEmail();

// CORRECT — normalize to catch duplicates
body('email').isEmail().normalizeEmail();
```

### Mistake 3: Missing `toInt()` for Numeric Parameters

```js
// WRONG — req.params.id is a string even if it looks like a number
param('id').isInt();
const user = await db.findById(req.params.id);  // "42" is passed as string

// CORRECT — convert to actual integer
param('id').isInt().toInt();
const user = await db.findById(req.params.id);  // 42 is passed as number
```

## Try It Yourself

### Exercise 1: Validate a Contact Form

Create a `POST /contact` endpoint. Validate: name (2-100 chars), email (valid format), subject (1-200 chars), message (10-5000 chars). Return structured errors.

### Exercise 2: Password Validation

Add a password validator that requires: minimum 8 characters, at least one uppercase, one lowercase, one digit, and one special character.

### Exercise 3: Custom Validator

Write a custom validator that checks if a username is already taken (simulating a database lookup). Use `body('username').custom(async (value) => { ... })`.

## Next Steps

You can validate and sanitize input. For HTTPS and TLS setup, continue to [Self-Signed Certs](../https-tls/01-self-signed-certs.md).
