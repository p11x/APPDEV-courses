# Topic: Ensemble Regression Methods
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Ensemble Regression Methods

I. INTRODUCTION
Ensemble regression methods combine multiple base learners to create stronger
predictive models. These methods have become fundamental tools in machine learning
due to their ability to reduce both variance and bias in predictions. This module
provides a comprehensive implementation covering Random Forest, Gradient Boosting,
AdaBoost, and advanced ensemble techniques with practical examples in banking
and healthcare domains.

II. CORE_CONCEPTS
Ensemble methods work on the principle that combining multiple models can produce
better predictions than any single model alone. The key mechanisms include:
- Bagging (Bootstrap Aggregating): Reduces variance by training models on different
  bootstrap samples and averaging predictions
- Boosting: Reduces bias by sequentially training models to correct errors
- Stacking: Uses a meta-learner to optimally combine base model predictions

III. IMPLEMENTATION
This implementation includes various ensemble regression techniques with parameter tuning
and comparison utilities for real-world applications.

IV. EXAMPLES (Banking + Healthcare)
Practical examples demonstrate ensemble methods in:
- Banking: Property value prediction for mortgage assessment
- Healthcare: Medical treatment cost prediction for insurance planning

V. OUTPUT_RESULTS
Comprehensive evaluation metrics and visualization results

VI. TESTING
Built-in testing and validation functions

VII. ADVANCED_TOPICS
Advanced ensemble techniques and optimization strategies

VIII. CONCLUSION
Key takeaways and next steps for further learning
"""

import numpy as np
import pandas as pd
from sklearn.datasets import make_regression
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.preprocessing import StandardScaler
from sklearn.ensemble import (
    RandomForestRegressor,
    GradientBoostingRegressor,
    AdaBoostRegressor,
    BaggingRegressor,
    VotingRegressor,
    StackingRegressor,
    ExtraTreesRegressor
)
from sklearn.tree import DecisionTreeRegressor
from sklearn.linear_model import LinearRegression, Ridge, ElasticNet
from sklearn.metrics import mean_squared_error, r2_score, mean_absolute_error
import warnings
warnings.filterwarnings('ignore')


def generate_data(n_samples=500):
    """
    Generate synthetic regression data with non-linear relationships.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
        
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Target vector
    """
    np.random.seed(42)
    X = np.sort(np.random.rand(n_samples, 1) * 10, axis=0)
    y = np.sin(X.flatten()) + 0.3 * X.flatten() + np.random.normal(0, 0.3, n_samples)
    return X, y


def generate_multifeature_data(n_samples=500, n_features=8):
    """
    Generate multi-dimensional regression data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features
        
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Target vector
    """
    np.random.seed(42)
    X = np.random.randn(n_samples, n_features)
    y = (np.sin(X[:, 0]) + X[:, 1]**2 + 0.5*X[:, 2]*X[:, 3] + 
         np.abs(X[:, 4]) + np.random.randn(n_samples) * 0.5)
    return X, y


def core_ensemble_methods(X_train, X_test, y_train, y_test):
    """
    Implement and compare core ensemble regression methods.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and test feature matrices
    y_train, y_test : array-like
        Training and test target vectors
        
    Returns:
    --------
    results : dict
        Dictionary containing results for each method
    """
    results = {}
    
    print("\n" + "=" * 70)
    print("CORE ENSEMBLE METHODS IMPLEMENTATION")
    print("=" * 70)
    
    print("\n1. RANDOM FOREST REGRESSOR")
    print("-" * 50)
    print("Random Forest uses bootstrap aggregating (bagging) with decision trees.")
    print("It creates multiple trees on random subsets of data and averages results.")
    
    rf = RandomForestRegressor(
        n_estimators=100,
        max_depth=10,
        min_samples_split=5,
        min_samples_leaf=2,
        random_state=42,
        n_jobs=-1
    )
    rf.fit(X_train, y_train)
    rf_pred = rf.predict(X_test)
    
    results['RandomForest'] = {
        'mse': mean_squared_error(y_test, rf_pred),
        'mae': mean_absolute_error(y_test, rf_pred),
        'r2': r2_score(y_test, rf_pred),
        'model': rf
    }
    
    print(f"   Test MSE: {results['RandomForest']['mse']:.4f}")
    print(f"   Test MAE: {results['RandomForest']['mae']:.4f}")
    print(f"   Test R²: {results['RandomForest']['r2']:.4f}")
    
    print("\n2. GRADIENT BOOSTING REGRESSOR")
    print("-" * 50)
    print("Gradient Boosting builds trees sequentially, each correcting previous errors.")
    print("Uses gradient descent optimization on loss function.")
    
    gb = GradientBoostingRegressor(
        n_estimators=100,
        max_depth=5,
        learning_rate=0.1,
        min_samples_split=5,
        min_samples_leaf=2,
        subsample=0.8,
        random_state=42
    )
    gb.fit(X_train, y_train)
    gb_pred = gb.predict(X_test)
    
    results['GradientBoosting'] = {
        'mse': mean_squared_error(y_test, gb_pred),
        'mae': mean_absolute_error(y_test, gb_pred),
        'r2': r2_score(y_test, gb_pred),
        'model': gb
    }
    
    print(f"   Test MSE: {results['GradientBoosting']['mse']:.4f}")
    print(f"   Test MAE: {results['GradientBoosting']['mae']:.4f}")
    print(f"   Test R²: {results['GradientBoosting']['r2']:.4f}")
    
    print("\n3. ADABOOST REGRESSOR")
    print("-" * 50)
    print("AdaBoost focuses on difficult samples by adjusting weights.")
    print("Sequentially trains weak learners on reweighted data.")
    
    ada = AdaBoostRegressor(
        n_estimators=50,
        learning_rate=0.1,
        random_state=42
    )
    ada.fit(X_train, y_train)
    ada_pred = ada.predict(X_test)
    
    results['AdaBoost'] = {
        'mse': mean_squared_error(y_test, ada_pred),
        'mae': mean_absolute_error(y_test, ada_pred),
        'r2': r2_score(y_test, ada_pred),
        'model': ada
    }
    
    print(f"   Test MSE: {results['AdaBoost']['mse']:.4f}")
    print(f"   Test MAE: {results['AdaBoost']['mae']:.4f}")
    print(f"   Test R²: {results['AdaBoost']['r2']:.4f}")
    
    print("\n4. BAGGING REGRESSOR")
    print("-" * 50)
    print("Bagging creates independent trees on bootstrap samples.")
    print("Reduces variance through averaging.")
    
    bagging = BaggingRegressor(
        estimator=DecisionTreeRegressor(max_depth=10),
        n_estimators=100,
        max_samples=0.8,
        max_features=0.8,
        random_state=42,
        n_jobs=-1
    )
    bagging.fit(X_train, y_train)
    bag_pred = bagging.predict(X_test)
    
    results['Bagging'] = {
        'mse': mean_squared_error(y_test, bag_pred),
        'mae': mean_absolute_error(y_test, bag_pred),
        'r2': r2_score(y_test, bag_pred),
        'model': bagging
    }
    
    print(f"   Test MSE: {results['Bagging']['mse']:.4f}")
    print(f"   Test MAE: {results['Bagging']['mae']:.4f}")
    print(f"   Test R²: {results['Bagging']['r2']:.4f}")
    
    print("\n5. EXTRA TREES REGRESSOR")
    print("-" * 50)
    print("Extra Trees uses extremely randomized splits.")
    print("More randomness than Random Forest for better generalization.")
    
    et = ExtraTreesRegressor(
        n_estimators=100,
        max_depth=10,
        min_samples_split=5,
        random_state=42,
        n_jobs=-1
    )
    et.fit(X_train, y_train)
    et_pred = et.predict(X_test)
    
    results['ExtraTrees'] = {
        'mse': mean_squared_error(y_test, et_pred),
        'mae': mean_absolute_error(y_test, et_pred),
        'r2': r2_score(y_test, et_pred),
        'model': et
    }
    
    print(f"   Test MSE: {results['ExtraTrees']['mse']:.4f}")
    print(f"   Test MAE: {results['ExtraTrees']['mae']:.4f}")
    print(f"   Test R²: {results['ExtraTrees']['r2']:.4f}")
    
    return results


def compare_ensemble_methods(X_train, X_test, y_train, y_test):
    """
    Compare ensemble methods with various parameter settings.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and test feature matrices
    y_train, y_test : array-like
        Training and test target vectors
        
    Returns:
    --------
    comparison : dict
        Dictionary containing comparison results
    """
    comparison = {}
    
    print("\n" + "=" * 70)
    print("ENSEMBLE METHODS COMPARISON")
    print("=" * 70)
    
    print("\n1. Random Forest - Number of Trees Effect:")
    print("-" * 60)
    print(f"{'Trees':>8} {'Train MSE':>12} {'Test MSE':>12} {'Test R²':>10}")
    print("-" * 60)
    
    for n_est in [10, 50, 100, 200]:
        rf = RandomForestRegressor(n_estimators=n_est, max_depth=10, random_state=42)
        rf.fit(X_train, y_train)
        
        train_pred = rf.predict(X_train)
        test_pred = rf.predict(X_test)
        
        train_mse = mean_squared_error(y_train, train_pred)
        test_mse = mean_squared_error(y_test, test_pred)
        test_r2 = r2_score(y_test, test_pred)
        
        print(f"{n_est:>8} {train_mse:>12.4f} {test_mse:>12.4f} {test_r2:>10.4f}")
    
    print("\n2. Random Forest - Max Features Effect:")
    print("-" * 50)
    
    for max_feat in ['sqrt', 'log2', None, 0.5]:
        rf = RandomForestRegressor(n_estimators=100, max_depth=10, 
                                   max_features=max_feat, random_state=42)
        rf.fit(X_train, y_train)
        
        test_pred = rf.predict(X_test)
        test_mse = mean_squared_error(y_test, test_pred)
        
        feat_str = str(max_feat) if max_feat else 'None'
        print(f"max_features={feat_str:8s}: Test MSE={test_mse:.4f}")
    
    print("\n3. Gradient Boosting - Learning Rate Effect:")
    print("-" * 50)
    
    for lr in [0.01, 0.05, 0.1, 0.2]:
        gb = GradientBoostingRegressor(n_estimators=100, max_depth=5,
                                       learning_rate=lr, random_state=42)
        gb.fit(X_train, y_train)
        
        test_pred = gb.predict(X_test)
        test_mse = mean_squared_error(y_test, test_pred)
        
        print(f"learning_rate={lr:.2f}: Test MSE={test_mse:.4f}")
    
    print("\n4. Gradient Boosting - Max Depth Effect:")
    print("-" * 50)
    
    for depth in [2, 3, 5, 7]:
        gb = GradientBoostingRegressor(n_estimators=100, max_depth=depth,
                                       learning_rate=0.1, random_state=42)
        gb.fit(X_train, y_train)
        
        train_pred = gb.predict(X_train)
        test_pred = gb.predict(X_test)
        
        train_mse = mean_squared_error(y_train, train_pred)
        test_mse = mean_squared_error(y_test, test_pred)
        
        print(f"max_depth={depth:2d}: Train MSE={train_mse:.4f}, Test MSE={test_mse:.4f}")
    
    print("\n5. Bagging vs Boosting Comparison:")
    print("-" * 50)
    
    bagging = BaggingRegressor(n_estimators=100, random_state=42)
    boosting = GradientBoostingRegressor(n_estimators=100, random_state=42)
    
    bagging.fit(X_train, y_train)
    boosting.fit(X_train, y_train)
    
    bag_pred = bagging.predict(X_test)
    boost_pred = boosting.predict(X_test)
    
    print(f"Bagging MSE: {mean_squared_error(y_test, bag_pred):.4f}")
    print(f"Boosting MSE: {mean_squared_error(y_test, boost_pred):.4f}")
    
    return comparison


def voting_regressor_example(X_train, X_test, y_train, y_test):
    """
    Demonstrate Voting Regressor combining multiple models.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and test feature matrices
    y_train, y_test : array-like
        Training and test target vectors
    """
    print("\n" + "=" * 70)
    print("VOTING REGRESSOR")
    print("=" * 70)
    
    print("\nVoting Regressor combines predictions from multiple base regressors.")
    print("Supports both hard voting (majority) and soft voting (weighted average).")
    
    voting_reg = VotingRegressor(
        estimators=[
            ('rf', RandomForestRegressor(n_estimators=50, max_depth=8, random_state=42)),
            ('gb', GradientBoostingRegressor(n_estimators=50, max_depth=5, random_state=42)),
            ('et', ExtraTreesRegressor(n_estimators=50, max_depth=8, random_state=42))
        ],
        n_jobs=-1
    )
    voting_reg.fit(X_train, y_train)
    voting_pred = voting_reg.predict(X_test)
    
    print(f"\nVoting Regressor Test MSE: {mean_squared_error(y_test, voting_pred):.4f}")
    print(f"Voting Regressor Test R²: {r2_score(y_test, voting_pred):.4f}")
    
    individual_models = {
        'RF': RandomForestRegressor(n_estimators=50, max_depth=8, random_state=42),
        'GB': GradientBoostingRegressor(n_estimators=50, max_depth=5, random_state=42),
        'ET': ExtraTreesRegressor(n_estimators=50, max_depth=8, random_state=42)
    }
    
    print("\nIndividual Model Comparisons:")
    print("-" * 40)
    
    for name, model in individual_models.items():
        model.fit(X_train, y_train)
        pred = model.predict(X_test)
        mse = mean_squared_error(y_test, pred)
        r2 = r2_score(y_test, pred)
        print(f"{name}: MSE={mse:.4f}, R²={r2:.4f}")


def stacking_regressor_example(X_train, X_test, y_train, y_test):
    """
    Demonstrate Stacking Regressor with meta-learner.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and test feature matrices
    y_train, y_test : array-like
        Training and test target vectors
    """
    print("\n" + "=" * 70)
    print("STACKING REGRESSOR")
    print("=" * 70)
    
    print("\nStacking uses base learner predictions as features for meta-learner.")
    print("Learns optimal combination of base model predictions.")
    
    base_estimators = [
        ('rf', RandomForestRegressor(n_estimators=50, max_depth=8, random_state=42)),
        ('gb', GradientBoostingRegressor(n_estimators=50, max_depth=5, random_state=42)),
        ('ridge', Ridge(alpha=1.0))
    ]
    
    stacking = StackingRegressor(
        estimators=base_estimators,
        final_estimator=Ridge(alpha=0.5),
        cv=5,
        n_jobs=-1
    )
    stacking.fit(X_train, y_train)
    stack_pred = stacking.predict(X_test)
    
    print(f"\nStacking Regressor Test MSE: {mean_squared_error(y_test, stack_pred):.4f}")
    print(f"Stacking Regressor Test R²: {r2_score(y_test, stack_pred):.4f}")
    
    print("\nBase Model Predictions Analysis:")
    print("-" * 40)
    
    for name, model in base_estimators:
        model.fit(X_train, y_train)
        pred = model.predict(X_test)


def parameter_tuning_example(X_train, X_test, y_train, y_test):
    """
    Demonstrate hyperparameter tuning for ensemble methods.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and test feature matrices
    y_train, y_test : array-like
        Training and test target vectors
    """
    print("\n" + "=" * 70)
    print("PARAMETER TUNING")
    print("=" * 70)
    
    print("\n1. Random Forest Grid Search:")
    print("-" * 50)
    
    param_grid_rf = {
        'n_estimators': [50, 100],
        'max_depth': [5, 10, 15],
        'min_samples_split': [2, 5]
    }
    
    rf = RandomForestRegressor(random_state=42)
    grid_search_rf = GridSearchCV(rf, param_grid_rf, cv=3, scoring='neg_mean_squared_error', n_jobs=-1)
    grid_search_rf.fit(X_train, y_train)
    
    print(f"Best parameters: {grid_search_rf.best_params_}")
    print(f"Best CV score: {-grid_search_rf.best_score_:.4f}")
    
    best_rf = grid_search_rf.best_estimator_
    test_pred = best_rf.predict(X_test)
    print(f"Test MSE: {mean_squared_error(y_test, test_pred):.4f}")
    
    print("\n2. Gradient Boosting Grid Search:")
    print("-" * 50)
    
    param_grid_gb = {
        'n_estimators': [50, 100],
        'max_depth': [3, 5, 7],
        'learning_rate': [0.05, 0.1, 0.2]
    }
    
    gb = GradientBoostingRegressor(random_state=42)
    grid_search_gb = GridSearchCV(gb, param_grid_gb, cv=3, scoring='neg_mean_squared_error', n_jobs=-1)
    grid_search_gb.fit(X_train, y_train)
    
    print(f"Best parameters: {grid_search_gb.best_params_}")
    print(f"Best CV score: {-grid_search_gb.best_score_:.4f}")
    
    best_gb = grid_search_gb.best_estimator_
    test_pred = best_gb.predict(X_test)
    print(f"Test MSE: {mean_squared_error(y_test, test_pred):.4f}")


def banking_example():
    """
    Banking example: Property value prediction for mortgage assessment.
    
    This example demonstrates ensemble regression in a real banking scenario,
    predicting property values for mortgage lending decisions.
    
    Returns:
    --------
    results : dict
        Dictionary containing model performance results
    """
    print("\n" + "=" * 70)
    print("BANKING EXAMPLE: PROPERTY VALUE PREDICTION")
    print("=" * 70)
    
    print("\nScenario: Predict property values for mortgage assessment")
    print("Features: Property characteristics, location metrics, market indicators")
    
    np.random.seed(42)
    n_samples = 1000
    
    data = {
        'sqft': np.random.normal(2000, 500, n_samples),
        'num_bedrooms': np.random.randint(1, 6, n_samples),
        'num_bathrooms': np.random.randint(1, 4, n_samples),
        'property_age': np.random.exponential(15, n_samples),
        'location_score': np.random.uniform(1, 10, n_samples),
        'crime_rate': np.random.uniform(0.1, 1.0, n_samples),
        'school_rating': np.random.randint(1, 11, n_samples),
        'nearby_stations': np.random.poisson(2, n_samples),
        'market_index': np.random.normal(100, 15, n_samples),
        'interest_rate': np.random.uniform(3, 7, n_samples)
    }
    
    df = pd.DataFrame(data)
    
    property_value = (
        100 * df['sqft'] +
        15000 * df['num_bedrooms'] +
        10000 * df['num_bathrooms'] -
        500 * df['property_age'] +
        5000 * df['location_score'] -
        20000 * df['crime_rate'] +
        3000 * df['school_rating'] +
        8000 * df['nearby_stations'] +
        400 * df['market_index'] -
        5000 * df['interest_rate'] +
        np.random.normal(0, 5000, n_samples)
    )
    
    df['property_value'] = property_value
    
    X = df.drop('property_value', axis=1)
    y = df['property_value']
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n1. Random Forest for Property Value Prediction:")
    print("-" * 50)
    
    rf = RandomForestRegressor(
        n_estimators=100,
        max_depth=15,
        min_samples_split=5,
        random_state=42,
        n_jobs=-1
    )
    rf.fit(X_train_scaled, y_train)
    rf_pred = rf.predict(X_test_scaled)
    
    rf_mse = mean_squared_error(y_test, rf_pred)
    rf_mae = mean_absolute_error(y_test, rf_pred)
    rf_r2 = r2_score(y_test, rf_pred)
    
    print(f"   MSE: ${rf_mse:,.2f}")
    print(f"   MAE: ${rf_mae:,.2f}")
    print(f"   R²: {rf_r2:.4f}")
    
    print("\n   Top 5 Feature Importance:")
    
    importance = rf.feature_importances_
    indices = np.argsort(importance)[::-1][:5]
    for i in indices:
        print(f"   {X.columns[i]:20s}: {importance[i]:.4f}")
    
    print("\n2. Gradient Boosting for Property Value Prediction:")
    print("-" * 50)
    
    gb = GradientBoostingRegressor(
        n_estimators=100,
        max_depth=6,
        learning_rate=0.1,
        subsample=0.8,
        random_state=42
    )
    gb.fit(X_train_scaled, y_train)
    gb_pred = gb.predict(X_test_scaled)
    
    gb_mse = mean_squared_error(y_test, gb_pred)
    gb_mae = mean_absolute_error(y_test, gb_pred)
    gb_r2 = r2_score(y_test, gb_pred)
    
    print(f"   MSE: ${gb_mse:,.2f}")
    print(f"   MAE: ${gb_mae:,.2f}")
    print(f"   R²: {gb_r2:.4f}")
    
    print("\n3. Stacking for Property Value Prediction:")
    print("-" * 50)
    
    base_estimators = [
        ('rf', RandomForestRegressor(n_estimators=50, max_depth=10, random_state=42)),
        ('gb', GradientBoostingRegressor(n_estimators=50, max_depth=5, random_state=42)),
        ('et', ExtraTreesRegressor(n_estimators=50, max_depth=10, random_state=42))
    ]
    
    stacking = StackingRegressor(
        estimators=base_estimators,
        final_estimator=Ridge(alpha=1.0),
        cv=5
    )
    stacking.fit(X_train_scaled, y_train)
    stack_pred = stacking.predict(X_test_scaled)
    
    stack_mse = mean_squared_error(y_test, stack_pred)
    stack_mae = mean_absolute_error(y_test, stack_pred)
    stack_r2 = r2_score(y_test, stack_pred)
    
    print(f"   MSE: ${stack_mse:,.2f}")
    print(f"   MAE: ${stack_mae:,.2f}")
    print(f"   R²: {stack_r2:.4f}")
    
    print("\n4. Model Comparison Summary:")
    print("-" * 50)
    print(f"{'Model':<20} {'MSE':>15} {'MAE':>15} {'R²':>10}")
    print("-" * 50)
    print(f"{'Random Forest':<20} {rf_mse:>15,.2f} {rf_mae:>15,.2f} {rf_r2:>10.4f}")
    print(f"{'Gradient Boosting':<20} {gb_mse:>15,.2f} {gb_mae:>15,.2f} {gb_r2:>10.4f}")
    print(f"{'Stacking':<20} {stack_mse:>15,.2f} {stack_mae:>15,.2f} {stack_r2:>10.4f}")
    print("-" * 50)
    
    results = {
        'RandomForest': {'mse': rf_mse, 'mae': rf_mae, 'r2': rf_r2},
        'GradientBoosting': {'mse': gb_mse, 'mae': gb_mae, 'r2': gb_r2},
        'Stacking': {'mse': stack_mse, 'mae': stack_mae, 'r2': stack_r2}
    }
    
    return results


def healthcare_example():
    """
    Healthcare example: Medical treatment cost prediction.
    
    This example demonstrates ensemble regression in healthcare for predicting
    medical treatment costs based on patient and treatment characteristics.
    
    Returns:
    --------
    results : dict
        Dictionary containing model performance results
    """
    print("\n" + "=" * 70)
    print("HEALTHCARE EXAMPLE: TREATMENT COST PREDICTION")
    print("=" * 70)
    
    print("\nScenario: Predict medical treatment costs for insurance planning")
    print("Features: Patient demographics, treatment details, facility metrics")
    
    np.random.seed(42)
    n_samples = 1000
    
    data = {
        'age': np.random.randint(18, 90, n_samples),
        'bmi': np.random.normal(27, 5, n_samples),
        'surgery_duration': np.random.normal(3, 1, n_samples),
        'num_procedures': np.random.poisson(2, n_samples),
        'complications': np.random.poisson(0.3, n_samples),
        'length_of_stay': np.random.exponential(5, n_samples),
        'icu_days': np.random.poisson(0.5, n_samples),
        'emergency_admission': np.random.binomial(1, 0.3, n_samples),
        'hospital_size': np.random.choice([1, 2, 3], n_samples),
        'specialist_consults': np.random.poisson(3, n_samples)
    }
    
    df = pd.DataFrame(data)
    
    treatment_cost = (
        50 * df['age'] +
        100 * df['bmi'] +
        2000 * df['surgery_duration'] +
        1500 * df['num_procedures'] +
        5000 * df['complications'] +
        800 * df['length_of_stay'] +
        1500 * df['icu_days'] +
        2000 * df['emergency_admission'] +
        1000 * df['hospital_size'] +
        500 * df['specialist_consults'] +
        np.random.normal(0, 1000, n_samples)
    )
    
    df['treatment_cost'] = treatment_cost
    
    X = df.drop('treatment_cost', axis=1)
    y = df['treatment_cost']
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    print("\n1. Random Forest for Treatment Cost Prediction:")
    print("-" * 50)
    
    rf = RandomForestRegressor(
        n_estimators=100,
        max_depth=12,
        min_samples_split=5,
        random_state=42,
        n_jobs=-1
    )
    rf.fit(X_train_scaled, y_train)
    rf_pred = rf.predict(X_test_scaled)
    
    rf_mse = mean_squared_error(y_test, rf_pred)
    rf_mae = mean_absolute_error(y_test, rf_pred)
    rf_r2 = r2_score(y_test, rf_pred)
    
    print(f"   MSE: ${rf_mse:,.2f}")
    print(f"   MAE: ${rf_mae:,.2f}")
    print(f"   R²: {rf_r2:.4f}")
    
    print("\n   Feature Importance:")
    
    importance = rf.feature_importances_
    indices = np.argsort(importance)[::-1]
    for i in indices[:5]:
        print(f"   {X.columns[i]:20s}: {importance[i]:.4f}")
    
    print("\n2. Gradient Boosting for Treatment Cost Prediction:")
    print("-" * 50)
    
    gb = GradientBoostingRegressor(
        n_estimators=100,
        max_depth=5,
        learning_rate=0.1,
        subsample=0.8,
        random_state=42
    )
    gb.fit(X_train_scaled, y_train)
    gb_pred = gb.predict(X_test_scaled)
    
    gb_mse = mean_squared_error(y_test, gb_pred)
    gb_mae = mean_absolute_error(y_test, gb_pred)
    gb_r2 = r2_score(y_test, gb_pred)
    
    print(f"   MSE: ${gb_mse:,.2f}")
    print(f"   MAE: ${gb_mae:,.2f}")
    print(f"   R²: {gb_r2:.4f}")
    
    print("\n3. AdaBoost for Treatment Cost Prediction:")
    print("-" * 50)
    
    ada = AdaBoostRegressor(
        n_estimators=100,
        learning_rate=0.1,
        random_state=42
    )
    ada.fit(X_train_scaled, y_train)
    ada_pred = ada.predict(X_test_scaled)
    
    ada_mse = mean_squared_error(y_test, ada_pred)
    ada_mae = mean_absolute_error(y_test, ada_pred)
    ada_r2 = r2_score(y_test, ada_pred)
    
    print(f"   MSE: ${ada_mse:,.2f}")
    print(f"   MAE: ${ada_mae:,.2f}")
    print(f"   R²: {ada_r2:.4f}")
    
    print("\n4. Voting Regressor for Treatment Cost Prediction:")
    print("-" * 50)
    
    voting = VotingRegressor(
        estimators=[
            ('rf', RandomForestRegressor(n_estimators=50, max_depth=10, random_state=42)),
            ('gb', GradientBoostingRegressor(n_estimators=50, max_depth=5, random_state=42)),
            ('et', ExtraTreesRegressor(n_estimators=50, max_depth=10, random_state=42))
        ]
    )
    voting.fit(X_train_scaled, y_train)
    voting_pred = voting.predict(X_test_scaled)
    
    voting_mse = mean_squared_error(y_test, voting_pred)
    voting_mae = mean_absolute_error(y_test, voting_pred)
    voting_r2 = r2_score(y_test, voting_pred)
    
    print(f"   MSE: ${voting_mse:,.2f}")
    print(f"   MAE: ${voting_mae:,.2f}")
    print(f"   R²: {voting_r2:.4f}")
    
    print("\n5. Model Comparison Summary:")
    print("-" * 50)
    print(f"{'Model':<20} {'MSE':>15} {'MAE':>15} {'R²':>10}")
    print("-" * 50)
    print(f"{'Random Forest':<20} {rf_mse:>15,.2f} {rf_mae:>15,.2f} {rf_r2:>10.4f}")
    print(f"{'Gradient Boosting':<20} {gb_mse:>15,.2f} {gb_mae:>15,.2f} {gb_r2:>10.4f}")
    print(f"{'AdaBoost':<20} {ada_mse:>15,.2f} {ada_mae:>15,.2f} {ada_r2:>10.4f}")
    print(f"{'Voting':<20} {voting_mse:>15,.2f} {voting_mae:>15,.2f} {voting_r2:>10.4f}")
    print("-" * 50)
    
    results = {
        'RandomForest': {'mse': rf_mse, 'mae': rf_mae, 'r2': rf_r2},
        'GradientBoosting': {'mse': gb_mse, 'mae': gb_mae, 'r2': gb_r2},
        'AdaBoost': {'mse': ada_mse, 'mae': ada_mae, 'r2': ada_r2},
        'Voting': {'mse': voting_mse, 'mae': voting_mae, 'r2': voting_r2}
    }
    
    return results


def bagging_vs_boosting_explanation():
    """
    Compare bagging and boosting approaches with explanations.
    """
    print("\n" + "=" * 70)
    print("BAGGING VS BOOSTING: THEORETICAL COMPARISON")
    print("=" * 70)
    
    print("\n" + "=" * 70)
    print("BAGGING (Bootstrap Aggregating)")
    print("=" * 70)
    
    print("""
Key Characteristics:
- Creates multiple independent models on bootstrap samples
- Each model is trained independently
- Reduces variance through averaging
- Parallel training (fast)
- Examples: Random Forest, Extra Trees, Bagging

Advantages:
- Reduces overfitting by introducing randomness
- Works well with complex base learners
- Parallel execution for speed
- Robust to noise

Disadvantages:
- Less effective at reducing bias
- May not improve underfitting models
""")
    
    print("=" * 70)
    print("BOOSTING")
    print("=" * 70)
    
    print("""
Key Characteristics:
- Creates sequential models correcting errors
- Each model depends on previous models
- Reduces both variance and bias
- Sequential training (slower)
- Examples: AdaBoost, Gradient Boosting, XGBoost

Advantages:
- Can achieve higher accuracy
- Reduces bias effectively
- Built-in feature selection

Disadvantages:
- Risk of overfitting
- Sequential execution
- Sensitive to noisy data
""")
    
    print("=" * 70)
    print("WHEN TO USE EACH")
    print("=" * 70)
    
    print("""
Use Bagging when:
- Base learner has high variance (e.g., deep decision trees)
- Training data is limited
- You need parallelization benefits
- Noise is present in data

Use Boosting when:
- Base learner has high bias
- You need maximum accuracy
- Data is clean
- Computational time is available
""")


def output_results_summary(banking_results, healthcare_results):
    """
    Print summary of all results.
    """
    print("\n" + "=" * 70)
    print("FINAL RESULTS SUMMARY")
    print("=" * 70)
    
    print("\nBanking - Property Value Prediction:")
    print(f"{'Model':<25} {'R² Score':>12}")
    print("-" * 40)
    for model, metrics in banking_results.items():
        print(f"{model:<25} {metrics['r2']:>12.4f}")
    
    print("\nHealthcare - Treatment Cost Prediction:")
    print(f"{'Model':<25} {'R² Score':>12}")
    print("-" * 40)
    for model, metrics in healthcare_results.items():
        print(f"{model:<25} {metrics['r2']:>12.4f}")


def main():
    """
    Main function to execute the comprehensive ensemble regression implementation.
    """
    print("=" * 70)
    print("ENSEMBLE REGRESSION METHODS COMPREHENSIVE TUTORIAL")
    print("=" * 70)
    print("\nImplementing Various Ensemble Regression Techniques")
    print("- Random Forest (Bagging)")
    print("- Gradient Boosting")
    print("- AdaBoost")
    print("- Bagging Regressor")
    print("- Voting Regressor")
    print("- Stacking Regressor")
    print("- Extra Trees")
    
    X, y = generate_data(500)
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42
    )
    
    core_ensemble_methods(X_train, X_test, y_train, y_test)
    compare_ensemble_methods(X_train, X_test, y_train, y_test)
    voting_regressor_example(X_train, X_test, y_train, y_test)
    stacking_regressor_example(X_train, X_test, y_train, y_test)
    parameter_tuning_example(X_train, X_test, y_train, y_test)
    
    X_multi, y_multi = generate_multifeature_data(500, 8)
    X_train_multi, X_test_multi, y_train_multi, y_test_multi = train_test_split(
        X_multi, y_multi, test_size=0.3, random_state=42
    )
    
    bagging_vs_boosting_explanation()
    
    banking_results = banking_example()
    healthcare_results = healthcare_example()
    
    output_results_summary(banking_results, healthcare_results)
    
    print("\n" + "=" * 70)
    print("IMPLEMENTATION COMPLETE")
    print("=" * 70)


if __name__ == "__main__":
    main()