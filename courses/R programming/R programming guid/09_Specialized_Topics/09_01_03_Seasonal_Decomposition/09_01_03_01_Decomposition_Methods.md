# Seasonal Decomposition Methods

## Learning Objectives

- Understand time series decomposition
- Apply classical decomposition
- Use STL for robust decomposition
- Interpret components

## Theory

Time series can be decomposed into trend, seasonal, and remainder components. Decomposition helps understand the underlying patterns and is useful for forecasting. Two main approaches are additive and multiplicative decomposition.

## Step-by-Step Guide

### Classical Decomposition

```r
# Additive decomposition
decomp <- decompose(my_ts, type = "additive")

# Multiplicative decomposition
decomp <- decompose(my_ts, type = "multiplicative")

# View components
plot(decomp)

# Components
decomp$trend
decomp$seasonal
decomp$random
```

### STL Decomposition

Robust to outliers:

```r
library(forecast)

# STL decomposition
stl_fit <- stl(my_ts, s.window = "periodic")

# Plot
plot(stl_fit)

# Seasonally adjusted
seasonally_adjust <- seasadj(stl_fit)
```

### Seasonal Adjustment

```r
# Remove seasonal component
adj <- seasadj(decomp)

# Forecast with seasonal adjustment
fit <- ets(adj)
forecast(fit, h = 12)
```

## Code Examples

### Moving Average Trend

```r
# Calculate trend using moving average
library(zoo)

trend <- rollapply(my_ts, 
                   width = 12, 
                   FUN = mean, 
                   align = "center")

# Detrended series
detrended <- my_ts - trend
```

### X-13-ARIMA-SEATS

```r
library(seasonal)

# Seasonal adjustment
adj_data <- seas(my_ts)

# Plot original vs adjusted
plot(adj_data)

# Predefined regression
seas_x13 <- seas(my_ts, 
                 x13 = "")
```

## Best Practices

1. **Check Seasonality**: Visualize before decomposing.

2. **Choose Model**: Additive vs multiplicative.

3. **Handle Missing**: Ensure no gaps in data.

4. **Use STL**: More robust than classical.

5. **Validate**: Check randomness in remainder.

## Exercises

1. Apply classical decomposition.

2. Use STL for robust decomposition.

3. Compare additive vs multiplicative.

4. Create seasonally adjusted series.

5. Plot and interpret components.

## Additional Resources

- [STL Decomposition](https://otexts.com/fpp3/decomposition.html)
- [seasonal Package](https://cran.r-project.org/web/packages/seasonal/)
- [forecast Package](https://cran.r-project.org/web/packages/forecast/)