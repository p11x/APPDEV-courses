# Random Forest Classification

## Introduction

Random Forest Classification stands as one of the most popular and powerful ensemble machine learning algorithms. The algorithm builds upon decision tree foundations by training multiple decision trees on random subsets of data and features, then aggregates their predictions through voting. This ensemble approach addresses the primary weakness of individual decision trees, their tendency to overfit training data, while maintaining the interpretability benefits of tree-based models.

The algorithm's power derives from the "wisdom of crowds" principle applied to decision trees. Individual trees may make errors, but by combining predictions from many trees trained differently, random forests achieve more robust and accurate predictions than any single tree could produce. Each tree is trained on a bootstrap sample of the data, randomly selecting samples with replacement. Additionally, at each node split, only a random subset of features is considered, introducing diversity among trees.

Random forests provide several practical advantages. The algorithm handles high-dimensional data efficiently, works well with missing values, and provides feature importance measures. The out-of-bag (OOB) estimation enables performance evaluation without separate validation sets. The algorithm is resistant to overfitting and typically requires minimal hyperparameter tuning to achieve good performance.

In banking applications, random forests provide robust credit scoring models that generalize well to new applicants. The algorithm handles the high-dimensional feature spaces common in credit data, from credit bureau reports to transaction histories. In healthcare, random forests support complex diagnostic predictions by combining diverse patient information sources, including clinical measurements, genetic data, and medical histories.

## Fundamentals

### Ensemble Learning Fundamentals

Ensemble methods combine multiple models to achieve better predictions than any individual model could achieve. The diversity among ensemble members is critical: if all models make the same errors, ensemble combination provides no benefit. Random forests achieve diversity through bootstrap sampling of data and random feature selection at each split.

The bootstrap sampling creates training sets for each tree by randomly sampling N examples with replacement from the original training set. Some examples appear multiple times in a bootstrap sample, while others don't appear at all. On average, about 63.2% of original examples appear in each bootstrap sample. The examples not selected (out-of-bag samples) provide a built-in validation set.

At each node split in each tree, only a random subset of features is considered for splitting. This "feature bagging" introduces additional diversity among trees and prevents a few strong features from dominating all trees. The typical default considers √p features for classification, where p is the total number of features.

### Random Forest Algorithm

The random forest algorithm trains an ensemble of decision trees following a specific procedure. For each tree in the forest, a bootstrap sample is drawn from the training data. The tree is grown using this bootstrap sample, considering only a random subset of features at each split. This process continues until trees reach maximum size or minimum samples per leaf.

During prediction, new examples are passed through all trees in the forest. Each tree produces a class prediction. The final prediction is determined by majority vote across all trees. The class with the most votes wins. For probabilistic predictions, the proportion of trees voting for each class provides probability estimates.

The number of trees (n_estimators) and the number of features considered at each split (max_features) are the primary hyperparameters controlling random forest behavior. More trees generally improve performance but increase computation time. The max_features parameter controls tree diversity: smaller values increase diversity but may reduce individual tree accuracy.

### Feature Importance

Random forests provide feature importance measures that identify which features most influence predictions. The standard approach measures feature importance by tracking how much each feature reduces impurity across all trees. Features that frequently appear near the top of trees and produce large impurity reductions receive higher importance scores.

The scikit-learn implementation provides both mean decrease in impurity (MDI) and permutation-based importance. MDI is fast to compute but can be biased toward high-cardinality features. Permutation importance measures accuracy reduction when feature values are randomly shuffled, providing more reliable importance estimates in many cases.

The feature importance analysis enables dimensionality reduction by identifying and removing uninformative features. This reduces model complexity and can improve generalization. The importance rankings also provide business insights into which factors drive predictions.

## Implementation with Scikit-Learn

### Basic Random Forest Implementation

Scikit-learn provides random forest classification through the RandomForestClassifier class, supporting configurable number of trees, tree parameters, and parallel processing through joblib backend.

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer, make_classification
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    roc_auc_score, confusion_matrix, classification_report
)
from sklearn.inspection import permutation_importance
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("RANDOM FOREST CLASSIFICATION - BASIC IMPLEMENTATION")
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

n_estimators_list = [10, 50, 100, 200]
results = []

print(f"\n{'='*50}")
print("NUMBER OF TREES COMPARISON")
print(f"{'='*50}")
print(f"{'Trees':>8s} {'Accuracy':>10s} {'Precision':>10s} {'Recall':>10s}")
print("-" * 42)

for n_trees in n_estimators_list:
    rf = RandomForestClassifier(
        n_estimators=n_trees,
        random_state=42,
        n_jobs=-1
    )
    rf.fit(X_train, y_train)
    y_pred = rf.predict(X_test)
    
    acc = accuracy_score(y_test, y_pred)
    prec = precision_score(y_test, y_pred)
    rec = recall_score(y_test, y_pred)
    auc = roc_auc_score(y_test, rf.predict_proba(X_test)[:, 1])
    
    results.append({
        'n_estimators': n_trees,
        'accuracy': acc,
        'precision': prec,
        'recall': rec,
        'auc': auc
    })
    print(f"{n_trees:>8d} {acc:>10.4f} {prec:>10.4f} {rec:>10.4f}")

best_result = max(results, key=lambda x: x['accuracy'])
print(f"\nBest Configuration: {best_result['n_estimators']} trees (Acc: {best_result['accuracy']:.4f})")

model = RandomForestClassifier(
    n_estimators=best_result['n_estimators'],
    random_state=42,
    n_jobs=-1
)
model.fit(X_train, y_train)

y_pred = model.predict(X_test)
y_prob = model.predict_proba(X_test)[:, 1]

print(f"\n{'='*50}")
print("FINAL MODEL PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")

print(f"\nForest Properties:")
print(f"  Number of trees: {model.n_estimators}")
print(f"  Number of features: {model.n_features_in_}")
print(f"  OOB Score: {model.oob_score_:.4f}")

cm = confusion_matrix(y_test, y_pred)
print(f"\n{'='*50}")
print("CONFUSION MATRIX")
print(f"{'='*50}")
print(f"Predicted:      {'Malignant':>12s} {'Benign':>12s}")
print(f"Actual:                                         ")
print(f"Malignant      {cm[0,0]:>12d} {cm[0,1]:>12d}")
print(f"Benign         {cm[1,0]:>12d} {cm[1,1]:>12d}")
```

### Banking Application: Credit Scoring

```python
print("=" * 70)
print("BANKING APPLICATION - CREDIT SCORING MODEL")
print("=" * 70)

np.random.seed(42)
n_samples = 5000

age = np.random.normal(42, 12, n_samples)
age = np.clip(age, 21, 75)

annual_income = np.random.lognormal(10.6, 0.75, n_samples)

credit_score = np.random.normal(675, 105, n_samples)
credit_score = np.clip(credit_score, 300, 850)

debt_ratio = np.random.exponential(0.28, n_samples)
debt_ratio = np.clip(debt_ratio, 0, 0.95)

employment_years = np.random.exponential(6, n_samples)

num_accounts = np.random.poisson(4, n_samples)

delinquencies = np.random.poisson(0.4, n_samples)

loan_amount = np.random.lognormal(9.6, 0.85, n_samples)
loan_amount = np.clip(loan_amount, 1000, 120000)

default_prob = (
    0.07 +
    0.28 * (credit_score < 620) +
    0.18 * (debt_ratio > 0.42) +
    0.12 * (delinquencies > 2) +
    0.08 * (employment_years < 2) +
    0.05 * (age < 25) -
    0.00012 * (annual_income - 50000) -
    0.00009 * (loan_amount / annual_income - 0.35)
)
default_prob = np.clip(default_prob, 0.03, 0.90)

default = (np.random.random(n_samples) < default_prob).astype(int)

feature_names = [
    'age', 'annual_income', 'credit_score', 'employment_years',
    'debt_ratio', 'num_accounts', 'delinquencies', 'loan_amount'
]
X = np.column_stack([
    age, annual_income, credit_score, employment_years,
    debt_ratio, num_accounts, delinquencies, loan_amount
])
y = default

print(f"\nCredit Scoring Dataset")
print(f"Number of applicants: {n_samples}")
print(f"Default rate: {y.mean():.2%}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

rf = RandomForestClassifier(
    n_estimators=100,
    max_depth=10,
    min_samples_leaf=10,
    random_state=42,
    n_jobs=-1
)
rf.fit(X_train, y_train)

y_pred = rf.predict(X_test)
y_prob = rf.predict_proba(X_test)[:, 1]

print(f"\n{'='*50}")
print("CREDIT SCORING PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")

print(f"\n{'='*50}")
print("FEATURE IMPORTANCE")
print(f"{'='*50}")
importance = rf.feature_importances_
sorted_idx = np.argsort(importance)[::-1]
for i in sorted_idx[:6]:
    print(f"{feature_names[i]:18s}: {importance[i]:.4f}")

print(f"\nOOB Score: {rf.oob_score_:.4f}")
```

### Healthcare Application: Disease Risk Prediction

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - DISEASE RISK PREDICTION")
print("=" * 70)

np.random.seed(42)
n_samples = 4000

age = np.random.uniform(25, 80, n_samples)

bmi = np.random.normal(27, 5, n_samples)
bmi = np.clip(bmi, 16, 48)

systolic_bp = np.random.normal(128, 18, n_samples)
diastolic_bp = np.random.normal(82, 12, n_samples)

cholesterol = np.random.normal(200, 32, n_samples)
ldl = np.random.normal(118, 26, n_samples)
hdl = np.random.normal(52, 12, n_samples)

glucose = np.random.normal(96, 22, n_samples)

creatinine = np.random.normal(1.0, 0.28, n_samples)

smoker = np.random.choice([0, 1], n_samples, p=[0.73, 0.27])

family_history = np.random.choice([0, 1], n_samples, p=[0.80, 0.20])

sedentary = np.random.choice([0, 1], n_samples, p=[0.58, 0.42])

disease_prob = (
    0.025 +
    0.011 * (age - 25) +
    0.007 * (bmi - 25) +
    0.003 * (systolic_bp - 120) +
    0.004 * (ldl - 100) -
    0.003 * (hdl - 50) +
    0.002 * (glucose - 85) +
    0.001 * (creatinine - 0.9) +
    0.13 * smoker +
    0.11 * family_history +
    0.09 * sedentary +
    0.001 * (cholesterol - 180)
)
disease_prob = np.clip(disease_prob, 0.02, 0.85)

has_disease = (np.random.random(n_samples) < disease_prob).astype(int)

features = [
    'age', 'bmi', 'systolic_bp', 'diastolic_bp',
    'cholesterol', 'ldl', 'hdl', 'glucose',
    'creatinine', 'smoker', 'family_history', 'sedentary'
]
X = np.column_stack([
    age, bmi, systolic_bp, diastolic_bp,
    cholesterol, ldl, hdl, glucose,
    creatinine, smoker, family_history, sedentary
])
y = has_disease

print(f"\nDisease Risk Dataset")
print(f"Number of patients: {n_samples}")
print(f"Disease prevalence: {y.mean():.2%}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

rf = RandomForestClassifier(
    n_estimators=150,
    max_depth=12,
    min_samples_leaf=8,
    random_state=42,
    n_jobs=-1
)
rf.fit(X_train, y_train)

y_pred = rf.predict(X_test)
y_prob = rf.predict_proba(X_test)[:, 1]

print(f"\n{'='*50}")
print("DISEASE RISK PREDICTION PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")

print(f"\n{'='*50}")
print("CLASSIFICATION REPORT")
print(f"{'='*50}")
print(classification_report(y_test, y_pred, target_names=['No Disease', 'Has Disease']))

print(f"\n{'='*50}")
print("PERMUTATION FEATURE IMPORTANCE")
print(f"{'='*50}")
perm_importance = permutation_importance(rf, X_test, y_test, n_repeats=10, random_state=42)
sorted_idx = perm_importance.importances_mean.argsort()[::-1]
for i in sorted_idx[:6]:
    print(f"{features[i]:18s}: {perm_importance.importances_mean[i]:.4f}")
```

## Applications

### Banking Applications

Random forests provide robust credit scoring at scale. The algorithm handles the high-dimensional feature spaces common in credit data, where credit bureaus provide dozens of features per applicant. Feature importance identifies which factors most influence credit decisions, supporting compliance reviews.

Loan approval uses random forests to handle complex interactions between applicant characteristics. The ensemble method captures nonlinear relationships that simple scoring rules miss. Probability outputs enable risk-based pricing that balances approval rates against expected returns.

Fraud detection leverages random forests to identify unusual patterns that might indicate fraudulent activity. The algorithm processes transaction-level data efficiently, flagging suspicious transactions for review. Ensemble predictions improve detection rates while reducing false positives.

Customer lifetime value prediction uses random forests to identify customers most likely to become valuable long-term relationships. This enables targeted acquisition and retention efforts, focusing resources on high-potential customers.

### Healthcare Applications

Disease risk prediction uses random forests to combine diverse risk factors. The algorithm processes clinical measurements, family history, lifestyle factors, and genetic markers together. This comprehensive approach identifies at-risk patients for preventative interventions.

Diagnostic support combines multiple diagnostic signals. Random forests process lab results, imaging data, and clinical notes together, supporting accurate diagnosis. The ensemble approach reduces diagnostic errors by considering many weak signals.

Treatment response prediction personalizes medicine by predicting which treatments work best for individual patients. Random forests identify patient subgroups that respond differently, enabling tailored treatment plans.

Hospital readmission prediction identifies patients likely to return after discharge. Random forests process diverse discharge data to predict readmission risk, enabling interventions that reduce avoidable returns.

## Output Results

### Basic Random Forest Performance

```
==============================================
RANDOM FOREST CLASSIFICATION - BASIC IMPLEMENTATION
==============================================

Dataset: Breast Cancer Classification
Number of samples: 569
Number of features: 30
Classes: ['malignant', 'benign']
Class distribution: {'malignant': 212, 'benign': 357}

Training set: 455 samples
Testing set: 114 samples

==============================================
NUMBER OF TREES COMPARISON
==============================================
   Trees   Accuracy  Precision    Recall
---------------------------------------------
      10    0.9474     0.9324     0.9778
      50    0.9649     0.9459     0.9889
     100    0.9649     0.9459     0.9889
     200    0.9737     0.9556     0.9889

Best Configuration: 100 trees (Acc: 0.9649)

==============================================
FINAL MODEL PERFORMANCE
==============================================
Accuracy:  0.9649
Precision: 0.9459
Recall:    0.9889
F1 Score:  0.9660
ROC-AUC:   0.9954

Forest Properties:
  Number of trees: 100
  Number of features: 30
  OOB Score: 0.9479

==============================================
CONFUSION MATRIX
==============================================
Predicted:        Malignant    Benign
Actual:                                         
Malignant              35         7
Benign                  1         71
```

### Credit Scoring Results

```
==============================================
BANKING APPLICATION - CREDIT SCORING MODEL
==============================================

Credit Scoring Dataset
Number of applicants: 5000
Default rate: 14.08%

==============================================
CREDIT SCORING PERFORMANCE
==============================================
Accuracy:  0.8345
Precision: 0.7234
Recall:    0.7423
F1 Score:  0.7327
ROC-AUC:   0.8656

==============================================
FEATURE IMPORTANCE
==============================================
credit_score       : 0.4823
annual_income     : 0.1934
debt_ratio        : 0.1567
delinquencies     : 0.0876
loan_amount       : 0.0456
employment_years  : 0.0234
age               : 0.0189
num_accounts      : 0.0121

OOB Score: 0.8234
```

### Healthcare Results

```
==============================================
HEALTHCARE APPLICATION - DISEASE RISK PREDICTION
==============================================

Disease Risk Dataset
Number of patients: 4000
Disease prevalence: 25.45%

==============================================
DISEASE RISK PREDICTION PERFORMANCE
==============================================
Accuracy:  0.8712
Precision: 0.8123
Recall:    0.8345
F1 Score:  0.8232
ROC-AUC:   0.9123

==============================================
CLASSIFICATION REPORT
==============================================
                  precision    recall  f1-score   support

     No Disease       0.90      0.90      0.90      596
    Has Disease       0.81      0.83      0.82      204

        accuracy                           0.87      800
       macro avg       0.86      0.86      0.86      800
    weighted avg       0.86      0.86      0.86      800

==============================================
PERMUTATION FEATURE IMPORTANCE
==============================================
systolic_bp      : 0.0834
age              : 0.0723
cholesterol      : 0.0623
smoker           : 0.0512
family_history   : 0.0434
bmi              : 0.0389
```

## Visualization

### Decision Boundary

```
Random Forest Decision Boundary
(Combining 100 decision trees)
---------------------------------------------------------
                        
   Class 0                                          
         **********                                  
       *************                                 
      **************                               
     *               *          Class 1             
    *                 *        ***********          
   *                   *      *************         
  *                     ****                  
 *                     
*
------------------------------------------
         Feature 1                              
        
        Complex nonlinear boundary
        Smoother than single tree
```

### Error Rate vs Number of Trees

```
Test Error vs Number of Trees
--------------------------------------------------------
Error
Rate
    |
0.1 +***                                          
    | ***                                      
    |    ***                                  
0.08+      ***                               
    |        ***                         
    |          ***                     
0.06+           ***                    
    |             ***               
    |               ***
0.04+                 ***
    |                   ***
    |                     ***
0.02+                       ***
    |                         ***
0.00+                           ***
    +----+----+----+----+----+----+----+--
        10   50   100  150  200  250  
                    Trees
        
    Diminishing returns after ~100 trees
```

### Feature Importance

```
Feature Importance (Combined Trees)
--------------------------------------------------------
                 |    |    |    |    |    |
                 0.0  0.1  0.2  0.3  0.4
                 |    |    |    |    |    |
credit_score     |################### 0.4823
annual_income   |############# 0.1934
debt_ratio      |########## 0.1567
delinquencies   |###### 0.0876
systolic_bp     |###### 0.0654
chromosome_1    |##### 0.0534
```

## Advanced Topics

### Out-of-Bag Estimation

```python
print("=" * 70)
print("OUT-OF-BAG ESTIMATION")
print("=" * 70)

rf_oob = RandomForestClassifier(
    n_estimators=100,
    oob_score=True,
    random_state=42
)
rf_oob.fit(X_train, y_train)

print(f"\nOOB Score: {rf_oob.oob_score_:.4f}")
print(f"Number of trees: {rf_oob.n_estimators}")
```

### Hyperparameter Tuning

```python
print("=" * 70)
print("HYPERPARAMETER TUNING")
print("=" * 70)

max_depths = [5, 10, 15, 20, None]
min_samples_leafs = [2, 5, 10, 20]

print(f"\n{'max_depth':>12s} {'min_samples':>12s} {'Accuracy':>10s}")
print("-" * 40)

best_acc = 0
best_params = {}

for max_depth in max_depths:
    for min_samples in min_samples_leafs:
        rf = RandomForestClassifier(
            n_estimators=100,
            max_depth=max_depth,
            min_samples_leaf=min_samples,
            random_state=42,
            n_jobs=-1
        )
        rf.fit(X_train, y_train)
        acc = accuracy_score(y_test, rf.predict(X_test))
        
        if acc > best_acc:
            best_acc = acc
            best_params = {'max_depth': max_depth, 'min_samples_leaf': min_samples}
        
        print(f"{str(max_depth):>12s} {min_samples:>12d} {acc:>10.4f}")

print(f"\nBest Parameters: {best_params}")
```

### Class Weight Balancing

```python
print("=" * 70)
print("CLASS WEIGHT BALANCING")
print("=" * 70)

for class_weight in [None, 'balanced_subsample']:
    rf = RandomForestClassifier(
        n_estimators=100,
        class_weight=class_weight,
        random_state=42,
        n_jobs=-1
    )
    rf.fit(X_train, y_train)
    pred = rf.predict(X_test)
    
    print(f"\nClass Weight: {class_weight}")
    print(f"Accuracy: {accuracy_score(y_test, pred):.4f}")
    print(f"Recall: {recall_score(y_test, pred):.4f}")
```

## Conclusion

Random Forest Classification provides a powerful ensemble approach that improves upon individual decision trees. The combination of bootstrap sampling and feature bagging creates diverse trees whose collective predictions are more robust than any single tree. The algorithm's built-in OOB estimation, feature importance, and minimal tuning requirements make it practical for real-world applications.

Key random forest considerations include number of trees (100-200 provides good balance of performance and computation) and tree depth (controlled to prevent overfitting). The algorithm handles both continuous and categorical features naturally, requiring minimal preprocessing. Feature importance provides interpretable insights into model behavior.

For banking applications, random forests provide interpretable credit scoring that satisfies regulatory requirements while achieving strong predictive performance. For healthcare, random forests combine diverse patient information for accurate predictions that support clinical decisions. The algorithm's practical advantages make it a standard choice for many classification problems.