# 📊 Classification Metrics

## 🎯 What You'll Learn

- Accuracy is NOT enough!
- Precision, Recall, F1-Score
- Confusion Matrix explained
- ROC curves and AUC

## 📦 Prerequisites

- Read [03_knn_svm_naive_bayes.md](./03_knn_svm_naive_bayes.md) first

## Why Accuracy Isn't Enough

Imagine a cancer test:
- **99% accurate** — sounds great!
- But if 1% of people have cancer, a test that says "no cancer" for everyone gets 99% accuracy!

```
Actual: Cancer →  1 case
Predicted: No Cancer → Test says "You're fine!"

Accuracy: 99/100 = 99% ← MISLEADING!
```

## Confusion Matrix

The foundation of all classification metrics:

```
                    Predicted
                 ┌─────────────┬─────────────┐
                 │   Negative  │   Positive  │
      ┌──────────┼─────────────┼─────────────┤
Actual│  Negative│     TN      │     FP      │
      ├──────────┼─────────────┼─────────────┤
      │  Positive│     FN      │     TP      │
      └──────────┴─────────────┴─────────────┘

TN = True Negative  (correctly said "no")
FP = False Positive (wrongly said "yes" — false alarm!)
FN = False Negative (wrongly said "no" — missed it!)
TP = True Positive  (correctly said "yes")
```

### Emoji Analogy

```
You receive an email. Is it spam?

✅ TRUE POSITIVE:  It's spam, you marked it spam ✓
❌ FALSE POSITIVE: Not spam, you marked it spam ✗
✅ TRUE NEGATIVE: Not spam, you marked it not spam ✓
❌ FALSE NEGATIVE: It's spam, you marked it not spam ✗

For SPAM classification:
- FP = Good email sent to spam folder (bad!)
- FN = Spam in inbox (annoying but tolerable)
```

## Metrics

### Accuracy

```
Accuracy = (TP + TN) / Total
```

```python
from sklearn.metrics import accuracy_score

y_true: list[int] = [1, 1, 0, 1, 0, 1, 0, 0, 1, 0]
y_pred: list[int] = [1, 1, 0, 0, 0, 1, 0, 1, 1, 0]

accuracy: float = accuracy_score(y_true, y_pred)
print(f"Accuracy: {accuracy:.2%}")  # 70%
```

### Precision

**"Of all the positive predictions, how many were correct?"**

```
Precision = TP / (TP + FP)
```

```python
from sklearn.metrics import precision_score

# Higher precision = fewer false alarms!
precision: float = precision_score(y_true, y_pred)
print(f"Precision: {precision:.2%}")
```

### Recall (Sensitivity)

**"Of all the actual positives, how many did we find?"**

```
Recall = TP / (TP + FN)
```

```python
from sklearn.metrics import recall_score

# Higher recall = fewer missed positives!
recall: float = recall_score(y_true, y_pred)
print(f"Recall: {recall:.2%}")
```

### F1-Score

**"Balance between precision and recall"**

```
F1 = 2 × (Precision × Recall) / (Precision + Recall)
```

```python
from sklearn.metrics import f1_score

# Harmonic mean of precision and recall
f1: float = f1_score(y_true, y_pred)
print(f"F1-Score: {f1:.2%}")
```

## Classification Report

One command does everything:

```python
from sklearn.metrics import classification_report
import numpy as np

y_true: np.ndarray = np.array([1, 1, 0, 1, 0, 1, 0, 0, 1, 0])
y_pred: np.ndarray = np.array([1, 1, 0, 0, 0, 1, 0, 1, 1, 0])

print(classification_report(y_true, y_pred, target_names=["Not Spam", "Spam"]))
```

### Output

```
              precision    recall  f1-score   support

    Not Spam       0.67      0.80      0.73         5
        Spam       0.75      0.60      0.67         5

    accuracy                           0.70        10
   macro avg       0.71      0.70      0.70        10
weighted avg       0.71      0.70      0.70        10
```

## When to Use Which?

| Scenario | Prioritize | Why |
|----------|-----------|-----|
| Spam detection | Precision | Don't want good emails in spam! |
| Medical diagnosis | Recall | Don't want to miss cancer cases! |
| Balanced problems | F1-Score | Need balance |

## ROC Curve & AUC

**ROC** = Receiver Operating Characteristic
**AUC** = Area Under the Curve

```python
from sklearn.metrics import roc_curve, auc, roc_auc_score
import numpy as np

# True labels
y_true: np.ndarray = np.array([0, 0, 0, 1, 1, 1, 1, 0, 1, 0])

# Predicted probabilities (not just 0/1!)
y_proba: np.ndarray = np.array([0.1, 0.3, 0.2, 0.9, 0.8, 0.7, 0.95, 0.4, 0.85, 0.3])

# Calculate ROC curve
fpr: np.ndarray  # False positive rate
tpr: np.ndarray  # True positive rate
thresholds: np.ndarray
fpr, tpr, thresholds = roc_curve(y_true, y_proba)

# AUC score
auc_score: float = roc_auc_score(y_true, y_proba)

print(f"AUC: {auc_score:.2%}")

# AUC Interpretation:
# 0.5 = random (no skill)
# 0.7-0.8 = reasonable
# 0.8-0.9 = good
# > 0.9 = excellent
```

### Visual: ROC Curve

```
True Positive Rate
(sensitivity)
     │
 1.0 ┤    ╭──────────────  ← Perfect classifier (AUC = 1.0)
     │   ╱
 0.8 ┤  ╱   ────────────
     │ ╱
 0.5 ┼────────────────────  ← Random classifier (AUC = 0.5)
     │╱
 0.0 ┼─╱
     └──────────────────────
      0.0   0.5   1.0
          False Positive Rate
          (1 - specificity)
```

## ✅ Summary

- **Accuracy** = correct / total (misleading with imbalanced data!)
- **Precision** = TP / (TP + FP) — "Of predicted positives, how many right?"
- **Recall** = TP / (TP + FN) — "Of actual positives, how many found?"
- **F1** = 2 × P × R / (P + R) — balance between precision and recall
- **Confusion matrix** = TP, TN, FP, FN — the foundation
- **ROC/AUC** = measure across all thresholds

## ➡️ Next Steps

Ready for regression metrics? Head to **[02_regression_metrics.md](./02_regression_metrics.md)**!

## 🔗 Further Reading

- [sklearn.metrics](https://scikit-learn.org/stable/modules/classes.html#sklearn-metrics)
- [Confusion Matrix Visualization](https://scikit-learn.org/stable/auto_examples/model_selection/plot_confusion_matrix.html)
- [ROC Curves](https://scikit-learn.org/stable/auto_examples/model_selection/plot_roc.html)
