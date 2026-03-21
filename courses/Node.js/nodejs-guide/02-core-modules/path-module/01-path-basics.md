# Path Module Basics

## What You'll Learn

- How to use the path module to work with file paths
- Common path functions: join, resolve, dirname, basename
- How to handle paths across different operating systems
- Why path handling matters for cross-platform compatibility

## Why Use the Path Module?

File paths look different on different operating systems:
- **Windows**: `C:\Users\Alice\Documents\file.txt`
- **macOS/Linux**: `/Users/Alice/Documents/file.txt`

The **path** module provides functions that automatically handle these differences, making your code work on any operating system.

## Importing the Path Module

```javascript
// Import specific functions from the path module
import { join, resolve, dirname, basename, extname } from 'path';
```

## Common Path Functions

### join - Combine Path Segments

`join` combines multiple path segments into one, using the correct separator for your OS:

```javascript
// path-join.js - Using path.join

import { join } from 'path';

// Combine path segments - automatically handles separators
const fullPath = join('folder', 'subfolder', 'file.txt');
console.log('Joined path:', fullPath);

// On Windows: folder\subfolder\file.txt
// On macOS/Linux: folder/subfolder/file.txt

// More examples
const configPath = join(__dirname, 'config', 'settings.json');
const dataPath = join(process.cwd(), 'data', 'users.csv');

console.log('Config path:', configPath);
console.log('Data path:', dataPath);
```

### resolve - Get Absolute Path

`resolve` converts a relative path to an absolute path:

```javascript
// path-resolve.js - Using path.resolve

import { resolve } from 'path';

// Resolve to absolute path
const absolutePath = resolve('myfile.txt');
console.log('Absolute:', absolutePath);
// If in /home/user/project, resolves to: /home/user/project/myfile.txt

// Resolve with multiple segments
const path2 = resolve('folder', 'subfolder', 'file.txt');
console.log('Absolute2:', path2);

// '..' goes up one directory
const path3 = resolve('folder', '..', 'file.txt');
console.log('Goes up:', path3);
```

### dirname - Get Folder Path

`dirname` returns the directory portion of a path:

```javascript
// path-dirname.js - Using path.dirname

import { dirname } from 'path';

const filePath = '/Users/Alice/Documents/project/file.txt';

const dir = dirname(filePath);
console.log('Directory:', dir);
// Output: /Users/Alice/Documents/project
```

### basename - Get File Name

`basename` returns the last portion of a path (usually the filename):

```javascript
// path-basename.js - Using path.basename

import { basename } from 'path';

const filePath = '/Users/Alice/Documents/project/file.txt';

// Get full filename
const name = basename(filePath);
console.log('Basename:', name);
// Output: file.txt

// Get filename without extension
const nameNoExt = basename(filePath, '.txt');
console.log('Without ext:', nameNoExt);
// Output: file
```

### extname - Get File Extension

`extname` returns the extension portion of a path:

```javascript
// path-extname.js - Using path.extname

import { extname } from 'path';

const filePath = '/Users/Alice/Documents/project/file.txt';

const ext = extname(filePath);
console.log('Extension:', ext);
// Output: .txt

// Works with other extensions
console.log(extname('image.jpeg'));   // .jpeg
console.log(extname('script.js'));    // .js
console.log(extname('noextension'));  // '' (empty)
```

## Code Example: Complete Path Operations

Here's a comprehensive example:

```javascript
// path-demo.js - Complete path module demonstration

import { 
  join, 
  resolve, 
  dirname, 
  basename, 
  extname,
  parse,
  format
} from 'path';

console.log('=== Path Module Demo ===\n');

// Example path
const filePath = '/Users/Alice/projects/my-app/src/index.js';

console.log('Original path:', filePath);

// 1. Get directory name
console.log('\n1. dirname():', dirname(filePath));
// /Users/Alice/projects/my-app/src

// 2. Get base name
console.log('2. basename():', basename(filePath));
// index.js

// 3. Get extension
console.log('3. extname():', extname(filePath));
// .js

// 4. Parse path into object
console.log('\n4. parse():');
const parsed = parse(filePath);
console.log(parsed);
/*
{
  root: '/',
  dir: '/Users/Alice/projects/my-app/src',
  base: 'index.js',
  ext: '.js',
  name: 'index'
}
*/

// 5. Create path from object
console.log('\n5. format():');
const newPath = format({
  dir: '/Users/Alice/projects',
  name: 'app',
  ext: '.js'
});
console.log('New path:', newPath);
// /Users/Alice/projects/app.js

// 6. Join paths
console.log('\n6. join():');
const joined = join('folder', 'subfolder', 'file.txt');
console.log('Joined:', joined);

// 7. Resolve to absolute
console.log('\n7. resolve():');
const absolute = resolve('file.txt');
console.log('Absolute:', absolute);

// 8. __dirname equivalent in ES Modules
// In ES Modules, we use import.meta.url instead
console.log('\n8. ES Module __dirname equivalent:');
// This would be: new URL('.', import.meta.url).pathname
// But we can't use import.meta.url in this context without a real module
console.log('(See next file for import.meta.url details)');
```

## Path with __dirname

In CommonJS, `__dirname` is available automatically. In ES Modules, you need to derive it:

```javascript
// Using __dirname in ES Modules
import { dirname } from 'path';
import { fileURLToPath } from 'url';

// Get __dirname equivalent
const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

console.log('Current directory:', __dirname);
```

## Common Mistakes

### Mistake 1: Using Hardcoded Slashes

```javascript
// WRONG - won't work on all systems
const path = 'folder/subfolder/file.txt';  // Wrong on Windows
const path2 = 'folder\\subfolder\\file.txt';  // Wrong on macOS/Linux

// CORRECT - use path.join
const path3 = join('folder', 'subfolder', 'file.txt');  // Works everywhere!
```

### Mistake 2: Forgetting path.join with __dirname

```javascript
// WRONG - might not work correctly
const filePath = __dirname + '/data/file.txt';

// CORRECT - use join
const filePath = join(__dirname, 'data', 'file.txt');
```

### Mistake 3: Not Handling ES Module Paths

In ES Modules, `__dirname` doesn't exist. Always derive it properly.

## Try It Yourself

### Exercise 1: Path Parser
Create a script that takes a file path and prints all its parts (directory, filename, extension).

### Exercise 2: Cross-Platform Path Builder
Use `path.join` to build a path to config.json in a config folder, regardless of OS.

### Exercise 3: Get ES Module Directory
Create an ES Module script that properly gets and prints the current directory.

## Next Steps

Now you understand basic path operations. Let's learn about URL paths and how they differ from file system paths. Continue to [URL vs Path](./02-url-vs-path.md).
