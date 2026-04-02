# 🔍 Finding and Reading Projects

> How to find the right open source project and actually understand it.

## 🎯 What You'll Learn

- Why contribute to open source
- Finding beginner-friendly projects
- Evaluating a project before contributing
- How to read an unfamiliar codebase
- Tools for exploring codebases

## 📦 Prerequisites

- Completion of [02_Coding_Challenges/02_recursion_and_dynamic_programming.md](../02_Coding_Challenges/02_recursion_and_dynamic_programming.md)

---

## Why Contribute to Open Source

1. **Learn from expert code** — read well-written, tested, battle-hardened code
2. **Build reputation** — demonstrate skills to potential employers
3. **Give back** — improve tools you use daily
4. **Meet developers** — join communities of like-minded people
5. **Improve skills** — learn code review, testing, documentation

---

## Finding Beginner-Friendly Projects

### GitHub Labels to Look For

| Label | Meaning |
|-------|---------|
| `good first issue` | Explicitly marked for newcomers |
| `help wanted` | Needs contribution |
| `beginner friendly` | Easy to start |
| `up-for-grabs` | Available for anyone |
| `first-timers-only` | Specifically for first-time contributors |

### Platforms for Finding Projects

- **goodfirstissue.dev** — Curated list of good first issues
- **up-for-grabs.net** - Projects with available tasks
- **firstcontributions.github.io** - Guided first contribution
- **GitHub Explore** - Browse by topic

### Python-Specific Projects

- **CPython** — Python interpreter itself
- **Pandas** — Data analysis library
- **Requests** — HTTP library
- **FastAPI** — Modern web framework
- **Rich** — Terminal formatting library
- **Black** — Code formatter

---

## Evaluating a Project

Before contributing, check:

```python
# 1. Last commit date - is it active?
# Check: github.com/project/repo/commits

# 2. Issue response time
# Check: github.com/project/repo/issues

# 3. PR merge rate
# Check: github.com/project/pulls

# 4. Contributor count
# Check: github.com/project/repo/graphs/contributors

# 5. README quality
# Check: Is it clear how to set up and contribute?
```

### Red Flags

- No activity in years
- No response to issues
- Hostile community
- Unclear contribution process

### Green Flags

- Active maintainers
- Clear CONTRIBUTING.md
- Good first issue labels
- Responsive to PRs
- Active Discord/Slack

---

## Reading a Codebase (5-Step Process)

### 1. Read README

Understand project purpose and architecture:

```python
# README typically covers:
# - What the project does
# - Installation
# - Quick start
# - Architecture overview
# - Contributing guide
```

### 2. Read Tests

Understand expected behavior:

```python
# Tests show:
# - How functions are supposed to work
# - Edge cases being handled
# - Expected inputs/outputs
# - Common usage patterns
```

### 3. Find Entry Point

Locate where execution starts:

```python
# Common patterns:
# - main.py
# - __main__.py
# - cli.py
# - __init__.py (for packages)
```

### 4. Follow One Feature End-to-End

Trace one feature through the code:

```python
# Example: Understand how a request flows
# 1. CLI entry point (main.py)
# 2. Router/dispatcher
# 3. Handler/controller
# 4. Service layer
# 5. Repository/database
```

### 5. Check Git History

Understand how the project evolved:

```bash
# View file history
git log --follow -p -- file.py

# See recent changes
git log --oneline -20

# Who changed what
git blame file.py
```

---

## Tools for Exploring Codebases

### grep / ripgrep

Search for patterns:

```bash
# Find function definition
grep -r "def function_name" .

# Find usage
grep -r "function_name" .

# ripgrep (faster)
rg "def function_name"
```

### ctags / cscope

Jump to definition:

```bash
# Generate tags
ctags -R .

# In vim/neovim
# Ctrl-] to jump to definition
# Ctrl-t to jump back
```

### IDE Navigation

VS Code:
- `Ctrl+Shift+F` — Search in files
- `F12` — Go to definition  
- `Alt+Left` — Go back
- `Ctrl+P` — Quick file open

---

## Summary

✅ **Good first issues** — look for labeled issues

✅ **Evaluate first** — check activity, community, documentation

✅ **Read tests** — understand expected behavior

✅ **Follow flow** — trace one feature completely

✅ **Use tools** — grep, tags, IDE for navigation

---

## ➡️ Next Steps

Continue to [02_git_workflow_for_contributors.md](./02_git_workflow_for_contributors.md)

---

## 🔗 Further Reading

- [First Contributions](https://firstcontributions.github.io/)
- [Good First Issues](https://goodfirstissue.dev/)
- [Open Source Guides](https://opensource.guide/)
