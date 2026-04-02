# Edge Optimization

## What You'll Learn

- How to optimize Edge Functions for performance
- Caching strategies at the edge
- How to minimize cold starts
- How to use streaming responses

## Caching at the Edge

```ts
// app/api/data/route.ts

export const runtime = 'edge';

export async function GET() {
  const data = await fetchData();

  return Response.json(data, {
    headers: {
      // Cache at CDN edge for 60 seconds
      'Cache-Control': 'public, s-maxage=60, stale-while-revalidate=300',
      // Vary by Accept header
      'Vary': 'Accept',
    },
  });
}
```

## Streaming Responses

```ts
// app/api/stream/route.ts

export const runtime = 'edge';

export async function GET() {
  const encoder = new TextEncoder();

  const stream = new ReadableStream({
    async start(controller) {
      for (let i = 0; i < 100; i++) {
        controller.enqueue(encoder.encode(`data: ${i}\n\n`));
        await new Promise((r) => setTimeout(r, 100));
      }
      controller.close();
    },
  });

  return new Response(stream, {
    headers: {
      'Content-Type': 'text/event-stream',
      'Cache-Control': 'no-cache',
    },
  });
}
```

## Minimizing Bundle Size

```ts
// Import only what you need
// BAD — imports entire library
import _ from 'lodash';

// GOOD — import specific function
import debounce from 'lodash/debounce';

// GOOD — use native APIs instead of libraries
const unique = [...new Set(array)];  // Instead of lodash.uniq
const grouped = Object.groupBy(items, (item) => item.category);
```

## Next Steps

For comparison with Node.js, continue to [Edge vs Node.js](./04-edge-vs-nodejs.md).
