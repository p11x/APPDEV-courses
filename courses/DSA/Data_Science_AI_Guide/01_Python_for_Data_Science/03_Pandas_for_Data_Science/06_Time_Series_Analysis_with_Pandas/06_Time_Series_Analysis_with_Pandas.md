# Time Series Analysis with Pandas

## Table of Contents
- [Introduction](#introduction)
- [Fundamentals](#fundamentals)
- [Implementation](#implementation)
- [Applications](#applications)
- [Output Results](#output-results)
- [Visualization](#visualization)
- [Advanced Topics](#advanced-topics)
- [Conclusion](#conclusion)

---

## Introduction

### What is Time Series?

A time series is a sequence of data points indexed in time order. It's fundamental to financial analysis, forecasting, and temporal data processing.

### Importance in Data Science

- Financial forecasting and trend analysis
- Seasonal pattern detection
- Anomaly detection in temporal data
- Predictive modeling

### Time Series Components

1. **Trend**: Long-term direction
2. **Seasonality**: Repeating patterns
3. **Cyclical**: Irregular fluctuations
4. **Noise**: Random variations

---

## Fundamentals

### Date/Time Handling

```python
import pandas as pd
import numpy as np

# Create date range
dates = pd.date_range('2024-01-01', periods=30, freq='D')
print(dates)

# Create DataFrame with dates
df = pd.DataFrame({
    'date': pd.date_range('2024-01-01', periods=100, freq='H'),
    'value': np.random.randn(100)
})

# Parse datetime
df['date'] = pd.to_datetime(df['date'])

# Extract components
df['year'] = df['date'].dt.year
df['month'] = df['date'].dt.month
df['day'] = df['date'].dt.day
df['hour'] = df['date'].dt.hour
df['weekday'] = df['date'].dt.weekday
df['quarter'] = df['date'].dt.quarter
```

### Setting Index

```python
# Set datetime as index
df.set_index('date', inplace=True)

# Sort by index
df.sort_index(inplace=True)

# Filter by date
df['2024-01':'2024-03']
df['2024']
```

### Resampling

```python
# Create sample data
ts = pd.Series(
    np.random.randn(365),
    index=pd.date_range('2024-01-01', periods=365, freq='D')
)

# Resample (downsampling)
monthly = ts.resample('M').mean()
quarterly = ts.resample('Q').sum()
yearly = ts.resample('Y').sum()

# Resample with multiple functions
resampled = ts.resample('M').agg(['mean', 'sum', 'std', 'min', 'max'])

# Resample (upsampling)
daily = ts.resample('D').ffill()
interpolated = ts.resample('D').interpolate()
```

### Rolling Statistics

```python
# Rolling mean (7-day window)
rolling = ts.rolling(window=7).mean()

# Rolling sum
rolling_sum = ts.rolling(window=7).sum()

# Moving average
ma = ts.rolling(window=30, min_periods=1).mean()

# Rolling standard deviation
rolling_std = ts.rolling(window=30).std()

# Exponential moving average
ema = ts.ewm(span=30).mean()
```

### Shifting

```python
# Shift forward
shifted = ts.shift(1)

# Shift backward
lagged = ts.shift(-1)

# Difference
diff = ts.diff()

# Percentage change
pct_change = ts.pct_change()

# Time difference
time_diff = ts.diff()
```

---

## Implementation

### Example 1: Stock Price Analysis

```python
# Create stock price data
np.random.seed(42)
dates = pd.date_range('2024-01-01', periods=252, freq='B')

stock_data = pd.DataFrame({
    'date': dates,
    'open': 100 + np.cumsum(np.random.randn(252) * 2),
    'high': 0,
    'low': 0,
    'close': 0,
    'volume': np.random.randint(1000000, 5000000, 252)
})

# Calculate high/low/close
stock_data['high'] = stock_data['open'] + np.abs(np.random.randn(252) * 3)
stock_data['low'] = stock_data['open'] - np.abs(np.random.randn(252) * 3)
stock_data['close'] = stock_data['open'] + np.random.randn(252) * 2

# Calculate daily return
stock_data['return'] = stock_data['close'].pct_change()

# Calculate moving averages
stock_data['ma_5'] = stock_data['close'].rolling(window=5).mean()
stock_data['ma_20'] = stock_data['close'].rolling(window=20).mean()
stock_data['ma_50'] = stock_data['close'].rolling(window=50).mean()

stock_data.set_index('date', inplace=True)

print("=== Stock Price Data ===")
print(stock_data.head(20))

# Monthly summary
monthly = stock_data.resample('M').agg({
    'open': 'first',
    'high': 'max',
    'low': 'min',
    'close': 'last',
    'volume': 'sum'
})

print("\n=== Monthly Summary ===")
print(monthly)
```

### Example 2: Website Traffic Analysis

```python
# Create website traffic data
dates = pd.date_range('2024-01-01', periods=168, freq='H')

traffic = pd.DataFrame({
    'datetime': dates,
    'page_views': np.random.randint(100, 1000, 168),
    'unique_visitors': np.random.randint(50, 500, 168),
    'bounce_rate': np.random.uniform(0.2, 0.6, 168)
})

traffic.set_index('datetime', inplace=True)

print("=== Website Traffic ===")
print(traffic.head(24))

# Hourly patterns
hourly_pattern = traffic.groupby(traffic.index.hour).mean()

print("\n=== Hourly Pattern (Average) ===")
print(hourly_pattern)

# Daily summary
daily = traffic.resample('D').sum()

print("\n=== Daily Summary ===")
print(daily)

# Weekly pattern
daily['weekday'] = daily.index.weekday
weekly_pattern = daily.groupby('weekday')['page_views'].mean()

print("\n=== Weekly Pattern ===")
print(weekly_pattern)
```

---

## Applications

### Banking Sector: Transaction Analysis

```python
import pandas as pd
import numpy as np

# Create transaction time series
dates = pd.date_range('2024-01-01', periods=365, freq='D')

transactions = pd.DataFrame({
    'date': dates,
    'deposits': np.abs(np.random.randn(365) * 10000 + 50000),
    'withdrawals': np.abs(np.random.randn(365) * 8000 + 40000),
    'transfers': np.random.randint(50, 500, 365),
    'atm_transactions': np.random.randint(100, 1000, 365)
})

transactions.set_index('date', inplace=True)

print("=== Banking Transactions (First 10 days) ===")
print(transactions.head(10))

# Monthly totals
monthly = transactions.resample('M').sum()

print("\n=== Monthly Totals ===")
print(monthly)

# Add derived metrics
transactions['net_flow'] = transactions['deposits'] - transactions['withdrawals']
transactions['net_flow_pct'] = (transactions['net_flow'] / transactions['deposits']) * 100

# Rolling average
transactions['deposits_ma_7'] = transactions['deposits'].rolling(window=7).mean()
transactions['deposits_ma_30'] = transactions['deposits'].rolling(window=30).mean()

print("\n=== With Moving Averages ===")
print(transactions[['deposits', 'deposits_ma_7', 'deposits_ma_30']].head(35))

# Daily change
transactions['deposits_change'] = transactions['deposits'].diff()
transactions['deposits_pct_change'] = transactions['deposits'].pct_change()

# Monthly and quarterly summary
quarterly = transactions.resample('Q').agg({
    'deposits': 'sum',
    'withdrawals': 'sum',
    'transfers': 'sum'
})

print("\n=== Quarterly Summary ===")
print(quarterly)

# Day of week analysis
transactions['day_of_week'] = transactions.index.dayofweek
dow_summary = transactions.groupby('day_of_week')['deposits'].mean()

print("\n=== Day of Week Average Deposits ===")
days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
for i, day in enumerate(days):
    print(f"{day}: ${dow_summary[i]:,.2f}")
```

### Healthcare Sector: Patient Admissions

```python
# Create patient admission data
dates = pd.date_range('2024-01-01', periods=365, freq='D')

admissions = pd.DataFrame({
    'date': dates,
    'emergency': np.random.randint(10, 50, 365),
    'elective': np.random.randint(20, 80, 365),
    'transfer': np.random.randint(5, 25, 365),
    'outpatient': np.random.randint(100, 300, 365)
})

admissions.set_index('date', inplace=True)

# Total admissions
admissions['total'] = admissions['emergency'] + admissions['elective'] + admissions['transfer']

print("=== Patient Admissions (First 10 days) ===")
print(admissions.head(10))

# Weekly summary
weekly = admissions.resample('W').sum()

print("\n=== Weekly Summary ===")
print(weekly)

# Monthly summary
monthly = admissions.resample('M').sum()

print("\n=== Monthly Summary ===")
print(monthly)

# Adding rolling metrics
admissions['total_ma_7'] = admissions['total'].rolling(window=7).mean()
admissions['total_ma_30'] = admissions['total'].rolling(window=30).mean()

# Emergency trend
admissions['emergency_ma_7'] = admissions['emergency'].rolling(window=7).mean()

print("\n=== With Moving Averages ===")
print(admissions[['total', 'total_ma_7', 'total_ma_30']].head(35))

# Day of week patterns
admissions['day_of_week'] = admissions.index.dayofweek
dow_admissions = admissions.groupby('day_of_week')['total'].mean()

print("\n=== Day of Week Average Admissions ===")
days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
for i, day in enumerate(days):
    print(f"{day}: {dow_admissions[i]:.1f} patients")

# Seasonal analysis
admissions['month'] = admissions.index.month
monthly_pattern = admissions.groupby('month')['total'].mean()

print("\n=== Monthly Pattern (Average) ===")
months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
for i, month in enumerate(months, 1):
    print(f"{month}: {monthly_pattern[i]:.1f} patients")
```

---

## Output Results

### Sample Output - Banking

```
=== Banking Transactions (First 10 days) ===
             deposits  withdrawals  transfers  atm_transactions
date                                                  
2024-01-01  52234.56    43152.23          342            823
2024-01-02  48923.12    38765.89          287            756
2024-01-03  51234.87    41234.56          312            891
2024-01-04  56789.23    45678.90          356            945
2024-01-05  49876.54    40123.45          298            678
...

=== Monthly Totals ===
            deposits  withdrawals  transfers  atm_transactions
date                                                  
2024-01-31  1.68e+06    1.31e+06         8922        25423
2024-02-29  1.52e+06    1.19e+06         8234        23112
2024-03-31  1.79e+06    1.40e+06         9123        26789

=== Day of Week Average Deposits ===
Monday: $52,345.67
Tuesday: $48,234.12
Wednesday: $51,234.56
Thursday: $54,567.89
Friday: $49,876.54
Saturday: $38,234.12
Sunday: $32,456.78
```

### Sample Output - Healthcare

```
=== Patient Admissions (First 10 days) ===
            emergency  elective  transfer  outpatient  total
date                                                   
2024-01-01         32         56        18         245     351
2024-01-02         28         61        15         232     336
2024-01-03         35         58        21         267     381
2024-01-04         31         63        17         251     362
2024-01-05         29         55        19         238     341

=== Monthly Summary ===
            emergency  elective  transfer  outpatient   total
date                                                   
2024-01-31         961      1845        512       7256  10574
2024-02-29         812      1654        487       6891   9844
2024-03-31         978      1892        534       7423  10827

=== Day of Week Average Admissions ===
Monday: 351.2 patients
Tuesday: 342.5 patients
Wednesday: 356.8 patients
Thursday: 348.9 patients
Friday: 334.2 patients
Saturday: 198.7 patients
Sunday: 145.3 patients
```

---

## Visualization

### ASCII - Time Series Components

```
+------------------------------------------------------------------+
|           TIME SERIES COMPONENTS                                      |
+------------------------------------------------------------------+

ORIGINAL DATA           TREND               SEASONALITY
+----------+          +----------+         +----------+
| /\/\/\   |          |    ___   |         | /\/\/\  |
| /\  /\  |   --->   |  /      |   --->  | /\  /\  |
|/      \/|          |/        |         |/      \ |
+----------+          +----------+         +----------+

CYCLICAL              NOISE
+----------+          +----------+
|   /\     |          | .   .   |
|  /  \   |          |  . .   |
| /    \  |          | .  .   |
+----------+          +----------+

+------------------------------------------------------------------+
```

### ASCII - Resampling Operations

```
+------------------------------------------------------------------+
|           RESAMPLING OPERATIONS                                     |
+------------------------------------------------------------------+

DAILY (365) → MONTHLY (12)  [DOWNSAMPLING]
    +---+---+---+---+       +-------+
D1 | 1 |   |   |       |  sum  |
D2 | 2 |   |   |       |  of   |
D3 | 3 |   |   |  --> |  D1-3 |
... |...|   |   |       +-------+
    +---+---+---+---+       +-------+
D30|30 |   |   |       | etc  |

DAILY (365) → HOURLY (8760) [UPSAMPLING]
    +---+               +---+---+---+
D1 | 1 |         -->   | 1 | 1 | 1 | ...
    +---+               +---+---+---+
                          fill

+------------------------------------------------------------------+
```

### ASCII - Rolling Window

```
+------------------------------------------------------------------+
|         ROLLING WINDOW VISUALIZATION                               |
+------------------------------------------------------------------+

WINDOW SIZE: 7 DAYS

Day 1: ░░░░░░░░ [-----> no output]
Day 2: ░░░░░░░░ [-----> no output]
Day 3: ░░░░░░░░ [-----> no output]
Day 4: ░░░░░░░░ [-----> no output]
Day 5: ░░░░░░░░ [-----> no output]
Day 6: ░░░░░░░░ [-----> no output]
Day 7: ░████████  [--> output: avg(D1:D7)]
Day 8: ░████████  [--> output: avg(D2:D8)]
Day 9: ░████████  [--> output: avg(D3:D9)]

MIN_PERIODS: 1 (immediate output)

Window 7  Window 7  Window 7
[->out]   [->out]   [->out]

+------------------------------------------------------------------+
```

---

## Advanced Topics

### Datetime Indexing

```python
# Index slicing
df['2024']
df['2024-01']
df['2024-01-01':'2024-01-31']
df['2024':'2024-06']

# Using partial string indexing
df.loc['2024-01', 'value']
df.loc['2024', 'value']

# Using Between Time
df.between_time('09:00', '17:00')
```

### Time Zone Handling

```python
# Create with timezone
dates = pd.date_range('2024-01-01', periods=10, freq='D', tz='US/Eastern')

# Convert timezone
dates.tz_convert('UTC')

# Localize
pd.date_range('2024-01-01', periods=10, freq='D').tz_localize('US/Eastern')
```

### Period Index

```python
# Create Period index
periods = pd.period_range('2024-01', periods=12, freq='M')

# Create DataFrame with Period index
df = pd.DataFrame({
    'value': np.random.randn(12)
}, index=periods)

# Convert to timestamp
df.index.to_timestamp()
```

### Rolling with Time Window

```python
# Time-based rolling
df.rolling(window='7D').mean()

# Rolling with custom center
df.rolling(window='7D', center=True).mean()

# Min periods
df.rolling(window='7D', min_periods=3).mean()
```

### String Frequencies

```python
# Using offset aliases
'B' - Business day
'D' - Calendar day
'W' - Weekly
'M' - Month end
'Q' - Quarter end
'A' - Year end
'H' - Hour
'min' - Minute
```

---

## Conclusion

### Key Takeaways

This module covered time series analysis:

1. **DateTime Handling**: Creating and parsing dates
2. **Resampling**: Changing frequency
3. **Rolling Statistics**: Moving averages
4. **Shifting**: Lag operations
5. **Filtering**: Date-based selection

### Practical Applications

- Banking: Transaction trends, forecasting
- Healthcare: Patient admissions patterns
- Analytics: Temporal patterns

### Next Steps

Continue to: **Advanced Pandas Operations** for power techniques.

### References

- Pandas Time Series: https://pandas.pydata.org/docs/user_guide/timeseries.html
- Date offset aliases