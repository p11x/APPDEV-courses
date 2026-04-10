# Topic: Decision Tree Regression
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Decision Tree Regression

I. INTRODUCTION
    Decision Tree Regression is a supervised learning algorithm that predicts continuous
    values by splitting the feature space into rectangular regions. Unlike linear
    regression, decision trees can capture non-linear relationships and complex patterns
    in data without requiring feature engineering.

    Key characteristics:
    - Tree-based method that creates a hierarchical structure
    - Makes predictions by traversing from root to leaf
    - Each leaf node contains a constant prediction value
    - Uses recursive binary splitting to partition the data
    - Can capture non-linear relationships naturally

II. CORE_CONCEPTS
    1. Splitting Criteria (MSE - Mean Squared Error):
       - For regression, we use variance reduction or MSE as the splitting criterion
       - MSE measures the average squared difference between actual and predicted values
       - The algorithm finds the split that minimizes the weighted average MSE of child nodes
    
    2. Tree Depth Control:
       - max_depth: Maximum depth of the tree
       - Controls the complexity of the model
       - Deeper trees can capture more complex patterns but risk overfitting
       - Shallower trees are more generalizable but may underfit
    
    3. Pruning Strategies:
       - Pre-pruning: Limit depth, minimum samples split, minimum samples leaf
       - Post-pruning: Cost complexity pruning (CCP)
       - Helps prevent overfitting
    
    4. Feature Importance:
       - Measures how much each feature contributes to reducing MSE
       - Normalized to sum to 1
       - Useful for feature selection and understanding

III. IMPLEMENTATION
    This module covers practical implementation including:
    - Basic decision tree regression
    - Hyperparameter tuning
    - Feature importance analysis
    - Real-world examples in banking and healthcare

IV. EXAMPLES (Banking + Healthcare)
    Banking: Loan amount prediction based on customer attributes
    Healthcare: Hospital stay duration prediction

V. OUTPUT_RESULTS
    Detailed performance metrics and visualizations

VI. TESTING
    Comprehensive test cases

VII. ADVANCED_TOPICS
    - Ensemble methods with decision trees
    - Cost complexity pruning
    - Cross-validation for hyperparameter selection

VIII. CONCLUSION
    Summary and best practices
"""

# Import necessary libraries
import numpy as np
import pandas as pd
from sklearn.datasets import make_regression, make_blobs
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.tree import DecisionTreeRegressor, plot_tree, export_text
from sklearn.metrics import mean_squared_error, r2_score, mean_absolute_error
import warnings
warnings.filterwarnings('ignore')

# Optional visualization libraries
try:
    import matplotlib.pyplot as plt
    import seaborn as sns
    HAS_VISUALIZATION = True
except ImportError:
    HAS_VISUALIZATION = False
    print("Note: matplotlib/seaborn not available. Visualizations skipped.")


def generate_synthetic_regression_data(n_samples=500, n_features=5, noise=10, seed=42):
    """
    Generate synthetic regression data for demonstration.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features
    noise : float
        Standard deviation of Gaussian noise
    seed : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : ndarray
        Feature matrix
    y : ndarray
        Target vector
    feature_names : list
        Names of features
    """
    np.random.seed(seed)
    
    # Generate regression data with specified noise
    X, y = make_regression(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_features - 1,
        noise=noise,
        random_state=seed,
        effective_rank=n_features
    )
    
    # Create meaningful feature names
    feature_names = [f'Feature_{i+1}' for i in range(n_features)]
    
    # Add some non-linearity
    y = y + np.sin(X[:, 0] * 2) * 50 + np.cos(X[:, 1] * 3) * 30
    
    return X, y, feature_names


def generate_banking_data(n_samples=1000, seed=42):
    """
    Generate synthetic banking data for loan amount prediction.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    seed : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : DataFrame
        Banking dataset with features and target
    """
    np.random.seed(seed)
    
    # Generate customer attributes
    credit_score = np.random.randint(300, 850, n_samples)
    annual_income = np.random.normal(60000, 25000, n_samples)
    annual_income = np.maximum(annual_income, 15000)
    employment_years = np.random.exponential(5, n_samples)
    debt_amount = np.random.exponential(10000, n_samples)
    existing_loans = np.random.randint(0, 4, n_samples)
    age = np.random.normal(35, 10, n_samples)
    age = np.clip(age, 18, 70)
    num_creditCards = np.random.randint(0, 6, n_samples)
    payment_history = np.random.uniform(0.7, 1.0, n_samples)
    
    # Calculate loan amount (target) with realistic relationships
    base_loan = 10000 + (credit_score - 300) * 50
    income_factor = annual_income * 0.3
    employment_factor = employment_years * 500
    debt_penalty = debt_amount * 0.2
    loan_penalty = existing_loans * 2000
    age_factor = (age - 25) * 100
    creditCard_factor = num_creditCards * 500
    payment_factor = payment_history * 20000
    
    # Add noise
    noise = np.random.normal(0, 3000, n_samples)
    
    # Calculate final loan amount
    loan_amount = (
        base_loan * 0.2 +
        income_factor +
        employment_factor +
        age_factor +
        payment_factor -
        debt_penalty -
        loan_penalty -
        creditCard_factor +
        noise
    )
    loan_amount = np.maximum(loan_amount, 1000)
    loan_amount = np.clip(loan_amount, 1000, 500000)
    
    # Create DataFrame
    df = pd.DataFrame({
        'Credit_Score': credit_score,
        'Annual_Income': annual_income,
        'Employment_Years': employment_years,
        'Debt_Amount': debt_amount,
        'Existing_Loans': existing_loans,
        'Age': age,
        'Num_CreditCards': num_creditCards,
        'Payment_History': payment_history,
        'Loan_Amount': loan_amount
    })
    
    return df


def generate_healthcare_data(n_samples=1000, seed=42):
    """
    Generate synthetic healthcare data for hospital stay duration prediction.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    seed : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : DataFrame
        Healthcare dataset with features and target
    """
    np.random.seed(seed)
    
    # Generate patient attributes
    age = np.random.normal(45, 15, n_samples)
    age = np.clip(age, 1, 95)
    bmi = np.random.normal(26, 5, n_samples)
    bmi = np.clip(bmi, 15, 45)
    num_previous_admissions = np.random.poisson(2, n_samples)
    severity_score = np.random.uniform(1, 10, n_samples)
    num_comorbidities = np.random.poisson(1, n_samples)
    emergency_case = np.random.binomial(1, 0.3, n_samples)
    surgery_performed = np.random.binomial(1, 0.4, n_samples)
    insurance_type = np.random.choice([0, 1, 2], n_samples, p=[0.4, 0.4, 0.2])
    days_before_admission = np.random.exponential(3, n_samples)
    
    # Calculate hospital stay duration (target) with realistic relationships
    base_stay = 2
    age_factor = (age / 10) * 0.5
    bmi_factor = (bmi - 20) * 0.3
    admission_factor = num_previous_admissions * 0.8
    severity_factor = severity_score * 1.5
    comorbidity_factor = num_comorbidities * 2
    emergency_factor = emergency_case * 1.5
    surgery_factor = surgery_performed * 3
    insurance_factor = insurance_type * 0.3
    days_factor = days_before_admission * 0.5
    
    # Add randomness
    noise = np.random.normal(0, 1, n_samples)
    
    # Calculate final stay duration
    stay_duration = (
        base_stay +
        age_factor +
        bmi_factor +
        admission_factor +
        severity_factor +
        comorbidity_factor +
        emergency_factor +
        surgery_factor +
        insurance_factor +
        days_factor +
        noise
    )
    stay_duration = np.maximum(stay_duration, 1)
    
    # Create DataFrame
    df = pd.DataFrame({
        'Age': age,
        'BMI': bmi,
        'Num_Previous_Admissions': num_previous_admissions,
        'Severity_Score': severity_score,
        'Num_Comorbidities': num_comorbidities,
        'Emergency_Case': emergency_case,
        'Surgery_Performed': surgery_performed,
        'Insurance_Type': insurance_type,
        'Days_Before_Admission': days_before_admission,
        'Stay_Duration': stay_duration
    })
    
    return df


def core_decision_tree_regression(X_train, X_test, y_train, y_test, max_depth=5, 
                                  min_samples_split=2, min_samples_leaf=1,
                                  random_state=42):
    """
    Core implementation of Decision Tree Regression.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test feature matrices
    y_train, y_test : ndarray
        Training and test target vectors
    max_depth : int
        Maximum depth of the tree
    min_samples_split : int
        Minimum samples required to split a node
    min_samples_leaf : int
        Minimum samples required in leaf node
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    model : DecisionTreeRegressor
        Trained model
    predictions : ndarray
        Predictions on test set
    metrics : dict
        Performance metrics
    """
    # Create and train decision tree regressor
    model = DecisionTreeRegressor(
        max_depth=max_depth,
        min_samples_split=min_samples_split,
        min_samples_leaf=min_samples_leaf,
        random_state=random_state,
        criterion='squared_error'
    )
    
    model.fit(X_train, y_train)
    
    # Make predictions
    predictions = model.predict(X_test)
    
    # Calculate metrics
    mse = mean_squared_error(y_test, predictions)
    rmse = np.sqrt(mse)
    mae = mean_absolute_error(y_test, predictions)
    r2 = r2_score(y_test, predictions)
    
    metrics = {
        'MSE': mse,
        'RMSE': rmse,
        'MAE': mae,
        'R2': r2
    }
    
    return model, predictions, metrics


def analyze_tree_depth(X_train, X_test, y_train, y_test, max_depths=None, 
                      random_state=42):
    """
    Analyze the effect of tree depth on model performance.
    
    Parameters:
    -----------
    X_train, X_test : ndarray
        Training and test feature matrices
    y_train, y_test : ndarray
        Training and test target vectors
    max_depths : list
        List of max depths to evaluate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : DataFrame
        Results containing depth and metrics
    """
    if max_depths is None:
        max_depths = [1, 2, 3, 4, 5, 7, 10, 15, 20, None]
    
    results = []
    
    for depth in max_depths:
        model = DecisionTreeRegressor(
            max_depth=depth,
            random_state=random_state,
            criterion='squared_error'
        )
        
        model.fit(X_train, y_train)
        
        train_pred = model.predict(X_train)
        test_pred = model.predict(X_test)
        
        results.append({
            'max_depth': depth if depth is not None else 'None',
            'train_rmse': np.sqrt(mean_squared_error(y_train, train_pred)),
            'test_rmse': np.sqrt(mean_squared_error(y_test, test_pred)),
            'train_r2': r2_score(y_train, train_pred),
            'test_r2': r2_score(y_test, test_pred),
            'n_leaves': model.get_n_leaves(),
            'tree_depth': model.get_depth()
        })
    
    results_df = pd.DataFrame(results)
    return results_df


def analyze_feature_importance(model, feature_names):
    """
    Analyze and display feature importance.
    
    Parameters:
    -----------
    model : DecisionTreeRegressor
        Trained model
    feature_names : list
        Names of features
        
    Returns:
    --------
    importance_df : DataFrame
        Feature importance scores
    """
    # Get feature importances
    importances = model.feature_importances_
    
    # Create DataFrame
    importance_df = pd.DataFrame({
        'Feature': feature_names,
        'Importance': importances
    }).sort_values('Importance', ascending=False)
    
    # Normalize to percentage
    importance_df['Importance_Percent'] = importance_df['Importance'] * 100
    
    return importance_df


def print_tree_rules(model, feature_names):
    """
    Print the decision tree rules in text format.
    
    Parameters:
    -----------
    model : DecisionTreeRegressor
        Trained model
    feature_names : list
        Names of features
        
    Returns:
    --------
    rules : str
        Text representation of tree rules
    """
    rules = export_text(model, feature_names=feature_names)
    return rules


def perform_cross_validation(X, y, max_depth=5, cv=5, random_state=42):
    """
    Perform cross-validation for decision tree regression.
    
    Parameters:
    -----------
    X : ndarray
        Feature matrix
    y : ndarray
        target vector
    max_depth : int
        Maximum depth of the tree
    cv : int
        Number of folds
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    cv_results : dict
        Cross-validation results
    """
    model = DecisionTreeRegressor(
        max_depth=max_depth,
        random_state=random_state,
        criterion='squared_error'
    )
    
    # Perform cross-validation
    neg_mse_scores = cross_val_score(model, X, y, cv=cv, scoring='neg_mean_squared_error')
    r2_scores = cross_val_score(model, X, y, cv=cv, scoring='r2')
    
    cv_results = {
        'neg_mse_mean': -neg_mse_scores.mean(),
        'neg_mse_std': neg_mse_scores.std(),
        'r2_mean': r2_scores.mean(),
        'r2_std': r2_scores.std(),
        'rmse_mean': np.sqrt(-neg_mse_scores.mean()),
        'rmse_std': np.sqrt(neg_mse_scores).std()
    }
    
    return cv_results


def grid_search_hyperparameters(X_train, y_train, param_grid=None, cv=5):
    """
    Perform grid search for hyperparameter tuning.
    
    Parameters:
    -----------
    X_train : ndarray
        Training feature matrix
    y_train : ndarray
        Training target vector
    param_grid : dict
        Parameter grid for search
    cv : int
        Number of folds
        
    Returns:
    --------
    best_params : dict
        Best hyperparameters found
    grid_results : DataFrame
        All grid search results
    """
    if param_grid is None:
        param_grid = {
            'max_depth': [3, 5, 7, 10],
            'min_samples_split': [2, 5, 10],
            'min_samples_leaf': [1, 2, 4]
        }
    
    model = DecisionTreeRegressor(
        criterion='squared_error',
        random_state=42
    )
    
    grid_search = GridSearchCV(
        model,
        param_grid,
        cv=cv,
        scoring='neg_mean_squared_error',
        return_train_score=True
    )
    
    grid_search.fit(X_train, y_train)
    
    best_params = grid_search.best_params_
    
    results_df = pd.DataFrame(grid_search.cv_results_)
    
    return best_params, results_df


def cost_complexity_pruning(X_train, y_train, X_val, y_val):
    """
    Perform cost complexity pruning to find optimal alpha.
    
    Parameters:
    -----------
    X_train : ndarray
        Training feature matrix
    y_train : ndarray
        Training target vector
    X_val : ndarray
        Validation feature matrix
    y_val : ndarray
        Validation target vector
        
    Returns:
    --------
    alphas : list
        Alpha values tested
    impurities : list
        Impurities (total leaf impurities)
    best_alpha : float
        Best alpha value
    """
    # First, train a full tree to get path
    full_tree = DecisionTreeRegressor(random_state=42)
    full_tree.fit(X_train, y_train)
    
    # Get ccp_alphas and impurities
    path = full_tree.cost_complexity_pruning_path(X_train, y_train)
    ccp_alphas = path.ccp_alphas
    impurities = path.impurities
    
    # Train trees with different alphas and find best
    best_alpha = 0
    best_rmse = float('inf')
    
    for alpha in ccp_alphas:
        pruned_tree = DecisionTreeRegressor(
            ccp_alpha=alpha,
            random_state=42
        )
        pruned_tree.fit(X_train, y_train)
        
        val_pred = pruned_tree.predict(X_val)
        rmse = np.sqrt(mean_squared_error(y_val, val_pred))
        
        if rmse < best_rmse:
            best_rmse = rmse
            best_alpha = alpha
    
    return ccp_alphas, impurities, best_alpha


def run_banking_example():
    """
    Run banking example: Loan Amount Prediction.
    
    This example demonstrates using Decision Tree Regression
    to predict loan amounts based on customer attributes.
    """
    print("\n" + "="*60)
    print("BANKING EXAMPLE: Loan Amount Prediction")
    print("="*60)
    
    # Generate banking data
    df = generate_banking_data(n_samples=1000, seed=42)
    
    print(f"\nDataset Shape: {df.shape}")
    print(f"\nDataset Columns: {list(df.columns)}")
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    # Prepare features and target
    feature_columns = [
        'Credit_Score', 'Annual_Income', 'Employment_Years',
        'Debt_Amount', 'Existing_Loans', 'Age',
        'Num_CreditCards', 'Payment_History'
    ]
    
    X = df[feature_columns].values
    y = df['Loan_Amount'].values
    feature_names = feature_columns
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    print(f"\nTraining samples: {len(X_train)}")
    print(f"Test samples: {len(X_test)}")
    
    # Analyze different tree depths
    print("\n--- Depth Analysis ---")
    depth_results = analyze_tree_depth(X_train, X_test, y_train, y_test)
    print(depth_results.to_string(index=False))
    
    # Train model with optimal depth (find best from results)
    best_idx = depth_results['test_rmse'].idxmin()
    optimal_depth = depth_results.loc[best_idx, 'max_depth']
    
    print(f"\nOptimal max_depth: {optimal_depth}")
    
    # Train final model
    model, predictions, metrics = core_decision_tree_regression(
        X_train, X_test, y_train, y_test,
        max_depth=optimal_depth if isinstance(optimal_depth, int) else 5,
        random_state=42
    )
    
    print("\n--- Model Performance ---")
    print(f"MSE: {metrics['MSE']:.2f}")
    print(f"RMSE: {metrics['RMSE']:.2f}")
    print(f"MAE: {metrics['MAE']:.2f}")
    print(f"R2: {metrics['R2']:.4f}")
    
    # Feature importance
    print("\n--- Feature Importance ---")
    importance_df = analyze_feature_importance(model, feature_names)
    print(importance_df.to_string(index=False))
    
    # Cross-validation
    print("\n--- Cross-Validation ---")
    cv_results = perform_cross_validation(
        X, y, max_depth=5, cv=5, random_state=42
    )
    print(f"CV RMSE: {cv_results['rmse_mean']:.2f} (+/- {cv_results['rmse_std']:.2f})")
    print(f"CV R2: {cv_results['r2_mean']:.4f} (+/- {cv_results['r2_std']:.4f})")
    
    print("\n" + "-"*60)
    
    return model, metrics, importance_df


def run_healthcare_example():
    """
    Run healthcare example: Hospital Stay Duration Prediction.
    
    This example demonstrates using Decision Tree Regression
    to predict hospital stay duration based on patient attributes.
    """
    print("\n" + "="*60)
    print("HEALTHCARE EXAMPLE: Hospital Stay Duration Prediction")
    print("="*60)
    
    # Generate healthcare data
    df = generate_healthcare_data(n_samples=1000, seed=42)
    
    print(f"\nDataset Shape: {df.shape}")
    print(f"\nDataset Columns: {list(df.columns)}")
    print(f"\nDataset Statistics:")
    print(df.describe())
    
    # Prepare features and target
    feature_columns = [
        'Age', 'BMI', 'Num_Previous_Admissions', 'Severity_Score',
        'Num_Comorbidities', 'Emergency_Case', 'Surgery_Performed',
        'Insurance_Type', 'Days_Before_Admission'
    ]
    
    X = df[feature_columns].values
    y = df['Stay_Duration'].values
    feature_names = feature_columns
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    print(f"\nTraining samples: {len(X_train)}")
    print(f"Test samples: {len(X_test)}")
    
    # Analyze different tree depths
    print("\n--- Depth Analysis ---")
    depth_results = analyze_tree_depth(X_train, X_test, y_train, y_test)
    print(depth_results.to_string(index=False))
    
    # Find optimal depth
    best_idx = depth_results['test_rmse'].idxmin()
    optimal_depth = depth_results.loc[best_idx, 'max_depth']
    
    print(f"\nOptimal max_depth: {optimal_depth}")
    
    # Train final model
    model, predictions, metrics = core_decision_tree_regression(
        X_train, X_test, y_train, y_test,
        max_depth=optimal_depth if isinstance(optimal_depth, int) else 5,
        random_state=42
    )
    
    print("\n--- Model Performance ---")
    print(f"MSE: {metrics['MSE']:.2f}")
    print(f"RMSE: {metrics['RMSE']:.2f}")
    print(f"MAE: {metrics['MAE']:.2f}")
    print(f"R2: {metrics['R2']:.4f}")
    
    # Feature importance
    print("\n--- Feature Importance ---")
    importance_df = analyze_feature_importance(model, feature_names)
    print(importance_df.to_string(index=False))
    
    # Print tree rules
    print("\n--- Decision Tree Rules ---")
    rules = print_tree_rules(model, feature_names)
    print(rules[:500])  # Print first 500 chars
    
    # Cross-validation
    print("\n--- Cross-Validation ---")
    cv_results = perform_cross_validation(
        X, y, max_depth=5, cv=5, random_state=42
    )
    print(f"CV RMSE: {cv_results['rmse_mean']:.2f} (+/- {cv_results['rmse_std']:.2f})")
    print(f"CV R2: {cv_results['r2_mean']:.4f} (+/- {cv_results['r2_std']:.4f})")
    
    print("\n" + "-"*60)
    
    return model, metrics, importance_df


def demonstrate_visualizations(model, X_test, y_test, feature_names):
    """
    Demonstrate visualization capabilities of decision trees.
    
    Note: Requires matplotlib and seaborn to be installed.
    """
    if not HAS_VISUALIZATION:
        print("Visualization libraries not available. Skipping.")
        return
    
    # Create figure with subplots
    fig, axes = plt.subplots(2, 2, figsize=(16, 12))
    
    # 1. Actual vs Predicted
    ax1 = axes[0, 0]
    y_pred = model.predict(X_test)
    ax1.scatter(y_test, y_pred, alpha=0.5)
    ax1.plot([y_test.min(), y_test.max()], [y_test.min(), y_test.max()], 'r--', lw=2)
    ax1.set_xlabel('Actual Values')
    ax1.set_ylabel('Predicted Values')
    ax1.set_title('Actual vs Predicted')
    
    # 2. Residuals
    ax2 = axes[0, 1]
    residuals = y_test - y_pred
    ax2.scatter(y_pred, residuals, alpha=0.5)
    ax2.axhline(y=0, color='r', linestyle='--', lw=2)
    ax2.set_xlabel('Predicted Values')
    ax2.set_ylabel('Residuals')
    ax2.set_title('Residual Plot')
    
    # 3. Feature Importance Bar Chart
    ax3 = axes[1, 0]
    importance_df = analyze_feature_importance(model, feature_names)
    ax3.barh(importance_df['Feature'], importance_df['Importance_Percent'])
    ax3.set_xlabel('Importance (%)')
    ax3.set_ylabel('Feature')
    ax3.set_title('Feature Importance')
    
    # 4. Residual Distribution
    ax4 = axes[1, 1]
    ax4.hist(residuals, bins=30, edgecolor='black', alpha=0.7)
    ax4.axvline(x=0, color='r', linestyle='--', lw=2)
    ax4.set_xlabel('Residuals')
    ax4.set_ylabel('Frequency')
    ax4.set_title('Residual Distribution')
    
    plt.tight_layout()
    plt.savefig('decision_tree_regression_visualizations.png', dpi=150)
    plt.close()
    
    print("Visualizations saved to decision_tree_regression_visualizations.png")


def run_comprehensive_tests():
    """
    Run comprehensive test cases for decision tree regression.
    """
    print("\n" + "="*60)
    print("COMPREHENSIVE TESTS")
    print("="*60)
    
    # Test 1: Synthetic data
    print("\n--- Test 1: Synthetic Data ---")
    X, y, feature_names = generate_synthetic_regression_data(
        n_samples=500, n_features=5, noise=10, seed=42
    )
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    model, predictions, metrics = core_decision_tree_regression(
        X_train, X_test, y_train, y_test,
        max_depth=5, random_state=42
    )
    
    print(f"R2 Score: {metrics['R2']:.4f}")
    print(f"RMSE: {metrics['RMSE']:.4f}")
    assert metrics['R2'] > 0, "R2 should be positive"
    print("Test 1 PASSED")
    
    # Test 2: Different max_depths
    print("\n--- Test 2: Different Max Depths ---")
    depths_to_test = [1, 3, 5, 10, None]
    for depth in depths_to_test:
        model = DecisionTreeRegressor(
            max_depth=depth,
            random_state=42,
            criterion='squared_error'
        )
        model.fit(X_train, y_train)
        train_r2 = r2_score(y_train, model.predict(X_train))
        test_r2 = r2_score(y_test, model.predict(X_test))
        print(f"max_depth={depth}: Train R2={train_r2:.4f}, Test R2={test_r2:.4f}")
        
        if depth is None:
            assert train_r2 >= test_r2, "Train R2 should be >= Test R2"
    
    print("Test 2 PASSED")
    
    # Test 3: Feature importance
    print("\n--- Test 3: Feature Importance ---")
    model = DecisionTreeRegressor(max_depth=5, random_state=42)
    model.fit(X_train, y_train)
    
    importance = model.feature_importances_
    print(f"Feature importances sum: {sum(importance):.4f}")
    assert abs(sum(importance) - 1.0) < 0.01, "Importances should sum to 1"
    print("Test 3 PASSED")
    
    # Test 4: Grid search
    print("\n--- Test 4: Grid Search ---")
    param_grid = {
        'max_depth': [3, 5, 7],
        'min_samples_split': [2, 5],
        'min_samples_leaf': [1, 2]
    }
    
    best_params, results_df = grid_search_hyperparameters(X_train, y_train, param_grid)
    print(f"Best params: {best_params}")
    assert 'max_depth' in best_params, "Should find best max_depth"
    print("Test 4 PASSED")
    
    # Test 5: Cross-validation
    print("\n--- Test 5: Cross-Validation ---")
    cv_results = perform_cross_validation(X, y, max_depth=5, cv=5)
    print(f"CV R2 mean: {cv_results['r2_mean']:.4f}")
    print(f"CV RMSE mean: {cv_results['rmse_mean']:.4f}")
    assert cv_results['r2_mean'] is not None, "CV should return R2 mean"
    print("Test 5 PASSED")
    
    print("\n" + "-"*60)
    print("All tests completed successfully!")


def main():
    """
    Main function to demonstrate Decision Tree Regression implementation.
    """
    print("="*60)
    print("DECISION TREE REGRESSION - COMPREHENSIVE IMPLEMENTATION")
    print("="*60)
    
    # Generate and visualize synthetic data
    print("\n--- Synthetic Data Generation ---")
    X, y, feature_names = generate_synthetic_regression_data(
        n_samples=500,
        n_features=5,
        noise=10,
        seed=42
    )
    print(f"Generated {len(X)} samples with {X.shape[1]} features")
    print(f"Features: {feature_names}")
    print(f"Target range: [{y.min():.2f}, {y.max():.2f}]")
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    print(f"\nTraining set: {X_train.shape[0]} samples")
    print(f"Test set: {X_test.shape[0]} samples")
    
    # Basic Decision Tree Regression
    print("\n--- Basic Decision Tree Regression ---")
    model, predictions, metrics = core_decision_tree_regression(
        X_train, X_test, y_train, y_test,
        max_depth=5,
        min_samples_split=2,
        min_samples_leaf=1,
        random_state=42
    )
    
    print(f"Tree Depth: {model.get_depth()}")
    print(f"Number of Leaves: {model.get_n_leaves()}")
    print(f"\nPerformance Metrics:")
    print(f"  MSE: {metrics['MSE']:.4f}")
    print(f"  RMSE: {metrics['RMSE']:.4f}")
    print(f"  MAE: {metrics['MAE']:.4f}")
    print(f"  R²: {metrics['R2']:.4f}")
    
    # Feature Importance
    print("\n--- Feature Importance ---")
    importance_df = analyze_feature_importance(model, feature_names)
    print(importance_df.to_string(index=False))
    
    # Tree Rules
    print("\n--- Decision Tree Rules ---")
    rules = print_tree_rules(model, feature_names)
    print(rules[:500])
    
    # Run banking example
    banking_model, banking_metrics, banking_importance = run_banking_example()
    
    # Run healthcare example
    healthcare_model, healthcare_metrics, healthcare_importance = run_healthcare_example()
    
    # Run comprehensive tests
    run_comprehensive_tests()
    
    print("\n" + "="*60)
    print("IMPLEMENTATION COMPLETE")
    print("="*60)
    
    print("""
VIII. CONCLUSION

Decision Tree Regression is a powerful and interpretable algorithm for 
predicting continuous values. Key takeaways:

1. Advantages:
   - Interpretable: Easy to visualize and explain
   - Non-linear: Captures complex patterns without transformation
   - Robust: Handles outliers well
   - No feature scaling required

2. Disadvantages:
   - Prone to overfitting
   - Unstable: Small changes in data can change tree significantly
   - Greedy: May not find optimal global solution

3. Best Practices:
   - Use max_depth to control complexity
   - Apply pruning strategies
   - Use cross-validation for hyperparameter tuning
   - Consider ensemble methods for improved performance

4. Common Applications:
   - Financial: Loan amount, risk prediction
   - Healthcare: Stay duration, readmission prediction
   - Real Estate: Property value estimation
   - Manufacturing: Quality control, defect prediction

5. Extensions:
   - Random Forest: Ensemble of decision trees
   - Gradient Boosting: Sequential ensemble method
   - XGBoost/LightBoost: Advanced boosting implementations
""")


if __name__ == "__main__":
    main()