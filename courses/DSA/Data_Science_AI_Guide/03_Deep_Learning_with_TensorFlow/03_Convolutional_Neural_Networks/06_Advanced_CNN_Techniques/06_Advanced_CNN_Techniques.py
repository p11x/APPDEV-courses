# Topic: Advanced CNN Techniques
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Advanced CNN Techniques

I. INTRODUCTION
   - Advanced CNN techniques push the boundaries of image recognition
   - Modern architectures with attention, normalization, and skip connections
   - State-of-the-art performance on benchmarks

II. CORE_CONCEPTS
   - Inception modules
   - Residual connections
   - Squeeze-and-excitation
   - Attention mechanisms

III. IMPLEMENTATION
   - Inception networks
   - SE blocks
   - Dense connections
   - Modern architectures
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def inception_module(x, filters_1x1, filters_3x3_reduce, filters_3x3, filters_5x5_reduce, filters_5x5, pool_proj):
    conv_1x1 = layers.Conv2D(filters_1x1, (1, 1), padding='same', activation='relu')(x)
    
    conv_3x3_reduce = layers.Conv2D(filters_3x3_reduce, (1, 1), padding='same', activation='relu')(x)
    conv_3x3 = layers.Conv2D(filters_3x3, (3, 3), padding='same', activation='relu')(conv_3x3_reduce)
    
    conv_5x5_reduce = layers.Conv2D(filters_5x5_reduce, (1, 1), padding='same', activation='relu')(x)
    conv_5x5 = layers.Conv2D(filters_5x5, (5, 5), padding='same', activation='relu')(conv_5x5_reduce)
    
    pool_proj = layers.MaxPooling2D((3, 3), strides=(1, 1), padding='same')(x)
    pool_proj = layers.Conv2D(pool_proj, (1, 1), padding='same', activation='relu')(pool_proj)
    
    output = layers.Concatenate()([conv_1x1, conv_3x3, conv_5x5, pool_proj])
    return output


def se_block(x, channels, reduction=16):
    squeeze = layers.GlobalAveragePooling2D()(x)
    squeeze = layers.Dense(channels // reduction, activation='relu')(squeeze)
    squeeze = layers.Dense(channels, activation='sigmoid')(squeeze)
    squeeze = layers.Reshape((1, 1, channels))(squeeze)
    return layers.Multiply()([x, squeeze])


def dense_block(x, num_layers, growth_rate):
    concat_feat = [x]
    for i in range(num_layers):
        x = layers.Conv2D(4 * growth_rate, (1, 1), padding='same', activation='relu')(x)
        x = layers.Conv2D(growth_rate, (3, 3), padding='same', activation='relu')(x)
        concat_feat.append(x)
        x = layers.Concatenate()(concat_feat)
    return x


def transition_block(x, num_channels):
    x = layers.BatchNormalization()(x)
    x = layers.Activation('relu')(x)
    x = layers.Conv2D(num_channels, (1, 1), padding='same', activation='relu')(x)
    x = layers.AveragePooling2D((2, 2), strides=(2, 2))(x)
    return x


def inception_network():
    inputs = keras.Input(shape=(224, 224, 3))
    
    x = layers.Conv2D(64, (7, 7), strides=(2, 2), padding='same', activation='relu')(inputs)
    x = layers.MaxPooling2D((3, 3), strides=(2, 2), padding='same')(x)
    x = layers.Conv2D(64, (1, 1), padding='same', activation='relu')(x)
    x = layers.Conv2D(192, (3, 3), padding='same', activation='relu')(x)
    x = layers.MaxPooling2D((3, 3), strides=(2, 2), padding='same')(x)
    
    x = inception_module(x, 64, 96, 128, 16, 32, 32)
    x = inception_module(x, 128, 128, 192, 32, 96, 64)
    x = layers.MaxPooling2D((3, 3), strides=(2, 2), padding='same')(x)
    
    x = inception_module(x, 192, 96, 208, 16, 48, 64)
    x = inception_module(x, 160, 112, 224, 24, 64, 64)
    x = inception_module(x, 128, 128, 256, 24, 64, 64)
    x = inception_module(x, 112, 144, 288, 32, 64, 64)
    x = inception_module(x, 256, 160, 320, 32, 128, 128)
    
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dropout(0.4)(x)
    x = layers.Dense(1000, activation='softmax')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    print("Inception Network:")
    model.summary()
    return model


def senet():
    inputs = keras.Input(shape=(224, 224, 3))
    
    x = layers.Conv2D(64, (7, 7), strides=(2, 2), padding='same', activation='relu')(inputs)
    x = layers.MaxPooling2D((3, 3), strides=(2, 2), padding='same')(x)
    
    x = layers.Conv2D(64, (1, 1), padding='same', activation='relu')(x)
    x = layers.Conv2D(64, (3, 3), padding='same', activation='relu')(x)
    x = se_block(x, 64, reduction=16)
    
    x = layers.Conv2D(128, (1, 1), padding='same', activation='relu')(x)
    x = layers.Conv2D(128, (3, 3), padding='same', activation='relu')(x)
    x = se_block(x, 128, reduction=16)
    
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(1000, activation='softmax')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    print("SE-Net:")
    model.summary()
    return model


def densenet():
    inputs = keras.Input(shape=(224, 224, 3))
    
    x = layers.Conv2D(64, (7, 7), strides=(2, 2), padding='same', activation='relu')(inputs)
    x = layers.MaxPooling2D((3, 3), strides=(2, 2), padding='same')(x)
    
    x = dense_block(x, num_layers=4, growth_rate=32)
    x = transition_block(x, num_channels=128)
    
    x = dense_block(x, num_layers=4, growth_rate=32)
    x = transition_block(x, num_channels=128)
    
    x = layers.BatchNormalization()(x)
    x = layers.Activation('relu')(x)
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(1000, activation='softmax')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    print("DenseNet:")
    model.summary()
    return model


def mobilenet_v2():
    inputs = keras.Input(shape=(224, 224, 3))
    
    x = layers.Conv2D(32, (3, 3), strides=(2, 2), padding='same', activation='relu')(inputs)
    
    x = layers.DepthwiseConv2D((3, 3), padding='same', activation='relu')(x)
    x = layers.Conv2D(16, (1, 1), padding='same', activation='relu')(x)
    
    x = layers.DepthwiseConv2D((3, 3), strides=(2, 2), padding='same', activation='relu')(x)
    x = layers.Conv2D(24, (1, 1), padding='same', activation='relu')(x)
    
    x = layers.DepthwiseConv2D((3, 3), padding='same', activation='relu')(x)
    x = layers.Conv2D(24, (1, 1), padding='same', activation='relu')(x)
    
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(1000, activation='softmax')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    print("MobileNetV2:")
    model.summary()
    return model


def custom_advanced_cnn():
    inputs = keras.Input(shape=(64, 64, 3))
    
    x = layers.Conv2D(32, (3, 3), padding='same', activation='relu')(inputs)
    x = layers.BatchNormalization()(x)
    x = layers.Conv2D(32, (3, 3), padding='same', activation='relu')(x)
    x = layers.BatchNormalization()(x)
    x = se_block(x, 32, reduction=8)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(64, (3, 3), padding='same', activation='relu')(x)
    x = layers.BatchNormalization()(x)
    x = layers.Conv2D(64, (3, 3), padding='same', activation='relu')(x)
    x = layers.BatchNormalization()(x)
    x = se_block(x, 64, reduction=8)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(128, (3, 3), padding='same', activation='relu')(x)
    x = layers.BatchNormalization()(x)
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(256, activation='relu')(x)
    x = layers.Dropout(0.5)(x)
    x = layers.Dense(10, activation='softmax')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    print("Custom Advanced CNN:")
    model.summary()
    return model


def train_advanced_cnn():
    X = tf.random.normal([50, 64, 64, 3])
    y = tf.random.uniform([50], minval=0, maxval=10, dtype=tf.int32)
    
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(64, 64, 3)),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.GlobalAveragePooling2D(),
        layers.Dense(10, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Training - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def core_implementation():
    print("Inception Network:")
    inception_network()
    print("\nSE-Net:")
    senet()
    print("\nDenseNet:")
    densenet()
    print("\nCustom Advanced CNN:")
    custom_advanced_cnn()
    print("\nTraining Advanced CNN:")
    train_advanced_cnn()
    return True


def banking_example():
    inputs = keras.Input(shape=(64, 64, 1))
    
    x = layers.Conv2D(32, (3, 3), padding='same', activation='relu')(inputs)
    x = layers.BatchNormalization()(x)
    x = se_block(x, 32, reduction=8)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(64, (3, 3), padding='same', activation='relu')(x)
    x = layers.BatchNormalization()(x)
    x = se_block(x, 64, reduction=8)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(128, (3, 3), padding='same', activation='relu')(x)
    x = layers.BatchNormalization()(x)
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(64, activation='relu')(x)
    x = layers.Dropout(0.4)(x)
    x = layers.Dense(1, activation='sigmoid')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([50, 64, 64, 1])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Banking - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    inputs = keras.Input(shape=(128, 128, 3))
    
    x = layers.Conv2D(32, (3, 3), padding='same', activation='relu')(inputs)
    x = layers.BatchNormalization()(x)
    x = se_block(x, 32, reduction=8)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(64, (3, 3), padding='same', activation='relu')(x)
    x = layers.BatchNormalization()(x)
    x = se_block(x, 64, reduction=8)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(128, (3, 3), padding='same', activation='relu')(x)
    x = layers.BatchNormalization()(x)
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(256, activation='relu')(x)
    x = layers.Dropout(0.4)(x)
    x = layers.Dense(5, activation='softmax')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([50, 128, 128, 3])
    y = tf.random.uniform([50], minval=0, maxval=5, dtype=tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Healthcare - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Advanced CNN Techniques implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()