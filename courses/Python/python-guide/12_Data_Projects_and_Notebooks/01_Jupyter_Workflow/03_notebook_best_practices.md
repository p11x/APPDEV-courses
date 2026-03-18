# 📋 Notebook Best Practices

## What You'll Learn

- Write notebooks that future-you will understand

## Structure

### Top Cell: Imports
Always start with imports:

```python
# Imports
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

# Constants
FILE_PATH = "data.csv"
```

## Naming

- Use clear names: `01_eda_sales.ipynb` not `Untitled3.ipynb`
- Use underscores: `exploration_data_analysis.ipynb`

## Organization

1. Title and introduction in Markdown
2. Imports at top
3. Data loading
4. EDA sections
5. Modeling sections
6. Conclusions

## Git

```gitignore
# .gitignore
__pycache__/
.ipynb_checkpoints/
*.pyc
```

## Clear Outputs

Clear outputs before committing:

```python
# Jupyter Lab: Kernel → Restart Kernel and Clear All Outputs
```

## Restart and Run All

Always test with "Restart Kernel and Run All Cells" before sharing!
