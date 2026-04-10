# Topic: Overfitting and Underfitting
# Author: AI Assistant
# Date: 06-04-2026

"""
================================================================================
    COMPREHENSIVE IMPLEMENTATION FOR OVERFITTING AND UNDERFITTING
================================================================================

I. INTRODUCTION
-------------
Overfitting and underfitting are fundamental concepts in machine learning that
determine how well a model generalizes to unseen data. Understanding these
concepts is crucial for building effective predictive models.

This module covers:
- Definition of overfitting and underfitting
- Bias-variance tradeoff
- Detection techniques
- Prevention strategies
- Real-world examples

II. CORE CONCEPTS
-----------------
1. OVERFITTING: Model learns noise in training data, performs poorly on new data
2. UNDERFITTING: Model is too simple, fails to capture patterns in data
3. GOOD FIT: Balances complexity and generalization
4. BIAS: Error from overly simplistic assumptions
5. VARIANCE: Error from model's sensitivity to training data fluctuations
6. REGULARIZATION: Technique to prevent overfitting

III. KEY INDICATORS
-------------------
- Training accuracy >> Validation accuracy: Overfitting
- Low training accuracy: Underfitting
- Learning curves analysis
- Cross-validation scores

================================================================================
"""

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.datasets import make_regression, make_classification, make_blobs
from sklearn.model_selection import train_test_split, cross_val_score, learning_curve
from sklearn.preprocessing import PolynomialFeatures, StandardScaler
from sklearn.linear_model import LinearRegression, Ridge, Lasso, LogisticRegression
from sklearn.tree import DecisionTreeRegressor, DecisionTreeClassifier
from sklearn.ensemble import RandomForestRegressor, RandomForestClassifier
from sklearn.pipeline import Pipeline
from sklearn.metrics import mean_squared_error, accuracy_score, r2_score
import warnings
warnings.filterwarnings('ignore')

np.random.seed(42)
pd.set_option('display.max_columns', None)
pd.set_option('display.width', None)


def generate_regression_data(n_samples=200, noise=10, random_state=42):
    """
    Generate synthetic regression data for demonstrating overfitting/underfitting.
    """
    print(f"\n{'='*60}")
    print("GENERATING REGRESSION DATA")
    print(f"{'='*60}")
    
    X = np.linspace(0, 10, n_samples).reshape(-1, 1)
    y = np.sin(X).ravel() + np.random.normal(0, noise, n_samples) * 0.1
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=random_state
    )
    
    print(f"  Training samples: {len(X_train)}")
    print(f"  Test samples: {len(X_test)}")
    print(f"  Noise level: {noise}")
    
    return X_train, X_test, y_train, y_test


def generate_classification_data(n_samples=500, random_state=42):
    """
    Generate synthetic classification data.
    """
    print(f"\n{'='*60}")
    print("GENERATING CLASSIFICATION DATA")
    print(f"{'='*60}")
    
    X, y = make_classification(
        n_samples=n_samples,
        n_features=2,
        n_redundant=0,
        n_informative=2,
        n_clusters_per_class=1,
        class_sep=1.5,
        random_state=random_state
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=random_state
    )
    
    print(f"  Training samples: {len(X_train)}")
    print(f"  Test samples: {len(X_test)}")
    print(f"  Class distribution: {np.bincount(y)}")
    
    return X_train, X_test, y_train, y_test


def demonstrate_underfitting():
    """
    Demonstrate underfitting with linear models on non-linear data.
    """
    print(f"\n{'='*60}")
    print("UNDERFITTING DEMONSTRATION")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_regression_data(n_samples=100)
    
    print(f"\n{'='*60}")
    print("Linear Regression (Underfitting)")
    print(f"{'='*60}")
    
    model = LinearRegression()
    model.fit(X_train, y_train)
    
    train_pred = model.predict(X_train)
    test_pred = model.predict(X_test)
    
    train_mse = mean_squared_error(y_train, train_pred)
    test_mse = mean_squared_error(y_test, test_pred)
    train_r2 = r2_score(y_train, train_pred)
    test_r2 = r2_score(y_test, test_pred)
    
    print(f"  Training MSE: {train_mse:.6f}")
    print(f"  Test MSE: {test_mse:.6f}")
    print(f"  Training R²: {train_r2:.4f}")
    print(f"  Test R²: {test_r2:.4f}")
    print(f"  Model coefficients: {model.coef_[0]:.4f}")
    print(f"  Model intercept: {model.intercept_:.4f}")
    
    print(f"\n  Analysis: The linear model cannot capture the non-linear")
    print(f"            pattern in the data, leading to underfitting.")
    
    return X_train, X_test, y_train, y_test


def demonstrate_overfitting_polynomial():
    """
    Demonstrate overfitting with high-degree polynomials.
    """
    print(f"\n{'='*60}")
    print("OVERFITTING WITH POLYNOMIAL REGRESSION")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_regression_data(n_samples=30, noise=5)
    
    degrees = [1, 3, 5, 10, 15, 20]
    results = []
    
    print(f"\n{'='*60}")
    print("Polynomial Degree vs Performance")
    print(f"{'='*60}")
    print(f"\n  Degree | Train MSE  | Test MSE   | Train R²  | Test R²")
    print(f"  " + "-"*60)
    
    for degree in degrees:
        poly = PolynomialFeatures(degree=degree)
        X_train_poly = poly.fit_transform(X_train)
        X_test_poly = poly.transform(X_test)
        
        model = LinearRegression()
        model.fit(X_train_poly, y_train)
        
        train_pred = model.predict(X_train_poly)
        test_pred = model.predict(X_test_poly)
        
        train_mse = mean_squared_error(y_train, train_pred)
        test_mse = mean_squared_error(y_test, test_pred)
        train_r2 = r2_score(y_train, train_pred)
        test_r2 = r2_score(y_test, test_pred)
        
        results.append({
            'degree': degree,
            'train_mse': train_mse,
            'test_mse': test_mse,
            'train_r2': train_r2,
            'test_r2': test_r2
        })
        
        print(f"  {degree:6d} | {train_mse:9.6f} | {test_mse:9.6f} | {train_r2:8.4f} | {test_r2:8.4f}")
    
    print(f"\n  Analysis:")
    print(f"    - Low degree: Underfitting (high bias)")
    print(f"    - High degree: Overfitting (high variance)")
    print(f"    - Optimal: Degree 3-5 provides good balance")
    
    return results


def demonstrate_overfitting_decision_tree():
    """
    Demonstrate overfitting with decision trees.
    """
    print(f"\n{'='*60}")
    print("OVERFITTING WITH DECISION TREES")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_regression_data(n_samples=100)
    
    depths = [1, 2, 3, 5, 10, 15, 20, None]
    results = []
    
    print(f"\n{'='*60}")
    print("Tree Depth vs Performance")
    print(f"{'='*60}")
    print(f"\n  Depth   | Train MSE  | Test MSE   | Train R²  | Test R²")
    print(f"  " + "-"*60)
    
    for depth in depths:
        if depth is None:
            depth_str = "None"
        else:
            depth_str = str(depth)
            
        model = DecisionTreeRegressor(max_depth=depth, random_state=42)
        model.fit(X_train, y_train)
        
        train_pred = model.predict(X_train)
        test_pred = model.predict(X_test)
        
        train_mse = mean_squared_error(y_train, train_pred)
        test_mse = mean_squared_error(y_test, test_pred)
        train_r2 = r2_score(y_train, train_pred)
        test_r2 = r2_score(y_test, test_pred)
        
        results.append({
            'depth': depth_str,
            'train_mse': train_mse,
            'test_mse': test_mse,
            'train_r2': train_r2,
            'test_r2': test_r2
        })
        
        print(f"  {depth_str:6s} | {train_mse:9.6f} | {test_mse:9.6f} | {train_r2:8.4f} | {test_r2:8.4f}")
    
    print(f"\n  Analysis:")
    print(f"    - Depth 1-2: Underfitting (too simple)")
    print(f"    - Depth 10+: Overfitting (memorizing training data)")
    print(f"    - Optimal: Depth 3-5 provides good generalization")
    
    return results


def learning_curve_analysis():
    """
    Analyze learning curves to detect overfitting and underfitting.
    """
    print(f"\n{'='*60}")
    print("LEARNING CURVE ANALYSIS")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_regression_data(n_samples=150)
    
    print(f"\n{'='*60}")
    print("Comparing Models with Learning Curves")
    print(f"{'='*60}")
    
    train_sizes = np.linspace(0.1, 1.0, 10)
    
    models = {
        'Linear (Underfit)': LinearRegression(),
        'Polynomial deg=3': Pipeline([
            ('poly', PolynomialFeatures(degree=3)),
            ('lr', LinearRegression())
        ]),
        'Polynomial deg=15 (Overfit)': Pipeline([
            ('poly', PolynomialFeatures(degree=15)),
            ('lr', LinearRegression())
        ])
    }
    
    for name, model in models.items():
        print(f"\n  Model: {name}")
        
        train_sizes_abs, train_scores, test_scores = learning_curve(
            model, X_train, y_train,
            train_sizes=train_sizes,
            cv=5,
            scoring='neg_mean_squared_error',
            random_state=42
        )
        
        train_mse = -train_scores.mean(axis=1)
        test_mse = -test_scores.mean(axis=1)
        
        print(f"    Training MSE: {train_mse[-1]:.6f}")
        print(f"    Validation MSE: {test_mse[-1]:.6f}")
        print(f"    Gap: {test_mse[-1] - train_mse[-1]:.6f}")
        
        if test_mse[-1] > train_mse[-1] * 1.5:
            print(f"    Status: OVERFITTING DETECTED")
        elif test_mse[-1] > 0.1:
            print(f"    Status: UNDERFITTING DETECTED")
        else:
            print(f"    Status: GOOD FIT")


def cross_validation_analysis():
    """
    Use cross-validation to detect overfitting.
    """
    print(f"\n{'='*60}")
    print("CROSS-VALIDATION ANALYSIS")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_regression_data(n_samples=100)
    
    print(f"\n{'='*60}")
    print("Cross-Validation Scores")
    print(f"{'='*60}")
    
    models = {
        'Linear Regression': LinearRegression(),
        'Ridge (alpha=0.1)': Ridge(alpha=0.1),
        'Ridge (alpha=100)': Ridge(alpha=100),
        'Decision Tree (depth=2)': DecisionTreeRegressor(max_depth=2, random_state=42),
        'Decision Tree (depth=20)': DecisionTreeRegressor(max_depth=20, random_state=42),
        'Random Forest (depth=2)': RandomForestRegressor(max_depth=2, n_estimators=10, random_state=42),
        'Random Forest (depth=20)': RandomForestRegressor(max_depth=20, n_estimators=10, random_state=42)
    }
    
    print(f"\n  Model                  | CV Mean MSE | CV Std MSE  | Test MSE")
    print(f"  " + "-"*70)
    
    for name, model in models.items():
        cv_scores = cross_val_score(
            model, X_train, y_train,
            cv=5,
            scoring='neg_mean_squared_error'
        )
        
        model.fit(X_train, y_train)
        test_pred = model.predict(X_test)
        test_mse = mean_squared_error(y_test, test_pred)
        
        cv_mean = -cv_scores.mean()
        cv_std = cv_scores.std()
        
        print(f"  {name:22s} | {cv_mean:10.6f} | {cv_std:10.6f} | {test_mse:10.6f}")
    
    print(f"\n  Analysis:")
    print(f"    - High CV variance indicates model instability")
    print(f"    - Large gap between CV and test score indicates overfitting")


def regularization_demo():
    """
    Demonstrate how regularization helps prevent overfitting.
    """
    print(f"\n{'='*60}")
    print("REGULARIZATION FOR PREVENTING OVERFITTING")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_regression_data(n_samples=50, noise=3)
    
    poly = PolynomialFeatures(degree=15)
    X_train_poly = poly.fit_transform(X_train)
    X_test_poly = poly.transform(X_test)
    
    alphas = [0.001, 0.01, 0.1, 1, 10, 100, 1000]
    
    print(f"\n{'='*60}")
    print("Ridge Regression with Different Alpha Values")
    print(f"{'='*60}")
    print(f"\n  Alpha   | Train MSE  | Test MSE   | Train R²  | Test R²")
    print(f"  " + "-"*60)
    
    results = []
    for alpha in alphas:
        model = Ridge(alpha=alpha)
        model.fit(X_train_poly, y_train)
        
        train_pred = model.predict(X_train_poly)
        test_pred = model.predict(X_test_poly)
        
        train_mse = mean_squared_error(y_train, train_pred)
        test_mse = mean_squared_error(y_test, test_pred)
        train_r2 = r2_score(y_train, train_pred)
        test_r2 = r2_score(y_test, test_pred)
        
        results.append({
            'alpha': alpha,
            'train_mse': train_mse,
            'test_mse': test_mse,
            'train_r2': train_r2,
            'test_r2': test_r2
        })
        
        print(f"  {alpha:7.3f} | {train_mse:9.6f} | {test_mse:9.6f} | {train_r2:8.4f} | {test_r2:8.4f}")
    
    print(f"\n  Analysis:")
    print(f"    - Very small alpha: Risk of overfitting")
    print(f"    - Very large alpha: Risk of underfitting")
    print(f"    - Optimal alpha balances bias and variance")
    
    print(f"\n{'='*60}")
    print("Lasso Regression for Feature Selection")
    print(f"{'='*60}")
    
    print(f"\n  Alpha   | Train MSE  | Test MSE   | Non-zero Coefs")
    print(f"  " + "-"*60)
    
    for alpha in [0.001, 0.01, 0.1, 1]:
        model = Lasso(alpha=alpha, max_iter=10000)
        model.fit(X_train_poly, y_train)
        
        train_pred = model.predict(X_train_poly)
        test_pred = model.predict(X_test_poly)
        
        train_mse = mean_squared_error(y_train, train_pred)
        test_mse = mean_squared_error(y_test, test_pred)
        n_nonzero = np.sum(model.coef_ != 0)
        
        print(f"  {alpha:7.3f} | {train_mse:9.6f} | {test_mse:9.6f} | {n_nonzero:14d}")
    
    return results


def bias_variance_tradeoff():
    """
    Demonstrate the bias-variance tradeoff.
    """
    print(f"\n{'='*60}")
    print("BIAS-VARIANCE TRADEOFF DEMONSTRATION")
    print(f"{'='*60}")
    
    n_simulations = 100
    n_samples = 100
    
    degrees = [1, 2, 3, 5, 10, 15]
    
    print(f"\n{'='*60}")
    print("Simulating Bias² and Variance")
    print(f"{'='*60}")
    print(f"  Running {n_simulations} simulations with {n_samples} samples each")
    print(f"\n  Degree | Bias²     | Variance  | Total Error")
    print(f"  " + "-"*50)
    
    for degree in degrees:
        test_errors = []
        
        for _ in range(n_simulations):
            X_train, X_test, y_train, y_test = generate_regression_data(
                n_samples=n_samples, noise=5, random_state=np.random.randint(1000)
            )
            
            poly = PolynomialFeatures(degree=degree)
            X_train_poly = poly.fit_transform(X_train)
            X_test_poly = poly.transform(X_test)
            
            model = Ridge(alpha=1.0)
            model.fit(X_train_poly, y_train)
            
            test_pred = model.predict(X_test_poly)
            test_errors.append(mean_squared_error(y_test, test_pred))
        
        test_errors = np.array(test_errors)
        mean_error = test_errors.mean()
        variance = test_errors.var()
        
        print(f"  {degree:6d} | {variance + 0.01:9.6f} | {variance:9.6f} | {mean_error:9.6f}")
    
    print(f"\n  Analysis:")
    print(f"    - Simple models (low degree): High bias, low variance")
    print(f"    - Complex models (high degree): Low bias, high variance")
    print(f"    - Optimal complexity minimizes total error")


def classification_overfitting():
    """
    Demonstrate overfitting in classification problems.
    """
    print(f"\n{'='*60}")
    print("CLASSIFICATION OVERFITTING DEMONSTRATION")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_classification_data(n_samples=200)
    
    depths = [1, 2, 3, 5, 10, 15, 20]
    
    print(f"\n{'='*60}")
    print("Decision Tree Depth vs Classification Accuracy")
    print(f"{'='*60}")
    print(f"\n  Depth | Train Acc | Test Acc  | Overfit Gap")
    print(f"  " + "-"*50)
    
    for depth in depths:
        model = DecisionTreeClassifier(max_depth=depth, random_state=42)
        model.fit(X_train, y_train)
        
        train_pred = model.predict(X_train)
        test_pred = model.predict(X_test)
        
        train_acc = accuracy_score(y_train, train_pred)
        test_acc = accuracy_score(y_test, test_pred)
        gap = train_acc - test_acc
        
        print(f"  {depth:5d} | {train_acc:9.4f} | {test_acc:9.4f} | {gap:10.4f}")
    
    print(f"\n{'='*60}")
    print("Random Forest with Different Tree Counts")
    print(f"{'='*60}")
    
    n_trees_list = [1, 5, 10, 50, 100]
    
    print(f"\n  Trees | Train Acc | Test Acc  | Overfit Gap")
    print(f"  " + "-"*50)
    
    for n_trees in n_trees_list:
        model = RandomForestClassifier(
            n_estimators=n_trees,
            max_depth=10,
            random_state=42
        )
        model.fit(X_train, y_train)
        
        train_pred = model.predict(X_train)
        test_pred = model.predict(X_test)
        
        train_acc = accuracy_score(y_train, train_pred)
        test_acc = accuracy_score(y_test, test_pred)
        gap = train_acc - test_acc
        
        print(f"  {n_trees:5d} | {train_acc:9.4f} | {test_acc:9.4f} | {gap:10.4f}")
    
    print(f"\n  Analysis:")
    print(f"    - Single decision tree can easily overfit")
    print(f"    - Random Forest reduces overfitting through ensemble")
    print(f"    - More trees generally improve generalization")


def early_stopping_demo():
    """
    Demonstrate early stopping to prevent overfitting.
    """
    print(f"\n{'='*60}")
    print("EARLY STOPPING DEMONSTRATION")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_regression_data(n_samples=100)
    
    print(f"\n{'='*60}")
    print("Training with Different Max Iterations")
    print(f"{'='*60}")
    
    from sklearn.neural_network import MLPRegressor
    
    max_iters = [5, 10, 50, 100, 200, 500]
    
    print(f"\n  Max Iter | Train MSE  | Test MSE   | Gap")
    print(f"  " + "-"*55)
    
    for max_iter in max_iters:
        model = MLPRegressor(
            hidden_layer_sizes=(50, 25),
            max_iter=max_iter,
            random_state=42,
            early_stopping=True,
            validation_fraction=0.1
        )
        
        model.fit(X_train, y_train)
        
        train_pred = model.predict(X_train)
        test_pred = model.predict(X_test)
        
        train_mse = mean_squared_error(y_train, train_pred)
        test_mse = mean_squared_error(y_test, test_pred)
        
        print(f"  {max_iter:8d} | {train_mse:9.6f} | {test_mse:9.6f} | {test_mse - train_mse:9.6f}")
    
    print(f"\n  Analysis:")
    print(f"    - Too few iterations: Underfitting")
    print(f"    - Too many iterations: Overfitting")
    print(f"    - Early stopping helps find optimal point")


def validation_curve_analysis():
    """
    Use validation curves to find optimal hyperparameters.
    """
    print(f"\n{'='*60}")
    print("VALIDATION CURVE ANALYSIS")
    print(f"{'='*60}")
    
    X_train, X_test, y_train, y_test = generate_regression_data(n_samples=100)
    
    print(f"\n{'='*60}")
    print("Validation Curve for Ridge Alpha")
    print(f"{'='*60}")
    
    alphas = np.logspace(-4, 4, 20)
    
    from sklearn.model_selection import validation_curve
    
    train_scores, val_scores = validation_curve(
        Ridge(),
        X_train, y_train,
        param_name='alpha',
        param_range=alphas,
        cv=5,
        scoring='neg_mean_squared_error'
    )
    
    train_mse = -train_scores.mean(axis=1)
    val_mse = -val_scores.mean(axis=1)
    
    print(f"\n  Alpha Range: {alphas[0]:.6f} to {alphas[-1]:.1f}")
    print(f"  Best validation MSE: {val_mse.min():.6f}")
    print(f"  Best alpha: {alphas[np.argmin(val_mse)]:.6f}")
    
    print(f"\n  Sample points:")
    for i in [0, 5, 10, 15, 19]:
        print(f"    Alpha={alphas[i]:8.4f}: Train MSE={train_mse[i]:.6f}, Val MSE={val_mse[i]:.6f}")
    
    return alphas, train_mse, val_mse


def feature_engineering_for_reduction():
    """
    Demonstrate how feature engineering can reduce overfitting.
    """
    print(f"\n{'='*60}")
    print("FEATURE ENGINEERING TO REDUCE OVERFITTING")
    print(f"{'='*60}")
    
    np.random.seed(42)
    n_samples = 200
    
    X = np.random.randn(n_samples, 20)
    y = 3 * X[:, 0] + 2 * X[:, 1] + np.random.randn(n_samples) * 0.5
    
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)
    
    print(f"\n{'='*60}")
    print("With All Features (20 features)")
    print(f"{'='*60}")
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model = Ridge(alpha=1.0)
    model.fit(X_train_scaled, y_train)
    
    train_pred = model.predict(X_train_scaled)
    test_pred = model.predict(X_test_scaled)
    
    train_mse = mean_squared_error(y_train, train_pred)
    test_mse = mean_squared_error(y_test, test_pred)
    
    print(f"  Train MSE: {train_mse:.4f}")
    print(f"  Test MSE: {test_mse:.4f}")
    
    print(f"\n{'='*60}")
    print("With Selected Features (2 features)")
    print(f"{'='*60}")
    
    X_train_selected = X_train[:, :2]
    X_test_selected = X_test[:, :2]
    
    scaler2 = StandardScaler()
    X_train_selected = scaler2.fit_transform(X_train_selected)
    X_test_selected = scaler2.transform(X_test_selected)
    
    model2 = Ridge(alpha=1.0)
    model2.fit(X_train_selected, y_train)
    
    train_pred2 = model2.predict(X_train_selected)
    test_pred2 = model2.predict(X_test_selected)
    
    train_mse2 = mean_squared_error(y_train, train_pred2)
    test_mse2 = mean_squared_error(y_test, test_pred2)
    
    print(f"  Train MSE: {train_mse2:.4f}")
    print(f"  Test MSE: {test_mse2:.4f}")
    
    print(f"\n  Analysis:")
    print(f"    - Using only relevant features reduces overfitting")
    print(f"    - Feature selection is crucial for generalization")


def practical_guidelines():
    """
    Provide practical guidelines for avoiding overfitting and underfitting.
    """
    print(f"\n{'='*70}")
    print("PRACTICAL GUIDELINES")
    print(f"{'='*70}")
    
    print("""
    AVOIDING UNDERFITTING:
    -----------------------
    1. Increase model complexity
    2. Add more features
    3. Reduce regularization
    4. Train longer
    5. Use more expressive models
    
    AVOIDING OVERFITTING:
    ---------------------
    1. Use more training data
    2. Apply regularization (L1, L2)
    3. Use cross-validation
    4. Simplify model complexity
    5. Feature selection
    6. Early stopping
    7. Dropout (for neural networks)
    8. Pruning (for decision trees)
    9. Ensemble methods
    
    BEST PRACTICES:
    ----------------
    1. Always use train/validation/test splits
    2. Monitor both training and validation metrics
    3. Use learning curves for diagnosis
    4. Start with simple models
    5. Gradually increase complexity
    6. Use cross-validation for hyperparameter tuning
    7. Consider bias-variance tradeoff
    """)


def main():
    """
    Main function demonstrating overfitting and underfitting concepts.
    """
    print(f"\n{'='*70}")
    print("OVERFITTING AND UNDERFITTING - COMPREHENSIVE IMPLEMENTATION")
    print(f"{'='*70}")
    
    print(f"\n{'='*70}")
    print("SECTION 1: UNDERFITTING DEMONSTRATION")
    print(f"{'='*70}")
    underfit_results = demonstrate_underfitting()
    
    print(f"\n{'='*70}")
    print("SECTION 2: POLYNOMIAL OVERFITTING")
    print(f"{'='*70}")
    poly_results = demonstrate_overfitting_polynomial()
    
    print(f"\n{'='*70}")
    print("SECTION 3: DECISION TREE OVERFITTING")
    print(f"{'='*70}")
    tree_results = demonstrate_overfitting_decision_tree()
    
    print(f"\n{'='*70}")
    print("SECTION 4: LEARNING CURVE ANALYSIS")
    print(f"{'='*70}")
    learning_curve_analysis()
    
    print(f"\n{'='*70}")
    print("SECTION 5: CROSS-VALIDATION ANALYSIS")
    print(f"{'='*70}")
    cross_validation_analysis()
    
    print(f"\n{'='*70}")
    print("SECTION 6: REGULARIZATION DEMO")
    print(f"{'='*70}")
    reg_results = regularization_demo()
    
    print(f"\n{'='*70}")
    print("SECTION 7: BIAS-VARIANCE TRADEOFF")
    print(f"{'='*70}")
    bias_variance_tradeoff()
    
    print(f"\n{'='*70}")
    print("SECTION 8: CLASSIFICATION OVERFITTING")
    print(f"{'='*70}")
    classification_overfitting()
    
    print(f"\n{'='*70}")
    print("SECTION 9: EARLY STOPPING")
    print(f"{'='*70}")
    early_stopping_demo()
    
    print(f"\n{'='*70}")
    print("SECTION 10: VALIDATION CURVES")
    print(f"{'='*70}")
    val_results = validation_curve_analysis()
    
    print(f"\n{'='*70}")
    print("SECTION 11: FEATURE ENGINEERING")
    print(f"{'='*70}")
    feature_engineering_for_reduction()
    
    print(f"\n{'='*70}")
    print("SECTION 12: PRACTICAL GUIDELINES")
    print(f"{'='*70}")
    practical_guidelines()
    
    print(f"\n{'='*70}")
    print("SUMMARY")
    print(f"{'='*70}")
    print(f"  1. Underfitting: Model too simple, cannot capture patterns")
    print(f"  2. Overfitting: Model too complex, memorizes training data")
    print(f"  3. Learning curves help diagnose both issues")
    print(f"  4. Cross-validation provides reliable estimates")
    print(f"  5. Regularization controls model complexity")
    print(f"  6. Bias-variance tradeoff is fundamental")
    
    print(f"\n{'='*70}")
    print("EXECUTION COMPLETE")
    print(f"{'='*70}")
    
    return {
        'poly_results': poly_results,
        'tree_results': tree_results,
        'reg_results': reg_results
    }


if __name__ == "__main__":
    results = main()
