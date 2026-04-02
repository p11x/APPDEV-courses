# Commander Setup

## What You'll Learn

- How to create a CLI tool with the `commander` library
- How to define commands, options, and arguments
- How to add version information and help text
- How to parse `process.argv` automatically
- How to handle subcommands

## What Is Commander?

Commander is a library for building CLI tools in Node.js. It handles:
- **Parsing arguments** — converts `--name Alice` into `{ name: 'Alice' }`
- **Help text** — auto-generates `--help` output
- **Version** — `--version` prints the version
- **Subcommands** — `git commit`, `git push`, etc.

## Project Setup

```bash
mkdir my-cli && cd my-cli
npm init -y
npm install commander
```

Update `package.json`:

```json
{
  "name": "my-cli",
  "version": "1.0.0",
  "type": "module",
  "bin": {
    "my-cli": "./index.js"
  }
}
```

## Basic CLI

```js
#!/usr/bin/env node
// index.js — Basic CLI tool with commander

import { Command } from 'commander';

// Create the program — the root command
const program = new Command();

program
  .name('my-cli')                    // Name shown in help
  .description('A sample CLI tool')  // Description shown in help
  .version('1.0.0');                 // --version prints this

// Define a command
program
  .command('greet')                  // Command name
  .description('Greet someone')      // Shown in help
  .argument('<name>', 'Person to greet')  // Required argument
  .option('-l, --loud', 'Shout the greeting')  // Optional boolean flag
  .option('-g, --greeting <text>', 'Custom greeting', 'Hello')  // Option with value
  .action((name, options) => {
    // name is the positional argument
    // options is an object with the parsed flags
    let message = `${options.greeting}, ${name}!`;

    if (options.loud) {
      message = message.toUpperCase();
    }

    console.log(message);
  });

// Another command
program
  .command('add')
  .description('Add two numbers')
  .argument('<a>', 'First number')
  .argument('<b>', 'Second number')
  .action((a, b) => {
    const sum = parseFloat(a) + parseFloat(b);
    console.log(`${a} + ${b} = ${sum}`);
  });

// Parse command line arguments
// This reads process.argv and calls the appropriate action
program.parse();
```

### Running

```bash
node index.js greet Alice
# Hello, Alice!

node index.js greet Alice --loud
# HELLO, ALICE!

node index.js greet Alice -g "Hey"
# Hey, Alice!

node index.js add 5 3
# 5 + 3 = 8

node index.js --help
# Shows auto-generated help text

node index.js --version
# 1.0.0
```

## Subcommands with Separate Files

```js
// index.js — Main program with subcommand loading

import { Command } from 'commander';
import { readFile } from 'node:fs/promises';

const program = new Command();

program
  .name('task')
  .description('Task management CLI')
  .version('1.0.0');

// Load subcommands from separate files
// Each file exports a function that receives the program
const { default: addCommand } = await import('./commands/add.js');
const { default: listCommand } = await import('./commands/list.js');

addCommand(program);
listCommand(program);

program.parse();
```

```js
// commands/add.js — Add a task subcommand

export default function addCommand(program) {
  program
    .command('add')
    .description('Add a new task')
    .argument('<title>', 'Task title')
    .option('-p, --priority <level>', 'Priority level (low, medium, high)', 'medium')
    .option('-t, --tags <tags...>', 'Task tags')  // ... means multiple values
    .action((title, options) => {
      const task = {
        id: Date.now(),
        title,
        priority: options.priority,
        tags: options.tags || [],
        done: false,
        createdAt: new Date().toISOString(),
      };

      console.log('Task added:');
      console.log(`  Title: ${task.title}`);
      console.log(`  Priority: ${task.priority}`);
      console.log(`  Tags: ${task.tags.join(', ') || 'none'}`);
    });
}
```

```js
// commands/list.js — List tasks subcommand

export default function listCommand(program) {
  program
    .command('list')
    .description('List all tasks')
    .option('-f, --filter <status>', 'Filter by status (done, pending)')
    .option('--sort <field>', 'Sort by field (title, priority)', 'title')
    .action((options) => {
      // Simulated tasks
      const tasks = [
        { title: 'Buy groceries', priority: 'high', done: false },
        { title: 'Write docs', priority: 'medium', done: true },
        { title: 'Fix bug', priority: 'high', done: false },
      ];

      let filtered = tasks;
      if (options.filter === 'done') filtered = tasks.filter((t) => t.done);
      if (options.filter === 'pending') filtered = tasks.filter((t) => !t.done);

      filtered.sort((a, b) => a[options.sort]?.localeCompare(b[options.sort]));

      console.log(`Tasks (${filtered.length}):`);
      for (const t of filtered) {
        const status = t.done ? '✓' : '○';
        console.log(`  ${status} [${t.priority}] ${t.title}`);
      }
    });
}
```

## How It Works

### Argument Types

| Syntax | Type | Example |
|--------|------|---------|
| `<name>` | Required | `greet <name>` |
| `[name]` | Optional | `greet [name]` |
| `<name...>` | Required, multiple | `add <files...>` |

### Option Types

| Syntax | Type | Example |
|--------|------|---------|
| `-v, --verbose` | Boolean flag | `--verbose` → `true` |
| `-n, --name <value>` | Required value | `--name Alice` |
| `-n, --name [value]` | Optional value | `--name` or `--name Alice` |
| `-t, --tags <values...>` | Multiple values | `--tags a b c` |

### Default Values

```js
.option('-p, --port <port>', 'Port number', '3000')
// Default is '3000' if --port is not provided
```

## Common Mistakes

### Mistake 1: Forgetting to Call `program.parse()`

```js
// WRONG — program never processes arguments
const program = new Command();
program.command('greet').action(() => console.log('Hello'));
// Nothing happens — parse() was not called

// CORRECT — always call parse() at the end
program.parse();
```

### Mistake 2: Wrong Argument Order in Action

```js
// WRONG — options are the second argument, not the third
program
  .command('greet')
  .argument('<name>')
  .option('-l, --loud')
  .action((name, extra, options) => {  // extra is undefined
    console.log(options.loud);
  });

// CORRECT — action receives (arguments..., options)
program
  .command('greet')
  .argument('<name>')
  .option('-l, --loud')
  .action((name, options) => {  // options is always the last argument
    console.log(options.loud);
  });
```

### Mistake 3: Not Adding the Shebang

```js
// WRONG — running the file with `./index.js` fails
// (system does not know to use Node.js)
import { Command } from 'commander';
// ...

// CORRECT — add the shebang line at the very top
#!/usr/bin/env node
import { Command } from 'commander';
```

## Try It Yourself

### Exercise 1: Timer CLI

Create a `timer` command: `timer start` begins a timer, `timer stop` stops it and shows elapsed time.

### Exercise 2: File Counter

Create `count <directory>` that counts files and directories. Add `--ext <extension>` to filter by extension.

### Exercise 3: Nested Commands

Create `db migrate`, `db rollback`, and `db status` subcommands using the separate-file pattern.

## Next Steps

You can build CLI tools with commands. For interactive user input, continue to [Interactive Prompts](./02-interactive-prompts.md).
