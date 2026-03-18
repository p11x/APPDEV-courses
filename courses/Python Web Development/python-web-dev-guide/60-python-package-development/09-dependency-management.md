# Dependency Management

## What You'll Learn

- Managing dependencies
- Lock files
- Security considerations

## Prerequisites

- Completed `08-distributing-cli-tools.md`

## Specifying Dependencies

```toml
# pyproject.toml
[project]
dependencies = [
    "requests>=2.28.0,<3.0.0",
    "click>=8.0.0",
    "pydantic>=2.0.0",
]

[project.optional-dependencies]
dev = [
    "pytest>=7.0.0",
    "black>=23.0.0",
]
test = [
    "pytest>=7.0.0",
    "pytest-cov>=4.0.0",
]
```

## Lock Files

Using pip-tools or poetry:

```bash
# With pip-tools
pip-compile requirements.in
pip-compile requirements-dev.in

# This creates requirements.txt with locked versions
```

## Security

### Safety

```bash
pip install safety
safety check
```

### Bandit

```bash
pip install bandit
bandit -r mypackage/
```

### GitHub Dependabot

```yaml
# .github/dependabot.yml
version: 2
updates:
  - package-ecosystem: "pip"
    directory: "/"
    schedule:
      interval: "weekly"
```

## Version Constraints

| Operator | Meaning |
|----------|---------|
| == | Exact version |
| != | Not equal |
| >, < | Greater/less than |
| >=, <= | Greater/less or equal |
| ~= | Compatible release |
| * | Any version |

## Summary

- Use version constraints carefully
- Generate lock files for reproducibility
- Use security tools regularly

## Next Steps

Continue to `10-maintaining-packages.md`.
