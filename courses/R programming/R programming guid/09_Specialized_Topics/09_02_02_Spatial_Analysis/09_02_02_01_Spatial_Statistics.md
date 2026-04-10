# Spatial Statistics in R

## Learning Objectives

- Understand spatial autocorrelation
- Apply spatial correlation measures
- Use Moran's I for clustering
- Perform spatial regression

## Theory

Spatial statistics analyzes spatial relationships and patterns. Key concepts include spatial autocorrelation (values correlated with nearby values), spatial heterogeneity, and spatial dependence. Methods typically use spatial weights matrices.

## Step-by-Step Guide

### Creating Spatial Weights

```r
library(spdep)

# Create neighbor list from shapefile
neighbors <- poly2nb(nc)

# Create weights
weights <- nb2listw(neighbors)

# Check neighbors
print(summary(neighbors))
```

### Moran's I Test

Test for spatial autocorrelation:

```r
library(spdep)

# Calculate Moran's I
moran_result <- moran.test(nc$income, 
                           weights)

# View results
print(moran_result)

# Plot Moran scatter plot
moran.plot(nc$income, weights)
```

## Code Examples

### Local Indicators of Spatial Association

```r
# Local Moran's I
local_moran <- local.moran(nc$income, weights)

# Get p-values
local_moran[, "Pr(z != E)"]

# Get quadrants
quadrants <- quadrat.sig(nc$income, weights)
```

### Geary's C

```r
# Geary's C test
geary_result <- geary.test(nc$income, weights)
print(geary_result)
```

### Spatial Regression

```r
library(spatialreg)

# Spatial lag model
lag_model <- lagsarlm(income ~ education + employment,
                     data = nc, 
                     weights)

# Spatial error model
error_model <- errorsarlm(income ~ education + employment,
                         data = nc,
                         weights)

# Compare models
AIC(lag_model, error_model)
```

## Best Practices

1. **Create Proper Weights**: Choose appropriate neighbor definition.

2. **Test Significance**: Consider multiple testing.

3. **Interpret Results**: Understand what significant values mean.

4. **Use Robust Methods**: Consider bootstrapping.

5. **Validate Models**: Check residuals for remaining patterns.

## Exercises

1. Create spatial weights from polygons.

2. Calculate global Moran's I.

3. Calculate local Moran's I.

4. Fit spatial lag model.

5. Check residuals for patterns.

## Additional Resources

- [spdep Package](https://cran.r-project.org/web/packages/spdep/)
- [Spatial Data Analysis](https://www.paulamoraga.com/book-geospatial/)
- [Spatial Econometrics](https://www.springer.com/gp/book/9780387787331)