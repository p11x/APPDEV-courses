# From Perceptron to MLP

## What You'll Learn

- Understanding the perceptron
- Activation functions
- Multi-layer perceptrons
- Universal approximation

## Prerequisites

- Read [08_shap_explainability.md](../../10_Machine_Learning/03_Model_Evaluation/08_shap_explainability.md) first

## The Perceptron

A perceptron is the simplest neural network unit.

```python
# perceptron.py

import numpy as np


class Perceptron:
    def __init__(self, n_inputs: int) -> None:
        self.weights = np.random.randn(n_inputs)
        self.bias = 0.0
    
    def forward(self, x: np.ndarray) -> float:
        """Compute perceptron output."""
        z = np.dot(x, self.weights) + self.bias
        return 1 if z > 0 else 0
    
    def predict(self, X: np.ndarray) -> np.ndarray:
        return np.array([self.forward(x) for x in X])


# Example: AND gate
X = np.array([[0, 0], [0, 1], [1, 0], [1, 1]])
y = np.array([0, 0, 0, 1])

perceptron = Perceptron(2)
print(perceptron.predict(X))
```

## Multi-Layer Perceptron

```python
# mlp.py

import numpy as np


def relu(x: np.ndarray) -> np.ndarray:
    return np.maximum(0, x)


def sigmoid(x: np.ndarray) -> np.ndarray:
    return 1 / (1 + np.exp(-x))


class MLP:
    def __init__(self, layer_sizes: list) -> None:
        self.weights = []
        self.biases = []
        
        for i in range(len(layer_sizes) - 1):
            w = np.random.randn(layer_sizes[i], layer_sizes[i + 1])
            b = np.random.randn(layer_sizes[i + 1])
            self.weights.append(w)
            self.biases.append(b)
    
    def forward(self, X: np.ndarray) -> np.ndarray:
        activation = X
        for w, b in zip(self.weights, self.biases):
            z = np.dot(activation, w) + b
            activation = relu(z)
        return activation


mlp = MLP([2, 4, 4, 1])
print(f"MLP created with {len(mlp.weights)} layers")
```

## Summary

- Understanding the perceptron
- Activation functions
- Multi-layer perceptrons

## Next Steps

Continue to **[02_activation_functions.md](./02_activation_functions.md)**
