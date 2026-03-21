# Module Resolution in Node.js

## What You'll Learn

- How Node.js finds modules
- The role of package.json type field
- Relative vs absolute imports
- Node module resolution algorithm

## Module Resolution Overview

When you write `import x from './module.js'`, Node.js needs to find that file. This is called **module resolution**.

## The Resolution Process

### Relative Imports

```javascript
// Import from current directory
import './utils.js';           // Same directory
import './lib/helper.js';      // Subdirectory
import '../services/api.js';   // Parent directory
```

### Absolute Imports

```javascript
// Import from node_modules
import express from 'express';
import lodash from 'lodash';

// Import from package
import { something } from 'some-package';
```

## package.json Type Field

The `type` field determines module system:

```json
{
  "type": "module"   // ES Modules
}
```

or

```json
{
  "type": "commonjs"  // CommonJS (default)
}
```

### ES Modules

```javascript
// When type: "module"
import { something } from './module.js';
export const value = 1;
```

### CommonJS

```javascript
// When type: "commonjs"
const something = require('./module.js');
module.exports = { value: 1 };
```

## Node Module Resolution Algorithm

### For Local Modules

1. Check if the path starts with `.` or `/`
2. If file exists as-is, use it
3. If not, try adding extensions: `.js`, `.json`
4. Try as directory with `index.js`

### For Node Modules

1. Check `node_modules` in current directory
2. Check `node_modules` in parent directories
3. Check up to system root

## Extensions Resolution

Node.js tries different extensions:

```javascript
// For './config':
// 1. ./config.js (if type=module)
// 2. ./config.json
// 3. ./config/index.js (package.json in folder)
// 4. ./config/index.json
```

## Code Example: Resolution Demo

### Project Structure

```
my-project/
├── src/
│   ├── index.js
│   ├── utils/
│   │   ├── index.js
│   │   └── helpers.js
│   └── lib/
│       └── api.js
├── package.json
└── node_modules/
    └── express/
```

### Resolution in Action

```javascript
// src/index.js

// Relative import - looks in same directory
import utils from './utils/index.js';

// Relative import - looks in lib directory
import api from '../lib/api.js';

// Node module - looks in node_modules
import express from 'express';
```

### index.js Exports

```javascript
// src/utils/index.js
export const add = (a, b) => a + b;
export const subtract = (a, b) => a - b;
```

### Using index Files

```javascript
// When importing a directory, Node looks for index.js
import { add, subtract } from './utils/index.js';
// OR (since index is special)
import { add, subtract } from './utils';
```

## Import Maps (Advanced)

Import maps let you define module aliases:

```json
{
  "imports": {
    "@utils": "./src/utils/",
    "@lib": "./src/lib/",
    "express": "express@4.18.0"
  }
}
```

Then use:

```javascript
import { add } from '@utils/helpers.js';
```

## Common Mistakes

### Mistake 1: Forgetting File Extension

```javascript
// WRONG - ESM requires explicit extension
import utils from './utils';

// CORRECT
import utils from './utils.js';
```

### Mistake 2: Wrong Type Setting

```javascript
// package.json has "type": "commonjs"
// But trying to use import

// index.js
import { something } from './module.js';  // ERROR!

// Fix: change package.json to "type": "module"
```

### Mistake 3: Mixing Module Systems

```javascript
// In ESM file (type: module)
import { something } from './module.js';

// module.js uses require
const x = require('./other.js');  // Might not work!

// Stick to one system per project
```

## Try It Yourself

### Exercise 1: Create Module Structure
Create a project with multiple folders and import between them.

### Experiment with Extensions
Try importing without extensions and see what happens.

### Explore node_modules
Look in node_modules to see how packages are structured.

## Next Steps

Now you understand module resolution. Let's learn about dynamic imports. Continue to [Dynamic Import](./03-dynamic-import.md).
