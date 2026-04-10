# Topic: Interactive_Plots_and_Annotations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Interactive Plots and Annotations

I. INTRODUCTION
   This module covers interactive plots and annotations.
   Prerequisites: Matplotlib basics
   Requirements: Matplotlib 3.5+
"""

import matplotlib.pyplot as plt
import numpy as np


def main():
    print("Executing Interactive Plots and Annotations")
    demonstrate_annotations()
    demonstrate_interactive()
    banking_application()
    healthcare_application()


def demonstrate_annotations():
    """Annotations"""
    print("\n--- Annotations ---")
    fig, ax = plt.subplots()
    ax.plot([1, 2, 3], [1, 2, 3])
    ax.annotate('Point', xy=(2, 2), xytext=(2.5, 2.5),
               arrowprops=dict(arrowstyle='->'))
    plt.close()


def demonstrate_interactive():
    """Interactive features"""
    print("\n--- Interactive ---")
    fig, ax = plt.subplots()
    ax.plot([1, 2, 3], [1, 4, 9])
    ax.grid(True)
    plt.close()


def banking_application():
    print("\n=== Annotated Stock Chart ===")
    fig, ax = plt.subplots()
    days = np.arange(30)
    prices = 100 + np.cumsum(np.random.randn(30))
    ax.plot(days, prices)
    ax.set_title('Stock Price with Annotations')
    ax.set_xlabel('Day')
    ax.set_ylabel('Price ($)')
    ax.grid(True)
    plt.close()


def healthcare_application():
    print("\n=== Annotated Vital Signs ===")
    fig, ax = plt.subplots()
    hours = np.arange(24)
    hr = 70 + np.random.randn(24) * 5
    ax.plot(hours, hr)
    ax.set_title('Heart Rate Over 24 Hours')
    ax.set_xlabel('Hour')
    ax.set_ylabel('Heart Rate (bpm)')
    ax.grid(True)
    plt.close()


def test_annotation():
    fig, ax = plt.subplots()
    ax.annotate('Test', xy=(1, 1), xytext=(1.5, 1.5))
    plt.close()
    print("Annotation test passed!")


if __name__ == "__main__":
    main()
    test_annotation()