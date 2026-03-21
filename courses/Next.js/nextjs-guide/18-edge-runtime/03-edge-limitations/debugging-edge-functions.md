# Debugging Edge Functions

## What You'll Learn
- Debug Edge function errors
- Use Vercel/Cloudflare debugging tools
- Common Edge issues and solutions

## Prerequisites
- Understanding of Edge Runtime
- Basic debugging knowledge

## Do I Need This Right Now?
Debugging Edge functions can be tricky since they run in a limited environment. This guide helps you identify and fix common issues quickly.

## Concept Explained Simply

Debugging Edge functions is like fixing something in a small, locked room. You can't easily open the door to look around (no console access like local dev), so you need to be clever about what information you can see. Most debugging happens through error messages, logs, and careful testing.

## Debugging Tools

### 1. Vercel Console Logs

```typescript
// app/api/debug/route.ts
export const runtime = 'edge';

export async function GET(request: Request) {
  // Console logs appear in Vercel function logs
  console.log('Request received:', request.url);
  console.log('Headers:', Object.fromEntries(request.headers.entries()));
  
  // Try-catch for error details
  try {
    const response = await fetch('https://api.example.com/data');
    console.log('External API status:', response.status);
    
    if (!response.ok) {
      throw new Error(`External API failed: ${response.status}`);
    }
    
    const data = await response.json();
    return Response.json({ success: true, data });
  } catch (error) {
    console.error('Error:', error);
    return Response.json(
      { error: error instanceof Error ? error.message : 'Unknown error' },
      { status: 500 }
    );
  }
}
```

### 2. Check Runtime at Runtime

```typescript
// lib/runtime-check.ts
export function checkRuntime() {
  // Check if running in Edge
  if (typeof EdgeRuntime !== 'undefined') {
    console.log('Running on Edge Runtime:', EdgeRuntime);
  }
  
  // Check available APIs
  console.log('fetch available:', typeof fetch === 'function');
  console.log('crypto available:', typeof crypto !== 'undefined');
  console.log('setTimeout available:', typeof setTimeout === 'function');
  
  // Check environment
  console.log('Node version:', process.version); // Won't work in Edge!
}
```

## Common Issues and Solutions

### Issue 1: "ReferenceError: fetch is not defined"

**Cause:** Using fetch in an environment where it's not available (very old Edge runtimes)

**Solution:** Use cross-fetch or ensure correct runtime

```typescript
// Wrong: fetch might not be available in some edge contexts
export async function GET() {
  const data = await fetch('https://api.example.com');
  return Response.json({ data });
}

// Better: Use cross-fetch
import fetch from 'cross-fetch';

export const runtime = 'edge';

export async function GET() {
  const response = await fetch('https://api.example.com');
  return Response.json({ status: response.status });
}
```

### Issue 2: "Cannot find module 'fs'"

**Cause:** Trying to use Node.js module in Edge

**Solution:** Remove the import or use fetch instead

```typescript
// Wrong: fs not available in Edge
import { readFile } from 'fs/promises';

export const runtime = 'edge';

export async function GET() {
  const data = await readFile('data.json'); // Error!
  return Response.json({ data });
}

// Correct: Use fetch or external storage
export const runtime = 'edge';

export async function GET() {
  const response = await fetch('https://storage.example.com/data.json');
  const data = await response.json();
  return Response.json({ data });
}
```

### Issue 3: "RangeError: Too many requests" or Timeout

**Cause:** Too much work or long-running operations

**Solution:** Optimize code, reduce complexity

```typescript
// Wrong: Too much processing
export const runtime = 'edge';

export async function POST(request: Request) {
  const body = await request.json();
  
  // Heavy computation - will timeout!
  const result = heavyComputation(body.data);
  
  return Response.json({ result });
}

// Better: Chunked processing or move to serverless
export const runtime = 'edge';

export async function POST(request: Request) {
  const body = await request.json();
  
  // Light processing only
  const result = lightProcessing(body.data);
  
  // If heavy work needed, call separate API
  // const heavyResult = await fetch('https://api.example.com/process', {
  //   method: 'POST',
  //   body: JSON.stringify(body),
  // });
  
  return Response.json({ result });
}
```

### Issue 4: Environment Variables Not Available

**Cause:** Using server-side env vars in Edge

**Solution:** Prefix with NEXT_PUBLIC_ or use different approach

```typescript
// Wrong: Server-only env var
const apiKey = process.env.DATABASE_URL; // Won't work!

// Correct: Use NEXT_PUBLIC_ for client/edge accessible
const apiKey = process.env.NEXT_PUBLIC_API_URL;

// Or for secrets, use Vercel KV, etc.
```

### Issue 5: Headers Not Available

**Cause:** Some headers are restricted in Edge

**Solution:** Check headers before using

```typescript
// Wrong: Assuming all headers exist
export async function GET(request: Request) {
  const userId = request.headers.get('authorization')!; // Might be null!
  // Use without check - will crash if null
}

// Correct: Always check headers
export async function GET(request: Request) {
  const authHeader = request.headers.get('authorization');
  
  if (!authHeader) {
    return Response.json({ error: 'No auth header' }, { status: 401 });
  }
  
  // Now safe to use
  return Response.json({ user: 'found' });
}
```

## Debugging Checklist

```typescript
// Quick debug checklist
export async function debugRequest(request: Request) {
  // 1. Check runtime
  console.log('Runtime:', typeof EdgeRuntime !== 'undefined' ? 'Edge' : 'Node');
  
  // 2. Check URL
  console.log('URL:', request.url);
  
  // 3. Check method
  console.log('Method:', request.method);
  
  // 4. Check headers
  console.log('Headers:', Object.fromEntries(request.headers));
  
  // 5. Check geo (Edge only)
  console.log('Country:', request.geo?.country);
  
  // 6. Try-catch everything
  try {
    const body = await request.json();
    console.log('Body:', body);
  } catch (e) {
    console.log('No body or invalid JSON');
  }
  
  return Response.json({ debug: true });
}
```

## Testing Edge Locally

```typescript
// You can test Edge runtime with Vercel CLI
// vercel dev runs Edge functions locally

// Create a test script
// scripts/test-edge.mjs
import { spawn } from 'child_process';

async function testEdge() {
  console.log('Starting Vercel dev server...');
  
  const dev = spawn('npx', ['vercel', 'dev'], {
    stdio: 'inherit',
    env: { ...process.env, NEXT_RUNTIME: 'edge' },
  });
  
  dev.on('close', (code) => {
    console.log('Server closed with code:', code);
  });
}

testEdge();
```

## Summary
- Use console.log() for basic debugging (appears in platform logs)
- Check for runtime errors in Vercel/Cloudflare dashboards
- Common issues: Node.js modules, bundle size, timeouts
- Always validate headers before using
- Use NEXT_PUBLIC_ prefix for env vars in Edge
- Test locally with `vercel dev`

## Next Steps
- [turborepo-overview.md](../19-monorepo-with-turborepo/01-monorepo-concepts/turborepo-overview.md) — Monorepo with Turborepo
