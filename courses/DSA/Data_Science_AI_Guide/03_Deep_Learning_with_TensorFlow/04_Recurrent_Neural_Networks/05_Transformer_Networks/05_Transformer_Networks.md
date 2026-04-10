# Transformer Networks

## I. INTRODUCTION

### What are Transformer Networks?

Transformers are a neural network architecture that relies entirely on self-attention mechanisms to compute representations, without using recurrence or convolutions. Introduced in the "Attention Is All You Need" paper, Transformers have revolutionized natural language processing and become the backbone of modern AI.

### Why Transformers Matter

- **Parallelization**: Process entire sequences simultaneously
- **Long-range dependencies**: Self-attention captures distant relationships
- **State-of-the-art**: GPT, BERT, T5, and more
- **Versatility**: NLP, vision, audio, multimodal

### Prerequisites

- Attention mechanisms
- Neural network fundamentals
- Basic linear algebra

## II. FUNDAMENTALS

### Transformer Architecture

1. **Encoder**: Processes input sequence
2. **Decoder**: Generates output sequence
3. **Multi-head attention**: Multiple attention heads
4. **Position encoding**: Sequence order information

### Key Components

- **Self-attention**: Attention within same sequence
- **Cross-attention**: Encoder-decoder attention
- **Feed-forward networks**: Position-wise FFN
- **Residual connections**: Deep networks
- **Layer normalization**: Training stability

## III. IMPLEMENTATION

```python
"""
Transformer Networks
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
print("TRANSFORMER NETWORKS")
print("=+60)

# Step 1: Positional Encoding
class PositionalEncoding(layers.Layer):
    def __init__(self, d_model, max_len=5000):
        super().__init__()
        self.d_model = d_model
    
    def call(self, x):
        seq_len = tf.shape(x)[1]
        position = tf.range(seq_len)[:, tf.newaxis]
        div_term = tf.exp(tf.range(0, self.d_model, 2) * (-np.log(10000.0) / self.d_model))
        
        pe = tf.zeros((seq_len, self.d_model))
        pe = tf.Variable(pe, trainable=False)
        pe[:, 0::2].assign(tf.sin(position * div_term))
        pe[:, 1::2].assign(tf.cos(position * div_term))
        
        return x + pe

print("Positional encoding defined")

# Step 2: Transformer Encoder Block
class TransformerEncoderBlock(layers.Layer):
    def __init__(self, d_model, num_heads, d_ff, dropout=0.1):
        super().__init__()
        self.mha = layers.MultiHeadAttention(num_heads=num_heads, key_dim=d_model)
        self.ffn = keras.Sequential([
            layers.Dense(d_ff, activation='relu'),
            layers.Dense(d_model)
        ])
        self.layernorm1 = layers.LayerNormalization(epsilon=1e-6)
        self.layernorm2 = layers.LayerNormalization(epsilon=1e-6)
        self.dropout1 = layers.Dropout(dropout)
        self.dropout2 = layers.Dropout(dropout)
    
    def call(self, x, training=False):
        attn_output = self.mha(x, x)
        attn_output = self.dropout1(attn_output, training=training)
        out1 = self.layernorm1(x + attn_output)
        
        ffn_output = self.ffn(out1)
        ffn_output = self.dropout2(ffn_output, training=training)
        out2 = self.layernorm2(out1 + ffn_output)
        
        return out2

print("Transformer encoder block created")

# Step 3: Complete Transformer
def create_transformer(seq_len, vocab_size, d_model=64, num_heads=4, d_ff=256, num_layers=2):
    inputs = layers.Input(shape=(seq_len,))
    x = layers.Embedding(vocab_size, d_model)(inputs)
    x = PositionalEncoding(d_model)(x)
    
    for _ in range(num_layers):
        x = TransformerEncoderBlock(d_model, num_heads, d_ff)(x)
    
    x = layers.GlobalAveragePooling1D()(x)
    outputs = layers.Dense(3, activation='softmax')(x)
    
    return keras.Model(inputs, outputs)

print("Transformer model created")
```

### Standard Example

```python
# Standard Example: Classification with Transformer
def transformer_classification():
    np.random.seed(42)
    
    n_samples = 800
    seq_length = 30
    
    X = np.random.randint(0, 200, (n_samples, seq_length))
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Transformer Classification")
    print("="*60)
    
    model = create_transformer(seq_length, 200)
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    return model

transformer_classification()
```

## IV. APPLICATIONS

### Banking - Sentiment Analysis

```python
# Banking - Transaction Description Classification
def banking_sentiment_analysis():
    np.random.seed(42)
    
    n_samples = 600
    seq_length = 40
    
    X = np.random.randint(0, 150, (n_samples, seq_length))
    y = keras.utils.to_categorical(np.random.randint(0, 2, n_samples), 2)
    
    print("\n" + "="*60)
    print("Banking - Transaction Sentiment")
    print("="*60)
    
    model = create_transformer(seq_length, 150, num_layers=3)
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit(X, y, epochs=10, batch_size=32, validation_split=0.2, verbose=0)
    print("Trained")
    return model

banking_sentiment_analysis()
```

### Healthcare - Clinical Text

```python
# Healthcare - Clinical Note Classification
def healthcare_clinical_notes():
    np.random.seed(42)
    
    n_samples = 500
    seq_length = 50
    
    X = np.random.randint(0, 300, (n_samples, seq_length))
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)
    
    print("\n" + "="*60)
    print("Healthcare - Clinical Note Classification")
    print("="*60)
    
    model = create_transformer(seq_length, 300, d_model=128, num_heads=8, num_layers=3)
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=0)
    print("Trained")
    return model

healthcare_clinical_notes()
```

## V. CONCLUSION

### Key Takeaways

1. **Transformers**: Self-attention, no recurrence
2. **Positional encoding**: Sequence order
3. **Multi-head attention**: Multiple perspectives
4. **State-of-the-art**: Foundation of modern NLP

### Further Reading

1. "Attention Is All You Need" (Vaswani et al., 2017)
2. "BERT: Pre-training of Deep Bidirectional Transformers" (Devlin et al., 2018)
3. "GPT-2: Language Models are Unsupervised Multitask Learners" (Radford et al., 2019)