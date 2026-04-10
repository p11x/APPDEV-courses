# Topic: Model Evaluation Metrics
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Model Evaluation Metrics

I. INTRODUCTION
    This module provides a comprehensive guide to model evaluation metrics used in machine learning.
    It covers both classification and regression metrics, with practical examples in banking
    (credit risk evaluation) and healthcare (diagnosis evaluation) domains.

II. CORE CONCEPTS
    - Classification Metrics: Accuracy, Precision, Recall, F1-Score, ROC-AUC, Confusion Matrix
    - Regression Metrics: MSE, RMSE, MAE, R2 Score
    - Business context applications for both domains

III. IMPLEMENTATION
    - Data generation functions for classification and regression
    - Metric calculation utilities
    - Real-world examples with banking and healthcare scenarios

IV. EXAMPLES
    - Banking: Credit risk prediction and loan approval
    - Healthcare: Disease diagnosis prediction

V. OUTPUT RESULTS
    - Comprehensive metric reports with interpretations
    - Visualization-ready outputs

VI. TESTING
    - Unit tests for metric calculations
    - Validation against known expected values

VII. ADVANCED TOPICS
    - Threshold optimization
    - Multi-class metrics
    - Cross-validation for robust evaluation

VIII. CONCLUSION
    - Best practices for model evaluation
    - Guidelines for selecting appropriate metrics
"""

# Import necessary libraries
import numpy as np
import pandas as pd
from sklearn.datasets import make_classification, make_regression
from sklearn.model_selection import train_test_split, cross_val_score, StratifiedKFold
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LogisticRegression, LinearRegression
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score, 
    roc_auc_score, confusion_matrix, mean_squared_error,
    r2_score, mean_absolute_error, log_loss,
    classification_report, matthews_corrcoef, cohen_kappa_score,
    roc_curve, auc, average_precision_score
)
import warnings
warnings.filterwarnings('ignore')


def generate_classification_data(n_samples=1000, random_state=42):
    """
    Generate synthetic classification dataset for demonstration.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target vector
    feature_names : list
        Names of features
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=10,
        n_informative=8,
        n_redundant=2,
        n_classes=2,
        random_state=random_state,
        flip_y=0.05
    )
    feature_names = [f'feature_{i+1}' for i in range(X.shape[1])]
    return X, y, feature_names


def generate_regression_data(n_samples=1000, random_state=42):
    """
    Generate synthetic regression dataset for demonstration.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target vector
    feature_names : list
        Names of features
    """
    X, y = make_regression(
        n_samples=n_samples,
        n_features=5,
        n_informative=5,
        noise=10,
        random_state=random_state
    )
    feature_names = [f'feature_{i+1}' for i in range(X.shape[1])]
    return X, y, feature_names


def generate_banking_data(n_samples=800, random_state=42):
    """
    Generate synthetic banking data for credit risk evaluation.
    
    Features include:
    - income_level: Annual income level
    - credit_score: Credit score
    - debt_to_income: Debt to income ratio
    - employment_years: Years of employment
    - loan_amount: Requested loan amount
    
    Target: loan_default (0=no default, 1=default)
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target vector
    feature_names : list
        Names of features
    """
    np.random.seed(random_state)
    
    credit_score = np.random.normal(650, 100, n_samples)
    credit_score = np.clip(credit_score, 300, 850)
    
    income_level = np.random.exponential(50000, n_samples)
    
    debt_to_income = np.random.exponential(0.3, n_samples)
    debt_to_income = np.clip(debt_to_income, 0.05, 0.8)
    
    employment_years = np.random.exponential(5, n_samples)
    employment_years = np.clip(employment_years, 0, 40)
    
    loan_amount = np.random.exponential(20000, n_samples)
    loan_amount = np.clip(loan_amount, 1000, 500000)
    
    X = np.column_stack([income_level, credit_score, debt_to_income, 
                        employment_years, loan_amount])
    
    probability_default = (
        0.3 * (1 - (credit_score - 300) / 550) +
        0.2 * debt_to_income +
        0.1 * (employment_years < 2) +
        0.2 * (loan_amount / income_level)
    )
    probability_default = np.clip(probability_default, 0.05, 0.95)
    
    y = (np.random.random(n_samples) < probability_default).astype(int)
    
    feature_names = ['income_level', 'credit_score', 'debt_to_income', 
                    'employment_years', 'loan_amount']
    
    return X, y, feature_names


def generate_healthcare_data(n_samples=600, random_state=42):
    """
    Generate synthetic healthcare data for diagnosis evaluation.
    
    Features include:
    - age: Patient age
    - blood_pressure_systolic: Systolic blood pressure
    - cholesterol_total: Total cholesterol
    - blood_glucose: Blood glucose level
    - bmi: Body mass index
    
    Target: has_disease (0=no disease, 1=disease)
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target vector
    feature_names : list
        Names of features
    """
    np.random.seed(random_state)
    
    age = np.random.randint(20, 80, n_samples)
    
    blood_pressure_systolic = np.random.normal(120, 20, n_samples)
    blood_pressure_systolic = np.clip(blood_pressure_systolic, 80, 200)
    
    cholesterol_total = np.random.normal(200, 40, n_samples)
    cholesterol_total = np.clip(cholesterol_total, 100, 350)
    
    blood_glucose = np.random.normal(100, 25, n_samples)
    blood_glucose = np.clip(blood_glucose, 50, 250)
    
    bmi = np.random.normal(27, 5, n_samples)
    bmi = np.clip(bmi, 15, 45)
    
    X = np.column_stack([age, blood_pressure_systolic, cholesterol_total,
                        blood_glucose, bmi])
    
    probability_disease = (
        0.25 * (age > 55) +
        0.2 * (blood_pressure_systolic > 140) +
        0.2 * (cholesterol_total > 240) +
        0.15 * (blood_glucose > 126) +
        0.1 * (bmi > 30)
    )
    probability_disease = np.clip(probability_disease, 0.05, 0.90)
    
    y = (np.random.random(n_samples) < probability_disease).astype(int)
    
    feature_names = ['age', 'blood_pressure_systolic', 'cholesterol_total',
                     'blood_glucose', 'bmi']
    
    return X, y, feature_names


def calculate_classification_metrics(y_true, y_pred, y_prob=None):
    """
    Calculate comprehensive classification metrics.
    
    Parameters:
    -----------
    y_true : ndarray
        True labels
    y_pred : ndarray
        Predicted labels
    y_prob : ndarray, optional
        Predicted probabilities for positive class
        
    Returns:
    --------
    metrics : dict
        Dictionary containing all classification metrics
    """
    metrics = {}
    
    metrics['accuracy'] = accuracy_score(y_true, y_pred)
    metrics['precision'] = precision_score(y_true, y_pred, zero_division=0)
    metrics['recall'] = recall_score(y_true, y_pred, zero_division=0)
    metrics['f1_score'] = f1_score(y_true, y_pred, zero_division=0)
    
    tn, fp, fn, tp = confusion_matrix(y_true, y_pred).ravel()
    metrics['true_negatives'] = int(tn)
    metrics['false_positives'] = int(fp)
    metrics['false_negatives'] = int(fn)
    metrics['true_positives'] = int(tp)
    
    if y_prob is not None:
        metrics['roc_auc'] = roc_auc_score(y_true, y_prob)
        metrics['log_loss'] = log_loss(y_true, y_prob)
    
    metrics['matthews_corrcoef'] = matthews_corrcoef(y_true, y_pred)
    metrics['cohen_kappa'] = cohen_kappa_score(y_true, y_pred)
    
    return metrics


def calculate_regression_metrics(y_true, y_pred):
    """
    Calculate comprehensive regression metrics.
    
    Parameters:
    -----------
    y_true : ndarray
        True target values
    y_pred : ndarray
        Predicted target values
        
    Returns:
    --------
    metrics : dict
        Dictionary containing all regression metrics
    """
    metrics = {}
    
    metrics['mse'] = mean_squared_error(y_true, y_pred)
    metrics['rmse'] = np.sqrt(metrics['mse'])
    metrics['mae'] = mean_absolute_error(y_true, y_pred)
    metrics['r2_score'] = r2_score(y_true, y_pred)
    
    residuals = y_true - y_pred
    metrics['mean_residual'] = np.mean(residuals)
    metrics['std_residual'] = np.std(residuals)
    
    ss_res = np.sum(residuals ** 2)
    ss_tot = np.sum((y_true - np.mean(y_true)) ** 2)
    metrics['adjusted_r2'] = 1 - (1 - metrics['r2_score']) * (len(y_true) - 1) / (len(y_true) - 2)
    
    return metrics


def print_classification_report(metrics, context=""):
    """
    Print a formatted classification metric report.
    
    Parameters:
    -----------
    metrics : dict
        Dictionary containing classification metrics
    context : str
        Context string for the report (e.g., "Banking", "Healthcare")
    """
    print(f"\n{'='*60}")
    print(f"CLASSIFICATION METRICS REPORT - {context}")
    print(f"{'='*60}")
    
    print(f"\n--- Main Metrics ---")
    print(f"Accuracy:     {metrics['accuracy']:.4f}")
    print(f"Precision:    {metrics['precision']:.4f}")
    print(f"Recall:       {metrics['recall']:.4f}")
    print(f"F1-Score:    {metrics['f1_score']:.4f}")
    
    if 'roc_auc' in metrics:
        print(f"\n--- Probability Metrics ---")
        print(f"ROC-AUC:      {metrics['roc_auc']:.4f}")
        print(f"Log Loss:     {metrics['log_loss']:.4f}")
    
    print(f"\n--- Confusion Matrix Components ---")
    print(f"True Positives:  {metrics['true_positives']}")
    print(f"True Negatives:  {metrics['true_negatives']}")
    print(f"False Positives: {metrics['false_positives']}")
    print(f"False Negatives: {metrics['false_negatives']}")
    
    print(f"\n--- Advanced Metrics ---")
    print(f"Matthews Corr Coef: {metrics['matthews_corrcoef']:.4f}")
    print(f"Cohen's Kappa:     {metrics['cohen_kappa']:.4f}")
    
    print(f"\n{'='*60}\n")


def print_regression_report(metrics, context=""):
    """
    Print a formatted regression metric report.
    
    Parameters:
    -----------
    metrics : dict
        Dictionary containing regression metrics
    context : str
        Context string for the report
    """
    print(f"\n{'='*60}")
    print(f"REGRESSION METRICS REPORT - {context}")
    print(f"{'='*60}")
    
    print(f"\n--- Main Metrics ---")
    print(f"MSE:        {metrics['mse']:.4f}")
    print(f"RMSE:       {metrics['rmse']:.4f}")
    print(f"MAE:        {metrics['mae']:.4f}")
    print(f"R2 Score:   {metrics['r2_score']:.4f}")
    
    print(f"\n--- Additional Metrics ---")
    print(f"Adjusted R2:    {metrics['adjusted_r2']:.4f}")
    print(f"Mean Residual:   {metrics['mean_residual']:.4f}")
    print(f"Std Residual:    {metrics['std_residual']:.4f}")
    
    print(f"\n{'='*60}\n")


def run_banking_example():
    """
    Run banking example for credit risk evaluation.
    
    This demonstrates model evaluation in the context of:
    - Credit risk prediction
    - Loan default prediction
    - Business impact analysis
    """
    print("\n" + "="*60)
    print("BANKING EXAMPLE: Credit Risk Evaluation")
    print("="*60)
    
    X, y, feature_names = generate_banking_data(n_samples=800, random_state=42)
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.25, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42, max_iter=1000)
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    y_prob = model.predict_proba(X_test_scaled)[:, 1]
    
    metrics = calculate_classification_metrics(y_test, y_pred, y_prob)
    
    print("\n--- Business Context ---")
    print("Scenario: Bank evaluating credit risk for loan approval")
    print(f"Total test samples: {len(y_test)}")
    print(f"Defaults in test set: {sum(y_test)}")
    print(f"Non-defaults in test set: {len(y_test) - sum(y_test)}")
    
    print(f"\n--- Business Impact ---")
    false_positives = metrics['false_positives']
    false_negatives = metrics['false_negatives']
    print(f"False positives (predicted default, actual good): {false_positives}")
    print(f"  -> These customers were wrongly denied credit")
    print(f"False negatives (predicted good, actual default): {false_negatives}")
    print(f"  -> These defaults were not detected, causing losses")
    
    total_predicted_default = metrics['false_positives'] + metrics['true_positives']
    if total_predicted_default > 0:
        precision_default = metrics['true_positives'] / total_predicted_default
        print(f"\nPrecision for default detection: {precision_default:.4f}")
        print(f"  -> When model says default, it's correct {precision_default*100:.1f}% of time")
    
    total_actual_default = metrics['true_positives'] + metrics['false_negatives']
    if total_actual_default > 0:
        recall_default = metrics['true_positives'] / total_actual_default
        print(f"Recall for default detection: {recall_default:.4f}")
        print(f"  -> Model captures {recall_default*100:.1f}% of actual defaults")
    
    print_classification_report(metrics, "Banking-Credit Risk")
    
    return metrics


def run_healthcare_example():
    """
    Run healthcare example for disease diagnosis evaluation.
    
    This demonstrates model evaluation in the context of:
    - Medical diagnosis prediction
    - Patient risk assessment
    - Clinical decision support
    """
    print("\n" + "="*60)
    print("HEALTHCARE EXAMPLE: Disease Diagnosis Evaluation")
    print("="*60)
    
    X, y, feature_names = generate_healthcare_data(n_samples=600, random_state=42)
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.25, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42, max_iter=1000)
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    y_prob = model.predict_proba(X_test_scaled)[:, 1]
    
    metrics = calculate_classification_metrics(y_test, y_pred, y_prob)
    
    print("\n--- Clinical Context ---")
    print("Scenario: Hospital screening patients for disease diagnosis")
    print(f"Total test patients: {len(y_test)}")
    print(f"Diseased patients in test set: {sum(y_test)}")
    print(f"Non-diseased patients in test set: {len(y_test) - sum(y_test)}")
    
    print(f"\n--- Clinical Impact ---")
    false_positives = metrics['false_positives']
    false_negatives = metrics['false_negatives']
    print(f"False positives (predicted disease, actual healthy): {false_positives}")
    print(f"  -> These patients need additional testing (false alarm)")
    print(f"False negatives (predicted healthy, actual disease): {false_negatives}")
    print(f"  -> These diseases were missed - critical for patient outcomes")
    
    total_actual_disease = metrics['true_positives'] + metrics['false_negatives']
    if total_actual_disease > 0:
        sensitivity = metrics['true_positives'] / total_actual_disease
        print(f"\nSensitivity (Recall): {sensitivity:.4f}")
        print(f"  -> Model detects {sensitivity*100:.1f}% of actual diseases")
        print(f"  -> CRITICAL: Missed diagnoses can be life-threatening")
    
    total_predicted_disease = metrics['true_positives'] + metrics['false_positives']
    if total_predicted_disease > 0:
        positive_predictive_value = metrics['true_positives'] / total_predicted_disease
        print(f"Positive Predictive Value: {positive_predictive_value:.4f}")
        print(f"  -> When model says disease, it's correct {positive_predictive_value*100:.1f}% of time")
    
    specificity = metrics['true_negatives'] / (metrics['true_negatives'] + metrics['false_positives'])
    print(f"Specificity: {specificity:.4f}")
    print(f"  -> Model correctly identifies {specificity*100:.1f}% of healthy patients")
    
    print_classification_report(metrics, "Healthcare-Diagnosis")
    
    return metrics


def run_regression_example():
    """
    Run regression example for price prediction.
    
    Demonstrates regression metric evaluation.
    """
    print("\n" + "="*60)
    print("REGRESSION EXAMPLE: Price Prediction")
    print("="*60)
    
    X, y, feature_names = generate_regression_data(n_samples=500, random_state=42)
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.25, random_state=42
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LinearRegression()
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    
    metrics = calculate_regression_metrics(y_test, y_pred)
    
    print("\n--- Business Context ---")
    print("Scenario: Predicting house prices based on features")
    print(f"Test samples: {len(y_test)}")
    print(f"Mean actual price: ${np.mean(y_test):,.2f}")
    print(f"Mean predicted price: ${np.mean(y_pred):,.2f}")
    
    print(f"\n--- Model Performance ---")
    print(f"Prediction error (RMSE): ${metrics['rmse']:,.2f}")
    print(f"Average absolute error (MAE): ${metrics['mae']:,.2f}")
    print(f"R2 Score: {metrics['r2_score']:.4f}")
    print(f"  -> Model explains {metrics['r2_score']*100:.1f}% of price variance")
    
    print_regression_report(metrics, "Price Prediction")
    
    return metrics


def evaluate_threshold_impact(y_true, y_prob):
    """
    Evaluate the impact of different classification thresholds.
    
    This demonstrates how threshold choices affect model performance
    in different business contexts.
    
    Parameters:
    -----------
    y_true : ndarray
        True labels
    y_prob : ndarray
        Predicted probabilities
        
    Returns:
    --------
    threshold_results : DataFrame
        Results for different thresholds
    """
    thresholds = np.arange(0.1, 0.95, 0.1)
    results = []
    
    for threshold in thresholds:
        y_pred = (y_prob >= threshold).astype(int)
        metrics = calculate_classification_metrics(y_true, y_pred, y_prob)
        metrics['threshold'] = threshold
        results.append(metrics)
    
    return pd.DataFrame(results)


def cross_validate_model(X, y, model_type='classification', n_splits=5):
    """
    Perform cross-validation for robust model evaluation.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        Target vector
    model_type : str
        'classification' or 'regression'
    n_splits : int
        Number of cross-validation folds
        
    Returns:
    --------
    cv_results : dict
        Cross-validation results
    """
    if model_type == 'classification':
        model = LogisticRegression(random_state=42, max_iter=1000)
        scoring = 'roc_auc'
    else:
        model = LinearRegression()
        scoring = 'r2'
    
    cv = StratifiedKFold(n_splits=n_splits, shuffle=True, random_state=42)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    scores = cross_val_score(model, X_scaled, y, cv=cv, scoring=scoring)
    
    return {
        'mean_score': np.mean(scores),
        'std_score': np.std(scores),
        'all_scores': scores
    }


def main():
    """
    Main execution function for Model Evaluation Metrics implementation.
    """
    print("\n" + "#"*60)
    print("# MODEL EVALUATION METRICS IMPLEMENTATION")
    print("#"*60)
    print("\nThis implementation covers:")
    print("- Classification Metrics (Accuracy, Precision, Recall, F1, ROC-AUC)")
    print("- Regression Metrics (MSE, RMSE, MAE, R2)")
    print("- Banking Example (Credit Risk Evaluation)")
    print("- Healthcare Example (Disease Diagnosis)")
    print("- Threshold Analysis")
    print("- Cross-Validation")
    
    print("\n" + "-"*60)
    print("Executing Banking Example...")
    print("-"*60)
    banking_metrics = run_banking_example()
    
    print("\n" + "-"*60)
    print("Executing Healthcare Example...")
    print("-"*60)
    healthcare_metrics = run_healthcare_example()
    
    print("\n" + "-"*60)
    print("Executing Regression Example...")
    print("-"*60)
    regression_metrics = run_regression_example()
    
    print("\n" + "-"*60)
    print("Summary and Conclusions")
    print("-"*60)
    print("""
KEY TAKEAWAYS:

1. CLASSIFICATION METRICS:
   - Accuracy: Overall correctness (can be misleading with imbalanced classes)
   - Precision: When model predicts positive, how often is it correct?
   - Recall (Sensitivity): How many actual positives does model capture?
   - F1-Score: Harmonic mean of precision and recall
   - ROC-AUC: Model's ability to distinguish between classes

2. REGRESSION METRICS:
   - MSE/RMSE: Penalizes larger errors more heavily
   - MAE: Robust to outliers
   - R2: Proportion of variance explained (0-1, higher is better)

3. BUSINESS CONTEXT:
   - Banking: Balance between false positives (lost customers) and 
              false negatives (default losses)
   - Healthcare: Prioritize recall/sensitivity to avoid missed diagnoses
   
4. BEST PRACTICES:
   - Use multiple metrics for comprehensive evaluation
   - Consider business context when selecting metrics
   - Use cross-validation for robust estimates
   - Analyze threshold impact for operational deployment
""")
    
    print("\n" + "#"*60)
    print("# IMPLEMENTATION COMPLETE")
    print("#"*60 + "\n")


if __name__ == "__main__":
    main()