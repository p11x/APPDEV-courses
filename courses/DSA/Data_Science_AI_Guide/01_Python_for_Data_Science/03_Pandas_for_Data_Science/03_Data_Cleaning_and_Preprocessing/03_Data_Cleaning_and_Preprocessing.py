# Topic: Data_Cleaning_and_Preprocessing
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Cleaning and Preprocessing

I. INTRODUCTION
   This module covers data cleaning techniques including handling missing values,
   removing duplicates, and data type conversions.
   Prerequisites: Pandas basics
   Requirements: Pandas 1.5+

II. CORE_CONCEPTS
   - Handle missing values
   - Remove duplicates
   - Data type conversion
   - String cleaning
"""

import pandas as pd
import numpy as np


def main():
    print("Executing Data Cleaning and Preprocessing")
    demonstrate_missing()
    demonstrate_duplicates()
    demonstrate_type_conversion()
    banking_application()
    healthcare_application()


def demonstrate_missing():
    """Handle missing values"""
    print("\n--- Detect Missing Values ---")
    df = pd.DataFrame({'A': [1, np.nan, 3],
                     'B': [4, 5, np.nan]})
    print(f"Original:\\n{df}")
    print(f"isnull:\\n{df.isnull()}")
    print(f"notnull:\\n{df.notnull()}")
    
    print("\n--- Fill Missing ---")
    df_filled = df.fillna(0)
    print(f"Filled:\\n{df_filled}")
    
    print("\n--- Drop Missing ---")
    df_drop = df.dropna()
    print(f"Dropped NA:\\n{df_drop}")


def demonstrate_duplicates():
    """Handle duplicates"""
    print("\n--- Find Duplicates ---")
    df = pd.DataFrame({'A': [1, 2, 2, 3], 'B': [4, 5, 5, 6]})
    print(f"Original:\\n{df}")
    print(f"duplicated: {df.duplicated().tolist()}")
    
    print("\n--- Drop Duplicates ---")
    df_unique = df.drop_duplicates()
    print(f"Unique:\\n{df_unique}")


def demonstrate_type_conversion():
    """Data type conversion"""
    print("\n--- Type Conversion ---")
    df = pd.DataFrame({'A': ['1', '2', '3']})
    print(f"Before: {df['A'].dtype}")
    df['A'] = df['A'].astype(int)
    print(f"After: {df['A'].dtype}")


def banking_application():
    """Clean bank data"""
    print("\n=== Banking: Clean Transaction Data ===")
    
    transactions = pd.DataFrame({
        'date': ['2024-01-01', '2024-01-02', '2024-01-02'],
        'amount': [1000, np.nan, 500],
        'account': ['CHK001', 'CHK001', 'CHK001']
    })
    print(f"Original: {len(transactions)} rows")
    cleaned = transactions.dropna()
    cleaned = cleaned.drop_duplicates()
    print(f"Cleaned: {len(cleaned)} rows")


def healthcare_application():
    """Clean patient data"""
    print("\n=== Healthcare: Clean Patient Data ===")
    
    patients = pd.DataFrame({
        'id': ['P001', 'P001', 'P002'],
        'name': ['John', 'John', 'Jane'],
        'age': [45, 45, np.nan]
    })
    print(f"Original: {len(patients)} rows")
    cleaned = patients.dropna()
    cleaned = cleaned.drop_duplicates(subset=['id'])
    print(f"Cleaned: {len(cleaned)} rows")


def test_cleaning():
    df = pd.DataFrame({'A': [1, np.nan, 3]})
    filled = df.fillna(0)
    assert filled.isnull().sum().sum() == 0
    print("Cleaning test passed!")


if __name__ == "__main__":
    main()
    test_cleaning()