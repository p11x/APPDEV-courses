# 📊 DataFrames and Series

## 🎯 What You'll Learn

- What Pandas is and why it's Excel for Python
- Create Series and DataFrames
- Inspect data with .head(), .tail(), .info(), .describe()
- Select columns and rows with .iloc[] and .loc[]
- The difference between iloc and loc

## 📦 Prerequisites

- Complete the NumPy section (folder 09/01_NumPy)

## What is Pandas?

Think of Pandas as **Microsoft Excel** but programmable, more powerful, and in Python! While NumPy handles numbers efficiently, Pandas handles **mixed data types** — numbers, text, dates — beautifully.

### Installing Pandas

```python
pip install pandas  # Install the package
```

### Import Convention

```python
import pandas as pd  # Convention: import as 'pd'
```

## Series: One-Dimensional Data

A Series is like a column in Excel — a labeled array of data:

```python
import pandas as pd

# Create a Series from a list
fruits: pd.Series = pd.Series(["Apple", "Banana", "Cherry", "Date"])
print(fruits)
# 0     Apple
# 1    Banana
# 2    Cherry
# 3       Date
# dtype: object
```

### Series with Custom Index

```python
import pandas as pd

# Create with custom labels
prices: pd.Series = pd.Series(
    data=[1.50, 0.75, 3.00, 0.50],
    index=["Apple", "Banana", "Cherry", "Date"],
    name="Fruit Prices"
)

print(prices)
# Apple     1.50
# Banana    0.75
# Cherry    3.00
# Date      0.50
# Name: Fruit Prices, dtype: float64
```

### 💡 Line-by-Line Breakdown

- `pd.Series(data=[...], index=[...])` - Create Series with custom labels
- `name="Fruit Prices"` - Name the Series (like a column header)

### Series Attributes

```python
import pandas as pd

data: list[int] = [10, 20, 30, 40]
s: pd.Series = pd.Series(data, index=["a", "b", "c", "d"])

print(s.values)    # [10 20 30 40] - the data
print(s.index)     # Index(['a', 'b', 'c', 'd'], dtype='object')
print(s.dtype)     # int64 - data type
print(s.size)      # 4 - number of elements
print(s.name)      # None - Series name
```

## DataFrame: Two-Dimensional Data

A DataFrame is like an entire spreadsheet — rows AND columns!

### Create from Dictionary

```python
import pandas as pd

# Create DataFrame from dictionary (keys become column names)
movies: pd.DataFrame = pd.DataFrame({
    "title": ["The Matrix", "Inception", "Interstellar", "Tenet"],
    "year": [1999, 2010, 2014, 2020],
    "rating": [8.7, 8.8, 8.6, 7.5],
    "genre": ["Sci-Fi", "Sci-Fi", "Sci-Fi", "Sci-Fi"]
})

print(movies)
```

### Output

```
         title  year  rating genre
0    The Matrix  1999    8.7  Sci-Fi
1     Inception  2010    8.8  Sci-Fi
2  Interstellar  2014    8.6  Sci-Fi
3        Tenet   2020    7.5  Sci-Fi
```

### 💡 Line-by-Line Breakdown

- Keys in dict become column names
- Each value list becomes a column
- Row indices are auto-generated (0, 1, 2, ...)

## Inspecting DataFrames

```python
import pandas as pd

# Create sample DataFrame
df: pd.DataFrame = pd.DataFrame({
    "name": ["Alice", "Bob", "Charlie", "Diana", "Eve"],
    "age": [25, 30, 35, 28, 32],
    "city": ["NYC", "LA", "Chicago", "NYC", "Boston"],
    "salary": [55000, 72000, 68000, 61000, 75000]
})

# First 3 rows
print(df.head(3))
#      name  age     city  salary
# 0   Alice   25      NYC   55000
# 1     Bob   30       LA   72000
# 2  Charlie  35  Chicago   68000

# Last 2 rows
print(df.tail(2))
#      name  age    city  salary
# 3   Diana   28      NYC   61000
# 4     Eve   32  Boston   75000

# Quick info
print(df.info())
# <class 'pandas.core.frame.DataFrame'>
# RangeIndex: 5 entries, 0 to 4
# Data columns (total 4 columns):
#  name    5 non-null object
#  age     5 non-null int64
#  city    5 non-null object
# salary   5 non-null int64

# Statistical summary
print(df.describe())
#          age        salary
# count   5.0      5.000000
# mean   30.0  66200.000000
# std     3.5   7782.704...
# min    25.0  55000.000000
# ...
```

### 💡 Line-by-Line Breakdown

- `.head(n)` - Show first n rows (default 5)
- `.tail(n)` - Show last n rows
- `.info()` - Data types, memory, null counts
- `.describe()` - Statistics for numeric columns

## Selecting Columns

```python
import pandas as pd

df: pd.DataFrame = pd.DataFrame({
    "name": ["Alice", "Bob", "Charlie"],
    "age": [25, 30, 35],
    "salary": [55000, 72000, 68000]
})

# Single column - returns a Series
ages: pd.Series = df["age"]
print(ages)
# 0    25
# 1    30
# 2    35
# Name: age, dtype: int64

# Multiple columns - returns a DataFrame
subset: pd.DataFrame = df[["name", "salary"]]
print(subset)
#       name  salary
# 0     Alice   55000
# 1       Bob   72000
# 2   Charlie   68000
```

### 💡 Line-by-Line Breakdown

- `df["column"]` - Get one column (returns Series)
- `df[["col1", "col2"]]` - Get multiple columns (returns DataFrame)

## Selecting Rows

This is where it gets tricky! Pandas has two ways:

### .iloc[] — Position-Based (Integer Location)

```python
import pandas as pd

df: pd.DataFrame = pd.DataFrame({
    "name": ["Alice", "Bob", "Charlie", "Diana"],
    "age": [25, 30, 35, 28]
})

# Get first row by position
print(df.iloc[0])
# name    Alice
# age        25
# Name: 0, dtype: object

# Get rows 1-2 (positions, not inclusive!)
print(df.iloc[1:3])
#       name  age
# 1      Bob   30
# 2  Charlie   35

# Get specific positions
print(df.iloc[[0, 2]])  # rows 0 and 2
#       name  age
# 0     Alice   25
# 2  Charlie   35
```

### .loc[] — Label-Based

```python
import pandas as pd

df: pd.DataFrame = pd.DataFrame({
    "name": ["Alice", "Bob", "Charlie", "Diana"],
    "age": [25, 30, 35, 28]
}, index=["a", "b", "c", "d"])

# Get row by index label
print(df.loc["a"])
# name    Alice
# age        25
# Name: a, dtype: object

# Get range of labels (INCLUSIVE!)
print(df.loc["a":"c"])
#       name  age
# a     Alice   25
# b       Bob   30
# c  Charlie   35
```

### Visual: iloc vs loc

```
DataFrame with index ["a", "b", "c", "d"]:

        iloc (positions)     loc (labels)
        ─────────────────    ────────────
        iloc[0]  → row "a"   loc["a"] → row "a"
        iloc[1]  → row "b"   loc["b"] → row "b"
        iloc[2]  → row "c"   loc["c"] → row "c"
        
Key difference:
- iloc[0:2] → positions 0 and 1 (EXCLUSIVE)
- loc["a":"b"] → labels "a" and "b" (INCLUSIVE!)
```

### 💡 Explanation

- **iloc** = "integer location" — uses positions (0, 1, 2...)
- **loc** — uses index labels ("a", "b", "c"...)
- Most beginners use iloc most of the time

## Combined Selection

```python
import pandas as pd

df: pd.DataFrame = pd.DataFrame({
    "name": ["Alice", "Bob", "Charlie", "Diana"],
    "age": [25, 30, 35, 28],
    "salary": [55000, 72000, 68000, 61000]
}, index=[100, 101, 102, 103])

# Row 0-1, columns "name" and "salary"
print(df.iloc[0:2, [0, 2]])
#      name  salary
# 100  Alice   55000
# 101    Bob   72000

# Row label 100-102, columns age through salary
print(df.loc[100:102, "age":"salary"])
#      age  salary
# 100   25   55000
# 101   30   72000
# 102   35   68000
```

## ✅ Summary

- **Series**: One-dimensional labeled array (like a column)
- **DataFrame**: Two-dimensional table (like a spreadsheet)
- `.head()`, `.tail()`, `.info()`, `.describe()` — essential inspection methods
- `df["col"]` — select one or more columns
- `.iloc[]` — select by position (0, 1, 2...)
- `.loc[]` — select by label

## ➡️ Next Steps

Real data is messy! Head to **[02_data_cleaning.md](./02_data_cleaning.md)** to learn data cleaning!

## 🔗 Further Reading

- [Pandas Documentation](https://pandas.pydata.org/docs/)
- [10 Minutes to Pandas](https://pandas.pydata.org/docs/user_guide/10min.html)
- [DataFrame Intro](https://pandas.pydata.org/docs/getting_started/intro_tutorials/01_table_oriented.html)
