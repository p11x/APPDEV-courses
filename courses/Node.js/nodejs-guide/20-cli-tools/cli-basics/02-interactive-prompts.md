# Interactive Prompts

## What You'll Learn

- How to use `@inquirer/prompts` for interactive CLI input
- How to create text input, selection menus, confirmations, and checkboxes
- How to validate user input in prompts
- How to chain multiple prompts together
- How to build an interactive setup wizard

## What Are Interactive Prompts?

Commander handles command-line arguments. **Prompts** handle interactive questions — the user types or selects answers in the terminal.

## Project Setup

```bash
npm install @inquirer/prompts
```

## Basic Prompts

```js
// prompts.js — Interactive prompt examples

import { input, select, confirm, password, checkbox } from '@inquirer/prompts';

async function main() {
  // Text input — user types a string
  const name = await input({
    message: 'What is your name?',
    default: 'Alice',           // Pre-filled value (press Enter to accept)
    validate: (value) => {
      if (value.trim().length < 2) return 'Name must be at least 2 characters';
      return true;  // Valid
    },
  });

  console.log(`Hello, ${name}!`);

  // Selection menu — user picks one option
  const framework = await select({
    message: 'Choose a framework:',
    choices: [
      { name: 'Express', value: 'express', description: 'Minimal and flexible' },
      { name: 'Fastify', value: 'fastify', description: 'High performance' },
      { name: 'Koa', value: 'koa', description: 'Expressive middleware' },
    ],
  });

  console.log(`Selected: ${framework}`);

  // Confirmation — yes/no question
  const useTypeScript = await confirm({
    message: 'Use TypeScript?',
    default: false,
  });

  console.log(`TypeScript: ${useTypeScript}`);

  // Password input — characters are hidden
  const secret = await password({
    message: 'Enter a secret key:',
    validate: (value) => {
      if (value.length < 8) return 'Secret must be at least 8 characters';
      return true;
    },
  });

  console.log(`Secret length: ${secret.length} characters`);

  // Checkbox — user selects multiple options
  const features = await checkbox({
    message: 'Select features to include:',
    choices: [
      { name: 'ESLint', value: 'eslint', checked: true },  // Pre-checked
      { name: 'Prettier', value: 'prettier' },
      { name: 'Husky', value: 'husky' },
      { name: 'Jest', value: 'jest', checked: true },
    ],
  });

  console.log(`Features: ${features.join(', ')}`);

  // Summary
  console.log('\n--- Summary ---');
  console.log(`Name: ${name}`);
  console.log(`Framework: ${framework}`);
  console.log(`TypeScript: ${useTypeScript}`);
  console.log(`Features: ${features.join(', ')}`);
}

main().catch(console.error);
```

## Setup Wizard

```js
// wizard.js — Project setup wizard

import { input, select, confirm, checkbox } from '@inquirer/prompts';
import { mkdir, writeFile } from 'node:fs/promises';
import { resolve } from 'node:path';

async function wizard() {
  console.log('🛠  Project Setup Wizard\n');

  const projectName = await input({
    message: 'Project name:',
    validate: (v) => /^[a-z0-9-]+$/.test(v) || 'Use lowercase letters, numbers, and hyphens',
  });

  const description = await input({
    message: 'Description:',
    default: '',
  });

  const type = await select({
    message: 'Project type:',
    choices: [
      { name: 'API Server', value: 'api' },
      { name: 'CLI Tool', value: 'cli' },
      { name: 'Library', value: 'lib' },
    ],
  });

  const features = await checkbox({
    message: 'Include features:',
    choices: [
      { name: 'ESLint', value: 'eslint' },
      { name: 'Prettier', value: 'prettier' },
      { name: 'Docker', value: 'docker', disabled: type === 'lib' },
      { name: 'GitHub Actions CI', value: 'ci' },
    ],
  });

  const useGit = await confirm({
    message: 'Initialize git repository?',
    default: true,
  });

  // Generate the project
  const dir = resolve(process.cwd(), projectName);
  await mkdir(dir, { recursive: true });

  const packageJson = {
    name: projectName,
    version: '1.0.0',
    description,
    type: 'module',
    main: type === 'cli' ? './index.js' : './src/index.js',
    scripts: {
      start: 'node src/index.js',
      test: 'echo "No tests yet"',
    },
    keywords: [],
    license: 'MIT',
  };

  if (type === 'cli') {
    packageJson.bin = { [projectName]: './index.js' };
  }

  await writeFile(
    resolve(dir, 'package.json'),
    JSON.stringify(packageJson, null, 2) + '\n'
  );

  await mkdir(resolve(dir, 'src'), { recursive: true });
  await writeFile(
    resolve(dir, 'src', 'index.js'),
    `console.log('${projectName} is running!');\n`
  );

  console.log(`\n✅ Project created at ${dir}`);
  console.log(`\nNext steps:`);
  console.log(`  cd ${projectName}`);
  console.log(`  npm install`);
  if (useGit) console.log(`  git init && git add . && git commit -m "Initial commit"`);
}

wizard().catch(console.error);
```

## How It Works

### Prompt Return Values

| Prompt | Returns | Example |
|--------|---------|---------|
| `input()` | `string` | `"Alice"` |
| `select()` | `string` (value) | `"express"` |
| `confirm()` | `boolean` | `true` |
| `password()` | `string` | `"secret123"` |
| `checkbox()` | `string[]` | `["eslint", "jest"]` |

### Validation

Each prompt accepts a `validate` function. Return `true` for valid input or a string error message for invalid input. The prompt re-asks until validation passes.

## Common Mistakes

### Mistake 1: Not Awaiting Prompts

```js
// WRONG — prompt returns a Promise
const name = input({ message: 'Name?' });
console.log(name);  // Promise { <pending> }

// CORRECT — await the result
const name = await input({ message: 'Name?' });
console.log(name);  // "Alice"
```

### Mistake 2: No Default Values for Common Options

```js
// WRONG — user must type everything from scratch
const port = await input({ message: 'Port?' });

// CORRECT — provide sensible defaults
const port = await input({ message: 'Port?', default: '3000' });
```

### Mistake 3: Confusing select() choices Format

```js
// WRONG — passing an array of strings
const choice = await select({ message: 'Pick:', choices: ['a', 'b', 'c'] });

// CORRECT — use objects with name and value
const choice = await select({
  message: 'Pick:',
  choices: [
    { name: 'Option A', value: 'a' },
    { name: 'Option B', value: 'b' },
  ],
});
```

## Try It Yourself

### Exercise 1: User Registration

Build a registration form: username (validated, no spaces), email (must contain @), password (min 8 chars), confirm password (must match). Save to a JSON file.

### Exercise 2: Survey Builder

Create an interactive survey that asks 5 questions and saves the answers. Support text, yes/no, and multiple-choice question types.

### Exercise 3: Database Config

Build a prompt chain that asks for database type (Postgres/MySQL/SQLite), host, port, username, password, and database name. Output a `.env` file.

## Next Steps

You can collect user input interactively. For visual feedback like spinners and progress bars, continue to [Progress & Spinners](./03-progress-spinners.md).
