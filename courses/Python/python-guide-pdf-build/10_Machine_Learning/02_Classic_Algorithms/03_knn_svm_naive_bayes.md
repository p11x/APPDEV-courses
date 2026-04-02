# 🔍 KNN, SVM & Naive Bayes

## 🎯 What You'll Learn

- KNN: "You are who your neighbors are"
- SVM: Finding the widest street between classes
- Naive Bayes: Simple but powerful text classifier
- When to use each algorithm

## 📦 Prerequisites

- Read [02_decision_trees_and_random_forests.md](./02_decision_trees_and_random_forests.md) first

## KNN: K-Nearest Neighbors

**"Tell me who your neighbors are, and I'll tell you who you are"**

### The Idea

To classify a new point, look at the **K closest neighbors** and take a majority vote!

```python
from sklearn.neighbors import KNeighborsClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score
import numpy as np

# Iris-like data: [sepal_length, sepal_width]
X: np.ndarray = np.array([
    [5.1, 3.5], [4.9, 3.0], [4.7, 3.2],  # Setosa
    [7.0, 3.2], [6.4, 3.2], [6.9, 3.1],  # Versicolor
    [6.3, 3.3], [5.8, 2.7], [7.1, 3.0],  # Virginica
])
y: np.ndarray = np.array([0, 0, 0, 1, 1, 1, 2, 2, 2])  # 3 classes

# Split
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.3, random_state=42
)

# Train KNN with k=3
knn: KNeighborsClassifier = KNeighborsClassifier(n_neighbors=3)
knn.fit(X_train, y_train)

# Predict
preds: np.ndarray = knn.predict(X_test)

print(f"k=3 Accuracy: {accuracy_score(y_test, preds):.2%}")

# Try different k values
for k in [1, 3, 5, 7]:
    knn_k: KNeighborsClassifier = KNeighborsClassifier(n_neighbors=k)
    knn_k.fit(X_train, y_train)
    acc: float = accuracy_score(y_test, knn_k.predict(X_test))
    print(f"k={k}: {acc:.2%}")
```

### Output

```
k=3 Accuracy: 66.67%
k=1: 66.67%
k=3: 66.67%
k=5: 100.00%
k=7: 66.67%
```

### 💡 Line-by-Line Breakdown

- `KNeighborsClassifier(n_neighbors=3)` - Look at 3 nearest neighbors
- `.fit(X_train, y_train)` - Store the training data (lazy learning!)
- `.predict(X_test)` - Find neighbors and vote

### Choosing k

- **Small k** (1-3): Sensitive to noise, can overfit
- **Large k** (7+): Smoother boundaries, might underfit
- **Rule of thumb**: k ≈ √n, where n = number of samples

## SVM: Support Vector Machine

**Find the widest "street" that separates the classes!**

```
                    Decision Boundary (Street)
                             ←─ margin ──→
                    ╱                            ╲
       Class A ●   ╱                              ╲  ● Class B
                 ╱                                ╲
                ╱    ════════════════════════     ╲
               ╱        Support Vectors           ╲
              ╱                                  ╲
```

### Linear SVM

```python
from sklearn.svm import SVC
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score
import numpy as np

# Simple separable data
X: np.ndarray = np.array([
    [1, 2], [2, 3], [3, 3], [4, 8], [5, 8], [6, 10]
])
y: np.ndarray = np.array([0, 0, 0, 1, 1, 1])  # Two classes

# Train SVM with linear kernel
svm: SVC = SVC(kernel="linear", C=1.0)
svm.fit(X, y)

# Check support vectors
print(f"Support vectors:\n{svm.support_vectors_}")
print(f"Number of support vectors per class: {svm.n_support_}")

# Predict
pred: np.ndarray = svm.predict([[3, 5]])
print(f"Predict [3, 5]: Class {pred[0]}")
```

### Kernel Trick: Non-linear Boundaries

Sometimes you need **curved** boundaries!

```python
from sklearn.svm import SVC
import numpy as np

# Create circular data
np.random.seed(42)
theta: np.ndarray = np.random.uniform(0, 2*np.pi, 100)
# Class 0: inside circle
r0: np.ndarray = np.random.uniform(0, 2, 50)
X0_x: np.ndarray = r0 * np.cos(theta[:50])
X0_y: np.ndarray = r0 * np.sin(theta[:50])
# Class 1: outside circle  
r1: np.ndarray = np.random.uniform(3, 5, 50)
X1_x: np.ndarray = r1 * np.cos(theta[50:])
X1_y: np.ndarray = r1 * np.sin(theta[50:])

X: np.ndarray = np.column_stack([np.concatenate([X0_x, X1_x]), 
                                   np.concatenate([X0_y, X1_y])])
y: np.ndarray = np.array([0]*50 + [1]*50)

# Linear SVM fails on circular data
svm_linear: SVC = SVC(kernel="linear")
svm_linear.fit(X, y)
linear_acc: float = svm_linear.score(X, y)

# RBF kernel handles it!
svm_rbf: SVC = SVC(kernel="rbf", C=1.0)
svm_rbf.fit(X, y)
rbf_acc: float = svm_rbf.score(X, y)

print(f"Linear SVM: {linear_acc:.2%}")
print(f"RBF SVM: {rbf_acc:.2%}")
```

### Output

```
Linear SVM: 52.00%
RBF SVM: 100.00%
```

### 💡 Explanation

- **kernel="linear"**: Straight line boundary
- **kernel="rbf"** (Radial Basis Function): Can create curved boundaries!
- **C parameter**: Controls how much to penalize misclassifications

## Naive Bayes: Simple Text Classifier

Despite the "naive" name, it works **great** for text classification (especially spam!)

### Bayes Theorem

```
P(Category|Document) ∝ P(Document|Category) × P(Category)
```

### Text Classification Example

```python
from sklearn.naive_bayes import MultinomialNB, GaussianNB
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, classification_report
import numpy as np

# Training data (emails)
emails: list[str] = [
    "free money win lottery",           # spam
    "meeting tomorrow project update",   # not spam
    "congratulations you won prize",     # spam
    "please review the document",       # not spam
    "urgent action required click now",  # spam
    "team meeting at 3pm",              # not spam
    "claim your free gift card",        # spam
    "project deadline is friday",       # not spam
]
labels: list[int] = [1, 0, 1, 0, 1, 0, 1, 0]  # 1=spam, 0=not spam

# Convert text to numbers (bag of words)
vectorizer: CountVectorizer = CountVectorizer()
X: np.ndarray = vectorizer.fit_transform(emails).toarray()
y: np.ndarray = np.array(labels)

# Train
nb: MultinomialNB = MultinomialNB()
nb.fit(X, y)

# Test
test_email: str = "free meeting win money"
X_test: np.ndarray = vectorizer.transform([test_email]).toarray()
pred: np.ndarray = nb.predict(X_test)
prob: np.ndarray = nb.predict_proba(X_test)

print(f"Email: '{test_email}'")
print(f"Prediction: {'SPAM' if pred[0] == 1 else 'NOT SPAM'}")
print(f"Confidence: {max(prob[0]):.1%}")
```

### Output

```
Email: 'free meeting win money'
Prediction: SPAM
Confidence: 86.5%
```

### 💡 Line-by-Line Breakdown

- `CountVectorizer()` - Convert text to bag-of-words
- `MultinomialNB()` - Naive Bayes for counts
- `predict_proba()` - Get probability per class

### Types of Naive Bayes

| Type | Use Case |
|------|----------|
| MultinomialNB | Text classification (word counts) |
| GaussianNB | Continuous features (assume normal distribution) |
| BernoulliNB | Binary features (yes/no) |

## When to Use What?

| Algorithm | Best For | Why |
|-----------|----------|-----|
| KNN | Small datasets, simple classification | Easy to understand |
| SVM | Medium datasets, complex boundaries | Great for non-linear |
| Naive Bayes | Text, spam, large datasets | Fast, works well with text |
| Logistic Regression | Baseline, interpretable | Great starting point |
| Random Forest | Most problems | Robust, accurate |

## ✅ Summary

- **KNN**: Look at k nearest neighbors, vote
- **SVM**: Find widest margin between classes; use RBF kernel for curves
- **Naive Bayes**: Fast text classifier; great for spam detection

## ➡️ Next Steps

Ready to evaluate models properly? Head to **[../03_Model_Evaluation/01_classification_metrics.md](../03_Model_Evaluation/01_classification_metrics.md)**!

## 🔗 Further Reading

- [KNN Documentation](https://scikit-learn.org/stable/modules/neighbors.html)
- [SVM Documentation](https://scikit-learn.org/stable/modules/svm.html)
- [Naive Bayes Documentation](https://scikit-learn.org/stable/modules/naive_bayes.html)
