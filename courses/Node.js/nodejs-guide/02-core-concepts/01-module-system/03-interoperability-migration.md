# Module Interoperability and Migration Strategies

## What You'll Learn

- Using CommonJS and ES Modules together
- Migration strategies from CJS to ESM
- Handling mixed module projects
- Package publishing for dual support

## CJS/ESM Interoperability

### Importing CJS from ESM

```javascript
// ESM file importing CommonJS module
import { createRequire } from 'node:module';
const require = createRequire(import.meta.url);

// Import CJS module
const legacyModule = require('./legacy-module.js');
const lodash = require('lodash');

// CJS named exports work if they're static
import { helper } from './cjs-module.js'; // Works if cjs exports are static

// Default import from CJS
import legacyModule from './legacy-module.js';
// Gets module.exports value
```

### Importing ESM from CJS (Node.js 22+)

```javascript
// CommonJS file importing ES Module (require(esm) in Node.js 22+)
// Must use dynamic import()
const { add } = await import('./esm-module.js');

// Or in an async context
async function loadModule() {
    const { helper } = await import('./utils.mjs');
    return helper();
}
```

### Dual Package Pattern

```json
// package.json — Support both CJS and ESM consumers
{
    "name": "my-package",
    "type": "module",
    "exports": {
        ".": {
            "import": {
                "types": "./dist/index.d.mts",
                "default": "./dist/index.mjs"
            },
            "require": {
                "types": "./dist/index.d.cts",
                "default": "./dist/index.cjs"
            }
        }
    },
    "main": "./dist/index.cjs",
    "module": "./dist/index.mjs",
    "types": "./dist/index.d.ts",
    "files": ["dist"]
}
```

### Build Script for Dual Output

```javascript
// build.js — Build CJS and ESM from TypeScript source

import { build } from 'esbuild';

// ESM build
await build({
    entryPoints: ['src/index.ts'],
    bundle: true,
    platform: 'node',
    target: 'node20',
    format: 'esm',
    outfile: 'dist/index.mjs',
    external: ['*'], // Externalize all dependencies
});

// CJS build
await build({
    entryPoints: ['src/index.ts'],
    bundle: true,
    platform: 'node',
    target: 'node20',
    format: 'cjs',
    outfile: 'dist/index.cjs',
    external: ['*'],
});

console.log('Dual CJS/ESM build complete');
```

## Migration Strategy

### Phase 1: Enable ESM

```json
// Add to package.json
{
    "type": "module"
}
```

```bash
# Fix extensions — ESM requires .js extensions
# Find imports without extensions
grep -r "from './" src/ | grep -v ".js'"
```

### Phase 2: Fix Imports

```javascript
// BEFORE (CJS style, no extension)
import { helper } from './utils';
import config from './config.json';

// AFTER (ESM style, with extension)
import { helper } from './utils.js';
import config from './config.json' with { type: 'json' };
// or: import config from './config.json' assert { type: 'json' };
```

### Phase 3: Replace __dirname/__filename

```javascript
// BEFORE (CJS)
const path = require('path');
const __dirname = path.dirname(__filename);
const configPath = path.join(__dirname, 'config.json');

// AFTER (ESM)
import { fileURLToPath } from 'node:url';
import { dirname, join } from 'node:path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const configPath = join(__dirname, 'config.json');
```

### Phase 4: Replace require() Patterns

```javascript
// BEFORE (CJS)
const fs = require('fs');
const data = fs.readFileSync('./data.json', 'utf-8');

const dynamic = condition ? require('./a') : require('./b');

// AFTER (ESM)
import { readFileSync } from 'node:fs';
const data = readFileSync(new URL('./data.json', import.meta.url), 'utf-8');

// Dynamic import
const dynamic = condition
    ? await import('./a.js')
    : await import('./b.js');
```

## Migration Checklist

```
CJS → ESM Migration Checklist:
─────────────────────────────────────────────
□ Add "type": "module" to package.json
□ Add .js extensions to all relative imports
□ Replace require() with import
□ Replace module.exports with export
□ Replace __dirname with import.meta.url derivation
□ Replace __filename with fileURLToPath(import.meta.url)
□ Use node: prefix for built-in modules
□ Replace require.resolve() with import.meta.resolve()
□ Handle JSON imports with import assertions
□ Test all imports and exports
□ Update test configuration
□ Update build scripts
□ Update CI/CD pipelines
```

## Common Issues and Fixes

```
Error: "Cannot use import statement outside a module"
Fix: Add "type": "module" to package.json

Error: "ERR_MODULE_NOT_FOUND" for './utils'
Fix: Add .js extension → './utils.js'

Error: "__dirname is not defined in ES module scope"
Fix: Use import.meta.url + fileURLToPath + dirname

Error: "ERR_UNKNOWN_FILE_EXTENSION .json"
Fix: Use import assertion: import data from './file.json' with { type: 'json' }

Error: "Must use import to load ES Module"
Fix: Use .mjs extension or "type": "module"
```

## Cross-References

- See [CommonJS Deep Dive](./01-commonjs-deep-dive.md) for CJS details
- See [ES Modules](./02-es-modules-implementation.md) for ESM details
- See [Built-in Modules](../02-built-in-modules/01-fs-path-os-modules.md) for core modules

## Next Steps

Continue to [Built-in Modules](../02-built-in-modules/01-fs-path-os-modules.md) for core module usage.
