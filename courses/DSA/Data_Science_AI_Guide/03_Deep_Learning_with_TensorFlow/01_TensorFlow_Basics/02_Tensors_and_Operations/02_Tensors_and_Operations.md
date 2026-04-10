# Tensors and Operations

## I. INTRODUCTION

### What are Tensors and Operations?
Tensors are the fundamental data structures in TensorFlow, representing multi-dimensional arrays of numbers. They are the building blocks upon which all deep learning models are constructed. In mathematical terms, a tensor is a generalization of scalars, vectors, and matrices to potentially higher dimensions.

The word "tensor" comes from the Latin "tendere" meaning "to stretch," reflecting how tensors can stretch or expand across multiple dimensions. In the context of TensorFlow, tensors flow through a computational graph, transforming as they pass through various operations.

Operations (ops) are the mathematical functions that manipulate tensors. These include basic arithmetic operations (add, multiply, divide), matrix operations (matmul, dot), activation functions (relu, sigmoid, softmax), and many specialized operations for neural networks (convolution, pooling, etc.).

### Why are they Important in Deep Learning?
Tensors and operations form the foundation upon which all deep learning models are built:

1. **Data Representation**: Every piece of data in deep learning - images, text, audio, numerical features - is represented as tensors. Images are 4D tensors (batch, height, width, channels), text can be tokenized into 2D tensors (sequence, embedding dimension), and numerical features are organized as 2D or 3D tensors.

2. **Computational Efficiency**: TensorFlow's tensor operations are optimized for parallel execution on GPUs and TPUs, enabling fast computation of complex neural network architectures.

3. **Automatic Differentiation**: TensorFlow tracks tensor operations to compute gradients automatically via the GradientTape, enabling efficient backpropagation for training neural networks.

4. **Memory Efficiency**: Tensors provide a unified memory layout optimized for neural network computations, reducing memory transfers and improving cache utilization.

### Prerequisites
- Basic understanding of linear algebra (vectors, matrices)
- Familiarity with Python programming
- Understanding of NumPy arrays (helpful but not required)
- Concept of computational graphs (introduced in this module)

## II. FUNDAMENTALS

### TensorRank and Shape

The rank (or order) of a tensor refers to its number of dimensions:

| Rank | Mathematical Term | Example | Shape |
|-----|------------------|---------|-------|
| 0 | Scalar | 5.0 | () |
| 1 | Vector | [1, 2, 3] | (3,) |
| 2 | Matrix | [[1,2],[3,4]] | (2, 2) |
| 3 | 3-Tensor | Cube of data | (d, h, w) |
| 4 | 4-Tensor | Batch of images | (batch, h, w, channels) |

### TensorFlow Data Types

TensorFlow supports various data types:

```python
# Common data types
tf.float32    # 32-bit floating point (default)
tf.float64    # 64-bit floating point
tf.int8      # 8-bit signed integer
tf.int16     # 16-bit signed integer
tf.int32     # 32-bit signed integer (default for ints)
tf.int64     # 64-bit signed integer
tf.uint8     # 8-bit unsigned integer
tf.bool      # Boolean
tf.string    # String (variable length)
tf.complex64 # 64-bit complex number
```

### Core Tensor Operations

1. **Creation Operations**: Create tensors from constants or distributions
2. **Shape Operations**: Manipulate tensor shapes (reshape, transpose, squeeze)
3. **Indexing and Slicing**: Extract portions of tensors
4. **Math Operations**: Element-wise and matrix operations
5. **Reduction Operations**: Sum, mean, max across axes

## III. IMPLEMENTATION

### Creating Tensors

```python
"""
TensorFlow Tensors and Operations - Module 2
This script demonstrates tensor creation and basic operations.
"""

import tensorflow as tf
import numpy as np

class TensorCreation:
    """
    Demonstration of various tensor creation methods in TensorFlow.
    """
    
    @staticmethod
    def create_scalar():
        """Create a scalar tensor (rank 0)."""
        scalar = tf.constant(42)
        print(f"Scalar: {scalar.numpy()}, Rank: {tf.rank(scalar).numpy()}")
        return scalar
    
    @staticmethod
    def create_vector():
        """Create a vector tensor (rank 1)."""
        # Method 1: From Python list
        vector1 = tf.constant([1, 2, 3, 4, 5])
        
        # Method 2: From NumPy array
        vector2 = tf.constant(np.array([1, 2, 3, 4, 5]))
        
        # Method 3: Using tf.range
        vector3 = tf.range(start=0, limit=10, delta=2)
        
        print(f"Vector from list: {vector1.numpy()}")
        print(f"Vector from NumPy: {vector2.numpy()}")
        print(f"Vector using range: {vector3.numpy()}")
        
        return vector1
    
    @staticmethod
    def create_matrix():
        """Create a matrix tensor (rank 2)."""
        # 2D matrix
        matrix = tf.constant([
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9]
        ], dtype=tf.float32)
        
        print(f"Matrix shape: {matrix.shape}")
        print(f"Matrix:\n{matrix.numpy()}")
        
        return matrix
    
    @staticmethod
    def create_3d_tensor():
        """Create a 3D tensor (rank 3)."""
        # 3D tensor (e.g., batch of sequences)
        tensor_3d = tf.constant([
            [[1, 2, 3], [4, 5, 6]],
            [[7, 8, 9], [10, 11, 12]],
            [[13, 14, 15], [16, 17, 18]]
        ], dtype=tf.float32)
        
        print(f"3D Tensor shape: {tensor_3d.shape}")
        print(f"3D Tensor:\n{tensor_3d.numpy()}")
        
        return tensor_3d
    
    @staticmethod
    def create_4d_tensor():
        """Create a 4D tensor (rank 4) - e.g., batch of images."""
        # 4D tensor: (batch, height, width, channels)
        # Simulating a batch of 2 RGB images (3x3 pixels)
        images = tf.constant([
            # Image 1 (Red channel high)
            [[[255, 0, 0], [255, 0, 0], [255, 0, 0]],
            [[255, 0, 0], [255, 0, 0], [255, 0, 0]],
            [[255, 0, 0], [255, 0, 0], [255, 0, 0]],
            # Image 2 (Green channel high)
            [[[0, 255, 0], [0, 255, 0], [0, 255, 0]],
            [[0, 255, 0], [0, 255, 0], [0, 255, 0]],
            [[0, 255, 0], [0, 255, 0], [0, 255, 0]]
        ], dtype=tf.float32)
        
        images = tf.divide(images, 255.0)  # Normalize to [0, 1]
        
        print(f"Images tensor shape: {images.shape}")
        
        return images
    
    @staticmethod
    def create_special_tensors():
        """Create special tensors like zeros, ones, identity."""
        # Zeros
        zeros = tf.zeros(shape=(3, 3), dtype=tf.float32)
        
        # Ones
        ones = tf.ones(shape=(3, 3), dtype=tf.float32)
        
        # Identity matrix
        identity = tf.eye(num_rows=3, num_columns=3)
        
        # Fill with constant
        filled = tf.fill(value=7.0, shape=(2, 2))
        
        # Range with linspace
        spaced = tf.linspace(start=0.0, stop=1.0, num=5)
        
        print(f"Zeros:\n{zeros.numpy()}")
        print(f"Ones:\n{ones.numpy()}")
        print(f"Identity:\n{identity.numpy()}")
        print(f"Filled with 7:\n{filled.numpy()}")
        print(f"Linspace [0,1]: {spaced.numpy()}")
        
        return zeros, ones, identity, filled
    
    @staticmethod
    def create_random_tensors():
        """Create tensors with random values."""
        # Set seed for reproducibility
        tf.random.set_seed(42)
        np.random.seed(42)
        
        # Uniform distribution [0, 1)
        uniform = tf.random.uniform(shape=(3, 3), minval=0, maxval=1)
        
        # Normal distribution (Gaussian)
        normal = tf.random.normal(shape=(3, 3), mean=0.0, stddev=1.0)
        
        # Truncated normal (within 2 stddev)
        truncated = tf.random.truncated_normal(shape=(3, 3), mean=0.0, stddev=1.0)
        
        # Random categorical (for one-hot encoding)
        categorical = tf.random.categorical(
            logits=tf.constant([[0.5, 0.5, 0.5]]), 
            num_samples=5
        )
        
        print(f"Uniform [0,1):\n{uniform.numpy()}")
        print(f"Normal (mean=0, std=1):\n{normal.numpy()}")
        print(f"Truncated normal:\n{truncated.numpy()}")
        print(f"Categorical samples: {categorical.numpy()}")
        
        return uniform, normal, truncated

def demonstrate_tensor_creation():
    """Demonstrate all tensor creation methods."""
    print("="*60)
    print("TENSOR CREATION METHODS")
    print("="*60)
    
    creator = TensorCreation()
    
    print("\n--- Scalar ---")
    creator.create_scalar()
    
    print("\n--- Vector ---")
    creator.create_vector()
    
    print("\n--- Matrix ---")
    creator.create_matrix()
    
    print("\n--- 3D Tensor ---")
    creator.create_3d_tensor()
    
    print("\n--- 4D Tensor (Images) ---")
    creator.create_4d_tensor()
    
    print("\n--- Special Tensors ---")
    creator.create_special_tensors()
    
    print("\n--- Random Tensors ---")
    creator.create_random_tensors()

class TensorOperations:
    """
    Demonstration of basic tensor operations.
    """
    
    @staticmethod
    def element_wise_operations():
        """Element-wise arithmetic operations."""
        a = tf.constant([1, 2, 3, 4], dtype=tf.float32)
        b = tf.constant([4, 3, 2, 1], dtype=tf.float32)
        
        # Addition
        c_add = tf.add(a, b)
        print(f"Add: {a.numpy()} + {b.numpy()} = {c_add.numpy()}")
        
        # Subtraction
        c_sub = tf.subtract(a, b)
        print(f"Subtract: {a.numpy()} - {b.numpy()} = {c_sub.numpy()}")
        
        # Multiplication
        c_mul = tf.multiply(a, b)
        print(f"Multiply: {a.numpy()} * {b.numpy()} = {c_mul.numpy()}")
        
        # Division
        c_div = tf.divide(a, b)
        print(f"Divide: {a.numpy()} / {b.numpy()} = {c_div.numpy()}")
        
        # Power
        c_pow = tf.pow(a, 2)
        print(f"Power: {a.numpy()} ^ 2 = {c_pow.numpy()}")
        
        # Modulo
        c_mod = tf.mod(a, b)
        print(f"Modulo: {a.numpy()} % {b.numpy()} = {c_mod.numpy()}")
        
        return c_add, c_sub, c_mul
    
    @staticmethod
    def matrix_operations():
        """Matrix operations."""
        # Matrix A (2x3)
        A = tf.constant([[1, 2, 3], [4, 5, 6]], dtype=tf.float32)
        
        # Matrix B (2x3)
        B = tf.constant([[7, 8, 9], [10, 11, 12]], dtype=tf.float32)
        
        # Matrix multiplication requires inner dimensions to match
        # A (2x3) @ B (3x2) = (2x2)
        # Let's create proper matrices for multiplication
        A_sq = tf.constant([[1, 2], [3, 4]], dtype=tf.float32)
        B_sq = tf.constant([[5, 6], [7, 8]], dtype=tf.float32)
        
        # Matrix multiplication
        matmul = tf.matmul(A_sq, B_sq)
        print(f"Matrix A:\n{A_sq.numpy()}")
        print(f"Matrix B:\n{B_sq.numpy()}")
        print(f"A @ B:\n{matmul.numpy()}")
        
        # Transpose
        transpose = tf.transpose(A_sq)
        print(f"Transpose of A:\n{transpose.numpy()}")
        
        # Determinant
        det = tf.linalg.det(tf.cast(A_sq, tf.float32))
        print(f"Determinant of A: {det.numpy()}")
        
        # Inverse
        # Need to ensure matrix is invertible
        invertible = tf.constant([[4, 7], [2, 6]], dtype=tf.float32)
        inv = tf.linalg.inv(invertible)
        print(f"Inverse of [[4,7],[2,6]]:\n{inv.numpy()}")
        
        return matmul
    
    @staticmethod
    def shape_operations():
        """Operations that change tensor shapes."""
        # Original tensor
        tensor = tf.constant([[1, 2, 3], [4, 5, 6]], dtype=tf.float32)
        print(f"Original: {tensor.shape} = {tensor.numpy()}")
        
        # Reshape (flatten to 1D)
        flat = tf.reshape(tensor, [6])
        print(f"Flattened: {flat.shape} = {flat.numpy()}")
        
        # Reshape to 3D
        tensor_3d = tf.reshape(tensor, [1, 2, 3])
        print(f"Reshaped to 3D: {tensor_3d.shape}")
        
        # Transpose
        transposed = tf.transpose(tensor)
        print(f"Transposed: {transposed.shape} = {transposed.numpy()}")
        
        # Squeeze (remove dimensions of size 1)
        squeezed = tf.squeeze(tensor_3d)
        print(f"Squeezed: {squeezed.shape} = {squeezed.numpy()}")
        
        # Expand dimensions
        expanded = tf.expand_dims(squeezed, axis=0)
        print(f"Expanded: {expanded.shape}")
        
        # Tile (repeat tensor)
        tiled = tf.tile(tf.constant([[1, 2]]), [3, 1])
        print(f"Tiled [[1,2]] x 3: {tiled.numpy()}")
        
        return flat, transposed, expanded
    
    @staticmethod
    def indexing_operations():
        """Indexing and slicing operations."""
        # Create a 2D tensor
        tensor = tf.constant([
            [1, 2, 3, 4],
            [5, 6, 7, 8],
            [9, 10, 11, 12]
        ], dtype=tf.float32)
        
        print(f"Original tensor:\n{tensor.numpy()}")
        
        # Direct indexing
        first_row = tensor[0]
        print(f"First row [0]: {first_row.numpy()}")
        
        # Element at position
        element = tensor[1, 2]
        print(f"Element at [1,2]: {element.numpy()}")
        
        # Slice rows
        rows_0_1 = tensor[0:2]
        print(f"Rows 0-1 [0:2]:\n{rows_0_1.numpy()}")
        
        # Slice specific rows and columns
        sub = tensor[0:2, 1:3]
        print(f"Sub-matrix [0:2,1:3]:\n{sub.numpy()}")
        
        # Using tf.slice
        sliced = tf.slice(tensor, [1, 1], [2, 2])
        print(f"tf.slice([1,1], [2,2]):\n{sliced.numpy()}")
        
        # Gather (non-contiguous selection)
        indices = [0, 2]
        gathered = tf.gather(tensor, indices, axis=0)
        print(f"Gather rows [0,2]:\n{gathered.numpy()}")
        
        # Boolean mask
        condition = tf.constant([True, False, True])
        masked = tf.boolean_mask(tensor, condition)
        print(f"Boolean mask [T,F,T]:\n{masked.numpy()}")
        
        return first_row, element, sub
    
    @staticmethod
    def reduction_operations():
        """Reduction operations (sum, mean, etc. across axes)."""
        # Create a 2D tensor
        tensor = tf.constant([
            [1, 2, 3, 4],
            [5, 6, 7, 8],
            [9, 10, 11, 12]
        ], dtype=tf.float32)
        
        print(f"Tensor:\n{tensor.numpy()}")
        
        # Sum
        total_sum = tf.reduce_sum(tensor)
        print(f"Sum (all): {total_sum.numpy()}")
        
        # Sum along axis
        sum_axis_0 = tf.reduce_sum(tensor, axis=0)
        print(f"Sum axis=0: {sum_axis_0.numpy()}")
        
        sum_axis_1 = tf.reduce_sum(tensor, axis=1)
        print(f"Sum axis=1: {sum_axis_1.numpy()}")
        
        # Mean
        mean_all = tf.reduce_mean(tensor)
        print(f"Mean (all): {mean_all.numpy()}")
        
        mean_axis_0 = tf.reduce_mean(tensor, axis=0)
        print(f"Mean axis=0: {mean_axis_0.numpy()}")
        
        # Max/Min
        max_val = tf.reduce_max(tensor)
        min_val = tf.reduce_min(tensor)
        print(f"Max: {max_val.numpy()}, Min: {min_val.numpy()}")
        
        # Argmax/Argmin
        argmax_axis = tf.argmax(tensor, axis=1)
        argmin_axis = tf.argmin(tensor, axis=0)
        print(f"Argmax axis=1: {argmax_axis.numpy()}")
        print(f"Argmin axis=0: {argmin_axis.numpy()}")
        
        return total_sum, mean_all, max_val

def demonstrate_operations():
    """Demonstrate all tensor operations."""
    print("\n" + "="*60)
    print("TENSOR OPERATIONS")
    print("="*60)
    
    ops = TensorOperations()
    
    print("\n--- Element-wise Operations ---")
    ops.element_wise_operations()
    
    print("\n--- Matrix Operations ---")
    ops.matrix_operations()
    
    print("\n--- Shape Operations ---")
    ops.shape_operations()
    
    print("\n--- Indexing Operations ---")
    ops.indexing_operations()
    
    print("\n--- Reduction Operations ---")
    ops.reduction_operations()

class TensorMathFunctions:
    """
    Mathematical functions for tensors.
    """
    
    @staticmethod
    def activation_functions():
        """Common activation functions."""
        # Test values
        x = tf.linspace(-5.0, 5.0, 11)
        
        # ReLU (Rectified Linear Unit) - max(0, x)
        relu = tf.nn.relu(x)
        
        # Sigmoid - 1 / (1 + exp(-x))
        sigmoid = tf.nn.sigmoid(x)
        
        # Softmax (usually for last layer)
        logits = tf.constant([1.0, 2.0, 3.0])
        softmax = tf.nn.softmax(logits)
        
        # Tanh
        tanh = tf.nn.tanh(x)
        
        # Leaky ReLU
        leaky = tf.nn.leaky_relu(x, alpha=0.2)
        
        print(f"x: {x.numpy()}")
        print(f"ReLU: {relu.numpy()}")
        print(f"Sigmoid: {sigmoid.numpy()}")
        print(f"Softmax([1,2,3]): {softmax.numpy()}")
        print(f"Tanh: {tanh.numpy()}")
        print(f"Leaky ReLU: {leaky.numpy()}")
        
        return relu, sigmoid, softmax
    
    @staticmethod
    def neural_network_ops():
        """Operations commonly used in neural networks."""
        # Softmax cross-entropy loss
        # Used for multi-class classification
        logits = tf.constant([[2.0, 1.0, 0.1],
                             [1.0, 3.0, 0.2]])
        labels = tf.constant([[1.0, 0.0, 0.0],
                            [0.0, 1.0, 0.0]])
        
        ce_loss = tf.nn.softmax_cross_entropy_with_logits(
            labels=labels, logits=logits
        )
        print(f"Cross-entropy loss: {ce_loss.numpy()}")
        
        # Sparse softmax cross-entropy (for integer labels)
        sparse_labels = tf.constant([0, 1])
        sparse_ce = tf.nn.sparse_softmax_cross_entropy_with_logits(
            labels=sparse_labels, logits=logits
        )
        print(f"Sparse CE loss: {sparse_ce.numpy()}")
        
        # Sigmoid cross-entropy (for binary classification)
        binary_labels = tf.constant([[1.0], [0.0]])
        binary_logits = tf.constant([[3.2], [-1.5]])
        sig_ce = tf.nn.sigmoid_cross_entropy_with_logits(
            labels=binary_labels, logits=binary_logits
        )
        print(f"Sigmoid CE: {sig_ce.numpy()}")
        
        # L2 loss
        predictions = tf.constant([1.0, 2.0, 3.0])
        targets = tf.constant([1.5, 2.5, 3.5])
        l2 = tf.nn.l2_loss(predictions - targets)
        print(f"L2 loss: {l2.numpy()}")
        
        return ce_loss, sparse_ce
    
    @staticmethod
    def normalization_ops():
        """Normalization operations."""
        # Batch normalization statistics
        # For training: compute mean/variance
        # For inference: use moving averages
        
        # Create test data
        x = tf.random.normal(shape=(4, 8, 8, 3))  # NHWC format
        
        # Compute moments for batch norm
        axes = [0, 1, 2]  # Spatial axes
        mean, variance = tf.nn.moments(x, axes)
        
        print(f"Input shape: {x.shape}")
        print(f"Mean shape: {mean.shape}, Variance shape: {variance.shape}")
        print(f"Mean: {mean.numpy()[:3]}...")
        print(f"Variance: {variance.numpy()[:3]}...")
        
        # Layer normalization (normalize across features)
        # Good for RNNs
        x_rnn = tf.random.normal(shape=(4, 8, 16))  # (batch, seq, features)
        ln_mean, ln_var = tf.nn.moments(x_rnn, axis=2, keepdims=True)
        normalized = (x_rnn - ln_mean) / tf.sqrt(ln_var + 1e-5)
        
        print(f"\nLayer norm input: {x_rnn.shape}")
        print(f"Normalized shape: {normalized.shape}")

def demonstrate_math_functions():
    """Demonstrate mathematical functions."""
    print("\n" + "="*60)
    print("MATHEMATICAL FUNCTIONS")
    print("="*60)
    
    funcs = TensorMathFunctions()
    
    print("\n--- Activation Functions ---")
    funcs.activation_functions()
    
    print("\n--- Neural Network Operations ---")
    funcs.neural_network_ops()
    
    print("\n--- Normalization Operations ---")
    funcs.normalization_ops()

class TensorFlowTape:
    """
    Automatic differentiation with GradientTape.
    """
    
    @staticmethod
    def basic_gradients():
        """Compute gradients using GradientTape."""
        # Simple example: y = x^2, compute dy/dx at x=3
        x = tf.Variable(3.0)
        
        with tf.GradientTape() as tape:
            y = x ** 2
        
        # Compute gradient
        grad = tape.gradient(y, x)
        print(f"y = x^2 at x=3")
        print(f"dy/dx = {grad.numpy()}")  # Should be 2*3 = 6
        
        # Multiple variables
        w = tf.Variable(2.0)
        b = tf.Variable(1.0)
        
        with tf.GradientTape() as tape:
            y = w * 5 + b
        
        # Gradients for both variables
        grad_w, grad_b = tape.gradient(y, [w, b])
        print(f"\ny = w*5 + b at w=2, b=1")
        print(f"dy/dw = {grad_w.numpy()}")  # Should be 5
        print(f"dy/db = {grad_b.numpy()}")  # Should be 1
        
        return grad
    
    @staticmethod
    def higher_order_gradients():
        """Compute higher-order gradients (second derivatives)."""
        x = tf.Variable(2.0)
        
        with tf.GradientTape() as tape1:
            with tf.GradientTape() as tape2:
                y = x ** 3
            
            # First derivative: dy/dx = 3x^2 = 12
            first_grad = tape2.gradient(y, x)
        
        # Second derivative: d²y/dx² = 6x = 12
        second_grad = tape1.gradient(first_grad, x)
        
        print(f"y = x^3 at x=2")
        print(f"First derivative (dy/dx): {first_grad.numpy()}")  # 12
        print(f"Second derivative (d²y/dx²): {second_grad.numpy()}")  # 12
        
        return first_grad, second_grad
    
    @staticmethod
    def jacobian_hessian():
        """Compute Jacobians and Hessians."""
        # Jacobian: first derivatives of vector-valued functions
        x = tf.Variable([1.0, 2.0, 3.0])
        
        def vector_function(x):
            return x ** 2  # [1, 4, 9]
        
        with tf.GradientTape() as tape:
            y = vector_function(x)
        
        jacobian = tape.jacobian(y, x)
        print(f"Jacobian of [x1², x2², x3²]:\n{jacobian.numpy()}")
        
        # Hessian: second derivatives (for quadratic optimization)
        # For simple quadratic functions
        def scalar_function(x):
            return tf.reduce_sum(x ** 2)
        
        with tf.GradientTape() as tape1:
            with tf.GradientTape() as tape2:
                loss = scalar_function(x)
            
            grad = tape2.gradient(loss, x)
        
        hessian = tape1.jacobian(grad, x)
        print(f"\nHessian of sum(x²):\n{hessian.numpy()}")

def demonstrate_gradient_tape():
    """Demonstrate automatic differentiation."""
    print("\n" + "="*60)
    print("AUTOMATIC DIFFERENTIATION")
    print("="*60)
    
    tape = TensorFlowTape()
    
    print("\n--- Basic Gradients ---")
    tape.basic_gradients()
    
    print("\n--- Higher-order Gradients ---")
    tape.higher_order_gradients()
    
    print("\n--- Jacobian and Hessian ---")
    tape.jacobian_hessian()

if __name__ == "__main__":
    demonstrate_tensor_creation()
    demonstrate_operations()
    demonstrate_math_functions()
    demonstrate_gradient_tape()
```

### Standard Example: Image Processing Pipeline

```python
"""
Standard Example: Image Processing with Tensors
Demonstrates real-world tensor operations for image processing.
"""

import tensorflow as tf
import numpy as np
import matplotlib.pyplot as plt

class ImagePipeline:
    """
    Complete image processing pipeline using TensorFlow tensor operations.
    """
    
    def __init__(self):
        self.target_size = (128, 128)
        
    def load_and_preprocess_image(self, image_data):
        """
        Load and preprocess an image for neural network input.
        
        Steps:
        1. Convert to tensor
        2. Resize
        3. Normalize
        4. Expand dimensions for batch
        """
        # Convert to tensor
        if not isinstance(image_data, tf.Tensor):
            image = tf.convert_to_tensor(image_data, dtype=tf.float32)
        else:
            image = tf.cast(image_data, tf.float32)
        
        # Resize using bilinear interpolation
        image = tf.image.resize(
            image, 
            self.target_size,
            method='bilinear'
        )
        
        # Normalize to [0, 1]
        image = image / 255.0
        
        # Add batch dimension
        image = tf.expand_dims(image, axis=0)
        
        return image
    
    def augment_image(self, image):
        """
        Data augmentation with tensor operations.
        """
        # Random flip
        if tf.random.uniform([]) > 0.5:
            image = tf.image.flip_left_right(image)
        
        # Random brightness
        delta = tf.random.uniform([], -0.2, 0.2)
        image = tf.image.adjust_brightness(image, delta)
        
        # Random contrast
        contrast_factor = tf.random.uniform([], 0.8, 1.2)
        image = tf.image.adjust_contrast(image, contrast_factor)
        
        # Random rotation (90 degree increments)
        k = tf.random.uniform([1], 0, 4, dtype=tf.int32)[0]
        image = tf.image.rot90(image, k=k)
        
        return image
    
    def preprocess_batch(self, images):
        """
        Preprocess a batch of images.
        """
        # Convert list to tensor
        images = tf.stack([self.load_and_preprocess_image(img) for img in images])
        return images
    
    def denormalize_image(self, image):
        """
        Convert from [-1, 1] or [0, 1] to [0, 255] for display.
        """
        # If normalized to [-1, 1]
        if tf.reduce_min(image) < 0:
            image = (image + 1.0) / 2.0
        
        # Clamp to [0, 1]
        image = tf.clip_by_value(image, 0.0, 1.0)
        
        # Convert to uint8
        image = tf.cast(image * 255, tf.uint8)
        
        return image

def generate_synthetic_images():
    """
    Generate synthetic images for demonstration.
    """
    # Create simple gradient images
    images = []
    labels = []
    
    for i in range(10):
        # Create gradient image
        img = np.zeros((64, 64, 3), dtype=np.float32)
        
        # Horizontal gradient
        for x in range(64):
            img[:, x, :] = x / 64
        
        images.append(img)
        labels.append(i % 2)
    
    return np.array(images), np.array(labels)

def run_image_pipeline_example():
    """Run the image processing example."""
    print("="*60)
    print("IMAGE PROCESSING PIPELINE EXAMPLE")
    print("="*60)
    
    # Generate synthetic images
    images, labels = generate_synthetic_images()
    
    print(f"Generated {len(images)} images")
    print(f"Image shape: {images[0].shape}")
    print(f"Labels: {labels}")
    
    # Create pipeline
    pipeline = ImagePipeline()
    
    # Preprocess all images
    processed = []
    for img in images:
        processed_img = pipeline.load_and_preprocess_image(img)
        processed.append(processed_img)
    
    processed = tf.concat(processed, axis=0)
    
    print(f"\nProcessed batch shape: {processed.shape}")
    print(f"Value range: [{tf.reduce_min(processed):.2f}, {tf.reduce_max(processed):.2f}]")
    
    # Apply augmentation
    augmented = pipeline.augment_image(processed[0:1])
    print(f"Augmented image shape: {augmented.shape}")
    
    print("\nImage pipeline complete!")
```

### Real-world Example 1: Banking/Finance - Transaction Data Processing

```python
"""
Real-world Example 1: Banking/Finance - Financial Transaction Feature Engineering
This example demonstrates tensor operations for processing financial transaction data.
"""

import tensorflow as tf
import numpy as np

class TransactionFeatureEngineering:
    """
    Feature engineering pipeline for financial transaction data.
    
    Features computed:
    - Amount statistics (rolling mean, std)
    - Time-based features (hour, day, weekend flag)
    - Behavioral patterns (velocity, deviation from norm)
    - Risk indicators (unusual amounts, suspicious patterns)
    """
    
    def __init__(self, window_size=7):
        self.window_size = window_size
        
    def load_transactions(self, n_transactions=1000):
        """
        Load synthetic transaction data.
        Each transaction: [amount, hour, day_of_week, merchant_category]
        """
        np.random.seed(42)
        
        # Generate realistic transaction amounts (log-normal distributed)
        amounts = np.random.lognormal(mean=4, sigma=1, size=n_transactions)
        
        # Generate timestamps (weighted for typical transaction times)
        hours = np.random.choice(
            np.arange(24), 
            size=n_transactions, 
            p=[0.02, 0.01, 0.01, 0.01, 0.02, 0.05, 
               0.10, 0.15, 0.10, 0.05, 0.05, 0.05,
               0.05, 0.05, 0.05, 0.05, 0.05, 0.08,
               0.10, 0.08, 0.05, 0.03, 0.02, 0.02]
        )
        
        # Days of week (0=Monday, 6=Sunday)
        days = np.random.randint(0, 7, size=n_transactions)
        
        # Merchant categories (0=grocery, 1=restaurant, 2=retail, 3=gas, 4=online)
        merchants = np.random.randint(0, 5, size=n_transactions)
        
        # Stack into feature matrix
        transactions = tf.constant(
            np.stack([amounts, hours, days, merchants], axis=1),
            dtype=tf.float32
        )
        
        return transactions
    
    def compute_rolling_features(self, amounts):
        """
        Compute rolling statistics for transaction amounts.
        """
        # Convert to 2D for rolling (sequence_length, 1)
        amounts_2d = tf.expand_dims(amounts, axis=-1)
        
        # Simple moving average using tf.map_fn (in production, use tf.data window)
        def compute_window_mean(idx):
            start_idx = tf.maximum(0, idx - self.window_size)
            window = amounts_2d[start_idx:idx+1]
            return tf.reduce_mean(window)
        
        indices = tf.range(tf.shape(amounts_2d)[0])
        rolling_means = tf.map_fn(compute_window_mean, indices, fn_output_signature=tf.float32)
        
        return rolling_means
    
    def compute_temporal_features(self, hours, days):
        """
        Compute time-based features.
        """
        # Hour features (cyclical encoding)
        hour_sin = tf.sin(2 * np.pi * tf.cast(hours, tf.float32) / 24)
        hour_cos = tf.cos(2 * np.pi * tf.cast(hours, tf.float32) / 24)
        
        # Day features (cyclical encoding)
        day_sin = tf.sin(2 * np.pi * tf.cast(days, tf.float32) / 7)
        day_cos = tf.cos(2 * np.pi * tf.cast(days, tf.float32) / 7)
        
        # Weekend flag
        is_weekend = tf.cast(days >= 5, tf.float32)
        
        # Business hours flag (9 AM - 6 PM)
        is_business_hours = tf.cast(
            (hours >= 9) & (hours <= 18),
            tf.float32
        )
        
        return {
            'hour_sin': hour_sin,
            'hour_cos': hour_cos,
            'day_sin': day_sin,
            'day_cos': day_cos,
            'is_weekend': is_weekend,
            'is_business_hours': is_business_hours
        }
    
    def compute_behavioral_features(self, amounts):
        """
        Compute behavioral features (customer patterns).
        """
        # Average transaction amount
        mean_amount = tf.reduce_mean(amounts)
        
        # Standard deviation
        std_amount = tf.math.reduce_std(amounts)
        
        # Deviation from mean
        deviation = amounts - mean_amount
        
        # Z-score (standardized deviation)
        z_score = deviation / (std_amount + 1e-8)
        
        # Velocity (count of transactions in time window)
        # Simplified: count per day
        count = tf.cast(tf.shape(amounts)[0], tf.float32)
        
        return {
            'mean_amount': mean_amount,
            'std_amount': std_amount,
            'deviation': deviation,
            'z_score': z_score,
            'count': count
        }
    
    def compute_risk_scores(self, amounts, hours):
        """
        Compute risk indicators for fraud detection.
        """
        # Unusual amount (more than 3 std from mean)
        mean = tf.reduce_mean(amounts)
        std = tf.math.reduce_std(amounts)
        
        # High amount threshold
        high_amount = tf.cast(amounts > mean + 3*std, tf.float32)
        
        # Off-hours transaction
        off_hours = tf.cast((hours < 6) | (hours > 22), tf.float32)
        
        # Combine into risk score
        risk_score = high_amount * 0.6 + off_hours * 0.4
        
        return risk_score
    
    def process_all_transactions(self, transactions):
        """
        Complete feature engineering pipeline.
        """
        amounts = transactions[:, 0]
        hours = tf.cast(transactions[:, 1], tf.int32)
        days = tf.cast(transactions[:, 2], tf.int32)
        merchants = transactions[:, 3]
        
        # Compute all features
        rolling_features = self.compute_rolling_features(amounts)
        temporal_features = self.compute_temporal_features(hours, days)
        behavioral_features = self.compute_behavioral_features(amounts)
        risk_scores = self.compute_risk_scores(amounts, hours)
        
        # Print feature statistics
        print("Transaction Feature Engineering:")
        print("="*60)
        print(f"Total transactions: {transactions.shape[0]}")
        print(f"Mean amount: ${behavioral_features['mean_amount']:.2f}")
        print(f"Std amount: ${behavioral_features['std_amount']:.2f}")
        print(f"High-risk transactions: {tf.reduce_sum(tf.cast(risk_scores > 0.5, tf.int32))}")
        
        return {
            'features': tf.stack([amounts, merchants], axis=1),
            'temporal': temporal_features,
            'risk': risk_scores
        }

def run_transaction_example():
    """Run the financial transaction processing example."""
    print("="*60)
    print("FINANCIAL TRANSACTION PROCESSING - BANKING EXAMPLE")
    print("="*60)
    
    processor = TransactionFeatureEngineering()
    
    # Load transactions
    transactions = processor.load_transactions(n_transactions=5000)
    print(f"Loaded {transactions.shape[0]} transactions")
    
    # Process transactions
    results = processor.process_all_transactions(transactions)
    
    print("\nFeature engineering complete!")
    print("="*60)

if __name__ == "__main__":
    run_transaction_example()
```

### Real-world Example 2: Healthcare - Patient Vitals Processing

```python
"""
Real-world Example 2: Healthcare - Patient Vital Signs Processing
This example demonstrates tensor operations for medical time-series data.
"""

import tensorflow as tf
import numpy as np

class VitalSignsProcessor:
    """
    Process patient vital signs for medical monitoring.
    
    Tracks:
    - Heart rate (HR): beats per minute
    - Blood oxygen (SpO2): percentage
    - Temperature: Fahrenheit
    - Blood pressure: systolic/diastolic
    - Respiratory rate: breaths per minute
    
    Generates:
    - Trend analysis
    - Anomaly detection
    - Early warning scores
    """
    
    def __init__(self):
        self.normal_ranges = {
            'heart_rate': (60, 100),
            'spo2': (95, 100),
            'temp': (97.0, 99.0),
            'bp_systolic': (90, 120),
            'bp_diastolic': (60, 80),
            'resp_rate': (12, 20)
        }
        
    def load_vital_signs(self, n_patients=100, n_readings=24):
        """
        Generate synthetic vital signs data.
        Format: (n_patients, n_readings, n_vitals)
        """
        np.random.seed(42)
        
        # Generate realistic vital signs
        vitals = []
        
        for _ in range(n_patients):
            patient_data = []
            base_hr = np.random.uniform(65, 95)
            base_spo2 = np.random.uniform(96, 99)
            base_temp = np.random.uniform(97.5, 98.5)
            base_sys = np.random.uniform(100, 120)
            base_dia = np.random.uniform(60, 80)
            base_rr = np.random.uniform(14, 18)
            
            for _ in range(n_readings):
                # Add some variation
                hr = base_hr + np.random.normal(0, 5)
                spo2 = base_spo2 + np.random.normal(0, 1)
                temp = base_temp + np.random.normal(0, 0.3)
                sys = base_sys + np.random.normal(0, 5)
                dia = base_dia + np.random.normal(0, 3)
                rr = base_rr + np.random.normal(0, 1)
                
                # Clip to realistic ranges
                spo2 = np.clip(spo2, 0, 100)
                temp = np.clip(temp, 94, 106)
                sys = np.clip(sys, 70, 200)
                dia = np.clip(dia, 40, 130)
                rr = np.clip(rr, 5, 40)
                
                patient_data.append([hr, spo2, temp, sys, dia, rr])
            
            vitals.append(patient_data)
        
        vitals = tf.constant(vitals, dtype=tf.float32)
        
        print(f"Loaded vital signs: {vitals.shape}")
        return vitals
    
    def normalize_vitals(self, vitals):
        """
        Normalize vital signs to standard ranges.
        """
        # Normalize each vital type
        normalized = tf.identity(vitals)
        
        # Heart rate (normalize to 0-1 based on normal range)
        hr = vitals[:, :, 0]
        hr_min, hr_max = 40, 180
        hr_norm = (hr - hr_min) / (hr_max - hr_min)
        
        # SpO2 (already percentage, clip to 0-100)
        spo2 = tf.clip_by_value(vitals[:, :, 1], 0, 100)
        
        # Temperature (normalize)
        temp = vitals[:, :, 2]
        temp_norm = (temp - 95) / 10
        
        # Blood pressure
        bp_sys = vitals[:, :, 3]
        bp_dia = vitals[:, :, 4]
        bp_sys_norm = (bp_sys - 70) / 130
        bp_dia_norm = (bp_dia - 40) / 90
        
        # Combine
        normalized = tf.stack([
            hr_norm,
            spo2 / 100,
            temp_norm,
            bp_sys_norm,
            bp_dia_norm,
            vitals[:, :, 5] / 30
        ], axis=2)
        
        return normalized
    
    def compute_trends(self, vitals):
        """
        Compute trends in vital signs over time.
        """
        # Simple linear regression per patient
        def compute_slope(vitals_sequence):
            # Use last vs first reading
            first = vitals_sequence[0]
            last = vitals_sequence[-1]
            return last - first
        
        # Apply across all patients and vital types
        trends = tf.map_fn(compute_slope, vitals, fn_output_signature=tf.float32)
        
        return trends
    
    def detect_anomalies(self, vitals):
        """
        Detect anomalous vital sign readings.
        """
        # Check each vital against normal ranges
        anomalies = tf.zeros_like(vitals, dtype=tf.bool)
        
        anomalies = tf.where(
            (vitals[:, :, 0] < 50) | (vitals[:, :, 0] > 150),  # HR
            True, anomalies
        )
        anomalies = tf.where(
            (vitals[:, :, 1] < 90),  # SpO2
            True, anomalies
        )
        anomalies = tf.where(
            (vitals[:, :, 2] < 95) | (vitals[:, :, 2] > 104),  # Temp
            True, anomalies
        )
        anomalies = tf.where(
            (vitals[:, :, 3] < 70) | (vitals[:, :, 3] > 180),  # BP Sys
            True, anomalies
        )
        anomalies = tf.where(
            (vitals[:, :, 4] < 40) | (vitals[:, :, 4] > 120),  # BP Dia
            True, anomalies
        )
        
        # Count anomalies per patient
        anomaly_counts = tf.reduce_sum(tf.cast(anomalies, tf.int32), axis=[1, 2])
        
        return anomalies, anomaly_counts
    
    def compute_early_warning_score(self, vitals):
        """
        Compute MEWS (Modified Early Warning Score).
        Simplified version for demonstration.
        """
        # Scores for each vital parameter
        scores = tf.zeros_like(vitals[:, :, 0])
        
        # Heart rate scoring
        hr = vitals[:, :, 0]
        scores = tf.where(hr < 50, scores + 3, scores)
        scores = tf.where((hr >= 50) & (hr < 60), scores + 2, scores)
        scores = tf.where((hr >= 60) & (hr < 100), scores + 0, scores)
        scores = tf.where((hr >= 100) & (hr < 120), scores + 1, scores)
        scores = tf.where(hr >= 120, scores + 2, scores)
        
        # SpO2 scoring
        spo2 = vitals[:, :, 1]
        scores = tf.where(spo2 < 90, scores + 4, scores)
        scores = tf.where((spo2 >= 90) & (spo2 < 94), scores + 2, scores)
        scores = tf.where(spo2 >= 94, scores + 0, scores)
        
        # Temperature scoring
        temp = vitals[:, :, 2]
        scores = tf.where(temp > 102, scores + 2, scores)
        scores = tf.where((temp >= 100) & (temp <= 102), scores + 1, scores)
        scores = tf.where((temp >= 96) & (temp < 100), scores + 0, scores)
        scores = tf.where(temp < 96, scores + 2, scores)
        
        # Sum scores
        total_scores = tf.reduce_sum(scores, axis=1)
        
        # Determine alert levels
        alert_levels = tf.constant(['Normal'] * tf.shape(vitals)[0])
        
        def update_alerts(scores):
            if scores < 4:
                return 'Normal'
            elif scores < 6:
                return 'Low Alert'
            elif scores < 8:
                return 'Medium Alert'
            else:
                return 'High Alert'
        
        # Get alert levels
        alert_levels = [update_alerts(s) for s in total_scores.numpy()]
        
        return total_scores, alert_levels
    
    def process_all_vitals(self, vitals):
        """
        Complete vital signs processing pipeline.
        """
        print("Patient Vital Signs Processing:")
        print("="*60)
        
        normalized = self.normalize_vitals(vitals)
        trends = self.compute_trends(vitals)
        anomalies, anomaly_counts = self.detect_anomalies(vitals)
        ews_scores, alert_levels = self.compute_early_warning_score(vitals)
        
        # Print summary
        print(f"Total patients: {vitals.shape[0]}")
        print(f"Readings per patient: {vitals.shape[1]}")
        
        for i in range(min(5, vitals.shape[0])):
            print(f"\nPatient {i+1}:")
            print(f"  Anomalies detected: {anomaly_counts[i].numpy()}")
            print(f"  EWS Score: {ews_scores[i].numpy()}")
            print(f"  Alert Level: {alert_levels[i]}")
        
        return {
            'normalized': normalized,
            'trends': trends,
            'anomalies': anomalies,
            'ews_scores': ews_scores
        }

def run_healthcare_example():
    """Run the healthcare vitals processing example."""
    print("="*60)
    print("PATIENT VITAL SIGNS PROCESSING - HEALTHCARE EXAMPLE")
    print("="*60)
    
    processor = VitalSignsProcessor()
    
    # Load vital signs
    vitals = processor.load_vital_signs(n_patients=100, n_readings=24)
    
    # Process all vitals
    results = processor.process_all_vitals(vitals)
    
    print("\n" + "="*60)
    print("Vital signs processing complete!")

if __name__ == "__main__":
    run_healthcare_example()
```

## V. OUTPUT_RESULTS

### Expected Outputs

For tensor creation examples:
```
TENSOR CREATION METHODS
============================================================
--- Scalar ---
Scalar: 42, Rank: 0
--- Vector ---
Vector from list: [1 2 3 4 5]
--- Matrix ---
Matrix shape: (3, 3)
Matrix:
[[1. 2. 3.]
 [4. 5. 6.]
 [7. 8. 9.]]
```

For gradient examples:
```
AUTOMATIC DIFFERENTIATION
============================================================
--- Basic Gradients ---
y = x^2 at x=3
dy/dx = 6.0

y = w*5 + b at w=2, b=1
dy/dw = 5.0
dy/db = 1.0

--- Higher-order Gradients ---
y = x^3 at x=2
First derivative (dy/dx): 12.0
Second derivative (d²y/dx²): 12.0
```

## VI. VISUALIZATION

### Tensor Operations Flow Chart

```
TENSOR CREATION AND MANIPULATION FLOW
==================================

┌─────────────────────────────────────────────────────────────────┐
                    TENSOR CREATION METHODS                       │
├─────────────────────────────────────────────────────────────────┤
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │ tf.constant │  │  tf.Variable │  │   tf.zeros  │           │
  │   (Fixed)   │  │  (Trainable) │  │   (Fill 0)  │           │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │  tf.ones     │  │  tf.random  │  │  tf.range    │           │
  │  (Fill 1)   │  │   .uniform  │  │  (Sequence)  │           │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │ tf.linspace  │  │    numpy    │  │   tf.eye     │           │
  │  (Evenly    │  │    .array   │  │  (Identity)  │           │
  │   spaced)   │  │             │  │              │           │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
└─────────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌───────────────────��─��───────────────────────────────────────────┐
                    SHAPE OPERATIONS                              │
├─────────────────────────────────────────────────────────────────┤
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │  tf.reshape  │  │tf.transpose │  │ tf.squeeze  │           │
  │    (New     │  │    (Swap    │  │  (Remove    │           │
  │   shape)    │  │   axes)     │  │  dims=1)    │           │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │ tf.expand   │  │   tf.tile  │  │  tf.concat  │           │
  │   _dims     │  │  (Repeat)  │  │   (Join)   │           │
  │  (Add axis) │  │            │  │            │           │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
└─────────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
                    ELEMENT-WISE OPERATIONS                        │
├─────────────────────────────────────────────────────────────────┤
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │   tf.add   │  │ tf.subtract│  │ tf.multiply│          │
  │     +     │  │     -     │  │     *     │          │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │  tf.divide  │  │  tf.pow     │  │  tf.sqrt    │           │
  │     /      │  │     ^       │  │    √       │           │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │  tf.exp     │  │   tf.log    │  │   tf.sign  │           │
  │    e^x     │  │   ln(x)    │  │  (+1/-1/0)│          │
  └──────────────┘  ���─���────────────┘  └──────────────┘           │
                                                                  │
└─────────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
                    MATRIX OPERATIONS                             │
├─────────────────────────────────────────────────────────────────┤
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │  tf.matmul  │  │    tf.dot │  │    tf.norm │          │
  │   (Matrix  │  │   (Dot    │  │  (Vector/ │          │
  │   mult)    │  │   prod)   │  │  Matrix)  │          │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │ tf.linalg    │  │    tf QR   │  │  tf.slice   │           │
  │   .det      │  │  (Factor)  │  │  (Extract) │           │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
└─────────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
                    REDUCTION OPERATIONS                          │
├─────────────────────────────────────────────────────────────────┤
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │ tf.reduce  │  │tf.reduce  │  │ tf.reduce  │           │
  │    _sum    │  │   _mean  │  │   _max    │           │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐           │
  │ tf.reduce    │  │ tf.argmax   │  │  tf.argmin │           │
  │    _min    │  │ (Max index)│  │ (Min index)│          │
  └──────────────┘  └──────────────┘  └──────────────┘           │
                                                                  │
└─────────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
                  AUTOMATIC DIFFERENTIATION                       │
├─────────────────────────────────────────────────────────────────┤
                                                                  │
                    GradientTape                                  │
                         │                                        │
         ┌───────────────┼───────────────┐                        │
         ▼               ▼               ▼                        │
    ┌──────────┐    ┌──────────┐    ┌──────────┐               │
    │ Forward  │    │ Compute  │    │  Apply   │               │
    │   Pass   │    │ Gradient │    │ Updates  │               │
    │ y = f(x) │    │ dy/dx    │    │ x -= lr* │               │
    └──────────┘    └──────────┘    │  grad    │               │
                                   └──────────┘               │
                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## VII. ADVANCED_TOPICS

### Advanced Topic 1: Custom Operations

```python
"""
Advanced Topic 1: Creating Custom Operations (Primitive ops)
"""

import tensorflow as tf

class CustomOperations:
    """
    Create custom tensor operations in TensorFlow.
    """
    
    @staticmethod
    @tf.function
    def custom_activation(x):
        """
        Custom activation function: Swish (x * sigmoid(x))
        """
        return x * tf.nn.sigmoid(x)
    
    @staticmethod
    @tf.function
    def custom_normalize(x):
        """
        Custom normalization: (x - mean) / (std + epsilon)
        """
        mean = tf.reduce_mean(x)
        std = tf.math.reduce_std(x)
        return (x - mean) / (std + 1e-8)
    
    @staticmethod
    @tf.function
    def custom_attention(query, key, value):
        """
        Simplified attention mechanism.
        attention(Q, K, V) = softmax(Q @ K^T / sqrt(d_k)) @ V
        """
        d_k = tf.cast(tf.shape(key)[-1], tf.float32)
        
        # Compute attention scores
        scores = tf.matmul(query, key, transpose_b=True)
        scores = scores / tf.sqrt(d_k)
        
        # Apply softmax
        attention_weights = tf.nn.softmax(scores, axis=-1)
        
        # Apply attention to values
        output = tf.matmul(attention_weights, value)
        
        return output, attention_weights

def demonstrate_custom_ops():
    """Demonstrate custom operations."""
    print("="*60)
    print("CUSTOM OPERATIONS")
    print("="*60)
    
    ops = CustomOperations()
    
    # Test custom activation
    x = tf.linspace(-5.0, 5.0, 11)
    swish = ops.custom_activation(x)
    print(f"Swish activation: {swish.numpy()}")
    
    # Test custom normalize
    data = tf.random.normal([5, 10])
    normalized = ops.custom_normalize(data)
    print(f"Normalized mean: {tf.reduce_mean(normalized):.4f}")
    print(f"Normalized std: {tf.reduce_std(normalized):.4f}")
```

### Advanced Topic 2: Gradient Clipping

```python
"""
Advanced Topic 2: Gradient Clipping for Stable Training
"""

import tensorflow as tf

class GradientClipping:
    """
    Various gradient clipping strategies.
    """
    
    @staticmethod
    def clip_by_value():
        """Clip gradients by value."""
        grads = [tf.constant([-5.0, -1.0, 0.0, 1.0, 5.0])]
        clipped = [tf.clip_by_value(g, -1.0, 1.0) for g in grads]
        return clipped
    
    @staticmethod
    def clip_by_norm():
        """Clip gradients by global norm."""
        grads = [
            tf.constant([[1.0, 2.0], [3.0, 4.0]]),
            tf.constant([[10.0, 20.0], [30.0, 40.0]])
        ]
        
        # Compute global norm
        global_norm = tf.sqrt(sum([tf.reduce_sum(g**2) for g in grads]))
        
        # Clip by norm (max_norm = 1.0)
        clipped_grads, _ = tf.clip_by_norm(grads, clip_norm=1.0)
        
        # Compute new norm
        new_norm = tf.sqrt(sum([tf.reduce_sum(g**2) for g in clipped_grads]))
        
        print(f"Original global norm: {global_norm.numpy():.2f}")
        print(f"Clipped global norm: {new_norm.numpy():.2f}")
        
        return clipped_grads
    
    @staticmethod
    def clip_by_global_norm():
        """Clip using global norm with preservation of direction."""
        grads = [
            tf.constant([100.0, 50.0]),
            tf.constant([10.0, 20.0])
        ]
        
        # Global norm clipping
        clipped, scaled = tf.clip_by_global_norm(
            grads, 
            clip_norm=1.0,
            use_norm=None  # Will compute if None
        )
        
        return clipped

def demonstrate_gradient_clipping():
    """Demonstrate gradient clipping."""
    print("="*60)
    print("GRADIENT CLIPPING")
    print("="*60)
    
    clipping = GradientClipping()
    
    print("\n--- Clip by value ---")
    clipped = clipping.clip_by_value()
    print(f"Clipped: {clipped[0].numpy()}")
    
    print("\n--- Clip by norm ---")
    clipping.clip_by_norm()
    
    print("\n--- Clip by global norm ---")
    clipping.clip_by_global_norm()
```

### Advanced Topic 3: Distributed Tensors

```python
"""
Advanced Topic 3: Distributed Tensor Operations
"""

import tensorflow as tf

class DistributedTensors:
    """
    Tensor operations in distributed setting.
    """
    
    @staticmethod
    def all_reduce():
        """
        All-reduce operation across devices.
        """
        # Create a strategy
        strategy = tf.distribute.MirroredStrategy()
        
        print(f"Number of replicas: {strategy.num_replicas_in_sync}")
        
        with strategy.scope():
            # Variable created in all devices
            variable = tf.Variable([1.0, 2.0, 3.0])
            
            # All-reduce happens automatically in distributed training
            # This is handled by the optimizer
        
        return variable
    
    @staticmethod
    def collective_ops():
        """
        TensorFlow collective operations.
        """
        try:
            # Check if GPU available
            devices = tf.config.list_physical_devices('GPU')
            
            if devices:
                print(f"Found {len(devices)} GPU(s)")
                
                # Using GPU for tensor operations
                with tf.device('/GPU:0'):
                    tensor = tf.random.normal([100, 100])
                    result = tf.matmul(tensor, tensor, transpose_a=True)
                    print(f"GPU computation complete: {result.shape}")
        
        except Exception as e:
            print(f"GPU operation skipped: {e}")

def demonstrate_distributed():
    """Demonstrate distributed tensor operations."""
    print("="*60)
    print("DISTRIBUTED TENSORS")
    print("="*60)
    
    dist = DistributedTensors()
    dist.all_reduce()
    dist.collective_ops()

### Common Pitfalls and Solutions

| Issue | Symptom | Solution |
|-------|--------|----------|
| Shape mismatch | RuntimeError: Dimensions | Check tensor shapes with `.shape` |
| Data type error | CastError | Use `tf.cast()` to convert types |
| Out of memory | OOM error | Reduce batch size or use tf.function |
| Non-leaf variable | No gradients | Watch variable in GradientTape |
| In-place mutation | Unexpected results | Use `tf.tensor_scatter_nd_update` |

## VIII. CONCLUSION

### Key Takeaways

1. **Tensors are Multi-dimensional Arrays**: All data in TensorFlow is represented as tensors with rank (dimensions), shape, and data type.

2. **Operations Transform Tensors**: TensorFlow provides hundreds of operations for creating, manipulating, and computing with tensors.

3. **Automatic Differentiation**: The GradientTape automatically computes gradients, enabling efficient backpropagation.

4. **Shape Manipulation**: Understanding tensor shapes is crucial for building and debugging neural networks.

5. **Optimization**: Use `tf.function` to compile operations into optimized graphs for faster execution.

### Next Steps

1. Study computational graphs in detail
2. Learn about Keras layers and models
3. Explore TensorFlow Extended (TFX) for production pipelines
4. Practice with real-world datasets

### Further Reading

- TensorFlow Guide: https://www.tensorflow.org/guide/tensor
- TensorFlow API: https://www.tensorflow.org/api_docs/python/tf
- "Learning TensorFlow" by Tom Hope et al.
- Deep Learning with Python by François Chollet

End of Tensors and Operations Tutorial