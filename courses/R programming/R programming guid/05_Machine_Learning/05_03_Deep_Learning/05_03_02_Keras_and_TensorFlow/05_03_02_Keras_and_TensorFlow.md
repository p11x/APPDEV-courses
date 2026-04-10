# Keras and TensorFlow in R

## Learning Objectives

- Use Keras in R
- Build deep learning models

## Code Examples

```r
# Keras in R

cat("===== KERAS IN R =====\n\n")

library(keras)

# Check if Keras is installed
cat("Keras version:", as.character(packageVersion("keras")), "\n")

# Simple model example (commented out - requires keras installed)
cat("\nNote: Requires keras and tensorflow packages\n")
cat("Install with: install.packages(\"keras\")\n")
cat("Then run: keras::install_keras()\n")

# Example model structure (pseudo-code)
cat("\nExample model structure:\n")
cat("model <- keras_model_sequential() %>%\n")
cat("  layer_dense(units = 128, activation = 'relu', input_shape = c(784)) %>%\n")
cat("  layer_dense(units = 10, activation = 'softmax')\n")
```

## Best Practices

1. Install both keras and tensorflow
2. Use GPU if available
3. Start with simple architectures
