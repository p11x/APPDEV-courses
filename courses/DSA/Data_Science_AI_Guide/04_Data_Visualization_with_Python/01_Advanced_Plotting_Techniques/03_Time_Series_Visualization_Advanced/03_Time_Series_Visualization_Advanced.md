# Time Series Visualization Advanced

## I. INTRODUCTION

### What is Time Series Visualization?

Time series visualization is the graphical representation of data points indexed by time. Unlike static charts, time series visualizations show how data evolves over time, revealing trends, cyclical patterns, seasonality, and anomalies that are fundamental to understanding temporal data.

Advanced time series visualization encompasses techniques beyond basic line charts, including multi-series visualizations, candle stick charts for financial data, decomposition views showing trend/seasonality/residual components, interactive time-series exploration, and animated temporal visualizations.

Time series data is ubiquitous across domains - stock prices, temperature readings, sales figures, website traffic, sensor measurements, and economic indicators all represent temporal phenomena requiring sophisticated visualization techniques.

### Why is Time Series Visualization Important?

1. **Trend Identification**: Visual representations make it easy to identify long-term trends that might be obscured in raw data tables.

2. **Seasonality Detection**: Cyclical patterns become visible through time series visualization, essential for demand planning and forecasting.

3. **Anomaly Detection**: Unusual spikes or drops stand out in well-designed visualizations, enabling quick identification of issues.

4. **Comparison Across Time Periods**: Year-over-year or period-over-period comparisons become intuitive.

5. **Forecasting Context**: Understanding historical patterns informs forecasting model selection and validation.

### Prerequisites

- Python programming fundamentals
- Understanding of pandas datetime functionality
- Basic statistics knowledge (mean, variance, trend)
- Familiarity with matplotlib, seaborn, and plotly
- Libraries needed: pandas, numpy, matplotlib, seaborn, plotly, statsmodels

## II. FUNDAMENTALS

### Basic Concepts and Definitions

**Time Series**: A sequence of data points indexed by time, typically at equally spaced intervals.

**Trend**: The long-term movement in the data, showing overall increase or decrease over time.

**Seasonality**: Regular, repeating patterns at fixed intervals (daily, monthly, yearly).

**Cyclical Patterns**: Larger-scale oscillations not following fixed calendars (business cycles).

**Noise**: Random variations that don't follow patterns.

**Stationarity**: A time series whose statistical properties don't change over time.

### Key Terminology

| Term | Definition |
|------|------------|
| Datetime Index | pandas index using datetime objects |
| Resampling | Converting data to different time frequency |
| Rolling Window | Fixed-size window for calculating rolling statistics |
| Lag | Previous time period used for comparison |
| YOY | Year-Over-Year comparison |
| CAGR | Compound Annual Growth Rate |
| ATR | Average True Range (volatility measure) |

### Core Principles

1. **Order Matters**: Time series should be displayed chronologically, typically left to right.

2. **Consistent Time Intervals**: Ensure equal spacing between data points for accurate comparison.

3. **Appropriate Time Scale**: Match the visualization time scale to the analytical question.

4. **Show Uncertainty**: Include confidence intervals when showing forecasts.

5. **Interactive Exploration**: Enable zooming and panning for detailed analysis.

## III. IMPLEMENTATION

### Step-by-Step Code Examples

#### Setup and Data Creation

```python
# Import all necessary libraries
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.dates as mdates
import seaborn as sns
import plotly.express as px
import plotly.graph_objects as go
from plotly.subplots import make_subplots
from datetime import datetime, timedelta
import warnings
warnings.filterwarnings('ignore')

# Set visualization style
plt.style.use('seaborn-v0_8-whitegrid')
sns.set_palette("husl")
```

#### Creating Basic Time Series Line Charts

```python
def create_basic_time_series():
    """
    Create basic time series line charts showing data over time.
    This is the foundation for more advanced visualizations.
    """
    # Generate sample time series data
    np.random.seed(42)
    
    # Create date range - daily data for 3 years
    dates = pd.date_range(start='2020-01-01', periods=1095, freq='D')
    
    # Create synthetic time series with trend, seasonality, and noise
    trend = np.linspace(100, 200, 1095)  # Upward trend
    seasonality = 20 * np.sin(np.linspace(0, 20 * np.pi, 1095))  # Seasonal pattern
    noise = np.random.normal(0, 5, 1095)  # Random noise
    
    # Combine components
    values = trend + seasonality + noise
    
    # Create DataFrame
    df = pd.DataFrame({
        'Date': dates,
        'Value': values,
        'Category': np.random.choice(['Product A', 'Product B', 'Product C'], 1095)
    })
    
    # Create figure with figure size
    fig, ax = plt.subplots(figsize=(14, 6))
    
    # Plot the time series
    ax.plot(df['Date'], df['Value'], color='steelblue', linewidth=1.5, alpha=0.8)
    
    # Fill under the line for visual emphasis
    ax.fill_between(df['Date'], df['Value'], alpha=0.3, color='steelblue')
    
    # Add mean line
    mean_value = df['Value'].mean()
    ax.axhline(y=mean_value, color='red', linestyle='--', linewidth=1.5, 
               label=f'Mean: {mean_value:.1f}')
    
    # Format x-axis dates
    ax.xaxis.set_major_locator(mdates.MonthLocator(interval=3))
    ax.xaxis.set_major_formatter(mdates.DateFormatter('%Y-%m'))
    plt.xticks(rotation=45)
    
    # Add labels and title
    ax.set_xlabel('Date', fontsize=12, fontweight='bold')
    ax.set_ylabel('Value', fontsize=12, fontweight='bold')
    ax.set_title('Basic Time Series Visualization', fontsize=14, fontweight='bold')
    
    # Add legend
    ax.legend(loc='upper left')
    
    # Grid styling
    ax.grid(True, alpha=0.3, linestyle='-')
    
    plt.tight_layout()
    
    return fig, df

fig_ts, df_ts = create_basic_time_series()
plt.show()
```

#### Creating Multi-Series Time Series Visualization

```python
def create_multi_series_time_series():
    """
    Create time series with multiple series for comparison.
    Essential for comparing multiple metrics or categories over time.
    """
    np.random.seed(42)
    
    # Create date range - monthly data for 4 years
    dates = pd.date_range(start='2019-01-01', periods=48, freq='M')
    
    # Generate data for three categories with different patterns
    data = pd.DataFrame({
        'Date': dates,
        'Product_A': 100 + np.cumsum(np.random.normal(2, 5, 48)),
        'Product_B': 150 + np.cumsum(np.random.normal(1.5, 4, 48)),
        'Product_C': 80 + np.cumsum(np.random.normal(3, 6, 48))
    })
    
    # Add some seasonality
    seasonal = 10 * np.sin(np.linspace(0, 8 * np.pi, 48))
    data['Product_A'] += seasonal
    data['Product_B'] -= seasonal * 0.5
    data['Product_C'] += seasonal * 1.5
    
    # Melt to long format for easier plotting
    data_long = data.melt(id_vars=['Date'], var_name='Product', value_name='Sales')
    
    # Create figure
    fig, axes = plt.subplots(2, 1, figsize=(14, 10))
    
    # Top plot: All series on same chart
    ax1 = axes[0]
    colors = {'Product_A': 'blue', 'Product_B': 'orange', 'Product_C': 'green'}
    
    for product in ['Product_A', 'Product_B', 'Product_C']:
        ax1.plot(data['Date'], data[product], 
                label=product, linewidth=2, color=colors[product])
    
    ax1.set_xlabel('Date', fontsize=11)
    ax1.set_ylabel('Sales', fontsize=11)
    ax1.set_title('Multi-Series Time Series Comparison', fontsize=13, fontweight='bold')
    ax1.legend(loc='upper left')
    ax1.grid(True, alpha=0.3)
    
    # Format dates
    ax1.xaxis.set_major_formatter(mdates.DateFormatter('%Y-%m'))
    plt.setp(ax1.xaxis.get_majorticklabels(), rotation=45)
    
    # Bottom plot: Normalized comparison
    ax2 = axes[1]
    
    # Normalize each series to start at 100
    for product in ['Product_A', 'Product_B', 'Product_C']:
        normalized = (data[product] / data[product].iloc[0]) * 100
        ax2.plot(data['Date'], normalized, 
                label=product, linewidth=2, color=colors[product])
    
    ax2.axhline(y=100, color='gray', linestyle='--', linewidth=1)
    ax2.set_xlabel('Date', fontsize=11)
    ax2.set_ylabel('Normalized Sales (Base=100)', fontsize=11)
    ax2.set_title('Normalized Time Series (Base = 100)', fontsize=13, fontweight='bold')
    ax2.legend(loc='upper left')
    ax2.grid(True, alpha=0.3)
    
    ax2.xaxis.set_major_formatter(mdates.DateFormatter('%Y-%m'))
    plt.setp(ax2.xaxis.get_majorticklabels(), rotation=45)
    
    plt.tight_layout()
    
    return fig, data

fig_multi, df_multi = create_multi_series_time_series()
plt.show()
```

#### Creating Rolling Statistics Visualization

```python
def create_rolling_statistics_viz():
    """
    Create visualization showing rolling statistics.
    Essential for understanding trends and volatility over time.
    """
    np.random.seed(42)
    
    # Generate daily stock price-like data
    dates = pd.date_range(start='2021-01-01', periods=252, freq='D')
    
    # Simulate stock prices with random walk
    returns = np.random.normal(0.001, 0.02, 252)
    prices = 100 * np.cumprod(1 + returns)
    
    df = pd.DataFrame({
        'Date': dates,
        'Price': prices
    })
    
    # Calculate rolling statistics
    df['MA_7'] = df['Price'].rolling(window=7).mean()
    df['MA_21'] = df['Price'].rolling(window=21).mean()
    df['MA_63'] = df['Price'].rolling(window=63).mean()
    
    # Calculate rolling standard deviation
    df['Rolling_Std_21'] = df['Price'].rolling(window=21).std()
    
    # Calculate Bollinger Bands
    df['Upper_Band'] = df['MA_21'] + 2 * df['Rolling_Std_21']
    df['Lower_Band'] = df['MA_21'] - 2 * df['Rolling_Std_21']
    
    # Create figure
    fig, axes = plt.subplots(2, 1, figsize=(14, 10), gridspec_kw={'height_ratios': [3, 1]})
    
    # Top plot: Price with moving averages and Bollinger Bands
    ax1 = axes[0]
    
    # Plot Bollinger Bands
    ax1.fill_between(df['Date'], df['Lower_Band'], df['Upper_Band'], 
                     alpha=0.2, color='gray', label='Bollinger Bands (2σ)')
    
    # Plot price
    ax1.plot(df['Date'], df['Price'], color='blue', linewidth=1.5, 
             label='Daily Price', alpha=0.8)
    
    # Plot moving averages
    ax1.plot(df['Date'], df['MA_7'], color='orange', linewidth=1.5, 
             label='7-Day MA', linestyle='--')
    ax1.plot(df['Date'], df['MA_21'], color='red', linewidth=2, 
             label='21-Day MA')
    ax1.plot(df['Date'], df['MA_63'], color='green', linewidth=2, 
             label='63-Day MA')
    
    ax1.set_xlabel('Date', fontsize=11)
    ax1.set_ylabel('Price ($)', fontsize=11)
    ax1.set_title('Stock Price with Moving Averages and Bollinger Bands', 
                 fontsize=13, fontweight='bold')
    ax1.legend(loc='upper left', fontsize=9)
    ax1.grid(True, alpha=0.3)
    
    # Format dates
    ax1.xaxis.set_major_formatter(mdates.DateFormatter('%Y-%m'))
    plt.setp(ax1.xaxis.get_majorticklabels(), rotation=45)
    
    # Bottom plot: Rolling volatility
    ax2 = axes[1]
    
    ax2.fill_between(df['Date'], df['Rolling_Std_21'], alpha=0.5, color='red')
    ax2.plot(df['Date'], df['Rolling_Std_21'], color='darkred', linewidth=1.5)
    
    ax2.set_xlabel('Date', fontsize=11)
    ax2.set_ylabel('21-Day Rolling Std', fontsize=11)
    ax2.set_title('Rolling Volatility (21-Day Standard Deviation)', 
                  fontsize=13, fontweight='bold')
    ax2.grid(True, alpha=0.3)
    
    ax2.xaxis.set_major_formatter(mdates.DateFormatter('%Y-%m'))
    plt.setp(ax2.xaxis.get_majorticklabels(), rotation=45)
    
    plt.tight_layout()
    
    return fig, df

fig_roll, df_roll = create_rolling_statistics_viz()
plt.show()
```

#### Creating Candlestick Chart (Financial)

```python
def create_candlestick_chart():
    """
    Create candlestick chart for financial OHLC data.
    Widely used for analyzing price movements.
    """
    np.random.seed(42)
    
    # Generate OHLC (Open, High, Low, Close) data
    dates = pd.date_range(start='2022-01-01', periods=60, freq='D')
    
    # Simulate price movements
    close = 100
    opens = []
    highs = []
    lows = []
    closes = []
    
    for i in range(60):
        open_price = close + np.random.normal(0, 1)
        change = np.random.normal(0, 2)
        close_price = open_price + change
        high_price = max(open_price, close_price) + abs(np.random.normal(0, 0.5))
        low_price = min(open_price, close_price) - abs(np.random.normal(0, 0.5))
        
        opens.append(open_price)
        closes.append(close_price)
        highs.append(high_price)
        lows.append(low_price)
        close = close_price
    
    df = pd.DataFrame({
        'Date': dates,
        'Open': opens,
        'High': highs,
        'Low': lows,
        'Close': closes
    })
    
    # Determine if day is up or down
    df['Increase'] = df['Close'] > df['Open']
    
    # Create Plotly candlestick chart
    fig = go.Figure(data=[
        go.Candlestick(
            x=df['Date'],
            open=df['Open'],
            high=df['High'],
            low=df['Low'],
            close=df['Close'],
            increasing_line_color='green',
            decreasing_line_color='red',
            increasing_fillcolor='green',
            decreasing_fillcolor='red'
        )
    ])
    
    # Update layout
    fig.update_layout(
        title='Candlestick Chart - Price Movement',
        yaxis_title='Price ($)',
        xaxis_title='Date',
        xaxis_rangeslider_visible=False,
        height=500
    )
    
    # Add some moving averages
    df['MA_5'] = df['Close'].rolling(5).mean()
    df['MA_10'] = df['Close'].rolling(10).mean()
    
    fig.add_trace(go.Scatter(
        x=df['Date'], y=df['MA_5'],
        mode='lines',
        name='5-Day MA',
        line=dict(color='orange', width=1)
    ))
    
    fig.add_trace(go.Scatter(
        x=df['Date'], y=df['MA_10'],
        mode='lines',
        name='10-Day MA',
        line=dict(color='blue', width=1)
    ))
    
    return fig, df

fig_candle, df_candle = create_candlestick_chart()
fig_candle.show()
```

#### Creating Seasonal Decomposition

```python
def create_seasonal_decomposition():
    """
    Create seasonal decomposition visualization.
    Shows trend, seasonality, and residuals separately.
    """
    np.random.seed(42)
    
    # Generate data with clear trend and seasonality
    dates = pd.date_range(start='2019-01-01', periods=48, freq='M')
    
    # Components
    trend = np.linspace(100, 200, 48)
    seasonal = 30 * np.sin(np.linspace(0, 8 * np.pi, 48))
    noise = np.random.normal(0, 5, 48)
    values = trend + seasonal + noise
    
    df = pd.DataFrame({
        'Date': dates,
        'Value': values
    })
    df.set_index('Date', inplace=True)
    
    # Calculate trend using moving average
    df['Trend'] = df['Value'].rolling(window=12, center=True).mean()
    
    # Calculate seasonal component
    monthly_means = df.groupby(df.index.month)['Value'].mean()
    df['Seasonal'] = df.index.month.map(monthly_means - monthly_means.mean())
    
    # Calculate residual
    df['Residual'] = df['Value'] - df['Trend'] - df['Seasonal']
    
    # Create figure with subplots
    fig, axes = plt.subplots(4, 1, figsize=(14, 12), sharex=True)
    
    # Original data
    axes[0].plot(df.index, df['Value'], color='blue', linewidth=1.5)
    axes[0].set_ylabel('Original', fontsize=10)
    axes[0].set_title('Time Series Seasonal Decomposition', fontsize=14, fontweight='bold')
    axes[0].grid(True, alpha=0.3)
    
    # Trend
    axes[1].plot(df.index, df['Trend'], color='red', linewidth=2)
    axes[1].set_ylabel('Trend', fontsize=10)
    axes[1].grid(True, alpha=0.3)
    
    # Seasonal
    axes[2].plot(df.index, df['Seasonal'], color='green', linewidth=1.5)
    axes[2].set_ylabel('Seasonal', fontsize=10)
    axes[2].grid(True, alpha=0.3)
    axes[2].axhline(y=0, color='gray', linestyle='--', linewidth=1)
    
    # Residual
    axes[3].plot(df.index, df['Residual'], color='purple', linewidth=1, alpha=0.7)
    axes[3].fill_between(df.index, df['Residual'], alpha=0.3, color='purple')
    axes[3].set_ylabel('Residual', fontsize=10)
    axes[3].set_xlabel('Date', fontsize=11)
    axes[3].grid(True, alpha=0.3)
    axes[3].axhline(y=0, color='gray', linestyle='--', linewidth=1)
    
    plt.tight_layout()
    
    return fig, df

fig_decomp, df_decomp = create_seasonal_decomposition()
plt.show()
```

### Interactive Time Series with Plotly

```python
def create_interactive_time_series():
    """
    Create interactive time series visualization with Plotly.
    Provides zooming, panning, and hover information.
    """
    np.random.seed(42)
    
    # Generate multiple series
    dates = pd.date_range(start='2021-01-01', periods=365, freq='D')
    
    data = pd.DataFrame({
        'Date': dates,
        'Revenue': 1000 + np.cumsum(np.random.normal(5, 100, 365)),
        'Costs': 500 + np.cumsum(np.random.normal(2, 50, 365)),
        'Users': 100 + np.cumsum(np.random.normal(1, 10, 365))
    })
    
    # Create interactive visualization
    fig = go.Figure()
    
    # Add traces for each metric
    fig.add_trace(go.Scatter(
        x=data['Date'], y=data['Revenue'],
        mode='lines',
        name='Revenue',
        line=dict(color='green', width=2),
        fill='tozeroy',
        fillcolor='rgba(0, 255, 0, 0.1)'
    ))
    
    fig.add_trace(go.Scatter(
        x=data['Date'], y=data['Costs'],
        mode='lines',
        name='Costs',
        line=dict(color='red', width=2),
        fill='tozeroy',
        fillcolor='rgba(255, 0, 0, 0.1)'
    ))
    
    fig.add_trace(go.Scatter(
        x=data['Date'], y=data['Users'],
        mode='lines',
        name='Users',
        line=dict(color='blue', width=2),
        yaxis='y2'
    ))
    
    # Update layout with dual y-axis
    fig.update_layout(
        title='Interactive Business Metrics Time Series',
        xaxis_title='Date',
        yaxis_title='Revenue/Costs ($)',
        yaxis2=dict(
            title='Users',
            titlefont=dict(color='blue'),
            tickfont=dict(color='blue'),
            anchor='x',
            overlaying='y',
            side='right'
        ),
        hovermode='x unified',
        legend=dict(
            yanchor='top',
            y=0.99,
            xanchor='left',
            x=0.01
        )
    )
    
    # Add range slider
    fig.update_layout(
        xaxis=dict(
            rangeselector=dict(
                buttons=[
                    dict(count=1, label='1M', step='month', stepmode='backward'),
                    dict(count=3, label='3M', step='month', stepmode='backward'),
                    dict(count=6, label='6M', step='month', stepmode='backward'),
                    dict(count=1, label='1Y', step='year', stepmode='backward'),
                    dict(step='all', label='All')
                ]
            ),
            rangeslider=dict(visible=True),
            type='date'
        )
    )
    
    return fig, data

fig_interactive, df_interactive = create_interactive_time_series()
fig_interactive.show()
```

### Best Practices

1. **Clear Time Axis**: Ensure time intervals are correctly spaced and labeled.

2. **Avoid Clutter**: Don't plot too many series on one chart.

3. **Consistent Colors**: Use consistent colors for the same metrics across visualizations.

4. **Include Context**: Add reference lines for averages, targets, or benchmarks.

5. **Show Missing Data**: Clearly indicate gaps in time series data.

## IV. APPLICATIONS

### Standard Example: Sales Data Analysis

```python
def sales_time_series_analysis():
    """
    Comprehensive time series analysis of sales data.
    Demonstrates multiple visualization techniques.
    """
    np.random.seed(42)
    
    # Create date range - daily for 2 years
    dates = pd.date_range(start='2021-01-01', end='2022-12-31', freq='D')
    n = len(dates)
    
    # Create base data with trend and seasonality
    base = 1000
    trend = np.linspace(base, base * 1.5, n)
    weekly_seasonal = 200 * np.sin(2 * np.pi * np.arange(n) / 7)
    yearly_seasonal = 300 * np.sin(2 * np.pi * np.arange(n) / 365)
    noise = np.random.normal(0, 50, n)
    
    sales = trend + weekly_seasonal + yearly_seasonal + noise
    
    data = pd.DataFrame({
        'Date': dates,
        'Sales': sales,
        'Target': trend + yearly_seasonal
    })
    
    # Add some anomalies
    anomaly_indices = [100, 200, 450, 500, 600]
    for idx in anomaly_indices:
        data.loc[idx, 'Sales'] *= 1.3
    
    # Create comprehensive visualization
    fig = make_subplots(
        rows=3, cols=2,
        specs=[[{'colspan': 2}, None],
               [{}, {}],
               [{}, {}]],
        subplot_titles=('Daily Sales vs Target', 'Weekly Aggregation', 
                      'Monthly Trend', 'Year-over-Year Comparison',
                      'Seasonal Pattern (Day of Week)'),
        vertical_spacing=0.1,
        horizontal_spacing=0.1
    )
    
    # Row 1: Daily sales vs target
    fig.add_trace(
        go.Scatter(x=data['Date'], y=data['Sales'],
                 mode='lines', name='Actual',
                 line=dict(color='blue', width=1)),
        row=1, col=1
    )
    fig.add_trace(
        go.Scatter(x=data['Date'], y=data['Target'],
                 mode='lines', name='Target',
                 line=dict(color='green', width=1, dash='dash')),
        row=1, col=1
    )
    
    # Row 2, Col 1: Weekly aggregation
    data['Week'] = data['Date'].dt.to_period('W')
    weekly = data.groupby('Week')['Sales'].sum().reset_index()
    weekly['Week'] = weekly['Week'].astype(str)
    
    fig.add_trace(
        go.Bar(x=weekly['Week'].iloc[::4], y=weekly['Sales'].iloc[::4],
               name='Weekly Sales', marker_color='steelblue'),
        row=2, col=1
    )
    
    # Row 2, Col 2: Monthly trend
    data['Month'] = data['Date'].dt.to_period('M')
    monthly = data.groupby('Month')['Sales'].sum().reset_index()
    monthly['Month'] = monthly['Month'].astype(str)
    
    fig.add_trace(
        go.Scatter(x=monthly['Month'], y=monthly['Sales'],
                   mode='lines+markers', name='Monthly Sales',
                   line=dict(color='red', width=2),
                   marker=dict(size=6)),
        row=2, col=2
    )
    
    # Row 3, Col 1: Year-over-year
    data['Year'] = data['Date'].dt.year
    data['DOY'] = data['Date'].dt.dayofyear
    yoy = data.pivot_table(index='DOY', columns='Year', values='Sales', aggfunc='sum')
    
    fig.add_trace(
        go.Scatter(x=yoy.index, y=yoy[2021], mode='lines', name='2021',
                   line=dict(color='blue', width=2)),
        row=3, col=1
    )
    fig.add_trace(
        go.Scatter(x=yoy.index, y=yoy[2022], mode='lines', name='2022',
                   line=dict(color='orange', width=2)),
        row=3, col=1
    )
    
    # Row 3, Col 2: Day of week pattern
    data['DayOfWeek'] = data['Date'].dt.dayofweek
    dow_pattern = data.groupby('DayOfWeek')['Sales'].mean()
    days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
    
    fig.add_trace(
        go.Bar(x=days, y=dow_pattern, name='Avg by Day',
               marker_color='green'),
        row=3, col=2
    )
    
    fig.update_layout(height=900, showlegend=False)
    
    # Print summary
    print("=" * 60)
    print("SALES TIME SERIES ANALYSIS SUMMARY")
    print("=" * 60)
    print(f"\nTotal Days: {len(data)}")
    print(f"Total Sales: ${data['Sales'].sum():,.0f}")
    print(f"Daily Average: ${data['Sales'].mean():,.2f}")
    print(f"Max Daily Sales: ${data['Sales'].max():,.2f}")
    print(f"Min Daily Sales: ${data['Sales'].min():,.2f}")
    
    return fig, data

fig_sales, df_sales = sales_time_series_analysis()
fig_sales.show()
```

### Real-world Example 1: Banking/Finance Domain

```python
def banking_time_series_analysis():
    """
    Banking/Finance time series visualization.
    Analyzes key financial metrics over time.
    """
    np.random.seed(42)
    
    # Generate 3 years of daily data
    n = 252 * 3
    dates = pd.date_range(start='2020-01-01', periods=n, freq='D')
    
    # Simulate key banking metrics
    # Assets with growth
    assets = 1000000 * np.cumprod(1 + np.random.normal(0.0003, 0.005, n))
    
    # Deposits with seasonality
    deposits = 500000 + 50000 * np.sin(2 * np.pi * np.arange(n) / 252) + \
               np.cumsum(np.random.normal(100, 20000, n))
    
    # Loans following deposits
    loans = 0.6 * deposits + np.cumsum(np.random.normal(50, 10000, n))
    
    # NIM (Net Interest Margin) percentage
    nim = 3.0 + np.random.normal(0, 0.3, n)
    
    # Non-performing loans ratio
    npl = 1.5 + 0.3 * np.sin(2 * np.pi * np.arange(n) / 252) + \
          np.cumsum(np.random.normal(0, 0.05, n))
    npl = np.clip(npl, 0.5, 5.0)
    
    # Customer acquisition
    customers = np.cumsum(np.random.poisson(10, n))
    
    df = pd.DataFrame({
        'Date': dates,
        'Total_Assets': assets,
        'Total_Deposits': deposits,
        'Total_Loans': loans,
        'NIM': nim,
        'NPL_Ratio': npl,
        'Customers': customers
    })
    
    # Calculate derived metrics
    df['Loan_to_Deposit_Ratio'] = df['Total_Loans'] / df['Total_Deposits'] * 100
    df['Asset_Growth'] = df['Total_Assets'].pct_change() * 252 * 100  # Annualized
    
    # Create comprehensive visualization
    fig = make_subplots(
        rows=3, cols=2,
        subplot_titles=('Total Assets & Deposits', 'Loans vs Deposits Ratio',
                     'NIM Trend', 'NPL Ratio',
                     'Customer Growth', 'Monthly Revenue Growth'),
        vertical_spacing=0.1,
        horizontal_spacing=0.1
    )
    
    # Plot 1: Assets and Deposits
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Total_Assets'] / 1e6,
                 mode='lines', name='Assets',
                 line=dict(color='blue', width=2)),
        row=1, col=1
    )
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Total_Deposits'] / 1e6,
                 mode='lines', name='Deposits',
                 line=dict(color='green', width=2)),
        row=1, col=1
    )
    
    # Plot 2: Loan-to-Deposit Ratio
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Loan_to_Deposit_Ratio'],
                 mode='lines', name='L/D Ratio',
                 line=dict(color='purple', width=1.5)),
        row=1, col=2
    )
    fig.add_hline(y=80, line_dash='dash', line_color='red',
                 annotation_text='Min 80%', row=1, col=2)
    
    # Plot 3: NIM
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['NIM'],
                 mode='lines', name='NIM',
                 line=dict(color='orange', width=1)),
        row=2, col=1
    )
    fig.add_hrect(y0=2.5, y1=4, fillcolor='green', opacity=0.1, 
                line_width=0, row=2, col=1)
    
    # Plot 4: NPL Ratio
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['NPL_Ratio'],
                 mode='lines', name='NPL',
                 line=dict(color='red', width=1.5),
                 fill='tozeroy', fillcolor='rgba(255,0,0,0.2)'),
        row=2, col=2
    )
    
    # Plot 5: Customer Growth
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Customers'],
                 mode='lines', name='Customers',
                 line=dict(color='teal', width=2),
                 fill='tozeroy', fillcolor='rgba(0,128,128,0.2)'),
        row=3, col=1
    )
    
    # Plot 6: Monthly Asset Growth
    df['Month'] = df['Date'].dt.to_period('M')
    monthly = df.groupby('Month')['Total_Assets'].sum().pct_change().dropna() * 100
    
    fig.add_trace(
        go.Bar(x=monthly.index.astype(str), y=monthly.values,
               name='Monthly Growth %', marker_color='coral'),
        row=3, col=2
    )
    fig.add_hline(y=0, line_color='gray', row=3, col=2)
    
    fig.update_layout(height=900, showlegend=True)
    
    # Print summary
    print("=" * 60)
    print("BANKING TIME SERIES ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Assets: ${df['Total_Assets'].iloc[-1]:,.0f}")
    print(f"Total Deposits: ${df['Total_Deposits'].iloc[-1]:,.0f}")
    print(f"Total Loans: ${df['Total_Loans'].iloc[-1]:,.0f}")
    print(f"Average NIM: {df['NIM'].mean():.2f}%")
    print(f"Average NPL Ratio: {df['NPL_Ratio'].mean():.2f}%")
    print(f"Total Customers: {df['Customers'].iloc[-1]:,}")
    
    return fig, df

fig_bank_ts, df_bank_ts = banking_time_series_analysis()
fig_bank_ts.show()
```

### Real-world Example 2: Healthcare Domain

```python
def healthcare_time_series_analysis():
    """
    Healthcare time series visualization.
    Tracks patient admissions, metrics over time.
    """
    np.random.seed(42)
    
    # Generate 2 years of daily data
    n = 365 * 2
    dates = pd.date_range(start='2021-01-01', periods=n, freq='D')
    
    # Create healthcare metrics
    # Daily patient admissions with weekly and yearly seasonality
    base_admissions = 50
    weekly_pattern = 20 * np.sin(2 * np.pi * np.arange(n) / 7)
    yearly_pattern = 15 * np.sin(2 * np.pi * np.arange(n) / 365)
    admissions = base_admissions + weekly_pattern + yearly_pattern + np.random.normal(0, 8, n)
    admissions = np.clip(admissions, 10, 100)
    
    # Bed occupancy
    occupancy = 0.7 + 0.2 * np.sin(2 * np.pi * np.arange(n) / 365) + \
                0.1 * np.sin(2 * np.pi * np.arange(n) / 7) + \
                np.random.normal(0, 0.05, n)
    occupancy = np.clip(occupancy, 0.4, 0.95)
    
    # Average length of stay
    los = 4.5 + np.random.normal(0, 1, n)
    los = np.clip(los, 2, 10)
    
    # Patient satisfaction (1-10)
    satisfaction = 7.5 + 0.5 * np.sin(2 * np.pi * np.arange(n) / 180) + \
                   np.random.normal(0, 0.5, n)
    satisfaction = np.clip(satisfaction, 5, 9)
    
    # Readmission rate (percentage)
    readmission = 8 + 2 * np.sin(2 * np.pi * np.arange(n) / 90) + \
                 np.random.normal(0, 1, n)
    readmission = np.clip(readmission, 3, 18)
    
    # Wait times (minutes)
    wait_times = 30 + 10 * np.sin(2 * np.pi * np.arange(n) / 7) + \
                np.random.normal(0, 10, n)
    wait_times = np.clip(wait_times, 10, 90)
    
    df = pd.DataFrame({
        'Date': dates,
        'Admissions': admissions,
        'Bed_Occupancy': occupancy * 100,
        'Avg_LOS': los,
        'Satisfaction': satisfaction,
        'Readmission_Rate': readmission,
        'Wait_Time': wait_times
    })
    
    # Create visualization
    fig = make_subplots(
        rows=3, cols=2,
        subplot_titles=('Daily Admissions', 'Bed Occupancy Rate',
                     'Average Length of Stay', 'Patient Satisfaction',
                     '30-Day Readmission Rate', 'Emergency Wait Times'),
        vertical_spacing=0.1,
        horizontal_spacing=0.1
    )
    
    # Plot 1: Daily Admissions
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Admissions'],
                 mode='lines', name='Admissions',
                 line=dict(color='blue', width=1),
                 fill='tozeroy'),
        row=1, col=1
    )
    
    # Add 30-day rolling average
    df['MA_30'] = df['Admissions'].rolling(30).mean()
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['MA_30'],
                 mode='lines', name='30-Day MA',
                 line=dict(color='red', width=2)),
        row=1, col=1
    )
    
    # Plot 2: Bed Occupancy
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Bed_Occupancy'],
                 mode='lines', name='Occupancy',
                 line=dict(color='green', width=1),
                 fill='tozeroy'),
        row=1, col=2
    )
    fig.add_hline(y=85, line_dash='dash', line_color='red',
                 annotation_text='Capacity', row=1, col=2)
    
    # Plot 3: Length of Stay
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Avg_LOS'],
                 mode='lines', name='LOS',
                 line=dict(color='purple', width=1)),
        row=2, col=1
    )
    
    # Plot 4: Satisfaction
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Satisfaction'],
                 mode='lines', name='Satisfaction',
                 line=dict(color='orange', width=1.5)),
        row=2, col=2
    )
    fig.add_hline(y=8, line_dash='dash', line_color='green',
                 annotation_text='Target', row=2, col=2)
    
    # Plot 5: Readmission Rate
    fig.add_trace(
        go.Scatter(x=df['Date'], y=df['Readmission_Rate'],
                 mode='lines', name='Readmission',
                 line=dict(color='red', width=1.5),
                 fill='tozeroy'),
        row=3, col=1
    )
    fig.add_hline(y=10, line_dash='dash', line_color='orange',
                 annotation_text='Target 10%', row=3, col=1)
    
    # Plot 6: Wait Times
    fig.add_trace(
        go.Box(x=df['Date'].dt.dayofweek, y=df['Wait_Time'],
               name='Wait Times', boxmean=True),
        row=3, col=2
    )
    
    fig.update_layout(height=900, showlegend=True)
    
    # Print summary
    print("=" * 60)
    print("HEALTHCARE TIME SERIES ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Admissions: {df['Admissions'].sum():,.0f}")
    print(f"Average Bed Occupancy: {df['Bed_Occupancy'].mean():.1f}%")
    print(f"Average Length of Stay: {df['Avg_LOS'].mean():.1f} days")
    print(f"Average Satisfaction: {df['Satisfaction'].mean():.1f}/10")
    print(f"Average Readmission: {df['Readmission_Rate'].mean():.1f}%")
    print(f"Average Wait Time: {df['Wait_Time'].mean():.1f} minutes")
    
    return fig, df

fig_health_ts, df_health_ts = healthcare_time_series_analysis()
fig_health_ts.show()
```

## V. OUTPUT_RESULTS

### Expected Outputs

1. **Basic Time Series**: Line chart showing values over time with clear trend and mean line.

2. **Multi-Series**: Comparison of multiple products showing different growth patterns.

3. **Rolling Statistics**: Price chart with moving averages and Bollinger bands, plus volatility subplot.

4. **Candlestick**: Financial chart showing open-high-low-close with green/red indicators.

5. **Decomposition**: Four-panel showing original, trend, seasonal, and residual components.

6. **Interactive**: Plotly chart with zoom, pan, and range selector capabilities.

7. **Banking Example**: Multi-panel showing assets, loans, deposits, NIM, and customer metrics.

8. **Healthcare Example**: Multi-panel showing admissions, occupancy, LOS, satisfaction, and readmission.

## VI. VISUALIZATION

### Flow Chart: Time Series Visualization Process

```
+----------------------+     +-----------------------+
|  COLLECT TIME SERIES  |---->|   UNDERSTAND PATTERN    |
|       DATA           |     |   (Trend/Seasonal)     |
+----------------------+     +-----------------------+
                                                         |
                                                         v
+----------------------+     +-----------------------+
|   PREPARE DATA       |---->|   CHOOSE VISUALIZATION |
|   (Resample/Agg)    |     |   TYPE              |
+----------------------+     +-----------------------+
                                                         |
                                                         v
+----------------------+     +-----------------------+
|   CALCULATE          |---->|   CREATE VISUAL       |
|   DERIVED METRICS   |     |   COMPONENTS         |
+----------------------+     +-----------------------+
                                                         |
                                                         v
+----------------------+     +-----------------------+
|   ADD ANNOTATIONS    |---->|   GENERATE OUTPUT    |
+----------------------+     +-----------------------+


TIME SERIES VISUALIZATION TYPES:
                         
+---> Line Chart -> Single metric over time
+---> Multi-Line -> Multiple metrics comparison
+---> Area Chart -> Volumes and magnitudes
+---> Candlestick -> Financial OHLC
+---> Heat Map -> Calendar view
+---> Decomposition -> Trend/Seasonal/Residual


ROLLING STATISTICS TO USE:
                       
+---> Simple Moving Average (SMA)
+---> Exponential Moving Average (EMA)
+---> Bollinger Bands
+---> Rolling Standard Deviation
+---> Rolling Correlation


AGGREGATION LEVELS:
                
+---> Daily -> Raw data points
+---> Weekly -> Sum or Average
+---> Monthly -> Business summaries
+---> Quarterly -> Financial reporting
+---> Yearly -> Annual summaries
```

## VII. ADVANCED_TOPICS

### Extensions and Variations

1. **Heatmap Calendar Visualization**:
```python
def create_calendar_heatmap():
    """Create GitHub-style calendar heatmap."""
    data['Year'] = data['Date'].dt.year
    data['DOY'] = data['Date'].dt.dayofyear
    
    pivot = data.pivot_table(index='DOY', columns='Year', values='Value')
    
    fig = px.imshow(pivot, labels=dict(x='Year', y='Day', color='Value'))
```

2. **Animated Time Series**:
```python
def create_animated_time_series():
    """Create animated visualization of growing time series."""
    fig = px.bar(df, x='Category', y='Value', animation_frame='Date')
```

3. **Forecasting with Confidence Intervals**:
```python
def plot_forecast():
    """Plot time series with forecast and confidence intervals."""
    fig.add_trace(go.Scatter(x=forecast.index, y=forecast, name='Forecast'))
    fig.add_trace(go.Scatter(
        name='Upper Bound', x=upper.index, y=upper,
        mode='lines', line=dict(width=0), showlegend=False
    ))
    fig.add_trace(go.Scatter(
        name='Lower Bound', x=lower.index, y=lower,
        mode='lines', fill='tonexty', fillcolor='rgba(0,100,80,0.2)',
        line=dict(width=0)
    ))
```

### Lag Analysis

```python
def create_lag_analysis():
    """Create lag plots for autocorrelation analysis."""
    from pandas.plotting import lag_plot
    
    fig, ax = plt.subplots(1, 4, figsize=(16, 4))
    for i, lag in enumerate([1, 7, 14, 30]):
        lag_plot(df['Value'], lag=lag, ax=ax[i], alpha=0.5)
        ax[i].set_title(f'Lag {lag}')
```

### Common Pitfalls

1. **Misleading Scales**: Using truncated y-axis to exaggerate changes - use zero baseline when showing volumes.

2. **Ignoring Seasonality**: Not accounting for regular patterns leading to wrong conclusions.

3. **Over-smoothing**: Rolling averages can hide important short-term patterns.

4. **Missing Data**: Not handling gaps or showing them as zeros.

5. **Cherry-Picking Periods**: Selecting time ranges that show desired patterns.

## VIII. CONCLUSION

### Key Takeaways

1. **Time Series is Unique**: Requires special visualization approaches accounting for temporal order.

2. **Multiple Techniques**: Different analytical questions require different visualizations.

3. **Interactive is Powerful**: Plotly enables exploration static charts can't provide.

4. **Decompose to Understand**: Breaking down into components reveals underlying patterns.

5. **Rolling Statistics**: Provide context and reduce noise in volatile data.

### Next Steps

1. Learn time series forecasting methods
2. Master seasonal decomposition techniques
3. Study anomaly detection in time series
4. Build interactive time series dashboards
5. Practice with real financial and healthcare data

### Further Reading

**Books**:
- "Time Series Analysis" by James D. Hamilton
- "Forecasting: Principles and Practice" by Hyndman & Athanasopoulos

**Online**:
- Plotly Time Series: plotly.com/python/time-series/
- Pandas Lag Plots: pandas.pydata.org/docs/reference/api/pandas.plotting.lag_plot.html