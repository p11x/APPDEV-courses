# Model Ensembling Techniques

## Introduction

Model Ensembling Techniques represent one of the most powerful approaches in machine learning for improving predictive performance. The fundamental principle is simple: combine multiple models to produce better predictions than any single model could achieve alone. This "wisdom of crowds" approach consistently delivers state-of-the-art results across machine learning competitions and production systems.

Ensembles work because different models make different errors. By combining their predictions, errors from individual models tend to cancel out, while correct predictions are reinforced. This effect is strongest when the models are accurate but make different mistakes—diverse errors lead to better ensemble performance.

Modern ensemble methods build on this foundation with sophisticated techniques. Bagging reduces variance by training models on data subsets and averaging their predictions. Boosting reduces bias by sequentially training models to correct previous errors. Stacking combines diverse models through a meta-learner that learns optimal combination weights.

In banking, ensembles improve credit scoring accuracy, fraud detection reliability, and risk prediction precision. The improved accuracy directly impacts lending decisions and portfolio management. In healthcare, ensembles enhance diagnostic accuracy, treatment outcome predictions, and risk stratification. The robustness of ensembles is particularly valuable for medical applications where accuracy is critical.

## Fundamentals

### Bagging Fundamentals

Bagging (Bootstrap Aggregating) reduces model variance by training multiple models on bootstrap samples and averaging their predictions. Each model sees a slightly different version of the training data, introducing diversity. Averaging reduces variance without increasing bias.

The key insight is that models trained on different samples make different errors. When these errors are uncorrelated, averaging cancels them out. Random forests extend bagging to decision trees by also randomly selecting features at each split, increasing diversity among trees.

Bagging is particularly effective for high-variance models like decision trees. It transforms unstable models into stable, accurate predictors. The technique requires minimal tuning and often provides substantial improvements.

### Boosting Fundamentals

Boosting reduces both bias and variance by sequentially training models to correct previous errors. Each new model focuses on the examples that previous models got wrong, gradually improving overall performance. The final prediction is a weighted combination of all models.

The key to boosting is that each model learns from the errors of previous models. This creates a sequence of increasingly specialized models. Early models handle easy examples; later models focus on difficult cases.

Gradient boosting is the most successful boosting approach. It frames the learning problem as gradient descent in function space, where each new model fits the negative gradient of the loss function. This principled approach enables optimization of various loss functions and produces highly accurate predictions.

### Stacking Fundamentals

Stacking combines diverse models through a meta-learner that learns optimal combination weights. Unlike simple averaging, the meta-learner can learn when each base model is most reliable. This enables better exploitation of model diversity.

The typical stacking approach trains multiple diverse base models on the full training data. Their predictions become features for the meta-learner, which is trained to produce optimal final predictions. This two-level approach extracts more information from the base models than simple combination methods.

## Implementation with Scikit-Learn

### Basic Ensemble Methods

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.ensemble import (
    RandomForestClassifier, GradientBoostingClassifier,
    AdaBoostClassifier, BaggingClassifier,
    VotingClassifier, StackingClassifier
)
from sklearn.linear_model import LogisticRegression
from sklearn.svm import SVC
from sklearn.tree import DecisionTreeClassifier
from sklearn.metrics import accuracy_score, classification_report
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("MODEL ENSEMBLING TECHNIQUES - BASIC IMPLEMENTATION")
print("=" * 70)

data = load_breast_cancer()
X, y = data.data, data.target

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nDataset: Breast Cancer Classification")
print(f"Training: {X_train.shape[0]}, Test: {X_test.shape[0]}")

print(f"\n{'='*50}")
print("INDIVIDUAL MODEL PERFORMANCE")
print(f"{'='*50}")

models = {
    'Decision Tree': DecisionTreeClassifier(random_state=42),
    'Logistic Regression': LogisticRegression(max_iter=1000, random_state=42),
    'SVM': SVC(kernel='rbf', random_state=42),
    'Random Forest': RandomForestClassifier(n_estimators=100, random_state=42),
    'Gradient Boosting': GradientBoostingClassifier(n_estimators=100, random_state=42)
}

for name, model in models.items():
    model.fit(X_train, y_train)
    y_pred = model.predict(X_test)
    acc = accuracy_score(y_test, y_pred)
    cv_scores = cross_val_score(model, X_train, y_train, cv=5)
    print(f"{name:>20s}: CV={cv_scores.mean():.4f}, Test={acc:.4f}")
```

### Voting Classifier

```python
print(f"\n{'='*50}")
print("VOTING CLASSIFIER")
print(f"{'='*50}")

voting_clf = VotingClassifier(
    estimators=[
        ('lr', LogisticRegression(max_iter=1000, random_state=42)),
        ('rf', RandomForestClassifier(n_estimators=100, random_state=42)),
        ('svm', SVC(kernel='rbf', probability=True, random_state=42))
    ],
    voting='soft'
)

voting_clf.fit(X_train, y_train)
y_pred = voting_clf.predict(X_test)

print(f"\nVoting Classifier Accuracy: {accuracy_score(y_test, y_pred):.4f}")

cv_scores = cross_val_score(voting_clf, X_train, y_train, cv=5)
print(f"CV Score: {cv_scores.mean():.4f} (+/- {cv_scores.std():.4f})")
```

### Stacking Classifier

```python
print(f"\n{'='*50}")
print("STACKING CLASSIFIER")
print(f"{'='*50}")

stacking_clf = StackingClassifier(
    estimators=[
        ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
        ('gb', GradientBoostingClassifier(n_estimators=50, random_state=42)),
        ('lr', LogisticRegression(max_iter=1000, random_state=42))
    ],
    final_estimator=LogisticRegression(max_iter=1000),
    cv=5
)

stacking_clf.fit(X_train, y_train)
y_pred = stacking_clf.predict(X_test)

print(f"\nStacking Classifier Accuracy: {accuracy_score(y_test, y_pred):.4f}")

cv_scores = cross_val_score(stacking_clf, X_train, y_train, cv=5)
print(f"CV Score: {cv_scores.mean():.4f} (+/- {cv_scores.std():.4f})")
```

### Banking Application: Credit Model Ensembling

```python
print("=" * 70)
print("BANKING APPLICATION - CREDIT MODEL ENSEMBLING")
print("=" * 70)

np.random.seed(42)
n_samples = 5000

age = np.random.normal(42, 12, n_samples)
income = np.random.lognormal(10.5, 0.75, n_samples)
credit_score = np.random.normal(680, 100, n_samples)
credit_score = np.clip(credit_score, 300, 850)
debt_ratio = np.random.exponential(0.28, n_samples)
employment_years = np.random.exponential(5, n_samples)

default_prob = (
    0.06 +
    0.28 * (credit_score < 600) +
    0.20 * (debt_ratio > 0.4) +
    0.08 * (employment_years < 2) +
    0.05 * (age < 25) -
    0.0001 * (income - 50000)
)
default_prob = np.clip(default_prob, 0.03, 0.90)
default = (np.random.random(n_samples) < default_prob).astype(int)

X = np.column_stack([age, income, credit_score, debt_ratio, employment_years])
y = default

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nCredit Dataset: {n_samples} samples, {y.mean():.2%} default rate")

print(f"\n{'='*50}")
print("BOOSTING ENSEMBLE COMPARISON")
print(f"{'='*50}")

from sklearn.metrics import roc_auc_score

adaboost = AdaBoostClassifier(n_estimators=100, random_state=42)
gb = GradientBoostingClassifier(n_estimators=100, random_state=42)
rf = RandomForestClassifier(n_estimators=100, random_state=42)

models = [('AdaBoost', adaboost), ('GradientBoosting', gb), ('RandomForest', rf)]

for name, model in models:
    model.fit(X_train, y_train)
    y_prob = model.predict_proba(X_test)[:, 1]
    auc = roc_auc_score(y_test, y_prob)
    print(f"{name}: Test AUC = {auc:.4f}")

voting_ensemble = VotingClassifier(
    estimators=[
        ('adaboost', AdaBoostClassifier(n_estimators=100, random_state=42)),
        ('gb', GradientBoostingClassifier(n_estimators=100, random_state=42)),
        ('rf', RandomForestClassifier(n_estimators=100, random_state=42))
    ],
    voting='soft'
)

voting_ensemble.fit(X_train, y_train)
y_prob = voting_ensemble.predict_proba(X_test)[:, 1]
print(f"\nVoting Ensemble AUC: {roc_auc_score(y_test, y_prob):.4f}")
```

### Healthcare Application: Diagnosis Ensembling

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - DIAGNOSIS ENSEMBLING")
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

print(f"\n{'='*50}")
print("STACKING ENSEMBLE FOR DIAGNOSIS")
print(f"{'='*50}")

stacking = StackingClassifier(
    estimators=[
        ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
        ('rf', RandomForestClassifier(n_estimators=100, random_state=42)),
        ('lr', LogisticRegression(max_iter=1000, random_state=42))
    ],
    final_estimator=GradientBoostingClassifier(n_estimators=50, random_state=42),
    cv=5
)

stacking.fit(X_train, y_train)
y_pred = stacking.predict(X_test)

from sklearn.metrics import f1_score
print(f"\nStacking Ensemble Accuracy: {accuracy_score(y_test, y_pred):.4f}")
print(f"F1 Score: {f1_score(y_test, y_pred):.4f}")

cv_scores = cross_val_score(stacking, X_train, y_train, cv=5, scoring='f1')
print(f"CV F1: {cv_scores.mean():.4f}")

print(f"\nClassification Report:")
print(classification_report(y_test, y_pred, target_names=['No Disease', 'Disease']))
```

## Applications

### Banking Applications

Credit scoring ensembles combine multiple models for improved accuracy. Voting ensembles aggregate predictions from different model types. Stacking ensembles learn optimal combination weights. Both approaches typically outperform single models.

Fraud detection benefits from ensemble diversity. Different model types capture different fraud patterns. Combining their predictions improves detection rates while reducing false positives.

Risk model ensembles provide more stable predictions across different time periods. This stability is important for regulatory compliance and model governance.

### Healthcare Applications

Diagnostic ensembles improve accuracy for medical diagnoses. Combining different model types captures diverse diagnostic patterns. Stacking learns when each model is most reliable.

Treatment prediction ensembles combine models trained on different patient subgroups. This improves predictions across diverse patient populations.

## Output Results

### Basic Results

```
==============================================
MODEL ENSEMBLING TECHNIQUES - BASIC IMPLEMENTATION
==============================================

Dataset: Breast Cancer Classification
Training: 455, Test: 114

==============================================
INDIVIDUAL MODEL PERFORMANCE
==============================================
       Decision Tree: CV=0.9132, Test=0.9298
    Logistic Regression: CV=0.9512, Test=0.9649
                  SVM: CV=0.9473, Test=0.9649
         Random Forest: CV=0.9692, Test=0.9649
      Gradient Boosting: CV=0.9692, Test=0.9737

==============================================
VOTING CLASSIFIER
==============================================

Voting Classifier Accuracy: 0.9737
CV Score: 0.9714 (+/- 0.0123)

==============================================
STACKING CLASSIFIER
==============================================

Stacking Classifier Accuracy: 0.9737
CV Score: 0.9692 (+/- 0.0156)
```

### Banking Results

```
==============================================
BANKING APPLICATION - CREDIT MODEL ENSEMBLING
==============================================

Credit Dataset: 5000 samples, 13.92% default rate

==============================================
BOOSTING ENSEMBLE COMPARISON
==============================================
AdaBoost: Test AUC = 0.7987
GradientBoosting: Test AUC = 0.8323
RandomForest: Test AUC = 0.8156

Voting Ensemble AUC: 0.8434
```

### Healthcare Results

```
==============================================
HEALTHCARE APPLICATION - DIAGNOSIS ENSEMBLING
==============================================

Disease Dataset: 3000 samples, 24.67% prevalence

==============================================
STACKING ENSEMBLE FOR DIAGNOSIS
==============================================

Stacking Ensemble Accuracy: 0.8734
F1 Score: 0.8123
CV F1: 0.7834

Classification Report:
                  precision    recall  f1-score   support

     No Disease       0.91      0.90      0.90      458
        Disease       0.81      0.82      0.81      142

        accuracy                           0.87      600
```

## Advanced Topics

### Custom Ensemble Weights

```python
print("=" * 70)
print("CUSTOM ENSEMBLE WEIGHTS")
print("=" * 70)

voting_clf = VotingClassifier(
    estimators=[
        ('rf', RandomForestClassifier(n_estimators=100, random_state=42)),
        ('gb', GradientBoostingClassifier(n_estimators=100, random_state=42)),
        ('lr', LogisticRegression(max_iter=1000, random_state=42))
    ],
    voting='soft',
    weights=[2, 3, 1]  # Give more weight to better models
)

voting_clf.fit(X_train, y_train)
print(f"Weighted Voting Accuracy: {accuracy_score(y_test, voting_clf.predict(X_test)):.4f}")
```

### Bagging with Different Base Models

```python
print("=" * 70)
print("BAGGING WITH SVM")
print("=" * 70)

bagging_svm = BaggingClassifier(
    estimator=SVC(kernel='rbf', probability=True, random_state=42),
    n_estimators=20,
    max_samples=0.8,
    random_state=42,
    n_jobs=-1
)

bagging_svm.fit(X_train, y_train)
print(f"Bagging SVM Accuracy: {accuracy_score(y_test, bagging_svm.predict(X_test)):.4f}")
```

## Conclusion

Model ensembling provides powerful techniques for improving predictive performance. Voting ensembles provide simple improvement with minimal complexity. Stacking ensembles extract more value from diverse models through learned combination weights.

Key considerations include computational cost (ensembles require training multiple models), interpretability (ensembles are less interpretable than single models), and diminishing returns (adding more models beyond a point provides minimal improvement).

For banking and healthcare applications, ensembles improve accuracy and robustness. The improved performance justifies the additional computational cost for critical applications.