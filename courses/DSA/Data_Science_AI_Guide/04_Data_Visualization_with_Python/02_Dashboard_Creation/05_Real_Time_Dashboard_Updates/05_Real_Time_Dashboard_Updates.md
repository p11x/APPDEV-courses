# Real-Time Dashboard Updates

## I. INTRODUCTION

### Real-Time Dashboards

Real-time updates refresh data automatically using:
- Interval components
- Clientside callbacks
- WebSocket updates

## II. INTERVAL COMPONENT

```python
from dash import dcc, html, Input, Output

app.layout = html.Div([
    dcc.Interval(id='interval', interval=5000),  # 5 seconds
    html.Div(id='live-data')
])

@callback(
    Output('live-data', 'children'),
    Input('interval', 'n_intervals')
)
def update_data(n):
    return f"Updated: {datetime.now()}"
```

## III. DATA REFRESH STRATEGIES

### Clientside Callbacks

```python
app.clientside_callback(
    "function(n_intervals)",
    Output('live-chart', 'figure'),
    Input('interval', 'n_intervals')
)
def update_clientside(n):
    return px.line(data, x='time', y='value')
```

## IV. CONCLUSION

Real-time updates enable live monitoring dashboards.