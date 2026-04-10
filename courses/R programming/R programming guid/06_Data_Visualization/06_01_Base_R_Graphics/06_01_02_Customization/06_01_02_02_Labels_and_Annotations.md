# Labels and Annotations in Base R Graphics

## Learning Objectives

- Add and customize axis labels and titles
- Use mtext() and text() for custom text placement
- Add mathematical annotations using expression()
- Create multi-line labels and text annotations
- Control text appearance with font properties

## Theoretical Background

### Text Functions in Base R

Base R provides several functions for adding text to plots:

1. **title()**: Adds main title, axis labels, and subtitle
2. **text()**: Adds text at specified coordinates
3. **mtext()**: Adds text in the margin
4. **axis()**: Creates custom axes with labels

### Text Appearance Properties

```
font-family: 1 (sans-serif), 2 (serif), 3 (monospace), 4 (symbol)
font-style:  1 (normal), 2 (italic), 3 (bold), 4 (bold-italic)
font-size:  cex (character expansion factor)
```

### Mathematical Expressions

Mathematical notation uses `plotmath` syntax:
- `sqrt(x)` for square root
- `frac(a, b)` for fractions
- `italic(x)` for italics
- `hat(x)` for circumflex
- `paste(a, b)` for concatenation

## Code Examples

### Example: Custom Title and Axis Labels

```r
cat("===== CUSTOM LABELS AND TITLES =====\n\n")

# Sample data for plotting
x <- 1:20
y <- c(3, 5, 4, 7, 6, 9, 8, 11, 10, 13, 12, 15, 14, 17, 16, 19, 18, 21, 20, 23)

# Create basic plot without labels
plot(x, y, type = "b", col = "blue", pch = 19)

# Add custom title and axis labels using title()
title(main = list("Sales Performance",
                font = 4,    # Bold
                cex = 1.5),   # Size 1.5x
     xlab = list("Quarter",
                font = 2),    # Italic
     ylab = list("Revenue ($K)",
                font = 2),
     sub = list("Source: Annual Report 2024",
                cex = 0.8))

cat("Added custom title, axis labels, and subtitle\n")
```

### Example: Adding Text Annotations

```r
cat("\n===== TEXT ANNOTATIONS =====\n\n")

# Create sample plot
plot(x, y, type = "b", col = "darkgreen", pch = 19,
     main = "Monthly Revenue", xlab = "Month", ylab = "Revenue")

# Add text at specific coordinates using text()
text(x = 5, y = 4, labels = "Target: $4K",
     col = "red", font = 2, pos = 1)

text(x = 15, y = 16, labels = "Peak Month",
     col = "blue", font = 3, pos = 3)

# Add text in margins using mtext()
mtext("Confidential", side = 4, line = 0, adj = 0.5,
      col = "gray50", cex = 0.7)

cat("Added text annotations at specific points\n")
```

### Example: Mathematical Expressions

```r
cat("\n===== MATHEMATICAL EXPRESSIONS =====\n\n")

# Create plot for mathematical annotation
x_norm <- seq(-3, 3, length.out = 100)
y_norm <- dnorm(x_norm)

plot(x_norm, y_norm, type = "l", col = "purple",
     xlab = "x", ylab = "Probability Density")

# Add expression using expression()
title(main = expression(paste("Normal Distribution: ", 
                             f(x) == frac(1, sqrt(2*pi*sigma^2)) * 
                             e^{-frac((x-mu)^2, 2*sigma^2)})))

# Add formula annotation
text(x = 0, y = 0.35, 
     labels = expression(mu == 0 * "," ~ sigma == 1),
     cex = 1.2, col = "darkblue")

# Add another mathematical annotation
text(x = -2, y = 0.15,
     labels = expression(integral(-infinity, infinity, 
                                  f(x) * dx) == 1),
     cex = 1, col = "darkred")

cat("Added mathematical expressions as annotations\n")
```

### Real-World Example: Sales Dashboard Labels

```r
cat("\n===== SALES DASHBOARD LABELS =====\n\n")

# Sample sales data
quarters <- c("Q1", "Q2", "Q3", "Q4")
revenue <- c(120, 150, 135, 180)
expenses <- c(80, 95, 90, 110)

# Create bar plot
barplot(revenue, names.arg = quarters, col = "steelblue",
        ylim = c(0, 200), xlab = "Quarter", ylab = "Amount ($K)")

# Add title with formatting
title(main = list("2024 Quarterly Financial Summary",
                 font = 4, cex = 1.6),
      xlab = "Fiscal Quarter",
      ylab = "Revenue (thousands $)")

# Add annotations for key values
text(x = 0.7, y = 125, labels = "$120K",
     col = "white", font = 2, pos = 3)
text(x = 1.7, y = 155, labels = "$150K",
     col = "white", font = 2, pos = 3)
text(x = 2.7, y = 140, labels = "$135K",
     col = "white", font = 2, pos = 3)
text(x = 3.7, y = 185, labels = "$180K",
     col = "white", font = 2, pos = 3)

# Add expense annotation
text(x = 2, y = 85, labels = "Expenses Trend",
     col = "darkred", font = 3)

cat("Created sales dashboard with detailed labels\n")
```

### Real-World Example: Scientific Plot Labels

```r
cat("\n===== SCIENTIFIC PLOT LABELS =====\n\n")

# Simulated enzyme kinetics data
substrate_conc <- c(0.1, 0.2, 0.5, 1.0, 2.0, 5.0, 10.0)
reaction_rate <- c(2.1, 3.8, 7.5, 11.2, 15.8, 18.9, 19.8)

# Create plot
plot(substrate_conc, reaction_rate, type = "p", col = "darkgreen",
     pch = 19, cex = 1.2, xlab = "[S] (mM)", ylab = "v (mol/L/min)")

# Add Michaelis-Menten equation title
title(main = expression(paste("Michaelis-Menten Kinetics: ",
                             v == frac(Vmax * [S], Km + [S]))))

# Label key points
text(x = 1, y = 11.5, labels = "Km ≈ 1 mM",
     col = "darkred", font = 3, pos = 4)

text(x = 5, y = 19.5, labels = "Vmax ≈ 20",
     col = "darkblue", font = 3, pos = 4)

# Add axis annotation for saturable region
mtext("Saturable", side = 4, line = 0.5, at = 10,
      col = "gray50", las = 2)

cat("Created scientific plot with proper notation\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use expression()** for mathematical notation - provides proper rendering
2. **Set pos parameter** in text() for consistent positioning relative to point
3. **Use cex for scaling** to adjust text size proportionally
4. **Add subtitle** for additional context (parameter: sub in title())
5. **Use font specifications** consistently across plots

### Common Issues

1. **Text overlapping**: Adjust with pos, offset, or plot margins
2. **Expression not rendering**: Use expression() wrapper, not paste()
3. **Special characters**: Use unicode or expression() for Greek letters
4. **Margin too small**: Increase with par(mar = c(bottom, left, top, right))

### Setting Margins for Labels

```r
# Increase top margin for multi-line title
par(mar = c(5, 4, 6, 2))
plot(x, y)
title(main = c("Main Title", "Second Line"))
```

## Related Concepts

- `legend()` - Adding legends (covered in next section)
- `axis()` - Creating custom axes with labels
- `grid()` - Adding grid lines for reference
- `pair()` - Mathematical pair notation in expressions

## Exercise Problems

1. Create a plot showing temperature data over time with proper labels showing units (°C and hours)
2. Add a mathematical annotation showing the mean formula to a histogram
3. Create a multi-line title using expression() with both text and mathematical symbols
4. Add text annotations indicating the maximum and minimum values on a scatter plot
5. Format axis labels using italics and bold for emphasis in a scientific figure