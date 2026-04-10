# Interactive Dashboard Components

## I. INTRODUCTION

### Interactive Components Overview

Dash provides numerous interactive components:
- Dropdowns
- Sliders
- Checkboxes
- Radio Buttons
- Date Pickers
- Upload Components
- Data Tables

## II. DROPDOWNS AND MULTI-SELECTS

```python
from dash import dcc, html, Input, Output

app.layout = html.Div([
    dcc.Dropdown(
        id='country-dropdown',
        options=[
            {'label': 'USA', 'value': 'USA'},
            {'label': 'Canada', 'value': 'Canada'},
            {'label': 'UK', 'value': 'UK'}
        ],
        value='USA',
        multi=True
    ),
    dcc.Graph(id='country-chart')
])

@callback(
    Output('country-chart', 'figure'),
    Input('country-dropdown', 'value')
)
def update_chart(countries):
    filtered = df[df['Country'].isin(countries)]
    return px.scatter(filtered, x='Sales', y='Profit')
```

## III. SLIDERS AND RANGES

```python
app.layout = html.Div([
    dcc.Slider(
        id='price-slider',
        min=0,
        max=1000,
        step=50,
        value=500,
        marks={0: '$0', 500: '$500', 1000: '$1000'}
    ),
    
    dcc.RangeSlider(
        id='range-slider',
        min=0,
        max=100,
        step=10,
        value=[20, 80]
    )
])
```

## IV. DATA TABLES

```python
from dash import dash_table

app.layout = dash.DataTable(
    id='data-table',
    columns=[{'name': i, 'id': i} for i in df.columns],
    data=df.to_dict('records'),
    page_size=10,
    filter_action='native',
    sort_action='native'
)
```

## V. GRAPHS WITH INTERACTIONS

```python
@callback(
    Output('click-data', 'children'),
    Input('scatter-plot', 'clickData')
def display_click_data(clickData):
    if clickData:
        return str(clickData['points'][0])
```

## VI. STORES FOR STATE

```python
dcc.Store(id='memory-store', data=initial_data)
dcc.Store(id='session-store', data={}, storage_type='session')
```

## VII. CONCLUSION

Key interactive components enable rich dashboard functionality.