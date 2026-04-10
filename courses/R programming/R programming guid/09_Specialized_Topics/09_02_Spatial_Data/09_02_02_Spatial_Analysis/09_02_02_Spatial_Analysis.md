# Spatial Analysis with sf Package

## Title
Spatial Operations and Analysis Using the sf Package

## Objectives
- Understand simple features (sf) data model
- Perform spatial joins and overlays
- Execute geometric operations
- Work with spatial predicates

## Introduction

The sf (Simple Features) package provides a tidyverse-compatible interface for spatial data analysis. It represents spatial objects as data frames with a geometry column.

## Installing and Loading sf

```r
# Install required packages
install.packages("sf")
install.packages("dplyr")

# Load libraries
library(sf)
library(dplyr)
```

## Creating sf Objects

```r
# Create sf points from data frame
points_df <- data.frame(
  id = 1:3,
  name = c("A", "B", "C"),
  x = c(1, 2, 3),
  y = c(1, 2, 1)
)

# Convert to sf with POINT geometry
points_sf <- st_as_sf(points_df, coords = c("x", "y"), 
                     crs = 4326)  # WGS84
print(points_sf)
class(points_sf)

# Create sf polygon
# Define polygon coordinates
coords_polygon <- rbind(
  c(0, 0), c(3, 0), c(3, 3), c(0, 3), c(0, 0)
)

# Create polygon as sf
polygon_sf <- st_sf(
  id = "poly1",
  st_polygon(list(coords_polygon), n)
)

# Alternative: use matrix directly
poly_matrix <- matrix(c(0, 0, 5, 0, 5, 5, 0, 5, 0, 0), ncol = 2)
square_sf <- st_sf(
  id = "square1",
  geometry = st_sfc(st_polygon(list(poly_matrix)), crs = 4326)
)

# Create sf with multiple polygons (multipolygon)
coords_list <- list(
  matrix(c(10, 10, 15, 10, 15, 15, 10, 15, 10, 10), ncol = 2),
  matrix(c(20, 20, 25, 20, 25, 25, 20, 25, 20, 20), ncol = 2)
)
multi_poly_sf <- st_sf(
  id = "multi",
  st_multipolygon(coords_list)
)
```

## Spatial Joins

```r
# Create sample data for spatial join
# Points (e.g., buildings)
buildings <- data.frame(
  id = c(1, 2, 3),
  name = c("Building A", "Building B", "Building C"),
  x = c(1.5, 2.5, 4.5),
  y = c(1.5, 1.5, 4.5)
) %>% 
  st_as_sf(coords = c("x", "y"), crs = 4326)

# Polygons (e.g., neighborhoods)
zone1 <- st_sf(
  name = "Zone 1",
  st_polygon(list(matrix(c(0, 0, 3, 0, 3, 3, 0, 3, 0, 0), ncol = 2)))
)
zone2 <- st_sf(
  name = "Zone 2", 
  st_polygon(list(matrix(c(3, 3, 6, 3, 6, 6, 3, 6, 3, 3), ncol = 2)))
)
zones <- rbind(zone1, zone2)
st_crs(zones) <- 4326

# Spatial join: find which zone each building is in
buildings_with_zone <- st_join(buildings, zones, join = st_within)
print(buildings_with_zone)

# Left join with spatial predicate
# st_intersects: returns TRUE if geometries intersect
# st_within: returns TRUE if one geometry is within another
# st_contains: returns TRUE if first geometry contains second
```

## Geometric Operations

```r
# Point in polygon test
st_within(points_sf, polygon_sf)

# Buffer operation
buffered <- st_buffer(points_sf, dist = 0.5)
plot(buffered, border = "blue")
plot(points_sf, add = TRUE)

# Union of geometries
unionized <- st_union(polygon_sf, square_sf)
plot(unionized)

# Difference
difference <- st_difference(square_sf, polygon_sf)
plot(difference)

# Intersection
intersection <- st_intersection(square_sf, polygon_sf)
plot(intersection, col = "red")
```

## Overlay Operations

```r
# Create sample polygons for overlay
polygons_layer1 <- data.frame(
  id = 1:2,
  type = c("residential", "commercial")
) %>%
  st_as_sf(
    st_sfc(
      st_polygon(list(matrix(c(0, 0, 2, 0, 2, 2, 0, 2, 0, 0), ncol = 2))),
      st_polygon(list(matrix(c(2, 2, 4, 2, 4, 4, 2, 4, 2, 2), ncol = 2)))
    ),
    crs = 4326
  )

polygons_layer2 <- data.frame(
  id = 1:2,
  zone = c("zone A", "zone B")
) %>%
  st_as_sf(
    st_sfc(
      st_polygon(list(matrix(c(1, 1, 3, 1, 3, 3, 1, 3, 1, 1), ncol = 2))),
      st_polygon(list(matrix(c(3, 1, 5, 1, 5, 3, 3, 3, 3, 1), ncol = 2)))
    ),
    crs = 4326
  )

# Union overlay (merge all geometries)
union_overlay <- st_union(polygons_layer1, polygons_layer2)
plot(union_overlay)

# Intersection overlay
intersection_overlay <- st_intersection(polygons_layer1, polygons_layer2)
plot(intersection_overlay)

# sym_difference (areas not in common)
sym_diff_overlay <- st_sym_difference(polygons_layer1, polygons_layer2)
plot(sym_diff_overlay)

# Difference (layer1 minus layer2)
diff_overlay <- st_difference(polygons_layer1, polygons_layer2)
plot(diff_overlay)
```

## Distance Operations

```r
# Create sample points
pt1 <- st_sf(id = "A", st_sfc(st_point(c(0, 0)), crs = 4326)
pt2 <- st_sf(id = "B", st_sfc(st_point(c(3, 4))), crs = 4326)
pt3 <- st_sf(id = "C", st_sfc(st_point(c(1, 1))), crs = 4326)

# Calculate distances
st_distance(pt1, pt2)  # Returns 5 (Euclidean distance)
st_distance(pt1, pt3)   # Returns sqrt(2)

# Find nearest point from multiple options
all_pts <- rbind(pt2, pt3)
st_nearest_feature(pt1, all_pts)

# Distance matrix
st_distance(rbind(pt1, pt2, pt3))
```

## Spatial Predicates

```r
# Create test geometries
g1 <- st_sfc(st_point(c(0, 0)))
g2 <- st_sfc(st_point(c(1, 1)))
g3 <- st_sfc(st_point(c(5, 5)))

# st_disjoint: TRUE if geometries don't touch
st_disjoint(g1, g2)  # FALSE
st_disjoint(g1, g3)   # TRUE

# st_touches: TRUE if boundaries touch
square1 <- st_sfc(st_polygon(list(matrix(c(0,0,1,0,1,1,0,1,0,0), ncol = 2))))
square2 <- st_sfc(st_polygon(list(matrix(c(1,0,2,0,2,1,1,1,1,0), ncol = 2))))
st_touches(square1, square2)  # TRUE - they share an edge

# st_intersects: TRUE if geometries overlap/touch
st_intersects(square1, square2)

# st_within: TRUE if one is completely inside another
st_within(square1, square2)

# st_contains: TRUE if first contains second
st_contains(square1, square2)

# st_equals: TRUE if geometries are identical
st_equals(square1, square1)

# st_covers / st_covered_by
st_covers(square1, g1)
st_covered_by(g1, square1)
```

## Summary

- sf provides tidyverse-compatible spatial data frames
- Spatial joins use predicates like st_within(), st_intersects()
- Buffer, union, difference, intersection are core geometric operations
- st_distance() calculates distances between geometries
- Spatial predicates test relationships between geometries
- sf integrates seamlessly with dplyr for data manipulation