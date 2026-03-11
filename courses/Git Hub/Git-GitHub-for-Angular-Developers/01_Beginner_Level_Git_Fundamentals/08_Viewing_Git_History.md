# Viewing Git History

## Topic Title
Exploring and Understanding Your Project History

## Concept Explanation

Git stores a complete history of all changes made to your project. Learning to view and interpret this history is crucial for understanding how your project evolved, finding bugs, and collaborating with other developers.

### Why Viewing History Matters

1. **Understanding evolution**: See how your project changed over time
2. **Finding bugs**: Use history to identify when problems were introduced
3. **Code review**: Examine what changes were made and why
4. **Collaboration**: Understand what others have contributed
5. **Documentation**: History serves as documentation of decisions

### Key Commands for Viewing History

- `git log` - View commit history
- `git status` - Check current state of files
- `git diff` - View differences between changes

## The `git log` Command

### Basic Usage

```bash
# View full commit history
git log

# View in short format (one line per commit)
git log --oneline

# View limited number of commits
git log -n 5

# Example output (oneline):
a1b2c3d Add user authentication
4e5f6g7 Fix login validation bug
9h8i7j6 Update README documentation
2k3l4m5 Create initial project setup
```

### Useful Log Options

```bash
# Show statistics (files changed, lines added/removed)
git log --stat

# Show patch (actual changes)
git log -p

# Show in graph format
git log --graph

# Show relative dates
git log --relative-date

# Show commits by author
git log --author="John"

# Show commits that modified a specific file
git log -- filename

# Show commits since a date
git log --since="2026-01-01"

# Show commits until a date
git log --until="2026-03-01"
```

### Example Output Formats

#### Standard Format
```
commit a1b2c3d4e5f6g7h8i9j0
Author: Jane Developer <jane@example.com>
Date:   Mon Mar 8 10:30:00 2026

    Add user authentication service

    - Create UserService with login/logout methods
    - Add JWT token handling
    - Implement auth guards
```

#### Oneline with Graph
```
* a1b2c3d Add user authentication
* 4e5f6g7 Fix login validation bug
* 9h8i7j6 Update README documentation
* 2k3l4m5 Create initial project setup
```

## The `git status` Command

### Basic Usage

```bash
# Check current status
git status

# Shorter format
git status -s

# Example output:
On branch main
Changes not staged for commit:
  (use "git add <file>..." to update what will be committed)
  (use "git restore <file>..." to discard changes in working directory)
    modified:   src/app/app.component.ts

Untracked files:
  (use "git add <file>..." to include in what will be committed)
    src/app/new-component/
```

### Short Format Legend

```
M  src/app/app.component.ts  = Modified
A  src/app/new.component.ts  = Added
D  src/app/old.component.ts  = Deleted
?? src/app/untracked/        = Untracked
```

### Status Meanings

| Status | Meaning |
|--------|---------|
| Modified | File changed but not staged |
| Staged | File marked for commit |
| Untracked | New file Git doesn't know about |
| Deleted | File removed |
| Renamed | File renamed |

## The `git diff` Command

### Understanding Differences

The `git diff` command shows the differences between various states:
- Working directory vs staging
- Working directory vs last commit
- Staging area vs last commit

### Types of Diff

```bash
# Show unstaged changes (working dir vs staging)
git diff

# Show staged changes (staging vs last commit)
git diff --cached

# Show changes between two commits
git diff commit1 commit2

# Show changes in a specific file
git diff filename

# Show changes in a specific directory
git diff src/
```

### Reading Diff Output

```
diff --git a/src/app/app.component.ts b/src/app/app.component.ts
--- a/src/app/app.component.ts      (old version)
+++ b/src/app/app.component.ts      (new version)
@@ -1,5 +1,6 @@                   (line 1-5 changed to 1-6)
 import { Component } from '@angular/core';
 
 @Component({
-  selector: 'app-root',
-  title: 'My App'
+  selector: 'app-root',
+  title: 'Angular App',
+  template: '<h1>{{title}}</h1>'
 })
```

### Diff Symbols

| Symbol | Meaning |
|--------|---------|
| `+` | Added line |
| `-` | Removed line |
| `@@` | Chunk of changes |
| `a/` | Original file |
| `b/` | Modified file |

## Real-World Angular Examples

### Example 1: Finding When a Bug Was Introduced

```bash
# Bug discovered in UserService
# Find when it was last working

# Check recent commits to UserService
git log --oneline src/app/services/user.service.ts

# View specific commit changes
git log -p src/app/services/user.service.ts

# Compare to previous version
git diff HEAD~1 HEAD -- src/app/services/user.service.ts
```

### Example 2: Reviewing Team Changes

```bash
# See recent commits by team
git log --oneline --author="Team Member"

# See what changed in a specific commit
git show commit-hash

# Example:
git show a1b2c3d
# Shows:
# commit a1b2c3d4...
# Author: Team Member <email>
# Date: Mon Mar 8
#
# Add new feature
#
# diff...
```

### Example 3: Checking Unpushed Commits

```bash
# See commits not yet pushed
git log origin/main..main

# See what's different from remote
git diff main origin/main
```

### Example 4: Understanding Component History

```bash
# View history of a component
git log --oneline src/app/header/

# See specific changes
git log -p src/app/header/header.component.ts
```

## Combining Commands

### Useful Combinations

```bash
# View recent commits with changes
git log -p -1

# See what files changed recently
git log --name-status -5

# See summary of changes
git log --shortstat -3

# Graph with branch names
git log --graph --oneline --all
```

### Aliases for Better Viewing

```bash
# Create useful aliases
git config --global alias.lg "log --oneline --graph --all"
git config --global alias.st "status -s"
git config --global alias.df "diff"
```

## Best Practices

### 1. Use --oneline for Quick Overview

```bash
# Quick view of recent commits
git log --oneline -10
```

### 2. Check Status Before Every Commit

```bash
# Always check what you're about to commit
git status
git diff --cached
```

### 3. Review Changes Before Staging

```bash
# See exactly what will be committed
git diff
```

### 4. Use Meaningful Log Filters

```bash
# Find commits related to a specific file
git log --follow -- filename

# Find commits by message
git log --grep="fix"
```

## Common Mistakes

### Mistake 1: Not Checking Status Before Committing

```bash
# ✗ Bad: Commit without checking
git commit -m "My changes"

# ✓ Good: Check first
git status
git diff --cached
git commit -m "My changes"
```

### Mistake 2: Confusing Diff Options

```bash
# Shows different things:
git diff          # working dir vs staged
git diff --cached # staged vs last commit
git diff HEAD     # working dir vs last commit
```

### Mistake 3: Not Using Relative Dates

```bash
# Hard to read dates
git log

# Easier to read
git log --relative-date

# Or use human-readable
git log --since="2 weeks ago"
```

## Exercises for Students

### Exercise 1: Explore History
1. Make several commits in your project
2. View history with different options
3. Practice using `--oneline`, `--stat`, `-p`

### Exercise 2: Use Status Effectively
1. Create a new file
2. Modify an existing file
3. Delete a file
4. Check `git status -s` and understand each symbol

### Exercise 3: Practice Diff
1. Make changes to a file
2. Use `git diff` to see unstaged changes
3. Stage the file
4. Use `git diff --cached` to see staged changes

## Mini Practice Tasks

### Task 1: View Log History
```bash
# Make some commits if you haven't
echo "change 1" > file1.txt
git add file1.txt
git commit -m "Add file1"

echo "change 2" > file2.txt
git add file2.txt
git commit -m "Add file2"

# View history
git log
git log --oneline
git log --stat
```

### Task 2: Understand Status
```bash
# Create untracked file
echo "new" > newfile.txt

# Modify tracked file
echo "more" >> existing.txt
git add existing.txt

# Check status
git status
git status -s

# Notice different symbols
```

### Task 3: Use Diff
```bash
# Modify a file
echo "new content" >> app.component.ts

# See unstaged changes
git diff

# Stage and see staged changes
git add app.component.ts
git diff --cached
```

### Task 4: Find Specific History
```bash
# Find commits affecting a specific file
git log --oneline src/app/app.component.ts

# Find commits by a specific author
git log --oneline --author="Your Name"

# Find commits with specific text
git log --oneline --grep="feature"
```

## Summary

Viewing Git history is essential for effective version control:

- **`git log`** - View commit history with many formatting options
- **`git status`** - Check current state of working directory
- **`git diff`** - View differences between file versions

These commands help you:
- Understand project history
- Find bugs and their origins
- Review changes before committing
- Collaborate effectively with your team

Master these commands to become proficient with Git!

---

**Next Lesson**: [Git Ignore](./09_Git_Ignore.md)

**Previous Lesson**: [Git Add and Commit](./07_Git_Add_and_Commit.md)
