# Merging and Joining in R

## Learning Objectives

- Merge data frames in R
- Perform different types of joins
- Handle duplicate keys
- Combine data from multiple sources

## Theoretical Background

### Merging Data

Merging combines data frames based on common columns:

1. **cbind()** - Column binding (side-by-side)
2. **rbind()** - Row binding (stacking)
3. **merge()** - SQL-style joins

### Join Types

- **Inner join**: Only matching rows
- **Outer join**: All rows from one or both
- **Left join**: All from first table
- **Right join**: All from second table

## Code Examples

### Standard Example: merge() Function

```r
# ===== MERGING DATA FRAMES =====

cat("===== MERGE EXAMPLES =====\n\n")

# Sample data frames
df1 <- data.frame(
  id = 1:4,
  name = c("Alice", "Bob", "Charlie", "Diana"),
  stringsAsFactors = FALSE
)

df2 <- data.frame(
  id = 3:6,
  salary = c(50000, 60000, 55000, 70000),
  stringsAsFactors = FALSE
)

cat("df1:\n")
print(df1)
cat("\ndf2:\n")
print(df2)

# Inner join - only matching IDs
cat("\nInner join (all = FALSE):\n")
inner <- merge(df1, df2, by = "id", all = FALSE)
print(inner)

# Outer join - all rows
cat("\nOuter join (all = TRUE):\n")
outer <- merge(df1, df2, by = "id", all = TRUE)
print(outer)

# Left join - all from df1
cat("\nLeft join:\n")
left <- merge(df1, df2, by = "id", all.x = TRUE)
print(left)

# Right join - all from df2
cat("\nRight join:\n")
right <- merge(df1, df2, by = "id", all.y = TRUE)
print(right)
```

**Output:**
```
===== MERGE EXAMPLES =====

df1:
  id   name
1  1  Alice
...
```

### Real-World Example: Customer Order Data

```r
# Real-world: Joining customer and order data
cat("===== CUSTOMER-ORDER JOINS =====\n\n")

customers <- data.frame(
  customer_id = c(101, 102, 103, 104, 105),
  name = c("Alice", "Bob", "Charlie", "Diana", "Edward"),
  city = c("New York", "Boston", "New York", "Chicago", "Boston"),
  stringsAsFactors = FALSE
)

orders <- data.frame(
  order_id = c(1001, 1002, 1003, 1004, 1005),
  customer_id = c(101, 102, 101, 103, 101),
  product = c("Laptop", "Mouse", "Keyboard", "Monitor", "Webcam"),
  amount = c(999, 29, 79, 349, 89),
  stringsAsFactors = FALSE
)

cat("Customers:\n")
print(customers)
cat("\nOrders:\n")
print(orders)

# 1. Inner join - get orders with customer info
cat("\n1. Inner join (customers with orders):\n")
order_details <- merge(orders, customers, by = "customer_id", all = FALSE)
print(order_details)

# 2. Left join - all customers with their orders
cat("\n2. Left join (all customers with orders):\n")
customer_orders <- merge(customers, orders, by = "customer_id", all.x = TRUE)
print(customer_orders)

# 3. Calculate total by customer
cat("\n3. Customer order totals:\n")
customer_totals <- aggregate(amount ~ customer_id, data = orders, FUN = sum)
customer_totals <- merge(customers[, c("customer_id", "name")], 
                         customer_totals, by = "customer_id", all.x = TRUE)
print(customer_totals)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Specify by.x and by.y for different column names
2. Use suffixes for duplicate column names
3. Check for duplicates before joining
4. Use all.x and all.y for join types

### Common Pitfalls

1. Mismatched column types
2. Duplicate keys causing cartesian products
3. Not handling NAs in keys
