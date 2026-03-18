# 📓 Jupyter Setup

## 🛠️ Setup

```python
pip install jupyterlab notebook
```

## Starting Jupyter

```bash
jupyter lab  # Use JupyterLab (modern)
jupyter notebook  # Classic interface
```

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Shift+Enter | Run cell |
| Ctrl+Enter | Run cell in place |
| A | Insert cell above |
| B | Insert cell below |
| DD | Delete cell |
| M | Markdown cell |
| Y | Code cell |

## Cell Types

- **Code**: Python code
- **Markdown**: Documentation
- **Raw**: Unformatted text

## Magic Commands

```python
%timeit sum(range(1000))  # Time execution
%matplotlib inline         # Show plots
!ls                        # Run shell command
```
