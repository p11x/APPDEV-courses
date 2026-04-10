# Topic: Supervised vs Unsupervised Learning
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Supervised vs Unsupervised Learning

I. INTRODUCTION
This module covers the fundamental paradigms of machine learning, distinguishing between
supervised learning (learning with labeled data) and unsupervised learning
(discovering patterns in unlabeled data). Understanding these paradigms is essential
for selecting the appropriate approach for any machine learning problem.

II. CORE_CONCEPTS
- Supervised Learning: Learning from labeled data to predict outputs
- Unsupervised Learning: Discovering hidden patterns without labels
- Types: Classification (discrete) vs Regression (continuous) for supervised
- Types: Clustering, Dimensionality Reduction, Association for unsupervised

III. IMPLEMENTATION
- Data preparation for both paradigms
- Algorithm selection and training
- Evaluation methodologies

IV. EXAMPLES (Banking + Healthcare)
- Banking: Credit risk prediction (supervised), Customer segmentation (unsupervised)
- Healthcare: Disease diagnosis (supervised), Patient clustering (unsupervised)

V. OUTPUT_RESULTS
- Model performance metrics
- Cluster assignments and interpretations
- Predictions for new data points

VI. TESTING
- Unit tests for data processing
- Validation of model training
- Evaluation against known outcomes

VII. ADVANCED_TOPICS
- Semi-supervised and self-supervised learning
- Transfer learning applications
- Multi-task learning

VIII. CONCLUSION
- Choosing the right paradigm for your problem
- Combining supervised and unsupervised approaches
"""

import numpy as np
import pandas as pd
from sklearn.datasets import make_blobs, make_regression, make_classification
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LinearRegression, LogisticRegression
from sklearn.cluster import KMeans
from sklearn.metrics import mean_squared_error, accuracy_score, silhouette_score
import warnings
warnings.filterwarnings('ignore')


def generate_supervised_regression_data(n_samples=500, n_features=5, noise=10):
    """
    Generate synthetic dataset for supervised regression learning.
    
    Args:
        n_samples: Number of data points
        n_features: Number of features
        noise: Noise level in target variable
        
    Returns:
        X: Feature matrix
        y: Target variable
    """
    np.random.seed(42)
    
    if n_features == 5:
        sqft = np.random.uniform(800, 3500, n_samples)
        bedrooms = np.random.randint(1, 6, n_samples)
        bathrooms = np.random.randint(1, 5, n_samples)
        age = np.random.uniform(0, 50, n_samples)
        distance = np.random.uniform(1, 30, n_samples)
        X = np.column_stack([sqft, bedrooms, bathrooms, age, distance])
        y = (50000 + 150 * sqft + 10000 * bedrooms + 5000 * bathrooms 
             - 500 * age - 1000 * distance + np.random.normal(0, noise, n_samples))
    
    return X, y


def generate_supervised_classification_data(n_samples=500, n_features=4, n_classes=2):
    """
    Generate synthetic dataset for supervised classification learning.
    
    Args:
        n_samples: Number of data points
        n_features: Number of features
        n_classes: Number of classes
        
    Returns:
        X: Feature matrix
        y: Target variable
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_classes=n_classes,
        n_informative=3,
        n_redundant=1,
        random_state=42
    )
    return X, y


def generate_unsupervised_clustering_data(n_samples=300, n_features=3, n_clusters=3):
    """
    Generate synthetic dataset for unsupervised clustering.
    
    Args:
        n_samples: Number of data points
        n_features: Number of features
        n_clusters: Number of clusters
        
    Returns:
        X: Feature matrix
    """
    X, _ = make_blobs(
        n_samples=n_samples,
        n_features=n_features,
        centers=n_clusters,
        cluster_std=[5, 8, 6],
        random_state=42
    )
    return X


def core_supervised_regression():
    """
    Core implementation for supervised regression learning.
    Demonstrates training a model on labeled data to predict continuous values.
    """
    print("\n" + "="*60)
    print("SUPERVISED REGRESSION IMPLEMENTATION")
    print("="*60)
    
    X, y = generate_supervised_regression_data(n_samples=500)
    print(f"\nDataset shape: {X.shape}")
    print(f"Target range: ${y.min():,.2f} - ${y.max():,.2f}")
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    print(f"Training samples: {len(X_train)}")
    print(f"Test samples: {len(X_test)}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LinearRegression()
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    mse = mean_squared_error(y_test, y_pred)
    rmse = np.sqrt(mse)
    
    print(f"\nModel Performance:")
    print(f"  Mean Squared Error: ${mse:,.2f}")
    print(f"  Root Mean Squared Error: ${rmse:,.2f}")
    print(f"  R² Score: {model.score(X_test_scaled, y_test):.4f}")
    print(f"  Coefficients: {model.coef_}")
    
    return model, scaler


def core_supervised_classification():
    """
    Core implementation for supervised classification learning.
    Demonstrates training a model on labeled data to predict discrete classes.
    """
    print("\n" + "="*60)
    print("SUPERVISED CLASSIFICATION IMPLEMENTATION")
    print("="*60)
    
    X, y = generate_supervised_classification_data(n_samples=500)
    print(f"\nDataset shape: {X.shape}")
    print(f"Class distribution: {np.bincount(y)}")
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42)
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    accuracy = accuracy_score(y_test, y_pred)
    
    print(f"\nModel Performance:")
    print(f"  Accuracy: {accuracy:.4f}")
    print(f"  Predictions: {np.unique(y_pred, return_counts=True)}")
    
    return model, scaler


def core_unsupervised_clustering():
    """
    Core implementation for unsupervised clustering.
    Demonstrates grouping data without known labels.
    """
    print("\n" + "="*60)
    print("UNSUPERVISED CLUSTERING IMPLEMENTATION")
    print("="*60)
    
    X = generate_unsupervised_clustering_data(n_samples=300, n_clusters=3)
    print(f"\nDataset shape: {X.shape}")
    print("Note: No labels provided - discovering patterns")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    inertias = []
    silhouette_scores = []
    K_range = range(2, 8)
    
    for k in K_range:
        kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
        kmeans.fit(X_scaled)
        inertias.append(kmeans.inertia_)
        silhouette_scores.append(silhouette_score(X_scaled, kmeans.labels_))
    
    optimal_k = 3
    kmeans = KMeans(n_clusters=optimal_k, random_state=42, n_init=10)
    labels = kmeans.fit_predict(X_scaled)
    
    print(f"\nClustering Results (k={optimal_k}):")
    print(f"  Silhouette Score: {silhouette_scores[optimal_k-2]:.4f}")
    print(f"  Cluster sizes: {np.bincount(labels)}")
    
    for i in range(optimal_k):
        cluster_points = X[labels == i]
        print(f"  Cluster {i}: {len(cluster_points)} points, "
              f"centroid at {cluster_points.mean(axis=0)}")
    
    return kmeans, scaler, labels


def compare_paradigms():
    """
    Compare supervised and unsupervised learning approaches.
    """
    print("\n" + "="*60)
    print("COMPARISON: SUPERVISED vs UNSUPERVISED")
    print("="*60)
    
    comparison = {
        'Aspect': ['Data Required', 'Learning Type', 'Evaluation', 
                   'Use Cases', 'Algorithms', 'Complexity'],
        'Supervised': ['Labeled (X,y)', 'Predictive', 'vs ground truth',
                        'Prediction, Classification',
                        'Regression, Decision Trees, SVM', 'Moderate'],
        'Unsupervised': ['Unlabeled (X)', 'Exploratory', 'Heuristic/indirect',
                         'Clustering, Dimensionality Reduction',
                         'K-Means, PCA, DBSCAN', 'Lower']
    }
    
    df = pd.DataFrame(comparison)
    print("\n" + df.to_string(index=False))
    print("\n" + "-"*60)
    print("When to Use Each:")
    print("-"*60)
    print("SUPERVISED: When you have labels and need predictions")
    print("UNSUPERVISED: When labels unavailable, want discovery")


def banking_example():
    """
    Banking/Finance industry application.
    Demonstrates both supervised (credit risk) and unsupervised (customer segmentation).
    """
    print("\n" + "="*60)
    print("BANKING EXAMPLE: Credit Risk Assessment")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 1000
    
    features = {
        'income': np.random.lognormal(10.5, 0.8, n_samples),
        'credit_score': np.random.normal(680, 100, n_samples).clip(300, 850),
        'debt_to_income': np.random.beta(2, 5, n_samples) * 0.5,
        'employment_years': np.random.exponential(5, n_samples).clip(0, 40)
    }
    
    X = np.column_stack([features['income'], features['credit_score'],
                        features['debt_to_income'], features['employment_years']])
    
    risk_prob = (0.1 + 0.2 * (features['credit_score'] < 620).astype(int)
                 + 0.15 * (features['debt_to_income'] > 0.36).astype(int)
                 + 0.1 * (features['employment_years'] < 2).astype(int))
    y = (np.random.random(n_samples) < risk_prob.clip(0.02, 0.5)).astype(int)
    
    print(f"\nDataset: {n_samples} loan applications")
    print(f"Default rate: {y.mean()*100:.2f}%")
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42)
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    accuracy = accuracy_score(y_test, y_pred)
    
    print(f"\nModel Performance:")
    print(f"  Accuracy: {accuracy:.4f}")
    print(f"  High-risk predictions: {y_pred.sum()}")
    
    print("\n" + "-"*60)
    print("Banking Example: Customer Segmentation")
    print("-"*60)
    
    customer_features = np.random.multivariate_normal(
        [40, 60, 50], [[100, 50, 30], [50, 100, 40], [30, 40, 80]],
        n_samples
    )
    
    scaler = StandardScaler()
    customer_scaled = scaler.fit_transform(customer_features)
    
    kmeans = KMeans(n_clusters=3, random_state=42, n_init=10)
    segments = kmeans.fit_predict(customer_scaled)
    
    print(f"\nCustomer Segments:")
    for i in range(3):
        seg_points = customer_features[segments == i]
        print(f"  Segment {i}: {len(seg_points)} customers, "
              f"avg features: {seg_points.mean(axis=0).round(1)}")
    
    return model, scaler, kmeans, scaler


def healthcare_example():
    """
    Healthcare industry application.
    Demonstrates supervised diagnosis and unsupervised patient clustering.
    """
    print("\n" + "="*60)
    print("HEALTHCARE EXAMPLE: Disease Diagnosis")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 1000
    
    features = {
        'age': np.random.normal(55, 20, n_samples).clip(18, 100),
        'bmi': np.random.normal(27, 5, n_samples).clip(15, 50),
        'blood_pressure': np.random.normal(130, 20, n_samples).clip(80, 200),
        'cholesterol': np.random.normal(200, 40, n_samples).clip(100, 300)
    }
    
    X = np.column_stack([features['age'], features['bmi'],
                        features['blood_pressure'], features['cholesterol']])
    
    disease_prob = (0.1 + 0.15 * (features['age'] > 60).astype(int)
                    + 0.1 * (features['bmi'] > 30).astype(int)
                    + 0.12 * (features['blood_pressure'] > 140).astype(int)
                    + 0.08 * (features['cholesterol'] > 240).astype(int))
    y = (np.random.random(n_samples) < disease_prob.clip(0.05, 0.6)).astype(int)
    
    print(f"\nDataset: {n_samples} patient records")
    print(f"Disease prevalence: {y.mean()*100:.2f}%")
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42)
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    accuracy = accuracy_score(y_test, y_pred)
    
    print(f"\nModel Performance:")
    print(f"  Accuracy: {accuracy:.4f}")
    print(f"  Diagnoses made: {y_pred.sum()}")
    
    print("\n" + "-"*60)
    print("Healthcare Example: Patient Clustering")
    print("-"*60)
    
    clinical_measures = np.random.multivariate_normal(
        [70, 25, 120], [[150, 60, 80], [60, 80, 40], [80, 40, 200]],
        n_samples
    )
    
    scaler = StandardScaler()
    clinical_scaled = scaler.fit_transform(clinical_measures)
    
    kmeans = KMeans(n_clusters=4, random_state=42, n_init=10)
    clusters = kmeans.fit_predict(clinical_scaled)
    
    print(f"\nPatient Clusters:")
    for i in range(4):
        cluster_points = clinical_measures[clusters == i]
        print(f"  Cluster {i}: {len(cluster_points)} patients, "
              f"avg measures: {cluster_points.mean(axis=0).round(1)}")
    
    return model, scaler, kmeans, scaler


def compute_elbow_method(X, max_k=10):
    """
    Compute elbow method for determining optimal cluster count.
    """
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    inertias = []
    silhouette_scores = []
    
    for k in range(2, max_k + 1):
        kmeans = KMeans(n_clusters=k, random_state=42, n_init=10)
        kmeans.fit(X_scaled)
        inertias.append(kmeans.inertia_)
        silhouette_scores.append(silhouette_score(X_scaled, kmeans.labels_))
    
    return inertias, silhouette_scores


def validate_supervised_model(X_train, y_train, X_test, y_test, model):
    """
    Validate supervised learning model performance.
    """
    predictions = model.predict(X_test)
    accuracy = accuracy_score(y_test, predictions)
    mse = mean_squared_error(y_test, predictions)
    
    return {
        'accuracy': accuracy,
        'mse': mse,
        'predictions': predictions,
        'actual': y_test
    }


def validate_unsupervised_model(X, labels, model):
    """
    Validate unsupervised learning model quality.
    """
    silhouette = silhouette_score(X, labels)
    inertia = model.inertia_
    
    unique_labels = np.unique(labels)
    cluster_sizes = [np.sum(labels == label) for label in unique_labels]
    
    return {
        'silhouette_score': silhouette,
        'inertia': inertia,
        'n_clusters': len(unique_labels),
        'cluster_sizes': cluster_sizes
    }


def main():
    """Main execution function."""
    print("="*60)
    print("SUPERVISED VS UNSUPERVISED LEARNING")
    print("="*60)
    
    core_supervised_regression()
    core_supervised_classification()
    core_unsupervised_clustering()
    compare_paradigms()
    banking_example()
    healthcare_example()
    
    print("\n" + "="*60)
    print("EXECUTION COMPLETE")
    print("="*60)


if __name__ == "__main__":
    main()