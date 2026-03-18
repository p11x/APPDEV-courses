# Resolving Merge Conflicts

## What You'll Learn

- What causes merge conflicts
- How to identify conflicts
- Tools and strategies for resolution
- Best practices to avoid conflicts
- When to get help

## Prerequisites

- Completed `05-git-workflows.md`
- Basic Git knowledge

## What Is a Merge Conflict?

A merge conflict happens when Git can't automatically combine changes:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHAT CAUSES CONFLICTS                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Git can automatically merge when:                                         │
│  • Changes are in different files                                          │
│  • Changes are in different parts of the same file                        │
│  • One branch deleted a file, another modified it (sometimes)             │
│                                                                             │
│  Git CANNOT auto-merge when:                                               │
│  • Same lines changed in both branches                                     │
│  • One branch modified a file, another deleted it                          │
│  • Same function renamed in both branches                                  │
│                                                                             │
│  Think of it like:                                                         │
│  Two people edited the same sentence in a document.                      │
│  You need to choose which version to keep.                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Identifying Conflicts

When you try to merge or rebase and have conflicts:

```bash
git merge feature/new-login
```

You'll see:

```
Auto-merging app.py
CONFLICT (content): Merge conflict in app.py
Automatic merge failed; fix conflicts and then commit the result.
```

Git also tells you which files have conflicts:

```bash
git status
```

```
On branch main
You have unmerged paths.
  (fix conflicts and run "git commit")

Unmerged paths:
  (use "git add <file>..." to mark resolution)
	both modified:   app.py
	deleted by them: utils.py
	added by them:  new_file.py
```

## Understanding Conflict Markers

Git marks conflicts in files like this:

```python
def authenticate():
<<<<<<< HEAD
    # Current branch (main)
    # New security requirements
    token = generate_secure_token()
    return token
=======
    # Incoming branch (feature)
    # Simpler approach
    return "basic-auth"
>>>>>>> feature/new-login
```

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CONFLICT MARKER PARTS                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  <<<<<<< HEAD                                                               │
│  Your current branch's changes                                             │
│  (what's in your working directory)                                        │
│  =======                                                                    │
│  The incoming changes being merged                                         │
│  =======                                                                    │
│  >>>>>>> branch-name                                                        │
│                                                                             │
│  You need to:                                                               │
│  1. Delete the markers (<, =, >)                                           │
│  2. Keep the code you want (or combine both)                               │
│  3. Save the file                                                          │
│  4. Mark as resolved with git add                                         │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Resolving Conflicts

### Step 1: Review the Conflicts

```bash
# See what's conflicting
git diff

# See just the conflicting files
git diff --name-only --diff-filter=U
```

### Step 2: Edit the Files

Open each conflicted file and choose your resolution:

**Option A: Keep your version (HEAD)**

```python
def authenticate():
    # Current branch (main)
    # New security requirements
    token = generate_secure_token()
    return token
```

**Option B: Keep their version (incoming)**

```python
def authenticate():
    # Incoming branch (feature)
    # Simpler approach
    return "basic-auth"
```

**Option C: Combine both**

```python
def authenticate():
    # Combined approach
    token = generate_secure_token()
    log_authentication(token)
    return token
```

### Step 3: Mark as Resolved

```bash
# After editing each file, stage it
git add app.py

# When all conflicts are resolved, complete the merge
git commit
```

## Using Tools

### VS Code

VS Code makes conflict resolution easier:
- Colored labels show which changes are where
- Click "Accept Current Change", "Accept Incoming Change", or "Accept Both"
- Inline diff view

### Git Mergetool

```bash
# Configure a merge tool
git config --global merge.tool vscode
git config --global mergetool.vscode.cmd 'code --wait $MERGED'

# Launch the tool
git mergetool
```

### Tools Comparison

| Tool | Best For |
|------|----------|
| VS Code | Visual editing, easy choice |
| KDiff3 | Three-way merge view |
| Beyond Compare | Professional, detailed |
| GitHub Desktop | Simple conflicts |

## Rebase Conflicts

Resolving during rebase is similar to merge:

```bash
git rebase main

# See conflicts
# Resolve them

git add conflicted-file.py
git rebase --continue

# Or abort if needed
git rebase --abort
```

The key difference: rebase applies commits one by one. After resolving, you continue applying remaining commits.

## Best Practices to Avoid Conflicts

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    AVOIDING CONFLICTS                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ PULL REGULARLY:                                                         │
│  git pull --rebase main  (or git fetch + git merge)                       │
│                                                                             │
│  ✅ COMMUNICATE:                                                            │
│  Let teammates know when working on related files                        │
│                                                                             │
│  ✅ SMALL COMMITS:                                                         │
│  Smaller changes = easier to resolve when conflicts happen                │
│                                                                             │
│  ✅ WORK ON SEPARATE FILES:                                                │
│  When possible, divide work by files                                      │
│                                                                             │
│  ✅ MERGE EARLY AND OFTEN:                                                 │
│  Don't let branches diverge too much                                      │
│                                                                             │
│  ❌ DON'T IGNORE CONFLICTS:                                                │
│  Always resolve completely before continuing                              │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Common Conflict Scenarios

### Same Line Changed

```python
# Both changed "return True" to different things
def is_valid():
<<<<<<< HEAD
    return len(value) > 0
=======
    return value is not None
>>>>>>> feature/validation
```

**Solution:** Choose or combine.

```python
def is_valid():
    return value is not None and len(value) > 0
```

### File Deleted by One Branch, Modified by Another

```
error: the following file is deleted by us: utils.py
```

**Solution:** Either keep the file (restore it) or delete it:

```bash
# Keep the file
git add -u utils.py
git checkout --theirs utils.py  # or --ours

# Or delete it
git rm utils.py
```

### After Resolving, But Made a Mistake

```bash
# Undo the merge and start over
git merge --abort

# Or undo the commit after resolving
git reset --hard HEAD~1
```

## Summary

- Conflicts occur when Git can't auto-merge changes
- Git marks conflicts with `<<<<<<<`, `=======`, `>>>>>>>`
- Edit files to keep one version, combine, or create new
- Stage with `git add` when resolved
- Pull regularly to avoid conflicts

## Next Steps

→ Continue to `07-git-hooks-and-automation.md` to learn how to automate tasks with Git hooks.
