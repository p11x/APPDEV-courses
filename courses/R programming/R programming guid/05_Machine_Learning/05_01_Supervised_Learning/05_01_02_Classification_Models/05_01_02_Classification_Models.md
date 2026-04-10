# Classification Models in R

## Learning Objectives

- Perform classification using logistic regression
- Use tree-based classifiers
- Evaluate classification models

## Code Examples

```r
# Classification in R

cat("===== CLASSIFICATION =====\n\n")

# Logistic Regression
data(mtcars)
mtcars$vs <- as.factor(mtcars$vs)

cat("Logistic: vs ~ wt + hp\n")
model_log <- glm(vs ~ wt + hp, data = mtcars, family = "binomial")
summary(model_log)

# Predictions
prob <- predict(model_log, type = "response")
pred <- ifelse(prob > 0.5, 1, 0)
cat("\nPredicted classes:\n")
print(pred)

# Decision Tree
cat("\nDecision Tree:\n")
library(rpart)
model_tree <- rpart(vs ~ wt + hp, data = mtcars)
print(model_tree)
```

## Best Practices

1. Use confusion matrix for evaluation
2. Check class imbalance
3. Use cross-validation
