# Streaming with Suspense

## What You'll Learn
- Streaming patterns with Suspense

## Example

```typescript
// src/app/page.tsx
import { Suspense } from "react";

export default function Page() {
  return (
    <div>
      <Suspense fallback={<HeaderSkeleton />}>
        <Header />
      </Suspense>
      <Suspense fallback={<SidebarSkeleton />}>
        <Sidebar />
      </Suspense>
      <Suspense fallback={<ContentSkeleton />}>
        <Content />
      </Suspense>
    </div>
  );
}
```

Each section streams independently!
