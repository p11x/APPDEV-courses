# Fresh vs Next.js

## What You'll Learn

- How Fresh compares to Next.js
- Performance differences
- When to choose each
- Migration considerations

## Comparison

| Feature | Next.js | Fresh |
|---------|---------|-------|
| Runtime | Node.js | Deno |
| Default JS shipped | Full bundle | Zero |
| Hydration | Full page | Per-island |
| React support | Yes | Preact |
| Edge runtime | Yes (middleware) | Native |
| Package manager | npm/yarn | jsr/deno.land |
| API routes | App Router | File-based handlers |
| Database | Any npm package | Deno-compatible |
| Deployment | Vercel, any host | Deno Deploy, Docker |
| Maturity | Very mature | Growing |
| Ecosystem | Massive | Smaller |

## Performance

| Metric | Next.js (Node.js) | Fresh (Deno) |
|--------|-------------------|--------------|
| TTI (first page) | 800ms | 200ms |
| JS bundle size | 150KB+ | 0-10KB |
| Lighthouse score | 85-95 | 95-100 |
| Server response | 50ms | 20ms |

## When to Choose Fresh

- **Performance is critical** — zero JS by default means faster pages
- **Deno ecosystem** — already using Deno
- **Edge-first** — deploy to Deno Deploy globally
- **Simple content sites** — blogs, docs, landing pages
- **TypeScript-first** — Deno has native TypeScript

## When to Choose Next.js

- **Large ecosystem needed** — many npm packages available
- **React 19 features** — server components, suspense
- **Complex state management** — Redux, Zustand integration
- **Existing React codebase** — migration is simpler
- **Vercel deployment** — tight integration

## Migration Path

```tsx
// Next.js → Fresh conversion:

// Next.js (React)
export default function Home() {
  const [count, setCount] = useState(0);  // Ships to client
  return <button onClick={() => setCount(count + 1)}>{count}</button>;
}

// Fresh (Preact) — move interactive part to island
// routes/index.tsx (server component)
import Counter from '../islands/Counter.tsx';
export default function Home() {
  return <Counter />;  // Only Counter ships JS
}

// islands/Counter.tsx (interactive island)
import { useState } from 'preact/hooks';
export default function Counter() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(count + 1)}>{count}</button>;
}
```

## Hybrid Architecture

You can use both:

```
User-facing pages → Fresh (fast, zero JS)
Admin dashboard  → Next.js (rich interactivity)
API backend      → Hono/Fastify (shared between both)
```

## Next Steps

This concludes Chapter 30. Return to the [guide index](../../index.html) to explore all chapters.
