# Project 3: GitHub Project Management

## Project Overview

This project teaches GitHub project management features including repositories, pull requests, issues, and project boards. Students will learn to use GitHub for managing Angular projects.

### Learning Objectives

- Create and manage GitHub repositories
- Create and manage GitHub Issues
- Create and manage Pull Requests
- Use GitHub Projects (Kanban boards)
- Use labels and milestones

### Prerequisites

- GitHub account
- Completed Projects 1 and 2

---

## Step 1: Create GitHub Repository

### Task 1.1: Create Repository on GitHub

1. Go to github.com
2. Click "+" → "New repository"
3. Fill in details:
   - Repository name: `angular-project-manager`
   - Description: "Angular project with GitHub management"
   - Public or Private
4. Click "Create repository"

### Task 1.2: Connect Local Repository

```bash
# If you have an existing project:
git remote add origin https://github.com/your-username/angular-project-manager.git

# Push to GitHub
git branch -M main
git push -u origin main
```

---

## Step 2: Manage Issues

### Task 2.1: Create Issues

Create the following issues on GitHub:

```
Issue 1: Add user login component
- Labels: enhancement
- Assignee: (yourself)

Issue 2: Fix navigation bug
- Labels: bug
- Assignee: (yourself)

Issue 3: Add dark mode support
- Labels: enhancement
- Milestone: v1.0
```

### Task 2.2: Issue Templates

Create `.github/ISSUE_TEMPLATE/bug_report.md`:

```markdown
---
name: Bug report
about: Create a report to help us improve
title: '[Bug] '
labels: bug
assignees: ''

---

## Bug Description
Brief description

## Steps to Reproduce
1. Go to...
2. Click on...
3. See error

## Expected Behavior
What should happen

## Actual Behavior
What happens instead

## Environment
- OS:
- Angular version:
- Browser:
```

### Task 2.3: Manage Labels

Create labels:
- `bug` (red)
- `enhancement` (green)
- `documentation` (yellow)
- `priority: high` (orange)
- `good first issue` (green)

---

## Step 3: Create Feature Branch and PR

### Task 3.1: Create Branch from Issue

```bash
# Create branch for issue
git checkout -b feature/add-login
```

### Task 3.2: Make Changes

```bash
# Generate login component
ng generate component components/login

# Make changes
# Edit files...

# Commit
git add .
git commit -m "feat(auth): add login component

Addresses issue #1"
```

### Task 3.3: Push and Create PR

```bash
# Push branch
git push -u origin feature/add-login
```

On GitHub:
1. Click "Compare & pull request"
2. Fill in PR details:
   - Title: "feat: Add user login component"
   - Description: "This PR adds a login component"
   - Linked issues: #1
3. Click "Create pull request"

---

## Step 4: Use GitHub Projects

### Task 4.1: Create Project Board

1. Go to "Projects" → "Create project"
2. Name: "Angular Development"
3. Add columns:
   - To Do
   - In Progress
   - In Review
   - Done

### Task 4.2: Add Issues to Project

1. Add created issues to project
2. Move issues between columns
3. Track progress visually

### Task 4.3: Link PRs to Project

Link pull requests to project for tracking.

---

## Step 5: Complete Workflow

### Task 5.1: Review Pull Request

On GitHub PR page:
- Review changes
- Add comments
- Request changes or approve

### Task 5.2: Merge Pull Request

1. After approval, click "Merge pull request"
2. Add commit message
3. Click "Confirm merge"

### Task 5.3: Cleanup

```bash
# Switch to main
git checkout main

# Pull merged changes
git pull origin main

# Delete feature branch
git branch -d feature/add-login
git push origin --delete feature/add-login
```

---

## Step 6: Use Milestones

### Task 6.1: Create Milestone

1. Go to "Issues" → "Milestones"
2. Create milestone "v1.0"
3. Set due date

### Task 6.2: Add Issues to Milestone

1. Open issue
2. Add "v1.0" milestone

### Task 6.3: Track Progress

- View milestone progress
- See remaining issues

---

## Step 7: Practice Code Review

### Task 7.1: Create Another PR

Create another feature and PR:
```bash
git checkout -b feature/add-dashboard
ng generate component components/dashboard
# make changes
git add .
git commit -m "feat(dashboard): add dashboard component"
git push -u origin feature/add-dashboard
# Create PR on GitHub
```

### Task 7.2: Review on GitHub

Review the PR on GitHub:
- Check files changed
- Add line comments
- Suggest improvements

### Task 7.3: Address Review

Make additional commits if needed:
```bash
# Make changes
git add .
git commit -m "fix: address review comments"
git push
```

---

## Exercise Requirements

### Exercise 1: Complete Issue Workflow

- [ ] Create at least 3 issues
- [ ] Add labels
- [ ] Create feature branches
- [ ] Create PRs
- [ ] Merge PRs

### Exercise 2: Use Project Board

- [ ] Create project board
- [ ] Add issues to board
- [ ] Move issues through columns

### Exercise 3: Track with Milestones

- [ ] Create milestone
- [ ] Add issues to milestone
- [ ] Close issues

---

## Assessment Criteria

| Criteria | Points |
|----------|--------|
| Repository created | 10 |
| Issues created and managed | 15 |
| Feature branches and PRs | 20 |
| Project board used | 15 |
| Milestones used | 10 |
| Code review process | 15 |
| Cleanup completed | 15 |

---

## GitHub Best Practices

### Issue Best Practices

1. Clear, descriptive titles
2. Detailed descriptions
3. Use labels for categorization
4. Assign to team members
5. Link to PRs

### PR Best Practices

1. Descriptive title and description
2. Link related issues
3. Keep changes focused
4. Respond to review comments
5. Clean up branches after merge

---

## Summary

This project teaches:

- GitHub repository management
- Issue tracking and management
- Pull request workflow
- Project boards (Kanban)
- Labels and milestones
- Code review process

You now know how to manage Angular projects on GitHub!

---

**Next Project**: [Angular CI/CD with GitHub](./Project_04_Angular_CI_CD_with_GitHub.md)

**Previous Project**: [Angular Team Collaboration](./Project_02_Angular_Team_Collaboration.md)
