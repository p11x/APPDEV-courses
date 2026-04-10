# Topic: Array_Manipulation_and_Indexing
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Array Manipulation and Indexing

I. INTRODUCTION
   This module covers NumPy array manipulation operations including reshaping,
   slicing, stacking, and advanced indexing techniques.
   Prerequisites: NumPy basics
   Requirements: NumPy 1.21+

II. CORE_CONCEPTS
   - Array reshaping (reshape, flatten)
   - Array slicing
   - Fancy indexing
   - Boolean indexing
   - Stacking arrays (vstack, hstack, concatenate)

III. IMPLEMENTATION
   - Step-by-step code examples
   - Best practices for indexing

IV. EXAMPLES
   - Standard demonstration
   - Real-world Application 1: Banking - Portfolio rebalancing
   - Real-world Application 2: Healthcare - Patient cohort selection

V. OUTPUT_RESULTS
   - Expected outputs

VI. TESTING
   - Unit tests
"""

import numpy as np
from typing import Tuple, List


def main():
    print("Executing Array Manipulation and Indexing")
    print("\n=== Reshaping ===")
    demonstrate_reshape()
    
    print("\n=== Slicing ===")
    demonstrate_slicing()
    
    print("\n=== Fancy Indexing ===")
    demonstrate_fancy_indexing()
    
    print("\n=== Boolean Indexing ===")
    demonstrate_boolean_indexing()
    
    print("\n=== Stacking ===")
    demonstrate_stacking()
    
    print("\n=== Banking Application ===")
    banking_application()
    
    print("\n=== Healthcare Application ===")
    healthcare_application()


def demonstrate_reshape():
    """Demonstrate array reshaping"""
    print("\n--- Basic Reshape ---")
    arr = np.arange(12)
    print(f"Original: {arr}")
    print(f"Shape: {arr.shape}")
    
    reshaped = arr.reshape(3, 4)
    print(f"Reshaped (3,4):\\n{reshaped}")
    
    print("\n--- Flatten ---")
    arr2d = np.array([[1, 2, 3], [4, 5, 6]])
    flattened = arr2d.flatten()
    print(f"2D:\\n{arr2d}")
    print(f"Flattened: {flattened}")
    
    print("\n--- Resize ---")
    arr = np.arange(6)
    arr.resize(2, 3)
    print(f"Resized: {arr}")
    
    print("\n--- Transpose ---")
    arr = np.array([[1, 2, 3], [4, 5, 6]])
    print(f"Original:\\n{arr}")
    print(f"T:\\n{arr.T}")
    
    print("\n--- New Axis ---")
    arr = np.array([1, 2, 3])
    print(f"Original: {arr.shape}")
    expanded = arr[:, np.newaxis]
    print(f"Expanded: {expanded.shape}")


def demonstrate_slicing():
    """Demonstrate array slicing"""
    print("\n--- 1D Slicing ---")
    arr = np.arange(10)
    print(f"Array: {arr}")
    print(f"arr[2:5]: {arr[2:5]}")
    print(f"arr[:3]: {arr[:3]}")
    print(f"arr[::2]: {arr[::2]}")
    print(f"arr[::-1]: {arr[::-1]}")
    
    print("\n--- 2D Slicing ---")
    arr = np.array([[1, 2, 3, 4], 
                   [5, 6, 7, 8], 
                   [9, 10, 11, 12]])
    print(f"Array:\\n{arr}")
    print(f"'arr[0]': {arr[0]}")
    print(f"arr[0, :2]: {arr[0, :2]}")
    print(f"arr[:, 1]: {arr[:, 1]}")
    print(f"arr[1:3, 1:3]:\\n{arr[1:3, 1:3]}")


def demonstrate_fancy_indexing():
    """Demonstrate fancy indexing"""
    print("\n--- Integer Array Indexing ---")
    arr = np.array([10, 20, 30, 40, 50])
    indices = [0, 2, 4]
    print(f"Array: {arr}")
    print(f"Indices: {indices}")
    print(f"arr[indices]: {arr[indices]}")
    
    print("\n--- 2D Integer Indexing ---")
    arr = np.array([[1, 2, 3],
                    [4, 5, 6],
                    [7, 8, 9]])
    rows = [0, 0, 2]
    cols = [0, 2, 0]
    print(f"arr[rows, cols]: {arr[rows, cols]}")
    
    print("\n--- np.ix_ ---")
    arr = np.arange(9).reshape(3, 3)
    print(f"Array:\\n{arr}")
    print(f"arr[np.ix_([0,2], [0,2])]:\\n{arr[np.ix_([0,2], [0,2])]}")


def demonstrate_boolean_indexing():
    """Demonstrate boolean indexing"""
    print("\n--- Boolean Array Indexing ---")
    arr = np.array([1, 2, 3, 4, 5])
    mask = np.array([True, False, True, False, True])
    print(f"Array: {arr}")
    print(f"Mask: {mask}")
    print(f"arr[mask]: {arr[mask]}")
    
    print("\n--- Comparison Operations ---")
    arr = np.array([1, 5, 3, 8, 2, 9])
    print(f"Array: {arr}")
    print(f"arr > 3: {arr > 3}")
    print(f"arr[arr > 3]: {arr[arr > 3]}")
    
    print("\n--- Multiple Conditions ---")
    arr = np.arange(10)
    print(f"Array: {arr}")
    print(f"(arr >= 3) & (arr <= 7): {arr[(arr >= 3) & (arr <= 7)]}")
    
    print("\n--- np.where ---")
    arr = np.array([1, 2, 3, 4, 5])
    result = np.where(arr > 3, "big", "small")
    print(f"np.where: {result}")


def demonstrate_stacking():
    """Demonstrate array stacking"""
    print("\n--- Vertical Stack ---")
    a = np.array([1, 2, 3])
    b = np.array([4, 5, 6])
    print(f"a: {a}, b: {b}")
    print(f"vstack: {np.vstack([a, b])}")
    
    print("\n--- Horizontal Stack ---")
    print(f"hstack: {np.hstack([a, b])}")
    
    print("\n--- Concatenate ---")
    print(f"concatenate: {np.concatenate([a, b])}")
    
    print("\n--- 2D Concatenate ---")
    a = np.array([[1, 2], [3, 4]])
    b = np.array([[5, 6]])
    print(f"a:\\n{a}")
    print(f"b:\\n{b}")
    print(f"concat axis 0:\\n{np.concatenate([a, b], axis=0)}")
    print(f"concat axis 1:\\n{np.concatenate([a, b.T], axis=1)}")


def banking_application():
    """Banking use case"""
    print("\n=== Banking: Portfolio Rebalancing ===")
    
    stocks = np.array([
        [100, 105, 102, 108, 110],
        [50, 48, 52, 55, 53],
        [200, 195, 198, 205, 210],
        [75, 78, 72, 70, 68]
    ])
    names = ["AAPL", "GOOGL", "MSFT", "AMZN"]
    
    print("Stock prices over 5 days:")
    print(f"Prices:\\n{stocks}")
    
    print("\n--- Select Performing Stocks ---")
    avg_returns = (stocks[:, -1] - stocks[:, 0]) / stocks[:, 0]
    print(f"Average returns: {avg_returns}")
    mask = avg_returns > 0.05
    print(f"Stocks > 5% return: {names}")
    for i, name in enumerate(names):
        if mask[i]:
            print(f"  {name}: {avg_returns[i]*100:.1f}%")


def healthcare_application():
    """Healthcare use case"""
    print("\n=== Healthcare: Patient Cohort Selection ===")
    
    np.random.seed(42)
    patients = np.random.rand(100, 4)
    patient_ids = np.arange(100)
    
    print(f"Patient data shape: {patients.shape}")
    print(f"First 5 patients:\\n{patients[:5]}")
    
    print("\n--- Select High-Risk Patients ---")
    risk_score = patients[:, 2] * 0.4 + patients[:, 3] * 0.6
    high_risk = patient_ids[risk_score > 0.5]
    print(f"High-risk patients (>0.5): {len(high_risk)}")
    print(f"First 10: {high_risk[:10]}")


def test_reshape():
    arr = np.arange(12).reshape(3, 4)
    assert arr.shape == (3, 4)
    print("Reshape test passed!")


def test_slicing():
    arr = np.arange(10)
    assert len(arr[2:5]) == 3
    print("Slicing test passed!")


def test_indexing():
    arr = np.array([1, 2, 3, 4, 5])
    assert arr[arr > 3].sum() == 9
    print("Indexing test passed!")


def run_all_tests():
    test_reshape()
    test_slicing()
    test_indexing()
    print("\n=== All Tests Passed! ===")


if __name__ == "__main__":
    main()
    print("\n" + "="*60)
    run_all_tests()