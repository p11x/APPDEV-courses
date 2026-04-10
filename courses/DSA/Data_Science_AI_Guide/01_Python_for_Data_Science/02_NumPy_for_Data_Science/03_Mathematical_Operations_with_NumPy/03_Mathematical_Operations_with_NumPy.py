# Topic: Mathematical_Operations_with_NumPy
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Mathematical Operations with NumPy

I. INTRODUCTION
   NumPy provides comprehensive mathematical functions for numerical computing.
   This module covers universal functions, mathematical transformations,
   and polynomial operations.
   Prerequisites: NumPy arrays
   Requirements: NumPy 1.21+

II. CORE_CONCEPTS
   - Trigonometric functions
   - Exponential and logarithmic
   - Rounding functions
   - Polynomial evaluation

III. IMPLEMENTATION
   - Code examples and best practices
"""

import numpy as np
import math


def main():
    print("Executing Mathematical Operations with NumPy")
    demonstrate_trig()
    demonstrate_explog()
    demonstrate_rounding()
    banking_application()
    healthcare_application()


def demonstrate_trig():
    """Trigonometric functions"""
    print("\n--- Trigonometric Functions ---")
    angles = np.array([0, np.pi/4, np.pi/2, np.pi])
    print(f"Angles: {angles}")
    print(f"sin: {np.sin(angles)}")
    print(f"cos: {np.cos(angles)}")
    print(f"tan: {np.tan(angles)}")
    
    print("\n--- Inverse Trig ---")
    x = np.array([0, 0.5, 1])
    print(f"x: {x}")
    print(f"arcsin: {np.arcsin(x)}")
    print(f"arccos: {np.arccos(x)}")
    print(f"arctan: {np.arctan(x)}")


def demonstrate_explog():
    """Exponential and logarithmic"""
    print("\n--- Exponential ---")
    x = np.array([0, 1, 2])
    print(f"x: {x}")
    print(f"exp: {np.exp(x)}")
    print(f"exp2: {np.exp2(x)}")
    
    print("\n--- Logarithmic ---")
    x = np.array([1, 2, math.e])
    print(f"x: {x}")
    print(f"log: {np.log(x)}")
    print(f"log10: {np.log10(x)}")
    print(f"log2: {np.log2(x)}")


def demonstrate_rounding():
    """Rounding functions"""
    print("\n--- Around ---")
    x = np.array([1.5, 2.5, 3.5])
    print(f"x: {x}")
    print(f"around: {np.around(x)}")
    
    print("\n--- Floor and Ceil ---")
    x = np.array([1.1, 1.9, 2.0])
    print(f"x: {x}")
    print(f"floor: {np.floor(x)}")
    print(f"ceil: {np.ceil(x)}")
    print(f"trunc: {np.trunc(x)}")


def banking_application():
    """Banking use case"""
    print("\n=== Banking: Financial Calculations ===")
    
    initial = 10000
    rate = 0.05
    years = np.arange(5)
    
    amounts = initial * np.exp(rate * years)
    print(f"Compound interest over {len(years)} years:")
    for y, a in zip(years, amounts):
        print(f"  Year {y}: ${a:.2f}")


def healthcare_application():
    """Healthcare use case"""
    print("\n=== Healthcare: pH Calculation ===")
    
    h_concentration = np.array([1e-7, 1e-6, 1e-5])
    pH = -np.log10(h_concentration)
    print(f"H+ concentration: {h_concentration}")
    print(f"pH values: {pH}")


def test_math():
    assert np.sin(np.pi/2) == 1.0
    print("Math test passed!")


if __name__ == "__main__":
    main()
    test_math()