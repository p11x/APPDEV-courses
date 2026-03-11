# Git Ignore

## Topic Title
Excluding Files from Version Control with .gitignore

## Concept Explanation

The `.gitignore` file tells Git which files and folders to ignore - files that should not be tracked by version control. This is essential for keeping your repository clean and preventing unnecessary files from being committed.

### What is .gitignore?

`.gitignore` is a plain text file in your project root that contains patterns for files Git should ignore. Each line specifies a pattern for files or directories to exclude.

### Why .gitignore is Important

1. **Excludes generated files**: Build outputs like `dist/`, compiled files
2. **Excludes dependencies**: `node_modules/` can be huge
3. **Protects secrets**: API keys, passwords, environment files
4. **Keeps repository clean**: No IDE files, OS files, or logs
5. **Faster operations**: Fewer files = faster Git operations

### What Should Be Ignored

| Category | Examples |
|----------|----------|
| Build outputs | `dist/`, `build/`, `out/` |
| Dependencies | `node_modules/`, `vendor/` |
| IDE files | `.vscode/`, `.idea/`, `*.swp` |
| OS files | `.DS_Store`, `Thumbs.db` |
| Logs | `*.log`, `logs/` |
| Secrets | `.env`, `*.pem`, `credentials.json` |
| Cache | `.cache/`, `.angular/` |

## Creating .gitignore

### Where to Create

The `.gitignore` file should be in your project root:

```
my-angular-app/
├── .gitignore    ← Here
├── src/
├── angular.json
└── package.json
```

### Basic Syntax

```bash
# Comment line
# Ignore a specific file
secret.txt

# Ignore all files with this extension
*.log

# Ignore a specific directory
node_modules/

# Ignore files in a directory
logs/*.log

# Negate a pattern (don't ignore)
!important.log
```

### Pattern Examples

```bash
# Ignore all .log files
*.log

# Ignore files ending with .log
*.log

# Ignore directory called temp
temp/

# Ignore files in temp directory
temp/*

# Ignore all files in any temp directory
**/temp/

# Ignore specific file
config/secrets.json

# Don't ignore a pattern (negation)
!src/config/default.json
```

## Angular-Specific .gitignore

### Essential Angular Entries

```gitignore
# Angular
## Angular ##
# Angular CLI cache
.angular/

# compiled output
dist/
tmp/
app/**/*.js
app/**/*.js.map

# dependencies
node_modules/

# IDEs
.vscode/
.idea/
*.swp
*.swo

# System
.DS_Store
Thumbs.db

# Environment files
.env
.env.local
.env.*.local

# Logs
logs/
*.log
npm-debug.log*
yarn-debug.log*
yarn-error.log*

# Testing
coverage/

# Misc
.sass-cache/
```

### Using Angular CLI's Default .gitignore

Angular CLI automatically creates a `.gitignore` when you create a new project:

```bash
# Create new Angular project (includes .gitignore)
ng new my-angular-app

# Navigate to project
cd my-angular-app

# View the default .gitignore
cat .gitignore
```

## Checking Ignored Files

### See What Git Ignores

```bash
# Check what files Git is ignoring
git status --ignored

# Check if a specific file would be ignored
git check-ignore -v filename

# Example
git check-ignore -v node_modules/
# Output: node_modules/ matches rule "node_modules/"
```

### Troubleshooting .gitignore

```bash
# If a file is already tracked, ignoring it won't untrack it
# You need to remove it from Git first
git rm --cached filename

# Then add to .gitignore
echo "filename" >> .gitignore
git add .gitignore
git commit -m "Stop tracking filename"
```

## Understanding Gitignore Patterns

### Basic Patterns

```gitignore
# Ignore all files with .log extension
*.log

# Ignore a specific file
error.log

# Ignore a directory
temp/

# Ignore files ending with ~ 
*~

# Ignore files starting with #
#something
```

### Directory Patterns

```gitignore
# Ignore everything in build directory
build/*

# Ignore any directory named test
**/test/

# Ignore build directory at any level
**/build/
```

### Special Patterns

```gitignore
# Negation - don't ignore
!important.log

# Escape special characters
# (Git handles this automatically)

# Empty directory tracking
# To track an empty directory, add a .gitkeep file
```

## Real-World Angular Examples

### Example 1: Environment Files

```bash
# Create environment files
src/
├── environments/
│   ├── environment.ts       ← Track (safe config)
│   ├── environment.prod.ts  ← Track
│   └── .env.local         ← Ignore (secrets)
```

.gitignore:
```gitignore
# Environment secrets
.env
.env.local
.env.*.local
```

### Example 2: Build Output

```bash
# After running ng build
dist/                    ← Contains production files
.angular/               ← Cache from CLI
```

.gitignore:
```gitignore
# Build output
dist/
.angular/
```

### Example 3: Dependencies

```bash
# After running npm install
node_modules/            ← Can be 100k+ files!
```

.gitignore:
```gitignore
# Dependencies
node_modules/
```

## Working with Already Tracked Files

### If You've Already Committed Ignored Files

```bash
# Remove from Git but keep local
git rm --cached filename

# Remove directory from Git
git rm -r --cached directory/

# Commit the removal
git commit -m "Stop tracking sensitive files"
```

### Important Warning

Never commit sensitive data (passwords, API keys, tokens) even to a private repository:

```bash
# ✗ Never do this
git add .env
git commit -m "Add env file"

# ✓ Correct approach
echo ".env" >> .gitignore
git add .gitignore
git commit -m "Add .env to gitignore"
```

## Best Practices

### 1. Create .gitignore First

```bash
# ✓ Good: Create before first commit
touch .gitignore
# Add your patterns
git add .gitignore
git commit -m "Add .gitignore"

# ✗ Bad: Commit files then try to ignore
```

### 2. Use Standard Templates

```bash
# GitHub has templates for popular projects
# https://github.com/github/gitignore
```

### 3. Keep It Minimal

```bash
# ✓ Good: Only what you need
*.log
node_modules/
dist/

# ✗ Bad: Too many rules that aren't needed
```

### 4. Don't Commit Secrets

```bash
# Always ignore environment files with secrets
.env
.env.*
```

### 5. Document Complex Patterns

```bash
# Add comments for team understanding
# Build output
dist/
.angular/

# Dependencies
node_modules/

# IDE
.vscode/
```

## Common Mistakes

### Mistake 1: Forgetting to Add .gitignore

```bash
# ✗ Problem: node_modules gets committed
# This makes repository huge!

# ✓ Solution: Add to .gitignore early
echo "node_modules/" >> .gitignore
```

### Mistake 2: Ignoring Already Tracked Files

```bash
# File is already tracked
# Adding to .gitignore doesn't stop tracking

# Must unstage first
git rm --cached filename
```

### Mistake 3: Wrong Patterns

```bash
# ✗ Wrong: Will ignore parent temp directory
temp

# ✓ Correct: Current directory only
/tmp/

# ✓ Correct: Any temp directory
**/temp/
```

## Exercises for Students

### Exercise 1: Create .gitignore
Create a proper `.gitignore` file for an Angular project with:
- node_modules
- dist
- .angular
- Environment files
- IDE files
- OS files
- Logs

### Exercise 2: Test Ignored Files
1. Create a file called `test.log`
2. Add `*.log` to `.gitignore`
3. Check status - file should be ignored

### Exercise 3: Fix Tracked Files
If you accidentally committed node_modules:
1. Add to .gitignore
2. Remove from Git tracking
3. Commit the change

## Mini Practice Tasks

### Task 1: Create Basic .gitignore
```bash
# 1. Create project folder
mkdir practice-project
cd practice-project
git init

# 2. Create .gitignore
touch .gitignore

# 3. Add common ignores
echo "node_modules/" > .gitignore
echo "dist/" >> .gitignore
echo ".env" >> .gitignore

# 4. View file
cat .gitignore
```

### Task 2: Test Ignoring
```bash
# 1. Create files
echo "test" > test.log
echo "code" > app.js

# 2. Check status
git status

# 3. See what's ignored
git status --ignored

# 4. Add .gitignore to Git
git add .gitignore
git commit -m "Add .gitignore"
```

### Task 3: Angular Project .gitignore
```bash
# 1. Create Angular project
ng new angular-project

# 2. View default .gitignore
cat .gitignore

# 3. Add custom ignores if needed
echo "custom-file.txt" >> .gitignore

# 4. Make initial commit
git add .
git commit -m "Initial commit with .gitignore"
```

## Summary

The `.gitignore` file is essential for every project:

- **Keep it clean**: Ignore generated files, dependencies, and secrets
- **Create early**: Add before first commit
- **Use standard patterns**: Follow common conventions
- **Don't commit secrets**: Always ignore sensitive files

Proper use of `.gitignore` keeps your repository:
- Fast (fewer files)
- Clean (no build artifacts)
- Secure (no secrets)

This completes the Beginner Level section. You're now ready for Intermediate Level Git workflows!

---

**Next Section**: [Intermediate Level - Git Workflows](./../02_Intermediate_Level_Git_Workflows/10_Git_Branches.md)

**Previous Lesson**: [Viewing Git History](./08_Viewing_Git_History.md)
