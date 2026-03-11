# Project 1: Version Control for Angular Project

## Project Overview

In this project, students will practice setting up Git version control for an Angular project from scratch. This foundational project teaches the essential Git workflow that every Angular developer needs.

### Learning Objectives

- Initialize Git repository in an Angular project
- Understand Git workflow (add, commit, status)
- Create and manage .gitignore
- Track Angular-specific files properly
- View project history

### Prerequisites

- Node.js installed
- Angular CLI installed
- Git installed and configured

---

## Step 1: Create New Angular Project

### Task 1.1: Set Up Environment

```bash
# Check Git installation
git --version

# Check Node.js
node --version

# Check Angular CLI
ng version
```

### Task 1.2: Create New Angular Project

```bash
# Create new Angular project
ng new angular-todo-app --routing=false --style=css --skip-tests

# Navigate into project
cd angular-todo-app
```

---

## Step 2: Initialize Git Repository

### Task 2.1: Initialize Repository

```bash
# Initialize Git
git init

# Verify initialization
ls -la
# Should see .git folder

# Check status
git status
```

### Task 2.2: Configure Git Identity

```bash
# Set user name
git config user.name "Your Name"

# Set user email
git config user.email "your.email@example.com"

# Verify configuration
git config --list
```

---

## Step 3: Create .gitignore

### Task 3.1: Create .gitignore File

Create a comprehensive .gitignore for Angular:

```bash
# Create .gitignore
touch .gitignore
```

Add the following content:

```gitignore
# See http://help.github.com/ignore-files/ for more about ignoring files.

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

### Task 3.2: Commit .gitignore

```bash
# Stage .gitignore
git add .gitignore

# Commit
git commit -m "Add .gitignore for Angular project"
```

---

## Step 4: Make Initial Commits

### Task 4.1: Stage All Files

```bash
# Check what files will be staged
git status

# Stage all files
git add .
```

### Task 4.2: Create Initial Commit

```bash
# Commit with descriptive message
git commit -m "Initial Angular project setup

- Created new Angular project
- Added Angular-specific .gitignore
- Configured basic project structure"
```

### Task 4.3: Verify Initial Commit

```bash
# View commit log
git log

# View in short format
git log --oneline
```

---

## Step 5: Track Component Changes

### Task 5.1: Generate Angular Component

```bash
# Generate a new component
ng generate component components/todo-list
```

### Task 5.2: Check Status

```bash
# Check what changed
git status

# View specific changes
git diff
```

### Task 5.3: Commit Component

```bash
# Stage the new component
git add src/app/components/todo-list/

# Commit with message
git commit -m "feat(components): add todo list component

- Created TodoListComponent
- Added basic template structure
- Added component styles"
```

---

## Step 6: Practice Git Commands

### Task 6.1: View History

```bash
# View full log
git log

# View short log
git log --oneline

# View log with stats
git log --stat

# View log with patch
git log -p
```

### Task 6.2: Check Status

```bash
# View working directory status
git status

# View short status
git status -s
```

### Task 6.3: View Differences

```bash
# Make a change to a file
# Edit src/app/app.component.ts

# View unstaged changes
git diff

# Stage and view staged changes
git add src/app/app.component.ts
git diff --cached
```

---

## Step 7: Create Multiple Commits

### Task 7.1: Add Service

```bash
# Generate Angular service
ng generate service services/todo
```

### Task 7.2: Commit Service

```bash
# Stage and commit
git add src/app/services/todo.service.ts
git commit -m "feat(services): add todo service

- Created TodoService
- Added basic CRUD methods"
```

### Task 7.3: Add Model

```bash
# Create model file manually
mkdir -p src/app/models
echo "export interface Todo {
  id: number;
  title: string;
  completed: boolean;
}" > src/app/models/todo.model.ts

# Stage and commit
git add src/app/models/todo.model.ts
git commit -m "feat(models): add Todo interface"
```

### Task 7.4: View Final History

```bash
# View complete history
git log --oneline
```

---

## Exercise Requirements

### Exercise 1: Complete the Workflow

Complete all steps above and ensure:
- [ ] Git repository initialized
- [ ] .gitignore created and committed
- [ ] Initial commit made
- [ ] At least 5 commits in history

### Exercise 2: Modify Files

Make changes to existing files:
- Edit app.component.ts
- Edit app.component.html
- Commit each change separately

### Exercise 3: View History

Demonstrate:
- [ ] `git status` shows correct output
- [ ] `git log` shows all commits
- [ ] `git diff` shows changes

---

## Assessment Criteria

| Criteria | Points |
|----------|--------|
| Git initialized correctly | 10 |
| .gitignore created | 10 |
| Initial commit made | 10 |
| Component generated and committed | 15 |
| Multiple commits made | 15 |
| History viewed correctly | 10 |
| Git commands used properly | 15 |
| Best practices followed | 15 |

---

## Solution Commands

### Quick Reference

```bash
# Setup
git init
git config user.name "Name"
git config user.email "email"

# Workflow
git status
git add .
git add filename
git commit -m "message"

# History
git log
git log --oneline
git diff
git diff --cached
```

---

## Summary

This project teaches:

- Initializing Git repositories
- Creating proper .gitignore files
- The basic Git workflow (add, commit, status)
- Viewing project history
- Best practices for Angular projects

You now have a version-controlled Angular project ready for development!

---

**Next Project**: [Angular Team Collaboration](./Project_02_Angular_Team_Collaboration.md)
