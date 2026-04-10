# 3D Surface Plots in R

## Learning Objectives

- Create 3D surface plots from data or functions
- Use persp() for base R surface visualization
- Implement wireframe plots with lattice
- Add color mapping to surface elevation
- Create interactive 3D surfaces with plotly

## Theoretical Background

### Surface Plot Mathematics

A 3D surface plot represents a function f(x, y) = z over a 2D domain:

- **Grid**: Regular x-y mesh
- **Elevation (z)**: Function value at each grid point
- **Coloring**: Often based on z-value for depth perception

### Surface Plot Functions

1. **persp()**: Base R 3D perspective plot
2. **wireframe()**: lattice package surface plots
3. **surface3d()**: rgl package 3D surfaces
4. **plot_ly()**: Plotly interactive surfaces

### The persp() Function

```
persp(x, y, z, 
      theta = 0, phi = 40,      # View angles
      r = sqrt(3),              # Distance
      scale = TRUE,             # Scale axes
      box = TRUE,               # Draw box
      col = NULL,               # Surface color
      xlab, ylab, zlab)         # Axis labels
```

### Color Mapping

Surface colors can be based on:
- Single color (flat)
- Elevation (gradient from low to high)
- Custom color palette

## Code Examples

### Example: Basic Surface with persp()

```r
cat("===== BASIC SURFACE PLOT =====\n\n")

# Create grid for surface
x <- seq(-2, 2, length.out = 30)
y <- seq(-2, 2, length.out = 30)

# Function to plot (peaks function)
z_func <- function(x, y) {
  sin(sqrt(x^2 + y^2))
}

# Create z matrix
z <- outer(x, y, z_func)

# Basic persp plot
persp(x, y, z,
      theta = 30, phi = 30,
      col = "steelblue",
      xlab = "X", ylab = "Y", zlab = "Z",
      main = "Peaks Function Surface")

cat("Created basic 3D surface with persp\n")
```

### Example: Colored Surface by Elevation

```r
cat("\n===== ELEVATION-COLORED SURFACE =====\n\n")

# Create grid
x <- seq(-3, 3, length.out = 40)
y <- seq(-3, 3, length.out = 40)

# Surface function
z_func <- function(x, y) {
  x^2 + y^2
}

z <- outer(x, y, z_func)

# Create color gradient based on z
n_colors <- 30
colors <- terrain.colors(n_colors)[cut(z, n_colors)]

# Plot with color
persp(x, y, z,
      theta = 45, phi = 30,
      col = colors,
      border = NA,
      shade = 0.5,
      xlab = "X", ylab = "Y", zlab = "Z",
      main = "Paraboloid with Elevation Colors")

cat("Created elevation-colored surface\n")
```

### Example: Multiple Surface Functions

```r
cat("\n===== MULTIPLE SURFACES =====\n\n")

# Create data for multiple surfaces
par(mfrow = c(1, 2), oma = c(1, 1, 2, 1))

# Grid
x <- seq(-2, 2, length.out = 25)
y <- seq(-2, 2, length.out = 25)

# Function 1: Saddle
z1 <- outer(x, y, function(x, y) x^2 - y^2)
colors1 <- heat.colors(25)[cut(z1, 25)]

persp(x, y, z1, theta = 30, phi = 30,
      col = colors1, shade = 0.5,
      main = "Saddle Surface")

# Function 2: Gaussian
z2 <- outer(x, y, function(x, y) exp(-(x^2 + y^2)))
colors2 <- cm.colors(25)[cut(z2, 25)]

persp(x, y, z2, theta = -30, phi = 30,
      col = colors2, shade = 0.5,
      main = "Gaussian Surface")

par(mfrow = c(1, 1))

cat("Created multiple surface plots\n")
```

### Real-World Example: Terrain Visualization

```r
cat("\n===== TERRAIN SURFACE =====\n\n")

# Simulated terrain data
set.seed(42)
n <- 50

# Create grid
x <- seq(0, 10, length.out = n)
y <- seq(0, 10, length.out = n)

# Generate terrain-like surface (Perlin-like noise simulation)
z <- matrix(0, n, n)
for (k in 1:5) {
  freq <- 0.5 * k
  z <- z + sin(x * freq) * cos(y * freq) * (1/k)
}
z <- z * 10 + 20  # Scale and offset

# Color by elevation
elev_colors <- terrain.colors(50)[cut(z, 50)]

# Plot terrain
persp(x, y, z,
      theta = 135, phi = 30,
      col = elev_colors,
      shade = 0.6,
      xlab = "West-East", 
      ylab = "South-North",
      zlab = "Elevation (m)",
      main = "Simulated Terrain Surface")

cat("Created terrain visualization\n")
```

### Real-World Example: Temperature Distribution

```r
cat("\n===== TEMPERATURE DISTRIBUTION =====\n\n")

# Simulated heat transfer data
x <- seq(0, 10, length.out = 30)
y <- seq(0, 10, length.out = 30)

# Heat source in center with dissipation
heat_func <- function(x, y) {
  center_x <- 5
  center_y <- 5
  dist <- sqrt((x - center_x)^2 + (y - center_y)^2)
  100 * exp(-dist^2 / 8)
}

z <- outer(x, y, heat_func)

# Temperature color palette
temp_colors <- colorRampPalette(c("blue", "cyan", "yellow", "orange", "red"))(30)

# Plot
persp(x, y, z,
      theta = 45, phi = 20,
      col = temp_colors[cut(z, 30)],
      shade = 0.4,
      xlab = "X Position",
      ylab = "Y Position",
      zlab = "Temperature (°C)",
      main = "Heat Distribution Surface")

cat("Created temperature distribution surface\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Choose appropriate viewing angles**: theta and phi control perspective
2. **Use shade parameter**: Adds depth perception (0-1 range)
3. **Match color to data**: Use appropriate color palette
4. **Add border lines**: Improves surface definition
5. **Set aspect ratios**: Maintain proportional scaling

### Common Issues

1. **Surface too flat**: Adjust z scaling or view angles
2. **Colors washed out**: Use more colors in gradient
3. **Plot cropped**: Adjust margins with par(mar)
4. **Performance slow**: Reduce grid resolution

### Wireframe with lattice

```r
library(lattice)

# Using wireframe for better control
wireframe(z ~ x * y,
          data = data.frame(x, y, z),
          scales = list(arrows = FALSE),
          drape = TRUE,
          col.regions = terrain.colors(100))
```

## Related Concepts

- `persp()` - Base R 3D perspective
- `image()` - 2D heat map representation
- `contour()` - Contour lines
- `plotly` - Interactive surface plots

## Exercise Problems

1. Create a surface plot showing a mathematical function (e.g., sin(x)*cos(y))
2. Add color mapping based on z-elevation
3. Create a surface with multiple peaks and valleys
4. Plot terrain data with appropriate colors
5. Create a heat transfer surface with source/sink