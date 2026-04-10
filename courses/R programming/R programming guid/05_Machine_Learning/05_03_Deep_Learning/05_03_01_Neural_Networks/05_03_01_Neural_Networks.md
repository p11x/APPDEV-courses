# Neural Networks in R

## Learning Objectives

- Understand neural network fundamentals
- Create basic neural networks in R

## Code Examples

```r
# Neural Networks in R

cat("===== NEURAL NETWORKS =====\n\n")

library(nnet)

# Sample data
set.seed(42)
data <- data.frame(
  x1 = runif(100),
  x2 = runif(100),
  y = ifelse(runif(100) > 0.5, 1, 0)
)

# Train neural network
cat("Training neural network:\n")
nn <- nnet(y ~ x1 + x2, data = data, size = 5, 
           linout = FALSE, maxit = 100)
cat("Neural network trained\n")
cat("Weights:", summary(nn), "\n")
```

## Best Practices

1. Scale inputs for neural networks
2. Choose appropriate network size
3. Monitor overfitting
