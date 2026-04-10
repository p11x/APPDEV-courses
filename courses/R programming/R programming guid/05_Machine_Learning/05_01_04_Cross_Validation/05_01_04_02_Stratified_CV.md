# Stratified Cross-Validation

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the importance of stratified sampling in cross-validation
- Implement stratified k-fold cross-validation in R
- Apply stratified CV to imbalanced classification problems
- Compare stratified vs. non-stratified cross-validation
- Handle multi-class stratification
- Use caret's `stratified` option for splitting data

## Theoretical Background

Standard k-fold cross-validation randomly splits data into k folds. However, when dealing with classification problems, especially those with imbalanced class distributions, random splitting can result in folds that don't represent the overall class distribution. This can lead to unreliable performance estimates.

### The Problem with Random Splitting

In standard k-fold CV:
- Some folds may have very few samples of minority class
- Some folds may have no samples of certain class entirely
- Performance estimates become unreliable due to high variance

For example, with a 90%/10% class distribution in k=10 folds:
- One fold might have 0% minority class, another 30%
- This leads to highly variable accuracy estimates

### Stratified K-Fold Cross-Validation

Stratified k-fold CV maintains the original class distribution in each fold:
- Each fold has approximately the same percentage of each class as the full dataset
- For multi-class problems, stratified sampling is applied to each class
- Results in more reliable and less variable performance estimates

### When to Use Stratified CV

1. **Imbalanced Classification**: Always use stratified CV
2. **Multi-class Problems**: Recommended when class sizes are unequal
3. **Rare Classes**: Critical when some classes have very few samples
4. **Cost-sensitive Learning**: For problems with uneven error costs

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("caret")
install.packages("e1071")

library(caret)
library(e1071)
```

### Step 2: Create Stratified Splits

```r
# Load data with class imbalance
data(iris)

# Check class distribution
table(iris$Species)
# setosa versicolor virginica 
#       50         50        50

# For imbalanced data, use Pima Indians dataset
data(PimaIndiansDiabetes, package = "mlbench")
pima <- PimaIndiansDiabetes
colnames(pima)[9] <- "Outcome"

# Check class distribution
table(pima$Outcome)
# neg pos 
# 500 268 
# More negative than positive

# Create stratified split manually
set.seed(42)

# Using createDataPartition with strata
strat_split <- createDataPartition(
  y = pima$Outcome,
  p = 0.7,
  list = FALSE
)

train_data <- pima[strat_split, ]
test_data <- pima[-strat_split, ]

# Verify stratification
cat("Original distribution:\n")
print(prop.table(table(pima$Outcome)))
cat("\nTraining distribution:\n")
print(prop.table(table(train_data$Outcome)))
cat("\nTest distribution:\n")
print(prop.table(table(test_data$Outcome)))
```

### Step 3: Stratified K-Fold CV

```r
# Define stratified training control
set.seed(42)

train_control_strat <- trainControl(
  method = "cv",
  number = 10,
  classProbs = TRUE,
  savePredictions = TRUE
)

# Train model with stratified CV on imbalanced data
model_strat <- train(
  Outcome ~ .,
  data = pima,
  method = "glm",
  family = "binomial",
  trControl = train_control_strat
)

# View results
print(model_strat)
# Resampling results
#         Accuracy   Kappa    
# 1      0.792     0.506    
# 2      0.792     0.506
# ... all similar because of stratification

# Check predictions are stratified
strat_pred <- model_strat$pred
head(strat_pred)
```

### Step 4: Compare Stratified vs Non-Stratified

```r
# Non-stratified CV
set.seed(42)

train_control_simple <- trainControl(
  method = "cv",
  number = 10
)

# Create simple non-stratified folds using manual splitting
folds_simple <- createFolds(
  y = pima$Outcome,
  k = 10,
  list = TRUE,
  returnTrain = FALSE
)

# Train and record each fold's class distribution
fold_dist <- lapply(folds_simple, function(fold_idx) {
  train_fold <- pima[-fold_idx, ]
  prop.table(table(train_fold$Outcome))
})

# Print class proportions in each fold
cat("Class proportions in each fold:\n")
print(do.call(rbind, fold_dist))

# Compare with stratified
set.seed(42)
folds_strat <- createFolds(
  y = pima$Outcome,
  k = 10,
  list = TRUE,
  returnTrain = FALSE,
  strat = TRUE
)

fold_dist_strat <- lapply(folds_strat, function(fold_idx) {
  train_fold <- pima[-fold_idx, ]
  prop.table(table(train_fold$Outcome))
})

cat("\nStratified class proportions:\n")
print(do.call(rbind, fold_dist_strat))
```

### Step 5: Multi-class Stratification

```r
# For multi-class problem, stratification maintains each class proportion
set.seed(42)

# Create multi-class data with imbalance
set.seed(123)
n <- 200
multi_data <- data.frame(
  x1 = rnorm(n),
  x2 = rnorm(n),
  x3 = rnorm(n),
  class = factor(c(rep("A", 100), rep("B", 70), rep("C", 30)))
)

# Check class distribution
table(multi_data$class)

# Create stratified folds
strat_folds <- createMultiFolds(
  y = multi_data$class,
  k = 5,
  times = 1
)

# Verify stratification
fold_sizes <- lapply(strat_folds, function(fold) {
  train_fold <- multi_data[fold, ]
  prop.table(table(train_fold$class))
})

cat("\nMulti-class stratified folds:\n")
print(do.call(rbind, fold_sizes))
```

## Code Examples

### Example 1: Imbalanced Credit Card Fraud

This example demonstrates stratified CV for fraud detection.

```r
# Simulate fraud detection data
set.seed(789)
n <- 5000

# 1% fraud rate
fraud_data <- data.frame(
  amount = rnorm(n, 100, 50),
  time = runif(n, 0, 24),
  merchant_type = factor(sample(1:5, n, replace = TRUE)),
  is_fraud = factor(c(rep("no", 4900), rep("yes", 100)))
)

# Set seed
set.seed(42)

# Stratified CV
ctrl_strat <- trainControl(
  method = "cv",
  number = 5,
  classProbs = TRUE,
  summaryFunction = twoClassSummary
)

# Train with stratified CV
fraud_model <- train(
  is_fraud ~ .,
  data = fraud_data,
  method = "glm",
  family = "binomial",
  trControl = ctrl_strat,
  metric = "ROC"
)

# View results
print(fraud_model)
# Each fold has similar class distribution
# ROC values are more stable
```

### Example 2: Rare Disease Diagnosis

This example shows stratified CV for medical diagnosis with rare disease.

```r
# Create rare disease data
set.seed(456)
n <- 1000

# 2% disease prevalence
disease_data <- data.frame(
  age = rnorm(n, 50, 15),
  bmi = rnorm(n, 28, 5),
  blood_pressure = rnorm(n, 130, 20),
  diagnosis = factor(c(rep("healthy", 980), rep("disease", 20)))
)

# Class distribution
prop.table(table(disease_data$diagnosis))

# Stratified CV with multiple repeats for stability
ctrl_strat <- trainControl(
  method = "repeatedcv",
  number = 5,
  repeats = 3,
  classProbs = TRUE,
  summaryFunction = twoClassSummary
)

# Train
set.seed(42)
disease_model <- train(
  diagnosis ~ .,
  data = disease_data,
  method = "rf",
  trControl = ctrl_strat,
  tuneGrid = data.frame(mtry = 2),
  metric = "ROC"
)

# View results
print(disease_model)
# Stratified fold distributions ensure consistent evaluation
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Always Stratify**: When dealing with classification, always use stratified CV
2. **Check Fold Distribution**: Always verify class proportions after stratification
3. **For Multi-class**: Use createMultiFolds for multi-class stratification
4. **Set Seeds**: Always set seeds for reproducibility with stratification

### Common Pitfalls

1. **Not Using Stratified CV**: Leading to unreliable estimates
2. **Too Few_samples**: When some classes have very few samples
3. **Ignoring Stratification in createFolds**: Not using the strat option
4. **Forgetting classProbs**: For binary problems, need for certain metrics

## Performance Considerations

### When Stratification is Critical

- Class imbalance ratio > 10:1
- Small sample size (< 100 per class)
- Rare events (<5% prevalence)
- Cost-sensitive classification

### Computational Notes

- Stratified CV is slightly more expensive
- Memory and time overhead is minimal
- Should always be preferred for classification

## Related Concepts

- **createMultiFolds**: For multi-class stratification
- **createStratifiedFolds**: Alternative approach
- **Up sampling/Down sampling**: For extreme imbalance before CV
- **SMOTE**: Synthetic minority oversampling

## Exercise Problems

1. **Basic**: Create stratified folds for imbalanced data.

2. **Intermediate**: Compare stratified vs non-stratified CV on imbalanced data.

3. **Advanced**: Implement stratified CV for multi-class problem.

4. **Real-World Challenge**: Use stratified CV for fraud detection.

5. **Extension**: Combine stratified CV with class weights.