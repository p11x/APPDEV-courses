# Regularization Methods in R

## Learning Objectives

- Understand the need for regularization in machine learning
- Implement Ridge, Lasso, and Elastic Net regression
- Tune hyperparameters using cross-validation
- Choose the best regularization method for your data

## Theoretical Background

### Why Regularization?

When we have many features or correlated predictors, standard linear/logistic regression can:
1. Overfit the training data
2. Produce unstable coefficients
3. Be computationally inefficient

Regularization adds a penalty term to the loss function to constrain coefficient magnitudes.

### Types of Regularization

#### 1. Ridge Regression (L2 Penalty)
Adds penalty: $\alpha \sum \beta_j^2$

- Shrinks coefficients toward zero but not exactly to zero
- Good for correlated predictors
- Keeps all features

#### 2. Lasso Regression (L1 Penalty)
Adds penalty: $\alpha \sum |\beta_j|$

- Can shrink coefficients exactly to zero (feature selection)
- Creates sparse models
- Good for feature selection

#### 3. Elastic Net
Combines L1 and L2 penalties:
$\alpha \rho \sum |\beta_j| + \frac{\alpha(1-\rho)}{2} \sum \beta_j^2$

- Balances Ridge and Lasso
- Useful when multiple correlated features exist

## Code Examples

### Standard Example: Ridge and Lasso

```r
# Install and load required packages
library(glmnet)

# Prepare data
set.seed(42)
n <- 100
p <- 20
X <- matrix(rnorm(n * p), n, p)
y <- X[, 1] + X[, 2] + rnorm(n)

# Fit Ridge regression
ridge_model <- glmnet(X, y, alpha = 0)  # alpha = 0 for Ridge

# Fit Lasso regression
lasso_model <- glmnet(X, y, alpha = 1)  # alpha = 1 for Lasso

# Cross-validation to find optimal lambda
cv_ridge <- cv.glmnet(X, y, alpha = 0)
cv_lasso <- cv.glmnet(X, y, alpha = 1)

# Best lambda values
best_lambda_ridge <- cv_ridge$lambda.min
best_lambda_lasso <- cv_lasso$lambda.min

cat("Best Ridge lambda:", best_lambda_ridge, "\n")
cat("Best Lasso lambda:", best_lambda_lasso, "\n")
```

### Real-World Example: Gene Expression Analysis

```r
# Gene expression data with many predictors
set.seed(123)
n_genes <- 500
n_samples <- 100

# Simulate gene expression data
gene_data <- data.frame(
  gene expression = matrix(rnorm(n_samples * n_genes), n_samples, n_genes)
)

# Create outcome (disease presence)
true_effect_genes <- c(1, 25, 50, 100, 200)  # Truly influential genes
gene_data$disease <- 0
for (i in true_effect_genes) {
  gene_data$disease <- gene_data$disease + gene_data[, i] * 0.5
}
gene_data$disease <- ifelse(gene_data$disease > 0, 1, 0)

# Prepare features and target
X <- as.matrix(gene_data[, -which(names(gene_data) == "disease")])
y <- gene_data$disease

# Fit Lasso for binary classification
lasso_class <- glmnet(X, y, family = "binomial", alpha = 1)

# Cross-validation
cv_lasso_class <- cv.glmnet(X, y, family = "binomial", alpha = 1)

# Find significant genes (non-zero coefficients)
best_model <- glmnet(X, y, family = "binomial", alpha = 1, 
                    lambda = cv_lasso_class$lambda.min)
coef_matrix <- as.matrix(coef(best_model))
significant_genes <- which(coef_matrix != 0) - 1  # Adjust for intercept

cat("Number of significant genes:", length(significant_genes), "\n")
cat("True influential genes found:", 
    sum(significant_genes %in% true_effect_genes), "\n")
```

### Real-World Example: House Price Prediction with Elastic Net

```r
# Using the housing dataset from earlier
set.seed(42)
n <- 300

# Create comprehensive housing features
housing <- data.frame(
  sqft = runif(n, 1000, 4000),
  bedrooms = sample(1:6, n, replace = TRUE),
  bathrooms = sample(1:4, n, replace = TRUE),
  age = runif(n, 1, 50),
  location_score = runif(n, 1, 10),
  school_rating = sample(1:10, n, replace = TRUE),
  crime_rate = runif(n, 1, 100),
  noise_level = sample(1:10, n, replace = TRUE),
  distance_to_work = runif(n, 1, 50),
  property_tax = runif(n, 2000, 15000)
)

# Add some correlated features
housing$sqft_per_bed <- housing$sqft / housing$bedrooms
housing$total_rooms <- housing$bedrooms + housing$bathrooms

# Create price with complex relationships
housing$price <- (
  100000 +
  150 * housing$sqft +
  15000 * housing$bedrooms -
  500 * housing$age +
  5000 * housing$location_score +
  3000 * housing$school_rating -
  200 * housing$crime_rate +
  0 * housing$noise_level -
  100 * housing$distance_to_work +
  5 * housing$property_tax +
  rnorm(n, 0, 20000)
)

# Prepare data for glmnet
X <- as.matrix(housing[, -which(names(housing) == "price")])
y <- housing$price

# Fit Elastic Net with different alpha values
# alpha = 0.5 means equal mix of L1 and L2
elastic_model <- glmnet(X, y, alpha = 0.5)

# Cross-validation to find best lambda
cv_elastic <- cv.glmnet(X, y, alpha = 0.5)

# Get best model
best_lambda <- cv_elastic$lambda.min
final_model <- glmnet(X, y, alpha = 0.5, lambda = best_lambda)

# Extract and display coefficients
cat("=== Elastic Net Coefficients ===\n")
coef_matrix <- as.data.frame(as.matrix(coef(final_model)))
coef_matrix <- coef_matrix[coef_matrix[, 1] != 0, , drop = FALSE]
print(coef_matrix)

cat("\nNumber of features used:", nrow(coef_matrix) - 1, "\n")
cat("Total features:", ncol(X), "\n")

# Compare with OLS (no regularization)
ols_model <- lm(price ~ ., data = housing)
cat("\nOLS uses all", ncol(X), "features\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Standardize predictors** before regularization (glmnet does this automatically)
2. **Use cross-validation** to select optimal lambda
3. **Start with Elastic Net** when uncertain about the best approach
4. **Consider domain knowledge** when interpreting coefficients

### Common Pitfalls

1. Not scaling features (penalty applies equally to all coefficients)
2. Using default lambda instead of cross-validated lambda
3. Ignoring multicollinearity effects on coefficient stability

## Performance Considerations

- **glmnet** uses efficient coordinate descent algorithm
- For very large datasets, consider `biglasso` package
- Sparse models (Lasso) are more memory-efficient

## Related Concepts

- **LARS**: Another algorithm for Lasso
- **Bayesian Lasso**: Probabilistic interpretation
- **Group Lasso**: For grouped variables

## Exercise Problems

1. Compare Ridge, Lasso, and Elastic Net on the mtcars dataset
2. Use cross-validation to find optimal lambda for each method
3. Interpret the coefficients and identify important predictors