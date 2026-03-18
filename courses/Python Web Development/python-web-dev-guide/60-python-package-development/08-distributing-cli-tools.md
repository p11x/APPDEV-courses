# Distributing CLI Tools

## What You'll Learn

- Publishing CLI tools
- Console script entry points
- Making tools portable

## Prerequisites

- Completed `07-cli-tools-with-click.md`

## Console Scripts

In `pyproject.toml`:

```toml
[project.scripts]
mytool = "mytool.cli:main"
```

After installation, users can run:

```bash
mytool --help
```

## Entry Points

```toml
[project.entry-points."mytool.plugins"]
custom = "mytool.plugins:load_custom"
```

```python
# mytool/plugins.py
def load_custom() -> dict:
    """Load custom plugins."""
    return {
        "name": "custom",
        "version": "1.0.0"
    }
```

## Using importlib.metadata

```python
from importlib.metadata import entry_points

def list_plugins() -> list[dict]:
    """List all registered plugins."""
    eps = entry_points(group="mytool.plugins")
    return [ep.load()() for ep in eps]
```

## Wheel Installation

```bash
# Build wheel
python -m build --wheel

# Install locally
pip install dist/mytool-1.0.0-py3-none-any.whl
```

## Portable Scripts

```python
# Create a standalone script using PyInstaller
# But recommend pip install for most cases
```

## Homebrew/Linuxbrew

```ruby
# Formula.rb
class Mytool < Formula
  desc "My CLI tool"
  homepage "https://github.com/user/mytool"
  url "https://github.com/user/mytool/archive/v1.0.0.tar.gz"
  sha256 "..."
  
  def install
    system "pip", "install", "."
  end
  
  test do
    system "mytool", "--version"
  end
end
```

## Summary

- Use entry points for CLI commands
- Publish to PyPI for wide distribution
- Consider Homebrew for macOS users

## Next Steps

Continue to `09-dependency-management.md`.
