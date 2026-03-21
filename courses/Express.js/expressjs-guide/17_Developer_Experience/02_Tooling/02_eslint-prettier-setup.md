# ESLint and Prettier Setup

## 📌 What You'll Learn

- ESLint for code quality
- Prettier for code formatting
- Configuring them to work together

## 🧠 Concept Explained (Plain English)

ESLint and Prettier are essential tools for maintaining code quality in your Express projects. ESLint analyzes your code for potential errors and enforces coding patterns, while Prettier handles code formatting to ensure consistent style across your codebase.

ESLint (the L stands for linting - the process of analyzing code for errors) uses rules you define to catch problems like unused variables, incorrect imports, or dangerous code patterns. It can automatically fix many issues.

Prettier is a formatter that takes your code and reformats it to follow consistent rules - things like indentation, line length, and quote styles. It makes decisions so you don't have to debate formatting in code reviews.

When used together with the right configuration, they complement each other perfectly: ESLint catches bugs and style issues, Prettier handles formatting.

## 💻 Code Example

```js
// Installation
// npm install -D eslint prettier eslint-config-prettier eslint-plugin-prettier
// npx eslint --init

// .eslintrc.cjs - ESLint Configuration
module.exports = {
  root: true,
  env: {
    node: true,
    es2022: true,
  },
  extends: [
    'eslint:recommended',
    'plugin:@typescript-eslint/recommended',
    'plugin:prettier/recommended',
  ],
  parser: '@typescript-eslint/parser',
  parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
  },
  plugins: ['@typescript-eslint'],
  rules: {
    // Custom rules
    'no-console': 'warn',  // Warn instead of error for console.log
    'no-unused-vars': 'off',  // Use TypeScript version instead
    '@typescript-eslint/no-unused-vars': ['error', {
      argsIgnorePattern: '^_',
      varsIgnorePattern: '^_',
    }],
    '@typescript-eslint/explicit-function-return-type': 'off',
    '@typescript-eslint/no-explicit-any': 'warn',
    'prettier/prettier': 'error',  // Make Prettier rules ESLint errors
  },
  ignorePatterns: ['dist', 'node_modules', 'coverage'],
};

// .prettierrc - Prettier Configuration
{
  "semi": true,
  "singleQuote": true,
  "tabWidth": 2,
  "trailingComma": "es5",
  "printWidth": 100,
  "bracketSpacing": true,
  "arrowParens": "avoid",
  "endOfLine": "lf"
}

// .eslintignore
node_modules
dist
coverage
*.config.js
*.config.cjs

// package.json scripts
{
  "scripts": {
    "lint": "eslint src --ext .ts,.js",
    "lint:fix": "eslint src --ext .ts,.js --fix",
    "format": "prettier --write \"src/**/*.{ts,js,json}\"",
    "format:check": "prettier --check \"src/**/*.{ts,js,json}\""
  }
}
```

## Integration: ESLint + Prettier

```js
// With this setup:
// 1. ESLint runs with Prettier as an ESLint plugin
// 2. Prettier formatting rules become ESLint rules
// 3. Running eslint --fix also runs prettier

// VS Code Settings (.vscode/settings.json)
{
  "editor.formatOnSave": true,
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": "explicit"
  },
  "eslint.validate": [
    "javascript",
    "typescript"
  ]
}
```

## ⚠️ Common Mistakes

1. **Conflicts between ESLint and Prettier**: Make sure to extend `plugin:prettier/recommended` in ESLint config
2. **Not ignoring build files**: Add dist, node_modules to .eslintignore to avoid linting generated code
3. **Running lint before build**: Some ESLint rules require compiled code - run lint after build

## ✅ Quick Recap

- ESLint catches bugs and enforces code patterns
- Prettier handles consistent formatting
- Use eslint-config-prettier to avoid conflicts
- Configure VS Code to run both on save
- Add lint and format scripts to package.json

## 🔗 What's Next

Learn about Husky and lint-staged for pre-commit validation.
