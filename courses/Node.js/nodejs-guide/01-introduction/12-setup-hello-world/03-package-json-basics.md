# Package.json Basics and Project Configuration

## What You'll Learn

- Creating and understanding `package.json`
- Configuring scripts, dependencies, and project metadata
- Understanding semantic versioning
- Best practices for project configuration

## Creating package.json

### Method 1: Interactive Setup

```bash
npm init
# Answer the prompts (name, version, description, etc.)
```

### Method 2: Quick Setup with Defaults

```bash
npm init -y
# Creates package.json with default values
```

Generated output:
```json
{
  "name": "my-project",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": [],
  "author": "",
  "license": "ISC"
}
```

## Anatomy of package.json

```json
{
  "name": "my-nodejs-app",
  "version": "1.0.0",
  "description": "A practical Node.js application",
  "type": "module",
  "main": "src/index.js",
  "scripts": {
    "start": "node src/index.js",
    "dev": "node --watch src/index.js",
    "test": "node --test src/**/*.test.js",
    "lint": "eslint src/",
    "build": "echo 'Build step here'"
  },
  "keywords": ["nodejs", "tutorial"],
  "author": "Your Name <your@email.com>",
  "license": "MIT",
  "engines": {
    "node": ">=20.0.0"
  },
  "dependencies": {
    "express": "^4.21.0"
  },
  "devDependencies": {
    "eslint": "^9.0.0"
  }
}
```

### Key Fields Explained

| Field | Purpose | Example |
|-------|---------|---------|
| `name` | Package identifier (lowercase, no spaces) | `"my-app"` |
| `version` | Semantic version (major.minor.patch) | `"1.0.0"` |
| `type` | Module system: `"module"` (ESM) or `"commonjs"` | `"module"` |
| `main` | Entry point when imported | `"src/index.js"` |
| `scripts` | Custom commands run with `npm run <name>` | See below |
| `engines` | Required Node.js version | `">=20.0.0"` |
| `dependencies` | Production packages | `"express": "^4.21.0"` |
| `devDependencies` | Development-only packages | `"eslint": "^9.0.0"` |

## Configuring Scripts

Scripts are custom commands you run with `npm run <name>`:

```json
{
  "scripts": {
    "start": "node src/index.js",
    "dev": "node --watch src/index.js",
    "test": "node --test",
    "test:watch": "node --test --watch",
    "lint": "eslint src/",
    "lint:fix": "eslint src/ --fix",
    "format": "prettier --write src/"
  }
}
```

Run them:
```bash
npm start              # Runs "start" script (no "run" needed)
npm run dev            # Runs "dev" script
npm test               # Runs "test" script (no "run" needed)
npm run lint           # Runs "lint" script
```

## ES Modules vs CommonJS

### ES Modules (Recommended for new projects)

```json
{
  "type": "module"
}
```

```javascript
// src/index.js — ES Module syntax
import { readFileSync } from 'node:fs';
import { createServer } from 'node:http';

export function greet(name) {
  return `Hello, ${name}!`;
}

const server = createServer((req, res) => {
  res.end(greet('World'));
});

server.listen(3000);
```

### CommonJS (Legacy)

```json
{
  "type": "commonjs"
}
// or omit "type" — CommonJS is the default
```

```javascript
// src/index.js — CommonJS syntax
const { readFileSync } = require('fs');
const { createServer } = require('http');

function greet(name) {
  return `Hello, ${name}!`;
}

module.exports = { greet };

const server = createServer((req, res) => {
  res.end(greet('World'));
});

server.listen(3000);
```

## Semantic Versioning

Versions follow `MAJOR.MINOR.PATCH`:

```
1.2.3
│ │ │
│ │ └── PATCH: Bug fixes (backward compatible)
│ └──── MINOR: New features (backward compatible)
└────── MAJOR: Breaking changes
```

### Version Ranges in package.json

```json
{
  "dependencies": {
    "express": "4.21.0",     // Exactly 4.21.0
    "lodash": "~4.17.0",     // >=4.17.0 <4.18.0 (patch only)
    "axios": "^1.6.0",       // >=1.6.0 <2.0.0 (minor + patch)
    "cors": "*",              // Any version (not recommended)
    "helmet": ">=7.0.0"      // 7.0.0 or higher
  }
}
```

| Symbol | Meaning | Range for `^1.2.3` |
|--------|---------|-------------------|
| `^` | Compatible with (minor updates) | `>=1.2.3 <2.0.0` |
| `~` | Approximately (patch only) | `>=1.2.3 <1.3.0` |
| `*` | Any version | Any |
| `>=` | Greater than or equal | `>=1.2.3` |

## Complete Example: Real-World package.json

```json
{
  "name": "task-manager-api",
  "version": "1.2.0",
  "description": "A RESTful task management API built with Node.js",
  "type": "module",
  "main": "src/server.js",
  "scripts": {
    "start": "NODE_ENV=production node src/server.js",
    "dev": "NODE_ENV=development node --watch src/server.js",
    "test": "node --test 'src/**/*.test.js'",
    "test:coverage": "node --test --experimental-test-coverage 'src/**/*.test.js'",
    "lint": "eslint 'src/**/*.js'",
    "lint:fix": "eslint 'src/**/*.js' --fix",
    "format": "prettier --write 'src/**/*.js'",
    "db:migrate": "node scripts/migrate.js",
    "db:seed": "node scripts/seed.js"
  },
  "keywords": ["api", "rest", "task-manager"],
  "author": "Your Name",
  "license": "MIT",
  "engines": {
    "node": ">=20.0.0",
    "npm": ">=10.0.0"
  },
  "dependencies": {
    "express": "^4.21.0",
    "cors": "^2.8.5",
    "helmet": "^8.0.0",
    "dotenv": "^16.4.0"
  },
  "devDependencies": {
    "eslint": "^9.0.0",
    "prettier": "^3.2.0"
  }
}
```

## Common Mistakes

### Mistake 1: Forgetting `"type": "module"`

Without this, `import`/`export` syntax will fail:
```
SyntaxError: Cannot use import statement outside a module
```

**Fix:** Add `"type": "module"` to package.json.

### Mistake 2: Committing node_modules

```bash
# Create .gitignore
echo "node_modules/" > .gitignore
echo "package-lock.json" >> .gitignore  # Optional: some teams commit this
```

### Mistake 3: Using wrong script syntax

```json
{
  "scripts": {
    "start": "node src/index.js",        // Correct
    "bad": "NODE_ENV=production node"    // May not work on Windows
  }
}
```

**Fix:** Use `cross-env` for cross-platform environment variables:
```bash
npm install -D cross-env
```
```json
{
  "scripts": {
    "start": "cross-env NODE_ENV=production node src/index.js"
  }
}
```

## Best Practices Checklist

- [ ] Always use `"type": "module"` for new projects
- [ ] Set `"engines"` to specify required Node.js version
- [ ] Use `^` for dependency versions (allows minor updates)
- [ ] Keep `devDependencies` separate from `dependencies`
- [ ] Create meaningful scripts (`dev`, `test`, `lint`, `build`)
- [ ] Add `.gitignore` to exclude `node_modules/`
- [ ] Use `npm ci` in CI/CD (clean install from lock file)
- [ ] Include `"description"` and `"license"` for documentation

## Next Steps

Now that your project is configured, learn about V8 engine internals in [V8 Engine in Practice](../13-v8-engine-practice/01-compilation-demonstration.md).
