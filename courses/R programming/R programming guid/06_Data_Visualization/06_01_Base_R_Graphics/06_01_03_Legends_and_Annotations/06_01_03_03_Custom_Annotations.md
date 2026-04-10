# Custom Annotations in Base R Graphics

## Learning Objectives

- Add custom text annotations at any position
- Draw arrows and segments for emphasis
- Create shaded regions for highlighting
- Use locator() for interactive positioning
- Add mathematical and formula annotations
- Combine multiple annotation types

## Theoretical Background

### Annotation Functions

1. **text()**: Add text at specific (x, y) coordinates
2. **arrows()**: Draw arrows between points
3. **segments()**: Draw line segments
4. **rect()**: Draw rectangles/shaded regions
5. **polygon()**: Draw arbitrary shapes
6. **locator()**: Interactive positioning

### Coordinate Systems

- **user coordinates**: Data coordinates (default)
- **ndc coordinates**: Normalized device coordinates (0 to 1)
- **string coordinates**: Character height units

### Shading Functions

- **rect()**: Rectangle with edges at specified coordinates
- **polygon()**: Custom shape with vertex coordinates
- **curve()**: Mathematical function curves

## Code Examples

### Example: Text Annotations with Arrows

```r
cat("===== TEXT WITH ARROW ANNOTATIONS =====\n\n")

# Sample data
x <- 1:20
y <- c(3, 5, 4, 7, 6, 9, 8, 11, 10, 13, 12, 15, 14, 17, 16, 19, 18, 21, 20, 23)

plot(x, y, type = "l", col = "steelblue", lwd = 2,
     xlab = "Time", ylab = "Value",
     main = "Time Series with Annotated Peaks")

# Identify peak
peak_x <- x[which.max(y)]
peak_y <- max(y)

# Add annotation with arrow pointing to peak
arrows(x0 = peak_x + 3, y0 = peak_y,
       x1 = peak_x + 0.5, y1 = peak_y,
       col = "red", lwd = 2, code = 1)

text(x = peak_x + 3.5, y = peak_y,
     labels = paste0("Peak: ", peak_y),
     col = "red", font = 2, pos = 4)

# Annotate minimum
min_x <- x[which.min(y)]
min_y <- min(y)

arrows(x0 = min_x + 3, y0 = min_y,
       x1 = min_x + 0.5, y1 = min_y,
       col = "darkgreen", lwd = 2, code = 1)

text(x = min_x + 3.5, y = min_y,
     labels = paste0("Min: ", min_y),
     col = "darkgreen", font = 2, pos = 4)

cat("Added text annotations with arrows\n")
```

### Example: Shaded Regions

```r
cat("\n===== SHADED REGION ANNOTATIONS =====\n\n")

# Create time series
time <- 0:100
set.seed(42)
value <- 50 + 10 * sin(time * 0.05) + rnorm(101, 0, 5)
baseline <- 50

plot(time, value, type = "l", col = "darkblue",
     xlab = "Time", ylab = "Value",
     main = "Process Monitoring with Control Limits")

# Add shaded region for normal range
rect(xleft = 0, xright = 100,
     ybottom = baseline - 10, ytop = baseline + 10,
     col = rgb(0, 0.8, 0, 0.2), border = NA)

# Add horizontal reference lines
abline(h = baseline, col = "darkgreen", lty = 2, lwd = 2)
abline(h = baseline + 10, col = "red", lty = 2, lwd = 1)
abline(h = baseline - 10, col = "red", lty = 2, lwd = 1)

# Add labels for control limits
text(x = 95, y = baseline + 10, labels = "UCL", col = "red", pos = 4)
text(x = 95, y = baseline - 10, labels = "LCL", col = "red", pos = 4)
text(x = 95, y = baseline, labels = "Target", col = "darkgreen", pos = 4)

cat("Created shaded region for control limits\n")
```

### Example: Segments and Lines

```r
cat("\n===== SEGMENT AND LINE ANNOTATIONS =====\n\n")

# Scatter plot
x <- runif(50, 0, 10)
y <- 2 * x + rnorm(50, 0, 2)

plot(x, y, col = "steelblue", pch = 19,
     xlab = "X", ylab = "Y",
     main = "Linear Regression with Annotations")

# Fit linear model
model <- lm(y ~ x)

# Add regression line
abline(model, col = "red", lwd = 2)

# Add vertical segments for residuals
for (i in seq(1, 50, by = 5)) {
  segments(x0 = x[i], y0 = y[i],
           x1 = x[i], y1 = predict(model)[i],
           col = rgb(0.8, 0, 0, 0.3), lwd = 1)
}

# Add slope annotation
text(x = 1, y = 18,
     labels = paste0("y = ", round(coef(model)[1], 2), " + ",
                    round(coef(model)[2], 2), "x"),
     col = "darkred", font = 2, cex = 1.2)

# Add R-squared annotation
rsq <- summary(model)$r.squared
text(x = 1, y = 16,
     labels = paste0("R² = ", round(rsq, 3)),
     col = "darkblue", font = 2)

cat("Added regression line with residual indicators\n")
```

### Real-World Example: Event Markers

```r
cat("\n===== EVENT MARKER ANNOTATIONS =====\n\n")

# Website traffic data
dates <- as.Date("2024-01-01") + 0:180
visitors <- 1000 + cumsum(rnorm(181, 5, 50))

plot(dates, visitors, type = "l", col = "darkgreen",
     xlab = "Date", ylab = "Visitors",
     main = "Website Traffic with Marketing Events")

# Mark marketing campaign start
campaign_date <- as.Date("2024-03-01")
campaign_x <- as.numeric(campaign_date - as.Date("2024-01-01")) + 1

# Add vertical line
abline(v = campaign_date, col = "purple", lty = 2, lwd = 2)

# Add campaign annotation
text(x = campaign_date, y = max(visitors) * 0.95,
     labels = "Campaign\nLaunch",
     col = "purple", font = 2, pos = 2)

# Mark holiday period
holiday_start <- as.Date("2024-12-20")
holiday_end <- as.Date("2024-12-31")

rect(xleft = holiday_start, xright = holiday_end,
     ybottom = par("usr")[3], ytop = par("usr")[4],
     col = rgb(1, 0.5, 0, 0.15), border = NA)

text(x = as.Date("2024-12-25"), y = max(visitors) * 0.85,
     labels = "Holiday\nPeriod",
     col = "darkorange", font = 2)

cat("Created event-marked time series\n")
```

### Real-World Example: Comparison Annotations

```r
cat("\n===== COMPARISON ANNOTATIONS =====\n\n")

# A/B test results
group_a <- rnorm(100, 5.2, 1.5)
group_b <- rnorm(100, 5.8, 1.3)

# Create histogram comparison
hist(group_a, col = rgb(0, 0.5, 0.8, 0.5), border = "darkblue",
     xlim = c(0, 10), ylim = c(0, 35),
     xlab = "Conversion Rate (%)",
     main = "A/B Test Results")

hist(group_b, col = rgb(0.8, 0.2, 0.2, 0.5), border = "darkred",
     add = TRUE)

# Add legend
legend(x = "topright",
       legend = c("Control (A)", "Variant (B)"),
       fill = c(rgb(0, 0.5, 0.8, 0.5), rgb(0.8, 0.2, 0.2, 0.5)),
       border = c("darkblue", "darkred"),
       bty = "o")

# Add mean lines
abline(v = mean(group_a), col = "blue", lty = 2, lwd = 2)
abline(v = mean(group_b), col = "red", lty = 2, lwd = 2)

# Add annotation box
text(x = 7.5, y = 30,
     labels = paste0("A mean: ", round(mean(group_a), 2), "\n",
                     "B mean: ", round(mean(group_b), 2), "\n",
                     "Lift: ", round((mean(group_b) - mean(group_a)) / mean(group_a) * 100, 1), "%"),
     font = 2, adj = 0)

cat("Created A/B test comparison with annotations\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use contrasting colors** for annotations vs data
2. **Add arrows with code** parameter to control arrowhead direction
3. **Use rect() for regions** and abline() for reference lines
4. **Position annotations** to avoid obscuring data
5. **Use pos parameter** in text() for consistent positioning

### Common Issues

1. **Annotations covering data**: Use inset positioning or relocate
2. **Arrow not visible**: Check code parameter (1, 2, or 3)
3. **Rect clipped**: Ensure coordinates within plot range
4. **Multiple calls overlapping**: Use unique positions or one polygon()

### Interactive Positioning

```r
# Use locator() to find coordinates interactively
plot(x, y)
cat("Click where you want to place annotation\n")
coords <- locator(1)
text(coords$x, coords$y, "Click point")
```

## Related Concepts

- `legend()` - Adding legends
- `title()` - Adding titles and axis labels
- `grid()` - Adding grid lines
- `par()` - Setting graphical parameters

## Exercise Problems

1. Create a scatter plot and annotate the outliers with arrows pointing to each one
2. Add a shaded rectangle highlighting a specific time period in a time series
3. Draw vertical lines marking three standard deviations from the mean
4. Use locator() to interactively place an annotation on a plot
5. Create a plot with multiple annotations showing statistical properties (mean, median, SD)