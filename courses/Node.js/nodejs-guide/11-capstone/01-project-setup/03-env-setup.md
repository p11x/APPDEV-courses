# Environment Variables Setup

## What You'll Build In This File

Setting up environment variables using dotenv, including a template file and configuration module.

## .env Example Template

Create a file called `.env.example` that serves as a template:

```
# Server Configuration
PORT=3000
NODE_ENV=development

# Database Configuration
DB_HOST=localhost
DB_PORT=5432
DB_NAME=nodemark
DB_USER=postgres
DB_PASSWORD=your_password_here

# JWT Configuration
JWT_SECRET=your_super_secret_jwt_key_change_in_production
JWT_EXPIRES_IN=7d

# Application URL (for emails, links, etc.)
APP_URL=http://localhost:3000
```

## Complete Config Module

Create `src/config/index.js`:

```javascript
// src/config/index.js - Centralized configuration for the application
// Loads environment variables and provides sensible defaults

import dotenv from 'dotenv';

// Load .env file from project root
// In production, environment variables are set externally
dotenv.config();

/**
 * Configuration object with environment variables and defaults
 * All config values are validated - missing required vars throw errors
 */
export const config = {
  // Server configuration
  port: parseInt(process.env.PORT || '3000',  // Default port 3000
  nodeEnv: process.env.NODE_ENV || 'development',  // Default to dev
  
  // Database configuration
  db: {
    host: process.env.DB_HOST || 'localhost',
    port: parseInt(process.env.DB_PORT || '5432'),
    database: process.env.DB_NAME || 'nodemark',
    user: process.env.DB_USER || 'postgres',
    password: process.env.DB_PASSWORD || '',
    // Connection pool settings
    max: 20,  // Max connections in pool
    idleTimeoutMillis: 30000,  // Close idle clients after 30s
    connectionTimeoutMillis: 2000,  // Return error after 2s if can't connect
  },
  
  // JWT configuration
  jwt: {
    // JWT secret - in production, MUST be set via environment variable
    secret: process.env.JWT_SECRET || 'dev-secret-change-in-production',
    expiresIn: process.env.JWT_EXPIRES_IN || '7d',  // 7 days
  },
  
  // Application URLs
  appUrl: process.env.APP_URL || 'http://localhost:3000',
};

/**
 * Validates required configuration
 * Throws errors for missing critical values in production
 */
export function validateConfig() {
  const errors = [];
  
  // In production, we require these values
  if (config.nodeEnv === 'production') {
    if (!process.env.JWT_SECRET || process.env.JWT_SECRET === 'dev-secret-change-in-production') {
      errors.push('JWT_SECRET must be set in production');
    }
    if (!process.env.DB_PASSWORD) {
      errors.push('DB_PASSWORD must be set in production');
    }
  }
  
  if (errors.length > 0) {
    throw new Error(`Configuration errors: ${errors.join(', ')}`);
  }
}

// Export convenience function to get db connection string
export function getDbConnectionString() {
  const { host, port, database, user, password } = config.db;
  return `postgresql://${user}:${password}@${host}:${port}/${database}`;
}
```

## How to Use Config in Your App

```javascript
// Using the config in your application
import { config, validateConfig, getDbConnectionString } from './config/index.js';

// Validate on startup - throws if config is invalid
validateConfig();

console.log('Starting in:', config.nodeEnv);
console.log('Port:', config.port);
console.log('DB Host:', config.db.host);
```

## .gitignore Entry

Add to your `.gitignore` file:

```
# Environment variables - NEVER commit actual .env
.env
.env.local
.env.*.local
```

This prevents accidentally committing secrets. See [10-deployment/environment/01-env-variables.md](../../../10-deployment/environment/01-env-variables.md) for more on this.

## How It Connects

This connects to concepts from the guide:

- Using dotenv for environment variables (see [04-npm-and-packages/useful-packages/01-dotenv.md](../../../04-npm-and-packages/useful-packages/01-dotenv.md))
- Centralized config pattern from [10-deployment/environment/02-config-module.md](../../../10-deployment/environment/02-config-module.md)
- Using process.env from [01-introduction/first-program/03-script-args.md](../../../01-introduction/first-program/03-script-args.md)

## Common Mistakes

- Committing .env file with real credentials
- Not using different values for production vs development
- Hardcoding secrets in source code
- Forgetting to validate required environment variables

## Try It Yourself

### Exercise 1: Create .env
Copy .env.example to .env and fill in your PostgreSQL credentials.

### Exercise 2: Add Validation
Add validation for another required environment variable.

### Exercise 3: Test Without .env
Try running your app without a .env file and see what happens.

## Next Steps

Continue to [../../02-database/01-schema.md](../../02-database/01-schema.md) to design the database schema.
