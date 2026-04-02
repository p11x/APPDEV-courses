# ES Modules Implementation Details

## What You'll Learn

- ES Modules syntax and semantics
- Static vs dynamic imports
- Module resolution algorithm
- Top-level await and live bindings

## ES Modules Fundamentals

### Enabling ES Modules

```json
// package.json — Enable ES Modules
{
    "type": "module"
}
```

```bash
# Or use .mjs extension
mv app.js app.mjs

# Or use --input-type=module flag
node --input-type=module -e "import { readFile } from 'node:fs/promises'"
```

### Import and Export Syntax

```javascript
// math.js — Exporting from ES Module

// Named exports
export function add(a, b) { return a + b; }
export function subtract(a, b) { return a - b; }
export const PI = 3.14159265359;

// Export list
function multiply(a, b) { return a * b; }
function divide(a, b) { return a / b; }
export { multiply, divide };

// Default export
export default class Calculator {
    add(a, b) { return a + b; }
}

// Re-export from another module
export { helper } from './utils.js';
export * from './constants.js';
```

```javascript
// app.js — Importing ES Modules

// Named imports
import { add, subtract, PI } from './math.js';

// Import with alias
import { add as sum } from './math.js';

// Default import
import Calculator from './math.js';

// Mixed default and named
import Calculator, { add, subtract } from './math.js';

// Namespace import (all exports as object)
import * as math from './math.js';
math.add(2, 3);

// Side-effect only import (no bindings)
import './setup.js';

// Dynamic import (returns Promise)
const module = await import('./math.js');
module.add(2, 3);

// Conditional dynamic import
if (needsFeature) {
    const { feature } = await import('./feature.js');
    feature();
}
```

### Live Bindings

```javascript
// counter.js — Live binding demonstration
export let count = 0;

export function increment() {
    count++; // Exported value changes live
}

// app.js
import { count, increment } from './counter.js';

console.log(count); // 0
increment();
console.log(count); // 1 — live binding updates!

// Note: You cannot reassign imported bindings
// count = 5; // SyntaxError: Assignment to constant variable
```

### Top-Level Await

```javascript
// config.js — Top-level await (no async function wrapper needed)

// Fetch configuration
const response = await fetch('https://config.example.com/app.json');
export const config = await response.json();

// Read file
import { readFile } from 'node:fs/promises';
export const packageJson = JSON.parse(
    await readFile('./package.json', 'utf-8')
);

// Database connection
import { createConnection } from 'node:net';
export const db = await createConnection({ host: 'localhost', port: 5432 });

// Conditional await
export const feature = process.env.FEATURE_ENABLED
    ? await import('./experimental-feature.js')
    : null;
```

### import.meta

```javascript
// import-meta.js — Module metadata

// Current file URL
console.log(import.meta.url);
// file:///C:/projects/myapp/src/utils.js

// __dirname equivalent in ESM
import { fileURLToPath } from 'node:url';
import { dirname } from 'node:path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// Check if running as main module
if (import.meta.main) {
    console.log('Running directly');
}

// Resolve relative to current module
import { resolve } from 'node:url';
const configPath = resolve('./config.json', import.meta.url);
```

## Module Resolution

```
ESM Resolution Algorithm:
─────────────────────────────────────────────
1. Bare specifier (e.g., 'lodash')
   → Look in node_modules/
   → Must have package.json with "exports" or "main"

2. Relative path (e.g., './utils.js')
   → Resolve relative to importing file
   → Must include file extension (.js, .mjs)

3. Absolute path (e.g., '/app/src/utils.js')
   → Use as-is

4. URL (e.g., 'file:///app/src/utils.js')
   → Use as-is

5. Built-in (e.g., 'node:fs')
   → Use node: protocol prefix
```

### Package Exports Field

```json
// package.json — Conditional exports
{
    "name": "my-package",
    "exports": {
        ".": {
            "import": "./dist/index.mjs",
            "require": "./dist/index.cjs",
            "types": "./dist/index.d.ts"
        },
        "./utils": {
            "import": "./dist/utils.mjs",
            "require": "./dist/utils.cjs"
        },
        "./package.json": "./package.json"
    },
    "main": "./dist/index.cjs",
    "module": "./dist/index.mjs",
    "types": "./dist/index.d.ts"
}
```

```javascript
// Consumer usage
import { helper } from 'my-package';         // Uses .mjs
import { helper } from 'my-package/utils';   // Uses utils.mjs
```

## Best Practices

```
ES Modules Best Practices:
─────────────────────────────────────────────
✓ Use "type": "module" in package.json
✓ Always include file extensions in imports
✓ Use node: prefix for built-in modules
✓ Use top-level await for initialization
✓ Use import.meta.url for file paths
✓ Prefer named exports over default exports
✓ Use dynamic import() for conditional loading
✗ Don't mix require() and import in same file
✗ Don't forget file extensions in relative imports
✗ Don't use __dirname/__filename (use import.meta)
```

## Cross-References

- See [CommonJS Deep Dive](./01-commonjs-deep-dive.md) for legacy module system
- See [Interoperability](./03-interoperability-migration.md) for migration strategies
- See [Built-in Modules](../02-built-in-modules/01-fs-path-os-modules.md) for core modules

## Next Steps

Continue to [Interoperability and Migration](./03-interoperability-migration.md) for migration strategies.
