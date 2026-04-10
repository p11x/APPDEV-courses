# Linting and Formatting Guide

## OVERVIEW

Linting and formatting maintain code quality and consistency. This guide covers ESLint, Prettier, and custom rules for Web Components.

## IMPLEMENTATION DETAILS

### ESLint Configuration

```javascript
// .eslintrc.js
module.exports = {
  env: {
    browser: true,
    es2021: true,
    node: true
  },
  extends: 'eslint:recommended',
  parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module'
  },
  rules: {
    'no-unused-vars': 'warn',
    'no-console': 'off'
  }
};
```

### Prettier Configuration

```javascript
// .prettierrc
{
  "semi": true,
  "singleQuote": true,
  "tabWidth": 2,
  "trailingComma": "es5"
}
```

### Custom Rules for Components

```javascript
// Check custom element naming
{
  "rules": {
    "custom-element-naming": "error"
  }
}
```

## NEXT STEPS

Proceed to **12_Tooling/12_4_Development-Server-Configuration**.