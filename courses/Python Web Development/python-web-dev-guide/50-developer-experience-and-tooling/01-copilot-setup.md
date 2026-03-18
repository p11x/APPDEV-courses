# Developer Experience and Tooling

## What You'll Learn
- IDE setup for Python development
- VS Code extensions
- Debugging tools
- Code formatting

## Prerequisites
- Python basics

## VS Code Extensions

Essential extensions for Python development:

- Python (Microsoft)
- Pylance (Microsoft)
- Python Debugger
- autoDocstring
- Python Type Hint

## Debugging

```python
# Using breakpoint()
@app.get("/debug")
async def debug_endpoint():
    data = process_data()
    breakpoint()  # Opens debugger
    return data
```

## Code Formatting

```bash
pip install black ruff isort
```

```toml
# pyproject.toml
[tool.black]
line-length = 88

[tool.ruff]
line-length = 88

[tool.isort]
profile = "black"
```

## Summary

- Use VS Code with Python extensions for best DX
- Configure debugging for breakpoints
- Use Black and Ruff for consistent formatting
- Add type hints for better IDE support
