# Topic: Group_By_Operations_and_Aggregation
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Group By Operations and Aggregation

I. INTRODUCTION
   This module covers groupby operations and aggregations.
   Prerequisites: Pandas basics
   Requirements: Pandas 1.5+
"""

import pandas as pd
import numpy as np


def main():
    print("Executing Group By Operations and Aggregation")
    demonstrate_groupby()
    demonstrate_aggregation()
    banking_application()
    healthcare_application()


def demonstrate_groupby():
    """Group by operations"""
    print("\n--- Group By ---")
    df = pd.DataFrame({'category': ['A', 'B', 'A', 'B'],
                     'value': [1, 2, 3, 4]})
    grouped = df.groupby('category')
    print(grouped.sum())


def demonstrate_aggregation():
    """Aggregations"""
    print("\n--- Multiple Aggregations ---")
    df = pd.DataFrame({'cat': ['A', 'B', 'A', 'B'],
                      'val': [1, 2, 3, 4]})
    print(df.groupby('cat').agg({'val': ['sum', 'mean']}))


def banking_application():
    print("\n=== Transaction Aggregation ===")
    df = pd.DataFrame({'type': ['debit', 'credit', 'debit'],
                     'amount': [100, 200, 150]})
    print(df.groupby('type').sum())


def healthcare_application():
    print("\n=== Patient Aggregation ===")
    df = pd.DataFrame({'dept': ['cardio', 'neuro', 'cardio'],
                     'age': [45, 52, 38]})
    print(df.groupby('dept').mean())


def test_groupby():
    df = pd.DataFrame({'a': [1, 1, 2], 'b': [1, 2, 3]})
    assert df.groupby('a').sum().loc[1] == 3
    print("Groupby test passed!")


if __name__ == "__main__":
    main()
    test_groupby()