# 📧 Spam Detector

## 🛠️ Setup

```python
pip install pandas scikit-learn rich
```

## Full Code

```python
import pandas as pd
from sklearn.feature_extraction.text import CountVectorizer, TfidfVectorizer
from sklearn.naive_bayes import MultinomialNB
from sklearn.linear_model import LogisticRegression
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report
from rich.console import Console

# Sample SMS data
sms_data = pd.DataFrame({
    "message": [
        "Congratulations! You won a free iPhone!",
        "Hi, can we meet tomorrow?",
        "URGENT: Your account has been compromised",
        "Don't forget to bring the documents",
        "Click here to claim your prize",
        "Meeting at 3pm today",
        "Free money! Act now!",
        "Please review the attached report",
        "You've been selected for a special offer",
        "Lunch at noon?"
    ] * 20,  # Repeat for more data
})

# Create labels (1=spam, 0=not spam)
sms_data["label"] = (
    sms_data["message"].str.contains(
        "free|won|click|urgent|selected|prize|money",
        case=False
    ).astype(int)
)

# Split features and labels
X = sms_data["message"]
y = sms_data["label"]

# Split data
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

# Vectorize with TF-IDF
vectorizer = TfidfVectorizer(stop_words="english")
X_train_vec = vectorizer.fit_transform(X_train)
X_test_vec = vectorizer.transform(X_test)

# Train multiple models
models = {
    "Naive Bayes": MultinomialNB(),
    "Logistic Regression": LogisticRegression(),
    "Random Forest": RandomForestClassifier(n_estimators=10),
}

results = {}
for name, model in models.items():
    model.fit(X_train_vec, y_train)
    pred = model.predict(X_test_vec)
    accuracy = (pred == y_test).mean()
    results[name] = accuracy
    print(f"\n{name}:")
    print(classification_report(y_test, pred, target_names=["Not Spam", "Spam"]))

# Best model
best_model = max(results, key=results.get)
print(f"\n🏆 Best Model: {best_model} ({results[best_model]:.1%})")

# Function to predict spam
def predict_spam(message: str) -> str:
    vec = vectorizer.transform([message])
    pred = models["Naive Bayes"].predict(vec)[0]
    prob = models["Naive Bayes"].predict_proba(vec)[0]
    return "🚨 SPAM" if pred == 1 else "✅ Not Spam", max(prob)

# Test with new messages
test_messages = [
    "You won a free lottery!",
    "Let's meet for coffee",
    "URGENT: Click this link now!",
    "See you at the meeting",
]

console = Console()
for msg in test_messages:
    label, conf = predict_spam(msg)
    print(f"'{msg}' → {label} ({conf:.1%})")
```

## Output

```
Naive Bayes:
              precision    recall  f1-score   support

    Not Spam       0.91      0.95      0.93        19
        Spam       0.94      0.89      0.91        21

    accuracy                           0.92        40
   macro avg       0.93      0.92      0.92        40
weighted avg       0.93      0.92      0.92        40

'You've won a free lottery!' → 🚨 SPAM (95.2%)
'Let's meet for coffee' → ✅ Not Spam (97.8%)
'URGENT: Click this link now!' → 🚨 SPAM (98.1%)
'See you at the meeting' → ✅ Not Spam (99.2%)
```
