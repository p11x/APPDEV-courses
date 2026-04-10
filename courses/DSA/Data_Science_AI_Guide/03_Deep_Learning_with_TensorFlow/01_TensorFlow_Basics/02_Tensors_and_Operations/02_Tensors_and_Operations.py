# Topic: Tensors and Operations
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Tensors and Operations

I. INTRODUCTION
   - Tensors are the fundamental data structure in TensorFlow
   - Operations define computations on tensors
   - Understanding tensors is essential for deep learning

II. CORE_CONCEPTS
   - Tensor creation and manipulation
   - Element-wise operations
   - Matrix operations
   - Tensor broadcasting

III. IMPLEMENTATION
   - TensorFlow tensor operations
   - Mathematical functions
   - Shape manipulation
"""

import tensorflow as tf
from tensorflow import keras
import numpy as np


def create_tensors():
    scalar = tf.constant(5)
    vector = tf.constant([1.0, 2.0, 3.0])
    matrix = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    tensor_3d = tf.constant([[[1.0, 2.0], [3.0, 4.0]], [[5.0, 6.0], [7.0, 8.0]]])
    return {'scalar': scalar, 'vector': vector, 'matrix': matrix, 'tensor_3d': tensor_3d}


def tensor_properties(tensors):
    for name, tensor in tensors.items():
        print(f"{name}: shape={tensor.shape}, dtype={tensor.dtype}")


def element_wise_operations():
    a = tf.constant([1.0, 2.0, 3.0])
    b = tf.constant([4.0, 5.0, 6.0])
    
    addition = tf.add(a, b)
    subtraction = tf.subtract(a, b)
    multiplication = tf.multiply(a, b)
    division = tf.divide(a, b)
    
    print(f"Addition: {addition.numpy()}")
    print(f"Subtraction: {subtraction.numpy()}")
    print(f"Multiplication: {multiplication.numpy()}")
    print(f"Division: {division.numpy()}")
    
    return {'addition': addition, 'subtraction': subtraction, 
            'multiplication': multiplication, 'division': division}


def matrix_operations():
    a = tf.constant([[1.0, 2.0], [3.0, 4.0]])
    b = tf.constant([[5.0, 6.0], [7.0, 8.0]])
    
    matmul = tf.matmul(a, b)
    dot_product = tf.reduce_sum(a * b)
    
    print(f"Matrix multiplication:\n{matmul.numpy()}")
    print(f"Dot product: {dot_product.numpy()}")
    
    return {'matmul': matmul, 'dot_product': dot_product}


def tensor_reshaping():
    tensor = tf.constant(range(12))
    print(f"Original: {tensor.numpy()}")
    
    reshaped_2x6 = tf.reshape(tensor, [2, 6])
    reshaped_3x4 = tf.reshape(tensor, [3, 4])
    reshaped_2x2x3 = tf.reshape(tensor, [2, 2, 3])
    
    print(f"Reshaped to 2x6:\n{reshaped_2x6.numpy()}")
    print(f"Reshaped to 3x4:\n{reshaped_3x4.numpy()}")
    print(f"Reshaped to 2x2x3:\n{reshaped_2x2x3.numpy()}")
    
    return {'2x6': reshaped_2x6, '3x4': reshaped_3x4, '2x2x3': reshaped_2x2x3}


def tensor_slicing():
    matrix = tf.constant([[1, 2, 3, 4], [5, 6, 7, 8], [9, 10, 11, 12], [13, 14, 15, 16]])
    print(f"Original matrix:\n{matrix.numpy()}")
    
    first_row = matrix[0, :]
    first_col = matrix[:, 0]
    sub_matrix = matrix[0:2, 1:3]
    
    print(f"First row: {first_row.numpy()}")
    print(f"First column: {first_col.numpy()}")
    print(f"Sub-matrix [0:2, 1:3]:\n{sub_matrix.numpy()}")
    
    return {'first_row': first_row, 'first_col': first_col, 'sub_matrix': sub_matrix}


def broadcasting_example():
    a = tf.constant([[1.0], [2.0], [3.0]])
    b = tf.constant([10.0, 20.0, 30.0])
    
    result = a + b
    print(f"Shape of a: {a.shape}")
    print(f"Shape of b: {b.shape}")
    print(f"Broadcasting result:\n{result.numpy()}")
    
    return result


def math_functions():
    x = tf.constant([0.0, 0.5, 1.0])
    
    print(f"Sigmoid: {tf.nn.sigmoid(x).numpy()}")
    print(f"ReLU: {tf.nn.relu(x).numpy()}")
    print(f"Tanh: {tf.nn.tanh(x).numpy()}")
    print(f"Softmax: {tf.nn.softmax(tf.constant([1.0, 2.0, 3.0])).numpy()}")
    
    return {
        'sigmoid': tf.nn.sigmoid(x),
        'relu': tf.nn.relu(x),
        'tanh': tf.nn.tanh(x)
    }


def tensor_concatenation():
    a = tf.constant([[1, 2], [3, 4]])
    b = tf.constant([[5, 6], [7, 8]])
    
    concat_axis_0 = tf.concat([a, b], axis=0)
    concat_axis_1 = tf.concat([a, b], axis=1)
    
    print(f"Concatenate axis 0:\n{concat_axis_0.numpy()}")
    print(f"Concatenate axis 1:\n{concat_axis_1.numpy()}")
    
    return {'axis_0': concat_axis_0, 'axis_1': concat_axis_1}


def tensor_stacking():
    a = tf.constant([1, 2, 3])
    b = tf.constant([4, 5, 6])
    
    stack_axis_0 = tf.stack([a, b], axis=0)
    stack_axis_1 = tf.stack([a, b], axis=1)
    
    print(f"Stack axis 0:\n{stack_axis_0.numpy()}")
    print(f"Stack axis 1:\n{stack_axis_1.numpy()}")
    
    return {'axis_0': stack_axis_0, 'axis_1': stack_axis_1}


def padding_and_clipping():
    tensor = tf.constant([[1, 2, 3], [4, 5, 6]])
    
    padded = tf.pad(tensor, [[1, 1], [1, 1]], constant_values=0)
    print(f"Padded tensor:\n{padded.numpy()}")
    
    clipped = tf.clip_by_value(tensor, 2, 5)
    print(f"Clipped tensor (2-5):\n{clipped.numpy()}")
    
    return {'padded': padded, 'clipped': clipped}


def gather_and_scatter():
    params = tf.constant([10, 20, 30, 40, 50])
    indices = tf.constant([0, 2, 4])
    
    gathered = tf.gather(params, indices)
    print(f"Gathered indices [0, 2, 4]: {gathered.numpy()}")
    
    updates = tf.constant([100, 200])
    scatter = tf.scatter_nd(tf.constant([1, 3]), updates, shape=[5])
    print(f"Scatter to indices [1, 3]: {scatter.numpy()}")
    
    return {'gathered': gathered, 'scatter': scatter}


def core_implementation():
    tensors = create_tensors()
    tensor_properties(tensors)
    element_wise_operations()
    matrix_operations()
    return tensors


def banking_example():
    transaction_features = tf.constant([
        [100.0, 1.0, 0.0],
        [50.0, 0.0, 1.0],
        [200.0, 1.0, 0.0]
    ])
    weights = tf.constant([0.5, 0.3, 0.2])
    
    risk_scores = tf.matmul(transaction_features, tf.reshape(weights, [3, 1]))
    print(f"Banking transaction risk scores:\n{risk_scores.numpy()}")
    
    return risk_scores


def healthcare_example():
    patient_data = tf.constant([
        [72.0, 120.0, 98.6],
        [68.0, 130.0, 97.8],
        [75.0, 115.0, 99.1]
    ])
    feature_weights = tf.constant([0.4, 0.4, 0.2])
    
    health_scores = tf.matmul(patient_data, tf.reshape(feature_weights, [3, 1]))
    print(f"Healthcare health scores:\n{health_scores.numpy()}")
    
    return health_scores


def main():
    print("Executing Tensors and Operations implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    print("Tensor Reshaping:")
    tensor_reshaping()
    print("\n" + "=" * 60)
    print("Tensor Slicing:")
    tensor_slicing()
    print("\n" + "=" * 60)
    print("Broadcasting:")
    broadcasting_example()
    print("\n" + "=" * 60)
    print("Math Functions:")
    math_functions()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()