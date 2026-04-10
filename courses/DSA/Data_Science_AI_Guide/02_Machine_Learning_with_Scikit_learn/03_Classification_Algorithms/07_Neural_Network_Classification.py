# Topic: Neural Network Classification
# Author: AI Assistant
# Date: 06-04-2026

"""
================================================================================
COMPREHENSIVE IMPLEMENTATION FOR NEURAL NETWORK CLASSIFICATION
================================================================================

I. INTRODUCTION
   - Overview of Neural Networks in Machine Learning
   - Historical Context and Evolution
   - Neural Networks vs Traditional ML Algorithms
   - When to Use Neural Networks
   - Advantages and Limitations

II. CORE CONCEPTS
   - Perceptron and Multi-Layer Perceptron (MLP)
   - Network Architecture (Input, Hidden, Output Layers)
   - Activation Functions (ReLU, Tanh, Sigmoid, Softmax)
   - Forward Propagation
   - Backpropagation Algorithm
   - Loss Functions (Cross-Entropy, Binary Cross-Entropy)
   - Optimization (Gradient Descent, Adam, SGD)
   - Hyperparameters (learning_rate, alpha, hidden_layers)

III. IMPLEMENTATION BASICS
   - MLPClassifier from Scikit-learn
   - Data Preprocessing for Neural Networks
   - Feature Scaling (StandardScaler)
   - Model Training and Prediction
   - Evaluation Metrics

IV. BANKING EXAMPLE: CREDIT CARD FRAUD DETECTION
   - Problem Definition
   - Dataset Generation
   - Feature Engineering
   - Model Training
   - Performance Evaluation
   - Interpretation of Results

V. HEALTHCARE EXAMPLE: DISEASE DIAGNOSIS
   - Problem Definition
   - Dataset Generation
   - Feature Engineering
   - Model Training
   - Performance Evaluation
   - Medical Interpretation

VI. ARCHITECTURE TUNING
   - Comparing Different Architectures
   - Single vs Multiple Hidden Layers
   - Layer Size Optimization
   - Activation Function Comparison
   - Solver Comparison (Adam, SGD, L-BFGS)
   - Regularization (alpha parameter)
   - Learning Rate Tuning
   - Early Stopping

VII. ADVANCED TOPICS
   - Dropout Regularization (conceptual)
   - Batch Normalization (conceptual)
   - Learning Rate Schedules
   - Momentum Optimization
   - Vanishing/Exploding Gradients
   - Hyperparameter Optimization Strategies

VIII. CONCLUSION
   - Key Takeaways
   - Best Practices
   - Common Pitfalls to Avoid
   - Future Directions

================================================================================
"""

# Import necessary libraries
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_classification, make_blobs, make_moons
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.preprocessing import StandardScaler, MinMaxScaler
from sklearn.neural_network import MLPClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score, 
    roc_auc_score, confusion_matrix, classification_report,
    roc_curve, precision_recall_curve
)
import warnings
warnings.filterwarnings('ignore')

# Set random seed for reproducibility
np.random.seed(42)


# ================================================================================
# SECTION I: INTRODUCTION AND UTILITY FUNCTIONS
# ================================================================================

def print_section_header(title):
    """Print a formatted section header"""
    print("\n" + "=" * 80)
    print(f"  {title}")
    print("=" * 80)


def print_subsection(title):
    """Print a formatted subsection header"""
    print(f"\n--- {title} ---")


def print_subsubsection(title):
    """Print a formatted sub-subsection header"""
    print(f"  > {title}")


def calculate_metrics(y_true, y_pred, y_prob=None):
    """
    Calculate comprehensive evaluation metrics for classification.
    
    Parameters:
    -----------
    y_true : array-like
        True labels
    y_pred : array-like
        Predicted labels
    y_prob : array-like, optional
        Predicted probabilities for positive class
    
    Returns:
    --------
    dict : Dictionary containing all evaluation metrics
    """
    metrics = {
        'accuracy': accuracy_score(y_true, y_pred),
        'precision': precision_score(y_true, y_pred, zero_division=0),
        'recall': recall_score(y_true, y_pred, zero_division=0),
        'f1_score': f1_score(y_true, y_pred, zero_division=0)
    }
    
    if y_prob is not None:
        try:
            metrics['roc_auc'] = roc_auc_score(y_true, y_prob)
        except:
            metrics['roc_auc'] = None
    
    return metrics


def print_metrics(metrics, indent=2):
    """Print metrics in a formatted way"""
    indent_str = " " * indent
    for metric_name, value in metrics.items():
        if value is not None:
            print(f"{indent_str}{metric_name}: {value:.4f}")
        else:
            print(f"{indent_str}{metric_name}: N/A")


# ================================================================================
# SECTION II: DATA GENERATION FUNCTIONS
# ================================================================================

def generate_synthetic_classification_data(n_samples=500, n_features=10, n_informative=5, 
                                          n_redundant=3, n_classes=2, class_sep=1.0):
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
    n_redundant : int
        Number of redundant features
    n_classes : int
        Number of classes
    class_sep : float
        Separation between classes
    
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Labels
    feature_names : list
        Names of features
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_classes=n_classes,
        n_clusters_per_class=2,
        class_sep=class_sep,
        random_state=42
    )
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    
    return X, y, feature_names


def generate_complex_nonlinear_data(n_samples=500):
    """
    Generate complex nonlinear data using make_moons and make_blobs.
    
    This creates a more challenging dataset that requires 
    nonlinear decision boundaries.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples
    
    Returns:
    --------
    X, y : arrays
        Features and labels
    """
    n_each = n_samples // 2
    
    X1, y1 = make_moons(n_samples=n_each, noise=0.15, random_state=42)
    X2, y2 = make_moons(n_samples=n_each, noise=0.15, random_state=123)
    y2 = 1 - y2
    
    X = np.vstack([X1, X2])
    y = np.hstack([y1, y2])
    
    shuffle_idx = np.random.permutation(len(y))
    X, y = X[shuffle_idx], y[shuffle_idx]
    
    return X, y


def generate_banking_fraud_data(n_samples=1000):
    """
    Generate synthetic banking data for fraud detection.
    
    Features include:
    - transaction_amount
    - transaction_frequency
    - time_since_last_transaction
    - average_transaction_amount
    - number_of_transactions_today
    - is_international
    - is_high_risk_merchant
    - time_of_day
    - day_of_week
    - account_age_days
    - balance_ratio
    - previous_fraudulent_transactions
    
    Parameters:
    -----------
    n_samples : int
        Number of transactions to generate
    
    Returns:
    --------
    X : array-like, shape (n_samples, n_features)
        Feature matrix
    y : array-like, shape (n_samples,)
        Labels (0 = legitimate, 1 = fraud)
    feature_names : list
        Names of features
    """
    np.random.seed(42)
    
    n_fraud = int(n_samples * 0.05)
    n_legitimate = n_samples - n_fraud
    
    transaction_amount = np.concatenate([
        np.random.exponential(100, n_legitimate),
        np.random.exponential(500, n_fraud)
    ])
    
    transaction_frequency = np.concatenate([
        np.random.exponential(5, n_legitimate),
        np.random.exponential(50, n_fraud)
    ])
    
    time_since_last = np.concatenate([
        np.random.exponential(60, n_legitimate),
        np.random.exponential(5, n_fraud)
    ])
    
    avg_amount = np.concatenate([
        np.random.exponential(150, n_legitimate),
        np.random.exponential(300, n_fraud)
    ])
    
    num_transactions_today = np.concatenate([
        np.random.poisson(3, n_legitimate),
        np.random.poisson(20, n_fraud)
    ])
    
    is_international = np.concatenate([
        np.random.choice([0, 1], n_legitimate, p=[0.9, 0.1]),
        np.random.choice([0, 1], n_fraud, p=[0.3, 0.7])
    ])
    
    is_high_risk_merchant = np.concatenate([
        np.random.choice([0, 1], n_legitimate, p=[0.95, 0.05]),
        np.random.choice([0, 1], n_fraud, p=[0.4, 0.6])
    ])
    
    time_of_day = np.concatenate([
        np.random.uniform(8, 22, n_legitimate),
        np.random.uniform(0, 24, n_fraud)
    ])
    
    day_of_week = np.random.randint(0, 7, n_samples)
    
    account_age = np.concatenate([
        np.random.exponential(365, n_legitimate),
        np.random.exponential(30, n_fraud)
    ])
    
    balance_ratio = np.concatenate([
        np.random.uniform(0.1, 2.0, n_legitimate),
        np.random.uniform(0.01, 0.2, n_fraud)
    ])
    
    prev_fraud = np.concatenate([
        np.zeros(n_legitimate),
        np.random.randint(1, 5, n_fraud)
    ])
    
    X = np.column_stack([
        transaction_amount,
        transaction_frequency,
        time_since_last,
        avg_amount,
        num_transactions_today,
        is_international,
        is_high_risk_merchant,
        time_of_day,
        day_of_week,
        account_age,
        balance_ratio,
        prev_fraud
    ])
    
    y = np.concatenate([np.zeros(n_legitimate), np.ones(n_fraud)])
    
    shuffle_idx = np.random.permutation(len(y))
    X = X[shuffle_idx]
    y = y[shuffle_idx]
    
    feature_names = [
        'transaction_amount', 'transaction_frequency', 'time_since_last_transaction',
        'average_transaction_amount', 'num_transactions_today', 'is_international',
        'is_high_risk_merchant', 'time_of_day', 'day_of_week', 
        'account_age_days', 'balance_ratio', 'prev_fraudulent_transactions'
    ]
    
    return X, y, feature_names


def generate_healthcare_disease_data(n_samples=1000):
    """
    Generate synthetic healthcare data for disease diagnosis.
    
    Features include:
    - age
    - blood_pressure_systolic
    - blood_pressure_diastolic
    - heart_rate
    - body_temperature
    - oxygen_saturation
    - white_blood_cell_count
    - red_blood_cell_count
    - glucose_level
    - cholesterol_level
    - bmi
    - smoking_status
    - exercise_frequency
    - family_history
    
    Parameters:
    -----------
    n_samples : int
        Number of patients to generate
    
    Returns:
    --------
    X : array-like, shape (n_samples, n_features)
        Feature matrix
    y : array-like, shape (n_samples,)
        Labels (0 = healthy, 1 = disease)
    feature_names : list
        Names of features
    """
    np.random.seed(42)
    
    n_disease = int(n_samples * 0.25)
    n_healthy = n_samples - n_disease
    
    age = np.concatenate([
        np.random.randint(20, 80, n_healthy),
        np.random.randint(40, 90, n_disease)
    ])
    
    bp_systolic = np.concatenate([
        np.random.normal(120, 15, n_healthy),
        np.random.normal(160, 25, n_disease)
    ])
    
    bp_diastolic = np.concatenate([
        np.random.normal(80, 10, n_healthy),
        np.random.normal(100, 15, n_disease)
    ])
    
    heart_rate = np.concatenate([
        np.random.normal(72, 10, n_healthy),
        np.random.normal(95, 20, n_disease)
    ])
    
    body_temp = np.concatenate([
        np.random.normal(36.6, 0.5, n_healthy),
        np.random.normal(37.5, 1.0, n_disease)
    ])
    
    oxygen_sat = np.concatenate([
        np.random.normal(98, 2, n_healthy),
        np.random.normal(92, 5, n_disease)
    ])
    
    wbc_count = np.concatenate([
        np.random.normal(7000, 1500, n_healthy),
        np.random.normal(12000, 3000, n_disease)
    ])
    
    rbc_count = np.concatenate([
        np.random.normal(5, 0.5, n_healthy),
        np.random.normal(4, 0.8, n_disease)
    ])
    
    glucose = np.concatenate([
        np.random.normal(90, 15, n_healthy),
        np.random.normal(160, 40, n_disease)
    ])
    
    cholesterol = np.concatenate([
        np.random.normal(190, 30, n_healthy),
        np.random.normal(250, 50, n_disease)
    ])
    
    bmi = np.concatenate([
        np.random.normal(24, 4, n_healthy),
        np.random.normal(30, 6, n_disease)
    ])
    
    smoking = np.concatenate([
        np.random.choice([0, 1], n_healthy, p=[0.8, 0.2]),
        np.random.choice([0, 1], n_disease, p=[0.4, 0.6])
    ])
    
    exercise = np.concatenate([
        np.random.choice([0, 1, 2], n_healthy, p=[0.2, 0.3, 0.5]),
        np.random.choice([0, 1, 2], n_disease, p=[0.6, 0.3, 0.1])
    ])
    
    family_history = np.concatenate([
        np.random.choice([0, 1], n_healthy, p=[0.85, 0.15]),
        np.random.choice([0, 1], n_disease, p=[0.5, 0.5])
    ])
    
    X = np.column_stack([
        age, bp_systolic, bp_diastolic, heart_rate, body_temp,
        oxygen_sat, wbc_count, rbc_count, glucose, cholesterol,
        bmi, smoking, exercise, family_history
    ])
    
    y = np.concatenate([np.zeros(n_healthy), np.ones(n_disease)])
    
    shuffle_idx = np.random.permutation(len(y))
    X = X[shuffle_idx]
    y = y[shuffle_idx]
    
    feature_names = [
        'age', 'blood_pressure_systolic', 'blood_pressure_diastolic',
        'heart_rate', 'body_temperature', 'oxygen_saturation',
        'white_blood_cell_count', 'red_blood_cell_count',
        'glucose_level', 'cholesterol_level', 'bmi',
        'smoking_status', 'exercise_frequency', 'family_history'
    ]
    
    return X, y, feature_names


# ================================================================================
# SECTION III: CORE NEURAL NETWORK IMPLEMENTATION
# ================================================================================

def create_mlp_classifier(hidden_layer_sizes=(100,), activation='relu', 
                          solver='adam', alpha=0.0001, learning_rate='constant',
                          learning_rate_init=0.001, max_iter=200, early_stopping=True,
                          validation_fraction=0.1, n_iter_no_change=10, random_state=42):
    """
    Create and configure an MLPClassifier.
    
    Parameters:
    -----------
    hidden_layer_sizes : tuple
        The number of neurons in each hidden layer
    activation : str
        Activation function ('relu', 'tanh', 'logistic', 'identity')
    solver : str
        The solver for weight optimization ('adam', 'sgd', 'lbfgs')
    alpha : float
        L2 regularization parameter
    learning_rate : str
        Learning rate schedule ('constant', 'invscaling', 'adaptive')
    learning_rate_init : float
        The initial learning rate
    max_iter : int
        Maximum number of iterations
    early_stopping : bool
        Whether to use early stopping
    validation_fraction : float
        Fraction of training data for validation
    n_iter_no_change : int
        Number of iterations with no improvement before stopping
    random_state : int
        Random seed for reproducibility
    
    Returns:
    --------
    MLPClassifier : Configured MLPClassifier instance
    """
    model = MLPClassifier(
        hidden_layer_sizes=hidden_layer_sizes,
        activation=activation,
        solver=solver,
        alpha=alpha,
        learning_rate=learning_rate,
        learning_rate_init=learning_rate_init,
        max_iter=max_iter,
        early_stopping=early_stopping,
        validation_fraction=validation_fraction,
        n_iter_no_change=n_iter_no_change,
        random_state=random_state,
        verbose=False
    )
    
    return model


def train_neural_network(X_train, y_train, X_test, y_test, **model_params):
    """
    Train a neural network classifier and evaluate its performance.
    
    Parameters:
    -----------
    X_train : array-like
        Training features
    y_train : array-like
        Training labels
    X_test : array-like
        Test features
    y_test : array-like
        Test labels
    **model_params : keyword arguments
        Parameters for MLPClassifier
    
    Returns:
    --------
    model : MLPClassifier
        Trained model
    metrics : dict
        Dictionary containing evaluation metrics
    y_pred : array-like
        Predictions on test set
    y_prob : array-like
        Predicted probabilities
    """
    model = create_mlp_classifier(**model_params)
    
    model.fit(X_train, y_train)
    
    y_pred = model.predict(X_test)
    y_prob = model.predict_proba(X_test)[:, 1]
    
    metrics = calculate_metrics(y_test, y_pred, y_prob)
    
    return model, metrics, y_pred, y_prob


def demonstrate_basic_mlp():
    """
    Demonstrate basic MLPClassifier usage.
    """
    print_section_header("BASIC MLP CLASSIFIER DEMONSTRATION")
    
    print("\n1. Generating synthetic classification data...")
    X, y, feature_names = generate_synthetic_classification_data(
        n_samples=500, n_features=10, n_informative=5, n_redundant=3
    )
    print(f"   Data shape: {X.shape}")
    print(f"   Class distribution: {np.bincount(y.astype(int))}")
    
    print("\n2. Splitting data into train and test sets...")
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    print(f"   Training set: {X_train.shape[0]} samples")
    print(f"   Test set: {X_test.shape[0]} samples")
    
    print("\n3. Scaling features using StandardScaler...")
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n4. Training MLPClassifier...")
    model = create_mlp_classifier(
        hidden_layer_sizes=(50, 25),
        activation='relu',
        solver='adam',
        alpha=0.001,
        learning_rate_init=0.001,
        max_iter=500,
        early_stopping=True,
        random_state=42
    )
    
    model.fit(X_train_scaled, y_train)
    print(f"   Training completed in {model.n_iter_} iterations")
    print(f"   Best validation score: {model.best_validation_score_:.4f}")
    
    print("\n5. Evaluating model performance...")
    y_pred = model.predict(X_test_scaled)
    y_prob = model.predict_proba(X_test_scaled)[:, 1]
    
    metrics = calculate_metrics(y_test, y_pred, y_prob)
    print_metrics(metrics)
    
    print("\n6. Showing model architecture...")
    print(f"   Hidden layers: {model.hidden_layer_sizes}")
    print(f"   Activation: {model.activation}")
    print(f"   Solver: {model.solver}")
    print(f"   Number of iterations: {model.n_iter_}")
    
    return model, metrics, scaler


# ================================================================================
# SECTION IV: ARCHITECTURE COMPARISON
# ================================================================================

def compare_architectures(X_train, y_train, X_test, y_test):
    """
    Compare different neural network architectures.
    """
    print_section_header("ARCHITECTURE COMPARISON")
    
    architectures = [
        {"name": "Single Layer (10 neurons)", "hidden_layer_sizes": (10,)},
        {"name": "Single Layer (50 neurons)", "hidden_layer_sizes": (50,)},
        {"name": "Two Layers (50, 25)", "hidden_layer_sizes": (50, 25)},
        {"name": "Two Layers (100, 50)", "hidden_layer_sizes": (100, 50)},
        {"name": "Three Layers (64, 32, 16)", "hidden_layer_sizes": (64, 32, 16)},
        {"name": "Wide (100, 100)", "hidden_layer_sizes": (100, 100)},
    ]
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    results = []
    
    print("\nComparing different architectures:")
    print("-" * 60)
    
    for arch in architectures:
        model = create_mlp_classifier(
            hidden_layer_sizes=arch["hidden_layer_sizes"],
            max_iter=300,
            random_state=42
        )
        
        model.fit(X_train_scaled, y_train)
        y_pred = model.predict(X_test_scaled)
        y_prob = model.predict_proba(X_test_scaled)[:, 1]
        
        metrics = calculate_metrics(y_test, y_pred, y_prob)
        
        results.append({
            'name': arch['name'],
            'hidden_layer_sizes': arch['hidden_layer_sizes'],
            'metrics': metrics,
            'iterations': model.n_iter_
        })
        
        print(f"\n{arch['name']}:")
        print(f"   Accuracy: {metrics['accuracy']:.4f}")
        print(f"   ROC-AUC: {metrics['roc_auc']:.4f}")
        print(f"   Iterations: {model.n_iter_}")
    
    print("\n" + "-" * 60)
    print("\nArchitecture Comparison Summary:")
    for result in results:
        print(f"  {result['name']}: Acc={result['metrics']['accuracy']:.4f}, "
              f"AUC={result['metrics']['roc_auc']:.4f}")
    
    return results


def compare_activation_functions(X_train, y_train, X_test, y_test):
    """
    Compare different activation functions.
    """
    print_section_header("ACTIVATION FUNCTION COMPARISON")
    
    activations = ['relu', 'tanh', 'logistic']
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\nComparing activation functions (with hidden_layer_sizes=(50, 25)):")
    print("-" * 60)
    
    results = []
    
    for activation in activations:
        model = create_mlp_classifier(
            hidden_layer_sizes=(50, 25),
            activation=activation,
            max_iter=300,
            random_state=42
        )
        
        model.fit(X_train_scaled, y_train)
        y_pred = model.predict(X_test_scaled)
        y_prob = model.predict_proba(X_test_scaled)[:, 1]
        
        metrics = calculate_metrics(y_test, y_pred, y_prob)
        
        results.append({
            'activation': activation,
            'metrics': metrics,
            'iterations': model.n_iter_
        })
        
        print(f"\n{activation.upper()}:")
        print_metrics(metrics)
        print(f"   Iterations: {model.n_iter_}")
    
    return results


def compare_solvers(X_train, y_train, X_test, y_test):
    """
    Compare different optimization solvers.
    """
    print_section_header("SOLVER COMPARISON")
    
    solvers = ['adam', 'sgd', 'lbfgs']
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\nComparing solvers (with hidden_layer_sizes=(50, 25)):")
    print("-" * 60)
    
    results = []
    
    for solver in solvers:
        model = create_mlp_classifier(
            hidden_layer_sizes=(50, 25),
            solver=solver,
            max_iter=500,
            random_state=42
        )
        
        model.fit(X_train_scaled, y_train)
        y_pred = model.predict(X_test_scaled)
        y_prob = model.predict_proba(X_test_scaled)[:, 1]
        
        metrics = calculate_metrics(y_test, y_pred, y_prob)
        
        results.append({
            'solver': solver,
            'metrics': metrics,
            'iterations': model.n_iter_
        })
        
        print(f"\n{solver.upper()}:")
        print_metrics(metrics)
        print(f"   Iterations: {model.n_iter_}")
    
    return results


def compare_regularization(X_train, y_train, X_test, y_test):
    """
    Compare different regularization strengths.
    """
    print_section_header("REGULARIZATION COMPARISON")
    
    alphas = [0.00001, 0.0001, 0.001, 0.01, 0.1]
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\nComparing regularization strengths (alpha):")
    print("-" * 60)
    
    results = []
    
    for alpha in alphas:
        model = create_mlp_classifier(
            hidden_layer_sizes=(50, 25),
            alpha=alpha,
            max_iter=300,
            random_state=42
        )
        
        model.fit(X_train_scaled, y_train)
        y_pred = model.predict(X_test_scaled)
        y_prob = model.predict_proba(X_test_scaled)[:, 1]
        
        metrics = calculate_metrics(y_test, y_pred, y_prob)
        
        results.append({
            'alpha': alpha,
            'metrics': metrics,
            'iterations': model.n_iter_
        })
        
        print(f"\nAlpha = {alpha}:")
        print_metrics(metrics)
    
    return results


# ================================================================================
# SECTION V: BANKING EXAMPLE - FRAUD DETECTION
# ================================================================================

def run_banking_fraud_detection():
    """
    Run the banking fraud detection example.
    """
    print_section_header("BANKING EXAMPLE: CREDIT CARD FRAUD DETECTION")
    
    print("\n1. Generating fraud detection dataset...")
    X, y, feature_names = generate_banking_fraud_data(n_samples=2000)
    print(f"   Total transactions: {len(y)}")
    print(f"   Fraud cases: {int(sum(y))} ({100*sum(y)/len(y):.1f}%)")
    print(f"   Features: {len(feature_names)}")
    
    print("\n2. Splitting data into train and test sets...")
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42, stratify=y
    )
    print(f"   Training set: {len(y_train)} transactions")
    print(f"   Test set: {len(y_test)} transactions")
    print(f"   Training frauds: {int(sum(y_train))}")
    print(f"   Test frauds: {int(sum(y_test))}")
    
    print("\n3. Scaling features...")
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n4. Training neural network model...")
    model = create_mlp_classifier(
        hidden_layer_sizes=(64, 32, 16),
        activation='relu',
        solver='adam',
        alpha=0.001,
        learning_rate_init=0.001,
        max_iter=500,
        early_stopping=True,
        validation_fraction=0.1,
        n_iter_no_change=15,
        random_state=42
    )
    
    model.fit(X_train_scaled, y_train)
    print(f"   Training completed in {model.n_iter_} iterations")
    print(f"   Best validation score: {model.best_validation_score_:.4f}")
    
    print("\n5. Model predictions on test set...")
    y_pred = model.predict(X_test_scaled)
    y_prob = model.predict_proba(X_test_scaled)[:, 1]
    
    print("\n6. Model Evaluation Metrics:")
    metrics = calculate_metrics(y_test, y_pred, y_prob)
    print_metrics(metrics)
    
    print("\n7. Confusion Matrix:")
    cm = confusion_matrix(y_test, y_pred)
    print(f"   True Negatives (correct legit): {cm[0,0]}")
    print(f"   False Positives (false fraud):  {cm[0,1]}")
    print(f"   False Negatives (missed fraud): {cm[1,0]}")
    print(f"   True Positives (caught fraud): {cm[1,1]}")
    
    fraud_detection_rate = cm[1,1] / (cm[1,0] + cm[1,1]) if (cm[1,0] + cm[1,1]) > 0 else 0
    false_alarm_rate = cm[0,1] / (cm[0,0] + cm[0,1]) if (cm[0,0] + cm[0,1]) > 0 else 0
    
    print(f"\n8. Key Business Metrics:")
    print(f"   Fraud Detection Rate (Recall): {fraud_detection_rate:.2%}")
    print(f"   False Alarm Rate: {false_alarm_rate:.2%}")
    
    print("\n9. Detailed Classification Report:")
    print(classification_report(y_test, y_pred, target_names=['Legitimate', 'Fraud']))
    
    print("\n10. Feature Importance (via weights magnitude):")
    coefs = np.abs(model.coefs_[0])
    feature_importance = np.mean(coefs, axis=1)
    sorted_idx = np.argsort(feature_importance)[::-1]
    
    for i in range(min(5, len(feature_names))):
        idx = sorted_idx[i]
        print(f"    {feature_names[idx]}: {feature_importance[idx]:.4f}")
    
    return model, scaler, metrics, y_test, y_pred, y_prob


# ================================================================================
# SECTION VI: HEALTHCARE EXAMPLE - DISEASE DIAGNOSIS
# ================================================================================

def run_healthcare_disease_prediction():
    """
    Run the healthcare disease diagnosis example.
    """
    print_section_header("HEALTHCARE EXAMPLE: DISEASE DIAGNOSIS")
    
    print("\n1. Generating disease diagnosis dataset...")
    X, y, feature_names = generate_healthcare_disease_data(n_samples=1500)
    print(f"   Total patients: {len(y)}")
    print(f"   Disease cases: {int(sum(y))} ({100*sum(y)/len(y):.1f}%)")
    print(f"   Features: {len(feature_names)}")
    
    print("\n2. Splitting data into train and test sets...")
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.25, random_state=42, stratify=y
    )
    print(f"   Training set: {len(y_train)} patients")
    print(f"   Test set: {len(y_test)} patients")
    
    print("\n3. Scaling features...")
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n4. Training neural network model...")
    model = create_mlp_classifier(
        hidden_layer_sizes=(100, 50, 25),
        activation='relu',
        solver='adam',
        alpha=0.0001,
        learning_rate_init=0.001,
        max_iter=500,
        early_stopping=True,
        validation_fraction=0.1,
        n_iter_no_change=15,
        random_state=42
    )
    
    model.fit(X_train_scaled, y_train)
    print(f"   Training completed in {model.n_iter_} iterations")
    print(f"   Best validation score: {model.best_validation_score_:.4f}")
    
    print("\n5. Model predictions on test set...")
    y_pred = model.predict(X_test_scaled)
    y_prob = model.predict_proba(X_test_scaled)[:, 1]
    
    print("\n6. Model Evaluation Metrics:")
    metrics = calculate_metrics(y_test, y_pred, y_prob)
    print_metrics(metrics)
    
    print("\n7. Confusion Matrix:")
    cm = confusion_matrix(y_test, y_pred)
    print(f"   True Negatives (healthy correctly): {cm[0,0]}")
    print(f"   False Positives (healthy as disease): {cm[0,1]}")
    print(f"   False Negatives (missed disease): {cm[1,0]}")
    print(f"   True Positives (disease detected): {cm[1,1]}")
    
    sensitivity = cm[1,1] / (cm[1,0] + cm[1,1]) if (cm[1,0] + cm[1,1]) > 0 else 0
    specificity = cm[0,0] / (cm[0,0] + cm[0,1]) if (cm[0,0] + cm[0,1]) > 0 else 0
    
    print(f"\n8. Key Clinical Metrics:")
    print(f"   Sensitivity (True Positive Rate): {sensitivity:.2%}")
    print(f"   Specificity (True Negative Rate): {specificity:.2%}")
    
    print("\n9. Detailed Classification Report:")
    print(classification_report(y_test, y_pred, target_names=['Healthy', 'Disease']))
    
    print("\n10. Identifying Important Risk Factors:")
    coefs = np.abs(model.coefs_[0])
    feature_importance = np.mean(coefs, axis=1)
    sorted_idx = np.argsort(feature_importance)[::-1]
    
    print("    Top 5 risk factors:")
    for i in range(min(5, len(feature_names))):
        idx = sorted_idx[i]
        print(f"    {i+1}. {feature_names[idx]}: {feature_importance[idx]:.4f}")
    
    return model, scaler, metrics, y_test, y_pred, y_prob


# ================================================================================
# SECTION VII: LEARNING RATE SCHEDULES AND OPTIMIZATION
# ================================================================================

def compare_learning_rates(X_train, y_train, X_test, y_test):
    """
    Compare different learning rate initializations.
    """
    print_section_header("LEARNING RATE COMPARISON")
    
    learning_rates = [0.0001, 0.001, 0.01, 0.1]
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\nComparing learning rate initializations:")
    print("-" * 60)
    
    results = []
    
    for lr_init in learning_rates:
        model = create_mlp_classifier(
            hidden_layer_sizes=(50, 25),
            solver='adam',
            learning_rate_init=lr_init,
            max_iter=300,
            random_state=42
        )
        
        model.fit(X_train_scaled, y_train)
        y_pred = model.predict(X_test_scaled)
        
        metrics = calculate_metrics(y_test, y_pred)
        
        results.append({
            'learning_rate': lr_init,
            'metrics': metrics,
            'iterations': model.n_iter_
        })
        
        print(f"\nLearning Rate = {lr_init}:")
        print_metrics(metrics)
        print(f"   Iterations: {model.n_iter_}")
    
    return results


def demonstrate_early_stopping():
    """
    Demonstrate early stopping effect.
    """
    print_section_header("EARLY STOPPING DEMONSTRATION")
    
    X, y, _ = generate_synthetic_classification_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n1. Training WITH early stopping:")
    model_with = create_mlp_classifier(
        hidden_layer_sizes=(50, 25),
        early_stopping=True,
        validation_fraction=0.1,
        n_iter_no_change=10,
        max_iter=1000,
        random_state=42
    )
    model_with.fit(X_train_scaled, y_train)
    y_pred = model_with.predict(X_test_scaled)
    metrics = calculate_metrics(y_test, y_pred)
    print(f"   Iterations: {model_with.n_iter_}")
    print(f"   Validation score: {model_with.best_validation_score_:.4f}")
    print(f"   Accuracy: {metrics['accuracy']:.4f}")
    
    print("\n2. Training WITHOUT early stopping:")
    model_without = create_mlp_classifier(
        hidden_layer_sizes=(50, 25),
        early_stopping=False,
        max_iter=1000,
        random_state=42
    )
    model_without.fit(X_train_scaled, y_train)
    y_pred = model_without.predict(X_test_scaled)
    metrics = calculate_metrics(y_test, y_pred)
    print(f"   Iterations: {model_without.n_iter_}")
    print(f"   Accuracy: {metrics['accuracy']:.4f}")
    
    return model_with, model_without


# ================================================================================
# SECTION VIII: ADVANCED TOPICS AND TUNING
# ================================================================================

def hyperparameter_tuning_example():
    """
    Demonstrate hyperparameter tuning with GridSearchCV.
    """
    print_section_header("HYPERPARAMETER TUNING EXAMPLE")
    
    X, y, _ = generate_synthetic_classification_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n1. Defining parameter grid...")
    param_grid = {
        'hidden_layer_sizes': [(50,), (100,), (50, 25), (100, 50)],
        'alpha': [0.0001, 0.001, 0.01],
        'learning_rate_init': [0.001, 0.01]
    }
    print(f"   Total combinations: {3*4*2} = 24")
    
    print("\n2. Running GridSearchCV (simplified for demonstration)...")
    
    best_score = 0
    best_params = {}
    best_model = None
    
    for hidden_sizes in param_grid['hidden_layer_sizes']:
        for alpha in param_grid['alpha']:
            for lr in param_grid['learning_rate_init']:
                model = create_mlp_classifier(
                    hidden_layer_sizes=hidden_sizes,
                    alpha=alpha,
                    learning_rate_init=lr,
                    max_iter=200,
                    random_state=42
                )
                model.fit(X_train_scaled, y_train)
                
                cv_scores = cross_val_score(model, X_train_scaled, y_train, cv=3)
                mean_score = cv_scores.mean()
                
                if mean_score > best_score:
                    best_score = mean_score
                    best_params = {'hidden_layer_sizes': hidden_sizes, 
                                  'alpha': alpha, 'learning_rate_init': lr}
                    best_model = model
    
    print(f"\n3. Best parameters found:")
    print(f"   Hidden layer sizes: {best_params['hidden_layer_sizes']}")
    print(f"   Alpha: {best_params['alpha']}")
    print(f"   Learning rate: {best_params['learning_rate_init']}")
    print(f"   Best CV score: {best_score:.4f}")
    
    print("\n4. Evaluating best model on test set...")
    y_pred = best_model.predict(X_test_scaled)
    metrics = calculate_metrics(y_test, y_pred)
    print_metrics(metrics)
    
    return best_params, metrics


def complex_nonlinear_demo():
    """
    Demonstrate neural network on complex nonlinear data.
    """
    print_section_header("COMPLEX NONLINEAR DATA DEMONSTRATION")
    
    print("\n1. Generating complex nonlinear data (moons)...")
    X, y = generate_complex_nonlinear_data(n_samples=400)
    print(f"   Total samples: {len(y)}")
    print(f"   Data requires nonlinear decision boundary")
    
    print("\n2. Splitting and scaling data...")
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.25, random_state=42)
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n3. Training neural network...")
    model = create_mlp_classifier(
        hidden_layer_sizes=(20, 10),
        activation='relu',
        solver='adam',
        alpha=0.001,
        max_iter=500,
        random_state=42
    )
    model.fit(X_train_scaled, y_train)
    
    print("\n4. Evaluating...")
    y_pred = model.predict(X_test_scaled)
    metrics = calculate_metrics(y_test, y_pred)
    print_metrics(metrics)
    print(f"   Iterations: {model.n_iter_}")
    
    return model, metrics


# ================================================================================
# SECTION IX: CROSS-VALIDATION AND STABILITY
# ================================================================================

def cross_validation_demo():
    """
    Demonstrate cross-validation for neural networks.
    """
    print_section_header("CROSS-VALIDATION DEMONSTRATION")
    
    X, y, _ = generate_synthetic_classification_data(n_samples=500)
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    print("\n1. Running 5-fold cross-validation...")
    model = create_mlp_classifier(
        hidden_layer_sizes=(50, 25),
        max_iter=300,
        random_state=42
    )
    
    cv_scores = cross_val_score(model, X_scaled, y, cv=5, scoring='accuracy')
    
    print(f"   Fold scores: {cv_scores}")
    print(f"   Mean: {cv_scores.mean():.4f}")
    print(f"   Std: {cv_scores.std():.4f}")
    
    print("\n2. Running multiple train-test splits for stability...")
    results = []
    for seed in [42, 123, 456, 789, 1000]:
        X_train, X_test, y_train, y_test = train_test_split(
            X_scaled, y, test_size=0.2, random_state=seed
        )
        
        model = create_mlp_classifier(
            hidden_layer_sizes=(50, 25),
            max_iter=300,
            random_state=seed
        )
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_test)
        acc = accuracy_score(y_test, y_pred)
        results.append(acc)
    
    results = np.array(results)
    print(f"   Accuracy scores: {results}")
    print(f"   Mean: {results.mean():.4f}")
    print(f"   Std: {results.std():.4f}")
    
    return cv_scores, results


# ================================================================================
# SECTION X: MODEL INTERPRETATION AND DEBUGGING
# ================================================================================

def analyze_model_convergence():
    """
    Analyze model convergence and training process.
    """
    print_section_header("MODEL CONVERGENCE ANALYSIS")
    
    X, y, _ = generate_synthetic_classification_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n1. Training with verbose output...")
    model = create_mlp_classifier(
        hidden_layer_sizes=(50, 25),
        max_iter=100,
        random_state=42
    )
    
    model.fit(X_train_scaled, y_train)
    
    print("\n2. Analyzing training history:")
    print(f"   Final loss: {model.loss_:.6f}")
    print(f"   Iterations: {model.n_iter_}")
    print(f"   Number of layers: {len(model.hidden_layer_sizes)}")
    print(f"   Output activation: {model.out_activation_}")
    
    if hasattr(model, 'loss_curve_'):
        print(f"   Loss curve available: {len(model.loss_curve_)} points")
    
    print("\n3. Checking gradient information:")
    print(f"   Number of input features: {model.n_layers_}")
    print(f"   Layers: {model.n_layers_}")
    print(f"   Outputs: {model.n_outputs_}")
    
    print("\n4. Weights analysis:")
    for i, (w, b) in enumerate(zip(model.coefs_, model.intercepts_)):
        print(f"   Layer {i+1}: weights shape {w.shape}, bias shape {b.shape}")
    
    return model


# ================================================================================
# SECTION XI: PRACTICAL TIPS AND BEST PRACTICES
# ================================================================================

def print_best_practices():
    """
    Print best practices for neural network classification.
    """
    print_section_header("BEST PRACTICES FOR NEURAL NETWORKS")
    
    practices = [
        ("1. Data Preprocessing", [
            "  - Always scale features using StandardScaler or MinMaxScaler",
            "  - Handle missing values before training",
            "  - Use stratified splitting for imbalanced datasets"
        ]),
        ("2. Architecture Design", [
            "  - Start with simple architectures (1-2 hidden layers)",
            "  - Use powers of 2 for layer sizes (32, 64, 128)",
            "  - Deeper networks can capture more complex patterns"
        ]),
        ("3. Hyperparameter Tuning", [
            "  - Use early stopping to prevent overfitting",
            "  - Try different solvers (adam, sgd, lbfgs)",
            "  - Tune alpha for regularization"
        ]),
        ("4. Training", [
            "  - Use sufficient max_iter (500-1000)",
            "  - Monitor validation score during training",
            "  - Use cross-validation for hyperparameter tuning"
        ]),
        ("5. Evaluation", [
            "  - Use multiple metrics (accuracy, precision, recall, F1, ROC-AUC)",
            "  - Consider class imbalance",
            "  - Test on held-out data"
        ])
    ]
    
    for title, items in practices:
        print(f"\n{title}:")
        for item in items:
            print(item)


# ================================================================================
# SECTION XII: MAIN FUNCTION
# ================================================================================

def main():
    """
    Main function to run all demonstrations.
    """
    print("\n" + "=" * 80)
    print("  NEURAL NETWORK CLASSIFICATION - COMPREHENSIVE IMPLEMENTATION")
    print("=" * 80)
    
    print("\n" + "-" * 80)
    print("  This implementation covers:")
    print("  - Basic MLPClassifier usage")
    print("  - Architecture comparison")
    print("  - Activation function comparison")
    print("  - Solver comparison")
    print("  - Regularization comparison")
    print("  - Banking fraud detection example")
    print("  - Healthcare disease diagnosis example")
    print("  - Learning rate tuning")
    print("  - Early stopping")
    print("  - Hyperparameter tuning")
    print("  - Complex nonlinear data handling")
    print("  - Cross-validation")
    print("  - Model convergence analysis")
    print("-" * 80)
    
    try:
        print_section_header("RUNNING BASIC MLP DEMONSTRATION")
        demonstrate_basic_mlp()
    except Exception as e:
        print(f"Error in basic MLP: {e}")
    
    try:
        print_section_header("RUNNING ARCHITECTURE COMPARISON")
        X, y, _ = generate_synthetic_classification_data(n_samples=500)
        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
        compare_architectures(X_train, y_train, X_test, y_test)
    except Exception as e:
        print(f"Error in architecture comparison: {e}")
    
    try:
        print_section_header("RUNNING ACTIVATION FUNCTION COMPARISON")
        X, y, _ = generate_synthetic_classification_data(n_samples=500)
        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
        compare_activation_functions(X_train, y_train, X_test, y_test)
    except Exception as e:
        print(f"Error in activation comparison: {e}")
    
    try:
        print_section_header("RUNNING SOLVER COMPARISON")
        X, y, _ = generate_synthetic_classification_data(n_samples=500)
        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
        compare_solvers(X_train, y_train, X_test, y_test)
    except Exception as e:
        print(f"Error in solver comparison: {e}")
    
    try:
        print_section_header("RUNNING BANKING FRAUD DETECTION")
        run_banking_fraud_detection()
    except Exception as e:
        print(f"Error in banking example: {e}")
    
    try:
        print_section_header("RUNNING HEALTHCARE DISEASE DIAGNOSIS")
        run_healthcare_disease_prediction()
    except Exception as e:
        print(f"Error in healthcare example: {e}")
    
    try:
        print_section_header("RUNNING EARLY STOPPING DEMONSTRATION")
        demonstrate_early_stopping()
    except Exception as e:
        print(f"Error in early stopping: {e}")
    
    try:
        print_section_header("RUNNING HYPERPARAMETER TUNING")
        hyperparameter_tuning_example()
    except Exception as e:
        print(f"Error in hyperparameter tuning: {e}")
    
    try:
        print_section_header("RUNNING COMPLEX NONLINEAR DATA DEMO")
        complex_nonlinear_demo()
    except Exception as e:
        print(f"Error in complex nonlinear: {e}")
    
    try:
        print_section_header("RUNNING CROSS-VALIDATION")
        cross_validation_demo()
    except Exception as e:
        print(f"Error in cross-validation: {e}")
    
    print_best_practices()
    
    print("\n" + "=" * 80)
    print("  IMPLEMENTATION COMPLETE")
    print("=" * 80)
    
    print("\n>>> KEY TAKEAWAYS:")
    print("  1. Neural networks excel at capturing complex nonlinear patterns")
    print("  2. MLPClassifier from sklearn provides flexible neural network classification")
    print("  3. Feature scaling is essential for neural network training")
    print("  4. Architecture choice depends on problem complexity")
    print("  5. Early stopping helps prevent overfitting")
    print("  6. Adam solver generally works well for most problems")
    print("  7. ReLU activation is the default and often best choice")
    print("  8. Regularization (alpha) helps control model complexity")
    print("  9. Use cross-validation for reliable performance estimation")
    print("  10. Combine multiple metrics for comprehensive evaluation")
    print("=" * 80)


if __name__ == "__main__":
    main()