# Multi-Page Application Design

## I. INTRODUCTION

### Multi-Page Dash Apps

Multi-page Dash applications enable navigation between different views while maintaining state. This is essential for complex dashboards with multiple sections.

### Architecture

- URL routing with dcc.Location
- Page IDs for content
- Shared data with dcc.Store

## II. IMPLEMENTATION

### Basic Multi-Page Structure

```python
import dash
from dash import dcc, html, Input, Output

app = dash.Dash(__name__)

app.layout = html.Div([
    dcc.Location(id='url', refresh=False),
    
    # Navigation
    html.Nav([
        dcc.Link('Home', href='/'),
        dcc.Link('Analytics', href='/analytics'),
        dcc.Link('Reports', href='/reports')
    ]),
    
    # Content container
    html.Div(id='page-content')
])

@callback(
    Output('page-content', 'children'),
    Input('url', 'pathname')
)
def display_page(pathname):
    if pathname == '/analytics':
        return html.H1('Analytics Page')
    elif pathname == '/reports':
        return html.H1('Reports Page')
    return html.H1('Home Page')
```

## III. PAGE COMPONENTS

### Separate Page Files

```python
# pages/home.py
import dash
from dash import html

def layout():
    return html.Div([
        html.H1('Welcome to the Dashboard'),
        # Home page content
    ])

# pages/analytics.py
def layout():
    return html.Div([
        html.H1('Analytics'),
        dcc.Graph(id='chart')
    ])
```

## IV. NAVIGATION PATTERNS

### Sidebar Navigation

```python
app.layout = html.Div([
    html.Div([
        dcc.Link('Overview', href='/'),
        dcc.Link('Sales', href='/sales'),
        dcc.Link('Customers', href='/customers')
    ], className='sidebar'),
    html.Div(id='page-content', className='content')
])
```

## V. SHARED DATA

### Cross-Page Data

```python
# Shared store
dcc.Store(id='shared-data', data=df.to_dict())

# In callbacks
@callback(
    Output('sales-chart', 'figure'),
    Input('shared-data', 'data')
)
def update_chart(data):
    df = pd.DataFrame(data)
    return px.bar(df, x='Month', y='Sales')
```

## VI. CONCLUSION

Multi-page apps enable scalable dashboard architectures.