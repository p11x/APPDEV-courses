# Feature Selection Methods

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the importance of feature selection in machine learning
- Implement filter methods (correlation, chi-square, mutual information)
- Apply wrapper methods (recursive feature elimination)
- Use embedded methods (Lasso, feature importance from trees)
- Choose appropriate feature selection method for different problems
- Avoid data leakage in feature selection

## Theoretical Background

Feature selection is the process of choosing a subset of relevant features for model building. It helps improve model performance, reduce overfitting, decrease training time, and enhance model interpretability.

### Why Feature Selection Matters

1. **防止维度灾难**: As the number of features increases, the feature space becomes sparse
2. **减少过拟合**: Fewer features reduce the risk of fitting noise
3. **提高模型性能**: Removing irrelevant or redundant features can improve accuracy
4. **提高可解释性**: Fewer features are easier to understand and explain
5. **减少训练时间**: Fewer features mean faster training and prediction

### Feature Selection Methods

#### Filter Methods

Filter methods select features based on statistical measures, independently of any machine learning algorithm:

- **Correlation**: Remove highly correlated features
- **Chi-Square Test**: For categorical features and classification
- **Mutual Information**: Measures dependency between variables
- **Variance Threshold**: Remove low-variance features

**Advantages**: Fast, scalable, model-agnostic
**Disadvantages**: May miss feature interactions

#### Wrapper Methods

Wrapper methods evaluate feature subsets by training a model:

- **Recursive Feature Elimination (RFE)**: Iteratively removes features
- **Forward Selection**: Iteratively adds features
- **Backward Elimination**: Iteratively removes features

**Advantages**: Considers feature interactions
**Disadvantages**: Computationally expensive, risk of overfitting

#### Embedded Methods

Embedded methods perform feature selection during model training:

- **Lasso Regression (L1)**: Drives coefficients to zero
- **Tree-based Importance**: Uses feature importance from trees
- **Regularization**: Penalizes complex models

**Advantages**: Combines benefits of filter and wrapper
**Disadvantages**: Model-specific

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("caret")
install.packages("corrplot")
install.packages("glmnet")
install.packages("randomForest")

library(caret)
library(corrplot)
library(glmnet)
library(randomForest)
```

### Step 2: Variance Threshold Method

```r
# Load data
data(iris)

# Check variance of features
feature_variances <- sapply(iris[, -5], var)
print("Feature variances:")
print(feature_variances)

# Using caret's nearZeroVar to identify low-variance features
nzv <- nearZeroVar(iris[, -5], saveMetrics = TRUE)
print(nzv)
#            freqRatio percentUnique var nzv
# Sepal.Length   1        0.667         0.141 FALSE
# ...

# Identify near-zero variance features
nearZeroVar(iris[, -5])
# integer(0) - no near-zero variance features

# Remove near-zero variance features
clean_features <- iris[, -5][, nearZeroVar(iris[, -5])]
```

### Step 2: Correlation-Based Filter

```r
# Create sample data with correlation
set.seed(42)
n <- 100
corr_data <- data.frame(
  x1 = rnorm(n),
  x2 = rnorm(n, mean = 2),
  x3 = rnorm(n),
  x4 = x1 + rnorm(n, 0, 0.1),  # Highly correlated with x1
  x5 = rnorm(n),
  y = x1 - x2 + rnorm(n, 0, 0.5)
)

# Calculate correlation matrix
cor_matrix <- cor(corr_data)
print(round(cor_matrix, 2))

# Visualize correlations
corrplot(cor_matrix, method = "circle")

# Find highly correlated features
high_corr <- findCorrelation(cor_matrix, cutoff = 0.9)
print("Highly correlated features to remove:")
print(high_corr)

# Remove correlated features
clean_data <- corr_data[, -high_corr]
print("Features after removing highly correlated:")
print(names(clean_data))
```

### Step 3: Univariate Feature Selection

```r
# Chi-square test for categorical features
# Create sample categorical data
set.seed(123)
n <- 200
chi_data <- data.frame(
  color = factor(sample(c("red", "blue", "green"), n, replace = TRUE)),
  size = factor(sample(c("small", "medium", "large"), n, replace = TRUE)),
  shape = factor(sample(c("circle", "square"), n, replace = TRUE)),
  category = factor(sample(c("A", "B"), n, prob = c(0.7, 0.3), replace = TRUE))
)

# Chi-square test for each feature
chi_results <- apply(chi_data[, -4], 2, function(x) {
  test <- chisq.test(table(x, chi_data$category))
  c(statistic = test$statistic, p.value = test$p.value)
})
print(chi_results)

# Using caret's chiSq filter
# Need to convert to binary for chiSq
x <- model.matrix(~ . - 1, data = chi_data[, -4])
y <- as.numeric(chi_data$category) - 1

# Filter features
filtered <- chiSq(x, y)
print(filtered)
```

### Step 4: Recursive Feature Elimination (Wrapper Method)

```r
# Load data
data(iris)

# Define RFE control
set.seed(42)
rfe_control <- rfeControl(
  functions = rfFuncs,  # Use random forest for evaluation
  method = "cv",
  number = 5
)

# Run RFE
feature_results <- rfe(
  x = iris[, -5],
  y = iris$Species,
  sizes = c(1, 2, 3, 4),
  rfeControl = rfe_control
)

# View results
print(feature_results)
# 
# Recursive Feature Selection
# 
# 150 samples
# 4 predictor
# 
# Resampling results:
# 
# Variables    Accuracy   Kappa    
#   1         0.833     0.750    
#   2         0.952     0.928    
#   3         0.967     0.950    
#   4         0.967     0.950

# Plot results
plot(feature_results, type = c("g", "o"))

# Selected features
feature_results$optVariables
```

### Step 5: Lasso Feature Selection (Embedded Method)

```r
# Create sample data
set.seed(456)
n <- 100
p <- 10

# Create features
x <- matrix(rnorm(n * p), n, p)
colnames(x) <- paste0("x", 1:p)

# Create response with some relevant features
y <- 2 * x[, 1] - 1.5 * x[, 3] + 0.5 * x[, 5] + rnorm(n, 0, 0.5)

# Fit Lasso
lasso_model <- glmnet(x, y, family = "gaussian", alpha = 1)

# Plot path
plot(lasso_model, xvar = "lambda", label = TRUE)

# Find best lambda using cross-validation
cv_lasso <- cv.glmnet(x, y)
plot(cv_lasso)

# Coefficients at optimal lambda
coef(lasso_model, s = cv_lasso$lambda.min)
# Shows which features have non-zero coefficients

# Extract selected features
selected_features <- which(coef(lasso_model, s = cv_lasso$lambda.min) != 0)
selected_features <- names(selected_features)[selected_features > 0]
print("Selected features:")
print(selected_features)
```

## Code Examples

### Example 1: Feature Selection for Spam Detection

This example shows feature selection for email spam classification.

```r
# Create spam data
set.seed(789)
n <- 500

spam_data <- data.frame(
  word_count = rpois(n, 150),
  capital_letters = rnorm(n, 50, 30),
  num_links = rpois(n, 3),
  num_images = rpois(n, 1),
  num_exclamations = rpois(n, 2),
  has_urgent = rbinom(n, 1, 0.1),
  has_free = rbinom(n, 1, 0.15),
  has_winner = rbinom(n, 1, 0.1),
  num_recipients = rpois(n, 2),
  subject_length = rnorm(n, 50, 15),
  spam = factor(sample(c("ham", "spam"), n, prob = c(0.7, 0.3), replace = TRUE))
)

# Add relationships
spam_data$spam[spam_data$has_winner == 1] <- "spam"
spam_data$spam[spam_data$num_links > 5] <- "spam"

# Correlation-based filtering
cor_matrix <- cor(spam_data[, -11])
high_corr <- findCorrelation(cor_matrix, cutoff = 0.8)
spam_clean <- spam_data[, -high_corr]

cat("Original features:", ncol(spam_data) - 1, "\n")
cat("After correlation filter:", ncol(spam_clean) - 1, "\n")

# RFE for final feature selection
set.seed(42)
rfe_control <- rfeControl(functions = rfFuncs, method = "cv", number = 5)

spam_rfe <- rfe(
  x = spam_clean[, -10],
  y = spam_clean$spam,
  sizes = c(2, 4, 6, 8),
  rfeControl = rfe_control
)

print(spam_rfe)
# Shows best feature subsets
```

### Example 2: Feature Selection for Medical Diagnosis

This example demonstrates embedded feature selection for medical data.

```r
# Create medical data
set.seed(321)
n <- 300

medical_data <- data.frame(
  age = rnorm(n, 50, 15),
  bmi = rnorm(n, 28, 5),
  blood_pressure_sys = rnorm(n, 130, 20),
  blood_pressure_dia = rnorm(n, 85, 12),
  cholesterol_total = rnorm(n, 200, 40),
  cholesterol_ldl = rnorm(n, 130, 35),
  cholesterol_hdl = rnorm(n, 50, 15),
  blood_sugar = rnorm(n, 100, 25),
  heart_rate = rnorm(n, 75, 12),
  exercise_minutes = rnorm(n, 30, 20),
  diagnosis = factor(ifelse(rnorm(n) > 0, "disease", "healthy"))
)

# Add correlations
medical_data$cholesterol_ldl <- medical_data$cholesterol_total * 0.6 + rnorm(n, 0, 10)
medical_data$cholesterol_hdl <- medical_data$cholesterol_total * 0.25 + rnorm(n, 0, 5)

# Random Forest importance
set.seed(42)
rf_model <- randomForest(diagnosis ~ ., data = medical_data, importance = TRUE)

# Get importance
importance_df <- data.frame(
  feature = names(importance(rf_model)),
  importance = importance(rf_model)[, "MeanDecreaseGini"]
)
importance_df <- importance_df[order(-importance_df$importance), ]
print(importance_df)

# Select top features
top_features <- importance_df$feature[1:5]
cat("\nTop 5 features:\n")
print(top_features)

# Train final model with selected features
formula <- as.formula(paste("diagnosis", paste(top_features, collapse = "+"), sep = "~"))
final_model <- randomForest(formula, data = medical_data)
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Split Data First**: Always split data before feature selection to avoid leakage
2. **Use Domain Knowledge**: Combine statistical methods with domain expertise
3. **Start Simple**: Begin with filter methods, then wrapper if needed
4. **Validate Selection**: Test selected features on held-out data
5. **Consider Redundancy**: Highly correlated features should be handled

### Common Pitfalls

1. **Data Leakage**: Feature selection on entire dataset before splitting
2. **Ignoring Correlation**: Removing one of correlated features
3. **Overfitting**: Too many features relative to sample size
4. **Not Considering Interaction**: Some methods miss feature interactions

## Performance Considerations

### Method Comparison

| Method | Speed | Handles Interactions | Risk of Overfitting |
|--------|-------|---------------------|--------------------|
| Filter | Fast | No | Low |
| Wrapper | Slow | Yes | High |
| Embedded | Medium | Yes | Medium |

### Guidelines

- For small datasets: Use embedded methods
- For large datasets: Use filter methods first, then wrapper
- For high-dimensional data: Start with variance and correlation filters

## Related Concepts

- **PCA**: For dimensionality reduction
- **Feature Engineering**: Creating new features
- **Domain Knowledge**: Using expert input
- **Regularization**: L1 (Lasso), L2 (Ridge)

## Exercise Problems

1. **Basic**: Identify low-variance features in a dataset.

2. **Intermediate**: Apply correlation-based feature selection.

3. **Advanced**: Implement forward feature selection.

4. **Real-World Challenge**: Perform feature selection on real dataset.

5. **Extension**: Compare different feature selection methods.