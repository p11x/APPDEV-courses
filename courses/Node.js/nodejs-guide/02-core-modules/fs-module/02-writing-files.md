# Writing Files in Node.js

## What You'll Learn

- How to write files using the fs module
- The difference between writeFile and appendFile
- How to use streams for large files
- Proper error handling for write operations

## Writing Files with fs/promises

Just like reading, Node.js provides promise-based functions for writing files. The main functions are:
- `writeFile` - Creates a new file or overwrites existing content
- `appendFile` - Adds content to the end of existing file

## Basic File Writing

### Creating a File

Create `write-file.js`:

```javascript
// write-file.js - Writing files with fs/promises

// Import writeFile from fs/promises
import { writeFile } from 'fs/promises';

async function writeToFile() {
  try {
    // Define the content to write
    const content = 'Hello, Node.js!\nWriting to files is easy.';
    
    // writeFile creates the file if it doesn't exist
    // and overwrites if it already exists
    await writeFile('output.txt', content, 'utf8');
    
    console.log('File written successfully!');
    
  } catch (error) {
    console.error('Error writing file:', error.message);
  }
}

// Execute the function
writeToFile();
```

Run it:
```bash
node write-file.js
```

Now check for `output.txt` in your folder - it should contain the text!

### Writing JSON Data

```javascript
// write-json.js - Writing JSON to a file

import { writeFile } from 'fs/promises';

async function writeJSON() {
  // Create a JavaScript object
  const data = {
    name: 'Alice',
    age: 25,
    hobbies: ['reading', 'coding', 'gaming'],
    timestamp: new Date().toISOString()
  };
  
  // Convert JavaScript object to JSON string
  const jsonString = JSON.stringify(data, null, 2);
  
  // Write to file
  await writeFile('data.json', jsonString, 'utf8');
  
  console.log('JSON file written!');
  console.log('Content:', jsonString);
}

writeJSON();
```

The `JSON.stringify(data, null, 2)` converts the object to a nicely formatted JSON string with 2-space indentation.

## Appending to Files

### Using appendFile

If you want to add content to the end of an existing file (without overwriting):

```javascript
// append-file.js - Appending to existing files

import { appendFile, writeFile } from 'fs/promises';

async function appendExample() {
  // First, create a file with initial content
  await writeFile('log.txt', 'Log started\n', 'utf8');
  
  // Now append more content
  await appendFile('log.txt', 'User logged in\n', 'utf8');
  await appendFile('log.txt', 'User viewed dashboard\n', 'utf8');
  await appendFile('log.txt', 'User logged out\n', 'utf8');
  
  console.log('Log entries appended!');
  
  // Read and display the file
  import { readFile } from 'fs/promises';
  const content = await readFile('log.txt', 'utf8');
  console.log('\nFile contents:');
  console.log(content);
}

appendExample();
```

### Why Use appendFile?

Appending is useful for:
- **Log files**: Adding timestamped entries
- **History tracking**: Keeping a record of actions
- **Data collection**: Adding new data points over time

## Writing Large Files with Streams

For very large files, using `writeFile` can use a lot of memory. Streams let you write data in chunks.

### Creating a Write Stream

```javascript
// write-stream.js - Writing large files with streams

import { createWriteStream } from 'fs';
import { pipeline } from 'stream/promises';

async function writeWithStream() {
  // Create a write stream - data goes into this file
  const writeStream = createWriteStream('large-file.txt');
  
  // Write some lines
  for (let i = 1; i <= 100; i++) {
    writeStream.write(`Line ${i}: This is line number ${i}\n`);
  }
  
  // Close the stream - important!
  // This flushes any remaining data to the file
  writeStream.end();
  
  // Wait for the stream to finish
  await new Promise((resolve, reject) => {
    writeStream.on('finish', resolve);
    writeStream.on('error', reject);
  });
  
  console.log('Large file written successfully!');
}

writeWithStream();
```

### Using pipeline (Recommended)

Node.js v15+ provides `pipeline` which handles errors automatically:

```javascript
// pipeline-example.js - Using pipeline for reliable streaming

import { createReadStream, createWriteStream } from 'fs';
import { pipeline } from 'stream/promises';

async function copyFile() {
  // Read from source file
  const readStream = createReadStream('source.txt');
  
  // Write to destination file
  const writeStream = createWriteStream('destination.txt');
  
  // pipeline automatically:
  // - Pipes data from read to write
  // - Handles errors
  // - cleans up streams when done
  
  await pipeline(readStream, writeStream);
  
  console.log('File copied successfully!');
}

copyFile();
```

## Code Example: Complete File Writer

Here's a comprehensive example combining different writing methods:

```javascript
// complete-writer.js - Complete file writing example

import { writeFile, appendFile, readFile } from 'fs/promises';
import { createWriteStream } from 'fs';

async function fileWritingDemo() {
  console.log('=== File Writing Demo ===\n');
  
  // Example 1: Simple write
  console.log('1. Writing simple text:');
  await writeFile('simple.txt', 'Hello World!', 'utf8');
  console.log('   ✓ simple.txt created');
  
  // Example 2: Writing JSON
  console.log('\n2. Writing JSON:');
  const user = { name: 'Bob', email: 'bob@example.com' };
  await writeFile('user.json', JSON.stringify(user, null, 2), 'utf8');
  console.log('   ✓ user.json created');
  
  // Example 3: Appending to file
  console.log('\n3. Appending to file:');
  await writeFile('notes.txt', 'First note\n', 'utf8');
  await appendFile('notes.txt', 'Second note\n', 'utf8');
  await appendFile('notes.txt', 'Third note\n', 'utf8');
  const notes = await readFile('notes.txt', 'utf8');
  console.log('   Notes content:', notes.trim().split('\n').length, 'lines');
  
  // Example 4: Using write stream
  console.log('\n4. Writing with streams:');
  const stream = createWriteStream('stream-output.txt');
  stream.write('Line 1\n');
  stream.write('Line 2\n');
  stream.write('Line 3\n');
  stream.end();
  await new Promise(resolve => stream.on('finish', resolve));
  console.log('   ✓ stream-output.txt created');
  
  console.log('\n=== All Files Written ===');
}

fileWritingDemo();
```

## Understanding File Flags

When writing files, you can use flags to control behavior:

| Flag | Description |
|------|-------------|
| `w` | Write (creates or overwrites) |
| `a` | Append (creates or adds to end) |
| `wx` | Write but fail if file exists |
| `ax` | Append but fail if file exists |

```javascript
// Using flags
await writeFile('file.txt', 'content', { flag: 'wx' });  // Fails if exists
await writeFile('file.txt', 'content', { flag: 'ax' });  // Fails if exists
```

## Common Mistakes

### Mistake 1: Forgetting await

```javascript
// WRONG - file might not be written yet
writeFile('file.txt', 'content');
console.log('Done');  // Might print before file is written

// CORRECT - wait for completion
await writeFile('file.txt', 'content');
console.log('Done');  // Prints after file is written
```

### Mistake 2: Writing Objects Directly

```javascript
// WRONG - objects can't be written directly
await writeFile('data.json', { name: 'Alice' });  // Writes [object Object]

// CORRECT - convert to JSON first
await writeFile('data.json', JSON.stringify({ name: 'Alice' }));
```

### Mistake 3: Not Closing Streams

Always call `stream.end()` or use `pipeline` for streams. Otherwise, data might not be fully written.

## Try It Yourself

### Exercise 1: Create a Todo List
Write a program that saves a todo list to a file, with each todo on a new line.

### Exercise 1: Append to Todo List
Extend the program to add new todos without overwriting existing ones.

### Exercise 3: Copy a File
Create a script that copies one file to another using streams.

## Next Steps

Now you know how to read and write files. Let's learn how to watch files for changes. Continue to [Watching Files](./03-watching-files.md).
