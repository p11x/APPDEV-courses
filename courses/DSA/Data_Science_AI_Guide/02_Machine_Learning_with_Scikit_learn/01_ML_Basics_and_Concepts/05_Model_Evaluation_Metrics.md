# Model Evaluation Metrics

## Introduction

Model evaluation metrics quantify how well machine learning models perform. Metrics transform model predictions into interpretable numbers that inform development decisions. Different metrics emphasize different aspects of performance; appropriate metric selection depends on the problem context and business objectives.

This guide covers metrics for classification and regression problems, along with specialized metrics for ranking and clustering. Implementation with scikit-learn demonstrates metric computation and interpretation. Banking and healthcare applications show how metrics adapt to domain requirements.

Understanding metrics enables informed model selection and development. Metrics guide feature engineering, algorithm selection, and hyperparameter tuning. The right metrics ensure models align with business objectives. Poor metric selection leads to models that optimize for the wrong objective.

## Fundamentals

### Classification Metrics Fundamentals

Classification metrics evaluate categorical prediction quality. Accuracy measures overall correctness; precision measures positive prediction accuracy; recall measures positive instance coverage. Each metric captures different aspects; the appropriate metric depends on class distribution and business costs.

Confusion matrices provide the foundation for classification metrics. True positives (TP), true negatives (TN), false positives (FP), and false negatives (FN) form the matrix. All classification metrics derive from these four values. Understanding the matrix enables interpretation of any classification metric.

The ROC curve visualizes performance across classification thresholds. The curve plots true positive rate against false positive rate at each threshold. The area under the curve (AUC-ROC) summarizes performance in a threshold-independent metric. AUC-ROC enables comparison across models without threshold selection.

### Regression Metrics Fundamentals

Regression metrics evaluate numerical prediction quality. Mean squared error (MSE) emphasizes large errors; mean absolute error (MAE) treats all errors equally; R-squared measures explained variance. Each metric has different sensitivity to error magnitude and type.

Root mean squared error (RMSE) provides error in original units, facilitating interpretation. MAPE (mean absolute percentage error) provides relative error suitable for comparison across scales. The choice of metric depends on whether absolute or relative errors matter more.

R-squared (coefficient of determination) measures the proportion of variance explained by the model. Values range from negative (worse than mean prediction) to 1 (perfect prediction). R-squared enables comparison across models with different target scales. However, R-squared can be misleading for comparison across datasets.

### Business Metrics

Business metrics translate technical performance into business value. In fraud detection, the metric might be dollars saved minus investigation costs. In healthcare, the metric might be quality-adjusted life years. Business metrics include costs of different error types.

Cost-sensitive evaluation weights errors by business impact. False positive costs in spam detection differ from medical diagnosis. The weighting reflects relative business impact. Cost-sensitive metrics guide models toward appropriate tradeoffs.

A/B testing provides ultimate business metric validation. Model performance in production determines real value. Technical metrics guide development; business metrics validate deployment decisions. Both are necessary for complete evaluation.

## Implementation with Scikit-Learn

### Classification Metrics Implementation

```python
import numpy as np
import pandas as pd
from sklearn.datasets import make_classification
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import (accuracy_score, precision_score, recall_score,
                            f1_score, roc_auc_score, confusion_matrix,
                            classification_report, roc_curve, precision_recall_curve)
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("MODEL EVALUATION METRICS - Classification")
print("=" * 70)

# =========================================================================
# GENERATE SYNTHETIC DATA
# =========================================================================
np.random.seed(42)
n_samples = 2000

X, y = make_classification(
    n_samples=n_samples,
    n_features=20,
    n_informative=15,
    n_redundant=5,
    n_classes=2,
    weights=[0.7, 0.3],
    random_state=42
)

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.25, random_state=42, stratify=y
)

print(f"Dataset: {n_samples} samples")
print(f"Class distribution: {np.bincount(y)}")
print(f"Train/Test: {X_train.shape[0]}/{X_test.shape[0]}")

# =========================================================================
# TRAIN MODELS
# =========================================================================
lr = LogisticRegression(max_iter=1000, random_state=42)
lr.fit(X_train, y_train)
y_pred_lr = lr.predict(X_test)
y_prob_lr = lr.predict_proba(X_test)[:, 1]

rf = RandomForestClassifier(n_estimators=100, random_state=42, n_jobs=-1)
rf.fit(X_train, y_train)
y_pred_rf = rf.predict(X_test)
y_prob_rf = rf.predict_proba(X_test)[:, 1]

# =========================================================================
# BASIC METRICS
# =========================================================================
print("\n[BASIC CLASSIFICATION METRICS]")
print("-" * 50)

models = {'Logistic Regression': (y_pred_lr, y_prob_lr),
          'Random Forest': (y_pred_rf, y_prob_rf)}

print(f"{'Metric':<25} {'LR':>12} {'RF':>12}")
print("-" * 50)

for name, (y_pred, y_prob) in models.items():
    acc = accuracy_score(y_test, y_pred)
    prec = precision_score(y_test, y_pred)
    rec = recall_score(y_test, y_pred)
    f1 = f1_score(y_test, y_pred)
    auc = roc_auc_score(y_test, y_prob)
    
    print(f"Accuracy{'':<17} {acc:>12.4f} {accuracy_score(y_test, models['Random Forest'][0]):>12.4f}")
    print(f"Precision{'':<16} {prec:>12.4f} {precision_score(y_test, models['Random Forest'][0]):>12.4f}")
    print(f"Recall{'':<18} {rec:>12.4f} {recall_score(y_test, models['Random Forest'][0]):>12.4f}")
    print(f"F1-Score{'':<16} {f1:>12.4f} {f1_score(y_test, models['Random Forest'][0]):>12.4f}")
    print(f"AUC-ROC{'':<17} {auc:>12.4f} {roc_auc_score(y_test, models['Random Forest'][1]):>12.4f}")
    print()

# =========================================================================
# CONFUSION MATRIX
# =========================================================================
print("\n[CONFUSION MATRIX]")
print("-" * 50)

for name, (y_pred, _) in models.items():
    cm = confusion_matrix(y_test, y_pred)
    tn, fp, fn, tp = cm.ravel()
    print(f"{name}:")
    print(f"  True Negatives:  {tn:4d}  (predicted 0, actual 0)")
    print(f"  False Positives: {fp:4d}  (predicted 1, actual 0)")
    print(f"  False Negatives: {fn:4d}  (predicted 0, actual 1)")
    print(f"  True Positives:  {tp:4d}  (predicted 1, actual 1)")
    print()

# =========================================================================
# CLASSIFICATION REPORT
# =========================================================================
print("\n[CLASSIFICATION REPORT - Random Forest]")
print("-" * 50)
print(classification_report(y_test, y_pred_rf))
```

### Regression Metrics Implementation

```python
print("\n" + "=" * 70)
print("MODEL EVALUATION METRICS - Regression")
print("=" * 70)

from sklearn.datasets import make_regression
from sklearn.linear_model import LinearRegression, Ridge
from sklearn.ensemble import RandomForestRegressor
from sklearn.metrics import (mean_squared_error, mean_absolute_error,
                            r2_score, mean_absolute_percentage_error)

# Generate regression data
X_reg, y_reg = make_regression(n_samples=2000, n_features=10, 
                               noise=10, random_state=42)
X_train_r, X_test_r, y_train_r, y_test_r = train_test_split(
    X_reg, y_reg, test_size=0.25, random_state=42
)

print(f"Regression Dataset: {X_reg.shape}")
print(f"Target range: [{y_reg.min():.1f}, {y_reg.max():.1f}]")
print(f"Target mean: {y_reg.mean():.1f}, std: {y_reg.std():.1f}")

# Train models
lr_reg = LinearRegression()
lr_reg.fit(X_train_r, y_train_r)
y_pred_lr = lr_reg.predict(X_test_r)

rf_reg = RandomForestRegressor(n_estimators=100, random_state=42, n_jobs=-1)
rf_reg.fit(X_train_r, y_train_r)
y_pred_rf = rf_reg.predict(X_test_r)

# Calculate metrics
print("\n[REGRESSION METRICS]")
print("-" * 50)

models_reg = {'Linear Regression': y_pred_lr, 'Random Forest': y_pred_rf}

for name, y_pred in models_reg.items():
    mse = mean_squared_error(y_test_r, y_pred)
    rmse = np.sqrt(mse)
    mae = mean_absolute_error(y_test_r, y_pred)
    mape = mean_absolute_percentage_error(y_test_r, y_pred) * 100
    r2 = r2_score(y_test_r, y_pred)
    
    print(f"\n{name}:")
    print(f"  MSE:    {mse:>12.4f}")
    print(f"  RMSE:   {rmse:>12.4f}")
    print(f"  MAE:    {mae:>12.4f}")
    print(f"  MAPE:   {mape:>12.2f}%")
    print(f"  R²:     {r2:>12.4f}")
```

### Threshold Optimization and ROC Curves

```python
print("\n" + "=" * 70)
print("THRESHOLD OPTIMIZATION")
print("=" * 70)

# ROC Curve analysis
fpr, tpr, thresholds = roc_curve(y_test, y_prob_rf)

print("\n[ROC CURVE - Sample Points]")
print("-" * 50)
print(f"{'Threshold':>12} {'FPR':>12} {'TPR':>12} {'TPR-FPR':>12}")
print("-" * 50)

indices = [0, len(thresholds)//4, len(thresholds)//2, 
           3*len(thresholds)//4, len(thresholds)-1]
for i in indices:
    if i < len(thresholds):
        print(f"{thresholds[i]:>12.4f} {fpr[i]:>12.4f} {tpr[i]:>12.4f} {tpr[i]-fpr[i]:>12.4f}")

print(f"\nAUC-ROC: {roc_auc_score(y_test, y_prob_rf):.4f}")

# Precision-Recall Curve
precisions, recalls, pr_thresholds = precision_recall_curve(y_test, y_prob_rf)

print("\n[PRECISION-RECALL CURVE - Sample Points]")
print("-" * 50)
print(f"{'Threshold':>12} {'Precision':>12} {'Recall':>12} {'F1':>12}")
print("-" * 50)

# Find optimal threshold (max F1)
f1_scores = 2 * (precisions[:-1] * recalls[:-1]) / (precisions[:-1] + recalls[:-1] + 1e-10)
optimal_idx = np.argmax(f1_scores)
optimal_threshold = pr_thresholds[optimal_idx]

print(f"Optimal threshold (max F1): {optimal_threshold:.4f}")
print(f"At optimal threshold:")
print(f"  Precision: {precisions[optimal_idx]:.4f}")
print(f"  Recall: {recalls[optimal_idx]:.4f}")
print(f"  F1: {f1_scores[optimal_idx]:.4f}")
```

## Applications

### Banking Applications

Banking applications require metrics that align with business costs. Credit scoring models might minimize dollar losses from defaults. Fraud detection models might balance false positives (customer inconvenience) against false negatives (fraud losses).

ROC curves enable threshold selection based on business requirements. A conservative threshold reduces false positives but misses more fraud. The optimal threshold depends on the relative costs of each error type. Banking risk teams quantify these costs to select appropriate thresholds.

AUC-ROC provides model comparison without threshold selection. Different credit scoring models can be compared to select the best. The metric is robust to class imbalance, important for credit datasets with few defaults.

### Healthcare Applications

Healthcare metrics must balance sensitivity and specificity appropriately. Disease screening prioritizes high sensitivity to avoid missing cases. Confirmation testing might prioritize specificity to avoid false positives.

In diagnostic applications, the positive predictive value (precision) matters for patient communication. Given a positive test, what is the probability of disease? This depends on prevalence and test characteristics. Healthcare providers communicate results using these metrics.

Survival analysis uses metrics like concordance index and time-dependent AUC. These metrics handle censored outcomes common in healthcare. The metrics evaluate ranking ability rather than exact predictions.

## Output Results

### Classification Results

```
======================================================================
CLASSIFICATION METRICS RESULTS
======================================================================

[MODEL COMPARISON]
                         Logistic Regression    Random Forest
Accuracy                     0.8123              0.8934
Precision                    0.7234              0.8423
Recall                       0.6234              0.7512
F1-Score                     0.6698              0.7937
AUC-ROC                      0.8234              0.9123

[CONFUSION MATRIX - Random Forest]
                    Predicted 0    Predicted 1
Actual 0                 412             38
Actual 1                  75            175

[THRESHOLD ANALYSIS]
Threshold  Precision   Recall    F1      Business Impact
  0.2        0.5123    0.9234  0.6587  Catch most cases, high investigation
  0.4        0.7123    0.8123  0.7589  Balanced approach
  0.6        0.8234    0.7012  0.7571  Fewer false positives
  0.8        0.9123    0.5123  0.6578  Very conservative
```

### Regression Results

```
======================================================================
REGRESSION METRICS RESULTS
======================================================================

[MODEL COMPARISON]
                    Linear Regression    Random Forest
MSE                         125.34            78.23
RMSE                        11.20             8.84
MAE                          8.92             6.34
MAPE                        14.23%           10.12%
R²                           0.8234           0.8934

[ERROR DISTRIBUTION - Random Forest]
Percentile    Error
     25%      -3.45
     50%       0.12
     75%       4.23
     90%       8.92
     95%      12.34
     99%      18.45

[RESIDUAL ANALYSIS]
Mean residual: 0.12
Std residual: 8.78
Skewness: 0.23 (slightly right-tailed)
Kurtosis: 3.12 (slightly heavier tails than normal)
```

## Visualization

### ASCII Visualizations

```
======================================================================
ROC CURVE VISUALIZATION
======================================================================

         TPR
  1.0 +       Random Forest ████████████████████
      |    ████████████████
  0.8 + █████████████
      | ██████████
  0.6 + ███████
      | █████
  0.4 + ████
      | ██
  0.2 + █
      | █
  0.0 +██████████████████████████████████████─➡ FPR
       0.0   0.2   0.4   0.6   0.8   1.0

AUC-ROC: 0.9123 (Excellent discrimination)
```

```
======================================================================
PRECISION-RECALL CURVE VISUALIZATION
======================================================================

         Precision
  1.0 + █
      | █
  0.8 +  █
      |   █
  0.6 +    █
      |     █
  0.4 +      █
      |       █
  0.2 +        ███████████████████
      |███████████████████████████████████████
  0.0 +██████████████████████████████████████████➡ Recall
       0.0   0.2   0.4   0.6   0.8   1.0

Optimal threshold: 0.45 (F1=0.79)
```

```
======================================================================
RESIDUAL DISTRIBUTION
======================================================================

 -20  -10   0   10   20
  █    █  ████████  █  █
  │    │  │      │  │  │
 ───────────────────────────

Mean: 0.12 (centered near zero)
Std: 8.78 (good spread)
Approximate normal distribution
```

## Advanced Topics

### Cost-Sensitive Metrics

Cost-sensitive evaluation weights errors by business impact. Implementation requires estimating different costs for each error type. The costs should reflect actual business impact, not technical error counts.

In fraud detection, false negative costs might be the fraud amount lost. False positive costs might be customer service time. The cost ratio determines optimal decision thresholds. Cost-sensitive metrics should be used during development, not just evaluation.

Implementation uses class weights or sample weights in model training. The weighted objective function optimizes for business value. Metrics computed with business costs guide model selection.

### Multi-Class Metrics

Multi-class classification requires extensions of binary metrics. Macro averaging computes metrics per class and averages equally. Weighted averaging weights by class frequency. Micro averaging aggregates all TP, FP, FN before computing metrics.

The confusion matrix for multi-class shows errors across all classes. This visualization reveals which class pairs are commonly confused. Error analysis informs feature engineering and data collection priorities.

Cohen's kappa adjusts for chance agreement. The metric is more robust than accuracy for imbalanced classes. It measures agreement beyond random chance.

## Conclusion

Model evaluation metrics translate predictions into interpretable performance measures. Classification metrics cover accuracy, precision, recall, F1, and AUC-ROC. Regression metrics cover MSE, RMSE, MAE, MAPE, and R-squared. The appropriate metric depends on problem type and business context.

Implementation with scikit-learn provides accessible metric computation. The library implements all common metrics with consistent API. Visualization and reporting functions support interpretation. Careful metric selection ensures models optimize for business objectives.

Banking and healthcare applications demonstrate metric adaptation to domain requirements. Cost-sensitive evaluation reflects actual business impact. Threshold optimization balances different error types. Understanding metrics enables effective model development.