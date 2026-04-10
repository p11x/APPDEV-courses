# Topic: 3D Data Visualization
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for 3D Data Visualization

I. INTRODUCTION
This module covers 3D visualization techniques including
3D scatter plots, surface plots, bar charts, and interactive
3D visualizations.

II. CORE CONCEPTS
- 3D coordinate systems
- Surface and mesh visualization
- 3D statistical plots

III. IMPLEMENTATION
- matplotlib 3D plotting
- plotly interactive 3D
- numpy for 3D data

IV. EXAMPLES
- Banking: Risk surfaces, portfolio performance
- Healthcare: MRI visualization, patient data

V. OUTPUT RESULTS
- 3D visualizations

VI. TESTING
- 3D plot functions

VII. ADVANCED TOPICS
- Animation in 3D

VIII. CONCLUSION
Best practices for 3D visualization
"""

import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import seaborn as sns
import numpy as np
import pandas as pd
import plotly.express as px
import plotly.graph_objects as go
import warnings
warnings.filterwarnings('ignore')

plt.style.use('seaborn-v0_8-whitegrid')


def generate_3d_data(n=500, seed=42):
    """Generate 3D data."""
    np.random.seed(seed)
    
    x = np.random.randn(n)
    y = np.random.randn(n)
    z = x * 0.5 + y * 0.3 + np.random.randn(n) * 0.2
    
    return x, y, z


def generate_surface_data():
    """Generate surface data."""
    x = np.linspace(-5, 5, 50)
    y = np.linspace(-5, 5, 50)
    X, Y = np.meshgrid(x, y)
    Z = np.sin(np.sqrt(X**2 + Y**2))
    
    return X, Y, Z


def create_3d_scatter(x, y, z, title='3D Scatter Plot'):
    """Create 3D scatter plot."""
    fig = plt.figure(figsize=(12, 10))
    ax = fig.add_subplot(111, projection='3d')
    
    scatter = ax.scatter(x, y, z, c=z, cmap='viridis', s=50, alpha=0.7)
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title(title, fontsize=14, fontweight='bold')
    
    plt.colorbar(scatter, ax=ax, shrink=0.5)
    plt.tight_layout()
    
    return fig


def create_3d_line(x, y, z, title='3D Line Plot'):
    """Create 3D line plot."""
    fig = plt.figure(figsize=(12, 10))
    ax = fig.add_subplot(111, projection='3d')
    
    ax.plot(x, y, z, linewidth=2, color='steelblue')
    ax.scatter(x[-1], y[-1], z[-1], s=200, color='red')
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title(title, fontsize=14, fontweight='bold')
    
    plt.tight_layout()
    
    return fig


def create_3d_surface(X, Y, Z, title='3D Surface Plot'):
    """Create 3D surface plot."""
    fig = plt.figure(figsize=(12, 10))
    ax = fig.add_subplot(111, projection='3d')
    
    surf = ax.plot_surface(X, Y, Z, cmap='viridis', alpha=0.8,
                         linewidth=0, antialiased=False)
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title(title, fontsize=14, fontweight='bold')
    
    plt.colorbar(surf, ax=ax, shrink=0.5)
    plt.tight_layout()
    
    return fig


def create_3d_wireframe(X, Y, Z, title='3D Wireframe'):
    """Create 3D wireframe."""
    fig = plt.figure(figsize=(12, 10))
    ax = fig.add_subplot(111, projection='3d')
    
    ax.plot_wireframe(X, Y, Z, color='steelblue', alpha=0.7)
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title(title, fontsize=14, fontweight='bold')
    
    plt.tight_layout()
    
    return fig


def create_3d_bar(x, y, z, title='3D Bar Chart'):
    """Create 3D bar chart."""
    fig = plt.figure(figsize=(12, 10))
    ax = fig.add_subplot(111, projection='3d')
    
    ax.bar3d(x, y, np.zeros_like(z), dx=0.5, dy=0.5, dz=z,
            color='steelblue', alpha=0.8)
    
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title(title, fontsize=14, fontweight='bold')
    
    plt.tight_layout()
    
    return fig


def create_3d_contour(X, Y, Z, title='3D Contour'):
    """Create 3D contour plot."""
    fig, axes = plt.subplots(1, 2, figsize=(16, 6))
    
    contour = axes[0].contourf(X, Y, Z, levels=20, cmap='viridis')
    axes[0].set_xlabel('X')
    axes[0].set_ylabel('Y')
    axes[0].set_title('2D Contour')
    plt.colorbar(contour, ax=axes[0])
    
    ax = fig.add_subplot(122, projection='3d')
    ax.contourf(X, Y, Z, levels=20, cmap='viridis', offset=0)
    ax.plot_surface(X, Y, Z, alpha=0.3, cmap='viridis')
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_title('3D Contour')
    
    plt.tight_layout()
    
    return fig


def create_interactive_3d_scatter(df, x_col, y_col, z_col, title='Interactive 3D Scatter'):
    """Create interactive 3D scatter plot."""
    fig = px.scatter_3d(df, x=x_col, y=y_col, z=z_col,
                        color=z_col, title=title)
    
    fig.update_layout(
        scene=dict(
            xaxis_title=x_col,
            yaxis_title=y_col,
            zaxis_title=z_col
        )
    )
    
    return fig


def create_interactive_3d_surface(X, Y, Z, title='Interactive 3D Surface'):
    """Create interactive 3D surface."""
    fig = go.Figure(data=[go.Surface(x=X, y=Y, z=Z)])
    
    fig.update_layout(
        title=title,
        scene=dict(
            xaxis_title='X',
            yaxis_title='Y',
            zaxis_title='Z'
        ),
        width=800,
        height=600
    )
    
    return fig


def core_implementation():
    """Core 3D visualization implementation."""
    x, y, z = generate_3d_data(300)
    X, Y, Z = generate_surface_data()
    
    results = {}
    
    results['scatter'] = create_3d_scatter(x, y, z, '3D Scatter')
    results['line'] = create_3d_line(x[:50], y[:50], z[:50], '3D Line')
    results['surface'] = create_3d_surface(X, Y, Z, '3D Surface')
    results['wireframe'] = create_3d_wireframe(X, Y, Z, '3D Wireframe')
    results['bar'] = create_3d_bar(x[:10], y[:10], z[:10], '3D Bar')
    results['contour'] = create_3d_contour(X, Y, Z, '3D Contour')
    
    return results


def banking_example():
    """Banking application - Risk Surface Visualization."""
    np.random.seed(123)
    
    returns_range = np.linspace(-20, 20, 30)
    volatility_range = np.linspace(5, 30, 30)
    R, V = np.meshgrid(returns_range, volatility_range)
    
    sharpe = (R - 2) / V
    sharpe = np.clip(sharpe, -2, 2)
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1, projection='3d')
    surf = ax1.plot_surface(R, V, sharpe, cmap='viridis', alpha=0.8)
    ax1.set_xlabel('Returns (%)')
    ax1.set_ylabel('Volatility (%)')
    ax1.set_zlabel('Sharpe Ratio')
    ax1.set_title('Risk-Return Surface')
    plt.colorbar(surf, ax=ax1, shrink=0.5)
    
    ax2 = fig.add_subplot(2, 3, 2, projection='3d')
    ax2.plot_wireframe(R, V, sharpe, color='steelblue', alpha=0.7)
    ax2.set_xlabel('Returns (%)')
    ax2.set_ylabel('Volatility (%)')
    ax2.set_zlabel('Sharpe Ratio')
    ax2.set_title('Risk Wireframe')
    
    ax3 = fig.add_subplot(2, 3, 3)
    contour = ax3.contourf(R, V, sharpe, levels=20, cmap='viridis')
    ax3.set_xlabel('Returns (%)')
    ax3.set_ylabel('Volatility (%)')
    ax3.set_title('Risk Contour Map')
    plt.colorbar(contour, ax=ax3)
    
    ax4 = fig.add_subplot(2, 3, 4, projection='3d')
    scatter = ax4.scatter(np.random.randn(100)*10, np.random.randn(100)*10,
                        np.random.randn(100), c=np.random.randn(100),
                        cmap='viridis', s=50)
    ax4.set_xlabel('Returns')
    ax4.set_ylabel('Volatility')
    ax4.set_zlabel('Returns')
    ax4.set_title('Portfolio Scatter')
    
    ax5 = fig.add_subplot(2, 3, 5, projection='3d')
    x = np.linspace(0, 10, 20)
    y = np.linspace(0, 10, 20)
    x, y = np.meshgrid(x, y)
    z = x * 0.5 + y * 0.5
    ax5.plot_surface(x, y, z, cmap='Blues', alpha=0.8)
    ax5.set_xlabel('Time')
    ax5.set_ylabel('Investment')
    ax5.set_zlabel('Value')
    ax5.set_title('Growth Surface')
    
    ax6 = fig.add_subplot(2, 3, 6)
    x_edges = np.arange(11)
    y_values = np.random.randint(100, 1000, 10)
    ax6.bar(x_edges, y_values, color='steelblue')
    ax6.set_xlabel('Period')
    ax6.set_ylabel('Value')
    ax6.set_title('Value by Period')
    ax6.grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    print("=" * 60)
    print("RISK SURFACE ANALYSIS")
    print("=" * 60)
    print(f"\nReturn Range: {returns_range.min():.1f}% to {returns_range.max():.1f}%")
    print(f"Volatility Range: {volatility_range.min():.1f}% to {volatility_range.max():.1f}%")
    print(f"Max Sharpe Ratio: {sharpe.max():.2f}")
    print(f"Min Sharpe Ratio: {sharpe.min():.2f}")
    
    return fig, (R, V, sharpe)


def healthcare_example():
    """Healthcare application - Patient Data 3D."""
    np.random.seed(456)
    
    n_patients = 300
    
    healthcare_data = pd.DataFrame({
        'Age': np.random.normal(50, 15, n_patients).clip(18, 95),
        'BMI': np.random.normal(28, 5, n_patients).clip(16, 45),
        'BP_Systolic': np.random.normal(130, 15, n_patients).clip(80, 200),
        'Cholesterol': np.random.normal(200, 35, n_patients).clip(100, 300),
        'Glucose': np.random.normal(100, 20, n_patients).clip(50, 200)
    })
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1, projection='3d')
    scatter = ax1.scatter(healthcare_data['Age'], healthcare_data['BMI'],
                        healthcare_data['BP_Systolic'],
                        c=healthcare_data['Cholesterol'], cmap='viridis',
                        s=30, alpha=0.7)
    ax1.set_xlabel('Age')
    ax1.set_ylabel('BMI')
    ax1.set_zlabel('BP')
    ax1.set_title('Age-BMI-BP 3D')
    plt.colorbar(scatter, ax=ax1, shrink=0.5)
    
    ax2 = fig.add_subplot(2, 3, 2, projection='3d')
    scatter = ax2.scatter(healthcare_data['Age'], healthcare_data['BP_Systolic'],
                        healthcare_data['Cholesterol'],
                        c=healthcare_data['Glucose'], cmap='Reds',
                        s=30, alpha=0.7)
    ax2.set_xlabel('Age')
    ax2.set_ylabel('BP')
    ax2.set_zlabel('Cholesterol')
    ax2.set_title('BP-Cholesterol-Age')
    plt.colorbar(scatter, ax=ax2, shrink=0.5)
    
    ax3 = fig.add_subplot(2, 3, 3)
    ax3.hist(healthcare_data['Age'], bins=20, color='steelblue',
           edgecolor='white', alpha=0.7)
    ax3.set_xlabel('Age')
    ax3.set_ylabel('Frequency')
    ax3.set_title('Age Distribution')
    ax3.grid(True, alpha=0.3)
    
    ax4 = fig.add_subplot(2, 3, 4, projection='3d')
    x = np.linspace(18, 95, 20)
    y = np.linspace(16, 45, 20)
    X, Y = np.meshgrid(x, y)
    Z = (X + Y) / 10
    ax4.plot_surface(X, Y, Z, cmap='Blues', alpha=0.7)
    ax4.set_xlabel('Age')
    ax4.set_ylabel('BMI')
    ax4.set_zlabel('Risk Score')
    ax4.set_title('Risk Surface')
    
    ax5 = fig.add_subplot(2, 3, 5, projection='3d')
    ax5.scatter(healthcare_data['BMI'], healthcare_data['BP_Systolic'],
              healthcare_data['Glucose'],
              c=healthcare_data['Age'], cmap='viridis', s=30, alpha=0.7)
    ax5.set_xlabel('BMI')
    ax5.set_ylabel('BP')
    ax5.set_zlabel('Glucose')
    ax5.set_title('BMI-BP-Glucose')
    
    ax6 = fig.add_subplot(2, 3, 6)
    ax6.scatter(healthcare_data['BP_Systolic'], healthcare_data['Cholesterol'],
              c=healthcare_data['BMI'], cmap='viridis', s=30, alpha=0.5)
    ax6.set_xlabel('BP Systolic')
    ax6.set_ylabel('Cholesterol')
    ax6.set_title('BP vs Cholesterol')
    ax6.grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    print("=" * 60)
    print("HEALTHCARE PATIENT DATA 3D")
    print("=" * 60)
    print(f"\nTotal Patients: {n_patients}")
    print(f"Average Age: {healthcare_data['Age'].mean():.1f}")
    print(f"Average BMI: {healthcare_data['BMI'].mean():.1f}")
    print(f"Average BP: {healthcare_data['BP_Systolic'].mean():.1f}")
    print(f"Average Cholesterol: {healthcare_data['Cholesterol'].mean():.1f}")
    
    return fig, healthcare_data


def main():
    """Main execution function."""
    print("Executing 3D Data Visualization")
    print("=" * 60)
    
    x, y, z = generate_3d_data(300)
    X, Y, Z = generate_surface_data()
    
    create_3d_scatter(x, y, z)
    create_3d_surface(X, Y, Z)
    create_3d_wireframe(X, Y, Z)
    create_3d_bar(x[:10], y[:10], z[:10])
    create_3d_contour(X, Y, Z)
    
    plt.show()
    
    return (x, y, z)


if __name__ == "__main__":
    main()