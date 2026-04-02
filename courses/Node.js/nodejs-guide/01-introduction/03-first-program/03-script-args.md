# Command-Line Arguments in Node.js

## What You'll Learn

- How to access command-line arguments using process.argv
- How to parse different types of arguments (flags, values)
- How to build a simple CLI tool
- Best practices for handling arguments

## Understanding process.argv

When you run a Node.js script, you can pass additional words (arguments) after the script name. These are stored in `process.argv`.

### What is process.argv?

`process.argv` is an array (list) that contains:
- Index 0: Path to the Node.js executable
- Index 1: Path to the script being executed
- Index 2+: Any arguments you pass

### Basic Example

Create a file named `show-args.js`:

```javascript
// show-args.js - Display all command-line arguments

console.log('=== All Arguments ===\n');

// Display the entire argv array
console.log('Full process.argv:');
process.argv.forEach((arg, index) => {
  console.log(`  [${index}]: ${arg}`);
});

console.log('\n=== Useful Information ===');
// The first two items are always node and script path
console.log('Node.js path:', process.argv[0]);
console.log('Script path:', process.argv[1]);

// Your actual arguments start at index 2
const myArgs = process.argv.slice(2);
console.log('Your arguments:', myArgs);
```

Run it with different arguments:

```bash
node show-args.js hello world 42
```

Output:
```
=== All Arguments ===

Full process.argv:
  [0]: C:\Program Files\nodejs\node.exe
  [1]: C:\Users\You\project\show-args.js
  [2]: hello
  [3]: world
  [4]: 42

=== Useful Information ===
Node.js path: C:\Program Files\nodejs\node.exe
Script path: C:\Users\You\project\show-args.js
Your arguments: [ 'hello', 'world', '42' ]
```

## Parsing Different Argument Styles

There are common patterns for passing arguments:

### Positional Arguments

Arguments in a specific order:

```bash
node program.js Alice 25
```

- First argument: name
- Second argument: age

### Flags

Boolean options (on/off):

```bash
node program.js --verbose --debug
```

### Flag with Values

Flags that have associated values:

```bash
node program.js --name Alice --age 25
```

### Short Flags

Single-letter flags:

```bash
node program.js -n Alice -a 25
# Same as: --name Alice --age 25
```

## Code Example: Building a Simple CLI

Create a file named `greet.js` - a simple CLI that greets users:

```javascript
// greet.js - CLI argument parsing example

// Function to parse command-line arguments
function parseArgs(args) {
  const result = {
    name: 'World',      // Default value
    age: null,
    verbose: false,
    help: false
  };
  
  // Loop through arguments starting from index 2
  for (let i = 0; i < args.length; i++) {
    const arg = args[i];
    
    if (arg === '--name' || arg === '-n') {
      // Next argument is the name value
      result.name = args[i + 1];
      i++; // Skip the next argument since we used it
    } else if (arg === '--age' || arg === '-a') {
      result.age = parseInt(args[i + 1]);
      i++;
    } else if (arg === '--verbose' || arg === '-v') {
      result.verbose = true;
    } else if (arg === '--help' || arg === '-h') {
      result.help = true;
    }
  }
  
  return result;
}

// Main program
const args = parseArgs(process.argv);

// Handle help flag
if (args.help) {
  console.log(`
Usage: node greet.js [options]

Options:
  -n, --name <name>    Name to greet (default: World)
  -a, --age <age>     Age to display
  -v, --verbose       Show detailed output
  -h, --help          Show this help message

Examples:
  node greet.js
  node greet.js --name Alice
  node greet.js -n Bob -a 30 --verbose
  `);
  process.exit(0); // Exit successfully
}

// Build greeting message
let greeting = `Hello, ${args.name}!`;

if (args.verbose) {
  console.log('Verbose mode enabled');
  console.log('Parsed arguments:', args);
}

// Add age if provided
if (args.age !== null) {
  greeting += ` You are ${args.age} years old.`;
}

console.log(greeting);
```

### Testing the CLI

Try these commands:

```bash
# Default greeting
node greet.js

# With name
node greet.js --name Alice
node greet.js -n Bob

# With name and age
node greet.js --name Charlie --age 25

# With verbose mode
node greet.js -n Diana -a 30 -v

# Show help
node greet.js --help
```

## Modern Argument Parsing

For more complex applications, use a library like `minimist` or `commander`. Here's how to use `minimist`:

### Installing minimist

```bash
npm install minimist
```

### Using minimist

Create `greet-modern.js`:

```javascript
// greet-modern.js - Using minimist for argument parsing

import minimist from 'minimist';

// Parse arguments with minimist
const args = minimist(process.argv.slice(2), {
  // Define expected flags and their types
  string: ['name', 'age'],    // These are string values
  boolean: ['verbose', 'help'], // These are boolean flags
  alias: {                    // Short aliases
    n: 'name',
    a: 'age',
    v: 'verbose',
    h: 'help'
  },
  default: {                  // Default values
    name: 'World',
    verbose: false
  }
});

// Handle help
if (args.help) {
  console.log(`
Usage: node greet-modern.js [options]

Options:
  -n, --name <name>    Name to greet (default: World)
  -a, --age <age>     Age to display
  -v, --verbose       Show detailed output
  -h, --help          Show this help message
  `);
  process.exit(0);
}

// Show parsed arguments if verbose
if (args.verbose) {
  console.log('Parsed arguments:', args);
}

// Build and display greeting
let greeting = `Hello, ${args.name}!`;
if (args.age) {
  greeting += ` You are ${args.age} years old.`;
}

console.log(greeting);
```

## Common Mistakes

### Mistake 1: Forgetting process.argv Slice

Remember: `process.argv` includes node path and script path. Use `process.argv.slice(2)` to get your actual arguments.

```javascript
// WRONG - includes node and script paths
const myArgs = process.argv;

// CORRECT - your actual arguments
const myArgs = process.argv.slice(2);
```

### Mistake 2: Not Handling Missing Arguments

Always provide default values:

```javascript
// WRONG - crashes if no name provided
const name = process.argv[2];

// CORRECT - provides default
const name = process.argv[2] || 'World';
```

### Mistake 3: Not Converting Types

Arguments come as strings. Convert to numbers when needed:

```javascript
// age comes as string "25", not number 25
const age = parseInt(process.argv[2]); // Converts to number
const price = parseFloat(process.argv[3]); // For decimals
```

### Mistake 4: Using Positional Arguments for Complex Tools

For complex CLIs, use a library like `commander` or `yargs` instead of parsing manually.

## Try It Yourself

### Exercise 1: Simple Calculator
Create a calculator that takes three arguments: `node calc.js <num1> <operator> <num2>`

Example: `node calc.js 10 + 5` should print `15`

### Exercise 2: File Creator
Create a script that accepts `--filename` and `--content` and creates a file.

### Exercise 3: Todo List CLI
Create a simple todo app with commands:
- `node todo.js add "Buy milk"`
- `node todo.js list`
- `node todo.js done <index>`

## Next Steps

Now you know how to create and run Node.js programs. You've completed the introduction section! 

Let's move on to learning about Node.js core modules - the built-in functionality that makes Node.js powerful. Continue to [Reading Files with the FS Module](../02-core-modules/fs-module/01-reading-files.md).
