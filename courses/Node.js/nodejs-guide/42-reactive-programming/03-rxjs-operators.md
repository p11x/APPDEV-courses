# RxJS Operators

## What You'll Learn

- Transformation operators
- Filtering operators
- Combination operators
- Utility operators

---

## Common Operators

```typescript
import { map, filter, reduce, switchMap, concatMap, mergeMap } from 'rxjs/operators';

// Transform
source$.pipe(map(x => x * 2));

// Filter
source$.pipe(filter(x => x > 5));

// Combine
source$.pipe(reduce((acc, x) => acc + x, 0));

// Flatten
source$.pipe(switchMap(x => of(x))); // Cancel previous
source$.pipe(concatMap(x => of(x))); // Wait for completion
source$.pipe(mergeMap(x => of(x))); // Parallel
```