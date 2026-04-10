# Topic: Cross Validation Strategies
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Cross Validation Strategies

I. INTRODUCTION
    Cross-validation is a fundamental technique in machine learning for assessing how well 
    a model generalizes to independent datasets. It provides a more reliable estimate of model 
    performance than a simple train/test split by using multiple random partitions of the data 
    for training and validation. This approach helps detect overfitting, underfitting, and 
    provides insight into model stability across different data subsets.
    
    The importance of cross-validation cannot be overstated:
    - Provides robust performance estimates independent of training data
    - Helps identify models that don't generalize well
    - Enables hyperparameter tuning without data leakage
    - Essential for model selection and comparison
    - Required for production-ready ML pipelines

II. CORE_CONCEPTS
    - K-Fold CV: Partition data into K equal folds, train K times
    - Stratified K-Fold: Preserve class distribution in each fold
    - Leave-One-Out (LOO): Use single sample as validation set
    - Time Series Split: Handle temporal dependencies
    - Nested CV: Avoid data leakage in hyperparameter tuning
    - Bias-Variance Tradeoff: Understanding the tradeoff
    - Choosing K: Impact of K on model evaluation

III. IMPLEMENTATION
    Comprehensive implementations covering all major cross-validation strategies.

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Credit scoring model evaluation with CV
    - Healthcare: Medical diagnosis model validation

V. OUTPUT_RESULTS
    Detailed metrics, visualizations, and analysis of CV results.

VI. TESTING
    Comprehensive testing across different scenarios and edge cases.

VII. ADVANCED_TOPICS
    - Repeated Stratified K-Fold for stable estimates
    - Group K-Fold for related samples
    - Custom CV strategies for specific domains
    - Cross-validation with feature selection
    - Permutation tests for model comparison

VIII. CONCLUSION
    Summary, best practices, and practical considerations.
"""

# Import necessary libraries
import numpy as np
import pandas as pd
from sklearn.datasets import make_classification, make_regression
from sklearn.model_selection import (
    train_test_split, KFold, StratifiedKFold, LeaveOneOut, 
    LeavePOut, GroupKFold, RepeatedStratifiedKFold, TimeSeriesSplit,
    cross_val_score, cross_validate, GridSearchCV, RandomizedSearchCV
)
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.linear_model import LogisticRegression, Ridge
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    roc_auc_score, confusion_matrix, classification_report,
    mean_squared_error, mean_absolute_error, r2_score,
    make_scorer, get_scorer_names
)
import warnings
warnings.filterwarnings('ignore')


def generate_classification_data(n_samples=500, n_features=10, n_informative=5,
                              n_clusters_per_class=2, random_state=42,
                              class_sep=1.0, weights=None):
    """
    Generate synthetic classification data for cross-validation demonstrations.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Total number of features
    n_informative : int
        Number of informative features
    n_clusters_per_class : int
        Number of clusters per class
    random_state : int
        Random seed for reproducibility
    class_sep : float
        Separation between classes (lower = more overlap)
    weights : list or None
        Class weights for imbalanced data
        
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    feature_names : list
        Names of features
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_features - n_informative,
        n_clusters_per_class=n_clusters_per_class,
        random_state=random_state,
        flip_y=0.05,
        class_sep=class_sep,
        weights=weights
    )
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    
    return X, y, feature_names


def generate_imbalanced_data(n_samples=1000, n_features=10, imbalance_ratio=0.1, random_state=42):
    """
    Generate imbalanced classification data for stratified CV demonstrations.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Total number of features
    imbalance_ratio : float
        Ratio of minority class (e.g., 0.1 = 10% minority)
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    feature_names : list
        Names of features
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=5,
        n_redundant=n_features - 5,
        n_clusters_per_class=2,
        random_state=random_state,
        flip_y=0.02,
        weights=[1 - imbalance_ratio, imbalance_ratio]
    )
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    
    return X, y, feature_names


def generate_time_series_data(n_samples=500, n_features=5, random_state=42):
    """
    Generate synthetic time series data for time series CV demonstrations.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples (time points) to generate
    n_features : int
        Number of features
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    time_index : array-like
        Time indices for temporal ordering
    """
    np.random.seed(random_state)
    
    time_index = np.arange(n_samples)
    
    X = np.random.randn(n_samples, n_features)
    
    for i in range(1, n_samples):
        X[i] = 0.7 * X[i-1] + np.random.randn(n_features) * 0.3
    
    y = np.sin(time_index / 10) + np.random.randn(n_samples) * 0.1
    y = (y > 0).astype(int)
    
    return X, y, time_index


def generate_grouped_data(n_samples=300, n_features=10, n_groups=10, random_state=42):
    """
    Generate grouped data for Group K-Fold demonstrations.
    Samples in the same group should not be split across train/test.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features
    n_groups : int
        Number of groups
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    groups : array-like
        Group assignments for each sample
    """
    X, y, _ = generate_classification_data(
        n_samples=n_samples,
        n_features=n_features,
        random_state=random_state
    )
    
    samples_per_group = n_samples // n_groups
    groups = np.repeat(np.arange(n_groups), samples_per_group)
    
    if len(groups) < n_samples:
        remaining = n_samples - len(groups)
        groups = np.concatenate([groups, np.zeros(remaining, dtype=int)])
    
    return X, y, groups


def basic_kfold_cv(X, y, n_splits=5, random_state=42):
    """
    Basic K-Fold cross-validation implementation.
    
    K-Fold divides the data into K equal-sized folds. For each fold:
    1. Use the fold as the validation set
    2. Train on all other K-1 folds
    
    This provides K performance estimates that are averaged to get
    a more robust estimate of model performance.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    n_splits : int
        Number of folds
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    fold_scores : list
        Accuracy for each fold
    cv_scores : list
        All cross-validation scores
    trained_models : list
        List of trained models
    """
    print("\n" + "="*70)
    print("BASIC K-FOLD CROSS-VALIDATION")
    print("="*70)
    print(f"\nDataset shape: {X.shape}")
    print(f"Number of folds: {n_splits}")
    print(f"Training samples per fold: ~{X.shape[0] * (n_splits-1) // n_splits}")
    print(f"Validation samples per fold: ~{X.shape[0] // n_splits}")
    
    kfold = KFold(n_splits=n_splits, shuffle=True, random_state=random_state)
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    cv_scores = []
    fold_scores = []
    trained_models = []
    
    print("\nFold-wise results:")
    print("-" * 50)
    
    for fold_idx, (train_idx, val_idx) in enumerate(kfold.split(X), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        
        cv_scores.append(score)
        fold_scores.append({
            'fold': fold_idx,
            'train_size': len(train_idx),
            'val_size': len(val_idx),
            'accuracy': score
        })
        trained_models.append(model)
        
        print(f"Fold {fold_idx}: Train size = {len(train_idx):4d}, "
              f"Val size = {len(val_idx):4d}, Accuracy = {score:.4f}")
    
    mean_score = np.mean(cv_scores)
    std_score = np.std(cv_scores)
    
    print("-" * 50)
    print(f"\nMean Accuracy: {mean_score:.4f} (+/- {std_score:.4f})")
    print(f"Min Accuracy: {min(cv_scores):.4f}")
    print(f"Max Accuracy: {max(cv_scores):.4f}")
    
    return fold_scores, cv_scores, trained_models


def stratified_kfold_cv(X, y, n_splits=5, random_state=42):
    """
    Stratified K-Fold cross-validation implementation.
    
    Stratified K-Fold ensures that each fold has the same proportion 
    of samples from each class as the original dataset. This is 
    especially important for imbalanced datasets.
    
    Benefits:
    - Maintains class distribution across folds
    - More reliable performance estimates for imbalanced data
    - Reduces variance in performance estimates
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    n_splits : int
        Number of folds
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    fold_info : list
        Information about each fold
    cv_scores : list
        All cross-validation scores
    class_distributions : dict
        Class distributions in each fold
    """
    print("\n" + "="*70)
    print("STRATIFIED K-FOLD CROSS-VALIDATION")
    print("="*70)
    
    unique, counts = np.unique(y, return_counts=True)
    print(f"\nOriginal class distribution:")
    for cls, cnt in zip(unique, counts):
        print(f"  Class {cls}: {cnt} samples ({cnt/len(y)*100:.1f}%)")
    
    print(f"\nDataset shape: {X.shape}")
    print(f"Number of folds: {n_splits}")
    
    skfold = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=random_state)
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    cv_scores = []
    fold_info = []
    class_distributions = []
    
    print("\nFold-wise results:")
    print("-" * 50)
    
    for fold_idx, (train_idx, val_idx) in enumerate(skfold.split(X, y), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        train_unique, train_counts = np.unique(y_train, return_counts=True)
        val_unique, val_counts = np.unique(y_val, return_counts=True)
        
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        
        cv_scores.append(score)
        
        fold_info.append({
            'fold': fold_idx,
            'train_size': len(train_idx),
            'val_size': len(val_idx),
            'accuracy': score
        })
        
        class_distributions.append({
            'train': dict(zip(train_unique, train_counts)),
            'val': dict(zip(val_unique, val_counts))
        })
        
        print(f"\nFold {fold_idx}:")
        print(f"  Train: {len(train_idx)} samples")
        print(f"  Val:   {len(val_idx)} samples")
        print(f"  Train class distribution: {dict(zip(train_unique, train_counts))}")
        print(f"  Val class distribution:  {dict(zip(val_unique, val_counts))}")
        print(f"  Accuracy: {score:.4f}")
    
    mean_score = np.mean(cv_scores)
    std_score = np.std(cv_scores)
    
    print("-" * 50)
    print(f"\nMean Accuracy: {mean_score:.4f} (+/- {std_score:.4f})")
    print(f"Min Accuracy: {min(cv_scores):.4f}")
    print(f"Max Accuracy: {max(cv_scores):.4f}")
    
    return fold_info, cv_scores, class_distributions


def compare_kfold_stratified(X, y, n_splits=5, random_state=42):
    """
    Compare K-Fold vs Stratified K-Fold on imbalanced data.
    
    This demonstrates why Stratified K-Fold is preferred for 
    imbalanced datasets.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix (imbalanced)
    y : array-like
        Target variable
    n_splits : int
        Number of folds
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    comparison_results : dict
        Results from both methods
    """
    print("\n" + "="*70)
    print("COMPARISON: K-FOLD vs STRATIFIED K-FOLD (Imbalanced Data)")
    print("="*70)
    
    unique, counts = np.unique(y, return_counts=True)
    print(f"\nOriginal class distribution:")
    for cls, cnt in zip(unique, counts):
        print(f"  Class {cls}: {cnt} samples ({cnt/len(y)*100:.1f}%)")
    
    kfold = KFold(n_splits=n_splits, shuffle=True, random_state=random_state)
    skfold = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=random_state)
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    kfold_scores = []
    skfold_scores = []
    
    kfold_class_var = []
    skfold_class_var = []
    
    print("\n--- K-Fold Results ---")
    for fold_idx, (train_idx, val_idx) in enumerate(kfold.split(X), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        val_unique, val_counts = np.unique(y_val, return_counts=True)
        class_ratio = val_counts[0] / val_counts[1] if len(val_counts) > 1 else float('inf')
        kfold_class_var.append(abs(class_ratio - counts[0]/counts[1]))
        
        model.fit(X_train, y_train)
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        kfold_scores.append(score)
        
        print(f"Fold {fold_idx}: Accuracy = {score:.4f}, Class ratio in val = {class_ratio:.2f}")
    
    print(f"\nK-Fold Mean Accuracy: {np.mean(kfold_scores):.4f} (+/- {np.std(kfold_scores):.4f})")
    print(f"K-Fold Class Ratio Variance: {np.mean(kfold_class_var):.2f}")
    
    print("\n--- Stratified K-Fold Results ---")
    for fold_idx, (train_idx, val_idx) in enumerate(skfold.split(X, y), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        val_unique, val_counts = np.unique(y_val, return_counts=True)
        class_ratio = val_counts[0] / val_counts[1] if len(val_counts) > 1 else float('inf')
        skfold_class_var.append(abs(class_ratio - counts[0]/counts[1]))
        
        model.fit(X_train, y_train)
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        skfold_scores.append(score)
        
        print(f"Fold {fold_idx}: Accuracy = {score:.4f}, Class ratio in val = {class_ratio:.2f}")
    
    print(f"\nStratified K-Fold Mean Accuracy: {np.mean(skfold_scores):.4f} (+/- {np.std(skfold_scores):.4f})")
    print(f"Stratified K-Fold Class Ratio Variance: {np.mean(skfold_class_var):.2f}")
    
    comparison_results = {
        'kfold_scores': kfold_scores,
        'skfold_scores': skfold_scores,
        'kfold_mean': np.mean(kfold_scores),
        'skfold_mean': np.mean(skfold_scores),
        'kfold_std': np.std(kfold_scores),
        'skfold_std': np.std(skfold_scores),
        'kfold_class_var': np.mean(kfold_class_var),
        'skfold_class_var': np.mean(skfold_class_var)
    }
    
    return comparison_results


def leave_one_out_cv(X, y, random_state=42):
    """
    Leave-One-Out cross-validation implementation.
    
    LOO uses a single sample as validation set and trains on 
    all other samples. This provides the most thorough evaluation
    but is computationally expensive for large datasets.
    
    Use cases:
    - Small datasets where every sample is precious
    - When you need the most accurate performance estimate
    - Benchmarking against other methods
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    loo_scores : list
        All cross-validation scores
    mean_score : float
        Mean accuracy
    std_score : float
        Standard deviation of scores
    """
    print("\n" + "="*70)
    print("LEAVE-ONE-OUT CROSS-VALIDATION")
    print("="*70)
    print(f"\nDataset shape: {X.shape}")
    print(f"Number of iterations: {X.shape[0]}")
    
    loo = LeaveOneOut()
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    cv_scores = []
    
    print("\nRunning LOO CV (showing first 10 and last 10 folds)...")
    print("-" * 50)
    
    for fold_idx, (train_idx, val_idx) in enumerate(loo.split(X), 1):
        if fold_idx <= 5 or fold_idx > X.shape[0] - 5:
            print(f"Training on {len(train_idx)} samples, Validating on 1 sample...")
        
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        
        cv_scores.append(score)
    
    mean_score = np.mean(cv_scores)
    std_score = np.std(cv_scores)
    
    print("-" * 50)
    print(f"\nMean Accuracy: {mean_score:.4f} (+/- {std_score:.4f})")
    print(f"Total correct predictions: {sum(cv_scores)}/{len(cv_scores)}")
    
    return cv_scores, mean_score, std_score


def leave_p_out_cv(X, y, p=5, random_state=42):
    """
    Leave-P-Out cross-validation implementation.
    
    Similar to LOO but leaves P samples out for validation.
    Provides a balance between LOO thoroughness and K-Fold efficiency.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    p : int
        Number of samples to leave out
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    cv_scores : list
        All cross-validation scores
    mean_score : float
        Mean accuracy
    """
    print("\n" + "="*70)
    print(f"LEAVE-{p}-OUT CROSS-VALIDATION")
    print("="*70)
    print(f"\nDataset shape: {X.shape}")
    print(f"Samples to leave out: {p}")
    print(f"Total iterations: {X.shape[0] choose p}")
    
    lpout = LeavePOut(p=p)
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    cv_scores = []
    
    max_iterations = 100
    iteration = 0
    
    print(f"\nRunning Leave-{p}-Out CV (limited to {max_iterations} iterations)...")
    print("-" * 50)
    
    for train_idx, val_idx in lpout.split(X):
        if iteration >= max_iterations:
            break
            
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        
        cv_scores.append(score)
        iteration += 1
    
    mean_score = np.mean(cv_scores)
    std_score = np.std(cv_scores)
    
    print("-" * 50)
    print(f"\nMean Accuracy (from {len(cv_scores)} iterations): {mean_score:.4f} (+/- {std_score:.4f})")
    
    return cv_scores, mean_score


def time_series_cv(X, y, n_splits=5, random_state=42):
    """
    Time Series cross-validation implementation.
    
    Time series data has temporal dependencies, so random splitting
    would cause data leakage. TimeSeriesSplit uses a forward-chaining
    approach where:
    - Fold 1: Train on [1:n-1], validate on [n]
    - Fold 2: Train on [1:n-2], validate on [n-1]
    - etc.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix (ordered by time)
    y : array-like
        Target variable
    n_splits : int
        Number of splits
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    fold_scores : list
        Scores for each fold
    cv_scores : list
        All CV scores
    """
    print("\n" + "="*70)
    print("TIME SERIES CROSS-VALIDATION")
    print("="*70)
    print(f"\nDataset shape: {X.shape}")
    print(f"Number of splits: {n_splits}")
    
    tscv = TimeSeriesSplit(n_splits=n_splits)
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    cv_scores = []
    fold_scores = []
    
    print("\nFold-wise results:")
    print("-" * 50)
    
    for fold_idx, (train_idx, val_idx) in enumerate(tscv.split(X), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        
        cv_scores.append(score)
        
        fold_scores.append({
            'fold': fold_idx,
            'train_size': len(train_idx),
            'val_size': len(val_idx),
            'train_period': f"{train_idx[0]}-{train_idx[-1]}",
            'val_period': f"{val_idx[0]}-{val_idx[-1]}",
            'accuracy': score
        })
        
        print(f"Fold {fold_idx}: Train [{train_idx[0]}:{train_idx[-1]}] "
              f"({len(train_idx):3d} samples), "
              f"Val [{val_idx[0]}:{val_idx[-1]}] "
              f"({len(val_idx):3d} samples), "
              f"Accuracy = {score:.4f}")
    
    mean_score = np.mean(cv_scores)
    std_score = np.std(cv_scores)
    
    print("-" * 50)
    print(f"\nMean Accuracy: {mean_score:.4f} (+/- {std_score:.4f})")
    print(f"Min Accuracy: {min(cv_scores):.4f}")
    print(f"Max Accuracy: {max(cv_scores):.4f}")
    
    return fold_scores, cv_scores


def group_kfold_cv(X, y, groups, n_splits=5, random_state=42):
    """
    Group K-Fold cross-validation implementation.
    
    Group K-Fold ensures that samples from the same group are 
    always in either the training OR validation set, never both.
    
    Use cases:
    - Avoiding data leakage in medical imaging (same patient)
    - Financial data (same customer)
    - Time series with multiple entities
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    groups : array-like
        Group assignments for each sample
    n_splits : int
        Number of folds
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    fold_scores : list
        Scores for each fold
    cv_scores : list
        All CV scores
    """
    print("\n" + "="*70)
    print("GROUP K-FOLD CROSS-VALIDATION")
    print("="*70)
    
    unique_groups = np.unique(groups)
    print(f"\nDataset shape: {X.shape}")
    print(f"Number of unique groups: {len(unique_groups)}")
    print(f"Samples per group: {X.shape[0] // len(unique_groups)}")
    
    gkfold = GroupKFold(n_splits=n_splits)
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    cv_scores = []
    fold_scores = []
    
    print("\nFold-wise results:")
    print("-" * 50)
    
    for fold_idx, (train_idx, val_idx) in enumerate(gkfold.split(X, y, groups), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        train_groups = np.unique(groups[train_idx])
        val_groups = np.unique(groups[val_idx])
        
        overlap = set(train_groups) & set(val_groups)
        
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        
        cv_scores.append(score)
        
        fold_scores.append({
            'fold': fold_idx,
            'train_size': len(train_idx),
            'val_size': len(val_idx),
            'train_groups': len(train_groups),
            'val_groups': len(val_groups),
            'group_overlap': len(overlap),
            'accuracy': score
        })
        
        print(f"Fold {fold_idx}: Train {len(train_idx):3d} samples "
              f"({len(train_groups)} groups), "
              f"Val {len(val_idx):3d} samples "
              f"({len(val_groups)} groups), "
              f"Overlap = {len(overlap)}, "
              f"Accuracy = {score:.4f}")
    
    mean_score = np.mean(cv_scores)
    std_score = np.std(cv_scores)
    
    print("-" * 50)
    print(f"\nMean Accuracy: {mean_score:.4f} (+/- {std_score:.4f})")
    
    return fold_scores, cv_scores


def repeated_stratified_kfold_cv(X, y, n_splits=5, n_repeats=3, random_state=42):
    """
    Repeated Stratified K-Fold cross-validation.
    
    Repeats Stratified K-Fold n times with different randomization
    in each repetition. This provides more stable estimates of
    model performance.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    n_splits : int
        Number of splits per repeat
    n_repeats : int
        Number of times to repeat
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    all_scores : list
        All CV scores across all repeats
    mean_score : float
        Mean accuracy
    summary : dict
        Summary statistics
    """
    print("\n" + "="*70)
    print("REPEATED STRATIFIED K-FOLD CROSS-VALIDATION")
    print("="*70)
    print(f"\nDataset shape: {X.shape}")
    print(f"Number of splits: {n_splits}")
    print(f"Number of repeats: {n_repeats}")
    print(f"Total evaluations: {n_splits * n_repeats}")
    
    rskfold = RepeatedStratifiedKFold(
        n_splits=n_splits,
        n_repeats=n_repeats,
        random_state=random_state
    )
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    cv_scores = []
    
    print("\nResults by repeat:")
    print("-" * 50)
    
    repeat_scores = []
    repeat_num = 1
    
    for fold_idx, (train_idx, val_idx) in enumerate(rskfold.split(X, y), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        
        cv_scores.append(score)
        repeat_scores.append(score)
        
        if fold_idx % n_splits == 0:
            print(f"Repeat {repeat_num}: Mean Accuracy = {np.mean(repeat_scores):.4f}")
            repeat_scores = []
            repeat_num += 1
    
    mean_score = np.mean(cv_scores)
    std_score = np.std(cv_scores)
    
    print("-" * 50)
    print(f"\nOverall Mean Accuracy: {mean_score:.4f}")
    print(f"Standard Deviation: {std_score:.4f}")
    print(f"95% CI: [{mean_score - 1.96*std_score:.4f}, {mean_score + 1.96*std_score:.4f}]")
    
    summary = {
        'mean': mean_score,
        'std': std_score,
        'min': min(cv_scores),
        'max': max(cv_scores),
        'all_scores': cv_scores
    }
    
    return cv_scores, mean_score, summary


def nested_cv_for_model_selection(X, y, param_grid, n_splits=5, random_state=42):
    """
    Nested Cross-Validation for unbiased model selection.
    
    Nested CV performs:
    1. Outer CV: Split data into train/test
    2. Inner CV (on outer train): Hyperparameter tuning
    3. Evaluate on outer test
    
    This prevents data leakage in the model selection process.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    param_grid : dict
        Parameter grid for GridSearchCV
    n_splits : int
        Number of folds for outer CV
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Results from nested CV
    best_params : list
        Best parameters from each outer fold
    outer_scores : list
        Scores on outer validation
    """
    print("\n" + "="*70)
    print("NESTED CROSS-VALIDATION")
    print("="*70)
    print(f"\nDataset shape: {X.shape}")
    print(f"Number of outer folds: {n_splits}")
    print(f"Parameter grid: {param_grid}")
    
    outer_cv = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=random_state)
    
    outer_scores = []
    best_params_list = []
    
    print("\nOuter Fold Results:")
    print("-" * 60)
    
    for fold_idx, (train_idx, val_idx) in enumerate(outer_cv.split(X, y), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        inner_cv = StratifiedKFold(n_splits=3, shuffle=True, random_state=random_state)
        
        model = LogisticRegression(max_iter=1000, random_state=random_state)
        
        grid_search = GridSearchCV(
            model,
            param_grid,
            cv=inner_cv,
            scoring='accuracy',
            n_jobs=-1
        )
        
        grid_search.fit(X_train, y_train)
        
        best_params_list.append(grid_search.best_params_)
        
        y_pred = grid_search.predict(X_val)
        score = accuracy_score(y_val, y_pred)
        
        outer_scores.append(score)
        
        print(f"Fold {fold_idx}: Best params = {grid_search.best_params_}, "
              f"Val Accuracy = {score:.4f}")
    
    mean_score = np.mean(outer_scores)
    std_score = np.std(outer_scores)
    
    print("-" * 60)
    print(f"\nNested CV Mean Accuracy: {mean_score:.4f} (+/- {std_score:.4f})")
    
    results = {
        'outer_scores': outer_scores,
        'best_params_list': best_params_list,
        'mean': mean_score,
        'std': std_score
    }
    
    return results


def analyze_bias_variance(X, y, n_splits=5, random_state=42):
    """
    Analyze bias-variance tradeoff using cross-validation.
    
    Understanding bias and variance helps choose the right model complexity:
    - High bias: Model is too simple (underfitting)
    - High variance: Model is too complex (overfitting)
    
    This analysis shows how different folds give different results,
    indicating model sensitivity to training data.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    n_splits : int
        Number of folds
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    analysis_results : dict
        Bias-variance analysis results
    """
    print("\n" + "="*70)
    print("BIAS-VARIANCE TRADEOFF ANALYSIS")
    print("="*70)
    print(f"\nDataset shape: {X.shape}")
    print(f"Number of folds: {n_splits}")
    
    y_true_mean = np.mean(y)
    
    skfold = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=random_state)
    
    model_simple = LogisticRegression(max_iter=1000, random_state=random_state)
    model_complex = RandomForestClassifier(
        n_estimators=100,
        max_depth=None,
        random_state=random_state
    )
    
    simple_scores = []
    complex_scores = []
    predictions_list = []
    
    print("\nRunning cross-validation with simple and complex models...")
    print("-" * 60)
    
    for fold_idx, (train_idx, val_idx) in enumerate(skfold.split(X, y), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        model_simple.fit(X_train, y_train)
        y_pred_simple = model_simple.predict(X_val)
        score_simple = accuracy_score(y_val, y_pred_simple)
        simple_scores.append(score_simple)
        
        model_complex.fit(X_train, y_train)
        y_pred_complex = model_complex.predict(X_val)
        score_complex = accuracy_score(y_val, y_pred_complex)
        complex_scores.append(score_complex)
        
        predictions_list.append({
            'fold': fold_idx,
            'simple': y_pred_simple,
            'complex': y_pred_complex,
            'val_idx': val_idx,
            'val_y': y_val
        })
        
        print(f"Fold {fold_idx}: Simple = {score_simple:.4f}, Complex = {score_complex:.4f}")
    
    print("\n--- Summary Statistics ---")
    print(f"Simple Model (Logistic Regression):")
    print(f"  Mean: {np.mean(simple_scores):.4f}, Std: {np.std(simple_scores):.4f}")
    print(f"\nComplex Model (Random Forest):")
    print(f"  Mean: {np.mean(complex_scores):.4f}, Std: {np.std(complex_scores):.4f}")
    
    simple_variance = np.var(simple_scores)
    complex_variance = np.var(complex_scores)
    
    print("\n--- Interpretation ---")
    print(f"Simple model variance: {simple_variance:.4f}")
    print(f"Complex model variance: {complex_variance:.4f}")
    
    if simple_variance < complex_variance:
        print("Simple model is MORE stable across folds.")
    else:
        print("Complex model is MORE stable across folds.")
    
    if np.mean(simple_scores) < np.mean(complex_scores):
        print("Complex model has HIGHER average performance.")
        print("However, higher variance suggests potential overfitting.")
    else:
        print("Simple model has HIGHER average performance.")
    
    analysis_results = {
        'simple': {
            'scores': simple_scores,
            'mean': np.mean(simple_scores),
            'std': np.std(simple_scores),
            'variance': simple_variance
        },
        'complex': {
            'scores': complex_scores,
            'mean': np.mean(complex_scores),
            'std': np.std(complex_scores),
            'variance': complex_variance
        }
    }
    
    return analysis_results


def choose_optimal_k(X, y, max_k=10, random_state=42):
    """
    Analyze the effect of K choice on cross-validation results.
    
    Choosing the right K is important:
    - Small K: Lower bias, higher variance (more training per fold)
    - Large K: Higher bias, lower variance (less training per fold)
    
    Common recommendations:
    - K = 5 or 10 (most common)
    - Use larger K for smaller datasets
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    max_k : int
        Maximum K to evaluate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Results for different K values
    """
    print("\n" + "="*70)
    print("CHOOSING OPTIMAL K FOR CROSS-VALIDATION")
    print("="*70)
    print(f"\nDataset shape: {X.shape}")
    print(f"Testing K from 2 to {max_k}")
    
    results = {
        'k_values': [],
        'mean_scores': [],
        'std_scores': [],
        'train_sizes': []
    }
    
    for k in range(2, max_k + 1):
        cv = StratifiedKFold(n_splits=k, shuffle=True, random_state=random_state)
        
        model = LogisticRegression(max_iter=1000, random_state=random_state)
        
        cv_scores = cross_val_score(model, X, y, cv=cv, scoring='accuracy')
        
        train_size = X.shape[0] * (k - 1) // k
        
        results['k_values'].append(k)
        results['mean_scores'].append(np.mean(cv_scores))
        results['std_scores'].append(np.std(cv_scores))
        results['train_sizes'].append(train_size)
        
        print(f"K = {k:2d}: Mean Accuracy = {np.mean(cv_scores):.4f} "
              f"(+/- {np.std(cv_scores):.4f}), "
              f"Train size = {train_size}")
    
    best_k = results['k_values'][np.argmax(results['mean_scores'])]
    
    print("\n--- Summary ---")
    print(f"Best K (by mean accuracy): {best_k}")
    print(f"Note: K=5 or K=10 are commonly recommended")
    print("      Smaller K: More training data per fold, less variance in estimate")
    print("      Larger K: Better approximation to full dataset, more variance in estimate")
    
    return results


def banking_credit_scoring_cv():
    """
    Banking example: Credit scoring model evaluation using cross-validation.
    
    This demonstrates how to properly evaluate a credit scoring model
    using cross-validation to ensure reliable performance estimates.
    
    Use case: Bank wants to evaluate a logistic regression model
    for credit approval prediction.
    """
    print("\n" + "="*80)
    print("BANKING EXAMPLE: CREDIT SCORING MODEL EVALUATION")
    print("="*80)
    print("\nScenario: A bank wants to evaluate a credit scoring model")
    print("to predict loan default for risk assessment.")
    
    np.random.seed(42)
    
    n_samples = 1000
    
    credit_score = np.random.normal(650, 100, n_samples)
    income = np.random.normal(50000, 15000, n_samples)
    debt_to_income = np.random.exponential(0.3, n_samples)
    employment_years = np.random.exponential(5, n_samples)
    
    credit_score = (credit_score - 400) / (850 - 400)
    credit_score = np.clip(credit_score, 0, 1)
    
    income = np.log1p(income) / 10
    
    default_prob = (
        0.1 +
        0.3 * (1 - credit_score) +
        0.2 * debt_to_income +
        0.05 * (income < 0.5) +
        0.05 * (employment_years < 1)
    )
    
    default_prob = np.clip(default_prob, 0.05, 0.95)
    
    y = (np.random.random(n_samples) < default_prob).astype(int)
    
    X = np.column_stack([
        credit_score,
        income,
        debt_to_income,
        employment_years,
        np.random.random(n_samples),
        np.random.random(n_samples),
        np.random.random(n_samples),
        np.random.random(n_samples),
        np.random.random(n_samples),
        np.random.random(n_samples)
    ])
    
    print(f"\nGenerated dataset:")
    print(f"  Total samples: {len(y)}")
    print(f"  Features: {X.shape[1]}")
    print(f"  Default rate: {np.mean(y)*100:.1f}%")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print("\n" + "-" * 60)
    print("EVALUATING WITH DIFFERENT CV STRATEGIES")
    print("-" * 60)
    
    print("\n1. Basic K-Fold CV (5-fold):")
    kfold = KFold(n_splits=5, shuffle=True, random_state=42)
    model = LogisticRegression(max_iter=1000, random_state=42)
    cv_scores = cross_val_score(model, X_scaled, y, cv=kfold, scoring='accuracy')
    print(f"   Accuracy: {np.mean(cv_scores):.4f} (+/- {np.std(cv_scores):.4f})")
    
    print("\n2. Stratified K-Fold CV (5-fold):")
    skfold = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
    cv_scores = cross_val_score(model, X_scaled, y, cv=skfold, scoring='accuracy')
    print(f"   Accuracy: {np.mean(cv_scores):.4f} (+/- {np.std(cv_scores):.4f})")
    
    print("\n3. Repeated Stratified K-Fold (5-fold, 3 repeats):")
    rskfold = RepeatedStratifiedKFold(n_splits=5, n_repeats=3, random_state=42)
    cv_scores = cross_val_score(model, X_scaled, y, cv=rskfold, scoring='accuracy')
    print(f"   Accuracy: {np.mean(cv_scores):.4f} (+/- {np.std(cv_scores):.4f})")
    
    print("\n4. Multiple metrics with Stratified K-Fold:")
    cv_results = cross_validate(
        model, X_scaled, y,
        cv=skfold,
        scoring=['accuracy', 'precision', 'recall', 'f1', 'roc_auc'],
        return_train_score=True
    )
    
    print(f"   Accuracy:  {np.mean(cv_results['test_accuracy']):.4f} (+/- {np.std(cv_results['test_accuracy']):.4f})")
    print(f"   Precision: {np.mean(cv_results['test_precision']):.4f} (+/- {np.std(cv_results['test_precision']):.4f})")
    print(f"   Recall:   {np.mean(cv_results['test_recall']):.4f} (+/- {np.std(cv_results['test_recall']):.4f})")
    print(f"   F1:       {np.mean(cv_results['test_f1']):.4f} (+/- {np.std(cv_results['test_f1']):.4f})")
    print(f"   ROC-AUC:  {np.mean(cv_results['test_roc_auc']):.4f} (+/- {np.std(cv_results['test_roc_auc']):.4f})")
    
    print("\n" + "-" * 60)
    print("MODEL COMPARISON WITH CROSS-VALIDATION")
    print("-" * 60)
    
    models = {
        'Logistic Regression': LogisticRegression(max_iter=1000, random_state=42),
        'Random Forest': RandomForestClassifier(n_estimators=100, random_state=42),
        'Gradient Boosting': GradientBoostingClassifier(n_estimators=100, random_state=42)
    }
    
    print("\nComparing different models using 5-fold Stratified CV:")
    print("-" * 50)
    
    model_results = {}
    
    for name, model in models.items():
        cv_scores = cross_val_score(model, X_scaled, y, cv=skfold, scoring='accuracy')
        model_results[name] = {
            'mean': np.mean(cv_scores),
            'std': np.std(cv_scores),
            'scores': cv_scores
        }
        print(f"{name:25s}: {np.mean(cv_scores):.4f} (+/- {np.std(cv_scores):.4f})")
    
    best_model = max(model_results, key=lambda x: model_results[x]['mean'])
    print(f"\nBest model: {best_model}")
    
    return model_results


def medical_diagnosis_cv():
    """
    Healthcare example: Medical diagnosis model evaluation using CV.
    
    This demonstrates cross-validation for medical diagnosis models
    where class imbalance is common and stratification is critical.
    
    Use case: Hospital wants to evaluate a model for early detection
    of a rare medical condition.
    """
    print("\n" + "="*80)
    print("HEALTHCARE EXAMPLE: MEDICAL DIAGNOSIS MODEL EVALUATION")
    print("="*80)
    print("\nScenario: Hospital wants to evaluate a diagnosis model")
    print("for early detection of a rare medical condition.")
    
    np.random.seed(42)
    
    n_samples = 2000
    prevalence = 0.05  # 5% prevalence (rare condition)
    
    biomarkers = np.random.randn(n_samples, 10)
    
    case_biomarkers = np.random.randn(int(n_samples * prevalence), 10) + 0.5
    control_biomarkers = np.random.randn(n_samples - int(n_samples * prevalence), 10)
    
    X = np.vstack([case_biomarkers, control_biomarkers])
    
    y = np.concatenate([
        np.ones(int(n_samples * prevalence)),
        np.zeros(n_samples - int(n_samples * prevalence))
    ])
    
    shuffle_idx = np.random.permutation(n_samples)
    X = X[shuffle_idx]
    y = y[shuffle_idx]
    
    print(f"\nGenerated dataset:")
    print(f"  Total samples: {n_samples}")
    print(f"  Prevalence: {prevalence*100:.1f}%")
    print(f"  Positive cases: {int(n_samples * prevalence)}")
    print(f"  Negative cases: {n_samples - int(n_samples * prevalence)}")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print("\n" + "-" * 60)
    print("EVALUATING WITH STRATIFIED CV (CRITICAL FOR IMBALANCED DATA)")
    print("-" * 60)
    
    skfold = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
    model = LogisticRegression(max_iter=1000, random_state=42)
    
    print("\nUsing 5-fold Stratified CV with multiple metrics:")
    print("-" * 50)
    
    cv_results = cross_validate(
        model, X_scaled, y,
        cv=skfold,
        scoring=['accuracy', 'precision', 'recall', 'f1', 'roc_auc', 'average_precision'],
        return_train_score=False
    )
    
    print(f"Accuracy:        {np.mean(cv_results['test_accuracy']):.4f} (+/- {np.std(cv_results['test_accuracy']):.4f})")
    print(f"Precision:       {np.mean(cv_results['test_precision']):.4f} (+/- {np.std(cv_results['test_precision']):.4f})")
    print(f"Recall:          {np.mean(cv_results['test_recall']):.4f} (+/- {np.std(cv_results['test_recall']):.4f})")
    print(f"F1 Score:       {np.mean(cv_results['test_f1']):.4f} (+/- {np.std(cv_results['test_f1']):.4f})")
    print(f"ROC-AUC:         {np.mean(cv_results['test_roc_auc']):.4f} (+/- {np.std(cv_results['test_roc_auc']):.4f})")
    print(f"Average Precision: {np.mean(cv_results['test_average_precision']):.4f} (+/- {np.std(cv_results['test_average_precision']):.4f})")
    
    print("\n" + "-" * 60)
    print("COMPARING STRATEGIES: REGULAR VS REPEATED CV")
    print("-" * 60)
    
    print("\n1. Standard 5-fold Stratified CV:")
    cv_scores = cross_val_score(model, X_scaled, y, cv=skfold, scoring='roc_auc')
    print(f"   ROC-AUC: {np.mean(cv_scores):.4f} (+/- {np.std(cv_scores):.4f})")
    print(f"   Individual folds: {[f'{s:.4f}' for s in cv_scores]}")
    
    print("\n2. Repeated 5-fold Stratified CV (5 repeats):")
    rskfold = RepeatedStratifiedKFold(n_splits=5, n_repeats=5, random_state=42)
    cv_scores_repeated = cross_val_score(model, X_scaled, y, cv=rskfold, scoring='roc_auc')
    print(f"   ROC-AUC: {np.mean(cv_scores_repeated):.4f} (+/- {np.std(cv_scores_repeated):.4f})")
    print(f"   95% CI: [{np.mean(cv_scores_repeated) - 1.96*np.std(cv_scores_repeated):.4f}, "
          f"{np.mean(cv_scores_repeated) + 1.96*np.std(cv_scores_repeated):.4f}]")
    
    print("\n" + "-" * 60)
    print("MODEL SELECTION FOR MEDICAL DIAGNOSIS")
    print("-" * 60)
    
    models = {
        'Logistic Regression': LogisticRegression(max_iter=1000, random_state=42),
        'Random Forest': RandomForestClassifier(n_estimators=100, random_state=42, class_weight='balanced'),
        'Gradient Boosting': GradientBoostingClassifier(n_estimators=100, random_state=42)
    }
    
    print("\nComparing models (using ROC-AUC for imbalanced data evaluation):")
    print("-" * 50)
    
    model_comparison = {}
    
    for name, model in models.items():
        cv_scores = cross_val_score(model, X_scaled, y, cv=skfold, scoring='roc_auc')
        model_comparison[name] = {
            'mean': np.mean(cv_scores),
            'std': np.std(cv_scores),
            'scores': cv_scores
        }
        print(f"{name:25s}: {np.mean(cv_scores):.4f} (+/- {np.std(cv_scores):.4f})")
    
    best_model = max(model_comparison, key=lambda x: model_comparison[x]['mean'])
    print(f"\nBest model for medical diagnosis: {best_model}")
    print("\nNote: For medical diagnosis, high recall is critical to avoid missing cases.")
    print("      Consider using class_weight='balanced' or adjusting decision threshold.")
    
    return model_comparison


def cross_validate_with_feature_selection(X, y, n_splits=5, random_state=42):
    """
    Cross-validation combined with feature selection.
    
    IMPORTANT: Feature selection must be done WITHIN each fold
    to avoid data leakage!
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    n_splits : int
        Number of folds
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Results with and without proper feature selection
    """
    print("\n" + "="*80)
    print("CROSS-VALIDATION WITH FEATURE SELECTION")
    print("="*80)
    
    print("\nThis demonstrates a common mistake and the correct approach.")
    
    from sklearn.feature_selection import SelectKBest, f_classif
    
    skfold = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=random_state)
    
    print("\n--- WRONG APPROACH: Feature selection on full data before CV ---")
    print("(This causes data leakage!)")
    
    selector_full = SelectKBest(f_classif, k=5)
    X_selected = selector_full.fit_transform(X, y)
    
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    cv_scores_wrong = cross_val_score(model, X_selected, y, cv=skfold, scoring='accuracy')
    
    print(f"Accuracy: {np.mean(cv_scores_wrong):.4f} (+/- {np.std(cv_scores_wrong):.4f})")
    print("Note: This estimate is OPTIMISTICALLY BIASED due to data leakage!")
    
    print("\n--- CORRECT APPROACH: Feature selection within each fold ---")
    
    cv_scores_correct = []
    
    for fold_idx, (train_idx, val_idx) in enumerate(skfold.split(X, y), 1):
        X_train, X_val = X[train_idx], X[val_idx]
        y_train, y_val = y[train_idx], y[val_idx]
        
        selector = SelectKBest(f_classif, k=5)
        X_train_selected = selector.fit_transform(X_train, y_train)
        X_val_selected = selector.transform(X_val)
        
        model = LogisticRegression(max_iter=1000, random_state=random_state)
        model.fit(X_train_selected, y_train)
        
        y_pred = model.predict(X_val_selected)
        score = accuracy_score(y_val, y_pred)
        cv_scores_correct.append(score)
        
        print(f"Fold {fold_idx}: Accuracy = {score:.4f}")
    
    mean_correct = np.mean(cv_scores_correct)
    std_correct = np.std(cv_scores_correct)
    
    print(f"\nMean Accuracy: {mean_correct:.4f} (+/- {std_correct:.4f})")
    
    print("\n--- COMPARISON ---")
    print(f"Wrong approach:    {np.mean(cv_scores_wrong):.4f}")
    print(f"Correct approach:  {mean_correct:.4f}")
    print(f"Difference:        {np.mean(cv_scores_wrong) - mean_correct:.4f}")
    print("\nThe correct approach gives a more realistic performance estimate!")
    
    return {
        'wrong': cv_scores_wrong,
        'correct': cv_scores_correct,
        'wrong_mean': np.mean(cv_scores_wrong),
        'correct_mean': mean_correct
    }


def custom_cv_with_permutation_test(X, y, n_splits=5, n_permutations=100, random_state=42):
    """
    Permutation test for model significance.
    
    Tests whether the model's performance is significantly better
    than random by comparing to permutations of the labels.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target variable
    n_splits : int
        Number of folds
    n_permutations : int
        Number of permutations
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Permutation test results
    """
    print("\n" + "="*80)
    print("PERMUTATION TEST FOR MODEL SIGNIFICANCE")
    print("="*80)
    
    print(f"\nDataset: {X.shape}")
    print(f"Running {n_permutations} permutations...")
    
    skfold = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=random_state)
    model = LogisticRegression(max_iter=1000, random_state=random_state)
    
    original_scores = cross_val_score(model, X, y, cv=skfold, scoring='accuracy')
    original_mean = np.mean(original_scores)
    
    print(f"\nOriginal model accuracy: {original_mean:.4f}")
    
    permutation_scores = []
    
    for i in range(n_permutations):
        y_permuted = np.random.permutation(y)
        
        perm_scores = cross_val_score(
            model, X, y_permuted, cv=skfold, scoring='accuracy'
        )
        permutation_scores.append(np.mean(perm_scores))
        
        if (i + 1) % 20 == 0:
            print(f"Completed {i + 1}/{n_permutations} permutations...")
    
    permutation_mean = np.mean(permutation_scores)
    permutation_std = np.std(permutation_scores)
    
    p_value = np.mean(np.array(permutation_scores) >= original_mean)
    
    print(f"\nPermutation test results:")
    print(f"  Permuted mean accuracy: {permutation_mean:.4f} (+/- {permutation_std:.4f})")
    print(f"  Original accuracy:      {original_mean:.4f}")
    print(f"  Better than random:     {original_mean > permutation_mean}")
    print(f"  P-value:              {p_value:.4f}")
    
    return {
        'original': original_mean,
        'permuted_mean': permutation_mean,
        'permuted_std': permutation_std,
        'p_value': p_value
    }


def comprehensive_cv_demonstration():
    """
    Run comprehensive cross-validation demonstration showing all strategies.
    """
    print("\n" + "="*80)
    print("COMPREHENSIVE CROSS-VALIDATION STRATEGIES DEMONSTRATION")
    print("="*80)
    
    print("\n1. Generating balanced classification data...")
    X, y, feature_names = generate_classification_data(
        n_samples=500, n_features=10, random_state=42
    )
    print(f"   Generated: {X.shape[0]} samples, {X.shape[1]} features")
    print(f"   Class distribution: {np.bincount(y)}")
    
    print("\n2. Running basic K-Fold CV...")
    basic_kfold_cv(X, y, n_splits=5, random_state=42)
    
    print("\n3. Running Stratified K-Fold CV...")
    stratified_kfold_cv(X, y, n_splits=5, random_state=42)
    
    print("\n4. Comparing K-Fold vs Stratified K-Fold...")
    X_imbalanced, y_imbalanced, _ = generate_imbalanced_data(
        n_samples=500, imbalance_ratio=0.1, random_state=42
    )
    compare_kfold_stratified(X_imbalanced, y_imbalanced, n_splits=5, random_state=42)
    
    print("\n5. Analyzing bias-variance tradeoff...")
    analyze_bias_variance(X, y, n_splits=5, random_state=42)
    
    print("\n6. Choosing optimal K...")
    choose_optimal_k(X, y, max_k=10, random_state=42)
    
    return {
        'message': 'Comprehensive CV demonstration complete'
    }


def output_results_summary():
    """
    Summary of cross-validation results and best practices.
    """
    print("\n" + "="*80)
    print("CROSS-VALIDATION RESULTS SUMMARY")
    print("="*80)
    
    print("""
BEST PRACTICES FOR CROSS-VALIDATION:
---------------------------------

1. CHOOSING THE RIGHT STRATEGY:
   - Balanced data: K-Fold or Stratified K-Fold (K=5 or K=10)
   - Imbalanced data: Stratified K-Fold (critical!)
   - Grouped data: Group K-Fold (e.g., patient, customer)
   - Time series: TimeSeriesSplit
   - Small datasets: Leave-One-Out or Repeated CV

2. AVOIDING DATA LEAKING:
   - Feature selection: Within each fold, not before CV
   - Hyperparameter tuning: Use nested CV
   - Scaling: Fit on train, transform on val within each fold

3. CHOOSING K:
   - Smaller K: More training data per fold, biased estimate
   - Larger K: Better approximation to full data, higher variance
   - Standard: K=5 or K=10 works well in most cases

4. STABLE ESTIMATES:
   - Use Repeated Stratified K-Fold for more stable estimates
   - Report mean and standard deviation
   - Consider 95% confidence intervals

5. MULTIPLE METRICS:
   - Use multiple metrics for comprehensive evaluation
   - For imbalanced data: precision, recall, F1, ROC-AUC
   - For classification: accuracy can be misleading

6. MODEL COMPARISON:
   - Use same CV splits for fair comparison
   - Statistical tests (e.g., paired t-test) for significance
   - Consider effect size, not just statistical significance

7. REPORTING:
   - Always report: Mean, std, and number of folds
   - Also report: Training time if relevant
   - Include configuration details
""")
    
    return {
        'summary': 'Cross-validation best practices summary complete'
    }


def main():
    """
    Main function to execute all cross-validation demonstration functions.
    """
    print("="*80)
    print("CROSS VALIDATION STRATEGIES IMPLEMENTATION")
    print("="*80)
    print("\nThis implementation covers comprehensive cross-validation strategies")
    print("including K-Fold, Stratified K-Fold, Time Series, Nested CV, and more.")
    
    print("\n" + "-"*80)
    print("SECTION 1: BASIC CROSS-VALIDATION STRATEGIES")
    print("-"*80 + "\n")
    
    X, y, feature_names = generate_classification_data(n_samples=500, random_state=42)
    
    basic_kfold_cv(X, y, n_splits=5, random_state=42)
    
    stratified_kfold_cv(X, y, n_splits=5, random_state=42)
    
    print("\n" + "-"*80)
    print("SECTION 2: COMPARING K-FOLD VS STRATIFIED K-FOLD")
    print("-"*80 + "\n")
    
    X_imbalanced, y_imbalanced, _ = generate_imbalanced_data(
        n_samples=500, imbalance_ratio=0.1, random_state=42
    )
    compare_kfold_stratified(X_imbalanced, y_imbalanced, n_splits=5, random_state=42)
    
    print("\n" + "-"*80)
    print("SECTION 3: ALTERNATIVE STRATEGIES")
    print("-"*80 + "\n")
    
    print("Leave-One-Out CV:")
    loo_scores, loo_mean, loo_std = leave_one_out_cv(X[:100], y[:100], random_state=42)
    
    print("\nTime Series CV:")
    X_time, y_time, time_index = generate_time_series_data(n_samples=200, random_state=42)
    time_series_cv(X_time, y_time, n_splits=5, random_state=42)
    
    print("\nGroup K-Fold CV:")
    X_group, y_group, groups = generate_grouped_data(n_samples=200, n_groups=10, random_state=42)
    group_kfold_cv(X_group, y_group, groups, n_splits=5, random_state=42)
    
    print("\n" + "-"*80)
    print("SECTION 4: REPEATED AND NESTED CV")
    print("-"*80 + "\n")
    
    repeated_stratified_kfold_cv(X, y, n_splits=5, n_repeats=3, random_state=42)
    
    param_grid = {
        'C': [0.01, 0.1, 1.0, 10.0]
    }
    nested_cv_for_model_selection(X, y, param_grid, n_splits=5, random_state=42)
    
    print("\n" + "-"*80)
    print("SECTION 5: BIAS-VARIANCE ANALYSIS")
    print("-"*80 + "\n")
    
    analyze_bias_variance(X, y, n_splits=5, random_state=42)
    
    print("\n" + "-"*80)
    print("SECTION 6: CHOOSING OPTIMAL K")
    print("-"*80 + "\n")
    
    choose_optimal_k(X, y, max_k=10, random_state=42)
    
    print("\n" + "-"*80)
    print("SECTION 7: BANKING EXAMPLE")
    print("-"*80 + "\n")
    
    banking_credit_scoring_cv()
    
    print("\n" + "-"*80)
    print("SECTION 8: HEALTHCARE EXAMPLE")
    print("-"*80 + "\n")
    
    medical_diagnosis_cv()
    
    print("\n" + "-"*80)
    print("SECTION 9: ADVANCED TOPICS")
    print("-"*80 + "\n")
    
    cross_validate_with_feature_selection(X, y, n_splits=5, random_state=42)
    
    print("\n" + "-"*80)
    print("SECTION 10: RESULTS SUMMARY")
    print("-"*80 + "\n")
    
    output_results_summary()
    
    return {
        'message': 'All cross-validation demonstrations complete'
    }


if __name__ == "__main__":
    results = main()
    print("\n" + "="*80)
    print("EXECUTION COMPLETE")
    print("="*80)