# Geoms and Stats in ggplot2

## Learning Objectives

- Use different geometric objects (geoms)
- Understand statistical transformations (stats)
- Combine geoms and stats
- Create various plot types

## Code Examples

### Standard Example: Different Geoms

```r
library(ggplot2)

cat("===== DIFFERENT GEOMS =====\n\n")

df <- data.frame(
  x = 1:10,
  y = c(2, 4, 3, 5, 6, 8, 7, 9, 10, 11)
)

cat("1. geom_point() - scatter plot:\n")
p1 <- ggplot(df, aes(x, y)) + geom_point()
print(p1)

cat("\n2. geom_line() - line plot:\n")
p2 <- ggplot(df, aes(x, y)) + geom_line()
print(p2)

cat("\n3. geom_bar() - bar chart:\n")
df_bar <- data.frame(category = c("A", "B", "C", "D"),
                     value = c(10, 20, 15, 25))
p3 <- ggplot(df_bar, aes(x = category, y = value)) + 
  geom_bar(stat = "identity")
print(p3)

cat("\n4. geom_histogram() - histogram:\n")
p4 <- ggplot(data.frame(x = rnorm(1000)), aes(x)) + geom_histogram()
print(p4)
```

### Real-World Example: Sales Analysis

```r
# Real-world: Sales data with different geoms
cat("===== SALES VISUALIZATION =====\n\n")

sales <- data.frame(
  product = rep(c("A", "B", "C"), each = 100),
  price = runif(300, 10, 100),
  rating = runif(300, 1, 5)
)

# Box plot
cat("Box plot by product:\n")
p1 <- ggplot(sales, aes(x = product, y = price)) +
  geom_boxplot() +
  theme_minimal()
print(p1)

# Violin plot
cat("\nViolin plot:\n")
p2 <- ggplot(sales, aes(x = product, y = price)) +
  geom_violin() +
  theme_minimal()
print(p2)

# Using stat_summary
cat("\nWith stat_summary (mean with error bars):\n")
p3 <- ggplot(sales, aes(x = product, y = price)) +
  stat_summary(fun = mean, geom = "point", size = 3, color = "red") +
  stat_summary(fun.data = mean_se, geom = "errorbar", width = 0.3) +
  theme_minimal()
print(p3)
```

## Best Practices

1. Choose geom based on data type
2. Use stat_* for computed values
3. Remember stat = "identity" for raw values
