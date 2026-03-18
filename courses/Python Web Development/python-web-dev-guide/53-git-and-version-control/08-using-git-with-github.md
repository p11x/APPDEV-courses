# Using Git with GitHub

## What You'll Learn

- Setting up SSH keys for GitHub
- Creating and managing repositories
- Using Pull Requests effectively
- Code review on GitHub
- GitHub Actions basics

## Prerequisites

- Completed `07-git-hooks-and-automation.md`
- A GitHub account

## Setting Up SSH Keys

SSH keys let you push without entering your password:

### Step 1: Generate SSH Key

```bash
ssh-keygen -t ed25519 -C "your.email@example.com"
```

🔍 **What this does:**
- `ssh-keygen` — Creates a new SSH key
- `-t ed25519` — Uses the modern, secure algorithm
- `-C` — Adds a comment (your email)

Save to default location: `~/.ssh/id_ed25519`

### Step 2: Add to SSH Agent

```bash
# Start the agent
eval "$(ssh-agent -s)"

# Add the key
ssh-add ~/.ssh/id_ed25519
```

### Step 3: Add to GitHub

```bash
# Copy the public key
cat ~/.ssh/id_ed25519.pub
```

Then:
1. Go to GitHub → Settings → SSH and GPG keys
2. Click "New SSH key"
3. Paste your key
4. Give it a title

### Step 4: Configure SSH

Create `~/.ssh/config`:

```
Host github.com
    HostName github.com
    User git
    IdentityFile ~/.ssh/id_ed25519
```

Now you can use SSH URLs:

```bash
git clone git@github.com:username/repository.git
```

## Creating a Repository

### On GitHub

1. Click "+" → "New repository"
2. Name it (e.g., "my-project")
3. Choose public or private
4. Add README, .gitignore, license (optional)
5. Click "Create repository"

### From Local Code

```bash
# Create on GitHub first, then:
git remote add origin git@github.com:username/my-project.git
git branch -M main
git push -u origin main
```

### .gitignore

Create a `.gitignore` for Python:

```
# Byte-compiled / optimized / DLL files
__pycache__/
*.py[cod]
*$py.class

# Virtual environments
venv/
env/
ENV/

# Distribution / packaging
*.egg-info/
dist/
build/

# IDE
.vscode/
.idea/
*.swp

# Environment variables
.env

# Testing
.pytest_cache/
.coverage
htmlcov/

# Django
*.log
local_settings.py
db.sqlite3
```

## Pull Requests

Pull Requests (PRs) are how you contribute code:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PULL REQUEST WORKFLOW                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. Fork the repository (if not your own)                                 │
│                                                                             │
│  2. Clone your fork                                                         │
│                                                                             │
│  3. Create a feature branch                                                │
│     git checkout -b feature/my-feature                                     │
│                                                                             │
│  4. Make changes and commit                                                │
│                                                                             │
│  5. Push to your fork                                                      │
│     git push -u origin feature/my-feature                                  │
│                                                                             │
│  6. Create PR on GitHub                                                    │
│                                                                             │
│  7. Respond to feedback                                                     │
│                                                                             │
│  8. Get approved and merge                                                 │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Creating a Good PR

**Title:** Clear and descriptive
- ✅ "Add user authentication with JWT"
- ❌ "fix" or "my changes"

**Description:**
```markdown
## What
Brief description of changes

## Why
Why this change is needed

## How
How it works

## Testing
How to test the changes
```

### PR Options

| Option | When to Use |
|--------|-------------|
| Draft PR | Work in progress, not ready for review |
| WIP | Work in progress (adds prefix) |
| Reviewers | Request specific people |
| Labels | Categorize your PR |
| Projects | Add to project board |

## Code Review on GitHub

### As a Reviewer

```markdown
# Good feedback examples:

# Compliment
Great approach! Clean solution.

# Suggest
Consider using X instead because of Y.

# Question
How does this handle edge case Z?

# Require
Please add tests for this before merging.
```

### As an Author

```markdown
# Responding to feedback:

# Addressed
Fixed in abc123

# Discuss
I went with this approach because X. Let's discuss.

# Thanks
Good catch! I missed that.
```

### Review Labels

| Label | Meaning |
|-------|---------|
| changes requested | Need to make changes |
| approved | Ready to merge |
| pending | Waiting for author |
| requires discussion | Has unresolved questions |

## GitHub Issues

### Creating Good Issues

```markdown
## Bug Report
**Describe the bug**
Clear description of the issue

**To Reproduce**
Steps to reproduce:
1. Go to...
2. Click on...
3. See error

**Expected behavior**
What should happen

**Environment**
- OS: 
- Python version:
- Package version:
```

### Issue Templates

Create `.github/ISSUE_TEMPLATE/bug_report.md`:

```markdown
---
name: Bug report
about: Create a report to help us improve
title: ''
labels: bug
assignees: ''

---

**Describe the bug**
A clear description

**To Reproduce**
Steps
```

## GitHub Actions

GitHub Actions automate your workflow:

### Basic Workflow

Create `.github/workflows/test.yml`:

```yaml
name: Tests

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v4
      
      - name: Set up Python
        uses: actions/setup-python@v5
        with:
          python-version: '3.11'
          
      - name: Install dependencies
        run: |
          pip install -r requirements.txt
          
      - name: Run tests
        run: |
          pytest --cov
          
      - name: Upload coverage
        uses: codecov/codecov-action@v3
```

🔍 **What this does:**
- Triggers on push to main and PRs
- Uses Ubuntu latest
- Sets up Python 3.11
- Installs requirements
- Runs pytest with coverage
- Uploads to Codecov

### Common Actions

```yaml
# Linting
- uses: psf/black@stable

# Type checking
- uses: python/mypy@v1

# Security scanning
- uses: pyupio/safety@2
```

## GitHub Features

| Feature | Use For |
|---------|---------|
| Projects | Kanban boards for issues/PRs |
| Milestones | Group issues into releases |
| Wikis | Documentation |
| Discussions | Q&A |
| Packages | Hosting packages |
| Security | Vulnerability scanning |

## Summary

- Set up SSH keys for password-free push
- Create .gitignore for Python projects
- Use PRs for all changes (even small ones)
- Write clear PR descriptions
- Use GitHub Actions for automated testing

## Next Steps

→ Continue to `09-git-best-practices.md` to learn professional Git habits.
