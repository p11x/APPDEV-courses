# Using dotenv for Environment Variables

## What You'll Learn

- What environment variables are
- How to use dotenv package
- Creating .env files
- Accessing environment variables in code

## Environment Variables

**Environment variables** are values that exist outside your code - in the environment where your app runs. They're used for:
- Configuration (API keys, database URLs)
- Secrets (passwords, tokens)
- Environment-specific settings (dev vs prod)

## The dotenv Package

The `dotenv` package loads variables from a `.env` file into `process.env`.

### Installing dotenv

```bash
npm install dotenv
```

### Creating .env File

Create a `.env` file in your project root:

```
# Database configuration
DB_HOST=localhost
DB_PORT=5432
DB_NAME=myapp

# API keys
API_KEY=your-api-key-here

# App settings
PORT=3000
NODE_ENV=development
```

### Loading in Code

```javascript
// config.js - Loading environment variables

// Import dotenv
import dotenv from 'dotenv';

// Load .env file
dotenv.config();

// Now access variables from process.env
console.log(process.env.DB_HOST);    // "localhost"
console.log(process.env.API_KEY);    // "your-api-key-here"
console.log(process.env.PORT);       // "3000"
```

## Using Environment Variables

### Basic Usage

```javascript
// index.js - Using environment variables

import dotenv from 'dotenv';
dotenv.config();

// Get port from environment, default to 3000
const PORT = process.env.PORT || 3000;

// Get database URL
const DB_URL = process.env.DATABASE_URL || 'sqlite://localhost';

console.log(`Starting server on port ${PORT}`);
console.log(`Connecting to database: ${DB_URL}`);
```

### With Express

```javascript
// server.js - Express with dotenv

import dotenv from 'dotenv';
import express from 'express';

dotenv.config();

const app = express();
const PORT = process.env.PORT || 3000;

app.get('/', (req, res) => {
  res.send('Hello, World!');
});

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

## .gitignore

**Important**: Never commit `.env` files to version control! Add them to `.gitignore`:

```
# .gitignore
node_modules/
.env
*.env
.env.local
```

### Example .gitignore

```
# Dependencies
node_modules/

# Environment files
.env
.env.local
.env.*.local

# Logs
*.log

# Build output
dist/
build/
```

## Code Example: Complete Setup

### Step 1: Install

```bash
npm install dotenv
```

### Step 2: Create .env

```
# .env
PORT=3000
DATABASE_URL=postgresql://localhost:5432/mydb
API_SECRET=my-secret-key
NODE_ENV=development
```

### Step 3: Use in Code

```javascript
// config.js - Configuration module

import dotenv from 'dotenv';

dotenv.config();

export const config = {
  port: parseInt(process.env.PORT || '3000'),
  databaseUrl: process.env.DATABASE_URL,
  apiSecret: process.env.API_SECRET,
  isDevelopment: process.env.NODE_ENV === 'development'
};

console.log('Config loaded:', {
  port: config.port,
  databaseUrl: config.databaseUrl,
  isDevelopment: config.isDevelopment
});
```

```javascript
// index.js - Main entry point

import { config } from './config.js';

console.log(`Starting ${config.isDevelopment ? 'development' : 'production'} server`);
console.log(`Port: ${config.port}`);
console.log(`Database: ${config.databaseUrl}`);
```

## Common Mistakes

### Mistake 1: Forgetting dotenv.config()

```javascript
// WRONG - variables not loaded
import { something } from 'process.env';

// CORRECT - load first
import dotenv from 'dotenv';
dotenv.config();

console.log(process.env.VAR_NAME);
```

### Mistake 2: Committing .env

```javascript
// NEVER commit .env files!
# .gitignore
.env  // Add this!
```

### Mistake 3: Using Wrong Variable Names

```javascript
// In .env: API_KEY=abc
// In code:

console.log(process.env.API_KEY);     // ✓ Works
console.log(process.env.api_key);     // ✗ Wrong - case sensitive!
```

## Try It Yourself

### Exercise 1: Set Up dotenv
Install dotenv and create a .env file with some variables.

### Exercise 2: Access Variables
Print all your environment variables from the .env file.

### Exercise 3: Use in Express
Create an Express server that uses variables from .env.

## Next Steps

Now you know how to use dotenv. Let's learn about chalk for terminal colors. Continue to [chalk Package](./02-chalk.md).
