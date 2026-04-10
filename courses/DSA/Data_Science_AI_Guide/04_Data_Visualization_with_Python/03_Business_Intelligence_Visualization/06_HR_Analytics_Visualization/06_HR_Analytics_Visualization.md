# HR Analytics Visualization

## I. INTRODUCTION

### HR Metrics

HR analytics visualizes workforce metrics including:
- Headcount
- Turnover rates
- Performance distributions
- Compensation analysis

## II. KEY METRICS

### Workforce Metrics

- Turnover Rate: (Departures / Average Headcount)
- Time to Fill: Days to fill positions
- Employee Engagement: Survey scores
- Training ROI: Investment vs productivity

## III. IMPLEMENTATION

### Turnover Analysis

```python
import plotly.express as px

fig = px.histogram(
    df, 
    x='Tenure', 
    color='Status',
    title='Tenure Distribution by Status'
)
```

### Compensation Chart

```python
fig = px.box(
    df,
    x='Department',
    y='Salary',
    title='Compensation by Department'
)
```

### Diversity Dashboard

```python
fig = px.donut(
    df,
    names='Department',
    values='Headcount',
    title='Headcount by Department'
)
```

## IV. CONCLUSION

HR analytics optimizes workforce management.