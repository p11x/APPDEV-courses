# Productivity Tools

## What You'll Learn

- CLI tools that boost productivity
- Fuzzy finders
- File search
- Git helpers
- Time-saving aliases

## Prerequisites

- Completed `07-debugging-tools.md`
- Comfortable with the terminal

## Must-Have CLI Tools

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    ESSENTIAL CLI TOOLS                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  FUZZY FINDERS:                                                             │
│  • fzf — Fast fuzzy finder                                                 │
│  • skim — Rust-based alternative                                           │
│                                                                             │
│  FILE SEARCH:                                                               │
│  • fd — Fast alternative to find                                          │
│  • ripgrep (rg) — Fast grep alternative                                    │
│                                                                             │
│  Git:                                                                       │
│  • gh — GitHub CLI                                                         │
│  • gitui — Terminal Git UI                                                 │
│                                                                             │
│  UTILITIES:                                                                 │
│  • bat — Better cat                                                        │
│  • exa — Better ls                                                         │
│  • tldr — Simplified man pages                                             │
│  • httpie — Human-friendly curl                                            │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Fuzzy Finders

### fzf

Install:

```bash
# macOS
brew install fzf

# Linux
apt install fzf

# Windows (via scoop or WSL)
scoop install fzf
```

Use:

```bash
# Search files
fzf

# Search history
Ctrl+R

# Search processes
Alt+C (cd into folder)
```

### Vim integration:

```vim
" Add to .vimrc
set rtp+=/usr/local/opt/fzf/vim
```

## Fast File Search: fd

Better than `find`:

```bash
# Install
brew install fd                              # macOS
apt install fd-find                          # Linux

# Basic usage
fd                      # List all files
fd "\.py$"             # Find Python files
fd -e py               # Files with .py extension
fd -d 2                # Max 2 directories deep
fd -H                  # Include hidden files

# Search in specific directory
fd pattern /path/to/dir

# Execute command on results
fd -e py -x py_compile {}
```

## ripgrep (rg)

Better than `grep`:

```bash
# Install
brew install ripgrep                         # macOS
apt install ripgrep                          # Linux

# Basic usage
rg "pattern"                    # Search current dir
rg "pattern" file.py            # Search specific file
rg "pattern" src/               # Search directory

# Options
rg -l "pattern"                 # Just filenames
rg -n "pattern"                 # Line numbers
rg -c "pattern"                 # Count matches
rg -w "pattern"                 # Whole words

# File types
rg --type py "pattern"          # Python files
rg --type-not py "pattern"      # Non-Python files

# Ignore patterns
rg "pattern" --ignore-case     # Case insensitive
```

## Better CLI Tools

### bat (Better cat)

```bash
# Install
brew install bat                              # macOS
apt install bat                               # Linux

# Use instead of cat
bat file.py

# Features:
# - Syntax highlighting
# - Line numbers
# - Git integration
# - File concatenation
```

### exa (Modern ls)

```bash
# Install
brew install exa                               # macOS
apt install exa                               # Linux

# Use instead of ls
exa
exa -l                # Long format
exa -la               # All files
exa --tree            # Tree view
```

### tldr (Simplified man pages)

```bash
# Install
brew install tldr                             # macOS
apt install tldr                              # Linux

# Use
tldr tar
tldr rsync
```

## Git Tools

### GitHub CLI (gh)

```bash
# Install
brew install gh                               # macOS
apt install gh                                # Linux

# Login
gh auth login

# Create PR
gh pr create --title "Add feature" --body "Description"

# View PRs
gh pr list
gh pr view 123

# Review PR
gh pr checkout 123
gh pr review 123 --approve

# Issues
gh issue create
gh issue list
```

### gitui (Terminal Git UI)

```bash
# Install
brew install gitui                             # macOS

# Run
gitui
```

## HTTP Tools

### HTTPie

```bash
# Install
brew install httpie                            # macOS
apt install httpie                            # Linux

# Simple requests
http GET api.example.com/users
http POST api.example.com/users name=Alice

# With headers
http GET api.example.com Authorization:"Bearer token"

# JSON
http POST api.example.com/users name=Alice email=alice@example.com
```

## Productivity Aliases

Add to your `.bashrc` or `.zshrc`:

```bash
# Git
alias gs='git status'
alias ga='git add'
alias gc='git commit -m'
alias gp='git push'
alias gl='git pull'
alias gd='git diff'
alias gb='git branch'
alias gco='git checkout'
alias glog='git log --oneline --graph --all'

# Navigation
alias ..='cd ..'
alias ...='cd ../..'
alias ....='cd ../../..'

# List
alias ll='exa -l'
alias la='exa -la'
alias lt='exa --tree'

# Safety
alias rm='rm -i'
alias cp='cp -i'
alias mv='mv -i'

# Quick navigation
alias projects='cd ~/projects'
alias code='cd ~/code'

# Python
alias venv='python -m venv venv && source venv/bin/activate'
alias pipup='pip list --outdated | cut -d " " -f1 | xargs pip install -U'
```

## tmux

Terminal multiplexer:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    TMUX                                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  WHAT IT DOES:                                                              │
│  • Multiple terminal windows in one                                       │
│  • Split panes (horizontal/vertical)                                       │
│  • Sessions persist (detached)                                             │
│  • Works over SSH                                                          │
│                                                                             │
│  INSTALL:                                                                  │
│  brew install tmux          # macOS                                       │
│  apt install tmux           # Linux                                       │
│                                                                             │
│  BASIC COMMANDS:                                                            │
│  tmux new -s session        # New session                                  │
│  Ctrl+b d                   # Detach                                       │
│  tmux attach                # Attach                                       │
│  Ctrl+b %                   # Split vertical                              │
│  Ctrl+b "                   # Split horizontal                            │
│  Ctrl+b arrow               # Navigate panes                              │
│  Ctrl+b c                   # New window                                   │
│                                                                             │
│  KEY CONCEPTS:                                                              │
│  • Sessions (collection of windows)                                        │
│  • Windows (tabs)                                                           │
│  • Panes (split views)                                                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## tmuxinator

Template manager for tmux:

```bash
# Install
gem install tmuxinator

# Create config
mkdir ~/.tmuxinator
touch ~/.tmuxinator/project.yml
```

```yaml
# ~/.tmuxinator/project.yml
name: project
root: ~/projects/myapp
windows:
  - editor: vim
  - server: python manage.py runserver
  - tests: pytest
```

## Summary

- fzf: Fuzzy file and command finder
- fd: Fast file search
- rg: Fast grep alternative
- bat: Better cat
- gh: GitHub CLI
- httpie: Human-friendly HTTP
- tmux: Terminal multiplexer

## Next Steps

→ Continue to `09-shell-scripting-basics.md` to learn how to automate tasks with shell scripts.
