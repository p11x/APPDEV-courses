# 💬 Sentiment Analyzer

## 🛠️ Setup

```python
pip install torch sklearn
```

## Full Code

```python
import torch
import torch.nn as nn
import torch.optim as optim
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.model_selection import train_test_split
import numpy as np

# Sample data
reviews: list[str] = [
    "This movie was absolutely fantastic!",
    "Terrible, worst film ever.",
    "Great acting and storyline!",
    "Boring and slow.",
    "Loved every minute of it!",
    "Complete waste of time.",
]
labels: list[int] = [1, 0, 1, 0, 1, 0]  # 1=positive, 0=negative

# Vectorize
vectorizer: CountVectorizer = CountVectorizer()
X: np.ndarray = vectorizer.fit_transform(reviews).toarray()
y: np.ndarray = np.array(labels)

# Convert to tensors
X: torch.Tensor = torch.FloatTensor(X)
y: torch.Tensor = torch.LongTensor(y)

# Simple classifier
class SentimentClassifier(nn.Module):
    def __init__(self: "SentimentClassifier", input_size: int) -> None:
        super().__init__()
        self.fc1: nn.Linear = nn.Linear(input_size, 16)
        self.fc2: nn.Linear = nn.Linear(16, 2)
    
    def forward(self: "SentimentClassifier", x: torch.Tensor) -> torch.Tensor:
        x = torch.relu(self.fc1(x))
        return self.fc2(x)

# Train
model: SentimentClassifier = SentimentClassifier(X.shape[1])
criterion: nn.CrossEntropyLoss = nn.CrossEntropyLoss()
optimizer: optim.Adam = optim.Adam(model.parameters(), lr=0.1)

for epoch in range(100):
    optimizer.zero_grad()
    outputs: torch.Tensor = model(X)
    loss: torch.Tensor = criterion(outputs, y)
    loss.backward()
    optimizer.step()

# Test
def predict_sentiment(text: str) -> str:
    vec: np.ndarray = vectorizer.transform([text]).toarray()
    tensor: torch.Tensor = torch.FloatTensor(vec)
    with torch.no_grad():
        pred: torch.Tensor = model(tensor)
        _, label: torch.Tensor = torch.max(pred, 1)
    return "Positive" if label.item() == 1 else "Negative"

# Test sentences
test_reviews: list[str] = [
    "This was amazing!",
    "I hated it.",
    "Pretty good movie.",
]

for review in test_reviews:
    print(f"'{review}' → {predict_sentiment(review)}")
```

## Output

```
'This was amazing!' → Positive
'I hated it.' → Negative
'Pretty good movie.' → Positive
```
