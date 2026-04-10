# Support Vector Machines (SVM) Classification

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the geometric foundations of Support Vector Machines
- Implement SVM classification in R using the `e1071` and `kernlab` packages
- Distinguish between linear and non-linear SVM kernels
- Tune the key hyperparameters: C (regularization) and kernel parameters
- Apply SVM to multi-class classification problems
- Visualize SVM decision boundaries
- Handle class imbalance and large datasets efficiently

## Theoretical Background

Support Vector Machines (SVM) are one of the most powerful and widely used classification algorithms. SVM finds the optimal hyperplane that maximally separates classes in feature space. The key advantage is that SVM focuses on the most difficult samples - the support vectors - making it robust to outliers.

### How SVM Works

The core idea is to find the hyperplane that maximizes the margin between classes. The margin is the distance between the hyperplane and the nearest data points from each class.

#### The Hyperplane

In a d-dimensional space, a hyperplane is a (d-1)-dimensional subspace defined by:
$$w \cdot x + b = 0$$

where:
- $w$ is the weight vector (normal to the hyperplane)
- $x$ is the feature vector
- $b$ is the bias term

For a binary classification problem with classes -1 and +1, the decision function is:
$$f(x) = sign(w \cdot x + b)$$

#### The Margin and Support Vectors

The functional margin defines the distance from a point to the hyperplane:
$$\gamma = y_i(w \cdot x_i + b)$$

The geometric margin is normalized by the weight vector:
$$\gamma_{geo} = \frac{y_i(w \cdot x_i + b)}{||w||}$$

Support vectors are the data points that lie exactly on the margin boundaries. They are the most critical samples - removing any other point wouldn't change the optimal hyperplane.

### The Optimization Problem

SVM finds the optimal hyperplane by solving:
$$\min_{w,b} \frac{1}{2}||w||^2 + C\sum_{i=1}^{n}\xi_i$$

subject to:
$$y_i(w \cdot x_i + b) \geq 1 - \xi_i$$
$$\xi_i \geq 0$$

where:
- $\xi_i$ are slack variables allowing soft margins
- C is the regularization parameter controlling the trade-off

#### The Role of C

- **Small C**: Larger margins, more tolerance for misclassification, higher bias
- **Large C**: Smaller margins, stricter classification, higher variance

### Kernel Trick

The kernel trick allows SVM to learn non-linear decision boundaries by implicitly mapping data to higher-dimensional space.

#### Linear Kernel
$$K(x_i, x_j) = x_i \cdot x_j$$

#### Polynomial Kernel
$$K(x_i, x_j) = (\gamma \cdot x_i \cdot x_j + r)^d$$

#### Radial Basis Function (RBF) Kernel
$$K(x_i, x_j) = exp(-\gamma ||x_i - x_j||^2)$$

The RBF kernel is the most commonly used and can model complex decision boundaries.

### Multi-class SVM

SVM is naturally binary classification. For multi-class problems:
1. **One-vs-One**: Train classifiers for each pair of classes, use voting
2. **One-vs-All**: Train one classifier per class against all others

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
# Install required packages
install.packages("e1071")
install.packages("kernlab")
install.packages("caret")
install.packages("ggplot2")

# Load libraries
library(e1071)
library(kernlab)
library(caret)
library(ggplot2)
```

### Step 2: Prepare the Data

```r
# Load the Iris dataset
data(iris)

# Use only two classes for initial demonstration
iris_binary <- iris[iris$Species != "setosa", ]
iris_binary$Species <- factor(iris_binary$Species)

# Create training and test splits
set.seed(42)
train_idx <- createDataPartition(iris_binary$Species, p = 0.7, list = FALSE)
train_data <- iris_binary[train_idx, ]
test_data <- iris_binary[-train_idx, ]
```

### Step 3: Train the SVM Model

```r
# Train a basic linear SVM
svm_model <- svm(Species ~ ., 
                 data = train_data,
                 kernel = "linear",
                 cost = 1,
                 scale = TRUE)

# View model summary
print(svm_model)
# 
# Call:
#   svm(formula = Species ~ ., data = train_data, kernel = "linear", cost = 1, 
#       scale = TRUE)
# 
# 
# Parameters:
#    SVM-Type:  C-classification 
#    cost:  1 
#    gamma:  0.25 
# 
# Number of Support Vectors:  24
# 
#  ( 12 12 )

# Predictions
predictions <- predict(svm_model, test_data)

# Confusion matrix
confusionMatrix(predictions, test_data$Species)
```

### Step 4: RBF Kernel SVM

```r
# Train SVM with RBF kernel
svm_rbf <- svm(Species ~ ., 
                data = train_data,
                kernel = "radial",
                cost = 1,
                gamma = 0.5,
                scale = TRUE)

# Print model
print(svm_rbf)
# Number of Support Vectors:  18
#  ( 9 9 )

# Evaluate
pred_rbf <- predict(svm_rbf, test_data)
confusionMatrix(pred_rbf, test_data$Species)
```

### Step 5: Visualize Decision Boundary

```r
# Create a grid for visualization
iris_grid <- iris_binary[, c("Petal.Length", "Petal.Width", "Species")]
x_min <- min(iris_grid$Petal.Length) - 0.5
x_max <- max(iris_grid$Petal.Length) + 0.5
y_min <- min(iris_grid$Petal.Width) - 0.5
y_max <- max(iris_grid$Petal.Width) + 0.5

grid_points <- expand.grid(
  Petal.Length = seq(x_min, x_max, length.out = 100),
  Petal.Width = seq(y_min, y_max, length.out = 100)
)

# Predict on grid
grid_pred <- predict(svm_rbf, grid_points)

# Plot using ggplot2
ggplot() +
  geom_tile(data = cbind(grid_points, Species = grid_pred), 
            aes(x = Petal.Length, y = Petal.Width, fill = Species), 
            alpha = 0.3) +
  geom_point(data = iris_grid, 
            aes(x = Petal.Length, y = Petal.Width, color = Species), 
            size = 2) +
  ggtitle("SVM Decision Boundary (RBF Kernel)") +
  theme_minimal()
```

### Step 6: Hyperparameter Tuning

```r
# Using caret for automatic tuning
svm_tuned <- train(Species ~ ., 
                   data = train_data,
                   method = "svmRadial",
                   preProcess = c("center", "scale"),
                   trControl = trainControl(method = "cv", number = 5),
                   tuneGrid = expand.grid(C = c(0.1, 1, 10, 100),
                                        sigma = c(0.01, 0.1, 1, 10)))

# View best parameters
print(svm_tuned$bestTune)
#    C sigma
# 4  1     1

# Print final model
print(svm_tuned)
# Accuracy was used to select the optimal model using the largest value.
# The final values used for the model were C = 1 and sigma = 1
```

## Code Examples

### Example 1: Spam Classification with SVM

This example demonstrates using SVM for email spam classification.

```r
# Create synthetic spam data
set.seed(123)
n <- 500

spam_data <- data.frame(
  word_count = rpois(n, 150),
  capital_letters = rnorm(n, 50, 30),
  num_links = rpois(n, 3),
  num_images = rpois(n, 1),
  num_exclamations = rpois(n, 2),
  num_questions = rpois(n, 1),
  has_urgent = rbinom(n, 1, 0.1),
  has_free = rbinom(n, 1, 0.15),
  has_winner = rbinom(n, 1, 0.1),
  has_verify = rbinom(n, 1, 0.2),
  spam = factor(sample(c("ham", "spam"), n, prob = c(0.7, 0.3), replace = TRUE))
)

# Add relationships
spam_data$spam[spam_data$has_winner == 1] <- "spam"
spam_data$spam[spam_data$num_links > 5] <- "spam"
spam_data$spam[spam_data$capital_letters > 100] <- "spam"

# Split
train_idx <- sample(1:n, n * 0.7)
train_spam <- spam_data[train_idx, ]
test_spam <- spam_data[-train_idx, ]

# Train SVM
spam_svm <- svm(spam ~ ., 
                data = train_spam,
                kernel = "radial",
                cost = 10,
                gamma = 0.1,
                scale = TRUE)

# Evaluate
spam_pred <- predict(spam_svm, test_spam)
spam_cm <- confusionMatrix(spam_pred, test_spam$spam)
print(spam_cm)

# View support vectors
spam_svm$index
length(spam_svm$index)  # Number of support vectors
```

### Example 2: Medical Diagnosis

This example shows SVM handling complex medical data patterns.

```r
# Create synthetic medical diagnosis data
set.seed(789)
n <- 400

medical_data <- data.frame(
  age = sample(20:90, n, replace = TRUE),
  bmi = rnorm(n, 28, 5),
  blood_pressure_sys = rnorm(n, 130, 20),
  blood_pressure_dia = rnorm(n, 85, 12),
  cholesterol = rnorm(n, 200, 40),
  blood_sugar = rnorm(n, 100, 25),
  heart_rate = rnorm(n, 75, 12),
  diagnosis = factor(sample(c("healthy", "disease"), n, 
                            prob = c(0.6, 0.4), replace = TRUE))
)

# Add realistic relationships
medical_data$diagnosis[medical_data$cholesterol > 240] <- "disease"
medical_data$diagnosis[medical_data$blood_sugar > 130] <- "disease"
medical_data$diagnosis[medical_data$blood_pressure_sys > 150] <- "disease"
medical_data$diagnosis[medical_data$bmi > 35] <- "disease"

# Split
train_idx <- sample(1:n, n * 0.7)
train_med <- medical_data[train_idx, ]
test_med <- medical_data[-train_idx, ]

# Train SVM with RBF kernel
med_svm <- svm(diagnosis ~ ., 
                data = train_med,
                kernel = "radial",
                cost = 1,
                gamma = 0.1,
                scale = TRUE)

# Predictions
med_pred <- predict(med_svm, test_med)
med_cm <- confusionMatrix(med_pred, test_med$diagnosis)
print(med_cm)

# Feature importance via permutation
print(med_svm$coefs)  # Coefficients for support vectors
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Always Scale Features**: SVM is distance-based; scale features to similar ranges.

2. **Start with RBF Kernel**: Default choice for most problems; linear only when clearly linear separable.

3. **Use Cross-Validation for C and gamma**: Default values rarely optimal.

4. **Check Support Vectors**: If many support vectors (>50% of data), consider adding features.

5. **Handle Class Imbalance**: Use class.weights parameter for imbalanced data.

### Common Pitfalls

1. **Not Scaling Features**: Results in poor performance.

2. **Wrong Kernel Choice**: Linear kernel won't work for complex patterns.

3. **C Too Large**: Can cause overfitting and slow training.

4. **C Too Small**: Results in underfitting.

5. **Gamma Too Large**: Overfits to training data.

6. **Gamma Too Small**: Model becomes too regularized.

## Performance Considerations

### Computational Complexity

- **Training**: O(n² × d) to O(n³ × d) depending on kernel
- **Prediction**: O(n_sv × d) where n_sv is number of support vectors
- **Space**: O(n × d + n_sv × d)

### Hyperparameter Guidelines

| Parameter | Small Value | Large Value |
|-----------|------------|-------------|
| C | Underfitting | Overfitting |
| gamma | Smooth boundary | Complex boundary |

### Optimization Tips

1. Use `kernlab` for larger datasets (more efficient)
2. Set cache size appropriately for training
3. Consider linear SVM for very high-dimensional data

## Related Concepts

- **Kernel PCA**: Non-linear PCA using kernel trick
- **Support Vector Regression (SVR)**: SVM for regression problems
- **Relevance Vector Machines (RVM)**: Sparse Bayesian kernel method

## Exercise Problems

1. **Basic**: Train SVM on Iris data. Compare linear and RBF kernels.

2. **Intermediate**: Tune C and gamma using grid search with cross-validation.

3. **Advanced**: Visualize decision boundaries for 2D data.

4. **Real-World Challenge**: Apply SVM to credit risk prediction.

5. **Extension**: Compare SVM with Random Forest on the same dataset.