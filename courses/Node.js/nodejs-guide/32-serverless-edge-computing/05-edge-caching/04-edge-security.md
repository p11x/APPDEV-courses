# Edge Security

## What You'll Learn

- Security considerations for edge computing
- How to protect edge endpoints
- How to handle secrets at the edge
- DDoS protection at the edge

## Security Headers

```ts
// Apply security headers at the edge

export default {
  async fetch(request) {
    const response = await fetch(request);

    // Add security headers
    response.headers.set('Strict-Transport-Security', 'max-age=31536000; includeSubDomains');
    response.headers.set('X-Content-Type-Options', 'nosniff');
    response.headers.set('X-Frame-Options', 'DENY');
    response.headers.set('Referrer-Policy', 'strict-origin-when-cross-origin');

    return response;
  },
};
```

## DDoS Protection

```ts
// Rate limiting at the edge

export default {
  async fetch(request, env) {
    const ip = request.headers.get('cf-connecting-ip');
    const key = `ratelimit:${ip}`;

    const count = Number(await env.CACHE.get(key) || '0');
    if (count > 100) {
      return new Response('Rate Limited', { status: 429 });
    }

    await env.CACHE.put(key, String(count + 1), { expirationTtl: 60 });

    return fetch(request);
  },
};
```

## Secrets Management

```ts
// Use environment variables for secrets (never hardcode)
// wrangler.toml
[vars]
API_KEY = "set-via-dashboard"  # Set in Cloudflare dashboard, not in code

// Access in code
export default {
  async fetch(request, env) {
    const apiKey = env.API_KEY;  // From environment, not source code
    return fetch('https://api.example.com', {
      headers: { Authorization: `Bearer ${apiKey}` },
    });
  },
};
```

## Next Steps

This concludes Chapter 32. Return to the [guide index](../../index.html).
