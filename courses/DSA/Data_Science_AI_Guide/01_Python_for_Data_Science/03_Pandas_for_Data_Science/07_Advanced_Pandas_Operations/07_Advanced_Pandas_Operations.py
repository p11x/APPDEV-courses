# Topic: Advanced_Pandas_Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Advanced Pandas Operations

I. INTRODUCTION
   Advanced Pandas operations including pivot tables, merging, and concatenation.
   Prerequisites: Pandas basics
   Requirements: Pandas 1.5+
"""

import pandas as pd
import numpy as np


def main():
    print("Executing Advanced Pandas Operations")
    demonstrate_pivot()
    demonstrate_merge()
    demonstrate_concat()
    banking_application()
    healthcare_application()


def demonstrate_pivot():
    """Pivot tables"""
    print("\n--- Pivot Table ---")
    df = pd.DataFrame({'A': ['foo', 'foo', 'bar', 'bar'],
                      'B': ['one', 'one', 'two', 'two'],
                      'C': [1, 2, 3, 4]})
    print(df.pivot(index='A', columns='B', values='C'))


def demonstrate_merge():
    """Merge DataFrames"""
    print("\n--- Merge ---")
    df1 = pd.DataFrame({'key': ['a', 'b'], 'val1': [1, 2]})
    df2 = pd.DataFrame({'key': ['a', 'b'], 'val2': [3, 4]})
    print(pd.merge(df1, df2, on='key'))


def demonstrate_concat():
    """Concatenate DataFrames"""
    print("\n--- Concat ---")
    df1 = pd.DataFrame({'A': [1, 2], 'B': [3, 4]})
    df2 = pd.DataFrame({'A': [5, 6], 'B': [7, 8]})
    print(pd.concat([df1, df2]))


def banking_application():
    print("\n=== Account Merge ===")
    accounts = pd.DataFrame({'id': ['A001', 'A002'], 'balance': [1000, 2000]})
    holdings = pd.DataFrame({'id': ['A001', 'A002'], 'stock': ['AAPL', 'GOOG']})
    merged = pd.merge(accounts, holdings, on='id')
    print(merged)


def healthcare_application():
    print("\n=== Patient Merge ===")
    demographics = pd.DataFrame({'id': ['P001', 'P002'], 'age': [45, 38]})
    vitals = pd.DataFrame({'id': ['P001', 'P002'], 'hr': [72, 75]})
    merged = pd.merge(demographics, vitals, on='id')
    print(merged)


def test_advanced():
    df1 = pd.DataFrame({'a': [1, 2]})
    df2 = pd.DataFrame({'a': [3, 4]})
    result = pd.concat([df1, df2])
    assert len(result) == 4
    print("Advanced test passed!")


if __name__ == "__main__":
    main()
    test_advanced()