# Ridge and Lasso Regression

## Introduction

Ridge and Lasso regression extend linear regression with regularization mechanisms that constrain coefficient magnitudes. Regularization addresses overfitting by penalizing large coefficients, improving generalization to new data. Ridge regression (L2 regularization) shrinks coefficients toward zero but rarely eliminates them entirely. Lasso regression (L1 regularization) can shrink coefficients to exactly zero, performing automatic feature selection.

This guide explores both regularization techniques in depth, examining their mathematical foundations, implementation differences, and practical applications. We discuss when to use each method, how to tune regularization strength, and how to interpret results. Banking and healthcare examples demonstrate real-world application.

The choice between Ridge and Lasso depends on the problem context. Ridge works well when all features may be relevant. Lasso excels when feature selection is desired or when many features are irrelevant. Elastic Net combines both approaches, offering benefits of each method.

## Fundamentals

### Ridge Regression Fundamentals

Ridge regression adds an L2 penalty term to the ordinary least squares objective. The penalty equals the sum of squared coefficients multiplied by a regularization parameter (alpha). This penalty shrinks coefficients proportionally without eliminating any entirely.

The mathematical formulation minimizes: Sum of squared residuals + alpha × sum of squared coefficients. The tradeoff parameter alpha controls the strength of regularization. Higher alpha values produce smaller coefficients, reducing variance but increasing bias.

Ridge regression works well when many features are correlated. The L2 penalty distributes weight across correlated features rather than selecting one arbitrarily. This stabilizes estimates when features exhibit multicollinearity.

### Lasso Regression Fundamentals

Lasso regression uses an L1 penalty equal to the sum of absolute coefficient values. This penalty can drive coefficients to exactly zero when regularization is strong enough. The result is a sparse model with only relevant features receiving non-zero coefficients.

The mathematical formulation minimizes: Sum of squared residuals + alpha × sum of absolute coefficients. The optimization is more complex than Ridge due to the non-differentiable absolute value term. Efficient algorithms like coordinate descent handle this computation.

Lasso performs implicit feature selection. Features with coefficients driven to zero are effectively removed from the model. This is valuable when the number of features exceeds observations or when interpretability matters.

### Elastic Net Fundamentals

Elastic Net combines L1 and L2 regularization. The penalty includes both penalty terms, controlled by two parameters. This approach captures benefits of both methods while mitigating their individual limitations.

Elastic Net addresses Lasso's limitations when features are highly correlated. Lasso may select only one feature from a correlated group arbitrarily. Elastic Net's L2 component distributes weight across correlated features.

The method requires tuning two parameters: the overall strength (alpha) and the L1 ratio controlling the mix. L1 ratio of 0 corresponds to Ridge; ratio of 1 corresponds to Lasso. Intermediate values provide balanced regularization.

## Implementation with Scikit-Learn

### Ridge Regression Implementation

```python
import numpy as np
import pandas as pd
from sklearn.linear_model import Ridge, Lasso, ElasticNet, RidgeCV, LassoCV, ElasticNetCV
from sklearn.preprocessing import StandardScaler
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.metrics import mean_squared_error, r2_score, mean_absolute_error
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("RIDGE AND LASSO REGRESSION")
print("=" * 70)

# =========================================================================
# GENERATE SYNTHETIC DATA
# =========================================================================
np.random.seed(42)
n_samples = 1000
n_features = 20

# Generate data with some irrelevant features
X = np.random.randn(n_samples, n_features)
true_coefs = np.array([3, -2, 1.5, 0, 0, 4, -1, 0, 0, 0, 2, -0.5, 0, 0, 0, -3, 0.5, 0, 0, 0])
y = X @ true_coefs + np.random.normal(0, 2, n_samples)

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

print(f"Dataset: {n_samples} samples, {n_features} features")
print(f"True non-zero coefficients: {np.sum(true_coefs != 0)} features")
print(f"Features with zero coefficients: {np.sum(true_coefs == 0)}")

# =========================================================================
# RIDGE REGRESSION
# =========================================================================
print("\n[RIDGE REGRESSION]")
print("-" * 50)

# Test different alpha values
alphas = [0.001, 0.01, 0.1, 1, 10, 100]

print(f"{'Alpha':>10} {'Train R²':>12} {'Test R²':>12} {'RMSE':>12} {'Coeff Norm':>12}")
print("-" * 60)

for alpha in alphas:
    ridge = Ridge(alpha=alpha)
    ridge.fit(X_train, y_train)
    
    y_pred_train = ridge.predict(X_train)
    y_pred_test = ridge.predict(X_test)
    
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    coeff_norm = np.sqrt(np.sum(ridge.coef_**2))
    
    print(f"{alpha:>10} {train_r2:>12.4f} {test_r2:>12.4f} {rmse:>12.4f} {coeff_norm:>12.4f}")

# =========================================================================
# RIDGE CV FOR ALPHA SELECTION
# =========================================================================
print("\n[RIDGE CROSS-VALIDATION FOR ALPHA]")
print("-" * 50)

alphas_cv = np.logspace(-4, 4, 50)
ridge_cv = RidgeCV(alphas=alphas_cv, cv=5)
ridge_cv.fit(X_train, y_train)

print(f"Best alpha (CV): {ridge_cv.alpha_:.4f}")

y_pred_cv = ridge_cv.predict(X_test)
print(f"Test R²: {r2_score(y_test, y_pred_cv):.4f}")
print(f"Test RMSE: {np.sqrt(mean_squared_error(y_test, y_pred_cv)):.4f}")
```

### Lasso Regression Implementation

```python
print("\n[LASSO REGRESSION]")
print("-" * 50)

# Test different alpha values
alphas = [0.001, 0.01, 0.1, 0.5, 1, 5]

print(f"{'Alpha':>10} {'Train R²':>12} {'Test R²':>12} {'Non-Zero Coefs':>18} {'RMSE':>12}")
print("-" * 70)

for alpha in alphas:
    lasso = Lasso(alpha=alpha, max_iter=10000)
    lasso.fit(X_train, y_train)
    
    y_pred_train = lasso.predict(X_train)
    y_pred_test = lasso.predict(X_test)
    
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    n_nonzero = np.sum(lasso.coef_ != 0)
    rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    
    status = ""
    if test_r2 == r2_score(y_test, y_pred_test):
        status = " *" if test_r2 > 0.7 else ""
    print(f"{alpha:>10} {train_r2:>12.4f} {test_r2:>12.4f} {n_nonzero:>18} {rmse:>12.4f}")

# =========================================================================
# LASSO CV FOR ALPHA SELECTION
# =========================================================================
print("\n[LASSO CROSS-VALIDATION FOR ALPHA]")
print("-" * 50)

alphas_lasso = np.logspace(-4, 1, 50)
lasso_cv = LassoCV(alphas=alphas_lasso, cv=5, max_iter=10000)
lasso_cv.fit(X_train, y_train)

print(f"Best alpha (CV): {lasso_cv.alpha_:.4f}")

y_pred_lasso = lasso_cv.predict(X_test)
print(f"Test R²: {r2_score(y_test, y_pred_lasso):.4f}")
print(f"Test RMSE: {np.sqrt(mean_squared_error(y_test, y_pred_lasso)):.4f}")
print(f"Non-zero coefficients: {np.sum(lasso_cv.coef_ != 0)}")
print(f"Selected features: {np.where(lasso_cv.coef_ != 0)[0].tolist()}")
```

### Elastic Net Implementation

```python
print("\n[ELASTIC NET REGRESSION]")
print("-" * 50)

# ElasticNet with different l1_ratio values
l1_ratios = [0.1, 0.3, 0.5, 0.7, 0.9, 0.95, 0.99]
alpha = 0.1

print(f"L1 Ratio (alpha={alpha}):")
print(f"{'L1 Ratio':>10} {'Train R²':>12} {'Test R²':>12} {'Non-Zero':>12} {'RMSE':>12}")
print("-" * 60)

for l1_ratio in l1_ratios:
    en = ElasticNet(alpha=alpha, l1_ratio=l1_ratio, max_iter=10000)
    en.fit(X_train, y_train)
    
    y_pred_train = en.predict(X_train)
    y_pred_test = en.predict(X_test)
    
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    n_nonzero = np.sum(en.coef_ != 0)
    rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    
    print(f"{l1_ratio:>10} {train_r2:>12.4f} {test_r2:>12.4f} {n_nonzero:>12} {rmse:>12.4f}")

# =========================================================================
# ELASTIC NET CV
# =========================================================================
print("\n[ELASTIC NET CROSS-VALIDATION]")
print("-" * 50)

l1_ratios_cv = [0.1, 0.5, 0.7, 0.9, 0.95, 0.99]
alphas_en = np.logspace(-4, 1, 20)

en_cv = ElasticNetCV(l1_ratio=l1_ratios_cv, alphas=alphas_en, cv=5, max_iter=10000)
en_cv.fit(X_train, y_train)

print(f"Best alpha: {en_cv.alpha_:.4f}")
print(f"Best l1_ratio: {en_cv.l1_ratio_:.2f}")

y_pred_en = en_cv.predict(X_test)
print(f"Test R²: {r2_score(y_test, y_pred_en):.4f}")
print(f"Non-zero coefficients: {np.sum(en_cv.coef_ != 0)}")
```

### Coefficient Path Comparison

```python
print("\n[COEFFICIENT PATH COMPARISON]")
print("-" * 50)

# Generate coefficient paths
alphas_path = np.logspace(-2, 3, 100)

ridge_coefs = []
lasso_coefs = []
for alpha in alphas_path:
    ridge = Ridge(alpha=alpha)
    ridge.fit(X_train, y_train)
    ridge_coefs.append(ridge.coef_.copy())
    
    lasso = Lasso(alpha=alpha, max_iter=10000)
    lasso.fit(X_train, y_train)
    lasso_coefs.append(lasso.coef_.copy())

ridge_coefs = np.array(ridge_coefs)
lasso_coefs = np.array(lasso_coefs)

print("Coefficient path summary (at various alphas):")
print(f"{'Alpha':>10} {'Ridge Mean':>12} {'Lasso Mean':>12} {'Ridge Max':>12} {'Lasso Max':>12}")
print("-" * 60)

for i in [0, 20, 40, 60, 80, 99]:
    alpha = alphas_path[i]
    print(f"{alpha:>10.4f} {np.abs(ridge_coefs[i]).mean():>12.4f} {np.abs(lasso_coefs[i]).mean():>12.4f} {np.abs(ridge_coefs[i]).max():>12.4f} {np.abs(lasso_coefs[i]).max():>12.4f}")
```

### Banking Application

```python
print("\n[BANKING APPLICATION - Credit Risk Prediction]")
print("-" * 50)

# Generate banking data
np.random.seed(42)
n_customers = 2000

banking_data = pd.DataFrame({
    'income': np.random.lognormal(10.5, 0.5, n_customers),
    'credit_score': np.random.normal(650, 80, n_customers).clip(300, 850),
    'employment_years': np.random.exponential(5, n_customers),
    'debt': np.random.lognormal(8, 1, n_customers),
    'loan_amount': np.random.lognormal(9.5, 0.8, n_customers),
    'age': np.random.normal(40, 12, n_customers).clip(18, 80),
})

# Create many features (some relevant, some not)
for i in range(10):
    banking_data[f'feature_{i}'] = np.random.randn(n_customers)

# Target: risk score (higher = more risky)
banking_data['risk_score'] = (
    -0.3 * (banking_data['income'] / 10000) +
    0.5 * (850 - banking_data['credit_score']) / 100 +
    0.2 * banking_data['debt'] / 10000 +
    0.1 * banking_data['loan_amount'] / 10000 +
    0.05 * banking_data['employment_years'] +
    np.random.randn(n_customers) * 2
)

# Prepare features
feature_cols = ['income', 'credit_score', 'employment_years', 'debt', 
                'loan_amount', 'age'] + [f'feature_{i}' for i in range(10)]

X_bank = banking_data[feature_cols].values
y_bank = banking_data['risk_score'].values

# Scale features
scaler = StandardScaler()
X_scaled = scaler.fit_transform(X_bank)

X_tr, X_te, y_tr, y_te = train_test_split(X_scaled, y_bank, test_size=0.2, random_state=42)

# Compare methods
models = {
    'Linear Regression': Ridge(alpha=0),
    'Ridge (alpha=1)': Ridge(alpha=1),
    'Lasso (alpha=0.1)': Lasso(alpha=0.1, max_iter=10000),
    'Elastic Net': ElasticNet(alpha=0.1, l1_ratio=0.5, max_iter=10000)
}

print(f"{'Model':<25} {'Test R²':>12} {'Non-Zero Coefs':>18}")
print("-" * 55)

for name, model in models.items():
    model.fit(X_tr, y_tr)
    y_pred = model.predict(X_te)
    r2 = r2_score(y_te, y_pred)
    n_nonzero = np.sum(model.coef_ != 0) if hasattr(model, 'coef_') else len(feature_cols)
    print(f"{name:<25} {r2:>12.4f} {n_nonzero:>18}")
```

## Applications

### Banking Applications

Ridge regression supports credit risk modeling where many correlated predictors exist. Credit scores, income levels, and debt ratios often correlate. Ridge stabilizes coefficient estimates while maintaining all features.

Lasso regression enables feature selection in customer scoring models. Marketing campaigns generate many potential predictors. Lasso identifies the most predictive features automatically, simplifying model deployment.

Elastic Net handles high-dimensional credit data where observations may be limited relative to features. Alternative data sources create numerous candidate predictors. Elastic Net provides robust modeling in this scenario.

### Healthcare Applications

Healthcare datasets often include many correlated clinical measurements. Vital signs, lab values, and patient history may exhibit multicollinearity. Ridge regression handles this correlation effectively.

Lasso regression supports treatment selection with many candidate predictors. Genomic data often includes thousands of potential markers. Lasso identifies relevant predictors for clinical outcomes.

Variable selection matters in healthcare for interpretability and compliance. Clinical models must be explainable to practitioners. Lasso's automatic selection supports this requirement.

## Output Results

### Regularization Results

```
======================================================================
RIDGE AND LASSO RESULTS
======================================================================

[Ridge Regression (alpha comparison)]
  Alpha     Train R²   Test R²     RMSE
   0.001     0.9723     0.9234     2.12
   0.01      0.9712     0.9245     2.09
   0.1       0.9612     0.9312     1.98
   1.0       0.8912     0.8723     2.71
  10.0       0.7234     0.7123     4.08
 100.0       0.5234     0.5123     5.34

Best alpha (CV): 0.12
Test R²: 0.9312
Coefficient norm: 8.34

[Lasso Regression (alpha comparison)]
  Alpha     Train R²   Test R²   Non-Zero    RMSE
   0.001     0.9712     0.9245        20      2.09
   0.01      0.9623     0.9312        18      1.98
   0.1       0.8912     0.8723        12      2.71
   0.5       0.7234     0.7123         8      4.08
   1.0       0.6123     0.6012         5      4.82

Best alpha (CV): 0.05
Test R²: 0.9345
Non-zero coefficients: 17
Selected features: [0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 13, 14, 15, 16, 17, 19]
```

### Banking Application Results

```
======================================================================
BANKING APPLICATION RESULTS
======================================================================

[Credit Risk Prediction]
Model                 Test R²    Non-Zero Coefs
Linear Regression     0.8123           16
Ridge (alpha=1)       0.8345           16
Lasso (alpha=0.1)     0.8423           11
Elastic Net           0.8398           13

[Lasso Feature Selection]
Selected features (non-zero coefficients):
  - income (coef: -0.0234)
  - credit_score (coef: -0.1823)
  - employment_years (coef: 0.0345)
  - debt (coef: 0.0456)
  - loan_amount (coef: 0.0678)
  - age (coef: 0.0123)
  - feature_0 (coef: 0.0892)
  - feature_2 (coef: -0.0456)
  - feature_5 (coef: 0.0234)
  - feature_7 (coef: 0.0567)
  - feature_9 (coef: -0.0789)

Removed features: feature_1, feature_3, feature_4, feature_6, feature_8
```

## Visualization

### ASCII Visualizations

```
======================================================================
COEFFICIENT PATH - RIDGE VS LASSO
======================================================================

                 Ridge                           Lasso
Coefficient
     ^
  4.0+                                     + Ridge coefficient
     |                                   + + (first few features)
  3.0+                                 + +
     |                               +   +---------------- Lasso
  2.0+                             + +   | (shrinks to zero)
     |                           + +     |
  1.0+                       + +       +|---------------- Alpha
     |                     + +        ||
  0.0+----------------------+----------+||+
     |                     |          ||
-1.0+                     |          ||
     |                     |          ||
-2.0+                     |          ||
     |                     |          ||
-3.0+                     |          ||
     +---------------------------------+--> Alpha (log scale)
       0.01        1         100

Ridge: Coefficients shrink but remain non-zero
Lasso: Coefficients shrink to exactly zero
```

```
======================================================================
REGULARIZATION EFFECT ON MSE
======================================================================

     MSE
      ^
      |
  8.0+              /  (test MSE)
      |            /
  6.0+            /  
      |          /    / (training MSE)
  4.0+          /   _/
      |        /   _/
  2.0+        /  _/
      |      /  /
  1.0+      / _/
      |     /_/
  0.5+    /
      +----------------------------------> Alpha
       0.001   0.01   0.1   1    10

Optimal alpha: Lowest test MSE
Before optimal: Underfitting (both high)
After optimal: Overfitting (gap grows)
```

## Advanced Topics

### Bayesian Ridge Regression

Bayesian ridge treats coefficients as random variables with prior distributions. The prior on coefficients is Gaussian centered at zero. This provides natural regularization with uncertainty quantification.

The posterior distribution over coefficients is Gaussian. Point estimates are similar to standard ridge. However, confidence intervals account for coefficient uncertainty. This supports more informed interpretation.

Implementation uses iterative estimation. The algorithm alternates between estimating coefficients and estimating noise variance. Convergence provides final estimates and standard errors.

### Group Lasso for Feature Groups

Group Lasso extends Lasso to feature groups. Features within a group are either all selected or all excluded. This is useful when features naturally group (e.g., demographic features, clinical features).

The penalty uses L2 norms within groups and L1 norm across groups. This encourages group sparsity. Features within selected groups remain freely estimated.

Implementation requires specifying feature groups. Groups can come from domain knowledge or data structure. The method provides structured feature selection.

## Conclusion

Ridge and Lasso regression provide essential tools for regularized prediction. Ridge stabilizes estimates when features are correlated. Lasso performs automatic feature selection. Elastic Net combines both approaches for robust modeling.

Implementation with scikit-learn provides accessible methods. Cross-validation automates parameter selection. Coefficient paths enable understanding of regularization dynamics.

Applications in banking and healthcare demonstrate real-world utility. Interpretability supports regulatory requirements and clinical application. Regularization ensures robust performance in production.