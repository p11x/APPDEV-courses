# Pull Requests

## Topic Title
Collaborating Through GitHub Pull Requests

## Concept Explanation

Pull Requests (PRs) are the foundation of collaboration on GitHub. They allow you to propose changes, request reviews, discuss modifications, and merge code into the main codebase. Every significant change in professional Angular projects goes through a Pull Request.

### What is a Pull Request?

A Pull Request is a request to merge changes from one branch to another. It provides:
- A place to discuss changes
- Code review process
- Automated checks/tests
- History of the change

### Why Use Pull Requests?

1. **Code Review**: Catch bugs before merging
2. **Discussion**: Team can comment on changes
3. **Quality Control**: Automated tests must pass
4. **Documentation**: Changes are documented
5. **Traceability**: Complete audit trail

## Creating a Pull Request

### Step-by-Step: From Local Branch

```bash
# 1. Create and checkout branch
git checkout -b feature/add-login

# 2. Make changes
# ... edit Angular files ...

# 3. Commit changes
git add .
git commit -m "feat(auth): add login component"

# 4. Push branch to GitHub
git push -u origin feature/add-login
```

### Step-by-Step: On GitHub

1. **Navigate to repository**
2. **Click "Compare & pull request"** (shown after push)
   Or go to "Pull requests" → "New pull request"

3. **Configure PR**:
   ```
   base: main          ← Where to merge
   compare: feature/add-login  ← Your branch
   ```

4. **Add Details**:
   ```
   Title: Add login component
   
   Description:
   - Created LoginComponent
   - Added form validation
   - Implemented JWT handling
   - Added unit tests
   
   Fixes #123
   ```

5. **Click "Create pull request"**

## Pull Request Features

### Description

Use markdown in PR description:

```markdown
## Summary
Brief description of changes

## Changes
- Added LoginComponent
- Added AuthService
- Added login form validation

## Testing
- [ ] Unit tests pass
- [ ] Tested locally

## Screenshots
[optional: add screenshots]

## Checklist
- [x] Code follows style guide
- [x] Tests added
- [ ] Documentation updated
```

### Reviewers

Request reviews:
- Click "Reviewers" on PR
- Select team members
- They get notified

### Labels

Add labels for categorization:
- `bug`, `feature`, `enhancement`
- `needs-review`, `in-progress`
- `high-priority`

### Milestones

Group related PRs:
- Link to milestone
- Track release progress

## Code Review Process

### For Reviewers

```bash
# 1. View changes on GitHub
# Or checkout PR locally

# Checkout PR to test locally
git fetch origin
git checkout -b pr-123 origin/pr/123

# Make comments on lines
# Request changes or approve

# Submit review
```

### Review Checklist

- [ ] Code follows project style
- [ ] Logic is correct
- [ ] Error handling included
- [ ] Tests are adequate
- [ ] Documentation updated
- [ ] No security issues
- [ ] Performance acceptable

### Types of Reviews

```
COMMENT - General feedback
APPROVE - Ready to merge
REQUEST CHANGES - Needs work
```

## Angular Team Workflow Example

### Scenario: Adding New Feature

```
Developer A: Working on user dashboard
```

```bash
# 1. Create feature branch
git checkout main
git pull
git checkout -b feature/user-dashboard
```

```bash
# 2. Generate Angular component
ng generate component components/user-dashboard

# 3. Generate service
ng generate service services/user-data
```

```bash
# 4. Make changes, commit
git add .
git commit -m "feat(dashboard): add user dashboard

- Created UserDashboardComponent
- Added UserDataService
- Implemented data fetching
- Added loading states"
```

```bash
# 5. Push and create PR
git push -u origin feature/user-dashboard
```

### On GitHub

1. Review changes in diff
2. Add reviewers
3. Add labels
4. Create PR

### After Review

```bash
# If changes requested:
# Make additional commits
git add .
git commit -m "fix: address review comments"
git push

# If approved:
# Merge on GitHub
# Or merge locally
git checkout main
git merge feature/user-dashboard
git push origin main
```

## Pull Request Best Practices

### 1. Keep PRs Small

```bash
# ✓ Good: One feature per PR
# PR: Add login form
# Separate PR: Add registration form

# ✗ Bad: Multiple unrelated changes
# "Add login, fix bug, update docs"
```

### 2. Write Good Descriptions

```markdown
## What
Brief description

## Why
Why this change is needed

## How
How it works

## Testing
How to test
```

### 3. Respond to Reviews

```bash
# Address comments promptly
# Ask questions if unclear
# Don't take feedback personally
```

### 4. Keep PRs Updated

```bash
# Rebase if main advanced
git fetch origin
git rebase origin/main
git push --force-with-lease
```

## Closing Pull Requests

### Merge

Click "Merge pull request" when
- Tests pass
- No conflicts

### Close

Close without merging if:
- Approved:
- Won't be merged
- Duplicate
- Not needed anymore

### Draft PRs

Create as draft if:
- Work in progress
- Don't want review yet

```bash
# Create draft PR from CLI
gh pr create --draft
```

## Exercises for Students

### Exercise 1: Create Your First PR
1. Create a branch
2. Make a small change
3. Push and create PR
4. Review the PR interface

### Exercise 2: Review a PR
1. View a classmate's PR
2. Leave comments
3. Suggest changes

### Exercise 3: Merge a PR
1. After approval, merge
2. Delete branch locally
3. Pull main to update

## Summary

Pull Requests enable collaboration:

- **Create**: From pushed branch
- **Review**: Request feedback
- **Discuss**: Comment on changes
- **Test**: Automated checks
- **Merge**: Integrate changes

Key workflow:
1. Create branch
2. Make changes
3. Push and create PR
4. Address reviews
5. Merge after approval

Master PRs for team collaboration!

---

**Next Lesson**: [GitHub Issues](./18_GitHub_Issues.md)

**Previous Lesson**: [GitHub Clone](./16_GitHub_Clone.md)
