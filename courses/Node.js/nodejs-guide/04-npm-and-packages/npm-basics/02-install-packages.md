# Installing npm Packages

## What You'll Learn

- How to install npm packages
- The difference between dependencies and devDependencies
- How to install packages globally
- Understanding node_modules and package-lock.json

## Installing Packages

### Basic Installation

```bash
# Install a package and save to dependencies
npm install express

# Shorthand
npm i express
```

### Save Flags

```bash
# Save to dependencies (default)
npm install express
npm install --save express

# Save to devDependencies
npm install --save-dev nodemon
npm install -D nodemon
```

## Package Locations

When you install packages, they go into the `node_modules` folder.

### Node Modules Structure

```
my-project/
├── node_modules/
│   ├── express/
│   ├── nodemon/
│   └── ... (all installed packages)
├── package.json
├── package-lock.json
└── index.js
```

## Understanding package-lock.json

**package-lock.json** is automatically generated when you install packages. It locks exact versions:

```json
{
  "dependencies": {
    "express": {
      "version": "4.18.2",
      "resolved": "https://registry.npmjs.org/express/-/express-4.18.2.tgz",
      "integrity": "sha512-..."
    }
  }
}
```

### Why It Matters

- Ensures consistent installs across machines
- Records exact versions used
- Don't edit it manually!

## Installing Different Package Types

### Regular Dependencies

```bash
# Production code dependencies
npm install express
npm install pg
npm install dotenv
```

### Development Dependencies

```bash
# Only needed for development
npm install -D nodemon   # Auto-restart server
npm install -D jest      # Testing
npm install -D eslint    # Code linting
```

### Global Installation

```bash
# Install globally (available system-wide)
npm install -g nodemon

# On some systems, you may need sudo
sudo npm install -g nodemon
```

Global packages don't go into your project - they're available anywhere.

## Installing from package.json

```bash
# Install all dependencies
npm install
npm i

# Install only production dependencies
npm install --production
```

This reads package.json and installs everything listed.

## Common npm Commands

### Viewing Packages

```bash
# List installed packages
npm list

# List packages at top level only
npm list --depth=0

# Check for outdated packages
npm outdated
```

### Updating Packages

```bash
# Update a package
npm update express

# Update all packages
npm update
```

### Removing Packages

```bash
# Remove a package (also removes from package.json)
npm uninstall express

# Remove from devDependencies
npm uninstall -D nodemon
```

## Code Example: Package Installation

### Setting Up a New Project

```bash
# 1. Create project
mkdir my-api
cd my-api

# 2. Initialize npm
npm init -y

# 3. Install production dependencies
npm install express pg dotenv

# 4. Install dev dependencies
npm install -D nodemon jest

# 5. Verify installation
npm list --depth=0
```

### Resulting package.json

```json
{
  "name": "my-api",
  "version": "1.0.0",
  "main": "index.js",
  "type": "module",
  "scripts": {
    "start": "node index.js",
    "dev": "nodemon index.js",
    "test": "jest"
  },
  "dependencies": {
    "dotenv": "^16.0.3",
    "express": "^4.18.2",
    "pg": "^8.11.0"
  },
  "devDependencies": {
    "jest": "^29.5.0",
    "nodemon": "^3.0.1"
  }
}
```

## When to Use Global vs Local

### Use Local (Recommended)

- Project-specific dependencies
- Version controlled per project
- Use npx to run local binaries

```bash
# Run local nodemon
npx nodemon server.js
```

### Use Global

- CLI tools you use across projects
- Like: create-react-app, typescript, etc.

```bash
# Install create-react-app globally
npm install -g create-react-app
```

## Common Mistakes

### Mistake 1: Not Using package-lock.json

Always commit package-lock.json to version control. It ensures everyone gets the exact same versions.

### Mistake 2: Installing Global for Project Work

```bash
# WRONG - Don't use -g for project dependencies
npm install -g express

# CORRECT - Install locally in project
npm install express
```

### Mistake 3: Deleting node_modules

Don't delete node_modules and expect npm install to work immediately. Use package-lock.json:

```bash
# Delete node_modules and reinstall
rm -rf node_modules
npm install
```

## Try It Yourself

### Exercise 1: Install Express
Create a new project and install Express as a dependency.

### Exercise 2: Install Dev Dependencies
Install nodemon as a devDependency.

### Exercise 3: Verify Installation
Use npm list to see what packages are installed.

## Next Steps

Now you can install packages. Let's learn about npm scripts to automate tasks. Continue to [npm Scripts](./03-npm-scripts.md).
