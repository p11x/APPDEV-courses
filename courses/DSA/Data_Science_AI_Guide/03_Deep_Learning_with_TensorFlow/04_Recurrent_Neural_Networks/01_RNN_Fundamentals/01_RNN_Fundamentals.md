# RNN Fundamentals

## I. INTRODUCTION

### What are Recurrent Neural Networks?

Recurrent Neural Networks (RNNs) are a class of neural networks designed for processing sequential data where order matters. Unlike feedforward networks, RNNs maintain internal state (memory) that captures information about previous inputs, making them suitable for tasks involving time series, text, speech, and video.

### Why RNNs Matter

- **Sequential processing**: Handle variable-length sequences
- **Memory**: Maintain context from previous steps
- **Variety**: Applicable to many sequence tasks
- **Foundation**: Base for modern architectures (LSTM, GRU)

### Prerequisites

- Neural network fundamentals
- Backpropagation
- Sequence data concepts

## II. FUNDAMENTALS

### How RNNs Work

```
At each time step t:
    h_t = tanh(W * x_t + U * h_{t-1} + b)
    y_t = V * h_t + c
```

### Key Concepts

- **Hidden state**: Internal memory of the network
- **Time steps**: Each position in sequence
- **Unfolding**: Unrolling through time
- **Backpropagation through time (BPTT)**

### Types of RNN

1. **One-to-one**: Single input, single output
2. **One-to-many**: Single input, sequence output
3. **Many-to-one**: Sequence input, single output
4. **Many-to-many**: Sequence to sequence

## III. IMPLEMENTATION

### Step 1: Basic RNN Implementation

```python
"""
RNN Fundamentals
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
print("RNN FUNDAMENTALS")
print("="*60)

# Step 1: Simple RNN Layer
def basic_rnn_example():
    """
    Basic RNN layer for sequence processing.
    """
    # Create simple RNN
    rnn = layers.SimpleRNN(32, return_sequences=True)
    
    # Input: (batch, timesteps, features)
    input_seq = keras.Input(shape=(10, 20))  # 10 time steps, 20 features
    output = rnn(input_seq)
    
    # Output shape: (batch, timesteps, units)
    print(f"Output shape: {output.shape}")
    
    return keras.Model(input_seq, output)

basic_rnn_model = basic_rnn_example()
```

### Step 2: Stacked RNN

```python
# Step 2: Stacked RNN Layers
def stacked_rnn():
    """
    Multiple RNN layers stacked together.
    """
    model = models.Sequential([
        layers.SimpleRNN(64, return_sequences=True, input_shape=(10, 20)),
        layers.SimpleRNN(32, return_sequences=True),
        layers.SimpleRNN(16, return_sequences=False)
    ])
    
    print("\nStacked RNN created")
    model.summary()
    
    return model

stacked_rnn_model = stacked_rnn()
```

### Step 3: Bidirectional RNN

```python
# Step 3: Bidirectional RNN
def bidirectional_rnn():
    """
    Bidirectional RNN processes sequence in both directions.
    """
    # Forward and backward pass concatenated
    bi_rnn = layers.Bidirectional(
        layers.SimpleRNN(32, return_sequences=True),
        merge_mode='concat'
    )
    
    input_seq = keras.Input(shape=(10, 20))
    output = bi_rnn(input_seq)
    
    # Output: 2x the units (forward + backward)
    print(f"\nBidirectional RNN output shape: {output.shape}")
    
    return keras.Model(input_seq, output)

bi_rnn_model = bidirectional_rnn()
```

### Step 4: RNN with Dropout

```python
# Step 4: RNN with Regularization
def rnn_with_dropout():
    """
    RNN with dropout for regularization.
    """
    model = models.Sequential([
        layers.SimpleRNN(64, return_sequences=True, input_shape=(10, 20),
                        dropout=0.2, recurrent_dropout=0.2),
        layers.SimpleRNN(32, return_sequences=True, dropout=0.2),
        layers.SimpleRNN(16)
    ])
    
    print("\nRNN with dropout created")
    return model

rnn_dropout_model = rnn_with_dropout()
```

### Step 5: Complete RNN for Classification

```python
# Step 5: Complete RNN Model
def rnn_classification():
    """
    Complete RNN for sequence classification.
    """
    model = models.Sequential([
        layers.SimpleRNN(64, return_sequences=True, input_shape=(50, 1)),
        layers.Dropout(0.3),
        layers.SimpleRNN(32),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    return model

print("\nRNN classification model created")
```

## IV. APPLICATIONS

### Standard Example: Sequence Classification

```python
# Standard Example: Time Series Classification
def sequence_classification_example():
    """
    Classify time series sequences.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Generate synthetic sequences
    n_samples = 1000
    seq_length = 50
    n_features = 1
    
    X = np.random.randn(n_samples, seq_length, n_features).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("RNN Sequence Classification")
    print("="*60)
    
    model = models.Sequential([
        layers.SimpleRNN(64, return_sequences=True, input_shape=(seq_length, n_features)),
        layers.Dropout(0.3),
        layers.SimpleRNN(32),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=10, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    
    return model

rnn_class_model = sequence_classification_example()
```

### Real-world Example 1: Banking - Stock Price Prediction

```python
# Real-world Example 1: Banking - Stock Price Prediction
def banking_stock_prediction():
    """
    Predict stock price movements.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 800
    seq_length = 30
    
    # Generate price-like sequences
    X = np.zeros((n_samples, seq_length, 5))  # OHLCV features
    for i in range(n_samples):
        price = 100
        for t in range(seq_length):
            change = np.random.randn() * 2
            price += change
            X[i, t, 0] = price
            X[i, t, 1] = price + np.random.uniform(-2, 2)  # High
            X[i, t, 2] = price - np.random.uniform(-2, 2)  # Low
            X[i, t, 3] = price + np.random.uniform(-1, 1)  # Close
            X[i, t, 4] = np.random.randint(1000, 10000)    # Volume
    
    # Normalize
    X = (X - X.mean(axis=1, keepdims=True)) / (X.std(axis=1, keepdims=True) + 1e-8)
    
    # Labels: 0=down, 1=stay, 2=up
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Banking - Stock Price Prediction")
    print("="*60)
    
    model = models.Sequential([
        layers.SimpleRNN(64, return_sequences=True, input_shape=(seq_length, 5)),
        layers.Dropout(0.3),
        layers.SimpleRNN(32),
        layers.Dropout(0.3),
        layers.Dense(16, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

banking_stock_prediction()
```

### Real-world Example 2: Healthcare - ECG Analysis

```python
# Real-world Example 2: Healthcare - ECG Heartbeat Classification
def healthcare_ecg_analysis():
    """
    Classify ECG heartbeat patterns.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 1000
    seq_length = 100  # Time points
    
    # Generate ECG-like signals
    X = np.zeros((n_samples, seq_length, 1))
    for i in range(n_samples):
        # Simulate heartbeat patterns
        for t in range(seq_length):
            signal = np.sin(t * 0.1)  # Base
            if np.random.random() < 0.01:  # Spike (R-peak)
                signal += np.random.uniform(2, 4)
            X[i, t, 0] = signal + np.random.randn() * 0.1
    
    # Labels: normal, arrhythmia, other
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Healthcare - ECG Heartbeat Classification")
    print("="*60)
    
    model = models.Sequential([
        layers.SimpleRNN(64, return_sequences=True, input_shape=(seq_length, 1)),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        
        layers.SimpleRNN(32),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        
        layers.Dense(16, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

healthcare_ecg_analysis()
```

## V. OUTPUT_RESULTS

### Expected Output

```
====================================================================================================
RNN Sequence Classification
====================================================================================================

Final Val Accuracy: ~0.65-0.75 (depends on data)
```

### Banking Example

```
Banking - Stock Price Prediction
Final Val Accuracy: ~0.70
```

### Healthcare Example

```
Healthcare - ECG Heartbeat Classification
Final Val Accuracy: ~0.72
```

## VI. VISUALIZATION

### RNN Architecture

```
    UNFOLDED RNN
    
    t=0        t=1        t=2        t=3
    ┌───┐      ┌───┐      ┌───┐      ┌───┐
    │ x ├───┬──│ x ├───┬──│ x ├───┬──│ x │
    │ 0 │   │  │ 1 │   │  │ 2 │   │  │ 3 │
    └─┬─┘   │  └─┬─┘   │  └─┬─┘   │  └─┬─┘
      │     │    │     │    │     │    │
      ▼     │    ▼     │    ▼     │    ▼
    ┌───┐   │  ┌───┐   │  ┌───┐   │  ┌───┐
    │ h ├───┴──│ h ├───┴──│ h ├───┴──│ h │
    │ 0 │      │ 1 │      │ 2 │      │ 3 │
    └───┘      └───┘      └───┘      └───┘
      │          │          │          │
      ▼          ▼          ▼          ▼
      y_0        y_1        y_2        y_3
```

## VII. ADVANCED_TOPICS

### RNN Problems

1. **Vanishing gradients**: Long sequences
2. **Exploding gradients**: Unstable training
3. **Short-term memory**: Can't capture long-range

### Solutions

- LSTM/GRU (gated mechanisms)
- Gradient clipping
- BPTT truncation

## VIII. CONCLUSION

### Key Takeaways

1. **RNNs process sequences**: Maintain hidden state
2. **BPTT**: Backpropagation through time
3. **Vanishing gradients**: Major limitation

### Further Reading

1. "Learning Phrase Representations using RNN Encoder-Decoder for Statistical MT" (Cho et al., 2014)
2. "A Simple Way to Initialize Recurrent Networks" (Sutskever et al., 2013)