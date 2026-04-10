# Dimensionality Reduction

## Learning Objectives

- Perform PCA (Principal Component Analysis)
- Understand when to use dimensionality reduction

## Code Examples

```r
# PCA in R

cat("===== DIMENSIONALITY REDUCTION =====\n\n")

# PCA on mtcars
data(mtcars)
pca <- prcomp(mtcars[, c("mpg", "disp", "hp", "wt", "qsec")])

cat("PCA Summary:\n")
cat("Standard deviations:\n")
print(pca$sdev)
cat("\nProportion of variance:\n")
print(summary(pca)$importance[2, ])
cat("\nRotation (loadings):\n")
print(pca$rotation[, 1:2])
```

## Best Practices

1. Standardize data before PCA
2. Choose number of components carefully
3. Interpret loadings
