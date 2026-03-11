# Git Stashing

## Topic Title
Temporarily Saving Changes with Git Stash

## Concept Explanation

Git stash is a powerful command that allows you to temporarily save changes that are not ready to be committed. It's particularly useful when you need to switch branches quickly but don't want to commit incomplete work.

### What is Stashing?

Stashing takes your uncommitted changes (staged and unstaged), saves them away, and then reverts the working directory to match the last commit. You can later restore these changes.

### Why Use Stashing?

1. **Switch branches**: Move to another branch without committing
2. **Save work in progress**: Keep changes without committing
3. **Pull changes**: Apply incoming changes while saving your work
4. **Experiment**: Try different approaches without losing work

## The `git stash` Command

### Basic Stashing

```bash
# Stash changes (staged and unstaged)
git stash

# Stash with a message
git stash save "Work in progress on login"
git stash push -m "Work in progress on login"

# Stash including untracked files
git stash -u
git stash --include-untracked
```

### Stashing Options

```bash
# Stash staged changes only
git stash --staged

# Stash all including ignored files
git stash -a
git stash --all

# Interactive stash
git stash -p
```

## Viewing and Managing Stashes

### List Stashes

```bash
# List all stashes
git stash list

# Output:
# stash@{0}: WIP on main: abc123 Last commit message
# stash@{1}: On feature-branch: Previous stash
```

### Show Stash Contents

```bash
# Show latest stash diff
git stash show

# Show with diff
git stash show -p
git stash show --patch

# Show specific stash
git stash show stash@{0}
```

### Applying Stashes

```bash
# Apply latest stash (keep stash)
git stash apply

# Apply specific stash
git stash apply stash@{1}

# Apply and remove from stash list
git stash pop
git stash pop stash@{0}
```

### Dropping Stashes

```bash
# Delete latest stash
git stash drop

# Delete specific stash
git stash drop stash@{1}

# Clear all stashes
git stash clear
```

## Real-World Angular Examples

### Example 1: Emergency Bug Fix

```bash
# Working on feature, bug fix needed immediately
git status
# Changes in progress: 5 files modified

# Quick! Save work
git stash

# Fix the bug
git checkout main
git pull
git checkout -b fix/urgent-bug

# ... fix the bug ...
git add .
git commit -m "Fix urgent login bug"
git push

# Back to feature
git checkout feature/my-feature
git stash pop
```

### Example 2: Pulling While Working

```bash
# Need to pull main, but have uncommitted changes
git stash

# Pull updates
git pull origin main

# Restore work
git stash pop
```

### Example 3: Switching Branches

```bash
# Need to check something on another branch
git stash

# Switch branch
git checkout other-branch
# ... do quick check ...

# Switch back
git checkout feature-branch
git stash pop
```

## Stash Branch

### Create Branch from Stash

```bash
# Create and switch to new branch from stash
git stash branch new-branch-name

# Create from specific stash
git stash branch new-branch stash@{1}

# Example:
git stash branch feature-login-bug stash@{0}
```

This is useful when:
- Stash has conflicts
- You want to continue work in a branch

## Stashing Best Practices

### 1. Use Descriptive Messages

```bash
# ✓ Good
git stash push -m "WIP: UserService refactoring"
git stash push -m "Draft: Login component styles"

# ✗ Bad
git stash
git stash save "stuff"
```

### 2. Don't Stash Too Long

```bash
# ✓ Good: Apply stash soon
git stash
# ... quick branch switch ...
git stash pop

# ✗ Bad: Stash accumulates
git stash
# ... weeks later ...
git stash list  # Many stashes!
```

### 3. Include Untracked When Needed

```bash
# Stash includes untracked files
git stash -u
git stash --include-untracked

# Needed for new files
```

## Common Mistakes

### Mistake 1: Forgetting What's Stashed

```bash
# Always check before dropping
git stash list
git stash show stash@{0}
```

### Mistake 2: Stash Conflicts

```bash
# May have conflicts when applying
git stash apply

# Resolve conflicts, then:
git add .
git commit -m "Resolve conflicts"
```

### Mistake 3: Using Stash Instead of Branch

```bash
# ✗ Bad: Using stash for long-term work
git stash
# ... months later ...
git stash pop  # Mess!

# ✓ Good: Use branch for any significant work
git checkout -b feature/new-login
```

## Exercises for Students

### Exercise 1: Basic Stash
1. Make changes to files
2. Stash changes
3. Check status
4. Apply stash
5. Verify changes restored

### Exercise 2: Multiple Stashes
1. Make changes, stash with message
2. Make different changes, stash with message
3. List stashes
4. Apply specific stash

### Exercise 3: Emergency Workflow
1. Start work on feature
2. Simulate urgent fix needed
3. Stash work
4. Switch branches
5. Complete fix
6. Restore stashed work

## Summary

Git stash temporarily saves uncommitted changes:

- **`git stash`** - Save changes temporarily
- **`git stash pop`** - Apply and remove stash
- **`git stash list`** - View all stashes
- **`git stash drop`** - Delete stash
- **`git stash branch`** - Create branch from stash

Key use cases:
- Switch branches quickly
- Save work in progress
- Pull changes with local modifications
- Handle emergency fixes

Master stash for flexible development workflows!

---

**Next Lesson**: [Git Reset and Revert](./20_Git_Reset_and_Revert.md)

**Previous Lesson**: [GitHub Issues](./../02_Intermediate_Level_Git_Workflows/18_GitHub_Issues.md)
