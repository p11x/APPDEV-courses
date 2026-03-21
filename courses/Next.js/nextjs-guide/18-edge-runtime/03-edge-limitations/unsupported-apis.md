# Unsupported APIs in Edge Runtime

## What You'll Learn
- Understand what Node.js APIs aren't available in Edge
- Find Edge-compatible alternatives
- Avoid common errors when writing Edge code

## Prerequisites
- Understanding of Edge Runtime basics

## Do I Need This Right Now?
This is crucial before writing any Edge code. Using unsupported APIs will cause your code to crash at runtime. Knowing what's unavailable helps you plan alternative solutions upfront.

## Concept Explained Simply

The Edge Runtime is like a stripped-down version of Node.js. Imagine a toolbox that only has a hammer, screwdriver, and wrench — it can do many jobs, but not everything. If you need a saw or drill, you'll need to call someone else (make an API call). This keeps the Edge fast and lightweight.

## APIs Not Available in Edge

### Filesystem (fs)

**Not Available:**
- `fs.readFile()`, `fs.writeFile()`
- `fs.readdir()`, `fs.stat()`
- `fs.createReadStream()`, `fs.createWriteStream()`

**Alternative:** Use external storage services (S3, Blob storage) via API calls:

```typescript
// Instead of fs.readFile
export const runtime = 'edge';

// Use external API
const response = await fetch('https://storage.example.com/file.txt');
const content = await response.text();
```

### Child Processes

**Not Available:**
- `child_process.spawn()`
- `child_process.exec()`
- `child_process.fork()`

**Alternative:** Use serverless functions or external services:

```typescript
// Can't spawn processes in Edge
// Instead, call an API that does the heavy work
const result = await fetch('https://api.example.com/process', {
  method: 'POST',
  body: JSON.stringify({ task: 'heavy-computation' }),
});
```

### Timers (Extended)

**Limited Availability:**
- `setTimeout()`, `setInterval()` — work but not for long durations
- `setImmediate()` — not available

**Best Practice:** Keep timers short:

```typescript
// Works but avoid long-running intervals
setTimeout(() => {
  console.log('Quick timeout');
}, 1000);

// Avoid: Long-running intervals will be killed
// setInterval(() => { ... }, 60000); // Won't work well!
```

### Buffer

**Limited Availability:**
- `Buffer.from()` — works but use sparingly
- `Buffer.concat()` — works
- Consider using `Uint8Array` instead

```typescript
// Works but prefer alternatives
const buffer = Buffer.from('hello');
const uint8 = new TextEncoder().encode('hello'); // Better for Edge
```

### Streams

**Limited Availability:**
- Full stream API not available
- Can consume streams but not create complex pipelines

```typescript
// Limited: Can consume fetch response streams
const response = await fetch('https://api.example.com/data');
const reader = response.body?.getReader();
// This works
```

### Net / TLS

**Not Available:**
- `net.createServer()`
- `tls.createServer()`
- `net.connect()`

**Alternative:** Use HTTP/HTTPS via fetch:

```typescript
// Can't create TCP servers
// Use HTTP APIs instead
const response = await fetch('https://api.example.com/data');
```

### DNS

**Not Available:**
- `dns.lookup()`
- `dns.resolve()`

**Alternative:** Use external services or hardcode IPs:

```typescript
// Can't do DNS lookups
// Use direct URLs instead
const response = await fetch('https://1.2.3.4/api/data'); // Direct IP
```

### path

**Limited Availability:**
- `path.join()`, `path.resolve()` — work
- `path.extname()`, `path.basename()` — work
- But limited to basic operations

### process

**Limited Availability:**
- `process.env` — works (read environment variables)
- `process.cwd()` — not available
- `process.exit()` — not available
- `process.uptime()` — not available

## Complete Code: Checking for Edge Compatibility

```typescript
// lib/runtime-check.ts

type Runtime = 'node' | 'edge' | 'unknown';

export function getRuntime(): Runtime {
  if (typeof globalThis.EdgeRuntime !== 'undefined') {
    return 'edge';
  }
  return 'node';
}

export function assertEdge(): void {
  if (getRuntime() !== 'edge') {
    throw new Error('This function must run on Edge Runtime');
  }
}

// Usage in a function
export async function edgeOnlyFunction() {
  assertEdge();
  
  // Now safe to use Edge-only features
  const data = await fetch('https://api.example.com/data');
  return data.json();
}
```

## Common Mistakes

### Mistake #1: Using fs Module
```typescript
// Wrong: fs doesn't exist in Edge
export const runtime = 'edge';

import { readFile } from 'fs/promises';

export async function GET() {
  const data = await readFile('data.json', 'utf-8'); // Error!
  return Response.json({ data });
}
```

```typescript
// Correct: Use fetch or external storage
export const runtime = 'edge';

export async function GET() {
  const response = await fetch('https://storage.example.com/data.json');
  const data = await response.json();
  return Response.json({ data });
}
```

### Mistake #2: Using Node.js Modules
```typescript
// Wrong: These modules don't exist in Edge
export const runtime = 'edge';

import { promisify } from 'util';
import { readFile } from 'fs/promises';
import path from 'path';

export async function GET() {
  const cwd = process.cwd(); // Error: process.cwd not available
  const fullPath = path.join(cwd, 'data.json'); // Might work
  return Response.json({ path: fullPath });
}
```

### Mistake #3: Using setInterval for Long Tasks
```typescript
// Wrong: Long intervals get killed
export const runtime = 'edge';

export async function GET() {
  // This will be terminated!
  setInterval(async () => {
    await fetch('https://api.example.com/cron', { method: 'POST' });
  }, 60000); // Every minute - won't work!
  
  return Response.json({ started: true });
}
```

## Summary
- Edge Runtime has limited APIs compared to Node.js
- No filesystem, child processes, or TCP/TLS servers
- Use fetch for external API calls instead of file/network operations
- Limited timer support — keep operations short
- Many npm packages won't work in Edge — check before using
- Use polyfills or alternative libraries when needed

## Next Steps
- [bundle-size-constraints.md](./bundle-size-constraints.md) — Edge bundle size limits
