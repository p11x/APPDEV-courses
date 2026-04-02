# 📉 Regression Metrics

## 🎯 What You'll Learn

- MAE: Average error distance
- MSE: Penalize big errors
- RMSE: Back in original units
- R² Score: How much variance is explained?

## 📦 Prerequisites

- Read [01_classification_metrics.md](./01_classification_metrics.md) first

## Regression vs Classification

- **Classification**: Predict a category (spam/not spam)
- **Regression**: Predict a number (price, temperature)

## Metrics for Regression

### MAE: Mean Absolute Error

**"On average, how wrong are we?"**

```python
from sklearn.metrics import mean_absolute_error
import numpy as np

# Actual house prices
y_true: np.ndarray = np.array([450000, 520000, 680000, 580000, 420000])

# Predicted prices  
y_pred: np.ndarray = np.array([440000, 550000, 650000, 600000, 410000])

mae: float = mean_absolute_error(y_true, y_pred)
print(f"MAE: ${mae:,.0f}")  # $30,000
```

### Interpretation

- MAE = $30,000 → On average, predictions are **$30,000 off**
- Easy to understand!
- Same units as the target variable

### MSE: Mean Squared Error

**"Penalize big errors more!"**

```python
from sklearn.metrics import mean_squared_error

mse: float = mean_squared_error(y_true, y_pred)
print(f"MSE: ${mse:,.0f}")  # 1,100,000,000

# MSE is in DOLLARS² (weird units!)
# That's why we use RMSE instead
```

### RMSE: Root Mean Squared Error

**"Back in original units!"**

```python
rmse: float = np.sqrt(mse)
print(f"RMSE: ${rmse:,.0f}")  # $33,166

# Or directly:
from sklearn.metrics import mean_squared_error
rmse: float = mean_squared_error(y_true, y_pred, squared=False)
print(f"RMSE: ${rmse:,.0f}")  # $33,166
```

### 💡 Why RMSE?

- RMSE penalizes **big errors more** than small ones
- More sensitive to outliers than MAE
- Back in original units!

### MAE vs RMSE Example

```python
# Predictions: off by [10, 10, 10, 100]
errors: np.ndarray = np.array([10, 10, 10, 100])

mae: float = np.mean(np.abs(errors))     # = 32.5
rmse: float = np.sqrt(np.mean(errors**2))  # = 52.2

print(f"MAE:  {mae:.1f}")
print(f"RMSE: {rmse:.1f}")

# The one big error (100) pulls RMSE up more!
```

## R² Score: Coefficient of Determination

**"How much of the variance does our model explain?"**

```
R² = 1 - (SS_res / SS_tot)
```

- **R² = 1**: Perfect prediction
- **R² = 0**: As good as predicting the mean
- **R² < 0**: Worse than predicting the mean!

```python
from sklearn.metrics import r2_score

y_true: np.ndarray = np.array([450000, 520000, 680000, 580000, 420000])
y_pred: np.ndarray = np.array([440000, 550000, 650000, 600000, 410000])

r2: float = r2_score(y_true, y_pred)
print(f"R² Score: {r2:.3f}")  # 0.975
```

### 💡 Interpretation

- R² = 0.975 → Model explains **97.5%** of the variance
- Only 2.5% unexplained!

## All Metrics Together

```python
from sklearn.linear_model import LinearRegression
from sklearn.model_selection import train_test_split
from sklearn.metrics import mean_absolute_error, mean_squared_error, r2_score
import numpy as np

# Simple data: square footage → price
X: np.ndarray = np.array([[1500], [1800], [2400], [2100], [1600], [2000]])
y: np.ndarray = np.array([450000, 520000, 680000, 580000, 420000, 550000])

# Split
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.33, random_state=42
)

# Train
model: LinearRegression = LinearRegression()
model.fit(X_train, y_train)

# Predict
y_pred: np.ndarray = model.predict(X_test)

# Calculate all metrics
mae: float = mean_absolute_error(y_test, y_pred)
mse: float = mean_squared_error(y_test, y_pred)
rmse: float = np.sqrt(mse)
r2: float = r2_score(y_test, y_pred)

print("╔════════════════════════════════╗")
print("║     REGRESSION METRICS        ║")
print("╠════════════════════════════════╣")
print(f"║ MAE:  ${mae:>12,.0f}       ║")
print(f"║ MSE:  ${mse:>12,.0f}       ║")
print(f"║ RMSE: ${rmse:>12,.0f}       ║")
print(f"║ R²:   {r2:>14.3f}       ║")
print("╚════════════════════════════════╝")
```

## Residual Plot

**Visualize prediction errors!**

```python
import matplotlib.pyplot as plt
import numpy as np
from sklearn.linear_model import LinearRegression

# Data
X: np.ndarray = np.array([[1500], [1800], [2400], [2100], [1600], [2000], [3000], [2500]])
y: np.ndarray = np.array([450000, 520000, 680000, 580000, 420000, 550000, 750000, 700000])

# Train
model: LinearRegression = LinearRegression()
model.fit(X, y)
y_pred: np.ndarray = model.predict(X)

# Residuals (errors)
residuals: np.ndarray = y - y_pred

# Plot
fig, ax = plt.subplots(figsize=(10, 6))
ax.scatter(y_pred, residuals, alpha=0.7, color="#FF6B6B")
ax.axhline(y=0, color="black", linestyle="--")
ax.set_xlabel("Predicted Values")
ax.set_ylabel("Residuals (Actual - Predicted)")
ax.set_title("Residual Plot")

plt.show()

# Good residual plot: random scatter around 0
# Bad: funnel shape (non-constant variance)
```

## ✅ Summary

| Metric | What It Measures | Sensitive to Outliers? |
|--------|------------------|------------------------|
| MAE | Average absolute error | No |
| MSE | Squared error | Yes (penalizes big errors!) |
| RMSE | √MSE, original units | Yes |
| R² | Variance explained (0-1) | N/A |

## ➡️ Next Steps

Ready to make your model bulletproof? Head to **[03_cross_validation_and_tuning.md](./03_cross_validation_and_tuning.md)**!

## 🔗 Further Reading

- [sklearn Regression Metrics](https://scikit-learn.org/stable/modules/classes.html#regression-metrics)
- [R² Explained](https://en.wikipedia.org/wiki/Coefficient_of_determination)
- [Residual Analysis](https://statisticsbyjim.com/regression/residual-plots/)
