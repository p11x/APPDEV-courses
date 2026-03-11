# Collaboration Workflows

## Topic Title
Team Collaboration Strategies and Workflows

## Concept Explanation

Effective collaboration requires well-defined workflows. Different teams use different approaches to manage development, and understanding these workflows helps you adapt to any team's needs.

### Why Use Workflows?

1. **Consistency**: Everyone follows same process
2. **Clarity**: Clear roles and responsibilities
3. **Quality**: Enforced through process
4. **Efficiency**: Reduced friction

## Common Workflows

### 1. Feature Branch Workflow

Every feature on its own branch:

```
main:     A---B---C---D---E---F

feature-a:       \---a1---a2
feature-b:             \---b1---b2
feature-c:                   \---c1
```

#### Steps

```bash
# 1. Update main
git checkout main
git pull

# 2. Create feature branch
git checkout -b feature/user-login

# 3. Work on feature
# ... commits ...

# 4. Push and create PR
git push -u origin feature/user-login
```

#### Pros
- Simple to understand
- Good for small teams
- Isolated changes

#### Cons
- Can have merge conflicts
- Long-lived branches problematic

### 2. GitFlow Workflow

More structured with multiple branches:

```
main:    A---B---C---M1---M2---M3
              \     \     \
develop:       D---E---F---M1---G---H---M2
                   \             \     \
feature-x:           X1---X2-------      \
                                         \
hotfix:                                    H1---H2
```

#### Branch Types

| Branch | Purpose | Base | Merges Into |
|--------|---------|------|-------------|
| main | Production | - | - |
| develop | Integration | main | main |
| feature/* | New features | develop | develop |
| release/* | Preparing release | develop | main, develop |
| hotfix/* | Urgent fixes | main | main, develop |

#### Commands

```bash
# Start feature
git checkout develop
git pull
git checkout -b feature/new-feature

# Finish feature
git checkout develop
git merge feature/new-feature
git push
git branch -d feature/new-feature

# Start release
git checkout develop
git checkout -b release/v1.0.0

# Finish release
git checkout main
git merge release/v1.0.0
git tag v1.0.0
git checkout develop
git merge release/v1.0.0
git push
git branch -d release/v1.0.0

# Start hotfix
git checkout main
git checkout -b hotfix/fix-bug
# fix...
git checkout main
git merge hotfix/fix-bug
git tag v1.0.1
git checkout develop
git merge hotfix/fix-bug
git push
```

### 3. Trunk-Based Development

Short-lived branches (hours, not days):

```
main:    A---B---C---D---E---F
              \   \       \
              f1--f2       g1--g2
```

```bash
# Work directly on main or very short branches
git checkout main
# small changes
git commit
# small changes
git commit
# done in hours not weeks
```

### 4. Forking Workflow

Contribute without direct access:

```
original repo: main: A---B---C

fork repo:     main: A---B---C
                     \
               feature: D---E

# PR from fork to original
```

#### Steps

```bash
# 1. Fork repository on GitHub
# 2. Clone your fork
git clone https://github.com/your-name/repo.git
cd repo

# 3. Add upstream
git remote add upstream https://github.com/original/repo.git

# 4. Create feature branch
git checkout -b feature/my-contribution

# 5. Work and push
git add .
git commit
git push origin feature/my-contribution

# 6. Create PR from your fork to original
```

## Angular Team Workflow Example

### Recommended: Feature Branch with GitHub Flow

```bash
# Setup
git clone git@github.com:team/angular-project.git
cd angular-project
git config user.name "Your Name"
git config user.email "your@email.com"

# Start feature
git checkout main
git pull
git checkout -b feature/add-user-dashboard

# Generate Angular components
ng generate component components/user-dashboard
ng generate service services/user

# Work and commit
git add .
git commit -m "feat(dashboard): add user dashboard component"

# Push and create PR
git push -u origin feature/add-user-dashboard
# Create PR on GitHub

# After review and approval, merge

# Sync and cleanup
git checkout main
git pull
git branch -d feature/add-user-dashboard
```

### Pull Request Workflow

```
1. Create feature branch
2. Make changes with clear commits
3. Push and create PR
4. Request review
5. Address feedback
6. Get approval
7. Squash and merge
8. Delete branch
```

## Best Practices for Teams

### 1. Keep Main Deployable

```bash
# Never push broken code to main
# Main should always be deployable

# Test locally before push
ng test
ng build
```

### 2. Small, Frequent Commits

```bash
# ✓ Good: Small, focused commits
git commit -m "Add button component"
git commit -m "Style button component"
git commit -m "Add button tests"

# ✗ Bad: Large, scattered commits
git commit -m "Add all the things"
```

### 3. Write Good Commit Messages

```bash
# ✓ Good
git commit -m "feat(auth): add login form validation

- Add email format validation
- Add required field validation
- Add password strength validation"

# ✗ Bad
git commit -m "fix"
git commit -m "updates"
```

### 4. Review Before Merging

```bash
# Always require code review
# Use GitHub protected branches
# Require status checks
```

### 5. Communicate

```bash
# Use issues for tracking
# Discuss in PR comments
# Use team communication channels
```

## Common Pitfalls

### Pitfall 1: Long-Lived Branches

```bash
# ✗ Avoid: Branches lasting months
# Gets increasingly difficult to merge

# ✓ Better: Short-lived branches
# Merge within days, not weeks
```

### Pitfall 2: Not Syncing

```bash
# ✗ Avoid: Working in isolation
# Conflicts pile up

# ✓ Better: Sync regularly
git fetch origin
git rebase origin/main
```

### Pitfall 3: Skipping Reviews

```bash
# ✗ Avoid: Self-approving PRs
# Skip the review process

# ✓ Better: Require reviews
# Get fresh perspective
```

## Exercises for Students

### Exercise 1: Feature Branch Workflow
1. Create feature branch
2. Make multiple commits
3. Push and create PR
4. Simulate review and merge

### Exercise 2: GitFlow
1. Use GitFlow structure
2. Create feature, release, hotfix branches
3. Practice merging in correct order

### Exercise 3: Forking Workflow
1. Fork a repository
2. Make changes
3. Create PR to original

## Summary

Choose the right workflow for your team:

- **Feature Branch**: Simple, good for most teams
- **GitFlow**: Structured, good for scheduled releases
- **Trunk-Based**: Fast, requires good testing
- **Forking**: For open source contributions

Key practices:
- Keep main deployable
- Use small, frequent commits
- Review code before merging
- Communicate with team

Effective collaboration leads to better software!

---

**Next Lesson**: [Managing Open Source Projects](./24_Managing_Open_Source_Projects.md)

**Previous Lesson**: [GitHub Actions](./22_GitHub_Actions.md)
