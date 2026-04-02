# 🔧 Cross-Validation & Hyperparameter Tuning

## 🎯 What You'll Learn

- K-Fold cross-validation
- GridSearchCV and RandomizedSearchCV
- The bias-variance tradeoff
- Learning curves

## 📦 Prerequisites

- Read [02_regression_metrics.md](./02_regression_metrics.md) first

## The Problem: One Test Set Isn't Enough

Your test set might just be **lucky** (or unlucky)!

```
Bad luck: Test set happens to have easy examples → Inflated accuracy!
Bad luck: Test set has hard examples → Deflated accuracy!
```

## K-Fold Cross-Validation

**Use multiple test sets for robust evaluation!**

```
5-Fold Cross-Validation:

Fold 1: [1] [2] [3] [4] [5]  → Train on 4, test on 1
Fold 2: [1] [2] [3] [4] [5]  → Train on 4, test on 1
Fold 3: [1] [2] [3] [4] [5]  → Train on 4, test on 1
Fold 4: [1] [2] [3] [4] [5]  → Train on 4, test on 1
Fold 5: [1] [2] [3] [4] [5]  → Train on 4, test on 1
```

```python
from sklearn.model_selection import cross_val_score
from sklearn.ensemble import RandomForestClassifier
import numpy as np

# Sample data
X: np.ndarray = np.random.randn(100, 5)
y: np.ndarray = np.random.choice([0, 1], 100)

# Model
rf: RandomForestClassifier = RandomForestClassifier(n_estimators=10)

# 5-fold cross-validation
scores: np.ndarray = cross_val_score(rf, X, y, cv=5)

print(f"Fold scores: {scores}")
print(f"Mean: {scores.mean():.3f} (+/- {scores.std()*2:.3f})")
```

### Output

```
Fold scores: [0.7  0.65 0.75 0.6  0.7 ]
Mean: 0.680 (+/- 0.111)
```

### Stratified K-Fold

For classification, use stratified to maintain class balance:

```python
from sklearn.model_selection import StratifiedKFold

skf: StratifiedKFold = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)

scores: np.ndarray = cross_val_score(rf, X, y, cv=skf)
print(f"Mean: {scores.mean():.3f}")
```

## Hyperparameters

**Hyperparameters** = Settings you choose BEFORE training:

| Parameter | Model | What it does |
|-----------|-------|--------------|
| n_estimators | RandomForest | Number of trees |
| max_depth | DecisionTree | Tree depth limit |
| C | SVM | Regularization strength |
| k | KNN | Number of neighbors |

## GridSearchCV: Try All Combinations

```python
from sklearn.model_selection import GridSearchCV
from sklearn.ensemble import RandomForestClassifier
import numpy as np

# Data
X: np.ndarray = np.random.randn(100, 5)
y: np.ndarray = np.random.choice([0, 1], 100)

# Define parameter grid
param_grid: dict = {
    "n_estimators": [10, 50, 100],
    "max_depth": [3, 5, 10, None],
    "min_samples_split": [2, 5]
}

# GridSearchCV
rf: RandomForestClassifier = RandomForestClassifier()
grid_search: GridSearchCV = GridSearchCV(
    rf,
    param_grid,
    cv=5,
    scoring="accuracy",
    n_jobs=-1  # Use all CPU cores!
)

grid_search.fit(X, y)

print(f"Best parameters: {grid_search.best_params_}")
print(f"Best score: {grid_search.best_score_:.3f}")
```

### Output

```
Best parameters: {'max_depth': 10, 'n_estimators': 50, 'min_samples_split': 2}
Best score: 0.690
```

### 💡 Line-by-Line Breakdown

- `param_grid` - Dictionary of parameter options to try
- `cv=5` - 5-fold cross-validation
- `scoring="accuracy"` - Metric to optimize
- `n_jobs=-1` - Parallel processing (fast!)
- `.best_params_` - Best hyperparameter values

## RandomizedSearchCV: Random Sampling

When grid is too big, sample randomly:

```python
from sklearn.model_selection import RandomizedSearchCV
from scipy.stats import randint, uniform

# Define parameter distributions (not a grid!)
param_dist: dict = {
    "n_estimators": randint(10, 200),
    "max_depth": randint(3, 20),
    "learning_rate": uniform(0.01, 0.3),
}

# RandomizedSearchCV
random_search: RandomizedSearchCV = RandomizedSearchCV(
    RandomForestClassifier(),
    param_dist,
    n_iter=20,  # Try 20 random combinations
    cv=5,
    random_state=42
)

random_search.fit(X, y)
print(f"Best params: {random_search.best_params_}")
```

## Bias-Variance Tradeoff

This is **the most important concept** in ML!

```
                    Error
                      │
   High Variance     │    ╭────────────
   (Overfitting)     │   ╱
                     │  ╱
                     │ ╱
                     │╱
        Best Model   ════════
                     │╲
                     │ ╲
                     │  ╲
   High Bias         │   ╲
   (Underfitting)    │    ╰──────────
                     │
                     └────────────────────
                        Model Complexity
```

| Problem | Symptoms | Fix |
|---------|----------|-----|
| **Overfitting** | Train good, test bad | More data, less features, regularization |
| **Underfitting** | Train bad, test bad | More features, more complex model |

## Learning Curves

Diagnose overfitting/underfitting:

```python
import matplotlib.pyplot as plt
import numpy as np
from sklearn.model_selection import learning_curve
from sklearn.ensemble import RandomForestClassifier

# Data
X: np.ndarray = np.random.randn(200, 5)
y: np.ndarray = np.random.choice([0, 1], 200)

# Learning curve
train_sizes: np.ndarray
train_scores: np.ndarray
test_scores: np.ndarray

train_sizes, train_scores, test_scores = learning_curve(
    RandomForestClassifier(n_estimators=50),
    X, y,
    train_sizes=np.linspace(0.1, 1.0, 10),
    cv=5,
    scoring="accuracy"
)

# Plot
train_mean: np.ndarray = train_scores.mean(axis=1)
test_mean: np.ndarray = test_scores.mean(axis=1)

plt.figure(figsize=(10, 6))
plt.plot(train_sizes, train_mean, "o-", color="blue", label="Training Score")
plt.plot(train_sizes, test_mean, "o-", color="orange", label="Validation Score")
plt.xlabel("Training Set Size")
plt.ylabel("Accuracy")
plt.title("Learning Curve")
plt.legend()
plt.grid(True)
plt.show()
```

### Visual: Diagnosing Problems

```
Good Fit:                          Overfitting:
─────────                          ───────────
  ↑ Accuracy                          ↑ Accuracy
  │                                    │
  │  ╭───────────                      │ ╭─────
  │ ╱           ╲                     │╱     
  │╱             ╲                     │       ───
  └─────────────────                  └──────────
     Training size                       Training size

Gap between lines = Variance           Large gap = Overfitting!


Underfitting:
─────────────
  ↑ Accuracy
  │
  │ ─────────────
  │
  │ _________________
  └──────────────────
     Training size

Both lines low = Underfitting!
```

## ✅ Summary

- **Cross-validation**: Use multiple train/test splits for robust evaluation
- **cv=5** is a good default
- **GridSearchCV**: Try all hyperparameter combinations
- **RandomizedSearchCV**: Random sampling when space is large
- **Bias-Variance Tradeoff**: Underfitting vs overfitting
- **Learning Curves**: Diagnose model problems

## ➡️ Next Steps

Ready for deep learning? Head to **[../../11_Deep_Learning_Intro/01_Neural_Networks/01_what_are_neural_networks.md](../../11_Deep_Learning_Intro/01_Neural_Networks/01_what_are_neural_networks.md)**!

## 🔗 Further Reading

- [Cross-Validation](https://scikit-learn.org/stable/modules/cross_validation.html)
- [GridSearchCV](https://scikit-learn.org/stable/modules/generated/sklearn.model_selection.GridSearchCV.html)
- [Learning Curves](https://scikit-learn.org/stable/auto_examples/model_selection/plot_learning_curve.html)
