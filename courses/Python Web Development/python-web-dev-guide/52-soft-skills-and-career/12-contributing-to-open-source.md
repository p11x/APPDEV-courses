# Contributing to Open Source

## What You'll Learn

- Why open source matters for your career
- How to find beginner-friendly projects
- The contribution workflow (fork, branch, PR)
- Writing good issues and PRs
- Building a reputation in the community

## Prerequisites

This is about community involvement—no specific technical prerequisites.

## Why Open Source Matters

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHY CONTRIBUTE TO OPEN SOURCE                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  FOR YOUR CAREER:                                                          │
│  • Proof you can work with others                                          │
│  • Shows you understand code beyond tutorials                             │
│  • Builds relationships with experienced developers                         │
│  • Gets your name out there                                                │
│  • Learning from reading production code                                   │
│                                                                             │
│  FOR YOUR SKILLS:                                                           │
│  • Learn from code reviews                                                  │
│  • Get feedback on your code                                               │
│  • Learn project management                                                │
│  • Understand how major projects work                                      │
│                                                                             │
│  FOR THE COMMUNITY:                                                        │
│  • Give back to tools you use                                              │
│  • Help others                                                             │
│  • Shape the future of tools you love                                      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Finding Projects

### Good Projects for Beginners

Look for these labels:

| Label | Meaning |
|-------|---------|
| `good first issue` | Designed for newcomers |
| `help wanted` | Needs community help |
| `beginner` | Simple tasks |
| `up-for-grabs` | Available for anyone |

### Where to Look

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHERE TO FIND PROJECTS                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  GitHub Explore: github.com/explore                                       │
│  • Topics: python, api, web-framework                                      │
│  • Collections: "Your first PR"                                            │
│                                                                             │
│  Good First Issue: goodfirstissue.dev                                     │
│  • Curated beginner issues                                                 │
│  • Multiple languages                                                      │
│                                                                             │
│  Up for Grabs: up-for-grabs.net                                           │
│  • Task hunting                                                            │
│  • Filter by language                                                      │
│                                                                             │
│  Your Dependencies:                                                         │
│  • Look at what you use (FastAPI, Pydantic, etc.)                         │
│  • Check their GitHub issues                                               │
│                                                                             │
│  CodeTriage: codetriage.com                                               │
│  • Get issues delivered to you                                            │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Python Projects to Consider

- **FastAPI** — FastAPI framework itself
- **Pydantic** — Data validation
- **SQLAlchemy** — ORM
- **Black** — Code formatter
- **Requests** — HTTP library
- **httpx** — Async HTTP
- **Pydantic Settings** — Settings management
- **Textual** — TUI framework

## The Contribution Workflow

### Step 1: Fork and Clone

```bash
# Fork on GitHub (click the Fork button)

# Clone your fork
git clone https://github.com/YOUR_USERNAME/project-name.git
cd project-name

# Add upstream remote
git remote add upstream https://github.com/original/project-name.git
```

🔍 **What this does:**
- Forking creates your copy of the repository
- Cloning downloads it to your machine
- Adding upstream lets you sync with the original

### Step 2: Create a Branch

```bash
# Make sure you're on main
git checkout main

# Sync with upstream
git fetch upstream
git merge upstream/main main

# Create a new branch
git checkout -b fix/issue-description
# or
git checkout -b feature/add-new-feature
```

🔍 **Branch naming:**
- `fix/` for bug fixes
- `feature/` for new features
- `docs/` for documentation
- Use lowercase, hyphens

### Step 3: Make Changes

```python
# Make your changes, then:

# Check what you changed
git status
git diff

# Stage your changes
git add filename.py
# or
git add .
```

🔍 **Best practices:**
- Make one logical change per PR
- Don't mix fixes with refactoring
- Write clear commit messages

### Step 4: Commit Your Changes

```bash
# Commit with a message
git commit -m "Fix bug in user authentication

- Validate token before checking expiry
- Add tests for edge case
- Fixes #123"
```

🔍 **Commit message format:**
- First line: Short description (50 chars max)
- Blank line
- Detailed description (if needed)
- Reference issue number

### Step 5: Push and Create PR

```bash
# Push to your fork
git push origin fix/issue-description

# Then on GitHub:
# 1. Go to your fork
# 2. Click "Compare & pull request"
# 3. Fill out the PR template
# 4. Submit
```

## Writing Good Issues

### Before Creating an Issue

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    BEFORE CREATING AN ISSUE                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ SEARCH FIRST:                                                          │
│  • Has this already been reported?                                         │
│  • Is there a workaround?                                                 │
│  • Is there an existing PR?                                               │
│                                                                             │
│  ✅ CHECK DOCUMENTATION:                                                   │
│  • Did you read the docs?                                                  │
│  • Is this expected behavior?                                             │
│                                                                             │
│  ✅ REPRODUCE:                                                            │
│  • Can you reproduce the bug?                                             │
│  • What steps lead to it?                                                  │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Issue Template

```markdown
## Bug Description
Clear and concise description of what the bug is.

## To Reproduce
Steps to reproduce the behavior:
1. Go to '...'
2. Click on '....'
3. See error

## Expected Behavior
A clear description of what you expected to happen.

## Environment
- OS: [e.g., Ubuntu 20.04]
- Python version: [e.g., 3.11]
- Package version: [e.g., 1.0.0]

## Additional Context
Add any other context about the problem here.
```

## Writing Good Pull Requests

### PR Template

```markdown
## Description
What does this PR do? Why is it needed?

## Type of Change
- [ ] Bug fix (non-breaking change)
- [ ] New feature (non-breaking change)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## Testing
What testing did you do?
- [ ] Unit tests added/updated
- [ ] Manual testing

## Checklist
- [ ] My code follows the style guidelines
- [ ] I have performed a self-review
- [ ] I have commented complex code
- [ ] I have updated documentation
- [ ] My changes generate no new warnings
```

### What Makes a Good PR

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    GOOD PR CHARACTERISTICS                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ SMALL:                                                                  │
│  • One logical change                                                      │
│  • Easy to review                                                          │
│  • Less likely to conflict                                                 │
│                                                                             │
│  ✅ COMPLETE:                                                              │
│  • Includes tests                                                          │
│  • Updates documentation                                                   │
│  • Works with existing code                                                │
│                                                                             │
│  ✅ CLEAR:                                                                 │
│  • Explains WHY                                                            │
│  • Shows what changed                                                      │
│  • References issues                                                       │
│                                                                             │
│  ❌ HUGE:                                                                  │
│  • Hundreds of files                                                       │
│  • Multiple features mixed                                                 │
│                                                                             │
│  ❌ INCOMPLETE:                                                           │
│  • No tests                                                                │
│  • Breaks existing functionality                                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Handling Feedback

### When Your PR Gets Comments

```markdown
# DON'T:
- Take feedback personally
- Argue endlessly
- Ignore feedback
- Get defensive

# DO:
- Thank reviewers
- Ask clarifying questions
- Make requested changes
- Explain your reasoning if you disagree
- Be patient
```

### Example Response

```markdown
Thanks for the review! 

I went with approach A because [reason]. However, I see your point about [concern]. 

Let me update the code to address your feedback:
- Changed X to handle Y case
- Added test for Z edge case

Let me know if this looks better.
```

## Building Your Reputation

### Start Small

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    START SMALL                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Week 1-2:                                                                 │
│  • Find a project you use                                                  │
│  • Fix typo in documentation                                             │
│  • Translate a message                                                     │
│                                                                             │
│  Week 3-4:                                                                 │
│  • Fix simple bug                                                         │
│  • Add test case                                                           │
│  • Improve error message                                                   │
│                                                                             │
│  Week 5+:                                                                  │
│  • Tackle medium difficulty issues                                       │
│  • Start your own features                                                │
│  • Help others                                                             │
│                                                                             │
│  Build up to harder contributions over time.                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Other Ways to Contribute

| Activity | Impact |
|----------|--------|
| Answer issues | Help others |
| Review docs | Improve usability |
| triage issues | Manage backlog |
| Write tests | Improve coverage |
| Report bugs | Find problems |
| Share your use case | Feature requests |

## Your First Contribution

### Example: Fixing a Documentation Typo

```bash
# 1. Fork the repo on GitHub
# 2. Clone your fork
git clone https://github.com/YOUR_USERNAME/the-repo.git
cd the-repo

# 3. Create a branch
git checkout -b docs/fix-typo

# 4. Find and fix the typo in README.md
# Using your favorite editor

# 5. Commit
git add README.md
git commit -m "Fix typo in installation instructions"

# 6. Push
git push origin docs/fix-typo

# 7. Create PR on GitHub
```

This seems small, but it's:
- A real contribution
- Practice with the workflow
- Gets your name on the project

## Summary

- Start with "good first issue" labels
- Fork → Branch → Commit → Push → PR
- Make small, focused changes
- Write clear issues and PRs
- Handle feedback gracefully
- Build reputation over time

## Next Steps

This completes the Soft Skills and Career folder. You've learned:
- Technical documentation writing
- Code reading and review
- Task estimation
- Debugging strategies
- Technical questions
- Imposter syndrome
- Portfolio building
- Interview preparation
- System design
- Open source contribution

These skills complement your technical knowledge and will help you succeed in your Python web development career.

To continue learning, explore other folders in this guide or start building real projects!
