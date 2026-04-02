# 📋 EDA Template

## What You'll Learn

- A reusable EDA template

## Load and Inspect

```python
import pandas as pd
import numpy as np
import seaborn as sns
import matplotlib.pyplot as plt

# Load data
df = pd.read_csv("data.csv")

# Basic info
print(f"Shape: {df.shape}")
print(df.head(10))
print(df.info())
print(df.describe())
```

## Missing Values

```python
# Missing values
missing = df.isnull().sum()
missing_pct = missing / len(df) * 100
print(missing_pct[missing_pct > 0])

# Visualize
sns.heatmap(df.isnull(), cbar=False)
plt.show()
```

## Distributions

```python
# Histograms
df.hist(bins=30, figsize=(12, 8))
plt.tight_layout()
plt.show()

# Value counts
for col in df.select_dtypes(include="object"):
    print(df[col].value_counts())
```

## Relationships

```python
# Correlation
corr = df.corr()
sns.heatmap(corr, annot=True, cmap="coolwarm")
plt.show()

# Scatter plots
for col in df.select_dtypes(include="number").columns[:5]:
    plt.figure()
    plt.scatter(df[col], df["target"])
    plt.xlabel(col)
    plt.ylabel("target")
```

## Outliers

```python
# Boxplots
for col in df.select_dtypes(include="number").columns:
    plt.figure()
    plt.boxplot(df[col].dropna())
    plt.title(col)
```
