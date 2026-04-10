# Time Series Objects in R

## Learning Objectives

- Understand ts class for time series
- Create time series objects
- Work with ts properties
- Apply time series operations

## Theory

Time series data is data collected at regular intervals over time. The base R ts class provides a simple way to store and work with time series data. Time series objects have start time, end time, frequency, and data values.

## Step-by-Step Guide

### Creating ts Objects

```r
# Basic creation
ts_data <- ts(data_vector, start = c(year, period), frequency = 12)

# Monthly data starting January 2020
monthly_ts <- ts(rnorm(24), start = c(2020, 1), frequency = 12)

# Quarterly data
quarterly_ts <- ts(rnorm(20), start = c(2015, 1), frequency = 4)

# Annual data
annual_ts <- ts(rnorm(10), start = 2010, frequency = 1)
```

### Time Series Properties

```r
# Start time
start(my_ts)

# End time
end(my_ts)

# Frequency
frequency(my_ts)

# Time
time(my_ts)
```

## Code Examples

### Creating ts from Data Frame

```r
# From data frame
df <- data.frame(
  date = seq(as.Date("2020-01-01"), by = "month", length.out = 24),
  value = rnorm(24)
)

# Convert to ts
ts_data <- ts(df$value, start = c(2020, 1), frequency = 12)

# Using zoo for irregular times
library(zoo)
zoo_data <- zoo(df$value, order.by = df$date)
```

### Subsetting Time Series

```r
# Window function
window(my_ts, start = c(2021, 1), end = c(2022, 12))

# Subset with time
my_ts[time(my_ts) >= 2021]

# Subset with index
my_ts[1:12]
```

### Lag and Diff

```r
# Lag
lag(my_ts, k = 1)
lag(my_ts, k = -1)

# Difference
diff(my_ts)
diff(my_ts, differences = 2)
```

## Best Practices

1. **Set Correct Start/End**: Ensure proper time range.

2. **Use Appropriate Frequency**: Monthly = 12, Quarterly = 4.

3. **Handle Missing**: Use na.approx for missing values.

4. **Check Properties**: Verify ts properties after creation.

## Exercises

1. Create ts object from a vector.

2. Create monthly time series from data frame.

3. Use window to subset time periods.

4. Apply lag and diff functions.

5. Convert between frequencies.

## Additional Resources

- [ts class documentation](https://stat.ethz.ch/R-manual/R-devel/library/stats/html/ts.html)
- [Time Series Analysis in R](https://cran.r-project.org/doc/manuals/R-intro.pdf)
- [zoo Package](https://cran.r-project.org/web/packages/zoo/)