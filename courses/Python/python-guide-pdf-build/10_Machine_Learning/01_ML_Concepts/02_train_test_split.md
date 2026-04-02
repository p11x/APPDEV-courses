# 📊 Train/Test Split

## 🎯 What You'll Learn

- Why you never test on training data
- The overfitting analogy (memorizing vs understanding)
- How to split data properly with train_test_split()
- Stratified splits for imbalanced data
- The difference between validation and test sets

## 📦 Prerequisites

- Read [01_what_is_ml.md](./01_what_is_ml.md) first

## The Critical Rule

**Never evaluate your model on data it has seen during training!**

This is like:
- Taking a math test with the exact same problems you memorized
- Taking a driving test in the exact same car you practiced in

You might pass, but you can't actually drive!

## The Overfitting Analogy

### ❌ Memorizing (Overfitting)

Student A studies the **exact answers** from practice tests. When the real test has different questions, they fail!

```
Training accuracy: 100% ← Perfect memorization!
Test accuracy: 45%     ← Failed the real test!
```

### ✅ Understanding (Generalizing)

Student B **learns the concepts** behind the problems. They can apply knowledge to new questions!

```
Training accuracy: 85% ← Good learning!
Test accuracy: 82%     ← Great generalization!
```

### Visual: Overfitting vs Good Fit

```
         Overfitting                    Good Fit                    Underfitting
         ──────────                     ────────                    ──────────────
     Accuracy                                    Accuracy                   Accuracy
        │                                          │                         │
   100% ┤ ╭──╮                                  ┌─╮                       ┌─╮
        │ ╯  ╰─╮                                │ │                       │ │
        │     ╰─╮                              ╭─╯ ╮                     ╭─╯ │
    0%  ├──────────→                      ╭────╯   ╰→               ╭────╯
        │          Complexity             │        Complexity         │   Complexity

    - Too complex model                - Balanced                    - Too simple model
    - Memorizes training data         - Learns patterns             - Can't learn patterns
```

## The Train/Test Split

### Basic Split

```python
from sklearn.model_selection import train_test_split
import numpy as np

# Our data
X: np.ndarray = np.arange(100).reshape(100, 1)  # Features (100 samples, 1 feature)
y: np.ndarray = np.arange(100)  # Labels (100 values)

# Split: 80% train, 20% test
X_train, X_test, y_train, y_test = train_test_split(
    X, y,
    test_size=0.2,        # 20% for testing
    random_state=42      # For reproducibility (same split every time)
)

print(f"Training samples: {len(X_train)}")  # 80
print(f"Test samples: {len(X_test)}")      # 20
```

### 💡 Line-by-Line Breakdown

- `X` - Features (input variables)
- `y` - Labels (what we want to predict)
- `test_size=0.2` - 20% of data goes to test set
- `random_state=42` - Fixed seed for reproducibility

### Why random_state Matters

```python
# Same random_state = same split every time!
X_train1, X_test1, y_train1, y_test1 = train_test_split(X, y, test_size=0.2, random_state=42)
X_train2, X_test2, y_train2, y_test2 = train_test_split(X, y, test_size=0.2, random_state=42)

# These are IDENTICAL:
print(np.array_equal(X_test1, X_test2))  # True!
```

## The Three-Way Split

For better evaluation, use **three** sets:

```
┌─────────────────────────────────────────────────┐
│              ALL DATA (100%)                    │
├────────────────────────┬────────────────────────┤
│     TRAINING (70%)     │      TEST (30%)        │
│                         │                        │
│    ┌─────────────────┐ │                        │
│    │   VALIDATION    │ │   HOLDOUT TEST         │
│    │     (15%)       │ │      (15%)             │
│    └─────────────────┘ │                        │
│                        │                        │
│    Used for:           │   Used for:            │
│    - Training          │   - FINAL evaluation   │
│    - Hyperparameter   │                        │
│      tuning            │                        │
└────────────────────────┴────────────────────────┘
```

```python
# First split: separate test
X_trainval, X_test, y_trainval, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

# Second split: separate validation
X_train, X_val, y_train, y_val = train_test_split(
    X_trainval, y_trainval, test_size=0.25, random_state=42
)

# Result: 60% train, 20% validation, 20% test
```

## Stratified Split

When your data is **imbalanced** (unequal classes), use stratified sampling:

```python
from sklearn.model_selection import train_test_split
import numpy as np

# Imbalanced data: 90% class A, 10% class B
X: np.ndarray = np.arange(1000).reshape(1000, 1)
y: np.ndarray = np.array(["A"] * 900 + ["B"] * 100)

# Regular split might put all B in training!
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

# Check class balance in test
print(f"B in test (regular): {sum(y_test == 'B')}")  # Might be 15, 25, etc.

# Stratified split keeps the same ratio!
X_train, X_test, y_train, y_test = train_test_split(
    X, y,
    test_size=0.2,
    random_state=42,
    stratify=y  # IMPORTANT: Keep same class ratio
)

print(f"B in test (stratified): {sum(y_test == 'B')}")  # Exactly 20!
```

### 💡 Explanation

- `stratify=y` - Ensure same percentage of each class in train and test
- Critical for medical diagnosis, fraud detection, etc.

## Visual: Dataset Split

```
Original Dataset (100 rows):
┌──┬──┬──┬──┬──┬──┬──┬──┬──┬──┐
│ 1│ 2│ 3│ 4│ 5│ 6│ 7│ 8│ 9│10│  ... → 100
└──┴──┴──┴──┴──┴──┴──┴──┴──┴──┘

After train_test_split(test_size=0.2):

Training (80 rows):              Test (20 rows):
┌──┬──┬──┬──┬──┬──┐           ┌──┬──┬──┐
│ 1│ 2│ 5│ 7│ 9│10│  ...       │ 3│ 4│ 6│  ...
└──┴──┴──┴──┴──┴──┘           └──┴──┴──┘

Model trains on TRAINING data
Model is evaluated on TEST data
```

## Cross-Validation (Quick Intro)

For robust evaluation, use **K-fold cross-validation**:

```python
from sklearn.model_selection import cross_val_score
from sklearn.linear_model import LogisticRegression
import numpy as np

# Sample data
X: np.ndarray = np.random.randn(100, 5)
y: np.ndarray = np.random.choice([0, 1], 100)

# Model
model: LogisticRegression = LogisticRegression()

# 5-fold cross-validation
scores: np.ndarray = cross_val_score(model, X, y, cv=5)

print(f"Fold scores: {scores}")
print(f"Mean: {scores.mean():.3f} (+/- {scores.std():.3f})")
```

### 💡 Explanation

- Split data into 5 "folds"
- Train on 4 folds, test on 1 — 5 times
- Average the scores for robust estimate

## ✅ Summary

- **Never test on training data** — leads to overfitting!
- **train_test_split()** — basic 80/20 split
- **random_state** — reproducibility
- **stratify=y** — maintain class balance for imbalanced data
- **Three-way split** — train/validation/test for tuning
- **Cross-validation** — more robust evaluation

## ➡️ Next Steps

Ready to prepare features for ML? Head to **[03_features_and_labels.md](./03_features_and_labels.md)**!

## 🔗 Further Reading

- [train_test_split Documentation](https://scikit-learn.org/stable/modules/generated/sklearn.model_selection.train_test_split.html)
- [Cross-Validation](https://scikit-learn.org/stable/modules/cross_validation.html)
- [Overfitting Explained](https://www.ibm.com/topics/overfitting)
