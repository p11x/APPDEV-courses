# Topic: ML Workflow and Methodology
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for ML Workflow and Methodology

I. INTRODUCTION
Machine Learning (ML) workflow represents a systematic approach to building, deploying, 
and maintaining ML models. This module covers the end-to-end lifecycle from data 
collection to model deployment, emphasizing best practices and methodology.

II. CORE_CONCEPTS
- Data Collection and Understanding
- Data Preprocessing and Feature Engineering
- Model Selection and Training
- Model Evaluation and Validation
- Model Deployment and Monitoring
- Cross-validation and hyperparameter tuning

III. IMPLEMENTATION
- Synthetic data generation for classification and regression
- Preprocessing pipelines with scaling
- Train/test split strategies
- Model training workflows
- Comprehensive evaluation metrics

IV. EXAMPLES (Banking + Healthcare)
- Banking: Loan approval prediction workflow
- Healthcare: Patient diagnosis classification

V. OUTPUT_RESULTS
- Detailed performance metrics printing
- Visualization-ready outputs

VI. TESTING
- Unit tests for each component
- Integration tests for workflows

VII. ADVANCED_TOPICS
- Cross-validation strategies
- Feature selection methods
- Pipeline automation
- Model persistence

VIII. CONCLUSION
- Best practices summary
- Future considerations
"""

import numpy as np
import pandas as pd
from sklearn.datasets import make_classification, make_regression
from sklearn.model_selection import train_test_split, cross_val_score, KFold
from sklearn.preprocessing import StandardScaler, MinMaxScaler
from sklearn.linear_model import LinearRegression, LogisticRegression
from sklearn.metrics import (
    mean_squared_error, 
    accuracy_score, 
    r2_score,
    precision_score,
    recall_score,
    f1_score,
    roc_auc_score,
    confusion_matrix,
    classification_report
)
import warnings
warnings.filterwarnings('ignore')


# =============================================================================
# I. DATA GENERATION FUNCTIONS
# =============================================================================

def generate_classification_data(n_samples=500, n_features=10, random_state=42):
    """
    Generate synthetic classification data for ML workflow demonstration.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features (informative + redundant)
    random_state : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray
        Feature matrix of shape (n_samples, n_features)
    y : ndarray
        Target labels of shape (n_samples,)
    feature_names : list
        Names of features
    """
    n_informative = int(n_features * 0.6)
    n_redundant = int(n_features * 0.3)
    n_clusters_per_class = 2
    
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_clusters_per_class=n_clusters_per_class,
        random_state=random_state,
        flip_y=0.05
    )
    
    feature_names = [f'Feature_{i+1}' for i in range(n_features)]
    
    print(f"Generated classification data: {X.shape[0]} samples, {X.shape[1]} features")
    print(f"Class distribution: Class 0: {np.sum(y==0)}, Class 1: {np.sum(y==1)}")
    
    return X, y, feature_names


def generate_regression_data(n_samples=500, n_features=10, random_state=42):
    """
    Generate synthetic regression data for ML workflow demonstration.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features
    random_state : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target values
    feature_names : list
        Names of features
    """
    X, y = make_regression(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=int(n_features * 0.8),
        noise=10.0,
        random_state=random_state
    )
    
    feature_names = [f'Feature_{i+1}' for i in range(n_features)]
    
    print(f"Generated regression data: {X.shape[0]} samples, {X.shape[1]} features")
    print(f"Target range: [{y.min():.2f}, {y.max():.2f}], Mean: {y.mean():.2f}")
    
    return X, y, feature_names


def generate_sample_data(n_samples=500):
    """
    Generate both classification and regression datasets.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples for each dataset
    
    Returns:
    --------
    dict containing all generated datasets
    """
    data = {}
    
    X_class, y_class, feature_names = generate_classification_data(
        n_samples=n_samples, 
        n_features=10,
        random_state=42
    )
    data['classification'] = {
        'X': X_class,
        'y': y_class,
        'feature_names': feature_names
    }
    
    X_reg, y_reg, feature_names = generate_regression_data(
        n_samples=n_samples,
        n_features=10,
        random_state=42
    )
    data['regression'] = {
        'X': X_reg,
        'y': y_reg,
        'feature_names': feature_names
    }
    
    return data


# =============================================================================
# II. PREPROCESSING FUNCTIONS
# =============================================================================

def apply_standard_scaling(X_train, X_test):
    """
    Apply StandardScaler to normalize features.
    
    Parameters:
    -----------
    X_train : ndarray
        Training feature matrix
    X_test : ndarray
        Test feature matrix
    
    Returns:
    --------
    X_train_scaled, X_test_scaled : ndarray
        Scaled feature matrices
    scaler : StandardScaler
        Fitted scaler object
    """
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("Applied StandardScaler normalization")
    print(f"Training data - Mean: {X_train_scaled.mean():.4f}, Std: {X_train_scaled.std():.4f}")
    print(f"Test data - Mean: {X_test_scaled.mean():.4f}, Std: {X_test_scaled.std():.4f}")
    
    return X_train_scaled, X_test_scaled, scaler


def apply_minmax_scaling(X_train, X_test):
    """
    Apply MinMaxScaler to normalize features to [0, 1] range.
    
    Parameters:
    -----------
    X_train : ndarray
        Training feature matrix
    X_test : ndarray
        Test feature matrix
    
    Returns:
    --------
    Scaled arrays and fitted scaler
    """
    scaler = MinMaxScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("Applied MinMaxScaler normalization to [0, 1] range")
    
    return X_train_scaled, X_test_scaled, scaler


def preprocess_data(X, y, test_size=0.2, random_state=42, scale=True):
    """
    Complete preprocessing pipeline: split and scale data.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        Target values
    test_size : float
        Proportion of test set
    random_state : int
        Random seed
    scale : bool
        Whether to apply scaling
    
    Returns:
    --------
    Preprocessed data dictionaries
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, 
        test_size=test_size,
        random_state=random_state
    )
    
    print(f"\nData Split Results:")
    print(f"Training set: {X_train.shape[0]} samples")
    print(f"Test set: {X_test.shape[0]} samples")
    
    scaler = None
    if scale:
        X_train, X_test, scaler = apply_standard_scaling(X_train, X_test)
    
    return X_train, X_test, y_train, y_test, scaler


# =============================================================================
# III. MODEL TRAINING FUNCTIONS
# =============================================================================

def train_classification_model(X_train, y_train, X_test, y_test):
    """
    Train and evaluate a Logistic Regression classifier.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test feature matrices
    y_train, y_test : ndarray
        Training and test labels
    
    Returns:
    --------
    model : trained model
    metrics : dictionary of evaluation metrics
    """
    model = LogisticRegression(random_state=42, max_iter=1000)
    model.fit(X_train, y_train)
    
    y_pred = model.predict(X_test)
    y_pred_proba = model.predict_proba(X_test)[:, 1]
    
    metrics = {
        'accuracy': accuracy_score(y_test, y_pred),
        'precision': precision_score(y_test, y_pred),
        'recall': recall_score(y_test, y_pred),
        'f1_score': f1_score(y_test, y_pred),
        'roc_auc': roc_auc_score(y_test, y_pred_proba),
        'confusion_matrix': confusion_matrix(y_test, y_pred)
    }
    
    print("\nClassification Model Results:")
    print(f"Accuracy: {metrics['accuracy']:.4f}")
    print(f"Precision: {metrics['precision']:.4f}")
    print(f"Recall: {metrics['recall']:.4f}")
    print(f"F1-Score: {metrics['f1_score']:.4f}")
    print(f"ROC-AUC: {metrics['roc_auc']:.4f}")
    print(f"Confusion Matrix:\n{metrics['confusion_matrix']}")
    
    return model, metrics


def train_regression_model(X_train, y_train, X_test, y_test):
    """
    Train and evaluate a Linear Regression model.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test feature matrices
    y_train, y_test : ndarray
        Training and test target values
    
    Returns:
    --------
    model : trained model
    metrics : dictionary of evaluation metrics
    """
    model = LinearRegression()
    model.fit(X_train, y_train)
    
    y_pred = model.predict(X_test)
    
    metrics = {
        'mse': mean_squared_error(y_test, y_pred),
        'rmse': np.sqrt(mean_squared_error(y_test, y_pred)),
        'r2_score': r2_score(y_test, y_pred),
        'mae': np.mean(np.abs(y_test - y_pred))
    }
    
    print("\nRegression Model Results:")
    print(f"MSE: {metrics['mse']:.4f}")
    print(f"RMSE: {metrics['rmse']:.4f}")
    print(f"R2-Score: {metrics['r2_score']:.4f}")
    print(f"MAE: {metrics['mae']:.4f}")
    
    return model, metrics


def cross_validate_model(X, y, model_type='classification', cv=5):
    """
    Perform k-fold cross-validation.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        Target values
    model_type : str
        'classification' or 'regression'
    cv : int
        Number of folds
    
    Returns:
    --------
    cv_scores : ndarray
        Cross-validation scores
    """
    kfold = KFold(n_splits=cv, shuffle=True, random_state=42)
    
    if model_type == 'classification':
        model = LogisticRegression(random_state=42, max_iter=1000)
        scoring = 'accuracy'
    else:
        model = LinearRegression()
        scoring = 'r2'
    
    cv_scores = cross_val_score(model, X, y, cv=kfold, scoring=scoring)
    
    print(f"\nCross-Validation Results ({cv}-Fold):")
    print(f"Scores: {cv_scores}")
    print(f"Mean: {cv_scores.mean():.4f}")
    print(f"Std: {cv_scores.std():.4f}")
    
    return cv_scores


# =============================================================================
# IV. EXAMPLE: BANKING/LOAN APPROVAL WORKFLOW
# =============================================================================

def generate_banking_data(n_samples=500):
    """
    Generate synthetic banking data for loan approval prediction.
    
    Features:
    - Credit Score (300-850)
    - Annual Income
    - Debt-to-Income Ratio
    - Employment Length
    - Loan Amount
    - Interest Rate
    - Existing Loans Count
    - Payment History Score
    - Savings Balance
    - Requested Term
    
    Target: Loan Approval (1=Approved, 0=Denied)
    """
    np.random.seed(42)
    
    n_samples = int(n_samples)
    
    credit_score = np.random.normal(650, 100, n_samples).clip(300, 850)
    annual_income = np.random.exponential(50000, n_samples) + 20000
    debt_to_income = np.random.uniform(0.1, 0.5, n_samples)
    employment_length = np.random.exponential(5, n_samples)
    loan_amount = np.random.uniform(1000, 50000, n_samples)
    interest_rate = np.random.uniform(3, 15, n_samples)
    existing_loans = np.random.poisson(1.5, n_samples)
    payment_history = np.random.normal(0.7, 0.2, n_samples).clip(0, 1)
    savings_balance = np.random.exponential(10000, n_samples)
    loan_term = np.random.choice([12, 24, 36, 48, 60], n_samples)
    
    X = np.column_stack([
        credit_score,
        annual_income,
        debt_to_income,
        employment_length,
        loan_amount,
        interest_rate,
        existing_loans,
        payment_history,
        savings_balance,
        loan_term
    ])
    
    approval_probs = (
        0.3 * (credit_score / 850) +
        0.2 * (annual_income / 100000) +
        0.2 * (1 - debt_to_income) +
        0.15 * payment_history +
        0.15 * (savings_balance / 50000)
    )
    
    y = (approval_probs + np.random.normal(0, 0.1, n_samples) > 0.5).astype(int)
    
    feature_names = [
        'Credit_Score', 'Annual_Income', 'Debt_to_Income_Ratio',
        'Employment_Length', 'Loan_Amount', 'Interest_Rate',
        'Existing_Loans', 'Payment_History', 'Savings_Balance', 'Loan_Term'
    ]
    
    print(f"\nBanking Data Generated: {n_samples} loan applications")
    print(f"Approved: {np.sum(y==1)}, Denied: {np.sum(y==0)}")
    
    return X, y, feature_names


def banking_example():
    """
    Complete banking/loan approval ML workflow example.
    """
    print("\n" + "="*60)
    print("EXAMPLE: BANKING - LOAN APPROVAL PREDICTION")
    print("="*60)
    
    X, y, feature_names = generate_banking_data(n_samples=500)
    
    print("\n--- Step 1: Data Preprocessing ---")
    X_train, X_test, y_train, y_test, scaler = preprocess_data(
        X, y, test_size=0.2, random_state=42, scale=True
    )
    
    print("\n--- Step 2: Model Training ---")
    model, metrics = train_classification_model(X_train, y_train, X_test, y_test)
    
    print("\n--- Step 3: Cross-Validation ---")
    cv_scores = cross_validate_model(X, y, model_type='classification', cv=5)
    
    print("\n--- Step 4: Feature Importance ---")
    print("Feature Coefficients:")
    for name, coef in zip(feature_names, model.coef_[0]):
        print(f"  {name}: {coef:.4f}")
    
    results = {
        'model': model,
        'metrics': metrics,
        'cv_scores': cv_scores,
        'scaler': scaler
    }
    
    print("\n" + "-"*60)
    print("Banking Workflow Complete")
    print("-"*60)
    
    return results


# =============================================================================
# V. EXAMPLE: HEALTHCARE/DIAGNOSIS WORKFLOW
# =============================================================================

def generate_healthcare_data(n_samples=500):
    """
    Generate synthetic healthcare data for diagnosis prediction.
    
    Features:
    - Age
    - BMI
    - Blood Pressure Systolic
    - Blood Pressure Diastolic
    - Heart Rate
    - Glucose Level
    - Cholesterol Total
    - White Blood Cell Count
    - Hemoglobin
    - Family History (binary)
    
    Target: Disease Presence (1=Positive, 0=Negative)
    """
    np.random.seed(42)
    
    n_samples = int(n_samples)
    
    age = np.random.normal(50, 15, n_samples).clip(18, 90)
    bmi = np.random.normal(27, 5, n_samples).clip(15, 45)
    bp_systolic = np.random.normal(130, 20, n_samples).clip(90, 200)
    bp_diastolic = np.random.normal(85, 12, n_samples).clip(60, 130)
    heart_rate = np.random.normal(72, 10, n_samples).clip(50, 120)
    glucose = np.random.normal(100, 25, n_samples).clip(70, 200)
    cholesterol = np.random.normal(200, 40, n_samples).clip(120, 300)
    wbc = np.random.normal(7000, 2000, n_samples).clip(4000, 15000)
    hemoglobin = np.random.normal(14, 2, n_samples).clip(8, 18)
    family_history = np.random.choice([0, 1], n_samples)
    
    X = np.column_stack([
        age, bmi, bp_systolic, bp_diastolic, heart_rate,
        glucose, cholesterol, wbc, hemoglobin, family_history
    ])
    
    risk_probs = (
        0.15 * (age > 55).astype(int) +
        0.12 * (bmi > 30).astype(int) +
        0.15 * (bp_systolic > 140).astype(int) +
        0.10 * (glucose > 126).astype(int) +
        0.10 * (cholesterol > 240).astype(int) +
        0.08 * family_history +
        0.05
    )
    
    y = (risk_probs + np.random.normal(0, 0.1, n_samples) > 0.35).astype(int)
    
    feature_names = [
        'Age', 'BMI', 'BP_Systolic', 'BP_Diastolic', 'Heart_Rate',
        'Glucose', 'Cholesterol', 'WBC_Count', 'Hemoglobin', 'Family_History'
    ]
    
    print(f"\nHealthcare Data Generated: {n_samples} patient records")
    print(f"Positive Diagnosis: {np.sum(y==1)}, Negative: {np.sum(y==0)}")
    
    return X, y, feature_names


def healthcare_example():
    """
    Complete healthcare/diagnosis ML workflow example.
    """
    print("\n" + "="*60)
    print("EXAMPLE: HEALTHCARE - DISEASE DIAGNOSIS PREDICTION")
    print("="*60)
    
    X, y, feature_names = generate_healthcare_data(n_samples=500)
    
    print("\n--- Step 1: Data Preprocessing ---")
    X_train, X_test, y_train, y_test, scaler = preprocess_data(
        X, y, test_size=0.2, random_state=42, scale=True
    )
    
    print("\n--- Step 2: Model Training ---")
    model, metrics = train_classification_model(X_train, y_train, X_test, y_test)
    
    print("\n--- Step 3: Cross-Validation ---")
    cv_scores = cross_validate_model(X, y, model_type='classification', cv=5)
    
    print("\n--- Step 4: Feature Importance ---")
    print("Feature Coefficients:")
    for name, coef in zip(feature_names, model.coef_[0]):
        print(f"  {name}: {coef:.4f}")
    
    results = {
        'model': model,
        'metrics': metrics,
        'cv_scores': cv_scores,
        'scaler': scaler
    }
    
    print("\n" + "-"*60)
    print("Healthcare Workflow Complete")
    print("-"*60)
    
    return results


# =============================================================================
# VI. CORE IMPLEMENTATION FUNCTION
# =============================================================================

def core_implementation():
    """
    Execute complete ML workflow with both classification and regression.
    """
    print("\n" + "="*60)
    print("CORE IMPLEMENTATION: ML WORKFLOW AND METHODOLOGY")
    print("="*60)
    
    print("\n--- Classification Workflow ---")
    X, y, feature_names = generate_classification_data(n_samples=500)
    
    X_train, X_test, y_train, y_test, scaler = preprocess_data(
        X, y, test_size=0.2, random_state=42, scale=True
    )
    
    model, metrics = train_classification_model(X_train, y_train, X_test, y_test)
    
    cv_scores = cross_validate_model(X, y, model_type='classification')
    
    print("\n--- Regression Workflow ---")
    X, y, feature_names = generate_regression_data(n_samples=500)
    
    X_train, X_test, y_train, y_test, scaler = preprocess_data(
        X, y, test_size=0.2, random_state=42, scale=True
    )
    
    model, metrics = train_regression_model(X_train, y_train, X_test, y_test)
    
    cv_scores = cross_validate_model(X, y, model_type='regression')
    
    print("\n" + "-"*60)
    print("Core Implementation Complete")
    print("-"*60)
    
    return True


# =============================================================================
# VII. ADVANCED TOPICS
# =============================================================================

def advanced_cross_validation(X, y, model_type='classification'):
    """
    Advanced cross-validation with multiple strategies.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        Target values
    model_type : str
        Type of ML problem
    
    Returns:
    --------
    Dictionary with results from different CV strategies
    """
    print("\n--- Advanced Cross-Validation Strategies ---")
    
    results = {}
    
    kfold_5 = KFold(n_splits=5, shuffle=True, random_state=42)
    kfold_10 = KFold(n_splits=10, shuffle=True, random_state=42)
    
    if model_type == 'classification':
        model = LogisticRegression(random_state=42, max_iter=1000)
        scoring = 'accuracy'
    else:
        model = LinearRegression()
        scoring = 'r2'
    
    scores_5 = cross_val_score(model, X, y, cv=kfold_5, scoring=scoring)
    scores_10 = cross_val_score(model, X, y, cv=kfold_10, scoring=scoring)
    
    results['5-fold'] = {'mean': scores_5.mean(), 'std': scores_5.std()}
    results['10-fold'] = {'mean': scores_10.mean(), 'std': scores_10.std()}
    
    print(f"5-Fold CV: {results['5-fold']['mean']:.4f} (+/- {results['5-fold']['std']:.4f})")
    print(f"10-Fold CV: {results['10-fold']['mean']:.4f} (+/- {results['10-fold']['std']:.4f})")
    
    return results


def feature_importance_analysis(X, y, feature_names):
    """
    Analyze feature importance using model coefficients.
    """
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42, max_iter=1000)
    model.fit(X_train_scaled, y_train)
    
    print("\n--- Feature Importance Analysis ---")
    importance = np.abs(model.coef_[0])
    indices = np.argsort(importance)[::-1]
    
    for i in indices:
        print(f"  {feature_names[i]}: {importance[i]:.4f}")
    
    return feature_names, importance


# =============================================================================
# VIII. TESTING FUNCTIONS
# =============================================================================

def test_data_generation():
    """Test data generation functions."""
    print("\n=== Testing: Data Generation ===")
    
    X, y, names = generate_classification_data(n_samples=100)
    assert X.shape == (100, 10), "Classification data shape mismatch"
    assert len(y) == 100, "Target length mismatch"
    assert len(names) == 10, "Feature names length mismatch"
    print("Data generation tests passed")
    
    X, y, names = generate_regression_data(n_samples=100)
    assert X.shape == (100, 10), "Regression data shape mismatch"
    print("Regression data tests passed")


def test_preprocessing():
    """Test preprocessing functions."""
    print("\n=== Testing: Preprocessing ===")
    
    X, y, _ = generate_classification_data(n_samples=100)
    X_train, X_test, y_train, y_test, scaler = preprocess_data(
        X, y, test_size=0.2, random_state=42
    )
    
    assert X_train.shape[0] == 80, "Training set size mismatch"
    assert X_test.shape[0] == 20, "Test set size mismatch"
    assert scaler is not None, "Scaler not returned"
    print("Preprocessing tests passed")


def test_training():
    """Test model training functions."""
    print("\n=== Testing: Model Training ===")
    
    X, y, _ = generate_classification_data(n_samples=100)
    X_train, X_test, y_train, y_test, _ = preprocess_data(X, y)
    
    model, metrics = train_classification_model(X_train, y_train, X_test, y_test)
    
    assert 'accuracy' in metrics, "Missing accuracy metric"
    assert 0 <= metrics['accuracy'] <= 1, "Accuracy out of range"
    print("Classification training tests passed")


def run_all_tests():
    """Run all testing functions."""
    print("\n" + "="*60)
    print("RUNNING ALL TESTS")
    print("="*60)
    
    test_data_generation()
    test_preprocessing()
    test_training()
    
    print("\n" + "-"*60)
    print("All Tests Completed Successfully")
    print("-"*60)


# =============================================================================
# IX. MAIN FUNCTION
# =============================================================================

def main():
    """
    Main execution function demonstrating complete ML workflow.
    """
    print("="*60)
    print("ML WORKFLOW AND METHODOLOGY IMPLEMENTATION")
    print("="*60)
    print(f"Execution Time: 06-04-2026")
    
    print("\n" + "="*60)
    print("SECTION I: INTRODUCTION")
    print("="*60)
    print("""
Machine Learning workflow is a systematic process that transforms raw data 
into actionable insights. This implementation covers:
- Data generation and preprocessing
- Model training and evaluation
- Industry-specific applications (Banking, Healthcare)
- Comprehensive testing and validation
""")
    
    print("\n" + "="*60)
    print("SECTION II: CORE CONCEPTS")
    print("="*60)
    print("""
Core ML Workflow Steps:
1. Data Collection - Gather and understand data
2. Data Preprocessing - Clean, transform, and scale data
3. Train/Test Split - Separate data for training and evaluation
4. Model Training - Train ML model on training data
5. Model Evaluation - Assess model performance on test data
6. Cross-Validation - Validate model using k-fold cross-validation
7. Model Deployment - Deploy model for production use
""")
    
    print("\n" + "="*60)
    print("SECTION III: CORE IMPLEMENTATION")
    print("="*60)
    core_implementation()
    
    print("\n" + "="*60)
    print("SECTION IV: BANKING EXAMPLE")
    print("="*60)
    banking_results = banking_example()
    
    print("\n" + "="*60)
    print("SECTION V: HEALTHCARE EXAMPLE")
    print("="*60)
    healthcare_results = healthcare_example()
    
    print("\n" + "="*60)
    print("SECTION VI: ADVANCED TOPICS")
    print("="*60)
    X, y, features = generate_classification_data(n_samples=200)
    advanced_cv_results = advanced_cross_validation(X, y)
    feature_importance_analysis(X, y, features)
    
    print("\n" + "="*60)
    print("SECTION VII: TESTING")
    print("="*60)
    run_all_tests()
    
    print("\n" + "="*60)
    print("SECTION VIII: CONCLUSION")
    print("="*60)
    print("""
Best Practices Summary:
1. Always use cross-validation for robust model evaluation
2. Preprocess data before training (scaling/normalization)
3. Use appropriate evaluation metrics for problem type
4. Test with multiple random seeds for stability
5. Document all preprocessing and model parameters
6. Monitor model performance over time

This implementation provides a complete ML workflow foundation that can be 
extended with more sophisticated models, feature engineering techniques, 
and deployment strategies.

Key Takeaways:
- Systematic approach ensures reproducibility
- Industry examples demonstrate real-world applicability
- Comprehensive testing validates implementation correctness
- Advanced topics prepare for production ML systems
""")


if __name__ == "__main__":
    main()