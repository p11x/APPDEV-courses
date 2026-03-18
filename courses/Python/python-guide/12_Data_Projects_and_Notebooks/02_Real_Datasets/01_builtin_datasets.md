# 📊 Built-in Datasets

## What You'll Learn

- Use sklearn built-in datasets
- Load seaborn datasets

## sklearn Datasets

```python
from sklearn.datasets import load_iris, load_wine, load_breast_cancer
import pandas as pd

# Load Iris dataset
iris_data = load_iris()
df = pd.DataFrame(iris_data.data, columns=iris_data.feature_names)
df['target'] = iris_data.target
print(df.head())
```

## seaborn Datasets

```python
import seaborn as sns

# Load built-in datasets
titanic = sns.load_dataset('titanic')
tips = sns.load_dataset('tips')
penguins = sns.load_dataset('penguins')

print(titanic.head())
print(tips.head())
print(penguins.head())
```
