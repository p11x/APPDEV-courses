# K-Nearest Neighbors (KNN) Classification

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the fundamental principles of the K-Nearest Neighbors algorithm
- Implement KNN classification in R using the `caret` and `class` packages
- Choose an appropriate value for K using cross-validation
- Apply KNN to real-world classification problems
- Evaluate KNN model performance using appropriate metrics
- Understand the advantages and limitations of instance-based learning

## Theoretical Background

K-Nearest Neighbors (KNN) is one of the simplest and most intuitive machine learning algorithms. Unlike most classification algorithms that learn a function from training data, KNN is an instance-based learning method that makes predictions based on the similarity between new instances and existing training examples.

### How KNN Works

The fundamental principle behind KNN is deceptively simple: "birds of a feather flock together." When you want to classify a new data point, KNN looks at the K closest (most similar) training examples and assigns the most common class among them.

#### The Algorithm Steps

1. **Choose K**: Select the number of nearest neighbors to consider
2. **Calculate Distance**: Compute the distance between the new instance and all training instances
3. **Find Neighbors**: Identify the K closest training instances
4. **Vote**: Determine the class by majority vote among the K neighbors
5. **Assign Class**: Return the predicted class for the new instance

#### Distance Metrics

The choice of distance metric is crucial in KNN. The most common options include:

- **Euclidean Distance**: The straight-line distance between two points in n-dimensional space. For points $x = (x_1, x_2, ..., x_n)$ and $y = (y_1, y_2, ..., y_n)$, the Euclidean distance is:
  $$d(x, y) = \sqrt{\sum_{i=1}^{n}(x_i - y_i)^2}$$

- **Manhattan Distance**: The sum of absolute differences along each dimension:
  $$d(x, y) = \sum_{i=1}^{n}|x_i - y_i|$$

- **Minkowski Distance**: A generalization of both Euclidean and Manhattan distance:
  $$d(x, y) = \left(\sum_{i=1}^{n}|x_i - y_i|^p\right)^{1/p}$$

- **Cosine Similarity**: Measures the angle between two vectors, useful for high-dimensional text data.

#### Choosing K

The value of K significantly affects KNN performance:
- **Small K**: Low bias, high variance. More sensitive to noise, risk of overfitting.
- **Large K**: High bias, low variance. Smoother decision boundaries, may miss local patterns.
- **K = 1**: Perfect memorization of training data, very sensitive to outliers.
- **K = sqrt(n)**: A common rule of thumb where n is the number of training samples.

#### The Curse of Dimensionality

KNN suffers from the curse of dimensionality. As the number of features increases, the distance between points becomes less meaningful because all points appear to be roughly equidistant. This is particularly problematic in high-dimensional data like images or text.

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
# Install required packages if not already installed
install.packages("caret", dependencies = TRUE)
install.packages("class")
install.packages("datasets")

# Load the libraries
library(caret)
library(class)
library(datasets)
```

### Step 2: Prepare the Data

```r
# Load the Iris dataset
data(iris)

# Examine the structure
str(iris)
# 'data.frame': 150 obs. of 5 variables:
# $ Sepal.Length: num  5.1 4.9 4.7 ...
# $ Sepal.Width : num  3.5 3 3.2 ...
# $ Petal.Length: num  1.4 1.4 1.3 ...
# $ Petal.Width : num  0.2 0.2 0.2 ...
# $ Species     : Factor w/ 3 levels "setosa","versicolor","virginica"

# Create a training/validation split
set.seed(42)  # For reproducibility
train_index <- createDataPartition(iris$Species, p = 0.7, list = FALSE)

train_data <- iris[train_index, ]
test_data <- iris[-train_index, ]

# Separate features and labels
train_features <- train_data[, -5]  # All columns except Species
test_features <- test_data[, -5]
train_labels <- train_data$Species
test_labels <- test_data$Species
```

### Step 3: Feature Scaling

Feature scaling is critical for KNN because it relies on distance calculations. Without scaling, features with larger ranges would dominate the distance calculation.

```r
# Apply Min-Max normalization (scale to 0-1 range)
preprocess_params <- preProcess(train_features, method = "range")
train_scaled <- predict(preprocess_params, train_features)
test_scaled <- predict(preprocess_params, test_features)

# Verify scaling
summary(train_scaled)
#    Sepal.Length   Sepal.Width    Petal.Length   Petal.Width
#  Min.   :0.000   Min.   :0.00   Min.   :0.000   Min.   :0.00
#  Max.   :1.000   Max.   :1.00   Max.   :1.000   Max.   :1.00
```

### Step 4: Train the KNN Model

```r
# Method 1: Using caret package
knn_model <- train(Species ~ ., 
                 data = train_data, 
                 method = "knn",
                 preProcess = c("center", "scale"),
                 trControl = trainControl(method = "cv", number = 5),
                 tuneGrid = data.frame(k = c(3, 5, 7, 9, 11)))

# Method 2: Using class package directly
# Train labels should be in a vector
predicted <- knn(train = train_scaled, 
                 test = test_scaled, 
                 cl = train_labels, 
                 k = 5)

# View model
print(knn_model)
# k-Nearest Neighbors 
# 
# 150 samples
# 4 predictor
# 3 classes: 'setosa', 'versicolor', 'virginica'
# 
# Pre-processing: centered (4), scaled (4) 
# Resampling: Cross-Validated (5 fold) 
# Summary of sample sizes: 120, 120, 120, 120, 120 
# Resampling results across tuning parameters:
# 
#   k   Accuracy   Kappa    
#   3  0.942     0.913   
#   5  0.952     0.928   
#   7  0.945     0.918   
#   9  0.935     0.903   
#  11  0.933     0.899
```

### Step 5: Make Predictions and Evaluate

```r
# Make predictions on test set
predictions <- predict(knn_model, test_data)

# Create confusion matrix
confusion_matrix <- confusionMatrix(predictions, test_labels)
print(confusion_matrix)
# Confusion Matrix and Statistics
# 
#          Reference
# Prediction   setosa versicolor virginica
#   setosa         15         0         0
#   versicolor     0        14         1
#   virginica     0         1        14
# 
# Overall Accuracy : 0.9556 
# 95% CI: (0.854, 0.994)
# Kappa : 0.9333

# Additional metrics
accuracy <- confusion_matrix$overall["Accuracy"]
print(paste("Accuracy:", round(accuracy, 4)))
# [1] "Accuracy: 0.9556"
```

## Code Examples

### Example 1: Customer Churn Prediction

This example demonstrates using KNN to predict customer churn based on usage patterns.

```r
# Simulate customer churn data
set.seed(123)
n <- 500

# Create synthetic customer data
customer_data <- data.frame(
  account_age = runif(n, 1, 60),           # Account age in months
  monthly_minutes = rnorm(n, 500, 150),  # Monthly usage minutes
  num_support_calls = rpois(n, 2),      # Number of support calls
  plan_upgrades = sample(0:3, n, replace = TRUE),
  churn = factor(sample(c("No", "Yes"), n, prob = c(0.8, 0.2), replace = TRUE))
)

# Add some correlation between features and churn
customer_data$monthly_minutes[customer_data$churn == "Yes"] <- 
  customer_data$monthly_minutes[customer_data$churn == "Yes"] + 200
customer_data$num_support_calls[customer_data$churn == "Yes"] <- 
  customer_data$num_support_calls[customer_data$churn == "Yes"] + 3

# Split data
train_idx <- sample(1:n, n * 0.7)
train_churn <- customer_data[train_idx, ]
test_churn <- customer_data[-train_idx, ]

# Train KNN model with cross-validation
churn_model <- train(churn ~ ., 
                    data = train_churn,
                    method = "knn",
                    preProcess = c("center", "scale"),
                    trControl = trainControl(method = "cv", number = 10),
                    tuneLength = 10)

# Find optimal k
print(churn_model$bestTune)
#    k
# 9  9

# Evaluate on test set
churn_pred <- predict(churn_model, test_churn)
churn_cm <- confusionMatrix(churn_pred, test_churn$churn)
print(churn_cm)
# Overall Accuracy: 0.82
```

### Example 2: Medical Diagnosis Classification

This example shows using KNN for medical diagnosis based on patient symptoms and test results.

```r
# Load breast cancer dataset from MASS package
install.packages("MASS")
library(MASS)

data(biopsy)
str(biopsy)
# 'data.frame': 699 obs. of 11 variables:
# $ ID    : chr  "1000025" "1017122" ...
# $ V1   : num  5 5 3 6 4 2 4 5 4 8 ...
# $ V2   : num  1 4 1 8 1 1 1 4 1 10 ...
# ... (nuclei characteristics)
# $ class: Factor w/ 2 levels "benign","malignant"

# Clean data - remove ID and handle missing values
biopsy_clean <- biopsy[, -1]
biopsy_clean <- na.omit(biopsy_clean)

# Split data
set.seed(456)
train_idx <- createDataPartition(biopsy_clean$class, p = 0.7, list = FALSE)
train_bio <- biopsy_clean[train_idx, ]
test_bio <- biopsy_clean[-train_idx, ]

# Train model with multiple k values
biopsy_knn <- train(class ~ ., 
                    data = train_bio,
                    method = "knn",
                    preProcess = c("center", "scale"),
                    trControl = trainControl(method = "cv", number = 5),
                    tuneGrid = data.frame(k = seq(3, 21, by = 2)))

# Print results
print(biopsy_knn)
plot(biopsy_knn)

# Best k value
biopsy_knn$bestTune
#    k
# 5  5

# Evaluate performance
bio_pred <- predict(biopsy_knn, test_bio)
bio_cm <- confusionMatrix(bio_pred, test_bio$class)
print(bio_cm)
# Sensitivity: 0.9737 (malignant)
# Specificity: 0.9412 (benign)
# Overall Accuracy: 0.9604
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Always Scale Features**: Apply standardization or normalization before applying KNN. Features measured on different scales will incorrectly influence distance calculations.

2. **Use Cross-Validation**: Always use cross-validation to select the optimal K and estimate model performance. Single train-test splits can be misleading.

3. **Odd K Values**: Use odd K values when dealing with binary classification to avoid ties in voting.

4. **Consider Feature Selection**: Reduce dimensionality before applying KNN, especially for high-dimensional data.

5. **Handle Missing Values**: KNN cannot handle missing values. Impute or remove missing data before training.

6. **Use Domain Knowledge**: The choice of K and distance metric should be informed by domain knowledge about the problem.

### Common Pitfalls

1. **Ignoring Feature Scaling**: Failing to scale features is the most common mistake. Features with larger ranges will dominate.

2. **Choosing K Arbitrarily**: Don't just use K = 5 or K = sqrt(n) without validation. Use cross-validation to find the optimal K.

3. **Ignoring the Curse of Dimensionality**: KNN degrades significantly with many features. Consider PCA or feature selection first.

4. **Ignoring Class Imbalance**: In imbalanced datasets, minority classes may never be predicted. Consider class weights or sampling strategies.

5. **Using KNN on Large Datasets**: KNN is computationally expensive for prediction because it must compute distances to all training points. Consider approximate nearest neighbor methods for large datasets.

6. **Ignoring Outliers**: KNN is sensitive to outliers. Consider using distance-weighted KNN or removing outliers first.

## Performance Considerations

### Computational Complexity

- **Training Time**: O(1) - KNN doesn't actually train a model; it just stores the training data.
- **Prediction Time**: O(n × d) where n is the number of training samples and d is the number of features.
- **Space Complexity**: O(n × d) to store the training data.

### Optimization Strategies

1. **KD-Trees**: For low-dimensional data (d < 20), KD-trees can significantly speed up neighbor search.

2. **Approximate Nearest Neighbors**: For very large datasets, consider approximate methods like locality-sensitive hashing (LSH).

3. **Feature Preprocessing**: Reduce dimensionality or select most important features before applying KNN.

4. **Distance-Weighted Voting**: Weight votes by inverse distance to give closer neighbors more influence:

```r
# Using distance-weighted KNN in class package
predictions <- knn(train = train_scaled, 
                  test = test_scaled, 
                  cl = train_labels, 
                  k = 5,
                  prob = TRUE)  # Returns probability information
```

## Related Concepts

- **K-Nearest Neighbors Regression**: KNN can also be used for regression by averaging the target values of K neighbors instead of voting.

- **Weighted KNN**: Closer neighbors receive more weight in voting, reducing sensitivity to K choice.

- **Radius Neighbors**: Instead of K neighbors, use all neighbors within a fixed radius.

- **Cover Trees**: Advanced data structure for fast nearest neighbor search in high dimensions.

- **Local Outlier Factor (LOF)**: Density-based method that uses KNN concepts to detect outliers.

## Exercise Problems

1. **Basic Exercise**: Using the Iris dataset, implement KNN classification with K = 3, 5, 7, and 9. Report accuracy for each K value.

2. **Intermediate Exercise**: Compare Euclidean, Manhattan, and Minkowski distances on a classification problem. Which performs best?

3. **Advanced Exercise**: Implement a custom KNN classifier with distance-weighted voting. Test it on the breast cancer dataset.

4. **Real-World Challenge**: Apply KNN to a real dataset from Kaggle (e.g., Titanic survival prediction). Perform full data preprocessing, cross-validation, and create a final model.

5. **Extension Challenge**: Implement KNN regression for predicting house prices. Compare performance with linear regression.