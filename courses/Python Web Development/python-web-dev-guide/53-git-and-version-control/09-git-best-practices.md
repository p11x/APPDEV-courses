# Git Best Practices

## What You'll Learn

- Professional Git habits
- Commit message conventions
- Avoiding common mistakes
- Daily workflows that scale
- Team conventions

## Prerequisites

- Completed `08-using-git-with-github.md`
- Experience with basic Git operations

## The Professional's Git Workflow

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DAILY PROFESSIONAL WORKFLOW                               │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  START OF DAY:                                                              │
│  1. git fetch origin                         ← Get latest                 │
│  2. git checkout main                                                 │
│  3. git pull origin main                     ← Update main               │
│                                                                             │
│  STARTING WORK:                                                             │
│  4. git checkout -b feature/my-task          ← Create branch             │
│                                                                             │
│  DURING WORK:                                                               │
│  5. Make small, focused commits                                            │
│  6. Write clear commit messages                                            │
│                                                                             │
│  BEFORE PUSHING:                                                            │
│  7. git fetch origin                                                        │
│  8. git rebase origin/main                    ← Update from main         │
│                                                                             │
│  END OF DAY:                                                                │
│  9. git push -u origin feature/my-task          ← Push branch             │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Commit Message Conventions

Good commit messages help everyone understand the history:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    COMMIT MESSAGE FORMAT                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  TYPE: short description (50 chars or less)                               │
│                                                                             │
│  Optional longer description (wrap at 72 chars)                           │
│                                                                             │
│  Optional footer (issue references)                                         │
│                                                                             │
│  ──────────────────────────────────────────────                           │
│                                                                             │
│  Types:                                                                     │
│  • feat:     A new feature                                                  │
│  • fix:      A bug fix                                                      │
│  • docs:     Documentation only changes                                    │
│  • style:    Formatting, no code change                                    │
│  • refactor: Code change that neither fixes nor adds                     │
│  • test:     Adding or updating tests                                      │
│  • chore:    Maintenance, deps, build changes                             │
│                                                                             │
│  Examples:                                                                  │
│                                                                             │
│  feat(auth): add JWT token refresh                                         │
│                                                                             │
│  fix: resolve redirect loop on logout                                      │
│                                                                             │
│  docs: update API documentation                                            │
│                                                                             │
│  refactor: simplify user model initialization                             │
│                                                                             │
│  Close #123                                                                 │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Commit Message Rules

```bash
# DON'T:
git commit -m "fix"
git commit -m "asdfasdf"
git commit -m "WIP"
git commit -m "stuff"

# DO:
git commit -m "feat: add user authentication"
git commit -m "fix: resolve login redirect loop"
git commit -m "docs: update setup instructions"
```

## Commit Size Guidelines

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    COMMIT SIZE                                               │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  SMALL COMMITS:                                                             │
│  ✅ Easier to review                                                        │
│  ✅ Easier to revert                                                       │
│  ✅ Clear history                                                          │
│  ✅ Better for bisecting                                                   │
│                                                                             │
│  HOW SMALL?                                                                 │
│  • One logical change per commit                                           │
│  • Can explain in one sentence                                            │
│  • Tests pass after each commit                                            │
│                                                                             │
│  ❌ AVOID:                                                                  │
│  • "and also..." commits                                                   │
│  • A week's worth of work in one commit                                    │
│  • Mixing refactoring with features                                        │
│                                                                             │
│  When in doubt, break it up!                                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Common Mistakes and How to Avoid Them

### 1. Committing Secrets

```bash
# ❌ NEVER DO:
git commit -m "add API key" app.py
# (contains API_KEY = "secret123")

# ✅ INSTEAD:
# Add to .gitignore
echo "config.py" >> .gitignore

# Remove from history (if already committed)
git filter-branch --force --index-filter \
  'git rm --cached --ignore-unmatch config.py' \
  --prune-empty --tag-name-filter 'cat' -- --all
```

### 2. Committing to Wrong Branch

```bash
# If you committed to wrong branch:
# Move the commit to correct branch
git branch feature/temp
git checkout correct-branch
git cherry-pick commit-hash
git checkout wrong-branch
git reset --hard HEAD~1
```

### 3. Forgot to Add Files

```bash
# Amend (add to last commit)
git add forgotten-file.py
git commit --amend --no-edit
```

### 4. Large Files in Git

```bash
# Check large files
git rev-list --objects --all | \
  git cat-file --batch-check='%(objecttype) %(objectname) %(objectsize) %(rest)' | \
  sed -n 's/^blob //p' | \
  sort -rnk2 | \
  head -20

# Remove from history
git lfs migrate --include="*.psd"
```

## Git Config for Productivity

```bash
# Essential aliases
git config --global alias.st status
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.ci commit
git config --global alias.df diff
git config --global alias.lg "log --oneline --graph --all"
git config --global alias.last "log -1 HEAD"
git config --global alias.unstage "reset HEAD --"

# Helpful settings
git config --global pull.rebase true
git config --global fetch.prune true
git config --global push.default current
git config --global init.defaultBranch main
```

## Daily Habits

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DAILY GIT HABITS                                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ ALWAYS:                                                                 │
│  • Pull before starting work                                               │
│  • Create a branch for each task                                           │
│  • Commit early, commit often                                              │
│  • Write meaningful commit messages                                         │
│  • Test before pushing                                                     │
│  • Review your changes before commit (git diff)                           │
│                                                                             │
│  ✅ USUALLY:                                                                │
│  • Rebase on main before pushing                                           │
│  • Squash messy commits before PR                                           │
│  • Delete merged branches                                                  │
│  • Sync fork if working on fork                                           │
│                                                                             │
│  ❌ NEVER:                                                                  │
│  • Force push to shared branches                                           │
│  • Commit secrets or credentials                                           │
│  • Commit broken code                                                      │
│  • Push directly to main                                                  │
│  • Work on main branch                                                    │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Team Conventions

### Establish These Team Standards

```markdown
# .github/CONTRIBUTING.md

## Branch Naming
- feature/description
- fix/description
- chore/description

## Commit Messages
- Use conventional commits
- 50 chars max for subject
- Reference issues (#123)

## Pull Requests
- All changes via PR
- Require 1 approval
- Pass CI checks
- Update docs if needed

## Code Review
- Be kind and constructive
- Request changes only when necessary
- Approve with minor comments
```

## Git Ignore Best Practices

```gitignore
# Python
__pycache__/
*.py[cod]
*$py.class
*.so
.Python
venv/
env/
ENV/
.eggs/
*.egg-info/
dist/
build/

# IDE
.vscode/
.idea/
*.swp
*.swo

# Testing
.pytest_cache/
.coverage
htmlcov/
.tox/

# Secrets
.env
.env.local
*.pem
*.key

# OS
.DS_Store
Thumbs.db

# Django
*.log
local_settings.py
db.sqlite3
media/
```

## Recovery Commands

```bash
# Lost a commit?
git reflog
git checkout HEAD@{number}

# Accidentally reset?
git reset --hard HEAD@{number}

# Deleted a branch?
git branch feature/deleted hash123

# Need to see what changed in a commit?
git show hash123

# Want to find when a bug was introduced?
git bisect start
git bisect bad
git bisect good known-good-commit
```

## Summary

- Pull before starting, create branches for work
- Write clear, conventional commit messages
- Keep commits small and focused
- Never commit secrets or broken code
- Establish team conventions and follow them

## Next Steps

This completes the Git and Version Control folder. You now have a comprehensive understanding of:
- Version control concepts
- Git basics and commands
- Branching strategies
- Merge vs rebase
- Team workflows
- Conflict resolution
- Git hooks and automation
- GitHub collaboration
- Professional best practices

Continue to other folders in this guide to build complete Python web development skills!
