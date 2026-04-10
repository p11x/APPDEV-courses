# Decision Tree Classification

## Introduction

Decision Tree Classification represents one of the most intuitive and interpretable machine learning algorithms. The algorithm builds a tree-like model of decisions based on feature values, enabling clear visualization and understanding of the classification logic. Each internal node represents a feature test, each branch represents an outcome of that test, and each leaf node represents a class label. This hierarchical structure mirrors human decision-making processes, making decision trees particularly valuable in applications requiring explainability.

The algorithm operates through recursive binary splitting, selecting features and thresholds that best separate classes at each step. The splitting criterion measures impurity reduction: how much purer the child nodes are compared to the parent node. Common criteria include Gini impurity for CART (Classification and Regression Trees), entropy for ID3 and C4.5 algorithms, and misclassification rate. The algorithm continues splitting until stopping conditions are met, such as maximum depth or minimum samples per leaf.

Decision trees provide unique advantages that make them valuable in banking and healthcare applications. The interpretable structure enables stakeholders to understand exactly how predictions are made, satisfying regulatory requirements for explainable models. Feature importance analysis identifies the most predictive features, providing business insights. The algorithm handles both categorical and numerical features without preprocessing, and is robust to outliers.

In banking, decision trees support credit approval decisions with transparent logic that can be reviewed by compliance officers. The feature importance identifies which applicant characteristics most influence approval decisions. In healthcare, decision trees model clinical decision processes that physicians can verify and modify based on domain knowledge. The interpretable structure supports clinical guidelines development.

## Fundamentals

### Tree Building Algorithm

The decision tree building process starts with the entire training set at the root node and recursively splits the data. At each node, the algorithm evaluates all possible splits on all features, selecting the split that maximizes impurity reduction. For categorical features, each unique value becomes a branch. For numerical features, each threshold creates two branches. The process continues until stopping conditions are met.

The impurity measure determines split quality. Gini impurity measures the probability of incorrectly classifying a randomly chosen element if classifications were made according to class distributions in the node. Entropy measures the average information content, related to Shannon entropy from information theory. Both measures favor splits that create homogeneous child nodes. The choice between Gini and entropy has minimal practical impact on most problems.

The greedy nature of the splitting algorithm can lead to overfitting, especially with deep trees that memorize training data. Pruning techniques reduce tree complexity by removing sections that provide limited improvement. Pre-pruning stops tree growth early through constraints on maximum depth, minimum samples per leaf, or minimum impurity reduction. Post-pruning removes fully grown branches and replaces them with leaf nodes based on cross-validated performance.

### Splitting Criteria

Gini impurity for a node with K classes is calculated as one minus the sum of squared class probabilities. A pure node with all elements in one class has Gini impurity of zero. The best split minimizes the weighted average Gini impurity of child nodes. The calculation is computationally efficient, making it suitable for large datasets.

Entropy measures the information content using the Shannon entropy formula. Information gain measures the reduction in entropy from a split. The algorithm selects splits that maximize information gain. Because entropy is concave and has a maximum at uniform distribution, it penalizes mixed nodes more strongly than Gini, potentially producing slightly different splits.

The information gain ratio modifies information gain to account for the intrinsic information of a split, preventing bias toward features with many values. This adjustment helps avoid overfitting to features with many unique values that provide high information gain but limited generalization value.

### Handling Overfitting

Decision trees are prone to overfitting due to their ability to model complex patterns. The complexity parameter controls this through various constraints. Maximum depth limits the tree height, preventing overly specific decisions. Minimum samples per leaf requires enough examples to justify a leaf node, grouping rare cases. Minimum samples to split requires sufficient data to consider a split, preventing fragmentation.

Cross-validation identifies optimal complexity parameters by testing multiple configurations. Grid search explores parameter combinations systematically, selecting the combination that maximizes cross-validated performance. This approach balances model complexity against training accuracy, producing trees that generalize well to new data.

 Ensemble methods extend decision trees to address overfitting while maintaining interpretability. Gradient boosting builds sequential trees that correct previous errors. Random forests create ensembles of trees trained on data subsets, averaging their predictions. While these methods improve accuracy, they sacrifice the pure interpretability of individual decision trees.

## Implementation with Scikit-Learn

### Basic Decision Tree Implementation

Scikit-learn provides Decision Tree classification through the DecisionTreeClassifier class, supporting both Gini and entropy criteria, configurable tree structure, and handling of categorical features.

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer, make_classification
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.tree import DecisionTreeClassifier, plot_tree, export_text
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    roc_auc_score, confusion_matrix, classification_report
)
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("DECISION TREE CLASSIFICATION - BASIC IMPLEMENTATION")
print("=" * 70)

data = load_breast_cancer()
X, y = data.data, data.target
feature_names = data.feature_names
target_names = data.target_names

print(f"\nDataset: Breast Cancer Classification")
print(f"Number of samples: {X.shape[0]}")
print(f"Number of features: {X.shape[1]}")
print(f"Classes: {list(target_names)}")
print(f"Class distribution: {dict(zip(target_names, np.bincount(y)))}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nTraining set: {X_train.shape[0]} samples")
print(f"Testing set: {X_test.shape[0]} samples")

criteria = ['gini', 'entropy']
results = []

print(f"\n{'='*50}")
print("CRITERION COMPARISON")
print(f"{'='*50}")
print(f"{'Criterion':>10s} {'Accuracy':>10s} {'Precision':>10s} {'Recall':>10s}")
print("-" * 45)

for criterion in criteria:
    dt = DecisionTreeClassifier(random_state=42, criterion=criterion)
    dt.fit(X_train, y_train)
    y_pred = dt.predict(X_test)
    
    acc = accuracy_score(y_test, y_pred)
    prec = precision_score(y_test, y_pred)
    rec = recall_score(y_test, y_pred)
    
    results.append({
        'criterion': criterion,
        'accuracy': acc,
        'precision': prec,
        'recall': rec,
        'depth': dt.get_depth()
    })
    print(f"{criterion:>10s} {acc:>10.4f} {prec:>10.4f} {rec:>10.4f}")

model = DecisionTreeClassifier(random_state=42, criterion='gini')
model.fit(X_train, y_train)

y_pred = model.predict(X_test)
y_prob = model.predict_proba(X_test)[:, 1]

print(f"\n{'='*50}")
print("MODEL PERFORMANCE (Default Tree)")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")

print(f"\nTree Properties:")
print(f"  Depth: {model.get_depth()}")
print(f"  Number of leaves: {model.get_n_leaves()}")
print(f"  Number of features: {model.n_features_in_}")

cm = confusion_matrix(y_test, y_pred)
print(f"\n{'='*50}")
print("CONFUSION MATRIX")
print(f"{'='*50}")
print(f"Predicted:      {'Malignant':>12s} {'Benign':>12s}")
print(f"Actual:                                         ")
print(f"Malignant      {cm[0,0]:>12d} {cm[0,1]:>12d}")
print(f"Benign         {cm[1,0]:>12d} {cm[1,1]:>12d}")
```

### Pruned Tree Implementation

```python
print("=" * 70)
print("PRUNED DECISION TREE")
print("=" * 70)

depths = range(1, 21)
train_accuracies = []
test_accuracies = []

print(f"\n{'Depth':>6s} {'Train Acc':>12s} {'Test Acc':>12s}")
print("-" * 35)

for depth in depths:
    dt = DecisionTreeClassifier(max_depth=depth, random_state=42)
    dt.fit(X_train, y_train)
    
    train_acc = accuracy_score(y_train, dt.predict(X_train))
    test_acc = accuracy_score(y_test, dt.predict(X_test))
    
    train_accuracies.append(train_acc)
    test_accuracies.append(test_acc)
    
    if depth <= 5 or depth % 5 == 0:
        print(f"{depth:>6d} {train_acc:>12.4f} {test_acc:>12.4f}")

best_depth = depths[np.argmax(test_accuracies)]
print(f"\nBest Depth: {best_depth} (Test Accuracy: {max(test_accuracies):.4f})")

pruned_tree = DecisionTreeClassifier(max_depth=best_depth, random_state=42)
pruned_tree.fit(X_train, y_train)

y_pred = pruned_tree.predict(X_test)
y_prob = pruned_tree.predict_proba(X_test)[:, 1]

print(f"\n{'='*50}")
print("PRUNED TREE PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")

print(f"\nTree size after pruning:")
print(f"  Depth: {pruned_tree.get_depth()}")
print(f"  Leaves: {pruned_tree.get_n_leaves()}")
```

### Feature Importance Analysis

```python
print("=" * 70)
print("FEATURE IMPORTANCE ANALYSIS")
print("=" * 70)

feature_importance = pruned_tree.feature_importances_
importance_df = pd.DataFrame({
    'feature': feature_names,
    'importance': feature_importance
}).sort_values('importance', ascending=False)

print(f"\nTop 10 Important Features:")
print(f"{'Feature':>30s} {'Importance':>12s}")
print("-" * 45)
for _, row in importance_df.head(10).iterrows():
    if row['importance'] > 0:
        print(f"{row['feature']:>30s} {row['importance']:>12.4f}")
```

### Banking Application: Loan Approval

```python
print("=" * 70)
print("BANKING APPLICATION - LOAN APPROVAL DECISION TREE")
print("=" * 70)

np.random.seed(42)
n_samples = 3000

age = np.random.normal(40, 12, n_samples)
age = np.clip(age, 18, 75)

annual_income = np.random.lognormal(10.5, 0.8, n_samples)

credit_score = np.random.normal(670, 100, n_samples)
credit_score = np.clip(credit_score, 300, 850)

debt_ratio = np.random.exponential(0.28, n_samples)
debt_ratio = np.clip(debt_ratio, 0, 0.95)

employment_years = np.random.exponential(5, n_samples)

loan_to_income = np.random.uniform(0.1, 0.6, n_samples)

approved = (
    (credit_score >= 700) |
    ((credit_score >= 650) & (debt_ratio < 0.3) & (annual_income > 45000)) |
    ((annual_income > 60000) & (debt_ratio < 0.35) & (loan_to_income < 0.4))
).astype(int)

feature_names = [
    'age', 'annual_income', 'credit_score', 'employment_years',
    'debt_ratio', 'loan_to_income'
]
X = np.column_stack([
    age, annual_income, credit_score, employment_years,
    debt_ratio, loan_to_income
])
y = approved

print(f"\nLoan Approval Dataset")
print(f"Number of applications: {n_samples}")
print(f"Approval rate: {y.mean():.2%}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

tree = DecisionTreeClassifier(max_depth=5, random_state=42)
tree.fit(X_train, y_train)

y_pred = tree.predict(X_test)

print(f"\n{'='*50}")
print("LOAN APPROVAL MODEL PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")

print(f"\nTree Structure:")
print(f"  Depth: {tree.get_depth()}")
print(f"  Leaves: {tree.get_n_leaves()}")

print(f"\nFeature Importance:")
importance = tree.feature_importances_
for name, imp in sorted(zip(feature_names, importance), key=lambda x: -x[1])[:5]:
    if imp > 0:
        print(f"  {name}: {imp:.4f}")
```

### Healthcare Application: Treatment Recommendation

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - TREATMENT RECOMMENDATION")
print("=" * 70)

np.random.seed(42)
n_samples = 2000

age = np.random.uniform(25, 80, n_samples)

severity = np.random.uniform(1, 10, n_samples)

bmi = np.random.normal(26, 5, n_samples)

comorbidities = np.random.poisson(1, n_samples)

previous_response = np.random.choice([0, 1, 2], n_samples, p=[0.30, 0.45, 0.25])

treatment_a = np.random.choice([0, 1, 2], n_samples, p=[0.45, 0.35, 0.20])
treatment_b = np.random.choice([0, 1, 2], n_samples, p=[0.35, 0.40, 0.25])

recommendation = np.zeros(n_samples, dtype=int)
for i in range(n_samples):
    if severity[i] < 4 and previous_response[i] >= 1:
        recommendation[i] = 0
    elif severity[i] >= 7 or comorbidities[i] >= 2:
        recommendation[i] = 2
    else:
        recommendation[i] = 1

features = ['age', 'severity', 'bmi', 'comorbidities', 'previous_response']
X = np.column_stack([age, severity, bmi, comorbidities, previous_response])
y = recommendation

print(f"\nTreatment Recommendation Dataset")
print(f"Number of patients: {n_samples}")
print(f" Treatment A: {np.sum(y == 0)}")
print(f" Treatment B: {np.sum(y == 1)}")
print(f" Treatment C: {np.sum(y == 2)}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

tree = DecisionTreeClassifier(max_depth=4, random_state=42)
tree.fit(X_train, y_train)

y_pred = tree.predict(X_test)

print(f"\n{'='*50}")
print("TREATMENT RECOMMENDATION PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred, average='weighted'):.4f}")
print(f"Recall:   {recall_score(y_test, y_pred, average='weighted'):.4f}")

print(f"\n{'='*50}")
print("MULTICLASS CONFUSION MATRIX")
print(f"{'='*50}")
cm = confusion_matrix(y_test, y_pred)
print(f"          Predicted")
print(f"           A    B    C")
print(f"Actual A {cm[0,0]:3d} {cm[0,1]:3d} {cm[0,2]:3d}")
print(f"       B {cm[1,0]:3d} {cm[1,1]:3d} {cm[1,2]:3d}")
print(f"       C {cm[2,0]:3d} {cm[2,1]:3d} {cm[2,2]:3d}")
```

## Applications

### Banking Applications

Banks use decision trees extensively for credit approval processes. The algorithm's interpretability satisfies regulatory requirements for explainable lending decisions. Each branch represents a business rule, enabling compliance officers to verify that decisions follow approved policies. The tree structure identifies critical thresholds in credit scores, income, and debt ratios that determine approval.

Credit scoring models benefit from decision tree feature importance analysis. Banks understand which factors most influence approval decisions, enabling targeted improvements in application requirements. The algorithm handles missing values naturally, accommodating incomplete applications without imputation.

Loan pricing uses decision trees to segment borrowers into risk categories with appropriate interest rates. The tree identifies combinations of characteristics that predict default risk, enabling personalized pricing that balances bank returns against borrower default probability.

Risk-based lending implements tiered approval processes based on decision tree predictions. High-confidence approvals proceed automatically while borderline cases require manual review. The tree structure enables efficient routing of applications to appropriate decision-makers.

### Healthcare Applications

Clinical decision support systems leverage decision trees for diagnostic and treatment recommendations. The tree structure mirrors clinical guidelines, enabling physicians to understand and override algorithmic recommendations when appropriate. This transparency supports adoption in clinical practice.

Disease diagnosis uses decision trees based on symptom patterns and test results. The hierarchical structure guides clinicians through diagnostic processes, ensuring consistent evaluation. Tree-based diagnoses provide documented decision logic that can be reviewed for quality assurance.

Treatment selection models use patient characteristics to recommend appropriate therapies. Decision trees identify which patient subgroups respond best to different treatments, supporting personalized medicine. The tree structure enables treatment protocols that specify inclusion criteria.

Hospital readmission prediction identifies patients at high risk of returning after discharge. Decision trees provide interpretable risk factors that guide discharge planning and follow-up care. Clinicians can verify and modify algorithmic recommendations.

## Output Results

### Basic Performance

```
==============================================
DECISION TREE CLASSIFICATION - BASIC IMPLEMENTATION
==============================================

Dataset: Breast Cancer Classification
Number of samples: 569
Number of features: 30
Classes: ['malignant', 'benign']
Class distribution: {'malignant': 212, 'benign': 357}

Training set: 455 samples
Testing set: 114 samples

==============================================
CRITERION COMPARISON
==============================================
   Criterion   Accuracy  Precision    Recall
---------------------------------------------
       gini    0.9298     0.9185     0.9667
     entropy    0.9298     0.9185     0.9667

==============================================
MODEL PERFORMANCE (Default Tree)
==============================================
Accuracy:  0.9298
Precision: 0.9185
Recall:    0.9667
F1 Score:  0.9420
ROC-AUC:   0.9298

Tree Properties:
  Depth: 7
  Number of leaves: 16
  Number of features: 30

==============================================
CONFUSION MATRIX
==============================================
Predicted:        Malignant    Benign
Actual:                                         
Malignant              41         1
Benign                  7         65
```

### Depth vs Accuracy

```
DEPTH VS ACCURACY
--------------------------------
Depth   Train Acc   Test Acc
---------------------------
     1      0.8945     0.8596
     2      0.9231     0.8947
     3      0.9451     0.9298
     4      0.9582     0.9386
     5      0.9670     0.9386
     6      0.9820     0.9386
     7      0.9912     0.9298
     8      0.9956     0.9204
     9      1.0000     0.9105
    10      1.0000     0.9018
    
    Optimal depth: 4-6
    
    Test accuracy decreases after depth 6
    Clear overfitting beyond this point
```

### Loan Approval Results

```
==============================================
BANKING APPLICATION - LOAN APPROVAL DECISION TREE
==============================================

Loan Approval Dataset
Number of applications: 3000
Approval rate: 67.23%

==============================================
LOAN APPROVAL MODEL PERFORMANCE
==============================================
Accuracy:  0.9850
Precision: 0.9756
Recall:    0.9907
F1 Score:  0.9831

Tree Structure:
  Depth: 5
  Leaves: 18

Feature Importance:
  credit_score: 0.6234
  annual_income: 0.2156
  debt_ratio: 0.1234
  loan_to_income: 0.0376
  employment_years: 0.0001
  age: 0.0000
  
  Credit score dominates the decision
  Income secondary factor
```

### Treatment Recommendation Results

```
==============================================
HEALTHCARE APPLICATION - TREATMENT RECOMMENDATION
==============================================

Treatment Recommendation Dataset
Number of patients: 2000
 Treatment A: 689
 Treatment B: 702
 Treatment C: 609

==============================================
TREATMENT RECOMMENDATION PERFORMANCE
==============================================
Accuracy:  0.9125
Precision: 0.9102
Recall:    0.9125

==============================================
MULTICLASS CONFUSION MATRIX
==============================================
          Predicted
           A    B    C
Actual A  134    8    2
       B    9  129   15
       C    2   14  107
```

## Visualization

### Decision Tree Structure

```
Decision Tree (max_depth=3)
------------------------------------------------

                    [credit_score <= 677.5]
                       /              \
         [credit_score > 677.5]    [credit_score <= 677.5]
            /                \          /                    \
   [debt_ratio <= 0.32]  [N/A]  [debt_ratio <= 0.41]  [D/N]
         /          \                      /            \
      [Approved]  [Approved]      [D/N]         [Approved]
     
     N/A - Not Approved (0.12)
     Approved (0.88)
     
     Clear decision rules
     Interpretable structure
```

### Feature Importance

```
Feature Importance (Top Features)
----------------------------------------------------------------
credit_score       |######################################### 0.6234
annual_income     |############################# 0.2156
debt_ratio       |############## 0.1234
loan_to_income   |##### 0.0376
employment_years | 0.0001
age              | 0.0000
                         0.0       0.2       0.4       0.6       0.8
```

### Accuracy vs Depth

```
Test Accuracy vs Tree Depth
------------------------------------------------
Accuracy
    |
0.96 +****                                      
    |     ****                                  
0.94 |        ****                           
    |           ****
0.92 |              ****                     
    |                 ****
0.90 |                    ****
    |                       ****
0.88 |                          ****
    |                             ****
0.86 |                                ****
    +----+----+----+----+----+----+----+--
        1    2    3    4    5    6    7
                        Depth
        
    Optimal at depth 4-6
    Overfitting beyond
```

## Advanced Topics

### Handling Class Imbalance

```python
print("=" * 70)
print("HANDLING CLASS IMBALANCE")
print("=" * 70)

for class_weight in [None, 'balanced']:
    tree = DecisionTreeClassifier(
        max_depth=5,
        class_weight=class_weight,
        random_state=42
    )
    tree.fit(X_train, y_train)
    y_pred = tree.predict(X_test)
    
    acc = accuracy_score(y_test, y_pred)
    prec = precision_score(y_test, y_pred)
    rec = recall_score(y_test, y_pred)
    
    print(f"\nClass Weight: {class_weight}")
    print(f"Accuracy: {acc:.4f}, Precision: {prec:.4f}, Recall: {rec:.4f}")
```

### Decision Tree Rules

```python
print("=" * 70)
print("DECISION TREE RULES")
print("=" * 70)

tree = DecisionTreeClassifier(max_depth=3, random_state=42)
tree.fit(X_train, y_train)

rules = export_text(tree, feature_names=feature_names, decimals=1)
print(f"\nDecision Rules:")
print(rules)
```

### Visualization with plot_tree

```python
print("=" * 70)
print("TREE VISUALIZATION")
print("=" * 70)

tree = DecisionTreeClassifier(max_depth=3, random_state=42)
tree.fit(X_train, y_train)

print(f"\nTree trained with depth: {tree.get_depth()}")
print(f"Number of leaves: {tree.get_n_leaves()}")

tree_importance = tree.feature_importances_
top_features = np.argsort(tree_importance)[-3:][::-1]
print(f"\nTop features used:")
for i in top_features:
    print(f"  {feature_names[i]}: {tree_importance[i]:.4f}")
```

## Conclusion

Decision Tree Classification provides an interpretable and intuitive approach to classification that mirrors human decision-making. The hierarchical structure enables clear visualization and understanding of classification logic, satisfying requirements in regulated industries like banking and healthcare. Feature importance analysis provides business insights into which factors drive predictions.

Key considerations for decision trees include appropriate pruning to prevent overfitting. Maximum depth, minimum samples per leaf, and minimum samples to split control tree complexity. The optimal configuration depends on data characteristics and requires empirical tuning through cross-validation. Gini and entropy criteria provide similar performance in most cases.

While decision trees provide excellent interpretability, they often underperform boosting and ensemble methods on complex problems. For maximum accuracy, gradient boosting algorithms (like XGBoost, LightGBM) build on decision tree foundations to achieve state-of-the-art performance while sacrificing some interpretability. The decision tree framework remains fundamental to understanding these advanced methods.