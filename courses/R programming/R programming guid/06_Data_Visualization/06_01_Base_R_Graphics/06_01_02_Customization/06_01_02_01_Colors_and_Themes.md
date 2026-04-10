# Colors and Themes in R

## Learning Objectives

By the end of this chapter, students will be able to:
- Use built-in color palettes in R
- Create custom color schemes
- Apply themes to ggplot2 visualizations
- Make colorblind-friendly visualizations
- Customize all visual elements
- Create consistent visual styles for publications

## Theoretical Background

Colors and themes are essential for creating effective visualizations. Proper use of color can highlight patterns, group data, and make charts accessible. Themes ensure consistency and professional appearance across all visualizations.

### Color Theory Basics

1. **Hue**: The color itself (red, blue, green)
2. **Saturation**: Intensity of color
3. **Brightness/Lightness**: How light or dark

### Color Types in Data Visualization

1. **Sequential**: Light to dark progression (for ordered data)
2. **Diverging**: Two colors with neutral in middle (for deviations)
3. **Categorical**: Distinct colors (for categorical groups)

### Built-in Palettes

R provides several color palettes:
- **rainbow**: Rainbow spectrum
- **heat.colors**: Yellow to red
- **terrain.colors**: Terrain map colors
- **topo.colors**: Topographic map colors
- **cm.colors**: Cyan-magenta

### ggplot2 Themes

Themes control non-data elements:
- Axis lines and text
- Panel backgrounds
- Legends
- Grid lines
- Titles

## Step-by-Step Implementation

### Step 1: Built-in Colors

```r
# Color names
colors()

# Use by name
plot(1:10, 1:10, col = "steelblue", pch = 19, cex = 2)

# Hex codes
plot(1:10, 1:10, col = c("#FF5733", "#33FF57", "#3357FF"), pch = 19, cex = 2)
```

### Step 2: Color Palettes

```r
# Using rainbow palette
pie(rep(1, 6), labels = 1:6, col = rainbow(6))

# Sequential palette
barplot(1:10, col = heat.colors(10))

# Diverging palette
barplot(1:10, col = cm.colors(10))
```

### Step 3: RColorBrewer Palettes

```r
install.packages("RColorBrewer")
library(RColorBrewer)

# Display all palettes
display.brewer.all()

# Sequential palette
display.brewer.pal(8, "Blues")

# Categorical palette
display.brewer.pal(8, "Set2")

# Using in plot
barplot(1:8, col = brewer.pal(8, "Set2"))
```

### Step 4: ggplot2 Themes

```r
# Default theme
p <- ggplot(mtcars, aes(x = factor(cyl), y = mpg)) + geom_boxplot()

# Various themes
p + theme_bw()       # Black and white
p + theme_minimal()  # Minimal
p + theme_classic()  # Classic
p + theme_dark()     # Dark background

# Custom theme elements
p + theme(
  panel.background = element_rect(fill = "lightgray"),
  panel.grid.major = element_line(color = "white"),
  axis.title = element_text(size = 14, face = "bold")
)
```

### Step 5: Colorblind-Friendly Colors

```r
# Use colorblind-safe palette
safe_colors <- c("#E69F00", "#56B4E9", "#009E73", "#F0E442", "#0072B2", "#D55E00", "#CC79A7")

# Okabe-Ito palette (recommended)
okabe_ito <- c("#E69F00", "#56B4E9", "#009E73", "#F0E442", "#0072B2", "#D55E00", "#CC79A7")

# Use in plot
ggplot(mtcars, aes(x = factor(cyl), y = mpg, fill = factor(am))) +
  geom_boxplot() +
  scale_fill_manual(values = okabe_ito[1:2])
```

### Step 6: Custom Scales in ggplot2

```r
# Continuous color scale
ggplot(iris, aes(x = Sepal.Length, y = Sepal.Width, color = Petal.Length)) +
  geom_point(size = 3) +
  scale_color_gradient(low = "blue", high = "red")

# Discrete color scale
ggplot(mtcars, aes(x = factor(cyl), y = mpg, fill = factor(cyl))) +
  geom_bar(stat = "identity") +
  scale_fill_brewer(palette = "Set2")

# Manual colors
ggplot(iris, aes(x = Species, y = Petal.Length, fill = Species)) +
  geom_boxplot() +
  scale_fill_manual(values = c("setosa" = "#FF5733", 
                               "versicolor" = "#33FF57", 
                               "virginica" = "#3357FF"))
```

## Code Examples

### Example 1: Custom Theme for Publication

This example shows creating a publication-ready theme.

```r
# Custom theme function
pub_theme <- function() {
  theme_bw() %+replace%
    theme(
      plot.title = element_text(size = 16, face = "bold", hjust = 0.5),
      axis.title = element_text(size = 12, face = "bold"),
      axis.text = element_text(size = 10),
      legend.position = "bottom",
      legend.title = element_text(size = 11, face = "bold"),
      panel.grid.major = element_line(color = "gray90"),
      panel.grid.minor = element_blank()
    )
}

# Apply to plot
ggplot(mtcars, aes(x = wt, y = mpg, color = factor(cyl))) +
  geom_point(size = 3) +
  ggtitle("Fuel Efficiency vs Weight") +
  pub_theme()
```

### Example 2: Corporate Color Scheme

This example shows applying corporate colors.

```r
# Corporate colors
corporate <- c(
  "primary" = "#003366",
  "secondary" = "#6D8EAD",
  "accent" = "#FF9900",
  "neutral" = "#666666"
)

# Use in visualization
ggplot(mtcars, aes(x = factor(cyl), y = mpg, fill = factor(cyl))) +
  geom_bar(stat = "identity") +
  scale_fill_manual(values = corporate[1:3]) +
  ggtitle("MPG by Cylinders") +
  theme_minimal()
```

### Example 3: Sequential Color for Continuous Data

This example demonstrates sequential coloring.

```r
# Create heatmap-like visualization
ggplot(iris, aes(x = Sepal.Length, y = Sepal.Width, fill = Petal.Length)) +
  geom_tile() +
  scale_fill_gradientn(colors = terrain.colors(10)) +
  ggtitle("Iris Measurements Heatmap") +
  theme_minimal()
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use Palettes**: Built-in palettes are color-coordinated
2. **Colorblind Testing**: Use okabe-ito or similar
3. **Consistency**: Use same colors for same categories
4. **Meaningful Colors**: Match color to meaning (red = negative)
5. **Limit Colors**: 4-6 for categorical data

### Common Pitfalls

1. **Rainbow Palette**: Can be misleading, avoid for data
2. **Too Many Colors**: Creates visual clutter
3. **Inconsistent Colors**: Confuses readers
4. **3D Colors**: Avoid 3D effects in plots
5. **Ignoring Context**: Colors should match the story

## Performance Considerations

### Color Palettes in ggplot2

| Function | Use Case |
|----------|----------|
| scale_fill_manual | Custom discrete colors |
| scale_fill_brewer | ColorBrewer palettes |
| scale_fill_gradient | Two-color gradient |
| scale_fill_gradientn | Multi-color gradient |
| scale_fill_viridis | Scientific color scales |

### Theme Selection

| Theme | Best For |
|-------|----------|
| theme_bw | Publications, reports |
| theme_minimal | Dashboards, web |
| theme_classic | Traditional journals |
| theme_dark | Dark backgrounds |

## Related Concepts

- **Viridis**: Scientific color scales
- **ggthemes**: Additional themes
- **hrbrthemes**: Typography-focused themes
- **ggthemr**: Complete theme packages

## Exercise Problems

1. **Basic**: Apply different themes to a plot.

2. **Intermediate**: Create a custom color scale.

3. **Advanced**: Make an accessible colorblind-friendly plot.

4. **Real-World Challenge**: Create a consistent theme for reports.

5. **Extension**: Create a custom theme function.