# Publishing to PyPI

## What You'll Learn

- Setting up PyPI accounts
- Building and uploading packages
- Using test PyPI

## Prerequisites

- Completed `02-setup-py-and-pyproject.md`

## Creating PyPI Accounts

1. Create account at [pypi.org](https://pypi.org)
2. Create account at [test.pypi.org](https://test.pypi.org) (for testing)
3. Set up API token for secure uploads

## Configure PyPI Upload

```toml
# ~/.pypirc
[pypi]
username = __token__
password = <your-api-token>

[testpypi]
username = __token__
password = <your-test-api-token>
```

## Building the Package

```bash
# Install build tools
pip install build twine

# Build the package
python -m build

# This creates:
# dist/
#   my-package-0.1.0.tar.gz
#   my_package-0.1.0-py3-none-any.whl
```

## Uploading

```bash
# Upload to Test PyPI first
twine upload --repository testpypi dist/*

# Install and test
pip install --index-url https://test.pypi.org/simple/ my-package

# Upload to production PyPI
twine upload dist/*
```

## Version Management

```bash
# Update version in pyproject.toml
# Then rebuild and upload

# Using bump2version (optional)
pip install bump2version
bump2version patch  # 0.1.0 -> 0.1.1
bump2version minor  # 0.1.0 -> 0.2.0
bump2version major  # 0.1.0 -> 1.0.0
```

## Summary

- Test on test.pypi.org first
- Use API tokens for security
- Follow semantic versioning

## Next Steps

Continue to `04-versioning-and-releases.md`.
