# Fastly Compute vs Cloudflare Workers

## What You'll Learn

- Detailed comparison of Fastly and Workers
- When to choose each
- Performance differences

## Comparison

| Feature | Fastly Compute | Cloudflare Workers |
|---------|---------------|-------------------|
| Runtime | WebAssembly | V8 |
| Languages | JS, Rust, Go | JS, TS, Rust, C |
| Cold start | <50μs | <5ms |
| Ecosystem | Smaller | Larger |
| Free tier | Limited | 100K requests/day |
| Global | 90+ POPs | 300+ data centers |

## When to Choose Fastly

- Need ultra-low latency (<50μs cold start)
- Using Rust at the edge
- Already using Fastly CDN
- Need advanced VCL-like logic

## When to Choose Workers

- JavaScript/TypeScript-first
- Need KV, D1, R2, Durable Objects
- Larger free tier
- More active ecosystem

## Next Steps

For Deno Deploy, continue to [Deno Setup](../04-deno-deploy/01-deno-setup.md).
