# Subplots and Figure Management

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

This module covers subplot creation, figure management, and complex layout techniques in Matplotlib. Managing multiple plots within a single figure is essential for creating comprehensive dashboards, comparative analyses, and multi-panel figures for presentations and publications.

Understanding how to effectively arrange, size, and style subplots enables data scientists to create sophisticated visualizations that communicate multiple insights simultaneously. This module explores gridspec, subplot2grid, constrained layout, and other advanced layout management tools.

### Learning Objectives

By the end of this module, you will be able to:
- Create subplot grids using various methods
- Manage figure size and aspect ratios
- Handle shared axes and subplot spacing
- Use gridspec for complex layouts
- Apply constrained layout and tight_layout
- Create multi-figure visualizations

### Prerequisites

- Python 3.7+
- Matplotlib, NumPy, Pandas installed
- Basic understanding of plotting

---

## Fundamentals

### Subplot Basics

```python
import matplotlib.pyplot as plt
import numpy as np

# Method 1: plt.subplots()
# Creates figure and axes in one call
fig, axes = plt.subplots(2, 2, figsize=(10, 8))

# Access individual axes
axes[0, 0].plot([1, 2, 3], [1, 2, 3])
axes[0, 1].plot([1, 2, 3], [3, 2, 1])
axes[1, 0].plot([1, 2, 3], [1, 3, 2])
axes[1, 1].plot([1, 2, 3], [2, 1, 3])

plt.tight_layout()
plt.show()

# Method 2: plt.subplot()
# Add subplot to existing figure
fig = plt.figure(figsize=(10, 8))

ax1 = fig.add_subplot(221)
ax1.set_title('Subplot 1')
ax1.plot([1, 2, 3], [1, 2, 3])

ax2 = fig.add_subplot(222)
ax2.set_title('Subplot 2')
ax2.plot([1, 2, 3], [3, 2, 1])

ax3 = fig.add_subplot(223)
ax3.set_title('Subplot 3')
ax3.plot([1, 2, 3], [1, 3, 2])

ax4 = fig.add_subplot(224)
ax4.set_title('Subplot 4')
ax4.plot([1, 2, 3], [2, 1, 3])

plt.tight_layout()
plt.show()

# Method 3: fig.add_subplot() with specific position
fig = plt.figure(figsize=(10, 8))

ax1 = fig.add_subplot(2, 2, 1)
ax1.set_title('Position 1')

ax2 = fig.add_subplot(2, 2, 2)
ax2.set_title('Position 2')

plt.tight_layout()
plt.show()
```

### GridSpec Layout

```python
import matplotlib.pyplot as plt
import matplotlib.gridspec as gridspec
import numpy as np

# GridSpec for complex layouts
fig = plt.figure(figsize=(12, 8))
gs = gridspec.GridSpec(3, 3, figure=fig)

# Create subplots from grid spec
ax1 = fig.add_subplot(gs[0, :])          # Top row, all columns
ax2 = fig.add_subplot(gs[1, :2])         # Middle row, first two columns
ax3 = fig.add_subplot(gs[1, 2])        # Middle row, last column
ax4 = fig.add_subplot(gs[2, 0])       # Bottom row, first column
ax5 = fig.add_subplot(gs[2, 1])        # Bottom row, middle column
ax6 = fig.add_subplot(gs[2, 2])       # Bottom row, last column

# Plot data on each subplot
x = np.linspace(0, 10, 50)

ax1.plot(x, np.sin(x))
ax1.set_title('Top - Full Width')

ax2.plot(x, np.cos(x))
ax2.set_title('Middle Left')

ax3.plot(x, np.tan(x).clip(-5, 5))
ax3.set_title('Middle Right')

ax4.plot(x, np.exp(-x/10))
ax4.set_title('Bottom Left')

ax5.plot(x, np.log(x + 1))
ax5.set_title('Bottom Middle')

ax6.plot(x, np.sqrt(x))
ax6.set_title('Bottom Right')

plt.tight_layout()
plt.show()

# Complex grid with varying sizes
fig = plt.figure(figsize=(12, 8))
gs = gridspec.GridSpec(4, 4, figure=fig, hspace=0.3, wspace=0.3)

# Main plot (large)
ax_main = fig.add_subplot(gs[0:3, 0:3])
ax_main.plot(x, np.sin(x), 'b-')
ax_main.set_title('Main Plot')

# Side plot
ax_side = fig.add_subplot(gs[0:3, 3])
ax_side.plot(np.cos(x), x, 'r-')
ax_side.set_title('Side')

# Bottom plots (small)
for i in range(3):
    ax = fig.add_subplot(gs[3, i])
    ax.plot(x[i*16:(i+1)*16], 'o-')
    ax.set_title(f'Panel {i+1}')

plt.tight_layout()
plt.show()
```

### Subplot2grid

```python
import matplotlib.pyplot as plt
import numpy as np

# Using subplot2grid for flexible layout
fig = plt.figure(figsize=(12, 8))

# First row - two plots
ax1 = plt.subplot2grid((3, 3), (0, 0), colspan=2)
ax2 = plt.subplot2grid((3, 3), (0, 2))

# Second row - one plot
ax3 = plt.subplot2grid((3, 3), (1, 0), colspan=3)

# Third row - three plots
ax4 = plt.subplot2grid((3, 3), (2, 0))
ax5 = plt.subplot2grid((3, 3), (2, 1))
ax6 = plt.subplot2grid((3, 3), (2, 2))

x = np.linspace(0, 10, 50)

ax1.plot(x, np.sin(x))
ax1.set_title('Plot 1')

ax2.plot(x, np.cos(x))
ax2.set_title('Plot 2')

ax3.plot(x, np.tan(x).clip(-5, 5))
ax3.set_title('Plot 3')

ax4.plot(x, np.exp(-x/10))
ax4.set_title('Plot 4')

ax5.plot(x, np.log(x + 1))
ax5.set_title('Plot 5')

ax6.plot(x, np.sqrt(x))
ax6.set_title('Plot 6')

plt.tight_layout()
plt.show()
```

### Shared Axes

```python
import matplotlib.pyplot as plt
import numpy as np

# Shared x-axis
fig, axes = plt.subplots(2, 2, figsize=(10, 8), sharex=True)

x = np.linspace(0, 10, 50)

axes[0, 0].plot(x, np.sin(x))
axes[0, 1].plot(x, np.cos(x))
axes[1, 0].plot(x, np.tan(x).clip(-2, 2))
axes[1, 1].plot(x, np.exp(-x/5))

# Remove x labels for shared axis
for ax in axes[0, :]:
    ax.tick_params(labelbottom=False)
for ax in axes[:, 1]:
    ax.tick_params(labelleft=False)

plt.suptitle('Shared X-Axis Example', fontsize=14)
plt.tight_layout()
plt.show()

# Shared y-axis (side by side)
fig, axes = plt.subplots(1, 3, figsize=(12, 4), sharey=True)

for i, ax in enumerate(axes):
    ax.plot(np.sin(x + i*np.pi/3), x)
    ax.set_title(f'Plot {i+1}')

axes[1].tick_params(labelleft=False)
axes[2].tick_params(labelleft=False)

plt.suptitle('Shared Y-Axis Example', fontsize=14)
plt.tight_layout()
plt.show()
```

### Layout Management

```python
import matplotlib.pyplot as plt
import numpy as np

# Tight layout
fig, axes = plt.subplots(2, 2, figsize=(10, 8))

data = [np.random.randn(50) for _ in range(4)]
titles = ['Normal', 'Uniform', 'Exponential', 'Log-Normal']

for ax, d, title in zip(axes.flat, data, titles):
    ax.hist(d, bins=20, edgecolor='white')
    ax.set_title(title)

plt.tight_layout()
plt.savefig('tight_layout.png', dpi=300, bbox_inches='tight')
plt.show()

# Constrained layout (automatic spacing)
fig, axes = plt.subplots(2, 2, figsize=(10, 8))

for ax, d, title in zip(axes.flat, data, titles):
    ax.hist(d, bins=20, edgecolor='white')
    ax.set_title(title, fontsize=12)
    ax.set_xlabel('Value', fontsize=10)
    ax.set_ylabel('Frequency', fontsize=10)

fig.suptitle('Constrained Layout Example', fontsize=14, fontweight='bold')

plt.savefig('constrained_layout.png', dpi=300, bbox_inches='tight')
plt.show()

# GridSpec with constrained_layout
fig = plt.figure(figsize=(12, 8))
gs = fig.add_gridspec(3, 3, hspace=0.3, wspace=0.3)

ax1 = fig.add_subplot(gs[0, :])
ax2 = fig.add_subplot(gs[1, :2])
ax3 = fig.add_subplot(gs[1, 2])
ax4 = fig.add_subplot(gs[2, :])

ax1.set_title('Title 1')
ax2.set_title('Title 2')
ax3.set_title('Title 3')
ax4.set_title('Title 4')

plt.savefig('gridspec_constrained.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Implementation

### Comprehensive Layout Examples

### Example 1: Financial Dashboard

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import matplotlib.gridspec as gridspec

def create_financial_dashboard():
    """Create comprehensive financial dashboard"""
    fig = plt.figure(figsize=(16, 10))
    gs = gridspec.GridSpec(4, 4, figure=fig, hspace=0.3, wspace=0.3)
    
    fig.suptitle('Financial Performance Dashboard', 
               fontsize=16, fontweight='bold', y=0.98)
    
    return fig, gs

fig, gs = create_financial_dashboard()

# Generate random financial data
np.random.seed(42)
dates = pd.date_range(start='2024-01-01', periods=100, freq='D')

# Plot 1: Revenue and Profit (top, full width)
ax1 = fig.add_subplot(gs[0, :2])
revenue = 100 + np.cumsum(np.random.randn(100) * 2)
profit = revenue * 0.15 + np.random.randn(100) * 3

ax1.plot(dates, revenue, color='#1f77b4', linewidth=2, label='Revenue')
ax1.plot(dates, profit, color='#2ca02c', linewidth=2, label='Profit')
ax1.set_ylabel('$M', fontsize=10)
ax1.legend(loc='upper left', fontsize=9)
ax1.set_title('Revenue & Profit Trend')

# Plot 2: Growth Rate (top right)
ax2 = fig.add_subplot(gs[0, 2:])
growth = np.diff(revenue) / revenue[:-1] * 100
ax2.bar(dates[1:], growth, color=np.where(growth > 0, '#2ca02c', '#d62728'), 
        alpha=0.7)
ax2.axhline(y=0, color='black', linestyle='-', linewidth=0.5)
ax2.set_ylabel('%', fontsize=10)
ax2.set_title('Growth Rate')

# Plot 3: Expense Breakdown (middle left)
ax3 = fig.add_subplot(gs[1, :2])
expenses = {'Operations': 30, 'Marketing': 20, 'R&D': 15, 
           'Salaries': 25, 'Other': 10}
colors = ['#1f77b4', '#ff7f0e', '#2ca02c', '#d62728', '#9467bd']
ax3.pie(expenses.values(), labels=expenses.keys(), autopct='%1.1f%%',
        colors=colors, startangle=90)
ax3.set_title('Expense Breakdown')

# Plot 4: Monthly Comparison (middle right)
ax4 = fig.add_subplot(gs[1, 2:])
months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun']
actual = [100, 115, 108, 125, 132, 140]
budget = [95, 110, 115, 120, 125, 130]

x = np.arange(len(months))
width = 0.35

ax4.bar(x - width/2, actual, width, label='Actual', color='#1f77b4')
ax4.bar(x + width/2, budget, width, label='Budget', color='#ff7f0e')
ax4.set_xticks(x)
ax4.set_xticklabels(months)
ax4.legend(fontsize=9)
ax4.set_title('Actual vs Budget')

# Plot 5: Key Metrics (bottom left)
ax5 = fig.add_subplot(gs[2, :2])
metrics = ['Revenue Growth', 'Profit Margin', 'ROI', 'EBITDA', 'EPS']
values = [15.2, 18.5, 12.3, 22.1, 3.45]

y_pos = np.arange(len(metrics))
ax5.barh(y_pos, values, color=['#1f77b4', '#2ca02c', '#ff7f0e', 
                               '#d62728', '#9467bd'])
ax5.set_yticks(y_pos)
ax5.set_yticklabels(metrics)
ax5.set_xlabel('Value')
ax5.set_title('Key Metrics')

for i, v in enumerate(values):
    ax5.text(v + 0.3, i, f'{v}', va='center')

# Plot 6: Quarterly Performance (bottom right)
ax6 = fig.add_subplot(gs[2, 2:])
quarters = ['Q1', 'Q2', 'Q3', 'Q4']
revenue_q = [320, 350, 380, 420]
profit_q = [45, 52, 58, 65]

x = np.arange(len(quarters))
width = 0.35

ax6.plot(quarters, revenue_q, 'o-', color='#1f77b4', label='Revenue')
ax6.set_ylabel('Revenue ($M)', fontsize=10, color='#1f77b4')
ax6.tick_params(axis='y', labelcolor='#1f77b4')

ax6b = ax6.twinx()
ax6b.plot(quarters, profit_q, 's--', color='#2ca02c', label='Profit')
ax6b.set_ylabel('Profit ($M)', fontsize=10, color='#2ca02c')
ax6b.tick_params(axis='y', labelcolor='#2ca02c')

ax6.set_title('Quarterly Performance')

# Plot 7: Summary Statistics (bottom, full width)
ax7 = fig.add_subplot(gs[3, :])
ax7.axis('off')

summary_text = """
╔═══════════════════════════════════════════���═���════════════════════════════════════════════╗
║                           FINANCIAL SUMMARY SUMMARY                               ║
╠═══════════════╦═══════════════╦═══════════════╦══════════════╦═════════════════╣
║ Total Revenue ║ Total Profit  ║ Net Margin    ║ YTD Growth  ║ YTD ROI         ║
║   $1,245M    ║   $220M       ║    17.7%     ║   +15.2%    ║    12.3%       ║
╚═══════════════╩═══════════════╩═══════════════╩════════════╩═════════════════╝
"""

ax7.text(0.5, 0.5, summary_text, transform=ax7.transAxes,
         fontsize=9, fontfamily='monospace', ha='center', va='center',
         bbox=dict(boxstyle='round', facecolor='#f5f5f5', edgecolor='gray'))

plt.savefig('financial_dashboard.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 2: Scientific Multi-Panel Figure

```python
import matplotlib.pyplot as plt
import numpy as np
import matplotlib.gridspec as gridspec

def create_scientific_figure():
    """Create publication-ready scientific figure"""
    fig = plt.figure(figsize=(12, 10))
    gs = gridspec.GridSpec(3, 3, figure=fig, hspace=0.25, wspace=0.25)
    
    return fig, gs

fig, gs = create_scientific_figure()

# Set general style parameters
plt.rcParams.update({
    'font.size': 10,
    'axes.titlesize': 11,
    'axes.labelsize': 10,
    'xtick.labelsize': 9,
    'ytick.labelsize': 9,
    'legend.fontsize': 9,
    'figure.titlesize': 12
})

# Generate scientific data
np.random.seed(42)
x = np.linspace(0, 10, 100)
time_data = np.arange(100)

# Panel A: Time Series
ax_a = fig.add_subplot(gs[0, :2])
signal1 = np.sin(x/2) + 0.1 * np.random.randn(100)
signal2 = np.sin(x/2 + np.pi/4) + 0.1 * np.random.randn(100)

ax_a.plot(time_data, signal1, 'b-', label='Signal A', linewidth=1)
ax_a.plot(time_data, signal2, 'r-', label='Signal B', linewidth=1)
ax_a.set_xlabel('Time (s)')
ax_a.set_ylabel('Amplitude (mV)')
ax_a.set_title('(A) Time Series Data')
ax_a.legend(loc='upper right')
ax_a.grid(True, alpha=0.3)

# Panel B: Frequency Spectrum
ax_b = fig.add_subplot(gs[0, 2])
fft1 = np.abs(np.fft.fft(signal1))[:50]
fft2 = np.abs(np.fft.fft(signal2))[:50]

ax_b.plot(fft1, 'b-', label='Signal A', linewidth=1)
ax_b.plot(fft2, 'r-', label='Signal B', linewidth=1)
ax_b.set_xlabel('Frequency (Hz)')
ax_b.set_ylabel('Magnitude')
ax_b.set_title('(B) Frequency Spectrum')
ax_b.legend()
ax_b.grid(True, alpha=0.3)

# Panel C: Scatter Plot
ax_c = fig.add_subplot(gs[1, 0])
scatter_x = np.random.randn(200)
scatter_y = scatter_x * 0.8 + np.random.randn(200) * 0.5

colors = np.where(scatter_y > 0, 'blue', 'red')
ax_c.scatter(scatter_x, scatter_y, c=colors, alpha=0.5, s=20)
ax_c.set_xlabel('Variable X')
ax_c.set_ylabel('Variable Y')
ax_c.set_title('(C) Scatter Analysis')
ax_c.grid(True, alpha=0.3)

# Panel D: Distribution
ax_d = fig.add_subplot(gs[1, 1])
ax_d.hist(scatter_x, bins=30, alpha=0.6, label='X', color='blue')
ax_d.hist(scatter_y, bins=30, alpha=0.6, label='Y', color='red')
ax_d.set_xlabel('Value')
ax_d.set_ylabel('Frequency')
ax_d.set_title('(D) Distribution')
ax_d.legend()
ax_d.grid(True, alpha=0.3)

# Panel E: Correlation Heatmap
ax_e = fig.add_subplot(gs[1, 2])
data = np.random.randn(100, 4)
data[:, 1] = data[:, 0] * 0.7 + np.random.randn(100) * 0.5
data[:, 2] = -data[:, 0] * 0.5 + np.random.randn(100) * 0.8
data[:, 3] = data[:, 1] * 0.6 + np.random.randn(100) * 0.6

corr = np.corrcoef(data.T)

im = ax_e.imshow(corr, cmap='coolwarm', vmin=-1, vmax=1)
ax_e.set_xticks([0, 1, 2, 3])
ax_e.set_yticks([0, 1, 2, 3])
ax_e.set_xticklabels(['A', 'B', 'C', 'D'])
ax_e.set_yticklabels(['A', 'B', 'C', 'D'])
ax_e.set_title('(E) Correlation')
plt.colorbar(im, ax=ax_e, shrink=0.8)

# Add correlation values
for i in range(4):
    for j in range(4):
        ax_e.text(j, i, f'{corr[i,j]:.2f}', ha='center', va='center',
                 color='white' if abs(corr[i,j]) > 0.5 else 'black')

# Panel F: Combined Analysis
ax_f = fig.add_subplot(gs[2, :])

# Create combined plot
y1 = np.sin(x/3)
y2 = np.cos(x/3)
y3 = y1 + y2

ax_f.plot(time_data, y3, 'k-', linewidth=1.5, label='Combined')
ax_f.plot(time_data, y1, 'b--', linewidth=1, alpha=0.7, label='Component 1')
ax_f.plot(time_data, y2, 'r--', linewidth=1, alpha=0.7, label='Component 2')

ax_f.fill_between(time_data, 0, y3, where=(y3 > 0), alpha=0.2, color='blue')
ax_f.fill_between(time_data, 0, y3, where=(y3 <= 0), alpha=0.2, color='red')

ax_f.set_xlabel('Time (s)')
ax_f.set_ylabel('Amplitude')
ax_f.set_title('(F) Combined Signal Analysis')
ax_f.legend(loc='upper right')
ax_f.grid(True, alpha=0.3)
ax_f.set_xlim(0, 100)

plt.savefig('scientific_figure.png', dpi=300, bbox_inches='tight')
plt.show()
```

### Example 3: Healthcare Dashboard

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import matplotlib.gridspec as gridspec

def create_healthcare_dashboard():
    """Create healthcare monitoring dashboard"""
    fig = plt.figure(figsize=(16, 10))
    gs = gridspec.GridSpec(4, 4, figure=fig, hspace=0.3, wspace=0.3)
    
    fig.suptitle('Patient Monitoring Dashboard', 
               fontsize=16, fontweight='bold', y=0.98)
    
    return fig, gs

fig, gs = create_healthcare_dashboard()

# Generate patient data
np.random.seed(42)
n_readings = 200
timestamps = [f'{i:02d}:00' for i in range(24)]

# Vital signs data
heart_rate = 70 + 10 * np.sin(np.linspace(0, 4*np.pi, 24)) + np.random.randn(24) * 5
bp_systolic = 120 + 15 * np.sin(np.linspace(0, 4*np.pi, 24)) + np.random.randn(24) * 5
bp_diastolic = 80 + 10 * np.sin(np.linspace(0, 4*np.pi, 24)) + np.random.randn(24) * 3
oxygen = 97 + 2 * np.random.randn(24)
temperature = 98.6 + 0.5 * np.random.randn(24)

# Panel 1: Heart Rate
ax1 = fig.add_subplot(gs[0, :2])
ax1.plot(timestamps, heart_rate, 'r-', linewidth=2, marker='o')
ax1.axhline(y=70, color='green', linestyle='--', alpha=0.5, label='Normal')
ax1.axhline(y=100, color='red', linestyle='--', alpha=0.5, label='High')
ax1.fill_between(timestamps, 60, 100, alpha=0.1, color='green')
ax1.set_ylabel('Heart Rate (bpm)')
ax1.set_title('Heart Rate Monitoring')
ax1.legend(fontsize=8)
ax1.tick_params(axis='x', rotation=45)

# Panel 2: Blood Pressure
ax2 = fig.add_subplot(gs[0, 2:])
ax2.plot(timestamps, bp_systolic, 'r-', linewidth=2, marker='o', label='Systolic')
ax2.plot(timestamps, bp_diastolic, 'b-', linewidth=2, marker='s', label='Diastolic')
ax2.axhline(y=120, color='red', linestyle='--', alpha=0.5)
ax2.axhline(y=80, color='blue', linestyle='--', alpha=0.5)
ax2.set_ylabel('Pressure (mmHg)')
ax2.set_title('Blood Pressure')
ax2.legend(fontsize=8)
ax2.tick_params(axis='x', rotation=45)

# Panel 3: Oxygen Saturation
ax3 = fig.add_subplot(gs[1, :2])
colors_o2 = ['green' if o >= 95 else 'orange' if o >= 92 else 'red' for o in oxygen]
bars = ax3.bar(timestamps, oxygen, color=colors_o2, alpha=0.7, edgecolor='white')
ax3.axhline(y=95, color='green', linestyle='--', alpha=0.5)
ax3.set_ylabel('SpO2 (%)')
ax3.set_title('Oxygen Saturation')
ax3.set_ylim(85, 100)

# Panel 4: Temperature
ax4 = fig.add_subplot(gs[1, 2:])
ax4.plot(timestamps, temperature, 'orange', linewidth=2, marker='o')
ax4.axhline(y=98.6, color='green', linestyle='--', alpha=0.5, label='Normal')
ax4.axhline(y=99.5, color='red', linestyle='--', alpha=0.5, label='Fever')
ax4.fill_between(timestamps, 98, 99.5, alpha=0.1, color='orange')
ax4.set_ylabel('Temperature (°F)')
ax4.set_title('Body Temperature')
ax4.legend(fontsize=8)

# Panel 5: Alert Summary
ax5 = fig.add_subplot(gs[2, :])
ax5.axis('off')

# Calculate alerts
high_hr = np.sum(heart_rate > 100)
high_bp = np.sum(bp_systolic > 140)
low_o2 = np.sum(oxygen < 92)
high_temp = np.sum(temperature > 99.5)

alerts_text = f"""
┌─────────────────────────────────────────┐
│          ALERT SUMMARY                  │
├──────────────────────────────────────┤
│ High Heart Rate:    {high_hr:2d} readings (>{high_hr/24*100:.0f}%)          │
│ High Blood Pressure: {high_bp:2d} readings (>{high_bp/24*100:.0f}%)          │
│ Low SpO2:           {low_o2:2d} readings (>{low_o2/24*100:.0f}%)          │
│ High Temperature:    {high_temp:2d} readings (>{high_temp/24*100:.0f}%)          │
└─────────────────────────────────────────┘
"""

ax5.text(0.5, 0.5, alerts_text, transform=ax5.transAxes,
         fontsize=10, fontfamily='monospace', ha='center', va='center',
         bbox=dict(boxstyle='round', facecolor='white', edgecolor='gray'))

# Patient Info Panel
ax6 = fig.add_subplot(gs[3, :2])
ax6.axis('off')

patient_info = """
┌─────────────────────────────────────────┐
│         PATIENT INFORMATION             │
├───────────────────────────────────────┤
│ Patient ID:      PVT-2024-0847        │
│ Age:             58 years             │
│ Gender:          Male                 │
│ Room:            ICU-204               │
│ Admission:       2024-01-15           │
│ Primary Dx:      Acute MI            │
│ Status:          Stable               │
└───────��─��───────────────────────────────┘
"""

ax6.text(0.5, 0.5, patient_info, transform=ax6.transAxes,
         fontsize=10, fontfamily='monospace', ha='center', va='center',
         bbox=dict(boxstyle='round', facecolor='white', edgecolor='gray'))

# Treatment Panel
ax7 = fig.add_subplot(gs[3, 2:])
ax7.axis('off')

treatment = """
┌─────────────────────────────────────────┐
│        CURRENT TREATMENT                │
├──────────────────────────────────────┤
│ Medications:                         │
│   • Nitroglycerin 0.4mg SL PRN     │
│   • Aspirin 325mg daily            │
│   • Metoprolol 25mg BID           │
│   • Atorvastatin 80mg daily       │
│                                   │
│ Vitals: Check every 1 hour         │
│ Activity: Bed rest                │
└─────────────────────────────────────────┘
"""

ax7.text(0.5, 0.5, treatment, transform=ax7.transAxes,
         fontsize=10, fontfamily='monospace', ha='center', va='center',
         bbox=dict(boxstyle='round', facecolor='white', edgecolor='gray'))

plt.savefig('healthcare_dashboard.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Applications

### Banking Sector Application

```python
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import matplotlib.gridspec as gridspec

def create_banking_report():
    """Create comprehensive banking report"""
    fig = plt.figure(figsize=(16, 10))
    gs = gridspec.GridSpec(4, 4, figure=fig, hspace=0.3, wspace=0.3)
    
    fig.suptitle('Banking Portfolio Analysis', 
               fontsize=16, fontweight='bold', y=0.98)
    
    return fig, gs

fig, gs = create_banking_report()

# Generate banking data
np.random.seed(42)
dates = pd.date_range(start='2024-01-01', periods=30, freq='D')

# Portfolio metrics
portfolio_value = 1000000 + np.cumsum(np.random.randn(30) * 10000)
daily_returns = np.diff(portfolio_value) / portfolio_value[:-1] * 100

# Panel 1: Portfolio Value
ax1 = fig.add_subplot(gs[0, :2])
ax1.plot(dates, portfolio_value, 'b-', linewidth=2)
ax1.fill_between(dates, portfolio_value, alpha=0.3)
ax1.set_ylabel('Value ($)')
ax1.set_title('Portfolio Value Over Time')
ax1.tick_params(axis='x', rotation=45)

# Panel 2: Daily Returns
ax2 = fig.add_subplot(gs[0, 2:])
colors = ['green' if r > 0 else 'red' for r in daily_returns]
ax2.bar(dates[1:], daily_returns, color=colors, alpha=0.7)
ax2.axhline(y=0, color='black', linewidth=0.5)
ax2.set_ylabel('Return (%)')
ax2.set_title('Daily Returns')

# Asset allocation
ax3 = fig.add_subplot(gs[1, :2])
allocation = {'Equities': 45, 'Bonds': 30, 'Alternatives': 15, 'Cash': 10}
colors = ['#1f77b4', '#ff7f0e', '#2ca02c', '#d62728']
explode = (0.05, 0, 0, 0)
ax3.pie(allocation.values(), labels=allocation.keys(), 
        autopct='%1.1f%%', colors=colors, explode=explode,
        startangle=90)
ax3.set_title('Asset Allocation')

# Performance metrics
ax4 = fig.add_subplot(gs[1, 2:])
metrics = ['Total Return', 'Sharpe Ratio', 'Max Drawdown', 'Volatility']
values = [15.2, 1.45, -8.5, 12.3]

bars = ax4.barh(metrics, values, color=['#1f77b4', '#2ca02c', '#d62728', '#ff7f0e'])
ax4.set_xlabel('Value')
ax4.set_title('Performance Metrics')
for i, v in enumerate(values):
    ax4.text(v + 0.2, i, f'{v}', va='center')

# Risk analysis
ax5 = fig.add_subplot(gs[2, :])
var_95 = np.percentile(daily_returns, 5)
var_99 = np.percentile(daily_returns, 1)
ax5.hist(daily_returns, bins=30, edgecolor='white', alpha=0.7)
ax5.axvline(x=var_95, color='orange', linestyle='--', label=f'VaR 95%: {var_95:.2f}%')
ax5.axvline(x=var_99, color='red', linestyle='--', label=f'VaR 99%: {var_99:.2f}%')
ax5.set_xlabel('Daily Return (%)')
ax5.set_ylabel('Frequency')
ax5.set_title('Value at Risk Analysis')
ax5.legend(fontsize=8)

# Sector breakdown
ax6 = fig.add_subplot(gs[3, :2])
sectors = ['Tech', 'Finance', 'Healthcare', 'Energy', 'Consumer']
sector_returns = [18.5, 12.3, 15.8, 8.5, 11.2]
sector_colors = ['#1f77b4', '#ff7f0e', '#2ca02c', '#d62728', '#9467bd']
bars = ax6.bar(sectors, sector_returns, color=sector_colors, edgecolor='white')
ax6.set_ylabel('Return (%)')
ax6.set_title('Sector Performance')

for bar, val in zip(bars, sector_returns):
    ax6.text(bar.get_x() + bar.get_width()/2., val + 0.3,
            f'{val:.1f}%', ha='center')

plt.savefig('banking_report.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Output Results

### Summary Output

```python
print("=" * 70)
print("SUBPLOT AND FIGURE MANAGEMENT OUTPUT RESULTS")
print("=" * 70)

print("\n1. Grid Layout Types:")
print("   - 2x2 Subplot Grid: 4 panels")
print("   - 3x3 GridSpec: 9 panels with varying sizes")
print("   - Custom GridSpec: Flexible spanning layouts")

print("\n2. Layout Methods:")
print("   - plt.subplots(): Simple grids")
print("   - GridSpec: Complex layouts")
print("   - subplot2grid: Flexible positioning")
print("   - constrained_layout: Automatic spacing")

print("\n3. Figure Sizes (inches):")
print("   - Small: 6x4")
print("   - Medium: 10x8")
print("   - Large: 16x10")

print("\n4. Output Formats:")
print("   - PNG (raster)")
print("   - PDF (vector)")
print("   - SVG (vector)")

print("=" * 70)
```

---

## Visualization

### ASCII Layout Visualizations

#### 2x2 Grid

```
2x2 SUBPLOT GRID
+---------+---------+
| Plot 1  | Plot 2  |
|         |         |
+---------+---------+
| Plot 3  | Plot 4  |
|         |         |
+---------+---------+
```

#### GridSpec Layout

```
GRIDSPEC LAYOUT
+-------------+---------+
|             |         |
|  Full Row   |  Plot 2 |
|             |         |
+------+------+---------+
| P3   | P4  |         |
|      |     |  Plot 5 |
+------+-----+         |
|      Plot 6         |
+--------------------+
```

#### 3-Panel Scientific

```
SCIENTIFIC FIGURE
+--------------------+-------+
|                    | FFT   |
|    Time Series     | Panel |
|                    |       |
+--------------------+-------+
| Scatter | Dist | Heat  |
| Panel   | Panel| Panel |
+--------------------+-------+
|                                    |
|        Combined Analysis             |
|                                   |
+-----------------------------------+
```

---

## Advanced Topics

### Dynamic Subplot Creation

```python
import matplotlib.pyplot as plt
import numpy as np
import matplotlib.gridspec as gridspec

def create_dynamic_grid(n_plots):
    """Create dynamic subplot grid"""
    n_cols = int(np.ceil(np.sqrt(n_plots)))
    n_rows = int(np.ceil(n_plots / n_cols))
    
    fig, axes = plt.subplots(n_rows, n_cols, figsize=(4*n_cols, 3*n_rows))
    
    # Flatten axes for easy iteration
    axes = axes.flatten() if n_plots > 1 else [axes]
    
    # Hide unused axes
    for i in range(n_plots, len(axes)):
        axes[i].axis('off')
    
    return fig, axes[:n_plots]

# Create 7 subplots
fig, axes = create_dynamic_grid(7)

for i, ax in enumerate(axes):
    ax.plot(np.random.randn(50))
    ax.set_title(f'Plot {i+1}')

plt.tight_layout()
plt.show()
```

### Nested Subplots

```python
import matplotlib.pyplot as plt
import numpy as np
import matplotlib.gridspec as gridspec

fig = plt.figure(figsize=(12, 8))
gs = gridspec.GridSpec(2, 2, figure=fig)

# Main plot
ax_main = fig.add_subplot(gs[0, :])
x = np.linspace(0, 10, 50)
ax_main.plot(x, np.sin(x), 'b-')
ax_main.set_title('Main Plot')

# Nested subplot in main plot (inset)
from mpl_toolkits.axes_grid1.inset_locator import inset_axes
ax_inset = inset_axes(ax_main, width="30%", height="30%", loc='upper left')
ax_inset.plot(x, np.cos(x), 'r-')
ax_inset.set_title('Inset', fontsize=8)

# Side plots
ax_left = fig.add_subplot(gs[1, 0])
ax_left.plot(x, np.tan(x).clip(-5, 5), 'g-')
ax_left.set_title('Tanget')

ax_right = fig.add_subplot(gs[1, 1])
ax_right.hist(np.random.randn(100), bins=20)
ax_right.set_title('Histogram')

plt.tight_layout()
plt.show()
```

### Complex Layout with Colorbars

```python
import matplotlib.pyplot as plt
import numpy as np
import matplotlib.gridspec as gridspec
from mpl_toolkits.axes_grid1 import make_axes_locatable

fig = plt.figure(figsize=(12, 8))
gs = gridspec.GridSpec(2, 2, figure=fig, hspace=0.3, wspace=0.3)

# Plot with colorbar
for i in range(4):
    ax = fig.add_subplot(gs[i // 2, i % 2])
    
    data = np.random.randn(100, 100)
    im = ax.imshow(data, cmap='viridis')
    
    # Add colorbar
    divider = make_axes_locatable(ax)
    cax = divider.append_axes("right", size="5%", pad=0.1)
    plt.colorbar(im, cax=cax)
    
    ax.set_title(f'Heatmap {i+1}')

plt.savefig('heatmap_with_colorbars.png', dpi=300, bbox_inches='tight')
plt.show()
```

---

## Conclusion

### Summary

This module covered subplot and figure management:

1. **Fundamentals**: Subplot creation, gridspec, shared axes
2. **Implementation**: Financial, scientific, healthcare dashboards
3. **Applications**: Banking and healthcare reports
4. **Advanced Topics**: Dynamic layouts, nested subplots

### Key Takeaways

- Choose appropriate layout method for complexity
- Use constrained_layout for automatic spacing
- Share axes when comparing related data
- Use gridspec for complex layouts

### Next Steps

Continue with:
- Module 05: Interactive Plots and Annotations
- Module 06: Advanced Visualization Techniques

### Resources

- Matplotlib Subplots: https://matplotlib.org/stable/tutorials/subplots_and_axes.html
- GridSpec: https://matplotlib.org/stable/examples/subplots_axes_and_figures/grid_spec.html

---

*End of Module 04: Subplots and Figure Management*