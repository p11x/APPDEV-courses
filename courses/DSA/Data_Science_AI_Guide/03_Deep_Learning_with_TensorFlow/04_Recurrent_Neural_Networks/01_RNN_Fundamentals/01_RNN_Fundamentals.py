# Topic: RNN Fundamentals
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for RNN Fundamentals

I. INTRODUCTION
   - Recurrent Neural Networks process sequential data
   - Maintain hidden state to capture temporal dependencies
   - Used for time series, NLP, and sequence modeling

II. CORE_CONCEPTS
   - Recurrent connections
   - Hidden state updates
   - Forward propagation through time
   - Unrolling across time steps

III. IMPLEMENTATION
   - SimpleRNN layer
   - Sequence processing
   - State management
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def simple_rnn_layer():
    model = models.Sequential([
        layers.SimpleRNN(32, input_shape=(10, 1)),
        layers.Dense(1)
    ])
    print("SimpleRNN Layer:")
    model.summary()
    return model


def rnn_with_return_sequences():
    model = models.Sequential([
        layers.SimpleRNN(32, return_sequences=True, input_shape=(10, 1)),
        layers.SimpleRNN(16),
        layers.Dense(1)
    ])
    print("RNN with return_sequences:")
    model.summary()
    return model


def stacked_rnn():
    model = models.Sequential([
        layers.SimpleRNN(64, return_sequences=True, input_shape=(20, 10)),
        layers.Dropout(0.2),
        layers.SimpleRNN(32, return_sequences=True),
        layers.SimpleRNN(16),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Stacked RNN:")
    model.summary()
    return model


def bidirectional_rnn():
    model = models.Sequential([
        layers.Bidirectional(layers.SimpleRNN(32, return_sequences=True), input_shape=(10, 1)),
        layers.Bidirectional(layers.SimpleRNN(16)),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Bidirectional RNN:")
    model.summary()
    return model


def manual_rnn_step():
    input_dim = 4
    hidden_dim = 8
    
    Wxh = tf.Variable(tf.random.normal([input_dim, hidden_dim]))
    Whh = tf.Variable(tf.random.normal([hidden_dim, hidden_dim]))
    bh = tf.Variable(tf.zeros([hidden_dim]))
    
    h = tf.zeros([hidden_dim])
    inputs = tf.random.normal([5, input_dim])
    
    for t in range(5):
        h = tf.nn.tanh(tf.matmul(tf.expand_dims(inputs[t], 0), Wxh) + tf.matmul(tf.expand_dims(h, 0), Whh) + bh)
        h = tf.squeeze(h)
    
    print(f"Manual RNN - Final hidden state shape: {h.shape}")
    return h


def rnn_sequence_classification():
    model = models.Sequential([
        layers.SimpleRNN(32, input_shape=(20, 10)),
        layers.Dropout(0.3),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([100, 20, 10])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"RNN Classification - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def rnn_sequence_regression():
    model = models.Sequential([
        layers.SimpleRNN(32, input_shape=(20, 10)),
        layers.Dense(1)
    ])
    
    model.compile(optimizer='adam', loss='mse')
    
    X = tf.random.normal([100, 20, 10])
    y = tf.random.normal([100, 1])
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"RNN Regression - Loss: {history.history['loss'][-1]:.4f}")
    return model


def rnn_state_management():
    model = models.Sequential([
        layers.SimpleRNN(32, input_shape=(10, 1), stateful=False),
        layers.Dense(1)
    ])
    print(f"RNN stateful: {model.layers[0].stateful}")
    return model


def core_implementation():
    print("SimpleRNN:")
    simple_rnn_layer()
    print("\nStacked RNN:")
    stacked_rnn()
    print("\nBidirectional RNN:")
    bidirectional_rnn()
    print("\nManual RNN:")
    manual_rnn_step()
    return True


def banking_example():
    model = models.Sequential([
        layers.SimpleRNN(64, input_shape=(30, 5), return_sequences=True),
        layers.Dropout(0.3),
        layers.SimpleRNN(32),
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
        layers.SimpleRNN(128, input_shape=(50, 10), return_sequences=True),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.SimpleRNN(64),
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
    print("Executing RNN Fundamentals implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()