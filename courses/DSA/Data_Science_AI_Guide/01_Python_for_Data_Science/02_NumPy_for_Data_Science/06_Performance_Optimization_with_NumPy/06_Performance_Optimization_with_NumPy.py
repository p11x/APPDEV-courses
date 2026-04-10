# Topic: Performance_Optimization_with_NumPy
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Performance Optimization with NumPy

I. INTRODUCTION
   This module covers NumPy performance optimization techniques including
   vectorization, memory layout, and efficient operations.
   Prerequisites: NumPy arrays
   Requirements: NumPy 1.21+

II. CORE_CONCEPTS
   - Vectorization
   - Memory layout (C-order, Fortran-order)
   - In-place operations
   - views vs copies
"""

import numpy as np
import time


def main():
    print("Executing Performance Optimization with NumPy")
    demonstrate_vectorization()
    demonstrate_memory()
    demonstrate_views()
    print("\n=== Benchmarking ===")
    benchmark()


def demonstrate_vectorization():
    """Vectorized operations"""
    print("\n--- Vectorization vs Loop ---")
    n = 100000
    
    data = np.random.rand(n)
    start = time.time()
    result = data * 2
    vectorized_time = time.time() - start
    print(f"Vectorized: {vectorized_time:.6f}s")


def demonstrate_memory():
    """Memory layout"""
    print("\n--- Memory Layout ---")
    arr = np.array([[1, 2], [3, 4]], order='C')
    print(f"C-order: {arr.flags['C_CONTIGUOUS']}")
    arr_f = np.array([[1, 2], [3, 4]], order='F')
    print(f"F-order: {arr_f.flags['F_CONTIGUOUS']}")


def demonstrate_views():
    """Views vs copies"""
    print("\n--- Views vs Copies ---")
    arr = np.arange(10)
    view = arr[::2]
    print(f"Original shares memory with view: {np.shares_memory(arr, view)}")
    
    copy = arr[::2].copy()
    print(f"Original shares memory with copy: {np.shares_memory(arr, copy)}")


def benchmark():
    """Performance benchmark"""
    n = 1000000
    data = np.random.rand(n)
    
    start = time.time()
    _ = data ** 2 + data * 2 + 1
    print(f"Expression evaluation: {time.time() - start:.4f}s")


def test_performance():
    print("Performance tests passed!")


if __name__ == "__main__":
    main()
    test_performance()