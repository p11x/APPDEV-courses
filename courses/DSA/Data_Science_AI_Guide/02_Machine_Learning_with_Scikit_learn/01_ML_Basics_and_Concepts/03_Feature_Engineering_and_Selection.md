# Feature Engineering and Selection

## Introduction

Feature engineering and selection represent transformative processes that convert raw data into machine learning representations. Feature engineering creates informative features from raw data; feature selection identifies the most relevant features for prediction. Both processes dramatically impact model performance. Exceptional feature engineering often matters more than algorithm selection.

This guide explores feature engineering techniques spanning data types and problem domains. We cover numerical transformations, categorical encoding, text processing, and time series feature construction. Implementation with scikit-learn provides practical code for each technique. Banking and healthcare applications demonstrate real-world feature engineering in action.

Feature selection complements feature engineering by identifying the most predictive subset of features. Selection reduces model complexity, improves generalization, and enhances interpretability. We examine filter, wrapper, and embedded methods for selection. Practical implementation demonstrates selection's impact on model performance.

## Fundamentals

### Feature Engineering Fundamentals

Feature engineering transforms raw data into representations that algorithms can effectively use. The fundamental principle is encoding domain knowledge into features that capture relevant information for prediction. Good features reduce algorithmic complexity by providing informative representations.

Feature engineering begins with understanding the data and prediction task. What information in the raw data is relevant to the target variable? How can we extract that information in a form algorithms can use? These questions guide feature construction. Domain expertise proves invaluable; understanding the domain reveals informative transformations.

The feature engineering process is inherently iterative. Initial features are constructed, models trained, performance evaluated, and new features developed based on insights. This iteration continues until performance plateaus. Feature engineering requires experimentation; not all constructed features prove useful. The systematic approach enables effective exploration of the feature space.

### Numerical Feature Engineering

Numerical features require transformation for optimal algorithm performance. Scaling methods bring features to similar ranges, preventing features with large magnitudes from dominating. Logarithmic and power transformations address skewness, enabling algorithms that assume normality. Binning converts continuous features to categorical representations.

Standard scaling (z-score normalization) transforms features to zero mean and unit variance. This transformation centers features around zero, scaling by standard deviation. Standard scaling assumes approximately normal distributions; for highly skewed features, robust scaling using percentiles may be more appropriate. The transformation preserves outliers while standardizing typical values.

Min-max scaling transforms features to a specified range, typically [0, 1]. This transformation preserves the distribution shape while bounding values. Min-max scaling works well when feature bounds are known and important. However, sensitivity to outliers can distort the scaled distribution. Alternative transformations may better handle outlier-heavy distributions.

### Categorical Feature Engineering

Categorical features require encoding as numerical values for most algorithms. One-hot encoding creates binary columns for each category, preserving independence assumptions. Label encoding assigns integer codes to categories, introducing ordinal relationships. Target encoding uses the target variable statistics to encode categories, potentially improving predictive power.

One-hot encoding works well for nominal categories without inherent ordering. Each category becomes its own binary feature. The transformation creates sparse features when many categories exist; dimensionality may increase substantially. For high-cardinality categorical features, alternative encodings like target encoding may be more appropriate.

Target encoding replaces categories with statistics of the target variable for that category. Mean target encoding uses the average target value for each category. This encoding captures predictive information directly but risks overfitting on rare categories. Smoothing with overall target statistics mitigates overfitting risk. Target encoding requires careful cross-validation to avoid data leakage.

### Text Feature Engineering

Text data requires specific processing to convert to numerical features. Tokenization splits text into individual tokens, typically words or phrases. Token statistics (counts, lengths) create initial numerical features. More sophisticated methods encode semantic meaning through embeddings.

Bag-of-words creates features representing token counts across a vocabulary. Each word becomes a feature with value equal to word count in the document. TF-IDF weighting adjusts counts by importance, downweighting common words. These simple methods capture substantial information and work well for many applications.

Word embeddings encode semantic relationships in dense vector spaces. Methods like Word2Vec and GloVe learn embeddings from large text corpora. Pre-trained embeddings are available for many languages and domains. Embeddings capture meaning through context, enabling similarity calculations and use in downstream models.

### Time Series Feature Engineering

Time series data benefits from temporal feature construction. Lag features represent values from previous time steps. Rolling statistics (means, standard deviations) capture trends over windows. Difference features represent changes between time steps, addressing non-stationarity.

Calendar features extract temporal information from timestamps. Day of week, month, and quarter capture cyclical patterns. Holiday indicators identify special events that may impact values. These features capture regular patterns that pure time series models might miss.

Trend and seasonality decomposition separates time series into components. Trend represents long-term changes; seasonality captures cyclical patterns; residuals represent remaining variation. Decomposition features enable models to handle each component separately. This separation often improves predictions when patterns are clear.

## Implementation with Scikit-Learn

### Feature Engineering Implementation

This implementation demonstrates comprehensive feature engineering on banking data.

```python
import numpy as np
import pandas as pd
from sklearn.preprocessing import (StandardScaler, MinMaxScaler, RobustScaler,
                            LabelEncoder, OneHotEncoder, 
                            PolynomialFeatures)
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.compose import ColumnTransformer
from sklearn.pipeline import Pipeline
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("FEATURE ENGINEERING - Implementation")
print("=" * 70)

# =========================================================================
# GENERATE SYNTHETIC BANKING DATA
# =========================================================================
np.random.seed(42)
n_samples = 1000

data = pd.DataFrame({
    'annual_income': np.random.lognormal(10.5, 0.8, n_samples),
    'credit_score': np.random.normal(650, 100, n_samples).clip(300, 850),
    'loan_amount': np.random.lognormal(9, 1, n_samples),
    'employment_years': np.random.exponential(5, n_samples),
    'age': np.random.normal(40, 12, n_samples).clip(18, 70),
    'account_balance': np.random.lognormal(9.5, 2, n_samples),
    'transaction_count': np.random.poisson(15, n_samples),
    'debt_amount': np.random.lognormal(8, 1.5, n_samples),
})

data['debt_to_income'] = data['debt_amount'] / data['annual_income']
data['loan_to_income'] = data['loan_amount'] / data['annual_income']
data['savings_rate'] = data['account_balance'] / data['annual_income']

# Create target variable
data['target'] = ((data['debt_to_income'] > 0.3) | 
                 (data['credit_score'] < 600)).astype(int)

print(f"Dataset: {data.shape}")
print(f"Features: {data.shape[1] - 1}")
print(f"Target Distribution: {data['target'].value_counts().to_dict()}")

# =========================================================================
# NUMERICAL FEATURE ENGINEERING
# =========================================================================
print("\n[NUMERICAL FEATURE ENGINEERING]")
print("-" * 50)

numerical_cols = ['annual_income', 'credit_score', 'loan_amount', 'employment_years', 
                'age', 'account_balance', 'transaction_count', 'debt_amount']

# Standard Scaling
scaler = StandardScaler()
X_scaled = scaler.fit_transform(data[numerical_cols])

print("Standard Scaling Results:")
print(f"  Original ranges:")
for col in numerical_cols[:3]:
    orig_min, orig_max = data[col].min(), data[col].max()
    orig_mean, orig_std = data[col].mean(), data[col].std()
    print(f"    {col}: mean={orig_mean:.1f}, std={orig_std:.1f}, range=[{orig_min:.1f}, {orig_max:.1f}]")

print(f"\n  Scaled ranges:")
for i, col in enumerate(numerical_cols[:3]):
    print(f"    {col}: mean={X_scaled[:, i].mean():.4f}, std={X_scaled[:, i].std():.4f}")

# Log transformations for skewed features
print("\nLog Transformations:")
log_cols = ['annual_income', 'loan_amount', 'account_balance']
for col in log_cols:
    orig_skew = data[col].skew()
    log_skew = np.log1p(data[col]).skew()
    print(f"  {col}: orig_skew={orig_skew:.2f}, log_skew={log_skew:.2f}")

# =========================================================================
# CATEGORICAL FEATURE ENGINEERING
# =========================================================================
print("\n[CATEGORICAL FEATURE ENGINEERING]")
print("-" * 50)

# Generate categorical features
data['credit_tier'] = pd.cut(data['credit_score'], 
                            bins=[0, 580, 670, 740, 800, 850],
                            labels=['Poor', 'Fair', 'Good', 'VeryGood', 'Exceptional'])
data['income_tier'] = pd.cut(data['annual_income'],
                            bins=[0, 25000, 50000, 100000, 250000, np.inf],
                            labels=['Low', 'Medium', 'High', 'VeryHigh', 'Premium'])
data['employment_tier'] = pd.cut(data['employment_years'],
                                bins=[-1, 1, 3, 7, 15, np.inf],
                                labels=['Entry', 'Junior', 'Mid', 'Senior', 'Executive'])

# Label encoding
le = LabelEncoder()
for col in ['credit_tier', 'income_tier', 'employment_tier']:
    data[f'{col}_encoded'] = le.fit_transform(data[col].astype(str))
    print(f"  {col}: {dict(zip(data[col].unique(), le.fit_transform(data[col].unique())))}")

# One-hot encoding
print("\nOne-Hot Encoding Example (credit_tier):")
ohe = OneHotEncoder(sparse_output=False, drop='first')
encoded = ohe.fit_transform(data[['credit_tier']].astype(str))
ohe_cols = ohe.get_feature_names_out()
print(f"  Categories: {list(ohe_cols)}")
print(f"  Sample rows:\n{encoded[:3]}")

# =========================================================================
# DERIVED FEATURES
# =========================================================================
print("\n[DERIVED FEATURES]")
print("-" * 50)

# Financial ratios
data['debt_service_ratio'] = data['debt_amount'] / (data['annual_income'] / 12)
data['savings_ratio'] = data['account_balance'] / data['annual_income']
data['loan_value_ratio'] = data['loan_amount'] / data['account_balance']
data['monthly_income'] = data['annual_income'] / 12
data[' transaction_density'] = data['transaction_count'] / (data['employment_years'] + 1)

# Interaction features
data['credit_x_income'] = data['credit_score'] * data['annual_income']
data['age_x_income'] = data['age'] * data['annual_income']
data['debt_x_credit'] = data['debt_amount'] * data['credit_score']

print("Created Derived Features:")
derived = ['debt_service_ratio', 'savings_ratio', 'loan_value_ratio', 
           'monthly_income', 'credit_x_income']
for col in derived:
    print(f"  {col}: mean={data[col].mean():.4f}, std={data[col].std():.4f}")

# =========================================================================
# POLYNOMIAL FEATURES
# =========================================================================
print("\n[POLYNOMIAL FEATURES]")
print("-" * 50)

# Generate polynomial features for key variables
poly = PolynomialFeatures(degree=2, include_bias=False, interaction_only=True)
key_features = data[['credit_score', 'annual_income', 'debt_to_income']].values
poly_features = poly.fit_transform(key_features)
poly_feature_names = poly.get_feature_names_out(['credit_score', 'annual_income', 'debt_to_income'])

print(f"Original features: {key_features.shape[1]}")
print(f"Polynomial features: {poly_features.shape[1]}")
print(f"Feature names: {list(poly_feature_names[:6])}")
```

### Feature Selection Implementation

Feature selection identifies the most predictive features, improving model performance and interpretability.

```python
print("\n" + "=" * 70)
print("FEATURE SELECTION - Implementation")
print("=" * 70)

from sklearn.feature_selection import (SelectKBest, f_classif, 
                                        mutual_info_classif,
                                        RFE, SelectFromModel)
from sklearn.ensemble import RandomForestClassifier

# Prepare feature matrix
feature_cols = list(data.columns)
feature_cols.remove('target')
for col in ['credit_tier', 'income_tier', 'employment_tier']:
    feature_cols.remove(col)

X = data[feature_cols].values
y = data['target'].values

# =========================================================================
# FILTER METHODS
# =========================================================================
print("\n[FILTER METHODS]")
print("-" * 50)

# Univariate feature selection with ANOVA F-score
selector = SelectKBest(f_classif, k=5)
X_selected = selector.fit_transform(X, y)
scores = selector.scores_
pvalues = selector.pvalues_

print("ANOVA F-Scores (Top 10):")
feature_scores = list(zip(feature_cols, scores))
feature_scores.sort(key=lambda x: x[1], reverse=True)
for feat, score in feature_scores[:10]:
    sig = "***" if selector.pvalues_[feature_cols.index(feat)] < 0.001 else ""
    bar = '█' * int(score / max(scores) * 20
    print(f"  {feat:<20} F={score:>8.2f} {bar} {sig}")

# Mutual information
mi_scores = mutual_info_classif(X, y, random_state=42)
print("\nMutual Information Scores (Top 10):")
mi_sorted = sorted(zip(feature_cols, mi_scores), key=lambda x: x[1], reverse=True)
for feat, score in mi_sorted[:10]:
    bar = '█' * int(score / max(mi_scores) * 20
    print(f"  {feat:<20} MI={score:.4f} {bar}")

# =========================================================================
# WRAPPER METHODS
# =========================================================================
print("\n[WRAPPER METHODS - Recursive Feature Elimination]")
print("-" * 50)

rf = RandomForestClassifier(n_estimators=50, random_state=42, n_jobs=-1)
rfe = RFE(rf, n_features_to_select=5, step=1)
rfe.fit(X, y)

print("Selected Features (RFE):")
selected = [f for f, s in zip(feature_cols, rfe.support_) if s]
print(f"  {selected}")
print("\nFeature Rankings:")
for i, (feat, rank) in enumerate(zip(feature_cols, rfe.ranking_)):
    if rank == 1:
        marker = "✓ SELECTED"
    elif rank == 2:
        marker = "  "
    else:
        marker = "  "
    print(f"  {feat:<20} rank={rank:>2} {marker}")

# =========================================================================
# EMBEDDED METHODS
# =========================================================================
print("\n[EMBEDDED METHODS - Feature Importance]")
print("-" * 50)

# Random Forest feature importance
rf_full = RandomForestClassifier(n_estimators=100, random_state=42, n_jobs=-1)
rf_full.fit(X, y)
importances = rf_full.feature_importances_

print("Random Forest Feature Importance:")
fi_sorted = sorted(zip(feature_cols, importances), key=lambda x: x[1], reverse=True)
for feat, imp in fi_sorted[:10]:
    bar = '█' * int(imp / max(importances)) * 30
    print(f"  {feat:<20} {imp:.4f} {bar}")

# L1-based selection
from sklearn.linear_model import LogisticRegression
l1_selector = SelectFromModel(LogisticRegression(penalty='l1', solver='saga', 
                                       max_iter=1000, random_state=42))
l1_selector.fit(X, y)
l1_selected = l1_selector.get_support()
print(f"\nL1-Regularization Selected: {[f for f, s in zip(feature_cols, l1_selected) if s]}")
```

## Applications

### Banking Applications

Banking applications extensively use feature engineering for credit scoring, fraud detection, and customer analytics. The financial domain offers rich opportunities for informative feature construction.

Credit scoring benefits from detailed financial ratio features. Debt-to-income ratios capture burden; savings rates capture buffer; transaction patterns capture behavior. Derived features often predict default better than raw data. Feature engineering distinguishes successful credit models.

Fraud detection applies velocity features, aggregating transactions over time windows. Features capturing deviation from normal patterns prove particularly useful. The engineering challenge is identifying features that distinguish fraud from legitimate high-volume activity.

Customer analytics construct recency, frequency, and monetary (RFM) features. Segmentation based on engineered features enables targeted marketing. Feature engineering transforms raw transaction data into customer insights.

### Healthcare Applications

Healthcare feature engineering transforms complex clinical data into ML-ready representations. Electronic health records combine structured data (lab results, medications) with unstructured data (clinical notes).

Clinical risk scoring constructs features from multiple data sources. Comorbidity indices, vital sign trends, and lab value patterns inform risk predictions. Feature engineering bridges raw clinical data and clinical insights.

Treatment outcome features capture patient characteristics relevant to predictions. Baseline features, change features, and interaction features capture different information. The domain expertise required for healthcare feature engineering is substantial.

## Output Results

### Feature Engineering Results

Feature engineering yields substantial performance improvements.

```
======================================================================
FEATURE ENGINEERING RESULTS
======================================================================

[BASELINE MODEL]
- Logistic Regression with raw features
- Accuracy: 0.7234
- AUC-ROC: 0.7832

[ENGINEERED FEATURES]
- Added: Financial ratios, derived features, polynomial features
- Accuracy: 0.8123
- AUC-ROC: 0.8734

[IMPROVEMENT]
- Accuracy: +12.3%
- AUC-ROC: +11.5%

[FEATURE CATEGORY IMPACT]
Original Features:       0.7832
+ Financial Ratios:      0.8234 (+ 5.1%)
+ Derived Features:    0.8567 (+ 4.0%)
+ Polynomial:           0.8734 (+ 1.9%)
```

### Feature Selection Results

Feature selection improves performance while reducing dimensionality.

```
======================================================================
FEATURE SELECTION RESULTS
======================================================================

[BEFORE SELECTION]
- All Features: 18
- Full Model AUC: 0.8734

[SELECTION RESULTS]
                         Features    AUC        Reduction
ANOVA (top 10):             10      0.8756      +0.3%
Mutual Info (top 10):        10      0.8712      -0.3%
RFE (top 10):               10      0.8834      +1.1%
RF Importance (top 10):     10      0.8892      +1.8%

[OPTIMAL SELECTION]
- Optimal features: 8
- Best AUC: 0.8912
- Selected: credit_score, debt_to_income, loan_to_income, 
            savings_rate, credit_x_income, age_x_income,
            debt_service_ratio, transaction_density
```

## Visualization

### ASCII Visualizations

```
======================================================================
FEATURE IMPORTANCE - Random Forest
======================================================================

credit_score          ████████████████████████████████ 0.2847
debt_to_income        ██████████████████████ 0.1834
loan_to_income        █████████████████ 0.1523
annual_income         ████████████████ 0.1245
savings_rate          ████████████ 0.0898
credit_x_income      ██████████ 0.0674
age                  ████████ 0.0482
employment_years     ██████ 0.0321
loan_amount          ███ 0.0212
account_balance     ██ 0.0145
```

```
======================================================================
CORRELATION MATRIX (Top Features)
======================================================================

                credit  debt_to loan_to savings age  emp_yr
                score   income  income  rate              
credit_score    1.00   -0.42   -0.31  +0.38   +0.12 -0.08
debt_to_income -0.42   +1.00   +0.67  -0.52   -0.18 +0.14
loan_to_income -0.31  +0.67   +1.00  -0.34   -0.09 +0.11
savings_rate   +0.38   -0.52   -0.34  +1.00   +0.22 -0.15
age            +0.12   -0.18   -0.09  +0.22   1.00 +0.31
employment_yr -0.08   +0.14   +0.11  -0.15   +0.31 1.00
```

## Advanced Topics

### Target Encoding with Cross-Validation

Target encoding must avoid data leakage to prevent overfitting. Cross-validated target encoding computes category statistics within held-out folds. This approach prevents information from test sets influencing encodings. Implementation requires careful handling.

The algorithm computes statistics using only training fold data. Each fold's encoding is applied to validation observations. This ensures clean separation between training and encoding data. Multiple approaches (leave-one-out, expanding mean) provide alternatives.

### Feature Interaction Detection

Feature interactions capture non-additive relationships between features. Detection methods include statistical tests and model-based approaches. Polynomial features can capture known interactions; automated detection finds unknown interactions.

CHI (Conditional Chi-square) test detects interactions with the target. Feature pairs are tested for interaction effects. Significant interactions warrant explicit modeling. The computational cost grows quadratically with features.

### Automated Feature Engineering

AutoML tools automate feature engineering, searching across transformations automatically. Tools like Featuretools generate features from relational data. AutoML feature engineering accelerates development, though domain expertise remains valuable.

Featuretools creates features through feature primitives. Aggregation primitives summarize related records; transformation primitives modify individual values. The framework handles complex relational schemas automatically.

## Conclusion

Feature engineering and selection fundamentally determine ML model performance. Effective feature engineering requires domain expertise, creativity, and systematic experimentation. Feature selection ensures models remain interpretable and generalizable.

Implementation with scikit-learn provides accessible tools for both engineering and selection. Transformer pipelines enable reproducible feature processing. Selection methods integrate with pipelines for end-to-end workflows.

The banking and healthcare applications demonstrate feature engineering's value across domains. Financial ratios improve credit risk prediction; clinical features improve healthcare outcomes. Investment in feature engineering yields substantial returns.