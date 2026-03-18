# ⚙️ Autograd and Training Loop

## 🎯 What You'll Learn

- Automatic differentiation with autograd
- The complete training loop
- Optimizers and loss functions

## 📦 Prerequisites

- Read [01_tensors.md](./01_tensors.md) first

## What is Autograd?

**Automatic Differentiation** — PyTorch computes gradients automatically!

```python
import torch

# Create tensor with requires_grad=True to track operations
x: torch.Tensor = torch.tensor([1.0, 2.0, 3.0], requires_grad=True)

# Do some operations
y: torch.Tensor = x * 2
z: torch.Tensor = y.mean()

# Compute gradients
z.backward()

# Now x.grad contains the gradients!
print(x.grad)  # tensor([0.6667, 0.6667, 0.6667])
```

## Training Loop

```python
import torch
import torch.nn as nn
import torch.optim as optim

# 1. Simple data
X: torch.Tensor = torch.randn(100, 5)  # 100 samples, 5 features
y: torch.Tensor = torch.randn(100, 1)   # 100 targets

# 2. Simple model
model: nn.Module = nn.Linear(5, 1)  # 5 inputs → 1 output

# 3. Loss and optimizer
criterion: nn.MSELoss = nn.MSELoss()
optimizer: optim.Adam = optim.Adam(model.parameters(), lr=0.01)

# 4. Training loop
for epoch in range(100):
    # Forward pass: predictions
    predictions: torch.Tensor = model(X)
    
    # Calculate loss
    loss: torch.Tensor = criterion(predictions, y)
    
    # Backward pass: compute gradients
    optimizer.zero_grad()  # Clear previous gradients!
    loss.backward()         # Compute new gradients
    
    # Update weights
    optimizer.step()
    
    # Print progress
    if epoch % 10 == 0:
        print(f"Epoch {epoch}: Loss = {loss.item():.4f}")

print(f"\nFinal weights: {model.weight}")
print(f"Final bias: {model.bias}")
```

## Key Points

- `optimizer.zero_grad()`: Clear gradients before each step!
- `loss.backward()`: Compute gradients via backpropagation
- `optimizer.step()`: Update weights using gradients

## ✅ Summary

- `requires_grad=True` enables gradient tracking
- `loss.backward()` computes gradients
- `optimizer.step()` updates weights
- Training loop: forward → loss → zero_grad → backward → step

## ➡️ Next Steps

Ready to build a neural network? Head to **[../02_PyTorch_Basics/03_building_a_neural_network.md](../02_PyTorch_Basics/03_building_a_neural_network.md)**!

## 🔗 Further Reading

- [Autograd Documentation](https://pytorch.org/docs/stable/autograd.html)
