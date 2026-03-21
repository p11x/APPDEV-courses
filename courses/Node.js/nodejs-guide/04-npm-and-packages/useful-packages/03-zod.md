# Using Zod for Data Validation

## What You'll Learn

- What Zod is and why to use it
- Creating schemas
- Validating data
- Custom validation rules

## What is Zod?

**Zod** is a TypeScript-first schema validation library. It lets you define the "shape" of your data and validate that data matches that shape.

## Installing Zod

```bash
npm install zod
```

## Basic Usage

### Creating a Schema

```javascript
// zod-basic.js - Basic Zod usage

import { z } from 'zod';

// Define a schema
const UserSchema = z.object({
  name: z.string(),
  age: z.number(),
  email: z.string().email()
});

// Validate data
const userData = {
  name: 'Alice',
  age: 25,
  email: 'alice@example.com'
};

const result = UserSchema.parse(userData);
console.log('Valid:', result);
```

### Invalid Data

```javascript
// zod-invalid.js - Handling validation errors

import { z } from 'zod';

const UserSchema = z.object({
  name: z.string(),
  age: z.number()
});

try {
  // This will fail - age should be a number!
  const result = UserSchema.parse({
    name: 'Alice',
    age: '25'  // String, not number!
  });
} catch (error) {
  console.log('Validation error:');
  console.log(error.errors);
}
```

## Schema Types

### Primitive Types

```javascript
import { z } from 'zod';

// String
z.string();
z.string().min(1);
z.string().max(100);
z.string().email();
z.string().url();

// Number
z.number();
z.number().min(0);
z.number().max(100);
z.number().int();  // Integer

// Boolean
z.boolean();

// Date
z.date();
```

### Arrays and Objects

```javascript
import { z } from 'zod';

// Array of strings
z.array(z.string());

// Array of numbers
z.array(z.number());

// Object with nested types
const PersonSchema = z.object({
  name: z.string(),
  address: z.object({
    street: z.string(),
    city: z.string(),
    zip: z.string()
  })
});
```

### Optional and Nullable

```javascript
import { z } from 'zod';

// Optional field
z.object({
  name: z.string(),
  nickname: z.string().optional()
});

// Nullable field
z.object({
  email: z.string().nullable()
});

// Default value
z.object({
  name: z.string().default('Anonymous')
});
```

## Code Example: Complete Zod Demo

```javascript
// zod-demo.js - Complete Zod demonstration

import { z } from 'zod';

console.log('=== Zod Validation Demo ===\n');

// ─────────────────────────────────────────
// 1. Basic Object Schema
// ─────────────────────────────────────────
console.log('1. Basic Object:');

const UserSchema = z.object({
  name: z.string(),
  age: z.number()
});

const user = { name: 'Alice', age: 25 };
console.log('   Valid:', UserSchema.parse(user));

// ─────────────────────────────────────────
// 2. With Validation Rules
// ─────────────────────────────────────────
console.log('\n2. With Validation:');

const ValidatedUser = z.object({
  name: z.string().min(2, 'Name too short'),
  age: z.number().min(0).max(150),
  email: z.string().email('Invalid email')
});

try {
  ValidatedUser.parse({
    name: 'A',  // Too short!
    age: 25,
    email: 'not-an-email'
  });
} catch (error) {
  console.log('   Errors:', error.errors);
}

// ─────────────────────────────────────────
// 3. Safe Parse (No Throws)
// ─────────────────────────────────────────
console.log('\n3. Safe Parse:');

const result = UserSchema.safeParse({ name: 'Bob', age: 30 });

if (result.success) {
  console.log('   Valid:', result.data);
} else {
  console.log('   Errors:', result.error.errors);
}

// ─────────────────────────────────────────
// 4. Arrays
// ─────────────────────────────────────────
console.log('\n4. Arrays:');

const strings = z.array(z.string());
console.log('   String array:', strings.parse(['a', 'b', 'c']));

const numbers = z.array(z.number());
console.log('   Number array:', numbers.parse([1, 2, 3]));

// ─────────────────────────────────────────
// 5. Optional and Defaults
// ─────────────────────────────────────────
console.log('\n5. Optional & Defaults:');

const OptionalUser = z.object({
  name: z.string(),
  role: z.string().default('user')
});

const user1 = OptionalUser.parse({ name: 'Alice' });
const user2 = OptionalUser.parse({ name: 'Bob', role: 'admin' });

console.log('   With default:', user1);  // role: 'user'
console.log('   Override:', user2);        // role: 'admin'

// ─────────────────────────────────────────
// 6. Real-World Example
// ─────────────────────────────────────────
console.log('\n6. API Request Validation:');

const CreateUserRequest = z.object({
  username: z.string().min(3).max(20),
  email: z.string().email(),
  age: z.number().optional(),
  password: z.string().min(8)
});

function createUser(data) {
  const result = CreateUserRequest.safeParse(data);
  
  if (!result.success) {
    console.log('   Validation failed:', result.error.errors);
    return null;
  }
  
  console.log('   Creating user:', result.data);
  return result.data;
}

createUser({ username: 'al', email: 'a@b.c', password: '12345678' });
createUser({ username: 'alice', email: 'alice@example.com', password: 'secret123' });
```

## Common Mistakes

### Mistake 1: Not Handling Errors

```javascript
// WRONG - throws on invalid data
const result = UserSchema.parse(invalidData);

// CORRECT - use safeParse
const result = UserSchema.safeParse(invalidData);
if (!result.success) {
  console.log(result.error.errors);
}
```

### Mistake 2: Wrong Type Order

```javascript
// Zod methods chain from the type
z.string().min(3);    // ✓ String with min length
z.number().email();  // ✗ Error! Numbers don't have email()
```

## Try It Yourself

### Exercise 1: Create User Schema
Create a schema for a user with name, email, and optional age.

### Exercise 2: Form Validation
Validate a form submission with multiple fields.

### Exercise 3: API Response Validation
Validate an API response matches expected structure.

## Next Steps

Now you've learned about useful npm packages. Let's move on to the Express framework. Continue to [Express Setup](./05-express-framework/getting-started/01-express-setup.md).
