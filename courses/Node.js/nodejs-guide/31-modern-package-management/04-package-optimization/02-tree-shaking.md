# Tree Shaking

## What You'll Learn

- What tree shaking is and how it works
- How to configure tree shaking
- How to write tree-shakable code
- Common tree shaking pitfalls

## What Is Tree Shaking?

Tree shaking removes unused code from your bundle. It works with ES Modules because `import`/`export` are static — the bundler can determine which exports are used at build time.

```
Before tree shaking:
  import { map, filter, reduce, sortBy, groupBy } from 'lodash-es'
  // Bundle includes ALL 5 functions (and their dependencies)

After tree shaking:
  import { map } from 'lodash-es'
  // Bundle includes ONLY map and its dependencies
```

## Writing Tree-Shakable Code

### Use Named Exports

```js
// GOOD — tree-shakable
export function add(a, b) { return a + b; }
export function subtract(a, b) { return a - b; }

// BAD — not tree-shakable (default export)
export default { add, subtract };
```

### Use ES Module Syntax

```js
// GOOD — tree-shakable
import { useState } from 'react';

// BAD — CommonJS, not tree-shakable
const { useState } = require('react');
```

### Use Modular Imports

```js
// GOOD — only imports what you need
import debounce from 'lodash-es/debounce';

// BAD — imports entire library
import _ from 'lodash';
```

## Configuration

```json
// tsconfig.json
{
  "compilerOptions": {
    "module": "ESNext",
    "moduleResolution": "bundler"
  }
}
```

```json
// package.json
{
  "sideEffects": false
  // Tells bundler: this package has no side effects, safe to tree-shake
}
```

## Side Effects

```js
// If a module has side effects, the bundler cannot remove it
import './styles.css';  // Side effect: injects CSS

// Mark as side effect in package.json
{
  "sideEffects": ["*.css", "./src/polyfills.js"]
}
```

## Verification

```bash
# Check what got tree-shaken
# With webpack
npx webpack --mode production --stats-modules-by-size

# With Rollup
npx rollup -c --environment DEBUG
```

## Next Steps

For code splitting, continue to [Code Splitting](./03-code-splitting.md).
