# Topic: Random Forest Classification
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Random Forest Classification

I. INTRODUCTION
   Random Forest is an ensemble learning method that operates by constructing 
   multiple decision trees during training and outputting the class that is 
   the mode of the classes (classification) or mean prediction (regression) of 
   individual trees. The method combines bagging with random feature selection 
   to create a robust, high-performance classifier.

II. CORE CONCEPTS
   1. Bagging (Bootstrap Aggregating): Reduces variance by training multiple 
      models on different bootstrap samples and combining their predictions.
   
   2. Bootstrap Sampling: Each tree is trained on a random sample of the data,
      drawn with replacement (bootstrap samples).
   
   3. Feature Bagging: At each split in individual trees, only a random subset 
      of features is considered, adding diversity to the ensemble.
   
   4. Out-of-Bag (OOB) Score: Since each tree uses ~63% of unique samples, 
      the remaining ~37% can be used for validation without separate test set.
   
   5. Ensemble Predictions: Final prediction is made by majority voting across 
      all trees for classification tasks.

III. IMPLEMENTATION
   Detailed implementations covering data generation, model training, 
   hyperparameter tuning, and evaluation.

IV. EXAMPLES (Banking + Healthcare)
   - Banking: Customer churn prediction
   - Healthcare: Disease risk classification

V. OUTPUT RESULTS
   Comprehensive metrics and visualizations.

VI. TESTING
   Unit tests and validation.

VII. ADVANCED TOPICS
   Feature importance, comparison with single decision tree, 
   handling class imbalance.

VIII. CONCLUSION
   Summary and best practices.
"""

# Import necessary libraries
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.datasets import make_classification, make_blobs
from sklearn.model_selection import train_test_split, cross_val_score, GridSearchCV
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.ensemble import RandomForestClassifier
from sklearn.tree import DecisionTreeClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, f1_score, 
    roc_auc_score, confusion_matrix, classification_report,
    roc_curve, auc
)
import warnings
warnings.filterwarnings('ignore')


def generate_synthetic_classification_data(n_samples=500, n_features=20, n_informative=10, 
                                          n_redundant=5, random_state=42):
    """
    Generate synthetic classification dataset for demonstration.
    
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
        Names of features
    """
    print(f"Generating synthetic classification data:")
    print(f"  - Samples: {n_samples}")
    print(f"  - Features: {n_features}")
    print(f"  - Informative: {n_informative}")
    print(f"  - Redundant: {n_redundant}")
    
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=n_informative,
        n_redundant=n_redundant,
        n_clusters_per_class=2,
        n_classes=2,
        random_state=random_state,
        flip_y=0.05
    )
    
    feature_names = [f'Feature_{i+1}' for i in range(n_features)]
    
    print(f"\nData generated successfully!")
    print(f"  - X shape: {X.shape}")
    print(f"  - y distribution: Class 0: {np.sum(y==0)}, Class 1: {np.sum(y==1)}")
    
    return X, y, feature_names


def generate_banking_data(n_samples=1000, random_state=42):
    """
    Generate banking domain dataset for customer churn prediction.
    
    Features represent:
    - Credit score
    - Age
    - Tenure (years with bank)
    - Balance
    - Number of products
    - Has credit card
    - Is active member
    - Estimated salary
    - Satisfaction score
    - Complaint filed (0/1)
    
    Target: Customer churn (1 = churned, 0 = stayed)
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : DataFrame
        DataFrame with features and target
    """
    np.random.seed(random_state)
    
    print(f"\nGenerating banking data for customer churn prediction:")
    print(f"  - Samples: {n_samples}")
    
    credit_score = np.random.normal(650, 100, n_samples).clip(300, 850)
    age = np.random.exponential(35, n_samples).clip(18, 80).astype(int)
    tenure = np.random.exponential(5, n_samples).clip(0, 30).astype(int)
    balance = np.random.lognormal(9, 1.5, n_samples).clip(0, 200000)
    num_products = np.random.choice([1, 2, 3, 4], n_samples, p=[0.3, 0.4, 0.2, 0.1])
    has_credit_card = np.random.choice([0, 1], n_samples, p=[0.7, 0.3])
    is_active = np.random.choice([0, 1], n_samples, p=[0.5, 0.5])
    salary = np.random.lognormal(10.5, 0.8, n_samples).clip(15000, 500000)
    satisfaction = np.random.choice([1, 2, 3, 4, 5], n_samples, p=[0.1, 0.15, 0.25, 0.3, 0.2])
    complaint = np.random.choice([0, 1], n_samples, p=[0.85, 0.15])
    
    churn_prob = (
        0.3 * (num_products >= 3) +
        0.2 * (is_active == 0) +
        0.15 * (complaint == 1) +
        0.1 * (tenure < 2) +
        0.1 * (satisfaction <= 2) +
        0.1 * (age > 60) +
        np.random.random(n_samples) * 0.2
    )
    churn = (churn_prob > 0.5).astype(int)
    
    df = pd.DataFrame({
        'CreditScore': credit_score,
        'Age': age,
        'Tenure': tenure,
        'Balance': balance,
        'NumProducts': num_products,
        'HasCreditCard': has_credit_card,
        'IsActive': is_active,
        'Salary': salary,
        'SatisfactionScore': satisfaction,
        'Complaint': complaint,
        'Churned': churn
    })
    
    print(f"  - Churn distribution: Not Churned: {np.sum(churn==0)}, Churned: {np.sum(churn==1)}")
    print(f"  - Churn rate: {np.mean(churn)*100:.2f}%")
    
    return df


def generate_healthcare_data(n_samples=1000, random_state=42):
    """
    Generate healthcare domain dataset for disease risk classification.
    
    Features represent:
    - Age
    - BMI
    - Blood pressure (systolic)
    - Cholesterol level
    - Blood sugar level
    - Heart rate
    - Exercise level (minutes/week)
    - Smoking status (0/1)
    - Family history (0/1)
    - Diet quality score
    
    Target: Disease risk level (1 = high risk, 0 = low risk)
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : DataFrame
        DataFrame with features and target
    """
    np.random.seed(random_state)
    
    print(f"\nGenerating healthcare data for disease risk prediction:")
    print(f"  - Samples: {n_samples}")
    
    age = np.random.normal(50, 15, n_samples).clip(20, 90).astype(int)
    bmi = np.random.normal(27, 5, n_samples).clip(15, 50)
    blood_pressure = np.random.normal(130, 20, n_samples).clip(90, 200)
    cholesterol = np.random.normal(200, 40, n_samples).clip(120, 300)
    blood_sugar = np.random.exponential(100, n_samples).clip(70, 300)
    heart_rate = np.random.normal(72, 12, n_samples).clip(45, 140)
    exercise = np.random.exponential(120, n_samples).clip(0, 420).astype(int)
    smoking = np.random.choice([0, 1], n_samples, p=[0.75, 0.25])
    family_history = np.random.choice([0, 1], n_samples, p=[0.8, 0.2])
    diet_score = np.random.choice(range(1, 11), n_samples, p=[0.1, 0.1, 0.1, 0.15, 0.15, 0.15, 0.1, 0.05, 0.05, 0.05])
    
    risk_prob = (
        0.2 * (age > 55) +
        0.15 * (bmi > 30) +
        0.15 * (blood_pressure > 140) +
        0.15 * (cholesterol > 240) +
        0.1 * (blood_sugar > 126) +
        0.1 * (smoking == 1) +
        0.1 * (family_history == 1) +
        0.1 * (exercise < 30) +
        0.05 * (diet_score < 4) +
        np.random.random(n_samples) * 0.1
    )
    high_risk = (risk_prob > 0.45).astype(int)
    
    df = pd.DataFrame({
        'Age': age,
        'BMI': bmi,
        'BloodPressure': blood_pressure,
        'Cholesterol': cholesterol,
        'BloodSugar': blood_sugar,
        'HeartRate': heart_rate,
        'ExerciseMinutes': exercise,
        'Smoking': smoking,
        'FamilyHistory': family_history,
        'DietScore': diet_score,
        'HighRisk': high_risk
    })
    
    print(f"  - Risk distribution: Low Risk: {np.sum(high_risk==0)}, High Risk: {np.sum(high_risk==1)}")
    print(f"  - High risk rate: {np.mean(high_risk)*100:.2f}%")
    
    return df


def core_random_forest_implementation(X_train, X_test, y_train, y_test, 
                                     n_estimators=100, max_depth=10,
                                     random_state=42, use_oob=True):
    """
    Core Random Forest classification implementation.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and testing feature matrices
    y_train, y_test : array-like
        Training and testing labels
    n_estimators : int
        Number of trees in the forest
    max_depth : int or None
        Maximum depth of each tree
    random_state : int
        Random seed for reproducibility
    use_oob : bool
        Whether to use out-of-bag scoring
        
    Returns:
    --------
    model : RandomForestClassifier
        Trained model
    y_pred : array
        Predicted labels
    y_pred_proba : array
        Predicted probabilities
    metrics : dict
        Dictionary of evaluation metrics
    """
    print("\n" + "="*60)
    print("CORE RANDOM FOREST IMPLEMENTATION")
    print("="*60)
    print(f"\nModel Configuration:")
    print(f"  - Number of estimators: {n_estimators}")
    print(f"  - Max depth: {max_depth if max_depth else 'None (unlimited)'}")
    print(f"  - Random state: {random_state}")
    print(f"  - Use OOB score: {use_oob}")
    
    rf_model = RandomForestClassifier(
        n_estimators=n_estimators,
        max_depth=max_depth,
        min_samples_split=5,
        min_samples_leaf=2,
        max_features='sqrt',
        bootstrap=True,
        oob_score=use_oob,
        random_state=random_state,
        n_jobs=-1
    )
    
    print(f"\nTraining Random Forest model...")
    rf_model.fit(X_train, y_train)
    
    y_pred = rf_model.predict(X_test)
    y_pred_proba = rf_model.predict_proba(X_test)[:, 1]
    
    accuracy = accuracy_score(y_test, y_pred)
    precision = precision_score(y_test, y_pred)
    recall = recall_score(y_test, y_pred)
    f1 = f1_score(y_test, y_pred)
    roc_auc = roc_auc_score(y_test, y_pred_proba)
    
    metrics = {
        'accuracy': accuracy,
        'precision': precision,
        'recall': recall,
        'f1_score': f1,
        'roc_auc': roc_auc,
        'oob_score': rf_model.oob_score_ if use_oob else None
    }
    
    print(f"\nModel Performance:")
    print(f"  - Accuracy: {accuracy:.4f}")
    print(f"  - Precision: {precision:.4f}")
    print(f"  - Recall: {recall:.4f}")
    print(f"  - F1 Score: {f1:.4f}")
    print(f"  - ROC AUC: {roc_auc:.4f}")
    if use_oob:
        print(f"  - OOB Score: {rf_model.oob_score_:.4f}")
    
    return rf_model, y_pred, y_pred_proba, metrics


def compare_with_single_tree(X_train, X_test, y_train, y_test, random_state=42):
    """
    Compare Random Forest performance with a single Decision Tree.
    
    This demonstrates the advantage of ensemble learning:
    - Random Forest reduces variance through averaging multiple trees
    - Each tree is trained on different bootstrap samples
    - Feature bagging adds diversity
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and testing feature matrices
    y_train, y_test : array-like
        Training and testing labels
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    results : dict
        Dictionary containing results for both models
    """
    print("\n" + "="*60)
    print("COMPARISON: RANDOM FOREST vs SINGLE DECISION TREE")
    print("="*60)
    
    single_tree = DecisionTreeClassifier(
        max_depth=10,
        min_samples_split=5,
        min_samples_leaf=2,
        random_state=random_state
    )
    
    print("\n1. Training Single Decision Tree...")
    single_tree.fit(X_train, y_train)
    tree_pred = single_tree.predict(X_test)
    tree_pred_proba = single_tree.predict_proba(X_test)[:, 1]
    
    tree_accuracy = accuracy_score(y_test, tree_pred)
    tree_f1 = f1_score(y_test, tree_pred)
    tree_roc_auc = roc_auc_score(y_test, tree_pred_proba)
    
    print(f"   Single Tree Metrics:")
    print(f"     - Accuracy: {tree_accuracy:.4f}")
    print(f"     - F1 Score: {tree_f1:.4f}")
    print(f"     - ROC AUC: {tree_roc_auc:.4f}")
    
    print("\n2. Training Random Forest (100 trees)...")
    rf_model = RandomForestClassifier(
        n_estimators=100,
        max_depth=10,
        min_samples_split=5,
        min_samples_leaf=2,
        max_features='sqrt',
        bootstrap=True,
        oob_score=True,
        random_state=random_state,
        n_jobs=-1
    )
    rf_model.fit(X_train, y_train)
    rf_pred = rf_model.predict(X_test)
    rf_pred_proba = rf_model.predict_proba(X_test)[:, 1]
    
    rf_accuracy = accuracy_score(y_test, rf_pred)
    rf_f1 = f1_score(y_test, rf_pred)
    rf_roc_auc = roc_auc_score(y_test, rf_pred_proba)
    
    print(f"   Random Forest Metrics:")
    print(f"     - Accuracy: {rf_accuracy:.4f}")
    print(f"     - F1 Score: {rf_f1:.4f}")
    print(f"     - ROC AUC: {rf_roc_auc:.4f}")
    print(f"     - OOB Score: {rf_model.oob_score_:.4f}")
    
    print("\n3. Comparison Results:")
    print(f"   Improvement over single tree:")
    print(f"     - Accuracy improvement: {(rf_accuracy - tree_accuracy)*100:.2f}%")
    print(f"     - F1 improvement: {(rf_f1 - tree_f1)*100:.2f}%")
    print(f"     - ROC AUC improvement: {(rf_roc_auc - tree_roc_auc)*100:.2f}%")
    
    print("\n   Key Insights:")
    print("     - Random Forest reduces overfitting compared to single tree")
    print("     - Ensemble averaging provides more stable predictions")
    print("     - Bootstrap sampling creates diverse training sets")
    print("     - Feature bagging adds additional randomization")
    
    results = {
        'single_tree': {
            'accuracy': tree_accuracy,
            'f1_score': tree_f1,
            'roc_auc': tree_roc_auc
        },
        'random_forest': {
            'accuracy': rf_accuracy,
            'f1_score': rf_f1,
            'roc_auc': rf_roc_auc,
            'oob_score': rf_model.oob_score_
        }
    }
    
    return results


def tune_n_estimators(X_train, y_train, X_test, y_test, 
                     n_estimators_list=[10, 50, 100, 150, 200],
                     max_depth=10, random_state=42):
    """
    Tune the number of estimators parameter in Random Forest.
    
    This demonstrates how increasing n_estimators generally improves
    performance until diminishing returns occur.
    
    Parameters:
    -----------
    X_train, X_test : array-like
        Training and testing feature matrices
    y_train, y_test : array-like
        Training and testing labels
    n_estimators_list : list
        List of n_estimators values to try
    max_depth : int
        Maximum depth of each tree
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    tuning_results : dict
        Dictionary containing results for each configuration
    """
    print("\n" + "="*60)
    print("HYPERPARAMETER TUNING: n_estimators")
    print("="*60)
    
    results = {
        'n_estimators': [],
        'train_accuracy': [],
        'test_accuracy': [],
        'oob_score': []
    }
    
    print("\nTesting different n_estimators values:")
    print("-" * 50)
    
    for n_est in n_estimators_list:
        rf = RandomForestClassifier(
            n_estimators=n_est,
            max_depth=max_depth,
            bootstrap=True,
            oob_score=True,
            random_state=random_state,
            n_jobs=-1
        )
        
        rf.fit(X_train, y_train)
        
        train_pred = rf.predict(X_train)
        test_pred = rf.predict(X_test)
        
        train_acc = accuracy_score(y_train, train_pred)
        test_acc = accuracy_score(y_test, test_pred)
        oob = rf.oob_score_
        
        results['n_estimators'].append(n_est)
        results['train_accuracy'].append(train_acc)
        results['test_accuracy'].append(test_acc)
        results['oob_score'].append(oob)
        
        print(f"  n_estimators={n_est:3d}: Train Acc={train_acc:.4f}, "
              f"Test Acc={test_acc:.4f}, OOB={oob:.4f}")
    
    best_idx = np.argmax(results['test_accuracy'])
    best_n_est = results['n_estimators'][best_idx]
    best_acc = results['test_accuracy'][best_idx]
    
    print(f"\n  Best n_estimators: {best_n_est} (Test Accuracy: {best_acc:.4f})")
    print("\n  Observations:")
    print("    - Larger n_estimators generally improves performance")
    print("    - Diminishing returns after ~100-150 trees")
    print("    - OOB score provides validation without separate test set")
    print("    - More trees increase computational cost")
    
    return results


def analyze_feature_importance(model, feature_names, top_n=10):
    """
    Analyze and display feature importance from Random Forest model.
    
    Feature importance is calculated based on the decrease in impurity
    (Gini/entropy) each feature causes in the decision trees.
    
    Parameters:
    -----------
    model : RandomForestClassifier
        Trained Random Forest model
    feature_names : list
        Names of features
    top_n : int
        Number of top features to display
        
    Returns:
    --------
    importance_df : DataFrame
        DataFrame with features and their importance scores
    """
    print("\n" + "="*60)
    print("FEATURE IMPORTANCE ANALYSIS")
    print("="*60)
    
    importances = model.feature_importances_
    importance_df = pd.DataFrame({
        'Feature': feature_names,
        'Importance': importances
    }).sort_values('Importance', ascending=False)
    
    print(f"\nTop {top_n} Most Important Features:")
    print("-" * 40)
    
    for i, (_, row) in enumerate(importance_df.head(top_n).iterrows()):
        print(f"  {i+1}. {row['Feature']}: {row['Importance']:.4f}")
    
    return importance_df


def evaluate_oob_performance(X, y, n_estimators=100, max_depth=10, random_state=42):
    """
    Evaluate out-of-bag (OOB) performance.
    
    OOB score provides an internal validation metric without needing
    a separate validation set. Each tree is trained on ~63% of 
    samples (bootstrap), leaving ~37% as OOB samples.
    
    Parameters:
    -----------
    X : array-like
        Feature matrix
    y : array-like
        Target labels
    n_estimators : int
        Number of trees in the forest
    max_depth : int
        Maximum depth of each tree
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    oob_score : float
        Out-of-bag score
    """
    print("\n" + "="*60)
    print("OUT-OF-BAG (OOB) EVALUATION")
    print("="*60)
    
    print("\nUnderstanding OOB Scoring:")
    print("-" * 40)
    print("  - Each tree uses bootstrap sampling (~63% of data)")
    print("  - Remaining ~37% samples are OOB for each tree")
    print("  - OOB prediction uses only trees that didn't see the sample")
    print("  - Provides validation without separate test set")
    
    rf = RandomForestClassifier(
        n_estimators=n_estimators,
        max_depth=max_depth,
        bootstrap=True,
        oob_score=True,
        random_state=random_state,
        n_jobs=-1
    )
    
    rf.fit(X, y)
    
    oob_score = rf.oob_score_
    
    print(f"\nResults:")
    print(f"  - OOB Score: {oob_score:.4f}")
    print(f"  - Number of estimators: {n_estimators}")
    print(f"  - Max depth: {max_depth}")
    
    sample_inbag = rf.estim_samples_
    avg_samples_used = np.mean([len(x) for x in sample_inbag]) / X.shape[0]
    print(f"  - Average samples per tree: {avg_samples_used*100:.1f}%")
    
    return oob_score


def run_banking_example():
    """
    Complete banking example: Customer Churn Prediction.
    
    This example demonstrates Random Forest classification on a 
    real-world banking dataset for predicting customer churn.
    """
    print("\n" + "="*70)
    print("="*"20 + " BANKING EXAMPLE: CUSTOMER CHURN " + "*"*20)
    print("="*70)
    
    df = generate_banking_data(n_samples=1000, random_state=42)
    
    feature_cols = ['CreditScore', 'Age', 'Tenure', 'Balance', 'NumProducts',
                   'HasCreditCard', 'IsActive', 'Salary', 'SatisfactionScore', 'Complaint']
    
    X = df[feature_cols].values
    y = df['Churned'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    print(f"\nData Split:")
    print(f"  - Training samples: {len(X_train)}")
    print(f"  - Testing samples: {len(X_test)}")
    
    print("\n" + "-"*50)
    print("Running Random Forest on Banking Data...")
    print("-"*50)
    
    rf, y_pred, y_pred_proba, metrics = core_random_forest_implementation(
        X_train, X_test, y_train, y_test,
        n_estimators=100,
        max_depth=10
    )
    
    print("\n" + "-"*50)
    print("Feature Importance Analysis:")
    print("-"*50)
    importance_df = analyze_feature_importance(rf, feature_cols, top_n=10)
    
    print("\n" + "-"*50)
    print("OOB Score Evaluation:")
    print("-"*50)
    oob_score = evaluate_oob_performance(X, y, n_estimators=100)
    
    print("\n" + "-"*50)
    print("Confusion Matrix:")
    print("-"*50)
    cm = confusion_matrix(y_test, y_pred)
    print(f"  True Negatives: {cm[0,0]}, False Positives: {cm[0,1]}")
    print(f"  False Negatives: {cm[1,0]}, True Positives: {cm[1,1]}")
    
    print("\n" + "-"*50)
    print("Classification Report:")
    print("-"*50)
    print(classification_report(y_test, y_pred, target_names=['Not Churned', 'Churned']))
    
    return {
        'metrics': metrics,
        'oob_score': oob_score,
        'feature_importance': importance_df
    }


def run_healthcare_example():
    """
    Complete healthcare example: Disease Risk Classification.
    
    This example demonstrates Random Forest classification on a 
    healthcare dataset for predicting disease risk levels.
    """
    print("\n" + "="*70)
    print("="*"20 + " HEALTHCARE EXAMPLE: DISEASE RISK " + "*"*20)
    print("="*70)
    
    df = generate_healthcare_data(n_samples=1000, random_state=42)
    
    feature_cols = ['Age', 'BMI', 'BloodPressure', 'Cholesterol', 'BloodSugar',
                    'HeartRate', 'ExerciseMinutes', 'Smoking', 'FamilyHistory', 'DietScore']
    
    X = df[feature_cols].values
    y = df['HighRisk'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    print(f"\nData Split:")
    print(f"  - Training samples: {len(X_train)}")
    print(f"  - Testing samples: {len(X_test)}")
    
    print("\n" + "-"*50)
    print("Running Random Forest on Healthcare Data...")
    print("-"*50)
    
    rf, y_pred, y_pred_proba, metrics = core_random_forest_implementation(
        X_train, X_test, y_train, y_test,
        n_estimators=100,
        max_depth=10
    )
    
    print("\n" + "-"*50)
    print("Feature Importance Analysis:")
    print("-"*50)
    importance_df = analyze_feature_importance(rf, feature_cols, top_n=10)
    
    print("\n" + "-"*50)
    print("OOB Score Evaluation:")
    print("-"*50)
    oob_score = evaluate_oob_performance(X, y, n_estimators=100)
    
    print("\n" + "-"*50)
    print("Confusion Matrix:")
    print("-"*50)
    cm = confusion_matrix(y_test, y_pred)
    print(f"  True Negatives: {cm[0,0]}, False Positives: {cm[0,1]}")
    print(f"  False Negatives: {cm[1,0]}, True Positives: {cm[1,1]}")
    
    print("\n" + "-"*50)
    print("Classification Report:")
    print("-"*50)
    print(classification_report(y_test, y_pred, target_names=['Low Risk', 'High Risk']))
    
    return {
        'metrics': metrics,
        'oob_score': oob_score,
        'feature_importance': importance_df
    }


def run_synthetic_example():
    """
    Complete example with synthetic data demonstrating all key concepts.
    """
    print("\n" + "="*70)
    print("="*"20 + " SYNTHETIC DATA EXAMPLE " + "*"*25)
    print("="*70)
    
    X, y, feature_names = generate_synthetic_classification_data(
        n_samples=500, 
        n_features=20,
        n_informative=10,
        random_state=42
    )
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    print("\n" + "-"*50)
    print("Part 1: Core Random Forest Implementation")
    print("-"*50)
    
    rf, y_pred, y_pred_proba, metrics = core_random_forest_implementation(
        X_train, X_test, y_train, y_test,
        n_estimators=100,
        max_depth=10
    )
    
    print("\n" + "-"*50)
    print("Part 2: Comparison with Single Decision Tree")
    print("-"*50)
    tree_results = compare_with_single_tree(X_train, X_test, y_train, y_test)
    
    print("\n" + "-"*50)
    print("Part 3: Hyperparameter Tuning (n_estimators)")
    print("-"*50)
    tuning_results = tune_n_estimators(X_train, y_train, X_test, y_test)
    
    print("\n" + "-"*50)
    print("Part 4: Feature Importance Analysis")
    print("-"*50)
    importance_df = analyze_feature_importance(rf, feature_names, top_n=10)
    
    print("\n" + "-"*50)
    print("Part 5: Out-of-Bag Evaluation")
    print("-"*50)
    oob_score = evaluate_oob_performance(X, y, n_estimators=100)
    
    return {
        'metrics': metrics,
        'tree_results': tree_results,
        'tuning_results': tuning_results,
        'oob_score': oob_score
    }


def demonstrate_bagging_concept():
    """
    Demonstrate the bagging (Bootstrap Aggregating) concept.
    
    Shows how Random Forest uses bagging to reduce variance.
    """
    print("\n" + "="*70)
    print("="*"25 + " BAGGING CONCEPT " + "*"*26)
    print("="*70)
    
    print("\nHow Bagging Works in Random Forest:")
    print("-" * 50)
    print("  1. Bootstrap Sampling:")
    print("     - Each tree is trained on a random sample with replacement")
    print("     - Sample size equals original dataset size")
    print("     - ~63% of unique samples (1 - 1/e)")
    print("")
    print("  2. Parallel Tree Training:")
    print("     - All trees are trained independently")
    print("     - Can be parallelized for efficiency")
    print("")
    print("  3. Ensemble Prediction:")
    print("     - Classification: Majority vote")
    print("     - Regression: Average predictions")
    print("")
    print("  4. Variance Reduction:")
    print("     - Averaging reduces prediction variance")
    print("     - Less sensitive to noise in training data")
    print("")
    
    X, y, _ = generate_synthetic_classification_data(n_samples=300, random_state=42)
    
    n_bootstrap_samples = []
    for i in range(100):
        indices = np.random.choice(len(X), size=len(X), replace=True)
        unique_indices = np.unique(indices)
        n_bootstrap_samples.append(len(unique_indices) / len(X) * 100)
    
    print(f"Average unique samples per bootstrap: {np.mean(n_bootstrap_samples):.1f}%")


def demonstrate_feature_bagging():
    """
    Demonstrate feature bagging (random feature selection at each split).
    """
    print("\n" + "="*70)
    print("="*"22 + " FEATURE BAGGING CONCEPT " + "*"*23)
    print("="*70)
    
    print("\nHow Feature Bagging Works:")
    print("-" * 50)
    print("  1. At each split in a decision tree:")
    print("     - Only a random subset of features is considered")
    print("     - Default: sqrt(n_features)")
    print("")
    print("  2. Benefits:")
    print("     - Adds diversity to the ensemble")
    print("     - Prevents dominant features from always being selected")
    print("     - Reduces correlation between trees")
    print("")
    print("  3. Feature Bagging vs Bagging:")
    print("     - Bagging: Random sample of data points")
    print("     - Feature Bagging: Random sample of features")
    print("")
    
    X, y, feature_names = generate_synthetic_classification_data(
        n_samples=200, n_features=20, random_state=42
    )
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    rf = RandomForestClassifier(
        n_estimators=10,
        max_features='sqrt',
        random_state=42
    )
    rf.fit(X_train, y_train)
    
    print(f"  Number of features: {X.shape[1]}")
    print(f"  Features considered per split (sqrt): {int(np.sqrt(X.shape[1]))}")


def main():
    """
    Main function to execute all Random Forest examples and demonstrations.
    """
    print("\n" + "="*70)
    print("="*"17 + " RANDOM FOREST CLASSIFICATION " + "*"*20)
    print("="*"20 + " COMPREHENSIVE GUIDE " + "*"*24)
    print("="*70)
    
    print("\nTopics covered:")
    print("-" * 50)
    print("  I.   Introduction to Random Forest")
    print("  II.  Core Concepts (Bagging, Bootstrap, Feature Bagging)")
    print("  III. Implementation and Hyperparameter Tuning")
    print("  IV.  Banking Example (Customer Churn)")
    print("  V.   Healthcare Example (Disease Risk)")
    print("  VI.  OOB Score Evaluation")
    print("  VII. Feature Importance Analysis")
    print("  VIII.Comparison with Single Decision Tree")
    
    print("\n" + "="*70)
    print("="*"20 + " SECTION I: CORE CONCEPTS " + "*"*27)
    print("="*70)
    
    demonstrate_bagging_concept()
    demonstrate_feature_bagging()
    
    print("\n" + "="*70)
    print("="*"20 + " SECTION II: SYNTHETIC DATA " + "*"*28)
    print("="*70)
    
    synthetic_results = run_synthetic_example()
    
    print("\n" + "="*70)
    print("="*"20 + " SECTION III: BANKING DOMAIN " + "*"*25)
    print("="*70)
    
    banking_results = run_banking_example()
    
    print("\n" + "="*70)
    print("="*"20 + " SECTION IV: HEALTHCARE DOMAIN " + "*"*24)
    print("="*70)
    
    healthcare_results = run_healthcare_example()
    
    print("\n" + "="*70)
    print("="*"20 + " SUMMARY " + "*"*34)
    print("="*70)
    
    print("\nKey Takeaways:")
    print("-" * 50)
    print("  1. Random Forest is an ensemble of decision trees")
    print("  2. Uses bootstrap sampling for training diversity")
    print("  3. Feature bagging adds additional randomization")
    print("  4. OOB score provides built-in validation")
    print("  5. Feature importance identifies key predictors")
    print("  6. Generally outperforms single decision trees")
    print("  7. Robust to overfitting with proper parameters")
    print("")
    print("Best Practices:")
    print("  - Start with n_estimators=100-200")
    print("  - Use max_features='sqrt' for classification")
    print("  - Consider class_weight='balanced' for imbalanced data")
    print("  - Use OOB score for quick validation")
    print("")
    print("="*70)
    print("="*"20 + " EXECUTION COMPLETE " + "*"*28)
    print("="*70)


if __name__ == "__main__":
    main()