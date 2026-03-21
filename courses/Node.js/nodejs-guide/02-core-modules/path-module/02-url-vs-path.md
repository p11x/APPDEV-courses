# URL vs File Path in Node.js

## What You'll Learn

- The difference between file URLs and file system paths
- How to use import.meta.url in ES Modules
- Converting between URLs and paths using fileURLToPath
- Practical use cases for URL handling

## Understanding URLs vs Paths

In Node.js, especially when working with ES Modules, you'll encounter two different ways to represent file locations:

### File System Paths (Strings)
```
/Users/Alice/projects/my-app/src/index.js
C:\Users\Alice\projects\my-app\src\index.js
```

### File URLs (Special String Format)
```
file:///Users/Alice/projects/my-app/src/index.js
file:///C:/Users/Alice/projects/my-app/src/index.js
```

## Why Does This Matter?

When using ES Modules (`import`), Node.js internally uses file URLs. This is especially important when:
- Getting the current file's directory
- Resolving module paths
- Working with dynamic imports

## Using import.meta.url

`import.meta.url` is a special ES Module feature that contains the URL of the current module file.

### Getting Current File Path

```javascript
// import-meta-url.js - Using import.meta.url

import { fileURLToPath } from 'url';
import { dirname, join } from 'path';

// Get the URL of this file
console.log('import.meta.url:', import.meta.url);
// Output: file:///path/to/your/file.js

// Convert URL to file system path
const currentFilePath = fileURLToPath(import.meta.url);
console.log('File path:', currentFilePath);
// Output: /path/to/your/file.js (or C:\path\to\your\file.js on Windows)

// Get the directory containing this file
const currentDir = dirname(currentFilePath);
console.log('Directory:', currentDir);
```

### Getting __dirname Equivalent

In ES Modules, there's no `__dirname`. Here's how to create it:

```javascript
// esm-dirname.js - Creating __dirname in ES Modules

import { fileURLToPath } from 'url';
import { dirname } from 'path';

// Create __dirname equivalent for ES Modules
const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

console.log('__filename:', __filename);
console.log('__dirname:', __dirname);

// Now you can use it like in CommonJS
import { join } from 'path';
const configPath = join(__dirname, 'config', 'settings.json');
console.log('Config path:', configPath);
```

## Working with URL Objects

Node.js provides the built-in URL class for parsing and handling URLs:

```javascript
// url-object.js - Using the URL class

// Parse a URL string
const myUrl = new URL('https://example.com:8080/path/name?query=value#hash');

console.log('=== URL Parts ===');
console.log('href:', myUrl.href);
console.log('protocol:', myUrl.protocol);  // https:
console.log('hostname:', myUrl.hostname);  // example.com
console.log('port:', myUrl.port);          // 8080
console.log('pathname:', myUrl.pathname);  // /path/name
console.log('search:', myUrl.search);       // ?query=value
console.log('hash:', myUrl.hash);          // #hash

// Modify URL parts
myUrl.searchParams.append('newparam', 'newvalue');
console.log('Modified search:', myUrl.search);
```

## File URLs Explained

File URLs have a specific format:

```
file:///absolute/path/to/file
```

Key points:
- `file://` is the protocol
- Three slashes for absolute paths (not two!)
- On Windows, drive letter becomes: `file:///C:/path/to/file`

### Converting Path to URL

```javascript
// path-to-url.js - Converting file paths to URLs

import { pathToFileURL, fileURLToPath } from 'url';

// Convert file path to file URL
const filePath = '/Users/Alice/projects/app/src/index.js';
const fileUrl = pathToFileURL(filePath);

console.log('Original path:', filePath);
console.log('As URL:', fileUrl.href);
// file:///Users/Alice/projects/app/src/index.js

// On Windows
const windowsPath = 'C:\\Users\\Alice\\projects\\app\\src\\index.js';
const windowsUrl = pathToFileURL(windowsPath);
console.log('Windows URL:', windowsUrl.href);
// file:///C:/Users/Alice/projects/app/src/index.js

// Convert back
const backToPath = fileURLToPath(fileUrl);
console.log('Back to path:', backToPath);
```

## Code Example: Complete URL/Path Handling

Here's a comprehensive example:

```javascript
// url-path-demo.js - Complete URL and path handling

import { 
  fileURLToPath, 
  pathToFileURL,
  URL 
} from 'url';
import { dirname, join, resolve } from 'path';

console.log('=== URL vs Path Demo ===\n');

// 1. Get current module's directory
console.log('1. Current module directory:');
const currentFile = fileURLToPath(import.meta.url);
const currentDir = dirname(currentFile);
console.log('   File:', currentFile);
console.log('   Dir:', currentDir);

// 2. Parse a web URL
console.log('\n2. Parsing web URL:');
const webUrl = new URL('https://api.example.com:3000/users/123?sort=name');
console.log('   Full URL:', webUrl.href);
console.log('   Host:', webUrl.host);
console.log('   Path:', webUrl.pathname);
console.log('   ID:', webUrl.pathname.split('/')[2]);

// 3. Create a file URL from path
console.log('\n3. Path to URL:');
const myPath = resolve('config.json');
const myUrl = pathToFileURL(myPath);
console.log('   Path:', myPath);
console.log('   URL:', myUrl.href);

// 4. Working with import paths
console.log('\n4. Module resolution:');
// When you import './module.js', Node.js resolves it to a file URL internally
// import './data.json' becomes file:///path/to/data.json

// 5. Query parameters
console.log('\n5. URL Query Parameters:');
const apiUrl = new URL('https://api.example.com/data');
apiUrl.searchParams.set('page', '1');
apiUrl.searchParams.set('limit', '10');
apiUrl.searchParams.set('sort', 'name');
console.log('   URL:', apiUrl.href);
console.log('   Page:', apiUrl.searchParams.get('page'));
```

## Common Mistakes

### Mistake 1: Using __dirname in ES Modules

```javascript
// WRONG - __dirname doesn't exist in ES Modules
const configPath = __dirname + '/config.json';

// CORRECT - derive it from import.meta.url
import { fileURLToPath } from 'url';
import { dirname } from 'path';
const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const configPath = join(__dirname, 'config.json');
```

### Mistake 2: Hardcoding Path Separators

```javascript
// WRONG - different on Windows vs Unix
const path = __dirname + '/subfolder/file.txt';

// CORRECT - use join
const path = join(__dirname, 'subfolder', 'file.txt');
```

### Mistake 3: Confusing File URLs with Web URLs

```javascript
// This is a file URL (points to a file)
const fileUrl = 'file:///path/to/file.txt';

// This is a web URL (points to a website)
const webUrl = 'https://example.com/path';

console.log(new URL(fileUrl).protocol);  // file:
console.log(new URL(webUrl).protocol);    // https:
```

## Try It Yourself

### Exercise 1: Create __dirname
Write an ES Module that properly creates and prints its own directory.

### Exercise 2: URL Parameter Parser
Create a script that takes a URL string, parses it, and prints each query parameter.

### Exercise 3: Path/URL Converter
Create functions that convert between file paths and file URLs, handling both Windows and Unix paths.

## Next Steps

Now you understand path handling. Let's explore the OS module to get system information. Continue to [OS Module - System Info](../os-module/01-os-info.md).
