# Node.js vs Edge Runtime

## What You'll Learn
- Understand the differences between Node.js and Edge Runtime
- Know when to choose each runtime
- Make informed decisions about runtime for your code

## Prerequisites
- Basic understanding of server-side JavaScript
- Knowledge of Next.js App Router

## Do I Need This Right Now?
This is foundational knowledge for using Edge features. Understanding the differences helps you avoid runtime errors and choose the right execution environment. If you're building anything that needs to run on Edge (middleware, fast API routes), this is essential.

## Concept Explained Simply

Think of Node.js as a fully equipped kitchen in a restaurant, and Edge Runtime as a food truck. The kitchen (Node.js) has every appliance and ingredient you could need — it's powerful but takes time to set up. The food truck (Edge) has only the essentials, but it's much faster to deploy and can go anywhere.

**Node.js Runtime:**
- Full JavaScript runtime with all Node.js APIs
- Can access filesystems, databases, child processes
- Runs on servers with persistent processes
- Slower cold starts (typically 100-500ms)

**Edge Runtime:**
- Lightweight JavaScript execution at CDN edge locations
- Limited APIs (fetch, crypto, URL, timers)
- No filesystem or database access directly
- Extremely fast cold starts (typically <10ms)
- Executes close to users geographically

## Feature Comparison Table

| Feature | Node.js | Edge Runtime |
|---------|---------|--------------|
| Filesystem (fs) | ✓ | ✗ |
| Database connections | ✓ | ✗ (use API calls) |
| Node.js modules | ✓ | ✗ |
| fetch() | ✓ | ✓ |
| Web Crypto API | ✓ | ✓ |
| URL/URLSearchParams | ✓ | ✓ |
| setTimeout/setInterval | ✓ | ✓ (limited) |
| Buffer | ✓ | Partial |
| Stream | ✓ | Limited |
| Environment variables | ✓ | ✓ |
| Cold start time | 100-500ms | <10ms |
| Geographic distribution | Single region | Global |

## When to Use Each

### Use Node.js When:
- You need filesystem access
- You connect to databases directly
- You need Node.js-specific modules
- Complex server-side logic with heavy processing
- WebSocket connections

### Use Edge When:
- Authentication/authorization middleware
- Geographic routing (A/B testing by country)
- Simple API transformations
- Personalization based on headers
- Rate limiting
- Authentication token verification

## Complete Code Example

Here's how to use both runtimes in the same Next.js app:

```typescript
// app/api/node/route.ts
// This runs in Node.js runtime (default)
import { writeFile, readFile } from 'fs/promises';
import { join } from 'path';

export async function POST(request: Request) {
  // This only works in Node.js!
  const data = await request.json();
  
  const filePath = join(process.cwd(), 'data.json');
  await writeFile(filePath, JSON.stringify(data));
  
  return Response.json({ success: true, runtime: 'node' });
}

export async function GET() {
  // Read from filesystem - Node.js only
  const filePath = join(process.cwd(), 'data.json');
  const content = await readFile(filePath, 'utf-8');
  
  return Response.json({ 
    data: JSON.parse(content), 
    runtime: 'node' 
  });
}
```

```typescript
// app/api/edge/route.ts
// This runs in Edge Runtime
export const runtime = 'edge';

export async function GET(request: Request) {
  // Can use fetch, crypto, URL - but NOT filesystem!
  
  const url = new URL(request.url);
  const name = url.searchParams.get('name') || 'World';
  
  // Use fetch to get data from external API
  const response = await fetch('https://api.example.com/data');
  const externalData = await response.json();
  
  return Response.json({
    message: `Hello, ${name}!`,
    externalData,
    runtime: 'edge',
    region: request.headers.get('x-vercel-id')?.split('::')[0] || 'unknown',
  });
}
```

## Common Mistakes

### Mistake #1: Using Node.js APIs in Edge
```typescript
// Wrong: fs module doesn't exist in Edge
export const runtime = 'edge';

import { writeFile } from 'fs/promises'; // Error!

export async function POST(request: Request) {
  await writeFile('data.json', 'hello');
}
```

```typescript
// Correct: Use external API or service for persistence
export const runtime = 'edge';

export async function POST(request: Request) {
  const data = await request.json();
  
  // Call external API or database
  await fetch('https://api.example.com/save', {
    method: 'POST',
    body: JSON.stringify(data),
  });
  
  return Response.json({ success: true });
}
```

### Mistake #2: Not Specifying Runtime
```typescript
// Ambiguous: Uses default Node.js runtime
// Could cause slow cold starts
export async function GET() {
  return Response.json({ hello: 'world' });
}
```

```typescript
// Clear: Explicitly specifies Edge for fast responses
export const runtime = 'edge';

export async function GET() {
  return Response.json({ hello: 'world' });
}
```

### Mistake #3: Connecting to Database Directly
```typescript
// Wrong: Database connections don't work well in Edge
export const runtime = 'edge';

import { PrismaClient } from '@prisma/client';

const prisma = new PrismaClient(); // May not work!

export async function GET() {
  const users = await prisma.user.findMany(); // Unreliable
  return Response.json(users);
}
```

```typescript
// Better: Use API route as intermediary
// Edge route calls Node.js API which connects to DB
export const runtime = 'edge';

export async function GET() {
  // Call your Node.js API route
  const response = await fetch('https://yourapp.com/api/users');
  const users = await response.json();
  
  return Response.json(users);
}
```

## Summary
- Node.js has full capabilities but slower cold starts
- Edge Runtime is fast and globally distributed but limited
- Choose Edge for simple, fast operations (middleware, routing, auth)
- Choose Node.js for database operations, file handling, complex logic
- Always specify runtime explicitly with `export const runtime = 'edge'`
- Avoid connecting to databases directly from Edge — use API calls instead

## Next Steps
- [enabling-edge-runtime.md](./enabling-edge-runtime.md) — How to enable Edge Runtime
