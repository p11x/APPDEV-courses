# Model Evaluation

## Learning Objectives

- Evaluate regression and classification models
- Use appropriate metrics
- Create confusion matrices

## Code Examples

```r
# Model evaluation

cat("===== MODEL EVALUATION =====\n\n")

# Regression metrics
data(mtcars)
train <- mtcars[1:20, ]
test <- mtcars[21:32, ]

model <- lm(mpg ~ wt + hp, data = train)
pred <- predict(model, test)

# Metrics
mae <- mean(abs(test$mpg - pred))
mse <- mean((test$mpg - pred)^2)
rmse <- sqrt(mse)

cat("Regression Metrics:\n")
cat("MAE:", mae, "\n")
cat("MSE:", mse, "\n")
cat("RMSE:", rmse, "\n")

# Classification metrics
cat("\nClassification Metrics:\n")
actual <- c(1, 0, 1, 1, 0, 0, 1, 0, 1, 1)
predicted <- c(1, 0, 1, 0, 0, 1, 1, 0, 1, 1)

conf_matrix <- table(Actual = actual, Predicted = predicted)
cat("Confusion Matrix:\n")
print(conf_matrix)

accuracy <- sum(diag(conf_matrix)) / sum(conf_matrix)
cat("\nAccuracy:", accuracy, "\n")
```

## Best Practices

1. Use test set for final evaluation
2. Compare multiple metrics
3. Consider business context
