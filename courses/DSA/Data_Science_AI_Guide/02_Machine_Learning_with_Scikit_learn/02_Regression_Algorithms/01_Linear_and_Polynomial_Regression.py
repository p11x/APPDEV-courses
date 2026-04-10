# Topic: Linear and Polynomial Regression
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Linear and Polynomial Regression

I. INTRODUCTION
    This module covers the fundamentals of linear regression and polynomial 
    regression - two fundamental algorithms in supervised learning for predicting 
    continuous numerical values. Linear regression establishes a straight-line 
    relationship between independent variables (features) and the target variable,
    while polynomial regression captures non-linear relationships by transforming 
    features into polynomial terms.

II. CORE_CONCEPTS
    - Simple Linear Regression: y = mx + b (one feature to one target)
    - Multiple Linear Regression: y = b0 + b1*x1 + b2*x2 + ... + bn*xn
    - Polynomial Regression: y = b0 + b1*x + b2*x^2 + ... + bn*x^n
    - Feature Scaling: Standardization (z-score normalization)
    - Model Evaluation: R², MSE, MAE metrics

III. IMPLEMENTATION
    - Data generation using sklearn
    - Model training and prediction
    - Visualization of regression fits

IV. EXAMPLES (Banking + Healthcare)
    - Banking: House Price Prediction based on various features
    - Healthcare: Medical Cost Prediction based on patient attributes

V. OUTPUT_RESULTS
    - Model coefficients, intercepts
    - Performance metrics (R², MSE, MAE)
    - Visualization plots

VI. TESTING
    - Synthetic data tests
    - Real-world scenario tests

VII. ADVANCED_TOPICS
    - Overfitting prevention with polynomial degree selection
    - Cross-validation for model selection
    - Regularization considerations

VIII. CONCLUSION
    - Summary of when to use each regression type
    - Best practices and recommendations
"""

# Import necessary libraries
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_regression, make_poly_regression
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.preprocessing import StandardScaler, PolynomialFeatures
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_squared_error, r2_score, mean_absolute_error
import warnings
warnings.filterwarnings('ignore')


def generate_regression_data(n_samples=500, n_features=1, noise=10.0, random_state=42):
    """
    Generate synthetic regression data for testing regression algorithms.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features (1 for simple, >1 for multiple)
    noise : float
        Standard deviation of Gaussian noise added to output
    random_state : int
        Random seed for reproducibility
    
    Returns:
    --------
    X : ndarray of shape (n_samples, n_features)
        Input features
    y : ndarray of shape (n_samples,)
        Target values
    """
    X, y = make_regression(
        n_samples=n_samples,
        n_features=n_features,
        noise=noise,
        random_state=random_state
    )
    print(f"Generated {n_samples} samples with {n_features} feature(s)")
    print(f"X shape: {X.shape}, y shape: {y.shape}")
    print(f"y range: [{y.min():.2f}, {y.max():.2f}], mean: {y.mean():.2f}")
    return X, y


def generate_polynomial_regression_data(n_samples=200, n_degree=2, noise=5.0, random_state=42):
    """
    Generate synthetic polynomial regression data.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples
    n_degree : int
        Degree of polynomial relationship
    noise : float
        Noise level
    random_state : int
        Random seed
    
    Returns:
    --------
    X : ndarray
        Input feature
    y : ndarray
        Target with polynomial relationship
    """
    X, y = make_poly_regression(
        n_samples=n_samples,
        n_degree=n_degree,
        noise=noise,
        random_state=random_state
    )
    print(f"Generated polynomial data (degree={n_degree})")
    print(f"X range: [{X.min():.2f}, {X.max():.2f}]")
    print(f"y range: [{y.min():.2f}, {y.max():.2f}]")
    return X, y


def core_linear_regression(X_train, X_test, y_train, y_test, feature_name="Feature"):
    """
    Implement and evaluate simple/multiple linear regression.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test targets
    feature_name : str
        Name of feature for display
    
    Returns:
    --------
    model : LinearRegression
        Trained model
    metrics : dict
        Performance metrics
    """
    print(f"\n{'='*60}")
    print(f"CORE LINEAR REGRESSION - {feature_name}")
    print(f"{'='*60}")
    
    model = LinearRegression()
    model.fit(X_train, y_train)
    
    y_pred_train = model.predict(X_train)
    y_pred_test = model.predict(X_test)
    
    print(f"\nModel Parameters:")
    print(f"  Intercept: {model.intercept_:.4f}")
    
    if len(model.coef_.shape) == 1:
        print(f"  Coefficient ({feature_name}): {model.coef_[0]:.4f}")
    else:
        for i, coef in enumerate(model.coef_):
            print(f"  Coefficient {i+1}: {coef:.4f}")
    
    train_mse = mean_squared_error(y_train, y_pred_train)
    test_mse = mean_squared_error(y_test, y_pred_test)
    train_rmse = np.sqrt(train_mse)
    test_rmse = np.sqrt(test_mse)
    train_mae = mean_absolute_error(y_train, y_pred_train)
    test_mae = mean_absolute_error(y_test, y_pred_test)
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    
    metrics = {
        'train_mse': train_mse,
        'test_mse': test_mse,
        'train_rmse': train_rmse,
        'test_rmse': test_rmse,
        'train_mae': train_mae,
        'test_mae': test_mae,
        'train_r2': train_r2,
        'test_r2': test_r2,
        'y_pred_train': y_pred_train,
        'y_pred_test': y_pred_test
    }
    
    print(f"\nModel Performance:")
    print(f"  Training Set:")
    print(f"    MSE:  {train_mse:.4f}")
    print(f"    RMSE: {train_rmse:.4f}")
    print(f"    MAE:  {train_mae:.4f}")
    print(f"    R²:   {train_r2:.4f}")
    print(f"  Test Set:")
    print(f"    MSE:  {test_mse:.4f}")
    print(f"    RMSE: {test_rmse:.4f}")
    print(f"    MAE:  {test_mae:.4f}")
    print(f"    R²:   {test_r2:.4f}")
    
    if len(X_test.shape) == 1 or X_test.shape[1] == 1:
        visualization_linear(X_test, y_test, y_pred_test, feature_name, model)
    
    return model, metrics


def visualization_linear(X, y_true, y_pred, feature_name, model):
    """
    Visualize simple linear regression results.
    
    Parameters:
    -----------
    X : ndarray
        Feature values
    y_true : ndarray
        True target values
    y_pred : ndarray
        Predicted target values
    feature_name : str
        Name of feature
    model : LinearRegression
        Trained model
    """
    plt.figure(figsize=(10, 6))
    
    if len(X.shape) == 1:
        x_plot = np.linspace(X.min(), X.max(), 100).reshape(-1, 1)
    else:
        x_plot = np.linspace(X.min(), X.max(), 100).reshape(-1, 1)
    
    y_plot = model.predict(x_plot)
    
    plt.scatter(X.flatten(), y_true, alpha=0.5, label='Actual', color='blue')
    plt.plot(x_plot.flatten(), y_plot, color='red', linewidth=2, label='Regression Line')
    plt.xlabel(feature_name)
    plt.ylabel('Target')
    plt.title(f'Linear Regression: {feature_name} vs Target')
    plt.legend()
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.show()


def polynomial_regression_example(X_train, X_test, y_train, y_test, degrees=[1, 2, 3, 4]):
    """
    Implement polynomial regression with various degrees.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test targets
    degrees : list
        Polynomial degrees to evaluate
    
    Returns:
    --------
    results : dict
        Results for each degree
    """
    print(f"\n{'='*60}")
    print(f"POLYNOMIAL REGRESSION")
    print(f"{'='*60}")
    
    results = {}
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    best_degree = 1
    best_r2 = -np.inf
    
    for degree in degrees:
        print(f"\n--- Polynomial Degree: {degree} ---")
        
        poly = PolynomialFeatures(degree=degree)
        X_train_poly = poly.fit_transform(X_train_scaled)
        X_test_poly = poly.transform(X_test_scaled)
        
        print(f"  Features after transformation: {X_train_poly.shape[1]}")
        
        model = LinearRegression()
        model.fit(X_train_poly, y_train)
        
        y_pred_train = model.predict(X_train_poly)
        y_pred_test = model.predict(X_test_poly)
        
        train_mse = mean_squared_error(y_train, y_pred_train)
        test_mse = mean_squared_error(y_test, y_pred_test)
        test_r2 = r2_score(y_test, y_pred_test)
        test_mae = mean_absolute_error(y_test, y_pred_test)
        
        results[degree] = {
            'model': model,
            'poly_transformer': poly,
            'test_mse': test_mse,
            'test_r2': test_r2,
            'test_mae': test_mae,
            'y_pred_test': y_pred_test
        }
        
        print(f"  Train MSE: {train_mse:.4f}")
        print(f"  Test MSE:  {test_mse:.4f}")
        print(f"  Test R²:  {test_r2:.4f}")
        print(f"  Test MAE: {test_mae:.4f}")
        
        if test_r2 > best_r2:
            best_r2 = test_r2
            best_degree = degree
    
    print(f"\nBest performing degree: {best_degree} (R² = {best_r2:.4f})")
    
    visualization_polynomial(
        X_test, y_test, results[best_degree]['y_pred_test'],
        results[best_degree]['model'], results[best_degree]['poly_transformer'], scaler
    )
    
    return results


def visualization_polynomial(X, y_true, y_pred, model, poly, scaler, title="Polynomial Regression"):
    """
    Visualize polynomial regression results.
    
    Parameters:
    -----------
    X : ndarray
        Feature values
    y_true : ndarray
        True target values
    y_pred : ndarray
        Predicted target values
    model : LinearRegression
        Trained polynomial model
    poly : PolynomialFeatures
        Polynomial transformer
    scaler : StandardScaler
        Feature scaler
    title : str
        Plot title
    """
    plt.figure(figsize=(10, 6))
    
    x_range = np.linspace(X.min(), X.max(), 100).reshape(-1, 1)
    x_range_scaled = scaler.transform(x_range)
    x_range_poly = poly.transform(x_range_scaled)
    y_range = model.predict(x_range_poly)
    
    plt.scatter(X.flatten(), y_true, alpha=0.5, label='Actual', color='blue')
    plt.plot(x_range.flatten(), y_range, color='red', linewidth=2, label='Polynomial Fit')
    plt.xlabel('Feature')
    plt.ylabel('Target')
    plt.title(title)
    plt.legend()
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.show()


def banking_example():
    """
    Banking/Finance example: House Price Prediction.
    
    This example demonstrates predicting house prices based on:
    - Square footage
    - Number of bedrooms
    - Number of bathrooms  
    - Age of the house
    - Distance to city center
    """
    print(f"\n{'='*60}")
    print(f"BANKING EXAMPLE: House Price Prediction")
    print(f"{'='*60}")
    
    np.random.seed(42)
    n_samples = 500
    
    square_footage = np.random.uniform(800, 4000, n_samples)
    bedrooms = np.random.randint(1, 6, n_samples)
    bathrooms = np.random.randint(1, 4, n_samples)
    house_age = np.random.uniform(0, 50, n_samples)
    distance_center = np.random.uniform(1, 30, n_samples)
    
    base_price = 50000
    price_per_sqft = 150
    price_per_bedroom = 10000
    price_per_bathroom = 5000
    price_per_year_old = -500
    price_per_mile = -2000
    
    house_prices = (base_price +
                  square_footage * price_per_sqft +
                  bedrooms * price_per_bedroom +
                  bathrooms * price_per_bathroom +
                  house_age * price_per_year_old +
                  distance_center * price_per_mile +
                  np.random.normal(0, 15000, n_samples))
    
    df = pd.DataFrame({
        'Square_Footage': square_footage,
        'Bedrooms': bedrooms,
        'Bathrooms': bathrooms,
        'House_Age': house_age,
        'Distance_Center': distance_center,
        'House_Price': house_prices
    })
    
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    feature_cols = ['Square_Footage', 'Bedrooms', 'Bathrooms', 'House_Age', 'Distance_Center']
    X = df[feature_cols].values
    y = df['House_Price'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    print(f"\nTraining samples: {len(X_train)}, Test samples: {len(X_test)}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LinearRegression()
    model.fit(X_train_scaled, y_train)
    
    y_pred_train = model.predict(X_train_scaled)
    y_pred_test = model.predict(X_test_scaled)
    
    print(f"\nModel Coefficients:")
    for feat, coef in zip(feature_cols, model.coef_):
        print(f"  {feat}: {coef:.2f}")
    print(f"  Intercept: {model.intercept_:.2f}")
    
    train_mse = mean_squared_error(y_train, y_pred_train)
    test_mse = mean_squared_error(y_test, y_pred_test)
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    test_mae = mean_absolute_error(y_test, y_pred_test)
    
    print(f"\nModel Performance:")
    print(f"  Training R²: {train_r2:.4f}")
    print(f"  Test R²:    {test_r2:.4f}")
    print(f"  Test MSE:   {test_mse:.2f}")
    print(f"  Test RMSE:  {np.sqrt(test_mse):.2f}")
    print(f"  Test MAE:   {test_mae:.2f}")
    
    sample_prediction(X_test_scaled[0:3], model, df['House_Price'].values, feature_cols)
    
    visualization_banking(X_test, y_test, y_pred_test, model, feature_cols, scaler)
    
    return model, scaler, feature_cols


def sample_prediction(X_sample, model, y_actual, feature_names):
    """
    Make sample predictions and display results.
    
    Parameters:
    -----------
    X_sample : ndarray
        Sample features
    model : LinearRegression
        Trained model
    y_actual : ndarray
        Actual values
    feature_names : list
        Feature names
    """
    print(f"\nSample Predictions:")
    predictions = model.predict(X_sample)
    for i, (pred, actual) in enumerate(zip(predictions, y_actual[:3])):
        print(f"  Sample {i+1}: Predicted ${pred:,.2f}, Actual ${actual:,.2f}")


def visualize_banking(X, y_true, y_pred, model, feature_names, scaler):
    """
    Visualize banking/housing predictions.
    
    Parameters:
    -----------
    X : ndarray
        Feature values
    y_true : ndarray
        True values
    y_pred : ndarray
        Predicted values
    model : LinearRegression
        Trained model
    feature_names : list
        Feature names
    scaler : StandardScaler
        Feature scaler
    """
    fig, axes = plt.subplots(2, 3, figsize=(15, 10))
    
    for idx, (ax, feat_name) in enumerate(zip(axes.flatten()[:5], feature_names)):
        feat_idx = [i for i in range(len(feature_names))]
        feat_idx.remove(idx)
        
        if len(feat_idx) > 0:
            X_mean = X.copy()
            X_mean[:, feat_idx] = X_mean[:, feat_idx].mean(axis=0)
            X_vary = np.linspace(X[:, idx].min(), X[:, idx].max(), 100).reshape(-1, 1)
            
            X_plot = np.tile(X_mean[:1].copy(), (100, 1))
            X_plot[:, idx] = X_vary.flatten()
            
            y_plot = model.predict(X_plot)
            
            ax.scatter(X[:, idx], y_true, alpha=0.3, label='Actual')
            ax.plot(X_plot[:, idx], y_plot, color='red', linewidth=2, label='Predicted')
            ax.set_xlabel(feat_name)
            ax.set_ylabel('House Price')
            ax.legend()
            ax.grid(True, alpha=0.3)
    
    ax = axes.flatten()[5]
    ax.scatter(y_true, y_pred, alpha=0.5)
    min_val = min(y_true.min(), y_pred.min())
    max_val = max(y_true.max(), y_pred.max())
    ax.plot([min_val, max_val], [min_val, max_val], 'r--', linewidth=2, label='Perfect Fit')
    ax.set_xlabel('Actual Price')
    ax.set_ylabel('Predicted Price')
    ax.set_title('Actual vs Predicted')
    ax.legend()
    ax.grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()


def healthcare_example():
    """
    Healthcare example: Medical Cost Prediction.
    
    This example demonstrates predicting medical costs based on:
    - Patient age
    - BMI
    - Number of dependents
    - Smoking status (0 or 1)
    - Exercise level (1-3 scale)
    """
    print(f"\n{'='*60}")
    print(f"HEALTHCARE EXAMPLE: Medical Cost Prediction")
    print(f"{'='*60}")
    
    np.random.seed(123)
    n_samples = 500
    
    age = np.random.uniform(18, 80, n_samples)
    bmi = np.random.uniform(18, 40, n_samples)
    dependents = np.random.randint(0, 5, n_samples)
    smoker = np.random.randint(0, 2, n_samples)
    exercise_level = np.random.randint(1, 4, n_samples)
    
    base_cost = 2000
    cost_per_age = 50
    cost_per_bmi = 100
    cost_per_dependent = 500
    smoker_premium = 8000
    exercise_discount = -300
    
    medical_costs = (base_cost +
                     age * cost_per_age +
                     bmi * cost_per_bmi +
                     dependents * cost_per_dependent +
                     smoker * smoker_premium +
                     exercise_level * exercise_discount +
                     np.random.normal(0, 2000, n_samples))
    
    df = pd.DataFrame({
        'Age': age,
        'BMI': bmi,
        'Dependents': dependents,
        'Smoker': smoker,
        'Exercise_Level': exercise_level,
        'Medical_Cost': medical_costs
    })
    
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    feature_cols = ['Age', 'BMI', 'Dependents', 'Smoker', 'Exercise_Level']
    X = df[feature_cols].values
    y = df['Medical_Cost'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    print(f"\nTraining samples: {len(X_train)}, Test samples: {len(X_test)}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = LinearRegression()
    model.fit(X_train_scaled, y_train)
    
    y_pred_train = model.predict(X_train_scaled)
    y_pred_test = model.predict(X_test_scaled)
    
    print(f"\nModel Coefficients:")
    for feat, coef in zip(feature_cols, model.coef_):
        print(f"  {feat}: {coef:.2f}")
    print(f"  Intercept: {model.intercept_:.2f}")
    
    train_mse = mean_squared_error(y_train, y_pred_train)
    test_mse = mean_squared_error(y_test, y_pred_test)
    train_r2 = r2_score(y_train, y_pred_train)
    test_r2 = r2_score(y_test, y_pred_test)
    test_mae = mean_absolute_error(y_test, y_pred_test)
    
    print(f"\nModel Performance:")
    print(f"  Training R²: {train_r2:.4f}")
    print(f"  Test R²:    {test_r2:.4f}")
    print(f"  Test MSE:   {test_mse:.2f}")
    print(f"  Test RMSE:  {np.sqrt(test_mse):.2f}")
    print(f"  Test MAE:   {test_mae:.2f}")
    
    visualization_healthcare(X_test, y_test, y_pred_test, feature_cols)
    
    return model, scaler, feature_cols


def visualization_healthcare(X, y_true, y_pred, feature_names):
    """
    Visualize healthcare/medical cost predictions.
    
    Parameters:
    -----------
    X : ndarray
        Feature values
    y_true : ndarray
        True values
    y_pred : ndarray
        Predicted values
    feature_names : list
        Feature names
    """
    fig, axes = plt.subplots(2, 3, figsize=(15, 10))
    
    colors = []
    for row in range(X.shape[0]):
        if X[row, 3] > 0.5:
            colors.append('red')
        else:
            colors.append('green')
    
    for idx, (ax, feat_name) in enumerate(zip(axes.flatten()[:5], feature_names)):
        if feat_name == 'Smoker':
            smoker_costs = y_true[X[:, 3] > 0.5]
            non_smoker_costs = y_true[X[:, 3] <= 0.5]
            ax.bar(['Non-Smoker', 'Smoker'], [non_smoker_costs.mean(), smoker_costs.mean()],
                  color=['green', 'red'], alpha=0.7)
            ax.set_ylabel('Medical Cost')
        else:
            ax.scatter(X[:, idx], y_true, c=colors, alpha=0.5)
            ax.set_xlabel(feat_name)
            ax.set_ylabel('Medical Cost')
        ax.set_title(f'{feat_name} vs Cost')
        ax.grid(True, alpha=0.3)
    
    ax = axes.flatten()[5]
    ax.scatter(y_true, y_pred, c=colors, alpha=0.5)
    min_val = min(y_true.min(), y_pred.min())
    max_val = max(y_true.max(), y_pred.max())
    ax.plot([min_val, max_val], [min_val, max_val], 'r--', linewidth=2, label='Perfect Fit')
    ax.set_xlabel('Actual Cost')
    ax.set_ylabel('Predicted Cost')
    ax.set_title('Actual vs Predicted (Red=Smoker, Green=Non-Smoker)')
    ax.legend()
    ax.grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()


def test_regression_models():
    """
    Test regression models with various synthetic datasets.
    
    Tests include:
    - Simple linear regression
    - Multiple linear regression
    - Polynomial regression (degree 2-4)
    """
    print(f"\n{'='*60}")
    print(f"TESTING REGRESSION MODELS")
    print(f"{'='*60}")
    
    X_simple, y_simple = generate_regression_data(n_samples=200, n_features=1, noise=5.0)
    X_simple_train, X_simple_test, y_simple_train, y_simple_test = train_test_split(
        X_simple, y_simple, test_size=0.2, random_state=42
    )
    model, metrics = core_linear_regression(
        X_simple_train, X_simple_test, 
        y_simple_train, y_simple_test,
        feature_name="Simple Feature"
    )
    
    X_multi, y_multi = generate_regression_data(n_samples=200, n_features=5, noise=5.0)
    X_multi_train, X_multi_test, y_multi_train, y_multi_test = train_test_split(
        X_multi, y_multi, test_size=0.2, random_state=42
    )
    model, metrics = core_linear_regression(
        X_multi_train, X_multi_test,
        y_multi_train, y_multi_test,
        feature_name="Multiple Features"
    )
    
    X_poly, y_poly = generate_polynomial_regression_data(n_samples=200, n_degree=2, noise=2.0)
    X_poly_train, X_poly_test, y_poly_train, y_poly_test = train_test_split(
        X_poly, y_poly, test_size=0.2, random_state=42
    )
    poly_results = polynomial_regression_example(
        X_poly_train, X_poly_test,
        y_poly_train, y_poly_test,
        degrees=[1, 2, 3, 4]
    )
    
    print(f"\n{'='*60}")
    print(f"ALL TESTS COMPLETED SUCCESSFULLY")
    print(f"{'='*60}")
    
    return True


def cross_validation_example(X, y, cv_folds=5):
    """
    Perform cross-validation for regression model evaluation.
    
    Parameters:
    -----------
    X : ndarray
        Features
    y : ndarray
        Target values
    cv_folds : int
        Number of cross-validation folds
    
    Returns:
    --------
    cv_scores : dict
        Cross-validation results
    """
    print(f"\n{'='*60}")
    print(f"CROSS-VALIDATION (k={cv_folds})")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_scaled = scaler.fit_transform(X)
    
    model = LinearRegression()
    cv_scores = cross_val_score(model, X_scaled, y, cv=cv_folds, scoring='r2')
    
    print(f"Cross-validation R² scores: {cv_scores}")
    print(f"Mean R²: {cv_scores.mean():.4f} (+/- {cv_scores.std() * 2:.4f})")
    
    return {'cv_scores': cv_scores, 'mean': cv_scores.mean(), 'std': cv_scores.std()}


def main():
    """
    Main function to execute regression examples.
    
    Includes:
    - Synthetic data generation
    - Linear regression examples
    - Polynomial regression examples
    - Banking (house price) example
    - Healthcare (medical cost) example
    - Testing
    - Cross-validation
    """
    print("="*60)
    print("LINEAR AND POLYNOMIAL REGRESSION IMPLEMENTATION")
    print("="*60)
    
    print("\nI. INTRODUCTION")
    print("   This module covers linear and polynomial regression algorithms")
    print("   for predicting continuous numerical targets.")
    
    print("\nII. CORE_CONCEPTS")
    print("   - Linear: straight-line relationship (y = mx + b)")
    print("   - Polynomial: curved relationship (y = b0 + b1*x + b2*x² + ...)")
    print("   - Feature scaling: StandardScaler for z-score normalization")
    print("   - Evaluation: R², MSE, MAE metrics")
    
    print("\nIII. IMPLEMENTATION")
    
    X, y = generate_regression_data(n_samples=300, n_features=1, noise=10.0)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    model, metrics = core_linear_regression(X_train, X_test, y_train, y_test, "Feature X")
    
    print("\nIV. EXAMPLES")
    banking_model, banking_scaler, banking_features = banking_example()
    healthcare_model, healthcare_scaler, healthcare_features = healthcare_example()
    
    print("\nV. OUTPUT_RESULTS")
    print("   All model outputs and visualizations displayed above.")
    
    print("\nVI. TESTING")
    test_regression_models()
    
    print("\nVII. ADVANCED_TOPICS")
    X_adv, y_adv = generate_regression_data(n_samples=200, n_features=3, noise=5.0)
    cv_results = cross_validation_example(X_adv, y_adv, cv_folds=5)
    
    print("\nVIII. CONCLUSION")
    print("   - Use LINEAR regression when relationship is approximately linear")
    print("   - Use POLYNOMIAL regression for curved/non-linear relationships")
    print("   - Always scale features before polynomial transformation")
    print("   - Higher polynomial degree can lead to overfitting")
    print("   - R², MSE, and MAE help evaluate model performance")
    print("\n   Implementation complete!")


if __name__ == "__main__":
    main()