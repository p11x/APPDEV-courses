# NumPy Arrays and Operations

## Introduction

NumPy, short for Numerical Python, is the foundational library for scientific computing in Python. It provides support for large, multi-dimensional arrays and matrices, along with a collection of high-level mathematical functions to operate on these arrays. NumPy is an essential tool in the data science ecosystem, forming the backbone for libraries like Pandas, SciPy, and scikit-learn. Understanding NumPy arrays and their operations is crucial for anyone working in data science, machine learning, or scientific computing.

The core of NumPy is the ndarray (N-dimensional array), which is far more efficient than Python's built-in list structures for numerical operations. While Python lists store pointers to objects, NumPy arrays store homogeneous data in contiguous memory blocks, enabling vectorized operations that are significantly faster than traditional Python loops. This efficiency makes NumPy indispensable for handling large datasets and performing complex mathematical computations.

NumPy's history dates back to 2005, with Travis Oliphant creating it as an extension of the earlier Numeric library. Since then, it has become the de facto standard for numerical computing in Python, with widespread adoption in academic research, finance, healthcare, and technology industries. The library's design emphasizes performance, flexibility, and ease of use, making it accessible to both beginners and experts.

This module covers the fundamentals of NumPy arrays, including creation, basic operations, data types, and array attributes. You'll learn how to create arrays from various data sources, manipulate array shape and structure, and perform element-wise operations. Each concept is reinforced with practical examples from banking and healthcare domains, demonstrating real-world applications of NumPy.

## Fundamentals

### NumPy Array Creation

Creating NumPy arrays is the first step in leveraging its powerful numerical computing capabilities. NumPy provides multiple methods to create arrays, each suited to different use cases. Understanding these creation methods is fundamental to working effectively with NumPy.

```python
import numpy as np

# Creating arrays from Python lists
# One-dimensional array
prices = np.array([100, 150, 200, 250, 300])
print(f"Stock Prices: {prices}")
print(f"Array shape: {prices.shape}")

# Two-dimensional array - representing a matrix
transaction_matrix = np.array([
    [1000, 2000, 3000],
    [1500, 2500, 3500],
    [2000, 3000, 4000]
])
print(f"\nTransaction Matrix:\n{transaction_matrix}")
print(f"Shape: {transaction_matrix.shape}")

# Creating arrays with specific data types
# Using float64 for financial calculations (default)
account_balances = np.array([5000.00, 7500.50, 10000.25], dtype=np.float64)
print(f"\nAccount Balances: {account_balances}")
print(f"Data type: {account_balances.dtype}")

# Using int32 for integer operations
transaction_counts = np.array([100, 250, 500, 750, 1000], dtype=np.int32)
print(f"\nTransaction Counts: {transaction_counts}")
print(f"Data type: {transaction_counts.dtype}")

# Output:
# Stock Prices: [100 150 200 250 300]
# Array shape: (5,)
# 
# Transaction Matrix:
# [[1000 2000 3000]
#  [1500 2500 3500]
#  [2000 3000 4000]]
# Shape: (3, 3)
# 
# Account Balances: [ 5000.   7500.5 10000.25]
# Data type: float64
# 
# Transaction Counts: [ 100  250  500  750 1000]
# Data type: int32
```

NumPy provides dedicated functions for creating arrays with specific patterns. These functions are optimized for different initialization scenarios and are more efficient than manually populating arrays.

```python
# Array creation functions
# Creating arrays with zeros - useful for initialization
zeros_array = np.zeros((3, 4))
print(f"Zeros Array (3x4):\n{zeros_array}")

# Creating arrays with ones
ones_array = np.ones((2, 3))
print(f"\nOnes Array (2x3):\n{ones_array}")

# Creating arrays with a constant value
full_array = np.full((3, 3), 99.99)
print(f"\nFull Array (constant 99.99):\n{full_array}")

# Creating identity matrix
identity_matrix = np.eye(4)
print(f"\n4x4 Identity Matrix:\n{identity_matrix}")

# Creating arrays with specific ranges
# np.arange - similar to Python's range
linear_sequence = np.arange(0, 100, 10)
print(f"\nArange (0 to 100, step 10): {linear_sequence}")

# np.linspace - evenly spaced numbers
evenly_spaced = np.linspace(0, 1, 5)
print(f"Linspace (0 to 1, 5 points): {evenly_spaced}")

# Creating random arrays
# Random uniform distribution
random_uniform = np.random.rand(3, 3)
print(f"\nRandom Uniform (3x3):\n{random_uniform}")

# Random normal (Gaussian) distribution
random_normal = np.random.randn(4, 4)
print(f"\nRandom Normal (4x4):\n{random_normal}")

# Random integers
random_integers = np.random.randint(1, 100, (3, 3))
print(f"\nRandom Integers 1-100 (3x3):\n{random_integers}")
```

### Array Attributes and Properties

NumPy arrays have numerous attributes that provide information about their structure and content. Understanding these attributes is essential for writing robust NumPy code.

```python
# Exploring array attributes
# Create sample arrays for demonstration
one_d_array = np.array([10, 20, 30, 40, 50])
two_d_array = np.array([
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
])
three_d_array = np.array([
    [[1, 2], [3, 4]],
    [[5, 6], [7, 8]]
])

# Basic attributes
print("One-dimensional array attributes:")
print(f"  Shape: {one_d_array.shape}")
print(f"  Ndim (dimensions): {one_d_array.ndim}")
print(f"  Size (total elements): {one_d_array.size}")
print(f"  Data type: {one_d_array.dtype}")
print(f"  Item size (bytes): {one_d_array.itemsize}")
print(f"  Data buffer (bytes): {one_d_array.nbytes}")

print("\nTwo-dimensional array attributes:")
print(f"  Shape: {two_d_array.shape}")
print(f"  Ndim: {two_d_array.ndim}")
print(f"  Size: {two_d_array.size}")
print(f"  Data type: {two_d_array.dtype}")

print("\nThree-dimensional array attributes:")
print(f"  Shape: {three_d_array.shape}")
print(f"  Ndim: {three_d_array.ndim}")
print(f"  Size: {three_d_array.size}")

# Checking array properties
print("\nArray property checks:")
print(f"  Is contiguous (1D): {one_d_array.flags['C_CONTIGUOUS']}")
print(f"  Is contiguous (2D): {two_d_array.flags['C_CONTIGUOUS']}")
print(f"  Is writeable: {two_d_array.flags['WRITEABLE']}")

# Array flags - detailed information about memory layout
print("\nArray flags for 2D array:")
for flag, value in two_d_array.flags.items():
    print(f"  {flag}: {value}")
```

### Array Indexing and Selection

NumPy provides powerful indexing capabilities that allow you to access and modify array elements efficiently. Understanding these indexing methods is crucial for data manipulation tasks.

```python
# Basic array indexing
# Create sample transaction data
transactions = np.array([
    [100, 200, 300, 400],
    [150, 250, 350, 450],
    [200, 300, 400, 500],
    [250, 350, 450, 550]
])

print("Transaction data:\n", transactions)

# Single element indexing
print(f"\nElement at row 1, col 2: {transactions[1, 2]}")
print(f"Element at row 0, col 0: {transactions[0, 0]}")
print(f"Last element: {transactions[-1, -1]}")

# Row and column selection
print(f"\nFirst row: {transactions[0, :]}")
print(f"Second column: {transactions[:, 1]}")
print(f"First two rows:\n{transactions[:2, :]}")

# Slicing - extracting sub-arrays
print(f"\nSub-matrix (rows 1-2, cols 1-3):\n{transactions[1:3, 1:4]}")

# Boolean indexing - filtering based on conditions
prices = np.array([100, 150, 200, 250, 300, 350, 400])
print(f"\nPrices: {prices}")
expensive = prices[prices > 250]
print(f"Prices over $250: {expensive}")

# Indexing with integer arrays - fancy indexing
selected_indices = np.array([0, 2, 4])
selected_prices = prices[selected_indices]
print(f"\nSelected prices (indices 0,2,4): {selected_prices}")

# Modifying array elements
print("\nModifying array elements:")
print(f"Before: {transactions[0, 0]}")
transactions[0, 0] = 999
print(f"After: {transactions[0, 0]}")
```

### Element-wise Operations

NumPy excels at performing element-wise operations on arrays. These operations are applied to each element individually, which is significantly faster than Python loops for large arrays.

```python
# Element-wise arithmetic operations
prices = np.array([100, 200, 300, 400, 500])

# Addition
print(f"Original prices: {prices}")
print(f"After adding $50: {prices + 50}")

# Subtraction
print(f"After discount ($50): {prices - 50}")

# Multiplication
print(f"Double quantity: {prices * 2}")

# Division
print(f"Half price: {prices / 2}")

# Power operations
print(f"Price squared: {prices ** 2}")

# Modulo operations
print(f"Remainder when divided by 100: {prices % 100}")

# Combined operations
result = (prices * 1.1) - 50  # 10% increase then $50 discount
print(f"After 10% increase and $50 discount: {result}")

# Array-to-array operations
price_a = np.array([100, 200, 300])
price_b = np.array([50, 150, 250])

print(f"\nPrice Array A: {price_a}")
print(f"Price Array B: {price_b}")
print(f"Sum: {price_a + price_b}")
print(f"Difference: {price_a - price_b}")
print(f"Product: {price_a * price_b}")
print(f"Quotient: {price_a / price_b}")

# Comparison operations (element-wise)
print(f"\nA > B: {price_a > price_b}")
print(f"A == B: {price_a == price_b}")
print(f"A >= 150: {price_a >= 150}")
```

## Implementation

### Array Manipulation Functions

NumPy provides numerous functions for manipulating array structure and content. These functions are essential for data preprocessing and reshaping operations.

```python
import numpy as np

# Reshaping arrays
# Create a 1D array
linear_data = np.arange(1, 13)
print(f"Linear data: {linear_data}")

# Reshape to 2D
matrix_3x4 = linear_data.reshape(3, 4)
print(f"\nReshaped to 3x4:\n{matrix_3x4}")

# Reshape to 2D (different orientation)
matrix_4x3 = linear_data.reshape(4, 3)
print(f"\nReshaped to 4x3:\n{matrix_4x3}")

# Reshape to 3D
cube_2x3x2 = linear_data.reshape(2, 3, 2)
print(f"\nReshaped to 2x3x2:\n{cube_2x3x2}")

# Transpose operations
matrix = np.array([[1, 2, 3], [4, 5, 6]])
print(f"\nOriginal matrix:\n{matrix}")
print(f"Transpose:\n{matrix.T}")

# Using swapaxes for specific axis swapping
data = np.arange(24).reshape(2, 3, 4)
print(f"\nOriginal shape: {data.shape}")
swapped = np.swapaxes(data, 1, 2)
print(f"After swapaxes (1,2): {swapped.shape}")

# Flatten and ravel
matrix = np.array([[1, 2, 3], [4, 5, 6]])
flattened = matrix.flatten()
print(f"\nFlattened: {flattened}")

# Resize
arr = np.array([1, 2, 3])
resized = np.resize(arr, 5)
print(f"Resize [1,2,3] to length 5: {resized}")

# Tile and repeat
arr = np.array([1, 2, 3])
tiled = np.tile(arr, 3)
print(f"Tile arr 3 times: {tiled}")
```

### Array Concatenation and Splitting

Combining and splitting arrays are common operations in data processing pipelines. NumPy provides efficient methods for these operations.

```python
# Array concatenation
array_a = np.array([[1, 2], [3, 4]])
array_b = np.array([[5, 6], [7, 8]])

print("Array A:\n", array_a)
print("Array B:\n", array_b)

# Concatenate along axis 0 (vertically/stack rows)
vertical_stack = np.concatenate([array_a, array_b], axis=0)
print(f"\nVertical stack (axis=0):\n{vertical_stack}")

# Concatenate along axis 1 (horizontally/stack columns)
horizontal_stack = np.concatenate([array_a, array_b], axis=1)
print(f"\nHorizontal stack (axis=1):\n{horizontal_stack}")

# Using vstack and hstack
vstack = np.vstack([array_a, array_b])
hstack = np.hstack([array_a, array_b])
print(f"\nvstack:\n{vstack}")
print(f"\nhstack:\n{hstack}")

# Column_stack and row_stack
cstack = np.column_stack([array_a[:, 0], array_b[:, 0]])
rstack = np.row_stack([array_a[0, :], array_b[0, :]])
print(f"\ncolumn_stack first columns:\n{cstack}")
print(f"row_stack first rows:\n{rstack}")

# Array splitting
data = np.arange(1, 13)
print(f"\nData: {data}")

# Split into equal parts
split_3 = np.split(data, 3)
print(f"Split into 3: {split_3}")

# Split at specific positions
split_at = np.array_split(data, [3, 7])
print(f"Split at [3,7]: {split_at}")

# Split matrix
matrix = np.arange(1, 17).reshape(4, 4)
print(f"\nMatrix:\n{matrix}")

# Horizontal split
h_split = np.hsplit(matrix, 2)
print(f"\nHSplit into 2:")
for i, h in enumerate(h_split):
    print(f"  Part {i+1}:\n{h}")

# Vertical split
v_split = np.vsplit(matrix, 2)
print(f"\nVSplit into 2:")
for i, v in enumerate(v_split):
    print(f"  Part {i+1}:\n{v}")
```

### Array Copying and Views

Understanding the difference between copies and views is crucial to avoid unintended modifications to data. NumPy provides mechanisms for both approaches.

```python
# Creating array copies and views
original = np.array([1, 2, 3, 4, 5])
print(f"Original: {original}")

# View - shares memory with original
view = original.view()
print(f"\nView: {view}")
print(f"Same base? {view.base is original}")

# Modifying view affects original
view[0] = 999
print(f"After modifying view:")
print(f"  Original: {original}")
print(f"  View: {view}")

# Copy - creates independent array
original = np.array([1, 2, 3, 4, 5])
copy = original.copy()
print(f"\nCopy: {copy}")
print(f"Same base? {copy.base is original}")

# Modifying copy doesn't affect original
copy[0] = 888
print(f"After modifying copy:")
print(f"  Original: {original}")
print(f"  Copy: {copy}")

# Deep copy with np.copy()
data = np.array([[1, 2, 3], [4, 5, 6]])
deep_copy = np.copy(data)
print(f"\nDeep copy:\n{deep_copy}")

# Copy with specific dtype
int_array = np.array([1, 2, 3], dtype=np.int32)
float_copy = int_array.astype(np.float64)
print(f"\nInt array: {int_array}, dtype: {int_array.dtype}")
print(f"Float copy: {float_copy}, dtype: {float_copy.dtype}")

# Assignment vs copy
arr = np.array([1, 2, 3])
assigned = arr  # Same object, not a copy
assigned[0] = 99
print(f"\nAssigned (same object): arr={arr}, assigned={assigned}")
```

## Applications

### Banking Application: Transaction Analysis

NumPy's array operations are extensively used in banking for transaction processing, risk analysis, and portfolio management. This example demonstrates how NumPy arrays can be used to analyze banking transactions.

```python
import numpy as np

# Simulating banking transaction data
# Transaction amounts over 30 days for 5 different accounts
np.random.seed(42)

# Generating synthetic transaction data
transaction_counts = 5
days = 30

# Generate random transaction amounts (simulating daily transactions)
daily_transactions = np.random.randint(0, 100, (transaction_counts, days))
print("Daily Transaction Counts (5 accounts, 30 days):")
print(daily_transactions)

# Calculate total transactions per account
total_per_account = np.sum(daily_transactions, axis=1)
print(f"\nTotal transactions per account: {total_per_account}")

# Average transactions per day for each account
avg_per_day = np.mean(daily_transactions, axis=1)
print(f"Average transactions per day: {avg_per_day}")

# Maximum and minimum daily transactions
max_daily = np.max(daily_transactions, axis=1)
min_daily = np.min(daily_transactions, axis=1)
print(f"Max daily transactions: {max_daily}")
print(f"Min daily transactions: {min_daily}")

# Account IDs and balances
account_ids = np.array([1001, 1002, 1003, 1004, 1005])
account_balances = np.array([50000, 75000, 25000, 100000, 5000])

# Transaction amounts in dollars (converting count to amount assuming average $100 per transaction)
avg_transaction_amount = 100
total_transaction_values = total_per_account * avg_transaction_amount

print(f"\n{'Account':<10} {'Balance':<12} {'Transactions':<12} {'Total Value':<12}")
print("-" * 50)
for i in range(len(account_ids)):
    print(f"{account_ids[i]:<10} ${account_balances[i]:<10,.0f} {total_per_account[i]:<10} ${total_transaction_values[i]:<10,.0f}")

# Calculate transaction frequency by time period
# Group days into weeks (7-day periods)
weekly_totals = daily_transactions.reshape(transaction_counts, 4, 7)
weekly_sum = np.sum(weekly_totals, axis=2)
print(f"\nWeekly totals per account:\n{weekly_sum}")

# Identify high-activity accounts (above average)
avg_transactions = np.mean(total_per_account)
high_activity = total_per_account > avg_transactions
print(f"\nAccounts with above-average activity: {account_ids[high_activity]}")
```

```python
# Portfolio analysis using NumPy arrays
# Portfolio holdings
holdings = np.array([
    [500, 150.25],    # Stock A: 500 shares at $150.25
    [1000, 75.50],   # Stock B: 1000 shares at $75.50
    [300, 200.00],   # Stock C: 300 shares at $200.00
    [200, 450.75],   # Stock D: 200 shares at $450.75
    [150, 100.00]    # Stock E: 150 shares at $100.00
])

shares = holdings[:, 0]
prices = holdings[:, 1]
total_value = shares * prices

stock_names = np.array(['Stock A', 'Stock B', 'Stock C', 'Stock D', 'Stock E'])

print("Portfolio Holdings:")
print(f"{'Stock':<10} {'Shares':<10} {'Price':<12} {'Value':<15}")
print("-" * 50)
for i in range(len(stock_names)):
    print(f"{stock_names[i]:<10} {shares[i]:<10} ${prices[i]:<10.2f} ${total_value[i]:<14,.2f}")

total_portfolio_value = np.sum(total_value)
print(f"\nTotal Portfolio Value: ${total_portfolio_value:,.2f}")

# Portfolio weights
portfolio_weights = total_value / total_portfolio_value
print(f"\nPortfolio Weights:")
for i in range(len(stock_names)):
    print(f"  {stock_names[i]}: {portfolio_weights[i]*100:.2f}%")

# Risk analysis - calculate daily returns simulation
np.random.seed(42)
daily_returns = np.random.normal(0.001, 0.02, (30, 5))
print(f"\nSimulated Daily Returns (30 days, 5 stocks):")
print(f"Mean daily returns: {np.mean(daily_returns, axis=0)}")
print(f"Std deviation: {np.std(daily_returns, axis=0)}")

# Value at Risk (VaR) calculation
portfolio_returns = np.sum(daily_returns * portfolio_weights, axis=1)
var_95 = np.percentile(portfolio_returns, 5)
var_99 = np.percentile(portfolio_returns, 1)
print(f"\nValue at Risk:")
print(f"  95% VaR: {var_95*100:.2f}%")
print(f"  99% VaR: {var_99*100:.2f}%")
```

### Healthcare Application: Patient Data Analysis

NumPy arrays are equally valuable in healthcare for patient data analysis, medical imaging processing, and clinical research.

```python
import numpy as np
import datetime

# Simulating patient vital signs data
# Creating time series data for vital signs over 7 days
np.random.seed(42)

# Generate synthetic patient data
num_patients = 10
num_observations = 7  # 7 days of observations

# Heart rate observations (beats per minute)
heart_rates = np.random.normal(75, 10, (num_patients, num_observations)).astype(int)
heart_rates = np.clip(heart_rates, 50, 120)  # Normal physiological range

# Blood pressure systolic
bp_systolic = np.random.normal(120, 15, (num_patients, num_observations)).astype(int)
bp_systolic = np.clip(bp_systolic, 90, 180)

# Blood pressure diastolic
bp_diastolic = np.random.normal(80, 10, (num_patients, num_observations)).astype(int)
bp_diastolic = np.clip(bp_diastolic, 60, 100)

# Temperature (Fahrenheit)
temperature = np.random.normal(98.2, 0.5, (num_patients, num_observations))
temperature = np.round(temperature, 1)

print("Patient Vital Signs Database")
print("=" * 70)

for patient_id in range(num_patients):
    print(f"\nPatient ID: {1000 + patient_id}")
    print(f"Heart Rate (7 days): {heart_rates[patient_id]}")
    print(f"BP Systolic: {bp_systolic[patient_id]}")
    print(f"BP Diastolic: {bp_diastolic[patient_id]}")
    print(f"Temperature: {temperature[patient_id]}")
    
    # Calculate averages
    avg_hr = np.mean(heart_rates[patient_id])
    avg_bp_sys = np.mean(bp_systolic[patient_id])
    avg_bp_dia = np.mean(bp_diastolic[patient_id])
    avg_temp = np.mean(temperature[patient_id])
    
    print(f"Averages - HR: {avg_hr:.1f}, BP: {avg_bp_sys:.0f}/{avg_bp_dia:.0f}, Temp: {avg_temp:.1f}F")

# Aggregate statistics across all patients
print("\n" + "=" * 70)
print("Aggregate Statistics:")
print(f"Overall Mean Heart Rate: {np.mean(heart_rates):.1f} bpm")
print(f"Overall Mean BP: {np.mean(bp_systolic):.0f}/{np.mean(bp_diastolic):.0f} mmHg")
print(f"Overall Mean Temperature: {np.mean(temperature):.1f}F")

# Identify abnormal readings
abnormal_hr = (heart_rates < 60) | (heart_rates > 100)
abnormal_bp = (bp_systolic > 140) | (bp_diastolic > 90)
abnormal_temp = temperature > 99.5

print(f"\nAbnormal Readings:")
print(f"Heart Rate alerts: {np.sum(abnormal_hr)} readings")
print(f"Blood Pressure alerts: {np.sum(abnormal_bp)} readings")
print(f"Fever alerts: {np.sum(abnormal_temp)} readings")
```

```python
# Medical imaging processing example with NumPy
# Simulating a 2D medical image (e.g., MRI slice)
np.random.seed(42)

# Create a simulated medical image (256x256 pixels)
# Simulating grayscale values (0-255)
image = np.random.randint(0, 256, (256, 256), dtype=np.uint8)

print("Simulated Medical Image Analysis")
print("=" * 50)
print(f"Image shape: {image.shape}")
print(f"Data type: {image.dtype}")
print(f"Min value: {np.min(image)}")
print(f"Max value: {np.max(image)}")
print(f"Mean intensity: {np.mean(image):.2f}")

# Image statistics by region
# Divide image into quadrants
quadrant_1 = image[:128, :128]
quadrant_2 = image[:128, 128:]
quadrant_3 = image[128:, :128]
quadrant_4 = image[128:, 128:]

print(f"\nQuadrant Statistics:")
print(f"Q1 (top-left) mean: {np.mean(quadrant_1):.2f}")
print(f"Q2 (top-right) mean: {np.mean(quadrant_2):.2f}")
print(f"Q3 (bottom-left) mean: {np.mean(quadrant_3):.2f}")
print(f"Q4 (bottom-right) mean: {np.mean(quadrant_4):.2f}")

# Create a binary mask (simulating tissue segmentation)
# Threshold at mean value
threshold = np.mean(image)
binary_mask = image > threshold
print(f"\nBinary segmentation (threshold={threshold:.0f}):")
print(f"Tissue pixels: {np.sum(binary_mask)}")
print(f"Background pixels: {np.sum(~binary_mask)}")

# Histogram computation
histogram, bins = np.histogram(image, bins=256, range=(0, 256))
print(f"\nHistogram computed: {len(histogram)} bins")

# Find top intensity values
top_indices = np.argsort(histogram)[-5:]
print(f"Top 5 intensity values: {top_indices}")
print(f"With counts: {histogram[top_indices]}")
```

## Output Results

### Formatted Output with NumPy

NumPy provides various ways to format and display array data. Understanding these capabilities is essential for creating readable output and reports.

```python
import numpy as np

# Creating formatted reports with NumPy arrays
# Customer account data
accounts = np.array([
    [1001, 50000, 3.5, 750],
    [1002, 75000, 4.2, 1200],
    [1003, 25000, 2.8, 400],
    [1004, 100000, 5.0, 2500],
    [1005, 5000, 1.5, 100]
])

account_id = accounts[:, 0]
balance = accounts[:, 1]
interest_rate = accounts[:, 2]
transactions = accounts[:, 3]

# Calculate interest earned
interest_earned = balance * interest_rate / 100

print("=" * 70)
print("BANKING CUSTOMER REPORT")
print("=" * 70)
print(f"{'Account ID':<12} {'Balance':<15} {'Rate (%)':<10} {'Interest':<12} {'Txns':<8}")
print("-" * 70)

for i in range(len(accounts)):
    print(f"{account_id[i]:<12} ${balance[i]:<14,.2f} {interest_rate[i]:<9.1f} ${interest_earned[i]:<11,.2f} {transactions[i]:<8}")

print("-" * 70)
total_balance = np.sum(balance)
total_interest = np.sum(interest_earned)
total_transactions = np.sum(transactions)
print(f"{'TOTAL':<12} ${total_balance:<14,.2f} {'':<9} ${total_interest:<11,.2f} {total_transactions:<8}")
print("=" * 70)
```

```python
# Statistical output formatting
# Performance metrics
data = np.array([
    [100, 92, 85],
    [150, 88, 90],
    [200, 95, 88],
    [250, 91, 92],
    [300, 87, 85]
])

metrics = ['Accuracy', 'Precision', 'Recall']
models = ['Model A', 'Model B', 'Model C']

print("MODEL PERFORMANCE METRICS")
print("=" * 50)

# Calculate statistics
for i, model in enumerate(models):
    col = data[:, i]
    print(f"\n{model}:")
    print(f"  Mean: {np.mean(col):.2f}%")
    print(f"  Std Dev: {np.std(col):.2f}%")
    print(f"  Min: {np.min(col):.2f}%")
    print(f"  Max: {np.max(col):.2f}%")
    print(f"  Range: {np.max(col) - np.min(col):.2f}%")

# Percentile analysis
print("\nPERCENTILE ANALYSIS")
print("=" * 40)
for i, model in enumerate(models):
    col = data[:, i]
    p25 = np.percentile(col, 25)
    p50 = np.percentile(col, 50)
    p75 = np.percentile(col, 75)
    print(f"{model}: P25={p25:.1f}, Median={p50:.1f}, P75={p75:.1f}")
```

### Array String Representation

NumPy arrays can be converted to strings for various output purposes, including saving to files or displaying in GUIs.

```python
# Array to string conversion
arr = np.array([1, 2, 3, 4, 5])

# Direct string conversion
arr_string = str(arr)
print(f"String: {arr_string}")

# Using tolist()
arr_list = arr.tolist()
print(f"List: {arr_list}")

# 2D array string
matrix = np.array([[1, 2, 3], [4, 5, 6]])
print(f"Matrix:\n{matrix}")

# CSV-style output
def array_to_csv(arr):
    """Convert 2D array to CSV string"""
    lines = [','.join(map(str, row)) for row in arr]
    return '\n'.join(lines)

data = np.array([[1, 2, 3], [4, 5, 6], [7, 8, 9]])
print(f"\nCSV:\n{array_to_csv(data)}")

# Pretty print with np.array_str
arr = np.array([1.23456789, 2.3456789, 3.456789])
np.set_printoptions(precision=2)
print(f"\nFormatted array: {arr}")
np.set_printoptions(precision=8)  # Reset to default
```

## Visualization

### ASCII Visualizations with NumPy

Creating ASCII-based visualizations is useful for quick data exploration and terminal-based outputs.

```python
import numpy as np

# ASCII bar chart from NumPy data
def ascii_bar_chart(data_dict, max_width=50, bar_char='█'):
    """Create ASCII bar chart visualization"""
    print("\nASCII Bar Chart")
    print("=" * 60)
    
    max_value = max(data_dict.values())
    
    for label, value in data_dict.items():
        scaled_width = int((value / max_value) * max_width)
        bar = bar_char * scaled_width
        print(f"{label:<15} |{bar} {value}")
    
    print("=" * 60)

# Banking example - Transaction volumes
transactions = {
    "January": 45000,
    "February": 52000,
    "March": 48000,
    "April": 61000,
    "May": 55000,
    "June": 67000
}
ascii_bar_chart(transactions)

# Healthcare example - Patient admissions
admissions = {
    "Week 1": 156,
    "Week 2": 189,
    "Week 3": 201,
    "Week 4": 178
}
ascii_bar_chart(admissions, max_width=40, bar_char='▓')
```

```python
# ASCII histogram with NumPy
def ascii_histogram(values, bins=10, width=50):
    """Create ASCII histogram from numerical data"""
    hist, bin_edges = np.histogram(values, bins=bins)
    
    max_count = np.max(hist)
    
    print("\nASCII Histogram")
    print("-" * (width + 20))
    
    for i in range(len(hist)):
        bar_length = int((hist[i] / max_count) * width)
        bar = '█' * bar_length
        print(f"{bin_edges[i]:6.1f}-{bin_edges[i+1]:6.1f}: {bar} ({hist[i]})")
    
    print("-" * (width + 20))

# Transaction amounts distribution
transaction_amounts = np.random.exponential(500, 1000)
ascii_histogram(transaction_amounts, bins=15)

# Heart rate distribution
heart_rates = np.random.normal(75, 12, 500).astype(int)
heart_rates = np.clip(heart_rates, 40, 120)
print("\nHeart Rate Distribution:")
ascii_histogram(heart_rates, bins=10)
```

```python
# ASCII heatmap
def ascii_heatmap(matrix, labels=None, char_matrix=' ░▒▓█'):
    """Create ASCII heatmap visualization"""
    print("\nASCII Heatmap")
    
    min_val = np.min(matrix)
    max_val = np.max(matrix)
    num_chars = len(char_matrix) - 1
    
    for i in range(matrix.shape[0]):
        if labels is not None:
            print(f"{labels[i]:<10} ", end="")
        
        for val in matrix[i]:
            normalized = (val - min_val) / (max_val - min_val)
            char_index = int(normalized * num_chars)
            print(char_matrix[char_index], end="")
        print()
    
    print(f"\nLegend: {char_matrix[0]} (low) -> {char_matrix[-1]} (high)")

# Example heatmap - Account balances over time
data = np.array([
    [100, 200, 300, 400],
    [150, 250, 350, 450],
    [200, 300, 400, 500],
    [250, 350, 450, 550]
])
ascii_heatmap(data, labels=['Q1', 'Q2', 'Q3', 'Q4'])
```

### ASCII Matrix Visualization

Displaying arrays as visual matrices helps understand data patterns and distributions.

```python
# ASCII matrix display
def print_ascii_matrix(arr, format_str="{:.1f}"):
    """Print array as formatted ASCII table"""
    print("\nData Matrix:")
    print("-" * (arr.shape[1] * 10))
    
    for row in arr:
        row_str = " ".join(format_str.format(x).center(9) for x in row)
        print(f"| {row_str} |")
    
    print("-" * (arr.shape[1] * 10))

# Financial data matrix
financial_data = np.array([
    [1000.5, 2500.3, 4000.7],
    [1500.2, 3000.5, 4500.8],
    [2000.8, 3500.1, 5000.3]
])
print_ascii_matrix(financial_data)

# Patient vitals matrix
vitals = np.array([
    [72, 120, 80, 98.6],
    [75, 118, 78, 98.4],
    [70, 122, 82, 98.8]
])
print_ascii_matrix(vitals, format_str="{:.1f}")
```

## Advanced Topics

### Memory Layout and Performance

Understanding NumPy's memory layout is crucial for writing high-performance code. NumPy arrays can be stored in different memory layouts that affect performance.

```python
import numpy as np

# Understanding array memory layouts
# C-contiguous (row-major) - default
c_array = np.array([[1, 2, 3], [4, 5, 6]], order='C')
print(f"C-contiguous: {c_array.flags['C_CONTIGUOUS']}")

# Fortran-contiguous (column-major)
f_array = np.array([[1, 2, 3], [4, 5, 6]], order='F')
print(f"Fortran-contiguous: {f_array.flags['F_CONTIGUOUS']}")

# Checking memory strided
print(f"\nC-array strides: {c_array.strides}")
print(f"F-array strides: {f_array.strides}")

# View creation and memory efficiency
base = np.arange(10)
print(f"\nBase array: {base}")
view = base[::2]
print(f"Strided view (every 2nd): {view}")
print(f"View shares memory: {view.base is base}")

# Efficient array operations
# Using np.add for in-place operations
arr = np.zeros(1000000)
np.add(arr, 1, out=arr)  # In-place, more efficient
print(f"\nSum after in-place add: {np.sum(arr)}")

# Vectorized operations performance
# Broadcasting reduces explicit loops
a = np.random.rand(1000, 1000)
b = np.random.rand(1000)

# Explicit loop (slow)
result_loop = np.zeros(1000)
for i in range(1000):
    result_loop[i] = np.sum(a[i, :] * b)

# Broadcasting (fast)
result_broadcast = np.dot(a, b)

print(f"\nLoop result sum: {np.sum(result_loop):.2f}")
print(f"Broadcast result sum: {np.sum(result_broadcast):.2f}")
```

### Structured Arrays

Structured arrays allow storing heterogeneous data in a single array, useful for record-like data.

```python
# Structured arrays for complex records
# Define structured data type
dt = np.dtype([
    ('account_id', 'i4'),
    ('balance', 'f8'),
    ('status', 'U10'),
    ('transactions', 'i4')
])

# Create structured array
accounts = np.array([
    (1001, 50000.0, 'Active', 750),
    (1002, 75000.0, 'Active', 1200),
    (1003, 25000.0, 'Inactive', 400),
    (1004, 100000.0, 'Premium', 2500),
    (1005, 5000.0, 'Active', 100)
], dtype=dt)

print("Structured Account Array:")
print(accounts)

# Accessing fields
print(f"\nAccount IDs: {accounts['account_id']}")
print(f"Balances: {accounts['balance']}")
print(f"Statuses: {accounts['status']}")

# Filtering by field
active = accounts[accounts['status'] == 'Active']
print(f"\nActive accounts:\n{active}")

# Sorting by field
by_balance = np.sort(accounts, order='balance')
print(f"\nSorted by balance:\n{by_balance}")
```

### Saving and Loading Arrays

NumPy provides efficient binary and text formats for saving and loading arrays.

```python
# Saving and loading arrays
import tempfile
import os

# Create sample data
arr = np.random.rand(100, 100)
matrix = np.random.randint(0, 100, (10, 10))

# Save to binary format (.npy)
with tempfile.NamedTemporaryFile(suffix='.npy', delete=False) as f:
    np.save(f.name, arr)
    temp_file = f.name

loaded = np.load(temp_file)
print(f"Binary save/load: {np.array_equal(arr, loaded)}")
os.unlink(temp_file)

# Save multiple arrays (.npz)
with tempfile.NamedTemporaryFile(suffix='.npz', delete=False) as f:
    np.savez(f.name, arr=arr, matrix=matrix)
    temp_file = f.name

loaded = np.load(temp_file)
print(f"Keys in npz: {list(loaded.keys())}")
print(f"Array shape: {loaded['arr'].shape}")
print(f"Matrix shape: {loaded['matrix'].shape}")
os.unlink(temp_file)

# Save to text format
with tempfile.NamedTemporaryFile(suffix='.txt', delete=False) as f:
    np.savetxt(f.name, arr[:5, :5], delimiter=',')
    temp_file = f.name

loaded_txt = np.loadtxt(temp_file, delimiter=',')
print(f"\nText save/load: {np.array_equal(arr[:5, :5], loaded_txt)}")
os.unlink(temp_file)
```

## Conclusion

NumPy arrays form the foundation of numerical computing in Python. Through this comprehensive exploration, you've learned how to create arrays, manipulate their structure, perform element-wise operations, and apply them to real-world banking and healthcare scenarios. These skills are essential for any data science workflow, providing the efficiency and flexibility needed to handle large datasets.

The banking examples demonstrated how NumPy can be used for transaction analysis, portfolio management, and risk calculation. The healthcare applications showed patient vital signs analysis, medical imaging processing, and statistical computations. Both domains benefit from NumPy's vectorized operations which eliminate the need for slow Python loops.

Continue practicing with real datasets to strengthen your understanding. Explore additional NumPy functions like linear algebra operations, Fourier transforms, and random number generation. Mastery of these array operations will serve as a strong foundation for learning Pandas, SciPy, and machine learning libraries.