# Spatial Objects in R

## Title
Creating and Managing Spatial Objects with the sp Package

## Objectives
- Understand spatial object classes in the sp package
- Create SpatialPoints, SpatialLines, and SpatialPolygons objects
- Attach attributes to spatial objects
- Handle coordinate reference systems

## Introduction

The sp package provides classes and methods for spatial data in R. It forms the foundation for many other spatial packages in R.

## Installing and Loading the sp Package

```r
# Install required packages
install.packages("sp")
install.packages("rgdal")

# Load libraries
library(sp)
library(rgdal)
```

## Creating SpatialPoints Objects

SpatialPoints represent discrete point locations.

```r
# Create coordinates matrix
coords <- matrix(c(1, 4, 2, 3, 3, 2, 5, 1), 
                ncol = 2, byrow = TRUE)
colnames(coords) <- c("x", "y")

# Create SpatialPoints object
sp_points <- SpatialPoints(coords)
print(sp_points)
summary(sp_points)

# With coordinate reference system (WGS84)
sp_points_ll <- SpatialPoints(coords, 
                              proj4string = CRS("+proj=longlat +datum=WGS84"))
print(sp_points_ll)

# Create SpatialPointsDataFrame (with attributes)
# Define attributes data frame
attributes_df <- data.frame(
  id = c("A", "B", "C", "D"),
  value = c(100, 200, 150, 300),
  category = c("type1", "type2", "type1", "type2")
)

# Create SpatialPointsDataFrame
sp_points_df <- SpatialPointsDataFrame(
  coords = coords,
  data = attributes_df,
  proj4string = CRS("+proj=longlat +datum=WGS84")
)
print(sp_points_df)
summary(sp_points_df)

# Access coordinates and data
coordinates(sp_points_df)
sp_points_df$value
```

## Creating SpatialLines Objects

SpatialLines represent line features.

```r
# Create first line
line1 <- Lines(
  list(Line(coords = matrix(c(0, 0, 1, 1, 2, 0), ncol = 2, byrow = TRUE))),
  ID = "line1"
)

# Create second line
line2 <- Lines(
  list(Line(coords = matrix(c(3, 3, 4, 2, 5, 3), ncol = 2, byrow = TRUE))),
  ID = "line2"
)

# Create SpatialLines object
sp_lines <- SpatialLines(list(line1, line2))
print(sp_lines)
summary(sp_lines)

# Add attributes with SpatialLinesDataFrame
lines_data <- data.frame(
  id = c("line1", "line2"),
  length = c(3.16, 2.83),
  road_type = c("highway", "local")
)

sp_lines_df <- SpatialLinesDataFrame(
  sp = sp_lines,
  data = lines_data,
  match.ID = TRUE
)
print(sp_lines_df)
```

## Creating SpatialPolygons Objects

SpatialPolygons represent area features.

```r
# Create a simple polygon (triangle)
# Three points define a triangle
triangle_coords <- matrix(c(0, 0, 3, 0, 1.5, 3), ncol = 2, byrow = TRUE)
triangle <- Polygon(triangle_coords, hole = FALSE)

# Create Polygons object (can contain multiple polygons)
triangles <- Polygons(list(triangle), ID = "triangle1")

# Create SpatialPolygons
sp_polygons <- SpatialPolygons(list(triangles))
print(sp_polygons)
summary(sp_polygons)

# Create a square
square_coords <- matrix(c(4, 4, 7, 4, 7, 7, 4, 7), ncol = 2, byrow = TRUE)
square <- Polygon(square_coords)

# Combine with triangle
all_polygons <- SpatialPolygons(
  list(
    Polygons(list(triangle), ID = "triangle1"),
    Polygons(list(square), ID = "square1")
  )
)
print(all_polygons)

# Create SpatialPolygonsDataFrame with attributes
poly_data <- data.frame(
  id = c("triangle1", "square1"),
  area = c(4.5, 9),
  type = c("triangle", "square")
)

sp_polygons_df <- SpatialPolygonsDataFrame(
  sp = all_polygons,
  data = poly_data,
  match.ID = TRUE
)
print(sp_polygons_df)
summary(sp_polygons_df)
```

## Coordinate Reference Systems

```r
# Define CRS using proj4string
# WGS84 (lat/lon)
crs_wgs84 <- CRS("+proj=longlat +datum=WGS84")

# UTM Zone 32N (meters)
crs_utm32 <- CRS("+proj=utm +zone=32 +datum=WGS84 +units=m")

# Project coordinates
sp_points_utm <- spTransform(sp_points_df, crs_utm32)
print(sp_points_utm)

# Get CRS of existing object
crs_obj <- CRS(proj4string(sp_points_df))
print(crs_obj)

# Check if CRS is defined
is.projected(sp_points_df)
is.geographic(sp_points_df)
```

## Spatial Bounding Boxes and Centroids

```r
# Get bounding box
bbox(sp_polygons_df)

# Get centroid of polygons
centroids <- coordinates(sp_polygons_df)
print(centroids)

# Buffer around points
library(rgeos)
buffer_points <- gBuffer(sp_points_df, width = 0.5)
plot(buffer_points)
plot(sp_points_df, add = TRUE)
```

## Grid Objects (SpatialPixels and SpatialGrid)

```r
# Create SpatialGrid
# Define grid parameters
grd <- GridTopology(
  cellcentre.offset = c(0, 0),
  cellsize = c(1, 1),
  cells.dim = c(5, 5)
)

# Create SpatialGrid
sp_grid <- SpatialGrid(grd, proj4string = CRS("+proj=longlat"))
print(sp_grid)

# Create SpatialGridDataFrame with values
grid_data <- data.frame(
  value = runif(25),
  category = sample(c("A", "B"), 25, replace = TRUE)
)

sp_grid_df <- SpatialGridDataFrame(
  grid = sp_grid,
  data = grid_data
)
print(sp_grid_df)

# Create SpatialPixels from points
sp_pixels <- SpatialPixels(sp_points_df)
print(sp_pixels)
```

## Summary

- SpatialPoints: point locations (x, y or lon, lat)
- SpatialLines: line features made of Line objects
- SpatialPolygons: area features made of Polygon objects
- Spatial*DataFrame variants attach attribute data
- CRS (Coordinate Reference System) defines how coordinates relate to real-world locations
- spTransform() performs coordinate transformations
- Spatial objects have bbox (bounding box) and CRS attributes