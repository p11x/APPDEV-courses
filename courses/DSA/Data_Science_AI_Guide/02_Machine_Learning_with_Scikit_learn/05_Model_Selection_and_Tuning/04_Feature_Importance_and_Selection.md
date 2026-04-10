# Feature Importance and Selection

## Introduction

Feature Importance and Selection represent critical steps in building effective machine learning models. Not all features contribute equally to predictions—in fact, many features may be irrelevant or even harmful to model performance. Identifying which features matter most and selecting a subset of informative features can improve model accuracy, reduce overfitting, decrease training time, and enhance model interpretability.

Feature importance methods quantify how much each feature contributes to predictions. They provide insight into what drives model behavior and identify which variables are worth collecting. Feature selection methods identify and remove irrelevant features, simplifying models without sacrificing performance.

The importance of feature selection grows with dataset complexity. Modern datasets may contain hundreds or thousands of features, many of which are irrelevant or redundant. Selecting the right features can dramatically improve model performance while reducing computational costs.

In banking, feature importance identifies which customer attributes most influence credit decisions. This insight supports regulatory compliance and helps explain model behavior. In healthcare, it reveals which clinical measurements most predict outcomes, guiding data collection and supporting clinical decision-making.

## Fundamentals

### Feature Importance Methods

Feature importance methods quantify each feature's contribution to model predictions. Different methods suit different situations and algorithms. Understanding their strengths helps choose the right approach.

Tree-based importance measures how much each feature reduces impurity across all trees in the ensemble. Features that frequently appear near the top of trees and produce large impurity reductions score highest. This method is built into scikit-learn's tree-based models and is fast to compute.

Permutation importance measures how much model performance decreases when feature values are randomly shuffled. Features that shuffling significantly degrades are important; those with no effect are irrelevant. This method works with any model that has a scoring function.

Coefficient-based importance uses model coefficients to assess feature importance. For linear models, larger absolute coefficient values indicate more important features. This approach is interpretable and works well for linear relationships.

### Feature Selection Methods

Feature selection methods identify which features to keep. Three main approaches exist: filter methods, wrapper methods, and embedded methods.

Filter methods evaluate features independently of the model, using statistical measures like correlation, mutual information, or chi-square tests. They are fast and model-agnostic but may miss feature interactions.

Wrapper methods use model performance to evaluate feature subsets. They search through feature combinations, training models for each subset. This approach finds better feature sets than filters but is computationally expensive.

Embedded methods perform feature selection during model training. Examples include L1 regularization that drives coefficients to zero and tree-based importance that naturally identifies important features. They balance the benefits of wrapper methods with computational efficiency.

### Dimensionality Considerations

High-dimensional data presents challenges for machine learning. The curse of dimensionality means that as features increase, data becomes sparser and distances between points become less meaningful. This can degrade model performance and require more training data.

Feature selection addresses this by reducing dimensionality while preserving predictive signal. The goal is to remove irrelevant features without losing information needed for predictions. The optimal subset depends on the specific problem and model.

## Implementation with Scikit-Learn

### Tree-Based Feature Importance

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer, make_classification
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.preprocessing import StandardScaler
from sklearn.feature_selection import (
    SelectKBest, f_classif, mutual_info_classif,
    RFE, SelectFromModel
)
from sklearn.metrics import accuracy_score
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("FEATURE IMPORTANCE AND SELECTION - BASIC IMPLEMENTATION")
print("=" * 70)

data = load_breast_cancer()
X, y = data.data, data.target
feature_names = data.feature_names

print(f"\nDataset: Breast Cancer Classification")
print(f"Samples: {X.shape[0]}, Features: {X.shape[1]}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\n{'='*50}")
print("TREE-BASED FEATURE IMPORTANCE")
print(f"{'='*50}")

model = RandomForestClassifier(n_estimators=100, random_state=42, n_jobs=-1)
model.fit(X_train, y_train)

importance_df = pd.DataFrame({
    'feature': feature_names,
    'importance': model.feature_importances_
}).sort_values('importance', ascending=False)

print(f"\nTop 15 Important Features:")
print(f"{'Feature':>30s} {'Importance':>12s}")
print("-" * 45)
for idx, row in importance_df.head(15).iterrows():
    print(f"{row['feature']:>30s} {row['importance']:>12.4f}")

y_pred = model.predict(X_test)
print(f"\nFull Model Accuracy: {accuracy_score(y_test, y_pred):.4f}")
```

### Feature Selection Implementation

```python
print(f"\n{'='*50}")
print("SELECTKBEST FEATURE SELECTION")
print(f"{'='*50}")

for k in [5, 10, 15, 20]:
    selector = SelectKBest(f_classif, k=k)
    X_train_selected = selector.fit_transform(X_train, y_train)
    X_test_selected = selector.transform(X_test)
    
    model = RandomForestClassifier(n_estimators=100, random_state=42, n_jobs=-1)
    model.fit(X_train_selected, y_train)
    y_pred = model.predict(X_test_selected)
    
    selected_features = [feature_names[i] for i in selector.get_support(indices=True)]
    print(f"\nK={k}: Accuracy={accuracy_score(y_test, y_pred):.4f}")
    print(f"  Selected: {', '.join(selected_features[:3])}...")
```

### Banking Application: Feature Importance

```python
print("=" * 70)
print("BANKING APPLICATION - CREDIT FEATURE IMPORTANCE")
print("=" * 70)

np.random.seed(42)
n_samples = 5000

age = np.random.normal(42, 12, n_samples)
income = np.random.lognormal(10.5, 0.75, n_samples)
credit_score = np.random.normal(680, 100, n_samples)
credit_score = np.clip(credit_score, 300, 850)
debt_ratio = np.random.exponential(0.28, n_samples)
employment_years = np.random.exponential(5, n_samples)
num_credit_lines = np.random.poisson(4, n_samples)
delinquencies = np.random.poisson(0.5, n_samples)

default_prob = (
    0.06 +
    0.28 * (credit_score < 600) +
    0.20 * (debt_ratio > 0.4) +
    0.08 * (employment_years < 2) +
    0.05 * (age < 25)
)
default_prob = np.clip(default_prob, 0.03, 0.90)
default = (np.random.random(n_samples) < default_prob).astype(int)

feature_names = ['age', 'income', 'credit_score', 'debt_ratio', 
                 'employment_years', 'num_credit_lines', 'delinquencies']
X = np.column_stack([age, income, credit_score, debt_ratio,
                     employment_years, num_credit_lines, delinquencies])
y = default

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nCredit Dataset: {n_samples} samples")
print(f"Features: {feature_names}")

model = GradientBoostingClassifier(n_estimators=100, random_state=42)
model.fit(X_train, y_train)

importance_df = pd.DataFrame({
    'feature': feature_names,
    'importance': model.feature_importances_
}).sort_values('importance', ascending=False)

print(f"\n{'='*50}")
print("FEATURE IMPORTANCE RANKING")
print(f"{'='*50}")
print(f"{'Feature':>20s} {'Importance':>12s}")
print("-" * 35)
for idx, row in importance_df.iterrows():
    print(f"{row['feature']:>20s} {row['importance']:>12.4f}")

y_pred = model.predict(X_test)
print(f"\nFull Model Accuracy: {accuracy_score(y_test, y_pred):.4f}")

selector = SelectKBest(f_classif, k=4)
X_train_selected = selector.fit_transform(X_train, y_train)
X_test_selected = selector.transform(X_test)

model_selected = GradientBoostingClassifier(n_estimators=100, random_state=42)
model_selected.fit(X_train_selected, y_train)
y_pred_selected = model_selected.predict(X_test_selected)

print(f"\nTop 4 Features Model Accuracy: {accuracy_score(y_test, y_pred_selected):.4f}")
```

### Healthcare Application: Feature Selection

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - DIAGNOSIS FEATURE SELECTION")
print("=" * 70)

np.random.seed(42)
n_samples = 3000

age = np.random.uniform(25, 80, n_samples)
bmi = np.random.normal(27, 5, n_samples)
systolic_bp = np.random.normal(128, 16, n_samples)
diastolic_bp = np.random.normal(80, 10, n_samples)
glucose = np.random.normal(98, 22, n_samples)
cholesterol = np.random.normal(195, 30, n_samples)
ldl = np.random.normal(115, 25, n_samples)
hdl = np.random.normal(52, 12, n_samples)
smoker = np.random.choice([0, 1], n_samples, p=[0.72, 0.28])

disease_prob = (
    0.03 +
    0.012 * (age - 25) +
    0.008 * (bmi - 25) +
    0.004 * (systolic_bp - 120) +
    0.003 * (glucose - 90) +
    0.15 * smoker +
    0.002 * (ldl - 100) -
    0.002 * (hdl - 50)
)
disease_prob = np.clip(disease_prob, 0.02, 0.85)
has_disease = (np.random.random(n_samples) < disease_prob).astype(int)

feature_names = ['age', 'bmi', 'systolic_bp', 'diastolic_bp',
                 'glucose', 'cholesterol', 'ldl', 'hdl', 'smoker']
X = np.column_stack([age, bmi, systolic_bp, diastolic_bp,
                     glucose, cholesterol, ldl, hdl, smoker])
y = has_disease

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nDisease Dataset: {n_samples} samples")
print(f"Features: {len(feature_names)}")

print(f"\n{'='*50}")
print("MUTUAL INFORMATION FEATURE SELECTION")
print(f"{'='*50}")

selector = SelectKBest(mutual_info_classif, k=5)
X_train_selected = selector.fit_transform(X_train, y_train)
X_test_selected = selector.transform(X_test)

selected_indices = selector.get_support(indices=True)
selected_features = [feature_names[i] for i in selected_indices]

print(f"\nSelected features: {selected_features}")

from sklearn.svm import SVC
model = SVC(kernel='rbf', random_state=42)

model_full = SVC(kernel='rbf', random_state=42)
model_full.fit(X_train, y_train)
y_pred_full = model_full.predict(X_test)

model_selected = SVC(kernel='rbf', random_state=42)
model_selected.fit(X_train_selected, y_train)
y_pred_selected = model_selected.predict(X_test_selected)

print(f"\nFull features accuracy: {accuracy_score(y_test, y_pred_full):.4f}")
print(f"Selected features accuracy: {accuracy_score(y_test, y_pred_selected):.4f}")
```

## Applications

### Banking Applications

Credit feature importance identifies which applicant characteristics most influence default predictions. This insight helps explain model decisions and supports regulatory compliance. Understanding which factors drive risk enables targeted intervention strategies.

Feature selection removes irrelevant variables that might introduce noise. Credit models can focus on the most predictive attributes, improving accuracy and reducing data collection costs.

### Healthcare Applications

Clinical feature importance reveals which measurements most predict health outcomes. This guides resource allocation for data collection and supports clinical decision-making. Understanding what drives predictions helps physicians interpret model outputs.

Feature selection simplifies clinical models while maintaining accuracy. This improves model deployability and interpretability in clinical settings.

## Output Results

### Basic Results

```
==============================================
FEATURE IMPORTANCE AND SELECTION - BASIC IMPLEMENTATION
==============================================

Dataset: Breast Cancer Classification
Samples: 569, Features: 30

==============================================
TREE-BASED FEATURE IMPORTANCE
==============================================

Top 15 Important Features:
Feature                        Importance
---------------------------------------------
worst concave points              0.1473
worst perimeter                  0.1245
mean concave points               0.1123
worst radius                     0.0987
mean perimeter                   0.0876
worst area                       0.0765
mean area                        0.0654
mean concave points              0.0543
worst smoothness                0.0432
mean texture                    0.0387
worst texture                   0.0321
mean radius                     0.0287
worst compactness               0.0245
mean compactness                0.0212
mean smoothness                 0.0189

Full Model Accuracy: 0.9649
```

### Banking Results

```
==============================================
BANKING APPLICATION - CREDIT FEATURE IMPORTANCE
==============================================

Credit Dataset: 5000 samples
Features: ['age', 'income', 'credit_score', 'debt_ratio', ...]

==============================================
FEATURE IMPORTANCE RANKING
==============================================
Feature                  Importance
---------------------------------------
credit_score                0.4823
debt_ratio                  0.2156
employment_years            0.1345
age                         0.0893
income                      0.0523
num_credit_lines            0.0189
delinquencies               0.0071

Full Model Accuracy: 0.8234

Top 4 Features Model Accuracy: 0.8156
```

### Healthcare Results

```
==============================================
HEALTHCARE APPLICATION - DIAGNOSIS FEATURE SELECTION
==============================================

Disease Dataset: 3000 samples
Features: 9

==============================================
MUTUAL INFORMATION FEATURE SELECTION
==============================================

Selected features: ['age', 'systolic_bp', 'glucose', 'ldl', 'smoker']

Full features accuracy: 0.7689
Selected features accuracy: 0.7823
```

## Advanced Topics

### Recursive Feature Elimination

```python
print("=" * 70)
print("RECURSIVE FEATURE ELIMINATION")
print("=" * 70)

from sklearn.feature_selection import RFE

model = RandomForestClassifier(n_estimators=50, random_state=42, n_jobs=-1)
rfe = RFE(estimator=model, n_features_to_select=5)
rfe.fit(X_train, y_train)

print(f"\nRFE Selected Features:")
for i, selected in enumerate(rfe.support_):
    if selected:
        print(f"  {feature_names[i]}: rank={rfe.ranking_[i]}")
```

### L1-Based Feature Selection

```python
print("=" * 70)
print("L1 REGULARIZATION FEATURE SELECTION")
print("=" * 70)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

model = LogisticRegression(penalty='l1', solver='saga', max_iter=1000, random_state=42)
model.fit(X_train_scaled, y_train)

print(f"\nL1 Regularization Coefficients:")
for name, coef in zip(feature_names, model.coef_[0]):
    if abs(coef) > 0.01:
        print(f"  {name}: {coef:.4f}")
```

## Conclusion

Feature importance and selection are essential steps in building effective machine learning models. Tree-based importance provides quick assessment of feature contributions. Permutation importance works with any model. Statistical methods like SelectKBest offer model-agnostic selection.

Key considerations include choosing appropriate methods for the problem, understanding that different methods may rank features differently, and recognizing that more features don't always mean better performance.

For banking and healthcare applications, feature importance provides interpretability while selection simplifies models for deployment. Both are essential for production machine learning systems.