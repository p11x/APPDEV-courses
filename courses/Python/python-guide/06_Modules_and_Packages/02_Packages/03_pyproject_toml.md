# pyproject.toml

## What You'll Learn

- Modern Python packaging with pyproject.toml
- [project] table
- Dependencies
- Comparison with setup.py

## Prerequisites

- Read [02_pip_and_venv.md](./02_pip_and_venv.md) first

## pyproject.toml

```toml
[project]
name = "mypackage"
version = "1.0.0"
description = "My Python package"
dependencies = [
    "requests>=2.28",
    "numpy>=1.21",
]

[project.scripts]
my-script = "mypackage.main:main"

[build-system]
requires = ["setuptools>=61.0"]
build-backend = "setuptools.build_meta"
```

## Summary

- Modern Python projects use pyproject.toml
- Replaces setup.py for most cases

## Next Steps

Continue to **[06_Modules_and_Packages/03_Popular_Third_Party/01_requests_and_httpx.md](../03_Popular_Third_Party/01_requests_and_httpx.md)**
