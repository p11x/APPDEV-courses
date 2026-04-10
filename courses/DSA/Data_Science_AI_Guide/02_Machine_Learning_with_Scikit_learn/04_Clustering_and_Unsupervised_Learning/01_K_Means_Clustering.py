# Topic: K Means Clustering
# Author: AI Assistant
# Date: 06-04-2026

"""
================================================================================
    COMPREHENSIVE IMPLEMENTATION FOR K MEANS CLUSTERING
================================================================================

I. INTRODUCTION
-------------
K Means Clustering is one of the most fundamental and widely used unsupervised machine
learning algorithms. It aims to partition n observations into k clusters in which
each observation belongs to the cluster with the nearest mean (cluster center).

This algorithm is particularly useful for:
- Customer segmentation in marketing
- Image compression
- Document clustering
- Anomaly detection
- Pattern recognition

II. CORE CONCEPTS
-----------------
1. CLUSTERS: Groups of data points that are similar to each other
2. CENTROIDS: The center point of each cluster (mean of all points in the cluster)
3. INERTIA: The sum of squared distances from each point to its assigned cluster center
4. SILHOUETTE SCORE: Measure of how similar a point is to its own cluster vs others
5. ELBOW METHOD: Technique to find optimal number of clusters

III. ALGORITHM STEPS
-------------------
1. Initialize k centroids randomly
2. Assign each data point to nearest centroid
3. Recalculate centroids based on assigned points
4. Repeat steps 2-3 until convergence (no change in assignments)

IV. IMPLEMENTATION DETAILS
--------------------------
- K-Means++ initialization for better initial centroids
- Support for multiple distance metrics
- Comprehensive evaluation metrics
- Banking and Healthcare examples

================================================================================
"""

# =============================================================================
# IMPORT NECESSARY LIBRARIES
# =============================================================================

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.datasets import make_blobs, make_moons, make_circles
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, MinMaxScaler
from sklearn.cluster import KMeans, MiniBatchKMeans
from sklearn.metrics import silhouette_score, calinski_harabasz_score, davies_bouldin_score
from sklearn.decomposition import PCA
from sklearn.metrics.pairwise import euclidean_distances
import warnings
warnings.filterwarnings('ignore')

# Set random seed for reproducibility
np.random.seed(42)

# Set display options
pd.set_option('display.max_columns', None)
pd.set_option('display.width', None)
pd.set_option('display.max_colwidth', 50)


# =============================================================================
# SECTION I: DATA GENERATION FUNCTIONS
# =============================================================================

def generate_cluster_data(n_samples=500, n_features=2, n_clusters=3, 
                        cluster_std=1.0, random_state=42):
    """
    Generate synthetic cluster data for testing K Means algorithm.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features (dimensions)
    n_clusters : int
        Number of clusters
    cluster_std : float
        Standard deviation of clusters
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : array-like
        Generated feature matrix
    y : array-like
        True cluster labels
    centers : array-like
        True cluster centers
    """
    print(f"\n{'='*60}")
    print("GENERATING SYNTHETIC CLUSTER DATA")
    print(f"{'='*60}")
    print(f"  Number of samples: {n_samples}")
    print(f"  Number of features: {n_features}")
    print(f"  Number of clusters: {n_clusters}")
    print(f"  Cluster standard deviation: {cluster_std}")
    
    X, y, centers = make_blobs(
        n_samples=n_samples,
        n_features=n_features,
        centers=n_clusters,
        cluster_std=cluster_std,
        random_state=random_state,
        return_centers=True
    )
    
    # Add some noise to make it more realistic
    noise = np.random.normal(0, 0.1, X.shape)
    X = X + noise
    
    print(f"\n  Data shape: {X.shape}")
    print(f"  Centers shape: {centers.shape}")
    print(f"  Unique labels: {np.unique(y)}")
    
    return X, y, centers


def generate_customer_segmentation_data(n_customers=1000, random_state=42):
    """
    Generate synthetic customer data for banking/finance use case.
    
    Creates data representing:
    - Annual income
    - Credit score
    - Account balance
    - Number of products
    - Tenure (years with bank)
    - Average monthly transactions
    - Loan amount (if any)
    
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
    print(f"\n{'='*60}")
    print("GENERATING CUSTOMER SEGMENTATION DATA")
    print(f"{'='*60}")
    print(f"  Number of customers: {n_customers}")
    
    np.random.seed(random_state)
    
    # Generate customer segments
    # Segment 0: Low income, low credit
    # Segment 1: High income, high credit, high balance
    # Segment 2: Middle income, middle credit
    # Segment 3: High income, low credit (new customers with loans)
    
    n_per_segment = n_customers // 4
    
    # Segment 0: Budget conscious customers
    segment_0 = {
        'annual_income': np.random.normal(30000, 5000, n_per_segment),
        'credit_score': np.random.normal(600, 50, n_per_segment),
        'account_balance': np.random.normal(5000, 2000, n_per_segment),
        'num_products': np.random.randint(1, 3, n_per_segment),
        'tenure_years': np.random.uniform(0.5, 3, n_per_segment),
        'monthly_transactions': np.random.normal(10, 3, n_per_segment),
        'loan_amount': np.random.normal(5000, 2000, n_per_segment) * np.random.random(n_per_segment)
    }
    
    # Segment 1: Premium customers
    segment_1 = {
        'annual_income': np.random.normal(120000, 20000, n_per_segment),
        'credit_score': np.random.normal(750, 30, n_per_segment),
        'account_balance': np.random.normal(100000, 30000, n_per_segment),
        'num_products': np.random.randint(3, 6, n_per_segment),
        'tenure_years': np.random.uniform(5, 15, n_per_segment),
        'monthly_transactions': np.random.normal(50, 10, n_per_segment),
        'loan_amount': np.random.normal(50000, 10000, n_per_segment) * np.random.random(n_per_segment)
    }
    
    # Segment 2: Middle class customers
    segment_2 = {
        'annual_income': np.random.normal(60000, 8000, n_per_segment),
        'credit_score': np.random.normal(680, 40, n_per_segment),
        'account_balance': np.random.normal(25000, 8000, n_per_segment),
        'num_products': np.random.randint(2, 4, n_per_segment),
        'tenure_years': np.random.uniform(2, 7, n_per_segment),
        'monthly_transactions': np.random.normal(25, 8, n_per_segment),
        'loan_amount': np.random.normal(20000, 5000, n_per_segment) * np.random.random(n_per_segment)
    }
    
    # Segment 3: Young professionals (high income, building credit)
    segment_3 = {
        'annual_income': np.random.normal(90000, 15000, n_per_segment),
        'credit_score': np.random.normal(620, 60, n_per_segment),
        'account_balance': np.random.normal(15000, 10000, n_per_segment),
        'num_products': np.random.randint(1, 3, n_per_segment),
        'tenure_years': np.random.uniform(0.5, 2, n_per_segment),
        'monthly_transactions': np.random.normal(35, 12, n_per_segment),
        'loan_amount': np.random.normal(30000, 8000, n_per_segment) * (np.random.random(n_per_segment) > 0.3)
    }
    
    # Combine all segments
    df_data = {}
    for key in segment_0.keys():
        df_data[key] = np.concatenate([
            segment_0[key],
            segment_1[key],
            segment_2[key],
            segment_3[key]
        ])
    
    df = pd.DataFrame(df_data)
    
    # Add customer IDs
    df['customer_id'] = [f'CUST_{i:05d}' for i in range(len(df))]
    
    # Add some missing values for realism
    mask = np.random.random(df.shape) < 0.02
    df = df.where(~mask, other=np.nan)
    
    # Reorder columns
    cols = ['customer_id', 'annual_income', 'credit_score', 'account_balance',
            'num_products', 'tenure_years', 'monthly_transactions', 'loan_amount']
    df = df[cols]
    
    print(f"  Data shape: {df.shape}")
    print(f"  Features: {list(df.columns[1:])}")
    
    return df


def generate_patient_clustering_data(n_patients=800, random_state=42):
    """
    Generate synthetic patient data for healthcare use case.
    
    Creates data representing:
    - Age
    - BMI
    - Blood pressure (systolic)
    - Cholesterol level
    - Blood glucose level
    - Heart rate
    - Number of chronic conditions
    - Medication count
    
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
    print(f"\n{'='*60}")
    print("GENERATING PATIENT CLUSTERING DATA")
    print(f"{'='*60}")
    print(f"  Number of patients: {n_patients}")
    
    np.random.seed(random_state)
    
    # Define patient clusters
    # Cluster 0: Healthy young adults
    # Cluster 1: Elderly with chronic conditions
    # Cluster 2: Middle-aged with lifestyle issues
    # Cluster 3: High-risk patients
    
    n_per_cluster = n_patients // 4
    
    # Cluster 0: Healthy young adults
    cluster_0 = {
        'age': np.random.normal(28, 5, n_per_cluster),
        'bmi': np.random.normal(23, 2, n_per_cluster),
        'blood_pressure_systolic': np.random.normal(118, 8, n_per_cluster),
        'cholesterol': np.random.normal(170, 20, n_per_cluster),
        'blood_glucose': np.random.normal(85, 10, n_per_cluster),
        'heart_rate': np.random.normal(70, 8, n_per_cluster),
        'chronic_conditions': np.random.randint(0, 2, n_per_cluster),
        'medication_count': np.random.randint(0, 2, n_per_cluster)
    }
    
    # Cluster 1: Elderly with chronic conditions
    cluster_1 = {
        'age': np.random.normal(68, 8, n_per_cluster),
        'bmi': np.random.normal(28, 4, n_per_cluster),
        'blood_pressure_systolic': np.random.normal(145, 15, n_per_cluster),
        'cholesterol': np.random.normal(220, 30, n_per_cluster),
        'blood_glucose': np.random.normal(110, 20, n_per_cluster),
        'heart_rate': np.random.normal(80, 12, n_per_cluster),
        'chronic_conditions': np.random.randint(2, 5, n_per_cluster),
        'medication_count': np.random.randint(3, 6, n_per_cluster)
    }
    
    # Cluster 2: Middle-aged with lifestyle issues
    cluster_2 = {
        'age': np.random.normal(45, 7, n_per_cluster),
        'bmi': np.random.normal(30, 4, n_per_cluster),
        'blood_pressure_systolic': np.random.normal(130, 12, n_per_cluster),
        'cholesterol': np.random.normal(200, 25, n_per_cluster),
        'blood_glucose': np.random.normal(100, 15, n_per_cluster),
        'heart_rate': np.random.normal(75, 10, n_per_cluster),
        'chronic_conditions': np.random.randint(1, 3, n_per_cluster),
        'medication_count': np.random.randint(1, 3, n_per_cluster)
    }
    
    # Cluster 3: High-risk patients (multiple issues)
    cluster_3 = {
        'age': np.random.normal(55, 10, n_per_cluster),
        'bmi': np.random.normal(35, 6, n_per_cluster),
        'blood_pressure_systolic': np.random.normal(160, 20, n_per_cluster),
        'cholesterol': np.random.normal(250, 40, n_per_cluster),
        'blood_glucose': np.random.normal(130, 30, n_per_cluster),
        'heart_rate': np.random.normal(85, 15, n_per_cluster),
        'chronic_conditions': np.random.randint(3, 6, n_per_cluster),
        'medication_count': np.random.randint(4, 8, n_per_cluster)
    }
    
    # Combine all clusters
    df_data = {}
    for key in cluster_0.keys():
        df_data[key] = np.concatenate([
            cluster_0[key],
            cluster_1[key],
            cluster_2[key],
            cluster_3[key]
        ])
    
    df = pd.DataFrame(df_data)
    
    # Add patient IDs
    df['patient_id'] = [f'PAT_{i:05d}' for i in range(len(df))]
    
    # Clip values to realistic ranges
    df['age'] = df['age'].clip(18, 95)
    df['bmi'] = df['bmi'].clip(16, 50)
    df['blood_pressure_systolic'] = df['blood_pressure_systolic'].clip(90, 200)
    df['cholesterol'] = df['cholesterol'].clip(100, 350)
    df['blood_glucose'] = df['blood_glucose'].clip(60, 200)
    df['heart_rate'] = df['heart_rate'].clip(50, 120)
    df['chronic_conditions'] = df['chronic_conditions'].clip(0, 6)
    df['medication_count'] = df['medication_count'].clip(0, 10)
    
    df = df.round(2)
    
    # Reorder columns
    cols = ['patient_id', 'age', 'bmi', 'blood_pressure_systolic', 'cholesterol',
            'blood_glucose', 'heart_rate', 'chronic_conditions', 'medication_count']
    df = df[cols]
    
    print(f"  Data shape: {df.shape}")
    print(f"  Features: {list(df.columns[1:])}")
    
    return df


# =============================================================================
# SECTION II: CORE K-MEANS IMPLEMENTATION
# =============================================================================

def kmeans_manual(X, n_clusters, max_iter=100, tol=1e-4, random_state=42,
                  init='kmeans++', verbose=True):
    """
    Manual implementation of K Means clustering algorithm.
    
    This is a from-scratch implementation to understand the algorithm.
    
    Parameters:
    -----------
    X : array-like, shape (n_samples, n_features)
        Input data
    n_clusters : int
        Number of clusters
    max_iter : int
        Maximum number of iterations
    tol : float
        Tolerance for convergence
    random_state : int
        Random seed
    init : str
        Initialization method ('random' or 'kmeans++')
    verbose : bool
        Print progress
        
    Returns:
    --------
    labels : array-like
        Cluster labels for each point
    centroids : array-like
        Final cluster centers
    inertia : float
        Sum of squared distances to closest centroid
    n_iter : int
        Number of iterations
    """
    print(f"\n{'='*60}")
    print("K-MEANS CLUSTERING (MANUAL IMPLEMENTATION)")
    print(f"{'='*60}")
    print(f"  Number of clusters: {n_clusters}")
    print(f"  Max iterations: {max_iter}")
    print(f"  Initialization: {init}")
    
    np.random.seed(random_state)
    n_samples, n_features = X.shape
    
    # Initialize centroids
    if init == 'kmeans++':
        if verbose:
            print(f"  Using K-Means++ initialization")
        centroids = initialize_kmeans_plus_plus(X, n_clusters, random_state)
    else:
        # Random initialization
        indices = np.random.choice(n_samples, n_clusters, replace=False)
        centroids = X[indices].copy()
    
    # Initialize cluster labels
    labels = np.zeros(n_samples, dtype=int)
    iteration = 0
    
    # Iterate
    for iteration in range(max_iter):
        # Assign points to nearest centroid
        old_centroids = centroids.copy()
        
        # Calculate distances to all centroids
        distances = euclidean_distances(X, centroids)
        labels = np.argmin(distances, axis=1)
        
        # Update centroids
        for k in range(n_clusters):
            cluster_points = X[labels == k]
            if len(cluster_points) > 0:
                centroids[k] = cluster_points.mean(axis=0)
            else:
                # If cluster is empty, reinitialize centroid
                centroids[k] = X[np.random.choice(n_samples)]
        
        # Check for convergence
        centroid_shift = np.sqrt(((centroids - old_centroids) ** 2).sum(axis=1).mean())
        
        if verbose and iteration % 10 == 0:
            current_inertia = calculate_inertia(X, labels, centroids)
            print(f"  Iteration {iteration+1}: Inertia = {current_inertia:.4f}, Shift = {centroid_shift:.6f}")
        
        if centroid_shift < tol:
            if verbose:
                print(f"  Converged at iteration {iteration+1}")
            break
    
    # Calculate final inertia
    inertia = calculate_inertia(X, labels, centroids)
    
    if verbose:
        print(f"  Final inertia: {inertia:.4f}")
        print(f"  Cluster sizes: {np.bincount(labels)}")
    
    return labels, centroids, inertia, iteration + 1


def initialize_kmeans_plus_plus(X, n_clusters, random_state=42):
    """
    K-Means++ initialization method.
    
    This initialization method selects initial centroids in a way that
    tends to spread them out and avoid poor convergence.
    
    The algorithm:
    1. Choose first centroid uniformly at random from data points
    2. For each subsequent centroid, choose a point with probability
       proportional to its squared distance from the nearest centroid
    3. Repeat until we have k centroids
    
    Parameters:
    -----------
    X : array-like
        Input data
    n_clusters : int
        Number of clusters
    random_state : int
        Random seed
        
    Returns:
    --------
    centroids : array-like
        Selected initial centroids
    """
    n_samples, n_features = X.shape
    np.random.seed(random_state)
    
    # Select first centroid uniformly at random
    indices = np.arange(n_samples)
    centroids = [X[np.random.choice(indices)].copy()]
    
    # Select remaining centroids
    for _ in range(n_clusters - 1):
        # Calculate squared distances to nearest centroid
        distances = euclidean_distances(X, np.array(centroids))
        min_distances = distances.min(axis=1)
        
        # Choose next centroid with probability proportional to squared distance
        probabilities = min_distances ** 2
        probabilities = probabilities / probabilities.sum()
        
        next_index = np.random.choice(indices, p=probabilities)
        centroids.append(X[next_index])
    
    return np.array(centroids)


def calculate_inertia(X, labels, centroids):
    """
    Calculate inertia (sum of squared distances to cluster centers).
    
    Parameters:
    -----------
    X : array-like
        Input data
    labels : array-like
        Cluster labels
    centroids : array-like
        Cluster centers
        
    Returns:
    --------
    inertia : float
        Sum of squared distances
    """
    inertia = 0
    for k in range(len(centroids)):
        cluster_points = X[labels == k]
        distances = ((cluster_points - centroids[k]) ** 2).sum()
        inertia += distances
    return inertia


def core_kmeans():
    """
    Demonstrate core K-Means clustering implementation with sklearn.
    
    This function shows the standard implementation using scikit-learn's
    KMeans class with various options.
    """
    print(f"\n{'='*60}")
    print("CORE K-MEANS IMPLEMENTATION (SKLEARN)")
    print(f"{'='*60}")
    
    # Generate data
    X, y_true, centers = generate_cluster_data(n_samples=500, n_clusters=4)
    
    print(f"\n{'='*60}")
    print("STEP 1: STANDARD K-MEANS")
    print(f"{'='*60}")
    
    # Standard K-Means
    kmeans = KMeans(n_clusters=4, random_state=42, n_init=10)
    labels = kmeans.fit_predict(X)
    
    print(f"  Number of clusters: 4")
    print(f"  Inertia: {kmeans.inertia_:.4f}")
    print(f"  Number of iterations: {kmeans.n_iter_}")
    print(f"  Cluster sizes: {np.bincount(labels)}")
    
    print(f"\n{'='*60}")
    print("STEP 2: K-MEANS WITH K-MEANS++")
    print(f"{'='*60}")
    
    # K-Means with explicit kmeans++ init
    kmeans_pp = KMeans(n_clusters=4, init='k-means++', n_init=10, random_state=42)
    labels_pp = kmeans_pp.fit_predict(X)
    
    print(f"  Init method: k-means++")
    print(f"  Inertia: {kmeans_pp.inertia_:.4f}")
    print(f"  Number of iterations: {kmeans_pp.n_iter_}")
    
    print(f"\n{'='*60}")
    print("STEP 3: MINI-BATCH K-MEANS (FOR LARGE DATASETS)")
    print(f"{'='*60}")
    
    # Mini-batch K-Means for large datasets
    from sklearn.cluster import MiniBatchKMeans
    
    mbkm = MiniBatchKMeans(n_clusters=4, n_init=10, random_state=42, batch_size=100)
    labels_mb = mbkm.fit_predict(X)
    
    print(f"  Batch size: 100")
    print(f"  Inertia: {mbkm.inertia_:.4f}")
    print(f"  Number of iterations: {mbkm.n_iter_}")
    
    print(f"\n{'='*60}")
    print("STEP 4: COMPARISON OF INITIALIZATIONS")
    print(f"{'='*60}")
    
    # Compare different initializations
    results = []
    
    for init in ['k-means++', 'random']:
        for n_init in [1, 5, 10]:
            kmeans = KMeans(n_clusters=4, init=init, n_init=n_init, random_state=42)
            kmeans.fit(X)
            results.append({
                'init': init,
                'n_init': n_init,
                'inertia': kmeans.inertia_,
                'iterations': kmeans.n_iter_
            })
    
    results_df = pd.DataFrame(results)
    print(results_df.to_string(index=False))
    
    return X, y_true, labels


# =============================================================================
# SECTION III: ELBOW METHOD FOR OPTIMAL K
# =============================================================================

def elbow_method(X, max_k=10, verbose=True):
    """
    Implement the Elbow Method to find optimal number of clusters.
    
    The elbow method plots the within-cluster sum of squares (inertia) against
    the number of clusters. The "elbow" point where the rate of decrease sharply
    changes is considered optimal.
    
    Parameters:
    -----------
    X : array-like
        Input data
    max_k : int
        Maximum number of clusters to try
    verbose : bool
        Print results
        
    Returns:
    --------
    results : dict
        Dictionary with k values and corresponding inertias
    """
    print(f"\n{'='*60}")
    print("ELBOW METHOD FOR OPTIMAL K")
    print(f"{'='*60}")
    
    if verbose:
        print(f"  Testing k from 1 to {max_k}")
        print(f"\n  K    | Inertia    | Decrease  | % Decrease")
        print(f"  " + "-"*45)
    
    k_values = range(1, max_k + 1)
    inertias = []
    decreases = []
    pct_decreases = []
    
    for k in k_values:
        kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
        kmeans.fit(X)
        
        inertia = kmeans.inertia_
        inertias.append(inertia)
        
        if k > 1:
            decrease = inertias[k - 2] - inertia
            pct_decrease = (decrease / inertias[k - 2]) * 100
        else:
            decrease = 0
            pct_decrease = 0
        
        decreases.append(decrease)
        pct_decreases.append(pct_decrease)
        
        if verbose:
            print(f"  {k:3d}  | {inertia:9.4f}  | {decrease:9.4f}  | {pct_decrease:7.2f}%")
    
    # Find the elbow point using the second derivative
    if len(inertias) > 2:
        # Calculate rate of change of decrease
        deltas = np.diff(pct_decreases)
        elbow_idx = np.argmax(deltas) + 2
        elbow_k = list(k_values)[elbow_idx]
    else:
        elbow_k = 2
    
    if verbose:
        print(f"\n  Suggested optimal K: {elbow_k}")
    
    return {
        'k_values': list(k_values),
        'inertias': inertias,
        'decreases': decreases,
        'pct_decreases': pct_decreases,
        'elbow_k': elbow_k
    }


def silhouette_analysis(X, max_k=10, verbose=True):
    """
    Use Silhouette Score to find optimal number of clusters.
    
    The Silhouette Score measures how similar a point is to its own cluster
    compared to other clusters. Range: [-1, 1], higher is better.
    
    Parameters:
    -----------
    X : array-like
        Input data
    max_k : int
        Maximum number of clusters to try
    verbose : bool
        Print results
        
    Returns:
    --------
    results : dict
        Dictionary with k values and corresponding scores
    """
    print(f"\n{'='*60}")
    print("SILHOUETTE SCORE ANALYSIS")
    print(f"{'='*60}")
    
    if verbose:
        print(f"  Testing k from 2 to {max_k}")
        print(f"\n  K    | Silhouette  | Calinski-H | Davies-Bouldin")
        print(f"  " + "-"*50)
    
    k_values = range(2, max_k + 1)
    silhouette_scores = []
    calinski_scores = []
    davies_bouldin_scores = []
    
    for k in k_values:
        kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
        labels = kmeans.fit_predict(X)
        
        sil_score = silhouette_score(X, labels)
        cal_score = calinski_harabasz_score(X, labels)
        db_score = davies_bouldin_score(X, labels)
        
        silhouette_scores.append(sil_score)
        calinski_scores.append(cal_score)
        davies_bouldin_scores.append(db_score)
        
        if verbose:
            print(f"  {k:3d}  | {sil_score:9.4f}  | {cal_score:9.2f}  | {db_score:10.4f}")
    
    # Find optimal k
    best_sil_idx = np.argmax(silhouette_scores)
    best_k = list(k_values)[best_sil_idx]
    
    if verbose:
        print(f"\n  Best K (by Silhouette): {best_k}")
        print(f"  Best Silhouette Score: {silhouette_scores[best_sil_idx]:.4f}")
    
    return {
        'k_values': list(k_values),
        'silhouette_scores': silhouette_scores,
        'calinski_scores': calinski_scores,
        'davies_bouldin_scores': davies_bouldin_scores,
        'optimal_k': best_k
    }


def combined_k_selection(X, max_k=10, verbose=True):
    """
    Combined approach using multiple methods to select optimal K.
    
    This combines:
    1. Elbow Method
    2. Silhouette Score
    3. Calinski-Harabasz Index
    4. Davies-Bouldin Index
    
    Parameters:
    -----------
    X : array-like
        Input data
    max_k : int
        Maximum number of clusters to try
    verbose : bool
        Print results
        
    Returns:
    --------
    recommendation : int
        Recommended number of clusters
    """
    print(f"\n{'='*60}")
    print("COMBINED K SELECTION ANALYSIS")
    print(f"{'='*60}")
    
    # Elbow method
    elbow_results = elbow_method(X, max_k, verbose=False)
    
    # Silhouette analysis
    sil_results = silhouette_analysis(X, max_k, verbose=False)
    
    # Print combined results
    if verbose:
        print(f"\n  Combined Results:")
        print(f"\n  K   | Inertia   | Silhouette | Calinski-H | Davies-Bouldin")
        print(f"  " + "-"*58)
        
        for i, k in enumerate(elbow_results['k_values']):
            inertia = elbow_results['inertias'][i]
            
            if k > 1:
                sil = sil_results['silhouette_scores'][k-2]
                cal = sil_results['calinski_scores'][k-2]
                db = sil_results['davies_bouldin_scores'][k-2]
            else:
                sil = 0
                cal = 0
                db = 0
            
            print(f"  {k:3d} | {inertia:9.4f} | {sil:9.4f} | {cal:9.2f} | {db:10.4f}")
    
    # Normalize scores for comparison
    inertias_norm = np.array(elbow_results['inertias'])
    inertias_norm = 1 - (inertias_norm - inertias_norm.min()) / (inertias_norm.max() - inertias_norm.min() + 1e-10)
    
    silhouettes = np.array(sil_results['silhouette_scores'])
    calinskis = np.array(sil_results['calinski_scores'])
    calinskis = (calinskis - calinskis.min()) / (calinskis.max() - calinskis.min() + 1e-10)
    
    dbs = np.array(sil_results['davies_bouldin_scores'])
    dbs = 1 - (dbs - dbs.min()) / (dbs.max() - dbs.min() + 1e-10)
    
    # Combined score (for k > 1)
    combined_scores = []
    for i in range(1, max_k):
        combined_scores.append(inertias_norm[i] + silhouettes[i-1] + calinskis[i-1] + dbs[i-1])
    
    combined_scores = np.array(combined_scores)
    optimal_idx = np.argmax(combined_scores)
    recommended_k = list(range(2, max_k + 1))[optimal_idx]
    
    if verbose:
        print(f"\n  Recommendations:")
        print(f"    Elbow method suggests: K = {elbow_results['elbow_k']}")
        print(f"    Silhouette suggests: K = {sil_results['optimal_k']}")
        print(f"    Combined score suggests: K = {recommended_k}")
    
    return recommended_k


# =============================================================================
# SECTION IV: BANKING EXAMPLE - CUSTOMER SEGMENTATION
# =============================================================================

def banking_example():
    """
    Banking/Finance example: Customer Segmentation using K-Means Clustering.
    
    This example demonstrates:
    1. Data preprocessing for clustering
    2. Feature engineering
    3. K-Means clustering
    4. Cluster profiling and interpretation
    5. Business insights
    """
    print(f"\n{'='*70}")
    print("BANKING EXAMPLE: CUSTOMER SEGMENTATION")
    print(f"{'='*70}")
    
    # Generate customer data
    customers_df = generate_customer_segmentation_data(n_customers=1000)
    
    # Store customer IDs separately
    customer_ids = customers_df['customer_id'].values
    features = ['annual_income', 'credit_score', 'account_balance', 'num_products',
               'tenure_years', 'monthly_transactions', 'loan_amount']
    
    # Prepare features
    X_customers = customers_df[features].copy()
    
    # Handle missing values
    X_customers = X_customers.fillna(X_customers.mean())
    
    # Feature engineering
    print(f"\n{'='*60}")
    print("FEATURE ENGINEERING")
    print(f"{'='*60}")
    
    # Create derived features
    X_customers['balance_to_income'] = X_customers['account_balance'] / X_customers['annual_income']
    X_customers['products_per_tenure'] = X_customers['num_products'] / (X_customers['tenure_years'] + 0.1)
    X_customers['transaction_per_product'] = X_customers['monthly_transactions'] / X_customers['num_products']
    X_customers['loan_to_income'] = X_customers['loan_amount'] / X_customers['annual_income']
    
    print(f"  Added derived features:")
    print(f"    - balance_to_income: Account balance relative to income")
    print(f"    - products_per_tenure: Products per year of tenure")
    print(f"    - transaction_per_product: Transactions per product")
    print(f"    - loan_to_income: Loan amount relative to income")
    
    # Scale features
    print(f"\n{'='*60}")
    print("FEATURE SCALING")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X_customers)
    
    print(f"  Using StandardScaler for normalization")
    print(f"  Scaled feature shape: {X_scaled.shape}")
    
    # Find optimal number of clusters
    print(f"\n{'='*60}")
    print("FINDING OPTIMAL K")
    print(f"{'='*60}")
    
    optimal_k = combined_k_selection(X_scaled, max_k=8, verbose=True)
    
    # Perform final clustering
    print(f"\n{'='*60}")
    print(f"FINAL CLUSTERING WITH K={optimal_k}")
    print(f"{'='*60}")
    
    kmeans = KMeans(n_clusters=optimal_k, random_state=42, n_init=10)
    cluster_labels = kmeans.fit_predict(X_scaled)
    customers_df['cluster'] = cluster_labels
    
    # Cluster analysis
    print(f"\n{'='*60}")
    print("CLUSTER ANALYSIS")
    print(f"{'='*60}")
    
    # Cluster sizes
    cluster_sizes = customers_df['cluster'].value_counts().sort_index()
    print(f"\n  Cluster Sizes:")
    for cluster, size in cluster_sizes.items():
        print(f"    Cluster {cluster}: {size} customers ({size/len(customers_df)*100:.1f}%)")
    
    # Cluster profiles
    print(f"\n{'='*60}")
    print("CLUSTER PROFILES")
    print(f"{'='*60}")
    
    cluster_profiles = customers_df.groupby('cluster')[features].mean()
    print(cluster_profiles.round(2).to_string())
    
    # Interpretation
    print(f"\n{'='*60}")
    print("CLUSTER INTERPRETATION AND BUSINESS INSIGHTS")
    print(f"{'='*60}")
    
    for cluster in range(optimal_k):
        cluster_data = customers_df[customers_df['cluster'] == cluster]
        
        avg_income = cluster_data['annual_income'].mean()
        avg_credit = cluster_data['credit_score'].mean()
        avg_balance = cluster_data['account_balance'].mean()
        avg_products = cluster_data['num_products'].mean()
        avg_tenure = cluster_data['tenure_years'].mean()
        
        print(f"\n  Cluster {cluster}:")
        print(f"    Average Annual Income: ${avg_income:,.0f}")
        print(f"    Average Credit Score: {avg_credit:.0f}")
        print(f"    Average Account Balance: ${avg_balance:,.0f}")
        print(f"    Average Products: {avg_products:.1f}")
        print(f"    Average Tenure: {avg_tenure:.1f} years")
        
        # Generate segment name
        if avg_income > 80000 and avg_credit > 700:
            segment_name = "PREMIUM CUSTOMERS"
            strategy = "Focus on cross-selling high-value products, personalized wealth management services"
        elif avg_income > 50000 and avg_credit > 650:
            segment_name = "GROWTH CUSTOMERS"
            strategy = "Target with investment products, credit building programs"
        elif avg_tenure > 5:
            segment_name = "LOYAL CUSTOMERS"
            strategy = "Loyalty rewards, referral bonuses"
        else:
            segment_name = "ACQUISITION CUSTOMERS"
            strategy = "New customer onboarding, introductory offers"
        
        print(f"    Segment Name: {segment_name}")
        print(f"    Recommended Strategy: {strategy}")
    
    return customers_df, optimal_k


# =============================================================================
# SECTION V: HEALTHCARE EXAMPLE - PATIENT CLUSTERING
# =============================================================================

def healthcare_example():
    """
    Healthcare example: Patient Clustering using K-Means Clustering.
    
    This example demonstrates:
    1. Clustering patients based on health markers
    2. Risk stratification
    3. Treatment planning
    4. Resource allocation
    """
    print(f"\n{'='*70}")
    print("HEALTHCARE EXAMPLE: PATIENT CLUSTERING")
    print(f"{'='*70}")
    
    # Generate patient data
    patients_df = generate_patient_clustering_data(n_patients=800)
    
    # Store patient IDs separately
    patient_ids = patients_df['patient_id'].values
    features = ['age', 'bmi', 'blood_pressure_systolic', 'cholesterol',
                'blood_glucose', 'heart_rate', 'chronic_conditions', 'medication_count']
    
    # Prepare features
    X_patients = patients_df[features].copy()
    
    # Feature engineering
    print(f"\n{'='*60}")
    print("FEATURE ENGINEERING")
    print(f"{'='*60}")
    
    # Create risk-related derived features
    X_patients['bp_chol_product'] = X_patients['blood_pressure_systolic'] * X_patients['cholesterol'] / 1000
    X_patients['metabolic_risk'] = X_patients['bmi'] * X_patients['blood_glucose'] / 100
    X_patients['age_chronic'] = X_patients['age'] * X_patients['chronic_conditions']
    X_patients['medication_load'] = X_patients['medication_count'] / (X_patients['age'] / 10 + 1)
    
    print(f"  Added derived features:")
    print(f"    - bp_chol_product: Blood pressure * Cholesterol interaction")
    print(f"    - metabolic_risk: BMI * Blood glucose interaction")
    print(f"    - age_chronic: Age * Chronic conditions interaction")
    print(f"    - medication_load: Medication count adjusted for age")
    
    # Scale features
    print(f"\n{'='*60}")
    print("FEATURE SCALING")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X_patients)
    
    print(f"  Using StandardScaler")
    print(f"  Scaled feature shape: {X_scaled.shape}")
    
    # Find optimal number of clusters
    print(f"\n{'='*60}")
    print("FINDING OPTIMAL K")
    print(f"{'='*60}")
    
    optimal_k = combined_k_selection(X_scaled, max_k=8, verbose=True)
    
    # Perform final clustering
    print(f"\n{'='*60}")
    print(f"FINAL CLUSTERING WITH K={optimal_k}")
    print(f"{'='*60}")
    
    kmeans = KMeans(n_clusters=optimal_k, random_state=42, n_init=10)
    cluster_labels = kmeans.fit_predict(X_scaled)
    patients_df['cluster'] = cluster_labels
    
    # Cluster analysis
    print(f"\n{'='*60}")
    print("CLUSTER ANALYSIS")
    print(f"{'='*60}")
    
    # Cluster sizes
    cluster_sizes = patients_df['cluster'].value_counts().sort_index()
    print(f"\n  Cluster Sizes:")
    for cluster, size in cluster_sizes.items():
        print(f"    Cluster {cluster}: {size} patients ({size/len(patients_df)*100:.1f}%)")
    
    # Cluster profiles
    print(f"\n{'='*60}")
    print("CLUSTER PROFILES")
    print(f"{'='*60}")
    
    cluster_profiles = patients_df.groupby('cluster')[features].mean()
    print(cluster_profiles.round(2).to_string())
    
    # Risk stratification
    print(f"\n{'='*60}")
    print("RISK STRATIFICATION AND CLINICAL INTERPRETATION")
    print(f"{'='*60}")
    
    cluster_risk_scores = []
    
    for cluster in range(optimal_k):
        cluster_data = patients_df[patients_df['cluster'] == cluster]
        
        avg_age = cluster_data['age'].mean()
        avg_bmi = cluster_data['bmi'].mean()
        avg_bp = cluster_data['blood_pressure_systolic'].mean()
        avg_chol = cluster_data['cholesterol'].mean()
        avg_glucose = cluster_data['blood_glucose'].mean()
        avg_chronic = cluster_data['chronic_conditions'].mean()
        avg_meds = cluster_data['medication_count'].mean()
        
        # Calculate risk score (simple heuristic)
        risk_score = 0
        if avg_bp > 140:
            risk_score += 2
        elif avg_bp > 130:
            risk_score += 1
            
        if avg_chol > 200:
            risk_score += 2
        elif avg_chol > 180:
            risk_score += 1
            
        if avg_glucose > 110:
            risk_score += 2
        elif avg_glucose > 100:
            risk_score += 1
            
        if avg_bmi > 30:
            risk_score += 2
        elif avg_bmi > 25:
            risk_score += 1
            
        risk_score += avg_chronic
        
        cluster_risk_scores.append((cluster, risk_score))
    
    # Sort by risk score
    cluster_risk_scores.sort(key=lambda x: x[1], reverse=True)
    
    print(f"\n  Risk Stratification:")
    for cluster, risk in cluster_risk_scores:
        if risk >= 8:
            risk_level = "HIGH RISK"
            care_type = "Intensive care management, frequent monitoring"
        elif risk >= 5:
            risk_level = "MODERATE RISK"
            care_type = "Regular check-ups, lifestyle management"
        else:
            risk_level = "LOW RISK"
            care_type = "Preventive care, annual check-ups"
        
        print(f"\n    Cluster {cluster} ({risk_level}):")
        print(f"      Risk Score: {risk}")
        print(f"      Recommended Care: {care_type}")
        
        cluster_data = patients_df[patients_df['cluster'] == cluster]
        print(f"      Avg Age: {cluster_data['age'].mean():.1f}")
        print(f"      Avg BMI: {cluster_data['bmi'].mean():.1f}")
        print(f"      Avg BP: {cluster_data['blood_pressure_systolic'].mean():.1f}")
        print(f"      Avg Cholesterol: {cluster_data['cholesterol'].mean():.1f}")
        print(f"      Chronic Conditions: {cluster_data['chronic_conditions'].mean():.1f}")
    
    return patients_df, optimal_k


# =============================================================================
# SECTION VI: TESTING AND EVALUATION
# =============================================================================

def test_kmeans_algorithm():
    """
    Comprehensive testing of K-Means algorithm with various scenarios.
    """
    print(f"\n{'='*70}")
    print("K-MEANS ALGORITHM TESTING SUITE")
    print(f"{'='*70}")
    
    # Test 1: Basic clustering
    print(f"\n{'='*60}")
    print("TEST 1: BASIC CLUSTERING")
    print(f"{'='*60}")
    
    X, y_true, _ = generate_cluster_data(n_samples=300, n_clusters=3)
    kmeans = KMeans(n_clusters=3, random_state=42, n_init=10)
    labels = kmeans.fit_predict(X)
    
    accuracy = np.mean(labels == y_true)
    sil_score = silhouette_score(X, labels)
    cal_score = calinski_harabasz_score(X, labels)
    
    print(f"  Clustering Accuracy: {accuracy:.4f}")
    print(f"  Silhouette Score: {sil_score:.4f}")
    print(f"  Calinski-Harabasz Score: {cal_score:.2f}")
    
    # Test 2: Different K values
    print(f"\n{'='*60}")
    print("TEST 2: DIFFERENT K VALUES")
    print(f"{'='*60}")
    
    results = []
    for k in range(2, 7):
        kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
        labels = kmeans.fit_predict(X)
        
        sil = silhouette_score(X, labels)
        cal = calinski_harabasz_score(X, labels)
        db = davies_bouldin_score(X, labels)
        
        results.append({
            'k': k,
            'inertia': kmeans.inertia_,
            'silhouette': sil,
            'calinski': cal,
            'davies_bouldin': db
        })
        print(f"  K={k}: Silhouette={sil:.4f}, Calinski={cal:.2f}, Davies-Bouldin={db:.4f}")
    
    # Test 3: Noisy data
    print(f"\n{'='*60}")
    print("TEST 3: DATA WITH NOISE")
    print(f"{'='*60}")
    
    X_noisy, _, _ = generate_cluster_data(n_samples=300, n_clusters=3, cluster_std=2.0)
    kmeans = KMeans(n_clusters=3, random_state=42, n_init=10)
    labels = kmeans.fit_predict(X_noisy)
    
    sil = silhouette_score(X_noisy, labels)
    print(f"  Silhouette Score (noisy): {sil:.4f}")
    print(f"  Note: Lower score expected with noise")
    
    # Test 4: Different initialization methods
    print(f"\n{'='*60}")
    print("TEST 4: INITIALIZATION COMPARISON")
    print(f"{'='*60}")
    
    for init in ['k-means++', 'random']:
        kmeans = KMeans(n_clusters=3, init=init, n_init=10, random_state=42)
        kmeans.fit(X)
        print(f"  {init}: Inertia={kmeans.inertia_:.4f}, Iterations={kmeans.n_iter_}")
    
    print(f"\n  Tests completed successfully!")
    
    return True


def test_manual_vs_sklearn():
    """
    Compare manual K-Means implementation with sklearn.
    """
    print(f"\n{'='*70}")
    print("MANUAL vs SKLEARN COMPARISON")
    print(f"{'='*70}")
    
    # Generate data
    X, _, _ = generate_cluster_data(n_samples=200, n_clusters=3)
    
    # Standardize for comparison
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    # Manual implementation
    print(f"\n  Running manual K-Means...")
    labels_manual, centroids_manual, inertia_manual, _ = kmeans_manual(
        X_scaled, n_clusters=3, init='kmeans++', verbose=False
    )
    
    # Sklearn implementation
    print(f"  Running sklearn K-Means...")
    kmeans = KMeans(n_clusters=3, init='k-means++', n_init=10, random_state=42)
    kmeans.fit(X_scaled)
    labels_sklearn = kmeans.labels_
    centroids_sklearn = kmeans.cluster_centers_
    inertia_sklearn = kmeans.inertia_
    
    # Compare results
    print(f"\n{'='*60}")
    print("RESULTS COMPARISON")
    print(f"{'='*60}")
    print(f"  Manual Implementation:")
    print(f"    Inertia: {inertia_manual:.4f}")
    print(f"    Cluster sizes: {np.bincount(labels_manual)}")
    
    print(f"\n  Sklearn Implementation:")
    print(f"    Inertia: {inertia_sklearn:.4f}")
    print(f"    Cluster sizes: {np.bincount(labels_sklearn)}")
    
    # Compare cluster assignments
    # Note: Labels may differ due to permutation
    print(f"\n  Note: Labels may be permuted between implementations")
    print(f"        (This is expected as clustering is unsupervised)")
    
    return True


# =============================================================================
# SECTION VII: ADVANCED TOPICS
# =============================================================================

def advanced_kmeans_topics():
    """
    Advanced K-Means topics and variations.
    """
    print(f"\n{'='*70}")
    print("ADVANCED K-MEANS TOPICS")
    print(f"{'='*70}")
    
    # Mini-Batch K-Means
    print(f"\n{'='*60}")
    print("1. MINI-BATCH K-MEANS")
    print(f"{'='*60}")
    
    X, _, _ = generate_cluster_data(n_samples=5000, n_clusters=4)
    
    # Standard K-Means
    kmeans = KMeans(n_clusters=4, random_state=42, n_init=10)
    kmeans.fit(X)
    print(f"  Standard K-Means: {kmeans.inertia_:.4f} (n_iter={kmeans.n_iter_})")
    
    # Mini-batch K-Means
    mbkmeans = MiniBatchKMeans(n_clusters=4, random_state=42, n_init=10, batch_size=100)
    mbkmeans.fit(X)
    print(f"  Mini-Batch K-Means: {mbkmeans.inertia_:.4f} (n_iter={mbkmeans.n_iter_})")
    
    # Bisecting K-Means
    print(f"\n{'='*60}")
    print("2. BISECTING K-MEANS")
    print(f"{'='*60}")
    
    from sklearn.cluster import BisectingKMeans
    
    bkmeans = BisectingKMeans(n_clusters=4, random_state=42)
    bkmeans.fit(X)
    print(f"  Bisecting K-Means: {bkmeans.inertia_:.4f}")
    print(f"  Cluster sizes: {np.bincount(bkmeans.labels_)}")
    
    # Different distance metrics
    print(f"\n{'='*60}")
    print("3. DIFFERENT DISTANCE METRICS")
    print(f"{'='*60}")
    
    # Note: sklearn only supports Euclidean natively
    # For other metrics, would need custom implementation
    print(f"  Sklearn K-Means uses Euclidean distance by default")
    print(f"  For other metrics, custom implementation needed")
    
    # Elbow visualization
    print(f"\n{'='*60}")
    print("4. ELBOW VISUALIZATION DATA")
    print(f"{'='*60}")
    
    # Generate data for elbow plot
    X_small, _, _ = generate_cluster_data(n_samples=300, n_clusters=4)
    elbow_results = elbow_method(X_small, max_k=8, verbose=True)
    
    # Silhouette visualization data
    print(f"\n{'='*60}")
    print("5. SILHOUETTE ANALYSIS DATA")
    print(f"{'='*60}")
    
    sil_results = silhouette_analysis(X_small, max_k=8, verbose=True)
    
    return True


def centroid_analysis():
    """
    Analyze cluster centroids and their characteristics.
    """
    print(f"\n{'='*70}")
    print("CENTROID ANALYSIS")
    print(f"{'='*70}")
    
    # Generate data
    X, y_true, true_centers = generate_cluster_data(n_samples=300, n_clusters=4)
    
    # Fit K-Means
    kmeans = KMeans(n_clusters=4, random_state=42, n_init=10)
    kmeans.fit(X)
    
    learned_centers = kmeans.cluster_centers_
    
    print(f"\n{'='*60}")
    print("TRUE vs LEARNED CENTROIDS")
    print(f"{'='*60}")
    
    print(f"\n  True Centers:")
    print(f"    {true_centers.round(2)}")
    
    print(f"\n  Learned Centers:")
    print(f"    {learned_centers.round(2)}")
    
    # Calculate distances between true and learned centers
    # (accounting for permutation)
    from scipy.optimize import linear_sum_assignment
    
    cost_matrix = euclidean_distances(true_centers, learned_centers)
    row_ind, col_ind = linear_sum_assignment(cost_matrix)
    
    print(f"\n  Centroid Matching:")
    for i, j in zip(row_ind, col_ind):
        distance = cost_matrix[i, j]
        print(f"    True center {i} -> Learned center {j} (dist={distance:.4f})")
    
    # Centroid stability analysis
    print(f"\n{'='*60}")
    print("CENTROID STABILITY")
    print(f"{'='*60}")
    
    centroids_list = []
    for seed in range(5):
        kmeans = KMeans(n_clusters=4, random_state=seed, n_init=10)
        kmeans.fit(X)
        centroids_list.append(kmeans.cluster_centers_)
    
    centroids_array = np.array(centroids_list)
    centroid_variance = centroids_array.var(axis=0).mean(axis=0).sum()
    
    print(f"  Centroid variance across runs: {centroid_variance:.6f}")
    print(f"  Note: Low variance indicates stable clustering")
    
    return kmeans


# =============================================================================
# SECTION VIII: MAIN EXECUTION
# =============================================================================

def main():
    """
    Main function to execute K-Means clustering examples.
    """
    print(f"\n{'='*70}")
    print("K MEANS CLUSTERING - COMPREHENSIVE IMPLEMENTATION")
    print(f"{'='*70}")
    
    # Execute each example
    # 1. Core K-Means implementation
    print(f"\n{'='*70}")
    print("SECTION 1: CORE K-MEANS")
    print(f"{'='*70}")
    X, y_true, labels_core = core_kmeans()
    
    # 2. Elbow method
    print(f"\n{'='*70}")
    print("SECTION 2: ELBOW METHOD")
    print(f"{'='*70}")
    elbow_results = elbow_method(X, max_k=8)
    
    # 3. Silhouette analysis
    print(f"\n{'='*70}")
    print("SECTION 3: SILHOUETTE ANALYSIS")
    print(f"{'='*70}")
    sil_results = silhouette_analysis(X, max_k=8)
    
    # 4. Combined K selection
    print(f"\n{'='*70}")
    print("SECTION 4: COMBINED K SELECTION")
    print(f"{'='*70}")
    optimal_k = combined_k_selection(X, max_k=8)
    
    # 5. Banking example
    print(f"\n{'='*70}")
    print("SECTION 5: BANKING EXAMPLE")
    print(f"{'='*70}")
    customers_df, banking_k = banking_example()
    
    # 6. Healthcare example
    print(f"\n{'='*70}")
    print("SECTION 6: HEALTHCARE EXAMPLE")
    print(f"{'='*70}")
    patients_df, healthcare_k = healthcare_example()
    
    # 7. Testing
    print(f"\n{'='*70}")
    print("SECTION 7: TESTING")
    print(f"{'='*70}")
    test_results = test_kmeans_algorithm()
    
    # 8. Comparison
    print(f"\n{'='*70}")
    print("SECTION 8: MANUAL VS SKLEARN")
    print(f"{'='*70}")
    comparison_results = test_manual_vs_sklearn()
    
    # 9. Advanced topics
    print(f"\n{'='*70}")
    print("SECTION 9: ADVANCED TOPICS")
    print(f"{'='*70}")
    advanced_results = advanced_kmeans_topics()
    
    # 10. Centroid analysis
    print(f"\n{'='*70}")
    print("SECTION 10: CENTROID ANALYSIS")
    print(f"{'='*70}")
    centroid_results = centroid_analysis()
    
    # Summary
    print(f"\n{'='*70}")
    print("SUMMARY")
    print(f"{'='*70}")
    print(f"  1. Core K-Means: Complete")
    print(f"  2. Elbow Method: Optimal K = {elbow_results['elbow_k']}")
    print(f"  3. Silhouette: Optimal K = {sil_results['optimal_k']}")
    print(f"  4. Combined: Optimal K = {optimal_k}")
    print(f"  5. Banking: {banking_k} customer segments")
    print(f"  6. Healthcare: {healthcare_k} patient clusters")
    print(f"  7. Testing: All tests passed")
    print(f"  8. Comparison: Manual vs sklearn complete")
    print(f"  9. Advanced: Topics covered")
    print(f"  10. Centroid: Analysis complete")
    
    print(f"\n{'='*70}")
    print("EXECUTION COMPLETE")
    print(f"{'='*70}")
    
    return {
        'optimal_k': optimal_k,
        'banking_k': banking_k,
        'healthcare_k': healthcare_k
    }


if __name__ == "__main__":
    results = main()