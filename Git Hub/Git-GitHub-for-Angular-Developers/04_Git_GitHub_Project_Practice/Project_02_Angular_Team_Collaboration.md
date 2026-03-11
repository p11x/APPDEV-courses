# Project 2: Angular Team Collaboration

## Project Overview

This project simulates a team collaboration workflow using Git branches, merging, and conflict resolution. Students will work as a team to develop an Angular application.

### Learning Objectives

- Create feature branches
- Merge branches with and without conflicts
- Resolve merge conflicts
- Use proper branch naming conventions
- Follow team workflow practices

### Prerequisites

- Completed Project 1
- Basic understanding of branches

---

## Setup: Prepare the Project

### Step 1: Create Team Repository

```bash
# One team member creates the repository
ng new angular-team-app
cd angular-team-app

# Initialize Git
git init

# Configure Git
git config user.name "Team Member"
git config user.email "team@example.com"

# Create .gitignore
touch .gitignore
echo "node_modules/
dist/
.angular/" > .gitignore

# Initial commit
git add .
git commit -m "Initial Angular project setup"
```

---

## Team Workflow: Branch-Based Development

### Step 2: Create Feature Branches

Each team member creates their own feature branch:

```bash
# Team Member 1: Feature branch for Header
git checkout -b feature/header-component

# Team Member 2: Feature branch for Footer  
git checkout -b feature/footer-component
```

---

## Step 3: Develop Features Independently

### Task 3.1: Team Member 1 - Header Component

```bash
# Switch to header branch
git checkout feature/header-component

# Generate header component
ng generate component components/header

# Add header styles and template
# Edit src/app/components/header/header.component.html
# Edit src/app/components/header/header.component.css

# Commit changes
git add .
git commit -m "feat(header): add header component

- Created HeaderComponent
- Added navigation links
- Added basic styling"
```

### Task 3.2: Team Member 2 - Footer Component

```bash
# Switch to footer branch
git checkout feature/footer-component

# Generate footer component
ng generate component components/footer

# Add footer content
# Edit src/app/components/footer/footer.component.html

# Commit changes
git add .
git commit -m "feat(footer): add footer component

- Created FooterComponent
- Added copyright text"
```

---

## Step 4: Merge Feature Branches

### Task 4.1: Update Main Branch

```bash
# Switch to main
git checkout main

# Pull latest (if remote exists)
git pull origin main

# Or just commit to local main
```

### Task 4.2: Merge Header Feature

```bash
# Merge header component
git merge feature/header-component

# View log
git log --oneline --graph
```

### Task 4.3: Merge Footer Feature

```bash
# Merge footer component
git merge feature/footer-component

# View log
git log --oneline --graph
```

---

## Step 5: Handle Merge Conflicts

### Task 5.1: Create Conflict Scenario

```bash
# Create branch for conflict
git checkout -b feature/conflict-test

# Make changes to app.component.html
echo '<h1>Team App - Version 1</h1>' > src/app/app.component.html
git add .
git commit -m "chore: update app title"

# Switch to main
git checkout main

# Make conflicting changes
echo '<h1>Different Title</h1>' > src/app/app.component.html
git add .
git commit -m "chore: change app title"
```

### Task 5.2: Attempt Merge

```bash
# Try to merge
git merge feature/conflict-test

# Output shows CONFLICT
# Auto-merging src/app/app.component.html
# CONFLICT (content): Merge conflict in src/app/app.component.html
```

### Task 5.3: Resolve Conflict

```bash
# View conflict
git status

# Open file and resolve
# src/app/app.component.html contains:
# <<<<<<< HEAD
# <h1>Different Title</h1>
# =======
# <h1>Team App - Version 1</h1>
# >>>>>>> feature/conflict-test

# Choose one version or combine:
echo '<h1>Team App - Final Version</h1>' > src/app/app.component.html

# Stage resolved file
git add src/app/app.component.html

# Complete merge
git commit -m "Merge: resolve conflict in app title

Combined ideas from both approaches"
```

---

## Step 6: Simulate Collaborative Workflow

### Task 6.1: Feature Branch Workflow

```bash
# Create feature branch
git checkout -b feature/user-profile

# Generate component
ng generate component components/user-profile

# Make changes
# Edit files...

# Stage and commit
git add .
git commit -m "feat(profile): add user profile component"

# Push branch
git push -u origin feature/user-profile
```

### Task 6.2: Code Review Simulation

```bash
# View changes before merge
git log feature/user-profile --oneline

# Check diff
git diff main..feature/user-profile
```

### Task 6.3: Complete Merge

```bash
# Switch to main
git checkout main

# Merge feature
git merge feature/user-profile

# Push to remote
git push origin main
```

---

## Step 7: Practice Branch Commands

### Task 7.1: List Branches

```bash
# List local branches
git branch

# List all branches
git branch -a
```

### Task 7.2: Delete Branches

```bash
# Delete merged local branch
git branch -d feature/user-profile

# Delete remote branch
git push origin --delete feature/user-profile
```

---

## Exercise Requirements

### Exercise 1: Complete Workflow

Complete all steps:
- [ ] Create feature branch
- [ ] Make commits
- [ ] Merge to main
- [ ] Resolve any conflicts
- [ ] Delete merged branches

### Exercise 2: Multiple Features

Create additional features:
- [ ] Create feature/user-settings
- [ ] Create feature/user-dashboard
- [ ] Merge sequentially to main
- [ ] Handle any conflicts

### Exercise 3: Branch Management

Demonstrate:
- [ ] List all branches
- [ ] Switch between branches
- [ ] Delete merged branches

---

## Assessment Criteria

| Criteria | Points |
|----------|--------|
| Feature branches created correctly | 15 |
| Commits made on branches | 15 |
| Merges completed successfully | 20 |
| Conflicts resolved properly | 20 |
| Branch cleanup | 10 |
| Git commands used correctly | 10 |
| Best practices followed | 10 |

---

## Team Collaboration Tips

### Best Practices for Teams

1. **Never work directly on main**
2. **Create branch for each feature**
3. **Keep branches short-lived**
4. **Communicate with team**
5. **Test before merging**
6. **Review changes together**

### Branch Naming Conventions

```bash
feature/add-user-login
feature/user-registration
bugfix/fix-validation-error
hotfix/security-patch
release/v1.0.0
```

---

## Summary

This project teaches:

- Creating feature branches
- Independent development on branches
- Merging branches to main
- Resolving merge conflicts
- Branch management and cleanup
- Team collaboration practices

You now understand team workflows in Git!

---

**Next Project**: [GitHub Project Management](./Project_03_GitHub_Project_Management.md)

**Previous Project**: [Version Control Angular Project](./Project_01_Version_Control_Angular_Project.md)
