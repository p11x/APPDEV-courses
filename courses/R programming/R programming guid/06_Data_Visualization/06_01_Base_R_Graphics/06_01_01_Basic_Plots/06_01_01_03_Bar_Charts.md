# Bar Charts in R

## Learning Objectives

By the end of this chapter, students will be able to:
- Create basic bar charts in R using base graphics and ggplot2
- Distinguish between grouped and stacked bar charts
- Customize bar colors, labels, and orientations
- Handle error bars for showing uncertainty
- Create diverging bar charts for showing differences
- Interpret categorical data comparisons

## Theoretical Background

Bar charts represent categorical data with rectangular bars whose lengths are proportional to the values they represent. They are excellent for comparing discrete categories, showing frequencies, and displaying summary statistics.

### When to Use Bar Charts

1. **Comparing Categories**: Comparing values across different groups
2. **Showing Counts**: Displaying frequency distributions
3. **Summarizing Data**: Presenting aggregated statistics
4. **Proportions**: Showing parts of a whole

### Types of Bar Charts

1. **Vertical Bar Chart**: Bars extend upward from x-axis
2. **Horizontal Bar Chart**: Bars extend horizontally
3. **Grouped Bar Chart**: Bars grouped by category
4. **Stacked Bar Chart**: Bars stacked on top of each other
5. **Diverging Bar Chart**: Bars extending both left and right

### Key Considerations

- **Bar Width**: Usually uniform but can vary for emphasis
- **Bar Spacing**: Can be touching (histogram) or separated
- **Order**: Alphabetical, by value, or custom order
- **Zero Baseline**: Bar charts should always start at zero

## Step-by-Step Implementation

### Step 1: Load Required Packages

```r
install.packages("ggplot2")
library(ggplot2)
```

### Step 2: Basic Bar Chart

```r
# Using base R
counts <- table(mtcars$cyl)
barplot(counts,
        xlab = "Number of Cylinders",
        ylab = "Count",
        main = "Car Distribution by Cylinders",
        col = "steelblue")
```

### Step 3: Bar Chart with ggplot2

```r
# Using ggplot2
df_bar <- as.data.frame(table(mtcars$cyl))
colnames(df_bar) <- c("cylinders", "count")

ggplot(df_bar, aes(x = cylinders, y = count, fill = cylinders)) +
  geom_bar(stat = "identity") +
  ggtitle("Car Distribution by Cylinders") +
  theme_minimal()
```

### Step 4: Grouped Bar Chart

```r
# Grouped bar chart
df_grouped <- mtcars %>%
  group_by(cyl, am) %>%
  summarise(mean_mpg = mean(mpg))

ggplot(df_grouped, aes(x = factor(cyl), y = mean_mpg, fill = factor(am))) +
  geom_bar(stat = "identity", position = "dodge") +
  ggtitle("MPG by Cylinders and Transmission") +
  xlab("Cylinders") +
  ylab("Mean MPG") +
  scale_fill_discrete(name = "Transmission", labels = c("Auto", "Manual")) +
  theme_minimal()
```

### Step 5: Stacked Bar Chart

```r
# Stacked bar chart
ggplot(df_grouped, aes(x = factor(cyl), y = mean_mpg, fill = factor(am))) +
  geom_bar(stat = "identity") +
  ggtitle("MPG by Cylinders (Stacked)") +
  scale_fill_discrete(name = "Transmission") +
  theme_minimal()
```

### Step 6: Horizontal Bar Chart

```r
# Horizontal bar chart
df_sorted <- df_bar[order(df_bar$count, decreasing = TRUE), ]

ggplot(df_sorted, aes(x = reorder(cylinders, -count), y = count, fill = cylinders)) +
  geom_bar(stat = "identity") +
  coord_flip() +
  ggtitle("Car Distribution (Horizontal)") +
  xlab("Number of Cylinders") +
  theme_minimal()
```

## Code Examples

### Example 1: Sales Data Visualization

This example shows sales by product category.

```r
# Create sales data
set.seed(123)
sales_data <- data.frame(
  product = c("Electronics", "Clothing", "Food", "Home", "Sports", "Books"),
  revenue = c(15000, 8000, 12000, 6000, 5000, 4000),
  profit = c(3000, 2000, 1500, 1000, 800, 600)
)

# Simple bar chart
ggplot(sales_data, aes(x = product, y = revenue, fill = product)) +
  geom_bar(stat = "identity") +
  ggtitle("Revenue by Product Category") +
  theme_minimal() +
  theme(axis.text.x = element_text(angle = 45, hjust = 1))

# Stacked with profit
ggplot(sales_data, aes(x = product, y = revenue, fill = "Revenue")) +
  geom_bar(stat = "identity", alpha = 0.7) +
  geom_bar(aes(x = product, y = profit * 5, fill = "Profit (x5)"), stat = "identity") +
  ggtitle("Revenue and Profit by Product") +
  theme_minimal()
```

### Example 2: Survey Results

This example shows survey response visualization.

```r
# Create survey data
set.seed(456)
survey_data <- data.frame(
  question = c("Q1: Ease of Use", "Q2: Features", "Q3: Support", 
               "Q4: Price", "Q5: Overall"),
  excellent = c(45, 35, 30, 25, 40),
  good = c(35, 40, 35, 30, 35),
  fair = c(15, 20, 25, 30, 20),
  poor = c(5, 5, 10, 15, 5)
)

# Reshape to long format
library(tidyr)
survey_long <- survey_data %>%
  gather(key = "rating", value = "count", -question)

# Stacked bar chart
ggplot(survey_long, aes(x = question, y = count, fill = rating)) +
  geom_bar(stat = "identity", position = "fill") +
  ggtitle("Survey Results") +
  scale_fill_brewer(palette = "RdYlGn", direction = -1) +
  theme_minimal() +
  theme(axis.text.x = element_text(angle = 45, hjust = 1))
```

### Example 3: Error Bars on Bar Charts

This example shows adding error bars for uncertainty.

```r
# Create summary statistics with error bars
summary_data <- iris %>%
  group_by(Species) %>%
  summarise(
    mean_length = mean(Petal.Length),
    sd_length = sd(Petal.Length),
    n = n(),
    se = sd_length / sqrt(n)
  )

# Bar chart with error bars
ggplot(summary_data, aes(x = Species, y = mean_length, fill = Species)) +
  geom_bar(stat = "identity", alpha = 0.7) +
  geom_errorbar(aes(ymin = mean_length - se, ymax = mean_length + se),
                width = 0.3, color = "black") +
  gggtitle("Mean Petal Length with Standard Error") +
  theme_minimal()
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Start at Zero**: Bar charts must have zero baseline
2. **Order Categories**: By value or meaningful order
3. **Label Clearly**: Include axis labels and title
4. **Use Color Effectively**: Group similar categories
5. **Consider Width**: Wider bars for more important categories

### Common Pitfalls

1. **3D Effects**: Avoid 3D that distorts perception
2. **Truncated Bars**: Never truncate bar chart axes
3. **Too Many Categories**: Limit to 5-7 categories per chart
4. **Inconsistent Ordering**: Confuses comparison
5. **Missing Legend**: Needed for grouped/stacked charts

## Performance Considerations

### When to Use Bar Charts

| Data Type | Chart Type |
|-----------|-----------|
| Single categorical | Simple bar |
| Multiple groups | Grouped/stacked |
| Proportions | 100% stacked |
| With uncertainty | Bar with error bars |

## Related Concepts

- **Histograms**: For continuous data distributions
- **Pie Charts**: For proportions (but bar charts are clearer)
- **Box Plots**: For showing distributions
- **Dot Charts**: Alternative to bar charts

## Exercise Problems

1. **Basic**: Create a bar chart of any categorical variable.

2. **Intermediate**: Create a grouped bar chart.

3. **Advanced**: Add error bars to a bar chart.

4. **Real-World Challenge**: Visualize survey results.

5. **Extension**: Create a diverging bar chart.