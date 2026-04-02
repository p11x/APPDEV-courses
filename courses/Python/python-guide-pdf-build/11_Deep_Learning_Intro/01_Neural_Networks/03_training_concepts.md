# 🔄 Training Concepts

## What You'll Learn

- Epochs, batch size, learning rate
- Mini-batch gradient descent
- Dropout and early stopping

## Key Training Concepts

### Epochs
One full pass through the training data.

### Batch Size
Process data in chunks, not all at once.

### Learning Rate
Too high = overshoot, too low = too slow!

### Dropout
Randomly disable neurons during training to prevent overfitting.

### Early Stopping
Stop training when validation loss stops improving.

## Training Loop Visual

```
Forward Pass → Calculate Loss → Backward Pass → Update Weights
     ↑              ↓              ↓              ↓
     └──────────────┴──────────────┴──────────────┘
                      Repeat!
```

## Bias-Variance Tradeoff

```
High Variance (Overfitting):
- Training error: low
- Validation error: high

High Bias (Underfitting):
- Training error: high
- Validation error: high

Ideal:
- Training error: low
- Validation error: low
```
