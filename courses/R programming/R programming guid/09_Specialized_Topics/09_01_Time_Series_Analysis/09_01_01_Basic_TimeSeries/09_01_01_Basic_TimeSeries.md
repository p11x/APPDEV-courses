# Basic Time Series in R

## Title
Introduction to Time Series Objects and Basic Operations in R

## Objectives
- Understand the fundamental structure of time series objects in R
- Learn how to create and manipulate time series using `ts()` and `as.ts()` functions
- Explore the zoo package for irregular time series
- Perform basic time series operations and transformations

## Introduction to Time Series

Time series data represents observations collected at successive points in time, typically at regular intervals. R provides several classes and packages for handling time series data efficiently.

## Creating Time Series Objects with ts()

The `ts()` function creates a regular time series object in R.

```r
# Create a simple time series with monthly data (1980-1989)
data <- c(10, 12, 15, 18, 20, 22, 25, 28, 30, 32, 35, 38,
          40, 42, 45, 48, 50, 52, 55, 58, 60, 62, 65, 68,
          70, 72, 75, 78, 80, 82, 85, 88, 90, 92, 95, 98,
          100, 102, 105, 108, 110, 112, 115, 118, 120, 122, 125, 128)

# Monthly frequency (12 observations per year)
monthly_ts <- ts(data, start = c(1980, 1), frequency = 12)
print(monthly_ts)

# Quarterly frequency (4 observations per year)
quarterly_ts <- ts(data, start = c(1980, 1), frequency = 4)
print(quarterly_ts)

# Annual frequency
annual_ts <- ts(data, start = 1980, frequency = 1)
print(annual_ts)
```

## Converting to Time Series with as.ts()

The `as.ts()` function converts other objects to time series.

```r
# Create a vector and convert to time series
my_vector <- c(5, 10, 15, 20, 25, 30)
converted_ts <- as.ts(my_vector, start = 2000, frequency = 4)
print(converted_ts)

# Convert a data frame column to time series
df <- data.frame(year = 2010:2019, value = c(100, 110, 105, 120, 125, 
                                               130, 135, 140, 145, 150))
ts_from_df <- ts(df$value, start = 2010, frequency = 1)
print(ts_from_df)
```

## Time Series Basics

```r
# Basic time series operations
# Extract start and end times
start(monthly_ts)  # Returns c(1980, 1)
end(monthly_ts)    # Returns c(1989, 12)

# Get frequency
frequency(monthly_ts)  # Returns 12

# Get time (vector of time points)
time(monthly_ts)

# Get cycle (seasonal cycle index)
cycle(monthly_ts)

# Window - subset time series
window(monthly_ts, start = c(1985, 1), end = c(1987, 12))

# Head and tail
head(monthly_ts, 12)  # First year
tail(monthly_ts, 12)  # Last year

# Lag and diff
lag(monthly_ts, 1)    # Shift by 1 period
diff(monthly_ts)      # First difference
```

## The zoo Package

The zoo package handles both regular and irregular time series.

```r
# Install and load zoo package
# install.packages("zoo")
library(zoo)

# Create regular zoo object
dates <- as.Date("2020-01-01") + 0:11
values <- c(10, 15, 12, 18, 20, 25, 22, 28, 30, 35, 32, 40)
regular_zoo <- zoo(values, order.by = dates)
print(regular_zoo)

# Create irregular zoo object (irregular intervals)
irregular_dates <- as.Date(c("2020-01-01", "2020-01-05", "2020-01-15", 
                              "2020-02-01", "2020-02-20", "2020-03-10"))
irregular_values <- c(100, 105, 110, 115, 120, 125)
irregular_zoo <- zoo(irregular_values, order.by = irregular_dates)
print(irregular_zoo)

# zoo operations
head(irregular_zoo)
tail(irregular_zoo)
window(irregular_zoo, start = "2020-01-10", end = "2020-02-15")

# Merge two zoo objects
zoo1 <- zoo(1:5, as.Date("2020-01-01") + 0:4)
zoo2 <- zoo(5:1, as.Date("2020-01-03") + 0:4)
merged <- merge(zoo1, zoo2)
print(merged)

# Fill NA with last observation (na.locf)
zoo_with_na <- zoo(c(1, NA, 3, NA, 5), as.Date("2020-01-01") + 0:4)
na.locf(zoo_with_na)
```

## Working with ts and zoo Together

```r
# Convert between ts and zoo
# ts to zoo
ts_to_zoo <- as.zoo(monthly_ts)
print(ts_to_zoo)

# zoo to ts (requires regular intervals)
zoo_to_ts <- as.ts(regular_zoo)
print(zoo_to_ts)

# Example: Add custom attributes to time series
attr(monthly_ts, "source") <- "Generated data"
attr(monthly_ts, "description") <- "Monthly sales data"
print(attributes(monthly_ts))
```

## Summary

- `ts()` creates regular time series objects with specified start, end, and frequency
- `as.ts()` converts other objects to time series format
- The zoo package extends time series capabilities to irregular time intervals
- Basic operations include window(), lag(), diff(), start(), end(), and frequency()
- Understanding these fundamentals is essential for advanced time series analysis