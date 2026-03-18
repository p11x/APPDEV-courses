# Setup.py and pyproject.toml

## What You'll Learn

- Understanding setup.py vs pyproject.toml
- Configuring package metadata
- Entry points and console scripts

## Prerequisites

- Completed `01-packaging-basics.md`

## Legacy setup.py

```python
from setuptools import setup, find_packages

setup(
    name="my-package",
    version="0.1.0",
    packages=find_packages(),
    install_requires=[
        "requests>=2.28.0",
    ],
    python_requires=">=3.11",
)
```

## Modern pyproject.toml

```toml
[build-system]
requires = ["setuptools>=61.0", "wheel"]
build-backend = "setuptools.build_meta"

[project]
name = "my-package"
version = "0.1.0"
description = "A short description"
authors = [
    {name = "Your Name", email = "you@example.com"}
]
classifiers = [
    "Programming Language :: Python :: 3",
    "License :: OSI Approved :: MIT License",
    "Operating System :: OS Independent",
]
dependencies = [
    "requests>=2.28.0",
]

[project.optional-dependencies]
dev = [
    "pytest>=7.0.0",
    "black>=23.0.0",
    "mypy>=1.0.0",
]

[project.scripts]
my-package = "my_package.cli:main"

[tool.setuptools]
packages = ["my_package"]

[tool.setuptools.package-data]
my_package = ["*.txt", "*.json"]
```

## Entry Points

Entry points let users run your code from command line:

```toml
[project.scripts]
myapp = "myapp.cli:main"

[project.entry-points."myapp.plugins"]
custom = "myapp.plugins:load_custom"
```

```python
# myapp/cli.py
def main():
    """Main entry point."""
    import click
    
    @click.command()
    def cli():
        click.echo("Hello from myapp!")
    
    return cli()

if __name__ == "__main__":
    main()
```

## Package Data

Include non-Python files:

```toml
[tool.setuptools.package-data]
my_package = ["*.txt", "*.json", "*.md"]
```

## Summary

- Use pyproject.toml for new projects
- Entry points create CLI commands
- Package data includes non-Python files

## Next Steps

Continue to `03-publishing-to-pypi.md`.
