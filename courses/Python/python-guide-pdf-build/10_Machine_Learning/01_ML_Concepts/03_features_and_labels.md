# 🎯 Features and Labels

## 🎯 What You'll Learn

- What features (X) and labels (y) actually are
- Feature types: numerical, categorical, text
- Feature scaling: why and how
- Encoding categorical variables
- Using sklearn Pipeline

## 📦 Prerequisites

- Read [02_train_test_split.md](./02_train_test_split.md) first

## Features vs Labels

This is the **most important concept** in ML!

```
┌─────────────────────────────────────────────────────────┐
│                    YOUR DATA                              │
├─────────────────────────────┬───────────────────────────┤
│      FEATURES (X)           │        LABEL (y)          │
│  (Input - what we use)      │  (Output - what we want) │
├─────────────────────────────┼───────────────────────────┤
│  square_feet: 1500          │  price: $450,000         │
│  bedrooms: 3                │                           │
│  bathrooms: 2              │  PRICE to predict!       │
│  age: 10                   │                           │
│  location: "suburb"        │                           │
├─────────────────────────────┴───────────────────────────┤
│                                                         │
│  Model: X (features) → [???] → y (label)               │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

## Feature Types

### 1. Numerical Features

Numbers — either discrete (count) or continuous (measurement):

```python
# Examples:
age: int = 30          # Discrete
price: float = 19.99   # Continuous
rating: int = 5        # Discrete (1-5 scale)
```

### 2. Categorical Features

Finite set of categories:

```python
# Examples:
color: str = "red"     # From: red, blue, green
city: str = "NYC"      # From: NYC, LA, Chicago
grade: str = "A"       # From: A, B, C, D, F
```

### 3. Text Features

Raw text (needs special processing):

```python
# Examples:
review: str = "This product was amazing!"
email: str = "Hello, I have a question..."
```

### 4. Date/Time Features

Timestamps can be decomposed:

```python
# From a date like "2024-01-15 14:30:00"
year: int = 2024       # Extracted
month: int = 1         # Extracted
day_of_week: str = "Monday"  # Extracted
hour: int = 14         # Extracted
is_weekend: bool = False      # Derived
```

## Feature Scaling

**Why?** Because models care about the **scale** of numbers!

### The Problem

```
Height (cm):      150 - 200      Range: 50
Weight (kg):       50 - 150      Range: 100
Age:               0 - 100       Range: 100
```

The model might think **weight** is more important because it's on a larger scale!

### Two Common Scalers

```python
from sklearn.preprocessing import StandardScaler, MinMaxScaler
import numpy as np

# Original data
X: np.ndarray = np.array([
    [150, 50, 25],
    [170, 70, 35],
    [180, 80, 45],
    [160, 60, 30]
])

# StandardScaler: mean=0, std=1
scaler: StandardScaler = StandardScaler()
X_standardized: np.ndarray = scaler.fit_transform(X)
print("Standardized:")
print(X_standardized)

# MinMaxScaler: range 0-1
minmax: MinMaxScaler = MinMaxScaler()
X_normalized: np.ndarray = minmax.fit_transform(X)
print("\nNormalized:")
print(X_normalized)
```

### When to Use Which?

- **StandardScaler**: Most algorithms (SVM, KNN, Neural Networks)
- **MinMaxScaler**: Images (pixel values 0-255), algorithms that need bounded values

## Encoding Categorical Variables

ML needs numbers! Convert categories to numbers:

### One-Hot Encoding (get_dummies)

```python
import pandas as pd

# Data with categorical
df: pd.DataFrame = pd.DataFrame({
    "color": ["red", "blue", "green", "red"],
    "size": ["S", "M", "L", "S"]
})

# One-hot encode
encoded: pd.DataFrame = pd.get_dummies(df, columns=["color"])
print(encoded)
```

### Output

```
   size  color_blue  color_green  color_red
0    S           0            0           1
1    M           1            0           0
2    L           0            1           0
3    S           0            0           1
```

### LabelEncoder

For ordinal categories (with inherent order):

```python
from sklearn.preprocessing import LabelEncoder

# Encode single column
le: LabelEncoder = LabelEncoder()
sizes: list[str] = ["S", "M", "L", "S", "M"]
encoded_sizes: np.ndarray = le.fit_transform(sizes)

print(encoded_sizes)  # [2 1 0 2 1]

# Map back
print(le.inverse_transform([0, 1, 2]))  # ['L', 'M', 'S']
```

## Pipeline: Chain Everything!

Pipeline lets you chain preprocessing and model in one:

```python
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler, OneHotEncoder
from sklearn.compose import ColumnTransformer
from sklearn.linear_model import LogisticRegression
import pandas as pd
import numpy as np

# Sample data with mixed types
df: pd.DataFrame = pd.DataFrame({
    "age": [25, 30, 35, 40],
    "income": [50000, 60000, 75000, 80000],
    "city": ["NYC", "LA", "NYC", "Chicago"]
})

# Define which columns are numerical vs categorical
numerical_cols: list[str] = ["age", "income"]
categorical_cols: list[str] = ["city"]

# Create preprocessor
preprocessor: ColumnTransformer = ColumnTransformer(
    transformers=[
        ("num", StandardScaler(), numerical_cols),
        ("cat", OneHotEncoder(), categorical_cols)
    ]
)

# Create pipeline
pipeline: Pipeline = Pipeline([
    ("preprocess", preprocessor),
    ("model", LogisticRegression())
])

# Fit on data
X: np.ndarray = df.values
y: np.ndarray = np.array([0, 1, 0, 1])

pipeline.fit(X, y)

# Predict
predictions: np.ndarray = pipeline.predict(X)
print(predictions)
```

### 💡 Line-by-Line Breakdown

- `ColumnTransformer` - Apply different transforms to different columns
- `Pipeline` - Chain steps together
- `fit(X, y)` - Train the entire pipeline
- `predict(X)` - Preprocess + predict in one call

### 💡 Explanation

Pipeline is crucial because:
1. Preprocessing is applied consistently (no data leakage!)
2. Code is cleaner
3. Easier to save and deploy

## Complete Example

```python
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_squared_error
import pandas as pd
import numpy as np

# 1. Load data
df: pd.DataFrame = pd.DataFrame({
    "sqft": [1500, 1800, 2400, 2100, 1600],
    "bedrooms": [3, 4, 4, 3, 3],
    "age": [10, 5, 2, 8, 15],
    "price": [450000, 520000, 680000, 580000, 420000]
})

# 2. Split features and label
X: pd.DataFrame = df[["sqft", "bedrooms", "age"]]
y: pd.Series = df["price"]

# 3. Train/test split
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

# 4. Scale features
scaler: StandardScaler = StandardScaler()
X_train_scaled: np.ndarray = scaler.fit_transform(X_train)
X_test_scaled: np.ndarray = scaler.transform(X_test)

# 5. Train model
model: LinearRegression = LinearRegression()
model.fit(X_train_scaled, y_train)

# 6. Evaluate
y_pred: np.ndarray = model.predict(X_test_scaled)
mse: float = mean_squared_error(y_test, y_pred)
rmse: float = np.sqrt(mse)
print(f"RMSE: ${rmse:,.2f}")
```

## ✅ Summary

- **Features (X)** = input variables to predict from
- **Label (y)** = what we want to predict
- **Numerical**: continuous or discrete numbers
- **Categorical**: finite categories
- **Feature scaling**: StandardScaler or MinMaxScaler
- **Encoding**: get_dummies for nominal, LabelEncoder for ordinal
- **Pipeline**: Chain preprocessing + model for clean code

## ➡️ Next Steps

Ready for your first ML algorithm? Head to **[../02_Classic_Algorithms/01_linear_and_logistic_regression.md](../02_Classic_Algorithms/01_linear_and_logistic_regression.md)**!

## 🔗 Further Reading

- [sklearn Preprocessing](https://scikit-learn.org/stable/modules/preprocessing.html)
- [ColumnTransformer](https://scikit-learn.org/stable/modules/generated/sklearn.compose.ColumnTransformer.html)
- [Pipeline](https://scikit-learn.org/stable/modules/pipeline.html)
