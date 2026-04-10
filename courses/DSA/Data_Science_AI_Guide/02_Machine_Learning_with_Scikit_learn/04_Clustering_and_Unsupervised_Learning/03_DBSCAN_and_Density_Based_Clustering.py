# Topic: DBSCAN and Density Based Clustering
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for DBSCAN and Density Based Clustering

I. INTRODUCTION
   DBSCAN (Density-Based Spatial Clustering of Applications with Noise) is a 
   fundamental density-based clustering algorithm that groups points close to 
   each other while marking points in low-density regions as outliers. Unlike 
   centroid-based methods like K-Means, DBSCAN can discover clusters of arbitrary 
   shape and automatically detect the number of clusters.

II. CORE CONCEPTS
   - Core Points: Points with at least min_samples neighbors within distance eps
   - Border Points: Points within eps of core points but not core themselves
   - Noise Points: Points not density-reachable from any core point
   - Epsilon (eps): The neighborhood radius for density calculation
   - min_samples: Minimum neighbors required to form a core point

III. IMPLEMENTATION
   - Basic DBSCAN clustering with scikit-learn
   - Parameter tuning strategies (eps and min_samples)
   - Visualization of clustering results
   - Comparison with K-Means

IV. BANKING APPLICATION
   - Fraud detection in transaction data
   - Customer behavior anomaly detection
   - Credit risk outlier identification
   - Transaction pattern analysis

V. HEALTHCARE APPLICATION
   - Patient vital sign anomaly detection
   - Medical imaging outlier identification
   - Disease outbreak spatial clustering
   - Clinical pattern discovery

VI. TESTING & VALIDATION
   - Silhouette score evaluation
   - Cluster quality metrics
   - Parameter sensitivity analysis
   - Cross-validation approaches

VII. ADVANCED TOPICS
   - HDBSCAN for improved clustering
   - OPTICS for variable density clusters
   - Custom distance metrics
   - Large-scale implementation strategies

VIII. CONCLUSION
   - Algorithm strengths and limitations
   - Best practices and recommendations
   - Real-world application considerations
"""

import numpy as np
import pandas as pd
from sklearn.datasets import make_blobs, make_moons, make_circles
from sklearn.preprocessing import StandardScaler
from sklearn.cluster import DBSCAN
from sklearn.metrics import silhouette_score, calinski_harabasz_score, davies_bouldin_score
from sklearn.neighbors import NearestNeighbors
import warnings
warnings.filterwarnings('ignore')


def generate_nonconvex_data(n_samples=500, noise=0.1, random_state=42):
    """
    Generate non-convex cluster data shapes that DBSCAN can handle but K-Means cannot.
    
    This function creates datasets with complex shapes:
    - Two interleaving moons (half-circles)
    - Concentric circles
    - Spiral patterns
    
    Parameters:
    -----------
    n_samples : int
        Number of points to generate
    noise : float
        Standard deviation of Gaussian noise added
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : ndarray
        Feature array of shape (n_samples, 2)
    y : ndarray
        True labels (not used by DBSCAN - for evaluation only)
    """
    np.random.seed(random_state)
    
    X_moons, y_moons = make_moons(n_samples=n_samples//2, noise=noise, random_state=random_state)
    
    outer_radius = 3.0
    inner_radius = 1.0
    n_circles = n_samples - n_samples//2
    
    angles = np.random.uniform(0, 2*np.pi, n_circles)
    radii = np.concatenate([
        np.random.uniform(inner_radius, inner_radius + 0.5, n_circles//2),
        np.random.uniform(outer_radius - 0.5, outer_radius, n_circles - n_circles//2)
    ])
    
    circles_x = radii * np.cos(angles) + np.random.normal(0, noise, n_circles)
    circles_y = radii * np.sin(angles) + np.random.normal(0, noise, n_circles)
    X_circles = np.column_stack([circles_x, circles_y])
    y_circles = np.ones(n_circles, dtype=int)
    
    X = np.vstack([X_moons, X_circles])
    y = np.concatenate([y_moons, y_circles])
    
    indices = np.random.permutation(len(X))
    X = X[indices]
    y = y[indices]
    
    return X, y


def generate_dense_clusters(n_samples=300, n_features=2, n_clusters=4, cluster_std=0.5, random_state=42):
    """
    Generate spherical clusters using make_blobs.
    
    This is the traditional clustering dataset where K-Means performs well.
    Comparing DBSCAN with K-Means on this data shows where each algorithm shines.
    
    Parameters:
    -----------
    n_samples : int
        Total number of points
    n_features : int
        Number of features (dimensions)
    n_clusters : int
        Number of cluster centers
    cluster_std : float
        Standard deviation of clusters
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : ndarray
        Feature array
    y : ndarray
        True cluster labels
    """
    X, y = make_blobs(
        n_samples=n_samples,
        n_features=n_features,
        centers=n_clusters,
        cluster_std=cluster_std,
        random_state=random_state
    )
    return X, y


def core_dbscan_implementation(X, eps=0.5, min_samples=5):
    """
    Core DBSCAN implementation demonstrating the algorithm.
    
    This function shows how DBSCAN works internally:
    1. Mark all points as unvisited
    2. For each unvisited point:
       - Find neighbors within eps distance
       - If min_samples or more: start new cluster
       - Else: mark as noise/border
    3. Expand cluster by finding密度-reachable points
    
    Parameters:
    -----------
    X : ndarray
        Feature data of shape (n_samples, n_features)
    eps : float
        Epsilon (neighborhood radius)
    min_samples : int
        Minimum samples in neighborhood to form core point
        
    Returns:
    --------
    labels : ndarray
        Cluster labels (-1 for noise)
    core_indices : ndarray
        Indices of core points
    """
    from sklearn.neighbors import NearestNeighbors
    
    n_samples = X.shape[0]
    
    nn = NearestNeighbors(radius=eps, n_jobs=-1)
    nn.fit(X)
    
    neighbors_graph = nn.radius_neighbors_graph(X, mode='connectivity', include_self=False)
    
    labels = np.full(n_samples, -2, dtype=int)
    labels.fill(-1)
    
    core_mask = np.zeros(n_samples, dtype=bool)
    cluster_id = 0
    
    visited = np.zeros(n_samples, dtype=bool)
    
    for i in range(n_samples):
        if visited[i]:
            continue
            
        visited[i] = True
        
        neighbor_indices = neighbors_graph[i].indices
        
        if len(neighbor_indices) >= min_samples:
            if labels[i] == -1:
                labels[i] = cluster_id
                core_mask[i] = True
                
                seeds = set(neighbor_indices)
                seeds.discard(i)
                
                while seeds:
                    seed = seeds.pop()
                    if visited[seed]:
                        continue
                        
                    visited[seed] = True
                    
                    seed_neighbors = neighbors_graph[seed].indices
                    
                    if len(seed_neighbors) >= min_samples:
                        core_mask[seed] = True
                        labels[seed] = cluster_id
                        for neighbor in seed_neighbors:
                            if not visited[neighbor]:
                                seeds.add(neighbor)
                    else:
                        labels[seed] = cluster_id
                        
                cluster_id += 1
    
    return labels, np.where(core_mask)[0]


def show_point_types(X, eps=0.5, min_samples=5):
    """
    Demonstrate core, border, and noise point classification.
    
    This function classifies each point and shows the distribution:
    - Core points: Have min_samples+ neighbors within eps
    - Border points: Within eps of core but not core themselves
    - Noise points: Not density-reachable from any core
    
    Parameters:
    -----------
    X : ndarray
        Feature data
    eps : float
        Epsilon neighborhood radius
    min_samples : int
        Minimum neighbors for core point
        
    Returns:
    --------
    point_types : dict
        Dictionary with 'core', 'border', 'noise' arrays
    """
    from sklearn.neighbors import NearestNeighbors
    
    nn = NearestNeighbors(radius=eps)
    nn.fit(X)
    
    neighbors = nn.radius_neighbors(X)
    neighbor_counts = np.array([len(n) for n in neighbors[1]])
    
    core_mask = neighbor_counts >= min_samples
    
    neighbor_graph = nn.radius_neighbors_graph(X, mode='connectivity', include_self=False)
    
    border_mask = np.zeros(len(X), dtype=bool)
    for i in np.where(core_mask)[0]:
        border_mask[neighbor_graph[i].indices] = True
    border_mask &= ~core_mask
    
    noise_mask = ~border_mask & ~core_mask
    
    point_types = {
        'core': np.where(core_mask)[0],
        'border': np.where(border_mask)[0],
        'noise': np.where(noise_mask)[0]
    }
    
    return point_types


def parameter_tuning_with_kdistance(X, k=5):
    """
    Tune epsilon using the k-distance graph method.
    
    The k-distance graph plots the distance to the k-th nearest neighbor
    for each point. The "elbow" or maximum curvature point suggests a good
    epsilon value.
    
    Parameters:
    -----------
    X : ndarray
        Scaled feature data
    k : int
        Number of neighbors (usually min_samples)
        
    Returns:
    --------
    k_distances : ndarray
        Sorted k-distances
    suggested_eps : float
        Suggested epsilon from elbow detection
    """
    nn = NearestNeighbors(n_neighbors=k)
    nn.fit(X)
    distances, _ = nn.kneighbors(X)
    
    k_distances = distances[:, k-1]
    k_distances_sorted = np.sort(k_distances)
    
    percentiles = [75, 90, 95, 99]
    suggested_eps = {}
    for p in percentiles:
        suggested_eps[p] = np.percentile(k_distances_sorted, p)
    
    return k_distances_sorted, suggested_eps


def grid_search_dbscan(X, eps_values, min_samples_values, y_true=None):
    """
    Perform grid search to find optimal DBSCAN parameters.
    
    This function tests all combinations of eps and min_samples values
    and returns the best performing parameters based on clustering quality.
    
    Parameters:
    -----------
    X : ndarray
        Feature data
    eps_values : list
        List of epsilon values to test
    min_samples_values : list
        List of min_samples values to test
    y_true : ndarray, optional
        True labels for adjusted Rand Index
        
    Returns:
    --------
    results : DataFrame
        Results for all parameter combinations
    """
    from sklearn.metrics import adjusted_rand_score
    
    results = []
    
    for eps in eps_values:
        for min_samples in min_samples_values:
            dbscan = DBSCAN(eps=eps, min_samples=min_samples)
            labels = dbscan.fit_predict(X)
            
            n_clusters = len(set(labels)) - (1 if -1 in labels else 0)
            n_noise = list(labels).count(-1)
            
            if n_clusters >= 2 and n_clusters < len(X) - 1:
                try:
                    silhouette = silhouette_score(X, labels)
                    calinski = calinski_harabasz_score(X, labels)
                    davies = davies_bouldin_score(X, labels)
                except:
                    silhouette = -1
                    calinski = 0
                    davies = float('inf')
            else:
                silhouette = -1
                calinski = 0
                davies = float('inf')
            
            if y_true is not None and n_clusters > 1:
                ari = adjusted_rand_score(y_true, labels)
            else:
                ari = -1
            
            results.append({
                'eps': eps,
                'min_samples': min_samples,
                'n_clusters': n_clusters,
                'n_noise': n_noise,
                'silhouette': silhouette,
                'calinski_harabasz': calinski,
                'davies_bouldin': davies,
                'ari': ari
            })
    
    return pd.DataFrame(results)


def compare_with_kmeans(X, y_true, n_clusters):
    """
    Compare DBSCAN with K-Means on the same dataset.
    
    This demonstrates where each algorithm excels:
    - DBSCAN: Arbitrary shapes, outlier detection
    - K-Means: Spherical clusters, known number
    
    Parameters:
    -----------
    X : ndarray
        Feature data
    y_true : ndarray
        True cluster labels
    n_clusters : int
        Number of clusters for K-Means
        
    Returns:
    --------
    comparison : dict
        Dictionary with results from both algorithms
    """
    from sklearn.cluster import KMeans
    
    dbscan = DBSCAN(eps=0.5, min_samples=5)
    dbscan_labels = dbscan.fit_predict(X)
    
    kmeans = KMeans(n_clusters=n_clusters, random_state=42, n_init=10)
    kmeans_labels = kmeans.fit_predict(X)
    
    comparison = {
        'dbscan': {
            'labels': dbscan_labels,
            'n_clusters': len(set(dbscan_labels)) - (1 if -1 in dbscan_labels else 0),
            'n_noise': list(dbscan_labels).count(-1),
            'silhouette': silhouette_score(X, dbscan_labels),
            'ari': adjusted_rand_score(y_true, dbscan_labels)
        },
        'kmeans': {
            'labels': kmeans_labels,
            'n_clusters': n_clusters,
            'n_noise': 0,
            'silhouette': silhouette_score(X, kmeans_labels),
            'ari': adjusted_rand_score(y_true, kmeans_labels)
        }
    }
    
    return comparison


def banking_fraud_detection_example(n_transactions=5000, random_state=42):
    """
    Banking example: Fraud detection using DBSCAN anomaly detection.
    
    This example demonstrates how DBSCAN can identify fraudulent transactions:
    1. Normal transactions form dense clusters
    2. Fraudulent transactions are often isolated or in small groups
    3. DBSCAN marks outliers as noise (-1 label)
    
    Features:
    - transaction_amount: Value of transaction
    - hour_of_day: When transaction occurred
    - distance_from_home: Distance from registered address
    - distance_from_last: Distance from last transaction
    - velocity: Transaction frequency
    - is_foreign: Whether transaction is international
    - is_online: Whether transaction is online
    
    Parameters:
    -----------
    n_transactions : int
        Number of transactions to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : DataFrame
        Transaction data with features
    labels : ndarray
        DBSCAN cluster labels
    scaler : StandardScaler
        Fitted scaler for inverse transformation
    """
    np.random.seed(random_state)
    
    amount = np.random.exponential(100, n_transactions)
    amount = np.clip(amount, 1, 10000)
    
    hour = np.random.choice(24, n_transactions, p=np.concatenate([
        np.ones(6) * 0.02,
        np.ones(12) * 0.08,
        np.ones(6) * 0.02
    ]))
    
    distance_home = np.random.exponential(5, n_transactions)
    distance_home = np.clip(distance_home, 0, 500)
    
    distance_last = np.random.exponential(2, n_transactions)
    distance_last = np.clip(distance_last, 0, 200)
    
    velocity = np.random.poisson(2, n_transactions)
    
    is_foreign = np.random.choice([0, 1], n_transactions, p=[0.92, 0.08])
    
    is_online = np.random.choice([0, 1], n_transactions, p=[0.65, 0.35])
    
    fraud_prob = (
        0.001 +
        0.02 * (amount > 500) +
        0.03 * (amount > 1000) +
        0.01 * ((hour >= 0) & (hour < 6)) +
        0.015 * (distance_home > 50) +
        0.02 * (distance_last > 30) +
        0.01 * (velocity > 5) +
        0.02 * (is_foreign == 1) +
        0.03 * (is_online == 1) * (amount > 300)
    )
    fraud_prob = np.clip(fraud_prob, 0.001, 0.15)
    
    is_fraud = (np.random.random(n_transactions) < fraud_prob).astype(int)
    
    df = pd.DataFrame({
        'transaction_id': range(n_transactions),
        'amount': amount,
        'hour_of_day': hour,
        'distance_from_home': distance_home,
        'distance_from_last': distance_last,
        'velocity': velocity,
        'is_foreign': is_foreign,
        'is_online': is_online,
        'is_fraud': is_fraud
    })
    
    features = ['amount', 'hour_of_day', 'distance_from_home', 
                 'distance_from_last', 'velocity', 'is_foreign', 'is_online']
    X = df[features].values
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    k_dist, suggested_eps = parameter_tuning_with_kdistance(X_scaled, k=5)
    eps_value = suggested_eps[90]
    
    dbscan = DBSCAN(eps=eps_value, min_samples=10)
    labels = dbscan.fit_predict(X_scaled)
    
    return df, labels, scaler


def analyze_fraud_clusters(df, labels):
    """
    Analyze DBSCAN clusters for fraud patterns.
    
    This analyzes which clusters have elevated fraud rates,
    helping identify high-risk transaction patterns.
    
    Parameters:
    -----------
    df : DataFrame
        Transaction data
    labels : ndarray
        Cluster labels from DBSCAN
        
    Returns:
    --------
    cluster_analysis : DataFrame
        Analysis of each cluster
    """
    df = df.copy()
    df['cluster'] = labels
    
    cluster_analysis = []
    
    unique_labels = sorted(set(labels))
    
    for label in unique_labels:
        cluster_data = df[df['cluster'] == label]
        
        if len(cluster_data) == 0:
            continue
            
        cluster_info = {
            'cluster': label,
            'size': len(cluster_data),
            'fraud_count': cluster_data['is_fraud'].sum(),
            'fraud_rate': cluster_data['is_fraud'].mean(),
            'avg_amount': cluster_data['amount'].mean(),
            'avg_velocity': cluster_data['velocity'].mean(),
            'foreign_pct': cluster_data['is_foreign'].mean(),
            'online_pct': cluster_data['is_online'].mean()
        }
        
        if label == -1:
            cluster_info['type'] = 'noise'
        else:
            cluster_info['type'] = 'cluster'
            
        cluster_analysis.append(cluster_info)
    
    result = pd.DataFrame(cluster_analysis)
    
    if len(result) > 0:
        result = result.sort_values('fraud_rate', ascending=False)
    
    return result


def banking_customer_segmentation_example(n_customers=2000, random_state=42):
    """
    Banking example: Customer segmentation with unusual behavior detection.
    
    This demonstrates DBSCAN for customer behavior analysis:
    - Normal customers form dense behavioral clusters
    - Unusual customers are detected as noise
    
    Features:
    - balance: Average account balance
    - transaction_frequency: Monthly transactions
    - avg_transaction_amount: Average transaction value
    - products_owned: Number of banking products
    - tenure: Customer tenure in months
    - age: Customer age
    """
    np.random.seed(random_state)
    
    balance = np.random.lognormal(8, 1.5, n_customers)
    balance = np.clip(balance, 100, 100000)
    
    transaction_frequency = np.random.poisson(5, n_customers)
    transaction_frequency = np.clip(transaction_frequency, 0, 50)
    
    avg_amount = np.random.lognormal(4, 0.8, n_customers)
    avg_amount = np.clip(avg_amount, 10, 5000)
    
    products_owned = np.random.choice(1, n_customers, p=[0.08, 0.25, 0.35, 0.20, 0.12])
    products_owned = np.random.choice(range(1, 6), n_customers, p=[0.08, 0.25, 0.35, 0.20, 0.12])
    
    tenure = np.random.exponential(24, n_customers)
    tenure = np.clip(tenure, 1, 120)
    
    age = np.random.normal(45, 15, n_customers)
    age = np.clip(age, 18, 90)
    
    df = pd.DataFrame({
        'customer_id': range(n_customers),
        'balance': balance,
        'transaction_frequency': transaction_frequency,
        'avg_transaction_amount': avg_amount,
        'products_owned': products_owned,
        'tenure_months': tenure,
        'age': age
    })
    
    features = ['balance', 'transaction_frequency', 'avg_transaction_amount',
                'products_owned', 'tenure_months', 'age']
    X = df[features].values
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    k_dist, suggested_eps = parameter_tuning_with_kdistance(X_scaled, k=5)
    eps_value = suggested_eps[85]
    
    dbscan = DBSCAN(eps=eps_value, min_samples=15)
    labels = dbscan.fit_predict(X_scaled)
    
    return df, labels, scaler


def healthcare_patient_anomaly_example(n_patients=2000, random_state=42):
    """
    Healthcare example: Patient vital sign anomaly detection.
    
    This demonstrates DBSCAN for medical anomaly detection:
    - Normal patients form dense clusters in vital sign space
    - Patients with unusual vital signs are detected as noise
    
    Features:
    - heart_rate: Beats per minute
    - systolic_bp: Systolic blood pressure
    - diastolic_bp: Diastolic blood pressure
    - temperature: Body temperature (F)
    - respiratory_rate: Breaths per minute
    - oxygen_saturation: SpO2 percentage
    - glucose: Blood glucose mg/dL
    - hemoglobin: Hemoglobin g/dL
    """
    np.random.seed(random_state)
    
    n_normal = int(n_patients * 0.90)
    n_anomaly = n_patients - n_normal
    
    heart_rate_normal = np.random.normal(72, 8, n_normal)
    heart_rate_anomaly = np.random.choice(
        [np.random.normal(40, 5, n_anomaly//4), 
         np.random.normal(120, 15, n_anomaly//4),
         np.random.normal(72, 8, n_anomaly//2)],
        p=[0.25, 0.25, 0.5]
    ).astype(float)
    np.random.shuffle(heart_rate_anomaly)
    heart_rate = np.concatenate([heart_rate_normal, heart_rate_anomaly])
    heart_rate = np.clip(heart_rate, 30, 150)
    
    systolic_normal = np.random.normal(120, 12, n_normal)
    systolic_anomaly = np.concatenate([
        np.random.normal(170, 15, n_anomaly//3),
        np.random.normal(85, 10, n_anomaly//3),
        np.random.normal(120, 12, n_anomaly//3)
    ])
    systolic = np.concatenate([systolic_normal, systolic_anomaly])
    systolic = np.clip(systolic, 70, 220)
    
    diastolic_normal = np.random.normal(80, 8, n_normal)
    diastolic_anomaly = np.concatenate([
        np.random.normal(110, 10, n_anomaly//3),
        np.random.normal(50, 8, n_anomaly//3),
        np.random.normal(80, 8, n_anomaly//3)
    ])
    diastolic = np.concatenate([diastolic_normal, diastolic_anomaly])
    diastolic = np.clip(diastolic, 40, 140)
    
    temp_normal = np.random.normal(98.6, 0.5, n_normal)
    temp_anomaly = np.concatenate([
        np.random.normal(104, 0.5, n_anomaly//3),
        np.random.normal(95, 0.8, n_anomaly//3),
        np.random.normal(98.6, 0.5, n_anomaly//3)
    ])
    temperature = np.concatenate([temp_normal, temp_anomaly])
    temperature = np.clip(temperature, 94, 106)
    
    resp_normal = np.random.normal(16, 2, n_normal)
    resp_anomaly = np.concatenate([
        np.random.normal(28, 3, n_anomaly//3),
        np.random.normal(8, 2, n_anomaly//3),
        np.random.normal(16, 2, n_anomaly//3)
    ])
    respiratory = np.concatenate([resp_normal, resp_anomaly])
    respiratory = np.clip(respiratory, 6, 40)
    
    o2_normal = np.random.normal(98, 2, n_normal)
    o2_anomaly = np.concatenate([
        np.random.normal(85, 3, n_anomaly//2),
        np.random.normal(98, 2, n_anomaly//2)
    ])
    o2_sat = np.concatenate([o2_normal, o2_anomaly])
    o2_sat = np.clip(o2_sat, 70, 100)
    
    labels = np.zeros(n_patients, dtype=int)
    labels[n_normal:] = 1
    
    indices = np.random.permutation(n_patients)
    heart_rate = heart_rate[indices]
    systolic = systolic[indices]
    diastolic = diastolic[indices]
    temperature = temperature[indices]
    respiratory = respiratory[indices]
    o2_sat = o2_sat[indices]
    labels = labels[indices]
    
    df = pd.DataFrame({
        'patient_id': range(n_patients),
        'heart_rate': heart_rate,
        'systolic_bp': systolic,
        'diastolic_bp': diastolic,
        'temperature': temperature,
        'respiratory_rate': respiratory,
        'oxygen_saturation': o2_sat,
        'is_anomaly': labels
    })
    
    features = ['heart_rate', 'systolic_bp', 'diastolic_bp', 'temperature',
                'respiratory_rate', 'oxygen_saturation']
    X = df[features].values
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    k_dist, suggested_eps = parameter_tuning_with_kdistance(X_scaled, k=5)
    eps_value = suggested_eps[90]
    
    dbscan = DBSCAN(eps=eps_value, min_samples=10)
    labels = dbscan.fit_predict(X_scaled)
    
    return df, labels, scaler


def analyze_patient_anomalies(df, labels):
    """
    Analyze DBSCAN clusters for patient anomalies.
    
    Parameters:
    -----------
    df : DataFrame
        Patient data
    labels : ndarray
        Cluster labels
        
    Returns:
    --------
    analysis : dict
        Analysis results
    """
    df = df.copy()
    df['cluster'] = labels
    
    outlier_mask = labels == -1
    outliers = df[outlier_mask]
    
    analysis = {
        'n_clusters': len(set(labels)) - (1 if -1 in labels else 0),
        'n_outliers': len(outliers),
        'outlier_rate': len(outliers) / len(df),
        'true_anomalies_in_outliers': outliers['is_anomaly'].sum(),
        'detection_rate': 0
    }
    
    total_anomalies = df['is_anomaly'].sum()
    if total_anomalies > 0:
        analysis['detection_rate'] = outliers['is_anomaly'].sum() / total_anomalies
    
    return analysis


def spatial_healthcare_example(n_locations=1000, random_state=42):
    """
    Healthcare example: Geographic disease outbreak detection.
    
    This demonstrates DBSCAN for spatial clustering to detect disease outbreaks:
    - Normal case distribution is sparse/diffuse
    - Outbreaks form dense geographic clusters
    
    Parameters:
    -----------
    n_locations : int
        Number of patient locations
    random_state : int
        Random seed
        
    Returns:
    --------
    df : DataFrame
        Location data
    labels : ndarray
        Cluster labels
    """
    np.random.seed(random_state)
    
    n_outbreak_locations = 5
    outbreak_size = 50
    
    base_x = np.random.uniform(0, 100, n_locations)
    base_y = np.random.uniform(0, 100, n_locations)
    
    outbreak_centers = [(20, 20), (80, 30), (50, 70), (30, 80), (70, 80)]
    
    outbreak_x = []
    outbreak_y = []
    outbreak_labels = []
    
    for i, (cx, cy) in enumerate(outbreak_centers):
        ox = np.random.normal(cx, 3, outbreak_size)
        oy = np.random.normal(cy, 3, outbreak_size)
        outbreak_x.extend(ox)
        outbreak_y.extend(oy)
        outbreak_labels.extend([i] * outbreak_size)
    
    x = np.concatenate([base_x, outbreak_x])
    y = np.concatenate([base_y, outbreak_y])
    
    x = np.clip(x, 0, 100)
    y = np.clip(y, 0, 100)
    
    is_outbreak = np.zeros(len(x), dtype=int)
    is_outbreak[n_locations:] = 1
    
    indices = np.random.permutation(len(x))
    x = x[indices]
    y = y[indices]
    is_outbreak = is_outbreak[indices]
    
    df = pd.DataFrame({
        'location_id': range(len(x)),
        'x': x,
        'y': y,
        'is_outbreak': is_outbreak
    })
    
    X = df[['x', 'y']].values
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    dbscan = DBSCAN(eps=3.0, min_samples=5)
    labels = dbscan.fit_predict(X_scaled)
    
    return df, labels


def analyze_outbreak_clusters(df, labels):
    """
    Analyze geographic clusters for disease outbreaks.
    
    Parameters:
    -----------
    df : DataFrame
        Location data
    labels : ndarray
        Cluster labels
        
    Returns:
    --------
    analysis : dict
        Cluster analysis
    """
    df = df.copy()
    df['cluster'] = labels
    
    unique_labels = sorted(set(labels))
    
    outbreak_clusters = []
    
    for label in unique_labels:
        if label == -1:
            continue
        cluster_data = df[df['cluster'] == label]
        
        if len(cluster_data) > 0:
            outbreak_rate = cluster_data['is_outbreak'].mean()
            outbreak_clusters.append({
                'cluster': label,
                'size': len(cluster_data),
                'outbreak_rate': outbreak_rate,
                'center_x': cluster_data['x'].mean(),
                'center_y': cluster_data['y'].mean()
            })
    
    return pd.DataFrame(outbreak_clusters)


def evaluate_clustering_quality(X, labels, y_true=None):
    """
    Comprehensive clustering quality evaluation.
    
    This provides multiple metrics for evaluating clustering performance:
    - Silhouette Score: Measures cluster cohesion and separation
    - Calinski-Harabasz Index: Ratio of between-cluster to within-cluster variance
    - Davies-Bouldin Index: Average similarity of each cluster to most similar cluster
    
    Parameters:
    -----------
    X : ndarray
        Feature data
    labels : ndarray
        Cluster labels
    y_true : ndarray, optional
        True labels forARI calculation
        
    Returns:
    --------
    metrics : dict
        Evaluation metrics
    """
    n_clusters = len(set(labels)) - (1 if -1 in labels else 0)
    n_noise = list(labels).count(-1)
    
    if n_clusters < 2 or n_clusters > len(X) - 1:
        return {
            'n_clusters': n_clusters,
            'n_noise': n_noise,
            'silhouette': -1,
            'calinski_harabasz': 0,
            'davies_bouldin': float('inf'),
            'ari': -1
        }
    
    try:
        silhouette = silhouette_score(X, labels)
    except:
        silhouette = -1
        
    try:
        calinski = calinski_harabasz_score(X, labels)
    except:
        calinski = 0
        
    try:
        davies = davies_bouldin_score(X, labels)
    except:
        davies = float('inf')
    
    from sklearn.metrics import adjusted_rand_score
    if y_true is not None:
        ari = adjusted_rand_score(y_true, labels)
    else:
        ari = -1
    
    metrics = {
        'n_clusters': n_clusters,
        'n_noise': n_noise,
        'silhouette': silhouette,
        'calinski_harabasz': calinski,
        'davies_bouldin': davies,
        'ari': ari
    }
    
    return metrics


def demonstrate_parameter_effects():
    """
    Demonstrate the effects of different DBSCAN parameters.
    
    This shows how eps and min_samples affect clustering results.
    """
    X, y = generate_dense_clusters(n_samples=300, n_clusters=4, random_state=42)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print("\n" + "="*70)
    print("PARAMETER SENSITIVITY DEMONSTRATION")
    print("="*70)
    
    print("\n--- Epsilon Effects ---")
    eps_values = [0.3, 0.5, 0.7, 1.0, 1.5]
    min_samples = 5
    
    print(f"\n{'eps':>8} {'clusters':>10} {'noise':>10} {'silhouette':>12}")
    print("-" * 45)
    
    for eps in eps_values:
        dbscan = DBSCAN(eps=eps, min_samples=min_samples)
        labels = dbscan.fit_predict(X_scaled)
        
        metrics = evaluate_clustering_quality(X_scaled, labels)
        
        print(f"{eps:>8.1f} {metrics['n_clusters']:>10} {metrics['n_noise']:>10} {metrics['silhouette']:>12.4f}")
    
    print("\n--- Min_samples Effects ---")
    eps = 0.7
    min_samples_values = [3, 5, 7, 10, 15]
    
    print(f"\n{'min_samples':>12} {'clusters':>10} {'noise':>10} {'silhouette':>12}")
    print("-" * 45)
    
    for min_s in min_samples_values:
        dbscan = DBSCAN(eps=eps, min_samples=min_s)
        labels = dbscan.fit_predict(X_scaled)
        
        metrics = evaluate_clustering_quality(X_scaled, labels)
        
        print(f"{min_s:>12d} {metrics['n_clusters']:>10} {metrics['n_noise']:>10} {metrics['silhouette']:>12.4f}")


def demonstrate_nonconvex_clustering():
    """
    Demonstrate DBSCAN on non-convex data where K-Means fails.
    """
    X, y = generate_nonconvex_data(n_samples=400, noise=0.1, random_state=42)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print("\n" + "="*70)
    print("NON-CONVEX CLUSTERING DEMONSTRATION")
    print("="*70)
    
    print("\n--- DBSCAN Results ---")
    dbscan = DBSCAN(eps=0.4, min_samples=5)
    dbscan_labels = dbscan.fit_predict(X_scaled)
    dbscan_metrics = evaluate_clustering_quality(X_scaled, dbscan_labels, y)
    
    print(f"Clusters: {dbscan_metrics['n_clusters']}")
    print(f"Noise points: {dbscan_metrics['n_noise']}")
    print(f"Silhouette: {dbscan_metrics['silhouette']:.4f}")
    print(f"ARI: {dbscan_metrics['ari']:.4f}")
    
    print("\n--- K-Means Results ---")
    from sklearn.cluster import KMeans
    
    kmeans = KMeans(n_clusters=2, random_state=42, n_init=10)
    kmeans_labels = kmeans.fit_predict(X_scaled)
    kmeans_metrics = evaluate_clustering_quality(X_scaled, kmeans_labels, y)
    
    print(f"Clusters: {kmeans_metrics['n_clusters']}")
    print(f"Noise points: {kmeans_metrics['n_noise']}")
    print(f"Silhouette: {kmeans_metrics['silhouette']:.4f}")
    print(f"ARI: {kmeans_metrics['ari']:.4f}")
    
    print("\n--- Comparison ---")
    print(f"DBSCAN silhouette: {dbscan_metrics['silhouette']:.4f}")
    print(f"K-Means silhouette: {kmeans_metrics['silhouette']:.4f}")
    print(f"DBSCAN correctly handles non-convex shapes!")


def run_banking_example():
    """
    Run comprehensive banking fraud detection example.
    """
    print("\n" + "="*70)
    print("BANKING EXAMPLE: FRAUD DETECTION")
    print("="*70)
    
    df, labels, scaler = banking_fraud_detection_example(n_transactions=5000, random_state=42)
    
    print(f"\nDataset: {len(df)} transactions")
    print(f"Fraud rate: {df['is_fraud'].mean():.2%}")
    
    cluster_analysis = analyze_fraud_clusters(df, labels)
    
    print(f"\nClusters found: {cluster_analysis[cluster_analysis['type'] == 'cluster']['cluster'].nunique()}")
    print(f"Noise points: {cluster_analysis[cluster_analysis['type'] == 'noise']['size'].values[0]}")
    
    high_risk = cluster_analysis[cluster_analysis['fraud_rate'] > 0.03]
    if len(high_risk) > 0:
        print(f"\nHigh-risk clusters (fraud rate > 3%):")
        for _, row in high_risk.iterrows():
            cluster_label = row['cluster']
            if cluster_label != -1:
                print(f"  Cluster {cluster_label}: {row['size']} transactions, {row['fraud_rate']:.2%} fraud rate")
    
    outlier_cluster = cluster_analysis[cluster_analysis['cluster'] == -1]
    if len(outlier_cluster) > 0:
        print(f"\nNoise/outlier cluster:")
        print(f"  Size: {outlier_cluster['size'].values[0]}")
        print(f"  Fraud rate: {outlier_cluster['fraud_rate'].values[0]:.2%}")
    
    print("\n--- Customer Segmentation ---")
    cust_df, cust_labels, cust_scaler = banking_customer_segmentation_example(n_customers=2000, random_state=42)
    
    print(f"\nDataset: {len(cust_df)} customers")
    print(f"Clusters: {len(set(cust_labels)) - 1}")
    print(f"Unusual customers: {list(cust_labels).count(-1)}")


def run_healthcare_example():
    """
    Run comprehensive healthcare examples.
    """
    print("\n" + "="*70)
    print("HEALTHCARE EXAMPLE: PATIENT ANOMALY DETECTION")
    print("="*70)
    
    df, labels, scaler = healthcare_patient_anomaly_example(n_patients=2000, random_state=42)
    
    print(f"\nDataset: {len(df)} patients")
    print(f"Known anomalies: {df['is_anomaly'].sum()}")
    
    analysis = analyze_patient_anomalies(df, labels)
    
    print(f"\nClusters: {analysis['n_clusters']}")
    print(f"Detected outliers: {analysis['n_outliers']}")
    print(f"True anomalies in outliers: {analysis['true_anomalies_in_outliers']}")
    print(f"Detection rate: {analysis['detection_rate']:.1%}")
    
    print("\n--- Spatial Outbreak Detection ---")
    spatial_df, spatial_labels = spatial_healthcare_example(n_locations=1000, random_state=42)
    
    print(f"\nDataset: {len(spatial_df)} locations")
    print(f"Outbreak locations: {spatial_df['is_outbreak'].sum()}")
    
    outbreak_analysis = analyze_outbreak_clusters(spatial_df, spatial_labels)
    
    print(f"\nDetected clusters: {len(outbreak_analysis)}")
    
    if len(outbreak_analysis) > 0:
        print("\nOutbreak clusters found:")
        for _, row in outbreak_analysis.iterrows():
            print(f"  Cluster {row['cluster']}: {row['size']} locations, {row['outbreak_rate']:.1%} outbreak rate")


def run_advanced_examples():
    """
    Run advanced DBSCAN examples and demonstrations.
    """
    print("\n" + "="*70)
    print("ADVANCED EXAMPLES")
    print("="*70)
    
    print("\n--- Point Type Classification ---")
    X, y = generate_nonconvex_data(n_samples=200, random_state=42)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    point_types = show_point_types(X_scaled, eps=0.4, min_samples=5)
    
    print(f"Core points: {len(point_types['core'])}")
    print(f"Border points: {len(point_types['border'])}")
    print(f"Noise points: {len(point_types['noise'])}")
    
    print("\n--- K-Distance Analysis ---")
    k_dist, suggested_eps = parameter_tuning_with_kdistance(X_scaled, k=5)
    
    print(f"\nK-distance percentiles:")
    for p, eps in suggested_eps.items():
        print(f"  {p}th percentile: {eps:.3f}")


def print_section_header(title):
    """Print a formatted section header."""
    print("\n" + "="*70)
    print(title)
    print("="*70)


def main():
    """
    Main function to run all DBSCAN examples and demonstrations.
    """
    print_section_header("DBSCAN AND DENSITY-BASED CLUSTERING IMPLEMENTATION")
    print("\nThis comprehensive implementation demonstrates:")
    print("- DBSCAN algorithm fundamentals")
    print("- Parameter tuning and selection")
    print("- Banking fraud detection application")
    print("- Healthcare anomaly detection")
    print("- Comparison with K-Means")
    print("- Advanced clustering scenarios")
    
    demonstrate_parameter_effects()
    
    demonstrate_nonconvex_clustering()
    
    run_banking_example()
    
    run_healthcare_example()
    
    run_advanced_examples()
    
    print_section_header("IMPLEMENTATION COMPLETE")
    print("\nDBSCAN key advantages:")
    print("- Discovers arbitrary-shaped clusters")
    print("- Automatic outlier/noise detection")
    print("- Does not require pre-specifying cluster count")
    print("- Handles high-dimensional data well")
    print("\nUse cases:")
    print("- Fraud detection in banking")
    print("- Medical anomaly detection")
    print("- Spatial clustering for location data")
    print("- Customer behavior segmentation")
    print("- Network intrusion detection")


if __name__ == "__main__":
    main()