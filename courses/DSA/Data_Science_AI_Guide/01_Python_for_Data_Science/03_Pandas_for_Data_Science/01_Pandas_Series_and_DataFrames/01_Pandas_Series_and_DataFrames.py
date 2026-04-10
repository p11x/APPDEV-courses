# Topic: Pandas_Series_and_DataFrames
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Pandas Series and DataFrames

I. INTRODUCTION
   Pandas is the primary data manipulation library in Python. This module
   covers Series and DataFrame creation, structure, and basic operations.
   Prerequisites: NumPy basics
   Requirements: Pandas 1.5+

II. CORE_CONCEPTS
   - Series creation and operations
   - DataFrame creation
   - Indexing and selection
   - DataFrame attributes
"""

import pandas as pd
import numpy as np


def main():
    print("Executing Pandas Series and DataFrames")
    demonstrate_series()
    demonstrate_dataframe()
    demonstrate_indexing()
    banking_application()
    healthcare_application()


def demonstrate_series():
    """Pandas Series"""
    print("\n--- Series from List ---")
    s = pd.Series([1, 2, 3, 4, 5])
    print(f"Series: {s.values}")
    print(f"Index: {s.index.tolist()}")
    
    print("\n--- Series with Index ---")
    s = pd.Series([1, 2, 3], index=['a', 'b', 'c'])
    print(f"Series: {s.to_dict()}")
    
    print("\n--- Series from Dict ---")
    d = {'a': 1, 'b': 2, 'c': 3}
    s = pd.Series(d)
    print(f"Series: {s.to_dict()}")
    
    print("\n--- Series Operations ---")
    s = pd.Series([1, 2, 3, 4, 5])
    print(f"Sum: {s.sum()}")
    print(f"Mean: {s.mean()}")
    print(f"Std: {s.std()}")


def demonstrate_dataframe():
    """Pandas DataFrame"""
    print("\n--- DataFrame from Dict ---")
    data = {'name': ['Alice', 'Bob', 'Charlie'],
           'age': [25, 30, 35],
           'score': [85, 90, 78]}
    df = pd.DataFrame(data)
    print(df)
    
    print("\n--- DataFrame from List of Dicts ---")
    data = [{'name': 'Alice', 'age': 25},
           {'name': 'Bob', 'age': 30}]
    df = pd.DataFrame(data)
    print(df)
    
    print("\n--- DataFrame Attributes ---")
    print(f"Shape: {df.shape}")
    print(f"Columns: {df.columns.tolist()}")
    print(f"Index: {df.index.tolist()}")
    print(f"dtypes:\\n{df.dtypes}")


def demonstrate_indexing():
    """Indexing DataFrames"""
    print("\n--- Select Columns ---")
    df = pd.DataFrame({'a': [1, 2, 3], 'b': [4, 5, 6]})
    print(f"df['a']: {df['a'].tolist()}")
    
    print("\n--- Select Rows ---")
    print(f"df.iloc[0]: {df.iloc[0].tolist()}")
    print(f"df.loc[0]: {df.loc[0].tolist()}")
    
    print("\n--- Boolean Selection ---")
    df = pd.DataFrame({'a': [1, 2, 3, 4], 'b': [5, 6, 7, 8]})
    print(f"df[df['a'] > 2]:\\n{df[df['a'] > 2]}")


def banking_application():
    """Banking DataFrame example"""
    print("\n=== Banking: Account Data ===")
    
    accounts = pd.DataFrame({
        'account_id': ['CHK001', 'SAV001', 'CC001'],
        'holder': ['Alice', 'Bob', 'Charlie'],
        'balance': [5000, 10000, 2500],
        'type': ['Checking', 'Savings', 'Credit']
    })
    print(accounts)


def healthcare_application():
    """Healthcare DataFrame example"""
    print("\n=== Healthcare: Patient Data ===")
    
    patients = pd.DataFrame({
        'patient_id': ['P001', 'P002', 'P003'],
        'name': ['John', 'Jane', 'Bob'],
        'age': [45, 38, 52],
        'glucose': [95, 105, 120]
    })
    print(patients)


def test_pandas():
    s = pd.Series([1, 2, 3])
    assert len(s) == 3
    
    df = pd.DataFrame({'a': [1, 2]})
    assert df.shape == (2, 1)
    
    print("Pandas tests passed!")


if __name__ == "__main__":
    main()
    test_pandas()