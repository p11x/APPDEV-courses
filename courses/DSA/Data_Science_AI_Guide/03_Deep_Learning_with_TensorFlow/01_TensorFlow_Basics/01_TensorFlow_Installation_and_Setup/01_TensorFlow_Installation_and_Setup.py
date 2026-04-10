# Topic: TensorFlow Installation and Setup
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for TensorFlow Installation and Setup

I. INTRODUCTION
   - TensorFlow is an open-source deep learning framework developed by Google
   - Provides comprehensive tools for building and deploying machine learning models
   - Supports CPU, GPU, and TPU computations

II. CORE_CONCEPTS
   - TensorFlow installation methods
   - Environment configuration
   - Version management
   - GPU/CUDA setup

III. IMPLEMENTATION
   - Installation procedures
   - Environment verification
   - Configuration testing
"""

import tensorflow as tf
from tensorflow import keras
import numpy as np
import sys
import os
import subprocess


def check_tensorflow_version():
    print(f"TensorFlow version: {tf.__version__}")
    print(f"Keras version: {keras.__version__}")
    return tf.__version__


def check_gpu_availability():
    gpus = tf.config.list_physical_devices('GPU')
    if gpus:
        print(f"GPUs available: {len(gpus)}")
        for gpu in gpus:
            print(f"  - {gpu}")
    else:
        print("No GPU detected, using CPU")
    return gpus


def test_basic_tensorflow_operations():
    a = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    b = tf.constant([[5.0, 6.0], [7.0, 8.0]])
    c = tf.matmul(a, b)
    print(f"Matrix multiplication result:\n{c.numpy()}")
    return c


def verify_tensorflow_installation():
    version_info = check_tensorflow_version()
    gpu_info = check_gpu_availability()
    test_result = test_basic_tensorflow_operations()
    print("\nTensorFlow installation verified successfully!")
    return {
        'version': version_info,
        'gpu_available': bool(gpu_info),
        'test_passed': True
    }


def configure_gpu_memory_growth():
    gpus = tf.config.list_physical_devices('GPU')
    if gpus:
        try:
            for gpu in gpus:
                tf.config.experimental.set_memory_growth(gpu, True)
            print("GPU memory growth enabled")
        except RuntimeError as e:
            print(f"Error setting GPU memory growth: {e}")
    return gpus


def set_mixed_precision():
    policy = tf.keras.mixed_precision.Policy('mixed_float16')
    tf.keras.mixed_precision.set_global_policy(policy)
    print(f"Mixed precision policy: {policy.name}")


def core_implementation():
    """Core TensorFlow setup implementation"""
    config = verify_tensorflow_installation()
    configure_gpu_memory_growth()
    return config


def banking_example():
    """Banking/Finance application - fraud detection model setup"""
    model = keras.Sequential([
        keras.layers.Dense(64, activation='relu', input_shape=(20,)),
        keras.layers.Dense(32, activation='relu'),
        keras.layers.Dense(1, activation='sigmoid')
    ])
    model.compile(
        optimizer='adam',
        loss='binary_crossentropy',
        metrics=['accuracy']
    )
    print("\nBanking fraud detection model created")
    print(model.summary())
    return model


def healthcare_example():
    """Healthcare application - patient diagnosis model setup"""
    model = keras.Sequential([
        keras.layers.Dense(128, activation='relu', input_shape=(50,)),
        keras.layers.Dropout(0.3),
        keras.layers.Dense(64, activation='relu'),
        keras.layers.Dense(32, activation='relu'),
        keras.layers.Dense(5, activation='softmax')
    ])
    model.compile(
        optimizer='adam',
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    print("\nHealthcare diagnosis model created")
    print(model.summary())
    return model


def display_system_info():
    print("=" * 60)
    print("TensorFlow System Information")
    print("=" * 60)
    print(f"\nPython version: {sys.version}")
    print(f"TensorFlow version: {tf.__version__}")
    print(f"NumPy version: {np.__version__}")
    
    import platform
    print(f"\nSystem: {platform.system()}")
    print(f"Processor: {platform.processor()}")
    print(f"Machine: {platform.machine()}")
    
    check_gpu_availability()


def test_training_loop():
    x_train = np.random.randn(1000, 10)
    y_train = np.random.randint(0, 2, size=(1000,))
    
    model = keras.Sequential([
        keras.layers.Dense(32, activation='relu', input_shape=(10,)),
        keras.layers.Dense(1, activation='sigmoid')
    ])
    model.compile(optimizer='adam', loss='binary_crossentropy')
    
    history = model.fit(x_train, y_train, epochs=5, batch_size=32, verbose=0)
    print(f"\nTraining completed - Final loss: {history.history['loss'][-1]:.4f}")
    return history


def main():
    print("Executing TensorFlow Installation and Setup implementation\n")
    display_system_info()
    print("\n" + "=" * 60)
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\n" + "=" * 60)
    test_training_loop()
    print("\n" + "=" * 60)
    print("All implementations completed successfully!")


if __name__ == "__main__":
    main()