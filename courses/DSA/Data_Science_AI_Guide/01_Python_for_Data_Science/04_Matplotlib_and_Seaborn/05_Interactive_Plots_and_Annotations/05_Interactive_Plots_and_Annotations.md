# Interactive Plots and Annotations

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

This module covers interactive plotting capabilities and advanced annotation techniques in Matplotlib. Interactive plots allow users to explore data dynamically, while annotations provide context and insights into visualizations. These skills are essential for creating dashboards, presentations, and data exploration tools that engage viewers and communicate findings effectively.

Modern data visualization increasingly requires interactivity - the ability to zoom, pan, hover for values, and click for details. Matplotlib provides multiple approaches for adding interactivity, from basic event handling to sophisticated widget-based interfaces.

### Learning Objectives

By the end of this module, you will be able to:
- Add and customize annotations (text, arrows, boxes)
- Implement hover annotations for data points
- Create interactive event handlers (click, key press)
- Build widgets for plot control (sliders, buttons)
- Save interactive HTML outputs
- Use mplcursors for hover information

### Prerequisites

- Python 3.7+
- Matplotlib, NumPy, Pandas installed
- Understanding of basic plotting concepts

---

## Fundamentals

### Basic Annotations

```python
import matplotlib.pyplot as plt
import numpy as np

# Basic text annotation
fig, ax = plt.subplots(figsize=(10, 6))

x = np.linspace(0, 10, 50)
y = np.sin(x)

ax.plot(x, y, 'b-', linewidth=2)

# Simple text annotation
ax.text(5, 0, 'Peak', fontsize=12, ha='center', va='bottom')

# Annotation with box
ax.annotate('Maximum', xy=(np.pi/2, 1), xytext=(np.pi, 0.5),
           fontsize=12, ha='center',
           bbox=dict(boxstyle='round', facecolor='yellow', alpha=0.5),
           arrowprops=dict(arrowstyle='->', color='red'))

plt.tight_layout()
plt.show()

# Arrow annotations
fig, ax = plt.subplots(figsize=(10, 6))

ax.plot(x, y, 'b-', linewidth=2)

# Arrow pointing to data
ax.annotate('Peak', xy=(np.pi/2, 1), xytext=(np.pi - 1, 0.5),
           fontsize=11, ha='center',
           arrowprops=dict(arrowstyle='->', color='red',
                         connectionstyle='arc3,rad=0.3'),
           bbox=dict(boxstyle='round,pad=0.3', facecolor='white',
                   edgecolor='red'))

ax.annotate('Trough', xy=(3*np.pi/2, -1), xytext=(2*np.pi - 1, -0.5),
           fontsize=11, ha='center',
           arrowprops=dict(arrowstyle='->', color='blue',
                         connectionstyle='arc3,rad=-0.3'),
           bbox=dict(boxstyle='round,pad=0.3', facecolor='white',
                   edgecolor='blue'))

plt.tight_layout()
plt.show()
```

### Annotation Styles

```python
import matplotlib.pyplot as plt
import matplotlib.patches as patches
import numpy as np

# Various annotation box styles
fig, ax = plt.subplots(figsize=(10, 6))

# Different box styles
styles = ['round', 'round4', 'sawtooth', 'roundtooth', 'darrow', 'circle']

for i, style in enumerate(styles):
    x_pos = (i % 3) * 3
    y_pos = 2 - (i // 3) * 2
    
    if style == 'circle':
        ax.annotate(f'Style: {style}', xy=(x_pos, y_pos), 
                   xytext=(x_pos, y_pos - 0.5),
                   fontsize=10,
                   bbox=dict(boxstyle=style, facecolor='lightblue'),
                   arrowprops=dict(arrowstyle='->'))
    else:
        ax.annotate(f'{style}', xy=(x_pos, y_pos),
                   xytext=(x_pos, y_pos - 0.5),
                   fontsize=10,
                   bbox=dict(boxstyle=style, facecolor='lightgreen'),
                   arrowprops=dict(arrowstyle='->'))

ax.set_xlim(-1, 10)
ax.set_ylim(-2, 3)
ax.axis('off')
ax.set_title('Annotation Box Styles', fontsize=14)

plt.tight_layout()
plt.show()

# Fancy arrow styles
fig, ax = plt.subplots(figsize=(10, 6))

x = np.linspace(0, 8, 50)
y = np.sin(x)

ax.plot(x, y, 'b-', linewidth=2)

# Fancy arrows
arrow_styles = ['->', '-[', '-|>', '<->', '<|-|>', 'fancy', 'simple', 'wedge']

for i, style in enumerate(arrow_styles):
    ax.annotate(f'style: {style}', 
               xy=(np.pi/2 + i*0.8, 1 - i*0.2),
               xytext=(4 + i*0.3, 1.3 - i*0.2),
               fontsize=8,
               arrowprops=dict(arrowstyle=style, color='red'),
               bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.5))

ax.set_xlim(0, 10)
ax.set_ylim(-0.5, 2)
ax.axis('off')

plt.tight_layout()
plt.show()
```

### Event Handling

```python
import matplotlib.pyplot as plt
import numpy as np

# Click events
fig, ax = plt.subplots(figsize=(10, 6))

x = np.random.randn(50)
y = np.random.randn(50)

ax.scatter(x, y, s=100, c='steelblue', alpha=0.7)

clicked_points = []

def on_click(event):
    if event.inaxes == ax:
        x, y = event.xdata, event.ydata
        clicked_points.append((x, y))
        
        # Mark clicked location
        ax.plot(x, y, 'ro', markersize=10, markerfacecolor='none',
               markeredgecolor='red', markeredgewidth=2)
        
        # Show coordinates
        ax.text(x, y, f'({x:.2f}, {y:.2f})', fontsize=8,
               xytext=(5, 5), textcoords='offset points')
        
        ax.figure.canvas.draw()

fig.canvas.mpl_connect('button_press_event', on_click)

ax.set_title('Click to add points (close to continue)')
plt.tight_layout()
plt.show()

# Key press events
fig, ax = plt.subplots(figsize=(10, 6))

x = np.linspace(0, 10, 100)
y = np.sin(x)

line, = ax.plot(x, y, 'b-', linewidth=2)

def on_key(event):
    if event.key == 'up':
        ax.set_ylim(ax.get_ylim()[0] * 1.1, ax.get_ylim()[1] * 1.1)
    elif event.key == 'down':
        ax.set_ylim(ax.get_ylim()[0] * 0.9, ax.get_ylim()[1] * 0.9)
    elif event.key == 'r':
        ax.set_xlim(0, 10)
        ax.set_ylim(-1.5, 1.5)
    ax.figure.canvas.draw()

fig.canvas.mpl_connect('key_press_event', on_key)

ax.set_title('Press UP/DOWN to zoom, R to reset')
plt.tight_layout()
plt.show()

# Motion events (hover)
fig, ax = plt.subplots(figsize=(10, 6))

np.random.seed(42)
x = np.random.randn(100)
y = x * 0.5 + np.random.randn(100) * 0.5

scatter = ax.scatter(x, y, s=100, c='steelblue', alpha=0.7)

hover_annotation = ax.annotate('', xy=(0, 0), xytext=(20, 20),
                           textcoords='offset points',
                           fontsize=10,
                           bbox=dict(boxstyle='round', facecolor='wheat'),
                           arrowprops=dict(arrowstyle='->'))

def on_motion(event):
    if event.inaxes == ax:
        contains, ind = scatter.contains(event)
        if contains:
            idx = ind['ind'][0]
            x_pt, y_pt = x[idx], y[idx]
            
            hover_annotation.xy = (x_pt, y_pt)
            hover_annotation.set_text(f'x: {x_pt:.2f}\ny: {y_pt:.2f}')
            hover_annotation.set_visible(True)
        else:
            hover_annotation.set_visible(False)
    else:
        hover_annotation.set_visible(False)
    
    fig.canvas.draw_idle()

fig.canvas.mpl_connect('motion_notify_event', on_motion)

ax.set_title('Hover over points to see coordinates')
plt.tight_layout()
plt.show()
```

### Widgets

```python
import matplotlib.pyplot as plt
import numpy as np
from matplotlib.widgets import Slider, Button, RadioButtons

# Slider widget
fig, ax = plt.subplots(figsize=(10, 6))
plt.subplots_adjust(left=0.1, bottom=0.25)

x = np.linspace(0, 10, 100)

# Initial parameters
freq = 1.0
amp = 1.0
phase = 0.0

line, = ax.plot(x, amp * np.sin(freq * x + phase), 'b-', linewidth=2)

ax.set_xlim(0, 10)
ax.set_ylim(-2.5, 2.5)

# Add sliders
ax_freq = plt.axes([0.2, 0.1, 0.65, 0.03])
ax_amp = plt.axes([0.2, 0.05, 0.65, 0.03])

slider_freq = Slider(ax_freq, 'Frequency', 0.1, 5.0, valinit=freq)
slider_amp = Slider(ax_amp, 'Amplitude', 0.1, 2.0, valinit=amp)

def update(val):
    freq = slider_freq.val
    amp = slider_amp.val
    line.set_ydata(amp * np.sin(freq * x + phase))
    fig.canvas.draw_idle()

slider_freq.on_changed(update)
slider_amp.on_changed(update)

ax.set_title('Interactive Frequency/Amplitude Control')
plt.tight_layout()
plt.show()

# Button widget
fig, ax = plt.subplots(figsize=(10, 6))
plt.subplots_adjust(bottom=0.2)

x = np.linspace(0, 10, 100)
y = np.sin(x)

ax.plot(x, y, 'b-', linewidth=2)
ax.set_xlim(0, 10)
ax.set_ylim(-1.5, 1.5)

# Add button
ax_button = plt.axes([0.7, 0.05, 0.1, 0.075])
button = Button(ax_button, 'Reset')

def reset(event):
    ax.set_xlim(0, 10)
    ax.set_ylim(-1.5, 1.5)
    fig.canvas.draw_idle()

button.on_clicked(reset)

ax.set_title('Click Reset to restore view')
plt.tight_layout()
plt.show()

# Radio buttons
fig, ax = plt.subplots(figsize=(10, 6))
plt.subplots_adjust(right=0.7)

x = np.linspace(0, 10, 100)

ax.plot(x, np.sin(x), label='sin')
ax.plot(x, np.cos(x), label='cos')
ax.plot(x, np.tan(x).clip(-2, 2), label='tan')
ax.legend()

ax_radio = plt.axes([0.8, 0.35, 0.15, 0.2])
radio = RadioButtons(ax_radio, ('sin', 'cos', 'tan'))

def update_label(label):
    for line in ax.lines:
        line.set_visible(line.get_label() == label)
    fig.canvas.draw_idle()

radio.on_clicked(update_label)

ax.set_title('Select function to display')
plt.tight_layout()
plt.show()
```

---

## Implementation

### Interactive Data Exploration

### Example 1: Stock Price Explorer

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from matplotlib.widgets import Slider, Button
from datetime import datetime, timedelta

np.random.seed(42)

# Generate stock data
n_days = 252
dates = pd.date_range(start='2024-01-01', periods=n_days, freq='D')

# Generate multiple stocks
stocks = {
    'Tech': 100 + np.cumsum(np.random.randn(n_days) * 2),
    'Finance': 80 + np.cumsum(np.random.randn(n_days) * 1.5),
    'Healthcare': 90 + np.cumsum(np.random.randn(n_days) * 1.8),
    'Energy': 60 + np.cumsum(np.random.randn(n_days) * 2.2)
}

fig, ax = plt.subplots(figsize=(12, 7))
plt.subplots_adjust(left=0.1, bottom=0.15)

# Plot first stock by default
lines = {}
for name, data in stocks.items():
    lines[name], = ax.plot(dates, data, linewidth=1.5, label=name, visible=False)

current_stock = 'Tech'
lines[current_stock].set_visible(True)

ax.set_xlabel('Date', fontsize=11)
ax.set_ylabel('Price ($)', fontsize=11)
ax.set_title('Stock Price Explorer', fontsize=14, fontweight='bold')
ax.legend(loc='upper left')
ax.grid(True, alpha=0.3)

# Slider for time window
ax_slider = plt.axes([0.15, 0.05, 0.7, 0.03])
slider = Slider(ax_slider, 'Window', 30, n_days, valinit=n_days, valstep=1)

def update_window(val):
    window = int(val)
    start_idx = max(0, n_days - window)
    
    for name, line in lines.items():
        line.set_xdata(dates[start_idx:])
        line.set_ydata(stocks[name][start_idx:])
    
    ax.set_xlim(dates[start_idx], dates[-1])
    
    y_all = np.concatenate([stocks[name][start_idx:] for name in stocks])
    ax.set_ylim(y_all.min() * 0.95, y_all.max() * 1.05)
    
    fig.canvas.draw_idle()

slider.on_changed(update_window)

# Radio buttons for stock selection
from matplotlib.widgets import RadioButtons as Radio

ax_radio = plt.axes([0.85, 0.6, 0.1, 0.15])
radio = Radio(ax_radio, list(stocks.keys()))

def select_stock(label):
    for name, line in lines.items():
        line.set_visible(name == label)
    fig.canvas.draw_idle()

radio.on_clicked(select_stock)

# Save button
ax_save = plt.axes([0.85, 0.05, 0.1, 0.05])
button = Button(ax_save, 'Save PNG')

def save_plot(event):
    plt.savefig('stock_explorer.png', dpi=300, bbox_inches='tight')

button.on_clicked(save_plot)

plt.tight_layout()
plt.show()
```

### Example 2: Interactive Scatter Matrix

```python
import matplotlib.pyplot as plt
import numpy as np
from matplotlib.widgets import Slider, RadioButtons

np.random.seed(42)

# Generate multidimensional data
n_samples = 500
data = {
    'Age': np.random.normal(45, 15, n_samples),
    'Income': np.random.lognormal(10.5, 0.8, n_samples),
    'Score': np.random.normal(70, 15, n_samples),
    'Experience': np.random.exponential(10, n_samples),
    'Savings': np.random.lognormal(9, 2, n_samples)
}

df = pd.DataFrame(data)

fig, axes = plt.subplots(2, 2, figsize=(14, 10))
axes = axes.flatten()

current_x = 'Age'
current_y = 'Income'
current_color = 'Score'

def scatter_plot(ax, x_col, y_col, color_col):
    ax.clear()
    scatter = ax.scatter(df[x_col], df[y_col], c=df[color_col],
                         cmap='viridis', alpha=0.6, s=30)
    ax.set_xlabel(x_col, fontsize=10)
    ax.set_ylabel(y_col, fontsize=10)
    ax.set_title(f'{x_col} vs {y_col}', fontsize=11)
    return scatter

for ax in axes:
    scatter_plot(ax, current_x, current_y, current_color)

# Add sliders for data range
ax_x = plt.axes([0.1, 0.02, 0.25, 0.03])
ax_y = plt.axes([0.4, 0.02, 0.25, 0.03])

slider_x = Slider(ax_x, 'Filter', 0, 100, valinit=100)
slider_y = Slider(ax_y, 'Max', 0, 100, valinit=100)

def update_scatter(val):
    filter_val = slider_x.val
    max_val = slider_y.val
    
    filtered = df[df['Age'] < df['Age'].quantile(filter_val/100)]
    filtered = filtered[filtered['Income'] < filtered['Income'].quantile(max_val/100)]
    
    for ax in axes:
        ax.clear()
        ax.scatter(filtered[current_x], filtered[current_y],
                  c=filtered[current_color], cmap='viridis',
                  alpha=0.6, s=30)
        ax.set_xlabel(current_x)
        ax.set_ylabel(current_y)
    
    fig.canvas.draw_idle()

slider_x.on_changed(update_scatter)
slider_y.on_changed(update_scatter)

plt.suptitle('Interactive Scatter Analysis', fontsize=14, fontweight='bold')
plt.tight_layout()
plt.show()
```

### Example 3: Financial Data Tooltip

```python
import matplotlib.pyplot as plt
import numpy as np
import mplcursors

np.random.seed(42)

# Generate financial time series
dates = pd.date_range(start='2024-01-01', periods=100, freq='D')
values = 100 + np.cumsum(np.random.randn(100))

fig, ax = plt.subplots(figsize=(12, 6))

# Line plot
line, = ax.plot(dates, values, 'b-', linewidth=1.5, label='Portfolio Value')
ax.fill_between(dates, values, alpha=0.2)

ax.set_xlabel('Date', fontsize=11)
ax.set_ylabel('Value ($)', fontsize=11)
ax.set_title('Hover for details', fontsize=14, fontweight='bold')
ax.grid(True, alpha=0.3)

# Add interactive cursor
cursor = mplcursors.cursor(line)

@cursor.connect("add")
def on_add(sel):
    sel.annotation.set_text(
        f"Date: {dates[sel.target.index].strftime('%Y-%m-%d')}\n"
        f"Value: ${values[sel.target.index]:,.2f}"
    )
    sel.annotation.get_bbox_patch().set(fc="white", alpha=0.9)

plt.tight_layout()
plt.show()

# Multi-line tooltip
fig, ax = plt.subplots(figsize=(12, 6))

lines = []
for i, name in enumerate(['Tech', 'Finance', 'Healthcare']):
    line, = ax.plot(dates, values * (1 + i * 0.1 + np.random.randn(100) * 0.05),
                  label=name, linewidth=1.5)
    lines.append(line)

ax.set_xlabel('Date', fontsize=11)
ax.set_ylabel('Value ($)', fontsize=11)
ax.set_title('Multi-line Tooltip', fontsize=14, fontweight='bold')
ax.legend()
ax.grid(True, alpha=0.3)

# Cursor for multiple lines
cursor = mplcursors.cursor(lines)

@cursor.connect("add")
def on_add(sel):
    idx = sel.target.index
    lines_data = [line.get_ydata()[idx] for line in lines]
    names = [line.get_label() for line in lines]
    
    text = f"Date: {dates[idx].strftime('%Y-%m-%d')}\n\n"
    for name, value in zip(names, lines_data):
        text += f"{name}: ${value:,.2f}\n"
    
    sel.annotation.set_text(text)
    sel.annotation.get_bbox_patch().set(fc="white", alpha=0.9)

plt.tight_layout()
plt.show()
```

### Example 4: Annotated Heatmap

```python
import matplotlib.pyplot as plt
import numpy as np
from matplotlib.colors import LogNorm

np.random.seed(42)

# Generate correlation matrix
variables = ['Revenue', 'Profit', 'Customer', 'Growth', 'Risk', 'Market']
n = len(variables)
data = np.random.randn(200, n)
corr = np.corrcoef(data.T)

fig, ax = plt.subplots(figsize=(10, 8))

# Create heatmap
im = ax.imshow(corr, cmap='coolwarm', vmin=-1, vmax=1)

# Add annotations
for i in range(n):
    for j in range(n):
        color = 'white' if abs(corr[i, j]) > 0.5 else 'black'
        ax.text(j, i, f'{corr[i, j]:.2f}', ha='center', va='center',
               color=color, fontsize=10, fontweight='bold')

# Add colorbar
cbar = plt.colorbar(im, ax=ax, shrink=0.8)
cbar.set_label('Correlation', fontsize=11)

# Set labels
ax.set_xticks(np.arange(n))
ax.set_yticks(np.arange(n))
ax.set_xticklabels(variables)
ax.set_yticklabels(variables)

ax.set_title('Correlation Heatmap with Annotations', fontsize=14, fontweight='bold')

plt.tight_layout()
plt.show()
```

---

## Applications

### Banking Sector Application

#### Application 1: Interactive Credit Risk Dashboard

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from matplotlib.widgets import Slider, Button, RadioButtons

np.random.seed(42)

# Generate credit data
n_customers = 1000
credit_data = pd.DataFrame({
    'customer_id': range(n_customers),
    'credit_score': np.random.normal(650, 100, n_customers).clip(300, 850),
    'income': np.random.lognormal(10.5, 0.8, n_customers),
    'debt_ratio': np.random.uniform(0.1, 0.5, n_customers),
    'default': (np.random.rand(n_customers) < 0.1).astype(int)
})

fig = plt.figure(figsize=(14, 10))
gs = fig.add_gridspec(3, 3, hspace=0.3, wspace=0.3)

# Main scatter plot
ax_main = fig.add_subplot(gs[0:2, 0:2])
sc = ax_main.scatter(credit_data['credit_score'], credit_data['income'],
                    c=credit_data['default'], cmap='RdYlGn_r',
                    alpha=0.5, s=20)

ax_main.set_xlabel('Credit Score', fontsize=11)
ax_main.set_ylabel('Annual Income ($)', fontsize=11)
ax_main.set_title('Credit Risk Analysis', fontsize=14, fontweight='bold')
ax_main.set_yscale('log')
ax_main.grid(True, alpha=0.3)

# Add hover annotations
cursor = mplcursors.cursor(sc)

@cursor.connect("add")
def on_add(sel):
    idx = sel.target.index
    score = credit_data['credit_score'].iloc[idx]
    income = credit_data['income'].iloc[idx]
    default = credit_data['default'].iloc[idx]
    
    sel.annotation.set_text(
        f"Score: {score:.0f}\n"
        f"Income: ${income:,.0f}\n"
        f"Default: {'Yes' if default else 'No'}"
    )
    sel.annotation.get_bbox_patch().set(fc="white", alpha=0.9)

# Distribution plot
ax_dist = fig.add_subplot(gs[0, 2])
ax_dist.hist(credit_data['default'] == 0, bins=20, alpha=0.7)
ax_dist.set_title('Score Distribution')
ax_dist.set_xlabel('Score')
ax_dist.set_ylabel('Count')

# Slider for filtering
ax_slider = plt.axes([0.15, 0.05, 0.6, 0.03])
slider = Slider(ax_slider, 'Min Score', 300, 850, valinit=300)

def update_filter(val):
    min_score = val
    filtered = credit_data[credit_data['credit_score'] >= min_score]
    
    ax_main.collections.clear()
    ax_main.scatter(filtered['credit_score'], filtered['income'],
                  c=filtered['default'], cmap='RdYlGn_r',
                  alpha=0.5, s=20)
    
    ax_dist.clear()
    ax_dist.hist(filtered['credit_score'], bins=20, alpha=0.7)
    ax_dist.set_title(f'n={len(filtered)}')
    
    fig.canvas.draw_idle()

slider.on_changed(update_filter)

# Save button
ax_save = plt.axes([0.8, 0.05, 0.1, 0.05])
button = Button(ax_save, 'Save')

def save_data(event):
    plt.savefig('credit_risk_interactive.png', dpi=300, bbox_inches='tight')

button.on_clicked(save_data)

plt.suptitle('Interactive Credit Risk Dashboard', fontsize=16, fontweight='bold')
plt.tight_layout()
plt.show()
```

### Healthcare Sector Application

#### Application 2: Patient Vitals Explorer

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from matplotlib.widgets import Slider, Button

np.random.seed(42)

# Generate patient vitals
n_readings = 500
vitals = pd.DataFrame({
    'timestamp': pd.date_range(start='2024-01-01', periods=n_readings, freq='15min'),
    'heart_rate': 70 + 10 * np.sin(np.linspace(0, 20*np.pi, n_readings)) + np.random.randn(n_readings) * 5,
    'blood_pressure_systolic': 120 + 15 * np.sin(np.linspace(0, 15*np.pi, n_readings)) + np.random.randn(n_readings) * 10,
    'blood_pressure_diastolic': 80 + 10 * np.sin(np.linspace(0, 15*np.pi, n_readings)) + np.random.randn(n_readings) * 5,
    'oxygen_saturation': 97 + 2 * np.random.randn(n_readings),
    'temperature': 98.6 + 0.5 * np.random.randn(n_readings),
    'alert': ((np.random.rand(n_readings) < 0.05) | 
              (vitals['heart_rate'] > 100) | 
              (vitals['oxygen_saturation'] < 93)).astype(int)
})

fig, axes = plt.subplots(4, 1, figsize=(12, 14))
plt.subplots_adjust(hspace=0.3)

# Plot each vital sign
vital_signs = [
    ('heart_rate', 'Heart Rate (bpm)', 'red'),
    ('blood_pressure_systolic', 'Blood Pressure (mmHg)', 'blue'),
    ('oxygen_saturation', 'SpO2 (%)', 'green'),
    ('temperature', 'Temperature (°F)', 'orange')
]

lines = []
for (col, label, color), ax in zip(vital_signs, axes):
    line, = ax.plot(vitals['timestamp'], vitals[col], color=color, linewidth=1)
    lines.append(line)
    
    ax.set_ylabel(label, fontsize=10)
    ax.grid(True, alpha=0.3)
    
    # Highlight alerts
    alerts = vitals[vitals['alert'] == 1]
    if len(alerts) > 0:
        ax.scatter(alerts['timestamp'], alerts[col], color='red', 
                  s=50, marker='x', alpha=0.7)

axes[-1].set_xlabel('Time', fontsize=11)

# Time window slider
ax_slider = plt.axes([0.15, 0.02, 0.7, 0.03])
slider = Slider(ax_slider, 'Hours', 1, 72, valinit=24)

def update_time(val):
    hours = int(val)
    mask = vitals['timestamp'] >= vitals['timestamp'].max() - pd.Timedelta(hours=hours)
    
    for ax in axes:
        ax.set_xlim(vitals[mask]['timestamp'].min(), vitals[mask]['timestamp'].max())
    
    fig.canvas.draw_idle()

slider.on_changed(update_time)

# Mark button
ax_mark = plt.axes([0.85, 0.02, 0.1, 0.05])
button = Button(ax_mark, 'Mark')

def mark_interesting(event):
    # Mark significant points
    for (col, _, _), ax in zip(vital_signs, axes):
        max_val = vitals[col].max()
        max_idx = vitals[col].idxmax()
        ax.plot(vitals.loc[max_idx, 'timestamp'], max_val, 'ko', 
               markersize=10, markerfacecolor='none')
    fig.canvas.draw_idle()

button.on_clicked(mark_interesting)

plt.suptitle('Patient Vitals Explorer', fontsize=14, fontweight='bold')
plt.tight_layout()
plt.savefig('vitals_explorer.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Output Results

### Interactive Output Summary

```python
print("=" * 70)
print("INTERACTIVE PLOTS AND ANNOTATIONS - OUTPUT SUMMARY")
print("=" * 70)

print("\n1. Annotation Types:")
print("   - Text annotations with boxes")
print("   - Arrow annotations")
print("   - Hover tooltips")
print("   - Dynamic labels")

print("\n2. Interactive Features:")
print("   - Click events")
print("   - Hover/motion events")
print("   - Key press events")
print("   - Slider widgets")
print("   - Button widgets")
print("   - Radio buttons")

print("\n3. Applications:")
print("   - Financial dashboards")
print("   - Healthcare monitoring")
print("   - Data exploration tools")

print("\n4. Output Formats:")
print("   - Static PNG/PDF")
print("   - Interactive HTML")
print("   - Widget-enabled figures")

print("=" * 70)
```

---

## Visualization

### ASCII Interactive Examples

#### Annotation Types

```
ANNOTATION STYLES
                
     Simple Text     Box with Arrow      Fancy Box
    
        Point           ----->               _[_
        |              |                  |  |___
        v              v                  v     |__

```

#### Widget Layout

```
WIDGET LAYOUT
+------------------------------------------+
|                                          |
|           Plot Area                      |
|                                          |
+------------------------------------------+
| [Slider=============================]    |
|                                          |
| [Radio] [Button] [Button]                |
+------------------------------------------+
```

---

## Advanced Topics

### Saving Interactive HTML

```python
import matplotlib.pyplot as plt
import numpy as np
import mpld3

# Create interactive plot
fig, ax = plt.subplots(figsize=(10, 6))

x = np.linspace(0, 10, 50)
y = np.sin(x)

ax.plot(x, y, 'b-', linewidth=2, label='sin(x)')
ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_title('Interactive HTML')
ax.legend()

# Save as interactive HTML
mpld3.save_html(fig, 'interactive_plot.html')

# Alternative: plotly
import plotly.express as px

df = pd.DataFrame({'x': x, 'y': y})
fig = px.line(df, x='x', y='y', title='Interactive Plotly')

# Save as HTML
fig.write_html('plotly_interactive.html')
```

### Advanced Event Handling

```python
import matplotlib.pyplot as plt
import numpy as np
from matplotlib.backend_tools import ToolBase

# Custom toolbar
fig, ax = plt.subplots(figsize=(10, 6))

x = np.linspace(0, 10, 50)
y = np.sin(x)

ax.plot(x, y, 'b-', linewidth=2)

# Pan tool (built-in)
ax.set_navigate_mode('PAN')

# Zoom rectangle
from matplotlib.widgets import LassoSelector

selector = LassoSelector(ax)

def select(vertices):
    print(f"Selected region: {vertices}")

selector.connect_event('on_select', select)

plt.tight_layout()
plt.show()
```

### Linked Subplots

```python
import matplotlib.pyplot as plt
import numpy as np

fig, axes = plt.subplots(2, 2, figsize=(12, 10))

x = np.linspace(0, 10, 50)

# Create four plots
for i, ax in enumerate(axes.flat):
    ax.plot(x, np.sin(x + i * np.pi/4), 'b-', linewidth=1)
    ax.set_title(f'Phase {i * 45}°')
    ax.grid(True, alpha=0.3)

# Link all axes for synchronized navigation
ax_linked = axes.flat[0]
for ax in axes.flat[1:]:
    ax_linked.get_shared_x_axes().join(ax, ax_linked)
    ax_linked.get_shared_y_axes().join(ax, ax_linked)

plt.suptitle('Linked Subplots (zoom one, all zoom)', fontsize=14)
plt.tight_layout()
plt.show()
```

---

## Conclusion

### Summary

This module covered interactive plots and annotations:

1. **Fundamentals**: Text, arrows, boxes, events, widgets
2. **Implementation**: Stock explorer, scatter matrix, tooltips
3. **Applications**: Banking credit dashboard, healthcare vitals
4. **Advanced Topics**: HTML output, linked subplots

### Key Takeaways

- Interactive plots enable data exploration
- Annotations provide context and insights
- Widgets allow parameter adjustment
- Event handling enables custom interactions

### Next Steps

Continue with:
- Module 06: Advanced Visualization Techniques

### Resources

- Matplotlib Events: https://matplotlib.org/stable/users/event_handling.html
- Widgets: https://matplotlib.org/stable/examples/widgets/index.html
- mplcursors: https://mplcursors.readthedocs.io/

---

*End of Module 05: Interactive Plots and Annotations*