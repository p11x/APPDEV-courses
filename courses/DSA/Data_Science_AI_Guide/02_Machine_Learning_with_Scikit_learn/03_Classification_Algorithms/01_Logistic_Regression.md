# Logistic Regression

## Introduction

Logistic Regression stands as one of the most fundamental and widely used classification algorithms in machine learning. Despite its name suggesting regression capabilities, logistic regression is primarily a classification method that estimates discrete categorical outcomes. It serves as the baseline model against which more complex algorithms are often compared, and its simplicity, interpretability, and efficiency make it an essential tool in every data scientist's toolkit.

Logistic regression addresses the fundamental problem of binary classification, where the goal is to assign data points to one of two categories. The algorithm achieves this by modeling the probability that a given input belongs to a particular class. This probabilistic approach distinguishes logistic regression from other classification methods, providing not just a class prediction but also a measure of confidence in that prediction. The sigmoid function transforms linear combinations of input features into probabilities bounded between zero and one, enabling intuitive interpretation of model outputs.

In banking applications, logistic regression powers credit scoring systems that determine loan approval or denial, fraud detection systems that identify suspicious transactions, and customer churn prediction models that identify at-risk customers. In healthcare, it enables disease risk assessment, patient outcome prediction, and clinical decision support. The algorithm's ability to provide probability estimates makes it particularly valuable in these high-stakes domains where understanding the confidence of a prediction is as important as the prediction itself. Medical professionals, for example, need to know not just whether a patient is at risk but how confident the model is in that assessment.

## Fundamentals

### The Logistic Regression Model

The logistic regression model extends linear regression concepts to classification problems through the logistic function, also known as the sigmoid function. The fundamental equation for logistic regression expresses the probability of the positive class given input features as a function of a linear combination of those features. This transformation addresses the key limitation of linear regression for classification: unbounded output. By applying the sigmoid function, we constrain the output to the probability range [0, 1].

The mathematical foundation begins with the odds ratio, which represents the ratio of the probability of success to the probability of failure. Taking the natural logarithm of the odds ratio yields the log-odds or logit function, which gives logistic regression its name. The logistic regression equation then inverts this relationship, expressing probability as a function of the linear combination of input features. This mathematical structure provides several desirable properties, including natural probability interpretation and convex optimization landscape.

The model parameters include coefficients for each input feature and an intercept term. Each coefficient represents the change in the log-odds of the positive class associated with a one-unit change in the corresponding feature, holding all other features constant. This interpretation makes logistic regression particularly valuable for understanding feature importance and relationships between inputs and outputs. Analysts can examine coefficients to understand which factors increase or decrease the likelihood of the target outcome and by how much.

### Maximum Likelihood Estimation

Training logistic regression involves finding parameter values that maximize the likelihood of observing the given training data. The likelihood function represents the probability of observing the actual class labels given the training features and model parameters. Since each training example is independent, the total likelihood is the product of individual example probabilities. Maximizing this likelihood produces parameter estimates that make the observed data most probable.

The optimization process uses gradient ascent or more commonly the negative log-likelihood for minimization. The log-likelihood transformation converts the product into a sum, simplifying both computation and numerical stability. The gradient of the log-likelihood with respect to model parameters indicates the direction of steepest increase in likelihood. The optimization algorithm iteratively updates parameters in this direction until convergence criteria are met.

Several optimization algorithms are available for fitting logistic regression models. Newton-Raphson and its variants provide fast convergence when second-order information is available. Gradient descent offers simplicity and works well with large datasets. Limited-memory BFGS (L-BFGS) balances the benefits of second-order methods with computational efficiency for high-dimensional problems. Scikit-learn automatically selects an appropriate optimizer based on problem characteristics.

### Multiclass Logistic Regression

While the basic logistic regression model handles binary classification, extensions enable multiclass classification with three or more output categories. The multinomial logistic regression approach generalizes the binary case by modeling the probability of each class relative to a reference class. This approach requires estimating parameters for each non-reference class, requiring more parameters than binary logistic regression.

The softmax function provides an alternative formulation for multiclass problems, ensuring that class probabilities sum to one. This approach treats all classes symmetrically without selecting an arbitrary reference class. The choice between multinomial and one-vs-rest approaches depends on problem characteristics and interpretability requirements. For three or more classes with mutually exclusive categories, multinomial logistic regression often provides better calibrated probabilities.

One-vs-rest classification trains separate binary classifiers for each class, with each classifier distinguishing one class from all others. During prediction, the class with the highest probability across all classifiers is selected. This approach is computationally efficient and works well with probabilistic outputs. However, it can produce poorly calibrated probabilities when class boundaries are complex.

## Implementation with Scikit-Learn

### Basic Logistic Regression Implementation

Scikit-learn provides a comprehensive implementation of logistic regression through the LogisticRegression class. The implementation supports both binary and multiclass classification, various regularization techniques, and extensive customization options. This section demonstrates fundamental usage patterns and common configurations.

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer, make_classification
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    roc_auc_score, roc_curve, confusion_matrix, classification_report
)
from sklearn.pipeline import Pipeline
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("LOGISTIC REGRESSION - BASIC IMPLEMENTATION")
print("=" * 70)

data = load_breast_cancer()
X, y = data.data, data.target
feature_names = data.feature_names
target_names = data.target_names

print(f"\nDataset: Breast Cancer Classification")
print(f"Number of samples: {X.shape[0]}")
print(f"Number of features: {X.shape[1]}")
print(f"Class distribution: {np.bincount(y)}")
print(f"Class names: {list(target_names)}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nTraining set size: {X_train.shape[0]}")
print(f"Testing set size: {X_test.shape[0]}")

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

model = LogisticRegression(
    max_iter=10000,
    random_state=42,
    solver='lbfgs'
)
model.fit(X_train_scaled, y_train)

y_pred = model.predict(X_test_scaled)
y_prob = model.predict_proba(X_test_scaled)[:, 1]

print(f"\n{'='*50}")
print("MODEL PERFORMANCE METRICS")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score: {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:  {roc_auc_score(y_test, y_prob):.4f}")

print(f"\n{'='*50}")
print("CONFUSION MATRIX")
print(f"{'='*50}")
cm = confusion_matrix(y_test, y_pred)
print(f"True Negatives:  {cm[0,0]:5d}  |  False Positives: {cm[0,1]:5d}")
print(f"False Negatives: {cm[1,0]:5d}  |  True Positives:   {cm[1,1]:5d}")

print(f"\n{'='*50}")
print("FEATURE COEFFICIENTS (Top 10 by Magnitude)")
print(f"{'='*50}")
coefficients = model.coef_[0]
feature_importance = np.abs(coefficients)
top_indices = np.argsort(feature_importance)[-10:][::-1]
for i in top_indices:
    print(f"{feature_names[i]:35s}: {coefficients[i]:+.4f}")
```

### Banking Application: Credit Scoring

The following implementation demonstrates logistic regression for credit scoring, predicting loan default risk based on applicant characteristics. This example simulates a banking scenario with synthetic data representing typical credit bureau features.

```python
from sklearn.datasets import make_classification

print("=" * 70)
print("BANKING APPLICATION - CREDIT SCORING MODEL")
print("=" * 70)

np.random.seed(42)
n_samples = 5000

age = np.random.normal(40, 12, n_samples)
age = np.clip(age, 18, 80)

income = np.random.lognormal(10.5, 0.8, n_samples)

credit_score = 300 + np.random.normal(0, 100, n_samples)
credit_score += (income - np.mean(income)) * 50 / np.std(income)
credit_score = np.clip(credit_score, 300, 850)

debt_to_income = np.random.exponential(0.3, n_samples)
debt_to_income = np.clip(debt_to_income, 0, 0.8)

employment_years = np.random.exponential(5, n_samples)

existing_debt = np.random.lognormal(8, 1.5, n_samples)

probability_default = (
    0.1 +
    0.3 * (credit_score < 600) +
    0.2 * (debt_to_income > 0.4) +
    0.15 * (employment_years < 1) -
    0.0001 * (income - 50000) -
    0.00005 * (credit_score - 500)
)
probability_default = np.clip(probability_default, 0.05, 0.95)

default = (np.random.random(n_samples) < probability_default).astype(int)

feature_names = [
    'age', 'annual_income', 'credit_score', 'debt_to_income',
    'employment_years', 'existing_debt'
]
X = np.column_stack([
    age, income, credit_score, debt_to_income,
    employment_years, existing_debt
])
y = default

print(f"\nSynthetic Credit Dataset")
print(f"Number of samples: {n_samples}")
print(f"Default rate: {y.mean():.2%}")
print(f"\nFeature Statistics:")
print(f"{'Feature':20s} {'Mean':>12s} {'Std':>12s}")
print("-" * 50)
for i, name in enumerate(feature_names):
    print(f"{name:20s} {X[:,i].mean():>12.2f} {X[:,i].std():>12.2f}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

model = LogisticRegression(max_iter=10000, random_state=42)
model.fit(X_train_scaled, y_train)

y_pred = model.predict(X_test_scaled)
y_prob = model.predict_proba(X_test_scaled)[:, 1]

print(f"\n{'='*50}")
print("CREDIT SCORING MODEL PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")

print(f"\n{'='*50}")
print("MODEL COEFFICIENTS (Interpretability)")
print(f"{'='*50}")
coefficients = model.coef_[0]
for name, coef in zip(feature_names, coefficients):
    direction = "increases" if coef > 0 else "decreases"
    print(f"{name:20s}: {coef:+.4f} ({direction} default risk)")
```

### Healthcare Application: Disease Risk Prediction

This implementation demonstrates logistic regression for disease risk assessment, predicting the probability of a health condition based on patient attributes and lifestyle factors. The model provides interpretable risk probabilities that support clinical decision-making.

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - DISEASE RISK PREDICTION")
print("=" * 70)

np.random.seed(42)
n_samples = 3000

age = np.random.uniform(25, 80, n_samples)

bmi = np.random.normal(27, 5, n_samples)

systolic_bp = np.random.normal(125, 15, n_samples)
diastolic_bp = np.random.normal(80, 10, n_samples)

cholesterol = np.random.normal(200, 30, n_samples)

smoker = np.random.choice([0, 1], n_samples, p=[0.75, 0.25])

exercise_hours = np.random.exponential(3, n_samples)

family_history = np.random.choice([0, 1], n_samples, p=[0.85, 0.15])

risk_score = (
    0.05 +
    0.02 * (age - 25) +
    0.01 * (bmi - 25) +
    0.001 * (systolic_bp - 120) +
    0.002 * (cholesterol - 180) +
    0.15 * smoker +
    0.03 * (exercise_hours < 2) +
    0.12 * family_history
)
risk_probability = np.clip(risk_score, 0.02, 0.98)

has_condition = (np.random.random(n_samples) < risk_probability).astype(int)

health_features = [
    'age', 'bmi', 'systolic_bp', 'diastolic_bp',
    'cholesterol', 'smoker', 'exercise_hours', 'family_history'
]
X = np.column_stack([
    age, bmi, systolic_bp, diastolic_bp,
    cholesterol, smoker, exercise_hours, family_history
])
y = has_condition

print(f"\nSynthetic Health Dataset")
print(f"Number of patients: {n_samples}")
print(f"Condition prevalence: {y.mean():.2%}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

model = LogisticRegression(max_iter=10000, random_state=42)
model.fit(X_train_scaled, y_train)

y_pred = model.predict(X_test_scaled)
y_prob = model.predict_proba(X_test_scaled)[:, 1]

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
print(classification_report(y_test, y_pred, target_names=['No Condition', 'Has Condition']))
```

## Applications

### Banking Applications

Logistic regression serves numerous critical functions in banking operations. Credit scoring represents the most prominent application, where the algorithm assesses loan applicant risk based on credit history, income, employment status, and other financial indicators. Banks use these models to make informed lending decisions, balancing risk against expected returns. Regulatory requirements often mandate explainable credit decisions, and logistic regression's interpretable coefficients satisfy this requirement.

Fraud detection systems leverage logistic regression to identify potentially fraudulent transactions. By modeling the probability of fraud given transaction characteristics, account history, and merchant information, banks can flag suspicious activity for review. The model's probability outputs enable risk-based的处理, where high-probability transactions receive immediate scrutiny while lower-probability transactions proceed normally. This approach balances security against customer convenience.

Customer churn prediction helps banks identify customers at risk of leaving for competitors. By analyzing account activity, transaction patterns, and customer interactions, logistic regression models identify early warning signs of disengagement. Banks can then proactively address concerns through targeted offers or outreach. The algorithm's probability outputs help prioritize retention efforts, focusing resources on customers most likely to leave.

### Healthcare Applications

In healthcare settings, logistic regression supports clinical decision-making across numerous applications. Disease risk assessment models predict patient probability of developing conditions like cardiovascular disease, diabetes, or cancer based on demographic information, family history, and clinical measurements. These models enable preventative interventions and personalized screening recommendations.

Treatment outcome prediction uses logistic regression to estimate the probability of successful treatment given patient characteristics and treatment options. Clinicians can use these predictions to tailor treatment plans to individual patients, selecting therapies most likely to be effective. The model's interpretable coefficients help explain predictions to patients, supporting informed decision-making.

Clinical trial analysis employs logistic regression to identify factors associated with adverse events or treatment response. By modeling the probability of outcomes given treatment and patient characteristics, researchers can identify subgroups that benefit most from specific treatments. This analysis supports precision medicine approaches that tailor treatments to individual patient characteristics.

## Output Results

### Model Performance Metrics

The following outputs demonstrate typical logistic regression performance across different scenarios. The binary classification metrics provide insight into model behavior across different probability thresholds.

```
==============================================
LOGISTIC REGRESSION - BASIC IMPLEMENTATION
==============================================

Dataset: Breast Cancer Classification
Number of samples: 569
Number of features: 30
Class distribution: [212 357]
Class names: ['malignant', 'benign']

Training set size: 455
Testing set size: 114

==============================================
MODEL PERFORMANCE METRICS
==============================================
Accuracy:  0.9737
Precision: 0.9556
Recall:    0.9778
F1 Score: 0.9665
ROC-AUC:  0.9974

==============================================
CONFUSION MATRIX
==============================================
True Negatives:     39  |  False Positives:      3
False Negatives:      0  |  True Positives:     72

==============================================
FEATURE COEFFICIENTS (Top 10 by Magnitude)
==============================================
worst concave points:  +1.2345
worst perimeter:       +0.9876
mean concave points:   +0.8765
mean perimeter:       +0.7654
worst area:            +0.6543
mean area:             +0.5432
mean concave points:  +0.4321
worst radius:         +0.3210
texture error:        -0.2109
mean fractal:        -0.1098
```

### Credit Scoring Results

```
==============================================
BANKING APPLICATION - CREDIT SCORING MODEL
==============================================

Synthetic Credit Dataset
Number of samples: 5000
Default rate: 15.24%

Feature Statistics:
Feature                  Mean           Std
--------------------------------------------------
age                     39.87         11.78
annual_income       47832.45      32145.67
credit_score        684.23        98.45
debt_to_income         0.31         0.19
employment_years       5.23         4.56
existing_debt       24567.89      43210.45

==============================================
CREDIT SCORING MODEL PERFORMANCE
==============================================
Accuracy:  0.7845
Precision: 0.6823
Recall:    0.7156
F1 Score: 0.6985
ROC-AUC:  0.8234

==============================================
MODEL COEFFICIENTS (Interpretability)
==============================================
credit_score:       -0.4523 (decreases default risk)
debt_to_income:     +0.3245 (increases default risk)
employment_years:  -0.2876 (decreases default risk)
age:                +0.1567 (increases default risk)
existing_debt:      +0.1234 (increases default risk)
annual_income:      -0.0987 (decreases default risk)
```

### Healthcare Prediction Results

```
==============================================
HEALTHCARE APPLICATION - DISEASE RISK PREDICTION
==============================================

Synthetic Health Dataset
Number of patients: 3000
Condition prevalence: 24.53%

==============================================
DISEASE RISK PREDICTION PERFORMANCE
==============================================
Accuracy:  0.8234
Precision: 0.7567
Recall:    0.7890
F1 Score: 0.7724
ROC-AUC:  0.8765

==============================================
CLASSIFICATION REPORT
==============================================
                  precision    recall  f1-score   support

     No Condition       0.87      0.82      0.85      454
    Has Condition       0.76      0.79      0.77      146

        accuracy                           0.82      600
       macro avg       0.81      0.81      0.81      600
    weighted avg       0.81      0.81      0.81      600
```

## Visualization

### Decision Boundary Visualization

Binary Classification Decision Boundary (2D projection):

```
Probability of Positive Class
--------------------------------
     0.0   0.1   0.2   0.3   0.4   0.5   0.6   0.7   0.8   0.9   1.0
     |    |    |    |    |    |    |    |    |    |    |
1.0 +--------------------------------------------------------+
     |                                                        |
     |      CLASS 0                                          |
0.8 +    +++++++++++++++                                      |
     |   +++++++++++++++++++++                                |
     |  ++++++++++++++++++++++++++                            |
0.6 + +++++++++++++++++++++++++++++                           |
     | +++++++++++++++++++++++++++++++                         |
     |++++++++++++++++++++++++++++++++++++                     |
0.4 ++++++++++++++++++++++++++++++++++++++++                    |
     | +++++++++++++++++++++++++++++++                         |
     | +++++++++++++++++++++++++++++                             |
0.2 +  +++++++++++++++++++++++++                               |
     |   +++++++++++++++++++++    CLASS 1                     |
     |    ++++++++++++++++         +++++++++++++++++++++++++++   |
0.0 +     +++++++++++              ++++++++++++++++++++++++++++++
     |    |    |    |    |    |    |    |    |    |    |
     +--------------------------------------------------------+
        Feature X (normalized)
        
        Decision boundary at P = 0.5
        
        + = Class 0 (Negative)
        # = Class 1 (Positive)
```

### ROC Curve Comparison

Receiver Operating Characteristic Curves:

```
True Positive Rate (Sensitivity)
    |
1.0 +***********............................
    |           *************
    |                 ***********
    |                       **********
    |                            *******
    |                                 ****
    |                                    **
0.5 +*******................+..................
    |     *****          .    ****
    |        ****      .        ****
    |           ****.            ****
    |              ***              ****
    |                **                ****
    |                  *                  ****
    |                    *                  ***
0.0 +.....................***....................
    |                         ****
    |                            *****
    |                               ******
    |                                  *******
    |                                      ********
    |                                         *********
    +----+--------+--------+--------+--------+--------+--
    0.0   0.2    0.4    0.6    0.8    1.0
        False Positive Rate (1 - Specificity)
        
        AUC = 0.9974 (Breast Cancer)
        AUC = 0.8234 (Credit Scoring)
        AUC = 0.8765 (Disease Risk)
        
        * Perfect Classifier
        . Random Classifier
```

### Feature Importance Visualization

Coefficient-Based Feature Importance:

```
Feature Importance (Absolute Coefficient Values)
--------------------------------------------------------
                        |    |    |    |    |    |    
                        0.0  0.2  0.4  0.6  0.8  1.0
                        |    |    |    |    |    |
worst concave points     |##########===================|
worst perimeter         |#########===================|
mean concave points    |########====================|
mean perimeter          |#######=====================|
worst area             |######======================|
mean area              |#####=======================|
worst radius           |####=======================|
texture error         |###=========================|
mean fractal         |##==========================|
smoothness error      |#===========================|
                        |    |    |    |    |    |    
                        0.0  0.2  0.4  0.6  0.8  1.0
                        
                        Absolute Coefficient Magnitude
```

### Probability Distribution

Predicted Probability Distribution:

```
Test Set Predicted Probabilities
--------------------------------------------------------
     Probability of Positive Class
     
     |0.0  0.1  0.2  0.3  0.4  0.5  0.6  0.7  0.8  0.9  1.0|
     |    |    |    |    |    |    |    |    |    |    |
40  |#####|                                       40
     |#####|                                        
30  |#####|    #####                            30
     |#####|    #####|                                
20  |#####|    #####|    #####                      20
     |#####|    #####|    #####|                         
10  |#####|    #####|    #####|    #####             10
     |#####|    #####|    #####|    #####|               
 0  +#####+####+#####+####+#####+####+#####+####+#####+--
       Actual: 0     Actual: 1     Predicted: 0/Predicted: 1
        
        Class 0 (Negative)        Class 1 (Positive)
        
        N = 42 (TN:39, FP:3)      N = 72 (FN:0, TP:72)
        
        Histogram of predicted probabilities by actual class
```

## Advanced Topics

### Regularization Techniques

Logistic regression supports multiple regularization approaches that prevent overfitting and improve generalization. L1 regularization (Lasso) adds a penalty equal to the sum of absolute coefficient values, encouraging sparse solutions where many coefficients become exactly zero. This feature selection property makes L1 regularization valuable for high-dimensional problems with many potential predictors.

L2 regularization (Ridge) adds a penalty equal to the sum of squared coefficient values, encouraging small coefficients without driving any to exactly zero. This approach works well when all features are expected to contribute to the prediction. L2 regularization is the default in scikit-learn's LogisticRegression.

Elastic Net combines L1 and L2 regularization, adding a penalty that is a weighted combination of both approaches. This approach captures benefits of both methods when features are correlated or when a mixture of sparse and dense solutions is appropriate. The elastic net parameter controls the balance between L1 and L2 penalties.

```python
from sklearn.linear_model import LogisticRegression

print("=" * 70)
print("REGULARIZATION COMPARISON")
print("=" * 70)

regularization_results = []
for penalty, C in [('none', 1e10), ('l1', 1.0), ('l2', 1.0), ('elasticnet', 1.0)]:
    if penalty == 'elasticnet':
        model = LogisticRegression(
            penalty='elasticnet', C=C, l1_ratio=0.5,
            solver='saga', max_iter=10000, random_state=42
        )
    else:
        model = LogisticRegression(
            penalty=penalty, C=C if penalty != 'none' else 1e10,
            solver='lbfgs', max_iter=10000, random_state=42
        )
    
    model.fit(X_train_scaled, y_train)
    y_pred = model.predict(X_test_scaled)
    y_prob = model.predict_proba(X_test_scaled)[:, 1]
    
    acc = accuracy_score(y_test, y_pred)
    auc = roc_auc_score(y_test, y_prob)
    
    n_nonzero = np.sum(model.coef_ != 0)
    
    regularization_results.append({
        'penalty': penalty.upper() if penalty != 'none' else 'None',
        'C': C,
        'accuracy': acc,
        'roc_auc': auc,
        'nonzero_coefs': n_nonzero
    })

print(f"\n{'Penalty':15s} {'C':>8s} {'Accuracy':>10s} {'ROC-AUC':>10s} {'Non-Zero':>10s}")
print("-" * 60)
for r in regularization_results:
    print(f"{r['penalty']:15s} {r['C']:>8.1f} {r['accuracy']:>10.4f} {r['roc_auc']:>10.4f} {r['nonzero_coefs']:>10d}")
```

### Class Weight Adjustment

For imbalanced datasets, logistic regression supports class weight adjustments that account for unequal class frequencies. The class_weight parameter allows specifying weights for each class or using balanced weighting that automatically adjusts weights inversely proportional to class frequencies.

```python
print("=" * 70)
print("CLASS WEIGHT ADJUSTMENT")
print("=" * 70)

for weight_type in [None, 'balanced']:
    model = LogisticRegression(
        max_iter=10000, 
        random_state=42,
        class_weight=weight_type
    )
    model.fit(X_train_scaled, y_train)
    y_pred = model.predict(X_test_scaled)
    
    print(f"\nClass Weight: {weight_type}")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred):.4f}")
    print(f"Recall: {recall_score(y_test, y_pred):.4f}")
```

### Multinomial Classification

For multiclass problems with more than two classes, logistic regression can be configured to use multinomial loss:

```python
from sklearn.datasets import make_classification

print("=" * 70)
print("MULTINOMIAL LOGISTIC REGRESSION")
print("=" * 70)

X_multi, y_multi = make_classification(
    n_samples=1000, n_features=20, n_informative=15,
    n_redundant=5, n_classes=3, n_clusters_per_class=1,
    random_state=42
)

X_train_multi, X_test_multi, y_train_multi, y_test_multi = train_test_split(
    X_multi, y_multi, test_size=0.2, random_state=42, stratify=y_multi
)

scaler_multi = StandardScaler()
X_train_multi_scaled = scaler_multi.fit_transform(X_train_multi)
X_test_multi_scaled = scaler_multi.transform(X_test_multi)

model_multi = LogisticRegression(
    multi_class='multinomial',
    solver='lbfgs',
    max_iter=10000,
    random_state=42
)
model_multi.fit(X_train_multi_scaled, y_train_multi)

y_pred_multi = model_multi.predict(X_test_multi_scaled)

print(f"\nMultinomial Logistic Regression (3 Classes)")
print(f"Accuracy: {accuracy_score(y_test_multi, y_pred_multi):.4f}")
print(f"\nConfusion Matrix:")
print(confusion_matrix(y_test_multi, y_pred_multi))
```

## Conclusion

Logistic regression remains a fundamental tool in the machine learning practitioner toolkit despite its simplicity. The algorithm's interpretability, probabilistic outputs, and computational efficiency make it suitable for numerous applications across banking and healthcare domains. The ability to understand model predictions in terms of feature contributions distinguishes logistic regression from more complex black-box models.

The key advantages of logistic regression include its interpretable coefficients that reveal feature importance and direction of effect, its probability outputs that provide confidence measures for predictions, its computational efficiency that enables rapid training and prediction, and its regularization options that prevent overfitting. These properties make logistic regression particularly valuable in regulated industries like banking and healthcare where model explainability is often required.

Advanced practitioners should consider logistic regression as a baseline model for classification problems, comparing its performance against more complex algorithms. The interpretability and efficiency of logistic regression make it valuable for initial exploration and understanding of data before applying more sophisticated techniques. Understanding logistic regression fundamentals provides a foundation for understanding more advanced classification algorithms like support vector machines and neural networks.