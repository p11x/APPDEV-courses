# Pie Charts in R

## Learning Objectives

By the end of this chapter, students will be able to:
- Create pie charts in R using base graphics and ggplot2
- Add labels and percentages
- Create 3D pie charts for visual effect
- Handle exploded slices for emphasis
- Understand when pie charts are and aren't appropriate
- Compare alternatives to pie charts

## Theoretical Background

Pie charts display data as circular slices, with each slice representing a proportion of the whole. They are commonly used to show how a total is divided into categories. However, their use is somewhat controversial in data visualization, as bar charts often communicate proportions more effectively.

### When to Use Pie Charts

1. **Part-to-Whole**: Showing composition of a single total
2. **Few Categories**: 2-5 slices work best
3. **Simple Proportions**: When comparing portions is easy
4. **Emphasis on Unity**: Showing all parts form a whole

### When NOT to Use Pie Charts

1. **Many Categories**: 6+ slices become hard to read
2. **Similar Values**: Hard to distinguish similar proportions
3. **Precise Comparisons**: Bar charts show differences better
4. **Time Series**: Pie charts can't show change over time

### Pie Chart Components

- **Slice**: Represents one category's proportion
- **Arc Length**: Proportional to category value
- **Labels**: Category names or percentages
- **Legend**: Explains colors
- **Exploded Slice**: Slice pulled out for emphasis

## Step-by-Step Implementation

### Step 1: Load Required Packages

```r
install.packages("ggplot2")
install.packages("plotrix")

library(ggplot2)
library(plotrix)
```

### Step 2: Basic Pie Chart

```r
# Using base R
slices <- c(10, 20, 30, 15, 25)
labels <- c("A", "B", "C", "D", "E")

pie(slices, labels = labels,
    main = "Basic Pie Chart",
    col = rainbow(length(slices)))
```

### Step 3: Pie Chart with Percentages

```r
# Calculate percentages
percentages <- round(slices / sum(slices) * 100, 1)
labels_pct <- paste(labels, percentages, "%")

pie(slices, labels = labels_pct,
    main = "Pie Chart with Percentages",
    col = rainbow(length(slices)))
```

### Step 4: Using ggplot2

```r
# Create data frame
df_pie <- data.frame(
  category = c("A", "B", "C", "D", "E"),
  value = c(10, 20, 30, 15, 25)
)

# ggplot2 requires computing positions
ggplot(df_pie, aes(x = "", y = value, fill = category)) +
  geom_bar(stat = "identity", width = 1) +
  coord_polar("y", start = 0) +
  ggtitle("Pie Chart with ggplot2") +
  theme_void()
```

### Step 5: 3D Pie Chart

```r
# Using plotrix
pie3D(slices, labels = labels,
      main = "3D Pie Chart",
      explode = 0.1,
      col = rainbow(length(slices)))
```

### Step 6: Donut Chart

```r
# Donut chart (pie with hole in center)
ggplot(df_pie, aes(x = 2, y = value, fill = category)) +
  geom_bar(stat = "identity", width = 1) +
  xlim(0.5, 2.5) +
  coord_polar("y") +
  theme_void() +
  ggtitle("Donut Chart") +
  theme(
    plot.title = element_text(hjust = 0.5),
    legend.position = "right"
  )
```

## Code Examples

### Example 1: Survey Response Distribution

This example shows survey results as pie chart.

```r
# Create survey data
set.seed(123)
survey <- data.frame(
  response = c("Very Satisfied", "Satisfied", "Neutral", "Dissatisfied", "Very Dissatisfied"),
  count = c(45, 30, 15, 7, 3)
)

# Create pie with percentages
survey$pct <- round(survey$count / sum(survey$count) * 100, 1)
survey$label <- paste(survey$response, "\n", survey$pct, "%")

# Plot
pie(survey$count, labels = survey$label,
    main = "Customer Satisfaction Survey",
    col = c("darkgreen", "green", "yellow", "orange", "red"))
```

### Example 2: Budget Breakdown

This example shows budget allocation.

```r
# Budget data
budget <- data.frame(
  category = c("Personnel", "Equipment", "Marketing", "Operations", "R&D"),
  amount = c(40, 20, 15, 15, 10)
)

# Calculate percentages
budget$pct <- budget$amount / sum(budget$amount) * 100

# Color palette
colors <- c("#4CAF50", "#2196F3", "#FF9800", "#9C27B0", "#F44336")

# Create pie
pie(budget$amount, labels = paste(budget$category, "\n", budget$pct, "%"),
    main = "Annual Budget Allocation",
    col = colors,
    clockwise = TRUE)
```

### Example 3: Market Share

This example shows market share visualization.

```r
# Market share data
market <- data.frame(
  company = c("Company A", "Company B", "Company C", "Others"),
  share = c(35, 25, 20, 20)
)

# Add percentages
market$label <- paste(market$company, "\n", market$share, "%")

# Create pie with legend
pie(market$share, labels = NA,
    main = "Market Share",
    col = c("steelblue", "lightblue", "lightgreen", "gray"))

# Add legend
legend("topright", legend = market$label, 
       fill = c("steelblue", "lightblue", "lightgreen", "gray"),
       cex = 0.8)
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Limit Slices**: 2-5 slices maximum
2. **Order Slices**: Clockwise from largest
3. **Use Labels**: Include percentages or values
4. **Color Wisely**: Use distinct colors
5. **Consider Donut**: Shows percentages in center

### Common Pitfalls

1. **Too Many Slices**: Hard to read
2. **Similar Values**: Can't distinguish proportions
3. **3D Effects**: Distort perception of size
4. **No Labels**: Requires legend lookup
5. **Always Pie**: Bar charts often better

## Performance Considerations

### When to Use Pie Charts

| Situation | Use Pie? | Alternative |
|-----------|----------|-------------|
| 2-3 categories | Yes | Bar chart |
| 4-5 categories | Maybe | Bar chart |
| 6+ categories | No | Bar chart |
| Part-to-whole | Yes | Stacked bar |

### Pie vs Bar Chart

- **Pie**: Shows "part of whole" concept
- **Bar**: Shows exact differences better
- **General rule**: Use bar charts unless "part of whole" is critical

## Related Concepts

- **Donut Charts**: Pie with center cut out
- **Bar Charts**: Often better for comparisons
- **Stacked Bar**: Shows composition
- **Treemap**: For hierarchical data

## Exercise Problems

1. **Basic**: Create a pie chart with percentages.

2. **Intermediate**: Add labels and legend.

3. **Advanced**: Create a donut chart.

4. **Real-World Challenge**: Visualize budget data.

5. **Extension**: Compare pie chart with equivalent bar chart.