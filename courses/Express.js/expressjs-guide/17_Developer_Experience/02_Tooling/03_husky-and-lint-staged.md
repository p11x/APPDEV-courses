# Husky and lint-staged

## 📌 What You'll Learn

- Setting up Husky for git hooks
- lint-staged for staged file validation
- Running tests on pre-push

## 🧠 Concept Explained (Plain English)

Git hooks are scripts that run automatically when you perform git operations like committing or pushing. Husky is a tool that makes it easy to configure these hooks directly from your package.json. lint-staged runs linters on files that are staged (added to git but not yet committed), rather than running on all files every time.

Together, they form a powerful quality gate: when a developer tries to commit code, Husky triggers lint-staged which runs ESLint and Prettier only on the files they've changed. If the code doesn't pass linting, the commit is blocked until issues are fixed.

This prevents bad code from ever entering your repository, keeping your main branch clean and your CI/CD pipeline smoother.

## 💻 Code Example

```js
// Installation
// npm install -D husky lint-staged

// Initialize Husky (creates .husky directory)
// npx husky init

// .husky/pre-commit - Runs before each commit
npm run lint-staged

// .husky/pre-push - Runs before push to remote
npm run test

// package.json - lint-staged configuration
{
  "lint-staged": {
    "*.{ts,js}": [
      "eslint --fix",
      "prettier --write"
    ],
    "*.{json,md,yml,yaml}": [
      "prettier --write"
    ]
  },
  "scripts": {
    "lint-staged": "lint-staged"
  }
}
```

## Conventional Commits Setup

```js
// Install conventional-changelog-conventionalcommits
// npm install -D conventional-changelog-conventionalcommits

// .husky/commit-msg - Validate commit message format
npm exec -- commitlint --edit "$1"

// commitlint.config.cjs
module.exports = {
  extends: ['@commitlint/config-conventional'],
  rules: {
    'type-enum': [
      2,
      'always',
      [
        'feat',     // New feature
        'fix',      // Bug fix
        'docs',     // Documentation changes
        'style',    // Formatting, no code change
        'refactor', // Code restructuring
        'perf',     // Performance improvement
        'test',     // Adding tests
        'build',    // Build system changes
        'ci',       // CI configuration
        'chore',    // Maintenance
      ],
    ],
    'subject-full-stop': [0, 'never'],
    'subject-case': [0, 'never'],
  },
};
```

## Complete Setup Example

```js
// Step 1: Install dependencies
// npm install -D husky lint-staged @commitlint/cli @commitlint/config-conventional

// Step 2: Initialize husky
// npx husky init

// Step 3: Update package.json
{
  "scripts": {
    "prepare": "husky",
    "lint-staged": "lint-staged"
  },
  "lint-staged": {
    "*.ts": [
      "eslint --fix",
      "prettier --write"
    ],
    "*.js": [
      "eslint --fix", 
      "prettier --write"
    ]
  }
}

// Step 4: Create commit-msg hook
// echo 'npx --no -- commitlint --edit "$1"' > .husky/commit-msg
```

## How It Works

1. Developer runs `git commit -m "feat: add user endpoint"`
2. Husky triggers pre-commit hook
3. lint-staged runs ESLint and Prettier on staged files
4. If linting fails, commit is blocked
5. If linting passes, commit-msg validates the message format
6. If message is valid, commit succeeds
7. On `git push`, pre-push runs tests before pushing

## ⚠️ Common Mistakes

1. **Forgetting to run npm install**: Husky needs to be installed for hooks to work
2. **Blocking commits for small issues**: Consider using warn instead of error for non-critical rules
3. **Slow lint-staged**: Only lint the file types that exist in your project

## ✅ Quick Recap

- Husky creates git hooks in .husky directory
- lint-staged runs linters only on staged files
- Configure pre-commit to lint and format code
- Configure pre-push to run tests before pushing
- Use conventional commits for consistent commit messages

## 🔗 What's Next

Learn about hot reloading patterns for fast development restarts.
