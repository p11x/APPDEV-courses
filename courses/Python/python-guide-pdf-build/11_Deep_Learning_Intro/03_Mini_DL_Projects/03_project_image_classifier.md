# 🖼️ CNN Image Classifier

## 🛠️ Setup

```python
pip install torch torchvision matplotlib
```

## Full Code

```python
import torch
import torch.nn as nn
import torch.optim as optim
import numpy as np

# Simple CNN for CIFAR-10
class CNN(nn.Module):
    def __init__(self: "CNN") -> None:
        super().__init__()
        # Conv layers
        self.conv1: nn.Conv2d = nn.Conv2d(3, 32, 3, padding=1)
        self.conv2: nn.Conv2d = nn.Conv2d(32, 64, 3, padding=1)
        # Pooling
        self.pool: nn.MaxPool2d = nn.MaxPool2d(2, 2)
        # FC layers
        self.fc1: nn.Linear = nn.Linear(64 * 8 * 8, 256)
        self.fc2: nn.Linear = nn.Linear(256, 10)
        self.relu: nn.ReLU = nn.ReLU()
        self.dropout: nn.Dropout = nn.Dropout(0.5)
    
    def forward(self: "CNN", x: torch.Tensor) -> torch.Tensor:
        # Conv block 1
        x = self.relu(self.conv1(x))
        x = self.pool(x)
        # Conv block 2
        x = self.relu(self.conv2(x))
        x = self.pool(x)
        # Flatten
        x = x.view(-1, 64 * 8 * 8)
        # FC
        x = self.dropout(self.relu(self.fc1(x)))
        x = self.fc2(x)
        return x

# Create model
model: CNN = CNN()

# Count parameters
total_params: int = sum(p.numel() for p in model.parameters())
print(f"Model has {total_params:,} parameters")

# Demo with random data
X_demo: torch.Tensor = torch.randn(4, 3, 32, 32)
output: torch.Tensor = model(X_demo)
print(f"Output shape: {output.shape}")  # [4, 10]

print("\nCNN ready for CIFAR-10 training!")
print("CIFAR-10 classes: airplane, automobile, bird, cat, deer, dog, frog, horse, ship, truck")
```

## Explanation

- **Conv2d**: Extracts features from images
- **MaxPool2d**: Downsamples feature maps
- **Flatten**: 2D → 1D for fully connected layers

## Further Reading

- [CIFAR-10 Dataset](https://www.cs.toronto.edu/~kriz/cifar.html)
