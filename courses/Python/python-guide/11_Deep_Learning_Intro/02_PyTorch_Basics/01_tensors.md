# 🔥 PyTorch Tensors

## 🎯 What You'll Learn

- What tensors are (NumPy arrays with superpowers!)
- Create tensors
- Move to GPU
- Tensor operations

## 📦 Prerequisites

- Read [01_what_are_neural_networks.md](./01_what_are_neural_networks.md) first

## What is PyTorch?

**PyTorch** is Facebook's deep learning framework. It's:
- Pythonic and intuitive
- Fast (runs on GPU!)
- Used by Facebook, Tesla, NVIDIA, and more!

### Installing

```python
pip install torch torchvision
```

## Tensors: NumPy with Superpowers

A **tensor** is just a multi-dimensional array, but:

```
NumPy Array: CPU only
PyTorch Tensor: CPU + GPU (with automatic differentiation!)
```

## Creating Tensors

```python
import torch

# From Python list
tensor1: torch.Tensor = torch.tensor([1, 2, 3])
print(tensor1)  # tensor([1, 2, 3])

# Zeros, ones, random
zeros: torch.Tensor = torch.zeros(3, 4)      # 3×4 matrix of zeros
ones: torch.Tensor = torch.ones(2, 3)       # 2×3 matrix of ones
rand: torch.Tensor = torch.randn(3, 3)      # random normal

# Range
arange: torch.Tensor = torch.arange(0, 10, 2)  # [0, 2, 4, 6, 8]
```

### Attributes

```python
t: torch.Tensor = torch.randn(3, 4)

print(t.shape)   # torch.Size([3, 4])
print(t.dtype)   # torch.float32
print(t.device)  # cpu (or cuda if GPU available!)
```

## GPU: The Game Changer

Move tensors to GPU for **massive speedup**:

```python
import torch

# Check GPU availability
print(torch.cuda.is_available())  # True if you have NVIDIA GPU!

# Move to GPU (if available)
device: str = "cuda" if torch.cuda.is_available() else "cpu"
t: torch.Tensor = torch.randn(1000, 1000).to(device)

# Operations on GPU are much faster!
```

## Tensor Operations

Just like NumPy!

```python
import torch

a: torch.Tensor = torch.tensor([1, 2, 3])
b: torch.Tensor = torch.tensor([4, 5, 6])

# Basic math
print(a + b)     # tensor([5, 7, 9])
print(a * b)     # tensor([4, 10, 18])
print(a.sum())   # tensor(6)

# Matrix multiplication
m1: torch.Tensor = torch.randn(2, 3)
m2: torch.Tensor = torch.randn(3, 2)
result: torch.Tensor = torch.mm(m1, m2)  # Or m1 @ m2
```

## Convert NumPy ↔ PyTorch

```python
import numpy as np
import torch

# NumPy → PyTorch
np_array: np.ndarray = np.array([1, 2, 3])
torch_tensor: torch.Tensor = torch.from_numpy(np_array)

# PyTorch → NumPy
torch_tensor: torch.Tensor = torch.tensor([1, 2, 3])
np_array: np.ndarray = torch_tensor.numpy()
```

## In-Place Operations

The underscore convention:

```python
t: torch.Tensor = torch.tensor([1, 2, 3])

t.add_(5)    # In-place: t is now [6, 7, 8]
t.mul_(2)    # In-place: t is now [12, 14, 16]

# vs. non-in-place:
t2: torch.Tensor = t.add(5)  # Returns new tensor, t unchanged
```

## ✅ Summary

- **Tensors**: Multi-dimensional arrays that can run on GPU
- `torch.tensor()`: Create from Python list
- `.to("cuda")`: Move to GPU
- NumPy-like operations
- `.numpy()`: Convert back to NumPy
- `_` suffix: In-place operations

## ➡️ Next Steps

Ready for automatic differentiation? Head to **[../02_PyTorch_Basics/02_autograd_and_training_loop.md](../02_PyTorch_Basics/02_autograd_and_training_loop.md)**!

## 🔗 Further Reading

- [PyTorch Documentation](https://pytorch.org/docs/)
- [PyTorch tensors tutorial](https://pytorch.org/tutorials/beginner/deep_learning_60min_blitz.html)
