# Setting Up Development Environment

## What You'll Learn

- Forking and cloning repositories
- Setting up virtual environments
- Running tests locally

## Prerequisites

- Completed `02-finding-projects-to-contribute-to.md`

## Fork and Clone

```bash
# 1. Fork the repository on GitHub (via web interface)

# 2. Clone your fork
git clone https://github.com/YOUR_USERNAME/project-name.git
cd project-name

# 3. Add upstream remote
git remote add upstream https://github.com/original/project-name.git

# 4. Verify remotes
git remote -v
```

## Setting Up Local Development

```bash
# Create virtual environment
python -m venv venv
source venv/bin/activate  # Linux/Mac
# or
venv\Scripts\activate  # Windows

# Install dependencies
pip install -e .  # Install in editable mode
pip install -r requirements-dev.txt  # Dev dependencies

# Install pre-commit hooks
pip install pre-commit
pre-commit install
```

## Running Tests

```bash
# Run all tests
pytest

# Run specific test file
pytest tests/test_core.py

# Run with coverage
pytest --cov=src --cov-report=html

# Run linting
flake8 src/
black src/
mypy src/
```

## Summary

- Fork and clone to create your own copy
- Use virtual environments for isolation
- Always run tests before making changes

## Next Steps

Continue to `04-making-your-first-contribution.md`.
