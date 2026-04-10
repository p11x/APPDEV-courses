# Advanced RNN Architectures

## I. INTRODUCTION

### What are Advanced RNN Architectures?

Advanced RNN architectures build on basic LSTM and GRU to include bidirectional processing, stacked layers, attention mechanisms, and architectural variations designed for improved performance on complex sequence tasks.

### Why Advanced Architectures Matter

- **Contextual understanding**: Bidirectional processing
- **Complex patterns**: Deeper networks
- **State-of-the-art**: Combine with attention
- **Different tasks**: Various configurations

### Prerequisites

- LSTM/GRU fundamentals
- Attention mechanisms
- Sequence modeling

## II. FUNDAMENTALS

### Architecture Types

1. **Stacked RNN**: Multiple layers
2. **Bidirectional RNN**: Forward and backward
3. **Attention-enhanced RNN**: Combined with attention
4. **CNN-RNN Hybrid**: Convolutional + recurrent

## III. IMPLEMENTATION

```python
"""
Advanced RNN Architectures
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
print("ADVANCED RNN ARCHITECTURES")
print("="*60)

# Step 1: Deep Stacked Bidirectional RNN
def deep_bidirectional_rnn():
    model = models.Sequential([
        layers.Bidirectional(
            layers.LSTM(64, return_sequences=True),
            input_shape=(20, 50)
        ),
        layers.Bidirectional(layers.LSTM(64, return_sequences=True)),
        layers.Bidirectional(layers.LSTM(32)),
        layers.Dense(3, activation='softmax')
    ])
    return model

# Step 2: CNN-LSTM Hybrid
def cnn_lstm_hybrid():
    model = models.Sequential([
        layers.Conv1D(64, 3, activation='relu', input_shape=(50, 20)),
        layers.MaxPooling1D(2),
        layers.Conv1D(128, 3, activation='relu'),
        layers.LSTM(64, return_sequences=True),
        layers.LSTM(32),
        layers.Dense(3, activation='softmax')
    ])
    return model

# Step 3: Attention-enhanced RNN
def attention_enhanced_rnn():
    inputs = keras.Input(shape=(20, 50))
    
    # RNN encoding
    x = layers.LSTM(64, return_sequences=True)(inputs)
    x = layers.LSTM(32)(x)
    
    # Attention
    x = layers.RepeatVector(1)(x)
    x = layers.Dense(1)(x)
    x = layers.Flatten()(x)
    x = layers.Activation('softmax')(x)
    x = layers.RepeatVector(32)(x)
    x = layers.Lambda(lambda x: tf.reduce_sum(x, 1))(x)
    
    outputs = layers.Dense(3, activation='softmax')(x)
    return keras.Model(inputs, outputs)

print("Advanced RNN architectures defined")
```

## IV. APPLICATIONS

### Standard Example

```python
# Standard: Sequence Classification
def advanced_rnn_classification():
    np.random.seed(42)
    
    n_samples = 800
    X = np.random.randn(n_samples, 30, 20).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Advanced RNN Classification")
    print("="*60)
    
    model = deep_bidirectional_rnn()
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"Final Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model

advanced_rnn_classification()
```

### Banking Example

```python
# Banking - Transaction Sequence Analysis
def banking_transaction_analysis():
    np.random.seed(42)
    
    n_samples = 600
    X = np.random.randn(n_samples, 25, 15).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)
    
    print("\n" + "="*60)
    print("Banking - Transaction Analysis")
    print("="*60)
    
    model = cnn_lstm_hybrid()
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit(X, y, epochs=10, batch_size=32, validation_split=0.2, verbose=0)
    print("Trained")
    return model

banking_transaction_analysis()
```

### Healthcare Example

```python
# Healthcare - Multivariate Time Series
def healthcare_time_series():
    np.random.seed(42)
    
    n_samples = 500
    X = np.random.randn(n_samples, 40, 10).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Healthcare - Patient Time Series")
    print("="*60)
    
    model = attention_enhanced_rnn()
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=0)
    print("Trained")
    return model

healthcare_time_series()
```

## V. CONCLUSION

### Key Takeaways

1. **Bidirectional**: Process sequences in both directions
2. **Stacked**: Multiple layers for complexity
3. **Hybrid**: CNN+RNN combinations

### Further Reading

1. "Bidirectional LSTM-CRF Models for Sequence Tagging" (Zheng et al., 2015)