# Bundle Size Constraints

## What You'll Learn
- Understand Edge bundle size limits
- Optimize code for Edge deployment
- Reduce bundle size for faster cold starts

## Prerequisites
- Understanding of Edge Runtime
- Knowledge of JavaScript bundling

## Do I Need This Right Now?
If you're deploying to Edge, bundle size directly affects cold start time and can even cause deployment failures if too large. This is essential for anyone deploying Edge functions.

## Concept Explained Simply

Edge functions are like carry-on luggage on an airplane. There's a strict weight limit — if your bag is too heavy, you can't board (deploy). Smaller bundles mean faster uploads, faster cold starts, and better performance. Every kilobyte counts!

## Size Limits

### Vercel Edge Functions
- **Maximum bundle size:** ~1MB (compressed)
- **Recommended:** Under 100KB for optimal performance
- Cold start increases with bundle size

### Cloudflare Workers
- **Maximum bundle size:** 1MB (uncompressed)
- **CPU time:** 10ms (free) to 50ms (paid) per request

## Strategies for Reducing Bundle Size

### 1. Use Edge-Compatible Libraries Only

```typescript
// Wrong: Heavy Node.js library
import { z } from 'zod'; // Can be large
import { pinyin } from 'pinyin'; // Large Chinese library

export const runtime = 'edge';
```

```typescript
// Correct: Use lightweight alternatives
// For validation, use built-in or smaller libraries
function validateEmail(email: string): boolean {
  return /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(email);
}
```

### 2. Lazy Load Heavy Dependencies

```typescript
// Heavy library - only load if needed
export const runtime = 'edge';

export async function POST(request: Request) {
  // Only import when needed
  const { heavyFunction } = await import('@/lib/heavy');
  
  const data = await request.json();
  return Response.json({ result: heavyFunction(data) });
}
```

### 3. Use Web APIs Instead of Node.js

```typescript
// Wrong: Node.js crypto - bundles extra code
import { createHash } from 'crypto';

export const runtime = 'edge';

export async function POST(request: Request) {
  const hash = createHash('sha256').update(await request.text()).digest('hex');
  return Response.json({ hash });
}
```

```typescript
// Correct: Web Crypto API - built into browser/Edge
export const runtime = 'edge';

export async function POST(request: Request) {
  const encoder = new TextEncoder();
  const data = encoder.encode(await request.text());
  const hashBuffer = await crypto.subtle.digest('SHA-256', data);
  const hashArray = Array.from(new Uint8Array(hashBuffer));
  const hash = hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
  return Response.json({ hash });
}
```

### 4. Configure Next.js to Exclude Node.js Modules

```typescript
// next.config.ts
import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  experimental: {
    // Automatically replace Node.js modules with Edge-compatible versions
    serverComponentsExternalPackages: [
      // List packages that should remain external
    ],
  },
  // Exclude specific packages from edge bundles
  webpack: (config, { isEdge }) => {
    if (isEdge) {
      // Mark as external to reduce bundle size
      config.externals = config.externals || [];
      config.externals.push('fs', 'path', 'crypto');
    }
    return config;
  },
};

export default nextConfig;
```

### 5. Code Splitting for Edge

```typescript
// Instead of one large route handler, split into smaller ones
// app/api/user/route.ts - handles user operations
// app/api/products/route.ts - handles product operations

// Avoid: bundling everything in one file
// app/api/all-in-one/route.ts - Don't do this
```

### 6. Measure Your Bundle

```typescript
// Create a script to analyze edge bundle
// scripts/analyze-edge.mjs
import { execSync } from 'child_process';
import { readFileSync } from 'fs';

console.log('Building Edge bundle...');
execSync('NEXT_RUNTIME=edge npm run build', { stdio: 'inherit' });

// Check sizes
const files = [
  '.next/server/edge/api/route.js',
];

for (const file of files) {
  const content = readFileSync(file);
  console.log(`${file}: ${(content.length / 1024).toFixed(2)} KB`);
}
```

## Complete Example: Optimized Edge Handler

```typescript
// app/api/optimized/route.ts
// This file is optimized for Edge

export const runtime = 'edge';

// 1. No imports from heavy Node.js modules
// 2. Use Web APIs directly
// 3. Minimal dependencies

interface RequestBody {
  email: string;
}

function validateEmail(email: string): boolean {
  // Simple regex instead of validation library
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

async function hashEmail(email: string): Promise<string> {
  // Use Web Crypto instead of Node.js crypto
  const encoder = new TextEncoder();
  const data = encoder.encode(email.toLowerCase());
  const hashBuffer = await crypto.subtle.digest('SHA-256', data);
  const hashArray = Array.from(new Uint8Array(hashBuffer));
  return hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
}

export async function POST(request: Request) {
  try {
    const body: RequestBody = await request.json();
    
    // Inline validation - no external dependencies
    if (!body.email || !validateEmail(body.email)) {
      return Response.json(
        { error: 'Invalid email' },
        { status: 400 }
      );
    }
    
    // Hash using Web Crypto - built-in
    const hashedEmail = await hashEmail(body.email);
    
    return Response.json({
      success: true,
      // Don't return the hash itself in production!
      // This is just for demo
      processed: true,
    });
  } catch (error) {
    return Response.json(
      { error: 'Invalid request body' },
      { status: 400 }
    );
  }
}
```

## Common Mistakes

### Mistake #1: Importing Large Libraries
```typescript
// Wrong: Large validation library
import { z } from 'zod';

export const runtime = 'edge';

const schema = z.object({
  email: z.string().email(),
});

export async function POST(request: Request) {
  const body = await request.json();
  const result = schema.parse(body); // Heavy!
}
```

```typescript
// Correct: Use simple regex or lightweight alternatives
export const runtime = 'edge';

function validateEmail(email: string): boolean {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}
```

### Mistake #2: Not Checking Bundle Size
```typescript
// Just add imports without checking size
import { someLargePackage } from 'large-package';
// ... lots more imports
// Then deploy and wonder why it's slow
```

```typescript
// Check bundle size regularly
// Add to package.json:
// "analyze": "ANALYZE=true next build"
```

### Mistake #3: Including Server-Only Code
```typescript
// Wrong: This won't work in Edge and adds to bundle
import { writeFile } from 'fs/promises';

export const runtime = 'edge';

export async function POST(request: Request) {
  // This will fail AND increase bundle size!
  await writeFile('data.json', await request.text());
}
```

## Summary
- Edge bundles have strict size limits (~1MB max)
- Keep bundles under 100KB for best performance
- Use Web APIs instead of Node.js modules
- Avoid large npm packages in Edge code
- Lazy load heavy dependencies when possible
- Regularly analyze bundle sizes during development
- Smaller bundles = faster cold starts = better UX

## Next Steps
- [debugging-edge-functions.md](./debugging-edge-functions.md) — Troubleshooting Edge issues
