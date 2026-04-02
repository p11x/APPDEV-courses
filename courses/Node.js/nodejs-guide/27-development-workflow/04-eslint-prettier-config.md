# ESLint & Prettier Configuration

## What You'll Learn

- Setting up ESLint for Node.js
- Configuring Prettier
- Integrating both tools

## ESLint Setup

```bash
npm install -D eslint @eslint/js
```

```js
// eslint.config.js
import js from '@eslint/js';

export default [
  js.configs.recommended,
  {
    languageOptions: {
      ecmaVersion: 2024,
      sourceType: 'module',
      globals: {
        process: 'readonly',
        console: 'readonly',
        setTimeout: 'readonly',
        setInterval: 'readonly',
      },
    },
    rules: {
      'no-unused-vars': ['error', { argsIgnorePattern: '^_' }],
      'no-console': 'off',
      'prefer-const': 'error',
      'no-var': 'error',
    },
  },
];
```

## Prettier Setup

```bash
npm install -D prettier
```

```json
// .prettierrc
{
  "semi": true,
  "singleQuote": true,
  "trailingComma": "all",
  "printWidth": 100,
  "tabWidth": 2
}
```

```json
// .prettierignore
node_modules/
dist/
coverage/
```

## Scripts

```json
{
  "scripts": {
    "lint": "eslint .",
    "lint:fix": "eslint . --fix",
    "format": "prettier --write .",
    "format:check": "prettier --check ."
  }
}
```

## Next Steps

For code quality tools, continue to [Chapter 28: Code Quality](../../28-code-quality/01-eslint-configuration.md).
