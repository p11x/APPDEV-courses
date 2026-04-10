# Topic: Model Ensembling Techniques
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Model Ensembling Techniques

I. INTRODUCTION
    Model ensembling is a powerful machine learning technique that combines
    predictions from multiple models to achieve better performance than
    any individual model. This module covers:
    
    - Bagging (Bootstrap Aggregating)
    - Boosting (Adaptive Boosting, Gradient Boosting)
    - Voting Classifiers (Hard and Soft Voting)
    - Stacking (Stacked Generalization)
    - Blending (Similar to stacking with hold-out data)
    - Ensemble Diversity and Error Correlation
    - Real-world Banking and Healthcare examples
    - When to use ensemble methods

II. CORE_CONCEPTS
    - Bias-Variance Tradeoff
    - Ensemble Learning Theory
    - Diversity vs Accuracy
    - Weak Learners vs Strong Learners
    - Overfitting Prevention with Ensembles

III. IMPLEMENTATION
    Multiple ensemble techniques with detailed implementations

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Credit Scoring Ensemble
    - Healthcare: Disease Diagnosis Ensemble

V. OUTPUT_RESULTS
    Comprehensive output analysis and interpretation

VI. TESTING
    Unit tests and validation procedures

VII. ADVANCED_TOPICS
    - Advanced stacking architectures
    - Error analysis
    - Meta-learning
    - Ensemble selection

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
from sklearn.datasets import make_classification, make_blobs, load_breast_cancer
from sklearn.model_selection import (
    train_test_split, cross_val_score, StratifiedKFold, KFold,
    learning_curve, validation_curve
)
from sklearn.preprocessing import StandardScaler, MinMaxScaler, RobustScaler
from sklearn.linear_model import (
    LogisticRegression, Ridge, Lasso, ElasticNet, LinearRegression
)
from sklearn.ensemble import (
    RandomForestClassifier, RandomForestRegressor, GradientBoostingClassifier,
    GradientBoostingRegressor, AdaBoostClassifier, BaggingClassifier,
    VotingClassifier, StackingClassifier, ExtraTreesClassifier,
    AdaBoostRegressor, BaggingRegressor
)
from sklearn.svm import SVC, SVR
from sklearn.neighbors import KNeighborsClassifier, KNeighborsRegressor
from sklearn.tree import DecisionTreeClassifier, DecisionTreeRegressor
from sklearn.naive_bayes import GaussianNB
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    roc_auc_score, mean_squared_error, mean_absolute_error, r2_score,
    confusion_matrix, classification_report, roc_curve, precision_recall_curve,
    log_loss
)
from sklearn.pipeline import Pipeline
from sklearn.base import BaseEstimator, TransformerMixin, clone
from sklearn.calibration import CalibratedClassifierCV
import time
import warnings
import json
import os
from collections import defaultdict
from itertools import combinations, product
from functools import partial
from copy import deepcopy

warnings.filterwarnings('ignore')

np.random.seed(42)

# ============================================================================
# SECTION I: INTRODUCTION AND DATA GENERATION
# ============================================================================

def generate_classification_data(n_samples=1000, n_features=20, n_informative=10,
                            n_redundant=5, n_classes=2, imbalance_ratio=0.3):
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
    
    noise = np.random.randn(n_samples, 5)
    X = np.hstack([X, noise])
    feature_names.extend([f'noise_feature_{i}' for i in range(5)])
    
    return X, y, feature_names


def generate_regression_data(n_samples=1000, n_features=10, noise=0.1):
    """
    Generate synthetic regression data for demonstration.
    """
    from sklearn.datasets import make_regression
    
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
    
    Features relevant to credit scoring:
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
# SECTION II: BAGGING (BOOTSTRAP AGGREGATING)
# ============================================================================

class BaggingEnsembleDemo:
    """
    Implementation and demonstration of Bagging (Bootstrap Aggregating).
    
    Bagging reduces variance by:
    1. Creating multiple bootstrap samples of training data
    2. Training base models on each bootstrap sample
    3. Aggregating predictions (voting for classification, averaging for regression)
    
    Key concepts:
    - Bootstrap sampling: Random sampling with replacement
    - Parallel model training
    - Voting/Averaging aggregation
    """
    
    def __init__(self):
        self.ensemble = None
        self.base_models = []
        self.bootstrap_samples = []
        
    def demonstrate_bagging_classification(self, X, y):
        """
        Demonstrate bagging for classification.
        
        Bagging is particularly effective for high-variance models
        like decision trees. It reduces overfitting and improves
        stability.
        """
        print("\n" + "="*60)
        print("BAGGING CLASSIFICATION DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        print(f"Training set size: {X_train.shape[0]}")
        print(f"Test set size: {X_test.shape[0]}")
        
        base_tree = DecisionTreeClassifier(random_state=42)
        
        print("\n--- Single Decision Tree Performance ---")
        base_tree.fit(X_train, y_train)
        
        train_pred = base_tree.predict(X_train)
        test_pred = base_tree.predict(X_test)
        
        train_acc = accuracy_score(y_train, train_pred)
        test_acc = accuracy_score(y_test, test_pred)
        
        print(f"Training Accuracy: {train_acc:.4f}")
        print(f"Test Accuracy: {test_acc:.4f}")
        
        n_estimators_list = [10, 50, 100, 200]
        
        print("\n--- Bagging with Different Numbers of Trees ---")
        
        results = []
        
        for n_estimators in n_estimators_list:
            bagging = BaggingClassifier(
                estimator=DecisionTreeClassifier(random_state=42),
                n_estimators=n_estimators,
                bootstrap=True,
                bootstrap_samples=0.8,
                random_state=42,
                n_jobs=-1
            )
            
            start_time = time.time()
            bagging.fit(X_train, y_train)
            train_time = time.time() - start_time
            
            train_pred = bagging.predict(X_train)
            test_pred = bagging.predict(X_test)
            
            train_acc = accuracy_score(y_train, train_pred)
            test_acc = accuracy_score(y_test, test_pred)
            
            results.append({
                'n_estimators': n_estimators,
                'train_accuracy': train_acc,
                'test_accuracy': test_acc,
                'train_time': train_time
            })
            
            print(f"n_estimators={n_estimators:3d}: Train Acc={train_acc:.4f}, "
                  f"Test Acc={test_acc:.4f}, Time={train_time:.2f}s")
        
        return results
    
    def demonstrate_bagging_regression(self, X, y):
        """
        Demonstrate bagging for regression.
        
        Bagging for regression:
        - Uses bootstrap samples
        - Averages predictions from all models
        - Reduces variance of unstable models
        """
        print("\n" + "="*60)
        print("BAGGING REGRESSION DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42
        )
        
        base_tree = DecisionTreeRegressor(random_state=42, max_depth=10)
        
        print("\n--- Single Decision Tree Performance ---")
        base_tree.fit(X_train, y_train)
        
        train_pred = base_tree.predict(X_train)
        test_pred = base_tree.predict(X_test)
        
        train_rmse = np.sqrt(mean_squared_error(y_train, train_pred))
        test_rmse = np.sqrt(mean_squared_error(y_test, test_pred))
        
        print(f"Training RMSE: {train_rmse:.4f}")
        print(f"Test RMSE: {test_rmse:.4f}")
        
        n_estimators_list = [10, 50, 100]
        
        print("\n--- Bagging Regressor with Different Numbers of Trees ---")
        
        results = []
        
        for n_estimators in n_estimators_list:
            bagging = BaggingRegressor(
                estimator=DecisionTreeRegressor(random_state=42, max_depth=10),
                n_estimators=n_estimators,
                bootstrap=True,
                bootstrap_samples=0.8,
                random_state=42,
                n_jobs=-1
            )
            
            start_time = time.time()
            bagging.fit(X_train, y_train)
            train_time = time.time() - start_time
            
            train_pred = bagging.predict(X_train)
            test_pred = bagging.predict(X_test)
            
            train_rmse = np.sqrt(mean_squared_error(y_train, train_pred))
            test_rmse = np.sqrt(mean_squared_error(y_test, test_pred))
            
            results.append({
                'n_estimators': n_estimators,
                'train_rmse': train_rmse,
                'test_rmse': test_rmse,
                'train_time': train_time
            })
            
            print(f"n_estimators={n_estimators:3d}: Train RMSE={train_rmse:.4f}, "
                  f"Test RMSE={test_rmse:.4f}, Time={train_time:.2f}s")
        
        return results
    
    def demonstrate_random_subspace_method(self, X, y):
        """
        Demonstrate Random Subspace Method (Feature Bagging).
        
        Unlike bagging which samples instances, random subspace samples features.
        This is useful for high-dimensional data.
        """
        print("\n" + "="*60)
        print("RANDOM SUBSPACE METHOD (FEATURE BAGGING)")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42
        )
        
        print("\n--- Random Forest (Feature Bagging + Instance Bagging) ---")
        
        rf = RandomForestClassifier(n_estimators=100, random_state=42, n_jobs=-1)
        rf.fit(X_train, y_train)
        
        test_pred = rf.predict(X_test)
        test_prob = rf.predict_proba(X_test)[:, 1]
        
        test_acc = accuracy_score(y_test, test_pred)
        test_auc = roc_auc_score(y_test, test_prob)
        
        print(f"Test Accuracy: {test_acc:.4f}")
        print(f"Test ROC-AUC: {test_auc:.4f}")
        
        print("\n--- Feature Importance (Top 10) ---")
        feature_importance = rf.feature_importances_
        indices = np.argsort(feature_importance)[::-1]
        
        for i in range(min(10, len(indices))):
            print(f"  Feature {indices[i]}: {feature_importance[indices[i]]:.4f}")
        
        return rf
    
    def analyze_bagging_variance_reduction(self, X, y):
        """
        Analyze how bagging reduces prediction variance.
        """
        print("\n" + "="*60)
        print("BAGGING VARIANCE REDUCTION ANALYSIS")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42
        )
        
        n_bootstrap = 50
        tree_predictions = []
        bagging_predictions = []
        
        for i in range(n_bootstrap):
            indices = np.random.choice(len(X_train), len(X_train), replace=True)
            X_boot = X_train[indices]
            y_boot = y_train[indices]
            
            tree = DecisionTreeClassifier(random_state=i)
            tree.fit(X_boot, y_boot)
            tree_predictions.append(tree.predict_proba(X_test)[:, 1])
            
            ensemble = BaggingClassifier(
                estimator=DecisionTreeClassifier(random_state=i),
                n_estimators=10,
                random_state=i,
                n_jobs=-1
            )
            ensemble.fit(X_boot, y_boot)
            bagging_predictions.append(ensemble.predict_proba(X_test)[:, 1])
        
        tree_predictions = np.array(tree_predictions)
        bagging_predictions = np.array(bagging_predictions)
        
        tree_var = np.var(tree_predictions, axis=0).mean()
        bagging_var = np.var(bagging_predictions, axis=0).mean()
        
        tree_std = np.std(tree_predictions, axis=0).mean()
        bagging_std = np.std(bagging_predictions, axis=0).mean()
        
        print(f"Average Prediction Variance (Single Tree): {tree_var:.6f}")
        print(f"Average Prediction Variance (Bagging): {bagging_var:.6f}")
        print(f"Variance Reduction: {(1 - bagging_var/tree_var)*100:.2f}%")
        
        print(f"\nAverage Prediction Std (Single Tree): {tree_std:.4f}")
        print(f"Average Prediction Std (Bagging): {bagging_std:.4f}")
        
        return tree_var, bagging_var


# ============================================================================
# SECTION III: BOOSTING
# ============================================================================

class BoostingEnsembleDemo:
    """
    Implementation and demonstration of Boosting methods.
    
    Boosting reduces bias by:
    1. Training weak learners sequentially
    2. Focusing on misclassified instances
    3. Combining weighted predictions
    
    Key algorithms:
    - AdaBoost (Adaptive Boosting)
    - Gradient Boosting
    - XGBoost-style boosting
    """
    
    def __init__(self):
        self.models = []
        self.weights = []
        
    def demonstrate_adaboost(self, X, y):
        """
        Demonstrate AdaBoost (Adaptive Boosting).
        
        AdaBoost:
        - Trains weak learners sequentially
        - Increases weights of misclassified samples
        - Combines with weighted voting
        """
        print("\n" + "="*60)
        print("ADABOOST CLASSIFICATION DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        print("\n--- AdaBoost with Decision Stumps ---")
        
        n_estimators_list = [10, 50, 100, 200]
        
        results = []
        
        for n_estimators in n_estimators_list:
            ada = AdaBoostClassifier(
                estimator=DecisionTreeClassifier(max_depth=1),
                n_estimators=n_estimators,
                learning_rate=1.0,
                random_state=42,
                algorithm='SAMME'
            )
            
            ada.fit(X_train, y_train)
            
            train_pred = ada.predict(X_train)
            test_pred = ada.predict(X_test)
            
            train_acc = accuracy_score(y_train, train_pred)
            test_acc = accuracy_score(y_test, test_pred)
            
            results.append({
                'n_estimators': n_estimators,
                'train_accuracy': train_acc,
                'test_accuracy': test_acc
            })
            
            print(f"n_estimators={n_estimators:3d}: Train Acc={train_acc:.4f}, "
                  f"Test Acc={test_acc:.4f}")
        
        print("\n--- AdaBoost Feature Importance ---")
        
        best_ada = AdaBoostClassifier(
            estimator=DecisionTreeClassifier(max_depth=1),
            n_estimators=100,
            random_state=42
        )
        best_ada.fit(X_train, y_train)
        
        feature_importance = best_ada.feature_importances_
        indices = np.argsort(feature_importance)[::-1]
        
        print("Top 10 Important Features:")
        for i in range(min(10, len(indices))):
            print(f"  Feature {indices[i]}: {feature_importance[indices[i]]:.4f}")
        
        return results
    
    def demonstrate_gradient_boosting(self, X, y):
        """
        Demonstrate Gradient Boosting.
        
        Gradient Boosting:
        - Uses gradient descent to minimize loss
        - Fits each tree to residuals
        - Creates additive model
        """
        print("\n" + "="*60)
        print("GRADIENT BOOSTING CLASSIFICATION DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        print("\n--- Gradient Boosting with Different Parameters ---")
        
        param_combinations = [
            {'n_estimators': 50, 'learning_rate': 0.1, 'max_depth': 3},
            {'n_estimators': 100, 'learning_rate': 0.1, 'max_depth': 3},
            {'n_estimators': 100, 'learning_rate': 0.05, 'max_depth': 5},
            {'n_estimators': 200, 'learning_rate': 0.01, 'max_depth': 3},
        ]
        
        results = []
        
        for params in param_combinations:
            gb = GradientBoostingClassifier(
                n_estimators=params['n_estimators'],
                learning_rate=params['learning_rate'],
                max_depth=params['max_depth'],
                subsample=0.8,
                random_state=42
            )
            
            start_time = time.time()
            gb.fit(X_train, y_train)
            train_time = time.time() - start_time
            
            train_pred = gb.predict(X_train)
            test_pred = gb.predict(X_test)
            test_prob = gb.predict_proba(X_test)[:, 1]
            
            train_acc = accuracy_score(y_train, train_pred)
            test_acc = accuracy_score(y_test, test_pred)
            test_auc = roc_auc_score(y_test, test_prob)
            
            results.append({
                **params,
                'train_accuracy': train_acc,
                'test_accuracy': test_acc,
                'test_auc': test_auc,
                'train_time': train_time
            })
            
            print(f"n_est={params['n_estimators']:3d}, lr={params['learning_rate']:.2f}, "
                  f"depth={params['max_depth']}: Test Acc={test_acc:.4f}, "
                  f"AUC={test_auc:.4f}")
        
        return results
    
    def demonstrate_gradient_boosting_regression(self, X, y):
        """
        Demonstrate Gradient Boosting for regression.
        """
        print("\n" + "="*60)
        print("GRADIENT BOOSTING REGRESSION DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42
        )
        
        print("\n--- Gradient Boosting Regressor ---")
        
        gb = GradientBoostingRegressor(
            n_estimators=100,
            learning_rate=0.1,
            max_depth=4,
            subsample=0.8,
            random_state=42
        )
        
        gb.fit(X_train, y_train)
        
        train_pred = gb.predict(X_train)
        test_pred = gb.predict(X_test)
        
        train_rmse = np.sqrt(mean_squared_error(y_train, train_pred))
        test_rmse = np.sqrt(mean_squared_error(y_test, test_pred))
        test_r2 = r2_score(y_test, test_pred)
        
        print(f"Training RMSE: {train_rmse:.4f}")
        print(f"Test RMSE: {test_rmse:.4f}")
        print(f"Test R²: {test_r2:.4f}")
        
        return gb
    
    def compare_boosting_algorithms(self, X, y):
        """
        Compare different boosting algorithms.
        """
        print("\n" + "="*60)
        print("BOOSTING ALGORITHMS COMPARISON")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        algorithms = {
            'AdaBoost': AdaBoostClassifier(
                n_estimators=100,
                learning_rate=0.1,
                random_state=42
            ),
            'GradientBoosting': GradientBoostingClassifier(
                n_estimators=100,
                learning_rate=0.1,
                max_depth=3,
                random_state=42
            ),
            'RandomForest': RandomForestClassifier(
                n_estimators=100,
                max_depth=10,
                random_state=42,
                n_jobs=-1
            )
        }
        
        results = []
        
        for name, model in algorithms.items():
            start_time = time.time()
            model.fit(X_train, y_train)
            train_time = time.time() - start_time
            
            test_pred = model.predict(X_test)
            test_prob = model.predict_proba(X_test)[:, 1]
            
            test_acc = accuracy_score(y_test, test_pred)
            test_auc = roc_auc_score(y_test, test_prob)
            
            results.append({
                'algorithm': name,
                'test_accuracy': test_acc,
                'test_auc': test_auc,
                'train_time': train_time
            })
            
            print(f"{name:20s}: Test Acc={test_acc:.4f}, "
                  f"AUC={test_auc:.4f}, Time={train_time:.2f}s")
        
        return results
    
    def analyze_learning_rate_impact(self, X, y):
        """
        Analyze impact of learning rate on gradient boosting.
        """
        print("\n" + "="*60)
        print("LEARNING RATE IMPACT ANALYSIS")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        learning_rates = [0.001, 0.01, 0.05, 0.1, 0.2, 0.5, 1.0]
        
        print("\n--- Impact of Learning Rate ---")
        
        results = []
        
        for lr in learning_rates:
            gb = GradientBoostingClassifier(
                n_estimators=100,
                learning_rate=lr,
                max_depth=3,
                random_state=42
            )
            
            gb.fit(X_train, y_train)
            
            train_pred = gb.predict(X_train)
            test_pred = gb.predict(X_test)
            test_prob = gb.predict_proba(X_test)[:, 1]
            
            train_acc = accuracy_score(y_train, train_pred)
            test_acc = accuracy_score(y_test, test_pred)
            test_auc = roc_auc_score(y_test, test_prob)
            
            results.append({
                'learning_rate': lr,
                'train_accuracy': train_acc,
                'test_accuracy': test_acc,
                'test_auc': test_auc
            })
            
            print(f"Learning Rate={lr:.3f}: Train Acc={train_acc:.4f}, "
                  f"Test Acc={test_acc:.4f}, AUC={test_auc:.4f}")
        
        return results


# ============================================================================
# SECTION IV: VOTING CLASSIFIERS
# ============================================================================

class VotingEnsembleDemo:
    """
    Implementation and demonstration of Voting Classifiers.
    
    Voting combines predictions from multiple models:
    - Hard Voting: Majority voting
    - Soft Voting: Probability averaging
    - Weighted Voting: Weight by performance
    """
    
    def __init__(self):
        self.voting_clf = None
        
    def demonstrate_hard_voting(self, X, y):
        """
        Demonstrate Hard Voting.
        
        Hard voting uses majority vote:
        - Each model gets one vote
        - Class with most votes wins
        """
        print("\n" + "="*60)
        print("HARD VOTING CLASSIFIER DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        print("\n--- Individual Model Performance ---")
        
        models = {
            'Logistic Regression': LogisticRegression(max_iter=1000, random_state=42),
            'Decision Tree': DecisionTreeClassifier(max_depth=5, random_state=42),
            'KNN': KNeighborsClassifier(n_neighbors=5),
            'SVM': SVC(random_state=42),
            'Naive Bayes': GaussianNB()
        }
        
        individual_results = []
        
        for name, model in models.items():
            model.fit(X_train, y_train)
            test_pred = model.predict(X_test)
            test_acc = accuracy_score(y_test, test_pred)
            
            individual_results.append({
                'model': name,
                'test_accuracy': test_acc
            })
            
            print(f"{name:20s}: Test Accuracy = {test_acc:.4f}")
        
        print("\n--- Hard Voting Ensemble ---")
        
        voting = VotingClassifier(
            estimators=[
                ('lr', LogisticRegression(max_iter=1000, random_state=42)),
                ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
                ('knn', KNeighborsClassifier(n_neighbors=5)),
                ('nb', GaussianNB())
            ],
            voting='hard',
            n_jobs=-1
        )
        
        voting.fit(X_train, y_train)
        
        test_pred = voting.predict(X_test)
        test_acc = accuracy_score(y_test, test_pred)
        
        print(f"Hard Voting Accuracy: {test_acc:.4f}")
        
        return individual_results
    
    def demonstrate_soft_voting(self, X, y):
        """
        Demonstrate Soft Voting.
        
        Soft voting averages probabilities:
        - Get probability from each model
        - Average probabilities
        - Select class with highest average
        - Better than hard voting when models are confident
        """
        print("\n" + "="*60)
        print("SOFT VOTING CLASSIFIER DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        print("\n--- Soft Voting Ensemble ---")
        
        voting = VotingClassifier(
            estimators=[
                ('lr', LogisticRegression(max_iter=1000, random_state=42)),
                ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
                ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
                ('knn', KNeighborsClassifier(n_neighbors=5)),
                ('nb', GaussianNB())
            ],
            voting='soft',
            n_jobs=-1
        )
        
        voting.fit(X_train, y_train)
        
        test_pred = voting.predict(X_test)
        test_prob = voting.predict_proba(X_test)[:, 1]
        
        test_acc = accuracy_score(y_test, test_pred)
        test_auc = roc_auc_score(y_test, test_prob)
        test_log_loss = log_loss(y_test, voting.predict_proba(X_test))
        
        print(f"Soft Voting Accuracy: {test_acc:.4f}")
        print(f"Soft Voting AUC: {test_auc:.4f}")
        print(f"Soft Voting Log Loss: {test_log_loss:.4f}")
        
        print("\n--- Comparison: Hard vs Soft Voting ---")
        
        voting_hard = VotingClassifier(
            estimators=[
                ('lr', LogisticRegression(max_iter=1000, random_state=42)),
                ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
                ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
                ('knn', KNeighborsClassifier(n_neighbors=5)),
                ('nb', GaussianNB())
            ],
            voting='hard',
            n_jobs=-1
        )
        
        voting_hard.fit(X_train, y_train)
        
        hard_pred = voting_hard.predict(X_test)
        hard_acc = accuracy_score(y_test, hard_pred)
        
        print(f"Hard Voting Accuracy: {hard_acc:.4f}")
        print(f"Soft Voting Accuracy: {test_acc:.4f}")
        print(f"Improvement: {(test_acc - hard_acc)*100:.2f}%")
        
        return test_acc
    
    def demonstrate_weighted_voting(self, X, y):
        """
        Demonstrate Weighted Voting.
        
        Weight models by their performance:
        - Better models get more weight
        - Can use cross-validation scores
        """
        print("\n" + "="*60)
        print("WEIGHTED VOTING CLASSIFIER DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        print("\n--- Calculate Weights via Cross-Validation ---")
        
        base_models = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42)),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
            ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
            ('nb', GaussianNB())
        ]
        
        cv_scores = []
        
        for name, model in base_models:
            scores = cross_val_score(model, X_train, y_train, cv=5, scoring='accuracy')
            cv_scores.append(scores.mean())
            print(f"{name}: CV Accuracy = {scores.mean():.4f}")
        
        weights = [score / sum(cv_scores) for score in cv_scores]
        
        print("\n--- Weighted Soft Voting ---")
        
        voting = VotingClassifier(
            estimators=base_models,
            voting='soft',
            weights=weights,
            n_jobs=-1
        )
        
        voting.fit(X_train, y_train)
        
        test_pred = voting.predict(X_test)
        test_prob = voting.predict_proba(X_test)[:, 1]
        
        test_acc = accuracy_score(y_test, test_pred)
        test_auc = roc_auc_score(y_test, test_prob)
        
        print(f"Weighted Voting Accuracy: {test_acc:.4f}")
        print(f"Weighted Voting AUC: {test_auc:.4f}")
        
        return test_acc
    
    def analyze_voting_diversity(self, X, y):
        """
        Analyze the diversity of voting ensemble members.
        """
        print("\n" + "="*60)
        print("VOTING ENSEMBLE DIVERSITY ANALYSIS")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        models = [
            ('Logistic Regression', LogisticRegression(max_iter=1000, random_state=42)),
            ('Decision Tree', DecisionTreeClassifier(max_depth=5, random_state=42)),
            ('Random Forest', RandomForestClassifier(n_estimators=50, random_state=42)),
            ('KNN', KNeighborsClassifier(n_neighbors=5)),
            ('Naive Bayes', GaussianNB())
        ]
        
        predictions = []
        
        for name, model in models:
            model.fit(X_train, y_train)
            pred = model.predict(X_test)
            predictions.append(pred)
        
        predictions = np.array(predictions)
        
        print("\n--- Pairwise Agreement Matrix ---")
        
        n_models = len(models)
        
        for i in range(n_models):
            for j in range(i+1, n_models):
                agreement = (predictions[i] == predictions[j]).mean()
                print(f"{models[i][0][:10]} vs {models[j][0][:10]}: {agreement:.4f}")


# ============================================================================
# SECTION V: STACKING
# ============================================================================

class StackingEnsembleDemo:
    """
    Implementation and demonstration of Stacking (Stacked Generalization).
    
    Stacking uses:
    - Base models for initial predictions
    - Meta-learner to combine base predictions
    - Can use original features + base predictions
    """
    
    def __init__(self):
        self.stacking_clf = None
        
    def demonstrate_basic_stacking(self, X, y):
        """
        Demonstrate basic stacking.
        
        Basic Stacking:
        1. Train base models on training data
        2. Get cross-validated predictions
        3. Train meta-learner on base predictions
        """
        print("\n" + "="*60)
        print("BASIC STACKING CLASSIFIER DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        print("\n--- Stacking with Multiple Base Models ---")
        
        base_estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42)),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
            ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
            ('knn', KNeighborsClassifier(n_neighbors=5)),
            ('nb', GaussianNB())
        ]
        
        stacking = StackingClassifier(
            estimators=base_estimators,
            final_estimator=LogisticRegression(random_state=42),
            cv=5,
            stack_method='predict_proba',
            n_jobs=-1
        )
        
        stacking.fit(X_train, y_train)
        
        test_pred = stacking.predict(X_test)
        test_prob = stacking.predict_proba(X_test)[:, 1]
        
        test_acc = accuracy_score(y_test, test_pred)
        test_auc = roc_auc_score(y_test, test_prob)
        
        print(f"Stacking Accuracy: {test_acc:.4f}")
        print(f"Stacking AUC: {test_auc:.4f}")
        
        print("\n--- Using Original Features in Meta-Learner ---")
        
        stacking_with_features = StackingClassifier(
            estimators=base_estimators,
            final_estimator=LogisticRegression(random_state=42),
            cv=5,
            stack_method='predict_proba',
            passthrough=True,
            n_jobs=-1
        )
        
        stacking_with_features.fit(X_train, y_train)
        
        test_pred = stacking_with_features.predict(X_test)
        test_prob = stacking_with_features.predict_proba(X_test)[:, 1]
        
        test_acc_feat = accuracy_score(y_test, test_pred)
        test_auc_feat = roc_auc_score(y_test, test_prob)
        
        print(f"Stacking with Features Accuracy: {test_acc_feat:.4f}")
        print(f"Stacking with Features AUC: {test_auc_feat:.4f}")
        
        return test_acc
    
    def demonstrate_stacking_regression(self, X, y):
        """
        Demonstrate stacking for regression.
        """
        print("\n" + "="*60)
        print("STACKING REGRESSOR DEMONSTRATION")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42
        )
        
        print("\n--- Stacking Regressor ---")
        
        from sklearn.ensemble import StackingRegressor
        
        base_estimators = [
            ('lr', LinearRegression()),
            ('dt', DecisionTreeRegressor(max_depth=5, random_state=42)),
            ('rf', RandomForestRegressor(n_estimators=50, random_state=42)),
            ('ridge', Ridge(alpha=1.0))
        ]
        
        stacking = StackingRegressor(
            estimators=base_estimators,
            final_estimator=Ridge(alpha=1.0),
            cv=5,
            n_jobs=-1
        )
        
        stacking.fit(X_train, y_train)
        
        train_pred = stacking.predict(X_train)
        test_pred = stacking.predict(X_test)
        
        train_rmse = np.sqrt(mean_squared_error(y_train, train_pred))
        test_rmse = np.sqrt(mean_squared_error(y_test, test_pred))
        test_r2 = r2_score(y_test, test_pred)
        
        print(f"Training RMSE: {train_rmse:.4f}")
        print(f"Test RMSE: {test_rmse:.4f}")
        print(f"Test R²: {test_r2:.4f}")
        
        return stacking
    
    def compare_stacking_variants(self, X, y):
        """
        Compare different stacking configurations.
        """
        print("\n" + "="*60)
        print("STACKING VARIANTS COMPARISON")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        base_estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42)),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
            ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
            ('nb', GaussianNB())
        ]
        
        variants = [
            ('LR Meta-Learner', LogisticRegression(max_iter=1000, random_state=42)),
            ('RF Meta-Learner', RandomForestClassifier(n_estimators=50, random_state=42)),
            ('DT Meta-Learner', DecisionTreeClassifier(max_depth=3, random_state=42))
        ]
        
        results = []
        
        for name, meta_learner in variants:
            stacking = StackingClassifier(
                estimators=deepcopy(base_estimators),
                final_estimator=meta_learner,
                cv=5,
                stack_method='predict_proba',
                n_jobs=-1
            )
            
            start_time = time.time()
            stacking.fit(X_train, y_train)
            train_time = time.time() - start_time
            
            test_pred = stacking.predict(X_test)
            test_prob = stacking.predict_proba(X_test)[:, 1]
            
            test_acc = accuracy_score(y_test, test_pred)
            test_auc = roc_auc_score(y_test, test_prob)
            
            results.append({
                'variant': name,
                'accuracy': test_acc,
                'auc': test_auc,
                'time': train_time
            })
            
            print(f"{name:20s}: Acc={test_acc:.4f}, AUC={test_auc:.4f}, "
                  f"Time={train_time:.2f}s")
        
        return results
    
    def analyze_base_model_contributions(self, X, y):
        """
        Analyze contributions of each base model to stacking.
        """
        print("\n" + "="*60)
        print("BASE MODEL CONTRIBUTIONS ANALYSIS")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        base_estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42)),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
            ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
            ('nb', GaussianNB())
        ]
        
        print("\n--- Individual Base Model Performance ---")
        
        base_performance = []
        
        for name, model in base_estimators:
            model.fit(X_train, y_train)
            test_pred = model.predict(X_test)
            test_acc = accuracy_score(y_test, test_pred)
            
            base_performance.append({
                'model': name,
                'accuracy': test_acc
            })
            
            print(f"{name}: Test Accuracy = {test_acc:.4f}")
        
        print("\n--- Stacking Performance ---")
        
        stacking = StackingClassifier(
            estimators=deepcopy(base_estimators),
            final_estimator=LogisticRegression(max_iter=1000, random_state=42),
            cv=5,
            stack_method='predict_proba',
            n_jobs=-1
        )
        
        stacking.fit(X_train, y_train)
        
        test_pred = stacking.predict(X_test)
        test_acc = accuracy_score(y_test, test_pred)
        
        print(f"Stacking Accuracy: {test_acc:.4f}")
        
        improvement = test_acc - max([p['accuracy'] for p in base_performance])
        print(f"Improvement over best base model: {improvement:.4f}")


# ============================================================================
# SECTION VI: BLENDING
# ============================================================================

class BlendingEnsembleDemo:
    """
    Implementation and demonstration of Blending.
    
    Blending is similar to stacking but:
    - Uses hold-out validation set instead of cross-validation
    - Simpler but less robust
    - Faster for large datasets
    """
    
    def __init__(self):
        self.blend_predictions = None
        
    def demonstrate_blending(self, X, y):
        """
        Demonstrate blending.
        
        Blending Steps:
        1. Split training data into train and validation
        2. Train base models on training set
        3. Predict on validation set (hold-out)
        4. Use these predictions as features for meta-learner
        5. Train base models on full training set
        """
        print("\n" + "="*60)
        print("BLENDING ENSEMBLE DEMONSTRATION")
        print("="*60)
        
        X_train_full, X_test, y_train_full, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        X_train, X_val, y_train, y_val = train_test_split(
            X_train_full, y_train_full, test_size=0.2, random_state=42, stratify=y
        )
        
        print(f"Training set: {X_train.shape[0]} samples")
        print(f"Validation set: {X_val.shape[0]} samples")
        print(f"Test set: {X_test.shape[0]} samples")
        
        base_estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42)),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
            ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
            ('nb', GaussianNB())
        ]
        
        print("\n--- Step 1: Train Base Models on Training Set ---")
        
        val_predictions = []
        
        for name, model in base_estimators:
            model.fit(X_train, y_train)
            pred = model.predict_proba(X_val)[:, 1].reshape(-1, 1)
            val_predictions.append(pred)
        
        val_features = np.hstack(val_predictions)
        
        print(f"Validation features shape: {val_features.shape}")
        
        print("\n--- Step 2: Train Meta-Learner on Validation Predictions ---")
        
        meta_learner = LogisticRegression(max_iter=1000, random_state=42)
        meta_learner.fit(val_features, y_val)
        
        print("Meta-learner trained successfully")
        
        print("\n--- Step 3: Retrain Base Models on Full Training Set ---")
        
        test_predictions = []
        
        for name, base_model in base_estimators:
            model = clone(base_model[1])
            model.fit(X_train_full, y_train_full)
            pred = model.predict_proba(X_test)[:, 1].reshape(-1, 1)
            test_predictions.append(pred)
        
        test_features = np.hstack(test_predictions)
        
        print("\n--- Step 4: Final Predictions ---")
        
        final_pred = meta_learner.predict(test_features)
        final_prob = meta_learner.predict_proba(test_features)[:, 1]
        
        test_acc = accuracy_score(y_test, final_pred)
        test_auc = roc_auc_score(y_test, final_prob)
        
        print(f"Blending Accuracy: {test_acc:.4f}")
        print(f"Blending AUC: {test_auc:.4f}")
        
        return test_acc
    
    def compare_stacking_vs_blending(self, X, y):
        """
        Compare stacking and blending approaches.
        """
        print("\n" + "="*60)
        print("STACKING vs BLENDING COMPARISON")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        base_estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42)),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42)),
            ('rf', RandomForestClassifier(n_estimators=50, random_state=42)),
            ('nb', GaussianNB())
        ]
        
        print("\n--- Stacking (Cross-Validation) ---")
        
        stacking = StackingClassifier(
            estimators=deepcopy(base_estimators),
            final_estimator=LogisticRegression(max_iter=1000, random_state=42),
            cv=5,
            stack_method='predict_proba',
            n_jobs=-1
        )
        
        start_time = time.time()
        stacking.fit(X_train, y_train)
        stacking_time = time.time() - start_time
        
        stacking_pred = stacking.predict(X_test)
        stacking_prob = stacking.predict_proba(X_test)[:, 1]
        
        stacking_acc = accuracy_score(y_test, stacking_pred)
        stacking_auc = roc_auc_score(y_test, stacking_prob)
        
        print(f"Accuracy: {stacking_acc:.4f}, AUC: {stacking_auc:.4f}")
        print(f"Time: {stacking_time:.2f}s")
        
        print("\n--- Blending (Hold-Out) ---")
        
        X_train2, X_val, y_train2, y_val = train_test_split(
            X_train, y_train, test_size=0.2, random_state=42, stratify=y
        )
        
        val_predictions = []
        
        for name, model in base_estimators:
            model.fit(X_train2, y_train2)
            pred = model.predict_proba(X_val)[:, 1].reshape(-1, 1)
            val_predictions.append(pred)
        
        val_features = np.hstack(val_predictions)
        
        meta_learner = LogisticRegression(max_iter=1000, random_state=42)
        meta_learner.fit(val_features, y_val)
        
        test_predictions = []
        
        for name, model in base_estimators:
            model = clone(model[1])
            model.fit(X_train, y_train)
            pred = model.predict_proba(X_test)[:, 1].reshape(-1, 1)
            test_predictions.append(pred)
        
        test_features = np.hstack(test_predictions)
        
        start_time = time.time()
        blending_pred = meta_learner.predict(test_features)
        blending_prob = meta_learner.predict_proba(test_features)[:, 1]
        blending_time = time.time() - start_time
        
        blending_acc = accuracy_score(y_test, blending_pred)
        blending_auc = roc_auc_score(y_test, blending_prob)
        
        print(f"Accuracy: {blending_acc:.4f}, AUC: {blending_auc:.4f}")
        print(f"Time: {blending_time:.2f}s")
        
        print("\n--- Comparison Summary ---")
        print(f"Stacking: Acc={stacking_acc:.4f}, AUC={stacking_auc:.4f}, Time={stacking_time:.2f}s")
        print(f"Blending: Acc={blending_acc:.4f}, AUC={blending_auc:.4f}, Time={blending_time:.2f}s")


# ============================================================================
# SECTION VII: BANKING EXAMPLE (CREDIT SCORING)
# ============================================================================

class BankingCreditScoringExample:
    """
    Comprehensive banking/finance example using ensemble methods.
    
    Application: Credit Scoring
    - Predict probability of loan default
    - Combine multiple models for better predictions
    - Use ensemble for risk assessment
    """
    
    def __init__(self):
        self.models = {}
        self.results = {}
        
    def load_and_preprocess_data(self):
        """Load and preprocess banking data."""
        print("\n" + "="*60)
        print("BANKING EXAMPLE: Credit Scoring with Ensembles")
        print("="*60)
        
        df = load_banking_data()
        
        print(f"Dataset shape: {df.shape}")
        print(f"Class distribution:\n{df['default'].value_counts()}")
        print(f"Default rate: {df['default'].mean():.2%}")
        
        X = df.drop('default', axis=1)
        y = df['default']
        
        scaler = StandardScaler()
        X_scaled = scaler.fit_transform(X)
        
        return X_scaled, y, df
    
    def train_individual_models(self, X, y):
        """Train individual models for comparison."""
        print("\n--- Individual Model Performance ---")
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        models = {
            'Logistic Regression': LogisticRegression(
                max_iter=1000, random_state=42, class_weight='balanced'
            ),
            'Decision Tree': DecisionTreeClassifier(
                max_depth=5, random_state=42, class_weight='balanced'
            ),
            'Random Forest': RandomForestClassifier(
                n_estimators=100, max_depth=10, random_state=42,
                class_weight='balanced', n_jobs=-1
            ),
            'Gradient Boosting': GradientBoostingClassifier(
                n_estimators=100, learning_rate=0.1, max_depth=3, random_state=42
            ),
            'AdaBoost': AdaBoostClassifier(
                n_estimators=100, learning_rate=0.1, random_state=42
            )
        }
        
        results = []
        
        for name, model in models.items():
            model.fit(X_train, y_train)
            
            y_pred = model.predict(X_test)
            y_prob = model.predict_proba(X_test)[:, 1]
            
            accuracy = accuracy_score(y_test, y_pred)
            precision = precision_score(y_test, y_pred)
            recall = recall_score(y_test, y_pred)
            auc = roc_auc_score(y_test, y_prob)
            
            results.append({
                'model': name,
                'accuracy': accuracy,
                'precision': precision,
                'recall': recall,
                'auc': auc
            })
            
            print(f"{name:20s}: Acc={accuracy:.4f}, Prec={precision:.4f}, "
                  f"Rec={recall:.4f}, AUC={auc:.4f}")
        
        self.results['individual'] = results
        return results
    
    def train_voting_ensemble(self, X, y):
        """Train voting ensemble for credit scoring."""
        print("\n--- Voting Ensemble ---")
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42, class_weight='balanced')),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42, class_weight='balanced')),
            ('rf', RandomForestClassifier(n_estimators=100, max_depth=10, random_state=42,
                                   class_weight='balanced', n_jobs=-1)),
            ('gb', GradientBoostingClassifier(n_estimators=100, learning_rate=0.1,
                                        max_depth=3, random_state=42))
        ]
        
        voting_hard = VotingClassifier(
            estimators=estimators,
            voting='hard',
            n_jobs=-1
        )
        
        voting_hard.fit(X_train, y_train)
        
        y_pred = voting_hard.predict(X_test)
        accuracy = accuracy_score(y_test, y_pred)
        
        print(f"Hard Voting Accuracy: {accuracy:.4f}")
        
        voting_soft = VotingClassifier(
            estimators=estimators,
            voting='soft',
            n_jobs=-1
        )
        
        voting_soft.fit(X_train, y_train)
        
        y_pred = voting_soft.predict(X_test)
        y_prob = voting_soft.predict_proba(X_test)[:, 1]
        
        accuracy = accuracy_score(y_test, y_pred)
        auc = roc_auc_score(y_test, y_prob)
        ll = log_loss(y_test, voting_soft.predict_proba(X_test))
        
        print(f"Soft Voting Accuracy: {accuracy:.4f}, AUC: {auc:.4f}, LogLoss: {ll:.4f}")
        
        return accuracy
    
    def train_stacking_ensemble(self, X, y):
        """Train stacking ensemble for credit scoring."""
        print("\n--- Stacking Ensemble ---")
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        base_estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42,
                                   class_weight='balanced')),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42,
                                   class_weight='balanced')),
            ('rf', RandomForestClassifier(n_estimators=50, max_depth=10, random_state=42,
                                   class_weight='balanced', n_jobs=-1)),
            ('gb', GradientBoostingClassifier(n_estimators=50, learning_rate=0.1,
                                          max_depth=3, random_state=42))
        ]
        
        stacking = StackingClassifier(
            estimators=base_estimators,
            final_estimator=LogisticRegression(max_iter=1000, random_state=42),
            cv=5,
            stack_method='predict_proba',
            n_jobs=-1
        )
        
        stacking.fit(X_train, y_train)
        
        y_pred = stacking.predict(X_test)
        y_prob = stacking.predict_proba(X_test)[:, 1]
        
        accuracy = accuracy_score(y_test, y_pred)
        auc = roc_auc_score(y_test, y_prob)
        ll = log_loss(y_test, stacking.predict_proba(X_test))
        
        print(f"Stacking Accuracy: {accuracy:.4f}, AUC: {auc:.4f}, LogLoss: {ll:.4f}")
        
        return accuracy
    
    def analyze_credit_risk(self, X, y, df):
        """
        Analyze credit risk using ensembles.
        """
        print("\n--- Credit Risk Analysis ---")
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        ensemble = VotingClassifier(
            estimators=[
                ('lr', LogisticRegression(max_iter=1000, random_state=42)),
                ('rf', RandomForestClassifier(n_estimators=100, random_state=42)),
                ('gb', GradientBoostingClassifier(n_estimators=100, random_state=42))
            ],
            voting='soft',
            weights=[1, 2, 2],
            n_jobs=-1
        )
        
        ensemble.fit(X_train, y_train)
        
        test_indices = np.random.choice(len(X_test), 100, replace=False)
        X_sample = X_test[test_indices]
        y_sample = y_test[test_indices]
        
        y_prob = ensemble.predict_proba(X_sample)[:, 1]
        
        risk_thresholds = [0.3, 0.5, 0.7]
        
        print("\nRisk Distribution at Different Thresholds:")
        
        for threshold in risk_thresholds:
            high_risk = (y_prob >= threshold).sum()
            print(f"Threshold {threshold}: {high_risk} high-risk ({high_risk}%)")


# ============================================================================
# SECTION VIII: HEALTHCARE EXAMPLE (DISEASE DIAGNOSIS)
# ============================================================================

class HealthcareDiagnosisExample:
    """
    Comprehensive healthcare/medical example using ensemble methods.
    
    Application: Disease Diagnosis
    - Predict probability of disease
    - Combine multiple models for reliable diagnosis
    - Use ensemble for medical decision support
    """
    
    def __init__(self):
        self.models = {}
        self.feature_importance = None
        
    def load_and_preprocess_data(self):
        """Load and preprocess healthcare data."""
        print("\n" + "="*60)
        print("HEALTHCARE EXAMPLE: Disease Diagnosis with Ensembles")
        print("="*60)
        
        df = load_healthcare_data()
        
        print(f"Dataset shape: {df.shape}")
        print(f"Class distribution:\n{df['disease'].value_counts()}")
        print(f"Disease prevalence: {df['disease'].mean():.2%}")
        
        X = df.drop('disease', axis=1)
        y = df['disease']
        
        scaler = StandardScaler()
        X_scaled = scaler.fit_transform(X)
        
        return X_scaled, y, df
    
    def train_individual_models(self, X, y):
        """Train individual models for diagnosis."""
        print("\n--- Individual Model Performance for Diagnosis ---")
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        models = {
            'Logistic Regression': LogisticRegression(max_iter=1000, random_state=42),
            'Decision Tree': DecisionTreeClassifier(max_depth=5, random_state=42),
            'Random Forest': RandomForestClassifier(n_estimators=100, max_depth=10,
                                         random_state=42, n_jobs=-1),
            'Gradient Boosting': GradientBoostingClassifier(n_estimators=100,
                                                  learning_rate=0.1,
                                                  max_depth=3,
                                                  random_state=42),
            'SVM': SVC(probability=True, random_state=42),
            'KNN': KNeighborsClassifier(n_neighbors=5),
            'Naive Bayes': GaussianNB()
        }
        
        results = []
        
        for name, model in models.items():
            model.fit(X_train, y_train)
            
            y_pred = model.predict(X_test)
            y_prob = model.predict_proba(X_test)[:, 1]
            
            accuracy = accuracy_score(y_test, y_pred)
            precision = precision_score(y_test, y_pred)
            recall = recall_score(y_test, y_pred)
            f1 = f1_score(y_test, y_pred)
            auc = roc_auc_score(y_test, y_prob)
            
            results.append({
                'model': name,
                'accuracy': accuracy,
                'precision': precision,
                'recall': recall,
                'f1': f1,
                'auc': auc
            })
            
            print(f"{name:20s}: Acc={accuracy:.4f}, F1={f1:.4f}, AUC={auc:.4f}")
        
        return results
    
    def train_diagnostic_ensemble(self, X, y):
        """Train ensemble optimized for medical diagnosis."""
        print("\n--- Diagnostic Ensemble ---")
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42)),
            ('rf', RandomForestClassifier(n_estimators=100, max_depth=10,
                                       random_state=42, n_jobs=-1)),
            ('gb', GradientBoostingClassifier(n_estimators=100, learning_rate=0.1,
                                         max_depth=3, random_state=42)),
            ('nb', GaussianNB())
        ]
        
        voting = VotingClassifier(
            estimators=estimators,
            voting='soft',
            weights=[1, 2, 2, 1],
            n_jobs=-1
        )
        
        voting.fit(X_train, y_train)
        
        y_pred = voting.predict(X_test)
        y_prob = voting.predict_proba(X_test)[:, 1]
        
        accuracy = accuracy_score(y_test, y_pred)
        precision = precision_score(y_test, y_pred)
        recall = recall_score(y_test, y_pred)
        f1 = f1_score(y_test, y_pred)
        auc = roc_auc_score(y_test, y_prob)
        
        print(f"Diagnostic Ensemble:")
        print(f"  Accuracy: {accuracy:.4f}")
        print(f"  Precision: {precision:.4f}")
        print(f"  Recall: {recall:.4f}")
        print(f"  F1 Score: {f1:.4f}")
        print(f"  AUC: {auc:.4f}")
        
        return accuracy
    
    def train_stacking_diagnosis(self, X, y):
        """Train stacking ensemble for diagnosis."""
        print("\n--- Stacking Diagnostic Ensemble ---")
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        base_estimators = [
            ('lr', LogisticRegression(max_iter=1000, random_state=42)),
            ('rf', RandomForestClassifier(n_estimators=100, max_depth=10,
                                 random_state=42, n_jobs=-1)),
            ('gb', GradientBoostingClassifier(n_estimators=100, learning_rate=0.1,
                                      max_depth=3, random_state=42)),
            ('dt', DecisionTreeClassifier(max_depth=5, random_state=42))
        ]
        
        stacking = StackingClassifier(
            estimators=base_estimators,
            final_estimator=RandomForestClassifier(n_estimators=50, random_state=42),
            cv=5,
            stack_method='predict_proba',
            n_jobs=-1
        )
        
        stacking.fit(X_train, y_train)
        
        y_pred = stacking.predict(X_test)
        y_prob = stacking.predict_proba(X_test)[:, 1]
        
        accuracy = accuracy_score(y_test, y_pred)
        auc = roc_auc_score(y_test, y_prob)
        
        print(f"Stacking Accuracy: {accuracy:.4f}, AUC: {auc:.4f}")
        
        return accuracy
    
    def analyze_diagnosis_confidence(self, X, y):
        """
        Analyze diagnosis confidence using ensemble probabilities.
        """
        print("\n--- Diagnosis Confidence Analysis ---")
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42, stratify=y
        )
        
        ensemble = VotingClassifier(
            estimators=[
                ('lr', LogisticRegression(max_iter=1000, random_state=42)),
                ('rf', RandomForestClassifier(n_estimators=100, random_state=42)),
                ('gb', GradientBoostingClassifier(n_estimators=100, random_state=42)),
                ('nb', GaussianNB())
            ],
            voting='soft',
            n_jobs=-1
        )
        
        ensemble.fit(X_train, y_train)
        
        y_prob = ensemble.predict_proba(X_test)[:, 1]
        
        confidence_levels = [(0.0, 0.3), (0.3, 0.5), (0.5, 0.7), (0.7, 1.0)]
        
        print("\nConfidence Level Distribution:")
        
        for low, high in confidence_levels:
            count = ((y_prob >= low) & (y_prob < high)).sum()
            pct = count / len(y_prob) * 100
            print(f"  {low:.1f}-{high:.1f}: {count} samples ({pct:.1f}%)")


# ============================================================================
# SECTION IX: WHEN TO USE ENSEMBLE METHODS
# ============================================================================

class EnsembleDecisionGuide:
    """
    Guide for when to use ensemble methods.
    """
    
    def __init__(self):
        self.guidelines = {}
        
    def analyze_when_to_ensemble(self, X, y):
        """
        Analyze when ensemble methods are beneficial.
        """
        print("\n" + "="*60)
        print("WHEN TO USE ENSEMBLE METHODS")
        print("="*60)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42
        )
        
        single = DecisionTreeClassifier(random_state=42)
        single.fit(X_train, y_train)
        
        single_pred = single.predict(X_test)
        single_acc = accuracy_score(y_test, single_pred)
        
        print("\n--- Scenario: Single Model vs Ensemble ---")
        print(f"Single Decision Tree Accuracy: {single_acc:.4f}")
        
        ensemble = RandomForestClassifier(n_estimators=100, random_state=42)
        ensemble.fit(X_train, y_train)
        
        ensemble_pred = ensemble.predict(X_test)
        ensemble_acc = accuracy_score(y_test, ensemble_pred)
        
        print(f"Random Forest (Ensemble) Accuracy: {ensemble_acc:.4f}")
        print(f"Improvement: {(ensemble_acc - single_acc)*100:.2f}%")
        
        print("\n--- Key Indicators for Using Ensembles ---")
        
        indicators = [
            "High variance in single model predictions",
            "Complex decision boundaries",
            "Multiple good but different models available",
            "Need for more stable predictions",
            "Large training dataset available"
        ]
        
        for indicator in indicators:
            print(f"  - {indicator}")
        
        return ensemble_acc
    
    def analyze_pros_cons(self):
        """
        Analyze pros and cons of ensemble methods.
        """
        print("\n" + "="*60)
        print("ENSEMBLE METHODS: PROS AND CONS")
        print("="*60)
        
        print("\n--- Advantages ---")
        pros = [
            "Improved prediction accuracy",
            "Reduced overfitting",
            "Better generalization",
            "More stable predictions",
            "Handles diverse data patterns",
            "Flexible to combine different models"
        ]
        
        for pro in pros:
            print(f"  + {pro}")
        
        print("\n--- Disadvantages ---")
        cons = [
            "Increased computational complexity",
            "More difficult to interpret",
            " Longer training time",
            "More hyperparameter tuning",
            "Possible overfitting with too many models"
        ]
        
        for con in cons:
            print(f"  - {con}")
        
        print("\n--- Best Practices ---")
        practices = [
            "Start with diverse base models",
            "Use cross-validation for stacking",
            "Don't overdo number of estimators",
            "Monitor for diminishing returns",
            "Consider computational budget"
        ]
        
        for practice in practices:
            print(f"  * {practice}")


# ============================================================================
# SECTION X: TESTING AND VALIDATION
# ============================================================================

def run_comprehensive_tests():
    """
    Run comprehensive tests on ensemble methods.
    """
    print("\n" + "="*60)
    print("RUNNING COMPREHENSIVE TESTS")
    print("="*60)
    
    X, y, _ = generate_classification_data(n_samples=1000)
    
    print("\n--- Test 1: Bagging ---")
    bagging_demo = BaggingEnsembleDemo()
    results = bagging_demo.demonstrate_bagging_classification(X, y)
    print(f"Bagging test completed: {len(results)} configurations tested")
    
    print("\n--- Test 2: Boosting ---")
    boosting_demo = BoostingEnsembleDemo()
    results = boosting_demo.demonstrate_adaboost(X, y)
    print(f"AdaBoost test completed: {len(results)} configurations tested")
    
    print("\n--- Test 3: Voting ---")
    voting_demo = VotingEnsembleDemo()
    results = voting_demo.demonstrate_hard_voting(X, y)
    print(f"Voting test completed")
    
    print("\n--- Test 4: Stacking ---")
    stacking_demo = StackingEnsembleDemo()
    results = stacking_demo.demonstrate_basic_stacking(X, y)
    print(f"Stacking test completed")
    
    print("\n--- Test 5: Blending ---")
    blending_demo = BlendingEnsembleDemo()
    results = blending_demo.demonstrate_blending(X, y)
    print(f"Blending test completed")
    
    print("\n" + "="*60)
    print("ALL TESTS COMPLETED SUCCESSFULLY")
    print("="*60)


# ============================================================================
# SECTION XI: MAIN EXECUTION
# ============================================================================

def main():
    """
    Main execution function for Model Ensembling Techniques.
    """
    print("="*60)
    print("MODEL ENSEMBLING TECHNIQUES - COMPREHENSIVE IMPLEMENTATION")
    print("="*60)
    
    print("\n" + "="*60)
    print("SECTION I: DATA GENERATION")
    print("="*60)
    
    X, y, feature_names = generate_classification_data(n_samples=1000)
    print(f"Generated data: {X.shape[0]} samples, {X.shape[1]} features")
    
    print("\n" + "="*60)
    print("SECTION II: BAGGING (BOOTSTRAP AGGREGATING)")
    print("="*60)
    
    bagging_demo = BaggingEnsembleDemo()
    bagging_results = bagging_demo.demonstrate_bagging_classification(X, y)
    
    print("\n" + "="*60)
    print("SECTION III: BOOSTING")
    print("="*60)
    
    boosting_demo = BoostingEnsembleDemo()
    boosting_demo.demonstrate_adaboost(X, y)
    boosting_demo.demonstrate_gradient_boosting(X, y)
    
    print("\n" + "="*60)
    print("SECTION IV: VOTING CLASSIFIERS")
    print("="*60)
    
    voting_demo = VotingEnsembleDemo()
    voting_demo.demonstrate_hard_voting(X, y)
    voting_demo.demonstrate_soft_voting(X, y)
    
    print("\n" + "="*60)
    print("SECTION V: STACKING")
    print("="*60)
    
    stacking_demo = StackingEnsembleDemo()
    stacking_demo.demonstrate_basic_stacking(X, y)
    
    print("\n" + "="*60)
    print("SECTION VI: BLENDING")
    print("="*60)
    
    blending_demo = BlendingEnsembleDemo()
    blending_demo.demonstrate_blending(X, y)
    
    print("\n" + "="*60)
    print("SECTION VII: BANKING EXAMPLE (CREDIT SCORING)")
    print("="*60)
    
    banking_example = BankingCreditScoringExample()
    X_banking, y_banking, df_banking = banking_example.load_and_preprocess_data()
    banking_example.train_individual_models(X_banking, y_banking)
    banking_example.train_voting_ensemble(X_banking, y_banking)
    banking_example.train_stacking_ensemble(X_banking, y_banking)
    
    print("\n" + "="*60)
    print("SECTION VIII: HEALTHCARE EXAMPLE (DISEASE DIAGNOSIS)")
    print("="*60)
    
    healthcare_example = HealthcareDiagnosisExample()
    X_healthcare, y_healthcare, df_healthcare = healthcare_example.load_and_preprocess_data()
    healthcare_example.train_individual_models(X_healthcare, y_healthcare)
    healthcare_example.train_diagnostic_ensemble(X_healthcare, y_healthcare)
    healthcare_example.train_stacking_diagnosis(X_healthcare, y_healthcare)
    
    print("\n" + "="*60)
    print("SECTION IX: WHEN TO USE ENSEMBLE METHODS")
    print("="*60)
    
    decision_guide = EnsembleDecisionGuide()
    decision_guide.analyze_when_to_ensemble(X, y)
    decision_guide.analyze_pros_cons()
    
    print("\n" + "="*60)
    print("SECTION X: TESTING")
    print("="*60)
    
    run_comprehensive_tests()
    
    print("\n" + "="*60)
    print("CONCLUSION")
    print("="*60)
    
    print("""
Model ensembling techniques provide powerful methods for improving
prediction accuracy by combining multiple models:

1. BAGGING (Bootstrap Aggregating):
   - Reduces variance through bootstrap sampling
   - Random Forest is a popular example
   - Parallel training and prediction

2. BOOSTING:
   - Reduces bias through sequential learning
   - Focuses on misclassified samples
   - AdaBoost, Gradient Boosting, XGBoost

3. VOTING CLASSIFIERS:
   - Hard Voting: Majority vote
   - Soft Voting: Probability averaging
   - Weighted Voting: Performance-based weights

4. STACKING:
   - Two-level architecture
   - Uses cross-validation for base predictions
   - Meta-learner combines predictions

5. BLENDING:
   - Similar to stacking but uses hold-out
   - Simpler implementation
   - Less prone to data leakage

Best Practices:
- Use diverse base models
- Don't over-engineer ensembles
- Consider computational costs
- Validate with proper cross-validation
- Monitor for overfitting

The choice of ensemble method depends on:
- Dataset size and complexity
- Available computational resources
- Desired interpretability
- Specific application domain
""")


if __name__ == "__main__":
    main()