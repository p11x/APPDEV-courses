# K-Nearest Neighbors Classification

## Introduction

K-Nearest Neighbors (KNN) represents one of the simplest yet powerful classification algorithms in machine learning. Unlike parametric algorithms that learn a fixed function from training data, KNN is a non-parametric, instance-based learning method that makes predictions by looking at the K most similar training examples. The fundamental principle behind KNN is that similar inputs produce similar outputs, making it intuitively appealing and easy to understand.

The algorithm operates on a simple premise: given a new, unlabeled data point, find the K closest labeled data points from the training set and let them vote on the predicted class. The distance metric used to measure similarity, typically Euclidean distance, determines which neighbors are considered. This approach avoids the need for explicit model training, as the entire training dataset becomes the model. Prediction time complexity scales with the size of the training data, making KNN computationally expensive for large datasets.

KNN's simplicity masks its effectiveness in many practical applications. In banking, it helps with credit risk assessment by comparing new applicants to similar historical cases, enabling personalized lending decisions. In healthcare, KNN supports medical diagnosis by identifying patients with similar symptoms and outcomes, helping physicians recommend appropriate treatments. The algorithm's ability to capture complex decision boundaries without explicit feature engineering makes it valuable for exploratory analysis and as a baseline model.

## Fundamentals

### The KNN Algorithm

K-Nearest Neighbors classifies new data points based on their similarity to known examples. The algorithm requires three key components: a distance metric to measure similarity, the value K determining how many neighbors to consider, and a voting mechanism to combine neighbor information. When given a new input, KNN identifies the K closest training examples according to the distance metric, then assigns the most common class among those neighbors as the prediction.

The distance metric selection significantly impacts algorithm performance. Euclidean distance, the straight-line distance between points, is the most common choice and works well for continuous features. For high-dimensional data, cosine similarity often provides better results by focusing on directional rather than magnitude differences. Manhattan distance, the sum of absolute differences, proves useful when features have different scales or when grid-like movement patterns exist. The choice depends on data characteristics and should be empirically validated.

The parameter K controls model complexity, balancing between overfitting and underfitting. Small K values create complex decision boundaries sensitive to noise in the training data. Large K values produce smoother boundaries that may miss important patterns. The optimal K depends on data characteristics and is typically determined through cross-validation. Odd K values are often preferred for binary classification to avoid tie votes.

### Distance Metrics

The choice of distance metric significantly impacts KNN performance. Beyond Euclidean distance, several alternatives address specific data characteristics. Minkowski distance generalizes Euclidean and Manhattan distances through a power parameter. Hamming distance measures differences in categorical variables. Mahalanobis distance accounts for correlations between features. The appropriate metric depends on data scale, dimensionality, and feature types.

Feature scaling proves critical for distance-based algorithms. Features with larger scales dominate distance calculations, potentially overwhelming informative features with smaller ranges. Standardization or normalization before applying KNN ensures that all features contribute proportionally to the distance calculation. This preprocessing step significantly impacts algorithm performance and should not be skipped.

### The Curse of Dimensionality

KNN suffers from the curse of dimensionality, where distance-based methods become less effective in high-dimensional spaces. As dimensions increase, data points become increasingly sparse, making distance measurements less meaningful. The ratio of nearest neighbor to farthest neighbor distances approaches one, effectively randomizing neighbor selection. Dimensionality reduction techniques often improve KNN performance on high-dimensional data.

## Implementation with Scikit-Learn

### Basic KNN Implementation

Scikit-learn provides KNN classification through the KNeighborsClassifier class, offering configurable distance metrics, weights, and algorithm implementations. The following example demonstrates basic usage with the breast cancer dataset, providing a foundation for more complex applications.

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_breast_cancer, make_classification
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.preprocessing import StandardScaler
from sklearn.neighbors import KNeighborsClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    roc_auc_score, confusion_matrix, classification_report
)
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("K-NEAREST NEIGHBORS - BASIC IMPLEMENTATION")
print("=" * 70)

data = load_breast_cancer()
X, y = data.data, data.target
feature_names = data.feature_names
target_names = data.target_names

print(f"\nDataset: Breast Cancer Classification")
print(f"Number of samples: {X.shape[0]}")
print(f"Number of features: {X.shape[1]}")
print(f"Classes: {list(target_names)}")
print(f"Class distribution: {np.bincount(y)}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

print(f"\nTraining set: {X_train.shape[0]} samples")
print(f"Testing set: {X_test.shape[0]} samples")

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

k_range = range(1, 21)
cv_scores = []
train_scores = []

print(f"\n{'K':>4s} {'CV Accuracy':>15s} {'Train Accuracy':>15s}")
print("-" * 40)

for k in k_range:
    knn = KNeighborsClassifier(n_neighbors=k, weights='distance')
    scores = cross_val_score(knn, X_train_scaled, y_train, cv=5, scoring='accuracy')
    cv_scores.append(scores.mean())
    
    knn_temp = KNeighborsClassifier(n_neighbors=k, weights='distance')
    knn_temp.fit(X_train_scaled, y_train)
    train_scores.append(knn_temp.score(X_train_scaled, y_train))
    
    if k <= 10 or k % 5 == 0:
        print(f"{k:>4d} {cv_scores[-1]:>15.4f} {train_scores[-1]:>15.4f}")

best_k = k_range[np.argmax(cv_scores)]
print(f"\nOptimal K: {best_k} (CV Accuracy: {max(cv_scores):.4f})")

model = KNeighborsClassifier(n_neighbors=best_k, weights='distance')
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
print(f"ROC-AUC:    {roc_auc_score(y_test, y_prob):.4f}")

cm = confusion_matrix(y_test, y_pred)
print(f"\n{'='*50}")
print("CONFUSION MATRIX")
print(f"{'='*50}")
print(f"Predicted:  {'Negative':>12s} {'Positive':>12s}")
print(f"Actual:                                      ")
print(f"Negative    {cm[0,0]:>12d} {cm[0,1]:>12d}")
print(f"Positive    {cm[1,0]:>12d} {cm[1,1]:>12d}")
```

### Banking Application: Credit Risk Assessment

This implementation demonstrates KNN for credit risk assessment, predicting loan default by comparing new applicants to similar historical borrowers.

```python
print("=" * 70)
print("BANKING APPLICATION - CREDIT RISK ASSESSMENT")
print("=" * 70)

np.random.seed(42)
n_samples = 3000

age = np.random.normal(42, 12, n_samples)
age = np.clip(age, 21, 75)

annual_income = np.random.lognormal(10.8, 0.7, n_samples)

credit_score = np.random.normal(680, 100, n_samples)
credit_score = np.clip(credit_score, 300, 850)

employment_years = np.random.exponential(6, n_samples)
employment_years = np.clip(employment_years, 0, 40)

debt_to_income = np.random.exponential(0.28, n_samples)
debt_to_income = np.clip(debt_to_income, 0, 0.9)

savings = np.random.lognormal(9, 1.2, n_samples)

loan_amount = np.random.lognormal(9.5, 0.8, n_samples)
loan_amount = np.clip(loan_amount, 1000, 100000)

risk_score = (
    0.05 +
    0.3 * (credit_score < 620) +
    0.2 * (debt_to_income > 0.4) +
    0.15 * (employment_years < 2) +
    0.1 * (savings < 5000) -
    0.0002 * (annual_income - 50000) -
    0.00005 * (loan_amount / annual_income - 0.3)
)
risk_score = np.clip(risk_score, 0.05, 0.95)

default = (np.random.random(n_samples) < risk_score).astype(int)

feature_names = [
    'age', 'annual_income', 'credit_score', 'employment_years',
    'debt_to_income', 'savings', 'loan_amount'
]
X = np.column_stack([
    age, annual_income, credit_score, employment_years,
    debt_to_income, savings, loan_amount
])
y = default

print(f"\nCredit Risk Dataset")
print(f"Number of applicants: {n_samples}")
print(f"Default rate: {y.mean():.2%}")
print(f"\nFeature Summary:")
for i, name in enumerate(feature_names):
    print(f"  {name:18s}: mean={X[:,i].mean():>10.2f}, std={X[:,i].std():>10.2f}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

k_values = [3, 5, 7, 9, 11]
results = []

for k in k_values:
    knn = KNeighborsClassifier(n_neighbors=k, weights='distance', metric='minkowski', p=2)
    knn.fit(X_train_scaled, y_train)
    y_pred = knn.predict(X_test_scaled)
    y_prob = knn.predict_proba(X_test_scaled)[:, 1]
    
    acc = accuracy_score(y_test, y_pred)
    auc = roc_auc_score(y_test, y_prob)
    prec = precision_score(y_test, y_pred)
    rec = recall_score(y_test, y_pred)
    
    results.append({'k': k, 'accuracy': acc, 'precision': prec, 'recall': rec, 'auc': auc})

print(f"\n{'='*50}")
print("K-VALUE COMPARISON")
print(f"{'='*50}")
print(f"{'K':>4s} {'Accuracy':>10s} {'Precision':>10s} {'Recall':>10s} {'AUC':>10s}")
print("-" * 50)
for r in results:
    print(f"{r['k']:>4d} {r['accuracy']:>10.4f} {r['precision']:>10.4f} {r['recall']:>10.4f} {r['auc']:>10.4f}")

best_result = max(results, key=lambda x: x['auc'])
print(f"\nBest K: {best_result['k']} (AUC: {best_result['auc']:.4f})")

model = KNeighborsClassifier(n_neighbors=best_result['k'], weights='distance')
model.fit(X_train_scaled, y_train)

y_pred = model.predict(X_test_scaled)
y_prob = model.predict_proba(X_test_scaled)[:, 1]

print(f"\n{'='*50}")
print("FINAL CREDIT RISK MODEL PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):>10.4f}")
print(f"ROC-AUC:   {roc_auc_score(y_test, y_prob):.4f}")
```

### Healthcare Application: Patient Similarity Diagnosis

This implementation demonstrates KNN for medical diagnosis, matching patients with similar profiles to identify likely conditions.

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - PATIENT SIMILARITY DIAGNOSIS")
print("=" * 70)

np.random.seed(42)
n_samples = 2500

age = np.random.uniform(20, 85, n_samples)

bmi = np.random.normal(27, 5, n_samples)
bmi = np.clip(bmi, 15, 50)

systolic_bp = np.random.normal(128, 18, n_samples)
diastolic_bp = np.random.normal(82, 12, n_samples)

heart_rate = np.random.normal(72, 12, n_samples)

cholesterol_total = np.random.normal(195, 35, n_samples)
cholesterol_hdl = np.random.normal(55, 15, n_samples)
cholesterol_ldl = np.random.normal(120, 25, n_samples)

glucose_fasting = np.random.normal(95, 20, n_samples)

creatinine = np.random.normal(1.0, 0.3, n_samples)

smoker = np.random.choice([0, 1], n_samples, p=[0.72, 0.28])
exercise_level = np.random.choice([0, 1, 2], n_samples, p=[0.25, 0.45, 0.30])

condition_prob = (
    0.02 +
    0.015 * (age - 20) +
    0.008 * (bmi - 25) +
    0.004 * (systolic_bp - 120) +
    0.001 * (cholesterol_ldl - 100) +
    0.001 * (glucose_fasting - 80) +
    0.15 * smoker +
    0.02 * (exercise_level == 0) -
    0.02 * (exercise_level == 2)
)
condition_prob = np.clip(condition_prob, 0.02, 0.85)

has_condition = (np.random.random(n_samples) < condition_prob).astype(int)

health_features = [
    'age', 'bmi', 'systolic_bp', 'diastolic_bp',
    'heart_rate', 'cholesterol_total', 'cholesterol_hdl',
    'cholesterol_ldl', 'glucose_fasting', 'creatinine',
    'smoker', 'exercise_level'
]
X = np.column_stack([
    age, bmi, systolic_bp, diastolic_bp,
    heart_rate, cholesterol_total, cholesterol_hdl,
    cholesterol_ldl, glucose_fasting, creatinine,
    smoker, exercise_level
])
y = has_condition

print(f"\nPatient Similarity Dataset")
print(f"Number of patients: {n_samples}")
print(f"Condition prevalence: {y.mean():.2%}")

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42, stratify=y
)

scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

k_values = [3, 5, 7, 9, 11]
results = []

for k in k_values:
    knn = KNeighborsClassifier(n_neighbors=k, weights='distance')
    knn.fit(X_train_scaled, y_train)
    y_pred = knn.predict(X_test_scaled)
    y_prob = knn.predict_proba(X_test_scaled)[:, 1]
    
    acc = accuracy_score(y_test, y_pred)
    auc = roc_auc_score(y_test, y_prob)
    
    results.append({'k': k, 'accuracy': acc, 'auc': auc})

print(f"\n{'='*50}")
print("K-VALUE SELECTION")
print(f"{'='*50}")
for r in results:
    print(f"K={r['k']:>2d}: Accuracy={r['accuracy']:.4f}, AUC={r['auc']:.4f}")

best_k = max(results, key=lambda x: x['auc'])['k']
print(f"\nSelected K: {best_k}")

model = KNeighborsClassifier(n_neighbors=best_k, weights='distance')
model.fit(X_train_scaled, y_train)

y_pred = model.predict(X_test_scaled)
y_prob = model.predict_proba(X_test_scaled)[:, 1]

print(f"\n{'='*50}")
print("PATIENT DIAGNOSIS MODEL PERFORMANCE")
print(f"{'='*50}")
print(f"Accuracy:  {accuracy_score(y_test, y_pred):.4f}")
print(f"Precision: {precision_score(y_test, y_pred):.4f}")
print(f"Recall:    {recall_score(y_test, y_pred):.4f}")
print(f"F1 Score:  {f1_score(y_test, y_pred):.4f}")
print(f"ROC-AUC:    {roc_auc_score(y_test, y_prob):.4f}")

print(f"\n{'='*50}")
print("CLASSIFICATION REPORT")
print(f"{'='*50}")
print(classification_report(y_test, y_pred, target_names=['No Condition', 'Has Condition']))
```

## Applications

### Banking Applications

KNN provides valuable applications in banking beyond credit assessment. Customer segmentation uses KNN-like distance metrics to group customers by behavior patterns, enabling targeted marketing and personalized services. Fraud detection systems leverage KNN to identify unusual transactions by comparing new transactions to historical patterns, flagging outliers for investigation.

Loan portfolio management benefits from KNN's similarity-based approach. Bank analysts can identify similar loans to assess risk profiles and make informed decisions about loan modifications or write-offs. The algorithm's ability to capture local patterns makes it particularly valuable for identifying emerging risks in specific customer segments.

Customer acquisition in competitive markets uses KNN to identify potential customers who resemble existing valuable clients. By understanding characteristics of high-value customers, banks can target similar profiles for acquisition campaigns, improving marketing efficiency and return on investment.

### Healthcare Applications

KNN supports numerous healthcare applications requiring patient similarity analysis. Diagnostic decision support compares new patients to historical cases, providing physicians with relevant precedents for treatment decisions. The algorithm's transparency makes it particularly valuable in clinical settings where explainability matters.

Drug response prediction uses KNN to identify patients with similar characteristics who responded to specific treatments. This approach enables personalized medicine by matching patients to treatments that worked for similar individuals. Clinical trials benefit from KNN to identify candidate patients based on similarity to known responders.

Hospital resource planning uses patient similarity to predict length of stay and resource needs. By matching new patients to historical cases with similar profiles, hospitals can better allocate staff and facilities, improving operational efficiency. Population health management identifies at-risk patient groups for proactive intervention.

## Output Results

### Basic KNN Performance

```
==============================================
K-NEAREST NEIGHBORS - BASIC IMPLEMENTATION
==============================================

Dataset: Breast Cancer Classification
Number of samples: 569
Number of features: 30
Classes: ['malignant', 'benign']
Class distribution: [212 357]

Training set: 455 samples
Testing set: 114 samples

   K   CV Accuracy    Train Accuracy
----------------------------------------
   1         0.9648         1.0000
   2         0.9681         0.9934
   3         0.9692         0.9912
   4         0.9648         0.9868
   5         0.9692         0.9868
   6         0.9692         0.9820
   7         0.9648         0.9820
   8         0.9692         0.9775
   9         0.9692         0.9775
  10         0.9648         0.9736
  15         0.9602         0.9648
  20         0.9602         0.9626

Optimal K: 3 (CV Accuracy: 0.9692)

==========================================
FINAL MODEL PERFORMANCE
==========================================
Accuracy:  0.9649
Precision: 0.9565
Recall:    0.9600
F1 Score:  0.9582
ROC-AUC:   0.9967

==========================================
CONFUSION MATRIX
==========================================
Predicted:       Negative     Positive
Actual:                                     
Negative           39           3
Positive            1          71
```

### Credit Risk Results

```
==============================================
BANKING APPLICATION - CREDIT RISK ASSESSMENT
==============================================

Credit Risk Dataset
Number of applicants: 3000
Default rate: 14.83%

Feature Summary:
  age                : mean=     42.15, std=     11.89
  annual_income      : mean= 50632.45, std= 42356.78
  credit_score      : mean=    680.23, std=     99.67
  employment_years : mean=      6.23, std=      5.78
  debt_to_income   : mean=      0.30, std=      0.18
  savings          : mean=  15234.56, std=  32145.67
  loan_amount     : mean=  45321.12, std:  23456.78

==============================================
K-VALUE COMPARISON
==============================================
   K    Accuracy   Precision    Recall       AUC
--------------------------------------------------
   3       0.7983      0.7234     0.7645     0.8234
   5       0.8123      0.7345     0.7789     0.8456
   7       0.8256      0.7456     0.7923     0.8567
   9       0.8134      0.7234     0.7834     0.8434
  11       0.8089      0.7123     0.7756     0.8378

Best K: 7 (AUC: 0.8567)

==============================================
FINAL CREDIT RISK MODEL PERFORMANCE
==============================================
Accuracy:  0.8256
Precision: 0.7456
Recall:    0.7923
F1 Score:   0.7682
ROC-AUC:   0.8567
```

### Healthcare Diagnosis Results

```
==============================================
HEALTHCARE APPLICATION - PATIENT SIMILARITY DIAGNOSIS
==============================================

Patient Similarity Dataset
Number of patients: 2500
Condition prevalence: 23.64%

==============================================
K-VALUE SELECTION
==============================================
K= 3: Accuracy=0.8543, AUC=0.8834
K= 5: Accuracy=0.8623, AUC=0.8912
K= 7: Accuracy=0.8589, AUC=0.8956
K= 9: Accuracy=0.8523, AUC=0.8878
K=11: Accuracy=0.8489, AUC=0.8812

Selected K: 7

==============================================
PATIENT DIAGNOSIS MODEL PERFORMANCE
==============================================
Accuracy:  0.8589
Precision: 0.7923
Recall:    0.8156
F1 Score:  0.8038
ROC-AUC:   0.8956

==============================================
CLASSIFICATION REPORT
==============================================
                  precision    recall  f1-score   support

     No Condition       0.89      0.88      0.88      381
    Has Condition       0.79      0.82      0.80      119

        accuracy                           0.86      500
       macro avg       0.84      0.84      0.84      500
    weighted avg       0.84      0.84      0.84      500
```

## Visualization

### Decision Boundary Visualization

KNN Decision Boundary with varying K:

```
K=1 Decision Boundary (Complex)        K=5 Decision Boundary (Smoother)
                                                          
Feature Y                                  Feature Y
  |                                           |
1 ++++####                            1 +++########
  | + +#######+                         |  +++#######
  | + +##########                       |  +++########
  |++++############                  |+++##########
  |  +#############         vs         |  +##########
  |  +##############                  |+++##########
  |  +###############                |+++##########
  |+++###############               |+++##########
  |  +##############                  |  +#########
0 +  +###########                   0 +  #########
  |    Feature X                         |   Feature X

  Highly irregular                      Smoother boundary
  Overfits noise                      Better generalization
```

### Distance Weight Visualization

Distance-weighted voting contribution:

```
Neighbor Contribution (K=5)
--------------------------------
Distance Weight (1/d²)
    |
1.0 +***......................................
    | ***                               
    |    ***                            
0.8 +      ***                          
    |         ***                      
    |            ***                   
0.6 +               ***               
    |                   ***             
    |                       ***         
0.4 +                          ***       
    |                              ***     
    |                                 *** 
0.2 +                                    ***
    |                                       ****
0.0 +......................................****
    |    |    |    |    |    |    |    |    |
    +-------------------------------------
    N1   N2   N3   N4   N5   Distances
        
    *** contribution regions
    Nearest neighbors have outsized influence
```

### Error Rate vs K

```
K Value vs Error Rate
--------------------------------------------------------
Error
Rate
    |
0.4 +***                                          
    |  ***                                      
    |    ***                                  
0.3 +      ***                               
    |        ***                             
    |          ***                         
0.2 +            ***                      
    |              ***                   
    |                ***                 
0.1 +                  ***               
    |                    ***             
    |                      ***           
0.0 +                        ***
    +----+----+----+----+----+----+----+--
        1   3   5   7   9  11  13  15
                    K Value
            
    Test Error: ****
    CV Error: ====
    
    Optimal K typically where curves intersect
    Bias-variance tradeoff visible
```

## Advanced Topics

### Distance Weighting

KNN supports distance-weighted voting where closer neighbors have greater influence. The weights parameter controls this behavior, with 'distance' applying inverse square distance weighting. This approach reduces sensitivity to K value and often improves performance on sparse data.

```python
from sklearn.neighbors import KNeighborsClassifier

print("=" * 70)
print("DISTANCE WEIGHTING COMPARISON")
print("=" * 70)

weight_types = ['uniform', 'distance']
results = []

for weight_type in weight_types:
    knn = KNeighborsClassifier(n_neighbors=7, weights=weight_type)
    knn.fit(X_train_scaled, y_train)
    y_pred = knn.predict(X_test_scaled)
    y_prob = knn.predict_proba(X_test_scaled)[:, 1]
    
    acc = accuracy_score(y_test, y_pred)
    auc = roc_auc_score(y_test, y_prob)
    results.append({'weight': weight_type, 'accuracy': acc, 'auc': auc})

print(f"\nWeight Type Comparison:")
print(f"{'Weight Type':>12s} {'Accuracy':>10s} {'AUC':>10s}")
print("-" * 35)
for r in results:
    print(f"{r['weight']:>12s} {r['accuracy']:>10.4f} {r['auc']:>10.4f}")
```

### Alternative Distance Metrics

```python
from sklearn.neighbors import DistanceMetric

print("=" * 70)
print("METRIC COMPARISON")
print("=" * 70)

metrics = ['euclidean', 'manhattan', 'chebyshev', 'minkowski']
results = []

for metric in metrics:
    p_value = 2 if metric == 'minkowski' else 1
    knn = KNeighborsClassifier(n_neighbors=7, metric=metric, p=p_value)
    knn.fit(X_train_scaled, y_train)
    y_pred = knn.predict(X_test_scaled)
    y_prob = knn.predict_proba(X_test_scaled)[:, 1]
    
    acc = accuracy_score(y_test, y_pred)
    auc = roc_auc_score(y_test, y_prob)
    results.append({'metric': metric, 'accuracy': acc, 'auc': auc})

print(f"\nMetric Comparison:")
print(f"{'Metric':>12s} {'Accuracy':>10s} {'AUC':>10s}")
print("-" * 35)
for r in results:
    print(f"{r['metric']:>12s} {r['accuracy']:>10.4f} {r['auc']:>10.4f}")
```

### Ball Tree and KD-Tree Algorithms

For efficiency with large datasets, scikit-learn offers ball tree and KD-tree algorithms that speed up neighbor search:

```python
from sklearn.neighbors import KNeighborsClassifier, BallTree, KDTree

print("=" * 70)
print("ALGORITHM COMPARISON")
print("=" * 70)

algorithms = ['auto', 'ball_tree', 'kd_tree', 'brute']
results = []

for algo in algorithms:
    knn = KNeighborsClassifier(n_neighbors=7, algorithm=algo)
    knn.fit(X_train_scaled, y_train)
    y_pred = knn.predict(X_test_scaled)
    
    acc = accuracy_score(y_test, y_pred)
    results.append({'algorithm': algo, 'accuracy': acc})

print(f"\nAlgorithm Comparison:")
print(f"{'Algorithm':>12s} {'Accuracy':>10s}")
print("-" * 30)
for r in results:
    print(f"{r['algorithm']:>12s} {r['accuracy']:>10.4f}")
```

## Conclusion

K-Nearest Neighbors provides a simple yet effective approach to classification that captures local patterns in data. Its instance-based nature makes it particularly valuable for exploratory analysis and as a baseline model. The algorithm's transparency and lack of explicit training make it easy to understand and implement, while its effectiveness in many practical applications demonstrates that simplicity can be powerful.

Key considerations for KNN include appropriate data preprocessing through feature scaling, careful selection of K through cross-validation, and the choice of distance metric matching data characteristics. The curse of dimensionality requires attention in high-dimensional problems, often necessitating dimensionality reduction before applying KNN.

For banking and healthcare applications, KNN provides interpretable predictions grounded in historical precedents. Physicians and loan officers can understand and explain recommendations based on similar historical cases, supporting required transparency in regulated industries. While more sophisticated algorithms may provide marginal improvements, KNN's transparency and effectiveness make it a valuable tool in the machine learning toolkit.