# Revalidation Strategies

## What You'll Learn
- Time-based revalidation
- On-demand revalidation
- Best practices

## Complete Guide

### Time-Based Revalidation

```typescript
// Revalidate every hour
fetch("/api/products", {
  next: { revalidate: 3600 },
});
```

### On-Demand Revalidation

```typescript
// src/app/api/revalidate/route.ts
import { revalidatePath, revalidateTag } from "next/cache";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
  const body = await request.json();
  
  // Revalidate by path
  revalidatePath("/blog");
  
  // Revalidate by tag
  revalidateTag("posts");
  
  return NextResponse.json({ revalidated: true });
}
```

## Summary

- Use time-based for predictable update intervals
- Use on-demand for CMS/webhook updates
- Combine both strategies as needed
