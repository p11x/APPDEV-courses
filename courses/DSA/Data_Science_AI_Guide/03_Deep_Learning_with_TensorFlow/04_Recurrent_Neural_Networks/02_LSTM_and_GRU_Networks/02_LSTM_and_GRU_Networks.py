# Topic: LSTM and GRU Networks
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for LSTM and GRU Networks

I. INTRODUCTION
   - LSTM (Long Short-Term Memory) and GRU (Gated Recurrent Unit)
   - Solve vanishing gradient problem in RNNs
   - Better at capturing long-term dependencies

II. CORE_CONCEPTS
   - Memory cell and gates
   - Input, forget, output gates
   - Update and reset gates
   - Bidirectional variants

III. IMPLEMENTATION
   - LSTM layers
   - GRU layers
   - Stacked and bidirectional
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def lstm_basic():
    model = models.Sequential([
        layers.LSTM(32, input_shape=(10, 1)),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Basic LSTM:")
    model.summary()
    return model


def lstm_return_sequences():
    model = models.Sequential([
        layers.LSTM(32, return_sequences=True, input_shape=(10, 1)),
        layers.LSTM(16),
        layers.Dense(1, activation='sigmoid')
    ])
    print("LSTM with return_sequences:")
    model.summary()
    return model


def gru_basic():
    model = models.Sequential([
        layers.GRU(32, input_shape=(10, 1)),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Basic GRU:")
    model.summary()
    return model


def gru_return_sequences():
    model = models.Sequential([
        layers.GRU(32, return_sequences=True, input_shape=(10, 1)),
        layers.GRU(16),
        layers.Dense(1, activation='sigmoid')
    ])
    print("GRU with return_sequences:")
    model.summary()
    return model


def bidirectional_lstm():
    model = models.Sequential([
        layers.Bidirectional(layers.LSTM(32, return_sequences=True), input_shape=(10, 1)),
        layers.Bidirectional(layers.LSTM(16)),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Bidirectional LSTM:")
    model.summary()
    return model


def bidirectional_gru():
    model = models.Sequential([
        layers.Bidirectional(layers.GRU(32, return_sequences=True), input_shape=(10, 1)),
        layers.Bidirectional(layers.GRU(16)),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Bidirectional GRU:")
    model.summary()
    return model


def stacked_lstm():
    model = models.Sequential([
        layers.LSTM(64, return_sequences=True, input_shape=(20, 10)),
        layers.Dropout(0.2),
        layers.LSTM(32, return_sequences=True),
        layers.LSTM(16),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Stacked LSTM:")
    model.summary()
    return model


def compare_lstm_gru():
    X = tf.random.normal([100, 20, 10])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model_lstm = models.Sequential([
        layers.LSTM(32, input_shape=(20, 10)),
        layers.Dense(1, activation='sigmoid')
    ])
    model_lstm.compile(optimizer='adam', loss='binary_crossentropy')
    history_lstm = model_lstm.fit(X, y, epochs=5, verbose=0)
    
    model_gru = models.Sequential([
        layers.GRU(32, input_shape=(20, 10)),
        layers.Dense(1, activation='sigmoid')
    ])
    model_gru.compile(optimizer='adam', loss='binary_crossentropy')
    history_gru = model_gru.fit(X, y, epochs=5, verbose=0)
    
    print(f"LSTM final loss: {history_lstm.history['loss'][-1]:.4f}")
    print(f"GRU final loss: {history_gru.history['loss'][-1]:.4f}")
    return model_lstm, model_gru


def lstm_with_peepholes():
    model = models.Sequential([
        layers.LSTM(32, input_shape=(10, 1), recurrent_activation='sigmoid'),
        layers.Dense(1)
    ])
    print("LSTM with default activation:")
    return model


def core_implementation():
    print("Basic LSTM:")
    lstm_basic()
    print("\nBasic GRU:")
    gru_basic()
    print("\nBidirectional LSTM:")
    bidirectional_lstm()
    print("\nCompare LSTM vs GRU:")
    compare_lstm_gru()
    return True


def banking_example():
    model = models.Sequential([
        layers.Bidirectional(layers.LSTM(64, return_sequences=True), input_shape=(30, 5)),
        layers.Dropout(0.3),
        layers.Bidirectional(layers.LSTM(32)),
        layers.Dropout(0.3),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([200, 30, 5])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Banking - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.Bidirectional(layers.LSTM(128, return_sequences=True), input_shape=(50, 10)),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Bidirectional(layers.LSTM(64)),
        layers.BatchNormalization(),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([200, 50, 10])
    y = tf.random.uniform([200], minval=0, maxval=5, dtype=tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Healthcare - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing LSTM and GRU Networks implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()