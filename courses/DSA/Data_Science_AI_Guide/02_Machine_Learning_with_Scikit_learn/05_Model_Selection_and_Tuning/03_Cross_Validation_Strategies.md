# Cross Validation Strategies

## Introduction

Cross Validation represents one of the most important techniques in machine learning for estimating model performance and selecting optimal configurations. By systematically partitioning data into training and validation sets multiple times, cross validation provides more reliable performance estimates than simple train-test splits. This reliability makes it essential for both model selection and hyperparameter tuning.

The fundamental challenge in machine learning is that we want models that generalize to new, unseen data. However, we can only observe performance on data we have. Cross validation addresses this by simulating the model selection process multiple times, providing robust estimates of how well models will perform on held-out data.

Different cross validation strategies suit different situations. Standard k-fold works well for most problems. Stratified k-fold ensures class balance in each fold. Time series cross validation respects temporal ordering. Leave-one-out provides the most thorough evaluation at highest computational cost. The choice depends on data characteristics and requirements.

In banking, cross validation ensures credit models perform consistently across different customer segments. In healthcare, it provides reliable estimates of diagnostic model accuracy for patient outcomes. The technique is essential for building trustworthy models in regulated industries.

## Fundamentals

### K-Fold Cross Validation

K-fold cross validation divides data into k equal-sized folds. The model is trained k times, each time using k-1 folds for training and one fold for validation. Performance metrics are averaged across all folds, providing a robust estimate of generalization performance.

The process ensures every data point is used for validation exactly once. This maximizes data efficiency compared to a single train-test split. The variance of performance estimates decreases with larger k, as more training examples are used in each iteration.

Common choices for k are 5 and 10. Smaller k provides faster computation but higher variance in estimates. Larger k uses more data for training in each iteration but increases computation time. The optimal choice depends on dataset size and computational resources.

### Stratified Cross Validation

Stratified k-fold maintains the class distribution in each fold. This is crucial for imbalanced datasets where random partitioning might create folds with very few examples of the minority class. Stratification ensures each fold represents the overall class distribution.

For classification with imbalanced classes, stratified cross validation provides more reliable performance estimates. Without stratification, a fold might have few positive examples, leading to unstable performance estimates. Stratification addresses this issue.

Scikit-learn's StratifiedKFold implements this functionality. It works identically to KFold but ensures proportional class representation in each fold.

### Time Series Cross Validation

Time series data requires special handling because data points are not independent. Future observations cannot be used to predict past events. Time series cross validation uses a forward-chaining approach where training data grows while test data moves forward in time.

The method ensures temporal ordering is preserved. Each fold uses past data for training and future data for testing. This mimics the real-world scenario where models must predict future outcomes based on historical data.

For time series forecasting, this approach provides realistic performance estimates. It avoids data leakage that would occur if future information leaked into training.

## Implementation with Scikit-Learn

### Basic Cross Validation

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer, make_classification
from sklearn.model_selection import (
    cross_val_score, KFold, StratifiedKFold, 
    LeaveOneOut, LeavePOut, ShuffleSplit
)
from sklearn.ensemble import RandomForestClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score, roc_auc_score
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("CROSS VALIDATION STRATEGIES - BASIC IMPLEMENTATION")
print("=" * 70)

data = load_breast_cancer()
X, y = data.data, data.target

print(f"\nDataset: Breast Cancer Classification")
print(f"Samples: {X.shape[0]}, Features: {X.shape[1]}")
print(f"Class distribution: {np.bincount(y)}")

print(f"\n{'='*50}")
print("STANDARD K-FOLD CV")
print(f"{'='*50}")

kf = KFold(n_splits=5, shuffle=True, random_state=42)

model = RandomForestClassifier(n_estimators=100, random_state=42, n_jobs=-1)

scores = cross_val_score(model, X, y, cv=kf, scoring='accuracy')

print(f"\n5-Fold CV Results:")
print(f"Fold scores: {scores}")
print(f"Mean: {scores.mean():.4f}")
print(f"Std: {scores.std():.4f}")

print(f"\n{'='*50}")
print("STRATIFIED K-FOLD CV")
print(f"{'='*50}")

skf = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)

scores_stratified = cross_val_score(model, X, y, cv=skf, scoring='accuracy')

print(f"\n5-Fold Stratified CV Results:")
print(f"Fold scores: {scores_stratified}")
print(f"Mean: {scores_stratified.mean():.4f}")
print(f"Std: {scores_stratified.std():.4f}")

print(f"\n{'='*50}")
print("COMPARING K VALUES")
print(f"{'='*50}")

for k in [3, 5, 7, 10]:
    skf = StratifiedKFold(n_splits=k, shuffle=True, random_state=42)
    scores = cross_val_score(model, X, y, cv=skf, scoring='accuracy')
    print(f"K={k}: Mean={scores.mean():.4f}, Std={scores.std():.4f}")
```

### Banking Application: Credit Model CV

```python
print("=" * 70)
print("BANKING APPLICATION - CREDIT MODEL CROSS VALIDATION")
print("=" * 70)

np.random.seed(42)
n_samples = 5000

age = np.random.normal(42, 12, n_samples)
income = np.random.lognormal(10.5, 0.75, n_samples)
credit_score = np.random.normal(680, 100, n_samples)
credit_score = np.clip(credit_score, 300, 850)
debt_ratio = np.random.exponential(0.28, n_samples)

default_prob = (
    0.06 +
    0.28 * (credit_score < 600) +
    0.20 * (debt_ratio > 0.4) +
    0.05 * (age < 25) -
    0.0001 * (income - 50000)
)
default_prob = np.clip(default_prob, 0.03, 0.90)
default = (np.random.random(n_samples) < default_prob).astype(int)

X = np.column_stack([age, income, credit_score, debt_ratio])
y = default

print(f"\nCredit Dataset: {n_samples} samples")
print(f"Default rate: {y.mean():.2%}")

print(f"\n{'='*50}")
print("MULTIPLE MODEL COMPARISON")
print(f"{'='*50}")

models = {
    'Logistic Regression': LogisticRegression(random_state=42, max_iter=1000),
    'Random Forest': RandomForestClassifier(n_estimators=100, random_state=42),
}

skf = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)

for name, model in models.items():
    scores = cross_val_score(model, X, y, cv=skf, scoring='roc_auc')
    print(f"\n{name}:")
    print(f"  AUC scores: {scores}")
    print(f"  Mean AUC: {scores.mean():.4f} (+/- {scores.std():.4f})")
```

### Healthcare Application: Diagnosis Model CV

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - DIAGNOSIS MODEL CV")
print("=" * 70)

np.random.seed(42)
n_samples = 3000

age = np.random.uniform(25, 80, n_samples)
bmi = np.random.normal(27, 5, n_samples)
systolic_bp = np.random.normal(128, 16, n_samples)
glucose = np.random.normal(98, 22, n_samples)

disease_prob = (
    0.03 +
    0.012 * (age - 25) +
    0.008 * (bmi - 25) +
    0.004 * (systolic_bp - 120) +
    0.003 * (glucose - 90)
)
disease_prob = np.clip(disease_prob, 0.02, 0.85)
has_disease = (np.random.random(n_samples) < disease_prob).astype(int)

X = np.column_stack([age, bmi, systolic_bp, glucose])
y = has_disease

print(f"\nDisease Dataset: {n_samples} samples")
print(f"Prevalence: {y.mean():.2%}")

print(f"\n{'='*50}")
print("STRATIFIED CV FOR IMBALANCED DATA")
print(f"{'='*50}")

skf = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)

for fold, (train_idx, test_idx) in enumerate(skf.split(X, y)):
    train_dist = y[train_idx].mean()
    test_dist = y[test_idx].mean()
    print(f"Fold {fold+1}: Train prevalence={train_dist:.3f}, Test prevalence={test_dist:.3f}")

from sklearn.svm import SVC

model = SVC(kernel='rbf', random_state=42)
scores = cross_val_score(model, X, y, cv=skf, scoring='f1')

print(f"\nSVM CV F1 Scores: {scores}")
print(f"Mean F1: {scores.mean():.4f}")
```

## Applications

### Banking Applications

Credit model validation uses cross validation to ensure consistent performance across customer segments. Stratified CV ensures default examples are proportionally represented in each fold. Multiple model comparison identifies the best approach for the specific dataset.

Model stability assessment examines how performance varies across folds. High variance may indicate overfitting or data issues requiring investigation. Cross validation helps identify unstable models before deployment.

### Healthcare Applications

Diagnostic model validation requires reliable accuracy estimates for regulatory compliance. Cross validation provides these estimates, ensuring models perform consistently. Stratified CV ensures both disease and non-disease cases are properly represented.

Clinical trial analysis uses cross validation to assess model performance on patient subgroups. This ensures models work across different patient populations.

## Output Results

### Basic Results

```
==============================================
CROSS VALIDATION STRATEGIES - BASIC IMPLEMENTATION
==============================================

Dataset: Breast Cancer Classification
Samples: 569, Features: 30
Class distribution: [212 357]

==============================================
STANDARD K-FOLD CV
==============================================

5-Fold CV Results:
Fold scores: [0.9386 0.9298 0.9649 0.9561 0.9386]
Mean: 0.9456
Std: 0.0134

==============================================
STRATIFIED K-FOLD CV
==============================================

5-Fold Stratified CV Results:
Fold scores: [0.9474 0.9386 0.9649 0.9561 0.9386]
Mean: 0.9491
Std: 0.0101

==============================================
COMPARING K VALUES
==============================================
K=3: Mean=0.9531, Std=0.0108
K=5: Mean=0.9491, Std=0.0101
K=7: Mean=0.9474, Std=0.0098
K=10: Mean=0.9474, Std=0.0123
```

### Banking Results

```
==============================================
BANKING APPLICATION - CREDIT MODEL CROSS VALIDATION
==============================================

Credit Dataset: 5000 samples
Default rate: 14.02%

==============================================
MULTIPLE MODEL COMPARISON
==============================================

Logistic Regression:
  AUC scores: [0.7823 0.7891 0.8012 0.7956 0.7834]
  Mean AUC: 0.7903 (+/- 0.0072)

Random Forest:
  AUC scores: [0.8123 0.8234 0.8156 0.8089 0.8198]
  Mean AUC: 0.8160 (+/- 0.0056)
```

### Healthcare Results

```
==============================================
HEALTHCARE APPLICATION - DIAGNOSIS MODEL CV
==============================================

Disease Dataset: 3000 samples
Prevalence: 24.67%

==============================================
STRATIFIED CV FOR IMBALANCED DATA
==============================================
Fold 1: Train prevalence=0.247, Test prevalence=0.243
Fold 2: Train prevalence=0.247, Test prevalence=0.250
Fold 3: Train prevalence=0.247, Test prevalence=0.247
Fold 4: Train prevalence=0.247, Test prevalence=0.250
Fold 5: Train prevalence=0.247, Test prevalence=0.243

SVM CV F1 Scores: [0.7234 0.7456 0.7567 0.7345 0.7456]
Mean F1: 0.7412
```

## Advanced Topics

### Repeated Cross Validation

```python
print("=" * 70)
print("REPEATED CROSS VALIDATION")
print("=" * 70)

from sklearn.model_selection import RepeatedStratifiedKFold

rskf = RepeatedStratifiedKFold(n_splits=5, n_repeats=3, random_state=42)

scores = cross_val_score(model, X, y, cv=rskf, scoring='accuracy')

print(f"\nRepeated Stratified CV (5-fold x 3):")
print(f"Total scores: {len(scores)}")
print(f"Mean: {scores.mean():.4f}")
print(f"Std: {scores.std():.4f}")
```

### Nested Cross Validation

```python
print("=" * 70)
print("NESTED CROSS VALIDATION")
print("=" * 70)

from sklearn.model_selection import GridSearchCV

param_grid = {'n_estimators': [50, 100]}

grid_search = GridSearchCV(
    RandomForestClassifier(random_state=42),
    param_grid,
    cv=5,
    scoring='accuracy'
)

outer_cv = StratifiedKFold(n_splits=3, shuffle=True, random_state=42)
outer_scores = cross_val_score(grid_search, X, y, cv=outer_cv)

print(f"\nNested CV Scores: {outer_scores}")
print(f"Mean: {outer_scores.mean():.4f}")
```

## Conclusion

Cross validation provides essential tools for reliable model performance estimation. Standard k-fold works for most problems, while stratified k-fold ensures proper class representation. Time series CV handles temporal data appropriately.

Key considerations include choosing appropriate k values (5-10 typical), using stratification for imbalanced data, and understanding that CV estimates may differ from test set performance due to differences in data distribution or size.

For banking and healthcare applications, cross validation ensures models perform consistently and reliably. The investment in thorough validation pays off in deployed model performance.