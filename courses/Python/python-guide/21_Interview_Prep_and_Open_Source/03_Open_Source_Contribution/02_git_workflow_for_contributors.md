# 🔄 Git Workflow for Contributors

> The exact Git workflow used by professional open source contributors.

## 🎯 What You'll Learn

- Fork vs clone workflow
- Complete git commands with explanations
- Keeping your fork in sync
- Conventional commits
- Squashing commits
- Interactive staging
- Resolving merge conflicts

## 📦 Prerequisites

- Completion of [01_finding_and_reading_projects.md](./01_finding_and_reading_projects.md)

---

## Fork vs Clone

### Why Fork First?

You never push directly to the original repository. You work on your fork:

```
Original Repo (upstream)
    ↓ fork
Your Fork (origin)
    ↓ clone
Your Local Machine
```

### Step-by-Step Workflow

```bash
# 1. Fork on GitHub (click Fork button)

# 2. Clone YOUR fork
git clone https://github.com/YOUR_USERNAME/PROJECT.git
cd PROJECT

# 3. Add upstream as remote (for syncing)
git remote add upstream https://github.com/ORIGINAL/PROJECT.git

# 4. Verify remotes
git remote -v
# origin   https://github.com/YOUR_USERNAME/PROJECT.git (fetch)
# origin   https://github.com/YOUR_USERNAME/PROJECT.git (push)
# upstream https://github.com/ORIGINAL/PROJECT.git (fetch)
# upstream https://github.com/ORIGINAL/PROJECT.git (push)
```

### 💡 Explanation

- `origin` — your fork on GitHub
- `upstream` — the original repository
- You pull from upstream, push to origin

---

## Making Changes

```bash
# 1. Create a new branch for your fix
git checkout -b fix/issue-123-description

# 2. Make your changes
# ... edit files ...

# 3. Check what changed
git status

# 4. Stage changes interactively (review each chunk)
git add -p

# 5. Commit with conventional message
git commit -m "fix: resolve issue #123 - description"

# 6. Push to YOUR fork
git push origin fix/issue-123-description
```

### Conventional Commits

Format: `<type>: <description>`

| Type | When to Use |
|------|-------------|
| `feat` | New feature |
| `fix` | Bug fix |
| `docs` | Documentation |
| `test` | Adding tests |
| `refactor` | Code refactoring |
| `chore` | Maintenance |

---

## Keeping Your Fork in Sync

```bash
# 1. Fetch latest from upstream
git fetch upstream

# 2. Rebase on main (cleaner history)
git rebase upstream/main

# 3. Force push to your fork (be careful!)
git push origin main --force-with-lease
```

### 💡 Explanation

- `--force-with-lease` — safer than `--force`, fails if someone else pushed
- Rebasing rewrites commit history to be linear
- Alternative: `git merge upstream/main` (creates merge commit)

---

## Interactive Staging

```bash
# Stage changes piece by piece
git add -p

# Options for each chunk:
# y - stage this hunk
# n - don't stage this hunk
# s - split into smaller hunks
# e - manually edit hunk
```

### Why Use Interactive Staging?

- Review each change before committing
- Separate logical changes into separate commits
- Don't accidentally commit debug prints or temp files

---

## Squashing Commits

Combine multiple commits into one:

```bash
# Before opening PR, squash last 3 commits
git rebase -i HEAD~3

# In editor:
# pick abc1234 First commit
# squash def5678 Second commit  
# squash ghi9012 Third commit

# Save and close editor
# Write new commit message
```

### 💡 Explanation

- `pick` — keep as is
- `squash` — combine with previous commit
- Results in one clean commit for your feature

---

## Resolving Merge Conflicts

```bash
# When rebase/merge fails due to conflicts:

# 1. See conflicted files
git status

# 2. Edit each file to resolve conflicts
# Look for <<<<<<< HEAD
# =======
# >>>>>>> branch-name

# 3. Mark as resolved
git add <file>

# 4. Continue rebase
git rebase --continue

# OR abort if things go wrong
git rebase --abort
```

### Conflict Resolution Tips

1. Don't panic — it's normal
2. Understand what both sides are trying to do
3. Ask in the issue/PR if unclear
4. Test after resolving

---

## Complete Workflow Example

```bash
# Setup (once)
git clone https://github.com/you/project.git
cd project
git remote add upstream https://github.com/original/project.git

# Starting work (each time)
git fetch upstream
git checkout main
git rebase upstream/main
git checkout -b fix/issue-123

# Making changes
# ... edit files ...
git add -p
git commit -m "fix: resolve issue #123"

# Keep up to date
git fetch upstream
git rebase upstream/main

# Push and create PR
git push origin fix/issue-123
# Now open PR on GitHub

# After PR feedback
# ... make more changes ...
git add -p
git commit -m "fix: address review feedback"
git push origin fix/issue-123
```

---

## Summary

✅ **Fork first** — never push to original repo

✅ **Branch per fix** — one branch per feature/fix

✅ **Use conventional commits** — feat, fix, docs, test, refactor

✅ **Interactive staging** — review each change with `git add -p`

✅ **Squash before PR** — clean commit history

✅ **Rebase to stay current** — keep your fork in sync

---

## ➡️ Next Steps

Continue to [03_making_your_first_contribution.md](./03_making_your_first_contribution.md)

---

## 🔗 Further Reading

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Git Handbook](https://guides.github.com/introduction/git-handbook/)
- [Oh Shit, Git!?!](https://ohshitgit.com/) - Common Git mistakes
