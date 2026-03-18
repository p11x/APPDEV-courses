# Git Basics

## What You'll Learn

- Setting up Git for the first time
- Creating and cloning repositories
- Making commits (the core workflow)
- Viewing history and differences
- Undoing mistakes

## Prerequisites

- Completed `01-introduction-to-version-control.md`
- Git installed on your machine

## First-Time Setup

Before using Git, configure your identity:

```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

🔍 **What this does:**
- `git config` — Git's configuration command
- `--global` — Applies to all repositories on your machine (not just one project)
- `user.name` — Your name as it will appear in commits
- `user.email` — Your email for commit records

**Why this matters:** Every commit records who made it. This information is publicly visible on GitHub.

### Useful Additional Settings

```bash
# Use VS Code as your editor
git config --global core.editor "code --wait"

# Default branch name
git config --global init.defaultBranch main

# Enable helpful aliases
git config --global alias.st status
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.lg "log --oneline --graph --decorate"
```

## Creating a Repository

### From Scratch

```bash
mkdir my-project
cd my-project
git init
```

🔍 **What this does:**
- `mkdir my-project` — Creates a new folder
- `cd my-project` — Moves into that folder
- `git init` — Initializes a new Git repository in that folder

This creates a hidden `.git` folder that stores all version history.

### From Existing Code

```bash
git init
git add .
git commit -m "Initial commit"
```

🔍 **What this does:**
- `git init` — Initialize the repository
- `git add .` — Stage ALL files (the `.` means "everything in this folder")
- `git commit -m "Initial commit"` — Create the first commit

### Cloning an Existing Repository

```bash
git clone https://github.com/username/repository.git
```

🔍 **What this does:**
- `git clone` — Downloads a complete copy of a repository
- Creates a folder named after the repository
- Sets up the remote connection automatically

You can also clone to a specific folder:

```bash
git clone https://github.com/username/repository.git my-folder
```

## The Basic Workflow

Here's your daily workflow:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GIT WORKFLOW                                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  WORK ON FILES                                                             │
│       │                                                                    │
│       ▼                                                                    │
│  git status  ← Check what changed                                          │
│       │                                                                    │
│       ▼                                                                    │
│  git add <files>  ← Stage changes you want to commit                      │
│       │                                                                    │
│       ▼                                                                    │
│  git commit -m "Description"  ← Save a snapshot                            │
│       │                                                                    │
│       ▼                                                                    │
│  git push  ← Share with team (if ready)                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Checking Status

```bash
git status
```

This shows:
- Which files have changed
- Which files are staged (ready to commit)
- Which branch you're on

Typical output:

```
On branch main
Your branch is up to date with 'origin/main'.

Changes not staged for commit:
  (use "git add <file>..." to update what will be committed)
  (use "git restore <file>..." to discard changes in working directory)
	modified:   app.py
	deleted:    old_file.py

Untracked files:
  (use "git add <file>..." to see untracked files)
	new_feature.py
```

🔍 **What this means:**
- `Changes not staged` — Modified files not yet added to staging
- `Untracked files` — New files Git doesn't know about yet

## Staging Files

```bash
# Stage specific file
git add app.py

# Stage multiple files
git add app.py utils.py

# Stage all changes
git add .

# Stage all files with a pattern
git add *.py

# Add to staging, then immediately unstage (keep changes)
git add -p app.py
```

🔍 **The staging area is like a loading dock:**
- Files are prepared there before being "shipped" (committed)
- You can stage some files and not others
- Lets you craft exactly what goes in each commit

## Committing Changes

```bash
# Commit with a message
git commit -m "Add user authentication"

# Stage and commit in one step (only for tracked files)
git commit -am "Fix bug in login"

# Amend (change) the last commit
git commit --amend -m "New message"
```

🔍 **Commit message best practices:**
- First line: 50 characters or less
- Use imperative mood: "Add feature" not "Added feature"
- Be specific: "Fix login bug on line 42" not "Fixed stuff"

## Viewing History

```bash
# Basic log
git log

# One line per commit
git log --oneline

# With graph
git log --graph --oneline --all

# Show what changed in each commit
git log -p

# Show only last 5 commits
git log -5

# Show changes to a specific file
git log --follow filename.py
```

🔍 **The commit hash:**
Every commit has a unique identifier like `abc1234`. You can use this to:
- View that specific commit: `git show abc1234`
- Revert to that commit: `git checkout abc1234`
- Reference it in issues

## Viewing Differences

```bash
# See unstaged changes
git diff

# See staged changes (what will be committed)
git diff --staged

# See changes in a specific file
git diff app.py

# Compare branches
git diff main..feature-branch

# Compare commits
git diff abc1234..def5678
```

## Undoing Things

### Unstage a File

```bash
git reset HEAD app.py
```

🔍 **What this does:**
- Removes `app.py` from staging
- Keeps your changes in the working directory

### Discard Local Changes

```bash
# Discard changes to a file
git checkout -- app.py

# Or (newer syntax)
git restore app.py
```

🔍 **What this does:**
- Reverts `app.py` to the last committed version
- **Warning:** This permanently deletes your changes!

### Undo a Commit (Keep Changes)

```bash
# Undo last commit, keep files staged
git reset --soft HEAD~1

# Undo last commit, unstage files
git reset HEAD~1

# Undo last commit, discard changes
git reset --hard HEAD~1
```

🔍 **HEAD~1 means "one commit before HEAD":**
- `HEAD` is your current position
- `HEAD~1` is the commit before
- `HEAD~2` is two commits before

### Revert a Commit (Safe)

```bash
# Creates a new commit that undoes the previous
git revert abc1234
```

🔍 **Why revert over reset:**
- Doesn't rewrite history
- Safe for shared branches
- Creates a clear audit trail

## Working with Remotes

```bash
# See your remotes
git remote -v

# Add a remote
git remote add origin https://github.com/user/repo.git

# Fetch changes
git fetch origin

# Pull changes (fetch + merge)
git pull origin main

# Push changes
git push origin main
```

## Summary

- Configure Git once with your name and email
- Create repos with `git init` or clone with `git clone`
- The workflow: work → stage → commit → push
- Use `git status` to see what's happening
- Use `git diff` to see changes
- `git reset` and `git revert` for undoing

## Next Steps

→ Continue to `03-branching-strategies.md` to learn how to work with branches effectively.
