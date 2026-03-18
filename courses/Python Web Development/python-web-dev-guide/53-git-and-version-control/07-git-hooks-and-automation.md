# Git Hooks and Automation

## What You'll Learn

- What Git hooks are
- Common pre-commit and post-commit hooks
- Setting up hooks for your project
- Using Husky for easier hook management
- Automating code quality checks

## Prerequisites

- Completed `06-resolving-merge-conflicts.md`
- Comfortable with command line

## What Are Git Hooks?

Git hooks are scripts that run automatically when certain Git events occur:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GIT HOOKS OVERVIEW                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  CLIENT-SIDE HOOKS (on your machine):                                       │
│  • pre-commit     — Before commit is created                                │
│  • prepare-commit-msg — Before commit message editor opens                 │
│  • commit-msg    — After commit message is entered                         │
│  • post-commit   — After commit is complete                                │
│  • pre-push      — Before push goes out                                    │
│  • pre-rebase    — Before rebase happens                                   │
│                                                                             │
│  SERVER-SIDE HOOKS (on remote):                                             │
│  • pre-receive   — Before accepting push                                    │
│  • update        — Per-branch check before accepting                       │
│  • post-receive  — After push is fully received                            │
│                                                                             │
│  Hooks live in: .git/hooks/                                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Creating Your First Hook

### A Simple Pre-commit Hook

Create `.git/hooks/pre-commit`:

```bash
#!/bin/sh

# Run tests before allowing commit
echo "Running tests before commit..."
python -m pytest

# If tests fail, abort commit
if [ $? -ne 0 ]; then
    echo "Tests failed! Commit aborted."
    exit 1
fi

echo "Tests passed! Proceeding with commit."
exit 0
```

🔍 **What this does:**
- Runs pytest before each commit
- If tests fail (`$? -ne 0`), exits with error code 1, aborting commit
- If tests pass, exits with 0, allowing commit

Make it executable:

```bash
chmod +x .git/hooks/pre-commit
```

## Common Hook Examples

### Pre-commit: Check Code Formatting

```bash
#!/bin/sh
# .git/hooks/pre-commit

# Check Python files are formatted with Black
git diff --cached --name-only --diff-filter=ACM | \
    grep '\.py$' | xargs black --check

if [ $? -ne 0 ]; then
    echo "Run 'black .' to format your code"
    exit 1
fi

exit 0
```

### Pre-commit: Lint Check

```bash
#!/bin/sh
# .git/hooks/pre-commit

# Run flake8 on staged Python files
git diff --cached --name-only --diff-filter=ACM | \
    grep '\.py$' | xargs flake8

if [ $? -ne 0 ]; then
    echo "Lint errors found! Fix before committing."
    exit 1
fi

exit 0
```

### Pre-push: Run Full Test Suite

```bash
#!/bin/sh
# .git/hooks/pre-push

echo "Running full test suite..."
python -m pytest --cov

if [ $? -ne 0 ]; then
    echo "Tests failed! Push aborted."
    exit 1
fi

exit 0
```

## Using Husky (Easier Hook Management)

Husky makes Git hooks easier to manage:

```bash
# Install Husky
npm install husky --save-dev

# Initialize Husky
npx husky install

# Add pre-commit hook
npx husky add .husky/pre-commit "npm test"

# Make sure hooks run on commit
npm pkg set scripts.prepare="husky install"
```

### Husky with Python

```bash
# Create pre-commit hook with Husky
npx husky add .husky/pre-commit "black --check . && flake8 . && pytest"
```

## Project-Level Hooks

Instead of local hooks, create project hooks:

```bash
# Create a hooks folder
mkdir -p .githooks

# Configure Git to use it
git config core.hooksPath .githooks

# Create hooks
touch .githooks/pre-commit
chmod +x .githooks/pre-commit
```

Add to `.git/config`:

```ini
[core]
	hooksPath = .githooks
```

Or globally:

```bash
git config --global core.hooksPath ~/.git-hooks
```

## Common Automation Workflows

### 1. Auto-format on Commit

```bash
#!/bin/sh
# .githooks/pre-commit

# Automatically format Python files with Black
git diff --cached --name-only --diff-filter=ACM | \
    grep '\.py$' | xargs black

# Stage the formatted files
git diff --cached --name-only --diff-filter=ACM | \
    grep '\.py$' | xargs git add

exit 0
```

### 2. Enforce Branch Naming

```bash
#!/bin/sh
# .githooks/pre-push

branch=$(git symbolic-ref --short HEAD)

# Check branch name format
if ! echo "$branch" | grep -qE '^(feature|fix|hotfix|release)/'; then
    echo "Branch name must match: type/description"
    echo "Examples: feature/login, fix/bug-123"
    exit 1
fi

exit 0
```

### 3. Require Issue Reference

```bash
#!/bin/sh
# .githooks/commit-msg

commit_msg=$(cat "$1")

# Must reference an issue
if ! echo "$commit_msg" | grep -qE '(#[0-9]+|closes|fixes)'; then
    echo "Commit message must reference an issue (e.g., #123)"
    exit 1
fi

exit 0
```

## Hooks for Code Quality

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    RECOMMENDED HOOKS                                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  PRE-COMMITS:                                                               │
│  ✅ Code formatting (Black)                                                 │
│  ✅ Linting (Flake8)                                                        │
│  ✅ Type checking (mypy)                                                   │
│  ✅ Security scanning (bandit)                                              │
│                                                                             │
│  PRE-PUSH:                                                                  │
│  ✅ Full test suite                                                         │
│  ✅ Coverage threshold                                                      │
│  ✅ Integration tests                                                       │
│                                                                             │
│  COMMIT-MSG:                                                                │
│  ✅ Enforce conventional commits                                           │
│  ✅ Require issue reference                                                 │
│                                                                             │
│  PRE-RECEIVE (server):                                                      │
│  ✅ Validate commits                                                        │
│  ✅ Run CI checks                                                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Commitzen: Conventional Commits

Use Commitzen for consistent commit messages:

```bash
# Install
npm install -g commitizen

# Use
git cz

# Or conventional-changelog-cli
npm install -g conventional-changelog-cli
conventional-changelog -p angular -i CHANGELOG.md -s
```

Commit message format:

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

Examples:

```
feat(auth): add login endpoint
fix: resolve redirect bug on logout
docs: update API documentation
style: format code with Black
refactor: simplify authentication logic
test: add tests for user model
```

## Summary

- Git hooks are scripts that run on Git events
- Client hooks: pre-commit, commit-msg, pre-push, etc.
- Use hooks to enforce code quality automatically
- Husky makes hook management easier
- Common uses: formatting, linting, testing

## Next Steps

→ Continue to `08-using-git-with-github.md` to learn GitHub-specific workflows.
