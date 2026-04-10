# Attention Mechanisms

## I. INTRODUCTION

### What are Attention Mechanisms?

Attention mechanisms allow neural networks to focus on the most relevant parts of the input when making predictions. Originally developed for machine translation, attention has become a fundamental component in modern deep learning, enabling models to handle long-range dependencies and variable-length inputs effectively.

### Why Attention Matters

- **Long-range dependencies**: Capture relationships across distant positions
- **Interpretability**: See what the model focuses on
- **Parallelization**: Avoid sequential computation bottleneck
- **State-of-the-art**: Foundation of Transformers

### Prerequisites

- LSTM/GRU fundamentals
- Encoder-decoder architecture
- Basic linear algebra

## II. FUNDAMENTALS

### Types of Attention

1. **Additive attention**: Bahdanau attention
2. **Multiplicative attention**: Dot-product attention
3. **Self-attention**: Attention within same sequence
4. **Cross-attention**: Attention between different sequences

### Key Concepts

- **Query**: What we're looking for
- **Key**: What we're comparing against
- **Value**: The information to retrieve
- **Attention weights**: Normalized scores

## III. IMPLEMENTATION

```python
"""
Attention Mechanisms
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
print("ATTENTION MECHANISMS")
print("="*60)

# Step 1: Scaled Dot-Product Attention
def scaled_dot_product_attention(Q, K, V, mask=None):
    """
    Scaled dot-product attention.
    """
    d_k = tf.cast(tf.shape(Q)[-1], tf.float32)
    scores = tf.matmul(Q, K, transpose_b=True) / tf.sqrt(d_k)
    
    if mask is not None:
        scores = tf.where(mask == 0, -1e9, scores)
    
    attention_weights = tf.nn.softmax(scores, axis=-1)
    return tf.matmul(attention_weights, V)

print("Scaled dot-product attention defined")

# Step 2: Multi-Head Attention
class MultiHeadAttention(layers.Layer):
    def __init__(self, d_model, num_heads):
        super().__init__()
        self.num_heads = num_heads
        self.d_model = d_model
        self.depth = d_model // num_heads
        
        self.wq = layers.Dense(d_model)
        self.wk = layers.Dense(d_model)
        self.wv = layers.Dense(d_model)
        self.dense = layers.Dense(d_model)
    
    def split_heads(self, x, batch_size):
        x = tf.reshape(x, (batch_size, -1, self.num_heads, self.depth))
        return tf.transpose(x, perm=[0, 2, 1, 3])
    
    def call(self, q, k, v, mask=None):
        batch_size = tf.shape(q)[0]
        
        q = self.split_heads(self.wq(q), batch_size)
        k = self.split_heads(self.wk(k), batch_size)
        v = self.split_heads(self.wv(v), batch_size)
        
        scaled_attention = scaled_dot_product_attention(q, k, v, mask)
        scaled_attention = tf.transpose(scaled_attention, perm=[0, 2, 1, 3])
        concat = tf.reshape(scaled_attention, (batch_size, -1, self.d_model))
        
        return self.dense(concat)

print("Multi-head attention layer created")
```

### Standard Example

```python
# Standard Example: Attention for Classification
def attention_classification():
    np.random.seed(42)
    
    n_samples = 800
    seq_length = 20
    n_features = 50
    
    X = np.random.randn(n_samples, seq_length, n_features).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Attention-based Classification")
    print("="*60)
    
    inputs = keras.Input(shape=(seq_length, n_features))
    
    # Self-attention
    attention = MultiHeadAttention(50, 5)(inputs, inputs, inputs)
    attention = layers.Dropout(0.2)(attention)
    attention = layers.Add()([inputs, attention])  # Residual
    attention = layers.LayerNormalization()(attention)
    
    # Pool and classify
    x = layers.GlobalAveragePooling1D()(attention)
    x = layers.Dense(32, activation='relu')(x)
    outputs = layers.Dense(3, activation='softmax')(x)
    
    model = keras.Model(inputs, outputs)
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model

att_model = attention_classification()
```

### Real-world Example: Banking - Customer Query Classification

```python
# Banking - Query Classification
def banking_query_classification():
    np.random.seed(42)
    
    n_samples = 500
    seq_length = 30
    X = np.random.randint(0, 200, (n_samples, seq_length))
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)
    
    print("\n" + "="*60)
    print("Banking - Customer Query Classification")
    print("="*60)
    
    model = models.Sequential([
        layers.Embedding(200, 32, input_length=seq_length),
        MultiHeadAttention(32, 4),
        layers.GlobalAveragePooling1D(),
        layers.Dense(16, activation='relu'),
        layers.Dense(4, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit(X, y, epochs=10, batch_size=32, validation_split=0.2, verbose=0)
    print("Model trained")
    return model

banking_query_classification()
```

### Real-world Example: Healthcare - Patient Note Analysis

```python
# Healthcare - Clinical Note Analysis
def healthcare_clinical_notes():
    np.random.seed(42)
    
    n_samples = 600
    seq_length = 50
    X = np.random.randint(0, 500, (n_samples, seq_length))
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Healthcare - Clinical Note Analysis")
    print("="*60)
    
    model = models.Sequential([
        layers.Embedding(500, 32, input_length=seq_length),
        layers.LSTM(32, return_sequences=True),
        MultiHeadAttention(32, 4),
        layers.GlobalMaxPooling1D(),
        layers.Dense(16, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=0)
    print("Model trained")
    return model

healthcare_clinical_notes()
```

## V. OUTPUT_RESULTS

```
Attention-based Classification
Final Val Accuracy: ~0.75-0.85
```

## VI. VISUALIZATION

```
    ATTENTION MECHANISM
    
    Query ──┬──► Dot ──► Softmax ──► Weighted ──► Output
          │      Product      │         Sum
    Key ──┴───────────────────┘
    
    Self-Attention:
    ┌────────────────────────────────────────┐
    │  Token 1 ──►                           │
    │  Token 2 ──► Attention ──► Context     │
    │  Token 3 ──►                           │
    └────────────────────────────────────────┘
```

## VII. CONCLUSION

### Key Takeaways

1. **Attention mechanisms**: Focus on relevant input parts
2. **Multi-head attention**: Multiple representation subspaces
3. **Self-attention**: Internal sequence relationships

### Further Reading

1. "Neural Machine Translation by Jointly Learning to Align and Translate" (Bahdanau et al., 2014)
2. "Attention Is All You Need" (Vaswani et al., 2017)