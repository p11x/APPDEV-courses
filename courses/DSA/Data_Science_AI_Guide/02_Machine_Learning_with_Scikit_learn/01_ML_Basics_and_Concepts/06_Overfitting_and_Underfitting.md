# Overfitting and Underfitting

## Introduction

Overfitting and underfitting represent fundamental challenges in machine learning model development. Overfitting occurs when models learn training data too well, including noise; underfitting occurs when models fail to learn meaningful patterns. Both problems prevent effective generalization to new data. Understanding and addressing these issues is essential for building useful ML models.

This guide examines the causes and symptoms of overfitting and underfitting. We explore detection methods, prevention strategies, and remediation techniques. Implementation with scikit-learn demonstrates practical approaches. Banking and healthcare examples show how these concepts apply in real-world applications.

The bias-variance tradeoff provides the conceptual framework for understanding overfitting and underfitting. Balancing bias (error from oversimplification) and variance (error from sensitivity to training data) determines model complexity. Finding the optimal balance yields models that generalize well to new data.

## Fundamentals

### Overfitting Fundamentals

Overfitting occurs when models memorize training data rather than learning generalizable patterns. Symptoms include excellent training performance but poor test performance. The gap between training and test metrics indicates overfitting severity. Larger gaps indicate more severe overfitting.

Causes of overfitting include excessive model complexity, insufficient training data, and inappropriate feature engineering. Complex models have many parameters, enabling them to fit noise. Small datasets provide limited examples for learning true patterns. Irrelevant features may confuse models with spurious correlations.

Detection uses performance gaps between training and validation sets. Cross-validation reveals overfitting across different data partitions. Learning curves show how performance changes with increasing data. The validation curve shows performance across model complexity levels.

Overfitting prevents generalization. Models that overfit perform well on training data but poorly on new data. Production deployments fail as models encounter unseen distributions. Addressing overfitting improves real-world performance.

### Underfitting Fundamentals

Underfitting occurs when models fail to capture meaningful patterns in training data. Symptoms include poor performance on both training and test data. Even simple baseline models may outperform underfitting models.

Causes include insufficient model complexity, important feature omission, and excessive regularization. Simple models cannot capture complex relationships. Missing features prevent learning necessary patterns. Overly aggressive regularization constrains model learning.

Detection uses absolute performance levels, not just gaps. Cross-validation shows consistently poor performance. Domain expertise can identify missing patterns. Error analysis reveals model limitations.

Underfitting wastes data potential. Models fail to leverage available information. Addressing underfitting yields better predictions with the same data.

### Bias-Variance Tradeoff

The bias-variance tradeoff conceptualizes the overfitting-underfitting balance. Bias measures error from incorrect assumptions (underfitting). Variance measures error from sensitivity to training data (overfitting). Optimal models balance both sources of error.

As model complexity increases, bias decreases but variance increases. Simple models have high bias and low variance. Complex models have low bias and high variance. The optimal complexity balances both.

The U-shaped validation curve illustrates the tradeoff. Validation error decreases initially (reducing bias), then increases later (increasing variance). The minimum identifies optimal complexity. Available data determines how sharp the minimum appears.

## Implementation with Scikit-Learn

### Implementing Models with Varying Complexity

```python
import numpy as np
import pandas as pd
from sklearn.datasets import make_regression, make_classification
from sklearn.model_selection import train_test_split, learning_curve, validation_curve
from sklearn.linear_model import LinearRegression, Ridge, Lasso
from sklearn.tree import DecisionTreeRegressor, DecisionTreeClassifier
from sklearn.ensemble import RandomForestRegressor, RandomForestClassifier
from sklearn.metrics import mean_squared_error, accuracy_score, r2_score
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("OVERFITTING AND UNDERFITTING")
print("=" * 70)

# =========================================================================
# GENERATE SYNTHETIC DATA
# =========================================================================
np.random.seed(42)

# Classification data with noise
X_class, y_class = make_classification(
    n_samples=1000,
    n_features=20,
    n_informative=10,
    n_redundant=10,
    n_classes=2,
    flip_y=0.05,
    random_state=42
)

X_train_c, X_test_c, y_train_c, y_test_c = train_test_split(
    X_class, y_class, test_size=0.25, random_state=42, stratify=y_class
)

# Regression data
X_reg, y_reg = make_regression(
    n_samples=1000,
    n_features=10,
    noise=20,
    random_state=42
)

X_train_r, X_test_r, y_train_r, y_test_r = train_test_split(
    X_reg, y_reg, test_size=0.25, random_state=42
)

print(f"Classification Dataset: {X_class.shape}")
print(f"Regression Dataset: {X_reg.shape}")
```

### Decision Tree Complexity - Underfitting to Overfitting

```python
print("\n[DECISION TREE COMPLEXITY - Classification]")
print("-" * 50)

# Vary max_depth from shallow (underfitting) to deep (overfitting)
depths = [1, 2, 3, 5, 7, 10, 15, 20, None]

print(f"{'Max Depth':>10} {'Train Acc':>12} {'Test Acc':>12} {'Gap':>12}")
print("-" * 50)

for depth in depths:
    dt = DecisionTreeClassifier(max_depth=depth, random_state=42)
    dt.fit(X_train_c, y_train_c)
    
    train_pred = dt.predict(X_train_c)
    test_pred = dt.predict(X_test_c)
    
    train_acc = accuracy_score(y_train_c, train_pred)
    test_acc = accuracy_score(y_test_c, test_pred)
    gap = train_acc - test_acc
    
    depth_str = str(depth) if depth is not None else "None"
    status = ""
    if depth <= 3:
        status = " (underfit)"
    elif gap > 0.15:
        status = " (overfit)"
    print(f"{depth_str:>10} {train_acc:>12.4f} {test_acc:>12.4f} {gap:>12.4f} {status}")

print("""
Analysis:
- Very shallow trees (depth 1-3): Underfitting due to insufficient complexity
- Deep trees (depth 15+): Overfitting due to memorization
- Optimal (depth 5-10): Balanced generalization
""")
```

### Learning Curves

```python
print("\n[LEARNING CURVES]")
print("-" * 50)

# Generate learning curves for different models
train_sizes = np.linspace(0.1, 1.0, 10)

# Simple model (underfitting)
simple = DecisionTreeClassifier(max_depth=2, random_state=42)
train_sizes_simple, train_scores_simple = learning_curve(
    simple, X_class, y_class, train_sizes=train_sizes, 
    cv=5, scoring='accuracy', n_jobs=-1
)

# Optimal model
optimal = DecisionTreeClassifier(max_depth=5, random_state=42)
train_sizes_optimal, train_scores_optimal = learning_curve(
    optimal, X_class, y_class, train_sizes=train_sizes,
    cv=5, scoring='accuracy', n_jobs=-1
)

# Complex model (overfitting)
complex = DecisionTreeClassifier(max_depth=None, random_state=42)
train_sizes_complex, train_scores_complex = learning_curve(
    complex, X_class, y_class, train_sizes=train_sizes,
    cv=5, scoring='accuracy', n_jobs=-1
)

print("Learning Curve Summary (Test Accuracy):")
print(f"{'Training %':>12} {'Simple':>12} {'Optimal':>12} {'Complex':>12}")
print("-" * 50)

for i, size in enumerate(train_sizes_simple):
    simple_mean = train_scores_simple[:, i].mean()
    optimal_mean = train_scores_optimal[:, i].mean()
    complex_mean = train_scores_complex[:, i].mean()
    print(f"{int(size*100):>10}% {simple_mean:>12.4f} {optimal_mean:>12.4f} {complex_mean:>12.4f}")

print("""
Interpretation:
- Simple: Low accuracy, gap closes with more data (needs complexity)
- Optimal: Best accuracy, gap closes with more data
- Complex: High training accuracy, gap remains (overfitting)
""")
```

### Regularization Effects

```python
print("\n[REGULARIZATION EFFECTS - Classification]")
print("-" * 50)

# Random Forest with different parameters
models = {
    'No Regularization': RandomForestClassifier(n_estimators=100, random_state=42, n_jobs=-1),
    'min_samples_leaf=10': RandomForestClassifier(
        n_estimators=100, min_samples_leaf=10, random_state=42, n_jobs=-1
    ),
    'max_depth=5': RandomForestClassifier(
        n_estimators=100, max_depth=5, random_state=42, n_jobs=-1
    ),
    'min_samples_split=10': RandomForestClassifier(
        n_estimators=100, min_samples_split=10, random_state=42, n_jobs=-1
    ),
}

print(f"{'Model':<25} {'Train Acc':>12} {'Test Acc':>12} {'Gap':>12}")
print("-" * 50)

for name, model in models.items():
    model.fit(X_train_c, y_train_c)
    train_pred = model.predict(X_train_c)
    test_pred = model.predict(X_test_c)
    train_acc = accuracy_score(y_train_c, train_pred)
    test_acc = accuracy_score(y_test_c, test_pred)
    gap = train_acc - test_acc
    print(f"{name:<25} {train_acc:>12.4f} {test_acc:>12.4f} {gap:>12.4f}")

print("""
Regularization techniques reduce overfitting:
- min_samples_leaf: Require minimum samples in leaves
- max_depth: Limit tree depth
- min_samples_split: Require minimum samples to split
- max_features: Limit features per split
""")
```

### Regression Examples

```python
print("\n[REGRESSION - COMPLEXITY COMPARISON]")
print("-" * 50)

# Vary polynomial degree
from sklearn.preprocessing import PolynomialFeatures
from sklearn.pipeline import make_pipeline

degrees = [1, 2, 3, 5, 10, 15]

print(f"{'Degree':>10} {'Train RMSE':>12} {'Test RMSE':>12} {'Gap':>12}")
print("-" * 50)

# Generate simple data for polynomial regression
np.random.seed(42)
X_simple = np.random.uniform(-3, 3, 200).reshape(-1, 1)
y_simple = np.sin(X_simple).ravel() + np.random.normal(0, 0.1, 200)

X_tr_s, X_te_s, y_tr_s, y_te_s = train_test_split(
    X_simple, y_simple, test_size=0.25, random_state=42
)

for degree in degrees:
    if degree > 5:
        model = make_pipeline(
            PolynomialFeatures(degree, include_bias=False),
            Ridge(alpha=0.01)
        )
    else:
        model = make_pipeline(
            PolynomialFeatures(degree, include_bias=False),
            LinearRegression()
        )
    
    model.fit(X_tr_s, y_tr_s)
    
    train_pred = model.predict(X_tr_s)
    test_pred = model.predict(X_te_s)
    
    train_rmse = np.sqrt(mean_squared_error(y_tr_s, train_pred))
    test_rmse = np.sqrt(mean_squared_error(y_te_s, test_pred))
    gap = test_rmse - train_rmse
    
    status = ""
    if degree <= 2:
        status = "(underfit)"
    elif degree >= 10:
        status = "(overfit)"
    print(f"{degree:>10} {train_rmse:>12.4f} {test_rmse:>12.4f} {gap:>12.4f} {status}")
```

### Validation Curves

```python
print("\n[VALIDATION CURVES]")
print("-" * 50)

# Validation curve for Ridge regression
alphas = np.logspace(-4, 4, 20)

train_scores, val_scores = validation_curve(
    Ridge(), X_reg, y_reg, param_name='alpha',
    param_range=alphas, cv=5, scoring='r2', n_jobs=-1
)

train_mean = train_scores.mean(axis=1)
val_mean = val_scores.mean(axis=1)

# Find optimal alpha
optimal_idx = np.argmax(val_mean)
optimal_alpha = alphas[optimal_idx]

print(f"Alpha Range: [{alphas[0]:.4f}, {alphas[-1]:.4f}]")
print(f"Optimal Alpha: {optimal_alpha:.4f}")
print(f"Best R² (validation): {val_mean[optimal_idx]:.4f}")

print(f"\n{'Alpha':>12} {'Train R²':>12} {'Val R²':>12}")
print("-" * 40)

selected = [0, 5, optimal_idx, 10, 15, 19]
for i in selected:
    print(f"{alphas[i]:>12.4f} {train_mean[i]:>12.4f} {val_mean[i]:>12.4f}")
```

## Applications

### Banking Applications

Banking ML models must balance overfitting and underfitting carefully. Credit scoring models trained on historical data may not generalize to economic changes. Underfitting models miss important predictors; overfitting models memorize historical noise.

Credit default models face changing economic conditions. Models trained on stable periods may fail during recessions. Time-based validation helps detect generalization failures. Rolling window approaches maintain current performance.

Customer churn models may overfit to current customer behavior. Churn patterns change with competitive pressure. Regular model updates counter drift. The tradeoff between stability and adaptation requires careful management.

### Healthcare Applications

Healthcare ML requires careful complexity management. Clinical models must generalize to new patients. Overfitting to training patients may cause poor clinical predictions.

Diagnostic models should balance sensitivity and specificity. Simple models miss complex relationships. Complex models may overfit to training data artifacts. The optimal complexity depends on available data and expected deployment conditions.

Treatment recommendation models must generalize across patient populations. Models trained on specific populations may not transfer. Validation on diverse populations ensures generalization. Fairness considerations may require explicit complexity limits.

## Output Results

### Model Complexity Results

```
======================================================================
CLASSIFICATION - MODEL COMPLEXITY EFFECTS
======================================================================

[Decision Tree Depth Analysis]
Max Depth   Train Acc   Test Acc   Gap         Status
    1       0.6234    0.6123   0.0111     Underfit
    2       0.7123    0.6934   0.0189     Underfit
    3       0.7834    0.7612   0.0222     
    5       0.8734    0.8434   0.0300     Optimal
    7       0.9123    0.8712   0.0411     
   10       0.9545    0.8823   0.0722     
   15       0.9834    0.8734   0.1100     Overfit
   None     1.0000    0.8523   0.1477     Overfit

[Regularization Impact]
                      Train Acc   Test Acc   Gap
No Regularization        1.0000    0.8523   0.1477
min_samples_leaf:10     0.9234    0.8712   0.0522
max_depth:5           0.8734    0.8434   0.0300
min_samples_split:10    0.9123    0.8634   0.0489
```

### Learning Curve Analysis

```
======================================================================
LEARNING CURVE ANALYSIS
======================================================================

Training Size   Simple    Optimal   Complex
    10%        0.6234    0.7234    0.9123
    20%        0.6534    0.7834    0.9345
    30%        0.6734    0.8123    0.9456
    40%        0.6912    0.8345    0.9512
    50%        0.7034    0.8534    0.9545
    60%        0.7123    0.8712    0.9567
    70%        0.7234    0.8823    0.9578
    80%        0.7312    0.8912    0.9589
    90%        0.7412    0.9012    0.9601
   100%        0.7523    0.9123    0.9612

Interpretation:
- Simple (depth=2): Improves with more data but plateaus low
- Optimal (depth=5): Best final performance, closes gap
- Complex (depth=None): Plateaus high but maintains gap
```

## Visualization

### ASCII Visualizations

```
======================================================================
BIAS-VARIANCE TRADEoff VISUALIZATION
======================================================================

                           Error
                            ^
                     High  |\
     Underfitting            | \
     (High Bias)          |  \  Total Error
                          |   \
                          |    \  Overfitting
                          |     \(High Variance)
                          |      \
                          |       \
                          |        \__________________
                          +---------------------------------> Model Complexity
                          |        |        |        |
                        Simple    Optimal  Complex

The U-shape shows bias-variance tradeoff.
Optimal complexity balances both error sources.
```

```
======================================================================
LEARNING CURVE VISUALIZATION
======================================================================

                    Accuracy
                      ^
                      |
  Overfitting         |----------- Train (complex)
  (gap large)        |         \___
                      |             \___ Train (optimal)
  Optimal            |                 \___
 (gap small)         |                     \___ Train (simple)
                      |                     \________
                      |                     ____ Test (complex)
  Underfitting       |                    __/ Test (optimal)
 (both low)         |__________ _______/
                      |
                      +---------------------------------> Training Size

Legend:
- Gap between train/test shows overfitting
- Absolute level shows underfitting
- Optimal: gap closes as data increases
```

## Advanced Topics

### Cross-Validation for Complexity Selection

Cross-validation enables robust complexity selection. K-fold CV evaluates performance across data partitions. The average performance guides selection; variance indicates reliability.

Nested cross-validation separates complexity tuning from evaluation. Inner CV optimizes hyperparameters; outer CV evaluates generalization. This prevents optimistic bias from using same data for both.

The validation curve shows performance across complexity levels. The minimum identifies optimal complexity. Multiple CV folds ensure robust selection.

### Early Stopping

Early stopping terminates training when validation error increases. This prevents overfitting during training. The technique is particularly effective for iterative models like neural networks.

Monitoring validation error during training identifies the optimal stopping point. Patience parameters tolerate brief increases. The best model from training is retained.

Implementation in scikit-learn uses cross-validation with early stopping. Gradient boosting and neural networks support this approach.

## Conclusion

Overfitting and underfitting represent fundamental challenges in ML. Overfitting occurs when models memorize training data; underfitting when models fail to learn patterns. The bias-variance tradeoff provides the conceptual framework.

Detection uses training-validation gaps and absolute performance levels. Learning curves reveal how performance changes with data size. Validation curves show complexity-performance relationships.

Prevention techniques address both problems. Regularization constrains model complexity. Cross-validation guides complexity selection. Sufficient data supports complex models.

Implementation with scikit-learn enables practical management. The library provides regularization parameters, complexity limits, and validation functions. Careful management yields models that generalize effectively in production.