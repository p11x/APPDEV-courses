# 3D Visualization in R

## Learning Objectives

- Create 3D visualizations using the rgl package
- Use persp() for 3D surface plots
- Understand the coordinate system in 3D plots
- Customize 3D graphics: colors, lighting, viewpoints
- Create animated 3D rotations and export as images

## Theoretical Background

### rgl Package

The `rgl` package provides 3D graphics capabilities for R:
- Uses OpenGL for hardware-accelerated rendering
- Creates interactive 3D scenes that can be rotated and zoomed
- Supports points, lines, surfaces, and shapes
- Can export to various formats including WebGL for HTML

### persp() Function

The base R `persp()` function creates 3D perspective plots of surfaces:
- Takes a matrix of z-values
- Projects onto a 2D plane with perspective
- Returns transformation matrix for adding elements

### 3D Coordinate System

In 3D visualization:
- X and Y form the horizontal plane
- Z represents height/elevation
- Viewpoint defined by azimuth, elevation, and distance

## Code Examples

### Example 1: Basic rgl 3D Scatter Plot

```r
# Load rgl package
library(rgl)

# Create 3D sample data
set.seed(42)
n <- 200
data <- data.frame(
  x = rnorm(n),
  y = rnorm(n),
  z = rnorm(n),
  group = factor(sample(c("A", "B", "C"), n, replace = TRUE))
)

# Basic 3D scatter
plot3d(data$x, data$y, data$z,
       col = as.numeric(data$group),
       size = 5,
       xlab = "X", ylab = "Y", zlab = "Z",
       main = "3D Scatter Plot")

# Add legend
legend3d("topright", legend = levels(data$group),
         col = 1:3, pch = 16)
```

### Example 2: Surface Plot with persp()

```r
# Create a surface from function
x <- seq(-2, 2, length.out = 30)
y <- seq(-2, 2, length.out = 30)

# Create grid and compute z values
f <- function(x, y) {
  exp(-(x^2 + y^2))
}

z <- outer(x, y, f)

# Create perspective plot
persp(x, y, z,
      theta = 30, phi = 30,
      col = "lightblue",
      border = "steelblue",
      xlab = "X", ylab = "Y", zlab = "Z",
      main = "Gaussian Surface",
      shade = 0.5)
```

### Example 3: Adding Elements to persp()

```r
# Create initial surface
x <- seq(0, 3, length.out = 30)
y <- seq(0, 3, length.out = 30)
z <- outer(x, y, function(x, y) sin(x) * cos(y))

# Add perspective with transformation matrix
pmat <- persp(x, y, z,
              theta = 45, phi = 25,
              col = "lightyellow",
              border = "orange",
              xlab = "X", ylab = "Y", zlab = "Z")

# Add points to surface (transformed coordinates)
points <- list(
  x = c(0.5, 1.5, 2.5),
  y = c(0.5, 1.5, 2.5),
  z = c(0.3, 0.2, 0.4)
)
trans <- trans3d(points$x, points$y, points$z, pmat)
points(trans$x, trans$y, pch = 19, col = "red", cex = 2)

# Add lines
lines <- trans3d(c(0, 3), c(0, 3), c(0, 0), pmat)
lines(lines$x, lines$y, col = "blue", lty = 2)
```

### Example 4: Custom 3D Surface with rgl

```r
library(rgl)

# Mathematical surface
x <- seq(-2, 2, length.out = 40)
y <- seq(-2, 2, length.out = 40)
z <- outer(x, y, function(x, y) x^2 - y^2)

# Create surface with rgl
open3d()
bg3d("white")
surface3d(x, y, z, 
           color = rainbow(40),
           smooth = TRUE,
           lit = TRUE)

# Add axes
axes3d()

# Set viewpoint
view3d(theta = 30, phi = 30, fov = 60)

# Add title
title3d(main = "Saddle Surface", 
        xlab = "X", ylab = "Y", zlab = "Z")
```

### Example 5: Colored Surface by Height

```r
library(rgl)

# Create data
x <- seq(-pi, pi, length.out = 50)
y <- seq(-pi, pi, length.out = 50)
z <- outer(x, y, function(x, y) sin(sqrt(x^2 + y^2)))

# Color based on z values
color_vals <- colorRampPalette(c("navy", "blue", 
                                  "white", "red"))(50)

open3d()
surface3d(x, y, z, 
           color = color_vals[cut(z, 50)],
           back = "lines")

# Add lighting
rgl.light(theta = 45, phi = 45, diffuse = 0.8)

axes3d()
title3d("Ripple Surface")
```

### Example 6: 3D Regression Surface

```r
library(rgl)

# Create data with relationship
set.seed(123)
n <- 100
x <- runif(n, 0, 10)
y <- runif(n, 0, 10)
z <- 2 + 0.3 * x + 0.5 * y + rnorm(n, 0, 1)

# Fit model
fit <- lm(z ~ x + y)

# Create grid for surface
x_grid <- seq(0, 10, length.out = 20)
y_grid <- seq(0, 10, length.out = 20)
z_pred <- outer(x_grid, y_grid, 
                function(x, y) predict(fit, 
                           data.frame(x = x, y = y)))

# Plot
open3d()
bg3d("lightgray")

# Add data points
plot3d(x, y, z, col = "blue", size = 5,
       xlab = "X", ylab = "Y", zlab = "Z")

# Add regression surface
surface3d(x_grid, y_grid, z_pred,
          color = "orange", alpha = 0.7)
```

### Example 7: 3D Bar Chart

```r
library(rgl)

# Sample data
x <- 1:4
y <- 1:3
z <- matrix(c(10, 20, 15, 25,
               15, 25, 20, 30,
               12, 18, 22, 28),
             nrow = 4, ncol = 3)

# Create 3D bars
open3d()
bg3d("white")

# Draw each bar
for(i in 1:length(x)) {
  for(j in 1:length(y)) {
    # Draw bar as cylinder-like shape
    rgl.primitive("LINES",
                  as.matrix(rbind(
                    c(x[i] - 0.4, y[j], 0),
                    c(x[i] - 0.4, y[j], z[i,j]),
                    c(x[i] + 0.4, y[j], 0),
                    c(x[i] + 0.4, y[j], z[i,j]),
                    c(x[i] - 0.4, y[j], z[i,j]),
                    c(x[i] + 0.4, y[j], z[i,j])
                  )))
  }
}

# Using simpler approach - cubes
open3d()
for(i in seq_along(x)) {
  for(j in seq_along(y)) {
    cube3d() %>% 
      translate3d(x[i], y[j], z[i,j]/2) %>%
      scale3d(0.8, 0.8, z[i,j]) %>%
      shade3d(col = "steelblue")
  }
}

# Better approach using rods3d
open3d()
bg3d("white")
for(i in seq_along(x)) {
  for(j in seq_along(y)) {
    cylinder3d(
      center = rbind(c(x[i], y[j], 0),
                     c(x[i], y[j], z[i,j])),
      radius = 0.3,
      sides = 20
    ) %>% shade3d(col = "coral")
  }
}
axes3d()
title3d("3D Bar Chart")
```

### Example 8: Interactive 3D with plotly

```r
library(plotly)

# Create 3D surface
x <- seq(-2, 2, length.out = 50)
y <- seq(-2, 2, length.out = 50)
z <- outer(x, y, function(x, y) sin(x) * cos(y))

# Interactive 3D surface
fig <- plot_ly(x = x, y = y, z = z,
               type = "surface",
               colors = "RdBu",
               showscale = FALSE)

fig <- fig %>% layout(
  title = "Interactive 3D Surface",
  scene = list(
    xaxis = list(title = "X"),
    yaxis = list(title = "Y"),
    zaxis = list(title = "Z")
  )
)

fig
```

### Example 9: Animated 3D Rotation

```r
library(rgl)

# Create surface
x <- seq(-2, 2, length.out = 30)
y <- seq(-2, 2, length.out = 30)
z <- outer(x, y, function(x, y) exp(-(x^2 + y^2)))

# Create movie
open3d()
bg3d("white")

# Initialize the surface
surface3d(x, y, z, color = "lightblue")

# Function to capture frames
for(i in seq(0, 360, by = 10)) {
  view3d(theta = i, phi = 30)
  Sys.sleep(0.1)
}

# Save as animation
if(require("av")) {
  open3d()
  surface3d(x, y, z, color = "steelblue")
  movie3d(spin3d(axis = c(0, 0, 1), rpm = 10),
          duration = 6, fps = 30,
          film = "animation.mp4")
}
```

### Example 10: Exporting 3D Graphics

```r
library(rgl)

# Create 3D plot
open3d()
x <- rnorm(100)
y <- rnorm(100)
z <- rnorm(100)
plot3d(x, y, z, col = "steelblue", size = 5,
       main = "3D Point Cloud")

# Export as PNG (multiple angles)
for(angle in c(0, 45, 90, 135)) {
  view3d(theta = angle, phi = 30)
  rgl.snapshot(paste0("plot_", angle, ".png"))
}

# Export as WebGL HTML
if(require("htmlwidgets")) {
  saveWidget(rglwidget(), "3d_plot.html",
             selfcontained = TRUE)
}

# Export as OBJ (for 3D printing)
writeOBJ("model.obj")

# Export as STL
writeSTL("model.stl")
```

## Best Practices

### 1. Choose Appropriate 3D Package

- Use `persp()` for simple surface plots and publication figures
- Use `rgl` for interactive 3D manipulation
- Use `plotly` for web-ready interactive visualizations

### 2. Color Mapping

Use meaningful color schemes:
- Height-based coloring for surfaces
- Group-based coloring for scatter plots
- Rainbow or terrain palettes for continuous data

### 3. Lighting and Shading

- Enable shading for surface depth perception
- Adjust light position for desired effect
- Use `smooth = TRUE` for better appearance

### 4. Viewpoint Selection

- Default viewpoint may not show structure
- Adjust `theta` and `phi` to find best angle
- Consider multiple views for complex data

### 5. File Size and Performance

- Reduce grid resolution for web export
- Use `rglwidget()` for HTML output
- Consider downsampling large datasets

### Common Issues

1. Overlapping elements: Use `alpha` for transparency
2. Clipped surfaces: Adjust `zoom` parameter
3. Lighting too dark: Use `rgl.light()` or enable `lit`
4. Export errors: Check file permissions and available formats