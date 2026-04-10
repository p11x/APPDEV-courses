# Data Mapping with ggplot2

## Learning Objectives

- Master aesthetic mappings
- Map data to visual properties
- Use color, size, shape, linetype
- Handle group mappings

## Code Examples

### Standard Example: Aesthetic Mappings

```r
library(ggplot2)

cat("===== AESTHETIC MAPPINGS =====\n\n")

# Data with groups
df <- data.frame(
  x = 1:20,
  y = c(1:10, 10:1),
  group = rep(c("A", "B"), each = 10),
  size = rep(1:5, each = 4)
)

# Basic mapping
cat("Basic scatter:\n")
p1 <- ggplot(df, aes(x = x, y = y)) + geom_point()
print(p1)

# Map color to group
cat("\nColor by group:\n")
p2 <- ggplot(df, aes(x = x, y = y, color = group)) + geom_point()
print(p2)

# Multiple aesthetics
cat("\nMultiple aesthetics (color, size):\n")
p3 <- ggplot(df, aes(x = x, y = y, color = group, size = size)) + 
  geom_point()
print(p3)
```

### Real-World Example: Customer Segmentation

```r
# Real-world: Customer data visualization
cat("===== CUSTOMER SEGMENTATION =====\n\n")

customers <- data.frame(
  age = runif(100, 20, 70),
  income = runif(100, 30000, 150000),
  segment = sample(c("Premium", "Standard", "Basic"), 100, replace = TRUE),
  purchases = rpois(100, 5)
)

# Visualize segments
p <- ggplot(customers, aes(x = age, y = income, 
                            color = segment, size = purchases)) +
  geom_point(alpha = 0.6) +
  labs(
    title = "Customer Segmentation Analysis",
    x = "Age (years)",
    y = "Income ($)",
    color = "Segment",
    size = "Purchases"
  ) +
  theme_minimal()

print(p)
```

## Best Practices

1. Put aes() in ggplot() for global mappings
2. Put aes() in geom_*() for specific mappings
3. Use alpha for overlapping points
