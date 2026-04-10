# Feature Scaling Methods

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand why feature scaling is important for machine learning
- Implement standardization (z-score normalization)
- Apply min-max normalization
- Use robust scaling methods
- Choose appropriate scaling method for different algorithms
- Avoid data leakage when scaling

## Theoretical Background

Feature scaling transforms features to have similar scales. Many machine learning algorithms perform better or require scaling to work correctly. Understanding when and how to scale features is crucial for building effective models.

### Why Feature Scaling Matters

1. **Distance-Based Algorithms**: KNN, SVM, K-means use distance calculations - features with larger ranges dominate
2. **Gradient Descent**: Faster convergence when features are on similar scales
3. **Regularization**: Penalizes coefficients equally - features must be comparable
4. **Neural Networks**: Faster and more stable training with scaled inputs

### When Scaling Is Critical

- **K-Nearest Neighbors**: Uses Euclidean distance
- **Support Vector Machines**: Kernel-based methods
- **K-Means Clustering**: Distance-based
- **Logistic/Linear Regression**: Regularization effects
- **Neural Networks**: Gradient descent optimization

### When Scaling May Not Be Needed

- **Decision Trees**: Based on thresholds, not distances
- **Random Forest**: Ensemble of trees, not affected by scale
- **Naive Bayes**: Based on probability distributions

### Common Scaling Methods

#### Standardization (Z-Score)

Transforms features to have zero mean and unit variance:

$$x_{scaled} = \frac{x - \mu}{\sigma}$$

- Preserves outliers better than min-max
- Good for data with Gaussian distribution
- Range: Not bounded, can be negative

#### Min-Max Normalization

Scales features to a specific range (typically [0, 1]):

$$x_{scaled} = \frac{x - x_{min}}{x_{max} - x_{min}}$$

- Bounded range
- Sensitive to outliers
- Good for bounded algorithms like neural networks

#### Robust Scaling

Uses median and interquartile range:

$$x_{scaled} = \frac{x - median}{IQR}$$

- Robust to outliers
- Good for data with extreme values

#### Max Abs Scaling

Scales each feature by its maximum absolute value:

$$x_{scaled} = \frac{x}{|x_{max}|}$$

- Preserves sign
- Range: [-1, 1]
- Good for sparse data

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("caret")
install.packages("scales")

library(caret)
library(scales)
```

### Step 2: Prepare Sample Data

```r
# Create sample data with different scales
set.seed(42)
n <- 100

sample_data <- data.frame(
  age = rnorm(n, 40, 10),           # range: ~20-60
  salary = rnorm(n, 50000, 15000),  # range: ~20000-80000
  score = rnorm(n, 75, 10),         # range: ~45-105
  height = rnorm(n, 170, 10)        # range: ~150-190
)

# Check ranges before scaling
cat("Original feature ranges:\n")
apply(sample_data, 2, function(x) {
  c(min = min(x), max = max(x), mean = mean(x), sd = sd(x))
})
```

### Step 3: Standardization (Z-Score)

```r
# Method 1: Using base R
standardize <- function(x) {
  (x - mean(x)) / sd(x)
}

sample_standardized <- as.data.frame(
  lapply(sample_data, standardize)
)

cat("After standardization:\n")
apply(sample_standardized, 2, function(x) {
  c(min = min(x), max = max(x), mean = mean(x), sd = sd(x))
})

# Method 2: Using caret
standardized <- preProcess(sample_data, method = c("center", "scale"))
sample_scaled <- predict(standardized, sample_data)

# Verify
cat("\nUsing caret (center + scale):\n")
print(summary(sample_scaled))
```

### Step 4: Min-Max Normalization

```r
# Method 1: Using base R
min_max_normalize <- function(x) {
  (x - min(x)) / (max(x) - min(x))
}

sample_minmax <- as.data.frame(
  lapply(sample_data, min_max_normalize)
)

cat("After min-max normalization:\n")
apply(sample_minmax, 2, function(x) {
  c(min = min(x), max = max(x))
})

# Method 2: Using caret with custom range
minmax_prep <- preProcess(sample_data, method = "range", range = c(0, 1))
sample_minmax_caret <- predict(minmax_prep, sample_data)

cat("\nUsing caret (range [0,1]):\n")
print(summary(sample_minmax_caret))
```

### Step 5: Robust Scaling

```r
# Using robust scaling with caret
robust_prep <- preProcess(sample_data, method = "robust Scaler")
sample_robust <- predict(robust_prep, sample_data)

cat("After robust scaling:\n")
print(summary(sample_robust))

# Verify median is 0 and IQR is 1
apply(sample_robust, 2, function(x) {
  c(median = median(x), IQR = IQR(x))
})
```

### Step 6: Scaling Inside Cross-Validation

```r
# Correct way: scale inside CV
set.seed(42)

# Define training control with preprocessing
train_control <- trainControl(
  method = "cv",
  number = 5,
  preProcess = c("center", "scale")
)

# Train model - preprocessing will be applied inside CV
model <- train(
  Species ~ .,
  data = iris,
  method = "knn",
  trControl = train_control,
  tuneGrid = data.frame(k = 5)
)

print(model)
```

## Code Examples

### Example 1: Scaling for KNN Classification

This example demonstrates the impact of scaling on KNN.

```r
# Create data with different scales
set.seed(123)
n <- 200

knn_data <- data.frame(
  age = rnorm(n, 40, 10),           # Large scale
  income = rnorm(n, 50000, 15000),  # Very large scale
  satisfaction_score = runif(n, 1, 10),  # Small scale
  class = factor(sample(c("A", "B"), n, replace = TRUE))
)

# Add relationship
knn_data$class[knn_data$income > 55000] <- "A"
knn_data$class[knn_data$satisfaction_score < 3] <- "B"

# Split
train_idx <- sample(1:n, n * 0.7)
train_knn <- knn_data[train_idx, ]
test_knn <- knn_data[-train_idx, ]

# Train without scaling
model_unscaled <- train(
  class ~ .,
  data = train_knn,
  method = "knn",
  tuneGrid = data.frame(k = 5)
)

# Train with scaling
model_scaled <- train(
  class ~ .,
  data = train_knn,
  method = "knn",
  preProcess = c("center", "scale"),
  tuneGrid = data.frame(k = 5)
)

# Compare
pred_unscaled <- predict(model_unscaled, test_knn)
pred_scaled <- predict(model_scaled, test_knn)

cat("Without scaling accuracy:", 
    sum(pred_unscaled == test_knn$class) / nrow(test_knn), "\n")
cat("With scaling accuracy:", 
    sum(pred_scaled == test_knn$class) / nrow(test_knn), "\n")
```

### Example 2: Scaling for SVM

This example shows scaling for SVM classification.

```r
# Create complex data
set.seed(456)
n <- 300

svm_data <- data.frame(
  x1 = rnorm(n),
  x2 = rnorm(n),
  x3 = rnorm(n, 100, 50),  # Different scale
  y = factor(ifelse(rnorm(n) > 0, "pos", "neg"))
)

# Add non-linear relationship
svm_data$y[svm_data$x3 > 100] <- "pos"

# Split
train_idx <- sample(1:n, n * 0.7)
train_svm <- svm_data[train_idx, ]
test_svm <- svm_data[-train_idx, ]

# Scale features using training set statistics
scaler <- preProcess(train_svm[, -4], method = c("center", "scale"))
train_svm_scaled <- predict(scaler, train_svm[, -4])
train_svm_scaled$y <- train_svm$y

test_svm_scaled <- predict(scaler, test_svm[, -4])
test_svm_scaled$y <- test_svm$y

# Train SVM on scaled data
model_svm <- svm(y ~ ., data = train_svm_scaled, kernel = "radial")

# Predictions
svm_pred <- predict(model_svm, test_svm_scaled)

# Confusion matrix
cm_svm <- confusionMatrix(svm_pred, test_svm_scaled$y)
print(cm_svm)
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Always Scale Training Data First**: Compute parameters on training, apply to test
2. **Use Same Parameters**: Apply training set statistics to all new data
3. **Scale Inside CV**: For proper evaluation, scale inside cross-validation
4. **Check Feature Ranges**: After scaling, verify ranges are appropriate
5. **Consider Algorithm**: Choose scaling method based on downstream algorithm

### Common Pitfalls

1. **Scaling After Split**: Computing scaling on entire dataset causes leakage
2. **Different Parameters**: Using different mean/std for train and test
3. **Outliers**: Min-max is sensitive to outliers
4. **Not Scaling at All**: Forgetting to scale when needed

## Performance Considerations

### Method Selection Guidelines

| Algorithm | Recommended Scaling | Why |
|-----------|-------------------|-----|
| KNN | Standardization | Distance-based |
| SVM | Standardization | Distance-based |
| K-Means | Standardization | Distance-based |
| Logistic Regression | Standardization | Regularization |
| Neural Networks | Min-Max [0,1] | Activation functions |
| Decision Trees | Not needed | Threshold-based |
| Random Forest | Not needed | Tree-based |

### Computational Notes

- Scaling is O(n × p) - linear in data size
- Caching scaled values recommended for large data
- Use double precision for accuracy

## Related Concepts

- **Normalization**: Alternative to scaling
- **Feature Selection**: Often combined with scaling
- **Data Leakage**: Proper scaling prevents leakage
- **Transformation**: Log, Box-Cox for skewed data

## Exercise Problems

1. **Basic**: Apply standardization to a dataset.

2. **Intermediate**: Compare different scaling methods.

3. **Advanced**: Scale features inside cross-validation.

4. **Real-World Challenge**: Scale data for KNN and compare results.

5. **Extension**: Implement custom scaling for specific features.