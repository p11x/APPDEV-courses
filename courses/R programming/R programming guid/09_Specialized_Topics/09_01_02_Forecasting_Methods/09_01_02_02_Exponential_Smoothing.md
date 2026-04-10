# Exponential Smoothing Methods

## Learning Objectives

- Understand exponential smoothing theory
- Implement simple exponential smoothing
- Use Holt's method for trends
- Apply Holt-Winters for seasonality

## Theory

Exponential smoothing assigns exponentially decreasing weights to observations. More recent observations get higher weights. The forecast is a weighted average of all past observations with recent values weighted more heavily.

## Step-by-Step Guide

### Simple Exponential Smoothing

For series with no trend or seasonality:

```r
library(forecast)

# Simple exponential smoothing
fit <- ses(goog, alpha = 0.2)

# Forecast
forecast(fit, h = 10)

# The alpha parameter controls smoothing
# Lower alpha = more smoothing
# Higher alpha = less smoothing
```

### Holt's Linear Trend

Add trend component:

```r
# Holt's linear trend method
fit <- holt(air passengers)

# Damped trend (more conservative)
fit <- holt(air passengers, damped = TRUE)

# Forecast
forecast(fit, h = 24)
```

### Holt-Winters Method

Add seasonal component:

```r
# Additive seasonality
fit_hw <- hw(AirPassengers, 
             seasonal = "additive",
             h = 24)

# Multiplicative seasonality
fit_hw <- hw(AirPassengers, 
              seasonal = "multiplicative",
              h = 24)
```

## Code Examples

### Tuning Parameters

```r
# Manual parameters
fit <- ets(y, 
           model = "ANN",  # Simple exponential smoothing
           alpha = 0.3)

# Auto selection
fit_auto <- ets(y)

# ETS model components
# Error: A (additive), M (multiplicative)
# Trend: N (none), A (additive), M (multiplicative), Ad (additive damped)
# Seasonal: N (none), A (additive), M (multiplicative), Ad (additive damped)
```

### Model Selection

```r
# Compare models
fit1 <- ses(train)
fit2 <- holt(train)
fit3 <- hw(train)

# Cross-validation
library(fable)
accuracy(fit1, test)
accuracy(fit2, test)
accuracy(fit3, test)
```

### Plotting Forecasts

```r
# Plot forecast
autoplot(forecast(fit))

# Plot components
autoplot(fit) +
  xlab("Year") +
  ggtitle("Exponential Smoothing Forecast")
```

## Best Practices

1. **Check Residuals**: Ensure no patterns in residuals.

2. **Compare Methods**: Try multiple smoothing methods.

3. **Validate**: Use time series cross-validation.

4. **Consider Seasonality**: Apply seasonal methods when needed.

5. **Damp Trends**: Use damping for long forecasts.

## Exercises

1. Apply simple exponential smoothing.

2. Use Holt's method for trend.

3. Apply Holt-Winters with seasonality.

4. Compare different models with AIC.

5. Plot and interpret forecasts.

## Additional Resources

- [forecast Package](https://otexts.com/fpp3/)
- [Exponential Smoothing](https://robjhyndman.com/publications/)
- [fable Package](https://fable.tidyverts.org/)