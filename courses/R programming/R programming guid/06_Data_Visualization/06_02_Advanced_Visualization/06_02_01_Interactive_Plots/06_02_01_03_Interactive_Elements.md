# Interactive Elements in Plots

## Learning Objectives

- Add interactive features to base R graphics
- Implement click, hover, and brush selection
- Create interactive legends and selectors
- Build drill-down capabilities
- Combine multiple interaction types

## Theoretical Background

### Interaction Types in R

1. **Click Events**: Identify clicked points using locator() or event handling
2. **Hover Effects**: Show information on mouse hover
3. **Brush Selection**: Select rectangular regions
4. **Zoom and Pan**: Navigate data interactively

### Interactive Packages

- **plotly**: Built-in JavaScript interactivity
- **shiny**: Web application framework
- **ggiraph**: Interactive ggplot2
- **htmlwidgets**: Custom interactive visualizations
- **crosstalk**: Linked brushing between plots

### Key Concepts

- **Reactivity**: Changes in response to user actions
- **Event handling**: Processing user interactions
- **Feedback**: Visual response to user input

## Code Examples

### Example: Interactive Point Selection

```r
cat("===== INTERACTIVE POINT SELECTION =====\n\n")

# Create sample data
set.seed(42)
x <- 1:20
y <- 2 * x + rnorm(20, 0, 3)
groups <- rep(c("A", "B"), each = 10)

# Identify points interactively
plot(x, y, col = ifelse(groups == "A", "steelblue", "darkorange"),
     pch = 19, cex = 1.5,
     main = "Click on points to identify (right-click to stop)")

# Add legend
legend(x = "topleft", legend = c("Group A", "Group B"),
       col = c("steelblue", "darkorange"), pch = 19)

cat("Use locator(1) to click and identify a point:\n")

# Interactive point identification
# Clicked <- locator(1)
# Clicked coordinates will be output

cat("Interactive identification complete\n")
```

### Example: Brush Selection for Data

```r
cat("\n===== BRUSH SELECTION =====\n\n")

# Note: In base R, brush selection requires shiny
# This example shows the concept in Shiny

# Sample data for selection
set.seed(123)
df <- data.frame(
  id = 1:100,
  x = rnorm(100),
  y = rnorm(100),
  group = sample(c("A", "B", "C"), 100, replace = TRUE)
)

cat("Brush selection concept:\n")
cat("- In Shiny, use brush = 'plot_brush' to enable\n")
cat("- Selected points can be extracted and filtered\n")
cat("- Selected data can drive secondary plots\n")

# Example Shiny code structure:
# output$plot <- renderPlot({
#   brush_info <- input$plot_brush
#   if (!is.null(brush_info)) {
#     selected <- brushedPoints(df, brush_info)
#     # Process selected points
#   }
# })

cat("Ready for Shiny brush implementation\n")
```

### Example: Dynamic Legend Selection

```r
cat("\n===== DYNAMIC LEGEND SELECTION =====\n\n")

# Create multiple series
x <- 1:30
series1 <- sin(x/5) + rnorm(30, 0, 0.2)
series2 <- cos(x/5) + rnorm(30, 0, 0.2)
series3 <- sin(x/5) + cos(x/5) + rnorm(30, 0, 0.2)

# Plot with toggle capability via Shiny
cat("Dynamic legend toggling requires Shiny:\n")

# Example concept:
# output$plot <- renderPlot({
#   # Plot all series
#   if (input$show_s1) {
#     lines(x, series1, col = "steelblue")
#   }
#   if (input$show_s2) {
#     lines(x, series2, col = "darkorange")
#   }
#   if (input$show_s3) {
#     lines(x, series3, col = "darkgreen")
#   }
# })

# Static plot for demonstration
plot(x, series1, type = "l", col = "steelblue",
     ylim = c(-2, 2), main = "Multiple Series")
lines(x, series2, col = "darkorange")
lines(x, series3, col = "darkgreen")
legend(x = "topright", 
       legend = c("Sine", "Cosine", "Combined"),
       col = c("steelblue", "darkorange", "darkgreen"),
       lty = 1)

cat("Created multi-series plot with legend\n")
```

### Real-World Example: Drill-Down Chart

```r
cat("\n===== DRILL-DOWN CHART =====\n\n")

# Concept: Click on bar to see detail
cat("Drill-down functionality structure:\n")

# This represents a Shiny app structure:
# ui <- fluidPage(
#   plotOutput("mainPlot", click = "plot_click")
# )
# 
# server <- function(input, output) {
#   output$mainPlot <- renderPlot({
#     # Aggregate to category level
#     agg <- aggregate(sales ~ category, data, sum)
#     barplot(agg$sales, names.arg = agg$category,
#            col = "steelblue", main = "Click category for details")
#   })
#   
#   output$detailPlot <- renderPlot({
#     req(input$plot_click)
#     # Get clicked category
#     clicked_cat <- categorize_click(input$plot_click)
#     # Show subcategory detail
#     detail <- subset(data, category == clicked_cat)
#     plot(detail$date, detail$sales, type = "l",
#         main = paste("Detail for", clicked_cat))
#   })
# }

# Simulated category data
categories <- c("Electronics", "Clothing", "Food", "Home")
sales <- c(45000, 32000, 28000, 22000)

barplot(sales, names.arg = categories, col = "steelblue",
        main = "Click on bar for category details",
        xlab = "Category", ylab = "Sales ($)")

cat("Created drill-down visualization concept\n")
```

### Real-World Example: Linked Brush Plots

```r
cat("\n===== LINKED BRUSH PLOTS =====\n\n")

# Concept: Brush in one plot highlights in another
cat("Linked brushing structure in Shiny/crosstalk:\n")

# Sample linked data
set.seed(42)
n <- 200
df <- data.frame(
  id = 1:n,
  age = rnorm(n, 40, 12),
  income = rnorm(n, 60000, 15000),
  score = rnorm(n, 75, 15)
)

cat("Data structure:\n")
cat("- id: unique identifier\n")
cat("- age: customer age\n")
cat("- income: annual income\n")
cat("- score: satisfaction score\n")

cat("\nLinked plot concepts:\n")
cat("1. Scatter plot shows age vs income\n")
cat("2. Histogram shows score distribution\n")
cat("3. Brushing scatter highlights histogram bins\n")

# Visual demonstration
par(mfrow = c(1, 2))

# Scatter plot
plot(df$age, df$income, pch = 19, col = "steelblue", alpha = 0.5,
     main = "Age vs Income", xlab = "Age", ylab = "Income")

# Histogram
hist(df$score, col = "steelblue", border = "white",
     main = "Score Distribution", xlab = "Score")

par(mfrow = c(1, 1))

cat("Created linked visualization demonstration\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use plotly** for built-in interactivity without coding
2. **Provide feedback** on selection (highlighted colors)
3. **Clear selection** between operations
4. **Optimize for large datasets** with sampling
5. **Test on touch devices** for mobile compatibility

### Common Issues

1. **Events not firing**: Ensure proper event handlers in Shiny
2. **Performance slow**: Use data.table for large data operations
3. **Selection not clearing**: Use isolate() or explicit reset
4. **Memory leaks**: Clean up reactive values

### Quick Interactive Plot with ggiraph

```r
# ggiraph provides interactive ggplot2
# library(ggiraph)
# 
# p <- ggplot(data, aes(x = x, y = y, tooltip = "info")) +
#   geom_point_interactive()
# 
# girafe(ggobj = p)
```

## Related Concepts

- `locator()` - Interactive point identification
- `plotly` - Interactive JavaScript plots
- `shiny` - Full interactive applications
- `crosstalk` - Linked interactive views

## Exercise Problems

1. Create an interactive scatter plot where clicking shows data details
2. Implement a zoomable time series plot
3. Add hover tooltips to display statistics
4. Build a cross-filter dashboard with linked plots
5. Create a selector that filters multiple plots simultaneously