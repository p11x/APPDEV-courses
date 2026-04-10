# ROC Curves and AUC

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the ROC curve and its components (TPR, FPR)
- Plot and interpret ROC curves in R
- Calculate Area Under the Curve (AUC)
- Choose optimal classification thresholds using ROC
- Apply ROC analysis to multi-class problems
- Compare model performance using ROC curves
- Understand when ROC is more appropriate than precision-recall curves

## Theoretical Background

Receiver Operating Characteristic (ROC) curves are a fundamental tool for evaluating binary classifiers. Originally developed for radar signal detection in World War II, ROC curves now widely used in machine learning to visualize classifier performance across all possible thresholds.

### The ROC Curve

The ROC curve plots the True Positive Rate (TPR/Sensitivity) against the False Positive Rate (FPR) at every classification threshold.

$$TPR = \frac{TP}{TP + FN}$$
$$FPR = \frac{FP}{FP + TN}$$

#### Key Points on ROC Curve

- **(0, 0)**: Classify everything as negative - 0% TP, 0% FP
- **(1, 1)**: Classify everything as positive - 100% TP, 100% FP
- **(0, 1)**: Perfect classifier - 100% TP, 0% FP
- **Diagonal line (random classifier)**: TPR = FPR for all thresholds

### Area Under Curve (AUC)

AUC represents the probability that a randomly chosen positive instance is ranked higher than a randomly chosen negative instance.

- **AUC = 1.0**: Perfect classifier
- **AUC = 0.9**: 90% chance of ranking positive higher than negative
- **AUC = 0.5**: Random classifier (diagonal line)
- **AUC < 0.5**: Worse than random (predictions inverted)

AUC is threshold-independent, making it excellent for comparing models.

### Why ROC Curves Matter

1. **Threshold Selection**: ROC shows performance at every threshold, allowing optimal trade-off selection
2. **Class Imbalance Robust**: Unlike accuracy, ROC is less affected by class distribution
3. **Model Comparison**: Visual comparison of multiple models
4. **Interpretability**: Easy to understand and explain

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("pROC")
install.packages("ROCR")
install.packages("caret")

library(pROC)
library(ROCR)
library(caret)
```

### Step 2: Train a Model with Probability Output

```r
# Load and prepare data
data(iris)
iris_binary <- iris[iris$Species != "setosa", ]
iris_binary$Species <- factor(iris_binary$Species)

# Create train/test split
set.seed(42)
train_idx <- createDataPartition(iris_binary$Species, p = 0.7, list = FALSE)
train_data <- iris_binary[train_idx, ]
test_data <- iris_binary[-train_idx, ]

# Train model with probability output
model <- train(Species ~ ., 
              data = train_data,
              method = "glm",
              family = "binomial",
              preProcess = c("center", "scale"))

# Get probability predictions
pred_probs <- predict(model, test_data, type = "prob")
pred_probs <- pred_probs[, "versicolor"]  # Probability of one class

# Get actual labels
actual <- as.numeric(test_data$Species) - 1  # Convert to 0/1
actual <- ifelse(test_data$Species == "versicolor", 1, 0)
```

### Step 3: Calculate ROC Curve

```r
# Create roc object
roc_obj <- roc(response = actual, 
               predictor = pred_probs,
               levels = c(0, 1),
               direction = "<")

# Print ROC object
print(roc_obj)
# Response:  (30+ samples)
# Predictor:  30 values, min = 0.08, max = 0.95
# ROC area: 0.957

# Calculate coordinates
roc_coords <- coords(roc_obj, x = "best", best.method = "closest.topleft")
print(roc_coords)
#   threshold   specificity sensitivity
# 1     0.49         0.9375     0.9333

# Calculate all points
roc_points <- coords(roc_obj, ret = c("threshold", "sensitivity", "specificity", "tn", "fp", "fn", "tp"))
head(roc_points)
```

### Step 4: Calculate AUC

```r
# Calculate AUC
auc_value <- auc(roc_obj)
print(paste("AUC:", round(auc_value, 4)))
# [1] "AUC: 0.9572"

# Calculate 95% CI using bootstrap
set.seed(42)
auc_ci <- ci.auc(roc_obj, conf.level = 0.95, method = "bootstrap", boot.n = 2000)
print(paste("95% CI:", round(auc_ci[1], 4), "-", round(auc_ci[3], 4)))
```

### Step 5: Plot ROC Curve

```r
# Basic ROC plot
plot(roc_obj, 
     main = "ROC Curve",
     col = "blue",
     lwd = 2)

# Add diagonal line (random classifier)
abline(a = 0, b = 1, lty = 2, col = "gray")

# Add AUC annotation
text(0.5, 0.3, paste("AUC =", round(auc_value, 3)), cex = 1.2)
```

### Step 6: Using ROCR Package

```r
# Prepare predictions for ROCR
pred_obj <- prediction(predictions = pred_probs, labels = actual)
perf_obj <- performance(pred_obj, measure = "tpr", x.measure = "fpr")

# Plot
plot(perf_obj, 
     main = "ROC Curve using ROCR",
     colorize = TRUE,
     print.cutoffs.at = seq(0, 1, by = 0.1),
     lwd = 2)

# Calculate AUC using ROCR
auc_perf <- performance(pred_obj, measure = "auc")
auc_rocr <- slot(auc_perf, "y.values")[[1]]
print(paste("AUC (ROCR):", round(auc_rocr, 4)))
```

## Code Examples

### Example 1: Compare Multiple Models

This example shows comparing multiple classifiers using ROC.

```r
# Create training data with multiple features
set.seed(123)
n <- 200

data_multi <- data.frame(
  f1 = rnorm(n, mean = ifelse(runif(n) > 0.3, 2, 0)),
  f2 = rnorm(n, mean = ifelse(runif(n) > 0.4, 1.5, 0)),
  f3 = rnorm(n, 0, 1),
  y = as.factor(ifelse(runif(n) > 0.3, "pos", "neg"))
)

# Split data
train_idx <- sample(1:n, n * 0.7)
train_df <- data_multi[train_idx, ]
test_df <- data_multi[-train_idx, ]

# Train multiple models
# Model 1: Logistic Regression
model_lr <- train(y ~ f1 + f2, data = train_df, method = "glm", 
                family = "binomial", preProcess = "center")

# Model 2: Random Forest
model_rf <- train(y ~ f1 + f2, data = train_df, method = "rf", 
                  preProcess = "center")

# Model 3: SVM
model_svm <- train(y ~ f1 + f2, data = train_df, method = "svmRadial", 
                  preProcess = "center")

# Get predictions for each model
preds_lr <- predict(model_lr, test_df, type = "prob")$pos
preds_rf <- predict(model_rf, test_df, type = "prob")$pos
preds_svm <- predict(model_svm, test_df, type = "prob")$pos
actual_test <- ifelse(test_df$y == "pos", 1, 0)

# Calculate ROC for each
roc_lr <- roc(actual_test, preds_lr, levels = c(0, 1))
roc_rf <- roc(actual_test, preds_rf, levels = c(0, 1))
roc_svm <- roc(actual_test, preds_svm, levels = c(0, 1))

# Plot all ROC curves
plot(roc_lr, col = "red", lwd = 2, main = "Model Comparison ROC Curves")
plot(roc_rf, col = "green", lwd = 2, add = TRUE)
plot(roc_svm, col = "blue", lwd = 2, add = TRUE)
abline(a = 0, b = 1, lty = 2, col = "gray")

# Legend
legend("bottomright", 
       legend = c(paste("LR (AUC=", round(auc(roc_lr), 3), ")"),
                paste("RF (AUC=", round(auc(roc_rf), 3), ")"),
                paste("SVM (AUC=", round(auc(roc_svm), 3), ")")),
       col = c("red", "green", "blue"), lwd = 2)
```

### Example 2: Optimal Threshold Selection

This example demonstrates choosing optimal classification threshold.

```r
# Create synthetic data with cost asymmetry
set.seed(456)
n <- 300

# Cost of false negative is 10x cost of false positive
cost_fn <- 10
cost_fp <- 1

data_cost <- data.frame(
  score = rbeta(n, 2, 5),
  actual = rbinom(n, 1, 0.3)  # 30% positive rate
)

# Calculate total cost at each threshold
thresholds <- seq(0.01, 0.99, by = 0.01)
total_costs <- sapply(thresholds, function(t) {
  predicted <- ifelse(data_cost$score > t, 1, 0)
  fp <- sum(predicted == 1 & data_cost$actual == 0)
  fn <- sum(predicted == 0 & data_cost$actual == 1)
  cost_fp * fp + cost_fn * fn
})

# Find optimal threshold
optimal_idx <- which.min(total_costs)
optimal_threshold <- thresholds[optimal_idx]
min_cost <- total_costs[optimal_idx]

# Calculate ROC to verify
roc_cost <- roc(data_cost$actual, data_cost$score)
plot(roc_cost, main = paste("Optimal Threshold:", optimal_threshold))
abline(h = 1 - optimal_threshold, lty = 2, col = "red")

cat("Optimal Threshold:", optimal_threshold, "\n")
cat("Minimum Total Cost:", min_cost, "\n")
cat("Related Sensitivity:", round(coords(roc_cost, optimal_threshold, ret = "sensitivity"), 3), "\n")
cat("Related Specificity:", round(coords(roc_cost, optimal_threshold, ret = "specificity"), 3), "\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use Smoothed ROC**: For cleaner visualizations, especially for noisy data

2. **Add Confidence Intervals**: Bootstrap confidence intervals show uncertainty

3. **Compare Multiple Models**: Overlay ROC curves to compare directly

4. **Consider Base Rate**: Low prevalence affects interpretation

5. **Use Partial AUC**: For focused comparison on relevant FPR range

### Common Pitfalls

1. **Ignoring Class Imbalance Effects**: AUC is generally robust but can still be misleading

2. **Using Improper Levels**: Always specify correct positive/negative levels

3. **Not Handling Direction**: Check if higher predictions mean more positive

4. **Ignoring Small Sample Effects**: Bootstrap for confidence intervals

5. **Comparing Inappropriate Ranges**: Use partial AUC for specific FPR ranges

## Performance Considerations

### Computational Complexity

- AUC calculation: O(n log n) due to sorting
- Bootstrap CI: O(n × boot.n)
- Memory: O(n) for storing predictions

### When to Use ROC vs PR Curves

| Scenario | Recommended |
|----------|-------------|
| Balanced classes | Either |
| Imbalanced (rare positive) | PR Curves |
| Many negative results | ROC |
| Cost-sensitive | ROC with cost-based threshold |

## Related Concepts

- **Confusion Matrix**: Binary confusion matrix
- **Precision-Recall Curve**: Better for imbalanced data
- **Cost-Sensitive Learning**: Incorporate error costs
- **Calibration**: Probability calibration

## Exercise Problems

1. **Basic**: Plot ROC curve for a classification model.

2. **Intermediate**: Calculate AUC with 95% confidence interval.

3. **Advanced**: Find optimal threshold using Youden's J statistic.

4. **Real-World Challenge**: Compare ROC curves for multiple models.

5. **Extension**: Implement one-vs-all ROC for multi-class problems.