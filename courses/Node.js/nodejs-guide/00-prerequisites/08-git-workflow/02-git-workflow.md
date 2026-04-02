# Git Workflow Strategies

## What You'll Learn

- Branching strategies for Node.js projects
- Git flow and GitHub flow
- Pull request workflows
- Collaboration best practices

## Branching Strategies

### Git Flow

```bash
# Main branches
main        # Production-ready code
develop     # Integration branch

# Supporting branches
feature/*   # New features
release/*   # Release preparation
hotfix/*    # Production fixes

# Example workflow
git checkout develop
git checkout -b feature/user-auth
# ... work on feature
git checkout develop
git merge feature/user-auth
git branch -d feature/user-auth
```

### GitHub Flow

```bash
# Simple workflow
main        # Always deployable

# Feature branches
feature/*   # Short-lived branches

# Example workflow
git checkout main
git pull origin main
git checkout -b feature/add-user
# ... work on feature
git push origin feature/add-user
# Create pull request
# After review, merge to main
```

### Trunk-Based Development

```bash
# Single main branch
main        # Trunk

# Short-lived feature branches
feature/*   # < 1 day

# Example workflow
git checkout main
git pull origin main
git checkout -b quick-fix
# ... quick changes
git checkout main
git merge quick-fix
git push origin main
```

## Feature Branch Workflow

### Creating Feature Branches

```bash
# Start from updated main
git checkout main
git pull origin main

# Create feature branch
git checkout -b feature/user-authentication

# Or using modern switch
git switch -c feature/user-authentication

# Push to remote
git push -u origin feature/user-authentication
```

### Working on Feature

```bash
# Make changes
# Stage changes
git add .

# Commit with conventional message
git commit -m "feat: add user authentication"

# Push changes
git push origin feature/user-authentication

# Keep branch updated with main
git fetch origin
git rebase origin/main
```

### Completing Feature

```bash
# Final commit
git commit -m "feat: complete user authentication"

# Push final changes
git push origin feature/user-authentication

# Create pull request on GitHub
# After merge, delete branch
git checkout main
git pull origin main
git branch -d feature/user-authentication
git push origin --delete feature/user-authentication
```

## Pull Request Workflow

### Creating Pull Requests

```bash
# Push feature branch
git push origin feature/user-authentication

# On GitHub:
# 1. Click "Compare & pull request"
# 2. Fill in title and description
# 3. Request reviewers
# 4. Add labels and milestones
```

### PR Best Practices

```markdown
## Title
feat: add user authentication system

## Description
### What
- Add JWT-based authentication
- Implement login/logout endpoints
- Add password hashing

### Why
- Secure user sessions
- Protect API endpoints
- Follow security best practices

### How
- Used bcrypt for password hashing
- Implemented JWT token generation
- Added authentication middleware

### Testing
- Added unit tests for auth functions
- Added integration tests for endpoints
- Tested manually with Postman
```

### Code Review Process

```bash
# Reviewer: Fetch PR branch
git fetch origin
git checkout feature/user-authentication

# Review code locally
# Make comments on GitHub

# Request changes if needed
# Approve when satisfied
```

## Merge Strategies

### Merge Commit

```bash
# Create merge commit
git checkout main
git merge --no-ff feature/user-authentication

# Preserves branch history
# Creates explicit merge commit
```

### Squash and Merge

```bash
# Squash all commits into one
git checkout main
git merge --squash feature/user-authentication
git commit -m "feat: add user authentication"

# Clean history
# Single commit per feature
```

### Rebase and Merge

```bash
# Rebase feature onto main
git checkout feature/user-authentication
git rebase main

# Fast-forward merge
git checkout main
git merge feature/user-authentication

# Linear history
# No merge commits
```

## Handling Conflicts

### Conflict Resolution

```bash
# Conflict during merge
git merge feature/user-authentication
# CONFLICT (content): Merge conflict in file.js

# See conflicted files
git status

# Edit files to resolve conflicts
<<<<<<< HEAD
// Current changes
=======
// Incoming changes
>>>>>>> feature/user-authentication

# After resolving
git add resolved-file.js
git commit
```

### Conflict Prevention

```bash
# Keep branches short-lived
# Pull/rebase frequently
git fetch origin
git rebase origin/main

# Communicate with team
# Use feature flags for large changes
```

## Git Hooks for Workflow

### Pre-commit Hook

```bash
#!/bin/sh
# .husky/pre-commit

# Run linting
npm run lint

# Run tests
npm test

# Check commit message format
# (using commitlint)
```

### Commit Message Hook

```bash
#!/bin/sh
# .husky/commit-msg

# Validate commit message format
npx --no -- commitlint --edit ${1}
```

### Pre-push Hook

```bash
#!/bin/sh
# .husky/pre-push

# Run full test suite
npm run test:ci

# Check for secrets
# (using git-secrets)
```

## Conventional Commits

### Commit Message Format

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

### Commit Types

```bash
feat:     # New feature
fix:      # Bug fix
docs:     # Documentation
style:    # Formatting, missing semicolons, etc.
refactor: # Code restructuring
test:     # Adding tests
chore:    # Maintenance tasks
perf:     # Performance improvements
ci:       # CI/CD changes
build:    # Build system changes
```

### Examples

```bash
git commit -m "feat(auth): add JWT authentication"
git commit -m "fix(api): handle null user response"
git commit -m "docs(readme): update installation steps"
git commit -m "test(auth): add login endpoint tests"
git commit -m "refactor(utils): extract validation logic"
```

## Release Workflow

### Semantic Versioning

```bash
# Version format: MAJOR.MINOR.PATCH
# MAJOR: Breaking changes
# MINOR: New features
# PATCH: Bug fixes

# Bump version
npm version patch   # 1.0.0 → 1.0.1
npm version minor   # 1.0.0 → 1.1.0
npm version major   # 1.0.0 → 2.0.0
```

### Release Process

```bash
# Create release branch
git checkout develop
git checkout -b release/1.2.0

# Final testing and fixes
# Bump version
npm version minor

# Merge to main
git checkout main
git merge --no-ff release/1.2.0
git tag -a v1.2.0 -m "Release version 1.2.0"

# Merge back to develop
git checkout develop
git merge --no-ff release/1.2.0

# Delete release branch
git branch -d release/1.2.0

# Push everything
git push origin main develop --tags
```

## Hotfix Workflow

### Production Hotfix

```bash
# Create hotfix from main
git checkout main
git checkout -b hotfix/fix-login

# Fix the issue
# Test thoroughly
git commit -m "fix(auth): resolve login timeout issue"

# Merge to main
git checkout main
git merge --no-ff hotfix/fix-login
git tag -a v1.2.1 -m "Hotfix: login timeout"

# Merge to develop
git checkout develop
git merge --no-ff hotfix/fix-login

# Delete hotfix branch
git branch -d hotfix/fix-login

# Push
git push origin main develop --tags
```

## Collaboration Best Practices

### Communication

```bash
# Use descriptive branch names
feature/user-authentication
bugfix/login-error
hotfix/security-patch

# Write clear commit messages
feat(auth): add password reset functionality

# Keep PRs small and focused
# Request reviews from relevant team members
# Respond to review comments promptly
```

### Code Review Guidelines

```markdown
## For Authors
- Keep PRs small (< 400 lines)
- Write clear descriptions
- Add tests for new features
- Respond to feedback constructively

## For Reviewers
- Review within 24 hours
- Be constructive and specific
- Test the changes locally
- Approve when satisfied
```

## Troubleshooting Common Issues

### Lost Commits

```bash
# Problem: Lost commit after rebase
# Solution: Use reflog

git reflog
git checkout <commit-hash>
git branch recovery-branch

# Or reset to previous state
git reset --hard HEAD@{1}
```

### Wrong Branch

```bash
# Problem: Committed to wrong branch
# Solution: Move commits

# Save current commits
git log --oneline -5

# Switch to correct branch
git checkout correct-branch

# Cherry-pick commits
git cherry-pick <commit-hash>

# Remove from wrong branch
git checkout wrong-branch
git reset --hard HEAD~1
```

### Push Rejected

```bash
# Problem: Push rejected (non-fast-forward)
# Solution: Pull and rebase

git pull --rebase origin main
git push origin main

# Or force push (dangerous)
git push --force-with-lease origin main
```

## Best Practices Checklist

- [ ] Choose appropriate branching strategy
- [ ] Keep branches short-lived
- [ ] Write conventional commit messages
- [ ] Review code before merging
- [ ] Resolve conflicts promptly
- [ ] Delete merged branches
- [ ] Tag releases appropriately
- [ ] Document workflow in README
- [ ] Set up Git hooks for quality
- [ ] Communicate with team members

## Performance Optimization Tips

- Use shallow clones for CI/CD
- Enable Git garbage collection
- Use Git LFS for large files
- Configure Git to cache credentials
- Use sparse checkout for monorepos
- Clean up old branches regularly
- Use Git worktrees for parallel work

## Cross-References

- See [Git Installation](./01-git-installation-config.md) for Git setup
- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for pre-commit hooks
- See [Testing Environment](../06-testing-environment/) for test automation
- See [Development Tools](../12-dev-tools-integration/) for Git integration

## Next Steps

Now that Git workflow is understood, let's set up debugging. Continue to [Debugging Environment Setup](../09-debugging-setup/).