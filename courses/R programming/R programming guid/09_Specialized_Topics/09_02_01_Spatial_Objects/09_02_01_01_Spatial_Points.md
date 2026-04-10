# Spatial Points in R

## Learning Objectives

- Create spatial point objects
- Use sp package for spatial data
- Handle coordinate reference systems
- Perform spatial operations
- Visualize point data

## Theory

Spatial points represent geographic locations with X (longitude) and Y (latitude) coordinates. The sp package provides classes for spatial data in R. Points are created using SpatialPoints() or SpatialPointsDataFrame() when combined with attribute data.

Key concepts: coordinate reference system (CRS), proj4string, and spatial operations like distance calculations. The sf package is the modern successor to sp.

## Step-by-Step

1. Create data frame with coordinates
2. Convert to SpatialPointsDataFrame
3. Set coordinate reference system
4. Perform spatial operations
5. Plot or analyze

## Code Examples

### Creating Spatial Points

```r
cat("===== CREATE POINTS =====\n\n")

library(sp)

# Data frame with coordinates
points_df <- data.frame(
  id = 1:5,
  name = c("A", "B", "C", "D", "E"),
  x = c(-122.4, -122.3, -122.2, -122.1, -122.0),  # longitude
  y = c(37.7, 37.75, 37.8, 37.85, 37.9)      # latitude
)

# Create SpatialPoints
coords <- points_df[, c("x", "y")]
spatial_points <- SpatialPoints(coords)

cat("Created", length(spatial_points), "spatial points\n")
cat("Class:", class(spatial_points), "\n")
```

### Spatial Points Data Frame

```r
cat("\n===== POINTS DATA FRAME =====\n\n")

# Create SpatialPointsDataFrame
spatial_df <- SpatialPointsDataFrame(
  coords = coords,
  data = points_df,
  proj4string = CRS("+proj=longlat +datum=WGS84")
)

cat("Points data frame:\n")
print(spatial_df)

# Access coordinates
cat("\nCoordinates:\n")
print(coordinates(spatial_df))
```

### Coordinate Reference Systems

```r
cat("\n===== CRS =====\n\n")

# Define CRS
wgs84 <- CRS("+proj=longlat +datum=WGS84")
cat("WGS84 CRS:\n")
print(wgs84)

# UTM projection (meters)
utm_zone <- CRS("+proj=utm +zone=10 +datum=WGS84")
cat("\nUTM Zone 10:\n")
print(utm_zone)

# Transform to different CRS
transformed <- spTransform(spatial_df, utm_zone)
cat("\nTransformed coordinates:\n")
print(coordinates(transformed))
```

### Distance Calculations

```r
cat("\n===== DISTANCE =====\n\n")

# Calculate distances between points
library(geosphere)

pts <- as.matrix(coordinates(spatial_df))
dist_matrix <- distM(pts)
cat("Distance matrix (meters):\n")
print(round(dist_matrix / 1000, 2))

# Distance from point 1 to others
cat("\nDistance from point 1 (km):\n")
print(dist_matrix[1, ] / 1000)
```

### Basic Plotting

```r
cat("\n===== PLOTTING =====\n\n")

cat("# Plot spatial points\n")
cat("plot(spatial_df, pch = 20, col = 'blue')\n\n")

cat("# With basemap (requires internet)\n")
cat("# library(ggmap)\n")
cat("# map <- get_map(location = 'San Francisco')\n")
cat("# ggmap(map) + geom_point(aes(x, y), data = df)\n")
```

## Real-World Example: Store Locations

```r
# Real-world: Store location analysis
cat("===== STORE ANALYSIS =====\n\n")

# Store locations
stores <- data.frame(
  store_id = 1:3,
  name = c("Downtown", "Mall", "Airport"),
  longitude = c(-122.4194, -122.4048, -122.2378),
  latitude = c(37.7749, 37.7858, 37.6213),
  revenue = c(500000, 350000, 200000)
)

# Create spatial points
coordinates(stores) <- c("longitude", "latitude")
proj4string(stores) <- CRS("+proj=longlat +datum=WGS84")

cat("Store locations:\n")
print(stores)

# Calculate distances
pts <- coordinates(stores)
mat <- distM(pts)
cat("\nDistances (km):\n")
print(mat / 1000)

# Find nearest store for each customer location
customer <- c(-122.35, 37.75)
dists <- distM(rbind(customer, pts)) / 1000
cat("\nNearest store distances:\n")
print(dists[-1])
```

## Best Practices

1. Always specify CRS
2. Use WGS84 (EPSG:4326) for geographic coordinates
3. Transform before calculating distances
4. Use sf for new projects
5. Validate coordinates are in range

## Exercises

1. Create SpatialPointsDataFrame from your data
2. Transform coordinates to UTM
3. Calculate distance matrix
4. Find nearest neighbor
5. Create a thematic map