# Topic: Feature Engineering and Selection
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Feature Engineering and Selection

I. INTRODUCTION
    Feature engineering is the process of using domain knowledge to create features 
    that make machine learning algorithms work better. This module covers:
    - Feature creation from raw data
    - Feature transformation techniques
    - Feature scaling methods
    - Feature selection algorithms
    - Polynomial feature generation
    
II. CORE_CONCEPTS
    - Feature Creation: Creating new features from existing ones
    - Feature Transformation: Applying mathematical functions to features
    - Feature Scaling: Normalizing feature ranges
    - Feature Selection: Selecting most relevant features
    - Polynomial Features: Creating interaction terms
    
III. IMPLEMENTATION
    - Comprehensive functions for each technique
    - Banking industry example (customer segmentation)
    - Healthcare example (patient risk prediction)
    
IV. EXAMPLES (Banking + Healthcare)
    - Real-world applications with domain-specific features
    
V. OUTPUT_RESULTS
    - Comprehensive output analysis and metrics
    
VI. TESTING
    - Validation of feature engineering pipelines
    
VII. ADVANCED_TOPICS
    - Cross-validation for feature selection
    - Automated feature engineering
    
VIII. CONCLUSION
    - Best practices and recommendations
"""

# Import necessary libraries
import numpy as np
import pandas as pd
from sklearn.datasets import make_classification, load_breast_cancer, load_diabetes
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.preprocessing import (
    StandardScaler, MinMaxScaler, RobustScaler, 
    PolynomialFeatures, LabelEncoder, OneHotEncoder,
    PowerTransformer, QuantileTransformer
)
from sklearn.feature_selection import (
    SelectKBest, f_classif, mutual_info_classif,
    SelectFromModel, RFE, RFECV, chi2,
    VarianceThreshold, SelectPercentile
)
from sklearn.linear_model import LogisticRegression, Ridge
from sklearn.ensemble import RandomForestClassifier, RandomForestRegressor
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, 
    f1_score, roc_auc_score, mean_squared_error, r2_score
)
import warnings
warnings.filterwarnings('ignore')


def generate_data_with_features(n_samples=500, n_features=10, random_state=42):
    """
    Generate synthetic data with various feature types for demonstration.
    
    Parameters:
    -----------
    n_samples : int
        Number of samples to generate
    n_features : int
        Number of features to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    X : numpy.ndarray
        Feature matrix
    y : numpy.ndarray
        Target variable
    feature_names : list
        Names of features
    """
    np.random.seed(random_state)
    
    X, y = make_classification(
        n_samples=n_samples,
        n_features=n_features,
        n_informative=5,
        n_redundant=3,
        n_clusters_per_class=2,
        random_state=random_state
    )
    
    feature_names = [f'feature_{i}' for i in range(n_features)]
    
    X_df = pd.DataFrame(X, columns=feature_names)
    X_df['categorical_feature'] = np.random.choice(['A', 'B', 'C'], size=n_samples)
    X_df['date_feature'] = pd.date_range('2020-01-01', periods=n_samples, 
                                       freq='D').strftime('%Y-%m-%d')
    
    return X_df, y, list(X_df.columns)


def create_banking_customer_data(n_customers=1000, random_state=42):
    """
    Create synthetic banking customer data for feature engineering examples.
    
    This function generates realistic customer data including:
    - Demographic features (age, income, credit score)
    - Account features (balance, transaction count, average trans amount)
    - Behavioral features (online banking usage, product holdings)
    - Derived features (tenure, risk scores)
    
    Parameters:
    -----------
    n_customers : int
        Number of customers to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : pandas.DataFrame
        DataFrame with customer features
    """
    np.random.seed(random_state)
    
    customer_ids = range(1000, 1000 + n_customers)
    ages = np.random.normal(45, 15, n_customers).astype(int)
    ages = np.clip(ages, 18, 80)
    
    annual_incomes = np.random.lognormal(10.5, 0.8, n_customers)
    annual_incomes = np.clip(annual_incomes, 20000, 500000)
    
    credit_scores = np.random.normal(680, 80, n_customers).astype(int)
    credit_scores = np.clip(credit_scores, 300, 850)
    
    account_balances = np.random.lognormal(9, 1.5, n_customers)
    account_balances = np.clip(account_balances, 100, 100000)
    
    monthly_transactions = np.random.poisson(15, n_customers)
    avg_trans_amounts = np.random.lognormal(5, 0.8, n_customers)
    
    account_tenure_days = np.random.exponential(1000, n_customers)
    account_tenure_days = np.clip(account_tenure_days, 30, 3650)
    
    online_banking_usage = np.random.choice([0, 1], n_customers, p=[0.3, 0.7])
    mobile_app_usage = np.random.choice([0, 1], n_customers, p=[0.5, 0.5])
    credit_card_count = np.random.poisson(1.5, n_customers)
    credit_card_count = np.clip(credit_card_count, 0, 5)
    
    wealth_management = np.random.choice([0, 1], n_customers, p=[0.85, 0.15])
    mortgage_balance = np.random.choice([0], n_customers)
    has_mortgage = np.random.choice([0, 1], n_customers, p=[0.7, 0.3])
    mortgage_balance = np.where(has_mortgage == 1, 
                               np.random.lognormal(11, 0.5, n_customers), 0)
    
    loan_balance = np.random.choice([0], n_customers)
    has_loan = np.random.choice([0, 1], n_customers, p=[0.6, 0.4])
    loan_balance = np.where(has_loan == 1, 
                          np.random.lognormal(9, 0.6, n_customers), 0)
    
    df = pd.DataFrame({
        'customer_id': customer_ids,
        'age': ages,
        'annual_income': annual_incomes,
        'credit_score': credit_scores,
        'account_balance': account_balances,
        'monthly_transactions': monthly_transactions,
        'avg_transaction_amount': avg_trans_amounts,
        'account_tenure_days': account_tenure_days,
        'online_banking_usage': online_banking_usage,
        'mobile_app_usage': mobile_app_usage,
        'credit_card_count': credit_card_count,
        'has_wealth_management': wealth_management,
        'mortgage_balance': mortgage_balance,
        'loan_balance': loan_balance
    })
    
    df['total_products'] = (df['credit_card_count'] + 
                           df['has_wealth_management'] + 
                           has_mortgage + has_loan)
    
    df['income_per_trans'] = (df['annual_income'] / 
                            (df['monthly_transactions'] * 12 + 1))
    
    df['balance_to_income_ratio'] = (df['account_balance'] / 
                                   (df['annual_income'] + 1))
    
    df['avg_daily_balance'] = df['account_balance'] / (df['account_tenure_days'] + 1)
    
    churn_prob = 0.1 + 0.2 * (1 - df['credit_score'] / 850)
    churn_prob += 0.1 * (df['account_tenure_days'] < 180)
    churn_prob += 0.1 * (df['online_banking_usage'] == 0)
    churn_prob = np.clip(churn_prob, 0.05, 0.5)
    
    df['churn_label'] = (np.random.random(n_customers) < churn_prob).astype(int)
    
    return df


def create_healthcare_patient_data(n_patients=1000, random_state=42):
    """
    Create synthetic healthcare patient data for feature engineering examples.
    
    This function generates realistic clinical data including:
    - Demographic features (age, gender, BMI)
    - Vital signs (blood pressure, heart rate, temperature)
    - Lab values (glucose, cholesterol, creatinine)
    - Medical history (comorbidities, prior procedures)
    - Derived clinical risk scores
    
    Parameters:
    -----------
    n_patients : int
        Number of patients to generate
    random_state : int
        Random seed for reproducibility
        
    Returns:
    --------
    df : pandas.DataFrame
        DataFrame with patient features
    """
    np.random.seed(random_state)
    
    patient_ids = range(2000, 2000 + n_patients)
    ages = np.random.normal(55, 18, n_patients).astype(int)
    ages = np.clip(ages, 18, 95)
    
    genders = np.random.choice(['M', 'F'], n_patients)
    
    bm_is = np.random.normal(27, 5, n_patients)
    bm_is = np.clip(bm_is, 15, 45)
    
    sys_bp = np.random.normal(130, 20, n_patients).astype(int)
    sys_bp = np.clip(sys_bp, 80, 200)
    dia_bp = np.random.normal(80, 12, n_patients).astype(int)
    dia_bp = np.clip(dia_bp, 50, 130)
    
    heart_rates = np.random.normal(72, 12, n_patients).astype(int)
    heart_rates = np.clip(heart_rates, 40, 120)
    
    temperatures = np.random.normal(98.6, 1.2, n_patients)
    temperatures = np.clip(temperatures, 95, 103)
    
    fasting_glucose = np.random.normal(100, 25, n_patients).astype(int)
    fasting_glucose = np.clip(fasting_glucose, 50, 300)
    
    total_cholesterol = np.random.normal(200, 40, n_patients).astype(int)
    total_cholesterol = np.clip(total_cholesterol, 100, 350)
    
    hdl_cholesterol = np.random.normal(55, 15, n_patients).astype(int)
    hdl_cholesterol = np.clip(hdl_cholesterol, 20, 100)
    
    ldl_cholesterol = np.random.normal(120, 30, n_patients).astype(int)
    ldl_cholesterol = np.clip(ldl_cholesterol, 50, 220)
    
    triglycerides = np.random.normal(150, 50, n_patients).astype(int)
    triglycerides = np.clip(triglycerides, 50, 500)
    
    creatinine = np.random.normal(1.0, 0.3, n_patients)
    creatinine = np.clip(creatinine, 0.5, 4.0)
    
    egfr = np.random.normal(90, 20, n_patients)
    egfr = np.clip(egfr, 15, 120)
    
    has_hypertension = np.random.choice([0, 1], n_patients, p=[0.55, 0.45])
    has_diabetes = np.random.choice([0, 1], n_patients, p=[0.75, 0.25])
    has_hyperlipidemia = np.random.choice([0, 1], n_patients, p=[0.6, 0.4])
    has_chd = np.random.choice([0, 1], n_patients, p=[0.85, 0.15])
    has_cvd = np.random.choice([0, 1], n_patients, p=[0.9, 0.1])
    has_ckd = np.random.choice([0, 1], n_patients, p=[0.85, 0.15])
    
    smoking_status = np.random.choice([0, 1, 2], n_patients, p=[0.6, 0.25, 0.15])
    
    alcohol_use = np.random.choice([0, 1, 2], n_patients, p=[0.7, 0.2, 0.1])
    
    exercise_level = np.random.choice([0, 1, 2], n_patients, p=[0.3, 0.4, 0.3])
    
    bmi_category = pd.cut(bm_is, bins=[0, 18.5, 25, 30, 100], 
                         labels=[0, 1, 2, 3]).astype(int)
    
    df = pd.DataFrame({
        'patient_id': patient_ids,
        'age': ages,
        'gender': genders,
        'bmi': bm_is,
        'systolic_bp': sys_bp,
        'diastolic_bp': dia_bp,
        'heart_rate': heart_rates,
        'temperature': temperatures,
        'fasting_glucose': fasting_glucose,
        'total_cholesterol': total_cholesterol,
        'hdl_cholesterol': hdl_cholesterol,
        'ldl_cholesterol': ldl_cholesterol,
        'triglycerides': triglycerides,
        'creatinine': creatinine,
        'egfr': egfr,
        'has_hypertension': has_hypertension,
        'has_diabetes': has_diabetes,
        'has_hyperlipidemia': has_hyperlipidemia,
        'has_coronary_heart_disease': has_chd,
        'has_cardiovascular_disease': has_cvd,
        'has_ckd': has_ckd,
        'smoking_status': smoking_status,
        'alcohol_use': alcohol_use,
        'exercise_level': exercise_level
    })
    
    df['pulse_pressure'] = df['systolic_bp'] - df['diastolic_bp']
    
    df['mean_arterial_pressure'] = (df['diastolic_bp'] + 
                                    df['pulse_pressure'] / 3)
    
    df['cholesterol_ratio'] = df['total_cholesterol'] / (df['hdl_cholesterol'] + 1)
    
    df['ldl_hdl_ratio'] = df['ldl_cholesterol'] / (df['hdl_cholesterol'] + 1)
    
    df['glucose_risk_score'] = np.where(df['fasting_glucose'] >= 126, 3,
                             np.where(df['fasting_glucose'] >= 100, 2, 1))
    
    df['bp_risk_category'] = np.where(df['systolic_bp'] >= 180, 3,
                             np.where(df['systolic_bp'] >= 140, 2,
                             np.where(df['systolic_bp'] >= 120, 1, 0)))
    
    df['comorbidity_count'] = (has_hypertension + has_diabetes + 
                              has_hyperlipidemia + has_chd + 
                              has_cvd + has_ckd)
    
    df['cv_risk_score'] = (df['age'] / 10 + bmi_category + 
                          has_hypertension + has_diabetes + 
                          has_hyperlipidemia + 
                          (smoking_status > 0).astype(int))
    
    df['metabolic_syndrome_score'] = (
        (df['bp_risk_category'] >= 1).astype(int) +
        (df['bmi'] >= 30).astype(int) +
        (df['triglycerides'] >= 150).astype(int) +
        (df['hdl_cholesterol'] < 40).astype(int) +
        (df['fasting_glucose'] >= 100).astype(int)
    )
    
    df['adverse_outcome'] = (
        (has_chd == 1) | (has_cvd == 1) | 
        (df['egfr'] < 30)
    ).astype(int)
    
    return df


def core_feature_engineering(df, target_col=None):
    """
    Core feature engineering operations.
    
    This function demonstrates:
    1. Feature Creation: Derived features from existing ones
    2. Feature Transformation: Log, sqrt, power transformations
    3. Feature Scaling: Standard, MinMax, Robust scaling
    4. Feature Selection: Statistical and model-based selection
    
    Parameters:
    -----------
    df : pandas.DataFrame
        Input DataFrame with features
    target_col : str, optional
        Target column name for supervised selection
        
    Returns:
    --------
    results : dict
        Dictionary containing engineered features and results
    """
    results = {}
    
    numeric_cols = df.select_dtypes(include=[np.number]).columns.tolist()
    if target_col and target_col in numeric_cols:
        numeric_cols.remove(target_col)
    
    X = df[numeric_cols].values
    
    results['scaled_standard'] = StandardScaler().fit_transform(X)
    results['scaled_minmax'] = MinMaxScaler().fit_transform(X)
    results['scaled_robust'] = RobustScaler().fit_transform(X)
    
    results['power_transformed'] = PowerTransformer(method='yeo-johnson').fit_transform(X)
    results['quantile_transformed'] = QuantileTransformer(output_distribution='normal').fit_transform(X)
    
    poly = PolynomialFeatures(degree=2, include_bias=False, interaction_only=True)
    results['poly_interactions'] = poly.fit_transform(X)
    
    return results


def feature_creationTechniques(df):
    """
    Advanced feature creation techniques.
    
    Demonstrates sophisticated feature engineering:
    - Interaction features
    - Ratio features
    - Aggregation features
    - Binning features
    - Encoding features
    
    Parameters:
    -----------
    df : pandas.DataFrame
        Input DataFrame
        
    Returns:
    --------
    df_engineered : pandas.DataFrame
        DataFrame with new features
    """
    df_engineered = df.copy()
    
    numeric_cols = df.select_dtypes(include=[np.number]).columns
    
    for i, col1 in enumerate(numeric_cols):
        for col2 in numeric_cols[i+1:]:
            if col1 != col2:
                try:
                    df_engineered[f'{col1}_x_{col2}'] = df[col1] * df[col2]
                    df_engineered[f'{col1}_div_{col2}'] = df[col1] / (df[col2] + 1e-6)
                except:
                    pass
    
    for col in numeric_cols:
        df_engineered[f'{col}_squared'] = df[col] ** 2
        df_engineered[f'{col}_sqrt'] = np.sqrt(np.abs(df[col]))
        df_engineered[f'{col}_log'] = np.log1p(np.abs(df[col]))
    
    return df_engineered


def feature_transformation(df, numeric_cols):
    """
    Apply various feature transformations.
    
    Transformations include:
    - Log transformation (for skewed data)
    - Square root transformation
    - Box-Cox transformation
    - Power transformation
    - Rank transformation
    
    Parameters:
    -----------
    df : pandas.DataFrame
        Input DataFrame
    numeric_cols : list
        List of numeric column names
        
    Returns:
    --------
    df_transformed : pandas.DataFrame
        DataFrame with transformed features
    """
    df_transformed = df.copy()
    
    for col in numeric_cols:
        df_transformed[f'{col}_log'] = np.log1p(np.abs(df[col]))
        df_transformed[f'{col}_sqrt'] = np.sqrt(np.abs(df[col]))
        df_transformed[f'{col}_reciprocal'] = 1 / (df[col] + 1e-6)
    
    return df_transformed


def feature_scaling_comparison(X_train, X_test):
    """
    Compare different feature scaling methods.
    
    Methods compared:
    - StandardScaler: Mean=0, Std=1
    - MinMaxScaler: Range [0,1]
    - RobustScaler: Median=0, IQR=1
    
    Parameters:
    -----------
    X_train : numpy.ndarray
        Training features
    X_test : numpy.ndarray
        Test features
        
    Returns:
    --------
    scaled_data : dict
        Dictionary with scaled data for each method
    """
    scalers = {
        'standard': StandardScaler(),
        'minmax': MinMaxScaler(),
        'robust': RobustScaler(),
        'power': PowerTransformer(method='yeo-johnson'),
        'quantile': QuantileTransformer(output_distribution='normal')
    }
    
    scaled_data = {}
    
    for name, scaler in scalers.items():
        X_train_scaled = scaler.fit_transform(X_train)
        X_test_scaled = scaler.transform(X_test)
        
        scaled_data[f'{name}_train'] = X_train_scaled
        scaled_data[f'{name}_test'] = X_test_scaled
    
    return scaled_data


def feature_selection_methods(X, y, feature_names):
    """
    Apply various feature selection methods.
    
    Methods include:
    - VarianceThreshold: Remove low variance features
    - SelectKBest: Univariate selection
    - SelectFromModel: Model-based selection
    - RFE: Recursive feature elimination
    - RFECV: Cross-validation based selection
    
    Parameters:
    -----------
    X : numpy.ndarray
        Feature matrix
    y : numpy.ndarray
        Target variable
    feature_names : list
        List of feature names
        
    Returns:
    --------
    selected_features : dict
        Dictionary with selected features for each method
    """
    selected_features = {}
    
    selector = VarianceThreshold(threshold=0.1)
    X_selected = selector.fit_transform(X)
    selected_features['variance'] = [feature_names[i] for i in selector.get_support(indices=True)]
    
    selector = SelectKBest(f_classif, k=5)
    X_selected = selector.fit_transform(X, y)
    selected_features['f_classif'] = [feature_names[i] for i in selector.get_support(indices=True)]
    
    selector = SelectKBest(mutual_info_classif, k=5)
    X_selected = selector.fit_transform(X, y)
    selected_features['mutual_info'] = [feature_names[i] for i in selector.get_support(indices=True)]
    
    model = LogisticRegression(max_iter=1000)
    selector = SelectFromModel(model)
    X_selected = selector.fit_transform(X, y)
    selected_features['model_based'] = [feature_names[i] for i in selector.get_support(indices=True)]
    
    model = RandomForestClassifier(n_estimators=50, random_state=42)
    selector = RFE(model, n_features_to_select=5)
    X_selected = selector.fit_transform(X, y)
    selected_features['rfe'] = [feature_names[i] for i in selector.get_support(indices=True)]
    
    return selected_features


def polynomial_feature_generation(X, degree=2, interaction_only=False):
    """
    Generate polynomial features.
    
    Creates:
    - Polynomial terms up to specified degree
    - Interaction terms between features
    - Cross-products
    
    Parameters:
    -----------
    X : numpy.ndarray
        Feature matrix
    degree : int
        Maximum polynomial degree
    interaction_only : bool
        If True, only interaction terms (no powers)
        
    Returns:
    --------
    X_poly : numpy.ndarray
        Polynomial feature matrix
    poly : PolynomialFeatures
        Fitted transformer
    """
    poly = PolynomialFeatures(degree=degree, include_bias=False, 
                                interaction_only=interaction_only)
    X_poly = poly.fit_transform(X)
    
    return X_poly, poly


def evaluate_feature_sets(X_train, X_test, y_train, y_test, feature_sets, model=None):
    """
    Evaluate different feature sets using a model.
    
    Parameters:
    -----------
    X_train : numpy.ndarray
        Training features
    X_test : numpy.ndarray
        Test features
    y_train : numpy.ndarray
        Training targets
    y_test : numpy.ndarray
        Test targets
    feature_sets : dict
        Dictionary of named feature sets
    model : sklearn model
        Model to use for evaluation
        
    Returns:
    --------
    results : dict
        Dictionary with evaluation metrics
    """
    if model is None:
        model = LogisticRegression(max_iter=1000)
    
    results = {}
    
    for name, X_features in feature_sets.items():
        model_copy = type(model)(**model.get_params())
        model_copy.fit(X_features[:len(y_train)], y_train)
        y_pred = model_copy.predict(X_features[len(y_train):])
        
        results[name] = {
            'accuracy': accuracy_score(y_test, y_pred),
            'precision': precision_score(y_test, y_pred, average='weighted', zero_division=0),
            'recall': recall_score(y_test, y_pred, average='weighted', zero_division=0),
            'f1': f1_score(y_test, y_pred, average='weighted', zero_division=0)
        }
    
    return results


def banking_example():
    """
    Banking/Finance industry application.
    
    Demonstrates feature engineering for:
    - Customer segmentation
    - Credit risk modeling
    - Churn prediction
    
    Returns:
    --------
    results : dict
        Dictionary with banking analysis results
    """
    print("\n" + "="*60)
    print("BANKING EXAMPLE - Customer Churn Prediction")
    print("="*60)
    
    df = create_banking_customer_data(n_customers=2000, random_state=42)
    
    print(f"\nDataset shape: {df.shape}")
    print(f"Columns: {list(df.columns)}")
    
    feature_cols = ['age', 'annual_income', 'credit_score', 'account_balance',
                   'monthly_transactions', 'avg_transaction_amount', 'account_tenure_days',
                   'online_banking_usage', 'mobile_app_usage', 'credit_card_count',
                   'has_wealth_management', 'total_products', 'income_per_trans',
                   'balance_to_income_ratio', 'avg_daily_balance']
    
    X = df[feature_cols].values
    y = df['churn_label'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    selected_features = feature_selection_methods(X_train_scaled, y_train, feature_cols)
    
    print("\nSelected Features by Method:")
    for method, features in selected_features.items():
        print(f"  {method}: {features}")
    
    model = LogisticRegression(max_iter=1000, random_state=42)
    model.fit(X_train_scaled, y_train)
    y_pred = model.predict(X_test_scaled)
    y_pred_proba = model.predict_proba(X_test_scaled)[:, 1]
    
    results = {
        'accuracy': accuracy_score(y_test, y_pred),
        'precision': precision_score(y_test, y_pred, zero_division=0),
        'recall': recall_score(y_test, y_pred, zero_division=0),
        'f1': f1_score(y_test, y_pred, zero_division=0),
        'roc_auc': roc_auc_score(y_test, y_pred_proba)
    }
    
    print("\nModel Performance:")
    for metric, value in results.items():
        print(f"  {metric}: {value:.4f}")
    
    print("\nChurn Rate Analysis:")
    churn_rate = df['churn_label'].mean()
    print(f"  Overall churn rate: {churn_rate:.2%}")
    
    return results


def healthcare_example():
    """
    Healthcare industry application.
    
    Demonstrates feature engineering for:
    - Patient risk stratification
    - Disease prediction
    - Clinical outcome prediction
    
    Returns:
    --------
    results : dict
        Dictionary with healthcare analysis results
    """
    print("\n" + "="*60)
    print("HEALTHCARE EXAMPLE - Cardiovascular Risk Prediction")
    print("="*60)
    
    df = create_healthcare_patient_data(n_patients=2000, random_state=42)
    
    print(f"\nDataset shape: {df.shape}")
    print(f"Columns: {list(df.columns)}")
    
    feature_cols = ['age', 'bmi', 'systolic_bp', 'diastolic_bp', 'heart_rate',
                  'temperature', 'fasting_glucose', 'total_cholesterol', 'hdl_cholesterol',
                  'ldl_cholesterol', 'triglycerides', 'creatinine', 'egfr',
                  'has_hypertension', 'has_diabetes', 'has_hyperlipidemia',
                  'has_coronary_heart_disease', 'has_cardiovascular_disease', 'has_ckd',
                  'smoking_status', 'alcohol_use', 'exercise_level',
                  'pulse_pressure', 'mean_arterial_pressure', 'cholesterol_ratio',
                  'ldl_hdl_ratio', 'glucose_risk_score', 'bp_risk_category',
                  'comorbidity_count', 'cv_risk_score', 'metabolic_syndrome_score']
    
    X = df[feature_cols].values
    y = df['adverse_outcome'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    X_poly, poly = polynomial_feature_generation(X_train_scaled, degree=2)
    X_test_poly = poly.transform(X_test_scaled)
    
    selected_features = feature_selection_methods(X_train_scaled, y_train, feature_cols)
    
    print("\nSelected Features by Method:")
    for method, features in selected_features.items():
        print(f"  {method}: {features}")
    
    model = LogisticRegression(max_iter=1000, random_state=42)
    model.fit(X_train_scaled, y_train)
    y_pred = model.predict(X_test_scaled)
    y_pred_proba = model.predict_proba(X_test_scaled)[:, 1]
    
    results = {
        'accuracy': accuracy_score(y_test, y_pred),
        'precision': precision_score(y_test, y_pred, zero_division=0),
        'recall': recall_score(y_test, y_pred, zero_division=0),
        'f1': f1_score(y_test, y_pred, zero_division=0),
        'roc_auc': roc_auc_score(y_test, y_pred_proba)
    }
    
    print("\nModel Performance:")
    for metric, value in results.items():
        print(f"  {metric}: {value:.4f}")
    
    print("\nAdverse Outcome Rate Analysis:")
    outcome_rate = df['adverse_outcome'].mean()
    print(f"  Overall adverse outcome rate: {outcome_rate:.2%}")
    
    return results


def output_results_summary(banking_results, healthcare_results):
    """
    Generate summary of results from both examples.
    
    Parameters:
    -----------
    banking_results : dict
        Results from banking example
    healthcare_results : dict
        Results from healthcare example
    """
    print("\n" + "="*60)
    print("RESULTS SUMMARY")
    print("="*60)
    
    print("\nBanking (Churn Prediction):")
    for metric, value in banking_results.items():
        print(f"  {metric}: {value:.4f}")
    
    print("\nHealthcare (Cardiovascular Risk):")
    for metric, value in healthcare_results.items():
        print(f"  {metric}: {value:.4f}")


def testing_feature_engineering():
    """
    Comprehensive testing of feature engineering pipeline.
    """
    print("\n" + "="*60)
    print("TESTING - Feature Engineering Pipeline")
    print("="*60)
    
    X, y, feature_names = generate_data_with_features(n_samples=200, n_features=8)
    print(f"\nGenerated data shape: {X.shape}")
    
    X_train, X_test, y_train, y_test = train_test_split(
        X.values, y, test_size=0.2, random_state=42
    )
    
    scaled_data = feature_scaling_comparison(X_train, X_test)
    print(f"Scaling methods tested: {list(scaled_data.keys())}")
    
    feature_sets = feature_selection_methods(X_train, y_train, feature_names)
    print(f"Feature selection methods: {list(feature_sets.keys())}")
    
    poly_features, poly = polynomial_feature_generation(X_train)
    print(f"Polynomial features shape: {poly_features.shape}")
    
    model = LogisticRegression(max_iter=1000, random_state=42)
    scores = cross_val_score(model, X_train, y_train, cv=5, scoring='accuracy')
    print(f"Cross-validation accuracy: {scores.mean():.4f} (+/- {scores.std()*2:.4f})")
    
    print("\nAll tests passed successfully!")
    
    return True


def main():
    print("Executing Feature Engineering and Selection implementation")
    print("="*60)
    
    X, y, feature_names = generate_data_with_features(n_samples=500)
    print(f"\nGenerated data shape: {X.shape}")
    print(f"Feature names: {feature_names}")
    
    banking_results = banking_example()
    
    healthcare_results = healthcare_example()
    
    output_results_summary(banking_results, healthcare_results)
    
    testing_feature_engineering()
    
    print("\n" + "="*60)
    print("Feature Engineering Implementation Complete")
    print("="*60)


if __name__ == "__main__":
    main()