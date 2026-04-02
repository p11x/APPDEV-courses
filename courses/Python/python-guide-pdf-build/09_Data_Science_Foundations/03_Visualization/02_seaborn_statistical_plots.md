# 📊 Seaborn Statistical Plots

## 🎯 What You'll Learn

- Create beautiful statistical charts with one line of code
- Histograms with KDE, box plots, violin plots
- Scatter plots with hue encoding
- Correlation heatmaps and pair plots

## 📦 Prerequisites

- Read [01_matplotlib_basics.md](./01_matplotlib_basics.md) first

## What is Seaborn?

Seaborn is built on top of Matplotlib and makes **beautiful statistical plots with minimal code**. It's like Instagram filters for your data!

### Installing

```python
pip install seaborn
```

### Import Convention

```python
import seaborn as sns
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np
```

### Set the Style

```python
# Always set the style first!
sns.set_theme(style="whitegrid")  # Beautiful default style
```

## Histogram with KDE

```python
import seaborn as sns
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd

# Generate sample data
np.random.seed(42)
data: pd.DataFrame = pd.DataFrame({
    "value": np.concatenate([np.random.normal(50, 10, 500), 
                              np.random.normal(80, 15, 500)])
})

fig, ax = plt.subplots(figsize=(10, 6))

# Histogram with KDE (Kernel Density Estimate)
sns.histplot(data["value"], kde=True, color="#FF6B6B", bins=30, alpha=0.7, ax=ax)

ax.set_title("Distribution with KDE", fontsize=14, fontweight="bold")
ax.set_xlabel("Value", fontsize=12)
ax.set_ylabel("Count", fontsize=12)

plt.show()
```

### 💡 Line-by-Line Breakdown

- `sns.histplot()` - Create histogram
- `kde=True` - Add smooth density curve
- `bins=30` - Number of bins
- `alpha=0.7` - Transparency

### 💡 Explanation

KDE (Kernel Density Estimate) shows the probability density — the "shape" of your distribution without the jagged bars!

## Box Plot

Show the five-number summary at a glance:

```python
import seaborn as sns
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

# Create sample data
np.random.seed(42)
tips: pd.DataFrame = pd.DataFrame({
    "day": ["Fri", "Sat", "Sun", "Fri", "Sat", "Sun"] * 4,
    "total_bill": np.random.uniform(10, 50, 24),
    "tip": np.random.uniform(2, 10, 24)
})

fig, ax = plt.subplots(figsize=(8, 6))

# Box plot: distribution by category
sns.boxplot(x="day", y="total_bill", data=tips, palette="Set2", ax=ax)

ax.set_title("Bill Amount by Day", fontsize=14, fontweight="bold")

plt.show()
```

### Output Explanation

```
    ┌─────┐
    │     │  ← 75th percentile
────┤     │
    │─────│  ← Median (50th percentile)
────┤     │
    │     │  ← 25th percentile
    └─────┘
      ↑     ← Whiskers: 1.5 × IQR
      ○  ← Outliers beyond whiskers
```

## Violin Plot

Combines box plot with KDE — shows the full distribution shape:

```python
import seaborn as sns
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

# Create data with different distributions
np.random.seed(42)
df: pd.DataFrame = pd.DataFrame({
    "Group": ["A"] * 100 + ["B"] * 100,
    "Value": np.concatenate([
        np.random.normal(50, 10, 100),    # Group A: normal
        np.random.exponential(50, 100)    # Group B: skewed
    ])
})

fig, ax = plt.subplots(figsize=(10, 6))

# Violin plot
sns.violinplot(x="Group", y="Value", data=df, palette="muted", ax=ax)

ax.set_title("Distribution Comparison", fontsize=14, fontweight="bold")

plt.show()
```

### 💡 Explanation

Violin plots show the **full shape** of your distribution — perfect for comparing groups!

## Scatter Plot with Hue

Encode additional information with color:

```python
import seaborn as sns
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

# Load built-in dataset or create one
tips: pd.DataFrame = pd.DataFrame({
    "total_bill": [16.99, 10.34, 21.01, 23.68, 24.59, 25.29, 8.77, 26.88],
    "tip": [1.01, 1.66, 3.50, 3.31, 3.61, 4.71, 2.00, 3.12],
    "smoker": ["No", "No", "No", "No", "Yes", "Yes", "No", "Yes"],
    "size": [2, 3, 2, 4, 4, 3, 1, 2]
})

fig, ax = plt.subplots(figsize=(10, 6))

# Scatter with hue encoding
sns.scatterplot(x="total_bill", y="tip", hue="smoker", size="size",
                data=tips, palette="Set1", s=100, alpha=0.7, ax=ax)

ax.set_title("Tips vs Total Bill", fontsize=14, fontweight="bold")

plt.show()
```

### 💡 Line-by-Line Breakdown

- `hue="smoker"` - Color by smoker status
- `size="size"` - Point size by party size
- `palette="Set1"` - Color palette
- `s=100` - Base size of points

### 💡 Explanation: Hue Encoding

Hue lets you visualize **three variables at once**:
- X axis: total_bill
- Y axis: tip
- Color (hue): smoker status
- Point size: party size

## Correlation Heatmap

See relationships between all numeric variables:

```python
import seaborn as sns
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

# Create correlated data
np.random.seed(42)
df: pd.DataFrame = pd.DataFrame({
    "Age": np.random.randint(20, 60, 100),
    "Income": np.random.randint(30000, 100000, 100),
    "Score": np.random.randint(0, 100, 100),
    "Hours": np.random.randint(20, 60, 100)
})

# Add correlation
df["Income"] = df["Age"] * 1000 + df["Income"] + np.random.randn(100) * 5000
df["Score"] = df["Income"] / 1000 + np.random.randn(100) * 10

fig, ax = plt.subplots(figsize=(10, 8))

# Correlation matrix
corr: pd.DataFrame = df.corr()

# Heatmap
sns.heatmap(corr, annot=True, cmap="coolwarm", center=0,
            fmt=".2f", linewidths=0.5, ax=ax)

ax.set_title("Correlation Matrix", fontsize=14, fontweight="bold")

plt.show()
```

### 💡 Line-by-Line Breakdown

- `df.corr()` - Calculate correlation matrix
- `annot=True` - Show numbers in cells
- `cmap="coolwarm"` - Blue=negative, Red=positive
- `fmt=".2f"` - Format to 2 decimal places

### 💡 Interpretation

- **1.0**: Perfect positive correlation (both go up together)
- **-1.0**: Perfect negative correlation (one goes up, other goes down)
- **0.0**: No correlation

## Pair Plot

Visualize relationships across ALL pairs of variables:

```python
import seaborn as sns
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

# Create sample data
np.random.seed(42)
iris_like: pd.DataFrame = pd.DataFrame({
    "sepal_length": np.random.normal(5.8, 0.8, 150),
    "sepal_width": np.random.normal(3.0, 0.5, 150),
    "petal_length": np.random.normal(3.9, 1.7, 150),
    "petal_width": np.random.normal(1.2, 0.7, 150),
    "species": np.random.choice(["setosa", "versicolor", "virginica"], 150)
})

# Pair plot - scatter plots for all variable pairs
g = sns.pairplot(iris_like, hue="species", palette="husl", diag_kind="kde")

g.fig.suptitle("Pairwise Relationships", y=1.02, fontsize=14, fontweight="bold")

plt.show()
```

### 💡 Line-by-Line Breakdown

- `hue="species"` - Color by species
- `diag_kind="kde"` - Show KDE on diagonal
- `palette="husl"` - Colorful palette

## Bar Plot with Error Bars

```python
import seaborn as sns
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

# Create data with variation
np.random.seed(42)
df: pd.DataFrame = pd.DataFrame({
    "category": ["A", "B", "C", "D"] * 10,
    "value": np.concatenate([
        np.random.normal(50, 5, 10),
        np.random.normal(60, 8, 10),
        np.random.normal(45, 3, 10),
        np.random.normal(70, 10, 10)
    ])
})

fig, ax = plt.subplots(figsize=(8, 6))

# Bar plot with error bars (95% CI automatically!)
sns.barplot(x="category", y="value", data=df, palette="Blues_d", 
            errorbar=("ci", 95), capsize=0.1, ax=ax)

ax.set_title("Mean with 95% Confidence Interval", fontsize=14, fontweight="bold")

plt.show()
```

### 💡 Explanation

Seaborn automatically calculates and shows **error bars** (confidence intervals). This is huge for data science!

## ✅ Summary

- Seaborn makes statistical plots with one line
- `sns.histplot(kde=True)` - distribution with density curve
- `sns.boxplot()` - five-number summary
- `sns.violinplot()` - full distribution shape
- `sns.scatterplot(hue=...)` - encode with color and size
- `sns.heatmap(corr)` - correlation matrix
- `sns.pairplot()` - all pairwise relationships
- `sns.barplot(errorbar=...)` - with error bars

## ➡️ Next Steps

Ready for interactive charts? Head to **[03_plotly_interactive.md](./03_plotly_interactive.md)**!

## 🔗 Further Reading

- [Seaborn Documentation](https://seaborn.pydata.org/)
- [Seaborn Tutorial](https://seaborn.pydata.org/tutorial.html)
- [Statistical Plots](https://seaborn.pydata.org/examples/index.html)
