# Node.js vs Browser

## What You'll Learn

- Key differences between Node.js and browser JavaScript
- Global objects that exist in each environment
- APIs available only in Node.js
- How to write code that works in both environments

## Understanding the Two Environments

When you write JavaScript, you might write it for a web browser (like Chrome, Firefox, Safari) or for Node.js (server-side). While both environments use the same JavaScript language, they have important differences.

### The Browser Environment

In a web browser, JavaScript runs in a **sandbox** (a restricted environment). The browser provides:
- Access to the webpage (DOM - Document Object Model)
- User interactions (clicks, keyboard input)
- Network requests (fetch, XMLHttpRequest)
- Visual display (rendering HTML/CSS)

### The Node.js Environment

Node.js runs JavaScript outside the browser, giving you:
- Access to the file system (reading/writing files)
- Network capabilities (creating servers, making HTTP requests)
- Command-line interface tools
- Access to the operating system

## Code Example: Comparing Environments

Create a file named `environment-check.js`:

```javascript
// This script checks what APIs are available in Node.js vs browsers

console.log('=== Global Objects Check ===\n');

// These exist in Node.js but NOT in browsers
console.log('Node.js specific globals:');
console.log('- global:', typeof global);           // The global namespace object
console.log('- process:', typeof process);         // Node.js process info
console.log('- __dirname:', typeof __dirname);      // Current directory path
console.log('- __filename:', typeof __filename);     // Current file path
console.log('- require:', typeof require);           // CommonJS require (not in ESM)

console.log('\n=== Browser vs Node.js Objects ===\n');

// These exist in browsers but NOT in Node.js (normally)
console.log('Browser-only globals (may be undefined in Node.js):');
console.log('- window:', typeof window);             // Browser window object
console.log('- document:', typeof document);        // HTML DOM
console.log('- navigator:', typeof navigator);       // Browser info
console.log('- fetch:', typeof fetch);              // Available in Node.js v18+

// In Node.js, we have different ways to access similar functionality
console.log('\n=== Node.js Alternatives ===\n');
console.log('In Node.js, use these instead:');
console.log('- process.env for environment variables');
console.log('- fs module for file operations');
console.log('- http module for network requests');
console.log('- console for logging (same as browser)');

console.log('\n=== Current Environment Info ===\n');
// Process gives us Node.js version and platform info
console.log('Node.js version:', process.version);
console.log('Operating system:', process.platform);
console.log('Architecture:', process.arch);
```

Run this with:
```bash
node environment-check.js
```

## Key Differences Explained

### 1. The Global Object

In browsers, the global object is `window`. In Node.js, it's `global`.

```javascript
// In a browser console:
console.log(window === window.window); // true

// In Node.js:
console.log(global === global.global); // true (but global !== globalThis in some contexts)
```

Modern Node.js (v12+) provides `globalThis` as a unified global that works in both Node.js and browsers.

### 2. File System Access

Browsers cannot directly access your computer's files (for security). Node.js can read and write files freely.

```javascript
// Node.js can do this:
import { readFile } from 'fs/promises';

const content = await readFile('my-file.txt', 'utf-8');
console.log(content);
```

In browsers, you'd need to use the File System Access API (limited) or upload files through a form.

### 3. Module Systems

**Browsers** (traditionally): No built-in module system
**Node.js**: Two module systems:
- CommonJS: `const fs = require('fs')`
- ES Modules: `import fs from 'fs'`

Modern browsers support ES Modules natively, but you'll need to use `<script type="module">`.

### 4. Network Requests

Both environments now support `fetch`, but Node.js didn't have it until version 18.

```javascript
// This works in browsers and Node.js v18+:
const response = await fetch('https://api.example.com/data');
const data = await response.json();
console.log(data);
```

In older Node.js versions, you would use the `http` module or a library like `axios`.

### 5. Timing Functions

Both support `setTimeout`, `setInterval`, but browsers have `requestAnimationFrame` (for animations) which Node.js doesn't have natively.

## Code Example: Writing Cross-Environment Code

Create `universal-code.js`:

```javascript
// This code works in both Node.js and browsers

// Use globalThis for the global object (available in both)
const globalObject = typeof globalThis !== 'undefined' 
  ? globalThis 
  : (typeof window !== 'undefined' ? window : global);

// Check if we're in Node.js
const isNodeJS = typeof process !== 'undefined' && 
                 process.versions?.node !== undefined;

console.log('Running in Node.js:', isNodeJS);
console.log('Global object available:', !!globalObject);

// Use fetch if available (Node v18+ or browsers)
async function fetchData(url) {
  if (typeof fetch === 'undefined') {
    // Node.js without fetch - use http module as fallback
    const http = await import('http');
    return new Promise((resolve, reject) => {
      http.get(url, (res) => {
        let data = '';
        res.on('data', chunk => data += chunk);
        res.on('end', () => resolve(data));
      }).on('error', reject);
    });
  }
  // Use native fetch
  return fetch(url).then(r => r.text());
}

// For console.log, both environments support it
console.log('Console logging works in both!');
```

## Common Mistakes

### Mistake 1: Using Browser APIs in Node.js
```javascript
// WRONG in Node.js:
document.getElementById('myDiv');  // Error: document is not defined
window.myVar = 'test';              // Error: window is not defined

// CORRECT in Node.js:
console.log(process.env.MY_VAR);    // Access environment variables
```

### Mistake 2: Using Node.js APIs in Browsers
```javascript
// WRONG in browsers:
const data = fs.readFileSync('file.txt');  // Error: fs is not defined
const server = http.createServer(...);     // Error: http is not defined
```

### Mistake 3: Assuming __filename and __dirname Work Everywhere
These are only available in CommonJS. In ES Modules, you need to derive them:

```javascript
// In ES Modules (import.meta.url):
import { fileURLToPath } from 'url';
import { dirname } from 'path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
```

## Try It Yourself

### Exercise 1: Detect Your Environment
Write a script that prints different messages depending on whether it's running in Node.js or a browser.

### Exercise 2: Create a Universal Module
Create a function that uses `console.log` and works regardless of environment.

### Exercise 3: Check API Availability
Write a script that checks if specific APIs (like `fetch`, `process`, `document`, `window`) are defined and reports which environment you're in.

## Next Steps

Now that you understand the differences between Node.js and browsers, let's get Node.js installed on your machine. Continue to [Installing nvm](../installation/01-install-nvm.md) to learn how to install and manage Node.js versions.
