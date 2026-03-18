# Git Workflows

## What You'll Learn

- Common team-based Git workflows
- Centralized workflow (single branch)
- Feature branch workflow
- Forking workflow
- Choosing the right workflow for your team

## Prerequisites

- Completed `04-merge-vs-rebase.md`
- Understanding of branching strategies

## What Is a Git Workflow?

A Git workflow is how your team uses Git to manage code changes. It defines:
- When to create branches
- How to integrate changes
- Who can push where
- Code review requirements

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHY HAVE A WORKFLOW                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Without a workflow:                                                        │
│  • Confusion about where to work                                           │
│  • Merge conflicts everywhere                                               │
│  • Broken main branch                                                       │
│  • Lost code                                                                │
│                                                                             │
│  With a defined workflow:                                                   │
│  • Clear expectations                                                      │
│  • Predictable process                                                     │
│  • Better collaboration                                                    │
│  • Safer code integration                                                  │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Centralized Workflow

The simplest workflow—everyone works on main:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CENTRALIZED WORKFLOW                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Setup:                                                                     │
│  ┌─────────────────────────────────────────────────────────────┐          │
│  │  origin/main  ◄── Everyone pushes to here                   │          │
│  └─────────────────────────────────────────────────────────────┘          │
│                                                                             │
│  Daily Workflow:                                                            │
│  1. git pull (get latest)                                                 │
│  2. Make changes                                                          │
│  3. git add + commit                                                      │
│  4. git push                                                              │
│                                                                             │
│  If conflict:                                                              │
│  1. git pull --rebase                                                     │
│  2. Resolve conflicts                                                     │
│  3. git push                                                              │
│                                                                             │
│  Pros: Simple, familiar to SVN users                                      │
│  Cons: Only works for small teams, no code review                         │
│                                                                             │
│  Best for: Solo developers, small teams (<5 people)                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Commands

```bash
# Start of day
git pull

# Work, then commit
git add .
git commit -m "Fix login bug"
git push
```

## Feature Branch Workflow

Everyone works on feature branches, merges via PR:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    FEATURE BRANCH WORKFLOW                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Setup:                                                                     │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐                │
│  │ origin/main  │    │ origin/feat1 │    │ origin/feat2 │                │
│  │ (protected)  │    │  (PR-based)  │    │  (PR-based)  │                │
│  └──────────────┘    └──────────────┘    └──────────────┘                │
│                                                                             │
│  Rules:                                                                     │
│  • main is protected (can't push directly)                                │
│  • All changes through branches                                            │
│  • All branches merged via Pull Request                                   │
│  • Requires code review                                                    │
│                                                                             │
│  Workflow:                                                                  │
│  1. Create branch from main                                               │
│  2. Work and commit                                                        │
│  3. Push and create PR                                                    │
│  4. Get reviews, address feedback                                         │
│  5. Merge after approval                                                   │
│                                                                             │
│  Pros: Code review, clean main, organized                                 │
│  Cons: More complex, PR overhead                                           │
│                                                                             │
│  Best for: Most teams                                                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Commands

```bash
# Start new feature
git checkout main
git pull
git checkout -b feature/new-login

# Work
git add .
git commit -m "Add login form"
git push -u origin feature/new-login

# On GitHub: Create PR, review, merge
# After merge:
git checkout main
git pull
git branch -d feature/new-login
```

## Forking Workflow

Contributors don't push to the main repo at all:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    FORKING WORKFLOW                                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Setup:                                                                     │
│  ┌─────────────────────────────────────────────────────────────────┐       │
│  │ UPSTREAM REPO (main project)                                   │       │
│  │ owner/project                                                   │       │
│  └─────────────────────────────────────────────────────────────────┘       │
│                              ▲                                              │
│                              │ PR                                            │
│  ┌─────────────────────────────────────────────────────────────────┐       │
│  │ YOUR FORK (your copy)                                          │       │
│  │ your-username/project                                          │       │
│  └─────────────────────────────────────────────────────────────────┘       │
│                              ▲                                              │
│                              │ push                                          │
│  ┌─────────────────────────────────────────────────────────────────┐       │
│  │ YOUR LOCAL REPO                                                │       │
│  └─────────────────────────────────────────────────────────────────┘       │
│                                                                             │
│  Workflow:                                                                  │
│  1. Fork the repository on GitHub                                          │
│  2. Clone your fork                                                         │
│  3. Add upstream remote                                                     │
│  4. Create branch from your fork                                            │
│  5. Work, commit, push to YOUR fork                                        │
│  6. Create PR to upstream                                                   │
│                                                                             │
│  Pros: Anyone can contribute, security (no push access needed)           │
│  Cons: More setup, syncing is complex                                       │
│                                                                             │
│  Best for: Open source projects                                             │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Commands

```bash
# 1. Fork on GitHub (click Fork button)

# 2. Clone your fork
git clone https://github.com/YOUR_USERNAME/project.git
cd project

# 3. Add upstream remote
git remote add upstream https://github.com/ORIGINAL/project.git

# 4. Create branch
git checkout -b feature/contribution

# 5. Work and commit
git add .
git commit -m "Add helpful contribution"

# 6. Push to YOUR fork
git push origin feature/contribution

# 7. Create PR to upstream
# (on GitHub: compare across forks, create PR)

# 8. Keep your fork synced
git fetch upstream
git checkout main
git merge upstream/main
git push origin main
```

## Git Flow Workflow

A more structured approach with specific branch types:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GIT FLOW WORKFLOW                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Branch Types:                                                              │
│  ┌──────────┬──────────────────────────────────────────┐                  │
│  │ Branch   │ Purpose                                  │                  │
│  ├──────────┼──────────────────────────────────────────┤                  │
│  │ main     │ Production code                          │                  │
│  │ develop  │ Integration branch                       │                  │
│  │ feature  │ New features (from develop, to develop) │                  │
│  │ release  │ Release prep (from develop, to both)    │                  │
│  │ hotfix   │ Emergency fixes (from main, to both)    │                  │
│  └──────────┴──────────────────────────────────────────┘                  │
│                                                                             │
│  Flow:                                                                      │
│  • feature/* branches from develop, merge back to develop                │
│  • release/* branches from develop, merge to main + develop              │
│  • hotfix/* branches from main, merge to main + develop                  │
│                                                                             │
│  Pros: Clear structure, good for releases                                  │
│  Cons: Complex, many branches                                               │
│                                                                             │
│  Best for: Projects with scheduled releases                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Commands

```bash
# Start feature
git checkout develop
git checkout -b feature/new-feature

# Finish feature
git checkout develop
git merge --no-ff feature/new-feature
git branch -d feature/new-feature

# Start release
git checkout -b release/v1.0.0 develop

# Finish release
git checkout main
git merge --no-ff release/v1.0.0
git tag -a v1.0.0 -m "Version 1.0.0"
git checkout develop
git merge --no-ff release/v1.0.0
git branch -d release/v1.0.0

# Start hotfix
git checkout -b hotfix/urgent-fix main

# Finish hotfix
git checkout main
git merge --no-ff hotfix/urgent-fix
git checkout develop
git merge --no-ff hotfix/urgent-fix
git branch -d hotfix/urgent-fix
```

## Choosing Your Workflow

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CHOOSING A WORKFLOW                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Solo Developer:                                                            │
│  → Centralized (simple, fast)                                              │
│                                                                             │
│  Small Team (<5):                                                          │
│  → Feature Branch (code review, organization)                             │
│                                                                             │
│  Growing Team (5-20):                                                      │
│  → Feature Branch + GitHub Flow                                           │
│                                                                             │
│  Large Team / Enterprise:                                                  │
│  → Git Flow (structured, release-based)                                    │
│                                                                             │
│  Open Source Project:                                                      │
│  → Forking Workflow                                                        │
│                                                                             │
│  Continuous Deployment:                                                    │
│  → GitHub Flow (trunk-based)                                              │
│                                                                             │
│  💡 Tip: You can adapt and combine workflows to fit your needs.          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- A workflow defines how your team uses Git
- Centralized: Simple, single branch, for small teams
- Feature Branch: Branches + PRs, recommended for most teams
- Forking: For open source, contributors don't need push access
- Git Flow: Structured, with specific branch types, good for releases

## Next Steps

→ Continue to `06-resolving-merge-conflicts.md` to learn how to handle conflicts when they arise.
