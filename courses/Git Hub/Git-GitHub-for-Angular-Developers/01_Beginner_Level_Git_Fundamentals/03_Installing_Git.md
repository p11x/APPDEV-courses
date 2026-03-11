# Installing Git

## Topic Title
Setting Up Git on Your Computer

## Concept Explanation

Installing Git is the first step to start using version control in your Angular projects. Git is available on all major operating systems: Windows, macOS, and Linux. The installation process varies slightly depending on your OS, but the outcome is the same - you'll have a fully functional Git environment.

### Why Git Installation Matters

Before you can start using Git commands, you need to have Git installed on your computer. This includes:

1. **The Git command-line tools**: The core commands we'll use throughout this course
2. **Git credential helper**: Helps manage authentication with remote repositories
3. **Default text editor**: For writing commit messages (optional but useful)

## Installing Git on Windows

### Method 1: Official Installer (Recommended)

**Step 1: Download Git**
1. Visit the official Git website: https://git-scm.com
2. Click on "Download for Windows"
3. The download should start automatically

**Step 2: Run the Installer**
1. Locate the downloaded file (usually in your Downloads folder)
2. Double-click to run the installer
3. Follow the installation wizard

**Step 3: Installation Options**

During installation, you'll see several screens:

```
┌────────────────────────────────────────────────────────┐
│                  Select Components                     │
├────────────────────────────────────────────────────────┤
│ ☑ Git Bash Here                                        │
│ ☑ Git LFS (Large File Support)                         │
│ ☑ Associate .git* with default editor                  │
│ ☑ Add Git icons to Quick Launch                        │
│ ☑ Use TrueType font in console                         │
└────────────────────────────────────────────────────────┘
```

Recommended options:
- Keep default selections
- Choose "Git Bash Here" for easy access
- Select your preferred code editor (VS Code recommended)

**Step 4: Choose Default Editor**
- VS Code (recommended for Angular)
- Notepad++
- Vim (advanced users)

**Step 5: Environment Variables**
Choose "Git from the command line and also from 3rd-party software"

### Method 2: Using Chocolatey

If you have Chocolatey package manager installed:

```powershell
# Open PowerShell as Administrator
choco install git
```

### Method 3: Using Winget

```powershell
# Open PowerShell or Command Prompt
winget install Git.Git
```

## Installing Git on macOS

### Method 1: Using Homebrew (Recommended)

**Step 1: Install Homebrew (if not installed)**
```bash
# Open Terminal and run
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
```

**Step 2: Install Git**
```bash
brew install git
```

### Method 2: Using Xcode Command Line Tools

**For macOS Catalina and later:**
```bash
# Open Terminal and run
git --version
```

This will prompt you to install the command line tools if they're not already installed.

**Step 1:** Click "Install" when prompted
**Step 2:** Wait for installation to complete
**Step 3:** Verify with `git --version`

### Method 3: Official Installer

1. Download from: https://git-scm.com/download/mac
2. Open the .dmg file
3. Follow the installation wizard

## Installing Git on Linux

### Debian/Ubuntu

```bash
# Update package list
sudo apt update

# Install Git
sudo apt install git

# Verify installation
git --version
```

### Fedora

```bash
# Install Git
sudo dnf install git

# Verify installation
git --version
```

### CentOS/RHEL

```bash
# Install Git
sudo yum install git

# Verify installation
git --version
```

### Arch Linux

```bash
# Install Git
sudo pacman -S git

# Verify installation
git --version
```

## Verifying Git Installation

After installation, verify that Git is properly installed:

```bash
# Check Git version
git --version
```

Expected output:
```
git version 2.43.0  (or newer version)
```

### Additional Verification

```bash
# Check Git help
git --help

# Check Git commands
git config --list
```

## Configuring Git for First Use

After installing Git, you need to configure your identity. This is important because every Git commit uses this information.

### Set Your Name

```bash
git config --global user.name "Your Name"
```

Example:
```bash
git config --global user.name "John Doe"
```

### Set Your Email

```bash
git config --global user.email "your.email@example.com"
```

Example:
```bash
git config --global user.email "john.doe@company.com"
```

### Verify Configuration

```bash
# View all configuration settings
git config --list

# View specific setting
git config user.name
git config user.email
```

## Angular Development Specific Setup

### Recommended Settings for Angular

```bash
# Set default branch name
git config --global init.defaultBranch main

# Set pull strategy
git config --global pull.rebase false

# Enable color output
git config --global color.ui auto

# Set default editor (VS Code)
git config --global core.editor "code --wait"

# On Windows, use Windows-style line endings
git config --global core.autocrlf true

# On Mac/Linux, use LF line endings
git config --global core.autocrlf input
```

### Angular-Specific .gitignore

We'll cover `.gitignore` in detail later, but for now, know that Angular projects need special handling for:
- `node_modules/` - npm packages
- `dist/` - Build output
- `.angular/` - Angular CLI cache

## Best Practices After Installation

1. **Configure your identity first**: Always set user.name and user.email
2. **Learn the basics**: Focus on init, add, commit, status, log
3. **Use Git Bash/Terminal**: Don't rely solely on GUI tools initially
4. **Practice daily**: Use Git even for small personal projects

## Common Installation Issues

### Issue 1: "git is not recognized"

**On Windows:**
- Restart your computer after installation
- Or, open a new terminal window
- Verify Git is in your PATH

### Issue 2: Permission Denied

**On Mac/Linux:**
```bash
# Fix permissions
sudo chown -R $(whoami) /usr/local
```

### Issue 3: Old Git Version

```bash
# Check version
git --version

# Update on Mac
brew upgrade git

# Update on Linux
sudo apt update && sudo apt upgrade git
```

## Exercises for Students

### Exercise 1: Install Git
Install Git on your computer using one of the methods described above. Take a screenshot of the successful installation.

### Exercise 2: Configure Git
Set up your Git identity:
- Your name
- Your email

Verify the configuration with `git config --list`.

### Exercise 3: Explore Git Documentation
After installation, explore Git's built-in documentation:
```bash
git help
git help -a
git help <command>
```

## Mini Practice Tasks

### Task 1: Windows Installation (if applicable)
1. Download Git from git-scm.com
2. Run the installer with default settings
3. Open Git Bash
4. Run `git --version`

### Task 2: macOS Installation (if applicable)
1. Open Terminal
2. Run `git --version`
3. If prompted, install Xcode Command Line Tools

### Task 3: Linux Installation (if applicable)
1. Open Terminal
2. Run distribution-specific install command
3. Verify with `git --version`

### Task 4: Configure Your Environment
```bash
# Set your identity
git config --global user.name "Your Name"
git config --global user.email "your@email.com"

# Set default branch
git config --global init.defaultBranch main

# Verify
git config --list
```

## Summary

Installing Git is the foundational step for version control. Remember:

- **Windows**: Use the official installer from git-scm.com
- **macOS**: Use Homebrew or Xcode Command Line Tools
- **Linux**: Use your distribution's package manager
- **Always verify**: Run `git --version` after installation
- **Configure immediately**: Set your name and email before starting

With Git installed and configured, you're ready to start version controlling your Angular projects!

---

**Next Lesson**: [Git Configuration](./04_Git_Configuration.md)

**Previous Lesson**: [Introduction to Git](./02_Introduction_to_Git.md)
