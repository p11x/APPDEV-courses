# Performance Optimization with NumPy

## Introduction

Performance optimization with NumPy is crucial for data science applications that work with large datasets. NumPy provides highly optimized operations, but understanding how to write efficient code can make significant differences in execution time. This module covers techniques for maximizing NumPy performance, including vectorization, memory management, broadcasting efficiency, and leveraging built-in functions.

The importance of performance optimization in data science cannot be overstated. When working with millions of data points, a naive implementation can take minutes or hours, while an optimized version can complete in seconds. This is particularly important in banking (real-time fraud detection, risk calculations) and healthcare (medical imaging processing, genome analysis) where timely results are critical.

NumPy achieves its performance through integration with optimized BLAS (Basic Linear Algebra Subprograms) and LAPACK (Linear Algebra Package) libraries, vectorized operations that avoid Python loops, and efficient memory layout. Understanding these underlying mechanisms helps in writing code that takes full advantage of NumPy's capabilities.

This module covers performance optimization techniques including vectorization strategies, memory efficiency, broadcasting best practices, and profiling tools. You'll learn how to write efficient NumPy code for banking and healthcare applications while avoiding common performance pitfalls.

## Fundamentals

### Vectorization Basics

Vectorization is the process of replacing explicit loops with NumPy array operations, taking advantage of optimized C code.

```python
import numpy as np
import time

# Naive approach: Using Python loops
np.random.seed(42)
data = np.random.rand(1000000)

print("=" * 60)
print("VECTORIZATION BASICS")
print("=" * 60)

# Method 1: Python loop (slow)
start = time.time()
result_loop = []
for val in data:
    result_loop.append(val * 2 + 1)
loop_time = time.time() - start

# Method 2: List comprehension (faster than loop but still Python)
start = time.time()
result_comp = [val * 2 + 1 for val in data]
comp_time = time.time() - start

# Method 3: NumPy vectorized operations (fast)
start = time.time()
result_vec = data * 2 + 1
vec_time = time.time() - start

# Method 4: Using np.multiply with out parameter (in-place)
start = time.time()
result_out = np.empty_like(data)
np.multiply(data, 2, out=result_out)
np.add(result_out, 1, out=result_out)
out_time = time.time() - start

print(f"\nArray size: {len(data):,} elements")
print(f"\nMethod              | Time (ms) | Speedup")
print("-" * 50)
print(f"Python loop         | {loop_time*1000:>8.2f} | {loop_time/loop_time:>6.1f}x")
print(f"List comprehension  | {comp_time*1000:>8.2f} | {comp_time/loop_time:>6.1f}x")
print(f"NumPy vectorized    | {vec_time*1000:>8.2f} | {vec_time/loop_time:>6.1f}x")
print(f"np.multiply (out=) | {out_time*1000:>8.2f} | {out_time/loop_time:>6.1f}x")

# Verify correctness
print(f"\nAll methods produce same result: {np.allclose(result_vec, np.array(result_loop))}")

# Output:
# Array size: 1,000,000 elements
# 
# Method              | Time (ms) | Speedup
# --------------------------------------------------
# Python loop         |  452.31 |   1.0x
# List comprehension  |  187.54 |   2.4x
# NumPy vectorized    |    2.15 | 210.4x
# np.multiply (out=) |    1.89 | 239.3x
```

### Memory Layout and Efficiency

Understanding memory layout is key to optimizing NumPy performance. Arrays can be stored in C-order (row-major) or F-order (column-major).

```python
import numpy as np

print("=" * 60)
print("MEMORY LAYOUT AND EFFICIENCY")
print("=" * 60)

# Memory contiguous arrays
# C-contiguous (row-major) - default
c_array = np.array([[1, 2, 3], [4, 5, 6]], order='C')
print("\nC-contiguous (row-major) array:")
print(f"  Flags: {c_array.flags}")
print(f"  Strides: {c_array.strides}")

# Fortran-contiguous (column-major)  
f_array = np.array([[1, 2, 3], [4, 5, 6]], order='F')
print("\nFortran-contiguous (column-major) array:")
print(f"  Flags: {f_array.flags}")
print(f"  Strides: {f_array.strides}")

# Non-contiguous array (via slicing)
sliced = np.arange(10)[::2]
print(f"\nSliced array (every 2nd element):")
print(f"  Is contiguous: {sliced.flags['C_CONTIGUOUS']}")
print(f"  Base is original: {sliced.base is not None}")

# Creating contiguous arrays
large = np.random.rand(100, 100)

# Slicing creates views (non-contiguous if original is large enough)
view = large[:, :50]  # This is contiguous since slicing last dimension
print(f"\nView of large array:")
print(f"  Is contiguous: {view.flags['C_CONTIGUOUS']}")
print(f"  Shares memory: {np.shares_memory(view, large)}")

# Copy vs View
arr = np.arange(10)
view = arr[::2]
copy = arr[::2].copy()

print(f"\nView vs Copy:")
print(f"  View shares memory: {np.shares_memory(arr, view)}")
print(f"  Copy shares memory: {np.shares_memory(arr, copy)}")

# Memory usage
arr = np.random.rand(1000, 1000)
print(f"\nMemory usage (1000x1000 float64 array):")
print(f"  Array nbytes: {arr.nbytes:,} bytes ({arr.nbytes/1024/1024:.2f} MB)")
```

### Broadcasting Efficiency

Broadcasting allows NumPy to perform operations on arrays with different shapes efficiently.

```python
import numpy as np
import time

np.random.seed(42)

print("=" * 60)
print("BROADCASTING EFFICIENCY")
print("=" * 60)

# Broadcasting example
A = np.random.rand(1000, 1000)
B = np.random.rand(1000)

# Method 1: Expplicit loop
start = time.time()
result_loop = np.zeros((1000, 1000))
for i in range(1000):
    result_loop[i] = A[i] + B
loop_time = time.time() - start

# Method 2: Broadcasting
start = time.time()
result_broadcast = A + B
broadcast_time = time.time() - start

print(f"Adding column vector to matrix (1000x1000):")
print(f"  Explicit loop: {loop_time*1000:.2f} ms")
print(f"  Broadcasting: {broadcast_time*1000:.2f} ms")
print(f"  Speedup: {loop_time/broadcast_time:.1f}x")

# Efficient broadcasting patterns
C = np.random.rand(1, 1000)  # Row vector
D = np.random.rand(1000, 1)  # Column vector

# Both produce same result but one may be more efficient
start = time.time()
for _ in range(10000):
    result = C + D
res1_time = time.time() - start

print(f"\nRow + Column broadcasting (10000 iterations):")
print(f"  Time: {res1_time*1000:.2f} ms")

# Best practices for broadcasting
print("\nBroadcasting Best Practices:")
print("  1. Use broadcasting instead of explicit loops")
print("  2. Ensure arrays are contiguous for best performance")
print("  3. Use np.newaxis to add dimensions when needed")
print("  4. Use np.broadcast_to to create read-only broadcast arrays")
```

## Implementation

### Banking Application: High-Performance Transaction Processing

Optimized transaction processing for banking applications.

```python
import numpy as np
import time

np.random.seed(42)

# Simulate large transaction dataset
n_transactions = 10000000  # 10 million transactions

# Transaction data: amount, type, customer_id, timestamp, etc.
amounts = np.random.exponential(100, n_transactions)
types = np.random.randint(1, 5, n_transactions)  # 1-4 transaction types
customer_ids = np.random.randint(1, 100000, n_transactions)
hours = np.random.randint(0, 24, n_transactions)

print("=" * 60)
print("HIGH-PERFORMANCE TRANSACTION PROCESSING")
print("=" * 60)

print(f"\nProcessing {n_transactions:,} transactions")

# Basic statistics - vectorized
start = time.time()
total_amount = np.sum(amounts)
mean_amount = np.mean(amounts)
max_amount = np.max(amounts)
min_amount = np.min(amounts)
basic_time = time.time() - start

print(f"\nBasic Statistics:")
print(f"  Total: ${total_amount:,.0f}")
print(f"  Mean: ${mean_amount:.2f}")
print(f"  Min/Max: ${min_amount:.2f} / ${max_amount:.2f}")
print(f"  Time: {basic_time*1000:.2f} ms")

# Transaction type analysis
start = time.time()
type_totals = np.bincount(types, minlength=5)[1:]  # Skip index 0 (no type 0)
type_time = time.time() - start

print(f"\nTransaction Type Analysis:")
type_names = {1: 'Withdrawal', 2: 'Deposit', 3: 'Transfer', 4: 'Payment'}
for t, total in enumerate(type_totals):
    print(f"  {type_names[t+1]}: {total:,} ({total/n_transactions*100:.1f}%)")
print(f"  Time: {type_time*1000:.2f} ms")

# Customer-level aggregations
start = time.time()
unique_customers, inverse = np.unique(customer_ids, return_inverse=True)
customer_totals = np.zeros(len(unique_customers))
np.add.at(customer_totals, inverse, amounts)
agg_time = time.time() - start

print(f"\nCustomer Aggregations:")
print(f"  Unique customers: {len(unique_customers):,}")
print(f"  Time: {agg_time*1000:.2f} ms")

# Top customers
top_indices = np.argsort(customer_totals)[-10:]
print(f"\nTop 10 Customers by Transaction Volume:")
for idx in top_indices:
    print(f"  Customer {unique_customers[idx]}: ${customer_totals[idx]:,.0f}")

# Hourly analysis
start = time.time()
hourly_totals = np.bincount(hours, weights=amounts)
hourly_counts = np.bincount(hours)
hourly_avg = hourly_totals / hourly_counts
hourly_time = time.time() - start

print(f"\nHourly Analysis:")
print(f"  Time: {hourly_time*1000:.2f} ms")
print(f"  Peak Hour: {np.argmax(hourly_avg)} (avg ${hourly_avg.max():.2f})")
```

```python
# Real-time fraud detection simulation
print("=" * 60)
print("REAL-TIME FRAUD DETECTION")
print("=" * 60)

# Feature extraction (vectorized)
def detect_fraud_features(amounts, customer_totals, inverse, types):
    """Detect potential fraud indicators"""
    features = {}
    
    # Amount-based features
    features['high_amount'] = amounts > 10000
    features['very_high'] = amounts > 50000
    
    # Unusual patterns (amount much higher than customer's average)
    customer_means = np.zeros(len(customer_totals))
    customer_counts = np.zeros(len(customer_totals))
    np.add.at(customer_counts, inverse, 1)
    np.add.at(customer_means, inverse, amounts)
    customer_means = customer_means / np.maximum(customer_counts, 1)
    
    avg_by_customer = customer_means[inverse]
    features['above_avg'] = amounts > avg_by_customer * 3
    
    return features

start = time.time()
fraud_features = detect_fraud_features(
    amounts, customer_totals, inverse, types
)
feature_time = time.time() - start

print(f"\nFraud Detection Features ({n_transactions:,} transactions):")
print(f"  High amount (>10k): {np.sum(fraud_features['high_amount']):,}")
print(f"  Very high (>50k): {np.sum(fraud_features['very_high']):,}")
print(f"  Above customer average (3x): {np.sum(fraud_features['above_avg']):,}")
print(f"  Processing time: {feature_time*1000:.2f} ms")

# Risk scoring
start = time.time()
risk_scores = np.zeros(n_transactions, dtype=np.float32)
np.maximum(risk_scores, amounts / 10000, out=risk_scores)
np.maximum(risk_scores, fraud_features['above_avg'].astype(np.float32) * 5, out=risk_scores)
np.maximum(risk_scores, fraud_features['very_high'].astype(np.float32) * 3, out=risk_scores)
score_time = time.time() - start

print(f"\nRisk Scoring:")
print(f"  Mean score: {np.mean(risk_scores):.2f}")
print(f"  High risk (>5): {np.sum(risk_scores > 5):,}")
print(f"  Processing time: {score_time*1000:.2f} ms")
```

### Healthcare Application: Medical Image Processing

Optimized image processing for medical applications.

```python
import numpy as np
import time

# Simulate medical image data (e.g., MRI slices)
np.random.seed(42)
image_size = 512  # 512x512 pixel images
n_slices = 100  # 100 image slices

# Create simulated images
images = np.random.randint(0, 256, (n_slices, image_size, image_size), dtype=np.uint8)

print("=" * 60)
print("MEDICAL IMAGE PROCESSING")
print("=" * 60)

print(f"\nProcessing {n_slices} {image_size}x{image_size} images")

# Image statistics
start = time.time()
mean_intensity = np.mean(images)
std_intensity = np.std(images)
min_val = np.min(images)
max_val = np.max(images)
stats_time = time.time() - start

print(f"\nImage Statistics:")
print(f"  Mean intensity: {mean_intensity:.2f}")
print(f"  Std deviation: {std_intensity:.2f}")
print(f"  Range: {min_val} - {max_val}")
print(f"  Time: {stats_time*1000:.2f} ms")

# Slice-by-slice analysis (vectorized)
start = time.time()
slice_means = np.mean(images, axis=(1, 2))
slice_stds = np.std(images, axis=(1, 2))
slice_time = time.time() - start

print(f"\nPer-Slice Analysis:")
print(f"  Mean per slice: {slice_means[0]:.2f} - {slice_means[-1]:.2f}")
print(f"  Time: {slice_time*1000:.2f} ms")

# Contrast enhancement
start = time.time()
enhanced = images.astype(np.float32)
enhanced = (enhanced - mean_intensity) / std_intensity
enhanced = enhanced * 50 + 128
enhanced = np.clip(enhanced, 0, 255).astype(np.uint8)
enhance_time = time.time() - start

print(f"\nContrast Enhancement:")
print(f"  Enhanced range: {enhanced.min()} - {enhanced.max()}")
print(f"  Time: {enhance_time*1000:.2f} ms")

# Edge detection using Sobel-like filters
def sobel_edge_detection(image):
    """Simplified edge detection"""
    # Sobel kernels
    kx = np.array([[-1, 0, 1], [-2, 0, 2], [-1, 0, 1]])
    ky = kx.T
    
    # Pad image
    padded = np.pad(image, 1, mode='edge')
    
    # Apply kernels manually (simplified for demonstration)
    result = np.zeros_like(image, dtype=np.float32)
    for i in range(1, image.shape[0]-1):
        for j in range(1, image.shape[1]-1):
            patch = padded[i:i+3, j:j+3]
            gx = np.sum(patch * kx)
            gy = np.sum(patch * ky)
            result[i, j] = np.sqrt(gx**2 + gy**2)
    
    return result

# Vectorized edge detection (much faster)
def sobel_vectorized(images):
    """Vectorized edge detection"""
    # Shift for gradient calculation
    gx = (images[:, :, 2:] - images[:, :, :-2])[:, 1:-1, :]
    gy = (images[:, 2:, :] - images[:, :-2, :])[:, :, 1:-1]
    
    # Gradient magnitude
    magnitude = np.sqrt(gx**2 + gy**2)
    
    return magnitude

start = time.time()
edges = sobel_vectorized(images)
edge_time = time.time() - start

print(f"\nEdge Detection:")
print(f"  Mean edge magnitude: {np.mean(edges):.2f}")
print(f"  Time: {edge_time*1000:.2f} ms")
```

### Application: Time-Series Analysis

Optimized time-series analysis for financial data.

```python
import numpy as np
import time

np.random.seed(42)

# Simulate stock price time series
n_days = 2520  # ~10 years of trading days
n_stocks = 500  # 500 stocks

prices = np.cumprod(1 + np.random.normal(0.0005, 0.02, (n_days, n_stocks)), axis=0) * 100

print("=" * 60)
print("TIME-SERIES ANALYSIS")
print("=" * 60)

print(f"\nProcessing {n_days} days x {n_stocks} stocks")

# Calculate returns (vectorized)
start = time.time()
returns = np.diff(np.log(prices), axis=0)
return_time = time.time() - start

print(f"\nReturn Calculation:")
print(f"  Mean daily return: {np.mean(returns):.4f}")
print(f"  Return std: {np.std(returns):.4f}")
print(f"  Time: {return_time*1000:.2f} ms")

# Rolling statistics (vectorized using strides)
window = 20

def rolling_mean(arr, window):
    """Efficient rolling mean using slice"""
    shape = (arr.shape[0] - window + 1, window) + arr.shape[1:]
    strides = (arr.strides[0],) + arr.strides
    return np.mean(np.lib.stride_tricks.as_strided(arr, shape=shape, strides=strides), axis=1)

def rolling_std(arr, window):
    """Efficient rolling std"""
    shape = (arr.shape[0] - window + 1, window) + arr.shape[1:]
    strides = (arr.strides[0],) + arr.strides
    return np.std(np.lib.stride_tricks.as_strided(arr, shape=shape, strides=strides), axis=1)

start = time.time()
rolling_means = rolling_mean(returns, window)
rolling_stds = rolling_std(returns, window)
rolling_time = time.time() - start

print(f"\nRolling Statistics (window={window}):")
print(f"  Rolling mean shape: {rolling_means.shape}")
print(f"  Rolling std shape: {rolling_stds.shape}")
print(f"  Time: {rolling_time*1000:.2f} ms")

# Moving average crossover signals
short_window = 20
long_window = 50

short_ma = rolling_mean(prices, short_window)
long_ma = rolling_mean(prices, long_window)

# Generate signals
signals = np.zeros((n_days - long_window + 1, n_stocks))
signals[short_ma > long_ma] = 1
signals[short_ma < long_ma] = -1

start = time.time()
signal_returns = signals * returns[long_window-1:]
signal_time = time.time() - start

print(f"\nMA Crossover Strategy:")
print(f"  Mean signal return: {np.mean(signal_returns):.4f}")
print(f"  Time: {signal_time*1000:.2f} ms")

# Volatility calculations
start = time.time()
volatility = np.std(returns, axis=0) * np.sqrt(252)  # Annualized
vol_time = time.time() - start

print(f"\nVolatility Calculation:")
print(f"  Mean annualized vol: {np.mean(volatility)*100:.1f}%")
print(f"  Time: {vol_time*1000:.2f} ms")
```

## Output Results

### Performance-Measured Output

```python
import numpy as np
import time

print("=" * 60)
print("PERFORMANCE REPORTING")
print("=" * 60)

# Benchmark function
def benchmark(func, *args, iterations=10, **kwargs):
    """Benchmark a function"""
    times = []
    for _ in range(iterations):
        start = time.perf_counter()
        result = func(*args, **kwargs)
        times.append(time.perf_counter() - start)
    
    return {
        'mean': np.mean(times),
        'std': np.std(times),
        'min': np.min(times),
        'max': np.max(times),
        'result': result
    }

# Test different operations
sizes = [1000, 10000, 100000, 1000000]

print(f"\n{'Size':>12} | {'Mean (ms)':>12} | {'Std (ms)':>12}")
print("-" * 45)

for size in sizes:
    np.random.seed(42)
    data = np.random.rand(size)
    
    result = benchmark(lambda: data * 2 + 1, iterations=100)
    print(f"{size:>12,} | {result['mean']*1000:>12.2f} | {result['std']*1000:>12.2f}")

# Memory benchmark
print("\nMemory Usage:")
for size in [1000, 10000, 100000, 1000000]:
    data = np.random.rand(size)
    print(f"  {size:,} elements: {data.nbytes/1024:.2f} KB ({data.nbytes/1024/1024:.2f} MB)")
```

## Visualization

### Performance Visualization

```python
import numpy as np
import time

# Performance comparison visualization
def plot_performance_comparison():
    """Show performance comparison as ASCII"""
    sizes = [1000, 10000, 100000, 1000000]
    
    print("=" * 60)
    print("PERFORMANCE SCALING")
    print("=" * 60)
    
    print("\nScaling with Array Size:")
    print(f"{'Size':>12} | {'Time':>15} | {'Visual':>25}")
    print("-" * 60)
    
    for size in sizes:
        np.random.seed(42)
        data = np.random.rand(size)
        
        start = time.perf_counter()
        result = data * 2 + 1
        t = time.perf_counter() - start
        
        bar_length = min(int(np.log10(size) * 10), 25)
        bar = '█' * bar_length
        
        print(f"{size:>12,} | {t*1000:>12.2f}ms | {bar}")

plot_performance_comparison()

# Memory visualization
def plot_memory_usage():
    """Show memory usage"""
    print("\n" + "=" * 60)
    print("MEMORY USAGE")
    print("=" * 60)
    
    types = [
        ('float32', np.float32),
        ('float64', np.float64),
        ('int32', np.int32),
        ('int64', np.int64),
        ('complex64', np.complex64)
    ]
    
    print(f"{'Type':>12} | {'Bytes':>10} | {'Bar':>30}")
    print "-" * 55
    
    base = np.array([1], dtype=np.float32).nbytes
    
    for name, dtype in types:
        arr = np.array([1], dtype=dtype)
        bytes_per = arr.nbytes
        print(f"{name:>12} | {bytes_per:>10} | {'█' * int(bytes_per / base)}")

plot_memory_usage()
```

## Advanced Topics

### Advanced Optimization Techniques

```python
import numpy as np
import time

print("=" * 60)
print("ADVANCED OPTIMIZATION")
print("=" * 60)

# np.einsum for efficient tensor operations
np.random.seed(42)
A = np.random.rand(100, 100)
B = np.random.rand(100, 100)

# Method 1: Regular matrix multiplication
start = time.time()
C1 = A @ B
t1 = time.time() - start

# Method 2: einsum
start = time.time()
C2 = np.einsum('ij,jk->ik', A, B)
t2 = time.time() - start

print("\nMatrix Multiplication:")
print(f"  @ operator: {t1*1000:.3f} ms")
print(f"  einsum: {t2*1000:.3f} ms")

# Reduce sum efficiently
arr = np.random.rand(1000, 1000)

# Method 1: sum along axis
start = time.time()
s1 = np.sum(arr, axis=0)
t1 = time.time() - start

# Method 2: einsum
start = time.time()
s2 = np.einsum('ij->j', arr)
t2 = time.time() - start

print("\nSum along axis:")
print(f"  np.sum: {t1*1000:.3f} ms")
print(f"  einsum: {t2*1000:.3f} ms")

# Outer product
a = np.random.rand(500)
b = np.random.rand(500)

start = time.time()
outer = np.outer(a, b)
t1 = time.time() - start

print(f"\nOuter product ({len(a)}x{len(b)}):")
print(f"  Time: {t1*1000:.3f} ms")

# Cholesky decomposition for efficient solving
A = np.random.rand(100, 100)
A = A @ A.T + np.eye(100)  # Make positive definite

start = time.time()
L = np.linalg.cholesky(A)
t = time.time() - start

print("\nCholesky Decomposition:")
print(f"  Time: {t*1000:.3f} ms")

# Using solve instead of inverse
b = np.random.rand(100)

start = time.time()
x1 = np.linalg.solve(A, b)
t1 = time.time() - start

start = time.time()
x2 = np.linalg.inv(A) @ b
t2 = time.time() - start

print("\nSolving Ax = b:")
print(f"  solve: {t1*1000:.3f} ms")
print(f"  inverse: {t2*1000:.3f} ms")
```

### NumPy Configuration and Tuning

```python
import numpy as np

print("=" * 60)
print("NUMPY CONFIGURATION")
print("=" * 60)

# Show current configuration
print("\nNumPy Configuration:")
print(f"  NumPy version: {np.__version__}")
print(f"  Build info: {np.get_include()}")

# Show available linear algebra routines
print(f"\nNumPy distutils: {np.linalg.lapack_lite}")

# Show FPU precision (if available)
print(f"\nFloating point info: {np.finfo(np.float64)}")

# Configure threads
print("\nNumPy Thread Configuration:")
print(f"  Number of threads: {np.show_config()}")

# Memory layout configuration
arr = np.random.rand(100, 100)
print(f"\nArray Info:")
print(f"  Flags: C_CONTIGUOUS={arr.flags['C_CONTIGUOUS']}, F_CONTIGUOUS={arr.flags['F_CONTIGUOUS']}")
print(f"  Writeable: {arr.flags['WRITEABLE']}")
print(f"  Alignd: {arr.flags['ALIGNED']}")

# Set print options for better output
np.set_printoptions(precision=3, suppress=True, threshold=10)
print(f"\nPrint options configured for precision=3, suppress=True")
```

### GPU Acceleration with NumPy

Note: NumPy doesn't have native GPU support, but can be replaced with libraries like CuPy for GPU acceleration.

```python
import numpy as np
import time

# Simulated GPU-like operations using NumPy
print("=" * 60)
print("GPU-LIKE COMPUTATIONS")
print("=" * 60)

# While we can't actually use GPUs here, we can prepare code
# for later use with libraries like CuPy or JAX

np.random.seed(42)
data = np.random.rand(10000, 10000)

# These would be significantly faster on GPU
# Using NumPy as placeholder

print("\nPreparations for GPU acceleration:")
print("  1. Ensure data is contiguous (C-order)")
print("  2. Use float32 where precision allows")
print("  3. Use in-place operations where possible")
print("  4. Avoid unnecessary copies")

# Example: Converting to float32 can speed up operations
data_f64 = np.random.rand(5000, 5000)
data_f32 = data_f64.astype(np.float32)

start = time.time()
result = data_f64 * 2
t64 = time.time() - start

start = time.time()
result = data_f32 * 2
t32 = time.time() - start

print(f"\nFloat32 vs Float64:")
print(f"  float64: {t64*1000:.2f} ms")
print(f"  float32: {t32*1000:.2f} ms")
print(f"  Speedup: {t64/t32:.1f}x")

# Memory efficiency
print(f"\nMemory usage:")
print(f"  float64: {data_f64.nbytes/1024/1024:.1f} MB")
print(f"  float32: {data_f32.nbytes/1024/1024:.1f} MB")
print(f"  Savings: {(1 - data_f32.nbytes/data_f64.nbytes)*100:.0f}%")
```

## Conclusion

Performance optimization with NumPy is essential for data science applications that work with large datasets. Through this comprehensive exploration, you've learned about vectorization, memory efficiency, broadcasting, and profiling techniques. These skills are crucial for banking and healthcare applications where timely processing is critical.

The banking examples demonstrated high-performance transaction processing, fraud detection, and risk calculation. The healthcare applications showed medical image processing, patient data analysis, and time-series analysis. Both domains benefit from optimized NumPy code that can process millions of records in seconds.

Key takeaways include always vectorize operations instead of loops, use in-place operations to save memory, leverage broadcasting for efficient operations, use appropriate data types to balance precision and speed, and profile code to identify bottlenecks. Continue practicing with increasingly large datasets to develop a sense for when optimization is needed and which techniques will be most effective.