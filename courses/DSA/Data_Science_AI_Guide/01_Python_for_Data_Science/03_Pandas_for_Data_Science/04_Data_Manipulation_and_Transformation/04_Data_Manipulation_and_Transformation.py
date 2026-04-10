# Topic: Data_Manipulation_and_Transformation
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Data Manipulation and Transformation

I. INTRODUCTION
   This module covers applying functions, mapping, and transforming data.
   Prerequisites: Pandas basics
   Requirements: Pandas 1.5+
"""

import pandas as pd
import numpy as np


def main():
    print("Executing Data Manipulation and Transformation")
    demonstrate_apply()
    demonstrate_map()
    banking_application()
    healthcare_application()


def demonstrate_apply():
    """Apply functions"""
    print("\n--- Apply Function to Column ---")
    df = pd.DataFrame({'a': [1, 2, 3], 'b': [4, 5, 6]})
    df['c'] = df['a'].apply(lambda x: x * 2)
    print(df)


def demonstrate_map():
    """Map values"""
    print("\n--- Map Values ---")
    s = pd.Series(['A', 'B', 'C'])
    mapped = s.map({'A': 'Apple', 'B': 'Banana', 'C': 'Cherry'})
    print(mapped)


def banking_application():
    print("\n=== Transform Account Data ===")
    df = pd.DataFrame({'balance': [1000, 2000, 3000]})
    df['balance_adj'] = df['balance'].apply(lambda x: x * 1.05)
    print(df)


def healthcare_application():
    print("\n=== Transform Patient Data ===")
    df = pd.DataFrame({'age': [25, 35, 45]})
    df['age_group'] = df['age'].apply(lambda x: 'Senior' if x > 40 else 'Adult')
    print(df)


def test_transform():
    df = pd.DataFrame({'a': [1, 2, 3]})
    df['b'] = df['a'].apply(lambda x: x * 2)
    assert df['b'].iloc[0] == 2
    print("Transform test passed!")


if __name__ == "__main__":
    main()
    test_transform()