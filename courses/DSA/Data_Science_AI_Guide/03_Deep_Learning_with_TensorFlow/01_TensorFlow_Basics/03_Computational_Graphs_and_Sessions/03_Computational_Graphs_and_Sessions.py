# Topic: Computational Graphs and Sessions
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Computational Graphs and Sessions

I. INTRODUCTION
   - Computational graphs represent operations as nodes
   - Sessions execute operations in the graph
   - Eager execution enables immediate evaluation

II. CORE_CONCEPTS
   - TensorFlow 1.x graphs and sessions
   - Eager execution (TensorFlow 2.x default)
   - Function tracing

III. IMPLEMENTATION
   - Graph construction
   - Session execution
   - tf.function for graph conversion
"""

import tensorflow as tf
from tensorflow import keras
import numpy as np


def eager_execution_demo():
    print("Eager Execution Demo (TensorFlow 2.x default)")
    a = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    b = tf.constant([[5.0, 6.0], [7.0, 8.0]])
    result = tf.matmul(a, b)
    print(f"Result:\n{result.numpy()}")
    return result


def graph_construction():
    @tf.function
    def matrix_multiply(x, y):
        return tf.matmul(x, y)
    
    a = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    b = tf.constant([[5.0, 6.0], [7.0, 8.0]])
    result = matrix_multiply(a, b)
    print(f"Graph execution result:\n{result.numpy()}")
    return result


def simple_neural_network_graph():
    @tf.function
    def neural_network_layer(x, weights, bias):
        return tf.nn.relu(tf.matmul(x, weights) + bias)
    
    x = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    weights = tf.constant([[0.5, 0.6], [0.7, 0.8]])
    bias = tf.constant([0.1, 0.2])
    
    result = neural_network_layer(x, weights, bias)
    print(f"Neural network layer output:\n{result.numpy()}")
    return result


def custom_gradient_graph():
    @tf.function
    def forward_pass(x, w, b):
        return tf.matmul(x, w) + b
    
    @tf.function
    def compute_gradients(x, y, w, b):
        with tf.GradientTape() as tape:
            predictions = forward_pass(x, w, b)
            loss = tf.reduce_mean(tf.square(y - predictions))
        gradients = tape.gradient(loss, [w, b])
        return loss, gradients
    
    x = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    y = tf.constant([[5.0], [6.0]])
    w = tf.Variable([[0.5], [0.5]])
    b = tf.Variable([0.0])
    
    loss, grads = compute_gradients(x, y, w, b)
    print(f"Loss: {loss.numpy()}")
    print(f"Weight gradient: {grads[0].numpy()}")
    print(f"Bias gradient: {grads[1].numpy()}")
    
    return {'loss': loss, 'gradients': grads}


def multi_operation_graph():
    @tf.function
    def complex_operation(x):
        a = tf.square(x)
        b = tf.sqrt(x)
        c = tf.add(a, b)
        d = tf.reduce_sum(c)
        return d
    
    x = tf.constant([1.0, 2.0, 3.0, 4.0])
    result = complex_operation(x)
    print(f"Complex operation result: {result.numpy()}")
    return result


def conditional_graph():
    @tf.function
    def conditional_op(x, training):
        if training:
            return x * 2
        else:
            return x / 2
    
    x = tf.constant([1.0, 2.0, 3.0])
    print(f"Training mode: {conditional_op(x, True).numpy()}")
    print(f"Inference mode: {conditional_op(x, False).numpy()}")
    
    return conditional_op


def loop_in_graph():
    @tf.function
    def loop_accumulate(n):
        accumulator = tf.Variable(0.0)
        for i in tf.range(n):
            accumulator.assign_add(tf.cast(i, tf.float32))
        return accumulator
    
    result = loop_accumulate(5)
    print(f"Loop result: {result.numpy()}")
    return result


def input_signature_example():
    @tf.function(input_signature=[
        tf.TensorSpec(shape=None, dtype=tf.float32),
        tf.TensorSpec(shape=None, dtype=tf.float32)
    ])
    def add_tensors(a, b):
        return a + b
    
    x = tf.constant([1.0, 2.0, 3.0])
    y = tf.constant([4.0, 5.0, 6.0])
    result = add_tensors(x, y)
    print(f"Input signature result: {result.numpy()}")
    return result


def autograph_example():
    @tf.function
    def while_loop_squares(n):
        result = tf.TensorArray(tf.float32, size=n)
        i = tf.Variable(0)
        while i < n:
            result = result.write(i, tf.cast(i ** 2, tf.float32))
            i.assign_add(1)
        return result.stack()
    
    result = while_loop_squares(5)
    print(f"While loop squares: {result.numpy()}")
    return result


def get_concrete_function():
    @tf.function
    def power(x, exponent):
        return x ** exponent
    
    concrete = power.get_concrete_function(
        x=tf.TensorSpec(shape=None, dtype=tf.float32),
        exponent=tf.TensorSpec(shape=None, dtype=tf.float32)
    )
    
    result = concrete(x=tf.constant(2.0), exponent=tf.constant(3.0))
    print(f"Concrete function result: {result.numpy()}")
    return concrete


def print_tracing_demo():
    @tf.function
    def traced_function(x):
        print(f"Tracing with x = {x}")
        return x * 2
    
    print("First call (tracing):")
    result1 = traced_function(tf.constant([1.0, 2.0]))
    print("Second call (no tracing):")
    result2 = traced_function(tf.constant([1.0, 2.0]))
    print(f"Results: {result1.numpy()}, {result2.numpy()}")
    return result1


def core_implementation():
    eager_execution_demo()
    print("\nGraph Construction:")
    graph_construction()
    print("\nSimple Neural Network Graph:")
    simple_neural_network_graph()
    print("\nCustom Gradient Graph:")
    custom_gradient_graph()
    print("\nMulti-operation Graph:")
    multi_operation_graph()
    return True


def banking_example():
    @tf.function
    def credit_risk_prediction(income, debt, credit_score):
        income_factor = income * 0.3
        debt_factor = debt * 0.2
        credit_factor = credit_score * 0.5
        return income_factor + debt_factor + credit_factor
    
    income = tf.constant([50000.0, 60000.0])
    debt = tf.constant([10000.0, 15000.0])
    credit_score = tf.constant([700.0, 650.0])
    
    risk_score = credit_risk_prediction(income, debt, credit_score)
    print(f"Banking credit risk prediction: {risk_score.numpy()}")
    return risk_score


def healthcare_example():
    @tf.function
    def triage_score(heart_rate, blood_pressure, temperature):
        hr_score = tf.where(heart_rate > 100, 2.0, 1.0)
        bp_score = tf.where(blood_pressure > 140, 2.0, 1.0)
        temp_score = tf.where(temperature > 99.5, 2.0, 1.0)
        return hr_score + bp_score + temp_score
    
    heart_rate = tf.constant([95.0, 110.0])
    blood_pressure = tf.constant([130.0, 145.0])
    temperature = tf.constant([98.6, 100.5])
    
    triage = triage_score(heart_rate, blood_pressure, temperature)
    print(f"Healthcare triage score: {triage.numpy()}")
    return triage


def main():
    print("Executing Computational Graphs and Sessions implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\n" + "=" * 60)
    print("All implementations completed!")


if __name__ == "__main__":
    main()