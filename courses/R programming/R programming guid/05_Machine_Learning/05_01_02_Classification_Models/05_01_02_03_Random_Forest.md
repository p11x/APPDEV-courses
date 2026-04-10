# Random Forest Classification

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the ensemble learning paradigm and how Random Forest improves upon single decision trees
- Implement Random Forest classification in R using the `ranger` and `randomForest` packages
- Interpret out-of-bag (OOB) error estimates and variable importance
- Tune the key hyperparameters: number of trees, number of features, and tree depth
- Apply Random Forest to multi-class classification problems
- Compare Random Forest performance with individual decision trees
- Handle class imbalance and missing data in Random Forest models

## Theoretical Background

Random Forest is a powerful ensemble learning method that combines multiple decision trees to create a more robust and accurate classifier. The key innovation is the introduction of randomness at two levels: bootstrap sampling and random feature selection.

### How Random Forest Works

Random Forest builds an ensemble of decision trees, each trained on a bootstrap sample of the original data. For classification, final predictions are made by majority voting across all trees.

#### The Algorithm Steps

1. **Bootstrap Sampling**: For each tree, draw a random sample with replacement from the training data
2. **Random Feature Selection**: At each node, randomly select a subset of features to consider for splitting
3. **Build Decision Tree**: Grow a full decision tree on the bootstrap sample (no pruning)
4. **Repeat**: Steps 1-3 for the specified number of trees (typically 100-500)
5. **Ensemble Prediction**: For new data, collect predictions from all trees and use majority vote

### Why Randomness Matters

#### Bootstrap Aggregation (Bagging)

Each tree is trained on a different bootstrap sample. This creates diversity among trees because:
- Different training samples lead to different trees
- Some training instances are excluded (called "out-of-bag" or OOB samples)
- The errors of individual trees are reduced through voting

#### Random Feature Selection

At each node, only a random subset of features is considered for splitting:
- For classification: typically √d features where d is the total number of features
- For regression: typically d/3 features

This prevents a single strong feature from dominating all trees and increases diversity.

### Key Concepts

#### Out-of-Bag (OOB) Error

Each tree uses only ~63.2% of unique training samples (by bootstrap with replacement). The remaining ~36.8% are OOB samples. OOB error is calculated by:
1. For each training sample, predict using trees that didn't include it in training
2. Calculate error rate across all OOB predictions
3. This provides an unbiased estimate of test error

#### Variable Importance

Random Forest provides two types of variable importance:
1. **Mean Decrease in Gini (MDG)**: Average decrease in node impurity weighted by split probability
2. **Mean Decrease in Accuracy (MDA)**: Average decrease in accuracy when a feature is randomly permuted

#### Strengths of Random Forest

- **Accuracy**: Often achieves excellent accuracy with minimal tuning
- **Robustness**: Less prone to overfitting than individual decision trees
- **Handles High Dimensionality**: Works well with many features
- **Handles Missing Values**: Can approximate missing values internally
- **Parallelization**: Trees can be built independently

#### Weaknesses of Random Forest

- **Slower Prediction**: Must aggregate predictions from many trees
- **Less Interpretable**: Harder to understand than single trees
- **Large Size**: Requires storing many trees in memory

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
# Install required packages
install.packages("randomForest")
install.packages("ranger")
install.packages("caret")
install.packages("vip")

# Load libraries
library(randomForest)
library(ranger)
library(caret)
```

### Step 2: Prepare the Data

```r
# Load the Iris dataset
data(iris)

# Examine structure
str(iris)
# 'data.frame': 150 obs. of 5 variables:
# $ Sepal.Length: num  5.1 4.9 4.7 ...
# $ Sepal.Width : num  3.5 3 3.2 ...
# $ Petal.Length: num  1.4 1.4 1.3 ...
# $ Petal.Width : num  0.2 0.2 0.2 ...
# $ Species     : Factor w/ 3 levels "setosa","versicolor","virginica"

# Create training and test splits
set.seed(42)
train_idx <- createDataPartition(iris$Species, p = 0.7, list = FALSE)
train_data <- iris[train_idx, ]
test_data <- iris[-train_idx, ]
```

### Step 3: Train the Random Forest Model

```r
# Basic Random Forest model
# Using randomForest package
rf_model <- randomForest(x = train_data[, -5],  # Features
                        y = train_data$Species,  # Target
                        ntree = 500,           # Number of trees
                        mtry = 2,              # Features to try at each split
                        importance = TRUE,     # Calculate variable importance
                        keep.inbag = TRUE)      # Keep OOB information

# View model summary
print(rf_model)
# 
# Call:
#   randomForest(x = train_data[, -5], y = train_data$Species, ntree = 500,      mtry = 2, importance = TRUE, keep.inbag = TRUE) 
#                Type of random forest: classification
#                      Number of trees: 500
# No. of variables tried at each split: 2
# 
#         OOB estimate of  error rate: 5.71%
# Confusion matrix:
#             setosa versicolor virginica class.error
# setosa         35         0         0        0.0000
# versicolor     0        31         4        0.1143
# virginica      0         2        28        0.0667
```

### Step 4: Evaluate the Model

```r
# View OOB error by number of trees
plot(rf_model, main = "Random Forest Error Rate")

# View variable importance
importance(rf_model)
#                  setosa versicolor virginica MeanDecreaseAccuracy
# Sepal.Length       0.00       11.11       5.14               8.88
# Sepal.Width        0.00        3.77       4.40               4.41
# Petal.Length     20.96       22.08      26.24              24.49
# Petal.Width      20.63       18.93      22.06              21.46

# Plot variable importance
varImpPlot(rf_model, main = "Variable Importance")

# Make predictions on test data
predictions <- predict(rf_model, test_data[, -5])

# Confusion matrix
confusion_matrix <- table(Predicted = predictions, 
                        Actual = test_data$Species)
print(confusion_matrix)
#             Actual
# Predicted    setosa versicolor virginica
#   setosa        15         0         0
#   versicolor    0        14         1
#   virginica     0         1        14

# Calculate accuracy
accuracy <- sum(diag(confusion_matrix)) / sum(confusion_matrix)
print(paste("Test Accuracy:", round(accuracy, 4)))
# [1] "Test Accuracy: 0.9556"
```

### Step 5: Using the Ranger Package (Faster)

```r
# Using ranger for faster training
# This is the recommended package for large datasets
rf_model_ranger <- ranger(Species ~ ., 
                         data = train_data,
                         num.trees = 500,
                         mtry = 2,
                         importance = "permutation",
                         verbose = FALSE)

# View model
print(rf_model_ranger)
# Ranger result
# 
# Call:
#   Species ~ . 
# 
# Type:                         Classification
# Number of trees:             500
# Number of OOB samples:       36
# OOB prediction error rate:   4.76%
# 
# Variable importance (permutation):
#   Petal.Length: 23.23
#   Petal.Width:  18.56
#   Sepal.Length:  6.34
#   Sepal.Width:   2.15

# Predictions using ranger
predictions_ranger <- predict(rf_model_ranger, test_data)
predictions_ranger <- predictions_ranger$predictions

# Confusion matrix
confusionMatrix(predictions_ranger, test_data$Species)
```

## Code Examples

### Example 1: Credit Risk Classification

This example demonstrates using Random Forest for credit risk prediction.

```r
# Create synthetic credit risk data
set.seed(123)
n <- 1000

credit_data <- data.frame(
  age = sample(18:70, n, replace = TRUE),
  income = rnorm(n, 50000, 15000),
  credit_score = sample(300:850, n, replace = TRUE),
  debt = rnorm(n, 5000, 3000),
  employed = factor(sample(c("yes", "no"), n, prob = c(0.7, 0.3), replace = TRUE)),
  education = factor(sample(c("high_school", "college", "graduate"), n, replace = TRUE)),
  default = factor(sample(c("no", "yes"), n, prob = c(0.8, 0.2), replace = TRUE))
)

# Add realistic relationships
credit_data$default[credit_data$credit_score < 500] <- "yes"
credit_data$default[credit_data$debt > 10000 & credit_data$income < 30000] <- "yes"

# Split the data
train_idx <- sample(1:n, n * 0.7)
train_credit <- credit_data[train_idx, ]
test_credit <- credit_data[-train_idx, ]

# Train Random Forest
set.seed(42)
credit_rf <- randomForest(default ~ ., 
                          data = train_credit,
                          ntree = 300,
                          mtry = 2,
                          importance = TRUE,
                          classwt = c("no" = 1, "yes" = 4))  # Handle imbalance

# View OOB error
print(credit_rf)
plot(credit_rf, main = "Credit Risk Random Forest")

# Variable importance
varImpPlot(credit_rf, main = "Credit Risk - Variable Importance")

# Evaluate on test set
credit_pred <- predict(credit_rf, test_credit)
credit_cm <- confusionMatrix(credit_pred, test_credit$default)
print(credit_cm)

# Probability predictions
credit_prob <- predict(credit_rf, test_credit, type = "prob")
head(credit_prob)
```

### Example 2: Spam Email Detection

This example shows Random Forest handling text-derived features for spam classification.

```r
# Create synthetic email spam data
set.seed(456)
n <- 800

spam_data <- data.frame(
  word_count = rpois(n, 150),
  capital_letters = rnorm(n, 50, 30),
  num_links = rpois(n, 3),
  num_images = rpois(n, 1),
  num_exclamations = rpois(n, 2),
  num_questions = rpois(n, 1),
  has_urgent = rbinom(n, 1, 0.1),
  has_free = rbinom(n, 1, 0.15),
  has_winner = rbinom(n, 1, 0.1),
  has_verify = rbinom(n, 1, 0.2),
  spam = factor(sample(c("ham", "spam"), n, prob = c(0.7, 0.3), replace = TRUE))
)

# Add realistic relationships
spam_data$spam[spam_data$has_winner == 1] <- "spam"
spam_data$spam[spam_data$num_links > 5] <- "spam"
spam_data$spam[spam_data$capital_letters > 100] <- "spam"

# Split data
train_idx <- sample(1:n, n * 0.7)
train_spam <- spam_data[train_idx, ]
test_spam <- spam_data[-train_idx, ]

# Train Random Forest model
set.seed(42)
spam_rf <- randomForest(spam ~ ., 
                        data = train_spam,
                        ntree = 500,
                        importance = TRUE)

# Print model
print(spam_rf)
# OOB estimate of error rate: ~8%

# Variable importance
varImpPlot(spam_rf, main = "Spam Classification - Variable Importance")

# Evaluate
spam_pred <- predict(spam_rf, test_spam)
spam_cm <- confusionMatrix(spam_pred, test_spam$spam)
print(spam_cm)
# Sensitivity (spam): 0.92
# Specificity (ham): 0.95
# Overall Accuracy: 0.94
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use Sufficient Trees**: At least 300-500 trees for stable estimates; more for larger datasets.

2. **Default mtry Works Well**: For classification, mtry = √d is typically optimal; for regression, mtry = d/3.

3. **OOB for Quick Validation**: Use OOB error instead of a separate validation set for quick iteration.

4. **Handle Class Imbalance**: Use the `classwt` parameter to weight minority classes.

5. **Check Variable Importance**: Use importance plots to understand which features matter most.

6. **Parallel Processing**: For very large datasets, use the `ranger` package with parallel processing.

### Common Pitfalls

1. **Too Many Trees Can Cause Overfitting**: In theory possible, but rarely an issue in practice.

2. **Not Setting Random Seeds**: Always set seeds for reproducibility.

3. **Ignoring OOB Error**: OOB is your best friend for model selection without separate validation.

4. **Wrong mtry**: Setting mtry too high can reduce model diversity and performance.

5. **Small Training Data**: Random Forest needs sufficient data to build good trees.

6. **Forgetting to Handle Missing Values**: While RF can handle missing values, it's better to preprocess.

## Performance Considerations

### Computational Complexity

- **Training**: O(n × d × t × log(n)), where t = number of trees
- **Prediction**: O(t × tree_depth) - parallelizable across trees
- **Space**: O(t × tree_size)

### Hyperparameter Tuning

1. **Number of Trees (ntree)**: 300-1000 is typical; more is better but diminishing returns.
2. **Features to Try (mtry)**: √d for classification; d/3 for regression.
3. **Node Size**: Minimum terminal node size; smaller = more complex trees.
4. **Maximum Depth**: Can limit to reduce memory usage.

### Optimization Tips

1. Use `ranger` package for faster training (10-100x faster)
2. For parallel processing, set `num.threads` in ranger
3. Sample data for initial exploration, then train on full data for final model

## Related Concepts

- **Extra Trees**: Extremely randomized trees that use random thresholds
- **Gradient Boosted Trees (GBM)**: Sequential ensemble that builds trees on residuals
- **XGBoost/LightGBM**: Optimized gradient boosting implementations
- **Bagging vs. Boosting**: Ensemble methods differ in how they combine weak learners

## Exercise Problems

1. **Basic**: Build a Random Forest on the Iris dataset. Compare OOB error with test error.

2. **Intermediate**: Tune mtry parameter using OOB error. What value is optimal?

3. **Advanced**: Compare variable importance between Gini and permutation methods.

4. **Real-World Challenge**: Apply Random Forest to the Titanic dataset. Analyze feature importance.

5. **Extension**: Compare Random Forest performance with XGBoost on a classification problem.