# Clustering Evaluation Methods

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand internal and external clustering evaluation metrics
- Calculate and interpret silhouette scores
- Apply the elbow method and gap statistic
- Use internal metrics: Dunn index, Calinski-Harabasz index
- Apply external metrics: Adjusted Rand Index, V-measure
- Choose appropriate evaluation methods for different scenarios

## Theoretical Background

Evaluating clustering results is crucial to determine the quality and usefulness of discovered clusters. Unlike supervised learning where we have ground truth labels, clustering evaluation often requires multiple approaches depending on whether true labels are available.

### Types of Evaluation Metrics

#### Internal Evaluation (Unsupervised)

Measures cluster quality based on the data itself without external labels:
- **Cohesion**: How similar points are within a cluster
- **Separation**: How different clusters are from each other
- Goals: Minimize within-cluster variance, maximize between-cluster variance

#### External Evaluation (Supervised)

Uses ground truth labels to evaluate clustering:
- Compare discovered clusters to true labels
- Measures like Adjusted Rand Index, V-measure
- Requires labeled data

#### Relative Evaluation

Compare different clustering results:
- Run same algorithm with different parameters
- Compare different algorithms
- Use same evaluation metric

### Common Internal Metrics

#### Silhouette Score

For each point i, silhouette coefficient:
$$s(i) = \frac{b(i) - a(i)}{\max(a(i), b(i))}$$

where:
- a(i) = average distance to points in same cluster
- b(i) = minimum average distance to points in other clusters

Overall silhouette score: mean of all s(i)
- Range: [-1, 1]
- Higher is better

#### Elbow Method

Plot within-cluster sum of squares (WCSS) vs number of clusters:
- Look for "elbow" where decrease slows down
- Choose K at the elbow
- Subjective but intuitive

#### Calinski-Harabasz Index (Variance Ratio)

$$CH = \frac{BGSS / (K - 1)}{WGSS / (n - K)}$$

where:
- BGSS = between-group sum of squares
- WGSS = within-group sum of squares
- Higher is better

#### Dunn Index

$$D = \frac{min_{i,j} d(C_i, C_j)}{max_k diam(C_k)}$$

where:
- d(C_i, C_j) = distance between clusters
- diam(C_k) = diameter of cluster
- Higher is better

### Common External Metrics

#### Adjusted Rand Index (ARI)

- Adjusted for chance
- Range: [-1, 1]
- 1 = perfect matching
- 0 = random clustering
- Negative = worse than random

#### V-Measure

Harmonic mean of:
- **Homogeneity**: Each cluster has only one class
- **Completeness**: All members of a class are in same cluster

$$V = \frac{2 \times h \times c}{h + c}$$

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("factoextra")
install.packages("cluster")
install.packages("fpc")
install.packages("dplyr")

library(factoextra)
library(cluster)
library(fpc)
library(dplyr)
```

### Step 2: Create Sample Data

```r
# Create data with known structure
set.seed(42)
n <- 150

# Three clear clusters
cluster1 <- data.frame(x = rnorm(50, mean = 2, sd = 0.5),
                       y = rnorm(50, mean = 2, sd = 0.5))
cluster2 <- data.frame(x = rnorm(50, mean = 6, sd = 0.5),
                       y = rnorm(50, mean = 6, sd = 0.5))
cluster3 <- data.frame(x = rnorm(50, mean = 2, sd = 0.5),
                       y = rnorm(50, mean = 6, sd = 0.5))

data_cluster <- rbind(cluster1, cluster2, cluster3)
true_labels <- rep(c(1, 2, 3), each = 50)

# Scale data
data_scaled <- scale(data_cluster)
```

### Step 3: Calculate Silhouette Score

```r
# Apply K-Means
set.seed(42)
kmeans_result <- kmeans(data_scaled, centers = 3, nstart = 10)

# Calculate silhouette
sil <- silhouette(kmeans_result$cluster, dist(data_scaled))

# View silhouette
head(sil)

# Average silhouette score
avg_sil <- mean(sil[, 3])
cat("Average Silhouette Score:", avg_sil, "\n")

# Plot silhouette
fviz_silhouette(sil)

# Per-cluster silhouette
cluster_sil <- sil %>%
  as.data.frame() %>%
  group_by(cluster) %>%
  summarise(avg_silhouette = mean(sil_width))
print(cluster_sil)
```

### Step 4: Elbow Method

```r
# Calculate WCSS for different K
set.seed(42)
wcss <- sapply(1:10, function(k) {
  km <- kmeans(data_scaled, centers = k, nstart = 10)
  km$tot.withinss
})

# Plot elbow curve
plot(1:10, wcss, type = "b",
     main = "Elbow Method",
     xlab = "Number of Clusters (K)",
     ylab = "Within-Cluster Sum of Squares")

# Using factoextra
fviz_nbclust(data_scaled, kmeans, method = "wss") +
  ggtitle("Elbow Method using factoextra")
```

### Step 5: Gap Statistic

```r
# Calculate gap statistic
set.seed(42)
gap_stat <- clusGap(data_scaled, FUN = kmeans, 
                     nstart = 10, K.max = 10, B = 50)

# Print gap statistic
print(gap_stat, method = "firstmax")

# Plot gap statistic
fviz_gap_stat(gap_stat) +
  ggtitle("Gap Statistic")

# Optimal K
k_gap <- maxSE(gap_stat$Tab[, "gap"], gap_stat$Tab[, "SE.sim"])
cat("Optimal K (gap):", k_gap, "\n")
```

### Step 6: External Metrics

```r
# Calculate internal metrics
set.seed(42)
kmeans_3 <- kmeans(data_scaled, centers = 3, nstart = 10)

# Dunn Index
dunn_index <- dunn(clusters = kmeans_3$cluster, Data = data_scaled)
cat("Dunn Index:", dunn_index, "\n")

# Calinski-Harabasz Index
ch_index <- calinhara(data_scaled, kmeans_3$cluster)
cat("Calinski-Harabasz Index:", ch_index, "\n")

# External metrics with true labels
# Adjusted Rand Index
ari <- adjustedRandIndex(kmeans_3$cluster, true_labels)
cat("Adjusted Rand Index:", ari, "\n")

# V-Measure
v_meas <- vmeasure.stats(kmeans_3$cluster, true_labels)$v
cat("V-Measure:", v_meas, "\n")
```

## Code Examples

### Example 1: Compare Clustering Algorithms

This example shows comparing different clustering methods.

```r
# Create data
set.seed(123)
n <- 200

# Data with non-spherical shapes
moons <- data.frame(
  x = c(sin(seq(0, pi, length.out = 100)), 
        sin(seq(0, pi, length.out = 100)) + 1.5),
  y = c(cos(seq(0, pi, length.out = 100)), 
        -cos(seq(0, pi, length.out = 100)) + 1),
  true_cluster = rep(c(1, 2), each = 100)
)

# Scale
moons_scaled <- scale(moons[, c("x", "y")])

# K-Means
set.seed(42)
km <- kmeans(moons_scaled, centers = 2, nstart = 10)

# Hierarchical
hc <- hclust(dist(moons_scaled), method = "complete")
hc_clusters <- cutree(hc, k = 2)

# DBSCAN
db <- dbscan(moons_scaled, eps = 0.3, minPts = 5)

# Compare with true labels
cat("K-Means ARI:", adjustedRandIndex(km$cluster, moons$true_cluster), "\n")
cat("Hierarchical ARI:", adjustedRandIndex(hc_clusters, moons$true_cluster), "\n")
cat("DBSCAN ARI:", adjustedRandIndex(db$cluster, moons$true_cluster), "\n")

# Compare with silhouette
cat("\nK-Means Silhouette:", mean(silhouette(km$cluster, dist(moons_scaled))[, 3]), "\n")
cat("Hierarchical Silhouette:", mean(silhouette(hc_clusters, dist(moons_scaled))[, 3]), "\n")
cat("DBSCAN Silhouette:", mean(silhouette(db$cluster, dist(moons_scaled))[, 3]), "\n")
```

### Example 2: Find Optimal K for Real Data

This example demonstrates finding optimal clusters for real data.

```r
# Use iris data (remove species for clustering, keep for evaluation)
data(iris)
iris_data <- iris[, -5]
true_species <- as.numeric(iris$Species)

# Test different numbers of clusters
set.seed(42)
results <- data.frame(
  K = 2:6,
  WCSS = NA,
  Silhouette = NA,
  CH = NA,
  ARI = NA
)

for(k in 2:6) {
  km <- kmeans(scale(iris_data), centers = k, nstart = 10)
  
  results$WCSS[results$K == k] <- km$tot.withinss
  
  sil <- silhouette(km$cluster, dist(scale(iris_data)))
  results$Silhouette[results$K == k] <- mean(sil[, 3])
  
  results$CH[results$K == k] <- calinhara(scale(iris_data), km$cluster)
  
  results$ARI[results$K == k] <- adjustedRandIndex(km$cluster, true_species)
}

print(results)

# Visualize
par(mfrow = c(2, 2))
plot(results$K, results$WCSS, type = "b", main = "WCSS")
plot(results$K, results$Silhouette, type = "b", main = "Silhouette")
plot(results$K, results$CH, type = "b", main = "Calinski-Harabasz")
plot(results$K, results$ARI, type = "b", main = "ARI with true labels")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use Multiple Metrics**: No single metric tells the whole story
2. **Consider Domain Knowledge**: Sometimes business needs override metrics
3. **External Evaluation When Possible**: Use true labels to validate
4. **Visualize Clusters**: Always visualize to understand results
5. **Check Stability**: Run multiple times with different seeds

### Common Pitfalls

1. **Relying on One Metric**: Can lead to wrong conclusions
2. **Ignoring Silhouette for K-Means**: Silhouette may favor different K
3. **External Metrics Not Always Available**: Can't always compare to true labels
4. **High Dimensionality**: Metrics may be misleading in high dimensions
5. **Cluster Shape Assumptions**: Some metrics assume spherical clusters

## Performance Considerations

### Metric Comparison

| Metric | Range | Uses | Pros/Cons |
|--------|-------|------|-----------|
| Silhouette | [-1, 1] | Internal | Intuitive, but assumes spherical |
| Dunn | [0, ∞) | Internal | Considers density, sensitive |
| CH | [0, ∞) | Internal | Fast, assumes spherical |
| ARI | [-1, 1] | External | Adjusted for chance |
| V-measure | [0, 1] | External | Considers homogeneity/completeness |

### When to Use Each

- **No true labels**: Use internal metrics (silhouette, CH)
- **True labels available**: Use external metrics (ARI, V-measure)
- **Parameter tuning**: Use internal metrics
- **Algorithm comparison**: Use same metric across methods

## Related Concepts

- **Bootstrap Stability**: Test clustering stability
- **Consensus Clustering**: Combine multiple clusterings
- **Biclustering**: Cluster rows and columns simultaneously
- **Subspace Clustering**: Cluster in feature subsets

## Exercise Problems

1. **Basic**: Calculate silhouette score for K-Means on Iris.

2. **Intermediate**: Use elbow method to find optimal K.

3. **Advanced**: Compare multiple clustering algorithms.

4. **Real-World Challenge**: Evaluate clustering on real dataset.

5. **Extension**: Implement consensus clustering.