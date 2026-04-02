# CommonJS Module System Deep Dive

## What You'll Learn

- CommonJS module loading mechanism
- require/exports patterns and internals
- Module caching behavior
- Circular dependency handling

## CommonJS Fundamentals

### How require() Works

```
require() Resolution Order:
─────────────────────────────────────────────
1. Module is a core module? → Return cached core module
2. Path starts with './' or '../' or '/'?
   → Resolve as file path
   a. File exists? → Load file
   b. Add .js → Check
   c. Add .json → Check
   d. Add .node → Check (native addon)
   e. Treat as directory → Look for package.json "main"
      → Then look for index.js
3. Look in node_modules/ directories
   → Search up directory tree until found
4. Module not found → throw MODULE_NOT_FOUND
```

### Module Wrapper

```javascript
// Every CommonJS file is wrapped in a function
(function(exports, require, module, __filename, __dirname) {
    // Your code runs here
    // exports = module.exports = {}
});

// These are available automatically:
// exports      — shortcut to module.exports
// require()    — load other modules
// module       — current module metadata
// __filename   — absolute path to current file
// __dirname    — directory of current file
```

### Creating and Exporting Modules

```javascript
// math.js — Exporting functions

// Method 1: Add properties to exports
exports.add = (a, b) => a + b;
exports.subtract = (a, b) => a - b;
exports.multiply = (a, b) => a * b;

// Method 2: Replace entire module.exports
module.exports = {
    add(a, b) { return a + b; },
    subtract(a, b) { return a - b; },
    multiply(a, b) { return a * b; },
};

// Method 3: Export a class
module.exports = class Calculator {
    add(a, b) { return a + b; }
    subtract(a, b) { return a - b; }
};

// Method 4: Export a single function
module.exports = function compute(a, b, op) {
    switch (op) {
        case '+': return a + b;
        case '-': return a - b;
        case '*': return a * b;
        default: throw new Error('Unknown operation');
    }
};
```

```javascript
// app.js — Importing modules

// Import entire module
const math = require('./math');
console.log(math.add(2, 3));

// Destructure specific exports
const { add, subtract } = require('./math');
console.log(add(5, 3));

// Import JSON
const config = require('./config.json');
console.log(config.port);

// Import core module
const path = require('path');
const fs = require('fs');
```

## Module Caching

```javascript
// counter.js — Demonstrates module caching
let count = 0;

module.exports = {
    increment() { count++; return count; },
    getCount() { return count; },
};
```

```javascript
// app.js — Module is cached (singleton behavior)
const counter1 = require('./counter');
const counter2 = require('./counter');

counter1.increment(); // count = 1
counter1.increment(); // count = 2
console.log(counter2.getCount()); // 2 — same instance!

// Verify they're the same object
console.log(counter1 === counter2); // true

// Clear cache (rare, but possible)
delete require.cache[require.resolve('./counter')];
const fresh = require('./counter');
console.log(fresh.getCount()); // 0 — fresh instance
```

### Module Cache Internals

```javascript
// Inspect the require cache
console.log(Object.keys(require.cache).length); // Number of loaded modules

// Each entry contains:
// - filename: absolute path
// - loaded: boolean
// - children: array of required modules
// - exports: the module's exports

// Find a cached module
const modulePath = require.resolve('./math');
const cached = require.cache[modulePath];
console.log(cached.filename);  // Full path
console.log(cached.loaded);    // true
console.log(cached.exports);   // Module exports
```

## Circular Dependencies

```javascript
// a.js — Circular dependency example
const b = require('./b');

module.exports = {
    name: 'module-a',
    getBName() { return b.name; },
    getBMethod() { return b.greet(); },
};

// b.js
const a = require('./a'); // a.js hasn't finished loading yet!

module.exports = {
    name: 'module-b',
    greet() { return `Hello from ${this.name}`; },
    getAName() { return a.name; }, // Works — a.name is set
};
```

```javascript
// app.js — Circular dependency behavior
const a = require('./a');
console.log(a.getBName()); // 'module-b' — works
console.log(a.getBMethod()); // 'Hello from module-b' — works
```

## Conditional and Dynamic Loading

```javascript
// Lazy loading — require only when needed
function getParser(format) {
    if (format === 'json') {
        return require('./parsers/json');
    } else if (format === 'xml') {
        return require('./parsers/xml');
    }
    throw new Error(`Unknown format: ${format}`);
}

// Conditional loading based on environment
const config = process.env.NODE_ENV === 'production'
    ? require('./config.prod')
    : require('./config.dev');
```

## Best Practices

```
CommonJS Best Practices:
─────────────────────────────────────────────
✓ Use module.exports for single export
✓ Use exports.X for multiple named exports
✓ Don't mix exports.X and module.exports
✓ Avoid circular dependencies when possible
✓ Use require.resolve() to check if module exists
✓ Clear require.cache only in tests
✗ Don't use require() inside tight loops
✗ Don't rely on module load order
✗ Don't modify exports after module loads
```

## Cross-References

- See [ES Modules](./02-es-modules-implementation.md) for modern module syntax
- See [Interoperability](./03-interoperability-migration.md) for migration strategies
- See [Built-in Modules](../02-built-in-modules/01-fs-path-os-modules.md) for core modules

## Next Steps

Continue to [ES Modules Implementation](./02-es-modules-implementation.md) for modern module syntax.
