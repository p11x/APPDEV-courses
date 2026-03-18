# Shell Scripting Basics

## What You'll Learn

- Creating and running shell scripts
- Script structure and shebang
- Variables and arguments
- Conditionals and loops
- Functions in scripts
- Error handling

## Prerequisites

- Completed `08-productivity-tools.md`
- Comfortable with command line

## Your First Script

Create a file called `hello.sh`:

```bash
#!/bin/bash

echo "Hello, World!"
```

Make it executable:

```bash
chmod +x hello.sh
./hello.sh
```

🔍 **What this does:**
- `#!/bin/bash` — Shebang, tells OS to use Bash
- `echo` — Prints text to terminal
- `chmod +x` — Makes file executable

## Script Structure

```bash
#!/bin/bash
# Comments start with #

# Description: This script does X
# Author: Your Name
# Date: 2024

# Exit on error
set -e

# Main code here
echo "Starting..."
```

## Variables

```bash
#!/bin/bash

# Create variables
name="Alice"
age=30

# Use variables
echo "Name: $name"
echo "Age: $age"

# Command output
current_date=$(date)
echo "Today: $current_date"

# Arithmetic
result=$((5 + 3))
echo "Result: $result"
```

### Special Variables

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SPECIAL VARIABLES                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  $0          → Script name                                                  │
│  $1, $2...   → Positional arguments                                         │
│  $@          → All arguments                                                │
│  $#          → Number of arguments                                          │
│  $?          → Exit status of last command                                  │
│  $$          → Process ID                                                   │
│  $USER       → Current user                                                 │
│  $HOME       → Home directory                                               │
│  $PWD        → Current directory                                            │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Arguments

```bash
#!/bin/bash
# script.sh arg1 arg2 arg3

echo "Script: $0"
echo "First arg: $1"
echo "Second arg: $2"
echo "All args: $@"
echo "Number of args: $#"
```

## Conditionals

### If/Else

```bash
#!/bin/bash

name="Alice"

if [ "$name" = "Alice" ]; then
    echo "Hello, Alice!"
elif [ "$name" = "Bob" ]; then
    echo "Hello, Bob!"
else
    echo "Hello, stranger!"
fi
```

### File Tests

```bash
# Check if file exists
if [ -f "file.txt" ]; then
    echo "File exists"
fi

# Check if directory
if [ -d "folder" ]; then
    echo "Folder exists"
fi

# Check if empty
if [ -z "$var" ]; then
    echo "Variable is empty"
fi
```

### Number Comparisons

```bash
x=10
y=20

if [ $x -eq $y ]; then  # Equal
if [ $x -ne $y ]; then  # Not equal
if [ $x -lt $y ]; then  # Less than
if [ $x -le $y ]; then  # Less than or equal
if [ $x -gt $y ]; then  # Greater than
if [ $x -ge $y ]; then  # Greater than or equal
```

## Loops

### For Loop

```bash
#!/bin/bash

# Loop through files
for file in *.txt; do
    echo "Processing $file"
done

# Loop through numbers
for i in {1..5}; do
    echo "Number: $i"
done

# Loop through arguments
for arg in "$@"; do
    echo "Arg: $arg"
done
```

### While Loop

```bash
#!/bin/bash

counter=1

while [ $counter -le 5 ]; do
    echo "Count: $counter"
    counter=$((counter + 1))
done

# Read file line by line
while IFS= read -r line; do
    echo "Line: $line"
done < file.txt
```

## Functions

```bash
#!/bin/bash

# Define function
greet() {
    local name="$1"  # First argument
    echo "Hello, $name!"
}

# Call function
greet Alice
greet Bob

# Function with return
add() {
    local a=$1
    local b=$2
    echo $((a + b))
}

result=$(add 5 3)
echo "Result: $result"
```

## Error Handling

```bash
#!/bin/bash

set -e  # Exit on error
set -u  # Exit on undefined variable
set -o pipefail  # Pipeline fails if any part fails

# Or handle errors explicitly
if ! command -v python >/dev/null 2>&1; then
    echo "Python is required but not installed"
    exit 1
fi

# Try/catch equivalent
command_that_might_fail() {
    echo "Trying..."
    false  # This fails
    return 0  # Won't reach here with set -e
}

if command_that_might_fail; then
    echo "Success"
else
    echo "Failed"
    exit 1
fi
```

## Practical Examples

### Backup Script

```bash
#!/bin/bash
# backup.sh

set -e

BACKUP_DIR="/tmp/backups"
SOURCE_DIR="$1"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)

if [ -z "$SOURCE_DIR" ]; then
    echo "Usage: $0 <directory-to-backup>"
    exit 1
fi

mkdir -p "$BACKUP_DIR"

backup_file="$BACKUP_DIR/backup_$TIMESTAMP.tar.gz"

tar -czf "$backup_file" "$SOURCE_DIR"

echo "Backup created: $backup_file"

# Keep only last 7 backups
cd "$BACKUP_DIR"
ls -t | tail -n +8 | xargs -r rm

echo "Cleanup complete"
```

### Deploy Script

```bash
#!/bin/bash
# deploy.sh

set -e

echo "=== Deploying application ==="

# Pull latest code
echo "Pulling latest code..."
git pull origin main

# Install dependencies
echo "Installing dependencies..."
pip install -r requirements.txt

# Run migrations
echo "Running migrations..."
python manage.py migrate

# Restart service
echo "Restarting service..."
sudo systemctl restart gunicorn

echo "=== Deploy complete ==="
```

## Best Practices

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SHELL SCRIPT BEST PRACTICES                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ ALWAYS:                                                                 │
│  • Use shebang                                                             │
│  • Quote variables ("$var")                                               │
│  • Use set -e for error handling                                          │
│  • Add comments                                                            │
│  • Test on minimal system                                                  │
│                                                                             │
│  ✅ USUALLY:                                                               │
│  • Use local variables in functions                                       │
│  • Check arguments exist                                                   │
│  • Use meaningful variable names                                           │
│  • Keep scripts small and focused                                          │
│                                                                             │
│  ❌ AVOID:                                                                 │
│  • Unquoted variables (breaks on spaces)                                  │
│  •set -x in production (debug only)                                      │
│  • Parsing ls output (use globbing)                                       │
│  • Using backticks (use $())                                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- Scripts start with shebang `#!/bin/bash`
- Variables: `name=value`, use `$name`
- Arguments: `$1`, `$2`, `$@`
- Conditionals: `if [ ]; then ... fi`
- Loops: `for`, `while`
- Functions: define with `name() { }`
- Always use error handling (`set -e`)

## Next Steps

This completes the Terminal and Developer Tooling folder. You now have a comprehensive understanding of:

- Command line basics
- Terminal emulators
- Shell scripting (Bash/Zsh)
- Text editors and IDEs
- Package managers
- Environment managers
- Debugging tools
- Productivity tools
- Shell scripting

Continue to other folders in this guide to build complete Python web development skills!
