# Versioning and Releases

## What You'll Learn

- Semantic versioning
- Release workflows
- Changelog management

## Prerequisites

- Completed `03-publishing-to-pypi.md`

## Semantic Versioning

Format: MAJOR.MINOR.PATCH

- **MAJOR** - Incompatible API changes
- **MINOR** - New functionality (backward compatible)
- **PATCH** - Bug fixes (backward compatible)

Examples:
```
1.0.0 - Initial release
1.0.1 - Bug fix
1.1.0 - New feature
2.0.0 - Breaking change
```

## Version Specifiers

In dependencies:

```toml
dependencies = [
    "package>=1.0.0",      # Greater than or equal
    "package>=1.0.0,<2.0.0",  # Range
    "package~=1.0.0",      # Compatible release
    "package==1.0.0",      # Exact version
]
```

## Release Workflow

```bash
# 1. Update version
bump2version patch

# 2. Update changelog
# Edit CHANGELOG.md

# 3. Commit changes
git add .
git commit -m "Release 1.0.1"

# 4. Create tag
git tag -a v1.0.1 -m "Release 1.0.1"

# 5. Push
git push && git push --tags
```

## Changelog Format

```markdown
# Changelog

## [1.0.1] - 2024-01-15

### Fixed
- Fixed bug in function X
- Improved error handling

## [1.1.0] - 2024-01-10

### Added
- Added new feature Y
- Added support for Z

### Changed
- Updated dependency versions
```

## GitHub Releases

```yaml
# .github/workflows/release.yml
name: Release

on:
  push:
    tags:
      - 'v*'

jobs:
  release:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-python@v5
      - run: pip install build
      - run: python -m build
      - uses: pypa/gh-action-pypi-publish@release/v1
```

## Summary

- Follow semantic versioning
- Keep a detailed changelog
- Automate releases with CI/CD

## Next Steps

Continue to `05-testing-packages.md`.
