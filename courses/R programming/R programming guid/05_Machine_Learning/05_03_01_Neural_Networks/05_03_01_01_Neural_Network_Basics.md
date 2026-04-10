# Neural Network Basics

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the fundamental concepts of neural networks
- Implement a simple neural network in R using the `neuralnet` package
- Understand the components: neurons, layers, weights, biases
- Apply backpropagation for training
- Interpret neural network results
- Understand activation functions and their roles

## Theoretical Background

Neural networks are computing systems inspired by biological neural networks. They consist of interconnected nodes (neurons) that process information using connectionist approaches. Neural networks can learn from data, identify patterns, and make predictions without being explicitly programmed.

### The Biological Inspiration

The human brain contains billions of neurons connected via synapses. Each neuron receives signals from other neurons, processes them, and passes signals to other neurons. Artificial neural networks mimic this structure.

### Neural Network Architecture

#### The Neuron (Node)

A single neuron takes multiple inputs, applies weights, adds a bias, passes through an activation function, and produces an output:

$$output = f(\sum_i w_i x_i + b)$$

where:
- $x_i$ = input values
- $w_i$ = weights
- $b$ = bias
- $f$ = activation function

#### Layers

1. **Input Layer**: Receives the raw data features
2. **Hidden Layers**: Intermediate layers that process patterns
3. **Output Layer**: Produces final predictions

#### Network Types

- **Feedforward**: Signals flow in one direction (input → output)
- **Convolutional**: Have convolutional layers for image processing
- **Recurrent**: Have connections that form cycles (for sequences)

### Activation Functions

Activation functions introduce non-linearity, allowing networks to learn complex patterns:

1. **Sigmoid**: Maps to (0, 1), used historically
   $$f(x) = \frac{1}{1 + e^{-x}}$$

2. **Tanh**: Maps to (-1, 1), zero-centered
   $$f(x) = \tanh(x)$$

3. **ReLU**: Returns max(0, x), most common now
   $$f(x) = \max(0, x)$$

4. **Softmax**: For multi-class output, normalizes to probabilities

### How Networks Learn

#### Forward Propagation

1. Inputs flow through the network
2. Each neuron computes: weighted sum + bias
3. Activation function transforms the sum
4. Output is produced

#### Loss Function

Measures prediction error:
- **Regression**: Mean Squared Error (MSE)
- **Classification**: Cross-Entropy / Log Loss

#### Backpropagation

The gradient descent algorithm for training:
1. Compute loss on predictions
2. Calculate gradients going backward
3. Update weights to minimize loss

$$w_{new} = w_{old} - \eta \times \frac{\partial L}{\partial w}$$

where $\eta$ is the learning rate.

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("neuralnet")
install.packages("ggplot2")
install.packages("caret")

library(neuralnet)
library(ggplot2)
library(caret)
```

### Step 2: Prepare Data

```r
# Use iris dataset - predict species
data(iris)

# Create binary classification (two classes)
iris_binary <- iris[iris$Species != "setosa", ]
iris_binary$Species <- factor(iris_binary$Species)

# Scale features (important for neural networks!)
features_scaled <- scale(iris_binary[, -5])

# Create data frame
nn_data <- data.frame(features_scaled, Species = iris_binary$Species)

# Check data
head(nn_data)
dim(nn_data)
```

### Step 3: Build Neural Network

```r
# Simple neural network
# Using neuralnet package

# Formula: species prediction from all features
formula <- Species ~ Sepal.Length + Sepal.Width + Petal.Length + Petal.Width

# Train the network
set.seed(42)
nn_model <- neuralnet(
  formula,
  data = nn_data,
  hidden = c(5),  # One hidden layer with 5 neurons
  act.fct = "logistic",  # Activation function
  linear.output = FALSE,  # Classification
  threshold = 0.01,
  stepmax = 1e+05
)

# View the network
plot(nn_model,
     rep = "best",
     col.hidden = "lightblue",
     col.out = "lightgreen",
     show.weights = TRUE)
```

### Step 4: Make Predictions

```r
# Get predictions
predictions <- predict(nn_model, nn_data[, -5])

# Convert probabilities to classes
predicted_classes <- ifelse(predictions > 0.5, "versicolor", "virginica")
predicted_classes <- factor(predicted_classes)

# Confusion matrix
confusionMatrix(predicted_classes, nn_data$Species)

# Accuracy
accuracy <- sum(predicted_classes == nn_data$Species) / nrow(nn_data)
cat("Training Accuracy:", accuracy, "\n")
```

### Step 5: Evaluate and Visualize

```r
# Plot training progress
plot(nn_model,
     rep = "best",
     main = "Neural Network Training")

# Error by iteration
nn_model$net.result[[1]][1:10]
nn_model$call

# Network weights (first few)
head(nn_model$weights[[1]][[1]])
```

### Step 6: Multiple Hidden Layers

```r
# Deeper network with two hidden layers
set.seed(42)
nn_deep <- neuralnet(
  formula,
  data = nn_data,
  hidden = c(10, 5),  # Two hidden layers
  act.fct = "logistic",
  linear.output = FALSE,
  threshold = 0.01
)

# Plot
plot(nn_deep, rep = "best")

# Evaluate
pred_deep <- predict(nn_deep, nn_data[, -5])
pred_classes_deep <- ifelse(pred_deep > 0.5, "versicolor", "virginica")
pred_classes_deep <- factor(pred_classes_deep)

confusionMatrix(pred_classes_deep, nn_data$Species)
```

## Code Examples

### Example 1: Regression with Neural Networks

This example shows using neural networks for regression.

```r
# Create synthetic regression data
set.seed(123)
n <- 200

# X: random values
x <- seq(0, 10, length.out = n)

# Y: non-linear function with noise
y <- sin(x) + 0.5 * x + rnorm(n, 0, 0.3)

# Create data frame
reg_data <- data.frame(x = x, y = y)

# Train neural network
set.seed(42)
nn_reg <- neuralnet(
  y ~ x,
  data = reg_data,
  hidden = c(10),
  act.fct = "tanh",
  linear.output = TRUE,  # Regression
  threshold = 0.01
)

# Make predictions
predictions <- predict(nn_reg, reg_data[, "x", drop = FALSE])

# Plot results
plot_df <- data.frame(x = x, y = y, predicted = predictions)
ggplot(plot_df, aes(x = x)) +
  geom_point(aes(y = y), alpha = 0.5) +
  geom_line(aes(y = predicted), color = "red") +
  ggtitle("Neural Network Regression") +
  theme_minimal()
```

### Example 2: Classification on Breast Cancer Data

This example demonstrates binary classification.

```r
# Create synthetic breast cancer data
set.seed(456)
n <- 300

# Features
cancer_data <- data.frame(
  radius_mean = rnorm(n, 15, 5),
  texture_mean = rnorm(n, 20, 5),
  perimeter_mean = rnorm(n, 100, 30),
  area_mean = rnorm(n, 800, 200),
  smoothness_mean = rnorm(n, 0.09, 0.02),
  diagnosis = factor(sample(c("benign", "malignant"), n, 
                           prob = c(0.65, 0.35), replace = TRUE))
)

# Add some correlation with diagnosis
cancer_data$radius_mean[cancer_data$diagnosis == "malignant"] <- 
  cancer_data$radius_mean[cancer_data$diagnosis == "malignant"] + 10

# Scale features
features <- scale(cancer_data[, -5])
cancer_df <- data.frame(features, diagnosis = cancer_data$diagnosis)

# Train-test split
set.seed(42)
train_idx <- sample(1:n, 0.7 * n)
train_cancer <- cancer_df[train_idx, ]
test_cancer <- cancer_df[-train_idx, ]

# Train neural network
set.seed(42)
nn_cancer <- neuralnet(
  diagnosis ~ .,
  data = train_cancer,
  hidden = c(10, 5),
  act.fct = "logistic",
  linear.output = FALSE,
  threshold = 0.01
)

# Predictions
pred_prob <- predict(nn_cancer, test_cancer[, -5])
pred_classes <- ifelse(pred_prob > 0.5, "malignant", "benign")
pred_classes <- factor(pred_classes)

# Evaluate
confusionMatrix(pred_classes, test_cancer$diagnosis)
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Scale Data**: Always scale features to similar ranges
2. **Start Simple**: Begin with one hidden layer
3. **Use Cross-Validation**: For hyperparameter selection
4. **Monitor Overfitting**: Check training vs. validation error
5. **Reasonable Learning Rate**: Too high = unstable, too low = slow

### Common Pitfalls

1. **Not Scaling Data**: Network won't converge properly
2. **Too Many Neurons**: Causes overfitting
3. **Too Few Iterations**: Network may not converge
4. **Local Minima**: Different seeds may give different results
5. **Vanishing Gradients**: Very deep networks may not train well

## Performance Considerations

### Network Architecture Guidelines

| Problem Type | Hidden Layers | Neurons per Layer |
|--------------|---------------|-------------------|
| Simple (linear) | 0 | - |
| Moderate | 1-2 | 5-20 |
| Complex | 2-3 | 20-100 |
| Very Complex | 3+ | 100+ |

### Computational Complexity

- **Training**: O(epochs × samples × neurons × connections)
- **Prediction**: Much faster than training (no gradient updates)
- **Memory**: O(weights + activations)

## Related Concepts

- **Deep Learning**: Neural networks with many layers
- **Convolutional Neural Networks**: For image data
- **Recurrent Neural Networks**: For sequence data
- **Backpropagation**: The training algorithm
- **Regularization**: Dropout, weight decay

## Exercise Problems

1. **Basic**: Build a simple neural network for Iris classification.

2. **Intermediate**: Experiment with different hidden layer sizes.

3. **Advanced**: Implement a network with multiple hidden layers.

4. **Real-World Challenge**: Apply neural networks to a real classification problem.

5. **Extension**: Compare neural network performance with other classifiers.