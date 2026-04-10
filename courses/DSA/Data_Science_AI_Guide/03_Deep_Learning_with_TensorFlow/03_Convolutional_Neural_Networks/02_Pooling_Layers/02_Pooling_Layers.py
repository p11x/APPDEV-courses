# Topic: Pooling Layers
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Pooling Layers

I. INTRODUCTION
   - Pooling reduces spatial dimensions
   - Provides translation invariance
   - Reduces computation and parameters

II. CORE_CONCEPTS
   - Max pooling
   - Average pooling
   - Global pooling
   - Pooling window sizes and strides

III. IMPLEMENTATION
   - MaxPool2D
   - AveragePool2D
   - Global pooling
   - Custom pooling
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def max_pooling_2d():
    model = models.Sequential([
        layers.MaxPooling2D(pool_size=(2, 2), input_shape=(28, 28, 1)),
    ])
    print("MaxPooling2D (2,2):")
    print(f"  Output shape: {model.output_shape}")
    return model


def average_pooling_2d():
    model = models.Sequential([
        layers.AveragePooling2D(pool_size=(2, 2), input_shape=(28, 28, 1)),
    ])
    print("AveragePooling2D (2,2):")
    print(f"  Output shape: {model.output_shape}")
    return model


def max_pool_with_stride():
    model = models.Sequential([
        layers.MaxPooling2D(pool_size=(2, 2), strides=(2, 2), input_shape=(28, 28, 1)),
    ])
    print("MaxPooling with stride (2,2):")
    print(f"  Output shape: {model.output_shape}")
    return model


def global_max_pooling():
    model = models.Sequential([
        layers.GlobalMaxPooling2D(input_shape=(28, 28, 1)),
    ])
    print("GlobalMaxPooling2D:")
    print(f"  Output shape: {model.output_shape}")
    return model


def global_average_pooling():
    model = models.Sequential([
        layers.GlobalAveragePooling2D(input_shape=(28, 28, 1)),
    ])
    print("GlobalAveragePooling2D:")
    print(f"  Output shape: {model.output_shape}")
    return model


def pooling_same_padding():
    model = models.Sequential([
        layers.MaxPooling2D(pool_size=(2, 2), padding='same', input_shape=(28, 28, 1)),
    ])
    print("MaxPooling with 'same' padding:")
    print(f"  Output shape: {model.output_shape}")
    return model


def pooling_valid_padding():
    model = models.Sequential([
        layers.MaxPooling2D(pool_size=(3, 3), padding='valid', input_shape=(28, 28, 1)),
    ])
    print("MaxPooling with 'valid' padding:")
    print(f"  Output shape: {model.output_shape}")
    return model


def pool_1d():
    model = models.Sequential([
        layers.MaxPooling1D(pool_size=2, input_shape=(100, 1)),
    ])
    print("MaxPooling1D:")
    print(f"  Output shape: {model.output_shape}")
    return model


def pool_3d():
    model = models.Sequential([
        layers.MaxPooling3D(pool_size=(2, 2, 2), input_shape=(16, 16, 16, 1)),
    ])
    print("MaxPooling3D:")
    print(f"  Output shape: {model.output_shape}")
    return model


def manual_max_pool():
    x = tf.constant(tf.random.normal([1, 4, 4, 1]))
    result = tf.nn.max_pool2d(x, ksize=[1, 2, 2, 1], strides=[1, 2, 2, 1], padding='VALID')
    print("Manual MaxPool:")
    print(f"  Input: {x.shape}")
    print(f"  Output: {result.shape}")
    return result


def combined_conv_pool():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(28, 28, 1)),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),
        layers.Flatten(),
        layers.Dense(10)
    ])
    print("Combined Conv + Pool:")
    model.summary()
    return model


def core_implementation():
    print("MaxPooling2D:")
    max_pooling_2d()
    print("\nAveragePooling2D:")
    average_pooling_2d()
    print("\nGlobalMaxPooling:")
    global_max_pooling()
    print("\nGlobalAveragePooling:")
    global_average_pooling()
    print("\nCombined Conv + Pool:")
    combined_conv_pool()
    return True


def banking_example():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(64, 64, 1)),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(128, (3, 3), activation='relu'),
        layers.GlobalAveragePooling2D(),
        layers.Dense(64, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([100, 64, 64, 1])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Banking - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(64, 64, 3)),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(128, (3, 3), activation='relu'),
        layers.GlobalAveragePooling2D(),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([100, 64, 64, 3])
    y = tf.cast(tf.reduce_mean(X) * 4, tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Healthcare - Final accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Pooling Layers implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()