# First Hello World and Running Node.js Applications

## What You'll Learn

- Creating and running your first Node.js script
- Understanding the execution lifecycle
- Running Node.js from the terminal with different options
- Using the Node.js REPL for quick experiments

## Step 1: Create Your First Script

Create a new directory and file:

```bash
mkdir my-first-app
cd my-first-app
```

Create `hello.js`:

```javascript
// hello.js — Your first Node.js program

console.log('Hello, World!');
console.log('Node.js version:', process.version);
console.log('Running on:', process.platform);
console.log('Current directory:', process.cwd());
console.log('Script arguments:', process.argv.slice(2));
```

## Step 2: Run the Script

```bash
node hello.js
```

Expected output:
```
Hello, World!
Node.js version: v22.14.0
Running on: win32
Current directory: C:\Users\you\my-first-app
Script arguments: []
```

## Step 3: Pass Arguments to Scripts

```bash
node hello.js Alice Bob
```

Update `hello.js` to use arguments:

```javascript
// hello.js — With argument handling

const args = process.argv.slice(2);

if (args.length === 0) {
  console.log('Hello, World!');
} else {
  args.forEach(name => {
    console.log(`Hello, ${name}!`);
  });
}
```

Output:
```
Hello, Alice!
Hello, Bob!
```

## Step 4: Create a More Practical Script

Create `system-info.js`:

```javascript
// system-info.js — Display system information

const os = require('node:os');

function formatBytes(bytes) {
  return (bytes / 1024 / 1024).toFixed(1) + ' MB';
}

const info = {
  hostname: os.hostname(),
  platform: `${os.type()} ${os.release()}`,
  arch: os.arch(),
  cpus: os.cpus().length + ' cores',
  totalMemory: formatBytes(os.totalmem()),
  freeMemory: formatBytes(os.freemem()),
  uptime: Math.round(os.uptime() / 60) + ' minutes',
  nodeVersion: process.version,
  pid: process.pid,
};

console.log('=== System Information ===\n');
for (const [key, value] of Object.entries(info)) {
  console.log(`  ${key.padEnd(15)}: ${value}`);
}
```

Run it:
```bash
node system-info.js
```

## Step 5: Use the Node.js REPL

The REPL (Read-Eval-Print Loop) is an interactive shell for quick experiments:

```bash
node
```

```javascript
> 2 + 2
4

> const name = 'Node.js'
undefined

> `Welcome to ${name}!`
'Welcome to Node.js!'

> [1, 2, 3].map(x => x * 2)
[2, 4, 6]

> .help    // Show REPL commands
> .exit    // Exit the REPL
```

Load a file in REPL:
```bash
node -e "require('./system-info.js')"
```

## Step 6: Watch Mode (Auto-Restart)

Node.js v18+ has built-in watch mode:

```bash
# Automatically restarts when files change
node --watch hello.js
```

Create `server.js` to see this in action:

```javascript
// server.js — Simple HTTP server with watch mode

const http = require('node:http');

const server = http.createServer((req, res) => {
  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end('Hello from Node.js!\n');
});

server.listen(3000, () => {
  console.log('Server running at http://localhost:3000');
  console.log('Edit this file and save — server restarts automatically');
});
```

```bash
node --watch server.js
# Open http://localhost:3000 in your browser
# Edit server.js, save — changes appear on refresh
```

## Step 7: Running with Debug Flags

```bash
# Syntax check without running
node --check hello.js

# Print V8 bytecode
node --print-bytecode hello.js 2>&1 | head -20

# Enable source maps for better error traces
node --enable-source-maps hello.js

# Set max memory (useful for large data processing)
node --max-old-space-size=512 hello.js  # 512MB limit
```

## Common Patterns

### Environment Variables

```javascript
// config.js — Read environment variables

const config = {
  port: process.env.PORT || 3000,
  env: process.env.NODE_ENV || 'development',
  debug: process.env.DEBUG === 'true',
};

console.log('Configuration:', config);
```

```bash
# Set environment variables and run
PORT=8080 NODE_ENV=production node config.js    # macOS/Linux
$env:PORT=8080; $env:NODE_ENV='production'; node config.js  # Windows PowerShell
```

### Graceful Shutdown

```javascript
// graceful.js — Handle process signals

const http = require('node:http');

const server = http.createServer((req, res) => {
  res.end('OK');
});

server.listen(3000, () => {
  console.log('Server started on port 3000');
  console.log('Press Ctrl+C to stop');
});

// Handle SIGINT (Ctrl+C)
process.on('SIGINT', () => {
  console.log('\nShutting down gracefully...');
  server.close(() => {
    console.log('Server closed');
    process.exit(0);
  });
});

// Handle SIGTERM (kill command)
process.on('SIGTERM', () => {
  console.log('Received SIGTERM, shutting down...');
  server.close(() => process.exit(0));
});
```

## Performance Optimization Checklist

- [ ] Use `node --watch` during development (not nodemon for v18+)
- [ ] Use `--max-old-space-size` for memory-intensive scripts
- [ ] Use `--enable-source-maps` for debugging
- [ ] Use `node --check` to validate syntax before running
- [ ] Implement graceful shutdown handlers in long-running processes

## Next Steps

Learn how to configure your project properly with [Package.json Basics](./03-package-json-basics.md).
