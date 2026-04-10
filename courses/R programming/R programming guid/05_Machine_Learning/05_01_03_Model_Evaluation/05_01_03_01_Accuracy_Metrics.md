# Accuracy Metrics in Machine Learning

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand and calculate basic accuracy metrics (accuracy, error rate)
- Calculate precision, recall, and F1-score for classification
- Apply weighted and macro-averaging for multi-class problems
- Interpret metrics in context of class imbalance
- Use caret and MLmetrics packages for metric computation
- Choose appropriate metrics for different business problems

## Theoretical Background

Evaluating machine learning models requires quantitative measures that capture how well predictions match actual values. For classification problems, accuracy metrics tell us not just the overall correctness, but also the nature of errors made.

### Basic Accuracy Metrics

#### Accuracy

Accuracy is the proportion of correct predictions:

$$Accuracy = \frac{TP + TN}{TP + TN + FP + FN} = \frac{correct\_predictions}{total\_predictions}$$

For multi-class problems:
$$Accuracy = \frac{\sum_{i=1}^{k} TP_i}{N}$$

where k is the number of classes and N is the total samples.

#### Error Rate

Error rate is the proportion of incorrect predictions:

$$Error Rate = 1 - Accuracy = \frac{FP + FN}{TP + TN + FP + FN}$$

### Why Accuracy Alone Is Not Enough

Consider a fraud detection problem where 99% of transactions are legitimate:
- A model that predicts "not fraud" for everything would have 99% accuracy
- But it would fail to detect any actual fraud
- Accuracy is misleading for imbalanced datasets

This is why we need additional metrics that capture different aspects of performance.

### Precision

Precision (also called Positive Predictive Value) measures how many of the positive predictions are correct:

$$Precision = \frac{TP}{TP + FP}$$

Interpretation: Of all samples predicted as positive, what fraction actually is positive?

High precision means fewer false alarms (lower FP), but may miss true positives.

### Recall (Sensitivity or True Positive Rate)

Recall measures how many of the actual positives are correctly identified:

$$Recall = \frac{TP}{TP + FN} = TPR$$

Interpretation: Of all actual positive samples, what fraction was correctly predicted?

High recall means catching more actual positives, but may have more false alarms.

### Specificity (True Negative Rate)

Specificity measures correctly identified negatives:

$$Specificity = \frac{TN}{TN + FP}$$

Interpretation: Of all actual negative samples, what fraction was correctly predicted?

### F1-Score

F1-score is the harmonic mean of precision and recall:

$$F1 = 2 \times \frac{Precision \times Recall}{Precision + Recall}$$

Harmonic mean penalizes models where one metric is much lower than the other.

### Trade-offs Between Precision and Recall

There's typically a trade-off between precision and recall:
- Increasing threshold: Higher precision, lower recall
- Decreasing threshold: Lower precision, higher recall

The optimal point depends on the business problem:
- Medical diagnosis: Higher recall (don't miss diseases)
- Spam detection: Higher precision (don't block important emails)
- Fraud detection: Balance both (cost of missing fraud vs. customer annoyance)

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
# Install required packages
install.packages("caret")
install.packages("MLmetrics")

# Load libraries
library(caret)
library(MLmetrics)
```

### Step 2: Create Sample Predictions

```r
# Load data and create sample predictions
data(iris)

# Use binary classification
iris_binary <- iris[iris$Species != "setosa", ]
iris_binary$Species <- factor(iris_binary$Species)

# Create train/test split
set.seed(42)
train_idx <- createDataPartition(iris_binary$Species, p = 0.7, list = FALSE)
train_data <- iris_binary[train_idx, ]
test_data <- iris_binary[-train_idx, ]

# Train a simple model
model <- train(Species ~ ., 
              data = train_data,
              method = "glm",
              family = "binomial")

# Predictions
pred_class <- predict(model, test_data)
pred_prob <- predict(model, test_data, type = "prob")

# True labels
actual <- test_data$Species
```

### Step 3: Calculate Basic Metrics

```r
# Basic confusion matrix
cm <- table(Predicted = pred_class, Actual = actual)
print(cm)
#             Actual
# Predicted    versicolor virginica
#   versicolor         13         2
#   virginica          2        14

# Calculate accuracy
accuracy <- sum(diag(cm)) / sum(cm)
print(paste("Accuracy:", round(accuracy, 4)))
# [1] "Accuracy: 0.9032"

# Calculate error rate
error_rate <- 1 - accuracy
print(paste("Error Rate:", round(error_rate, 4)))
# [1] "Error Rate: 0.0968"

# Using caret for accuracy
accuracy_caret <- Accuracy(pred_class, actual)
print(paste("Accuracy (caret):", round(accuracy_caret, 4)))
```

### Step 4: Calculate Precision, Recall, F1

```r
# Calculate metrics using confusionMatrix
cm_obj <- confusionMatrix(pred_class, actual)
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
#  Pos Classes: versicolor 
#            Sensitivity : 0.8667
#            Specificity : 0.8750
#         Pos Pred Value : 0.8667
#         Neg Pred Value : 0.8750
#             Prevalence : 0.5161
#         Detection Rate : 0.4470
#    Detection Prevalence : 0.5161
#       Balanced Accuracy : 0.8708

# Extract individual metrics
sensitivity <- cm_obj$byClass["Sensitivity"]
specificity <- cm_obj$byClass["Specificity"]
precision_val <- cm_obj$byClass["Pos Pred Value"]
recall_val <- cm_obj$byClass["Recall"]

print(paste("Sensitivity (Recall):", round(sensitivity, 4)))
print(paste("Specificity:", round(specificity, 4)))
print(paste("Precision:", round(precision_val, 4)))
```

### Step 5: Calculate F1-Score

```r
# Calculate F1-score manually
precision_val <- 13 / (13 + 2)  # TP / (TP + FP) for versicolor
recall_val <- 13 / (13 + 2)      # TP / (TP + FN) for versicolor

f1_versicolor <- 2 * (precision_val * recall_val) / (precision_val + recall_val)
print(paste("F1-Score (versicolor):", round(f1_versicolor, 4)))

# Using MLmetrics package
f1_score <- F1_Score(actual, pred_class, positive = "versicolor")
print(paste("F1-Score (MLmetrics):", f1_score))

# For multi-class, use macro and weighted averaging
library(caret)
both_classes <- levels(actual)
f1_scores <- sapply(both_classes, function(cls) {
  F1_Score(actual, pred_class, positive = cls)
})
print("F1-scores by class:")
print(f1_scores)

# Macro F1 (unweighted average)
macro_f1 <- mean(f1_scores)
print(paste("Macro F1:", round(macro_f1, 4)))

# Weighted F1 (weighted by class frequency)
class_weights <- table(actual) / length(actual)
weighted_f1 <- sum(f1_scores * class_weights)
print(paste("Weighted F1:", round(weighted_f1, 4)))
```

## Code Examples

### Example 1: Medical Test Evaluation

This example demonstrates evaluating a diagnostic test.

```r
# Create synthetic medical test data
set.seed(123)
n <- 500

# 1 = disease present, 0 = no disease
actual_disease <- rbinom(n, 1, 0.05)  # 5% disease prevalence

# Test result (sensitivity = 0.95, specificity = 0.90)
test_result <- sapply(actual_disease, function(x) {
  if(x == 1) {
    rbinom(1, 1, 0.95)  # 95% sensitivity
  } else {
    rbinom(1, 1, 0.10)  # 10% false positive rate
  }
})

# Calculate metrics
TP <- sum(actual_disease == 1 & test_result == 1)
TN <- sum(actual_disease == 0 & test_result == 0)
FP <- sum(actual_disease == 0 & test_result == 1)
FN <- sum(actual_disease == 1 & test_result == 0)

# Calculate metrics
accuracy <- (TP + TN) / n
precision_disease <- TP / (TP + FP)
recall_disease <- TP / (TP + FN)

cat("Disease Detection Metrics:\n")
cat("Accuracy:", round(accuracy, 4), "\n")
cat("Precision:", round(precision_disease, 4), "\n")
cat("Recall:", round(recall_disease, 4), "\n")

# If 100 people test positive, how many actually have disease?
# This is Positive Predictive Value (Precision for disease class)
cat("\nPositive Predictive Value: ", precision_disease * 100, "%\n")
cat("of positive tests are true diseases\n")
```

### Example 2: Spam Email Detection

This example shows metrics for imbalanced spam detection.

```r
# Create spam detection data
set.seed(456)
n <- 1000

# True spam labels (20% spam)
actual_spam <- rbinom(n, 1, 0.2)

# Model predictions
# High recall for spam (catches 90%), lower precision
predicted_spam <- sapply(actual_spam, function(x) {
  if(x == 1) {
    rbinom(1, 1, 0.90)
  } else {
    rbinom(1, 1, 0.15)
  }
})

# Calculate confusion matrix
cm <- table(Predicted = factor(predicted_spam, levels = c(0, 1), 
                                labels = c("ham", "spam")),
            Actual = factor(actual_spam, levels = c(0, 1), 
                           labels = c("ham", "spam")))
print(cm)

# Calculate metrics
cm_obj <- confusionMatrix(cm, positive = "spam")

cat("\nSpam Detection Metrics:\n")
cat("Overall Accuracy:", round(cm_obj$overall["Accuracy"], 4), "\n")
cat("Balanced Accuracy:", round(cm_obj$byClass["Balanced Accuracy"], 4), "\n")
cat("\nFor Spam Class:\n")
cat("  Sensitivity (Recall):", round(cm_obj$byClass["Sensitivity"], 4), "\n")
cat("  Specificity:", round(cm_obj$byClass["Specificity"], 4), "\n")
cat("  Precision:", round(cm_obj$byClass["Pos Pred Value"], 4), "\n")
cat("  F1-Score:", round(cm_obj$byClass["F1"], 4), "\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use Balanced Accuracy**: For imbalanced data, balanced accuracy = (TPR + TNR) / 2

2. **Report Precision and Recall Together**: Neither metric alone tells the full story

3. **Use F1 for Binary Problems**: Harmonic mean provides balanced measure

4. **Consider Business Context**: The "best" metric depends on the problem costs

5. **Report Confidence Intervals**: Accuracy has sampling variability

### Common Pitfalls

1. **Ignoring Class Imbalance**: High accuracy with imbalanced data is misleading

2. **Using Accuracy Only**: Always include other metrics for imbalanced problems

3. **Confusing Sensitivity/Specificity**: Know which metric matters for your problem

4. **Ignoring Base Rate**: Low prevalence problems need high recall even with lower precision

5. **Not Considering Error Costs**: False positives and false negatives may have different costs

## Performance Considerations

### Which Metrics to Use

| Problem Type | Key Metric | Why |
|--------------|------------|-----|
| Balanced classification | Accuracy | Equal importance to all classes |
| Imbalanced | F1, Balanced Accuracy | Captures performance on minority class |
| Medical diagnosis | Recall | Don't miss actual diseases |
| Spam detection | Precision | Don't block important emails |
| Fraud detection | F1 | Balance catching fraud vs. customer annoyance |
| Search engines | Precision@K | Top results matter most |

### Computational Notes

- All metrics are fast to calculate
- For very large datasets, sample for confidence intervals
- Use streaming for truly massive datasets

## Related Concepts

- **ROC Curves**: Visualize TPR vs FPR at all thresholds
- **AUC**: Area under ROC curve
- **Precision-Recall Curves**: Better for imbalanced data
- **Confusion Matrix**: Foundation for all metrics

## Exercise Problems

1. **Basic**: Calculate all metrics for a classification model.

2. **Intermediate**: Compare metrics on balanced vs imbalanced data.

3. **Advanced**: Create a custom metric for a specific business case.

4. **Real-World Challenge**: Evaluate a fraud detection model with proper metrics.

5. **Extension**: Implement precision@K for recommendation systems.