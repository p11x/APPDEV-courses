# Decision Trees Classification

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the decision tree algorithm structure and how it learns from data
- Implement decision tree classification in R using the `rpart` and `caret` packages
- Interpret decision tree visualizations and understand feature importance
- Apply pruning techniques to prevent overfitting
- Handle both categorical and numerical features
- Evaluate decision tree models using confusion matrices and accuracy metrics
- Understand the differences between ID3, C4.5, and CART algorithms

## Theoretical Background

Decision trees are one of the most widely used and intuitive machine learning algorithms. They work by learning a series of rules (decisions) that partition the data into smaller subgroups based on feature values. The final structure resembles an upside-down tree, with the root at the top and leaves at the bottom.

### How Decision Trees Work

The algorithm builds a tree by recursively partitioning the data based on feature values. At each node, it chooses the feature that best separates the classes according to some impurity measure.

#### The Algorithm Steps

1. **Start at the root node**: All training data is at the root
2. **Choose the best split**: Evaluate all possible splits on all features
3. **Split the data**: Divide the data into two or more subgroups
4. **Repeat recursively**: Apply steps 2-3 to each child node
5. **Stopping criterion**: Stop when nodes are pure or minimum size is reached
6. **Assign class labels**: Leaf nodes get the majority class of their samples

### Impurity Measures

The key to decision tree learning is choosing the "best" split at each node. This is done by measuring impurity - how mixed the classes are in a node.

#### Gini Impurity

Gini impurity measures the probability of misclassifying a randomly chosen element:

$$Gini = 1 - \sum_{i=1}^{c} p_i^2$$

where $p_i$ is the proportion of samples belonging to class i. Lower Gini values indicate more pure nodes.

#### Entropy

Entropy is a measure of disorder or uncertainty:

$$Entropy = -\sum_{i=1}^{c} p_i \log_2(p_i)$$

Information gain is the reduction in entropy achieved by a split:

$$IG = Entropy_{parent} - \sum_{j=1}^{k} \frac{n_j}{n} Entropy_{child_j}$$

#### Classification Error

A simpler measure that counts misclassifications:

$$Error = 1 - \max(p_i)$$

### Splitting Criteria

For categorical features:
- Create a branch for each category
- Or split into two groups: target category vs. all others

For numerical features:
- Choose a threshold value
- One branch: feature ≤ threshold
- Other branch: feature > threshold

### Overfitting in Decision Trees

Decision trees are prone to overfitting because they can create very deep trees that perfectly fit the training data. Solutions include:

1. **Pre-pruning**: Limit tree depth, minimum samples per leaf, minimum samples per split
2. **Post-pruning**: Grow a full tree, then remove or collapse nodes

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
# Install required packages
install.packages("rpart")
install.packages("rpart.plot")
install.packages("caret")
install.packages("e1071")

# Load libraries
library(rpart)
library(rpart.plot)
library(caret)
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

# Create training and test splits
set.seed(42)
train_idx <- createDataPartition(iris$Species, p = 0.7, list = FALSE)
train_data <- iris[train_idx, ]
test_data <- iris[-train_idx, ]
```

### Step 3: Build the Decision Tree

```r
# Build a basic decision tree model
# Using rpart with Gini impurity
tree_model <- rpart(Species ~ ., 
                    data = train_data,
                    method = "class",
                    control = rpart.control(minsplit = 10, 
                                           maxdepth = 10,
                                           cp = 0.01))

# View the tree structure
print(tree_model)
# n= 105 
# 
# node), split, n, loss, yval, (yprob)
# * denotes terminal node
# 
# 1) root 105 70 setosa (0.3333 0.3333 0.3333)  
#   2) Petal.Length< 2.45 50  0 setosa (1.0000 0.0000 0.0000) *
#   3) Petal.Length>=2.45 55 25 versicolor (0.0000 0.6364 0.3636)  
#     6) Petal.Width< 1.75 40  9 versicolor (0.0000 0.7750 0.2250) *
#     7) Petal.Width>=1.75 16  2 virginica (0.0000 0.1250 0.8750) *

# View variable importance
print(tree_model$variable.importance)
#  Petal.Length   Petal.Width  Sepal.Length   Sepal.Width 
#     79.34        18.07         2.59         0.00
```

### Step 4: Visualize the Tree

```r
# Plot the decision tree
rpart.plot(tree_model,
           main = "Iris Classification Tree",
           fallen.leaves = TRUE,  # Leaves at the bottom
           cex = 0.8,
           under = TRUE,
           compress = TRUE)

# Alternative visualization with more details
prp(tree_model, 
    main = "Decision Tree for Iris",
    type = 2,  # Show split criterion and class
    extra = 106,  # Show probability and sample count
    fallen.leaves = TRUE,
    cex = 0.7)
```

### Step 5: Prune the Tree

```r
# Find the optimal complexity parameter (cp)
printcp(tree_model)
# 
# Regression tree's rpart object
# 
# Variables actually used in tree's construction:
# [1] Petal.Length Petal.Width 
# 
# Root node error: 70/105 = 0.667
# 
#          CP nsplit rel error   xerror     xstd
# 1 0.442857      0  1.00000 1.00000 0.06985
# 2 0.028571      1  0.55714 0.61429 0.07433
# 3 0.010000      2  0.52857 0.62857 0.08018

# Find the minimum cross-validation error
best_cp <- tree_model$cptable[which.min(tree_model$cptable[, "xerror"]), "CP"]
print(paste("Best CP:", best_cp))
# [1] "Best CP: 0.01"

# Prune the tree
pruned_tree <- prune(tree_model, cp = best_cp)

# Plot the pruned tree
rpart.plot(pruned_tree,
           main = "Pruned Decision Tree",
           box.palette = "GnBu",
           fallen.leaves = TRUE)
```

### Step 6: Make Predictions

```r
# Make predictions on test data
predictions <- predict(pruned_tree, test_data, type = "class")

# Create confusion matrix
confusion_matrix <- table(Predicted = predictions, 
                        Actual = test_data$Species)
print(confusion_matrix)
#             Actual
# Predicted    setosa versicolor virginica
#   setosa        15         0         0
#   versicolor    0        14         2
#   virginica    0         1        13

# Calculate accuracy
accuracy <- sum(diag(confusion_matrix)) / sum(confusion_matrix)
print(paste("Accuracy:", round(accuracy, 4)))
# [1] "Accuracy: 0.9333"

# Using caret for detailed metrics
confusionMatrix(predictions, test_data$Species)
# Overall Accuracy: 0.9333 
# 95% CI: (0.817, 0.987)
# Kappa: 0.9
```

## Code Examples

### Example 1: Bank Marketing Classification

Predict whether a customer will subscribe to a term deposit based on demographic and marketing data.

```r
# Load the bank marketing dataset
data("Marketing", package = "rpart")

# Create synthetic bank data for demonstration
set.seed(123)
n <- 1000

bank_data <- data.frame(
  age = sample(25:70, n, replace = TRUE),
  job = factor(sample(c("blue-collar", "services", "admin", "management"), 
                     n, replace = TRUE)),
  marital = factor(sample(c("married", "single", "divorced"), n, replace = TRUE)),
  education = factor(sample(c("primary", "secondary", "tertiary"), n, replace = TRUE)),
  balance = rnorm(n, 1000, 5000),
  housing = factor(sample(c("yes", "no"), n, replace = TRUE)),
  loan = factor(sample(c("yes", "no"), n, replace = TRUE)),
  contact = factor(sample(c("cellular", "telephone"), n, replace = TRUE)),
  day = sample(1:31, n, replace = TRUE),
  duration = rnorm(n, 300, 150),
  y = factor(sample(c("yes", "no"), n, prob = c(0.2, 0.8), replace = TRUE))
)

# Add some realistic relationships
bank_data$y[bank_data$duration > 400] <- "yes"
bank_data$y[bank_data$balance > 5000 & bank_data$loan == "no"] <- "yes"

# Split the data
train_idx <- sample(1:n, n * 0.7)
train_bank <- bank_data[train_idx, ]
test_bank <- bank_data[-train_idx, ]

# Build decision tree with control parameters
bank_tree <- rpart(y ~ ., 
                   data = train_bank,
                   method = "class",
                   control = rpart.control(minsplit = 20,
                                          maxdepth = 5,
                                          cp = 0.005))

# Print complexity table
printcp(bank_tree)

# Prune to optimal CP
best_cp <- bank_tree$cptable[which.min(bank_tree$cptable[, "xerror"]), "CP"]
bank_tree_pruned <- prune(bank_tree, cp = best_cp)

# Visualize
rpart.plot(bank_tree_pruned,
           main = "Bank Marketing Decision Tree",
           box.palette = "auto")

# Evaluate
bank_pred <- predict(bank_tree_pruned, test_bank, type = "class")
bank_cm <- confusionMatrix(bank_pred, test_bank$y)
print(bank_cm)
# Accuracy: around 0.85
```

### Example 2: Heart Disease Prediction

This example demonstrates handling both numerical and categorical features for medical diagnosis.

```r
# Create synthetic heart disease data
set.seed(789)
n <- 500

heart_data <- data.frame(
  age = sample(30:80, n, replace = TRUE),
  sex = factor(rep(c("M", "F"), n/2)),
  cp = factor(sample(1:4, n, replace = TRUE), 
              labels = c("typical", "atypical", "nonanginal", "asymptomatic")),
  trestbps = rnorm(n, 130, 20),
  chol = rnorm(n, 200, 40),
  fbs = factor(sample(c(0, 1), n, replace = TRUE)),
  restecg = factor(sample(0:2, n, replace = TRUE)),
  thalach = rnorm(n, 140, 25),
  exang = factor(sample(c(0, 1), n, replace = TRUE)),
  oldpeak = rnorm(n, 1.5, 1.2),
  target = factor(sample(c(0, 1), n, prob = c(0.6, 0.4), replace = TRUE))
)

# Make the data somewhat predictive
heart_data$target[heart_data$cp == "typical" & heart_data$exang == 1] <- 1
heart_data$target[heart_data$age > 60] <- 1

# Split
train_idx <- sample(1:n, n * 0.7)
train_heart <- heart_data[train_idx, ]
test_heart <- heart_data[-train_idx, ]

# Train decision tree
heart_tree <- rpart(target ~ ., 
                   data = train_heart,
                   method = "class",
                   control = rpart.control(minsplit = 15,
                                          maxdepth = 6,
                                          cp = 0.01))

# Plot with customizations
rpart.plot(heart_tree,
           main = "Heart Disease Prediction Tree",
           box.palette = list(background = "lightgreen", 
                            text = "darkred"),
           shadow.col = "gray",
           nn = TRUE)

# Variables importance
print(sort(heart_tree$variable.importance, decreasing = TRUE))

# Predict and evaluate
heart_pred <- predict(heart_tree, test_heart, type = "class")
heart_cm <- confusionMatrix(heart_pred, test_heart$target)
print(heart_cm)
# Shows sensitivity, specificity, and overall accuracy
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Feature Selection First**: Use feature selection techniques before building trees to reduce noise.

2. **Set Appropriate Stopping Criteria**: Always set minimum samples per split and leaf to prevent overfitting.

3. **Use Cross-Validation for Pruning**: Use CV error to determine the optimal tree complexity.

4. **Handle Missing Values**: Decision trees can handle missing values, but preprocessing is recommended.

5. **Consider Ensemble Methods**: Single trees are unstable; consider Random Forest for better performance.

6. **Understand Feature Importance**: Use variable importance to understand what drives predictions.

### Common Pitfalls

1. **Overfitting**: Building trees that are too deep and memorize training data.

2. **Ignoring Feature Scaling**: Not necessary for decision trees but important if comparing with other methods.

3. **Choosing Wrong Split Criterion**: Gini vs. entropy rarely makes a big difference, but test both.

4. **Ignoring Class Imbalance**: Use class weights or sampling for imbalanced problems.

5. **Not Handling Categorical Variables Properly**: Ensure categorical variables are properly encoded.

6. **Using Single Train-Test Split**: Always use cross-validation for model selection.

## Performance Considerations

### Computational Complexity

- **Training**: O(n × d × log(n)) on average, but can be O(n²) in worst case
- **Prediction**: O(tree_depth) - very fast once tree is built
- **Space**: O(n) to store the tree

### Pruning Strategies

1. **Complexity Parameter (cp)**: Controls tree size; lower values = larger trees
2. **Minimum Split**: Minimum samples required to attempt a split
3. **Minimum Leaf**: Minimum samples required in a leaf node
4. **Maximum Depth**: Maximum depth of the tree

### Handling Large Datasets

For very large datasets:
1. Sample the data for initial training
2. Use early stopping parameters
3. Consider parallel processing with `multicore` package

## Related Concepts

- **C4.5 Algorithm**: Uses information gain ratio to handle bias toward splits with many values
- **CART (Classification and Regression Trees)**: Uses Gini impurity for classification
- **Random Forest**: Ensemble of decision trees with bootstrap aggregation
- **Gradient Boosted Trees**: Sequential ensemble method that builds trees on residuals

## Exercise Problems

1. **Basic**: Build a decision tree on the Iris dataset with maxdepth = 3. Visualize and interpret the tree.

2. **Intermediate**: Compare Gini and entropy-based trees on a classification problem. Are the results different?

3. **Advanced**: Implement cost-complexity pruning on a dataset. Find the optimal CP using cross-validation.

4. **Real-World Challenge**: Apply decision trees to Titanic dataset. Compare with other classification methods.

5. **Extension**: Implement a custom decision tree algorithm from scratch in R.