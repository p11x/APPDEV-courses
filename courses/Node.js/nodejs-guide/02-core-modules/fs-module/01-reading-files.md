# Reading Files in Node.js

## What You'll Learn

- How to read files using the fs module
- The difference between synchronous and asynchronous file reading
- How to use the promises API with async/await
- Error handling when reading files

## The fs Module

Node.js includes a built-in module called `fs` (File System) that lets you work with files and directories. You can read, write, copy, delete, and more.

### Importing the fs Module

There are two ways to import fs in modern Node.js:

**Using the promises API (recommended):**
```javascript
import { readFile } from 'fs/promises';
```

**Using the callback API:**
```javascript
import { readFile } from 'fs';
```

We'll focus on the promises API since it works better with async/await.

## Reading Files with async/await

### Creating a Test File First

Before we read, let's create a file to read. Create a file named `hello.txt`:

```
Hello from Node.js!
This is a test file.
We will read this content using the fs module.
```

### Basic File Reading

Create `read-file.js`:

```javascript
// read-file.js - Reading files with fs/promises

// Import the readFile function from fs/promises
// This gives us promise-based file operations
import { readFile } from 'fs/promises';

// Define an async function to read the file
async function readMyFile() {
  try {
    // readFile returns a Promise that resolves with file contents
    // The second argument 'utf8' tells Node.js to decode the bytes as text
    const content = await readFile('hello.txt', 'utf8');
    
    // Print the file contents
    console.log('File contents:');
    console.log(content);
    
  } catch (error) {
    // If something goes wrong (file not found, etc.)
    console.error('Error reading file:', error.message);
  }
}

// Call the function to read the file
readMyFile();
```

Run it:
```bash
node read-file.js
```

### Reading a File as Buffer (Raw Bytes)

Sometimes you need raw binary data instead of text:

```javascript
// read-binary.js - Reading files as binary buffers

import { readFile } from 'fs/promises';

async function readBinary() {
  try {
    // Read without specifying encoding - returns a Buffer
    const buffer = await readFile('hello.txt');
    
    console.log('Buffer type:', typeof buffer);
    console.log('Buffer length:', buffer.length);
    console.log('Buffer content:', buffer);
    
    // Convert buffer to string manually if needed
    const text = buffer.toString('utf8');
    console.log('\nAs string:', text);
    
  } catch (error) {
    console.error('Error:', error.message);
  }
}

readBinary();
```

## Understanding Buffers

A **Buffer** is a region of memory that temporarily holds data being moved around. When reading files without specifying encoding, Node.js gives you a Buffer.

### Buffer to String

```javascript
// Converting between Buffer and String
const buffer = Buffer.from('Hello');  // Create buffer from string
const string = buffer.toString('utf8');  // Convert back to string
```

## Code Example: Complete File Reader

Here's a more complete example that handles different scenarios:

```javascript
// complete-reader.js - Complete file reading example

import { readFile } from 'fs/promises';
import { resolve, join } from 'path';

async function readFileExample() {
  // Get the directory where this script is located
  const scriptDir = import.meta.url 
    ? new URL('.', import.meta.url).pathname 
    : process.cwd();
  
  console.log('=== Reading Files Example ===\n');
  
  // Example 1: Read a simple text file
  console.log('1. Reading text file:');
  try {
    const text = await readFile(join(scriptDir, 'hello.txt'), 'utf8');
    console.log(text);
  } catch (e) {
    console.log('   (hello.txt not found, skipping)');
  }
  
  // Example 2: Read with specific encoding
  console.log('\n2. Reading as UTF-8:');
  const text2 = await readFile(join(scriptDir, 'hello.txt'), { encoding: 'utf8' });
  console.log(text2);
  
  // Example 3: Read as buffer (binary)
  console.log('\n3. Reading as buffer:');
  const buffer = await readFile(join(scriptDir, 'hello.txt'));
  console.log(`   Buffer size: ${buffer.length} bytes`);
  
  // Example 4: Handle missing files gracefully
  console.log('\n4. Handling missing file:');
  try {
    await readFile('nonexistent.txt', 'utf8');
  } catch (error) {
    console.log(`   Error: ${error.message}`);
    console.log('   (This is expected for missing files)');
  }
}

// Check if file exists before reading (optional approach)
import { access, constants } from 'fs/promises';

async function safeRead(filename) {
  try {
    // First check if file exists and is readable
    await access(filename, constants.R_OK);
    // Then read it
    return await readFile(filename, 'utf8');
  } catch (error) {
    if (error.code === 'ENOENT') {
      return null;  // File doesn't exist
    }
    throw error;    // Other error
  }
}

// Run the examples
readFileExample().then(() => {
  console.log('\n=== Examples Complete ===');
});
```

## How It Works

### Step-by-Step Breakdown

1. **Import**: `import { readFile } from 'fs/promises'` brings in the promise-based read function.

2. **Await**: `await readFile(path, encoding)` pauses execution until the file is read. Meanwhile, Node.js can do other things (non-blocking!).

3. **Encoding**: When you specify `'utf8'`, Node.js automatically converts the binary file data into a text string. Without encoding, you get raw bytes (a Buffer).

4. **Error Handling**: Always wrap file operations in try/catch because files might not exist, permissions might be wrong, or the disk might fail.

## Common Mistakes

### Mistake 1: Forgetting the await

```javascript
// WRONG - returns a Promise, not the content
const content = readFile('file.txt', 'utf8');
console.log(content);  // Prints Promise { <pending> }

// CORRECT - use await
const content = await readFile('file.txt', 'utf8');
console.log(content);  // Prints actual file content
```

### Mistake 2: Wrong File Path

Paths can be tricky. Use absolute paths or make sure you're in the right directory:

```javascript
// Using path module to build correct paths
import { join } from 'path';
const filePath = join(__dirname, 'data', 'hello.txt');
```

### Mistake 3: Not Handling Errors

Always wrap file operations in try/catch:

```javascript
// WRONG - crashes if file doesn't exist
const content = await readFile('missing.txt');

// CORRECT - handles error gracefully
try {
  const content = await readFile('missing.txt');
} catch (error) {
  console.log('File not found!');
}
```

## Try It Yourself

### Exercise 1: Read Your package.json
Create a script that reads and displays your package.json file.

### Exercise 2: File Statistics
Use `stat` from fs/promises to get file information (size, creation date, etc.).

### Exercise 3: Read Multiple Files
Create a script that reads multiple files and prints their contents one after another.

## Next Steps

Now you know how to read files. Let's learn how to write files next. Continue to [Writing Files](./02-writing-files.md).
