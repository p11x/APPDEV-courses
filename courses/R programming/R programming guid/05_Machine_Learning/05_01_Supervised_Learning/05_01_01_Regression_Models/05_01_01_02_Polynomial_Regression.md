# Polynomial Regression in R

## Learning Objectives

- Understand when and why to use polynomial regression
- Implement polynomial regression models in R
- Evaluate model fit and determine optimal polynomial degree
- Avoid overfitting through proper model selection

## Theoretical Background

### What is Polynomial Regression?

Polynomial regression extends linear regression by adding polynomial terms. While linear regression fits a straight line, polynomial regression can fit curved relationships:

$$Y = \beta_0 + \beta_1 X + \beta_2 X^2 + \beta_3 X^3 + ... + \epsilon$$

### When to Use Polynomial Regression

1. When relationship between variables is curvilinear
2. When data shows non-linear patterns
3. When simple linear model has high residual error

### Choosing the Right Degree

- **Degree 1**: Linear (straight line)
- **Degree 2**: Quadratic (parabola)
- **Degree 3**: Cubic
- Higher degrees: More flexible but risk overfitting

## Code Examples

### Standard Example

```r
# Polynomial regression example
x <- 1:20
y <- x^2 + rnorm(20, 0, 10)  # Quadratic relationship with noise

# Fit polynomial models of different degrees
model1 <- lm(y ~ x)
model2 <- lm(y ~ x + I(x^2))
model3 <- lm(y ~ x + I(x^2) + I(x^3))

# Compare R-squared
cat("Degree 1 R²:", summary(model1)$r.squared, "\n")
cat("Degree 2 R²:", summary(model2)$r.squared, "\n")
cat("Degree 3 R²:", summary(model3)$r.squared, "\n")
```

### Real-World Example: Economic Growth

```r
# GDP growth analysis with polynomial regression
gdp_data <- data.frame(
  year = 1990:2020,
  gdp_growth = c(3.5, 2.1, 4.2, 3.8, -0.5, 2.8, 4.5, 3.2, 1.9, 3.1,
                2.5, 1.8, 3.9, 4.1, 2.7, 3.0, 2.2, 1.5, 2.8, 3.4,
                2.1, 3.5, 2.9, 1.2, 2.6, 3.8, 4.0, 3.1, 2.4, 3.2, 1.8)
)

# Fit quadratic model
poly_model <- lm(gdp_growth ~ year + I(year^2), data = gdp_data)
summary(poly_model)

# Plot with polynomial fit
plot(gdp_data$year, gdp_data$gdp_growth, main = "GDP Growth")
lines(gdp_data$year, predict(poly_model), col = "red")
```

## Best Practices

1. Don't use degree > 3 without strong justification
2. Use cross-validation to select optimal degree
3. Always plot the fitted curve to visualize fit