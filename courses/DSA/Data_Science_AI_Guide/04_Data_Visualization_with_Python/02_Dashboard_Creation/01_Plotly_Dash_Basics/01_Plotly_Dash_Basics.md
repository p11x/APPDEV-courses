# Plotly Dash Basics

## I. INTRODUCTION

### What is Plotly Dash?

Dash is an open-source Python framework for building analytical web applications. Itcombines the power of Plotly charts with Flask and provides a simple Python API for creating interactive dashboards without requiring JavaScript.

Dash applications consist of:
- Layout: Visual components
- Callbacks: Functions that update the layout based on inputs

### Why Use Dash?

1. Pure Python - no JavaScript needed
2. Reactive - updates automatically
3. Production-ready
4. Extensive gallery

### Prerequisites

- Python 3.6+
- pip install dash pandas plotly

## II. FUNDAMENTALS

### Basic Structure

```python
import dash
from dash import dcc, html, Input, Output, callback

app = dash.Dash(__name__)

app.layout = html.Div([
    dcc.Location(id='url', refresh=False),
    html.H1('My Dashboard'),
    dcc.Graph(id='chart')
])

@callback(Output('chart', 'figure'), Input('url', 'pathname'))
def update_chart(pathname):
    return {'data': [...]}

if __name__ == '__main__':
    app.run(debug=True)
```

## III. IMPLEMENTATION

### Basic App Structure

```python
import dash
from dash import dcc, html
import plotly.express as px
import pandas as pd
import numpy as np

# Create app
app = dash.Dash(__name__)

# Sample data
np.random.seed(42)
df = pd.DataFrame({
    'Category': ['A', 'B', 'C', 'D'] * 25,
    'Value': np.random.randn(100),
    'Date': pd.date_range('2022-01-01', periods=100)
})

# App layout
app.layout = html.Div([
    html.H1('Sales Dashboard'),
    
    html.Div([
        html.Div([
            html.H3('Total Sales'),
            html.P(f"${df['Value'].sum():,.0f}")
        ], className='card'),
        html.Div([
            html.H3('Avg Order'),
            html.P(f"${df['Value'].mean():,.0f}")
        ], className='card'),
        html.Div([
            html.H3('Orders'),
            html.P(len(df))
        ], className='card'),
    ], className='metrics'),
    
    dcc.Graph(id='main-chart'),
    
    dcc.Store(id='stored-data', data=df.to_dict())
], style={'fontFamily': 'Arial'})

# Run
if __name__ == '____main__':
    app.run(debug=True)
```

### Dropdown Controls

```python
from dash import dcc, html, Input, Output

# Add dropdown to layout
app.layout = html.Div([
    dcc.Dropdown(
        id='category-dropdown',
        options=[{'label': c, 'value': c} for c in df['Category'].unique()],
        value='A'
    ),
    dcc.Graph(id='filtered-chart')
])

@callback(
    Output('filtered-chart', 'figure'),
    Input('category-dropdown', 'value')
)
def update_filtered_chart(category):
    filtered = df[df['Category'] == category]
    
    fig = px.bar(filtered, x='Date', y='Value', title=f'Sales for {category}')
    return fig
```

### Slider Controls

```python
from dash import dcc, html, Input, Output

app.layout = html.Div([
    html.Label('Select Range'),
    dcc.Slider(
        id='range-slider',
        min=0,
        max=100,
        step=10,
        value=50,
        marks={0: '0', 50: '50', 100: '100'}
    ),
    dcc.Graph(id='slider-chart')
])

@callback(
    Output('slider-chart', 'figure'),
    Input('range-slider', 'value')
)
def update_slider_chart(value):
    filtered = df[df['Value'] < value]
    fig = px.histogram(filtered, x='Value')
    return fig
```

### Tabs for Multiple Pages

```python
from dash import dcc, html

app.layout = html.Div([
    dcc.Tabs([
        dcc.Tab(label='Overview', value='overview'),
        dcc.Tab(label='Details', value='details'),
        dcc.Tab(label='Settings', value='settings')
    ]),
    html.Div(id='tab-content')
])

@callback(
    Output('tab-content', 'children'),
    Input('tabs', 'value')
)
def render_tab_content(value):
    if value == 'overview':
        return html.H2('Overview Content')
    elif value == 'details':
        return html.H2('Details Content')
    return html.H2('Settings Content')
```

## IV. APPLICATIONS

### Banking Dashboard

```python
def create_banking_dashboard():
    app = dash.Dash(__name__)
    
    np.random.seed(42)
    accounts = pd.DataFrame({
        'Account': [f'ACC-{i:04d}' for i in range(100)],
        'Balance': np.random.exponential(5000, 100),
        'Type': np.random.choice(['Checking', 'Savings', 'Investment'], 100),
        'Status': np.random.choice(['Active', 'Inactive', 'Blocked'], 100)
    })
    
    app.layout = html.Div([
        html.H1('Banking Dashboard'),
        
        # KPI cards
        html.Div([
            html.Div([
                html.H4('Total Accounts'),
                html.H2(len(accounts))
            ], className='kpi'),
            html.Div([
                html.H4('Total Balance'),
                html.H2(f"${accounts['Balance'].sum():,.0f}")
            ], className='kpi'),
            html.Div([
                html.H4('Active Rate'),
                html.H2(f"{(accounts['Status']=='Active').mean()*100:.0f}%")
            ], className='kpi')
        ], className='kpi-row'),
        
        # Charts
        dcc.Graph(
            id='balance-histogram',
            figure=px.histogram(accounts, x='Balance', title='Balance Distribution')
        ),
        
        dcc.Graph(
            id='account-type-pie',
            figure=px.pie(accounts, names='Type', title='Account Types')
        ),
        
        dcc.Graph(
            id='status-bar',
            figure=px.bar(accounts.groupby('Status')['Balance'].sum().reset_index(),
                         x='Status', y='Balance', title='Balance by Status')
        )
    ])
    
    return app
```

### Healthcare Dashboard

```python
def create_healthcare_dashboard():
    app = dash.Dash(__name__)
    
    np.random.seed(42)
    patients = pd.DataFrame({
        'Patient_ID': range(200),
        'Age': np.random.normal(45, 18, 200).clip(1, 100),
        'Department': np.random.choice(['Emergency', 'Surgery', 'Pediatrics', 'Cardiology'], 200),
        'Wait_Time': np.random.exponential(30, 200),
        'Satisfaction': np.random.uniform(1, 10, 200)
    })
    
    app.layout = html.Div([
        html.H1('Patient Metrics Dashboard'),
        
        # Summary cards
        html.Div([
            html.Div([html.H4('Total Patients'), html.H2(len(patients))]),
            html.Div([html.H4('Avg Wait Time'), 
                     html.H2(f"{patients['Wait_Time'].mean():.0f} min")]),
            html.Div([html.H4('Avg Satisfaction'),
                     html.H2(f"{patients['Satisfaction'].mean():.1f}/10")])
        ], className='kpi-row'),
        
        dcc.Graph(id='age-dist', 
                 figure=px.histogram(patients, x='Age', title='Age Distribution')),
        
        dcc.Graph(id='dept-bar',
                 figure=px.bar(patients.groupby('Department').size().reset_index(),
                              x='Department', y=0, title='Patients by Department'))
    ])
    
    return app
```

## V. STYLING

### CSS Styling

```python
# Custom CSS
app = dash.Dash(__name__, external_stylesheets=['/style.css'])

# Internal styles
app.index_string = '''
<!DOCTYPE html>
<html>
    <head>
        {%metas%}
        <title>{%title%}</title>
        {%favicon%}
        {%css%}
        <style>
            .card {
                background: white;
                padding: 20px;
                margin: 10px;
                border-radius: 8px;
                box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            }
            .kpi {
                text-align: center;
                padding: 20px;
                background: #f8f9fa;
                border-radius: 8px;
            }
        </style>
    </head>
    <body>
        {%app_entry%}
        <footer>
            {%config%}
            {%scripts%}
            {%renderer%}
        </footer>
    </body>
</html>
'''
```

## VI. DEPLOYMENT

### Running the App

```python
if __name__ == '__main__':
    app.run(debug=True)  # Development
    
# Production
# app.run(host='0.0.0.0', port=8050)
```

## VII. CONCLUSION

### Key Takeaways

1. Dash provides Python-only dashboard building.

2. Layout + callbacks = complete app.

3. Extensive component library.

4. Easy deployment.

### Further Reading

- Dash Documentation: dash.plotly.com
- Plotly Express: plotly.com/python/plotly-express/