# Topic: Hyperparameter Tuning Methods
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Hyperparameter Tuning Methods

I. INTRODUCTION
    This module provides an in-depth exploration of hyperparameter tuning methods
    in machine learning using scikit-learn. Hyperparameters are parameters that
    are not learned from data but must be set prior to training. Proper tuning of
    these hyperparameters can significantly improve model performance.
    
    Topics covered:
    - Manual hyperparameter tuning
    - Systematic hyperparameter tuning (Grid Search, Random Search)
    - Bayesian Optimization
    - Sensitivity Analysis
    - Cross-validation strategies for hyperparameter evaluation
    - Real-world banking and healthcare examples
    - Best practices and pitfalls

II. CORE_CONCEPTS
    - Hyperparameters vs Model Parameters
    - Parameter Spaces and Search Grids
    - Overfitting vs Underfitting in Hyperparameter Space
    - Validation Strategies
    - Computational Trade-offs

III. IMPLEMENTATION
    Multiple approaches to hyperparameter optimization with detailed examples

IV. EXAMPLES (Banking + Healthcare)
    - Credit scoring model tuning
    - Medical diagnosis model tuning

V. OUTPUT_RESULTS
    Comprehensive output analysis and interpretation

VI. TESTING
    Unit tests and validation procedures

VII. ADVANCED_TOPICS
    - Bayesian optimization
    - Multi-objective tuning
    - Meta-learning for hyperparameter optimization

VIII. CONCLUSION
    Best practices and recommendations
"""

# ============================================================================
# Import necessary libraries
# ============================================================================

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.datasets import make_classification, make_regression, load_breast_cancer
from sklearn.model_selection import (
    train_test_split, cross_val_score, GridSearchCV, RandomizedSearchCV,
    StratifiedKFold, KFold, LeaveOneOut, TimeSeriesSplit
)
from sklearn.preprocessing import StandardScaler, MinMaxScaler, RobustScaler
from sklearn.linear_model import (
    LogisticRegression, Ridge, Lasso, ElasticNet, LinearRegression
)
from sklearn.ensemble import (
    RandomForestClassifier, RandomForestRegressor, GradientBoostingClassifier,
    GradientBoostingRegressor, AdaBoostClassifier
)
from sklearn.svm import SVC, SVR
from sklearn.neighbors import KNeighborsClassifier, KNeighborsRegressor
from sklearn.tree import DecisionTreeClassifier, DecisionTreeRegressor
from sklearn.naive_bayes import GaussianNB
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score, 
    roc_auc_score, mean_squared_error, mean_absolute_error, r2_score,
    confusion_matrix, classification_report, roc_curve, precision_recall_curve
)
from sklearn.pipeline import Pipeline
from sklearn.base import BaseEstimator, TransformerMixin
import time
import warnings
import json
import os
from collections import defaultdict
from itertools import product
from functools import partial

warnings.filterwarnings('ignore')

# Set random seed for reproducibility
np.random.seed(42)

# ============================================================================
# SECTION I: INTRODUCTION AND DATA GENERATION
# ============================================================================

def generate_classification_data(n_samples=1000, n_features=20, n_informative=10, 
                               n_redundant=5, n_classes=2, imbalance_ratio=0.3):
    """
    Generate synthetic classification data for demonstration purposes.
    
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
    imbalance_ratio : float
        Ratio of minority class
    
    Returns:
    --------
    X : array-like, shape (n_samples, n_features)
        Feature matrix
    y : array-like, shape (n_samples,)
        Target variable
    feature_names : list
        Names of features
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_classes=n_classes,
        weights=[1 - imbalance_ratio, imbalance_ratio],
        random_state=42,
        flip_y=0.05,
        class_sep=1.0
    )
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    
    # Add some noise features
    noise = np.random.randn(n_samples, 5)
    X = np.hstack([X, noise])
    feature_names.extend([f'noise_feature_{i}' for i in range(5)])
    
    return X, y, feature_names


def generate_regression_data(n_samples=1000, n_features=10, noise=0.1):
    """
    Generate synthetic regression data for demonstration.
    """
    X, y = make_regression(
        n_samples=n_samples,
        n_features=n_features,
        noise=noise,
        random_state=42,
        effective_rank=5
    )
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    return X, y, feature_names


def load_banking_data():
    """
    Simulate banking/finance data for credit scoring example.
    
    Returns a DataFrame with features relevant to credit scoring:
    - income: Annual income
    - credit_history_length: Years of credit history
    - debt_to_income: Debt to income ratio
    - employment_years: Years at current employment
    - loan_amount: Requested loan amount
    - previous_loans: Number of previous loans
    - payment_history: Payment history score
    - age: Age of applicant
    - education_level: Education level (categorical)
    - mortgage: Whether they have a mortgage
    """
    np.random.seed(42)
    n_samples = 2000
    
    income = np.random.lognormal(10.5, 0.8, n_samples)
    credit_history_length = np.random.exponential(5, n_samples) + 1
    debt_to_income = np.random.beta(2, 8, n_samples) * 0.5
    employment_years = np.random.exponential(3, n_samples) + 0.5
    loan_amount = np.random.lognormal(9, 1, n_samples)
    previous_loans = np.random.poisson(2, n_samples)
    payment_history = np.random.beta(8, 2, n_samples)
    age = np.random.normal(40, 12, n_samples)
    age = np.clip(age, 21, 70)
    education_level = np.random.choice([1, 2, 3, 4], n_samples, p=[0.2, 0.4, 0.3, 0.1])
    mortgage = np.random.choice([0, 1], n_samples, p=[0.6, 0.4])
    
    # Create target (default = 1, no default = 0)
    # Higher risk: low income, short credit history, high debt-to-income,
    # many previous loans, poor payment history
    risk_score = (
        -0.3 * (income / income.mean()) +
        -0.2 * (credit_history_length / credit_history_length.mean()) +
        0.4 * debt_to_income +
        -0.1 * (employment_years / employment_years.mean()) +
        -0.2 * payment_history +
        0.1 * (previous_loans / previous_loans.max())
    )
    
   prob_default = 1 / (1 + np.exp(-risk_score * 3))
    y = (np.random.random(n_samples) < prob_default).astype(int)
    
    df = pd.DataFrame({
        'income': income,
        'credit_history_length': credit_history_length,
        'debt_to_income': debt_to_income,
        'employment_years': employment_years,
        'loan_amount': loan_amount,
        'previous_loans': previous_loans,
        'payment_history': payment_history,
        'age': age,
        'education_level': education_level,
        'mortgage': mortgage,
        'default': y
    })
    
    return df


def load_healthcare_data():
    """
    Simulate healthcare/medical data for diagnosis prediction.
    
    Features relevant to medical diagnosis:
    - age: Patient age
    - bmi: Body mass index
    - blood_pressure_systolic: Systolic blood pressure
    - blood_pressure_diastolic: Diastolic blood pressure
    - cholesterol_total: Total cholesterol
    - cholesterol_hdl: HDL cholesterol
    - cholesterol_ldl: LDL cholesterol
    - glucose: Blood glucose level
    - heart_rate: Resting heart rate
    - smoking_years: Years of smoking
    - alcohol_use: Weekly alcohol consumption
    - exercise_minutes: Weekly exercise minutes
    - family_history: Family history of disease
    """
    np.random.seed(42)
    n_samples = 2000
    
    age = np.random.normal(55, 15, n_samples)
    age = np.clip(age, 18, 90)
    
    bmi = np.random.normal(27, 5, n_samples)
    bmi = np.clip(bmi, 15, 45)
    
    blood_pressure_systolic = np.random.normal(130, 20, n_samples)
    blood_pressure_systolic = np.clip(blood_pressure_systolic, 90, 200)
    
    blood_pressure_diastolic = np.random.normal(85, 12, n_samples)
    blood_pressure_diastolic = np.clip(blood_pressure_diastolic, 60, 130)
    
    cholesterol_total = np.random.normal(200, 40, n_samples)
    cholesterol_hdl = np.random.normal(55, 15, n_samples)
    cholesterol_ldl = np.random.normal(120, 30, n_samples)
    
    glucose = np.random.normal(100, 25, n_samples)
    glucose = np.clip(glucose, 70, 300)
    
    heart_rate = np.random.normal(72, 12, n_samples)
    
    smoking_years = np.random.exponential(10, n_samples)
    smoking_years = np.clip(smoking_years, 0, 50)
    
    alcohol_use = np.random.poisson(3, n_samples)
    alcohol_use = np.clip(alcohol_use, 0, 20)
    
    exercise_minutes = np.random.exponential(120, n_samples)
    exercise_minutes = np.clip(exercise_minutes, 0, 400)
    
    family_history = np.random.choice([0, 1], n_samples, p=[0.7, 0.3])
    
    # Create target (disease = 1, no disease = 0)
    # Higher risk: older, higher BMI, high BP, high cholesterol, high glucose,
    # smoker, no exercise, family history
    risk_score = (
        0.02 * age +
        0.05 * (bmi - 27) +
        0.01 * (blood_pressure_systolic - 130) +
        0.002 * (cholesterol_total - 200) +
        0.01 * (glucose - 100) +
        0.02 * smoking_years +
        -0.005 * exercise_minutes +
        0.3 * family_history
    )
    
    prob_disease = 1 / (1 + np.exp(-risk_score * 0.15))
    y = (np.random.random(n_samples) < prob_disease).astype(int)
    
    df = pd.DataFrame({
        'age': age,
        'bmi': bmi,
        'blood_pressure_systolic': blood_pressure_systolic,
        'blood_pressure_diastolic': blood_pressure_diastolic,
        'cholesterol_total': cholesterol_total,
        'cholesterol_hdl': cholesterol_hdl,
        'cholesterol_ldl': cholesterol_ldl,
        'glucose': glucose,
        'heart_rate': heart_rate,
        'smoking_years': smoking_years,
        'alcohol_use': alcohol_use,
        'exercise_minutes': exercise_minutes,
        'family_history': family_history,
        'disease': y
    })
    
    return df


# ============================================================================
# SECTION II: MANUAL HYPERPARAMETER TUNING
# ============================================================================

class ManualTuningDemo:
    """
    Demonstration of manual hyperparameter tuning approach.
    
    Manual tuning involves iteratively adjusting hyperparameters based on
    validation performance. While time-consuming, it can be effective
    for understanding the relationship between hyperparameters and model performance.
    """
    
    def __init__(self, model_type='logistic'):
        self.model_type = model_type
        self.best_params = None
        self.best_score = 0
        self.history = []
        
    def tune_logistic_regression(self, X_train, y_train, X_val, y_val):
        """
        Manually tune Logistic Regression hyperparameters.
        
        Key hyperparameters:
        - C: Inverse of regularization strength
        - penalty: Type of regularization (l1, l2, elasticnet)
        - solver: Optimization algorithm
        - max_iter: Maximum iterations
        """
        print("\n" + "="*60)
        print("MANUAL TUNING: Logistic Regression")
        print("="*60)
        
        param_grid = {
            'C': [0.001, 0.01, 0.1, 1, 10, 100],
            'penalty': ['l1', 'l2'],
            'solver': ['liblinear', 'saga']
        }
        
        results = []
        
        for C in param_grid['C']:
            for penalty in param_grid['penalty']:
                for solver in param_grid['solver']:
                    # Skip invalid combinations
                    if penalty == 'l1' and solver not in ['liblinear', 'saga']:
                        continue
                    
                    try:
                        model = LogisticRegression(
                            C=C,
                            penalty=penalty,
                            solver=solver,
                            max_iter=1000,
                            random_state=42
                        )
                        model.fit(X_train, y_train)
                        
                        train_score = model.score(X_train, y_train)
                        val_score = model.score(X_val, y_val)
                        
                        results.append({
                            'C': C,
                            'penalty': penalty,
                            'solver': solver,
                            'train_score': train_score,
                            'val_score': val_score
                        })
                        
                        print(f"C={C:>8}, penalty={penalty}, solver={solver:<10} "
                              f"-> Train: {train_score:.4f}, Val: {val_score:.4f}")
                        
                        if val_score > self.best_score:
                            self.best_score = val_score
                            self.best_params = {
                                'C': C,
                                'penalty': penalty,
                                'solver': solver
                            }
                    except Exception as e:
                        print(f"C={C}, penalty={penalty}, solver={solver} -> Error: {e}")
        
        self.history.extend(results)
        return results
    
    def tune_random_forest(self, X_train, y_train, X_val, y_val):
        """
        Manually tune Random Forest hyperparameters.
        
        Key hyperparameters:
        - n_estimators: Number of trees
        - max_depth: Maximum depth of trees
        - min_samples_split: Minimum samples to split
        - min_samples_leaf: Minimum samples in leaf
        - max_features: Number of features to consider
        """
        print("\n" + "="*60)
        print("MANUAL TUNING: Random Forest")
        print("="*60)
        
        param_grid = {
            'n_estimators': [50, 100, 200],
            'max_depth': [3, 5, 10, None],
            'min_samples_split': [2, 5, 10],
            'min_samples_leaf': [1, 2, 4]
        }
        
        results = []
        total = (len(param_grid['n_estimators']) * 
                len(param_grid['max_depth']) * 
                len(param_grid['min_samples_split']) * 
                len(param_grid['min_samples_leaf']))
        
        print(f"Testing {total} parameter combinations...")
        
        count = 0
        for n_estimators in param_grid['n_estimators']:
            for max_depth in param_grid['max_depth']:
                for min_samples_split in param_grid['min_samples_split']:
                    for min_samples_leaf in param_grid['min_samples_leaf']:
                        count += 1
                        
                        try:
                            model = RandomForestClassifier(
                                n_estimators=n_estimators,
                                max_depth=max_depth,
                                min_samples_split=min_samples_split,
                                min_samples_leaf=min_samples_leaf,
                                random_state=42,
                                n_jobs=-1
                            )
                            model.fit(X_train, y_train)
                            
                            train_score = model.score(X_train, y_train)
                            val_score = model.score(X_val, y_val)
                            
                            results.append({
                                'n_estimators': n_estimators,
                                'max_depth': max_depth,
                                'min_samples_split': min_samples_split,
                                'min_samples_leaf': min_samples_leaf,
                                'train_score': train_score,
                                'val_score': val_score
                            })
                            
                            if count % 10 == 0 or count == total:
                                print(f"Progress: {count}/{total} combinations tested, "
                                      f"best val score: {self.best_score:.4f}")
                            
                            if val_score > self.best_score:
                                self.best_score = val_score
                                self.best_params = {
                                    'n_estimators': n_estimators,
                                    'max_depth': max_depth,
                                    'min_samples_split': min_samples_split,
                                    'min_samples_leaf': min_samples_leaf
                                }
                        except Exception as e:
                            print(f"Error: {e}")
        
        self.history.extend(results)
        print(f"\nBest parameters: {self.best_params}")
        print(f"Best validation score: {self.best_score:.4f}")
        
        return results
    
    def sensitivity_analysis(self, X_train, y_train, X_val, y_val,
                            param_name, param_values):
        """
        Perform sensitivity analysis on a single hyperparameter.
        
        This helps understand how changes in a specific hyperparameter
        affect model performance.
        """
        print(f"\nSensitivity Analysis: {param_name}")
        print("-" * 40)
        
        results = []
        
        for value in param_values:
            if self.model_type == 'logistic':
                model = LogisticRegression(
                    **{param_name: value},
                    max_iter=1000,
                    random_state=42
                )
            else:
                model = RandomForestClassifier(
                    **{param_name: value},
                    n_estimators=100,
                    random_state=42
                )
            
            model.fit(X_train, y_train)
            
            train_score = model.score(X_train, y_train)
            val_score = model.score(X_val, y_val)
            
            results.append({
                param_name: value,
                'train_score': train_score,
                'val_score': val_score
            })
            
            print(f"{param_name}={value} -> Train: {train_score:.4f}, Val: {val_score:.4f}")
        
        return results


# ============================================================================
# SECTION III: SYSTEMATIC HYPERPARAMETER TUNING
# ============================================================================

class SystematicTuningDemo:
    """
    Systematic approaches to hyperparameter tuning using:
    - Grid Search
    - Random Search
    - Combined approaches
    """
    
    def __init__(self):
        self.best_params = None
        self.best_score = 0
        self.cv_results = None
        
    def grid_search_demo(self, X, y, param_grid=None, cv=5):
        """
        Demonstrate Grid Search Cross-Validation.
        
        Grid Search exhaustively searches through all possible
        combinations of hyperparameters in the defined parameter grid.
        
        Pros:
        - Exhaustive search guarantees finding optimal combination
        - Simple to implement and understand
        
        Cons:
        - Computationally expensive for large parameter spaces
        - May miss optimal values between grid points
        """
        print("\n" + "="*60)
        print("GRID SEARCH CROSS-VALIDATION")
        print("="*60)
        
        if param_grid is None:
            param_grid = {
                'classifier__C': [0.01, 0.1, 1, 10],
                'classifier__penalty': ['l1', 'l2'],
                'classifier__solver': ['liblinear']
            }
        
        # Create pipeline with scaling
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', LogisticRegression(max_iter=1000, random_state=42))
        ])
        
        # Grid Search with cross-validation
        grid_search = GridSearchCV(
            pipeline,
            param_grid,
            cv=cv,
            scoring='accuracy',
            return_train_score=True,
            verbose=1,
            n_jobs=-1
        )
        
        start_time = time.time()
        grid_search.fit(X, y)
        elapsed = time.time() - start_time
        
        print(f"\nGrid Search Results:")
        print(f"  Best Score (CV): {grid_search.best_score_:.4f}")
        print(f"  Best Parameters: {grid_search.best_params_}")
        print(f"  Time Elapsed: {elapsed:.2f} seconds")
        
        self.best_params = grid_search.best_params_
        self.best_score = grid_search.best_score_
        self.cv_results = pd.DataFrame(grid_search.cv_results_)
        
        return grid_search
    
    def random_search_demo(self, X, y, param_distributions=None, n_iter=50, cv=5):
        """
        Demonstrate Random Search Cross-Validation.
        
        Random Search samples random combinations of hyperparameters
        from specified distributions.
        
        Pros:
        - More efficient for large parameter spaces
        - Can find better values in continuous parameters
        - Better exploration of parameter space
        
        Cons:
        - May miss optimal combination
        - Results may vary between runs
        """
        print("\n" + "="*60)
        print("RANDOM SEARCH CROSS-VALIDATION")
        print("="*60)
        
        if param_distributions is None:
            param_distributions = {
                'classifier__C': [0.001, 0.01, 0.1, 1, 10, 100],
                'classifier__penalty': ['l1', 'l2'],
                'classifier__solver': ['liblinear'],
                'classifier__class_weight': [None, 'balanced']
            }
        
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', LogisticRegression(max_iter=1000, random_state=42))
        ])
        
        # Random Search with cross-validation
        random_search = RandomizedSearchCV(
            pipeline,
            param_distributions,
            n_iter=n_iter,
            cv=cv,
            scoring='accuracy',
            return_train_score=True,
            verbose=1,
            random_state=42,
            n_jobs=-1
        )
        
        start_time = time.time()
        random_search.fit(X, y)
        elapsed = time.time() - start_time
        
        print(f"\nRandom Search Results:")
        print(f"  Best Score (CV): {random_search.best_score_:.4f}")
        print(f"  Best Parameters: {random_search.best_params_}")
        print(f"  Time Elapsed: {elapsed:.2f} seconds")
        
        self.best_params = random_search.best_params_
        self.best_score = random_search.best_score_
        self.cv_results = pd.DataFrame(random_search.cv_results_)
        
        return random_search
    
    def nested_cross_validation(self, X, y):
        """
        Demonstrate nested cross-validation for unbiased evaluation.
        
        Nested CV uses:
        - Inner loop: Hyperparameter tuning
        - Outer loop: Model evaluation
        
        This provides an unbiased estimate of model performance.
        """
        print("\n" + "="*60)
        print("NESTED CROSS-VALIDATION")
        print("="*60)
        
        param_grid = {
            'classifier__C': [0.01, 0.1, 1, 10],
            'classifier__penalty': ['l1', 'l2']
        }
        
        # Outer CV (for evaluation)
        outer_cv = StratifiedKFold(n_splits=5, shuffle=True, random_state=42)
        
        # Inner CV (for tuning) - defined in GridSearchCV
        inner_cv = StratifiedKFold(n_splits=3, shuffle=True, random_state=42)
        
        outer_scores = []
        
        for fold, (train_idx, val_idx) in enumerate(outer_cv.split(X, y)):
            X_train, X_val = X[train_idx], X[val_idx]
            y_train, y_val = y[train_idx], y[val_idx]
            
            pipeline = Pipeline([
                ('scaler', StandardScaler()),
                ('classifier', LogisticRegression(max_iter=1000, random_state=42))
            ])
            
            grid_search = GridSearchCV(
                pipeline,
                param_grid,
                cv=inner_cv,
                scoring='accuracy'
            )
            
            grid_search.fit(X_train, y_train)
            
            # Evaluate on outer fold
            y_pred = grid_search.predict(X_val)
            score = accuracy_score(y_val, y_pred)
            outer_scores.append(score)
            
            print(f"Fold {fold+1}: Score = {score:.4f}, "
                  f"Best params: {grid_search.best_params_}")
        
        print(f"\nNested CV Mean Score: {np.mean(outer_scores):.4f} "
              f"+/- {np.std(outer_scores):.4f}")
        
        return outer_scores
    
    def comparison_grid_vs_random(self, X, y):
        """
        Compare Grid Search and Random Search approaches.
        """
        print("\n" + "="*60)
        print("COMPARISON: GRID SEARCH vs RANDOM SEARCH")
        print("="*60)
        
        # Define parameter space
        param_grid = {
            'classifier__n_estimators': [50, 100, 200],
            'classifier__max_depth': [3, 5, 7, 10],
            'classifier__min_samples_split': [2, 5, 10],
            'classifier__min_samples_leaf': [1, 2, 4]
        }
        
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', RandomForestClassifier(random_state=42, n_jobs=-1))
        ])
        
        # Calculate total combinations
        total_combinations = 1
        for key, values in param_grid.items():
            total_combinations *= len(values)
        print(f"Total parameter combinations: {total_combinations}")
        
        # Grid Search
        print("\n--- Grid Search ---")
        start_time = time.time()
        grid_search = GridSearchCV(
            pipeline,
            param_grid,
            cv=3,
            scoring='accuracy',
            n_jobs=-1
        )
        grid_search.fit(X, y)
        grid_time = time.time() - start_time
        
        print(f"Best Score: {grid_search.best_score_:.4f}")
        print(f"Time: {grid_time:.2f} seconds")
        
        # Random Search with same number of iterations
        print("\n--- Random Search (same iterations) ---")
        start_time = time.time()
        random_search = RandomizedSearchCV(
            pipeline,
            param_grid,
            n_iter=total_combinations,
            cv=3,
            scoring='accuracy',
            random_state=42,
            n_jobs=-1
        )
        random_search.fit(X, y)
        random_time = time.time() - start_time
        
        print(f"Best Score: {random_search.best_score_:.4f}")
        print(f"Time: {random_time:.2f} seconds")
        
        return {
            'grid_search': {
                'best_score': grid_search.best_score_,
                'time': grid_time,
                'best_params': grid_search.best_params_
            },
            'random_search': {
                'best_score': random_search.best_score_,
                'time': random_time,
                'best_params': random_search.best_params_
            }
        }


# ============================================================================
# SECTION IV: ADVANCED TUNING METHODS
# ============================================================================

class AdvancedTuningMethods:
    """
    Advanced hyperparameter tuning methods including:
    - Bayesian Optimization
    - Gradient-based optimization
    - Meta-learning approaches
    """
    
    def bayesian_optimization_simulation(self, X, y, n_iter=20):
        """
        Simulate Bayesian Optimization for hyperparameter tuning.
        
        In practice, libraries like optuna or scikit-optimize are used.
        This demonstrates the conceptual approach.
        """
        print("\n" + "="*60)
        print("BAYESIAN OPTIMIZATION (Simulated)")
        print("="*60)
        
        # Define parameter space
        param_space = {
            'n_estimators': (10, 200),
            'max_depth': (1, 20),
            'min_samples_split': (2, 20),
            'learning_rate': (0.01, 0.3)
        }
        
        # Initialize with random hyperparameters
        def get_random_params():
            return {
                'n_estimators': int(np.random.uniform(10, 200)),
                'max_depth': int(np.random.uniform(1, 20)),
                'min_samples_split': int(np.random.uniform(2, 20)),
                'learning_rate': np.random.uniform(0.01, 0.3)
            }
        
        def evaluate_params(params):
            model = RandomForestClassifier(
                n_estimators=params['n_estimators'],
                max_depth=params['max_depth'],
                min_samples_split=params['min_samples_split'],
                random_state=42,
                n_jobs=-1
            )
            
            scores = cross_val_score(model, X, y, cv=3, scoring='accuracy')
            return np.mean(scores)
        
        # Track results
        history = []
        best_score = 0
        best_params = None
        
        # Initial random evaluations
        n_initial = 5
        print(f"Initial random evaluations: {n_initial}")
        
        for i in range(n_initial):
            params = get_random_params()
            score = evaluate_params(params)
            history.append((params, score))
            
            if score > best_score:
                best_score = score
                best_params = params.copy()
            
            print(f"  Iteration {i+1}: Score = {score:.4f}")
        
        # Bayesian optimization iterations (simplified)
        print(f"\nBayesian optimization iterations: {n_iter}")
        
        for i in range(n_iter):
            # In practice, use Gaussian Process to model the function
            # and select next point using acquisition function
            # Here we use a simple heuristic: favor exploration near best params
            
            # Simulated acquisition: random exploration
            if np.random.random() < 0.3:
                params = get_random_params()
            else:
                # Exploitation: sample near best params
                params = {
                    'n_estimators': int(best_params['n_estimators'] + 
                                      np.random.normal(0, 10)),
                    'max_depth': int(best_params['max_depth'] + 
                                np.random.normal(0, 2)),
                    'min_samples_split': int(best_params['min_samples_split'] + 
                                         np.random.normal(0, 2)),
                    'learning_rate': best_params['learning_rate'] + np.random.normal(0, 0.02)
                }
                
                # Clamp values
                params['n_estimators'] = np.clip(params['n_estimators'], 10, 200)
                params['max_depth'] = np.clip(params['max_depth'], 1, 20)
                params['min_samples_split'] = np.clip(params['min_samples_split'], 2, 20)
                params['learning_rate'] = np.clip(params['learning_rate'], 0.01, 0.3)
            
            params['n_estimators'] = int(params['n_estimators'])
            params['max_depth'] = int(params['max_depth'])
            params['min_samples_split'] = int(params['min_samples_split'])
            
            score = evaluate_params(params)
            history.append((params, score))
            
            if score > best_score:
                best_score = score
                best_params = params.copy()
                print(f"  Iteration {n_initial + i + 1}: NEW BEST! Score = {score:.4f}")
            else:
                print(f"  Iteration {n_initial + i + 1}: Score = {score:.4f}")
        
        print(f"\nBest Score: {best_score:.4f}")
        print(f"Best Parameters: {best_params}")
        
        return history, best_params, best_score
    
    def multi_objective_tuning(self, X, y):
        """
        Demonstrate multi-objective hyperparameter tuning.
        
        Balance multiple objectives like:
        - Accuracy
        - Model complexity
        - Training time
        - Inference time
        """
        print("\n" + "="*60)
        print("MULTI-OBJECTIVE TUNING")
        print("="*60)
        
        param_grid = {
            'n_estimators': [10, 50, 100, 200],
            'max_depth': [3, 5, 10, 15, None],
            'min_samples_leaf': [1, 2, 4, 8]
        }
        
        results = []
        
        for params in product(*param_grid.values()):
            param_dict = dict(zip(param_grid.keys(), params))
            
            model = RandomForestClassifier(
                **param_dict,
                random_state=42,
                n_jobs=-1
            )
            
            # Measure accuracy
            scores = cross_val_score(model, X, y, cv=3, scoring='accuracy')
            accuracy = np.mean(scores)
            
            # Measure training time
            start = time.time()
            model.fit(X, y)
            train_time = time.time() - start
            
            # Count nodes (proxy for complexity)
            n_nodes = sum([tree.node_count for tree in model.estimators_])
            
            # Combined score (Pareto optimization)
            results.append({
                **param_dict,
                'accuracy': accuracy,
                'train_time': train_time,
                'n_nodes': n_nodes,
                'complexity': n_nodes / 1000  # Normalized
            })
        
        # Find Pareto-optimal solutions
        pareto_solutions = []
        for i, r in enumerate(results):
            dominated = False
            for j, other in enumerate(results):
                if i != j:
                    if (other['accuracy'] >= r['accuracy'] and 
                        other['train_time'] <= r['train_time'] and
                        other['complexity'] <= r['complexity']):
                        if (other['accuracy'] > r['accuracy'] or 
                            other['train_time'] < r['train_time'] or
                            other['complexity'] < r['complexity']):
                            dominated = True
                            break
            if not dominated:
                pareto_solutions.append(r)
        
        print(f"\nFound {len(pareto_solutions)} Pareto-optimal solutions:")
        
        for i, sol in enumerate(pareto_solutions[:5]):
            print(f"  Solution {i+1}: accuracy={sol['accuracy']:.4f}, "
                  f"train_time={sol['train_time']:.4f}s, "
                  f"complexity={sol['complexity']:.2f}K nodes")
        
        return results, pareto_solutions


# ============================================================================
# SECTION V: BANKING EXAMPLE (Credit Scoring)
# ============================================================================

class BankingCreditScoringExample:
    """
    Comprehensive example of hyperparameter tuning for credit scoring model.
    
    This demonstrates:
    - Data preprocessing for financial data
    - Model selection and tuning
    - Business-specific evaluation metrics
    - Production considerations
    """
    
    def __init__(self):
        self.model = None
        self.best_params = None
        self.scaler = None
        
    def load_and_preprocess_data(self):
        """Load and preprocess banking data."""
        print("\n" + "="*60)
        print("BANKING EXAMPLE: Credit Scoring")
        print("="*60)
        
        df = load_banking_data()
        
        print(f"Dataset shape: {df.shape}")
        print(f"Class distribution:\n{df['default'].value_counts()}")
        print(f"Default rate: {df['default'].mean():.2%}")
        
        # Separate features and target
        X = df.drop('default', axis=1)
        y = df['default']
        
        # Scale features
        self.scaler = StandardScaler()
        X_scaled = self.scaler.fit_transform(X)
        
        return X_scaled, y, df
    
    def tune_logistic_regression(self, X, y):
        """
        Tune Logistic Regression for credit scoring.
        
        Key considerations:
        - C: Regularization strength (prevent overfitting)
        - class_weight: Handle class imbalance
        - penalty: L1 for feature selection
        """
        print("\n1. Logistic Regression Tuning")
        print("-" * 40)
        
        param_grid = {
            'classifier__C': [0.001, 0.01, 0.1, 1, 10, 100],
            'classifier__penalty': ['l1', 'l2'],
            'classifier__class_weight': [None, 'balanced'],
            'classifier__solver': ['liblinear']
        }
        
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', LogisticRegression(max_iter=1000, random_state=42))
        ])
        
        # Use stratified k-fold for imbalanced data
        grid_search = GridSearchCV(
            pipeline,
            param_grid,
            cv=StratifiedKFold(n_splits=5, shuffle=True, random_state=42),
            scoring='roc_auc',
            return_train_score=True,
            n_jobs=-1
        )
        
        grid_search.fit(X, y)
        
        print(f"Best ROC-AUC: {grid_search.best_score_:.4f}")
        print(f"Best Parameters: {grid_search.best_params_}")
        
        self.best_params = grid_search.best_params_
        
        return grid_search
    
    def tune_random_forest(self, X, y):
        """
        Tune Random Forest for credit scoring.
        
        Key parameters for financial models:
        - n_estimators: Stability
        - max_depth: Prevent overfitting
        - min_samples_leaf: Handle outliers
        - class_weight: Handle imbalance
        """
        print("\n2. Random Forest Tuning")
        print("-" * 40)
        
        param_grid = {
            'classifier__n_estimators': [50, 100, 200],
            'classifier__max_depth': [3, 5, 10, None],
            'classifier__min_samples_leaf': [1, 5, 10, 20],
            'classifier__class_weight': [None, 'balanced', 'balanced_subsample']
        }
        
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', RandomForestClassifier(random_state=42, n_jobs=-1))
        ])
        
        grid_search = GridSearchCV(
            pipeline,
            param_grid,
            cv=StratifiedKFold(n_splits=5, shuffle=True, random_state=42),
            scoring='roc_auc',
            return_train_score=True,
            n_jobs=-1
        )
        
        grid_search.fit(X, y)
        
        print(f"Best ROC-AUC: {grid_search.best_score_:.4f}")
        print(f"Best Parameters: {grid_search.best_params_}")
        
        return grid_search
    
    def tune_gradient_boosting(self, X, y):
        """
        Tune Gradient Boosting for credit scoring.
        
        Gradient Boosting often provides best performance but
        requires careful tuning of:
        - n_estimators: Number of boosting stages
        - learning_rate: Shrinkage
        - max_depth: Tree complexity
        - subsample: Stochastic sampling
        """
        print("\n3. Gradient Boosting Tuning")
        print("-" * 40)
        
        param_grid = {
            'classifier__n_estimators': [50, 100, 200],
            'classifier__learning_rate': [0.01, 0.05, 0.1, 0.2],
            'classifier__max_depth': [2, 3, 5, 7],
            'classifier__subsample': [0.7, 0.8, 0.9, 1.0]
        }
        
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', GradientBoostingClassifier(random_state=42))
        ])
        
        grid_search = GridSearchCV(
            pipeline,
            param_grid,
            cv=StratifiedKFold(n_splits=5, shuffle=True, random_state=42),
            scoring='roc_auc',
            return_train_score=True,
            n_jobs=-1
        )
        
        grid_search.fit(X, y)
        
        print(f"Best ROC-AUC: {grid_search.best_score_:.4f}")
        print(f"Best Parameters: {grid_search.best_params_}")
        
        return grid_search
    
    def evaluate_models(self, X, y, models_dict):
        """
        Comprehensive evaluation of tuned models.
        
        Metrics for credit scoring:
        - ROC-AUC: Overall discrimination
        - Precision/Recall: At various thresholds
        - Calibration: Probability accuracy
        - Business metrics: False positive/negative costs
        """
        print("\n" + "="*60)
        print("MODEL EVALUATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        results = []
        
        for name, model in models_dict.items():
            model.fit(X_train, y_train)
            
            y_pred = model.predict(X_test)
            y_prob = model.predict_proba(X_test)[:, 1]
            
            # Calculate metrics
            accuracy = accuracy_score(y_test, y_pred)
            precision = precision_score(y_test, y_pred)
            recall = recall_score(y_test, y_pred)
            f1 = f1_score(y_test, y_pred)
            roc_auc = roc_auc_score(y_test, y_prob)
            
            # Confusion matrix
            cm = confusion_matrix(y_test, y_pred)
            
            print(f"\n{name}:")
            print(f"  Accuracy:  {accuracy:.4f}")
            print(f"  Precision: {precision:.4f}")
            print(f"  Recall:    {recall:.4f}")
            print(f"  F1 Score: {f1:.4f}")
            print(f"  ROC-AUC:  {roc_auc:.4f}")
            print(f"  Confusion Matrix:")
            print(f"    TN: {cm[0,0]}, FP: {cm[0,1]}")
            print(f"    FN: {cm[1,0]}, TP: {cm[1,1]}")
            
            results.append({
                'model': name,
                'accuracy': accuracy,
                'precision': precision,
                'recall': recall,
                'f1': f1,
                'roc_auc': roc_auc,
                'cm': cm
            })
        
        return results
    
    def business_analysis(self, results, df):
        """
        Business-specific analysis of model performance.
        
        Credit Scoring considers:
        - False Positive Rate: Rejecting good customers
        - False Negative Rate: Accepting bad customers
        - Expected loss calculation
        """
        print("\n" + "="*60)
        print("BUSINESS ANALYSIS")
        print("="*60)
        
        # Assume average loan amount
        avg_loan = df['loan_amount'].mean()
        
        # Assume recovery rate for defaulted loans
        recovery_rate = 0.3
        
        for result in results:
            cm = result['cm']
            tn, fp, fn, tp = cm.ravel()
            
            # Calculate rates
            fp_rate = fp / (fp + tn)  # Good customers rejected
            fn_rate = fn / (fn + tp)  # Bad customers accepted
            
            # Estimate losses
            # False negatives cause losses
            expected_loss_from_defaults = fn * avg_loan * (1 - recovery_rate)
            
            # False positives lose potential interest
            interest_rate = 0.05  # 5% interest
            potential_interest_lost = fp * avg_loan * interest_rate * 5  # 5-year loan
            
            print(f"\n{result['model']}:")
            print(f"  False Positive Rate: {fp_rate:.2%}")
            print(f"  False Negative Rate: {fn_rate:.2%}")
            print(f"  Expected Loss from Defaults: ${expected_loss_from_defaults:,.0f}")
            print(f"  Potential Interest Lost: ${potential_interest_lost:,.0f}")
            print(f"  Total Cost: ${expected_loss_from_defaults + potential_interest_lost:,.0f}")


# ============================================================================
# SECTION VI: HEALTHCARE EXAMPLE (Diagnosis Prediction)
# ============================================================================

class HealthcareDiagnosisExample:
    """
    Comprehensive example of hyperparameter tuning for medical diagnosis.
    
    Key considerations:
    - Interpretability for medical professionals
    - Robustness and reliability
    - Handling missing data
    - Multi-class classification for multiple conditions
    """
    
    def __init__(self):
        self.model = None
        self.best_params = None
        
    def load_and_preprocess_data(self):
        """Load and preprocess healthcare data."""
        print("\n" + "="*60)
        print("HEALTHCARE EXAMPLE: Disease Diagnosis")
        print("="*60)
        
        df = load_healthcare_data()
        
        print(f"Dataset shape: {df.shape}")
        print(f"Class distribution:\n{df['disease'].value_counts()}")
        print(f"Disease prevalence: {df['disease'].mean():.2%}")
        
        # Check for missing values
        missing = df.isnull().sum()
        if missing.sum() > 0:
            print(f"Missing values:\n{missing}")
        else:
            print("No missing values")
        
        # Separate features and target
        X = df.drop('disease', axis=1)
        y = df['disease']
        
        # Scale features
        scaler = StandardScaler()
        X_scaled = scaler.fit_transform(X)
        
        return X_scaled, y, df
    
    def tune_for_interpretability(self, X, y):
        """
        Tune models with focus on interpretability.
        
        For medical applications, interpretability is crucial.
        Consider:
        - Decision trees (directly interpretable)
        - Logistic Regression (coefficient-based)
        - Simpler models with good performance
        """
        print("\n1. Interpretable Models")
        print("-" * 40)
        
        # Decision Tree
        print("\nDecision Tree:")
        param_grid = {
            'classifier__max_depth': [2, 3, 4, 5, 6],
            'classifier__min_samples_leaf': [5, 10, 20, 50],
            'classifier__criterion': ['gini', 'entropy']
        }
        
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', DecisionTreeClassifier(random_state=42))
        ])
        
        grid_search = GridSearchCV(
            pipeline,
            param_grid,
            cv=StratifiedKFold(n_splits=5, shuffle=True, random_state=42),
            scoring='f1',
            n_jobs=-1
        )
        
        grid_search.fit(X, y)
        
        print(f"  Best F1 Score: {grid_search.best_score_:.4f}")
        print(f"  Best Parameters: {grid_search.best_params_}")
        
        # Get feature importances from best model
        best_model = grid_search.best_estimator_.named_steps['classifier']
        if hasattr(best_model, 'feature_importances_'):
            importances = best_model.feature_importances_
            feature_names = df.drop('disease', axis=1).columns
            sorted_idx = np.argsort(importances)[::-1]
            
            print("\n  Top 5 Important Features:")
            for i in sorted_idx[:5]:
                print(f"    {feature_names[i]}: {importances[i]:.4f}")
        
        return grid_search
    
    def tune_for_robustness(self, X, y):
        """
        Tune models with focus on robustness.
        
        For medical applications, model should be:
        - Robust to small data variations
        - Not overfit to training data
        - Generalize well to new patients
        """
        print("\n2. Robust Models")
        print("-" * 40)
        
        # Random Forest with regularization
        print("\nRandom Forest (Regularized):")
        param_grid = {
            'classifier__n_estimators': [100, 200, 300],
            'classifier__max_depth': [3, 5, 7],
            'classifier__min_samples_leaf': [10, 20, 30],
            'classifier__max_features': ['sqrt', 'log2'],
            'classifier__bootstrap': [True]
        }
        
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', RandomForestClassifier(random_state=42))
        ])
        
        # Use multiple random splits for robustness
        robust_scores = []
        
        for random_state in [42, 123, 456]:
            grid_search = GridSearchCV(
                pipeline,
                param_grid,
                cv=StratifiedKFold(n_splits=5, shuffle=True, random_state=random_state),
                scoring='roc_auc',
                n_jobs=-1
            )
            
            grid_search.fit(X, y)
            robust_scores.append(grid_search.best_score_)
        
        print(f"  Robustness Check (3 different CV splits):")
        print(f"    Scores: {robust_scores}")
        print(f"    Mean: {np.mean(robust_scores):.4f}")
        print(f"    Std: {np.std(robust_scores):.4f}")
        
        return robust_scores
    
    def tune_ensemble_medical(self, X, y):
        """
        Tune ensemble methods for medical diagnosis.
        
        Combining multiple models can improve:
        - Reliability through voting
        - Confidence through probability estimates
        - Robustness through diversity
        """
        print("\n3. Ensemble Methods")
        print("-" * 40)
        
        # Gradient Boosting with careful tuning
        print("\nGradient Boosting:")
        param_grid = {
            'classifier__n_estimators': [50, 100, 150],
            'classifier__learning_rate': [0.01, 0.05, 0.1],
            'classifier__max_depth': [2, 3, 4],
            'classifier__min_samples_leaf': [10, 20],
            'classifier__subsample': [0.8, 0.9]
        }
        
        pipeline = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', GradientBoostingClassifier(random_state=42))
        ])
        
        grid_search = GridSearchCV(
            pipeline,
            param_grid,
            cv=StratifiedKFold(n_splits=5, shuffle=True, random_state=42),
            scoring='roc_auc',
            n_jobs=-1
        )
        
        grid_search.fit(X, y)
        
        print(f"  Best ROC-AUC: {grid_search.best_score_:.4f}")
        print(f"  Best Parameters: {grid_search.best_params_}")
        
        return grid_search
    
    def clinical_validation(self, X, y):
        """
        Perform clinical validation of models.
        
        Clinical validation includes:
        - Sensitivity/Specificity analysis
        - ROC curve analysis
        - Precision-Recall curve (for imbalanced data)
        - Decision curve analysis
        """
        print("\n" + "="*60)
        print("CLINICAL VALIDATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        # Train model
        model = Pipeline([
            ('scaler', StandardScaler()),
            ('classifier', GradientBoostingClassifier(
                n_estimators=100,
                learning_rate=0.1,
                max_depth=3,
                random_state=42
            ))
        ])
        
        model.fit(X_train, y_train)
        
        y_prob = model.predict_proba(X_test)[:, 1]
        y_pred = model.predict(X_test)
        
        # ROC Curve
        fpr, tpr, thresholds = roc_curve(y_test, y_prob)
        
        print("\nROC Curve Analysis:")
        print(f"  False Positive Rate vs True Positive Rate:")
        for i in range(0, len(thresholds), len(thresholds)//5):
            print(f"    Threshold: {thresholds[i]:.2f}, "
                  f"FPR: {fpr[i]:.3f}, TPR: {tpr[i]:.3f}")
        
        # Find optimal threshold (Youden's J statistic)
        j_scores = tpr - fpr
        optimal_idx = np.argmax(j_scores)
        optimal_threshold = thresholds[optimal_idx]
        
        print(f"\nOptimal Threshold (Youden's J):")
        print(f"  Threshold: {optimal_threshold:.3f}")
        print(f"  Sensitivity: {tpr[optimal_idx]:.3f}")
        print(f"  Specificity: {1 - fpr[optimal_idx]:.3f}")
        
        # Precision-Recall Curve
        precision, recall, pr_thresholds = precision_recall_curve(y_test, y_prob)
        
        print("\nPrecision-Recall Analysis:")
        print(f"  Disease Prevalence in Test: {y_test.mean():.2%}")
        print(f"  Best Precision-Recall Balance:")
        
        # Find threshold with best F1
        f1_scores = 2 * (precision * recall) / (precision + recall + 1e-10)
        best_f1_idx = np.argmax(f1_scores)
        
        print(f"    Precision: {precision[best_f1_idx]:.3f}")
        print(f"    Recall: {recall[best_f1_idx]:.3f}")
        print(f"    F1 Score: {f1_scores[best_f1_idx]:.3f}")
        
        # Compare with default threshold
        default_pred = (y_prob >= 0.5).astype(int)
        default_f1 = f1_score(y_test, default_pred)
        
        print(f"\n  Default threshold (0.5) F1: {default_f1:.4f}")
        print(f"  Optimal threshold ({optimal_threshold:.3f}) F1: {f1_scores[best_f1_idx]:.4f}")


# ============================================================================
# SECTION VII: BEST PRACTICES AND GUIDELINES
# ============================================================================

class HyperparameterBestPractices:
    """
    Best practices for hyperparameter tuning in machine learning.
    """
    
    @staticmethod
    def print_guidelines():
        """Print comprehensive guidelines."""
        print("\n" + "="*60)
        print("BEST PRACTICES FOR HYPERPARAMETER TUNING")
        print("="*60)
        
        guidelines = """
1. GENERAL PRINCIPLES
   - Start with simple models and parameters
   - Use cross-validation for reliable estimates
   - Avoid overfitting to validation set
   - Consider computational cost

2. WHEN TO USE EACH METHOD
   
   Manual Tuning:
   - Small parameter space (< 10 combinations)
   - When you understand the domain well
   - For rapid initial experimentation
   
   Grid Search:
   - Small to medium parameter space
   - When you need exhaustive search
   - When computational resources allow
   
   Random Search:
   - Large parameter space
   - When some parameters are continuous
   - When time is limited
   
   Bayesian Optimization:
   - Very large parameter space
   - When evaluation is expensive
   - When you need the best possible result

3. COMMON PITFALLS
   
   a) Data Leakage:
      - Always use proper cross-validation
      - Don't include test data in tuning
      - Apply preprocessing inside CV folds
   
   b) Overfitting to Validation:
      - Use nested cross-validation
      - Limit number of hyperparameter combinations
      - Use held-out test set for final evaluation
   
   c) Computational Waste:
      - Start with coarse grid, then refine
      - Use early stopping when possible
      - Parallelize cross-validation folds

4. MODEL-SPECIFIC GUIDELINES
   
   Logistic Regression:
   - C: Start with wide range, then refine
   - penalty: 'l2' is default, 'l1' for sparsity
   - solver: 'lbfgs' for large data, 'liblinear' for small
   
   Random Forest:
   - n_estimators: More is usually better (but diminishing returns)
   - max_depth: None (unlimited) for complex data, smaller for regularization
   - min_samples_leaf: Increase for noisy data
   
   Gradient Boosting:
   - learning_rate: Lower needs more trees
   - max_depth: Usually 3-7 (shallow trees)
   - subsample: 0.8-0.9 for regularization
   
   SVM:
   - C: Higher for complex decision boundaries
   - kernel: 'rbf' for non-linear, 'linear' for high-dimensional
   - gamma: 'scale' for default, 'auto' for large data

5. EVALUATION METRICS
   
   Classification:
   - Accuracy: Balanced classes
   - ROC-AUC: Imbalanced classes
   - F1 Score: Binary classification with imbalance
   - Precision-Recall AUC: Highly imbalanced
   
   Regression:
   - MSE/RMSE: Sensitive to outliers
   - MAE: Robust to outliers
   - R^2: Explained variance

6. PRODUCTION CONSIDERATIONS
   
   - Monitor for drift in hyperparameters
   - Retrain when performance degrades
   - Consider automated hyperparameter tuning
   - Document the tuning process
   - Version control your models and parameters
"""
        print(guidelines)
    
    @staticmethod
    def create_param_cheatsheet():
        """Create a quick reference cheatsheet for common parameters."""
        print("\n" + "="*60)
        print("HYPERPARAMETER CHEATSHEET")
        print("="*60)
        
        cheatsheet = """
Logistic Regression:
  C:              [0.001, 0.01, 0.1, 1, 10, 100]
  penalty:        ['l1', 'l2']
  solver:         ['lbfgs', 'liblinear', 'saga']
  class_weight:   [None, 'balanced']

Random Forest:
  n_estimators:   [50, 100, 200, 300]
  max_depth:       [3, 5, 7, 10, None]
  min_samples_split: [2, 5, 10]
  min_samples_leaf: [1, 2, 4]
  max_features:  ['sqrt', 'log2', None]

Gradient Boosting:
  n_estimators:  [50, 100, 200]
  learning_rate:  [0.01, 0.05, 0.1, 0.2]
  max_depth:     [2, 3, 4, 5]
  subsample:     [0.7, 0.8, 0.9, 1.0]

SVM:
  C:              [0.01, 0.1, 1, 10, 100]
  kernel:         ['linear', 'rbf', 'poly']
  gamma:          ['scale', 'auto', 0.001, 0.01]

KNN:
  n_neighbors:    [3, 5, 7, 9, 11]
  weights:         ['uniform', 'distance']
  metric:         ['euclidean', 'manhattan']
"""
        print(cheatsheet)


# ============================================================================
# SECTION VIII: TESTING AND VALIDATION
# ============================================================================

def run_tests():
    """
    Run comprehensive tests for hyperparameter tuning functionality.
    """
    print("\n" + "="*60)
    print("RUNNING TESTS")
    print("="*60)
    
    # Generate test data
    X, y, _ = generate_classification_data(n_samples=500)
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    # Test 1: Data Generation
    print("\n1. Testing Data Generation...")
    assert X.shape[0] == 500
    assert y.shape[0] == 500
    assert len(np.unique(y)) == 2
    print("   PASSED")
    
    # Test 2: Manual Tuning
    print("\n2. Testing Manual Tuning...")
    manual_tuner = ManualTuningDemo(model_type='logistic')
    results = manual_tuner.tune_logistic_regression(
        X_train, y_train, X_test, y_test
    )
    assert len(results) > 0
    print("   PASSED")
    
    # Test 3: Systematic Tuning
    print("\n3. Testing Systematic Tuning...")
    systematic_tuner = SystematicTuningDemo()
    grid_result = systematic_tuner.grid_search_demo(X, y, cv=3)
    assert hasattr(grid_result, 'best_params_')
    print("   PASSED")
    
    # Test 4: Banking Example
    print("\n4. Testing Banking Example...")
    banking = BankingCreditScoringExample()
    X_bank, y_bank, df_bank = banking.load_and_preprocess_data()
    lr_result = banking.tune_logistic_regression(X_bank, y_bank)
    assert hasattr(lr_result, 'best_params_')
    print("   PASSED")
    
    # Test 5: Healthcare Example
    print("\n5. Testing Healthcare Example...")
    healthcare = HealthcareDiagnosisExample()
    X_health, y_health, df_health = healthcare.load_and_preprocess_data()
    interpret_result = healthcare.tune_for_interpretability(X_health, y_health)
    assert hasattr(interpret_result, 'best_params_')
    print("   PASSED")
    
    print("\n" + "="*60)
    print("ALL TESTS PASSED")
    print("="*60)


# ============================================================================
# MAIN EXECUTION
# ============================================================================

def main():
    """
    Main execution function demonstrating all hyperparameter tuning methods.
    """
    print("="*60)
    print("HYPERPARAMETER TUNING IMPLEMENTATION")
    print("="*60)
    print("Topic: Hyperparameter Tuning Methods")
    print("Author: AI Assistant")
    print("Date: 06-04-2026")
    print("="*60)
    
    # Generate sample data
    print("\nGenerating sample data...")
    X, y, feature_names = generate_classification_data(n_samples=1000)
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    print(f"Data shape: {X.shape}")
    print(f"Class distribution: {np.bincount(y)}")
    
    # Section 1: Manual Tuning
    print("\n" + "="*60)
    print("SECTION 1: MANUAL HYPERPARAMETER TUNING")
    print("="*60)
    
    manual_tuner = ManualTuningDemo()
    manual_results = manual_tuner.tune_logistic_regression(
        X_train, y_train, X_test, y_test
    )
    
    # Sensitivity Analysis
    print("\nSensitivity Analysis: C parameter")
    sensitivity_results = manual_tuner.sensitivity_analysis(
        X_train, y_train, X_test, y_test,
        'C', [0.001, 0.01, 0.1, 1, 10, 100]
    )
    
    # Section 2: Systematic Tuning
    print("\n" + "="*60)
    print("SECTION 2: SYSTEMATIC HYPERPARAMETER TUNING")
    print("="*60)
    
    systematic_tuner = SystematicTuningDemo()
    
    # Grid Search
    grid_result = systematic_tuner.grid_search_demo(X, y, cv=3)
    
    # Random Search
    random_result = systematic_tuner.random_search_demo(X, y, n_iter=20, cv=3)
    
    # Nested CV
    print("\nNested Cross-Validation:")
    nested_scores = systematic_tuner.nested_cross_validation(X, y)
    
    # Comparison
    comparison = systematic_tuner.comparison_grid_vs_random(X, y)
    
    # Section 3: Banking Example
    print("\n" + "="*60)
    print("SECTION 3: BANKING EXAMPLE (Credit Scoring)")
    print("="*60)
    
    banking = BankingCreditScoringExample()
    X_bank, y_bank, df_bank = banking.load_and_preprocess_data()
    
    # Tune models
    lr_result = banking.tune_logistic_regression(X_bank, y_bank)
    rf_result = banking.tune_random_forest(X_bank, y_bank)
    gb_result = banking.tune_gradient_boosting(X_bank, y_bank)
    
    # Evaluate
    print("\nModel Comparison:")
    models_dict = {
        'Logistic Regression': lr_result.best_estimator_,
        'Random Forest': rf_result.best_estimator_,
        'Gradient Boosting': gb_result.best_estimator_
    }
    eval_results = banking.evaluate_models(X_bank, y_bank, models_dict)
    
    # Business analysis
    banking.business_analysis(eval_results, df_bank)
    
    # Section 4: Healthcare Example
    print("\n" + "="*60)
    print("SECTION 4: HEALTHCARE EXAMPLE (Disease Diagnosis)")
    print("="*60)
    
    healthcare = HealthcareDiagnosisExample()
    X_health, y_health, df_health = healthcare.load_and_preprocess_data()
    
    # Tune for interpretability
    interpret_result = healthcare.tune_for_interpretability(X_health, y_health)
    
    # Tune for robustness
    robust_scores = healthcare.tune_for_robustness(X_health, y_health)
    
    # Tune ensemble
    ensemble_result = healthcare.tune_ensemble_medical(X_health, y_health)
    
    # Clinical validation
    healthcare.clinical_validation(X_health, y_health)
    
    # Section 5: Best Practices
    print("\n" + "="*60)
    print("SECTION 5: BEST PRACTICES")
    print("="*60)
    
    HyperparameterBestPractices.print_guidelines()
    HyperparameterBestPractices.create_param_cheatsheet()
    
    # Section 6: Advanced Methods
    print("\n" + "="*60)
    print("SECTION 6: ADVANCED TUNING METHODS")
    print("="*60)
    
    advanced = AdvancedTuningMethods()
    
    # Bayesian Optimization
    history, best_params, best_score = advanced.bayesian_optimization_simulation(
        X, y, n_iter=10
    )
    
    # Multi-objective
    results, pareto = advanced.multi_objective_tuning(X, y)
    
    # Section 7: Tests
    print("\n" + "="*60)
    print("SECTION 7: TESTING")
    print("="*60)
    
    run_tests()
    
    # Summary
    print("\n" + "="*60)
    print("SUMMARY")
    print("="*60)
    
    summary = """
Hyperparameter Tuning Complete!

Key Takeaways:
1. Always use cross-validation for hyperparameter tuning
2. Start with simple models and parameters
3. Use appropriate evaluation metrics for your problem
4. Consider both model performance and computational cost
5. Apply domain-specific knowledge (banking, healthcare, etc.)
6. Document your tuning process for reproducibility

Methods Demonstrated:
- Manual Tuning: Iterative adjustment based on validation
- Grid Search: Exhaustive search over parameter grid
- Random Search: Random sampling from parameter distributions
- Bayesian Optimization: Intelligent search using prior knowledge
- Multi-objective Tuning: Balancing multiple objectives

Applications:
- Credit Scoring: Banking/Finance domain
- Disease Diagnosis: Healthcare domain
"""
    print(summary)
    
    print("\nExecution completed successfully!")


if __name__ == "__main__":
    main()