# Git Branches

## Topic Title
Understanding and Using Branches in Git

## Concept Explanation

Branches are one of Git's most powerful features. They allow you to work on different versions of your project simultaneously. Think of branches as parallel universes where you can make changes without affecting the main codebase.

### What is a Branch?

A branch in Git is simply a lightweight movable pointer to a specific commit. By default, every repository starts with a main branch (formerly called "master").

### Why Use Branches?

1. **Isolate work**: Work on features without affecting stable code
2. **Parallel development**: Multiple team members can work simultaneously
3. **Experiment safely**: Try new ideas without risk
4. **Organize work**: Separate features, bug fixes, and experiments
5. **Release management**: Maintain different versions of your software

### Visual Representation

```
                    feature-branch
                        ↓
main: A --- B --- C --- D --- E
                    ↑
              bugfix-branch
```

## The `git branch` Command

### Viewing Branches

```bash
# List all local branches
git branch

# List all branches (local and remote)
git branch -a

# List remote branches
git branch -r

# List branches with last commit info
git branch -v
```

### Creating Branches

```bash
# Create a new branch (but don't switch to it)
git branch feature-login

# Create and switch to new branch
git checkout -b feature-login

# Or using git switch (modern Git)
git switch -c feature-login
```

### Deleting Branches

```bash
# Delete a local branch (safe - only if merged)
git branch -d feature-login

# Force delete a local branch
git branch -D feature-login

# Delete a remote branch
git push origin --delete feature-login
```

## The `git checkout` and `git switch` Commands

### Switching Branches

```bash
# Switch to an existing branch
git checkout feature-login

# Or using git switch (modern Git)
git switch feature-login

# Switch to previous branch
git checkout -

# Or
git switch -
```

### Creating and Switching

```bash
# Old way (still works)
git checkout -b new-feature

# Modern way
git switch -c new-feature
```

### Important: Uncommitted Changes

Before switching branches, either:
- Commit your changes
- Stash your changes (covered later)

```bash
# If you have uncommitted changes:
git stash  # Save changes temporarily
git checkout other-branch
# ... work on other branch ...
git checkout first-branch
git stash pop  # Restore changes
```

## Working with Branches in Angular

### Example: Creating a Feature Branch

```bash
# Start from main
git checkout main
git pull origin main

# Create feature branch
git checkout -b feature/user-dashboard

# Generate Angular component
ng generate component components/user-dashboard

# Work on the feature
# ... make changes ...

# Stage and commit
git add .
git commit -m "feat(dashboard): add user dashboard component"

# Push to remote
git push -u origin feature/user-dashboard
```

### Example: Bug Fix Branch

```bash
# Create bug fix branch from main
git checkout -b fix/login-validation

# Fix the bug
# ... edit files ...

# Commit the fix
git add .
git commit -m "fix: resolve login validation issue"

# Push
git push -u origin fix/login-validation
```

### Example: Release Branch

```bash
# Create release branch
git checkout -b release/v1.0

# Make final adjustments
# ... changes ...

# Commit
git add .
git commit -m "chore: prepare v1.0 release"

# Merge to main
git checkout main
git merge release/v1.0

# Tag the release
git tag v1.0

# Delete release branch
git branch -d release/v1.0
```

## Branch Naming Conventions

### Common Branch Types

| Type | Prefix | Example |
|------|--------|---------|
| Feature | feature/ | feature/user-auth |
| Bug Fix | fix/ | fix/login-error |
| Hotfix | hotfix/ | hotfix/security-patch |
| Release | release/ | release/v1.0 |
| Experiment | experiment/ | experiment/new-design |

### Best Practices

```bash
# ✓ Good branch names
feature/add-user-profile
fix/login-validation-error
bugfix/navbar-z-index
release/v1.0.0

# ✗ Bad branch names
fix
my-branch
feature
```

## Understanding HEAD

HEAD is a special pointer that indicates where you currently are in the repository:

```bash
# See where HEAD points
cat .git/HEAD
# Output: ref: refs/heads/main

# Or using Git command
git rev-parse HEAD

# See recent HEAD movements
git reflog
```

### Detached HEAD

When you checkout a specific commit (not a branch), you enter "detached HEAD" state:

```bash
# Checkout a specific commit
git checkout a1b2c3d

# You'll see:
# Note: checking out 'a1b2c3d'.
# You are in 'detached HEAD' state.

# To save work, create a branch
git checkout -b new-branch-from-commit
```

## Real-World Angular Examples

### Scenario: Team Feature Development

```
Timeline:

Developer A:
main:    A---B---C---D---E---F
              \
feature-A:     \---X---Y

Developer B:
main:    A---B---C---D---E---F
              \
feature-B:     \---P---Q---R
```

### Merging Features

```bash
# Developer A finishes feature-A
git checkout main
git merge feature-A

# Result:
main:    A---B---C---D---E---F---M
              \                 /
feature-A:     \---X---Y------/

# Developer B finishes feature-B
git merge feature-B

# Result:
main:    A---B---C---D---E---F---M---N
              \                 /   /
feature-A:     \---X---Y------/   /
              \                  /
feature-B:     \---P---Q---R----/
```

## Best Practices

### 1. Keep Branch Life Short

```bash
# ✓ Good: Short-lived branches
git checkout -b feature/small-feature
# ... work ...
git merge

# ✗ Bad: Long-lived branches
# Work for months on one branch
# This leads to merge conflicts!
```

### 2. Sync Branch Regularly

```bash
# Before starting work
git pull origin main

# During work, rebase on main
git fetch origin
git rebase origin/main
```

### 3. Delete Merged Branches

```bash
# Clean up after merging
git branch -d feature-completed
git push origin --delete feature-completed
```

### 4. Use Descriptive Names

```bash
# ✓ Clear and descriptive
git checkout -b feature/add-user-profile-picture-upload

# ✗ Vague
git checkout -b feature1
```

## Common Mistakes

### Mistake 1: Working on Main Branch

```bash
# ✗ Bad: Working directly on main
# Any commit goes directly to production code!

# ✓ Good: Use feature branches
git checkout -b feature/new-login
# ... work ...
git checkout main
git merge feature/new-login
```

### Mistake 2: Forgetting Which Branch You're On

```bash
# Check before making commits
git branch  # Shows current branch with *

# Or use
git status  # Shows "On branch X"
```

### Mistake 3: Not Pushing Branch to Remote

```bash
# ✗ Local-only branch could be lost

# ✓ Push to remote for backup
git push -u origin feature/my-feature
```

## Exercises for Students

### Exercise 1: Create and Switch Branches
1. Create a new branch called "experiment"
2. Switch to that branch
3. Make a commit
4. Switch back to main
5. See the different histories

### Exercise 2: Feature Branch Workflow
1. Create a feature branch
2. Add an Angular component
3. Commit the changes
4. Switch to main and verify files aren't there
5. Merge the feature branch

### Exercise 3: Branch Naming
Practice creating branches with proper naming:
- feature/user-registration
- fix/navbar-bug
- experiment/dark-mode

## Mini Practice Tasks

### Task 1: Basic Branching
```bash
# 1. Check current branch
git branch

# 2. Create new branch
git checkout -b feature/test

# 3. Verify you're on new branch
git branch

# 4. Make a commit
echo "test" > test.txt
git add .
git commit -m "Add test file"

# 5. Switch back
git checkout main
```

### Task 2: Merge Feature Branch
```bash
# 1. Create and switch to branch
git checkout -b feature/add-login

# 2. Add files
echo "Login" > login.txt
git add .
git commit -m "Add login feature"

# 3. Switch to main
git checkout main

# 4. Merge the feature
git merge feature/add-login

# 5. Delete branch
git branch -d feature/add-login
```

### Task 3: Angular Component Branch
```bash
# 1. Create branch for new component
git checkout -b feature/add-header

# 2. Generate Angular component
ng generate component components/header

# 3. Commit changes
git add .
git commit -m "feat: add header component"

# 4. Push to remote
git push -u origin feature/add-header
```

## Summary

Branches are essential for effective development:

- **`git branch`** - List, create, delete branches
- **`git checkout`** - Switch branches (older syntax)
- **`git switch`** - Switch branches (modern syntax)
- **Branch naming** - Use descriptive prefixes like feature/, fix/

Key concepts:
- Branches are lightweight pointers to commits
- Always use branches for new work
- Keep branches short-lived
- Sync with main regularly

Branches enable safe experimentation and parallel development!

---

**Next Lesson**: [Git Merging](./11_Git_Merging.md)

**Previous Lesson**: [.gitignore](./../01_Beginner_Level_Git_Fundamentals/09_Git_Ignore.md)
