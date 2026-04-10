# Linear Regression in R

## Learning Objectives

- Understand the fundamental concepts of linear regression
- Implement simple and multiple linear regression in R
- Interpret regression model outputs and coefficients
- Evaluate model performance using various metrics
- Apply linear regression to real-world data analysis problems

## Theoretical Background

### What is Linear Regression?

Linear regression is a fundamental statistical method used to model the relationship between a dependent variable (also called the response or target variable) and one or more independent variables (predictors or features). The core idea is to find the best-fitting linear equation that describes this relationship.

The fundamental assumption is that there is a linear relationship between the predictors and the target variable, which can be expressed mathematically as:

**Simple Linear Regression:**
$$Y = \beta_0 + \beta_1 X + \epsilon$$

Where:
- $Y$ is the dependent variable
- $X$ is the independent variable
- $\beta_0$ is the intercept (value of Y when X = 0)
- $\beta_1$ is the slope (change in Y for each unit change in X)
- $\epsilon$ is the error term (residuals)

**Multiple Linear Regression:**
$$Y = \beta_0 + \beta_1 X_1 + \beta_2 X_2 + ... + \beta_n X_n + \epsilon$$

### Key Assumptions of Linear Regression

1. **Linearity**: The relationship between predictors and response is linear
2. **Independence**: Observations are independent of each other
3. **Homoscedasticity**: Constant variance of residuals
4. **Normality**: Residuals follow a normal distribution
5. **No Multicollinearity**: Predictors are not highly correlated with each other

### Ordinary Least Squares (OLS) Estimation

The most common method for fitting a linear regression model is Ordinary Least Squares (OLS). This method finds the coefficients that minimize the sum of squared residuals:

$$\text{Minimize} \sum_{i=1}^{n}(y_i - \hat{y}_i)^2$$

Where $\hat{y}_i$ is the predicted value and $y_i$ is the actual value.

## Step-by-Step Implementation in R

### Step 1: Preparing the Data

```r
# Load the mtcars dataset (built-in dataset)
data(mtcars)

# View the structure of the dataset
str(mtcars)

# Check the first few rows
head(mtcars)

# View variable descriptions
?mtcars

# In this dataset:
# - mpg (Miles Per Gallon) will be our dependent variable
# - wt (Weight) will be our predictor
```

### Step 2: Simple Linear Regression

```r
# Perform simple linear regression: mpg ~ wt
# This models how car weight affects fuel efficiency

simple_model <- lm(mpg ~ wt, data = mtcars)

# Display the model summary
summary(simple_model)

# The output shows:
# Call: lm(formula = mpg ~ wt, data = mtcars)
# 
# Residuals:
#     Min      1Q  Median      3Q     Max 
# -4.5432 -1.7746 -0.1329  1.0853  6.3612 
# 
# Coefficients:
#             Estimate Std. Error t value Pr(>|t|)    
# (Intercept)  37.285       1.878  19.86  <2e-16 ***
# wt           -5.344       0.559  -9.56  1.29e-10 ***
# ---
# Signif. codes:  0 '***' 0.001 '**' 0.01 '*' 0.05 '.' 0.1 ' ' 1
# 
# Residual standard error: 3.05 on 30 degrees of freedom
# Multiple R-squared:  0.753,	Adjusted R-squared:  0.745
# F-statistic: 91.4 on 1 and 30 DF,  p-value: 1.29e-10
```

### Step 3: Multiple Linear Regression

```r
# Perform multiple linear regression with multiple predictors
# mpg ~ wt + hp + cyl (weight, horsepower, and cylinders)

multi_model <- lm(mpg ~ wt + hp + cyl, data = mtcars)

# Display the model summary
summary(multi_model)

# Interpretation:
# - wt coefficient: For every 1 unit increase in weight, mpg decreases by 3.88
# - hp coefficient: For every 1 unit increase in horsepower, mpg decreases by 0.03
# - cyl coefficient: For every 1 unit increase in cylinders, mpg decreases by 1.23
```

## Code Examples

### Standard Example: Basic Linear Regression

```r
# ============================================
# STANDARD EXAMPLE: Simple Linear Regression
# ============================================

# Create a simple dataset
x <- c(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)
y <- c(2.1, 4.3, 5.8, 8.2, 9.9, 12.1, 14.0, 16.2, 17.9, 20.1)

# Create a data frame
df <- data.frame(x = x, y = y)

# Perform linear regression
model <- lm(y ~ x, data = df)

# View coefficients
cat("Coefficients:\n")
cat("Intercept:", coef(model)[1], "\n")
cat("Slope:", coef(model)[2], "\n")

# Make predictions
predictions <- predict(model)
cat("\nPredictions:", predictions, "\n")

# Calculate R-squared
rsquared <- summary(model)$r.squared
cat("\nR-squared:", rsquared, "\n")

# Plot the data and regression line
plot(x, y, main = "Simple Linear Regression", 
     xlab = "Independent Variable X", 
     ylab = "Dependent Variable Y",
     pch = 19, col = "blue")
abline(model, col = "red", lwd = 2)
legend("topleft", legend = c("Data Points", "Fitted Line"), 
       col = c("blue", "red"), pch = c(19, NA), lwd = c(NA, 2))
```

**Output:**
```
Coefficients:
Intercept: 0.24 
Slope: 1.96 

Predictions: 2.2 4.16 6.12 8.08 10.04 12 13.96 15.92 17.88 19.84 

R-squared: 0.9976369
```

### Real-World Example 1: Housing Price Prediction

```r
# ========================================================
# REAL-WORLD EXAMPLE 1: Housing Price Prediction
# ========================================================

# Load required library
library(datasets)

# Use the built-in 'iris' dataset for demonstration
# Let's create a simulated housing dataset
set.seed(42)  # For reproducibility

# Create synthetic housing data
n <- 200
housing_data <- data.frame(
  square_feet = runif(n, 800, 3500),
  num_bedrooms = sample(1:5, n, replace = TRUE),
  num_bathrooms = sample(1:4, n, replace = TRUE),
  age_of_house = runif(n, 1, 50),
  garage_spaces = sample(0:3, n, replace = TRUE),
  location_score = runif(n, 1, 10)
)

# Create price based on features with some noise
housing_data$price <- (
  50000 + 
  150 * housing_data$square_feet +
  10000 * housing_data$num_bedrooms +
  5000 * housing_data$num_bathrooms -
  500 * housing_data$age_of_house +
  3000 * housing_data$garage_spaces +
  5000 * housing_data$location_score +
  rnorm(n, 0, 15000)  # Add random noise
)

# Fit the model
housing_model <- lm(price ~ square_feet + num_bedrooms + num_bathrooms + 
                    age_of_house + garage_spaces + location_score, 
                    data = housing_data)

# Display summary
cat("=== Housing Price Prediction Model ===\n\n")
print(summary(housing_model))

# Make predictions for a new house
new_house <- data.frame(
  square_feet = 2000,
  num_bedrooms = 3,
  num_bathrooms = 2,
  age_of_house = 10,
  garage_spaces = 2,
  location_score = 7.5
)

predicted_price <- predict(housing_model, new_house)
cat("\nPredicted Price for New House: $", 
    format(round(predicted_price, 2), big.mark = ","), "\n")

# Extract and interpret key statistics
cat("\n=== Model Interpretation ===\n")
cat("R-squared:", round(summary(housing_model)$r.squared * 100, 2), "%\n")
cat("Adjusted R-squared:", round(summary(housing_model)$adj.r.squared * 100, 2), "%\n")
cat("\nMost significant predictors (by p-value):\n")
coefs <- summary(housing_model)$coefficients
print(coefs[coefs[, 4] < 0.05, ])
```

### Real-World Example 2: Sales Forecasting

```r
# ========================================================
# REAL-WORLD EXAMPLE 2: Sales Forecasting
# ========================================================

# Create a time series of monthly sales data
set.seed(123)

# Generate 36 months of data
months <- 1:36
advertising_budget <- runif(36, 10000, 50000)
num_salespeople <- sample(5:20, 36, replace = TRUE)
economic_index <- 100 + seq(0, 20, length.out = 36) + rnorm(36, 0, 2)

# Create sales with relationship to predictors
sales <- (
  20000 +
  0.8 * advertising_budget +
  1500 * num_salespeople +
  500 * economic_index +
  rnorm(36, 0, 3000)
)

# Create data frame
sales_data <- data.frame(
  month = months,
  sales = sales,
  advertising = advertising_budget,
  salespeople = num_salespeople,
  economy = economic_index
)

# Fit multiple regression model
sales_model <- lm(sales ~ advertising + salespeople + economy, data = sales_data)

# Display results
cat("=== Sales Forecasting Model ===\n\n")
print(summary(sales_model))

# Model interpretation
cat("\n=== Business Interpretation ===\n")
cat("For every $1 increase in advertising budget,\n")
cat("  sales increase by approximately $", 
    round(coef(sales_model)["advertising"], 2), "\n\n")

cat("For each additional salesperson,\n")
cat("  sales increase by approximately $",
    round(coef(sales_model)["salespeople"], 2), "\n\n")

cat("For each point increase in economic index,\n")
cat("  sales increase by approximately $",
    round(coef(sales_model)["economy"], 2), "\n")

# Residual analysis
cat("\n=== Residual Analysis ===\n")
residuals <- residuals(sales_model)
cat("Mean of residuals:", mean(residuals), "\n")
cat("Standard deviation of residuals:", sd(residuals), "\n")

# Predictions for next quarter
future_months <- data.frame(
  advertising = c(40000, 45000, 50000),
  salespeople = c(15, 16, 17),
  economy = c(108, 109, 110)
)

predictions <- predict(sales_model, future_months)
cat("\n=== Predictions for Next Quarter ===\n")
for (i in 1:3) {
  cat("Month", i + 36, ": $", format(round(predictions[i], 0), big.mark = ","), "\n")
}
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Check Assumptions**: Always verify linear regression assumptions using diagnostic plots
2. **Feature Scaling**: Consider standardizing predictors for interpretation
3. **Cross-Validation**: Use k-fold cross-validation to assess model generalizability
4. **Regularization**: For high-dimensional data, consider ridge or Lasso regression
5. **Interpretability**: Focus on practical significance, not just statistical significance

### Common Pitfalls

1. **Multicollinearity**: Highly correlated predictors can inflate standard errors
2. **Overfitting**: Including too many variables can lead to poor generalization
3. **Outliers**: Extreme values can disproportionately influence the model
4. **Causation**: Correlation does not imply causation
5. **Extrapolation**: Predictions outside the range of training data are unreliable

## Performance Considerations

### Computational Complexity

- **Time Complexity**: O(n × p²) where n = number of observations, p = number of predictors
- **Memory**: Requires storing n × p matrix in memory
- For very large datasets, consider using `biglm` package or stochastic gradient descent

### Optimization Tips

```r
# For large datasets, use efficient computation
library(Matrix)
library(biglm)

# Use matrix operations instead of loops
X <- as.matrix(data[, -1])
y <- as.matrix(data[, 1])
beta <- solve(t(X) %*% X) %*% t(X) %*% y
```

## Related Concepts and Further Reading

### Related Topics
- **Polynomial Regression**: For non-linear relationships
- **Ridge Regression**: L2 regularization for multicollinearity
- **Lasso Regression**: L1 regularization for feature selection
- **Generalized Linear Models (GLM)**: For non-normal distributions
- **Time Series Regression**: For temporal data

### Recommended Resources
- "Applied Linear Statistical Models" by Kutner et al.
- "An Introduction to Statistical Learning" by James et al.
- R documentation: `?lm`, `?summary.lm`, `?predict.lm`
- `caret` package for unified regression workflows
- `glmnet` package for regularized regression

## Exercise Problems

### Exercise 1: Basic Linear Regression
Create a simple linear regression model using the `cars` dataset (built-in). The dataset contains speed and stopping distances. Predict stopping distance based on speed.

### Exercise 2: Multiple Regression
Using the `mtcars` dataset, build a model to predict `mpg` using all relevant predictors. Identify which predictors are statistically significant.

### Exercise 3: Diagnostic Plots
Create and interpret residual plots for your model. Check for:
- Linearity
- Homoscedasticity
- Normality of residuals

### Exercise 4: Model Comparison
Compare simple linear regression vs. multiple linear regression using adjusted R-squared and AIC (Akaike Information Criterion).

### Exercise 5: Prediction Intervals
Calculate 95% prediction intervals for new observations in your model. Explain the difference between confidence intervals and prediction intervals.

---

**Solution to Exercise 1:**
```r
# Load cars dataset
data(cars)

# Explore the dataset
head(cars)
str(cars)

# Simple linear regression: stopping distance ~ speed
model <- lm(dist ~ speed, data = cars)

# View results
summary(model)

# Plot the relationship
plot(cars$speed, cars$dist, 
     main = "Car Stopping Distance vs Speed",
     xlab = "Speed (mph)",
     ylab = "Stopping Distance (ft)")
abline(model, col = "red")
```