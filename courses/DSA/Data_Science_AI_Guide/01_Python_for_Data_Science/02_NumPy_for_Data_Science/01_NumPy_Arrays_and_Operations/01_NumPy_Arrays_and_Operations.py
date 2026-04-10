# Topic: NumPy_Arrays_and_Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for NumPy Arrays and Operations

I. INTRODUCTION
   NumPy is the foundation of data science in Python, providing efficient
   numerical array operations. This module covers array creation, basic
   operations, and data types.
   Prerequisites: Python fundamentals
   Requirements: NumPy 1.21+

II. CORE_CONCEPTS
   - NumPy array creation
   - Array attributes (shape, dtype, ndim)
   - Data types (int, float, complex)
   - Array indexing
   - Basic operations

III. IMPLEMENTATION
   - Step-by-step code examples
   - Best practices for NumPy
   - Detailed comments throughout

IV. EXAMPLES
   - Standard demonstration
   - Real-world Application 1: Banking - Price arrays
   - Real-world Application 2: Healthcare - Vital signs arrays

V. OUTPUT_RESULTS
   - Expected outputs
   - Performance analysis

VI. TESTING
   - Unit tests for main functions

VII. ADVANCED_TOPICS
   - Structured arrays
   - Memory layout
   - Broadcasting basics

VIII. CONCLUSION
   - Key takeaways
   - Next steps for learning
"""

import numpy as np
from typing import Tuple, Optional, Any


def main():
    print("Executing NumPy Arrays and Operations implementation")
    print("\n=== Array Creation ===")
    demonstrate_array_creation()
    
    print("\n=== Array Attributes ===")
    demonstrate_array_attributes()
    
    print("\n=== Data Types ===")
    demonstrate_data_types()
    
    print("\n=== Basic Operations ===")
    demonstrate_basic_operations()
    
    print("\n=== Banking Application ===")
    banking_application()
    
    print("\n=== Healthcare Application ===")
    healthcare_application()


def demonstrate_array_creation():
    """Demonstrate array creation methods"""
    print("\n--- Create from Python List ---")
    arr1d = np.array([1, 2, 3, 4, 5])
    print(f"1D array: {arr1d}")
    print(f"Type: {type(arr1d)}")
    
    arr2d = np.array([[1, 2, 3], [4, 5, 6]])
    print(f"2D array:\\n{arr2d}")
    
    print("\n--- Using arange ---")
    arr = np.arange(10)
    print(f"arange(10): {arr}")
    
    arr = np.arange(0, 20, 2)
    print(f"arange(0, 20, 2): {arr}")
    
    print("\n--- Using linspace ---")
    arr = np.linspace(0, 10, 5)
    print(f"linspace(0, 10, 5): {arr}")
    
    print("\n--- Zeros and Ones ---")
    zeros = np.zeros(5)
    print(f"zeros(5): {zeros}")
    
    ones = np.ones((3, 3))
    print(f"ones((3,3)):\\n{ones}")
    
    print("\n--- Empty and Full ---")
    empty = np.empty(5)
    print(f"empty(5): {empty}")
    
    full = np.full(5, 7)
    print(f"full(5, 7): {full}")
    
    print("\n--- Random Arrays ---")
    rand = np.random.rand(5)
    print(f"random.rand(5): {rand}")
    
    randn = np.random.randn(3, 3)
    print(f"random.randn(3,3):\\n{randn}")
    
    randint = np.random.randint(1, 10, (3, 3))
    print(f"random.randint(1, 10, (3,3)):\\n{randint}")


def demonstrate_array_attributes():
    """Demonstrate array attributes"""
    print("\n--- Shape and Size ---")
    arr = np.array([[1, 2, 3], [4, 5, 6]])
    print(f"Array:\\n{arr}")
    print(f"Shape: {arr.shape}")
    print(f"Size: {arr.size}")
    print(f"ndim: {arr.ndim}")
    
    print("\n--- Data Type ---")
    arr = np.array([1, 2, 3])
    print(f"Integer array dtype: {arr.dtype}")
    
    arr = np.array([1.5, 2.5, 3.5])
    print(f"Float array dtype: {arr.dtype}")
    
    print("\n--- Strides ---")
    arr = np.array([[1, 2, 3], [4, 5, 6]])
    print(f"Array:\\n{arr}")
    print(f"Strides: {arr.strides}")
    
    print("\n--- Memory Info ---")
    arr = np.zeros(1000)
    print(f"Itemsize: {arr.itemsize} bytes")
    print(f"nbytes: {arr.nbytes} bytes")


def demonstrate_data_types():
    """Demonstrate NumPy data types"""
    print("\n--- Integer Types ---")
    arr_int8 = np.array([1, 2, 127], dtype=np.int8)
    print(f"int8: {arr_int8}, dtype: {arr_int8.dtype}")
    
    arr_int32 = np.array([1, 2, 3], dtype=np.int32)
    print(f"int32: {arr_int32}, dtype: {arr_int32.dtype}")
    
    arr_int64 = np.array([1, 2, 3], dtype=np.int64)
    print(f"int64: {arr_int64}, dtype: {arr_int64.dtype}")
    
    print("\n--- Float Types ---")
    arr_float16 = np.array([1.5, 2.5, 3.5], dtype=np.float16)
    print(f"float16: {arr_float16}, dtype: {arr_float16.dtype}")
    
    arr_float32 = np.array([1.5, 2.5, 3.5], dtype=np.float32)
    print(f"float32: {arr_float32}, dtype: {arr_float32.dtype}")
    
    arr_float64 = np.array([1.5, 2.5, 3.5], dtype=np.float64)
    print(f"float64: {arr_float64}, dtype: {arr_float64.dtype}")
    
    print("\n--- Complex and Other Types ---")
    arr_complex = np.array([1+2j, 3+4j], dtype=np.complex128)
    print(f"complex: {arr_complex}, dtype: {arr_complex.dtype}")
    
    arr_bool = np.array([True, False, True], dtype=np.bool_)
    print(f"bool: {arr_bool}, dtype: {arr_bool.dtype}")
    
    print("\n--- Type Conversion ---")
    arr = np.array([1, 2, 3])
    arr_float = arr.astype(float)
    print(f"int -> float: {arr_float.dtype}")
    
    arr_str = arr.astype(str)
    print(f"int -> str: {arr_str.dtype}")


def demonstrate_basic_operations():
    """Demonstrate basic array operations"""
    print("\n--- Element-wise Operations ---")
    a = np.array([1, 2, 3])
    b = np.array([4, 5, 6])
    
    print(f"a = {a}")
    print(f"b = {b}")
    print(f"a + b = {a + b}")
    print(f"a - b = {a - b}")
    print(f"a * b = {a * b}")
    print(f"a / b = {a / b}")
    
    print("\n--- Scalar Operations ---")
    a = np.array([1, 2, 3])
    
    print(f"a = {a}")
    print(f"a + 5 = {a + 5}")
    print(f"a * 2 = {a * 2}")
    print(f"a ** 2 = {a ** 2}")
    
    print("\n--- Aggregation Functions ---")
    a = np.array([1, 2, 3, 4, 5])
    
    print(f"a = {a}")
    print(f"sum: {a.sum()}")
    print(f"mean: {a.mean()}")
    print(f"std: {a.std()}")
    print(f"min: {a.min()}")
    print(f"max: {a.max()}")
    
    print("\n--- Axis Operations ---")
    arr = np.array([[1, 2, 3], [4, 5, 6]])
    print(f"Array:\\n{arr}")
    print(f"Sum (axis 0): {arr.sum(axis=0)}")
    print(f"Sum (axis 1): {arr.sum(axis=1)}")
    print(f"Mean (axis 0): {arr.mean(axis=0)}")


def practical_example():
    """Practical demonstration"""
    print("\n=== Practical Example: Temperature Data ===")
    
    temps = np.array([
        [72, 75, 78, 74, 71],
        [68, 70, 72, 69, 65],
        [75, 78, 82, 79, 76]
    ])
    
    print(f"Temperature data:\\n{temps}")
    print(f"Overall mean: {temps.mean():.2f}")
    print(f"Daily mean: {temps.mean(axis=1)}")
    print(f"Hourly mean: {temps.mean(axis=0)}")
    print(f"Min/Max: {temps.min()}, {temps.max()}")


class FinancialArray:
    """Financial array operations"""
    @staticmethod
    def calculate_returns(prices: np.ndarray) -> np.ndarray:
        """Calculate simple returns"""
        return np.diff(prices) / prices[:-1]
    
    @staticmethod
    def calculate_log_returns(prices: np.ndarray) -> np.ndarray:
        """Calculate log returns"""
        return np.log(prices[1:] / prices[:-1])
    
    @staticmethod
    def calculate_volatility(returns: np.ndarray) -> float:
        """Calculate volatility (std of returns)"""
        return returns.std() * np.sqrt(252)
    
    @staticmethod
    def calculate_sharpe_ratio(returns: np.ndarray, 
                               risk_free_rate: float = 0.02) -> float:
        """Calculate Sharpe ratio"""
        excess_returns = returns.mean() - risk_free_rate / 252
        return excess_returns / returns.std() * np.sqrt(252)


def banking_application():
    """
    Banking/Finance use case - Price arrays
    """
    print("\n=== Banking Application: Financial Data Arrays ===")
    
    print("\n--- Stock Price Analysis ---")
    prices = np.array([
        100.0, 102.5, 101.0, 105.0, 103.5, 
        108.0, 110.0, 109.5, 112.0, 115.0
    ])
    
    print(f"Prices: {prices}")
    print(f"Mean price: {prices.mean():.2f}")
    print(f"Price std: {prices.std():.2f}")
    
    returns = FinancialArray.calculate_returns(prices)
    print(f"Daily returns: {returns}")
    print(f"Mean return: {returns.mean():.4f}")
    print(f"Volatility (annualized): {FinancialArray.calculate_volatility(returns):.4f}")
    print(f"Sharpe ratio: {FinancialArray.calculate_sharpe_ratio(returns):.4f}")
    
    print("\n--- Portfolio Values ---")
    shares = np.array([100, 150, 200, 75, 50])
    prices = np.array([50, 100, 75, 200, 150])
    
    portfolio_value = (shares * prices).sum()
    print(f"Shares: {shares}")
    print(f"Prices: {prices}")
    print(f"Total value: ${portfolio_value:,.2f}")


class MedicalArray:
    """Medical array operations"""
    @staticmethod
    def calculate_bmi(weight_kg: np.ndarray, 
                       height_m: np.ndarray) -> np.ndarray:
        """Calculate BMI"""
        return weight_kg / (height_m ** 2)
    
    @staticmethod
    def classify_bmi(bmi: np.ndarray) -> np.ndarray:
        """Classify BMI categories"""
        categories = np.empty_like(bmi, dtype=object)
        categories[bmi < 18.5] = "Underweight"
        categories[(bmi >= 18.5) & (bmi < 25)] = "Normal"
        categories[(bmi >= 25) & (bmi < 30)] = "Overweight"
        categories[bmi >= 30] = "Obese"
        return categories
    
    @staticmethod
    def calculate_pulse_pressure(systolic: np.ndarray,
                                diastolic: np.ndarray) -> np.ndarray:
        """Calculate pulse pressure"""
        return systolic - diastolic
    
    @staticmethod
    def calculate_mean_arterial_pressure(systolic: np.ndarray,
                                       diastolic: np.ndarray) -> np.ndarray:
        """Calculate MAP"""
        return diastolic + (systolic - diastolic) / 3


def healthcare_application():
    """
    Healthcare use case - Vital signs arrays
    """
    print("\n=== Healthcare Application: Vital Signs Arrays ===")
    
    print("\n--- Patient BMI Calculation ---")
    weights = np.array([70, 85, 95, 60, 75])  # kg
    heights = np.array([1.75, 1.80, 1.70, 1.65, 1.78])  # meters
    
    bmi = MedicalArray.calculate_bmi(weights, heights)
    categories = MedicalArray.classify_bmi(bmi)
    
    for i in range(len(weights)):
        print(f"Patient {i+1}: Weight={weights[i]}kg, "
              f"Height={heights[i]}m, BMI={bmi[i]:.1f}, "
              f"Category={categories[i]}")
    
    print("\n--- Blood Pressure Analysis ---")
    systolic = np.array([120, 135, 142, 128, 115])
    diastolic = np.array([80, 88, 92, 85, 75])
    
    pulse_pressure = MedicalArray.calculate_pulse_pressure(systolic, diastolic)
    map_pressure = MedicalArray.calculate_mean_arterial_pressure(
        systolic, diastolic
    )
    
    for i in range(len(systolic)):
        print(f"Patient {i+1}: BP={systolic[i]}/{diastolic[i]}, "
              f"Pulse Pressure={pulse_pressure[i]}, MAP={map_pressure[i]:.0f}")


def test_array_creation():
    """Test array creation"""
    print("\n=== Testing Array Creation ===")
    
    arr = np.array([1, 2, 3])
    assert arr.shape == (3,)
    
    arr = np.zeros(5)
    assert arr.shape == (5,)
    
    arr = np.arange(10)
    assert len(arr) == 10
    
    print("All array creation tests passed!")


def test_array_attributes():
    """Test array attributes"""
    print("\n=== Testing Array Attributes ===")
    
    arr = np.array([[1, 2], [3, 4]])
    assert arr.shape == (2, 2)
    assert arr.ndim == 2
    assert arr.size == 4
    
    print("All array attribute tests passed!")


def test_data_types():
    """Test data types"""
    print("\n=== Testing Data Types ===")
    
    arr = np.array([1, 2, 3], dtype=np.int32)
    assert arr.dtype == np.int32
    
    arr = np.array([1.5, 2.5], dtype=np.float64)
    assert arr.dtype == np.float64
    
    print("All data type tests passed!")


def test_basic_operations():
    """Test basic operations"""
    print("\n=== Testing Basic Operations ===")
    
    a = np.array([1, 2, 3])
    b = np.array([4, 5, 6])
    
    assert (a + b).sum() == 21
    assert (a * b).sum() == 44
    
    print("All basic operation tests passed!")


def test_financial_arrays():
    """Test financial arrays"""
    print("\n=== Testing Financial Arrays ===")
    
    prices = np.array([100, 105, 110])
    returns = FinancialArray.calculate_returns(prices)
    assert len(returns) == 2
    
    volatility = FinancialArray.calculate_volatility(returns)
    assert volatility > 0
    
    print("All financial array tests passed!")


def test_medical_arrays():
    """Test medical arrays"""
    print("\n=== Testing Medical Arrays ===")
    
    weights = np.array([70, 80])
    heights = np.array([1.70, 1.80])
    bmi = MedicalArray.calculate_bmi(weights, heights)
    assert len(bmi) == 2
    
    print("All medical array tests passed!")


def run_all_tests():
    """Run all unit tests"""
    test_array_creation()
    test_array_attributes()
    test_data_types()
    test_basic_operations()
    test_financial_arrays()
    test_medical_arrays()
    print("\n=== All Tests Passed! ===")


if __name__ == "__main__":
    main()
    print("\n" + "="*60)
    run_all_tests()