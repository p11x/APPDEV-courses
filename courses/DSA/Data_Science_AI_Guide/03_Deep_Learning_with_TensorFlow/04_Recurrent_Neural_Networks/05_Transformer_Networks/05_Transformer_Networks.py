# Topic: Transformer Networks
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Transformer Networks

I. INTRODUCTION
   - Transformer is the state-of-the-art architecture for NLP
   - Uses self-attention instead of recurrence
   - Powers BERT, GPT, and other large language models

II. CORE_CONCEPTS
   - Self-attention mechanism
   - Positional encoding
   - Feed-forward networks
   - Layer normalization

III. IMPLEMENTATION
   - Transformer encoder
   - Transformer decoder
   - Complete transformer
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def positional_encoding(max_position, d_model):
    position = np.arange(max_position)[:, np.newaxis]
    div_term = np.exp(np.arange(0, d_model, 2) * (-np.log(10000.0) / d_model))
    pe = np.zeros((max_position, d_model))
    pe[:, 0::2] = np.sin(position * div_term)
    pe[:, 1::2] = np.cos(position * div_term)
    return tf.constant(pe, dtype=tf.float32)


def transformer_encoder_block():
    inputs = keras.Input(shape=(None, 64))
    
    attn_output = layers.MultiHeadAttention(num_heads=4, key_dim=16)(inputs, inputs)
    attn_output = layers.Dropout(0.1)(attn_output)
    out1 = layers.LayerNormalization(epsilon=1e-6)(inputs + attn_output)
    
    ffn = layers.Dense(128, activation='relu')(out1)
    ffn = layers.Dense(64)(ffn)
    ffn = layers.Dropout(0.1)(ffn)
    output = layers.LayerNormalization(epsilon=1e-6)(out1 + ffn)
    
    model = models.Model(inputs=inputs, outputs=output)
    print("Transformer Encoder Block:")
    model.summary()
    return model


def transformer_decoder_block():
    inputs = keras.Input(shape=(None, 64))
    encoder_output = keras.Input(shape=(None, 64))
    
    attn1 = layers.MultiHeadAttention(num_heads=4, key_dim=16)(inputs, inputs)
    attn1 = layers.Dropout(0.1)(attn1)
    out1 = layers.LayerNormalization(epsilon=1e-6)(inputs + attn1)
    
    attn2 = layers.MultiHeadAttention(num_heads=4, key_dim=16)(out1, encoder_output)
    attn2 = layers.Dropout(0.1)(attn2)
    out2 = layers.LayerNormalization(epsilon=1e-6)(out1 + attn2)
    
    ffn = layers.Dense(128, activation='relu')(out2)
    ffn = layers.Dense(64)(ffn)
    ffn = layers.Dropout(0.1)(ffn)
    output = layers.LayerNormalization(epsilon=1e-6)(out2 + ffn)
    
    model = models.Model(inputs=[inputs, encoder_output], outputs=output)
    print("Transformer Decoder Block:")
    model.summary()
    return model


def simple_transformer():
    vocab_size = 10000
    max_length = 100
    embed_dim = 64
    num_heads = 4
    feed_forward_dim = 128
    
    inputs = keras.Input(shape=(max_length,), dtype=tf.int32)
    embedding = layers.Embedding(vocab_size, embed_dim)(inputs)
    positional = positional_encoding(max_length, embed_dim)
    x = embedding + positional[:max_length]
    
    for _ in range(2):
        x = layers.MultiHeadAttention(num_heads=num_heads, key_dim=embed_dim)(x, x)
        x = layers.Dropout(0.1)(x)
        x = layers.LayerNormalization(epsilon=1e-6)(x)
        
        x = layers.Dense(feed_forward_dim, activation='relu')(x)
        x = layers.Dense(embed_dim)(x)
        x = layers.Dropout(0.1)(x)
        x = layers.LayerNormalization(epsilon=1e-6)(x)
    
    x = layers.GlobalAveragePooling1D()(x)
    outputs = layers.Dense(1, activation='sigmoid')(x)
    
    model = models.Model(inputs=inputs, outputs=outputs)
    print("Simple Transformer:")
    model.summary()
    return model


def core_implementation():
    print("Transformer Encoder Block:")
    transformer_encoder_block()
    print("\nSimple Transformer:")
    simple_transformer()
    return True


def banking_example():
    max_len = 50
    embed_dim = 64
    
    inputs = keras.Input(shape=(max_len,), dtype=tf.int32)
    x = layers.Embedding(5000, embed_dim)(inputs)
    x = x + positional_encoding(max_len, embed_dim)[:max_len]
    
    x = layers.MultiHeadAttention(num_heads=4, key_dim=16)(x, x)
    x = layers.Dropout(0.2)(x)
    x = layers.LayerNormalization(epsilon=1e-6)(x)
    
    x = layers.Dense(128, activation='relu')(x)
    x = layers.GlobalAveragePooling1D()(x)
    x = layers.Dense(1, activation='sigmoid')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.uniform([100, max_len], minval=0, maxval=5000, dtype=tf.int32)
    y = tf.cast(tf.reduce_mean(tf.cast(X, tf.float32)) > 2500, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Banking - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    max_len = 100
    embed_dim = 64
    
    inputs = keras.Input(shape=(max_len,), dtype=tf.int32)
    x = layers.Embedding(10000, embed_dim)(inputs)
    x = x + positional_encoding(max_len, embed_dim)[:max_len]
    
    x = layers.MultiHeadAttention(num_heads=8, key_dim=16)(x, x)
    x = layers.LayerNormalization(epsilon=1e-6)(x)
    
    x = layers.Dense(128, activation='relu')(x)
    x = layers.Dropout(0.3)(x)
    x = layers.GlobalAveragePooling1D()(x)
    x = layers.Dense(5, activation='softmax')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.uniform([100, max_len], minval=0, maxval=10000, dtype=tf.int32)
    y = tf.random.uniform([100], minval=0, maxval=5, dtype=tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Healthcare - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Transformer Networks implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()