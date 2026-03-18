# Environment Managers

## What You'll Learn

- Why separate Python environments matter
- Using venv (built-in)
- Using pyenv for Python versions
- Using conda for data science
- Best practices for project isolation

## Prerequisites

- Completed `05-package-managers.md`
- Python installed on your machine

## Why Environment Managers?

Different projects need different dependencies:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    THE PROBLEM                                               │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Project A: needs Django 3.2                                               │
│  Project B: needs Django 4.2                                               │
│                                                                             │
│  If both use the same Python:                                              │
│  • One will break                                                          │
│  • Can't upgrade without breaking                                          │
│                                                                             │
│  SOLUTION: Separate environments per project                               │
│                                                                             │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐                   │
│  │ Project A   │    │ Project B   │    │ Project C   │                   │
│  │ Django 3.2  │    │ Django 4.2  │    │ FastAPI     │                   │
│  │ Python 3.9   │    │ Python 3.11 │    │ Python 3.12 │                   │
│  └─────────────┘    └─────────────┘    └─────────────┘                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## venv (Built-in)

Python's built-in virtual environment manager:

### Creating a Virtual Environment

```bash
# Create a new environment
python -m venv myproject-env

# Activate it (Linux/macOS)
source myproject-env/bin/activate

# Activate it (Windows)
myproject-env\Scripts\activate

# Check it's working
which python   # Linux/macOS
where python   # Windows
```

🔍 **What this does:**
- Creates a folder with its own Python executable
- Its own site-packages directory
- Installing packages goes to THIS environment only

### Working with venv

```bash
# Activate
source myproject-env/bin/activate

# Install packages
pip install fastapi uvicorn

# See what's installed
pip list

# Export requirements
pip freeze > requirements.txt

# Deactivate when done
deactivate
```

### VS Code with venv

```bash
# VS Code automatically detects venv
# 1. Ctrl+Shift+P → Python: Select Interpreter
# 2. Choose the venv Python
# 3. Terminal will auto-activate
```

## pyenv (Python Version Manager)

Manage multiple Python versions:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PYENV                                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  WHAT IT DOES:                                                              │
│  • Install multiple Python versions                                       │
│  • Switch between versions globally or per-project                        │
│  • Build from source with custom options                                   │
│                                                                             │
│  INSTALL (macOS/Linux):                                                    │
│  curl https://pyenv.run | bash                                             │
│                                                                             │
│  WINDOWS ALTERNATIVE:                                                      │
│  • pyenv-win (pyenv-win.github.io)                                        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Using pyenv

```bash
# List available Python versions
pyenv install --list

# Install a version
pyenv install 3.11.5
pyenv install 3.12.0

# Set global version
pyenv global 3.11.5

# Set local version (per project)
pyenv local 3.11.5  # Creates .python-version file

# Check current version
python --version

# List installed versions
pyenv versions
```

### Combining pyenv + venv

```bash
# 1. Set Python version with pyenv
pyenv local 3.11.5

# 2. Create venv with that version
python -m venv venv

# 3. Activate
source venv/bin/activate
```

## conda

Conda is both a package and environment manager:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CONDA                                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  WHAT IT DOES:                                                              │
│  • Manages packages (Python and non-Python)                                │
│  • Manages environments                                                    │
│  • Comes with Anaconda or Miniconda                                        │
│                                                                             │
│  INSTALL:                                                                   │
│  • Anaconda: Full distribution (many packages)                           │
│  • Miniconda: Minimal, add what you need                                  │
│                                                                             │
│  BEST FOR:                                                                  │
│  • Data science ( NumPy, SciPy, pandas pre-installed)                    │
│  • Scientific computing                                                     │
│  • When you need non-Python packages                                       │
│                                                                             │
│  NOT IDEAL FOR:                                                             │
│  • Web development (pip is usually better)                                │
│  • When you need latest Python versions quickly                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Using conda

```bash
# Create environment
conda create -n myproject python=3.11

# Activate
conda activate myproject

# Install packages
conda install numpy pandas

# Also can use pip inside conda
pip install fastapi

# List environments
conda env list

# Deactivate
conda deactivate
```

## Comparison

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CHOOSING AN ENVIRONMENT MANAGER                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  VENV:                                                                      │
│  ✅ Built-in (no install needed)                                          │
│  ✅ Simple                                                                 │
│  ✅ Works everywhere                                                       │
│  ❌ Only manages packages, not Python versions                            │
│                                                                             │
│  PYENV:                                                                     │
│  ✅ Manages Python versions                                               │
│  ✅ Works well with venv                                                   │
│  ✅ Popular in community                                                   │
│  ❌ macOS/Linux only (mostly)                                            │
│                                                                             │
│  CONDA:                                                                     │
│  ✅ All-in-one                                                            │
│  ✅ Non-Python packages                                                   │
│  ✅ Good for data science                                                  │
│  ❌ Slower, larger                                                        │
│                                                                             │
│  FOR WEB DEV:                                                              │
│  → Use pyenv for Python versions + venv for environments                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Modern Tools: Rye and uv

### uv (Astral)

```bash
# Install uv
curl -LsSf https://astral.sh/uv/install.sh | sh

# Create project
uv init my-project

# Add dependencies
uv add fastapi
uv add --dev pytest

# Sync environment
uv sync
```

### Rye

```bash
# Install rye
curl -sSf https://rye-up.com/get | bash

# Create project
rye init my-project

# Add dependencies
rye add fastapi
rye add --dev pytest

# Sync
rye sync
```

## Best Practices

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    ENVIRONMENT BEST PRACTICES                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ ALWAYS USE A VIRTUAL ENVIRONMENT:                                       │
│  • For every project                                                       │
│  • Never pip install globally                                             │
│                                                                             │
│  ✅ SEPARATE ENVIRONMENTS:                                                  │
│  • One per project                                                         │
│  • Or one per major version                                                │
│                                                                             │
│  ✅ NAME CONSISTENTLY:                                                      │
│  • venv/, .venv (in project)                                              │
│  • Or clearly named                                                        │
│                                                                             │
│  ✅ GITIGNORE:                                                             │
│  venv/                                                                     │
│  .venv/                                                                    │
│  env/                                                                      │
│                                                                             │
│  ✅ DOCUMENT:                                                               │
│  • requirements.txt or pyproject.toml                                    │
│  • Python version (.python-version)                                        │
│                                                                             │
│  ❌ DON'T:                                                                  │
│  • Share environments between projects                                    │
│  • Commit venv to git                                                     │
│  • Use system Python for projects                                         │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- Always use virtual environments for projects
- venv is built-in and simple
- pyenv manages Python versions
- conda is good for data science
- Modern tools like uv combine both

## Next Steps

→ Continue to `07-debugging-tools.md` to learn debugging Python applications.
