# Advanced Visualization Techniques

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

This final module covers advanced visualization techniques that push the boundaries of what Matplotlib and Seaborn can accomplish. Topics include 3D plotting, complex animations, geographic visualizations, network graphs, and integrating with other visualization libraries like Plotly, Bokeh, and Altair.

These advanced techniques are essential for creating cutting-edge data visualizations that stand out in presentations, publications, and interactive applications. They enable data scientists to explore multi-dimensional data and communicate complex insights effectively.

### Learning Objectives

By the end of this module, you will be able to:
- Create 3D plots and surface visualizations
- Build animated visualizations
- Create geographic and network visualizations
- Integrate with alternative visualization libraries
- Apply specialized techniques for specific domains

### Prerequisites

- Python 3.7+
- Matplotlib, NumPy, Pandas, Seaborn installed
- Understanding of all previous modules

---

## Fundamentals

### 3D Plotting

```python
import matplotlib.pyplot as plt
import numpy as np
from mpl_toolkits.mplot3d import Axes3D

# Basic 3D line plot
fig = plt.figure(figsize=(10, 8))
ax = fig.add_subplot(111, projection='3d')

# Generate data
t = np.linspace(0, 10, 100)
x = np.sin(t)
y = np.cos(t)
z = t

ax.plot(x, y, z, 'b-', linewidth=2, label='Helix')
ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')
ax.set_title('3D Line Plot')
ax.legend()

plt.tight_layout()
plt.show()

# 3D scatter plot
fig = plt.figure(figsize=(10, 8))
ax = fig.add_subplot(111, projection='3d')

# Generate random 3D data
n = 200
x = np.random.randn(n)
y = np.random.randn(n)
z = np.random.randn(n)
c = np.random.randn(n)

scatter = ax.scatter(x, y, z, c=c, cmap='viridis', s=50, alpha=0.6)
ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')
ax.set_title('3D Scatter Plot')

# Add colorbar
cbar = plt.colorbar(scatter, ax=ax, shrink=0.5)
cbar.set_label('Value')

plt.tight_layout()
plt.show()

# 3D surface plot
fig = plt.figure(figsize=(10, 8))
ax = fig.add_subplot(111, projection='3d')

# Create meshgrid
X = np.linspace(-5, 5, 50)
Y = np.linspace(-5, 5, 50)
X, Y = np.meshgrid(X, Y)
Z = np.sin(np.sqrt(X**2 + Y**2))

# Plot surface
surf = ax.plot_surface(X, Y, Z, cmap='coolwarm', edgecolor='none', alpha=0.8)
ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')
ax.set_title('3D Surface Plot')

# Add colorbar
cbar = plt.colorbar(surf, ax=ax, shrink=0.5)
cbar.set_label('Value')

plt.tight_layout()
plt.show()

# Wireframe
fig = plt.figure(figsize=(10, 8))
ax = fig.add_subplot(111, projection='3d')

ax.plot_wireframe(X, Y, Z, color='blue', alpha=0.3)
ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')
ax.set_title('Wireframe Plot')

plt.tight_layout()
plt.show()
```

### Contour Plots

```python
import matplotlib.pyplot as plt
import numpy as np

# Create data for contour
x = np.linspace(-3, 3, 100)
y = np.linspace(-3, 3, 100)
X, Y = np.meshgrid(x, y)
Z = X**2 + Y**2

fig, axes = plt.subplots(2, 2, figsize=(12, 10))

# Filled contour
ax1 = axes[0, 0]
cs1 = ax1.contourf(X, Y, Z, levels=20, cmap='viridis')
ax1.set_title('Filled Contour')
plt.colorbar(cs1, ax=ax1)

# Contour lines
ax2 = axes[0, 1]
cs2 = ax2.contour(X, Y, Z, levels=10, cmap='viridis')
ax2.clabel(cs2, inline=True, fontsize=8)
ax2.set_title('Contour Lines')

# Multiple contours
ax3 = axes[1, 0]
cs3 = ax3.contourf(X, Y, Z, levels=[0, 2, 5, 10, 20], cmap='RdYlBu_r')
ax3.set_title('Multiple Levels')
plt.colorbar(cs3, ax=ax3)

# Combined contour and scatter
ax4 = axes[1, 1]
ax4.contourf(X, Y, Z, levels=20, cmap='viridis', alpha=0.5)
cs4 = ax4.contour(X, Y, Z, levels=10, colors='white', linewidths=0.5)
ax4.clabel(cs4, inline=True, fontsize=8)

# Add sample points
sample_x = np.random.uniform(-2, 2, 20)
sample_y = np.random.uniform(-2, 2, 20)
ax4.scatter(sample_x, sample_y, c='red', s=30, edgecolors='white')
ax4.set_title('Contour with Points')

plt.tight_layout()
plt.show()
```

### Quiver and Vector Fields

```python
import matplotlib.pyplot as plt
import numpy as np

# Create vector field
x = np.linspace(-2, 2, 20)
y = np.linspace(-2, 2, 20)
X, Y = np.meshgrid(x, y)

# Create vector components
U = -Y / (X**2 + Y**2 + 0.1)
V = X / (X**2 + Y**2 + 0.1)

fig, axes = plt.subplots(1, 2, figsize=(14, 6))

# Quiver plot
ax1 = axes[0]
ax1.quiver(X, Y, U, V, color='blue', alpha=0.7)
ax1.set_xlabel('X')
ax1.set_ylabel('Y')
ax1.set_title('Quiver Plot')
ax1.set_xlim(-2.5, 2.5)
ax1.set_ylim(-2.5, 2.5)
ax1.set_aspect('equal')

# Stream plot
ax2 = axes[1]
ax2.streamplot(X, Y, U, V, color=np.sqrt(U**2 + V**2), cmap='viridis', linewidth=1)
ax2.set_xlabel('X')
ax2.set_ylabel('Y')
ax2.set_title('Stream Plot')
ax2.set_xlim(-2.5, 2.5)
ax2.set_ylim(-2.5, 2.5)
ax2.set_aspect('equal')

plt.tight_layout()
plt.show()
```

### Polar Plots

```python
import matplotlib.pyplot as plt
import numpy as np

# Generate polar data
theta = np.linspace(0, 2*np.pi, 100)
r1 = 1 + np.sin(theta)
r2 = 1 + 0.5*np.sin(2*theta)
r3 = 0.5 + 0.5*np.sin(3*theta)

fig, axes = plt.subplots(1, 3, figsize=(15, 5), subplot_kw={'projection': 'polar'})

# Polar line plot
ax1 = axes[0]
ax1.plot(theta, r1, 'b-', linewidth=2)
ax1.fill(theta, r1, alpha=0.3, color='blue')
ax1.set_title('Polar Line')

# Polar bar chart
ax2 = axes[1]
categories = ['A', 'B', 'C', 'D', 'E']
values = [3, 4, 2, 5, 3]
angles = np.linspace(0, 2*np.pi, len(categories), endpoint=False)
ax2.bar(angles, values, width=0.5, alpha=0.7)
ax2.set_xticks(angles)
ax2.set_xticklabels(categories)
ax2.set_title('Polar Bar')

# Polar scatter
ax3 = axes[2]
r = np.random.rand(50)
theta_rand = np.random.rand(50) * 2 * np.pi
ax3.scatter(theta_rand, r, c=r, cmap='viridis', s=100, alpha=0.7)
ax3.set_title('Polar Scatter')

plt.tight_layout()
plt.show()
```

### Error Ellipses

```python
import matplotlib.pyplot as plt
import numpy as np
from matplotlib.patches import Ellipse

# Generate correlated data
n = 200
mean = [1, 2]
cov = [[1, 0.8], [0.8, 1]]  # Positive correlation
x = np.random.multivariate_normal(mean, cov, n)

fig, ax = plt.subplots(figsize=(10, 8))

ax.scatter(x[:, 0], x[:, 1], c='blue', alpha=0.5, s=30, label='Data')

# Calculate covariance
cov_matrix = np.cov(x[:, 0], x[:, 1])

# Confidence levels
for confidence in [0.5, 0.75, 0.95]:
    # Chi-squared values for confidence levels
    chi2_val = {0.5: 1.386, 0.75: 2.834, 0.95: 5.991}[confidence]
    
    # Eigenvalues and eigenvectors
    eigenvalues, eigenvectors = np.linalg.eig(cov_matrix)
    angle = np.arctan2(eigenvectors[1, 0], eigenvectors[0, 0]) * 180 / np.pi
    
    # Width and height
    width = 2 * np.sqrt(chi2_val * eigenvalues[0])
    height = 2 * np.sqrt(chi2_val * eigenvalues[1])
    
    # Create ellipse
    ellipse = Ellipse(xy=mean, width=width, height=height, angle=angle,
                    fill=False, edgecolor='red', linewidth=2,
                    label=f'{int(confidence*100)}% CI')
    ax.add_patch(ellipse)

ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_title('Error Ellipses')
ax.legend()
ax.set_xlim(-3, 5)
ax.set_ylim(-1, 5)
ax.set_aspect('equal')
ax.grid(True, alpha=0.3)

plt.tight_layout()
plt.show()
```

---

## Implementation

### Advanced Examples

### Example 1: Climate Data Visualization

```python
import matplotlib.pyplot as plt
import numpy as np
import matplotlib.colors as mcolors

# Generate climate grid data
lat = np.linspace(-90, 90, 50)
lon = np.linspace(-180, 180, 50)
LAT, LON = np.meshgrid(lat, lon)

# Temperature anomaly (degrees C)
temp_anomaly = 30 * np.sin(LAT * np.pi / 180) * np.cos(LON * np.pi / 360)

fig = plt.figure(figsize=(14, 10))

# Create 3D globe
ax = fig.add_subplot(111, projection='3d')

# Convert to spherical coordinates
r = 100
theta = LAT * np.pi / 180
phi = LON * np.pi / 180

x_sphere = r * np.sin(theta) * np.cos(phi)
y_sphere = r * np.sin(theta) * np.sin(phi)
z_sphere = r * np.cos(theta)

# Plot surface
surf = ax.plot_surface(x_sphere, y_sphere, z_sphere, facecolors=plt.cm.RdBu_r((temp_anomaly - temp_anomaly.min()) / (temp_anomaly.max() - temp_anomaly.min())),
                     rstride=1, cstride=1, alpha=0.9, shade=False)

ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')
ax.set_title('Global Temperature Anomaly on 3D Globe', fontsize=14)
ax.set_box_aspect([1, 1, 1])

# Add colorbar
mappable = plt.cm.ScalarMappable(cmap='RdBu_r')
mappable.set_array(temp_anomaly)
mappable.set_clim(temp_anomaly.min(), temp_anomaly.max())
cbar = plt.colorbar(mappable, ax=ax, shrink=0.6, pad=0.1)
cbar.set_label('Temperature Anomaly (°C)')

plt.tight_layout()
plt.show()

# Equirectangular projection (2D)
fig, ax = plt.subplots(figsize=(14, 6))

im = ax.imshow(temp_anomaly, extent=[-180, 180, -90, 90], 
             cmap='RdBu_r', aspect='equal')
ax.set_xlabel('Longitude')
ax.set_ylabel('Latitude')
ax.set_title('Temperature Anomaly (Equirectangular)')
ax.set_xticks(np.arange(-180, 181, 60))
ax.set_yticks(np.arange(-90, 91, 30))

cbar = plt.colorbar(im, ax=ax)
cbar.set_label('Temperature Anomaly (°C)')

plt.tight_layout()
plt.show()
```

### Example 2: Neural Network Architecture

```python
import matplotlib.pyplot as plt
import numpy as np
import matplotlib.patches as mpatches

# Define neural network architecture
layers = [784, 256, 128, 10]
layer_names = ['Input', 'Hidden 1', 'Hidden 2', 'Output']

fig, ax = plt.subplots(figsize=(12, 10))

# Node positions
node_positions = {}
for i, n_nodes in enumerate(layers):
    x = i * 3
    y_positions = np.linspace(0, (n_nodes - 1) * 0.8, n_nodes)
    node_positions[i] = [(x, y) for y in y_positions]

# Draw connections (weights)
for i in range(len(layers) - 1):
    for pos1 in node_positions[i][:10]:  # Sample for visibility
        for pos2 in node_positions[i + 1][:10]:
            alpha = np.random.uniform(0.02, 0.1)
            color = 'blue' if np.random.rand() > 0.5 else 'red'
            ax.plot([pos1[0], pos2[0]], [pos1[1], pos2[1]], 
                   color=color, alpha=alpha, linewidth=0.5)

# Draw nodes
colors = plt.cm.Set3(np.linspace(0, 1, len(layers)))
for i, (layer_size, positions) in enumerate(node_positions.items()):
    color = colors[i]
    for x, y in positions[:15]:  # Show sample nodes
        size = 300 if i == 0 or i == len(layers) - 1 else 150
        circle = plt.Circle((x, y), 0.15, color=color, ec='black', linewidth=1)
        ax.add_patch(circle)

# Add layer labels
for i, name in enumerate(layer_names):
    ax.text(i * 3, -1.5, name, ha='center', fontsize=12, fontweight='bold')

ax.set_xlim(-1, (len(layers) - 1) * 3 + 1)
ax.set_ylim(-2, max(len(layers[0], len(layers[-1])) * 0.8 + 1)
ax.axis('off')
ax.set_title('Neural Network Architecture', fontsize=14, fontweight='bold')

# Add legend
legend_elements = [
    mpatches.Patch(facecolor=colors[0], edgecolor='black', label='Input Layer'),
    mpatches.Patch(facecolor=colors[1], edgecolor='black', label='Hidden Layers'),
    mpatches.Patch(facecolor=colors[-1], edgecolor='black', label='Output Layer')
]
ax.legend(handles=legend_elements, loc='upper right')

plt.tight_layout()
plt.show()
```

### Example 3: Financial Heatmap with Dendrogram

```python
import matplotlib.pyplot as plt
import numpy as np
import matplotlib.patches as mpatches
from scipy.cluster.hierarchy import linkage, dendrogram
from scipy.spatial.distance import pdist

# Generate stock correlation data
np.random.seed(42)
stocks = ['AAPL', 'GOOG', 'MSFT', 'AMZN', 'META', 'NVDA', 'TSLA', 'JPM', 'V', 'WMT']
n_stocks = len(stocks)

# Generate correlation matrix
returns = np.random.randn(100, n_stocks)
corr_matrix = np.corrcoef(returns.T)

# Add structure
for i in range(n_stocks):
    for j in range(n_stocks):
        if i < j:
            corr_matrix[i, j] *= 0.8

fig = plt.figure(figsize=(14, 10))
gs = fig.add_gridspec(1, 2, width_ratios=[1, 3], wspace=0.2)

# Dendrogram
ax_dendro = fig.add_subplot(gs[0])
dist = pdist(corr_matrix)
linkage_matrix = linkage(dist, method='average')
dendrogram(linkage_matrix, labels=stocks, orientation='left', ax=ax_dendro)
ax_dendro.set_title('Clustering', fontsize=11)
ax_dendro.set_xlabel('Distance')

# Heatmap
ax_heatmap = fig.add_subplot(gs[1])
im = ax_heatmap.imshow(corr_matrix, cmap='RdBu_r', vmin=-1, vmax=1, aspect='auto')
ax_heatmap.set_xticks(np.arange(n_stocks))
ax_heatmap.set_yticks(np.arange(n_stocks))
ax_heatmap.set_xticklabels(stocks)
ax_heatmap.set_yticklabels(stocks)
ax_heatmap.set_title('Stock Correlations', fontsize=11)

# Add values
for i in range(n_stocks):
    for j in range(n_stocks):
        color = 'white' if abs(corr_matrix[i, j]) > 0.5 else 'black'
        ax_heatmap.text(j, i, f'{corr_matrix[i, j]:.2f}', 
                     ha='center', va='center', color=color, fontsize=8)

# Colorbar
cbar = plt.colorbar(im, ax=ax_heatmap, shrink=0.8)
cbar.set_label('Correlation')

plt.suptitle('Financial Correlation with Clustering', fontsize=14, fontweight='bold')
plt.tight_layout()
plt.show()
```

### Example 4: EEG Brain Visualization

```python
import matplotlib.pyplot as plt
import numpy as np

# Electrode positions (simplified 2D layout)
# Standard 10-20 system positions
electrodes = {
    'Fp1': (-1.5, 2), 'Fp2': (1.5, 2),
    'F7': (-2.5, 1), 'F3': (-1, 1), 'Fz': (0, 1), 'F4': (1, 1), 'F8': (2.5, 1),
    'T7': (-3, 0), 'C3': (-1.5, 0), 'Cz': (0, 0), 'C4': (1.5, 0), 'T8': (3, 0),
    'P7': (-2.5, -1), 'P3': (-1, -1), 'Pz': (0, -1), 'P4': (1, -1), 'P8': (2.5, -1),
    'O1': (-1.5, -2), 'O2': (1.5, -2)
}

# Generate simulated EEG data
np.random.seed(42)
n_timepoints = 100
eeg_data = {electrode: np.random.randn(n_timepoints) * 10 for electrode in electrodes}

# Create figure
fig, axes = plt.subplots(1, 2, figsize=(14, 8))

# 2D topographic map
ax1 = axes[0]
for electrode, (x, y) in electrodes.items():
    # Color based on activity
    activity = eeg_data[electrode][50]  # Sample at midpoint
    color = plt.cm.RdBu_r((activity + 20) / 40)
    size = 200 + abs(activity) * 10
    
    ax1.scatter(x, y, c=[color], s=size, edgecolors='black', linewidth=1)
    ax1.text(x, y, electrode, ha='center', va='center', fontsize=8)

ax1.set_xlim(-4, 4)
ax1.set_ylim(-3, 3)
ax1.set_aspect('equal')
ax1.axis('off')
ax1.set_title('2D Topographic Map', fontsize=12)

# Time series
ax2 = axes[1]
for electrode, positions in electrodes.items():
    # Get frontal electrodes
    if 'F' in electrode or 'O' in electrode:
        ax2.plot(eeg_data[electrode], label=electrode, alpha=0.7)

ax2.set_xlabel('Time (ms)', fontsize=11)
ax2.set_ylabel('Amplitude (μV)', fontsize=11)
ax2.set_title('EEG Time Series', fontsize=12)
ax2.legend(fontsize=8, ncol=3)
ax2.grid(True, alpha=0.3)

plt.suptitle('EEG Brain Activity Visualization', fontsize=14, fontweight='bold')
plt.tight_layout()
plt.show()

# 3D brain wireframe
fig = plt.figure(figsize=(10, 8))
ax = fig.add_subplot(111, projection='3d')

# Create brain outline
theta = np.linspace(0, 2*np.pi, 30)
phi = np.linspace(0, np.pi, 20)
THETA, PHI = np.meshgrid(theta, phi)

r = 1
x_brain = r * np.sin(PHI) * np.cos(THETA)
y_brain = r * np.sin(PHI) * np.sin(THETA)
z_brain = r * np.cos(PHI)

ax.plot_wireframe(x_brain, y_brain, z_brain, color='lightblue', alpha=0.3)

# Add electrode positions
for electrode, (x2d, y2d) in electrodes.items():
    # Simple projection to 3D
    x3d = x2d / 3
    y3d = y2d / 2.5
    z3d = 0.5 if y2d > 0 else -0.5
    
    activity = eeg_data[electrode][50]
    color = plt.cm.RdBu_r((activity + 20) / 40)
    ax.scatter([x3d], [y3d], [z3d], c=[color], s=100, edgecolors='black')
    ax.text(x3d, y3d, z3d + 0.1, electrode, fontsize=6)

ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')
ax.set_title('3D Brain Electrode Positions', fontsize=12)

plt.tight_layout()
plt.show()
```

---

## Applications

### Banking Sector: Risk Surface

```python
import matplotlib.pyplot as plt
import numpy as np
from mpl_toolkits.mplot3d import Axes3D

# Generate risk data
np.random.seed(42)
n_portfolios = 500

# Variables
returns = np.random.normal(8, 12, n_portfolios)
risks = np.random.normal(15, 5, n_portfolios)
leverages = np.random.normal(2, 0.5, n_portfolios)

fig = plt.figure(figsize=(14, 10))
gs = fig.add_gridspec(2, 2, hspace=0.3, wspace=0.3)

# 3D risk surface
ax1 = fig.add_subplot(gs[0, 0], projection='3d')
ax1.scatter(risks, returns, leverages, c=returns, cmap='viridis', s=30, alpha=0.6)
ax1.set_xlabel('Risk (σ)')
ax1.set_ylabel('Return (%)')
ax1.set_zlabel('Leverage')
ax1.set_title('3D Risk-Return Space')

# Risk-return contour
ax2 = fig.add_subplot(gs[0, 1])
risk_range = np.linspace(5, 25, 50)
return_range = np.linspace(-10, 25, 50)
RISK, RETURN = np.meshgrid(risk_range, return_range)
SHARPE = (RETURN - 2) / RISK

contour = ax2.contourf(RISK, RETURN, SHARPE, levels=20, cmap='viridis')
ax2.contour(RISK, RETURN, SHARPE, levels=10, colors='white', linewidths=0.5, alpha=0.5)
ax2.scatter(risks, returns, c='red', s=10, alpha=0.3)
ax2.set_xlabel('Risk')
ax2.set_ylabel('Return (%)')
ax2.set_title('Risk-Return Contours (Sharpe)')
plt.colorbar(contour, ax=ax2, label='Sharpe Ratio')

# Leverage vs Risk
ax3 = fig.add_subplot(gs[1, 0])
colors = plt.cm.RdYlGn_r((returns - returns.min()) / (returns.max() - returns.min()))
ax3.scatter(risks, leverages, c=colors, s=30, alpha=0.6)
ax3.set_xlabel('Risk')
ax3.set_ylabel('Leverage')
ax3.set_title('Risk vs Leverage')

# Parallel coordinates
ax4 = fig.add_subplot(gs[1, 1])

# Normalize data
returns_norm = (returns - returns.mean()) / returns.std()
risks_norm = (risks - risks.mean()) / risks.std()
leverages_norm = (leverages - leverages.mean()) / leverages.std()

for i in range(min(50, n_portfolios)):
    ax4.plot([0, 1, 2], [returns_norm[i], risks_norm[i], leverages_norm[i]], 
           color=plt.cm.Blues(0.3 + i/100), alpha=0.3, linewidth=1)

ax4.set_xticks([0, 1, 2])
ax4.set_xticklabels(['Return', 'Risk', 'Leverage'])
ax4.set_title('Parallel Coordinates')

plt.suptitle('Multi-Dimensional Risk Analysis', fontsize=14, fontweight='bold')
plt.tight_layout()
plt.show()
```

### Healthcare: Medical Imaging Visualization

```python
import matplotlib.pyplot as plt
import numpy as np

# Generate MRI-like slices
np.random.seed(42)
slices = []
for i in range(20):
    # Create pseudo-MRI data
    data = np.zeros((50, 50))
    
    # Add ellipses to simulate brain structures
    from matplotlib.patches import Ellipse
    
    # White matter
    data += Ellipse((25, 25), 15, 15, fill=True).get_mask().astype(float) * 0.8
    # Gray matter (random patterns)
    data += np.random.rand(50, 50) * 0.3
    # Add noise
    data += np.random.rand(50, 50) * 0.2
    
    slices.append(data * (1 + 0.1 * np.sin(i * np.pi / 10)))

fig = plt.figure(figsize=(14, 10))

# Multiple slices
for i in range(16):
    ax = plt.subplot(4, 4, i + 1)
    ax.imshow(slices[i], cmap='gray', vmin=0, vmax=1.5)
    ax.set_title(f'Slice {i+1}', fontsize=8)
    ax.axis('off')

plt.suptitle('MRI Slice Visualization', fontsize=14, fontweight='bold')
plt.tight_layout()
plt.show()

# 3D volume rendering (simplified)
fig = plt.figure(figsize=(12, 10))
ax = fig.add_subplot(111, projection='3d')

x, y, z = np.mgrid[:50:10, :50:10, :20:5]
vol = np.array(slices)[::5, ::2, ::2]

# Simple volume visualization using voxels
colors = plt.cm.gray(vol / vol.max())
ax.voxels(vol > 0.3, facecolors=colors, edgecolors='gray', linewidth=0.3)
ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')
ax.set_title('3D Volume Rendering', fontsize=12)

plt.tight_layout()
plt.show()

# Intensity profile
fig, ax = plt.subplots(figsize=(12, 6))

# Extract intensity across slices at a point
point_x, point_y = 25, 25
intensity_profile = [slices[i][point_x, point_y] for i in range(len(slices))]

ax.plot(intensity_profile, 'b-', linewidth=2)
ax.fill_between(range(len(slices)), intensity_profile, alpha=0.3)
ax.set_xlabel('Slice Number')
ax.set_ylabel('Intensity')
ax.set_title(f'Intensity Profile at ({point_x}, {point_y})')
ax.grid(True, alpha=0.3)

# Add annotations
ax.axhline(y=np.mean(intensity_profile), color='red', linestyle='--', label='Mean')
ax.axhline(y=np.mean(intensity_profile) + np.std(intensity_profile), 
            color='orange', linestyle=':', label='+1 Std')
ax.axhline(y=np.mean(intensity_profile) - np.std(intensity_profile), 
            color='orange', linestyle=':', label='-1 Std')
ax.legend()

plt.tight_layout()
plt.show()
```

---

## Output Results

### Advanced Output Summary

```python
print("=" * 70)
print("ADVANCED VISUALIZATION - OUTPUT SUMMARY")
print("=" * 70)

print("\n1. 3D Visualizations:")
print("   - Line plots")
print("   - Surface plots")
print("   - Wireframes")
print("   - Scatter plots")

print("\n2. Specialized Plots:")
print("   - Contour plots")
print("   - Quiver/stream plots")
print("   - Polar plots")
print("   - Error ellipses")

print("\n3. Domain Applications:")
print("   - Climate data")
print("   - Neural network architecture")
print("   - Financial correlations")
print("   - Brain/EEG visualization")
print("   - Medical imaging")

print("\n4. Advanced Techniques:")
print("   - 3D surfaces on spherical coordinates")
print("   - Volume rendering")
print("   - Topographic maps")
print("   - Network graphs")

print("=" * 70)
```

---

## Visualization

### ASCII Advanced Visualizations

#### 3D Surface

```
3D SURFACE VISUALIZATION
                    Z
                    |
              +----+----+----+
             /            \
            /      peak     \
           /       |        \
    Y ----+----+----+----+----
           \            /
            \  valley /
             \      /
              +----+
             /
    X -------(view from above)
```

#### Volume Rendering

```
VOLUME SLICES (MRI-style)

   +-----+   +-----+   +-----+
   |***  |   |*****|   | *** |
   |**   |   |***  |   |     |
   +-----+   +-----+   +-----+
    Slice     Slice     Slice
      1        2        3
        
Stack to form 3D volume
```

#### Network

```
NEURAL NETWORK
              
        o o o o    (Input: 784)
          |        
       o  o       
        \  /       
        o o   (Hidden: 256)
          |        
       o  o       
        \  /       
        o o o o    (Output: 10)
```

---

## Advanced Topics

### Plotly Integration

```python
import plotly.graph_objects as go
import numpy as np

# Convert Matplotlib to Plotly
fig = go.Figure()

x = np.linspace(0, 10, 100)
y = np.sin(x)

fig.add_trace(go.Scatter(x=x, y=y, mode='lines', name='sin'))

fig.update_layout(title='Interactive Plotly Figure', 
               xaxis_title='X', yaxis_title='Y')

# Export as HTML
fig.write_html('interactive_plotly.html')
```

### Bokeh Integration

```python
from bokeh.plotting import figure, output_file, show

# Simple Bokeh plot
p = figure(title='Bokeh Example', width=400, height=300)
p.line(x, y, line_color='blue', line_width=2)

output_file('bokeh_plot.html')
show(p)
```

### Altair Integration

```python
import altair as alt
import pandas as pd

# Create DataFrame
df = pd.DataFrame({'x': x, 'y': y})

# Create Altair chart
chart = alt.Chart(df).mark_line().encode(
    x='x:Q',
    y='y:Q'
).properties(title='Altair Chart')

# Save as HTML
chart.save('altair_chart.html')
```

---

## Conclusion

### Summary

This final module covered advanced visualization techniques:

1. **Fundamentals**: 3D plotting, contours, vectors, polar, error ellipses
2. **Implementation**: Climate, neural networks, financial correlations, EEG, MRI
3. **Applications**: Banking risk analysis, healthcare imaging
4. **Advanced Topics**: Plotly, Bokeh, Altair integration

### Key Takeaways

- 3D visualization reveals multi-dimensional structure
- Specialized plots serve specific domains
- Integration with other libraries extends capabilities
- Advanced techniques enable cutting-edge visualizations

### Progress Complete

All 6 modules in Matplotlib and Seaborn are now complete:
1. Basic Plotting with Matplotlib
2. Statistical Plots with Seaborn
3. Customizing Plots and Styling
4. Subplots and Figure Management
5. Interactive Plots and Annotations
6. Advanced Visualization Techniques

### Resources

- Matplotlib 3D: https://matplotlib.org/stable/gallery/mplot3d/
- Plotly: https://plotly.com/python/
- Bokeh: https://docs.bokeh.org/

---

*End of Module 06: Advanced Visualization Techniques*
*Complete Data Science AI Guide - Matplotlib and Seaborn*