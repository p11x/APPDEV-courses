# Git Tags

## Topic Title
Marking Important Points with Git Tags

## Concept Explanation

Git tags are references that point to specific points in Git history. They are most commonly used to mark release versions (v1.0, v2.0.1) but can also be used for any important point in the project timeline.

### What are Tags?

Tags are like branches that don't change. They mark a specific commit as important, typically for releases.

### Why Use Tags?

1. **Mark releases**: Version numbers for your software
2. **Reference points**: Important milestones
3. **Rollback points**: Easy to find old versions
4. **Documentation**: Mark significant changes

## Types of Tags

### Annotated Tags

```bash
# Include tagger info, message, and GPG signature
git tag -a v1.0.0 -m "Release version 1.0.0"
```

Include:
- Tagger name and email
- Date
- Message
- Optional GPG signature

### Lightweight Tags

```bash
# Simple pointer to a commit
git tag v1.0.0
```

Just a name pointing to a commit.

## Creating Tags

### Basic Tag Creation

```bash
# Create annotated tag
git tag -a v1.0.0 -m "Initial release"

# Create lightweight tag
git tag v1.0.0

# Tag specific commit
git tag -a v0.9.0 abc1234 -m "Beta release"
```

### Tag Naming Conventions

```bash
# Semantic versioning is common
v1.0.0        # Major.Minor.Patch
v1.0.0-rc1    # Release candidate
v1.0.0-beta   # Beta
v1.0.0-alpha  # Alpha

# Angular versions are examples
v17.0.0
v17.1.0
v17.2.0-next.1
```

## Managing Tags

### Listing Tags

```bash
# List all tags
git tag

# List with details
git tag -l -n

# List tags matching pattern
git tag -l "v1.*"
git tag -l "v17*"
```

### Viewing Tag Details

```bash
# Show tag details
git show v1.0.0

# Show tag info
git tag -v v1.0.0
```

### Deleting Tags

```bash
# Delete local tag
git tag -d v1.0.0

# Delete remote tag
git push origin --delete v1.0.0
git push origin :refs/tags/v1.0.0
```

## Sharing Tags

### Push Tags to Remote

```bash
# Push specific tag
git push origin v1.0.0

# Push all tags
git push --tags
git push origin --tags
```

### Fetch Tags

```bash
# Fetch tags
git fetch --tags

# Fetch all
git fetch --all
```

## Angular Release Example

### Tagging a Release

```bash
# 1. Ensure on main branch
git checkout main

# 2. Pull latest
git pull

# 3. Build for production
ng build --configuration production

# 4. Tag the release
git tag -a v1.0.0 -m "Release v1.0.0

Features:
- User dashboard
- Login system

Bug fixes:
- Fixed navigation bug"

# 5. Push tag
git push origin v1.0.0
```

### Checking Out Tags

```bash
# View tag (read-only)
git checkout v1.0.0

# Create branch from tag
git checkout -b release-branch v1.0.0
```

## GitHub and Tags

### View Tags on GitHub

- Tags appear in repository
- Can browse by tag
- Download specific versions

### Release from Tags

```bash
# On GitHub:
# 1. Go to releases
# 2. Click "Draft a new release"
# 3. Select tag
# 4. Add release notes
# 5. Publish
```

### GitHub CLI

```bash
# Create release from tag
gh release create v1.0.0 --title "Version 1.0" --notes "Release notes"
```

## Best Practices

### 1. Use Annotated Tags

```bash
# ✓ Good: Annotated for releases
git tag -a v1.0.0 -m "Release v1.0.0"

# Lightweight is fine for local/temporary
git tag my-temp-tag
```

### 2. Follow Versioning

```bash
# Use semantic versioning
v1.0.0  # Major - breaking changes
v1.1.0  # Minor - new features
v1.1.1  # Patch - bug fixes
```

### 3. Tag After Merging to Main

```bash
# Tag on main after merge
git checkout main
git merge feature-branch
git tag -a v1.0.0 -m "Release"
git push origin v1.0.0
```

### 4. Include Release Notes

```bash
# Detailed tag message
git tag -a v1.0.0 -m "Release v1.0.0

New Features:
- User dashboard
- Login functionality

Bug Fixes:
- Fixed navigation issue
- Fixed form validation

Breaking Changes:
- Removed deprecated API"
```

## Common Mistakes

### Mistake 1: Forgetting to Push Tags

```bash
# Tags aren't pushed automatically
git push          # Doesn't push tags
git push --tags   # Push all tags
```

### Mistake 2: Tagging Wrong Commit

```bash
# Check current commit
git log --oneline

# Tag correct commit
git tag -a v1.0.0 abc1234 -m "Release"
```

### Mistake 3: Deleting Tags Incorrectly

```bash
# Delete both local and remote
git tag -d v1.0.0                  # Local
git push origin --delete v1.0.0   # Remote
```

## Exercises for Students

### Exercise 1: Create Tags
1. Make a commit
2. Create annotated tag
3. View tag details
4. Push tag to remote

### Exercise 2: Work with Multiple Tags
1. Create multiple versions (v0.1, v0.2, v1.0)
2. List all tags
3. Checkout different tags

### Exercise 3: Release Workflow
1. Complete a feature
2. Merge to main
3. Tag the release
4. Push tag

## Summary

Git tags mark important points in history:

- **`git tag`** - Create tags
- **`git tag -a`** - Annotated tags (recommended)
- **`git tag -l`** - List tags
- **`git push --tags`** - Push tags to remote
- **`git tag -d`** - Delete tags

Key uses:
- Version releases (v1.0.0)
- Milestones
- Reference points

Use tags for meaningful points in your Angular project!

---

**Next Lesson**: [GitHub Actions](./22_GitHub_Actions.md)

**Previous Lesson**: [Git Reset and Revert](./20_Git_Reset_and_Revert.md)
