# Merge vs Rebase

## What You'll Learn

- The difference between merge and rebase
- When to use each approach
- The golden rule of rebasing
- Interactive rebase for cleaning up commits
- Common pitfalls and how to avoid them

## Prerequisites

- Completed `03-branching-strategies.md`
- Understanding of branches and commits

## The Two Ways to Combine Changes

When you have diverged branches, you can bring them together in two ways:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    MERGE VS REBASE                                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  MERGE:                                                                     │
│  Creates a new "merge commit" that combines both branches                  │
│                                                                             │
│  Before:                                                                    │
│  main:    A──B──C                                                          │
│                \                                                            │
│  feature:     D──E                                                          │
│                                                                             │
│  After merge:                                                               │
│  main:    A──B──C─────M                                                    │
│                \     /                                                      │
│  feature:     D──E───                                                      │
│                                                                             │
│  REBASE:                                                                    │
│  Rewrites your commits on top of the target branch                         │
│                                                                             │
│  Before:                                                                    │
│  main:    A──B──C                                                          │
│                \                                                            │
│  feature:     D──E                                                          │
│                                                                             │
│  After rebase:                                                             │
│  main:    A──B──C                                                          │
│                    \                                                        │
│  feature:         D'──E'                                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## When to Use Merge

Use merge when:
- Working on public/shared branches
- You want to preserve the complete history
- You're merging feature branches into main
- You need to keep the branch history for audit

```bash
# First, make sure you're on the branch you want to merge INTO
git checkout main

# Merge the feature branch
git merge feature/new-login
```

🔍 **What this does:**
- Creates a merge commit
- Preserves all branch history
- Both branches stay intact

**Merge commit message:** You can customize or use the default:

```
Merge branch 'feature/new-login' into main

Added user authentication feature
```

## When to Use Rebase

Use rebase when:
- Updating your feature branch with latest main
- You want a clean, linear history
- You're working on your own local branch
- Before submitting a PR (for a clean history)

```bash
# Make sure you're on your feature branch
git checkout feature/my-feature

# Rebase onto main
git rebase main
```

🔍 **What this does:**
- Takes your commits
- Temporarily sets aside your changes
- Applies the target branch's commits
- Re-applies your changes on top

This creates a linear history—no merge commits!

## The Golden Rule of Rebasing

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    THE GOLDEN RULE                                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  NEVER REBASE PUBLIC SHARED BRANCHES!                                      │
│                                                                             │
│  ❌ NEVER DO:                                                               │
│  git checkout main                                                         │
│  git rebase feature/other-person's-branch                                  │
│                                                                             │
│  This rewrites shared history and breaks everyone's repo!                │
│                                                                             │
│  ✅ SAFE TO DO:                                                            │
│  git checkout my-feature                                                   │
│  git rebase main                                                           │
│                                                                             │
│  Your local branch, your rules.                                            │
│                                                                             │
│  If you've already pushed, you'll need to force push:                      │
│  git push --force-with-lease                                               │
│                                                                             │
│  Only do this if you're sure no one else is using your branch!            │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Interactive Rebase

Interactive rebase lets you:
- Squash commits (combine multiple into one)
- Edit commit messages
- Reorder commits
- Drop commits (delete them)

```bash
# Squash last 3 commits into one
git rebase -i HEAD~3
```

This opens an editor:

```bash
pick abc1234 First commit
pick def5678 Second commit  
pick ghi9012 Third commit

# Commands:
# p, pick   = use commit
# r, reword = use commit, but edit the message
# e, edit   = use commit, but stop for amending
# s, squash = use commit, but meld into previous commit
# f, fixup  = like "squash", but discard this commit's message
# x, exec   = run command (the rest of the line) using shell
# d, drop   = remove commit
```

Change to:

```bash
pick abc1234 First commit
squash def5678 Second commit
squash ghi9012 Third commit
```

This combines all three into one commit.

## Rebase Workflow Example

Here's a common workflow:

```bash
# 1. You're working on a feature
git checkout -b feature/user-auth
# ... make commits ...

# 2. Someone else pushes to main
# ... time passes ...

# 3. Update your branch with latest main
git fetch origin
git rebase origin/main

# 4. If conflicts, resolve them:
# (edit the conflicted files)
git add .
git rebase --continue

# 5. Keep making commits as needed

# 6. When ready to merge, push
git push origin feature/user-auth
```

## Merge vs Rebase: When to Choose

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CHOOSING THE RIGHT APPROACH                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  USE MERGE WHEN:                                                            │
│  • Merging feature branches into main                                      │
│  • Working with a team on shared branches                                  │
│  • You want to preserve complete history                                   │
│  • You need to track when branches diverged                               │
│                                                                             │
│  USE REBASE WHEN:                                                           │
│  • Updating your local feature branch from main                            │
│  • You want a clean, linear history                                        │
│  • Before creating a PR (to review changes cleanly)                       │
│  • Combining multiple small commits into one                              │
│                                                                             │
│  KEY INSIGHT:                                                              │
│  Use rebase for local cleanup, merge for integrating branches.            │
│                                                                             │
│  Both are valid—choose based on your workflow and team conventions.       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Handling Rebase Conflicts

When rebasing, you might encounter conflicts:

```
<<<<<<< HEAD
def authenticate():
    return "main version"
=======
def authenticate():
    return "your version"
>>>>>>> feature/my-feature
```

To resolve:

```bash
# 1. Edit the file to fix the conflict
# 2. Remove the conflict markers
# 3. Stage the fixed file
git add conflicted-file.py

# 4. Continue the rebase
git rebase --continue

# 5. If you want to abort
git rebase --abort
```

## Aborting Operations

```bash
# Abort a rebase
git rebase --abort

# Abort a merge
git merge --abort

# Undo a rebase (if it went wrong)
git reflog
# Find the state before rebase
git reset --hard HEAD@{number}
```

## Summary

- Merge creates a merge commit, preserving history
- Rebase rewrites commits for a linear history
- **Never rebase public/shared branches**
- Use interactive rebase to clean up commits
- Choose merge for integration, rebase for updating

## Next Steps

→ Continue to `05-git-workflows.md` to learn team-based Git workflows.
