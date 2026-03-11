# Git Add and Commit

## Topic Title
Staging and Saving Changes in Git

## Concept Explanation

The `git add` and `git commit` commands are the core of the Git workflow. Together, they allow you to stage specific changes and save them permanently in your repository's history. Understanding these commands is essential for maintaining a clean and meaningful project history.

### What is `git add`?

The `git add` command adds changes from the working directory to the staging area. It tells Git which files you want to include in your next commit.

### What is `git commit`?

The `git commit` command takes all the staged changes and saves them as a permanent snapshot in the repository. Each commit has:
- A unique identifier (SHA-1 hash)
- Author information
- Timestamp
- Commit message

### Why Staging and Committing Matter

1. **Selective commits**: You can choose exactly what goes into each commit
2. **Organized history**: Related changes can be grouped together
3. **Review process**: Staging gives you a chance to review before committing
4. **Clear history**: Good commits make project history useful

## The `git add` Command

### Basic Syntax

```bash
# Add a specific file
git add filename

# Add all files in current directory
git add .

# Add all files with specific extension
git add *.ts

# Add all files in a directory
git add src/

# Add multiple specific files
git add file1.txt file2.js
```

### Adding Files by Status

```bash
# Add all new files (untracked)
git add .

# Add all modified files
git add -u

# Add all changes (new, modified, deleted)
git add -A
```

### Interactive Staging

```bash
# Interactive mode - choose what to stage
git add -i

# Patch mode - choose which parts of files to stage
git add -p
```

### Understanding Staging

When you run `git add`, Git:
1. Reads the file content
2. Creates a blob object
3. Adds it to the staging area (index)
4. Marks it to be included in next commit

```
Working Directory          Staging Area           Repository
┌─────────────┐          ┌─────────────┐
│ file.ts     │  ──add──►│ file.ts     │
│ (modified)  │          │ (staged)    │
└─────────────┘          └─────────────┘
```

## The `git commit` Command

### Basic Syntax

```bash
# Commit with a message
git commit -m "Your message here"

# Commit all staged changes
git commit

# Add and commit in one command (only for tracked files)
git commit -a -m "Message"

# Amend the last commit
git commit --amend
```

### Commit Message Best Practices

#### Good Commit Messages

```bash
# ✓ Specific and clear
git commit -m "Add user authentication service"

# ✓ Include what and why
git commit -m "Fix login form validation

- Add email format validation
- Show error message for invalid emails
- Update unit tests"

# ✓ Use present tense
git commit -m "Add component" (not "Added component")
```

#### Bad Commit Messages

```bash
# ✗ Too vague
git commit -m "Fixed stuff"

# ✗ No description
git commit -m "Update"

# ✗ Not specific
git commit -m "Changes"
```

### Commit Message Format

Many teams use a specific format for commit messages:

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `style`: Formatting
- `refactor`: Code restructuring
- `test`: Adding tests
- `chore`: Maintenance

**Examples:**
```bash
git commit -m "feat(auth): add login component"

git commit -m "fix(validation): resolve email regex issue

The previous regex didn't handle all valid email formats.
Updated to use Angular's built-in email validator."

git commit -m "docs(readme): update installation instructions"
```

## Practical Examples with Angular

### Example 1: Adding a New Component

```bash
# Generate a new component
ng generate component components/user-list

# Check what was created
git status

# Add the new component files
git add src/app/components/user-list/

# Commit with descriptive message
git commit -m "feat(components): add UserListComponent

- Create UserListComponent with Angular CLI
- Add template with user table
- Add basic styling
- Add component spec file"
```

### Example 2: Modifying Existing Files

```bash
# Edit app.component.ts
# Add a new property

# Check status
git status
# Output: modified: src/app/app.component.ts

# Stage the change
git add src/app/app.component.ts

# Commit
git commit -m "fix(app): add title property to AppComponent

- Add title property with default value
- Update template to display title"
```

### Example 3: Adding Multiple Related Changes

```bash
# Work on a feature across multiple files
# 1. Create service
ng generate service services/user

# 2. Update component to use service

# 3. Update routing

# Stage all related changes
git add src/app/services/
git add src/app/app.component.ts
git add src/app-routing.module.ts

# Commit together
git commit -m "feat(user): add UserService integration

- Create UserService with getUsers method
- Inject service in AppComponent
- Update routing to include user route"
```

## Advanced Staging

### Partial File Staging

You can stage only parts of a file:

```bash
# Enter patch mode
git add -p filename
```

Git will show you each "hunk" (section) of changes and ask:
- `y` - stage this hunk
- `n` - don't stage this hunk
- `s` - split into smaller hunks

### Staging Deleted Files

```bash
# Stage a deleted file
git add deleted-file.txt

# Or use
git add -u
```

### Checking What Will Be Committed

```bash
# See what's staged
git status

# See staged changes (diff)
git diff --cached

# See unstaged changes
git diff
```

## The Commit Object

When you commit, Git creates a commit object:

```
┌─────────────────────────────────────┐
│          COMMIT OBJECT             │
├─────────────────────────────────────┤
│  SHA-1: 4a2b3c...                   │
│                                     │
│  Author: Jane <jane@example.com>   │
│  Date: 2026-03-08 10:00:00         │
│                                     │
│  Message: Add login feature        │
│                                     │
│  Tree: 5d6e7f...                    │
│  Parent: 3g2h1i... (previous)      │
└─────────────────────────────────────┘
```

Each commit points to:
- A tree object (snapshot of files)
- Parent commit(s) (history)
- Author and committer info

## Best Practices

### 1. Commit Related Changes Together

```bash
# ✓ Good: Related changes
git add src/app/login/
git commit -m "Add login component and validation"

# ✗ Bad: Unrelated changes in one commit
git add .
git commit -m "Fix bug and add feature"
```

### 2. Commit Frequently

```bash
# Make small, focused commits
# Easier to understand and revert
git commit -m "Add button component"
git commit -m "Style button component"
git commit -m "Add button tests"
```

### 3. Review Before Committing

```bash
# Always check what you're staging
git status
git diff --cached
```

### 4. Write Good Messages

- First line: 50 characters or less
- Body: Explain what and why, not how
- Use imperative mood

### 5. Never Commit Generated Files

```bash
# Don't commit:
# - node_modules/
# - dist/
# - .angular/

# Use .gitignore to exclude these
```

## Common Mistakes

### Mistake 1: Forgetting to Stage

```bash
# Edit files
# Try to commit without staging
git commit -m "My changes"

# ✗ Error: nothing to commit
# ✓ Fix: stage first
git add .
git commit -m "My changes"
```

### Mistake 2: Committing Everything

```bash
# Adding everything including unwanted files
git add .
git commit -m "Everything"

# ✓ Better: be selective
git add src/
git add package.json
git commit -m "Add source files"
```

### Mistake 3: Empty Commit Messages

```bash
# ✗ Bad
git commit -m ""

# ✓ Good
git commit -m "Add user service"
```

### Mistake 4: Amending Published Commits

```bash
# Don't amend commits that have been pushed
git commit --amend  # Only for local commits!
```

## Exercises for Students

### Exercise 1: Practice Staging
1. Create 3 new files
2. Stage only 2 of them
3. Verify with `git status`
4. Commit the staged files

### Exercise 2: Write Good Messages
For each scenario, write a good commit message:
- Adding a new Angular component
- Fixing a validation bug
- Updating documentation
- Refactoring a service

### Exercise 3: Related Changes
1. Modify one file
2. Create another related file
3. Stage both
4. Commit with a message that explains both

## Mini Practice Tasks

### Task 1: Basic Add and Commit
```bash
# 1. Create a file
echo "Hello Angular" > hello.txt

# 2. Check status
git status

# 3. Stage the file
git add hello.txt

# 4. Check status again
git status

# 5. Commit
git commit -m "Add hello file"

# 6. View log
git log
```

### Task 2: Multiple Files
```bash
# 1. Create multiple files
echo "Component" > component.ts
echo "Template" > component.html
echo "Styles" > component.css

# 2. Stage all TypeScript files
git add *.ts

# 3. Commit
git commit -m "Add component TypeScript files"

# 4. Stage remaining files
git add *.html *.css

# 5. Commit
git commit -m "Add component template and styles"
```

### Task 3: Angular Feature Workflow
```bash
# 1. Generate Angular component
ng generate component components/todo-list

# 2. Check status
git status

# 3. Stage all new files
git add src/app/components/todo-list/

# 4. Commit with proper message
git commit -m "feat(todo): add TodoListComponent

- Create component with Angular CLI
- Add basic template structure
- Add styling for list layout"
```

### Task 4: Fix and Commit
```bash
# 1. Make a change to a file
# Edit src/app/app.component.ts

# 2. Check what changed
git diff src/app/app.component.ts

# 3. Stage the change
git add src/app/app.component.ts

# 4. Commit with fix message
git commit -m "fix: update component title"
```

## Summary

The `git add` and `git commit` commands are essential:

- **`git add`** stages files for commit
- **`git commit`** saves staged changes permanently
- Use `.gitignore` to exclude unwanted files
- Write clear, descriptive commit messages
- Commit related changes together
- Review before committing with `git diff --cached`

These commands form the foundation of your Git workflow. Master them to maintain a clean, useful project history!

---

**Next Lesson**: [Viewing Git History](./08_Viewing_Git_History.md)

**Previous Lesson**: [Creating a Git Repository](./06_Creating_a_Git_Repository.md)
