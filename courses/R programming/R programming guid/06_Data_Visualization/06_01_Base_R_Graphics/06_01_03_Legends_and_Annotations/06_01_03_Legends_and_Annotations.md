# Legends and Annotations in Base R

## Learning Objectives

- Add and customize legends using legend()
- Add text annotations using text() and mtext()
- Draw lines and arrows with abline(), lines(), and arrows()
- Add mathematical expressions to plots
- Create complex annotations with multiple elements

## Theoretical Background

### Legend Function

The `legend()` function adds a legend to existing plots. It has numerous parameters for customization:
- position: "topright", "topleft", "bottomright", "bottomleft", or coordinates
- contents: Labels for each legend entry
- visual elements: pch, lty, fill, col

### Annotation Functions

Base R provides several annotation functions:
- `text()`: Add text at specific coordinates within the plot
- `mtext()`: Add text in the margins
- `title()`: Add/modify plot title
- `axis()`: Add custom axes

### Line-Drawing Functions

- `abline()`: Add a line given by intercept and slope, or horizontal/vertical lines
- `lines()`: Add connected line segments
- `segments()`: Add line segments between specific points
- `arrows()`: Add arrows between points
- `rect()`: Add rectangles

## Code Examples

### Example 1: Basic Legend

```r
# Sample data
x <- 1:20
y1 <- x + rnorm(20, 0, 3)
y2 <- 1.5 * x + rnorm(20, 0, 4)

# Create plot with first series
plot(x, y1, type = "o", col = "blue",
     pch = 19, main = "Legend Example",
     xlab = "X", ylab = "Y")

# Add second series
lines(x, y2, type = "o", col = "red", pch = 17)

# Add legend
legend("topleft",
       legend = c("Series 1", "Series 2"),
       col = c("blue", "red"),
       pch = c(19, 17),
       lty = 1,
       bty = "n")  # No box around legend
```

### Example 2: Customized Legend with Fill

```r
# Bar plot with custom legend
par(mar = c(5, 4, 4, 2))

# Sample data
bar_data <- matrix(c(10, 15, 12, 20, 18, 22),
                   nrow = 3, ncol = 2)
rownames(bar_data) <- c("Q1", "Q2", "Q3")
colnames(bar_data) <- c("Product A", "Product B")

# Create grouped bar plot
barplot(bar_data,
        beside = TRUE,
        col = c("lightblue", "lightgreen", "lightpink"),
        main = "Quarterly Sales by Product",
        xlab = "Product",
        ylab = "Sales ($K)")

# Add legend
legend("top",
       legend = rownames(bar_data),
       fill = c("lightblue", "lightgreen", "lightpink"),
       bty = "n",
       cex = 0.8)
```

### Example 3: Text Annotations

```r
# Create base plot
x <- 1:10
y <- c(2, 4, 3, 5, 6, 8, 7, 9, 10, 11)

plot(x, y, type = "o", pch = 19,
     main = "Text Annotations",
     xlab = "Time", ylab = "Value")

# Add text annotation at specific point
text(5, 8, "Peak Value", pos = 4,
     col = "red", cex = 0.9)

# Add annotation with arrow
arrows(5, 7.5, 5, 6.5,
       col = "red", length = 0.15)

# Add label with offset
text(7, 10, "Growing Trend",
     pos = 3, offset = 0.5,
     col = "darkblue", font = 2)

# Add data labels
text(x, y + 0.3, labels = round(y, 1),
     cex = 0.7, col = "gray40")
```

### Example 4: Margin Text (mtext)

```r
# Create plot with outer margin text
par(oma = c(3, 3, 3, 1), mar = c(5, 4, 4, 2))

x <- 1:15
y <- sin(x / 2) + rnorm(15, 0, 0.1)

plot(x, y, type = "l",
     main = "Margin Annotations",
     xlab = "", ylab = "")

# Inner margin labels
mtext("X Axis Label", side = 1, line = 3)
mtext("Y Axis Label", side = 2, line = 3)

# Outer margin labels - for multi-line titles
mtext("OUTER TITLE", side = 3, line = 0,
      outer = TRUE, cex = 1.5, font = 2)
mtext("Subtitle in outer margin",
      side = 3, line = 1.5,
      outer = TRUE, col = "gray50")

# Multiple margin lines
for(i in 1:3) {
  mtext(paste("Line", i), side = 3, 
        line = i - 0.5, outer = TRUE)
}
```

### Example 5: Lines and Guidelines with abline

```r
# Create data
x <- 1:30
y <- 3 + 0.5 * x + rnorm(30, 0, 2)

plot(x, y, pch = 19, col = "steelblue",
     main = "Using abline()",
     xlab = "X", ylab = "Y")

# Add regression line
fit <- lm(y ~ x)
abline(fit, col = "red", lty = 2, lwd = 2)

# Add horizontal reference line
abline(h = mean(y), col = "darkgreen",
      lty = 3, lwd = 2)

# Add vertical reference line
abline(v = median(x), col = "purple",
      lty = 4, lwd = 1.5)

# Add legend
legend("topleft",
       legend = c("Data", "Regression", 
                  "Mean Y", "Median X"),
       col = c("steelblue", "red", 
               "darkgreen", "purple"),
       lty = c(NA, 2, 3, 4),
       pch = c(19, NA, NA, NA),
       bty = "n")
```

### Example 6: Mathematical Expressions

```r
# Create plot for mathematical annotations
x <- seq(0, 2 * pi, length.out = 100)
y <- sin(x)

plot(x, y, type = "l",
     main = "Mathematical Expressions",
     xlab = expression(theta),
     ylab = expression(sin(theta)))

# Add mean line
abline(h = 0, col = "gray")

# Add text with mathematical notation
text(pi, 0, expression(sin(pi) == 0),
     pos = 4)

# Add peak annotation
text(pi/2, 1, expression(paste("maximum at ", 
                                frac(pi, 2))),
     pos = 3)

# Add equation annotation
text(3 * pi / 2, -1,
     expression(italic(f)(x) = sum(1, n) ),
     col = "blue")
```

### Example 7: Complex Multi-Element Annotations

```r
# Create multi-series plot
par(mar = c(6, 4, 4, 2))

x <- 1:12
y1 <- c(100, 120, 110, 130, 125, 140, 145, 150, 
        155, 160, 170, 180)
y2 <- c(80, 85, 82, 90, 88, 95, 100, 102, 105,
        108, 110, 115)
y3 <- c(50, 52, 55, 58, 56, 60, 62, 65, 63, 68,
        70, 72)

# Create filled area plot
plot(x, y1, type = "n",
     main = "Multi-Series Analysis",
     xlab = "Month", ylab = "Revenue ($K)",
     ylim = c(0, 200))

# Add filled areas
polygon(c(x, rev(x)), c(y1, rep(0, length(x))),
        col = rgb(0.2, 0.4, 0.8, 0.3), border = NA)
polygon(c(x, rev(x)), c(y2, rep(0, length(x))),
        col = rgb(0.8, 0.2, 0.2, 0.3), border = NA)
polygon(c(x, rev(x)), c(y3, rep(0, length(x))),
        col = rgb(0.2, 0.8, 0.2, 0.3), border = NA)

# Add lines
lines(x, y1, col = "navy", lwd = 2)
lines(x, y2, col = "firebrick", lwd = 2)
lines(x, y3, col = "forestgreen", lwd = 2)

# Add data points
points(x, y1, pch = 19, col = "navy")
points(x, y2, pch = 19, col = "firebrick")
points(x, y3, pch = 19, col = "forestgreen")

# Add vertical reference lines
abline(v = 6, col = "gray70", lty = 2)
text(6, 195, "Mid Year", pos = 3, cex = 0.8)

# Add legend
legend("topright",
       legend = c("Product A", "Product B", "Product C"),
       fill = c(rgb(0.2, 0.4, 0.8, 0.5),
                rgb(0.8, 0.2, 0.2, 0.5),
                rgb(0.2, 0.8, 0.2, 0.5)),
       bty = "n")

# Add annotations for key events
text(4, y1[4] + 10, "April Peak",
     cex = 0.7, col = "navy")
```

### Example 8: Segments and Arrows

```r
# Create base plot
plot(c(0, 10), c(0, 10), type = "n",
     main = "Segments and Arrows",
     xlab = "X", ylab = "Y")

# Add point
points(5, 5, pch = 19, cex = 2)

# Add line segment
segments(1, 1, 4, 4, col = "blue", lwd = 2)

# Add arrow showing direction
arrows(5, 3, 5, 7,
       col = "red", lwd = 2,
       angle = 30, code = 2)

# Add bidirectional arrow
arrows(6, 3, 8, 3,
       col = "darkgreen", lwd = 2,
       angle = 30, code = 3,
       length = 0.15)

# Add multiple segments
segments(c(2, 3, 4), c(8, 7, 6),
         c(4, 5, 6), c(8, 7, 6),
         col = terrain.colors(3),
         lwd = 2)

# Annotation with text
text(5, 2, "Center Point", pos = 1)
```

## Best Practices

### 1. Position Legends Carefully

- Use coordinates instead of keywords for precise placement
- Consider using `bty = "n"` for cleaner legends
- Add inset for margins: `inset = c(0, 0)`

### 2. Match Legend to Plot Elements

The legend entries should correspond exactly to plot elements:
- Same `pch` values as used in `points()`
- Same `lty` values as used in `lines()`
- Same colors as used in plot

### 3. Use Vectorized Parameters

For multiple series, use vectors:
```r
colors <- c("red", "blue", "green")
legend("topright", legend = names, fill = colors)
```

### 4. Annotate Sparingly

Too many annotations clutter the plot. Prioritize:
- Key findings or peak values
- Reference lines or thresholds
- Important data points

### 5. Use expression() for Proper Mathematical Typesetting

For publication-quality plots, use:
```r
expression(beta[1], sigma^2, sum(x[i]))
```

### Common Issues

1. Legend covering data: Use `inset` or coordinates
2. Text clipped by margins: Increase `mar` or `oma` parameters
3. Arrows pointing wrong direction: Adjust `code` parameter (1, 2, 3)
4. Legend boxes too large: Use custom `fill` instead of `density`