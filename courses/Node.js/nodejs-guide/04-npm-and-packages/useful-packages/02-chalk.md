# Using Chalk for Terminal Colors

## What You'll Learn

- How to use chalk for colored terminal output
- Different color and style options
- Chaining styles
- Using chalk with ES modules

## What is Chalk?

**Chalk** is a Node.js library for styling terminal output with colors and styles. It's the most popular solution for making console output more readable.

## Installing Chalk

Chalk v5 is ESM-only. Install it with:

```bash
npm install chalk
```

## Basic Usage

```javascript
// chalk-basic.js - Basic chalk usage

import chalk from 'chalk';

console.log(chalk.red('Red text'));
console.log(chalk.green('Green text'));
console.log(chalk.blue('Blue text'));
```

## Available Colors

### Standard Colors

```javascript
import chalk from 'chalk';

console.log(chalk.black('Black'));
console.log(chalk.red('Red'));
console.log(chalk.green('Green'));
console.log(chalk.yellow('Yellow'));
console.log(chalk.blue('Blue'));
console.log(chalk.magenta('Magenta'));
console.log(chalk.cyan('Cyan'));
console.log(chalk.white('White'));

// Bright versions
console.log(chalk.redBright('Bright Red'));
console.log(chalk.greenBright('Bright Green'));
```

### Background Colors

```javascript
// Background colors - use bg prefix
console.log(chalk.bgRed('Red background'));
console.log(chalk.bgGreen('Green background'));
console.log(chalk.bgBlue('Blue background'));
```

### Text Styles

```javascript
// Text styles
console.log(chalk.bold('Bold text'));
console.log(chalk.dim('Dim text'));
console.log(chalk.italic('Italic text'));
console.log(chalk.underline('Underlined text'));
console.log(chalk.strikethrough('Strikethrough'));
```

## Chaining Styles

```javascript
// chalk-chain.js - Chaining chalk styles

import chalk from 'chalk';

// Chain multiple styles
console.log(chalk.bold.green('Bold and green'));
console.log(chalk.red.bold.underline('Red, bold, underlined'));
console.log(chalk.bgYellow.black('Yellow background with black text'));
```

## Code Example: Complete Chalk Demo

```javascript
// chalk-demo.js - Complete chalk demonstration

import chalk from 'chalk';

console.log('=== Chalk Demo ===\n');

// ─────────────────────────────────────────
// 1. Basic Colors
// ─────────────────────────────────────────
console.log('1. Basic Colors:');
console.log(chalk.red('Error'));
console.log(chalk.green('Success'));
console.log(chalk.blue('Info'));

// ─────────────────────────────────────────
// 2. Background Colors
// ─────────────────────────────────────────
console.log('\n2. Background Colors:');
console.log(chalk.bgRed('Error background'));
console.log(chalk.bgGreen('Success background'));

// ─────────────────────────────────────────
// 3. Text Styles
// ─────────────────────────────────────────
console.log('\n3. Text Styles:');
console.log(chalk.bold('Bold'));
console.log(chalk.underline('Underlined'));
console.log(chalk.strikethrough('Strikethrough'));

// ─────────────────────────────────────────
// 4. Combined Styles
// ─────────────────────────────────────────
console.log('\n4. Combined Styles:');
console.log(chalk.bold.red('Bold red error!'));
console.log(chalk.green.bgBlack.bold('Success!'));
console.log(chalk.blue.underline('Click here'));

// ─────────────────────────────────────────
// 5. Template Literals
// ─────────────────────────────────────────
console.log('\n5. Template Literals:');

const name = 'Alice';
const count = 5;

console.log(chalk`Hello {green ${name}}!`);
console.log(chalk`You have {bold ${count}} messages`);

// ─────────────────────────────────────────
// 6. Real-World Example
// ─────────────────────────────────────────
console.log('\n6. Real-World Example:');

function logMessage(type, message) {
  const styles = {
    info: chalk.blue('[INFO]'),
    success: chalk.green('[SUCCESS]'),
    warning: chalk.yellow('[WARNING]'),
    error: chalk.red('[ERROR]')
  };
  
  console.log(`${styles[type]} ${message}`);
}

logMessage('info', 'Server started');
logMessage('success', 'Connected to database');
logMessage('warning', 'Cache miss');
logMessage('error', 'Failed to connect');
```

## Using in Logging

```javascript
// chalk-logging.js - Styled logging

import chalk from 'chalk';

const logger = {
  info: (...args) => console.log(chalk.blue('[INFO]'), ...args),
  success: (...args) => console.log(chalk.green('[SUCCESS]'), ...args),
  warning: (...args) => console.log(chalk.yellow('[WARNING]'), ...args),
  error: (...args) => console.log(chalk.red('[ERROR]'), ...args)
};

logger.info('Application started');
logger.success('Database connected');
logger.warning('Using fallback value');
logger.error('Connection failed');
```

## Common Mistakes

### Mistake 1: Wrong Import

```javascript
// WRONG - Chalk v5 is ESM only
const chalk = require('chalk');

// CORRECT - Use import
import chalk from 'chalk';
```

### Mistake 2: String Concatenation Instead of Template Literals

```javascript
// Works but less clean
console.log(chalk.red('Error: ' + message));

// Better - template literals
console.log(chalk.red(`Error: ${message}`));
```

## Try It Yourself

### Exercise 1: Basic Colors
Create output with multiple different colors.

### Exercise 2: Styled Logging
Create a logger with colored prefixes.

### Exercise 3: Status Display
Create a status display showing different states with colors.

## Next Steps

Now you know about chalk. Let's learn about Zod for validation. Continue to [zod Package](./03-zod.md).
