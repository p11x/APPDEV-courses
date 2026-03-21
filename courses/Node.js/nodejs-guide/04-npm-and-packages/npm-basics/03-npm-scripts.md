# npm Scripts

## What You'll Learn

- What npm scripts are
- How to define and run scripts
- Pre and post hooks
- Common script patterns

## What are npm Scripts?

npm scripts are commands defined in package.json that automate tasks like:
- Starting your server
- Running tests
- Building your project
- Linting code

## Basic Script Definition

### In package.json

```json
{
  "scripts": {
    "start": "node index.js",
    "test": "jest"
  }
}
```

### Running Scripts

```bash
# Run start script
npm start

# Run test script
npm test

# Run any script
npm run script-name
```

## Common Scripts

### Development Scripts

```json
{
  "scripts": {
    "start": "node src/index.js",
    "dev": "nodemon src/index.js",
    "build": "node build.js"
  }
}
```

### Testing Scripts

```json
{
  "scripts": {
    "test": "jest",
    "test:watch": "jest --watch",
    "test:coverage": "jest --coverage"
  }
}
```

### Linting Scripts

```json
{
  "scripts": {
    "lint": "eslint src/",
    "lint:fix": "eslint src/ --fix"
  }
}
```

## Pre and Post Hooks

npm runs pre-* and post-* scripts automatically:

```json
{
  "scripts": {
    "prebuild": "echo 'Building...'",
    "build": "node build.js",
    "postbuild": "echo 'Build complete!'"
  }
}
```

Running `npm run build` outputs:
```
> npm run prebuild
> echo 'Building...'
Building...

> npm run build
> node build.js

> npm run postbuild
> echo 'Build complete!'
Build complete!
```

## Passing Arguments

### To npm scripts

```bash
# Pass extra arguments after --
npm test -- --coverage

# Pass arguments to nodemon
npm run dev -- --inspect
```

### In package.json

```json
{
  "scripts": {
    "test": "jest",
    "test:ci": "jest --ci --coverage --reporters=default"
  }
}
```

## Code Example: Complete Scripts

```json
{
  "name": "my-project",
  "version": "1.0.0",
  "type": "module",
  "scripts": {
    "start": "node src/index.js",
    "dev": "nodemon src/index.js",
    "test": "node --test src/test.js",
    "lint": "eslint src/",
    "lint:fix": "eslint src/ --fix",
    "build": "node scripts/build.js",
    "build:prod": "NODE_ENV=production node scripts/build.js",
    "clean": "rm -rf dist",
    "clean:all": "rm -rf node_modules dist"
  }
}
```

## Running npm Scripts

### Basic Commands

```bash
# Run start (special name, no 'run' needed)
npm start

# Run test (special name)
npm test

# Run custom script
npm run dev
npm run build

# List all available scripts
npm run
```

### Using npx

```bash
# Run local binaries from node_modules
npx nodemon server.js
npx jest

# Run packages without installing
npx create-react-app my-app
```

## Common Mistakes

### Mistake 1: Using Spaces in Script Names

```javascript
// WRONG - spaces don't work in script names
"scripts": {
  "my script": "echo hello"
}

// CORRECT - use colons or hyphens
"scripts": {
  "my-script": "echo hello",
  "my:script": "echo hello"
}
```

### Mistake 2: Not Using npx

```bash
# WRONG - trying to run local binary directly
./node_modules/.bin/nodemon server.js

# CORRECT - use npx
npx nodemon server.js
```

### Mistake 3: Forgetting Environment Variables

```json
// Set environment in script
"scripts": {
  "start:prod": "NODE_ENV=production node src/index.js"
}
```

On Windows, you might need cross-env:

```bash
npm install --save-dev cross-env
```

```json
{
  "scripts": {
    "start:prod": "cross-env NODE_ENV=production node src/index.js"
  }
}
```

## Best Practices

### 1. Document Your Scripts

Add a test script even if just echoing:

```json
{
  "scripts": {
    "test": "echo \"No tests specified\" && exit 1"
  }
}
```

### 2. Use Descriptive Names

```json
{
  "scripts": {
    "server": "node server.js",
    "client": "npm run dev --prefix client",
    "dev": "concurrently \"npm run server\" \"npm run client\""
  }
}
```

### 3. Keep Scripts Simple

```json
{
  "scripts": {
    "start": "node src/index.js",
    "dev": "nodemon src/index.js"
  }
}
```

Complex logic belongs in separate scripts files.

## Try It Yourself

### Exercise 1: Add Scripts
Add start, dev, and test scripts to your package.json.

### Exercise 2: Create Pre/Post Hooks
Add prebuild and postbuild scripts.

### Exercise 3: Run Scripts
Run each of your scripts to make sure they work.

## Next Steps

Now you understand npm basics. Let's learn about ES Modules. Continue to [Import and Export](./../esm-modules/01-import-export.md).
