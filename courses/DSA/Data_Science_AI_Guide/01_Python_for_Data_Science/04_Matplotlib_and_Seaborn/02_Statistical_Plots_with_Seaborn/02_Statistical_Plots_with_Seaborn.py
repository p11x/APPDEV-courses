# Topic: Statistical_Plots_with_Seaborn
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Statistical Plots with Seaborn

I. INTRODUCTION
   Seaborn provides statistical visualization. This module covers statistical plots.
   Prerequisites: Matplotlib basics
   Requirements: Seaborn 0.11+
"""

import seaborn as sns
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd


def main():
    print("Executing Statistical Plots with Seaborn")
    demonstrate_histplot()
    demonstrate_boxplot()
    demonstrate_heatmap()
    banking_application()
    healthcare_application()


def demonstrate_histplot():
    """Histogram"""
    print("\n--- Histogram ---")
    data = np.random.randn(100)
    fig, ax = plt.subplots()
    sns.histplot(data, ax=ax)
    plt.close()


def demonstrate_boxplot():
    """Box plot"""
    print("\n--- Box Plot ---")
    df = pd.DataFrame({'A': [1, 2, 3, 4], 'B': [2, 3, 4, 5]})
    fig, ax = plt.subplots()
    sns.boxplot(data=df, ax=ax)
    plt.close()


def demonstrate_heatmap():
    """Heatmap"""
    print("\n--- Heatmap ---")
    data = pd.DataFrame(np.random.rand(5, 5))
    fig, ax = plt.subplots()
    sns.heatmap(data, ax=ax)
    plt.close()


def banking_application():
    print("\n=== Return Distribution ===")
    returns = np.random.randn(1000)
    fig, ax = plt.subplots()
    sns.histplot(returns, ax=ax, kde=True)
    plt.close()


def healthcare_application():
    print("\n=== Patient Data Distribution ===")
    data = pd.DataFrame({
        'age': np.random.randint(20, 70, 100),
        'bp': np.random.randint(100, 160, 100)
    })
    fig, ax = plt.subplots()
    sns.histplot(data=data, x='age', y='bp', ax=ax)
    plt.close()


def test_seaborn():
    fig, ax = plt.subplots()
    sns.histplot([1, 2, 3], ax=ax)
    plt.close()
    print("Seaborn test passed!")


if __name__ == "__main__":
    main()
    test_seaborn()