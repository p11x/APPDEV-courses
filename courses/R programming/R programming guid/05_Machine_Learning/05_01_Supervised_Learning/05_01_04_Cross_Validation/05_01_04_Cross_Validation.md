# Cross-Validation

## Learning Objectives

- Understand cross-validation
- Perform k-fold cross-validation
- Use cross-validation for model selection

## Code Examples

```r
# Cross-validation

cat("===== CROSS-VALIDATION =====\n\n")

library(caret)

data(mtcars)

# K-fold cross-validation
cat("5-Fold CV for linear regression:\n")
train_control <- trainControl(method = "cv", number = 5)
model <- train(mpg ~ wt + hp, data = mtcars, 
              method = "lm", trControl = train_control)
print(model)

# Leave-one-out CV
cat("\nLeave-One-Out CV:\n")
train_control2 <- trainControl(method = "LOOCV")
model2 <- train(mpg ~ wt + hp, data = mtcars,
               method = "lm", trControl = train_control2)
print(model2)
```

## Best Practices

1. Use 5 or 10 folds for most cases
2. Use repeated CV for stability
3. Consider time series CV
