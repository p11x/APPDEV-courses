# Topic: Weight Initialization Strategies
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Weight Initialization Strategies

I. INTRODUCTION
   - Proper weight initialization prevents vanishing/exploding gradients
   - Different strategies suit different activation functions
   - Critical for training stability and convergence

II. CORE_CONCEPTS
   - Zero initialization
   - Random initialization
   - Xavier/Glorot initialization
   - He initialization

III. IMPLEMENTATION
   - Various initialization methods
   - Impact on training
   - Best practices
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models, initializers
import numpy as np


def zero_initialization():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='zeros', bias_initializer='zeros', 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("Zero Initialization:")
    weights = model.get_weights()
    print(f"First layer weights: {weights[0].flatten()[:5]}")
    return model


def random_initialization():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='random_normal', 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("Random Normal Initialization:")
    weights = model.get_weights()
    print(f"Weights: {weights[0].flatten()[:5]}")
    return model


def uniform_initialization():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='random_uniform', 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("Random Uniform Initialization:")
    weights = model.get_weights()
    print(f"Weights range: [{weights[0].min():.2f}, {weights[0].max():.2f}]")
    return model


def glorot_uniform_init():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='glorot_uniform', 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("Glorot Uniform Initialization:")
    weights = model.get_weights()
    print(f"Weights shape: {weights[0].shape}")
    print(f"Weights std: {weights[0].std():.4f}")
    return model


def glorot_normal_init():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='glorot_normal', 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("Glorot Normal Initialization:")
    weights = model.get_weights()
    print(f"Weights mean: {weights[0].mean():.4f}")
    print(f"Weights std: {weights[0].std():.4f}")
    return model


def he_normal_init():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='he_normal', 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("He Normal Initialization:")
    weights = model.get_weights()
    print(f"Weights mean: {weights[0].mean():.4f}")
    print(f"Weights std: {weights[0].std():.4f}")
    return model


def he_uniform_init():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='he_uniform', 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("He Uniform Initialization:")
    weights = model.get_weights()
    print(f"Weights range: [{weights[0].min():.2f}, {weights[0].max():.2f}]")
    return model


def lecun_uniform_init():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='lecun_uniform', 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("LeCun Uniform Initialization:")
    weights = model.get_weights()
    print(f"Weights range: [{weights[0].min():.2f}, {weights[0].max():.2f}]")
    return model


def custom_initializer():
    class CustomInitializer(keras.initializers.Initializer):
        def __init__(self, factor=1.0):
            self.factor = factor
        
        def __call__(self, shape, dtype=None):
            return tf.random.normal(shape, stddev=self.factor / np.sqrt(shape[0]))
    
    model = models.Sequential([
        layers.Dense(32, kernel_initializer=CustomInitializer(0.5), 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("Custom Initializer:")
    weights = model.get_weights()
    print(f"Weights std: {weights[0].std():.4f}")
    return model


def initializer_comparison():
    initializers_list = [
        ('zeros', 'zeros'),
        ('random_normal', 'random_normal'),
        ('glorot_uniform', 'glorot_uniform'),
        ('glorot_normal', 'glorot_normal'),
        ('he_normal', 'he_normal'),
        ('he_uniform', 'he_uniform')
    ]
    
    results = {}
    for name, init in initializers_list:
        model = models.Sequential([
            layers.Dense(64, kernel_initializer=init, input_shape=(10,)),
            layers.Dense(1, activation='sigmoid')
        ])
        model.compile(optimizer='adam', loss='binary_crossentropy')
        
        X = tf.random.normal([300, 10])
        y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
        y = tf.expand_dims(y, axis=1)
        
        history = model.fit(X, y, epochs=5, verbose=0)
        results[name] = history.history['loss'][-1]
    
    print("Initializer Comparison (final loss):")
    for name, loss in results.items():
        print(f"  {name}: {loss:.4f}")
    return results


def relu_vs_tanh_init():
    X = tf.random.normal([500, 20])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model_relu = models.Sequential([
        layers.Dense(64, kernel_initializer='he_normal', activation='relu', input_shape=(20,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    model_relu.compile(optimizer='adam', loss='binary_crossentropy')
    history_relu = model_relu.fit(X, y, epochs=5, verbose=0)
    
    model_tanh = models.Sequential([
        layers.Dense(64, kernel_initializer='glorot_normal', activation='tanh', input_shape=(20,)),
        layers.Dense(32, activation='tanh'),
        layers.Dense(1, activation='sigmoid')
    ])
    model_tanh.compile(optimizer='adam', loss='binary_crossentropy')
    history_tanh = model_tanh.fit(X, y, epochs=5, verbose=0)
    
    print(f"ReLU (He init) loss: {history_relu.history['loss'][-1]:.4f}")
    print(f"Tanh (Glorot init) loss: {history_tanh.history['loss'][-1]:.4f}")
    return model_relu, model_tanh


def initialize_variance_tracking():
    model = models.Sequential([
        layers.Dense(32, kernel_initializer='glorot_uniform', input_shape=(10,)),
        layers.ReLU(),
        layers.Dense(16),
        layers.ReLU(),
        layers.Dense(1)
    ])
    
    _ = model(tf.ones([1, 10]))
    
    print("Weight statistics through layers:")
    for i, layer in enumerate(model.layers):
        if hasattr(layer, 'kernel'):
            print(f"Layer {i}: mean={layer.kernel.numpy().mean():.4f}, std={layer.kernel.numpy().std():.4f}")
    return model


def core_implementation():
    print("Initializers:")
    zero_initialization()
    print()
    random_initialization()
    print()
    glorot_uniform_init()
    print()
    he_normal_init()
    print("\nInitializer Comparison:")
    initializer_comparison()
    return True


def banking_example():
    model = models.Sequential([
        layers.Dense(128, kernel_initializer='he_normal', activation='relu', input_shape=(20,)),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Dense(64, kernel_initializer='he_normal', activation='relu'),
        layers.BatchNormalization(),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy')
    
    X = tf.random.normal([1000, 20])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0.3, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Banking - Final val_loss: {history.history['val_loss'][-1]:.4f}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.Dense(256, kernel_initializer='he_normal', activation='relu', input_shape=(50,)),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Dense(128, kernel_initializer='he_normal', activation='relu'),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Dense(64, kernel_initializer='he_normal', activation='relu'),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([1000, 50])
    y = tf.cast(tf.reduce_mean(X, axis=1) * 4, tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Healthcare - Final val_accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Weight Initialization Strategies implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()