# Topic: Sequence Modeling Applications
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Sequence Modeling Applications

I. INTRODUCTION
   - Sequence modeling applies RNNs to real-world sequential data
   - Time series prediction, text generation, sentiment analysis
   - Data preprocessing for sequences

II. CORE_CONCEPTS
   - Time series forecasting
   - Text classification
   - Sequence-to-sequence
   - Padding and masking

III. IMPLEMENTATION
   - Time series models
   - Text classification
   - Sequence generation
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def time_series_lstm():
    model = models.Sequential([
        layers.LSTM(64, input_shape=(30, 1), return_sequences=True),
        layers.Dropout(0.2),
        layers.LSTM(32),
        layers.Dense(1)
    ])
    print("Time Series LSTM:")
    model.summary()
    return model


def text_classification_lstm():
    model = models.Sequential([
        layers.Embedding(10000, 128, input_length=100),
        layers.LSTM(64),
        layers.Dropout(0.3),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Text Classification LSTM:")
    model.summary()
    return model


def sequence_to_sequence():
    encoder_inputs = keras.Input(shape=(None, 10))
    encoder = layers.LSTM(32, return_state=True)
    encoder_outputs, state_h, state_c = encoder(encoder_inputs)
    encoder_states = [state_h, state_c]
    
    decoder_inputs = keras.Input(shape=(None, 10))
    decoder = layers.LSTM(32, return_sequences=True, return_state=True)
    decoder_outputs, _, _ = decoder(decoder_inputs, initial_state=encoder_states)
    decoder_dense = layers.Dense(10, activation='softmax')
    decoder_outputs = decoder_dense(decoder_outputs)
    
    model = models.Model([encoder_inputs, decoder_inputs], decoder_outputs)
    print("Seq2Seq Model:")
    model.summary()
    return model


def train_sequence_model():
    X = tf.random.normal([100, 30, 1])
    y = tf.random.normal([100, 1])
    
    model = models.Sequential([
        layers.LSTM(32, input_shape=(30, 1)),
        layers.Dense(1)
    ])
    
    model.compile(optimizer='adam', loss='mse')
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Training - Loss: {history.history['loss'][-1]:.4f}")
    return model


def padding_sequences():
    sequences = [
        [1, 2, 3],
        [4, 5],
        [6, 7, 8, 9]
    ]
    
    padded = keras.preprocessing.sequence.pad_sequences(sequences, maxlen=4, padding='post')
    print(f"Padded sequences:\n{padded}")
    return padded


def masking_example():
    inputs = keras.Input(shape=(3, 4))
    mask = layers.Masking(mask_value=0.0)(inputs)
    lstm_out = layers.LSTM(8)(mask)
    output = layers.Dense(1)(lstm_out)
    model = models.Model(inputs=inputs, outputs=output)
    print("Model with masking:")
    model.summary()
    return model


def core_implementation():
    print("Time Series LSTM:")
    time_series_lstm()
    print("\nText Classification LSTM:")
    text_classification_lstm()
    print("\nPadding Sequences:")
    padding_sequences()
    return True


def banking_example():
    model = models.Sequential([
        layers.LSTM(128, input_shape=(60, 3), return_sequences=True),
        layers.Dropout(0.3),
        layers.LSTM(64),
        layers.Dropout(0.3),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([200, 60, 3])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Banking - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.LSTM(128, input_shape=(100, 10), return_sequences=True),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.LSTM(64),
        layers.BatchNormalization(),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([200, 100, 10])
    y = tf.random.uniform([200], minval=0, maxval=5, dtype=tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Healthcare - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Sequence Modeling Applications implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()