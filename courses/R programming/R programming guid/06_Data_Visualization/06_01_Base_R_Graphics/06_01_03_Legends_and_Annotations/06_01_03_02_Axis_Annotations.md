# Axis Annotations in Base R Graphics

## Learning Objectives

- Create custom axes with axis() function
- Add minor tick marks for enhanced readability
- Format axis labels with different scales and units
- Add axis lines and custom tick styling
- Use axis() for specialized axis configurations

## Theoretical Background

### The axis() Function

The `axis()` function provides fine-grained control over axis appearance:

```
axis(side, at = NULL, labels = TRUE, 
     col = NULL, lty = "solid", lwd = 1,
     tick = TRUE, lty.ticks = "solid", lwd.ticks = 1,
     hadj = NA, padj = NA, ...)
```

### Side Parameters

- side = 1: Bottom axis (X-axis)
- side = 2: Left axis (Y-axis)
- side = 3: Top axis
- side = 4: Right axis

### Key Concepts

1. **Tick marks**: Positions controlled by `at`, labels by `labels`
2. **Axis line**: Controlled by col, lty, lwd parameters
3. **Minor ticks**: Added with minor.tick() from Hmisc or manually
4. **Custom labeling**: Using character vectors or formatting functions

## Code Examples

### Example: Basic Custom Axis

```r
cat("===== BASIC CUSTOM AXIS =====\n\n")

# Create plot without default axes
x <- 1:12
y <- c(120, 145, 130, 160, 175, 190, 210, 195, 230, 245, 260, 280)

plot(x, y, type = "l", col = "steelblue", lwd = 2,
     xaxt = "n", yaxt = "n",    # Suppress default axes
     xlab = "", ylab = "",
     main = "Monthly Revenue")

# Custom X-axis with month names
axis(side = 1,
     at = 1:12,
     labels = month.abb,
     col = "darkgray",
     lwd = 2,
     padj = 0.5)

# Custom Y-axis with dollar format
axis(side = 2,
     at = seq(100, 300, 50),
     labels = paste0("$", seq(100, 300, 50), "K"),
     col = "darkgray",
     lwd = 2,
     hadj = 0.5)

# Add box
box(col = "darkgray", lwd = 2)

cat("Created custom formatted axes\n")
```

### Example: Multiple Axis Scales

```r
cat("\n===== MULTIPLE AXIS SCALES =====\n\n")

# Create sample data with different scales
days <- 1:30
temperature <- 20 + 5 * sin(seq(0, 2*pi, length.out = 30)) + rnorm(30, 0, 1)
humidity <- 60 + 20 * cos(seq(0, 2*pi, length.out = 30)) + rnorm(30, 0, 2)

# Plot with dual Y-axes
plot(days, temperature, type = "l", col = "darkred", lwd = 2,
     xlab = "Day", ylab = "Temperature (°C)",
     main = "Weather Conditions")

# Add second Y-axis for humidity
par(new = TRUE)
plot(days, humidity, type = "l", col = "darkblue", lwd = 2,
     xaxt = "n", yaxt = "n",
     xlab = "", ylab = "")
axis(side = 4, at = seq(40, 100, 10),
     labels = paste0(seq(40, 100, 10), "%"),
     col = "darkblue", col.ticks = "darkblue",
     las = 1)
mtext("Humidity (%)", side = 4, line = 2, col = "darkblue")

# Reset par
par(new = FALSE)

# Add legend
legend(x = "topleft",
       legend = c("Temperature", "Humidity"),
       col = c("darkred", "darkblue"),
       lty = 1, lwd = 2,
       bty = "n")

cat("Created dual Y-axis plot\n")
```

### Example: Scientific Axis Formatting

```r
cat("\n===== SCIENTIFIC AXIS FORMATTING =====\n\n")

# Create exponential data
x <- 1:20
y <- 10^(x/5)  # Exponential growth

plot(x, y, type = "b", col = "purple", pch = 19,
     log = "y",                    # Logarithmic Y-axis
     xlab = "Observation",
     ylab = "Value (log scale)",
     main = "Exponential Growth Data")

# Custom log axis ticks
axis(side = 2,
     at = c(10, 100, 1000, 10000, 100000),
     labels = c("10", "100", "1K", "10K", "100K"),
     col = "darkgray", lwd = 2,
     las = 1)

cat("Created log scale axis\n")
```

### Real-World Example: Financial Axis Labels

```r
cat("\n===== FINANCIAL AXIS LABELS =====\n\n")

# Stock price data
dates <- as.Date("2024-01-01") + 0:90
prices <- 100 + cumsum(rnorm(91, 0.2, 2))

# Plot without axes
plot(dates, prices, type = "l", col = "darkgreen",
     xaxt = "n", yaxt = "n",
     xlab = "", ylab = "",
     main = "Stock Price Movement - 2024")

# Custom date axis (every 2 weeks)
date_breaks <- seq(as.Date("2024-01-01"), as.Date("2024-04-01"), by = "14 days")
axis(side = 1,
     at = as.numeric(dates[seq(1, 91, 14)]),
     labels = format(date_breaks, "%b %d"),
     col = "darkgray",
     padj = 0.8)

# Price axis with dollar format
price_ticks <- seq(80, 120, 5)
axis(side = 2,
     at = price_ticks,
     labels = paste0("$", price_ticks),
     col = "darkgray",
     las = 1)

# Add grid
grid(nx = NA, ny = NULL, col = "gray80", lty = "dotted")

# Box
box(col = "darkgray")

cat("Created financial chart with date axis\n")
```

### Real-World Example: Demographic Scale Axis

```r
cat("\n===== DEMOGRAPHIC SCALE AXIS =====\n\n")

# Population data (in millions)
years <- seq(1970, 2020, by = 5)
population <- c(4.4, 5.0, 5.6, 6.1, 6.8, 7.3, 7.9, 8.4, 9.0, 9.5, 10.0)

plot(years, population, type = "b", col = "darkorange", pch = 19,
     xaxt = "n", yaxt = "n",
     xlab = "", ylab = "",
     main = "Population Growth (in millions)")

# X-axis with decade markers
decades <- seq(1970, 2020, by = 10)
axis(side = 1,
     at = decades,
     labels = paste0(decades, "s"),
     col = "darkgray", lwd = 2)

# Y-axis with million notation
pop_ticks <- seq(4, 10, by = 1)
axis(side = 2,
     at = pop_ticks,
     labels = paste0(pop_ticks, "M"),
     col = "darkgray", lwd = 2,
     las = 1)

# Minor tick marks for 5-year intervals
minor_ticks <- setdiff(seq(1970, 2020, by = 5), decades)
axis(side = 1, at = minor_ticks, labels = FALSE, 
     tcl = -0.3, col = "gray60")

box(col = "darkgray")

cat("Created population chart with decade scale\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use xaxt = "n"** to suppress default axes before adding custom ones
2. **Use las = 1** to make all axis labels horizontal for readability
3. **Match col.ticks** to label color for visual consistency
4. **Use padj and hadj** for precise label positioning
5. **Add grid lines** after axes for better readability

### Common Issues

1. **Overlapping labels**: Increase tick spacing or use angle rotation
2. **Axis not appearing**: Ensure par(new = TRUE) or correct coordinate system
3. **Labels clipped**: Adjust plot margins with par(mar = )
4. **Dual axis scaling**: Use consistent scaling or normalize data

### Axis Label Positioning

```r
# Adjust label position
axis(side = 1, 
     at = 1:12, 
     labels = month.abb,
     padj = 0.5,    # Vertical adjustment (0 = on line, >0 = below)
     hadj = 0.5)    # Horizontal adjustment
```

## Related Concepts

- `title()` - Adding axis labels and plot titles
- `legend()` - Adding legends for multiple series
- `grid()` - Adding reference grid lines
- `par()` - Setting graphical parameters including margins

## Exercise Problems

1. Create a plot with the X-axis showing quarter labels (Q1, Q2, Q3, Q4) for 4 years
2. Add a secondary X-axis at the top showing month abbreviations
3. Create a log-scale plot with properly formatted axis labels
4. Add minor tick marks between major tick marks on both axes
5. Design a financial chart with currency formatting on the Y-axis ($1K, $2K, etc.)