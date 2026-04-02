---
title: "Git Workflow for Bootstrap Projects"
module: "Team Workflow"
difficulty: 2
estimated_time: 20
tags: ["git", "workflow", "branching", "releases"]
prerequisites: ["Git fundamentals", "Bootstrap project structure"]
---

## Overview

A well-defined Git workflow ensures that Bootstrap component changes, SCSS customizations, and theme updates are developed, reviewed, and released in a controlled manner. This guide covers branching strategies, commit conventions, and release flows tailored to Bootstrap projects where visual changes require careful coordination between developers and designers.

## Basic Implementation

**Branch Strategy**

Use a feature-branch workflow with protected main and develop branches.

```
main (production)
  └── develop (integration)
       ├── feature/navbar-redesign
       ├── feature/dark-mode-support
       ├── fix/modal-focus-trap
       └── release/v2.3.0
```

```bash
# Create feature branch from develop
git checkout develop
git pull origin develop
git checkout -b feature/card-component-update

# Work on changes
git add src/scss/_cards.scss
git commit -m "feat(card): add horizontal layout variant"

# Push and create PR to develop
git push -u origin feature/card-component-update
```

**Commit Convention**

Use conventional commits to enable automated changelog generation.

```bash
# Format: type(scope): description

# Component changes
git commit -m "feat(alert): add dismissible animation variant"
git commit -m "fix(modal): resolve backdrop click not closing modal"
git commit -m "refactor(navbar): extract collapse logic to utility"

# SCSS changes
git commit -m "style(grid): adjust gutter values for mobile breakpoint"
git commit -m "feat(theme): add dark mode CSS custom properties"

# Documentation
git commit -m "docs(readme): update setup instructions for Bootstrap 5.3"
```

**Release Flow**

```bash
# Create release branch from develop
git checkout develop
git checkout -b release/v2.3.0

# Bump version and finalize changelog
npm version minor
git add CHANGELOG.md
git commit -m "chore(release): v2.3.0"

# Merge to main and develop
git checkout main
git merge --no-ff release/v2.3.0
git tag -a v2.3.0 -m "Release v2.3.0"

git checkout develop
git merge --no-ff release/v2.3.0

# Delete release branch
git branch -d release/v2.3.0
```

## Advanced Variations

**Hotfix Workflow**

```bash
# Create hotfix from main
git checkout main
git checkout -b hotfix/modal-z-index-fix

# Fix the issue
git commit -m "fix(modal): correct z-index stacking for nested modals"

# Merge to both main and develop
git checkout main
git merge --no-ff hotfix/modal-z-index-fix
git tag -a v2.2.1 -m "Hotfix: modal z-index"

git checkout develop
git merge --no-ff hotfix/modal-z-index-fix
```

**Theme Branch Strategy**

For projects with multiple themes, use long-running theme branches.

```
main
  ├── theme/dark-mode (long-running)
  │    ├── feature/dark-cards
  │    └── feature/dark-forms
  └── theme/high-contrast (long-running)
       └── feature/contrast-buttons
```

```bash
# Rebase theme branch periodically
git checkout theme/dark-mode
git rebase main
```

## Best Practices

1. **Branch from develop** for features, from main for hotfixes
2. **Use conventional commit messages** with type and scope
3. **Keep feature branches short-lived** - merge within 2-3 days
4. **Rebase feature branches** before creating PRs to keep history clean
5. **Never force-push to main or develop** - use revert commits instead
6. **Tag all releases** with semantic version numbers
7. **Include Bootstrap version in release notes** when it changes
8. **Run visual regression tests** before merging Bootstrap changes
9. **Use `--no-ff` for merges** to preserve branch history
10. **Delete merged branches** to keep the repository clean
11. **Write descriptive PR titles** that match commit conventions
12. **Squash trivial commits** before merging feature branches

## Common Pitfalls

1. **Working directly on main** - bypasses review and breaks production
2. **Long-lived feature branches** - accumulate merge conflicts over time
3. **Inconsistent commit messages** - prevents automated tooling
4. **No tag for releases** - cannot reference specific versions in issues
5. **Force-pushing shared branches** - destroys collaborators' work
6. **Merging without PR review** - bypasses quality gates
7. **Not rebasing before merge** - introduces unnecessary merge commits
8. **Ignoring .gitignore** - committing `node_modules` or compiled CSS
9. **Mixing feature and fix commits** - makes cherry-picking difficult
10. **No branch naming convention** - branches cannot be categorized

## Accessibility Considerations

Include accessibility test results in PR descriptions for UI changes. Ensure that feature branches with accessibility fixes are prioritized in the merge queue. Tag releases that include accessibility improvements distinctly in changelogs.

## Responsive Behavior

When visual regression testing is part of the workflow, ensure it captures multiple breakpoints. Include responsive screenshot comparisons in PR reviews for layout changes. Document any breakpoint-specific fixes in commit messages for traceability.
