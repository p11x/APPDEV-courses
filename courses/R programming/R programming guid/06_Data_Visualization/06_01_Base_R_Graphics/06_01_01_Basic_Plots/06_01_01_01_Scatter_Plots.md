# Scatter Plots in R

## Learning Objectives

By the end of this chapter, students will be able to:
- Create basic scatter plots in R using both base graphics and ggplot2
- Customize scatter plots with colors, sizes, and shapes
- Add regression lines and trend smoothing
- Handle overplotting with transparency and jittering
- Create bubble charts for three-variable visualizations
- Interpret relationships between variables from scatter plots

## Theoretical Background

A scatter plot displays the relationship between two numerical variables. Each point represents an observation, with its position on the x and y axes showing values for two variables. Scatter plots are fundamental tools for exploring relationships, identifying patterns, and detecting outliers in data.

### When to Use Scatter Plots

1. **Examining Correlations**: Visualize linear or non-linear relationships
2. **Identifying Clusters**: Detect natural groupings in data
3. **Outlier Detection**: Find unusual observations
4. **Comparing Variables**: Evaluate which variables are related

### Types of Scatter Plot Variations

1. **Simple Scatter**: Two variables plotted against each other
2. **Colored Scatter**: Points colored by a third categorical variable
3. **Size Scatter (Bubble Chart)**: Point size represents a third variable
4. **Matrix Scatter**: Multiple scatter plots in a grid

### Key Patterns to Look For

- **Positive Correlation**: Points trend upward (y increases as x increases)
- **Negative Correlation**: Points trend downward (y decreases as x increases)
- **No Correlation**: Random scatter, no clear pattern
- **Non-linear**: Curved relationship between variables
- **Clusters**: Distinct groups in the data

## Step-by-Step Implementation

### Step 1: Load Required Packages

```r
# Base R is sufficient, but we'll also use ggplot2
install.packages("ggplot2")
install.packages("dplyr")

library(ggplot2)
library(dplyr)
```

### Step 2: Create Basic Scatter Plot

```r
# Using the iris dataset
data(iris)

# Base R scatter plot
plot(iris$Sepal.Length, iris$Sepal.Width,
     xlab = "Sepal Length (cm)",
     ylab = "Sepal Width (cm)",
     main = "Sepal Length vs Sepal Width in Iris",
     pch = 19,
     col = "steelblue")
```

### Step 3: Using ggplot2

```r
# Basic scatter with ggplot2
ggplot(iris, aes(x = Sepal.Length, y = Sepal.Width)) +
  geom_point() +
  ggtitle("Sepal Dimensions in Iris") +
  xlab("Sepal Length (cm)") +
  ylab("Sepal Width (cm)")
```

### Step 4: Color by Species

```r
# Color points by categorical variable
ggplot(iris, aes(x = Sepal.Length, y = Sepal.Width, color = Species)) +
  geom_point(size = 3) +
  ggtitle("Sepal Dimensions by Species") +
  theme_minimal()
```

### Step 5: Add Regression Line

```r
# Add trend line with confidence interval
ggplot(iris, aes(x = Sepal.Length, y = Sepal.Width)) +
  geom_point(aes(color = Species), size = 2) +
  geom_smooth(method = "lm", se = TRUE) +
  ggtitle("Sepal Dimensions with Regression Line") +
  theme_minimal()
```

### Step 6: Handle Overplotting

```r
# Create sample data with many points
set.seed(42)
n <- 1000
overplot_data <- data.frame(
  x = rnorm(n, mean = 50, sd = 15),
  y = rnorm(n, mean = 50, sd = 15)
)

# Without any adjustment - points overlap heavily
p1 <- ggplot(overplot_data, aes(x, y)) + geom_point()

# With alpha (transparency) to show density
p2 <- ggplot(overplot_data, aes(x, y)) + geom_point(alpha = 0.3)

# With jitter for discrete data
df_discrete <- data.frame(
  x = sample(1:5, 500, replace = TRUE),
  y = sample(1:5, 500, replace = TRUE)
)
p3 <- ggplot(df_discrete, aes(x, y)) + geom_jitter(width = 0.2, height = 0.2)

# 2D density for continuous
p4 <- ggplot(overplot_data, aes(x, y)) + 
  geom_bin2d(bins = 30) +
  scale_fill_gradient(low = "lightblue", high = "darkblue")

library(gridExtra)
gridExtra::grid.arrange(p1, p2, p3, p4, ncol = 2)
```

## Code Examples

### Example 1: Marketing Campaign Analysis

This example shows analyzing marketing campaign effectiveness.

```r
# Create marketing campaign data
set.seed(123)
n <- 200

marketing <- data.frame(
  spend = runif(n, 1000, 50000),
  impressions = runif(n, 10000, 500000),
  conversions = rpois(n, 5),
  campaign = factor(sample(c("A", "B", "C"), n, replace = TRUE))
)

# Add relationships
marketing$conversions <- as.integer(marketing$spend / 5000 + 
                                    marketing$impressions / 100000 * 3 +
                                    rpois(n, 2))

# Scatter plot: spend vs conversions by campaign
ggplot(marketing, aes(x = spend, y = conversions, color = campaign)) +
  geom_point(size = 3, alpha = 0.7) +
  geom_smooth(method = "lm", se = FALSE) +
  ggtitle("Marketing Campaign Performance") +
  xlab("Campaign Spend ($)") +
  ylab("Number of Conversions") +
  scale_color_brewer(palette = "Set1") +
  theme_minimal()
```

### Example 2: Bubble Chart

Bubble charts add a third dimension through point size.

```r
# Create GDP and life expectancy data
set.seed(456)
n <- 50

countries <- data.frame(
  country = paste0("Country_", 1:n),
  gdp_per_capita = runif(n, 5000, 60000),
  life_expectancy = runif(n, 60, 85),
  population = runif(n, 1, 1000) * 1000000
)

# Bubble chart
ggplot(countries, aes(x = gdp_per_capita, y = life_expectancy, 
                      size = population, fill = life_expectancy)) +
  geom_point(alpha = 0.7, shape = 21) +
  scale_size_continuous(name = "Population (M)", 
                        range = c(5, 25)) +
  scale_fill_gradient(low = "lightblue", high = "darkblue") +
  ggtitle("GDP vs Life Expectancy") +
  xlab("GDP per Capita ($)") +
  ylab("Life Expectancy (years)") +
  theme_minimal()
```

### Example 3: Scatter Plot Matrix

Creating multiple scatter plots for exploring multiple variables.

```r
# Using mtcars dataset
data(mtcars)

# Select key variables
vars <- c("mpg", "disp", "hp", "wt")
mtcars_subset <- mtcars[, vars]

# Base R pairs plot
pairs(mtcars_subset, 
      main = "Scatter Plot Matrix",
      pch = 19,
      col = "steelblue",
      cex = 0.8)

# Using ggplot2 - pair plot with ggpairs from GGally
library(GGally)
ggpairs(mtcars_subset, 
        title = "Pairwise Relationships in mtcars")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Include Labels**: Always label axes with units
2. **Consider Data Size**: Large datasets may need smoothing or sampling
3. **Use Color Effectively**: Add meaningful dimensions, not just decoration
4. **Add Trend Lines**: Help identify patterns
5. **Handle Overlap**: Use transparency or jittering

### Common Pitfalls

1. **Misleading Axes**: Not starting at zero can exaggerate differences
2. **Overplotting**: Too many points hide the pattern
3. **Ignoring Outliers**: May indicate data quality issues
4. **Poor Color Choices**: Use colorblind-friendly palettes
5. **Missing Context**: Consider adding reference lines

## Performance Considerations

### Large Datasets

| Approach | When to Use |
|----------|-------------|
| Transparency | n < 10,000 |
| Binning | n < 100,000 |
| Sampling | n > 100,000 |
| 2D Density | Any size |

## Related Concepts

- **Line Plots**: When x-axis represents time or order
- **Box Plots**: Summarize distributions
- **Heatmaps**: For matrix data
- **3D Plots**: For three continuous variables

## Exercise Problems

1. **Basic**: Create a scatter plot of any two variables in the iris dataset.

2. **Intermediate**: Add color and size to show three variables.

3. **Advanced**: Create a scatter plot matrix for a dataset.

4. **Real-World Challenge**: Analyze relationships in a real dataset.

5. **Extension**: Add interactive elements with plotly.