# Topic: Attention Mechanisms
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Attention Mechanisms

I. INTRODUCTION
   - Attention allows models to focus on relevant parts of input
   - Revolutionized seq2seq models and transformers
   - Types: additive, multiplicative, scaled dot-product

II. CORE_CONCEPTS
   - Query, Key, Value
   - Attention weights
   - Context vectors
   - Self-attention

III. IMPLEMENTATION
   - Attention layers
   - Multi-head attention
   - Bahdanau attention
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def bahdanau_attention():
    query = layers.Dense(32)(tf.ones([1, 10, 32]))
    value = layers.Dense(32)(tf.ones([1, 20, 32]))
    
    attention = layers.AdditiveAttention()
    context = attention([query, value])
    print(f"Bahdanau Attention output shape: {context.shape}")
    return context


def luong_attention():
    query = layers.Dense(32)(tf.ones([1, 10, 32]))
    value = layers.Dense(32)(tf.ones([1, 20, 32]))
    
    attention = layers.Attention()
    context = attention([query, value])
    print(f"Luong Attention output shape: {context.shape}")
    return context


def multi_head_attention():
    num_heads = 8
    key_dim = 64
    
    inputs = keras.Input(shape=(10, 32))
    mha = layers.MultiHeadAttention(num_heads=num_heads, key_dim=key_dim)
    output = mha(inputs, inputs)
    
    model = models.Model(inputs=inputs, outputs=output)
    print("Multi-Head Attention:")
    model.summary()
    return model


def self_attention_layer():
    model = models.Sequential([
        layers.Dense(64, input_shape=(10, 32)),
        layers.MultiHeadAttention(num_heads=4, key_dim=16),
        layers.Dense(32)
    ])
    print("Self-Attention Layer:")
    model.summary()
    return model


def attention_in_seq2seq():
    encoder_inputs = keras.Input(shape=(20, 10))
    encoder = layers.LSTM(32, return_sequences=True)
    encoder_outputs = encoder(encoder_inputs)
    
    decoder_inputs = keras.Input(shape=(10, 10))
    attention = layers.AdditiveAttention()([decoder_inputs, encoder_outputs])
    decoder_lstm = layers.LSTM(32)
    decoder_outputs = decoder_lstm(decoder_inputs)
    
    output = layers.Dense(10)(decoder_outputs)
    model = models.Model([encoder_inputs, decoder_inputs], output)
    print("Attention in Seq2Seq:")
    model.summary()
    return model


def core_implementation():
    print("Bahdanau Attention:")
    bahdanau_attention()
    print("\nLuong Attention:")
    luong_attention()
    print("\nMulti-Head Attention:")
    multi_head_attention()
    return True


def banking_example():
    inputs = keras.Input(shape=(30, 5))
    x = layers.LSTM(32, return_sequences=True)(inputs)
    x = layers.AdditiveAttention()([x, x])
    x = layers.LSTM(16)(x)
    x = layers.Dense(1, activation='sigmoid')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([100, 30, 5])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Banking - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    inputs = keras.Input(shape=(50, 10))
    x = layers.Bidirectional(layers.LSTM(32, return_sequences=True))(inputs)
    x = layers.MultiHeadAttention(num_heads=4, key_dim=16)(x, x)
    x = layers.GlobalAveragePooling1D()(x)
    x = layers.Dense(5, activation='softmax')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([100, 50, 10])
    y = tf.random.uniform([100], minval=0, maxval=5, dtype=tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Healthcare - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Attention Mechanisms implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()