# ⚡ Activation and Loss Functions

## What You'll Learn

- Implement ReLU, Sigmoid, Softmax manually
- Plot activation functions
- Binary and categorical cross-entropy

## ReLU Implementation

```python
import numpy as np
import matplotlib.pyplot as plt

def relu(x):
    return np.maximum(0, x)

x = np.linspace(-5, 5, 100)
plt.plot(x, relu(x))
plt.title("ReLU")
plt.show()
```

## Sigmoid Implementation

```python
def sigmoid(x):
    return 1 / (1 + np.exp(-x))

plt.plot(x, sigmoid(x))
plt.title("Sigmoid")
plt.show()
```

## Softmax Implementation

```python
def softmax(x):
    exp_x = np.exp(x - np.max(x))
    return exp_x / exp_x.sum()

scores = [2.0, 1.0, 0.1]
print(softmax(scores))
```

## Cross-Entropy Loss

```python
def binary_cross_entropy(y_true, y_pred):
    epsilon = 1e-15
    y_pred = np.clip(y_pred, epsilon, 1 - epsilon)
    return -np.mean(y_true * np.log(y_pred) + (1 - y_true) * np.log(1 - y_pred))
```
