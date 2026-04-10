# Customizing Plots and Styling

## Table of Contents
1. [Introduction](#introduction)
2. [Fundamentals](#fundamentals)
3. [Implementation](#implementation)
4. [Applications](#applications)
5. [Output Results](#output-results)
6. [Visualization](#visualization)
7. [Advanced Topics](#advanced-topics)
8. [Conclusion](#conclusion)

---

## Introduction

### Overview

This module covers advanced plot customization and styling techniques in Matplotlib and Seaborn. Learning to customize plots effectively is crucial for creating publication-quality visualizations that communicate data insights clearly and professionally. This module explores colors, fonts, spines, ticks, legends, annotations, and styling themes.

Customization goes beyond basic plotting to include fine-tuning every visual element, from axis labels to color palettes. This level of control distinguishes good visualizations from great ones.

### Learning Objectives

By the end of this module, you will be able to:
- Customize colors, fonts, and text properties
- Control tick marks and axis spines
- Style and position legends effectively
- Add annotations and callouts
- Apply custom themes and stylesheets
- Create publication-ready figures

### Prerequisites

- Python 3.7+
- Matplotlib, Seaborn, NumPy, Pandas installed
- Understanding of basic plotting concepts

---

## Fundamentals

### Understanding the Matplotlib Object Hierarchy

```
Figure
├── Backdrop
│   ├── Facecolor
│   └── Edgecolor
├── Subplots (Axes)
│   ├── Spine (top, bottom, left, right)
│   ├── Axis (XAxis, YAxis)
│   │   ├── Ticks (Major, Minor)
│   │   ├── Tick Labels
│   │   └── Label
│   ├── Title
│   ├── Labels (xlabel, ylabel)
│   ├── Legend
│   ├── Grid
│   └── Artists (Line2D, Text, Patch)
└── Colorbar
```

### Basic Styling

```python
import matplotlib.pyplot as plt
import numpy as np

# Create a figure with custom styling
fig, ax = plt.subplots(figsize=(10, 6), facecolor='#f5f5f5')
ax.set_facecolor('#ffffff')

# Add data
x = np.linspace(0, 10, 100)
y = np.sin(x)

ax.plot(x, y, color='#1f77b4', linewidth=2, linestyle='-')

# Customize spines
ax.spines['top'].set_visible(False)
ax.spines['right'].set_visible(False)
ax.spines['left'].set_color('#333333')
ax.spines['bottom'].set_color('#333333')

# Customize ticks
ax.tick_params(axis='both', colors='#333333', labelsize=10)
ax.set_xticks([0, 2, 4, 6, 8, 10])
ax.set_yticks([-1, -0.5, 0, 0.5, 1])

# Add labels
ax.set_xlabel('X Values', fontsize=12, color='#333333')
ax.set_ylabel('Y Values', fontsize=12, color='#333333')
ax.set_title('Sine Wave', fontsize=14, fontweight='bold', color='#333333')

# Add grid
ax.grid(True, alpha=0.3, linestyle='--', color='#cccccc')

plt.tight_layout()
plt.savefig('styled_plot.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Colors and Colormaps

```python
import matplotlib.pyplot as plt
import matplotlib.colors as mcolors
import numpy as np

# Named colors
named_colors = ['red', 'blue', 'green', 'orange', 'purple', 'brown']
print("Named colors available:", len(mcolors.XKCD_COLORS_NAMES), "colors")

# RGB and RGBA colors
fig, axes = plt.subplots(1, 2, figsize=(12, 5))

# Using RGB tuple
ax1 = axes[0]
x = np.linspace(0, 10, 50)
ax1.plot(x, np.sin(x), color=(0.2, 0.4, 0.8), linewidth=2, label='RGB')
ax1.plot(x, np.cos(x), color=(0.8, 0.4, 0.2), linewidth=2, label='RGB')
ax1.set_title('RGB Colors')
ax1.legend()

# Using RGBA with alpha
ax2 = axes[1]
ax2.plot(x, np.sin(x), color=(0.2, 0.4, 0.8, 0.7), linewidth=2, label='RGBA')
ax2.plot(x, np.cos(x), color=(0.8, 0.4, 0.2, 0.7), linewidth=2, label='RGBA')
ax2.set_title('RGBA with Alpha')
ax2.legend()

plt.tight_layout()
plt.show()

# Hex colors
hex_colors = ['#FF5733', '#33FF57', '#3357FF', '#F0F0F0', '#000000']

# Colormaps
colormaps = ['viridis', 'plasma', 'inferno', 'magma', 'cividis',
             'Blues', 'Reds', 'Greens', 'Oranges', 'Purples']

fig, ax = plt.subplots(figsize=(10, 6))
for i, cmap in enumerate(colormaps[:5]):
    gradient = np.linspace(0, 1, 256).reshape(1, -1)
    ax.imshow(gradient, aspect='auto', cmap=cmap, 
             extent=[0, 10, i*2, i*2+1])
ax.set_yticks([0.5, 2.5, 4.5, 6.5, 8.5])
ax.set_yticklabels(colormaps[:5])
ax.set_title('Available Colormaps')
plt.tight_layout()
plt.show()
```

### Fonts and Text

```python
import matplotlib.pyplot as plt
import matplotlib.font_manager as fm
import numpy as np

# List available fonts
available_fonts = [f.name for f in fm.fontManager.ttflist[:20]]
print("Sample fonts:", available_fonts[:10])

# Custom font properties
fig, ax = plt.subplots(figsize=(10, 6))

# Title with custom font
ax.set_title('Custom Font Styles', fontsize=18, fontfamily='serif', 
             fontweight='bold', color='#333333')

# X and Y labels
ax.set_xlabel('X Axis Label', fontsize=14, fontfamily='sans-serif')
ax.set_ylabel('Y Axis Label', fontsize=14, fontfamily='sans-serif')

# Add text annotations
ax.text(5, 0.5, 'Serif Text', fontsize=14, fontfamily='serif', 
        ha='center', va='center')
ax.text(5, 0, 'Sans-serif Text', fontsize=14, fontfamily='sans-serif', 
        ha='center', va='center')
ax.text(5, -0.5, 'Monospace Text', fontsize=14, fontfamily='monospace', 
        ha='center', va='center')

# Annotation with arrow
ax.annotate('Peak', xy=(np.pi/2, 1), xytext=(np.pi, 0.5),
            fontsize=12, arrowprops=dict(arrowstyle='->', color='red'),
            ha='center')

# Add tick labels
ax.set_xticks([0, np.pi/2, np.pi, 3*np.pi/2, 2*np.pi])
ax.set_xticklabels(['0', 'π/2', 'π', '3π/2', '2π'])

plt.tight_layout()
plt.show()
```

### Legends

```python
import matplotlib.pyplot as plt
import numpy as np

fig, axes = plt.subplots(2, 2, figsize=(12, 10))

# Simple legend
ax1 = axes[0, 0]
x = np.linspace(0, 10, 50)
ax1.plot(x, np.sin(x), label='sin(x)')
ax1.plot(x, np.cos(x), label='cos(x)')
ax1.legend(loc='upper right')
ax1.set_title('Simple Legend')

# Legend with custom frame
ax2 = axes[0, 1]
ax2.plot(x, np.sin(x), label='sin(x)')
ax2.plot(x, np.cos(x), label='cos(x)')
ax2.legend(loc='upper right', frameon=True, framealpha=0.9, 
          facecolor='white', edgecolor='gray')
ax2.set_title('Custom Frame Legend')

# Multiple lines legend
ax3 = axes[1, 0]
for i in range(5):
    ax3.plot(x, np.sin(x + i*np.pi/5), label=f'Series {i+1}')
ax3.legend(loc='center left', bbox_to_anchor=(1, 0.5), ncol=1)
ax3.set_title('Legend Outside Plot')

# Legend with custom markers
ax4 = axes[1, 1]
line1, = ax4.plot([1, 2, 3], [1, 2, 3], 'o-', label='Circle')
line2, = ax4.plot([1, 2, 3], [2, 3, 4], 's-', label='Square')
line3, = ax4.plot([1, 2, 3], [3, 4, 5], '^-', label='Triangle')
ax4.legend(handles=[line1, line2, line3], loc='upper left')
ax4.set_title('Custom Markers Legend')

plt.tight_layout()
plt.show()
```

### Spines and Ticks

```python
import matplotlib.pyplot as plt
import numpy as np
from matplotlib.ticker import MultipleLocator, FormatStrFormatter

fig, axes = plt.subplots(1, 2, figsize=(14, 6))

# Spine positioning
ax1 = axes[0]
x = np.linspace(0, 10, 50)
ax1.plot(x, np.sin(x), color='#1f77b4', linewidth=2)

# Move spines
ax1.spines['left'].set_position('zero')
ax1.spines['bottom'].set_position('zero')
ax1.spines['top'].set_visible(False)
ax1.spines['right'].set_visible(False)

ax1.set_title('Centered Spines')

# Custom ticks
ax2 = axes[1]
x = np.linspace(0, 10, 50)
ax2.plot(x, np.sin(x), color='#1f77b4', linewidth=2)

# Major and minor locators
ax2.xaxis.set_major_locator(MultipleLocator(2))
ax2.xaxis.set_minor_locator(MultipleLocator(0.5))
ax2.yaxis.set_major_locator(MultipleLocator(0.5))
ax2.yaxis.set_minor_locator(MultipleLocator(0.1))

# Format tick labels
ax2.xaxis.set_major_formatter(FormatStrFormatter('%d'))
ax2.yaxis.set_major_formatter(FormatStrFormatter('%.1f'))

# Style minor ticks
ax2.tick_params(axis='both', which='minor', length=3, color='gray')
ax2.tick_params(axis='both', which='major', length=5, color='black')

ax2.set_title('Custom Ticks')

# Grid
ax2.grid(True, which='major', linestyle='-', alpha=0.6)
ax2.grid(True, which='minor', linestyle=':', alpha=0.3)

plt.tight_layout()
plt.show()
```

---

## Implementation

### Comprehensive Styling Examples

### Example 1: Corporate Financial Report Style

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd

# Define corporate colors
CORPORATE_COLORS = {
    'primary': '#003366',
    'secondary': '#006699',
    'accent': '#FF9900',
    'gray': '#666666',
    'light_gray': '#CCCCCC',
    'background': '#F5F5F5'
}

def create_corporate_style():
    """Create a corporate-styled figure"""
    fig, ax = plt.subplots(figsize=(12, 7), facecolor='white')
    ax.set_facecolor(CORPORATE_COLORS['background'])
    
    # Style the axes
    ax.spines['top'].set_visible(False)
    ax.spines['right'].set_visible(False)
    ax.spines['left'].set_color(CORPORATE_COLORS['gray'])
    ax.spines['bottom'].set_color(CORPORATE_COLORS['gray'])
    
    # Style ticks
    ax.tick_params(axis='both', colors=CORPORATE_COLORS['gray'], 
                   labelsize=10)
    
    # Add grid
    ax.grid(True, alpha=0.3, linestyle='-', 
            color=CORPORATE_COLORS['light_gray'])
    
    return fig, ax

# Financial data plot
np.random.seed(42)
years = [2019, 2020, 2021, 2022, 2023, 2024]
revenue = [100, 115, 130, 155, 180, 210]
profit = [15, 18, 22, 28, 35, 42]

fig, ax = create_corporate_style()

# Plot revenue
bars = ax.bar(years, revenue, color=CORPORATE_COLORS['primary'], 
             edgecolor='white', width=0.6, label='Revenue ($M)')

# Plot profit line
ax.plot(years, profit, color=CORPORATE_COLORS['accent'], 
        linewidth=3, marker='o', markersize=8, label='Profit ($M)')

# Add value labels on bars
for bar, value in zip(bars, revenue):
    ax.text(bar.get_x() + bar.get_width()/2., value + 3,
           f'${value}M', ha='center', va='bottom', fontsize=10,
           fontweight='bold', color=CORPORATE_COLORS['primary'])

# Labels and title
ax.set_xlabel('Year', fontsize=12, fontweight='bold',
            color=CORPORATE_COLORS['gray'])
ax.set_ylabel('Amount ($M)', fontsize=12, fontweight='bold',
            color=CORPORATE_COLORS['gray'])
ax.set_title('Financial Performance 2019-2024', 
           fontsize=16, fontweight='bold',
           color=CORPORATE_COLORS['primary'])

# Legend
ax.legend(loc='upper left', fontsize=10, frameon=True,
        facecolor='white', edgecolor=CORPORATE_COLORS['light_gray'])

# Set y-axis limits
ax.set_ylim(0, 250)

# Format x-axis
ax.set_xticks(years)
ax.set_xticklabels(years)

plt.tight_layout()
plt.savefig('corporate_financial.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 2: Scientific Publication Style

```python
import matplotlib.pyplot as plt
import numpy as np

# Publication style constants
PUB_STYLE = {
    'font_family': 'serif',
    'font_size': 10,
    'title_size': 12,
    'label_size': 10,
    'linewidth': 1.0,
    'marker_size': 4,
    'color_1': '#2171b5',
    'color_2': '#cb181d',
    'color_3': '#238b45'
}

def setup_publication_axes(ax):
    """Setup axes for publication"""
    ax.spines['top'].set_visible(False)
    ax.spines['right'].set_visible(False)
    ax.spines['left'].set_linewidth(0.8)
    ax.spines['bottom'].set_linewidth(0.8)
    ax.tick_params(axis='both', which='major', 
                  width=0.8, labelsize=9)
    return ax

# Generate scientific data
np.random.seed(42)
x = np.linspace(0, 10, 50)
y1 = 2 * x + 5 + np.random.randn(50) * 2
y2 = 2.2 * x + 4 + np.random.randn(50) * 2.5

fig, ax = plt.subplots(figsize=(6, 4.5))

# Setup publication style
ax = setup_publication_axes(ax)

# Add data with error bands
ax.plot(x, y1, color=PUB_STYLE['color_1'], 
        linewidth=PUB_STYLE['linewidth'], 
        label='Condition A')
ax.plot(x, y2, color=PUB_STYLE['color_2'], 
        linewidth=PUB_STYLE['linewidth'], 
        label='Condition B')

# Add confidence bands (shaded)
for y, color, label in [(y1, PUB_STYLE['color_1'], 'A'), 
                        (y2, PUB_STYLE['color_2'], 'B')]:
    y_mean = pd.Series(y).rolling(5).mean()
    y_std = pd.Series(y).rolling(5).std()
    ax.fill_between(x, y_mean - y_std, y_mean + y_std, 
                    alpha=0.2, color=color)

ax.set_xlabel('Time (s)', fontsize=PUB_STYLE['label_size'], 
              fontfamily=PUB_STYLE['font_family'])
ax.set_ylabel('Response (AU)', fontsize=PUB_STYLE['label_size'], 
             fontfamily=PUB_STYLE['font_family'])
ax.set_title('Response vs Time', fontsize=PUB_STYLE['title_size'], 
            fontfamily=PUB_STYLE['font_family'], fontweight='bold')

ax.legend(fontsize=9, frameon=False)
ax.grid(True, alpha=0.3, linestyle=':')

plt.tight_layout()
plt.savefig('publication_figure.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 3: Dashboard Style

```python
import matplotlib.pyplot as plt
import numpy as np

DASHBOARD_COLORS = {
    'bg_dark': '#1e1e1e',
    'bg_light': '#2d2d2d',
    'text': '#ffffff',
    'accent': '#00d4ff',
    'success': '#00ff88',
    'warning': '#ffaa00',
    'danger': '#ff4444'
}

def create_dashboard_theme():
    """Set up dashboard dark theme"""
    plt.style.use('dark_background')
    plt.rcParams.update({
        'figure.facecolor': DASHBOARD_COLORS['bg_dark'],
        'axes.facecolor': DASHBOARD_COLORS['bg_light'],
        'axes.edgecolor': DASHBOARD_COLORS['text'],
        'axes.labelcolor': DASHBOARD_COLORS['text'],
        'xtick.color': DASHBOARD_COLORS['text'],
        'ytick.color': DASHBOARD_COLORS['text'],
        'text.color': DASHBOARD_COLORS['text'],
        'grid.color': '#444444',
        'grid.alpha': 0.5
    })

create_dashboard_theme()

# Create dashboard plot
fig, ax = plt.subplots(figsize=(10, 6))

# Generate time series data
np.random.seed(42)
dates = pd.date_range(start='2024-01-01', periods=30, freq='D')
values = np.cumsum(np.random.randn(30))

# Plot with dashboard colors
ax.plot(dates, values, color=DASHBOARD_COLORS['accent'], 
        linewidth=2, label='Metric')
ax.fill_between(dates, values, alpha=0.3, color=DASHBOARD_COLORS['accent'])

# Add threshold line
ax.axhline(y=np.mean(values), color=DASHBOARD_COLORS['success'],
           linestyle='--', linewidth=1.5, label='Target')

# Style the plot
ax.set_xlabel('Date', fontsize=11)
ax.set_ylabel('Value', fontsize=11)
ax.set_title('Real-Time Metric Dashboard', fontsize=14, fontweight='bold')

ax.legend(loc='upper left', facecolor=DASHBOARD_COLORS['bg_light'],
         edgecolor='gray')

plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig('dashboard_plot.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 4: Custom Annotations and Callouts

```python
import matplotlib.pyplot as plt
import matplotlib.patches as mpatches
from matplotlib.patches import FancyBboxPatch, FancyArrowPatch
import numpy as np

fig, ax = plt.subplots(figsize=(12, 8))

# Generate data
np.random.seed(42)
x = np.linspace(0, 20, 100)
y = np.sin(x/2) * np.exp(-x/10) + np.random.randn(100) * 0.1

# Plot main data
ax.plot(x, y, color='#1f77b4', linewidth=2, label='Data')

# Add annotation with box
ax.annotate('Peak\nMaximum', 
            xy=(3.14, 1.0), xytext=(6, 0.7),
            fontsize=11, ha='center',
            bbox=dict(boxstyle='round,pad=0.5', 
                     facecolor='#ffffcc', 
                     edgecolor='#cccc00', 
                     alpha=0.9),
            arrowprops=dict(arrowstyle='->', 
                         color='#cccc00', 
                         connectionstyle='arc3,rad=0.3'))

# Add annotation with pointing arrow
ax.annotate('Trough\nMinimum', 
            xy=(9.42, -0.4), xytext=(12, -0.8),
            fontsize=11, ha='center',
            bbox=dict(boxstyle='round,pad=0.5', 
                     facecolor='#ffe6e6', 
                     edgecolor='#cc0000', 
                     alpha=0.9),
            arrowprops=dict(arrowstyle='->', 
                         color='#cc0000', 
                         connectionstyle='arc3,rad=-0.3'))

# Add callout box
ax.add_patch(FancyBboxPatch((1.5, 0.5), 3, 1.5,
                            boxstyle='round,pad=0.1',
                            facecolor='#e6f3ff',
                            edgecolor='#0066cc',
                            linewidth=2))

ax.text(3, 1.2, 'Interesting\nRegion', ha='center', va='bottom',
        fontsize=10, color='#0066cc')

ax.set_xlabel('Time', fontsize=12)
ax.set_ylabel('Amplitude', fontsize=12)
ax.set_title('Custom Annotations and Callouts', fontsize=14, fontweight='bold')
ax.legend(loc='upper right')
ax.grid(True, alpha=0.3)

plt.tight_layout()
plt.savefig('annotations.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 5: Multi-Style Legend with Custom Entries

```python
import matplotlib.pyplot as plt
import matplotlib.lines as mlines
import numpy as np

fig, ax = plt.subplots(figsize=(10, 6))

# Generate data
np.random.seed(42)
x = np.linspace(0, 10, 50)

# Plot different line styles
line1, = ax.plot(x, np.sin(x), 'o-', color='#1f77b4', markersize=4,
                label='Sine wave')
line2, = ax.plot(x, np.cos(x), 's--', color='#ff7f0e', markersize=4,
                label='Cosine wave')
line3, = ax.plot(x, np.sin(x + np.pi/2), '^:', color='#2ca02c', markersize=4,
                label='Shifted sine')

# Add custom legend entries
p1 = mpatches.Patch(color='#1f77b4', label='Primary')
p2 = mpatches.Patch(color='#ff7f0e', label='Secondary')
p3 = mpatches.Patch(color='#2ca02c', label='Tertiary')

# Create custom legend
legend = ax.legend(handles=[line1, line2, line3, p1, p2, p3],
                   labels=['Primary Data', 'Secondary Data', 'Tertiary Data',
                          'Analysis 1', 'Analysis 2', 'Analysis 3'],
                   loc='upper right', framealpha=0.9,
                   ncol=2, fontsize=10)

# Style legend box
legend.get_frame().set_facecolor('#f5f5f5')
legend.get_frame().set_edgecolor('#cccccc')

ax.set_xlabel('X Values', fontsize=12)
ax.set_ylabel('Y Values', fontsize=12)
ax.set_title('Multi-Style Legend', fontsize=14, fontweight='bold')
ax.grid(True, alpha=0.3)

plt.tight_layout()
plt.savefig('custom_legend.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Applications

### Banking Sector Applications

#### Application 1: Risk Dashboard Styling

```python
import matplotlib.pyplot as plt
import numpy as np |
import pandas as pd

RISK_COLORS = {
    'low': '#28a745',
    'medium': '#ffc107',
    'high': '#dc3545',
    'critical': '#6f0000',
    'neutral': '#6c757d'
}

def create_risk_dashboard():
    """Create styled risk dashboard"""
    fig, axes = plt.subplots(2, 2, figsize=(14, 10))
    fig.suptitle('Risk Management Dashboard', fontsize=16, 
                fontweight='bold', y=0.98)
    
    return fig, axes

fig, axes = create_risk_dashboard()

# Risk distribution
ax1 = axes[0, 0]
risk_categories = ['Low', 'Medium', 'High', 'Critical']
risk_counts = [150, 80, 45, 25]
colors = [RISK_COLORS['low'], RISK_COLORS['medium'], 
          RISK_COLORS['high'], RISK_COLORS['critical']]

bars = ax1.bar(risk_categories, risk_counts, color=colors, edgecolor='white')
ax1.set_xlabel('Risk Category', fontsize=11)
ax1.set_ylabel('Count', fontsize=11)
ax1.set_title('Risk Distribution', fontsize=12, fontweight='bold')

for bar, count in zip(bars, risk_counts):
    ax1.text(bar.get_x() + bar.get_width()/2., bar.get_height() + 2,
            str(count), ha='center', fontweight='bold')

# Risk by department
ax2 = axes[0, 1]
departments = ['IT', 'Finance', 'Operations', 'Sales', 'Marketing']
dept_risk = [25, 18, 32, 15, 20]
colors_dept = ['#dc3545' if v > 25 else '#ffc107' if v > 20 else '#28a745' 
               for v in dept_risk]
ax2.barh(departments, dept_risk, color=colors_dept, edgecolor='white')
ax2.set_xlabel('Average Risk Score', fontsize=11)
ax2.set_title('Risk by Department', fontsize=12, fontweight='bold')
ax2.axvline(x=20, color='orange', linestyle='--', linewidth=1.5, label='Threshold')

# Risk trend
ax3 = axes[1, 0]
np.random.seed(42)
dates = pd.date_range(start='2024-01-01', periods=12, freq='M')
risk_score = 30 + np.cumsum(np.random.randn(12) * 2)

ax3.plot(dates, risk_score, color='#dc3545', linewidth=2, marker='o')
ax3.fill_between(dates, risk_score, alpha=0.3, color='#dc3545')
ax3.set_xlabel('Month', fontsize=11)
ax3.set_ylabel('Risk Score', fontsize=11)
ax3.set_title('Risk Score Trend', fontsize=12, fontweight='bold')
ax3.tick_params(axis='x', rotation=45)

# Portfolio composition
ax4 = axes[1, 1]
asset_classes = ['Equities', 'Bonds', 'Alternatives', 'Cash']
allocations = [45, 30, 15, 10]
explode = (0.05, 0, 0, 0)

wedges, texts, autotexts = ax4.pie(allocations, explode=explode, 
           labels=asset_classes, autopct='%1.1f%%',
           startangle=90, colors=['#1f77b4', '#ff7f0e', '#2ca02c', '#d62728'])
ax4.set_title('Asset Allocation', fontsize=12, fontweight='bold')

plt.tight_layout()
plt.savefig('risk_dashboard.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Healthcare Sector Applications

#### Application 2: Clinical Report Styling

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd

CLINICAL_COLORS = {
    'normal': '#28a745',
    'elevated': '#ffc107',
    'high': '#dc3545',
    'baseline': '#17a2b8',
    'treatment': '#6f42c1'
}

def create_clinical_report():
    """Create styled clinical report"""
    fig, ax = plt.subplots(figsize=(12, 8))
    
    # Style the figure
    ax.set_facecolor('#f8f9fa')
    
    return fig, ax

np.random.seed(42)

# Patient vital signs over time
fig, axes = plt.subplots(2, 2, figsize=(14, 10))

# Blood pressure
ax1 = axes[0, 0]
days = range(1, 31)
systolic = 120 + 10 * np.sin(np.linspace(0, 4*np.pi, 30)) + np.random.randn(30) * 5
diastolic = 80 + 7 * np.sin(np.linspace(0, 4*np.pi, 30)) + np.random.randn(30) * 3

ax1.plot(days, systolic, color='#dc3545', linewidth=2, label='Systolic')
ax1.plot(days, diastolic, color='#17a2b8', linewidth=2, label='Diastolic')
ax1.axhline(y=120, color='#dc3545', linestyle='--', alpha=0.7, label='Normal (Systolic)')
ax1.axhline(y=80, color='#17a2b8', linestyle='--', alpha=0.7, label='Normal (Diastolic)')
ax1.fill_between(days, 0, systolic, where=np.array(systolic) > 120, 
                  alpha=0.3, color='#dc3545')
ax1.set_xlabel('Day', fontsize=11)
ax1.set_ylabel('Blood Pressure (mmHg)', fontsize=11)
ax1.set_title('Blood Pressure Monitoring', fontsize=12, fontweight='bold')
ax1.legend(fontsize=9, loc='upper right')
ax1.grid(True, alpha=0.3)

# Heart rate variability
ax2 = axes[0, 1]
hrv = 50 + np.random.randn(30) * 10
ax2.plot(days, hrv, color=CLINICAL_COLORS['baseline'], linewidth=2)
ax2.fill_between(days, hrv, alpha=0.3, color=CLINICAL_COLORS['baseline'])
ax2.axhline(y=50, color='gray', linestyle='--', alpha=0.5)
ax2.set_xlabel('Day', fontsize=11)
ax2.set_ylabel('HRV (ms)', fontsize=11)
ax2.set_title('Heart Rate Variability', fontsize=12, fontweight='bold')
ax2.grid(True, alpha=0.3)

# Treatment response comparison
ax3 = axes[1, 0]
baseline = 50 + np.random.randn(100) * 15
treatment = baseline + 10 + np.random.randn(100) * 10

ax3.hist(baseline, bins=20, alpha=0.6, color=CLINICAL_COLORS['baseline'], 
        label='Baseline', edgecolor='white')
ax3.hist(treatment, bins=20, alpha=0.6, color=CLINICAL_COLORS['treatment'], 
        label='Treatment', edgecolor='white')
ax3.set_xlabel('Response Score', fontsize=11)
ax3.set_ylabel('Frequency', fontsize=11)
ax3.set_title('Treatment Response Distribution', fontsize=12, fontweight='bold')
ax3.legend(fontsize=9)

# Adverse events by category
ax4 = axes[1, 1]
categories = ['Gastrointestinal', 'Cardiovascular', 'Neurological', 'Dermatological', 'Respiratory']
event_counts = [15, 8, 12, 20, 5]
colors_cat = [CLINICAL_COLORS['elevated'] if 15 <= c < 18 else CLINICAL_COLORS['high'] 
            if c >= 18 else '#17a2b8' for c in event_counts]

bars = ax4.barh(categories, event_counts, color=colors_cat, edgecolor='white')
ax4.set_xlabel('Number of Events', fontsize=11)
ax4.set_title('Adverse Events by Category', fontsize=12, fontweight='bold')

for bar, count in zip(bars, event_counts):
    ax4.text(count + 0.3, bar.get_y() + bar.get_height()/2., 
            str(count), va='center', fontsize=10)

plt.tight_layout()
plt.savefig('clinical_report.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Output Results

### Styled Output Examples

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd

# Generate sample outputs
np.random.seed(42)

print("=" * 70)
print("STYLING OUTPUT RESULTS")
print("=" * 70)

# Corporate style output
fig, ax = plt.subplots(figsize=(10, 6), facecolor='#f5f5f5')
ax.set_facecolor('white')
years = [2019, 2020, 2021, 2022, 2023]
values = [100, 120, 145, 170, 200]
ax.bar(years, values, color='#003366', edgecolor='white')
ax.set_xlabel('Year', fontsize=12, fontweight='bold')
ax.set_ylabel('Revenue ($M)', fontsize=12, fontweight='bold')
ax.set_title('Corporate Revenue', fontsize=14, fontweight='bold')
ax.grid(True, alpha=0.3)
plt.tight_layout()

plt.close()

# Scientific style output
fig, ax = plt.subplots(figsize=(6, 4.5))
ax.spines['top'].set_visible(False)
ax.spines['right'].set_visible(False)
x = np.linspace(0, 10, 50)
y = np.sin(x)
ax.plot(x, y, color='#2171b5', linewidth=1.5)
ax.set_xlabel('Time (s)', fontsize=10, fontfamily='serif')
ax.set_ylabel('Response (AU)', fontsize=10, fontfamily='serif')
ax.set_title('Scientific Plot', fontsize=12, fontweight='bold', fontfamily='serif')
ax.grid(True, alpha=0.3, linestyle=':')
plt.tight_layout()

plt.close()

# Dashboard style output
plt.style.use('dark_background')
fig, ax = plt.subplots(figsize=(10, 6), facecolor='#1e1e1e')
ax.set_facecolor('#2d2d2d')
dates = pd.date_range(start='2024-01-01', periods=30, freq='D')
values = np.cumsum(np.random.randn(30))
ax.plot(dates, values, color='#00d4ff', linewidth=2)
ax.fill_between(dates, values, alpha=0.3, color='#00d4ff')
ax.set_xlabel('Date', fontsize=11, color='white')
ax.set_ylabel('Metric Value', fontsize=11, color='white')
ax.set_title('Dashboard Metric', fontsize=14, fontweight='bold', color='white')
ax.tick_params(colors='white')
ax.grid(True, alpha=0.3)
plt.tight_layout()
plt.close()

print("\nStyle Types Created:")
print("  1. Corporate Style - Professional business presentation")
print("  2. Scientific Style - Publication-ready academic figures")
print("  3. Dashboard Style - Dark theme for monitoring")
print("  4. Custom Themes - Industry-specific styling")

print("\n" + "=" * 70)
```

---

## Visualization

### ASGII Styling Examples

#### Corporate Style

```
CORPORATE STYLE OUTPUT
Revenue ($M)
    |
200 |           +--------+
    |          |        |
150 |        +-+        |
    |        |          |
100 |      +-+          |
    |      |           |
 50 |    +-+          |
    |    |           +--------+
  0 +----+---------+-----+----> Year
        2019  2020  2021  2022  2023
        
Box: Bar Chart  | Line: Trend
```

#### Scientific Style

```
SCIENTIFIC STYLE OUTPUT
Response
    |        .---.
    |      .'     '.
    |     /         \
  0 +---+           '---.
    |   /               '.
    +--'------------------- time
      5    10    15    20
        
Points: Data | Dashed: Trend line
```

#### Dashboard Style

```
DASHBOARD STYLE OUTPUT
Metric Value
    |     /\
    |    /  \___
    |   /       \__
    |  /          \__
 20 +-+           \__ \
    |  \           \__ \
    |   \           \__\
    +----\------------\--> Date
          Jan   Feb   Mar
          
Style: Dark background, neon accents
```

#### Annotation Box

```
ANNOTATION EXAMPLE
         |      Box with arrow
         |         |
         v         v
        +---------+
        |         |
--------+         |
|       |         |
|  Data |         |
|       |         +--------
+--------+  Key Point
         |            |
         +-----------+
                   ^
                   |
              Text label
```

---

## Advanced Topics

### Custom Stylesheets

```python
import matplotlib.pyplot as plt
import matplotlib.style as style
import numpy as np

# Define custom style parameters
custom_params = {
    'figure.facecolor': 'white',
    'axes.facecolor': '#f5f5f5',
    'axes.edgecolor': '#cccccc',
    'axes.linewidth': 1,
    'grid.color': '#e0e0e0',
    'grid.linestyle': '-',
    'grid.linewidth': 0.5,
    'axes.labelsize': 10,
    'axes.titlesize': 12,
    'xtick.labelsize': 9,
    'ytick.labelsize': 9,
    'legend.fontsize': 9,
    'figure.titlesize': 14
}

# Apply custom style
plt.rcParams.update(custom_params)

# Plot with custom style
fig, ax = plt.subplots(figsize=(10, 6))
x = np.linspace(0, 10, 50)
ax.plot(x, np.sin(x), color='#1f77b4', linewidth=2)
ax.set_xlabel('X Values')
ax.set_ylabel('Y Values')
ax.set_title('Custom Stylesheet Example')
ax.grid(True, alpha=0.3)
plt.tight_layout()
plt.show()

# Save custom style
style.use('default')  # Reset to default
```

### Advanced Text Rendering

```python
import matplotlib.pyplot as plt
import matplotlib.patheffects as path_effects
import numpy as np

fig, ax = plt.subplots(figsize=(10, 6))

# Text with outline effect
text = ax.text(0.5, 0.5, 'Styled Text', fontsize=30, 
              ha='center', va='center',
              transform=ax.transAxes)

# Add outline effect
text.set_path_effects([
    path_effects.Stroke(linewidth=3, foreground='white'),
    path_effects.Normal()
])

# Add shadow effect
shadow = ax.text(0.52, 0.48, 'Drop Shadow', fontsize=30,
                ha='center', va='center',
                transform=ax.transAxes,
                color='gray')
shadow.set_path_effects([
    path_effects.withSimplePatchShadow(shadow_rgbColor='black', 
                                 alpha=0.5)
])

ax.set_xlim(0, 1)
ax.set_ylim(0, 1)
ax.axis('off')
ax.set_title('Text Effects', fontsize=14)

plt.tight_layout()
plt.show()
```

### Custom Grid Styling

```python
import matplotlib.pyplot as plt
import matplotlib.gridspec as gridspec
import numpy as np

fig = plt.figure(figsize=(12, 8))
gs = gridspec.GridSpec(2, 2, figure=fig, hspace=0.3, wspace=0.3)

ax1 = fig.add_subplot(gs[0, 0])
ax2 = fig.add_subplot(gs[0, 1])
ax3 = fig.add_subplot(gs[1, 0])
ax4 = fig.add_subplot(gs[1, 1])

# Custom grid configurations
for ax, style in [(ax1, 'major'), (ax2, 'minor'), 
                  (ax3, 'both'), (ax4, 'custom')]:
    x = np.linspace(0, 10, 50)
    ax.plot(x, np.sin(x))
    
    if style == 'major':
        ax.grid(True, which='major', linestyle='-', alpha=0.6)
    elif style == 'minor':
        ax.grid(True, which='minor', linestyle=':', alpha=0.3)
    elif style == 'both':
        ax.grid(True, which='both', linestyle='-', alpha=0.3)
    else:
        # Custom grid
        ax.xaxis.set_major_locator(plt.MultipleLocator(2))
        ax.xaxis.set_minor_locator(plt.MultipleLocator(0.5))
        ax.yaxis.set_major_locator(plt.MultipleLocator(0.5))
        ax.grid(True, which='major', linestyle='-', alpha=0.5)
        ax.grid(True, which='minor', linestyle=':', alpha=0.2)

plt.tight_layout()
plt.show()
```

---

## Conclusion

### Summary

This module covered comprehensive plot customization and styling:

1. **Fundamentals**: Object hierarchy, colors, fonts, legends, spines, ticks
2. **Implementation**: Corporate, scientific, dashboard, annotations styles
3. **Applications**: Banking risk dashboards, clinical reports
4. **Output Results**: Styled output examples
5. **Advanced Topics**: Custom stylesheets, text effects

### Key Takeaways

- Consistent styling is crucial for professional visualizations
- Customize every visual element for maximum impact
- Match style to purpose (publication, dashboard, report)
- Use themes for consistent branding

### Next Steps

Continue with:
- Module 04: Subplots and Figure Management
- Module 05: Interactive Plots and Annotations
- Module 06: Advanced Visualization Techniques

### Resources

- Matplotlib Styling: https://matplotlib.org/stable/tutorials/intermediate/artists.html
- Custom Colors: https://matplotlib.org/stable/gallery/color/color_demo.html
- Text Effects: https://matplotlib.org/stable/examples/text_labels_and_annotation/text effects.html

---

*End of Module 03: Customizing Plots and Styling*