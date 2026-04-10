# Seasonal Decomposition in R

## Title
Time Series Decomposition: Trend, Seasonal, and Residual Components

## Objectives
- Understand the components of a time series (trend, seasonality, residual)
- Apply classical, STL, and SEATS decomposition methods
- Perform seasonal adjustment
- Visualize and interpret decomposition results

## Introduction

Time series decomposition separates a time series into its fundamental components: trend, seasonal, and irregular (residual) components. This is essential for understanding the underlying patterns in data.

## Classical Decomposition

The classical decomposition method uses moving averages to extract trend and seasonal components.

```r
# Load forecast package
library(forecast)

# Create sample time series with trend and seasonality
set.seed(456)
t <- 1:120
trend <- 0.5 * t
seasonal <- 15 * sin(2 * pi * t / 12)
noise <- rnorm(120, 0, 3)
ts_data <- ts(200 + trend + seasonal + noise, 
              start = c(2010, 1), frequency = 12)

# Plot the original time series
plot(ts_data, main = "Original Time Series", xlab = "Year", ylab = "Value")

# Classical decomposition - additive model
# Trend + Seasonal + Irregular
decomp_additive <- decompose(ts_data, type = "additive")
print(decomp_additive)

# Plot decomposition components
plot(decomp_additive)

# Classical decomposition - multiplicative model
# Trend * Seasonal * Irregular (for data with proportional seasonality)
ts_mult <- ts(exp(log(200) + log(1 + trend/200) + log(1 + seasonal/100)), 
              start = c(2010, 1), frequency = 12)
decomp_mult <- decompose(ts_mult, type = "multiplicative")
plot(decomp_mult)

# Extract individual components
trend_comp <- decomp_additive$trend
seasonal_comp <- decomp_additive$seasonal
random_comp <- decomp_additive$random

# Plot trend
plot(trend_comp, main = "Trend Component", xlab = "Year", ylab = "Trend")

# Plot seasonal pattern
plot(seasonal_comp, main = "Seasonal Pattern", xlab = "Month", 
     ylab = "Seasonal Effect")
```

## STL Decomposition

STL (Seasonal and Trend decomposition using Loess) is more robust and flexible than classical decomposition.

```r
# STL decomposition
stl_decomp <- stl(ts_data, s.window = "periodic", robust = TRUE)
print(summary(stl_decomp))

# Plot STL decomposition
plot(stl_decomp, main = "STL Decomposition")

# Customize STL options
# s.window: seasonal smoother window (number of years)
# t.window: trend smoother window
# robust: outlier-robust fitting

stl_decomp2 <- stl(ts_data, 
                   s.window = 13,    # 13-period seasonal window
                   t.window = 27,    # 27-period trend window  
                   robust = TRUE)
plot(stl_decomp2)

# Extract components from STL
stl_trend <- stl_decomp2$time.series[, "trend"]
stl_seasonal <- stl_decomp2$time.series[, "seasonal"]
stl_remainder <- stl_decomp2$time.series[, "remainder"]

# Convert to standard time series components
as.ts(stl_seasonal)
```

## Seasonal Adjustment

Seasonal adjustment removes seasonal effects to obtain seasonally adjusted data.

```r
# Seasonal adjustment using classical decomposition
seasonally_adjusted <- ts_data - decomp_additive$seasonal
plot( seasonally_adjusted, 
      main = "Seasonally Adjusted Series",
      xlab = "Year", 
      ylab = "Adjusted Value")

# Seasonal adjustment using STL
seasonally_adjusted_stl <- seasadj(stl_decomp)
plot(seasonally_adjusted_stl,
     main = "STL Seasonally Adjusted",
     xlab = "Year",
     ylab = "Adjusted Value")

# Compare original and adjusted
plot(ts_data, col = "gray", main = "Original vs Seasonally Adjusted")
lines(seasonally_adjusted_stl, col = "red")
legend("topleft", c("Original", "Adjusted"), 
       col = c("gray", "red"), lty = 1)
```

## SEATS Decomposition

SEATS (Seasonal Extraction in ARIMA Time Series) is widely used in official statistics.

```r
# Install and load seasonal package
install.packages("seasonal")
library(seasonal)

# SEATS decomposition
# Note: require stsm or seasonal package
# This example uses the stl approach as SEATS alternative

# Convert to mts (multiple time series) for SEATS
# SEATS requires the data to have proper dates
ts_seats <- ts_data
class(ts_seats)

# For SEATS, use final() to convert seasonal adjustment
# (requires seasonal package with preinstalled seasonal)
```

## Handling Multiple Seasonal Patterns

```r
# Multiple seasonal decomposition
# For data with multiple frequencies

# Create data with daily pattern + weekly pattern
set.seed(789)
n <- 365 * 24  # hourly data for one year
hourly_trend <- 0.01 * (1:n)

# Daily pattern (24 hours)
daily_pattern <- rep(rep(c(sin(2 * pi * (1:24) / 24), 
                          cos(2 * pi * (1:24) / 24)), each = 24), 
                    length.out = n)

# Weekly pattern (168 hours)
weekly_pattern <- rep(rep(c(rep(5, 24*5), rep(0, 24*2)), 
                         each = 24 * 7 / 7), length.out = n)

# Combine
hourly_data <- ts(100 + hourly_trend + 10*daily_pattern + 5*weekly_pattern + rnorm(n, 0, 1),
                 frequency = 24)

# Decompose
hourly_decomp <- stl(hourly_data, s.window = "periodic")
plot(hourly_decomp)
```

## Seasonal Pattern Analysis

```r
# Extract and analyze seasonal pattern
seasonal_pattern <- decomp_additive$seasonal

# Get one complete seasonal cycle
one_cycle <- window(seasonal_pattern, start = c(2010, 1), end = c(2010, 12))
print(one_cycle)

# Plot seasonal pattern by month/quarter
months <- c("Jan", "Feb", "Mar", "Apr", "May", "Jun",
           "Jul", "Aug", "Sep", "Oct", "Nov", "Dec")
barplot(one_cycle, names.arg = months, 
       main = "Seasonal Effects by Month",
       xlab = "Month",
       ylab = "Seasonal Effect")

# Seasonal strength calculation
seasonal_strength <- 1 - (var(stl_remainder) / var(stl_seasonal + stl_remainder))
print(paste("Seasonal Strength:", seasonal_strength))
```

## Summary

- Decomposition separates time series into trend, seasonal, and residual components
- Classical decomposition uses simple moving averages
- STL provides robust, flexible decomposition with customizable windows
- Seasonal adjustment is essential for economic analysis
- Multiple seasonal patterns can be handled with appropriate frequency settings
- Understanding decomposition is foundational for accurate forecasting