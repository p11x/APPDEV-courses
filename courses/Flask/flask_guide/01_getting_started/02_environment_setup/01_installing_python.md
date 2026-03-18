<!-- FILE: 01_getting_started/02_environment_setup/01_installing_python.md -->

## Overview

Before you can build Flask applications, you need **Python** installed on your computer. Python is the programming language that Flask runs on. This file guides you through checking if Python is already installed, and if not, installing it on Windows, macOS, or Linux. Flask requires Python 3.12 or later for the best experience with modern features.

## Prerequisites

- A computer running Windows, macOS, or Linux
- Administrator or user-level access to install software
- Basic familiarity with using the command line/terminal

## Core Concepts

### Why Python 3.12+?

Flask 3.x (the latest major version) requires Python 3.9 minimum, but Python 3.12 brings significant improvements:
- **Faster execution** — Python 3.12 is notably faster than previous versions
- **Better error messages** — Debugging is easier with improved tracebacks
- **Modern syntax features** — Pattern matching, improved f-strings, and more

### Checking if Python is Already Installed

Open your terminal (Command Prompt on Windows, Terminal on macOS/Linux) and type:

```bash
python --version
```

If Python is installed, you'll see output like `Python 3.12.2`. If you see an error or an older version (below 3.9), you need to install or update Python.

> **⚠️ Warning:** Some systems have `python3` instead of `python` as the command. Try `python3 --version` if `python` doesn't work.

## Code Walkthrough

### Installing Python on Windows

1. **Download Python** — Visit https://www.python.org/downloads/windows/
2. **Run the installer** — Double-click the downloaded `.exe` file
3. **Important checkboxes**:
   - ✅ **"Add Python to PATH"** — This lets you run Python from any terminal
   - ✅ **"Install pip"** — pip is Python's package manager (needed for Flask)
   - ✅ **"Install Python for all users"** — Optional, but recommended

4. **Verify installation** — Open a new Command Prompt and run:
```bash
python --version
# Expected output: Python 3.12.x
```

```bash
pip --version
# Expected output: pip 24.x
```

### Installing Python on macOS

**Option A — Using Homebrew (Recommended)**
```bash
# First, install Homebrew if you don't have it (paste in Terminal)
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Then install Python
brew install python
```

**Option B — Using the Installer**
1. Visit https://www.python.org/downloads/mac-osx/
2. Download the macOS 64-bit universal installer
3. Run the package and follow the wizard

**Verify installation:**
```bash
python3 --version
# Expected output: Python 3.12.x
```

### Installing Python on Linux

**Debian/Ubuntu:**
```bash
sudo apt update
sudo apt install python3 python3-pip python3-venv
```

**Fedora:**
```bash
sudo dnf install python3 python3-pip
```

**Arch Linux:**
```bash
sudo pacman -S python python-pip
```

**Verify installation:**
```bash
python3 --version
# Expected output: Python 3.12.x
```

### Verifying Your Setup

Create a simple test script to confirm everything works:

```python
# test_python.py — Simple test to verify Python installation
import sys  # Import sys module for system-related functionality
print(f"Python version: {sys.version}")  # Print the Python version
print(f"Executable path: {sys.executable}")  # Print where Python is installed

# Test pip
import subprocess  # Import subprocess to run shell commands
result = subprocess.run([sys.executable, "-m", "pip", "--version"], capture_output=True, text=True)
print(f"pip version: {result.stdout.strip()}")
```

Run this with:
```bash
python test_python.py
```

You should see version numbers printed, confirming Python and pip are working.

## Common Mistakes

❌ **Installing Python but forgetting to add it to PATH**
```bash
# WRONG — If Python was installed but not added to PATH, this fails
python --version
# 'python' is not recognized as an internal or external command
```

✅ **Correct — Check the "Add Python to PATH" checkbox during Windows installation, or reinstall**
```bash
# CORRECT — After proper installation
python --version
# Python 3.12.2
```

❌ **Using the wrong Python command on macOS/Linux**
```bash
# WRONG — On macOS/Linux, 'python' may refer to Python 2 (legacy)
python --version
# Python 2.7.18
```

✅ **Correct — Use python3 instead**
```bash
# CORRECT — Python 3 is accessed via python3
python3 --version
# Python 3.12.2
```

❌ **Installing Flask globally without understanding virtual environments**
```bash
# WRONG — Installing packages globally can cause version conflicts between projects
pip install flask
# This installs Flask system-wide, which can cause problems later
```

✅ **Correct — We will cover virtual environments in the next file (02_virtual_environments.md)**
```bash
# For now, just verify pip works
pip --version
```

## Quick Reference

| Command | Description |
|---------|-------------|
| `python --version` | Check Python version (Windows) |
| `python3 --version` | Check Python version (macOS/Linux) |
| `pip --version` | Check if pip is installed |
| `python -m pip --version` | Alternative way to check pip (works if pip is in PATH) |

## Next Steps

Now that Python is installed, continue to [02_virtual_environments.md](02_virtual_environments.md) to learn how to create isolated Python environments for each Flask project — an essential practice for professional development.