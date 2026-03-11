# Git Rebasing

## Topic Title
Rewriting History with Git Rebase

## Concept Explanation

Rebasing is an alternative to merging that rewrites the commit history to create a linear project timeline. While merging creates merge commits that show the actual history of branches diverging and coming together, rebasing replays your changes on top of another branch.

### What is Rebasing?

Rebasing moves the entire branch to begin from a different point. Instead of merging (which combines histories), rebasing rewrites one branch's history to appear as if it was built on top of another.

### Why Use Rebasing?

1. **Clean history**: Creates a linear, easy-to-follow commit history
2. **Cleaner code reviews**: Each commit stands alone
3. **Avoid merge commits**: No unnecessary merge commits
4. **Up-to-date branches**: Keep feature branches current with main

## Understanding Rebase vs Merge

### Merge Approach

```
main:     A---B---C---M
              \       /
feature:      D---E---

M = Merge commit
Creates: A-B-C-D-E-M
```

### Rebase Approach

```
Before rebase:
main:     A---B---C
feature:      D---E

After rebase:
main:     A---B---C---D'---E'
```

The commits D and E are "replayed" on top of C, creating new commits D' and E'.

## The `git rebase` Command

### Basic Syntax

```bash
# Rebase current branch onto main
git rebase main

# Rebase onto a specific branch
git rebase main feature-branch

# Interactive rebase
git rebase -i main
```

### Simple Rebase Workflow

```bash
# 1. Make sure on feature branch
git checkout feature/my-feature

# 2. Rebase onto main
git rebase main

# 3. If successful, fast-forward merge
git checkout main
git merge feature/my-feature
```

## Interactive Rebasing

Interactive rebase allows you to modify commits:

```bash
# Rebase last 3 commits
git rebase -i HEAD~3

# Rebase onto main interactively
git rebase -i main
```

### Interactive Commands

In the editor, you can change commands:

```
pick   a1b2c3d First commit message
pick   d4e5f6g Second commit message
pick   g7h8i9j Third commit message
```

Available commands:
- **pick** - Use commit as is
- **reword** - Change commit message
- **edit** - Stop to modify commit
- **squash** - Combine with previous commit
- **fixup** - Combine and discard message
- **drop** - Remove commit

### Example: Squash Commits

```bash
# Before: Multiple small commits
# abc1234 Add component
# def5678 Add styling
# ghi9012 Fix typo

git rebase -i HEAD~3

# Change to:
# pick abc1234 Add component
# squash def5678 Add styling
# squash ghi9012 Fix typo

# Result: Single commit with all changes
```

## Real-World Angular Examples

### Example 1: Keep Feature Branch Updated

```bash
# Feature branch was created from old main
git checkout main
git pull

git checkout feature/user-profile
git rebase main

# Now feature has latest main changes
# Resolve any conflicts if needed

# Then merge cleanly
git checkout main
git merge feature/user-profile
```

### Example 2: Clean Up Commit History

```bash
# After working on a feature, clean up commits
git rebase -i main

# Edit to combine commits:
# pick abc1234 Add user component
# squash def5678 Add user template  
# squash ghi9012 Add user styles
# squash jkl3456 Fix import error
```

### Example 3: Split a Commit

```bash
# Start interactive rebase
git rebase -i HEAD~1

# Change to 'edit' for the commit you want to split
# After Git stops:

git reset HEAD^
git add file1.txt
git commit -m "Add file 1"
git add file2.txt
git commit -m "Add file 2"

git rebase --continue
```

## Rebasing Best Practices

### 1. Never Rebase Public/Branch History

```bash
# ✗ Bad: Rebase commits already pushed
git rebase main
git push --force  # DANGER!

# ✓ Good: Rebase local commits before pushing
git rebase main
git push
```

### 2. Use Rebase for Local Changes

```bash
# Good use: Clean up local commits before pushing
git rebase -i HEAD~3

# Before pushing:
git push
```

### 3. Don't Rebase Published Feature Branches

```bash
# Only rebase if you're the only one on the branch
# Don't rebase if teammates are also using the branch
```

### 4. Use Merge for Long-Lived Branches

```bash
# For branches with many team members:
git checkout main
git pull
git merge feature/long-running  # Use merge instead
```

## When to Use Merge vs Rebase

### Use Merge When:
- Working with a team on the same branch
- You want to preserve complete history
- Branch has already been pushed

### Use Rebase When:
- Updating local feature branch
- Cleaning up commit history
- Creating a clean linear history for PR

## Resolving Rebase Conflicts

### Process

```bash
# Rebase may encounter conflicts
git rebase main

# Conflict occurs
# Resolve conflicts in editor
git add resolved-file.txt

# Continue rebase
git rebase --continue

# Skip the problematic commit
git rebase --skip

# Abort if things go wrong
git rebase --abort
```

## Common Mistakes

### Mistake 1: Force Pushing

```bash
# ✗ Dangerous: Overwrites remote history
git push --force

# Only do if absolutely necessary and you're sure!
```

### Mistake 2: Rebasing Public Branches

```bash
# ✗ Bad: Changes shared history
git rebase main
git push --force

# Can break teammates' repositories!
```

### Mistake 3: Not Saving Work Before Rebase

```bash
# ✓ Always backup first
git branch backup-branch

# Then rebase
git rebase main

# If things go wrong:
git reset --hard backup-branch
```

## Exercises for Students

### Exercise 1: Basic Rebase
1. Create a branch from main
2. Make a commit
3. Add a commit to main
4. Rebase branch onto main
5. See the new commit positions

### Exercise 2: Interactive Rebase
1. Make 3 commits on a branch
2. Use interactive rebase to squash them
3. See the combined commit

### Exercise 3: Update Feature Branch
1. Create feature branch
2. Let main advance (add commits)
3. Rebase feature onto main
4. Merge cleanly

## Mini Practice Tasks

### Task 1: Simple Rebase
```bash
# 1. Create branch
git checkout -b feature/test
echo "feature" > test.txt
git add .
git commit -m "Add test file"

# 2. Add commit to main
git checkout main
echo "main" > main.txt
git add .
git commit -m "Add main file"

# 3. Rebase
git checkout feature/test
git rebase main

# 4. Merge
git checkout main
git merge feature/test
```

### Task 2: Interactive Squash
```bash
# 1. Make multiple commits
git checkout -b feature/small
echo "1" > file.txt
git add . && git commit -m "Change 1"
echo "2" >> file.txt
git add . && git commit -m "Change 2"
echo "3" >> file.txt
git add . && git commit -m "Change 3"

# 2. Squash
git rebase -i HEAD~3

# Change to:
# pick abcdef1 Change 1
# squash defghi2 Change 2  
# squash efghij3 Change 3
```

### Task 3: Rebase Main onto Feature
```bash
# Sometimes you rebase main onto feature
# (instead of feature onto main)

git checkout feature/my-feature
git rebase main

# Now feature includes main's changes
# Good when you need latest main in your feature
```

## Summary

Rebasing rewrites commit history:

- **`git rebase main`** - Replay commits on main
- **`git rebase -i`** - Interactive rebase for editing
- **Squash** - Combine multiple commits
- **Reword** - Change commit messages

Key differences from merge:
- Creates linear history
- Rewrites commits (new SHA)
- No merge commits

Remember:
- Never rebase public/shared branches
- Use for local cleanup
- Keep backups before major rebases

Rebasing creates cleaner project history!

---

**Next Lesson**: [Working with Remote Repositories](./13_Working_with_Remote_Repositories.md)

**Previous Lesson**: [Git Merging](./11_Git_Merging.md)
