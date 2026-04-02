# ESLint Configuration for Node.js Projects

## What You'll Learn

- Installing and configuring ESLint
- Setting up rules for Node.js development
- Integrating ESLint with VS Code
- Customizing rules for your project

## ESLint Installation

### Basic Installation

```bash
# Install ESLint
npm install --save-dev eslint

# Or with specific version
npm install --save-dev eslint@8
```

### Initialize ESLint Configuration

```bash
# Interactive setup
npm init @eslint/config

# Or create manually
touch .eslintrc.json
```

## ESLint Configuration

### Basic Configuration

```json
{
    "env": {
        "node": true,
        "es2021": true,
        "jest": true
    },
    "extends": [
        "eslint:recommended"
    ],
    "parserOptions": {
        "ecmaVersion": "latest",
        "sourceType": "module"
    },
    "rules": {
        "no-console": "warn",
        "no-unused-vars": "error",
        "no-undef": "error",
        "semi": ["error", "always"],
        "quotes": ["error", "single"]
    }
}
```

### Advanced Configuration with TypeScript

```json
{
    "env": {
        "node": true,
        "es2021": true,
        "jest": true
    },
    "extends": [
        "eslint:recommended",
        "plugin:@typescript-eslint/recommended"
    ],
    "parser": "@typescript-eslint/parser",
    "parserOptions": {
        "ecmaVersion": "latest",
        "sourceType": "module",
        "project": "./tsconfig.json"
    },
    "plugins": [
        "@typescript-eslint"
    ],
    "rules": {
        "@typescript-eslint/explicit-function-return-type": "off",
        "@typescript-eslint/no-explicit-any": "warn",
        "@typescript-eslint/no-unused-vars": "error"
    }
}
```

## Common ESLint Rules

### Error Prevention

```javascript
// .eslintrc.json rules section
{
    "rules": {
        // Possible errors
        "no-console": "warn",
        "no-debugger": "error",
        "no-alert": "error",
        
        // Best practices
        "eqeqeq": "error",
        "no-eval": "error",
        "no-implied-eval": "error",
        "no-new-wrappers": "error",
        "no-throw-literal": "error",
        "no-return-await": "error",
        
        // Variables
        "no-unused-vars": ["error", { "argsIgnorePattern": "^_" }],
        "no-undef": "error",
        "no-use-before-define": "error"
    }
}
```

### Style Rules

```javascript
{
    "rules": {
        // Semicolons
        "semi": ["error", "always"],
        
        // Quotes
        "quotes": ["error", "single", { "avoidEscape": true }],
        
        // Indentation
        "indent": ["error", 2],
        
        // Trailing commas
        "comma-dangle": ["error", "never"],
        
        // Arrow functions
        "arrow-parens": ["error", "as-needed"],
        
        // Object curly spacing
        "object-curly-spacing": ["error", "always"],
        
        // Array bracket spacing
        "array-bracket-spacing": ["error", "never"]
    }
}
```

## ESLint for Node.js Specific Rules

### Async/Await Rules

```javascript
{
    "rules": {
        // Require await in async functions
        "require-await": "error",
        
        // No await in loop
        "no-await-in-loop": "warn",
        
        // Return await
        "no-return-await": "error",
        
        // Async function syntax
        "func-style": ["error", "expression"]
    }
}
```

### Import/Export Rules

```javascript
{
    "rules": {
        // Import order
        "import/order": ["error", {
            "groups": [
                "builtin",
                "external",
                "internal",
                "parent",
                "sibling",
                "index"
            ],
            "newlines-between": "always"
        }],
        
        // No duplicate imports
        "import/no-duplicates": "error",
        
        // No unused imports
        "import/no-unused-modules": "error"
    }
}
```

## VS Code Integration

### VS Code Settings

```json
// .vscode/settings.json
{
    "eslint.validate": [
        "javascript",
        "javascriptreact",
        "typescript",
        "typescriptreact"
    ],
    "eslint.alwaysShowStatus": true,
    "eslint.run": "onType",
    "editor.codeActionsOnSave": {
        "source.fixAll.eslint": true
    }
}
```

### ESLint Extension

Install the ESLint extension from VS Code Marketplace for real-time linting.

## npm Scripts Integration

### package.json Scripts

```json
{
    "scripts": {
        "lint": "eslint .",
        "lint:fix": "eslint . --fix",
        "lint:check": "eslint . --max-warnings 0"
    }
}
```

### Pre-commit Hook

```json
// package.json with husky and lint-staged
{
    "husky": {
        "hooks": {
            "pre-commit": "lint-staged"
        }
    },
    "lint-staged": {
        "*.js": [
            "eslint --fix",
            "git add"
        ]
    }
}
```

## Custom Rule Configuration

### Project-Specific Rules

```javascript
// .eslintrc.js
module.exports = {
    env: {
        node: true,
        es2021: true
    },
    extends: ['eslint:recommended'],
    rules: {
        // Custom rules for this project
        'no-console': process.env.NODE_ENV === 'production' ? 'error' : 'warn',
        'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'warn',
        
        // Allow unused vars starting with _
        'no-unused-vars': ['error', { 'argsIgnorePattern': '^_' }],
        
        // Enforce consistent return
        'consistent-return': 'error',
        
        // No var
        'no-var': 'error',
        
        // Prefer const
        'prefer-const': 'error'
    }
};
```

## Ignoring Files

### .eslintignore

```
# Dependencies
node_modules/

# Build output
dist/
build/

# Coverage
coverage/

# Logs
*.log

# OS files
.DS_Store
Thumbs.db

# IDE
.vscode/
.idea/

# Config files
.env
.env.local
```

## Troubleshooting Common Issues

### ESLint Not Running

```bash
# Problem: ESLint not detecting files
# Solution: Check configuration

# Check if ESLint is installed
npx eslint --version

# Check configuration
npx eslint --print-config src/index.js

# Run with debug
DEBUG=eslint:* npx eslint src/
```

### Too Many Errors

```bash
# Problem: Too many errors on first run
# Solution: Start with warnings

# Temporarily downgrade rules to warnings
"rules": {
    "no-console": "warn",
    "no-unused-vars": "warn"
}

# Or use --fix to auto-fix
npx eslint . --fix
```

### Configuration Conflicts

```bash
# Problem: Conflicting rules
# Solution: Use extends properly

# Check for multiple .eslintrc files
find . -name ".eslintrc*"

# Use root: true to stop searching
{
    "root": true,
    "extends": ["eslint:recommended"]
}
```

## Best Practices Checklist

- [ ] Install ESLint as dev dependency
- [ ] Create .eslintrc configuration file
- [ ] Set up VS Code integration
- [ ] Add lint scripts to package.json
- [ ] Configure pre-commit hooks
- [ ] Set up .eslintignore file
- [ ] Start with recommended rules
- [ ] Customize rules for project needs
- [ ] Fix errors incrementally
- [ ] Document custom rules

## Performance Optimization Tips

- Use `--cache` flag for faster subsequent runs
- Ignore unnecessary files in .eslintignore
- Use `--max-warnings` to fail on warnings
- Run lint only on changed files in CI
- Use eslint-plugin-import for import optimization
- Configure parser for better performance

## Cross-References

- See [Prettier Setup](./02-prettier-setup.md) for code formatting
- See [Git Workflow](../08-git-workflow/) for pre-commit hooks
- See [Testing Environment](../06-testing-environment/) for test configuration
- See [Development Tools](../12-dev-tools-integration/) for IDE setup

## Next Steps

Now that ESLint is configured, let's set up Prettier. Continue to [Prettier Formatting Setup](./02-prettier-setup.md).