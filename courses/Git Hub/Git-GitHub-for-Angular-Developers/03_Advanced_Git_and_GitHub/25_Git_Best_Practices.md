# Git Best Practices

## Topic Title
Professional Git Habits for Angular Developers

## Concept Explanation

Following Git best practices ensures clean, maintainable project history and smooth collaboration. These habits separate professional developers from beginners.

### Why Best Practices Matter

1. **Clean history**: Easy to understand project evolution
2. **Collaboration**: Team members can follow along
3. **Debugging**: Can trace bugs to their source
4. **Maintenance**: Long-term project health

## Commit Best Practices

### Write Meaningful Messages

```bash
# ✓ Good: Specific and clear
git commit -m "feat(auth): add login form validation"

git commit -m "fix: resolve navigation bug on mobile

The navigation menu didn't close after clicking a link
on mobile devices due to missing click handler.

Fixes #123"

# ✗ Bad: Vague
git commit -m "fix"
git commit -m "updates"
git commit -m "changes"
```

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- feat: New feature
- fix: Bug fix
- docs: Documentation
- style: Formatting
- refactor: Code restructuring
- test: Adding tests
- chore: Maintenance

**Examples:**
```bash
git commit -m "feat(dashboard): add user statistics component"

git commit -m "fix(auth): resolve token refresh issue

The token refresh was failing silently when the refresh
token was expired. Added error handling to redirect
users to login.

Closes #456"

git commit -m "docs(readme): update installation steps"
```

### Make Small, Focused Commits

```bash
# ✓ Good: One feature per commit
git commit -m "Add button component"
git commit -m "Style button component"
git commit -m "Add button tests"

# ✗ Bad: Multiple unrelated changes
git commit -m "Add login, fix sidebar, update docs"
```

### Commit Frequently

```bash
# ✓ Good: Commit often
# After each logical change
git commit -m "feat: add user model"
git commit -m "feat: add user service"
git commit -m "feat: add user component"

# ✗ Bad: Rare, huge commits
# Months of work in one commit
```

## Branch Best Practices

### Use Descriptive Names

```bash
# ✓ Good
feature/add-user-dashboard
feature/user-authentication
fix/login-validation-bug
hotfix/security-patch
release/v1.0.0

# ✗ Bad
feature
fix
my-work
```

### Keep Branches Short-Lived

```bash
# ✓ Good: Short branches
# Create, work, merge within days
git checkout -b feature/quick-fix
# ... work ...
git checkout main
git merge feature/quick-fix

# ✗ Bad: Long-lived branches
# Months of work without merging
```

### Use Branches for Everything

```bash
# ✓ Good: All work on branches
git checkout -b feature/new-login
# make commits
git push -u origin feature/new-login

# ✗ Bad: Work directly on main
git checkout main
# edit directly
git commit -m "changes"
```

## .gitignore Best Practices

### Include Essential Files

```gitignore
# Dependencies
node_modules/

# Build output
dist/
.angular/

# IDE
.vscode/
.idea/

# Environment
.env

# OS
.DS_Store
Thumbs.db

# Logs
*.log

# Testing
coverage/
```

### Add Early

```bash
# Create before first commit
touch .gitignore
echo "node_modules/" > .gitignore
echo "dist/" >> .gitignore
git add .gitignore
git commit -m "Add .gitignore"
```

## Workflow Best Practices

### Always Pull Before Push

```bash
# ✓ Good: Pull first
git pull origin main
git push

# ✗ Bad: Push without pulling
git push
# May fail with conflicts!
```

### Test Before Merging

```bash
# Always test before merging
ng test
ng build

# Then merge
git checkout main
git merge feature-branch
```

### Use Protected Branches

On GitHub:
```
Settings → Branches → Add rule
- Require pull request reviews
- Require status checks
- Require conversation resolution
```

## Collaboration Best Practices

### Review Before Committing

```bash
# Check what you're committing
git status
git diff --cached
```

### Communicate Changes

- Use issues for tracking work
- Discuss in pull requests
- Update team on progress

### Respond to Reviews

- Address feedback promptly
- Ask questions if unclear
- Don't take feedback personally

## Angular-Specific Practices

### Commit Generated Files Carefully

```bash
# Generated files can be committed
# But be careful about what you change manually

# Angular generates these:
src/app/app.component.ts
src/app/app.component.html
src/app/app.component.css
src/app/app.module.ts

# Track them, but don't manually edit
# Let Angular CLI manage them
```

### Build Before Committing

```bash
# Always build before pushing
ng build --configuration production
# Ensure no build errors

# Run tests
ng test
# Ensure tests pass
```

### Version Control Angular Projects

```bash
# Commit structure:
# 1. Initial setup
# 2. Feature by feature
# 3. Bug fixes
# 4. Configuration changes

# Track package.json changes
# These define your dependencies
```

## Common Mistakes to Avoid

### 1. Committing Secrets

```bash
# ✗ Never commit secrets
git add .env
git commit -m "Add env"

# ✓ Use .gitignore
echo ".env" >> .gitignore
```

### 2. Pushing Too Early

```bash
# ✗ Don't push unfinished work
# Share when ready for review

# ✓ Push when ready for PR
```

### 3. Ignoring Conflicts

```bash
# ✗ Don't ignore conflicts
# Resolve them properly

# ✓ Resolve conflicts
# Test after resolving
```

### 4. Not Using Branches

```bash
# ✗ Don't work on main
# Keep main deployable

# ✓ Use feature branches
```

## Summary

Follow these best practices:

- **Commits**: Clear messages, small, frequent
- **Branches**: Descriptive names, short-lived
- **.gitignore**: Include essential files
- **Collaboration**: Test, review, communicate
- **Security**: Never commit secrets

These habits make you a professional developer!

---

**Next Section**: [Project Practice](./../04_Git_GitHub_Project_Practice/Project_01_Version_Control_Angular_Project.md)

**Previous Lesson**: [Managing Open Source Projects](./24_Managing_Open_Source_Projects.md)
