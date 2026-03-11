# Git Merging

## Topic Title
Combining Branch Changes with Git Merge

## Concept Explanation

Merging is the process of combining changes from one branch into another. In Git, you merge branches to integrate feature work, bug fixes, or any changes made on separate branches back into your main codebase.

### What is Merging?

Merging takes the commits from one branch and applies them to another. This allows teams to work on separate features in parallel and then combine their work into a unified codebase.

### Why Merging Matters

1. **Combine work**: Bring together different lines of development
2. **Integrate features**: Add completed features to the main codebase
3. **Preserve history**: Maintain a complete record of all changes
4. **Collaborate**: Enable multiple developers to work simultaneously

## The `git merge` Command

### Basic Syntax

```bash
# Merge a branch into current branch
git merge branch-name

# Example: Merge feature branch into main
git checkout main
git merge feature-login
```

### Types of Merges

#### 1. Fast-Forward Merge

When the branch you're merging is directly ahead of the current branch, Git performs a "fast-forward" merge:

```
Before merge:
main:    A---B---C
                  ↑
feature:         D---E---F
                          ↑
                        main

After fast-forward:
main:    A---B---C---D---E---F
```

Command:
```bash
git checkout main
git merge feature-login
```

#### 2. Three-Way Merge

When branches have diverged, Git creates a merge commit:

```
Before merge:
main:    A---B---C---X
              \     ↑
feature:       D---E
                    ↑
                  main

After merge:
main:    A---B---C---X---M
              \         /
feature:       D---E---- 
                    ↑
                   M (merge commit)
```

### Merging in Practice

### Step-by-Step: Merge a Feature Branch

```bash
# 1. Make sure you're on main
git checkout main

# 2. Pull latest changes
git pull origin main

# 3. Merge the feature branch
git merge feature/user-dashboard

# 4. If successful, you'll see:
# Merge made by the 'recursive' strategy.
# a1b2c3d..d4e5f6g  feature/user-dashboard -> main

# 5. Delete the feature branch (optional)
git branch -d feature/user-dashboard
```

## Merge Conflicts

### What Are Merge Conflicts?

A merge conflict occurs when the same part of a file has been changed in both branches and Git cannot automatically determine which change to keep.

### Visualizing Conflicts

```
Main branch:
<<<<<<< HEAD
const title = 'My App';
=======
const title = 'Angular App';
>>>>>>> feature/new-title
```

### Resolving Conflicts

#### Step 1: Identify Conflicts

```bash
# Merge will show conflicts
git merge feature-branch

# Output:
# Auto-merging src/app/app.component.ts
# CONFLICT (content): Merge conflict in src/app/app.component.ts
# Automatic merge failed; fix conflicts and then commit
```

#### Step 2: Check Conflicted Files

```bash
# See which files have conflicts
git status

# Files will show "both modified"
```

#### Step 3: Edit the File

Open the conflicted file and choose what to keep:

```typescript
<<<<<<< HEAD
@Component({
  selector: 'app-root',
  title: 'My App'
=======
@Component({
  selector: 'app-root',
  title: 'Angular App',
  template: '<h1>{{title}}</h1>'
>>>>>>> feature/new-title
```

Choose one version or combine:

```typescript
// Choose one version:
@Component({
  selector: 'app-root',
  title: 'Angular App'
})

// Or combine both:
@Component({
  selector: 'app-root',
  title: 'Angular App',
  template: '<h1>{{title}}</h1>'
})
```

#### Step 4: Mark as Resolved

```bash
# After editing, stage the file
git add filename

# Or mark all as resolved
git add .
```

#### Step 5: Complete the Merge

```bash
# If merging, complete with commit
git commit

# Git will open editor with merge message
# Save and close to complete
```

### Aborting a Merge

If things go wrong:

```bash
# Abort the merge
git merge --abort
```

## Angular Example: Merging Feature Branches

### Scenario

Two developers work on different features:
- Developer A: Add user profile component
- Developer B: Add login component

### Workflow

```bash
# Developer A: Merge user profile feature
git checkout main
git pull origin main
git merge feature/user-profile

# Resolve any conflicts
# ... resolve conflicts ...

git push origin main

# Developer B: Merge login feature  
git checkout main
git pull origin main
git merge feature/login

# May have conflicts if both touched same files
# ... resolve conflicts ...

git push origin main
```

### Real-World Merge Example

```bash
# 1. Start on main
git checkout main

# 2. Pull latest
git pull origin main

# 3. Try to merge feature
git merge feature/add-shopping-cart

# 4. If conflicts:
# Auto-merging src/app/services/cart.service.ts
# CONFLICT (content): Merge conflict in src/app/services/cart.service.ts

# 5. Resolve conflict
# Edit the file to fix

# 6. Stage the resolution
git add src/app/services/cart.service.ts

# 7. Complete merge
git commit -m "Merge feature/add-shopping-cart

Resolved conflicts in cart.service.ts"

# 8. Push
git push origin main
```

## Best Practices

### 1. Update Before Merging

```bash
# Always pull main before merging
git checkout main
git pull origin main
git merge feature-branch
```

### 2. Use Descriptive Merge Messages

```bash
# Default merge message (usually good)
git merge feature-branch
# "Merge branch 'feature-branch' into main"

# Custom message
git merge feature-branch -m "Merge login feature into main"
```

### 3. Test Before Merging

```bash
# Test before merging
ng test
# If tests pass:
git merge feature-branch
```

### 4. Don't Force Push Merge Commits

```bash
# ✗ Never do this
git push --force

# This can break team members' repositories!
```

## Common Mistakes

### Mistake 1: Not Updating Main Before Merging

```bash
# ✗ Bad: Merging without updating
git checkout main
git merge feature-branch
# May include outdated code!

# ✓ Good: Update first
git checkout main
git pull origin main
git merge feature-branch
```

### Mistake 2: Leaving Merge Conflicts Unresolved

```bash
# ✗ Bad: Don't leave conflicts
# Edit file but don't stage

# ✓ Good: Always resolve completely
git add resolved-files
git commit
```

### Mistake 3: Merging to Wrong Branch

```bash
# ✗ Oops, merged to wrong branch!
# Abort immediately
git merge --abort
```

## Exercises for Students

### Exercise 1: Fast-Forward Merge
1. Create a new branch
2. Make a commit
3. Switch to main
4. Merge (should be fast-forward)
5. Notice the linear history

### Exercise 2: Three-Way Merge
1. Make a commit on main
2. Create branch and make different commit
3. Make another commit on main
4. Merge branch - see merge commit

### Exercise 3: Resolve a Conflict
1. Create two branches from main
2. Modify same file in both branches
3. Try to merge - see conflict
4. Resolve and complete merge

## Mini Practice Tasks

### Task 1: Simple Merge
```bash
# 1. Create and work on branch
git checkout -b feature/simple
echo "feature work" > feature.txt
git add .
git commit -m "Add feature"

# 2. Switch to main
git checkout main

# 3. Merge
git merge feature/simple

# 4. Push
git push origin main
```

### Task 2: Conflict Resolution
```bash
# 1. Create branch from main
git checkout -b conflict-test

# 2. Modify a file
echo "branch version" > test.txt
git add .
git commit -m "Branch version"

# 3. Switch to main
git checkout main

# 4. Modify same file differently
echo "main version" > test.txt
git add .
git commit -m "Main version"

# 5. Try to merge - will conflict
git merge conflict-test

# 6. Resolve conflict
# Edit test.txt
echo "resolved version" > test.txt
git add test.txt
git commit -m "Resolve merge conflict"
```

### Task 3: Angular Feature Merge
```bash
# 1. Create feature branch
git checkout -b feature/dashboard

# 2. Generate Angular component
ng generate component components/dashboard

# 3. Commit
git add .
git commit -m "Add dashboard component"

# 4. Switch to main
git checkout main

# 5. Merge
git merge feature/dashboard

# 6. Push
git push origin main
```

## Summary

Merging combines branch changes:

- **`git merge branch`** - Merge a branch into current branch
- **Fast-forward** - When branches haven't diverged
- **Three-way merge** - Creates merge commit for diverged branches
- **Resolve conflicts** - Manually fix conflicting changes

Key points:
- Always update main before merging
- Test before merging
- Resolve conflicts completely
- Use meaningful commit messages

Master merging to effectively combine team work!

---

**Next Lesson**: [Git Rebasing](./12_Git_Rebasing.md)

**Previous Lesson**: [Git Branches](./10_Git_Branches.md)
