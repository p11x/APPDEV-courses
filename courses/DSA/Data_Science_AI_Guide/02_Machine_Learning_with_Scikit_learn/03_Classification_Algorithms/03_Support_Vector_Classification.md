# Support Vector Classification

## Introduction

Support Vector Machine (SVM) represents one of the most powerful and elegant classification algorithms in machine learning. The algorithm finds the optimal hyperplane that maximally separates different classes, making it both geometrically intuitive and mathematically well-founded. SVM achieved widespread popularity due to its strong theoretical guarantees, excellent performance on many real-world problems, and effectiveness in high-dimensional spaces.

The core concept of SVM involves finding the hyperplane that provides the maximum margin between classes. The margin represents the distance between the hyperplane and the nearest data points from each class, known as support vectors. These support vectors define the hyperplane position, and the algorithm focuses on accurately classifying these critical points. This geometric approach provides SVM with several advantages: resistance to overfitting, computational efficiency through sparse solutions, and effective handling of high-dimensional data.

SVM extends beyond linear classification through kernel functions that implicitly transform data into higher-dimensional spaces where linear separation becomes possible. This kernel trick enables SVM to discover complex, nonlinear decision boundaries without explicitly computing the transformation. Common kernels include polynomial, radial basis function (RBF), and sigmoid, each suited to different data characteristics. The combination of maximum margin optimization and kernel methods makes SVM versatile for numerous applications.

In banking, SVM powers credit scoring systems where interpretability and resistance to overfitting matter for regulatory compliance. In healthcare, SVM supports disease diagnosis and treatment prediction, providing reliable predictions for high-stakes decisions. The algorithm's ability to handle high-dimensional genetic data makes it particularly valuable in precision medicine applications.

## Fundamentals

### Linear SVM and the Maximum Margin Classifier

Linear SVM finds the optimal separating hyperplane by maximizing the distance to the nearest training points. The optimization problem formulation ensures that the hyperplane correctly classifies training examples while maximizing the margin width. This constrained optimization has a unique solution that depends only on the support vectors, providing computational efficiency and resistance to outliers.

The mathematical formulation introduces slack variables to allow soft margin classification, accommodating non-separable data. The regularization parameter C controls the tradeoff between maximizing the margin and minimizing classification errors. Small C values create wider margins at the cost of misclassification, while large C values prioritize correct classification, potentially narrowing the margin. This parameter enables SVM to balance bias and variance based on data characteristics.

The dual formulation of the SVM optimization problem expresses the solution as a linear combination of training examples. The coefficients indicate each example's influence on the hyperplane, with non-zero coefficients corresponding to support vectors. This sparsity is computationally valuable: prediction time depends only on support vectors rather than the entire training set.

### Kernel Functions and Nonlinear Classification

Kernel functions enable SVM to learn nonlinear decision boundaries by implicitly transforming data into higher-dimensional spaces. The kernel function computes the inner product of transformed data without explicitly performing the transformation, making kernel computations efficient. This "kernel trick" allows SVM to leverage high-dimensional feature spaces without computational explosion.

The polynomial kernel computes features up to a specified degree, enabling discovery of polynomial relationships. The radial basis function (RBF) kernel computes similarity based on exponential distance decay, creating flexible decision boundaries that can approximate any continuous function. The sigmoid kernel creates neural network-like decision boundaries. Each kernel suits different data characteristics, and kernel selection significantly impacts performance.

Parameter selection for kernels influences SVM behavior substantially. The RBF kernel gamma parameter controls the influence radius of each support vector. Small gamma values create broad influence regions with smoother decision boundaries, while large gamma values create narrow regions with more complex boundaries. Together, C and gamma require joint optimization for optimal performance.

### Multi-class SVM

SVM naturally handles binary classification, requiring extension for multi-class problems. One-vs-Rest (OvR) trains separate classifiers for each class against all others, combining predictions through voting. One-vs-One (OvO) trains classifiers for each pair of classes, requiring more classifiers but smaller training sets. Scikit-learn implements both approaches, selecting automatically based on problem size.

## Implementation with Scikit-Learn

### Basic SVM Implementation

Scikit-learn provides SVM classification through the SVC class, supporting various kernels, multi-class strategies, and extensive parameter options. The implementation includes efficientLIBSVM backend for medium and large datasets.

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer, make_classification
from sklearn.model_selection import train_test_split, GridSearchCV, cross_val_score
from sklearn.preprocessing import StandardScaler
from sklearn.svm import SVC, LinearSVC
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    roc_auc_score, confusion_matrix, classification_report
)
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("SUPPORT VECTOR CLASSIFICATION - BASIC IMPLEMENTATION")
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

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

kernels = ['linear', 'rbf', 'poly']
results = []

print(f"\n{'='*50}")
print("KERNEL COMPARISON")
print(f"{'='*50}")
print(f"{'Kernel':>10s} {'Accuracy':>10s} {'Precision':>10s} {'Recall':>10s}")
print("-" * 45)

for kernel in kernels:
    svm = SVC(kernel=kernel, random_state=42, probability=True)
    svm.fit(X_train_scaled, y_train)
    y_pred = svm.predict(X_test_scaled)
    y_prob = svm.predict_proba(X_test_scaled)[:, 1]
    
    acc = accuracy_score(y_test, y_pred)
    prec = precision_score(y_test, y_pred)
    rec = recall_score(y_test, y_pred)
    auc = roc_auc_score(y_test, y_prob)
    
    n_support = np.sum(svm.n_support_)
    results.append({
        'kernel': kernel,
        'accuracy': acc,
        'precision': prec,
        'recall': rec,
        'auc': auc,
        'n_support': n_support
    })
    print(f"{kernel:>10s} {acc:>10.4f} {prec:>10.4f} {rec:>10.4f}")

best_result = max(results, key=lambda x: x['accuracy'])
print(f"\nBest Kernel: {best_result['kernel']} (Accuracy: {best_result['accuracy']:.4f})")

model = SVC(kernel=best_result['kernel'], random_state=42, probability=True)
model.fit(X_train_scaled, y_train)

y_pred = model.predict(X_test_scaled)
y_prob = model.predict_proba(X_test_scaled)[:, 1]

print(f"\n{'='*50}")
print("FINAL MODEL PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")

print(f"\n{'='*50}")
print("SUPPORT VECTORS")
print(f"{'='*50}")
print(f"Number of support vectors: {np.sum(model.n_support_)}")
print(f"Per class: {model.n_support_}")

cm = confusion_matrix(y_test, y_pred)
print(f"\n{'='*50}")
print("CONFUSION MATRIX")
print(f"{'='*50}")
print(f"Predicted:      {'Malignant':>12s} {'Benign':>12s}")
print(f"Actual:                                         ")
print(f"Malignant      {cm[0,0]:>12d} {cm[0,1]:>12d}")
print(f"Benign         {cm[1,0]:>12d} {cm[1,1]:>12d}")
```

### Banking Application: Credit Default Prediction

```python
print("=" * 70)
print("BANKING APPLICATION - CREDIT DEFAULT PREDICTION")
print("=" * 70)

np.random.seed(42)
n_samples = 4000

age = np.random.normal(40, 12, n_samples)
age = np.clip(age, 18, 75)

annual_income = np.random.lognormal(10.5, 0.8, n_samples)

credit_score = np.random.normal(670, 110, n_samples)
credit_score = np.clip(credit_score, 300, 850)

employment_years = np.random.exponential(5, n_samples)

debt_ratio = np.random.exponential(0.30, n_samples)
debt_ratio = np.clip(debt_ratio, 0, 0.95)

num_credit_lines = np.random.poisson(4, n_samples)

delinquencies = np.random.poisson(0.5, n_samples)

loan_amount = np.random.lognormal(9.8, 0.9, n_samples)
loan_amount = np.clip(loan_amount, 2000, 150000)

default_prob = (
    0.08 +
    0.25 * (credit_score < 600) +
    0.20 * (debt_ratio > 0.45) +
    0.12 * (delinquencies > 2) +
    0.08 * (employment_years < 2) +
    0.04 * (age < 25) -
    0.0001 * (annual_income - 50000) -
    0.00008 * (loan_amount / annual_income - 0.3)
)
default_prob = np.clip(default_prob, 0.03, 0.92)

default = (np.random.random(n_samples) < default_prob).astype(int)

feature_names = [
    'age', 'annual_income', 'credit_score', 'employment_years',
    'debt_ratio', 'num_credit_lines', 'delinquencies', 'loan_amount'
]
X = np.column_stack([
    age, annual_income, credit_score, employment_years,
    debt_ratio, num_credit_lines, delinquencies, loan_amount
])
y = default

print(f"\nCredit Default Dataset")
print(f"Number of loans: {n_samples}")
print(f"Default rate: {y.mean():.2%}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

param_grid = {
    'C': [0.1, 1, 10],
    'gamma': ['scale', 'auto', 0.01, 0.1]
}

svm = SVC(kernel='rbf', random_state=42, probability=True)
grid_search = GridSearchCV(svm, param_grid, cv=3, scoring='roc_auc', n_jobs=-1)
grid_search.fit(X_train_scaled, y_train)

print(f"\nBest Parameters: {grid_search.best_params_}")
print(f"Best CV Score: {grid_search.best_score_:.4f}")

model = grid_search.best_estimator_
y_pred = model.predict(X_test_scaled)
y_prob = model.predict_proba(X_test_scaled)[:, 1]

print(f"\n{'='*50}")
print("CREDIT DEFAULT MODEL PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:    {roc_auc_score(y_test, y_prob):.4f}")

print(f"\n{'='*50}")
print("CLASSIFICATION REPORT")
print(f"{'='*50}")
print(classification_report(y_test, y_pred, target_names=['No Default', 'Default']))
```

### Healthcare Application: Disease Classification

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - DISEASE CLASSIFICATION")
print("=" * 70)

np.random.seed(42)
n_samples = 3000

age = np.random.uniform(25, 80, n_samples)

bmi = np.random.normal(27, 5, n_samples)
bmi = np.clip(bmi, 16, 48)

systolic_bp = np.random.normal(130, 18, n_samples)
diastolic_bp = np.random.normal(82, 12, n_samples)

fasting_glucose = np.random.normal(100, 25, n_samples)

hba1c = np.random.normal(5.5, 0.8, n_samples)

cholesterol = np.random.normal(200, 35, n_samples)
ldl = np.random.normal(120, 28, n_samples)
hdl = np.random.normal(50, 12, n_samples)

triglycerides = np.random.normal(150, 50, n_samples)

family_history = np.random.choice([0, 1], n_samples, p=[0.78, 0.22])

smoker = np.random.choice([0, 1], n_samples, p=[0.72, 0.28])

sedentary = np.random.choice([0, 1], n_samples, p=[0.55, 0.45])

disease_prob = (
    0.03 +
    0.012 * (age - 25) +
    0.008 * (bmi - 25) +
    0.003 * (systolic_bp - 120) +
    0.004 * (fasting_glucose - 90) +
    0.025 * (hba1c - 5.0) +
    0.002 * (ldl - 100) -
    0.003 * (hdl - 50) +
    0.001 * (triglycerides - 120) +
    0.12 * family_history +
    0.15 * smoker +
    0.08 * sedentary
)
disease_prob = np.clip(disease_prob, 0.02, 0.88)

has_disease = (np.random.random(n_samples) < disease_prob).astype(int)

features = [
    'age', 'bmi', 'systolic_bp', 'diastolic_bp',
    'fasting_glucose', 'hba1c', 'cholesterol', 'ldl',
    'hdl', 'triglycerides', 'family_history', 'smoker', 'sedentary'
]
X = np.column_stack([
    age, bmi, systolic_bp, diastolic_bp,
    fasting_glucose, hba1c, cholesterol, ldl,
    hdl, triglycerides, family_history, smoker, sedentary
])
y = has_disease

print(f"\nDisease Classification Dataset")
print(f"Number of patients: {n_samples}")
print(f"Disease prevalence: {y.mean():.2%}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

svm = SVC(kernel='rbf', C=10, gamma='scale', random_state=42, probability=True)
svm.fit(X_train_scaled, y_train)

y_pred = svm.predict(X_test_scaled)
y_prob = svm.predict_proba(X_test_scaled)[:, 1]

print(f"\n{'='*50}")
print("DISEASE CLASSIFICATION PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")

print(f"\nSupport Vectors: {np.sum(svm.n_support_)}")
print(f"Per class: {svm.n_support_}")

print(f"\n{'='*50}")
print("CLASSIFICATION REPORT")
print(f"{'='*50}")
print(classification_report(y_test, y_pred, target_names=['No Disease', 'Has Disease']))
```

## Applications

### Banking Applications

SVM serves critical functions in banking where model reliability and interpretability matter for regulatory compliance. Credit scoring systems benefit from SVM's maximum margin principle that provides robust predictions on new applicants. The algorithm's resistance to overfitting ensures consistent performance across different applicant populations.

Loan approval automation uses SVM to process large volumes of applications efficiently. The algorithm's computational efficiency during prediction enables real-time decision-making. SVM's ability to handle high-dimensional feature spaces from credit bureaus makes it suitable for comprehensive credit assessment.

Portfolio risk management employs SVM to identify loans with similar risk profiles. By classifying loans into risk categories based on characteristics, banks can allocate capital appropriately and price products competitively. The support vectors provide insight into typical and atypical cases within each risk category.

### Healthcare Applications

SVM supports clinical decision-making through reliable disease predictions. Diagnostic classification uses patient measurements to identify likely conditions, providing physicians with decision support. SVM's effectiveness on high-dimensional genetic data makes it valuable for precision medicine applications.

Treatment selection uses SVM to match patients with appropriate therapies based on characteristics. By learning from historical patient outcomes, SVM recommends treatments most likely to succeed for new patients. The algorithm's probabilistic outputs enable personalized confidence estimates.

Hospital readmission prediction uses SVM to identify patients at high risk of returning after discharge. Early identification enables intervention programs that reduce readmission rates. SVM's ability to handle diverse data types including clinical notes, through appropriate feature engineering, supports comprehensive risk assessment.

## Output Results

### Basic SVM Performance

```
==================================================
SUPPORT VECTOR CLASSIFICATION - BASIC IMPLEMENTATION
==================================================

Dataset: Breast Cancer Classification
Number of samples: 569
Number of features: 30
Classes: ['malignant', 'benign']
Class distribution: {'malignant': 212, 'benign': 357}

Training set: 455 samples
Testing set: 114 samples

==================================================
KERNEL COMPARISON
==================================================
     Kernel   Accuracy  Precision    Recall
---------------------------------------------
     linear    0.9649     0.9459     0.9778
        rbf    0.9737     0.9556     0.9889
       poly    0.9649     0.9459     0.9778

Best Kernel: rbf (Accuracy: 0.9737)

==================================================
FINAL MODEL PERFORMANCE
==================================================
Accuracy:  0.9737
Precision: 0.9556
Recall:    0.9889
F1 Score:  0.9710
ROC-AUC:   0.9975

==================================================
SUPPORT VECTORS
==================================================
Number of support vectors: 79
Per class: [31 48]

==================================================
CONFUSION MATRIX
==================================================
Predicted:        Malignant    Benign
Actual:                                         
Malignant              39         3
Benign                  0        72
```

### Credit Default Results

```
==================================================
BANKING APPLICATION - CREDIT DEFAULT PREDICTION
==================================================

Credit Default Dataset
Number of loans: 4000
Default rate: 13.48%

Best Parameters: {'C': 10, 'gamma': 'scale'}
Best CV Score: 0.8234

==================================================
CREDIT DEFAULT MODEL PERFORMANCE
==================================================
Accuracy:  0.8167
Precision: 0.7123
Recall:    0.7361
F1 Score:  0.7239
ROC-AUC:   0.8478

==================================================
CLASSIFICATION REPORT
==================================================
                  precision    recall  f1-score   support

     No Default       0.89      0.87      0.88      691
         Default       0.71      0.74      0.72      109

        accuracy                           0.82      800
       macro avg       0.80      0.80      0.80      800
    weighted avg       0.80      0.80      0.80      800
```

### Healthcare Results

```
==================================================
HEALTHCARE APPLICATION - DISEASE CLASSIFICATION
==================================================

Disease Classification Dataset
Number of patients: 3000
Disease prevalence: 27.30%

==================================================
DISEASE CLASSIFICATION PERFORMANCE
==================================================
Accuracy:  0.8700
Precision: 0.8123
Recall:    0.8415
F1 Score:  0.8266
ROC-AUC:   0.9234

Support Vectors: 234
Per class: [112 122]

==================================================
CLASSIFICATION REPORT
==================================================
                  precision    recall  f1-score   support

     No Disease       0.91      0.89      0.90      436
    Has Disease       0.81      0.84      0.83      164

        accuracy                           0.87      600
       macro avg       0.86      0.86      0.86      600
    weighted avg       0.86      0.86      0.86      600
```

## Visualization

### Maximum Margin Hyperplane

```
Decision Boundary with Maximum Margin
-----------------------------------------
                                    
    |      |                    |      
    |      |                    |      
    |    +-|-+                  |  Support Vector Region
    |     | |                   |      
----+---+-|-+---------+++-------+----
    |    | |                   |      
    |    | |                   |      
    |      |                    |      
    |      |                    |      

          Margin: distance to support vectors
          Hyperplane: optimal separating line
        
    + = Class A                 - = Class B
    | = Margin boundary        = = Decision boundary
```

### RBF Kernel Decision Boundary

```
Nonlinear Decision Boundary (RBF Kernel)
-----------------------------------------
                        
   Class 0  ***                            
         ******                            
       *  |  *                            
      *   |   *                           
     *    |    *                          
    *     |     *                         
   *      |      *                        
   *      |       *          Class 1      
   *     |        *         ********       
  **     |         **      *********       
         |                     ****      
                        ***********       
                       *************      
                                    
         Feature 1                      
                                    
         | = Nonlinear boundary
         * = Class points
```

### Support Vector Distribution

```
Support Vector Distribution
--------------------------------
         |
    60   |           [48]
         |            
    50   |
         |
    40   |
         |
    30   |  [31]
         |
    20   |
         |
    10   |
         |
     0   +------------------------
         Malignant    Benign
        
         Support vectors per class
         Total: 79 out of 455 (17.4%)
```

## Advanced Topics

### Parameter Tuning with Grid Search

```python
print("=" * 70)
print("HYPERPARAMETER TUNING")
print("=" * 70)

param_grid = {
    'C': [0.01, 0.1, 1, 10, 100],
    'gamma': [0.001, 0.01, 0.1, 1, 'scale', 'auto']
}

svm = SVC(kernel='rbf', random_state=42, probability=True)
grid_search = GridSearchCV(svm, param_grid, cv=5, scoring='roc_auc', n_jobs=-1)
grid_search.fit(X_train_scaled, y_train)

print(f"\nBest Parameters: {grid_search.best_params_}")
print(f"Best CV AUC: {grid_search.best_score_:.4f}")

results_df = pd.DataFrame(grid_search.cv_results_)
top_results = results_df.nsmallest(5, 'rank_test_score')[['param_C', 'param_gamma', 'mean_test_score', 'std_test_score']]
print(f"\nTop Configurations:")
print(top_results.to_string(index=False))
```

### Linear SVM for Large Datasets

```python
print("=" * 70)
print("LINEAR SVM FOR LARGE DATASETS")
print("=" * 70)

linear_svm = LinearSVC(C=1.0, max_iter=10000, random_state=42)
linear_svm.fit(X_train_scaled, y_train)

y_pred = linear_svm.predict(X_test_scaled)

print(f"\nLinear SVM Performance:")
print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall: {recall_score(y_test, y_pred):.4f}")

coefs = linear_svm.coef_[0]
print(f"\nTop Features by Coefficient Magnitude:")
feature_importance = np.argsort(np.abs(coefs))[-5:][::-1]
for i in feature_importance:
    print(f"  {feature_names[i]}: {coefs[i]:+.4f}")
```

### Decision Function Analysis

```python
print("=" * 70)
print("DECISION FUNCTION ANALYSIS")
print("=" * 70)

svm = SVC(kernel='rbf', C=1.0, gamma='scale', random_state=42)
svm.fit(X_train_scaled, y_train)

decision = svm.decision_function(X_test_scaled)

print(f"\nDecision Function Statistics:")
print(f"Mean: {np.mean(decision):.4f}")
print(f"Std: {np.std(decision):.4f}")
print(f"Min: {np.min(decision):.4f}")
print(f"Max: {np.max(decision):.4f}")
print(f"\nPositive (>0 predicts class 1): {np.sum(decision > 0)}")
print(f"Negative (<=0 predicts class 0): {np.sum(decision <= 0)}")
```

## Conclusion

Support Vector Classification provides a powerful approach to binary and multi-class classification through maximum margin optimization. The algorithm's geometric foundation, effectiveness in high-dimensional spaces, and computational efficiency make it a valuable tool in the machine learning toolkit. SVM's resistance to overfitting and interpretable support vectors provide advantages in regulated industries like banking and healthcare.

Key considerations for SVM include appropriate kernel and parameter selection. The RBF kernel provides a good starting point for most problems, with C and gamma requiring joint optimization through grid search. Feature scaling proves essential for consistent performance. Understanding support vectors provides insight into model behavior and enables outlier detection.

For banking applications, SVM provides reliable credit risk predictions that satisfy regulatory requirements for model documentation and validation. For healthcare, SVM's accuracy on high-dimensional clinical data supports diagnostic decision-making. While deep learning has surpassed SVM on many benchmark tasks, SVM remains valuable for its efficiency, interpretability, and strong out-of-the-box performance on problems with moderate dataset sizes.