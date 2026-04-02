# Debug Module

## What You'll Learn

- How to use the `debug` package for conditional logging
- How the `DEBUG` environment variable controls output
- How to organize debug output with namespaces
- How to extend debug loggers
- How debug differs from console.log

## Why Debug?

`console.log` is always visible — you must remove or comment it out for production. The `debug` package is **disabled by default** and enabled only when you set the `DEBUG` environment variable.

```bash
# No output — debug is off by default
node server.js

# Enable all debug output
DEBUG=* node server.js

# Enable only specific namespaces
DEBUG=app:auth,app:db node server.js

# Enable everything EXCEPT noisy modules
DEBUG=*,-express:* node server.js
```

## Project Setup

```bash
npm install debug
```

## Basic Usage

```js
// auth.js — Authentication module with debug logging

import debug from 'debug';

// Create a debugger with a namespace
// Namespace convention: app:module or app:module:submodule
const log = debug('app:auth');

export function login(email, password) {
  // log() only outputs when DEBUG=app:auth (or DEBUG=*)
  log('Login attempt for %s', email);

  if (!email || !password) {
    log('Missing credentials');
    return { success: false, error: 'Missing credentials' };
  }

  // Simulate authentication
  const isValid = email === 'alice@example.com' && password === 'secret';

  if (isValid) {
    log('Login successful for %s', email);
    return { success: true, token: 'jwt-token' };
  } else {
    log('Login failed for %s — invalid credentials', email);
    return { success: false, error: 'Invalid credentials' };
  }
}
```

```js
// db.js — Database module with debug logging

import debug from 'debug';

const log = debug('app:db');

export async function query(sql, params) {
  log('Executing: %s', sql);
  log('Parameters: %O', params);  // %O prints objects nicely

  // Simulate query execution
  await new Promise((r) => setTimeout(r, 50));

  const rows = [{ id: 1, name: 'Alice' }];
  log('Query returned %d rows', rows.length);

  return rows;
}
```

```js
// server.js — Main server using auth and db modules

import { createServer } from 'node:http';
import debug from 'debug';
import { login } from './auth.js';
import { query } from './db.js';

const log = debug('app:server');

const server = createServer(async (req, res) => {
  log('Incoming request: %s %s', req.method, req.url);

  if (req.url === '/login' && req.method === 'POST') {
    let body = '';
    req.on('data', (chunk) => { body += chunk; });
    req.on('end', () => {
      const { email, password } = JSON.parse(body);
      const result = login(email, password);

      res.writeHead(result.success ? 200 : 401, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify(result));
    });
    return;
  }

  if (req.url === '/users') {
    const users = await query('SELECT * FROM users', []);
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify(users));
    return;
  }

  res.writeHead(404);
  res.end('Not found');
});

server.listen(3000, () => {
  log('Server started on port 3000');
});
```

### Running

```bash
# No debug output
node server.js

# Only auth module
DEBUG=app:auth node server.js

# Only database module
DEBUG=app:db node server.js

# Everything
DEBUG=app:* node server.js

# Everything except database
DEBUG=app:*,-app:db node server.js
```

## Extending Debug Loggers

```js
// extended.js — Add custom logic to debug loggers

import debug from 'debug';

const base = debug('app');

// Extend with a custom formatter
const log = base.extend('worker');

// Add a timestamp prefix
log.log = (...args) => {
  const timestamp = new Date().toISOString();
  console.log(`[${timestamp}]`, ...args);
};

log('Worker started');
log('Processing job %d', 42);
```

## How It Works

### Namespace Matching

The `DEBUG` environment variable supports glob patterns:

| DEBUG value | Matches |
|-------------|---------|
| `*` | All namespaces |
| `app:*` | `app:auth`, `app:db`, `app:server` |
| `app:auth` | Only `app:auth` |
| `*,-app:db` | Everything except `app:db` |
| `app:*:error` | `app:auth:error`, `app:db:error` |

### debug vs console.log

| Feature | console.log | debug |
|---------|-------------|-------|
| Always visible | Yes | Only when DEBUG is set |
| Namespace filtering | No | Yes |
| Performance when disabled | Slow (stringify) | Zero cost (no-op) |
| Color-coded output | No | Yes (auto-colored per namespace) |

## Common Mistakes

### Mistake 1: Using console.log Instead of debug

```js
// WRONG — always visible, must remove for production
console.log('User logged in:', userId);

// CORRECT — only visible when DEBUG is set
log('User logged in: %s', userId);
```

### Mistake 2: No Namespace Organization

```js
// WRONG — all logs use the same namespace
const log = debug('app');
log('User auth');   // Can't filter separately
log('DB query');    // Can't filter separately

// CORRECT — separate namespaces per module
const authLog = debug('app:auth');
const dbLog = debug('app:db');
```

### Mistake 3: Expensive Debug Arguments

```js
// WRONG — stringify runs even when debug is disabled
log('Result: ' + JSON.stringify(hugeObject));

// CORRECT — pass objects as arguments (debug skips if disabled)
log('Result: %O', hugeObject);  // %O is only evaluated if debug is enabled
```

## Try It Yourself

### Exercise 1: Namespace Filtering

Create 3 modules with different debug namespaces. Enable only one at a time and verify the output.

### Exercise 2: Glob Patterns

Use `DEBUG=app:*,-app:db` to enable all app debuggers except the database one.

### Exercise 3: Performance

Compare performance: loop 1 million times with `console.log` vs `debug`. Verify debug is faster when disabled.

## Next Steps

You can debug with conditional logging. For CPU profiling, continue to [CPU Profiling](../profiling/01-cpu-profiling.md).
