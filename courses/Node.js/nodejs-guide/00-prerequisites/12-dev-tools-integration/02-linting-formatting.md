# Linting Rules Configuration and Formatting Automation

## What You'll Learn

- Configuring ESLint rules for Node.js
- Setting up Prettier formatting
- Automating code quality checks
- Development workflow optimization

## ESLint Configuration

### Basic ESLint Setup

```bash
# Install ESLint
npm install --save-dev eslint

# Initialize configuration
npm init @eslint/config
```

### ESLint Configuration File

```javascript
// .eslintrc.js
module.exports = {
    env: {
        node: true,
        es2021: true,
        jest: true
    },
    extends: [
        'eslint:recommended'
    ],
    parserOptions: {
        ecmaVersion: 'latest',
        sourceType: 'module'
    },
    rules: {
        'no-console': 'warn',
        'no-unused-vars': 'error',
        'no-undef': 'error',
        'semi': ['error', 'always'],
        'quotes': ['error', 'single']
    }
};
```

### ESLint Ignore File

```
# .eslintignore
node_modules/
dist/
coverage/
*.min.js
```

## ESLint Rules for Node.js

### Error Prevention Rules

```javascript
// .eslintrc.js rules section
{
    "rules": {
        // Prevent errors
        "no-console": "warn",
        "no-debugger": "error",
        "no-alert": "error",
        "no-eval": "error",
        "no-implied-eval": "error",
        "no-new-wrappers": "error",
        "no-throw-literal": "error",
        "no-return-await": "error",
        
        // Variables
        "no-unused-vars": ["error", { "argsIgnorePattern": "^_" }],
        "no-undef": "error",
        "no-use-before-define": "error",
        
        // Best practices
        "eqeqeq": "error",
        "curly": "error",
        "default-case": "error",
        "no-fallthrough": "error"
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

### Async/Await Rules

```javascript
{
    "rules": {
        // Require await in async functions
        "require-await": "error",
        
        // No await in loop
        "no-await-in-loop": "warn",
        
        // Return await
        "no-return-await": "error"
    }
}
```

## Prettier Configuration

### Basic Prettier Setup

```bash
# Install Prettier
npm install --save-dev prettier

# Create configuration
echo {} > .prettierrc
```

### Prettier Configuration File

```json
// .prettierrc
{
    "semi": true,
    "singleQuote": true,
    "tabWidth": 2,
    "useTabs": false,
    "trailingComma": "none",
    "bracketSpacing": true,
    "arrowParens": "avoid",
    "printWidth": 80,
    "endOfLine": "lf"
}
```

### Prettier Ignore File

```
# .prettierignore
node_modules/
dist/
coverage/
package-lock.json
yarn.lock
pnpm-lock.yaml
```

## ESLint + Prettier Integration

### Combined Configuration

```bash
# Install integration packages
npm install --save-dev eslint-config-prettier eslint-plugin-prettier
```

```javascript
// .eslintrc.js
module.exports = {
    env: {
        node: true,
        es2021: true
    },
    extends: [
        'eslint:recommended',
        'plugin:prettier/recommended'
    ],
    plugins: ['prettier'],
    rules: {
        'prettier/prettier': 'error'
    }
};
```

## Automation with npm Scripts

### Package.json Scripts

```json
{
    "scripts": {
        "lint": "eslint .",
        "lint:fix": "eslint . --fix",
        "format": "prettier --write .",
        "format:check": "prettier --check .",
        "check": "npm run lint && npm run format:check",
        "fix": "npm run lint:fix && npm run format"
    }
}
```

## Pre-commit Hooks with Husky

### Installation

```bash
# Install Husky and lint-staged
npm install --save-dev husky lint-staged

# Initialize Husky
npx husky install

# Add to package.json
npm set-script prepare "husky install"
```

### Pre-commit Hook

```bash
# Create pre-commit hook
npx husky add .husky/pre-commit "npx lint-staged"
```

### lint-staged Configuration

```json
// package.json
{
    "lint-staged": {
        "*.js": [
            "eslint --fix",
            "prettier --write"
        ],
        "*.{json,md}": [
            "prettier --write"
        ]
    }
}
```

## IDE Integration

### VS Code Settings

```json
// .vscode/settings.json
{
    "editor.formatOnSave": true,
    "editor.defaultFormatter": "esbenp.prettier-vscode",
    "editor.codeActionsOnSave": {
        "source.fixAll.eslint": true
    },
    
    "eslint.validate": [
        "javascript",
        "javascriptreact",
        "typescript",
        "typescriptreact"
    ],
    
    "[javascript]": {
        "editor.defaultFormatter": "esbenp.prettier-vscode"
    },
    
    "[json]": {
        "editor.defaultFormatter": "esbenp.prettier-vscode"
    }
}
```

## TypeScript Configuration

### TypeScript ESLint

```bash
# Install TypeScript ESLint
npm install --save-dev @typescript-eslint/parser @typescript-eslint/eslint-plugin typescript
```

```javascript
// .eslintrc.js
module.exports = {
    env: {
        node: true,
        es2021: true
    },
    extends: [
        'eslint:recommended',
        'plugin:@typescript-eslint/recommended',
        'prettier'
    ],
    parser: '@typescript-eslint/parser',
    parserOptions: {
        ecmaVersion: 'latest',
        sourceType: 'module',
        project: './tsconfig.json'
    },
    plugins: ['@typescript-eslint'],
    rules: {
        '@typescript-eslint/explicit-function-return-type': 'off',
        '@typescript-eslint/no-explicit-any': 'warn',
        '@typescript-eslint/no-unused-vars': 'error'
    }
};
```

## Custom Rules

### Project-Specific Rules

```javascript
// .eslintrc.js
module.exports = {
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

## CI/CD Integration

### GitHub Actions Workflow

```yaml
# .github/workflows/lint.yml
name: Lint and Format

on: [push, pull_request]

jobs:
  lint:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '20'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Run linter
        run: npm run lint
      
      - name: Check formatting
        run: npm run format:check
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

### Conflicts Between ESLint and Prettier

```bash
# Problem: Rules conflict
# Solution: Use eslint-config-prettier

npm install --save-dev eslint-config-prettier

# In .eslintrc.js
extends: ['eslint:recommended', 'prettier']
```

### Pre-commit Hook Not Running

```bash
# Problem: Husky hook not executing
# Solution: Reinstall Husky

# Remove existing hooks
rm -rf .husky

# Reinstall
npx husky install

# Add hook again
npx husky add .husky/pre-commit "npx lint-staged"
```

## Best Practices Checklist

- [ ] Configure ESLint for Node.js
- [ ] Set up Prettier formatting
- [ ] Integrate ESLint with Prettier
- [ ] Add npm scripts for linting
- [ ] Set up pre-commit hooks
- [ ] Configure IDE integration
- [ ] Add TypeScript support if needed
- [ ] Create custom rules for project
- [ ] Set up CI/CD linting
- [ ] Document linting rules

## Performance Optimization Tips

- Use --cache flag for faster linting
- Ignore unnecessary files in .eslintignore
- Use eslint-plugin-import for import optimization
- Run linting in parallel with tests
- Use lint-staged for staged files only
- Configure IDE to lint on save

## Cross-References

- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for detailed setup
- See [Git Workflow](../08-git-workflow/) for pre-commit hooks
- See [Testing Environment](../06-testing-environment/) for test integration
- See [Development Tools](../12-dev-tools-integration/) for IDE setup

## Next Steps

Now that linting and formatting are configured, you're ready to start Node.js development. Continue to [Chapter 1: Introduction to Node.js](../01-introduction/).