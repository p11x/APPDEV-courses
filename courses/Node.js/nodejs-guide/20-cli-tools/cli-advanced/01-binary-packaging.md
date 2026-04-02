# Binary Packaging

## What You'll Learn

- How to make a Node.js script executable with a shebang
- How to configure `bin` in package.json
- How to use `npm link` for local testing
- How to publish a CLI tool to npm
- How to use `npx` to run tools without installing

## The Shebang

A **shebang** is the first line of a script that tells the operating system which interpreter to use:

```js
#!/usr/bin/env node
// The rest of your script...
```

When you run `./my-script.js`, the OS reads the shebang and executes `node my-script.js`.

## Project Setup

```bash
mkdir my-tool && cd my-tool
npm init -y
```

## Making the Script Executable

```js
#!/usr/bin/env node
// index.js — Executable CLI tool

const args = process.argv.slice(2);  // Remove 'node' and script path

if (args.length === 0) {
  console.log('Usage: my-tool <command> [options]');
  console.log('');
  console.log('Commands:');
  console.log('  hello [name]   Say hello');
  console.log('  version        Show version');
  process.exit(0);
}

const command = args[0];

if (command === 'hello') {
  const name = args[1] || 'World';
  console.log(`Hello, ${name}!`);
} else if (command === 'version') {
  console.log('1.0.0');
} else {
  console.error(`Unknown command: ${command}`);
  process.exit(1);
}
```

### Make It Executable

```bash
# Unix/Mac: add execute permission
chmod +x index.js

# Now you can run it directly
./index.js hello Alice
# Hello, Alice!
```

## Configuring package.json

```json
{
  "name": "my-tool",
  "version": "1.0.0",
  "type": "module",
  "bin": {
    "my-tool": "./index.js"
  },
  "files": [
    "index.js"
  ],
  "engines": {
    "node": ">=20"
  }
}
```

| Field | Purpose |
|-------|---------|
| `bin` | Maps command names to scripts — `my-tool` runs `./index.js` |
| `files` | Files to include in the published package |
| `engines` | Required Node.js version |

## Local Testing with npm link

```bash
# In your project directory
npm link

# This creates a symlink:
# /usr/local/bin/my-tool → ./index.js

# Now you can run it globally
my-tool hello Alice
# Hello, Alice!

# To unlink when done
npm unlink my-tool
```

## Running with npx

After publishing (or even locally), anyone can run your tool with `npx`:

```bash
# Run without installing
npx my-tool hello Alice

# npx downloads the package, runs it, then caches it
```

## Publishing to npm

```bash
# 1. Create an npm account (if you don't have one)
npm adduser

# 2. Check the package name is available
npm view my-tool

# 3. Publish
npm publish

# 4. Now anyone can install and run it
npm install -g my-tool
my-tool hello Alice
```

### Scoped Packages

If the name `my-tool` is taken, use a scope:

```json
{
  "name": "@yourname/my-tool",
  "bin": { "my-tool": "./index.js" }
}
```

```bash
# Scoped packages need a public flag for free accounts
npm publish --access public

# Users install with:
npx @yourname/my-tool hello
```

## Complete Example: File Counter CLI

```js
#!/usr/bin/env node
// index.js — A real-world CLI that counts files

import { readdir, stat } from 'node:fs/promises';
import { resolve, extname } from 'node:path';

const args = process.argv.slice(2);

// Parse arguments
let directory = '.';
let extension = null;

for (let i = 0; i < args.length; i++) {
  if (args[i] === '--ext' && args[i + 1]) {
    extension = args[i + 1];
    i++;  // Skip the next argument
  } else if (args[i] === '--help') {
    console.log(`
Usage: filecount [directory] [--ext <extension>]

Options:
  --ext <ext>   Filter by file extension (e.g., .js, .md)
  --help        Show this help message
  --version     Show version
`);
    process.exit(0);
  } else if (args[i] === '--version') {
    console.log('1.0.0');
    process.exit(0);
  } else if (!args[i].startsWith('-')) {
    directory = args[i];
  }
}

async function countFiles(dir, ext) {
  let count = 0;
  let totalSize = 0;

  async function walk(currentDir) {
    const entries = await readdir(currentDir, { withFileTypes: true });

    for (const entry of entries) {
      const fullPath = resolve(currentDir, entry.name);

      if (entry.isDirectory()) {
        // Skip hidden directories and node_modules
        if (!entry.name.startsWith('.') && entry.name !== 'node_modules') {
          await walk(fullPath);
        }
      } else if (entry.isFile()) {
        if (ext && extname(entry.name) !== ext) continue;
        count++;
        const { size } = await stat(fullPath);
        totalSize += size;
      }
    }
  }

  await walk(dir);
  return { count, totalSize };
}

try {
  const { count, totalSize } = await countFiles(directory, extension);
  const sizeMB = (totalSize / (1024 * 1024)).toFixed(2);

  console.log(`Files: ${count}`);
  console.log(`Total size: ${sizeMB} MB`);
  if (extension) console.log(`Extension filter: ${extension}`);
} catch (err) {
  console.error(`Error: ${err.message}`);
  process.exit(1);
}
```

## How It Works

### The bin Field

When you run `npm install -g my-tool`, npm reads the `bin` field and creates a symlink:

```
/usr/local/bin/my-tool → /usr/local/lib/node_modules/my-tool/index.js
```

The shebang tells the OS to use Node.js to execute the file.

### Package Distribution

| Method | Use Case |
|--------|----------|
| `npm install -g` | Permanent global install |
| `npx my-tool` | One-time run without install |
| `npm link` | Local development testing |

## Common Mistakes

### Mistake 1: Missing Shebang

```js
// WRONG — running ./index.js fails on Unix
console.log('Hello');

// CORRECT — shebang must be the very first line
#!/usr/bin/env node
console.log('Hello');
```

### Mistake 2: Forgetting chmod +x

```bash
# WRONG — permission denied
./index.js hello
# zsh: permission denied: ./index.js

# CORRECT — make it executable
chmod +x index.js
```

### Mistake 3: Not Listing bin in files

```json
{
  "files": ["src/"]
  // WRONG — index.js is not in src/, so npm package does not include it
}

{
  "files": ["index.js", "src/"]
  // CORRECT — explicitly include the entry point
}
```

## Try It Yourself

### Exercise 1: Hello World CLI

Create a CLI that takes a name argument and prints a greeting. Add `--shout` to uppercase the output. Publish it to npm.

### Exercise 2: File Stats

Create a CLI that shows file count, total size, and largest file in a directory. Add `--ext .js` filtering.

### Exercise 3: Global Install

Install your CLI globally with `npm link`. Test it from a different directory.

## Next Steps

You can package CLI tools. For managing configuration files, continue to [Config Files](./02-config-files.md).
