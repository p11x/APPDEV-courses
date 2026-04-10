# Topic: Logistic Regression
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Logistic Regression

I. INTRODUCTION
    Logistic Regression is a statistical method for analyzing datasets in which the outcome variable 
    is categorical (binary or multi-class). Despite its name, it is a classification algorithm 
    that uses the logistic (sigmoid) function to transform predictions into probability values 
    between 0 and 1. It is widely used in various domains including finance, healthcare, 
    marketing, and social sciences for predictive modeling and classification tasks.

II. CORE_CONCEPTS
    - Binary Classification: Predicting one of two possible outcomes (yes/no, default/no default)
    - Multi-class Classification: Using One-vs-Rest (OvR) strategy for >2 classes
    - Regularization: L1 (Lasso) and L2 (Ridge) to prevent overfitting via C parameter
    - Class Weights: Handling imbalanced datasets by penalizing misclassification
    - Probability Prediction: Outputting probability scores for each class
    - Threshold Tuning: Adjusting decision threshold for optimal performance
    - ROC Curve: Visualizing trade-off between true positive and false positive rates
    - Decision Boundaries: Linear decision surfaces separating classes

III. IMPLEMENTATION
    Comprehensive implementation covering various scenarios and use cases.

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Loan default prediction model
    - Healthcare: Disease diagnosis prediction

V. OUTPUT_RESULTS
    Detailed metrics and visualizations for model evaluation.

VI. TESTING
    Comprehensive testing across different scenarios.

VII. ADVANCED_TOPICS
    - Multiclass classification with different solvers
    - Feature engineering for logistic regression
    - Cross-validation strategies

VIII. CONCLUSION
    Summary and practical considerations.

"""

# Import necessary libraries
import numpy as np
import pandas as pd
from sklearn.datasets import make_classification, make_blobs
from sklearn.model_selection import train_test_split, cross_val_score, StratifiedKFold
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score, 
    roc_auc_score, confusion_matrix, classification_report,
    roc_curve, precision_recall_curve, average_precision_score
)
import warnings
warnings.filterwarnings('ignore')


def generate_classification_data(n_samples=500, n_features=10, n_informative=5, 
                                  n_clusters_per_class=2, random_state=42):
    """
    Generate synthetic classification data for demonstration.
    
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
        flip_y=0.05
    )
    
    feature_names = [f'Feature_{i+1}' for i in range(n_features)]
    
    print(f"Generated {n_samples} samples with {n_features} features")
    print(f"Class distribution: Class 0 = {np.sum(y==0)}, Class 1 = {np.sum(y==1)}")
    
    return X, y, feature_names


def generate_multiclass_data(n_samples=600, n_classes=3, n_features=8, random_state=42):
    """
    Generate synthetic multi-class classification data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_classes : int
        Number of classes
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
    """
    X, y = make_blobs(
        n_samples=n_samples,
        centers=n_classes,
        n_features=n_features,
        random_state=random_state,
        cluster_std=2.0
    )
    
    print(f"Generated {n_samples} samples with {n_classes} classes")
    return X, y


def core_logistic_regression():
    """
    Core implementation of logistic regression with various configurations.
    """
    print("\n" + "="*70)
    print("I. CORE LOGISTIC REGRESSION IMPLEMENTATION")
    print("="*70)
    
    X, y, feature_names = generate_classification_data(n_samples=500)
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n--- Binary Classification ---")
    
    model = LogisticRegression(random_state=42, max_iter=1000)
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    y_proba = model.predict_proba(X_test_scaled)[:, 1]
    
    print(f"\nModel Parameters:")
    print(f"  - Regularization (C): {model.C}")
    print(f"  - Penalty: {model.penalty}")
    print(f"  - Solver: {model.solver}")
    print(f"  - Max iterations: {model.max_iter}")
    
    print(f"\nPerformance Metrics (Default threshold = 0.5):")
    print(f"  - Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"  - Precision: {precision_score(y_test, y_pred):.4f}")
    print(f"  - Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"  - F1 Score: {f1_score(y_test, y_pred):.4f}")
    print(f"  - ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print("\nConfusion Matrix:")
    cm = confusion_matrix(y_test, y_pred)
    print(f"  TN: {cm[0,0]:3d}  FP: {cm[0,1]:3d}")
    print(f"  FN: {cm[1,0]:3d}  TP: {cm[1,1]:3d}")
    
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred))
    
    print("\n--- Regularization Comparison (C parameter) ---")
    
    for C_value in [0.001, 0.01, 0.1, 1.0, 10.0, 100.0]:
        model_reg = LogisticRegression(C=C_value, random_state=42, max_iter=1000)
        model_reg.fit(X_train_scaled, y_train)
        
        y_pred_reg = model_reg.predict(X_test_scaled)
        y_proba_reg = model_reg.predict_proba(X_test_scaled)[:, 1]
        
        print(f"C = {C_value:7.3f} | Accuracy: {accuracy_score(y_test, y_pred_reg):.4f} | "
              f"ROC-AUC: {roc_auc_score(y_test, y_proba_reg):.4f} | "
              f"Coef norm: {np.linalg.norm(model_reg.coef_):.4f}")
    
    print("\n--- Class Weight Handling ---")
    
    print("\nWithout class weights:")
    model_unweighted = LogisticRegression(random_state=42, max_iter=1000)
    model_unweighted.fit(X_train_scaled, y_train)
    print(f"  - Accuracy: {accuracy_score(y_test, model_unweighted.predict(X_test_scaled)):.4f}")
    
    print("\nWith balanced class weights:")
    model_weighted = LogisticRegression(
        class_weight='balanced', random_state=42, max_iter=1000
    )
    model_weighted.fit(X_train_scaled, y_train)
    print(f"  - Accuracy: {accuracy_score(y_test, model_weighted.predict(X_test_scaled)):.4f}")
    
    print("\nWith custom class weights (emphasize minority class):")
    model_custom = LogisticRegression(
        class_weight={0: 1, 1: 5}, random_state=42, max_iter=1000
    )
    model_custom.fit(X_train_scaled, y_train)
    print(f"  - Accuracy: {accuracy_score(y_test, model_custom.predict(X_test_scaled)):.4f}")
    
    return model, scaler


def threshold_tuning_example():
    """
    Demonstrate threshold tuning for optimal classification.
    """
    print("\n" + "="*70)
    print("II. THRESHOLD TUNING IMPLEMENTATION")
    print("="*70)
    
    X, y, _ = generate_classification_data(n_samples=500, random_state=42)
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42, max_iter=1000)
    model.fit(X_train_scaled, y_train)
    
    y_proba = model.predict_proba(X_test_scaled)[:, 1]
    
    fpr, tpr, thresholds = roc_curve(y_test, y_proba)
    
    precision_vals, recall_vals, pr_thresholds = precision_recall_curve(y_test, y_proba)
    
    print("\nROC Curve Analysis:")
    print(f"  - FPR range: [{fpr.min():.4f}, {fpr.max():.4f}]")
    print(f"  - TPR range: [{tpr.min():.4f}, {tpr.max():.4f}]")
    print(f"  - Number of thresholds: {len(thresholds)}")
    
    print("\nThreshold Optimization (F1 Score based):")
    
    best_f1 = 0
    best_threshold = 0.5
    
    for threshold in np.arange(0.1, 0.9, 0.05):
        y_pred_thresh = (y_proba >= threshold).astype(int)
        f1 = f1_score(y_test, y_pred_thresh)
        
        if f1 > best_f1:
            best_f1 = f1
            best_threshold = threshold
    
    print(f"  - Best threshold: {best_threshold:.2f}")
    print(f"  - Best F1 Score: {best_f1:.4f}")
    
    y_pred_optimal = (y_proba >= best_threshold).astype(int)
    y_pred_default = (y_proba >= 0.5).astype(int)
    
    print(f"\nComparison at different thresholds:")
    print(f"  Threshold = 0.50 (default):")
    print(f"    - Accuracy: {accuracy_score(y_test, y_pred_default):.4f}")
    print(f"    - F1: {f1_score(y_test, y_pred_default):.4f}")
    print(f"  Threshold = {best_threshold:.2f} (optimized):")
    print(f"    - Accuracy: {accuracy_score(y_test, y_pred_optimal):.4f}")
    print(f"    - F1: {f1_score(y_test, y_pred_optimal):.4f}")
    
    print("\nYouden's J statistic (optimal operating point):")
    j_scores = tpr - fpr
    optimal_idx = np.argmax(j_scores)
    optimal_threshold = thresholds[optimal_idx]
    print(f"  - Optimal threshold: {optimal_threshold:.4f}")
    print(f"  - TPR at threshold: {tpr[optimal_idx]:.4f}")
    print(f"  - FPR at threshold: {fpr[optimal_idx]:.4f}")


def multiclass_classification():
    """
    Multi-class logistic regression using One-vs-Rest strategy.
    """
    print("\n" + "="*70)
    print("III. MULTI-CLASS CLASSIFICATION (One-vs-Rest)")
    print("="*70)
    
    X, y = generate_multiclass_data(n_samples=600, n_classes=3, random_state=42)
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n--- Multi-class Classification ---")
    
    model_multi = LogisticRegression(
        multi_class='ovr',
        solver='lbfgs',
        max_iter=1000,
        random_state=42
    )
    model_multi.fit(X_train_scaled, y_train)
    
    y_pred = model_multi.predict(X_test_scaled)
    y_proba = model_multi.predict_proba(X_test_scaled)
    
    print(f"\nNumber of classes: {len(np.unique(y))}")
    print(f"Class distribution in test: {np.bincount(y_test)}")
    
    print(f"\nPerformance Metrics:")
    print(f"  - Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    for i, class_name in enumerate(['Class 0', 'Class 1', 'Class 2']):
        y_test_binary = (y_test == i).astype(int)
        y_pred_binary = (y_pred == i).astype(int)
        print(f"  - {class_name} Precision: {precision_score(y_test_binary, y_pred_binary):.4f}")
        print(f"  - {class_name} Recall: {recall_score(y_test_binary, y_pred_binary):.4f}")
    
    print("\nConfusion Matrix:")
    cm = confusion_matrix(y_test, y_pred)
    print(cm)
    
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred))
    
    print("\nProbability Predictions (first 5 test samples):")
    print("    Class0    Class1    Class2    Predicted")
    for i in range(5):
        probs = y_proba[i]
        pred = y_pred[i]
        print(f"  {i}: {probs[0]:.4f}   {probs[1]:.4f}   {probs[2]:.4f}       {pred}")
    
    print("\n--- Different Solvers Comparison ---")
    
    solvers = ['lbfgs', 'newton-cg', 'sag', 'saga']
    for solver in solvers:
        try:
            model = LogisticRegression(
                multi_class='ovr',
                solver=solver,
                max_iter=1000,
                random_state=42
            )
            model.fit(X_train_scaled, y_train)
            y_pred = model.predict(X_test_scaled)
            print(f"  Solver: {solver:12s} | Accuracy: {accuracy_score(y_test, y_pred):.4f}")
        except Exception as e:
            print(f"  Solver: {solver:12s} | Error: {str(e)[:30]}")


def banking_example():
    """
    Banking/Finance example: Loan Default Prediction.
    """
    print("\n" + "="*70)
    print("IV. BANKING EXAMPLE: LOAN DEFAULT PREDICTION")
    print("="*70)
    
    np.random.seed(42)
    n_samples = 1000
    
    data = {
        'credit_score': np.random.normal(650, 80, n_samples).clip(300, 850),
        'income': np.random.exponential(50000, n_samples),
        'debt_to_income': np.random.uniform(0.1, 0.5, n_samples),
        'employment_years': np.random.exponential(5, n_samples),
        'loan_amount': np.random.uniform(5000, 100000, n_samples),
        'existing_loans': np.random.poisson(2, n_samples),
        'payment_history': np.random.uniform(0.5, 1.0, n_samples),
    }
    
    df = pd.DataFrame(data)
    
    default_prob = (
        0.3 * (df['credit_score'] < 600).astype(int) +
        0.25 * (df['debt_to_income'] > 0.4).astype(int) +
        0.2 * (df['payment_history'] < 0.7).astype(int) +
        0.15 * (df['employment_years'] < 1).astype(int) +
        0.1 * np.random.random(n_samples)
    )
    
    df['default'] = (default_prob > 0.35).astype(int)
    
    print(f"\nDataset: {len(df)} loan applications")
    print(f"Features: {list(df.columns[:-1])}")
    print(f"Default rate: {df['default'].mean()*100:.2f}%")
    
    feature_cols = ['credit_score', 'income', 'debt_to_income', 
                   'employment_years', 'loan_amount', 'existing_loans', 'payment_history']
    
    X = df[feature_cols].values
    y = df['default'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.25, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n--- Model Training ---")
    
    model = LogisticRegression(
        class_weight='balanced',
        random_state=42,
        max_iter=1000,
        solver='lbfgs'
    )
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    y_proba = model.predict_proba(X_test_scaled)[:, 1]
    
    print(f"\nModel Performance:")
    print(f"  - Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"  - Precision (default): {precision_score(y_test, y_pred):.4f}")
    print(f"  - Recall (default): {recall_score(y_test, y_pred):.4f}")
    print(f"  - F1 Score: {f1_score(y_test, y_pred):.4f}")
    print(f"  - ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print("\nConfusion Matrix:")
    cm = confusion_matrix(y_test, y_pred)
    print(f"                   Predicted")
    print(f"                No Default  Default")
    print(f"Actual No Default  {cm[0,0]:6d}    {cm[0,1]:6d}")
    print(f"       Default    {cm[1,0]:6d}    {cm[1,1]:6d}")
    
    print("\nFeature Coefficients (Risk Factors):")
    for feature, coef in sorted(zip(feature_cols, model.coef_[0]), 
                                  key=lambda x: abs(x[1]), reverse=True):
        direction = "increases" if coef > 0 else "decreases"
        print(f"  - {feature:20s}: {coef:+.4f} ({direction} default risk)")
    
    print("\n--- Risk Assessment Examples ---")
    
    test_cases = [
        [750, 80000, 0.2, 10, 20000, 1, 0.95],
        [550, 30000, 0.45, 1, 50000, 3, 0.6],
        [700, 60000, 0.3, 5, 35000, 2, 0.85],
    ]
    
    for i, case in enumerate(test_cases):
        case_scaled = scaler.transform([case])
        prob = model.predict_proba(case_scaled)[0, 1]
        prediction = "DEFAULT" if prob > 0.5 else "NO DEFAULT"
        
        print(f"\n  Case {i+1}: ")
        print(f"    Credit Score: {case[0]}, Income: ${case[1]:,.0f}")
        print(f"    DTI: {case[2]:.0%}, Employment: {case[3]:.0f} years")
        print(f"    Default Probability: {prob:.2%}")
        print(f"    Prediction: {prediction}")


def healthcare_example():
    """
    Healthcare example: Disease Diagnosis Prediction.
    """
    print("\n" + "="*70)
    print("V. HEALTHCARE EXAMPLE: DISEASE DIAGNOSIS")
    print("="*70)
    
    np.random.seed(42)
    n_samples = 800
    
    data = {
        'age': np.random.normal(45, 15, n_samples).clip(18, 90),
        'bmi': np.random.normal(27, 5, n_samples).clip(15, 45),
        'blood_pressure_sys': np.random.normal(130, 20, n_samples).clip(90, 200),
        'blood_pressure_dia': np.random.normal(85, 12, n_samples).clip(60, 130),
        'cholesterol': np.random.normal(200, 40, n_samples).clip(100, 300),
        'glucose': np.random.normal(100, 25, n_samples).clip(70, 200),
        'smoker': np.random.binomial(1, 0.25, n_samples),
        'exercise_level': np.random.uniform(0, 10, n_samples),
        'family_history': np.random.binomial(1, 0.15, n_samples),
    }
    
    df = pd.DataFrame(data)
    
    risk_score = (
        0.20 * (df['age'] > 55).astype(int) +
        0.15 * (df['bmi'] > 30).astype(int) +
        0.15 * (df['blood_pressure_sys'] > 140).astype(int) +
        0.15 * (df['cholesterol'] > 220).astype(int) +
        0.15 * (df['glucose'] > 120).astype(int) +
        0.10 * df['smoker'] +
        0.10 * (df['exercise_level'] < 3).astype(int) +
        0.10 * df['family_history'] +
        0.05 * np.random.random(n_samples)
    )
    
    df['disease'] = (risk_score > 0.35).astype(int)
    
    print(f"\nDataset: {n_samples} patient records")
    print(f"Features: {list(df.columns[:-1])}")
    print(f"Disease prevalence: {df['disease'].mean()*100:.2f}%")
    
    feature_cols = ['age', 'bmi', 'blood_pressure_sys', 'blood_pressure_dia',
                   'cholesterol', 'glucose', 'smoker', 'exercise_level', 'family_history']
    
    X = df[feature_cols].values
    y = df['disease'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n--- Model Training ---")
    
    model = LogisticRegression(
        class_weight='balanced',
        random_state=42,
        max_iter=1000,
        solver='lbfgs'
    )
    model.fit(X_train_scaled, y_train)
    
    y_pred = model.predict(X_test_scaled)
    y_proba = model.predict_proba(X_test_scaled)[:, 1]
    
    print(f"\nModel Performance:")
    print(f"  - Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"  - Precision (disease): {precision_score(y_test, y_pred):.4f}")
    print(f"  - Recall (disease): {recall_score(y_test, y_pred):.4f}")
    print(f"  - F1 Score: {f1_score(y_test, y_pred):.4f}")
    print(f"  - ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred, target_names=['No Disease', 'Disease']))
    
    print("\nFeature Coefficients (Risk Factors):")
    for feature, coef in sorted(zip(feature_cols, model.coef_[0]), 
                                  key=lambda x: abs(x[1]), reverse=True):
        direction = "increases" if coef > 0 else "decreases"
        print(f"  - {feature:25s}: {coef:+.4f} ({direction} disease risk)")
    
    print("\n--- Patient Risk Assessment ---")
    
    sample_patients = [
        [35, 24, 120, 80, 180, 95, 0, 7, 0],
        [65, 32, 155, 95, 250, 130, 1, 1, 1],
        [50, 28, 135, 88, 210, 105, 0, 5, 1],
    ]
    
    for i, patient in enumerate(sample_patients):
        patient_scaled = scaler.transform([patient])
        prob = model.predict_proba(patient_scaled)[0, 1]
        risk_level = "HIGH" if prob > 0.6 else "MEDIUM" if prob > 0.3 else "LOW"
        
        print(f"\n  Patient {i+1}:")
        print(f"    Age: {patient[0]}, BMI: {patient[1]}, BP: {patient[2]}/{patient[3]}")
        print(f"    Smoker: {'Yes' if patient[6] else 'No'}, Exercise: {patient[7]:.0f}/10")
        print(f"    Disease Probability: {prob:.2%}")
        print(f"    Risk Level: {risk_level}")


def decision_boundary_demo():
    """
    Visualize decision boundaries for logistic regression.
    """
    print("\n" + "="*70)
    print("VI. DECISION BOUNDARY ANALYSIS")
    print("="*70)
    
    X, y = make_blobs(n_samples=400, centers=2, n_features=2, 
                    random_state=42, cluster_std=3)
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LogisticRegression(random_state=42, max_iter=1000)
    model.fit(X_train_scaled, y_train)
    
    print("\n--- 2D Decision Boundary ---")
    print(f"Feature 1 range: [{X[:, 0].min():.2f}, {X[:, 0].max():.2f}]")
    print(f"Feature 2 range: [{X[:, 1].min():.2f}, {X[:, 1].max():.2f}]")
    
    coef = model.coef_[0]
    intercept = model.intercept_[0]
    
    print(f"\nDecision Boundary Equation:")
    print(f"  {coef[0]:.4f} * Feature1 + {coef[1]:.4f} * Feature2 + {intercept:.4f} = 0")
    
    x1_range = np.linspace(X[:, 0].min() - 1, X[:, 0].max() + 1, 100)
    x2_boundary = (-intercept - coef[0] * x1_range) / coef[1]
    
    print(f"\nBoundary intercepts:")
    print(f"  Feature1 = {-intercept/coef[0]:.4f} when Feature2 = 0")
    print(f"  Feature2 = {-intercept/coef[1]:.4f} when Feature1 = 0")
    
    y_pred = model.predict(X_test_scaled)
    accuracy = accuracy_score(y_test, y_pred)
    print(f"\nModel Accuracy: {accuracy:.4f}")
    
    print("\n--- Regularization Effect on Decision Boundary ---")
    
    for C_val in [0.01, 0.1, 1.0, 10.0]:
        model_reg = LogisticRegression(C=C_val, random_state=42, max_iter=1000)
        model_reg.fit(X_train_scaled, y_train)
        coef_norm = np.linalg.norm(model_reg.coef_)
        print(f"  C = {C_val:5.2f} | Coefficient norm: {coef_norm:.4f}")


def cross_validation_demo():
    """
    Demonstrate cross-validation for logistic regression.
    """
    print("\n" + "="*70)
    print("VII. CROSS-VALIDATION ANALYSIS")
    print("="*70)
    
    X, y, _ = generate_classification_data(n_samples=500, random_state=42)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    model = LogisticRegression(random_state=42, max_iter=1000)
    
    cv = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
    
    accuracy_scores = cross_val_score(model, X_scaled, y, cv=cv, scoring='accuracy')
    f1_scores = cross_val_score(model, X_scaled, y, cv=cv, scoring='f1')
    roc_auc_scores = cross_val_score(model, X_scaled, y, cv=cv, scoring='roc_auc')
    
    print("\n5-Fold Cross-Validation Results:")
    print(f"\nAccuracy: {accuracy_scores.mean():.4f} (+/- {accuracy_scores.std()*2:.4f})")
    print(f"  Fold scores: {[f'{s:.4f}' for s in accuracy_scores]}")
    
    print(f"\nF1 Score: {f1_scores.mean():.4f} (+/- {f1_scores.std()*2:.4f})")
    print(f"  Fold scores: {[f'{s:.4f}' for s in f1_scores]}")
    
    print(f"\nROC-AUC: {roc_auc_scores.mean():.4f} (+/- {roc_auc_scores.std()*2:.4f})")
    print(f"  Fold scores: {[f'{s:.4f}' for s in roc_auc_scores]}")
    
    print("\n--- Hyperparameter Tuning with CV ---")
    
    C_values = [0.001, 0.01, 0.1, 1.0, 10.0, 100.0]
    
    print("\nC Value | Mean Accuracy | Std Accuracy")
    print("-" * 45)
    
    best_C = 1.0
    best_score = 0
    
    for C_val in C_values:
        model = LogisticRegression(C=C_val, random_state=42, max_iter=1000)
        scores = cross_val_score(model, X_scaled, y, cv=cv, scoring='accuracy')
        mean_score = scores.mean()
        
        print(f"{C_val:7.3f} | {mean_score:.4f}     | {scores.std()*2:.4f}")
        
        if mean_score > best_score:
            best_score = mean_score
            best_C = C_val
    
    print(f"\nBest C value: {best_C} (Accuracy: {best_score:.4f})")


def main():
    """
    Main function to execute all logistic regression demonstrations.
    """
    print("="*70)
    print("LOGISTIC REGRESSION COMPREHENSIVE IMPLEMENTATION")
    print("="*70)
    print(f"Topic: Logistic Regression")
    print(f"Author: AI Assistant")
    print(f"Date: 06-04-2026")
    
    print("\n" + "="*70)
    print("EXECUTING LOGISTIC REGRESSION IMPLEMENTATION")
    print("="*70)
    
    try:
        core_logistic_regression()
    except Exception as e:
        print(f"Error in core implementation: {e}")
    
    try:
        threshold_tuning_example()
    except Exception as e:
        print(f"Error in threshold tuning: {e}")
    
    try:
        multiclass_classification()
    except Exception as e:
        print(f"Error in multiclass classification: {e}")
    
    try:
        banking_example()
    except Exception as e:
        print(f"Error in banking example: {e}")
    
    try:
        healthcare_example()
    except Exception as e:
        print(f"Error in healthcare example: {e}")
    
    try:
        decision_boundary_demo()
    except Exception as e:
        print(f"Error in decision boundary: {e}")
    
    try:
        cross_validation_demo()
    except Exception as e:
        print(f"Error in cross-validation: {e}")
    
    print("\n" + "="*70)
    print("IMPLEMENTATION COMPLETE")
    print("="*70)
    
    print("\nVIII. CONCLUSION")
    print("-" * 70)
    print("""
Logistic Regression Summary:

1. STRENGTHS:
   - Interpretable model with probability outputs
   - Efficient training and prediction
   - Works well with linearly separable data
   - Provides feature importance through coefficients
   - Handles both binary and multi-class classification

2. LIMITATIONS:
   - Assumes linear decision boundaries
   - Sensitive to outliers
   - May underfit complex relationships
   - Requires feature scaling

3. PRACTICAL CONSIDERATIONS:
   - Always scale features for stable training
   - Use class weights for imbalanced data
   - Tune regularization parameter (C)
   - Adjust threshold based on business requirements
   - Consider feature engineering for better performance

4. USE CASES:
   - Credit risk assessment
   - Disease diagnosis prediction
   - Customer churn prediction
   - Marketing campaign response
   - Fraud detection

5. NEXT STEPS:
   - Explore regularization variants (L1, Elastic Net)
   - Consider ensemble methods
   - Add feature interactions
   - Implement advanced preprocessing
""")


if __name__ == "__main__":
    main()