# Regression Models in R

## Learning Objectives

- Understand linear regression
- Perform simple and multiple regression
- Interpret model results
- Evaluate model fit

## Code Examples

```r
# Linear regression in R

cat("===== LINEAR REGRESSION =====\n\n")

# Simple linear regression
data(mtcars)
cat("Simple: mpg ~ wt\n")
model_simple <- lm(mpg ~ wt, data = mtcars)
summary(model_simple)

cat("\nMultiple: mpg ~ wt + hp\n")
model_multi <- lm(mpg ~ wt + hp, data = mtcars)
summary(model_multi)

# Predictions
cat("\nPredictions:\n")
new_data <- data.frame(wt = c(2, 3, 4), hp = c(100, 150, 200))
predictions <- predict(model_multi, new_data)
cat("New predictions:", predictions, "\n")
```

## Best Practices

1. Check model assumptions
2. Use adjusted R-squared for multiple regression
3. Examine residuals
