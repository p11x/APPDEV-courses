# Git Basic Workflow

## Topic Title
Understanding the Git Workflow: Working Directory, Staging Area, and Repository

## Concept Explanation

The Git workflow is the foundation of how Git tracks changes in your projects. Understanding this workflow is essential for every Angular developer because it determines how you save, manage, and track your code over time.

### What is the Git Workflow?

The Git workflow describes how files move through three main areas during development:

1. **Working Directory**: Where you actually work on your files
2. **Staging Area**: Where you prepare what will be committed
3. **Local Repository**: Where your committed changes are stored permanently

### Why This Workflow is Important

This three-stage approach provides several key benefits:

- **Selective commits**: Choose exactly what goes into each commit
- **Review before saving**: Review changes before making them permanent
- **Organized history**: Keep your commit history clean and logical
- **Error recovery**: Easy to undo changes at any stage

## Understanding the Three Areas

### 1. Working Directory

The Working Directory is your project folder on your computer - it's where you create, edit, and delete files.

```
Location: Your actual project folder
Purpose: Where you do your actual coding work
State: Contains both tracked and untracked files
```

**Example for Angular:**
```bash
my-angular-app/
├── src/
│   ├── app/
│   │   ├── app.component.ts
│   │   ├── app.component.html
│   │   └── app.component.css
│   └── main.ts
├── angular.json
├── package.json
└── tsconfig.json
```

### 2. Staging Area (Index)

The Staging Area is a file (stored in `.git/index`) that contains information about what will go into your next commit.

```
Location: .git/index
Purpose: Preparation area for commits
State: Files marked for commit but not yet committed
```

Think of it like preparing items for shipment:
- You work with items in your workspace
- You select specific items to ship
- Only selected items get shipped (committed)

### 3. Local Repository

The Local Repository is the `.git` folder in your project - it's where Git stores all your committed snapshots.

```
Location: .git/ folder
Purpose: Permanent storage of project history
State: Contains all committed changes
```

## How Git Tracks Changes

### The File Lifecycle

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│  Untracked  │───►│   Modified  │───►│   Staged    │
│   (new)     │    │  (changed) │    │  (selected) │
└─────────────┘    └─────────────┘    └─────────────┘
                       │                    │
                       │                    ▼
                       │              ┌─────────────┐
                       └─────────────►│  Committed  │
                                      │   (saved)   │
                                      └─────────────┘
```

### File Status in Git

Every file in your project can be in one of these states:

1. **Untracked**: New file that Git doesn't know about
2. **Modified**: Existing file that has been changed
3. **Staged**: Modified file marked for commit
4. **Committed**: Change saved in repository

## The Git Workflow in Practice

### Step 1: Working with Files

You create or edit files in your working directory:

```bash
# Create a new Angular component
ng generate component components/user-list
```

This creates/modifies files in your working directory.

### Step 2: Checking Status

See what's changed:

```bash
# See status of all files
git status

# Short format
git status -s
```

**Output example:**
```
On branch main
Untracked files:
  (use "git add <file>..." to include in what will be committed)
    src/app/components/user-list/
```

### Step 3: Staging Changes

Mark files for the next commit:

```bash
# Add a specific file
git add src/app/app.component.ts

# Add all new and modified files
git add .

# Add all files in a directory
git add src/

# Add files matching a pattern
git add *.ts
```

After staging, check status again:

```bash
git status
```

**Output:**
```
Changes to be committed:
  (use "git rm --cached <file>..." to unstage)
    new file:   src/app/components/user-list/user-list.component.ts
    new file:   src/app/components/user-list/user-list.component.html
    new file:   src/app/components/user-list/user-list.component.css
```

### Step 4: Committing Changes

Save your staged changes:

```bash
# Commit with a message
git commit -m "Add user list component"

# Commit with detailed message
git commit -m "Add user list component

- Created UserListComponent with Angular CLI
- Added template with table structure
- Added basic styling for layout"
```

### Step 5: Viewing History

See your committed history:

```bash
# View full history
git log

# View short history
git log --oneline

# View with changes
git log -p
```

## Visual Flow Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                        YOUR COMPUTER                           │
│                                                                 │
│  ┌──────────────────┐      ┌──────────────────┐               │
│  │  Working         │      │  .git/            │               │
│  │  Directory       │      │  Directory       │               │
│  │                  │      │                  │               │
│  │  - app.component │      │  ┌────────────┐  │               │
│  │  - user.service  │      │  │ Repository │  │               │
│  │  - styles.css    │      │  │            │  │               │
│  │                  │      │  │ Commits    │  │               │
│  │  [EDIT FILES]    │      │  │ Snapshots  │  │               │
│  └────────┬─────────┘      │  │ History    │  │               │
│           │                │  └────────────┘  │               │
│           │ git add        │                  │               │
│           ▼                │                  │               │
│  ┌──────────────────┐      │                  │               │
│  │  Staging Area    │      │                  │               │
│  │  (.git/index)    │      │                  │               │
│  │                  │      │                  │               │
│  │  [STAGED FILES]  │      │                  │               │
│  └────────┬─────────┘      │                  │               │
│           │ git commit     │                  │               │
│           ▼                ▼                  │               │
│  ───────────────────────────────────────────────               │
│           COMMIT SNAPSHOT                                       │
└─────────────────────────────────────────────────────────────────┘
```

## Real-World Angular Example

### Scenario: Adding a New Feature

**Step 1: Work on feature**
```bash
# Edit your Angular component
# src/app/user.service.ts

@Injectable({
  providedIn: 'root'
})
export class UserService {
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>('/api/users');
  }
}
```

**Step 2: Check status**
```bash
git status
# Output: modified: src/app/user.service.ts
```

**Step 3: Stage the change**
```bash
git add src/app/user.service.ts
```

**Step 4: Commit**
```bash
git commit -m "Add getUsers method to UserService"
```

**Step 5: Verify**
```bash
git log --oneline
# Output: a1b2c3d Add getUsers method to UserService
```

## Best Practices

### 1. Commit Related Changes Together

```bash
# ✓ Good: Related changes in one commit
git add src/app/user.service.ts
git add src/app/user.service.spec.ts
git commit -m "Add UserService with getUsers method"

# ✗ Bad: Unrelated changes in one commit
git add .
git commit -m "Fix bug and add feature"
```

### 2. Write Meaningful Commit Messages

```bash
# ✗ Bad
git commit -m "updates"

# ✓ Good
git commit -m "Add form validation to login component"

# ✓ Better
git commit -m "Add email validation to login form

- Validate email format using Angular validators
- Show error message for invalid emails
- Add unit tests for validation logic"
```

### 3. Review Before Committing

```bash
# Always check what you're about to commit
git diff --cached

# Or use status
git status
```

### 4. Commit Frequently

- Small commits are easier to understand
- Easier to revert if something goes wrong
- Better collaboration with team

## Common Mistakes

### Mistake 1: Forgetting to Stage

```bash
# You edited files but didn't stage
git commit -m "My changes"

# ✗ Error: nothing to commit
# ✓ Fix: stage first
git add .
git commit -m "My changes"
```

### Mistake 2: Committing Untested Code

```bash
# Don't commit broken code
# Always test before committing
ng test
git commit -m "Add new feature"
```

### Mistake 3: Using `.` Too Liberally

```bash
# Adding everything can include unwanted files
git add .

# Better: be specific
git add src/
git add package.json
```

## Exercises for Students

### Exercise 1: Track the Workflow
Create a new Angular project (or any project) and:
1. Create a new file
2. Stage the file
3. Commit the file
4. View the history

### Exercise 2: Understand File States
Create multiple files:
1. Track only one file and commit
2. Leave another file untracked
3. Modify a committed file
4. Check all different statuses

### Exercise 3: Practice Selective Staging
1. Create 3 new files
2. Modify 2 existing files
3. Stage only 2 of the 5 changes
4. Commit only the staged changes
5. Verify the uncommitted changes remain

## Mini Practice Tasks

### Task 1: Basic Workflow Practice
```bash
# 1. Create or modify a file
echo "Hello Angular" > test.txt

# 2. Check status
git status

# 3. Stage the file
git add test.txt

# 4. Check status again
git status

# 5. Commit
git commit -m "Add test file"

# 6. View log
git log
```

### Task 2: Angular Component Workflow
```bash
# 1. Generate an Angular component
ng generate component components/dashboard

# 2. Check what files were created
git status

# 3. Stage all new files
git add src/app/components/dashboard/

# 4. Commit
git commit -m "Add dashboard component"

# 5. View the log
git log --oneline
```

### Task 3: Understanding Untracked Files
```bash
# 1. Create a new file without Git knowing
echo "secret data" > secret.txt

# 2. Check status
git status  # Shows as untracked

# 3. Add to .gitignore (we'll learn this later)
echo "secret.txt" > .gitignore
git add .gitignore
git commit -m "Add .gitignore"

# 4. Check status again
git status  # secret.txt still shows as untracked
```

## Summary

The Git workflow consists of three main areas:

1. **Working Directory**: Where you edit files
2. **Staging Area**: Where you prepare commits
3. **Repository**: Where committed changes are stored

The basic workflow:
1. Modify files in your working directory
2. Stage the files you want to commit
3. Commit the staged changes

Understanding this workflow is crucial for:
- Making clean, organized commits
- Working effectively with Git
- Collaborating with other developers

In the next lesson, we'll learn how to create a Git repository.

---

**Next Lesson**: [Creating a Git Repository](./06_Creating_a_Git_Repository.md)

**Previous Lesson**: [Git Configuration](./04_Git_Configuration.md)
