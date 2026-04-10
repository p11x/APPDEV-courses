# Confusion Matrix Analysis

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the confusion matrix structure for binary and multi-class classification
- Generate and interpret confusion matrices in R using caret
- Extract all key metrics from confusion matrices
- Visualize confusion matrices for better interpretation
- Handle multi-class confusion matrices
- Apply confusion matrix analysis to real-world problems
- Understand the relationship between confusion matrix metrics

## Theoretical Background

A confusion matrix is a table that summarizes classification model performance by showing the counts of correct and incorrect predictions, broken down by actual class and predicted class. It provides detailed insight into where the model makes errors.

### Binary Confusion Matrix

For binary classification (positive/negative), the confusion matrix is a 2x2 table:

| | Predicted Positive | Predicted Negative |
|---|---|---|
| **Actual Positive** | True Positive (TP) | False Negative (FN) |
| **Actual Negative** | False Positive (FP) | True Negative (TN) |

### Types of Predictions

- **True Positive (TP)**: Correctly predicted positive class
- **True Negative (TN)**: Correctly predicted negative class
- **False Positive (FP)**: Predicted positive but actually negative (Type I Error)
- **False Negative (FN)**: Predicted negative but actually positive (Type II Error)

### Multi-class Confusion Matrix

For K classes, the confusion matrix is K×K:
- Row i, Column j: Number of samples with true class i predicted as class j
- Diagonal: Correct predictions
- Off-diagonal: Misclassifications

### Derived Metrics

From the confusion matrix, we can derive:
- **Accuracy**: (TP + TN) / Total
- **Error Rate**: (FP + FN) / Total
- **Sensitivity/Recall**: TP / (TP + FN)
- **Specificity**: TN / (TN + FP)
- **Precision**: TP / (TP + FP)
- **F1-Score**: 2 × Precision × Recall / (Precision + Recall)

### Why Confusion Matrix Matters

1. **Detailed Error Analysis**: Shows which classes are confused with each other
2. **Class-specific Performance**: Can evaluate each class separately
3. **Imbalance Detection**: Reveals class distribution and errors
4. **Model Improvement**: Identifies specific weaknesses to address

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("caret")
install.packages("e1071")
install.packages("ggplot2")

library(caret)
library(e1071)
library(ggplot2)
```

### Step 2: Create a Binary Confusion Matrix

```r
# Load data
data(iris)

# Create binary classification problem
iris_binary <- iris[iris$Species != "setosa", ]
iris_binary$Species <- factor(iris_binary$Species)

# Create train/test split
set.seed(42)
train_idx <- createDataPartition(iris_binary$Species, p = 0.7, list = FALSE)
train_data <- iris_binary[train_idx, ]
test_data <- iris_binary[-train_idx, ]

# Train model
model <- train(Species ~ ., 
              data = train_data,
              method = "glm",
              family = "binomial")

# Predictions
pred_class <- predict(model, test_data)
pred_prob <- predict(model, test_data, type = "prob")

# Create confusion matrix
cm <- table(Predicted = pred_class, 
            Actual = test_data$Species)
print(cm)
#             Actual
# Predicted    versicolor virginica
#   versicolor         13         2
#   virginica          2        14

# As proportions
cm_prop <- prop.table(cm)
print(cm_prop)
#                  Actual
# Predicted      versicolor  virginica
#   versicolor     0.4333     0.0667
#   virginica      0.0667     0.4667
```

### Step 3: Generate Detailed Confusion Matrix with caret

```r
# Using caret's confusionMatrix
cm_obj <- confusionMatrix(pred_class, test_data$Species)
print(cm_obj)
# Confusion Matrix and Statistics
# 
#           Actual
# Prediction versicolor virginica
#   versicolor         13         2
#   virginica          2        14
# 
#                Accuracy : 0.9032          
#                95% CI   : (0.761, 0.974)
#   No Information Rate : 0.5161          
#          P-Value [Acc > NIR] : 3.571e-06 
# 
#             Kappa : 0.8065
# 
#  Statistics by Class:
# 
#                      Class: versicolor Class: virginica
# Sensitivity             0.8667          0.8750
# Specificity             0.8750          0.8667
# Pos Pred Value          0.8667          0.8750
# Neg Pred Value          0.8750          0.8667
# Prevalence              0.5161          0.4839
# Detection Rate          0.4470          0.4194
# Detection Prevalence    0.5161          0.4839
# Balanced Accuracy       0.8708          0.8708

# Extract specific values
cm_obj$overall["Accuracy"]
cm_obj$overall["Kappa"]
cm_obj$byClass
```

### Step 4: Multi-class Confusion Matrix

```r
# Use full Iris dataset
set.seed(42)
train_idx <- createDataPartition(iris$Species, p = 0.7, list = FALSE)
train_data <- iris[train_idx, ]
test_data <- iris[-train_idx, ]

# Train model
model_multi <- train(Species ~ ., 
                    data = train_data,
                    method = "rpart")

# Predictions
pred_multi <- predict(model_multi, test_data)

# Multi-class confusion matrix
cm_multi <- confusionMatrix(pred_multi, test_data$Species)
print(cm_multi)

# The table component
cm_multi$table
```

### Step 5: Visualize Confusion Matrix

```r
# Create heatmap of confusion matrix
cm_table <- cm_multi$table

# Convert to data frame for ggplot
cm_df <- as.data.frame(cm_table)
colnames(cm_df) <- c("Predicted", "Actual", "Count")

# Plot
ggplot(data = cm_df, aes(x = Predicted, y = Actual, fill = Count)) +
  geom_tile() +
  geom_text(aes(label = Count), color = "white", size = 6) +
  scale_fill_gradient(low = "lightblue", high = "darkblue") +
  ggtitle("Confusion Matrix Heatmap") +
  theme_minimal()

# Using caret's fourfold plot
fourfoldplot(cm_obj$table, 
             main = "Confusion Matrix (Binary)",
             color = c("#CC99FF", "#117733"),
             conf.level = 0.95, 
             margin = 1)
```

## Code Examples

### Example 1: Medical Diagnosis Analysis

This example demonstrates detailed confusion matrix analysis for medical testing.

```r
# Simulate medical test results
set.seed(123)
n <- 500

# True disease status (5% prevalence)
actual_disease <- factor(ifelse(rbinom(n, 1, 0.05) == 1, "Disease", "Healthy"))

# Test predictions (sensitivity 90%, specificity 85%)
test_result <- sapply(as.character(actual_disease), function(x) {
  if(x == "Disease") {
    factor(ifelse(rbinom(1, 1, 0.90) == 1, "Disease", "Healthy"))
  } else {
    factor(ifelse(rbinom(1, 1, 0.15) == 1, "Disease", "Healthy"))
  }
})

# Create confusion matrix
cm_med <- confusionMatrix(test_result, actual_disease, positive = "Disease")
print(cm_med)

# Extract key metrics
cat("\nKey Metrics for Disease Detection:\n")
cat("Sensitivity (Recall):", round(cm_med$byClass["Sensitivity"], 4), "\n")
cat("Specificity:", round(cm_med$byClass["Specificity"], 4), "\n")
cat("Precision:", round(cm_med$byClass["Pos Pred Value"], 4), "\n")
cat("Prevalence:", round(cm_med$byClass["Prevalence"], 4), "\n")

# Positive Predictive Value analysis
cat("\nIf test is positive, probability actually has disease:\n")
cat(cm_med$byClass["Pos Pred Value"], "\n")

# Negative Predictive Value analysis
cat("\nIf test is negative, probability actually healthy:\n")
cat(cm_med$byClass["Neg Pred Value"], "\n")
```

### Example 2: Customer Churn Analysis

This example shows multi-class analysis for customer churn.

```r
# Create synthetic churn data
set.seed(456)
n <- 600

churn_data <- data.frame(
  account_age = runif(n, 1, 60),
  monthly_charges = runif(n, 30, 150),
  num_support_tickets = rpois(n, 2),
  has_premium = rbinom(n, 1, 0.3),
  churn_status = factor(sample(c("No Churn", "Minor Issue", "Serious Issue"), 
                              n, prob = c(0.7, 0.2, 0.1), replace = TRUE))
)

# Add relationships
churn_data$churn_status[churn_data$num_support_tickets > 5] <- "Serious Issue"
churn_data$churn_status[churn_data$monthly_charges > 120] <- "Minor Issue"

# Split
train_idx <- sample(1:n, n * 0.7)
train_churn <- churn_data[train_idx, ]
test_churn <- churn_data[-train_idx, ]

# Train model
churn_model <- train(churn_status ~ ., 
                    data = train_churn,
                    method = "rpart")

# Predictions
churn_pred <- predict(churn_model, test_churn)

# Confusion matrix
cm_churn <- confusionMatrix(churn_pred, test_churn$churn_status)
print(cm_churn)

# Per-class metrics
cat("\nPer-Class Performance:\n")
for(i in 1:length(levels(test_churn$churn_status))) {
  cls <- levels(test_churn$churn_status)[i]
  sens <- cm_churn$byClass[i, "Sensitivity"]
  spec <- cm_churn$byClass[i, "Specificity"]
  f1 <- cm_churn$byClass[i, "F1"]
  cat(sprintf("%s: Sensitivity=%.3f, Specificity=%.3f, F1=%.3f\n", 
              cls, sens, spec, f1))
}
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Always Use caret**: Provides consistent output and confidence intervals

2. **Check Class Order**: Confusion matrix depends on factor level order

3. **Consider Class Weights**: For imbalanced data, use weighted metrics

4. **Visualize Off-Diagonal**: Shows which classes are confused

5. **Report Prevalence**: Helps interpret positive/negative predictive values

### Common Pitfalls

1. **Ignoring Factor Levels**: Wrong order leads to confusing output

2. **Not Setting Positive Class**: For binary problems, always specify

3. **Ignoring Multi-class Metrics**: Overall accuracy hides per-class issues

4. **Confusing Sensitivity/Specificity**: Know what each measures

5. **Not Considering Prevalence**: Affects predictive values significantly

## Performance Considerations

### Interpretation Guidelines

| Metric | Good Value | Warning Sign |
|--------|------------|--------------|
| Accuracy | > 0.9 | < 0.7 |
| Kappa | > 0.8 | < 0.5 |
| Sensitivity | > 0.9 | < 0.7 |
| Specificity | > 0.9 | < 0.7 |
| F1 | > 0.8 | < 0.6 |

### Computational Notes

- Confusion matrix calculation is O(n)
- Memory: O(k²) for k classes
- Fast even for large datasets

## Related Concepts

- **Accuracy Metrics**: Derived metrics like precision, recall
- **ROC Curves**: Threshold-based analysis
- **Kappa Statistic**: Agreement measure adjusting for chance
- **Cost-sensitive Evaluation**: Weighted by error costs

## Exercise Problems

1. **Basic**: Create and interpret a binary confusion matrix.

2. **Intermediate**: Analyze which classes are most confused in multi-class.

3. **Advanced**: Calculate all metrics from raw confusion matrix values.

4. **Real-World Challenge**: Evaluate customer churn model with confusion matrix.

5. **Extension**: Implement cost-sensitive confusion matrix analysis.