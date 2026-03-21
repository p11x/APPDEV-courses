# Cache Options in Next.js Fetch

## What You'll Learn
- All fetch cache options
- When to use each option

## Complete Guide

| Option | Behavior | Use Case |
|--------|----------|----------|
| `force-cache` | Cache forever | Static content |
| `no-store` | Never cache | Real-time data |
| `no-cache` | Validate before use | Frequently changing |
| `only-if-cached` | Only use cache | Offline support |

```typescript
// Examples
fetch("/api/data", { cache: "force-cache" }); // Static
fetch("/api/stocks", { cache: "no-store" });   // Live
```
