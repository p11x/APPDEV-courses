# Plotly Basics in R

## Learning Objectives

- Understand the Plotly R library and its capabilities
- Create basic interactive plots using plot_ly()
- Customize plot appearance and interactivity
- Convert ggplot2 plots to Plotly using ggplotly()
- Add interactive elements like hover and zoom

## Theoretical Background

### What is Plotly?

Plotly is an open-source JavaScript visualization library with R bindings. It creates interactive, publication-quality graphs that can be embedded in web applications or R Markdown documents.

### Key Features

1. **Interactivity**: Hover tooltips, zoom, pan, selection
2. **Web-based**: Renders as HTML/JavaScript
3. ** ggplot2 integration**: Convert ggplot2 to plotly
4. **3D visualizations**: Native 3D plot support
5. **Dash support**: Building analytical web apps

### Plotly R API

```
plot_ly(data, x = ~column, y = ~column, type = "scatter", mode = "markers")
```

### Trace Types

- "scatter": Points, lines, or both
- "bar": Bar charts
- "histogram": Histograms
- "box": Box plots
- "surface": 3D surfaces
- "mesh3d": 3D meshes

## Code Examples

### Example: Basic Scatter Plot

```r
cat("===== PLOTLY BASIC SCATTER =====\n\n")

library(plotly)

# Sample data
df <- data.frame(
  x = 1:50,
  y = cumsum(rnorm(50)),
  category = rep(c("A", "B"), each = 25),
  size = rep(1:25, 2)
)

# Create basic scatter plot
fig <- plot_ly(df, x = ~x, y = ~y, type = "scatter", mode = "markers")

# Add styling
fig <- fig %>% layout(
  title = "Interactive Scatter Plot",
  xaxis = list(title = "Time"),
  yaxis = list(title = "Value")
)

cat("Created basic Plotly scatter plot\n")

# Display
print(fig)
```

### Example: Line Chart with Custom Styling

```r
cat("\n===== PLOTLY LINE CHART =====\n\n")

# Time series data
dates <- seq(as.Date("2024-01-01"), by = "day", length.out = 90)
values <- 100 + cumsum(rnorm(90, 0.1, 2))
values2 <- 100 + cumsum(rnorm(90, 0.05, 1.5))

# Create line plot with multiple traces
fig <- plot_ly()

# Add first series
fig <- fig %>% add_trace(
  x = dates, y = values,
  type = "scatter", mode = "lines",
  name = "Product A",
  line = list(color = "steelblue", width = 2)
)

# Add second series
fig <- fig %>% add_trace(
  x = dates, y = values2,
  type = "scatter", mode = "lines",
  name = "Product B",
  line = list(color = "darkorange", width = 2)
)

# Add layout
fig <- fig %>% layout(
  title = "Product Performance Comparison",
  xaxis = list(title = "Date"),
  yaxis = list(title = "Sales ($K)"),
  hovermode = "x unified"
)

cat("Created multi-line Plotly chart\n")
```

### Example: Bar Chart with Color Mapping

```r
cat("\n===== PLOTLY BAR CHART =====\n\n")

# Sales data by category
categories <- c("Electronics", "Clothing", "Food", "Books", "Home")
sales <- c(45000, 32000, 28000, 15000, 22000)
profit <- c(12, 18, 8, 25, 15)

# Create grouped bar chart
fig <- plot_ly()

fig <- fig %>% add_trace(
  x = categories, y = sales,
  type = "bar", name = "Sales",
  marker = list(color = "steelblue")
)

fig <- fig %>% add_trace(
  x = categories, y = profit * 1000,
  type = "bar", name = "Profit",
  marker = list(color = "darkorange"),
  yaxis = "y2"
)

fig <- fig %>% layout(
  title = "Sales and Profit by Category",
  barmode = "group",
  yaxis = list(title = "Sales ($)"),
  yaxis2 = list(title = "Profit ($)", overlaying = "y", side = "right"),
  xaxis = list(title = "Category"),
  legend = list(x = 1, y = 1)
)

cat("Created grouped bar chart\n")
```

### Real-World Example: Financial Candlestick

```r
cat("\n===== CANDLESTICK CHART =====\n\n")

# Generate stock data
set.seed(42)
n <- 100
open_prices <- 100 + cumsum(rnorm(n, 0.1, 1))
close_prices <- open_prices + rnorm(n, 0, 1.5)
high_prices <- pmax(open_prices, close_prices) + abs(rnorm(n, 0, 0.5))
low_prices <- pmin(open_prices, close_prices) - abs(rnorm(n, 0, 0.5))
dates <- as.Date("2024-01-01") + 1:n

# Create candlestick chart
fig <- plot_ly()

fig <- fig %>% add_trace(
  x = dates,
  type = "candlestick",
  open = open_prices,
  close = close_prices,
  high = high_prices,
  low = low_prices,
  name = "Stock Price",
  increasing = list(line = list(color = "green")),
  decreasing = list(line = list(color = "red"))
)

fig <- fig %>% layout(
  title = "Stock Price Candlestick Chart",
  xaxis = list(rangeslider = list(visible = FALSE)),
  yaxis = list(title = "Price ($)")
)

cat("Created candlestick chart\n")
```

### Real-World Example: Interactive Dashboard Widget

```r
cat("\n===== DASHBOARD WIDGET =====\n\n")

# Customer data
set.seed(123)
customers <- data.frame(
  age = rnorm(500, 40, 12),
  income = rnorm(500, 60000, 15000),
  satisfaction = sample(1:10, 500, replace = TRUE),
  segment = sample(c("Premium", "Standard", "Basic"), 500, replace = TRUE)
)

# Create scatter with color and size mapping
fig <- plot_ly(customers, 
               x = ~age, 
               y = ~income,
               color = ~segment,
               size = ~satisfaction,
               colors = c("Premium" = "gold", 
                         "Standard" = "steelblue", 
                         "Basic" = "gray"),
               type = "scatter", mode = "markers",
               marker = list(opacity = 0.7, sizemode = "diameter"),
               text = ~paste("Age:", age, "<br>Income: $", round(income),
                            "<br>Satisfaction:", satisfaction, "/10"),
               hoverinfo = "text")

fig <- fig %>% layout(
  title = "Customer Segmentation Analysis",
  xaxis = list(title = "Customer Age"),
  yaxis = list(title = "Annual Income ($)"),
  legend = list(title = list(text = "Segment"))
)

cat("Created customer segmentation visualization\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use ggplotly()** for quick conversion of existing ggplot2 code
2. **Set hoverinfo** appropriately to avoid cluttered tooltips
3. **Use layout()** for consistent styling across subplots
4. **Enable range slider** for time series
5. **Set appropriate mode** (markers, lines, lines+markers)

### Common Issues

1. **Plot not rendering**: Ensure RStudio viewer or browser is selected
2. **Large data slow**: Use sampling or aggregation for large datasets
3. **Hover info too long**: Simplify text or use hoverinfo selectively
4. **Colors not matching**: Use explicit color lists

### Saving Plots

```r
# Save as HTML
saveWidget(fig, "plot.html")

# Save as image
orca(fig, "plot.png")
```

## Related Concepts

- ggplotly() - Converting ggplot2 to Plotly
- subplot() - Creating multi-panel plots
- add_trace() - Adding additional traces
- layout() - Customizing plot layout

## Exercise Problems

1. Create a Plotly scatter plot with color-coded groups and custom hover text
2. Convert a ggplot2 histogram to an interactive Plotly chart
3. Create a line chart with multiple series and a legend
4. Build a bar chart with tooltips showing exact values
5. Design an interactive dashboard widget with zoom capabilities