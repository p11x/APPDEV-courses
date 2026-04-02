# 🤖 What is Machine Learning?

## 🎯 What You'll Learn

- Understand the three main types of ML
- What "training" and "predicting" actually mean
- The ML workflow from raw data to predictions
- What scikit-learn is and why it rocks

## 📦 Prerequisites

- Complete the Data Science Foundations section (folder 09)

## Machine Learning Explained Simply

**Machine Learning = Finding patterns in data, then using those patterns to make predictions.**

That's it! Instead of writing explicit rules ("if age > 30 AND income > 50000 THEN..."), we show the computer examples and let it **learn the rules itself**.

### The Big Difference

**Traditional Programming:**
```
Rules + Data → Programs → Output
```

**Machine Learning:**
```
Data + Output → Models → Rules (patterns)
```

## The Three Types of ML

### 1. Supervised Learning — Learning with a Teacher

You have **labeled data** — examples with the "right answer" already known.

**Analogy:** Studying with an answer key!

- **Classification:** Predict categories (spam/not spam, cat/dog)
- **Regression:** Predict numbers (house price, temperature)

**Real Examples:**
- Email spam filter (classification)
- House price prediction (regression)
- Medical diagnosis (classification)

### 2. Unsupervised Learning — Finding Patterns Alone

You have **unlabeled data** — no answers, just raw data. The model finds structure!

**Analogy:** Sorting laundry without being told what the clothes are!

- **Clustering:** Group similar items together (customer segments)
- **Dimensionality Reduction:** Compress data while keeping important info

**Real Examples:**
- Customer segmentation (who are your similar customers?)
- Anomaly detection (find unusual transactions)
- Topic modeling (find themes in documents)

### 3. Reinforcement Learning — Learning from Trial and Error

An **agent** learns by taking actions in an environment, getting rewards or penalties.

**Analogy:** Training a dog with treats!

- **Game AI:** Chess, Go, video games
- **Robotics:** Robot learns to walk
- **Resource management:** Optimize data center cooling

**Real Examples:**
- AlphaGo (beat Go world champion!)
- Self-driving cars
- Recommendation systems

## The ML Workflow

```
┌─────────────────────────────────────────────────────────────┐
│                    MACHINE LEARNING WORKFLOW                 │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────┐    ┌──────────────┐    ┌──────────────────┐  │
│  │ Raw Data │ →  │ Clean/Transform│ →  │ Feature Engine  │  │
│  └──────────┘    └──────────────┘    └──────────────────┘  │
│        ↑                                          ↓        │
│        │        ┌──────────────────┐    ┌───────────────┐   │
│        │        │   Train Model    │ ←  │ Select Model │   │
│        │        └──────────────────┘    └───────────────┘   │
│        │                ↓                               │   │
│        │        ┌──────────────────┐                   │   │
│        └─────── │  Evaluate Model   │                   │   │
│                 └──────────────────┘                   │   │
│                         ↓                               │   │
│                 ┌──────────────────┐                   │   │
│                 │   Deploy/ Predict │ ← New data comes  │   │
│                 └──────────────────┘                   │   │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Step-by-Step

1. **Collect Data** — Get raw data (CSV, database, API)
2. **Clean Data** — Handle missing values, outliers
3. **Feature Engineering** — Transform data into model-friendly format
4. **Split Data** — Training set vs test set
5. **Train Model** — Find patterns in training data
6. **Evaluate** — Check performance on test data
7. **Tune** — Adjust hyperparameters
8. **Deploy** — Make predictions on new data!

## What is scikit-learn?

**scikit-learn** (sklearn) is THE beginner-friendly ML library in Python:

- Simple, consistent API
- Great documentation
- Built-in datasets
- Covers all major algorithms

```python
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_squared_error
```

## When to Use Each Type

| Problem Type | ML Type | Examples |
|--------------|---------|----------|
| Predict category | Classification | Spam detection, image classification |
| Predict number | Regression | Price prediction, temperature |
| Find groups | Clustering | Customer segmentation |
| Learn from rewards | Reinforcement | Game AI, robotics |

## Python Libraries You'll Use

```python
# Data handling
import pandas as pd
import numpy as np

# Visualization (for understanding data)
import matplotlib.pyplot as plt
import seaborn as sns

# Machine Learning
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LogisticRegression
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import accuracy_score

# Deep Learning (later!)
import tensorflow as tf  # or import torch
```

## ✅ Summary

- **ML = Finding patterns in data, making predictions**
- **Supervised:** Labeled data → learn to predict (classification/regression)
- **Unsupervised:** No labels → find structure (clustering)
- **Reinforcement:** Learn from rewards/penalties
- **scikit-learn** is the go-to library for beginners

## ➡️ Next Steps

Ready to split your data properly? Head to **[02_train_test_split.md](./02_train_test_split.md)** to learn the critical train/test split!

## 🔗 Further Reading

- [scikit-learn Documentation](https://scikit-learn.org/)
- [Machine Learning Overview](https://scikit-learn.org/stable/getting_started.html)
- [What is Machine Learning?](https://www.ibm.com/topics/machine-learning)
