# Keras Basics in R

## Learning Objectives

By the end of this chapter, students will be able to:
- Set up Keras with TensorFlow in R
- Understand the Keras Sequential API
- Build and train neural network models
- Compile models with appropriate optimizers and loss functions
- Evaluate and make predictions with Keras models
- Understand layers and their configurations
- Apply Keras to classification and regression problems

## Theoretical Background

Keras is a high-level neural networks API that runs on top of TensorFlow. Originally developed in Python, Keras has been ported to R through the `keras` R package. It provides a clean, simple interface for building deep learning models while allowing access to lower-level TensorFlow operations when needed.

### Why Keras?

1. **User-Friendly**: Simple, consistent API
2. **Modular**: Easy to combine layers and models
3. **Extensible**: Custom components possible
4. **Production-Ready**: Can deploy models to production
5. **Backend Support**: Works with TensorFlow, CNTK, Theano

### Keras Architecture

#### Sequential Model

The simplest approach - a linear stack of layers:

```r
model <- keras_model_sequential() %>%
  layer_dense(units = 64, activation = "relu", input_shape = c(784)) %>%
  layer_dropout(0.2) %>%
  layer_dense(units = 10, activation = "softmax")
```

#### Functional API

For more complex architectures with multiple inputs/outputs:

```r
# Define multiple inputs
input1 <- layer_input(shape = c(32))
input2 <- layer_input(shape = c(32))

# Process each
out1 <- input1 %>% layer_dense(64)
out2 <- input2 %>% layer_dense(64)

# Combine
combined <- layer_concatenate(c(out1, out2)) %>% layer_dense(1)
model <- keras_model(c(input1, input2), combined)
```

### Core Components

1. **Layers**: Building blocks (Dense, Conv2D, LSTM, Dropout, etc.)
2. **Model**: Container for layers
3. **Optimizer**: Gradient descent algorithm (Adam, SGD, RMSprop)
4. **Loss Function**: Objective to minimize (categorical_crossentropy, mse)
5. **Metrics**: Evaluation measures (accuracy, AUC)

### Training Process

1. **Compile**: Configure optimizer, loss, metrics
2. **Fit**: Train on data with epochs and batch size
3. **Evaluate**: Assess on test data
4. **Predict**: Generate predictions on new data

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
# Install keras package
install.packages("keras")
install.packages("tensorflow")

# Load
library(keras)
library(tensorflow)

# Install TensorFlow (one-time setup)
install_tensorflow()
```

### Step 2: Prepare Data

```r
# Load sample data
data <- dataset_mnist()

# Training and test data
train_images <- data$train$x
train_labels <- data$train$y
test_images <- data$test$x
test_labels <- data$test$y

# Reshape and normalize
train_images <- array_reshape(train_images, c(60000, 28, 28))
test_images <- array_reshape(test_images, c(10000, 28, 28))

# Normalize to [0, 1]
train_images <- train_images / 255
test_images <- test_images / 255

# One-hot encode labels
train_labels <- to_categorical(train_labels, 10)
test_labels <- to_categorical(test_labels, 10)

# Check dimensions
dim(train_images)  # c(60000, 28, 28)
dim(train_labels)  # c(60000, 10)
```

### Step 3: Build the Model

```r
# Create sequential model
model <- keras_model_sequential() %>%
  # Flatten 28x28 to 784
  layer_flatten(input_shape = c(28, 28)) %>%
  # Dense layer 1
  layer_dense(units = 128, activation = "relu") %>%
  # Dropout for regularization
  layer_dropout(0.2) %>%
  # Dense layer 2
  layer_dense(units = 64, activation = "relu") %>%
  layer_dropout(0.2) %>%
  # Output layer (10 classes)
  layer_dense(units = 10, activation = "softmax")

# View model architecture
summary(model)
```

### Step 4: Compile the Model

```r
# Compile model
model %>% compile(
  optimizer = "adam",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)

# Model summary shows parameters
# Total parameters: 235,146
```

### Step 5: Train the Model

```r
# Train the model
history <- model %>% fit(
  train_images,
  train_labels,
  epochs = 10,
  batch_size = 128,
  validation_split = 0.2,
  verbose = 2
)

# View training history
plot(history)
```

### Step 6: Evaluate and Predict

```r
# Evaluate on test data
model %>% evaluate(test_images, test_labels, verbose = 0)
# $loss: 0.1...
# $accuracy: 0.97...

# Make predictions
predictions <- model %>% predict(test_images)
dim(predictions)  # c(10000, 10)

# Get predicted classes
predicted_classes <- apply(predictions, 1, which.max) - 1
head(predicted_classes)

# Confusion matrix
actual_classes <- apply(test_labels, 1, which.max) - 1
table(predicted_classes, actual_classes)
```

## Code Examples

### Example 1: Binary Classification

This example shows binary classification with Keras.

```r
# Create synthetic binary classification data
set.seed(123)
n <- 1000

# Generate features
x <- matrix(rnorm(n * 20), nrow = n, ncol = 20)

# Generate labels (with some pattern)
y_prob <- 1 / (1 + exp(-(x[, 1] * 0.5 + x[, 2] * 0.3 + x[, 5:10] %*% rep(0.3, 6))))
y <- rbinom(n, 1, y_prob)

# Split data
train_idx <- sample(1:n, 0.7 * n)
x_train <- x[train_idx, ]
x_test <- x[-train_idx, ]
y_train <- y[train_idx]
y_test <- y[-train_idx]

# Build model
model_binary <- keras_model_sequential() %>%
  layer_dense(units = 32, activation = "relu", input_shape = c(20)) %>%
  layer_dense(units = 16, activation = "relu") %>%
  layer_dense(units = 1, activation = "sigmoid")

# Compile
model_binary %>% compile(
  optimizer = "adam",
  loss = "binary_crossentropy",
  metrics = c("accuracy")
)

# Train
history_binary <- model_binary %>% fit(
  x_train, y_train,
  epochs = 20,
  batch_size = 32,
  validation_split = 0.2,
  verbose = 0
)

# Evaluate
model_binary %>% evaluate(x_test, y_test)
```

### Example 2: Regression Model

This example demonstrates regression with Keras.

```r
# Create synthetic regression data
set.seed(456)
n <- 1000

# Features
x_reg <- matrix(rnorm(n * 10), nrow = n, ncol = 10)

# Target: linear combination with noise
y_reg <- x_reg[, 1] * 2 + x_reg[, 2] * (-1.5) + x_reg[, 3] * 0.5 +
  rnorm(n, 0, 0.5)

# Split
train_idx <- sample(1:n, 0.7 * n)
x_train <- x_reg[train_idx, ]
x_test <- x_reg[-train_idx, ]
y_train <- y_reg[train_idx]
y_test <- y_reg[-train_idx]

# Build regression model
model_reg <- keras_model_sequential() %>%
  layer_dense(units = 64, activation = "relu", input_shape = c(10)) %>%
  layer_dense(units = 32, activation = "relu") %>%
  layer_dense(units = 1)  # No activation for regression

# Compile with MSE loss
model_reg %>% compile(
  optimizer = "adam",
  loss = "mse",
  metrics = c("mae")
)

# Train
history_reg <- model_reg %>% fit(
  x_train, y_train,
  epochs = 50,
  batch_size = 32,
  validation_split = 0.2,
  verbose = 0
)

# Evaluate
model_reg %>% evaluate(x_test, y_test)
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Normalize Data**: Scale inputs to [0,1] or standardize
2. **Use Validation Split**: Monitor for overfitting
3. **Start Simple**: Begin with baseline, add complexity
4. **Use Callbacks**: Early stopping, learning rate reduction
5. **Save Models**: Use save_model_hdf5() for persistence

### Common Pitfalls

1. **Wrong Output Activation**: Classification needs softmax/sigmoid
2. **Wrong Loss Function**: Use appropriate loss for problem type
3. **Learning Rate Too High**: Loss may explode
4. **Missing Flatten**: Image data needs reshaping
5. **Data Leakage**: Never use test data in training

## Performance Considerations

### Model Architecture

| Problem | Hidden Layers | Activation | Output Activation |
|---------|---------------|-------------|-------------------|
| Binary | 1-2 | ReLU | Sigmoid |
| Multi-class | 2-3 | ReLU | Softmax |
| Regression | 1-2 | ReLU | None/Linear |

### Hyperparameters

- **Epochs**: 10-100, depending on data and early stopping
- **Batch Size**: 32, 64, 128 typical
- **Learning Rate**: 0.001 default for Adam
- **Dropout**: 0.2-0.5 for regularization

## Related Concepts

- **TensorFlow**: Backend for Keras
- **CNTK**: Alternative backend
- **TensorFlow Lite**: For mobile deployment
- **TensorFlow Serving**: For production deployment
- **Custom Layers**: Extending Keras functionality

## Exercise Problems

1. **Basic**: Build a Keras model for MNIST classification.

2. **Intermediate**: Add more layers and tune hyperparameters.

3. **Advanced**: Implement a custom callback.

4. **Real-World Challenge**: Apply Keras to a real classification problem.

5. **Extension**: Save and load a trained Keras model.