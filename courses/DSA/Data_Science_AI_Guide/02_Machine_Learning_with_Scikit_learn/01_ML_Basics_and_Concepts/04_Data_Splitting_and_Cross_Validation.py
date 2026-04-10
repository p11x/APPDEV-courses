# Topic: Data Splitting and Cross Validation
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Splitting and Cross Validation

I. INTRODUCTION
    Data splitting and cross-validation are fundamental techniques in machine learning
    for evaluating model performance, preventing overfitting, and ensuring model generalizability.
    This module provides comprehensive coverage of these concepts with practical examples.

II. CORE_CONCEPTS
    A. Train/Test Split: Dividing data into training and testing sets
    B. K-Fold Cross-Validation: Partitioning data into k folds for model validation
    C. Stratified K-Fold: Maintaining class distribution across folds
    D. Cross-Validation Scoring: Evaluating model performance across folds

III. IMPLEMENTATION
    - Train/test split with various ratios
    - K-fold cross-validation
    - Stratified k-fold for imbalanced datasets
    - Multiple scoring metrics
    - Banking (credit scoring) example
    - Healthcare (diagnosis) example

IV. EXAMPLES (Banking + Healthcare)
    V. OUTPUT_RESULTS
    VI. TESTING
    VII. ADVANCED_TOPICS
    VIII. CONCLUSION
"""

# Import necessary libraries
import numpy as np
import pandas as pd
from sklearn.datasets import make_classification, make_regression
from sklearn.model_selection import (
    train_test_split, 
    cross_val_score, 
    KFold, 
    StratifiedKFold,
    LeaveOneOut,
    LeavePOut,
    ShuffleSplit,
    cross_validate
)
from sklearn.preprocessing import StandardScaler, MinMaxScaler
from sklearn.linear_model import LogisticRegression, Ridge
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
from sklearn.metrics import (
    accuracy_score, 
    precision_score, 
    recall_score, 
    f1_score,
    roc_auc_score,
    mean_squared_error,
    mean_absolute_error,
    r2_score,
    classification_report,
    confusion_matrix
)
import warnings
warnings.filterwarnings('ignore')


def generate_classification_data(n_samples=500, n_features=10, n_informative=5, 
                               n_redundant=2, n_clusters_per_class=2,
                               class_separation=1.0, flip_y=0.01, random_state=42):
    """
    Generate synthetic classification dataset for demonstration purposes.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Total number of features
    n_informative : int
        Number of informative features
    n_redundant : int
        Number of redundant features
    n_clusters_per_class : int
        Number of clusters per class
    class_separation : float
        Separation between classes
    flip_y : float
        Proportion of labels to flip (noise)
    random_state : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target labels
    feature_names : list
        Names of features
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_clusters_per_class=n_clusters_per_class,
        class_separation=class_separation,
        flip_y=flip_y,
        random_state=random_state,
        n_classes=2
    )
    
    # Generate feature names
    feature_names = [f'feature_{i+1}' for i in range(n_features)]
    
    # Add some named features for demonstration
    np.random.seed(random_state)
    
    return X, y, feature_names


def generate_regression_data(n_samples=500, n_features=5, noise=10.0, random_state=42):
    """
    Generate synthetic regression dataset for demonstration purposes.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features
    noise : float
        Noise level
    random_state : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target values
    """
    X, y = make_regression(
        n_samples=n_samples,
        n_features=n_features,
        noise=noise,
        random_state=random_state
    )
    
    return X, y


def train_test_split_basic(X, y, test_size=0.2, random_state=42):
    """
    Perform basic train/test split of the data.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        Target values
    test_size : float
        Proportion of data for test set
    random_state : int
        Random seed for reproducibility
    
    Returns:
    --------
    X_train, X_test, y_train, y_test : ndarrays
        Split data
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, 
        test_size=test_size, 
        random_state=random_state,
        shuffle=True
    )
    
    return X_train, X_test, y_train, y_test


def train_test_split_stratified(X, y, test_size=0.2, random_state=42):
    """
    Perform stratified train/test split to maintain class distribution.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        Target labels
    test_size : float
        Proportion of data for test set
    random_state : int
        Random seed for reproducibility
    
    Returns:
    --------
    X_train, X_test, y_train, y_test : ndarrays
        Split data with stratification
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y,
        test_size=test_size,
        random_state=random_state,
        stratify=y
    )
    
    return X_train, X_test, y_train, y_test


def perform_kfold_cv(X_train, y_train, model, n_splits=5, scoring='accuracy'):
    """
    Perform K-Fold cross-validation on training data.
    
    Parameters:
    -----------
    X_train : ndarray
        Training feature matrix
    y_train : ndarray
        Training target values
    model : estimator
        Machine learning model
    n_splits : int
        Number of folds
    scoring : str
        Scoring metric
    
    Returns:
    --------
    cv_scores : ndarray
        Cross-validation scores for each fold
    """
    kfold = KFold(n_splits=n_splits, shuffle=True, random_state=42)
    
    cv_scores = cross_val_score(
        estimator=model,
        X=X_train,
        y=y_train,
        cv=kfold,
        scoring=scoring
    )
    
    return cv_scores


def perform_stratified_kfold_cv(X_train, y_train, model, n_splits=5, scoring='accuracy'):
    """
    Perform Stratified K-Fold cross-validation maintaining class distribution.
    
    Parameters:
    -----------
    X_train : ndarray
        Training feature matrix
    y_train : ndarray
        Training target labels
    model : estimator
        Machine learning model
    n_splits : int
        Number of folds
    scoring : str
        Scoring metric
    
    Returns:
    --------
    cv_scores : ndarray
        Cross-validation scores for each fold
    """
    skfold = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=42)
    
    cv_scores = cross_val_score(
        estimator=model,
        X=X_train,
        y=y_train,
        cv=skfold,
        scoring=scoring
    )
    
    return cv_scores


def perform_multiple_metric_cv(X_train, y_train, model, n_splits=5):
    """
    Perform cross-validation with multiple scoring metrics.
    
    Parameters:
    -----------
    X_train : ndarray
        Training feature matrix
    y_train : ndarray
        Training target labels
    model : estimator
        Machine learning model
    n_splits : int
        Number of folds
    
    Returns:
    --------
    results : dict
        Dictionary containing scores for each metric
    """
    skfold = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=42)
    
    scoring_metrics = {
        'accuracy': 'accuracy',
        'precision': 'precision',
        'recall': 'recall',
        'f1': 'f1',
        'roc_auc': 'roc_auc'
    }
    
    results = {}
    
    for metric_name, scoring in scoring_metrics.items():
        try:
            scores = cross_val_score(
                estimator=model,
                X=X_train,
                y=y_train,
                cv=skfold,
                scoring=scoring
            )
            results[metric_name] = {
                'scores': scores,
                'mean': scores.mean(),
                'std': scores.std()
            }
        except Exception as e:
            results[metric_name] = {'error': str(e)}
    
    return results


def evaluate_different_split_ratios(X, y, model_class, n_splits=5):
    """
    Evaluate model performance with different train/test split ratios.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        Target values
    model_class : class
        Model class to instantiate
    n_splits : int
        Number of folds for CV
    
    Returns:
    --------
    results : dict
        Results for each split ratio
    """
    test_sizes = [0.1, 0.2, 0.3, 0.4, 0.5]
    results = {}
    
    for test_size in test_sizes:
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=test_size, random_state=42, stratify=y
        )
        
        # Scale features
        scaler = StandardScaler()
        X_train_scaled = scaler.fit_transform(X_train)
        X_test_scaled = scaler.transform(X_test)
        
        # Create and train model
        model = model_class(random_state=42)
        model.fit(X_train_scaled, y_train)
        
        # Evaluate on test set
        y_pred = model.predict(X_test_scaled)
        test_accuracy = accuracy_score(y_test, y_pred)
        
        # Cross-validation score on training set
        cv_scores = cross_val_score(
            estimator=model_class(random_state=42),
            X=X_train_scaled,
            y=y_train,
            cv=StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=42),
            scoring='accuracy'
        )
        
        results[test_size] = {
            'train_size': len(X_train),
            'test_size': len(X_test),
            'test_accuracy': test_accuracy,
            'cv_mean': cv_scores.mean(),
            'cv_std': cv_scores.std()
        }
    
    return results


def banking_credit_scoring_example():
    """
    Banking/Finance example: Credit Scoring with Cross-Validation
    
    This example demonstrates using cross-validation for credit scoring,
    predicting whether a loan applicant will default on their loan.
    """
    print("\n" + "="*70)
    print("BANKING EXAMPLE: Credit Scoring with Cross-Validation")
    print("="*70)
    
    # Generate synthetic credit scoring data
    # Features: income, credit_score, debt_to_income, employment_years, loan_amount
    np.random.seed(42)
    n_samples = 1000
    
    # Generate features
    income = np.random.lognormal(mean=10.5, sigma=0.5, size=n_samples)
    credit_score = np.random.normal(loc=650, scale=100, size=n_samples)
    credit_score = np.clip(credit_score, 300, 850)
    debt_to_income = np.random.exponential(scale=0.3, size=n_samples)
    employment_years = np.random.exponential(scale=5, size=n_samples)
    loan_amount = np.random.lognormal(mean=9.5, sigma=0.8, size=n_samples)
    
    # Create feature matrix
    X = np.column_stack([income, credit_score, debt_to_income, employment_years, loan_amount])
    
    # Create target variable (1 = default, 0 = no default)
    # Higher credit score = less likely to default
    # Higher debt to income = more likely to default
    default_prob = (
        0.1 +
        0.3 * (credit_score < 600).astype(float) +
        0.2 * (debt_to_income > 0.4).astype(float) +
        0.1 * (income < 20000).astype(float) +
        0.05 * (employment_years < 1).astype(float)
    )
    default_prob = np.clip(default_prob, 0, 1)
    y = (np.random.random(n_samples) < default_prob).astype(int)
    
    print(f"\nDataset Size: {n_samples} samples")
    print(f"Features: income, credit_score, debt_to_income, employment_years, loan_amount")
    print(f"Class Distribution: Default={y.sum()}, No Default={n_samples-y.sum()}")
    print(f"Default Rate: {y.mean()*100:.2f}%")
    
    # Split data without stratification
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    print(f"\nTrain Set: {len(X_train)} samples")
    print(f"Test Set: {len(X_test)} samples")
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # Train logistic regression model
    model = LogisticRegression(random_state=42, max_iter=1000)
    model.fit(X_train_scaled, y_train)
    
    # Basic evaluation on test set
    y_pred = model.predict(X_test_scaled)
    y_pred_proba = model.predict_proba(X_test_scaled)[:, 1]
    
    print(f"\n--- Basic Test Set Evaluation ---")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred):.4f}")
    print(f"Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"F1 Score: {f1_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_pred_proba):.4f}")
    
    # Cross-validation evaluation
    print(f"\n--- Cross-Validation Evaluation ---")
    
    # K-Fold CV
    kfold = KFold(n_splits=5, shuffle=True, random_state=42)
    kfold_scores = cross_val_score(
        estimator=LogisticRegression(random_state=42, max_iter=1000),
        X=X_train_scaled,
        y=y_train,
        cv=kfold,
        scoring='accuracy'
    )
    print(f"K-Fold CV Accuracy: {kfold_scores.mean():.4f} (+/- {kfold_scores.std()*2:.4f})")
    
    # Stratified K-Fold CV
    skfold = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
    skfold_scores = cross_val_score(
        estimator=LogisticRegression(random_state=42, max_iter=1000),
        X=X_train_scaled,
        y=y_train,
        cv=skfold,
        scoring='accuracy'
    )
    print(f"Stratified K-Fold CV Accuracy: {skfold_scores.mean():.4f} (+/- {skfold_scores.std()*2:.4f})")
    
    # Multiple metrics with Stratified K-Fold
    print(f"\n--- Multiple Metrics (Stratified K-Fold) ---")
    multi_scores = perform_multiple_metric_cv(
        X_train_scaled, y_train, 
        LogisticRegression(random_state=42, max_iter=1000),
        n_splits=5
    )
    
    for metric, scores in multi_scores.items():
        if 'error' not in scores:
            print(f"{metric.upper()}: {scores['mean']:.4f} (+/- {scores['std']*2:.4f})")
        else:
            print(f"{metric.upper()}: Error - {scores['error']}")
    
    # Compare with different split ratios
    print(f"\n--- Evaluation with Different Split Ratios ---")
    split_results = evaluate_different_split_ratios(
        X, y, LogisticRegression, n_splits=5
    )
    
    print(f"{'Test Size':<12} {'Train Size':<12} {'Test Acc':<12} {'CV Mean':<12} {'CV Std':<12}")
    print("-" * 60)
    for test_size, result in split_results.items():
        print(f"{test_size:<12.1f} {result['train_size']:<12} {result['test_accuracy']:<12.4f} "
              f"{result['cv_mean']:<12.4f} {result['cv_std']:<12.4f}")
    
    print("\n" + "="*70)
    print("END OF BANKING EXAMPLE")
    print("="*70)
    
    return {
        'basic_accuracy': accuracy_score(y_test, y_pred),
        'kfold_cv_mean': kfold_scores.mean(),
        'skfold_cv_mean': skfold_scores.mean(),
        'multi_scores': multi_scores
    }


def healthcare_diagnosis_example():
    """
    Healthcare example: Medical Diagnosis with Cross-Validation
    
    This example demonstrates using cross-validation for medical diagnosis prediction,
    predicting whether a patient has a certain condition based on clinical features.
    """
    print("\n" + "="*70)
    print("HEALTHCARE EXAMPLE: Medical Diagnosis with Cross-Validation")
    print("="*70)
    
    # Generate synthetic medical diagnosis data
    np.random.seed(42)
    n_samples = 1200
    
    # Generate clinical features
    # age, blood_pressure_systolic, blood_pressure_diastolic, cholesterol_total,
    # glucose_level, bmi, heart_rate, oxygen_saturation
    age = np.random.normal(loc=50, scale=15, size=n_samples)
    age = np.clip(age, 18, 100)
    
    bp_systolic = np.random.normal(loc=120, scale=20, size=n_samples)
    bp_diastolic = np.random.normal(loc=80, scale=15, size=n_samples)
    
    cholesterol = np.random.normal(loc=200, scale=40, size=n_samples)
    glucose = np.random.lognormal(mean=4.5, sigma=0.5, size=n_samples)
    bmi = np.random.normal(loc=27, scale=5, size=n_samples)
    heart_rate = np.random.normal(loc=72, scale=10, size=n_samples)
    oxygen_saturation = np.random.normal(loc=98, scale=2, size=n_samples)
    oxygen_saturation = np.clip(oxygen_saturation, 90, 100)
    
    # Create feature matrix
    X = np.column_stack([
        age, bp_systolic, bp_diastolic, cholesterol, glucose,
        bmi, heart_rate, oxygen_saturation
    ])
    
    # Create target variable (1 = has condition, 0 = no condition)
    # Using a risk model based on clinical indicators
    condition_prob = (
        0.05 +
        0.15 * (age > 60).astype(float) +
        0.10 * (bp_systolic > 140).astype(float) +
        0.10 * (bp_diastolic > 90).astype(float) +
        0.10 * (cholesterol > 240).astype(float) +
        0.10 * (glucose > 5.5).astype(float) +
        0.08 * (bmi > 30).astype(float) +
        0.08 * (oxygen_saturation < 95).astype(float)
    )
    condition_prob = np.clip(condition_prob, 0, 1)
    y = (np.random.random(n_samples) < condition_prob).astype(int)
    
    print(f"\nDataset Size: {n_samples} samples")
    print(f"Features: age, bp_systolic, bp_diastolic, cholesterol, glucose, bmi, heart_rate, oxygen_saturation")
    print(f"Class Distribution: Positive={y.sum()}, Negative={n_samples-y.sum()}")
    print(f"Condition Prevalence: {y.mean()*100:.2f}%")
    
    # Use stratified split
    X_train, X_test, y_train, y_test = train_test_split_stratified(
        X, y, test_size=0.2, random_state=42
    )
    
    print(f"\nTrain Set: {len(X_train)} samples")
    print(f"Test Set: {len(X_test)} samples")
    print(f"Train Positive Rate: {y_train.mean()*100:.2f}%")
    print(f"Test Positive Rate: {y_test.mean()*100:.2f}%")
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # Train Random Forest model
    rf_model = RandomForestClassifier(
        n_estimators=100,
        max_depth=10,
        random_state=42,
        n_jobs=-1
    )
    rf_model.fit(X_train_scaled, y_train)
    
    # Basic evaluation
    y_pred = rf_model.predict(X_test_scaled)
    y_pred_proba = rf_model.predict_proba(X_test_scaled)[:, 1]
    
    print(f"\n--- Basic Test Set Evaluation (Random Forest) ---")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred):.4f}")
    print(f"Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"F1 Score: {f1_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_pred_proba):.4f}")
    
    # Cross-validation evaluation
    print(f"\n--- Cross-Validation Evaluation ---")
    
    # Compare different models
    models = {
        'Logistic Regression': LogisticRegression(random_state=42, max_iter=1000),
        'Random Forest': RandomForestClassifier(n_estimators=100, max_depth=10, random_state=42),
        'Gradient Boosting': GradientBoostingClassifier(n_estimators=100, max_depth=5, random_state=42)
    }
    
    cv_results = {}
    
    for model_name, model in models.items():
        skfold = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
        
        # Accuracy
        accuracy_scores = cross_val_score(
            estimator=model,
            X=X_train_scaled,
            y=y_train,
            cv=skfold,
            scoring='accuracy'
        )
        
        # ROC-AUC
        roc_scores = cross_val_score(
            estimator=model,
            X=X_train_scaled,
            y=y_train,
            cv=skfold,
            scoring='roc_auc'
        )
        
        # F1
        f1_scores = cross_val_score(
            estimator=model,
            X=X_train_scaled,
            y=y_train,
            cv=skfold,
            scoring='f1'
        )
        
        cv_results[model_name] = {
            'accuracy': (accuracy_scores.mean(), accuracy_scores.std()),
            'roc_auc': (roc_scores.mean(), roc_scores.std()),
            'f1': (f1_scores.mean(), f1_scores.std())
        }
        
        print(f"\n{model_name}:")
        print(f"  Accuracy: {accuracy_scores.mean():.4f} (+/- {accuracy_scores.std()*2:.4f})")
        print(f"  ROC-AUC:  {roc_scores.mean():.4f} (+/- {roc_scores.std()*2:.4f})")
        print(f"  F1:       {f1_scores.mean():.4f} (+/- {f1_scores.std()*2:.4f})")
    
    # Select best model based on CV
    best_model_name = max(cv_results.keys(), 
                         key=lambda x: cv_results[x]['roc_auc'][0])
    print(f"\n--- Best Model: {best_model_name} (based on ROC-AUC) ---")
    
    # Train best model on full training set
    best_model = models[best_model_name]
    best_model.fit(X_train_scaled, y_train)
    
    # Final evaluation on test set
    y_pred_best = best_model.predict(X_test_scaled)
    y_pred_proba_best = best_model.predict_proba(X_test_scaled)[:, 1]
    
    print(f"\n--- Final Test Set Evaluation ({best_model_name}) ---")
    print(f"Accuracy: {accuracy_score(y_test, y_pred_best):.4f}")
    print(f"ROC-AUC:  {roc_auc_score(y_test, y_pred_proba_best):.4f}")
    
    # Confusion matrix
    cm = confusion_matrix(y_test, y_pred_best)
    print(f"\nConfusion Matrix:")
    print(f"                 Predicted")
    print(f"              Neg    Pos")
    print(f"Actual Neg   {cm[0,0]:4d}  {cm[0,1]:4d}")
    print(f"Actual Pos   {cm[1,0]:4d}  {cm[1,1]:4d}")
    
    # Classification report
    print(f"\nClassification Report:")
    print(classification_report(y_test, y_pred_best, target_names=['Negative', 'Positive']))
    
    print("\n" + "="*70)
    print("END OF HEALTHCARE EXAMPLE")
    print("="*70)
    
    return {
        'cv_results': cv_results,
        'best_model': best_model_name,
        'test_accuracy': accuracy_score(y_test, y_pred_best),
        'test_roc_auc': roc_auc_score(y_test, y_pred_proba_best)
    }


def compare_cv_strategies(X, y, model):
    """
    Compare different cross-validation strategies.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        Target values
    model : estimator
        Machine learning model
    
    Returns:
    --------
    results : dict
        Results for each CV strategy
    """
    print("\n--- Comparing CV Strategies ---")
    
    # Standard K-Fold
    kfold = KFold(n_splits=5, shuffle=True, random_state=42)
    kfold_scores = cross_val_score(model, X, y, cv=kfold, scoring='accuracy')
    print(f"K-Fold (5):           {kfold_scores.mean():.4f} (+/- {kfold_scores.std()*2:.4f})")
    
    # Stratified K-Fold
    skfold = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
    skfold_scores = cross_val_score(model, X, y, cv=skfold, scoring='accuracy')
    print(f"Stratified K-Fold(5): {skfold_scores.mean():.4f} (+/- {skfold_scores.std()*2:.4f})")
    
    # Leave-One-Out
    loo = LeaveOneOut()
    loo_scores = cross_val_score(model, X, y, cv=loo, scoring='accuracy')
    print(f"Leave-One-Out:        {loo_scores.mean():.4f} (+/- {loo_scores.std()*2:.4f})")
    
    # Shuffle Split
    ss = ShuffleSplit(n_splits=5, test_size=0.2, random_state=42)
    ss_scores = cross_val_score(model, X, y, cv=ss, scoring='accuracy')
    print(f"Shuffle Split(5):    {ss_scores.mean():.4f} (+/- {ss_scores.std()*2:.4f})")
    
    return {
        'kfold': kfold_scores,
        'stratified_kfold': skfold_scores,
        'leave_one_out': loo_scores,
        'shuffle_split': ss_scores
    }


def test_data_splitting():
    """
    Test function to validate data splitting and cross-validation implementations.
    """
    print("\n" + "="*70)
    print("TESTING: Data Splitting and Cross-Validation")
    print("="*70)
    
    # Generate test data
    X, y, _ = generate_classification_data(
        n_samples=200, 
        n_features=5, 
        random_state=42
    )
    
    print(f"\nTest Data: {X.shape[0]} samples, {X.shape[1]} features")
    print(f"Class Distribution: {np.bincount(y)}")
    
    # Test 1: Basic train/test split
    print("\n--- Test 1: Basic Train/Test Split ---")
    X_train, X_test, y_train, y_test = train_test_split_basic(X, y, test_size=0.2)
    print(f"Train: {X_train.shape[0]}, Test: {X_test.shape[0]}")
    assert len(X_train) + len(X_test) == len(X), "Split size mismatch"
    print("PASSED")
    
    # Test 2: Stratified train/test split
    print("\n--- Test 2: Stratified Train/Test Split ---")
    X_train, X_test, y_train, y_test = train_test_split_stratified(X, y, test_size=0.2)
    train_ratio = y_train.mean()
    test_ratio = y_test.mean()
    print(f"Train positive ratio: {train_ratio:.4f}")
    print(f"Test positive ratio:  {test_ratio:.4f}")
    assert abs(train_ratio - test_ratio) < 0.1, "Stratification failed"
    print("PASSED")
    
    # Test 3: K-Fold CV
    print("\n--- Test 3: K-Fold Cross-Validation ---")
    model = LogisticRegression(random_state=42, max_iter=1000)
    cv_scores = perform_kfold_cv(X, y, model, n_splits=5)
    print(f"CV Scores: {cv_scores}")
    print(f"Mean: {cv_scores.mean():.4f}, Std: {cv_scores.std():.4f}")
    assert len(cv_scores) == 5, "Wrong number of folds"
    print("PASSED")
    
    # Test 4: Stratified K-Fold CV
    print("\n--- Test 4: Stratified K-Fold Cross-Validation ---")
    cv_scores = perform_stratified_kfold_cv(X, y, model, n_splits=5)
    print(f"CV Scores: {cv_scores}")
    print(f"Mean: {cv_scores.mean():.4f}, Std: {cv_scores.std():.4f}")
    assert len(cv_scores) == 5, "Wrong number of folds"
    print("PASSED")
    
    # Test 5: Compare CV strategies
    print("\n--- Test 5: Compare CV Strategies ---")
    results = compare_cv_strategies(X, y, model)
    print("PASSED")
    
    print("\n" + "="*70)
    print("ALL TESTS PASSED")
    print("="*70)
    
    return True


def main():
    """Main execution function."""
    print("="*70)
    print("Data Splitting and Cross-Validation - Comprehensive Implementation")
    print("="*70)
    
    print("\nI. GENERATING SYNTHETIC DATA")
    X, y, feature_names = generate_classification_data(
        n_samples=500, 
        n_features=10, 
        random_state=42
    )
    print(f"Generated {X.shape[0]} samples with {X.shape[1]} features")
    print(f"Target distribution: Class 0: {np.sum(y==0)}, Class 1: {np.sum(y==1)}")
    
    print("\nII. BASIC DATA SPLITTING")
    X_train, X_test, y_train, y_test = train_test_split_basic(X, y, test_size=0.2)
    print(f"Train/Test Split (80/20): Train={len(X_train)}, Test={len(X_test)}")
    
    print("\nIII. STRATIFIED DATA SPLITTING")
    X_train, X_test, y_train, y_test = train_test_split_stratified(X, y, test_size=0.2)
    print(f"Stratified Split: Train={len(X_train)}, Test={len(X_test)}")
    print(f"Train class distribution: {np.bincount(y_train)}")
    print(f"Test class distribution: {np.bincount(y_test)}")
    
    print("\nIV. CROSS-VALIDATION DEMONSTRATION")
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42, max_iter=1000)
    
    print("\nK-Fold CV:")
    kfold_scores = perform_kfold_cv(X_train_scaled, y_train, model, n_splits=5)
    print(f"  Scores: {kfold_scores}")
    print(f"  Mean: {kfold_scores.mean():.4f}")
    
    print("\nStratified K-Fold CV:")
    skfold_scores = perform_stratified_kfold_cv(X_train_scaled, y_train, model, n_splits=5)
    print(f"  Scores: {skfold_scores}")
    print(f"  Mean: {skfold_scores.mean():.4f}")
    
    # V. BANKING EXAMPLE
    banking_results = banking_credit_scoring_example()
    
    # VI. HEALTHCARE EXAMPLE
    healthcare_results = healthcare_diagnosis_example()
    
    # VII. TESTING
    test_data_splitting()
    
    print("\n" + "="*70)
    print("VIII. CONCLUSION")
    print("="*70)
    print("""
Data Splitting and Cross-Validation are essential techniques for:
1. Evaluating model performance reliably
2. Preventing overfitting
3. Ensuring model generalizability
4. Selecting the best model and hyperparameters

Key takeaways:
- Use stratified splitting for imbalanced datasets
- Stratified K-Fold CV is recommended for classification
- Consider multiple metrics beyond accuracy
- Different CV strategies may yield different results
""")
    
    print("="*70)
    print("EXECUTION COMPLETE")
    print("="*70)


if __name__ == "__main__":
    main()