# Topic: Advanced Regression Techniques
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Advanced Regression Techniques

This module provides advanced regression techniques including Bayesian Ridge Regression,
Stacking Regressors, Pipeline with preprocessing, Advanced feature engineering,
Hyperparameter tuning, Model comparison, Banking/Finance examples, and Healthcare examples.

I. INTRODUCTION
    - Overview of advanced regression techniques
    - When to use each technique
    - Comparison framework

II. CORE CONCEPTS
    - Bayesian Ridge Regression
    - Stacking Ensemble Methods
    - Pipeline Architecture
    - Feature Engineering
    - Hyperparameter Tuning

III. IMPLEMENTATION
    - Data Generation
    - Model Training
    - Evaluation Metrics
    - Comparison Analysis

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Credit Risk Prediction
    - Healthcare: Patient Outcome Prediction

V. OUTPUT RESULTS
    - Model Performance Metrics
    - Feature Importance
    - Comparative Analysis

VI. TESTING
    - Unit Tests
    - Integration Tests
    - Validation Tests

VII. ADVANCED TOPICS
    - Regularization Techniques
    - Ensemble Methods
    - Cross-Validation Strategies
    - Model Selection

VIII. CONCLUSION
    - Best Practices
    - Recommendations
"""

import numpy as np
import pandas as pd
from sklearn.datasets import make_regression, make_friedman1
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV, KFold
from sklearn.preprocessing import StandardScaler, PolynomialFeatures, RobustScaler, MinMaxScaler
from sklearn.linear_model import Ridge, Lasso, ElasticNet, BayesianRidge, LinearRegression, HuberRegressor
from sklearn.svm import SVR
from sklearn.ensemble import (
    RandomForestRegressor, 
    GradientBoostingRegressor,
    AdaBoostRegressor,
    StackingRegressor,
    VotingRegressor,
    BaggingRegressor
)
from sklearn.neighbors import KNeighborsRegressor
from sklearn.tree import DecisionTreeRegressor
from sklearn.pipeline import Pipeline, FeatureUnion
from sklearn.metrics import mean_squared_error, r2_score, mean_absolute_error, median_absolute_error
from sklearn.base import BaseEstimator, TransformerMixin, RegressorMixin
import warnings
warnings.filterwarnings('ignore')


class FeatureSelector(BaseEstimator, TransformerMixin):
    """Custom transformer for selecting specific features"""
    
    def __init__(self, feature_indices):
        self.feature_indices = feature_indices
    
    def fit(self, X, y=None):
        return self
    
    def transform(self, X):
        return X[:, self.feature_indices]


class InteractionFeatures(BaseEstimator, TransformerMixin):
    """Create interaction features between selected columns"""
    
    def __init__(self, interaction_pairs):
        self.interaction_pairs = interaction_pairs
    
    def fit(self, X, y=None):
        return self
    
    def transform(self, X):
        interactions = []
        for i, j in self.interaction_pairs:
            interactions.append(X[:, i] * X[:, j])
        return np.column_stack([X] + interactions)


class LogTransform(BaseEstimator, TransformerMixin):
    """Apply log transformation to features"""
    
    def __init__(self, epsilon=1e-6):
        self.epsilon = epsilon
    
    def fit(self, X, y=None):
        return self
    
    def transform(self, X):
        return np.log1p(np.abs(X))


def generate_complex_data(n_samples=500, n_features=10, noise=0.1, random_state=42):
    """
    Generate complex regression data with non-linear relationships
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features
    noise : float
        Noise level
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Target vector
    feature_names : list
        Names of features
    """
    np.random.seed(random_state)
    
    X, y = make_friedman1(n_samples=n_samples, n_features=n_features, noise=noise, random_state=random_state)
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    feature_names[0] = ' income'
    feature_names[1] = ' employment_duration'
    feature_names[2] = ' loan_amount'
    feature_names[3] = ' interest_rate'
    feature_names[4] = ' credit_score'
    feature_names[5] = ' debt_to_income'
    feature_names[6] = ' savings_balance'
    feature_names[7] = ' age'
    feature_names[8] = ' num_credit_lines'
    feature_names[9] = ' unemployment_rate'
    
    X_df = pd.DataFrame(X, columns=feature_names)
    
    X_df[' income_squared'] = X_df[' income'] ** 2
    X_df[' credit_score_normalized'] = (X_df[' credit_score'] - X_df[' credit_score'].min()) / (X_df[' credit_score'].max() - X_df[' credit_score'].min())
    X_df[' loan_to_income'] = X_df[' loan_amount'] / (X_df[' income'] + 1)
    X_df[' debt_burden'] = X_df[' debt_to_income'] * X_df[' loan_amount']
    
    X_augmented = X_df.values
    
    return X_augmented, y, list(X_df.columns)


def generate_banking_data(n_samples=1000, random_state=42):
    """
    Generate synthetic banking/finance data for credit risk prediction
    
    Features:
    - Income
    - Employment Duration
    - Loan Amount
    - Interest Rate
    - Credit Score
    - Debt to Income Ratio
    - Savings Balance
    - Age
    - Number of Credit Lines
    - Unemployment Rate
    - Previous Defaults
    - Loan Purpose Type
    - Housing Status
    - Monthly Debt Payments
    
    Target:
    - Loan Default Probability
    - Interest Charged
    - Loan Approval Amount
    """
    np.random.seed(random_state)
    
    n_features = 14
    X, _ = make_friedman1(n_samples=n_samples, n_features=n_features, noise=0.15, random_state=random_state)
    
    credit_score = 300 + X[:, 4] * 550
    income = 20000 + X[:, 0] * 100000
    loan_amount = 1000 + X[:, 2] * 50000
    employment_duration = X[:, 1] * 20
    debt_to_income = X[:, 5] * 0.5
    interest_rate = 3 + X[:, 3] * 15
    savings_balance = X[:, 6] * 50000
    age = 18 + X[:, 7] * 62
    num_credit_lines = (X[:, 8] * 10).astype(int)
    unemployment_rate = X[:, 9] * 0.15
    previous_defaults = np.random.randint(0, 5, size=n_samples)
    loan_purpose = np.random.randint(0, 4, size=n_samples)
    housing_status = np.random.randint(0, 3, size=n_samples)
    monthly_debt = debt_to_income * income / 12
    
    base_default_prob = 0.1
    credit_score_effect = (650 - credit_score) / 650 * 0.3
    debt_effect = debt_to_income * 0.2
    income_effect = 50000 / (income + 1) * 0.1
    default_prob = np.clip(base_default_prob + credit_score_effect + debt_effect + income_effect, 0.01, 0.99)
    loan_default = (np.random.random(n_samples) < default_prob).astype(int)
    
    interest_charged = (interest_rate * 0.01 * loan_amount) + (default_prob * loan_amount * 0.05)
    interest_charged = interest_charged * (1 + np.random.normal(0, 0.1, n_samples))
    
    loan_approval_amount = np.minimum(loan_amount, income * 0.4)
    loan_approval_amount = loan_approval_amount * (1 + (credit_score - 500) / 500)
    loan_approval_amount = np.maximum(loan_approval_amount, 0)
    
    feature_names = [
        'income', 'employment_duration', 'loan_amount', 'interest_rate',
        'credit_score', 'debt_to_income', 'savings_balance', 'age',
        'num_credit_lines', 'unemployment_rate', 'previous_defaults',
        'loan_purpose', 'housing_status', 'monthly_debt'
    ]
    
    X_df = pd.DataFrame(X[:, :n_features], columns=feature_names)
    X_df['credit_score'] = credit_score
    X_df['income'] = income
    X_df['loan_amount'] = loan_amount
    X_df['interest_rate'] = interest_rate
    X_df['debt_to_income'] = debt_to_income
    X_df['savings_balance'] = savings_balance
    X_df['age'] = age
    X_df['num_credit_lines'] = num_credit_lines
    X_df['unemployment_rate'] = unemployment_rate
    X_df['previous_defaults'] = previous_defaults
    X_df['loan_purpose'] = loan_purpose
    X_df['housing_status'] = housing_status
    X_df['monthly_debt'] = monthly_debt
    
    X_df['credit_score_squared'] = credit_score ** 2
    X_df['loan_to_income_ratio'] = loan_amount / (income + 1)
    X_df['debt_burden_ratio'] = monthly_debt / (income / 12 + 1)
    X_df['employment_income_ratio'] = employment_duration / (income / 10000 + 1)
    X_df['savings_loan_ratio'] = savings_balance / (loan_amount + 1)
    X_df['age_credit_interaction'] = age * credit_score
    
    y_default = loan_default
    y_interest = interest_charged
    y_approval = loan_approval_amount
    
    return X_df.values, y_default, y_interest, y_approval, list(X_df.columns)


def generate_healthcare_data(n_samples=1000, random_state=42):
    """
    Generate synthetic healthcare data for patient outcome prediction
    
    Features:
    - Age
    - BMI
    - Blood Pressure (systolic)
    - Blood Pressure (diastolic)
    - Cholesterol (total)
    - HDL Cholesterol
    - LDL Cholesterol
    - Blood Glucose
    - Heart Rate
    - Respiratory Rate
    - Temperature
    - White Blood Cell Count
    - Platelet Count
    - Smoking Status
    - Exercise Level
    - Family History
    
    Target:
    - Healthcare Cost
    - Length of Stay
    - Readmission Probability
    """
    np.random.seed(random_state)
    
    n_features = 17
    X = np.random.randn(n_samples, n_features)
    
    age = 18 + X[:, 0] * 50
    bmi = 18 + X[:, 1] * 15
    bp_systolic = 90 + X[:, 2] * 40
    bp_diastolic = 60 + X[:, 3] * 25
    cholesterol_total = 150 + X[:, 4] * 100
    hdl = 30 + X[:, 5] * 40
    ldl = 50 + X[:, 6] * 80
    blood_glucose = 70 + X[:, 7] * 50
    heart_rate = 60 + X[:, 8] * 30
    respiratory_rate = 12 + X[:, 9] * 8
    temperature = 36.5 + X[:, 10] * 1.5
    wbc = 4000 + X[:, 11] * 4000
    platelet = 150000 + X[:, 12] * 100000
    smoking_status = (X[:, 13] > 0.5).astype(int)
    exercise_level = np.random.randint(0, 4, size=n_samples)
    family_history = (X[:, 15] > 0.3).astype(int)
    gender = (X[:, 16] > 0).astype(int)
    
    base_cost = 1000
    age_cost = age * 15
    bmi_cost = np.where(bmi > 30, (bmi - 30) * 50, 0)
    bp_cost = np.where(bp_systolic > 140, (bp_systolic - 140) * 20, 0)
    cholesterol_cost = (cholesterol_total - 200) * 3
    glucose_cost = np.where(blood_glucose > 100, (blood_glucose - 100) * 10, 0)
    smoking_cost = smoking_status * 500
    exercise_benefit = -exercise_level * 100
    family_cost = family_history * 300
    
    healthcare_cost = base_cost + age_cost + bmi_cost + bp_cost + cholesterol_cost + glucose_cost + smoking_cost + exercise_benefit + family_cost
    healthcare_cost = healthcare_cost * (1 + np.random.normal(0, 0.15, n_samples))
    healthcare_cost = np.maximum(healthcare_cost, 100)
    
    length_of_stay = 1 + (age / 30) + (bmi / 20) + (bp_systolic / 50) + (smoking_status * 2) - (exercise_level * 0.5)
    length_of_stay = length_of_stay * (1 + np.random.normal(0, 0.3, n_samples))
    length_of_stay = np.maximum(np.round(length_of_stay), 1)
    
    base_readmit = 0.1
    age_readmit = age / 100 * 0.1
    bmi_readmit = np.where(bmi > 30, (bmi - 30) / 30 * 0.15, 0)
    bp_readmit = np.where(bp_systolic > 140, 0.1, 0)
    cholesterol_readmit = np.where(cholesterol_total > 240, 0.1, 0)
    glucose_readmit = np.where(blood_glucose > 126, 0.15, 0)
    smoking_readmit = smoking_status * 0.2
    exercise_readmit = -exercise_level / 4 * 0.1
    family_readmit = family_history * 0.1
    
    readmit_prob = np.clip(base_readmit + age_readmit + bmi_readmit + bp_readmit + cholesterol_readmit + glucose_readmit + smoking_readmit + exercise_readmit + family_readmit, 0.01, 0.99)
    readmit = (np.random.random(n_samples) < readmit_prob).astype(int)
    
    feature_names = [
        'age', 'bmi', 'bp_systolic', 'bp_diastolic', 'cholesterol_total',
        'hdl', 'ldl', 'blood_glucose', 'heart_rate', 'respiratory_rate',
        'temperature', 'wbc', 'platelet', 'smoking_status', 'exercise_level',
        'family_history', 'gender'
    ]
    
    X_df = pd.DataFrame(X[:, :n_features], columns=feature_names)
    X_df['age'] = age
    X_df['bmi'] = bmi
    X_df['bp_systolic'] = bp_systolic
    X_df['bp_diastolic'] = bp_diastolic
    X_df['cholesterol_total'] = cholesterol_total
    X_df['hdl'] = hdl
    X_df['ldl'] = ldl
    X_df['blood_glucose'] = blood_glucose
    X_df['heart_rate'] = heart_rate
    X_df['respiratory_rate'] = respiratory_rate
    X_df['temperature'] = temperature
    X_df['wbc'] = wbc
    X_df['platelet'] = platelet
    X_df['smoking_status'] = smoking_status
    X_df['exercise_level'] = exercise_level
    X_df['family_history'] = family_history
    X_df['gender'] = gender
    
    X_df['bmi_squared'] = bmi ** 2
    X_df['bp_product'] = bp_systolic * bp_diastolic
    X_df['cholesterol_ratio'] = ldl / (hdl + 1)
    X_df['age_bmi_interaction'] = age * bmi
    X_df['age_smoking_interaction'] = age * smoking_status
    
    y_cost = healthcare_cost
    y_stay = length_of_stay
    y_readmit = readmit
    
    return X_df.values, y_cost, y_stay, y_readmit, list(X_df.columns)


def advanced_regression_comparison(X, y, random_state=42):
    """
    Compare multiple advanced regression methods
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target vector
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Dictionary containing results for each model
    """
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=random_state)
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    models = {
        'Bayesian Ridge': BayesianRidge(alpha_1=1e-6, alpha_2=1e-6, lambda_1=1e-6, lambda_2=1e-6),
        'Ridge': Ridge(alpha=1.0, random_state=random_state),
        'Lasso': Lasso(alpha=0.1, random_state=random_state, max_iter=10000),
        'ElasticNet': ElasticNet(alpha=0.1, l1_ratio=0.5, random_state=random_state, max_iter=10000),
        'Huber': HuberRegressor(max_iter=1000),
        'SVR': SVR(kernel='rbf', C=1.0, epsilon=0.1),
        'Random Forest': RandomForestRegressor(n_estimators=100, random_state=random_state, n_jobs=-1),
        'Gradient Boosting': GradientBoostingRegressor(n_estimators=100, random_state=random_state),
    }
    
    results = {}
    
    print("=" * 70)
    print("ADVANCED REGRESSION COMPARISON")
    print("=" * 70)
    
    for name, model in models.items():
        if name in ['SVR']:
            model.fit(X_train_scaled, y_train)
            y_pred = model.predict(X_test_scaled)
        else:
            model.fit(X_train, y_train)
            y_pred = model.predict(X_test)
        
        mse = mean_squared_error(y_test, y_pred)
        rmse = np.sqrt(mse)
        mae = mean_absolute_error(y_test, y_pred)
        r2 = r2_score(y_test, y_pred)
        
        cv_scores = cross_val_score(model, X_train, y_train, cv=5, scoring='r2')
        cv_mean = cv_scores.mean()
        cv_std = cv_scores.std()
        
        results[name] = {
            'MSE': mse,
            'RMSE': rmse,
            'MAE': mae,
            'R2': r2,
            'CV_R2_Mean': cv_mean,
            'CV_R2_Std': cv_std,
            'model': model
        }
        
        print(f"\n{name}:")
        print(f"  MSE:  {mse:.4f}")
        print(f"  RMSE: {rmse:.4f}")
        print(f"  MAE:  {mae:.4f}")
        print(f"  R2:   {r2:.4f}")
        print(f"  CV R2: {cv_mean:.4f} (+/- {cv_std:.4f})")
    
    print("\n" + "=" * 70)
    
    return results


def stacking_regressor_example(X, y, random_state=42):
    """
    Implement Stacking Regressor with multiple base models
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target vector
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    stacking_model : StackingRegressor
        Fitted stacking regressor model
    results : dict
        Dictionary containing performance metrics
    """
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=random_state)
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    base_estimators = [
        ('ridge', Ridge(alpha=1.0)),
        ('lasso', Lasso(alpha=0.1, max_iter=10000)),
        ('rf', RandomForestRegressor(n_estimators=50, random_state=random_state)),
        ('gb', GradientBoostingRegressor(n_estimators=50, random_state=random_state)),
        ('svr', SVR(kernel='rbf', C=1.0))
    ]
    
    stacking_model = StackingRegressor(
        estimators=base_estimators,
        final_estimator=BayesianRidge(),
        cv=5,
        n_jobs=-1
    )
    
    stacking_model.fit(X_train_scaled, y_train)
    y_pred = stacking_model.predict(X_test_scaled)
    
    mse = mean_squared_error(y_test, y_pred)
    rmse = np.sqrt(mse)
    mae = mean_absolute_error(y_test, y_pred)
    r2 = r2_score(y_test, y_pred)
    
    print("\n" + "=" * 70)
    print("STACKING REGRESSOR")
    print("=" * 70)
    print(f"\nMSE:  {mse:.4f}")
    print(f"RMSE: {rmse:.4f}")
    print(f"MAE:  {mae:.4f}")
    print(f"R2:   {r2:.4f}")
    print("=" * 70)
    
    results = {
        'MSE': mse,
        'RMSE': rmse,
        'MAE': mae,
        'R2': r2,
        'model': stacking_model
    }
    
    return stacking_model, results


def voting_regressor_example(X, y, random_state=42):
    """
    Implement Voting Regressor with multiple models
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target vector
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    voting_model : VotingRegressor
        Fitted voting regressor model
    results : dict
        Dictionary containing performance metrics
    """
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=random_state)
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    estimators = [
        ('ridge', Ridge(alpha=1.0)),
        ('rf', RandomForestRegressor(n_estimators=50, random_state=random_state)),
        ('gb', GradientBoostingRegressor(n_estimators=50, random_state=random_state))
    ]
    
    voting_model = VotingRegressor(estimators=estimators, n_jobs=-1)
    voting_model.fit(X_train_scaled, y_train)
    
    y_pred = voting_model.predict(X_test_scaled)
    
    mse = mean_squared_error(y_test, y_pred)
    rmse = np.sqrt(mse)
    mae = mean_absolute_error(y_test, y_pred)
    r2 = r2_score(y_test, y_pred)
    
    print("\n" + "=" * 70)
    print("VOTING REGRESSOR")
    print("=" * 70)
    print(f"\nMSE:  {mse:.4f}")
    print(f"RMSE: {rmse:.4f}")
    print(f"MAE:  {mae:.4f}")
    print(f"R2:   {r2:.4f}")
    print("=" * 70)
    
    results = {
        'MSE': mse,
        'RMSE': rmse,
        'MAE': mae,
        'R2': r2,
        'model': voting_model
    }
    
    return voting_model, results


def feature_engineering_for_regression(X, y, feature_names=None, random_state=42):
    """
    Advanced feature engineering techniques for regression
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target vector
    feature_names : list, optional
        Names of features
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X_engineered : array-like
        Engineered feature matrix
    new_feature_names : list
        Names of new features
    """
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=random_state)
    
    if feature_names is None:
        feature_names = [f'feature_{i}' for i in range(X.shape[1])]
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    poly = PolynomialFeatures(degree=2, include_bias=False, interaction_only=False)
    X_train_poly = poly.fit_transform(X_train_scaled)
    X_test_poly = poly.transform(X_test_scaled)
    
    poly_feature_names = poly.get_feature_names_out(feature_names)
    
    robust_scaler = RobustScaler()
    X_train_robust = robust_scaler.fit_transform(X_train)
    X_test_robust = robust_scaler.transform(X_test)
    
    minmax_scaler = MinMaxScaler()
    X_train_minmax = minmax_scaler.fit_transform(X_train)
    X_test_minmax = minmax_scaler.transform(X_test)
    
    X_combined_train = np.hstack([X_train_scaled, X_train_poly[:, len(feature_names):]])
    X_combined_test = np.hstack([X_test_scaled, X_test_poly[:, len(feature_names):]])
    
    print("\n" + "=" * 70)
    print("FEATURE ENGINEERING")
    print("=" * 70)
    print(f"\nOriginal features: {X.shape[1]}")
    print(f"Polynomial features: {X_train_poly.shape[1]}")
    print(f"Combined features: {X_combined_train.shape[1]}")
    
    model = Ridge(alpha=1.0)
    model.fit(X_train_scaled, y_train)
    y_pred = model.predict(X_test_scaled)
    r2_scaled = r2_score(y_test, y_pred)
    
    model.fit(X_train_poly, y_train)
    y_pred = model.predict(X_test_poly)
    r2_poly = r2_score(y_test, y_pred)
    
    model.fit(X_combined_train, y_train)
    y_pred = model.predict(X_combined_test)
    r2_combined = r2_score(y_test, y_pred)
    
    print(f"\nR2 with StandardScaler: {r2_scaled:.4f}")
    print(f"R2 with PolynomialFeatures: {r2_poly:.4f}")
    print(f"R2 with Combined: {r2_combined:.4f}")
    print("=" * 70)
    
    return X_combined_train, X_combined_test, list(poly_feature_names), {
        'R2_Scaled': r2_scaled,
        'R2_Poly': r2_poly,
        'R2_Combined': r2_combined
    }


def hyperparameter_tuning(X, y, random_state=42):
    """
    Hyperparameter tuning using GridSearchCV
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target vector
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    best_params : dict
        Best hyperparameters found
    best_model : model
        Best fitted model
    """
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=random_state)
    
    param_grid_ridge = {
        'alpha': [0.01, 0.1, 1.0, 10.0, 100.0]
    }
    
    ridge = Ridge()
    grid_search_ridge = GridSearchCV(ridge, param_grid_ridge, cv=5, scoring='r2', n_jobs=-1)
    grid_search_ridge.fit(X_train, y_train)
    
    best_ridge_params = grid_search_ridge.best_params_
    best_ridge_score = grid_search_ridge.best_score_
    
    param_grid_gb = {
        'n_estimators': [50, 100, 200],
        'learning_rate': [0.01, 0.05, 0.1],
        'max_depth': [3, 5, 7]
    }
    
    gb = GradientBoostingRegressor(random_state=random_state)
    grid_search_gb = GridSearchCV(gb, param_grid_gb, cv=5, scoring='r2', n_jobs=-1)
    grid_search_gb.fit(X_train, y_train)
    
    best_gb_params = grid_search_gb.best_params_
    best_gb_score = grid_search_gb.best_score_
    
    param_grid_svr = {
        'C': [0.1, 1.0, 10.0],
        'epsilon': [0.01, 0.1, 0.2],
        'kernel': ['rbf', 'linear']
    }
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    svr = SVR()
    grid_search_svr = GridSearchCV(svr, param_grid_svr, cv=5, scoring='r2', n_jobs=-1)
    grid_search_svr.fit(X_train_scaled, y_train)
    
    best_svr_params = grid_search_svr.best_params_
    best_svr_score = grid_search_svr.best_score_
    
    print("\n" + "=" * 70)
    print("HYPERPARAMETER TUNING RESULTS")
    print("=" * 70)
    print(f"\nRidge - Best params: {best_ridge_params}, Best CV R2: {best_ridge_score:.4f}")
    print(f"Gradient Boosting - Best params: {best_gb_params}, Best CV R2: {best_gb_score:.4f}")
    print(f"SVR - Best params: {best_svr_params}, Best CV R2: {best_svr_score:.4f}")
    print("=" * 70)
    
    return {
        'ridge': best_ridge_params,
        'gradient_boosting': best_gb_params,
        'svr': best_svr_params
    }, {
        'ridge': grid_search_ridge.best_estimator_,
        'gradient_boosting': grid_search_gb.best_estimator_,
        'svr': grid_search_svr.best_estimator_
    }


def pipeline_with_preprocessing(random_state=42):
    """
    Create and demonstrate pipeline with preprocessing
    
    Returns:
    --------
    pipeline : Pipeline
        Complete pipeline with preprocessing
    """
    X, y, _ = generate_complex_data(n_samples=500, random_state=random_state)
    
    pipeline = Pipeline([
        ('scaler', StandardScaler()),
        ('poly', PolynomialFeatures(degree=2, include_bias=False)),
        ('ridge', Ridge(alpha=1.0))
    ])
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=random_state)
    
    pipeline.fit(X_train, y_train)
    y_pred = pipeline.predict(X_test)
    
    mse = mean_squared_error(y_test, y_pred)
    r2 = r2_score(y_test, y_pred)
    
    print("\n" + "=" * 70)
    print("PIPELINE WITH PREPROCESSING")
    print("=" * 70)
    print(f"\nMSE: {mse:.4f}")
    print(f"R2: {r2:.4f}")
    print("=" * 70)
    
    return pipeline, {'MSE': mse, 'R2': r2}


def banking_example(random_state=42):
    """
    Banking/Finance example: Credit Risk and Interest Prediction
    
    This example demonstrates:
    - Loan default probability prediction
    - Interest charged prediction
    - Loan approval amount prediction
    
    Parameters:
    -----------
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Dictionary containing results for each prediction task
    """
    print("\n" + "=" * 70)
    print("BANKING EXAMPLE: CREDIT RISK PREDICTION")
    print("=" * 70)
    
    X, y_default, y_interest, y_approval, feature_names = generate_banking_data(n_samples=1000, random_state=random_state)
    
    print(f"\nDataset shape: {X.shape}")
    print(f"Feature names: {feature_names}")
    print(f"Average default rate: {y_default.mean():.2%}")
    print(f"Average interest charged: ${y_interest.mean():.2f}")
    print(f"Average approval amount: ${y_approval.mean():.2f}")
    
    print("\n" + "-" * 50)
    print("Predicting Loan Default Probability")
    print("-" * 50)
    
    default_results = advanced_regression_comparison(X, y_default, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Predicting Interest Charged")
    print("-" * 50)
    
    interest_results = advanced_regression_comparison(X, y_interest, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Predicting Loan Approval Amount")
    print("-" * 50)
    
    approval_results = advanced_regression_comparison(X, y_approval, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Stacking Regressor for Banking")
    print("-" * 50)
    
    stacking_default, stacking_results = stacking_regressor_example(X, y_default, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Hyperparameter Tuning for Banking")
    print("-" * 50)
    
    best_params, best_models = hyperparameter_tuning(X, y_interest, random_state=random_state)
    
    print("\n" + "=" * 70)
    print("BANKING EXAMPLE COMPLETED")
    print("=" * 70)
    
    return {
        'default_prediction': default_results,
        'interest_prediction': interest_results,
        'approval_prediction': approval_results,
        'stacking': stacking_results,
        'hyperparameter_tuning': best_params
    }


def healthcare_example(random_state=42):
    """
    Healthcare example: Patient Outcome Prediction
    
    This example demonstrates:
    - Healthcare cost prediction
    - Length of stay prediction
    - Readmission probability prediction
    
    Parameters:
    -----------
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Dictionary containing results for each prediction task
    """
    print("\n" + "=" * 70)
    print("HEALTHCARE EXAMPLE: PATIENT OUTCOME PREDICTION")
    print("=" * 70)
    
    X, y_cost, y_stay, y_readmit, feature_names = generate_healthcare_data(n_samples=1000, random_state=random_state)
    
    print(f"\nDataset shape: {X.shape}")
    print(f"Feature names: {feature_names}")
    print(f"Average healthcare cost: ${y_cost.mean():.2f}")
    print(f"Average length of stay: {y_stay.mean():.2f} days")
    print(f"Readmission rate: {y_readmit.mean():.2%}")
    
    print("\n" + "-" * 50)
    print("Predicting Healthcare Costs")
    print("-" * 50)
    
    cost_results = advanced_regression_comparison(X, y_cost, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Predicting Length of Stay")
    print("-" * 50)
    
    stay_results = advanced_regression_comparison(X, y_stay, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Predicting Readmission Probability")
    print("-" * 50)
    
    readmit_results = advanced_regression_comparison(X, y_readmit, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Feature Engineering for Healthcare")
    print("-" * 50)
    
    X_engineered, _, engineered_names, fe_results = feature_engineering_for_regression(X, y_cost, feature_names, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Voting Regressor for Healthcare")
    print("-" * 50)
    
    voting_healthcare, voting_results = voting_regressor_example(X, y_cost, random_state=random_state)
    
    print("\n" + "-" * 50)
    print("Pipeline for Healthcare")
    print("-" * 50)
    
    pipeline_healthcare, pipeline_results = pipeline_with_preprocessing(random_state=random_state)
    
    print("\n" + "=" * 70)
    print("HEALTHCARE EXAMPLE COMPLETED")
    print("=" * 70)
    
    return {
        'cost_prediction': cost_results,
        'stay_prediction': stay_results,
        'readmit_prediction': readmit_results,
        'feature_engineering': fe_results,
        'voting': voting_results,
        'pipeline': pipeline_results
    }


def main():
    """
    Main function to execute the Advanced Regression Techniques implementation
    """
    print("=" * 70)
    print("ADVANCED REGRESSION TECHNIQUES")
    print("Comprehensive Implementation with Scikit-Learn")
    print("=" * 70)
    print("\nExecuting Advanced Regression Techniques implementation...")
    
    random_state = 42
    
    X, y, feature_names = generate_complex_data(n_samples=500, random_state=random_state)
    print(f"\nGenerated complex data: {X.shape}")
    
    print("\n" + "=" * 70)
    print("SECTION 1: ADVANCED REGRESSION COMPARISON")
    print("=" * 70)
    comparison_results = advanced_regression_comparison(X, y, random_state=random_state)
    
    print("\n" + "=" * 70)
    print("SECTION 2: STACKING REGRESSOR")
    print("=" * 70)
    stacking_model, stacking_results = stacking_regressor_example(X, y, random_state=random_state)
    
    print("\n" + "=" * 70)
    print("SECTION 3: VOTING REGRESSOR")
    print("=" * 70)
    voting_model, voting_results = voting_regressor_example(X, y, random_state=random_state)
    
    print("\n" + "=" * 70)
    print("SECTION 4: FEATURE ENGINEERING")
    print("=" * 70)
    X_engineered, _, engineered_names, fe_results = feature_engineering_for_regression(X, y, feature_names, random_state=random_state)
    
    print("\n" + "=" * 70)
    print("SECTION 5: HYPERPARAMETER TUNING")
    print("=" * 70)
    best_params, best_models = hyperparameter_tuning(X, y, random_state=random_state)
    
    print("\n" + "=" * 70)
    print("SECTION 6: PIPELINE WITH PREPROCESSING")
    print("=" * 70)
    pipeline_model, pipeline_results = pipeline_with_preprocessing(random_state=random_state)
    
    banking_results = banking_example(random_state=random_state)
    
    healthcare_results = healthcare_example(random_state=random_state)
    
    print("\n" + "=" * 70)
    print("IMPLEMENTATION COMPLETE")
    print("=" * 70)
    print("\nAll advanced regression techniques have been demonstrated:")
    print("- Bayesian Ridge Regression")
    print("- Stacking Regressor")
    print("- Voting Regressor")
    print("- Pipeline with Preprocessing")
    print("- Feature Engineering")
    print("- Hyperparameter Tuning")
    print("- Banking Example")
    print("- Healthcare Example")
    print("=" * 70)
    
    return {
        'comparison': comparison_results,
        'stacking': stacking_results,
        'voting': voting_results,
        'feature_engineering': fe_results,
        'hyperparameter_tuning': best_params,
        'pipeline': pipeline_results,
        'banking': banking_results,
        'healthcare': healthcare_results
    }


if __name__ == "__main__":
    results = main()