# 🧹 Data Cleaning

## 🎯 What You'll Learn

- Detect missing values with .isna() and .notna()
- Handle missing data: drop or fill
- Change data types with .astype()
- Rename columns and remove duplicates
- Clean string data with .str methods

## 📦 Prerequisites

- Read [01_dataframes_and_series.md](./01_dataframes_and_series.md) first

## Real Data is Messy!

Real-world data is like your room — rarely clean! You'll encounter:
- **Missing values** — blank cells, "NA", "-999"
- **Wrong types** — numbers stored as text
- **Inconsistent naming** — "NYC", "New York", "new york"
- **Duplicates** — same data entered twice

Let's clean a messy sales dataset!

## Our Messy Dataset

```python
import pandas as pd
import numpy as np

# Create a messy sales dataset
sales: pd.DataFrame = pd.DataFrame({
    "date": ["2024-01-01", "2024-01-02", "2024-01-03", "2024-01-02", "2024-01-04"],
    "product": ["Laptop", "Mouse", "Keyboard", "mouse", None],
    "quantity": [1, 2, 1, np.nan, 3],
    "price": ["999.00", "29.99", "79.99", "29.99", "149.99"],
    "city": ["NYC", "nyc", "New York", "NYC", "NYC "],  # messy!
})

print("BEFORE CLEANING:")
print(sales)
print("\n")
print(sales.dtypes)
```

### Output (Before)

```
         date  product  quantity   price    city
0  2024-01-01   Laptop         1.0  999.00     NYC
1  2024-01-02    Mouse         2.0  29.99     nyc
2  2024-01-03  Keyboard         1.0  79.99  New York
3  2024-01-02     mouse         NaN  29.99     NYC
4  2024-01-04     None         3.0  149.99    NYC 
```

## Detecting Missing Values

```python
# Check for missing values (True = missing)
print(sales.isna())
#         date  product  quantity  price   city
# 0    False    False     False  False  False
# 1    False    False    False  False  False
# 2    False    False    False  False  False
# 3    False    False     True  False  False
# 4    False     True    False  False  False

# Count missing per column
print(sales.isna().sum())
# date         0
# product      1
# quantity     1
# price        0
# city         0
```

### 💡 Line-by-Line Breakdown

- `.isna()` - Returns True for missing values (NaN, None, NaT)
- `.isna().sum()` - Count missing values per column

## Handling Missing Values

### Option 1: Drop Rows with Missing Values

```python
# Drop rows with ANY missing value
cleaned: pd.DataFrame = sales.dropna()
print(f"Rows before: {len(sales)}, after: {len(cleaned)}")
# Rows before: 5, after: 3

# Drop rows where SPECIFIC column is missing
cleaned2: pd.DataFrame = sales.dropna(subset=["product"])
```

### Option 2: Fill Missing Values

```python
# Fill with a specific value
filled: pd.DataFrame = sales.fillna(0)  # Fill with 0

# Forward fill: use previous value
filled_fwd: pd.DataFrame = sales.fillna(method="ffill")

# Fill with mean (for numeric columns)
sales["quantity"] = sales["quantity"].fillna(sales["quantity"].mean())
```

### 💡 Explanation

- **Drop**: Use when missing data is rare and you can afford to lose rows
- **Fill with 0**: Good for counts or when missing means "none"
- **Forward fill**: Good for time series (carry forward last known value)
- **Fill with mean**: Preserves average, good for continuous data

## Changing Data Types

Our price column is a string — let's fix that!

```python
# Convert string to numeric
sales["price"] = pd.to_numeric(sales["price"])
print(sales.dtypes)
# date        object
# product     object
# quantity    float64
# price       float64  ← now numeric!
# city        object
```

### 💡 Line-by-Line Breakdown

- `pd.to_numeric()` - Convert string/other to numeric type
- Also works: `df["col"].astype(int)` type changes for simple

## Renaming Columns

```python
# Rename specific columns
sales_renamed: pd.DataFrame = sales.rename(columns={
    "date": "order_date",
    "product": "item_name"
})

# Rename all columns to lowercase
sales.columns = sales.columns.str.lower()
print(sales.columns)
# Index(['date', 'product', 'quantity', 'price', 'city'], dtype='object')
```

## Removing Duplicates

```python
# Find duplicates
print(sales.duplicated())
# 0    False
# 1    False
# 2    False
# 3    False
# 4    False
# Wait, row 1 and row 3 have same date/product...

# Remove duplicates
deduplicated: pd.DataFrame = sales.drop_duplicates()
print(f"Rows: {len(sales)} → {len(deduplicated)}")
```

## Cleaning String Data

Strings need special love:

```python
# Convert to lowercase
sales["city"] = sales["city"].str.lower()
# "NYC" → "nyc", "New York" → "new york"

# Remove whitespace
sales["city"] = sales["city"].str.strip()
# "NYC " → "NYC"

# Replace values
sales["product"] = sales["product"].str.replace("mouse", "Mouse")
```

### String Method Chaining

```python
# Chain multiple string operations
sales["product"] = (
    sales["product"]
    .str.lower()           # lowercase
    .str.strip()           # remove whitespace
    .str.replace("laptop", "Laptop")  # fix case
)
```

### 💡 Explanation

String methods in Pandas use `.str.` prefix. Common ones:
- `.str.lower()`, `.str.upper()`, `.str.title()`
- `.str.strip()`, `.str.lstrip()`, `.str.rstrip()`
- `.str.replace(old, new)`
- `.str.contains(substring)`

## Replacing Specific Values

```python
# Replace values (not just missing)
sales["product"] = sales["product"].replace("mouse", "Mouse")

# Replace multiple at once
sales["product"] = sales["product"].replace({
    "mouse": "Mouse",
    "keyboard": "Keyboard",
    None: "Unknown"
})
```

## Complete Cleaning Pipeline

```python
import pandas as pd
import numpy as np

# Reload messy data
sales: pd.DataFrame = pd.DataFrame({
    "date": ["2024-01-01", "2024-01-02", "2024-01-03", "2024-01-02", "2024-01-04"],
    "product": ["Laptop", "Mouse", "Keyboard", "mouse", None],
    "quantity": [1, 2, 1, np.nan, 3],
    "price": ["999.00", "29.99", "79.99", "29.99", "149.99"],
    "city": ["NYC", "nyc", "New York", "NYC", "NYC "],
})

# Step 1: Fix types
sales["price"] = pd.to_numeric(sales["price"])

# Step 2: Clean strings
sales["city"] = sales["city"].str.lower().str.strip()
sales["product"] = sales["product"].str.lower().str.strip()

# Step 3: Fill missing
sales["product"] = sales["product"].fillna("Unknown")
sales["quantity"] = sales["quantity"].fillna(sales["quantity"].mean())

# Step 4: Remove duplicates
sales = sales.drop_duplicates()

print("\nAFTER CLEANING:")
print(sales)
```

### Final Output

```
         date   product  quantity   price    city
0  2024-01-01    laptop       1.0  999.00      nyc
1  2024-01-02     mouse       2.0   29.99      nyc
2  2024-01-03  keyboard       1.0   79.99  new york
4  2024-01-04  unknown       3.0  149.99      nyc
```

## ✅ Summary

- **Missing values**: `.isna()` to detect, `.fillna()` or `.dropna()` to handle
- **Types**: Use `pd.to_numeric()` or `.astype()`
- **Strings**: `.str.lower()`, `.str.strip()`, `.str.replace()`
- **Duplicates**: `.drop_duplicates()`
- **Renaming**: `.rename(columns={...})`

## ➡️ Next Steps

Ready to find patterns? Head to **[03_groupby_and_aggregation.md](./03_groupby_and_aggregation.md)** to learn grouping!

## 🔗 Further Reading

- [Working with Missing Data](https://pandas.pydata.org/docs/user_guide/missing_data.html)
- [String Methods](https://pandas.pydata.org/docs/user_guide/text.html)
- [Cleaning Data in Pandas](https://realpython.com/pandas-data-cleaning/)
