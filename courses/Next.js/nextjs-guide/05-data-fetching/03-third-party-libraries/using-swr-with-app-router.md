# Using SWR with App Router

## What You'll Learn
- Using SWR in Client Components
- Real-time data fetching

## Complete Example

```typescript
// src/hooks/useData.ts
"use client";

import useSWR from "swr";

const fetcher = (url: string) => fetch(url).then((res) => res.json());

export function useData(url: string) {
  const { data, error, isLoading, mutate } = useSWR(url, fetcher);
  
  return {
    data,
    isLoading,
    isError: error,
    mutate,
  };
}
```

```typescript
// src/components/ProductList.tsx
"use client";

import { useData } from "@/hooks/useData";

export function ProductList() {
  const { data, isLoading } = useData("/api/products");
  
  if (isLoading) return <div>Loading...</div>;
  
  return (
    <ul>
      {data?.map((p: any) => <li key={p.id}>{p.name}</li>)}
    </ul>
  );
}
```

## Summary

- SWR works in Client Components
- Great for real-time data updates
- Use for client-side fetching with caching
