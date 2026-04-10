# Statistical Plots with Seaborn

## Table of Contents
1. [Introduction](#introduction)
2. [Fundamentals](#fundamentals)
3. [Implementation](#implementation)
4. [Applications](#applications)
5. [Output Results](#output-results)
6. [Visualization](#visualization)
7. [Advanced Topics](#advanced-topics)
8. [Conclusion](#conclusion)

---

## Introduction

### Overview

Seaborn is a powerful statistical data visualization library built on top of Matplotlib. Created by Michael Waskom, Seaborn provides a high-level interface for creating informative, attractive statistical graphics with minimal code. It integrates closely with Pandas data structures and automatically handles statistical transformations.

This module covers statistical visualization techniques using Seaborn, including distribution plots, categorical plots, regression plots, and matrix plots. These visualizations are essential for exploratory data analysis and presenting statistical findings.

### Learning Objectives

By the end of this module, you will be able to:
- Create distribution visualizations with KDE and rug plots
- Build categorical plots for comparing groups
- Generate regression plots with confidence intervals
- Create heatmaps and cluster visualizations
- Customize Seaborn plots for publication quality
- Apply statistical aesthetics and themes

### Prerequisites

- Python 3.7+
- NumPy, Pandas, Matplotlib
- Seaborn (`pip install seaborn`)

---

## Fundamentals

### Seaborn Architecture

Seaborn operates at a higher abstraction level than Matplotlib:

```
Data Input (DataFrame, Array, List)
         |
         v
    Figure Level Functions (<type>plot)
         |
         +--> Axes Level Functions (axplot)
         |
         v
    Statistical Estimation + Aggregation
         |
         v
       Matplotlib Backend
```

#### Figure-Level vs Axes-Level

Seaborn provides two types of functions:

```python
import seaborn as sns
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd

# Figure-level (creates subplots automatically)
fig = sns.displot(data=df, x='value', hue='category', kind='kde')

# Axes-level (returns matplotlib axes)
fig, ax = plt.subplots()
sns.kdeplot(data=df, x='value', hue='category', ax=ax)
```

### Statistical Plot Types

#### Distribution Plots

Distribution plots show the statistical distribution of data:

```python
import seaborn as sns
import numpy as np
import pandas as pd

# Generate sample data
np.random.seed(42)
df = pd.DataFrame({
    'value': np.concatenate([np.random.normal(50, 10, 100), 
                             np.random.normal(70, 15, 100)]),
    'group': ['A'] * 100 + ['B'] * 100
})

# Histogram with KDE
fig, ax = plt.subplots(figsize=(10, 6))
sns.histplot(data=df, x='value', kde=True, ax=ax, color='#1f77b4')
ax.set_title('Distribution Plot with KDE')
plt.show()

# KDE plot
fig, ax = plt.subplots(figsize=(10, 6))
sns.kdeplot(data=df, x='value', hue='group', ax=ax)
ax.set_title('Kernel Density Estimation')
plt.show()

# Rug plot
fig, ax = plt.subplots(figsize=(10, 6))
sns.kdeplot(data=df, x='value', ax=ax)
sns.rugplot(data=df, x='value', ax=ax, height=0.1)
ax.set_title('KDE with Rug Plot')
plt.show()
```

#### Categorical Plots

Categorical plots compare distributions across categories:

```python
import seaborn as sns
import numpy as np
import pandas as pd

# Generate categorical data
np.random.seed(42)
df = pd.DataFrame({
    'category': ['A', 'B', 'C'] * 50,
    'value': np.random.randn(150) * 20 + 50,
    'subcategory': np.random.choice(['X', 'Y'], 150)
})

# Box plot
fig, ax = plt.subplots(figsize=(10, 6))
sns.boxplot(data=df, x='category', y='value', ax=ax)
ax.set_title('Box Plot')
plt.show()

# Violin plot
fig, ax = plt.subplots(figsize=(10, 6))
sns.violinplot(data=df, x='category', y='value', ax=ax)
ax.set_title('Violin Plot')
plt.show()

# Swarm plot
fig, ax = plt.subplots(figsize=(10, 6))
sns.swarmplot(data=df, x='category', y='value', ax=ax, size=4)
ax.set_title('Swarm Plot')
plt.show()
```

#### Regression Plots

Regression plots show statistical relationships:

```python
import seaborn as sns
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Generate regression data
np.random.seed(42)
df = pd.DataFrame({
    'x': np.linspace(0, 10, 100),
    'y': 2 * np.linspace(0, 10, 100) + 5 + np.random.randn(100) * 3,
    'category': np.random.choice(['A', 'B'], 100)
})

# Scatter with regression line
fig, ax = plt.subplots(figsize=(10, 6))
sns.regplot(data=df, x='x', y='y', ax=ax)
ax.set_title('Regression Plot with Confidence Interval')
plt.show()

# Scatter with multiple regression lines
fig, ax = plt.subplots(figsize=(10, 6))
sns.lmplot(data=df, x='x', y='y', hue='category', ci=95)
ax.set_title('Multiple Regression Plot')
plt.show()
```

### Seaborn Themes and Aesthetics

```python
# Set the theme
sns.set_theme(style='whitegrid', palette='muted', font_scale=1.2)

# Available themes
themes = ['darkgrid', 'whitegrid', 'dark', 'white', 'ticks']

# Available palettes
palettes = ['deep', 'muted', 'pastel', 'bright', 'dark', 'colorblind']

# Custom palette
custom_palette = ['#1f77b4', '#ff7f0e', '#2ca02c', '#d62728', '#9467bd']
sns.set_palette(custom_palette)

# Plot with theme
fig, ax = plt.subplots(figsize=(10, 6))
sns.boxplot(data=df, x='category', y='value', ax=ax, palette=custom_palette)
plt.show()
```

---

## Implementation

### Comprehensive Examples

### Example 1: Financial Portfolio Analysis

```python
import seaborn as sns
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Generate portfolio data
np.random.seed(42)
n_assets = 500
portfolio = pd.DataFrame({
    'asset_id': range(n_assets),
    'return_annual': np.random.normal(0.08, 0.15, n_assets) * 100,
    'risk_volatility': np.random.normal(0.15, 0.08, n_assets) * 100,
    'asset_class': np.random.choice(['Equity', 'Bonds', 'Commodities', 'Real Estate'], n_assets,
                                   p=[0.4, 0.3, 0.15, 0.15]),
    'market_cap': np.random.lognormal(8, 2, n_assets)
})

fig, axes = plt.subplots(2, 2, figsize=(14, 10))

# Distribution of returns
ax1 = axes[0, 0]
sns.histplot(data=portfolio, x='return_annual', kde=True, ax=ax1, color='#1f77b4')
ax1.axvline(x=0, color='red', linestyle='--', alpha=0.7)
ax1.set_xlabel('Annual Return (%)', fontsize=11)
ax1.set_ylabel('Frequency', fontsize=11)
ax1.set_title('Distribution of Annual Returns', fontsize=12, fontweight='bold')

# Risk vs Return scatter
ax2 = axes[0, 1]
sns.scatterplot(data=portfolio, x='risk_volatility', y='return_annual', 
               hue='asset_class', ax=ax2, palette='deep', alpha=0.7)
ax2.axhline(y=0, color='red', linestyle='--', alpha=0.5)
ax2.set_xlabel('Risk (Volatility %)', fontsize=11)
ax2.set_ylabel('Annual Return (%)', fontsize=11)
ax2.set_title('Risk vs Return by Asset Class', fontsize=12, fontweight='bold')

# Returns by asset class (violin)
ax3 = axes[1, 0]
sns.violinplot(data=portfolio, x='asset_class', y='return_annual', ax=ax3, palette='muted')
ax3.axhline(y=0, color='red', linestyle='--', alpha=0.5)
ax3.set_xlabel('Asset Class', fontsize=11)
ax3.set_ylabel('Annual Return (%)', fontsize=11)
ax3.set_title('Return Distribution by Asset Class', fontsize=12, fontweight='bold')

# Market cap distribution (log scale)
ax4 = axes[1, 1]
sns.kdeplot(data=portfolio, x='market_cap', hue='asset_class', ax=ax4, 
           fill=True, alpha=0.3)
ax4.set_xlabel('Market Cap ($)', fontsize=11)
ax4.set_ylabel('Density', fontsize=11)
ax4.set_title('Market Cap Distribution by Asset Class', fontsize=12, fontweight='bold')
ax4.set_xscale('log')

plt.tight_layout()
plt.savefig('portfolio_analysis.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 2: Medical Clinical Trials

```python
import seaborn as sns
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Generate clinical trial data
np.random.seed(42)
n_patients = 600
trials = pd.DataFrame({
    'patient_id': range(n_patients),
    'treatment': np.random.choice(['Placebo', 'Treatment A', 'Treatment B'], n_patients,
                                  p=[0.33, 0.34, 0.33]),
    'age': np.random.normal(50, 15, n_patients).clip(18, 85),
    'baseline_score': np.random.normal(50, 10, n_patients),
    'final_score': 0,
    'improvement': 0.0,
    'adverse_event': np.random.rand(n_patients) < 0.1
})

# Generate realistic final scores based on treatment
for idx, row in trials.iterrows():
    if row['treatment'] == 'Treatment A':
        trials.loc[idx, 'final_score'] = row['baseline_score'] + np.random.normal(10, 5)
        trials.loc[idx, 'improvement'] = np.random.normal(10, 5)
    elif row['treatment'] == 'Treatment B':
        trials.loc[idx, 'final_score'] = row['baseline_score'] + np.random.normal(15, 4)
        trials.loc[idx, 'improvement'] = np.random.normal(15, 4)
    else:
        trials.loc[idx, 'final_score'] = row['baseline_score'] + np.random.normal(2, 6)
        trials.loc[idx, 'improvement'] = np.random.normal(2, 6)

fig, axes = plt.subplots(2, 2, figsize=(14, 10))

# Improvement by treatment (box)
ax1 = axes[0, 0]
sns.boxplot(data=trials, x='treatment', y='improvement', ax=ax1, palette='Set2')
sns.stripplot(data=trials, x='treatment', y='improvement', ax=ax1, 
            color='black', alpha=0.3, size=3)
ax1.axhline(y=0, color='red', linestyle='--', alpha=0.5)
ax1.set_xlabel('Treatment', fontsize=11)
ax1.set_ylabel('Improvement Score', fontsize=11)
ax1.set_title('Clinical Improvement by Treatment', fontsize=12, fontweight='bold')

# Age distribution by treatment
ax2 = axes[0, 1]
sns.histplot(data=trials, x='age', hue='treatment', ax=ax2, 
           kde=True, palette='Set2', alpha=0.6)
ax2.set_xlabel('Age (years)', fontsize=11)
ax2.set_ylabel('Frequency', fontsize=11)
ax2.set_title('Age Distribution by Treatment', fontsize=12, fontweight='bold')

# Baseline vs Final score (scatter)
ax3 = axes[1, 0]
sns.scatterplot(data=trials, x='baseline_score', y='final_score', 
              hue='treatment', ax=ax3, palette='Set2', alpha=0.6)
# Add identity line
ax3.plot([20, 80], [20, 80], 'k--', alpha=0.5, label='No Change')
ax3.set_xlabel('Baseline Score', fontsize=11)
ax3.set_ylabel('Final Score', fontsize=11)
ax3.set_title('Baseline vs Final Score', fontsize=12, fontweight='bold')

# Adverse events by treatment
ax4 = axes[1, 1]
adverse_counts = trials.groupby('treatment')['adverse_event'].mean() * 100
sns.barplot(x=adverse_counts.index, y=adverse_counts.values, ax=ax4, palette='Set2')
ax4.set_xlabel('Treatment', fontsize=11)
ax4.set_ylabel('Adverse Event Rate (%)', fontsize=11)
ax4.set_title('Adverse Events by Treatment', fontsize=12, fontweight='bold')
for i, v in enumerate(adverse_counts.values):
    ax4.text(i, v + 0.3, f'{v:.1f}%', ha='center', fontsize=10)

plt.tight_layout()
plt.savefig('clinical_trials.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 3: Customer Segmentation Analysis

```python
import seaborn as sns
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Generate customer data
np.random.seed(42)
n_customers = 1000
customers = pd.DataFrame({
    'customer_id': range(n_customers),
    'annual_income': np.random.lognormal(10.5, 0.8, n_customers),
    'spending_score': np.random.normal(50, 20, n_customers).clip(0, 100),
    'age': np.random.normal(40, 12, n_customers).clip(18, 75),
    'segment': '',
    'purchase_frequency': np.random.exponential(2, n_customers),
    'loyalty_years': np.random.exponential(3, n_customers).clip(0, 20)
})

# Assign segments based on income and spending
for idx, row in customers.iterrows():
    if row['annual_income'] > 80000 and row['spending_score'] > 60:
        customers.loc[idx, 'segment'] = 'Premium'
    elif row['annual_income'] > 60000 and row['spending_score'] > 40:
        customers.loc[idx, 'segment'] = 'Standard'
    elif row['annual_income'] < 40000 and row['spending_score'] < 30:
        customers.loc[idx, 'segment'] = 'Budget'
    else:
        customers.loc[idx, 'segment'] = 'Economy'

fig, axes = plt.subplots(2, 2, figsize=(14, 10))

# Income vs Spending by segment
ax1 = axes[0, 0]
sns.scatterplot(data=customers, x='annual_income', y='spending_score', 
              hue='segment', ax=ax1, palette='deep', alpha=0.6)
ax1.set_xlabel('Annual Income ($)', fontsize=11)
ax1.set_ylabel('Spending Score', fontsize=11)
ax1.set_title('Customer Segmentation (Income vs Spending)', fontsize=12, fontweight='bold')

# Distributions by segment
ax2 = axes[0, 1]
for segment in ['Budget', 'Economy', 'Standard', 'Premium']:
    subset = customers[customers['segment'] == segment]['annual_income']
    sns.kdeplot(x=subset, ax=ax2, label=segment, linewidth=2)
ax2.set_xlabel('Annual Income ($)', fontsize=11)
ax2.set_ylabel('Density', fontsize=11)
ax2.set_title('Income Distribution by Segment', fontsize=12, fontweight='bold')
ax2.legend(fontsize=9)

# Violin plot by segment
ax3 = axes[1, 0]
order = ['Budget', 'Economy', 'Standard', 'Premium']
sns.violinplot(data=customers, x='segment', y='loyalty_years', ax=ax3, 
             order=order, palette='deep')
ax3.set_xlabel('Customer Segment', fontsize=11)
ax3.set_ylabel('Loyalty (Years)', fontsize=11)
ax3.set_title('Loyalty by Customer Segment', fontsize=12, fontweight='bold')

# Pair plot data
ax4 = axes[1, 1]
sns.barplot(data=customers, x='segment', y='purchase_frequency', ax=ax4, 
         order=order, palette='deep')
ax4.set_xlabel('Customer Segment', fontsize=11)
ax4.set_ylabel('Purchase Frequency', fontsize=11)
ax4.set_title('Purchase Frequency by Segment', fontsize=12, fontweight='bold')

plt.tight_layout()
plt.savefig('customer_segmentation.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 4: Time Series Statistical Analysis

```python
import seaborn as sns
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from datetime import datetime, timedelta

# Generate time series data
np.random.seed(42)
n_days = 365
dates = [datetime(2024, 1, 1) + timedelta(days=i) for i in range(n_days)]

ts_data = pd.DataFrame({
    'date': dates,
    'value': np.cumsum(np.random.randn(n_days)),
    'rolling_mean': pd.Series(np.random.randn(n_days)).rolling(30).mean(),
    'rolling_std': pd.Series(np.random.randn(n_days)).rolling(30).std(),
    'day_of_week': [d.weekday() for d in dates],
    'month': [d.month for d in dates]
})
ts_data['day_name'] = ts_data['date'].dt.day_name()

# Add trend
ts_data['value'] = ts_data['value'] + np.linspace(0, 50, n_days)

fig, axes = plt.subplots(2, 2, figsize=(14, 10))

# Time series with rolling mean
ax1 = axes[0, 0]
sns.lineplot(data=ts_data, x='date', y='value', ax=ax1, color='#1f77b4', alpha=0.3)
sns.lineplot(data=ts_data, x='date', y=ts_data['value'].rolling(30).mean(), 
             ax=ax1, color='#d62728', linewidth=2, label='30-day MA')
ax1.fill_between(ts_data['date'], 
                ts_data['value'].rolling(30).mean() - ts_data['value'].rolling(30).std(),
                ts_data['value'].rolling(30).mean() + ts_data['value'].rolling(30).std(),
                alpha=0.2, color='#d62728')
ax1.set_xlabel('Date', fontsize=11)
ax1.set_ylabel('Value', fontsize=11)
ax1.set_title('Time Series with Rolling Statistics', fontsize=12, fontweight='bold')
ax1.legend(fontsize=9)

# Day of week pattern
ax2 = axes[0, 1]
day_order = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
sns.boxplot(data=ts_data, x='day_name', y='value', ax=ax2, order=day_order, palette='Set3')
ax2.set_xlabel('Day of Week', fontsize=11)
ax2.set_ylabel('Value', fontsize=11)
ax2.set_title('Value Distribution by Day of Week', fontsize=12, fontweight='bold')
ax2.tick_params(axis='x', rotation=45)

# Monthly distribution
ax3 = axes[1, 0]
month_order = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
sns.violinplot(data=ts_data, x='month', y='value', ax=ax3, palette='Set3')
ax3.set_xlabel('Month', fontsize=11)
ax3.set_ylabel('Value', fontsize=11)
ax3.set_title('Value Distribution by Month', fontsize=12, fontweight='bold')

# Regression on time
ax4 = axes[1, 1]
ts_data['day_number'] = range(n_days)
sns.regplot(data=ts_data, x='day_number', y='value', ax=ax4, 
         scatter_kws={'alpha': 0.3}, line_kws={'color': 'red'})
ax4.set_xlabel('Day Number', fontsize=11)
ax4.set_ylabel('Value', fontsize=11)
ax4.set_title('Trend Analysis', fontsize=12, fontweight='bold')

plt.tight_layout()
plt.savefig('time_series_analysis.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 5: Correlation Heatmap Analysis

```python
import seaborn as sns
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Generate multivariate data
np.random.seed(42)
n_samples = 500
data = pd.DataFrame({
    'income': np.random.lognormal(10, 1, n_samples),
    'age': np.random.normal(40, 12, n_samples),
    'credit_score': np.random.normal(650, 100, n_samples).clip(300, 850),
    'debt_ratio': np.random.uniform(0, 0.5, n_samples),
    'employment_years': np.random.exponential(5, n_samples),
    'savings': np.random.lognormal(8, 2, n_samples),
    'loan_amount': np.random.lognormal(9, 1.5, n_samples)
})

# Add correlations
data['loan_amount'] = data['income'] * 0.3 + np.random.randn(n_samples) * 5000
data['savings'] = data['income'] * 0.2 + np.random.randn(n_samples) * 3000
data['credit_score'] = 500 + data['employment_years'] * 10 + np.random.randn(n_samples) * 50

# Calculate correlation matrix
corr_matrix = data.corr()

fig, axes = plt.subplots(1, 2, figsize=(16, 6))

# Basic heatmap
ax1 = axes[0]
sns.heatmap(corr_matrix, annot=True, fmt='.2f', cmap='coolwarm', 
           center=0, ax=ax1, square=True, linewidths=0.5)
ax1.set_title('Correlation Matrix Heatmap', fontsize=12, fontweight='bold')

# Masked heatmap (lower triangle)
ax2 = axes[1]
mask = np.triu(np.ones_like(corr_matrix, dtype=bool))
sns.heatmap(corr_matrix, mask=mask, annot=True, fmt='.2f', cmap='coolwarm', 
           center=0, ax=ax2, square=True, linewidths=0.5)
ax2.set_title('Lower Triangle Correlation', fontsize=12, fontweight='bold')

plt.tight_layout()
plt.savefig('correlation_analysis.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Applications

### Banking Sector Applications

#### Application 1: Credit Risk Assessment

```python
import seaborn as sns
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Generate credit data
np.random.seed(42)
n_applicants = 2000
credit_data = pd.DataFrame({
    'applicant_id': range(n_applicants),
    'credit_score': np.random.normal(650, 100, n_applicants).clip(300, 850),
    'annual_income': np.random.lognormal(10.5, 0.8, n_applicants),
    'debt_to_income': np.random.uniform(0.1, 0.5, n_applicants),
    'employment_years': np.random.exponential(5, n_applicants),
    'num_credit_lines': np.random.poisson(5, n_applicants),
    'loan_amount': np.random.lognormal(9.5, 1.2, n_applicants),
    'default': (np.random.rand(n_applicants) < 0.1).astype(int)
})

# Introduce some realistic correlation with default
credit_data.loc[credit_data['credit_score'] < 500, 'default'] = 1
credit_data.loc[credit_data['debt_to_income'] > 0.4, 'default'] = 1

fig, axes = plt.subplots(2, 2, figsize=(14, 10))

# Default rate by credit score bucket
ax1 = axes[0, 0]
credit_data['score_bucket'] = pd.cut(credit_data['credit_score'], 
                                  bins=[0, 500, 600, 700, 800, 850],
                                  labels=['<500', '500-600', '600-700', '700-800', '800+'])
default_rate = credit_data.groupby('score_bucket')['default'].mean() * 100
bars = sns.barplot(x=default_rate.index, y=default_rate.values, ax=ax1, palette='Reds_r')
ax1.set_xlabel('Credit Score Bucket', fontsize=11)
ax1.set_ylabel('Default Rate (%)', fontsize=11)
ax1.set_title('Default Rate by Credit Score', fontsize=12, fontweight='bold')

# Credit score distribution (default vs non-default)
ax2 = axes[0, 1]
sns.kdeplot(data=credit_data[credit_data['default']==0], x='credit_score', 
          ax=ax2, label='Non-Default', fill=True, alpha=0.5)
sns.kdeplot(data=credit_data[credit_data['default']==1], x='credit_score', 
          ax=ax2, label='Default', fill=True, alpha=0.5)
ax2.set_xlabel('Credit Score', fontsize=11)
ax2.set_ylabel('Density', fontsize=11)
ax2.set_title('Credit Score Distribution by Default Status', fontsize=12, fontweight='bold')
ax2.legend(fontsize=9)

# Income vs loan amount by default
ax3 = axes[1, 0]
sns.scatterplot(data=credit_data, x='annual_income', y='loan_amount', 
              hue='default', ax=ax3, palette='Set1', alpha=0.5)
ax3.set_xlabel('Annual Income ($)', fontsize=11)
ax3.set_ylabel('Loan Amount ($)', fontsize=11)
ax3.set_title('Income vs Loan Amount by Default', fontsize=12, fontweight='bold')

# Violin plot of DTI by default
ax4 = axes[1, 1]
sns.violinplot(data=credit_data, x='default', y='debt_to_income', ax=ax4, palette='Set1')
ax4.set_xlabel('Default Status', fontsize=11)
ax4.set_ylabel('Debt to Income Ratio', fontsize=11)
ax4.set_title('Debt-to-Income by Default Status', fontsize=12, fontweight='bold')

plt.tight_layout()
plt.savefig('credit_risk_analysis.png', dpi=300, bbox_inches='tight')
plt.show()

# Print summary statistics
print("=" * 60)
print("CREDIT RISK ASSESSMENT RESULTS")
print("=" * 60)
print(f"Total Applicants: {n_applicants}")
print(f"Default Rate: {credit_data['default'].mean()*100:.2f}%")
print(f"\nDefault Rate by Credit Score:")
for bucket, rate in default_rate.items():
    print(f"  {bucket}: {rate:.2f}%")
print(f"\nAverage Credit Score:")
print(f"  Non-Default: {credit_data[credit_data['default']==0]['credit_score'].mean():.1f}")
print(f"  Default: {credit_data[credit_data['default']==1]['credit_score'].mean():.1f}")
print("=" * 60)
```

### Healthcare Sector Applications

#### Application 2: Patient Outcome Analysis

```python
import seaborn as sns
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# Generate patient outcome data
np.random.seed(42)
n_patients = 1500
patients = pd.DataFrame({
    'patient_id': range(n_patients),
    'age': np.random.normal(55, 15, n_patients).clip(18, 90),
    'bmi': np.random.normal(28, 5, n_patients).clip(15, 50),
    'systolic_bp': np.random.normal(130, 15, n_patients).clip(90, 200),
    'treatment_outcome': np.random.choice(['Excellent', 'Good', 'Fair', 'Poor'], 
                                         n_patients, p=[0.3, 0.4, 0.2, 0.1]),
    'length_of_stay': np.random.exponential(5, n_patients).clip(1, 30),
    'readmission_30day': (np.random.rand(n_patients) < 0.12).astype(int),
    'complication': (np.random.rand(n_patients) < 0.08).astype(int)
})

# Add realistic correlations
patients.loc[patients['bmi'] > 35, 'complication'] = 1
patients.loc[patients['systolic_bp'] > 160, 'complication'] = 1

fig, axes = plt.subplots(2, 2, figsize=(14, 10))

# Outcome by age group
ax1 = axes[0, 0]
patients['age_group'] = pd.cut(patients['age'], 
                              bins=[0, 30, 45, 60, 75, 100],
                              labels=['18-30', '31-45', '46-60', '61-75', '75+'])
outcome_order = ['Excellent', 'Good', 'Fair', 'Poor']
outcome_pct = patients.groupby('age_group')['treatment_outcome'].value_counts(normalize=True).unstack() * 100
outcome_pct[outcome_order].plot(kind='bar', ax=ax1, colormap='RdYlGn_r')
ax1.set_xlabel('Age Group', fontsize=11)
ax1.set_ylabel('Percentage (%)', fontsize=11)
ax1.set_title('Treatment Outcome by Age Group', fontsize=12, fontweight='bold')
ax1.legend(title='Outcome', fontsize=8)
ax1.tick_params(axis='x', rotation=0)

# BMI vs length of stay
ax2 = axes[0, 1]
sns.scatterplot(data=patients, x='bmi', y='length_of_stay', 
              hue='treatment_outcome', ax=ax2, palette='RdYlGn_r', alpha=0.6)
ax2.set_xlabel('BMI', fontsize=11)
ax2.set_ylabel('Length of Stay (days)', fontsize=11)
ax2.set_title('BMI vs Length of Stay by Outcome', fontsize=12, fontweight='bold')

# Readmission rate by age
ax3 = axes[1, 0]
readmit_rate = patients.groupby('age_group')['readmission_30day'].mean() * 100
sns.barplot(x=readmit_rate.index, y=readmit_rate.values, ax=ax3, palette='Oranges')
ax3.set_xlabel('Age Group', fontsize=11)
ax3.set_ylabel('30-Day Readmission Rate (%)', fontsize=11)
ax3.set_title('Readmission Rate by Age Group', fontsize=12, fontweight='bold')
for i, v in enumerate(readmit_rate.values):
    ax3.text(i, v + 0.3, f'{v:.1f}%', ha='center', fontsize=9)

# Complication by BMI category
ax4 = axes[1, 1]
patients['bmi_category'] = pd.cut(patients['bmi'], 
                                  bins=[0, 18.5, 25, 30, 100],
                                  labels=['Underweight', 'Normal', 'Overweight', 'Obese'])
comp_rate = patients.groupby('bmi_category')['complication'].mean() * 100
sns.barplot(x=comp_rate.index, y=comp_rate.values, ax=ax4, palette='Reds')
ax4.set_xlabel('BMI Category', fontsize=11)
ax4.set_ylabel('Complication Rate (%)', fontsize=11)
ax4.set_title('Complication Rate by BMI Category', fontsize=12, fontweight='bold')
for i, v in enumerate(comp_rate.values):
    ax4.text(i, v + 0.2, f'{v:.1f}%', ha='center', fontsize=9)

plt.tight_layout()
plt.savefig('patient_outcomes.png', dpi=300, bbox_inches='tight')
plt.show()

# Print summary
print("=" * 60)
print("PATIENT OUTCOME ANALYSIS RESULTS")
print("=" * 60)
print(f"Total Patients: {n_patients}")
print(f"Average Length of Stay: {patients['length_of_stay'].mean():.1f} days")
print(f"30-Day Readmission Rate: {patients['readmission_30day'].mean()*100:.2f}%")
print(f"Complication Rate: {patients['complication'].mean()*100:.2f}%")
print("\nOutcome Distribution:")
for outcome in outcome_order:
    count = (patients['treatment_outcome'] == outcome).sum()
    print(f"  {outcome}: {count} ({count/len(patients)*100:.1f}%)")
print("=" * 60)
```

---

## Output Results

### Summary Statistics

```python
# Generate summary statistics for portfolio
import numpy as np
import pandas as pd

np.random.seed(42)

# Portfolio summary
portfolio = pd.DataFrame({
    'return_annual': np.random.normal(0.08, 0.15, 500) * 100,
    'risk_volatility': np.random.normal(0.15, 0.08, 500) * 100,
    'asset_class': np.random.choice(['Equity', 'Bonds', 'Commodities', 'Real Estate'], 500)
})

print("=" * 60)
print("STATISTICAL SUMMARY - PORTFOLIO ANALYSIS")
print("=" * 60)
for asset_class in portfolio['asset_class'].unique():
    subset = portfolio[portfolio['asset_class'] == asset_class]
    print(f"\n{asset_class}:")
    print(f"  Count: {len(subset)}")
    print(f"  Mean Return: {subset['return_annual'].mean():.2f}%")
    print(f"  Std Dev: {subset['return_annual'].std():.2f}%")
    print(f"  Mean Risk: {subset['risk_volatility'].mean():.2f}%")

# Clinical trials summary
trials = pd.DataFrame({
    'treatment': np.random.choice(['Placebo', 'Treatment A', 'Treatment B'], 600),
    'improvement': np.random.normal(10, 5, 600)
})

print("\n" + "=" * 60)
print("STATISTICAL SUMMARY - CLINICAL TRIALS")
print("=" * 60)
for treatment in trials['treatment'].unique():
    subset = trials[trials['treatment'] == treatment]
    print(f"\n{treatment}:")
    print(f"  Count: {len(subset)}")
    print(f"  Mean Improvement: {subset['improvement'].mean():.2f}")
    print(f"  95% CI: [{subset['improvement'].mean() - 1.96*subset['improvement'].std()/np.sqrt(len(subset)):.2f}, "
          f"{subset['improvement'].mean() + 1.96*subset['improvement'].std()/np.sqrt(len(subset)):.2f}]")

# Customer segmentation summary
customers = pd.DataFrame({
    'segment': np.random.choice(['Premium', 'Standard', 'Budget', 'Economy'], 1000),
    'annual_income': np.random.lognormal(10.5, 0.8, 1000),
    'spending_score': np.random.normal(50, 20, 1000)
})

print("\n" + "=" * 60)
print("STATISTICAL SUMMARY - CUSTOMER SEGMENTS")
print("=" * 60)
for segment in ['Premium', 'Standard', 'Economy', 'Budget']:
    subset = customers[customers['segment'] == segment]
    print(f"\n{segment}:")
    print(f"  Count: {len(subset)}")
    print(f"  Mean Income: ${subset['annual_income'].mean():,.0f}")
    print(f"  Mean Spending Score: {subset['spending_score'].mean():.1f}")
print("=" * 60)
```

---

## Visualization

### ASCII Statistical Visualizations

#### Distribution Plot

```
Age Distribution in Clinical Trial

Density
  0.04 |                    ___----___
  0.03 |              ____/           \____
  0.02 |          __--                   --__
  0.01 |        _-                           -_
  0.00 |______/                                 \______
        |-----|-----|-----|-----|-----|-----|-----|
              30     40     50     60     70     80
                              Age
                              
Mode: 50  |  Mean: 50.1  |  Std: 15.2
```

#### Box Plot Comparison

```
Treatment Comparison
Improvement
   25 |                    +-------+
   20 |          +-------+ |       |
   15 |    +-----+       | |       |
   10 |    |     |       | |       |
    5 |----+     +-------+-+       +-------+
    0 |                                 |
   -5 +-----+       +-------+       +---+ 
         A         B          C    
         
Box: IQR (25th-75th percentile)     +: Median
 Whiskers: 1.5*IQR
```

#### Violin Plot

```
Score Distribution by Segment
Density
  High +                +-------+
       |          +-----+-------+
       |      +---+           +---+
       |  +---+                   +---+
       +---+                       +---+
       | | |                     | | |
       Low                       High
       
       
   Budget    Economy    Standard   Premium
```

#### Heat Map

```
Correlation Heatmap

         Income  Age  Credit  DTI  Employ
Income    1.00  0.05  -0.12  0.34   0.15
Age       0.05  1.00   0.08  0.02   0.21
Credit   -0.12  0.08  1.00 -0.18   0.42
DTI       0.34  0.02 -0.18  1.00  -0.05
Employ    0.15  0.21  0.42 -0.05  1.00

Color: Red = Pos, Blue = Neg
```

---

## Advanced Topics

### Joint and Marginal Plots

```python
import seaborn as sns
import numpy as np
import matplotlib.pyplot as plt

# Joint plot with marginals
np.random.seed(42)
x = np.random.randn(200)
y = x + np.random.randn(200) * 0.5

# Create a DataFrame
df = pd.DataFrame({'x': x, 'y': y})

g = sns.jointplot(data=df, x='x', y='y', kind='reg', 
                height=8, ratio=4, color='#1f77b4')
g.fig.suptitle('Joint Plot with Regression', y=1.02)
plt.tight_layout()
plt.savefig('joint_plot.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Pair Plot for Multiple Variables

```python
import seaborn as sns
import numpy as np
import pandas as pd

# Generate multivariate data
np.random.seed(42)
n = 300
df = pd.DataFrame({
    'Age': np.random.normal(50, 15, n),
    'Income': np.random.lognormal(10.5, 0.8, n),
    'Score': np.random.normal(70, 15, n),
    'Category': np.random.choice(['A', 'B', 'C'], n)
})

g = sns.pairplot(df, hue='Category', diag_kind='kde', 
                palette='deep', height=2.5)
g.fig.suptitle('Pair Plot by Category', y=1.02)
plt.tight_layout()
plt.savefig('pair_plot.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Clustered Heatmap

```python
import seaborn as sns
import numpy as np
import pandas as pd

# Generate data for clustered heatmap
np.random.seed(42)
data = np.random.randn(20, 15)
data = pd.DataFrame(data, 
                  columns=[f'Feature_{i}' for i in range(15)],
                  index=[f'Sample_{i}' for i in range(20)])

# Cluster the heatmap
g = sns.clustermap(data, cmap='coolwarm', center=0, 
                  figsize=(12, 10))
g.fig.suptitle('Clustered Heatmap', y=1.02)
plt.tight_layout()
plt.savefig('clustered_heatmap.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Statistical Visualization Best Practices

```python
import seaborn as sns
import numpy as np

# Set professional style
sns.set_theme(style='whitegrid', font_scale=1.1)
sns.set_palette('deep')

# Example: Publication-quality plot
np.random.seed(42)
data = pd.DataFrame({
    'group': np.random.choice(['Control', 'Treatment'], 200),
    'value': np.concatenate([np.random.normal(50, 10, 100), 
                          np.random.normal(60, 12, 100)])
})

fig, ax = plt.subplots(figsize=(8, 6))

# Create publication-quality plot
sns.violinplot(data=data, x='group', y='value', ax=ax, 
              inner='box', palette='Set2')
sns.stripplot(data=data, x='group', y='value', ax=ax, 
            color='black', alpha=0.3, size=3)

ax.set_xlabel('Group', fontsize=12)
ax.set_ylabel('Value', fontsize=12)
ax.set_title('Treatment vs Control', fontsize=14, fontweight='bold')

# Add statistical annotation
ax.annotate('p < 0.001', xy=(0.5, 80), fontsize=10, 
           ha='center', fontfamily='sans-serif')

plt.tight_layout()
plt.savefig('publication_quality.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Conclusion

### Summary

This module covered statistical visualization with Seaborn:

1. **Fundamentals**: Seaborn architecture, plot types, themes
2. **Implementation**: Financial, clinical, customer, time series, correlation examples
3. **Applications**: Banking (credit risk) and healthcare (patient outcomes)
4. **Output Results**: Statistical summaries and confidence intervals
5. **Advanced Topics**: Joint plots, pair plots, clustered heatmaps

### Key Takeaways

- Seaborn simplifies statistical visualization with built-in statistical transformations
- Choose appropriate plot types for different data relationships
- Apply themes and palettes consistently for professional appearance
- Use confidence intervals to show statistical uncertainty

### Next Steps

Continue with:
- Module 03: Customizing Plots and Styling
- Module 04: Subplots and Figure Management
- Module 05: Interactive Plots and Annotations
- Module 06: Advanced Visualization Techniques

### Resources

- Seaborn Official Documentation: https://seaborn.pydata.org/
- Statistical Visualization Tutorial: https://seaborn.pydata.org/tutorial.html
- Gallery: https://seaborn.pydata.org/examples/index.html

---

*End of Module 02: Statistical Plots with Seaborn*