# ggplot2 Maps

## Learning Objectives

- Create maps with ggplot2
- Use sf package with ggplot2
- Add geoms to maps
- Customize themes for maps

## Theory

ggplot2 now supports sf objects directly through geom_sf(). This integrates seamlessly with grammar of graphics. For interactive maps, use mapview or leaflet.

## Step-by-Step Guide

### Basic ggplot2 Map

```r
library(ggplot2)
library(sf)

# US states as sf
us_states <- st_as_sf(map("state", plot = FALSE, fill = TRUE))

# Plot
ggplot(us_states) +
  geom_sf() +
  theme_minimal()
```

### Adding Points

```r
# Create points
points <- data.frame(
  name = c("Seattle", "Boston"),
  lon = c(-122, -71),
  lat = c(47, 42)
) |>
  st_as_sf(coords = c("lon", "lat"), crs = 4326)

# Add to map
ggplot(us_states) +
  geom_sf() +
  geom_sf(data = points, color = "red", size = 3) +
  theme_minimal()
```

### Choropleth Map

```r
# Add data to states
us_states$data <- runif(51)

# Choropleth
ggplot(us_states) +
  geom_sf(aes(fill = data)) +
  scale_fill_viridis_c() +
  theme_minimal()
```

## Code Examples

### Custom Map Theme

```r
# Custom theme
map_theme <- theme_minimal() +
  theme(
    panel.grid = element_blank(),
    axis.text = element_blank(),
    plot.background = element_rect(fill = "lightblue")
  )

ggplot(us_states) +
  geom_sf(fill = "white") +
  map_theme
```

### mapview for Interactive Maps

```r
library(mapview)

# Interactive map
mapview(us_states, zcol = "data")

# Add points
mapview(us_states) + points
```

### Using rnaturalearth

```r
library(rnaturalearth)

# Get world data
world <- ne_countries(returnclass = "sf")

# Plot
ggplot(world) +
  geom_sf(aes(fill = pop_est)) +
  scale_fill_viridis_c()
```

## Best Practices

1. **Use sf Objects**: ggplot2 works natively with sf.

2. **Set CRS**: Ensure consistent coordinate system.

3. **Choose Colors**: Use appropriate color scales.

4. **Add Labels**: Include informative labels.

5. **Consider Interactivity**: Use mapview or leaflet for interactive.

## Exercises

1. Create basic map with ggplot2.

2. Add points to map.

3. Create choropleth.

4. Customize theme.

5. Create interactive map.

## Additional Resources

- [ggplot2](https://ggplot2.tidyverse.org/)
- [sf Package](https://r-spatial.github.io/sf/)
- [mapview](https://r-spatial.github.io/mapview/)
- [rnaturalearth](https://github.com/ropensci/rnaturalearth)