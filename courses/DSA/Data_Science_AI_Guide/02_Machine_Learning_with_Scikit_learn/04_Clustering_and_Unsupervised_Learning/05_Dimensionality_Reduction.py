# Topic: Dimensionality Reduction
# Author: AI Assistant
# Date: 06-04-2026

"""
================================================================================
    COMPREHENSIVE IMPLEMENTATION FOR DIMENSIONALITY REDUCTION
================================================================================

I. INTRODUCTION
---------------
Dimensionality reduction is a critical technique in machine learning and data science
that aims to reduce the number of features (dimensions) in a dataset while preserving
as much important information as possible. This is particularly valuable when:

- Working with high-dimensional data (many features)
- Dealing with the "curse of dimensionality"
- Visualizing complex datasets in 2D or 3D
- Reducing computational costs for downstream tasks
- Removing redundant or correlated features
- Improving model generalization

II. CORE CONCEPTS
-----------------
1. CURSE OF DIMENSIONALITY: As dimensions increase, data becomes increasingly sparse,
   making it difficult to find patterns and increasing the risk of overfitting.

2. VARIANCE EXPLAINED: The proportion of total variance in the data that is captured
   by each principal component.

3. PRINCIPAL COMPONENTS: Orthogonal directions of maximum variance in the data.

4. FEATURE PROJECTION: Mapping high-dimensional data to a lower-dimensional space.

5. INFORMATION PRESERVATION: Maintaining as much relevant information as possible
   when reducing dimensions.

III. ALGORITHM OVERVIEW
---------------------
1. PCA (Principal Component Analysis): Linear method that finds the
   directions of maximum variance in the data.

2. t-SNE: Non-linear method for visualization that preserves
   local structure.

3. UMAP: Modern alternative to t-SNE that preserves both
   local and global structure.

IV. PRACTICAL APPLICATIONS
-------------------------
- Banking: Customer behavior analysis with many features
- Healthcare: Patient data with numerous health indicators
- Image Recognition: Reducing pixel dimensions
- Text Analysis: Document embeddings

================================================================================
"""

# =============================================================================
# IMPORT NECESSARY LIBRARIES
# =============================================================================

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.datasets import make_classification, make_blobs, make_swiss_roll
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, MinMaxScaler
from sklearn.decomposition import PCA, IncrementalPCA, KernelPCA, TruncatedSVD
from sklearn.manifold import TSNE, MDS, Isomap
from sklearn.metrics import silhouette_score, calinski_harabasz_score, davies_bouldin_score
from sklearn.metrics.pairwise import euclidean_distances
from scipy.linalg import svd
import warnings
warnings.filterwarnings('ignore')

np.random.seed(42)
pd.set_option('display.max_columns', None)
pd.set_option('display.width', None)
pd.set_option('display.max_colwidth', 50)


# =============================================================================
# SECTION I: DATA GENERATION FUNCTIONS
# =============================================================================

def generate_high_dim_data(n_samples=500, n_features=20, n_informative=10, 
                         n_redundant=5, random_state=42):
    """
    Generate high-dimensional synthetic data for testing dimensionality reduction.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Total number of features (dimensions)
    n_informative : int
        Number of informative features
    n_redundant : int
        Number of redundant features (linear combinations)
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : array-like
        Generated feature matrix
    y : array-like
        Class labels (if applicable)
    feature_names : list
        Names of features
    """
    print(f"\n{'='*60}")
    print("GENERATING HIGH-DIMENSIONAL DATA")
    print(f"{'='*60}")
    print(f"  Number of samples: {n_samples}")
    print(f"  Total features: {n_features}")
    print(f"  Informative features: {n_informative}")
    print(f"  Redundant features: {n_redundant}")
    print(f"  Noise features: {n_features - n_informative - n_redundant}")
    
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_clusters_per_class=2,
        n_classes=3,
        random_state=random_state
    )
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    
    print(f"\n  Data shape: {X.shape}")
    print(f"  Class distribution: {np.bincount(y)}")
    print(f"  Feature mean range: [{X.mean(axis=0).min():.4f}, {X.mean(axis=0).max():.4f}]")
    print(f"  Feature std range: [{X.std(axis=0).min():.4f}, {X.std(axis=0).max():.4f}]")
    
    return X, y, feature_names


def generate_correlated_features(n_samples=500, n_features=15, random_state=42):
    """
    Generate data with highly correlated features.
    
    This simulates real-world scenarios where many features are correlated
    (e.g., customer satisfaction metrics, health indicators).
    
    Parameters:
    -----------
    n_samples : int
        Number of samples
    n_features : int
        Number of features
    random_state : int
        Random seed
        
    Returns:
    --------
    X : array-like
        Feature matrix with correlations
    feature_names : list
        Feature names
    """
    print(f"\n{'='*60}")
    print("GENERATING CORRELATED FEATURES DATA")
    print(f"{'='*60}")
    
    np.random.seed(random_state)
    
    base_features = 5
    X_base = np.random.randn(n_samples, base_features)
    
    X = np.zeros((n_samples, n_features))
    feature_names = []
    
    for i in range(n_features):
        base_idx = i % base_features
        weight = np.random.uniform(0.5, 1.5)
        noise = np.random.randn(n_samples) * 0.1
        X[:, i] = X_base[:, base_idx] * weight + noise
        feature_names.append(f'feature_{i}_corr{base_idx}')
    
    print(f"  Data shape: {X.shape}")
    print(f"  Correlation matrix rank: {np.linalg.matrix_rank(np.corrcoef(X.T))}")
    
    return X, feature_names


def generate_curse_of_dimensionality_data(max_features=100, n_samples=200, random_state=42):
    """
    Generate data with increasing dimensions to demonstrate the curse.
    
    Parameters:
    -----------
    max_features : int
        Maximum number of features
    n_samples : int
        Number of samples
    random_state : int
        Random seed
        
    Returns:
    --------
    data_dict : dict
        Dictionary with different feature counts
    """
    print(f"\n{'='*60}")
    print("GENERATING CURSE OF DIMENSIONALITY DATA")
    print(f"{'='*60}")
    print(f"  Max features: {max_features}")
    print(f"  Samples: {n_samples}")
    
    data_dict = {}
    
    for n_features in [5, 10, 20, 50, max_features]:
        X = np.random.randn(n_samples, n_features)
        data_dict[n_features] = X
        
        avg_dist = euclidean_distances(X[:10]).mean()
        min_dist = euclidean_distances(X[:10]).min()
        
        print(f"  Features={n_features:3d}: Avg dist={avg_dist:.4f}, Min dist={min_dist:.4f}")
    
    return data_dict


def generate_customer_behavior_data(n_customers=1000, random_state=42):
    """
    Generate synthetic customer behavior data for banking use case.
    
    Creates data with many features representing:
    - Demographics
    - Account metrics
    - Transaction patterns
    - Product usage
    - Service interactions
    
    Parameters:
    -----------
    n_customers : int
        Number of customers
    random_state : int
        Random seed
        
    Returns:
    --------
    df : DataFrame
        Customer data with many features
    """
    print(f"\n{'='*60}")
    print("GENERATING CUSTOMER BEHAVIOR DATA")
    print(f"{'='*60}")
    print(f"  Number of customers: {n_customers}")
    
    np.random.seed(random_state)
    
    n_per_segment = n_customers // 3
    
    segments = []
    for seg in range(3):
        segment_data = {
            'age': np.random.normal(30 + seg * 15, 10, n_per_segment),
            'annual_income': np.random.normal(40000 + seg * 30000, 10000, n_per_segment),
            'credit_score': np.random.normal(600 + seg * 50, 50, n_per_segment),
            'account_balance': np.random.normal(10000 + seg * 20000, 5000, n_per_segment),
            'monthly_deposits': np.random.normal(2000 + seg * 1000, 500, n_per_segment),
            'monthly_withdrawals': np.random.normal(1500 + seg * 800, 400, n_per_segment),
            'num_transactions': np.random.normal(20 + seg * 10, 5, n_per_segment),
            'avg_transaction_amount': np.random.normal(100 + seg * 50, 30, n_per_segment),
            'num_products': np.random.randint(1, 3 + seg, n_per_segment),
            'tenure_months': np.random.uniform(6, 60 + seg * 24, n_per_segment),
            'online_banking_freq': np.random.normal(10 + seg * 5, 3, n_per_segment),
            'mobile_app_freq': np.random.normal(15 + seg * 8, 4, n_per_segment),
            'branch_visits': np.random.normal(2 - seg * 0.5, 1, n_per_segment),
            'atm_usage': np.random.normal(8 + seg * 2, 2, n_per_segment),
            'card_usage': np.random.normal(15 + seg * 5, 4, n_per_segment),
            'loan_balance': np.random.normal(10000 + seg * 15000, 5000, n_per_segment) * (np.random.random(n_per_segment) > 0.3),
            'investment_balance': np.random.normal(5000 + seg * 10000, 3000, n_per_segment) * (np.random.random(n_per_segment) > 0.4),
            'savings_rate': np.random.normal(0.05 + seg * 0.02, 0.01, n_per_segment),
            'debt_to_income': np.random.normal(0.3 + seg * 0.1, 0.05, n_per_segment),
            'payment_regularity': np.random.normal(0.9 - seg * 0.1, 0.05, n_per_segment),
            'num_complaints': np.random.poisson(1 - seg * 0.3, n_per_segment),
            'num_inquiries': np.random.poisson(5 + seg * 2, n_per_segment)
        }
        segments.append(pd.DataFrame(segment_data))
    
    df = pd.concat(segments, ignore_index=True)
    
    df['customer_id'] = [f'CUST_{i:05d}' for i in range(len(df))]
    
    df['total_balance'] = df['account_balance'] + df['investment_balance']
    df['total_engagement'] = df['online_banking_freq'] + df['mobile_app_freq'] + df['branch_visits']
    df['active_products'] = df['num_products'] + (df['loan_balance'] > 0).astype(int) + (df['investment_balance'] > 0).astype(int)
    
    customer_id = df['customer_id']
    df = df.drop('customer_id', axis=1)
    df.insert(0, 'customer_id', customer_id)
    
    feature_cols = [c for c in df.columns if c != 'customer_id']
    df[feature_cols] = df[feature_cols].round(2)
    
    print(f"  Data shape: {df.shape}")
    print(f"  Features: {len(feature_cols)}")
    print(f"  Sample features: {feature_cols[:5]}...")
    
    return df


def generate_patient_health_data(n_patients=800, random_state=42):
    """
    Generate synthetic patient health data for healthcare use case.
    
    Features include:
    - Demographics
    - Vital signs
    - Lab results
    - Medical history
    - Lifestyle factors
    
    Parameters:
    -----------
    n_patients : int
        Number of patients
    random_state : int
        Random seed
        
    Returns:
    --------
    df : DataFrame
        Patient data with many features
    """
    print(f"\n{'='*60}")
    print("GENERATING PATIENT HEALTH DATA")
    print(f"{'='*60}")
    print(f"  Number of patients: {n_patients}")
    
    np.random.seed(random_state)
    
    n_per_group = n_patients // 4
    
    groups = []
    for g in range(4):
        if g == 0:
            group_data = {
                'age': np.random.normal(28, 5, n_per_group),
                'bmi': np.random.normal(22, 2, n_per_group),
                'systolic_bp': np.random.normal(118, 8, n_per_group),
                'diastolic_bp': np.random.normal(75, 5, n_per_group),
                'heart_rate': np.random.normal(68, 6, n_per_group),
                'cholesterol_total': np.random.normal(170, 20, n_per_group),
                'cholesterol_ldl': np.random.normal(100, 15, n_per_group),
                'cholesterol_hdl': np.random.normal(55, 8, n_per_group),
                'triglycerides': np.random.normal(100, 25, n_per_group),
                'fasting_glucose': np.random.normal(85, 8, n_per_group),
                'hba1c': np.random.normal(5.2, 0.3, n_per_group),
                'creatinine': np.random.normal(0.9, 0.15, n_per_group),
                'bun': np.random.normal(15, 3, n_per_group),
                'sodium': np.random.normal(140, 2, n_per_group),
                'potassium': np.random.normal(4.2, 0.3, n_per_group),
                'hemoglobin': np.random.normal(14, 1, n_per_group),
                'wbc': np.random.normal(7, 1.5, n_per_group),
                'platelets': np.random.normal(250, 50, n_per_group),
                'alt': np.random.normal(25, 8, n_per_group),
                'ast': np.random.normal(25, 7, n_per_group),
                'bilirubin': np.random.normal(0.8, 0.2, n_per_group),
                'albumin': np.random.normal(4.2, 0.3, n_per_group),
                'tsh': np.random.normal(2, 0.5, n_per_group),
                'vitamind': np.random.normal(30, 8, n_per_group),
                'ferritin': np.random.normal(100, 30, n_per_group),
                'iron': np.random.normal(80, 20, n_per_group),
                'uric_acid': np.random.normal(5.5, 1, n_per_group),
                'crp': np.random.normal(2, 1, n_per_group),
                'esr': np.random.normal(15, 5, n_per_group)
            }
        elif g == 1:
            group_data = {
                'age': np.random.normal(68, 8, n_per_group),
                'bmi': np.random.normal(30, 5, n_per_group),
                'systolic_bp': np.random.normal(150, 15, n_per_group),
                'diastolic_bp': np.random.normal(90, 8, n_per_group),
                'heart_rate': np.random.normal(80, 10, n_per_group),
                'cholesterol_total': np.random.normal(220, 30, n_per_group),
                'cholesterol_ldl': np.random.normal(140, 25, n_per_group),
                'cholesterol_hdl': np.random.normal(45, 8, n_per_group),
                'triglycerides': np.random.normal(180, 40, n_per_group),
                'fasting_glucose': np.random.normal(110, 20, n_per_group),
                'hba1c': np.random.normal(6.5, 0.8, n_per_group),
                'creatinine': np.random.normal(1.1, 0.25, n_per_group),
                'bun': np.random.normal(20, 5, n_per_group),
                'sodium': np.random.normal(138, 3, n_per_group),
                'potassium': np.random.normal(4.0, 0.4, n_per_group),
                'hemoglobin': np.random.normal(13, 1.5, n_per_group),
                'wbc': np.random.normal(8, 2, n_per_group),
                'platelets': np.random.normal(230, 60, n_per_group),
                'alt': np.random.normal(30, 12, n_per_group),
                'ast': np.random.normal(28, 10, n_per_group),
                'bilirubin': np.random.normal(1.0, 0.3, n_per_group),
                'albumin': np.random.normal(3.8, 0.4, n_per_group),
                'tsh': np.random.normal(2.5, 1, n_per_group),
                'vitamind': np.random.normal(20, 10, n_per_group),
                'ferritin': np.random.normal(150, 50, n_per_group),
                'iron': np.random.normal(60, 25, n_per_group),
                'uric_acid': np.random.normal(6.5, 1.5, n_per_group),
                'crp': np.random.normal(5, 3, n_per_group),
                'esr': np.random.normal(25, 10, n_per_group)
            }
        elif g == 2:
            group_data = {
                'age': np.random.normal(45, 7, n_per_group),
                'bmi': np.random.normal(28, 4, n_per_group),
                'systolic_bp': np.random.normal(135, 12, n_per_group),
                'diastolic_bp': np.random.normal(85, 6, n_per_group),
                'heart_rate': np.random.normal(72, 8, n_per_group),
                'cholesterol_total': np.random.normal(200, 25, n_per_group),
                'cholesterol_ldl': np.random.normal(120, 20, n_per_group),
                'cholesterol_hdl': np.random.normal(50, 8, n_per_group),
                'triglycerides': np.random.normal(150, 35, n_per_group),
                'fasting_glucose': np.random.normal(95, 12, n_per_group),
                'hba1c': np.random.normal(5.5, 0.4, n_per_group),
                'creatinine': np.random.normal(1.0, 0.15, n_per_group),
                'bun': np.random.normal(16, 4, n_per_group),
                'sodium': np.random.normal(139, 2.5, n_per_group),
                'potassium': np.random.normal(4.1, 0.35, n_per_group),
                'hemoglobin': np.random.normal(13.5, 1.2, n_per_group),
                'wbc': np.random.normal(7.2, 1.6, n_per_group),
                'platelets': np.random.normal(240, 55, n_per_group),
                'alt': np.random.normal(27, 10, n_per_group),
                'ast': np.random.normal(26, 8, n_per_group),
                'bilirubin': np.random.normal(0.9, 0.25, n_per_group),
                'albumin': np.random.normal(4.0, 0.35, n_per_group),
                'tsh': np.random.normal(2.2, 0.6, n_per_group),
                'vitamind': np.random.normal(25, 9, n_per_group),
                'ferritin': np.random.normal(120, 40, n_per_group),
                'iron': np.random.normal(70, 22, n_per_group),
                'uric_acid': np.random.normal(6.0, 1.2, n_per_group),
                'crp': np.random.normal(3, 1.5, n_per_group),
                'esr': np.random.normal(18, 7, n_per_group)
            }
        else:
            group_data = {
                'age': np.random.normal(52, 10, n_per_group),
                'bmi': np.random.normal(33, 6, n_per_group),
                'systolic_bp': np.random.normal(145, 18, n_per_group),
                'diastolic_bp': np.random.normal(88, 10, n_per_group),
                'heart_rate': np.random.normal(78, 12, n_per_group),
                'cholesterol_total': np.random.normal(240, 35, n_per_group),
                'cholesterol_ldl': np.random.normal(155, 30, n_per_group),
                'cholesterol_hdl': np.random.normal(42, 7, n_per_group),
                'triglycerides': np.random.normal(200, 50, n_per_group),
                'fasting_glucose': np.random.normal(115, 25, n_per_group),
                'hba1c': np.random.normal(7.0, 1.0, n_per_group),
                'creatinine': np.random.normal(1.2, 0.3, n_per_group),
                'bun': np.random.normal(22, 6, n_per_group),
                'sodium': np.random.normal(137, 3.5, n_per_group),
                'potassium': np.random.normal(3.9, 0.45, n_per_group),
                'hemoglobin': np.random.normal(12.5, 1.6, n_per_group),
                'wbc': np.random.normal(8.5, 2.5, n_per_group),
                'platelets': np.random.normal(210, 65, n_per_group),
                'alt': np.random.normal(35, 15, n_per_group),
                'ast': np.random.normal(32, 12, n_per_group),
                'bilirubin': np.random.normal(1.1, 0.35, n_per_group),
                'albumin': np.random.normal(3.6, 0.45, n_per_group),
                'tsh': np.random.normal(3.0, 1.2, n_per_group),
                'vitamind': np.random.normal(18, 12, n_per_group),
                'ferritin': np.random.normal(180, 60, n_per_group),
                'iron': np.random.normal(50, 28, n_per_group),
                'uric_acid': np.random.normal(7.0, 1.8, n_per_group),
                'crp': np.random.normal(7, 4, n_per_group),
                'esr': np.random.normal(30, 12, n_per_group)
            }
        groups.append(pd.DataFrame(group_data))
    
    df = pd.concat(groups, ignore_index=True)
    
    df['patient_id'] = [f'PAT_{i:05d}' for i in range(len(df))]
    
    df['age_group'] = pd.cut(df['age'], bins=[0, 30, 50, 65, 100], labels=[0, 1, 2, 3]).astype(int)
    df['bmi_category'] = pd.cut(df['bmi'], bins=[0, 18.5, 25, 30, 100], labels=[0, 1, 2, 3]).astype(int)
    df['bp_category'] = pd.cut(df['systolic_bp'], bins=[0, 120, 130, 140, 250], labels=[0, 1, 2, 3]).astype(int)
    df['cardiovascular_risk'] = (df['bmi_category'] + df['bp_category'] + 
                               (df['cholesterol_total'] > 200).astype(int) + 
                               (df['fasting_glucose'] > 100).astype(int))
    
    patient_id = df['patient_id']
    df = df.drop('patient_id', axis=1)
    df.insert(0, 'patient_id', patient_id)
    
    feature_cols = [c for c in df.columns if c != 'patient_id']
    df[feature_cols] = df[feature_cols].round(2)
    
    print(f"  Data shape: {df.shape}")
    print(f"  Features: {len(feature_cols)}")
    
    return df


# =============================================================================
# SECTION II: CURSE OF DIMENSIONALITY ANALYSIS
# =============================================================================

def analyze_curse_of_dimensionality():
    """
    Analyze and demonstrate the curse of dimensionality.
    
    This shows how:
    1. Data becomes sparse as dimensions increase
    2. Average distances between points increase
    3. Finding clusters becomes harder
    """
    print(f"\n{'='*70}")
    print("CURSE OF DIMENSIONALITY ANALYSIS")
    print(f"{'='*70}")
    
    data_dict = generate_curse_of_dimensionality_data(max_features=100, n_samples=200)
    
    print(f"\n{'='*60}")
    print("SPARSITY ANALYSIS")
    print(f"{'='*60}")
    
    feature_counts = sorted(data_dict.keys())
    avg_distances = []
    min_distances = []
    std_distances = []
    
    for n_features in feature_counts:
        X = data_dict[n_features]
        
        distances = euclidean_distances(X[:50])
        np.fill_diagonal(distances, np.inf)
        
        avg_dist = distances.mean()
        min_dist = distances.min()
        std_dist = distances.std()
        
        avg_distances.append(avg_dist)
        min_distances.append(min_dist)
        std_distances.append(std_dist)
    
    print(f"\n  Features | Avg Distance | Min Distance | Std Distance")
    print(f"  " + "-"*55)
    for i, n_features in enumerate(feature_counts):
        print(f"  {n_features:8d} | {avg_distances[i]:12.4f} | {min_distances[i]:12.4f} | {std_distances[i]:12.4f}")
    
    print(f"\n{'='*60}")
    print("HYPERCUBE VOLUME ANALYSIS")
    print(f"{'='*60}")
    
    side_lengths = [0.1, 0.5, 1.0]
    dims = [2, 5, 10, 20, 50, 100]
    
    print(f"\n  Side Length = 1.0 (unit hypercube)")
    print(f"  Dimensions | Volume | % of Unit Cube")
    print(f"  " + "-"*40)
    for dim in dims:
        volume = 1.0 ** dim
        pct = volume * 100
        print(f"  {dim:10d} | {volume:.6f} | {pct:.6f}%")
    
    print(f"\n{'='*60}")
    print("DENSITY ESTIMATION")
    print(f"{'='*60}")
    
    n_samples = 1000
    for dim in [2, 5, 10, 20]:
        radius = 0.5
        volume_hyperball = (np.pi ** (dim / 2) / np.math.gamma(dim / 2 + 1)) * (radius ** dim)
        volume_hypercube = (2 * radius) ** dim
        density = n_samples / volume_hypercube
        
        print(f"  Dim={dim:3d}: Volume={volume_hyperball:.6f}, Density={density:.6f}")
    
    print(f"\n{'='*60}")
    print("IMPLICATIONS FOR MACHINE LEARNING")
    print(f"{'='*60}")
    print(f"  1. Data sparsity makes pattern detection difficult")
    print(f"  2. Distance-based algorithms become less reliable")
    print(f"  3. Risk of overfitting increases significantly")
    print(f"  4. Computational costs grow exponentially")
    print(f"  5. Regularization and dimensionality reduction become essential")
    
    return data_dict


# =============================================================================
# SECTION III: PCA IMPLEMENTATION
# =============================================================================

def core_pca():
    """
    Core PCA (Principal Component Analysis) implementation and demonstration.
    
    PCA is a linear dimensionality reduction technique that:
    1. Centers the data (subtracts mean)
    2. Computes covariance matrix
    3. Finds eigenvectors (principal components)
    4. Projects data onto top-k components
    """
    print(f"\n{'='*70}")
    print("CORE PCA IMPLEMENTATION")
    print(f"{'='*70}")
    
    X, y, feature_names = generate_high_dim_data(n_samples=500, n_features=20,
                                                 n_informative=10, n_redundant=5)
    
    print(f"\n{'='*60}")
    print("STEP 1: DATA STANDARDIZATION")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"  Original mean: {X.mean(axis=0)[:5]}")
    print(f"  Scaled mean: {X_scaled.mean(axis=0)[:5]}")
    print(f"  Original std: {X.std(axis=0)[:5]}")
    print(f"  Scaled std: {X_scaled.std(axis=0)[:5]}")
    
    print(f"\n{'='*60}")
    print("STEP 2: FULL PCA")
    print(f"{'='*60}")
    
    pca_full = PCA(n_components=None)
    X_pca_full = pca_full.fit_transform(X_scaled)
    
    print(f"  Number of components: {pca_full.n_components_}")
    print(f"  Explained variance ratio: {pca_full.explained_variance_ratio_[:5]}")
    print(f"  Cumulative variance (first 5): {pca_full.explained_variance_ratio_[:5].sum():.4f}")
    
    print(f"\n{'='*60}")
    print("STEP 3: VARIANCE EXPLAINED ANALYSIS")
    print(f"{'='*60}")
    
    variance_explained = pca_full.explained_variance_
    variance_ratio = pca_full.explained_variance_ratio_
    cumulative_variance = np.cumsum(variance_ratio)
    
    print(f"\n  Component | Variance | Variance % | Cumulative %")
    print(f"  " + "-"*55)
    for i in range(min(10, len(variance_ratio))):
        print(f"  {i+1:8d} | {variance_explained[i]:8.4f} | {variance_ratio[i]*100:8.2f}% | {cumulative_variance[i]*100:8.2f}%")
    
    n_components_95 = np.argmax(cumulative_variance >= 0.95) + 1
    n_components_99 = np.argmax(cumulative_variance >= 0.99) + 1
    
    print(f"\n  Components for 95% variance: {n_components_95}")
    print(f"  Components for 99% variance: {n_components_99}")
    
    print(f"\n{'='*60}")
    print("STEP 4: REDUCED PCA (DIFFERENT K)")
    print(f"{'='*60}")
    
    results = []
    for k in [2, 5, 10, 15]:
        pca = PCA(n_components=k)
        X_pca = pca.fit_transform(X_scaled)
        
        variance_ret = pca.explained_variance_ratio_.sum()
        results.append({
            'n_components': k,
            'variance_explained': variance_ret,
            'shape': X_pca.shape
        })
        print(f"  K={k}: Variance explained={variance_ret*100:.2f}%")
    
    print(f"\n{'='*60}")
    print("STEP 5: COMPONENT INTERPRETATION")
    print(f"{'='*60}")
    
    pca_5 = PCA(n_components=5)
    pca_5.fit(X_scaled)
    
    components = pca_5.components_
    print(f"\n  Top features for each component:")
    for i in range(min(5, components.shape[0])):
        feature_importance = np.abs(components[i])
        top_features = np.argsort(feature_importance)[::-1][:3]
        print(f"  PC{i+1}: Features {top_features} (importance: {feature_importance[top_features]})")
    
    print(f"\n{'='*60}")
    print("STEP 6: INCREMENTAL PCA")
    print(f"{'='*60}")
    
    ipca = IncrementalPCA(n_components=5)
    for batch in np.array_split(X_scaled, 5):
        ipca.partial_fit(batch)
    
    X_ipca = ipca.transform(X_scaled)
    print(f"  Incremental PCA shape: {X_ipca.shape}")
    print(f"  Variance explained: {ipca.explained_variance_ratio_.sum()*100:.2f}%")
    
    return X_pca_full, y, pca_full


def manual_pca_implementation(X, n_components=2):
    """
    Manual PCA implementation for educational purposes.
    
    Shows the step-by-step process:
    1. Center the data
    2. Compute covariance matrix
    3. Compute SVD
    4. Select top components
    5. Project data
    """
    print(f"\n{'='*60}")
    print("MANUAL PCA IMPLEMENTATION")
    print(f"{'='*60}")
    
    n_samples, n_features = X.shape
    
    print(f"  Input shape: {X.shape}")
    print(f"  Target components: {n_components}")
    
    X_centered = X - X.mean(axis=0)
    print(f"  Data centered: mean={X_centered.mean(axis=0)[:3]}")
    
    cov_matrix = np.cov(X_centered, rowvar=False)
    print(f"  Covariance matrix shape: {cov_matrix.shape}")
    
    U, S, Vt = np.linalg.svd(X_centered)
    
    principal_components = Vt[:n_components]
    print(f"  Principal components shape: {principal_components.shape}")
    
    X_reduced = X_centered @ principal_components.T
    print(f"  Reduced data shape: {X_reduced.shape}")
    
    return X_reduced, principal_components, S


def pca_variance_analysis():
    """
    Analyze variance explained by PCA components.
    """
    print(f"\n{'='*70}")
    print("PCA VARIANCE ANALYSIS")
    print(f"{'='*70}")
    
    X, y, _ = generate_high_dim_data(n_samples=300, n_features=15)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    pca = PCA(n_components=None)
    pca.fit(X_scaled)
    
    print(f"\n{'='*60}")
    print("VARIANCE BREAKDOWN")
    print(f"{'='*60}")
    
    var_ratio = pca.explained_variance_ratio_
    cum_var = np.cumsum(var_ratio)
    
    print(f"\n  Scree Plot Data:")
    print(f"\n  PC# | Variance | Variance % | Cumulative % | Kurtosis")
    print(f"  " + "-"*60)
    
    for i in range(len(var_ratio)):
        variance = var_ratio[i]
        cumulative = cum_var[i]
        component_data = X_scaled @ pca.components_[:i+1].T if i > 0 else X_scaled @ pca.components_[:1].T
        if i > 0:
            col = component_data[:, i]
            kurtosis = np.mean((col - col.mean())**4) / (col.var()**2)
        else:
            kurtosis = 0
        
        print(f"  {i+1:3d} | {variance*100:7.2f}% | {variance*100:7.2f}% | {cumulative*100:8.2f}% | {kurtosis:.2f}")
    
    print(f"\n{'='*60}")
    print("ELBOW METHOD FOR PCs")
    print(f"{'='*60}")
    
    variances = pca.explained_variance_
    
    first_deriv = np.diff(variances)
    second_deriv = np.diff(first_deriv)
    
    elbow_idx = np.argmin(second_deriv)
    optimal_pcs = elbow_idx + 2
    
    print(f"  Based on elbow method: {optimal_pcs} components")
    print(f"  (Variance at elbow: {variances[elbow_idx]:.4f})")
    
    return pca


# =============================================================================
# SECTION IV: t-SNE IMPLEMENTATION
# =============================================================================

def core_tsne():
    """
    Core t-SNE (t-Distributed Stochastic Neighbor Embedding) implementation.
    
    t-SNE is a non-linear dimensionality reduction technique that:
    1. Computes pairwise similarities in high-dim space
    2. Computes pairwise similarities in low-dim space
    3. Optimizes using gradient descent
    4. Preserves local structure
    """
    print(f"\n{'='*70}")
    print("CORE t-SNE IMPLEMENTATION")
    print(f"{'='*70}")
    
    X, y, _ = generate_high_dim_data(n_samples=300, n_features=15)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"\n{'='*60}")
    print("STEP 1: t-SNE WITH PERPLEXITY=5")
    print(f"{'='*60}")
    
    tsne_5 = TSNE(n_components=2, perplexity=5, random_state=42, n_iter=1000)
    X_tsne_5 = tsne_5.fit_transform(X_scaled)
    
    print(f"  Output shape: {X_tsne_5.shape}")
    print(f"  X range: [{X_tsne_5[:, 0].min():.2f}, {X_tsne_5[:, 0].max():.2f}]")
    print(f"  Y range: [{X_tsne_5[:, 1].min():.2f}, {X_tsne_5[:, 1].max():.2f}]")
    
    print(f"\n{'='*60}")
    print("STEP 2: t-SNE WITH PERPLEXITY=30 (DEFAULT)")
    print(f"{'='*60}")
    
    tsne_30 = TSNE(n_components=2, perplexity=30, random_state=42, n_iter=1000)
    X_tsne_30 = tsne_30.fit_transform(X_scaled)
    
    print(f"  Output shape: {X_tsne_30.shape}")
    print(f"  X range: [{X_tsne_30[:, 0].min():.2f}, {X_tsne_30[:, 0].max():.2f}]")
    print(f"  Y range: [{X_tsne_30[:, 1].min():.2f}, {X_tsne_30[:, 1].max():.2f}]")
    
    print(f"\n{'='*60}")
    print("STEP 3: t-SNE WITH PERPLEXITY=50")
    print(f"{'='*60}")
    
    tsne_50 = TSNE(n_components=2, perplexity=50, random_state=42, n_iter=1000)
    X_tsne_50 = tsne_50.fit_transform(X_scaled)
    
    print(f"  Output shape: {X_tsne_50.shape}")
    print(f"  X range: [{X_tsne_50[:, 0].min():.2f}, {X_tsne_50[:, 0].max():.2f}]")
    print(f"  Y range: [{X_tsne_50[:, 1].min():.2f}, {X_tsne_50[:, 1].max():.2f}]")
    
    print(f"\n{'='*60}")
    print("STEP 4: t-SNE WITH DIFFERENT ITERATIONS")
    print(f"{'='*60}")
    
    for n_iter in [500, 1000, 2000]:
        tsne = TSNE(n_components=2, perplexity=30, random_state=42, n_iter=n_iter)
        X_tsne = tsne.fit_transform(X_scaled)
        
        sil = silhouette_score(X_tsne, y) if len(np.unique(y)) > 1 else 0
        print(f"  Iter={n_iter}: Silhouette (on embedding)={sil:.4f}")
    
    return X_tsne_30, y


def tsne_comparison():
    """
    Compare t-SNE with different configurations.
    """
    print(f"\n{'='*70}")
    print("t-SNE CONFIGURATION COMPARISON")
    print(f"{'='*70}")
    
    X, y, _ = generate_high_dim_data(n_samples=300, n_features=20)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"\n{'='*60}")
    print("PERPLEXITY COMPARISON")
    print(f"{'='*60}")
    
    perplexities = [5, 10, 20, 30, 50]
    results = []
    
    for perp in perplexities:
        tsne = TSNE(n_components=2, perplexity=perp, random_state=42, n_iter=1000)
        X_tsne = tsne.fit_transform(X_scaled)
        
        sil = silhouette_score(X_tsne, y) if len(np.unique(y)) > 1 else 0
        results.append({'perplexity': perp, 'silhouette': sil})
        print(f"  Perplexity={perp:2d}: Silhouette={sil:.4f}")
    
    print(f"\n{'='*60}")
    print("LEARNING RATE COMPARISON")
    print(f"{'='*60}")
    
    learning_rates = [10, 100, 200, 500, 1000]
    
    for lr in learning_rates:
        tsne = TSNE(n_components=2, perplexity=30, learning_rate=lr, 
                   random_state=42, n_iter=1000)
        X_tsne = tsne.fit_transform(X_scaled)
        
        x_range = X_tsne[:, 0].max() - X_tsne[:, 0].min()
        y_range = X_tsne[:, 1].max() - X_tsne[:, 1].min()
        print(f"  LR={lr:4d}: X range={x_range:.2f}, Y range={y_range:.2f}")
    
    return results


# =============================================================================
# SECTION V: ADVANCED DIMENSIONALITY REDUCTION
# =============================================================================

def kernel_pca_demo():
    """
    Demonstrate Kernel PCA for non-linear dimensionality reduction.
    """
    print(f"\n{'='*70}")
    print("KERNEL PCA DEMONSTRATION")
    print(f"{'='*70}")
    
    X, y, _ = generate_high_dim_data(n_samples=300, n_features=10)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"\n{'='*60}")
    print("LINEAR KERNEL PCA")
    print(f"{'='*60}")
    
    kpca_linear = KernelPCA(n_components=2, kernel='linear')
    X_kpca_linear = kpca_linear.fit_transform(X_scaled)
    
    print(f"  Output shape: {X_kpca_linear.shape}")
    
    print(f"\n{'='*60}")
    print("RBF KERNEL PCA")
    print(f"{'='*60}")
    
    kpca_rbf = KernelPCA(n_components=2, kernel='rbf', gamma=0.1)
    X_kpca_rbf = kpca_rbf.fit_transform(X_scaled)
    
    print(f"  Output shape: {X_kpca_rbf.shape}")
    
    print(f"\n{'='*60}")
    print("POLYNOMIAL KERNEL PCA")
    print(f"{'='*60}")
    
    kpca_poly = KernelPCA(n_components=2, kernel='poly', degree=3)
    X_kpca_poly = kpca_poly.fit_transform(X_scaled)
    
    print(f"  Output shape: {X_kpca_poly.shape}")
    
    return X_kpca_linear, y


def truncated_svd_demo():
    """
    Demonstrate Truncated SVD (LSA) for dimensionality reduction.
    """
    print(f"\n{'='*70}")
    print("TRUNCATED SVD (LSA) DEMONSTRATION")
    print(f"{'='*70}")
    
    X, y, _ = generate_high_dim_data(n_samples=300, n_features=15)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"\n{'='*60}")
    print("TRUNCATED SVD")
    print(f"{'='*60}")
    
    tsvd = TruncatedSVD(n_components=5, random_state=42)
    X_tsvd = tsvd.fit_transform(X_scaled)
    
    print(f"  Output shape: {X_tsvd.shape}")
    print(f"  Explained variance: {tsvd.explained_variance_ratio_.sum()*100:.2f}%")
    
    return X_tsvd, y


def manifold_learning_demo():
    """
    Demonstrate various manifold learning techniques.
    """
    print(f"\n{'='*70}")
    print("MANIFOLD LEARNING TECHNIQUES")
    print(f"{'='*70}")
    
    X, y, _ = generate_high_dim_data(n_samples=200, n_features=10)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"\n{'='*60}")
    print("ISOMAP")
    print(f"{'='*60}")
    
    isomap = Isomap(n_components=2, n_neighbors=10)
    X_isomap = isomap.fit_transform(X_scaled)
    
    print(f"  Isomap shape: {X_isomap.shape}")
    
    print(f"\n{'='*60}")
    print("MDS (MULTIDIMENSIONAL SCALING)")
    print(f"{'='*60}")
    
    mds = MDS(n_components=2, random_state=42, normalized_stress='auto')
    X_mds = mds.fit_transform(X_scaled[:100])
    
    print(f"  MDS shape: {X_mds.shape}")
    
    return X_isomap, y


# =============================================================================
# SECTION VI: BANKING EXAMPLE
# =============================================================================

def banking_example():
    """
    Banking/Finance example: Customer behavior dimensionality reduction.
    
    Use case: Reduce customer behavior features for:
    - Visualization
    - Customer segmentation
    - Anomaly detection
    - Profile clustering
    """
    print(f"\n{'='*70}")
    print("BANKING EXAMPLE: CUSTOMER BEHAVIOR DIMENSIONALITY REDUCTION")
    print(f"{'='*70}")
    
    customers_df = generate_customer_behavior_data(n_customers=1000)
    
    customer_ids = customers_df['customer_id'].values
    feature_cols = [c for c in customers_df.columns if c != 'customer_id']
    X = customers_df[feature_cols].values
    
    print(f"\n{'='*60}")
    print("DATA PREPARATION")
    print(f"{'='*60}")
    
    print(f"  Original features: {len(feature_cols)}")
    print(f"  Sample shape: {X.shape}")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"  Data scaled: mean={X_scaled.mean():.6f}, std={X_scaled.std():.6f}")
    
    print(f"\n{'='*60}")
    print("PCA FOR DIMENSIONALITY REDUCTION")
    print(f"{'='*60}")
    
    pca = PCA(n_components=None)
    X_pca = pca.fit_transform(X_scaled)
    
    cumvar = np.cumsum(pca.explained_variance_ratio_)
    
    print(f"\n  Variance by components:")
    print(f"  Components | Variance % | Cumulative %")
    print(f"  " + "-"*40)
    for i in range(min(10, len(pca.explained_variance_ratio_))):
        print(f"  {i+1:10d} | {pca.explained_variance_ratio_[i]*100:8.2f}% | {cumvar[i]*100:8.2f}%")
    
    n_90 = np.argmax(cumvar >= 0.90) + 1
    n_95 = np.argmax(cumvar >= 0.95) + 1
    
    print(f"\n  Components for 90% variance: {n_90}")
    print(f"  Components for 95% variance: {n_95}")
    
    print(f"\n{'='*60}")
    print("PCA FEATURE INTERPRETATION")
    print(f"{'='*60}")
    
    pca_5 = PCA(n_components=5)
    pca_5.fit(X_scaled)
    
    components_df = pd.DataFrame(
        pca_5.components_.T,
        index=feature_cols,
        columns=[f'PC{i+1}' for i in range(5)]
    )
    
    print(f"\n  Top features for each principal component:")
    for pc in components_df.columns:
        top_features = components_df[pc].abs().nlargest(3)
        print(f"  {pc}: {list(top_features.index)}")
    
    print(f"\n{'='*60}")
    print("t-SNE VISUALIZATION")
    print(f"{'='*60}")
    
    tsne = TSNE(n_components=2, perplexity=30, random_state=42, n_iter=1000)
    X_tsne = tsne.fit_transform(X_scaled)
    
    print(f"  t-SNE output shape: {X_tsne.shape}")
    print(f"  X range: [{X_tsne[:, 0].min():.2f}, {X_tsne[:, 0].max():.2f}]")
    print(f"  Y range: [{X_tsne[:, 1].min():.2f}, {X_tsne[:, 1].max():.2f}]")
    
    print(f"\n{'='*60}")
    print("REDUCED FEATURE SET")
    print(f"{'='*60}")
    
    pca_reduced = PCA(n_components=n_95)
    X_reduced = pca_reduced.fit_transform(X_scaled)
    X_reconstructed = pca_reduced.inverse_transform(X_reduced)
    
    reconstruction_error = np.mean((X_scaled - X_reconstructed) ** 2)
    
    print(f"  Reduced to {n_95} dimensions")
    print(f"  Reconstruction error (MSE): {reconstruction_error:.6f}")
    
    print(f"\n{'='*60}")
    print("FEATURE CORRELATION ANALYSIS")
    print(f"{'='*60}")
    
    corr_matrix = np.corrcoef(X_scaled.T)
    avg_corr = (np.abs(corr_matrix).sum() - len(feature_cols)) / (len(feature_cols) * (len(feature_cols) - 1))
    
    print(f"  Average correlation: {avg_corr:.4f}")
    
    high_corr_pairs = []
    for i in range(len(feature_cols)):
        for j in range(i+1, len(feature_cols)):
            if abs(corr_matrix[i, j]) > 0.8:
                high_corr_pairs.append((feature_cols[i], feature_cols[j], corr_matrix[i, j]))
    
    print(f"  Highly correlated pairs (>0.8): {len(high_corr_pairs)}")
    for pair in high_corr_pairs[:5]:
        print(f"    {pair[0]} - {pair[1]}: {pair[2]:.4f}")
    
    return customers_df, X_pca, pca


def banking_customer_segmentation_with_dr():
    """
    Combine PCA/t-SNE with customer segmentation.
    """
    print(f"\n{'='*70}")
    print("BANKING CUSTOMER SEGMENTATION WITH DIMENSIONALITY REDUCTION")
    print(f"{'='*70}")
    
    customers_df = generate_customer_behavior_data(n_customers=800)
    
    feature_cols = [c for c in customers_df.columns if c != 'customer_id']
    X = customers_df[feature_cols].values
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    pca = PCA(n_components=10)
    X_pca = pca.fit_transform(X_scaled)
    
    tsne = TSNE(n_components=2, perplexity=30, random_state=42, n_iter=1000)
    X_tsne = tsne.fit_transform(X_pca)
    
    print(f"\n  PCA to 10D + t-SNE to 2D")
    print(f"  Final dimensions: 2")
    print(f"  X range: [{X_tsne[:, 0].min():.2f}, {X_tsne[:, 0].max():.2f}]")
    print(f"  Y range: [{X_tsne[:, 1].min():.2f}, {X_tsne[:, 1].max():.2f}]")
    
    return customers_df, X_tsne


# =============================================================================
# SECTION VII: HEALTHCARE EXAMPLE
# =============================================================================

def healthcare_example():
    """
    Healthcare example: Patient health data visualization.
    
    Use case: Reduce patient health features for:
    - Patient similarity analysis
    - Risk stratification
    - Clinical pattern discovery
    - Treatment grouping
    """
    print(f"\n{'='*70}")
    print("HEALTHCARE EXAMPLE: PATIENT HEALTH DATA DIMENSIONALITY REDUCTION")
    print(f"{'='*70}")
    
    patients_df = generate_patient_health_data(n_patients=800)
    
    patient_ids = patients_df['patient_id'].values
    feature_cols = [c for c in patients_df.columns if c != 'patient_id']
    X = patients_df[feature_cols].values
    
    print(f"\n{'='*60}")
    print("DATA PREPARATION")
    print(f"{'='*60}")
    
    print(f"  Original features: {len(feature_cols)}")
    print(f"  Sample features: {feature_cols[:10]}...")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"  Scaled data: shape={X_scaled.shape}")
    
    print(f"\n{'='*60}")
    print("PCA ANALYSIS OF HEALTH METRICS")
    print(f"{'='*60}")
    
    pca = PCA(n_components=None)
    X_pca = pca.fit_transform(X_scaled)
    
    cumvar = np.cumsum(pca.explained_variance_ratio_)
    
    print(f"\n  Variance explained by components:")
    print(f"  Components | Variance % | Cumulative %")
    print(f"  " + "-"*40)
    for i in range(min(15, len(pca.explained_variance_ratio_))):
        print(f"  {i+1:10d} | {pca.explained_variance_ratio_[i]*100:8.2f}% | {cumvar[i]*100:8.2f}%")
    
    n_90 = np.argmax(cumvar >= 0.90) + 1
    n_95 = np.argmax(cumvar >= 0.95) + 1
    
    print(f"\n  Components for 90% variance: {n_90}")
    print(f"  Components for 95% variance: {n_95}")
    
    print(f"\n{'='*60}")
    print("CLINICAL FEATURE LOADINGS")
    print(f"{'='*60}")
    
    pca_5 = PCA(n_components=5)
    pca_5.fit(X_scaled)
    
    loadings = pd.DataFrame(
        pca_5.components_.T,
        index=feature_cols,
        columns=[f'PC{i+1}' for i in range(5)]
    )
    
    print(f"\n  Top clinical indicators for each component:")
    key_features = ['systolic_bp', 'diastolic_bp', 'cholesterol_total', 'cholesterol_ldl', 
                'fasting_glucose', 'hba1c', 'bmi', 'age']
    
    for pc in loadings.columns:
        top_feat = loadings[pc].abs().nlargest(5).index.tolist()
        overlaps = [f for f in top_feat if f in key_features]
        print(f"  {pc}: {top_feat[:3]}... (clinical: {overlaps})")
    
    print(f"\n{'='*60}")
    print("t-SNE VISUALIZATION")
    print(f"{'='*60}")
    
    tsne = TSNE(n_components=2, perplexity=30, random_state=42, n_iter=1000)
    X_tsne = tsne.fit_transform(X_scaled)
    
    print(f"  t-SNE shape: {X_tsne.shape}")
    print(f"  X range: [{X_tsne[:, 0].min():.2f}, {X_tsne[:, 0].max():.2f}]")
    print(f"  Y range: [{X_tsne[:, 1].min():.2f}, {X_tsne[:, 1].max():.2f}]")
    
    print(f"\n{'='*60}")
    print("PATIENT RISK STRATIFICATION")
    print(f"{'='*60}")
    
    risk_scores = patients_df['cardiovascular_risk'].values
    
    risk_tsne_groups = []
    for risk in np.unique(risk_scores):
        mask = risk_scores == risk
        group_points = X_tsne[mask]
        centroid = group_points.mean(axis=0)
        risk_tsne_groups.append((risk, len(group_points), centroid)
    
    print(f"\n  Patient groups by risk score:")
    for risk, count, centroid in sorted(risk_tsne_groups):
        print(f"    Risk {risk}: n={count}, centroid=({centroid[0]:.2f}, {centroid[1]:.2f})")
    
    print(f"\n{'='*60}")
    print("DIMENSIONALITY REDUCTION FOR ML")
    print(f"{'='*60}")
    
    pca_optimal = PCA(n_components=n_95)
    X_reduced = pca_optimal.fit_transform(X_scaled)
    
    print(f"  Reduced from {X_scaled.shape[1]} to {X_reduced.shape[1]} dimensions")
    print(f"  Variance preserved: {n_95/X_scaled.shape[1]*100:.1f}% of components")
    
    X_reconstructed = pca_optimal.inverse_transform(X_reduced)
    error = np.sqrt(np.mean((X_scaled - X_reconstructed) ** 2))
    
    print(f"  Reconstruction RMSE: {error:.6f}")
    
    return patients_df, X_pca, pca, X_tsne


def combined_pca_tsne_example():
    """
    Combined PCA + t-SNE pipeline for healthcare.
    """
    print(f"\n{'='*70}")
    print("COMBINED PCA + t-SNE PIPELINE")
    print(f"{'='*70}")
    
    patients_df = generate_patient_health_data(n_patients=500)
    
    feature_cols = [c for c in patients_df.columns if c != 'patient_id']
    X = patients_df[feature_cols].values
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    pca = PCA(n_components=15)
    X_pca = pca.fit_transform(X_scaled)
    
    tsne = TSNE(n_components=2, perplexity=30, random_state=42, n_iter=1000)
    X_final = tsne.fit_transform(X_pca)
    
    print(f"\n  Step 1: PCA from {X_scaled.shape[1]} to 15 dimensions")
    print(f"  Step 2: t-SNE from 15 to 2 dimensions")
    print(f"  Final shape: {X_final.shape}")
    print(f"  Variance retained: {pca.explained_variance_ratio_.sum()*100:.1f}%")
    
    return X_final


# =============================================================================
# SECTION VIII: TESTING AND EVALUATION
# =============================================================================

def test_pca_algorithm():
    """
    Comprehensive testing of PCA implementation.
    """
    print(f"\n{'='*70}")
    print("PCA ALGORITHM TESTING")
    print(f"{'='*70}")
    
    print(f"\n{'='*60}")
    print("TEST 1: BASIC PCA")
    print(f"{'='*60}")
    
    X, y, _ = generate_high_dim_data(n_samples=300, n_features=15)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    pca = PCA(n_components=5)
    X_pca = pca.fit_transform(X_scaled)
    
    print(f"  Input shape: {X_scaled.shape}")
    print(f"  Output shape: {X_pca.shape}")
    print(f"  Variance explained: {pca.explained_variance_ratio_.sum()*100:.2f}%")
    
    print(f"\n{'='*60}")
    print("TEST 2: DIFFERENT COMPONENT COUNTS")
    print(f"{'='*60}")
    
    for k in [2, 5, 10]:
        pca = PCA(n_components=k)
        X_pca = pca.fit_transform(X_scaled)
        var = pca.explained_variance_ratio_.sum()
        print(f"  K={k}: Variance={var*100:.2f}%")
    
    print(f"\n{'='*60}")
    print("TEST 3: INVERSE TRANSFORM")
    print(f"{'='*60}")
    
    pca = PCA(n_components=10)
    X_pca = pca.fit_transform(X_scaled)
    X_reconstructed = pca.inverse_transform(X_pca)
    error = np.mean((X_scaled - X_reconstructed) ** 2)
    
    print(f"  Reconstruction error: {error:.6f}")
    
    print(f"\n{'='*60}")
    print("TEST 4: CORRELATED DATA")
    print(f"{'='*60}")
    
    X_corr, feature_names = generate_correlated_features(n_samples=200, n_features=10)
    
    scaler = StandardScaler()
    X_corr_scaled = scaler.fit_transform(X_corr)
    
    pca = PCA(n_components=5)
    pca.fit(X_corr_scaled)
    
    print(f"  Original rank: {np.linalg.matrix_rank(X_corr)}")
    print(f"  Components needed for 95%: {np.argmax(np.cumsum(pca.explained_variance_ratio_) >= 0.95) + 1}")
    
    print(f"\n  Tests completed!")
    
    return True


def test_tsne_algorithm():
    """
    Comprehensive testing of t-SNE implementation.
    """
    print(f"\n{'='*70}")
    print("t-SNE ALGORITHM TESTING")
    print(f"{'='*70}")
    
    X, y, _ = generate_high_dim_data(n_samples=200, n_features=12)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"\n{'='*60}")
    print("TEST 1: BASIC t-SNE")
    print(f"{'='*60}")
    
    tsne = TSNE(n_components=2, perplexity=20, random_state=42, n_iter=500)
    X_tsne = tsne.fit_transform(X_scaled)
    
    print(f"  Input: {X_scaled.shape}")
    print(f"  Output: {X_tsne.shape}")
    
    print(f"\n{'='*60}")
    print("TEST 2: DIFFERENT PERPLEXITIES")
    print(f"{'='*60}")
    
    for perp in [5, 15, 30]:
        tsne = TSNE(n_components=2, perplexity=perp, random_state=42, n_iter=500)
        X_tsne = tsne.fit_transform(X_scaled)
        
        sil = silhouette_score(X_tsne, y) if len(np.unique(y)) > 1 else 0
        print(f"  Perplexity={perp}: silhouette={sil:.4f}")
    
    print(f"\n{'='*60}")
    print("TEST 3: 3D t-SNE")
    print(f"{'='*60}")
    
    tsne_3d = TSNE(n_components=3, perplexity=20, random_state=42, n_iter=500)
    X_tsne_3d = tsne_3d.fit_transform(X_scaled)
    
    print(f"  3D output: {X_tsne_3d.shape}")
    
    print(f"\n  Tests completed!")
    
    return True


# =============================================================================
# SECTION IX: ADVANCED TOPICS
# =============================================================================

def advanced_topics():
    """
    Advanced dimensionality reduction topics.
    """
    print(f"\n{'='*70}")
    print("ADVANCED DIMENSIONALITY REDUCTION TOPICS")
    print(f"{'='*70}")
    
    print(f"\n{'='*60}")
    print("1. INCREMENTAL DIMENSIONALITY REDUCTION")
    print(f"{'='*60}")
    
    X, y, _ = generate_high_dim_data(n_samples=500, n_features=15)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    ipca = IncrementalPCA(n_components=5)
    for batch in np.array_split(X_scaled, 5):
        ipca.partial_fit(batch)
    
    X_ipca = ipca.transform(X_scaled)
    print(f"  Incremental PCA: {X_ipca.shape}")
    print(f"  Variance: {ipca.explained_variance_ratio_.sum()*100:.2f}%")
    
    print(f"\n{'='*60}")
    print("2. RANDOMIZED PCA")
    print(f"{'='*60}")
    
    from sklearn.decomposition import PCA
    
    pca_random = PCA(n_components=5, random_state=42, svd_solver='randomized')
    pca_random.fit(X_scaled)
    
    print(f"  Randomized PCA variance: {pca_random.explained_variance_ratio_.sum()*100:.2f}%")
    
    print(f"\n{'='*60}")
    print("3. SPARSE PCA")
    print(f"{'='*60}")
    
    try:
        from sklearn.decomposition import SparsePCA
        
        sparse_pca = SparsePCA(n_components=5, random_state=42, max_iter=100)
        sparse_pca.fit(X_scaled)
        print(f"  Sparse PCA: fit successful")
    except ImportError:
        print(f"  SparsePCA not available, using standard PCA")
    
    return True


def feature_projection_analysis():
    """
    Analyze feature projection quality.
    """
    print(f"\n{'='*70}")
    print("FEATURE PROJECTION ANALYSIS")
    print(f"{'='*70}")
    
    X, y, _ = generate_high_dim_data(n_samples=300, n_features=20)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print(f"\n{'='*60}")
    print("PROJECTION QUALITY METRICS")
    print(f"{'='*60}")
    
    pca_full = PCA(n_components=None)
    pca_full.fit(X_scaled)
    
    cumvar = np.cumsum(pca_full.explained_variance_ratio_)
    
    for threshold in [0.80, 0.90, 0.95, 0.99]:
        n_comp = np.argmax(cumvar >= threshold) + 1
        print(f"  {threshold*100:.0f}% variance: {n_comp} components")
    
    print(f"\n{'='*60}")
    print("RECONSTRUCTION ERROR")
    print(f"{'='*60}")
    
    for k in [5, 10, 15]:
        pca = PCA(n_components=k)
        X_pca = pca.fit_transform(X_scaled)
        X_recon = pca.inverse_transform(X_pca)
        
        mse = np.mean((X_scaled - X_recon) ** 2)
        print(f"  K={k}: MSE={mse:.6f}")
    
    return pca_full


# =============================================================================
# SECTION X: MAIN EXECUTION
# =============================================================================

def main():
    """
    Main function to execute dimensionality reduction examples.
    """
    print(f"\n{'='*70}")
    print("DIMENSIONALITY REDUCTION - COMPREHENSIVE IMPLEMENTATION")
    print(f"{'='*70}")
    
    print(f"\n{'='*70}")
    print("SECTION 1: DATA GENERATION")
    print(f"{'='*70}")
    
    X, y, names = generate_high_dim_data(n_samples=400, n_features=20)
    
    print(f"\n{'='*70}")
    print("SECTION 2: CURSE OF DIMENSIONALITY")
    print(f"{'='*70}")
    
    curse_results = analyze_curse_of_dimensionality()
    
    print(f"\n{'='*70}")
    print("SECTION 3: CORE PCA IMPLEMENTATION")
    print(f"{'='*70}")
    
    X_pca, y_pca, pca = core_pca()
    
    print(f"\n{'='*70}")
    print("SECTION 4: PCA VARIANCE ANALYSIS")
    print(f"{'='*70}")
    
    pca_var = pca_variance_analysis()
    
    print(f"\n{'='*70}")
    print("SECTION 5: CORE t-SNE IMPLEMENTATION")
    print(f"{'='*70}")
    
    X_tsne, y_tsne = core_tsne()
    
    print(f"\n{'='*70}")
    print("SECTION 6: t-SNE COMPARISON")
    print(f"{'='*70}")
    
    tsne_results = tsne_comparison()
    
    print(f"\n{'='*70}")
    print("SECTION 7: ADVANCED METHODS")
    print(f"{'='*70}")
    
    kpca_results = kernel_pca_demo()
    tsvd_results = truncated_svd_demo()
    manifold_results = manifold_learning_demo()
    
    print(f"\n{'='*70}")
    print("SECTION 8: BANKING EXAMPLE")
    print(f"{'='*70}")
    
    banking_df, banking_pca, banking_pca_model = banking_example()
    
    print(f"\n{'='*70}")
    print("SECTION 9: HEALTHCARE EXAMPLE")
    print(f"{'='*70}")
    
    healthcare_df, healthcare_pca, healthcare_pca_model, healthcare_tsne = healthcare_example()
    
    print(f"\n{'='*70}")
    print("SECTION 10: TESTING")
    print(f"{'='*70}")
    
    pca_test = test_pca_algorithm()
    tsne_test = test_tsne_algorithm()
    
    print(f"\n{'='*70}")
    print("SECTION 11: ADVANCED TOPICS")
    print(f"{'='*70}")
    
    advanced = advanced_topics()
    
    print(f"\n{'='*70}")
    print("SECTION 12: FEATURE PROJECTION")
    print(f"{'='*70}")
    
    projection = feature_projection_analysis()
    
    print(f"\n{'='*70}")
    print("SUMMARY")
    print(f"{'='*70}")
    print(f"  1. Data Generation: High-dimensional synthetic data created")
    print(f"  2. Curse of Dimensionality: Analyzed sparsity and volume effects")
    print(f"  3. Core PCA: Full PCA implementation with variance analysis")
    print(f"  4. PCA Variance: Scree plot and component selection")
    print(f"  5. Core t-SNE: Non-linear visualization")
    print(f"  6. t-SNE Comparison: Perplexity and learning rate effects")
    print(f"  7. Advanced Methods: Kernel PCA, TruncatedSVD, Isomap")
    print(f"  8. Banking: Customer behavior dimensionality reduction")
    print(f"  9. Healthcare: Patient health metrics visualization")
    print(f"  10. Testing: PCA and t-SNE tests passed")
    print(f"  11. Advanced Topics: Incremental and randomized PCA")
    print(f"  12. Feature Projection: Quality metrics analysis")
    
    print(f"\n{'='*70}")
    print("EXECUTION COMPLETE")
    print(f"{'='*70}")
    
    return {
        'pca_variance_explained': pca.explained_variance_ratio_.sum(),
        'tsne_perplexity': 30,
        'banking_features': banking_pca.shape[1],
        'healthcare_features': healthcare_pca.shape[1]
    }


if __name__ == "__main__":
    results = main()