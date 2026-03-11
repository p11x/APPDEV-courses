# GitHub Issues

## Topic Title
Tracking Bugs and Features with GitHub Issues

## Concept Explanation

GitHub Issues are the primary way to track bugs, feature requests, and tasks on GitHub. They provide a centralized place to discuss problems, plan features, and manage project work.

### What are Issues?

Issues are:
- Bug reports
- Feature requests
- Task tracking
- Questions/discussions
- Improvement suggestions

### Why Use Issues?

1. **Track work**: Don't forget important tasks
2. **Collaboration**: Discuss with team
3. **Transparency**: Everyone sees what needs doing
4. **Accountability**: Know who's responsible
5. **History**: Keep record of decisions

## Creating Issues

### Basic Issue Creation

1. Go to repository
2. Click "Issues" tab
3. Click "New issue"
4. Fill in details
5. Submit

### Issue Fields

```
Title: Clear, concise description
Body:
- Description
- Steps to reproduce (bugs)
- Expected behavior
- Actual behavior
- Environment

Labels: bug, feature, enhancement
Projects: (optional)
Milestone: (optional)
Assignees: (optional)
```

## Issue Templates

Create templates for consistency:

### Bug Report Template

```markdown
## Bug Description
Brief description

## Steps to Reproduce
1. Go to...
2. Click on...
3. See error

## Expected Behavior
What should happen

## Actual Behavior
What actually happens

## Environment
- OS:
- Browser:
- Angular version:

## Possible Fix
Suggestions?
```

### Feature Request Template

```markdown
## Feature Description
Brief description

## Problem Solved
What problem does this solve?

## Proposed Solution
How should it work?

## Alternatives
Other solutions considered?

## Additional Context
Screenshots, examples?
```

## Issue Labels

### Common Labels

| Label | Color | Use |
|-------|-------|-----|
| bug | red | Something isn't working |
| enhancement | green | Improvement request |
| feature | blue | New feature request |
| documentation | yellow | Docs improvements |
| help wanted | purple | Need assistance |
| good first issue | green | Good for beginners |
| question | gray | Question |
| priority | orange | High priority |

### Custom Labels

Create custom labels:
```
Settings → Labels → New label
```

## Managing Issues

### Search and Filter

```bash
# Search issues
is:issue is:open bug

# Labels
label:bug label:enhancement

# Assignee
assignee:username

# Milestone
milestone:"v1.0"

# Time
created:>2024-01-01
updated:<2024-03-01
```

### Using Projects

Kanban-style boards:
```
Projects → New project → Board
```

Columns:
- To Do
- In Progress
- Done

## Closing Issues

### Automatically with Commits

Reference issues in commits:

```bash
# Fix a bug
git commit -m "Fix login validation bug

Fixes #123"

# Close issue
git commit -m "Add user dashboard

Closes #456"

# Multiple
git commit -m "Update dependencies

Closes #1
Closes #2"
```

### Manually

- Comment "Fixed in #XXX"
- Click "Close issue" button
- Close as "not planned" if rejected

## Angular-Specific Issues

### Common Angular Issues

```
## Angular Bug Report
Version: Angular 17

## Bug
Form validation not working with async validators

## Steps
1. Create form with async validator
2. Submit form
3. Error not displayed

## Expected
Show error message

## Actual
No error shown
```

### Issue Lifecycle

```
Open → In Progress → Review → Merged → Closed
```

## GitHub Projects Integration

### Create Project Board

1. Go to Projects
2. Create new project
3. Add columns
4. Link issues

### Workflow

```
To Do → In Progress → In Review → Done
     → To Do → In Progress → In Review → Done
```

## Best Practices

### 1. Good Issue Titles

```bash
# ✓ Good
"Login form validation not showing error message"
"Add dark mode support"
"Improve dashboard load performance"

# ✗ Bad
"Help!"
"Bug"
"Feature"
```

### 2. Include Details

```bash
# Always include:
- Clear description
- Steps to reproduce (bugs)
- Expected vs actual behavior
- Environment details
- Screenshots if helpful
```

### 3. Use Labels

```bash
# Categorize issues
label:bug
label:feature
label:priority
```

### 4. Assign to People

```bash
# Clear ownership
assignee:username
```

## GitHub CLI for Issues

```bash
# List issues
gh issue list

# Create issue
gh issue create --title "Bug" --body "Description"

# View issue
gh issue view 123

# Close issue
gh issue close 123
```

## Exercises for Students

### Exercise 1: Create Bug Report
1. Find any Angular project on GitHub
2. Create a mock bug report issue
3. Include all required details

### Exercise 2: Use Labels
1. Create issues with different labels
2. Filter by labels
3. See how search works

### Exercise 3: Link to Commits
1. Create a branch
2. Make commits referencing issue numbers
3. Push and see automatic closure

## Summary

Issues track project work:

- **Create**: Report bugs, request features
- **Organize**: Labels, milestones, projects
- **Search**: Filter and find issues
- **Close**: Link to commits or manual

Key concepts:
- Detailed bug reports help fix faster
- Feature requests explain the "why"
- Labels organize work
- Projects track progress

Use issues to manage Angular projects effectively!

---

**Next Section**: [Advanced Git and GitHub](./../03_Advanced_Git_and_GitHub/19_Git_Stashing.md)

**Previous Lesson**: [Pull Requests](./17_Pull_Requests.md)
