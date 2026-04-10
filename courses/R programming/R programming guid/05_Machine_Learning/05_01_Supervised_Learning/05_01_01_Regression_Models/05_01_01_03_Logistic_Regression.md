# Logistic Regression in R

## Learning Objectives

- Understand the fundamentals of logistic regression
- Implement binary and multinomial logistic regression
- Interpret odds ratios and model coefficients
- Evaluate classification performance metrics
- Apply logistic regression to real-world classification problems

## Theoretical Background

### What is Logistic Regression?

Logistic regression is a statistical method for analyzing datasets where the outcome variable is categorical (binary or multinomial). Unlike linear regression which predicts continuous values, logistic regression predicts probabilities of class membership.

### The Logistic Function

The logistic (sigmoid) function maps any real number to a probability between 0 and 1:

$$P(Y=1) = \frac{1}{1 + e^{-(\beta_0 + \beta_1 X_1 + ... + \beta_n X_n)}}$$

### Odds and Log-Odds

- **Odds** = P(event) / P(no event) = p / (1-p)
- **Log-odds** = log(odds) = ln(p / (1-p))

The logistic model can be rewritten as:
$$\ln\left(\frac{p}{1-p}\right) = \beta_0 + \beta_1 X_1 + ... + \beta_n X_n$$

## Step-by-Step Implementation

### Binary Logistic Regression

```r
# Load data
data(mtcars)
mtcars$vs <- as.factor(mtcars$vs)

# Fit logistic regression: vs ~ wt + hp
model <- glm(vs ~ wt + hp, data = mtcars, family = binomial)
summary(model)

# Interpret coefficients
exp(coef(model))  # Odds ratios
```

## Code Examples

### Standard Example

```r
# Standard logistic regression example
set.seed(42)
n <- 100

# Generate binary outcome
x <- runif(n, 0, 10)
prob <- 1 / (1 + exp(-(0.5 * x - 2.5)))
y <- rbinom(n, 1, prob)

# Fit model
model <- glm(y ~ x, family = binomial)
summary(model)

# Predictions
pred_probs <- predict(model, type = "response")
pred_class <- ifelse(pred_probs > 0.5, 1, 0)
```

### Real-World Example: Customer Churn

```r
# Customer churn prediction
churn_data <- data.frame(
  tenure = c(2, 12, 24, 6, 18, 36, 4, 8, 48, 1),
  monthly_charge = c(70, 85, 95, 55, 75, 90, 60, 65, 100, 45),
  contract = c("month-to-month", "annual", "annual", "month-to-month", 
               "annual", "annual", "month-to-month", "month-to-month",
               "annual", "month-to-month"),
  num_support_calls = c(3, 1, 0, 2, 1, 0, 4, 2, 0, 5),
  churn = c(1, 0, 0, 1, 0, 0, 1, 1, 0, 1)
)

# Convert to factors
churn_data$contract <- as.factor(churn_data$contract)
churn_data$churn <- as.factor(churn_data$churn)

# Fit model
churn_model <- glm(churn ~ tenure + monthly_charge + contract + num_support_calls,
                  data = churn_data, family = binomial)

# Summary and odds ratios
cat("=== Churn Prediction Model ===\n")
print(summary(churn_model))
cat("\nOdds Ratios:\n")
print(exp(coef(churn_model)))

# Make predictions for new customers
new_customer <- data.frame(
  tenure = 12,
  monthly_charge = 75,
  contract = "annual",
  num_support_calls = 1
)

churn_prob <- predict(churn_model, new_customer, type = "response")
cat("\nChurn probability:", round(churn_prob * 100, 2), "%\n")
```

## Best Practices

1. Check for separation issues (perfect prediction)
2. Use regularization for high-dimensional data
3. Validate with cross-validation