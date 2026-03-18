# Making Your First Contribution

## What You'll Learn

- Understanding issue tracking
- Creating pull requests
- Following contribution guidelines

## Prerequisites

- Completed `03-setting-up-development-environment.md`

## Finding an Issue

1. Search for "good first issue" or "beginner" labels
2. Read the issue description carefully
3. Ask questions if anything is unclear
4. Comment that you're working on it

## Making Changes

```bash
# 1. Create a new branch
git checkout -b fix/issue-number-description

# 2. Make your changes
# Edit files, add tests

# 3. Commit with descriptive message
git add .
git commit -m "Fix: Description of what was fixed

- What was the problem
- How this fixes it
- What tests were added"

# 4. Push to your fork
git push origin fix/issue-number-description
```

## Creating a Pull Request

1. Navigate to the original repository
2. Click "Compare & pull request"
3. Fill out the PR template:
   - Description of changes
   - Related issue number
   - Screenshots (if applicable)
4. Submit and wait for review

## PR Best Practices

- Keep PRs small and focused
- Write clear commit messages
- Respond to feedback promptly
- Be patient with review

## Summary

- Start with good first issues
- Create focused, small PRs
- Be responsive to feedback

## Next Steps

Continue to `05-understanding-contribution-guidelines.md`.
