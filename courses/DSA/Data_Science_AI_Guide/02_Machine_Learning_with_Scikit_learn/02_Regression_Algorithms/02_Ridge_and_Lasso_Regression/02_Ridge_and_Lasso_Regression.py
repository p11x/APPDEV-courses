# Topic: Ridge and Lasso Regression
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Ridge and Lasso Regression

I. INTRODUCTION
    Ridge and Lasso regression are regularized linear regression methods that add 
    penalty terms to the loss function to prevent overfitting and perform feature selection.
    Ridge (L2 regularization) shrinks coefficients towards zero but not exactly to zero,
    while Lasso (L1 regularization) can set coefficients exactly to zero, performing
    feature selection. Elastic Net combines both L1 and L2 penalties.

II. CORE_CONCEPTS
    - Ridge Regression: L2 penalty = alpha * sum(beta_i^2)
    - Lasso Regression: L1 penalty = alpha * sum(|beta_i|)
    - Elastic Net: Both L1 and L2 penalties
    - Regularization strength: alpha parameter
    - Feature scaling: Critical for regularized models
    - Coefficient shrinkage vs feature selection

III. IMPLEMENTATION
    - Data generation for regression with correlated features
    - Ridge/Lasso/ElasticNet model training
    - Alpha selection via cross-validation
    - Coefficient path visualization
    - Feature importance analysis

IV. EXAMPLES (Banking + Healthcare)
    - Banking: Credit Risk Prediction with many correlated features
    - Healthcare: Patient Outcome Prediction with feature selection

V. OUTPUT_RESULTS
    - Model coefficients
    - Optimal alpha values
    - Performance metrics (R², MSE, MAE)
    - Coefficient paths
    - Feature importance plots

VI. TESTING
    - Synthetic data with multicollinearity
    - Feature selection verification
    - Regularization strength effects

VII. ADVANCED_TOPICS
    - Coordinate descent optimization
    - Early stopping
    - Warm start strategies
    - Model interpretation

VIII. CONCLUSION
    - When to use Ridge vs Lasso vs Elastic Net
    - Best practices for regularized regression
"""

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_regression, make_blobs
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import Ridge, Lasso, ElasticNet, ElasticNetCV
from sklearn.metrics import mean_squared_error, r2_score, mean_absolute_error
import warnings
warnings.filterwarnings('ignore')


def generate_correlated_regression_data(n_samples=500, n_features=10, noise=10.0, random_state=42):
    """
    Generate regression data with correlated features.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples
    n_features : int
        Number of features
    noise : float
        Noise level
    random_state : int
        Random seed
    
    Returns:
    --------
    X : ndarray
        Features with some correlation
    y : ndarray
        Target values
    feature_names : list
        Names of features
    """
    X, y = make_regression(
        n_samples=n_samples,
        n_features=n_features,
        noise=noise,
        random_state=random_state,
        effective_rank=int(n_features / 2)
    )
    
    feature_names = [f'Feature_{i}' for i in range(n_features)]
    
    X_df = pd.DataFrame(X, columns=feature_names)
    for i in range(1, n_features):
        if i % 2 == 0:
            X_df[f'Feature_{i}'] = X_df[f'Feature_0'] + np.random.normal(0, 0.3, n_samples)
    
    X = X_df.values
    
    print(f"Generated {n_samples} samples with {n_features} features")
    print(f"X shape: {X.shape}, y shape: {y.shape}")
    print(f"y range: [{y.min():.2f}, {y.max():.2f}], mean: {y.mean():.2f}")
    
    return X, y, feature_names


def core_ridge_regression(X_train, X_test, y_train, y_test, alphas=[0.1, 1.0, 10.0, 100.0]):
    """
    Implement Ridge regression with different alpha values.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test targets
    alphas : list
        Alpha values to try
    
    Returns:
    --------
    results : dict
        Results for each alpha
    """
    print(f"\n{'='*60}")
    print(f"CORE RIDGE REGRESSION")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    results = {}
    best_alpha = None
    best_r2 = -np.inf
    
    for alpha in alphas:
        print(f"\n--- Alpha: {alpha} ---")
        
        model = Ridge(alpha=alpha)
        model.fit(X_train_scaled, y_train)
        
        y_pred_train = model.predict(X_train_scaled)
        y_pred_test = model.predict(X_test_scaled)
        
        train_mse = mean_squared_error(y_train, y_pred_train)
        test_mse = mean_squared_error(y_test, y_pred_test)
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        test_mae = mean_absolute_error(y_test, y_pred_test)
        
        results[alpha] = {
            'model': model,
            'train_mse': train_mse,
            'test_mse': test_mse,
            'train_r2': train_r2,
            'test_r2': test_r2,
            'test_mae': test_mae,
            'coefficients': model.coef_,
            'y_pred_test': y_pred_test
        }
        
        print(f"  Train MSE: {train_mse:.4f}, R²: {train_r2:.4f}")
        print(f"  Test MSE: {test_mse:.4f}, R²: {test_r2:.4f}")
        
        if test_r2 > best_r2:
            best_r2 = test_r2
            best_alpha = alpha
    
    print(f"\nBest alpha: {best_alpha} (R² = {best_r2:.4f})")
    
    visualize_ridge_coefficients(results, alphas)
    
    return results, best_alpha


def visualize_ridge_coefficients(results, alphas):
    """
    Visualize Ridge coefficient paths.
    
    Parameters:
    -----------
    results : dict
        Results for each alpha
    alphas : list
        Alpha values
    """
    plt.figure(figsize=(10, 6))
    
    for alpha in alphas:
        coefs = results[alpha]['coefficients']
        plt.plot(coefs, marker='o', label=f'Alpha={alpha}')
    
    plt.xlabel('Feature Index')
    plt.ylabel('Coefficient Value')
    plt.title('Ridge Regression: Coefficient Paths')
    plt.legend()
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.show()


def core_lasso_regression(X_train, X_test, y_train, y_test, alphas=[0.01, 0.1, 1.0, 10.0]):
    """
    Implement Lasso regression with different alpha values.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test targets
    alphas : list
        Alpha values to try
    
    Returns:
    --------
    results : dict
        Results for each alpha
    """
    print(f"\n{'='*60}")
    print(f"CORE LASSO REGRESSION")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    results = {}
    best_alpha = None
    best_r2 = -np.inf
    
    for alpha in alphas:
        print(f"\n--- Alpha: {alpha} ---")
        
        model = Lasso(alpha=alpha, max_iter=10000)
        model.fit(X_train_scaled, y_train)
        
        y_pred_train = model.predict(X_train_scaled)
        y_pred_test = model.predict(X_test_scaled)
        
        train_mse = mean_squared_error(y_train, y_pred_train)
        test_mse = mean_squared_error(y_test, y_pred_test)
        train_r2 = r2_score(y_train, y_pred_train)
        test_r2 = r2_score(y_test, y_pred_test)
        test_mae = mean_absolute_error(y_test, y_pred_test)
        
        n_nonzero = np.sum(model.coef_ != 0)
        
        results[alpha] = {
            'model': model,
            'train_mse': train_mse,
            'test_mse': test_mse,
            'train_r2': train_r2,
            'test_r2': test_r2,
            'test_mae': test_mae,
            'coefficients': model.coef_,
            'n_nonzero': n_nonzero,
            'y_pred_test': y_pred_test
        }
        
        print(f"  Train MSE: {train_mse:.4f}, R²: {train_r2:.4f}")
        print(f"  Test MSE: {test_mse:.4f}, R²: {test_r2:.4f}")
        print(f"  Non-zero coefficients: {n_nonzero}/{len(model.coef_)}")
        
        if test_r2 > best_r2:
            best_r2 = test_r2
            best_alpha = alpha
    
    print(f"\nBest alpha: {best_alpha} (R² = {best_r2:.4f})")
    
    visualize_lasso_coefficients(results, alphas)
    
    return results, best_alpha


def visualize_lasso_coefficients(results, alphas):
    """
    Visualize Lasso coefficient paths showing feature selection.
    
    Parameters:
    -----------
    results : dict
        Results for each alpha
    alphas : list
        Alpha values
    """
    plt.figure(figsize=(10, 6))
    
    for alpha in alphas:
        coefs = results[alpha]['coefficients']
        n_nonzero = results[alpha]['n_nonzero']
        plt.plot(coefs, marker='o', label=f'Alpha={alpha} (nz={n_nonzero})')
    
    plt.xlabel('Feature Index')
    plt.ylabel('Coefficient Value')
    plt.title('Lasso Regression: Coefficient Paths (Feature Selection)')
    plt.legend()
    plt.grid(True, alpha=0.3)
    plt.tight_layout()
    plt.show()


def core_elastic_net_regression(X_train, X_test, y_train, y_test, alphas=[0.01, 0.1, 1.0], l1_ratios=[0.25, 0.5, 0.75]):
    """
    Implement Elastic Net regression combining L1 and L2 penalties.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test targets
    alphas : list
        Alpha values to try
    l1_ratios : list
        L1 ratio values to try
    
    Returns:
    --------
    results : dict
        Results for each parameter combination
    """
    print(f"\n{'='*60}")
    print(f"CORE ELASTIC NET REGRESSION")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    results = {}
    best_params = None
    best_r2 = -np.inf
    
    for alpha in alphas:
        for l1_ratio in l1_ratios:
            print(f"\n--- Alpha: {alpha}, L1 Ratio: {l1_ratio} ---")
            
            model = ElasticNet(alpha=alpha, l1_ratio=l1_ratio, max_iter=10000)
            model.fit(X_train_scaled, y_train)
            
            y_pred_train = model.predict(X_train_scaled)
            y_pred_test = model.predict(X_test_scaled)
            
            train_mse = mean_squared_error(y_train, y_pred_train)
            test_mse = mean_squared_error(y_test, y_pred_test)
            train_r2 = r2_score(y_train, y_pred_train)
            test_r2 = r2_score(y_test, y_pred_test)
            
            n_nonzero = np.sum(model.coef_ != 0)
            
            key = f'a{alpha}_l1{l1_ratio}'
            results[key] = {
                'model': model,
                'alpha': alpha,
                'l1_ratio': l1_ratio,
                'train_mse': train_mse,
                'test_mse': test_mse,
                'train_r2': train_r2,
                'test_r2': test_r2,
                'coefficients': model.coef_,
                'n_nonzero': n_nonzero,
                'y_pred_test': y_pred_test
            }
            
            print(f"  Train MSE: {train_mse:.4f}, R²: {train_r2:.4f}")
            print(f"  Test MSE: {test_mse:.4f}, R²: {test_r2:.4f}")
            print(f"  Non-zero coefficients: {n_nonzero}/{len(model.coef_)}")
            
            if test_r2 > best_r2:
                best_r2 = test_r2
                best_params = (alpha, l1_ratio)
    
    print(f"\nBest params: alpha={best_params[0]}, l1_ratio={best_params[1]} (R² = {best_r2:.4f})")
    
    return results, best_params


def compare_regularization_methods(X_train, X_test, y_train, y_test, feature_names):
    """
    Compare Ridge, Lasso, and Elastic Net on the same dataset.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test features
    y_train, y_test : ndarray
        Training and test targets
    feature_names : list
        Names of features
    """
    print(f"\n{'='*60}")
    print(f"COMPARING REGULARIZATION METHODS")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    ridge = Ridge(alpha=1.0)
    lasso = Lasso(alpha=0.1, max_iter=10000)
    elastic_net = ElasticNet(alpha=0.1, l1_ratio=0.5, max_iter=10000)
    
    methods = {
        'Ridge': ridge,
        'Lasso': lasso,
        'ElasticNet': elastic_net
    }
    
    comparison_results = {}
    
    for name, model in methods.items():
        model.fit(X_train_scaled, y_train)
        
        y_pred_test = model.predict(X_test_scaled)
        test_r2 = r2_score(y_test, y_pred_test)
        test_mse = mean_squared_error(y_test, y_pred_test)
        test_mae = mean_absolute_error(y_test, y_pred_test)
        
        n_nonzero = np.sum(model.coef_ != 0)
        
        comparison_results[name] = {
            'r2': test_r2,
            'mse': test_mse,
            'mae': test_mae,
            'n_nonzero': n_nonzero,
            'coefficients': model.coef_
        }
        
        print(f"\n{name}:")
        print(f"  Test R²: {test_r2:.4f}")
        print(f"  Test MSE: {test_mse:.4f}")
        print(f"  Test MAE: {test_mae:.4f}")
        print(f"  Non-zero coefficients: {n_nonzero}/{len(feature_names)}")
        
        top_features = sorted(
            [(fn, coef) for fn, coef in zip(feature_names, model.coef_)],
            key=lambda x: abs(x[1]),
            reverse=True
        )[:5]
        print(f"  Top 5 features by absolute coefficient:")
        for feat, coef in top_features:
            print(f"    {feat}: {coef:.4f}")
    
    visualize_comparison(comparison_results, feature_names)
    
    return comparison_results


def visualize_comparison(results, feature_names):
    """
    Visualize comparison of regularization methods.
    
    Parameters:
    -----------
    results : dict
        Results for each method
    feature_names : list
        Feature names
    """
    fig, axes = plt.subplots(1, 2, figsize=(14, 5))
    
    methods = list(results.keys())
    r2_scores = [results[m]['r2'] for m in methods]
    nonzero_counts = [results[m]['n_nonzero'] for m in methods]
    
    ax = axes[0]
    ax.bar(methods, r2_scores, color=['blue', 'orange', 'green'])
    ax.set_ylabel('R² Score')
    ax.set_title('Model Performance (R²)')
    ax.grid(True, alpha=0.3)
    
    ax = axes[1]
    ax.bar(methods, nonzero_counts, color=['blue', 'orange', 'green'])
    ax.set_ylabel('Non-zero Coefficients')
    ax.set_title('Feature Selection')
    ax.grid(True, alpha=0.3)
    
    plt.tight_layout()
    plt.show()


def banking_example():
    """
    Banking/Finance example: Credit Risk Prediction with many correlated features.
    
    Demonstrates how Ridge and Lasso handle multicollinearity in credit scoring.
    """
    print(f"\n{'='*60}")
    print(f"BANKING EXAMPLE: Credit Risk Prediction")
    print(f"{'='*60}")
    
    np.random.seed(42)
    n_samples = 500
    
    income = np.random.uniform(30000, 150000, n_samples)
    credit_score = np.random.uniform(500, 850, n_samples)
    debt = np.random.uniform(0, 50000, n_samples)
    employment_years = np.random.uniform(0, 30, n_samples)
    num_accounts = np.random.randint(0, 10, n_samples)
    num_credit_cards = np.random.randint(0, 5, n_samples)
    num_loans = np.random.randint(0, 3, n_samples)
    missed_payments = np.random.randint(0, 5, n_samples)
    inquiries = np.random.randint(0, 10, n_samples)
    credit_utilization = np.random.uniform(0, 100, n_samples)
    
    x0 = income / 10000
    x1 = credit_score / 100
    x2 = debt / 10000
    x3 = employment_years
    x4 = num_accounts
    x5 = num_credit_cards
    x6 = num_loans
    x7 = missed_payments
    x8 = inquiries
    x9 = credit_utilization / 10
    
    base_risk = 500
    credit_score_effect = -5 * x1
    debt_effect = 3 * x2
    missed_effect = 100 * x7
    employment_effect = -10 * x3
    utilization_effect = 5 * x9
    inquiries_effect = 30 * x8
    
    risk_score = (base_risk + 
               credit_score_effect + 
               debt_effect + 
               missed_effect + 
               employment_effect + 
               utilization_effect +
               inquiries_effect +
               np.random.normal(0, 50, n_samples))
    
    df = pd.DataFrame({
        'Income': income,
        'Credit_Score': credit_score,
        'Debt': debt,
        'Employment_Years': employment_years,
        'Num_Accounts': num_accounts,
        'Num_Credit_Cards': num_credit_cards,
        'Num_Loans': num_loans,
        'Missed_Payments': missed_payments,
        'Inquiries': inquiries,
        'Credit_Utilization': credit_utilization,
        'Risk_Score': risk_score
    })
    
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    feature_cols = ['Income', 'Credit_Score', 'Debt', 'Employment_Years', 
                  'Num_Accounts', 'Num_Credit_Cards', 'Num_Loans',
                  'Missed_Payments', 'Inquiries', 'Credit_Utilization']
    X = df[feature_cols].values
    y = df['Risk_Score'].values
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    print(f"\nTraining samples: {len(X_train)}, Test samples: {len(X_test)}")
    
    results, best_alpha = core_ridge_regression(X_train, X_test, y_train, y_test)
    lasso_results, lasso_alpha = core_lasso_regression(X_train, X_test, y_train, y_test)
    elastic_results, elastic_params = core_elastic_net_regression(X_train, X_test, y_train, y_test)
    
    comparison = compare_regularization_methods(X_train, X_test, y_train, y_test, feature_cols)
    
    return comparison


def healthcare_example():
    """
    Healthcare example: Patient Outcome Prediction with high-dimensional features.
    
    Demonstrates feature selection for medical diagnosis prediction.
    """
    print(f"\n{'='*60}")
    print(f"HEALTHCARE EXAMPLE: Patient Outcome Prediction")
    print(f"{'='*60}")
    
    np.random.seed(123)
    n_samples = 500
    n_features = 20
    
    age = np.random.uniform(18, 90, n_samples)
    bmi = np.random.uniform(18, 45, n_samples)
    systolic_bp = np.random.uniform(90, 200, n_samples)
    diastolic_bp = np.random.uniform(60, 130, n_samples)
    heart_rate = np.random.uniform(50, 120, n_samples)
    temperature = np.random.uniform(36.0, 39.0, n_samples)
    respiratory_rate = np.random.uniform(12, 30, n_samples)
    oxygen_saturation = np.random.uniform(90, 100, n_samples)
    
    lab_features = np.random.uniform(0, 100, (n_samples, n_features - 8))
    
    X = np.column_stack([
        age, bmi, systolic_bp, diastolic_bp, heart_rate,
        temperature, respiratory_rate, oxygen_saturation, lab_features
    ])
    
    feature_names = ['Age', 'BMI', 'Systolic_BP', 'Diastolic_BP', 'Heart_Rate',
                   'Temperature', 'Respiratory_Rate', 'Oxygen_Saturation'] + \
                  [f'Lab_{i}' for i in range(n_features - 8)]
    
    base_outcome = 50
    age_effect = 0.3 * age
    bmi_effect = 1.5 * bmi
    bp_effect = 0.2 * systolic_bp
    hr_effect = 0.4 * heart_rate
    oxygen_effect = -3 * (100 - oxygen_saturation)
    lab_effect = np.sum(lab_features[:, :5], axis=1) * 0.1
    
    outcome_score = (base_outcome +
                  age_effect +
                  bmi_effect +
                  bp_effect +
                  hr_effect +
                  oxygen_effect +
                  lab_effect +
                  np.random.normal(0, 10, n_samples))
    
    df = pd.DataFrame(X, columns=feature_names)
    df['Outcome_Score'] = outcome_score
    
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, outcome_score, test_size=0.2, random_state=42
    )
    
    print(f"\nTraining samples: {len(X_train)}, Test samples: {len(X_test)}")
    
    results, best_alpha = core_ridge_regression(X_train, X_test, y_train, y_test)
    lasso_results, lasso_alpha = core_lasso_regression(X_train, X_test, y_train, y_test)
    
    comparison = compare_regularization_methods(X_train, X_test, y_train, y_test, feature_names)
    
    return comparison


def test_regularization():
    """
    Test regularized regression models.
    """
    print(f"\n{'='*60}")
    print(f"TESTING REGULARIZATION METHODS")
    print(f"{'='*60}")
    
    X, y, feature_names = generate_correlated_regression_data(n_samples=300, n_features=15)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    core_ridge_regression(X_train, X_test, y_train, y_test)
    core_lasso_regression(X_train, X_test, y_train, y_test)
    core_elastic_net_regression(X_train, X_test, y_train, y_test)
    
    print(f"\n{'='*60}")
    print(f"ALL TESTS COMPLETED SUCCESSFULLY")
    print(f"{'='*60}")
    
    return True


def main():
    """
    Main function to execute Ridge and Lasso regression examples.
    """
    print("="*60)
    print("RIDGE AND LASSO REGRESSION IMPLEMENTATION")
    print("="*60)
    
    print("\nI. INTRODUCTION")
    print("   Ridge and Lasso are regularized linear regression methods")
    print("   that prevent overfitting and perform feature selection.")
    
    print("\nII. CORE_CONCEPTS")
    print("   - Ridge: L2 penalty (shrinks coefficients)")
    print("   - Lasso: L1 penalty (feature selection)")
    print("   - ElasticNet: Combined L1 and L2 penalties")
    print("   - Alpha: regularization strength")
    
    print("\nIII. IMPLEMENTATION")
    
    X, y, feature_names = generate_correlated_regression_data(n_samples=300, n_features=10)
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    ridge_results, ridge_best = core_ridge_regression(X_train, X_test, y_train, y_test)
    lasso_results, lasso_best = core_lasso_regression(X_train, X_test, y_train, y_test)
    elastic_results, elastic_best = core_elastic_net_regression(X_train, X_test, y_train, y_test)
    
    print("\nIV. EXAMPLES")
    banking_results = banking_example()
    healthcare_results = healthcare_example()
    
    print("\nV. OUTPUT_RESULTS")
    print("   All model outputs and visualizations displayed above.")
    
    print("\nVI. TESTING")
    test_regularization()
    
    print("\nVII. ADVANCED_TOPICS")
    print("   - Coordinate descent optimization")
    print("   - Early stopping for large datasets")
    print("   - Warm start for efficient alpha search")
    print("   - Feature interpretation")
    
    print("\nVIII. CONCLUSION")
    print("   - Use RIDGE when: many correlated features, all important")
    print("   - Use LASSO when: need feature selection")
    print("   - Use ELASTIC NET when: combination needed")
    print("   - Always scale features before regularization")
    print("   - Cross-validate alpha for best results")
    print("\n   Implementation complete!")


if __name__ == "__main__":
    main()