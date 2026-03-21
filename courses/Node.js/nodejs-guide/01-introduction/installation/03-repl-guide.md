# Node REPL Guide

## What You'll Learn

- What the Node REPL is and when to use it
- How to navigate and use REPL commands
- How to experiment with JavaScript interactively
- Tips for getting the most out of the REPL

## What is the REPL?

**REPL** stands for **Read-Eval-Print Loop**. It's an interactive programming environment where you can type JavaScript code and see immediate results. Think of it as a playground where you can experiment without creating files.

The REPL is built into Node.js, so you can start it with a single command.

## Starting the REPL

### Basic Start

Open your terminal and type:

```bash
node
```

You'll see something like:

```
Welcome to Node.js v20.11.0.
Type ".help" for more information.
>
```

The `>` symbol is the REPL prompt. It's waiting for you to type JavaScript.

### Exit the REPL

To exit the REPL, type:
- `.exit` and press Enter
- Or press Ctrl+C twice (Ctrl+D on macOS/Linux)

## Using the REPL

### Basic Math

Try typing some math expressions:

```
> 2 + 2
4
> 10 - 5
5
> 3 * 4
12
> 10 / 2
5
> 5 % 3   // Remainder operator
2
```

### Variables and Assignment

You can create variables and use them:

```
> let x = 10
undefined
> let y = 20
undefined
> x + y
30
> let name = "Alice"
undefined
> Hello, ${name}!
'Hello, Alice!'
```

Note: In the REPL, `let` declarations automatically print `undefined` after assignment (that's just how the REPL works).

### Functions

Create and call functions:

```
> function greet(person) {
...   return "Hello, " + person + "!";
... }
undefined
> greet("World")
'Hello, World!'
```

Notice how `...` appears for multi-line input. The REPL knows you're not done until you close your function.

### Working with Objects

```
> const user = { name: "Alice", age: 25 }
undefined
> user.name
'Alice'
> user.age
25
> Object.keys(user)
[ 'name', 'age' ]
```

## REPL Commands

The REPL has special commands that start with a dot (`.`). These aren't JavaScript—they're commands for the REPL itself.

### .help - Show Available Commands

```
> .help
.break    Sometimes you get stuck, this gets you out
.clear    Alias for .break
.editor   Enter editor mode
.exit     Exit the REPL
.help     Show help options
.load     Load JS from a file
.save     Save current REPL session to a file
```

### .clear - Clear the REPL Context

```
> .clear
```

This clears all variables and resets the REPL state.

### .save - Save Your Session

```
> .save my-session.js
```

This saves all the code you've typed in the current REPL session to a file.

### .load - Load a File

```
> .load my-script.js
```

This loads and executes a JavaScript file in the REPL.

### .editor - Enter Editor Mode

```
> .editor
# Press Enter
# Now you can type multiple lines
# Press Ctrl+D to finish
```

This opens a multi-line editor mode, useful for writing larger functions.

## Code Example: Using the REPL

Let's practice using the REPL with some practical examples:

### Example 1: String Manipulation

```
> const text = "Hello, Node.js!"
undefined
> text.toUpperCase()
'HELLO, NODE.JS!'
> text.length
15
> text.split(', ')
[ 'Hello', 'Node.js!' ]
```

### Example 2: Arrays

```
> const numbers = [1, 2, 3, 4, 5]
undefined
> numbers.push(6)
6
> numbers
[ 1, 2, 3, 4, 5, 6 ]
> numbers.filter(n => n > 3)
[ 4, 5, 6 ]
> numbers.map(n => n * 2)
[ 2, 4, 6, 8, 10, 12 ]
```

### Example 3: Using Node.js Modules

```
> const path = require('path')
[Function: resolve]
> path.join('folder', 'file.txt')
'folder\\file.txt'   // On Windows
// or 'folder/file.txt' on macOS/Linux
```

### Example 4: Async Operations

You can even use async/await in the REPL:

```
> async function getData() {
...   return "Hello async!";
... }
undefined
> await getData()
'Hello async!'
```

Note: `await` only works inside async functions in the REPL.

## REPL Features

### Tab Completion

The REPL supports tab completion for:
- Global variables
- Module names (after `require(`)
- Object properties

Try typing `process.` and press Tab to see available properties.

### Underscore Variable

The underscore `_` stores the last result:

```
> 10 + 20
30
> _ * 2
60
```

### REPL History

Use arrow keys to:
- **Up/Down**: Navigate through command history
- **Left/Right**: Edit current line
- **Ctrl+A**: Move to beginning of line
- **Ctrl+E**: Move to end of line

## Common Mistakes

### Mistake 1: Trying to Exit with Ctrl+Z

On Windows, Ctrl+Z means "end of file" and doesn't exit. Use `.exit` or Ctrl+C twice instead.

### Mistake 2: Forgetting Multi-line Mode

If you're writing a function or if statement that spans multiple lines, the REPL shows `...` and expects you to complete it. If you get stuck, type `.break` to start over.

### Mistake 3: Expecting Persistent Variables

Variables created in the REPL are lost when you exit. Use `.save` to save your work or write code in a file instead.

## Try It Yourself

### Exercise 1: String Practice
In the REPL, create a string with your name and:
- Convert it to uppercase
- Get its length
- Use template literals to add "Hello" before it

### Exercise 2: Array Methods
Create an array of numbers in the REPL and practice:
- `.map()` to double each number
- `.filter()` to keep only numbers greater than 5
- `.reduce()` to sum all numbers

### Exercise 3: Explore Built-in Modules
In the REPL, explore the `os` module:
```
> const os = require('os')
> os.cpus()
> os.freemem()
> os.homedir()
```

## When to Use the REPL

The REPL is great for:
- Quick experiments and prototyping
- Testing small code snippets
- Learning JavaScript syntax
- Exploring Node.js APIs

For larger projects or code you want to keep, use files instead.

## Next Steps

Now you know how to use the REPL for interactive JavaScript. Let's write your first real Node.js program using files. Continue to [Hello World](../first-program/01-hello-world.md) to learn how to create and run your first Node.js script.
