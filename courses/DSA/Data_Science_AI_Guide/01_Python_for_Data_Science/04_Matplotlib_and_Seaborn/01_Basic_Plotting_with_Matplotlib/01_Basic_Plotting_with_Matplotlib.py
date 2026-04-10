# Topic: Basic_Plotting_with_Matplotlib
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Basic Plotting with Matplotlib

I. INTRODUCTION
   Matplotlib is the primary visualization library in Python. This module covers basic plots.
   Prerequisites: NumPy basics
   Requirements: Matplotlib 3.5+
"""

import matplotlib.pyplot as plt
import numpy as np


def main():
    print("Executing Basic Plotting with Matplotlib")
    demonstrate_line()
    demonstrate_scatter()
    demonstrate_bar()
    banking_application()
    healthcare_application()


def demonstrate_line():
    """Line plot"""
    print("\n--- Line Plot ---")
    x = np.linspace(0, 10, 50)
    y = np.sin(x)
    fig, ax = plt.subplots()
    ax.plot(x, y)
    plt.close()


def demonstrate_scatter():
    """Scatter plot"""
    print("\n--- Scatter Plot ---")
    x = np.random.rand(50)
    y = np.random.rand(50)
    fig, ax = plt.subplots()
    ax.scatter(x, y)
    plt.close()


def demonstrate_bar():
    """Bar chart"""
    print("\n--- Bar Chart ---")
    categories = ['A', 'B', 'C']
    values = [10, 20, 15]
    fig, ax = plt.subplots()
    ax.bar(categories, values)
    plt.close()


def banking_application():
    """Bank plot"""
    print("\n=== Stock Price Chart ===")
    days = np.arange(30)
    prices = 100 + np.cumsum(np.random.randn(30))
    fig, ax = plt.subplots()
    ax.plot(days, prices)
    plt.close()


def healthcare_application():
    """Healthcare plot"""
    print("\n=== Patient Vitals Chart ===")
    time_points = np.arange(10)
    heart_rate = 70 + np.random.randn(10) * 5
    fig, ax = plt.subplots()
    ax.plot(time_points, heart_rate)
    plt.close()


def test_plot():
    fig, ax = plt.subplots()
    ax.plot([1, 2, 3], [1, 2, 3])
    plt.close()
    print("Plot test passed!")


if __name__ == "__main__":
    main()
    test_plot()