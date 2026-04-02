# Supervised vs Unsupervised Learning

## What You'll Learn

- Understanding supervised learning
- Understanding unsupervised learning
- When to use each type
- Common algorithms for each

## Prerequisites

- Read [08_visualization_best_practices.md](../../09_Data_Science_Foundations/03_Visualization/08_visualization_best_practices.md) first

## Supervised Learning

Supervised learning uses labeled data to learn a mapping from inputs to outputs.

```python
# supervised_learning.py

from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression
import numpy as np


# Example: Predict house prices
X = np.array([[1500], [1800], [2400], [3000], [3500]])  # Square footage
y = np.array([245000, 312000, 450000, 550000, 680000])  # Prices

# Split data
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

# Train model
model = LinearRegression()
model.fit(X_train, y_train)

# Predict
price = model.predict([[2500]])
print(f"Predicted price: ${price[0]:,.0f}")
```

## Unsupervised Learning

Unsupervised learning finds patterns in unlabeled data.

```python
# unsupervised_learning.py

from sklearn.cluster import KMeans
import numpy as np


# Example: Customer segmentation
data = np.array([
    [25, 50000],
    [30, 60000],
    [35, 55000],
    [45, 80000],
    [50, 90000],
    [28, 45000],
    [40, 75000],
])

# Cluster customers into groups
kmeans = KMeans(n_clusters=2, random_state=42, n_init=10)
clusters = kmeans.fit_predict(data)

print(f"Cluster assignments: {clusters}")
print(f"Cluster centers: {kmeans.cluster_centers_}")
```

## Summary

- Supervised learning uses labeled data
- Unsupervised learning finds patterns in unlabeled data
- Classification and regression are supervised
- Clustering and dimensionality reduction are unsupervised

## Next Steps

Continue to **[02_bias_variance_tradeoff.md](./02_bias_variance_tradeoff.md)**
