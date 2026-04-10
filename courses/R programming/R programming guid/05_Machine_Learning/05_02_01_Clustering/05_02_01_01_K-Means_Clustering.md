# K-Means Clustering

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the K-Means clustering algorithm and its mechanics
- Implement K-Means clustering in R using the `stats` package
- Choose the optimal number of clusters using the elbow method and silhouette analysis
- Interpret and visualize clustering results
- Apply K-Means to real-world data segmentation problems
- Understand the limitations and assumptions of K-Means

## Theoretical Background

K-Means is one of the most popular and widely used clustering algorithms. It partitions data into K distinct clusters based on feature similarity. The algorithm aims to minimize the within-cluster variance, also known as the within-cluster sum of squares (WCSS).

### How K-Means Works

The K-Means algorithm follows these steps:

1. **Initialize**: Choose K random centroids (cluster centers)
2. **Assign**: Assign each data point to the nearest centroid
3. **Update**: Recalculate centroids as the mean of all points in each cluster
4. **Iterate**: Repeat steps 2 and 3 until convergence or maximum iterations

### The Objective Function

K-Means minimizes the within-cluster sum of squares:

$$WCSS = \sum_{i=1}^{K} \sum_{x \in C_i} ||x - \mu_i||^2$$

where:
- $K$ is the number of clusters
- $C_i$ is the set of points in cluster i
- $\mu_i$ is the centroid of cluster i
- $||x - \mu_i||^2$ is the squared Euclidean distance

### Initialization Methods

1. **Random Initialization**: Randomly select K data points as centroids
2. **Forgy Method**: Randomly assign K points as initial centroids
3. **K-Means++**: Smart initialization that spreads initial centroids

### Important Assumptions

1. **Spherical Clusters**: Assumes clusters are roughly spherical and equally sized
2. **Equal Variance**: Assumes features have similar variance
3. **Continuous Data**: Works best with continuous numerical features
4. **Predefined K**: Requires specifying K in advance

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("stats")
install.packages("ggplot2")
install.packages("factoextra")
install.packages("dplyr")

library(stats)
library(ggplot2)
library(factoextra)
library(dplyr)
```

### Step 2: Prepare Data

```r
# Create sample data
set.seed(42)
n <- 150

# Generate three distinct clusters
cluster1 <- data.frame(
  x = rnorm(50, mean = 2, sd = 0.5),
  y = rnorm(50, mean = 2, sd = 0.5)
)
cluster2 <- data.frame(
  x = rnorm(50, mean = 6, sd = 0.5),
  y = rnorm(50, mean = 6, sd = 0.5)
)
cluster3 <- data.frame(
  x = rnorm(50, mean = 2, sd = 0.5),
  y = rnorm(50, mean = 6, sd = 0.5)
)

# Combine into one dataset
data_kmeans <- rbind(cluster1, cluster2, cluster3)
data_kmeans$cluster <- rep(c(1, 2, 3), each = 50)

# Scale the data (important for K-Means)
data_scaled <- scale(data_kmeans[, c("x", "y")])

# Visualize original data
ggplot(data_kmeans, aes(x = x, y = y, color = factor(cluster))) +
  geom_point() +
  ggtitle("Original Data Clusters") +
  theme_minimal()
```

### Step 3: Implement K-Means

```r
# Basic K-Means with 3 clusters
set.seed(42)
kmeans_result <- kmeans(data_scaled, centers = 3, nstart = 10)

# View results
print(kmeans_result)
# K-means clustering with 3 clusters of sizes 50, 50, 50
# 
# Cluster means:
#       x          y
# 1 -1.032779  -1.006117
# 2  1.014949  -0.987585
# 3 -1.015618   1.033284
# 
# Available components:
# [1] "cluster"      "centers"      "totss"        "withinss"     "tot.withinss"
# [6] "betweenss"    "size"         "iter"         "ifault"       "method"

# Cluster assignments
head(kmeans_result$cluster)

# Cluster sizes
kmeans_result$size

# Within-cluster sum of squares
kmeans_result$withinss

# Total within-cluster sum of squares
kmeans_result$tot.withinss
```

### Step 4: Determine Optimal Number of Clusters

```r
# Method 1: Elbow Method
set.seed(42)

# Calculate WCSS for different K values
wcss <- sapply(1:10, function(k) {
  kmeans(data_scaled, centers = k, nstart = 10)$tot.withinss
})

# Plot elbow curve
plot(1:10, wcss, type = "b", 
     main = "Elbow Method",
     xlab = "Number of Clusters (K)",
     ylab = "Within-Cluster Sum of Squares")

# Method 2: Silhouette Analysis
library(factoextra)

silhouette_scores <- sapply(2:10, function(k) {
  km <- kmeans(data_scaled, centers = k, nstart = 10)
  sil <- silhouette(km$cluster, dist(data_scaled))
  mean(sil[, 3])
})

plot(2:10, silhouette_scores, type = "b",
     main = "Silhouette Analysis",
     xlab = "Number of Clusters (K)",
     ylab = "Average Silhouette Score")

# Method 3: Using factoextra
fviz_nbclust(data_scaled, kmeans, method = "wss") +
  ggtitle("Optimal Number of Clusters (Elbow)")
```

### Step 5: Visualize Results

```r
# Add cluster assignments to data
data_kmeans$assigned_cluster <- kmeans_result$cluster

# Visualize clusters
ggplot(data_kmeans, aes(x = x, y = y, color = factor(assigned_cluster))) +
  geom_point() +
  geom_point(data = as.data.frame(kmeans_result$centers), 
             aes(x = x, y = y), 
             size = 5, shape = "X", color = "black") +
  ggtitle("K-Means Clustering Results (K=3)") +
  theme_minimal()

# Using factoextra
fviz_cluster(kmeans_result, data = data_scaled,
             main = "K-Means Cluster Visualization")
```

### Step 6: Evaluate Clustering Quality

```r
# Calculate silhouette score
sil <- silhouette(kmeans_result$cluster, dist(data_scaled))
summary(sil)

# Average silhouette width
avg_sil <- mean(sil[, 3])
cat("Average Silhouette Score:", avg_sil, "\n")

# Plot silhouette
fviz_silhouette(sil)

# Additional metrics
library(factoextra)
cluster_stats <- get_clust_tendency(data_scaled, n = 50)
cat("Hopkins Statistic:", cluster_stats$hopkins_stat, "\n")
# Values close to 0.5 suggest data is uniformly distributed
# Values close to 1 suggest clustering tendency
```

## Code Examples

### Example 1: Customer Segmentation

This example demonstrates K-Means for customer segmentation.

```r
# Create synthetic customer data
set.seed(123)
n <- 300

customer_data <- data.frame(
  age = runif(n, 18, 70),
  annual_income = rnorm(n, 50000, 15000),
  spending_score = runif(n, 1, 100),
  purchases_per_month = rpois(n, 5)
)

# Scale features
customer_scaled <- scale(customer_data)

# Find optimal K
set.seed(42)
wcss_customers <- sapply(1:10, function(k) {
  kmeans(customer_scaled, centers = k, nstart = 10)$tot.withinss
})

plot(1:10, wcss_customers, type = "b",
     main = "Customer Data - Elbow Method",
     xlab = "Number of Clusters",
     ylab = "WCSS")

# Based on elbow, let's use K=5
set.seed(42)
customer_km <- kmeans(customer_scaled, centers = 5, nstart = 10)

# Add clusters to data
customer_data$cluster <- customer_km$cluster

# Analyze clusters
cluster_summary <- customer_data %>%
  group_by(cluster) %>%
  summarise(
    n = n(),
    avg_age = mean(age),
    avg_income = mean(annual_income),
    avg_spending = mean(spending_score),
    avg_purchases = mean(purchases_per_month)
  )
print(cluster_summary)
```

### Example 2: Image Color Quantization

This example shows K-Means for reducing colors in images.

```r
# Create a simple color image data
set.seed(456)
n <- 500

# Simulate pixel data with 3 main colors
color_data <- rbind(
  data.frame(r = rnorm(170, 200, 20), g = rnorm(170, 50, 20), b = rnorm(170, 50, 20)),
  data.frame(r = rnorm(170, 50, 20), g = rnorm(170, 200, 20), b = rnorm(170, 50, 20)),
  data.frame(r = rnorm(160, 50, 20), g = rnorm(160, 50, 20), b = rnorm(160, 200, 20))
)

# Scale
color_scaled <- scale(color_data)

# Apply K-Means with K=3
set.seed(42)
color_km <- kmeans(color_scaled, centers = 3, nstart = 10)

# Get dominant colors
dominant_colors <- color_km$centers
print("Dominant RGB colors:")
print(dominant_colors)

# Add cluster assignments
color_data$color_cluster <- color_km$cluster

# Visualize
ggplot(color_data, aes(x = r, y = g, color = factor(color_cluster))) +
  geom_point(alpha = 0.5) +
  ggtitle("Color Clustering") +
  theme_minimal()
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Always Scale Data**: K-Means is distance-based; scale features before clustering
2. **Use Multiple Restarts**: Set nstart > 1 to avoid local optima
3. **Validate Clusters**: Use silhouette scores and domain knowledge
4. **Consider K**: Use elbow method, silhouette, or gap statistic
5. **Check Assumptions**: Ensure data meets K-Means assumptions

### Common Pitfalls

1. **Not Scaling Data**: Features with different scales dominate
2. **Wrong K**: Choosing K poorly affects results significantly
3. **Sensitive to Initialization**: Different runs can give different results
4. **Assuming Spherical Clusters**: K-Means struggles with non-spherical data
5. **Ignoring Outliers**: Outliers can distort centroids significantly

## Performance Considerations

### Computational Complexity

- **Time**: O(K × n × d × i) where i = iterations
- **Space**: O(K × d + n × d)
- **Convergence**: Typically 10-100 iterations

### Optimization Tips

1. **Smart Initialization**: Use kmeans++ (set nstart properly)
2. **Mini-Batch K-Means**: For very large datasets
3. **Elkan's Algorithm**: For reduced distance calculations
4. **Parallel Processing**: Some implementations support parallel execution

## Related Concepts

- **Hierarchical Clustering**: Different approach to clustering
- **DBSCAN**: Density-based, handles arbitrary shapes
- **Gaussian Mixture Models**: Probabilistic clustering
- **K-Medoids**: More robust to outliers

## Exercise Problems

1. **Basic**: Apply K-Means to the Iris dataset.

2. **Intermediate**: Use the elbow method to find optimal K.

3. **Advanced**: Compare K-Means with hierarchical clustering.

4. **Real-World Challenge**: Segment customers into meaningful groups.

5. **Extension**: Implement Mini-Batch K-Means for large datasets.