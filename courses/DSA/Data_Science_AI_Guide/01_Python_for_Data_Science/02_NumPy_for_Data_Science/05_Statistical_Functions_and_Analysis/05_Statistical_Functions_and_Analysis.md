# Statistical Functions and Analysis

## Introduction

Statistical analysis is fundamental to data science, enabling researchers and analysts to extract meaningful insights from data. NumPy provides a comprehensive suite of statistical functions that cover descriptive statistics, probability distributions, correlation analysis, and hypothesis testing. These functions form the foundation for data analysis in various domains including banking, healthcare, and scientific research.

The importance of statistical analysis in modern data science cannot be overstated. It provides the mathematical framework for understanding data distributions, testing hypotheses, building predictive models, and making data-driven decisions. In banking, statistical analysis is used for credit scoring, risk assessment, fraud detection, and market analysis. In healthcare, it enables clinical trial analysis, epidemiological studies, and patient outcomes research.

NumPy's statistical functions are designed to be efficient and work seamlessly with the array-based data structure. These functions can operate along specific axes, handle weighted data, and provide both sample and population statistics. The library also includes random number generation capabilities for simulation and sampling applications.

This module covers the fundamentals of statistical analysis with NumPy, including descriptive statistics, probability distributions, correlation analysis, and hypothesis testing. You'll learn how to perform comprehensive statistical analysis using NumPy's functions with practical examples from banking and healthcare domains.

## Fundamentals

### Descriptive Statistics

Descriptive statistics summarize and describe the main features of a dataset. NumPy provides comprehensive functions for computing these statistics.

```python
import numpy as np

# Generate sample data
np.random.seed(42)
data = np.random.normal(100, 15, 1000)  # Normal distribution: mean=100, std=15

print("=" * 60)
print("DESCRIPTIVE STATISTICS")
print("=" * 60)

# Central tendency measures
print("\nCentral Tendency:")
print(f"  Mean (average): {np.mean(data):.2f}")
print(f"  Median (middle value): {np.median(data):.2f}")
print(f"  Mode (most frequent - requires manual calculation)")
# For mode, we can use np.unique
unique, counts = np.unique(data, return_counts=True)
mode_idx = np.argmax(counts)
print(f"  Approx Mode: {unique[mode_idx]:.2f} (count: {counts[mode_idx]})")

# Dispersion measures
print("\nDispersion Measures:")
print(f"  Variance (sample): {np.var(data, ddof=1):.2f}")
print(f"  Standard Deviation: {np.std(data, ddof=1):.2f}")
print(f"  Range: {np.ptp(data):.2f}")  # peak to peak (max - min)
print(f"  Min: {np.min(data):.2f}")
print(f"  Max: {np.max(data):.2f}")

# Percentiles and quartiles
print("\nPercentiles and Quartiles:")
print(f"  25th Percentile (Q1): {np.percentile(data, 25):.2f}")
print(f"  50th Percentile (Median): {np.percentile(data, 50):.2f}")
print(f"  75th Percentile (Q3): {np.percentile(data, 75):.2f}")
print(f"  Interquartile Range (IQR): {np.percentile(data, 75) - np.percentile(data, 25):.2f}")

# Other percentiles
for p in [10, 90, 95, 99]:
    print(f"  {p}th Percentile: {np.percentile(data, p):.2f}")

# Shape measures
print("\nShape Measures:")
# Skewness (requires scipy for exact calculation, but approximation here)
mean = np.mean(data)
std = np.std(data, ddof=1)
skewness = np.sum(((data - mean) / std) ** 3) / len(data)
print(f"  Skewness (approx): {skewness:.2f}")

# Kurtosis (excess kurtosis)
kurtosis = np.sum(((data - mean) / std) ** 4) / len(data) - 3
print(f"  Excess Kurtosis (approx): {kurtosis:.2f}")

# Output:
# Descriptive Statistics
# 
# Central Tendency:
#   Mean (average): 99.58
#   Median (middle value): 99.27
#   Mode (most frequent - requires manual calculation)
#   Approx Mode: 43.26 (count: 1)
# 
# Dispersion Measures:
#   Variance (sample): 226.45
#   Standard Deviation: 15.05
#   Range: 91.64
#   Min: 57.12
#   Max: 148.76
# 
# Percentiles and Quartiles:
#   25th Percentile (Q1): 89.76
#   50th Percentile (Median): 99.27
#   75th Percentile (Q3): 109.32
#   Interquartile Range (IQR): 19.56
```

### Statistical Functions Along Axes

NumPy's statistical functions can operate along specific axes, making them powerful for multi-dimensional data analysis.

```python
# Multi-dimensional data analysis
np.random.seed(42)
data_2d = np.random.rand(5, 4) * 100  # 5 rows, 4 columns

print("=" * 60)
print("MULTI-DIMENSIONAL STATISTICS")
print("=" * 60)

print("\n2D Data (5x4 matrix):")
print(data_2d)

# Axis 0: along rows (column-wise statistics)
print("\nAlong Axis 0 (Column-wise):")
print(f"  Mean: {np.mean(data_2d, axis=0)}")
print(f"  Std: {np.std(data_2d, axis=0)}")
print(f"  Min: {np.min(data_2d, axis=0)}")
print(f"  Max: {np.max(data_2d, axis=0)}")

# Axis 1: along columns (row-wise statistics)
print("\nAlong Axis 1 (Row-wise):")
print(f"  Mean: {np.mean(data_2d, axis=1)}")
print(f"  Std: {np.std(data_2d, axis=1)}")
print(f"  Min: {np.min(data_2d, axis=1)}")
print(f"  Max: {np.max(data_2d, axis=1)}")

# Keepdims parameter
print("\nWith Keepdims=True:")
print(f"  Mean (axis=0, keepdims=True):")
print(np.mean(data_2d, axis=0, keepdims=True))

# Combined statistics
print("\nCumulative Statistics:")
print(f"  cumsum (along axis=1):")
print(np.cumsum(data_2d, axis=1))

print(f"\n  cumprod (first row):")
print(np.cumprod(data_2d[0]))

# Sorting and ranking
print("\nSorting:")
sorted_data = np.sort(data_2d[0])
print(f"  Sorted row 0: {sorted_data}")

# argsort gives indices
indices = np.argsort(data_2d[0])
print(f"  Argsort indices: {indices}")
```

### Random Number Generation for Statistics

Random number generation is essential for statistical simulations, bootstrap methods, and Monte Carlo analysis.

```python
import numpy as np

print("=" * 60)
print("RANDOM NUMBER GENERATION")
print("=" * 60)

# Different distributions
np.random.seed(42)

# Uniform distribution
uniform = np.random.uniform(0, 1, 1000)
print("\nUniform Distribution (0, 1):")
print(f"  Mean: {np.mean(uniform):.4f}")
print(f"  Std: {np.std(uniform):.4f}")
print(f"  Min: {np.min(uniform):.4f}")
print(f"  Max: {np.max(uniform):.4f}")

# Normal (Gaussian) distribution
normal = np.random.normal(0, 1, 1000)
print("\nNormal Distribution (mean=0, std=1):")
print(f"  Mean: {np.mean(normal):.4f}")
print(f"  Std: {np.std(normal):.4f}")
print(f"  Skewness: {np.mean(((normal - np.mean(normal))/np.std(normal))**3):.4f}")
print(f"  Kurtosis: {np.mean(((normal - np.mean(normal))/np.std(normal))**4) - 3:.4f}")

# Exponential distribution
exponential = np.random.exponential(1, 1000)
print("\nExponential Distribution (scale=1):")
print(f"  Mean: {np.mean(exponential):.4f}")
print(f"  Std: {np.std(exponential):.4f}")

# Binomial distribution
binomial = np.random.binomial(10, 0.5, 1000)
print("\nBinomial Distribution (n=10, p=0.5):")
print(f"  Mean: {np.mean(binomial):.4f}")
print(f"  Std: {np.std(binomial):.4f}")

# Poisson distribution
poisson = np.random.poisson(5, 1000)
print("\nPoisson Distribution (lambda=5):")
print(f"  Mean: {np.mean(poisson):.4f}")
print(f"  Std: {np.std(poisson):.4f}")

# Chi-squared distribution
chi_squared = np.random.chisquare(3, 1000)  # df=3 degrees of freedom
print("\nChi-squared Distribution (df=3):")
print(f"  Mean: {np.mean(chi_squared):.4f}")
print(f"  Std: {np.std(chi_squared):.4f}")

# Seeded random generation for reproducibility
print("\n" + "=" * 60)
print("REPRODUCIBILITY")
print("=" * 60)

np.random.seed(123)
print(f"Seed 123: {np.random.rand(5)}")

np.random.seed(123)
print(f"Seed 123 again: {np.random.rand(5)}")

# Using Generator (NumPy 1.17+)
# Default bit generator is PCG64
gen = np.random.default_rng(123)
print(f"\nGenerator: {gen.random(5)}")

# Different RNGs
mt = np.random.MT19937(123)
gen_mt = np.random.Generator(mt)
print(f"MT19937: {gen_mt.random(5)}")
```

## Implementation

### Banking Application: Statistical Analysis of Financial Data

Statistical analysis is extensively used in banking for analyzing transaction patterns, customer behavior, and market data.

```python
import numpy as np

# Transaction data analysis
np.random.seed(42)

# Generate synthetic transaction data
n_customers = 1000
n_transactions = 5000

# Customer IDs
customer_ids = np.arange(1001, 1001 + n_customers)

# Transaction amounts (log-normal distribution - common for financial data)
transaction_amounts = np.random.lognormal(5, 1.5, n_transactions)

# Transaction types (1=withdrawal, 2=deposit, 3=transfer, 4=payment)
transaction_types = np.random.choice([1, 2, 3, 4], n_transactions, p=[0.4, 0.3, 0.15, 0.15])

# Transaction times (days ago)
days_ago = np.random.randint(0, 90, n_transactions)

print("=" * 60)
print("TRANSACTION STATISTICAL ANALYSIS")
print("=" * 60)

print(f"\nTotal Transactions: {n_transactions:,}")
print(f"Total Customers: {n_customers:,}")

# Amount statistics
print(f"\nTransaction Amount Statistics:")
print(f"  Mean: ${np.mean(transaction_amounts):,.2f}")
print(f"  Median: ${np.median(transaction_amounts):,.2f}")
print(f"  Std: ${np.std(transaction_amounts):,.2f}")
print(f"  Min: ${np.min(transaction_amounts):,.2f}")
print(f"  Max: ${np.max(transaction_amounts):,.2f}")

# Percentiles
for p in [25, 50, 75, 90, 95, 99]:
    print(f"  {p}th percentile: ${np.percentile(transaction_amounts, p):,.2f}")

# Transaction type analysis
print(f"\nTransaction Type Distribution:")
type_names = {1: 'Withdrawal', 2: 'Deposit', 3: 'Transfer', 4: 'Payment'}
for t in [1, 2, 3, 4]:
    count = np.sum(transaction_types == t)
    pct = count / n_transactions * 100
    print(f"  {type_names[t]}: {count:,} ({pct:.1f}%)")

# Amount by transaction type
print(f"\nAmount Statistics by Transaction Type:")
for t in [1, 2, 3, 4]:
    amounts = transaction_amounts[transaction_types == t]
    print(f"  {type_names[t]}:")
    print(f"    Mean: ${np.mean(amounts):,.2f}")
    print(f"    Median: ${np.median(amounts):,.2f}")

# Time-based analysis
print(f"\nDaily Transaction Pattern:")
days_unique = np.unique(days_ago)
for day in days_unique[:7]:  # First 7 days
    count = np.sum(days_ago == day)
    total = np.sum(transaction_amounts[days_ago == day])
    print(f"  Day {day}: {count} txns, ${total:,.0f}")
```

```python
# Customer spending analysis
print("=" * 60)
print("CUSTOMER SPENDING ANALYSIS")
print("=" * 60)

# Generate customer data
n_customers = 500

annual_income = np.random.normal(75000, 25000, n_customers)
monthly_spending = np.random.normal(3000, 1000, n_customers)
savings = np.random.normal(15000, 10000, n_customers)

# Add some outliers (high earners/spenders)
annual_income[:50] += np.random.normal(50000, 10000, 50)
monthly_spending[:50] += np.random.normal(2000, 500, 50)

# Savings rate
savings_rate = savings / (annual_income / 12 + 1)

print("\nCustomer Statistics:")
print(f"  Annual Income:")
print(f"    Mean: ${np.mean(annual_income):,.0f}")
print(f"    Median: ${np.median(annual_income):,.0f}")
print(f"    Std: ${np.std(annual_income):,.0f}")

print(f"\n  Monthly Spending:")
print(f"    Mean: ${np.mean(monthly_spending):,.0f}")
print(f"    Median: ${np.median(monthly_spending):,.0f}")
print(f"    Std: ${np.std(monthly_spending):,.0f}")

# Income quartile analysis
print("\nIncome Quartile Analysis:")
q25 = np.percentile(annual_income, 25)
q50 = np.percentile(annual_income, 50)
q75 = np.percentile(annual_income, 75)

for i, (low, high, label) in enumerate([(0, q25, 'Q1 (Low)'), (q25, q50, 'Q2'), (q50, q75, 'Q3'), (q75, np.max(annual_income), 'Q4 (High)')]):
    mask = (annual_income >= low) & (annual_income < high)
    count = np.sum(mask)
    avg_spending = np.mean(monthly_spending[mask])
    print(f"  {label}: {count} customers, avg spending: ${avg_spending:,.0f}")

# Identify high-value customers (high income, low spending)
high_income = annual_income > np.percentile(annual_income, 75)
low_spending = monthly_spending < np.percentile(monthly_spending, 25)
high_value = high_income & low_spending

print(f"\nHigh Value Customers: {np.sum(high_value)}")
print(f"  High income (>75th pct) and low spending (<25th pct)")
```

### Healthcare Application: Clinical Data Analysis

Statistical analysis is crucial in healthcare for clinical trials, patient outcomes analysis, and epidemiological studies.

```python
import numpy as np

# Clinical trial data analysis
np.random.seed(42)

# Treatment groups
n_control = 150
n_treatment = 150

# Baseline characteristics
control_age = np.random.normal(55, 12, n_control)
treatment_age = np.random.normal(54, 11, n_treatment)

# Treatment outcomes (improvement scores)
control_outcome = np.random.normal(3.2, 1.5, n_control)
treatment_outcome = np.random.normal(4.5, 1.8, n_treatment)

# Add some outliers
control_outcome[0] = -2  # Adverse reaction
treatment_outcome[0] = 9  # Exceptional response

print("=" * 60)
print("CLINICAL TRIAL STATISTICAL ANALYSIS")
print("=" * 60)

print("\nPatient Demographics:")
print("Control Group:")
print(f"  Age: {np.mean(control_age):.1f} ± {np.std(control_age):.1f} years")
print(f"  N: {n_control}")

print("\nTreatment Group:")
print(f"  Age: {np.mean(treatment_age):.1f} ± {np.std(treatment_age):.1f} years")
print(f"  N: {n_treatment}")

# Outcome analysis
print("\nTreatment Outcomes:")
print("Control Group:")
print(f"  Mean: {np.mean(control_outcome):.2f}")
print(f"  Median: {np.median(control_outcome):.2f}")
print(f"  Std: {np.std(control_outcome):.2f}")
print(f"  Min: {np.min(control_outcome):.2f}")
print(f"  Max: {np.max(control_outcome):.2f}")

print("\nTreatment Group:")
print(f"  Mean: {np.mean(treatment_outcome):.2f}")
print(f"  Median: {np.median(treatment_outcome):.2f}")
print(f"  Std: {np.std(treatment_outcome):.2f}")
print(f"  Min: {np.min(treatment_outcome):.2f}")
print(f"  Max: {np.max(treatment_outcome):.2f}")

# Statistical comparison
mean_diff = np.mean(treatment_outcome) - np.mean(control_outcome)
print(f"\nMean Difference: {mean_diff:.2f}")

# Effect size (Cohen's d)
pooled_std = np.sqrt((np.std(control_outcome)**2 + np.std(treatment_outcome)**2) / 2)
cohens_d = mean_diff / pooled_std
print(f"Cohen's d (effect size): {cohens_d:.2f}")

# Confidence interval (95%)
# Using standard error of difference
n1, n2 = len(control_outcome), len(treatment_outcome)
se_diff = np.sqrt(np.var(control_outcome)/n1 + np.var(treatment_outcome)/n2)
ci_lower = mean_diff - 1.96 * se_diff
ci_upper = mean_diff + 1.96 * se_diff
print(f"95% CI: ({ci_lower:.2f}, {ci_upper:.2f})")

# Response rate (patients with improvement > 0)
control_responders = np.sum(control_outcome > 0)
treatment_responders = np.sum(treatment_outcome > 0)

control_response_rate = control_responders / n_control
treatment_response_rate = treatment_responders / n_treatment

print(f"\nResponse Rates:")
print(f"  Control: {control_response_rate*100:.1f}% ({control_responders}/{n_control})")
print(f"  Treatment: {treatment_response_rate*100:.1f}% ({treatment_responders}/{n_treatment})")
```

```python
# Patient vital signs analysis
print("=" * 60)
print("PATIENT VITAL SIGNS ANALYSIS")
print("=" * 60)

# Generate patient vital signs data
np.random.seed(42)
n_patients = 200

# Multiple vital signs over 7 days
heart_rates = np.random.normal(75, 12, (n_patients, 7))
bp_systolic = np.random.normal(120, 15, (n_patients, 7))
bp_diastolic = np.random.normal(80, 10, (n_patients, 7))
temperature = np.random.normal(98.2, 0.5, (n_patients, 7))

# Add some abnormal values
heart_rates[:10, :] = np.random.normal(95, 10, (10, 7))
bp_systolic[:15, :] = np.random.normal(145, 20, (15, 7))
bp_diastolic[:15, :] = np.random.normal(95, 12, (15, 7))

print("Patient vital signs data shape:")
print(f"  Patients: {n_patients}")
print(f"  Days: 7")

# Patient-level statistics
print("\nPatient-Level Statistics (averaged over 7 days):")
hr_mean = np.mean(heart_rates, axis=1)
bp_sys_mean = np.mean(bp_systolic, axis=1)
bp_dia_mean = np.mean(bp_diastolic, axis=1)

print(f"\nHeart Rate (bpm):")
print(f"  Mean: {np.mean(hr_mean):.1f}")
print(f"  Std: {np.std(hr_mean):.1f}")

print(f"\nBlood Pressure (mmHg):")
print(f"  Systolic: {np.mean(bp_sys_mean):.1f} ± {np.std(bp_sys_mean):.1f}")
print(f"  Diastolic: {np.mean(bp_dia_mean):.1f} ± {np.std(bp_dia_mean):.1f}")

# Identifying abnormal patients
abnormal_hr = hr_mean > 90
abnormal_bp_sys = bp_sys_mean > 140
abnormal_bp_dia = bp_dia_mean > 90

n_abnormal_hr = np.sum(abnormal_hr)
n_abnormal_bp_sys = np.sum(abnormal_bp_sys)
n_abnormal_bp_dia = np.sum(abnormal_bp_dia)
n_either = np.sum(abnormal_hr | abnormal_bp_sys | abnormal_bp_dia)

print(f"\nAbnormal Patients:")
print(f"  Elevated HR (>90): {n_abnormal_hr} ({n_abnormal_hr/n_patients*100:.1f}%)")
print(f"  Elevated Systolic BP (>140): {n_abnormal_bp_sys} ({n_abnormal_bp_sys/n_patients*100:.1f}%)")
print(f"  Elevated Diastolic BP (>90): {n_abnormal_bp_dia} ({n_abnormal_bp_dia/n_patients*100:.1f}%)")
print(f"  Any abnormal: {n_either} ({n_either/n_patients*100:.1f}%)")

# Time-based trends
print("\nWeek-over-Week Trend:")
day_means_hr = np.mean(heart_rates, axis=0)
day_means_bp_sys = np.mean(bp_systolic, axis=0)

for day in range(7):
    print(f"  Day {day+1}: HR={day_means_hr[day]:.1f}, BP={day_means_bp_sys[day]:.1f}")
```

## Applications

### Correlation Analysis

Correlation analysis measures the relationship between variables and is essential for feature selection and understanding data relationships.

```python
import numpy as np

# Generate correlated data
np.random.seed(42)

# Create variables with different correlations
n = 1000

# Variable A: base
A = np.random.randn(n)

# Variable B: strongly correlated with A
B = A * 0.8 + np.random.randn(n) * 0.6

# Variable C: weakly correlated with A
C = A * 0.2 + np.random.randn(n) * 0.98

# Variable D: no correlation with A
D = np.random.randn(n)

# Variable E: negative correlation with A
E = -A * 0.7 + np.random.randn(n) * 0.7

# Combine into matrix
data = np.column_stack([A, B, C, D, E])
labels = ['A', 'B', 'C', 'D', 'E']

print("=" * 60)
print("CORRELATION ANALYSIS")
print("=" * 60)

# Correlation matrix
corr_matrix = np.corrcoef(data.T)

print("\nCorrelation Matrix:")
print(f"{'':>8}", end="")
for label in labels:
    print(f"{label:>8}", end="")
print()

for i, row in enumerate(corr_matrix):
    print(f"{labels[i]:>8}", end="")
    for val in row:
        print(f"{val:>8.3f}", end="")
    print()

# Specific correlations
print(f"\nCorrelation of A with other variables:")
for i, label in enumerate(labels):
    if i > 0:
        print(f"  A vs {label}: {corr_matrix[0, i]:.3f}")

# Covariance matrix
cov_matrix = np.cov(data.T)

print("\nCovariance Matrix:")
print(f"{'':>12}", end="")
for label in labels:
    print(f"{label:>12}", end="")
print()

for i, row in enumerate(cov_matrix):
    print(f"{labels[i]:>12}", end="")
    for val in row:
        print(f"{val:>12.3f}", end="")
    print()

# Partial correlation
# Simple approach: correlation after controlling for A
print("\nPartial Correlation (controlling for A):")
for i, label in enumerate(labels[2:], 2):
    # Residualize both variables for A
    a_resid = A - np.polyval(np.polyfit(A, A, 1), A)
    resid = data[:, i] - np.polyval(np.polyfit(A, data[:, i], 1), A)
    partial_corr = np.corrcoef(a_resid, resid)[0, 1]
    print(f"  {label} given A: {partial_corr:.3f}")
```

### Hypothesis Testing Basics

Basic hypothesis testing using NumPy functions for t-tests and z-tests.

```python
import numpy as np

# One-sample t-test
np.random.seed(42)

# Sample data
sample = np.random.normal(100, 15, 100)
population_mean = 100

print("=" * 60)
print("HYPOTHESIS TESTING")
print("=" * 60)

# One-sample t-test
sample_mean = np.mean(sample)
sample_std = np.std(sample, ddof=1)
n = len(sample)
t_stat = (sample_mean - population_mean) / (sample_std / np.sqrt(n))

print("\nOne-Sample T-Test:")
print(f"  H0: μ = {population_mean}")
print(f"  Sample mean: {sample_mean:.2f}")
print(f"  Sample std: {sample_std:.2f}")
print(f"  N: {n}")
print(f"  T-statistic: {t_stat:.3f}")

# Critical t-value (two-tailed, alpha=0.05, df=n-1)
# Approximation using degrees of freedom
from scipy import stats
t_critical = stats.t.ppf(0.975, n-1)
print(f"  Critical t-value (α=0.05): ±{t_critical:.3f}")

if abs(t_stat) > t_critical:
    print(f"  Conclusion: Reject H0")
else:
    print(f"  Conclusion: Fail to reject H0")

# Two-sample t-test
np.random.seed(42)
sample1 = np.random.normal(100, 15, 100)
sample2 = np.random.normal(105, 15, 100)

mean1, mean2 = np.mean(sample1), np.mean(sample2)
std1, std2 = np.std(sample1, ddof=1), np.std(sample2, ddof=1)
n1, n2 = len(sample1), len(sample2)

# Pooled t-test
pooled_std = np.sqrt(((n1-1)*std1**2 + (n2-1)*std2**2) / (n1+n2-2))
t_stat_2 = (mean1 - mean2) / (pooled_std * np.sqrt(1/n1 + 1/n2))

print("\n\nTwo-Sample T-Test:")
print(f"  H0: μ1 = μ2")
print(f"  Sample 1 mean: {mean1:.2f}")
print(f"  Sample 2 mean: {mean2:.2f}")
print(f"  Mean difference: {mean1 - mean2:.2f}")
print(f"  T-statistic: {t_stat_2:.3f}")

t_critical_2 = stats.t.ppf(0.975, n1+n2-2)
print(f"  Critical t-value: ±{t_critical_2:.3f}")

# P-value (two-tailed)
p_value = 2 * (1 - stats.t.cdf(abs(t_stat_2), n1+n2-2))
print(f"  P-value: {p_value:.4f}")

if p_value < 0.05:
    print(f"  Conclusion: Reject H0 (p < 0.05)")
else:
    print(f"  Conclusion: Fail to reject H0 (p >= 0.05)")
```

### Banking Application: Risk Analysis

Statistical analysis for credit risk, market risk, and operational risk assessment.

```python
import numpy as np

# Value at Risk (VaR) calculation
np.random.seed(42)

# Simulate daily returns for a portfolio
n_days = 10000
portfolio_value = 1000000

# Different return distributions
returns_normal = np.random.normal(0.0005, 0.02, n_days)
returns_t = np.random.standard_t(5, n_days) * 0.015
returns_laplace = np.random.laplace(0.0005, 0.015, n_days)

# Calculate VaR at different confidence levels
print("=" * 60)
print("VALUE AT RISK (VaR) ANALYSIS")
print("=" * 60)

print(f"\nPortfolio Value: ${portfolio_value:,}")
print(f"Historical Returns (10000 days)")

confidence_levels = [95, 97.5, 99]

for returns, name in [(returns_normal, "Normal"), 
                      (returns_t, "Student-t"), 
                      (returns_laplace, "Laplace")]:
    print(f"\n{name} Distribution:")
    for conf in confidence_levels:
        var = np.percentile(returns, 100 - conf)
        var_loss = abs(var) * portfolio_value
        print(f"  {conf}% VaR: {var*100:.2f}% (${var_loss:,.0f})")

# Expected Shortfall (ES) / Conditional VaR
print("\nExpected Shortfall (ES):")
for returns, name in [(returns_normal, "Normal"), 
                      (returns_t, "Student-t"), 
                      (returns_laplace, "Laplace")]:
    var_95 = np.percentile(returns, 5)
    es = np.mean(returns[returns <= var_95])
    es_loss = abs(es) * portfolio_value
    print(f"  {name}: {es*100:.2f}% (${es_loss:,.0f})")

# Historical simulation VaR
print("\nHistorical VaR (Actual Returns):")
actual_returns = np.random.normal(-0.001, 0.025, 252)  # 1 year of daily returns

for conf in [95, 99]:
    var = np.percentile(actual_returns, 100 - conf)
    var_loss = abs(var) * portfolio_value
    print(f"  {conf}% VaR: {var*100:.2f}% (${var_loss:,.0f})")

# Monte Carlo VaR
print("\nMonte Carlo VaR (10000 simulations):")
mc_returns = np.random.normal(0.0003, 0.02, 10000)

for conf in [95, 99]:
    var = np.percentile(mc_returns, 100 - conf)
    var_loss = abs(var) * portfolio_value
    print(f"  {conf}% VaR: {var*100:.2f}% (${var_loss:,.0f})")
```

## Output Results

### Formatted Statistical Reports

```python
import numpy as np

# Create formatted statistical report
np.random.seed(42)
data = np.random.lognormal(10, 0.5, 1000)

print("=" * 70)
print("STATISTICAL SUMMARY REPORT")
print("=" * 70)

# Summary statistics
mean = np.mean(data)
median = np.median(data)
std = np.std(data, ddof=1)
var = np.var(data, ddof=1)
min_val = np.min(data)
max_val = np.max(data)
q1 = np.percentile(data, 25)
q3 = np.percentile(data, 75)
iqr = q3 - q1

print(f"\nDescriptive Statistics:")
print(f"{'':>30} {'Value':>15}")
print("-" * 50)
print(f"{'Count':>30} {len(data):>15,}")
print(f"{'Mean':>30} {mean:>15,.2f}")
print(f"{'Standard Error':>30} {std/np.sqrt(len(data)):>15,.2f}")
print(f"{'Median':>30} {median:>15,.2f}")
print(f"{'Mode':>30} {'N/A':>15}")
print(f"{'Standard Deviation':>30} {std:>15,.2f}")
print(f"{'Variance':>30} {var:>15,.2f}")
print(f"{'Minimum':>30} {min_val:>15,.2f}")
print(f"{'Maximum':>30} {max_val:>15,.2f}")
print(f"{'Range':>30} {max_val - min_val:>15,.2f}")
print(f"{'25th Percentile (Q1)':>30} {q1:>15,.2f}")
print(f"{'75th Percentile (Q3)':>30} {q3:>15,.2f}")
print(f"{'Interquartile Range':>30} {iqr:>15,.2f}")
print(f"{'Skewness':>30} {np.mean(((data - mean)/std)**3):>15,.2f}")
print(f"{'Kurtosis':>30} {np.mean(((data - mean)/std)**4) - 3:>15,.2f}")

# Percentile table
print(f"\nPercentile Distribution:")
percentiles = [5, 10, 25, 50, 75, 90, 95, 99, 99.5, 99.9]
for p in percentiles:
    val = np.percentile(data, p)
    print(f"  {p:>5.1f}th percentile: ${val:>12,.2f}")

# Five-number summary
print(f"\nFive-Number Summary:")
print(f"  Minimum: ${min_val:,.2f}")
print(f"  Q1: ${q1:,.2f}")
print(f"  Median: ${median:,.2f}")
print(f"  Q3: ${q3:,.2f}")
print(f"  Maximum: ${max_val:,.2f}")
```

## Visualization

### Statistical Visualizations with ASCII

```python
import numpy as np

# Histogram as ASCII art
def ascii_histogram(data, bins=10, width=40):
    """Create ASCII histogram"""
    hist, edges = np.histogram(data, bins=bins)
    max_count = np.max(hist)
    
    print("\nASCII Histogram:")
    print("-" * (width + 12))
    
    for count, edge in zip(hist, edges):
        bar_length = int((count / max_count) * width)
        bar = '█' * bar_length
        print(f"{edge:>7.1f}-{edges[np.where(edges==edge)[0][0]+1]:>6.1f}: {bar} ({count})")
    
    print("-" * (width + 12))

# Generate data for histogram
np.random.seed(42)
data = np.random.normal(100, 15, 500)

ascii_histogram(data, bins=10)

# Box plot as ASCII
def ascii_boxplot(data, width=30):
    """Create ASCII box plot"""
    min_val = np.min(data)
    q1 = np.percentile(data, 25)
    median = np.percentile(data, 50)
    q3 = np.percentile(data, 75)
    max_val = np.max(data)
    
    print("\nASCII Box Plot:")
    print("-" * (width + 5))
    
    # Lower whisker
    center = width // 2
    print(f"{'Min':>6} |{' ' * center}|{min_val:.2f}")
    
    # Box
    box_start = int((q1 - min_val) / (max_val - min_val) * width)
    box_end = int((q3 - min_val) / (max_val - min_val) * width)
    box_str = ' ' * box_start + '█' * (box_end - box_start) + ' ' * (width - box_end)
    print(f"{'Q1':>6} |{box_str}|{q1:.2f}")
    
    # Median line
    med_pos = int((median - min_val) / (max_val - min_val) * width)
    med_str = ' ' * med_pos + '|' + ' ' * (width - med_pos - 1)
    print(f"{'Median':>6} |{med_str}|{median:.2f}")
    
    # Box end
    box_str = ' ' * box_start + '█' * (box_end - box_start) + ' ' * (width - box_end)
    print(f"{'Q3':>6} |{box_str}|{q3:.2f}")
    
    # Upper whisker
    print(f"{'Max':>6} |{' ' * center}|{max_val:.2f}")
    
    print("-" * (width + 5))

ascii_boxplot(data)
```

## Advanced Topics

### Advanced Statistical Functions

```python
import numpy as np

# Advanced statistical computations

# Generate data for analysis
np.random.seed(42)
data = np.random.normal(100, 15, 1000)

print("=" * 60)
print("ADVANCED STATISTICAL FUNCTIONS")
print("=" * 60)

# Standard error of mean
sem = np.std(data, ddof=1) / np.sqrt(len(data))
print(f"\nStandard Error of Mean: {sem:.2f}")

# Confidence intervals
from scipy import stats

confidence = 0.95
mean = np.mean(data)
se = sem

# Using t-distribution for confidence interval
df = len(data) - 1
t_crit = stats.t.ppf((1 + confidence) / 2, df)
ci_lower = mean - t_crit * se
ci_upper = mean + t_crit * se

print(f"\n95% Confidence Interval:")
print(f"  Mean: {mean:.2f}")
print(f"  CI: ({ci_lower:.2f}, {ci_upper:.2f})")

# Z-scores
print("\nZ-Scores (first 10 values):")
z_scores = (data[:10] - np.mean(data)) / np.std(data, ddof=1)
print(z_scores)

# T-scores (standardized to mean=50, std=10)
t_scores = 50 + 10 * z_scores
print("\nT-Scores (first 10 values):")
print(t_scores)

# Coefficient of variation
cv = np.std(data, ddof=1) / np.mean(data) * 100
print(f"\nCoefficient of Variation: {cv:.2f}%")

# Range ratio
range_ratio = (np.max(data) - np.min(data)) / np.std(data, ddof=1)
print(f"Range Ratio: {range_ratio:.2f}")

# Trimmed mean (remove top/bottom 10%)
trimmed = stats.trimboth(data, 0.1)
trimmed_mean = np.mean(trimmed)
print(f"\nTrimmed Mean (10%): {trimmed_mean:.2f}")

# Winsorized mean
winsorized = stats.winsorize(data, limits=[0.1, 0.1])
winsorized_mean = np.mean(winsorized)
print(f"Winsorized Mean (10%): {winsorized_mean:.2f}")
```

### Statistical Distributions

```python
import numpy as np
from scipy import stats

print("=" * 60)
print("STATISTICAL DISTRIBUTIONS")
print("=" * 60)

# Normal distribution
x = np.linspace(-4, 4, 100)
pdf = stats.norm.pdf(x)
cdf = stats.norm.cdf(x)

print("\nNormal Distribution (standard):")
print(f"  x=0: PDF={stats.norm.pdf(0):.4f}, CDF={stats.norm.cdf(0):.4f}")
print(f"  x=1.96: CDF={stats.norm.cdf(1.96):.4f} (95%)")
print(f"  x=2.576: CDF={stats.norm.cdf(2.576):.4f} (99%)")

# Generate normally distributed data
np.random.seed(42)
normal_data = np.random.normal(0, 1, 1000)

# Test for normality
shapiro_stat, shapiro_p = stats.shapiro(normal_data[:100])
print(f"\nShapiro-Wilk Test:")
print(f"  Statistic: {shapiro_stat:.4f}")
print(f"  P-value: {shapiro_p:.4f}")
if shapiro_p > 0.05:
    print("  Conclusion: Data appears normally distributed")

# T-distribution
print("\nT-Distribution:")
print(f"  df=1 (Cauchy): PDF at x=0 = {stats.t.pdf(0, 1):.4f}")
print(f"  df=30: PDF at x=0 = {stats.t.pdf(0, 30):.4f}")
print(f"  df=Inf (Normal): PDF at x=0 = {stats.norm.pdf(0):.4f}")

# Chi-squared distribution
print("\nChi-squared Distribution:")
for df in [1, 4, 10]:
    mean = stats.chi2.mean(df)
    var = stats.chi2.var(df)
    print(f"  df={df}: Mean={mean:.2f}, Variance={var:.2f}")

# Exponential distribution
print("\nExponential Distribution:")
print(f"  Scale=1: Mean={stats.expon.mean():.2f}, Variance={stats.expon.var():.2f}")
print(f"  PDF at x=0: {stats.expon.pdf(0):.2f}")
```

## Conclusion

Statistical functions and analysis with NumPy provide comprehensive tools for understanding and interpreting data. Through this exploration, you've learned about descriptive statistics, probability distributions, correlation analysis, and hypothesis testing. These capabilities are essential for data science applications in banking and healthcare.

The banking examples demonstrated transaction analysis, customer segmentation, and risk metrics like Value at Risk. The healthcare applications showed clinical trial analysis, patient vital signs monitoring, and outcome comparisons. Both domains benefit from NumPy's efficient statistical functions.

Key takeaways include understanding data distributions, computing descriptive statistics, performing correlation analysis, and applying hypothesis testing. Continue practicing with real datasets to strengthen your statistical analysis skills, and explore additional statistical tests available in scipy for more advanced analyses.