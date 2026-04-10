# 3D Data Visualization

## I. INTRODUCTION

### What is 3D Data Visualization?

3D data visualization extends traditional 2D plotting into three dimensions, allowing visualization of data with three variables simultaneously. This technique enables visualization of complex relationships that would be difficult or impossible to see in 2D plots.

3D visualization is particularly useful for:
- Volume visualization
- Surface plots
- 3D scatter plots
- Animated 3D rotations
- Medical imaging
- Scientific simulations

### Why 3D Visualization Matters?

1. **Enhanced Perception**: Adding depth reveals patterns in three-variable relationships.

2. **Volume Representation**: Essential for medical, geological, and weather data.

3. **Spatial Relationships**: Shows 3D spatial distributions.

4. **Interactive Exploration**: 3D plots can be rotated to find optimal viewing angles.

### Prerequisites

- Python programming
- numpy for 3D arrays
- matplotlib for basic 3D
- plotly for interactive 3D
- Mayavi for advanced scientific visualization

## II. FUNDAMENTALS

### Key Concepts

**3D Axes**: X, Y, Z axes representing three variables.

**Surface Plot**: 3D surface showing function of two variables.

**Wireframe**: Grid-based 3D representation.

**Scatter 3D**: Points in 3D space.

**3D Bar**: 3D bar charts.

## III. IMPLEMENTATION

```python
import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import plotly.graph_objects as go
from plotly.subplots import make_subplots
import pandas as pd
import warnings
warnings.filterwarnings('ignore')


def create_3d_scatter():
    """Create 3D scatter plot."""
    np.random.seed(42)
    
    n = 100
    x = np.random.normal(0, 1, n)
    y = np.random.normal(0, 1, n)
    z = np.random.normal(0, 1, n)
    
    fig = plt.figure(figsize=(10, 8))
    ax = fig.add_subplot(111, projection='3d')
    
    # Color by z value
    colors = z
    sc = ax.scatter(x, y, z, c=colors, cmap='viridis', s=50, alpha=0.7)
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title('3D Scatter Plot')
    
    plt.colorbar(sc, label='Z Value')
    plt.tight_layout()
    
    return fig

fig = create_3d_scatter()
plt.show()


def create_3d_surface():
    """Create 3D surface plot."""
    # Create meshgrid
    x = np.linspace(-5, 5, 50)
    y = np.linspace(-5, 5, 50)
    X, Y = np.meshgrid(x, y)
    
    # Z as function of X and Y
    Z = np.sin(np.sqrt(X**2 + Y**2))
    
    fig = plt.figure(figsize=(12, 8))
    ax = fig.add_subplot(111, projection='3d')
    
    # Plot surface
    surf = ax.plot_surface(X, Y, Z, cmap='viridis', alpha=0.8,
                          linewidth=0, antialiased=False)
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title('3D Surface Plot')
    
    plt.colorbar(surf, shrink=0.5)
    plt.tight_layout()
    
    return fig

fig = create_3d_surface()
plt.show()


def create_3d_wireframe():
    """Create wireframe plot."""
    x = np.linspace(-5, 5, 20)
    y = np.linspace(-5, 5, 20)
    X, Y = np.meshgrid(x, y)
    Z = X * np.exp(-X**2 - Y**2)
    
    fig = plt.figure(figsize=(10, 8))
    ax = fig.add_subplot(111, projection='3d')
    
    ax.plot_wireframe(X, Y, Z, color='blue', alpha=0.5)
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title('3D Wireframe')
    
    plt.tight_layout()
    
    return fig

fig = create_3d_wireframe()
plt.show()


def create_3d_bar_chart():
    """Create 3D bar chart."""
    np.random.seed(42)
    
    # Data
    xpos = [0, 1, 2, 3]
    ypos = [0, 1, 2, 3]
    xpos, ypos = np.meshgrid(xpos, ypos)
    xpos = xpos.flatten()
    ypos = ypos.flatten()
    
    zpos = np.zeros_like(xpos)
    
    dx = 0.5 * np.ones_like(xpos)
    dy = 0.5 * np.ones_like(xpos)
    dz = np.random.normal(10, 3, len(xpos))
    
    fig = plt.figure(figsize=(10, 8))
    ax = fig.add_subplot(111, projection='3d')
    
    ax.bar3d(xpos, ypos, zpos, dx, dy, dz, color='steelblue', alpha=0.8)
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title('3D Bar Chart')
    
    plt.tight_layout()
    
    return fig

fig = create_3d_bar_chart()
plt.show()


def create_interactive_3d_plotly():
    """Create interactive 3D with Plotly."""
    np.random.seed(42)
    
    n = 500
    data = pd.DataFrame({
        'X': np.random.randn(n),
        'Y': np.random.randn(n),
        'Z': np.random.randn(n),
        'Group': np.random.choice(['A', 'B', 'C'], n),
        'Size': np.random.uniform(5, 20, n)
    })
    
    fig = go.Figure()
    
    for group in ['A', 'B', 'C']:
        subset = data[data['Group'] == group]
        fig.add_trace(go.Scatter3d(
            x=subset['X'],
            y=subset['Y'],
            z=subset['Z'],
            mode='markers',
            name=f'Group {group}',
            marker=dict(
                size=subset['Size'],
                opacity=0.7
            )
        ))
    
    fig.update_layout(
        scene=dict(
            xaxis_title='X',
            yaxis_title='Y',
            zaxis_title='Z'
        ),
        title='Interactive 3D Scatter'
    )
    
    return fig

fig = create_interactive_3d_plotly()
fig.show()


def create_3d_time_series():
    """Create animated 3D time series."""
    np.random.seed(42)
    
    # Time steps
    n_steps = 50
    
    fig = go.Figure()
    
    for t in range(n_steps):
        x = np.sin(t / 10 + np.arange(20) * 0.3) + np.random.normal(0, 0.1, 20)
        y = np.cos(t / 10 + np.arange(20) * 0.3) + np.random.normal(0, 0.1, 20)
        z = np.arange(20) * 0.2 + np.random.normal(0, 0.1, 20)
        
        fig.add_trace(go.Scatter3d(
            x=x, y=y, z=z,
            mode='lines+markers',
            name=f'Time {t}',
            visible=(t == 0)  # Only first frame visible initially
        ))
    
    # Create slider
    sliders = [dict(
        active=t,
        steps=[dict(
            method='update',
            args=[{'visible': [False] * n_steps},
                  {'title': f'Frame {t}'}]
        ) for t in range(n_steps)]
    )]
    
    fig.update_layout(
        title='3D Time Series Animation',
        sliders=sliders
    )
    
    return fig

fig = create_3d_time_series()
fig.show()
```

## IV. APPLICATIONS

### Standard Example: Multi-variable Analysis

```python
def sales_3d_analysis():
    """Analyze sales data with 3D visualization."""
    np.random.seed(42)
    
    n = 200
    data = pd.DataFrame({
        'Marketing_Spend': np.random.uniform(1000, 50000, n),
        'Sales_Team_Size': np.random.randint(5, 100, n),
        'Customer_Satisfaction': np.random.uniform(3, 9, n),
        'Revenue': np.random.uniform(10000, 500000, n)
    })
    
    # Add correlation
    data['Revenue'] = (0.3 * data['Marketing_Spend'] + 
                      2000 * data['Sales_Team_Size'] +
                      10000 * data['Customer_Satisfaction'])
    data['Revenue'] += np.random.normal(0, 20000, n)
    
    fig = make_subplots(rows=2, cols=2,
                       specs=[[{'type': 'scatter3d'}, {'type': 'surface'}],
                              [{'type': 'scatter3d'}, {'type': 'scatter3d'}]],
                       subplot_titles=['3D: Revenue vs Marketing & Team',
                                     'Surface: Satisfaction vs Variables',
                                     'Color by Revenue', 'Rotated View'])
    
    # 3D Scatter
    fig.add_trace(go.Scatter3d(
        x=data['Marketing_Spend'] / 1000,
        y=data['Sales_Team_Size'],
        z=data['Revenue'] / 1000,
        mode='markers',
        marker=dict(
            size=5,
            color=data['Revenue'],
            colorscale='Viridis',
            opacity=0.7
        )
    ), row=1, col=1)
    
    # Surface plot
    x = np.linspace(1000, 50000, 30)
    y = np.linspace(5, 100, 30)
    X, Y = np.meshgrid(x, y)
    Z = 0.3 * X + 2000 * Y + 20000
    
    fig.add_trace(go.Surface(x=x, y=y, z=Z), row=1, col=2)
    
    # Color by satisfaction
    fig.add_trace(go.Scatter3d(
        x=data['Marketing_Spend'] / 1000,
        y=data['Sales_Team_Size'],
        z=data['Customer_Satisfaction'],
        mode='markers',
        marker=dict(
            size=5,
            color=data['Revenue'],
            colorscale='Plasma'
        )
    ), row=2, col=1)
    
    # Different angle
    fig.add_trace(go.Scatter3d(
        x=data['Sales_Team_Size'],
        y=data['Customer_Satisfaction'],
        z=data['Revenue'] / 1000,
        mode='markers',
        marker=dict(
            size=5,
            color=data['Marketing_Spend'],
            colorscale='Blues'
        )
    ), row=2, col=2)
    
    fig.update_layout(height=900)
    
    return fig, data

fig, data = sales_3d_analysis()
fig.show()
```

### Real-world Example 1: Banking

```python
def banking_risk_3d():
    """3D visualization of banking risk factors."""
    np.random.seed(42)
    
    n = 150
    data = pd.DataFrame({
        'Credit_Score': np.random.normal(680, 80, n).clip(300, 850),
        'Debt_to_Income': np.random.beta(2, 5, n) * 0.5,
        'Employment_Years': np.random.exponential(5, n).clip(0, 20),
        'Loan_Amount': np.random.lognormal(10, 0.8, n).clip(5000, 500000),
        'Default': np.random.choice([0, 1], n, p=[0.85, 0.15])
    })
    
    # More defaults for low credit score
    data.loc[data['Credit_Score'] < 600, 'Default'] = 1
    
    fig = go.Figure()
    
    # Color by default status
    colors = {0: 'green', 1: 'red'}
    
    for status in [0, 1]:
        subset = data[data['Default'] == status]
        label = 'Non-Default' if status == 0 else 'Default'
        
        fig.add_trace(go.Scatter3d(
            x=subset['Credit_Score'],
            y=subset['Debt_to_Income'],
            z=subset['Employment_Years'],
            mode='markers',
            name=label,
            marker=dict(
                size=5,
                color=colors[status],
                opacity=0.7
            )
        ))
    
    fig.update_layout(
        scene=dict(
            xaxis_title='Credit Score',
            yaxis_title='Debt to Income',
            zaxis_title='Employment (Years)'
        ),
        title='Loan Risk Assessment - 3D View'
    )
    
    return fig, data

fig, data = banking_risk_3d()
fig.show()
```

### Real-world Example 2: Healthcare

```python
def patient_outcomes_3d():
    """3D visualization of patient outcomes."""
    np.random.seed(42)
    
    n = 200
    data = pd.DataFrame({
        'Age': np.random.normal(55, 18, n).clip(18, 95),
        'BMI': np.random.normal(28, 5, n).clip(16, 45),
        'BP_Systolic': np.random.normal(130, 15, n).clip(80, 200),
        'Hospital_Stay': np.random.exponential(3, n).clip(1, 30),
        'Outcome': np.random.choice(['Good', 'Fair', 'Poor'], n,
                                    p=[0.6, 0.25, 0.15])
    })
    
    fig = go.Figure()
    
    outcome_colors = {'Good': 'green', 'Fair': 'orange', 'Poor': 'red'}
    
    for outcome in ['Good', 'Fair', 'Poor']:
        subset = data[data['Outcome'] == outcome]
        fig.add_trace(go.Scatter3d(
            x=subset['Age'],
            y=subset['BMI'],
            z=subset['BP_Systolic'],
            mode='markers',
            name=outcome,
            marker=dict(
                size=5,
                color=outcome_colors[outcome],
                opacity=0.7
            )
        ))
    
    fig.update_layout(
        scene=dict(
            xaxis_title='Age',
            yaxis_title='BMI',
            zaxis_title='Blood Pressure'
        ),
        title='Patient Outcomes by Risk Factors'
    )
    
    return fig, data

fig, data = patient_outcomes_3d()
fig.show()
```

## V. OUTPUT_RESULTS

Expected outputs:
- 3D scatter with color mapping
- Surface plots with lighting
- Wireframe grids
- 3D bar charts
- Interactive Plotly visualizations
- Animated 3D sequences

## VI. BEST PRACTICES

1. Use appropriate viewing angles
2. Add proper lighting for surfaces
3. Consider performance with large datasets
4. Use color scales appropriately
5. Include rotation for better perception

## VII. CONCLUSION

### Key Takeaways

1. 3D visualization reveals complex relationships.

2. Interactive 3D allows exploration.

3. Multiple techniques: surface, scatter, wireframe.

4. Domain applications in finance, healthcare.

### Further Reading

- Plotly 3D: plotly.com/python/3d-charts/
- Matplotlib 3D: matplotlib.org/stable/gallery/mplot3d