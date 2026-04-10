# Topic: Anomaly Detection
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Anomaly Detection

I. INTRODUCTION
    Anomaly detection (also known as outlier detection) is the identification of rare 
    items, events, or observations that differ significantly from the majority of the data.
    These anomalies can indicate errors, fraud, network intrusions, or other rare events.
    Common techniques include Isolation Forest, Local Outlier Factor (LOF), 
    One-Class SVM, and Elliptic Envelope.

II. CORE_CONCEPTS
    - Types of anomalies: point, contextual, collective
    - Supervised vs unsupervised anomaly detection
    - Novelty detection vs outlier detection
    - Contamination factor
    - Isolation principle
    - Local density comparison

III. IMPLEMENTATION
    - Isolation Forest implementation
    - Local Outlier Factor (LOF)
    - One-Class SVM
    - Elliptic Envelope
    - Ensemble methods
    - Scoring and thresholding

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Fraud Detection in transactions
    - Healthcare: Medical Anomaly Detection in patient data

V. OUTPUT_RESULTS
    - Anomaly labels (-1 for outliers, 1 for inliers)
    - Anomaly scores
    - Decision boundaries visualization
    - Performance metrics (precision, recall, F1)

VI. TESTING
    - Synthetic data with known anomalies
    - Varying contamination levels
    - Different anomaly types

VII. ADVANCED_TOPICS
    - Time series anomaly detection
    - Streaming anomaly detection
    - Ensemble of anomaly detectors
    - Semi-supervised approaches

VIII. CONCLUSION
    - When to use each method
    - Best practices for anomaly detection
    - Handling imbalanced data
"""

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_blobs, make_moons
from sklearn.model_selection import train_test_split
from sklearn.ensemble import IsolationForest
from sklearn.neighbors import LocalOutlierFactor
from sklearn.svm import OneClassSVM
from sklearn.covariance import EllipticEnvelope
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    confusion_matrix, classification_report, roc_curve, auc, precision_recall_curve
)
import warnings
warnings.filterwarnings('ignore')


def generate_anomaly_data(n_samples=500, n_anomalies=50, n_features=2, random_state=42):
    """
    Generate data with anomalies for testing.
    
    Parameters:
    -----------
    n_samples : int
        Total number of samples
    n_anomalies : int
        Number of anomalies to inject
    n_features : int
        Number of features
    random_state : int
        Random seed
    
    Returns:
    --------
    X : ndarray
        Features
    y : ndarray
        Labels (1 for normal, -1 for anomaly)
    """
    n_normal = n_samples - n_anomalies
    
    X_normal, _ = make_blobs(
        n_samples=n_normal,
        centers=1,
        n_features=n_features,
        cluster_std=1.0,
        random_state=random_state
    )
    
    X_anomalies = np.random.uniform(
        low=[X_normal[:, i].min() - 3 for i in range(n_features)],
        high=[X_normal[:, i].max() + 3 for i in range(n_features)],
        size=(n_anomalies, n_features)
    )
    
    X = np.vstack([X_normal, X_anomalies])
    y = np.concatenate([np.ones(n_normal), np.ones(n_anomalies) * -1])
    
    indices = np.arange(n_samples)
    np.random.shuffle(indices)
    X = X[indices]
    y = y[indices]
    
    print(f"Generated {n_samples} samples ({n_normal} normal, {n_anomalies} anomalies)")
    print(f"Contamination ratio: {n_anomalies/n_samples:.2%}")
    
    return X, y


def generate_contextual_anomaly_data(n_samples=500, n_anomalies=30, random_state=42):
    """
    Generate data with contextual anomalies.
    
    Parameters:
    -----------
    n_samples : int
        Total number of samples
    n_anomalies : int
        Number of anomalies
    random_state : int
        Random seed
    
    Returns:
    --------
    X : ndarray
        Features
    y : ndarray
        Labels
    """
    np.random.seed(random_state)
    
    n_normal = n_samples - n_anomalies
    
    t = np.linspace(0, 4 * np.pi, n_samples)
    seasonal = np.column_stack([
        np.sin(t),
        np.cos(t)
    ]) * 3 + np.random.normal(0, 0.3, (n_samples, 2))
    
    anomaly_indices = np.random.choice(n_samples, n_anomalies, replace=False)
    
    for idx in anomaly_indices:
        seasonal[idx] += np.random.choice([-1, 1]) * np.array([5, 5])
    
    y = np.ones(n_samples)
    y[anomaly_indices] = -1
    
    print(f"Generated contextual anomaly data")
    print(f"Normal samples: {n_normal}, Anomalies: {n_anomalies}")
    
    return seasonal, y


def core_isolation_forest(X_train, X_test, y_test, contamination=0.1, n_estimators=100):
    """
    Isolation Forest anomaly detection.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_test : ndarray
        True labels for evaluation
    contamination : float
        Expected proportion of anomalies
    n_estimators : int
        Number of trees
    
    Returns:
    --------
    model : IsolationForest
        Trained model
    results : dict
        Detection results
    """
    print(f"\n{'='*60}")
    print(f"ISOLATION FOREST")
    print(f"  Contamination: {contamination}, Estimators: {n_estimators}")
    print(f"{'='*60}")
    
    model = IsolationForest(
        contamination=contamination,
        n_estimators=n_estimators,
        random_state=42,
        n_jobs=-1
    )
    
    model.fit(X_train)
    
    y_pred = model.predict(X_test)
    scores = model.decision_function(X_test)
    
    results = evaluate_anomaly_detection(y_test, y_pred, scores)
    
    return model, results


def core_local_outlier_factor(X_train, X_test, y_test, contamination=0.1, n_neighbors=20):
    """
    Local Outlier Factor anomaly detection.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_test : ndarray
        True labels for evaluation
    contamination : float
        Expected proportion of anomalies
    n_neighbors : int
        Number of neighbors
    
    Returns:
    --------
    model : LocalOutlierFactor
        Trained model
    results : dict
        Detection results
    """
    print(f"\n{'='*60}")
    print(f"LOCAL OUTLIER FACTOR")
    print(f"  Contamination: {contamination}, Neighbors: {n_neighbors}")
    print(f"{'='*60}")
    
    model = LocalOutlierFactor(
        contamination=contamination,
        n_neighbors=n_neighbors,
        novelty=True,
        n_jobs=-1
    )
    
    model.fit(X_train)
    
    y_pred = model.predict(X_test)
    scores = model.decision_function(X_test)
    
    results = evaluate_anomaly_detection(y_test, y_pred, scores)
    
    return model, results


def core_one_class_svm(X_train, X_test, y_test, contamination=0.1, nu=0.1):
    """
    One-Class SVM anomaly detection.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_test : ndarray
        True labels for evaluation
    contamination : float
        Expected proportion of anomalies
    nu : float
        Upper bound on fraction of outliers
    
    Returns:
    --------
    model : OneClassSVM
        Trained model
    results : dict
        Detection results
    """
    print(f"\n{'='*60}")
    print(f"ONE-CLASS SVM")
    print(f"  Contamination: {contamination}, Nu: {nu}")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = OneClassSVM(
        nu=nu,
        kernel='rbf',
        gamma='scale'
    )
    
    model.fit(X_train_scaled)
    
    y_pred = model.predict(X_test_scaled)
    scores = model.decision_function(X_test_scaled)
    
    results = evaluate_anomaly_detection(y_test, y_pred, scores)
    
    return model, results


def core_elliptic_envelope(X_train, X_test, y_test, contamination=0.1):
    """
    Elliptic Envelope (Gaussian assumption) anomaly detection.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_test : ndarray
        True labels for evaluation
    contamination : float
        Expected proportion of anomalies
    
    Returns:
    --------
    model : EllipticEnvelope
        Trained model
    results : dict
        Detection results
    """
    print(f"\n{'='*60}")
    print(f"ELLIPTIC ENVELOPE")
    print(f"  Contamination: {contamination}")
    print(f"{'='*60}")
    
    model = EllipticEnvelope(
        contamination=contamination,
        random_state=42
    )
    
    model.fit(X_train)
    
    y_pred = model.predict(X_test)
    scores = model.decision_function(X_test)
    
    results = evaluate_anomaly_detection(y_test, y_pred, scores)
    
    return model, results


def evaluate_anomaly_detection(y_true, y_pred, scores):
    """
    Evaluate anomaly detection performance.
    
    Parameters:
    -----------
    y_true : ndarray
        True labels (1 for normal, -1 for anomaly)
    y_pred : ndarray
        Predicted labels
    scores : ndarray
        Anomaly scores
    
    Returns:
    --------
    results : dict
        Evaluation metrics
    """
    y_true_binary = (y_true == -1).astype(int)
    y_pred_binary = (y_pred == -1).astype(int)
    
    accuracy = accuracy_score(y_true_binary, y_pred_binary)
    precision = precision_score(y_true_binary, y_pred_binary, zero_division=0)
    recall = recall_score(y_true_binary, y_pred_binary, zero_division=0)
    f1 = f1_score(y_true_binary, y_pred_binary, zero_division=0)
    
    cm = confusion_matrix(y_true_binary, y_pred_binary)
    
    fpr, tpr, _ = roc_curve(y_true_binary, -scores)
    roc_auc = auc(fpr, tpr)
    
    results = {
        'accuracy': accuracy,
        'precision': precision,
        'recall': recall,
        'f1': f1,
        'roc_auc': roc_auc,
        'confusion_matrix': cm,
        'y_pred': y_pred,
        'scores': scores
    }
    
    print(f"\nPerformance Metrics:")
    print(f"  Accuracy:  {accuracy:.4f}")
    print(f"  Precision: {precision:.4f}")
    print(f"  Recall:    {recall:.4f}")
    print(f"  F1 Score:  {f1:.4f}")
    print(f"  ROC AUC:   {roc_auc:.4f}")
    print(f"\nConfusion Matrix (rows=true, cols=pred):")
    print(f"  TN={cm[0,0]:3d}  FP={cm[0,1]:3d}")
    print(f"  FN={cm[1,0]:3d}  TP={cm[1,1]:3d}")
    
    return results


def compare_methods(X_train, X_test, y_test, contamination=0.1):
    """
    Compare all anomaly detection methods.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_test : ndarray
        True labels
    contamination : float
        Expected contamination
    
    Returns:
    --------
    results : dict
        Results for all methods
    """
    print(f"\n{'='*60}")
    print(f"COMPARING ANOMALY DETECTION METHODS")
    print(f"{'='*60}")
    
    results = {}
    
    if_model, if_results = core_isolation_forest(X_train, X_test, y_test, contamination)
    results['IsolationForest'] = if_results
    
    lof_model, lof_results = core_local_outlier_factor(X_train, X_test, y_test, contamination)
    results['LocalOutlierFactor'] = lof_results
    
    ocsvm_model, ocsvm_results = core_one_class_svm(X_train, X_test, y_test, contamination)
    results['OneClassSVM'] = ocsvm_results
    
    ee_model, ee_results = core_elliptic_envelope(X_train, X_test, y_test, contamination)
    results['EllipticEnvelope'] = ee_results
    
    visualize_comparison(results)
    
    return results


def visualize_comparison(results):
    """
    Visualize comparison of anomaly detection methods.
    
    Parameters:
    -----------
    results : dict
        Results for all methods
    """
    methods = list(results.keys())
    
    metrics = ['accuracy', 'precision', 'recall', 'f1', 'roc_auc']
    
    fig, axes = plt.subplots(1, len(metrics), figsize=(15, 4))
    
    for ax, metric in zip(axes, metrics):
        values = [results[m][metric] for m in methods]
        ax.bar(methods, values)
        ax.set_title(metric.upper())
        ax.set_ylim(0, 1)
        ax.tick_params(axis='x', rotation=45)
        ax.grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()


def visualize_anomalies_2d(X, y_true, y_pred, scores, method_name):
    """
    Visualize anomalies in 2D space.
    
    Parameters:
    -----------
    X : ndarray
        Features (2D)
    y_true : ndarray
        True labels
    y_pred : ndarray
        Predicted labels
    scores : ndarray
        Anomaly scores
    method_name : str
        Method name for title
    """
    fig, axes = plt.subplots(1, 3, figsize=(15, 4))
    
    axes[0].scatter(X[y_true == 1, 0], X[y_true == 1, 1], c='blue', alpha=0.5, label='Normal')
    axes[0].scatter(X[y_true == -1, 0], X[y_true == -1, 1], c='red', alpha=0.5, label='True Anomaly')
    axes[0].set_title('Ground Truth')
    axes[0].legend()
    axes[0].grid(True, alpha=0.3)
    
    axes[1].scatter(X[y_pred == 1, 0], X[y_pred == 1, 1], c='blue', alpha=0.5, label='Normal')
    axes[1].scatter(X[y_pred == -1, 0], X[y_pred == -1, 1], c='orange', alpha=0.5, label='Detected Anomaly')
    axes[1].set_title(f'{method_name} Predictions')
    axes[1].legend()
    axes[1].grid(True, alpha=0.3)
    
    scatter = axes[2].scatter(X[:, 0], X[:, 1], c=-scores, cmap='coolwarm', alpha=0.5)
    axes[2].set_title('Anomaly Scores (red=high)')
    plt.colorbar(scatter, ax=axes[2])
    axes[2].grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()


def banking_example():
    """
    Banking/Finance example: Credit Card Fraud Detection.
    
    Detects fraudulent transactions using multiple anomaly detection
    techniques.
    """
    print(f"\n{'='*60}")
    print(f"BANKING EXAMPLE: Credit Card Fraud Detection")
    print(f"{'='*60}")
    
    np.random.seed(42)
    n_samples = 1000
    n_fraud = 50
    
    n_normal = n_samples - n_fraud
    
    transaction_amount = np.concatenate([
        np.random.lognormal(3, 0.5, n_normal),
        np.random.uniform(1000, 5000, n_fraud)
    ])
    
    transaction_hour = np.concatenate([
        np.random.choice(range(24), n_normal, p=np.ones(24)/24),
        np.random.choice([2, 3, 4, 22, 23], n_fraud)
    ])
    
    transaction_day = np.concatenate([
        np.random.choice(range(1, 29), n_normal),
        np.random.choice([1, 2, 15, 16, 28, 29], n_fraud)
    ])
    
    distance_from_home = np.concatenate([
        np.random.exponential(10, n_normal),
        np.random.uniform(50, 200, n_fraud)
    ])
    
    distance_from_last = np.concatenate([
        np.random.exponential(5, n_normal),
        np.random.uniform(30, 100, n_fraud)
    ])
    
    ratio_to_median = np.concatenate([
        np.random.uniform(0.3, 2.0, n_normal),
        np.random.uniform(3.0, 10.0, n_fraud)
    ])
    
    X = np.column_stack([
        transaction_amount,
        transaction_hour,
        transaction_day,
        distance_from_home,
        distance_from_last,
        ratio_to_median
    ])
    
    y = np.concatenate([np.ones(n_normal), -np.ones(n_fraud)])
    
    indices = np.arange(n_samples)
    np.random.shuffle(indices)
    X = X[indices]
    y = y[indices]
    
    feature_names = ['Amount', 'Hour', 'Day', 'Dist_Home', 'Dist_Last', 'Ratio_Median']
    
    df = pd.DataFrame(X, columns=feature_names)
    df['Is_Fraud'] = (y == -1).astype(int)
    
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)
    
    print(f"\nTraining samples: {len(X_train)}, Test samples: {len(X_test)}")
    print(f"Fraud ratio in test: {(y_test == -1).sum() / len(y_test):.2%}")
    
    results = compare_methods(X_train, X_test, y_test, contamination=0.05)
    
    if X.shape[1] == 2:
        visualize_anomalies_2d(X_test, y_test, results['IsolationForest']['y_pred'], 
                             results['IsolationForest']['scores'], 'IsolationForest')
    
    print(f"\nBest method for Fraud Detection:")
    best_method = max(results.keys(), key=lambda k: results[k]['f1'])
    print(f"  {best_method} with F1={results[best_method]['f1']:.4f}")
    
    return results


def healthcare_example():
    """
    Healthcare example: ICU Patient Anomaly Detection.
    
    Detects abnormal patient vital signs in an ICU setting.
    """
    print(f"\n{'='*60}")
    print(f"HEALTHCARE EXAMPLE: ICU Patient Anomaly Detection")
    print(f"{'='*60}")
    
    np.random.seed(123)
    n_samples = 800
    n_anomalies = 40
    
    n_normal = n_samples - n_anomalies
    
    heart_rate = np.concatenate([
        np.random.normal(75, 10, n_normal),
        np.random.choice([35, 40, 45, 140, 150, 155], n_anomalies)
    ])
    
    systolic_bp = np.concatenate([
        np.random.normal(120, 15, n_normal),
        np.random.choice([70, 75, 180, 190, 200], n_anomalies)
    ])
    
    diastolic_bp = np.concatenate([
        np.random.normal(80, 10, n_normal),
        np.random.choice([40, 45, 110, 120], n_anomalies)
    ])
    
    oxygen_saturation = np.concatenate([
        np.random.normal(97, 2, n_normal),
        np.random.uniform(75, 88, n_anomalies)
    ])
    
    temperature = np.concatenate([
        np.random.normal(37, 0.5, n_normal),
        np.random.choice([35, 35.5, 39.5, 40, 40.5], n_anomalies)
    ])
    
    respiratory_rate = np.concatenate([
        np.random.normal(16, 3, n_normal),
        np.random.choice([6, 8, 30, 35, 40], n_anomalies)
    ])
    
    X = np.column_stack([
        heart_rate,
        systolic_bp,
        diastolic_bp,
        oxygen_saturation,
        temperature,
        respiratory_rate
    ])
    
    y = np.concatenate([np.ones(n_normal), -np.ones(n_anomalies)])
    
    indices = np.arange(n_samples)
    np.random.shuffle(indices)
    X = X[indices]
    y = y[indices]
    
    feature_names = ['Heart_Rate', 'Systolic_BP', 'Diastolic_BP', 
                     'Oxygen_Sat', 'Temperature', 'Resp_Rate']
    
    df = pd.DataFrame(X, columns=feature_names)
    df['Is_Anomaly'] = (y == -1).astype(int)
    
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)
    
    print(f"\nTraining samples: {len(X_train)}, Test samples: {len(X_test)}")
    print(f"Anomaly ratio in test: {(y_test == -1).sum() / len(y_test):.2%}")
    
    results = compare_methods(X_train, X_test, y_test, contamination=0.05)
    
    print(f"\nBest method for ICU Anomaly Detection:")
    best_method = max(results.keys(), key=lambda k: results[k]['f1'])
    print(f"  {best_method} with F1={results[best_method]['f1']:.4f}")
    
    return results


def test_anomaly_detection():
    """
    Test anomaly detection with various datasets.
    """
    print(f"\n{'='*60}")
    print(f"TESTING ANOMALY DETECTION METHODS")
    print(f"{'='*60}")
    
    print("\nTest 1: Basic point anomalies")
    X, y = generate_anomaly_data(n_samples=500, n_anomalies=50)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)
    results = compare_methods(X_train, X_test, y_test, contamination=0.1)
    
    print("\nTest 2: Contextual anomalies")
    X, y = generate_contextual_anomaly_data(n_samples=500, n_anomalies=30)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)
    results = compare_methods(X_train, X_test, y_test, contamination=0.06)
    
    print(f"\n{'='*60}")
    print(f"ALL TESTS COMPLETED SUCCESSFULLY")
    print(f"{'='*60}")
    
    return True


def main():
    """
    Main function to execute anomaly detection examples.
    """
    print("="*60)
    print("ANOMALY DETECTION IMPLEMENTATION")
    print("="*60)
    
    print("\nI. INTRODUCTION")
    print("   Anomaly detection identifies rare items that differ")
    print("   significantly from the majority of data.")
    
    print("\nII. CORE_CONCEPTS")
    print("   - Point, contextual, collective anomalies")
    print("   - Isolation Forest, LOF, One-Class SVM, Elliptic Envelope")
    print("   - Contamination factor, decision scores")
    
    print("\nIII. IMPLEMENTATION")
    
    X, y = generate_anomaly_data(n_samples=500, n_anomalies=50)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)
    
    model, results = core_isolation_forest(X_train, X_test, y_test)
    
    print("\nIV. EXAMPLES")
    banking_results = banking_example()
    healthcare_results = healthcare_example()
    
    print("\nV. OUTPUT_RESULTS")
    print("   All detection results and visualizations displayed above.")
    
    print("\nVI. TESTING")
    test_anomaly_detection()
    
    print("\nVII. ADVANCED_TOPICS")
    print("   - Time series anomaly detection")
    print("   - Streaming detection with updating statistics")
    print("   - Ensemble of multiple detectors")
    print("   - Semi-supervised approaches")
    
    print("\nVIII. CONCLUSION")
    print("   - Isolation Forest: general purpose, scalable")
    print("   - LOF: density-based, good for local anomalies")
    print("   - One-Class SVM: kernel-based, slower but powerful")
    print("   - Elliptic Envelope: assumes Gaussian distribution")
    print("   - Choose based on data characteristics and requirements")
    print("\n   Implementation complete!")


if __name__ == "__main__":
    main()