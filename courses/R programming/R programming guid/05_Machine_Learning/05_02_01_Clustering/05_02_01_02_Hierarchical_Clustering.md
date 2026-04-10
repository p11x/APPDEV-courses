# Hierarchical Clustering

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the principles of hierarchical clustering
- Implement agglomerative and divisive clustering in R
- Apply different linkage methods (single, complete, average, Ward)
- Create and interpret dendrograms
- Cut the dendrogram to obtain flat clusters
- Compare hierarchical clustering with K-Means

## Theoretical Background

Hierarchical clustering builds a hierarchy of clusters either by progressively merging smaller clusters (agglomerative) or by progressively splitting larger clusters (divisive). Unlike K-Means, it doesn't require pre-specifying the number of clusters and produces a tree-like structure (dendrogram) showing relationships between clusters.

### Types of Hierarchical Clustering

#### Agglomerative (Bottom-Up)

1. Start with each point as its own cluster
2. Iteratively merge the two nearest clusters
3. Continue until all points are in one cluster

#### Divisive (Top-Down)

1. Start with all points in one cluster
2. Iteratively split clusters into two
3. Continue until each cluster contains one point

Agglomerative is more commonly used and easier to implement.

### Distance Measures

#### Linkage Methods

How to calculate distance between clusters:

1. **Single Linkage**: Minimum distance between any two points in clusters
   - Can create long,链状 clusters
   - Sensitive to outliers

2. **Complete Linkage**: Maximum distance between any two points
   - Tends to create compact clusters
   - More robust to outliers than single

3. **Average Linkage**: Average distance between all point pairs
   - Good compromise between single and complete
   - Less sensitive to outliers

4. **Ward Linkage**: Minimize within-cluster variance increase
   - Similar to K-Means objective
   - Produces compact, similar-sized clusters

### Dendrogram Interpretation

A dendrogram shows:
- **Vertical axis**: Distance (dissimilarity)
- **Horizontal axis**: Data points or clusters
- **Horizontal cuts**: Different numbers of clusters

Reading the dendrogram:
- Find horizontal line that crosses vertical lines
- Number of vertical lines it crosses = number of clusters
- Height indicates cluster separation

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("stats")
install.packages("ggplot2")
install.packages("factoextra")
install.packages("dendextend")

library(stats)
library(ggplot2)
library(factoextra)
library(dendextend)
```

### Step 2: Prepare Data

```r
# Create sample data
set.seed(42)
n <- 30

# Generate data with clear clusters
cluster1 <- data.frame(
  x = rnorm(10, mean = 0, sd = 0.5),
  y = rnorm(10, mean = 0, sd = 0.5),
  group = "A"
)
cluster2 <- data.frame(
  x = rnorm(10, mean = 5, sd = 0.5),
  y = rnorm(10, mean = 5, sd = 0.5),
  group = "B"
)
cluster3 <- data.frame(
  x = rnorm(10, mean = 0, sd = 0.5),
  y = rnorm(10, mean = 5, sd = 0.5),
  group = "C"
)

data_hclust <- rbind(cluster1, cluster2, cluster3)

# Calculate distance matrix
dist_matrix <- dist(data_hclust[, c("x", "y")])
print(as.matrix(dist_matrix)[1:5, 1:5])
```

### Step 3: Agglomerative Hierarchical Clustering

```r
# Different linkage methods
# Single linkage
hc_single <- hclust(dist_matrix, method = "single")

# Complete linkage
hc_complete <- hclust(dist_matrix, method = "complete")

# Average linkage
hc_average <- hclust(dist_matrix, method = "average")

# Ward linkage
hc_ward <- hclust(dist_matrix, method = "ward.D2")

# View dendrograms
par(mfrow = c(2, 2))
plot(hc_single, main = "Single Linkage")
plot(hc_complete, main = "Complete Linkage")
plot(hc_average, main = "Average Linkage")
plot(hc_ward, main = "Ward Linkage")
```

### Step 4: Cut Dendrogram

```r
# Cut dendrogram to get 3 clusters
clusters_3 <- cutree(hc_complete, k = 3)

# Add cluster assignments to data
data_hclust$cluster <- clusters_3

# Visualize clusters
ggplot(data_hclust, aes(x = x, y = y, color = factor(cluster))) +
  geom_point(size = 3) +
  ggtitle("Hierarchical Clustering (K=3)") +
  theme_minimal()

# Cut by height
# If dendrogram height shows clear separation at height 10
clusters_height <- cutree(hc_complete, h = 10)
print(clusters_height)
```

### Step 5: Different Distance Metrics

```r
# Using correlation-based distance
# This is useful for high-dimensional data
library(proxy)

# Euclidean distance (default)
dist_euclidean <- dist(data_hclust[, c("x", "y")], method = "euclidean")

# Manhattan distance
dist_manhattan <- dist(data_hclust[, c("x", "y")], method = "manhattan")

# Correlation distance
dist_correlation <- dist(data_hclust[, c("x", "y")], method = "correlation")

# Compare results
hc_euclidean <- hclust(dist_euclidean, method = "complete")
hc_manhattan <- hclust(dist_manhattan, method = "complete")

# Plot both
par(mfrow = c(1, 2))
plot(hc_euclidean, main = "Euclidean Distance")
plot(hc_manhattan, main = "Manhattan Distance")
```

### Step 6: Using factoextra for Visualization

```r
# Enhanced visualization
fviz_dend(hc_complete, 
          main = "Dendrogram - Complete Linkage",
          k = 3,
          color_labels_by_k = TRUE)

# Different visualization styles
fviz_dend(hc_complete, 
          main = "Circular Dendrogram",
          type = "circular")

# Silhouette analysis
sil <- silhouette(cutree(hc_complete, k = 3), dist_matrix)
fviz_silhouette(sil)

# Average silhouette score
avg_sil <- mean(sil[, 3])
cat("Average Silhouette Score:", avg_sil, "\n")
```

## Code Examples

### Example 1: Document Clustering

This example demonstrates hierarchical clustering for document similarity.

```r
# Create synthetic document data (TF-IDF like features)
set.seed(123)
n <- 20
p <- 10

# Create feature matrix
doc_data <- matrix(rnorm(n * p, mean = 0, sd = 1), nrow = n)

# Add structure: first 5 docs similar, next 5 similar, etc.
doc_data[1:5, 1:5] <- doc_data[1:5, 1:5] + 2
doc_data[6:10, 6:8] <- doc_data[6:10, 6:8] + 2
doc_data[11:15, c(1, 3, 5)] <- doc_data[11:15, c(1, 3, 5)] + 2

# Calculate distance
doc_dist <- dist(doc_data, method = "euclidean")

# Hierarchical clustering
hc_docs <- hclust(doc_dist, method = "ward.D2")

# Plot dendrogram
fviz_dend(hc_docs, 
          main = "Document Clustering Dendrogram",
          k = 4,
          color_labels_by_k = TRUE,
          rect = TRUE)

# Cut into 4 clusters
doc_clusters <- cutree(hc_docs, k = 4)
cat("Document cluster assignments:\n")
print(doc_clusters)
```

### Example 2: Gene Expression Clustering

This example shows hierarchical clustering for gene expression data.

```r
# Create synthetic gene expression data
set.seed(456)
n_genes <- 50
n_samples <- 10

# Gene expression matrix
gene_data <- matrix(rnorm(n_genes * n_samples), nrow = n_genes)
rownames(gene_data) <- paste0("gene_", 1:n_genes)
colnames(gene_data) <- paste0("sample_", 1:n_samples)

# Add patterns: first 15 genes upregulated in samples 1-5
gene_data[1:15, 1:5] <- gene_data[1:15, 1:5] + 3

# Genes 16-30 upregulated in samples 6-10
gene_data[16:30, 6:10] <- gene_data[16:30, 6:10] + 3

# Genes 31-40 downregulated in all samples
gene_data[31:40, ] <- gene_data[31:40, ] - 2

# Scale features
gene_data_scaled <- scale(gene_data)

# Transpose for sample clustering
gene_dist <- dist(t(gene_data_scaled), method = "euclidean")

# Cluster samples
hc_samples <- hclust(gene_dist, method = "average")

# Cluster genes
gene_dist_genes <- dist(gene_data_scaled, method = "euclidean")
hc_genes <- hclust(gene_dist_genes, method = "ward.D2")

# Visualize
par(mfrow = c(1, 2))
plot(hc_samples, main = "Sample Clustering")
plot(hc_genes, main = "Gene Clustering")

# Heatmap visualization
library(pheatmap)
pheatmap(gene_data_scaled, 
         cluster_rows = hc_genes,
         cluster_cols = hc_samples,
         main = "Gene Expression Heatmap")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Choose Appropriate Linkage**: Ward for compact clusters, single for discovering chains
2. **Scale Data**: Always scale before clustering
3. **Use Dendrogram**: Visualize to understand cluster structure
4. **Consider Multiple Methods**: Try different linkages
5. **Combine with K-Means**: Use hierarchical to find initial centroids

### Common Pitfalls

1. **Ignoring Scale**: Distance measures are affected by different scales
2. **Single Linkage Issues**: Can produce unbalanced, chaining clusters
3. **Large Datasets**: Full dendrogram becomes hard to visualize
4. **Correlation Distance**: May produce unexpected results
5. **Not Cutting at Right Height**: Wrong number of clusters

## Performance Considerations

### Computational Complexity

- **Agglomerative**: O(n² log n) to O(n³)
- **Divisive**: O(n² log n)
- **Space**: O(n²) for distance matrix

### When to Use Hierarchical Clustering

- **Small to Medium Datasets**: n < 10,000 typically
- **Tree Structure Needed**: When cluster relationships matter
- **No Pre-specified K**: When number of clusters is unknown
- **Data Exploration**: When exploring cluster structure

## Related Concepts

- **K-Means**: Flat clustering
- **DBSCAN**: Density-based clustering
- **Gaussian Mixture Models**: Probabilistic clustering
- **Biclustering**: For two-way data

## Exercise Problems

1. **Basic**: Apply hierarchical clustering to the Iris dataset.

2. **Intermediate**: Compare different linkage methods.

3. **Advanced**: Create a heatmap with hierarchical clustering.

4. **Real-World Challenge**: Cluster similar documents.

5. **Extension**: Compare hierarchical with K-Means clustering.