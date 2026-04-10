# Time Objects in R

## Learning Objectives

- Work with Date class
- Use POSIXct and POSIXlt for date-time
- Parse date strings
- Perform date arithmetic
- Format dates for output

## Theory

R has several classes for handling dates and times. Date handles calendar dates without time. POSIXct stores dates as seconds since Unix epoch. POSIXlt stores components in a list. lubridate package provides easier parsing and manipulation.

Date arithmetic works naturally in R, allowing addition, subtraction, and comparison. Different formats exist for input/output, requiring careful parsing.

## Step-by-Step

1. Determine date type needed (Date vs POSIXct)
2. Parse strings with as.Date() or as.POSIXct()
3. Perform arithmetic operations
4. Format output with format()
5. Extract components with accessor functions

## Code Examples

### Date Class

```r
cat("===== DATE OPERATIONS =====\n\n")

# Create dates
today <- Sys.Date()
cat("Today:", today, "\n")
cat("Class:", class(today), "\n")

# Parse date strings
date1 <- as.Date("2024-01-15")
date2 <- as.Date("01/15/2024", format = "%m/%d/%Y")
cat("Parsed:", date2, "\n")

# Date arithmetic
cat("\nDate arithmetic:\n")
cat("Tomorrow:", today + 1, "\n")
cat("Week ago:", today - 7, "\n")
cat("Days between:", today - as.Date("2024-01-01"), "\n")
```

### POSIXct and POSIXlt

```r
cat("\n===== POSIX DATE-TIME =====\n\n")

# Current time
now <- Sys.time()
cat("Now:", now, "\n")
cat("Class:", class(now), "\n")

# Parse datetime strings
datetime1 <- as.POSIXct("2024-01-15 10:30:00")
datetime2 <- as.POSIXct("01/15/2024 10:30", format = "%m/%d/%Y %H:%M")
cat("Parsed:", datetime2, "\n")

# POSIXlt components
lt <- as.POSIXlt(now)
cat("\nComponents:\n")
cat("Year:", lt$year + 1900, "\n")
cat("Month:", lt$mon + 1, "\n")
cat("Day:", lt$mday, "\n")
cat("Hour:", lt$hour, "\n")
cat("Minute:", lt$min, "\n")
```

### Date Extraction and Formatting

```r
cat("\n===== EXTRACT AND FORMAT =====\n\n")

# Extract components
date <- as.Date("2024-03-15")
cat("Year:", format(date, "%Y"), "\n")
cat("Month:", format(date, "%m"), "\n")
cat("Day:", format(date, "%d"), "\n")
cat("Weekday:", format(date, "%A"), "\n")
cat("Month name:", format(date, "%B"), "\n")

# Custom formats
cat("\nCustom formats:\n")
cat(format(date, "%B %d, %Y"), "\n")
cat(format(date, "%d/%m/%y"), "\n")
```

### Date Sequences and Ranges

```r
cat("\n===== DATE SEQUENCES =====\n\n")

# Sequence of dates
seq_dates <- seq(from = as.Date("2024-01-01"), 
                 to = as.Date("2024-01-31"), 
                 by = "week")
cat("Weekly sequence:\n")
print(seq_dates)

# By day
seq_daily <- seq(as.Date("2024-01-01"), length.out = 10, by = "day")
cat("\n10 days:", paste(seq_daily, collapse = ", "), "\n")

# By month
seq_monthly <- seq(as.Date("2024-01-01"), by = "month", length.out = 6)
cat("\nMonthly:", paste(seq_monthly, collapse = ", "), "\n")
```

### Using lubridate

```r
cat("\n===== LUBRIDATE =====\n\n")

library(lubridate)

cat("Parse various formats:\n")
cat("ymd():", ymd("20240115"), "\n")
cat("dmy():", dmy("15/01/2024"), "\n")
cat("mdy():", mdy("01/15/2024"), "\n")

dt <- ymd_hms("2024-01-15 10:30:45")
cat("\nComponents:", year(dt), month(dt), day(dt), "\n")
cat("Hour:", hour(dt), "Minute:", minute(dt), "Second:", second(dt), "\n")

# Easy rounding
cat("\nRounding:\n")
cat("Floor:", floor_date(dt, "day"), "\n")
cat("Ceiling:", ceiling_date(dt, "hour"), "\n")
```

## Real-World Example: Financial Data

```r
# Real-world: Working with financial time series
cat("===== FINANCIAL TIME SERIES =====\n\n")

# Create sample financial data
set.seed(42)
dates <- seq(as.Date("2024-01-01"), by = "day", length.out = 30)
prices <- cumsum(rnorm(30)) + 100

financial_data <- data.frame(
  date = dates,
  price = round(prices, 2)
)

cat("Sample data:\n")
print(tail(financial_data, 5))

# Calculate daily returns
financial_data$return <- c(NA, diff(financial_data$price) / financial_data$price[-nrow(financial_data)])

# Filter by date range
start_date <- as.Date("2024-01-10")
end_date <- as.Date("2024-01-20")
filtered <- financial_data[financial_data$date >= start_date & financial_data$date <= end_date, ]
cat("\nFiltered (Jan 10-20):\n")
print(filtered)

# Monthly summary
financial_data$month <- format(financial_data$date, "%Y-%m")
monthly <- aggregate(price ~ month, data = financial_data, FUN = mean)
cat("\nMonthly average:\n")
print(monthly)
```

## Best Practices

1. Use Date for dates, POSIXct for date-times
2. Specify format when parsing non-standard strings
3. Use lubridate for complex date operations
4. Store dates in ISO format (YYYY-MM-DD)
5. Handle time zones explicitly with tz parameter
6. Use seq() for date sequences

## Exercises

1. Parse various date formats using as.Date()
2. Calculate business days between two dates
3. Create a time series with daily data
4. Extract and analyze monthly patterns
5. Handle dates across time zones