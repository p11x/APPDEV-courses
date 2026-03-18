# Open Source Licenses

## What You'll Learn

- Common open source licenses
- Choosing the right license
- License compatibility

## Prerequisites

- Completed `08-building-community.md`

## Popular Licenses

### MIT License

Simple and permissive:

- Allows commercial use
- Allows modification
- Allows distribution
- Provides no warranty

```text
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software...
```

### Apache 2.0

Similar to MIT but also provides:

- Explicit patent rights
- More detailed terms

### GPLv3

Stronger copyleft:

- Must distribute source code
- Must use same license for derivatives

### BSD 3-Clause

Like MIT but prohibits:

- Using contributors' names for endorsement

## Choosing a License

| Use Case | Recommended License |
|----------|-------------------|
| Permissive, commercial friendly | MIT, Apache 2.0 |
| Ensure improvements are open | GPLv3 |
| Libraries | MIT, Apache 2.0 |
| Applications | Any |

## Adding a License

```bash
# Add license file
cp LICENSE LICENSE

# In Python projects, also declare in setup.py/pyproject.toml
[project]
license = {text = "MIT"}
```

## Summary

- Choose a license before publishing
- MIT is good for beginners
- Consider your project's goals

## Next Steps

Continue to `10-commercial-open-source.md`.
