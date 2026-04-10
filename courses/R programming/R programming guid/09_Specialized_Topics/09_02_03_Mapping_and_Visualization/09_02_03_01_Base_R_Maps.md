# Base R Maps

## Learning Objectives

- Create maps with base R graphics
- Use map packages for boundaries
- Add points and shapes to maps
- Customize map appearance

## Theory

Base R provides fundamental mapping capabilities through the graphics package. Combined with packages like maps and mapdata, you can create geographic visualizations. For interactive maps, consider leaflet.

## Step-by-Step Guide

### Basic Map Creation

```r
# Install map packages
install.packages(c("maps", "mapdata"))

library(maps)

# US map
map("usa")

# US state map
map("state")

# World map
map("world")
```

### Adding Points

```r
# Add points to map
map("state")
points(x = c(-122, -74), y = c(37, 41),
       pch = 20, col = "red", cex = 2)
```

### Adding Lines

```r
# Add route lines
map("state")
lines(x = c(-122, -100, -74),
      y = c(37, 39, 41),
      lwd = 2, col = "blue")
```

## Code Examples

### Color by Value

```r
library(maps)

# State data
map("state", fill = TRUE, 
    col = heat.colors(50))

# Choropleth map
state_data <- data.frame(
  region = tolower(state.name),
  value = runif(50)
)

# Create color palette
colors <- colorRampPalette(c("white", "red"))(50)

# Map with colors
map("state", fill = TRUE, 
    col = colors[cut(state_data$value, 50)])
```

### Custom Map Regions

```r
# Specific states
map("state", regions = c("california", "oregon", "washington"))

# Counties (requires mapdata)
library(mapdata)
map("county", regions = "california")
```

### Map with Spatial Points

```r
library(sp)

# Create spatial points
coords <- data.frame(x = c(-122, -118, -115), 
                 y = c(37, 34, 36))
coordinates(coords) <- ~x + y

# Set projection
proj4string(coords) <- CRS("+proj=longlat")

# Plot
map("state")
plot(coords, add = TRUE, pch = 20, col = "red")
```

## Best Practices

1. **Choose Colors**: Use appropriate color schemes.

2. **Add Legends**: Include map legends.

3. **Label Regions**: Add labels when needed.

4. **Set Margins**: Adjust margins for presentation.

5. **Consider Projections**: Use appropriate CRS.

## Exercises

1. Create basic US map.

2. Add points for locations.

3. Create choropleth map.

4. Add map legend.

5. Customize colors.

## Additional Resources

- [maps Package](https://cran.r-project.org/web/packages/maps/)
- [mapdata Package](https://cran.r-project.org/web/packages/mapdata/)
- [Base Graphics](https://cran.r-project.org/doc/manuals/R-intro.pdf)