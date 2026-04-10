# Facets and Themes in ggplot2

## Learning Objectives

- Use facets to create subplots
- Apply facet_wrap and facet_grid
- Customize themes
- Create publication-ready plots

## Code Examples

### Standard Example: Faceting

```r
library(ggplot2)

cat("===== FACETS IN GGPLOT2 =====\n\n")

# Sample data with categories
df <- data.frame(
  x = 1:30,
  y = 1:30 + rnorm(30),
  group = rep(c("A", "B", "C"), each = 10),
  category = rep(c("X", "Y"), 15)
)

# facet_wrap - single variable
cat("1. facet_wrap by group:\n")
p1 <- ggplot(df, aes(x, y)) + 
  geom_point() + 
  facet_wrap(~group)
print(p1)

# facet_grid - two variables
cat("\n2. facet_grid by group and category:\n")
p2 <- ggplot(df, aes(x, y)) + 
  geom_point() + 
  facet_grid(group ~ category)
print(p2)

# Free scales
cat("\n3. facet_wrap with free scales:\n")
p3 <- ggplot(df, aes(x, y)) + 
  geom_point() + 
  facet_wrap(~group, scales = "free")
print(p3)
```

### Real-World Example: Multi-Panel Dashboard

```r
# Real-world: Sales dashboard with facets
cat("===== SALES DASHBOARD =====\n\n")

sales <- data.frame(
  region = rep(c("North", "South", "East", "West"), each = 12),
  month = rep(1:12, 4),
  sales = runif(48, 50, 150),
  profit = runif(48, 10, 50)
)

# Create dashboard with facets
p <- ggplot(sales, aes(x = month, y = sales, fill = region)) +
  geom_bar(stat = "identity") +
  facet_wrap(~region, ncol = 2) +
  labs(
    title = "Regional Sales by Month",
    x = "Month",
    y = "Sales ($)"
  ) +
  theme_bw() +
  theme(legend.position = "none")

print(p)
```

### Themes

```r
cat("===== THEMES =====\n\n")

# Different built-in themes
p <- ggplot(mtcars, aes(mpg, wt)) + geom_point()

cat("theme_bw():\n")
p1 <- p + theme_bw()

cat("theme_minimal():\n")
p2 <- p + theme_minimal()

cat("theme_classic():\n")
p3 <- p + theme_classic()
```

## Best Practices

1. Use facets for comparing categories
2. Use facet_grid for two variables
3. Choose appropriate theme for context
