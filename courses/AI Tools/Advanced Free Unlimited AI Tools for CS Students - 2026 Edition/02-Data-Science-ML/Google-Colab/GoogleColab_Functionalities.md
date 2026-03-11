# Google Colab Functionalities

## Basic Operations

### Writing Code

```python
# Single line execution
print("Hello, World!")

# Multi-line code
def greet(name):
    return f"Hello, {name}!"

print(greet("Student"))
```

### Markdown Support

- Add text explanations
- Include equations: $E = mc^2$
- Display images
- Create headers

## Data Science Workflows

### Loading Data

```python
import pandas as pd

# From CSV
df = pd.read_csv('data.csv')

# From URL
df = pd.read_csv('https://example.com/data.csv')

# From Google Drive
from google.colab import drive
drive.mount('/content/drive')
df = pd.read_csv('/content/drive/...')
```

### Machine Learning

```python
# TensorFlow example
import tensorflow as tf
model = tf.keras.Sequential([...])

# PyTorch example
import torch
model = torch.nn.Linear(10, 1)
```

### Visualization

```python
import matplotlib.pyplot as plt

plt.plot([1, 2, 3], [4, 5, 6])
plt.show()
```

## Key Functionalities

| Function | Use Case |
|----------|----------|
| Cell execution | Run code blocks |
| Runtime management | Restart, connect |
| File upload | Add datasets |
| Drive mount | Access cloud files |

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Shift+Enter | Run cell |
| Ctrl+M M | Convert to markdown |
| Ctrl+M D | Delete cell |
| Ctrl+M A | Insert cell above |