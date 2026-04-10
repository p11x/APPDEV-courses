# Forecasting Methods in R

## Title
Time Series Forecasting with ARIMA and Exponential Smoothing

## Objectives
- Learn to use the forecast package for time series prediction
- Understand ARIMA (AutoRegressive Integrated Moving Average) models
- Implement exponential smoothing methods (ETS)
- Evaluate and compare forecasting accuracy

## Introduction

The forecast package provides comprehensive tools for time series forecasting, implementing state-of-the-art methods including ARIMA and exponential smoothing models.

## Installing and Loading the forecast Package

```r
# Install forecast package
install.packages("forecast")
install.packages("tseries")

# Load libraries
library(forecast)
library(tseries)
```

## Exponential Smoothing (ETS Models)

Exponential smoothing methods assign exponentially decreasing weights to past observations.

```r
# Create sample time series data
set.seed(123)
# Simulate a time series with trend and seasonality
t <- 1:120
trend <- 0.1 * t
seasonal <- 10 * sin(2 * pi * t / 12)
noise <- rnorm(120, 0, 2)
ts_data <- ts(100 + trend + seasonal + noise, 
              start = c(2010, 1), frequency = 12)

# Simple Exponential Smoothing (SES)
# Good for series with no trend or seasonality
ses_model <- ets(ts_data, model = "ANN")  # Additive error, No trend, No seasonality
print(summary(ses_model))

# Holt's method (with trend)
holt_model <- ets(ts_data, model = "AAN")  # Additive error, Additive trend, No seasonality
print(summary(holt_model))

# Holt-Winters (with trend and seasonality)
# Additive seasonality
hw_add <- ets(ts_data, model = "AAA")  
print(summary(hw_add))

# Multiplicative seasonality
hw_mult <- ets(ts_data, model = "MAM")  
print(summary(hw_mult))

# Forecast using Holt-Winters
forecast_hw <- forecast(hw_add, h = 24)  # Forecast 24 periods ahead
print(forecast_hw)

# Plot forecast
plot(forecast_hw, 
     main = "Holt-Winters Forecast",
     xlab = "Time",
     ylab = "Value")
```

## ARIMA Models

ARIMA models combine autoregressive (AR), differencing (I), and moving average (MA) components.

```r
# Check stationarity with ACF and PACF
acf(ts_data, main = "Autocorrelation Function")
pacf(ts_data, main = "Partial Autocorrelation Function")

# Augmented Dickey-Fuller test for stationarity
adf_result <- adf.test(ts_data)
print(adf_result)

# If non-stationary, difference the series
diff_ts <- diff(ts_data)
adf_test_diff <- adf.test(diff_ts)
print(adf_test_diff)

# Automatic ARIMA model selection
auto_arima <- auto.arima(ts_data)
print(summary(auto_arima))

# Manual ARIMA specification
# ARIMA(p,d,q) - p: AR order, d: differencing, q: MA order
manual_arima <- arima(ts_data, order = c(1, 1, 1))
print(summary(manual_arima))

# Seasonal ARIMA (SARIMA)
# SARIMA(p,d,q)(P,D,Q)[s] - s is seasonal period
seasonal_arima <- arima(ts_data, 
                       order = c(1, 1, 1),
                       seasonal = list(order = c(1, 1, 1), period = 12))
print(summary(seasonal_arima))
```

## Forecasting with Models

```r
# Forecast with ETS model
ets_forecast <- forecast(ets(ts_data), h = 36)
plot(ets_forecast, 
     main = "ETS Forecast",
     xlab = "Year",
     ylab = "Value")

# Forecast with ARIMA model
arima_forecast <- forecast(auto_arima, h = 36)
plot(arima_forecast, 
     main = "ARIMA Forecast",
     xlab = "Year",
     ylab = "Value")

# Confidence intervals
print(arima_forecast)
# $lower contains lower bounds
# $upper contains upper bounds
# $mean contains point forecasts
```

## Model Evaluation and Comparison

```r
# Split data into training and test sets
train <- window(ts_data, end = c(2018, 12))
test <- window(ts_data, start = c(2019, 1))

# Fit models on training data
ets_fit <- ets(train, model = "AAA")
arima_fit <- auto.arima(train)

# Forecast on test period
ets_fc <- forecast(ets_fit, h = length(test))
arima_fc <- forecast(arima_fit, h = length(test))

# Calculate accuracy metrics
ets_accuracy <- accuracy(ets_fc, test)
arima_accuracy <- accuracy(arima_fc, test)

print("ETS Model Accuracy:")
print(ets_accuracy)

print("ARIMA Model Accuracy:")
print(arima_accuracy)

# Compare models using AIC
AIC(ets_fit)
AIC(arima_fit)

# Residual diagnostics
checkresiduals(arima_fit)
```

## Advanced Forecasting Options

```r
# Theta method (often performs well in competitions)
theta_model <- thetaf(ts_data, h = 24)
plot(theta_model)

# Cross-validation for time series
tsCV(ts_data, forecastfunction = auto.arima, h = 1)

# Ensemble forecasting
# Combine multiple models
combined_forecast <- (ets_fc$mean + arima_fc$mean) / 2
plot(combined_forecast, type = "l")

# Box-Cox transformation for variance stabilization
lambda <- BoxCox.lambda(ts_data)
transformed_ts <- BoxCox(ts_data, lambda)
print(lambda)
```

## Summary

- The forecast package provides comprehensive forecasting tools
- ETS models handle trend and seasonality through exponential smoothing
- ARIMA models capture autoregressive and moving average patterns
- auto.arima() automates model selection
- Model accuracy is measured using MAE, RMSE, MAPE
- Always perform residual diagnostics to validate model assumptions