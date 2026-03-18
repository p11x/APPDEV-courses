<!-- FILE: 01_getting_started/02_environment_setup/03_installing_flask.md -->

## Overview

Now that you have Python installed and understand virtual environments, it is time to install **Flask** itself. Flask is a Python package (a collection of modules) that you install via pip, Python's package manager. This file guides you through installing Flask, verifying the installation, and understanding what gets installed alongside it.

## Prerequisites

- Python 3.9+ installed on your computer
- A virtual environment set up (from the previous file)
- Basic familiarity with the terminal/command line

## Core Concepts

### What is pip?

**pip** (Pip Installs Packages) is Python's default package manager. It downloads and installs packages from the **Python Package Index (PyPI)**, a vast repository of open-source Python libraries. When you run `pip install flask`, pip:

1. Connects to PyPI (https://pypi.org)
2. Downloads the Flask package and its dependencies
3. Installs them into your current Python environment (globally or in a virtual environment)

### Flask and Its Dependencies

When you install Flask, pip also installs several other packages that Flask depends on:
- **Werkzeug** — WSGI utilities and request/response handling
- **Jinja2** — Template engine for rendering HTML
- **Click** — Command-line interface framework
- **itsdangerous** — Cryptographic signing utilities

These are bundled together as the "Werkzeug stack" and form the foundation Flask is built upon.

### Flask [async] Extension

Flask 3.x supports asynchronous views. To use async features, you need to install Flask with the async extra:

```bash
pip install flask[async]
```

This installs additional packages needed for async support.

> **💡 Tip:** For most beginner projects, `pip install flask` is sufficient. The `[async]` extra is only needed if you want to use Python's `async`/`await` syntax in your view functions.

## Code Walkthrough

### Step 1: Activate Your Virtual Environment

Before installing Flask, make sure your virtual environment is active:

**On Windows (Command Prompt):**
```bash
venv\Scripts\activate
```

**On macOS/Linux:**
```bash
source venv/bin/activate
```

You should see `(venv)` at the beginning of your terminal prompt.

### Step 2: Install Flask

```bash
pip install flask
```

You will see output like this:
```
Collecting flask
  Downloading Flask-3.0.0-py3-none-any.whl (78 kB)
Collecting Werkzeug>=3.0.0 (from flask)
  Downloading Werkzeug-3.0.0-py3-none-any.whl (266 kB)
Collecting Jinja2>=3.1.2 (from flask)
  Downloading Jinja2-3.1.2-py3-none-any.whl (133 kB)
Collecting itsdangerous>=2.1.2 (from flask)
  Downloading itsdangerous-2.1.2-py3-none-any.whl (19 kB)
Collecting click>=8.1.3 (from flask)
  Downloading click-8.1.3-py3-none-any.whl (84 kB)
Installing collected packages: werkzeug, jinja2, itsdangerous, click, flask
Successfully installed click-8.1.3 flask-3.0.0 itsdangerous-2.1.2 jinja2-3.1.2 werkzeug-3.0.0
```

### Step 3: Verify Installation

Check that Flask is installed correctly:

```bash
pip show flask
```

Output:
```
Name: Flask
Version: 3.0.0
Summary: A simple framework for building complex web applications.
Home-page: https://palletsprojects.com/
Author: Armin Ronacher
Author-email: armin.ronacher@active-4.com
License: BSD-3-Clause
Location: .../venv/lib/python3.12/site-packages
Requires: Werkzeug, Jinja2, itsdangerous, click
Required-by:
```

### Step 4: Test Flask Works

Create a quick test script to verify Flask can be imported and used:

```python
# test_flask.py — Verify Flask installation
import flask  # Import the flask module

print(f"Flask version: {flask.__version__}")  # Print the installed Flask version

# Verify Flask's core components work
from flask import Flask, request, jsonify, render_template

# Create a test app to ensure everything initializes properly
test_app = Flask(__name__)

@test_app.route("/test")
def test_route():
    return "Flask is working!"

# Run a simple test using Flask's test client
with test_app.test_client() as client:  # Create a test client (simulates HTTP requests)
    response = client.get("/test")  # Make a GET request to /test
    print(f"Test response: {response.status_code}")  # Should print 200
    print(f"Response data: {response.data.decode()}")  # Should print "Flask is working!"

print("Flask installation verified successfully!")
```

Run this test:
```bash
python test_flask.py
```

Expected output:
```
Flask version: 3.0.0
Test response: 200
Response data: Flask is working!
Flask installation verified successfully!
```

### Step 5: Export Requirements (Optional but Recommended)

When you collaborate with others or deploy to production, they need to know which packages to install. Generate a requirements file:

```bash
pip freeze > requirements.txt
```

This creates a `requirements.txt` file with all installed packages and versions. Others can install everything with:

```bash
pip install -r requirements.txt
```

## Common Mistakes

❌ **Installing Flask without activating the virtual environment**
```bash
# WRONG — Flask installs globally, not in your project
pip install flask
# Now you have system-wide Flask instead of project-specific
```

✅ **Correct — Always activate the virtual environment first**
```bash
# CORRECT — Install Flask in the project-specific environment
source venv/bin/activate  # or venv\Scripts\activate on Windows
pip install flask
```

❌ **Using pip3 instead of pip when in a virtual environment**
```bash
# UNNECESSARY — Inside an activated venv, 'pip' already uses the venv's Python
pip3 install flask  # Works but unnecessary when venv is active
```

✅ **Correct — Just use pip**
```bash
# CORRECT — pip automatically uses the correct Python in an active venv
pip install flask
```

❌ **Not checking the Flask version after installation**
```bash
# WRONG — You might have an old version with different API
# (Flask 2.x has different features than Flask 3.x)
```

✅ **Correct — Always verify the version**
```bash
# CORRECT — Check the version to know what features are available
pip show flask
# Or in Python:
import flask
print(flask.__version__)
```

## Quick Reference

| Command | Description |
|---------|-------------|
| `pip install flask` | Install Flask in the active environment |
| `pip install flask[async]` | Install Flask with async support |
| `pip show flask` | Display Flask package information |
| `pip list` | List all installed packages |
| `pip freeze > requirements.txt` | Export all packages to a requirements file |
| `pip install -r requirements.txt` | Install all packages from a requirements file |

## Next Steps

Now that Flask is installed, continue to [01_hello_world.md](../03_first_app/01_hello_world.md) to create your first Flask application and see it run in the browser.