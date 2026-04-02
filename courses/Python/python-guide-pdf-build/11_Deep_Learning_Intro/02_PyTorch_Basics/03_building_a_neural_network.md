# 🏗️ Building a Neural Network

## 🎯 What You'll Learn

- Build a neural network with nn.Module
- Define layers in __init__
- Implement forward pass

## 📦 Prerequisites

- Read [02_autograd_and_training_loop.md](./02_autograd_and_training_loop.md) first

## nn.Module: The Base Class

```python
import torch
import torch.nn as nn
import torch.optim as optim
import numpy as np

# Create sample data (Iris-like)
np.random.seed(42)
X: np.ndarray = np.random.randn(150, 4)
y: np.random.randint(0, 3, 150)  # 3 classes

# Convert to tensors
X: torch.Tensor = torch.FloatTensor(X)
y: torch.Tensor = torch.LongTensor(y)

# Define Neural Network
class SimpleNN(nn.Module):
    def __init__(self, input_size: int, hidden_size: int, num_classes: int):
        super().__init__()
        self.layer1: nn.Linear = nn.Linear(input_size, hidden_size)
        self.layer2: nn.Linear = nn.Linear(hidden_size, hidden_size)
        self.output: nn.Linear = nn.Linear(hidden_size, num_classes)
        self.relu: nn.ReLU = nn.ReLU()
    
    def forward(self, x: torch.Tensor) -> torch.Tensor:
        x = self.relu(self.layer1(x))
        x = self.relu(self.layer2(x))
        x = self.output(x)
        return x

# Create model
model: SimpleNN = SimpleNN(4, 16, 3)

# Loss and optimizer
criterion: nn.CrossEntropyLoss = nn.CrossEntropyLoss()
optimizer: optim.Adam = optim.Adam(model.parameters(), lr=0.01)

# Training
for epoch in range(100):
    optimizer.zero_grad()
    outputs: torch.Tensor = model(X)
    loss: torch.Tensor = criterion(outputs, y)
    loss.backward()
    optimizer.step()
    
    if epoch % 20 == 0:
        print(f"Epoch {epoch}: Loss = {loss.item():.4f}")

# Evaluate
with torch.no_grad():
    outputs: torch.Tensor = model(X)
    _, preds: torch.Tensor = torch.max(outputs, 1)
    accuracy: float = (preds == y).float().mean()
    print(f"\nAccuracy: {accuracy:.2%}")
```

## Summary

- nn.Module for custom networks
- __init__: define layers
- forward(): define forward pass

## Further Reading

- [PyTorch nn.Module](https://pytorch.org/docs/stable/generated/torch.nn.Module.html)
