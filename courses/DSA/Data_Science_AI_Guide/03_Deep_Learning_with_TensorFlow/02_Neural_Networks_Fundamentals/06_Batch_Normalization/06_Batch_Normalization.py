# Topic: Batch Normalization
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Batch Normalization

I. INTRODUCTION
   - Batch normalization normalizes layer inputs
   - Reduces internal covariate shift
   - Enables higher learning rates and faster convergence

II. CORE_CONCEPTS
   - Mean and variance computation
   - Learnable parameters (gamma, beta)
   - Training vs inference mode
   - Impact on gradient flow

III. IMPLEMENTATION
   - BatchNorm layer usage
   - Different positions in network
   - Best practices
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def basic_batch_norm():
    model = models.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Dense(32),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Basic Batch Normalization:")
    model.summary()
    return model


def batch_norm_after_dense():
    model = models.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.BatchNormalization(),
        layers.Dense(32),
        layers.Dense(1, activation='sigmoid')
    ])
    print("BatchNorm after Dense:")
    return model


def batch_norm_before_activation():
    model = models.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.BatchNormalization(momentum=0.99, epsilon=0.001),
        layers.ReLU(),
        layers.Dense(32),
        layers.Dense(1, activation='sigmoid')
    ])
    print("BatchNorm before Activation:")
    return model


def batch_norm_momentum():
    model = models.Sequential([
        layers.Dense(64, input_shape=(10,), batch_normalization_momentum=0.9),
        layers.Dense(32),
        layers.Dense(1)
    ])
    print("BatchNorm with momentum:")
    return model


def batch_norm_training_vs_inference():
    model = models.Sequential([
        layers.Dense(32, input_shape=(10,)),
        layers.BatchNormalization(),
        layers.Dense(1, activation='sigmoid')
    ])
    
    _ = model(tf.ones([5, 10]), training=False)
    print(f"Training mode: mean={model.layers[1].moving_mean.numpy().mean():.4f}")
    print(f"              variance={model.layers[1].moving_variance.numpy().mean():.4f}")
    
    return model


def custom_batch_norm_layer():
    class CustomBatchNorm(layers.Layer):
        def __init__(self, momentum=0.99, epsilon=1e-3, **kwargs):
            super().__init__(**kwargs)
            self.momentum = momentum
            self.epsilon = epsilon
        
        def build(self, input_shape):
            self.gamma = self.add_weight(name='gamma', shape=(input_shape[-1],),
                                        initializer='ones', trainable=True)
            self.beta = self.add_weight(name='beta', shape=(input_shape[-1],),
                                       initializer='zeros', trainable=True)
            self.moving_mean = self.add_weight(name='moving_mean', shape=(input_shape[-1],),
                                              initializer='zeros', trainable=False)
            self.moving_variance = self.add_weight(name='moving_variance', shape=(input_shape[-1],),
                                                   initializer='ones', trainable=False)
        
        def call(self, inputs, training=None):
            if training:
                mean, variance = tf.nn.moments(inputs, axes=0)
                self.moving_mean.assign(self.momentum * self.moving_mean + (1 - self.momentum) * mean)
                self.moving_variance.assign(self.momentum * self.moving_variance + (1 - self.momentum) * variance)
            else:
                mean = self.moving_mean
                variance = self.moving_variance
            
            normalized = (inputs - mean) / tf.sqrt(variance + self.epsilon)
            return self.gamma * normalized + self.beta
    
    layer = CustomBatchNorm()
    x = tf.random.normal([10, 5])
    output = layer(x, training=True)
    print(f"Custom BatchNorm output shape: {output.shape}")
    return layer


def compare_with_without_bn():
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model_without_bn = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    model_without_bn.compile(optimizer='adam', loss='binary_crossentropy')
    history_without = model_without_bn.fit(X, y, epochs=10, verbose=0)
    
    model_with_bn = models.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Dense(32),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Dense(1, activation='sigmoid')
    ])
    model_with_bn.compile(optimizer='adam', loss='binary_crossentropy')
    history_with = model_with_bn.fit(X, y, epochs=10, verbose=0)
    
    print(f"Without BN - final loss: {history_without.history['loss'][-1]:.4f}")
    print(f"With BN - final loss: {history_with.history['loss'][-1]:.4f}")
    return model_with_bn


def batch_norm_in_cnn():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), input_shape=(28, 28, 1)),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3)),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Flatten(),
        layers.Dense(10)
    ])
    print("CNN with BatchNorm:")
    model.summary()
    return model


def batch_norm_in_rnn():
    model = models.Sequential([
        layers.LSTM(32, input_shape=(10, 1), return_sequences=True),
        layers.BatchNormalization(),
        layers.LSTM(16),
        layers.BatchNormalization(),
        layers.Dense(1)
    ])
    print("RNN with BatchNorm:")
    model.summary()
    return model


def fused_batch_norm():
    model = models.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.BatchNormalization(fused=True),
        layers.Dense(1)
    ])
    print("Fused BatchNorm:")
    return model


def moving_average_tracking():
    model = models.Sequential([
        layers.Dense(32, input_shape=(10,)),
        layers.BatchNormalization(),
        layers.Dense(1)
    ])
    
    bn_layer = model.layers[1]
    
    for i in range(5):
        x = tf.random.normal([32, 10])
        _ = model(x, training=True)
    
    print(f"Moving mean: {bn_layer.moving_mean.numpy()[:5]}")
    print(f"Moving variance: {bn_layer.moving_variance.numpy()[:5]}")
    return model


def batch_norm_freeze():
    model = models.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.BatchNormalization(),
        layers.Dense(32),
        layers.Dense(1)
    ])
    
    model.layers[1].trainable = False
    print(f"BN layer trainable: {model.layers[1].trainable}")
    return model


def batch_norm_custom_momentum():
    model = models.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.BatchNormalization(momentum=0.5),
        layers.Dense(1)
    ])
    print("BatchNorm with custom momentum (0.5):")
    return model


def core_implementation():
    print("Basic Batch Normalization:")
    basic_batch_norm()
    print("\nCompare with/without BN:")
    compare_with_without_bn()
    print("\nCNN with BatchNorm:")
    batch_norm_in_cnn()
    print("\nMoving Average Tracking:")
    moving_average_tracking()
    return True


def banking_example():
    model = models.Sequential([
        layers.Dense(128, input_shape=(20,)),
        layers.BatchNormalization(momentum=0.99),
        layers.ReLU(),
        layers.Dropout(0.3),
        layers.Dense(64),
        layers.BatchNormalization(momentum=0.99),
        layers.ReLU(),
        layers.Dropout(0.3),
        layers.Dense(32),
        layers.BatchNormalization(momentum=0.99),
        layers.ReLU(),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([2000, 20])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0.3, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Banking - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.Dense(256, input_shape=(50,)),
        layers.BatchNormalization(momentum=0.99),
        layers.ReLU(),
        layers.Dropout(0.4),
        layers.Dense(128),
        layers.BatchNormalization(momentum=0.99),
        layers.ReLU(),
        layers.Dropout(0.3),
        layers.Dense(64),
        layers.BatchNormalization(momentum=0.99),
        layers.ReLU(),
        layers.Dropout(0.2),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([1000, 50])
    y = tf.cast(tf.reduce_mean(X, axis=1) * 4, tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Healthcare - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Batch Normalization implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()