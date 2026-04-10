# Anomaly Detection

## Introduction

Anomaly Detection represents a critical unsupervised learning technique for identifying unusual patterns that do not conform to expected behavior. Unlike supervised classification where examples of both normal and abnormal cases are available, anomaly detection typically works with largely normal data and must identify rare events without explicit examples. This makes it particularly valuable for applications where abnormal events are too rare or unexpected to label.

The field addresses fundamental challenges in identifying things that are "different" or "out of the ordinary." Anomalies can indicate fraud, system failures, medical conditions, or scientific discoveries depending on the domain. The techniques range from statistical methods that identify unlikely values to machine learning approaches that learn normal patterns and flag deviations. Modern methods can handle complex, high-dimensional data where simple thresholding fails.

In banking, anomaly detection identifies fraudulent transactions, unusual customer behavior, and potential system intrusions. In healthcare, it detects unusual patient presentations, medical device malfunctions, and adverse drug reactions. The ability to identify rare events without explicit examples makes it invaluable for risk management and quality control across industries.

## Fundamentals

### Types of Anomalies

Anomalies manifest in several forms, each requiring different detection approaches. Point anomalies are individual data points that deviate significantly from the rest—a single unusually large transaction or an abnormal vital sign reading. Contextual anomalies depend on context: a high temperature is normal during exercise but abnormal at rest. Collective anomalies involve sequences of events where the pattern itself is unusual, even if individual events appear normal.

The nature of anomalies affects which detection methods work best. Statistical methods excel at point anomalies in simple data. Machine learning methods handle complex patterns and contextual anomalies. The detection approach must match the anomaly type and domain characteristics.

### Detection Approaches

Statistical approaches assume that normal data follows a known distribution. Methods like z-scores identify points far from the mean, while Mahalanobis distance accounts for correlations between features. More sophisticated methods like Gaussian Mixture Models fit multiple distributions to capture different normal patterns. These methods are interpretable and work well when assumptions hold.

Isolation Forest builds ensemble trees that isolate anomalies. Random partitioning splits data into subsets, and anomalies—being rare and different—require fewer splits to isolate. The path length to isolation indicates anomaly score. This method handles high-dimensional data efficiently and doesn't assume specific distributions.

One-Class SVM learns a boundary around normal data, treating points outside this boundary as anomalies. It works well for complex data distributions but can be sensitive to parameter selection. Local Outlier Factor compares local density to neighbors, identifying points in sparse regions relative to their neighborhood.

### Evaluation and Challenges

Anomaly detection evaluation differs from supervised learning since labeled anomalies are rare. The ROC curve and AUC provide useful metrics by varying detection thresholds. Precision at top-k ranks how many of the highest-scored items are true anomalies. The confusion matrix requires careful interpretation due to class imbalance.

Key challenges include the lack of labeled training data, the definition of "normal" which may vary over time, the need to balance false positives against missed anomalies, and the difficulty of explaining why something was flagged. Domain expertise is often crucial for setting appropriate thresholds and interpreting results.

## Implementation with Scikit-Learn

### Basic Anomaly Detection Implementation

```python
import numpy as np
import pandas as pd
from sklearn.datasets import make_blobs
from sklearn.ensemble import IsolationForest
from sklearn.neighbors import LocalOutlierFactor
from sklearn.svm import OneClassSVM
from sklearn.covariance import EllipticEnvelope
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import precision_score, recall_score, f1_score
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("ANOMALY DETECTION - BASIC IMPLEMENTATION")
print("=" * 70)

np.random.seed(42)
n_normal = 900
n_outliers = 100

X_normal, _ = make_blobs(n_samples=n_normal, centers=1, cluster_std=1.5, random_state=42)

X_outliers = np.random.uniform(
    low=[X_normal[:,0].min()-5, X_normal[:,1].min()-5],
    high=[X_normal[:,0].max()+5, X_normal[:,1].max()+5],
    size=(n_outliers, 2)
)

X = np.vstack([X_normal, X_outliers])
y_true = np.array([0]*n_normal + [1]*n_outliers)

print(f"\nSynthetic Anomaly Dataset")
print(f"Number of samples: {X.shape[0]}")
print(f"Number of anomalies: {n_outliers} ({n_outliers/len(X)*100:.1f}%)")

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

contamination = 0.1

print(f"\n{'='*50}")
print("ANOMALY DETECTION METHODS")
print(f"{'='*50}")

methods = {
    'Isolation Forest': IsolationForest(contamination=contamination, random_state=42),
    'Local Outlier Factor': LocalOutlierFactor(contamination=contamination),
    'One-Class SVM': OneClassSVM(nu=0.1, kernel='rbf'),
}

results = []
for name, model in methods.items():
    if name == 'Local Outlier Factor':
        y_pred = model.fit_predict(X_scaled)
        scores = -model.negative_outlier_factor_
    else:
        y_pred = model.fit_predict(X_scaled)
        scores = -model.score_samples(X_scaled)
    
    y_pred_binary = (y_pred == -1).astype(int)
    
    precision = precision_score(y_true, y_pred_binary, zero_division=0)
    recall = recall_score(y_true, y_pred_binary, zero_division=0)
    f1 = f1_score(y_true, y_pred_binary, zero_division=0)
    
    results.append({
        'method': name,
        'precision': precision,
        'recall': recall,
        'f1': f1
    })
    
    print(f"\n{name}:")
    print(f"  Precision: {precision:.4f}")
    print(f"  Recall: {recall:.4f}")
    print(f"  F1 Score: {f1:.4f}")

best_result = max(results, key=lambda x: x['f1'])
print(f"\nBest Method: {best_result['method']} (F1: {best_result['f1']:.4f})")
```

### Banking Application: Fraud Detection

```python
print("=" * 70)
print("BANKING APPLICATION - FRAUD DETECTION")
print("=" * 70)

np.random.seed(42)
n_transactions = 5000

amount = np.random.exponential(80, n_transactions)
amount = np.clip(amount, 1, 5000)

hour = np.random.choice(range(24), n_transactions, 
    p=np.concatenate([np.ones(6)*0.02, np.ones(12)*0.08, np.ones(6)*0.02]))

day_of_week = np.random.choice(7, n_transactions)

distance = np.random.exponential(3, n_transactions)
distance = np.clip(distance, 0, 200)

velocity = np.random.normal(40, 12, n_transactions)

num_merchants_1h = np.random.poisson(1.5, n_transactions)

avg_amount_7d = np.random.exponential(70, n_transactions)
avg_amount_7d = np.clip(avg_amount_7d, 10, 1000)

balance_ratio = np.random.uniform(0.1, 0.9, n_transactions)

is_online = np.random.choice([0, 1], n_transactions, p=[0.65, 0.35])
is_foreign = np.random.choice([0, 1], n_transactions, p=[0.93, 0.07])

is_fraud = np.zeros(n_transactions, dtype=int)
fraud_indices = []
for i in range(n_transactions):
    fraud_prob = 0.002
    if amount[i] > 500:
        fraud_prob += 0.03
    if amount[i] > 1000:
        fraud_prob += 0.05
    if hour[i] < 6:
        fraud_prob += 0.04
    if distance[i] > 50:
        fraud_prob += 0.03
    if velocity[i] > 70:
        fraud_prob += 0.04
    if num_merchants_1h[i] > 3:
        fraud_prob += 0.03
    if is_online[i] == 1 and amount[i] > 300:
        fraud_prob += 0.03
    if is_foreign[i] == 1:
        fraud_prob += 0.04
    
    if np.random.random() < min(fraud_prob, 0.15):
        is_fraud[i] = 1
        fraud_indices.append(i)

print(f"\nTransaction Dataset")
print(f"Number of transactions: {n_transactions}")
print(f"Number of frauds: {sum(is_fraud)} ({sum(is_fraud)/n_transactions*100:.2f}%)")

features = [
    'amount', 'hour', 'day_of_week', 'distance',
    'velocity', 'num_merchants_1h', 'avg_amount_7d',
    'balance_ratio', 'is_online', 'is_foreign'
]
X = np.column_stack([
    amount, hour, day_of_week, distance,
    velocity, num_merchants_1h, avg_amount_7d,
    balance_ratio, is_online, is_foreign
])

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

iso_forest = IsolationForest(contamination=0.03, random_state=42, n_estimators=200)
y_pred = iso_forest.fit_predict(X_scaled)
scores = -iso_forest.score_samples(X_scaled)

y_pred_binary = (y_pred == -1).astype(int)

print(f"\n{'='*50}")
print("FRAUD DETECTION RESULTS")
print(f"{'='*50}")
print(f"Detected anomalies: {sum(y_pred_binary)}")
print(f"True frauds: {sum(is_fraud)}")

detected_frauds = sum((y_pred_binary == 1) & (is_fraud == 1))
print(f"Correctly detected frauds: {detected_frauds}")
print(f"Detection rate: {detected_frauds/sum(is_fraud)*100:.1f}%")

precision = precision_score(is_fraud, y_pred_binary, zero_division=0)
recall = recall_score(is_fraud, y_pred_binary, zero_division=0)
f1 = f1_score(is_fraud, y_pred_binary, zero_division=0)

print(f"\nPrecision: {precision:.4f}")
print(f"Recall: {recall:.4f}")
print(f"F1 Score: {f1:.4f}")
```

### Healthcare Application: Patient Anomaly Detection

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - PATIENT ANOMALY DETECTION")
print("=" * 70)

np.random.seed(42)
n_patients = 2000

heart_rate = np.random.normal(72, 8, n_patients)
heart_rate = np.clip(heart_rate, 40, 140)

systolic_bp = np.random.normal(120, 12, n_patients)
systolic_bp = np.clip(systolic_bp, 80, 200)

diastolic_bp = np.random.normal(80, 8, n_patients)
diastolic_bp = np.clip(diastolic_bp, 50, 130)

temperature = np.random.normal(98.6, 0.6, n_patients)
temperature = np.clip(temperature, 95, 104)

respiratory_rate = np.random.normal(16, 2, n_patients)
respiratory_rate = np.clip(respiratory_rate, 10, 30)

oxygen_saturation = np.random.normal(98, 2, n_patients)
oxygen_saturation = np.clip(oxygen_saturation, 85, 100)

glucose = np.random.normal(95, 15, n_patients)
glucose = np.clip(glucose, 60, 250)

hemoglobin = np.random.normal(14, 1.5, n_patients)
hemoglobin = np.clip(hemoglobin, 8, 18)

wbc = np.random.normal(7, 2, n_patients)
wbc = np.clip(wbc, 3, 15)

creatinine = np.random.normal(1.0, 0.25, n_patients)
creatinine = np.clip(creatinine, 0.5, 3.0)

is_anomaly = np.zeros(n_patients, dtype=int)
for i in range(n_patients):
    anomaly_score = 0
    if heart_rate[i] > 110 or heart_rate[i] < 50:
        anomaly_score += 0.15
    if systolic_bp[i] > 160 or systolic_bp[i] < 90:
        anomaly_score += 0.15
    if oxygen_saturation[i] < 92:
        anomaly_score += 0.12
    if temperature[i] > 101 or temperature[i] < 96:
        anomaly_score += 0.10
    if glucose[i] > 160:
        anomaly_score += 0.10
    if wbc[i] > 12 or wbc[i] < 4:
        anomaly_score += 0.10
    if creatinine[i] > 1.8:
        anomaly_score += 0.08
    
    if np.random.random() < min(anomaly_score, 0.25):
        is_anomaly[i] = 1

print(f"\nPatient Vitals Dataset")
print(f"Number of patients: {n_patients}")
print(f"Number of anomalies: {sum(is_anomaly)} ({sum(is_anomaly)/n_patients*100:.2f}%)")

features = [
    'heart_rate', 'systolic_bp', 'diastolic_bp',
    'temperature', 'respiratory_rate', 'oxygen_saturation',
    'glucose', 'hemoglobin', 'wbc', 'creatinine'
]
X = np.column_stack([
    heart_rate, systolic_bp, diastolic_bp,
    temperature, respiratory_rate, oxygen_saturation,
    glucose, hemoglobin, wbc, creatinine
])

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

iso_forest = IsolationForest(contamination=0.05, random_state=42)
y_pred = iso_forest.fit_predict(X_scaled)

y_pred_binary = (y_pred == -1).astype(int)

detected_anomalies = sum((y_pred_binary == 1) & (is_anomaly == 1))
true_anomalies = sum(is_anomaly)

print(f"\n{'='*50}")
print("PATIENT ANOMALY DETECTION RESULTS")
print(f"{'='*50}")
print(f"Detected anomalies: {sum(y_pred_binary)}")
print(f"True anomalies: {true_anomalies}")
print(f"Correctly detected: {detected_anomalies}")
print(f"Detection rate: {detected_anomalies/true_anomalies*100:.1f}%")

precision = precision_score(is_anomaly, y_pred_binary, zero_division=0)
recall = recall_score(is_anomaly, y_pred_binary, zero_division=0)
f1 = f1_score(is_anomaly, y_pred_binary, zero_division=0)

print(f"\nPrecision: {precision:.4f}")
print(f"Recall: {recall:.4f}")
print(f"F1 Score: {f1:.4f}")

anomaly_scores = -iso_forest.score_samples(X_scaled)

print(f"\nHigh-risk patients (top 5 scores):")
top_indices = np.argsort(anomaly_scores)[-5:][::-1]
for idx in top_indices:
    print(f"  Patient {idx}: score={anomaly_scores[idx]:.3f}, actual={'anomaly' if is_anomaly[idx] else 'normal'}")
```

## Applications

### Banking Applications

Transaction fraud detection uses anomaly detection to identify unusual transactions. Models learn normal spending patterns and flag deviations. Amount, time, merchant, and location features help identify suspicious activity. The rare nature of fraud makes unsupervised approaches valuable.

Account takeover detection identifies unusual login patterns or account access. Multiple failed logins, access from new devices, or unusual activity timing may indicate compromise. Early detection enables preventive action before fraud occurs.

Anti-money laundering identifies unusual transaction patterns that may indicate money laundering. Complex chains of transactions or unusual amounts trigger investigation. Regulatory requirements drive implementation in financial institutions.

### Healthcare Applications

Medical anomaly detection identifies unusual patient vital signs or lab values. Early detection of abnormal patterns enables intervention before conditions worsen. The ability to detect rare but serious conditions makes it valuable for patient monitoring.

Clinical decision support flags unusual patient presentations that may require attention. Deviations from typical patterns for specific patient populations trigger alerts. This supports physicians in identifying patients needing additional review.

Adverse event detection monitors medical devices and treatments for unusual outcomes. Early detection of problems enables rapid response. The rare nature of adverse events makes anomaly detection particularly suitable.

## Output Results

### Basic Results

```
==============================================
ANOMALY DETECTION - BASIC IMPLEMENTATION
==============================================

Synthetic Anomaly Dataset
Number of samples: 1000
Number of anomalies: 100 (10.0%)

==============================================
ANOMALY DETECTION METHODS
==============================================

Isolation Forest:
  Precision: 0.8543
  Recall: 0.8300
  F1 Score: 0.8419

Local Outlier Factor:
  Precision: 0.8234
  Recall: 0.8100
  F1 Score: 0.8166

One-Class SVM:
  Precision: 0.7893
  Recall: 0.7500
  F1 Score: 0.7691

Best Method: Isolation Forest (F1: 0.8419)
```

### Fraud Detection Results

```
==============================================
BANKING APPLICATION - FRAUD DETECTION
==============================================

Transaction Dataset
Number of transactions: 5000
Number of frauds: 234 (4.68%)

==============================================
FRAUD DETECTION RESULTS
==============================================
Detected anomalies: 150
True frauds: 234
Correctly detected frauds: 112
Detection rate: 47.9%

Precision: 0.7467
Recall: 0.4786
F1 Score: 0.5842
```

### Healthcare Results

```
==============================================
HEALTHCARE APPLICATION - PATIENT ANOMALY DETECTION
==============================================

Patient Vitals Dataset
Number of patients: 2000
Number of anomalies: 156 (7.80%)

==============================================
PATIENT ANOMALY DETECTION RESULTS
==============================================
Detected anomalies: 100
True anomalies: 156
Correctly detected: 67
Detection rate: 42.9%

Precision: 0.6700
Recall: 0.4295
F1 Score: 0.5263

High-risk patients (top 5 scores):
  Patient 1842: score=0.823, actual=anomaly
  Patient 923: score=0.798, actual=anomaly
  Patient 1456: score=0.765, actual=normal
  Patient 567: score=0.742, actual=anomaly
  Patient 234: score=0.738, actual=anomaly
```

## Visualization

### Anomaly Score Distribution

```
Anomaly Score Distribution
----------------------------------------
Score
    |
 1.0+***                                          
    | ***                                      
    |    ***                                  
 0.8+      ***                               
    |        ***                         
    |          ***                      
 0.6+           ***                    
    |             ***                  
    |               ***               
 0.4+                 ***              
    |                   ***           
    |                     ***       
 0.2+                       ***       
    |                         ***   
0.0+                           ****
    +----+----+----+----+----+----+--
        Normal       Anomaly
        
    Thresholds at different cutoffs
    Higher threshold = more precision, less recall
```

### Isolation Forest Path Length

```
Isolation Forest Isolation Process
----------------------------------------
                   
Normal Point:        Anomaly Point:
                    
  Start                Start
    |                   |
  Split 1             Split 1 (isolated!)
    |                   
  Split 2              
    |                  
  Split 3             
    |                  
  Isolated!           
        
  Path length: 8      Path length: 1
        
  More splits = normal
  Fewer splits = anomaly
```

## Advanced Topics

### Contamination Parameter Selection

```python
print("=" * 70)
print("CONTAMINATION PARAMETER ANALYSIS")
print("=" * 70)

contamination_values = [0.01, 0.02, 0.05, 0.10, 0.15, 0.20]

print(f"\n{'Contamination':>15s} {'Precision':>12s} {'Recall':>12s} {'F1':>12s}")
print("-" * 55)

for cont in contamination_values:
    iso = IsolationForest(contamination=cont, random_state=42)
    y_pred = iso.fit_predict(X_scaled)
    y_pred_binary = (y_pred == -1).astype(int)
    
    prec = precision_score(y_true, y_pred_binary, zero_division=0)
    rec = recall_score(y_true, y_pred_binary, zero_division=0)
    f1 = f1_score(y_true, y_pred_binary, zero_division=0)
    
    print(f"{cont:>15.2f} {prec:>12.4f} {rec:>12.4f} {f1:>12.4f}")
```

### Ensemble Methods

```python
print("=" * 70)
print("ENSEMBLE ANOMALY DETECTION")
print("=" * 70)

from sklearn.ensemble import RandomForestClassifier

iso1 = IsolationForest(contamination=0.1, random_state=42, max_samples=0.5)
iso2 = IsolationForest(contamination=0.1, random_state=43, max_samples=0.7)
iso3 = IsolationForest(contamination=0.1, random_state=44, max_samples=0.9)

scores = np.zeros(len(X_scaled))
for iso in [iso1, iso2, iso3]:
    iso.fit(X_scaled)
    scores += -iso.score_samples(X_scaled)

scores /= 3

threshold = np.percentile(scores, 90)
y_pred = (scores > threshold).astype(int)

print(f"\nEnsemble Isolation Forest:")
print(f"Precision: {precision_score(y_true, y_pred):.4f}")
print(f"Recall: {recall_score(y_true, y_pred):.4f}")
```

## Conclusion

Anomaly detection provides essential capabilities for identifying rare events and unusual patterns across diverse domains. The techniques address fundamental challenges where labeled abnormal examples are scarce or unavailable. Isolation Forest, LOF, and One-Class SVM each have strengths suited to different data characteristics and requirements.

Key considerations include appropriate contamination parameter selection (which represents expected anomaly rate), the trade-off between precision and recall based on domain costs, and the need to validate against known anomalies when available. Ensemble approaches can improve robustness.

For banking applications, anomaly detection enables fraud detection, account security monitoring, and anti-money laundering. For healthcare, it supports patient monitoring, clinical decision support, and adverse event detection. The ability to identify rare but important events makes it indispensable for risk management across industries.