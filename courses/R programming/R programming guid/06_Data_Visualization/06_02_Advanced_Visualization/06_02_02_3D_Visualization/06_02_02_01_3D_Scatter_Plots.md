# 3D Scatter Plots in R

## Learning Objectives

- Create 3D scatter plots using various R packages
- Customize 3D plot appearance and orientation
- Add color, size, and symbol encoding
- Create animated 3D plots
- Understand the mathematics of 3D visualization

## Theoretical Background

### 3D Plotting Packages

1. **rgl**: OpenGL-based 3D graphics
2. **scatterplot3d**: Simple 3D scatter plots
3. **plotly**: Interactive 3D plots
4. **lattice**: 3D lattice plots

### 3D Coordinate System

A 3D scatter plot displays data in three dimensions:
- X-axis: First dimension
- Y-axis: Second dimension  
- Z-axis: Third dimension

The 3D projection is calculated using:
```
x_2d = x_3d * cos(theta) + z_3d * sin(theta)
y_2d = y_3d - (x_3d * sin(theta) + z_3d * cos(theta)) * sin(phi)
```

Where theta is azimuth and phi is elevation angle.

### rgl Package

The rgl package provides OpenGL-based 3D graphics with:
- Real-time rotation and zoom
- Lighting effects
- Texture support
- Export to various formats

## Code Examples

### Example: Basic 3D Scatter with rgl

```r
cat("===== BASIC 3D SCATTER WITH RGL =====\n\n")

library(rgl)

# Generate 3D sample data
set.seed(42)
n <- 200

x <- rnorm(n, mean = 50, sd = 10)
y <- rnorm(n, mean = 100, sd = 20)
z <- rnorm(n, mean = 75, sd = 15)
colors <- rainbow(n)

# Create 3D scatter plot
open3d()
plot3d(x, y, z, 
       col = colors,
       size = 5,
       xlab = "Variable X",
       ylab = "Variable Y",
       zlab = "Variable Z",
       main = "3D Scatter Plot")

# Add bounding box
rgl.bbox(color = "black", emission = "gray")

# Add axes
axis3d(edge = "x--", at = seq(20, 80, 20))
axis3d(edge = "y--", at = seq(40, 160, 40))
axis3d(edge = "z--", at = seq(30, 120, 30))

cat("Created 3D scatter plot with rgl\n")
```

### Example: Colored 3D Scatter by Group

```r
cat("\n===== GROUPED 3D SCATTER =====\n\n")

library(rgl)

# Create data with groups
set.seed(123)
n <- 150

# Three clusters
group1 <- data.frame(
  x = rnorm(50, 20, 5),
  y = rnorm(50, 30, 5),
  z = rnorm(50, 40, 5),
  group = "Cluster A"
)

group2 <- data.frame(
  x = rnorm(50, 40, 5),
  y = rnorm(50, 50, 5),
  z = rnorm(50, 60, 5),
  group = "Cluster B"
)

group3 <- data.frame(
  x = rnorm(50, 60, 5),
  y = rnorm(50, 70, 5),
  z = rnorm(50, 80, 5),
  group = "Cluster C"
)

df <- rbind(group1, group2, group3)

# Map group to color
df$color <- ifelse(df$group == "Cluster A", "steelblue",
                  ifelse(df$group == "Cluster B", "darkorange", "darkgreen"))

# Plot with groups
open3d()
plot3d(df$x, df$y, df$z,
       col = df$color,
       size = 6,
       pch = c(19, 17, 15)[as.factor(df$group)],
       xlab = "X", ylab = "Y", zlab = "Z",
       main = "Three-Cluster 3D Data")

# Add legend
legend3d("topright", legend = c("Cluster A", "Cluster B", "Cluster C"),
         col = c("steelblue", "darkorange", "darkgreen"),
         pch = c(19, 17, 15))

# Add planes at median
abclines3d(20, 30, 40, dir = c(1, 0, 0), col = "gray50", lty = 2)
abclines3d(20, 30, 40, dir = c(0, 1, 0), col = "gray50", lty = 2)
abclines3d(20, 30, 40, dir = c(0, 0, 1), col = "gray50", lty = 2)

cat("Created grouped 3D scatter plot\n")
```

### Example: 3D Scatter with Size Encoding

```r
cat("\n===== SIZE-ENCODED 3D SCATTER =====\n\n")

library(rgl)

# Data with continuous variable for size
set.seed(456)
n <- 100

df <- data.frame(
  x = runif(n, 0, 100),
  y = runif(n, 0, 100),
  z = runif(n, 0, 100),
  value = runif(n, 10, 50),
  category = sample(c("A", "B", "C"), n, replace = TRUE)
)

# Size based on value
sizes <- (df$value - min(df$value)) / (max(df$value) - min(df$value)) * 10 + 2

# Color based on category
colors <- ifelse(df$category == "A", "red",
                ifelse(df$category == "B", "blue", "green"))

open3d()
plot3d(df$x, df$y, df$z,
       size = sizes,
       col = colors,
       xlab = "Feature 1", ylab = "Feature 2", zlab = "Feature 3",
       main = "3D Scatter with Size Encoding")

# Add grid
grid3d("x", at = NULL)
grid3d("y", at = NULL)
grid3d("z", at = NULL)

# Add orientation
aspect3d(1, 1, 1)

cat("Created size-encoded 3D scatter plot\n")
```

### Real-World Example: Customer Segmentation

```r
cat("\n===== CUSTOMER SEGMENTATION 3D =====\n\n")

library(rgl)

# Simulated customer data
set.seed(789)
n <- 300

customers <- data.frame(
  recency = runif(n, 1, 365),      # Days since last purchase
  frequency = rpois(n, 5),         # Number of purchases
  monetary = rnorm(n, 500, 150),   # Total spend
  satisfaction = runif(n, 1, 10), # Satisfaction score
  segment = sample(c("VIP", " "Regular", "At Risk", "Churned"), 
                   n, replace = TRUE,
                   prob = c(0.15, 0.45, 0.25, 0.15))
)

# Map segments to colors
seg_colors <- c("VIP" = "gold", 
                "Regular" = "steelblue", 
                "At Risk" = "darkorange", 
                "Churned" = "darkred")

# Create 3D RFM plot
open3d()
plot3d(customers$recency, 
       customers$frequency, 
       customers$monetary,
       col = seg_colors[customers$segment],
       size = customers$satisfaction / 2,
       pch = 19,
       xlab = "Recency (days)",
       ylab = "Frequency (purchases)",
       zlab = "Monetary ($)",
       main = "RFM Customer Segmentation")

legend3d("topright", 
        legend = c("VIP", "Regular", "At Risk", "Churned"),
        col = seg_colors,
        pch = 19)

cat("Created customer RFM segmentation 3D plot\n")
```

### Real-World Example: Quality Control Visualization

```r
cat("\n===== QUALITY CONTROL 3D =====\n\n")

library(rgl)

# Simulated manufacturing quality data
set.seed(321)
n <- 200

quality <- data.frame(
  weight = rnorm(n, 100, 2),
  diameter = rnorm(n, 50, 1.5),
  height = rnorm(n, 25, 1),
  temperature = rnorm(n, 200, 10),
  pressure = rnorm(n, 100, 5),
  defect = ifelse(runif(n) < 0.05, "Defective", "OK")
)

# Color: green for OK, red for defective
quality$color <- ifelse(quality$defect == "OK", "steelblue", "red")

# Size: larger for defective
quality$size <- ifelse(quality$defect == "OK", 4, 10)

open3d()
plot3d(quality$weight, 
       quality$diameter, 
       quality$height,
       col = quality$color,
       size = quality$size,
       pch = ifelse(quality$defect == "OK", 19, 4),
       xlab = "Weight (g)",
       ylab = "Diameter (mm)",
       zlab = "Height (mm)",
       main = "Quality Control 3D Analysis")

# Add control limits
bbox3d(color = "gray50", emission = "gray50")

# Add defective legend
legend3d("topright", 
        legend = c("OK", "Defective"),
        col = c("steelblue", "red"),
        pch = c(19, 4))

cat("Created quality control 3D visualization\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Limit points** to 500-1000 for performance
2. **Use color and size** to encode additional variables
3. **Add rotation** for better perspective
4. **Include axis labels** for interpretation
5. **Use alpha** for dense point clouds

### Common Issues

1. **Slow rendering**: Reduce point count or use scatterplot3d
2. **Points overlapping**: Use transparency (alpha)
3. **Orientation unclear**: Always label axes
4. **Export issues**: Use appropriate device for format

### Save 3D Plots

```r
# Save as PNG
rgl.postscript("plot.png", fmt = "png")

# Save as PDF
rgl.postscript("plot.pdf", fmt = "pdf")

# Save as OBJ for 3D printing
writeOBJ("model.obj")
```

## Related Concepts

- `scatterplot3d` - Simple 3D scatter
- `plotly` - Interactive 3D with hover
- `rglwidget()` - For Shiny integration
- `persp()` - 3D surface plots

## Exercise Problems

1. Create a 3D scatter plot of three correlated variables
2. Add color encoding based on a fourth variable
3. Create grouped 3D scatter with different point types
4. Implement interactive rotation in rgl
5. Export a 3D plot as an image file