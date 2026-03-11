# Git Configuration

## Topic Title
Setting Up Git for Angular Development

## Concept Explanation

Git configuration allows you to customize Git's behavior to match your workflow and preferences. Think of it as setting up your personal workspace - you configure Git once, and it remembers your preferences for all future projects.

### Why Git Configuration is Important

Proper Git configuration is crucial because:

1. **Identity tracking**: Every commit needs your name and email
2. **Personalization**: Customize Git to work the way you want
3. **Team consistency**: Shared configurations help teams work together
4. **Performance**: Some settings improve Git's performance
5. **Security**: Proper credentials management

### Configuration Levels

Git has three levels of configuration:

```
┌─────────────────────────────────────────────────────────┐
│                  Local (.git/config)                    │
│         Project-specific settings (highest)            │
├─────────────────────────────────────────────────────────┤
│              Global (~/.gitconfig)                      │
│           User-specific settings (middle)              │
├─────────────────────────────────────────────────────────┤
│              System (/etc/gitconfig)                    │
│         System-wide settings (lowest priority)          │
└─────────────────────────────────────────────────────────┘
```

## Understanding Configuration Files

### 1. System Level Configuration
- Location: `/etc/gitconfig` (Linux/Mac) or `C:\Program Files\Git\etc\gitconfig` (Windows)
- Applies to all users on the system
- Requires administrator/root access

### 2. Global (User) Level Configuration
- Location: `~/.gitconfig` (Linux/Mac) or `C:\Users\username\.gitconfig` (Windows)
- Applies to all repositories for the current user
- Most common configuration level

### 3. Local Level Configuration
- Location: `.git/config` inside each repository
- Applies only to that specific repository
- Overrides global settings

## Essential Git Configuration Commands

### Setting Your Identity

```bash
# Set your name (appears in commits)
git config --global user.name "Your Name"

# Set your email (appears in commits)
git config --global user.email "your.email@example.com"

# Example for Angular developer
git config --global user.name "Jane Developer"
git config --global user.email "jane@angularpro.dev"
```

**Important**: Use the email associated with your GitHub account!

### Verifying Your Configuration

```bash
# View all configuration settings
git config --list

# View specific settings
git config user.name
git config user.email

# View with origin (where setting comes from)
git config --list --show-origin
```

### Output After Configuration

```
user.name=Jane Developer
user.email=jane@angularpro.dev
core.editor=code --wait
init.defaultbranch=main
```

## Recommended Configuration for Angular Developers

### Default Branch Name

```bash
# Set main as default branch name
git config --global init.defaultBranch main
```

Modern Git uses "main" instead of "master" as the default branch name.

### Text Editor

```bash
# VS Code (recommended for Angular)
git config --global core.editor "code --wait"

# VS Code (Windows)
git config --global core.editor "code --wait"

# VS Code (Mac)
git config --global core.editor "code --wait"

# If using code doesn't work, try:
git config --global core.editor "code"
```

### Line Endings

Different operating systems handle line endings differently:

```bash
# Windows (recommended)
git config --global core.autocrlf true

# Mac/Linux (recommended)
git config --global core.autocrlf input

# Or disable entirely
git config --global core.autocrlf false
```

### Color Output

```bash
# Enable color output
git config --global color.ui auto
```

### Pull Behavior

```bash
# Merge when pulling (default)
git config --global pull.merge true

# Or rebase when pulling
git config --global pull.rebase true
```

### Push Behavior

```bash
# Push current branch to matching branch on remote
git config --global push.default current

# Or set to simple (recommended)
git config --global push.default simple
```

## Angular-Specific Configuration

### Complete Recommended Setup

```bash
# Identity
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"

# Default branch
git config --global init.defaultBranch main

# Editor
git config --global core.editor "code --wait"

# Line endings (Windows)
git config --global core.autocrlf true

# Color
git config --global color.ui auto

# Pull behavior
git config --global pull.rebase false

# Push behavior
git config --global push.default simple
```

### Advanced Angular Settings

```bash
# Aliases for Angular workflow
git config --global alias.st status
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.ci commit

# Useful Angular-specific aliases
git config --global alias.last 'log -1 HEAD'
git config --global alias.unstage 'reset HEAD --'
```

## Editing Configuration Directly

### Viewing Global Configuration File

```bash
# Open global config in editor
git config --global --edit

# Example output:
[user]
    name = Jane Developer
    email = jane@angularpro.dev
[core]
    editor = code --wait
    autocrlf = true
[init]
    defaultBranch = main
[alias]
    st = status
    co = checkout
```

### Adding Configuration Manually

You can also edit the `.gitconfig` file directly:

```bash
# Windows
notepad %USERPROFILE%\.gitconfig

# Mac/Linux
nano ~/.gitconfig
```

## Repository-Specific Configuration

### When to Use Local Configuration

Some settings should be repository-specific:

```bash
# Go to your Angular project
cd my-angular-project

# Set repository name
git config-specific user user.name "Work Name"

# Verify it's set for this repository only
git config --local user.name

# View all local settings
git config --local --list
```

### Example: Different Emails for Different Projects

```bash
# Personal project
git config user.email "personal@email.com"

# Work project  
git config user.email "work@company.com"
```

## Best Practices

1. **Set identity immediately**: Configure user.name and user.email first
2. **Use global for personal settings**: Most settings should be global
3. **Use local for project-specific settings**: Override only when needed
4. **Share team configurations**: Use a `.gitconfig` template for teams
5. **Review your settings**: Run `git config --list` regularly

## Common Configuration Mistakes

### Mistake 1: Forgetting to Configure

```bash
# Without configuration, you'll see warnings
git commit -m "Initial commit"

# Warning:
*** Please tell me who you are.
# You need to set your identity first
```

### Mistake 2: Using Wrong Email

```bash
# Wrong email won't connect to GitHub properly
git config --global user.email "wrong@email.com"

# Correct email matches your GitHub account
git config --global user.email "correct@github.com"
```

### Mistake 3: Case Sensitivity Issues

```bash
# Git on Windows/macOS is case-insensitive by default
# But Linux servers might be case-sensitive

# Be consistent with branch names
git config --global core.ignorecase false
```

## Exercises for Students

### Exercise 1: Configure Your Git Environment
Set up your complete Git configuration:
1. Set your name and email
2. Choose your preferred editor
3. Configure endings for line your OS
4. Set default branch to "main"

### Exercise 2: Create Git Aliases
Create useful shortcuts:
```bash
git config --global alias.st status
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.ci commit
```

### Exercise 3: Explore Configuration
Run these commands and explain what each shows:
```bash
git config --list
git config --list --show-origin
git config --global --edit
```

## Mini Practice Tasks

### Task 1: Basic Configuration
```bash
# Set your identity
git config --global user.name "Your Name"
git config --global user.email "your@email.com"

# Verify
git config user.name
git config user.email
```

### Task 2: Configure for Angular
```bash
# Set default branch
git config --global init.defaultBranch main

# Set editor (VS Code)
git config --global core.editor "code --wait"

# Configure line endings for your OS
# Windows:
git config --global core.autocrlf true
# Mac/Linux:
git config --global core.autocrlf input
```

### Task 3: Create Useful Aliases
```bash
# Status short
git config --global alias.s "status -s"

# Show last commit
git config --global alias.last "log -1 HEAD"

# Unstage files
git config --global alias.unstage "reset HEAD --"

# Show all aliases
git config --global --get-regexp alias
```

### Task 4: View and Edit Configuration
```bash
# View all settings
git config --list

# Edit global config
git config --global --edit

# View specific setting origin
git config --show-origin user.email
```

## Summary

Git configuration is essential for effective version control:

- **Three levels**: System, Global, and Local configuration
- **Identity first**: Always set user.name and user.email
- **Editor selection**: Choose your preferred code editor
- **OS-specific settings**: Configure line endings appropriately
- **Aliases**: Create shortcuts for common commands

With proper configuration, your Git experience will be smooth and efficient, letting you focus on building great Angular applications.

---

**Next Lesson**: [Git Basic Workflow](./05_Git_Basic_Workflow.md)

**Previous Lesson**: [Installing Git](./03_Installing_Git.md)
