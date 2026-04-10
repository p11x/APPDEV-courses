# Plot Customization in Base R

## Learning Objectives

- Master the `par()` function for global graphics parameters
- Customize colors, linetypes, point styles (pch), and margins
- Understand the difference between local and global graphical parameters
- Create publication-ready plots with proper formatting

## Theoretical Background

### The par() Function

The `par()` function in R is the primary mechanism for controlling graphical parameters. It operates on the graphics device level, making its settings global for all subsequent plots until changed. This differs from local parameters passed directly to plotting functions like `plot()`, which affect only that specific visualization.

### Global vs Local Parameters

There are two ways to control graphics appearance in base R:

1. **Global parameters** via `par()`: Affect all subsequent plots
2. **Local parameters** passed to plotting functions: Affect only that specific plot

Some parameters can only be set globally (read-only parameters like `mar`, `mfrow`, `oma`), while others like `col`, `lwd`, `pch` can be set either way.

### Color Specification

Colors in R can be specified in multiple ways:
- Named colors: `"red"`, `"blue"`, `"forestgreen"`
- Hexadecimal: `"#FF0000"`, `"#0000FF"`
- RGB function: `rgb(255, 0, 0, maxColorValue = 255)`
- Color palette functions: `rainbow()`, `heat.colors()`, `terrain.colors()`

### Point Styles (pch)

The `pch` parameter controls point symbols. Values 0-25 represent different shapes:
- 0-18: Various geometric shapes
- 19-25: Filled shapes with optional colors
- Character: Any single character can be used as a point symbol

### Linetypes (lty)

Line types can be specified by integer (0-6) or string:
- 0: blank
- 1: solid
- 2: dashed
- 3: dotted
- 4: dotdash
- 5: longdash
- 6: twodash

### Margin Control

Margins are controlled by two parameter sets:
- `mar`/`mai`: Plot margins (inner margins)
- `oma`/`omi`: Outer margins (outside the plot area)

The order for both is: bottom, left, top, right. `mar` uses lines of text, `mai` uses inches.

## Code Examples

### Example 1: Basic Plot Customization

```r
# Save original par settings to restore later
opar <- par(no.readonly = TRUE)

# Create sample data
x <- 1:20
y <- x + rnorm(20, 0, 3)

# Customized scatter plot
plot(x, y,
     main = "Customized Scatter Plot",
     xlab = "X Axis Label",
     ylab = "Y Axis Label",
     col = "steelblue",
     pch = 16,           # Filled circle points
     cex = 1.5,          # Point size 1.5x default
     lwd = 2)            # Line width

# Restore original settings
par(opar)
```

### Example 2: Using par() for Global Settings

```r
# Set global parameters for multiple plots
par(mfrow = c(2, 2),    # 2x2 plot layout
    mar = c(5, 4, 4, 2) + 0.1,  # Custom margins
    bg = "white",       # Background color
    col.main = "darkblue",    # Title color
    col.lab = "darkgray",    # Axis label color
    font.main = 2)     # Bold title font

# Plot 1: Line plot
plot(x, y, type = "l",
     main = "Line Plot",
     col = "red",
     lty = 2,
     lwd = 2)

# Plot 2: Points with different pch
plot(x, y, pch = 21,
     main = "Different Point Style",
     bg = "lightblue")

# Plot 3: Multiple series
y2 <- x * 1.5 + rnorm(20, 0, 2)
plot(x, y, type = "o",
     main = "Multiple Series",
     col = "blue",
     pch = 19,
     ylim = range(c(y, y2)))
lines(x, y2, col = "red", pch = 17, type = "o")

# Plot 4: Bar plot
barplot(c(10, 25, 15, 30),
        main = "Bar Plot",
        col = terrain.colors(4),
        names.arg = c("Q1", "Q2", "Q3", "Q4"))
```

### Example 3: Color Palettes and Customization

```r
# Demonstrating color generation functions
par(mfrow = c(2, 2), mar = c(4, 4, 3, 1))

# Rainbow colors
barplot(rep(1, 7), col = rainbow(7),
        main = "Rainbow Palette")

# Heat colors
barplot(rep(1, 5), col = heat.colors(5),
        main = "Heat Palette")

# Custom RGB colors
custom_colors <- rgb(
  c(139, 69, 70),
  c(19, 19, 19),
  c(19, 19, 19),
  maxColorValue = 255
)
barplot(rep(1, 3), col = custom_colors,
        main = "Custom RGB Colors")

# Using colorRampPalette for gradient
gradient <- colorRampPalette(c("navy", "white", "firebrick"))(10)
barplot(rep(1, 10), col = gradient,
        main = "Gradient Palette")
```

### Example 4: Advanced Margin Control

```r
# Reset par
par(opar)

# Create plot with outer margins for multi-line titles
par(oma = c(3, 3, 3, 1),  # Outer margins in lines
    mar = c(5, 4, 4, 2)) # Inner margins

# Create base plot
plot(x, y, type = "n",
     xlab = "X Variable",
     ylab = "Y Variable",
     main = "Main Title\nSubtitle")

# Add grid
grid(col = "lightgray", lty = "dotted")

# Add data
points(x, y, pch = 19, col = "blue")

# Add text in outer margins
mtext("Outer Title", side = 3, line = 1,
      outer = TRUE, cex = 1.5, font = 2)
mtext("Left Outer Label", side = 2, line = 1,
      outer = TRUE, cex = 1.2)
```

### Example 5: Complete Publication-Ready Plot

```r
# Publication-ready plot setup
par(family = "serif",      # Serif font for publication
    bg = "white",          # White background
    mar = c(5, 5, 4, 2) + 0.1,
    cex.lab = 1.2,         # Axis label size
    cex.axis = 1.0,        # Axis tick label size
    cex.main = 1.3)        # Title size

# Create data
set.seed(42)
data_x <- 1:30
data_y <- 2 * data_x + rnorm(30, 0, 5)

# Main plot
plot(data_x, data_y,
     type = "p",
     pch = 19,
     col = rgb(0.2, 0.4, 0.6, 0.8),  # Semi-transparent blue
     cex = 1.2,
     xlab = expression(italic(X) ~ "Variable"),
     ylab = expression(italic(Y) ~ "Response"),
     main = "Linear Relationship with Noise")

# Add regression line
abline(lm(data_y ~ data_x),
       col = "firebrick",
       lty = 1,
       lwd = 2)

# Add legend
legend("topleft",
       legend = c("Data Points", "Linear Fit"),
       pch = c(19, NA),
       lty = c(NA, 1),
       col = c(rgb(0.2, 0.4, 0.6, 0.8), "firebrick"),
       bty = "n")

# Add confidence band (simplified)
conf_int <- predict(lm(data_y ~ data_x),
                    interval = "confidence")
lines(data_x, conf_int[, 2], lty = 2, col = "gray50")
lines(data_x, conf_int[, 3], lty = 2, col = "gray50")
```

## Best Practices

### 1. Save and Restore Par Settings

Always save original settings before making changes and restore them after:

```r
# Good practice
opar <- par(no.readonly = TRUE)
# ... your plotting code ...
par(opar)  # Restore original settings
```

### 2. Use Vectorized Colors for Groups

When plotting multiple groups, use color vectors:

```r
groups <- factor(rep(c("A", "B", "C"), each = 10))
colors <- c("red", "blue", "green")[groups]
plot(x, y, col = colors, pch = 19)
```

### 3. Choose Appropriate pch Values

- Use pch 19-25 for filled shapes with border control
- Use pch 21-25 when you need different fill and border colors
- Use character pch when you need custom symbols (e.g., letters as points)

### 4. Consistent Margins for Multi-Plot Layouts

When creating multi-panel figures, ensure consistent margins:

```r
par(mfrow = c(2, 1), mar = c(4, 4, 2, 1))
```

### 5. Use Expression() for Mathematical Symbols

For scientific plotting, use `expression()` for proper mathematical notation:

```r
plot(x, y, xlab = expression(beta), 
     ylab = expression(sigma^2))
```

### 6. Avoid Over-Visualization

Keep plots clean and avoid unnecessary:
- Too many colors or point styles
- Excessive gridlines
- Redundant decorative elements

### Common Pitfalls

1. Forgetting to restore par settings after modification
2. Not considering screen vs. print output (different resolutions)
3. Using default margins that cut off labels
4. Not accounting for transparency when saving to PDF