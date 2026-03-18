# Branching Strategies

## What You'll Learn

- What branches are and why they matter
- Common branch naming conventions
- Popular branching workflows
- When to create branches
- Branch management commands

## Prerequisites

- Completed `02-git-basics.md`
- Understanding of basic Git commands

## What Are Branches?

A branch is a separate line of development. Think of it like a parallel universe:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    BRANCH VISUALIZATION                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  main ──────●───────●───────●───────●───────                            │
│                │                                                             │
│                │                                                             │
│          feature ────●──────●───────                                    │
│                                                                             │
│  The dots are commits. main and feature are separate lines of development. │
│                                                                             │
│  At any point, you can create a branch (start a new line)                 │
│  or merge branches (combine lines).                                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

**Why use branches?**
- Work on new features without breaking working code
- Isolate experimental work
- Enable parallel development
- Make it easy to code review changes

## Branch Naming Conventions

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    BRANCH NAMING                                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  FORMAT: type/description                                                   │
│                                                                             │
│  Types:                                                                     │
│  • feature/  - New features                                                 │
│  • fix/      - Bug fixes                                                     │
│  • bugfix/   - Same as fix                                                  │
│  • hotfix/   - Urgent production fixes                                       │
│  • docs/     - Documentation changes                                         │
│  • refactor/ - Code refactoring                                             │
│  • test/     - Adding tests                                                 │
│  • chore/    - Maintenance tasks                                            │
│                                                                             │
│  Examples:                                                                  │
│  • feature/user-authentication                                            │
│  • fix/login-redirect-bug                                                  │
│  • hotfix/security-patch                                                  │
│  • docs/update-readme                                                     │
│                                                                             │
│  Rules:                                                                     │
│  ✅ Use lowercase                                                           │
│  ✅ Use hyphens to separate words                                         │
│  ✅ Be descriptive but concise                                             │
│  ❌ Don't use special characters                                           │
│  ❌ Don't use ticket numbers alone                                         │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Creating and Managing Branches

### Creating a Branch

```bash
# Create and switch to new branch
git checkout -b feature/new-login

# Or (newer syntax)
git switch -c feature/new-login
```

🔍 **What this does:**
- `-c` stands for "create and switch"
- Creates a new branch from your current position
- Switches you to that branch

### Switching Branches

```bash
# Switch to existing branch
git checkout main

# Or (newer syntax)
git switch main
```

🔍 **What this does:**
- Moves you to the specified branch
- **Warning:** Uncommitted changes should be stashed or committed first

### Listing Branches

```bash
# Local branches
git branch

# Remote branches
git branch -r

# All branches
git branch -a
```

### Deleting Branches

```bash
# Delete a local branch (safe - only if merged)
git branch -d feature/old-feature

# Force delete a local branch
git branch -D feature/abandoned

# Delete remote branch
git push origin --delete feature/old-feature
```

## Popular Branching Strategies

### 1. Git Flow

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GIT FLOW                                                 │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Main branches (permanent):                                                │
│  • main ──── Production-ready code                                        │
│  • develop ─ Integration branch                                            │
│                                                                             │
│  Supporting branches (temporary):                                          │
│  • feature/* ─ New features (from develop, to develop)                   │
│  • release/* ─ Release prep (from develop, to main + develop)             │
│  • hotfix/*  ─ Urgent fixes (from main, to main + develop)               │
│                                                                             │
│  Visual:                                                                   │
│                                                                             │
│  main:     ●────────●────────●────────●───                              │
│                    ↑              ↑                                        │
│  develop: ●──●──●──●──●──●──●──●──●──●──●─                             │
│              ↑       ↑       ↑                                             │
│  feature:      ●──●──●──●                                               │
│                                                                             │
│  Best for: Traditional teams with scheduled releases                        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

**Git Flow Commands:**

```bash
# Start a feature
git checkout -b feature/my-feature develop
# ... do work ...
git checkout develop
git merge --no-ff my-feature
git branch -d my-feature

# Start a hotfix
git checkout -b hotfix/critical-fix main
# ... do work ...
git checkout main
git merge --no-ff hotfix/critical-fix
git checkout develop
git merge --no-ff hotfix/critical-fix
git branch -d hotfix/critical-fix
```

### 2. GitHub Flow

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GITHUB FLOW                                               │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Rules:                                                                     │
│  1. main branch is always deployable                                      │
│  2. Work in feature branches                                              │
│  3. Open pull request to merge                                            │
│  4. Merge after review                                                     │
│  5. Deploy immediately                                                     │
│                                                                             │
│  Visual:                                                                   │
│                                                                             │
│  main:   ●────────●──────●──────●───────                                │
│               ↑        ↑     ↑                                             │
│  feature:    ●──●──●──●                                                │
│                                                                             │
│  Best for: Teams with continuous deployment                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

**GitHub Flow Commands:**

```bash
# Create feature branch
git checkout -b feature/my-feature main

# Make commits
git commit -am "Add new feature"

# Push and create PR
git push -u origin feature/my-feature

# After PR approval and merge
git checkout main
git pull
git branch -d feature/my-feature
```

### 3. Trunk-Based Development

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    TRUNK-BASED DEVELOPMENT                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Rules:                                                                     │
│  1. Developers work in short-lived branches (hours or days)               │
│  2. Or directly on main/trunk                                             │
│  3. Small, frequent commits                                               │
│  4. Feature flags to hide unfinished work                                 │
│                                                                             │
│  Visual:                                                                   │
│                                                                             │
│  main:   ●──●──●──●──●──●──●──●──●──●──●──●──●──●──●                      │
│              ↑       ↑                                                     │
│  branch:    ●──●──●──●                                                    │
│                                                                             │
│  Best for: High-velocity teams, continuous deployment                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## When to Create Branches

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHEN TO BRANCH                                            │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ CREATE A BRANCH WHEN:                                                  │
│  • Working on a new feature                                                │
│  • Fixing a bug (even small ones!)                                        │
│  • Experimenting with something risky                                     │
│  • Changing configuration                                                  │
│  • Updating documentation                                                  │
│  • Refactoring code                                                        │
│                                                                             │
│  ⚠️  DON'T NEED A BRANCH FOR:                                             │
│  • Single-file typo fixes on main (if quick and safe)                     │
│  • Very simple changes                                                     │
│                                                                             │
│  💡 PRO TIP: When in doubt, branch out. It's cheap and safe.              │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Branch Commands Reference

```bash
# Create and switch
git checkout -b feature/name
git switch -c feature/name

# Switch branches
git checkout branch-name
git switch branch-name

# List branches
git branch          # local
git branch -r       # remote
git branch -a       # all

# Delete branches
git branch -d branch-name        # safe delete
git branch -D branch-name        # force delete

# Rename branch
git branch -m old-name new-name

# Track remote branch
git checkout --track origin/feature/name
```

## Working with Remote Branches

```bash
# List remote branches
git fetch --all
git branch -r

# Clone a specific branch
git clone -b branch-name url

# Track a remote branch
git checkout --track origin/feature/name
# or
git checkout -b feature/name origin/feature/name

# Push branch to remote
git push -u origin feature/name

# Delete remote branch
git push origin --delete feature/name
```

## Summary

- Branches isolate work—each branch is a separate line of development
- Name branches with type/description (feature, fix, hotfix, etc.)
- Git Flow: main + develop + feature/release/hotfix branches
- GitHub Flow: main + feature branches + PRs
- Trunk-based: short-lived branches, work directly on main
- Choose a strategy that fits your team's workflow

## Next Steps

→ Continue to `04-merge-vs-rebase.md` to understand when to merge vs rebase.
