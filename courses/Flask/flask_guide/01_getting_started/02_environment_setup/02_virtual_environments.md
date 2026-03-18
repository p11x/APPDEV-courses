<!-- FILE: 01_getting_started/02_environment_setup/02_virtual_environments.md -->

## Overview

**Virtual environments** are isolated Python environments that allow you to install packages (like Flask) for each project separately. This prevents version conflicts — if Project A needs Flask 2.0 and Project B needs Flask 3.0, virtual environments let you maintain both without them interfering with each other. Every professional Python developer uses virtual environments, and it is considered a best practice for any Flask project.

## Prerequisites

- Python 3.x installed on your computer (from the previous file)
- Basic familiarity with using the terminal/command line
- Understanding of what pip is (Python's package manager)

## Core Concepts

### The Problem Without Virtual Environments

Imagine you have two Flask projects:
- **Project A** — Built last year, uses Flask 2.0 and an older library
- **Project B** — New project, needs Flask 3.0 and modern libraries

If you install packages globally (system-wide), updating Flask for Project B would break Project A. You would be forced to choose between using old versions (limiting new features) or breaking existing projects.

### The Solution: Virtual Environments

A virtual environment creates an isolated folder that contains:
- Its own Python interpreter (optionally)
- Its own `site-packages` directory where pip installs libraries
- Its own pip executable

Think of it as creating a separate "sandbox" for each project — each sandbox has its own set of packages, and they never interfere with each other.

### How Virtual Environments Work

1. You create a virtual environment in your project folder using `python -m venv`
2. You "activate" the environment, which modifies your terminal's PATH to use the environment's Python and pip
3. You install packages with pip — they go into the environment's folder, not globally
4. When you run Python, it uses the environment's packages
5. When done, you "deactivate" the environment to return to normal

> **💡 Tip:** Create a new virtual environment for **every** Python project you work on. It takes 30 seconds and prevents countless headaches.

## Code Walkthrough

### Creating a Virtual Environment (All Operating Systems)

Open your terminal and navigate to your project folder:

```bash
# Navigate to your project directory (replace with your actual path)
cd /path/to/your/project

# Create a virtual environment named 'venv'
python -m venv venv
```

On Windows, you can also use:
```bash
python -m venv venv
```

### Activating the Virtual Environment

**On Windows (Command Prompt):**
```bash
venv\Scripts\activate
```

**On Windows (PowerShell):**
```powershell
venv\Scripts\Activate
```

**On macOS/Linux (bash/zsh):**
```bash
source venv/bin/activate
```

After activation, your terminal prompt changes to show the environment name:
- Windows: `(venv) C:\path\to\project>`
- macOS/Linux: `(venv) user@computer:~/project$`

### Installing Packages in the Virtual Environment

Now that the environment is active, pip installs packages locally:

```bash
# Install Flask in the virtual environment
pip install flask
```

Verify Flask is installed in the environment (not globally):
```bash
# Should show the path to your venv folder
pip show flask
# Location: .../venv/lib/python3.12/site-packages
```

### Deactivating the Virtual Environment

When finished working, deactivate to return to normal:

```bash
deactivate
```

The terminal prompt returns to normal, and pip commands work with the global Python again.

### Complete Workflow Example

```bash
# 1. Create project folder
mkdir my_flask_project
cd my_flask_project

# 2. Create virtual environment
python -m venv venv

# 3. Activate the environment
# Windows:
venv\Scripts\activate
# macOS/Linux:
source venv/bin/activate

# 4. Install Flask
pip install flask

# 5. Verify installation
pip list
# Should show: Flask (3.x.x) and its dependencies

# 6. Create your Flask app
# (We'll cover this in the next file)

# 7. Deactivate when done
deactivate
```

## Common Mistakes

❌ **Skipping virtual environments and installing Flask globally**
```bash
# WRONG — This can cause version conflicts between projects
pip install flask
# All projects now share the same Flask version
```

✅ **Correct — Always use a virtual environment**
```bash
# CORRECT — Each project has its own isolated environment
python -m venv venv
source venv/bin/activate  # or venv\Scripts\activate on Windows
pip install flask
```

❌ **Forgetting to activate the virtual environment**
```bash
# WRONG — If you don't activate, pip installs globally
pip install flask
# Installing globally instead of in venv!
```

✅ **Correct — Always activate before installing packages**
```bash
# CORRECT — Activate first, then install
source venv/bin/activate
pip install flask  # Now installs in venv
```

❌ **Committing venv folder to version control**
```python
# WRONG — The venv folder is huge and specific to your system
# It should not be in your repository
```

✅ **Correct — Add venv to .gitignore**
```bash
# CORRECT — Add this to your .gitignore file
echo "venv/" >> .gitignore
echo "__pycache__/" >> .gitignore
echo "*.pyc" >> .gitignore
```

## Quick Reference

| Command | Description |
|---------|-------------|
| `python -m venv venv` | Create a virtual environment named "venv" |
| `source venv/bin/activate` | Activate on macOS/Linux |
| `venv\Scripts\activate` | Activate on Windows |
| `deactivate` | Deactivate the virtual environment |
| `pip install package_name` | Install a package (only in active venv) |
| `pip list` | List all installed packages in the environment |
| `pip freeze > requirements.txt` | Export packages to a requirements file |

## Next Steps

Now that you understand virtual environments, continue to [03_installing_flask.md](03_installing_flask.md) to install Flask itself and verify your first working application.