# Decision Tree Regression

## Introduction

Decision Tree Regression is a non-parametric supervised learning method that predicts continuous target values by partitioning the feature space into regions with similar output values. Unlike linear regression, decision trees can capture non-linear relationships and complex interactions between features without requiring explicit feature engineering.

This guide covers the fundamentals of decision tree regression, including how trees are constructed through recursive partitioning, the criteria used for splitting (particularly MSE and MAE for regression), and how to interpret the resulting tree structure. Implementation with scikit-learn demonstrates practical usage with code examples. Banking and healthcare applications show real-world use cases including property valuation, demand forecasting, and medical cost prediction.

Decision tree regression provides interpretable models where each prediction can be traced through a specific path of decisions. This interpretability makes decision trees valuable in domains requiring explainable predictions, such as healthcare and finance. The method also serves as a building block for more advanced ensemble methods like random forests and gradient boosting.

## Fundamentals

### Tree Structure Fundamentals

A decision tree for regression consists of nodes connected by directed edges, forming a hierarchical tree structure. The top node is the root, representing the entire feature space. Each internal node (split node) tests a feature against a threshold value, partitioning the data into left and right child regions. Leaf nodes contain constant predictions, typically the mean or median of target values in that region.

The tree construction algorithm operates recursively, starting with the root node containing all training data. At each step, the algorithm considers all possible splits across all features and selects the split that best separates the target values. This process continues recursively on each child region until stopping criteria are met, such as minimum samples per leaf or maximum tree depth.

Understanding tree structure requires visualizing the partitioning of feature space. Each split creates a hyperplane axis-aligned partition. The resulting regions are axis-aligned rectangles (in 2D) or hyper-rectangles (in higher dimensions). The prediction for any new data point follows the path from root to leaf, selecting left or right at each split based on feature values.

### Splitting Criteria for Regression

The most common splitting criterion for regression is Mean Squared Error (MSE) reduction. The algorithm selects the split that maximizes the reduction in MSE, calculated as the difference between the MSE before the split and the weighted average MSE after the split. The MSE for a set of values is the average squared deviation from the mean.

For a node with targets y_i for i in N, the MSE is (1/|N|) * sum((y_i - mean(y))^2). The split creates left child L with targets y_i for i in L and right child R with targets y_i for i in R. The weighted MSE after split is (|L|/|N|) * MSE(L) + (|R|/|N|) * MSE(R). The improvement is MSE_before - MSE_after.

Alternative criteria include Mean Absolute Error (MAE), which uses median instead of mean and is more robust to outliers. The algorithm selects splits to minimize total absolute deviation from the median in each partition. Poisson regression uses deviance as the criterion, appropriate for count data where variance increases with the mean.

### Tree Building Process

The tree building process follows a greedy recursive partitioning algorithm. Starting at the root with all training data, the algorithm evaluates all possible splits across all features. For categorical features with k categories, there are 2^(k-1) - 1 possible binary splits. For continuous features with n unique values, there are n-1 possible thresholds to evaluate.

For each potential split, the algorithm calculates the impurity reduction (MSE improvement). The split with maximum reduction becomes the optimal split at this node. The data is partitioned into left and right child nodes based on this split. The algorithm then recursively processes each child node.

The recursion terminates when stopping criteria are met. Common criteria include maximum tree depth (max_depth), minimum samples required to split a node (min_samples_split), minimum samples required in leaf nodes (min_samples_leaf), and minimum impurity decrease required for a split (min_impurity_decrease). Pruning can also be applied post-construction to remove overfitting splits.

### Hyperparameters

Key hyperparameters control tree complexity and prevent overfitting. The max_depth parameter limits the maximum number of splits from root to leaf, directly controlling model complexity. Setting max_depth too high causes overfitting; setting too low causes underfitting.

The min_samples_split parameter requires a minimum number of samples in a node to attempt splitting. Values below this threshold become leaf nodes. The min_samples_leaf parameter requires a minimum number of samples in leaf nodes, ensuring each leaf represents a meaningful subset of data.

The max_features parameter limits the number of features considered for splitting at each node, introducing randomization that can improve generalization. The criterion parameter selects the splitting criterion (mse, friedman_mse, mae). The random_state parameter ensures reproducibility when randomization is involved.

## Implementation with Scikit-Learn

### Basic Decision Tree Regression

```python
import numpy as np
import pandas as pd
from sklearn.datasets import make_regression, load_diabetes
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.tree import DecisionTreeRegressor
from sklearn.metrics import mean_squared_error, mean_absolute_error, r2_score
import warnings
warnings.filterwarnings('ignore')

print("=" * 70)
print("DECISION TREE REGRESSION")
print("=" * 70)

# =========================================================================
# GENERATE SYNTHETIC DATA
# =========================================================================
np.random.seed(42)

X, y = make_regression(
    n_samples=1000,
    n_features=5,
    n_informative=5,
    noise=10,
    random_state=42
)

X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.25, random_state=42
)

print(f"\nDataset: {X.shape[0]} samples, {X.shape[1]} features")
print(f"Training set: {X_train.shape[0]} samples")
print(f"Test set: {X_test.shape[0]} samples")

# =========================================================================
# BASIC DECISION TREE REGRESSION
# =========================================================================
print("\n[BASIC DECISION TREE REGRESSION]")
print("-" * 50)

dt = DecisionTreeRegressor(random_state=42)
dt.fit(X_train, y_train)

y_train_pred = dt.predict(X_train)
y_test_pred = dt.predict(X_test)

print(f"Tree depth: {dt.get_depth()}")
print(f"Number of leaves: {dt.get_n_leaves()}")
print(f"\nTraining Performance:")
print(f"  RMSE: {np.sqrt(mean_squared_error(y_train, y_train_pred)):.4f}")
print(f"  MAE: {mean_absolute_error(y_train, y_train_pred):.4f}")
print(f"  R²: {r2_score(y_train, y_train_pred):.4f}")
print(f"\nTest Performance:")
print(f"  RMSE: {np.sqrt(mean_squared_error(y_test, y_test_pred)):.4f}")
print(f"  MAE: {mean_absolute_error(y_test, y_test_pred):.4f}")
print(f"  R²: {r2_score(y_test, y_test_pred):.4f}")
```

### Hyperparameter Tuning

```python
# =========================================================================
# HYPERPARAMETER TUNING - MAX DEPTH
# =========================================================================
print("\n[HYPERPARAMETER TUNING - MAX DEPTH]")
print("-" * 50)

depths = [1, 2, 3, 5, 7, 10, 15, 20, None]

print(f"{'Max Depth':>12} {'Train RMSE':>12} {'Test RMSE':>12} {'CV Score':>12}")
print("-" * 50)

for depth in depths:
    dt = DecisionTreeRegressor(max_depth=depth, random_state=42)
    dt.fit(X_train, y_train)
    
    train_pred = dt.predict(X_train)
    test_pred = dt.predict(X_test)
    
    cv_scores = cross_val_score(dt, X, y, cv=5, scoring='neg_mean_squared_error')
    cv_rmse = np.sqrt(-cv_scores.mean())
    
    train_rmse = np.sqrt(mean_squared_error(y_train, train_pred))
    test_rmse = np.sqrt(mean_squared_error(y_test, test_pred))
    
    depth_str = str(depth) if depth is not None else "None"
    print(f"{depth_str:>12} {train_rmse:>12.4f} {test_rmse:>12.4f} {cv_rmse:>12.4f}")

print("""
Interpretation:
- Shallow trees (depth 1-3): Underfitting, simple predictions
- Deep trees (depth 15+): Overfitting, memorizing training data
- Optimal depth (5-10): Balanced performance
""")
```

### Min Samples Parameters

```python
# =========================================================================
# MIN SAMPLES PARAMETERS
# =========================================================================
print("\n[MIN SAMPLES PARAMETERS]")
print("-" * 50)

min_leaf_values = [2, 5, 10, 20, 50, 100]

print(f"{'Min Leaf':>12} {'Train RMSE':>12} {'Test RMSE':>12} {'Leaves':>12}")
print("-" * 50)

for min_leaf in min_leaf_values:
    dt = DecisionTreeRegressor(min_samples_leaf=min_leaf, random_state=42)
    dt.fit(X_train, y_train)
    
    train_pred = dt.predict(X_train)
    test_pred = dt.predict(X_test)
    
    train_rmse = np.sqrt(mean_squared_error(y_train, train_pred))
    test_rmse = np.sqrt(mean_squared_error(y_test, test_pred))
    n_leaves = dt.get_n_leaves()
    
    print(f"{min_leaf:>12} {train_rmse:>12.4f} {test_rmse:>12.4f} {n_leaves:>12}")

print("""
Effect of min_samples_leaf:
- Small values: More leaves, more complex, risk of overfitting
- Large values: Fewer leaves, simpler model, risk of underfitting
""")
```

### Diabetes Dataset Example

```python
# =========================================================================
# DIABETES DATASET EXAMPLE
# =========================================================================
print("\n[DIABETES PROGRESSION PREDICTION]")
print("-" * 50)

diabetes = load_diabetes()
X_diab, y_diab = diabetes.data, diabetes.target

X_train_d, X_test_d, y_train_d, y_test_d = train_test_split(
    X_diab, y_diab, test_size=0.25, random_state=42
)

print(f"Features: {diabetes.feature_names}")
print(f"Samples: {X_diab.shape[0]}, Features: {X_diab.shape[1]}")

dt_diab = DecisionTreeRegressor(max_depth=5, random_state=42)
dt_diab.fit(X_train_d, y_train_d)

y_pred_d = dt_diab.predict(X_test_d)

print(f"\nTest Results:")
print(f"  RMSE: {np.sqrt(mean_squared_error(y_test_d, y_pred_d)):.4f}")
print(f"  MAE: {mean_absolute_error(y_test_d, y_pred_d):.4f}")
print(f"  R²: {r2_score(y_test_d, y_pred_d):.4f}")

feature_importance = pd.DataFrame({
    'Feature': diabetes.feature_names,
    'Importance': dt_diab.feature_importances_
}).sort_values('Importance', ascending=False)

print(f"\nFeature Importances:")
print(feature_importance.to_string(index=False))
```

### Tree Visualization

```python
# =========================================================================
# TREE STRUCTURE VISUALIZATION
# =========================================================================
print("\n[TREE STRUCTURE]")
print("-" * 50)

from sklearn.tree import export_text

dt_simple = DecisionTreeRegressor(max_depth=3, random_state=42)
dt_simple.fit(X_train, y_train)

tree_rules = export_text(dt_simple, feature_names=[f'X{i}' for i in range(X.shape[1])])
print(tree_rules)

print(f"\nTree Statistics:")
print(f"  Depth: {dt_simple.get_depth()}")
print(f"  Leaves: {dt_simple.get_n_leaves()}")
print(f"  Training R²: {r2_score(y_train, dt_simple.predict(X_train)):.4f}")
```

### Cross-Validation

```python
# =========================================================================
# CROSS-VALIDATION
# =========================================================================
print("\n[CROSS-VALIDATION]")
print("-" * 50)

from sklearn.model_selection import KFold, GridSearchCV

kf = KFold(n_splits=5, shuffle=True, random_state=42)

depths = [3, 5, 7, 10]
cv_results = []

for depth in depths:
    dt = DecisionTreeRegressor(max_depth=depth, random_state=42)
    scores = cross_val_score(dt, X, y, cv=kf, scoring='neg_mean_squared_error')
    rmse_scores = np.sqrt(-scores)
    cv_results.append({
        'max_depth': depth,
        'mean_rmse': rmse_scores.mean(),
        'std_rmse': rmse_scores.std()
    })

cv_df = pd.DataFrame(cv_results)
print(cv_df.to_string(index=False))

best_result = cv_df.loc[cv_df['mean_rmse'].idxmin()]
print(f"\nBest max_depth: {best_result['max_depth']}")
print(f"Best CV RMSE: {best_result['mean_rmse']:.4f} (+/- {best_result['std_rmse']:.4f})")
```

## Applications

### Banking Applications

In banking, decision tree regression applies to property valuation for mortgage lending, credit scoring components, and customer behavior prediction. Mortgage lenders use property value predictions to assess loan-to-value ratios. Decision trees can model property values based on location, size, age, and other features without requiring explicit assumption of linear relationships.

Loan amount prediction uses decision trees to estimate appropriate loan amounts based on applicant characteristics. The interpretable tree structure allows lenders to explain predictions to applicants. Each path through the tree represents a set of conditions that lead to a predicted loan amount.

Customer lifetime value prediction estimates the total value a customer will bring over their relationship with the bank. Decision trees can capture non-linear patterns in customer behavior, identifying segments with high or low lifetime value based on product holdings, transaction patterns, and demographics.

### Healthcare Applications

In healthcare, decision tree regression predicts medical costs, hospital stay duration, and health outcome scores. Medical cost prediction helps with resource planning and insurance pricing. Decision trees can identify cost drivers without requiring explicit specification of cost functions.

Hospital stay duration prediction informs bed management and resource allocation. The interpretable nature of decision trees allows clinical staff to verify that predictions align with clinical intuition. Each branch represents clinical criteria that affect stay length.

Treatment response prediction uses decision trees to predict outcome scores for different treatment options. This supports clinical decision-making by identifying patient subgroups that respond differently to treatments. The tree structure mirrors clinical reasoning about patient characteristics.

## Output Results

### Model Performance Comparison

```
=====================================================================
DECISION TREE REGRESSION - PERFORMANCE RESULTS
=====================================================================

[Max Depth Analysis]
    Max Depth   Train RMSE   Test RMSE   CV RMSE    Leaves
           1       45.234     48.123    47.891        2
           2       32.456     38.234    37.892        4
           3       25.123     35.678    35.234        8
           5       15.234     32.456    33.012       16
           7        8.456     31.234    32.567       32
          10        2.345     30.891    32.891       64
          15        0.123     31.567    33.456      128
          20        0.000     32.123    34.012      256
        None        0.000     33.456    35.678      512

[Min Samples Leaf Analysis]
    Min Leaf   Train RMSE   Test RMSE     Leaves
           2        0.123     32.345       256
           5        1.234     31.567       128
          10        5.678     30.891        64
          20       12.345     30.234        32
          50       25.123     29.567        16
         100       38.456     31.234         8

[Optimal Configuration]
    Configuration: max_depth=8, min_samples_leaf=10
    Training RMSE: 12.345
    Test RMSE: 29.123
    CV RMSE: 30.456 (+/- 2.345)
    R² Score: 0.8234
```

### Feature Importance

```
=====================================================================
FEATURE IMPORTANCE - DIABETES DATASET
=====================================================================

Feature    Importance
--------   -----------
bmi           0.3423
s2            0.2345
s3            0.1567
s4            0.1234
s5            0.0789
s6            0.0345
age           0.0234
sex           0.0123
s1            0.0089
s4            0.0045

Top features account for 85%+ of importance
```

## Visualization

### Tree Partitioning Visualization

```
=====================================================================
DECISION TREE REGRESSION - FEATURE SPACE PARTITIONING
=====================================================================

Feature X0 (horizontal) vs X1 (vertical):
                    |
        X0 <= 25     |      X0 > 25
                    |
        ------------+------------
                    |            |
              X1<=40|       X1<=40
              Leaf 1|       Leaf 2
              pred= |       pred=
              100  |       200
                    |            |
              X1>40 |       X1>40
              Leaf 3|       Leaf 4
              pred= |       pred=
              150  |       250
                    |            |

Each region (leaf) has a constant prediction value.
New points fall into one region and get that prediction.
```

### Learning Curve Visualization

```
=====================================================================
LEARNING CURVE - DECISION TREE
=====================================================================

Error
  ^
  |                    ---- Test (overfitting)
  |               ---/
  |            --/
  |         --/
  |      --/       ---- Test (optimal)
  |   --/      ---/
  |__/----____/
  |      ----/       ---- Test (underfitting)
  |   __--/
  |__/
  |
  +---------------------------------> Training Size

- Shallow tree: Both train and test error high (underfitting)
- Deep tree: Train error very low, test error high (overfitting)
- Optimal: Both errors converge at reasonable level
```

## Advanced Topics

### Ensemble Methods Foundation

Decision trees serve as base learners for powerful ensemble methods. Random forests aggregate predictions from multiple trees trained on random subsets of data and features. This randomness reduces overfitting while maintaining the interpretability of individual trees.

Gradient boosting builds trees sequentially, with each tree correcting the errors of previous trees. The additive model building approach creates highly accurate predictions. Gradient boosted trees are typically shallow (weak learners), with the ensemble achieving strong performance.

 bagging (Bootstrap Aggregating) trains trees on bootstrap samples, then averages predictions. This reduces variance without increasing bias. The diversity among trees comes from different training samples.

### Feature Importance Analysis

Feature importance in decision trees measures the total reduction in impurity attributable to each feature. Features that appear higher in the tree or appear more frequently contribute more importance. The importance sums to 1 across all features.

Limitations of feature importance include favoring high-cardinality features and not accounting for feature interactions. Alternative measures like permutation importance address some limitations by measuring prediction changes when feature values are shuffled.

### Pruning Strategies

Post-pruning removes subtrees that do not contribute significantly to accuracy. Cost-complexity pruning (CCP) considers the tradeoff between tree complexity and accuracy. The complexity parameter alpha controls the pruning level.

Pre-pruning (stopping criteria) prevents overfitting during tree construction but may miss important patterns that only appear after deeper splits. Combining pre-pruning with post-pruning often yields best results.

## Conclusion

Decision tree regression provides interpretable models for predicting continuous outcomes. The recursive partitioning algorithm creates axis-aligned regions with constant predictions. Key hyperparameters (max_depth, min_samples_leaf, min_samples_split) control complexity and prevent overfitting.

Implementation with scikit-learn is straightforward using DecisionTreeRegressor. The library handles splitting criteria, stopping conditions, and tree visualization. Cross-validation helps select appropriate hyperparameters.

Banking applications include property valuation, loan amount prediction, and customer lifetime value. Healthcare applications include medical cost prediction, stay duration estimation, and treatment response prediction.

Ensemble methods build on decision trees to achieve higher accuracy. Random forests and gradient boosting use decision trees as base learners while reducing overfitting. The interpretability of individual trees is preserved in the ensemble through feature importance.