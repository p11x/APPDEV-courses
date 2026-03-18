<!-- FILE: 01_getting_started/03_first_app/02_debug_mode.md -->

## Overview

Flask's **debug mode** is a powerful development feature that automatically restarts your server when code changes are detected and provides detailed error pages when exceptions occur. Understanding debug mode is essential for efficient development, but it also comes with critical security considerations that you must understand before deploying to production.

## Prerequisites

- A working Flask application (from the previous file)
- Understanding of how to run a Flask development server
- Basic knowledge of what exceptions and tracebacks are

## Core Concepts

### What Debug Mode Does

When you set `debug=True` in `app.run()`, Flask enables two key features:

1. **Auto-Reload (Live Reload)**
   - Flask monitors your source files for changes
   - When you save a file (like `app.py` or a template), Flask automatically restarts the server
   - You see changes immediately without manually stopping and starting the server
   - This speeds up development significantly

2. **Interactive Debugger**
   - When an exception occurs, Flask displays a detailed error page in your browser
   - The error page shows the full traceback, allowing you to inspect variables at each step
   - You can open a Python shell directly in the browser to test code in the context of the error
   - This makes debugging much easier than reading terminal output alone

### The Debugger Interface

When an error occurs in debug mode, you see a Flask error page with:
- The exception type (e.g., `NameError`, `TypeError`)
- The full traceback showing exactly where the error occurred
- Each frame of the call stack with line numbers and source code
- A Python interactive console where you can execute code in the context of the error

### Security Warning

> **⚠️ Warning:** Debug mode MUST be disabled in production. The interactive debugger allows anyone who can access your site to execute arbitrary Python code on your server. This is a severe security vulnerability.

When Flask detects it is running in production (not on localhost), it disables the interactive debugger automatically. However, you should never rely on this — always explicitly set `debug=False` for production.

## Code Walkthrough

### A Simple Application with an Error

Create an application that intentionally has an error to see the debugger in action:

```python
# debug_app.py — Application to demonstrate debug mode
from flask import Flask, request

app = Flask(__name__)

# This route has a bug — it tries to use an undefined variable
@app.route("/calculate")
def calculate():
    """Attempt to calculate a value (with a bug)."""
    # Bug: 'result' is misspelled as 'resul'
    x = int(request.args.get("x", 0))  # Get 'x' from query string, default to 0
    y = int(request.args.get("y", 0))  # Get 'y' from query string, default to 0
    resul = x + y  # Typo: 'result' would be correct, but we typed 'resul'
    return f"The sum is: {result}"  # Error: 'result' is not defined

# This route works correctly
@app.route("/working")
def working():
    """A working route."""
    return "This route works correctly!"

if __name__ == "__main__":
    # Run with debug mode enabled
    app.run(debug=True)
```

### Running and Testing

1. Run the application: `python debug_app.py`
2. Visit `http://127.0.0.1:5000/working` — This works fine
3. Visit `http://127.0.0.1:5000/calculate?x=5&y=3` — This triggers the error

You will see a detailed Flask error page showing:
- `NameError: name 'result' is not defined`
- The line causing the error (line 15)
- The full call stack

### Enabling Debug Mode with Environment Variables

The recommended way to enable debug mode is using an environment variable:

```python
# app.py — Using environment variables for debug mode
import os  # Import os module to access environment variables
from flask import Flask

app = Flask(__name__)

# Get DEBUG setting from environment, default to False
# This allows you to control debug mode without changing code
debug_mode = os.environ.get("FLASK_DEBUG", "False").lower() in ("true", "1", "yes")

@app.route("/")
def index():
    return f"Debug mode is: {debug_mode}"

if __name__ == "__main__":
    # Run with the configured debug setting
    app.run(debug=debug_mode)
```

Run with debug mode enabled:
```bash
# Windows (Command Prompt)
set FLASK_DEBUG=True
python app.py

# Windows (PowerShell)
$env:FLASK_DEBUG="True"
python app.py

# macOS/Linux
FLASK_DEBUG=True python app.py
```

### Using flask run Command

Flask provides a `flask run` command that is smarter than `python app.py`:

```bash
# Run with debug mode via flask run
FLASK_DEBUG=True flask run

# Or set environment variable first
export FLASK_DEBUG=True
flask run
```

The `flask run` command:
- Automatically enables debug mode if `FLASK_ENV=development` (deprecated in Flask 2.3+) or `FLASK_DEBUG=1`
- Loads configuration from environment variables and `.env` files
- Is the recommended way to run Flask in development

## Common Mistakes

❌ **Leaving debug mode enabled in production**
```python
# WRONG — This is a serious security vulnerability!
app.run(debug=True)  # NEVER do this in production
# The interactive debugger lets attackers execute code on your server
```

✅ **Correct — Use environment-based configuration**
```python
# CORRECT — Use environment variable to control debug mode
import os
app.run(debug=os.environ.get("FLASK_DEBUG", "False") == "True")
# Or just:
app.run(debug=False)  # Always False for production
```

❌ **Not restarting the server after fixing errors**
```bash
# WRONG — If auto-reload isn't working, changes won't show
# You keep seeing the old error even after fixing the code
```

✅ **Correct — Check the terminal for reloader status**
```bash
# CORRECT — The terminal shows when the server restarts
# Look for messages like "Detected change in ..., reloading"
```

❌ **Using debug mode with the reloader on certain IDEs**
```python
# POTENTIAL ISSUE — Some IDEs have their own debuggers that conflict
# If you have issues, try running from the terminal instead
```

## Quick Reference

| Setting | Description |
|---------|-------------|
| `debug=True` | Enables auto-reload and interactive debugger (development only) |
| `debug=False` | Disables debug features (production default) |
| `FLASK_DEBUG=True` | Environment variable to enable debug mode |
| `flask run` | Recommended command to run the application |
| `CTRL+C` | Stop the development server |

## Next Steps

Now that you understand debug mode, continue to [03_project_structure.md](03_project_structure.md) to learn how to organize your Flask project files properly as it grows beyond a single file.