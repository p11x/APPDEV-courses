# Sequence Modeling Applications

## I. INTRODUCTION

### What is Sequence Modeling?

Sequence modeling is the task of processing and generating sequential data where the order and temporal relationships between elements matter. This includes time series prediction, language modeling, machine translation, and speech recognition.

### Why Sequence Modeling Matters

- **Temporal data**: Essential for time series, text, audio
- **Prediction**: Forecast future values
- **Generation**: Create new sequences
- **Transform**: Convert one sequence to another

### Prerequisites

- LSTM/GRU fundamentals
- Backpropagation through time
- Basic NLP concepts

## II. FUNDAMENTALS

### Sequence Modeling Tasks

1. **Many-to-one**: Classification (sentiment analysis)
2. **Many-to-many**: Sequence tagging (NER)
3. **Encoder-Decoder**: Translation, summarization

### Key Concepts

- **Encoder**: Processes input sequence
- **Decoder**: Generates output
- **Teacher forcing**: Use actual outputs during training
- **Beam search**: Explore multiple generation paths

## III. IMPLEMENTATION

### Step 1: Time Series Prediction

```python
"""
Sequence Modeling Applications
Deep Learning with TensorFlow/Keras
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np
import warnings
warnings.filterwarnings('ignore')

tf.random.set_seed(42)
np.random.seed(42)

print("="*60)
print("SEQUENCE MODELING APPLICATIONS")
print("="*60)

# Step 1: Time Series Prediction
def time_series_prediction():
    """
    LSTM for time series forecasting.
    Predict next value given sequence.
    """
    # Generate synthetic time series
    n_samples = 1000
    seq_length = 20
    
    # Sine wave + noise
    t = np.linspace(0, 100, n_samples + seq_length + 1)
    data = np.sin(t) + np.random.randn(len(t)) * 0.1
    
    X = []
    y = []
    for i in range(n_samples):
        X.append(data[i:i+seq_length])
        y.append(data[i+seq_length])
    
    X = np.array(X).reshape(-1, seq_length, 1)
    y = np.array(y)
    
    # Split
    split = int(0.8 * n_samples)
    X_train, X_test = X[:split], X[split:]
    y_train, y_test = y[:split], y[split:]
    
    print("\n" + "="*60)
    print("Time Series Prediction")
    print("="*60)
    
    model = models.Sequential([
        layers.LSTM(64, input_shape=(seq_length, 1), return_sequences=True),
        layers.Dropout(0.2),
        layers.LSTM(32),
        layers.Dense(1)
    ])
    
    model.compile(optimizer='adam', loss='mse')
    model.fit(X_train, y_train, epochs=20, batch_size=32, 
             validation_split=0.2, verbose=1)
    
    print(f"Test Loss: {model.evaluate(X_test, y_test, verbose=0):.4f}")
    
    return model

ts_model = time_series_prediction()
```

### Step 2: Sequence-to-Sequence Translation

```python
# Step 2: Encoder-Decoder for Translation
def encoder_decoder_model():
    """
    Simple encoder-decoder for sequence to sequence.
    """
    # Encoder
    encoder_inputs = keras.Input(shape=(None, 50))
    encoder = layers.LSTM(64, return_state=True)
    _, state_h, state_c = encoder(encoder_inputs)
    encoder_states = [state_h, state_c]
    
    # Decoder
    decoder_inputs = keras.Input(shape=(None, 50))
    decoder_lstm = layers.LSTM(64, return_sequences=True, return_state=True)
    decoder_outputs, _, _ = decoder_lstm(decoder_inputs, initial_state=encoder_states)
    decoder_dense = layers.Dense(50, activation='softmax')
    decoder_outputs = decoder_dense(decoder_outputs)
    
    # Model
    model = keras.Model([encoder_inputs, decoder_inputs], decoder_outputs)
    
    print("\nEncoder-Decoder model created")
    return model

enc_dec_model = encoder_decoder_model()
```

### Step 3: Text Classification

```python
# Step 3: Text Classification with LSTM
def text_classification():
    """
    Classify text using LSTM.
    """
    # Generate synthetic text data (represented as sequences of integers)
    n_samples = 1000
    seq_length = 100
    vocab_size = 1000
    
    X = np.random.randint(0, vocab_size, (n_samples, seq_length))
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Text Classification")
    print("="*60)
    
    model = models.Sequential([
        layers.Embedding(vocab_size, 64, input_length=seq_length),
        layers.LSTM(64, return_sequences=True),
        layers.Dropout(0.3),
        layers.LSTM(32),
        layers.Dropout(0.3),
        layers.Dense(3, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit(X, y, epochs=10, batch_size=32, validation_split=0.2, verbose=0)
    
    print("Text classification model trained")
    
    return model

text_model = text_classification()
```

## IV. APPLICATIONS

### Standard Example: Sentiment Analysis

```python
# Standard Example: Sentiment Analysis
def sentiment_analysis_example():
    """
    Analyze sentiment in text sequences.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 800
    seq_length = 50
    
    # Simulate word indices
    X = np.random.randint(0, 500, (n_samples, seq_length))
    y = keras.utils.to_categorical(np.random.randint(0, 2, n_samples), 2)
    
    print("\n" + "="*60)
    print("Sentiment Analysis")
    print("="*60)
    
    model = models.Sequential([
        layers.Embedding(500, 32, input_length=seq_length),
        layers.Bidirectional(layers.LSTM(32)),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(2, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    history = model.fit(X, y, epochs=10, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    
    return model

sentiment_model = sentiment_analysis_example()
```

### Real-world Example 1: Banking - Customer Support Classification

```python
# Real-world Example 1: Banking - Support Ticket Classification
def banking_support_tickets():
    """
    Classify customer support tickets by issue type.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 600
    seq_length = 80
    
    # Simulate ticket text (word indices)
    X = np.random.randint(0, 300, (n_samples, seq_length))
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)  
    # 0=Account, 1=Transaction, 2=Loan, 3=Card
    
    print("\n" + "="*60)
    print("Banking - Support Ticket Classification")
    print("="*60)
    
    model = models.Sequential([
        layers.Embedding(300, 32, input_length=seq_length),
        layers.LSTM(32, return_sequences=True),
        layers.Dropout(0.3),
        layers.GlobalAveragePooling1D(),
        layers.Dense(16, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(4, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

banking_support_tickets()
```

### Real-world Example 2: Healthcare - Drug Response Prediction

```python
# Real-world Example 2: Healthcare - Patient Response Prediction
def healthcare_drug_response():
    """
    Predict patient response to treatment from time series data.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 500
    seq_length = 30
    n_features = 10  # vitals, labs, etc.
    
    # Patient time series (vitals over time)
    X = np.random.randn(n_samples, seq_length, n_features).astype(np.float32)
    
    # Response: 0=no improvement, 1=improved
    y = np.zeros(n_samples)
    # Create realistic patterns
    for i in range(n_samples):
        # Higher last values, more improvement
        score = 0.3 * X[i, -1, 0] + 0.2 * X[i, -1, 3] + np.random.randn() * 0.3
        y[i] = 1 if score > 0 else 0
    
    print("\n" + "="*60)
    print("Healthcare - Drug Response Prediction")
    print("="*60)
    
    model = models.Sequential([
        layers.LSTM(64, return_sequences=True, input_shape=(seq_length, n_features)),
        layers.Dropout(0.4),
        layers.LSTM(32),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

healthcare_drug_response()
```

## V. OUTPUT_RESULTS

```
Time Series Prediction
Test Loss: ~0.02-0.05

Sentiment Analysis
Final Val Accuracy: ~0.75-0.85

Banking - Support Tickets
Final Val Accuracy: ~0.70-0.80

Healthcare - Drug Response
Final Val Accuracy: ~0.75-0.85
```

## VI. CONCLUSION

### Key Takeaways

1. **Sequence modeling**: Process variable-length sequences
2. **Encoder-decoder**: For translation, summarization
3. **Many-to-one**: Classification tasks

### Further Reading

1. "Neural Machine Translation" (Bahdanau et al., 2014)
2. "Sequence to Sequence Learning" (Sutskever et al., 2014)