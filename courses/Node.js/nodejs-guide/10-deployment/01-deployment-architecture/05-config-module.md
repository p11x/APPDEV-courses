# Centralized Configuration

## What You'll Learn

- Creating a config module
- Managing environment-specific settings

## Config Module

```javascript
// config.js - Centralized configuration

import dotenv from 'dotenv';
dotenv.config();

const env = process.env.NODE_ENV || 'development';

const config = {
  // Server
  port: parseInt(process.env.PORT || '3000'),
  
  // Database
  database: {
    url: process.env.DATABASE_URL,
    ssl: env === 'production'
  },
  
  // JWT
  jwt: {
    secret: process.env.JWT_SECRET,
    expiresIn: process.env.JWT_EXPIRES_IN || '1d'
  },
  
  // Environment
  env,
  isDev: env === 'development',
  isProd: env === 'production',
  isTest: env === 'test'
};

export default config;
```

## Using Config

```javascript
// app.js - Using config

import config from './config.js';
import express from 'express';

const app = express();

app.listen(config.port, () => {
  console.log(`Server running on port ${config.port}`);
  console.log(`Environment: ${config.env}`);
});
```

## Code Example

```javascript
// config-example.js - Complete config example

import dotenv from 'dotenv';
dotenv.config();

export default {
  port: parseInt(process.env.PORT || '3000'),
  database: {
    url: process.env.DATABASE_URL || 'sqlite://db.sqlite'
  },
  jwt: {
    secret: process.env.JWT_SECRET || 'dev-secret'
  },
  env: process.env.NODE_ENV || 'development'
};
```

## Try It Yourself

### Exercise 1: Create Config
Create a centralized config module.

### Exercise 2: Use in App
Import and use the config throughout your app.
