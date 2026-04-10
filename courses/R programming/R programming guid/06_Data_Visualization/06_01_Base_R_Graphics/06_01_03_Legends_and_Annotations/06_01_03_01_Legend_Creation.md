# Legend Creation in Base R Graphics

## Learning Objectives

- Create legends using legend() function
- Customize legend position, appearance, and content
- Handle multiple groups and aesthetic mappings
- Use inset legends and legend styling options
- Control legend keys and symbols

## Theoretical Background

### The legend() Function

The `legend()` function in Base R creates legends with extensive customization options:

```
legend(x, y, legend, col, pch, lty, lwd, 
       bty = "o", bg = "white",
       xjust = 0, yjust = 1,
       x.intersp = 1, y.intersp = 1,
       adj = c(0, 0.5),
       text.width = NULL,
       ncol = 1, horiz = FALSE)
```

### Key Parameters

1. **Position**: x, y coordinates or keywords ("top", "bottomright", etc.)
2. **Content**: legend text labels in character vector
3. **Styling**: col (colors), pch (point symbols), lty (line types), lwd (line widths)
4. **Box**: bty (box type: "o", "n", "l", "7", "c", "u"), bg (background color)
5. **Layout**: ncol (columns), horiz (horizontal layout)

### Legend Positioning

- Keywords: "top", "topright", "right", "bottomright", "bottom", "bottomleft", "left", "topleft"
- Coordinates: x and y in data coordinates
- Inset: Use inset parameter as fraction of plot area

## Code Examples

### Example: Basic Legend

```r
cat("===== BASIC LEGEND CREATION =====\n\n")

# Sample data with multiple groups
x <- 1:10
y1 <- c(2, 4, 3, 5, 6, 8, 7, 9, 10, 11)
y2 <- c(1, 3, 2, 4, 5, 7, 6, 8, 9, 10)

# Create plot with two lines
plot(x, y1, type = "b", col = "blue", pch = 19,
     xlab = "Time", ylab = "Value", main = "Product Comparison")
lines(x, y2, type = "b", col = "red", pch = 17)

# Add legend
legend(x = "topleft",        # Position keyword
       legend = c("Product A", "Product B"),
       col = c("blue", "red"),
       pch = c(19, 17),
       lty = c(1, 1),
       bty = "o",
       bg = "white")

cat("Created basic legend with position keyword\n")
```

### Example: Custom Legend Appearance

```r
cat("\n===== CUSTOM LEGEND APPEARANCE =====\n\n")

# Create plot with multiple groups
x <- 1:15
set.seed(42)

# Three different data series
group_a <- 5 + 0.8 * x + rnorm(15, 0, 1)
group_b <- 3 + 0.6 * x + rnorm(15, 0, 1)
group_c <- 1 + 0.4 * x + rnorm(15, 0, 1)

plot(x, group_a, type = "l", col = "darkgreen", lwd = 2,
     xlab = "Period", ylab = "Performance", main = "Performance Trends")
lines(x, group_b, col = "darkorange", lwd = 2, lty = 2)
lines(x, group_c, col = "purple", lwd = 2, lty = 3)

# Custom styled legend
legend(x = "topleft",
       legend = c("Team Alpha", "Team Beta", "Team Gamma"),
       col = c("darkgreen", "darkorange", "purple"),
       lty = c(1, 2, 3),
       lwd = 2,
       bty = "o",
       bg = "lightyellow",
       box.col = "darkgray",
       text.font = 4,        # Italic text
       cex = 0.9,
       ncol = 3,             # Horizontal layout
       horiz = TRUE)

cat("Created custom styled horizontal legend\n")
```

### Example: Legend with Points and Lines

```r
cat("\n===== LEGEND WITH POINTS AND LINES =====\n\n")

# Create sample data
years <- 2015:2024
revenue <- c(100, 120, 135, 150, 165, 190, 210, 230, 255, 280)
profit <- c(10, 15, 18, 22, 28, 35, 42, 50, 60, 72)

# Plot with both points and trend lines
plot(years, revenue, type = "p", col = "steelblue", pch = 19, cex = 1.5,
     ylim = c(0, 300), xlab = "Year", ylab = "Amount ($K)",
     main = "Financial Performance")
lines(years, revenue, col = "steelblue", lty = 1, lwd = 1.5)

points(years, profit, col = "darkred", pch = 17, cex = 1.5)
lines(years, profit, col = "darkred", lty = 2, lwd = 1.5)

# Add legend showing both point type and line style
legend(x = "topleft",
       legend = c("Revenue", "Profit"),
       col = c("steelblue", "darkred"),
       pch = c(19, 17),
       lty = c(1, 2),
       lwd = 1.5,
       bty = "n",            # No box
       bg = "white",
       text.font = 2)

# Add vertical line separator
abline(v = 2019.5, col = "gray50", lty = 3)

cat("Created legend with both points and lines\n")
```

### Real-World Example: Marketing Campaign Legend

```r
cat("\n===== MARKETING CAMPAIGN LEGEND =====\n\n")

# Marketing data
channels <- c("Social", "Email", "Organic", "Paid", "Referral")
visitors <- c(45000, 28000, 62000, 35000, 18000)
conversions <- c(2.1, 3.8, 1.5, 2.8, 4.2)

# Create dual-axis plot
bar_positions <- barplot(visitors, names.arg = channels, col = "lightblue",
                         ylim = c(0, 70000), xlab = "Channel",
                         ylab = "Visitors")

# Add conversion line
par(new = TRUE)
plot(conversions, type = "b", col = "darkred", pch = 19, axes = FALSE,
     xlab = "", ylab = "", ylim = c(0, 5))
axis(side = 4, at = seq(0, 5, 1))
mtext("Conversion Rate (%)", side = 4, line = 2)

# Reset parameters
par(new = FALSE)

# Add legend
legend(x = "top",
       legend = c("Visitors (bars)", "Conversion Rate (line)"),
       col = c("lightblue", "darkred"),
       pch = c(15, 19),
       lty = c(0, 1),
       bty = "o",
       bg = "white",
       ncol = 2,
       horiz = TRUE,
       x.intersp = 0.5)

cat("Created marketing dashboard legend\n")
```

### Real-World Example: Survey Results Legend

```r
cat("\n===== SURVEY RESULTS LEGEND =====\n\n")

# Survey data across categories
categories <- c("Very Satisfied", "Satisfied", "Neutral", "Dissatisfied")
responses_q1 <- c(35, 40, 15, 10)
responses_q2 <- c(45, 35, 12, 8)
responses_q3 <- c(28, 45, 18, 9)

# Stacked bar plot
barplot(rbind(responses_q1, responses_q2, responses_q3),
        beside = FALSE,
        col = c("darkgreen", "steelblue", "darkorange"),
        names.arg = categories,
        ylim = c(0, 100),
        xlab = "Satisfaction Level",
        ylab = "Percentage (%)",
        main = "Customer Satisfaction Survey Results")

# Add legend with survey quarters
legend(x = "topright",
       legend = c("Q1 2024", "Q2 2024", "Q3 2024"),
       fill = c("darkgreen", "steelblue", "darkorange"),
       bty = "o",
       bg = "white",
       box.col = "gray80",
       title = "Quarter",
       title.font = 2)

cat("Created survey results legend with fill colors\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use descriptive legend labels** that match data source names
2. **Match legend aesthetics** exactly to plot elements (col, pch, lty, lwd)
3. **Use ncol for horizontal legends** when space permits
4. **Set bty = "n"** for cleaner appearance without box
5. **Use inset for precise positioning** when keywords are not exact

### Common Issues

1. **Legend covering data**: Use position keywords or adjust coordinates
2. **Legend text too long**: Abbreviate or use shorter labels
3. **Colors not matching**: Ensure col vector matches legend order
4. **pch not showing**: Set pch correctly in legend, not just in plot

### Positioning with Inset

```r
# Position legend with inset (fraction of plot area)
legend(x = "right", inset = c(-0.15, 0),
       legend = c("A", "B"), col = c("red", "blue"),
       pch = c(19, 17))
```

## Related Concepts

- `title()` - Adding plot titles and axis labels
- `text()` - Adding custom text annotations
- `par()` - Setting graphical parameters
- `layout()` - Multi-panel plot arrangements

## Exercise Problems

1. Create a scatter plot with different point colors by group and add a corresponding legend
2. Add a legend positioned inside the plot area using coordinates
3. Create a horizontal legend at the bottom of a plot with 4 categories
4. Design a legend that shows both line type and point character for a line plot with markers
5. Create an inset legend in the corner of a plot without overlapping data