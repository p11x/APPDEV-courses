# Dynamic Import in Node.js

## What You'll Learn

- What dynamic import is
- Lazy loading modules
- Conditional imports
- Code splitting with dynamic import

## What is Dynamic Import?

**Dynamic import** lets you load modules at runtime using `import()`. Unlike static imports at the top of the file, dynamic imports can be conditional and happen when needed.

## Static vs Dynamic Import

### Static Import (Top-Level)

```javascript
// This runs when the file is first loaded
import heavyModule from './heavy-module.js';
import alwaysNeeded from './needed.js';
```

### Dynamic Import (Runtime)

```javascript
// This runs when the code actually executes
const heavyModule = await import('./heavy-module.js');
```

## Basic Dynamic Import

```javascript
// dynamic-basic.js - Basic dynamic import

console.log('Before dynamic import');

// Dynamic import - loads module when reached
const module = await import('./math.js');

console.log('After dynamic import');

// Use the imported module
console.log(module.add(2, 3));  // 5
console.log(module.PI);         // 3.14159
```

## Conditional Loading

```javascript
// dynamic-conditional.js - Conditional imports

async function loadModule(condition) {
  if (condition === 'math') {
    const math = await import('./math.js');
    return math;
  } else if (condition === 'string') {
    const string = await import('./string.js');
    return string;
  }
}

// Load different modules based on runtime condition
const mod = await loadModule('math');
console.log(mod.add(5, 3));
```

## Lazy Loading

```javascript
// dynamic-lazy.js - Lazy loading heavy modules

// Only load heavy library when user actually needs it
async function handleExport() {
  // Heavy library loaded only when this function is called!
  const { generateReport } = await import('./heavy-report-library.js');
  
  return generateReport();
}

// Application starts fast, loads heavy code only when needed
console.log('App started!');
// ... later
const report = await handleReport(); // Now loads heavy library
```

## Error Handling

```javascript
// dynamic-error.js - Error handling with dynamic imports

async function safeImport(path) {
  try {
    const module = await import(path);
    return { success: true, module };
  } catch (error) {
    return { success: false, error: error.message };
  }
}

// Try to import, handle errors
const result = await safeImport('./nonexistent.js');

if (result.success) {
  console.log('Loaded:', result.module);
} else {
  console.log('Failed:', result.error);
}
```

## Code Example: Dynamic Import Demo

### math.js (module to import)

```javascript
// math.js
export const add = (a, b) => a + b;
export const multiply = (a, b) => a * b;
export const PI = 3.14159;
export default { add, multiply, PI };
```

### string.js (module to import)

```javascript
// string.js
export const capitalize = s => s.toUpperCase();
export const reverse = s => s.split('').reverse().join('');
export default { capitalize, reverse };
```

### main.js (using dynamic import)

```javascript
// main.js - Dynamic import demonstration

console.log('=== Dynamic Import Demo ===\n');

// ─────────────────────────────────────────
// 1. Basic dynamic import
// ─────────────────────────────────────────
console.log('1. Basic Dynamic Import:');

const math = await import('./math.js');
console.log('   2 + 3 =', math.add(2, 3));
console.log('   4 × 5 =', math.multiply(4, 5));

// ─────────────────────────────────────────
// 2. Conditional loading
// ─────────────────────────────────────────
console.log('\n2. Conditional Loading:');

const type = 'string';
let utils;

if (type === 'math') {
  utils = await import('./math.js');
} else if (type === 'string') {
  utils = await import('./string.js');
}

console.log('   Loaded:', utils.default);

// ─────────────────────────────────────────
// 3. Lazy loading pattern
// ─────────────────────────────────────────
console.log('\n3. Lazy Loading:');

async function useMathLater() {
  console.log('   About to load math...');
  const m = await import('./math.js');
  console.log('   Math loaded!', m.PI);
}

useMathLater();

// ─────────────────────────────────────────
// 4. Error handling
// ─────────────────────────────────────────
console.log('\n4. Error Handling:');

async function tryImport(path) {
  try {
    const mod = await import(path);
    console.log('   Success:', Object.keys(mod));
  } catch (err) {
    console.log('   Failed:', err.message);
  }
}

await tryImport('./existing.js');
await tryImport('./does-not-exist.js');
```

## Use Cases

### 1. Code Splitting

Load different parts of your app on demand:

```javascript
// Only load admin panel when needed
async function loadAdmin() {
  const admin = await import('./admin-panel.js');
  admin.init();
}
```

### 2. Optional Features

Load features only if available:

```javascript
async function loadOptional() {
  try {
    const feature = await import('./optional-feature.js');
    feature.enable();
  } catch {
    console.log('Feature not available');
  }
}
```

### 3. Polyfill Loading

Load polyfills conditionally:

```javascript
async function loadPolyfills() {
  if (!globalThis.fetch) {
    await import('node-fetch');
  }
}
```

## Common Mistakes

### Mistake 1: Forgetting await

```javascript
// WRONG - returns a Promise, not the module
const math = import('./math.js');
math.add(2, 3);  // Error!

// CORRECT - await the import
const math = await import('./math.js');
math.add(2, 3);  // Works!
```

### Mistake 2: Using Static Instead of Dynamic

```javascript
// If you always need it, use static import
import math from './math.js';  // Better if always needed

// Only use dynamic if you conditionally load
if (needMath) {
  const math = await import('./math.js');  // Dynamic makes sense
}
```

## Try It Yourself

### Exercise 1: Dynamic Load
Create two modules and dynamically import one based on a condition.

### Exercise 2: Error Handling
Add try/catch to handle failed imports.

### Exercise 3: Lazy Loading
Implement lazy loading for a "heavy" module.

## Next Steps

Now you know about ES Modules. Let's look at useful packages. Continue to [dotenv Package](./../useful-packages/01-dotenv.md).
