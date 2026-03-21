# Streaming in Next.js

## What You'll Learn
- What is streaming
- How it improves perceived performance

## Concept Explained Simply

**Streaming** sends parts of the page as they become ready, instead of waiting for everything. Users see content faster even if some parts take longer.

```typescript
// src/app/page.tsx
import { Suspense } from "react";

export default function Page() {
  return (
    <main>
      <h1>My Page</h1>
      <Suspense fallback={<Loading />}>
        <SlowComponent />
      </Suspense>
    </main>
  );
}

function SlowComponent() {
  // Takes time to load
  return <div>Data loaded!</div>;
}

function Loading() {
  return <div>Loading...</div>;
}
```

## Summary

- Uses Suspense boundaries
- Shows fallback while loading
- Improves perceived performance
