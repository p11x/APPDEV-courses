# Documentation

## What You'll Learn

- Writing README files
- API documentation
- Using Sphinx

## Prerequisites

- Completed `05-testing-packages.md`

## README Structure

```markdown
# Package Name

Short description of what your package does.

## Installation

```bash
pip install package-name
```

## Quick Start

```python
from package import function

result = function("input")
```

## API Reference

### function(param: str) -> str

Description of what the function does.

**Parameters:**
- `param` (str): Description of parameter

**Returns:**
- str: Description of return value

## Contributing

See CONTRIBUTING.md

## License

MIT License
```

## Docstrings

```python
def process_data(
    data: list[dict],
    config: dict | None = None
) -> list[dict]:
    """Process data according to configuration.
    
    This function takes a list of dictionaries and applies
    transformations based on the provided configuration.
    
    Args:
        data: List of dictionaries to process
        config: Optional configuration dictionary
        
    Returns:
        List of processed dictionaries
        
    Raises:
        ValueError: If data is empty
        
    Example:
        >>> data = [{"value": 1}, {"value": 2}]
        >>> process_data(data)
        [{"value": 1}, {"value": 2}]
    """
    if not data:
        raise ValueError("Data cannot be empty")
    
    config = config or {}
    # Processing logic
    
    return data
```

## Sphinx Documentation

```bash
# Install Sphinx
pip install sphinx sphinx-rtd-theme

# Create documentation
sphinx-quickstart docs
```

```python
# docs/conf.py
project = 'My Package'
copyright = '2024, Your Name'
author = 'Your Name'
release = '1.0.0'

extensions = [
    'sphinx.ext.autodoc',
    'sphinx.ext.viewcode',
]

html_theme = 'sphinx_rtd_theme'
```

```rst
.. docs/index.rst

Welcome
=======

.. toctree::
   :maxdepth: 2
   :caption: Contents:

   api

Indices and tables
==================

* :ref:`genindex`
* :ref:`modindex`
* :ref:`search`
```

## Hosting on Read the Docs

```yaml
# .readthedocs.yaml
version: 2

build:
  os: ubuntu-22.04
  tools:
    python: "3.11"

sphinx:
  configuration: docs/conf.py

python:
  install:
    - method: pip
      path: .
      extra-requirements: [dev]
```

## Summary

- Write clear README files
- Use docstrings for API docs
- Host documentation online

## Next Steps

Continue to `07-cli-tools-with-click.md`.
