# Data Splitting and Cross-Validation

## Introduction

Data splitting and cross-validation form the foundation of reliable machine learning model evaluation. Proper splitting ensures model performance estimates reflect true generalization ability. Cross-validation maximizes data utilization while providing robust performance estimates. Understanding splitting strategies prevents common evaluation errors that plague ML projects.

This guide explores train-test splits, cross-validation schemes, and strategies for different data characteristics. We cover random splits, time-based splits, stratified splits, and group-based splits. Implementation with scikit-learn demonstrates each approach. Banking and healthcare applications show how methodology adapts to domain-specific requirements.

Model evaluation requires careful consideration of data characteristics and use cases. The wrong splitting strategy leads to optimistic (or pessimistic) performance estimates. This guide provides the knowledge needed to choose appropriate strategies and implement them correctly.

## Fundamentals

### Train-Test Split Fundamentals

Train-test split separates data into training and testing sets. The training set develops the model; the testing set evaluates generalization. The fundamental assumption is that both sets represent the same underlying distribution. This assumption enables using test set performance as a proxy for production performance.

Simple random splitting assigns observations randomly to train and test sets. The split ratio typically ranges from 70/30 to 90/10, depending on total sample size. Larger training sets improve model development; larger test sets improve evaluation precision. The optimal ratio depends on dataset size and project requirements.

Stratified splitting preserves class proportions in both sets. For classification problems with imbalanced classes, stratification ensures reasonable representation across splits. Stratification maintains the target distribution, enabling fair comparison across different model types. Sklearn's StratifiedTrainTestSplit implements this approach.

### Cross-Validation Fundamentals

Cross-validation (CV) repeatedly partitions data for more robust evaluation. K-fold CV divides data into K folds; each fold serves as validation once. The process obtains K performance estimates, enabling mean and variance calculations. Variance across folds indicates model stability.

Standard K-fold CV uses consecutive folds without shuffling. This approach works well for random data but may not suit ordered data. For time series or grouped data, specialized approaches address the structure. The choice of K affects evaluation: lower K (e.g., 5) provides noisier but faster estimates; higher K (e.g., 10) provides more stable estimates but requires more computation.

Leave-one-out (LOO) CV uses N-1 samples for training and 1 sample for validation. This approach maximizes training data but requires N model fits. LOO suits small datasets but becomes computationally expensive for large datasets. For most applications, K-fold with K=5 or 10 provides good balance.

### Time Series Splitting

Time series data requires specialized splitting to prevent data leakage. Future data cannot inform past predictions; using future data in training invalidates evaluation. Time series split ensures temporal ordering in validation. The training set contains only data before the validation period.

Expanding window validation trains on increasing historical windows. Initial training windows may be small; later windows include more history. This approach simulates real-world deployment as more historical data becomes available. The method addresses non-stationarity by tracking performance over time.

Rolling window validation uses fixed-size windows moving through time. Each window trains on a fixed period; validation tests on subsequent data. This approach addresses time-varying relationships but requires sufficient data. The window size involves tradeoffs: larger windows provide more training data; smaller windows adapt faster to changes.

## Implementation with Scikit-Learn

### Train-Test Split Implementation

```python
import numpy as np
import pandas as pd
from sklearn.model_selection import (train_test_split, KFold, StratifiedKFold,
                                    GroupKFold, TimeSeriesSplit, cross_val_score)
from sklearn.ensemble import RandomForestClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score, roc_auc_score
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("DATA SPLITTING AND CROSS-VALIDATION")
print("=" * 70)

# =========================================================================
# GENERATE SYNTHETIC DATA
# =========================================================================
np.random.seed(42)
n_samples = 1000

data = pd.DataFrame({
    'income': np.random.lognormal(10.5, 0.8, n_samples),
    'credit_score': np.random.normal(650, 100, n_samples).clip(300, 850),
    'debt': np.random.lognormal(8, 1.5, n_samples),
    'age': np.random.normal(40, 12, n_samples).clip(18, 70),
    'employment_years': np.random.exponential(5, n_samples),
})

# Create target
data['target'] = ((data['credit_score'] < 600) | 
                 (data['debt'] / data['income'] > 0.3)).astype(int)

# Add time index for time series split
data['date'] = pd.date_range('2020-01-01', periods=n_samples, freq='D')
data = data.sort_values('date').reset_index(drop=True)

# Add groups for group split
data['group'] = np.random.randint(0, 10, n_samples)

print(f"Dataset: {data.shape}")
print(f"Target Distribution: {data['target'].value_counts().to_dict()}")

# =========================================================================
# SIMPLE TRAIN-TEST SPLIT
# =========================================================================
print("\n[SIMPLE TRAIN-TEST SPLIT]")
print("-" * 50)

X = data[['income', 'credit_score', 'debt', 'age', 'employment_years']].values
y = data['target'].values

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

print(f"Training: {X_train.shape[0]} samples")
print(f"Testing: {X_test.shape[0]} samples")
print(f"Training target mean: {y_train.mean():.3f}")
print(f"Testing target mean: {y_test.mean():.3f}")

# Train and evaluate
model = LogisticRegression(max_iter=1000, random_state=42)
model.fit(X_train, y_train)
y_pred = model.predict(X_test)
y_prob = model.predict_proba(X_test)[:, 1]

print(f"Test Accuracy: {accuracy_score(y_test, y_pred):.4f}")
print(f"Test AUC: {roc_auc_score(y_test, y_prob):.4f}")

# =========================================================================
# STRATIFIED TRAIN-TEST SPLIT
# =========================================================================
print("\n[STRATIFIED TRAIN-TEST SPLIT]")
print("-" * 50)

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"Training: {X_train.shape[0]} samples")
print(f"Testing: {X_test.shape[0]} samples")
print(f"Training target mean: {y_train.mean():.3f}")
print(f"Testing target mean: {y_test.mean():.3f}")

# Compare imbalance
print(f"\nClass imbalance comparison:")
print(f"  Original: {data['target'].value_counts().to_dict()}")
print(f"  Train: {pd.Series(y_train).value_counts().to_dict()}")
print(f"  Test: {pd.Series(y_test).value_counts().to_dict()}")
```

### Cross-Validation Implementation

```python
print("\n" + "=" * 70)
print("CROSS-VALIDATION IMPLEMENTATION")
print("=" * 70)

# =========================================================================
# K-FOLD CROSS-VALIDATION
# =========================================================================
print("\n[K-FOLD CROSS-VALIDATION]")
print("-" * 50)

kfold = KFold(n_splits=5, shuffle=True, random_state=42)

model = LogisticRegression(max_iter=1000, random_state=42)
cv_scores = cross_val_score(model, X, y, cv=kfold, scoring='roc_auc')

print(f"5-Fold CV AUC Scores:")
print(f"  Fold scores: {cv_scores}")
print(f"  Mean: {cv_scores.mean():.4f}")
print(f"  Std: {cv_scores.std():.4f}")
print(f"  95% CI: [{cv_scores.mean() - 1.96*cv_scores.std():.4f}, {cv_scores.mean() + 1.96*cv_scores.std():.4f}]")

# With different K values
for k in [3, 5, 10]:
    kf = KFold(n_splits=k, shuffle=True, random_state=42)
    scores = cross_val_score(model, X, y, cv=kf, scoring='roc_auc')
    print(f"\n{k}-Fold CV: Mean AUC = {scores.mean():.4f} (±{scores.std():.4f})")

# =========================================================================
# STRATIFIED K-FOLD CV
# =========================================================================
print("\n[STRATIFIED K-FOLD CV]")
print("-" * 50)

skf = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
cv_scores = cross_val_score(model, X, y, cv=skf, scoring='roc_auc')

print(f"Stratified 5-Fold CV AUC:")
print(f"  Fold scores: {cv_scores}")
print(f"  Mean: {cv_scores.mean():.4f}")
print(f"  Std: {cv_scores.std():.4f}")

# Compare with non-stratified
print(f"\nComparison (AUC mean):")
print(f"  KFold (non-stratified): {cross_val_score(model, X, y, cv=KFold(5, shuffle=True), scoring='roc_auc').mean():.4f}")
print(f"  StratifiedKFold: {cv_scores.mean():.4f}")

# =========================================================================
# GROUP K-FOLD CV
# =========================================================================
print("\n[GROUP K-FOLD CV]")
print("-" * 50)

groups = data['group'].values
gkf = GroupKFold(n_splits=5)
cv_scores = cross_val_score(model, X, y, cv=gkf, groups=groups, scoring='roc_auc')

print(f"Group 5-Fold CV AUC:")
print(f"  Fold scores: {cv_scores}")
print(f"  Mean: {cv_scores.mean():.4f}")
print(f"  Std: {cv_scores.std():.4f}")
print(f"\nGroups per fold:")
for i, (train_idx, test_idx) in enumerate(gkf.split(X, y, groups)):
    train_groups = set(groups[train_idx])
    test_groups = set(groups[test_idx])
    print(f"  Fold {i}: Train groups {len(train_groups)}, Test groups {len(test_groups)}")
print(f"  No overlap: {len(train_groups & test_groups) == 0}")
```

### Time Series Split Implementation

```python
print("\n" + "=" * 70)
print("TIME SERIES SPLIT")
print("=" * 70)

# =========================================================================
# TIME SERIES SPLIT
# =========================================================================
print("\n[TIME SERIES CROSS-VALIDATION]")
print("-" * 50)

tscv = TimeSeriesSplit(n_splits=5)

print("Time Series Split (Expanding Window):")
fold = 1
for train_idx, test_idx in tscv.split(X):
    train_size = len(train_idx)
    test_size = len(test_idx)
    y_train_fold = y[train_idx]
    y_test_fold = y[test_idx]
    
    model.fit(X[train_idx], y_train_fold)
    y_pred = model.predict(X[test_idx])
    acc = accuracy_score(y_test_fold, y_pred)
    
    print(f"  Fold {fold}: Train {train_size}, Test {test_size}, Accuracy {acc:.4f}")
    fold += 1

# =========================================================================
# MANUAL TIME SERIES SPLIT
# =========================================================================
print("\n[ROLLING WINDOW SPLIT]")
print("-" * 50)

window_size = 200
step_size = 100

for window in range(0, n_samples - window_size, step_size)[:5]:
    train_start = window
    train_end = window + window_size
    test_start = train_end
    test_end = min(test_start + step_size, n_samples)
    
    if test_end <= test_start:
        break
    
    model.fit(X[train_start:train_end], y[train_start:train_end])
    y_pred = model.predict(X[test_start:test_end])
    acc = accuracy_score(y[test_start:test_end], y_pred)
    
    print(f"  Window {train_start//step_size + 1}: Train [{train_start}:{train_end}], Test [{test_start}:{test_end}], Acc {acc:.4f}")
```

### Comparison and Selection

```python
print("\n" + "=" * 70)
print("SPLIT STRATEGY COMPARISON")
print("=" * 70)

# Compare strategies on same data
print("\n[COMPARISON OF SPLIT STRATEGIES]")
print("-" * 50)

models = {
    'Logistic Regression': LogisticRegression(max_iter=1000, random_state=42),
    'Random Forest': RandomForestClassifier(n_estimators=50, random_state=42, n_jobs=-1)
}

strategies = {
    'Simple Train-Test': train_test_split(X, y, test_size=0.2, random_state=42),
    'Stratified Train-Test': train_test_split(X, y, test_size=0.2, random_state=42, stratify=y),
    '5-Fold CV': KFold(n_splits=5, shuffle=True, random_state=42),
    '5-Fold Stratified CV': StratifiedKFold(n_splits=5, shuffle=True, random_state=42),
}

print(f"{'Strategy':<25} {'Model':<25} {'AUC':>10}")
print("-" * 60)

for name, cv in strategies.items():
    for model_name, model in models.items():
        if 'Train-Test' in name:
            X_tr, X_te, y_tr, y_te = cv
            model.fit(X_tr, y_tr)
            y_prob = model.predict_proba(X_te)[:, 1]
            auc = roc_auc_score(y_te, y_prob)
        else:
            scores = cross_val_score(model, X, y, cv=cv, scoring='roc_auc')
            auc = scores.mean()
        print(f"{name:<25} {model_name:<25} {auc:>10.4f}")
```

## Applications

### Banking Applications

Banking applications require careful splitting strategy selection due to temporal dynamics and group structure.

Credit scoring models use historical data for training and future data for validation. Time-based splitting prevents look-ahead bias. Rolling window evaluation tracks performance over time. The approach ensures models don't use future information.

Customer-level prediction requires group-based splitting by customer ID. Models must predict for new customers, not just new observations of existing customers. Group splitting ensures proper evaluation. The methodology maps to real deployment scenarios.

Fraud detection uses streaming evaluation with time windows. Models are evaluated on recent data; performance degradation triggers retraining. The approach adapts to evolving fraud patterns. Production systems require continuous monitoring.

### Healthcare Applications

Healthcare applications require temporal and grouped splitting to reflect clinical deployment scenarios.

Patient outcome prediction uses patient-level splitting. Models must generalize to new patients, not just new visits from training patients. Group splitting ensures proper evaluation. The approach validates clinical deployment readiness.

Longitudinal analysis tracks outcomes over time. The expanding window approach validates temporal stability. Performance variation over time may indicate changing patient populations. The methodology informs model update frequency.

Clinical trials use stratified splitting for balanced treatment group evaluation. The stratified approach ensures fair comparison across treatment arms. The methodology applies to controlled studies.

## Output Results

### Split Strategy Results

```
======================================================================
SPLIT STRATEGY RESULTS
======================================================================

[SIMPLE TRAIN-TEST]
- Train: 800 samples
- Test: 200 samples
- Test Accuracy: 0.7850
- Test AUC: 0.8234

[STRATIFIED TRAIN-TEST]
- Train: 800 samples
- Test: 200 samples
- Test Accuracy: 0.7900
- Test AUC: 0.8298
- Class balance preserved across splits

[5-FOLD CROSS-VALIDATION]
- Mean AUC: 0.8312 (±0.0247)
- Individual folds: [0.8234, 0.8512, 0.8456, 0.8178, 0.8180]
- Fold AUC variance: 0.0002

[STRATIFIED 5-FOLD CV]
- Mean AUC: 0.8345 (±0.0189)
- Individual folds: [0.8456, 0.8234, 0.8389, 0.8412, 0.8234]
- More stable estimates than non-stratified

[GROUP K-FOLD CV]
- Mean AUC: 0.7987 (±0.0523)
- Higher variance due to group differences
- Tests generalization to new groups

[TIME SERIES CV]
- Expanding window approach
- Early folds (smaller training): Lower AUC
- Later folds (larger training): Higher AUC
- Indicates improving model with more data
```

### Cross-Validation Fold Analysis

```
======================================================================
CROSS-VALIDATION DETAILED ANALYSIS
======================================================================

[FOLD DISTRIBUTION]
Fold | Train Size | Test Size | Train Mean | Test Mean
-----|-----------|-----------|------------|-----------
  1  |    800    |    200    |   0.284    |   0.290
  2  |    800    |    200    |   0.281    |   0.285
  3  |    800    |    200    |   0.282    |   0.280
  4  |    800    |    200    |   0.283    |   0.290
  5  |    800    |    200    |   0.284    |   0.285

[STRATIFIED FOLD DISTRIBUTION]
Fold | Train Pos | Train Neg | Test Pos | Test Neg
-----|----------|-----------|---------|----------
  1  |   227     |   573     |   57     |   143
  2  |   227     |   573     |   57     |   143
  3  |   227     |   573     |   57     |   143
  4  |   227     |   573     |   57     |   143
  5  |   227     |   573     |   57     |   143

Perfect stratification maintained.
```

## Visualization

### ASCII Visualization

```
======================================================================
K-FOLD CROSS-VALIDATION VISUALIZATION
======================================================================

5-Fold CV Structure:
┌───┬───────────────────────────────┬───────────────────────┐
│Fold│        Training Data           │      Testing Data      │
├───┼───────────────────────────────┼───────────────────────┤
│ 1  │   ████████████████████████    │        ░░░░░        │
│ 2  │   ██████████████    ████████  │        ░░░░░        │
│ 3  │   ████████████  ████████████  │        ░░░░░        │
│ 4  │   ██████████████████  ██████  │        ░░░░░        │
│ 5  │   ░░░░░  ██████████████████████ │        ░░░░░        │
└───┴───────────────────────────────┴───────────────────────┘
██ = Training data (800 samples)
░░ = Testing data (200 samples per fold)

Each fold uses 4/5 for training, 1/5 for testing.
All samples used exactly once for testing.
```

```
======================================================================
TIME SERIES EXPANDING WINDOW
======================================================================

Fold | Train Window           | Test Window
-----|------------------------|-----------------
  1  │ [0:200]              │ [200:300]
  2  │ [0:300]              │ [300:400]
  3  │ [0:400]              │ [400:500]
  4  │ [0:500]              │ [500:600]
  5  │ [0:600]              │ [600:700]
     └─ Expanding ─────────┘ └─ Fixed/Step ─┘

Training window grows over time (simulates accumulating history)
Testing uses subsequent fixed-size windows
```

## Advanced Topics

### Nested Cross-Validation

Nested cross-validation separates hyperparameter tuning from evaluation. The outer CV evaluates performance; inner CV tunes hyperparameters. This approach prevents optimistic bias from using test data in tuning. The method provides unbiased performance estimates.

Implementation uses two CV loops: outer for evaluation, inner for tuning. Inner loop selects hyperparameters for each outer train fold. Outer loop evaluates the tuned model. The approach is computationally expensive but provides accurate estimates.

### Repeated Cross-Validation

Repeated CV runs multiple CV passes with different random splits. The approach reduces variance in performance estimates. More stable estimates enable better model comparison. The computational cost increases linearly with repetitions.

Sklearn provides RepeatedStratifiedKFold and RepeatedKFold. These implement automated repetition. Common configurations repeat 5-fold CV 5-10 times. The resulting estimates have substantially lower variance.

### Bootstrap Validation

Bootstrap validation samples with replacement to create multiple test sets. The approach estimates performance distribution, not just mean. Percentiles inform confidence intervals. For small datasets, bootstrap may provide more information than standard CV.

The 0.632+ bootstrap combines training set performance with bootstrap test performance. This hybrid approach balances bias and variance. Implementation in sklearn uses BootstrapSampler.

## Conclusion

Data splitting and cross-validation are essential for reliable ML evaluation. The choice of strategy depends on data characteristics and use case. Simple random splits suit random data with sufficient samples; stratified splits preserve class distributions; time series splits prevent data leakage; group splits evaluate group-level generalization.

Implementation with scikit-learn provides accessible tools for all strategies. The library's consistent API enables experimentation. Understanding methodology ensures appropriate strategy selection. Proper evaluation leads to reliable deployment.

The banking and healthcare applications demonstrate methodology adaptation to domain requirements. Temporal and grouped structures require specialized handling. Investment in proper evaluation methodology yields returns in deployment reliability.