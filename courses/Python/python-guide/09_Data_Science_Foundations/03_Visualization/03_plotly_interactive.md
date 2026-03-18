# 🎯 Plotly Interactive Charts

## 🎯 What You'll Learn

- Create interactive charts you can zoom, hover, and pan
- Use plotly.express for simple, beautiful plots
- Customize hover data and tooltips
- Create 3D scatter plots
- Save interactive HTML files

## 📦 Prerequisites

- Read [02_seaborn_statistical_plots.md](./02_seaborn_statistical_plots.md) first

## What is Plotly?

Plotly creates **interactive charts** that you can zoom, pan, hover, and share in a browser. Static plots are so 2010!

### Installing

```python
pip install plotly pandas
```

### Two Ways to Use Plotly

1. **plotly.express (px)** — Simple, one-liner plots (recommended for beginners!)
2. **plotly.graph_objects (go)** — Full control, more complex

We'll focus on **px** (express)!

## Quick Start with Plotly Express

```python
import plotly.express as px
import pandas as pd
import numpy as np

# Create sample data
df: pd.DataFrame = pd.DataFrame({
    "x": np.arange(1, 11),
    "y": np.random.randint(10, 100, 10),
    "category": ["A", "B"] * 5
})

# Create interactive line chart
fig: px.Line = px.line(df, x="x", y="y", title="My First Plotly Chart!")

# Show in browser
fig.show()
```

### 💡 Line-by-Line Breakdown

- `px.line()` - Create line chart
- `x=`, `y=` - Column names from DataFrame
- `fig.show()` - Open interactive chart in browser

## Scatter Plot with Hover

```python
import plotly.express as px
import pandas as pd
import numpy as np

# Create richer data
df: pd.DataFrame = pd.DataFrame({
    "math": np.random.normal(70, 10, 100),
    "reading": np.random.normal(75, 12, 100),
    "science": np.random.normal(72, 11, 100),
    "student_id": [f"Student_{i}" for i in range(100)],
    "grade": np.random.choice(["A", "B", "C", "D"], 100)
})

# Interactive scatter with custom hover data
fig: px.Scatter = px.scatter(
    df,
    x="math",
    y="reading",
    color="grade",
    size="science",
    hover_data=["student_id"],
    title="Student Scores Comparison",
    labels={"math": "Math Score", "reading": "Reading Score"},
    color_discrete_sequence=px.colors.qualitative.Set2
)

fig.show()
```

### 💡 Line-by-Line Breakdown

- `color="grade"` - Color points by grade
- `size="science"` - Point size based on science score
- `hover_data=["student_id"]` - Show student ID on hover
- `color_discrete_sequence` - Custom color palette

## Bar Chart

```python
import plotly.express as px
import pandas as pd

# Sales data
df: pd.DataFrame = pd.DataFrame({
    "product": ["Laptop", "Phone", "Tablet", "Watch", "Headphones"],
    "sales": [150, 200, 80, 120, 95],
    "revenue": [150000, 80000, 40000, 24000, 19000]
})

fig: px.Bar = px.bar(
    df,
    x="product",
    y="sales",
    text="sales",  # Show value on bar
    title="Product Sales",
    color="sales",  # Color by value
    color_continuous_scale="Viridis"
)

fig.update_traces(textposition="outside")  # Move labels outside
fig.show()
```

## Histogram

```python
import plotly.express as px
import pandas as pd
import numpy as np

# Generate data
df: pd.DataFrame = pd.DataFrame({
    "score": np.concatenate([
        np.random.normal(70, 10, 500),
        np.random.normal(85, 8, 300)
    ]),
    "test": ["Test 1"] * 500 + ["Test 2"] * 300
})

fig: px.Histogram = px.histogram(
    df,
    x="score",
    color="test",
    nbins=30,
    barmode="overlay",  # Overlay both histograms
    title="Score Distribution by Test",
    opacity=0.7
)

fig.show()
```

### 💡 Line-by-Line Breakdown

- `nbins=30` - Number of bins
- `barmode="overlay"` - Stack or overlay multiple histograms
- `opacity=0.7` - Transparency

## Box Plot

```python
import plotly.express as px
import pandas as pd
import numpy as np

# Tips dataset
tips: pd.DataFrame = pd.DataFrame({
    "day": ["Sat", "Sun", "Mon", "Tue", "Wed", "Thu", "Fri"] * 10,
    "tip": np.random.uniform(2, 8, 70),
    "total_bill": np.random.uniform(10, 50, 70),
    "smoker": ["Yes"] * 35 + ["No"] * 35
})

fig: px.Box = px.box(
    tips,
    x="day",
    y="tip",
    color="smoker",
    title="Tip Distribution by Day",
    points="all"  # Show individual points too
)

fig.show()
```

## 3D Scatter Plot (Just for Fun!)

```python
import plotly.express as px
import pandas as pd
import numpy as np

# 3D data
df: pd.DataFrame = pd.DataFrame({
    "x": np.random.randn(200),
    "y": np.random.randn(200),
    "z": np.random.randn(200),
    "category": np.random.choice(["A", "B", "C"], 200)
})

fig: px.Scatter3d = px.scatter_3d(
    df,
    x="x",
    y="y",
    z="z",
    color="category",
    title="3D Scatter Plot!",
    size_max=10,
    opacity=0.8
)

fig.show()
```

### 💡 Explanation

3D plots are great for exploring three variables, but can get hard to read. Use sparingly!

## Save and Share

```python
# Save as interactive HTML
fig.write_html("my_chart.html")

# Save as static image (requires kaleido)
fig.write_image("my_chart.png")
```

## Real Example: World Happiness Explorer

```python
import plotly.express as px
import pandas as pd

# Simulated happiness data
happiness: pd.DataFrame = pd.DataFrame({
    "Country": ["Finland", "Denmark", "Switzerland", "Iceland", "Netherlands",
                "Norway", "Sweden", "Israel", "New Zealand", "Austria"],
    "Score": [7.84, 7.62, 7.57, 7.55, 7.45, 7.39, 7.36, 7.36, 7.28, 7.25],
    "GDP": [45000, 42000, 48000, 41000, 40000, 55000, 43000, 38000, 36000, 44000],
    "Freedom": [0.94, 0.95, 0.92, 0.96, 0.92, 0.93, 0.93, 0.88, 0.94, 0.91],
    "Region": ["Europe", "Europe", "Europe", "Europe", "Europe",
               "Europe", "Europe", "Asia", "Oceania", "Europe"]
})

fig: px.Scatter = px.scatter(
    happiness,
    x="GDP",
    y="Score",
    size="Freedom",
    color="Region",
    hover_name="Country",
    text="Country",
    title="World Happiness vs GDP (Bubble Size = Freedom)",
    log_x=True,  # Log scale for GDP
    size_max=40
)

fig.update_traces(textposition="top center")
fig.update_layout(template="plotly_white")

fig.show()
```

## Brief: What is Dash?

Dash is Plotly's framework for building **web dashboards** with Python. It's like Streamlit but more powerful!

```python
# Just so you know it exists:
# pip install dash
# Then create: app.layout, @app.callback, run_server()
```

This is beyond our scope, but now you know Dash exists for when you need full web apps!

## ✅ Summary

- Plotly creates **interactive** charts (zoom, hover, pan)
- Use `px` (plotly.express) for simple, beautiful plots
- `px.scatter()`, `px.line()`, `px.bar()`, `px.histogram()`, `px.box()`
- `color=`, `size=`, `hover_data=` - encode extra info
- `fig.show()` - open in browser
- `fig.write_html()` - save as interactive HTML

## ➡️ Next Steps

Ready to learn machine learning? Head to **[../../10_Machine_Learning/01_ML_Concepts/01_what_is_ml.md](../../10_Machine_Learning/01_ML_Concepts/01_what_is_ml.md)**!

## 🔗 Further Reading

- [Plotly Express Docs](https://plotly.com/python/plotly-express/)
- [Plotly Gallery](https://plotly.com/python/)
- [Dash Framework](https://dash.plotly.com/)
