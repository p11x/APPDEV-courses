# Python Integration with reticulate

## Learning Objectives

- Set up reticulate for R-Python integration
- Call Python functions from R
- Transfer data between R and Python
- Use Python virtual environments
- Call Python libraries from R

## Theoretical Background

### reticulate Package

reticulate provides an interface between R and Python, enabling:

1. **Python in R** - Call Python code directly from R
2. **R in Python** - Call R from Python (via rpy2)
3. **Data conversion** - Automatic conversion between R and Python objects
4. **Virtual environments** - Manage Python environments

### Installation Setup

reticulate requires:
- Python installed on the system
- reticulate R package
- Optional: Python virtual environments

### Data Type Mapping

| R Type | Python Type |
|--------|-------------|
| numeric | float |
| integer | int |
| character | str |
| logical | bool |
| list | dict/list |
| data.frame | pandas.DataFrame |

## Code Examples

### Standard Example: Basic reticulate Setup

```r
# ===== RETICULATE BASICS =====

# Install reticulate if needed
# install.packages("reticulate")

cat("===== LOADING RETICULATE =====\n")

# Load reticulate
library(reticulate)

# Check Python version
cat("Python version:\n")
print(py_config()$python)

# Check if Python is available
cat("\nPython available:", py_available(), "\n")

# Create a simple Python string
py_run_string("x = 'hello from python'")
cat("x:", py$x, "\n")

# Create a Python list
py_run_string("my_list = [1, 2, 3, 4, 5]")
cat("my_list:", py$my_list, "\n")
cat("Type:", class(py$my_list), "\n")
```

### Standard Example: Python Functions

```r
# ===== PYTHON FUNCTIONS =====
cat("\n===== PYTHON FUNCTIONS =====\n")

# Define a Python function
py_run_string("
def greet(name):
    return 'Hello, ' + name + '!'

def add_numbers(a, b):
    return a + b

def process_list(lst):
    return [x * 2 for x in lst]
")

# Call from R
cat("greet('World'):", py$greet("World"), "\n")
cat("add_numbers(5, 3):", py$add_numbers(5, 3), "\n")

# Pass R vector to Python
r_vec <- c(1, 2, 3, 4, 5)
result <- py$process_list(r_vec)
cat("process_list(1:5):", result, "\n")

# Using py_function
cat("\n===== DEFINE PYTHON MODULE =====\n")

# Create a Python module file
py_module_code <- '
# mymodule.py
def calculate_stats(data):
    """Calculate basic statistics for data."""
    import statistics
    return {
        "mean": statistics.mean(data),
        "median": statistics.median(data),
        "stdev": statistics.stdev(data),
        "variance": statistics.variance(data)
    }

def normalize(data):
    """Normalize data to 0-1 range."""
    min_val = min(data)
    max_val = max(data)
    return [(x - min_val) / (max_val - min_val) for x in data]
'

cat("Example Python module:\n")
cat(py_module_code)

cat("\n\n===== IMPORT MODULE =====\n")
import_example <- "
# Import Python module from R
mymodule <- import_from_path(\"mymodule.py\", path = \"/path/to/module\")

# Call functions
data <- c(1, 2, 3, 4, 5)
stats <- mymodule$calculate_stats(data)
print(stats)

normalized <- mymodule$normalize(data)
print(normalized)
"
cat(import_example)
```

### Standard Example: Data Conversion

```r
# ===== DATA CONVERSION =====
cat("\n===== DATA CONVERSION =====\n")

# R vector to Python
cat("R vector to Python:\n")
r_vector <- c(1, 2, 3, 4, 5)
py_vector <- r_to_py(r_vector)
cat("Converted type:", class(py_vector), "\n")

# Python list to R
cat("\nPython list to R:\n")
py_run_string("py_list = [10, 20, 30, 40, 50]")
r_vector_from_py <- py_to_r(py$py_list)
print(r_vector_from_py)

# R dataframe to Python pandas
cat("\nR dataframe to pandas:\n")
r_df <- data.frame(
  id = 1:5,
  name = c("Alice", "Bob", "Charlie", "Diana", "Edward"),
  score = c(85, 90, 78, 92, 88),
  stringsAsFactors = FALSE
)

# Convert to pandas DataFrame
py_df <- r_to_py(r_df)
cat("Python type:", py_class(py_df), "\n")

# Display the dataframe
py_run_string("pandas_df = r_to_py(r_df)")
cat("\nPandas DataFrame:\n")
py_run_string("print(pandas_df)")
```

### Standard Example: Using numpy

```r
# ===== NUMPY INTEGRATION =====
cat("\n===== USING NUMPY =====\n")

# Import numpy
np <- import("numpy")

# Create numpy array
arr <- np$array(c(1, 2, 3, 4, 5))
cat("Created numpy array:\n")
print(arr)

# Array operations
cat("\nArray operations:\n")
cat("arr * 2:", arr * 2, "\n")
cat("np.sum():", np$sum(arr), "\n")
cat("np.mean():", np$mean(arr), "\n")

# Create 2D array
mat <- np$array(rbind(c(1, 2, 3), c(4, 5, 6)))
cat("\n2D array:\n")
print(mat)

cat("\nMatrix operations:\n")
cat("mat.shape:", paste(mat$shape, collapse = "x"), "\n")
cat("np.transpose(mat):\n")
print(np$transpose(mat))

# Create array with specific type
int_arr <- np$array(c(1, 2, 3), dtype = "int32")
cat("\nInteger array type:", int_arr$dtype, "\n")
```

### Standard Example: Using Python Libraries

```r
# ===== PYTHON LIBRARIES =====
cat("\n===== USING PYTHON LIBRARIES =====\n")

# Using pandas
cat("===== PANDAS =====\n")
pandas <- import("pandas")

# Create DataFrame
df <- pandas$DataFrame(
  data.frame(
    name = c("Alice", "Bob", "Charlie"),
    age = c(25, 30, 35),
    score = c(85, 90, 92)
  )
)
print(df)

# Operations
cat("\nDataFrame operations:\n")
cat("df.mean():\n")
print(df$mean())

cat("\ndf.apply():\n")
py_run_string("df_apply = df.apply(lambda x: x * 2 if x.name in ['score'] else x)")
print(df_apply)

# Using scikit-learn
cat("\n===== SCIKIT-LEARN =====\n")
sklearn <- import("sklearn")
datasets <- import("sklearn.datasets")
linear_model <- import("sklearn.linear_model")

# Simple linear regression
# Generate sample data
sample_data <- datasets$make_regression(n_samples = 100, 
                                   n_features = 1, 
                                   noise = 10)

X <- sample_data[[0]]
y <- sample_data[[1]]

# Fit model
model <- linear_model$LinearRegression()
model$fit(X, y)

cat("Coefficient:", model$coef_, "\n")
cat("Intercept:", model$intercept_, "\n")

# Predict
predictions <- model$predict(X)
cat("First 5 predictions:", head(predictions, 5), "\n")
```