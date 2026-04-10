# Topic: Gradient Boosting Classification
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Gradient Boosting Classification

I. INTRODUCTION
    - Overview of Gradient Boosting and its importance in machine learning
    - Historical context and evolution of boosting algorithms
    - Comparison with other ensemble methods
    - Applications in various domains

II. CORE CONCEPTS
    - Sequential learning and additive modeling
    - Loss functions (exponential, log loss, hinge)
    - Gradient descent optimization in functional space
    - Weak learners and decision stumps
    - Learning rate (shrinkage) and n_estimators
    - Subsampling and stochastic gradient boosting
    - Early stopping criteria

III. IMPLEMENTATION
    - Creating synthetic datasets
    - Building GradientBoostingClassifier models
    - Hyperparameter tuning strategies
    - Feature importance analysis
    - Cross-validation approaches

IV. EXAMPLES
    A. Banking/Finance Example (Credit Risk Assessment)
       - Predicting loan defaults
       - Feature engineering for financial data
       - Model evaluation with industry metrics
       
    B. Healthcare Example (Patient Outcome Prediction)
       - Predicting patient mortality
       - Medical data preprocessing
       - Clinical interpretation

V. OUTPUT RESULTS
    - Comprehensive metrics and visualizations
    - ROC curves and precision-recall curves
    - Confusion matrix analysis
    - Model comparison tables

VI. TESTING
    - Unit tests for core functionality
    - Integration tests
    - Performance benchmarks
    - Validation checks

VII. ADVANCED TOPICS
    - XGBoost, LightGBM, CatBoost comparisons
    - Handling class imbalance
    - Missing value strategies
    - Regularization techniques
    - Model interpretability with SHAP

VIII. CONCLUSION
    - Summary of key concepts
    - Best practices
    - Future directions
"""

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.datasets import make_classification, make_blobs
from sklearn.model_selection import train_test_split, cross_val_score, StratifiedKFold, learning_curve
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.ensemble import (
    GradientBoostingClassifier, 
    RandomForestClassifier, 
    AdaBoostClassifier,
    BaggingClassifier
)
from sklearn.tree import DecisionTreeClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import (
    accuracy_score, 
    precision_score, 
    recall_score, 
    f1_score, 
    roc_auc_score,
    confusion_matrix, 
    classification_report,
    roc_curve,
    precision_recall_curve,
    mean_squared_error,
    mean_absolute_error
)
import warnings
warnings.filterwarnings('ignore')

np.random.seed(42)


def print_section(title):
    """Print a formatted section header."""
    print("\n" + "=" * 80)
    print(f"  {title}")
    print("=" * 80)


def print_subsection(title):
    """Print a formatted subsection header."""
    print(f"\n--- {title} ---")


class GradientBoostingImplementation:
    """
    Custom implementation of Gradient Boosting Classifier
    to demonstrate the core algorithm mechanics.
    """
    
    def __init__(self, n_estimators=10, learning_rate=0.1, max_depth=3, 
                 min_samples_split=2, min_samples_leaf=1, subsample=1.0):
        self.n_estimators = n_estimators
        self.learning_rate = learning_rate
        self.max_depth = max_depth
        self.min_samples_split = min_samples_split
        self.min_samples_leaf = min_samples_leaf
        self.subsample = subsample
        self.estimators = []
        self.initial_prediction = None
        
    def _compute_pseudo_residuals(self, y, predictions):
        """Compute pseudo-residuals for gradient boosting."""
        probabilities = 1 / (1 + np.exp(-predictions))
        probabilities = np.clip(probabilities, 1e-15, 1 - 1e-15)
        residuals = y - probabilities
        return residuals
    
    def _fit_decision_tree(self, X, residuals, sample_indices):
        """Fit a decision tree to predict residuals."""
        tree = DecisionTreeClassifier(
            max_depth=self.max_depth,
            min_samples_split=self.min_samples_split,
            min_samples_leaf=self.min_samples_leaf
        )
        
        X_subset = X[sample_indices]
        residuals_subset = residuals[sample_indices]
        
        tree.fit(X_subset, residuals_subset)
        return tree
    
    def fit(self, X, y):
        """Fit the gradient boosting model."""
        X = np.array(X)
        y = np.array(y)
        
        self.initial_prediction = np.log(np.mean(y) / (1 - np.mean(y)))
        
        current_predictions = np.full(len(y), self.initial_prediction)
        
        self.estimators = []
        
        for i in range(self.n_estimators):
            pseudo_residuals = self._compute_pseudo_residuals(y, current_predictions)
            
            sample_indices = np.arange(len(y))
            if self.subsample < 1.0:
                n_samples = int(len(y) * self.subsample)
                sample_indices = np.random.choice(len(y), n_samples, replace=False)
            
            tree = self._fit_decision_tree(X, pseudo_residuals, sample_indices)
            
            leaf_values = {}
            for leaf in tree.tree_.feature:
                if leaf >= 0:
                    pass
            
            self.estimators.append(tree)
            
            tree_predictions = tree.predict(X)
            current_predictions += self.learning_rate * tree_predictions
        
        return self
    
    def predict_proba(self, X):
        """Predict class probabilities."""
        X = np.array(X)
        
        current_predictions = np.full(len(X), self.initial_prediction)
        
        for tree in self.estimators:
            tree_predictions = tree.predict(X)
            current_predictions += self.learning_rate * tree_predictions
        
        probabilities = 1 / (1 + np.exp(-current_predictions))
        return np.column_stack([1 - probabilities, probabilities])
    
    def predict(self, X):
        """Predict class labels."""
        proba = self.predict_proba(X)
        return (proba[:, 1] >= 0.5).astype(int)


def generate_classification_data(n_samples=500, n_features=10, n_informative=5, 
                                 n_redundant=2, n_clusters_per_class=2,
                                 class_separation=1.0, flip_y=0.05):
    """
    Generate synthetic classification data for testing gradient boosting.
    
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
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target labels
    feature_names : list
        Names of features
    """
    print(f"Generating classification data with {n_samples} samples and {n_features} features...")
    
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_clusters_per_class=n_clusters_per_class,
        class_sep=class_separation,
        flip_y=flip_y,
        random_state=42
    )
    
    feature_names = [f'Feature_{i+1}' for i in range(n_features)]
    for i in range(n_informative):
        feature_names[i] = f'Informative_{i+1}'
    for i in range(n_informative, n_informative + n_redundant):
        feature_names[i] = f'Redundant_{i+1}'
    
    return X, y, feature_names


def generate_imbalanced_data(n_samples=1000, n_features=10, imbalance_ratio=0.1):
    """
    Generate imbalanced classification data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features
    imbalance_ratio : float
        Ratio of minority to majority class
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target labels (0 or 1)
    """
    print(f"Generating imbalanced data with ratio {imbalance_ratio}...")
    
    n_minority = int(n_samples * imbalance_ratio)
    n_majority = n_samples - n_minority
    
    X_minority, y_minority = make_blobs(
        n_samples=n_minority,
        centers=1,
        n_features=n_features,
        cluster_std=1.5,
        random_state=42
    )
    y_minority = np.ones(n_minority)
    
    X_majority, y_majority = make_blobs(
        n_samples=n_majority,
        centers=1,
        n_features=n_features,
        cluster_std=2.0,
        center_box=(-5, 5),
        random_state=123
    )
    y_majority = np.zeros(n_majority)
    
    X = np.vstack([X_minority, X_majority])
    y = np.concatenate([y_minority, y_majority])
    
    shuffle_idx = np.random.permutation(len(y))
    X = X[shuffle_idx]
    y = y[shuffle_idx]
    
    return X, y


def generate_multiclass_data(n_samples=600, n_features=8, n_classes=3):
    """
    Generate multi-class classification data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples
    n_features : int
        Number of features
    n_classes : int
        Number of classes
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target labels
    """
    print(f"Generating multi-class data with {n_classes} classes...")
    
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=5,
        n_redundant=2,
        n_clusters_per_class=1,
        n_classes=n_classes,
        class_sep=1.5,
        random_state=42
    )
    
    return X, y


def core_gradient_boosting_implementation():
    """
    Main implementation of core gradient boosting concepts.
    """
    print_section("CORE GRADIENT BOOSTING IMPLEMENTATION")
    
    print_subsection("1. Basic Dataset Creation")
    X, y, feature_names = generate_classification_data(
        n_samples=500, 
        n_features=10, 
        n_informative=5,
        class_separation=1.2
    )
    
    print(f"Dataset shape: {X.shape}")
    print(f"Class distribution: {np.bincount(y)}")
    print(f"Feature names: {feature_names[:5]}...")
    
    print_subsection("2. Train-Test Split")
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    print(f"Training set size: {X_train.shape[0]}")
    print(f"Test set size: {X_test.shape[0]}")
    
    print_subsection("3. Feature Scaling")
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("Features scaled using StandardScaler")
    print(f"Mean of scaled features: {np.mean(X_train_scaled, axis=0)[:3]}")
    print(f"Std of scaled features: {np.std(X_train_scaled, axis=0)[:3]}")
    
    print_subsection("4. Basic Gradient Boosting Classifier")
    gb_classifier = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        min_samples_split=5,
        min_samples_leaf=2,
        subsample=0.8,
        random_state=42
    )
    
    gb_classifier.fit(X_train_scaled, y_train)
    
    print("Gradient Boosting Classifier trained successfully")
    print(f"Number of estimators: {gb_classifier.n_estimators}")
    print(f"Number of features: {gb_classifier.n_features_in_}")
    
    print_subsection("5. Making Predictions")
    y_pred = gb_classifier.predict(X_test_scaled)
    y_pred_proba = gb_classifier.predict_proba(X_test_scaled)
    
    print(f"Predictions shape: {y_pred.shape}")
    print(f"Probability predictions for class 1: {y_pred_proba[:5, 1]}")
    
    print_subsection("6. Model Evaluation")
    accuracy = accuracy_score(y_test, y_pred)
    precision = precision_score(y_test, y_pred)
    recall = recall_score(y_test, y_pred)
    f1 = f1_score(y_test, y_pred)
    
    print(f"Accuracy: {accuracy:.4f}")
    print(f"Precision: {precision:.4f}")
    print(f"Recall: {recall:.4f}")
    print(f"F1 Score: {f1:.4f}")
    
    print_subsection("7. Feature Importance")
    feature_importance = gb_classifier.feature_importances_
    importance_df = pd.DataFrame({
        'Feature': feature_names,
        'Importance': feature_importance
    }).sort_values('Importance', ascending=False)
    
    print("Top 5 Important Features:")
    print(importance_df.head().to_string(index=False))
    
    print_subsection("8. Staged Predict (Sequential Learning)")
    for i, y_pred_stage in enumerate(gb_classifier.staged_predict(X_test_scaled)):
        if i % 10 == 0 or i == gb_classifier.n_estimators - 1:
            stage_accuracy = accuracy_score(y_test, y_pred_stage)
            print(f"Stage {i+1}: Accuracy = {stage_accuracy:.4f}")
    
    print_subsection("9. Learning Rate Impact")
    learning_rates = [0.01, 0.05, 0.1, 0.2, 0.5]
    n_estimators_list = [50, 100, 200]
    
    results = []
    for lr in learning_rates:
        for n_est in n_estimators_list:
            gb = GradientBoostingClassifier(
                n_estimators=n_est,
                learning_rate=lr,
                max_depth=3,
                random_state=42
            )
            gb.fit(X_train_scaled, y_train)
            y_pred = gb.predict(X_test_scaled)
            acc = accuracy_score(y_test, y_pred)
            results.append({
                'Learning Rate': lr,
                'n_estimators': n_est,
                'Accuracy': acc
            })
    
    results_df = pd.DataFrame(results)
    print("\nLearning Rate and n_estimators Impact:")
    print(results_df.to_string(index=False))
    
    print_subsection("10. Subsample Impact")
    subsample_rates = [0.5, 0.6, 0.7, 0.8, 0.9, 1.0]
    
    subsample_results = []
    for subsample in subsample_rates:
        gb = GradientBoostingClassifier(
            n_estimators=50,
            learning_rate=0.1,
            max_depth=3,
            subsample=subsample,
            random_state=42
        )
        gb.fit(X_train_scaled, y_train)
        y_pred = gb.predict(X_test_scaled)
        acc = accuracy_score(y_test, y_pred)
        subsample_results.append({
            'Subsample': subsample,
            'Accuracy': acc
        })
    
    subsample_df = pd.DataFrame(subsample_results)
    print("\nSubsample Impact:")
    print(subsample_df.to_string(index=False))
    
    return {
        'classifier': gb_classifier,
        'accuracy': accuracy,
        'feature_importance': importance_df,
        'results_df': results_df
    }


def boosting_comparison():
    """
    Compare Gradient Boosting with other boosting methods.
    """
    print_section("BOOSTING METHODS COMPARISON")
    
    X, y, feature_names = generate_classification_data(
        n_samples=500,
        n_features=10,
        class_separation=1.0
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subsection("1. Gradient Boosting Classifier")
    gb = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb.fit(X_train_scaled, y_train)
    y_pred_gb = gb.predict(X_test_scaled)
    y_proba_gb = gb.predict_proba(X_test_scaled)[:, 1]
    
    print(f"Training time: GradientBoostingClassifier")
    print(f"Accuracy: {accuracy_score(y_test, y_pred_gb):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba_gb):.4f}")
    
    print_subsection("2. Random Forest Classifier")
    rf = RandomForestClassifier(
        n_estimators=50,
        max_depth=5,
        random_state=42
    )
    rf.fit(X_train_scaled, y_train)
    y_pred_rf = rf.predict(X_test_scaled)
    y_proba_rf = rf.predict_proba(X_test_scaled)[:, 1]
    
    print(f"Accuracy: {accuracy_score(y_test, y_pred_rf):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba_rf):.4f}")
    
    print_subsection("3. AdaBoost Classifier")
    ada = AdaBoostClassifier(
        n_estimators=50,
        learning_rate=0.1,
        random_state=42
    )
    ada.fit(X_train_scaled, y_train)
    y_pred_ada = ada.predict(X_test_scaled)
    y_proba_ada = ada.predict_proba(X_test_scaled)[:, 1]
    
    print(f"Accuracy: {accuracy_score(y_test, y_pred_ada):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba_ada):.4f}")
    
    print_subsection("4. Logistic Regression (Baseline)")
    lr = LogisticRegression(random_state=42, max_iter=1000)
    lr.fit(X_train_scaled, y_train)
    y_pred_lr = lr.predict(X_test_scaled)
    y_proba_lr = lr.predict_proba(X_test_scaled)[:, 1]
    
    print(f"Accuracy: {accuracy_score(y_test, y_pred_lr):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba_lr):.4f}")
    
    print_subsection("5. Comparison Summary")
    comparison_data = {
        'Model': ['Gradient Boosting', 'Random Forest', 'AdaBoost', 'Logistic Regression'],
        'Accuracy': [
            accuracy_score(y_test, y_pred_gb),
            accuracy_score(y_test, y_pred_rf),
            accuracy_score(y_test, y_pred_ada),
            accuracy_score(y_test, y_pred_lr)
        ],
        'ROC-AUC': [
            roc_auc_score(y_test, y_proba_gb),
            roc_auc_score(y_test, y_proba_rf),
            roc_auc_score(y_test, y_proba_ada),
            roc_auc_score(y_test, y_proba_lr)
        ],
        'Precision': [
            precision_score(y_test, y_pred_gb),
            precision_score(y_test, y_pred_rf),
            precision_score(y_test, y_pred_ada),
            precision_score(y_test, y_pred_lr)
        ],
        'Recall': [
            recall_score(y_test, y_pred_gb),
            recall_score(y_test, y_pred_rf),
            recall_score(y_test, y_pred_ada),
            recall_score(y_test, y_pred_lr)
        ],
        'F1': [
            f1_score(y_test, y_pred_gb),
            f1_score(y_test, y_pred_rf),
            f1_score(y_test, y_pred_ada),
            f1_score(y_test, y_pred_lr)
        ]
    }
    
    comparison_df = pd.DataFrame(comparison_data)
    print("\nModel Comparison Table:")
    print(comparison_df.to_string(index=False))
    
    print_subsection("6. Key Differences Explained")
    print("""
    Gradient Boosting vs Random Forest:
    ------------------------------------
    - Gradient Boosting: Sequential learning, builds on errors
    - Random Forest: Parallel learning, independent trees
    
    Gradient Boosting vs AdaBoost:
    -------------------------------
    - Gradient Boosting: General loss functions (log loss)
    - AdaBoost: Exponential loss function
    
    Gradient Boosting vs Logistic Regression:
    -------------------------------------------
    - Gradient Boosting: Non-linear decision boundaries
    - Logistic Regression: Linear decision boundaries
    """)
    
    return comparison_df


def early_stopping_example():
    """
    Demonstrate early stopping in gradient boosting.
    """
    print_section("EARLY STOPPING IMPLEMENTATION")
    
    X, y, feature_names = generate_classification_data(
        n_samples=1000,
        n_features=10,
        class_separation=1.0
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subsection("1. Training with Early Stopping")
    gb_with_early_stopping = GradientBoostingClassifier(
        n_estimators=200,
        learning_rate=0.1,
        max_depth=3,
        validation_fraction=0.1,
        n_iter_no_change=10,
        random_state=42
    )
    
    gb_with_early_stopping.fit(X_train_scaled, y_train)
    
    print(f"Best iteration: {gb_with_early_stopping.n_estimators_}")
    print(f"Actual iterations used: {gb_with_early_stopping.n_estimators_}")
    
    y_pred = gb_with_early_stopping.predict(X_test_scaled)
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    
    print_subsection("2. Training without Early Stopping")
    gb_without_early_stopping = GradientBoostingClassifier(
        n_estimators=200,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    
    gb_without_early_stopping.fit(X_train_scaled, y_train)
    
    print(f"Iterations used: {gb_without_early_stopping.n_estimators}")
    
    y_pred = gb_without_early_stopping.predict(X_test_scaled)
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    
    print_subsection("3. Score Evolution Across Iterations")
    train_scores = []
    test_scores = []
    iterations = []
    
    for y_train_pred, y_test_pred in zip(
        gb_with_early_stopping.staged_predict(X_train_scaled),
        gb_with_early_stopping.staged_predict(X_test_scaled)
    ):
        iterations.append(iterations[-1] + 1 if iterations else 1)
        train_scores.append(accuracy_score(y_train, y_train_pred))
        test_scores.append(accuracy_score(y_test, y_test_pred))
    
    score_evolution = pd.DataFrame({
        'Iteration': range(1, len(train_scores) + 1),
        'Train_Accuracy': train_scores,
        'Test_Accuracy': test_scores
    })
    
    print("\nScore Evolution (selected iterations):")
    print(score_evolution.iloc[::10].to_string(index=False))
    
    return {
        'gb_with_early_stopping': gb_with_early_stopping,
        'gb_without_early_stopping': gb_without_early_stopping
    }


def banking_example():
    """
    Banking/Finance example: Credit Risk Assessment
    """
    print_section("BANKING EXAMPLE: CREDIT RISK ASSESSMENT")
    
    print_subsection("1. Data Generation - Simulated Credit Data")
    n_samples = 1000
    
    np.random.seed(42)
    
    credit_scores = np.random.normal(650, 100, n_samples)
    income = np.random.exponential(50000, n_samples)
    employment_years = np.random.exponential(5, n_samples)
    debt_to_income = np.random.exponential(0.3, n_samples)
    num_credit_lines = np.random.poisson(5, n_samples)
    age = np.random.normal(35, 10, n_samples)
    savings = np.random.exponential(10000, n_samples)
    existing_loans = np.random.poisson(2, n_samples)
    payment_regularity = np.random.normal(0, 1, n_samples)
    num_inquiries = np.random.poisson(2, n_samples)
    
    X = np.column_stack([
        credit_scores,
        income,
        employment_years,
        debt_to_income,
        num_credit_lines,
        age,
        savings,
        existing_loans,
        payment_regularity,
        num_inquiries
    ])
    
    credit_scores_norm = (credit_scores - 580) / (850 - 580)
    income_norm = (income - 20000) / (200000 - 20000)
    high_dti = (debt_to_income > 0.4).astype(float)
    low_savings = (savings < 5000).astype(float)
    many_inquiries = (num_inquiries > 5).astype(float)
    low_credit = (credit_scores < 620).astype(float)
    short_employment = (employment_years < 2).astype(float)
    
    default_prob = (
        0.3 * high_dti +
        0.2 * low_savings +
        0.15 * many_inquiries +
        0.2 * low_credit +
        0.1 * short_employment
    )
    default_prob = np.clip(default_prob, 0.05, 0.95)
    
    y = (np.random.random(n_samples) < default_prob).astype(int)
    
    feature_names = [
        'Credit_Score', 'Income', 'Employment_Years', 'Debt_to_Income',
        'Num_Credit_Lines', 'Age', 'Savings', 'Existing_Loans',
        'Payment_Regularity', 'Num_Inquiries'
    ]
    
    print(f"Generated {n_samples} credit applications")
    print(f"Default rate: {y.mean():.2%}")
    
    print_subsection("2. Train-Test Split")
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    print(f"Training samples: {X_train.shape[0]}")
    print(f"Test samples: {X_test.shape[0]}")
    print(f"Training default rate: {y_train.mean():.2%}")
    print(f"Test default rate: {y_test.mean():.2%}")
    
    print_subsection("3. Feature Scaling")
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print(f"Scaled features mean: {np.round(np.mean(X_train_scaled, axis=0)[:3], 2)}")
    print(f"Scaled features std: {np.round(np.std(X_train_scaled, axis=0)[:3], 2)}")
    
    print_subsection("4. Gradient Boosting Model")
    gb_credit = GradientBoostingClassifier(
        n_estimators=100,
        learning_rate=0.05,
        max_depth=4,
        min_samples_split=10,
        min_samples_leaf=5,
        subsample=0.8,
        max_features='sqrt',
        random_state=42
    )
    
    gb_credit.fit(X_train_scaled, y_train)
    
    print("Gradient Boosting model trained")
    print(f"Number of estimators: {gb_credit.n_estimators}")
    
    print_subsection("5. Predictions and Evaluation")
    y_pred = gb_credit.predict(X_test_scaled)
    y_pred_proba = gb_credit.predict_proba(X_test_scaled)[:, 1]
    
    accuracy = accuracy_score(y_test, y_pred)
    precision = precision_score(y_test, y_pred)
    recall = recall_score(y_test, y_pred)
    f1 = f1_score(y_test, y_pred)
    roc_auc = roc_auc_score(y_test, y_pred_proba)
    
    print(f"Accuracy: {accuracy:.4f}")
    print(f"Precision: {precision:.4f}")
    print(f"Recall: {recall:.4f}")
    print(f"F1 Score: {f1:.4f}")
    print(f"ROC-AUC: {roc_auc:.4f}")
    
    print_subsection("6. Confusion Matrix")
    cm = confusion_matrix(y_test, y_pred)
    print("\nConfusion Matrix:")
    print(f"                Predicted")
    print(f"              No Default  Default")
    print(f"Actual No Default  {cm[0,0]:5d}    {cm[0,1]:5d}")
    print(f"       Default    {cm[1,0]:5d}    {cm[1,1]:5d}")
    
    tn, fp, fn, tp = cm.ravel()
    print(f"\nTrue Negatives: {tn}")
    print(f"False Positives: {fp}")
    print(f"False Negatives: {fn}")
    print(f"True Positives: {tp}")
    
    print_subsection("7. Classification Report")
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred, target_names=['No Default', 'Default']))
    
    print_subsection("8. Feature Importance")
    feature_importance = gb_credit.feature_importances_
    importance_df = pd.DataFrame({
        'Feature': feature_names,
        'Importance': feature_importance
    }).sort_values('Importance', ascending=False)
    
    print("\nFeature Importance Ranking:")
    print(importance_df.to_string(index=False))
    
    print_subsection("9. Risk Score Distribution")
    low_risk = (y_pred_proba < 0.3).sum()
    medium_risk = ((y_pred_proba >= 0.3) & (y_pred_proba < 0.7)).sum()
    high_risk = (y_pred_proba >= 0.7).sum()
    
    print(f"\nRisk Distribution:")
    print(f"Low Risk (< 30%): {low_risk} applications ({low_risk/len(y_pred_proba):.1%})")
    print(f"Medium Risk (30-70%): {medium_risk} applications ({medium_risk/len(y_pred_proba):.1%})")
    print(f"High Risk (> 70%): {high_risk} applications ({high_risk/len(y_pred_proba):.1%})")
    
    print_subsection("10. Business Metrics")
    approved = (y_pred == 0).sum()
    defaults_predicted = (y_pred == 1).sum()
    actual_defaults = y_test.sum()
    correct_defaults = ((y_pred == 1) & (y_test == 1)).sum()
    
    print(f"\nBusiness Impact Analysis:")
    print(f"Applications Approved: {approved}")
    print(f"Expected Defaults in Approved: {actual_defaults - fn}")
    print(f"Defaults Correctly Identified: {correct_defaults}")
    print(f"Potential Loss Avoided: ${correct_defaults * 15000:.0f} (avg loan $15,000)")
    
    return {
        'model': gb_credit,
        'accuracy': accuracy,
        'precision': precision,
        'recall': recall,
        'f1': f1,
        'roc_auc': roc_auc,
        'feature_importance': importance_df,
        'y_pred_proba': y_pred_proba
    }


def healthcare_example():
    """
    Healthcare example: Patient Outcome Prediction
    """
    print_section("HEALTHCARE EXAMPLE: PATIENT OUTCOME PREDICTION")
    
    print_subsection("1. Data Generation - Simulated Medical Data")
    n_samples = 1000
    
    np.random.seed(42)
    
    age = np.random.normal(60, 15, n_samples)
    bmi = np.random.normal(28, 5, n_samples)
    systolic_bp = np.random.normal(140, 20, n_samples)
    diastolic_bp = np.random.normal(85, 10, n_samples)
    heart_rate = np.random.normal(75, 12, n_samples)
    temperature = np.random.normal(98.6, 1.2, n_samples)
    oxygen_saturation = np.random.normal(96, 3, n_samples)
    white_blood_cell = np.random.normal(7000, 2000, n_samples)
    hemoglobin = np.random.normal(14, 2, n_samples)
    platelet_count = np.random.normal(250000, 50000, n_samples)
    creatinine = np.random.normal(1.0, 0.3, n_samples)
    alt_enzyme = np.random.normal(25, 10, n_samples)
    
    X = np.column_stack([
        age,
        bmi,
        systolic_bp,
        diastolic_bp,
        heart_rate,
        temperature,
        oxygen_saturation,
        white_blood_cell,
        hemoglobin,
        platelet_count,
        creatinine,
        alt_enzyme
    ])
    
    severe_age = (age > 70).astype(float)
    high_bmi = (bmi > 35).astype(float)
    hypertension = (systolic_bp > 160).astype(float)
    low_oxygen = (oxygen_saturation < 92).astype(float)
    high_wbc = (white_blood_cell > 10000).astype(float)
    low_hemoglobin = (hemoglobin < 10).astype(float)
    low_platelets = (platelet_count < 150000).astype(float)
    high_creatinine = (creatinine > 1.5).astype(float)
    
    mortality_prob =(
        0.2 * severe_age +
        0.15 * high_bmi +
        0.15 * hypertension +
        0.2 * low_oxygen +
        0.1 * high_wbc +
        0.1 * low_hemoglobin +
        0.05 * low_platelets +
        0.1 * high_creatinine
    )
    mortality_prob = np.clip(mortality_prob, 0.02, 0.98)
    
    y = (np.random.random(n_samples) < mortality_prob).astype(int)
    
    feature_names = [
        'Age', 'BMI', 'Systolic_BP', 'Diastolic_BP', 'Heart_Rate',
        'Temperature', 'Oxygen_Saturation', 'White_Blood_Cell',
        'Hemoglobin', 'Platelet_Count', 'Creatinine', 'ALT_Enzyme'
    ]
    
    print(f"Generated {n_samples} patient records")
    print(f"Mortality rate: {y.mean():.2%}")
    
    print_subsection("2. Train-Test Split")
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    print(f"Training samples: {X_train.shape[0]}")
    print(f"Test samples: {X_test.shape[0]}")
    print(f"Training mortality rate: {y_train.mean():.2%}")
    print(f"Test mortality rate: {y_test.mean():.2%}")
    
    print_subsection("3. Feature Scaling")
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("Features scaled using StandardScaler")
    
    print_subsection("4. Gradient Boosting Model with Class Weights")
    sample_weight_train = np.where(y_train == 1, 3.0, 1.0)
    
    gb_health = GradientBoostingClassifier(
        n_estimators=100,
        learning_rate=0.05,
        max_depth=4,
        min_samples_split=10,
        min_samples_leaf=5,
        subsample=0.8,
        max_features='sqrt',
        random_state=42
    )
    
    gb_health.fit(X_train_scaled, y_train, sample_weight=sample_weight_train)
    
    print("Gradient Boosting model trained with weighted samples")
    print(f"Number of estimators: {gb_health.n_estimators}")
    
    print_subsection("5. Predictions and Evaluation")
    y_pred = gb_health.predict(X_test_scaled)
    y_pred_proba = gb_health.predict_proba(X_test_scaled)[:, 1]
    
    accuracy = accuracy_score(y_test, y_pred)
    precision = precision_score(y_test, y_pred, zero_division=0)
    recall = recall_score(y_test, y_pred)
    f1 = f1_score(y_test, y_pred)
    roc_auc = roc_auc_score(y_test, y_pred_proba)
    
    print(f"Accuracy: {accuracy:.4f}")
    print(f"Precision: {precision:.4f}")
    print(f"Recall: {recall:.4f}")
    print(f"F1 Score: {f1:.4f}")
    print(f"ROC-AUC: {roc_auc:.4f}")
    
    print_subsection("6. Confusion Matrix")
    cm = confusion_matrix(y_test, y_pred)
    print("\nConfusion Matrix:")
    print(f"                  Predicted")
    print(f"              Survived  Deceased")
    print(f"Actual Survived  {cm[0,0]:5d}    {cm[0,1]:5d}")
    print(f"       Deceased  {cm[1,0]:5d}    {cm[1,1]:5d}")
    
    tn, fp, fn, tp = cm.ravel()
    print(f"\nTrue Negatives: {tn}")
    print(f"False Positives: {fp}")
    print(f"False Negatives: {fn}")
    print(f"True Positives: {tp}")
    
    print_subsection("7. Critical Care Triage")
    critical = (y_pred_proba > 0.7).sum()
    serious = ((y_pred_proba > 0.3) & (y_pred_proba <= 0.7)).sum()
    stable = (y_pred_proba <= 0.3).sum()
    
    print(f"\nTriage Distribution:")
    print(f"Critical Care: {critical} patients ({critical/len(y_pred_proba):.1%})")
    print(f"Serious: {serious} patients ({serious/len(y_pred_proba):.1%})")
    print(f"Stable: {stable} patients ({stable/len(y_pred_proba):.1%})")
    
    print_subsection("8. Feature Importance")
    feature_importance = gb_health.feature_importances_
    importance_df = pd.DataFrame({
        'Feature': feature_names,
        'Importance': feature_importance
    }).sort_values('Importance', ascending=False)
    
    print("\nFeature Importance Ranking:")
    print(importance_df.to_string(index=False))
    
    print_subsection("9. Clinical Risk Factors")
    top_factors = importance_df.head(5)['Feature'].tolist()
    print(f"\nTop 5 Clinical Risk Factors:")
    for i, factor in enumerate(top_factors, 1):
        print(f"  {i}. {factor}")
    
    print_subsection("10. Model Sensitivity Analysis")
    thresholds = [0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9]
    
    sensitivity_results = []
    for threshold in thresholds:
        y_pred_threshold = (y_pred_proba >= threshold).astype(int)
        tp = ((y_pred_threshold == 1) & (y_test == 1)).sum()
        fn = ((y_pred_threshold == 0) & (y_test == 1)).sum()
        fp = ((y_pred_threshold == 1) & (y_test == 0)).sum()
        tn = ((y_pred_threshold == 0) & (y_test == 0)).sum()
        
        sensitivity = tp / (tp + fn) if (tp + fn) > 0 else 0
        specificity = tn / (tn + fp) if (tn + fp) > 0 else 0
        
        sensitivity_results.append({
            'Threshold': threshold,
            'True_Positive': tp,
            'False_Negative': fn,
            'False_Positive': fp,
            'True_Negative': tn,
            'Sensitivity': sensitivity,
            'Specificity': specificity
        })
    
    sensitivity_df = pd.DataFrame(sensitivity_results)
    print("\nThreshold Sensitivity Analysis:")
    print(sensitivity_df.to_string(index=False))
    
    print_subsection("11. Model Calibration")
    perfect_calibration = np.mean(y_pred_proba)
    print(f"\nMean Predicted Probability: {perfect_calibration:.4f}")
    print(f"Actual Positive Rate: {y_test.mean():.4f}")
    print(f"Calibration Error: {abs(perfect_calibration - y_test.mean()):.4f}")
    
    return {
        'model': gb_health,
        'accuracy': accuracy,
        'precision': precision,
        'recall': recall,
        'f1': f1,
        'roc_auc': roc_auc,
        'feature_importance': importance_df,
        'y_pred_proba': y_pred_proba,
        'confusion_matrix': cm
    }


def hyperparameter_tuning_strategies():
    """
    Demonstrate hyperparameter tuning strategies for gradient boosting.
    """
    print_section("HYPERPARAMETER TUNING STRATEGIES")
    
    X, y, feature_names = generate_classification_data(
        n_samples=500,
        n_features=10
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subsection("1. Learning Rate Impact")
    learning_rate_results = []
    for lr in [0.001, 0.01, 0.05, 0.1, 0.2, 0.5]:
        gb = GradientBoostingClassifier(
            n_estimators=100,
            learning_rate=lr,
            max_depth=3,
            random_state=42
        )
        gb.fit(X_train_scaled, y_train)
        y_pred = gb.predict(X_test_scaled)
        y_proba = gb.predict_proba(X_test_scaled)[:, 1]
        
        learning_rate_results.append({
            'Learning Rate': lr,
            'n_estimators': 100,
            'Accuracy': accuracy_score(y_test, y_pred),
            'ROC-AUC': roc_auc_score(y_test, y_proba)
        })
    
    lr_df = pd.DataFrame(learning_rate_results)
    print("\nLearning Rate Impact:")
    print(lr_df.to_string(index=False))
    
    print_subsection("2. n_estimators Impact (with fixed learning rate)")
    n_estimators_results = []
    for n_est in [10, 25, 50, 100, 150, 200, 300]:
        gb = GradientBoostingClassifier(
            n_estimators=n_est,
            learning_rate=0.1,
            max_depth=3,
            random_state=42
        )
        gb.fit(X_train_scaled, y_train)
        y_pred = gb.predict(X_test_scaled)
        y_proba = gb.predict_proba(X_test_scaled)[:, 1]
        
        n_estimators_results.append({
            'n_estimators': n_est,
            'Accuracy': accuracy_score(y_test, y_pred),
            'ROC-AUC': roc_auc_score(y_test, y_proba)
        })
    
    n_est_df = pd.DataFrame(n_estimators_results)
    print("\nn_estimators Impact:")
    print(n_est_df.to_string(index=False))
    
    print_subsection("3. Max Depth Impact")
    max_depth_results = []
    for depth in [1, 2, 3, 4, 5, 7, 10]:
        gb = GradientBoostingClassifier(
            n_estimators=50,
            learning_rate=0.1,
            max_depth=depth,
            random_state=42
        )
        gb.fit(X_train_scaled, y_train)
        y_pred = gb.predict(X_test_scaled)
        y_proba = gb.predict_proba(X_test_scaled)[:, 1]
        
        max_depth_results.append({
            'Max Depth': depth,
            'Accuracy': accuracy_score(y_test, y_pred),
            'ROC-AUC': roc_auc_score(y_test, y_proba)
        })
    
    depth_df = pd.DataFrame(max_depth_results)
    print("\nMax Depth Impact:")
    print(depth_df.to_string(index=False))
    
    print_subsection("4. Min Samples Split Impact")
    min_split_results = []
    for min_split in [2, 5, 10, 20, 50]:
        gb = GradientBoostingClassifier(
            n_estimators=50,
            learning_rate=0.1,
            max_depth=3,
            min_samples_split=min_split,
            random_state=42
        )
        gb.fit(X_train_scaled, y_train)
        y_pred = gb.predict(X_test_scaled)
        y_proba = gb.predict_proba(X_test_scaled)[:, 1]
        
        min_split_results.append({
            'Min Samples Split': min_split,
            'Accuracy': accuracy_score(y_test, y_pred),
            'ROC-AUC': roc_auc_score(y_test, y_proba)
        })
    
    min_split_df = pd.DataFrame(min_split_results)
    print("\nMin Samples Split Impact:")
    print(min_split_df.to_string(index=False))
    
    print_subsection("5. Min Samples Leaf Impact")
    min_leaf_results = []
    for min_leaf in [1, 2, 5, 10, 20]:
        gb = GradientBoostingClassifier(
            n_estimators=50,
            learning_rate=0.1,
            max_depth=3,
            min_samples_leaf=min_leaf,
            random_state=42
        )
        gb.fit(X_train_scaled, y_train)
        y_pred = gb.predict(X_test_scaled)
        y_proba = gb.predict_proba(X_test_scaled)[:, 1]
        
        min_leaf_results.append({
            'Min Samples Leaf': min_leaf,
            'Accuracy': accuracy_score(y_test, y_pred),
            'ROC-AUC': roc_auc_score(y_test, y_proba)
        })
    
    min_leaf_df = pd.DataFrame(min_leaf_results)
    print("\nMin Samples Leaf Impact:")
    print(min_leaf_df.to_string(index=False))
    
    print_subsection("6. Subsample Impact")
    subsample_results = []
    for subsample in [0.5, 0.6, 0.7, 0.8, 0.9, 1.0]:
        gb = GradientBoostingClassifier(
            n_estimators=50,
            learning_rate=0.1,
            max_depth=3,
            subsample=subsample,
            random_state=42
        )
        gb.fit(X_train_scaled, y_train)
        y_pred = gb.predict(X_test_scaled)
        y_proba = gb.predict_proba(X_test_scaled)[:, 1]
        
        subsample_results.append({
            'Subsample': subsample,
            'Accuracy': accuracy_score(y_test, y_pred),
            'ROC-AUC': roc_auc_score(y_test, y_proba)
        })
    
    subsample_df = pd.DataFrame(subsample_results)
    print("\nSubsample Impact:")
    print(subsample_df.to_string(index=False))
    
    print_subsection("7. Max Features Impact")
    max_features_results = []
    for max_features in [2, 3, 5, 7, 'sqrt', 'log2']:
        gb = GradientBoostingClassifier(
            n_estimators=50,
            learning_rate=0.1,
            max_depth=3,
            max_features=max_features,
            random_state=42
        )
        gb.fit(X_train_scaled, y_train)
        y_pred = gb.predict(X_test_scaled)
        y_proba = gb.predict_proba(X_test_scaled)[:, 1]
        
        max_features_results.append({
            'Max Features': str(max_features),
            'Accuracy': accuracy_score(y_test, y_pred),
            'ROC-AUC': roc_auc_score(y_test, y_proba)
        })
    
    max_features_df = pd.DataFrame(max_features_results)
    print("\nMax Features Impact:")
    print(max_features_df.to_string(index=False))
    
    print_subsection("8. Comprehensive Tuning Strategy")
    print("""
    Recommended Hyperparameter Tuning Strategy:
    
    1. Start with Default Parameters
       - Establish baseline performance
    
    2. Tune Learning Rate and n_estimators Together
       - Lower learning rate -> more estimators needed
       - Use early stopping to find optimal n_estimators
    
    3. Tune Tree Structure Parameters
       - max_depth: Control tree complexity
       - min_samples_split: Prevent overfitting
       - min_samples_leaf: Ensure leaf node robustness
    
    4. Tune Subsample
       - Add stochastic gradient boosting
       - Typically 0.8-0.9 works well
    
    5. Tune Feature Selection
       - max_features: Add randomness
       - sqrt and log2 are common choices
    
    6. Use Cross-Validation
       - Stratified K-Fold for classification
       - GridSearchCV or RandomizedSearchCV
    
    7. Use Early Stopping
       - Prevent overfitting
       - Reduce training time
    """)
    
    return {
        'learning_rate_results': lr_df,
        'n_estimators_results': n_est_df,
        'max_depth_results': depth_df
    }


def cross_validation_example():
    """
    Demonstrate cross-validation with gradient boosting.
    """
    print_section("CROSS-VALIDATION WITH GRADIENT BOOSTING")
    
    X, y, feature_names = generate_classification_data(
        n_samples=500,
        n_features=10
    )
    
    print_subsection("1. Stratified K-Fold Cross-Validation")
    gb = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    
    cv = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
    
    accuracy_scores = cross_val_score(gb, X, y, cv=cv, scoring='accuracy')
    f1_scores = cross_val_score(gb, X, y, cv=cv, scoring='f1')
    roc_auc_scores = cross_val_score(gb, X, y, cv=cv, scoring='roc_auc')
    precision_scores = cross_val_score(gb, X, y, cv=cv, scoring='precision')
    recall_scores = cross_val_score(gb, X, y, cv=cv, scoring='recall')
    
    print(f"\n5-Fold Cross-Validation Results:")
    print(f"Accuracy: {accuracy_scores.mean():.4f} (+/- {accuracy_scores.std()*2:.4f})")
    print(f"F1 Score: {f1_scores.mean():.4f} (+/- {f1_scores.std()*2:.4f})")
    print(f"ROC-AUC: {roc_auc_scores.mean():.4f} (+/- {roc_auc_scores.std()*2:.4f})")
    print(f"Precision: {precision_scores.mean():.4f} (+/- {precision_scores.std()*2:.4f})")
    print(f"Recall: {recall_scores.mean():.4f} (+/- {recall_scores.std()*2:.4f})")
    
    print_subsection("2. Fold-by-Fold Results")
    fold_results = []
    for fold, (train_idx, test_idx) in enumerate(cv.split(X, y), 1):
        X_train, X_test = X[train_idx], X[test_idx]
        y_train, y_test = y[train_idx], y[test_idx]
        
        gb_fold = GradientBoostingClassifier(
            n_estimators=50,
            learning_rate=0.1,
            max_depth=3,
            random_state=42
        )
        gb_fold.fit(X_train, y_train)
        y_pred = gb_fold.predict(X_test)
        y_proba = gb_fold.predict_proba(X_test)[:, 1]
        
        fold_results.append({
            'Fold': fold,
            'Train Size': len(train_idx),
            'Test Size': len(test_idx),
            'Accuracy': accuracy_score(y_test, y_pred),
            'ROC-AUC': roc_auc_score(y_test, y_proba)
        })
        print(f"Fold {fold}: Accuracy = {accuracy_score(y_test, y_pred):.4f}, ROC-AUC = {roc_auc_score(y_test, y_proba):.4f}")
    
    print_subsection("3. Leave-One-Out Cross-Validation")
    from sklearn.model_selection import LeaveOneOut
    
    loo = LeaveOneOut()
    
    print("\nNote: LOO CV is computationally expensive for large datasets")
    print(f"Total samples: {len(y)}")
    
    return {
        'accuracy_scores': accuracy_scores,
        'f1_scores': f1_scores,
        'roc_auc_scores': roc_auc_scores,
        'fold_results': fold_results
    }


def loss_functions_explanation():
    """
    Explain different loss functions in gradient boosting.
    """
    print_section("LOSS FUNCTIONS IN GRADIENT BOOSTING")
    
    X, y, feature_names = generate_classification_data(
        n_samples=500,
        n_features=8
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    print_subsection("1. Log Loss (Binomial Deviance)")
    print("""
    Log Loss is the default loss function for classification:
    
    L(y, p) = -[y * log(p) + (1-y) * log(1-p)]
    
    Properties:
    - Penalizes confident wrong predictions heavily
    - Provides probabilistic outputs
    - Smooth gradient for optimization
    - Used in logistic regression
    """)
    
    gb_log = GradientBoostingClassifier(
        loss='log_loss',
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb_log.fit(X_train, y_train)
    y_pred_log = gb_log.predict(X_test)
    y_proba_log = gb_log.predict_proba(X_test)[:, 1]
    
    print(f"Log Loss Accuracy: {accuracy_score(y_test, y_pred_log):.4f}")
    print(f"Log Loss ROC-AUC: {roc_auc_score(y_test, y_proba_log):.4f}")
    
    print_subsection("2. Exponential Loss")
    print("""
    Exponential Loss (same as AdaBoost):
    
    L(y, p) = exp(-y * p)
    
    Properties:
    - Similar to AdaBoost algorithm
    - Heavily penalizes misclassified samples
    - More sensitive to outliers
    - Can be less robust than log loss
    """)
    
    print("Note: scikit-learn doesn't expose exponential loss directly")
    print("for GradientBoostingClassifier, but it's used internally")
    print("in AdaBoost implementation")
    
    print_subsection("3. Hinge Loss")
    print("""
    Hinge Loss (used in SVM):
    
    L(y, p) = max(0, 1 - y * p)
    
    Properties:
    - Creates maximum-margin classifiers
    - Produces sparse solutions
    - Not differentiable at all points
    - Used in SVM and perceptron
    """)
    
    print("Note: Hinge loss is not available in GradientBoostingClassifier")
    print("but can be approximated with custom implementations")
    
    print_subsection("4. Loss Function Comparison Summary")
    print("""
    Summary of Loss Functions:
    ============================
    
    | Loss Function | Use Case | Robustness | Output |
    |----------------|----------|-----------|--------|
    | Log Loss       | General  | High      | Prob   |
    | Exponential    | Boosting | Medium    | Margin |
    | Hinge Loss     | SVM      | Low       | Class  |
    
    Recommendations:
    - Use log loss for most classification problems
    - Use exponential for boosting-specific tasks
    - Use hinge when maximum margin is needed
    """)
    
    return {
        'log_loss_accuracy': accuracy_score(y_test, y_pred_log),
        'log_loss_roc_auc': roc_auc_score(y_test, y_proba_log)
    }


def regularized_gradient_boosting():
    """
    Demonstrate regularization techniques in gradient boosting.
    """
    print_section("REGULARIZED GRADIENT BOOSTING")
    
    X, y, feature_names = generate_classification_data(
        n_samples=500,
        n_features=10
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subsection("1. L2 Regularization (via max_depth and min_samples)")
    gb_reg = GradientBoostingClassifier(
        n_estimators=100,
        learning_rate=0.05,
        max_depth=3,
        min_samples_split=10,
        min_samples_leaf=5,
        max_features='sqrt',
        random_state=42
    )
    gb_reg.fit(X_train_scaled, y_train)
    y_pred = gb_reg.predict(X_test_scaled)
    y_proba = gb_reg.predict_proba(X_test_scaled)[:, 1]
    
    print("L2 Regularization (tree constraints):")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print_subsection("2. Learning Rate Regularization (Shrinkage)")
    gb_shrinkage = GradientBoostingClassifier(
        n_estimators=200,
        learning_rate=0.01,
        max_depth=3,
        random_state=42
    )
    gb_shrinkage.fit(X_train_scaled, y_train)
    y_pred = gb_shrinkage.predict(X_test_scaled)
    y_proba = gb_shrinkage.predict_proba(X_test_scaled)[:, 1]
    
    print("Learning Rate (Shrinkage):")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print_subsection("3. Stochastic Regularization (Subsample)")
    gb_stochastic = GradientBoostingClassifier(
        n_estimators=100,
        learning_rate=0.1,
        max_depth=3,
        subsample=0.7,
        random_state=42
    )
    gb_stochastic.fit(X_train_scaled, y_train)
    y_pred = gb_stochastic.predict(X_test_scaled)
    y_proba = gb_stochastic.predict_proba(X_test_scaled)[:, 1]
    
    print("Subsample Regularization:")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print_subsection("4. Combined Regularization")
    gb_combined = GradientBoostingClassifier(
        n_estimators=150,
        learning_rate=0.02,
        max_depth=2,
        min_samples_split=15,
        min_samples_leaf=8,
        subsample=0.6,
        max_features='sqrt',
        random_state=42
    )
    gb_combined.fit(X_train_scaled, y_train)
    y_pred = gb_combined.predict(X_test_scaled)
    y_proba = gb_combined.predict_proba(X_test_scaled)[:, 1]
    
    print("Combined Regularization:")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print_subsection("5. Regularization Parameter Summary")
    print("""
    Regularization Techniques:
    ==========================
    
    1. Learning Rate (Shrinkage):
       - Lower values -> more trees needed
       - Typically 0.01-0.2
       
    2. Tree Complexity:
       - max_depth: Smaller trees
       - min_samples_split: Prevent splits
       - min_samples_leaf: Ensure leaf size
       
    3. Stochastic Gradient Boosting:
       - subsample: Use subset of data
       - Typically 0.5-0.9
       
    4. Feature Randomness:
       - max_features: Random subset
       - sqrt, log2, or float
       
    5. Early Stopping:
       - validation_fraction
       - n_iter_no_change
       
    Best Practice:
    - Combine multiple regularization techniques
    - Use cross-validation to find optimal parameters
    """)
    
    return {
        'accuracy': accuracy_score(y_test, y_pred),
        'roc_auc': roc_auc_score(y_test, y_proba)
    }


def imbalanced_data_handling():
    """
    Handle imbalanced data in gradient boosting.
    """
    print_section("HANDLING IMBALANCED DATA")
    
    X, y = generate_imbalanced_data(
        n_samples=1000,
        n_features=10,
        imbalance_ratio=0.1
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    print(f"Imbalance ratio: {y.mean():.2%}")
    
    print_subsection("1. Unbalanced Data Baseline")
    gb_unbalanced = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb_unbalanced.fit(X_train, y_train)
    y_pred = gb_unbalanced.predict(X_test)
    y_proba = gb_unbalanced.predict_proba(X_test)[:, 1]
    
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred, zero_division=0):.4f}")
    print(f"Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"F1: {f1_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print_subsection("2. Sample Weighting")
    class_weights = {0: 1.0, 1: 9.0}
    sample_weights = np.array([class_weights[label] for label in y_train])
    
    gb_weighted = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb_weighted.fit(X_train, y_train, sample_weight=sample_weights)
    y_pred = gb_weighted.predict(X_test)
    y_proba = gb_weighted.predict_proba(X_test)[:, 1]
    
    print("Sample Weighting (1:9):")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred, zero_division=0):.4f}")
    print(f"Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"F1: {f1_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print_subsection("3. Threshold Adjustment")
    gb_proba = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb_proba.fit(X_train, y_train)
    y_proba = gb_proba.predict_proba(X_test)[:, 1]
    
    threshold_results = []
    for threshold in [0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8]:
        y_pred_thresh = (y_proba >= threshold).astype(int)
        precision = precision_score(y_test, y_pred_thresh, zero_division=0)
        recall = recall_score(y_test, y_pred_thresh)
        f1 = f1_score(y_test, y_pred_thresh)
        
        threshold_results.append({
            'Threshold': threshold,
            'Precision': precision,
            'Recall': recall,
            'F1': f1
        })
    
    threshold_df = pd.DataFrame(threshold_results)
    print("\nThreshold Optimization:")
    print(threshold_df.to_string(index=False))
    
    print_subsection("4. SMOTE-like Oversampling")
    from sklearn.utils import resample
    
    X_majority = X_train[y_train == 0]
    X_minority = X_train[y_train == 1]
    y_majority = y_train[y_train == 0]
    y_minority = y_train[y_train == 1]
    
    X_minority_upsampled = resample(
        X_minority,
        replace=True,
        n_samples=len(X_majority),
        random_state=42
    )
    y_minority_upsampled = np.ones(len(X_minority_upsampled))
    
    X_upsampled = np.vstack([X_majority, X_minority_upsampled])
    y_upsampled = np.concatenate([y_majority, y_minority_upsampled])
    
    gb_upsampled = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb_upsampled.fit(X_upsampled, y_upsampled)
    y_pred = gb_upsampled.predict(X_test)
    y_proba = gb_upsampled.predict_proba(X_test)[:, 1]
    
    print("Oversampling:")
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred, zero_division=0):.4f}")
    print(f"Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"F1: {f1_score(y_test, y_pred):.4f}")
    print(f"ROC-AUC: {roc_auc_score(y_test, y_proba):.4f}")
    
    print_subsection("5. Handling Imbalanced Data Summary")
    print("""
    Strategies for Imbalanced Data:
    ================================
    
    1. Class Weights:
       - Use sample_weight parameter
       - Adjust for class distribution
       
    2. Threshold Adjustment:
       - Lower threshold for minority class
       - Optimize based on business requirements
       
    3. Resampling:
       - Oversample minority class
       - Undersample majority class
       - SMOTE and variants
       
    4. Evaluation Metrics:
       - Use ROC-AUC over accuracy
       - Focus on precision/recall trade-off
       - Consider business cost
       
    5. Ensemble Methods:
       - BalancedRandomForestClassifier
       - EasyEnsemble
       - RUSBoost
    """)
    
    return {
        'balanced_accuracy': accuracy_score(y_test, y_pred),
        'balanced_f1': f1_score(y_test, y_pred)
    }


def model_interpretability():
    """
    Demonstrate feature importance and model interpretability.
    """
    print_section("MODEL INTERPRETABILITY")
    
    X, y, feature_names = generate_classification_data(
        n_samples=500,
        n_features=10
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    gb = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb.fit(X_train, y_train)
    
    print_subsection("1. Feature Importance (Gini Importance)")
    feature_importance = gb.feature_importances_
    importance_df = pd.DataFrame({
        'Feature': feature_names,
        'Importance': feature_importance,
        'Importance_Pct': feature_importance * 100
    }).sort_values('Importance', ascending=False)
    
    print("\nFeature Importance:")
    print(importance_df.to_string(index=False))
    
    print_subsection("2. Permutation Importance")
    from sklearn.inspection import permutation_importance
    
    perm_importance = permutation_importance(
        gb, X_test, y_test, 
        n_repeats=10, 
        random_state=42
    )
    
    perm_importance_df = pd.DataFrame({
        'Feature': feature_names,
        'Importance_Mean': perm_importance.importances_mean,
        'Importance_Std': perm_importance.importances_std
    }).sort_values('Importance_Mean', ascending=False)
    
    print("\nPermutation Importance:")
    print(perm_importance_df.head().to_string(index=False))
    
    print_subsection("3. SHAP-like Analysis (Partial Dependence)")
    from sklearn.inspection import PartialDependenceDisplay
    
    print("\nNote: SHAP requires shap library")
    print("Partial Dependence plots show marginal effects")
    
    print_subsection("4. Single Prediction Explanation")
    sample_idx = 0
    sample = X_test[sample_idx:sample_idx+1]
    prediction = gb.predict(sample)[0]
    probability = gb.predict_proba(sample)[0]
    
    print(f"\nExplanation for Single Prediction:")
    print(f"Sample Index: {sample_idx}")
    print(f"Prediction: {prediction}")
    print(f"Probability Class 0: {probability[0]:.4f}")
    print(f"Probability Class 1: {probability[1]:.4f}")
    
    print("\nContributing Features for this Prediction:")
    sample_importance = []
    for i, (feature, importance) in enumerate(zip(feature_names, feature_importance)):
        sample_importance.append({
            'Feature': feature,
            'Importance': importance,
            'Value': X_test[sample_idx, i]
        })
    
    sample_df = pd.DataFrame(sample_importance).sort_values('Importance', ascending=False)
    print(sample_df.head().to_string(index=False))
    
    print_subsection("5. Interpretability Summary")
    print("""
    Model Interpretability Methods:
    ================================
    
    1. Feature Importance:
       - Gini/Mean Decrease Impurity
       - Easy to compute
       
    2. Permutation Importance:
       - Model-agnostic
       - Considers feature interactions
       
    3. SHAP Values:
       - Game-theoretic interpretation
       - Accurate feature attribution
       
    4. Partial Dependence:
       - Marginal effects
       - Easy to visualize
       
    5. Individual Predictions:
       - LIME explanations
       - Counterfactual analysis
       
    Best Practice:
    - Use multiple interpretability methods
    - Focus on business-meaningful features
    - Validate with domain experts
    """)
    
    return importance_df


def advanced_topics():
    """
    Cover advanced topics in gradient boosting.
    """
    print_section("ADVANCED TOPICS")
    
    X, y, feature_names = generate_classification_data(
        n_samples=500,
        n_features=10
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print_subsection("1. Multi-Class Gradient Boosting")
    X_multi, y_multi = generate_multiclass_data(
        n_samples=600,
        n_features=8,
        n_classes=3
    )
    
    X_train_multi, X_test_multi, y_train_multi, y_test_multi = train_test_split(
        X_multi, y_multi, test_size=0.2, random_state=42, stratify=y_multi
    )
    
    gb_multi = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb_multi.fit(X_train_multi, y_train_multi)
    y_pred_multi = gb_multi.predict(X_test_multi)
    
    print("Multi-class Classification (3 classes):")
    print(f"Accuracy: {accuracy_score(y_test_multi, y_pred_multi):.4f}")
    print(f"Classification Report:")
    print(classification_report(y_test_multi, y_pred_multi))
    
    print_subsection("2. Staged Predictions (Learning Curve)")
    print("\nStaged Predictions (First 10 stages):")
    gb_staged = GradientBoostingClassifier(
        n_estimators=100,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb_staged.fit(X_train_scaled, y_train)
    
    for i, (train_pred, test_pred) in enumerate(zip(
        gb_staged.staged_predict(X_train_scaled),
        gb_staged.staged_predict(X_test_scaled)
    )):
        if i < 10:
            train_acc = accuracy_score(y_train, train_pred)
            test_acc = accuracy_score(y_test, test_pred)
            print(f"Stage {i+1}: Train Acc = {train_acc:.4f}, Test Acc = {test_acc:.4f}")
    
    print_subsection("3. Probability Calibration")
    from sklearn.calibration import CalibratedClassifierCV
    
    gb_uncalibrated = GradientBoostingClassifier(
        n_estimators=50,
        learning_rate=0.1,
        max_depth=3,
        random_state=42
    )
    gb_uncalibrated.fit(X_train_scaled, y_train)
    
    gb_calibrated = CalibratedClassifierCV(
        GradientBoostingClassifier(
            n_estimators=50,
            learning_rate=0.1,
            max_depth=3,
            random_state=42
        ),
        method='isotonic',
        cv=3
    )
    gb_calibrated.fit(X_train_scaled, y_train)
    
    y_proba_uncalibrated = gb_uncalibrated.predict_proba(X_test_scaled)[:, 1]
    y_proba_calibrated = gb_calibrated.predict_proba(X_test_scaled)[:, 1]
    
    print("Probability Calibration:")
    print(f"Mean Uncalibrated: {y_proba_uncalibrated.mean():.4f}")
    print(f"Mean Calibrated: {y_proba_calibrated.mean():.4f}")
    print(f"Actual Positive Rate: {y_test.mean():.4f}")
    
    print_subsection("4. Warm Start (Incremental Learning)")
    print("\nWarm Start - Incrementally Adding Estimators:")
    
    gb_warm = GradientBoostingClassifier(
        n_estimators=10,
        learning_rate=0.1,
        max_depth=3,
        random_state=42,
        warm_start=True
    )
    gb_warm.fit(X_train_scaled, y_train)
    
    print(f"Initial n_estimators: {gb_warm.n_estimators}")
    first_pred = gb_warm.predict(X_test_scaled)
    print(f"Initial accuracy: {accuracy_score(y_test, first_pred):.4f}")
    
    gb_warm.n_estimators = 30
    gb_warm.fit(X_train_scaled, y_train)
    
    print(f"After warm start: {gb_warm.n_estimators}")
    warm_pred = gb_warm.predict(X_test_scaled)
    print(f"After warm accuracy: {accuracy_score(y_test, warm_pred):.4f}")
    
    print_subsection("5. Advanced Topics Summary")
    print("""
    Advanced Topics:
    ================
    
    1. Multi-class Classification:
       - One-vs-All strategy
       - Multi-output support
       
    2. Staged Predictions:
       - Track learning progress
       - Early stopping support
       
    3. Probability Calibration:
       - Isotonic regression
       - Sigmoid calibration
       
    4. Warm Start:
       - Incremental learning
       - Find optimal n_estimators
       
    5. Other Boosting Libraries:
       - XGBoost: Performance optimized
       - LightGBM: Large-scale data
       - CatBoost: Categorical features
    """)
    
    return {
        'multi_class_accuracy': accuracy_score(y_test_multi, y_pred_multi),
        'calibration_improvement': abs(y_proba_calibrated.mean() - y_test.mean()) < abs(y_proba_uncalibrated.mean() - y_test.mean())
    }


def testing_functions():
    """
    Testing functions for gradient boosting implementation.
    """
    print_section("TESTING FUNCTIONS")
    
    print_subsection("1. Unit Tests - Data Generation")
    X, y, feature_names = generate_classification_data(n_samples=100)
    assert X.shape[0] == 100
    assert len(y) == 100
    assert len(feature_names) == X.shape[1]
    print("Data generation tests: PASSED")
    
    print_subsection("2. Unit Tests - Model Training")
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2)
    gb = GradientBoostingClassifier(n_estimators=10)
    gb.fit(X_train, y_train)
    y_pred = gb.predict(X_test)
    assert len(y_pred) == len(y_test)
    print("Model training tests: PASSED")
    
    print_subsection("3. Unit Tests - Predictions")
    y_pred_proba = gb.predict_proba(X_test)
    assert y_pred_proba.shape[1] == 2
    assert np.allclose(y_pred_proba.sum(axis=1), 1.0)
    print("Prediction tests: PASSED")
    
    print_subsection("4. Unit Tests - Cross-Validation")
    X, y, _ = generate_classification_data(n_samples=200)
    cv = StratifiedKFold(n_splits=3)
    scores = cross_val_score(
        GradientBoostingClassifier(n_estimators=10),
        X, y, cv=cv, scoring='accuracy'
    )
    assert len(scores) == 3
    assert all(0 <= s <= 1 for s in scores)
    print("Cross-validation tests: PASSED")
    
    print_subsection("5. Integration Tests - Banking Example")
    bank_result = banking_example()
    assert 'model' in bank_result
    assert 'accuracy' in bank_result
    assert 0 <= bank_result['accuracy'] <= 1
    print("Banking example tests: PASSED")
    
    print_subsection("6. Integration Tests - Healthcare Example")
    health_result = healthcare_example()
    assert 'model' in health_result
    assert 'accuracy' in health_result
    assert 0 <= health_result['accuracy'] <= 1
    print("Healthcare example tests: PASSED")
    
    print_subsection("7. Performance Tests")
    import time
    
    X, y, _ = generate_classification_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(X, y)
    
    start_time = time.time()
    gb = GradientBoostingClassifier(n_estimators=100)
    gb.fit(X_train, y_train)
    train_time = time.time() - start_time
    
    print(f"Training time for 500 samples: {train_time:.4f} seconds")
    assert train_time < 10
    print("Performance tests: PASSED")
    
    print("\nAll tests completed successfully!")
    
    return {
        'passed': True,
        'tests_run': 7
    }


def print_results_summary():
    """
    Print comprehensive summary of all results.
    """
    print_section("RESULTS SUMMARY")
    
    print("""
    =========================================================================
                          GRADIENT BOOSTING CLASSIFICATION
                              RESULTS SUMMARY
    =========================================================================
    
    Implementation Components:
    ---------------------------
    1. Core Implementation
       - Custom GradientBoostingImplementation class
       - Sequential learning algorithm
       - Multiple loss function support
    
    2. Hyperparameter Impact
       - Learning Rate: Key regularization parameter
       - n_estimators: Number of boosting stages
       - max_depth: Tree complexity control
       - subsample: Stochastic gradient boosting
    
    3. Banking Example
       - Credit risk assessment model
       - 10 financial features
       - Business metrics calculation
    
    4. Healthcare Example
       - Patient outcome prediction
       - Medical feature importance
       - Clinical triage system
    
    5. Comparison Results
       - vs Random Forest
       - vs AdaBoost
       - vs Logistic Regression
    
    Key Findings:
    -----------
    - Gradient Boosting achieves high accuracy on synthetic data
    - Learning rate and n_estimators are critical parameters
    - Feature importance provides interpretability
    - Subsample regularization improves generalization
    - Early stopping prevents overfitting
    
    Best Practices:
    ---------------
    1. Start with default parameters
    2. Use cross-validation for tuning
    3. Monitor early stopping
    4. Analyze feature importance
    5. Combine regularization techniques
    
    Future Directions:
    ------------------
    1. XGBoost and LightGBM integration
    2. SHAP for model interpretability
    3. Distributed computing
    4. AutoML integration
    5. Deep learning comparison
    
    =========================================================================
    """)


def main():
    """
    Main function to execute all gradient boosting implementations.
    """
    print("\n" + "=" * 80)
    print("   GRADIENT BOOSTING CLASSIFICATION - COMPREHENSIVE IMPLEMENTATION")
    print("=" * 80)
    
    print("\nExecuting Core Gradient Boosting Implementation...")
    core_result = core_gradient_boosting_implementation()
    
    print("\nExecuting Boosting Comparison...")
    comparison_result = boosting_comparison()
    
    print("\nExecuting Early Stopping Example...")
    early_stopping_result = early_stopping_example()
    
    print("\nExecuting Banking Example...")
    banking_result = banking_example()
    
    print("\nExecuting Healthcare Example...")
    healthcare_result = healthcare_example()
    
    print("\nExecuting Hyperparameter Tuning...")
    tuning_result = hyperparameter_tuning_strategies()
    
    print("\nExecuting Cross-Validation...")
    cv_result = cross_validation_example()
    
    print("\nExecuting Loss Functions...")
    loss_result = loss_functions_explanation()
    
    print("\nExecuting Regularized Gradient Boosting...")
    regularized_result = regularized_gradient_boosting()
    
    print("\nExecuting Imbalanced Data Handling...")
    imbalanced_result = imbalanced_data_handling()
    
    print("\nExecuting Model Interpretability...")
    interpretability_result = model_interpretability()
    
    print("\nExecuting Advanced Topics...")
    advanced_result = advanced_topics()
    
    print("\nExecuting Testing Functions...")
    testing_result = testing_functions()
    
    print_results_summary()
    
    print("\n" + "=" * 80)
    print("   EXECUTION COMPLETED SUCCESSFULLY")
    print("=" * 80)


if __name__ == "__main__":
    main()