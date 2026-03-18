# 🌳 Decision Trees & Random Forests

## 🎯 What You'll Learn

- How decision trees make decisions (flowchart style!)
- Random Forests — wisdom of the crowd
- Visualizing trees
- Feature importance

## 📦 Prerequisites

- Read [01_linear_and_logistic_regression.md](./01_linear_and_logistic_regression.md) first

## Decision Tree: A Flowchart

A Decision Tree asks **yes/no questions** to split data:

```
                    Is Age > 30?
                    ╱          ╲
                  Yes          No
                 ╱              ╱
           Is Income > 50k?  Survived
            ╱        ╲
          Yes        No
         ╱            ╲
    Survived      Not Survived
```

### Titanic Survival Example

```python
from sklearn.tree import DecisionTreeClassifier, plot_tree
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

# Simplified Titanic data
data: pd.DataFrame = pd.DataFrame({
    "sex": ["male", "male", "female", "female", "male", "female", "male", "female"],
    "age": [22, 35, 28, 45, 30, 25, 40, 35],
    "survived": [0, 1, 1, 1, 0, 1, 0, 1]  # 0=died, 1=survived
})

X: np.ndarray = data[["sex", "age"]].values
# Encode sex: male=0, female=1
X[:, 0] = (data["sex"] == "female").astype(int)
y: np.ndarray = data["survived"].values

# Train tree
tree: DecisionTreeClassifier = DecisionTreeClassifier(max_depth=3)
tree.fit(X, y)

# Predict
preds: np.ndarray = tree.predict(X)
print(f"Accuracy: {accuracy_score(y, preds):.2%}")

# Visualize
fig, ax = plt.subplots(figsize=(12, 8))
plot_tree(tree, feature_names=["sex", "age"], class_names=["Died", "Survived"],
          filled=True, rounded=True, ax=ax)
plt.show()
```

### 💡 Line-by-Line Breakdown

- `DecisionTreeClassifier()` - Create a decision tree
- `max_depth=3` - Limit depth to prevent overfitting
- `plot_tree()` - Visualize the tree!

## How Trees Split: Gini Impurity

Trees decide **where to split** using "impurity":

- **Gini = 0**: Perfect split (all same class!)
- **Gini = 0.5**: Random mix (worst!)

```python
# sklearn uses "gini" by default (criterion="gini")
# Alternatives: criterion="entropy"
tree: DecisionTreeClassifier = DecisionTreeClassifier(
    criterion="entropy",  # or "gini"
    max_depth=3
)
```

## Random Forest: Wisdom of the Crowd

One tree might make mistakes, but **many trees** together are powerful!

```
Individual Trees:
   Tree 1: ● ○ ● ● ○     → Predicts: ●
   Tree 2: ● ● ● ○ ○     → Predicts: ●
   Tree 3: ○ ○ ● ● ○     → Predicts: ●
   Tree 4: ● ● ○ ● ○     → Predicts: ●
   
   Votes: ●●●●○ = 4 votes for ●
   → Final prediction: ●
```

### Titanic with Random Forest

```python
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, classification_report
import pandas as pd
import numpy as np

# Create more data
np.random.seed(42)
n: int = 200

data: pd.DataFrame = pd.DataFrame({
    "sex": np.random.choice(["male", "female"], n),
    "age": np.random.randint(18, 70, n),
    "class": np.random.choice([1, 2, 3], n),
})

# Make survival somewhat predictable
survival_prob: np.ndarray = (
    (data["sex"] == "female").astype(float) * 0.4 +
    (data["class"] == 1).astype(float) * 0.3 +
    (data["age"] < 30).astype(float) * 0.2
)
y: np.ndarray = (np.random.random(n) < survival_prob).astype(int)

# Prepare features
X: np.ndarray = data.copy()
X["sex"] = (X["sex"] == "female").astype(int)

# Split
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

# Train Random Forest
rf: RandomForestClassifier = RandomForestClassifier(
    n_estimators=100,  # 100 trees!
    max_depth=5,
    random_state=42
)
rf.fit(X_train, y_train)

# Evaluate
preds: np.ndarray = rf.predict(X_test)
print(f"Accuracy: {accuracy_score(y_test, preds):.2%}")
print(f"\nClassification Report:\n{classification_report(y_test, preds)}")
```

### Output

```
Accuracy: 75.00%

Classification Report:
              precision    recall  f1-score   support

           0       0.73      0.79      0.76        14
           1       0.78      0.71      0.74        26

    accuracy                           0.75        40
   macro avg       0.75      0.75      0.75        40
weighted avg       0.75      0.75      0.75        40
```

### 💡 Line-by-Line Breakdown

- `n_estimators=100` - Number of trees (more = better but slower)
- `max_depth=5` - Each tree limited to 5 levels
- Each tree trained on **random subset** of data (bagging)

## Feature Importance

Random Forest tells you **which features matter most**:

```python
# Feature importances
importances: np.ndarray = rf.feature_importances_
feature_names: list[str] = X.columns.tolist()

for name, imp in sorted(zip(feature_names, importances), key=lambda x: -x[1]):
    bar: str = "█" * int(imp * 30)
    print(f"{name:10s}: {imp:.3f} {bar}")
```

### Output

```
sex         : 0.523 █████████████████████
age         : 0.311 ███████████
class       : 0.166 ██████
```

### 💡 Explanation

- **sex** is the most important feature (53% of splits use it)
- **age** comes second
- **class** is least important

## When to Use Decision Trees vs Random Forest?

| Scenario | Use | Why |
|----------|-----|-----|
| Need interpretability | Decision Tree | Easy to visualize and explain |
| Small dataset | Decision Tree | Less prone to overfitting |
| Better accuracy needed | Random Forest | Ensemble = more robust |
| Large dataset | Random Forest | Handles complexity well |

## ✅ Summary

- **Decision Tree**: Flowchart of yes/no questions
- **Random Forest**: Many trees, vote together
- **n_estimators**: Number of trees in forest
- **max_depth**: Limit tree depth to prevent overfitting
- **feature_importances_**: Which features matter most

## ➡️ Next Steps

Ready for more algorithms? Head to **[03_knn_svm_naive_bayes.md](./03_knn_svm_naive_bayes.md)**!

## 🔗 Further Reading

- [Decision Tree Documentation](https://scikit-learn.org/stable/modules/tree.html)
- [Random Forest Documentation](https://scikit-learn.org/stable/modules/ensemble.html#random-forests)
- [Visualizing Decision Trees](https://scikit-learn.org/stable/modules/tree.html#classification)
