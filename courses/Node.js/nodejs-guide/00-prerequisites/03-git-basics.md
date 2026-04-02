# Git Basics

## What You'll Learn

- How Git version control works
- Essential Git commands for Node.js projects
- How to use .gitignore for Node.js
- Branching strategies for teams
- How to work with GitHub/GitLab

## What Is Git?

Git is a **distributed version control system**. It tracks changes to your files over time, lets you revert to previous versions, and enables multiple people to work on the same codebase without conflicts.

## Initial Setup

```bash
# Configure your identity (once per machine)
git config --global user.name "Your Name"
git config --global user.email "your@email.com"

# Verify configuration
git config --list
```

## Core Workflow

```bash
# 1. Initialize a new repository
git init

# 2. Clone an existing repository
git clone https://github.com/user/repo.git

# 3. Check status
git status

# 4. Stage changes
git add index.js          # Stage specific file
git add .                 # Stage all changes
git add -p                # Interactive staging (review each change)

# 5. Commit
git commit -m "Add user authentication"

# 6. Push to remote
git push origin main

# 7. Pull latest changes
git pull origin main
```

## .gitignore for Node.js

Create `.gitignore` in your project root:

```gitignore
# Dependencies (reinstall with npm install)
node_modules/

# Build output
dist/
build/

# Environment variables (NEVER commit secrets)
.env
.env.local
.env.production

# OS files
.DS_Store
Thumbs.db

# IDE files
.vscode/
.idea/
*.swp
*.swo

# Logs
logs/
*.log
npm-debug.log*

# Coverage reports
coverage/

# Temporary files
tmp/
temp/
```

## Branching

```bash
# Create a new branch
git checkout -b feature/user-auth

# Work on the branch
git add .
git commit -m "Add login endpoint"

# Push the branch
git push origin feature/user-auth

# Switch back to main
git checkout main

# Merge the branch
git merge feature/user-auth

# Delete the branch
git branch -d feature/user-auth
```

### Branch Naming Conventions

```
feature/user-auth       # New feature
bugfix/login-error      # Bug fix
hotfix/security-patch   # Urgent production fix
chore/update-deps       # Maintenance task
docs/api-reference      # Documentation update
```

## Working with Remote Repositories

```bash
# Add a remote
git remote add origin https://github.com/user/repo.git

# View remotes
git remote -v

# Fetch changes without merging
git fetch origin

# Pull changes (fetch + merge)
git pull origin main

# Push changes
git push origin main

# Force push (dangerous — use with caution)
git push --force origin main
```

## Viewing History

```bash
# View commit log
git log
git log --oneline          # Compact view
git log --oneline -10      # Last 10 commits
git log --graph            # Visual branch history

# View changes
git diff                   # Unstaged changes
git diff --staged          # Staged changes
git diff main..feature     # Compare branches

# View a specific commit
git show abc1234
```

## Undoing Changes

```bash
# Unstage a file (keep changes)
git reset HEAD index.js

# Discard changes to a file
git checkout -- index.js

# Undo last commit (keep changes staged)
git reset --soft HEAD~1

# Undo last commit (discard changes)
git reset --hard HEAD~1

# Create a new commit that undoes a previous commit
git revert abc1234
```

## Stashing

```bash
# Save work in progress without committing
git stash

# List stashes
git stash list

# Apply and remove latest stash
git stash pop

# Apply without removing
git stash apply

# Drop a stash
git stash drop
```

## Common Mistakes

### Mistake 1: Committing node_modules

```bash
# WRONG — node_modules is huge and platform-specific
git add .
git commit -m "Initial commit"

# CORRECT — add node_modules to .gitignore first
echo "node_modules/" >> .gitignore
git add .
git commit -m "Initial commit"
```

### Mistake 2: Committing Secrets

```bash
# WRONG — API keys in code
const API_KEY = 'sk-abc123secret';
git add .
git commit -m "Add API integration"

# CORRECT — use environment variables
const API_KEY = process.env.API_KEY;
echo ".env" >> .gitignore
```

### Mistake 3: Force Pushing to Shared Branches

```bash
# WRONG — overwrites other people's work
git push --force origin main

# CORRECT — create a new branch, merge via PR
git checkout -b fix/my-changes
git push origin fix/my-changes
# Create pull request on GitHub
```

## Next Steps

Now you can use Git for version control. Let's put it all together with a terminal cheatsheet. Continue to [Terminal Cheatsheet](./04-terminal-cheatsheet.md).
