# Environment Variables

## What You'll Learn

- NODE_ENV
- Using dotenv
- Environment-specific configurations

## NODE_ENV

NODE_ENV tells Node.js which environment you're running in:

```javascript
console.log(process.env.NODE_ENV);
// 'development', 'production', or 'test'
```

## Common Values

```javascript
// development - local dev
// production - live app
// test - running tests
```

## Setting NODE_ENV

```bash
# Linux/Mac
NODE_ENV=production node app.js

# Windows (PowerShell)
$env:NODE_ENV="production"
```

## Code Example

```javascript
// env-example.js - Environment configuration

import dotenv from 'dotenv';
dotenv.config();

// Environment
const env = process.env.NODE_ENV || 'development';

// Config based on environment
const config = {
  development: {
    port: 3000,
    dbUrl: 'localhost:5432'
  },
  production: {
    port: 8080,
    dbUrl: 'prod-db:5432'
  },
  test: {
    port: 3001,
    dbUrl: 'test-db:5432'
  }
}[env];

console.log('Environment:', env);
console.log('Config:', config);
```

## Try It Yourself

### Exercise 1: Set Environment
Set NODE_ENV to different values.

### Exercise 2: Load Config
Load different config based on environment.
