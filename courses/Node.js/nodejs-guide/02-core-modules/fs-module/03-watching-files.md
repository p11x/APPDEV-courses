# Watching Files in Node.js

## What You'll Learn

- How to monitor files for changes using fs.watch
- Understanding the FSWatcher object
- Real-world use cases for file watching
- Differences between watch and watchFile

## Why Watch Files?

File watching is useful for:
- **Hot reloading**: Automatically restart development servers when code changes
- **Build tools**: Trigger builds when source files are modified
- **Log monitoring**: Watch log files for new entries
- **File synchronization**: Sync files across locations

## Using fs.watch

Node.js provides `fs.watch()` for monitoring file system changes.

### Basic File Watch

Create `watch-file.js`:

```javascript
// watch-file.js - Basic file watching example

import { watch } from 'fs';
import { join } from 'path';

console.log('=== File Watcher Demo ===');
console.log('Create, modify, or delete data.txt to see events');
console.log('Press Ctrl+C to stop\n');

// Get the filename to watch (from command line or use default)
const filename = process.argv[2] || 'data.txt';

// Create a watcher that monitors the specified file/folder
// The watcher emits events when files change
const watcher = watch('.', { recursive: true }, (eventType, filename) => {
  // eventType is either 'change' or 'rename'
  // filename is the name of the file that changed
  
  if (filename) {
    console.log(`Event: ${eventType} - File: ${filename}`);
  }
});

// Handle graceful shutdown
process.on('SIGINT', () => {
  console.log('\nStopping watcher...');
  watcher.close();
  process.exit(0);
});
```

Run it:
```bash
node watch-file.js
```

Now create or modify `data.txt` while the script runs!

### Watching a Specific File

```javascript
// watch-specific.js - Watching a specific file

import { watch } from 'fs';

const filename = 'config.json';

console.log(`Watching ${filename} for changes...`);

const watcher = watch(filename, (eventType) => {
  console.log(`\n${filename} was ${eventType}d!`);
  
  // Read the new content
  import { readFile } from 'fs/promises';
  try {
    const content = await readFile(filename, 'utf8');
    console.log('New content:', content.trim());
  } catch (error) {
    console.log('Could not read file:', error.message);
  }
});

console.log('Press Ctrl+C to stop');
```

## Understanding FSWatcher

The `watch()` function returns an FSWatcher object with useful methods:

```javascript
// fswatcher-object.js - Working with FSWatcher

import { watch } from 'fs';

const watcher = watch('data.txt');

// FSWatcher has these methods:
console.log('Watcher methods available:');
console.log('  - close(): Stop watching');
console.log('  - ref(): Keep process alive for watching');
console.log('  - unref(): Allow process to exit even if watching');

// Close the watcher when done
// watcher.close();
```

### Watching with Options

```javascript
// watch-options.js - File watching with options

import { watch } from 'fs';

const watcher = watch('.', {
  persistent: true,    // Keep process running while watching
  recursive: true,    // Watch subdirectories too
  encoding: 'utf8'    // Encoding for filenames
}, (eventType, filename) => {
  console.log(`Changed: ${filename} (${eventType})`);
});
```

## Code Example: Auto-Restart Server

Here's a practical example - a simple file watcher that restarts a script:

```javascript
// auto-restart.js - Auto-restart when files change

import { watch } from 'fs';
import { spawn } from 'child_process';

let childProcess = null;

// Function to start the target script
function startScript() {
  // Kill existing process if running
  if (childProcess) {
    console.log('Restarting script...');
    childProcess.kill();
  } else {
    console.log('Starting script...');
  }
  
  // Spawn a new Node.js process
  childProcess = spawn('node', ['server.js'], {
    stdio: 'inherit'  // Show output in terminal
  });
  
  childProcess.on('exit', (code) => {
    console.log(`Script exited with code ${code}`);
  });
}

// Watch for changes in current directory
const watcher = watch('.', { recursive: true }, (eventType, filename) => {
  // Only restart for .js and .json files
  if (filename && (filename.endsWith('.js') || filename.endsWith('.json'))) {
    console.log(`\n[Watcher] ${filename} changed (${eventType})`);
    
    // Debounce - wait 500ms before restarting
    clearTimeout(restartTimer);
    restartTimer = setTimeout(startScript, 500);
  }
});

let restartTimer;

// Handle shutdown
process.on('SIGINT', () => {
  console.log('\nShutting down...');
  if (childProcess) childProcess.kill();
  watcher.close();
  process.exit(0);
});

console.log('Auto-restart watching started...');
console.log('Edit server.js to see automatic restart\n');

startScript();
```

## Real-World Use Cases

### Use Case 1: Development Hot Reload

```javascript
// hot-reload.js - Simple hot reload for development

import { watch } from 'fs';

function startServer() {
  console.log('Development server started on http://localhost:3000');
}

// Watch for changes in src folder
const watcher = watch('./src', { recursive: true }, (event, file) => {
  if (file && file.endsWith('.js')) {
    console.log(`\n📦 File changed: ${file}`);
    console.log('Recompiling...');
    // In real use, you'd rebuild and restart your server here
  }
});

console.log('Watching ./src for changes...');
startServer();
```

### Use Case 2: Log File Monitoring

```javascript
// monitor-log.js - Watching a log file

import { watch } from 'fs';
import { readFile } from 'fs/promises';

let lastSize = 0;

async function checkNewContent() {
  try {
    const stats = await import('fs').then(fs => 
      fs.promises.stat('app.log')
    );
    
    if (stats.size > lastSize) {
      // File grew - read new content
      const content = await readFile('app.log', 'utf8');
      const newContent = content.slice(lastSize);
      console.log('New log entries:', newContent.trim());
      lastSize = stats.size;
    }
  } catch (error) {
    // File might not exist yet
  }
}

const watcher = watch('app.log', (event) => {
  if (event === 'change') {
    checkNewContent();
  }
});

console.log('Watching app.log for new entries...');
```

## fs.watch vs fs.watchFile

Node.js has two ways to watch files:

### fs.watch (Recommended)
- Uses OS-level file watching (more efficient)
- May not work consistently across all systems
- Can miss rapid changes

```javascript
import { watch } from 'fs';
watch('file.txt', callback);
```

### fs.watchFile (Older)
- Polls the file periodically (less efficient)
- More consistent across platforms
- Uses more CPU

```javascript
import { watchFile } from 'fs';
watchFile('file.txt', (curr, prev) => {
  console.log('File modified!');
});
```

## Common Mistakes

### Mistake 1: Not Closing the Watcher

Always close watchers when done to free up system resources:

```javascript
// WRONG - watcher stays open
watch('folder', callback);

// CORRECT - close when done
const watcher = watch('folder', callback);
// When done:
watcher.close();
```

### Mistake 2: Watching Too Many Files

Watching entire directories recursively can be resource-intensive. Consider watching only specific directories.

### Mistake 3: Not Debouncing

Rapid file changes trigger multiple events. Use debouncing (setTimeout) to wait until changes settle:

```javascript
let debounceTimer;
watch('folder', (event, file) => {
  clearTimeout(debounceTimer);
  debounceTimer = setTimeout(() => {
    handleChange(file);
  }, 300);  // Wait 300ms after last change
});
```

## Try It Yourself

### Exercise 1: Simple Watcher
Create a watcher that prints a message every time you modify a specific file.

### Exercise 2: File Change Counter
Create a script that counts how many times a file has been modified. Store the count in a separate file.

### Exercise 3: Auto-Build Trigger
Create a watcher that triggers a build (prints "Building...") when any .js file in src/ changes.

## Next Steps

Now you understand file operations. Let's move on to the path module for working with file paths. Continue to [Path Basics](../path-module/01-path-basics.md).
