# pip and venv

## What You'll Learn

- pip install
- requirements.txt
- Creating venv
- Why virtual environments matter

## Prerequisites

- Read [01_package_structure.md](./01_package_structure.md) first

## pip

```bash
pip install requests
pip install -r requirements.txt
pip freeze > requirements.txt
```

## venv

```bash
# Create virtual environment
python -m venv myenv

# Activate (Windows)
myenv\Scripts\activate

# Activate (macOS/Linux)
source myenv/bin/activate
```

## Summary

- **pip**: Package installer
- **venv**: Virtual environment
- Use separate venv per project

## Next Steps

Continue to **[03_pyproject_toml.md](./03_pyproject_toml.md)**
