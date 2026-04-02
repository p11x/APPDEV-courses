# Bun Setup

## What You'll Learn

- What Bun is and why it exists
- How to install and configure Bun
- How to run JavaScript and TypeScript with Bun
- How Bun differs from Node.js at the runtime level

## What Is Bun?

Bun is a **JavaScript runtime** built from scratch using JavaScriptCore (Safari's engine) instead of V8. It includes a bundler, transpiler, package manager, and test runner — all in a single binary. The goal is maximum performance with zero configuration.

| Feature | Node.js | Bun |
|---------|---------|-----|
| Engine | V8 | JavaScriptCore |
| Package manager | npm/yarn/pnpm | bun (built-in) |
| Bundler | webpack/esbuild (external) | Built-in |
| TypeScript | Needs tsc | Built-in transpiler |
| Test runner | node:test | bun:test |
| Startup time | ~40ms | ~6ms |
| HTTP requests/sec | ~50,000 | ~100,000+ |

## Installation

```bash
# macOS / Linux / WSL
curl -fsSL https://bun.sh/install | bash

# Verify installation
bun --version
# e.g., 1.1.x

# Windows (experimental — via WSL recommended)
powershell -c "irm bun.sh/install.ps1 | iex"
```

## First Bun Program

```js
// hello.ts — Bun runs TypeScript directly, no tsc needed

const greeting: string = 'Hello from Bun!';
console.log(greeting);
console.log(`Bun version: ${Bun.version}`);
```

```bash
# Run directly — no compilation step
bun hello.ts
# Hello from Bun!
# Bun version: 1.1.x
```

## Project Initialization

```bash
# Create a new project (like npm init but faster)
bun init -y

# This creates:
# package.json
# tsconfig.json (auto-configured for Bun)
# .gitignore
```

## Bun's Built-in APIs

Bun provides Node.js-compatible APIs plus additional ones:

```js
// bun-apis.ts — Bun-specific APIs

// File reading — 10x faster than node:fs for small files
const file = Bun.file('./data.json');
const data = await file.json();

// Write files
await Bun.write('./output.txt', 'Hello from Bun!');

// HTTP server — uses uWebSockets under the hood
const server = Bun.serve({
  port: 3000,
  fetch(req) {
    return new Response('Hello from Bun!', {
      headers: { 'Content-Type': 'text/plain' },
    });
  },
});

console.log(`Server running on http://localhost:${server.port}`);

// SQLite — built-in, no driver needed
import { Database } from 'bun:sqlite';
const db = new Database('mydb.sqlite');
db.run('CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, name TEXT)');
db.run('INSERT INTO users (name) VALUES (?)', ['Alice']);
const users = db.query('SELECT * FROM users').all();
console.log(users);
```

## Environment Variables

```bash
# .env file is auto-loaded by Bun (no dotenv needed)
DATABASE_URL=postgresql://localhost/mydb
API_KEY=secret
```

```ts
// Auto-loaded from .env
console.log(process.env.DATABASE_URL);
console.log(process.env.API_KEY);
```

## How It Works

### Why Bun Is Fast

1. **JavaScriptCore** — Safari's engine, faster startup than V8
2. **Zig** — written in Zig (systems language), not C++
3. **Bundling** — built-in bundler reduces file I/O
4. **Native APIs** — SQLite, WebSocket, file I/O are native Zig, not C++ bindings
5. **No module resolution overhead** — static analysis at install time

### Compatibility

Bun aims for Node.js API compatibility. Most `node:` modules work:

| Module | Bun Support |
|--------|------------|
| `node:fs` | Full |
| `node:http` | Full |
| `node:crypto` | Full |
| `node:stream` | Full |
| `node:worker_threads` | Full |
| `node:test` | Partial |
| `node:vm` | Partial |

Some npm packages with native addons (C++ bindings) may not work. Test before migrating.

## Common Mistakes

### Mistake 1: Mixing npm and bun install

```bash
# WRONG — creates conflicting lock files
npm install express
bun install fastify
# package-lock.json and bun.lockb both exist

# CORRECT — use one package manager per project
bun install express fastify
```

### Mistake 2: Assuming All npm Packages Work

```bash
# WRONG — some native packages fail on Bun
bun install sharp  # May not compile

# CORRECT — test compatibility, use alternatives if needed
bun install @aspect-build/img-processor  # Bun-compatible alternative
```

### Mistake 3: Using Node.js-Specific APIs Without Testing

```ts
// WRONG — some Node.js internals may behave differently
import { createRequire } from 'node:module';  // May not work in Bun

// CORRECT — use Bun's native APIs where possible
import { file } from 'bun';  // Bun-specific file API
```

## Next Steps

For a detailed comparison, continue to [Bun vs Node.js](./02-bun-vs-node.md).
