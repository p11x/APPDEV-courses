# Edge Computing with Node.js

## What You'll Learn

- Cloudflare Workers with Node.js patterns
- Vercel Edge Functions
- Edge runtime constraints and optimizations
- When to use edge deployment

## Cloudflare Workers

```javascript
// worker.js — Cloudflare Worker (V8 isolates, not Node.js)

export default {
    async fetch(request, env, ctx) {
        const url = new URL(request.url);
        
        // Routing
        if (url.pathname === '/api/hello') {
            return new Response(JSON.stringify({
                message: 'Hello from the edge!',
                timestamp: Date.now(),
                cf: request.cf, // Geographic info
            }), {
                headers: { 'Content-Type': 'application/json' },
            });
        }
        
        // KV storage
        const value = await env.MY_KV.get('key');
        
        // D1 database
        const results = await env.DB.prepare('SELECT * FROM users LIMIT 10').all();
        
        return new Response('Not found', { status: 404 });
    },
};
```

### Edge Constraints

```
Edge Runtime Limitations:
─────────────────────────────────────────────
Available:
├── fetch() API
├── Web Crypto API
├── TextEncoder/Decoder
├── URL, URLSearchParams
├── ArrayBuffer, TypedArray
├── Promise, async/await
├── JSON, Math, Date
└── KV, R2, D1 (Cloudflare)

Not Available:
├── Node.js built-in modules (fs, net, etc.)
├── File system access
├── Raw TCP/UDP sockets
├── process object (limited)
├── Buffer (use Uint8Array)
├── __dirname, __filename
├── require() (ESM only)
└── Native addons
```

## Vercel Edge Functions

```javascript
// api/hello.js — Vercel Edge Function

export const config = {
    runtime: 'edge',
};

export default async function handler(request) {
    const { searchParams } = new URL(request.url);
    const name = searchParams.get('name') || 'World';
    
    return new Response(JSON.stringify({
        message: `Hello, ${name}!`,
        runtime: 'edge',
        region: request.headers.get('x-vercel-id'),
    }), {
        headers: { 'Content-Type': 'application/json' },
    });
}
```

## Best Practices Checklist

- [ ] Minimize bundle size for edge deployment
- [ ] Use Web APIs instead of Node.js APIs
- [ ] Cache aggressively at edge
- [ ] Keep edge functions stateless
- [ ] Use streaming responses for large payloads
- [ ] Test edge functions locally before deployment

## Cross-References

- See [WASM Integration](../18-wasm-integration/01-wasm-basics.md) for WASM at edge
- See [Security Best Practices](../21-security-modern/01-security-headers-deps.md) for edge security
- See [Performance Deep Dive](../09-performance-deep-dive/01-performance-characteristics.md) for optimization

## Next Steps

Continue to [Vercel Edge Functions](./02-vercel-edge.md) for Vercel-specific patterns.
