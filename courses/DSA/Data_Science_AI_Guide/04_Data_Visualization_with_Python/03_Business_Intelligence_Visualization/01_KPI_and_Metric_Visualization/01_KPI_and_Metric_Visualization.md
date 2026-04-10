# KPI and Metric Visualization

## I. INTRODUCTION

### What are KPIs?

Key Performance Indicators (KPIs) are measurable values demonstrating effectiveness in achieving business objectives. Visualization transforms KPIs into actionable insights.

### Common KPIs

- Revenue growth
- Profit margins  
- Customer acquisition cost
- Conversion rates
- Churn rates

## II. FUNDAMENTALS

### KPI Categories

1. **Financial**: Revenue, profit, ROI
2. **Operational**: Efficiency, throughput
3. **Customer**: Satisfaction, retention
4. **Employee**: Productivity, turnover

## III. IMPLEMENTATION

### KPI Gauge Charts

```python
import plotly.graph_objects as go

fig = go.Figure(go.Indicator(
    mode = "gauge+number",
    value = 450,
    title = {"text": "Revenue (K)"},
    gauge = {
        'axis': {'range': [0, 500]},
        'bar': {'color': "darkblue"}
    }
))
```

### KPI Cards

```python
import dash
from dash import html

kpi_card = html.Div([
    html.H3('Total Revenue'),
    html.H2('$1.2M'),
    html.P('+15% vs last month', style={'color': 'green'})
], className='kpi-card')
```

### Trend Indicators

```python
import plotly.express as px

fig = px.bar(df, x='Month', y='Revenue',
            title='Monthly Revenue Trend')
```

## IV. APPLICATIONS

### Banking KPIs

```python
banking_kpis = {
    'NIM': 'Net Interest Margin',
    'NPL': 'Non-Performing Loans Ratio',
    'ROE': 'Return on Equity',
    'CAR': 'Capital Adequacy Ratio'
}
```

### Healthcare KPIs

```python
healthcare_kpis = {
    'Bed Occupancy': 'Percentage',
    'Wait Time': 'Minutes',
    'Readmission Rate': 'Percentage',
    'Satisfaction Score': 'Out of 10'
}
```

## V. CONCLUSION

Effective KPI visualization drives business decisions.