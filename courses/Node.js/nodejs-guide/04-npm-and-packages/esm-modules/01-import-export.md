# Import and Export in ES Modules

## What You'll Learn

- Named exports and imports
- Default exports and imports
- Namespace imports
- Combining export types

## ES Modules Overview

ES Modules (ESM) are the standard way to organize JavaScript code into reusable files. You use:
- `import` to bring code from other files
- `export` to make code available to other files

## Named Exports

### Exporting

```javascript
// math.js - Named exports

// Export individual items
export const PI = 3.14159;
export const E = 2.71828;

// Export functions
export function add(a, b) {
  return a + b;
}

export function subtract(a, b) {
  return a - b;
}

// Export class
export class Calculator {
  multiply(a, b) {
    return a * b;
  }
}
```

### Importing Named Exports

```javascript
// Import specific items
import { add, subtract } from './math.js';

console.log(add(5, 3));      // 8
console.log(subtract(10, 4)); // 6

// Import with aliases
import { add as sum } from './math.js';
console.log(sum(2, 3));  // 5

// Import all as namespace
import * as Math from './math.js';
console.log(Math.add(1, 2));  // 3
console.log(Math.PI);         // 3.14159
```

## Default Exports

### Exporting

```javascript
// config.js - Default export

// Default export - one per file
const config = {
  port: 3000,
  host: 'localhost',
  env: 'development'
};

export default config;

// You can also use export default with values directly
export default {
  port: 3000,
  host: 'localhost'
};
```

### Importing Default Exports

```javascript
// Import default export
import config from './config.js';

console.log(config.port);  // 3000

// Default can be imported with any name
import anything from './config.js';
console.log(anything.port);  // 3000
```

## Combining Named and Default

```javascript
// combined.js - Both named and default exports

const API_URL = 'https://api.example.com';

function fetchData() {
  return fetch(API_URL);
}

function postData(data) {
  return fetch(API_URL, { method: 'POST', body: JSON.stringify(data) });
}

// Default export
export default {
  fetchData,
  postData
};

// Named exports
export { API_URL };
export { fetchData, postData };
```

Importing:

```javascript
// Import default
import api from './combined.js';
api.fetchData();

// Import named
import { API_URL, fetchData } from './combined.js';

// Import both
import api, { API_URL } from './combined.js';
```

## Code Example: Complete Module Demo

### Creating Modules

```javascript
// utils.js - Utility functions module

// Named exports
export function formatDate(date) {
  return date.toISOString().split('T')[0];
}

export function capitalize(str) {
  return str.charAt(0).toUpperCase() + str.slice(1);
}

// Default export
export default {
  formatDate,
  capitalize
};
```

```javascript
// user.js - User module with mixed exports

const defaultUser = {
  name: 'Guest',
  role: 'user'
};

function createUser(name, role = 'user') {
  return { name, role };
}

function isAdmin(user) {
  return user.role === 'admin';
}

// Default export
export default createUser;

// Named exports
export { defaultUser, isAdmin };
```

### Using the Modules

```javascript
// main.js - Using the modules

// Import from utils.js
import { formatDate, capitalize } from './utils.js';
import utils from './utils.js';

console.log(formatDate(new Date()));  // "2024-01-01"
console.log(capitalize('hello'));     // "Hello"
console.log(utils.capitalize('world')); // "World"

// Import from user.js
import createUser, { defaultUser, isAdmin } from './user.js';

const user = createUser('Alice', 'admin');
console.log(user);           // { name: 'Alice', role: 'admin' }
console.log(isAdmin(user)); // true

console.log(defaultUser);   // { name: 'Guest', role: 'user' }
```

## Common Mistakes

### Mistake 1: Missing File Extension

```javascript
// WRONG - missing .js extension
import { add } from './math';

// CORRECT - include .js extension
import { add } from './math.js';
```

### Mistake 2: Confusing Default and Named

```javascript
// export.js
export const value = 1;          // Named export
export default 2;               // Default export

// import.js
import value from './export.js';    // Imports default (2)
import { value } from './export.js'; // Imports named (1)
```

### Mistake 3: Circular Dependencies

Avoid circular imports when possible. If needed, use lazy imports:

```javascript
// Instead of circular imports, consider:
async function loadModule() {
  const module = await import('./heavy-module.js');
  return module;
}
```

## Try It Yourself

### Exercise 1: Create a Math Module
Create a math module with add, subtract, multiply, divide as named exports.

### Exercise 2: Create a Config Module
Create a config module with a default export and named exports.

### Exercise 3: Import and Use
Import your modules and use them in a main file.

## Next Steps

Now you understand imports and exports. Let's learn about module resolution. Continue to [Module Resolution](./02-module-resolution.md).
