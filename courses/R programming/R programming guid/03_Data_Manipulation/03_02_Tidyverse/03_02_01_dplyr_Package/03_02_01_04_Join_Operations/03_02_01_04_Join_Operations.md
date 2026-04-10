# Join Operations in dplyr

## Learning Objectives

- Perform all join types with dplyr
- Use join functions effectively
- Handle duplicate keys
- Work with multiple tables

## Theoretical Background

### Join Types in dplyr

1. **inner_join()** - Matching rows from both
2. **left_join()** - All from left, matching right
3. **right_join()** - All from right, matching left
4. **full_join()** - All rows from both
5. **semi_join()** - Filter left based on right
6. **anti_join()** - Remove matching from left

## Code Examples

### Standard Example: Join Types

```r
library(dplyr)

cat("===== JOIN EXAMPLES =====\n\n")

# Sample tables
customers <- data.frame(
  id = 1:4,
  name = c("Alice", "Bob", "Charlie", "Diana"),
  stringsAsFactors = FALSE
)

orders <- data.frame(
  order_id = c(1001, 1002, 1003, 1004),
  customer_id = c(1, 2, 4, 5),
  amount = c(100, 200, 150, 300),
  stringsAsFactors = FALSE
)

cat("Customers:\n")
print(customers)
cat("\nOrders:\n")
print(orders)

# Inner join
cat("\nInner join:\n")
inner <- inner_join(customers, orders, by = c("id" = "customer_id"))
print(inner)

# Left join
cat("\nLeft join:\n")
left <- left_join(customers, orders, by = c("id" = "customer_id"))
print(left)

# Full join
cat("\nFull join:\n")
full <- full_join(customers, orders, by = c("id" = "customer_id"))
print(full)
```

**Output:**
```
===== JOIN EXAMPLES =====

Customers:
  id     name
1  1    Alice
...
```

### Real-World Example: Multi-Table Joins

```r
# Real-world: Joining customer, order, product tables
cat("===== MULTI-TABLE JOINS =====\n\n")

customers <- data.frame(
  customer_id = 1:4,
  name = c("Alice", "Bob", "Charlie", "Diana"),
  city = c("New York", "Boston", "New York", "Chicago"),
  stringsAsFactors = FALSE
)

products <- data.frame(
  product_id = 1:3,
  name = c("Laptop", "Mouse", "Keyboard"),
  price = c(999, 29, 79),
  category = c("Electronics", "Electronics", "Electronics"),
  stringsAsFactors = FALSE
)

orders <- data.frame(
  order_id = c(1001, 1002, 1003, 1004),
  customer_id = c(1, 2, 1, 3),
  product_id = c(1, 2, 3, 1),
  quantity = c(1, 2, 1, 1),
  stringsAsFactors = FALSE
)

# Join all three tables
cat("Complete order details:\n")
result <- orders %>%
  left_join(customers, by = "customer_id") %>%
  left_join(products, by = "product_id") %>%
  mutate(total = price * quantity) %>%
  select(order_id, name, city, name.y, quantity, total)
colnames(result) <- c("Order ID", "Customer", "City", "Product", "Qty", "Total")
print(result)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Use descriptive suffixes for duplicate columns
2. Check join keys before joining
3. Use semi_join for filtering

### Common Issues

1. Duplicate key issues
2. Column type mismatches
3. NAs in join keys
