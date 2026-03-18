# Command Line Basics

## What You'll Learn

- Why the command line matters for developers
- Essential commands for navigation and files
- Working with directories
- File operations (create, copy, move, delete)
- Using flags and arguments
- Combining commands with pipes

## Prerequisites

None—this is an introduction for those new to the terminal.

## Why the Command Line?

Modern IDEs have buttons for everything, so why learn the command line?

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHY USE THE COMMAND LINE                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ POWER:                                                                  │
│  • Many tools are CLI-only                                                │
│  • Can automate repetitive tasks                                          │
│  • Better control over operations                                         │
│                                                                             │
│  ✅ EFFICIENCY:                                                            │
│  • Faster than clicking through menus                                     │
│  • Can repeat commands easily                                             │
│  • History and autocomplete                                               │
│                                                                             │
│  ✅ PORTABILITY:                                                           │
│  • Commands work the same everywhere                                      │
│  • Scripts work on any machine                                            │
│  • No GUI required (server work)                                          │
│                                                                             │
│  ✅ DEVELOPER TOOLS:                                                       │
│  • Git, Docker, npm, pip all use CLI                                      │
│  • Most developer tools are CLI-first                                      │
│  • Cloud platforms (AWS, GCP, Azure) are CLI-driven                        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Opening the Terminal

### Windows

- **PowerShell**: Start menu → "PowerShell"
- **Command Prompt**: Start menu → "cmd"
- **Windows Terminal**: Microsoft Store → "Windows Terminal" (recommended)

### macOS

- **Terminal**: Applications → Utilities → Terminal
- **iTerm2**: Better alternative (iterm2.com)

### Linux

- **GNOME Terminal**: Ctrl+Alt+T
- **Konsole**: KDE default
- **Alacritty**: Modern, fast

## Navigation Commands

```bash
# Where am I?
pwd                     # Print working directory

# What's in this folder?
ls                      # List files
ls -l                   # Long format (details)
ls -a                   # Show hidden files
ls -la                  # Combined

# Change directory
cd folder-name          # Go into folder
cd ..                   # Go up one level
cd ~                    # Go to home directory
cd -                    # Go to previous location
```

🔍 **What this does:**
- `pwd` — Shows your current full path (e.g., `/home/user/projects`)
- `ls` — Lists files in current directory
- `cd` — Changes your working directory

## Working with Files

```bash
# Create files
touch newfile.txt       # Create empty file
touch file1.txt file2.txt  # Multiple files

# View file contents
cat file.txt            # Print entire file
head file.txt           # First 10 lines
tail file.txt           # Last 10 lines
tail -f file.txt        # Follow (live updates)

# Copy files
cp original.txt copy.txt
cp file.txt destination/
cp file1.txt file2.txt backup/

# Move/rename files
mv oldname.txt newname.txt
mv file.txt destination/

# Delete files
rm file.txt
rm file1.txt file2.txt
rm -r folder/          # Delete folder and contents
```

## Working with Directories

```bash
# Create directories
mkdir newfolder
mkdir -p path/to/nested/folder  # Create parents

# Remove directories
rmdir empty-folder     # Only if empty
rm -rf folder/        # Force delete (careful!)

# Copy directories
cp -r source/ dest/

# Move directories
mv source/ dest/
```

## Flags and Arguments

Commands often have options (flags):

```bash
ls -l -a -h
ls -lah                    # Combined flags

# Meaning:
# -l = long format
# -a = all files (including hidden)
# -h = human-readable sizes
```

Common patterns:

```bash
command -flag argument
command --long-flag argument

# Examples:
python --version
pip install --upgrade package
git log --oneline --graph
```

## Pipes and Redirection

### Pipes (|)

Send output of one command as input to another:

```bash
# Count lines in a file
cat file.txt | wc -l

# Find specific text
cat file.txt | grep "search"

# List files, sort alphabetically
ls | sort

# Find processes, filter
ps aux | grep python
```

🔍 **What this does:**
- The pipe `|` takes the output from the left command
- Feeds it as input to the right command

### Redirection (> and >>)

```bash
# Save output to file (overwrite)
echo "hello" > file.txt

# Append to file
echo "world" >> file.txt

# Save errors
command 2> errors.txt

# Save both output and errors
command > output.txt 2>&1
```

## Combining Commands

```bash
# Multiple commands on one line
cd folder && ls -la

# Run second command if first succeeds
mkdir newproject && cd newproject

# Or (run second even if first fails)
cd folder || echo "Folder doesn't exist"
```

## Useful Shortcuts

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    KEYBOARD SHORTCUTS                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Tab              → Autocomplete commands/files                             │
│  Ctrl+C          → Cancel current command                                  │
│  Ctrl+Z          → Suspend (background) process                             │
│  Ctrl+L          → Clear screen                                           │
│  Ctrl+U          → Clear current line                                      │
│  Ctrl+A          → Move to beginning of line                               │
│  Ctrl+E          → Move to end of line                                     │
│  Up/Down arrows  → Navigate command history                                │
│  Ctrl+R          → Search command history                                 │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Finding Help

```bash
# Get help for a command
command --help
man command           # Manual pages
info command          # More detailed info
```

## Summary

- The command line gives you power and efficiency
- Core navigation: `pwd`, `ls`, `cd`
- File operations: `touch`, `cat`, `cp`, `mv`, `rm`
- Directory operations: `mkdir`, `rmdir`
- Combine commands with pipes (`|`)
- Redirect output with `>` and `>>`

## Next Steps

→ Continue to `02-terminal-emulators.md` to learn about choosing and customizing your terminal.
