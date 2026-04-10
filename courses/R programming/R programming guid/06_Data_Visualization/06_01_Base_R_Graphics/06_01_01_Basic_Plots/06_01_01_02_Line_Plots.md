# Line Plots in R

## Learning Objectives

By the end of this chapter, students will be able to:
- Create basic line plots in R using base graphics and ggplot2
- Add multiple lines and customize line types
- Create time series visualizations
- Add confidence intervals and area fills
- Handle dates and time on the x-axis
- Create connected scatter plots

## Theoretical Background

Line plots connect data points with straight line segments, making them ideal for showing trends over time or continuous data. They are particularly effective for visualizing changes, trajectories, and relationships where order matters.

### When to Use Line Plots

1. **Time Series Data**: When one variable represents time
2. **Sequential Data**: When order matters (not just pairs)
3. **Trends**: Showing increase/decrease over a range
4. **Multiple Series**: Comparing several trends on the same plot

### Components of a Line Plot

1. **Data Points**: Markers showing actual values
2. **Line Segments**: Connecting the points
3. **Axes**: Scaled appropriately for the data
4. **Labels**: Title, axis labels, legend

### Line Plot Variations

1. **Simple Line**: Single line showing one variable
2. **Multi-line**: Multiple lines for different groups
3. **Step Plot**: Lines with right/left angles
4. **Area Plot**: Filled area under the line
5. **Connected Scatter**: Points connected in order of another variable

## Step-by-Step Implementation

### Step 1: Load Required Packages

```r
install.packages("ggplot2")
library(ggplot2)
```

### Step 2: Basic Line Plot with Base R

```r
# Create sample data
x <- 1:10
y <- c(2, 5, 4, 7, 8, 6, 9, 10, 11, 13)

# Basic line plot
plot(x, y, type = "o",
     xlab = "Time Period",
     ylab = "Value",
     main = "Basic Line Plot",
     col = "steelblue",
     pch = 16,
     lwd = 2)
```

### Step 3: Using ggplot2

```r
# Create data frame
df <- data.frame(
  time = 1:10,
  value = c(2, 5, 4, 7, 8, 6, 9, 10, 11, 13)
)

# Simple line plot
ggplot(df, aes(x = time, y = value)) +
  geom_line() +
  ggtitle("Simple Line Plot") +
  theme_minimal()
```

### Step 4: Multiple Lines

```r
# Data with multiple groups
df_multi <- data.frame(
  time = rep(1:10, 3),
  value = c(c(2, 5, 4, 7, 8, 6, 9, 10, 11, 13),
           c(1, 3, 2, 5, 6, 4, 7, 8, 9, 11),
           c(3, 6, 5, 8, 9, 7, 10, 11, 12, 14)),
  group = rep(c("A", "B", "C"), each = 10)
)

# Multi-line plot
ggplot(df_multi, aes(x = time, y = value, color = group)) +
  geom_line() +
  ggtitle("Multiple Line Plot") +
  theme_minimal()
```

### Step 5: Add Points and Confidence Interval

```r
# Using built-in data
data(iris)
iris_summary <- iris %>%
  group_by(Species) %>%
  summarise(
    mean_petal = mean(Petal.Length),
    sd_petal = sd(Petal.Length),
    n = n(),
    se = sd_petal / sqrt(n)
  )

# Line plot with points and error bars
ggplot(iris_summary, aes(x = Species, y = mean_petal, color = Species)) +
  geom_point(size = 3) +
  geom_errorbar(aes(ymin = mean_petal - se, ymax = mean_petal + se), width = 0.2) +
  geom_line(group = 1) +
  ggtitle("Mean Petal Length by Species with SE") +
  theme_minimal()
```

### Step 6: Area Plot

```r
# Area plot
df_area <- data.frame(
  x = 1:10,
  y = c(2, 5, 4, 7, 8, 6, 9, 10, 11, 13)
)

ggplot(df_area, aes(x = x, y = y)) +
  geom_area(fill = "steelblue", alpha = 0.5) +
  geom_line() +
  ggtitle("Area Plot") +
  theme_minimal()
```

## Code Examples

### Example 1: Time Series Analysis

This example shows stock price visualization.

```r
# Create stock data
set.seed(123)
n <- 100

stock_data <- data.frame(
  date = seq(as.Date("2023-01-01"), by = "day", length.out = n),
  price = cumsum(rnorm(n, 0.5, 2)) + 100,
  volume = rpois(n, 1000000)
)

# Time series line plot
ggplot(stock_data, aes(x = date, y = price)) +
  geom_line(color = "steelblue", size = 1) +
  geom_point(size = 1, color = "steelblue", alpha = 0.5) +
  ggtitle("Stock Price Over Time") +
  xlab("Date") +
  ylab("Price ($)") +
  theme_minimal() +
  theme(axis.text.x = element_text(angle = 45))
```

### Example 2: Growth Curves

This example demonstrates growth curve visualization.

```r
# Create growth data
set.seed(456)
n <- 50

growth_data <- data.frame(
  day = 1:n,
  control = 10 + 0.3 * (1:n) + rnorm(n, 0, 1),
  treatment = 10 + 0.5 * (1:n) + rnorm(n, 0, 1)
)

# Reshape to long format
library(tidyr)
growth_long <- growth_data %>%
  gather(key = "group", value = "value", -day)

# Plot
ggplot(growth_long, aes(x = day, y = value, color = group)) +
  geom_line() +
  geom_smooth(method = "lm", se = FALSE) +
  ggtitle("Growth Curves: Control vs Treatment") +
  xlab("Days") +
  ylab("Measurement") +
  theme_minimal()
```

### Example 3: Connected Scatter Plot

Shows relationship between two variables as they evolve over time.

```r
# Create data showing relationship changes
set.seed(789)
n <- 30

connected_data <- data.frame(
  x = cumsum(rnorm(n, 0, 2)),
  y = cumsum(rnorm(n, 0, 2)),
  time = 1:n
)

# Connected scatter
ggplot(connected_data, aes(x = x, y = y)) +
  geom_point(aes(color = time), size = 3) +
  geom_path(color = "gray50") +
  scale_color_gradient(low = "lightblue", high = "darkblue") +
  ggtitle("Connected Scatter Plot") +
  theme_minimal()
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Order Data**: Sort by x-axis variable before plotting
2. **Label Clearly**: Use clear axis labels with units
3. **Consider Scale**: Log scales for data spanning large ranges
4. **Add Points**: Mark actual data points for precision
5. **Use Legends**: When plotting multiple lines

### Common Pitfalls

1. **Unsorted Data**: Lines will zigzag incorrectly
2. **Missing Values**: Gaps in lines where data is missing
3. **Too Many Lines**: Makes plot unreadable
4. **Ignoring Scale**: Different scales need different plots

## Performance Considerations

### When to Use Line Plots

| Situation | Recommendation |
|-----------|---------------|
| Time series | Line plot with dates |
| Sequential | Connected points or lines |
| Comparison | Multiple lines with legend |
| Distribution | Consider box plots |

## Related Concepts

- **Area Charts**: Filled version of line plots
- **Step Plots**: For categorical progression
- **Time Series**: Specialized line plots for temporal data
- **Ridge Plots**: For multiple time series

## Exercise Problems

1. **Basic**: Create a line plot from any dataset.

2. **Intermediate**: Add multiple lines with different groups.

3. **Advanced**: Add confidence intervals to a line plot.

4. **Real-World Challenge**: Visualize time series data.

5. **Extension**: Create a connected scatter plot.