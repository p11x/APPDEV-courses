# Grammar of Graphics with ggplot2

## Learning Objectives

- Understand the grammar of graphics
- Build ggplot visualizations layer by layer
- Use aes() for aesthetic mappings
- Understand geoms and stats

## Theoretical Background

### Grammar of Graphics

The grammar of graphics (by Leland Wilkinson) is implemented in ggplot2:

1. **Data**: The data being plotted
2. **Aesthetics (aes)**: Visual properties mapped to data
3. **Geometries (geom)**: The geometric objects drawn
4. **Facets**: Splitting into subplots
5. **Themes**: Visual styling

### ggplot2 Structure

```
ggplot(data) +
  aes(mappings) +
  geom_*() +
  facet_*() +
  theme_*()
```

## Code Examples

### Standard Example: Basic ggplot2

```r
library(ggplot2)

cat("===== BASIC GGPLOT2 =====\n\n")

# Create sample data
df <- data.frame(
  x = 1:10,
  y = c(2, 4, 3, 5, 6, 8, 7, 9, 10, 11)
)

# Simple scatter plot
p <- ggplot(df, aes(x = x, y = y)) +
  geom_point() +
  labs(title = "Simple Scatter Plot", 
       x = "X Axis", y = "Y Axis") +
  theme_minimal()

cat("Plot created successfully\n")

# Using different geoms
cat("\nLine plot:\n")
p2 <- ggplot(df, aes(x = x, y = y)) +
  geom_line() +
  theme_minimal()

cat("Line plot created\n")

cat("\nCombined point and line:\n")
p3 <- ggplot(df, aes(x = x, y = y)) +
  geom_line() +
  geom_point() +
  theme_minimal()
```

**Output:**
```
===== BASIC GGPLOT2 =====

Plot created successfully
```

### Real-World Example: Sales Visualization

```r
# Real-world: Sales data visualization
cat("===== SALES VISUALIZATION =====\n\n")

# Sample sales data
sales <- data.frame(
  month = 1:12,
  sales = c(100, 120, 110, 150, 140, 160, 180, 170, 190, 200, 220, 250),
  target = rep(150, 12)
)

# Create visualization
p <- ggplot(sales, aes(x = month)) +
  # Target line
  geom_line(aes(y = target), linetype = "dashed", color = "red") +
  # Sales line
  geom_line(aes(y = sales), color = "blue", size = 1.5) +
  # Points
  geom_point(aes(y = sales), color = "blue", size = 3) +
  # Labels
  labs(
    title = "Monthly Sales vs Target",
    x = "Month",
    y = "Sales ($)",
    color = "Legend"
  ) +
  # Theme
  theme_minimal() +
  # X axis labels
  scale_x_continuous(breaks = 1:12,
                     labels = month.abb)

print(p)
```

## Best Practices and Common Pitfalls

### Best Practices

1. Start with ggplot() and add layers
2. Use aes() for data-driven visual properties
3. Use labs() for labels
4. Choose appropriate geoms

### Common Issues

1. Forgetting aes() inside ggplot vs geom
2. Data not in long format
3. Overlapping plot elements
