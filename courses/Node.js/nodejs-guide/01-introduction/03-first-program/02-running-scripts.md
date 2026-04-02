# Running Node.js Scripts

## What You'll Learn

- Different ways to run Node.js scripts
- How to use the --watch flag for auto-restart
- How to pass command-line arguments
- Understanding node command options

## Basic Script Execution

### Running a Single File

The most common way to run a Node.js script:

```bash
node index.js
```

This runs the file `index.js` in the current directory.

### Running Files in Different Locations

```bash
# Run a file in a specific folder
node src/index.js

# Run with absolute path
node /path/to/your/project/index.js
```

## Using the --watch Flag

Node.js v18+ includes a built-in `--watch` flag that automatically restarts your script when files change. This is extremely useful during development.

### Basic Watch Usage

```bash
node --watch index.js
```

Now, every time you save changes to `index.js`, Node.js will automatically restart and run the script again.

### Code Example with Watch

Create a file named `counter.js`:

```javascript
// counter.js - A simple counter that demonstrates auto-restart

// Initialize a counter variable that persists while the script runs
let count = 0;

// Function to increment and display the counter
function increment() {
  count++;
  console.log(`Counter: ${count}`);
}

// Increment immediately
increment();

// Set up an interval to increment every 2 seconds
const intervalId = setInterval(() => {
  increment();
  
  // Stop after 10 counts
  if (count >= 10) {
    console.log('Counter reached 10, stopping...');
    clearInterval(intervalId);
  }
}, 2000);

console.log('Script started. Will increment every 2 seconds.');
console.log('Save this file to restart the script (watch mode).');
```

Run with watch mode:

```bash
node --watch counter.js
```

Try editing the file while it's running. Node.js will restart automatically!

## Node Command Options

The `node` command has many options you can use:

### Common Options

| Option | Description |
|--------|-------------|
| `-v` or `--version` | Show Node.js version |
| `-e` or `--eval` | Execute code directly from command line |
| `-p` or `--print` | Execute and print result |
| `--watch` | Watch for file changes |
| `-r` or `--require` | Preload a module |

### Using --eval or -e

Execute JavaScript directly without a file:

```bash
# Simple calculation
node -e "console.log(2 + 2)"

# Use template literals
node -e "const x = 10; console.log(\`Value: \${x}\`)"

# Execute multiple statements
node -e "const arr = [1,2,3]; console.log(arr.map(x => x * 2))"
```

### Using --print or -p

Execute and print the result:

```bash
# Prints the return value automatically
node -p "2 + 2"
# Output: 4

node -p "'Hello, ' + 'World!'"
# Output: Hello, World!
```

### Using --require or -r

Preload a module before running your script. This is useful for:

- Loading environment variables
- Setting up global mocks for testing
- Enabling experimental features

```bash
# Load dotenv before running (covered later)
node -r dotenv/config index.js

# Load a custom setup file
node -r ./setup.js index.js
```

## Understanding Module Preloading

When you use `-r` or `--require`, Node.js loads the specified module before your main script. Create a setup file:

```javascript
// setup.js - Preloaded module example

console.log('=== Setup Module Loading ===');

// Add a global function that will be available in all scripts
global.describe = function(title, fn) {
  console.log(`DESCRIBE: ${title}`);
  fn();
};

// Add a helper to global scope
global.it = function(title, fn) {
  console.log(`  IT: ${title}`);
};

console.log('Setup complete!\n');
```

Now run any script with:

```bash
node -r ./setup.js your-script.js
```

## Running Scripts via npm

npm provides convenient script running through `package.json`:

### Setting Up npm Scripts

In your `package.json`:

```json
{
  "name": "my-app",
  "version": "1.0.0",
  "type": "module",
  "scripts": {
    "start": "node index.js",
    "dev": "node --watch index.js",
    "calc": "node -e \"console.log(10 * 5)\"",
    "test": "node --test"
  }
}
```

Now you can run:

```bash
npm run start    # Runs: node index.js
npm run dev     # Runs: node --watch index.js
npm run calc    # Runs: node -e "console.log(10 * 5)"
npm test        # Special: 'npm test' works without 'run'
```

## Code Example: Complete Script Runner

Here's a comprehensive example demonstrating various running methods:

Create `demo.js`:

```javascript
// demo.js - Demonstrates different execution patterns

console.log('=== Script Execution Demo ===\n');

// Check if arguments were passed
if (process.argv.length > 2) {
  console.log('Command-line arguments received:');
  // process.argv[0] = node path
  // process.argv[1] = script path
  // process.argv[2+] = your arguments
  process.argv.slice(2).forEach((arg, index) => {
    console.log(`  Argument ${index + 1}: ${arg}`);
  });
  console.log('');
}

// Display environment info
console.log('Environment:');
console.log(`  Node.js version: ${process.version}`);
console.log(`  Platform: ${process.platform}`);
console.log(`  Working directory: ${process.cwd()}`);

// Check if running in watch mode
// There's no direct flag check, but you can detect restart
console.log(`\n  Script timestamp: ${new Date().toISOString()}`);

console.log('\n=== Demo Complete ===');
```

Run with various arguments:

```bash
# Basic run
node demo.js

# With arguments
node demo.js arg1 arg2 arg3

# With npm script (add to package.json first)
# npm run demo -- arg1 arg2
```

## Common Mistakes

### Mistake 1: Wrong Working Directory

If your script can't find files, check your current directory:

```bash
# Check where you are
pwd           # macOS/Linux
cd            # Windows

# Navigate to correct folder
cd path/to/folder
```

### Mistake 2: Forgetting to Save Before Watch Restarts

With `--watch`, the script restarts when you SAVE the file, not when you switch to the terminal.

### Mistake 3: Using CommonJS Syntax Without Configuration

If your `.js` files use `import/export` but you forgot `"type": "module"` in `package.json`, you'll get errors.

### Mistake 4: Arguments Not Reaching Your Script

Remember: `process.argv` includes node path and script path as the first two elements. Use `process.argv.slice(2)` to get your actual arguments.

## Try It Yourself

### Exercise 1: Watch Mode Practice
Create a script that prints the current time every second. Run it with `--watch`. Edit the file and watch it restart.

### Exercise 2: Create npm Scripts
Set up a `package.json` with scripts for:
- `npm start` - runs your main file
- `npm run dev` - runs with --watch
- `npm run greet` - runs a script that greets a name passed as argument

### Exercise 3: Command-line Calculator
Create a script that accepts two numbers and an operation, then prints the result. Run it with arguments like:
```bash
node calculator.js 10 + 5
```

## Next Steps

Now you understand how to run Node.js scripts. Let's learn how to pass and handle command-line arguments in your programs. Continue to [Script Arguments](./03-script-args.md).
