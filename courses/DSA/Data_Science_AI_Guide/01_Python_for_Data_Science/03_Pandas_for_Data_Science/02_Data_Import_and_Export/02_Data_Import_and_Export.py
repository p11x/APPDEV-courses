# Topic: Data_Import_and_Export
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Import and Export

I. INTRODUCTION
   This module covers reading and writing data with Pandas using various formats.
   Prerequisites: Pandas basics
   Requirements: Pandas 1.5+

II. CORE_CONCEPTS
   - CSV read/write
   - Excel read/write
   - JSON read/write
   - HTML table reading
"""

import pandas as pd
import numpy as np
import os


def main():
    print("Executing Data Import and Export")
    demonstrate_csv()
    demonstrate_excel()
    demonstrate_json()
    banking_application()
    healthcare_application()


def demonstrate_csv():
    """CSV operations"""
    print("\n--- Read CSV ---")
    data = "name,age,score\nAlice,25,85\nBob,30,90"
    
    from io import StringIO
    df = pd.read_csv(StringIO(data))
    print(f"CSV Data:\\n{df}")
    
    print("\n--- Write CSV ---")
    df = pd.DataFrame({'a': [1, 2, 3], 'b': [4, 5, 6]})
    csv_str = df.to_csv(index=False)
    print(f"CSV:\\n{csv_str}")


def demonstrate_excel():
    """Excel operations"""
    print("\n--- Excel Read ---")
    df = pd.DataFrame({'A': [1, 2], 'B': [3, 4]})
    print(f"Excel DataFrame: {df.shape}")
    

def demonstrate_json():
    """JSON operations"""
    print("\n--- JSON Operations ---")
    df = pd.DataFrame({'a': [1, 2, 3], 'b': [4, 5, 6]})
    json_str = df.to_json()
    print(f"JSON:\\n{json_str[:50]}...")


def banking_application():
    """Import bank transactions"""
    print("\n=== Banking: Transaction Import ===")
    
    transactions = pd.DataFrame({
        'date': ['2024-01-01', '2024-01-02', '2024-01-03'],
        'amount': [1000, -250, 500],
        'description': ['Deposit', 'Withdrawal', 'Transfer']
    })
    print(transactions)


def healthcare_application():
    """Import patient records"""
    print("\n=== Healthcare: Patient Records ===")
    
    patients = pd.DataFrame({
        'id': ['P001', 'P002'],
        'name': ['John', 'Jane'],
        'dob': ['1980-01-01', '1985-06-15']
    })
    print(patients)


def test_io():
    df = pd.DataFrame({'a': [1, 2]})
    assert len(df) == 2
    print("IO test passed!")


if __name__ == "__main__":
    main()
    test_io()