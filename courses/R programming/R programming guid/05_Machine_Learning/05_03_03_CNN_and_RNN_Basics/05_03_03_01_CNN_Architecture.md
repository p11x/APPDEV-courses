# Convolutional Neural Network (CNN) Architecture

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the architecture and principles of CNNs
- Implement CNN layers in Keras
- Build a CNN for image classification
- Understand convolutions, pooling, and feature maps
- Apply transfer learning with pre-trained models
- Interpret CNN feature visualizations

## Theoretical Background

Convolutional Neural Networks (CNNs) are specialized neural networks designed for processing grid-like data, most commonly images. They use convolutional layers that automatically learn spatial hierarchies of features, making them highly effective for image recognition, object detection, and image segmentation tasks.

### Why CNNs for Images?

Traditional neural networks treat each pixel as a separate feature, losing spatial information. CNNs preserve spatial relationships by using local connectivity and parameter sharing:

1. **Local Receptive Fields**: Each neuron connects to a small region of the input
2. **Parameter Sharing**: Same weights used across the entire image
3. **Translation Invariance**: Pattern can be detected anywhere in the image

### CNN Architecture Components

#### Convolutional Layer

The core building block. Applies convolution operation:

$$(f * g)(t) = \sum_{\tau} f(\tau) g(t - \tau)$$

In images:
- **Input**: Image or feature map
- **Kernel/Filter**: Small weight matrix (e.g., 3×3)
- **Stride**: How far the filter moves
- **Padding**: Adding borders around input

The convolution extracts features like edges, textures, patterns.

#### Pooling Layer

Reduces spatial dimensions through downsampling:

- **Max Pooling**: Takes maximum value in each region
- **Average Pooling**: Takes average value
- Reduces computation and controls overfitting

#### Fully Connected Layer

After convolutional/pooling layers, uses classic neural network for classification.

### Classic CNN Architectures

#### LeNet (1998)
- 7 layers
- Conv + Avg Pool + Conv + Avg Pool + FC + FC

#### AlexNet (2012)
- 8 layers
- ReLU activation
- Dropout
- 11×11, 5×5, 3×3 convolutions

#### VGG (2014)
- 16-19 layers
- 3×3 convolutions only
- Simple, uniform architecture

#### ResNet (2015)
- Residual connections
- Enables very deep networks (100+ layers)
- Solves vanishing gradient problem

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("keras")

library(keras)
```

### Step 2: Prepare Image Data

```r
# Load MNIST data
data <- dataset_mnist()

train_images <- data$train$x
train_labels <- data$train$y
test_images <- data$test$x
test_labels <- data$test$y

# Reshape for CNN (samples, rows, cols, channels)
train_images <- array_reshape(train_images, c(60000, 28, 28, 1))
test_images <- array_reshape(test_images, c(10000, 28, 28, 1))

# Normalize
train_images <- train_images / 255
test_images <- test_images / 255

# One-hot encode labels
num_classes <- 10
train_labels <- to_categorical(train_labels, num_classes)
test_labels <- to_categorical(test_labels, num_classes)

# Check dimensions
dim(train_images)  # c(60000, 28, 28, 1)
```

### Step 3: Build CNN Model

```r
# Build CNN model
model <- keras_model_sequential() %>%
  # First convolutional layer
  layer_conv_2d(
    filters = 32,
    kernel_size = c(3, 3),
    activation = "relu",
    input_shape = c(28, 28, 1)
  ) %>%
  # Max pooling
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  
  # Second convolutional layer
  layer_conv_2d(
    filters = 64,
    kernel_size = c(3, 3),
    activation = "relu"
  ) %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  
  # Third convolutional layer
  layer_conv_2d(
    filters = 64,
    kernel_size = c(3, 3),
    activation = "relu"
  ) %>%
  
  # Flatten for dense layers
  layer_flatten() %>%
  
  # Dense layers
  layer_dense(units = 64, activation = "relu") %>%
  layer_dropout(0.5) %>%
  layer_dense(units = 10, activation = "softmax")

# View architecture
summary(model)
```

### Step 4: Compile and Train

```r
# Compile
model %>% compile(
  optimizer = "adam",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)

# Train
history <- model %>% fit(
  train_images,
  train_labels,
  epochs = 10,
  batch_size = 64,
  validation_split = 0.2,
  verbose = 2
)

# Plot training history
plot(history)
```

### Step 5: Evaluate Model

```r
# Evaluate on test set
model %>% evaluate(test_images, test_labels, verbose = 0)

# Predictions
predictions <- model %>% predict(test_images)
predicted_classes <- apply(predictions, 1, which.max) - 1
actual_classes <- apply(test_labels, 1, which.max) - 1

# Confusion matrix
table(predicted_classes, actual_classes)
```

## Code Examples

### Example 1: Image Classification with CIFAR-10

This example shows CIFAR-10 image classification.

```r
# Create synthetic CIFAR-like data
set.seed(123)
n <- 5000
img_size <- 32

# Create random images (10 classes)
cifar_data <- list()

for(i in 1:10) {
  images <- array(rnorm(n/10 * img_size * img_size * 3), 
                 c(n/10, img_size, img_size, 3))
  # Add some pattern for each class
  for(j in 1:(n/10)) {
    if(i %% 2 == 1) {
      images[j, , , 1] <- images[j, , , 1] + 1
    }
  }
  cifar_data[[i]] <- images
}

x_train <- do.call(rbind, cifar_data)
y_train <- rep(0:9, each = n/10)

# Add dummy labels
train_labels <- to_categorical(y_train, 10)

# Normalize
x_train <- x_train / 255

# Build model
model_cifar <- keras_model_sequential() %>%
  layer_conv_2d(filters = 32, kernel_size = c(3, 3), activation = "relu",
                input_shape = c(img_size, img_size, 3)) %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 64, kernel_size = c(3, 3), activation = "relu") %>%
  layer_max_pooling_2d(pool_size = c(2, 2)) %>%
  layer_conv_2d(filters = 64, kernel_size = c(3, 3), activation = "relu") %>%
  layer_flatten() %>%
  layer_dense(units = 64, activation = "relu") %>%
  layer_dense(units = 10, activation = "softmax")

# Compile and train
model_cifar %>% compile(
  optimizer = "adam",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)

model_cifar %>% fit(x_train[1:4000, , , ], train_labels[1:4000, , ],
                   epochs = 5, batch_size = 32, validation_split = 0.2)
```

### Example 2: Transfer Learning with Pre-trained Models

This example demonstrates using VGG16 for transfer learning.

```r
# Load VGG16 without top layers
base_model <- application_vgg16(
  weights = "imagenet",
  include_top = FALSE,
  input_shape = c(224, 224, 3)
)

# Freeze base model layers
freeze_weights(base_model)

# Create new model on top
model_transfer <- keras_model_sequential() %>%
  base_model %>%
  layer_flatten() %>%
  layer_dense(units = 256, activation = "relu") %>%
  layer_dropout(0.5) %>%
  layer_dense(units = 10, activation = "softmax")

# Compile
model_transfer %>% compile(
  optimizer = "adam",
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)

# Summary shows frozen layers
summary(model_transfer)
```

### Example 3: Feature Visualization

This example shows visualizing learned features.

```r
# Get first conv layer filters
layer_output <- get_layer(model, 1)$output
filter_model <- keras_model(inputs = model$input, outputs = layer_output)

# Visualize some filters
# Note: Need actual images to visualize patterns
# This is placeholder for feature visualization code
cat("Feature visualization would use actual images\n")
cat("to find patterns that maximize filter activations\n")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Start with Simple Models**: Add complexity only if needed
2. **Use Appropriate Padding**: 'same' or 'valid' as needed
3. **Balance Channels**: Don't grow too fast
4. **Use Batch Normalization**: Stabilizes training
5. **Apply Dropout**: Prevents overfitting

### Common Pitfalls

1. **Too Many Filters**: Leads to overfitting
2. **Large Kernel Sizes**: Use 3×3 when possible
3. **Forgetting Pooling**: Without pooling, dimensions grow too fast
4. **Training from Scratch**: Use transfer learning when data is limited

## Performance Considerations

### Layer Patterns

| Layer | Typical Use | Parameters |
|-------|-------------|-------------|
| Conv 2D | First layers | 3×3 or 5×5 kernel |
| Max Pool | After conv | 2×2 pool |
| Batch Norm | After conv | None |
| Dropout | Before FC | None |
| Dense | Final layers | Many |

### Model Size Guidelines

- **Small Dataset**: Fewer filters, fewer layers
- **Large Dataset**: Can use deeper models
- **Limited Compute**: Smaller batch sizes, fewer epochs

## Related Concepts

- **Object Detection**: YOLO, R-CNN
- **Semantic Segmentation**: U-Net
- **Transfer Learning**: Using pre-trained weights
- **Data Augmentation**: Increasing training data
- **RNN**: For sequential data

## Exercise Problems

1. **Basic**: Build a CNN for MNIST classification.

2. **Intermediate**: Experiment with different filter sizes.

3. **Advanced**: Implement a custom layer.

4. **Real-World Challenge**: Apply CNN to real image classification.

5. **Extension**: Implement transfer learning.