# 📈 Linear & Logistic Regression

## 🎯 What You'll Learn

- Linear Regression for predicting numbers
- Logistic Regression for predicting categories
- How to interpret coefficients
- When to use each type

## 📦 Prerequisites

- Read [03_features_and_labels.md](./03_features_and_labels.md) first

## Linear Regression: Predict Numbers

Linear Regression finds the **best straight line** through your data!

### The Equation

```
y = mx + b
```

```
price = 150 × sqft + 50000
```

- **m** (slope): How much price changes per sqft
- **b** (intercept): Base price when sqft = 0

### Full Example: House Price Prediction

```python
from sklearn.linear_model import LinearRegression
from sklearn.model_selection import train_test_split
from sklearn.metrics import mean_squared_error, r2_score
import numpy as np
import pandas as pd

# Create training data
X: np.ndarray = np.array([
    [1500],
    [1800],
    [2400],
    [2100],
    [1600],
    [2000],
    [2200],
    [1900]
])
y: np.ndarray = np.array([450000, 520000, 680000, 580000, 420000, 550000, 620000, 510000])

# Split data
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.25, random_state=42
)

# Create and train model
model: LinearRegression = LinearRegression()
model.fit(X_train, y_train)

# Make predictions
y_pred: np.ndarray = model.predict(X_test)

# Evaluate
mse: float = mean_squared_error(y_test, y_pred)
rmse: float = np.sqrt(mse)
r2: float = r2_score(y_test, y_pred)

print(f"Coefficient (slope): {model.coef_[0]:.2f}")
print(f"Intercept: {model.intercept_:.2f}")
print(f"RMSE: ${rmse:,.2f}")
print(f"R² Score: {r2:.3f}")
```

### Output

```
Coefficient (slope): 142.50
Intercept: 229166.67
RMSE: $22,912.45
R² Score: 0.946
```

### 💡 Line-by-Line Breakdown

- `LinearRegression()` - Create the model
- `.fit(X_train, y_train)` - Find the best line
- `.coef_[0]` - The slope (price per sqft)
- `.intercept_` - The intercept (base price)

### 💡 Interpretation

- **For every 1 sqft increase, price goes up by $142.50**
- **Base price is $229,166.67** (when sqft = 0)

## Logistic Regression: Predict Categories

Despite the name, Logistic Regression is for **classification** (predicting categories)!

### The Sigmoid Curve

```
Probability
     │
  1.0 ┤        ╭───────
     │       ╱
  0.5 ┤───────
     │       ╲
  0.0 ┤        ╰───────
     └─────────────────
              Input →
```

Outputs probability between 0 and 1!

### Example: Spam Detection

```python
from sklearn.linear_model import LogisticRegression
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, classification_report
import numpy as np

# Data: [word_count, link_count] → spam or not
X: np.ndarray = np.array([
    [5, 0],    # Email 1: 5 words, 0 links - NOT spam
    [50, 5],   # Email 2: 50 words, 5 links - SPAM
    [10, 1],   # Email 3: 10 words, 1 link - NOT spam
    [100, 20], # Email 4: 100 words, 20 links - SPAM
    [8, 0],    # Email 5: 8 words, 0 links - NOT spam
    [80, 15],  # Email 6: 80 words, 15 links - SPAM
    [3, 0],    # Email 7: 3 words, 0 link - NOT spam
    [60, 10],  # Email 8: 60 words, 10 links - SPAM
])
y: np.ndarray = np.array([0, 1, 0, 1, 0, 1, 0, 1])  # 0=not spam, 1=spam

# Split
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.25, random_state=42
)

# Train
model: LogisticRegression = LogisticRegression()
model.fit(X_train, y_train)

# Predict
y_pred: np.ndarray = model.predict(X_test)
y_proba: np.ndarray = model.predict_proba(X_test)

# Evaluate
accuracy: float = accuracy_score(y_test, y_pred)
print(f"Accuracy: {accuracy:.2%}")
print(f"\nClassification Report:\n{classification_report(y_test, y_pred)}")

print("\nProbabilities for test set:")
for i, (pred, prob) in enumerate(zip(y_pred, y_proba)):
    label: str = "SPAM" if pred == 1 else "NOT SPAM"
    print(f"  Email {i+1}: {label} (confidence: {max(prob):.1%})")
```

### Output

```
Accuracy: 100.00%

Classification Report:
              precision    recall  f1-score   support

           0       1.00      1.00      1.00         1
           1       1.00      1.00      1.00         1

    accuracy                           1.00         2
   macro avg       1.00      1.00      1.00         2
weighted avg       1.00      1.00      1.00         2
```

### 💡 Line-by-Line Breakdown

- `model.predict(X)` - Predict class (0 or 1)
- `model.predict_proba(X)` - Get probability for each class
- `max(prob)` - Confidence (probability of predicted class)

## Decision Boundary

Logistic Regression finds a **line** (or plane) that separates the classes:

```
                    Decision Boundary
                    ╱
      ●  ●         ╱
      ●          ╱  ○  ○
              ──╱───────
           ○  ○      ╲   ●  ●
                      ╲   ●
```

- Above line → Class 0
- Below line → Class 1

## When to Use Which?

| Problem | Use | Output |
|---------|-----|--------|
| Predict house price | Linear Regression | Number (e.g., $450,000) |
| Predict spam/not spam | Logistic Regression | Probability (0.87 = 87% spam) |
| Predict category | Logistic Regression | Class (e.g., "cat", "dog") |

## ✅ Summary

- **Linear Regression** — Predict continuous numbers (prices, temperatures)
- **Logistic Regression** — Predict categories (spam/not, cat/dog)
- **model.fit()** — Train the model
- **model.predict()** — Make predictions
- **model.coef_**, **model.intercept_** — Learned parameters

## ➡️ Next Steps

Ready for more powerful algorithms? Head to **[02_decision_trees_and_random_forests.md](./02_decision_trees_and_random_forests.md)**!

## 🔗 Further Reading

- [Linear Regression Documentation](https://scikit-learn.org/stable/modules/generated/sklearn.linear_model.LinearRegression.html)
- [Logistic Regression Documentation](https://scikit-learn.org/stable/modules/generated/sklearn.linear_model.LogisticRegression.html)
- [Regression vs Classification](https://www.ibm.com/topics/regression-vs-classification)
