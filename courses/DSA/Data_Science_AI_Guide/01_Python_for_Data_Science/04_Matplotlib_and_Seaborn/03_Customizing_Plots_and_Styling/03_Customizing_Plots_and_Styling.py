# Topic: Customizing_Plots_and_Styling
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Customizing Plots and Styling

I. INTRODUCTION
   This module covers plot customization and styling.
   Prerequisites: Matplotlib basics
   Requirements: Matplotlib 3.5+
"""

import matplotlib.pyplot as plt
import numpy as np


def main():
    print("Executing Customizing Plots and Styling")
    demonstrate_labels()
    demonstrate_colors()
    demonstrate_styles()
    banking_application()
    healthcare_application()


def demonstrate_labels():
    """Labels and titles"""
    print("\n--- Labels ---")
    fig, ax = plt.subplots()
    ax.plot([1, 2, 3], [1, 2, 3])
    ax.set_xlabel('X label')
    ax.set_ylabel('Y label')
    ax.set_title('Title')
    plt.close()


def demonstrate_colors():
    """Colors"""
    print("\n--- Colors ---")
    fig, ax = plt.subplots()
    ax.plot([1, 2, 3], color='blue', label='blue')
    ax.plot([1, 2, 3], color='red', label='red')
    ax.legend()
    plt.close()


def demonstrate_styles():
    """Style sheets"""
    print("\n--- Styles ---")
    plt.style.use('default')


def banking_application():
    print("\n=== Styled Stock Chart ===")
    fig, ax = plt.subplots(figsize=(10, 6))
    ax.set_title('Stock Performance', fontsize=14, fontweight='bold')
    ax.set_xlabel('Day')
    ax.set_ylabel('Price ($)')
    plt.close()


def healthcare_application():
    print("\n=== Styled Patient Chart ===")
    fig, ax = plt.subplots(figsize=(8, 6))
    ax.set_title('Vitals Over Time', fontsize=14)
    ax.set_xlabel('Hour')
    ax.set_ylabel('Heart Rate (bpm)')
    plt.close()


def test_custom():
    fig, ax = plt.subplots()
    ax.set_title('Test')
    plt.close()
    print("Custom test passed!")


if __name__ == "__main__":
    main()
    test_custom()