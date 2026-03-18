# Shell Basics: Bash and Zsh

## What You'll Learn

- What the shell is and how it works
- Bash vs Zsh comparison
- Variables and environment
- Command substitution
- Globbing and expansion
- Aliases and functions

## Prerequisites

- Completed `02-terminal-emulators.md`
- Comfortable with basic terminal commands

## What Is the Shell?

The shell is the program that interprets your commands:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    HOW THE SHELL WORKS                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  When you type "ls -la":                                                    │
│                                                                             │
│  ┌──────────────┐     ┌─────────────┐     ┌──────────────┐               │
│  │ You type     │ ──▶ │ Shell       │ ──▶ │ System Call  │               │
│  │ "ls -la"     │     │ parses cmd  │     │ to kernel    │               │
│  └──────────────┘     └─────────────┘     └──────────────┘               │
│                              │                                             │
│                              ▼                                             │
│                        ┌─────────────┐                                     │
│                        │ Executable  │                                     │
│                        │ /bin/ls     │                                     │
│                        └─────────────┘                                     │
│                              │                                             │
│                              ▼                                             │
│                        ┌─────────────┐                                     │
│                        │ Output      │                                     │
│                        │ to screen   │                                     │
│                        └─────────────┘                                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Bash vs Zsh

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    BASH VS ZSH                                               │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  BASH (Bourne Again Shell)                                                  │
│  • Default on Linux and Windows WSL                                        │
│  • POSIX compliant                                                         │
│  • Most compatible                                                         │
│  • Older, more established                                                 │
│                                                                             │
│  ZSH (Z Shell)                                                              │
│  • Default on macOS (since Catalina)                                      │
│  • More features out of the box                                           │
│  • Better completion                                                       │
│  • Oh My Zsh framework                                                    │
│                                                                             │
│  CHOOSE:                                                                   │
│  • macOS → Zsh (default)                                                  │
│  • Linux → Bash or Zsh                                                    │
│  • WSL → Bash (default)                                                   │
│  • Scripts → Bash (portability)                                          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Variables

### Setting Variables

```bash
# Create a variable
name="Alice"
age=30

# Print a variable
echo $name
echo ${name}

# Use in commands
echo "Hello, $name"
```

🔍 **What this does:**
- Variables are set with `name=value` (no spaces!)
- Access with `$name` or `${name}`
- Only available in current shell session

### Environment Variables

```bash
# Export to make available to child processes
export PATH=$PATH:/new/path

# View all environment variables
env
printenv

# View specific variable
echo $HOME
echo $PATH
echo $USER
```

## Globbing and Expansion

### Wildcards

```bash
# Match all .py files
ls *.py

# Match files starting with test
ls test*

# Match exactly one character
ls file?.txt

# Match character sets
ls [abc]*.txt

# Negation
ls !(*.py)
```

### Brace Expansion

```bash
# Create multiple files
touch file{1,2,3}.txt

# Create a sequence
touch page{1..10}.txt

# Copy to multiple locations
cp file.txt backup/ archive/ recent/
```

### Command Substitution

```bash
# Get command output
current_date=$(date)
echo "Today is $current_date"

# Or with backticks (older style)
current_date=`date`
```

### Arithmetic

```bash
# Calculate
result=$((5 + 3))
echo $result

# Variables
x=10
y=5
sum=$(($x + $y))
product=$(($x * $y))
```

## Aliases

Create shortcuts for common commands:

```bash
# Create alias
alias ll='ls -la'
alias la='ls -a'
alias gs='git status'
alias gp='git push'

# Make alias permanent (add to ~/.bashrc or ~/.zshrc)
echo "alias ll='ls -la'" >> ~/.bashrc
```

🔍 **What this does:**
- `ll` now runs `ls -la`
- Only works in interactive shells (not scripts)
- Add to config file to persist

## Functions

For more complex operations:

```bash
# Simple function
function greet {
    echo "Hello, $1!"
}

# Call it
greet Alice

# With return value
function add {
    echo $(($1 + $2))
}

result=$(add 5 3)
echo $result

# Or with local variables
function calculate {
    local x=$1
    local y=$2
    echo $(($x * $y))
}
```

## Control Structures

### If/Else

```bash
if [ $age -gt 18 ]; then
    echo "Adult"
elif [ $age -gt 13 ]; then
    echo "Teenager"
else
    echo "Child"
fi
```

### For Loop

```bash
# Loop through files
for file in *.txt; do
    echo "Processing $file"
done

# Or on one line
for f in *.txt; do echo "Processing $f"; done
```

### While Loop

```bash
counter=0
while [ $counter -lt 5 ]; do
    echo "Count: $counter"
    counter=$((counter + 1))
done
```

## Configuration Files

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CONFIG FILES                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  BASH:                                                                      │
│  ~/.bashrc       → Run for interactive non-login shells                   │
│  ~/.bash_profile → Run for login shells                                    │
│  ~/.bash_aliases → Aliases (often sourced from .bashrc)                   │
│                                                                             │
│  ZSH:                                                                       │
│  ~/.zshrc        → Main config                                            │
│  ~/.zshenv       → Always sourced                                           │
│  ~/.zprofile     → Login shells                                             │
│                                                                             │
│  Apply changes:                                                             │
│  source ~/.bashrc  (or . ~/.bashrc)                                        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Shebang

The first line of shell scripts:

```bash
#!/bin/bash
# This script does something

echo "Hello from bash!"

#!/bin/zsh
# This script uses zsh

echo "Hello from zsh!"

#!/usr/bin/env python
# This is actually Python!
print("Hello from Python!")
```

🔍 **What this does:**
- Tells the system which interpreter to use
- `#!/bin/bash` = use Bash
- `#!/usr/bin/env python` = find Python in PATH and use it

## Common Patterns

```bash
# Check if file exists
if [ -f "file.txt" ]; then
    echo "File exists"
fi

# Check if directory exists
if [ -d "folder" ]; then
    echo "Folder exists"
fi

# Check if command exists
if command -v git >/dev/null 2>&1; then
    echo "Git is installed"
fi

# Default value
value=${var:-default}

# String length
length=${#variable}
```

## Summary

- The shell interprets your commands
- Bash is standard on Linux, Zsh is default on macOS
- Variables: `name=value`, access with `$name`
- Use aliases for shortcuts, functions for complex logic
- Add aliases/functions to ~/.bashrc or ~/.zshrc

## Next Steps

→ Continue to `04-text-editors-and-ides.md` to learn about choosing code editors.
