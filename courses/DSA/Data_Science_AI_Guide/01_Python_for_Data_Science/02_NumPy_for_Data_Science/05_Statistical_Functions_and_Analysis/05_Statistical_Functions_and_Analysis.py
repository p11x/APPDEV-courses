# Topic: Statistical_Functions_and_Analysis
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Statistical Functions and Analysis

I. INTRODUCTION
   NumPy provides comprehensive statistical functions for data analysis.
   This module covers descriptive statistics, correlation, and histograms.
   Prerequisites: NumPy arrays
   Requirements: NumPy 1.21+

II. CORE_CONCEPTS
   - Descriptive statistics
   - Correlation and covariance
   - Histograms
   - Percentiles and quantiles
"""

import numpy as np


def main():
    print("Executing Statistical Functions and Analysis")
    demonstrate_stats()
    demonstrate_correlation()
    demonstrate_histogram()
    banking_application()
    healthcare_application()


def demonstrate_stats():
    """Descriptive statistics"""
    print("\n--- Basic Statistics ---")
    data = np.array([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
    print(f"Data: {data}")
    print(f"mean: {data.mean()}")
    print(f"median: {np.median(data)}")
    print(f"std: {data.std()}")
    print(f"var: {data.var()}")
    print(f"min: {data.min()}, max: {data.max()}")
    print(f"sum: {data.sum()}")
    print(f"percentile 25: {np.percentile(data, 25)}")
    print(f"percentile 75: {np.percentile(data, 75)}")


def demonstrate_correlation():
    """Correlation and covariance"""
    print("\n--- Correlation ---")
    x = np.array([1, 2, 3, 4, 5])
    y = np.array([2, 4, 6, 8, 10])
    print(f"x: {x}, y: {y}")
    print(f"corrcoef: {np.corrcoef(x, y)}")
    print(f"cov: {np.cov(x, y)}")


def demonstrate_histogram():
    """Histograms"""
    print("\n--- Histogram ---")
    data = np.random.randn(1000)
    hist, bin_edges = np.histogram(data, bins=10)
    print(f"Histogram values: {hist}")
    print(f"Bin edges: {bin_edges}")


def banking_application():
    """Statistical analysis for banking"""
    print("\n=== Banking: Return Statistics ===")
    
    returns = np.random.randn(252) * 0.01
    print(f"Daily returns (252 days):")
    print(f"  Mean: {returns.mean():.4f}")
    print(f"  Std: {returns.std():.4f}")
    print(f"  Sharpe: {returns.mean() / returns.std() * np.sqrt(252):.2f}")
    print(f"  VaR 5%: {np.percentile(returns, 5):.4f}")
    

def healthcare_application():
    """Statistical analysis for healthcare"""
    print("\n=== Healthcare: Lab Results Statistics ===")
    
    glucose = np.random.normal(100, 15, 100)
    print(f"Glucose levels (n=100):")
    print(f"  Mean: {glucose.mean():.1f}")
    print(f"  Std: {glucose.std():.1f}")
    print(f"  Normal (<100): {(glucose < 100).sum()}")


def test_stats():
    data = np.array([1, 2, 3, 4, 5])
    assert data.mean() == 3.0
    print("Stats test passed!")


if __name__ == "__main__":
    main()
    test_stats()