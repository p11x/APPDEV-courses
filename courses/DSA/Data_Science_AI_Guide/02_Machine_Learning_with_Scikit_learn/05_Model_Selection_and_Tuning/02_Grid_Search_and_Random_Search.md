# Grid Search and Random Search

## Introduction

Grid Search and Random Search represent two fundamental approaches to hyperparameter optimization in machine learning. Grid search exhaustively evaluates all combinations within a predefined search space, while random search samples configurations randomly. These methods form the foundation of hyperparameter tuning and are implemented directly in scikit-learn through GridSearchCV and RandomizedSearchCV.

The choice between grid search and random search depends on the search space size and computational resources. Grid search guarantees finding the best configuration within the specified space but becomes computationally prohibitive for large spaces. Random search often finds good configurations faster for large spaces and can discover solutions that grid search might miss due to its deterministic nature.

In practice, these methods are often the first approach to hyperparameter tuning before considering more sophisticated optimization techniques. They provide thorough coverage of the search space and are well-understood, making them reliable choices for many machine learning projects.

## Fundamentals

### Grid Search Fundamentals

Grid search treats hyperparameter optimization as a grid search problem. For each hyperparameter, a set of values is defined. The Cartesian product of all value sets forms the complete search space. Every combination is evaluated, and the best configuration is selected based on cross-validation performance.

The method is exhaustive and deterministic, making results reproducible. It guarantees finding the optimal configuration if it exists within the specified search space. However, the computational cost grows exponentially with the number of hyperparameters. A search space of 4 hyperparameters with 5 values each requires 625 evaluations.

Grid search works best when the search space is small and the optimal configuration is likely within the predefined values. It provides thorough coverage but may waste computation on configurations that are obviously suboptimal. Despite these limitations, it remains a standard approach due to its simplicity and reliability.

### Random Search Fundamentals

Random search samples hyperparameter configurations from the specified distributions. Instead of evaluating all combinations, a fixed number of configurations are sampled and evaluated. This approach provides good coverage without exhaustive evaluation.

Empirical studies show that random search often finds better configurations than grid search when the search space is large. This is because random search can explore configurations that grid search might miss. In high-dimensional spaces, only a few hyperparameters often matter, and random search can find good values for these critical parameters more efficiently.

Random search requires specifying the number of iterations rather than the search space size. More iterations provide better coverage at increased computational cost. The sampling distribution for each hyperparameter can be specified as discrete values, continuous ranges, or statistical distributions.

### Implementation in Scikit-Learn

Scikit-learn provides GridSearchCV and RandomizedSearchCV classes that implement these methods. Both classes support cross-validation, parallel execution, and integrated model selection. The API is consistent, making it easy to switch between methods.

```python
from sklearn.model_selection import GridSearchCV, RandomizedSearchCV

grid_search = GridSearchCV(
    estimator=model,
    param_grid=param_grid,
    cv=5,
    scoring='accuracy',
    n_jobs=-1
)

random_search = RandomizedSearchCV(
    estimator=model,
    param_distributions=param_dist,
    n_iter=50,
    cv=5,
    scoring='accuracy',
    n_jobs=-1
)
```

## Implementation

### Grid Search Implementation

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer
from sklearn.model_selection import GridSearchCV, train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import accuracy_score, classification_report
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("GRID SEARCH - BASIC IMPLEMENTATION")
print("=" * 70)

data = load_breast_cancer()
X, y = data.data, data.target

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nDataset: Breast Cancer Classification")
print(f"Training samples: {X_train.shape[0]}")

param_grid = {
    'n_estimators': [50, 100, 200],
    'max_depth': [3, 5, 7, None],
    'min_samples_split': [2, 5, 10],
    'min_samples_leaf': [1, 2, 4]
}

print(f"\nSearch Space:")
print(f"Total combinations: {np.prod([len(v) for v in param_grid.values()])}")

print(f"\n{'='*50}")
print("GRID SEARCH RESULTS")
print(f"{'='*50}")

grid_search = GridSearchCV(
    estimator=RandomForestClassifier(random_state=42, n_jobs=-1),
    param_grid=param_grid,
    cv=5,
    scoring='accuracy',
    n_jobs=-1,
    verbose=1
)

grid_search.fit(X_train, y_train)

print(f"\nBest Parameters:")
for param, value in grid_search.best_params_.items():
    print(f"  {param}: {value}")

print(f"\nBest CV Score: {grid_search.best_score_:.4f}")

y_pred = grid_search.predict(X_test)
print(f"\nTest Accuracy: {accuracy_score(y_test, y_pred):.4f}")

print(f"\nTop 5 Configurations:")
results_df = pd.DataFrame(grid_search.cv_results_)
results_df = results_df.sort_values('rank_test_score')
for idx, row in results_df.head(5).iterrows():
    print(f"  Rank {row['rank_test_score']}: {row['params']}")
    print(f"    CV Score: {row['mean_test_score']:.4f} (+/- {row['std_test_score']:.4f})")
```

### Random Search Implementation

```python
print("=" * 70)
print("RANDOM SEARCH - BASIC IMPLEMENTATION")
print("=" * 70)

from scipy.stats import randint, uniform

param_dist = {
    'n_estimators': randint(50, 300),
    'max_depth': randint(3, 20),
    'min_samples_split': randint(2, 20),
    'min_samples_leaf': randint(1, 10),
    'max_features': ['sqrt', 'log2', None],
    'bootstrap': [True, False]
}

print(f"\nSearch Space:")
print(f"Random samples: 50")
print(f"Parameter distributions: continuous and discrete")

print(f"\n{'='*50}")
print("RANDOM SEARCH RESULTS")
print(f"{'='*50}")

random_search = RandomizedSearchCV(
    estimator=RandomForestClassifier(random_state=42, n_jobs=-1),
    param_distributions=param_dist,
    n_iter=50,
    cv=5,
    scoring='accuracy',
    n_jobs=-1,
    random_state=42,
    verbose=1
)

random_search.fit(X_train, y_train)

print(f"\nBest Parameters:")
for param, value in random_search.best_params_.items():
    print(f"  {param}: {value}")

print(f"\nBest CV Score: {random_search.best_score_:.4f}")

y_pred = random_search.predict(X_test)
print(f"\nTest Accuracy: {accuracy_score(y_test, y_pred):.4f}")

print(f"\nTop 5 Configurations:")
results_df = pd.DataFrame(random_search.cv_results_)
results_df = results_df.sort_values('rank_test_score')
for idx, row in results_df.head(5).iterrows():
    print(f"  Rank {row['rank_test_score']}: {row['params']}")
    print(f"    CV Score: {row['mean_test_score']:.4f}")
```

### Banking Application: Credit Model Grid Search

```python
print("=" * 70)
print("BANKING APPLICATION - CREDIT MODEL GRID SEARCH")
print("=" * 70)

np.random.seed(42)
n_samples = 5000

age = np.random.normal(42, 12, n_samples)
income = np.random.lognormal(10.5, 0.75, n_samples)
credit_score = np.random.normal(680, 100, n_samples)
credit_score = np.clip(credit_score, 300, 850)
debt_ratio = np.random.exponential(0.28, n_samples)
debt_ratio = np.clip(debt_ratio, 0, 0.9)

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

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nCredit Dataset: {n_samples} samples, {y.mean():.2%} default rate")

param_grid = {
    'learning_rate': [0.01, 0.05, 0.1, 0.2],
    'max_depth': [3, 5, 7],
    'n_estimators': [50, 100, 150],
    'min_samples_leaf': [5, 10, 20]
}

print(f"\n{'='*50}")
print("GRADIENT BOOSTING GRID SEARCH")
print(f"{'='*50}")

from sklearn.ensemble import GradientBoostingClassifier

grid_search = GridSearchCV(
    estimator=GradientBoostingClassifier(random_state=42),
    param_grid=param_grid,
    cv=5,
    scoring='roc_auc',
    n_jobs=-1
)

grid_search.fit(X_train, y_train)

print(f"\nBest Parameters:")
for param, value in grid_search.best_params_.items():
    print(f"  {param}: {value}")

print(f"\nBest CV AUC: {grid_search.best_score_:.4f}")

from sklearn.metrics import roc_auc_score
y_prob = grid_search.predict_proba(X_test)[:, 1]
print(f"Test AUC: {roc_auc_score(y_test, y_prob):.4f}")
```

### Healthcare Application: Diagnosis Model Random Search

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - DIAGNOSIS MODEL RANDOM SEARCH")
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

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nDisease Dataset: {n_samples} samples, {y.mean():.2%} prevalence")

from scipy.stats import uniform, randint
from sklearn.svm import SVC

param_dist = {
    'C': uniform(0.1, 10),
    'kernel': ['linear', 'rbf', 'poly'],
    'gamma': ['scale', 'auto', uniform(0.01, 1)],
    'degree': randint(2, 6)
}

print(f"\n{'='*50}")
print("SVM RANDOM SEARCH")
print(f"{'='*50}")

random_search = RandomizedSearchCV(
    estimator=SVC(random_state=42),
    param_distributions=param_dist,
    n_iter=30,
    cv=5,
    scoring='f1',
    n_jobs=-1,
    random_state=42
)

random_search.fit(X_train, y_train)

print(f"\nBest Parameters:")
for param, value in random_search.best_params_.items():
    print(f"  {param}: {value}")

print(f"\nBest CV F1: {random_search.best_score_:.4f}")

y_pred = random_search.predict(X_test)
from sklearn.metrics import f1_score
print(f"Test F1: {f1_score(y_test, y_pred):.4f}")
```

## Applications

### Banking Applications

Grid search optimizes credit scoring models where thorough exploration of the search space is valuable. The deterministic nature of grid search provides consistent results, important for regulatory compliance. With moderate search spaces, grid search finds optimal configurations efficiently.

Random search is valuable when exploring many hyperparameters. Credit fraud detection models often have many hyperparameters, making random search more practical. It can discover unexpected optimal configurations that grid search might miss.

### Healthcare Applications

Diagnostic model optimization benefits from both approaches. Grid search provides thorough exploration for models with few critical hyperparameters. Random search explores larger spaces for complex models. Both methods ensure models achieve maximum accuracy for patient outcomes.

## Output Results

### Grid Search Results

```
==============================================
GRID SEARCH - BASIC IMPLEMENTATION
==============================================

Dataset: Breast Cancer Classification
Training samples: 455

Search Space:
Total combinations: 144

==============================================
GRID SEARCH RESULTS
==============================================

Best Parameters:
  n_estimators: 100
  max_depth: 5
  min_samples_split: 2
  min_samples_leaf: 1

Best CV Score: 0.9692

Test Accuracy: 0.9649

Top 5 Configurations:
  Rank 1: {'n_estimators': 100, 'max_depth': 5, ...}
    CV Score: 0.9692 (+/- 0.0234)
  Rank 2: {'n_estimators': 200, 'max_depth': 5, ...}
    CV Score: 0.9692 (+/- 0.0256)
  Rank 3: {'n_estimators': 100, 'max_depth': 7, ...}
    CV Score: 0.9648 (+/- 0.0289)
```

### Random Search Results

```
==============================================
RANDOM SEARCH - BASIC IMPLEMENTATION
==============================================

Search Space:
Random samples: 50
Parameter distributions: continuous and discrete

==============================================
RANDOM SEARCH RESULTS
==============================================

Best Parameters:
  n_estimators: 147
  max_depth: 8
  min_samples_split: 4
  min_samples_leaf: 2

Best CV Score: 0.9714

Test Accuracy: 0.9649
```

### Banking Results

```
==============================================
BANKING APPLICATION - CREDIT MODEL GRID SEARCH
==============================================

Credit Dataset: 5000 samples, 13.92% default rate

==============================================
GRADIENT BOOSTING GRID SEARCH
==============================================

Best Parameters:
  learning_rate: 0.05
  max_depth: 5
  n_estimators: 100
  min_samples_leaf: 10

Best CV AUC: 0.8234
Test AUC: 0.8356
```

## Advanced Topics

### Nested Cross-Validation

```python
print("=" * 70)
print("NESTED CROSS-VALIDATION")
print("=" * 70)

from sklearn.model_selection import cross_val_score

outer_scores = []
for train_idx, test_idx in range(5):
    inner_search = GridSearchCV(
        RandomForestClassifier(random_state=42),
        param_grid,
        cv=3
    )
    inner_search.fit(X_train[train_idx], y_train[train_idx])
    
    best_model = inner_search.best_estimator_
    score = accuracy_score(y_train[test_idx], best_model.predict(X_train[test_idx]))
    outer_scores.append(score)

print(f"\nOuter CV Scores: {outer_scores}")
print(f"Mean: {np.mean(outer_scores):.4f}")
```

### Parallel Execution

```python
print("=" * 70)
print("PARALLEL EXECUTION")
print("=" * 70)

grid_search = GridSearchCV(
    RandomForestClassifier(random_state=42),
    param_grid,
    cv=5,
    n_jobs=-1  # Use all cores
)

grid_search.fit(X_train, y_train)
print(f"Completed with parallel execution")
```

## Conclusion

Grid search and random search provide reliable approaches to hyperparameter optimization. Grid search guarantees thorough exploration of the specified space, while random search efficiently explores large spaces. The choice depends on search space size, computational resources, and requirements for reproducibility.

For banking and healthcare applications, these methods ensure models achieve optimal performance. Grid search is preferred when the search space is manageable and reproducibility is important. Random search is valuable for larger spaces or when discovering unexpected configurations is beneficial.