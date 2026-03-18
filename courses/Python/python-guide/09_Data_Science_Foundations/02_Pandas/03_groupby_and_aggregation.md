# 📊 GroupBy and Aggregation

## 🎯 What You'll Learn

- Group data by categories with .groupby()
- Calculate statistics per group
- Use aggregation functions with .agg()
- Create pivot tables
- Sort and find top/bottom values

## 📦 Prerequisites

- Read [02_data_cleaning.md](./02_data_cleaning.md) first

## What is GroupBy?

Think of GroupBy like **pivot tables in Excel** — you split data into groups, calculate something for each group, then combine the results.

### The Three Steps

1. **Split** — Divide data into groups
2. **Apply** — Do something to each group
3. **Combine** — Put results back together

## Our Dataset: Supermarket Sales

```python
import pandas as pd
import numpy as np

# Create supermarket sales data
sales: pd.DataFrame = pd.DataFrame({
    "date": ["2024-01-01", "2024-01-01", "2024-01-02", "2024-01-02", "2024-01-03", "2024-01-03"],
    "city": ["NYC", "LA", "NYC", "LA", "NYC", "LA"],
    "category": ["Electronics", "Food", "Electronics", "Food", "Food", "Electronics"],
    "product": ["Laptop", "Bread", "Phone", "Milk", "Coffee", "Tablet"],
    "sales": [999, 5, 799, 3, 15, 450],
    "quantity": [1, 10, 1, 5, 20, 2]
})

print(sales)
```

### Output

```
         date    city    category    product  sales  quantity
0  2024-01-01    NYC  Electronics    Laptop    999         1
1  2024-01-01     LA        Food      Bread      5        10
2  2024-01-02    NYC  Electronics     Phone    799         1
3  2024-01-02     LA        Food       Milk      3         5
4  2024-01-03    NYC        Food      Coffee     15        20
5  2024-01-03     LA  Electronics    Tablet    450         2
```

## Basic GroupBy

```python
# Group by city - get average sales per city
city_sales: pd.DataFrame = sales.groupby("city")["sales"].mean()
print(city_sales)
# city
# LA     152.666667
# NYC    604.333333

# Group by category - get total quantity sold
category_qty: pd.Series = sales.groupby("category")["quantity"].sum()
print(category_qty)
# category
# Electronics    4
# Food          35
```

### 💡 Line-by-Line Breakdown

- `sales.groupby("city")` - Split data into groups by city
- `["sales"]` - Select the sales column
- `.mean()` - Calculate mean for each group

## Group by Multiple Columns

```python
# Group by city AND category
city_cat_sales: pd.DataFrame = sales.groupby(["city", "category"])["sales"].sum()
print(city_cat_sales)
```

### Output

```
city  category    
LA    Electronics    450
      Food             8
NYC   Electronics    1798
      Food            15
```

### Reset Index (Make It a Normal Table)

```python
# Convert to regular DataFrame
city_cat_df: pd.DataFrame = sales.groupby(["city", "category"])["sales"].sum().reset_index()
print(city_cat_df)
```

## Multiple Aggregations with .agg()

Get multiple statistics at once:

```python
# Multiple aggregations on one column
stats: pd.DataFrame = sales.groupby("city")["sales"].agg(["mean", "sum", "count", "min", "max"])
print(stats)
```

### Output

```
         mean        sum  count  min    max
city                                      
LA    152.666667    458      3    3   450
NYC   604.333333   1813      3    3   999
```

### Different Aggregations Per Column

```python
# Different stats for different columns
detailed: pd.DataFrame = sales.groupby("city").agg({
    "sales": ["sum", "mean"],
    "quantity": ["sum", "max"]
})
print(detailed)
```

## Custom Aggregation with .apply()

```python
# Custom function - find the top product in each group
def top_product(group: pd.DataFrame) -> str:
    """Return the product with highest sales."""
    return group.loc[group["sales"].idxmax(), "product"]

top_by_city: pd.Series = sales.groupby("city").apply(top_product)
print(top_by_city)
# city
# LA        Tablet
# NYC       Laptop
```

## Pivot Tables

Pivot tables are another way to summarize data:

```python
# Simple pivot table
pivot: pd.DataFrame = pd.pivot_table(
    sales,
    values="sales",
    index="city",
    columns="category",
    aggfunc="sum"
)
print(pivot)
```

### Output

```
category   Electronics  Food
city                       
LA              450.0    8.0
NYC             1798.0   15.0
```

### Fill Missing Values

```python
# Fill NaN with 0 for missing combinations
pivot_filled: pd.DataFrame = pivot.fillna(0)
```

## Value Counts

Quick frequency counting:

```python
# How many sales in each city?
city_counts: pd.Series = sales["city"].value_counts()
print(city_counts)
# NYC    3
# LA     3

# Get percentages
city_pct: pd.Series = sales["city"].value_counts(normalize=True)
print(city_pct)
# NYC    0.5
# LA     0.5
```

## Sorting

```python
# Sort by sales (descending = highest first)
sorted_sales: pd.DataFrame = sales.sort_values("sales", ascending=False)
print(sorted_sales)

# Sort by multiple columns
sorted_multi: pd.DataFrame = sales.sort_values(
    ["city", "sales"],  # First by city, then by sales
    ascending=[True, False]  # City A→Z, Sales high→low
)
```

## Top and Bottom Values

```python
# Top 2 products by sales
top_2: pd.DataFrame = sales.nlargest(2, "sales")
print(top_2)

# Bottom 2 products by quantity
bottom_2: pd.DataFrame = sales.nsmallest(2, "quantity")
print(bottom_2)
```

## Real Example: Which City Earns Most?

```python
import pandas as pd
import numpy as np

# Full analysis
summary: pd.DataFrame = sales.groupby("city").agg({
    "sales": ["sum", "mean", "count"],
    "quantity": "sum"
}).round(2)

summary.columns = ["Total Sales", "Avg Sale", "Num Transactions", "Total Quantity"]
summary = summary.sort_values("Total Sales", ascending=False)

print("🏆 CITY RANKING BY REVENUE:")
print(summary)
```

### Expected Output

```
🏆 CITY RANKING BY REVENUE:
         Total Sales  Avg Sale  Num Transactions  Total Quantity
city                                                           
NYC             1813    604.33                 3               22
LA               458    152.67                 3               17
```

## ✅ Summary

- `.groupby("column")` — split data by category
- `.agg(["mean", "sum", ...])` — multiple aggregations
- `.agg({"col1": "sum", "col2": "mean"})` — different stats per column
- `pd.pivot_table()` — Excel-like summaries
- `.value_counts()` — quick frequency counts
- `.nlargest()` / `.nsmallest()` — top/bottom rows

## ➡️ Next Steps

Ready to combine multiple datasets? Head to **[04_merging_and_reshaping.md](./04_merging_and_reshaping.md)**!

## 🔗 Further Reading

- [GroupBy Documentation](https://pandas.pydata.org/docs/user_guide/groupby.html)
- [Aggregation Documentation](https://pandas.pydata.org/docs/user_guide/aggregation.html)
- [Pivot Tables](https://pandas.pydata.org/docs/reference/api/pandas.pivot_table.html)
