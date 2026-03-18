# Setting Up Your Environment

## What You'll Learn
- How to install Python on your computer
- Setting up a code editor (VS Code)
- Creating virtual environments (best practice)
- Installing and managing packages with pip
- Running your first Python web application

## Prerequisites
- A computer with internet access
- Basic command line knowledge (opening terminal)

## Installing Python

### Windows

1. Visit [python.org/downloads](https://python.org/downloads)
2. Click the "Download Python" button
3. Run the installer
4. **IMPORTANT**: Check "Add Python to PATH" at the bottom of the installer
5. Click "Install Now"

To verify installation, open Command Prompt and type:

```bash
python --version
```

You should see something like `Python 3.11.x`

### macOS

macOS usually comes with Python pre-installed, but it's often an older version. We recommend installing a newer version via Homebrew:

```bash
# If you don't have Homebrew, install it first:
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Then install Python:
brew install python
```

Verify with:

```bash
python3 --version
```

### Linux

Most Linux distributions come with Python. For Ubuntu/Debian:

```bash
sudo apt update
sudo apt install python3 python3-pip python3-venv
```

Verify with:

```bash
python3 --version
```

## Installing a Code Editor

We recommend **Visual Studio Code (VS Code)** — it's free, powerful, and works great with Python.

### Installing VS Code

1. Visit [code.visualstudio.com](https://code.visualstudio.com)
2. Download for your operating system
3. Install and open VS Code

### Essential VS Code Extensions

Open VS Code and install these extensions:

1. **Python** — Microsoft's official Python extension
   - Provides IntelliSense (autocomplete)
   - Enables debugging
   - Linting (catches errors before running)

2. **Pylance** — Fast Python language server
   - Makes autocomplete smarter
   - Shows type information

3. **Prettier** — Code formatter
   - Keeps your code consistent
   - Press Shift+Alt+F to format

Install extensions by:
1. Click the Extensions icon (left sidebar, looks like squares)
2. Search for each extension
3. Click "Install"

## Understanding Virtual Environments

A **virtual environment** is an isolated Python environment for your project. Think of it as a separate "room" for each project with its own packages.

### Why Use Virtual Environments?

Imagine you have Project A needing `flask==2.0` and Project B needing `flask==3.0`. Without virtual environments, you'd have to uninstall and reinstall Flask every time you switch projects. With virtual environments, each project has its own isolated Python "room" with exactly the packages it needs.

### Creating a Virtual Environment

```bash
# Navigate to your project folder
cd my-web-project

# Create a virtual environment named "venv"
python -m venv venv
```

On Windows, activate it:

```bash
venv\Scripts\activate
```

On macOS/Linux:

```bash
source venv/bin/activate
```

When activated, you'll see `(venv)` at the beginning of your terminal prompt:

```
(venv) C:\Users\You\my-web-project>
```

To deactivate:

```bash
deactivate
```

## Installing Packages with pip

**pip** is Python's package manager. It downloads and installs packages from PyPI (Python Package Index).

### Basic pip Commands

```bash
# Install a package
pip install flask

# Install a specific version
pip install flask==3.0.0

# Install multiple packages
pip install flask fastapi sqlalchemy

# See installed packages
pip list

# Freeze requirements (save all packages to a file)
pip freeze > requirements.txt

# Install from requirements.txt
pip install -r requirements.txt
```

### Understanding requirements.txt

Create a `requirements.txt` file in your project:

```
flask==3.0.0
fastapi==0.109.0
sqlalchemy==2.0.25
pydantic==2.5.3
```

This file lists all dependencies. When you share your project or deploy it, others can install everything with:

```bash
pip install -r requirements.txt
```

## Setting Up Your First Project

Let's create a complete development setup:

### Step 1: Create Project Folder

```bash
mkdir python-web-guide
cd python-web-guide
```

### Step 2: Create Virtual Environment

```bash
python -m venv venv
```

### Step 3: Activate Virtual Environment

Windows:
```bash
venv\Scripts\activate
```

macOS/Linux:
```bash
source venv/bin/activate
```

### Step 4: Install Flask

```bash
pip install flask
```

### Step 5: Create Your First App

Create a file called `app.py`:

```python
from flask import Flask

app = Flask(__name__)

@app.route("/")
def home() -> str:
    return """
    <!DOCTYPE html>
    <html>
    <head>
        <title>My First App</title>
    </head>
    <body>
        <h1>Hello, World!</h1>
        <p>My first Python web app is running!</p>
    </body>
    </html>
    """

if __name__ == "__main__":
    app.run(debug=True, port=5000)
```

🔍 **Line-by-Line Breakdown:**

1. `from flask import Flask` — Imports the Flask class from the flask package we installed.
2. `app = Flask(__name__)` — Creates a Flask application instance. `__name__` is a special variable that equals "__main__" when running the file directly.
3. `@app.route("/")` — A decorator that maps the root URL ("/") to the function below.
4. `def home() -> str:` — A function that returns a string (HTML in this case). The `-> str` is a type hint.
5. The HTML string — This is what gets displayed in the browser.
6. `if __name__ == "__main__":` — Ensures the server only runs when executing the file directly.
7. `app.run(debug=True, port=5000)` — Starts the development server on port 5000 with debug mode enabled (auto-reload on changes).

### Step 6: Run the App

```bash
python app.py
```

You'll see output like:

```
 * Serving Flask app 'app'
 * Debug mode: on
 * Running on http://127.0.0.1:5000
```

Open your browser to `http://127.0.0.1:5000` — you should see your webpage!

### Step 7: Save Dependencies

```bash
pip freeze > requirements.txt
```

## VS Code: Running Python

In VS Code, you can run Python files:

1. Open `app.py`
2. Press `F5` or click the "Run" button
3. Or right-click and select "Run Python File in Terminal"

VS Code will automatically use the virtual environment if it's activated.

## Best Practices Summary

1. **Always use virtual environments** — Keeps projects isolated
2. **Use requirements.txt** — Documents dependencies
3. **Never commit venv to git** — Add `venv/` to your `.gitignore`
4. **Use meaningful names** — `my_project` not `mp` or `test`

Create a `.gitignore` file:

```
venv/
__pycache__/
*.pyc
.env
.pytest_cache/
```

## Summary
- Install Python 3.11+ from python.org
- Use VS Code with Python extensions
- **Virtual environments** isolate project dependencies
- Use **pip** to install packages
- Create **requirements.txt** to document dependencies
- Run Flask with `python app.py`

## Next Steps
→ Continue to `../01-html-css-basics/01-html-fundamentals.md` to learn HTML, the foundation of web pages.
