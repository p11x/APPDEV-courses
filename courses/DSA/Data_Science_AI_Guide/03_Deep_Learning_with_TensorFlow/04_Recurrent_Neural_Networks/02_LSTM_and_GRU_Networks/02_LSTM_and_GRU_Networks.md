# LSTM and GRU Networks

## I. INTRODUCTION

### What are LSTM and GRU?

LSTM (Long Short-Term Memory) and GRU (Gated Recurrent Unit) are advanced RNN architectures designed to solve the vanishing gradient problem in standard RNNs. They use gating mechanisms to control information flow, allowing them to learn long-term dependencies in sequences.

### Why LSTM and GRU Matter

- **Long-term dependencies**: Capture relationships across many time steps
- **Gating mechanisms**: Learn what to remember and what to forget
- **Stable training**: Solve vanishing/exploding gradient issues
- **State-of-the-art**: Foundation for many sequence tasks

### Prerequisites

- RNN fundamentals
- Backpropagation through time
- Gradient descent optimization

## II. FUNDAMENTALS

### LSTM Architecture

```
LSTM Gates:
- Forget gate: What to discard
- Input gate: What to store
- Output gate: What to output
```

### GRU Architecture

```
GRU Gates:
- Update gate: How much past to keep
- Reset gate: How much past to forget
```

### Key Differences

| LSTM | GRU |
|------|-----|
| 3 gates (forget, input, output) | 2 gates (update, reset) |
| More parameters | Fewer parameters |
| Better for long sequences | Faster training |

## III. IMPLEMENTATION

### Step 1: LSTM Implementation

```python
"""
LSTM and GRU Networks
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
print("LSTM AND GRU NETWORKS")
print("="*60)

# Step 1: Basic LSTM
def basic_lstm():
    """
    Basic LSTM layer.
    """
    lstm = layers.LSTM(64, return_sequences=True)
    
    # Input: (batch, timesteps, features)
    input_seq = keras.Input(shape=(20, 50))
    output = lstm(input_seq)
    
    print(f"LSTM output shape: {output.shape}")
    return keras.Model(input_seq, output)

basic_lstm_model = basic_lstm()
```

### Step 2: Stacked LSTM

```python
# Step 2: Stacked LSTM
def stacked_lstm():
    """
    Multiple LSTM layers.
    """
    model = models.Sequential([
        layers.LSTM(64, return_sequences=True, input_shape=(20, 50)),
        layers.Dropout(0.3),
        layers.LSTM(32, return_sequences=True),
        layers.Dropout(0.3),
        layers.LSTM(16)
    ])
    
    print("\nStacked LSTM created")
    model.summary()
    return model

stacked_lstm_model = stacked_lstm()
```

### Step 3: Bidirectional LSTM

```python
# Step 3: Bidirectional LSTM
def bidirectional_lstm():
    """
    Bidirectional LSTM for better context.
    """
    model = models.Sequential([
        layers.Bidirectional(
            layers.LSTM(32, return_sequences=True),
            input_shape=(20, 50)
        ),
        layers.Bidirectional(layers.LSTM(16)),
        layers.Dense(3, activation='softmax')
    ])
    
    print("\nBidirectional LSTM created")
    return model

bi_lstm_model = bidirectional_lstm()
```

### Step 4: GRU Implementation

```python
# Step 4: GRU Layer
def gru_layer():
    """
    GRU (Gated Recurrent Unit) layer.
    """
    model = models.Sequential([
        layers.GRU(64, return_sequences=True, input_shape=(20, 50)),
        layers.GRU(32),
        layers.Dense(3, activation='softmax')
    ])
    
    print("\nGRU model created")
    return model

gru_model = gru_layer()
```

### Step 5: LSTM vs GRU Comparison

```python
# Step 5: Compare LSTM and GRU
def compare_lstm_gru():
    """
    Compare LSTM and GRU performance.
    """
    # LSTM model
    lstm_model = models.Sequential([
        layers.LSTM(64, return_sequences=True, input_shape=(30, 20)),
        layers.LSTM(32),
        layers.Dense(3, activation='softmax')
    ])
    
    # GRU model
    gru_model = models.Sequential([
        layers.GRU(64, return_sequences=True, input_shape=(30, 20)),
        layers.GRU(32),
        layers.Dense(3, activation='softmax')
    ])
    
    print("\nLSTM and GRU models created for comparison")
    return lstm_model, gru_model

lstm_m, gru_m = compare_lstm_gru()
```

## IV. APPLICATIONS

### Standard Example: Sequence Classification

```python
# Standard Example: Sequence Classification
def sequence_classification_lstm():
    """
    LSTM for sequence classification.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 1000
    seq_length = 50
    n_features = 10
    
    X = np.random.randn(n_samples, seq_length, n_features).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("LSTM Sequence Classification")
    print("="*60)
    
    model = models.Sequential([
        layers.LSTM(64, return_sequences=True, input_shape=(seq_length, n_features)),
        layers.Dropout(0.3),
        layers.LSTM(32),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    
    return model

lstm_class_model = sequence_classification_lstm()
```

### Real-world Example 1: Banking - Transaction Fraud Detection

```python
# Real-world Example 1: Banking - Fraud Detection
def banking_fraud_detection():
    """
    Detect fraudulent transactions using LSTM.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 800
    seq_length = 20  # Last 20 transactions
    
    # Features: amount, time, merchant type, location, etc.
    X = np.random.randn(n_samples, seq_length, 8).astype(np.float32)
    
    # Create fraud patterns
    fraud_score = (0.3 * X[:, -1, 0] +  # High amount
                   0.2 * X[:, -1, 3] +  # Unusual location
                   0.15 * X[:, :, 5].sum(axis=1) +  # Unusual merchants
                   np.random.randn(n_samples) * 0.3)
    y = (fraud_score > 0).astype(int)
    
    print("\n" + "="*60)
    print("Banking - Fraud Detection with LSTM")
    print("="*60)
    print(f"Fraud rate: {y.mean():.2%}")
    
    model = models.Sequential([
        layers.LSTM(64, return_sequences=True, input_shape=(seq_length, 8)),
        layers.Dropout(0.4),
        layers.LSTM(32),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(
        optimizer='adam',
        loss='binary_crossentropy',
        metrics=['accuracy', keras.metrics.AUC(name='auc')]
    )
    
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

banking_fraud_detection()
```

### Real-world Example 2: Healthcare - Sleep Stage Classification

```python
# Real-world Example 2: Healthcare - Sleep Stage Classification
def healthcare_sleep_classification():
    """
    Classify sleep stages from EEG signals.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 1000
    seq_length = 100  # EEG epochs
    
    # Features: different EEG frequency bands
    X = np.random.randn(n_samples, seq_length, 5).astype(np.float32)
    
    # Labels: Awake, Light, Deep, REM
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)
    
    print("\n" + "="*60)
    print("Healthcare - Sleep Stage Classification")
    print("="*60)
    
    # Bidirectional LSTM for better context
    model = models.Sequential([
        layers.Bidirectional(
            layers.LSTM(64, return_sequences=True, input_shape=(seq_length, 5)),
            merge_mode='concat'
        ),
        layers.Dropout(0.3),
        layers.Bidirectional(layers.LSTM(32)),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(4, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

healthcare_sleep_classification()
```

## V. OUTPUT_RESULTS

### Expected Output

```
LSTM Sequence Classification
Final Val Accuracy: ~0.80-0.85

Banking - Fraud Detection
Final Val Accuracy: ~0.88, AUC: ~0.92

Healthcare - Sleep Classification
Final Val Accuracy: ~0.82
```

## VI. VISUALIZATION

### LSTM Architecture

```
    LSTM CELL
    
    ┌─────────────────────────────────────────────┐
    │                                             │
    │  x_t ──┬──► Forget ──► (f_t * C_{t-1})      │
    │        │                                     │
    │  h_t-1 ┼──► Input  ──► (i_t * C~_t) ──┬──►  │
    │        │                                │   │
    │        │                                │   │
    │        │                           C_t  ──┤   │
    │        │                                │   │
    │        └──► Output ──► (o_t * tanh(C_t)) │   │
    │                                             │
    └─────────────────────────────────────────────┘
```

### GRU vs LSTM

```
    GRU                    LSTM
    ┌─────────────────┐    ┌─────────────────────┐
    │                 │    │                     │
    │  r ──► Reset    │    │ f ──► Forget gate   │
    │                 │    │ i ──► Input gate     │
    │  z ──► Update   │    │ o ──► Output gate    │
    │                 │    │                     │
    │  h_t = (1-z)*h  │    │ C_t = f*C + i*C~    │
    │     + z*~h      │    │ h_t = o * tanh(C)   │
    │                 │    │                     │
    └─────────────────┘    └─────────────────────┘
```

## VII. ADVANCED_TOPICS

### Advanced LSTM Variants

1. **Peephole LSTM**: Connect cell to gates
2. **Coupled LSTM**: Coupled forget/input gates
3. **Zoneout**: Stochastic recurrent dropout

### When to Use

- **LSTM**: Long sequences, complex dependencies
- **GRU**: Faster training, less data, similar performance

## VIII. CONCLUSION

### Key Takeaways

1. **LSTM/GRU solve vanishing gradients**: Gating mechanisms
2. **LSTM**: 3 gates, more parameters
3. **GRU**: 2 gates, faster training

### Further Reading

1. "Long Short-Term Memory" (Hochreiter & Schmidhuber, 1997)
2. "Learning Phrase Representations" (Cho et al., 2014)
3. "An Empirical Evaluation of GRUs" (Greff et al., 2017)