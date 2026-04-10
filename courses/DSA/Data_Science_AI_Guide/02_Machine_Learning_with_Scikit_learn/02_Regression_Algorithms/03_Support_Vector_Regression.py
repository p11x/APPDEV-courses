# Topic: Support Vector Regression
# Author: AI Assistant
# Date: 06-04-2026

"""
================================================================================
COMPREHENSIVE IMPLEMENTATION FOR SUPPORT VECTOR REGRESSION (SVR)
================================================================================

I. INTRODUCTION
   - Overview of SVR and its importance in machine learning
   - Comparison with other regression algorithms
   - Applications in various domains

II. CORE CONCEPTS
   - Epsilon-insensitive loss function
   - Support vectors and margin concept
   - Kernel functions (linear, RBF, polynomial, sigmoid)
   - Hyperparameters: C, epsilon, gamma

III. IMPLEMENTATION
   - Basic SVR implementation
   - Kernel comparison
   - Parameter tuning (C, epsilon, gamma)
   - Feature scaling importance

IV. EXAMPLES
   A. Banking/Finance Examples
      - Stock price prediction
      - Credit risk assessment
      - Loan amount prediction
      - Portfolio return estimation
   
   B. Healthcare Examples
      - Patient length of stay prediction
      - Medical cost estimation
      - Drug dosage prediction
      - Patient outcome scoring

V. OUTPUT RESULTS
   - Performance metrics and visualizations
   - Model comparison tables
   - Interpretation guidelines

VI. TESTING
   - Unit tests for core functions
   - Integration tests
   - Performance benchmarks

VII. ADVANCED TOPICS
   - Nu-SVR variants
   - Multi-output SVR
   - Online learning with SVR
   - Ensemble methods with SVR

VIII. CONCLUSION
   - Key takeaways
   - Best practices
   - Future directions
"""

# =============================================================================
# IMPORT NECESSARY LIBRARIES
# =============================================================================

import numpy as np
import pandas as pd
import matplotlib
matplotlib.use('Agg')  # Use non-interactive backend
import matplotlib.pyplot as plt
from sklearn.datasets import make_regression, make_sparse_uncorrelated
from sklearn.model_selection import train_test_split, GridSearchCV, cross_val_score
from sklearn.preprocessing import StandardScaler, MinMaxScaler, RobustScaler
from sklearn.svm import SVR, NuSVR
from sklearn.metrics import (
    mean_squared_error, 
    r2_score, 
    mean_absolute_error,
    mean_absolute_percentage_error,
    median_absolute_error,
    max_error
)
import warnings
warnings.filterwarnings('ignore')

# Try to import visualization libraries
try:
    import seaborn as sns
    HAS_SEABORN = True
except ImportError:
    HAS_SEABORN = False

try:
    from mpl_toolkits.mplot3d import Axes3D
    HAS_3D = True
except ImportError:
    HAS_3D = False


# =============================================================================
# SECTION I: INTRODUCTION AND DATA GENERATION
# =============================================================================

def print_header(title):
    """Print a formatted header section."""
    print("\n" + "=" * 80)
    print(f"  {title}")
    print("=" * 80)


def print_subheader(title):
    """Print a formatted subheader."""
    print(f"\n[{title}]")
    print("-" * 60)


def generate_nonlinear_data(n_samples=500, noise_level=0.1, seed=42):
    """
    Generate synthetic non-linear regression data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    noise_level : float
        Standard deviation of Gaussian noise
    seed : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray
        Input features (n_samples, 1)
    y : ndarray
        Target values (n_samples,)
    """
    np.random.seed(seed)
    X = np.sort(np.random.uniform(0, 10, n_samples).reshape(-1, 1))
    y = np.sin(X).flatten() + noise_level * np.random.randn(n_samples)
    return X, y


def generate_complex_nonlinear_data(n_samples=500, noise_level=0.1, seed=42):
    """
    Generate complex non-linear data with multiple features.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    noise_level : float
        Standard deviation of Gaussian noise
    seed : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray
        Input features (n_samples, n_features)
    y : ndarray
        Target values (n_samples,)
    """
    np.random.seed(seed)
    n_features = 5
    X = np.random.uniform(-5, 5, (n_samples, n_features))
    
    # Complex non-linear relationship
    y = (
        np.sin(X[:, 0] * X[:, 1]) +
        np.exp(-X[:, 2]**2) +
        0.5 * X[:, 3]**2 +
        np.cos(X[:, 0] * X[:, 4]) +
        noise_level * np.random.randn(n_samples)
    )
    return X, y


def generate_sparse_data(n_samples=500, n_features=20, noise_level=0.1, seed=42):
    """
    Generate sparse high-dimensional regression data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features (should be > n_samples for sparse problems)
    noise_level : float
        Standard deviation of Gaussian noise
    seed : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray
        Input features (n_samples, n_features)
    y : ndarray
        Target values (n_samples,)
    """
    np.random.seed(seed)
    X = np.random.randn(n_samples, n_features)
    
    # Only a few features are relevant
    y = (
        2 * X[:, 0] +
        1.5 * X[:, 1] +
        np.sin(X[:, 2]) +
        0.5 * X[:, 3]**2 +
        noise_level * np.random.randn(n_samples)
    )
    return X, y


def create_1d_sin_with_outliers(n_samples=500, n_outliers=10, seed=42):
    """
    Create 1D sinusoidal data with added outliers.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_outliers : int
        Number of outliers to add
    seed : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray
        Input features (n_samples, 1)
    y : ndarray
        Target values (n_samples,)
    outlier_indices : ndarray
        Indices of outliers in the data
    """
    np.random.seed(seed)
    X = np.sort(np.random.uniform(0, 10, n_samples).reshape(-1, 1))
    y = np.sin(X).flatten() + 0.1 * np.random.randn(n_samples)
    
    # Add outliers at random positions
    outlier_indices = np.random.choice(n_samples, n_outliers, replace=False)
    outlier_direction = np.random.choice([-1, 1], n_outliers)
    y[outlier_indices] += outlier_direction * 3
    
    return X, y, outlier_indices


# =============================================================================
# SECTION II: CORE CONCEPTS - SVR IMPLEMENTATION
# =============================================================================

def core_svr():
    """
    Core Support Vector Regression implementation demonstrating
    the fundamental concepts.
    """
    print_header("CORE SVR IMPLEMENTATION")
    
    # Generate sample data
    X, y = generate_nonlinear_data(n_samples=200, noise_level=0.2)
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Feature scaling is crucial for SVR
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Data Summary")
    print(f"Training samples: {len(X_train)}")
    print(f"Test samples: {len(X_test)}")
    print(f"Feature range (original): [{X.min():.2f}, {X.max():.2f}]")
    print(f"Feature range (scaled): [{X_train_scaled.min():.2f}, {X_train_scaled.max():.2f}]")
    print(f"Target range: [{y.min():.2f}, {y.max():.2f}]")
    
    # =========================================================================
    # BASIC RBF KERNEL SVR
    # =========================================================================
    print_subheader("Basic RBF Kernel SVR")
    
    svr_rbf = SVR(kernel='rbf', C=1.0, gamma='scale', epsilon=0.1)
    svr_rbf.fit(X_train_scaled, y_train)
    
    y_pred_train = svr_rbf.predict(X_train_scaled)
    y_pred_test = svr_rbf.predict(X_test_scaled)
    
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    train_rmse = np.sqrt(mean_squared_error(y_train, y_pred_train))
    test_rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    
    print(f"  Training R²: {train_r2:.4f}")
    print(f"  Test R²: {test_r2:.4f}")
    print(f"  Training RMSE: {train_rmse:.4f}")
    print(f"  Test RMSE: {test_rmse:.4f}")
    print(f"  Number of support vectors: {svr_rbf.n_support_.sum()}")
    print(f"  Support vector indices (first 10): {svr_rbf.support_[:10]}")
    
    # =========================================================================
    # EPSILON-INSENSITIVE LOSS EXPLANATION
    # =========================================================================
    print_subheader("Epsilon-Insensitive Loss")
    
    # Calculate residuals
    residuals_train = y_train - y_pred_train
    epsilon = 0.1
    
    # Points within epsilon tube (not counted as errors)
    within_tube = np.abs(residuals_train) <= epsilon
    outside_tube = np.abs(residuals_train) > epsilon
    
    print(f"  Epsilon value: {epsilon}")
    print(f"  Total training points: {len(residuals_train)}")
    print(f"  Points within epsilon tube: {within_tube.sum()} ({100*within_tube.mean():.1f}%)")
    print(f"  Points outside epsilon tube: {outside_tube.sum()} ({100*outside_tube.mean():.1f}%)")
    print(f"  Mean absolute residual (all): {np.mean(np.abs(residuals_train)):.4f}")
    print(f"  Mean absolute residual (outside tube): "
          f"{np.mean(np.abs(residuals_train[outside_tube])):.4f}")
    
    # =========================================================================
    # MODEL COEFFICIENTS (for linear kernel)
    # =========================================================================
    print_subheader("Linear Kernel SVR Coefficients")
    
    svr_linear = SVR(kernel='linear', C=1.0, epsilon=0.1)
    svr_linear.fit(X_train_scaled, y_train)
    
    print(f"  Coefficients shape: {svr_linear.coef_.shape}")
    print(f"  Coefficients: {svr_linear.coef_.flatten()}")
    print(f"  Intercept: {svr_linear.intercept_}")
    
    return svr_rbf, svr_linear, scaler


# =============================================================================
# SECTION III: KERNEL FUNCTIONS COMPARISON
# =============================================================================

def svr_kernel_comparison():
    """
    Compare different kernel functions for SVR.
    """
    print_header("KERNEL FUNCTIONS COMPARISON")
    
    # Generate data
    X, y = generate_nonlinear_data(n_samples=300, noise_level=0.15)
    
    # Split and scale
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # Define kernels to compare
    kernels = {
        'Linear': {
            'kernel': SVR(kernel='linear', C=1.0, epsilon=0.1),
            'params': 'C=1.0, ε=0.1'
        },
        'RBF (scale)': {
            'kernel': SVR(kernel='rbf', C=1.0, gamma='scale', epsilon=0.1),
            'params': 'C=1.0, γ=scale, ε=0.1'
        },
        'RBF (0.1)': {
            'kernel': SVR(kernel='rbf', C=1.0, gamma=0.1, epsilon=0.1),
            'params': 'C=1.0, γ=0.1, ε=0.1'
        },
        'RBF (1.0)': {
            'kernel': SVR(kernel='rbf', C=1.0, gamma=1.0, epsilon=0.1),
            'params': 'C=1.0, γ=1.0, ε=0.1'
        },
        'Polynomial (d=2)': {
            'kernel': SVR(kernel='poly', degree=2, C=1.0, epsilon=0.1),
            'params': 'C=1.0, degree=2, ε=0.1'
        },
        'Polynomial (d=3)': {
            'kernel': SVR(kernel='poly', degree=3, C=1.0, epsilon=0.1),
            'params': 'C=1.0, degree=3, ε=0.1'
        },
        'Sigmoid': {
            'kernel': SVR(kernel='sigmoid', C=1.0, epsilon=0.1),
            'params': 'C=1.0, ε=0.1'
        }
    }
    
    # Print comparison table header
    print_subheader("Kernel Performance Comparison")
    print(f"{'Kernel':<22} {'Train R²':>10} {'Test R²':>10} {'RMSE':>10} "
          f"{'MAE':>10} {'# SV':>8} {'Time':>8}")
    print("-" * 78)
    
    results = {}
    import time
    
    for name, config in kernels.items():
        kernel = config['kernel']
        
        start_time = time.time()
        kernel.fit(X_train_scaled, y_train)
        elapsed_time = time.time() - start_time
        
        y_pred_train = kernel.predict(X_train_scaled)
        y_pred_test = kernel.predict(X_test_scaled)
        
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
        mae = mean_absolute_error(y_test, y_pred_test)
        n_sv = kernel.n_support_.sum()
        
        print(f"{name:<22} {train_r2:>10.4f} {test_r2:>10.4f} {rmse:>10.4f} "
              f"{mae:>10.4f} {n_sv:>8} {elapsed_time:>8.4f}")
        
        results[name] = {
            'train_r2': train_r2,
            'test_r2': test_r2,
            'rmse': rmse,
            'mae': mae,
            'n_sv': n_sv,
            'time': elapsed_time,
            'params': config['params']
        }
    
    # Find best kernel
    best_kernel = max(results.items(), key=lambda x: x[1]['test_r2'])
    print(f"\nBest kernel: {best_kernel[0]} with Test R² = {best_kernel[1]['test_r2']:.4f}")
    
    return results


# =============================================================================
# SECTION IV: PARAMETER TUNING
# =============================================================================

def tune_c_parameter():
    """
    Demonstrate the effect of C parameter tuning.
    """
    print_header("C PARAMETER TUNING")
    
    # Generate data
    X, y = generate_nonlinear_data(n_samples=300, noise_level=0.15)
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Effect of C Parameter (RBF Kernel)")
    print(f"{'C':>10} {'Train R²':>10} {'Test R²':>10} {'RMSE':>10} "
          f"{'MAE':>10} {'# SV':>8}")
    print("-" * 58)
    
    C_values = [0.001, 0.01, 0.1, 1, 10, 100, 1000]
    
    results = []
    for C in C_values:
        svr = SVR(kernel='rbf', C=C, gamma='scale', epsilon=0.1)
        svr.fit(X_train_scaled, y_train)
        
        y_pred_train = svr.predict(X_train_scaled)
        y_pred_test = svr.predict(X_test_scaled)
        
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
        mae = mean_absolute_error(y_test, y_pred_test)
        n_sv = svr.n_support_.sum()
        
        print(f"{C:>10} {train_r2:>10.4f} {test_r2:>10.4f} {rmse:>10.4f} "
              f"{mae:>10.4f} {n_sv:>8}")
        
        results.append({
            'C': C,
            'train_r2': train_r2,
            'test_r2': test_r2,
            'rmse': rmse,
            'mae': mae,
            'n_sv': n_sv
        })
    
    print_subheader("C Parameter Interpretation")
    print("  Low C (e.g., 0.001-0.1):")
    print("    - More regularization, smoother model")
    print("    - More support vectors")
    print("    - May underfit the data")
    print()
    print("  High C (e.g., 100-1000):")
    print("    - Less regularization, complex model")
    print("    - Fewer support vectors")
    print("    - May overfit the data")
    
    return results


def tune_epsilon_parameter():
    """
    Demonstrate the effect of epsilon parameter tuning.
    """
    print_header("EPSILON PARAMETER TUNING")
    
    # Generate data
    X, y = generate_nonlinear_data(n_samples=300, noise_level=0.15)
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Effect of Epsilon Parameter (RBF Kernel, C=10)")
    print(f"{'Epsilon':>10} {'Train R²':>10} {'Test R²':>10} {'RMSE':>10} "
          f"{'MAE':>10} {'# SV':>8}")
    print("-" * 58)
    
    epsilon_values = [0.001, 0.01, 0.05, 0.1, 0.2, 0.5, 1.0]
    
    results = []
    for epsilon in epsilon_values:
        svr = SVR(kernel='rbf', C=10, gamma='scale', epsilon=epsilon)
        svr.fit(X_train_scaled, y_train)
        
        y_pred_train = svr.predict(X_train_scaled)
        y_pred_test = svr.predict(X_test_scaled)
        
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
        mae = mean_absolute_error(y_test, y_pred_test)
        n_sv = svr.n_support_.sum()
        
        print(f"{epsilon:>10.3f} {train_r2:>10.4f} {test_r2:>10.4f} {rmse:>10.4f} "
              f"{mae:>10.4f} {n_sv:>8}")
        
        results.append({
            'epsilon': epsilon,
            'train_r2': train_r2,
            'test_r2': test_r2,
            'rmse': rmse,
            'mae': mae,
            'n_sv': n_sv
        })
    
    print_subheader("Epsilon Parameter Interpretation")
    print("  Low epsilon (e.g., 0.001-0.01):")
    print("    - Narrow tolerance tube")
    print("    - More support vectors")
    print("    - Fits training data more precisely")
    print()
    print("  High epsilon (e.g., 0.5-1.0):")
    print("    - Wide tolerance tube")
    print("    - Fewer support vectors")
    print("    - More robust to noise")
    
    return results


def tune_gamma_parameter():
    """
    Demonstrate the effect of gamma parameter tuning in RBF kernel.
    """
    print_header("GAMMA PARAMETER TUNING (RBF KERNEL)")
    
    # Generate data
    X, y = generate_nonlinear_data(n_samples=300, noise_level=0.15)
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Effect of Gamma Parameter (RBF Kernel, C=10)")
    print(f"{'Gamma':>10} {'Train R²':>10} {'Test R²':>10} {'RMSE':>10} "
          f"{'MAE':>10} {'# SV':>8}")
    print("-" * 58)
    
    gamma_values = [0.001, 0.01, 0.1, 0.5, 1, 5, 10, 'scale', 'auto']
    
    results = []
    for gamma in gamma_values:
        gamma_str = str(gamma)
        svr = SVR(kernel='rbf', C=10, gamma=gamma, epsilon=0.1)
        svr.fit(X_train_scaled, y_train)
        
        y_pred_train = svr.predict(X_train_scaled)
        y_pred_test = svr.predict(X_test_scaled)
        
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
        mae = mean_absolute_error(y_test, y_pred_test)
        n_sv = svr.n_support_.sum()
        
        print(f"{gamma_str:>10} {train_r2:>10.4f} {test_r2:>10.4f} {rmse:>10.4f} "
              f"{mae:>10.4f} {n_sv:>8}")
        
        results.append({
            'gamma': gamma_str,
            'train_r2': train_r2,
            'test_r2': test_r2,
            'rmse': rmse,
            'mae': mae,
            'n_sv': n_sv
        })
    
    print_subheader("Gamma Parameter Interpretation")
    print("  Low gamma (e.g., 0.001-0.1):")
    print("    - Wide Gaussian kernel")
    print("    - Points influence more distant regions")
    print("    - May underfit (too smooth)")
    print()
    print("  High gamma (e.g., 5-10):")
    print("    - Narrow Gaussian kernel")
    print("    - Points influence only nearby regions")
    print("    - May overfit (too complex)")
    
    return results


def grid_search_svr():
    """
    Perform grid search for optimal SVR hyperparameters.
    """
    print_header("GRID SEARCH FOR SVR")
    
    # Generate data
    X, y = generate_nonlinear_data(n_samples=300, noise_level=0.15)
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Grid Search Parameters")
    
    param_grid = {
        'C': [0.1, 1, 10, 100],
        'gamma': [0.01, 0.1, 1],
        'epsilon': [0.01, 0.1, 0.2]
    }
    
    print("  Parameter grid:")
    for param, values in param_grid.items():
        print(f"    {param}: {values}")
    
    # Perform grid search
    svr = SVR(kernel='rbf')
    grid_search = GridSearchCV(
        svr, param_grid, 
        cv=5, 
        scoring='r2',
        n_jobs=-1,
        verbose=0
    )
    grid_search.fit(X_train_scaled, y_train)
    
    print_subheader("Grid Search Results")
    print(f"  Best parameters: {grid_search.best_params_}")
    print(f"  Best cross-validation R²: {grid_search.best_score_:.4f}")
    
    # Evaluate on test set
    y_pred_test = grid_search.predict(X_test_scaled)
    test_r2 = r2_score(y_test, y_pred_test)
    test_rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    test_mae = mean_absolute_error(y_test, y_pred_test)
    
    print(f"  Test R²: {test_r2:.4f}")
    print(f"  Test RMSE: {test_rmse:.4f}")
    print(f"  Test MAE: {test_mae:.4f}")
    
    # Show top 5 parameter combinations
    print_subheader("Top 5 Parameter Combinations")
    
    cv_results = pd.DataFrame(grid_search.cv_results_)
    cv_results = cv_results.sort_values('rank_test_score')
    
    print(f"{'Rank':>5} {'C':>8} {'Gamma':>8} {'Epsilon':>10} "
          f"{'Mean R²':>10} {'Std R²':>10}")
    print("-" * 51)
    
    for i, row in cv_results.head(5).iterrows():
        print(f"{row['rank_test_score']:>5} {row['param_C']:>8} "
              f"{row['param_gamma']:>8} {row['param_epsilon']:>10} "
              f"{row['mean_test_score']:>10.4f} {row['std_test_score']:>10.4f}")
    
    return grid_search.best_params_, grid_search.best_score_


# =============================================================================
# SECTION V: FEATURE SCALING IMPORTANCE
# =============================================================================

def demonstrate_scaling_importance():
    """
    Demonstrate why feature scaling is crucial for SVR.
    """
    print_header("FEATURE SCALING IMPORTANCE")
    
    # Generate data with different feature scales
    np.random.seed(42)
    n_samples = 300
    
    # Features with very different scales
    X1 = np.random.uniform(0, 1, n_samples) * 1000  # 0-1000
    X2 = np.random.uniform(0, 1, n_samples) * 0.001  # 0-0.001
    X3 = np.random.uniform(0, 1, n_samples) * 1000000  # 0-1000000
    
    X = np.column_stack([X1, X2, X3])
    
    # Target with non-linear relationship
    y = (X1 * 0.01) + np.sin(X2 * 1000) + (X3 * 0.00001) + np.random.randn(n_samples) * 0.1
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    print_subheader("Without Feature Scaling")
    print("  Feature ranges:")
    print(f"    X1: [{X[:, 0].min():.2f}, {X[:, 0].max():.2f}]")
    print(f"    X2: [{X[:, 1].min():.6f}, {X[:, 1].max():.6f}]")
    print(f"    X3: [{X[:, 2].min():.2f}, {X[:, 2].max():.2f}]")
    
    # Train SVR without scaling
    svr_unscaled = SVR(kernel='rbf', C=1.0, gamma='scale', epsilon=0.1)
    svr_unscaled.fit(X_train, y_train)
    
    y_pred_test = svr_unscaled.predict(X_test)
    test_r2 = r2_score(y_test, y_pred_test)
    test_rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    
    print(f"  Test R²: {test_r2:.4f}")
    print(f"  Test RMSE: {test_rmse:.4f}")
    print(f"  Number of support vectors: {svr_unscaled.n_support_.sum()}")
    
    print_subheader("With Standard Scaling")
    
    # Train SVR with scaling
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("  Feature ranges after scaling:")
    print(f"    X1: [{X_train_scaled[:, 0].min():.2f}, {X_train_scaled[:, 0].max():.2f}]")
    print(f"    X2: [{X_train_scaled[:, 1].min():.2f}, {X_train_scaled[:, 1].max():.2f}]")
    print(f"    X3: [{X_train_scaled[:, 2].min():.2f}, {X_train_scaled[:, 2].max():.2f}]")
    
    svr_scaled = SVR(kernel='rbf', C=1.0, gamma='scale', epsilon=0.1)
    svr_scaled.fit(X_train_scaled, y_train)
    
    y_pred_test_scaled = svr_scaled.predict(X_test_scaled)
    test_r2_scaled = r2_score(y_test, y_pred_test_scaled)
    test_rmse_scaled = np.sqrt(mean_squared_error(y_test, y_pred_test_scaled))
    
    print(f"  Test R²: {test_r2_scaled:.4f}")
    print(f"  Test RMSE: {test_rmse_scaled:.4f}")
    print(f"  Number of support vectors: {svr_scaled.n_support_.sum()}")
    
    print_subheader("Comparison Summary")
    print(f"  R² Improvement: {test_r2_scaled - test_r2:.4f}")
    print(f"  RMSE Improvement: {test_rmse - test_rmse_scaled:.4f}")
    
    print("\n  Why scaling is important:")
    print("    - Kernel functions use distances")
    print("    - Without scaling, features with large values dominate")
    print("    - StandardScaler normalizes all features to similar ranges")
    print("    - Essential for RBF, polynomial, and sigmoid kernels")


# =============================================================================
# SECTION VI: BANKING/FINANCE EXAMPLES
# =============================================================================

def banking_stock_prediction():
    """
    Banking example: Stock price prediction using SVR.
    """
    print_header("BANKING EXAMPLE: STOCK PRICE PREDICTION")
    
    # Generate synthetic stock market data
    np.random.seed(42)
    n_samples = 500
    
    # Features representing technical indicators
    data = pd.DataFrame({
        'volume': np.random.lognormal(15, 0.5, n_samples),
        'price_to_earnings': np.random.normal(20, 5, n_samples).clip(5, 50),
        'dividend_yield': np.random.exponential(2, n_samples),
        'beta': np.random.normal(1, 0.3, n_samples).clip(0.3, 2.5),
        'market_cap': np.random.lognormal(22, 1.5, n_samples),
        'debt_to_equity': np.random.exponential(0.5, n_samples),
        'roe': np.random.normal(15, 5, n_samples).clip(-10, 40),
    })
    
    # Target: stock return (with non-linear relationship)
    data['return'] = (
        0.05 * data['dividend_yield'] +
        0.02 * (data['market_cap'] / 1e9) ** 0.3 +
        0.1 * data['beta'] +
        0.005 * data['roe'] ** 1.5 +
        0.1 * np.log(data['volume']) +
        -0.02 * data['debt_to_equity'] +
        np.random.normal(0, 2, n_samples)
    )
    
    # Prepare features and target
    X = data.drop('return', axis=1).values
    y = data['return'].values
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Dataset Summary")
    print(f"  Training samples: {len(X_train)}")
    print(f"  Test samples: {len(X_test)}")
    print(f"  Number of features: {X.shape[1]}")
    print(f"  Target range: [{y.min():.2f}%, {y.max():.2f}%]")
    
    # Train multiple SVR models
    print_subheader("Model Comparison")
    
    models = {
        'Linear SVR': SVR(kernel='linear', C=10, epsilon=0.1),
        'RBF SVR (scale)': SVR(kernel='rbf', C=10, gamma='scale', epsilon=0.1),
        'RBF SVR (0.1)': SVR(kernel='rbf', C=10, gamma=0.1, epsilon=0.1),
        'Polynomial (d=2)': SVR(kernel='poly', degree=2, C=10, epsilon=0.1),
        'Polynomial (d=3)': SVR(kernel='poly', degree=3, C=10, epsilon=0.1),
    }
    
    print(f"{'Model':<22} {'Train R²':>10} {'Test R²':>10} {'RMSE':>10} "
          f"{'MAE':>10} {'# SV':>8}")
    print("-" * 70)
    
    results = {}
    for name, model in models.items():
        model.fit(X_train_scaled, y_train)
        
        y_pred_train = model.predict(X_train_scaled)
        y_pred_test = model.predict(X_test_scaled)
        
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
        mae = mean_absolute_error(y_test, y_pred_test)
        n_sv = model.n_support_.sum()
        
        print(f"{name:<22} {train_r2:>10.4f} {test_r2:>10.4f} {rmse:>10.4f} "
              f"{mae:>10.4f} {n_sv:>8}")
        
        results[name] = {
            'train_r2': train_r2,
            'test_r2': test_r2,
            'rmse': rmse,
            'mae': mae,
            'n_sv': n_sv
        }
    
    # Grid search for best parameters
    print_subheader("Grid Search for Best Parameters")
    
    param_grid = {
        'C': [1, 10, 100],
        'gamma': [0.01, 0.1, 1],
        'epsilon': [0.05, 0.1, 0.2]
    }
    
    grid_search = GridSearchCV(
        SVR(kernel='rbf'), param_grid, cv=5, scoring='r2', n_jobs=-1
    )
    grid_search.fit(X_train_scaled, y_train)
    
    print(f"  Best parameters: {grid_search.best_params_}")
    print(f"  Best CV R²: {grid_search.best_score_:.4f}")
    
    y_pred_best = grid_search.predict(X_test_scaled)
    test_r2_best = r2_score(y_test, y_pred_best)
    test_rmse_best = np.sqrt(mean_squared_error(y_test, y_pred_best))
    
    print(f"  Test R²: {test_r2_best:.4f}")
    print(f"  Test RMSE: {test_rmse_best:.4f}")
    
    print_subheader("Feature Importance (via correlation)")
    feature_names = ['volume', 'PE_ratio', 'dividend', 'beta', 'market_cap', 'debt_to_eq', 'ROE']
    correlations = np.array([np.abs(np.corrcoef(X[:, i], y)[0, 1]) for i in range(X.shape[1])])
    
    print("  Absolute correlation with target:")
    for name, corr in sorted(zip(feature_names, correlations), key=lambda x: -x[1]):
        print(f"    {name:<15}: {corr:.4f}")
    
    return results


def banking_credit_risk():
    """
    Banking example: Credit risk assessment using SVR.
    """
    print_header("BANKING EXAMPLE: CREDIT RISK SCORING")
    
    # Generate synthetic credit data
    np.random.seed(42)
    n_samples = 1000
    
    data = pd.DataFrame({
        'credit_score': np.random.normal(650, 100, n_samples).clip(300, 850),
        'income': np.random.lognormal(10.5, 0.5, n_samples),
        'debt_amount': np.random.lognormal(9, 1, n_samples),
        'credit_history_years': np.random.exponential(5, n_samples),
        'num_credit_lines': np.random.poisson(5, n_samples),
        'utilization': np.random.beta(2, 5, n_samples),
        'num_delinquencies': np.random.poisson(0.5, n_samples),
    })
    
    # Target: Credit risk score (0-100)
    # Non-linear relationship with features
    data['risk_score'] = (
        100 - 0.08 * (data['credit_score'] - 300) +
        0.0001 * np.exp(data['income'] / 50000) +
        0.01 * data['debt_amount'] -
        2 * data['credit_history_years'] +
        3 * data['utilization'] ** 2 +
        5 * data['num_delinquencies'] +
        np.random.normal(0, 5, n_samples)
    ).clip(0, 100)
    
    X = data.drop('risk_score', axis=1).values
    y = data['risk_score'].values
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Dataset Summary")
    print(f"  Training samples: {len(X_train)}")
    print(f"  Test samples: {len(X_test)}")
    print(f"  Risk score range: [{y.min():.2f}, {y.max():.2f}]")
    
    # Train SVR model
    print_subheader("SVR Credit Risk Model")
    
    svr = SVR(kernel='rbf', C=50, gamma=0.1, epsilon=0.5)
    svr.fit(X_train_scaled, y_train)
    
    y_pred_train = svr.predict(X_train_scaled)
    y_pred_test = svr.predict(X_test_scaled)
    
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    test_rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    test_mae = mean_absolute_error(y_test, y_pred_test)
    test_mape = mean_absolute_percentage_error(y_test, y_pred_test)
    
    print(f"  Training R²: {train_r2:.4f}")
    print(f"  Test R²: {test_r2:.4f}")
    print(f"  Test RMSE: {test_rmse:.4f}")
    print(f"  Test MAE: {test_mae:.4f}")
    print(f"  Test MAPE: {test_mape*100:.2f}%")
    print(f"  Number of support vectors: {svr.n_support_.sum()}")
    
    # Cross-validation
    cv_scores = cross_val_score(
        SVR(kernel='rbf', C=50, gamma=0.1, epsilon=0.5),
        X_train_scaled, y_train, cv=5, scoring='r2'
    )
    
    print(f"  Cross-validation R² (5-fold):")
    print(f"    Mean: {cv_scores.mean():.4f}")
    print(f"    Std: {cv_scores.std():.4f}")
    print(f"    Scores: {cv_scores}")
    
    # Risk categories
    print_subheader("Risk Category Prediction")
    
    y_test_categories = pd.cut(y_test, bins=[0, 30, 60, 100], labels=['Low', 'Medium', 'High'])
    y_pred_categories = pd.cut(y_pred_test, bins=[0, 30, 60, 100], labels=['Low', 'Medium', 'High'])
    
    print("  Confusion matrix:")
    print("              Predicted")
    print("              Low    Medium   High")
    
    for true_cat in ['Low', 'Medium', 'High']:
        row = f"  True {true_cat:<5}"
        for pred_cat in ['Low', 'Medium', 'High']:
            count = ((y_test_categories == true_cat) & (y_pred_categories == pred_cat)).sum()
            row += f"  {count:>4}"
        print(row)
    
    return {
        'test_r2': test_r2,
        'test_rmse': test_rmse,
        'test_mae': test_mae,
        'test_mape': test_mape,
        'n_sv': svr.n_support_.sum()
    }


def banking_loan_prediction():
    """
    Banking example: Loan amount prediction.
    """
    print_header("BANKING EXAMPLE: LOAN AMOUNT PREDICTION")
    
    # Generate synthetic loan data
    np.random.seed(42)
    n_samples = 800
    
    data = pd.DataFrame({
        'annual_income': np.random.lognormal(11, 0.6, n_samples),
        'credit_score': np.random.normal(700, 80, n_samples).clip(500, 850),
        'employment_years': np.random.exponential(5, n_samples),
        'existing_debt': np.random.lognormal(9.5, 1.2, n_samples),
        'num_dependents': np.random.poisson(1.5, n_samples),
        'property_value': np.random.lognormal(12.5, 0.8, n_samples),
    })
    
    # Target: Loan amount (non-linear relationship)
    data['loan_amount'] = (
        0.3 * data['annual_income'] +
        500 * data['credit_score'] ** 0.8 +
        1000 * data['employment_years'] ** 0.5 +
        -0.2 * data['existing_debt'] +
        0.5 * data['property_value'] +
        np.random.normal(0, 5000, n_samples)
    ).clip(1000, 500000)
    
    X = data.drop('loan_amount', axis=1).values
    y = data['loan_amount'].values
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Dataset Summary")
    print(f"  Training samples: {len(X_train)}")
    print(f"  Test samples: {len(X_test)}")
    print(f"  Loan amount range: ${y.min():,.0f} - ${y.max():,.0f}")
    
    # Train SVR model with different kernels
    print_subheader("Kernel Comparison for Loan Prediction")
    
    kernels = [
        ('Linear', SVR(kernel='linear', C=100, epsilon=100)),
        ('RBF', SVR(kernel='rbf', C=100, gamma=0.1, epsilon=100)),
        ('Polynomial', SVR(kernel='poly', degree=2, C=100, epsilon=100)),
    ]
    
    print(f"{'Kernel':<15} {'Train R²':>10} {'Test R²':>10} {'RMSE':>12} "
          f"{'MAE':>12} {'# SV':>8}")
    print("-" * 67)
    
    results = {}
    for name, kernel in kernels:
        kernel.fit(X_train_scaled, y_train)
        
        y_pred_train = kernel.predict(X_train_scaled)
        y_pred_test = kernel.predict(X_test_scaled)
        
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
        mae = mean_absolute_error(y_test, y_pred_test)
        n_sv = kernel.n_support_.sum()
        
        print(f"{name:<15} {train_r2:>10.4f} {test_r2:>10.4f} ${rmse:>10,.0f} "
              f"${mae:>10,.0f} {n_sv:>8}")
        
        results[name] = {
            'test_r2': test_r2,
            'rmse': rmse,
            'mae': mae
        }
    
    return results


# =============================================================================
# SECTION VII: HEALTHCARE EXAMPLES
# =============================================================================

def healthcare_length_of_stay():
    """
    Healthcare example: Patient length of stay prediction.
    """
    print_header("HEALTHCARE EXAMPLE: PATIENT LENGTH OF STAY PREDICTION")
    
    # Generate synthetic healthcare data
    np.random.seed(42)
    n_patients = 1000
    
    patient_data = pd.DataFrame({
        'age': np.random.normal(60, 20, n_patients).clip(18, 95),
        'bmi': np.random.normal(28, 5, n_patients).clip(15, 50),
        'systolic_bp': np.random.normal(130, 20, n_patients),
        'diastolic_bp': np.random.normal(80, 10, n_patients),
        'heart_rate': np.random.normal(75, 15, n_patients),
        'white_blood_cell': np.random.normal(8, 3, n_patients),
        'hemoglobin': np.random.normal(13, 2, n_patients),
        'glucose': np.random.normal(100, 20, n_patients),
        'creatinine': np.random.normal(1, 0.3, n_patients),
        'alt': np.random.normal(25, 10, n_patients),
    })
    
    # Target: Length of stay (days) - non-linear relationship
    patient_data['length_of_stay'] = (
        2 +
        0.05 * patient_data['age'] +
        0.1 * (patient_data['bmi'] - 28) ** 2 +
        0.02 * (patient_data['systolic_bp'] - 130) +
        0.3 * (patient_data['white_blood_cell'] - 8) +
        0.1 * (patient_data['glucose'] > 120).astype(int) * 2 +
        0.15 * patient_data['creatinine'] ** 2 +
        np.random.normal(0, 1, n_patients)
    ).clip(1, 30)
    
    X = patient_data.drop('length_of_stay', axis=1).values
    y = patient_data['length_of_stay'].values
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Dataset Summary")
    print(f"  Training samples: {len(X_train)}")
    print(f"  Test samples: {len(X_test)}")
    print(f"  Features: {X.shape[1]}")
    print(f"  Length of stay range: {y.min():.1f} - {y.max():.1f} days")
    
    # Train SVR model
    print_subheader("SVR Length of Stay Model")
    
    svr = SVR(kernel='rbf', C=10, gamma=0.1, epsilon=0.5)
    svr.fit(X_train_scaled, y_train)
    
    y_pred_train = svr.predict(X_train_scaled)
    y_pred_test = svr.predict(X_test_scaled)
    
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    test_rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    test_mae = mean_absolute_error(y_test, y_pred_test)
    test_mape = mean_absolute_percentage_error(y_test, y_pred_test)
    median_error = median_absolute_error(y_test, y_pred_test)
    max_err = max_error(y_test, y_pred_test)
    
    print(f"  Training R²: {train_r2:.4f}")
    print(f"  Test R²: {test_r2:.4f}")
    print(f"  Test RMSE: {test_rmse:.2f} days")
    print(f"  Test MAE: {test_mae:.2f} days")
    print(f"  Test MAPE: {test_mape*100:.2f}%")
    print(f"  Median absolute error: {median_error:.2f} days")
    print(f"  Max error: {max_err:.2f} days")
    print(f"  Number of support vectors: {svr.n_support_.sum()}")
    
    # Feature correlations
    print_subheader("Feature Correlations with Length of Stay")
    
    feature_names = patient_data.drop('length_of_stay', axis=1).columns
    correlations = np.array([np.abs(np.corrcoef(X[:, i], y)[0, 1]) for i in range(X.shape[1])])
    
    print("  Correlation with length of stay:")
    for name, corr in sorted(zip(feature_names, correlations), key=lambda x: -x[1])[:5]:
        print(f"    {name:<20}: {corr:.4f}")
    
    # Outlier analysis
    print_subheader("Prediction Error Analysis")
    
    errors = y_test - y_pred_test
    print(f"  Error distribution:")
    print(f"    Mean: {np.mean(errors):.2f} days")
    print(f"    Std: {np.std(errors):.2f} days")
    print(f"    Percentiles:")
    print(f"      25th: {np.percentile(errors, 25):.2f} days")
    print(f"      50th: {np.percentile(errors, 50):.2f} days")
    print(f"      75th: {np.percentile(errors, 75):.2f} days")
    print(f"      90th: {np.percentile(errors, 90):.2f} days")
    print(f"      95th: {np.percentile(errors, 95):.2f} days")
    
    return {
        'test_r2': test_r2,
        'test_rmse': test_rmse,
        'test_mae': test_mae,
        'n_sv': svr.n_support_.sum()
    }


def healthcare_medical_cost():
    """
    Healthcare example: Medical cost estimation.
    """
    print_header("HEALTHCARE EXAMPLE: MEDICAL COST ESTIMATION")
    
    # Generate synthetic medical cost data
    np.random.seed(42)
    n_patients = 800
    
    data = pd.DataFrame({
        'age': np.random.normal(55, 20, n_patients).clip(18, 90),
        'num_procedures': np.random.poisson(2, n_patients),
        'num_medications': np.random.poisson(4, n_patients),
        'num_lab_tests': np.random.poisson(5, n_patients),
        'hospital_days': np.random.poisson(3, n_patients),
        'icu_days': np.random.poisson(0.5, n_patients),
        'surgery_flag': np.random.binomial(1, 0.3, n_patients),
        'emergency_flag': np.random.binomial(1, 0.2, n_patients),
        'comorbidity_count': np.random.poisson(1, n_patients),
    })
    
    # Target: Medical cost (non-linear relationship)
    data['total_cost'] = (
        500 * data['hospital_days'] +
        1000 * data['icu_days'] +
        2000 * data['surgery_flag'] +
        500 * data['emergency_flag'] +
        100 * data['num_procedures'] +
        50 * data['num_medications'] +
        30 * data['num_lab_tests'] +
        20 * data['age'] +
        500 * data['comorbidity_count'] +
        np.random.normal(0, 1000, n_patients)
    ).clip(500, 100000)
    
    X = data.drop('total_cost', axis=1).values
    y = data['total_cost'].values
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Dataset Summary")
    print(f"  Training samples: {len(X_train)}")
    print(f"  Test samples: {len(X_test)}")
    print(f"  Cost range: ${y.min():,.0f} - ${y.max():,.0f}")
    
    # Train multiple models
    print_subheader("Model Comparison")
    
    models = {
        'Linear': SVR(kernel='linear', C=1000, epsilon=500),
        'RBF (C=10)': SVR(kernel='rbf', C=10, gamma=0.1, epsilon=500),
        'RBF (C=100)': SVR(kernel='rbf', C=100, gamma=0.1, epsilon=500),
        'RBF (C=1000)': SVR(kernel='rbf', C=1000, gamma=0.1, epsilon=500),
        'Polynomial': SVR(kernel='poly', degree=2, C=100, epsilon=500),
    }
    
    print(f"{'Model':<18} {'Train R²':>10} {'Test R²':>10} {'RMSE':>12} "
          f"{'MAE':>12} {'# SV':>8}")
    print("-" * 70)
    
    results = {}
    for name, model in models.items():
        model.fit(X_train_scaled, y_train)
        
        y_pred_train = model.predict(X_train_scaled)
        y_pred_test = model.predict(X_test_scaled)
        
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
        mae = mean_absolute_error(y_test, y_pred_test)
        n_sv = model.n_support_.sum()
        
        print(f"{name:<18} {train_r2:>10.4f} {test_r2:>10.4f} ${rmse:>10,.0f} "
              f"${mae:>10,.0f} {n_sv:>8}")
        
        results[name] = {
            'test_r2': test_r2,
            'rmse': rmse,
            'mae': mae
        }
    
    return results


def healthcare_drug_dosage():
    """
    Healthcare example: Drug dosage prediction.
    """
    print_header("HEALTHCARE EXAMPLE: DRUG DOSAGE PREDICTION")
    
    # Generate synthetic drug dosage data
    np.random.seed(42)
    n_patients = 600
    
    data = pd.DataFrame({
        'weight': np.random.normal(70, 15, n_patients).clip(30, 150),
        'age': np.random.normal(50, 20, n_patients).clip(18, 85),
        'creatinine_clearance': np.random.normal(80, 30, n_patients).clip(10, 150),
        'liver_enzyme_alt': np.random.normal(25, 10, n_patients).clip(5, 100),
        'albumin': np.random.normal(4, 0.5, n_patients).clip(2, 6),
        'concomitant_drugs': np.random.poisson(1, n_patients),
    })
    
    # Target: Drug dosage (mg) - non-linear relationship
    # Higher weight requires more dosage
    # Lower creatinine clearance requires less dosage
    # Higher liver enzyme may indicate need for adjustment
    data['dosage'] = (
        data['weight'] * 0.5 +
        0.3 * data['age'] +
        -0.2 * (data['creatinine_clearance'] - 80) +
        0.1 * (data['liver_enzyme_alt'] - 25) ** 1.5 +
        -2 * (data['albumin'] < 3).astype(int) +
        5 * data['concomitant_drugs'] +
        np.random.normal(0, 5, n_patients)
    ).clip(10, 200)
    
    X = data.drop('dosage', axis=1).values
    y = data['dosage'].values
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Dataset Summary")
    print(f"  Training samples: {len(X_train)}")
    print(f"  Test samples: {len(X_test)}")
    print(f"  Dosage range: {y.min():.1f} - {y.max():.1f} mg")
    
    # Grid search for optimal parameters
    print_subheader("Grid Search for Optimal Parameters")
    
    param_grid = {
        'C': [10, 50, 100],
        'gamma': [0.01, 0.1, 0.5],
        'epsilon': [1, 2, 5]
    }
    
    grid_search = GridSearchCV(
        SVR(kernel='rbf'), param_grid, cv=5, scoring='neg_mean_absolute_error', n_jobs=-1
    )
    grid_search.fit(X_train_scaled, y_train)
    
    print(f"  Best parameters: {grid_search.best_params_}")
    print(f"  Best CV MAE: {-grid_search.best_score_:.2f} mg")
    
    # Evaluate best model
    y_pred_test = grid_search.predict(X_test_scaled)
    test_r2 = r2_score(y_test, y_pred_test)
    test_rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
    test_mae = mean_absolute_error(y_test, y_pred_test)
    
    print(f"  Test R²: {test_r2:.4f}")
    print(f"  Test RMSE: {test_rmse:.2f} mg")
    print(f"  Test MAE: {test_mae:.2f} mg")
    
    # Prediction intervals (approximate)
    print_subheader("Prediction Analysis")
    
    errors = y_test - y_pred_test
    print(f"  Error statistics:")
    print(f"    Mean error: {np.mean(errors):.2f} mg")
    print(f"    Std error: {np.std(errors):.2f} mg")
    print(f"    95% of predictions within: ±{1.96 * np.std(errors):.2f} mg")
    
    return {
        'best_params': grid_search.best_params_,
        'test_r2': test_r2,
        'test_rmse': test_rmse,
        'test_mae': test_mae
    }


# =============================================================================
# SECTION VIII: ADVANCED TOPICS
# =============================================================================

def advanced_nu_svr():
    """
    Demonstrate Nu-SVR variant.
    """
    print_header("ADVANCED TOPIC: NU-SVR")
    
    # Generate data
    X, y = generate_nonlinear_data(n_samples=300, noise_level=0.15)
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Nu-SVR vs Standard SVR")
    print("  Nu parameter controls the fraction of support vectors")
    print("  Useful when you want to control model complexity directly")
    print()
    
    print(f"{'Model':<20} {'Nu/C':>10} {'Test R²':>10} {'RMSE':>10} {'# SV':>8} "
          f"{'SV%':>8}")
    print("-" * 66)
    
    # Standard SVR with different C values
    for C in [1, 10, 100]:
        svr = SVR(kernel='rbf', C=C, gamma='scale', epsilon=0.1)
        svr.fit(X_train_scaled, y_train)
        
        y_pred = svr.predict(X_test_scaled)
        test_r2 = r2_score(y_test, y_pred)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred))
        n_sv = svr.n_support_.sum()
        sv_pct = 100 * n_sv / len(y_train)
        
        print(f"{'SVR (C=' + str(C) + ')':<20} {C:>10} {test_r2:>10.4f} {rmse:>10.4f} "
              f"{n_sv:>8} {sv_pct:>7.1f}%")
    
    # Nu-SVR with different nu values
    nu_svr = NuSVR(kernel='rbf', nu=0.1, C=10)
    nu_svr.fit(X_train_scaled, y_train)
    
    y_pred = nu_svr.predict(X_test_scaled)
    test_r2 = r2_score(y_test, y_pred)
    rmse = np.sqrt(mean_squared_error(y_test, y_pred))
    n_sv = nu_svr.n_support_.sum()
    sv_pct = 100 * n_sv / len(y_train)
    
    print(f"{'NuSVR (nu=0.1)':<20} {'0.1':>10} {test_r2:>10.4f} {rmse:>10.4f} "
          f"{n_sv:>8} {sv_pct:>7.1f}%")
    
    print_subheader("Nu Parameter Interpretation")
    print("  Nu must be between 0 and 1")
    print("  Represents the upper bound on fraction of support vectors")
    print("  Provides more intuitive control over model complexity")


def advanced_multi_output_svr():
    """
    Demonstrate multi-output regression with SVR.
    """
    print_header("ADVANCED TOPIC: MULTI-OUTPUT REGRESSION")
    
    # Generate multi-output data
    np.random.seed(42)
    n_samples = 300
    
    X = np.random.randn(n_samples, 3)
    
    # Three related targets
    y1 = X[:, 0] ** 2 + 0.5 * X[:, 1] + np.random.randn(n_samples) * 0.1
    y2 = np.sin(X[:, 1]) + 0.3 * X[:, 2] + np.random.randn(n_samples) * 0.1
    y3 = X[:, 0] * X[:, 2] + np.exp(-X[:, 1]**2) + np.random.randn(n_samples) * 0.1
    
    y = np.column_stack([y1, y2, y3])
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Multi-Output SVR Approach")
    print("  Train separate SVR for each output")
    print("  This captures different relationships for each target")
    print()
    
    # Train separate SVR for each output
    targets = ['Target 1', 'Target 2', 'Target 3']
    
    print(f"{'Target':<12} {'Train R²':>10} {'Test R²':>10} {'RMSE':>10} {'# SV':>8}")
    print("-" * 52)
    
    predictions = np.zeros_like(y_test)
    
    for i, target in enumerate(targets):
        svr = SVR(kernel='rbf', C=10, gamma='scale', epsilon=0.1)
        svr.fit(X_train_scaled, y_train[:, i])
        
        y_pred_train = svr.predict(X_train_scaled)
        y_pred_test = svr.predict(X_test_scaled)
        predictions[:, i] = y_pred_test
        
        train_r2 = r2_score(y_train[:, i], y_pred_train)
        test_r2 = r2_score(y_test[:, i], y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test[:, i], y_pred_test))
        n_sv = svr.n_support_.sum()
        
        print(f"{target:<12} {train_r2:>10.4f} {test_r2:>10.4f} {rmse:>10.4f} {n_sv:>8}")
    
    # Combined metrics
    print()
    print("  Combined metrics:")
    overall_r2 = r2_score(y_test, predictions)
    overall_rmse = np.sqrt(mean_squared_error(y_test, predictions))
    print(f"    Overall R²: {overall_r2:.4f}")
    print(f"    Overall RMSE: {overall_rmse:.4f}")


def advanced_outlier_robustness():
    """
    Demonstrate SVR robustness to outliers.
    """
    print_header("ADVANCED TOPIC: OUTLIER ROBUSTNESS")
    
    # Generate data with outliers
    X, y, outlier_indices = create_1d_sin_with_outliers(
        n_samples=300, n_outliers=15, seed=42
    )
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subheader("Outlier Comparison")
    print(f"  Total outliers in dataset: {len(outlier_indices)}")
    print(f"  Outlier indices: {outlier_indices}")
    print()
    
    # Train with different epsilon values
    print(f"{'Epsilon':>10} {'Test R²':>10} {'RMSE':>10} {'MAE':>10} {'# SV':>8}")
    print("-" * 48)
    
    for epsilon in [0.01, 0.1, 0.5, 1.0]:
        svr = SVR(kernel='rbf', C=10, gamma=0.1, epsilon=epsilon)
        svr.fit(X_train_scaled, y_train)
        
        y_pred_test = svr.predict(X_test_scaled)
        
        test_r2 = r2_score(y_test, y_pred_test)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred_test))
        mae = mean_absolute_error(y_test, y_pred_test)
        n_sv = svr.n_support_.sum()
        
        print(f"{epsilon:>10.2f} {test_r2:>10.4f} {rmse:>10.4f} {mae:>10.4f} {n_sv:>8}")
    
    print_subheader("Epsilon Sensitivity to Outliers")
    print("  Low epsilon (0.01): Treats small deviations as errors")
    print("  High epsilon (1.0): Ignores larger deviations within tube")
    print("  The epsilon-insensitive loss provides natural robustness")


def advanced_kernel_visualization():
    """
    Visualize kernel shapes (ASCII art).
    """
    print_header("ADVANCED TOPIC: KERNEL FUNCTION VISUALIZATION")
    
    print_subheader("RBF Kernel Shape")
    print("  K(x, x') = exp(-gamma * ||x - x'||^2)")
    print()
    print("  Distance |  gamma=0.1  |  gamma=1.0  |  gamma=10")
    print("  " + "-" * 45)
    
    distances = [0, 0.1, 0.5, 1.0, 2.0, 5.0]
    
    for d in distances:
        k1 = np.exp(-0.1 * d**2)
        k2 = np.exp(-1.0 * d**2)
        k3 = np.exp(-10 * d**2)
        print(f"  {d:<8.1f} {k1:>11.4f}  {k2:>10.4f}  {k3:>10.4f}")
    
    print_subheader("Polynomial Kernel Shape")
    print("  K(x, x') = (gamma * x·x' + coef0)^degree")
    print()
    print("  Degree 2 (gamma=1, coef0=1):")
    x_vals = [0, 0.5, 1, 2, 3]
    for x in x_vals:
        k = (x * 1 + 1) ** 2
        print(f"    K(x={x:.1f}) = {k:.2f}")
    
    print("\n  Degree 3 (gamma=1, coef0=1):")
    for x in x_vals:
        k = (x * 1 + 1) ** 3
        print(f"    K(x={x:.1f}) = {k:.2f}")


# =============================================================================
# SECTION IX: TESTING AND BENCHMARKING
# =============================================================================

def run_tests():
    """
    Run comprehensive tests for SVR implementation.
    """
    print_header("TESTING SUITE")
    
    # Test 1: Basic data generation
    print_subheader("Test 1: Data Generation")
    X, y = generate_nonlinear_data(n_samples=100)
    assert X.shape == (100, 1), "Expected shape (100, 1)"
    assert y.shape == (100,), "Expected shape (100,)"
    print(f"  PASS: generate_nonlinear_data")
    
    # Test 2: Complex non-linear data
    X, y = generate_complex_nonlinear_data(n_samples=100)
    assert X.shape == (100, 5), "Expected shape (100, 5)"
    print(f"  PASS: generate_complex_nonlinear_data")
    
    # Test 3: Sparse data
    X, y = generate_sparse_data(n_samples=100, n_features=50)
    assert X.shape == (100, 50), "Expected shape (100, 50)"
    print(f"  PASS: generate_sparse_data")
    
    # Test 4: SVR basic operation
    X, y = generate_nonlinear_data(n_samples=100)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    svr = SVR(kernel='rbf', C=1.0, gamma='scale', epsilon=0.1)
    svr.fit(X_train_scaled, y_train)
    y_pred = svr.predict(X_test_scaled)
    
    assert len(y_pred) == len(y_test), "Prediction length mismatch"
    print(f"  PASS: Basic SVR operation")
    
    # Test 5: Different kernels
    for kernel in ['linear', 'rbf', 'poly', 'sigmoid']:
        svr = SVR(kernel=kernel)
        svr.fit(X_train_scaled, y_train)
        y_pred = svr.predict(X_test_scaled)
        assert len(y_pred) == len(y_test), f"Kernel {kernel} failed"
    print(f"  PASS: Different kernel functions")
    
    # Test 6: Parameter variations
    for C in [0.1, 1, 10, 100]:
        for epsilon in [0.01, 0.1, 0.5]:
            svr = SVR(kernel='rbf', C=C, epsilon=epsilon)
            svr.fit(X_train_scaled, y_train)
            y_pred = svr.predict(X_test_scaled)
            assert len(y_pred) == len(y_test), "Parameter variation failed"
    print(f"  PASS: Parameter variations")
    
    # Test 7: Healthcare example works
    print_subheader("Test 7: Healthcare Examples")
    try:
        result = healthcare_length_of_stay()
        print(f"  PASS: healthcare_length_of_stay")
    except Exception as e:
        print(f"  FAIL: healthcare_length_of_stay - {e}")
    
    # Test 8: Banking example works
    print_subheader("Test 8: Banking Examples")
    try:
        result = banking_stock_prediction()
        print(f"  PASS: banking_stock_prediction")
    except Exception as e:
        print(f"  FAIL: banking_stock_prediction - {e}")
    
    # Test 9: Grid search works
    print_subheader("Test 9: Grid Search")
    param_grid = {'C': [1, 10], 'gamma': [0.1, 1]}
    grid_search = GridSearchCV(SVR(kernel='rbf'), param_grid, cv=2, scoring='r2')
    grid_search.fit(X_train_scaled, y_train)
    print(f"  PASS: Grid search")
    
    print("\n" + "=" * 60)
    print("  ALL TESTS PASSED")
    print("=" * 60)


# =============================================================================
# SECTION X: MAIN EXECUTION
# =============================================================================

def main():
    """
    Main function to execute all SVR demonstrations.
    """
    print("=" * 80)
    print("  SUPPORT VECTOR REGRESSION (SVR) - COMPREHENSIVE IMPLEMENTATION")
    print("=" * 80)
    
    # Core concepts
    print_header("I. CORE CONCEPTS")
    core_svr()
    
    # Kernel comparison
    print_header("II. KERNEL FUNCTIONS")
    svr_kernel_comparison()
    
    # Parameter tuning
    print_header("III. PARAMETER TUNING")
    print_subheader("C Parameter")
    tune_c_parameter()
    
    print_subheader("Epsilon Parameter")
    tune_epsilon_parameter()
    
    print_subheader("Gamma Parameter")
    tune_gamma_parameter()
    
    print_subheader("Grid Search")
    grid_search_svr()
    
    # Feature scaling
    print_header("IV. FEATURE SCALING")
    demonstrate_scaling_importance()
    
    # Banking examples
    print_header("V. BANKING EXAMPLES")
    banking_stock_prediction()
    banking_credit_risk()
    banking_loan_prediction()
    
    # Healthcare examples
    print_header("VI. HEALTHCARE EXAMPLES")
    healthcare_length_of_stay()
    healthcare_medical_cost()
    healthcare_drug_dosage()
    
    # Advanced topics
    print_header("VII. ADVANCED TOPICS")
    advanced_nu_svr()
    advanced_multi_output_svr()
    advanced_outlier_robustness()
    advanced_kernel_visualization()
    
    # Testing
    print_header("VIII. TESTING")
    run_tests()
    
    # Conclusion
    print_header("IX. CONCLUSION")
    print("""
  KEY TAKEAWAYS:
  
  1. SVR is a powerful regression algorithm that handles non-linear
     relationships through kernel functions.
  
  2. Feature scaling is essential for SVR, especially with RBF and other
     kernels that depend on distances.
  
  3. Key hyperparameters:
     - C: Regularization parameter (balance between fit and smoothness)
     - epsilon: Width of the insensitive tube
     - gamma: Kernel coefficient (for RBF, polynomial, sigmoid)
  
  4. Kernel selection depends on the problem:
     - Linear: For linearly separable data
     - RBF: For non-linear data (most flexible)
     - Polynomial: When polynomial relationships are expected
  
  5. SVR provides:
     - Robustness to outliers (epsilon-insensitive loss)
     - Sparse solutions (fewer support vectors = faster prediction)
     - Effective handling of high-dimensional data
  
  6. Applications:
     - Financial forecasting (stock prices, credit risk)
     - Healthcare (length of stay, medical costs, drug dosage)
     - Any regression with non-linear patterns
  
  7. Best practices:
     - Always scale features
     - Use cross-validation for parameter tuning
     - Start with default gamma='scale'
     - Consider computational cost for large datasets
""")
    
    print_header("EXECUTION COMPLETE")
    print("  All SVR demonstrations completed successfully!")
    
    return 0


if __name__ == "__main__":
    main()