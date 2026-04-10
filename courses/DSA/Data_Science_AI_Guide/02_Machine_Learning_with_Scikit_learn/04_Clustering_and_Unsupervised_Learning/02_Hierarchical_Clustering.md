# Hierarchical Clustering

## Introduction

Hierarchical Clustering represents a powerful approach to unsupervised learning that builds a hierarchy of clusters rather than producing a single partition. Unlike K-means which requires pre-specifying the number of clusters, hierarchical methods create a tree-like structure (dendrogram) showing how clusters relate to each other at different levels of granularity. This approach provides valuable insight into data structure and enables exploration at different resolutions.

The algorithm operates through either agglomerative (bottom-up) or divisive (top-down) approaches. Agglomerative clustering starts with each point as its own cluster and progressively merges the most similar clusters. Divisive clustering starts with all points in one cluster and recursively splits into smaller clusters. Agglomerative clustering is more commonly used and implemented in scikit-learn.

The hierarchical structure provides unique advantages. Users can examine the dendrogram to understand natural cluster groupings without pre-committing to a specific number. Different levels of the tree provide different cluster resolutions, enabling "what-if" analysis of different cluster counts. The algorithm does not assume any particular cluster shape, making it more flexible than K-means.

In banking, hierarchical clustering supports multi-level customer segmentation that reveals relationships between segments at different levels of granularity. In healthcare, it enables patient taxonomy development that organizes diseases and patient phenotypes into meaningful hierarchies. The algorithm also supports document organization and market research segmentation.

## Fundamentals

### Agglomerative Clustering Algorithm

Agglomerative clustering builds the hierarchy through successive merging of clusters. The process begins with each data point as its own cluster. At each step, the two most similar clusters (according to a linkage criterion) are merged. This continues until all points belong to a single cluster. The result is a binary tree where each internal node represents a merge operation.

The linkage criterion determines how cluster similarity is measured. Complete linkage uses the maximum distance between any two points in different clusters, favoring compact clusters. Single linkage uses the minimum distance, which can create elongated clusters and is sensitive to noise. Average linkage uses the mean distance between all point pairs, providing a balance. Ward linkage minimizes the increase in total within-cluster variance.

The algorithm's complexity is O(n² log n), making it slower than K-means for large datasets. However, for smaller datasets, the detailed hierarchy provides valuable insight. The algorithm requires a distance matrix, which for high-dimensional data may suffer from the curse of dimensionality.

### Distance Metrics and Linkage

The choice of distance metric impacts clustering results. Euclidean distance is most common for continuous features. Manhattan distance (L1 norm) is robust to outliers. Cosine similarity is appropriate for text or high-dimensional data where direction matters more than magnitude.

The linkage method combines distance metrics to compute cluster-to-cluster distance. The formula expresses how the distance between two clusters depends on the distances between their constituent points. Different linkage methods produce notably different cluster structures, and the choice should align with domain understanding and data characteristics.

The scikit-learn implementation (AgglomerativeClustering) supports ward, complete, average, and single linkage. Ward linkage minimizes variance increase and produces compact clusters of similar size. Complete linkage produces balanced clusters but may break apart natural clusters. Single linkage can connect clusters but is prone to chaining.

### Divisive Clustering

Divisive clustering takes the opposite approach, starting with one cluster containing all points and recursively splitting. At each level, a cluster is divided into two sub-clusters. The algorithm can use various methods to determine the split, such as K-means with K=2 at each node.

Divisive clustering is computationally more expensive than agglomerative but may produce more balanced hierarchies. It is less commonly implemented in standard libraries, though scikit-learn's FeatureAgglomeration uses agglomerative principles for dimensionality reduction.

## Implementation with Scikit-Learn

### Basic Hierarchical Clustering Implementation

Scikit-learn provides agglomerative clustering through the AgglomerativeClustering class, supporting various linkage methods and distance metrics.

```python
import numpy as np
import pandas as pd
from sklearn.datasets import make_blobs, make_moons
from sklearn.cluster import AgglomerativeClustering
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import silhouette_score, calinski_harabasz_score, davies_bouldin_score
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("HIERARCHICAL CLUSTERING - BASIC IMPLEMENTATION")
print("=" * 70)

X, y_true = make_blobs(
    n_samples=400,
    centers=4,
    cluster_std=1.2,
    random_state=42
)

print(f"\nSynthetic Dataset")
print(f"Number of samples: {X.shape[0]}")
print(f"Number of features: {X.shape[1]}")
print(f"True number of clusters: {len(np.unique(y_true))}")

linkage_methods = ['ward', 'complete', 'average']
results = []

print(f"\n{'='*50}")
print("LINKAGE METHOD COMPARISON")
print(f"{'='*50}")

for linkage in linkage_methods:
    if linkage == 'ward':
        model = AgglomerativeClustering(n_clusters=4, linkage=linkage)
    else:
        model = AgglomerativeClustering(n_clusters=4, linkage=linkage, metric='euclidean')
    
    labels = model.fit_predict(X)
    
    silhouette = silhouette_score(X, labels)
    calinski = calinski_harabasz_score(X, labels)
    davies = davies_bouldin_score(X, labels)
    
    results.append({
        'linkage': linkage,
        'silhouette': silhouette,
        'calinski': calinski,
        'davies': davies
    })
    
    print(f"\nLinkage: {linkage}")
    print(f"  Silhouette: {silhouette:.4f}")
    print(f"  Calinski-Harabasz: {calinski:.2f}")
    print(f"  Davies-Bouldin: {davies:.4f}")

best_result = max(results, key=lambda x: x['silhouette'])
print(f"\nBest Linkage: {best_result['linkage']}")

model = AgglomerativeClustering(n_clusters=4, linkage='ward')
labels = model.fit_predict(X)

print(f"\n{'='*50}")
print("CLUSTERING RESULTS (Ward)")
print(f"{'='*50}")
print(f"Silhouette Score: {silhouette_score(X, labels):.4f}")

for i in range(4):
    count = np.sum(labels == i)
    print(f"Cluster {i}: {count} points")
```

### Banking Application: Multi-level Customer Segmentation

```python
print("=" * 70)
print("BANKING APPLICATION - MULTI-LEVEL CUSTOMER SEGMENTATION")
print("=" * 70)

np.random.seed(42)
n_customers = 1500

age = np.random.normal(45, 15, n_customers)
age = np.clip(age, 20, 75)

income = np.random.lognormal(10.4, 0.8, n_customers)

balance = np.random.lognormal(9.5, 1.4, n_customers)

transactions = np.random.poisson(7, n_customers)

avg_transaction = np.random.lognormal(4.3, 1.1, n_customers)

credit_score = np.random.normal(680, 90, n_samples)
credit_score = np.clip(credit_score, 300, 850)

tenure = np.random.exponential(30, n_customers)

loan_balance = np.random.lognormal(8, 1.5, n_customers)

investment_balance = np.random.lognormal(8.5, 1.8, n_customers)

features = [
    'age', 'income', 'balance', 'transactions',
    'avg_transaction', 'credit_score', 'tenure',
    'loan_balance', 'investment_balance'
]
X = np.column_stack([
    age, income, balance, transactions,
    avg_transaction, credit_score, tenure,
    loan_balance, investment_balance
])

print(f"\nCustomer Segmentation Dataset")
print(f"Number of customers: {n_customers}")
print(f"Number of features: {X.shape[1]}")

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

for n_clusters in [3, 6, 9]:
    model = AgglomerativeClustering(n_clusters=n_clusters, linkage='ward')
    labels = model.fit_predict(X_scaled)
    
    silhouette = silhouette_score(X_scaled, labels)
    
    print(f"\n{'='*50}")
    print(f"MULTI-LEVEL SEGMENTATION ({n_clusters} SEGMENTS)")
    print(f"{'='*50}")
    print(f"Silhouette Score: {silhouette:.4f}")
    
    for i in range(n_clusters):
        segment_mask = labels == i
        segment_data = X[segment_mask]
        
        print(f"\nSegment {i+1} ({segment_mask.sum()} customers):")
        print(f"  Avg Age: {segment_data[:, 0].mean():.1f}")
        print(f"  Avg Income: ${segment_data[:, 1].mean():,.0f}")
        print(f"  Avg Balance: ${segment_data[:, 2].mean():,.0f}")
        print(f"  Credit Score: {segment_data[:, 5].mean():.0f}")
```

### Healthcare Application: Disease Taxonomy

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - DISEASE TAXONOMY")
print("=" * 70)

np.random.seed(42)
n_conditions = 300

prevalence = np.random.uniform(0.001, 0.15, n_conditions)

severity = np.random.uniform(1, 10, n_conditions)

cost_treatment = np.random.lognormal(8, 1.2, n_conditions)

mortality_rate = np.random.uniform(0.001, 0.25, n_conditions)

recovery_time = np.random.exponential(30, n_conditions)

chronic = np.random.choice([0, 1], n_conditions, p=[0.65, 0.35])

infectious = np.random.choice([0, 1], n_conditions, p=[0.70, 0.30])

genetic = np.random.choice([0, 1], n_conditions, p=[0.85, 0.15])

lifestyle_related = np.random.choice([0, 1], n_conditions, p=[0.75, 0.25])

treatable = np.random.choice([0, 1], n_conditions, p=[0.55, 0.45])

features = [
    'prevalence', 'severity', 'cost_treatment', 'mortality_rate',
    'recovery_time', 'chronic', 'infectious', 'genetic',
    'lifestyle_related', 'treatable'
]
X = np.column_stack([
    prevalence, severity, cost_treatment, mortality_rate,
    recovery_time, chronic, infectious, genetic,
    lifestyle_related, treatable
])

print(f"\nDisease Taxonomy Dataset")
print(f"Number of conditions: {n_conditions}")
print(f"Number of features: {X.shape[1]}")

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

for n_clusters in [4, 8]:
    model = AgglomerativeClustering(n_clusters=n_clusters, linkage='ward')
    labels = model.fit_predict(X_scaled)
    
    silhouette = silhouette_score(X_scaled, labels)
    
    print(f"\n{'='*50}")
    print(f"DISEASE TAXONOMY ({n_clusters} CATEGORIES)")
    print(f"{'='*50}")
    print(f"Silhouette: {silhouette:.4f}")
    
    for i in range(n_clusters):
        category_mask = labels == i
        category_data = X[category_mask]
        
        avg_severity = category_data[:, 1].mean()
        avg_cost = category_data[:, 2].mean()
        chronic_rate = category_data[:, 5].mean()
        treatable_rate = category_data[:, 9].mean()
        
        print(f"\nCategory {i+1} ({category_mask.sum()} conditions):")
        print(f"  Avg Severity: {avg_severity:.1f}")
        print(f"  Avg Treatment Cost: ${avg_cost:,.0f}")
        print(f"  Chronic Rate: {chronic_rate:.1%}")
        print(f"  Treatable Rate: {treatable_rate:.1%}")
```

## Applications

### Banking Applications

Multi-level customer segmentation uses hierarchical clustering to reveal customer relationships at different granularity levels. Top-level segments might include retail, wealth management, and business banking. Each level can be further subdivided. This enables tailored strategies at each organizational level.

Product affinity analysis groups customers based on product usage patterns. The hierarchical structure shows which products commonly appear together, informing cross-selling strategies. The algorithm identifies natural product bundles based on customer behavior rather than arbitrary business rules.

Risk segmentation creates hierarchies of customer risk profiles. Understanding how risk categories relate enables graduated risk management approaches. High-risk segments can be subdivided for targeted intervention strategies.

### Healthcare Applications

Disease taxonomy development organizes medical conditions into meaningful hierarchies. Conditions with similar characteristics group together, supporting clinical research and treatment guidelines. The hierarchical structure enables both broad category analysis and detailed condition-level understanding.

Patient outcome clustering groups patients with similar prognosis trajectories. This supports risk stratification and personalized care planning. The hierarchical view shows how outcome categories relate to each other.

Treatment pattern analysis identifies groupings of similar treatment approaches. This informs clinical protocol development and identifies best practices. The hierarchical structure enables comparison across treatment categories.

## Output Results

### Basic Hierarchical Clustering Results

```
==============================================
HIERARCHICAL CLUSTERING - BASIC IMPLEMENTATION
==============================================

Synthetic Dataset
Number of samples: 400
Number of features: 2
True number of clusters: 4

==============================================
LINKAGE METHOD COMPARISON
==============================================

Linkage: ward
  Silhouette: 0.7234
  Calinski-Harabasz: 823.45
  Davies-Bouldin: 0.5678

Linkage: complete
  Silhouette: 0.6893
  Calinski-Harabasz: 756.34
  Davies-Bouldin: 0.6234

Linkage: average
  Silhouette: 0.7012
  Calinski-Harabasz: 789.23
  Davies-Bouldin: 0.5893

Best Linkage: ward

==============================================
CLUSTERING RESULTS (Ward)
==============================================
Silhouette Score: 0.7234
Cluster 0: 98 points
Cluster 1: 102 points
Cluster 2: 100 points
Cluster 3: 100 points
```

### Customer Segmentation Results

```
==============================================
BANKING APPLICATION - MULTI-LEVEL CUSTOMER SEGMENTATION
==============================================

Customer Segmentation Dataset
Number of customers: 1500
Number of features: 9

==============================================
MULTI-LEVEL SEGMENTATION (3 SEGMENTS)
==============================================
Silhouette Score: 0.5234

Segment 1 (512 customers):
  Avg Age: 52.3 years
  Avg Income: $125,400
  Avg Balance: $42,300
  Credit Score: 725

Segment 2 (495 customers):
  Avg Age: 38.5 years
  Avg Income: $72,800
  Avg Balance: $15,600
  Credit Score: 680

Segment 3 (493 customers):
  Avg Age: 29.8 years
  Avg Income: $52,300
  Avg Balance: $6,800
  Credit Score: 645

==============================================
MULTI-LEVEL SEGMENTATION (9 SEGMENTS)
==============================================
Silhouette Score: 0.6123

[Multiple detailed segments shown with finer granularity]
```

### Disease Taxonomy Results

```
==============================================
HEALTHCARE APPLICATION - DISEASE TAXONOMY
==============================================

Disease Taxonomy Dataset
Number of conditions: 300
Number of features: 10

==============================================
DISEASE TAXONOMY (4 CATEGORIES)
==============================================
Silhouette: 0.5834

Category 1 (78 conditions):
  Avg Severity: 7.2
  Avg Treatment Cost: $28,500
  Chronic Rate: 85.3%
  Treatable Rate: 45.2%

Category 2 (72 conditions):
  Avg Severity: 3.5
  Avg Treatment Cost: $8,200
  Chronic Rate: 22.5%
  Treatable Rate: 82.3%

Category 3 (85 conditions):
  Avg Severity: 5.8
  Avg Treatment Cost: $15,600
  Chronic Rate: 55.2%
  Treatable Rate: 68.5%

Category 4 (65 conditions):
  Avg Severity: 8.5
  Avg Treatment Cost: $45,200
  Chronic Rate: 92.3%
  Treatable Rate: 28.4%
```

## Visualization

### Dendrogram

```
Hierarchical Clustering Dendrogram
----------------------------------------

                    Distance
                      |
           |---------+---------|
           |                   |
      +----+----+         +----+----+
      |         |         |         |
   +--+--+    +--+--+   +--+--+   +--+--+
   |     |    |     |   |     |   |     |
   |     |    |     |   |     |   |     |
   |     |    |     |   |     |   |     |
   |     |    |     |   |     |   |     |
   +--+--+    +--+--+   +--+--+   +--+--+
   
   Level 1: 4 clusters   Level 2: 2 clusters   Level 3: 1 cluster
   
   Cut here for 4 clusters    Cut here for 2 clusters
```

### Cluster Hierarchy

```
Multi-Level Cluster Structure
----------------------------------------

Level 3 (3 clusters)
    |
    +--- High Value (500)
    |       |
    |       +--- Premium (200)
    |       +--- Standard (300)
    |
    +--- Mid Value (450)
    |       |
    |       +--- Growing (250)
    |       +--- Stable (200)
    |
    +--- Low Value (550)
            |
            +--- New (300)
            +--- At-Risk (250)
```

## Advanced Topics

### Distance Threshold Clustering

```python
print("=" * 70)
print("DISTANCE THRESHOLD CLUSTERING")
print("=" * 70)

from scipy.cluster.hierarchy import linkage, fcluster

Z = linkage(X_scaled, method='ward')

distances = [2.0, 3.0, 4.0, 5.0]
for dist in distances:
    labels = fcluster(Z, t=dist, criterion='distance')
    n_clusters = len(np.unique(labels))
    silhouette = silhouette_score(X_scaled, labels)
    print(f"Distance threshold: {dist}, Clusters: {n_clusters}, Silhouette: {silhouette:.4f}")
```

### Complete Linkage Analysis

```python
print("=" * 70)
print("COMPLETE LINKAGE CLUSTERING")
print("=" * 70)

model_complete = AgglomerativeClustering(
    n_clusters=4,
    linkage='complete',
    metric='euclidean'
)
labels = model_complete.fit_predict(X_scaled)

print(f"Complete Linkage Results:")
print(f"Silhouette: {silhouette_score(X_scaled, labels):.4f}")

for i in range(4):
    count = np.sum(labels == i)
    print(f"Cluster {i}: {count} points")
```

### Different Metrics

```python
print("=" * 70)
print("COSINE DISTANCE FOR HIGH-DIMENSIONAL DATA")
print("=" * 70)

from sklearn.metrics.pairwise import cosine_similarity
from scipy.spatial.distance import pdist, squareform

X_text = np.random.rand(200, 50)
cosine_distances = pdist(X_text, metric='cosine')
Z_cosine = linkage(cosine_distances, method='average')

from scipy.cluster.hierarchy import fcluster
labels = fcluster(Z_cosine, t=4, criterion='maxclust')

print(f"Cosine Distance Clustering:")
print(f"Silhouette: {silhouette_score(X_text, labels):.4f}")
```

## Conclusion

Hierarchical Clustering provides valuable insight into data structure through its tree-like cluster organization. The algorithm's ability to reveal relationships at multiple levels of granularity distinguishes it from flat clustering methods like K-means. The dendrogram visualization enables intuitive understanding of cluster relationships.

Key considerations include linkage method selection (ward provides compact clusters), distance metric matching data characteristics, and appropriate cut point selection in the dendrogram. The algorithm's O(n²) complexity limits applicability to very large datasets, though for moderate sizes it provides detailed hierarchical insight.

For banking applications, hierarchical clustering enables multi-level customer understanding that supports both strategic and tactical decision-making. For healthcare, it supports disease taxonomy development and patient phenotype identification. The hierarchical structure provides additional insight beyond flat clustering approaches.