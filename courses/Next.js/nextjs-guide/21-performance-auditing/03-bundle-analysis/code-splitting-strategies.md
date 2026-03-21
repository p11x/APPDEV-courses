# Code Splitting Strategies

## What You'll Learn
- Split code for better performance
- Lazy load components
- Optimize imports

## Prerequisites
- Understanding of bundles

## Do I Need This Right Now?
Code splitting loads JavaScript only when needed. Essential for fast initial page load.

## Dynamic Imports

```typescript
// Lazy load heavy component
import dynamic from 'next/dynamic';

const HeavyChart = dynamic(
  () => import('./components/Chart'),
  { 
    loading: () => <p>Loading...</p>,
    ssr: false // Disable SSR for client-only
  }
);

// Usage
export default function Dashboard() {
  return (
    <div>
      <h1>Dashboard</h1>
      <HeavyChart data={data} />
    </div>
  );
}
```

## Route-Based Splitting

Next.js does this automatically:
- Each page = separate JavaScript file
- Only loads code for current page

## Third-Party Libraries

```typescript
// Only load when needed
const ReactMarkdown = dynamic(
  () => import('react-markdown'),
  { ssr: false }
);
```

## Summary
- Use dynamic imports for heavy components
- Next.js automatically splits routes
- Lazy load third-party libraries
- Disable SSR when not needed

## Next Steps
- [what-is-ci-cd.md](../22-ci-cd/01-github-actions-basics/what-is-ci-cd.md) — CI/CD basics
