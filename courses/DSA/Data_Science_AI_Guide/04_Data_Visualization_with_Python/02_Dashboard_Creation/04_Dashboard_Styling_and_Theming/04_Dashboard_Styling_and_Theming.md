# Dashboard Styling and Theming

## I. INTRODUCTION

### Styling Principles

Effective dashboard styling:
- Consistent color schemes
- Clear typography
- Proper spacing
- Visual hierarchy

## II. CSS STYLING

### Inline Styles

```python
import dash
from dash import html

app.layout = html.Div([
    html.H1('Title', style={'fontSize': '32px', 'color': '#333'}),
    html.Div('Content', style={'padding': '20px', 'background': '#f5f5f5'})
])
```

### CSS Classes

```python
# CSS file (assets/style.css)
.card {
    background: white;
    padding: 20px;
    margin: 10px;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

# In app
app = dash.Dash(__name__, external_stylesheets=['/style.css'])
```

## III. THEMES

### Dark Theme

```python
dark_theme = {
    'background': '#1a1a1a',
    'text': '#ffffff',
    'accent': '#00a8e8'
}
```

### Light Theme

```python
light_theme = {
    'background': '#ffffff',
    'text': '#333333',
    'accent': '#0074D9'
}
```

## IV. CUSTOMIZING COMPONENTS

### Component Styles

```python
style={'color': 'blue', 'fontSize': '18px'}
className='custom-dropdown'
```

## V. RESPONSIVE DESIGN

### Responsive Containers

```python
app.index_string = '''
...
<style>
    .container { max-width: 1200px; margin: 0 auto; }
    @media (max-width: 768px) { .container { padding: 10px; } }
</style>
...
'''
```

## VI. CONCLUSION

Consistent theming creates professional dashboards.