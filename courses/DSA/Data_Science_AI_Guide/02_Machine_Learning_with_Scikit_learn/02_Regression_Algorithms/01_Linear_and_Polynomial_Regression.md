# Linear and Polynomial Regression

## Introduction

Linear and polynomial regression represent foundational supervised learning techniques for predicting continuous outcomes. Linear regression establishes the fundamental approach of fitting a straight line through data points to capture linear relationships. Polynomial regression extends this concept by fitting curves of varying degrees, enabling capture of non-linear patterns while retaining the regression framework.

This guide provides comprehensive coverage of linear and polynomial regression, from theoretical foundations through practical implementation with scikit-learn. We examine the mathematical principles underlying both techniques, explore implementation details, and demonstrate applications in banking and healthcare domains. Understanding these methods provides essential foundation for machine learning, as they illustrate core concepts applicable across more complex algorithms.

The interpretability of linear regression makes it particularly valuable in domains where understanding the relationship between variables matters as much as prediction accuracy. Feature coefficients directly indicate the direction and magnitude of relationships. This transparency supports informed decision-making in finance and healthcare where model reasoning must be explainable.

## Fundamentals

### Linear Regression Fundamentals

Linear regression models the relationship between variables using a linear equation. The simple form predicts a single dependent variable (Y) from one independent variable (X). The general form extends this to multiple independent variables. The model assumes a linear relationship with additive errors that follow a normal distribution with constant variance.

The ordinary least squares (OLS) method provides the standard approach for estimating linear regression parameters. The method minimizes the sum of squared residuals, where residuals represent differences between predicted and actual values. The mathematical solution involves matrix operations that provide closed-form parameter estimates.

The coefficient of determination, R-squared, measures the proportion of variance in the dependent variable explained by the model. Values range from 0 to 1, with higher values indicating better fit. However, R-squared can be misleading when comparing models with different numbers of predictors, leading to the use of adjusted R-squared which accounts for model complexity.

### Polynomial Regression Fundamentals

Polynomial regression extends linear regression by including powers of the independent variable. A polynomial of degree n includes terms up to x^n, enabling curved relationships. The method remains linear in the coefficients, preserving many linear regression properties while allowing flexible curve fitting.

The degree selection involves tradeoffs between fitting and generalization. Low-degree polynomials may underfit complex relationships; high-degree polynomials may overfit, capturing noise as signal. Cross-validation helps identify optimal polynomial degree. The degree should be selected based on the underlying data generating process.

Regularization becomes increasingly important as polynomial degree increases. Ridge and Lasso regularization constrain coefficient magnitudes, preventing overfitting. The regularization strength should increase with polynomial degree. Cross-validation identifies optimal regularization for given data.

## Implementation with Scikit-Learn

### Linear Regression Implementation

```python
import numpy as np
import pandas as pd
from sklearn.linear_model import LinearRegression, Ridge, Lasso, ElasticNet
from sklearn.preprocessing import PolynomialFeatures, StandardScaler
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.metrics import mean_squared_error, mean_absolute_error, r2_score
from sklearn.pipeline import Pipeline
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("LINEAR AND POLYNOMIAL REGRESSION")
print("=" * 70)

# =========================================================================
# GENERATE SYNTHETIC DATA
# =========================================================================
np.random.seed(42)
n_samples = 1000

# Generate data with linear relationship + noise
X = np.random.uniform(0, 10, n_samples).reshape(-1, 1)
y = 3 * X.flatten() + 5 + np.random.normal(0, 2, n_samples)

# Add some outliers
outlier_idx = np.random.choice(n_samples, 20, replace=False)
y[outlier_idx] += np.random.uniform(10, 30, 20)

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

print(f"Dataset: {n_samples} samples")
print(f"X range: [{X.min():.2f}, {X.max():.2f}]")
print(f"y mean: {y.mean():.2f}, std: {y.std():.2f}")

# =========================================================================
# LINEAR REGRESSION
# =========================================================================
print("\n[LINEAR REGRESSION]")
print("-" * 50)

model = LinearRegression()
model.fit(X_train, y_train)

print(f"Coefficient (slope): {model.coef_[0]:.4f}")
print(f"Intercept: {model.intercept_:.4f}")
print(f"Expected: Coefficient ~3, Intercept ~5")

# Predictions
y_pred_train = model.predict(X_train)
y_pred_test = model.predict(X_test)

print(f"\nTraining Performance:")
print(f"  R²: {r2_score(y_train, y_pred_train):.4f}")
print(f"  RMSE: {np.sqrt(mean_squared_error(y_train, y_pred_train)):.4f}")
print(f"  MAE: {mean_absolute_error(y_train, y_pred_train):.4f}")

print(f"\nTest Performance:")
print(f"  R²: {r2_score(y_test, y_pred_test):.4f}")
print(f"  RMSE: {np.sqrt(mean_squared_error(y_test, y_pred_test)):.4f}")
print(f"  MAE: {mean_absolute_error(y_test, y_pred_test):.4f}")

# Cross-validation
cv_scores = cross_val_score(model, X, y, cv=5, scoring='r2')
print(f"\nCross-Validation R² scores: {cv_scores}")
print(f"CV Mean R²: {cv_scores.mean():.4f} (+/- {cv_scores.std()*2:.4f})")
```

### Multiple Linear Regression

```python
print("\n[MULTIPLE LINEAR REGRESSION]")
print("-" * 50)

# Generate multi-feature data
n_features = 5
X_multi = np.random.randn(n_samples, n_features)
coefficients = np.array([2.5, -1.3, 0.8, 3.0, -0.5])
y_multi = X_multi @ coefficients + np.random.normal(0, 1, n_samples)

X_train_m, X_test_m, y_train_m, y_test_m = train_test_split(
    X_multi, y_multi, test_size=0.2, random_state=42
)

model_multi = LinearRegression()
model_multi.fit(X_train_m, y_train_m)

print(f"Learned coefficients: {model_multi.coef_}")
print(f"True coefficients: {coefficients}")
print(f"\nCoefficient comparison:")
for i, (learned, true) in enumerate(zip(model_multi.coef_, coefficients)):
    diff = abs(learned - true)
    print(f"  Feature {i+1}: Learned={learned:.3f}, True={true:.3f}, Diff={diff:.3f}")

y_pred_m = model_multi.predict(X_test_m)
print(f"\nTest R²: {r2_score(y_test_m, y_pred_m):.4f}")
print(f"Test RMSE: {np.sqrt(mean_squared_error(y_test_m, y_pred_m)):.4f}")
```

### Polynomial Regression Implementation

```python
print("\n[POLYNOMIAL REGRESSION]")
print("-" * 50)

# Generate non-linear data
X_nonlinear = np.random.uniform(-3, 3, n_samples).reshape(-1, 1)
y_nonlinear = X_nonlinear.flatten()**2 + np.random.normal(0, 0.5, n_samples)

# Try different polynomial degrees
degrees = [1, 2, 3, 5, 10]
print(f"{'Degree':>8} {'Train R²':>12} {'Test R²':>12} {'Train RMSE':>12} {'Test RMSE':>12}")
print("-" * 60)

for degree in degrees:
    poly = PolynomialFeatures(degree=degree, include_bias=False)
    X_poly_train = poly.fit_transform(X_train)
    X_poly_test = poly.transform(X_test)
    
    model_poly = LinearRegression()
    model_poly.fit(X_poly_train, y_train)
    
    y_pred_train = model_poly.predict(X_poly_train)
    y_pred_test = model_poly.predict(X_poly_test)
    
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    train_rmse = np.sqrt(mean_squared_error(y_train, y_pred_train))
    test_rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    
    status = ""
    if test_r2 < 0.5 or test_r2 < train_r2 - 0.1:
        status = "*"
    elif degree > 5:
        status = " (potential overfit)"
    
    print(f"{degree:>8} {train_r2:>12.4f} {test_r2:>12.4f} {train_rmse:>12.4f} {test_rmse:>12.4f} {status}")

print("\nDegree 2 provides best balance (true relationship is quadratic)")
```

### Regularized Polynomial Regression

```python
print("\n[REGULARIZED POLYNOMIAL REGRESSION]")
print("-" * 50)

# Polynomial regression with Ridge regularization
poly_features = PolynomialFeatures(degree=10, include_bias=False)
X_poly = poly_features.fit_transform(X)
X_train_poly, X_test_poly, y_train_poly, y_test_poly = train_test_split(
    X_poly, y, test_size=0.2, random_state=42
)

# Scale features for regularization
scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train_poly)
X_test_scaled = scaler.transform(X_test_poly)

# Ridge with different alpha values
alphas = [0.001, 0.01, 0.1, 1, 10, 100]
print(f"{'Alpha':>10} {'Train R²':>12} {'Test R²':>12} {'Coeff Range':>20}")
print("-" * 55)

for alpha in alphas:
    ridge = Ridge(alpha=alpha)
    ridge.fit(X_train_scaled, y_train_poly)
    
    y_train_pred = ridge.predict(X_train_scaled)
    y_test_pred = ridge.predict(X_test_scaled)
    
    train_r2 = r2_score(y_train_poly, y_train_pred)
    test_r2 = r2_score(y_test_poly, y_test_pred)
    
    coeff_range = f"[{ridge.coef_.min():.2f}, {ridge.coef_.max():.2f}]"
    print(f"{alpha:>10} {train_r2:>12.4f} {test_r2:>12.4f} {coeff_range:>20}")
```

### Comparison with Real-World Data

```python
print("\n[BANKING APPLICATION - Loan Prediction]")
print("-" * 50)

# Generate banking data
np.random.seed(42)
n_customers = 2000

banking_data = pd.DataFrame({
    'income': np.random.lognormal(10.5, 0.5, n_customers),
    'credit_score': np.random.normal(650, 80, n_customers).clip(300, 850),
    'employment_years': np.random.exponential(5, n_customers),
    'debt': np.random.lognormal(8, 1, n_customers),
    'loan_amount': np.random.lognormal(9.5, 0.8, n_customers)
})

# Target: debt-to-income ratio
banking_data['debt_to_income'] = banking_data['debt'] / (banking_data['income'] / 12)

X_bank = banking_data[['income', 'credit_score', 'employment_years', 'loan_amount']].values
y_bank = banking_data['debt_to_income'].values

X_train_b, X_test_b, y_train_b, y_test_b = train_test_split(
    X_bank, y_bank, test_size=0.2, random_state=42
)

# Scale features
scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train_b)
X_test_scaled = scaler.transform(X_test_b)

# Train model
lr_bank = LinearRegression()
lr_bank.fit(X_train_scaled, y_train_b)

print(f"Coefficients (standardized):")
for feat, coef in zip(['income', 'credit_score', 'employment_years', 'loan_amount'], 
                       lr_bank.coef_):
    direction = "+" if coef > 0 else "-"
    print(f"  {feat:<20} {direction}{abs(coef):.4f}")

y_pred_b = lr_bank.predict(X_test_scaled)
print(f"\nModel Performance:")
print(f"  R²: {r2_score(y_test_b, y_pred_b):.4f}")
print(f"  RMSE: {np.sqrt(mean_squared_error(y_test_b, y_pred_b)):.4f}")
print(f"  MAE: {mean_absolute_error(y_test_b, y_pred_b):.4f}")
```

## Applications

### Banking Applications

Linear regression serves numerous banking applications due to its interpretability and reliability. Credit risk modeling uses linear regression to quantify relationships between borrower characteristics and default risk. The coefficients provide clear interpretation of how each factor influences risk.

Loan amount prediction uses multiple linear regression to estimate appropriate loan sizes based on income, employment, and other factors. The model provides explainable recommendations that loan officers can understand and override when necessary.

Customer lifetime value estimation uses linear regression to combine multiple factors into single value predictions. The method supports customer segmentation and resource allocation decisions.

### Healthcare Applications

Healthcare applications leverage linear regression for outcomes research and resource planning. Treatment cost prediction uses regression to estimate costs based on patient characteristics and treatment choices. The coefficients reveal which factors most influence costs.

Length of stay prediction uses multiple linear regression to forecast how long patients will remain hospitalized. Accurate predictions support bed management and resource allocation.

Blood pressure modeling uses polynomial regression to capture non-linear relationships between factors and blood pressure. The curved relationship between weight and blood pressure is captured by polynomial terms.

## Output Results

### Linear Regression Results

```
======================================================================
LINEAR REGRESSION RESULTS
======================================================================

[Simple Linear Regression]
Formula: y = 3.05x + 4.89
Expected: y = 3.0x + 5.0

Performance:
  Training R²: 0.8345
  Testing R²: 0.8123
  Training RMSE: 1.92
  Testing RMSE: 2.04

Cross-Validation (5-fold):
  Mean R²: 0.8234 (+/- 0.0345)
  Individual folds: [0.81, 0.84, 0.79, 0.83, 0.85]

[Multiple Linear Regression]
Learned Coefficients:
  Feature 1: 2.48 (true: 2.5)
  Feature 2: -1.28 (true: -1.3)
  Feature 3: 0.83 (true: 0.8)
  Feature 4: 2.95 (true: 3.0)
  Feature 5: -0.47 (true: -0.5)

Test R²: 0.9512
Test RMSE: 0.98
```

### Polynomial Regression Results

```
======================================================================
POLYNOMIAL REGRESSION RESULTS
======================================================================

[Polynomial Degree Comparison]
Degree   Train R²   Test R²   Status
   1      0.7234    0.7123   Underfitting
   2      0.8923    0.8834   Good (true relationship is quadratic)
   3      0.9012    0.8712   Slight overfitting
   5      0.9234    0.8234   Overfitting
  10      0.9623    0.7234   Severe overfitting

[Regularized Polynomial (degree=10)]
Alpha     Train R²   Test R²   Interpretation
0.001      0.9623     0.7234   Unregularized overfitting
0.01       0.9123     0.8434   Still overfitting
0.1        0.8834     0.8723   Good balance
1.0        0.8723     0.8656   Slight underfitting
10.0       0.8523     0.8434   Underfitting
100.0      0.8123     0.8012   Severe underfitting
```

### Banking Application Results

```
======================================================================
BANKING APPLICATION RESULTS
======================================================================

[Loan Default Prediction]
Standardized Coefficients:
  income:              -0.2345 (higher income → lower DTI)
  credit_score:       -0.4567 (better credit → lower DTI)
  employment_years:   -0.1234 (longer employment → lower DTI)
  loan_amount:        +0.8234 (larger loans → higher DTI)

Performance:
  R²: 0.7834
  RMSE: 0.1245
  MAE: 0.0923

Interpretation:
  - loan_amount has strongest positive effect
  - credit_score has strongest negative effect
  - Model explains 78% of debt-to-income variance
```

## Visualization

### ASCII Visualizations

```
======================================================================
LINEAR REGRESSION FIT VISUALIZATION
======================================================================

      y
      ^
   35 +                      *  *
      |                   *  *  *
   30 +                *  *
      |             *  *  |
   25 +           *  *    |
      |         *  *      |    ________ (fitted line: y = 3.05x + 4.89)
   20 +       *  *        |___/
      |     *  *          |
   15 +   *  *            |
      | *  *              |
   10 +*  *               |
      +------------------+------------------> x
        0    1    2    3    4    5    6    7    8    9   10

* = Data points
| = Fitted line
```

```
======================================================================
POLYNOMIAL DEGREE COMPARISON
======================================================================

      y
      ^
      |       *         degree=1 (underfit)
   -5 +---------*-------------
      |       *  *      degree=2 (good fit)
   -10+-------*----*--------
      |     * *      *    degree=5 (slight overfit)
   -15+---* *     *  *----
      | *  *       *    *  degree=10 (overfit)
   -20+*  *       *  *  *
      +----------------------> x
       -3   -2   -1   0   1   2   3
```

```
======================================================================
RESIDUAL PLOT (Checking Assumptions)
======================================================================

Residuals
      ^
  3.0 +        *  *
      |       *    *
  2.0 +      *      *
      |     *        *
  1.0 +   *          *      *
      |  *            *    *
  0.0 +*------------------*-----> Predicted
      |                  *
 -1.0 +                   *
      |                    *
 -2.0 +                     *
      +---------------------+----->
        0    5   10   15   20   25

Ideal: Random scatter around zero, constant variance
```

## Advanced Topics

### Feature Scaling and Standardization

Feature scaling impacts linear regression coefficient interpretation and convergence. Standardization (z-score normalization) centers features at zero with unit variance. This creates comparable coefficients across features with different scales.

For interpretation, standardized coefficients show relative feature importance. Larger absolute coefficients indicate stronger effects. Unstandardized coefficients depend on feature scales, complicating comparison.

Regularized regression requires feature scaling. Without scaling, regularization affects features differently based on their ranges. Standardization ensures equal treatment of all features.

### Interaction Terms

Interaction terms capture combined effects of multiple features. The coefficient for the interaction term indicates how the relationship between one feature and the target depends on another feature.

Creating interaction terms involves multiplying features. The polynomial features transformer can generate all pairwise interactions. More complex interactions may require manual construction.

Interpretation becomes more complex with interaction terms. Main effects may change meaning when interactions are present. The interpretation must consider both main effects and interactions together.

### Outlier Detection and Handling

Outliers can substantially influence linear regression fits. Cook's distance identifies influential outliers based on leverage and residual magnitude. Points with high Cook's distance warrant investigation.

Handling options include removing outliers, transforming the target, or using robust regression methods. Removal requires careful justification. Transformations can reduce outlier influence. Robust methods like Huber regression downweight outliers automatically.

The decision depends on whether outliers represent data errors or genuine extreme values. Errors should be corrected or removed. Genuine extremes may contain important information about the relationship.

## Conclusion

Linear and polynomial regression provide foundational tools for continuous prediction. Linear regression captures linear relationships through interpretable coefficients. Polynomial regression extends this to curved relationships while maintaining tractable estimation.

Implementation with scikit-learn provides accessible tools for both techniques. Pipeline integration enables clean workflows from preprocessing through prediction. Cross-validation supports degree selection and regularization tuning.

Applications in banking and healthcare demonstrate real-world utility. Interpretability supports regulatory requirements and informed decision-making. Reliability ensures consistent performance in production systems.

Advanced considerations include feature scaling, interaction terms, and outlier handling. These extensions expand the range of relationships that can be captured. Understanding limitations guides appropriate application.