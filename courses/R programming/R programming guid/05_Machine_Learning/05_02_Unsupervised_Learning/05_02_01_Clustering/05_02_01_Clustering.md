# Clustering in R

## Learning Objectives

- Understand clustering algorithms
- Perform k-means clustering
- Use hierarchical clustering

## Code Examples

```r
# Clustering in R

cat("===== CLUSTERING =====\n\n")

# K-means clustering
set.seed(42)
data <- data.frame(
  x = c(rnorm(20, mean = 0), rnorm(20, mean = 3)),
  y = c(rnorm(20, mean = 0), rnorm(20, mean = 3))
)

cat("K-means with k = 2:\n")
km <- kmeans(data, centers = 2)
cat("Cluster centers:\n")
print(km$centers)
cat("Cluster sizes:", km$size, "\n")

# Hierarchical clustering
cat("\nHierarchical clustering:\n")
dist_matrix <- dist(data)
hc <- hclust(dist_matrix)
cat("Created hierarchical cluster\n")
```

## Best Practices

1. Scale data before clustering
2. Choose appropriate k
3. Use elbow method
