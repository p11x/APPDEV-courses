# Topic: K Nearest Neighbors
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for K Nearest Neighbors

I. INTRODUCTION
K Nearest Neighbors (KNN) is a simple, instance-based learning algorithm that classifies
new data points based on similarity to training data. It is a type of lazy learning where
the algorithm does not explicitly learn a model but stores all training data and makes
predictions at inference time by computing distances to nearest neighbors.

II. CORE_CONCEPTS
- Distance Metrics: Euclidean, Manhattan, Minkowski
- K Value Selection: Number of neighbors to consider
- Weighted Voting: Weight points by their distance
- Curse of Dimensionality: Challenges in high-dimensional spaces
- Feature Scaling: Importance of normalizing features

III. IMPLEMENTATION
- Synthetic data generation for demonstration
- Core KNN implementation using scikit-learn
- Banking example: Credit scoring prediction
- Healthcare example: Patient diagnosis classification

IV. EXAMPLES (Banking + Healthcare)

V. OUTPUT_RESULTS
- Model performance metrics
- Visualization of results

VI. TESTING
- Unit tests for core functionality

VII. ADVANCED_TOPICS
- Elbow method for optimal k selection
- Distance weighting strategies
- Dimensionality considerations

VIII. CONCLUSION
"""

# Import necessary libraries
import numpy as np
import pandas as pd
from sklearn.datasets import make_classification, load_breast_cancer
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.preprocessing import StandardScaler, MinMaxScaler
from sklearn.neighbors import KNeighborsClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score,
    confusion_matrix, classification_report, roc_curve, auc
)
import warnings
warnings.filterwarnings('ignore')


def generate_data(n_samples=500, n_features=10, n_informative=5, 
                  n_redundant=2, random_state=42):
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
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : array-like
        Feature matrix
    y : array-like
        Target labels
    feature_names : list
        Names of the features
    """
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_classes=2,
        random_state=random_state,
        flip_y=0.05
    )
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    
    print(f"Generated {n_samples} samples with {n_features} features")
    print(f"Class distribution: Class 0: {np.sum(y==0)}, Class 1: {np.sum(y==1)}")
    
    return X, y, feature_names


def calculate_distance_metrics():
    """
    Demonstrate different distance metrics used in KNN.
    
    Distance Metrics:
    - Euclidean: sqrt(sum((x_i - y_i)^2))
    - Manhattan: sum(|x_i - y_i|)
    - Minkowski: (sum(|x_i - y_i|)^p)^(1/p)
    """
    print("\n" + "="*60)
    print("DISTANCE METRICS COMPARISON")
    print("="*60)
    
    point1 = np.array([2, 3, 4])
    point2 = np.array([5, 7, 9])
    
    # Euclidean Distance
    euclidean = np.sqrt(np.sum((point1 - point2)**2))
    print(f"Euclidean Distance: {euclidean:.4f}")
    
    # Manhattan Distance
    manhattan = np.sum(np.abs(point1 - point2))
    print(f"Manhattan Distance: {manhattan:.4f}")
    
    # Minkowski Distance (p=3)
    minkowski = np.power(np.sum(np.power(np.abs(point1 - point2), 3)), 1/3)
    print(f"Minkowski Distance (p=3): {minkowski:.4f}")
    
    return {
        'euclidean': euclidean,
        'manhattan': manhattan,
        'minkowski': minkowski
    }


def core_knn(X_train, X_test, y_train, y_test, n_neighbors=5, 
             weights='uniform', metric='minkowski', p=2):
    """
    Core KNN implementation with configurable parameters.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and testing feature matrices
    y_train, y_test : array-like
        Training and testing labels
    n_neighbors : int
        Number of neighbors to consider (k)
    weights : str
        Weight function ('uniform' or 'distance')
    metric : str
        Distance metric ('euclidean', 'manhattan', 'minkowski')
    p : int
        Power parameter for Minkowski metric
        
    Returns:
    --------
    model : KNeighborsClassifier
        Trained KNN model
    y_pred : array
        Predictions on test set
    metrics : dict
        Performance metrics
    """
    model = KNeighborsClassifier(
        n_neighbors=n_neighbors,
        weights=weights,
        metric=metric,
        p=p
    )
    
    model.fit(X_train, y_train)
    y_pred = model.predict(X_test)
    
    metrics = {
        'accuracy': accuracy_score(y_test, y_pred),
        'precision': precision_score(y_test, y_pred, average='weighted'),
        'recall': recall_score(y_test, y_pred, average='weighted'),
        'f1_score': f1_score(y_test, y_pred, average='weighted')
    }
    
    return model, y_pred, metrics


def k_value_analysis(X_train, X_test, y_train, y_test, max_k=30):
    """
    Analyze different k values to find optimal k using elbow method.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and testing data
    y_train, y_test : array-like
        Training and testing labels
    max_k : int
        Maximum k value to evaluate
        
    Returns:
    --------
    results : dict
        Dictionary containing k values and corresponding metrics
    """
    print("\n" + "="*60)
    print("K VALUE ANALYSIS (Elbow Method)")
    print("="*60)
    
    k_values = range(1, max_k + 1)
    train_scores = []
    test_scores = []
    cv_scores = []
    
    for k in k_values:
        model = KNeighborsClassifier(n_neighbors=k)
        model.fit(X_train, y_train)
        
        train_scores.append(model.score(X_train, y_train))
        test_scores.append(model.score(X_test, y_test))
        
        cv_score = cross_val_score(model, X_train, y_train, cv=5).mean()
        cv_scores.append(cv_score)
    
    results = {
        'k_values': list(k_values),
        'train_scores': train_scores,
        'test_scores': test_scores,
        'cv_scores': cv_scores
    }
    
    optimal_k = k_values[np.argmax(cv_scores)]
    print(f"Optimal K based on CV score: {optimal_k}")
    print(f"Best CV Score: {max(cv_scores):.4f}")
    
    print("\nK Value | Train Acc | Test Acc | CV Score")
    print("-" * 50)
    for i in [0, 4, 9, 14, 19, 24, 29]:
        if i < len(k_values):
            print(f"  {k_values[i]:3d}  |  {train_scores[i]:.4f}  |  {test_scores[i]:.4f}  | {cv_scores[i]:.4f}")
    
    return results, optimal_k


def weighted_voting_comparison(X_train, X_test, y_train, y_test, k=5):
    """
    Compare uniform weighting vs distance weighting.
    
    In distance-weighted KNN, closer neighbors have more influence
    on the prediction. This is useful when neighbors have varying
    degrees of relevance.
    """
    print("\n" + "="*60)
    print("WEIGHTED VOTING COMPARISON")
    print("="*60)
    
    # Uniform weights
    model_uniform = KNeighborsClassifier(n_neighbors=k, weights='uniform')
    model_uniform.fit(X_train, y_train)
    y_pred_uniform = model_uniform.predict(X_test)
    
    # Distance weights
    model_distance = KNeighborsClassifier(n_neighbors=k, weights='distance')
    model_distance.fit(X_train, y_train)
    y_pred_distance = model_distance.predict(X_test)
    
    results = {
        'uniform': {
            'accuracy': accuracy_score(y_test, y_pred_uniform),
            'predictions': y_pred_uniform
        },
        'distance': {
            'accuracy': accuracy_score(y_test, y_pred_distance),
            'predictions': y_pred_distance
        }
    }
    
    print(f"Uniform Weights - Accuracy: {results['uniform']['accuracy']:.4f}")
    print(f"Distance Weights - Accuracy: {results['distance']['accuracy']:.4f}")
    
    return results


def feature_scaling_importance():
    """
    Demonstrate why feature scaling is critical for KNN.
    
    KNN uses distance metrics which are sensitive to feature scales.
    Features with larger ranges dominate the distance calculation
    if not properly scaled.
    """
    print("\n" + "="*60)
    print("FEATURE SCALING IMPORTANCE")
    print("="*60)
    
    np.random.seed(42)
    
    # Create data with different feature scales
    X = np.random.randn(200, 3)
    X[:, 0] = X[:, 0] * 1        # Scale: ~1
    X[:, 1] = X[:, 1] * 100     # Scale: ~100
    X[:, 2] = X[:, 2] * 1000    # Scale: ~1000
    
    y = (X[:, 0] + 0.01*X[:, 1] + 0.001*X[:, 2] > 0).astype(int)
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42
    )
    
    # Without scaling
    model_unscaled = KNeighborsClassifier(n_neighbors=5)
    model_unscaled.fit(X_train, y_train)
    acc_unscaled = model_unscaled.score(X_test, y_test)
    
    # With StandardScaler
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    model_scaled = KNeighborsClassifier(n_neighbors=5)
    model_scaled.fit(X_train_scaled, y_train)
    acc_scaled = model_scaled.score(X_test_scaled, y_test)
    
    print(f"Without scaling: {acc_unscaled:.4f}")
    print(f"With StandardScaler: {acc_scaled:.4f}")
    
    # Show how first feature dominates without scaling
    distances = X_test_scaled[:5] - X_train_scaled[:1]
    print(f"\nFeature contributions (scaled): {distances[0]}")
    
    return {
        'unscaled_accuracy': acc_unscaled,
        'scaled_accuracy': acc_scaled
    }


def curse_of_dimensionality_analysis():
    """
    Demonstrate the curse of dimensionality in KNN.
    
    As dimensions increase, the distance between points becomes more uniform,
    making it harder for KNN to find meaningful neighbors.
    """
    print("\n" + "="*60)
    print("CURSE OF DIMENSIONALITY ANALYSIS")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 500
    dimensions = [2, 5, 10, 20, 50, 100]
    
    results = []
    
    for dim in dimensions:
        X = np.random.randn(n_samples, dim)
        y = np.random.randint(0, 2, n_samples)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.3, random_state=42
        )
        
        scaler = StandardScaler()
        X_train_scaled = scaler.fit_transform(X_train)
        X_test_scaled = scaler.transform(X_test)
        
        model = KNeighborsClassifier(n_neighbors=5)
        model.fit(X_train_scaled, y_train)
        
        acc = model.score(X_test_scaled, y_test)
        
        # Calculate mean distance to neighbors
        distances, _ = model.kneighbors(X_test_scaled[:10])
        mean_dist = np.mean(distances)
        
        results.append({
            'dimensions': dim,
            'accuracy': acc,
            'mean_distance': mean_dist
        })
        
        print(f"Dimensions: {dim:3d} | Accuracy: {acc:.4f} | Mean Dist: {mean_dist:.4f}")
    
    return results


def banking_example():
    """
    Banking Example: Credit Scoring Prediction
    
    Use KNN to predict whether a loan applicant will default based on:
    - Credit score
    - Annual income
    - Debt-to-income ratio
    - Number of existing loans
    - Employment duration
    """
    print("\n" + "="*80)
    print("BANKING EXAMPLE: Credit Scoring Prediction")
    print("="*80)
    
    np.random.seed(42)
    n_samples = 1000
    
    # Generate synthetic credit scoring data
    credit_score = np.random.normal(650, 100, n_samples)
    annual_income = np.random.exponential(50000, n_samples)
    debt_to_income = np.random.uniform(0.1, 0.5, n_samples)
    num_loans = np.random.poisson(2, n_samples)
    employment_years = np.random.exponential(5, n_samples)
    
    # Create feature matrix
    X = np.column_stack([
        credit_score,
        annual_income,
        debt_to_income,
        num_loans,
        employment_years
    ])
    
    # Create target: 1 = Default, 0 = No Default
    # Higher debt-to-income, lower credit score, fewer employment years -> higher default risk
    default_prob = (
        (700 - credit_score) / 700 * 0.3 +
        (0.3 - debt_to_income) / 0.3 * 0.3 +
        (5 - employment_years) / 10 * 0.2 +
        num_loans / 10 * 0.2
    )
    y = (default_prob + np.random.random(n_samples) * 0.2 > 0.5).astype(int)
    
    feature_names = ['credit_score', 'annual_income', 'debt_to_income', 
                     'num_loans', 'employment_years']
    
    print(f"\nDataset: {n_samples} loan applications")
    print(f"Features: {feature_names}")
    print(f"Default rate: {np.mean(y)*100:.1f}%")
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42, stratify=y
    )
    
    # Scale features (critical for KNN)
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # Find optimal k
    k_values = range(1, 31)
    cv_scores = []
    
    for k in k_values:
        model = KNeighborsClassifier(n_neighbors=k, weights='distance')
        scores = cross_val_score(model, X_train_scaled, y_train, cv=5)
        cv_scores.append(scores.mean())
    
    optimal_k = k_values[np.argmax(cv_scores)]
    print(f"\nOptimal K: {optimal_k}")
    
    # Train final model
    model = KNeighborsClassifier(n_neighbors=optimal_k, weights='distance')
    model.fit(X_train_scaled, y_train)
    y_pred = model.predict(X_test_scaled)
    
    # Evaluate
    print("\n" + "-"*40)
    print("MODEL PERFORMANCE")
    print("-"*40)
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred):.4f}")
    print(f"Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"F1-Score: {f1_score(y_test, y_pred):.4f}")
    
    print("\nConfusion Matrix:")
    cm = confusion_matrix(y_test, y_pred)
    print(f"  TN: {cm[0,0]:4d}  FP: {cm[0,1]:4d}")
    print(f"  FN: {cm[1,0]:4d}  TP: {cm[1,1]:4d}")
    
    # Sample predictions
    print("\nSample Predictions:")
    sample_indices = np.random.choice(len(y_test), 5, replace=False)
    for idx in sample_indices:
        actual = y_test[idx]
        predicted = y_pred[idx]
        print(f"  Sample {idx}: Actual={actual}, Predicted={predicted}")
    
    return {
        'model': model,
        'scaler': scaler,
        'optimal_k': optimal_k,
        'accuracy': accuracy_score(y_test, y_pred)
    }


def healthcare_example():
    """
    Healthcare Example: Breast Cancer Diagnosis Prediction
    
    Use KNN to classify tumors as malignant or benign based on:
    - Cell nucleus characteristics
    - Texture, radius, perimeter, area
    - Smoothness, compactness
    """
    print("\n" + "="*80)
    print("HEALTHCARE EXAMPLE: Breast Cancer Diagnosis")
    print("="*80)
    
    # Load breast cancer dataset
    data = load_breast_cancer()
    X = data.data
    y = data.target
    feature_names = data.feature_names
    
    print(f"\nDataset: {X.shape[0]} samples, {X.shape[1]} features")
    print(f"Classes: {data.target_names}")
    print(f"Class distribution - Benign: {np.sum(y==0)}, Malignant: {np.sum(y==1)}")
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42, stratify=y
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # Analyze different k values
    k_values = range(1, 31)
    test_scores = []
    
    for k in k_values:
        model = KNeighborsClassifier(n_neighbors=k)
        model.fit(X_train_scaled, y_train)
        test_scores.append(model.score(X_test_scaled, y_test))
    
    optimal_k = k_values[np.argmax(test_scores)]
    print(f"\nOptimal K: {optimal_k}")
    
    # Train final model with optimal k
    model = KNeighborsClassifier(n_neighbors=optimal_k, weights='distance', metric='manhattan')
    model.fit(X_train_scaled, y_train)
    y_pred = model.predict(X_test_scaled)
    
    # Evaluate
    print("\n" + "-"*40)
    print("MODEL PERFORMANCE")
    print("-"*40)
    print(f"Accuracy: {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred):.4f}")
    print(f"Recall: {recall_score(y_test, y_pred):.4f}")
    print(f"F1-Score: {f1_score(y_test, y_pred):.4f}")
    
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred, target_names=data.target_names))
    
    # Feature importance (based on distance to neighbors)
    distances, indices = model.kneighbors(X_test_scaled[:5])
    print("\nNeighbor analysis for first 5 test samples:")
    for i in range(5):
        print(f"  Sample {i}: {len(indices[i])} neighbors at distances {distances[i]}")
    
    return {
        'model': model,
        'scaler': scaler,
        'optimal_k': optimal_k,
        'accuracy': accuracy_score(y_test, y_pred),
        'feature_names': feature_names
    }


def run_comprehensive_test():
    """
    Run comprehensive tests on all implemented functions.
    """
    print("\n" + "="*80)
    print("COMPREHENSIVE TESTING")
    print("="*80)
    
    # Generate test data
    X, y, feature_names = generate_data(n_samples=200, n_features=8)
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # Test 1: Distance metrics
    print("\n[Test 1] Distance Metrics")
    distances = calculate_distance_metrics()
    assert 'euclidean' in distances
    assert 'manhattan' in distances
    print("PASSED")
    
    # Test 2: Core KNN
    print("\n[Test 2] Core KNN Implementation")
    model, y_pred, metrics = core_knn(
        X_train_scaled, X_test_scaled, y_train, y_test,
        n_neighbors=5, weights='uniform'
    )
    assert 0 <= metrics['accuracy'] <= 1
    print(f"Accuracy: {metrics['accuracy']:.4f}")
    print("PASSED")
    
    # Test 3: K value analysis
    print("\n[Test 3] K Value Analysis")
    results, optimal_k = k_value_analysis(
        X_train_scaled, X_test_scaled, y_train, y_test, max_k=15
    )
    assert optimal_k in range(1, 16)
    print(f"Optimal K: {optimal_k}")
    print("PASSED")
    
    # Test 4: Weighted voting
    print("\n[Test 4] Weighted Voting")
    voting_results = weighted_voting_comparison(
        X_train_scaled, X_test_scaled, y_train, y_test, k=5
    )
    assert 'uniform' in voting_results
    assert 'distance' in voting_results
    print("PASSED")
    
    # Test 5: Feature scaling
    print("\n[Test 5] Feature Scaling Importance")
    scaling_results = feature_scaling_importance()
    assert scaling_results['scaled_accuracy'] >= scaling_results['unscaled_accuracy']
    print("PASSED")
    
    # Test 6: Curse of dimensionality
    print("\n[Test 6] Curse of Dimensionality")
    dim_results = curse_of_dimensionality_analysis()
    assert len(dim_results) == 6
    print("PASSED")
    
    print("\n" + "="*80)
    print("ALL TESTS PASSED SUCCESSFULLY")
    print("="*80)


def main():
    """
    Main function to execute all KNN implementations and examples.
    """
    print("="*80)
    print("K NEAREST NEIGHBORS - COMPREHENSIVE IMPLEMENTATION")
    print("="*80)
    print("\nExecuting K Nearest Neighbors implementation")
    
    # Generate and analyze data
    X, y, feature_names = generate_data(n_samples=500)
    
    # Split data
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.3, random_state=42, stratify=y
    )
    
    # Scale features
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # Core KNN demonstration
    print("\n" + "="*60)
    print("CORE KNN DEMONSTRATION")
    print("="*60)
    
    model, y_pred, metrics = core_knn(
        X_train_scaled, X_test_scaled, y_train, y_test,
        n_neighbors=5, weights='distance', metric='minkowski', p=2
    )
    
    print(f"Accuracy: {metrics['accuracy']:.4f}")
    print(f"Precision: {metrics['precision']:.4f}")
    print(f"Recall: {metrics['recall']:.4f}")
    print(f"F1-Score: {metrics['f1_score']:.4f}")
    
    # K value analysis
    results, optimal_k = k_value_analysis(
        X_train_scaled, X_test_scaled, y_train, y_test
    )
    
    # Distance metrics demonstration
    calculate_distance_metrics()
    
    # Weighted voting comparison
    weighted_voting_comparison(X_train_scaled, X_test_scaled, y_train, y_test)
    
    # Feature scaling importance
    feature_scaling_importance()
    
    # Curse of dimensionality
    curse_of_dimensionality_analysis()
    
    # Banking example
    banking_result = banking_example()
    
    # Healthcare example
    healthcare_result = healthcare_example()
    
    # Run comprehensive tests
    run_comprehensive_test()
    
    print("\n" + "="*80)
    print("IMPLEMENTATION COMPLETE")
    print("="*80)
    print("\nKey Takeaways:")
    print("1. KNN is a simple but effective algorithm for classification")
    print("2. Feature scaling is critical for distance-based methods")
    print("3. Optimal k should be chosen using cross-validation")
    print("4. Distance weighting can improve predictions")
    print("5. KNN suffers from the curse of dimensionality")
    print("="*80)


if __name__ == "__main__":
    main()
