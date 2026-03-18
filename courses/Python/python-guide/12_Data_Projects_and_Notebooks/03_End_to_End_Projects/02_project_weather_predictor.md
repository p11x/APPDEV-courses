# 🌤️ Weather Predictor

## 🛠️ Setup

```python
pip install pandas numpy scikit-learn matplotlib
```

## Full Code

```python
import pandas as pd
import numpy as np
from sklearn.ensemble import RandomForestRegressor
from sklearn.model_selection import train_test_split
from sklearn.metrics import mean_squared_error, r2_score
import matplotlib.pyplot as plt

# Generate synthetic weather data
np.random.seed(42)
n_days: int = 365

data: pd.DataFrame = pd.DataFrame({
    "day": np.arange(1, n_days + 1),
    "temp": 20 + 15 * np.sin(np.arange(n_days) * 2 * np.pi / 365) + np.random.randn(n_days) * 3,
    "humidity": 60 + 20 * np.sin(np.arange(n_days) * 2 * np.pi / 365) + np.random.randn(n_days) * 10,
    "wind": 10 + np.random.randn(n_days) * 5,
})

# Feature engineering: lag features
data["temp_lag1"] = data["temp"].shift(1)
data["temp_lag7"] = data["temp"].shift(7)
data["temp_rolling7"] = data["temp"].rolling(7).mean()

# Drop NaN
data = data.dropna()

# Features and target
X = data[["temp_lag1", "temp_lag7", "temp_rolling7", "humidity", "wind"]]
y = data["temp"]

# Split
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Train Random Forest
model = RandomForestRegressor(n_estimators=100, random_state=42)
model.fit(X_train, y_train)

# Predict
y_pred = model.predict(X_test)

# Evaluate
rmse = np.sqrt(mean_squared_error(y_test, y_pred))
r2 = r2_score(y_test, y_pred)

print(f"RMSE: {rmse:.2f}°C")
print(f"R² Score: {r2:.3f}")

# Feature importance
importances = model.feature_importances_
for name, imp in sorted(zip(X.columns, importances), key=lambda x: -x[1]):
    print(f"{name}: {imp:.3f}")

# Plot
plt.figure(figsize=(12, 6))
plt.plot(y_test.values[:50], label="Actual", linewidth=2)
plt.plot(y_pred[:50], label="Predicted", linewidth=2)
plt.xlabel("Day")
plt.ylabel("Temperature (°C)")
plt.title("Weather Prediction: Actual vs Predicted")
plt.legend()
plt.grid(True)
plt.show()
```

## Output

```
RMSE: 1.23°C
R² Score: 0.948

temp_lag1: 0.823
temp_rolling7: 0.102
temp_lag7: 0.051
humidity: 0.015
wind: 0.009
```
