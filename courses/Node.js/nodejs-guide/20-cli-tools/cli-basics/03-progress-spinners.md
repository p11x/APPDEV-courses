# Progress & Spinners

## What You'll Learn

- How to add spinners for long-running tasks with ora
- How to create progress bars with cli-progress
- How to style terminal output with chalk
- How to combine spinners with async operations
- How to show multi-step task progress

## Project Setup

```bash
npm install ora cli-progress chalk
```

## Spinners with Ora

```js
// spinner.js — Loading spinners with ora

import ora from 'ora';

async function main() {
  // Basic spinner
  const spinner = ora('Loading...').start();

  // Simulate work
  await new Promise((r) => setTimeout(r, 2000));

  spinner.succeed('Data loaded successfully!');  // Green checkmark + message

  // Spinner that fails
  const failSpinner = ora('Connecting to database...').start();
  await new Promise((r) => setTimeout(r, 1500));
  failSpinner.fail('Database connection failed!');  // Red X + message

  // Spinner with info
  const infoSpinner = ora('Checking dependencies...').start();
  await new Promise((r) => setTimeout(r, 1000));
  infoSpinner.info('3 packages need updating');  // Blue info icon

  // Spinner with custom text updates
  const dynamicSpinner = ora('Step 1: Downloading...').start();
  await new Promise((r) => setTimeout(r, 1000));
  dynamicSpinner.text = 'Step 2: Extracting...';
  await new Promise((r) => setTimeout(r, 1000));
  dynamicSpinner.text = 'Step 3: Installing...';
  await new Promise((r) => setTimeout(r, 1000));
  dynamicSpinner.succeed('All steps complete!');

  // Spinner with warning
  const warnSpinner = ora('Validating config...').start();
  await new Promise((r) => setTimeout(r, 800));
  warnSpinner.warn('Config has 2 warnings');  // Yellow warning icon
}

main();
```

## Progress Bars with cli-progress

```js
// progress-bar.js — Progress bars with cli-progress

import cliProgress from 'cli-progress';

async function main() {
  // Create a progress bar
  const bar = new cliProgress.SingleBar({
    format: 'Downloading |{bar}| {percentage}% | {value}/{total} files | ETA: {eta}s',
    barCompleteChar: '█',    // Filled portion
    barIncompleteChar: '░',  // Empty portion
    hideCursor: true,        // Hide terminal cursor during progress
  }, cliProgress.Presets.shades_classic);

  const totalFiles = 50;
  bar.start(totalFiles, 0);  // Start the bar with total value

  // Simulate downloading files
  for (let i = 0; i < totalFiles; i++) {
    await new Promise((r) => setTimeout(r, 50 + Math.random() * 100));
    bar.increment();  // Advance by 1
  }

  bar.stop();  // Remove the bar from the terminal

  console.log('Download complete!');

  // === Multi-bar (multiple progress bars at once) ===

  const multibar = new cliProgress.MultiBar({
    clearOnComplete: false,
    hideCursor: true,
    format: '{task} |{bar}| {percentage}% | {value}/{total}',
  }, cliProgress.Presets.shades_classic);

  const bar1 = multibar.create(100, 0, { task: 'Downloading ' });
  const bar2 = multibar.create(100, 0, { task: 'Processing' });
  const bar3 = multibar.create(100, 0, { task: 'Uploading  ' });

  // Simulate concurrent operations with different speeds
  await Promise.all([
    simulateProgress(bar1, 30),
    simulateProgress(bar2, 50),
    simulateProgress(bar3, 80),
  ]);

  multibar.stop();
  console.log('All tasks complete!');
}

async function simulateProgress(bar, delay) {
  while (bar.value < bar.total) {
    await new Promise((r) => setTimeout(r, delay + Math.random() * 20));
    bar.increment();
  }
}

main();
```

## Terminal Colors with Chalk

```js
// colors.js — Styling terminal output with chalk

import chalk from 'chalk';

// Basic colors
console.log(chalk.red('Error: something went wrong'));
console.log(chalk.green('Success: operation complete'));
console.log(chalk.yellow('Warning: disk space low'));
console.log(chalk.blue('Info: processing started'));

// Styles
console.log(chalk.bold('Bold text'));
console.log(chalk.dim('Dim text'));
console.log(chalk.underline('Underlined text'));

// Chained styles
console.log(chalk.bold.red('Bold red error'));
console.log(chalk.bgRed.white(' White on red background '));

// Template literals with chalk
const user = 'Alice';
console.log(chalk`Hello, {green ${user}}!`);

// Custom themes for a CLI tool
const error = chalk.bold.red;
const success = chalk.bold.green;
const warning = chalk.yellow;
const info = chalk.blue;
const highlight = chalk.cyan.bold;

console.log(error('✖ Error: invalid configuration'));
console.log(success('✔ Config validated'));
console.log(warning('⚠ Deprecated API version'));
console.log(info('ℹ Using Node.js v20'));
console.log(highlight('→ Output: /dist/bundle.js'));
```

## Combined Example: Task Runner

```js
// task-runner.js — Multi-step task with spinner, progress, and colors

import ora from 'ora';
import cliProgress from 'cli-progress';
import chalk from 'chalk';

async function runTask(name, steps) {
  console.log(chalk.bold.cyan(`\n▸ ${name}`));

  for (const step of steps) {
    const spinner = ora(step.name).start();

    try {
      await step.run();
      spinner.succeed(chalk.green(step.name));
    } catch (err) {
      spinner.fail(chalk.red(`${step.name}: ${err.message}`));
      throw err;
    }
  }
}

// Define tasks
await runTask('Build Project', [
  {
    name: 'Installing dependencies',
    run: async () => {
      await new Promise((r) => setTimeout(r, 1000));
    },
  },
  {
    name: 'Compiling TypeScript',
    run: async () => {
      // Simulate compilation with progress bar
      const bar = new cliProgress.SingleBar({
        format: '  Compiling |{bar}| {percentage}%',
        barCompleteChar: '█',
        barIncompleteChar: '░',
      }, cliProgress.Presets.shades_classic);

      bar.start(100, 0);
      for (let i = 0; i < 100; i++) {
        await new Promise((r) => setTimeout(r, 20));
        bar.increment();
      }
      bar.stop();
    },
  },
  {
    name: 'Running tests',
    run: async () => {
      await new Promise((r) => setTimeout(r, 800));
    },
  },
  {
    name: 'Creating bundle',
    run: async () => {
      await new Promise((r) => setTimeout(r, 500));
    },
  },
]);

console.log(chalk.bold.green('\n✔ Build complete!\n'));
```

## How It Works

### Ora Lifecycle

```
ora('message').start()   →  Spinner animating
spinner.succeed('done')  →  ✔ done (green)
spinner.fail('error')    →  ✖ error (red)
spinner.warn('warn')     →  ⚠ warn (yellow)
spinner.info('info')     →  ℹ info (blue)
spinner.stop()           →  (removes spinner)
```

### Chalk Chaining

```js
chalk.bold.red.bgWhite('text')
// → bold + red text + white background
```

Chalk functions are chainable — each returns a new formatter with the added style.

## Common Mistakes

### Mistake 1: Not Stopping Spinners on Error

```js
// WRONG — spinner runs forever if the task throws
const spinner = ora('Working...').start();
await failingTask();  // Throws
spinner.succeed('Done');  // Never reached

// CORRECT — wrap in try/catch
const spinner = ora('Working...').start();
try {
  await failingTask();
  spinner.succeed('Done');
} catch (err) {
  spinner.fail(err.message);
}
```

### Mistake 2: Progress Bar Total Not Set

```js
// WRONG — bar shows NaN%
const bar = new cliProgress.SingleBar({});
bar.start();  // No total!

// CORRECT — provide total
bar.start(100, 0);
```

### Mistake 3: Chalk in Non-TTY Environments

```js
// WRONG — chalk output is garbled when piped to a file
// node mycli.js > output.txt  → ANSI escape codes in the file

// CORRECT — chalk auto-detects TTY, or force disable
import chalk from 'chalk';
const c = new chalk.Instance({ level: 0 });  // No colors
```

## Try It Yourself

### Exercise 1: Download Simulator

Create a CLI that simulates downloading 10 files with a progress bar. Show file names with ora spinners as each completes.

### Exercise 2: Color Log Levels

Create a logger that colorizes log levels: ERROR (red), WARN (yellow), INFO (blue), DEBUG (gray). Test with sample messages.

### Exercise 3: Build Dashboard

Show 3 progress bars simultaneously for parallel build tasks (lint, test, compile). Each finishes at a different time.

## Next Steps

You can create visual CLI output. For packaging your CLI as an installable tool, continue to [Binary Packaging](../cli-advanced/01-binary-packaging.md).
