# Topic: Perceptrons and Activation Functions
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Perceptrons and Activation Functions

I. INTRODUCTION
   - Perceptron is the fundamental unit of neural networks
   - Activation functions introduce non-linearity
   - Essential for learning complex patterns

II. CORE_CONCEPTS
   - Single perceptron architecture
   - Activation functions (sigmoid, tanh, ReLU, etc.)
   - Gradient computation
   - Binary classification

III. IMPLEMENTATION
   - Perceptron class implementation
   - Various activation functions
   - Training loop
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np


class SimplePerceptron:
    def __init__(self, input_dim):
        self.weights = tf.Variable(tf.random.normal([input_dim, 1], stddev=0.5))
        self.bias = tf.Variable(tf.zeros([1]))
    
    def forward(self, x):
        z = tf.matmul(x, self.weights) + self.bias
        return tf.nn.sigmoid(z)
    
    def predict(self, x):
        return tf.cast(self.forward(x) > 0.5, tf.float32)


def create_perceptron(input_dim, output_dim):
    weights = tf.Variable(tf.random.normal([input_dim, output_dim], stddev=0.3))
    bias = tf.Variable(tf.zeros([output_dim]))
    return weights, bias


def sigmoid_activation():
    x = tf.constant([-2.0, -1.0, 0.0, 1.0, 2.0])
    y = tf.nn.sigmoid(x)
    print(f"Sigmoid: {y.numpy()}")
    return y


def tanh_activation():
    x = tf.constant([-2.0, -1.0, 0.0, 1.0, 2.0])
    y = tf.nn.tanh(x)
    print(f"Tanh: {y.numpy()}")
    return y


def relu_activation():
    x = tf.constant([-2.0, -1.0, 0.0, 1.0, 2.0])
    y = tf.nn.relu(x)
    print(f"ReLU: {y.numpy()}")
    return y


def leaky_relu_activation():
    x = tf.constant([-2.0, -1.0, 0.0, 1.0, 2.0])
    y = tf.nn.leaky_relu(x, alpha=0.2)
    print(f"Leaky ReLU: {y.numpy()}")
    return y


def elu_activation():
    x = tf.constant([-2.0, -1.0, 0.0, 1.0, 2.0])
    y = tf.nn.elu(x)
    print(f"ELU: {y.numpy()}")
    return y


def softmax_activation():
    x = tf.constant([1.0, 2.0, 3.0])
    y = tf.nn.softmax(x)
    print(f"Softmax: {y.numpy()}")
    print(f"Sum: {tf.reduce_sum(y).numpy()}")
    return y


def swish_activation():
    x = tf.constant([-2.0, -1.0, 0.0, 1.0, 2.0])
    y = tf.nn.swish(x)
    print(f"Swish: {y.numpy()}")
    return y


def perceptron_training():
    X = tf.constant([[0, 0], [0, 1], [1, 0], [1, 1]], dtype=tf.float32)
    y = tf.constant([[0], [1], [1], [1]], dtype=tf.float32)
    
    weights = tf.Variable(tf.random.normal([2, 1], stddev=0.5))
    bias = tf.Variable(tf.zeros([1]))
    
    learning_rate = 0.5
    
    for epoch in range(100):
        with tf.GradientTape() as tape:
            predictions = tf.nn.sigmoid(tf.matmul(X, weights) + bias)
            loss = tf.reduce_mean(tf.square(y - predictions))
        
        gradients = tape.gradient(loss, [weights, bias])
        weights.assign_sub(learning_rate * gradients[0])
        bias.assign_sub(learning_rate * gradients[1])
    
    final_predictions = tf.nn.sigmoid(tf.matmul(X, weights) + bias)
    print(f"Final predictions: {final_predictions.numpy().flatten()}")
    print(f"Actual: {y.numpy().flatten()}")
    return weights, bias


def multi_class_perceptron():
    X = tf.constant([[1, 2], [2, 1], [-1, -2], [-2, -1]], dtype=tf.float32)
    y = tf.constant([0, 0, 1, 1], dtype=tf.int32)
    y_onehot = tf.one_hot(y, depth=2)
    
    weights = tf.Variable(tf.random.normal([2, 2], stddev=0.5))
    bias = tf.Variable(tf.zeros([2]))
    
    learning_rate = 0.5
    
    for epoch in range(100):
        with tf.GradientTape() as tape:
            logits = tf.matmul(X, weights) + bias
            predictions = tf.nn.softmax(logits)
            loss = tf.reduce_mean(tf.nn.softmax_cross_entropy_with_logits(y_onehot, logits))
        
        gradients = tape.gradient(loss, [weights, bias])
        weights.assign_sub(learning_rate * gradients[0])
        bias.assign_sub(learning_rate * gradients[1])
    
    final_logits = tf.matmul(X, weights) + bias
    final_predictions = tf.argmax(final_logits, axis=1)
    print(f"Final predictions: {final_predictions.numpy()}")
    print(f"Actual: {y.numpy()}")
    return weights, bias


def keras_activation_layers():
    model = keras.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(16, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Keras model with activation functions:")
    model.summary()
    return model


def custom_activation():
    def custom_swish(x):
        return x * tf.nn.sigmoid(x)
    
    layer = layers.Dense(32, activation=custom_swish)
    x = tf.ones((1, 10))
    print(f"Custom activation output shape: {layer(x).shape}")
    return layer


def batch_normalization_effect():
    model_with_bn = keras.Sequential([
        layers.Dense(32, input_shape=(10,)),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Dense(1)
    ])
    
    model_without_bn = keras.Sequential([
        layers.Dense(32, input_shape=(10,)),
        layers.ReLU(),
        layers.Dense(1)
    ])
    
    print("With BatchNorm:")
    model_with_bn.summary()
    print("\nWithout BatchNorm:")
    model_without_bn.summary()
    return model_with_bn, model_without_bn


def activation_derivatives():
    x = tf.Variable(1.0)
    
    with tf.GradientTape() as tape:
        y = tf.nn.sigmoid(x)
    grad_sigmoid = tape.gradient(y, x)
    
    with tf.GradientTape() as tape:
        y = tf.nn.tanh(x)
    grad_tanh = tape.gradient(y, x)
    
    with tf.GradientTape() as tape:
        y = tf.nn.relu(x)
    grad_relu = tape.gradient(y, x)
    
    print(f"Sigmoid gradient at x=1: {grad_sigmoid.numpy()}")
    print(f"Tanh gradient at x=1: {grad_tanh.numpy()}")
    print(f"ReLU gradient at x=1: {grad_relu.numpy()}")
    return grad_sigmoid, grad_tanh, grad_relu


def core_implementation():
    print("Creating perceptron:")
    p = SimplePerceptron(2)
    print(f"Weights shape: {p.weights.shape}")
    print(f"Bias shape: {p.bias.shape}")
    
    print("\nActivation Functions:")
    sigmoid_activation()
    tanh_activation()
    relu_activation()
    leaky_relu_activation()
    elu_activation()
    softmax_activation()
    swish_activation()
    
    print("\nPerceptron Training:")
    perceptron_training()
    
    print("\nActivation Derivatives:")
    activation_derivatives()
    return True


def banking_example():
    X = tf.random.normal([1000, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = keras.Sequential([
        layers.Dense(16, activation='relu', input_shape=(10,)),
        layers.Dense(8, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    history = model.fit(X, y, epochs=10, verbose=0)
    
    print(f"Banking fraud detection - Final accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    X = tf.random.normal([1000, 20])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0.5, tf.int32)
    y = tf.one_hot(y, depth=4)
    
    model = keras.Sequential([
        layers.Dense(64, activation='relu', input_shape=(20,)),
        layers.Dropout(0.3),
        layers.Dense(32, activation='relu'),
        layers.Dense(4, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    history = model.fit(X, y, epochs=10, verbose=0)
    
    print(f"Healthcare diagnosis - Final accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Perceptrons and Activation Functions implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()