# Topic: Variables and State Management
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Variables and State Management

I. INTRODUCTION
   - Variables store mutable tensor values
   - Essential for model parameters
   - State management in training loops

II. CORE_CONCEPTS
   - Variable creation and initialization
   - Assign operations
   - Trackable objects
   - Model checkpoints

III. IMPLEMENTATION
   - Variable operations
   - Checkpoint saving/restoring
   - Training state management
"""

import tensorflow as tf
from tensorflow import keras
import numpy as np
import os


def create_variables():
    scalar_var = tf.Variable(3.0, trainable=True)
    vector_var = tf.Variable([1.0, 2.0, 3.0], trainable=True)
    matrix_var = tf.Variable([[1.0, 2.0], [3.0, 4.0]], trainable=True)
    
    print(f"Scalar variable: {scalar_var.numpy()}")
    print(f"Vector variable: {vector_var.numpy()}")
    print(f"Matrix variable:\n{matrix_var.numpy()}")
    
    return {'scalar': scalar_var, 'vector': vector_var, 'matrix': matrix_var}


def variable_assign():
    var = tf.Variable([1.0, 2.0, 3.0])
    print(f"Initial: {var.numpy()}")
    
    var.assign([4.0, 5.0, 6.0])
    print(f"After assign: {var.numpy()}")
    
    var.assign_add([1.0, 1.0, 1.0])
    print(f"After assign_add: {var.numpy()}")
    
    var.assign_sub([0.5, 0.5, 0.5])
    print(f"After assign_sub: {var.numpy()}")
    
    return var


def variable_scatter():
    var = tf.Variable([0.0, 0.0, 0.0, 0.0, 0.0])
    indices = [0, 2, 4]
    updates = [1.0, 2.0, 3.0]
    
    var.scatter_update(tf.constant(indices), tf.constant(updates))
    print(f"After scatter_update: {var.numpy()}")
    
    var.scatter_add(tf.constant(indices), tf.constant([1.0, 1.0, 1.0]))
    print(f"After scatter_add: {var.numpy()}")
    
    return var


def variable_weights():
    weights = tf.Variable(tf.keras.initializers.GlorotUniform()(shape=(10, 5)))
    print(f"Weights shape: {weights.shape}")
    print(f"Weights initialized:\n{weights.numpy()[:3, :]}")
    return weights


def trainable_vs_non_trainable():
    trainable_var = tf.Variable([1.0, 2.0], trainable=True)
    non_trainable_var = tf.Variable([3.0, 4.0], trainable=False)
    
    print(f"Trainable: {trainable_var.trainable}")
    print(f"Non-trainable: {non_trainable_var.trainable}")
    return {'trainable': trainable_var, 'non_trainable': non_trainable_var}


def checkpoint_variables():
    var1 = tf.Variable([1.0, 2.0, 3.0], name='var1')
    var2 = tf.Variable([[1.0, 2.0], [3.0, 4.0]], name='var2')
    checkpoint = tf.train.Checkpoint(var1=var1, var2=var2)
    
    return checkpoint


def save_checkpoint(checkpoint, path='./checkpoint'):
    save_path = checkpoint.save(path)
    print(f"Checkpoint saved to: {save_path}")
    return save_path


def load_checkpoint(checkpoint, path='./checkpoint'):
    status = checkpoint.restore(path)
    print(f"Checkpoint restored from: {path}")
    print(f"var1: {checkpoint.var1.numpy()}")
    print(f"var2: {checkpoint.var2.numpy()}")
    return status


def variable_scope_example():
    with tf.variable_scope('layer1'):
        w1 = tf.Variable(tf.random.normal([3, 3]), name='weights')
        b1 = tf.Variable(tf.zeros([3]), name='biases')
    
    with tf.variable_scope('layer2'):
        w2 = tf.Variable(tf.random.normal([3, 2]), name='weights')
        b2 = tf.Variable(tf.zeros([2]), name='biases')
    
    print(f"Layer 1 weights shape: {w1.shape}")
    print(f"Layer 1 biases shape: {b1.shape}")
    print(f"Layer 2 weights shape: {w2.shape}")
    print(f"Layer 2 biases shape: {b2.shape}")
    
    return {'layer1': (w1, b1), 'layer2': (w2, b2)}


def model_variable_tracking():
    class SimpleModel(keras.Model):
        def __init__(self):
            super().__init__()
            self.dense1 = keras.layers.Dense(32, activation='relu')
            self.dense2 = keras.layers.Dense(1)
        
        def call(self, x):
            return self.dense2(self.dense1(x))
    
    model = SimpleModel()
    _ = model(tf.ones((1, 10)))
    
    print("Model variables:")
    for var in model.variables:
        print(f"  {var.name}: {var.shape}")
    return model


def gradient_descent_manual():
    x = tf.Variable(3.0)
    y = tf.Variable(0.0)
    
    learning_rate = 0.1
    
    for i in range(10):
        with tf.GradientTape() as tape:
            f = y + x ** 2
        
        gradients = tape.gradient(f, [y, x])
        x.assign_sub(learning_rate * gradients[0])
        y.assign_sub(learning_rate * gradients[1])
        print(f"Step {i}: x={x.numpy():.4f}, y={y.numpy():.4f}")
    
    return x, y


def momentum_optimizer():
    var = tf.Variable([1.0, 2.0, 3.0])
    optimizer = keras.optimizers.SGD(momentum=0.9)
    
    for i in range(5):
        with tf.GradientTape() as tape:
            loss = tf.reduce_sum(var ** 2)
        
        gradients = [2.0 * var]
        optimizer.apply_gradients(zip(gradients, [var]))
        print(f"Step {i}: {var.numpy()}")
    
    return var


def moving_average():
    var = tf.Variable([1.0, 2.0, 3.0])
    ema = tf.train.ExponentialMovingAverage(decay=0.9)
    
    update_op = ema.apply([var])
    
    with tf.control_dependencies([update_op]):
        ema_value = ema.average(var)
    
    print(f"Original: {var.numpy()}")
    print(f"EMA: {ema_value.numpy()}")
    return ema_value


def get_local_variables():
    with tf.variable_scope('scope1'):
        v1 = tf.Variable([1.0], name='v1')
    
    with tf.variable_scope('scope2'):
        v2 = tf.Variable([2.0], name='v2')
    
    all_vars = tf.compat.v1.trainable_variables()
    print(f"All trainable variables: {len(all_vars)}")
    for v in all_vars:
        print(f"  {v.name}: {v.numpy()}")
    return all_vars


def core_implementation():
    create_variables()
    print("\nVariable Assign:")
    variable_assign()
    print("\nVariable Scatter:")
    variable_scatter()
    print("\nVariable Weights:")
    variable_weights()
    return True


def banking_example():
    account_balance = tf.Variable(1000.0)
    interest_rate = tf.Variable(0.05)
    
    def apply_interest():
        account_balance.assign(account_balance * (1 + interest_rate))
        return account_balance
    
    for month in range(6):
        apply_interest()
        print(f"Month {month}: ${account_balance.numpy():.2f}")
    
    return account_balance


def healthcare_example():
    patient_vitals = tf.Variable([72.0, 120.0, 98.6])
    baseline = tf.Variable([72.0, 120.0, 98.6])
    
    def check_anomaly():
        diff = patient_vitals - baseline
        anomaly_score = tf.reduce_mean(tf.abs(diff))
        return anomaly_score
    
    patient_vitals.assign([85.0, 130.0, 99.1])
    score = check_anomaly()
    print(f"Anomaly score: {score.numpy():.2f}")
    
    return patient_vitals


def main():
    print("Executing Variables and State Management implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\n" + "=" * 60)
    gradient_descent_manual()
    print("\n" + "=" * 60)
    momentum_optimizer()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()