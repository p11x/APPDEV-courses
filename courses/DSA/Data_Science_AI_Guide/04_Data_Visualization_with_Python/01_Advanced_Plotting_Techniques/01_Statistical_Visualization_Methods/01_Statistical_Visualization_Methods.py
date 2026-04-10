# Topic: Statistical Visualization Methods
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Statistical Visualization Methods

I. INTRODUCTION
This module covers essential statistical visualization techniques including
histograms, box plots, violin plots, correlation heatmaps, and pair plots
using matplotlib, seaborn, and plotly libraries.

II. CORE CONCEPTS
- Distribution visualization (histograms, KDE, violin plots)
- Comparative visualization (box plots, bar charts)
- Correlation analysis (heatmaps, scatter matrices)
- Statistical annotations (mean, median, confidence intervals)

III. IMPLEMENTATION
- matplotlib for customizable static visualizations
- seaborn for statistical statistical graphics
- plotly for interactive visualizations

IV. EXAMPLES
- Banking: Loan portfolio analysis, credit risk assessment
- Healthcare: Patient outcomes, clinical trial data

V. OUTPUT RESULTS
- Statistical charts with proper annotations
- Interactive plots for data exploration

VI. TESTING
- Unit tests for visualization functions
- Integration tests for multi-panel figures

VII. ADVANCED TOPICS
- Large dataset optimization
- Statistical annotations and tests
- Custom styling and themes

VIII. CONCLUSION
Best practices for statistical data visualization in practice
"""

import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
import pandas as pd
import plotly.express as px
import plotly.graph_objects as go
from scipy import stats
import warnings
warnings.filterwarnings('ignore')

plt.style.use('seaborn-v0_8-whitegrid')
sns.set_palette("husl")


def generate_sample_data(n=1000, seed=42):
    """Generate sample data for statistical visualization examples."""
    np.random.seed(seed)
    data = pd.DataFrame({
        'Age': np.random.normal(40, 12, n).clip(18, 80),
        'Income': np.random.normal(65000, 20000, n).clip(20000, 200000),
        'Score': np.random.normal(72, 12, n).clip(0, 100),
        'Value': np.random.exponential(10000, n).clip(0, 100000),
        'Category': np.random.choice(['A', 'B', 'C', 'D'], n),
        'Region': np.random.choice(['North', 'South', 'East', 'West'], n)
    })
    data['Income'] = data['Income'] + 0.3 * (data['Score'] - 72) * 500
    return data


def create_histogram(data, column, bins=20, title=None, xlabel=None, ylabel='Frequency'):
    """Create a histogram with statistical annotations."""
    fig, ax = plt.subplots(figsize=(12, 6))
    
    n, bin_edges, patches = ax.hist(
        data[column], bins=bins, edgecolor='white',
        linewidth=1.2, alpha=0.7, color='steelblue'
    )
    
    cm = plt.cm.get_cmap('Blues')
    bin_centers = 0.5 * (bin_edges[:-1] + bin_edges[1:])
    max_val = data[column].max()
    for center, patch in zip(bin_centers, patches):
        plt.setp(patch, 'facecolor', cm(center / max_val))
    
    mean_val = data[column].mean()
    median_val = data[column].median()
    mode_val = data[column].mode()[0]
    
    ax.axvline(mean_val, color='red', linestyle='--', linewidth=2, label=f'Mean: {mean_val:.1f}')
    ax.axvline(median_val, color='green', linestyle='-.', linewidth=2, label=f'Median: {median_val:.1f}')
    ax.axvline(mode_val, color='blue', linestyle=':', linewidth=2, label=f'Mode: {mode_val:.1f}')
    
    ax.set_xlabel(xlabel or column, fontsize=12, fontweight='bold')
    ax.set_ylabel(ylabel, fontsize=12, fontweight='bold')
    ax.set_title(title or f'Distribution of {column}', fontsize=14, fontweight='bold')
    ax.legend(loc='upper right', fontsize=10)
    ax.grid(True, alpha=0.3)
    
    plt.tight_layout()
    return fig


def create_boxplot(data, x_col, y_col, title=None, rotate_x=True):
    """Create box plots for comparing distributions across categories."""
    fig, ax = plt.subplots(figsize=(12, 7))
    
    box = sns.boxplot(
        x=x_col, y=y_col, data=data,
        palette='Set2', ax=ax, linewidth=1.5,
        showmeans=True,
        meanprops={"marker": "D", "markerfacecolor": "red",
                   "markeredgecolor": "darkred", "markersize": 8}
    )
    
    sns.stripplot(
        x=x_col, y=y_col, data=data,
        color='black', alpha=0.3, size=3,
        ax=ax, jitter=True
    )
    
    ax.set_xlabel(x_col, fontsize=12, fontweight='bold')
    ax.set_ylabel(y_col, fontsize=12, fontweight='bold')
    ax.set_title(title or f'{y_col} by {x_col}', fontsize=14, fontweight='bold')
    
    if rotate_x:
        plt.xticks(rotation=15, ha='right')
    
    ax.grid(True, alpha=0.3, axis='y')
    plt.tight_layout()
    return fig


def create_violinplot(data, x_col, y_col, title=None, split_by=None):
    """Create violin plots showing distribution shape."""
    fig, ax = plt.subplots(figsize=(12, 6))
    
    if split_by:
        sns.violinplot(
            x=x_col, y=y_col, hue=split_by, data=data,
            palette='Set2', split=True, ax=ax,
            inner='box', linewidth=1.5
        )
        ax.legend(title=split_by, loc='upper right')
    else:
        sns.violinplot(
            x=x_col, y=y_col, data=data,
            palette='muted', ax=ax,
            inner='box', linewidth=1.5
        )
    
    ax.set_xlabel(x_col, fontsize=12, fontweight='bold')
    ax.set_ylabel(y_col, fontsize=12, fontweight='bold')
    ax.set_title(title or f'{y_col} Distribution by {x_col}', fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3, axis='y')
    
    plt.tight_layout()
    return fig


def create_correlation_heatmap(data, columns=None, title='Correlation Matrix', annot=True):
    """Create correlation heatmap showing relationships between variables."""
    if columns:
        corr_data = data[columns]
    else:
        corr_data = data.select_dtypes(include=[np.number])
    
    corr_matrix = corr_data.corr()
    
    fig, ax = plt.subplots(figsize=(10, 8))
    
    sns.heatmap(
        corr_matrix, annot=annot, fmt='.2f', cmap='RdBu_r',
        center=0, vmin=-1, vmax=1, square=True,
        linewidths=0.5, ax=ax,
        cbar_kws={'shrink': 0.8}
    )
    
    ax.set_title(title, fontsize=14, fontweight='bold')
    plt.tight_layout()
    return fig


def create_scatter_matrix(data, dimensions, color_col=None, title='Scatter Matrix'):
    """Create interactive scatter matrix using Plotly."""
    fig = px.scatter_matrix(
        data, dimensions=dimensions, color=color_col,
        title=title,
        color_discrete_sequence=['steelblue']
    )
    
    fig.update_traces(
        diagonal_visible=False,
        showupperhalf=False
    )
    
    fig.update_layout(
        title_font_size=16,
        width=900,
        height=700
    )
    
    return fig


def create_kde_plot(data, x_col, y_col=None, title=None):
    """Create kernel density estimation plots."""
    fig, ax = plt.subplots(figsize=(10, 6))
    
    if y_col:
        sns.kdeplot(data=data, x=x_col, y=y_col, cmap='Blues', fill=True, ax=ax)
    else:
        sns.kdeplot(data=data, x=x_col, cmap='Blues', fill=True, ax=ax)
    
    ax.set_xlabel(x_col, fontsize=12, fontweight='bold')
    if y_col:
        ax.set_ylabel(y_col, fontsize=12, fontweight='bold')
    ax.set_title(title or f'Kernel Density Estimation: {x_col}', fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3)
    
    plt.tight_layout()
    return fig


def create_pairplot_with_regression(data, x_col, y_col):
    """Create scatter plot with regression line and confidence interval."""
    fig, ax = plt.subplots(figsize=(10, 6))
    
    sns.regplot(
        data=data, x=x_col, y=y_col,
        scatter_kws={'alpha': 0.5, 's': 30},
        line_kws={'color': 'red', 'linewidth': 2},
        ci=95, ax=ax
    )
    
    ax.set_xlabel(x_col, fontsize=12, fontweight='bold')
    ax.set_ylabel(y_col, fontsize=12, fontweight='bold')
    ax.set_title(f'{y_col} vs {x_col} with Regression', fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3)
    
    slope, intercept, r_value, p_value, std_err = stats.linregress(data[x_col], data[y_col])
    ax.text(0.05, 0.95, f'R² = {r_value**2:.3f}\np-value = {p_value:.4f}',
           transform=ax.transAxes, fontsize=10, verticalalignment='top',
           bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.5))
    
    plt.tight_layout()
    return fig


def create_distplot(data, column, title=None):
    """Create distribution plot with histogram and KDE."""
    fig, ax = plt.subplots(figsize=(10, 6))
    
    sns.histplot(data=data, x=column, kde=True, ax=ax, bins=20,
              edgecolor='white', alpha=0.7)
    
    ax.set_xlabel(column, fontsize=12, fontweight='bold')
    ax.set_ylabel('Frequency', fontsize=12, fontweight='bold')
    ax.set_title(title or f'Distribution of {column}', fontsize=14, fontweight='bold')
    ax.grid(True, alpha=0.3)
    
    stats_text = f"Mean: {data[column].mean():.2f}\nStd: {data[column].std():.2f}\nSkew: {data[column].skew():.2f}"
    ax.text(0.95, 0.95, stats_text, transform=ax.transAxes, fontsize=10,
           verticalalignment='top', horizontalalignment='right',
           bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.5))
    
    plt.tight_layout()
    return fig


def create_multi_panel_statistics(data, numeric_cols, category_col=None):
    """Create comprehensive multi-panel statistical visualization."""
    n_cols = len(numeric_cols)
    n_rows = min(n_cols, 4)
    
    fig, axes = plt.subplots((n_cols + 3) // 4, 4, figsize=(16, 4 * ((n_cols + 3) // 4)))
    axes = axes.flatten() if n_cols > 1 else [axes]
    
    for i, col in enumerate(numeric_cols):
        axes[i].hist(data[col], bins=20, edgecolor='white', alpha=0.7, color='steelblue')
        axes[i].axvline(data[col].mean(), color='red', linestyle='--', linewidth=2)
        axes[i].set_xlabel(col, fontsize=10)
        axes[i].set_ylabel('Freq', fontsize=10)
        axes[i].set_title(f'{col}: μ={data[col].mean():.1f}', fontsize=10)
        axes[i].grid(True, alpha=0.3)
    
    for j in range(i + 1, len(axes)):
        axes[j].axis('off')
    
    plt.tight_layout()
    return fig


def core_implementation():
    """Core statistical visualization implementation."""
    data = generate_sample_data(500)
    
    results = {}
    
    results['histogram'] = create_histogram(data, 'Age', bins=15)
    results['boxplot'] = create_boxplot(data, 'Category', 'Score')
    results['violin'] = create_violinplot(data, 'Category', 'Income')
    results['heatmap'] = create_correlation_heatmap(data)
    results['kde'] = create_kde_plot(data, 'Income', 'Score')
    results['distplot'] = create_distplot(data, 'Value')
    
    return results


def banking_example():
    """Banking/Finance application - Loan Portfolio Analysis."""
    np.random.seed(123)
    n_loans = 1000
    
    loan_types = ['Personal', 'Home', 'Auto', 'Business', 'Credit Card']
    
    banking_data = pd.DataFrame({
        'Loan_ID': range(1, n_loans + 1),
        'Loan_Type': np.random.choice(loan_types, n_loans),
        'Loan_Amount': np.random.lognormal(10, 0.8, n_loans).clip(5000, 500000),
        'Credit_Score': np.random.normal(680, 80, n_loans).clip(300, 850),
        'Annual_Income': np.random.lognormal(11, 0.6, n_loans).clip(15000, 500000),
        'Debt_To_Income': np.random.beta(2, 5, n_loans) * 0.5,
        'Employment_Years': np.random.exponential(5, n_loans).clip(0, 30),
        'Default_Risk': np.random.choice([0, 1], n_loans, p=[0.85, 0.15])
    })
    
    banking_data.loc[banking_data['Credit_Score'] < 600, 'Default_Risk'] = 1
    banking_data.loc[banking_data['Credit_Score'] > 750, 'Default_Risk'] = 0
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1)
    for loan_type in loan_types:
        subset = banking_data[banking_data['Loan_Type'] == loan_type]['Loan_Amount']
        ax1.hist(subset, bins=20, alpha=0.5, label=loan_type)
    ax1.set_xlabel('Loan Amount ($)')
    ax1.set_ylabel('Frequency')
    ax1.set_title('Loan Amount Distribution by Type')
    ax1.legend()
    ax1.set_xscale('log')
    
    ax2 = fig.add_subplot(2, 3, 2)
    sns.boxplot(x='Loan_Type', y='Credit_Score', data=banking_data,
               palette='Set2', ax=ax2, hue='Loan_Type')
    default = banking_data[banking_data['Default_Risk'] == 1]
    ax2.scatter(default['Loan_Type'], default['Credit_Score'],
               color='red', s=30, alpha=0.5, marker='x', label='Default')
    ax2.legend()
    ax2.set_xlabel('Loan Type')
    ax2.set_ylabel('Credit Score')
    ax2.set_title('Credit Score by Loan Type')
    ax2.tick_params(axis='x', rotation=45)
    
    ax3 = fig.add_subplot(2, 3, 3)
    banking_data['Status'] = banking_data['Default_Risk'].map({0: 'Non-Default', 1: 'Default'})
    sns.violinplot(x='Status', y='Debt_To_Income', data=banking_data,
                  palette='Set1', ax=ax3, inner='quartile')
    ax3.set_xlabel('Loan Status')
    ax3.set_ylabel('Debt-to-Income Ratio')
    ax3.set_title('DTI by Default Status')
    
    ax4 = fig.add_subplot(2, 3, 4)
    colors = {0: 'green', 1: 'red'}
    for status in [0, 1]:
        subset = banking_data[banking_data['Default_Risk'] == status]
        ax4.scatter(subset['Credit_Score'], subset['Loan_Amount'],
                   c=colors[status], alpha=0.4, s=20,
                   label='Default' if status else 'Non-Default')
    ax4.set_xlabel('Credit Score')
    ax4.set_ylabel('Loan Amount ($)')
    ax4.set_title('Credit Score vs Loan Amount')
    ax4.legend()
    ax4.set_yscale('log')
    
    ax5 = fig.add_subplot(2, 3, 5)
    numeric_cols = ['Loan_Amount', 'Credit_Score', 'Annual_Income',
                  'Debt_To_Income', 'Employment_Years', 'Default_Risk']
    corr = banking_data[numeric_cols].corr()
    mask = np.triu(np.ones_like(corr, dtype=bool))
    sns.heatmap(corr, mask=mask, annot=True, fmt='.2f', cmap='RdYlGn_r',
               center=0, ax=ax5, square=True, linewidths=0.5)
    ax5.set_title('Loan Portfolio Correlation')
    
    ax6 = fig.add_subplot(2, 3, 6)
    default_rate = banking_data.groupby('Loan_Type')['Default_Risk'].mean() * 100
    default_rate = default_rate.sort_values(ascending=True)
    colors = ['green' if x < 10 else 'orange' if x < 20 else 'red' for x in default_rate.values]
    bars = ax6.barh(default_rate.index, default_rate.values, color=colors, edgecolor='white')
    ax6.axvline(10, color='orange', linestyle='--', linewidth=2, label='Warning (10%)')
    ax6.set_xlabel('Default Rate (%)')
    ax6.set_ylabel('Loan Type')
    ax6.set_title('Default Rate by Loan Type')
    ax6.legend()
    
    for bar, val in zip(bars, default_rate.values):
        ax6.text(val + 0.5, bar.get_y() + bar.get_height()/2,
                f'{val:.1f}%', va='center', fontsize=9)
    
    plt.tight_layout()
    
    print("=" * 60)
    print("BANK LOAN PORTFOLIO ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Loans: {len(banking_data):,}")
    print(f"Total Portfolio: ${banking_data['Loan_Amount'].sum():,.0f}")
    print(f"Average Loan: ${banking_data['Loan_Amount'].mean():,.0f}")
    print(f"\nOverall Default Rate: {banking_data['Default_Risk'].mean()*100:.2f}%")
    print(f"Average Credit Score: {banking_data['Credit_Score'].mean():.0f}")
    
    return fig, banking_data


def healthcare_example():
    """Healthcare application - Patient Outcomes Analysis."""
    np.random.seed(456)
    n_patients = 1200
    
    conditions = ['Diabetes', 'Hypertension', 'Heart Disease', 'COPD', 'Asthma']
    
    healthcare_data = pd.DataFrame({
        'Patient_ID': range(1, n_patients + 1),
        'Age': np.random.normal(55, 18, n_patients).clip(18, 95).astype(int),
        'Condition': np.random.choice(conditions, n_patients),
        'BMI': np.random.normal(28, 5, n_patients).clip(16, 45),
        'BP_Systolic': np.random.normal(130, 15, n_patients).clip(80, 200),
        'BP_Diastolic': np.random.normal(82, 10, n_patients).clip(50, 120),
        'Heart_Rate': np.random.normal(72, 12, n_patients).clip(40, 130),
        'Cholesterol': np.random.normal(200, 35, n_patients).clip(100, 300),
        'Length_Of_Stay': np.random.exponential(3, n_patients).clip(1, 30),
        'Readmission_30Day': np.random.choice([0, 1], n_patients, p=[0.88, 0.12]),
        'Patient_Satisfaction': np.random.normal(7.5, 1.5, n_patients).clip(1, 10),
        'Treatment_Cost': np.random.lognormal(9, 0.8, n_patients).clip(1000, 100000)
    })
    
    healthcare_data.loc[healthcare_data['Condition'] == 'Heart Disease', 'BP_Systolic'] += 20
    healthcare_data.loc[healthcare_data['Condition'] == 'Hypertension', 'BP_Systolic'] += 15
    healthcare_data.loc[healthcare_data['BMI'] > 30, 'Cholesterol'] += 20
    healthcare_data.loc[healthcare_data['Age'] > 65, 'Length_Of_Stay'] *= 1.5
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1)
    colors = {'Diabetes': 'blue', 'Hypertension': 'red',
              'Heart Disease': 'green', 'COPD': 'orange', 'Asthma': 'purple'}
    for condition in conditions:
        subset = healthcare_data[healthcare_data['Condition'] == condition]['Age']
        ax1.hist(subset, bins=20, alpha=0.4, label=condition)
    ax1.set_xlabel('Age (Years)')
    ax1.set_ylabel('Frequency')
    ax1.set_title('Age Distribution by Condition')
    ax1.legend()
    
    ax2 = fig.add_subplot(2, 3, 2)
    sns.boxplot(x='Condition', y='Length_Of_Stay', data=healthcare_data,
               hue='Condition', palette='Set2', ax=ax2, legend=False)
    ax2.set_xlabel('Medical Condition')
    ax2.set_ylabel('Length of Stay (Days)')
    ax2.set_title('Length of Stay by Condition')
    ax2.tick_params(axis='x', rotation=45)
    
    ax3 = fig.add_subplot(2, 3, 3)
    healthcare_data['Readmission_Status'] = healthcare_data['Readmission_30Day'].map({
        0: 'Not Readmitted', 1: 'Readmitted'
    })
    sns.violinplot(x='Readmission_Status', y='BMI', data=healthcare_data,
                  palette='Set1', ax=ax3, inner='box')
    ax3.set_xlabel('30-Day Readmission')
    ax3.set_ylabel('BMI')
    ax3.set_title('BMI by Readmission Status')
    
    ax4 = fig.add_subplot(2, 3, 4)
    scatter = ax4.scatter(healthcare_data['BP_Systolic'], healthcare_data['Heart_Rate'],
                         c=healthcare_data['Age'], cmap='RdYlBu_r', alpha=0.5,
                         s=healthcare_data['BMI']*2, edgecolors='white', linewidth=0.3)
    plt.colorbar(scatter, ax=ax4, label='Age')
    ax4.set_xlabel('Systolic BP (mmHg)')
    ax4.set_ylabel('Heart Rate (bpm)')
    ax4.set_title('BP vs Heart Rate\n(Size=BMI, Color=Age)')
    ax4.axvline(140, color='red', linestyle='--', alpha=0.5, label='High BP')
    ax4.axhline(100, color='orange', linestyle='--', alpha=0.5, label='High HR')
    ax4.legend(fontsize=8)
    
    ax5 = fig.add_subplot(2, 3, 5)
    numeric_cols = ['Age', 'BMI', 'BP_Systolic', 'BP_Diastolic', 'Heart_Rate',
                   'Cholesterol', 'Length_Of_Stay', 'Patient_Satisfaction',
                   'Treatment_Cost', 'Readmission_30Day']
    corr = healthcare_data[numeric_cols].corr()
    mask = np.triu(np.ones_like(corr, dtype=bool))
    sns.heatmap(corr, mask=mask, annot=True, fmt='.1f', cmap='RdBu_r',
                center=0, ax=ax5, square=True, linewidths=0.5,
                annot_kws={'fontsize': 7})
    ax5.set_title('Patient Correlation Matrix')
    
    ax6 = fig.add_subplot(2, 3, 6)
    stats_by_condition = healthcare_data.groupby('Condition').agg({
        'Readmission_30Day': 'mean',
        'Patient_Satisfaction': 'mean',
        'Length_Of_Stay': 'mean'
    }).reset_index()
    
    stats_by_condition['Readmission_Rate'] = stats_by_condition['Readmission_30Day'] * 100
    
    x = np.arange(len(conditions))
    width = 0.35
    
    bars1 = ax6.bar(x - width/2, stats_by_condition['Patient_Satisfaction'],
                   width, label='Satisfaction', color='green', alpha=0.7)
    bars2 = ax6.bar(x + width/2, stats_by_condition['Readmission_Rate'],
                  width, label='Readmission (%)', color='red', alpha=0.7)
    
    ax6.set_xlabel('Medical Condition')
    ax6.set_ylabel('Score / Rate (%)')
    ax6.set_title('Satisfaction vs Readmission')
    ax6.set_xticks(x)
    ax6.set_xticklabels(conditions, rotation=45)
    ax6.legend()
    
    plt.tight_layout()
    
    print("=" * 60)
    print("HEALTHCARE PATIENT OUTCOMES ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Patients: {len(healthcare_data):,}")
    print(f"Average Age: {healthcare_data['Age'].mean():.1f} years")
    print(f"Average BMI: {healthcare_data['BMI'].mean():.1f}")
    print(f"Average Stay: {healthcare_data['Length_Of_Stay'].mean():.1f} days")
    print(f"\n30-Day Readmission: {healthcare_data['Readmission_30Day'].mean()*100:.1f}%")
    print(f"Satisfaction: {healthcare_data['Patient_Satisfaction'].mean():.1f}/10")
    print(f"Total Cost: ${healthcare_data['Treatment_Cost'].sum():,.0f}")
    
    return fig, healthcare_data


def main():
    """Main execution function."""
    print("Executing Statistical Visualization Methods implementation")
    print("=" * 60)
    
    data = generate_sample_data(500)
    print(f"Generated sample data: {data.shape[0]} rows, {data.shape[1]} columns")
    
    create_histogram(data, 'Age', bins=15, title='Age Distribution')
    create_boxplot(data, 'Category', 'Score', title='Scores by Category')
    create_violinplot(data, 'Category', 'Income', title='Income Distribution')
    create_correlation_heatmap(data, title='Variable Correlations')
    create_kde_plot(data, 'Income', 'Score', title='Income vs Score Density')
    create_distplot(data, 'Value', title='Value Distribution')
    
    plt.show()
    
    return data


def run_all_examples():
    """Run all visualization examples."""
    print("\n" + "=" * 60)
    print("RUNNING ALL EXAMPLES")
    print("=" * 60 + "\n")
    
    print("1. Core Implementation...")
    core_implementation()
    plt.show()
    
    print("2. Banking Example...")
    banking_fig, banking_data = banking_example()
    plt.show()
    
    print("3. Healthcare Example...")
    healthcare_fig, healthcare_data = healthcare_example()
    plt.show()
    
    print("4. Interactive Scatter Matrix...")
    data = generate_sample_data(300)
    scatter_fig = create_scatter_matrix(data, ['Age', 'Income', 'Score', 'Value'], color_col='Category')
    scatter_fig.show()
    
    print("5. Pairplot with Regression...")
    reg_fig = create_pairplot_with_regression(data, 'Age', 'Income')
    plt.show()
    
    print("\nAll examples completed successfully!")
    
    return {
        'core': core_implementation(),
        'banking': banking_example(),
        'healthcare': healthcare_example()
    }


if __name__ == "__main__":
    main()