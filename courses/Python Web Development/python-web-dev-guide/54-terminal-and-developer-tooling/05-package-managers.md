# Package Managers

## What You'll Learn

- What package managers are
- Using pip for Python packages
- Using pipx for CLI tools
- Requirements files
- PyPI and package best practices

## Prerequisites

- Completed `04-text-editors-and-ides.md`
- Python installed on your machine

## What Is a Package Manager?

A package manager handles installing, updating, and removing software packages:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PACKAGE MANAGERS IN PYTHON                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  PIP:                                                                      │
│  • Python's package manager                                                │
│  • Installs from PyPI (Python Package Index)                               │
│  • Most common                                                             │
│                                                                             │
│  PIPX:                                                                     │
│  • For CLI applications                                                    │
│  • Creates isolated environments                                           │
│  • Safe to install global tools                                            │
│                                                                             │
│  POETRY / PDM:                                                             │
│  • Modern alternatives                                                     │
│  • Better dependency resolution                                           │
│  • Modern Python project management                                        │
│                                                                             │
│  CONDA:                                                                    │
│  • Anaconda package manager                                                │
│  • Includes non-Python packages                                            │
│  • Good for data science                                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Pip Basics

### Installing Packages

```bash
# Install a package
pip install requests

# Install specific version
pip install requests==2.31.0

# Install minimum version
pip install "requests>=2.30.0"

# Install from git
pip install git+https://github.com/psf/requests.git
```

🔍 **What this does:**
- Downloads the package from PyPI
- Installs it to your Python environment
- Also installs dependencies

### Updating Packages

```bash
# Check for updates
pip list --outdated

# Update a package
pip install --upgrade requests

# Update all packages
pip list --outdated | awk '{print $1}' | xargs pip install --upgrade
```

### Uninstalling

```bash
# Uninstall a package
pip uninstall requests

# Uninstall multiple
pip uninstall requests flask
```

### Searching

```bash
# Search PyPI
pip search requests  # Note: PyPI search is deprecated

# Use PyPI website instead
# https://pypi.org
```

## Requirements Files

### Creating Requirements Files

```bash
# Export current environment
pip freeze > requirements.txt

# Or use pip-chill (only your packages, not deps)
pip-chill > requirements.txt
```

🔍 **What this does:**
- `pip freeze` lists all installed packages
- Can be used to recreate the environment

### Using Requirements Files

```bash
# Install from requirements
pip install -r requirements.txt

# Install in development mode
pip install -e .

# Install with dev dependencies
pip install -r requirements-dev.txt
```

### Requirements File Structure

```
# requirements.txt
fastapi>=0.100.0
uvicorn[standard]>=0.22.0
pydantic>=2.0.0
sqlalchemy>=2.0.0
python-jose[cryptography]>=3.3.0
passlib[bcrypt]>=1.7.4
python-multipart>=0.0.6
```

### Environment Separation

```
# requirements.txt (production)
fastapi>=0.100.0
uvicorn>=0.22.0

# requirements-dev.txt (development)
-r requirements.txt
pytest>=7.0.0
pytest-cov>=4.0.0
black>=23.0.0
flake8>=6.0.0
mypy>=1.0.0
```

## PipX

For CLI tools, use pipx:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PIPX FOR CLI TOOLS                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  WHY PIPX:                                                                 │
│  • Isolates CLI tools in their own environments                            │
│  • Safe to install global tools                                            │
│  • Easy to update and remove                                               │
│  • Makes tools available in PATH                                          │
│                                                                             │
│  EXAMPLES:                                                                  │
│  pipx install black       # Install black CLI                            │
│  pipx install httpie      # Install HTTPie                                │
│  pipx install ptpython    # Install PTPython                              │
│  pipx run black .         # Run without installing                         │
│                                                                             │
│  COMMANDS:                                                                 │
│  pipx list                # List installed packages                       │
│  pipx upgrade <package>   # Update a package                               │
│  pipx uninstall <package> # Remove package                                 │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Modern Tools: Poetry and PDM

### Poetry

```bash
# Install Poetry
curl -sSL https://install.python-poetry.org | python3 -

# Initialize project
poetry new my-project

# Add dependencies
poetry add fastapi
poetry add --group dev pytest

# Install dependencies
poetry install

# Generate requirements
poetry export -f requirements.txt --output requirements.txt
```

### PDM

```bash
# Install PDM
pip install pdm

# Initialize project
pdm init

# Add dependencies
pdm add fastapi
pdm add -dG test pytest
```

## Best Practices

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PACKAGE MANAGEMENT BEST PRACTICES                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ USE VIRTUAL ENVIRONMENTS:                                               │
│  • Never install packages globally (except system tools)                 │
│  • Use venv, poetry, or pdm                                               │
│                                                                             │
│  ✅ PIN VERSIONS:                                                          │
│  • Use specific versions or minimum versions                              │
│  • Avoid unpinned dependencies                                             │
│                                                                             │
│  ✅ USE REQUIREMENTS FILES:                                                │
│  • Commit requirements.txt                                                │
│  • Separate dev and prod requirements                                      │
│                                                                             │
│  ✅ USE DEPENDENCY LOCKING:                                                │
│  • Poetry: poetry.lock                                                    │
│  • Pip-tools: requirements-lock.txt                                      │
│  • PDM: pdm.lock                                                          │
│                                                                             │
│  ✅ AUDIT DEPENDENCIES:                                                    │
│  • pip-audit (security vulnerabilities)                                   │
│  • safety (check dependencies)                                            │
│                                                                             │
│  ❌ DON'T:                                                                 │
│  • pip install without a virtual environment                              │
│  • pip install from untrusted sources                                     │
│  • Ignore security warnings                                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Security Scanning

```bash
# Install pip-audit
pip install pip-audit

# Check for vulnerabilities
pip-audit

# Install safety
pip install safety

# Check requirements
safety check -r requirements.txt
```

## PyPI Basics

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PYPI (PYTHON PACKAGE INDEX)                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  WHAT IS PYPI:                                                             │
│  • The official Python package repository                                 │
│  • Over 400,000 packages                                                   │
│  • pypi.org                                                               │
│                                                                             │
│  FINDING PACKAGES:                                                         │
│  • Search on pypi.org                                                     │
│  • Check downloads, last update, maintenance                              │
│  • Look at package popularity (downloads)                                 │
│  • Check for type stubs (pyright support)                                 │
│                                                                             │
│  RELEASING YOUR OWN PACKAGE:                                               │
│  • Use twine to upload                                                    │
│  • Create setup.py or pyproject.toml                                      │
│  • Follow PEP standards                                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- Use pip for Python packages
- Use pipx for CLI tools
- Use requirements files for reproducibility
- Consider Poetry or PDM for modern projects
- Always use virtual environments

## Next Steps

→ Continue to `06-environment-managers.md` to learn about Python version and environment management.
