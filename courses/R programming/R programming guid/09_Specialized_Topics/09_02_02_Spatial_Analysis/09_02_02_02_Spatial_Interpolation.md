# Spatial Interpolation in R

## Learning Objectives

- Understand interpolation methods
- Apply IDW interpolation
- Use Kriging for prediction
- Create interpolated surfaces

## Theory

Spatial interpolation estimates values at unobserved locations from known values. Methods range from simple distance weighting to advanced geostatistical kriging. Choice depends on data characteristics and desired accuracy.

## Step-by-Step Guide

### Inverse Distance Weighting

```r
library(gstat)

# Create sample data
data <- data.frame(
  x = runif(50, 0, 100),
  y = runif(50, 0, 100),
  value = rnorm(50)
)

# Create grid for prediction
grid <- expand.grid(x = seq(0, 100, 2), y = seq(0, 100, 2))

# IDW interpolation
idw_model <- idw(value ~ 1, 
                 locations = ~x + y, 
                 data = data,
                 newdata = grid,
                 idp = 2)

# Plot
spplot(idw_model, "var1.pred")
```

### Ordinary Kriging

```r
library(gstat)

# Fit variogram
variogram_fit <- fit.variogram(
  variogram(value ~ x + y, data),
  vgm(1, "Sph", 30))

# Kriging prediction
kriged <- krige(value ~ 1, 
                locations = ~x + y,
                data,
                newdata = grid,
                model = variogram_fit)

# Plot
spplot(kriged, "var1.pred")
```

## Code Examples

### Universal Kriging

```r
# With covariate
variogram_uk <- variogram(value ~ x + y, data)
variogram_fit <- fit.variogram(variogram_uk, vgm(1, "Sph", 30))

kriged_uk <- krige(value ~ x + y,
                  locations = ~x + y,
                  data,
                  newdata = grid,
                  model = variogram_fit)
```

### Cross-Validation

```r
# Leave-one-out cross-validation
cv_result <- krige.cv(value ~ 1,
                     locations = ~x + y,
                     data,
                     model = variogram_fit)

# Calculate RMSE
sqrt(mean(cv_result$residual^2))
```

### Visualization

```r
library(ggplot2)

# Convert to data frame for plotting
kriged_df <- as.data.frame(kriged)
colnames(kriged_df) <- c("x", "y", "predicted", "variance")

# Plot
ggplot(kriged_df, aes(x = x, y = y, fill = predicted)) +
  geom_raster() +
  scale_fill_viridis_c()
```

## Best Practices

1. **Explore Data**: Check for trends first.

2. **Fit Variogram**: Use appropriate variogram model.

3. **Cross-Validate**: Test prediction accuracy.

4. **Consider Anisotropy**: Check for directional effects.

5. **Validate Assumptions**: Check for stationarity.

## Exercises

1. Perform IDW interpolation.

2. Fit variogram for data.

3. Apply ordinary kriging.

4. Perform cross-validation.

5. Visualize interpolated surface.

## Additional Resources

- [gstat Package](https://r-spatial.github.io/gstat/)
- [Geostatistics](https://www.springer.com/gp/book/9780387787331)
- [Spatial Interpolation](https://rspatial.org/raster/expert/03-interpolation.html)