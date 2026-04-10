# Topic: Decision Tree Classification
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Decision Tree Classification

I. INTRODUCTION
    Decision Tree Classification is a supervised learning algorithm that builds
    a tree-like model of decisions based on feature values. Each internal node
    represents a test on a feature, each branch represents an outcome,
    and each leaf node represents a class label. Decision trees are
    interpretable, handle both numerical and categorical data, and
    can capture non-linear relationships.

II. CORE_CONCEPTS
    - Tree structure: root, internal nodes, leaves
    - Splitting criteria: Gini impurity, entropy (information gain)
    - Pruning: pre-pruning and post-pruning
    - Hyperparameters: max_depth, min_samples_split, min_samples_leaf
    - Feature importance: measures how much each feature contributes
    - Handling overfitting: depth control, pruning strategies

III. IMPLEMENTATION
    - Building decision trees with different criteria
    - Visualization of tree structure
    - Feature importance analysis
    - Hyperparameter tuning
    - Handling categorical variables

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Loan Approval Classification
    - Healthcare: Disease Diagnosis Classification

V. OUTPUT_RESULTS
    - Tree visualization
    - Classification metrics (accuracy, precision, recall, F1)
    - Feature importance rankings
    - Confusion matrices

VI. TESTING
    - Synthetic classification data
    - Real-world scenario tests

VII. ADVANCED_TOPICS
    - Ensemble methods (Random Forest, Gradient Boosting)
    - Cost-complexity pruning
    - Multi-output classification

VIII. CONCLUSION
    - When to use decision trees
    - Advantages and limitations
    - Best practices
"""

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_classification, make_blobs
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.tree import DecisionTreeClassifier, plot_tree, export_text
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    confusion_matrix, classification_report, roc_curve, auc
)
from sklearn.preprocessing import LabelEncoder
import warnings
warnings.filterwarnings('ignore')


def generate_classification_data(n_samples=500, n_features=4, n_informative=3, random_state=42):
    """
    Generate synthetic classification data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples
    n_features : int
        Number of features
    n_informative : int
        Number of informative features
    random_state : int
        Random seed
    
    Returns:
    --------
    X : ndarray
        Features
    y : ndarray
        Class labels
    feature_names : list
        Names of features
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=0,
        n_classes=2,
        random_state=random_state
    )
    
    feature_names = [f'Feature_{i}' for i in range(n_features)]
    
    print(f"Generated {n_samples} samples with {n_features} features")
    print(f"X shape: {X.shape}, y shape: {y.shape}")
    print(f"Class distribution: {np.bincount(y)}")
    
    return X, y, feature_names


def generate_multiclass_data(n_samples=500, n_classes=3, n_features=4, random_state=42):
    """
    Generate multi-class classification data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples
    n_classes : int
        Number of classes
    n_features : int
        Number of features
    random_state : int
        Random seed
    
    Returns:
    --------
    X : ndarray
        Features
    y : ndarray
        Class labels
    feature_names : list
        Names of features
    """
    X, y = make_blobs(
        n_samples=n_samples,
        centers=n_classes,
        n_features=n_features,
        random_state=random_state
    )
    
    feature_names = [f'Feature_{i}' for i in range(n_features)]
    
    print(f"Generated {n_samples} samples with {n_classes} classes")
    print(f"Class distribution: {np.bincount(y)}")
    
    return X, y, feature_names


def core_decision_tree(X_train, X_test, y_train, y_test, criterion='gini', max_depth=None):
    """
    Core decision tree classification implementation.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test labels
    criterion : str
        Splitting criterion ('gini' or 'entropy')
    max_depth : int
        Maximum depth of tree
    
    Returns:
    --------
    model : DecisionTreeClassifier
        Trained model
    metrics : dict
        Performance metrics
    """
    print(f"\n{'='*60}")
    print(f"CORE DECISION TREE CLASSIFICATION")
    print(f"  Criterion: {criterion}, Max Depth: {max_depth}")
    print(f"{'='*60}")
    
    model = DecisionTreeClassifier(
        criterion=criterion,
        max_depth=max_depth,
        random_state=42
    )
    
    model.fit(X_train, y_train)
    
    y_pred_train = model.predict(X_train)
    y_pred_test = model.predict(X_test)
    
    train_accuracy = accuracy_score(y_train, y_pred_train)
    test_accuracy = accuracy_score(y_test, y_pred_test)
    test_precision = precision_score(y_test, y_pred_test, average='weighted')
    test_recall = recall_score(y_test, y_pred_test, average='weighted')
    test_f1 = f1_score(y_test, y_pred_test, average='weighted')
    
    cm = confusion_matrix(y_test, y_pred_test)
    
    metrics = {
        'train_accuracy': train_accuracy,
        'test_accuracy': test_accuracy,
        'test_precision': test_precision,
        'test_recall': test_recall,
        'test_f1': test_f1,
        'confusion_matrix': cm,
        'y_pred_test': y_pred_test,
        'feature_importance': model.feature_importances_
    }
    
    print(f"\nModel Performance:")
    print(f"  Training Accuracy: {train_accuracy:.4f}")
    print(f"  Test Accuracy: {test_accuracy:.4f}")
    print(f"  Test Precision: {test_precision:.4f}")
    print(f"  Test Recall: {test_recall:.4f}")
    print(f"  Test F1 Score: {test_f1:.4f}")
    print(f"\nConfusion Matrix:")
    print(f"  {cm}")
    print(f"\nTree Statistics:")
    print(f"  Tree Depth: {model.get_depth()}")
    print(f"  Number of Leaves: {model.get_n_leaves()}")
    
    return model, metrics


def compare_criteria(X_train, X_test, y_train, y_test, feature_names):
    """
    Compare Gini and Entropy splitting criteria.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test labels
    feature_names : list
        Names of features
    
    Returns:
    --------
    results : dict
        Results for each criterion
    """
    print(f"\n{'='*60}")
    print(f"COMPARING SPLITTING CRITERIA")
    print(f"{'='*60}")
    
    criteria = ['gini', 'entropy']
    results = {}
    
    for criterion in criteria:
        print(f"\n--- Criterion: {criterion} ---")
        
        model, metrics = core_decision_tree(
            X_train, X_test, y_train, y_test,
            criterion=criterion, max_depth=5
        )
        
        results[criterion] = metrics
        
        importance_df = pd.DataFrame({
            'Feature': feature_names,
            'Importance': model.feature_importances_
        }).sort_values('Importance', ascending=False)
        
        print(f"\nFeature Importance:")
        for _, row in importance_df.iterrows():
            if row['Importance'] > 0:
                print(f"  {row['Feature']}: {row['Importance']:.4f}")
    
    return results


def compare_max_depths(X_train, X_test, y_train, y_test, depths=[1, 2, 3, 4, 5, 10, None]):
    """
    Compare different maximum depths.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test labels
    depths : list
        Max depths to compare
    
    Returns:
    --------
    results : dict
        Results for each depth
    """
    print(f"\n{'='*60}")
    print(f"COMPARING MAX DEPTHS")
    print(f"{'='*60}")
    
    results = {}
    
    for depth in depths:
        model = DecisionTreeClassifier(max_depth=depth, random_state=42)
        model.fit(X_train, y_train)
        
        train_accuracy = accuracy_score(y_train, model.predict(X_train))
        test_accuracy = accuracy_score(y_test, model.predict(X_test))
        
        key = str(depth) if depth is not None else 'None'
        results[key] = {
            'train_accuracy': train_accuracy,
            'test_accuracy': test_accuracy,
            'depth': model.get_depth(),
            'n_leaves': model.get_n_leaves()
        }
        
        print(f"Max Depth {key}: Train Acc={train_accuracy:.4f}, Test Acc={test_accuracy:.4f}, "
              f"Actual Depth={model.get_depth()}, Leaves={model.get_n_leaves()}")
    
    visualize_depth_comparison(results)
    
    return results


def visualize_depth_comparison(results):
    """
    Visualize accuracy vs max depth.
    
    Parameters:
    -----------
    results : dict
        Results from depth comparison
    """
    plt.figure(figsize=(10, 6))
    
    depths = list(results.keys())
    train_acc = [results[d]['train_accuracy'] for d in depths]
    test_acc = [results[d]['test_accuracy'] for d in depths]
    
    x = range(len(depths))
    plt.plot(x, train_acc, marker='o', label='Train Accuracy')
    plt.plot(x, test_acc, marker='s', label='Test Accuracy')
    
    plt.xlabel('Max Depth')
    plt.ylabel('Accuracy')
    plt.title('Train vs Test Accuracy by Max Depth')
    plt.xticks(x, depths)
    plt.legend()
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.show()


def visualize_tree(model, feature_names, class_names=None):
    """
    Visualize decision tree structure.
    
    Parameters:
    -----------
    model : DecisionTreeClassifier
        Trained model
    feature_names : list
        Names of features
    class_names : list
        Names of classes
    """
    plt.figure(figsize=(20, 10))
    
    if class_names is None:
        class_names = [f'Class {i}' for i in range(model.n_classes_)]
    
    plot_tree(
        model,
        feature_names=feature_names,
        class_names=class_names,
        filled=True,
        rounded=True,
        fontsize=8
    )
    plt.title('Decision Tree Visualization')
    plt.tight_layout()
    plt.show()
    
    tree_rules = export_text(model, feature_names=feature_names)
    print(f"\nDecision Tree Rules:")
    print(tree_rules)


def analyze_feature_importance(model, feature_names):
    """
    Analyze and visualize feature importance.
    
    Parameters:
    -----------
    model : DecisionTreeClassifier
        Trained model
    feature_names : list
        Names of features
    """
    importance = model.feature_importances_
    
    importance_df = pd.DataFrame({
        'Feature': feature_names,
        'Importance': importance
    }).sort_values('Importance', ascending=True)
    
    plt.figure(figsize=(10, 6))
    plt.barh(importance_df['Feature'], importance_df['Importance'])
    plt.xlabel('Importance')
    plt.ylabel('Feature')
    plt.title('Feature Importance')
    plt.tight_layout()
    plt.show()
    
    print(f"\nFeature Importance Ranking:")
    for _, row in importance_df.iloc[::-1].iterrows():
        print(f"  {row['Feature']}: {row['Importance']:.4f}")


def banking_example():
    """
    Banking/Finance example: Loan Approval Classification.
    
    Decision tree for predicting whether a loan should be approved
    based on applicant financial profile.
    """
    print(f"\n{'='*60}")
    print(f"BANKING EXAMPLE: Loan Approval Classification")
    print(f"{'='*60}")
    
    np.random.seed(42)
    n_samples = 500
    
    income = np.random.uniform(25000, 200000, n_samples)
    credit_score = np.random.uniform(500, 850, n_samples)
    employment_years = np.random.uniform(0, 30, n_samples)
    existing_debt = np.random.uniform(0, 100000, n_samples)
    loan_amount = np.random.uniform(1000, 50000, n_samples)
    existing_loans = np.random.randint(0, 5, n_samples)
    on_time_payments_ratio = np.random.uniform(0, 1, n_samples)
    
    debt_to_income_ratio = existing_debt / income
    loan_to_income_ratio = loan_amount / income
    
    approve = np.zeros(n_samples)
    for i in range(n_samples):
        score = 0
        if credit_score > 700:
            score += 2
        if credit_score > 800:
            score += 2
        if income > 50000:
            score += 1
        if income > 100000:
            score += 2
        if employment_years > 2:
            score += 1
        if debt_to_income_ratio < 0.3:
            score += 2
        elif debt_to_income_ratio < 0.5:
            score += 1
        if on_time_payments_ratio > 0.9:
            score += 2
        if existing_loans < 2:
            score += 1
        if loan_to_income_ratio < 0.5:
            score += 1
        
        if score >= 8:
            approve[i] = 1
        elif score >= 5 and np.random.random() > 0.3:
            approve[i] = 1
        else:
            approve[i] = 0
    
    df = pd.DataFrame({
        'Income': income,
        'Credit_Score': credit_score,
        'Employment_Years': employment_years,
        'Existing_Debt': existing_debt,
        'Loan_Amount': loan_amount,
        'Existing_Loans': existing_loans,
        'On_Time_Payments_Ratio': on_time_payments_ratio,
        'Approved': approve
    })
    
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    feature_cols = ['Income', 'Credit_Score', 'Employment_Years', 'Existing_Debt',
                   'Loan_Amount', 'Existing_Loans', 'On_Time_Payments_Ratio']
    X = df[feature_cols].values
    y = df['Approved'].values
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    print(f"\nTraining samples: {len(X_train)}, Test samples: {len(X_test)}")
    print(f"Class distribution (train): {np.bincount(y_train)}")
    print(f"Class distribution (test): {np.bincount(y_test)}")
    
    model, metrics = core_decision_tree(
        X_train, X_test, y_train, y_test,
        criterion='gini', max_depth=5
    )
    
    visualize_tree(model, feature_cols, ['Rejected', 'Approved'])
    analyze_feature_importance(model, feature_cols)
    
    tree_rules = export_text(model, feature_names=feature_cols)
    print(f"\nLoan Approval Decision Rules:")
    print(tree_rules)
    
    return model, metrics


def healthcare_example():
    """
    Healthcare example: Disease Diagnosis Classification.
    
    Decision tree for predicting disease presence based on
    patient symptoms and test results.
    """
    print(f"\n{'='*60}")
    print(f"HEALTHCARE EXAMPLE: Disease Diagnosis Classification")
    print(f"{'='*60}")
    
    np.random.seed(123)
    n_samples = 500
    
    fever = np.random.uniform(36.0, 40.0, n_samples)
    cough = np.random.randint(0, 2, n_samples)
    fatigue = np.random.randint(0, 2, n_samples)
    headache = np.random.randint(0, 2, n_samples)
    body_aches = np.random.randint(0, 2, n_samples)
    sore_throat = np.random.randint(0, 2, n_samples)
    shortness_breath = np.random.randint(0, 2, n_samples)
    oxygen_levels = np.random.uniform(85, 100, n_samples)
    heart_rate = np.random.uniform(60, 120, n_samples)
    white_blood_cell_count = np.random.uniform(4000, 15000, n_samples)
    
    has_disease = np.zeros(n_samples)
    for i in range(n_samples):
        score = 0
        if fever[i] > 38.5:
            score += 2
        if cough[i] == 1:
            score += 1
        if fatigue[i] == 1:
            score += 1
        if headache[i] == 1:
            score += 1
        if body_aches[i] == 1:
            score += 1
        if sore_throat[i] == 1:
            score += 1
        if shortness_breath[i] == 1:
            score += 2
        if oxygen_levels[i] < 92:
            score += 3
        elif oxygen_levels[i] < 96:
            score += 1
        if heart_rate[i] > 100:
            score += 1
        if white_blood_cell_count[i] > 11000:
            score += 2
        elif white_blood_cell_count[i] > 9000:
            score += 1
        
        if score >= 7:
            has_disease[i] = 1
        elif score >= 4 and np.random.random() > 0.4:
            has_disease[i] = 1
        else:
            has_disease[i] = 0
    
    df = pd.DataFrame({
        'Fever': fever,
        'Cough': cough,
        'Fatigue': fatigue,
        'Headache': headache,
        'Body_Aches': body_aches,
        'Sore_Throat': sore_throat,
        'Shortness_Breath': shortness_breath,
        'Oxygen_Levels': oxygen_levels,
        'Heart_Rate': heart_rate,
        'White_Blood_Cell_Count': white_blood_cell_count,
        'Has_Disease': has_disease
    })
    
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    feature_cols = ['Fever', 'Cough', 'Fatigue', 'Headache', 'Body_Aches',
                   'Sore_Throat', 'Shortness_Breath', 'Oxygen_Levels',
                   'Heart_Rate', 'White_Blood_Cell_Count']
    X = df[feature_cols].values
    y = df['Has_Disease'].values
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    print(f"\nTraining samples: {len(X_train)}, Test samples: {len(X_test)}")
    print(f"Class distribution (train): {np.bincount(y_train)}")
    print(f"Class distribution (test): {np.bincount(y_test)}")
    
    results = compare_max_depths(X_train, X_test, y_train, y_test)
    
    model, metrics = core_decision_tree(
        X_train, X_test, y_train, y_test,
        criterion='entropy', max_depth=5
    )
    
    visualize_tree(model, feature_cols, ['Healthy', 'Disease'])
    analyze_feature_importance(model, feature_cols)
    
    return model, metrics


def test_decision_trees():
    """
    Test decision tree models with various configurations.
    """
    print(f"\n{'='*60}")
    print(f"TESTING DECISION TREE MODELS")
    print(f"{'='*60}")
    
    X, y, feature_names = generate_classification_data(n_samples=300)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    compare_criteria(X_train, X_test, y_train, y_test, feature_names)
    compare_max_depths(X_train, X_test, y_train, y_test)
    
    print(f"\n{'='*60}")
    print(f"ALL TESTS COMPLETED SUCCESSFULLY")
    print(f"{'='*60}")
    
    return True


def main():
    """
    Main function to execute decision tree classification examples.
    """
    print("="*60)
    print("DECISION TREE CLASSIFICATION IMPLEMENTATION")
    print("="*60)
    
    print("\nI. INTRODUCTION")
    print("   Decision trees build a tree-like model of decisions")
    print("   based on feature values for classification.")
    
    print("\nII. CORE_CONCEPTS")
    print("   - Tree structure: root, internal nodes, leaves")
    print("   - Splitting: Gini impurity, entropy")
    print("   - Pruning: control max_depth, min_samples")
    print("   - Feature importance")
    
    print("\nIII. IMPLEMENTATION")
    
    X, y, feature_names = generate_classification_data(n_samples=300)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    model, metrics = core_decision_tree(X_train, X_test, y_train, y_test)
    visualize_tree(model, feature_names)
    
    print("\nIV. EXAMPLES")
    banking_model, banking_metrics = banking_example()
    healthcare_model, healthcare_metrics = healthcare_example()
    
    print("\nV. OUTPUT_RESULTS")
    print("   - Tree visualization and rules")
    print("   - Classification metrics")
    print("   - Feature importance")
    
    print("\nVI. TESTING")
    test_decision_trees()
    
    print("\nVII. ADVANCED_TOPICS")
    print("   - Random Forest (ensemble of trees)")
    print("   - Gradient Boosting")
    print("   - Cost-complexity pruning")
    
    print("\nVIII. CONCLUSION")
    print("   - Decision trees are interpretable and easy to visualize")
    print("   - Handle both numerical and categorical features")
    print("   - Prone to overfitting - use depth control and pruning")
    print("   - Good for feature importance analysis")
    print("   - Base for ensemble methods like Random Forest")
    print("\n   Implementation complete!")


if __name__ == "__main__":
    main()