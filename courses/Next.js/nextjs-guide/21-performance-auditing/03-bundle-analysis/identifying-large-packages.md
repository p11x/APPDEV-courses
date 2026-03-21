# Identifying Large Packages

## What You'll Learn
- Find packages taking too much space
- Use alternatives
- Reduce bundle size

## Prerequisites
- Bundle analyzer results

## Do I Need This Right Now?
Large packages slow down your app. Finding them is the first step to optimization.

## Common Culprits

| Package | Size | Alternative |
|---------|------|--------------|
| moment.js | 300KB+ | date-fns, dayjs |
| lodash | 70KB+ | lodash-es |
| axios | 20KB+ | fetch |
| styled-components | 40KB+ | CSS modules, Tailwind |
| chart.js | 500KB+ | Recharts, tinycharts |

## Finding Large Packages

```bash
# List all packages by size
npm install -g bundlephobia-cli
npx bundlephobia
```

## Replacing Large Packages

```typescript
// Instead of moment.js
// npm install date-fns
import { format } from 'date-fns';

format(new Date(), 'yyyy-MM-dd');

// Instead of lodash
// Use native methods or lodash-es
import { debounce } from 'lodash-es';

// Instead of axios
// Use fetch (built into browsers)
fetch('/api/data')
  .then(r => r.json())
  .then(data => console.log(data));
```

## Summary
- Use bundlephobia to find large packages
- Replace heavy libraries with lighter alternatives
- Use tree-shaking (lodash-es)

## Next Steps
- [code-splitting-strategies.md](./code-splitting-strategies.md) — Splitting code
