# Topic: Hierarchical Clustering
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive Implementation for Hierarchical Clustering

================================================================================
I. INTRODUCTION
================================================================================

Hierarchical Clustering is an unsupervised machine learning technique that creates a 
hierarchy of clusters without requiring the number of clusters to be specified in advance.
Unlike K-Means, which requires预先 knowing the number of clusters (k), hierarchical 
clustering builds a dendrogram (tree structure) that shows how clusters are related.

This comprehensive guide covers:
- Theory behind hierarchical clustering
- Different linkage methods (single, complete, average, ward)
- Distance metrics (euclidean, manhattan, cosine, etc.)
- Dendrogram interpretation and cutting
- Practical applications in banking and healthcare
- Advanced topics and best practices

================================================================================
II. CORE CONCEPTS
================================================================================

Hierarchical Clustering operates on the principle of agglomerative (bottom-up) or 
divisive (top-down) clustering:

AGGLOMERATIVE (Bottom-up):
1. Start with each point as its own cluster
2. Find the two closest clusters and merge them
3. Repeat until all points are in one cluster

DIVISIVE (Top-down):
1. Start with all points in one cluster
2. Recursively split clusters until each point is in its own cluster

KEY COMPONENTS:
- Linkage Method: How to measure distance between clusters
- Distance Metric: How to measure distance between individual points
- Dendrogram: Tree visualization showing cluster relationships

================================================================================
III. IMPLEMENTATION
================================================================================
"""

# Import necessary libraries
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from matplotlib.colors import ListedColormap
import seaborn as sns
from sklearn.datasets import make_blobs, make_circles, make_moons
from sklearn.preprocessing import StandardScaler, MinMaxScaler
from sklearn.cluster import AgglomerativeClustering
from sklearn.metrics import silhouette_score, calinski_harabasz_score, davies_bouldin_score
from sklearn.metrics import pairwise_distances
from scipy.cluster.hierarchy import dendrogram, linkage, fcluster, cut_tree
from scipy.spatial.distance import pdist, squareform
from scipy import stats
import warnings
warnings.filterwarnings('ignore')

# Set plotting style
plt.style.use('seaborn-v0_8-whitegrid')
sns.set_palette("husl")


# ================================================================================
# SECTION 1: DATA GENERATION FUNCTIONS
# ================================================================================

def generate_synthetic_blobs(n_samples=500, n_features=2, centers=4, cluster_std=1.0, 
                           random_state=42):
    """
    Generate synthetic blob data for clustering demonstrations.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features (dimensions)
    centers : int
        Number of cluster centers
    cluster_std : float
        Standard deviation of clusters
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        True cluster labels
    """
    X, y = make_blobs(
        n_samples=n_samples,
        n_features=n_features,
        centers=centers,
        cluster_std=cluster_std,
        random_state=random_state
    )
    
    print(f"Generated {n_samples} samples with {n_features} features")
    print(f"True number of clusters: {centers}")
    print(f"Cluster distribution: {np.bincount(y)}")
    
    return X, y


def generate_banking_customer_data(n_customers=1000, random_state=42):
    """
    Generate synthetic banking customer data for segmentation.
    
    Features include:
    - annual_income: Customer's annual income
    - credit_score: Credit score (300-850)
    - account_balance: Total account balance
    - transaction_frequency: Monthly transaction count
    - loan_amount: Outstanding loan amount
    - age: Customer age
    - relationship_years: Years with the bank
    
    Parameters:
    -----------
    n_customers : int
        Number of customers to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : DataFrame
        Customer data with features
    """
    np.random.seed(random_state)
    
    # Define customer segments with different characteristics
    n_segments = 5
    customers_per_segment = n_customers // n_segments
    
    data = []
    
    for segment in range(n_segments):
        if segment == 0:  # High Net Worth
            income = np.random.normal(200000, 30000, customers_per_segment)
            credit_score = np.random.normal(780, 20, customers_per_segment)
            balance = np.random.normal(500000, 100000, customers_per_segment)
            transactions = np.random.normal(15, 3, customers_per_segment)
            loan = np.random.normal(50000, 20000, customers_per_segment)
            age = np.random.normal(55, 8, customers_per_segment)
            years = np.random.normal(15, 4, customers_per_segment)
            
        elif segment == 1:  # Upper Middle Class
            income = np.random.normal(120000, 20000, customers_per_segment)
            credit_score = np.random.normal(720, 25, customers_per_segment)
            balance = np.random.normal(150000, 40000, customers_per_segment)
            transactions = np.random.normal(20, 5, customers_per_segment)
            loan = np.random.normal(150000, 30000, customers_per_segment)
            age = np.random.normal(42, 7, customers_per_segment)
            years = np.random.normal(10, 3, customers_per_segment)
            
        elif segment == 2:  # Middle Class
            income = np.random.normal(75000, 15000, customers_per_segment)
            credit_score = np.random.normal(680, 30, customers_per_segment)
            balance = np.random.normal(50000, 15000, customers_per_segment)
            transactions = np.random.normal(12, 4, customers_per_segment)
            loan = np.random.normal(200000, 40000, customers_per_segment)
            age = np.random.normal(35, 6, customers_per_segment)
            years = np.random.normal(7, 2, customers_per_segment)
            
        elif segment == 3:  # Lower Middle Class
            income = np.random.normal(45000, 10000, customers_per_segment)
            credit_score = np.random.normal(620, 35, customers_per_segment)
            balance = np.random.normal(15000, 5000, customers_per_segment)
            transactions = np.random.normal(8, 3, customers_per_segment)
            loan = np.random.normal(100000, 25000, customers_per_segment)
            age = np.random.normal(30, 5, customers_per_segment)
            years = np.random.normal(4, 1, customers_per_segment)
            
        else:  # Young/Saver
            income = np.random.normal(35000, 8000, customers_per_segment)
            credit_score = np.random.normal(580, 40, customers_per_segment)
            balance = np.random.normal(5000, 2000, customers_per_segment)
            transactions = np.random.normal(5, 2, customers_per_segment)
            loan = np.random.normal(5000, 3000, customers_per_segment)
            age = np.random.normal(24, 3, customers_per_segment)
            years = np.random.normal(2, 1, customers_per_segment)
        
        # Clip values to realistic ranges
        income = np.clip(income, 20000, 500000)
        credit_score = np.clip(credit_score, 300, 850)
        balance = np.clip(balance, 0, 2000000)
        transactions = np.clip(transactions, 1, 50)
        loan = np.clip(loan, 0, 500000)
        age = np.clip(age, 18, 80)
        years = np.clip(years, 1, 40)
        
        for i in range(customers_per_segment):
            data.append({
                'customer_id': segment * customers_per_segment + i + 1,
                'segment': segment,
                'annual_income': income[i],
                'credit_score': credit_score[i],
                'account_balance': balance[i],
                'transaction_frequency': transactions[i],
                'loan_amount': loan[i],
                'age': age[i],
                'relationship_years': years[i]
            })
    
    df = pd.DataFrame(data)
    
    # Shuffle the data
    df = df.sample(frac=1, random_state=random_state).reset_index(drop=True)
    df['customer_id'] = range(1, n_customers + 1)
    
    print(f"Generated banking customer data: {n_customers} customers")
    print(f"Features: {list(df.columns)}")
    
    return df


def generate_healthcare_patient_data(n_patients=800, random_state=42):
    """
    Generate synthetic healthcare patient data for patient grouping.
    
    Features include:
    - age: Patient age
    - bmi: Body Mass Index
    - blood_pressure_systolic: Systolic blood pressure
    - blood_pressure_diastolic: Diastolic blood pressure
    - heart_rate: Heart rate
    - cholesterol_total: Total cholesterol
    - glucose_level: Blood glucose level
    - visit_frequency: Annual visit frequency
    - num_chronic_conditions: Number of chronic conditions
    
    Parameters:
    -----------
    n_patients : int
        Number of patients to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : DataFrame
        Patient data with features
    """
    np.random.seed(random_state)
    
    # Define patient groups with different health profiles
    n_groups = 6
    patients_per_group = n_patients // n_groups
    
    data = []
    
    for group in range(n_groups):
        if group == 0:  # Healthy Young Adults
            age = np.random.normal(28, 4, patients_per_group)
            bmi = np.random.normal(23, 2, patients_per_group)
            bp_sys = np.random.normal(118, 8, patients_per_group)
            bp_dia = np.random.normal(75, 5, patients_per_group)
            heart_rate = np.random.normal(70, 6, patients_per_group)
            cholesterol = np.random.normal(175, 20, patients_per_group)
            glucose = np.random.normal(85, 8, patients_per_group)
            visits = np.random.normal(2, 1, patients_per_group)
            chronic = np.random.normal(0, 0, patients_per_group)
            
        elif group == 1:  # Middle-aged with Pre-hypertension
            age = np.random.normal(45, 6, patients_per_group)
            bmi = np.random.normal(27, 3, patients_per_group)
            bp_sys = np.random.normal(135, 10, patients_per_group)
            bp_dia = np.random.normal(85, 6, patients_per_group)
            heart_rate = np.random.normal(75, 7, patients_per_group)
            cholesterol = np.random.normal(210, 25, patients_per_group)
            glucose = np.random.normal(95, 10, patients_per_group)
            visits = np.random.normal(4, 1, patients_per_group)
            chronic = np.random.normal(1, 0.5, patients_per_group)
            
        elif group == 2:  # High-risk Hypertensive
            age = np.random.normal(58, 7, patients_per_group)
            bmi = np.random.normal(31, 4, patients_per_group)
            bp_sys = np.random.normal(155, 15, patients_per_group)
            bp_dia = np.random.normal(95, 8, patients_per_group)
            heart_rate = np.random.normal(80, 8, patients_per_group)
            cholesterol = np.random.normal(245, 30, patients_per_group)
            glucose = np.random.normal(110, 15, patients_per_group)
            visits = np.random.normal(6, 2, patients_per_group)
            chronic = np.random.normal(2, 0.8, patients_per_group)
            
        elif group == 3:  # Diabetic Group
            age = np.random.normal(52, 8, patients_per_group)
            bmi = np.random.normal(32, 4, patients_per_group)
            bp_sys = np.random.normal(140, 12, patients_per_group)
            bp_dia = np.random.normal(88, 7, patients_per_group)
            heart_rate = np.random.normal(78, 7, patients_per_group)
            cholesterol = np.random.normal(220, 25, patients_per_group)
            glucose = np.random.normal(160, 25, patients_per_group)
            visits = np.random.normal(8, 2, patients_per_group)
            chronic = np.random.normal(2, 1, patients_per_group)
            
        elif group == 4:  # Cardiovascular Risk
            age = np.random.normal(62, 6, patients_per_group)
            bmi = np.random.normal(29, 3, patients_per_group)
            bp_sys = np.random.normal(148, 14, patients_per_group)
            bp_dia = np.random.normal(90, 7, patients_per_group)
            heart_rate = np.random.normal(85, 10, patients_per_group)
            cholesterol = np.random.normal(260, 35, patients_per_group)
            glucose = np.random.normal(105, 12, patients_per_group)
            visits = np.random.normal(7, 2, patients_per_group)
            chronic = np.random.normal(3, 1, patients_per_group)
            
        else:  # Elderly with Multiple Conditions
            age = np.random.normal(72, 7, patients_per_group)
            bmi = np.random.normal(28, 4, patients_per_group)
            bp_sys = np.random.normal(142, 16, patients_per_group)
            bp_dia = np.random.normal(85, 8, patients_per_group)
            heart_rate = np.random.normal(82, 8, patients_per_group)
            cholesterol = np.random.normal(200, 30, patients_per_group)
            glucose = np.random.normal(120, 20, patients_per_group)
            visits = np.random.normal(10, 3, patients_per_group)
            chronic = np.random.normal(4, 1.5, patients_per_group)
        
        # Clip values to realistic ranges
        age = np.clip(age, 18, 95)
        bmi = np.clip(bmi, 15, 50)
        bp_sys = np.clip(bp_sys, 90, 200)
        bp_dia = np.clip(bp_dia, 50, 120)
        heart_rate = np.clip(heart_rate, 45, 130)
        cholesterol = np.clip(cholesterol, 100, 350)
        glucose = np.clip(glucose, 60, 250)
        visits = np.clip(visits, 1, 20)
        chronic = np.clip(chronic, 0, 6)
        
        for i in range(patients_per_group):
            data.append({
                'patient_id': group * patients_per_group + i + 1,
                'group': group,
                'age': age[i],
                'bmi': bmi[i],
                'blood_pressure_systolic': bp_sys[i],
                'blood_pressure_diastolic': bp_dia[i],
                'heart_rate': heart_rate[i],
                'cholesterol_total': cholesterol[i],
                'glucose_level': glucose[i],
                'visit_frequency': visits[i],
                'num_chronic_conditions': chronic[i]
            })
    
    df = pd.DataFrame(data)
    
    # Shuffle the data
    df = df.sample(frac=1, random_state=random_state).reset_index(drop=True)
    df['patient_id'] = range(1, n_patients + 1)
    
    print(f"Generated healthcare patient data: {n_patients} patients")
    print(f"Features: {list(df.columns)}")
    
    return df


def generate_complex_clusters(n_samples=600, random_state=42):
    """
    Generate complex cluster shapes for testing different linkage methods.
    """
    np.random.seed(random_state)
    
    # Create three different cluster shapes
    n_per_cluster = n_samples // 3
    
    # Cluster 1: Elliptical blob
    theta = np.random.uniform(0, 2*np.pi, n_per_cluster)
    r = np.random.uniform(0, 3, n_per_cluster)
    X1 = np.column_stack([3*r*np.cos(theta), 2*r*np.sin(theta)])
    X1 += np.random.normal(0, 0.3, X1.shape)
    X1 += [8, 2]
    
    # Cluster 2: Linear stretched
    x2 = np.random.uniform(0, 5, n_per_cluster)
    X2 = np.column_stack([x2, 0.3*x2 + np.random.normal(0, 0.3, n_per_cluster)])
    X2 += np.random.normal(0, 0.2, X2.shape)
    X2 += [-2, 6]
    
    # Cluster 3: Circle
    theta = np.random.uniform(0, 2*np.pi, n_per_cluster)
    r = np.random.uniform(2, 2.5, n_per_cluster)
    X3 = np.column_stack([r*np.cos(theta), r*np.sin(theta)])
    X3 += np.random.normal(0, 0.2, X3.shape)
    X3 += [0, 0]
    
    X = np.vstack([X1, X2, X3])
    y = np.array([0]*n_per_cluster + [1]*n_per_cluster + [2]*n_per_cluster)
    
    return X, y


# ================================================================================
# SECTION 2: CORE HIERARCHICAL CLUSTERING IMPLEMENTATION
# ================================================================================

def core_hierarchical_clustering(X, n_clusters=3, linkage_method='ward', 
                               affinity='euclidean', compute_distances=True):
    """
    Core hierarchical clustering implementation using AgglomerativeClustering.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix (n_samples, n_features)
    n_clusters : int
        Number of clusters to form
    linkage_method : str
        Linkage criterion: 'ward', 'complete', 'average', 'single'
    affinity : str
        Distance metric: 'euclidean', 'manhattan', 'cosine', 'l1', 'l2'
    compute_distances : bool
        Whether to compute pairwise distances
        
    Returns:
    --------
    cluster_labels : ndarray
        Cluster labels for each sample
    model : AgglomerativeClustering
        Fitted clustering model
    """
    model = AgglomerativeClustering(
        n_clusters=n_clusters,
        linkage=linkage_method,
        affinity=affinity,
        compute_distances=compute_distances
    )
    
    labels = model.fit_predict(X)
    
    return labels, model


def perform_linkage_analysis(X, methods=['single', 'complete', 'average', 'ward']):
    """
    Perform hierarchical clustering with different linkage methods.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    methods : list
        List of linkage methods to compare
        
    Returns:
    --------
    results : dict
        Results for each linkage method
    """
    results = {}
    
    for method in methods:
        try:
            # Use euclidean for ward, allow others for non-ward
            if method == 'ward':
                affinity = 'euclidean'
            else:
                affinity = 'euclidean'
            
            labels, model = core_hierarchical_clustering(
                X, 
                n_clusters=3, 
                linkage_method=method,
                affinity=affinity
            )
            
            silhouette = silhouette_score(X, labels)
            calinski = calinski_harabasz_score(X, labels)
            davies = davies_bouldin_score(X, labels)
            
            results[method] = {
                'labels': labels,
                'model': model,
                'silhouette': silhouette,
                'calinski_harabasz': calinski,
                'davies_bouldin': davies,
                'n_clusters': len(np.unique(labels))
            }
            
            print(f"Linkage method: {method}")
            print(f"  Silhouette Score: {silhouette:.4f}")
            print(f"  Calinski-Harabasz: {calinski:.2f}")
            print(f"  Davies-Bouldin: {davies:.4f}")
            print()
            
        except Exception as e:
            print(f"Error with method {method}: {e}")
            results[method] = None
    
    return results


def compute_linkage_matrix(X, method='ward', metric='euclidean'):
    """
    Compute the linkage matrix using scipy.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    method : str
        Linkage method
    metric : str
        Distance metric
        
    Returns:
    --------
    Z : ndarray
        Linkage matrix
    """
    Z = linkage(X, method=method, metric=metric)
    
    print(f"Linkage matrix shape: {Z.shape}")
    print(f"Linkage method: {method}")
    print(f"Distance metric: {metric}")
    print(f"\nLinkage matrix columns: [cluster1, cluster2, distance, n_samples]")
    
    return Z


# ================================================================================
# SECTION 3: DENDROGRAM ANALYSIS AND VISUALIZATION
# ================================================================================

def create_dendrogram(X, method='ward', metric='euclidean', truncate_mode=None, 
                      p=30, leaf_rotation=90, leaf_font_size=8):
    """
    Create and display a dendrogram for hierarchical clustering.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    method : str
        Linkage method
    metric : str
        Distance metric
    truncate_mode : str
        Truncation mode: None, 'last', 'level'
    p : int
        Truncation parameter
    leaf_rotation : float
        Rotation of leaf labels
    leaf_font_size : int
        Font size of leaf labels
        
    Returns:
    --------
    Z : ndarray
        Linkage matrix
    """
    Z = compute_linkage_matrix(X, method, metric)
    
    fig, ax = plt.subplots(figsize=(14, 8))
    
    dendrogram(
        Z,
        truncate_mode=truncate_mode,
        p=p,
        leaf_rotation=leaf_rotation,
        leaf_font_size=leaf_font_size,
        ax=ax,
        show_contracted=True
    )
    
    ax.set_title(f'Hierarchical Clustering Dendrogram (Method: {method})', 
                fontsize=14, fontweight='bold')
    ax.set_xlabel('Sample Index', fontsize=12)
    ax.set_ylabel('Distance', fontsize=12)
    ax.axhline(y=5, color='r', linestyle='--', label='Cut threshold')
    ax.legend()
    
    plt.tight_layout()
    plt.show()
    
    return Z


def cut_dendrogram(Z, n_clusters=None, height=None, criterion='maxclust'):
    """
    Cut the dendrogram to obtain cluster assignments.
    
    Parameters:
    -----------
    Z : ndarray
        Linkage matrix
    n_clusters : int
        Number of clusters (if criterion is 'maxclust')
    height : float
        Height to cut (if criterion is 'distance')
    criterion : str
        Criterion for cutting: 'maxclust' or 'distance'
        
    Returns:
    --------
    labels : ndarray
        Cluster labels
    """
    if criterion == 'maxclust':
        labels = fcluster(Z, n_clusters, criterion=criterion)
    else:
        labels = fcluster(Z, height, criterion=criterion)
    
    print(f"Cut dendrogram: {criterion}")
    print(f"Number of clusters: {len(np.unique(labels))}")
    
    return labels


def analyze_dendrogram_cuts(Z, X, max_clusters=10):
    """
    Analyze different numbers of clusters by cutting at various heights.
    
    Parameters:
    -----------
    Z : ndarray
        Linkage matrix
    X : ndarray
        Feature matrix
    max_clusters : int
        Maximum number of clusters to analyze
        
    Returns:
    --------
    analysis : DataFrame
        Analysis results for different cluster numbers
    """
    results = []
    
    for n in range(2, max_clusters + 1):
        labels = fcluster(Z, n, criterion='maxclust')
        
        silhouette = silhouette_score(X, labels)
        calinski = calinski_harabasz_score(X, labels)
        davies = davies_bouldin_score(X, labels)
        
        results.append({
            'n_clusters': n,
            'silhouette': silhouette,
            'calinski_harabasz': calinski,
            'davies_bouldin': davies
        })
    
    df = pd.DataFrame(results)
    print("\n=== Dendrogram Cut Analysis ===")
    print(df.to_string(index=False))
    
    return df


def plot_linkage_matrices(X, methods=['single', 'complete', 'average', 'ward']):
    """
    Plot linkage matrices for different methods side by side.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    methods : list
        List of linkage methods
    """
    fig, axes = plt.subplots(2, 2, figsize=(16, 12))
    axes = axes.flatten()
    
    for idx, method in enumerate(methods):
        try:
            Z = linkage(X, method=method)
            
            dendrogram(
                Z,
                truncate_mode='last',
                p=20,
                ax=axes[idx],
                show_contracted=True
            )
            
            axes[idx].set_title(f'Linkage: {method}', fontsize=12, fontweight='bold')
            axes[idx].set_xlabel('Sample Index')
            axes[idx].set_ylabel('Distance')
            
        except Exception as e:
            axes[idx].set_title(f'Error: {e}')
    
    plt.tight_layout()
    plt.show()


# ================================================================================
# SECTION 4: DISTANCE METRICS AND AFFINITY
# ================================================================================

def compute_distance_matrix(X, metric='euclidean'):
    """
    Compute pairwise distance matrix.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    metric : str
        Distance metric
        
    Returns:
    --------
    dist_matrix : ndarray
        Pairwise distance matrix
    """
    dist_matrix = pdist(X, metric=metric)
    dist_matrix = squareform(dist_matrix)
    
    print(f"Distance metric: {metric}")
    print(f"Distance matrix shape: {dist_matrix.shape}")
    print(f"Min distance: {dist_matrix[dist_matrix > 0].min():.4f}")
    print(f"Max distance: {dist_matrix.max():.4f}")
    print(f"Mean distance: {dist_matrix[dist_matrix > 0].mean():.4f}")
    
    return dist_matrix


def compare_distance_metrics(X, metrics=['euclidean', 'manhattan', 'cosine']):
    """
    Compare clustering results using different distance metrics.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    metrics : list
        List of distance metrics
        
    Returns:
    --------
    results : dict
        Results for each distance metric
    """
    results = {}
    
    for metric in metrics:
        try:
            # Compute distances
            dist_matrix = compute_distance_matrix(X[:100], metric)  # Use subset for speed
            
            # Fit clustering (use 'euclidean' affinity for ward linkage)
            labels, model = core_hierarchical_clustering(
                X, 
                n_clusters=3, 
                linkage_method='ward',
                affinity='euclidean'  # Ward only supports euclidean
            )
            
            silhouette = silhouette_score(X, labels)
            
            results[metric] = {
                'silhouette': silhouette,
                'labels': labels
            }
            
            print(f"Metric: {metric}, Silhouette: {silhouette:.4f}")
            
        except Exception as e:
            print(f"Error with metric {metric}: {e}")
    
    return results


# ================================================================================
# SECTION 5: VISUALIZATION FUNCTIONS
# ================================================================================

def plot_clusters_2d(X, labels, title='Cluster Visualization', 
                    centroids=None, save_path=None):
    """
    Plot 2D cluster visualization.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix (n_samples, 2)
    labels : ndarray
        Cluster labels
    title : str
        Plot title
    centroids : ndarray
        Cluster centroids (optional)
    save_path : str
        Path to save plot
    """
    fig, ax = plt.subplots(figsize=(10, 8))
    
    unique_labels = np.unique(labels)
    colors = plt.cm.tab10(np.linspace(0, 1, len(unique_labels)))
    
    for idx, label in enumerate(unique_labels):
        mask = labels == label
        ax.scatter(X[mask, 0], X[mask, 1], 
                   c=[colors[idx]], 
                   label=f'Cluster {label}',
                   alpha=0.6, 
                   edgecolors='w', 
                   s=50)
    
    if centroids is not None:
        ax.scatter(centroids[:, 0], centroids[:, 1],
                   c='black', marker='X', s=200,
                   label='Centroids', edgecolors='w')
    
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.set_xlabel('Feature 1', fontsize=12)
    ax.set_ylabel('Feature 2', fontsize=12)
    ax.legend()
    ax.grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    if save_path:
        plt.savefig(save_path, dpi=150, bbox_inches='tight')
    
    plt.show()


def plot_cluster_comparison(X, results, save_path=None):
    """
    Plot comparison of different clustering methods.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    results : dict
        Results from different methods
    save_path : str
        Path to save plot
    """
    n_methods = len(results)
    n_cols = 2
    n_rows = (n_methods + 1) // 2
    
    fig, axes = plt.subplots(n_rows, n_cols, figsize=(14, 6*n_rows))
    axes = axes.flatten()
    
    for idx, (method, result) in enumerate(results.items()):
        if result is None:
            continue
            
        labels = result['labels']
        
        unique_labels = np.unique(labels)
        colors = plt.cm.tab10(np.linspace(0, 1, len(unique_labels)))
        
        for label in unique_labels:
            mask = labels == label
            axes[idx].scatter(X[mask, 0], X[mask, 1], 
                             c=[colors[label]], 
                             label=f'Cluster {label}',
                             alpha=0.6, edgecolors='w', s=50)
        
        silhouette = result['silhouette']
        axes[idx].set_title(f'Method: {method}\nSilhouette: {silhouette:.4f}', 
                          fontsize=11, fontweight='bold')
        axes[idx].set_xlabel('Feature 1')
        axes[idx].set_ylabel('Feature 2')
        axes[idx].legend()
        axes[idx].grid(True, alpha=0.3)
    
    # Hide unused subplots
    for idx in range(len(results), len(axes)):
        axes[idx].axis('off')
    
    plt.tight_layout()
    
    if save_path:
        plt.savefig(save_path, dpi=150, bbox_inches='tight')
    
    plt.show()


def plot_silhouette_analysis(X, labels, save_path=None):
    """
    Plot silhouette analysis for clusters.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    labels : ndarray
        Cluster labels
    save_path : str
        Path to save plot
    """
    from sklearn.metrics import silhouette_samples
    
    fig, axes = plt.subplots(1, 2, figsize=(14, 6))
    
    # Silhouette scores for each sample
    silhouette_avg = silhouette_score(X, labels)
    sample_silhouette_values = silhouette_samples(X, labels)
    
    y_lower = 10
    n_clusters = len(np.unique(labels))
    
    colors = plt.cm.tab10(np.linspace(0, 1, n_clusters))
    
    for i in range(n_clusters):
        cluster_silhouette_values = sample_silhouette_values[labels == i]
        cluster_silhouette_values.sort()
        
        size_cluster_i = cluster_silhouette_values.shape[0]
        y_upper = y_lower + size_cluster_i
        
        axes[0].fill_betweenx(
            np.arange(y_lower, y_upper),
            0, cluster_silhouette_values,
            facecolor=colors[i], edgecolor=colors[i], alpha=0.7
        )
        axes[0].text(-0.05, y_lower + 0.5 * size_cluster_i, str(i))
        y_lower = y_upper + 10
    
    axes[0].axvline(x=silhouette_avg, color="red", linestyle="--", 
                   label=f'Average: {silhouette_avg:.4f}')
    axes[0].set_title("Silhouette Analysis", fontweight='bold')
    axes[0].set_xlabel("Silhouette Coefficient")
    axes[0].set_ylabel("Cluster")
    axes[0].legend()
    axes[0].set_xlim([-0.2, 1])
    
    # Cluster size distribution
    cluster_sizes = np.bincount(labels)
    axes[1].bar(range(n_clusters), cluster_sizes, color=colors)
    axes[1].set_title("Cluster Sizes", fontweight='bold')
    axes[1].set_xlabel("Cluster")
    axes[1].set_ylabel("Number of Samples")
    
    for i, v in enumerate(cluster_sizes):
        axes[1].text(i, v + 5, str(v), ha='center')
    
    plt.tight_layout()
    
    if save_path:
        plt.savefig(save_path, dpi=150, bbox_inches='tight')
    
    plt.show()


def plot_dendrogram_with_threshold(Z, X, thresholds=[2, 3, 4, 5], save_path=None):
    """
    Plot dendrogram with different cut thresholds.
    
    Parameters:
    -----------
    Z : ndarray
        Linkage matrix
    X : ndarray
        Feature matrix
    thresholds : list
        List of threshold values
    save_path : str
        Path to save plot
    """
    fig, axes = plt.subplots(2, 2, figsize=(14, 10))
    axes = axes.flatten()
    
    for idx, threshold in enumerate(thresholds):
        labels = fcluster(Z, threshold, criterion='distance')
        n_clusters = len(np.unique(labels))
        
        unique_labels = np.unique(labels)
        colors = plt.cm.tab10(np.linspace(0, 1, len(unique_labels)))
        
        for label in unique_labels:
            mask = labels == label
            axes[idx].scatter(X[mask, 0], X[mask, 1], 
                             c=[colors[label-1]], 
                             label=f'Cluster {label}',
                             alpha=0.6, edgecolors='w', s=50)
        
        axes[idx].set_title(f'Threshold: {threshold}, Clusters: {n_clusters}', 
                          fontweight='bold')
        axes[idx].set_xlabel('Feature 1')
        axes[idx].set_ylabel('Feature 2')
        axes[idx].legend()
        axes[idx].grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    if save_path:
        plt.savefig(save_path, dpi=150, bbox_inches='tight')
    
    plt.show()


# ================================================================================
# SECTION 6: BANKING EXAMPLE - CUSTOMER SEGMENTATION
# ================================================================================

def banking_customer_segmentation(n_customers=500, random_state=42):
    """
    Complete banking customer segmentation using hierarchical clustering.
    
    This example demonstrates:
    - Data generation for banking customers
    - Feature preprocessing
    - Hierarchical clustering implementation
    - Customer segment analysis
    - Interpretation of results
    
    Parameters:
    -----------
    n_customers : int
        Number of customers to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Segmentation results and analysis
    """
    print("="*70)
    print("BANKING CUSTOMER SEGMENTATION - HIERARCHICAL CLUSTERING")
    print("="*70)
    
    # Step 1: Generate customer data
    print("\n1. Generating customer data...")
    df_customers = generate_banking_customer_data(n_customers, random_state)
    
    # Step 2: Feature selection and preprocessing
    print("\n2. Feature preprocessing...")
    feature_columns = [
        'annual_income', 'credit_score', 'account_balance',
        'transaction_frequency', 'loan_amount', 'age', 'relationship_years'
    ]
    
    X = df_customers[feature_columns].values
    
    # Standardize features
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"Features used: {feature_columns}")
    print(f"Number of features: {len(feature_columns)}")
    
    # Step 3: Create dendrogram
    print("\n3. Creating dendrogram...")
    Z = compute_linkage_matrix(X_scaled, method='ward')
    
    fig, ax = plt.subplots(figsize=(16, 8))
    dendrogram(Z, truncate_mode='last', p=30, ax=ax, 
              leaf_rotation=90, leaf_font_size=6)
    ax.set_title('Customer Segmentation Dendrogram', fontsize=14, fontweight='bold')
    ax.set_xlabel('Customer Index', fontsize=12)
    ax.set_ylabel('Distance (Ward)', fontsize=12)
    ax.axhline(y=10, color='r', linestyle='--', label='Cut threshold')
    ax.legend()
    plt.tight_layout()
    plt.show()
    
    # Step 4: Determine optimal number of clusters
    print("\n4. Analyzing cluster quality...")
    cluster_analysis = []
    
    for n in range(2, 10):
        labels = fcluster(Z, n, criterion='maxclust')
        silhouette = silhouette_score(X_scaled, labels)
        calinski = calinski_harabasz_score(X_scaled, labels)
        davies = davies_bouldin_score(X_scaled, labels)
        
        cluster_analysis.append({
            'n_clusters': n,
            'silhouette': silhouette,
            'calinski_harabasz': calinski,
            'davies_bouldin': davies
        })
    
    df_analysis = pd.DataFrame(cluster_analysis)
    print(df_analysis.to_string(index=False))
    
    # Choose optimal number based on silhouette score
    optimal_n = df_analysis.loc[df_analysis['silhouette'].idxmax(), 'n_clusters']
    print(f"\nOptimal number of clusters (by silhouette): {optimal_n}")
    
    # Step 5: Final clustering
    print(f"\n5. Performing hierarchical clustering with {optimal_n} clusters...")
    labels = fcluster(Z, optimal_n, criterion='maxclust')
    
    df_customers['cluster'] = labels
    
    # Step 6: Analyze clusters
    print("\n6. Cluster analysis...")
    cluster_summary = df_customers.groupby('cluster')[feature_columns].agg(['mean', 'std', 'count'])
    print(f"\nCluster Summary:")
    print(cluster_summary.to_string())
    
    # Step 7: Visualize clusters (using first 2 PCA components for visualization)
    from sklearn.decomposition import PCA
    
    pca = PCA(n_components=2)
    X_pca = pca.fit_transform(X_scaled)
    
    fig, axes = plt.subplots(1, 2, figsize=(14, 6))
    
    # PCA visualization
    colors = plt.cm.tab10(np.linspace(0, 1, optimal_n))
    for i in range(optimal_n):
        mask = labels == i + 1
        axes[0].scatter(X_pca[mask, 0], X_pca[mask, 1], 
                        c=[colors[i]], label=f'Cluster {i+1}',
                        alpha=0.6, edgecolors='w', s=50)
    
    axes[0].set_title(f'Customer Segments (PCA)\n{optimal_n} Clusters', 
                     fontsize=12, fontweight='bold')
    axes[0].set_xlabel(f'PC1 ({pca.explained_variance_ratio_[0]*100:.1f}%)')
    axes[0].set_ylabel(f'PC2 ({pca.explained_variance_ratio_[1]*100:.1f}%)')
    axes[0].legend()
    axes[0].grid(True, alpha=0.3)
    
    # Feature comparison (Bar chart of cluster means)
    cluster_means = df_customers.groupby('cluster')[feature_columns].mean()
    cluster_means_normalized = (cluster_means - cluster_means.min()) / (cluster_means.max() - cluster_means.min())
    
    cluster_means_normalized.T.plot(kind='bar', ax=axes[1], colormap='tab10')
    axes[1].set_title('Cluster Feature Comparison (Normalized)', 
                     fontsize=12, fontweight='bold')
    axes[1].set_xlabel('Feature')
    axes[1].set_ylabel('Normalized Value')
    axes[1].legend(title='Cluster')
    axes[1].tick_params(axis='x', rotation=45)
    
    plt.tight_layout()
    plt.show()
    
    # Step 8: Generate segment profiles
    print("\n7. Customer Segment Profiles:")
    
    for cluster_id in sorted(df_customers['cluster'].unique()):
        cluster_data = df_customers[df_customers['cluster'] == cluster_id]
        
        print(f"\n--- Cluster {cluster_id} ({len(cluster_data)} customers) ---")
        print(f"Average Income: ${cluster_data['annual_income'].mean():,.0f}")
        print(f"Average Credit Score: {cluster_data['credit_score'].mean():.0f}")
        print(f"Average Balance: ${cluster_data['account_balance'].mean():,.0f}")
        print(f"Average Transactions/Month: {cluster_data['transaction_frequency'].mean():.1f}")
        print(f"Average Loan Amount: ${cluster_data['loan_amount'].mean():,.0f}")
        print(f"Average Age: {cluster_data['age'].mean():.1f}")
        print(f"Average Relationship Years: {cluster_data['relationship_years'].mean():.1f}")
    
    results = {
        'df_customers': df_customers,
        'labels': labels,
        'optimal_n': optimal_n,
        'cluster_analysis': df_analysis,
        'scaler': scaler,
        'feature_columns': feature_columns
    }
    
    return results


# ================================================================================
# SECTION 7: HEALTHCARE EXAMPLE - PATIENT GROUPING
# ================================================================================

def healthcare_patient_grouping(n_patients=600, random_state=42):
    """
    Complete healthcare patient grouping using hierarchical clustering.
    
    This example demonstrates:
    - Data generation for patient data
    - Multiple feature types
    - Hierarchical clustering with different configurations
    - Patient group interpretation
    
    Parameters:
    -----------
    n_patients : int
        Number of patients to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Grouping results and analysis
    """
    print("="*70)
    print("HEALTHCARE PATIENT GROUPING - HIERARCHICAL CLUSTERING")
    print("="*70)
    
    # Step 1: Generate patient data
    print("\n1. Generating patient data...")
    df_patients = generate_healthcare_patient_data(n_patients, random_state)
    
    # Step 2: Feature selection
    print("\n2. Feature preprocessing...")
    feature_columns = [
        'age', 'bmi', 'blood_pressure_systolic', 'blood_pressure_diastolic',
        'heart_rate', 'cholesterol_total', 'glucose_level', 
        'visit_frequency', 'num_chronic_conditions'
    ]
    
    X = df_patients[feature_columns].values
    
    # Standardize features
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"Features used: {feature_columns}")
    print(f"Number of features: {len(feature_columns)}")
    
    # Step 3: Create dendrogram
    print("\n3. Creating dendrogram...")
    Z = compute_linkage_matrix(X_scaled, method='ward')
    
    fig, ax = plt.subplots(figsize=(16, 8))
    dendrogram(Z, truncate_mode='last', p=30, ax=ax, 
              leaf_rotation=90, leaf_font_size=6)
    ax.set_title('Patient Grouping Dendrogram', fontsize=14, fontweight='bold')
    ax.set_xlabel('Patient Index', fontsize=12)
    ax.set_ylabel('Distance (Ward)', fontsize=12)
    ax.axhline(y=12, color='r', linestyle='--', label='Cut threshold')
    ax.legend()
    plt.tight_layout()
    plt.show()
    
    # Step 4: Compare linkage methods
    print("\n4. Comparing linkage methods...")
    linkage_methods = ['single', 'complete', 'average', 'ward']
    linkage_results = perform_linkage_analysis(X_scaled[:500], linkage_methods)
    
    # Step 5: Determine optimal number of clusters
    print("\n5. Analyzing cluster quality...")
    cluster_analysis = []
    
    for n in range(2, 10):
        labels = fcluster(Z, n, criterion='maxclust')
        silhouette = silhouette_score(X_scaled, labels)
        calinski = calinski_harabasz_score(X_scaled, labels)
        davies = davies_bouldin_score(X_scaled, labels)
        
        cluster_analysis.append({
            'n_clusters': n,
            'silhouette': silhouette,
            'calinski_harabasz': calinski,
            'davies_bouldin': davies
        })
    
    df_analysis = pd.DataFrame(cluster_analysis)
    print(df_analysis.to_string(index=False))
    
    optimal_n = df_analysis.loc[df_analysis['silhouette'].idxmax(), 'n_clusters']
    print(f"\nOptimal number of clusters: {optimal_n}")
    
    # Step 6: Final clustering
    print(f"\n6. Performing hierarchical clustering with {optimal_n} clusters...")
    labels = fcluster(Z, optimal_n, criterion='maxclust')
    
    df_patients['cluster'] = labels
    
    # Step 7: Visualize using PCA
    from sklearn.decomposition import PCA
    
    pca = PCA(n_components=2)
    X_pca = pca.fit_transform(X_scaled)
    
    fig, axes = plt.subplots(1, 2, figsize=(14, 6))
    
    colors = plt.cm.tab10(np.linspace(0, 1, optimal_n))
    for i in range(optimal_n):
        mask = labels == i + 1
        axes[0].scatter(X_pca[mask, 0], X_pca[mask, 1], 
                        c=[colors[i]], label=f'Group {i+1}',
                        alpha=0.6, edgecolors='w', s=50)
    
    axes[0].set_title(f'Patient Groups (PCA)\n{optimal_n} Groups', 
                     fontsize=12, fontweight='bold')
    axes[0].set_xlabel(f'PC1 ({pca.explained_variance_ratio_[0]*100:.1f}%)')
    axes[0].set_ylabel(f'PC2 ({pca.explained_variance_ratio_[1]*100:.1f}%)')
    axes[0].legend()
    axes[0].grid(True, alpha=0.3)
    
    # Cluster size distribution
    cluster_sizes = df_patients['cluster'].value_counts().sort_index()
    axes[1].bar(cluster_sizes.index, cluster_sizes.values, color=colors)
    axes[1].set_title('Patient Group Sizes', fontsize=12, fontweight='bold')
    axes[1].set_xlabel('Group')
    axes[1].set_ylabel('Number of Patients')
    for i, v in enumerate(cluster_sizes.values):
        axes[1].text(cluster_sizes.index[i], v + 5, str(v), ha='center')
    
    plt.tight_layout()
    plt.show()
    
    # Step 8: Analyze patient groups
    print("\n7. Patient Group Profiles:")
    
    for cluster_id in sorted(df_patients['cluster'].unique()):
        cluster_data = df_patients[df_patients['cluster'] == cluster_id]
        
        print(f"\n--- Group {cluster_id} ({len(cluster_data)} patients) ---")
        print(f"Average Age: {cluster_data['age'].mean():.1f} years")
        print(f"Average BMI: {cluster_data['bmi'].mean():.1f}")
        print(f"Average BP (Systolic): {cluster_data['blood_pressure_systolic'].mean():.0f} mmHg")
        print(f"Average BP (Diastolic): {cluster_data['blood_pressure_diastolic'].mean():.0f} mmHg")
        print(f"Average Heart Rate: {cluster_data['heart_rate'].mean():.0f} bpm")
        print(f"Average Cholesterol: {cluster_data['cholesterol_total'].mean():.0f} mg/dL")
        print(f"Average Glucose: {cluster_data['glucose_level'].mean():.0f} mg/dL")
        print(f"Average Annual Visits: {cluster_data['visit_frequency'].mean():.1f}")
        print(f"Average Chronic Conditions: {cluster_data['num_chronic_conditions'].mean():.1f}")
    
    # Step 9: Risk stratification
    print("\n8. Risk Stratification:")
    
    risk_scores = []
    for cluster_id in sorted(df_patients['cluster'].unique()):
        cluster_data = df_patients[df_patients['cluster'] == cluster_id]
        
        # Calculate risk score based on various factors
        risk_score = (
            cluster_data['blood_pressure_systolic'].mean() / 120 +  # BP normalized
            cluster_data['cholesterol_total'].mean() / 200 +  # Cholesterol normalized
            cluster_data['glucose_level'].mean() / 100 +  # Glucose normalized
            cluster_data['num_chronic_conditions'].mean()  # Chronic conditions
        ) / 4
        
        risk_label = 'Low'
        if risk_score > 1.2:
            risk_label = 'High'
        elif risk_score > 1.0:
            risk_label = 'Medium'
        
        risk_scores.append({
            'group': cluster_id,
            'risk_score': risk_score,
            'risk_label': risk_label,
            'n_patients': len(cluster_data)
        })
        
        print(f"Group {cluster_id}: Risk Score = {risk_score:.2f} ({risk_label})")
    
    results = {
        'df_patients': df_patients,
        'labels': labels,
        'optimal_n': optimal_n,
        'cluster_analysis': df_analysis,
        'scaler': scaler,
        'feature_columns': feature_columns,
        'risk_scores': risk_scores
    }
    
    return results


# ================================================================================
# SECTION 8: TESTING AND EVALUATION
# ================================================================================

def test_linkage_methods():
    """
    Test different linkage methods on synthetic data.
    """
    print("="*70)
    print("TESTING LINKAGE METHODS")
    print("="*70)
    
    # Generate test data
    X, y = generate_synthetic_blobs(n_samples=300, centers=4, random_state=42)
    
    # Test each linkage method
    methods = ['single', 'complete', 'average', 'ward']
    
    results = perform_linkage_analysis(X[:300], methods)
    
    # Visualize results
    plot_cluster_comparison(X, results)
    
    return results


def test_distance_metrics():
    """
    Test different distance metrics.
    """
    print("="*70)
    print("TESTING DISTANCE METRICS")
    print("="*70)
    
    # Generate test data
    X, y = generate_synthetic_blobs(n_samples=300, centers=3, random_state=42)
    
    # Test each distance metric
    metrics = ['euclidean', 'manhattan']
    
    results = compare_distance_metrics(X[:100], metrics)
    
    return results


def test_dendrogram_cutting():
    """
    Test dendrogram cutting at different heights.
    """
    print("="*70)
    print("TESTING DENDROGRAM CUTTING")
    print("="*70)
    
    # Generate test data
    X, y = generate_synthetic_blobs(n_samples=300, centers=4, random_state=42)
    
    # Compute linkage
    Z = compute_linkage_matrix(X, method='ward')
    
    # Test different cut heights
    heights = [2, 3, 4, 5, 6, 7]
    
    fig, axes = plt.subplots(2, 3, figsize=(18, 10))
    axes = axes.flatten()
    
    for idx, height in enumerate(heights):
        labels = fcluster(Z, height, criterion='distance')
        n_clusters = len(np.unique(labels))
        
        colors = plt.cm.tab10(np.linspace(0, 1, n_clusters))
        
        for label in np.unique(labels):
            mask = labels == label
            axes[idx].scatter(X[mask, 0], X[mask, 1], 
                             c=[colors[label-1]], 
                             label=f'Cluster {label}',
                             alpha=0.6, edgecolors='w', s=40)
        
        silhouette = silhouette_score(X, labels)
        axes[idx].set_title(f'Height: {height}, Clusters: {n_clusters}\nSilhouette: {silhouette:.3f}', 
                           fontweight='bold')
        axes[idx].set_xlabel('Feature 1')
        axes[idx].set_ylabel('Feature 2')
        axes[idx].legend()
        axes[idx].grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()
    
    return Z


def test_complex_shapes():
    """
    Test clustering on complex cluster shapes.
    """
    print("="*70)
    print("TESTING COMPLEX SHAPES")
    print("="*70)
    
    # Generate complex data
    X, y = generate_complex_clusters(n_samples=300, random_state=42)
    
    # Test each linkage method
    methods = ['single', 'complete', 'average', 'ward']
    
    results = {}
    for method in methods:
        labels, model = core_hierarchical_clustering(X, n_clusters=3, linkage_method=method)
        silhouette = silhouette_score(X, labels)
        
        results[method] = {
            'labels': labels,
            'model': model,
            'silhouette': silhouette
        }
        
        print(f"Method: {method}, Silhouette: {silhouette:.4f}")
    
    # Visualize results
    plot_cluster_comparison(X, results)
    
    return results


# ================================================================================
# SECTION 9: ADVANCED TOPICS
# ================================================================================

def hierarchical_clustering_with_distance_threshold(X, distance_threshold=5.0):
    """
    Hierarchical clustering using distance threshold instead of n_clusters.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    distance_threshold : float
        Distance threshold for cutting
        
    Returns:
    --------
    labels : ndarray
        Cluster labels
    model : AgglomerativeClustering
        Fitted model
    """
    model = AgglomerativeClustering(
        n_clusters=None,
        distance_threshold=distance_threshold,
        linkage='ward'
    )
    
    labels = model.fit_predict(X)
    
    n_clusters = len(np.unique(labels))
    print(f"Distance threshold: {distance_threshold}")
    print(f"Number of clusters: {n_clusters}")
    
    return labels, model


def find_optimal_clusters(X, method='ward', max_clusters=10):
    """
    Find optimal number of clusters using various metrics.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    method : str
        Linkage method
    max_clusters : int
        Maximum number of clusters to consider
        
    Returns:
    --------
    optimal_n : int
        Optimal number of clusters
    analysis : DataFrame
        Analysis results
    """
    Z = linkage(X, method=method)
    
    results = []
    
    for n in range(2, max_clusters + 1):
        labels = fcluster(Z, n, criterion='maxclust')
        
        silhouette = silhouette_score(X, labels)
        calinski = calinski_harabasz_score(X, labels)
        davies = davies_bouldin_score(X, labels)
        
        # Calculate cluster coherence
        coherences = []
        for cluster in np.unique(labels):
            cluster_points = X[labels == cluster]
            if len(cluster_points) > 1:
                cluster_dist = pdist(cluster_points)
                coherence = cluster_dist.mean()
                coherences.append(coherence)
        
        avg_coherence = np.mean(coherences) if coherences else 0
        
        results.append({
            'n_clusters': n,
            'silhouette': silhouette,
            'calinski_harabasz': calinski,
            'davies_bouldin': davies,
            'avg_coherence': avg_coherence
        })
    
    df = pd.DataFrame(results)
    
    # Find optimal using silhouette
    optimal_silhouette = df.loc[df['silhouette'].idxmax(), 'n_clusters']
    
    print("\n=== Optimal Cluster Analysis ===")
    print(df.to_string(index=False))
    print(f"\nOptimal clusters (silhouette): {optimal_silhouette}")
    
    return optimal_silhouette, df


def cluster_with_precomputed_distance(X, precomputed_distances):
    """
    Hierarchical clustering using precomputed distance matrix.
    """
    from sklearn.cluster import AgglomerativeClustering
    
    # Use precomputed distances (must be squareform)
    model = AgglomerativeClustering(
        n_clusters=4,
        metric='precomputed',
        linkage='average'
    )
    
    labels = model.fit_predict(precomputed_distances)
    
    return labels, model


def incremental_hierarchical_clustering(X, batch_size=50):
    """
    Simulate incremental hierarchical clustering.
    """
    n_samples = X.shape[0]
    
    # Process in batches
    labels = np.zeros(n_samples)
    batch_labels = []
    
    for i in range(0, n_samples, batch_size):
        batch = X[i:min(i+batch_size, n_samples)]
        
        if len(batch_labels) == 0:
            # First batch - cluster it
            model = AgglomerativeClustering(n_clusters=3, linkage='ward')
            batch_label = model.fit_predict(batch)
        else:
            # Subsequent batches - assign to existing clusters
            # This is a simplified approach
            model = AgglomerativeClustering(n_clusters=3, linkage='ward')
            batch_label = model.fit_predict(batch)
        
        batch_labels.extend(batch_label.tolist())
    
    return np.array(batch_labels[:n_samples])


# ================================================================================
# SECTION 10: PERFORMANCE OPTIMIZATION
# ================================================================================

def optimize_linkage_computation(X, method='ward'):
    """
    Optimize linkage computation for large datasets.
    """
    import time
    
    # For large datasets, use truncated dendrogram
    n_samples = X.shape[0]
    
    if n_samples > 1000:
        print(f"Large dataset ({n_samples} samples) - using optimization")
        
        # Sample for linkage computation
        sample_size = min(500, n_samples)
        indices = np.random.choice(n_samples, sample_size, replace=False)
        X_sample = X[indices]
        
        start_time = time.time()
        Z = linkage(X_sample, method=method)
        elapsed = time.time() - start_time
        
        print(f"Linkage computed in {elapsed:.2f} seconds")
        
        # Assign remaining points to clusters
        # (simplified - just use sampling)
        return Z
    
    else:
        start_time = time.time()
        Z = linkage(X, method=method)
        elapsed = time.time() - start_time
        
        print(f"Linkage computed in {elapsed:.2f} seconds")
        
        return Z


# ================================================================================
# SECTION 11: MAIN EXECUTION
# ================================================================================

def main():
    """
    Main execution function demonstrating hierarchical clustering.
    """
    print("="*70)
    print("HIERARCHICAL CLUSTERING - COMPREHENSIVE IMPLEMENTATION")
    print("="*70)
    
    # Example 1: Basic hierarchical clustering
    print("\n" + "="*70)
    print("EXAMPLE 1: BASIC HIERARCHICAL CLUSTERING")
    print("="*70)
    
    X, y = generate_synthetic_blobs(n_samples=300, centers=4, random_state=42)
    
    # Standardize
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    # Create dendrogram
    Z = compute_linkage_matrix(X_scaled, method='ward')
    
    # Cut at different heights
    labels_3 = fcluster(Z, 3, criterion='maxclust')
    labels_4 = fcluster(Z, 4, criterion='maxclust')
    
    silhouette_3 = silhouette_score(X_scaled, labels_3)
    silhouette_4 = silhouette_score(X_scaled, labels_4)
    
    print(f"Silhouette (3 clusters): {silhouette_3:.4f}")
    print(f"Silhouette (4 clusters): {silhouette_4:.4f}")
    
    # Example 2: Compare linkage methods
    print("\n" + "="*70)
    print("EXAMPLE 2: COMPARING LINKAGE METHODS")
    print("="*70)
    
    methods = ['single', 'complete', 'average', 'ward']
    results = perform_linkage_analysis(X_scaled, methods)
    
    # Example 3: Banking customer segmentation
    print("\n" + "="*70)
    print("EXAMPLE 3: BANKING CUSTOMER SEGMENTATION")
    print("="*70)
    
    banking_results = banking_customer_segmentation(n_customers=500, random_state=42)
    
    # Example 4: Healthcare patient grouping
    print("\n" + "="*70)
    print("EXAMPLE 4: HEALTHCARE PATIENT GROUPING")
    print("="*70)
    
    healthcare_results = healthcare_patient_grouping(n_patients=500, random_state=42)
    
    print("\n" + "="*70)
    print("EXECUTION COMPLETE")
    print("="*70)


if __name__ == "__main__":
    main()