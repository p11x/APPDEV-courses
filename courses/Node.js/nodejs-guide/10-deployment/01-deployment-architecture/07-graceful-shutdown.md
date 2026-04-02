# Graceful Shutdown

## What You'll Learn

- Handling shutdown signals
- Cleaning up resources

## Handling Signals

```javascript
// shutdown.js - Graceful shutdown

import express from 'express';

const app = express();
const server = app.listen(3000);

// Handle SIGTERM (from PM2, docker)
process.on('SIGTERM', () => {
  console.log('SIGTERM received, shutting down...');
  
  server.close(() => {
    console.log('HTTP server closed');
    // Close database, etc.
    process.exit(0);
  });
});

// Handle SIGINT (Ctrl+C)
process.on('SIGINT', () => {
  console.log('SIGINT received, shutting down...');
  server.close(() => {
    process.exit(0);
  });
});
```

## Cleanup

```javascript
// cleanup.js - Cleanup on shutdown

let db;

async function shutdown() {
  console.log('Shutting down...');
  
  // Close database connections
  if (db) {
    await db.close();
  }
  
  // Close file handles
  // Clear caches
  // etc.
  
  process.exit(0);
}

process.on('SIGTERM', shutdown);
process.on('SIGINT', shutdown);
```

## Code Example

```javascript
// server.js - Complete server with graceful shutdown

import express from 'express';

const app = express();
const server = app.listen(3000);

app.get('/', (req, res) => {
  res.send('Running');
});

// Graceful shutdown
async function shutdown() {
  console.log('Received shutdown signal');
  
  server.close(async () => {
    console.log('HTTP server closed');
    
    // Cleanup
    console.log('Cleanup complete');
    process.exit(0);
  });
  
  // Force exit after 10 seconds
  setTimeout(() => {
    console.error('Forced shutdown after timeout');
    process.exit(1);
  }, 10000);
}

process.on('SIGTERM', shutdown);
process.on('SIGINT', shutdown);
```

## Try It Yourself

### Exercise 1: Add Shutdown Handler
Add graceful shutdown to your server.

### Exercise 2: Cleanup Resources
Clean up resources on shutdown.
