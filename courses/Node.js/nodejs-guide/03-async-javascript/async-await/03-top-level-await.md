# Top-Level Await

## What You'll Learn

- What top-level await is
- When and where you can use await outside async functions
- Use cases for top-level await
- Limitations and browser compatibility

## What is Top-Level Await?

**Top-level await** allows you to use the `await` keyword at the top level of an ES Module, outside of any async function. Previously, await could only be used inside async functions.

### Before Top-Level Await

```javascript
// OLD - Before top-level await, you needed an async IIFE
(async () => {
  const data = await fetchData();
  console.log(data);
})();
```

### With Top-Level Await

```javascript
// NEW - Top-level await works directly
const data = await fetchData();
console.log(data);
```

## Using Top-Level Await

### Requirements

For top-level await to work:
1. File must be an ES Module (have `"type": "module"` in package.json)
2. The await must be at the module's top level (not inside a function)

### Basic Example

Create a file named `tla-example.js`:

```javascript
// tla-example.js - Top-level await demonstration

// This await works at the top level in ES Modules!
const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));

console.log('Starting...');

// Top-level await - waits for promise before continuing
await delay(1000);
console.log('Waited 1 second!');

// You can await a simple promise
const result = await Promise.resolve('Direct await!');
console.log('Result:', result);
```

## Use Cases

### 1. Loading Configuration Files

```javascript
// config.js - Load config at module load time

import { readFile } from 'fs/promises';

const config = JSON.parse(
  await readFile(new URL('./config.json', import.meta.url))
);

console.log('Config loaded:', config);
export default config;
```

### 2. Database Connections

```javascript
// db.js - Connect to database when module loads

import { Client } from 'pg';

const client = new Client({
  connectionString: process.env.DATABASE_URL
});

// Connect when module loads
await client.connect();

console.log('Database connected!');

export { client };
```

### 3. Environment Variable Loading

```javascript
// env.js - Load environment variables

import { readFile } from 'fs/promises';

// Wait for .env file to be read
const envContent = await readFile('.env', 'utf8');

// Parse environment variables
const env = Object.fromEntries(
  envContent.split('\n')
    .filter(line => line.includes('='))
    .map(line => line.split('='))
);

console.log('Environment loaded');

export default env;
```

### 4. Dynamic Imports

```javascript
// dynamic-import.js - Lazy load modules

// Top-level await can be used with dynamic import
const { default: chalk } = await import('chalk');

console.log(chalk.red('This is red text!'));
```

## Code Example: Complete Top-Level Await Demo

```javascript
// tla-demo.js - Complete demonstration

console.log('=== Top-Level Await Demo ===\n');

// ─────────────────────────────────────────
// Example 1: Simple await at top level
// ─────────────────────────────────────────
console.log('1. Simple Top-Level Await:');

// Helper
const getData = () => new Promise(r => setTimeout(() => r('Data!'), 100));

// This runs when the module loads!
const data = await getData();
console.log('   Loaded:', data);

// ─────────────────────────────────────────
// Example 2: Multiple awaits
// ─────────────────────────────────────────
console.log('\n2. Multiple Awaits:');

const a = await Promise.resolve(1);
const b = await Promise.resolve(2);
const c = await Promise.resolve(3);

console.log('   Sum:', a + b + c);

// ─────────────────────────────────────────
// Example 3: Using with imports
// ─────────────────────────────────────────
console.log('\n3. With Imports:');

// This works because getData is defined above and awaited
const processed = data.toUpperCase();
console.log('   Processed:', processed);
```

## ES Module Setup Required

Remember, top-level await requires an ES Module setup. Make sure your `package.json` has:

```json
{
  "type": "module"
}
```

## Error Handling at Top Level

```javascript
// tla-error.js - Error handling with top-level await

console.log('Starting...');

try {
  // This might fail
  const response = await fetch('https://invalid-url-that-does-not-exist.example');
  console.log('Response:', response);
} catch (error) {
  console.log('Error caught at top level:', error.message);
}

console.log('Module execution continued after error');
```

## Common Mistakes

### Mistake 1: Using Top-Level Await in CommonJS

```javascript
// WRONG - won't work in CommonJS files
const data = await fetchData();  // SyntaxError in CommonJS!

// Make sure your package.json has "type": "module"
```

### Mistake 2: Using Top-Level Await in Function

Top-level await only works at the top level of the module, not inside functions:

```javascript
// This is fine - top level
const data = await fetchData();

// This is ALSO fine - inside async function
async function getData() {
  const data = await fetchData();
  return data;
}
```

### Mistake 3: Forgetting Module is Async

The module itself becomes async when using top-level await. Importers need to handle this:

```javascript
// my-module.js
export const data = await fetchData();

// importer.js
// The imported value is already resolved when imported
import { data } from './my-module.js';
// data is ready to use - no await needed!
```

## Top-Level Await and Imports

When you use top-level await:

1. The importing module waits for the exporting module to finish loading
2. The imported values are fully resolved before the importer continues

```javascript
// slow-module.js
export const value = await new Promise(r => setTimeout(() => r('slow'), 2000));

// fast-module.js
import { value } from './slow-module.js';

// This console.log waits for slow-module to finish!
console.log(value);  // Prints 'slow' after 2 seconds
```

## When to Use Top-Level Await

**Use it for:**
- Loading configuration files at startup
- Initializing database connections
- One-time setup operations

**Avoid it for:**
- Code that might fail and would block the entire module
- Code that doesn't need to complete before the module is used

## Try It Yourself

### Exercise 1: Config Loading
Create a module that loads a JSON config file using top-level await.

### Exercise 2: Multiple Imports
Create two modules that use top-level await and import them from a third module.

### Exercise 3: Error Handling
Add try/catch to handle errors in your top-level await code.

## Next Steps

Congratulations! You've completed the Async JavaScript section. Now let's learn about npm and packages. Continue to [Package.json Basics](../04-npm-and-packages/npm-basics/01-package-json.md).
