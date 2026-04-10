# Mathematical Operations with NumPy

## Introduction

Mathematical operations form the backbone of numerical computing, and NumPy provides an extensive suite of mathematical functions that enable efficient computation on large arrays. These operations range from basic arithmetic to advanced mathematical functions like trigonometric, logarithmic, and exponential operations. Understanding these functions is essential for performing data analysis, scientific computing, and machine learning tasks.

NumPy's mathematical operations are designed to be vectorized, meaning they operate on entire arrays without requiring explicit loops. This vectorization approach provides significant performance benefits, especially when working with large datasets. The library leverages optimized C and Fortran code under the hood, making these operations orders of magnitude faster than equivalent Python loops.

The mathematical functions in NumPy can be categorized into several groups: basic arithmetic operations, trigonometric functions, logarithmic and exponential functions, special functions, and floating-point routines. Each category serves different purposes and is optimized for specific types of computations. For example, trigonometric functions are essential for signal processing and physics simulations, while logarithmic functions are crucial for financial calculations and information theory applications.

This module covers the fundamentals of mathematical operations with NumPy, including element-wise operations, array-wide operations, and broadcasting. You'll learn how to perform complex mathematical computations efficiently using NumPy's optimized functions, with practical examples from banking and healthcare domains.

## Fundamentals

### Basic Arithmetic Operations

NumPy provides comprehensive arithmetic operations that work element-wise on arrays. These operations are the foundation for more complex mathematical computations and are optimized for performance.

```python
import numpy as np

# Basic arithmetic operations on arrays
# Element-wise addition
a = np.array([10, 20, 30, 40, 50])
b = np.array([1, 2, 3, 4, 5])

print(f"Array A: {a}")
print(f"Array B: {b}")
print(f"\nAddition (A + B): {np.add(a, b)}")
print(f"Subtraction (A - B): {np.subtract(a, b)}")
print(f"Multiplication (A * B): {np.multiply(a, b)}")
print(f"Division (A / B): {np.divide(a, b)}")
print(f"Floor Division (A // B): {np.floor_divide(a, b)}")
print(f"Modulus (A % B): {np.mod(a, b)}")
print(f"Power (A ** B): {np.power(a, b)}")

# Output:
# Array A: [10 20 30 40 50]
# Array B: [1 2 3 4 5]
# 
# Addition (A + B): [11 22 33 44 55]
# Subtraction (A - B): [ 9 18 27 36 45]
# Multiplication (A * B): [ 10  40  90 160 250]
# Division (A / B): [10. 10. 10. 10. 10.]
# Floor Division (A // B): [10 10 10 10 10]
# Modulus (A % B): [0 0 0 0 0]
# Power (A ** B): [      10     400   27000 256000 312500000]
```

The arithmetic operations also support scalar operations, where a scalar value is applied to every element in the array. This is known as broadcasting and is one of NumPy's most powerful features.

```python
# Scalar operations (broadcasting)
prices = np.array([100, 200, 300, 400, 500])

print(f"Original prices: {prices}")
print(f"Add 10%: {prices * 1.10}")
print(f"Subtract $50: {prices - 50}")
print(f"Multiply by 2: {prices * 2}")
print(f"Divide by 4: {prices / 4}")

# Combined operations with proper parentheses
result = ((prices * 1.1) - 50) / 1.05
print(f"((prices * 1.1) - 50) / 1.05: {result}")

# Using NumPy functions for better precision
result = np.divide(np.subtract(np.multiply(prices, 1.10), 50), 1.05)
print(f"Using np.divide/subtract/multiply: {result}")

# Output:
# Original prices: [100 200 300 400 500]
# Add 10%: [110. 220. 330. 440. 550.]
# Subtract $50: [150 250 350 450 500]
# Multiply by 2: [200 400 600 800 1000]
# Divide by 4: [ 25.   50.   75.  100.  125.]
# ((prices * 1.1) - 50) / 1.05: [ 57.14 190.48 323.81 457.14 590.48]
# Using np.divide/subtract/multiply: [ 57.14 190.48 323.81 457.14 590.48]
```

### Trigonometric Functions

NumPy provides comprehensive trigonometric functions that operate element-wise on arrays. These functions are essential for signal processing, physics simulations, and various mathematical applications.

```python
# Trigonometric functions
angles = np.array([0, np.pi/6, np.pi/4, np.pi/3, np.pi/2, np.pi])

print("Angle (radians):", angles)
print("sin():", np.sin(angles))
print("cos():", np.cos(angles))
print("tan():", np.tan(angles))

# Converting between degrees and radians
degrees = np.array([0, 30, 45, 60, 90, 180, 270, 360])
radians = np.deg2rad(degrees)
print(f"\nDegrees: {degrees}")
print(f"Radians: {radians}")
print(f"sin(degrees): {np.sin(radians)}")

# Inverse trigonometric functions
values = np.array([-1, -0.5, 0, 0.5, 1])
print(f"\nValues: {values}")
print(f"arcsin(): {np.arcsin(values)}")
print(f"arccos(): {np.arccos(values)}")
print(f"arctan(): {np.arctan(values)}")

# Hyperbolic functions
x = np.array([0, 1, 2, 3])
print(f"\nx: {x}")
print(f"sinh(): {np.sinh(x)}")
print(f"cosh(): {np.cosh(x)}")
print(f"tanh(): {np.tanh(x)}")

# Output:
# Angle (radians): [0.         0.52359878 0.78539816 1.04719755 1.57079633 3.14159265]
# sin(): [0.0000000e+00 5.0000000e-01 7.0710678e-01 8.6602540e-01 1.0000000e+00 1.2246468e-16]
# cos(): [1.0000000e+00 8.6602540e-01 7.0710678e-01 5.0000000e-01 6.1232340e-17 -1.0000000e+00]
# tan(): [0.0000000e+00 5.7735027e-01 1.0000000e+00 1.7320508e+00 1.6331239e+32 -1.2246468e-16]
```

### Logarithmic and Exponential Functions

Logarithmic and exponential functions are crucial for many scientific and financial calculations. NumPy provides both natural and base-specific logarithmic functions.

```python
# Logarithmic and exponential functions
x = np.array([0.1, 0.5, 1, 2, 5, 10])

print(f"x: {x}")
print(f"exp(x): {np.exp(x)}")
print(f"exp2(x): {np.exp2(x)}")
print(f"expm1(x): {np.expm1(x)}")  # exp(x) - 1, more accurate for small x

print(f"\nlog(x): {np.log(x)}")
print(f"log2(x): {np.log2(x)}")
print(f"log10(x): {np.log10(x)}")
print(f"log1p(x): {np.log1p(x)}")  # log(1 + x), more accurate for small x

# Logarithm with specific base
base = 3
print(f"\nlog(x) base {base}: {np.log(x) / np.log(base)}")
print(f"log(base={base}): {np.log(x) / np.log(base)}")

# Demonstration of numerical stability
small_x = np.array([1e-10, 1e-9, 1e-8])
print(f"\nSmall x values: {small_x}")
print(f"log(1 + x) accurate: {np.log1p(small_x)}")
print(f"log(1 + x) inaccurate: {np.log(small_x + 1)}")

# Output:
# x: [ 0.1  0.5  1.   2.   5.  10. ]
# exp(x): [  1.64872    3.48871    7.38906   20.08554  148.41316 22026.46579]
# exp2(x): [ 1.07177    2.82843    4.         8.        32.       1024.    ]
# expm1(x): [ 0.64872    3.48871    6.38906   19.08554 147.41316 22025.46579]
# 
# log(x): [-2.302585  -0.693147   0.           0.693147   1.609438  2.302585]
# log2(x): [-3.321928 -1.           0.           1.         2.321928  3.321928]
# log10(x): [-1.         -0.30103    0.           0.30103    0.69897   1.      ]
# log1p(x): [-2.278854 -0.693147   0.           0.693147   1.609438 2.302585]
```

### Special Functions

NumPy includes special mathematical functions that are used in various scientific computations. These include functions for handling small numerical differences and special mathematical constants.

```python
# Special mathematical functions
# Handling numerical precision with fudge factor
arr = np.array([1e-15, 1e-10, 1e-5, 1e-3])

# Using finfo for floating point information
print(f"Machine epsilon: {np.finfo(float).eps}")
print(f"Smallest normal: {np.finfo(float).tiny}")
print(f"Maximum value: {np.finfo(float).max}")

# Comparing values with tolerance
a = np.array([1.0, 2.0, 3.0])
b = np.array([1.0 + 1e-10, 2.0, 3.0 - 1e-10])

print(f"\na: {a}")
print(f"b: {b}")
print(f"a == b (exact): {np.equal(a, b)}")
print(f"np.allclose(a, b): {np.allclose(a, b)}")
print(f"np.isclose(a, b, rtol=1e-5): {np.isclose(a, b, rtol=1e-5)}")

#isfinite, isinf, isnan checks
values = np.array([1.0, np.inf, -np.inf, np.nan, 0.0])
print(f"\nValues: {values}")
print(f"isfinite: {np.isfinite(values)}")
print(f"isinf: {np.isinf(values)}")
print(f"isnan: {np.isnan(values)}")

# sign and absolute value
neg_pos = np.array([-5, -3, 0, 3, 5])
print(f"\nArray: {neg_pos}")
print(f"abs(): {np.abs(neg_pos)}")
print(f"sign(): {np.sign(neg_pos)}")

# Output:
# Machine epsilon: 2.220446049250313e-16
# Smallest normal: 2.2250738585072014e-308
# Maximum value: 1.7976931348623157e+308
# 
# a: [1. 2. 3.]
# b: [1. 2. 3.]
# a == b (exact): [ True  True  True]
# np.allclose(a, b): True
# np.isclose(a, b, rtol=1e-5): [ True  True  True]
# 
# Values: [ 1.  inf -inf  nan  0.]
# isfinite: [ True False False False  True]
# isinf: [False  True  True False False]
# isnan: [False False False  True False]
# 
# Array: [-5 -3  0  3  5]
# abs(): [5 3 0 3 5]
# sign(): [-1 -1  0  1  1]
```

## Implementation

### Banking Application: Compound Interest Calculations

Compound interest calculations are fundamental to banking and finance. NumPy's mathematical functions enable efficient computation of various interest-related metrics.

```python
import numpy as np

# Compound interest calculations
# Calculate future value with compound interest
# FV = PV * (1 + r/n)^(n*t)
# Where PV = present value, r = annual rate, n = compounding frequency, t = time in years

principal = 10000  # Initial investment
annual_rate = 0.05  # 5% annual interest
years = 10
compounding_frequency = 12  # Monthly compounding

# Calculate future value
r = annual_rate / compounding_frequency
n = compounding_frequency * years
future_value = principal * (1 + r) ** n

print("=" * 60)
print("COMPOUND INTEREST CALCULATOR")
print("=" * 60)
print(f"Principal: ${principal:,.2f}")
print(f"Annual Rate: {annual_rate*100:.1f}%")
print(f"Years: {years}")
print(f"Compounding: Monthly")
print(f"Future Value: ${future_value:,.2f}")

# Calculate interest earned
interest_earned = future_value - principal
print(f"Interest Earned: ${interest_earned:,.2f}")

# Calculate effective annual rate
effective_rate = (future_value / principal) ** (1/years) - 1
print(f"Effective Annual Rate: {effective_rate*100:.2f}%")

# Calculate for different compounding frequencies
frequencies = [1, 4, 12, 365]  # Annual, Quarterly, Monthly, Daily
print(f"\n{'Compounding':<15} {'Future Value':<20} {'Effective Rate':<15}")
print("-" * 55)

for freq in frequencies:
    fv = principal * (1 + annual_rate/freq) ** (freq * years)
    eff_rate = (fv / principal) ** (1/years) - 1
    freq_name = {1: "Annual", 4: "Quarterly", 12: "Monthly", 365: "Daily"}[freq]
    print(f"{freq_name:<15} ${fv:>15,.2f} {eff_rate*100:>12.2f}%")

# Using logarithmic functions for time calculations
# Calculate time to double investment
target_ratio = 2  # Double the investment
time_to_double = np.log(target_ratio) / np.log(1 + annual_rate)
print(f"\nTime to double investment: {time_to_double:.1f} years")
```

```python
# Loan amortization calculations using NumPy
# Calculate monthly payment for a loan
# M = P * [r(1+r)^n] / [(1+r)^n - 1]
# Where P = principal, r = monthly rate, n = total payments

principal = 250000  # Loan amount
annual_rate = 0.065  # 6.5% annual rate
years = 30
n_payments = years * 12  # Total monthly payments

monthly_rate = annual_rate / 12

# Calculate monthly payment
monthly_payment = principal * (monthly_rate * (1 + monthly_rate) ** n_payments) / ((1 + monthly_rate) ** n_payments - 1)

print("=" * 60)
print("LOAN AMORTIZATION SCHEDULE")
print("=" * 60)
print(f"Loan Amount: ${principal:,.2f}")
print(f"Annual Rate: {annual_rate*100:.2f}%")
print(f"Term: {years} years")
print(f"Monthly Payment: ${monthly_payment:,.2f}")

# Generate amortization schedule
balance = principal
schedule = []

for month in range(1, n_payments + 1):
    interest_payment = balance * monthly_rate
    principal_payment = monthly_payment - interest_payment
    balance -= principal_payment
    
    if month % 36 == 0:  # Every 3 years
        schedule.append({
            'month': month,
            'balance': balance,
            'principal_paid': principal_payment,
            'interest_paid': interest_payment
        })

print(f"\n{'Month':<10} {'Balance':<15} {'Principal':<15} {'Interest':<12}")
print("-" * 55)
for row in schedule:
    print(f"{row['month']:<10} ${row['balance']:>12,.2f} ${row['principal_paid']:>12,.2f} ${row['interest_paid']:>10,.2f}")

# Total interest paid
total_interest = (monthly_payment * n_payments) - principal
print(f"\nTotal Interest Paid: ${total_interest:,.2f}")
```

### Healthcare Application: Medical Dosage Calculations

Mathematical operations are essential in healthcare for medication dosage calculations, drug concentration analysis, and medical imaging processing.

```python
# Drug concentration calculations
# Calculate drug concentration over time using exponential decay
# C = C0 * e^(-kt)
# Where C0 = initial concentration, k = elimination constant, t = time

initial_concentration = 100  # Initial concentration (mg/L)
elimination_constant = 0.15  # Elimination rate constant (per hour)

# Calculate concentration over 24 hours
time_hours = np.arange(0, 25, 1)  # 0 to 24 hours
concentration = initial_concentration * np.exp(-elimination_constant * time_hours)

print("=" * 60)
print("DRUG CONCENTRATION OVER TIME")
print("=" * 60)
print(f"Initial Concentration: {initial_concentration} mg/L")
print(f"Elimination Rate: {elimination_constant} per hour")
print(f"\n{'Hour':<10} {'Concentration (mg/L)':<20}")
print("-" * 35)

for t, c in zip(time_hours[::4], concentration[::4]):
    bar = '█' * int(c / 5)
    print(f"{t:<10} {c:>10.2f} {bar}")

# Calculate half-life
# t(1/2) = ln(2) / k
half_life = np.log(2) / elimination_constant
print(f"\nDrug Half-life: {half_life:.2f} hours")

# Calculate time to reach therapeutic window
therapeutic_min = 20  # Minimum therapeutic concentration
therapeutic_max = 60  # Maximum therapeutic concentration

time_to_therapeutic = -np.log(therapeutic_max / initial_concentration) / elimination_constant
time_to_sub_therapeutic = -np.log(therapeutic_min / initial_concentration) / elimination_constant

print(f"Time to reach therapeutic level: {time_to_therapeutic:.2f} hours")
print(f"Time until sub-therapeutic: {time_to_sub_therapeutic:.2f} hours")
```

```python
# IV drip rate calculations
# Calculate drip rate for intravenous therapy
# Drip Rate (mL/hr) = (Volume to infuse * Drop factor) / Time

patient_weight_kg = 70  # Patient weight
dosage_required = 5  # mcg/kg/min
concentration = 4  # mg/mL (drug concentration)
drop_factor = 60  # drops/mL (IV set specification)

# Convert dosage to mL/hour
# Rate = (dosage * weight * 60) / (concentration * 1000)
# where dosage is in mcg/kg/min, concentration in mg/mL
rate_ml_per_hour = (dosage_required * patient_weight_kg * 60) / (concentration * 1000)

print("=" * 60)
print("IV DRIP RATE CALCULATOR")
print("=" * 60)
print(f"Patient Weight: {patient_weight_kg} kg")
print(f"Required Dosage: {dosage_required} mcg/kg/min")
print(f"Drug Concentration: {concentration} mg/mL")
print(f"IV Set Drop Factor: {drop_factor} drops/mL")
print(f"\nDrip Rate: {rate_ml_per_hour:.2f} mL/hour")
print(f"Drop Rate: {rate_ml_per_hour * drop_factor:.0f} drops/minute")

# Calculate for different concentrations
concentrations = np.array([1, 2, 4, 5, 10])  # mg/mL
rates = (dosage_required * patient_weight_kg * 60) / (concentrations * 1000)

print(f"\n{'Concentration':<15} {'Rate (mL/hr)':<15} {'Drop Rate':<15}")
print("-" * 50)
for c, r in zip(concentrations, rates):
    drop_rate = r * drop_factor
    print(f"{c} mg/mL{'':<8} {r:>12.2f} {drop_rate:>12.0f} drops/min")
```

### Body Mass Index and Medical Metrics

NumPy mathematical functions are used to calculate various medical metrics including BMI, body surface area, and other clinical measurements.

```python
# BMI calculation and classification
patients = np.array([
    [170, 70],   # Height (cm), Weight (kg)
    [165, 85],
    [180, 90],
    [155, 45],
    [175, 100],
    [160, 55],
    [185, 110]
])

heights_cm = patients[:, 0]
weights_kg = patients[:, 1]

# Convert height to meters
heights_m = heights_cm / 100

# Calculate BMI = weight / height^2
bmi = weights_kg / (heights_m ** 2)

print("=" * 60)
print("BMI CALCULATION AND CLASSIFICATION")
print("=" * 60)
print(f"{'Height(cm)':<12} {'Weight(kg)':<12} {'BMI':<10} {'Category':<15}")
print("-" * 55)

for h, w, b in zip(heights_cm, weights_kg, bmi):
    if b < 18.5:
        category = "Underweight"
    elif b < 25:
        category = "Normal"
    elif b < 30:
        category = "Overweight"
    else:
        category = "Obese"
    print(f"{h:<12} {w:<12} {b:<10.1f} {category:<15}")

# Statistical summary
print(f"\nBMI Statistics:")
print(f"  Mean: {np.mean(bmi):.1f}")
print(f"  Median: {np.median(bmi):.1f}")
print(f"  Std Dev: {np.std(bmi):.1f}")
print(f"  Min: {np.min(bmi):.1f}")
print(f"  Max: {np.max(bmi):.1f}")

# Calculate ideal body weight using Devine formula
# Male: IBW = 50 + 2.3 * (height in inches - 60)
# Female: IBW = 45.5 + 2.3 * (height in inches - 60)
height_inches = heights_cm / 2.54
ibw_male = 50 + 2.3 * (height_inches - 60)
ibw_female = 45.5 + 2.3 * (height_inches - 60)

print(f"\nIdeal Body Weight (Male formula):")
for h, ibw in zip(heights_cm, ibw_male):
    print(f"  Height {h} cm: {ibw:.1f} kg")
```

## Applications

### Advanced Mathematical Operations

NumPy provides advanced mathematical operations that enable complex scientific computations. These include accumulation operations, polynomial operations, and rounding functions.

```python
import numpy as np

# Accumulation operations - running calculations
# cumsum, cumprod, cumulative operations
data = np.array([10, 20, 30, 40, 50])

print("Data:", data)
print(f"cumsum (running sum): {np.cumsum(data)}")
print(f"cumprod (running product): {np.cumprod(data)}")
print(f"diff (differences): {np.diff(data)}")

# Using diff for calculating rates of change
prices = np.array([100, 105, 103, 110, 115, 112, 120])
price_changes = np.diff(prices)
price_percent_change = (price_changes / prices[:-1]) * 100

print(f"\n{'Day':<8} {'Price':<10} {'Change':<12} {'% Change':<12}")
print("-" * 45)
for i in range(len(prices)):
    if i == 0:
        print(f"{i:<8} {prices[i]:<10} {'N/A':<12} {'N/A':<12}")
    else:
        pct = price_percent_change[i-1]
        print(f"{i:<8} {prices[i]:<10} {price_changes[i-1]:<+12} {pct:>10.1f}%")

# Polynomial operations
# Evaluate polynomial using np.polyval
coefficients = [1, -2, 1]  # x^2 - 2x + 1
x_values = np.array([0, 1, 2, 3, 4, 5])

y_values = np.polyval(coefficients, x_values)
print(f"\nPolynomial: x^2 - 2x + 1")
print(f"x: {x_values}")
print(f"y: {y_values}")

# Polynomial roots
roots = np.roots(coefficients)
print(f"Roots: {roots}")

# Derivative of polynomial
derivative = np.polyder(coefficients)
print(f"Derivative coefficients: {derivative}")
print(f"Derivative at x=2: {np.polyval(derivative, 2)}")
```

### Banking Application: Risk Metrics

Mathematical operations are fundamental to calculating financial risk metrics like Value at Risk (VaR) and other risk measures.

```python
# Value at Risk (VaR) calculation
np.random.seed(42)

# Simulate daily returns for a portfolio
n_days = 1000
mean_return = 0.0005  # Average daily return
std_dev = 0.02  # Daily standard deviation

daily_returns = np.random.normal(mean_return, std_dev, n_days)

# Calculate VaR at different confidence levels
var_95 = np.percentile(daily_returns, 5)
var_99 = np.percentile(daily_returns, 1)
var_999 = np.percentile(daily_returns, 0.1)

print("=" * 60)
print("VALUE AT RISK (VaR) CALCULATION")
print("=" * 60)
print(f"Portfolio Value: $1,000,000")
print(f"Mean Daily Return: {mean_return*100:.2f}%")
print(f"Daily Volatility: {std_dev*100:.2f}%")
print(f"\nConfidence Level | VaR (Daily) | VaR ($1M)")
print("-" * 45)
print(f"95%            | {var_95*100:>8.2f}%   | ${abs(var_95 * 1000000):>10,.0f}")
print(f"99%            | {var_99*100:>8.2f}%   | ${abs(var_99 * 1000000):>10,.0f}")
print(f"99.9%          | {var_999*100:>8.2f}%   | ${abs(var_999 * 1000000):>10,.0f}")

# Calculate expected shortfall (CVaR)
returns_sorted = np.sort(daily_returns)
cvar_95 = np.mean(returns_sorted[:int(n_days * 0.05)])
cvar_99 = np.mean(returns_sorted[:int(n_days * 0.01)])

print(f"\nConditional VaR (Expected Shortfall):")
print(f"95% CVaR: {cvar_95*100:.2f}%")
print(f"99% CVaR: {cvar_99*100:.2f}%")

# Maximum Drawdown calculation
portfolio_values = 1000000 * np.cumprod(1 + daily_returns)
rolling_max = np.maximum.accumulate(portfolio_values)
drawdowns = (portfolio_values - rolling_max) / rolling_max

max_drawdown = np.min(drawdowns)
max_drawdown_date = np.argmin(drawdowns)

print(f"\nMaximum Drawdown: {max_drawdown*100:.2f}%")
```

```python
# Sharpe Ratio and Performance Metrics
risk_free_rate = 0.02 / 252  # Daily risk-free rate
excess_returns = daily_returns - risk_free_rate

sharpe_ratio = np.mean(excess_returns) / np.std(excess_returns) * np.sqrt(252)

print("=" * 60)
print("RISK-ADJUSTED PERFORMANCE METRICS")
print("=" * 60)
print(f"Sharpe Ratio (annualized): {sharpe_ratio:.2f}")

# Sortino Ratio (downside deviation)
negative_returns = excess_returns[excess_returns < 0]
downside_deviation = np.std(negative_returns) * np.sqrt(252)
sortino_ratio = np.mean(excess_returns) * 252 / downside_deviation

print(f"Sortino Ratio (annualized): {sortino_ratio:.2f}")

# Calmar Ratio (return / max drawdown)
annual_return = np.mean(daily_returns) * 252
calmar_ratio = annual_return / abs(max_drawdown)

print(f"Calmar Ratio: {calmar_ratio:.2f}")
print(f"Annual Return: {annual_return*100:.2f}%")
print(f"Annual Volatility: {np.std(daily_returns) * np.sqrt(252)*100:.2f}%")
```

### Healthcare Application: Statistical Analysis

Mathematical operations enable statistical analysis of medical data, including hypothesis testing and confidence intervals.

```python
# Statistical analysis of treatment outcomes
# Comparing two treatment groups
np.random.seed(42)

# Treatment group A (new treatment)
group_a = np.random.normal(85, 10, 100)
# Treatment group B (control)
group_b = np.random.normal(78, 12, 100)

print("=" * 60)
print("TREATMENT OUTCOME ANALYSIS")
print("=" * 60)

print(f"\nGroup A (New Treatment):")
print(f"  N: {len(group_a)}")
print(f"  Mean: {np.mean(group_a):.2f}")
print(f"  Std Dev: {np.std(group_a, ddof=1):.2f}")
print(f"  Median: {np.median(group_a):.2f}")

print(f"\nGroup B (Control):")
print(f"  N: {len(group_b)}")
print(f"  Mean: {np.mean(group_b):.2f}")
print(f"  Std Dev: {np.std(group_b, ddof=1):.2f}")
print(f"  Median: {np.median(group_b):.2f}")

# Calculate difference in means
mean_diff = np.mean(group_a) - np.mean(group_b)
print(f"\nMean Difference: {mean_diff:.2f}")

# Calculate standard error
 pooled_n = len(group_a) + len(group_b)
pooled_var = ((len(group_a)-1)*np.var(group_a, ddof=1) + 
              (len(group_b)-1)*np.var(group_b, ddof=1)) / (pooled_n - 2)
se = np.sqrt(pooled_var * (1/len(group_a) + 1/len(group_b)))

# T-statistic
t_stat = mean_diff / se
print(f"T-statistic: {t_stat:.2f}")

# Degrees of freedom
df = pooled_n - 2
print(f"Degrees of Freedom: {df}")

# Calculate confidence interval (95%)
confidence_level = 0.95
t_critical = 2.0  # Approximation for large n
ci_lower = mean_diff - t_critical * se
ci_upper = mean_diff + t_critical * se

print(f"\n95% Confidence Interval: ({ci_lower:.2f}, {ci_upper:.2f})")
```

## Output Results

### Formatted Output with Mathematical Operations

NumPy provides various ways to format and display results from mathematical operations.

```python
import numpy as np

# Formatted output for financial calculations
accounts = np.array([
    [1001, 50000.00, 0.035, 12],
    [1002, 75000.00, 0.042, 6],
    [1003, 25000.00, 0.028, 24],
    [1004, 100000.00, 0.050, 3],
    [1005, 5000.00, 0.015, 48]
])

account_id = accounts[:, 0]
balance = accounts[:, 1]
rate = accounts[:, 2]
monthly_deposit = accounts[:, 3]

# Calculate annual interest
annual_interest = balance * rate

# Calculate future value over different periods
years = np.array([1, 5, 10, 20])
future_values = np.zeros((len(accounts), len(years)))

for i, (bal, r) in enumerate(zip(balance, rate)):
    for j, y in enumerate(years):
        fv = bal * (1 + r) ** y
        future_values[i, j] = fv

print("=" * 70)
print("ACCOUNT GROWTH PROJECTIONS")
print("=" * 70)

for i in range(len(accounts)):
    print(f"\nAccount {account_id[i]}: Initial Balance ${balance[i]:,.2f}")
    print(f"  Rate: {rate[i]*100:.2f}%")
    print(f"  Annual Interest: ${annual_interest[i]:,.2f}")
    print(f"\n  {'':>12}", end="")
    for y in years:
        print(f"Year {y:>6}", end="")
    print(f"\n  {'Future Value':<12}", end="")
    for y in range(len(years)):
        print(f"${future_values[i, y]:>12,.0f}", end="")
    print()

# Summary statistics
total_balance = np.sum(balance)
total_interest = np.sum(annual_interest)
avg_rate = np.average(rate, weights=balance)

print(f"\n{'='*70}")
print(f"Portfolio Summary:")
print(f"  Total Balance: ${total_balance:,.2f}")
print(f"  Total Annual Interest: ${total_interest:,.2f}")
print(f"  Weighted Average Rate: {avg_rate*100:.2f}%")
```

### Medical Metrics Output

```python
# Medical metrics formatted output
# Patient treatment response data
np.random.seed(42)

patient_ids = np.arange(1001, 1011)
baseline_scores = np.random.normal(50, 10, 10)
final_scores = baseline_scores + np.random.normal(15, 8, 10)
change_scores = final_scores - baseline_scores
percent_change = (change_scores / baseline_scores) * 100

print("=" * 70)
print("PATIENT TREATMENT RESPONSE REPORT")
print("=" * 70)

print(f"\n{'Patient ID':<12} {'Baseline':<12} {'Final':<12} {'Change':<12} {'% Change':<12}")
print("-" * 65)

for id, base, final, change, pct in zip(patient_ids, baseline_scores, 
                                        final_scores, change_scores, percent_change):
    print(f"{id:<12} {base:<12.1f} {final:<12.1f} {change:<+12.1f} {pct:<+12.1f}%")

# Summary statistics
print(f"\n{'='*70}")
print("Summary Statistics:")
print(f"{'':>20} {'Mean':<12} {'Std Dev':<12} {'Min':<12} {'Max':<12}")
print("-" * 70)
print(f"{'Baseline Score':<20} {np.mean(baseline_scores):<12.1f} {np.std(baseline_scores):<12.1f} {np.min(baseline_scores):<12.1f} {np.max(baseline_scores):<12.1f}")
print(f"{'Final Score':<20} {np.mean(final_scores):<12.1f} {np.std(final_scores):<12.1f} {np.min(final_scores):<12.1f} {np.max(final_scores):<12.1f}")
print(f"{'Change':<20} {np.mean(change_scores):<12.1f} {np.std(change_scores):<12.1f} {np.min(change_scores):<12.1f} {np.max(change_scores):<12.1f}")
print(f"{'% Change':<20} {np.mean(percent_change):<12.1f}% {np.std(percent_change):<12.1f}% {np.min(percent_change):<12.1f}% {np.max(percent_change):<12.1f}%")

# Responder analysis (patients with improvement > 20%)
responders = percent_change > 20
response_rate = np.sum(responders) / len(responders) * 100

print(f"\nResponse Rate (>20% improvement): {response_rate:.1f}%")
```

## Visualization

### ASCII Mathematical Visualizations

Creating visualizations of mathematical functions helps understand their behavior and relationships.

```python
import numpy as np

# Visualize exponential growth and decay
print("=" * 60)
print("EXPONENTIAL GROWTH AND DECAY")
print("=" * 60)

time = np.linspace(0, 5, 30)

# Exponential growth
growth = np.exp(time)
# Exponential decay
decay = np.exp(-time)

max_growth = np.max(growth)
max_decay = np.max(decay)

print("\nExponential Growth: exp(t)")
print("-" * 40)
for t, g in zip(time[::3], growth[::3]):
    bar_length = int((g / max_growth) * 30)
    bar = '█' * bar_length
    print(f"t={t:4.1f} | {bar}")

print("\nExponential Decay: exp(-t)")
print("-" * 40)
for t, d in zip(time[::3], decay[::3]):
    bar_length = int((d / max_decay) * 30)
    bar = '▓' * bar_length
    print(f"t={t:4.1f} | {bar}")

# Compare growth rates
print("\n" + "=" * 60)
print("GROWTH RATE COMPARISON")
print("=" * 60)

rates = [0.5, 1.0, 1.5, 2.0]
t_range = np.linspace(0, 3, 25)

for rate in rates:
    values = np.exp(rate * t_range)
    max_val = np.max(values)
    print(f"\nRate = {rate}:")
    for t, v in zip(t_range[::5], values[::5]):
        bar_length = int((v / max_val) * 30)
        bar = '█' * bar_length
        print(f"  t={t:4.1f} | {bar}")
```

### ASCII Function Plots

```python
# Plot trigonometric functions
print("=" * 60)
print("TRIGONOMETRIC FUNCTIONS")
print("=" * 60)

angles = np.linspace(0, 2*np.pi, 40)

# Sine wave
sin_values = np.sin(angles)
# Cosine wave
cos_values = np.cos(angles)

print("\nSine Wave (0 to 2π):")
print("-" * 40)
max_s = np.max(sin_values)
min_s = np.min(sin_values)

for i, (a, s) in enumerate(zip(angles, sin_values)):
    if i % 4 == 0:
        bar_pos = int((s - min_s) / (max_s - min_s) * 20)
        bar_neg = int((0 - min_s) / (max_s - min_s) * 20)
        
        if s >= 0:
            center = " " * bar_neg + "|" + "█" * bar_pos
        else:
            center = " " * bar_pos + "|" + "▓" * bar_neg
        print(f"{a/np.pi:4.1f}π | {center}")

print("\nCosine Wave (0 to 2π):")
print("-" * 40)
max_c = np.max(cos_values)
min_c = np.min(cos_values)

for i, (a, c) in enumerate(zip(angles, cos_values)):
    if i % 4 == 0:
        bar_pos = int((c - min_c) / (max_c - min_c) * 20)
        bar_neg = int((0 - min_c) / (max_c - min_c) * 20)
        
        if c >= 0:
            center = " " * bar_neg + "|" + "█" * bar_pos
        else:
            center = " " * bar_pos + "|" + "▓" * bar_neg
        print(f"{a/np.pi:4.1f}π | {center}")
```

### Medical Data Visualizations

```python
# Patient response distribution
print("=" * 60)
print("PATIENT RESPONSE DISTRIBUTION")
print("=" * 60)

np.random.seed(42)
responses = np.random.normal(25, 10, 100)
responses = np.clip(responses, 0, 60)

# Create histogram bins
hist, edges = np.histogram(responses, bins=10)
max_count = np.max(hist)

print("\nResponse Improvement Distribution:")
print("-" * 40)

for i, (count, edge) in enumerate(zip(hist, edges)):
    bar_length = int((count / max_count) * 30)
    bar = '█' * bar_length
    print(f"{edge:5.1f}-{edges[i+1]:5.1f}: {bar} ({count})")

print(f"Mean improvement: {np.mean(responses):.1f}%")
print(f"Median improvement: {np.median(responses):.1f}%")
print(f"Std deviation: {np.std(responses):.1f}%")

# Box plot representation
print("\nBox Plot Representation:")
print("-" * 40)

q1 = np.percentile(responses, 25)
q2 = np.percentile(responses, 50)
q3 = np.percentile(responses, 75)
min_val = np.min(responses)
max_val = np.max(responses)

print(f"Min: {min_val:.1f}")
print(f"Q1: {q1:.1f}")
print(f"Q2 (Median): {q2:.1f}")
print(f"Q3: {q3:.1f}")
print(f"Max: {max_val:.1f}")
```

## Advanced Topics

### Numerical Stability and Precision

Understanding numerical stability is crucial for accurate mathematical computations, especially when working with very small or large numbers.

```python
import numpy as np

# Demonstrate numerical precision issues
print("=" * 60)
print("NUMERICAL PRECISION DEMONSTRATION")
print("=" * 60)

# Large number addition issues
large = 1e15
small = 1e-10

result = large + small
print(f"Large + Small: {1e15} + {1e-10}")
print(f"Result: {result}")
print(f"Expected: {1e15 + 1e-10}")
print(f"Lost precision: {1e15 + 1e-10 - result}")

# Catastrophic cancellation
a = 1.0000001
b = 1.0000000

print(f"\nCatastrophic Cancellation:")
print(f"a = {a}, b = {b}")
print(f"a - b = {a - b}")
print(f"ln(a) - ln(b) = {np.log(a) - np.log(b)}")

# Using np.log1p for small values
x = 1e-10
print(f"\nFor small x = {x}:")
print(f"log(1 + x) = {np.log(1 + x)}")
print(f"log1p(x) = {np.log1p(x)}")

# Summation precision
values = np.array([1e10, 1, -1e10])
print(f"\nSummation precision:")
print(f"Values: {values}")
print(f"np.sum: {np.sum(values)}")
print(f"Expected: 0")

# Kahan summation for better precision
def kahan_sum(arr):
    """Kahan summation for improved precision"""
    sum_val = 0.0
    c = 0.0  # Compensation
    for val in arr:
        y = val - c
        t = sum_val + y
        c = (t - sum_val) - y
        sum_val = t
    return sum_val

print(f"Kahan sum: {kahan_sum(values)}")

# Floating point comparison
print(f"\nFloating Point Comparison:")
x = 0.1 + 0.1 + 0.1
y = 0.3

print(f"x = 0.1 + 0.1 + 0.1 = {x}")
print(f"y = 0.3 = {y}")
print(f"x == y: {x == y}")
print(f"np.isclose(x, y): {np.isclose(x, y)}")
print(f"np.allclose([0.1*3], [0.3]): {np.allclose([0.1*3], [0.3])}")
```

### Performance Considerations

The choice of mathematical operations can significantly impact performance, especially with large arrays.

```python
# Performance comparison of different mathematical approaches
import time

np.random.seed(42)
size = 1000000

# Generate test data
arr = np.random.rand(size)

# Method 1: Using Python loops (slow)
start = time.time()
result_loop = []
for val in arr:
    result_loop.append(val ** 2)
loop_time = time.time() - start

# Method 2: Using list comprehension (faster than loop but still Python)
start = time.time()
result_comp = [val ** 2 for val in arr]
comp_time = time.time() - start

# Method 3: Using NumPy vectorized operations (fastest)
start = time.time()
result_np = arr ** 2
np_time = time.time() - start

# Method 4: Using np.power (similar to **)
start = time.time()
result_power = np.power(arr, 2)
power_time = time.time() - start

print("=" * 60)
print("PERFORMANCE COMPARISON")
print("=" * 60)
print(f"Array size: {size:,} elements")
print(f"\nMethod                   | Time (sec) | Relative Speed")
print("-" * 55)
print(f"Python loop            | {loop_time:>8.4f} | {loop_time/loop_time:>14.2f}x")
print(f"List comprehension    | {comp_time:>8.4f} | {comp_time/loop_time:>14.2f}x")
print(f"NumPy ** operator    | {np_time:>8.4f} | {np_time/loop_time:>14.2f}x")
print(f"np.power()           | {power_time:>8.4f} | {power_time/loop_time:>14.2f}x")

# Verify results are the same
print(f"\nResults verification:")
print(f"np.allclose: {np.allclose(np.array(result_loop), result_np)}")

# In-place vs copy operations
# In-place operations save memory
arr = np.random.rand(1000000)

# Method 1: Create new array
start = time.time()
result_new = arr + 1
new_time = time.time() - start

# Method 2: Use out parameter (in-place)
start = time.time()
result_out = np.empty_like(arr)
np.add(arr, 1, out=result_out)
out_time = time.time() - start

print(f"\nMemory optimization:")
print(f"New array: {new_time*1000:.3f} ms")
print(f"In-place (out=): {out_time*1000:.3f} ms")
```

### Advanced Mathematical Functions

NumPy provides additional mathematical functions for specialized computations.

```python
# Clip and wrap functions
arr = np.array([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])

print("=" * 60)
print("CLIP AND WRAP FUNCTIONS")
print("=" * 60)

# Clip values to a range
clipped = np.clip(arr, 2, 8)
print(f"Original: {arr}")
print(f"Clipped (2, 8): {clipped}")

# Wrap values around a range
wrapped = np.wrap(arr, 5)
print(f"Wrapped modulo 5: {wrapped}")

# Square root and cbrt (cube root)
values = np.array([-4, -1, 0, 1, 4, 8, 27])

print(f"\nValues: {values}")
print(f"np.sqrt: {np.sqrt(values)}")  # Returns NaN for negative
print(f"np.cbrt: {np.cbrt(values)}")  # Handles negatives

# Sign and absolute value combinations
neg_vals = np.array([-3, -2, -1, 0, 1, 2, 3])

print(f"\nNegative values: {neg_vals}")
print(f"np.abs: {np.abs(neg_vals)}")
print(f"np.sign: {np.sign(neg_vals)}")
print(f"np.negative: {np.negative(neg_vals)}")
print(f"np.positive: {np.positive(neg_vals)}")

# Maximum and minimum with multiple arguments
a = np.array([1, 5, 3])
b = np.array([3, 2, 6])

print(f"\na: {a}")
print(f"b: {b}")
print(f"np.maximum(a, b): {np.maximum(a, b)}")
print(f"np.minimum(a, b): {np.minimum(a, b)}")
print(f"np.fmax(a, b): {np.fmax(a, b)}")  # Treats NaN as missing
print(f"np.fmin(a, b): {np.fmin(a, b)}")
```

## Conclusion

Mathematical operations with NumPy provide the foundation for numerical computing in Python. Through this comprehensive exploration, you've learned about basic arithmetic operations, trigonometric functions, logarithmic and exponential functions, and special mathematical functions. These operations are essential for performing complex calculations efficiently in both banking and healthcare applications.

The banking examples demonstrated compound interest calculations, loan amortization, risk metrics like Value at Risk, and performance ratios. The healthcare applications showed drug concentration calculations, IV drip rate calculations, BMI calculations, and statistical analysis of treatment outcomes. Both domains benefit significantly from NumPy's vectorized mathematical operations.

Key takeaways include the importance of vectorization for performance, understanding numerical stability for accuracy, and using appropriate mathematical functions for specific applications. Continue practicing with real datasets to strengthen your understanding of these mathematical operations and their applications in data science.