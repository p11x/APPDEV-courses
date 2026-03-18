# Installing Python

## What You'll Learn

- How to install Python 3.12+ on Windows, macOS, and Linux
- How to verify your Python installation
- What the REPL is and how to use it
- How to run your first Python command

## Prerequisites

- Read [01_what_is_python.md](./01_what_is_python.md) first

## Installing Python on Windows

### Step 1: Download the Installer

1. Visit [python.org/downloads](https://python.org/downloads)
2. Click the **"Download Python 3.12+"** button (or latest version)
3. Wait for the installer to download

### Step 2: Run the Installer

1. Open the downloaded file (e.g., `python-3.12.x.exe`)
2. **Important**: Check the box that says **"Add Python to PATH"**
3. Click **"Install Now"**
4. Wait for installation to complete
5. Click **"Close"** when done

### Step 3: Verify Installation

Open Command Prompt and type:

```cmd
python --version
```

You should see output like:
```
Python 3.12.0
```

## Installing Python on macOS

### Option 1: Download from Python.org

1. Visit [python.org/downloads](https://python.org/downloads)
2. Download the macOS installer
3. Run the package and follow the instructions

### Option 2: Install with Homebrew (Recommended)

If you have Homebrew installed:

```bash
# Install Python using Homebrew
brew install python

# Verify installation
python3 --version
```

You should see:
```
Python 3.12.0
```

### Note: python vs python3

On macOS (and Linux), the command is `python3` not `python`. Some systems have both Python 2 (legacy) and Python 3 installed.

## Installing Python on Linux

### Debian/Ubuntu

```bash
# Update package list
sudo apt update

# Install Python
sudo apt install python3 python3-pip

# Verify installation
python3 --version
```

### Fedora

```bash
sudo dnf install python3

# Verify installation
python3 --version
```

### Arch Linux

```bash
sudo pacman -S python

# Verify installation
python3 --version
```

### Verify pip is installed

```bash
pip3 --version
```

You should see something like:
```
pip 24.0 from /usr/lib/python3.12/site-packages/pip (python 3.12)
```

## What Is the REPL?

The **REPL** (Read-Eval-Print Loop) is an interactive environment where you can type Python code and see immediate results. It's perfect for experimenting and learning.

### How to Access the REPL

Open your terminal and type:

```bash
python3
```

or on Windows:
```cmd
python
```

You'll see something like:
```
Python 3.12.0 (main, ...) 
Type "help", "copyright", "credits" or "license" for more information.
>>>
```

The `>>>` is the **prompt** — it's waiting for you to type Python code!

### Your First REPL Session

Try typing these commands:

```python
# A simple math calculation
>>> 2 + 2
4

# A greeting
>>> print("Hello, World!")
Hello, World!

# Using a variable
>>> name = "Python Learner"
>>> print(f"My name is {name}")
My name is Python Learner

# Basic math functions
>>> pow(2, 8)
256

# Exit the REPL
>>> exit()
```

### Quick REPL Reference

| Command | What It Does |
|---------|--------------|
| `exit()` or `quit()` | Exit the REPL |
| `Ctrl+D` (Unix) / `Ctrl+Z` (Windows) | Exit the REPL |
| `Ctrl+L` | Clear the screen |
| `help()` | Enter interactive help |
| `Ctrl+C` | Interrupt current execution |

### Why Use the REPL?

- **Quick testing** - Test small code snippets instantly
- **Learning** - Experiment and see results immediately
- **Debugging** - Check how functions behave
- **Exploration** - Try out new libraries

## Using the REPL for Calculations

The REPL is great for quick calculations:

```python
# Basic math
>>> 10 + 5
15
>>> 10 - 5
5
>>> 10 * 5
50
>>> 10 / 5
2.0

# More complex
>>> (10 + 5) * 2
30
>>> 2 ** 10  # 2 to the power of 10
1024
>>> 17 // 5  # Integer division
3
>>> 17 % 5   # Remainder
2
```

## Running Python Scripts

Besides the REPL, you can write Python code in a file and run it:

### Step 1: Create a Python File

Create a new file called `hello.py` with this content:

```python
print("Hello from a file!")
```

### Step 2: Run the Script

```cmd
# On Windows
python hello.py
```

```bash
# On macOS/Linux
python3 hello.py
```

Output:
```
Hello from a file!
```

## Troubleshooting Common Issues

### "python is not recognized"

- **Windows**: Make sure you checked "Add Python to PATH" during installation
- Restart your terminal after installation
- Or use the full path: `C:\Python312\python.exe hello.py`

### "python3: command not found"

- **macOS**: Use `python3` instead of `python`
- **Linux**: Install with your package manager (see above)

### Multiple Python Versions

If you have multiple versions:

```bash
# Check which Python is being used
which python3

# Check all Python versions
ls -la /usr/bin/python*

# Create an alias (add to ~/.bashrc or ~/.zshrc)
alias python=python3
```

## Summary

- **Windows**: Download installer from python.org, check "Add to PATH"
- **macOS**: Use Homebrew (`brew install python`) or download installer
- **Linux**: Use your package manager (`apt`, `dnf`, `pacman`)
- **Verify**: Run `python --version` or `python3 --version`
- **REPL**: Interactive environment for testing Python code
- **Run scripts**: `python your_file.py`

## Next Steps

Now that Python is installed, head to **[03_your_first_script.md](./03_your_first_script.md)** to write your first Python script and understand every part of it.
