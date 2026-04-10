# Spatial Polygons in R

## Learning Objectives

- Understand spatial polygon objects
- Create polygons from coordinates
- Work with sf and sp packages
- Perform polygon operations

## Theory

Spatial polygons represent areas with multiple boundary segments. The sf package provides a modern approach to spatial data in R, while sp offers the traditional S4 class system. Polygons can contain holes and multiple rings.

## Step-by-Step Guide

### Creating Polygons with sf

```r
library(sf)

# Create polygon from coordinates
coords <- matrix(c(0, 0, 1, 0, 1, 1, 0, 1, 0, 0), 
                ncol = 2, byrow = TRUE)

poly <- st_polygon(list(coords))
plot(poly)
```

### Creating Multipolygons

```r
# Multiple polygons as one feature
coords1 <- matrix(c(0, 0, 1, 0, 1, 1, 0, 1, 0, 0), 
                 ncol = 2, byrow = TRUE)
coords2 <- matrix(c(2, 0, 3, 0, 3, 1, 2, 1, 2, 0), 
                 ncol = 2, byrow = TRUE)

multipoly <- st_multipolygon(list(
  list(coords1),
  list(coords2)
))
```

### Using Built-in Data

```r
library(spData)

# North Carolina counties (sf object)
nc <- st_read(system.file("shp/nc.shp", package = "spData"))

# Plot
plot(st_geometry(nc))

# Check class
class(nc)
```

## Code Examples

### Creating Polygons from Data Frame

```r
# Create from tidy data
library(tidyverse)

polygon_data <- data.frame(
  id = 1,
  x = c(0, 1, 1, 0),
  y = c(0, 0, 1, 1)
)

coords <- polygon_data |>
  select(x, y) |>
  as.matrix()

poly <- st_polygon(list(coords))
```

### Polygon Operations

```r
library(sf)

# Union polygons
poly_union <- st_union(poly1, poly2)

# Intersection
poly_inter <- st_intersection(poly1, poly2)

# Difference
poly_diff <- st_difference(poly1, poly2)

# Buffer
poly_buffer <- st_buffer(poly, dist = 0.1)
```

### Area and Centroid

```r
# Calculate area
st_area(poly)

# Centroid
centroid <- st_centroid(poly)

# Boundary
boundary <- st_boundary(poly)
```

## Best Practices

1. **Check Validity**: Use st_is_valid() to check.

2. **Use Coordinate System**: Set CRS with st_crs().

3. **Simplify for Display**: Use st_simplify().

4. **Consider Topology**: Handle topology issues.

## Exercises

1. Create a polygon from coordinates.

2. Plot NC counties data.

3. Calculate polygon areas.

4. Perform polygon union.

5. Set coordinate reference system.

## Additional Resources

- [sf Package](https://r-spatial.github.io/sf/)
- [Spatial Data in R](https://r-spatial.org/)
- [geocomp with R](https://www.springer.com/gp/book/9780387787331)