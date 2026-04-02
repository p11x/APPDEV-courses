# Vercel Edge Functions and Deno Deploy

## What You'll Learn

- Vercel Edge Functions setup and deployment
- Deno Deploy for edge computing
- Edge caching strategies
- Performance optimization for edge

## Vercel Edge Functions

```javascript
// middleware.js — Vercel Edge Middleware

export const config = {
    matcher: ['/api/:path*', '/dashboard/:path*'],
};

export default function middleware(request) {
    // Geo-routing
    const country = request.geo?.country || 'US';
    
    // A/B testing at edge
    const bucket = Math.random() < 0.5 ? 'control' : 'variant';
    
    // Rewrite or redirect
    if (country === 'CN') {
        return Response.redirect('https://cn.example.com');
    }
    
    // Add headers
    const response = NextResponse.next();
    response.headers.set('x-country', country);
    response.headers.set('x-bucket', bucket);
    
    return response;
}
```

## Deno Deploy

```typescript
// Deno Deploy edge function
Deno.serve(async (req: Request) => {
    const url = new URL(req.url);
    
    if (url.pathname === '/api/time') {
        return Response.json({
            time: new Date().toISOString(),
            region: Deno.env.get('DENO_REGION'),
        });
    }
    
    return new Response('Hello from Deno Deploy!');
});
```

## Edge Caching

```javascript
// Edge caching with Cache API
export default {
    async fetch(request) {
        const cache = caches.default;
        
        // Check cache
        let response = await cache.match(request);
        if (response) return response;
        
        // Generate response
        response = new Response(JSON.stringify({ data: 'fresh' }), {
            headers: {
                'Content-Type': 'application/json',
                'Cache-Control': 's-maxage=60', // 60 seconds at edge
            },
        });
        
        // Store in cache
        await cache.put(request, response.clone());
        
        return response;
    },
};
```

## Best Practices Checklist

- [ ] Use edge functions for geo-routing and auth
- [ ] Cache API responses at edge
- [ ] Keep edge function code minimal
- [ ] Use environment variables for configuration
- [ ] Monitor edge function cold starts

## Cross-References

- See [Cloudflare Workers](./01-cloudflare-workers.md) for Cloudflare patterns
- See [Edge Optimization](./03-edge-optimization.md) for performance tuning
- See [WASM Integration](../18-wasm-integration/01-wasm-basics.md) for WASM at edge

## Next Steps

Continue to [Edge Optimization](./03-edge-optimization.md) for performance tuning.
