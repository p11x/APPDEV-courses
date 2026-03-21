# Static Export

## Configure

```typescript
// next.config.ts
const nextConfig: NextConfig = {
  output: "export",
};

export default nextConfig;
```

## Build

```bash
npm run build
```

Output is in the `out` folder - can be deployed to any static host!

Note: Some Next.js features like Server-Side Rendering won't work with static export.
