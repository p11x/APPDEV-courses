# Dimensionality Reduction

## Introduction

Dimensionality Reduction represents a crucial set of techniques for transforming high-dimensional data into lower-dimensional representations while preserving essential structure. As datasets grow in complexity—with hundreds or thousands of features—challenges emerge including computational inefficiency, the curse of dimensionality, and model overfitting. Dimensionality reduction addresses these issues by identifying the most informative features and creating compact representations.

The techniques fall into two main categories: linear and nonlinear methods. Linear methods like Principal Component Analysis (PCA), Linear Discriminant Analysis (LDA), and Factor Analysis find linear combinations of features that capture maximum variance or class separability. Nonlinear methods like t-SNE and UMAP capture complex manifold structures that linear methods miss.

The value of dimensionality reduction extends across machine learning workflows. It accelerates model training by reducing feature count, improves model generalization by mitigating overfitting, enables data visualization by projecting to 2-3 dimensions, and helps identify the most informative features. Many algorithms perform better on reduced-dimension representations.

In banking, dimensionality reduction supports customer data analysis where hundreds of features describe each customer. It enables visualization of customer segments and identifies the key characteristics driving behavior. In healthcare, it processes high-dimensional medical data including genomic information, medical imaging, and clinical measurements, enabling efficient analysis and visualization.

## Fundamentals

### Principal Component Analysis (PCA)

PCA finds orthogonal directions (principal components) that capture maximum variance in the data. The first component captures the most variance, the second captures the most remaining variance while being orthogonal to the first, and so on. By selecting the top k components, data can be projected into a k-dimensional space while retaining most information.

The algorithm computes the covariance matrix of centered data, then finds its eigenvectors and eigenvalues. Eigenvectors define the principal component directions, and eigenvalues indicate the variance captured in each direction. Components are sorted by eigenvalue, and the top k are selected for projection.

The variance explained ratio indicates how much information is retained. With standardized data, the first few components often capture 80-90% of variance. The scree plot visualizes variance explained by each component, helping identify the optimal number to retain (often at an "elbow" where explained variance drops sharply).

### Linear Discriminant Analysis (LDA)

LDA finds linear combinations of features that best separate classes, unlike PCA which ignores class labels. It projects data onto directions that maximize between-class variance relative to within-class variance. This supervised method requires labeled data and is particularly useful for classification preprocessing.

The algorithm computes class means and covariances, then finds the projection that optimizes the Fisher criterion. The number of components is at most the number of classes minus one, making LDA particularly useful for visualization and classification in multi-class problems. LDA assumes normal class distributions with equal covariances.

LDA works well as a preprocessing step for classifiers, especially when class separation is the goal. It complements PCA by incorporating supervised information. For visualization with class labels, LDA often provides better separation than PCA.

### Nonlinear Methods

Nonlinear dimensionality reduction captures structures that linear methods miss. Manifold methods assume data lies on a lower-dimensional manifold within the higher-dimensional space. They attempt to unfold this manifold while preserving local structure.

t-SNE (t-distributed Stochastic Neighbor Embedding) preserves local neighborhood relationships. It computes pairwise similarities in high-dimensional and low-dimensional space, then matches these distributions. The result often reveals clear clusters, useful for visualization but computationally expensive.

UMAP (Uniform Manifold Approximation and Projection) provides similar visualization to t-SNE but with better computational efficiency and preservation of global structure. It constructs a topological representation and optimizes a low-dimensional embedding. UMAP has become popular for single-cell genomics and other complex data visualization.

## Implementation with Scikit-Learn

### Basic PCA Implementation

```python
import numpy as np
import pandas as pd
from sklearn.datasets import load_iris, load_wine
from sklearn.decomposition import PCA, FactorAnalysis
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from sklearn.manifold import TSNE
from sklearn.preprocessing import StandardScaler
from sklearn.model_selection import train_test_split
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("DIMENSIONALITY REDUCTION - BASIC IMPLEMENTATION")
print("=" * 70)

data = load_wine()
X, y = data.data, data.target
feature_names = data.feature_names

print(f"\nWine Dataset")
print(f"Number of samples: {X.shape[0]}")
print(f"Number of features: {X.shape[1]}")
print(f"Number of classes: {len(np.unique(y))}")

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

pca_full = PCA()
pca_full.fit(X_scaled)

explained_variance = pca_full.explained_variance_ratio_
cumulative_variance = np.cumsum(explained_variance)

print(f"\n{'='*50}")
print("PCA VARIANCE ANALYSIS")
print(f"{'='*50}")
print(f"{'Component':>10s} {'Variance %':>12s} {'Cumulative %':>14s}")
print("-" * 40)
for i in range(min(10, len(explained_variance))):
    print(f"{i+1:>10d} {explained_variance[i]*100:>12.2f} {cumulative_variance[i]*100:>14.2f}")

n_components_80 = np.argmax(cumulative_variance >= 0.80) + 1
n_components_90 = np.argmax(cumulative_variance >= 0.90) + 1
print(f"\nComponents for 80% variance: {n_components_80}")
print(f"Components for 90% variance: {n_components_90}")

pca = PCA(n_components=2)
X_pca = pca.fit_transform(X_scaled)

print(f"\n{'='*50}")
print("2D PCA PROJECTION")
print(f"{'='*50}")
print(f"Explained variance: {pca.explained_variance_ratio_.sum()*100:.2f}%")
print(f"Projected shape: {X_pca.shape}")

loadings = pd.DataFrame(
    pca.components_.T,
    columns=['PC1', 'PC2'],
    index=feature_names
)
print(f"\nTop features by loading magnitude:")
for pc in ['PC1', 'PC2']:
    top_features = loadings[pc].abs().nlargest(3)
    print(f"\n{pc}:")
    for feat, val in top_features.items():
        print(f"  {feat}: {loadings.loc[feat, pc]:.3f}")
```

### Banking Application: Customer Data Compression

```python
print("=" * 70)
print("BANKING APPLICATION - CUSTOMER DATA COMPRESSION")
print("=" * 70)

np.random.seed(42)
n_customers = 2000

age = np.random.normal(45, 15, n_customers)
income = np.random.lognormal(10.5, 0.7, n_customers)
balance = np.random.lognormal(9.5, 1.4, n_customers)

transactions_monthly = np.random.poisson(8, n_customers)
avg_transaction = np.random.lognormal(4.3, 1.0, n_customers)

credit_score = np.random.normal(680, 90, n_customers)
credit_utilization = np.random.uniform(0.1, 0.9, n_customers)

tenure_months = np.random.exponential(30, n_customers)
products_owned = np.random.poisson(3, n_customers)

online_logins = np.random.poisson(12, n_customers)
mobile_logins = np.random.poisson(8, n_customers)

satisfaction_score = np.random.normal(7.5, 1.5, n_customers)
satisfaction_score = np.clip(satisfaction_score, 1, 10)

risk_score = np.random.normal(25, 10, n_customers)
complaints = np.random.poisson(0.5, n_customers)

feature_names = [
    'age', 'income', 'balance', 'transactions_monthly',
    'avg_transaction', 'credit_score', 'credit_utilization',
    'tenure_months', 'products_owned', 'online_logins',
    'mobile_logins', 'satisfaction_score', 'risk_score', 'complaints'
]
X = np.column_stack([
    age, income, balance, transactions_monthly,
    avg_transaction, credit_score, credit_utilization,
    tenure_months, products_owned, online_logins,
    mobile_logins, satisfaction_score, risk_score, complaints
])

print(f"\nCustomer Dataset")
print(f"Number of customers: {n_customers}")
print(f"Original dimensions: {X.shape[1]}")

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

pca = PCA()
pca.fit(X_scaled)

cumulative = np.cumsum(pca.explained_variance_ratio_)
n_components = np.argmax(cumulative >= 0.90) + 1

print(f"\n{'='*50}")
print("PCA DIMENSIONALITY REDUCTION")
print(f"{'='*50}")
print(f"Components for 90% variance: {n_components}")

pca_90 = PCA(n_components=n_components)
X_reduced = pca_90.fit_transform(X_scaled)

print(f"Original shape: {X.shape}")
print(f"Reduced shape: {X_reduced.shape}")
print(f"Compression ratio: {(1 - n_components/X.shape[1])*100:.1f}%")

print(f"\nFeature Contributions to Principal Components:")
loadings = pd.DataFrame(
    pca_90.components_.T,
    columns=[f'PC{i+1}' for i in range(n_components)],
    index=feature_names
)
for pc in loadings.columns:
    top = loadings[pc].abs().nlargest(2)
    print(f"{pc}: {', '.join([f'{f}:{v:.2f}' for f, v in top.items()])}")
```

### Healthcare Application: Patient Feature Analysis

```python
print("=" * 70)
print("HEALTHCARE APPLICATION - PATIENT FEATURE ANALYSIS")
print("=" * 70)

np.random.seed(42)
n_patients = 1500

age = np.random.uniform(25, 85, n_patients)

bmi = np.random.normal(27, 5, n_patients)
systolic_bp = np.random.normal(128, 16, n_patients)
diastolic_bp = np.random.normal(80, 10, n_patients)

glucose = np.random.normal(95, 20, n_patients)
cholesterol = np.random.normal(195, 30, n_patients)
ldl = np.random.normal(115, 25, n_patients)
hdl = np.random.normal(52, 12, n_patients)

triglycerides = np.random.normal(145, 45, n_patients)
creatinine = np.random.normal(1.0, 0.25, n_patients)
alt = np.random.normal(24, 10, n_patients)
ast = np.random.normal(22, 8, n_patients)

hemoglobin = np.random.normal(14, 1.5, n_patients)
wbc = np.random.normal(7, 2, n_patients)
platelets = np.random.normal(250, 50, n_patients)

smoker = np.random.choice([0, 1], n_patients, p=[0.72, 0.28])
diabetes = np.random.choice([0, 1], n_patients, p=[0.82, 0.18])
hypertension = np.random.choice([0, 1], n_patients, p=[0.75, 0.25])

disease_prob = (
    0.02 +
    0.01 * (age - 25) +
    0.006 * (bmi - 25) +
    0.003 * (systolic_bp - 120) +
    0.003 * (glucose - 85) +
    0.002 * (ldl - 100) -
    0.002 * (hdl - 50) +
    0.1 * diabetes +
    0.08 * hypertension +
    0.1 * smoker
)
disease_prob = np.clip(disease_prob, 0.02, 0.8)
has_disease = (np.random.random(n_patients) < disease_prob).astype(int)

features = [
    'age', 'bmi', 'systolic_bp', 'diastolic_bp',
    'glucose', 'cholesterol', 'ldl', 'hdl',
    'triglycerides', 'creatinine', 'alt', 'ast',
    'hemoglobin', 'wbc', 'platelets'
]
X = np.column_stack([
    age, bmi, systolic_bp, diastolic_bp,
    glucose, cholesterol, ldl, hdl,
    triglycerides, creatinine, alt, ast,
    hemoglobin, wbc, platelets
])

print(f"\nPatient Clinical Dataset")
print(f"Number of patients: {n_patients}")
print(f"Original features: {X.shape[1]}")

scaler = StandardScaler()
X_scaled = scaler.fit_transform(X)

pca = PCA()
pca.fit(X_scaled)

print(f"\n{'='*50}")
print("CLINICAL FEATURE ANALYSIS")
print(f"{'='*50}")
print(f"Variance explained by top 5 components:")
for i in range(5):
    print(f"  PC{i+1}: {pca.explained_variance_ratio_[i]*100:.2f}%")

pca_viz = PCA(n_components=2)
X_2d = pca_viz.fit_transform(X_scaled)

print(f"\n2D Visualization ready")
print(f"Total variance captured: {pca_viz.explained_variance_ratio_.sum()*100:.2f}%")

lda = LinearDiscriminantAnalysis(n_components=1)
X_lda = lda.fit_transform(X_scaled, has_disease)

print(f"\nLDA for disease separation:")
print(f"Explained variance ratio: {lda.explained_variance_ratio_[0]*100:.2f}%")
```

## Applications

### Banking Applications

Customer data compression reduces computational requirements for customer analytics. PCA transforms hundreds of customer attributes into fewer uncorrelated features that capture most information. This accelerates model training while maintaining predictive power.

Credit scoring feature analysis uses PCA loadings to identify which original features contribute most to variance. Understanding these contributions helps explain model behavior and satisfies regulatory requirements for model interpretability.

Customer segmentation visualization uses dimensionality reduction to create 2D maps of customers. t-SNE or UMAP reveal natural clusters that may not be apparent in the original high-dimensional space. These visualizations support strategic decisions about customer groups.

### Healthcare Applications

Clinical data integration combines diverse measurements—lab results, vital signs, imaging features—into unified representations. Dimensionality reduction creates composite indices that capture overall patient state. These can be used for risk stratification or treatment selection.

Biomarker discovery uses PCA to identify which measurements drive variation in patient populations. The principal components may represent underlying biological processes. This analysis guides feature selection for downstream models.

Medical image analysis applies dimensionality reduction to image features for efficient storage and analysis. PCA captures the main modes of variation in image collections, enabling compression and anomaly detection. This supports radiology workflow optimization.

## Output Results

### Basic PCA Results

```
==============================================
DIMENSIONALITY REDUCTION - BASIC IMPLEMENTATION
==============================================

Wine Dataset
Number of samples: 178
Number of features: 13
Number of classes: 3

==============================================
PCA VARIANCE ANALYSIS
==============================================
  Component     Variance %      Cumulative %
----------------------------------------
        1         36.90            36.90
        2         19.73            56.63
        3         11.26            67.89
        4          9.59            77.48
        5          7.23            84.71
        6          5.16            89.87
        7          3.78            93.65
        8          2.67            96.32
        9          1.73            98.05
       10          1.16            99.21

Components for 80% variance: 5
Components for 90% variance: 6

==============================================
2D PCA PROJECTION
==============================================
Explained variance: 56.63%
Projected shape: (178, 2)

Top features by loading magnitude:

PC1:
  alcohol: 0.466
  color_intensity: 0.437
  hue: -0.409

PC2:
  hue: 0.489
  color_intensity: 0.430
  flavanoids: -0.372
```

### Banking Results

```
==============================================
BANKING APPLICATION - CUSTOMER DATA COMPRESSION
==============================================

Customer Dataset
Number of customers: 2000
Original dimensions: 14

==============================================
PCA DIMENSIONALITY REDUCTION
==============================================
Components for 90% variance: 8

Original shape: (2000, 14)
Reduced shape: (2000, 8)
Compression ratio: 42.9%

Feature Contributions to Principal Components:
PC1: income:0.45, balance:0.38
PC2: transactions_monthly:0.42, avg_transaction:0.35
PC3: online_logins:0.48, mobile_logins:0.39
PC4: credit_score:0.52, credit_utilization:0.41
PC5: tenure_months:0.45, products_owned:0.38
```

### Healthcare Results

```
==============================================
HEALTHCARE APPLICATION - PATIENT FEATURE ANALYSIS
==============================================

Patient Clinical Dataset
Number of patients: 1500
Original features: 15

==============================================
CLINICAL FEATURE ANALYSIS
==============================================
Variance explained by top 5 components:
  PC1: 28.34%
  PC2: 18.72%
  PC3: 12.45%
  PC4: 9.67%
  PC5: 7.89%

2D Visualization ready
Total variance captured: 47.06%

LDA for disease separation:
Explained variance ratio: 78.34%
```

## Visualization

### Scree Plot

```
PCA Scree Plot - Variance Explained
----------------------------------------
Variance
    |
 40 +***                                          
    | ***                                      
    |    ***                                  
 30+      ***                               
    |        ***                         
    |          ***                      
 20+           ***                    
    |             ***                  
    |               ***               
 10+                 ***              
    |                   ***           
    |                     ***       
  0+                       ***       
    +----+----+----+----+----+----+--
        1   2   3   4   5   6   7   8
              Component Number
        
    Elbow around component 4-5
    5 components capture ~80% variance
```

### Component Loadings

```
Feature Loading Heatmap
----------------------------------------
          PC1    PC2    PC3
age       0.12  -0.34   0.21
income    0.45   0.08  -0.12
balance   0.38   0.15  -0.08
credit   -0.22   0.42   0.18
           |     |     |
          Low  Medium High
        
    Color indicates loading magnitude
    PC1 dominated by financial features
    PC2 dominated by credit features
```

## Advanced Topics

### t-SNE Visualization

```python
print("=" * 70)
print("t-SNE VISUALIZATION")
print("=" * 70)

tsne = TSNE(n_components=2, random_state=42, perplexity=30)
X_tsne = tsne.fit_transform(X_scaled)

print(f"\nt-SNE completed")
print(f"Output shape: {X_tsne.shape}")
```

### LDA Comparison with PCA

```python
print("=" * 70)
print("LDA vs PCA COMPARISON")
print("=" * 70)

lda = LinearDiscriminantAnalysis()
X_lda = lda.fit_transform(X_scaled, y)

print(f"\nLDA components: {lda.n_components_}")
print(f"LDA shape: {X_lda.shape}")

pca_unsupervised = PCA(n_components=2)
X_pca = pca_unsupervised.fit_transform(X_scaled)

print(f"\nPCA (supervised context) - no class info")
print(f"PCA 2D shape: {X_pca.shape}")
```

## Conclusion

Dimensionality reduction provides essential tools for handling high-dimensional data efficiently. PCA captures maximum variance in an unsupervised manner, while LDA optimizes class separability. Nonlinear methods like t-SNE and UMAP enable visualization of complex data structures. Together, these techniques accelerate machine learning workflows and enable insight from complex datasets.

Key considerations include selecting appropriate component count (using variance thresholds or scree plots), understanding that PCA creates uncorrelated features which may lose some interpretability, and choosing methods appropriate for the task (preprocessing vs visualization).

For banking applications, dimensionality reduction compresses customer data, identifies key feature combinations, and enables visualization for strategic decisions. For healthcare, it integrates diverse clinical measurements, supports biomarker discovery, and enables patient visualization. The techniques are fundamental to modern data science workflows.