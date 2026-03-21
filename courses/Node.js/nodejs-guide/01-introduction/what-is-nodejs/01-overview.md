# What is Node.js?

## What You'll Learn

- What Node.js is and why it was created
- How the V8 JavaScript engine powers Node.js
- The difference between server-side and client-side JavaScript
- Why Node.js is popular for building web applications

## Understanding Node.js

**Node.js** is a runtime environment that allows you to run JavaScript code outside of a web browser. Before Node.js, JavaScript could only run inside browsers like Chrome, Firefox, or Safari. Node.js changed that by taking JavaScript from the browser and making it work on servers, laptops, and any computer.

### Why Was Node.js Created?

In 2009, Ryan Dahl created Node.js because he wanted to build real-time websites with push capabilities. At the time, web servers typically handled each request by creating a new thread (a separate line of execution). This approach uses a lot of memory when handling many simultaneous connections.

Node.js uses a different approach called **event-driven, non-blocking I/O** (Input/Output). This means when Node.js is waiting for something (like reading a file or waiting for data from a database), it doesn't stop—it can handle other requests in the meantime. This makes Node.js very efficient for applications that need to handle many simultaneous connections.

### The V8 Engine: Node.js's Heart

Node.js is built on top of **V8**, the same JavaScript engine that powers Google Chrome. V8 compiles JavaScript into machine code (binary code that computers understand directly), making it extremely fast.

Here's how it works:

1. You write JavaScript code
2. V8 compiles your code into machine code
3. Your computer's processor executes the machine code

V8 is written in C++ and is open source. Node.js adds additional features to V8, like file system access, network connections, and more.

## Code Example: Running JavaScript with Node.js

Create a file named `hello.js` and add this code:

```javascript
// This is a simple greeting function that returns a personalized message
function greet(name) {
  return `Hello, ${name}! Welcome to Node.js.`;
}

// Call the function and store the result in a variable
const message = greet('World');

// Print the message to the console
console.log(message);

// Print Node.js version information
console.log(`Running on Node.js version: ${process.version}`);
```

Run this script using the Node.js command:

```bash
node hello.js
```

You should see output like:

```
Hello, World! Welcome to Node.js.
Running on Node.js version: v20.x.x
```

## How It Works

Let's break down the code above:

1. **Function Definition**: We define a `greet` function that takes a `name` parameter and returns a template literal string (text with embedded variables using backticks `` ` ``).

2. **Variable Assignment**: We use `const` to create a variable called `message` that stores the result of calling `greet('World')`. Using `const` means this variable cannot be reassigned to a different value.

3. **console.log()**: This is a built-in function that prints text to the terminal. It's the same function you use in browser developer tools.

4. **process.version**: This is a special Node.js global object called `process` that gives you information about the current Node.js environment. The `version` property tells you which version of Node.js is running.

## Common Mistakes

### Mistake 1: Confusing Node.js with a Framework
Node.js is NOT a framework (like React or Express). It's a **runtime environment**—a platform that executes JavaScript code. Frameworks like Express are built ON TOP of Node.js.

### Mistake 2: Using Browser-Only APIs
Some JavaScript code that works in browsers won't work in Node.js. For example:
- `window` doesn't exist in Node.js
- `document` doesn't exist in Node.js
- `fetch()` is available in Node.js v18+ but may behave differently

### Mistake 3: Forgetting That Node.js Uses CommonJS by Default (Historically)
Older versions of Node.js used CommonJS (`require()`) by default. Modern Node.js (v20+) prefers ES Modules (`import/export`). We'll cover this in detail later.

## Try It Yourself

### Exercise 1: Create Your First Script
Create a file called `my-info.js` that prints:
- Your name
- The current Node.js version
- The current working directory

### Exercise 2: Do Some Math
Create a script that calculates the area of a rectangle (length × width) and prints the result with a descriptive message.

### Exercise 3: Experiment with process Object
Create a script that prints different properties of the `process` object, such as:
- `process.platform` (your operating system)
- `process.arch` (your computer architecture)
- `process.uptime()` (how long Node.js has been running)

## Next Steps

Now that you understand what Node.js is, let's explore how Node.js handles tasks internally. Continue to [The Event Loop](../02-event-loop.md) to learn about the core mechanism that makes Node.js efficient.
