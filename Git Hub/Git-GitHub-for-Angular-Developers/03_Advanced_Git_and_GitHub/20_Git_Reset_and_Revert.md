# Git Reset and Revert

## Topic Title
Undoing Changes Safely with Git Reset and Revert

## Concept Explanation

Sometimes we make mistakes in Git and need to undo changes. Git provides two main ways to undo changes: `git reset` and `git revert`. Understanding when to use each is crucial for maintaining a clean project history.

### What is Reset?

Reset moves the HEAD and branch pointer back to a previous commit, effectively "unmaking" commits. It changes the commit history.

### What is Revert?

Revert creates a new commit that undoes previous changes. It doesn't change history but adds new commits that reverse the changes.

### When to Use Each

| Reset | Revert |
|-------|--------|
| Local commits only | Commits already pushed |
| Want to change history | Need safe, non-destructive undo |
| Commits not shared | Shared/public commits |

## The `git reset` Command

### Understanding Reset Modes

```bash
# Soft reset - keep changes staged
git reset --soft HEAD~1

# Mixed reset - keep changes unstaged (default)
git reset --mixed HEAD~1

# Hard reset - discard all changes
git reset --hard HEAD~1
```

### Visual Explanation

```
Before Reset (HEAD~2):
A---B---C---D (HEAD points to D)

After git reset --soft HEAD~2:
A---B---C---D (HEAD)
         staged: changes from C, D

After git reset --mixed HEAD~2:
A---B---C---D (HEAD)
         unstaged: changes from C, D

After git reset --hard HEAD~2:
A---B (HEAD)
         changes from C, D are LOST
```

### Reset Examples

```bash
# Reset to previous commit (keep changes staged)
git reset --soft HEAD~1

# Reset to previous commit (keep changes in working dir)
git reset HEAD~1

# Reset to previous commit (discard changes)
git reset --hard HEAD~1

# Reset to specific commit
git reset abc1234
git reset --hard abc1234
```

## The `git revert` Command

### Basic Revert

```bash
# Revert last commit
git revert HEAD

# Revert specific commit
git revert abc1234

# Revert without auto-commit
git revert -n HEAD
git revert -n abc1234
# Then commit manually
```

### Visual Explanation

```
Before Revert:
A---B---C---D (HEAD)

After git revert HEAD:
A---B---C---D---D' (HEAD)
                (D' undoes D's changes)
```

### Revert Examples

```bash
# Revert last commit
git revert HEAD

# Revert commit with message
git revert HEAD -m "Revert feature - caused bugs"

# Revert merge commit
git revert -m 1 merge-commit-hash

# Multiple commits
git revert HEAD~3..HEAD
```

## Reset vs Revert Comparison

### Reset (local changes)

```bash
# Scenario: Committed but not pushed
# Working on feature, made mistakes

git log --oneline
# abc1234 Add feature
# def5678 Previous work
# ghi9012 Initial

# Reset to before mistakes
git reset --hard ghi9012

# Result: History changed
# abc1234 Add feature - GONE
# def5678 Previous work - GONE
# ghi9012 Initial - NOW HEAD
```

### Revert (shared commits)

```bash
# Scenario: Committed and pushed
# Need to undo but can't change history

git log --oneline
# abc1234 Add feature (pushed!)
# def5678 Previous work

# Revert the bad commit
git revert abc1234

# Result: New commit added
# hij3456 Revert "Add feature"
# abc1234 Add feature
# def5678 Previous work
```

## Angular Example Scenarios

### Example 1: Undo Local Commits

```bash
# Made mistakes in local commits
git log --oneline
# a1b2c3d Add user validation (mistake)
# d4e5f6g Add user service
# g7h8i9j Initial commit

# Undo last commit, keep changes
git reset --soft HEAD~1

# Now changes are staged
git status
# Changes to be committed:
# - Add user validation

# Amend or recommit properly
git commit -m "Add user validation properly"
```

### Example 2: Revert Pushed Commit

```bash
# Bad commit was pushed to main
git log --oneline
# xyz7890 Feature that broke production (pushed!)
# abc1234 Previous good commit

# Revert safely
git revert xyz7890

# New commit created
git log --oneline
# 0123456 Revert "Feature that broke production"
# xyz7890 Feature that broke production
# abc1234 Previous good commit

# Push the revert
git push origin main
```

### Example 3: Reset Before Push

```bash
# Want to clean up commits before pushing
git log --oneline
# abc123 WIP - don't push this
# def456 WIP - don't push this
# ghi789 Good commit

# Reset to good commit
git reset --hard ghi789

# Make clean commits
git add .
git commit -m "Clean feature implementation"

# Now push
git push origin main
```

## Safe Undoing Rules

### Golden Rule: Don't Reset Public History

```bash
# ✗ NEVER do this with pushed commits
git reset --hard HEAD~1
git push --force

# This changes shared history!
# Teammates will have problems

# ✓ Safe: Revert instead
git revert HEAD~1
git push
```

### When to Use Reset

- Commits not pushed yet
- Working on local branch
- Need to change/redo commits

### When to Use Revert

- Commits already pushed
- Need to undo in shared history
- Need audit trail of changes

## Exercises for Students

### Exercise 1: Soft Reset
1. Make several commits
2. Use soft reset to undo
3. See changes are staged
4. Make new commit

### Exercise 2: Hard Reset
1. Make commits
2. Use hard reset
3. See changes are lost
4. Use reflog to recover (advanced)

### Exercise 3: Revert
1. Push a commit
2. Revert it
3. See both commits in history

## Summary

Both reset and revert undo changes:

- **`git reset`** - Move HEAD back, change history
  - `--soft`: Keep staged
  - `--mixed`: Keep unstaged (default)
  - `--hard`: Discard changes
- **`git revert`** - Create new commit that undoes changes
  - Safe for shared commits
  - Preserves history

Key points:
- Use reset for local, un-pushed commits
- Use revert for shared, pushed commits
- Never force push to shared branches

Undo changes safely to maintain clean history!

---

**Next Lesson**: [Git Tags](./21_Git_Tags.md)

**Previous Lesson**: [Git Stashing](./19_Git_Stashing.md)
