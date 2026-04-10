# Mapping and Visualization in R

## Title
Creating Interactive and Static Maps with leaflet, tmap, and mapview

## Objectives
- Create interactive maps using leaflet
- Build static thematic maps with tmap
- Use mapview for quick interactive visualization
- Customize map appearance and add layers

## Introduction

R provides excellent packages for mapping and spatial visualization. This guide covers leaflet (interactive), tmap (static thematic), and mapview (quick interactive).

## Installing Required Packages

```r
# Install packages
install.packages("leaflet")
install.packages("tmap")
install.packages("mapview")
install.packages("sf")
install.packages("raster")

# Load libraries
library(leaflet)
library(tmap)
library(mapview)
library(sf)
library(raster)
```

## Basic leaflet Maps

leaflet creates interactive JavaScript maps that can be viewed in RStudio or saved as HTML.

```r
# Create basic leaflet map
leaflet() %>%
  addTiles() %>%
  addMarkers(lng = -0.09, lat = 51.505, 
             popup = "London, UK")

# Add multiple markers from data frame
# Define locations
locations <- data.frame(
  name = c("London", "Paris", "Berlin"),
  lat = c(51.5074, 48.8566, 52.5200),
  lng = c(-0.1278, 2.3522, 13.4050)
)

# Create map with markers
leaflet(locations) %>%
  addTiles() %>%
  addMarkers(~lng, ~lat, popup = ~name)

# Add circles instead of markers
leaflet(locations) %>%
  addTiles() %>%
  addCircles(~lng, ~lat, radius = 50000, 
             popup = ~name, color = "red")
```

## tile Layers and Basemaps

```r
# Add different tile providers

# Default OpenStreetMap
leaflet() %>% addTiles()

# CartoDB basemaps
leaflet() %>%
  addProviderTiles("CartoDB.Positron") %>%
  addMarkers(lng = -0.09, lat = 51.505)

leaflet() %>%
  addProviderTiles("CartoDB.DarkMatter")

leaflet() %>%
  addProviderTiles("Esri.WorldImagery")

# Stamen tiles (for artistic maps)
leaflet() %>%
  addProviderTiles("Stamen.Toner") %>%
  addMarkers(lng = -0.09, lat = 51.505,
             popup = "London", 
             label = "London")

# Add layer control
leaflet() %>%
  addProviderTiles("CartoDB.Positron") %>%
  addProviderTiles("Esri.WorldImagery", 
                   group = "Satellite") %>%
  addMarkers(lng = -0.09, lat = 51.505,
             group = "Cities") %>%
  addMarkers(lng = 2.3522, lat = 48.8566,
             group = "Cities") %>%
  layersControl(
    baseGroups = c("Street Map", "Satellite"),
    overlayGroups = c("Cities")
  )
```

## tmap for Thematic Maps

tmap creates publication-quality static maps.

```r
# Create sample sf data
# Load built-in data if available, or create sample
data("mtq", package = "leaflet")
data("World", package = "tmap")

# Create sample polygon data
sample_data <- data.frame(
  id = 1:4,
  name = c("Area A", "Area B", "Area C", "Area D"),
  value = c(100, 200, 150, 300),
  geometry = st_sfc(
    st_polygon(list(matrix(c(0,0, 1,0, 1,1, 0,1, 0,0), ncol = 2))),
    st_polygon(list(matrix(c(1,0, 2,0, 2,1, 1,1, 1,0), ncol = 2))),
    st_polygon(list(matrix(c(0,1, 1,1, 1,2, 0,2, 0,1), ncol = 2))),
    st_polygon(list(matrix(c(1,1, 2,1, 2,2, 1,2, 1,1), ncol = 2)))
  )
) %>% st_as_sf()

# Basic tmap
tm_shape(sample_data) +
  tm_polygons("value")

# Add borders and style
tm_shape(sample_data) +
  tm_polygons("value", 
             title = "Value",
             border.col = "black",
             border.lwd = 1) +
  tm_layout(title = "Thematic Map")

# Multiple layers
tm_shape(sample_data) +
  tm_polygons("value") +
  tm_shape(sample_data) +
  tm_text("name", size = 0.5)
```

## mapview for Quick Visualization

mapview provides instant interactive maps with minimal code.

```r
# Create sample points
points_sf <- data.frame(
  id = 1:5,
  name = c("A", "B", "C", "D", "E"),
  value = c(10, 20, 15, 25, 30),
  geometry = st_sfc(
    st_point(c(0, 0)),
    st_point(c(1, 1)),
    st_point(c(2, 2)),
    st_point(c(0, 2)),
    st_point(c(2, 0))
  )
) %>% st_as_sf()

# Basic mapview map
mapview(points_sf)

# Mapview with color based on value
mapview(points_sf, zcol = "value")

# Mapview for polygons
polygons_sf <- data.frame(
  id = 1:2,
  name = c("Zone 1", "Zone 2"),
  value = c(100, 200),
  geometry = st_sfc(
    st_polygon(list(matrix(c(0,0, 2,0, 2,2, 0,2, 0,0), ncol = 2))),
    st_polygon(list(matrix(c(2,2, 4,2, 4,4, 2,4, 2,2), ncol = 2)))
  )
) %>% st_as_sf()

mapview(polygons_sf, zcol = "value")

# Combined layers
mapview(points_sf, color = "red") + 
  mapview(polygons_sf, alpha = 0.3)
```

## Customizing Map Appearance

```r
# leaflet custom popup
popup_content <- paste0(
  "<b>Location:</b> ", locations$name, "<br>",
  "<b>Coordinates:</b><br>",
  "Lat: ", locations$lat, "<br>",
  "Lng: ", locations$lng
)

leaflet(locations) %>%
  addTiles() %>%
  addMarkers(~lng, ~lat, 
             popup = popup_content,
             label = ~name)

# Custom colors
my_pal <- colorNumeric(
  palette = "viridis",
  domain = c(10, 30)
)

leaflet(points_sf) %>%
  addTiles() %>%
  addCircleMarkers(fillColor = ~my_pal(value),
                   fillOpacity = 0.8,
                   color = "black",
                   weight = 1,
                   popup = ~paste0("Value: ", value))

# Add legend
leaflet(points_sf) %>%
  addTiles() %>%
  addCircleMarkers(fillColor = ~my_pal(value),
                   fillOpacity = 0.8) %>%
  addLegend(pal = my_pal, 
            values = ~value,
            title = "Values")
```

## Exporting Maps

```r
# Save leaflet as HTML
map <- leaflet() %>% addTiles() %>% addMarkers(
  lng = -0.09, lat = 51.505,
  popup = "London"
)
saveWidget(map, "map.html")

# Save tmap as image or PDF
tm_shape(sample_data) +
  tm_polygons("value")

# Save as PNG
tmap_save(tm = last_map(), filename = "map.png")

# Save as PDF
tmap_save(tm = last_map(), filename = "map.pdf")

# Save mapview as HTML
mapview(points_sf, screenshot = TRUE)
```

## Summary

- leaflet creates interactive web maps with JavaScript
- Add markers, circles, polygons to leaflet maps
- Multiple tile providers available via addProviderTiles
- tmap creates static thematic maps for publication
- mapview provides quick interactive visualization
- All packages support sf objects natively
- Maps can be exported as HTML, PNG, or PDF