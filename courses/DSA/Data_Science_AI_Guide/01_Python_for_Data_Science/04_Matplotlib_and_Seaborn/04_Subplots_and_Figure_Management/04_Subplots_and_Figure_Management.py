# Topic: Subplots_and_Figure_Management
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Subplots and Figure Management

I. INTRODUCTION
   This module covers creating multiple subplots and managing figures.
   Prerequisites: Matplotlib basics
   Requirements: Matplotlib 3.5+
"""

import matplotlib.pyplot as plt
import numpy as np


def main():
    print("Executing Subplots and Figure Management")
    demonstrate_subplots()
    demonstrate_gridspec()
    banking_application()
    healthcare_application()


def demonstrate_subplots():
    """Multiple subplots"""
    print("\n--- Subplots ---")
    fig, axes = plt.subplots(2, 2)
    axes[0, 0].plot([1, 2, 3])
    axes[0, 1].plot([3, 2, 1])
    axes[1, 0].plot([1, 1, 1])
    axes[1, 1].plot([2, 2, 2])
    plt.close()


def demonstrate_gridspec():
    """GridSpec"""
    print("\n--- GridSpec ---")
    fig = plt.figure()
    plt.close()


def banking_application():
    print("\n=== Multi-Panel Banking Dashboard ===")
    fig, axes = plt.subplots(2, 2, figsize=(10, 8))
    axes[0, 0].set_title('Price History')
    axes[0, 1].set_title('Volume')
    axes[1, 0].set_title('Returns')
    axes[1, 1].set_title('Volatility')
    plt.close()


def healthcare_application():
    print("\n=== Multi-Panel Medical Dashboard ===")
    fig, axes = plt.subplots(2, 2, figsize=(10, 8))
    axes[0, 0].set_title('Heart Rate')
    axes[0, 1].set_title('Blood Pressure')
    axes[1, 0].set_title('Temperature')
    axes[1, 1].set_title('Oxygen Saturation')
    plt.close()


def test_subplot():
    fig, axes = plt.subplots(2, 1)
    assert len(axes) == 2
    plt.close()
    print("Subplot test passed!")


if __name__ == "__main__":
    main()
    test_subplot()