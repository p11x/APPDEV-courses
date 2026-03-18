# 📈 Matplotlib Basics

## 🎯 What You'll Learn

- Create plots with the fig, ax pattern
- Draw line plots, scatter plots, bar charts, histograms
- Customize titles, labels, and legends
- Save figures and create subplots

## 📦 Prerequisites

- Complete the Pandas section (folder 09/02_Pandas)

## What is Matplotlib?

Matplotlib is the **grandparent of all Python plotting**! It powers seaborn, pandas plotting, and most visualization libraries. Master this, and everything else becomes easier.

### Installing

```python
pip install matplotlib
```

### Import Convention

```python
import matplotlib.pyplot as plt  # Most common import
import numpy as np
```

## The fig, ax Pattern (Important!)

Always use this pattern — NOT `plt.plot()` directly!

```python
import matplotlib.pyplot as plt
import numpy as np

# Create figure and axes
fig, ax = plt.subplots()

# Plot data
x: np.ndarray = np.array([1, 2, 3, 4, 5])
y: np.ndarray = np.array([2, 4, 6, 8, 10])

ax.plot(x, y)

# Show the plot
plt.show()
```

### 💡 Line-by-Line Breakdown

- `plt.subplots()` - Create figure and axes (returns fig, ax)
- `ax.plot(x, y)` - Draw a line plot on the axes
- `plt.show()` - Display the plot

### Why fig, ax?

Because you get **more control**! With the axes object:
- Multiple plots on one figure
- Each plot has its own style
- Easier to save and customize

## Line Plot

```python
import matplotlib.pyplot as plt
import numpy as np

# Temperature data for a week
days: list[str] = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"]
temps: list[int] = [22, 24, 23, 25, 28, 26, 24]

# Create figure
fig, ax = plt.subplots(figsize=(10, 6))  # Width, height in inches

# Plot line
ax.plot(days, temps, linewidth=2, color="#FF6B6B", marker="o")

# Customize
ax.set_title("Weekly Temperature", fontsize=14, fontweight="bold")
ax.set_xlabel("Day of Week", fontsize=12)
ax.set_ylabel("Temperature (°C)", fontsize=12)
ax.grid(True, alpha=0.3)  # Add grid lines

# Save figure
plt.savefig("temperature_plot.png", dpi=150, bbox_inches="tight")
plt.show()
```

### 💡 Line-by-Line Breakdown

- `figsize=(10, 6)` - Set figure size in inches
- `linewidth=2` - Line thickness
- `color="#FF6B6B"` - Hex color (coral red)
- `marker="o"` - Add circles at data points
- `dpi=150` - Resolution (higher = sharper)
- `bbox_inches="tight"` - Remove extra white space

## Scatter Plot

```python
import matplotlib.pyplot as plt
import numpy as np

# Generate random data
np.random.seed(42)  # For reproducibility
x: np.ndarray = np.random.randn(50)  # 50 random points, normal distribution
y: np.ndarray = x * 2 + np.random.randn(50) * 0.5  # Correlated with noise

fig, ax = plt.subplots()

# Scatter plot
ax.scatter(x, y, alpha=0.7, s=80, c="#4ECDC4", edgecolors="white", linewidth=1)

ax.set_title("Scatter: X vs Y", fontsize=14)
ax.set_xlabel("X Value", fontsize=12)
ax.set_ylabel("Y Value", fontsize=12)
ax.grid(True, alpha=0.3)

plt.show()
```

### 💡 Line-by-Line Breakdown

- `alpha=0.7` - Transparency (0=invisible, 1=solid)
- `s=80` - Size of points
- `c="#4ECDC4"` - Teal color
- `edgecolors="white"` - White border around points
- `linewidth=1` - Border thickness

## Bar Chart

```python
import matplotlib.pyplot as plt
import numpy as np

# Sales data
products: list[str] = ["Laptop", "Phone", "Tablet", "Watch", "Headphones"]
sales: list[int] = [150, 200, 80, 120, 95]

fig, ax = plt.subplots(figsize=(10, 6))

# Bar chart
bars: list = ax.bar(products, sales, color="#95E1D3", edgecolor="#38A3A5", linewidth=2)

# Add value labels on top of bars
for bar in bars:
    height: float = bar.get_height()
    ax.text(bar.get_x() + bar.get_width()/2, height + 3,
            f'{int(height)}', ha="center", va="bottom", fontsize=10)

ax.set_title("Product Sales", fontsize=14, fontweight="bold")
ax.set_xlabel("Product", fontsize=12)
ax.set_ylabel("Units Sold", fontsize=12)
ax.set_ylim(0, 250)  # Set y-axis limit

plt.show()
```

### 💡 Line-by-Line Breakdown

- `ax.bar(products, sales)` - Create bar chart
- `ax.text()` - Add text annotations (sales numbers)
- `ha="center"` - Horizontal alignment
- `va="bottom"` - Vertical alignment (above the bar)
- `ax.set_ylim(0, 250)` - Set y-axis range

## Histogram

```python
import matplotlib.pyplot as plt
import numpy as np

# Generate 1000 random numbers from normal distribution
data: np.ndarray = np.random.normal(loc=100, scale=15, size=1000)

fig, ax = plt.subplots(figsize=(10, 6))

# Histogram
n, bins, patches = ax.hist(data, bins=30, color="#F38181", edgecolor="white", alpha=0.8)

# Add vertical line at mean
mean_val: float = np.mean(data)
ax.axvline(mean_val, color="#2D3436", linestyle="--", linewidth=2, label=f"Mean: {mean_val:.1f}")

ax.set_title("Distribution of Values", fontsize=14, fontweight="bold")
ax.set_xlabel("Value", fontsize=12)
ax.set_ylabel("Frequency", fontsize=12)
ax.legend()

plt.show()
```

### 💡 Line-by-Line Breakdown

- `bins=30` - Number of bins (higher = more detail)
- `ax.hist()` - Returns (counts, bin edges, patches)
- `ax.axvline()` - Add vertical line
- `linestyle="--"` - Dashed line style
- `label=f"Mean: {mean_val:.1f}"` - Format to 1 decimal place

## Pie Chart

```python
import matplotlib.pyplot as plt

# Market share data
labels: list[str] = ["Company A", "Company B", "Company C", "Others"]
sizes: list[int] = [35, 25, 20, 20]
colors: list[str] = ["#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4"]
explode: tuple = (0.05, 0, 0, 0)  # Slightly separate first slice

fig, ax = plt.subplots(figsize=(8, 8))

ax.pie(sizes, explode=explode, labels=labels, colors=colors,
       autopct="%1.1f%%", shadow=True, startangle=90)

ax.set_title("Market Share", fontsize=14, fontweight="bold")

plt.show()
```

### 💡 Line-by-Line Breakdown

- `explode=(0.05, 0, 0, 0)` - Pull out first slice slightly
- `autopct="%1.1f%%"` - Show percentages
- `shadow=True` - Add 3D shadow effect
- `startangle=90` - Start from top

## Subplots: Multiple Charts

```python
import matplotlib.pyplot as plt
import numpy as np

# Create 2x2 subplots
fig, axes = plt.subplots(2, 2, figsize=(12, 10))

# Plot 1: Line
axes[0, 0].plot([1, 2, 3], [1, 4, 9], color="#FF6B6B")
axes[0, 0].set_title("Line Plot")

# Plot 2: Scatter
np.random.seed(42)
axes[0, 1].scatter(np.random.randn(30), np.random.randn(30), alpha=0.7, color="#4ECDC4")
axes[0, 1].set_title("Scatter Plot")

# Plot 3: Bar
axes[1, 0].bar(["A", "B", "C", "D"], [10, 25, 15, 30], color="#95E1D3")
axes[1, 0].set_title("Bar Chart")

# Plot 4: Histogram
axes[1, 1].hist(np.random.randn(100), bins=20, color="#F38181", edgecolor="white")
axes[1, 1].set_title("Histogram")

# Add overall title
fig.suptitle("Four Different Plots", fontsize=16, fontweight="bold", y=1.02)

plt.tight_layout()  # Adjust spacing
plt.show()
```

### 💡 Line-by-Line Breakdown

- `plt.subplots(2, 2)` - Create 2×2 grid, returns axes array
- `axes[0, 0]` - First subplot (top-left)
- `fig.suptitle()` - Title for entire figure
- `plt.tight_layout()` - Prevent overlapping labels

## Styles

```python
import matplotlib.pyplot as plt

# Use built-in style
plt.style.use("seaborn-v0_8-whitegrid")  # Many styles available!

# Common styles: "seaborn-v0_8-darkgrid", "ggplot", "bmh"
# Check available: plt.style.available

# Your plot will now look beautiful automatically!
fig, ax = plt.subplots()
ax.plot([1, 2, 3], [1, 4, 9])
plt.show()
```

## ✅ Summary

- Always use `fig, ax = plt.subplots()` pattern
- `ax.plot()` - line plot
- `ax.scatter()` - scatter plot
- `ax.bar()` - bar chart
- `ax.hist()` - histogram
- `ax.set_title()`, `ax.set_xlabel()`, `ax.set_ylabel()` - labels
- `plt.savefig()` - save to file
- `plt.subplots(2, 2)` - create subplots

## ➡️ Next Steps

Ready to make beautiful statistical plots? Head to **[02_seaborn_statistical_plots.md](./02_seaborn_statistical_plots.md)**!

## 🔗 Further Reading

- [Matplotlib Documentation](https://matplotlib.org/stable/index.html)
- [PyPlot Tutorial](https://matplotlib.org/stable/tutorials/introductory/pyplot.html)
- [Sample Plots](https://matplotlib.org/stable/gallery/index.html)
