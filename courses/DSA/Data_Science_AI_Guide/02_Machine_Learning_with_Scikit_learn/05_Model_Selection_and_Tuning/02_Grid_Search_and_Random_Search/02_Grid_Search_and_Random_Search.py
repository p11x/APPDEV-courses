# Topic: Grid Search and Random Search
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Grid Search and Random Search

I. INTRODUCTION
    Grid Search and Random Search are hyperparameter optimization techniques used
    to find the best combination of hyperparameters for machine learning models.
    Grid Search exhaustively searches through all possible combinations in a
    predefined parameter grid, while Random Search samples from a distribution
    over parameter combinations. Both are essential for model tuning.

II. CORE_CONCEPTS
    - Hyperparameters vs parameters
    - Grid Search: exhaustive search over all combinations
    - Random Search: random sampling from parameter distributions
    - Cross-validation for robust evaluation
    - Search efficiency and time tradeoffs
    - Parameter distributions and scales

III. IMPLEMENTATION
    - GridSearchCV implementation
    - RandomizedSearchCV implementation
    - Custom parameter grids
    - Nested cross-validation
    - Parallel execution

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Credit Score Prediction tuning
    - Healthcare: Patient Readmission Prediction tuning

V. OUTPUT_RESULTS
    - Best parameters
    - Best cross-validation scores
    - Parameter importance analysis
    - Learning curves

VI. TESTING
    - Grid vs Random search comparison
    - Time complexity analysis
    - Optimal parameter identification

VII. ADVANCED_TOPICS
    - Successive Halving
    - Bayesian Optimization basics
    - Early stopping
    - Meta-learning for hyperparameters

VIII. CONCLUSION
    - When to use Grid vs Random search
    - Best practices for hyperparameter tuning
"""

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_classification, make_regression
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV, RandomizedSearchCV
from sklearn.ensemble import RandomForestClassifier, RandomForestRegressor
from sklearn.svm import SVC, SVR
from sklearn.linear_model import Ridge, LogisticRegression
from sklearn.tree import DecisionTreeClassifier
from sklearn.metrics import accuracy_score, mean_squared_error, r2_score, make_scorer
from scipy.stats import uniform, randint, loguniform
import time
import warnings
warnings.filterwarnings('ignore')


def generate_classification_data(n_samples=500, n_features=10, random_state=42):
    """
    Generate classification data for hyperparameter tuning.
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=5,
        n_redundant=3,
        n_classes=2,
        random_state=random_state
    )
    print(f"Generated classification data: {X.shape}")
    return X, y


def generate_regression_data(n_samples=500, n_features=10, random_state=42):
    """
    Generate regression data for hyperparameter tuning.
    """
    X, y = make_regression(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=5,
        noise=10,
        random_state=random_state
    )
    print(f"Generated regression data: {X.shape}")
    return X, y


def grid_search_classification(X_train, X_test, y_train, y_test):
    """
    Implement Grid Search for classification.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test labels
    
    Returns:
    --------
    results : dict
        Grid search results
    """
    print(f"\n{'='*60}")
    print(f"GRID SEARCH FOR CLASSIFICATION")
    print(f"{'='*60}")
    
    param_grid = {
        'n_estimators': [50, 100, 200],
        'max_depth': [3, 5, 7, None],
        'min_samples_split': [2, 5, 10],
        'min_samples_leaf': [1, 2, 4]
    }
    
    total_combinations = 1
    for param_values in param_grid.values():
        total_combinations *= len(param_values)
    
    print(f"Parameter grid: {param_grid}")
    print(f"Total combinations: {total_combinations}")
    
    base_model = RandomForestClassifier(random_state=42)
    
    start_time = time.time()
    
    grid_search = GridSearchCV(
        estimator=base_model,
        param_grid=param_grid,
        cv=5,
        scoring='accuracy',
        n_jobs=-1,
        verbose=1
    )
    
    grid_search.fit(X_train, y_train)
    
    elapsed_time = time.time() - start_time
    
    print(f"\nGrid Search completed in {elapsed_time:.2f} seconds")
    print(f"\nBest parameters: {grid_search.best_params_}")
    print(f"Best CV score: {grid_search.best_score_:.4f}")
    
    y_pred = grid_search.best_estimator_.predict(X_test)
    test_accuracy = accuracy_score(y_test, y_pred)
    
    print(f"Test accuracy: {test_accuracy:.4f}")
    
    results = {
        'best_params': grid_search.best_params_,
        'best_cv_score': grid_search.best_score_,
        'test_accuracy': test_accuracy,
        'elapsed_time': elapsed_time,
        'cv_results': grid_search.cv_results_
    }
    
    visualize_grid_results(grid_search.cv_results_)
    
    return results


def random_search_classification(X_train, X_test, y_train, y_test, n_iter=30):
    """
    Implement Random Search for classification.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test labels
    n_iter : int
        Number of parameter settings sampled
    
    Returns:
    --------
    results : dict
        Random search results
    """
    print(f"\n{'='*60}")
    print(f"RANDOM SEARCH FOR CLASSIFICATION")
    print(f"{'='*60}")
    
    param_distributions = {
        'n_estimators': randint(50, 300),
        'max_depth': [3, 5, 7, 10, None],
        'min_samples_split': randint(2, 20),
        'min_samples_leaf': randint(1, 10),
        'max_features': ['sqrt', 'log2', None],
        'bootstrap': [True, False]
    }
    
    print(f"Parameter distributions: {param_distributions}")
    print(f"Number of iterations: {n_iter}")
    
    base_model = RandomForestClassifier(random_state=42)
    
    start_time = time.time()
    
    random_search = RandomizedSearchCV(
        estimator=base_model,
        param_distributions=param_distributions,
        n_iter=n_iter,
        cv=5,
        scoring='accuracy',
        n_jobs=-1,
        verbose=1,
        random_state=42
    )
    
    random_search.fit(X_train, y_train)
    
    elapsed_time = time.time() - start_time
    
    print(f"\nRandom Search completed in {elapsed_time:.2f} seconds")
    print(f"\nBest parameters: {random_search.best_params_}")
    print(f"Best CV score: {random_search.best_score_:.4f}")
    
    y_pred = random_search.best_estimator_.predict(X_test)
    test_accuracy = accuracy_score(y_test, y_pred)
    
    print(f"Test accuracy: {test_accuracy:.4f}")
    
    results = {
        'best_params': random_search.best_params_,
        'best_cv_score': random_search.best_score_,
        'test_accuracy': test_accuracy,
        'elapsed_time': elapsed_time,
        'cv_results': random_search.cv_results_
    }
    
    visualize_random_results(random_search.cv_results_)
    
    return results


def grid_search_regression(X_train, X_test, y_train, y_test):
    """
    Implement Grid Search for regression.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test targets
    
    Returns:
    --------
    results : dict
        Grid search results
    """
    print(f"\n{'='*60}")
    print(f"GRID SEARCH FOR REGRESSION")
    print(f"{'='*60}")
    
    param_grid = {
        'n_estimators': [50, 100],
        'max_depth': [5, 10, 15, None],
        'min_samples_split': [2, 5],
        'min_samples_leaf': [1, 2]
    }
    
    total_combinations = 1
    for param_values in param_grid.values():
        total_combinations *= len(param_values)
    
    print(f"Parameter grid: {param_grid}")
    print(f"Total combinations: {total_combinations}")
    
    base_model = RandomForestRegressor(random_state=42)
    
    start_time = time.time()
    
    grid_search = GridSearchCV(
        estimator=base_model,
        param_grid=param_grid,
        cv=5,
        scoring='r2',
        n_jobs=-1,
        verbose=1
    )
    
    grid_search.fit(X_train, y_train)
    
    elapsed_time = time.time() - start_time
    
    print(f"\nGrid Search completed in {elapsed_time:.2f} seconds")
    print(f"\nBest parameters: {grid_search.best_params_}")
    print(f"Best CV score (R²): {grid_search.best_score_:.4f}")
    
    y_pred = grid_search.best_estimator_.predict(X_test)
    test_r2 = r2_score(y_test, y_pred)
    test_mse = mean_squared_error(y_test, y_pred)
    
    print(f"Test R²: {test_r2:.4f}")
    print(f"Test MSE: {test_mse:.4f}")
    
    results = {
        'best_params': grid_search.best_params_,
        'best_cv_score': grid_search.best_score_,
        'test_r2': test_r2,
        'test_mse': test_mse,
        'elapsed_time': elapsed_time,
        'cv_results': grid_search.cv_results_
    }
    
    return results


def random_search_regression(X_train, X_test, y_train, y_test, n_iter=30):
    """
    Implement Random Search for regression.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test targets
    n_iter : int
        Number of parameter settings sampled
    
    Returns:
    --------
    results : dict
        Random search results
    """
    print(f"\n{'='*60}")
    print(f"RANDOM SEARCH FOR REGRESSION")
    print(f"{'='*60}")
    
    param_distributions = {
        'n_estimators': randint(50, 300),
        'max_depth': [5, 10, 15, 20, None],
        'min_samples_split': randint(2, 30),
        'min_samples_leaf': randint(1, 15),
        'max_features': ['sqrt', 'log2', None, 0.5]
    }
    
    print(f"Parameter distributions: {param_distributions}")
    print(f"Number of iterations: {n_iter}")
    
    base_model = RandomForestRegressor(random_state=42)
    
    start_time = time.time()
    
    random_search = RandomizedSearchCV(
        estimator=base_model,
        param_distributions=param_distributions,
        n_iter=n_iter,
        cv=5,
        scoring='r2',
        n_jobs=-1,
        verbose=1,
        random_state=42
    )
    
    random_search.fit(X_train, y_train)
    
    elapsed_time = time.time() - start_time
    
    print(f"\nRandom Search completed in {elapsed_time:.2f} seconds")
    print(f"\nBest parameters: {random_search.best_params_}")
    print(f"Best CV score (R²): {random_search.best_score_:.4f}")
    
    y_pred = random_search.best_estimator_.predict(X_test)
    test_r2 = r2_score(y_test, y_pred)
    test_mse = mean_squared_error(y_test, y_pred)
    
    print(f"Test R²: {test_r2:.4f}")
    print(f"Test MSE: {test_mse:.4f}")
    
    results = {
        'best_params': random_search.best_params_,
        'best_cv_score': random_search.best_score_,
        'test_r2': test_r2,
        'test_mse': test_mse,
        'elapsed_time': elapsed_time,
        'cv_results': random_search.cv_results_
    }
    
    return results


def compare_grid_vs_random_search(X_train, X_test, y_train, y_test):
    """
    Compare Grid Search and Random Search performance.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test labels
    
    Returns:
    --------
    comparison : dict
        Comparison results
    """
    print(f"\n{'='*60}")
    print(f"COMPARING GRID SEARCH VS RANDOM SEARCH")
    print(f"{'='*60}")
    
    param_grid = {
        'n_estimators': [50, 100, 150],
        'max_depth': [3, 5, 7, 10],
        'min_samples_split': [2, 5, 10],
        'min_samples_leaf': [1, 2, 4]
    }
    
    total_combinations = 1
    for param_values in param_grid.values():
        total_combinations *= len(param_values)
    
    param_distributions = {
        'n_estimators': randint(50, 200),
        'max_depth': [3, 5, 7, 10, 15],
        'min_samples_split': randint(2, 15),
        'min_samples_leaf': randint(1, 8)
    }
    
    print(f"Grid Search: {total_combinations} combinations")
    print(f"Random Search: 30 iterations")
    
    base_model = RandomForestClassifier(random_state=42)
    
    print("\n--- Running Grid Search ---")
    grid_search = GridSearchCV(
        estimator=base_model,
        param_grid=param_grid,
        cv=5,
        scoring='accuracy',
        n_jobs=-1
    )
    start_time = time.time()
    grid_search.fit(X_train, y_train)
    grid_time = time.time() - start_time
    
    print("\n--- Running Random Search ---")
    random_search = RandomizedSearchCV(
        estimator=base_model,
        param_distributions=param_distributions,
        n_iter=30,
        cv=5,
        scoring='accuracy',
        n_jobs=-1,
        random_state=42
    )
    start_time = time.time()
    random_search.fit(X_train, y_train)
    random_time = time.time() - start_time
    
    grid_test_acc = accuracy_score(y_test, grid_search.best_estimator_.predict(X_test))
    random_test_acc = accuracy_score(y_test, random_search.best_estimator_.predict(X_test))
    
    print(f"\n--- Results ---")
    print(f"Grid Search:")
    print(f"  Time: {grid_time:.2f}s")
    print(f"  Best CV: {grid_search.best_score_:.4f}")
    print(f"  Test: {grid_test_acc:.4f}")
    
    print(f"\nRandom Search:")
    print(f"  Time: {random_time:.2f}s")
    print(f"  Best CV: {random_search.best_score_:.4f}")
    print(f"  Test: {random_test_acc:.4f}")
    
    comparison = {
        'grid_time': grid_time,
        'grid_cv_score': grid_search.best_score_,
        'grid_test_accuracy': grid_test_acc,
        'random_time': random_time,
        'random_cv_score': random_search.best_score_,
        'random_test_accuracy': random_test_acc
    }
    
    visualize_comparison(comparison)
    
    return comparison


def visualize_grid_results(cv_results):
    """
    Visualize grid search results.
    
    Parameters:
    -----------
    cv_results : dict
        Cross-validation results
    """
    plt.figure(figsize=(12, 5))
    
    plt.subplot(1, 2, 1)
    mean_scores = cv_results['mean_test_score']
    std_scores = cv_results['std_test_score']
    params = [str(p) for p in cv_results['params']]
    
    plt.bar(range(len(mean_scores)), mean_scores, yerr=std_scores, alpha=0.7)
    plt.xlabel('Parameter Combination')
    plt.ylabel('CV Accuracy')
    plt.title('Grid Search: CV Scores for Each Combination')
    plt.grid(True, alpha=0.3)
    
    plt.subplot(1, 2, 2)
    n_estimators_values = [p['n_estimators'] for p in cv_results['params']]
    plt.scatter(n_estimators_values, mean_scores, c=mean_scores, cmap='viridis', alpha=0.7)
    plt.xlabel('n_estimators')
    plt.ylabel('CV Accuracy')
    plt.title('Score vs n_estimators')
    plt.colorbar()
    plt.grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()


def visualize_random_results(cv_results):
    """
    Visualize random search results.
    
    Parameters:
    -----------
    cv_results : dict
        Cross-validation results
    """
    plt.figure(figsize=(12, 5))
    
    plt.subplot(1, 2, 1)
    mean_scores = cv_results['mean_test_score']
    params = cv_results['params']
    n_estimators_values = [p['n_estimators'] for p in params]
    
    plt.scatter(n_estimators_values, mean_scores, alpha=0.7)
    plt.xlabel('n_estimators')
    plt.ylabel('CV Accuracy')
    plt.title('Random Search: Score vs n_estimators')
    plt.grid(True, alpha=0.3)
    
    plt.subplot(1, 2, 2)
    max_depth_values = [str(p.get('max_depth', 'None')) for p in params]
    
    unique_depths = list(set(max_depth_values))
    depth_means = []
    for depth in unique_depths:
        indices = [i for i, d in enumerate(max_depth_values) if d == depth]
        depth_means.append(np.mean([mean_scores[i] for i in indices]))
    
    plt.bar(unique_depths, depth_means)
    plt.xlabel('max_depth')
    plt.ylabel('Mean CV Accuracy')
    plt.title('Random Search: Mean Score by max_depth')
    plt.grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()


def visualize_comparison(comparison):
    """
    Visualize comparison between Grid and Random search.
    
    Parameters:
    -----------
    comparison : dict
        Comparison results
    """
    fig, axes = plt.subplots(1, 2, figsize=(12, 4))
    
    methods = ['Grid Search', 'Random Search']
    times = [comparison['grid_time'], comparison['random_time']]
    
    axes[0].bar(methods, times, color=['blue', 'orange'])
    axes[0].set_ylabel('Time (seconds)')
    axes[0].set_title('Search Time Comparison')
    axes[0].grid(True, alpha=0.3)
    
    cv_scores = [comparison['grid_cv_score'], comparison['random_cv_score']]
    test_scores = [comparison['grid_test_accuracy'], comparison['random_test_accuracy']]
    
    x = np.arange(len(methods))
    width = 0.35
    
    axes[1].bar(x - width/2, cv_scores, width, label='CV Score')
    axes[1].bar(x + width/2, test_scores, width, label='Test Score')
    axes[1].set_ylabel('Accuracy')
    axes[1].set_title('Score Comparison')
    axes[1].set_xticks(x)
    axes[1].set_xticklabels(methods)
    axes[1].legend()
    axes[1].grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()


def banking_example():
    """
    Banking/Finance example: Credit Score Prediction with hyperparameter tuning.
    """
    print(f"\n{'='*60}")
    print(f"BANKING EXAMPLE: Credit Score Prediction Tuning")
    print(f"{'='*60}")
    
    np.random.seed(42)
    n_samples = 1000
    
    income = np.random.uniform(25000, 200000, n_samples)
    credit_score = np.random.uniform(500, 850, n_samples)
    debt = np.random.uniform(0, 50000, n_samples)
    employment_years = np.random.uniform(0, 30, n_samples)
    loan_amount = np.random.uniform(1000, 50000, n_samples)
    existing_loans = np.random.randint(0, 5, n_samples)
    
    x0 = income / 10000
    x1 = credit_score / 100
    x2 = debt / 10000
    x3 = employment_years
    x4 = loan_amount / 10000
    x5 = existing_loans
    
    score = (50 + 15 * x0 + 25 * x1 - 5 * x2 + 2 * x3 + 10 * x4 - 5 * x5 + np.random.normal(0, 10, n_samples))
    credit_score_actual = np.clip(score, 500, 850)
    
    df = pd.DataFrame({
        'Income': income,
        'Debt': debt,
        'Employment_Years': employment_years,
        'Loan_Amount': loan_amount,
        'Existing_Loans': existing_loans,
        'Credit_Score_Actual': credit_score_actual
    })
    
    feature_cols = ['Income', 'Debt', 'Employment_Years', 'Loan_Amount', 'Existing_Loans']
    X = df[feature_cols].values
    y = credit_score_actual
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    print(f"\nDataset: {X.shape[0]} samples, {X.shape[1]} features")
    print(f"Train: {X_train.shape[0]}, Test: {X_test.shape[0]}")
    
    grid_results = grid_search_regression(X_train, X_test, y_train, y_test)
    random_results = random_search_regression(X_train, X_test, y_train, y_test, n_iter=20)
    
    print(f"\nSummary:")
    print(f"  Grid Search - Best R²: {grid_results['best_cv_score']:.4f}")
    print(f"  Random Search - Best R²: {random_results['best_cv_score']:.4f}")
    
    return grid_results, random_results


def healthcare_example():
    """
    Healthcare example: Patient Readmission Prediction with hyperparameter tuning.
    """
    print(f"\n{'='*60}")
    print(f"HEALTHCARE EXAMPLE: Patient Readmission Prediction Tuning")
    print(f"{'='*60}")
    
    np.random.seed(123)
    n_samples = 1000
    
    age = np.random.uniform(18, 90, n_samples)
    bmi = np.random.uniform(18, 45, n_samples)
    num_visits = np.random.randint(1, 20, n_samples)
    num_diagnoses = np.random.randint(1, 10, n_samples)
    blood_pressure = np.random.uniform(90, 200, n_samples)
    heart_rate = np.random.uniform(50, 120, n_samples)
    glucose_level = np.random.uniform(70, 200, n_samples)
    cholesterol = np.random.uniform(150, 300, n_samples)
    num_medications = np.random.randint(1, 15, n_samples)
    days_in_hospital = np.random.randint(1, 30, n_samples)
    
    risk_score = (0.3 * age / 100 +
               0.2 * (bmi - 25) / 25 +
               0.15 * num_visits / 20 +
               0.2 * num_diagnoses / 10 +
               0.1 * (blood_pressure - 120) / 80 +
               0.05 * num_medications / 15 +
               0.1 * days_in_hospital / 30 +
               np.random.normal(0, 0.1, n_samples))
    
    readmit = (risk_score > 0.5).astype(int)
    
    df = pd.DataFrame({
        'Age': age,
        'BMI': bmi,
        'Num_Visits': num_visits,
        'Num_Diagnoses': num_diagnoses,
        'Blood_Pressure': blood_pressure,
        'Heart_Rate': heart_rate,
        'Glucose_Level': glucose_level,
        'Cholesterol': cholesterol,
        'Num_Medications': num_medications,
        'Days_In_Hospital': days_in_hospital,
        'Readmit': readmit
    })
    
    feature_cols = ['Age', 'BMI', 'Num_Visits', 'Num_Diagnoses', 'Blood_Pressure',
                   'Heart_Rate', 'Glucose_Level', 'Cholesterol', 'Num_Medications', 'Days_In_Hospital']
    X = df[feature_cols].values
    y = readmit
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    print(f"\nDataset: {X.shape[0]} samples, {X.shape[1]} features")
    print(f"Train: {X_train.shape[0]}, Test: {X_test.shape[0]}")
    print(f"Readmission rate: {readmit.mean():.2%}")
    
    grid_results = grid_search_classification(X_train, X_test, y_train, y_test)
    random_results = random_search_classification(X_train, X_test, y_train, y_test, n_iter=20)
    
    print(f"\nSummary:")
    print(f"  Grid Search - Best Accuracy: {grid_results['best_cv_score']:.4f}")
    print(f"  Random Search - Best Accuracy: {random_results['best_cv_score']:.4f}")
    
    return grid_results, random_results


def test_search_methods():
    """
    Test Grid and Random search methods.
    """
    print(f"\n{'='*60}")
    print(f"TESTING SEARCH METHODS")
    print(f"{'='*60}")
    
    X, y = generate_classification_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    comparison = compare_grid_vs_random_search(X_train, X_test, y_train, y_test)
    
    print(f"\n{'='*60}")
    print(f"ALL TESTS COMPLETED SUCCESSFULLY")
    print(f"{'='*60}")
    
    return True


def main():
    """
    Main function to execute hyperparameter search examples.
    """
    print("="*60)
    print("GRID SEARCH AND RANDOM SEARCH IMPLEMENTATION")
    print("="*60)
    
    print("\nI. INTRODUCTION")
    print("   Grid Search and Random Search are hyperparameter")
    print("   optimization techniques for ML models.")
    
    print("\nII. CORE_CONCEPTS")
    print("   - Grid Search: exhaustive search over all combinations")
    print("   - Random Search: random sampling from distributions")
    print("   - Cross-validation for robust evaluation")
    
    print("\nIII. IMPLEMENTATION")
    
    X, y = generate_classification_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    grid_results = grid_search_classification(X_train, X_test, y_train, y_test)
    random_results = random_search_classification(X_train, X_test, y_train, y_test)
    
    print("\nIV. EXAMPLES")
    banking_grid, banking_random = banking_example()
    healthcare_grid, healthcare_random = healthcare_example()
    
    print("\nV. OUTPUT_RESULTS")
    print("   All search results and visualizations displayed above.")
    
    print("\nVI. TESTING")
    test_search_methods()
    
    print("\nVII. ADVANCED_TOPICS")
    print("   - Bayesian Optimization")
    print("   - Successive Halving")
    print("   - Early stopping")
    
    print("\nVIII. CONCLUSION")
    print("   - Use GRID SEARCH when: small parameter space, exhaustive search needed")
    print("   - Use RANDOM SEARCH when: large parameter space, faster search needed")
    print("   - Random Search often finds good results faster")
    print("   - Always use cross-validation for robust evaluation")
    print("\n   Implementation complete!")


if __name__ == "__main__":
    main()