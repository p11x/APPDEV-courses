# Topic: Backpropagation and Optimization
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Backpropagation and Optimization

I. INTRODUCTION
   - Backpropagation computes gradients for weight updates
   - Optimization algorithms minimize loss functions
   - Foundation of neural network training

II. CORE_CONCEPTS
   - Chain rule and gradient computation
   - Forward and backward passes
   - Loss functions
   - Optimizers (SGD, Adam, etc.)

III. IMPLEMENTATION
   - Manual backpropagation
   - Gradient descent variants
   - Training loop implementation
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models, optimizers
import numpy as np


def manual_backpropagation():
    X = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    y = tf.constant([[1.0], [0.0]])
    
    w1 = tf.Variable(tf.random.normal([2, 4], stddev=0.5))
    b1 = tf.Variable(tf.zeros([4]))
    w2 = tf.Variable(tf.random.normal([4, 1], stddev=0.5))
    b2 = tf.Variable(tf.zeros([1]))
    
    learning_rate = 0.1
    
    for epoch in range(10):
        with tf.GradientTape() as tape:
            hidden = tf.nn.relu(tf.matmul(X, w1) + b1)
            output = tf.nn.sigmoid(tf.matmul(hidden, w2) + b2)
            loss = tf.reduce_mean(tf.square(y - output))
        
        gradients = tape.gradient(loss, [w1, b1, w2, b2])
        w1.assign_sub(learning_rate * gradients[0])
        b1.assign_sub(learning_rate * gradients[1])
        w2.assign_sub(learning_rate * gradients[2])
        b2.assign_sub(learning_rate * gradients[3])
        
        if epoch % 2 == 0:
            print(f"Epoch {epoch}: Loss = {loss.numpy():.4f}")
    
    return w1, w2


def sgd_optimizer():
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    optimizer = optimizers.SGD(learning_rate=0.01, momentum=0.9)
    model.compile(optimizer=optimizer, loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"SGD - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def adam_optimizer():
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Adam - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def rmsprop_optimizer():
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='rmsprop', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"RMSprop - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def adamax_optimizer():
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adamax', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Adamax - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def nadam_optimizer():
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='nadam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"NAdam - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def gradient_clipping():
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    optimizer = optimizers.Adam(learning_rate=0.01, clipnorm=1.0)
    model.compile(optimizer=optimizer, loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Gradient Clipping - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def learning_rate_schedules():
    lr_schedule = optimizers.schedules.ExponentialDecay(
        initial_learning_rate=0.1,
        decay_steps=10,
        decay_rate=0.9
    )
    
    optimizer = optimizers.SGD(learning_rate=lr_schedule)
    
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    model.compile(optimizer=optimizer, loss='binary_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=10, verbose=0)
    print(f"LR Schedule - Final loss: {history.history['loss'][-1]:.4f}")
    return model


def custom_training_loop():
    X = tf.random.normal([200, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    optimizer = optimizers.Adam()
    loss_fn = keras.losses.BinaryCrossentropy()
    
    losses = []
    for epoch in range(10):
        with tf.GradientTape() as tape:
            predictions = model(X, training=True)
            loss = loss_fn(y, predictions)
        
        gradients = tape.gradient(loss, model.trainable_weights)
        optimizer.apply_gradients(zip(gradients, model.trainable_weights))
        losses.append(loss.numpy())
    
    print(f"Custom Training - Final loss: {losses[-1]:.4f}")
    return model, losses


def compute_gradients():
    model = models.Sequential([
        layers.Dense(4, activation='relu', input_shape=(2,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    X = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    y = tf.constant([[1.0], [0.0]])
    
    with tf.GradientTape() as tape:
        predictions = model(X)
        loss = keras.losses.binary_crossentropy(y, predictions)
    
    gradients = tape.gradient(loss, model.trainable_weights)
    
    print(f"Number of gradient tensors: {len(gradients)}")
    for i, g in enumerate(gradients):
        if g is not None:
            print(f"  Gradient {i}: shape = {g.shape}")
    return gradients


def loss_functions():
    y_true = tf.constant([[1.0], [0.0], [1.0]])
    y_pred = tf.constant([[0.9], [0.1], [0.8])
    
    mse = keras.losses.MeanSquaredError()(y_true, y_pred)
    bce = keras.losses.BinaryCrossentropy()(y_true, y_pred)
    cce = keras.losses.CategoricalCrossentropy()(
        tf.constant([[1.0, 0.0], [0.0, 1.0]]),
        tf.constant([[0.8, 0.2], [0.3, 0.7]])
    )
    
    print(f"MSE: {mse.numpy():.4f}")
    print(f"BCE: {bce.numpy():.4f}")
    print(f"CCE: {cce.numpy():.4f}")
    return {'mse': mse, 'bce': bce, 'cse': cce}


def optimizer_comparison():
    X = tf.random.normal([300, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    optimizers_to_test = {
        'SGD': 'sgd',
        'Adam': 'adam',
        'RMSprop': 'rmsprop',
        'Adadelta': 'adadelta',
        'Adamax': 'adamax'
    }
    
    results = {}
    for name, opt_name in optimizers_to_test.items():
        model = models.Sequential([
            layers.Dense(16, activation='relu', input_shape=(10,)),
            layers.Dense(1, activation='sigmoid')
        ])
        model.compile(optimizer=opt_name, loss='binary_crossentropy')
        history = model.fit(X, y, epochs=10, verbose=0)
        results[name] = history.history['loss'][-1]
    
    for name, loss in results.items():
        print(f"{name}: {loss:.4f}")
    return results


def core_implementation():
    print("Manual Backpropagation:")
    manual_backpropagation()
    print("\nLoss Functions:")
    loss_functions()
    print("\nGradient Computation:")
    compute_gradients()
    print("\nOptimizer Comparison:")
    optimizer_comparison()
    return True


def banking_example():
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(20,)),
        layers.Dropout(0.3),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(
        optimizer=optimizers.Adam(learning_rate=0.001),
        loss='binary_crossentropy',
        metrics=['accuracy', keras.metrics.Precision(), keras.metrics.Recall()]
    )
    
    X = tf.random.normal([1000, 20])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0.3, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Banking - Val loss: {history.history['val_loss'][-1]:.4f}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.Dense(128, activation='relu', input_shape=(50,)),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Dense(64, activation='relu'),
        layers.BatchNormalization(),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(
        optimizer=optimizers.Adam(learning_rate=0.001),
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    X = tf.random.normal([1000, 50])
    y_raw = tf.cast(tf.reduce_mean(X, axis=1) * 4, tf.int32)
    y = tf.one_hot(y_raw, depth=5)
    
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    print(f"Healthcare - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Backpropagation and Optimization implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()