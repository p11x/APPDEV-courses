# Hello World with ES Modules

## What You'll Learn

- How to create your first Node.js program using ES Modules
- The structure of a basic Node.js script
- How to use console.log() for output
- Understanding the import statement

## What is ES Modules?

**ES Modules** (ECMAScript Modules) is the standard way to organize and share code in JavaScript. It uses:
- `import` to bring code from other files
- `export` to make code available to other files

This is the modern way to write Node.js code (Node.js v14+ supports ES Modules natively).

## Creating Your First Program

### Step 1: Create a Project Folder

Create a new folder for your project. This keeps your code organized.

### Step 2: Create the Package.json File

Every Node.js project needs a `package.json` file. This file tells Node.js about your project and how to run it.

Create a file named `package.json` in your project folder:

```json
{
  "name": "my-first-nodejs-app",
  "version": "1.0.0",
  "description": "My first Node.js program",
  "type": "module",
  "main": "index.js",
  "scripts": {
    "start": "node index.js"
  }
}
```

The important line is `"type": "module"` - this tells Node.js to use ES Modules instead of the older CommonJS system.

### Step 3: Create the Hello World Script

Create a file named `index.js` and add:

```javascript
// This is my first Node.js program
// It demonstrates basic console output using ES Modules

// Define a simple greeting function that returns a personalized message
function createGreeting(name) {
  return `Hello, ${name}! Welcome to Node.js.`;
}

// Call the function with "World" as the argument
const greeting = createGreeting('World');

// Print the greeting to the console
console.log(greeting);

// Print some additional information about our environment
console.log('---');
console.log('Node.js version:', process.version);
console.log('Current directory:', process.cwd());
```

### Step 4: Run the Program

Open your terminal, navigate to your project folder, and run:

```bash
node index.js
```

You should see output like:

```
Hello, World! Welcome to Node.js.
---
Node.js version: v20.11.0
Current directory: C:\Users\YourName\Projects\my-first-nodejs-app
```

## Understanding the Code

Let's break down each part:

### Comments

```javascript
// This is a single-line comment
/* This is a
   multi-line comment */
```

Comments help you and others understand your code. Node.js ignores comments when running.

### Function Definition

```javascript
function createGreeting(name) {
  return `Hello, ${name}! Welcome to Node.js.`;
}
```

This creates a reusable function that takes a `name` parameter and returns a greeting string using template literals.

### Template Literals

```javascript
`Hello, ${name}!`
```

Template literals use backticks (`` ` ``) instead of quotes. They allow you to embed variables using `${variableName}`.

### Variable Declaration

```javascript
const greeting = createGreeting('World');
```

We use `const` to declare a variable that won't be reassigned. The value comes from calling our function.

### Console Output

```javascript
console.log(greeting);
```

`console.log()` prints text to the terminal. It's the most common way to see output in Node.js.

### Process Object

```javascript
process.version   // Node.js version
process.cwd()     // Current working directory
```

`process` is a built-in Node.js object that provides information about the current Node.js environment.

## Code Example: Multiple Functions

Let's expand our program to show more features:

```javascript
// index.js - Enhanced version with multiple functions

// Function that adds two numbers
function add(a, b) {
  return a + b;
}

// Function that creates a formatted message
function createMessage(name, age) {
  return `${name} is ${age} years old.`;
}

// Function that demonstrates array usage
function showFruits() {
  const fruits = ['apple', 'banana', 'orange'];
  
  // Loop through fruits and print each one
  for (const fruit of fruits) {
    console.log(`- ${fruit}`);
  }
}

// Main execution - this runs when we execute the file
console.log('=== Basic Math ===');
const sum = add(5, 3);
console.log('5 + 3 =', sum);

console.log('\n=== Personal Message ===');
const message = createMessage('Alice', 25);
console.log(message);

console.log('\n=== Fruits List ===');
showFruits();

console.log('\n=== Program Complete ===');
```

Run with:
```bash
node index.js
```

## Common Mistakes

### Mistake 1: Forgetting "type": "module"

If you forget to add `"type": "module"` in `package.json`, Node.js will use CommonJS (`require()`) instead of ES Modules (`import`).

### Mistake 2: Using Wrong Quotes

```javascript
// WRONG - single or double quotes don't work for template literals
const greeting = 'Hello, ${name}!'

// CORRECT - use backticks
const greeting = `Hello, ${name}!`
```

### Mistake 3: Forgetting to Save

Always save your files before running. The Node.js won't see your changes until you save.

### Mistake 4: Wrong File Path

Make sure you're in the correct directory when running `node index.js`. Use `pwd` (Unix) or `cd` to navigate.

## Try It Yourself

### Exercise 1: Personal Greeting
Modify the program to ask for your name and print a personalized greeting.

### Exercise 2: Calculator
Create functions for:
- Subtraction
- Multiplication
- Division

Test each function with console.log().

### Exercise 3: Temperature Converter
Create a function that converts Celsius to Fahrenheit using the formula: F = (C * 9/5) + 32

## Next Steps

Now you know how to create and run a basic Node.js program. Let's learn more about running scripts and different execution options. Continue to [Running Scripts](./02-running-scripts.md).
