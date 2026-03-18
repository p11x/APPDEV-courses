# Virtual Environments

## What You'll Learn
- What virtual environments are and why they matter
- Creating and activating virtual environments
- Managing dependencies with pip
- Using requirements.txt for reproducible environments
- Best practices for project isolation

## Prerequisites
- Python 3.11+ installed
- Basic command line knowledge
- Completed functions and modules section

## Why Virtual Environments Matter

Imagine you're working on two projects:

- **Project A** needs `flask==2.0` 
- **Project B** needs `flask==3.0`

Without virtual environments, you'd have to uninstall Flask and reinstall the other version every time you switch projects. That's tedious and error-prone!

A **virtual environment** is an isolated Python environment where each project can have its own dependencies, independent of other projects and your system Python.

Think of it like having separate workshops for different projects — each with its own tools and materials.

## Creating a Virtual Environment

### Using the Built-in venv Module

Python 3.3+ includes `venv` (virtual environment) in the standard library:

```bash
# Navigate to your project
cd my_project

# Create a virtual environment named "venv"
python -m venv venv
```

This creates a folder called `venv` containing:
- A copy of the Python interpreter
- A `pip` installation
- Space for installed packages

### Activating the Virtual Environment

**Windows (Command Prompt):**
```bash
venv\Scripts\activate
```

**Windows (PowerShell):**
```bash
venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source venv/bin/activate
```

When activated, your terminal prompt changes:

```
# Before activation
C:\Users\You\my_project>

# After activation
(venv) C:\Users\You\my_project>
```

### Deactivating

```bash
deactivate
```

The `(venv)` prefix disappears, and you're back to the system Python.

## Installing Packages in Virtual Environments

With your virtual environment activated:

```bash
# Install a single package
pip install flask

# Install specific version
pip install flask==3.0.0

# Install multiple packages
pip install flask fastapi sqlalchemy

# Upgrade a package
pip install --upgrade flask

# Uninstall a package
pip uninstall flask

# List installed packages
pip list
```

🔍 **pip Commands Explained:**

1. `pip install package` — Downloads and installs from PyPI
2. `pip install package==version` — Installs exact version
3. `pip install --upgrade package` — Updates to latest version
4. `pip uninstall package` — Removes the package
5. `pip list` — Shows all installed packages

## requirements.txt

A `requirements.txt` file documents your project's dependencies. This is crucial for reproducibility and collaboration.

### Creating requirements.txt

```bash
# Freeze all installed packages
pip freeze > requirements.txt
```

This creates a file like:

```
Flask==3.0.0
Werkzeug==3.0.1
click==8.1.7
```

### Installing from requirements.txt

```bash
# Install all dependencies
pip install -r requirements.txt
```

### Better: Pinning Only Direct Dependencies

`pip freeze` includes all dependencies (including sub-dependencies). For cleaner files, create manually:

```
# Project dependencies only (not sub-dependencies)
Flask==3.0.0
SQLAlchemy==2.0.25
Pydantic==2.5.3
httpx==0.26.0
```

### Using pip-tools for Production

For serious projects, use `pip-tools` to manage dependencies:

```bash
pip install pip-tools

# Create requirements.in (direct dependencies only)
# Edit the file to add:
# Flask
# SQLAlchemy
# FastAPI

# Compile to requirements.txt
pip-compile requirements.in
```

This generates a fully pinned `requirements.txt`.

## The Complete Workflow

Here's the recommended workflow for starting a new project:

```bash
# 1. Create project directory
mkdir my_web_app
cd my_web_app

# 2. Create virtual environment
python -m venv venv

# 3. Activate it
# Windows:
venv\Scripts\activate
# macOS/Linux:
source venv/bin/activate

# 4. Install packages
pip install flask sqlalchemy pydantic

# 5. Create requirements.txt
pip freeze > requirements.txt

# 6. Create .gitignore
echo "venv/" >> .gitignore
echo "__pycache__/" >> .gitignore

# 7. Create your application files
# ... (write your Python code)

# 8. Run your app
python app.py
```

## Virtual Environments with VS Code

VS Code works great with virtual environments:

1. **Automatic Detection**: When you open a folder with a `venv`, VS Code asks to use it
2. **Manual Selection**: 
   - Press `Ctrl+Shift+P`
   - Type "Python: Select Interpreter"
   - Choose your virtual environment

3. **Integrated Terminal**: The terminal in VS Code can automatically activate the venv:

```bash
# In .bashrc or similar (macOS/Linux)
# Add this line to auto-activate when entering directory
# eval "$(conda shell.bash hook)"
# conda activate venv
```

## Virtual Environments with Poetry

**Poetry** is a modern alternative to pip for dependency management:

```bash
# Install Poetry
pip install poetry

# Initialize project
poetry init

# Add dependencies
poetry add flask fastapi

# Install all dependencies
poetry install

# Generate lock file
poetry lock
```

Poetry creates `pyproject.toml` and `poetry.lock` files with precise dependency versions.

## Virtual Environments with Pipenv

**Pipenv** combines pip and virtualenv:

```bash
# Install Pipenv
pip install pipenv

# Install packages (creates Pipfile and virtual environment)
pipenv install flask
pipenv install --dev pytest

# Activate the shell
pipenv shell

# Run a command in the environment
pipenv run python app.py
```

## Best Practices

### DO:

✅ **Always use virtual environments** for each project

✅ **Pin versions** in requirements.txt for reproducibility

✅ **Use .gitignore** to exclude venv folders:

```
# Add to .gitignore
venv/
.venv/
env/
.env/
__pycache__/
*.pyc
.pytest_cache/
```

✅ **Commit requirements.txt** to version control (not the venv folder itself)

### DON'T:

❌ **Don't install packages globally** unless truly system-wide needed

❌ **Don't share venv folders** — recreate with requirements.txt

❌ **Don't use `pip install --user`** inside a venv (confusing)

## Troubleshooting

### "python" command not found

```bash
# Windows: Use py launcher
py -m venv venv

# macOS/Linux: Check Python installation
which python3
ls /usr/bin/python*
```

### Activation fails on Windows

```bash
# If Scripts folder is missing, recreate virtual environment
rm -rf venv
python -m venv venv

# If execution policy blocks scripts
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### Package conflicts

```bash
# Delete lock files and recreate
rm -rf venv
python -m venv venv
pip install -r requirements.txt
```

## Summary
- **Virtual environments** isolate project dependencies
- Create with `python -m venv venv`
- Activate with `venv\Scripts\activate` (Windows) or `source venv/bin/activate` (Mac/Linux)
- Use **requirements.txt** to document dependencies
- Use `.gitignore` to exclude venv from version control
- Tools like **Poetry** and **Pipenv** offer alternative dependency management

## Next Steps
→ Continue to `04-type-hints-and-modern-python.md` to learn advanced type hints and modern Python patterns.
