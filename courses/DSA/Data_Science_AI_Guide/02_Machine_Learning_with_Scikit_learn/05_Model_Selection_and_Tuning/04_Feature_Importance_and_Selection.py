# Topic: Feature Importance and Selection
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Feature Importance and Selection

This module provides a comprehensive guide to feature importance analysis and feature
selection techniques in machine learning using Scikit-learn. It covers various methods
for identifying the most important features, selecting optimal feature subsets, and
understanding the trade-offs between different approaches.

I. INTRODUCTION
    - Why Feature Selection Matters
    - Types of Feature Importance
    - Overview of Selection Methods
    
II. CORE CONCEPTS
    - Feature Importance vs Feature Selection
    - Filter Methods
    - Wrapper Methods
    - Embedded Methods
    - Dimensionality Reduction vs Feature Selection
    
III. IMPLEMENTATION
    - Tree-based Feature Importance
    - Permutation Importance
    - SelectKBest with Statistical Tests
    - Recursive Feature Elimination (RFE)
    - SelectFromModel
    - Feature Selection Pipelines
    
IV. EXAMPLES
    - Banking/Finance Example: Credit Risk Prediction
    - Healthcare Example: Disease Diagnosis
    
V. OUTPUT RESULTS
    - Performance Comparison
    - Visualization of Feature Importance
    
VI. TESTING
    - Unit Tests for Feature Selection Functions
    - Integration Tests
    
VII. ADVANCED TOPICS
    - Handling Correlated Features
    - Stability Selection
    - Cross-Validation in Feature Selection
    - Overfitting Prevention Strategies
    
VIII. CONCLUSION
    - Best Practices
    - Common Pitfalls
    - Summary

Author: AI Assistant
Date: 06-04-2026
"""

# ============================================================================
# SECTION I: INTRODUCTION AND SETUP
# ============================================================================

"""
This section introduces the fundamental concepts of feature importance and 
selection, explaining why these techniques are critical for building robust
machine learning models.

Why Feature Selection Matters:
1. Improves model performance by removing noisy features
2. Reduces overfitting by eliminating irrelevant or redundant features
3. Enhances model interpretability
4. Speeds up training and inference
5. Reduces memory and computational requirements

Types of Feature Importance:
1. Tree-based importance (from Random Forest, Gradient Boosting)
2. Permutation importance
3. Statistical importance (ANOVA F-value, Mutual Information)
4. Coefficients-based importance (Linear models)
5. Regularization-based importance (L1, Elastic Net)
"""

# Import necessary libraries
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from typing import Tuple, List, Dict, Optional, Callable, Any
from dataclasses import dataclass, field
from abc import ABC, abstractmethod

# Scikit-learn imports
from sklearn.datasets import make_classification, make_regression, load_breast_cancer, load_diabetes
from sklearn.model_selection import train_test_split, cross_val_score, StratifiedKFold
from sklearn.preprocessing import StandardScaler, MinMaxScaler, LabelEncoder
from sklearn.ensemble import (
    RandomForestClassifier, 
    RandomForestRegressor,
    GradientBoostingClassifier,
    GradientBoostingRegressor,
    ExtraTreesClassifier,
    AdaBoostClassifier
)
from sklearn.linear_model import LogisticRegression, LinearRegression, Lasso, Ridge, ElasticNet
from sklearn.feature_selection import (
    SelectKBest, 
    f_classif, 
    f_regression, 
    mutual_info_classif, 
    mutual_info_regression,
    SelectFromModel,
    RFE,
    RFECV,
    SequentialFeatureSelector,
    SelectPercentile,
    GenericUnivariateSelect,
    chi2
)
from sklearn.decomposition import PCA, FactorAnalysis
from sklearn.inspection import permutation_importance
from sklearn.metrics import (
    accuracy_score, 
    precision_score, 
    recall_score, 
    f1_score,
    roc_auc_score,
    mean_squared_error,
    r2_score,
    classification_report,
    confusion_matrix
)
from sklearn.pipeline import Pipeline, make_pipeline
from sklearn.base import BaseEstimator, TransformerMixin, ClassifierMixin, RegressorMixin

# Set random seed for reproducibility
np.random.seed(42)

# Suppress warnings for cleaner output
import warnings
warnings.filterwarnings('ignore')

# Configure display options
pd.set_option('display.max_columns', 50)
pd.set_option('display.width', 200)

# ============================================================================
# SECTION II: CORE CONCEPTS AND DATA STRUCTURES
# ============================================================================

"""
This section defines core data structures and helper classes for feature 
importance and selection operations.
"""

@dataclass
class FeatureSelectionResult:
    """
    Data class to store feature selection results.
    
    Attributes:
        selected_features: List of selected feature indices or names
        feature_importances: Array of importance scores for all features
        method_used: Name of the feature selection method
        performance_metric: Model performance after selection
        n_features_selected: Number of features selected
    """
    selected_features: List[int]
    feature_importances: np.ndarray
    method_used: str
    performance_metric: float
    n_features_selected: int
    feature_names: Optional[List[str]] = None
    
    def __post_init__(self):
        if self.feature_names is None:
            self.feature_names = [f"feature_{i}" for i in self.selected_features]
    
    def to_dataframe(self) -> pd.DataFrame:
        """Convert results to pandas DataFrame."""
        return pd.DataFrame({
            'feature': self.feature_names,
            'importance': self.feature_importances[self.selected_features],
            'selected': [True if i in self.selected_features else False 
                        for i in range(len(self.feature_importances))]
        })


@dataclass
class FeatureImportanceConfig:
    """
    Configuration for feature importance analysis.
    
    Attributes:
        n_iterations: Number of iterations for permutation importance
        random_state: Random seed for reproducibility
        n_jobs: Number of CPU cores to use
        scoring: Metric for evaluation
        cv_folds: Number of cross-validation folds
    """
    n_iterations: int = 10
    random_state: int = 42
    n_jobs: int = -1
    scoring: str = 'accuracy'
    cv_folds: int = 5


class FeatureSelector(ABC):
    """
    Abstract base class for feature selection methods.
    
    This class defines the interface that all feature selection methods
    must implement.
    """
    
    @abstractmethod
    def fit(self, X: np.ndarray, y: np.ndarray) -> 'FeatureSelector':
        """
        Fit the feature selector to the data.
        
        Args:
            X: Feature matrix
            y: Target variable
            
        Returns:
            Self
        """
        pass
    
    @abstractmethod
    def transform(self, X: np.ndarray) -> np.ndarray:
        """
        Transform the feature matrix.
        
        Args:
            X: Feature matrix
            
        Returns:
            Transformed feature matrix
        """
        pass
    
    def fit_transform(self, X: np.ndarray, y: np.ndarray) -> np.ndarray:
        """
        Fit and transform in one step.
        
        Args:
            X: Feature matrix
            y: Target variable
            
        Returns:
            Transformed feature matrix
        """
        return self.fit(X, y).transform(X)


# ============================================================================
# SECTION III: DATA GENERATION AND PREPROCESSING
# ============================================================================

"""
This section provides functions for generating synthetic datasets and 
preprocessing data for feature importance analysis.
"""

def generate_classification_data(
    n_samples: int = 1000,
    n_features: int = 20,
    n_informative: int = 10,
    n_redundant: int = 5,
    n_clusters_per_class: int = 2,
    random_state: int = 42
) -> Tuple[np.ndarray, np.ndarray, List[str]]:
    """
    Generate synthetic classification dataset for feature importance analysis.
    
    This function creates a classification dataset with controlled amounts of
    informative, redundant, and noise features to demonstrate feature 
    selection techniques.
    
    Args:
        n_samples: Number of samples to generate
        n_features: Total number of features
        n_informative: Number of informative features
        n_redundant: Number of redundant features
        n_clusters_per_class: Number of clusters per class
        random_state: Random seed for reproducibility
        
    Returns:
        Tuple of (X, y, feature_names)
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_clusters_per_class=n_clusters_per_class,
        random_state=random_state,
        flip_y=0.01,
        class_sep=1.0
    )
    
    # Generate feature names
    feature_names = []
    for i in range(n_features):
        if i < n_informative:
            if i < n_informative // 2:
                feature_names.append(f"income_{i}")
            else:
                feature_names.append(f"credit_score_{i}")
        elif i < n_informative + n_redundant:
            feature_names.append(f"redundant_{i - n_informative}")
        else:
            feature_names.append(f"noise_{i - n_informative - n_redundant}")
    
    return X, y, feature_names


def generate_regression_data(
    n_samples: int = 1000,
    n_features: int = 20,
    n_informative: int = 10,
    noise: float = 0.1,
    random_state: int = 42
) -> Tuple[np.ndarray, np.ndarray, List[str]]:
    """
    Generate synthetic regression dataset for feature importance analysis.
    
    Args:
        n_samples: Number of samples to generate
        n_features: Total number of features
        n_informative: Number of informative features
        noise: Standard deviation of Gaussian noise
        random_state: Random seed for reproducibility
        
    Returns:
        Tuple of (X, y, feature_names)
    """
    X, y = make_regression(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        noise=noise,
        random_state=random_state
    )
    
    # Generate feature names
    feature_names = [f"feature_{i}" for i in range(n_features)]
    
    return X, y, feature_names


def generate_correlated_features(
    n_samples: int = 500,
    n_features: int = 10,
    correlation: float = 0.8,
    random_state: int = 42
) -> Tuple[np.ndarray, List[str]]:
    """
    Generate dataset with correlated features.
    
    This is useful for testing feature selection methods that handle
    correlated features.
    
    Args:
        n_samples: Number of samples
        n_features: Number of features
        correlation: Correlation strength (0 to 1)
        random_state: Random seed
        
    Returns:
        Tuple of (X, feature_names)
    """
    np.random.seed(random_state)
    
    # Generate base features
    X = np.random.randn(n_samples, n_features)
    
    # Add correlation between features
    for i in range(1, n_features):
        X[:, i] = correlation * X[:, 0] + (1 - correlation) * X[:, i]
    
    feature_names = [f"feature_{i}" for i in range(n_features)]
    
    return X, feature_names


def add_noisy_features(
    X: np.ndarray,
    y: np.ndarray,
    n_noise_features: int = 10,
    random_state: int = 42
) -> Tuple[np.ndarray, np.ndarray]:
    """
    Add random noise features to dataset.
    
    Args:
        X: Original feature matrix
        y: Target variable
        n_noise_features: Number of noise features to add
        random_state: Random seed
        
    Returns:
        Tuple of (X_with_noise, y)
    """
    np.random.seed(random_state)
    n_samples = X.shape[0]
    
    # Generate noise features
    noise = np.random.randn(n_samples, n_noise_features)
    
    # Concatenate with original features
    X_with_noise = np.hstack([X, noise])
    
    return X_with_noise, y


def preprocess_data(
    X: np.ndarray,
    y: np.ndarray,
    test_size: float = 0.2,
    random_state: int = 42,
    scale: bool = True
) -> Tuple[np.ndarray, np.ndarray, np.ndarray, np.ndarray]:
    """
    Preprocess data for feature importance analysis.
    
    Args:
        X: Feature matrix
        y: Target variable
        test_size: Proportion of data for testing
        random_state: Random seed
        scale: Whether to scale features
        
    Returns:
        Tuple of (X_train, X_test, y_train, y_test)
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=test_size, random_state=random_state
    )
    
    if scale:
        scaler = StandardScaler()
        X_train = scaler.fit_transform(X_train)
        X_test = scaler.transform(X_test)
    
    return X_train, X_test, y_train, y_test


# ============================================================================
# SECTION IV: TREE-BASED FEATURE IMPORTANCE
# ============================================================================

"""
This section implements tree-based feature importance using Random Forest,
Gradient Boosting, and other tree-based models.

Tree-based feature importance is calculated based on how much each feature
contributes to reducing impurity (for classification) or variance (for
regression) in decision trees.

 Key Points:
1. Built-in to tree-based ensemble methods
2. Captures non-linear relationships
3. Automatically handles feature interactions
4. Can be biased towards features with more unique values
"""


def calculate_tree_importance(
    X: np.ndarray,
    y: np.ndarray,
    model_type: str = 'random_forest',
    n_estimators: int = 100,
    random_state: int = 42,
    task: str = 'classification'
) -> np.ndarray:
    """
    Calculate feature importance using tree-based models.
    
    Args:
        X: Feature matrix
        y: Target variable
        model_type: Type of tree-based model
        n_estimators: Number of estimators
        random_state: Random seed
        task: 'classification' or 'regression'
        
    Returns:
        Array of feature importance scores
    """
    if task == 'classification':
        if model_type == 'random_forest':
            model = RandomForestClassifier(
                n_estimators=n_estimators,
                random_state=random_state,
                n_jobs=-1
            )
        elif model_type == 'gradient_boosting':
            model = GradientBoostingClassifier(
                n_estimators=n_estimators,
                random_state=random_state
            )
        elif model_type == 'extra_trees':
            model = ExtraTreesClassifier(
                n_estimators=n_estimators,
                random_state=random_state,
                n_jobs=-1
            )
        else:
            raise ValueError(f"Unknown model type: {model_type}")
    else:
        if model_type == 'random_forest':
            model = RandomForestRegressor(
                n_estimators=n_estimators,
                random_state=random_state,
                n_jobs=-1
            )
        elif model_type == 'gradient_boosting':
            model = GradientBoostingRegressor(
                n_estimators=n_estimators,
                random_state=random_state
            )
        else:
            raise ValueError(f"Unknown model type: {model_type}")
    
    model.fit(X, y)
    return model.feature_importances_


def get_ranked_features_by_importance(
    importances: np.ndarray,
    feature_names: Optional[List[str]] = None,
    ascending: bool = False
) -> pd.DataFrame:
    """
    Rank features by importance scores.
    
    Args:
        importances: Array of importance scores
        feature_names: List of feature names
        ascending: Whether to sort in ascending order
        
    Returns:
        DataFrame with ranked features
    """
    if feature_names is None:
        feature_names = [f"feature_{i}" for i in range(len(importances))]
    
    df = pd.DataFrame({
        'feature': feature_names,
        'importance': importances
    })
    
    df = df.sort_values('importance', ascending=ascending).reset_index(drop=True)
    df['rank'] = range(1, len(df) + 1)
    
    return df[['rank', 'feature', 'importance']]


def compare_tree_models(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str],
    task: str = 'classification'
) -> pd.DataFrame:
    """
    Compare feature importance across different tree-based models.
    
    This function calculates and compares feature importance scores
    from multiple tree-based models to identify consistent predictors.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        task: 'classification' or 'regression'
        
    Returns:
        DataFrame with comparison results
    """
    results = {'feature': feature_names}
    
    models = ['random_forest', 'gradient_boosting', 'extra_trees']
    
    for model in models:
        importances = calculate_tree_importance(
            X, y, model_type=model, task=task
        )
        results[model] = importances
    
    # Calculate average importance
    results['average'] = np.mean([results[m] for m in models], axis=0)
    
    # Calculate standard deviation
    results['std'] = np.std([results[m] for m in models], axis=0)
    
    df = pd.DataFrame(results)
    df = df.sort_values('average', ascending=False).reset_index(drop=True)
    
    return df


def visualize_tree_importance(
    importances: np.ndarray,
    feature_names: List[str],
    top_n: int = 20,
    title: str = "Tree-Based Feature Importance",
    save_path: Optional[str] = None
) -> None:
    """
    Visualize tree-based feature importance.
    
    Args:
        importances: Array of importance scores
        feature_names: List of feature names
        top_n: Number of top features to display
        title: Plot title
        save_path: Path to save figure
    """
    # Sort and select top N features
    indices = np.argsort(importances)[::-1][:top_n]
    
    plt.figure(figsize=(10, 8))
    
    plt.barh(
        range(len(indices)),
        importances[indices],
        color='steelblue',
        edgecolor='black'
    )
    
    plt.yticks(range(len(indices)), [feature_names[i] for i in indices])
    plt.xlabel('Importance Score')
    plt.title(title)
    plt.gca().invert_yaxis()
    
    plt.tight_layout()
    
    if save_path:
        plt.savefig(save_path, dpi=300, bbox_inches='tight')
    
    plt.close()


# ============================================================================
# SECTION V: PERMUTATION IMPORTANCE
# ============================================================================

"""
This section implements permutation importance, a model-agnostic method
for calculating feature importance.

Permutation importance measures the decrease in model performance when
a feature's values are randomly shuffled. This breaks the relationship
between the feature and the target, allowing us to measure the feature's
contribution to the model's predictions.

Key Advantages:
1. Works with any model
2. Accounts for feature interactions
3. More reliable than tree-based importance
4. Can be computed on test set
"""


def calculate_permutation_importance(
    estimator,
    X: np.ndarray,
    y: np.ndarray,
    scoring: str = 'accuracy',
    n_repeats: int = 10,
    random_state: int = 42,
    test_size: float = 0.2
) -> Tuple[np.ndarray, np.ndarray]:
    """
    Calculate permutation importance.
    
    Args:
        estimator: Fitted sklearn estimator
        X: Feature matrix
        y: Target variable
        scoring: Scoring metric
        n_repeats: Number of times to permute each feature
        random_state: Random seed
        test_size: Proportion of data for testing
        
    Returns:
        Tuple of (importances, standard deviations)
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=test_size, random_state=random_state
    )
    
    # Refit the estimator if needed
    if not hasattr(estimator, 'fitted_'):
        estimator.fit(X_train, y_train)
    
    result = permutation_importance(
        estimator,
        X_test,
        y_test,
        scoring=scoring,
        n_repeats=n_repeats,
        random_state=random_state,
        n_jobs=-1
    )
    
    return result.importances_mean, result.importances_std


def calculate_cv_permutation_importance(
    estimator,
    X: np.ndarray,
    y: np.ndarray,
    scoring: str = 'accuracy',
    n_repeats: int = 10,
    cv: int = 5,
    random_state: int = 42
) -> Tuple[np.ndarray, np.ndarray]:
    """
    Calculate permutation importance with cross-validation.
    
    This provides more robust estimates by averaging over multiple
    train-test splits.
    
    Args:
        estimator: Sklearn estimator
        X: Feature matrix
        y: Target variable
        scoring: Scoring metric
        n_repeats: Number of permutations per fold
        cv: Number of cross-validation folds
        random_state: Random seed
        
    Returns:
        Tuple of (mean importances, std importances)
    """
    kfold = StratifiedKFold(n_splits=cv, shuffle=True, random_state=random_state)
    
    all_importances = []
    
    for train_idx, test_idx in kfold.split(X, y):
        X_train, X_test = X[train_idx], X[test_idx]
        y_train, y_test = y[train_idx], y[test_idx]
        
        # Clone and fit estimator
        from sklearn.base import clone
        est = clone(estimator)
        est.fit(X_train, y_train)
        
        result = permutation_importance(
            est,
            X_test,
            y_test,
            scoring=scoring,
            n_repeats=n_repeats,
            random_state=random_state,
            n_jobs=-1
        )
        
        all_importances.append(result.importances_mean)
    
    return np.mean(all_importances, axis=0), np.std(all_importances, axis=0)


def compare_importance_methods(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str],
    task: str = 'classification'
) -> pd.DataFrame:
    """
    Compare tree-based and permutation importance methods.
    
    This function calculates both tree-based and permutation importance
    and compares them to identify discrepancies.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        task: 'classification' or 'regression'
        
    Returns:
        DataFrame with comparison results
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Tree-based importance
    tree_importance = calculate_tree_importance(X, y, task=task)
    
    # Fit model for permutation importance
    if task == 'classification':
        model = RandomForestClassifier(n_estimators=100, random_state=42)
    else:
        model = RandomForestRegressor(n_estimators=100, random_state=42)
    
    model.fit(X_train, y_train)
    
    perm_result = permutation_importance(
        model, X_test, y_test, n_repeats=10, random_state=42
    )
    perm_importance = perm_result.importances_mean
    
    # Normalize both to sum to 1
    tree_importance_norm = tree_importance / tree_importance.sum()
    perm_importance_norm = perm_importance / perm_importance.sum()
    
    df = pd.DataFrame({
        'feature': feature_names,
        'tree_importance': tree_importance_norm,
        'permutation_importance': perm_importance_norm
    })
    
    df['difference'] = abs(df['tree_importance'] - df['permutation_importance'])
    df = df.sort_values('tree_importance', ascending=False)
    
    return df


# ============================================================================
# SECTION VI: STATISTICAL FEATURE SELECTION
# ============================================================================

"""
This section implements statistical feature selection methods including
SelectKBest, SelectPercentile, and mutual information-based selection.

These are filter methods that select features based on statistical tests
without using a model.
"""


def select_k_best_features(
    X: np.ndarray,
    y: np.ndarray,
    k: int = 10,
    score_func: Callable = f_classif
) -> Tuple[np.ndarray, np.ndarray]:
    """
    Select top K features using statistical tests.
    
    Args:
        X: Feature matrix
        y: Target variable
        k: Number of features to select
        score_func: Scoring function
        
    Returns:
        Tuple of (selected features, selector object)
    """
    selector = SelectKBest(score_func, k=k)
    X_selected = selector.fit_transform(X, y)
    
    return X_selected, selector


def select_percentile_features(
    X: np.ndarray,
    y: np.ndarray,
    percentile: int = 20,
    score_func: Callable = f_classif
) -> Tuple[np.ndarray, SelectPercentile]:
    """
    Select features based on top percentile.
    
    Args:
        X: Feature matrix
        y: Target variable
        percentile: Percentage of features to keep
        score_func: Scoring function
        
    Returns:
        Tuple of (selected features, selector object)
    """
    selector = SelectPercentile(score_func, percentile=percentile)
    X_selected = selector.fit_transform(X, y)
    
    return X_selected, selector


def get_statistical_scores(
    X: np.ndarray,
    y: np.ndarray,
    score_func: Callable = f_classif
) -> np.ndarray:
    """
    Get statistical scores for all features.
    
    Args:
        X: Feature matrix
        y: Target variable
        score_func: Scoring function
        
    Returns:
        Array of scores and p-values
    """
    scores, p_values = score_func(X, y)
    return scores, p_values


def mutual_information_selection(
    X: np.ndarray,
    y: np.ndarray,
    k: int = 10,
    random_state: int = 42
) -> Tuple[np.ndarray, SelectKBest]:
    """
    Select features based on mutual information.
    
    Args:
        X: Feature matrix
        y: Target variable
        k: Number of features to select
        random_state: Random seed
        
    Returns:
        Tuple of (selected features, selector object)
    """
    selector = SelectKBest(
        mutual_info_classif,
        k=k
    )
    X_selected = selector.fit_transform(X, y)
    
    return X_selected, selector


def compare_statistical_methods(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str],
    k: int = 10
) -> pd.DataFrame:
    """
    Compare different statistical feature selection methods.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        k: Number of features to select
        
    Returns:
        DataFrame with comparison results
    """
    # ANOVA F-value
    selector_f = SelectKBest(f_classif, k=k)
    selector_f.fit_transform(X, y)
    f_scores = selector_f.scores_
    
    # Mutual Information
    selector_mi = SelectKBest(mutual_info_classif, k=k)
    selector_mi.fit_transform(X, y)
    mi_scores = selector_mi.scores_
    
    df = pd.DataFrame({
        'feature': feature_names,
        'f_score': f_scores,
        'mutual_info': mi_scores
    })
    
    df['f_rank'] = df['f_score'].rank(ascending=False)
    df['mi_rank'] = df['mutual_info'].rank(ascending=False)
    df['avg_rank'] = (df['f_rank'] + df['mi_rank']) / 2
    
    return df.sort_values('avg_rank')


# ============================================================================
# SECTION VII: RECURSIVE FEATURE ELIMINATION (RFE)
# ============================================================================

"""
This section implements Recursive Feature Elimination (RFE) and its variant
with cross-validation (RFECV).

RFE is a wrapper method that recursively removes features based on the
importance weights from a model.
"""


def perform_rfe(
    estimator,
    X: np.ndarray,
    y: np.ndarray,
    n_features_to_select: int = 5,
    step: int = 1,
    scoring: str = 'accuracy'
) -> Tuple[List[int], np.ndarray]:
    """
    Perform Recursive Feature Elimination.
    
    Args:
        estimator: Sklearn estimator
        X: Feature matrix
        y: Target variable
        n_features_to_select: Number of features to select
        step: Number of features to remove at each step
        scoring: Scoring metric
        
    Returns:
        Tuple of (selected feature indices, ranking)
    """
    rfe = RFE(
        estimator,
        n_features_to_select=n_features_to_select,
        step=step
    )
    rfe.fit(X, y)
    
    selected_indices = np.where(rfe.support_)[0]
    ranking = rfe.ranking_
    
    return selected_indices, ranking


def perform_rfecv(
    estimator,
    X: np.ndarray,
    y: np.ndarray,
    scoring: str = 'accuracy',
    cv: int = 5,
    step: int = 1,
    min_features_to_select: int = 1
) -> Tuple[List[int], np.ndarray, int]:
    """
    Perform Recursive Feature Elimination with Cross-Validation.
    
    This automatically finds the optimal number of features.
    
    Args:
        estimator: Sklearn estimator
        X: Feature matrix
        y: Target variable
        scoring: Scoring metric
        cv: Number of cross-validation folds
        step: Number of features to remove at each step
        min_features_to_select: Minimum number of features to select
        
    Returns:
        Tuple of (selected indices, cv scores, optimal n features)
    """
    rfecv = RFECV(
        estimator,
        scoring=scoring,
        cv=cv,
        step=step,
        min_features_to_select=min_features_to_select,
        n_jobs=-1
    )
    rfecv.fit(X, y)
    
    selected_indices = np.where(rfecv.support_)[0]
    cv_scores = rfecv.cv_results_['mean_test_score']
    optimal_n = rfecv.n_features_
    
    return selected_indices, cv_scores, optimal_n


def get_rfe_ranking(
    X: np.ndarray,
    y: np.ndarray,
    estimator,
    feature_names: List[str]
) -> pd.DataFrame:
    """
    Get RFE ranking for all features.
    
    Args:
        X: Feature matrix
        y: Target variable
        estimator: Sklearn estimator
        feature_names: List of feature names
        
    Returns:
        DataFrame with RFE rankings
    """
    rfe = RFE(estimator, n_features_to_select=1)
    rfe.fit(X, y)
    
    df = pd.DataFrame({
        'feature': feature_names,
        'ranking': rfe.ranking_,
        'selected': rfe.support_
    })
    
    return df.sort_values('ranking')


def plot_rfecv_results(
    cv_scores: np.ndarray,
    optimal_n: int,
    save_path: Optional[str] = None
) -> None:
    """
    Plot RFECV results.
    
    Args:
        cv_scores: Array of cross-validation scores
        optimal_n: Optimal number of features
        save_path: Path to save figure
    """
    plt.figure(figsize=(10, 6))
    
    n_features = range(1, len(cv_scores) + 1)
    
    plt.plot(n_features, cv_scores, marker='o', linewidth=2)
    plt.axvline(x=optimal_n, color='r', linestyle='--', label=f'Optimal: {optimal_n}')
    
    plt.xlabel('Number of Features')
    plt.ylabel('Cross-Validation Score')
    plt.title('RFECV Results')
    plt.legend()
    plt.grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    if save_path:
        plt.savefig(save_path, dpi=300, bbox_inches='tight')
    
    plt.close()


# ============================================================================
# SECTION VIII: FEATURE SELECTION VS DIMENSIONALITY REDUCTION
# ============================================================================

"""
This section compares feature selection and dimensionality reduction approaches.

Feature Selection:
- Selects a subset of original features
- Preserves interpretability
- Can be done with or without labels
- Examples: SelectKBest, RFE, LASSO

Dimensionality Reduction:
- Transforms features into new representation
- May lose interpretability
- Usually unsupervised
- Examples: PCA, t-SNE, UMAP

Key Differences:
1. Feature selection keeps original features; reduction creates new ones
2. Selection is more interpretable
3. Reduction can capture latent patterns
4. Selection preferred when interpretability matters
"""


def apply_pca(
    X: np.ndarray,
    n_components: int = 10,
    scale: bool = True
) -> Tuple[np.ndarray, PCA]:
    """
    Apply PCA for dimensionality reduction.
    
    Args:
        X: Feature matrix
        n_components: Number of components
        scale: Whether to scale features first
        
    Returns:
        Tuple of (transformed X, fitted PCA)
    """
    if scale:
        scaler = StandardScaler()
        X = scaler.fit_transform(X)
    
    pca = PCA(n_components=n_components)
    X_transformed = pca.fit_transform(X)
    
    return X_transformed, pca


def compare_selection_vs_reduction(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str],
    n_selected: int = 10,
    n_components: int = 10,
    test_size: float = 0.2,
    random_state: int = 42
) -> Dict[str, Any]:
    """
    Compare feature selection and dimensionality reduction.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        n_selected: Number of features for selection
        n_components: Number of components for PCA
        test_size: Proportion for testing
        random_state: Random seed
        
    Returns:
        Dictionary with comparison results
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=test_size, random_state=random_state
    )
    
    results = {}
    
    # Method 1: Random Forest Feature Selection
    model = RandomForestClassifier(n_estimators=100, random_state=random_state)
    selector = SelectFromModel(model, threshold='median')
    selector.fit(X_train, y_train)
    X_train_selected = selector.transform(X_train)
    X_test_selected = selector.transform(X_test)
    
    model.fit(X_train_selected, y_train)
    y_pred = model.predict(X_test_selected)
    results['feature_selection'] = accuracy_score(y_test, y_pred)
    
    # Method 2: PCA
    pca = PCA(n_components=n_components)
    X_train_pca = pca.fit_transform(X_train)
    X_test_pca = pca.transform(X_test)
    
    model.fit(X_train_pca, y_train)
    y_pred_pca = model.predict(X_test_pca)
    results['pca'] = accuracy_score(y_test, y_pred_pca)
    
    # Method 3: Original Features
    model.fit(X_train, y_train)
    y_pred_orig = model.predict(X_test)
    results['original'] = accuracy_score(y_test, y_pred_orig)
    
    return results


def evaluate_feature_selection_pipeline(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str],
    test_size: float = 0.2,
    random_state: int = 42
) -> pd.DataFrame:
    """
    Evaluate different feature selection methods in a pipeline.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        test_size: Proportion for testing
        random_state: Random seed
        
    Returns:
        DataFrame with evaluation results
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=test_size, random_state=random_state
    )
    
    models = {
        'RandomForestClassifier': RandomForestClassifier(n_estimators=100, random_state=random_state),
        'GradientBoostingClassifier': GradientBoostingClassifier(n_estimators=100, random_state=random_state)
    }
    
    selection_methods = {
        'None': None,
        'SelectKBest_F': SelectKBest(f_classif, k=10),
        'SelectKBest_MI': SelectKBest(mutual_info_classif, k=10),
        'SelectFromModel': SelectFromModel(RandomForestClassifier(n_estimators=50, random_state=random_state))
    }
    
    results = []
    
    for model_name, model in models.items():
        for selector_name, selector in selection_methods.items():
            pipe = make_pipeline(selector, model) if selector else make_pipeline(model)
            
            try:
                pipe.fit(X_train, y_train)
                y_pred = pipe.predict(X_test)
                accuracy = accuracy_score(y_test, y_pred)
                
                results.append({
                    'model': model_name,
                    'selector': selector_name,
                    'accuracy': accuracy
                })
            except Exception as e:
                results.append({
                    'model': model_name,
                    'selector': selector_name,
                    'accuracy': np.nan,
                    'error': str(e)
                })
    
    return pd.DataFrame(results)


# ============================================================================
# SECTION IX: BANKING EXAMPLE - CREDIT RISK PREDICTION
# ============================================================================

"""
This section demonstrates feature importance and selection in a banking/
finance context, specifically for credit risk prediction.

In this example, we:
1. Generate realistic credit scoring data
2. Apply various feature selection methods
3. Identify the most important features for predicting credit risk
4. Compare performance with different feature subsets
"""


def generate_credit_data(
    n_samples: int = 2000,
    random_state: int = 42
) -> Tuple[np.ndarray, np.ndarray, List[str]]:
    """
    Generate synthetic credit risk data.
    
    Creates realistic credit scoring features that mimic actual
    banking data including credit scores, income, debt-to-income ratio,
    employment length, and other relevant features.
    
    Args:
        n_samples: Number of samples
        random_state: Random seed
        
    Returns:
        Tuple of (X, y, feature_names)
    """
    np.random.seed(random_state)
    
    feature_names = [
        'credit_score',           # Credit score (300-850)
        'annual_income',         # Annual income
        'debt_to_income_ratio',  # Debt to income ratio
        'employment_years',      # Years of employment
        'num_credit_lines',      # Number of credit lines
        'utilization_rate',      # Credit utilization rate
        'payment_history',        # Payment history score
        'num_delinquencies',     # Number of delinquencies
        'loan_amount',          # Loan amount requested
        'loan_term',            # Loan term in months
        'interest_rate',         # Interest rate
        'num_inquiries',        # Number of credit inquiries
        'oldest_credit_age',   # Age of oldest credit account
        'total_credit_limit',  # Total credit limit
        'num_open_accounts',   # Number of open accounts
        'balance_to_limit',    # Balance to limit ratio
        ' debt_amount',         # Total debt amount
        'monthly_payment',     # Monthly payment
        'savings_balance',    # Savings account balance
        'checking_balance',   # Checking account balance
        'age',                 # Customer age
        'education_level',     # Education level (1-5)
        'num_dependents',     # Number of dependents
        'home_owner',          # Home ownership (0/1)
        'marital_status',      # Marital status (0/1)
    ]
    
    n_features = len(feature_names)
    
    # Generate correlated base features
    credit_score = np.random.uniform(300, 850, n_samples)
    annual_income = np.random.uniform(20000, 200000, n_samples)
    debt_to_income = np.random.uniform(0, 0.5, n_samples)
    employment_years = np.random.uniform(0, 30, n_samples)
    
    # Generate features based on base features
    X = np.zeros((n_samples, n_features))
    
    X[:, 0] = credit_score
    X[:, 1] = annual_income
    X[:, 2] = debt_to_income
    X[:, 3] = employment_years
    X[:, 4] = np.random.poisson(3, n_samples) + 1
    X[:, 5] = np.random.uniform(0, 1, n_samples)
    X[:, 6] = np.random.uniform(0, 1, n_samples)
    X[:, 7] = np.random.poisson(1, n_samples)
    X[:, 8] = np.random.uniform(1000, 50000, n_samples)
    X[:, 9] = np.random.choice([12, 24, 36, 48, 60], n_samples)
    X[:, 10] = np.random.uniform(0.05, 0.25, n_samples)
    X[:, 11] = np.random.poisson(2, n_samples)
    X[:, 12] = np.random.uniform(1, 30, n_samples)
    X[:, 13] = X[:, 1] * np.random.uniform(0.1, 0.5, n_samples)
    X[:, 14] = np.random.poisson(5, n_samples) + 1
    X[:, 15] = np.random.uniform(0, 1, n_samples)
    X[:, 16] = annual_income * debt_to_income
    X[:, 17] = X[:, 8] / X[:, 9]
    X[:, 18] = np.random.uniform(0, 50000, n_samples)
    X[:, 19] = np.random.uniform(0, 10000, n_samples)
    X[:, 20] = np.random.uniform(21, 70, n_samples)
    X[:, 21] = np.random.choice([1, 2, 3, 4, 5], n_samples)
    X[:, 22] = np.random.poisson(1, n_samples)
    X[:, 23] = np.random.choice([0, 1], n_samples)
    X[:, 24] = np.random.choice([0, 1], n_samples)
    
    # Generate target based on features
    # Higher credit score, lower DTI, lower delinquencies = lower risk
    risk_score = (
        -0.003 * credit_score +
        annual_income / 10000 +
        -2 * debt_to_income +
        -0.1 * X[:, 7] +  # delinquencies
        0.5 * X[:, 6] +    # payment history
        np.random.randn(n_samples) * 0.5
    )
    
    # Convert to binary target (1 = default, 0 = no default)
    threshold = np.median(risk_score)
    y = (risk_score < threshold).astype(int)
    
    return X, y, feature_names


def analyze_credit_features(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str]
) -> Dict[str, Any]:
    """
    Analyze feature importance for credit risk prediction.
    
    Uses multiple methods to identify the most important features
    for predicting credit default.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        
    Returns:
        Dictionary with analysis results
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    results = {}
    
    # Method 1: Random Forest Feature Importance
    rf = RandomForestClassifier(n_estimators=200, random_state=42)
    rf.fit(X_train, y_train)
    rf_importance = rf.feature_importances_
    
    results['random_forest'] = get_ranked_features_by_importance(
        rf_importance, feature_names
    )
    
    # Method 2: Permutation Importance
    perm_result = permutation_importance(
        rf, X_test, y_test, n_repeats=10, random_state=42
    )
    results['permutation'] = get_ranked_features_by_importance(
        perm_result.importances_mean, feature_names
    )
    
    # Method 3: SelectKBest
    selector = SelectKBest(f_classif, k=10)
    selector.fit(X_train, y_train)
    scores = selector.scores_
    results['selectkbest'] = get_ranked_features_by_importance(
        scores, feature_names
    )
    
    # Method 4: LASSO Coefficients
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    
    lasso = LogisticRegression(penalty='l1', solver='saga', max_iter=1000)
    lasso.fit(X_train_scaled, y_train)
    lasso_importance = np.abs(lasso.coef_[0])
    results['lasso'] = get_ranked_features_by_importance(
        lasso_importance, feature_names
    )
    
    return results


def credit_feature_selection_workflow(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str]
) -> pd.DataFrame:
    """
    Complete workflow for credit feature selection.
    
    Demonstrates the entire process of feature selection for credit
    risk prediction, including comparison of multiple methods.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        
    Returns:
        DataFrame with final recommendations
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # Test different numbers of features
    feature_counts = [5, 10, 15, 20]
    results = []
    
    for n_features in feature_counts:
        # Method 1: SelectKBest
        selector = SelectKBest(f_classif, k=n_features)
        X_train_selected = selector.fit_transform(X_train, y_train)
        X_test_selected = selector.transform(X_test)
        
        rf = RandomForestClassifier(n_estimators=100, random_state=42)
        rf.fit(X_train_selected, y_train)
        y_pred = rf.predict(X_test_selected)
        
        accuracy = accuracy_score(y_test, y_pred)
        
        results.append({
            'method': 'SelectKBest',
            'n_features': n_features,
            'accuracy': accuracy
        })
    
    # Method 2: SelectFromModel
    model = RandomForestClassifier(n_estimators=100, random_state=42)
    selector = SelectFromModel(model, threshold='median')
    X_train_selected = selector.fit_transform(X_train, y_train)
    X_test_selected = selector.transform(X_test)
    
    rf = RandomForestClassifier(n_estimators=100, random_state=42)
    rf.fit(X_train_selected, y_train)
    y_pred = rf.predict(X_test_selected)
    
    accuracy = accuracy_score(y_test, y_pred)
    n_selected = X_train_selected.shape[1]
    
    results.append({
        'method': 'SelectFromModel',
        'n_features': n_selected,
        'accuracy': accuracy
    })
    
    # Method 3: LASSO
    lasso = LogisticRegression(penalty='l1', solver='saga', max_iter=1000)
    lasso.fit(X_train_scaled, y_train)
    
    selected_features = np.where(np.abs(lasso.coef_[0]) > 0)[0]
    
    if len(selected_features) > 0:
        X_train_lasso = X_train_scaled[:, selected_features]
        X_test_lasso = X_test_scaled[:, selected_features]
        
        rf = RandomForestClassifier(n_estimators=100, random_state=42)
        rf.fit(X_train_lasso, y_train)
        y_pred = rf.predict(X_test_lasso)
        
        accuracy = accuracy_score(y_test, y_pred)
        
        results.append({
            'method': 'LASSO',
            'n_features': len(selected_features),
            'accuracy': accuracy
        })
    
    return pd.DataFrame(results)


# ============================================================================
# SECTION X: HEALTHCARE EXAMPLE - DISEASE DIAGNOSIS
# ============================================================================

"""

This section demonstrates feature importance and selection in a healthcare context,
specifically for disease diagnosis prediction.

In this example, we:
1. Generate realistic medical diagnosis data
2. Apply various feature selection methods
3. Identify the most important diagnostic features
4. Compare performance with different feature subsets

"""




def generate_healthcare_data(
    n_samples: int = 2000,
    random_state: int = 42
) -> Tuple[np.ndarray, np.ndarray, List[str]]:
    """
    Generate synthetic healthcare/disease diagnosis data.
    
    Creates realistic medical features including vital signs, lab results,
    symptoms, and other clinical measurements.
    
    Args:
        n_samples: Number of samples
        random_state: Random seed
        
    Returns:
        Tuple of (X, y, feature_names)
    """
    np.random.seed(random_state)
    
    feature_names = [
        'age',
        'gender',
        'bmi',
        'systolic_bp',
        'diastolic_bp',
        'heart_rate',
        'temperature',
        'respiratory_rate',
        'oxygen_saturation',
        'white_blood_cell',
        'red_blood_cell',
        'hemoglobin',
        'platelet_count',
        'glucose',
        'creatinine',
        'sodium',
        'potassium',
        'cholesterol',
        'ldl_cholesterol',
        'hdl_cholesterol',
        'triglycerides',
        'alt',
        'ast',
        'bilirubin',
        'albumin',
        'chest_pain',
        'shortness_breath',
        'fatigue',
        'headache',
        'dizziness',
        'nausea',
        'cough',
        'fever',
        'weight_loss',
        'night_sweats',
        'family_history',
        'smoking_status',
        'alcohol_use',
        'exercise_level',
    ]
    
    n_features = len(feature_names)
    
    X = np.zeros((n_samples, n_features))
    
    X[:, 0] = np.random.uniform(18, 90, n_samples)
    X[:, 1] = np.random.choice([0, 1], n_samples)
    X[:, 2] = np.random.uniform(15, 50, n_samples)
    X[:, 3] = np.random.normal(120, 20, n_samples)
    X[:, 4] = np.random.normal(80, 10, n_samples)
    X[:, 5] = np.random.normal(70, 10, n_samples)
    X[:, 6] = np.random.normal(98.6, 1.5, n_samples)
    X[:, 7] = np.random.normal(14, 2, n_samples)
    X[:, 8] = np.random.normal(97, 3, n_samples)
    X[:, 9] = np.random.normal(7000, 2000, n_samples)
    X[:, 10] = np.random.normal(5, 0.5, n_samples)
    X[:, 11] = np.random.normal(14, 2, n_samples)
    X[:, 12] = np.random.normal(250000, 50000, n_samples)
    X[:, 13] = np.random.normal(100, 20, n_samples)
    X[:, 14] = np.random.normal(1, 0.2, n_samples)
    X[:, 15] = np.random.normal(140, 3, n_samples)
    X[:, 16] = np.random.normal(4, 0.5, n_samples)
    X[:, 17] = np.random.normal(200, 30, n_samples)
    X[:, 18] = np.random.normal(100, 25, n_samples)
    X[:, 19] = np.random.normal(50, 10, n_samples)
    X[:, 20] = np.random.normal(150, 50, n_samples)
    X[:, 21] = np.random.normal(25, 10, n_samples)
    X[:, 22] = np.random.normal(25, 10, n_samples)
    X[:, 23] = np.random.normal(1, 0.3, n_samples)
    X[:, 24] = np.random.normal(4, 0.5, n_samples)
    X[:, 25] = np.random.choice([0, 1], n_samples)
    X[:, 26] = np.random.choice([0, 1], n_samples)
    X[:, 27] = np.random.choice([0, 1], n_samples)
    X[:, 28] = np.random.choice([0, 1], n_samples)
    X[:, 29] = np.random.choice([0, 1], n_samples)
    X[:, 30] = np.random.choice([0, 1], n_samples)
    X[:, 31] = np.random.choice([0, 1], n_samples)
    X[:, 32] = np.random.choice([0, 1], n_samples)
    X[:, 33] = np.random.choice([0, 1], n_samples)
    X[:, 34] = np.random.choice([0, 1], n_samples)
    X[:, 35] = np.random.choice([0, 1], n_samples)
    X[:, 36] = np.random.choice([0, 1], n_samples)
    X[:, 37] = np.random.choice([0, 1], n_samples)
    X[:, 38] = np.random.choice([0, 1], n_samples)
    
    age = X[:, 0]
    bmi = X[:, 2]
    systolic = X[:, 3]
    glucose = X[:, 13]
    cholesterol = X[:, 17]
    smoker = X[:, 36]
    
    disease_score = (
        (age - 50) / 20 +
        (bmi - 25) / 5 +
        (systolic - 120) / 30 +
        (glucose - 100) / 30 +
        (cholesterol - 200) / 50 +
        smoker * 0.5 +
        np.random.randn(n_samples) * 0.3
    )
    
    threshold = np.median(disease_score)
    y = (disease_score > threshold).astype(int)
    
    return X, y, feature_names


def analyze_healthcare_features(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str]
) -> Dict[str, pd.DataFrame]:
    """
    Analyze feature importance for healthcare diagnosis.
    
    Uses gradient boosting for feature importance analysis in
    a medical diagnosis context.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        
    Returns:
        Dictionary with analysis results
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    results = {}
    
    gb = GradientBoostingClassifier(n_estimators=100, random_state=42)
    gb.fit(X_train, y_train)
    
    tree_importance = gb.feature_importances_
    results['gradient_boosting'] = get_ranked_features_by_importance(
        tree_importance, feature_names
    )
    
    perm_result = permutation_importance(
        gb, X_test, y_test, n_repeats=10, random_state=42
    )
    results['permutation'] = get_ranked_features_by_importance(
        perm_result.importances_mean, feature_names
    )
    
    selector = SelectKBest(f_classif, k=15)
    selector.fit(X_train, y_train)
    scores = selector.scores_
    results['selectkbest'] = get_ranked_features_by_importance(
        scores, feature_names
    )
    
    return results


def healthcare_feature_selection_workflow(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str]
) -> pd.DataFrame:
    """
    Complete workflow for healthcare feature selection.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        
    Returns:
        DataFrame with comparison results
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    results = []
    models = [
        ('RandomForest', RandomForestClassifier(n_estimators=100, random_state=42)),
        ('GradientBoosting', GradientBoostingClassifier(n_estimators=100, random_state=42))
    ]
    
    for model_name, base_model in models:
        for n_features in [5, 10, 15, 20]:
            selector = SelectKBest(f_classif, k=n_features)
            X_train_selected = selector.fit_transform(X_train, y_train)
            X_test_selected = selector.transform(X_test)
            
            from sklearn.base import clone
            model = clone(base_model)
            model.fit(X_train_selected, y_train)
            y_pred = model.predict(X_test_selected)
            
            accuracy = accuracy_score(y_test, y_pred)
            
            results.append({
                'model': model_name,
                'n_features': n_features,
                'accuracy': accuracy
            })
    
    return pd.DataFrame(results)


# ============================================================================
# SECTION XI: OVERFITTING PREVENTION STRATEGIES
# ============================================================================

"""

This section covers strategies for preventing overfitting during feature
selection, including proper cross-validation, stability selection, and
regularization techniques.

Overfitting in Feature Selection:
1. Data leakage during selection
2. Selection based on test set
3. Too many features relative to samples
4. Not using nested cross-validation

Solutions:
1. Nested cross-validation
2. Stability selection
3. Regularized models (LASSO, Elastic Net)
4. Conservative feature selection
"""




def nested_feature_selection(
    X: np.ndarray,
    y: np.ndarray,
    scoring: str = 'accuracy',
    cv_outer: int = 5,
    cv_inner: int = 5,
    random_state: int = 42
) -> Tuple[List[int], float]:
    """
    Perform nested cross-validation for feature selection.
    
    This prevents data leakage by using inner CV for selection
    and outer CV for evaluation.
    
    Args:
        X: Feature matrix
        y: Target variable
        scoring: Scoring metric
        cv_outer: Outer CV folds
        cv_inner: Inner CV folds
        random_state: Random seed
        
    Returns:
        Tuple of (selected indices, outer CV score)
    """
    kfold_outer = StratifiedKFold(n_splits=cv_outer, shuffle=True, random_state=random_state)
    
    all_selected = []
    all_scores = []
    
    for train_idx, test_idx in kfold_outer.split(X, y):
        X_train, X_test = X[train_idx], X[test_idx]
        y_train, y_test = y[train_idx], y[test_idx]
        
        # Inner CV for feature selection
        kfold_inner = StratifiedKFold(n_splits=cv_inner, shuffle=True, random_state=random_state)
        
        feature_scores = np.zeros(X.shape[1])
        
        for inner_train_idx, inner_val_idx in kfold_inner.split(X_train, y_train):
            X_inner_train = X_train[inner_train_idx]
            y_inner_train = y_train[inner_train_idx]
            X_inner_val = X_train[inner_val_idx]
            y_inner_val = y_train[inner_val_idx]
            
            model = RandomForestClassifier(n_estimators=50, random_state=random_state)
            model.fit(X_inner_train, y_inner_train)
            feature_scores += model.feature_importances_
        
        feature_scores /= cv_inner
        
        # Select top features
        k = min(10, X.shape[1])
        selected_indices = np.argsort(feature_scores)[::-1][:k]
        
        # Train and evaluate on outer test set
        model = RandomForestClassifier(n_estimators=100, random_state=random_state)
        model.fit(X_train[:, selected_indices], y_train)
        y_pred = model.predict(X_test[:, selected_indices])
        
        score = accuracy_score(y_test, y_pred)
        
        all_selected.append(selected_indices)
        all_scores.append(score)
    
    # Get most frequently selected features
    all_selected = np.array(all_selected)
    selected_counts = np.zeros(X.shape[1])
    
    for indices in all_selected:
        selected_counts[indices] += 1
    
    final_selected = np.argsort(selected_counts)[::-1][:10]
    
    return final_selected, np.mean(all_scores)


def stability_selection(
    X: np.ndarray,
    y: np.ndarray,
    n_bootstrap: int = 50,
    sample_fraction: float = 0.7,
    random_state: int = 42
) -> pd.DataFrame:
    """
    Perform stability selection with bootstrap sampling.
    
    Features that are consistently selected across multiple
    bootstrap samples are considered stable.
    
    Args:
        X: Feature matrix
        y: Target variable
        n_bootstrap: Number of bootstrap samples
        sample_fraction: Fraction of samples per bootstrap
        random_state: Random seed
        
    Returns:
        DataFrame with stability scores
    """
    n_samples = X.shape[0]
    n_select = int(sample_fraction * n_samples)
    
    selection_counts = np.zeros(X.shape[1])
    
    for b in range(n_bootstrap):
        # Bootstrap sample
        indices = np.random.choice(n_samples, size=n_select, replace=True)
        X_bootstrap = X[indices]
        y_bootstrap = y[indices]
        
        # Fit model and get feature importance
        model = RandomForestClassifier(n_estimators=50, random_state=b)
        model.fit(X_bootstrap, y_bootstrap)
        
        # Select top 10 features
        importances = model.feature_importances_
        selected = np.argsort(importances)[::-1][:10]
        selection_counts[selected] += 1
    
    # Normalize
    selection_counts /= n_bootstrap
    
    df = pd.DataFrame({
        'feature': range(len(selection_counts)),
        'selection_frequency': selection_counts
    })
    
    df = df.sort_values('selection_frequency', ascending=False)
    
    return df


def regularized_feature_selection(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str],
    penalty: str = 'l1',
    C: float = 1.0,
    random_state: int = 42
) -> Tuple[List[int], np.ndarray]:
    """
    Select features using regularized models.
    
    LASSO (L1) promotes sparsity by setting coefficients to zero.
    Ridge (L2) shrinks coefficients but doesn't set to zero.
    Elastic Net combines L1 and L2.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        penalty: 'l1', 'l2', or 'elasticnet'
        C: Regularization strength (smaller = more regularization)
        random_state: Random seed
        
    Returns:
        Tuple of (selected indices, coefficients)
    """
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    if penalty == 'l1':
        model = LogisticRegression(penalty=penalty, solver='saga', C=C, max_iter=1000, random_state=random_state)
    elif penalty == 'l2':
        model = LogisticRegression(penalty=penalty, solver='lbfgs', C=C, max_iter=1000, random_state=random_state)
    else:
        model = LogisticRegression(penalty='elasticnet', solver='saga', l1_ratio=0.5, C=C, max_iter=1000, random_state=random_state)
    
    model.fit(X_scaled, y)
    
    coefficients = model.coef_[0]
    selected_indices = np.where(np.abs(coefficients) > 0)[0]
    
    return selected_indices, coefficients


def compare_overfitting_prevention_methods(
    X: np.ndarray,
    y: np.ndarray,
    feature_names: List[str]
) -> pd.DataFrame:
    """
    Compare different overfitting prevention methods.
    
    Args:
        X: Feature matrix
        y: Target variable
        feature_names: List of feature names
        
    Returns:
        DataFrame with comparison results
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    results = []
    
    # Method 1: No feature selection (baseline)
    model = RandomForestClassifier(n_estimators=100, random_state=42)
    model.fit(X_train, y_train)
    y_pred = model.predict(X_test)
    results.append({
        'method': 'No selection',
        'n_features': X.shape[1],
        'accuracy': accuracy_score(y_test, y_pred)
    })
    
    # Method 2: Simple SelectKBest (potential overfitting)
    selector = SelectKBest(f_classif, k=5)
    X_selected = selector.fit_transform(X_train, y_train)
    X_test_selected = selector.transform(X_test)
    
    model = RandomForestClassifier(n_estimators=100, random_state=42)
    model.fit(X_selected, y_train)
    y_pred = model.predict(X_test_selected)
    results.append({
        'method': 'SelectKBest (k=5)',
        'n_features': 5,
        'accuracy': accuracy_score(y_test, y_pred)
    })
    
    # Method 3: Nested CV
    selected_indices, score = nested_feature_selection(X, y)
    results.append({
        'method': 'Nested CV',
        'n_features': len(selected_indices),
        'accuracy': score
    })
    
    # Method 4: Stability selection
    stability_df = stability_selection(X, y)
    top_features = stability_df.head(10)['feature'].values
    
    model = RandomForestClassifier(n_estimators=100, random_state=42)
    model.fit(X_train[:, top_features], y_train)
    y_pred = model.predict(X_test[:, top_features])
    results.append({
        'method': 'Stability',
        'n_features': 10,
        'accuracy': accuracy_score(y_test, y_pred)
    })
    
    # Method 5: LASSO
    selected, _ = regularized_feature_selection(X_train, y_train, feature_names)
    if len(selected) > 0:
        model = RandomForestClassifier(n_estimators=100, random_state=42)
        model.fit(X_train_scaled[:, selected], y_train)
        y_pred = model.predict(X_test_scaled[:, selected])
        results.append({
            'method': 'LASSO',
            'n_features': len(selected),
            'accuracy': accuracy_score(y_test, y_pred)
        })
    
    return pd.DataFrame(results)


# ============================================================================
# SECTION XII: TESTING AND VALIDATION
# ============================================================================

"""

This section provides testing functions to validate feature selection
implementations.
"""




def test_tree_importance() -> bool:
    """
    Test tree-based feature importance calculation.
    """
    X, y, _ = generate_classification_data(n_samples=200, random_state=42)
    
    importance = calculate_tree_importance(X, y, model_type='random_forest')
    
    assert len(importance) == X.shape[1], "Wrong number of importance values"
    assert np.allclose(importance.sum(), 1.0, atol=0.01), "Importances should sum to 1"
    
    return True


def test_permutation_importance() -> bool:
    """
    Test permutation importance calculation.
    """
    X, y, _ = generate_classification_data(n_samples=200, random_state=42)
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    model = RandomForestClassifier(n_estimators=50, random_state=42)
    model.fit(X_train, y_train)
    
    importances, stds = calculate_permutation_importance(
        model, X, y, scoring='accuracy', n_repeats=5
    )
    
    assert len(importances) == X.shape[1], "Wrong number of importance values"
    assert np.all(stds >= 0), "Standard deviations should be non-negative"
    
    return True


def test_statistical_selection() -> bool:
    """
    Test statistical feature selection.
    """
    X, y, _ = generate_classification_data(n_samples=200, random_state=42)
    
    X_selected, selector = select_k_best_features(X, y, k=5)
    
    assert X_selected.shape[1] == 5, "Wrong number of selected features"
    assert X_selected.shape[0] == X.shape[0], "Wrong number of samples"
    
    return True


def test_rfe() -> bool:
    """
    Test Recursive Feature Elimination.
    """
    X, y, _ = generate_classification_data(n_samples=200, random_state=42)
    
    model = RandomForestClassifier(n_estimators=50, random_state=42)
    selected_indices, ranking = perform_rfe(model, X, y, n_features_to_select=5)
    
    assert len(selected_indices) == 5, "Wrong number of selected features"
    assert np.all(ranking >= 1), "Rankings should be >= 1"
    
    return True


def test_stability_selection() -> bool:
    """
    Test stability selection.
    """
    X, y, _ = generate_classification_data(n_samples=200, random_state=42)
    
    df = stability_selection(X, y, n_bootstrap=20)
    
    assert len(df) == X.shape[1], "Wrong number of features in output"
    assert df['selection_frequency'].max() <= 1.0, "Frequency should be <= 1"
    
    return True


def test_credit_data() -> bool:
    """
    Test credit data generation.
    """
    X, y, feature_names = generate_credit_data(n_samples=100)
    
    assert X.shape[0] == 100, "Wrong number of samples"
    assert X.shape[1] == len(feature_names), "Wrong number of features"
    assert len(np.unique(y)) <= 2, "Should be binary classification"
    
    return True


def test_healthcare_data() -> bool:
    """
    Test healthcare data generation.
    """
    X, y, feature_names = generate_healthcare_data(n_samples=100)
    
    assert X.shape[0] == 100, "Wrong number of samples"
    assert X.shape[1] == len(feature_names), "Wrong number of features"
    assert len(np.unique(y)) <= 2, "Should be binary classification"
    
    return True


def run_all_tests() -> Dict[str, bool]:
    """
    Run all tests and return results.
    """
    tests = {
        'tree_importance': test_tree_importance,
        'permutation_importance': test_permutation_importance,
        'statistical_selection': test_statistical_selection,
        'rfe': test_rfe,
        'stability_selection': test_stability_selection,
        'credit_data': test_credit_data,
        'healthcare_data': test_healthcare_data,
    }
    
    results = {}
    
    for test_name, test_func in tests.items():
        try:
            results[test_name] = test_func()
        except Exception as e:
            results[test_name] = False
            print(f"Test {test_name} failed: {e}")
    
    return results


# ============================================================================
# SECTION XIII: MAIN FUNCTION AND EXECUTION
# ============================================================================

"""

This section contains the main function that executes all examples and
demonstrates the feature importance and selection techniques.
"""




def print_section_header(title: str) -> None:
    """
    Print a formatted section header.
    """
    print("\n" + "=" * 70)
    print(title.center(70))
    print("=" * 70)




def print_subsection_header(title: str) -> None:
    """
    Print a formatted subsection header.
    """
    print("\n" + "-" * 50)
    print(title)
    print("-" * 50)




def banking_example():
    """
    Execute banking/finance feature selection example.
    """
    print_section_header("BANKING EXAMPLE: CREDIT RISK PREDICTION")
    
    print_subsection_header("Generating Credit Risk Data")
    X, y, feature_names = generate_credit_data(n_samples=2000)
    print(f"Generated {X.shape[0]} samples with {X.shape[1]} features")
    print(f"Features: {feature_names[:5]}...")
    print(f"Target distribution: {np.bincount(y)}")
    
    print_subsection_header("Analyzing Feature Importance")
    results = analyze_credit_features(X, y, feature_names)
    
    print("\nTop 10 Features by Random Forest:")
    print(results['random_forest'].head(10).to_string(index=False))
    
    print("\nTop 10 Features by Permutation Importance:")
    print(results['permutation'].head(10).to_string(index=False))
    
    print_subsection_header("Performing Feature Selection")
    selection_results = credit_feature_selection_workflow(X, y, feature_names)
    print("\nFeature Selection Results:")
    print(selection_results.to_string(index=False))
    
    print("\n*** Banking Example Complete ***")




def healthcare_example():
    """
    Execute healthcare feature selection example.
    """
    print_section_header("HEALTHCARE EXAMPLE: DISEASE DIAGNOSIS")
    
    print_subsection_header("Generating Healthcare Data")
    X, y, feature_names = generate_healthcare_data(n_samples=2000)
    print(f"Generated {X.shape[0]} samples with {X.shape[1]} features")
    print(f"Features: {feature_names[:5]}...")
    print(f"Target distribution: {np.bincount(y)}")
    
    print_subsection_header("Analyzing Feature Importance")
    results = analyze_healthcare_features(X, y, feature_names)
    
    print("\nTop 10 Features by Gradient Boosting:")
    print(results['gradient_boosting'].head(10).to_string(index=False))
    
    print("\nTop 10 Features by Permutation Importance:")
    print(results['permutation'].head(10).to_string(index=False))
    
    print_subsection_header("Performing Feature Selection")
    selection_results = healthcare_feature_selection_workflow(X, y, feature_names)
    print("\nFeature Selection Results:")
    print(selection_results.to_string(index=False))
    
    print("\n*** Healthcare Example Complete ***")




def demonstration():
    """
    Main demonstration function.
    """
    print_section_header("FEATURE IMPORTANCE AND SELECTION DEMONSTRATION")
    
    print("\nGenerating synthetic classification data...")
    X, y, feature_names = generate_classification_data(
        n_samples=1000,
        n_features=20,
        n_informative=10,
        n_redundant=5
    )
    print(f"Data shape: {X.shape}")
    print(f"Target distribution: {np.bincount(y)}")
    
    print_subsection_header("Tree-Based Feature Importance")
    importance = calculate_tree_importance(X, y, model_type='random_forest')
    ranked = get_ranked_features_by_importance(importance, feature_names)
    print("\nTop 10 Features by Tree Importance:")
    print(ranked.head(10).to_string(index=False))
    
    print_subsection_header("Comparing Tree Models")
    comparison = compare_tree_models(X, y, feature_names)
    print("\nComparison across tree models:")
    print(comparison.head(10).to_string(index=False))
    
    print_subsection_header("Statistical Feature Selection")
    df = compare_statistical_methods(X, y, feature_names, k=10)
    print("\nTop 10 Features by Statistical Methods:")
    print(df.head(10).to_string(index=False))
    
    print_subsection_header("Comparison: Selection vs Reduction")
    comparison = compare_selection_vs_reduction(X, y, feature_names)
    print("\nMethod Comparison:")
    for method, score in comparison.items():
        print(f"  {method}: {score:.4f}")
    
    print_subsection_header("Overfitting Prevention")
    prevention = compare_overfitting_prevention_methods(X, y, feature_names)
    print("\nOverfitting Prevention Comparison:")
    print(prevention.to_string(index=False))
    
    print_subsection_header("Stability Selection")
    stability = stability_selection(X, y, n_bootstrap=30)
    print("\nStable Features:")
    print(stability.head(10).to_string(index=False))
    
    print("\n*** Demonstration Complete ***")




def main():
    """
    Main function to execute all examples.
    """
    print("\n" + "#" * 70)
    print("# FEATURE IMPORTANCE AND SELECTION".center(70))
    print("# Comprehensive Implementation using Scikit-learn".center(70))
    print("#" * 70)
    
    demonstration()
    banking_example()
    healthcare_example()
    
    print_section_header("RUNNING TESTS")
    test_results = run_all_tests()
    print("\nTest Results:")
    for test_name, passed in test_results.items():
        status = "PASSED" if passed else "FAILED"
        print(f"  {test_name}: {status}")
    
    all_passed = all(test_results.values())
    
    print("\n" + "#" * 70)
    if all_passed:
        print("ALL TESTS PASSED".center(70))
    else:
        print("SOME TESTS FAILED".center(70))
    print("#" * 70)
    
    return all_passed




if __name__ == "__main__":
    main()