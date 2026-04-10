# Topic: Support Vector Classification
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Support Vector Classification

I. INTRODUCTION
    Support Vector Machine (SVM) is a powerful supervised learning algorithm used for
    classification and regression tasks. Support Vector Classification (SVC) aims to
    find the optimal hyperplane that maximally separates different classes in the feature space.

II. CORE_CONCEPTS
    - Hyperplane: A decision boundary that separates different classes
    - Support Vectors: The data points closest to the hyperplane that define the margin
    - Maximum Margin Classifier: The hyperplane that maximizes the distance to nearest points
    - Kernel Trick: Transforming data to higher dimensions to make it linearly separable
    - Soft Margin: Allowing some misclassification for better generalization
    - Hard Margin: Strict separation without any misclassification

III. IMPLEMENTATION
    This module covers:
    - Linear Kernel: For linearly separable data
    - RBF (Radial Basis Function) Kernel: For non-linear data
    - Polynomial Kernel: For polynomial decision boundaries
    - Hyperparameter tuning: C (regularization) and gamma (kernel coefficient)

IV. EXAMPLES
    - Banking: Credit card fraud detection
    - Healthcare: Medical condition classification

V. OUTPUT_RESULTS
    - Performance metrics and visualizations
    - Model accuracy, precision, recall, F1-score
    - ROC-AUC scores and confusion matrices

VI. TESTING
    -单元测试 for all SVC implementations
    - Verification of kernel functions
    - Validation of hyperparameters

VII. ADVANCED_TOPICS
    - Multi-class classification strategies
    - Imbalanced dataset handling
    - Cross-validation techniques

VIII. CONCLUSION
    - Summary of SVC strengths and limitations
    - Practical recommendations
"""

# Import necessary libraries
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_classification, make_blobs, make_moons
from sklearn.model_selection import train_test_split, GridSearchCV, StratifiedKFold
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.svm import SVC
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score, 
    roc_auc_score, confusion_matrix, classification_report,
    roc_curve, auc
)
from sklearn.calibration import CalibratedClassifierCV
import warnings
warnings.filterwarnings('ignore')


def generate_nonlinear_data(n_samples=500):
    """
    Generate non-linearly separable synthetic data using various patterns.
    
    This function creates three different types of non-linear datasets:
    1. Moons: Two interleaving half circles
    2. Circles: Data points in concentric circles
    3. Blobs: Gaussian distributed clusters
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate for each class
    
    Returns:
    --------
    X : ndarray
        Feature matrix of shape (n_samples * 3, 2)
    y : ndarray
        Target labels
    dataset_type : list
        Labels indicating the dataset type for each sample
    """
    print("-" * 60)
    print("Generating Non-linear Datasets")
    print("-" * 60)
    
    # Generate moon-shaped data (non-linearly separable)
    X_moons, y_moons = make_moons(n_samples=n_samples, noise=0.15, random_state=42)
    y_moons = y_moons * 2 - 1  # Convert to -1, 1 labels
    
    # Generate circular data
    X_circles, y_circles = make_classification(
        n_samples=n_samples * 2,
        n_features=2,
        n_redundant=0,
        n_informative=2,
        n_clusters_per_class=1,
        class_sep=1.5,
        random_state=42
    )
    # Transform to circular pattern
    radius = np.sqrt(X_circles[:, 0]**2 + X_circles[:, 1]**2).reshape(-1, 1)
    angle = np.arctan2(X_circles[:, 1], X_circles[:, 0]).reshape(-1, 1)
    X_circles_transformed = np.hstack([radius * np.cos(angle * 3), radius * np.sin(angle * 3)])
    y_circles = y_circles * 2 - 1  # Convert to -1, 1 labels
    
    # Generate blob data
    X_blobs, y_blobs = make_blobs(
        n_samples=n_samples * 2,
        centers=[(-2, -2), (2, 2)],
        cluster_std=1.5,
        random_state=42
    )
    y_blobs = y_blobs * 2 - 1  # Convert to -1, 1 labels
    
    # Combine all datasets
    X = np.vstack([X_moons[:n_samples], X_circles_transformed[:n_samples], X_blobs[:n_samples]])
    y = np.concatenate([y_moons[:n_samples], y_circles[:n_samples], y_blobs[:n_samples]])
    
    dataset_type = (['moons'] * n_samples + ['circles'] * n_samples + ['blobs'] * n_samples)
    
    print(f"Total samples generated: {len(y)}")
    print(f"Class distribution: Class -1: {np.sum(y == -1)}, Class 1: {np.sum(y == 1)}")
    print("Data generation complete!")
    
    return X, y, dataset_type


def core_svc():
    """
    Core Support Vector Classification implementation demonstrating various kernels.
    
    This function shows:
    - Linear kernel for linearly separable data
    - RBF kernel for non-linear patterns
    - Polynomial kernel for polynomial decision boundaries
    - Effect of C parameter on margin width
    - Effect of gamma parameter on kernel influence
    """
    print("\n" + "=" * 60)
    print("CORE SUPPORT VECTOR CLASSIFICATION")
    print("=" * 60)
    
    # Generate linearly separable data
    X_linear, y_linear = make_classification(
        n_samples=300,
        n_features=2,
        n_redundant=0,
        n_informative=2,
        n_clusters_per_class=1,
        class_sep=2.0,
        random_state=42
    )
    
    # Generate non-linear data (moons)
    X_nonlinear, y_nonlinear = make_moons(n_samples=300, noise=0.2, random_state=42)
    
    # Standardize features (REQUIRED for SVM)
    scaler = StandardScaler()
    X_linear_scaled = scaler.fit_transform(X_linear)
    X_nonlinear_scaled = scaler.fit_transform(X_nonlinear)
    
    # Split data
    X_train_lin, X_test_lin, y_train_lin, y_test_lin = train_test_split(
        X_linear_scaled, y_linear, test_size=0.25, random_state=42
    )
    X_train_non, X_test_non, y_train_non, y_test_non = train_test_split(
        X_nonlinear_scaled, y_nonlinear, test_size=0.25, random_state=42
    )
    
    results = {}
    
    # ========== LINEAR KERNEL ==========
    print("\n1. LINEAR KERNEL")
    print("-" * 40)
    
    svc_linear = SVC(kernel='linear', C=1.0, random_state=42)
    svc_linear.fit(X_train_lin, y_train_lin)
    y_pred_lin = svc_linear.predict(X_test_lin)
    
    results['linear'] = {
        'accuracy': accuracy_score(y_test_lin, y_pred_lin),
        'support_vectors': svc_linear.n_support_,
        'total_support_vectors': np.sum(svc_linear.n_support_)
    }
    
    print(f"Accuracy: {results['linear']['accuracy']:.4f}")
    print(f"Number of support vectors: {results['linear']['total_support_vector']}")
    print(f"Support vectors per class: {results['linear']['support_vectors']}")
    
    # ========== RBF KERNEL ==========
    print("\n2. RBF (RADIAL BASIS FUNCTION) KERNEL")
    print("-" * 40)
    
    # RBF with different gamma values
    for gamma in [0.1, 1.0, 10.0]:
        svc_rbf = SVC(kernel='rbf', C=1.0, gamma=gamma, random_state=42)
        svc_rbf.fit(X_train_non, y_train_non)
        y_pred_rbf = svc_rbf.predict(X_test_non)
        
        print(f"Gamma={gamma}: Accuracy={accuracy_score(y_test_non, y_pred_rbf):.4f}, "
              f"Support Vectors={np.sum(svc_rbf.n_support_)}")
    
    # Default RBF
    svc_rbf = SVC(kernel='rbf', C=1.0, gamma='scale', random_state=42)
    svc_rbf.fit(X_train_non, y_train_non)
    y_pred_rbf = svc_rbf.predict(X_test_non)
    
    results['rbf'] = {
        'accuracy': accuracy_score(y_test_non, y_pred_rbf),
        'support_vectors': svc_rbf.n_support_,
        'total_support_vectors': np.sum(svc_rbf.n_support_)
    }
    
    print(f"\nRBF (default gamma='scale'):")
    print(f"Accuracy: {results['rbf']['accuracy']:.4f}")
    print(f"Number of support vectors: {results['rbf']['total_support_vectors']}")
    
    # ========== POLYNOMIAL KERNEL ==========
    print("\n3. POLYNOMIAL KERNEL")
    print("-" * 40)
    
    for degree in [2, 3, 4]:
        svc_poly = SVC(kernel='poly', C=1.0, degree=degree, random_state=42)
        svc_poly.fit(X_train_non, y_train_non)
        y_pred_poly = svc_poly.predict(X_test_non)
        
        print(f"Degree={degree}: Accuracy={accuracy_score(y_test_non, y_pred_poly):.4f}, "
              f"Support Vectors={np.sum(svc_poly.n_support_)}")
    
    # ========== C PARAMETER COMPARISON ==========
    print("\n4. C PARAMETER (REGULARIZATION) COMPARISON")
    print("-" * 40)
    print("C controls the trade-off between:")
    print("  - Maximizing the margin (small C)")
    print("  - Minimizing misclassification (large C)")
    
    for C in [0.01, 0.1, 1.0, 10.0, 100.0]:
        svc_c = SVC(kernel='rbf', C=C, gamma='scale', random_state=42)
        svc_c.fit(X_train_non, y_train_non)
        y_pred_c = svc_c.predict(X_test_non)
        
        print(f"C={C:6.2f}: Accuracy={accuracy_score(y_test_non, y_pred_c):.4f}, "
              f"Support Vectors={np.sum(svc_c.n_support_)}")
    
    # ========== GAMMA PARAMETER COMPARISON ==========
    print("\n5. GAMMA PARAMETER (KERNEL COEFFICIENT)")
    print("-" * 40)
    print("Gamma defines how far the influence of a single training example reaches:")
    print("  - Low gamma: Far reach (consider many nearby points)")
    print("  - High gamma: Short reach (consider only nearby points)")
    
    for gamma in ['scale', 'auto', 0.01, 0.1, 1.0, 10.0]:
        svc_g = SVC(kernel='rbf', C=1.0, gamma=gamma, random_state=42)
        svc_g.fit(X_train_non, y_train_non)
        y_pred_g = svc_g.predict(X_test_non)
        
        gamma_str = str(gamma) if isinstance(gamma, str) else gamma
        print(f"Gamma={gamma_str:8s}: Accuracy={accuracy_score(y_test_non, y_pred_g):.4f}")
    
    return results


def kernel_comparison():
    """
    Compare different kernel types on the same dataset.
    
    This function systematically compares:
    - Linear kernel
    - RBF kernel with various gamma values
    - Polynomial kernel with various degrees
    - Sigmoid kernel
    
    Returns:
    --------
    results : dict
        Dictionary containing comparison results for each kernel
    """
    print("\n" + "=" * 60)
    print("KERNEL COMPARISON ANALYSIS")
    print("=" * 60)
    
    # Generate complex non-linear data
    X, y = make_moons(n_samples=400, noise=0.15, random_state=42)
    
    # Scale features (REQUIRED for SVM)
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X_scaled, y, test_size=0.25, random_state=42, stratify=y
    )
    
    results = {}
    kernels = ['linear', 'rbf', 'poly', 'sigmoid']
    
    for kernel in kernels:
        print(f"\nTesting {kernel.upper()} kernel...")
        
        if kernel == 'linear':
            params = {'kernel': kernel, 'C': 1.0}
        elif kernel == 'rbf':
            params = {'kernel': kernel, 'C': 1.0, 'gamma': 'scale'}
        elif kernel == 'poly':
            params = {'kernel': kernel, 'C': 1.0, 'degree': 3}
        else:  # sigmoid
            params = {'kernel': kernel, 'C': 1.0, 'gamma': 'scale'}
        
        svc = SVC(**params, random_state=42)
        svc.fit(X_train, y_train)
        y_pred = svc.predict(X_test)
        
        # Calculate all metrics
        accuracy = accuracy_score(y_test, y_pred)
        precision = precision_score(y_test, y_pred, average='weighted')
        recall = recall_score(y_test, y_pred, average='weighted')
        f1 = f1_score(y_test, y_pred, average='weighted')
        
        results[kernel] = {
            'accuracy': accuracy,
            'precision': precision,
            'recall': recall,
            'f1': f1,
            'support_vectors': np.sum(svc.n_support_)
        }
        
        print(f"  Accuracy:  {accuracy:.4f}")
        print(f"  Precision: {precision:.4f}")
        print(f"  Recall:    {recall:.4f}")
        print(f"  F1-Score:  {f1:.4f}")
        print(f"  Support Vectors: {np.sum(svc.n_support_)}")
    
    # Find best kernel
    best_kernel = max(results, key=lambda k: results[k]['f1'])
    print(f"\n{'=' * 40}")
    print(f"Best kernel by F1-Score: {best_kernel.upper()}")
    print(f"{'=' * 40}")
    
    return results


def banking_example():
    """
    Banking/Finance example: Credit Card Fraud Detection
    
    This example demonstrates SVC for detecting fraudulent credit card
    transactions using various features including transaction amount, time,
    location, and historical patterns.
    """
    print("\n" + "=" * 60)
    print("BANKING EXAMPLE: CREDIT CARD FRAUD DETECTION")
    print("=" * 60)
    
    np.random.seed(42)
    
    # Generate synthetic credit card transaction data
    n_normal = 2000
    n_fraud = 200
    
    # Normal transactions
    normal_amounts = np.random.lognormal(mean=3.5, sigma=1.0, size=n_normal)
    normal_times = np.random.normal(loc=14, scale=4, size=n_normal)  # Hour of day
    normal_distances = np.random.exponential(scale=5, size=n_normal)
    normal_frequencies = np.random.poisson(lam=5, size=n_normal)
    
    # Fraud transactions (typically higher amount, unusual time, far distance)
    fraud_amounts = np.random.lognormal(mean=4.5, sigma=1.5, size=n_fraud)
    fraud_times = np.random.choice([2, 3, 4, 23, 24], size=n_fraud)  # Late night
    fraud_distances = np.random.exponential(scale=50, size=n_fraud)  # Far distances
    fraud_frequencies = np.random.poisson(lam=1, size=n_fraud)  # Low frequency
    
    # Combine features
    X_normal = np.column_stack([normal_amounts, normal_times, normal_distances, normal_frequencies])
    X_fraud = np.column_stack([fraud_amounts, fraud_times, fraud_distances, fraud_frequencies])
    X = np.vstack([X_normal, X_fraud])
    
    # Labels (0 = normal, 1 = fraud)
    y = np.concatenate([np.zeros(n_normal), np.ones(n_fraud)])
    
    print(f"Total transactions: {len(y)}")
    print(f"Normal transactions: {np.sum(y == 0)}")
    print(f"Fraud transactions: {np.sum(y == 1)}")
    print(f"Fraud rate: {np.sum(y == 1) / len(y) * 100:.2f}%")
    
    # Feature scaling (CRITICAL for SVM)
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    # Split data with stratification
    X_train, X_test, y_train, y_test = train_test_split(
        X_scaled, y, test_size=0.25, random_state=42, stratify=y
    )
    
    print("\nTraining SVC models...")
    
    # Model 1: Linear kernel
    print("\n1. Linear Kernel SVC:")
    svc_linear = SVC(kernel='linear', C=1.0, class_weight='balanced', random_state=42)
    svc_linear.fit(X_train, y_train)
    y_pred_linear = svc_linear.predict(X_test)
    
    print(f"   Accuracy: {accuracy_score(y_test, y_pred_linear):.4f}")
    print(f"   Precision: {precision_score(y_test, y_pred_linear):.4f}")
    print(f"   Recall: {recall_score(y_test, y_pred_linear):.4f}")
    print(f"   F1-Score: {f1_score(y_test, y_pred_linear):.4f}")
    print(f"   Support Vectors: {np.sum(svc_linear.n_support_)}")
    
    # Model 2: RBF kernel with tuned parameters
    print("\n2. RBF Kernel SVC (tuned):")
    svc_rbf = SVC(kernel='rbf', C=10.0, gamma='scale', class_weight='balanced', random_state=42)
    svc_rbf.fit(X_train, y_train)
    y_pred_rbf = svc_rbf.predict(X_test)
    
    print(f"   Accuracy: {accuracy_score(y_test, y_pred_rbf):.4f}")
    print(f"   Precision: {precision_score(y_test, y_pred_rbf):.4f}")
    print(f"   Recall: {recall_score(y_test, y_pred_rbf):.4f}")
    print(f"   F1-Score: {f1_score(y_test, y_pred_rbf):.4f}")
    print(f"   Support Vectors: {np.sum(svc_rbf.n_support_)}")
    
    # Model 3: Polynomial kernel
    print("\n3. Polynomial Kernel SVC:")
    svc_poly = SVC(kernel='poly', C=1.0, degree=3, class_weight='balanced', random_state=42)
    svc_poly.fit(X_train, y_train)
    y_pred_poly = svc_poly.predict(X_test)
    
    print(f"   Accuracy: {accuracy_score(y_test, y_pred_poly):.4f}")
    print(f"   Precision: {precision_score(y_test, y_pred_poly):.4f}")
    print(f"   Recall: {recall_score(y_test, y_pred_poly):.4f}")
    print(f"   F1-Score: {f1_score(y_test, y_pred_poly):.4f}")
    print(f"   Support Vectors: {np.sum(svc_poly.n_support_)}")
    
    # Confusion matrices
    print("\nConfusion Matrices:")
    print("\nLinear Kernel:")
    cm_linear = confusion_matrix(y_test, y_pred_linear)
    print(f"  TN: {cm_linear[0,0]}, FP: {cm_linear[0,1]}")
    print(f"  FN: {cm_linear[1,0]}, TP: {cm_linear[1,1]}")
    
    print("\nRBF Kernel:")
    cm_rbf = confusion_matrix(y_test, y_pred_rbf)
    print(f"  TN: {cm_rbf[0,0]}, FP: {cm_rbf[0,1]}")
    print(f"  FN: {cm_rbf[1,0]}, TP: {cm_rbf[1,1]}")
    
    return {
        'linear': {'accuracy': accuracy_score(y_test, y_pred_linear),
                   'f1': f1_score(y_test, y_pred_linear)},
        'rbf': {'accuracy': accuracy_score(y_test, y_pred_rbf),
                'f1': f1_score(y_test, y_pred_rbf)},
        'poly': {'accuracy': accuracy_score(y_test, y_pred_poly),
                 'f1': f1_score(y_test, y_pred_poly)}
    }


def healthcare_example():
    """
    Healthcare example: Medical Condition Classification
    
    This example demonstrates SVC for classifying medical conditions
    based on patient vital signs and test results.
    """
    print("\n" + "=" * 60)
    print("HEALTHCARE EXAMPLE: MEDICAL CONDITION CLASSIFICATION")
    print("=" * 60)
    
    np.random.seed(42)
    
    # Generate synthetic medical data
    n_healthy = 500
    n_condition_a = 250  # Condition A
    n_condition_b = 250  # Condition B
    
    # Generate features for each class
    # Healthy patients (normal vital signs)
    healthy_bp = np.random.normal(120, 15, n_healthy)  # Blood pressure
    healthy_hr = np.random.normal(72, 10, n_healthy)   # Heart rate
    healthy_temp = np.random.normal(98.6, 0.5, n_healthy)  # Temperature
    healthy_o2 = np.random.normal(98, 2, n_healthy)    # O2 saturation
    
    # Condition A patients (e.g., hypertension)
    cond_a_bp = np.random.normal(160, 20, n_condition_a)
    cond_a_hr = np.random.normal(85, 12, n_condition_a)
    cond_a_temp = np.random.normal(98.6, 0.5, n_condition_a)
    cond_a_o2 = np.random.normal(96, 3, n_condition_a)
    
    # Condition B patients (e.g., respiratory issue)
    cond_b_bp = np.random.normal(125, 18, n_condition_b)
    cond_b_hr = np.random.normal(95, 15, n_condition_b)
    cond_b_temp = np.random.normal(100.5, 1.0, n_condition_b)
    cond_b_o2 = np.random.normal(91, 4, n_condition_b)
    
    # Combine features
    X_healthy = np.column_stack([healthy_bp, healthy_hr, healthy_temp, healthy_o2])
    X_cond_a = np.column_stack([cond_a_bp, cond_a_hr, cond_a_temp, cond_a_o2])
    X_cond_b = np.column_stack([cond_b_bp, cond_b_hr, cond_b_temp, cond_b_o2])
    X = np.vstack([X_healthy, X_cond_a, X_cond_b])
    
    # Labels (0: Healthy, 1: Condition A, 2: Condition B)
    y = np.concatenate([
        np.zeros(n_healthy),
        np.ones(n_condition_a),
        np.full(n_condition_b, 2)
    ])
    
    feature_names = ['Blood Pressure', 'Heart Rate', 'Temperature', 'O2 Saturation']
    
    print(f"Total patients: {len(y)}")
    print(f"Healthy: {np.sum(y == 0)}")
    print(f"Condition A: {np.sum(y == 1)}")
    print(f"Condition B: {np.sum(y == 2)}")
    
    # Feature scaling (CRITICAL for SVM)
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    # Split data with stratification
    X_train, X_test, y_train, y_test = train_test_split(
        X_scaled, y, test_size=0.25, random_state=42, stratify=y
    )
    
    print("\nTraining SVC models for multi-class classification...")
    
    # SVM with OvO (One-vs-One) strategy for multi-class
    # Model 1: Linear kernel
    print("\n1. Linear Kernel SVC:")
    svc_linear = SVC(kernel='linear', C=1.0, decision_function_shape='ovO', random_state=42)
    svc_linear.fit(X_train, y_train)
    y_pred_linear = svc_linear.predict(X_test)
    
    print(f"   Accuracy: {accuracy_score(y_test, y_pred_linear):.4f}")
    print(f"   Support Vectors per class: {svc_linear.n_support_}")
    print("\n   Classification Report:")
    print(classification_report(y_test, y_pred_linear, 
                                  target_names=['Healthy', 'Condition A', 'Condition B']))
    
    # Model 2: RBF kernel
    print("\n2. RBF Kernel SVC:")
    svc_rbf = SVC(kernel='rbf', C=10.0, gamma='scale', decision_function_shape='ovO', random_state=42)
    svc_rbf.fit(X_train, y_train)
    y_pred_rbf = svc_rbf.predict(X_test)
    
    print(f"   Accuracy: {accuracy_score(y_test, y_pred_rbf):.4f}")
    print(f"   Support Vectors per class: {svc_rbf.n_support_}")
    print("\n   Classification Report:")
    print(classification_report(y_test, y_pred_rbf,
                                  target_names=['Healthy', 'Condition A', 'Condition B']))
    
    # Model 3: Polynomial kernel
    print("\n3. Polynomial Kernel SVC:")
    svc_poly = SVC(kernel='poly', C=1.0, degree=3, decision_function_shape='ovO', random_state=42)
    svc_poly.fit(X_train, y_train)
    y_pred_poly = svc_poly.predict(X_test)
    
    print(f"   Accuracy: {accuracy_score(y_test, y_pred_poly):.4f}")
    print(f"   Support Vectors per class: {svc_poly.n_support_}")
    print("\n   Classification Report:")
    print(classification_report(y_test, y_pred_poly,
                                  target_names=['Healthy', 'Condition A', 'Condition B']))
    
    return {
        'linear': accuracy_score(y_test, y_pred_linear),
        'rbf': accuracy_score(y_test, y_pred_rbf),
        'poly': accuracy_score(y_test, y_pred_poly)
    }


def hyperparameter_tuning():
    """
    Hyperparameter tuning using GridSearchCV.
    
    This function demonstrates systematic hyperparameter tuning for SVC:
    - C parameter (regularization)
    - Gamma parameter (kernel coefficient)
    - Kernel type
    
    Uses stratified k-fold cross-validation for robust evaluation.
    """
    print("\n" + "=" * 60)
    print("HYPERPARAMETER TUNING")
    print("=" * 60)
    
    # Generate sample data
    X, y = make_moons(n_samples=300, noise=0.15, random_state=42)
    
    # Scale features
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X_scaled, y, test_size=0.25, random_state=42, stratify=y
    )
    
    # Define parameter grid
    param_grid = {
        'C': [0.1, 1, 10, 100],
        'gamma': ['scale', 'auto', 0.01, 0.1, 1],
        'kernel': ['rbf']
    }
    
    print("Parameter grid:")
    for key, values in param_grid.items():
        print(f"  {key}: {values}")
    
    print("\nRunning GridSearchCV...")
    print("This may take a moment...")
    
    # Create SVM classifier
    svc = SVC(random_state=42)
    
    # Grid search with cross-validation
    cv = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
    grid_search = GridSearchCV(
        svc, param_grid, cv=cv, scoring='f1_weighted', 
        n_jobs=-1, verbose=1
    )
    grid_search.fit(X_train, y_train)
    
    print("\nBest parameters found:")
    print(f"  {grid_search.best_params_}")
    print(f"  Best CV Score (F1): {grid_search.best_score_:.4f}")
    
    # Evaluate on test set
    best_model = grid_search.best_estimator_
    y_pred = best_model.predict(X_test)
    
    print("\nTest Set Performance:")
    print(f"  Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"  Precision: {precision_score(y_test, y_pred):.4f}")
    print(f"  Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"  F1-Score: {f1_score(y_test, y_pred):.4f}")
    print(f"  Support Vectors: {np.sum(best_model.n_support_)}")
    
    return grid_search.best_params_


def test_soft_margin_vs_hard_margin():
    """
    Demonstrate the difference between soft margin and hard margin SVM.
    
    Hard margin SVM:
    - Requires perfect linear separation
    - Can fail on non-linearly separable data
    - No misclassification allowed
    
    Soft margin SVM:
    - Allows some misclassification
    - Better generalization to noisy data
    - Controlled by C parameter
    """
    print("\n" + "=" * 60)
    print("SOFT MARGIN vs HARD MARGIN COMPARISON")
    print("=" * 60)
    
    # Generate noisy (non-linearly separable) data
    X, y = make_moons(n_samples=200, noise=0.3, random_state=42)
    
    # Scale features
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X_scaled, y, test_size=0.25, random_state=42
    )
    
    print("\nHard Margin (very large C, effectively infinite):")
    svc_hard = SVC(kernel='rbf', C=1e6, gamma='scale', random_state=42)
    svc_hard.fit(X_train, y_train)
    y_pred_hard = svc_hard.predict(X_test)
    
    print(f"  C = 1e6 (effectively hard margin)")
    print(f"  Train Accuracy: {svc_hard.score(X_train, y_train):.4f}")
    print(f"  Test Accuracy: {accuracy_score(y_test, y_pred_hard):.4f}")
    print(f"  Support Vectors: {np.sum(svc_hard.n_support_)}")
    
    print("\nSoft Margin (moderate C):")
    svc_soft = SVC(kernel='rbf', C=1.0, gamma='scale', random_state=42)
    svc_soft.fit(X_train, y_train)
    y_pred_soft = svc_soft.predict(X_test)
    
    print(f"  C = 1.0")
    print(f"  Train Accuracy: {svc_soft.score(X_train, y_train):.4f}")
    print(f"  Test Accuracy: {accuracy_score(y_test, y_pred_soft):.4f}")
    print(f"  Support Vectors: {np.sum(svc_soft.n_support_)}")
    
    print("\nSoft Margin (small C, more regularization):")
    svc_soft_small = SVC(kernel='rbf', C=0.01, gamma='scale', random_state=42)
    svc_soft_small.fit(X_train, y_train)
    y_pred_soft_small = svc_soft_small.predict(X_test)
    
    print(f"  C = 0.01")
    print(f"  Train Accuracy: {svc_soft_small.score(X_train, y_train):.4f}")
    print(f"  Test Accuracy: {accuracy_score(y_test, y_pred_soft_small):.4f}")
    print(f"  Support Vectors: {np.sum(svc_soft_small.n_support_)}")
    
    print("\n" + "-" * 40)
    print("Analysis:")
    print("-" * 40)
    print("As C decreases, the margin becomes 'softer' (wider).")
    print("This allows more misclassification but can improve")
    print("generalization to unseen data.")
    print("-" * 40)


def main():
    """
    Main function to execute all SVC implementations and examples.
    """
    print("=" * 60)
    print("SUPPORT VECTOR CLASSIFICATION IMPLEMENTATION")
    print("=" * 60)
    print("Author: AI Assistant")
    print("Date: 06-04-2026")
    
    # Run core SVC demonstration
    core_svc_results = core_svc()
    
    # Run kernel comparison
    kernel_comparison_results = kernel_comparison()
    
    # Run banking example
    banking_results = banking_example()
    
    # Run healthcare example
    healthcare_results = healthcare_example()
    
    # Run hyperparameter tuning
    best_params = hyperparameter_tuning()
    
    # Run soft margin vs hard margin demonstration
    test_soft_margin_vs_hard_margin()
    
    # Summary
    print("\n" + "=" * 60)
    print("SUMMARY")
    print("=" * 60)
    print("""
Key Takeaways:
1. SVM requires feature scaling (StandardScaler) for optimal performance
2. RBF kernel is versatile and works well for most non-linear problems
3. C parameter controls the margin width (regularization)
4. Gamma controls the influence of individual training examples
5. Support vectors are the critical data points that define the decision boundary
6. Soft margin (smaller C) often generalizes better than hard margin
7. GridSearchCV is recommended for systematic hyperparameter tuning
    """)


if __name__ == "__main__":
    main()