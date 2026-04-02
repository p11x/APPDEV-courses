# Package Optimization: Tree Shaking and Bundle Size

## What You'll Learn

- Tree shaking effectiveness
- Bundle size analysis techniques
- Dependency optimization strategies
- Loading performance improvements

## Tree Shaking

```javascript
// Good: Named exports enable tree shaking
// utils.js
export function add(a, b) { return a + b; }
export function subtract(a, b) { return a - b; }
export function multiply(a, b) { return a * b; }

// Consumer: Only imports what's needed
import { add } from './utils.js';
// Tree shaker can eliminate subtract and multiply

// Bad: Default export prevents tree shaking
// utils.js
export default { add, subtract, multiply };

// Consumer: Must import entire object
import utils from './utils.js';
```

## Bundle Size Analysis

```bash
# Analyze bundle size
npx bundlephobia express lodash axios

# Check package size before installing
npx package-size express
npx package-size lodash

# Analyze node_modules size
npx node-modules-size

# Find largest packages
du -sh node_modules/* | sort -rh | head -20
```

## Dependency Optimization

```javascript
// Import specific functions (smaller bundle)
import debounce from 'lodash/debounce'; // ~1KB
import { debounce } from 'lodash';       // ~70KB (entire lodash)

// Use lighter alternatives
// Instead of moment.js (~300KB), use date-fns (~20KB for what you need)
import { format } from 'date-fns';

// Instead of lodash (~70KB), use native methods
// Native: array.filter().map().reduce()
// Lodash: _.flow([_.filter, _.map, _.reduce])
```

## Best Practices Checklist

- [ ] Use named exports for tree shaking
- [ ] Import specific functions from large libraries
- [ ] Analyze bundle size regularly
- [ ] Use lighter alternatives when possible
- [ ] Monitor dependency size in CI/CD

## Cross-References

- See [Dependency Management](../04-dependency-management/01-tree-analysis.md) for dependencies
- See [Security](../11-package-security/01-supply-chain.md) for security
- See [Testing](../13-package-testing/01-unit-testing.md) for testing

## Next Steps

Continue to [Package Testing](../13-package-testing/01-unit-testing.md) for testing.
