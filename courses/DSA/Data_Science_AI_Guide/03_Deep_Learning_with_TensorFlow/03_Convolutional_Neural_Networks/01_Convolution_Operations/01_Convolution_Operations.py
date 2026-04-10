# Topic: Convolution Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Convolution Operations

I. INTRODUCTION
   - Convolution is the core operation in CNNs
   - Extracts features from input images
   - Mathematical operation: element-wise multiplication and sum

II. CORE_CONCEPTS
   - Conv1D, Conv2D, Conv3D
   - Kernel/filters
   - Stride and padding
   - Feature maps

III. IMPLEMENTATION
   - Convolution layers
   - Custom kernels
   - Feature extraction
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def conv2d_basic():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(28, 28, 1)),
        layers.Flatten(),
        layers.Dense(10)
    ])
    print("Basic Conv2D:")
    model.summary()
    return model


def conv1d_example():
    model = models.Sequential([
        layers.Conv1D(32, 3, activation='relu', input_shape=(100, 1)),
        layers.GlobalAveragePooling1D(),
        layers.Dense(10)
    ])
    print("Conv1D:")
    model.summary()
    return model


def conv3d_example():
    model = models.Sequential([
        layers.Conv3D(32, (3, 3, 3), activation='relu', input_shape=(16, 16, 16, 1)),
        layers.Flatten(),
        layers.Dense(10)
    ])
    print("Conv3D:")
    model.summary()
    return model


def different_kernel_sizes():
    model = models.Sequential([
        layers.Conv2D(16, (1, 1), activation='relu', input_shape=(28, 28, 1), name='1x1'),
        layers.Conv2D(16, (3, 3), activation='relu', name='3x3'),
        layers.Conv2D(16, (5, 5), activation='relu', name='5x5'),
    ])
    print("Different Kernel Sizes:")
    for layer in model.layers:
        print(f"  {layer.name}: {layer.kernel.size}")
    return model


def stride_dilation():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), strides=(2, 2), input_shape=(28, 28, 1)),
    ])
    print("Stride (2,2):")
    print(f"  Output shape: {model.output_shape}")
    return model


def padding_same():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), padding='same', input_shape=(28, 28, 1)),
    ])
    print("Padding 'same':")
    print(f"  Output shape: {model.output_shape}")
    return model


def padding_valid():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), padding='valid', input_shape=(28, 28, 1)),
    ])
    print("Padding 'valid':")
    print(f"  Output shape: {model.output_shape}")
    return model


def manual_conv2d():
    image = tf.constant(tf.random.normal([1, 5, 5, 1]), dtype=tf.float32)
    kernel = tf.constant(tf.ones([3, 3, 1, 1]), dtype=tf.float32)
    
    result = tf.nn.conv2d(image, kernel, strides=[1, 1, 1, 1], padding='VALID')
    print(f"Manual Conv2D:")
    print(f"  Input shape: {image.shape}")
    print(f"  Kernel shape: {kernel.shape}")
    print(f"  Output shape: {result.shape}")
    return result


def depthwise_conv2d():
    model = models.Sequential([
        layers.DepthwiseConv2D((3, 3), input_shape=(28, 28, 3)),
    ])
    print("DepthwiseConv2D:")
    print(f"  Output shape: {model.output_shape}")
    return model


def separable_conv2d():
    model = models.Sequential([
        layers.SeparableConv2D(32, (3, 3), input_shape=(28, 28, 1)),
    ])
    print("SeparableConv2D:")
    print(f"  Output shape: {model.output_shape}")
    return model


def conv_feature_extraction():
    base_model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(28, 28, 1)),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),
    ])
    
    _ = base_model(tf.ones([1, 28, 28, 1]))
    
    print("Feature extraction:")
    for i, layer in enumerate(base_model.layers):
        print(f"  Layer {i}: {layer.name}, output shape: {layer.output_shape}")
    return base_model


def multiple_filters():
    model = models.Sequential([
        layers.Conv2D(64, (3, 3), activation='relu', input_shape=(28, 28, 1)),
    ])
    print(f"Multiple filters (64):")
    print(f"  Total parameters: {model.count_params()}")
    return model


def dilate_convolution():
    model = models.Sequential([
        layers.Conv2D(16, (3, 3), dilation_rate=(2, 2), input_shape=(28, 28, 1)),
    ])
    print("Dilated convolution (dilation_rate=2):")
    print(f"  Output shape: {model.output_shape}")
    return model


def apply_multiple_filters():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), padding='same', input_shape=(28, 28, 1)),
    ])
    
    test_image = tf.ones([1, 28, 28, 1])
    output = model(test_image)
    
    print(f"Apply multiple filters:")
    print(f"  Input: {test_image.shape}")
    print(f"  Output: {output.shape}")
    print(f"  Number of feature maps: {output.shape[-1]}")
    return output


def core_implementation():
    print("Conv2D Basic:")
    conv2d_basic()
    print("\nConv1D:")
    conv1d_example()
    print("\nDifferent Kernel Sizes:")
    different_kernel_sizes()
    print("\nPadding:")
    padding_same()
    padding_valid()
    print("\nFeature Extraction:")
    conv_feature_extraction()
    return True


def banking_example():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(64, 64, 1)),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(128, (3, 3), activation='relu'),
        layers.Flatten(),
        layers.Dense(64, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([100, 64, 64, 1])
    y = tf.cast(tf.reduce_mean(X, axis=(1, 2, 3)) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Banking - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(64, 64, 3)),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(128, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        layers.Flatten(),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([100, 64, 64, 3])
    y = tf.cast(tf.reduce_mean(X, axis=(1, 2, 3)) * 4, tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Healthcare - Final accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Convolution Operations implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()