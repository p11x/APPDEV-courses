# Commit Messages and Pull Requests

## What You'll Learn

- The Conventional Commits specification and how to use it
- Anatomy of a perfect commit message
- How to write PR descriptions that reviewers love
- When to squash commits vs. preserve history
- Linking PRs to issues effectively

## Prerequisites

This builds on Git basics from folder 53. You should understand:
- Basic Git commands (commit, push, pull, branch)
- Creating and merging branches

## Why Commit Messages Matter

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    THE VALUE OF GOOD COMMITS                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  GOOD COMMITS:                                                            │
│  • Help future developers (including you) understand why changes were made │
│  • Make it easy to find when and why a bug was introduced                │
│  • Enable tools like git blame and bisect to work effectively             │
│  • Document the evolution of the codebase                                  │
│                                                                             │
│  BAD COMMITS:                                                             │
│  • "fixed stuff"                                                         │
│  • "asdf"                                                                │
│  • "WIP"                                                                 │
│                                                                             │
│  When you find a bug in 6 months, you'll thank your past self             │
│  for writing a descriptive commit message.                                 │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Conventional Commits

The Conventional Commits specification provides a standardized format:

```
<type>(<scope>): <subject>

[optional body]

[optional footer(s)]
```

### Type

The type describes the kind of change:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         COMMIT TYPES                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  feat     A new feature                                                    │
│  fix      A bug fix                                                        │
│  docs     Documentation changes only                                       │
│  style    Formatting, no code change (ruff, black)                        │
│  refactor Code change that neither fixes nor adds                         │
│  perf     Performance improvement                                          │
│  test     Adding or updating tests                                         │
│  chore    Build, tooling, dependencies (no production code)               │
│  ci       CI configuration changes                                         │
│  revert   Reverting a previous commit                                      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Scope (Optional)

The scope describes what part of the codebase changed:

```
feat(auth): add password reset endpoint
fix(database): fix connection pool leak
refactor(api): simplify user serialization
```

### Subject

The subject is a short description:
- Use imperative mood: "add" not "added" or "adds"
- Don't capitalize first letter
- No period at the end
- Under 50 characters

```
✅ feat: add user authentication
✅ fix: resolve memory leak in cache
❌ Added user authentication
❌ Added new feature
```

### Body (Optional)

Add context that doesn't fit in the subject:

```
feat(auth): add JWT refresh token rotation

Implemented refresh token rotation to improve security.
Old refresh tokens are now invalidated when used and
a new pair is issued.

Fixes #123
```

### Footer (Optional)

Link issues and breaking changes:

```
fix(api): validate email format

Closes #456
Breaks: #789 (see migration guide)
```

## Examples

### Good Commit Messages

```bash
# Feature
feat(auth): add OAuth2 login with Google

# Bug Fix
fix(api): return 404 for non-existent user

# Documentation
docs(readme): add installation instructions

# Refactoring
refactor(models): extract user validation to service

# Performance
perf(db): add index on user.email for login queries
```

### Bad Commit Messages

```bash
# Too vague
git commit -m "fixed stuff"
git commit -m "asdf"
git commit -m "WIP"

# Wrong tense
git commit -m "Added user authentication"
git commit -m "Fixing bugs"

# Too long
git commit -m "This commit adds the ability for users to log in using their Google account through OAuth2 which is a feature we've been planning for a while"
```

## The Anatomy of a Perfect PR

A good PR description answers these questions:

1. **What** does this PR do?
2. **Why** is this change needed?
3. **How** does it work?
4. **How to test** it?

### PR Description Template

```markdown
## Summary

<!-- One sentence: what does this PR do? -->
Adds JWT refresh token rotation for improved security.

## Context

<!-- Why is this needed? What problem does it solve? -->
Previously, refresh tokens never expired. If a token was stolen,
an attacker could use it indefinitely. This implements token
rotation per OWASP recommendations.

## Changes

<!-- How does it work? -->
- Added `refresh_token` column to users table
- Created `/auth/refresh` endpoint that issues new token pair
- Modified `/auth/login` to return both access and refresh tokens
- Token expiry: access 15 min, refresh 7 days

## Testing

<!-- How to verify this works? -->
- [ ] Unit tests pass: `pytest tests/test_auth.py`
- [ ] Tested login flow manually
- [ ] Tested refresh with expired access token

## Screenshots (if UI)

[Before/after screenshots if applicable]

## Related Issues

Closes #123
Related to #456

## Checklist

- [ ] Tests added/updated
- [ ] Documentation updated
- [ ] No breaking changes (or documented in footer)
```

## When to Squash vs. Preserve Commits

### Squash Commits (Most Common)

Squash all commits into one before merging when:

- Working on a feature branch with many "WIP" commits
- Each commit doesn't make sense independently
- You want a clean main history

```bash
# Interactive rebase to squash
git rebase -i HEAD~3
```

In the interactive editor:

```
pick abc1234 first commit
squash def5678 second commit  
squash ghi9012 third commit
```

### Preserve Commits (Rare)

Keep multiple commits when:

- Each commit is a logical, testable unit
- The history tells a story (e.g., a complex refactor)
- You're working with a team that values detailed history

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SQUASH VS PRESERVE DECISION                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  SQUASH WHEN:                   PRESERVE WHEN:                             │
│  - "WIP" commits               - Logical units of work                  │
│  - Fix-commits                  - Each commit compiles and passes tests    │
│  - Small incremental changes    - History tells a story                   │
│  - Feature branch              - Long-running branch                      │
│                                                                             │
│  DEFAULT: SQUASH                                                               │
│  Your main branch history will thank you.                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Linking PRs to Issues

### GitHub Keywords

```markdown
Closes #123        # Closes issue when PR merges
Fixes #123         # Same as closes
Closes #123, #456  # Multiple issues
Related to #789    # Links without closing
```

### Conventional Commits with Issues

```
feat(auth): add OAuth2 login

Implements Google OAuth2 as an alternative to password
authentication.

Closes #45
```

When this commit is in main with "Closes #45", GitHub automatically closes issue #45.

## GitHub Actions for Commit Messages

Enforce commit message standards with GitHub Actions:

```yaml
# .github/workflows/commitlint.yml
name: CommitLint

on:
  pull_request:
    types: [opened, synchronize, reopened]

jobs:
  commitlint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0
      
      - uses: wagoid/commitlint-github-action@v5
```

This will fail PRs that don't follow Conventional Commits.

## Real-World Application

### At a Startup

At a startup, PR speed matters. Don't block on perfect commits. Instead:
- Use squash by default
- Focus on clear PR descriptions
- Link issues for traceability

### In Open Source

Open source projects often enforce commit standards because:
- Contributors are unfamiliar with the codebase
- Maintainers need to understand changes quickly
- Clean history helps with bisecting bugs

Many projects like Angular, Conventional Commits, and FastAPI require conventional commits.

## Tools & Resources

| Tool | Purpose |
|------|---------|
| commitlint | Enforce commit message format |
| commitizen | Interactive commit creation |
| git-secrets | Prevent secrets in commits |

```bash
# Install commitizen for interactive commits
pip install commitizen
cz commit  # Interactive commit wizard
```

**Key resources:**
- [Conventional Commits](https://www.conventionalcommits.org/) — The specification
- [gitmoji](https://gitmoji.dev/) — Emoji commit convention (fun alternative)

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Writing Vague Messages

**Wrong:**
```
git commit -m "fixed bug"
```

**Why it fails:**
Doesn't tell future developers what bug, where, or why.

**Correct:**
```
fix(api): return 404 for non-existent user id

Previously, the API returned 500 when a user wasn't found.
Now it returns 404 with a helpful error message.
```

### ❌ Mistake 2: Not Linking Issues

**Wrong:**
Commit exists but no issue linked.

**Why it fails:**
Hard to trace why changes were made.

**Correct:**
```
feat(search): add full-text search

Implements PostgreSQL full-text search for product listings.

Closes #234
```

### ❌ Mistake 3: Huge PRs

**Wrong:**
500 lines changed, 12 files, doing everything at once.

**Why it fails:**
Hard to review thoroughly. Takes forever to merge.

**Correct:**
Break into multiple PRs:
- PR 1: Database schema changes
- PR 2: API endpoints
- PR 3: Frontend integration

### ❌ Mistake 4: No Testing Notes

**Wrong:**
PR description just says "tests pass."

**Why it fails:**
Reviewers don't know how to verify the changes work.

**Correct:**
```
## Testing
- [ ] Unit tests pass
- [ ] Manual test: created new user via API
- [ ] Manual test: login with new user works
```

## Summary

- Use Conventional Commits: type(scope): subject
- Write descriptive, imperative commit messages
- PR descriptions should answer: what, why, how, how to test
- Squash most commits; preserve when history tells a story
- Link PRs to issues using keywords (Closes, Fixes, Related)

## Next Steps

→ `06-debugging-mindset-and-strategies.md` — How to debug effectively using the scientific method.
