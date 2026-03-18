# ESLint and Prettier Setup

## Overview

Code quality tools are essential for maintaining consistent code style and catching common errors before they become problems. ESLint analyzes your JavaScript code for problematic patterns, while Prettier formats your code to ensure consistent styling. Together, they form the foundation of a professional development environment. This guide will walk you through setting up these tools for your React project.

## Prerequisites

- Node.js installed on your machine
- A React project created with Vite or CRA
- Basic understanding of JavaScript
- Familiarity with package.json and npm

## Core Concepts

### What is ESLint?

ESLint is a static code analysis tool that identifies patterns in JavaScript code that are inconsistent or prone to errors. It helps you find and fix problems before running your code, saving debugging time and improving code quality.

```javascript
// File: src/eslint-example.js

// ESLint catches issues like:
// - Unused variables
// - Potential runtime errors
// - Code that doesn't follow established patterns
// - Inconsistent coding style

// Example issues ESLint would catch:
let x = 5; // Variable declared but never used
const arr = [1, 2, 3];
arr.forEach(() => {}); // Empty function - probably a bug
if (true) { // Condition is always true
  console.log('This always runs');
}
```

### What is Prettier?

Prettier is an opinionated code formatter that enforces consistent style across your entire codebase. It handles formatting so you don't have to worry about tabs vs spaces, semicolons, line lengths, and other stylistic choices.

```javascript
// File: src/prettier-example.js

// Before Prettier (inconsistent formatting):
function  greet(name)  {
    return   `Hello, ${    name    }!`
}

// After Prettier (consistent formatting):
function greet(name) {
  return `Hello, ${name}!`;
}

// Prettier handles:
// - Indentation (tabs vs spaces)
// - Semicolons
// - Quote style (single vs double)
// - Line length
// - Trailing commas
// - And much more
```

### Setting Up ESLint

```bash
# File: src/eslint-setup.sh

# If using Vite, ESLint is already included:
npm create vite@latest my-app -- --template react

# Navigate to your project
cd my-app

# Install ESLint and related packages
npm install -D eslint eslint-config-react-app eslint-plugin-react eslint-plugin-react-hooks eslint-plugin-jsx-a11y

# Create ESLint configuration file
touch .eslintrc.cjs
```

```javascript
// File: .eslintrc.cjs

module.exports = {
  root: true,
  env: {
    browser: true,
    es2020: true,
    node: true, // Enable Node.js globals
  },
  extends: [
    'eslint:recommended',
    'plugin:react/recommended',
    'plugin:react/jsx-runtime',
    'plugin:react-hooks/recommended',
    'plugin:jsx-a11y/recommended',
  ],
  ignorePatterns: ['dist', '.eslintrc.cjs'],
  parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
    ecmaFeatures: {
      jsx: true,
    },
  },
  settings: {
    react: {
      version: '18.2', // Tell ESLint which React version you're using
    },
  },
  plugins: ['react-refresh'], // Only include what you need
  rules: {
    // Customize rules for your project
    
    // Disable some React rules that might be too strict
    'react/prop-types': 'off', // Disable if using TypeScript
    
    // Enable helpful rules
    'no-unused-vars': 'warn', // Warn about unused variables
    'react-hooks/exhaustive-deps': 'warn', // Warn about missing dependencies
    
    // Custom rules
    'react/jsx-uses-react': 'off', // Not needed with new JSX transform
    'react-refresh/only-export-components': [
      'warn',
      { allowConstantExport: true },
    ],
  },
};
```

### Setting Up Prettier

```bash
# File: src/prettier-setup.sh

# Install Prettier as a dev dependency
npm install -D prettier

# Create Prettier config file
touch .prettierrc

# Optional: Create .prettierignore file
touch .prettierignore
```

```javascript
// File: .prettierrc

{
  "semi": true,              // Add semicolons at end of statements
  "singleQuote": true,       // Use single quotes instead of double
  "tabWidth": 2,             // Use 2 spaces for indentation
  "trailingComma": "es5",    // Trailing commas where valid in ES5
  "printWidth": 80,          // Maximum line length
  "bracketSpacing": true,   // Add spaces between { and }
  "arrowParens": "always",   // Always include parentheses around arrow function args
  "endOfLine": "lf"          // Use linefeed (\n) for line endings
}
```

```text
# File: .prettierignore

# Dependencies
node_modules/

# Build outputs
dist/
build/

# Cache
.cache/

# Misc
*.log
.env
.env.local
.env.*.local
```

### Configuring VS Code for ESLint and Prettier

```json
// File: .vscode/extensions.json (Recommended Extensions)

{
  "recommendations": [
    "dbaeumer.vscode-eslint",
    "esbenp.prettier-vscode"
  ]
}
```

```json
// File: .vscode/settings.json (Workspace Settings)

{
  // Default formatter is Prettier
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  
  // Format on save
  "editor.formatOnSave": true,
  
  // ESLint configuration
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": "explicit"
  },
  
  // Run ESLint on save
  "eslint.validate": [
    "javascript",
    "javascriptreact",
    "typescript",
    "typescriptreact"
  ],
  
  // Prettier configuration
  "[javascript]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  },
  "[javascriptreact]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  },
  "[typescript]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  },
  "[typescriptreact]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  },
  
  // Don't format these files
  "[json]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  },
  "[markdown]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  }
}
```

### Making ESLint and Prettier Work Together

The key to making ESLint and Prettier work together is understanding their roles:

- **Prettier**: Handles all formatting (indentation, quotes, semicolons)
- **ESLint**: Handles code quality (unused variables, potential bugs)

To prevent conflicts, use `eslint-config-prettier`:

```bash
# Install eslint-config-prettier
npm install -D eslint-config-prettier
```

```javascript
// File: .eslintrc.cjs (Updated)

module.exports = {
  root: true,
  env: {
    browser: true,
    es2020: true,
    node: true,
  },
  extends: [
    'eslint:recommended',
    'plugin:react/recommended',
    'plugin:react/jsx-runtime',
    'plugin:react-hooks/recommended',
    'plugin:jsx-a11y/recommended',
    'prettier', // Add this LAST to disable conflicting ESLint rules
  ],
  ignorePatterns: ['dist', '.eslintrc.cjs'],
  parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
    ecmaFeatures: {
      jsx: true,
    },
  },
  settings: {
    react: {
      version: '18.2',
    },
  },
  plugins: ['react-refresh'],
  rules: {
    'react/prop-types': 'off',
    'no-unused-vars': 'warn',
    'react-hooks/exhaustive-deps': 'warn',
    'react/jsx-uses-react': 'off',
    'react-refresh/only-export-components': [
      'warn',
      { allowConstantExport: true },
    ],
  },
};
```

### Adding Scripts to package.json

```json
// File: package.json

{
  "name": "my-react-app",
  "version": "1.0.0",
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview",
    "lint": "eslint . --ext js,jsx --report-unused-disable-directives --max-warnings 0",
    "lint:fix": "eslint . --ext js,jsx --fix",
    "format": "prettier --write .",
    "format:check": "prettier --check ."
  },
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0"
  },
  "devDependencies": {
    "@types/react": "^18.2.0",
    "@types/react-dom": "^18.2.0",
    "@vitejs/plugin-react": "^4.0.0",
    "eslint": "^8.45.0",
    "eslint-config-prettier": "^8.8.0",
    "eslint-plugin-react": "^7.32.2",
    "eslint-plugin-react-hooks": "^4.6.0",
    "eslint-plugin-react-refresh": "^0.4.3",
    "prettier": "^3.0.0",
    "vite": "^4.4.0"
  }
}
```

## Common Mistakes

### Mistake 1: Not Running Prettier Before ESLint

```bash
# ❌ WRONG - Running ESLint first can cause conflicts
npm run lint  # This might fix formatting, then Prettier will fix it again

# ✅ CORRECT - Run in this order:
npm run format  # Prettier first - handles all formatting
npm run lint    # ESLint second - handles code quality
```

### Mistake 2: Too Many ESLint Rules

```javascript
// ❌ WRONG - Too many custom rules can be frustrating
module.exports = {
  rules: {
    'react/prop-types': 'error',
    'react-hooks/exhaustive-deps': 'error',
    'no-console': 'error',
    'no-debugger': 'error',
    'prefer-const': 'error',
    'no-var': 'error',
    'arrow-spacing': 'error',
    'comma-dangle': 'error',
    // Too many rules makes development painful!
  },
};

// ✅ CORRECT - Start with defaults, add rules gradually as needed
module.exports = {
  extends: ['eslint:recommended', 'prettier'],
  rules: {
    // Only add custom rules for specific project needs
    'no-unused-vars': 'warn',
  },
};
```

### Mistake 3: Not Ignoring Generated Files

```javascript
// ❌ WRONG - Linting build output wastes time
// Running eslint on dist/ folder

// ✅ CORRECT - Add ignore patterns
module.exports = {
  ignorePatterns: [
    'dist',           // Vite build output
    'build',          // CRA build output
    '.eslintrc.cjs',  // Config file
    'node_modules',
    'coverage',
    '*.min.js',
  ],
};
```

### Mistake 4: Not Using Git Hooks

```bash
# ❌ WRONG - Relying on manual linting/formatting

# ✅ CORRECT - Use husky to run checks before commits

# Install husky
npm install -D husky

# Initialize husky
npx husky install

# Add pre-commit hook
npx husky add .husky/pre-commit "npm run lint"
# or
npx husky add .husky/pre-commit "npm run lint && npm run format"
```

```json
// package.json with husky
{
  "scripts": {
    "prepare": "husky install",
    "lint": "eslint .",
    "format": "prettier --write ."
  },
  "devDependencies": {
    "husky": "^8.0.0"
  }
}
```

## Real-World Example

Let's create a complete setup for a React project:

```bash
# File: src/setup-commands.sh

# 1. Create new Vite project
npm create vite@latest my-app -- --template react
cd my-app

# 2. Install all development dependencies
npm install -D \
  eslint \
  prettier \
  eslint-config-prettier \
  eslint-plugin-react \
  eslint-plugin-react-hooks \
  eslint-plugin-jsx-a11y \
  eslint-plugin-react-refresh \
  husky

# 3. Initialize husky
npm run prepare

# 4. Create pre-commit hook
npx husky add .husky/pre-commit "npm run lint"

# 5. Create configuration files
# (see examples above)

# 6. Test it works
npm run lint   # Should show no errors
npm run format # Should format all files
```

Now when you work on your project:

```bash
# Run dev server
npm run dev

# Before committing (husky will run this automatically)
npm run lint    # Check for code issues
npm run format  # Format code

# Fix auto-fixable issues
npm run lint:fix
```

## Key Takeaways

- ESLint analyzes code for errors and enforces coding patterns
- Prettier handles all code formatting for consistent style
- Use `eslint-config-prettier` to prevent conflicts between them
- Configure VS Code to format on save for the best developer experience
- Add npm scripts for easy command-line usage
- Consider adding husky to run linting before commits
- Start with recommended configurations and customize gradually
- Ignore build outputs and node_modules in ESLint config

## What's Next

Now that you have your development environment set up, let's dive deep into JSX - the syntax extension that makes React development intuitive. The next file covers JSX syntax rules in detail.
