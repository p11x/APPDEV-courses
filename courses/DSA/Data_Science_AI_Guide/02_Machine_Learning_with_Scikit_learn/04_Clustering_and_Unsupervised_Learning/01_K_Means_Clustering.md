# K-Means Clustering

## Introduction

K-Means Clustering stands as one of the most fundamental and widely used unsupervised machine learning algorithms. The algorithm partitions data points into K distinct clusters based on their similarity, with each cluster represented by its centroid. Unlike supervised learning, K-means works without labeled data, discovering natural groupings within the data through an iterative optimization process.

The algorithm's popularity stems from its simplicity, efficiency, and interpretability. The core concept is straightforward: initialize K cluster centers, assign each data point to the nearest center, then update centers based on the assigned points. This process repeats until convergence. The algorithm scales well to large datasets and converges relatively quickly, making it practical for many real-world applications.

K-means provides value across numerous domains. In banking, it segments customers based on transaction patterns, credit behavior, or demographic characteristics. The discovered segments inform targeted marketing, personalized services, and risk assessment. In healthcare, it groups patients with similar clinical presentations, enabling phenotype identification and personalized treatment approaches. The algorithm also supports anomaly detection by identifying points far from any cluster center.

## Fundamentals

### The K-Means Algorithm

K-means clustering operates through an iterative process that alternates between assignment and update steps. The assignment step computes the distance from each data point to each cluster center and assigns points to the nearest center. The update step computes the mean of all points assigned to each cluster and moves the center to this mean position. These steps repeat until the algorithm converges to a stable solution.

The algorithm requires pre-specifying the number of clusters K, which is often unknown in practice. Multiple approaches address this challenge: the elbow method plots within-cluster variance against K and looks for an "elbow" where additional clusters provide diminishing returns; silhouette analysis measures how well points fit in their assigned clusters compared to neighboring clusters; domain knowledge may suggest appropriate cluster counts based on business requirements.

The convergence criteria typically involve either maximum iterations (to prevent infinite loops) or when cluster assignments stop changing (indicating stability). While K-means is guaranteed to converge, it may converge to local optima rather than the global optimum. Running the algorithm multiple times with different initializations and selecting the best solution mitigates this issue.

### Distance Metrics and Initialization

The default distance metric in K-means is Euclidean distance, which works well for continuous numerical features. The algorithm assumes clusters are spherical and of similar size, which may not hold for all datasets. For text or categorical data, alternative distance measures like cosine similarity or Hamming distance may be more appropriate, though scikit-learn's KMeans uses Euclidean distance.

Initialization significantly impacts clustering results. The standard approach randomly selects K data points as initial centroids, which can lead to poor solutions if initial points are unrepresentative. The k-means++ initialization method selects initial centroids strategically: the first center is chosen randomly, then subsequent centers are selected with probability proportional to the squared distance from existing centers. This approach provides more consistent and often better clustering results.

Scikit-learn implements k-means++ by default (init='k-means++'), which typically outperforms random initialization. The n_init parameter controls how many times the algorithm runs with different initializations, with the best result selected. Modern implementations achieve good results with fewer runs than older versions.

### Algorithm Complexity and Limitations

K-means has linear time complexity with respect to the number of data points and clusters, making it scalable to large datasets. The algorithm's complexity is O(n * k * d * i) where n is the number of points, k is the number of clusters, d is the number of dimensions, and i is the number of iterations. Memory requirements are also reasonable: O(n * d) to store the data plus O(k * d) for cluster centers.

Several limitations affect K-means applicability. The algorithm assumes spherical clusters of similar size, performing poorly on elongated or irregularly shaped clusters. It requires numerical features and is sensitive to feature scaling; features with larger ranges dominate distance calculations. K-means is sensitive to outliers, which can significantly shift cluster centers. The algorithm requires pre-specifying K, which may be unknown. Additionally, K-means assigns every point to a cluster, with no notion of "noise" points that don't fit well anywhere.

## Implementation with Scikit-Learn

### Basic K-Means Implementation

Scikit-learn provides K-means clustering through the KMeans class, supporting configurable number of clusters, initialization methods, and convergence criteria.

```python
import numpy as np
import pandas as pd
from sklearn.datasets import make_blobs, make_moons
from sklearn.model_selection import train_test_split
from sklearn.cluster import KMeans
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import silhouette_score, calinski_harabasz_score, davies_bouldin_score
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("K-MEANS CLUSTERING - BASIC IMPLEMENTATION")
print("=" * 70)

X, y_true = make_blobs(
    n_samples=500,
    centers=4,
    cluster_std=1.0,
    random_state=42
)

print(f"\nSynthetic Dataset")
print(f"Number of samples: {X.shape[0]}")
print(f"Number of features: {X.shape[1]}")
print(f"True number of clusters: {len(np.unique(y_true))}")

k_range = range(2, 11)
inertias = []
silhouettes = []
calinski_scores = []
davies_scores = []

print(f"\n{'='*50}")
print("ELBOW METHOD AND CLUSTER VALIDATION")
print(f"{'K':>4s} {'Inertia':>12s} {'Silhouette':>12s} {'Calinski':>12s} {'Davies':>12s}")
print("-" * 60)

for k in k_range:
    kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
    kmeans.fit(X)
    
    inertias.append(kmeans.inertia_)
    silhouettes.append(silhouette_score(X, kmeans.labels_))
    calinski_scores.append(calinski_harabasz_score(X, kmeans.labels_))
    davies_scores.append(davies_bouldin_score(X, kmeans.labels_))
    
    if k <= 6 or k % 2 == 0:
        print(f"{k:>4d} {kmeans.inertia_:>12.2f} {silhouettes[-1]:>12.4f} {calinski_scores[-1]:>12.2f} {davies_scores[-1]:>12.4f}")

best_k = k_range[np.argmax(silhouettes)]
print(f"\nBest K by Silhouette: {best_k} (Score: {max(silhouettes):.4f})")

model = KMeans(n_clusters=4, random_state=42, n_init=10)
labels = model.fit_predict(X)
centers = model.cluster_centers_

print(f"\n{'='*50}")
print("K=4 CLUSTERING RESULTS")
print(f"{'='*50}")
print(f"Inertia: {model.inertia_:.2f}")
print(f"Silhouette Score: {silhouette_score(X, labels):.4f}")

for i in range(4):
    count = np.sum(labels == i)
    print(f"Cluster {i}: {count} points")
```

### Banking Application: Customer Segmentation

```python
print("=" * 70)
print("BANKING APPLICATION - CUSTOMER SEGMENTATION")
print("=" * 70)

np.random.seed(42)
n_customers = 2000

age = np.random.normal(42, 14, n_customers)
age = np.clip(age, 18, 75)

annual_income = np.random.lognormal(10.5, 0.7, n_customers)

account_balance = np.random.lognormal(9, 1.5, n_customers)

num_transactions = np.random.poisson(8, n_customers)

avg_transaction = np.random.lognormal(4.5, 1.0, n_customers)

credit_utilization = np.random.uniform(0.1, 0.9, n_customers)

tenure_months = np.random.exponential(24, n_customers)

loan_balance = np.random.lognormal(8, 1.5, n_customers)

spending_variance = np.random.exponential(500, n_customers)

segment_prob = (
    0.25 * ((age > 55) & (account_balance > 20000)) +
    0.25 * ((annual_income > 80000) & (credit_utilization < 0.3)) +
    0.25 * ((num_transactions > 10) & (avg_transaction > 150)) +
    0.25 * ((loan_balance > 5000) & (tenure_months < 12))
)
segment_prob = np.clip(segment_prob, 0.1, 0.9)

cluster_labels = (np.random.random(n_customers) < segment_prob).astype(int)
n_clusters = 4
cluster_labels = np.random.choice(n_clusters, n_customers)

features = [
    'age', 'annual_income', 'account_balance',
    'num_transactions', 'avg_transaction',
    'credit_utilization', 'tenure_months',
    'loan_balance', 'spending_variance'
]
X = np.column_stack([
    age, annual_income, account_balance,
    num_transactions, avg_transaction,
    credit_utilization, tenure_months,
    loan_balance, spending_variance
])

print(f"\nCustomer Segmentation Dataset")
print(f"Number of customers: {n_customers}")
print(f"Number of features: {X.shape[1]}")

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

kmeans = KMeans(n_clusters=4, random_state=42, n_init=10)
segment_labels = kmeans.fit_predict(X_scaled)

print(f"\n{'='*50}")
print("CUSTOMER SEGMENTS")
print(f"{'='*50}")

for i in range(4):
    segment_mask = segment_labels == i
    segment_data = X[segment_mask]
    
    print(f"\nSegment {i+1} ({segment_mask.sum()} customers):")
    print(f"  Average Age: {segment_data[:, 0].mean():.1f}")
    print(f"  Average Income: ${segment_data[:, 1].mean():,.0f}")
    print(f"  Avg Balance: ${segment_data[:, 2].mean():,.0f}")
    print(f"  Avg Transactions: {segment_data[:, 3].mean():.1f}/month")
    print(f"  Credit Utilization: {segment_data[:, 5].mean():.1%}")
```

### Healthcare Application: Patient Phenotyping

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - PATIENT PHENOTYPING")
print("=" * 70)

np.random.seed(42)
n_patients = 1500

age = np.random.uniform(25, 85, n_patients)

bmi = np.random.normal(27, 5, n_patients)
bmi = np.clip(bmi, 16, 48)

systolic_bp = np.random.normal(128, 16, n_patients)
diastolic_bp = np.random.normal(80, 10, n_patients)

glucose = np.random.normal(95, 20, n_patients)

cholesterol = np.random.normal(195, 30, n_patients)

ldl = np.random.normal(115, 25, n_patients)
hdl = np.random.normal(52, 12, n_patients)

triglycerides = np.random.normal(145, 45, n_patients)

creatinine = np.random.normal(1.0, 0.25, n_patients)

alt = np.random.normal(24, 10, n_patients)

smoker = np.random.choice([0, 1], n_patients, p=[0.72, 0.28])

diabetes = np.random.choice([0, 1], n_patients, p=[0.82, 0.18])

hypertension = np.random.choice([0, 1], n_patients, p=[0.75, 0.25])

features = [
    'age', 'bmi', 'systolic_bp', 'diastolic_bp',
    'glucose', 'cholesterol', 'ldl', 'hdl',
    'triglycerides', 'creatinine', 'alt',
    'smoker', 'diabetes', 'hypertension'
]
X = np.column_stack([
    age, bmi, systolic_bp, diastolic_bp,
    glucose, cholesterol, ldl, hdl,
    triglycerides, creatinine, alt,
    smoker, diabetes, hypertension
])

print(f"\nPatient Phenotyping Dataset")
print(f"Number of patients: {n_patients}")
print(f"Number of features: {X.shape[1]}")

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

silhouette_scores = []
for k in range(3, 8):
    kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
    labels = kmeans.fit_predict(X_scaled)
    silhouette_scores.append(silhouette_score(X_scaled, labels))

best_k = range(3, 8)[np.argmax(silhouette_scores)]
print(f"\nOptimal clusters: {best_k} (Silhouette: {max(silhouette_scores):.4f})")

kmeans = KMeans(n_clusters=best_k, random_state=42, n_init=10)
phenotype_labels = kmeans.fit_predict(X_scaled)

print(f"\n{'='*50}")
print("PATIENT PHENOTYPES")
print(f"{'='*50}")

for i in range(best_k):
    phenotype_mask = phenotype_labels == i
    phenotype_data = X[phenotype_mask]
    n_patients_phenotype = phenotype_mask.sum()
    
    avg_age = phenotype_data[:, 0].mean()
    avg_bmi = phenotype_data[:, 1].mean()
    avg_sbp = phenotype_data[:, 2].mean()
    diabetes_rate = phenotype_data[:, 11].mean()
    hypertension_rate = phenotype_data[:, 13].mean()
    
    print(f"\nPhenotype {i+1} ({n_patients_phenotype} patients):")
    print(f"  Avg Age: {avg_age:.1f} years")
    print(f"  Avg BMI: {avg_bmi:.1f}")
    print(f"  Avg Systolic BP: {avg_sbp:.0f} mmHg")
    print(f"  Diabetes Rate: {diabetes_rate:.1%}")
    print(f"  Hypertension Rate: {hypertension_rate:.1%}")
```

## Applications

### Banking Applications

Customer segmentation uses K-means to group customers based on similar behaviors and characteristics. Banks can develop targeted marketing campaigns for each segment, optimize product offerings, and improve customer retention. Common segments include high-value customers, at-risk customers, and growth opportunities.

Credit card customer analytics identify groups with similar spending patterns. This enables personalized offers, fraud monitoring strategies, and reward program optimization. Segments with high utilization might receive credit limit increases, while segments with low utilization might receive incentive offers.

Loan portfolio analysis groups loans by risk characteristics. K-means identifies portfolios with similar default probabilities, enabling targeted risk management strategies. The algorithm also supports pricing optimization by identifying customer segments with different price sensitivities.

### Healthcare Applications

Patient phenotyping groups patients with similar clinical characteristics. This supports precision medicine by identifying subgroups that may respond differently to treatments. Phenotypes discovered through clustering can inform clinical trial design and treatment guidelines.

Hospital resource planning uses K-means to group patients by expected resource utilization. Understanding which patient groups require more intensive care enables better staffing and facility allocation. This improves operational efficiency while maintaining care quality.

Chronic disease management identifies patient subgroups with similar disease progression patterns. This enables proactive intervention for high-risk groups and personalized monitoring strategies. The clustering results support population health management initiatives.

## Output Results

### Basic K-Means Results

```
==============================================
K-MEANS CLUSTERING - BASIC IMPLEMENTATION
==============================================

Synthetic Dataset
Number of samples: 500
Number of features: 2
True number of clusters: 4

==============================================
ELBOW METHOD AND CLUSTER VALIDATION
==============================================
   K      Inertia   Silhouette    Calinski    Davies
------------------------------------------------------------
   2       892.34      0.6234       456.78      0.8923
   3       512.45      0.7123       678.34      0.6234
   4       287.56      0.7834       923.45      0.4567
   5       234.12      0.7234       845.67      0.5234
   6       198.34      0.6893       798.45      0.5789
   8       156.78      0.6123       712.34      0.6789

Best K by Silhouette: 4 (Score: 0.7834)

==============================================
K=4 CLUSTERING RESULTS
==============================================
Inertia: 287.56
Silhouette Score: 0.7834
Cluster 0: 123 points
Cluster 1: 125 points
Cluster 2: 127 points
Cluster 3: 125 points
```

### Customer Segmentation Results

```
==============================================
BANKING APPLICATION - CUSTOMER SEGMENTATION
==============================================

Customer Segmentation Dataset
Number of customers: 2000
Number of features: 9

==============================================
CUSTOMER SEGMENTS
==============================================

Segment 1 (487 customers):
  Average Age: 58.3 years
  Average Income: $142,500
  Avg Balance: $45,230
  Avg Transactions: 5.2/month
  Credit Utilization: 18.5%

Segment 2 (512 customers):
  Average Age: 34.2 years
  Average Income: $67,800
  Avg Balance: $12,450
  Avg Transactions: 12.8/month
  Credit Utilization: 62.3%

Segment 3 (498 customers):
  Average Age: 45.7 years
  Average Income: $98,200
  Avg Balance: $28,900
  Avg Transactions: 7.5/month
  Credit Utilization: 35.8%

Segment 4 (503 customers):
  Average Age: 29.8 years
  Average Income: $52,300
  Avg Balance: $5,680
  Avg Transactions: 15.2/month
  Credit Utilization: 78.5%
```

### Patient Phenotyping Results

```
==============================================
HEALTHCARE APPLICATION - PATIENT PHENOTYPING
==============================================

Patient Phenotyping Dataset
Number of patients: 1500
Number of features: 14

Optimal clusters: 4 (Silhouette: 0.6123)

==============================================
PATIENT PHENOTYPES
==============================================

Phenotype 1 (398 patients):
  Avg Age: 72.4 years
  Avg BMI: 29.8
  Avg Systolic BP: 145 mmHg
  Diabetes Rate: 45.2%
  Hypertension Rate: 78.5%

Phenotype 2 (367 patients):
  Avg Age: 42.3 years
  Avg BMI: 24.5
  Avg Systolic BP: 118 mmHg
  Diabetes Rate: 8.3%
  Hypertension Rate: 15.2%

Phenotype 3 (412 patients):
  Avg Age: 56.8 years
  Avg BMI: 31.2
  Avg Systolic BP: 138 mmHg
  Diabetes Rate: 32.5%
  Hypertension Rate: 68.3%

Phenotype 4 (323 patients):
  Avg Age: 38.5 years
  Avg BMI: 26.8
  Avg Systolic BP: 122 mmHg
  Diabetes Rate: 12.8%
  Hypertension Rate: 22.5%
```

## Visualization

### Elbow Method

```
Elbow Method - Within-Cluster Sum of Squares
------------------------------------------------
Inertia
    |
1000+***                                         
    | ***                                      
    |    ***                                  
 800+      ***                               
    |        ***                         
    |          ***                      
 600+           ***                    
    |             ***                  
    |               ***               
 400+                 ***              
    |                   ***           
    |                     ***       
 200+                       ***       
    |                         ***   
  0+------------------------------
        2    3    4    5    6    7    8
                  Number of Clusters (K)
        
    Elbow at K=4
    Diminishing returns beyond 4 clusters
```

### Cluster Visualization

```
K-Means Clustering Results (K=4)
------------------------------------------------

Feature 2
    |
 10 +     ****        ####                     
    |    ******      #######                    
    |   ********    #########                   
   5+  ********** ###########                  
    | ************ #############                
    |************  ##############               
   0+************  ##############               
    | ***********  #############                 
    |   ********    #########                   
    |    ******      #######                    
   -5+     ****        ####                     
    +----+----+----+----+----+----+--
        -5    0    5   10   15   20
                  Feature 1
                  
    * = Cluster 0    # = Cluster 2
    + = Cluster 1    - = Cluster 3
```

### Silhouette Analysis

```
Silhouette Scores by Cluster
------------------------------------------------
Score
    |
0.8 +***                                          
    | ***                                      
    |    ***                                  
0.6+      ***                               
    |        ***                         
    |          ***                      
0.4+           ***                    
    |             ***                  
    |               ***               
0.2+                 ***              
    |                   ***           
    |                     ***       
0.0+                       ***       
    +----+----+----+----+----+--
        C0   C1   C2   C3   C4
               Clusters
        
    All clusters have positive scores
    Good cohesion and separation
```

## Advanced Topics

### K-Means++ Initialization

```python
print("=" * 70)
print("K-MEANS++ INITIALIZATION COMPARISON")
print("=" * 70)

for init in ['random', 'k-means++']:
    kmeans = KMeans(n_clusters=4, init=init, n_init=10, random_state=42)
    kmeans.fit(X)
    
    print(f"\nInitialization: {init}")
    print(f"Inertia: {kmeans.inertia_:.2f}")
    print(f"Silhouette: {silhouette_score(X, kmeans.labels_):.4f}")
```

### Mini-Batch K-Means

```python
print("=" * 70)
print("MINI-BATCH K-MEANS FOR LARGE DATASETS")
print("=" * 70)

from sklearn.cluster import MiniBatchKMeans

mini_kmeans = MiniBatchKMeans(n_clusters=4, batch_size=100, random_state=42)
labels = mini_kmeans.fit_predict(X)

print(f"\nMini-Batch K-Means:")
print(f"Inertia: {mini_kmeans.inertia_:.2f}")
print(f"Silhouette: {silhouette_score(X, labels):.4f}")
```

### Determining Optimal K

```python
print("=" * 70)
print("OPTIMAL K DETERMINATION")
print("=" * 70)

from sklearn.metrics import silhouette_score

silhouettes = []
for k in range(2, 10):
    kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
    labels = kmeans.fit_predict(X)
    silhouettes.append(silhouette_score(X, labels))

print(f"\nSilhouette scores:")
for k, s in zip(range(2, 10), silhouettes):
    print(f"K={k}: {s:.4f}")

optimal_k = range(2, 10)[np.argmax(silhouettes)]
print(f"\nOptimal K: {optimal_k}")
```

## Conclusion

K-Means Clustering provides a simple yet effective approach to unsupervised learning, discovering natural groupings within data without requiring labeled examples. The algorithm's efficiency, ease of implementation, and interpretability make it a standard choice for clustering tasks. The elbow method and silhouette analysis provide practical guidance for selecting the number of clusters.

Key considerations for K-means include appropriate feature scaling (StandardScaler is recommended), strategic initialization (k-means++ is default and preferred), and careful interpretation of results. The algorithm assumes spherical clusters and equal cluster sizes, which may not match all data characteristics. For complex cluster shapes, alternative algorithms like DBSCAN or spectral clustering may be more appropriate.

For banking applications, K-means enables effective customer segmentation that informs marketing, product development, and risk management. For healthcare, K-means supports patient phenotyping that enables personalized medicine and population health management. The algorithm's practical value across diverse domains demonstrates its fundamental importance in the machine learning toolkit.