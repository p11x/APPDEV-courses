# 🔗 Merging and Reshaping Data

## 🎯 What You'll Learn

- Combine DataFrames with pd.concat() and pd.merge()
- Understand inner, left, right, and outer joins
- Transform data with melt and pivot
- One-hot encoding with pd.get_dummies()

## 📦 Prerequisites

- Read [03_groupby_and_aggregation.md](./03_groupby_and_aggregation.md) first

## Why Combine Data?

Real data lives in multiple tables! Think of a database:
- **Customers** table: customer info
- **Orders** table: what they bought
- **Products** table: product details

You need to combine them to answer questions!

## pd.concat: Stacking Data

### Vertical Stacking (Add Rows)

```python
import pandas as pd

df1: pd.DataFrame = pd.DataFrame({
    "name": ["Alice", "Bob"],
    "age": [25, 30]
})

df2: pd.DataFrame = pd.DataFrame({
    "name": ["Charlie", "Diana"],
    "age": [35, 28]
})

# Stack vertically (add rows)
combined: pd.DataFrame = pd.concat([df1, df2], ignore_index=True)
print(combined)
```

### Output

```
      name  age
0    Alice   25
1      Bob   30
2  Charlie   35
3    Diana   28
```

### 💡 Line-by-Line Breakdown

- `pd.concat([df1, df2])` - Stack DataFrames vertically
- `ignore_index=True` - Reset index to 0, 1, 2...

### Horizontal Stacking (Add Columns)

```python
import pandas as pd

df1: pd.DataFrame = pd.DataFrame({
    "name": ["Alice", "Bob"],
    "age": [25, 30]
})

df2: pd.DataFrame = pd.DataFrame({
    "city": ["NYC", "LA"],
    "salary": [55000, 72000]
})

# Stack horizontally (add columns)
combined: pd.DataFrame = pd.concat([df1, df2], axis=1)
print(combined)
```

## pd.merge: Combining Like SQL JOINs

This is like SQL JOINs — combine based on a common column!

### Setup: Customers and Orders

```python
import pandas as pd

# Customers table
customers: pd.DataFrame = pd.DataFrame({
    "customer_id": [1, 2, 3, 4],
    "name": ["Alice", "Bob", "Charlie", "Diana"],
    "city": ["NYC", "LA", "NYC", "Chicago"]
})

# Orders table
orders: pd.DataFrame = pd.DataFrame({
    "order_id": [101, 102, 103, 104],
    "customer_id": [1, 2, 2, 5],  # Note: customer 5 doesn't exist!
    "amount": [100, 250, 75, 500]
})

print("Customers:")
print(customers)
print("\nOrders:")
print(orders)
```

### Inner Join: Only Matching Records

```python
# Inner join: only customers with orders
inner: pd.DataFrame = pd.merge(customers, orders, on="customer_id", how="inner")
print(inner)
```

### Output (Inner Join)

```
   customer_id   name    city  order_id  amount
0            1  Alice     NYC      101     100
1            2    Bob      LA      102     250
2            2    Bob      LA      103      75
```

### Left Join: All Customers, Their Orders

```python
# Left join: all customers, their orders (if any)
left: pd.DataFrame = pd.merge(customers, orders, on="customer_id", how="left")
print(left)
```

### Output (Left Join)

```
   customer_id   name       city  order_id  amount
0            1  Alice       NYC      101  100.0
1            2    Bob        LA      102  250.0
2            2    Bob        LA      103   75.0
3            3  Charlie     NYC      NaN    NaN
4            4   Diana  Chicago      NaN    NaN
```

### Right Join: All Orders, Customer Info

```python
# Right join: all orders, customer info where available
right: pd.DataFrame = pd.merge(customers, orders, on="customer_id", how="right")
print(right)
```

### Output (Right Join)

```
   customer_id   name     city  order_id  amount
0            1  Alice      NYC      101     100
1            2    Bob        LA      102     250
2            2    Bob        LA      103      75
3            5    NaN      NaN      104     500
```

### Outer Join: Everything

```python
# Outer join: all records from both
outer: pd.DataFrame = pd.merge(customers, orders, on="customer_id", how="outer")
print(outer)
```

## Visual: Join Types

```
Customers:          Orders:
┌─────────────┐     ┌─────────────┐
│ 1: Alice    │     │ 1: $100     │
│ 2: Bob      │     │ 2: $250     │
│ 3: Charlie  │     │ 2: $75      │
│ 4: Diana    │     │ 5: $500     │ ← no matching customer!
└─────────────┘     └─────────────┘

Inner:   Only {1, 2}  (matching)      → Alice + Bob (3 rows)
Left:    All customers                → Alice, Bob ×2, Charlie, Diana (4 rows)
Right:   All orders                   → Alice, Bob ×2, Order 5 (4 rows)
Outer:   Everything                   → All 5 rows
```

## .join() Method

Shorthand when index is the key:

```python
# If both have customer_id as index
customers_idx: pd.DataFrame = customers.set_index("customer_id")
orders_idx: pd.DataFrame = orders.set_index("customer_id")

# Join on index
joined: pd.DataFrame = customers_idx.join(orders_idx, how="left")
```

## Reshaping: melt and pivot

### melt(): Wide to Long

```python
import pandas as pd

# Wide format (one column per month)
wide: pd.DataFrame = pd.DataFrame({
    "product": ["Laptop", "Phone"],
    "Jan": [100, 80],
    "Feb": [110, 85],
    "Mar": [120, 90]
})

print("WIDE:")
print(wide)

# Melt to long format
long: pd.DataFrame = wide.melt(
    id_vars=["product"],        # Column to keep
    var_name="month",           # Name for month column
    value_name="sales"          # Name for sales column
)

print("\nLONG:")
print(long)
```

### Output

```
WIDE:
   product  Jan  Feb  Mar
0  Laptop   100  110  120
1   Phone    80   85   90

LONG:
   product month  sales
0  Laptop   Jan    100
1   Phone   Jan     80
2  Laptop   Feb    110
3   Phone   Feb     85
4  Laptop   Mar    120
5   Phone   Mar     90
```

### pivot(): Long to Wide

```python
# Pivot back to wide
back_to_wide: pd.DataFrame = long.pivot(
    index="product",
    columns="month",
    values="sales"
).reset_index()

print(back_to_wide)
```

## One-Hot Encoding: pd.get_dummies()

Machine learning needs numbers! Convert categories to binary columns:

```python
import pandas as pd

# Data with categorical column
df: pd.DataFrame = pd.DataFrame({
    "name": ["Alice", "Bob", "Charlie"],
    "city": ["NYC", "LA", "NYC"],
    "age": [25, 30, 35]
})

# One-hot encode 'city'
encoded: pd.DataFrame = pd.get_dummies(df, columns=["city"])
print(encoded)
```

### Output

```
      name  age  city_LA  city_NYC
0    Alice   25       0        1
1      Bob   30       1        0
2  Charlie   35       0        1
```

### 💡 Explanation

- `city_LA` = 1 if city is LA, else 0
- `city_NYC` = 1 if city is NYC, else 0

This is essential for ML models that need numeric input!

## ✅ Summary

- `pd.concat()` - stack DataFrames vertically or horizontally
- `pd.merge()` - join like SQL (inner, left, right, outer)
- `.join()` - shorthand when using index
- `.melt()` - wide to long format
- `.pivot()` - long to wide format
- `pd.get_dummies()` - one-hot encoding for ML

## ➡️ Next Steps

Ready to visualize your data? Head to **[../03_Visualization/01_matplotlib_basics.md](../03_Visualization/01_matplotlib_basics.md)**!

## 🔗 Further Reading

- [Merge Documentation](https://pandas.pydata.org/docs/user_guide/merging.html)
- [Join Methods](https://pandas.pydata.org/docs/reference/api/pandas.DataFrame.join.html)
- [Reshaping](https://pandas.pydata.org/docs/user_guide/reshaping.html)
