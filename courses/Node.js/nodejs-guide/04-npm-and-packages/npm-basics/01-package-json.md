# Understanding package.json

## What You'll Learn

- What package.json is and why it's important
- The different fields in package.json
- How to create and modify package.json
- Understanding dependencies and devDependencies

## What is package.json?

**package.json** is a JSON file that sits at the root of your Node.js project. It contains:
- Project metadata (name, version, description)
- Dependencies (packages your project needs)
- Scripts (automated tasks)
- Other configuration

## Creating package.json

### Automatic Creation

The easiest way to create package.json is using `npm init`:

```bash
npm init
```

This command asks you questions about your project:
- package name
- version (default: 1.0.0)
- description
- entry point (default: index.js)
- test command
- git repository
- keywords
- author
- license

### Create with Defaults

For quick setup:

```bash
npm init -y
```

This creates a package.json with all default values.

## Package.json Fields Explained

Here's a typical package.json:

```json
{
  "name": "my-awesome-project",
  "version": "1.0.0",
  "description": "A cool project that does stuff",
  "main": "src/index.js",
  "type": "module",
  "scripts": {
    "start": "node src/index.js",
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": ["nodejs", "tutorial"],
  "author": "Your Name",
  "license": "MIT",
  "dependencies": {
    "express": "^4.18.0"
  },
  "devDependencies": {
    "nodemon": "^3.0.0"
  }
}
```

### Key Fields

| Field | Description |
|-------|-------------|
| `name` | Your project name (lowercase, no spaces) |
| `version` | Semantic version (major.minor.patch) |
| `main` | Entry point file |
| `type` | Module type: "module" or "commonjs" |
| `scripts` | Automation commands |
| `dependencies` | Packages needed to run |
| `devDependencies` | Packages only for development |

## Dependencies vs DevDependencies

### Dependencies

Packages needed for your app to run in production:

```json
{
  "dependencies": {
    "express": "^4.18.0",
    "pg": "^8.11.0",
    "dotenv": "^16.0.0"
  }
}
```

### DevDependencies

Packages only needed during development:

```json
{
  "devDependencies": {
    "nodemon": "^3.0.0",
    "jest": "^29.0.0"
  }
}
```

DevDependencies are not installed in production.

## Version Numbers

Node.js uses **semantic versioning** (semver):

```
major.minor.patch
^4.18.0
|  |   |
|  |   +-- Patch: Bug fixes
|  +------ Minor: New features (backward compatible)
+--------- Major: Breaking changes
```

### Version Ranges

| Symbol | Meaning |
|--------|---------|
| `^4.18.0` | 4.x.x (compatible) |
| `~4.18.0` | 4.18.x (patch only) |
| `4.18.0` | Exactly 4.18.0 |
| `>=4.18.0` | 4.18.0 or higher |
| `*` | Latest version |

## Code Example: Package.json in Action

### Setting Up a Project

```bash
# Create project folder
mkdir my-project
cd my-project

# Initialize npm (creates package.json)
npm init -y

# Add a dependency
npm install express

# Add a dev dependency
npm install --save-dev nodemon
```

### Resulting package.json

```json
{
  "name": "my-project",
  "version": "1.0.0",
  "main": "index.js",
  "type": "module",
  "scripts": {
    "start": "node index.js",
    "dev": "nodemon index.js",
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "dependencies": {
    "express": "^4.18.2"
  },
  "devDependencies": {
    "nodemon": "^3.0.1"
  }
}
```

## Adding Dependencies

### Install and Save

```bash
# Save to dependencies (default for most packages)
npm install express

# Save to dependencies explicitly
npm install --save express

# Save to devDependencies
npm install --save-dev nodemon
```

### Install from Package.json

```bash
# Install all dependencies from package.json
npm install
```

This reads the dependencies and devDependencies and installs them.

## Type Field

The `type` field determines which module system to use:

```json
{
  "type": "module"
}
```

- `"type": "module"` - Uses ES Modules (import/export)
- `"type": "commonjs"` - Uses CommonJS (require/module.exports)

If not specified, defaults to CommonJS.

## Common Mistakes

### Mistake 1: Not Committing package.json

Always commit package.json to version control. It's essential for reproducibility.

### Mistake 2: Wrong Dependency Type

```javascript
// WRONG - Express is production code, should be in dependencies
// But nodemon is dev-only

// CORRECT
npm install express      // → dependencies
npm install -D nodemon   // → devDependencies
```

### Mistake 3: Using Wrong Version Ranges

```javascript
// Using exact version for production
"express": "4.18.2"  // Only this exact version

// Using ^ for compatible updates
"express": "^4.18.2"  // 4.x.x but not 5.x.x
```

## Try It Yourself

### Exercise 1: Create package.json
Create a new project and initialize package.json with npm init.

### Exercise 2: Add Dependencies
Install express as a dependency and nodemon as a devDependency.

### Exercise 3: Configure Scripts
Add custom scripts to your package.json.

## Next Steps

Now you understand package.json. Let's learn about installing packages. Continue to [Installing Packages](./02-install-packages.md).
