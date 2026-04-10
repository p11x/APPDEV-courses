# Topic: Time Series Visualization Advanced
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Time Series Visualization Advanced

I. INTRODUCTION
This module covers advanced time series visualization techniques including line charts,
area charts, candlestick charts, decomposition, rolling statistics,
and interactive time series visualizations.

II. CORE CONCEPTS
- Time series fundamentals
- Trend, seasonality, and noise decomposition
- Rolling windows and moving averages
- Lag plots and autocorrelation

III. IMPLEMENTATION
- matplotlib for static visualizations
- seaborn for statistical time series
- plotly for interactive charts

IV. EXAMPLES
- Banking: Stock prices, interest rate trends
- Healthcare: Patient admission trends, disease patterns

V. OUTPUT RESULTS
- Time series charts with annotations

VI. TESTING
- Unit tests for time functions

VII. ADVANCED TOPICS
- Forecasting visualizations
- Anomaly detection

VIII. CONCLUSION
Best practices for time series visualization
"""

import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
import pandas as pd
import plotly.express as px
import plotly.graph_objects as go
from datetime import datetime, timedelta
import warnings
warnings.filterwarnings('ignore')

plt.style.use('seaborn-v0_8-whitegrid')


def generate_time_series_data(n_days=365, seed=42):
    """Generate sample time series data."""
    np.random.seed(seed)
    
    dates = pd.date_range(start='2022-01-01', periods=n_days, freq='D')
    
    data = pd.DataFrame({
        'Date': dates,
        'Value': np.cumsum(np.random.randn(n_days) * 50 + 100),
        'Volume': np.random.randint(100000, 1000000, n_days),
        'Category': np.random.choice(['A', 'B', 'C'], n_days)
    })
    
    trend = np.linspace(0, 200, n_days)
    data['Value'] = data['Value'] + trend
    
    return data


def generate_stock_data(n_days=252, seed=42):
    """Generate stock price data."""
    np.random.seed(seed)
    
    dates = pd.date_range(start='2022-01-01', periods=n_days, freq='B')
    
    prices = 100
    prices_list = [prices]
    for _ in range(n_days - 1):
        change = np.random.randn() * 2
        prices = max(prices + change, 10)
        prices_list.append(prices)
    
    data = pd.DataFrame({
        'Date': dates,
        'Open': prices_list,
        'High': [p + abs(np.random.randn() * 2) for p in prices_list],
        'Low': [p - abs(np.random.randn() * 2) for p in prices_list],
        'Close': [p + np.random.randn() * 1.5 for p in prices_list],
        'Volume': np.random.randint(1000000, 10000000, n_days)
    })
    
    return data


def generate_multi_time_series(n_days=365, seed=42):
    """Generate multiple time series data."""
    np.random.seed(seed)
    
    dates = pd.date_range(start='2022-01-01', periods=n_days, freq='D')
    
    n_series = 4
    data = {'Date': dates}
    
    for i in range(n_series):
        base = 100 * (i + 1)
        values = np.cumsum(np.random.randn(n_days) * 20 + base)
        data[f'Series_{i+1}'] = values
    
    return pd.DataFrame(data)


def create_time_series_line(data, x_col='Date', y_col='Value', title='Time Series'):
    """Create basic time series line chart."""
    fig, ax = plt.subplots(figsize=(14, 6))
    
    ax.plot(data[x_col], data[y_col], linewidth=1.5, color='steelblue')
    ax.fill_between(data[x_col], data[y_col], alpha=0.3, color='steelblue')
    
    ax.set_xlabel('Date', fontsize=12, fontweight='bold')
    ax.set_ylabel(y_col, fontsize=12, fontweight='bold')
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3)
    
    mean_val = data[y_col].mean()
    ax.axhline(mean_val, color='red', linestyle='--', linewidth=1.5, label=f'Mean: {mean_val:.1f}')
    ax.legend()
    
    plt.xticks(rotation=45)
    plt.tight_layout()
    
    return fig


def create_multi_line_series(data, x_col='Date', title='Multiple Time Series'):
    """Create multiple time series line chart."""
    fig, ax = plt.subplots(figsize=(14, 6))
    
    y_cols = [col for col in data.columns if col != x_col]
    colors = ['steelblue', 'red', 'green', 'orange']
    
    for i, col in enumerate(y_cols):
        ax.plot(data[x_col], data[col], linewidth=1.5, label=col, color=colors[i])
    
    ax.set_xlabel('Date', fontsize=12, fontweight='bold')
    ax.set_ylabel('Value', fontsize=12, fontweight='bold')
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.legend(loc='upper left')
    ax.grid(True, alpha=0.3)
    
    plt.xticks(rotation=45)
    plt.tight_layout()
    
    return fig


def create_candlestick_chart(data, title='Candlestick Chart'):
    """Create candlestick chart for stock data."""
    fig = go.Figure(data=[go.Candlestick(
        x=data['Date'],
        open=data['Open'],
        high=data['High'],
        low=data['Low'],
        close=data['Close']
    )])
    
    fig.update_layout(
        title=title,
        xaxis_title='Date',
        yaxis_title='Price',
        template='plotly_white'
    )
    
    return fig


def create_area_chart(data, x_col='Date', y_col='Value', title='Area Chart'):
    """Create area chart."""
    fig = px.area(data, x=x_col, y=y_col, title=title)
    
    fig.update_layout(
        xaxis_title='Date',
        yaxis_title=y_col,
        template='plotly_white'
    )
    
    return fig


def create_rolling_statistics(data, y_col='Value', window=30, title='Rolling Statistics'):
    """Create rolling mean and std visualization."""
    fig, ax = plt.subplots(figsize=(14, 6))
    
    rolling_mean = data[y_col].rolling(window=window).mean()
    rolling_std = data[y_col].rolling(window=window).std()
    
    ax.plot(data['Date'], data[y_col], alpha=0.3, color='gray', label='Original')
    ax.plot(data['Date'], rolling_mean, color='red', linewidth=2, label=f'Rolling Mean ({window})')
    ax.fill_between(
        data['Date'],
        rolling_mean - rolling_std,
        rolling_mean + rolling_std,
        alpha=0.2, color='red', label=f'±1 Std'
    )
    
    ax.set_xlabel('Date', fontsize=12, fontweight='bold')
    ax.set_ylabel(y_col, fontsize=12, fontweight='bold')
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.legend()
    ax.grid(True, alpha=0.3)
    
    plt.xticks(rotation=45)
    plt.tight_layout()
    
    return fig


def create_decomposition_plot(data, y_col='Value', title='Time Series Decomposition'):
    """Create time series decomposition plot."""
    from statsmodels.tsa.seasonal import seasonal_decompose
    
    result = seasonal_decompose(data[y_col], model='additive', period=30)
    
    fig, axes = plt.subplots(4, 1, figsize=(14, 12))
    
    axes[0].plot(data['Date'], result.observed, color='steelblue')
    axes[0].set_ylabel('Observed')
    axes[0].set_title(title, fontsize=14)
    axes[0].grid(True, alpha=0.3)
    
    axes[1].plot(data['Date'], result.trend, color='red')
    axes[1].set_ylabel('Trend')
    axes[1].grid(True, alpha=0.3)
    
    axes[2].plot(data['Date'], result.seasonal, color='green')
    axes[2].set_ylabel('Seasonal')
    axes[2].grid(True, alpha=0.3)
    
    axes[3].plot(data['Date'], result.resid, color='purple')
    axes[3].set_ylabel('Residual')
    axes[3].grid(True, alpha=0.3)
    
    for ax in axes:
        ax.tick_params(axis='x', rotation=45)
    
    plt.tight_layout()
    
    return fig


def create_lag_plot(data, y_col='Value', lags=5, title='Lag Plots'):
    """Create lag plots for time series."""
    fig, axes = plt.subplots(1, lags, figsize=(15, 3))
    
    for i in range(lags):
        ax = axes[i]
        ax.scatter(data[y_col].shift(i+1), data[y_col], alpha=0.5, s=10)
        ax.set_xlabel(f'Lag {i+1}')
        ax.set_ylabel(y_col if i == 0 else '')
        ax.grid(True, alpha=0.3)
    
    fig.suptitle(title, fontsize=14, fontweight='bold')
    plt.tight_layout()
    
    return fig


def create_autocorrelation_plot(data, y_col='Value', title='Autocorrelation'):
    """Create autocorrelation plot."""
    from statsmodels.tsa.stattools import acf
    
    fig, ax = plt.subplots(figsize=(12, 6))
    
    acf_values = acf(data[y_col], nlags=40)
    
    ax.bar(range(len(acf_values)), acf_values, color='steelblue', alpha=0.7)
    ax.axhline(y=0, color='black', linewidth=1)
    ax.axhline(y=1.96/np.sqrt(len(data)), color='red', linestyle='--', linewidth=1)
    ax.axhline(y=-1.96/np.sqrt(len(data)), color='red', linestyle='--', linewidth=1)
    
    ax.set_xlabel('Lag')
    ax.set_ylabel('Autocorrelation')
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    return fig


def create_seasonal_boxplot(data, y_col='Value', period='M', title='Seasonal Boxplot'):
    """Create seasonal boxplot."""
    data_copy = data.copy()
    data_copy['Month'] = data_copy['Date'].dt.month
    data_copy['Year'] = data_copy['Date'].dt.year
    
    fig, ax = plt.subplots(figsize=(14, 6))
    
    data_copy.boxplot(column=y_col, by='Month', ax=ax)
    
    ax.set_xlabel('Month', fontsize=12)
    ax.set_ylabel(y_col, fontsize=12, fontweight='bold')
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3)
    
    plt.suptitle('')
    plt.tight_layout()
    
    return fig


def create_heatmap_calendar(data, y_col='Value', title='Calendar Heatmap'):
    """Create calendar heatmap for time series."""
    data_copy = data.copy()
    data_copy['Month'] = data_copy['Date'].dt.month
    data_copy['Day'] = data_copy['Date'].dt.day
    data_copy['Week'] = data_copy['Date'].dt.isocalendar().week
    
    pivot = data_copy.pivot_table(values=y_col, index='Month', columns='Week', aggfunc='mean')
    
    fig, ax = plt.subplots(figsize=(14, 6))
    
    sns.heatmap(pivot, cmap='YlOrRd', ax=ax, cbar_kws={'label': y_col})
    
    ax.set_xlabel('Week', fontsize=12)
    ax.set_ylabel('Month', fontsize=12)
    ax.set_title(title, fontsize=14, fontweight='bold')
    
    plt.tight_layout()
    
    return fig


def create_volatility_chart(data, window=30, title='Volatility Chart'):
    """Create volatility chart."""
    returns = data['Close'].pct_change()
    volatility = returns.rolling(window=window).std() * np.sqrt(252)
    
    fig, ax = plt.subplots(figsize=(14, 6))
    
    ax.plot(data['Date'], volatility, color='red', linewidth=1.5)
    ax.fill_between(data['Date'], volatility, alpha=0.3, color='red')
    
    ax.set_xlabel('Date', fontsize=12, fontweight='bold')
    ax.set_ylabel('Volatility', fontsize=12, fontweight='bold')
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3)
    
    plt.xticks(rotation=45)
    plt.tight_layout()
    
    return fig


def core_implementation():
    """Core time series visualization implementation."""
    data = generate_time_series_data(365)
    stock_data = generate_stock_data(252)
    multi_data = generate_multi_time_series(365)
    
    results = {}
    
    results['line'] = create_time_series_line(data, 'Date', 'Value', 'Time Series Line')
    results['multi_line'] = create_multi_line_series(multi_data, 'Date', 'Multiple Series')
    results['candlestick'] = create_candlestick_chart(stock_data, 'Stock Candlestick')
    results['area'] = create_area_chart(data[:50], 'Date', 'Value', 'Area Chart')
    results['rolling'] = create_rolling_statistics(data, 'Value', 30, 'Rolling Statistics')
    results['decomposition'] = create_decomposition_plot(data, 'Value')
    
    return results


def banking_example():
    """Banking/Finance application - Financial Time Series."""
    np.random.seed(123)
    
    dates = pd.date_range(start='2020-01-01', periods=1000, freq='D')
    
    interest_rates = 2 + np.cumsum(np.random.randn(1000) * 0.05)
    interest_rates = interest_rates.clip(0.5, 8)
    
    bank_data = pd.DataFrame({
        'Date': dates,
        'Interest_Rate': interest_rates,
        'Loan_Applications': np.random.randint(100, 1000, 1000),
        'Approved_Loans': np.random.randint(50, 500, 1000),
        'Default_Rate': np.random.uniform(1, 10, 1000),
        'Stock_Price': 100 + np.cumsum(np.random.randn(1000) * 2),
        'Deposits': np.cumsum(np.random.randn(1000) * 1000000 + 5000000),
        'Withdrawals': np.cumsum(np.random.randn(1000) * 800000 + 4000000)
    })
    
    bank_data['Approval_Rate'] = (bank_data['Approved_Loans'] / bank_data['Loan_Applications']) * 100
    bank_data['Net_Flow'] = bank_data['Deposits'] - bank_data['Withdrawals']
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1)
    ax1.plot(bank_data['Date'], bank_data['Interest_Rate'], color='red', linewidth=1.5)
    ax1.axhline(bank_data['Interest_Rate'].mean(), color='blue', linestyle='--',
                label=f"Mean: {bank_data['Interest_Rate'].mean():.2f}%")
    ax1.set_xlabel('Date')
    ax1.set_ylabel('Interest Rate (%)')
    ax1.set_title('Interest Rate Trend')
    ax1.legend()
    ax1.grid(True, alpha=0.3)
    ax1.tick_params(axis='x', rotation=45)
    
    ax2 = fig.add_subplot(2, 3, 2)
    ax2.plot(bank_data['Date'], bank_data['Stock_Price'], color='green', linewidth=1.5)
    ax2.fill_between(bank_data['Date'], bank_data['Stock_Price'], alpha=0.3, color='green')
    ax2.set_xlabel('Date')
    ax2.set_ylabel('Stock Price ($)')
    ax2.set_title('Bank Stock Price')
    ax2.grid(True, alpha=0.3)
    ax2.tick_params(axis='x', rotation=45)
    
    ax3 = fig.add_subplot(2, 3, 3)
    ax3.plot(bank_data['Date'], bank_data['Net_Flow'] / 1000000, color='blue', linewidth=1.5)
    ax3.axhline(0, color='red', linestyle='--', linewidth=1)
    ax3.set_xlabel('Date')
    ax3.set_ylabel('Net Flow (Millions $)')
    ax3.set_title('Deposits - Withdrawals')
    ax3.grid(True, alpha=0.3)
    ax3.tick_params(axis='x', rotation=45)
    
    ax4 = fig.add_subplot(2, 3, 4)
    rolling = bank_data['Loan_Applications'].rolling(30).mean()
    ax4.plot(bank_data['Date'], bank_data['Loan_Applications'], alpha=0.3, color='gray')
    ax4.plot(bank_data['Date'], rolling, color='red', linewidth=2, label='30-day MA')
    ax4.set_xlabel('Date')
    ax4.set_ylabel('Loan Applications')
    ax4.set_title('Loan Applications Trend')
    ax4.legend()
    ax4.grid(True, alpha=0.3)
    ax4.tick_params(axis='x', rotation=45)
    
    ax5 = fig.add_subplot(2, 3, 5)
    ax5.fill_between(bank_data['Date'], bank_data['Approval_Rate'], alpha=0.5, color='green')
    ax5.set_xlabel('Date')
    ax5.set_ylabel('Approval Rate (%)')
    ax5.set_title('Loan Approval Rate')
    ax5.grid(True, alpha=0.3)
    ax5.tick_params(axis='x', rotation=45)
    
    ax6 = fig.add_subplot(2, 3, 6)
    ax6.plot(bank_data['Date'], bank_data['Default_Rate'], color='red', linewidth=1.5)
    ax6.axhline(5, color='orange', linestyle='--', label='Warning (5%)')
    ax6.set_xlabel('Date')
    ax6.set_ylabel('Default Rate (%)')
    ax6.set_title('Default Rate Over Time')
    ax6.legend()
    ax6.grid(True, alpha=0.3)
    ax6.tick_params(axis='x', rotation=45)
    
    plt.tight_layout()
    
    print("=" * 60)
    print("FINANCIAL TIME SERIES ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Days: {len(bank_data)}")
    print(f"Average Interest Rate: {bank_data['Interest_Rate'].mean():.2f}%")
    print(f"Average Stock Price: ${bank_data['Stock_Price'].mean():.2f}")
    print(f"Average Approval Rate: {bank_data['Approval_Rate'].mean():.1f}%")
    print(f"Average Default Rate: {bank_data['Default_Rate'].mean():.2f}%")
    print(f"\nTotal Deposits: ${bank_data['Deposits'].iloc[-1]:,.0f}")
    print(f"Total Withdrawals: ${bank_data['Withdrawals'].iloc[-1]:,.0f}")
    
    return fig, bank_data


def healthcare_example():
    """Healthcare application - Patient Admission Trends."""
    np.random.seed(456)
    
    dates = pd.date_range(start='2020-01-01', periods=730, freq='D')
    
    seasonal_pattern = 50 * np.sin(2 * np.pi * np.arange(730) / 365)
    trend = np.linspace(0, 30, 730)
    
    healthcare_data = pd.DataFrame({
        'Date': dates,
        'Admissions': 200 + seasonal_pattern + trend + np.random.randn(730) * 20,
        'Discharges': 190 + seasonal_pattern + trend + np.random.randn(730) * 20,
        'ER_Waits': np.random.normal(45, 15, 730),
        'Bed_Occupancy': np.random.uniform(60, 95, 730),
        'Readmissions': np.random.poisson(10, 730),
        'Satisfaction': np.random.normal(7.5, 1, 730),
        'Staff_On_Duty': np.random.randint(50, 150, 730)
    })
    
    healthcare_data['Admissions'] = healthcare_data['Admissions'].clip(100, 350).astype(int)
    healthcare_data['Discharges'] = healthcare_data['Discharges'].clip(100, 350).astype(int)
    healthcare_data['ER_Waits'] = healthcare_data['ER_Waits'].clip(10, 120)
    healthcare_data['Satisfaction'] = healthcare_data['Satisfaction'].clip(1, 10)
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1)
    ax1.plot(healthcare_data['Date'], healthcare_data['Admissions'], color='blue', alpha=0.5)
    rolling = healthcare_data['Admissions'].rolling(30).mean()
    ax1.plot(healthcare_data['Date'], rolling, color='red', linewidth=2, label='30-day MA')
    ax1.set_xlabel('Date')
    ax1.set_ylabel('Admissions')
    ax1.set_title('Patient Admissions Trend')
    ax1.legend()
    ax1.grid(True, alpha=0.3)
    ax1.tick_params(axis='x', rotation=45)
    
    ax2 = fig.add_subplot(2, 3, 2)
    ax2.plot(healthcare_data['Date'], healthcare_data['Discharges'], color='green', alpha=0.5)
    rolling_d = healthcare_data['Discharges'].rolling(30).mean()
    ax2.plot(healthcare_data['Date'], rolling_d, color='red', linewidth=2, label='30-day MA')
    ax2.set_xlabel('Date')
    ax2.set_ylabel('Discharges')
    ax2.set_title('Patient Discharges Trend')
    ax2.legend()
    ax2.grid(True, alpha=0.3)
    ax2.tick_params(axis='x', rotation=45)
    
    ax3 = fig.add_subplot(2, 3, 3)
    ax3.plot(healthcare_data['Date'], healthcare_data['ER_Waits'], color='orange', linewidth=1.5)
    ax3.axhline(60, color='red', linestyle='--', label='Target (60 min)')
    ax3.set_xlabel('Date')
    ax3.set_ylabel('Wait Time (minutes)')
    ax3.set_title('ER Wait Times')
    ax3.legend()
    ax3.grid(True, alpha=0.3)
    ax3.tick_params(axis='x', rotation=45)
    
    ax4 = fig.add_subplot(2, 3, 4)
    ax4.fill_between(healthcare_data['Date'], healthcare_data['Bed_Occupancy'], alpha=0.5, color='purple')
    ax4.axhline(85, color='red', linestyle='--', label='Critical (85%)')
    ax4.set_xlabel('Date')
    ax4.set_ylabel('Bed Occupancy (%)')
    ax4.set_title('Bed Occupancy Rate')
    ax4.legend()
    ax4.grid(True, alpha=0.3)
    ax4.tick_params(axis='x', rotation=45)
    
    ax5 = fig.add_subplot(2, 3, 5)
    ax5.plot(healthcare_data['Date'], healthcare_data['Readmissions'], color='red', markersize=3, alpha=0.5)
    rolling_r = healthcare_data['Readmissions'].rolling(30).mean()
    ax5.plot(healthcare_data['Date'], rolling_r, color='blue', linewidth=2, label='30-day MA')
    ax5.set_xlabel('Date')
    ax5.set_ylabel('Readmissions')
    ax5.set_title('30-Day Readmissions')
    ax5.legend()
    ax5.grid(True, alpha=0.3)
    ax5.tick_params(axis='x', rotation=45)
    
    ax6 = fig.add_subplot(2, 3, 6)
    ax6.plot(healthcare_data['Date'], healthcare_data['Satisfaction'], color='green', linewidth=1.5)
    ax6.axhline(7, color='orange', linestyle='--', label='Good (7.0)')
    ax6.set_xlabel('Date')
    ax6.set_ylabel('Satisfaction Score')
    ax6.set_title('Patient Satisfaction')
    ax6.legend()
    ax6.grid(True, alpha=0.3)
    ax6.tick_params(axis='x', rotation=45)
    
    plt.tight_layout()
    
    print("=" * 60)
    print("HEALTHCARE TIME SERIES ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Days: {len(healthcare_data)}")
    print(f"Average Daily Admissions: {healthcare_data['Admissions'].mean():.0f}")
    print(f"Average ER Wait: {healthcare_data['ER_Waits'].mean():.1f} minutes")
    print(f"Average Bed Occupancy: {healthcare_data['Bed_Occupancy'].mean():.1f}%")
    print(f"Average Readmissions: {healthcare_data['Readmissions'].mean():.1f}")
    print(f"Average Satisfaction: {healthcare_data['Satisfaction'].mean():.1f}/10")
    
    return fig, healthcare_data


def main():
    """Main execution function."""
    print("Executing Time Series Visualization Advanced")
    print("=" * 60)
    
    data = generate_time_series_data(365)
    stock_data = generate_stock_data(252)
    multi_data = generate_multi_time_series(365)
    
    create_time_series_line(data, 'Date', 'Value', 'Time Series')
    create_multi_line_series(multi_data, 'Date')
    create_candlestick_chart(stock_data)
    create_area_chart(data[:50])
    create_rolling_statistics(data, 'Value', 30)
    create_decomposition_plot(data, 'Value')
    
    plt.show()
    
    return data


if __name__ == "__main__":
    main()