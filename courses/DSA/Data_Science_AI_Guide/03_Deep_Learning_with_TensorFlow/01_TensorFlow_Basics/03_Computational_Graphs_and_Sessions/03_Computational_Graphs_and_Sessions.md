# Computational Graphs and Sessions

## I. INTRODUCTION

### What are Computational Graphs?
A computational graph is a directed graph where nodes represent operations (ops) and edges represent data flow (tensors). In TensorFlow 1.x, you would define the graph first, then execute it within a session. In TensorFlow 2.x, eager execution is the default, meaning operations execute immediately as they're called. However, understanding computational graphs is still crucial for optimization, deployment, and advanced use cases.

The concept of a computational graph allows TensorFlow to:
1. Optimize computation by analyzing the graph structure
2. Distribute operations across multiple devices (CPUs, GPUs, TPUs)
3. Execute subgraphs efficiently (only compute what's needed)
4. Enable automatic differentiation via backpropagation

### Why are Computational Graphs Important?

1. **Performance Optimization**: Graphs can be optimized before execution. TensorFlow analyzes the graph to:
   - Eliminate common subexpressions
   - Fuse operations where possible
   - Schedule operations efficiently across devices

2. **Portability**: Graphs are platform-agnostic representations. A graph defined in Python can be:
   - Saved and loaded in other languages
   - Deployed to mobile devices (TensorFlow Lite)
   - Deployed to production servers (TensorFlow Serving)

3. **Automatic Differentiation**: The graph tracks all operations, enabling automatic gradient computation through backpropagation.

4. **Distributed Computing**: Graphs can be partitioned and executed across multiple machines.

### Prerequisites
- Understanding of tensors (covered in previous module)
- Basic Python programming
- Familiarity with neural network concepts
- Understanding of gradient descent

## II. FUNDAMENTALS

### Key Concepts

1. **Eager Execution**: Operations execute immediately, returning concrete values. This is TensorFlow 2.x default.

2. **tf.function**: Decorator that traces the function to create a computational graph. Enables graph optimization while maintaining eager-like syntax.

3. **Graph Mode**: The underlying representation used by TensorFlow for optimization.

4. **Session (TF 1.x)**: The mechanism for executing graphs (deprecated in TF 2.x but available for compatibility).

### Graph Components

- **Nodes**: Operations (tf.Operation) like `tf.matmul`, `tf.add`, `tf.nn.relu`
- **Edges**: Tensors (tf.Tensor) flowing between operations
- **Placeholders**: Input nodes (legacy TF 1.x concept)
- **Variables**: Trainable parameters stored in state

## III. IMPLEMENTATION

### Eager Execution vs Graph Execution

```python
"""
TensorFlow Computational Graphs - Module 3
This module demonstrates computational graphs in TensorFlow.
"""

import tensorflow as tf
import numpy as np

class EagerVsGraph:
    """
    Compare eager execution vs graph execution.
    """
    
    @staticmethod
    def eager_execution():
        """
        Eager execution - executes immediately.
        """
        print("Eager Execution:")
        print("="*50)
        
        # Operations execute immediately
        a = tf.constant([[1.0, 2.0], [3.0, 4.0]])
        b = tf.constant([[5.0, 6.0], [7.0, 8.0]])
        
        # Immediate execution
        result = tf.matmul(a, b)
        
        print(f"a = {a.numpy()}")
        print(f"b = {b.numpy()}")
        print(f"a @ b = {result.numpy()}")
        
        # Python control flow executes normally
        for i in range(3):
            val = tf.constant(float(i))
            print(f"  Iteration {i}: {val.numpy()}")
        
        return result
    
    @staticmethod
    def graph_execution():
        """
        Graph execution using tf.function.
        """
        print("\n" + "="*50)
        print("Graph Execution with tf.function:")
        print("="*50)
        
        # Define a function to trace into a graph
        @tf.function
        def matrix_multiply(a, b):
            """
            This function will be traced into a computation graph.
            """
            return tf.matmul(a, b)
        
        # Call the function - first call traces, subsequent calls use cached graph
        a = tf.constant([[1.0, 2.0], [3.0, 4.0]])
        b = tf.constant([[5.0, 6.0], [7.0, 8.0]])
        
        result = matrix_multiply(a, b)
        
        print(f"Result from tf.function: {result.numpy()}")
        
        # Get the concrete function
        print(f"Concrete function: {matrix_multiply.get_concrete_function(a, b)}")
        
        return result
    
    @staticmethod
    def graph_with_control_flow():
        """
        Graph with control flow - tf.function handles Python control flow.
        """
        print("\n" + "="*50)
        print("Graph with Control Flow:")
        print("="*50)
        
        @tf.function
        def process_with_loop(inputs):
            """
            Process inputs through a loop - gets traced into graph.
            """
            result = tf.Variable(tf.zeros_like(inputs))
            
            for i in range(tf.shape(inputs)[0]):
                result[i].assign(inputs[i] * 2)
            
            return result
        
        @tf.function
        def process_with_condition(x):
            """
            Conditional execution in graph.
            """
            if tf.reduce_mean(x) > 0.5:
                return x * 2
            else:
                return x / 2
        
        # Test loop function
        inputs = tf.constant([1.0, 2.0, 3.0])
        loop_result = process_with_loop(inputs)
        print(f"Loop result: {loop_result.numpy()}")
        
        # Test conditional
        x_high = tf.constant([0.8, 0.9, 0.7])
        x_low = tf.constant([0.1, 0.2, 0.3])
        
        cond_result1 = process_with_condition(x_high)
        cond_result2 = process_with_condition(x_low)
        
        print(f"Condition high (mean>0.5): {cond_result1.numpy()}")
        print(f"Condition low (mean<0.5): {cond_result2.numpy()}")

def demonstrate_eager_vs_graph():
    """Demonstrate eager vs graph execution."""
    
    demo = EagerVsGraph()
    demo.eager_execution()
    demo.graph_execution()
    demo.graph_with_control_flow()

class GraphStructure:
    """
    Analyze and manipulate graph structure.
    """
    
    @staticmethod
    def inspect_graph():
        """
        Inspect the auto-generated graph.
        """
        print("\n" + "="*50)
        print("Graph Inspection:")
        print("="*50)
        
        @tf.function
        def simple_function(x):
            """A simple function to inspect."""
            a = x * 2
            b = a + 1
            c = b * 3
            return c
        
        # Get concrete function
        concrete = simple_function.get_concrete_function(
            tf.TensorSpec(shape=None, dtype=tf.float32)
        )
        
        # Get the graph
        graph = concrete.graph
        
        print(f"Graph operations: {len(graph.get_operations())}")
        
        # Print operations
        print("\nOperations in graph:")
        for op in graph.get_operations()[:5]:
            print(f"  {op.name}: {op.type}")
        
        return graph
    
    @staticmethod
    def get_trace():
        """
        Get traces from tf.function.
        """
        print("\n" + "="*50)
        print("Function Traces:")
        print("="*50)
        
        # Call with different shapes to see tracing behavior
        @tf.function
        def dynamic_function(x):
            return tf.reduce_sum(x)
        
        # First call - creates initial trace
        result1 = dynamic_function(tf.constant([1, 2, 3]))
        print(f"Trace 1 (shape=(3,)): {result1.numpy()}")
        
        # Second call with same shape - uses cached trace
        result2 = dynamic_function(tf.constant([1, 2, 3, 4]))
        print(f"Trace 2 (shape=(4,)): {result2.numpy()}")
        
        # Third call - creates new trace for different shape
        result3 = dynamic_function(tf.constant([1, 2]))
        print(f"Trace 3 (shape=(2,)): {result3.numpy()}")
        
        # Analyze traces
        print(f"\nTraced graphs: {len(dynamic_function.traced_outputs)}")

class InputSignatures:
    """
    Working with input signatures.
    """
    
    @staticmethod
    def basic_signature():
        """
        Using input signatures to control tracing.
        """
        print("\n" + "="*50)
        print("Input Signatures:")
        print("="*50)
        
        # Define with input signature - controls what gets traced
        @tf.function(
            input_signature=[
                tf.TensorSpec(shape=(None, 10), dtype=tf.float32)
            ]
        )
        def process_features(features):
            """Process features with fixed signature."""
            return tf.nn.relu(features @ tf.Variable(
                tf.random.normal([10, 5]), name='weights'
            ) + tf.Variable(
                tf.zeros([5]), name='bias'
            ))
        
        # Test with different batch sizes
        batch1 = tf.random.normal([32, 10])
        batch2 = tf.random.normal([64, 10])
        
        result1 = process_features(batch1)
        result2 = process_features(batch2)
        
        print(f"Input shapes: {batch1.shape}, {batch2.shape}")
        print(f"Output shapes: {result1.shape}, {result2.shape}")
    
    @staticmethod
    def multiple_signatures():
        """
        Multiple input signatures.
        """
        @tf.function(
            input_signature=[
                tf.TensorSpec(shape=(None,), dtype=tf.float32),
                tf.TensorSpec(shape=(None,), dtype=tf.float32)
            ]
        )
        def compute_similarity(x, y):
            """Compute cosine similarity."""
            x_norm = x / tf.sqrt(tf.reduce_sum(x**2))
            y_norm = y / tf.sqrt(tf.reduce_sum(y**2))
            return tf.reduce_sum(x_norm * y_norm)
        
        # Test
        x = tf.constant([1.0, 2.0, 3.0])
        y = tf.constant([1.0, 2.0, 3.0])
        
        similarity = compute_similarity(x, y)
        print(f"Cosine similarity: {similarity.numpy():.4f}")

class GradientInGraphs:
    """
    Computing gradients in graph mode.
    """
    
    @staticmethod
    def compute_gradients():
        """
        Compute gradients in graph mode.
        """
        print("\n" + "="*50)
        print("Gradients in Graph Mode:")
        print("="*50)
        
        # Variables
        w = tf.Variable(tf.random.normal([3, 3]), name='weight')
        b = tf.Variable(tf.zeros([3]), name='bias')
        
        @tf.function
        def forward_pass(x, y):
            """Forward pass with gradient computation."""
            # Forward pass
            prediction = tf.matmul(x, w) + b
            
            # Compute loss (MSE)
            loss = tf.reduce_mean((prediction - y) ** 2)
            
            return prediction, loss
        
        # Compute gradients
        x = tf.random.normal([10, 3])
        y = tf.random.normal([10, 3])
        
        # Using GradientTape with tf.function
        with tf.GradientTape() as tape:
            _, loss = forward_pass(x, y)
        
        # Compute gradients
        grad_w, grad_b = tape.gradient(
            loss, 
            [w, b]
        )
        
        print(f"Loss: {loss.numpy():.4f}")
        print(f"Weight gradient shape: {grad_w.shape}")
        print(f"Bias gradient shape: {grad_b.shape}")
        
        return grad_w, grad_b
    
    @staticmethod
    def jacobian_computation():
        """
        Compute Jacobians in graph mode.
        """
        @tf.function
        def vector_function(x):
            """Vector-valued function."""
            return x ** 2 + tf.reduce_sum(x)
        
        x = tf.Variable([1.0, 2.0, 3.0])
        
        with tf.GradientTape() as tape:
            y = vector_function(x)
        
        jacobian = tape.jacobian(y, x)
        
        print(f"\nJacobian:\n{jacobian.numpy()}")

class GraphOptimization:
    """
    Graph optimization techniques.
    """
    
    @staticmethod
    def auto_mixed_precision():
        """
        Use mixed precision for faster computation.
        """
        print("\n" + "="*50)
        print("Auto Mixed Precision:")
        print("="*50)
        
        # Enable mixed precision
        policy = tf.keras.mixed_precision.Policy('mixed_float16')
        tf.keras.mixed_precision.set_global_policy(policy)
        
        @tf.function
        def compute_intensive(x):
            """Compute intensive operation."""
            for _ in range(10):
                x = x @ tf.transpose(x)
            return tf.reduce_mean(x)
        
        # Test
        x = tf.random.normal([100, 100])
        
        result = compute_intensive(x)
        
        print(f"Result: {result.numpy():.4f}")
        print(f"Policy: {tf.keras.mixed_precision.global_policy()}")
        
        # Reset policy
        tf.keras.mixed_precision.set_global_policy('float32')
    
    @staticmethod
    def xla_compilation():
        """
        XLA (Accelerated Linear Algebra) compilation.
        """
        print("\n" + "="*50)
        print("XLA Compilation:")
        print("="*50)
        
        # Enable XLA
        tf.config.optimizer.set_jit(True)
        
        @tf.function(jit_compile=True)
        def optimized_function(x):
            """Function optimized with XLA."""
            return tf.reduce_sum(x ** 2 + tf.reduce_mean(x))
        
        x = tf.random.normal([1000, 1000])
        result = optimized_function(x)
        
        print(f"Result: {result.numpy():.4f}")
        
        # Disable XLA (reset)
        tf.config.optimizer.set_jit(False)

def demonstrate_graph_features():
    """Demonstrate advanced graph features."""
    
    struct = GraphStructure()
    struct.inspect_graph()
    struct.get_trace()
    
    signatures = InputSignatures()
    signatures.basic_signature()
    signatures.multiple_signatures()
    
    gradients = GradientInGraphs()
    gradients.compute_gradients()
    gradients.jacobian_computation()
    
    optimize = GraphOptimization()
    optimize.auto_mixed_precision()
    optimize.xla_compilation()

### Standard Example: Building a Neural Network with tf.function

```python
"""
Standard Example: Neural Network using tf.function
Demonstrates building efficient neural networks with graph mode.
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers

class NeuralNetworkGraph:
    """
    Build a neural network using tf.function for optimization.
    """
    
    def __init__(self, input_dim, hidden_units, output_dim):
        self.input_dim = input_dim
        self.hidden_units = hidden_units
        self.output_dim = output_dim
        
        self.weights1 = None
        self.biases1 = None
        self.weights2 = None
        self.biases2 = None
        
        self._initialize_weights()
    
    def _initialize_weights(self):
        """Initialize trainable weights."""
        self.weights1 = tf.Variable(
            tf.random.normal([self.input_dim, self.hidden_units])
        )
        self.biases1 = tf.Variable(tf.zeros([self.hidden_units]))
        
        self.weights2 = tf.Variable(
            tf.random.normal([self.hidden_units, self.output_dim])
        )
        self.biases2 = tf.Variable(tf.zeros([self.output_dim]))
    
    @tf.function
    def forward_pass(self, x):
        """
        Forward pass - traced into a computation graph.
        """
        # First layer
        hidden = tf.matmul(x, self.weights1) + self.biases1
        hidden = tf.nn.relu(hidden)
        
        # Second layer (output)
        output = tf.matmul(hidden, self.weights2) + self.biases2
        output = tf.nn.softmax(output)
        
        return output
    
    def train_step(self, x, y, optimizer):
        """
        Single training step.
        """
        with tf.GradientTape() as tape:
            predictions = self.forward_pass(x)
            loss = tf.reduce_mean(
                tf.keras.losses.categorical_crossentropy(y, predictions)
            )
        
        gradients = tape.gradient(
            loss, 
            [self.weights1, self.biases1, self.weights2, self.biases2]
        )
        
        optimizer.apply_gradients(zip(
            gradients,
            [self.weights1, self.biases1, self.weights2, self.biases2]
        ))
        
        return loss
    
    @tf.function
    def train_step_graph(self, x, y, optimizer):
        """
        Training step fully traced - including gradient computation.
        """
        with tf.GradientTape() as tape:
            predictions = self.forward_pass(x)
            loss = tf.reduce_mean(
                tf.keras.losses.categorical_crossentropy(y, predictions)
            )
        
        gradients = tape.gradient(
            loss,
            [self.weights1, self.biases1, self.weights2, self.biases2]
        )
        
        optimizer.apply_gradients(zip(
            gradients,
            [self.weights1, self.biases1, self.weights2, self.biases2]
        ))
        
        return loss

def run_neural_network_example():
    """Run the neural network graph example."""
    print("="*60)
    print("NEURAL NETWORK WITH TF.FUNCTION")
    print("="*60)
    
    # Generate data
    n_samples = 1000
    input_dim = 20
    hidden_units = 32
    output_dim = 10
    
    # Generate synthetic data
    np.random.seed(42)
    x_train = np.random.randn(n_samples, input_dim).astype(np.float32)
    y_train = np.random.randint(0, output_dim, n_samples)
    y_train = keras.utils.to_categorical(y_train, output_dim)
    
    # Normalize input
    x_train = (x_train - x_train.mean(axis=0)) / x_train.std(axis=0)
    
    # Create model
    model = NeuralNetworkGraph(input_dim, hidden_units, output_dim)
    
    # Optimizer
    optimizer = keras.optimizers.Adam(learning_rate=0.01)
    
    # Training loop
    batch_size = 32
    n_epochs = 5
    
    print("\nTraining...")
    for epoch in range(n_epochs):
        total_loss = 0
        n_batches = 0
        
        # Shuffle data
        indices = np.random.permutation(n_samples)
        
        for i in range(0, n_samples, batch_size):
            idx = indices[i:i+batch_size]
            x_batch = tf.constant(x_train[idx])
            y_batch = tf.constant(y_train[idx])
            
            loss = model.train_step_graph(x_batch, y_batch, optimizer)
            total_loss += loss.numpy()
            n_batches += 1
        
        avg_loss = total_loss / n_batches
        print(f"Epoch {epoch+1}/{n_epochs}, Loss: {avg_loss:.4f}")
    
    # Evaluate
    test_input = tf.constant(x_train[:5])
    predictions = model.forward_pass(test_input)
    
    print(f"\nSample predictions: {predictions.numpy()[:2]}")
    print(f"True labels: {y_train[:2]}")

if __name__ == "__main__":
    run_neural_network_example()
```

### Real-world Example 1: Banking/Finance - Risk Model

```python
"""
Real-world Example 1: Banking/Finance - Credit Risk Model
This example demonstrates computational graphs for financial modeling.
"""

import tensorflow as tf
import numpy as np

class CreditRiskModel:
    """
    Credit risk prediction model using graph-optimized computations.
    
    Predicts probability of default based on:
    - Customer financial history
    - Transaction patterns
    - External risk factors
    """
    
    def __init__(self, n_features):
        self.n_features = n_features
        
        # Initialize model components
        self.dense1_weights = tf.Variable(
            tf.random.normal([n_features, 64])
        )
        self.dense1_bias = tf.Variable(tf.zeros([64]))
        
        self.dense2_weights = tf.Variable(
            tf.random.normal([64, 32])
        )
        self.dense2_bias = tf.Variable(tf.zeros([32]))
        
        self.output_weights = tf.Variable(
            tf.random.normal([32, 1])
        )
        self.output_bias = tf.Variable(tf.zeros([1]))
    
    @tf.function
    def forward_pass(self, features):
        """
        Forward pass through the risk model.
        Uses traced computation for efficiency.
        """
        # Feature engineering layer
        x = tf.matmul(features, self.dense1_weights) + self.dense1_bias
        x = tf.nn.relu(x)
        
        # Hidden layer
        x = tf.matmul(x, self.dense2_weights) + self.dense2_bias
        x = tf.nn.relu(x)
        
        # Output layer (risk score)
        risk_score = tf.matmul(x, self.output_weights) + self.output_bias
        probability = tf.nn.sigmoid(risk_score)
        
        return probability
    
    @tf.function
    def compute_loss(self, features, labels):
        """
        Compute binary cross-entropy loss.
        """
        predictions = self.forward_pass(features)
        loss = tf.reduce_mean(
            tf.keras.losses.binary_crossentropy(labels, predictions)
        )
        return loss
    
    @tf.function
    def train_step(self, features, labels, optimizer):
        """
        Single training step - fully traced.
        """
        with tf.GradientTape() as tape:
            loss = self.compute_loss(features, labels)
        
        gradients = tape.gradient(
            loss,
            [
                self.dense1_weights, self.dense1_bias,
                self.dense2_weights, self.dense2_bias,
                self.output_weights, self.output_bias
            ]
        )
        
        optimizer.apply_gradients(zip(gradients, [
            self.dense1_weights, self.dense1_bias,
            self.dense2_weights, self.dense2_bias,
            self.output_weights, self.output_bias
        ]))
        
        return loss

def generate_financial_data(n_samples, n_features):
    """Generate synthetic financial data."""
    np.random.seed(42)
    
    # Generate features
    features = np.random.randn(n_samples, n_features).astype(np.float32)
    
    # Generate labels (10% default rate)
    labels = np.random.binomial(1, 0.1, n_samples).astype(np.float32)
    labels = labels.reshape(-1, 1)
    
    return features, labels

def run_banking_example():
    """Run banking example."""
    print("="*60)
    print("CREDIT RISK MODEL - BANKING EXAMPLE")
    print("="*60)
    
    # Configuration
    n_samples = 5000
    n_features = 15
    
    # Generate data
    features, labels = generate_financial_data(n_samples, n_features)
    
    print(f"Generated {n_samples} samples with {n_features} features")
    print(f"Default rate: {labels.mean():.2%}")
    
    # Create model
    model = CreditRiskModel(n_features)
    
    # Optimizer
    optimizer = tf.keras.optimizers.Adam(learning_rate=0.001)
    
    # Training
    batch_size = 64
    n_epochs = 10
    
    print("\nTraining credit risk model...")
    for epoch in range(n_epochs):
        total_loss = 0
        n_batches = 0
        
        indices = np.random.permutation(n_samples)
        
        for i in range(0, n_samples, batch_size):
            idx = indices[i:i+batch_size]
            
            x_batch = tf.constant(features[idx])
            y_batch = tf.constant(labels[idx])
            
            loss = model.train_step_graph(x_batch, y_batch, optimizer)
            total_loss += loss.numpy()
            n_batches += 1
        
        if (epoch + 1) % 2 == 0:
            print(f"Epoch {epoch+1}/{n_epochs}, Loss: {total_loss/n_batches:.4f}")
    
    # Evaluate
    test_features = tf.constant(features[:10])
    risk_scores = model.forward_pass(test_features)
    
    print(f"\nSample risk scores (first 10):")
    print(f"Mean: {tf.reduce_mean(risk_scores):.4f}")
    print(f"Range: [{tf.reduce_min(risk_scores):.4f}, {tf.reduce_max(risk_scores):.4f}]")
    
    print("\nCredit risk model complete!")

if __name__ == "__main__":
    run_banking_example()
```

### Real-world Example 2: Healthcare - Patient Outcome Prediction

```python
"""
Real-world Example 2: Healthcare - Patient Outcome Prediction
This example demonstrates computational graphs for medical predictions.
"""

import tensorflow as tf
import numpy as np

class PatientOutcomeModel:
    """
    Patient outcome prediction model for hospital readmission.
    
    Predicts:
    - 30-day readmission probability
    - Mortality risk
    - Length of stay prediction
    """
    
    def __init__(self, input_features):
        self.input_features = input_features
        
        # Model weights
        self.layer1 = tf.Variable(
            tf.keras.initializers.GlorotUniform()(
                [input_features, 128]
            )
        )
        self.layer1_bias = tf.Variable(tf.zeros([128]))
        
        self.layer2 = tf.Variable(
            tf.keras.initializers.GlorotUniform()([128, 64])
        )
        self.layer2_bias = tf.Variable(tf.zeros([64]))
        
        # Multiple outputs
        self.readmit_output = tf.Variable(
            tf.keras.initializers.GlorotUniform()([64, 1])
        )
        self.mortality_output = tf.Variable(
            tf.keras.initializers.GlorotUniform()([64, 1])
        )
        self.los_output = tf.Variable(
            tf.keras.initializers.GlorotUniform()([64, 1])
        )
    
    @tf.function(input_signature=[
        tf.TensorSpec(shape=(None, None), dtype=tf.float32)
    ])
    def predict_outcomes(self, patient_data):
        """
        Predict multiple patient outcomes.
        """
        # Shared feature extraction
        x = tf.matmul(patient_data, self.layer1) + self.layer1_bias
        x = tf.nn.relu(x)
        
        x = tf.matmul(x, self.layer2) + self.layer2_bias
        x = tf.nn.relu(x)
        
        # Specific outputs
        readmit = tf.nn.sigmoid(
            tf.matmul(x, self.readmit_output)
        )
        mortality = tf.nn.sigmoid(
            tf.matmul(x, self.mortality_output)
        )
        los = tf.nn.relu(
            tf.matmul(x, self.los_output)
        )
        
        return {
            'readmission_prob': readmit,
            'mortality_prob': mortality,
            'length_of_stay': los
        }
    
    @tf.function(input_signature=[
        tf.TensorSpec(shape=(None, None), dtype=tf.float32),
        tf.TensorSpec(shape=(None, 3), dtype=tf.float32)
    ])
    def compute_loss(self, features, labels):
        """
        Compute multi-task loss.
        """
        predictions = self.predict_outcomes(features)
        
        # Multi-task loss
        readmit_loss = tf.reduce_mean(
            tf.keras.losses.binary_crossentropy(
                labels[:, 0:1], 
                predictions['readmission_prob']
            )
        )
        mortality_loss = tf.reduce_mean(
            tf.keras.losses.binary_crossentropy(
                labels[:, 1:2], 
                predictions['mortality_prob']
            )
        )
        los_loss = tf.reduce_mean(
            tf.keras.losses.mean_squared_error(
                labels[:, 2:3], 
                predictions['length_of_stay']
            )
        )
        
        total_loss = readmit_loss + mortality_loss + los_loss
        
        return total_loss

def generate_patient_data(n_patients, n_vitals):
    """Generate synthetic patient data."""
    np.random.seed(42)
    
    # Features: vitals, demographics, lab values
    features = np.random.randn(n_patients, n_vitals).astype(np.float32)
    
    # Labels: readmission, mortality, LOS
    readmit = np.random.binomial(1, 0.15, n_patients)
    mortality = np.random.binomial(1, 0.05, n_patients)
    los = np.random.exponential(3, n_patients)
    
    labels = np.stack([readmit, mortality, los], axis=1).astype(np.float32)
    
    return features, labels

def run_healthcare_example():
    """Run healthcare example."""
    print("="*60)
    print("PATIENT OUTCOME PREDICTION - HEALTHCARE EXAMPLE")
    print("="*60)
    
    # Configuration
    n_patients = 10000
    n_vitals = 20  # Vital signs, demographics, lab values
    
    # Generate patient data
    features, labels = generate_patient_data(n_patients, n_vitals)
    
    print(f"Generated data for {n_patients} patients")
    print(f"Features: {n_vitals}")
    print(f"Readmission rate: {labels[:, 0].mean():.1%}")
    print(f"Mortality rate: {labels[:, 1].mean():.1%}")
    print(f"Mean LOS: {labels[:, 2].mean():.1f} days")
    
    # Create model
    model = PatientOutcomeModel(n_vitals)
    
    # Get first predictions
    sample_features = tf.constant(features[:5])
    predictions = model.predict_outcomes(sample_features)
    
    print(f"\nSample predictions (first 5):")
    print(f"  Readmission: {predictions['readmission_prob'].numpy().flatten()[:3]}")
    print(f"  Mortality: {predictions['mortality_prob'].numpy().flatten()[:3]}")
    print(f"  LOS: {predictions['length_of_stay'].numpy().flatten()[:3]}")
    
    print("\nPatient outcome prediction complete!")

if __name__ == "__main__":
    run_healthcare_example()
```

## V. OUTPUT_RESULTS

### Expected Output

```
TENSORFLOW COMPUTATIONAL GRAPHS
============================================================
Eager Execution:
==================================================
a = [[1. 2.]
 [3. 4.]]
b = [[5. 6.]
 [7. 8.]]
a @ b = [[19. 22.]
 [43. 50.]]

==================================================
Graph Execution with tf.function:
==================================================
Result from tf.function: [[19. 22.]
 [43. 50.]]
Concrete function: <ConcreteFunction ...

Neural Network with tf.function
============================================================
Training...
Epoch 1/5, Loss: 2.3083
Epoch 2/5, Loss: 2.2341
...
```

## VI. VISUALIZATION

### Computational Graph Architecture

```
COMPUTATIONAL GRAPH WORKFLOW
========================

┌─────────────────────────────────────────────────────────────────┐
                    TF.FUNCTION FLOW                              │
├─────────────────────────────────────────────────────────────────┤
                                                                  │
  ┌─────────────────────────────────────────────────────────┐      │
  │         PYTHON FUNCTION CODE                            │      │
  │                                                         │      │
  │      @tf.function                                      │      │
  │      def forward_pass(x):                              │      │
  │          y = x @ w + b                               │      │
  │          y = relu(y)                                │      │
  │          return softmax(y)                             │      │
  └─────────────────────────────────────────────────────────┘      │
                        │                                         │
                        ▼                                         │
  ┌─────────────────────────────────────────────────────────┐      │
  │         TF.TRACE PROCESS                            │      │
  │                                                         │      │
  │   Call 1: trace(x) → create graph                    │      │
  │   Call 2: same shape → use cached graph               │      │
  │   Call 3: new shape → retrace + cache               │      │
  └─────────────────────────────────────────────────────────┘      │
                        │                                         │
                        ▼                                         │
  ┌─────────────────────────────────────────────────────────┐      │
  │        OPTIMIZED COMPUTATION GRAPH                    │      │
  │                                                         │      │
  │       [Input]                                            │      │
  │          │                                              │      │
  │          ▼                                              │      │
  │   ┌──────────┐                                          │      │
  │   │ MatMul   │ ──► [Weight]                             │      │
  │   └──────────┘                                          │      │
  │          │                                              │      │
  │          ▼                                              │      │
  │   ┌──────────┐                                          │      │
  │   │ Add     │ ──► [Bias]                               │      │
  │   └──────────┘                                          │      │
  │          │                                              │      │
  │          ▼                                              │      │
  │   ┌──────────┐                                          │      │
  │   │ Relu    │                                          │      │
  │   └──────────┘                                          │      │
  │          │                                              │      │
  │          ▼                                              │      │
  │   ┌──────────┐                                          │      │
  │   │ Softmax │                                          │      │
  │   └──────────┘                                          │      │
  │          │                                              │      │
  │          ▼                                              │      │
  │       [Output]                                           │      │
  │                                                         │      │
  └─────────────────────────────────────────────────────────┘      │
                                                                  │
└─────────────────────────────────────────────────────────────────┘

                          │
                          ▼
┌──────────────────��─��────────────────────────────────────────────┐
                    OPTIMIZATION LAYERS                            │
├─────────────────────────────────────────────────────────────────┤
                                                                  │
  Graph Optimizer:                                                 │
  1. Remove dead ops (unused results)                              │
  2. Fuse operations (MatMul + Add = Dense)                    │
  3. Constant folding (static values computed at trace time)         │
  4. Layout transformation (NHWC → NCHW for GPU)               │
                                                                  │
  XLA Compilation:                                               │
  1. JIT compilation to machine code                            │
  2. Memory optimization                                     │
  3. Parallel kernel fusion                                  │
                                                                  │
  Mixed Precision:                                              │
  1. Float16 for compute-intensive ops                          │
  2. Float32 for accumulation                                 │
  3. Volta+ Tensor Cores for matrix mult                        │
                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## VII. ADVANCED_TOPICS

### Advanced Topic 1: Custom Graph Operations

```python
"""
Advanced Topic 1: Custom Operations in Graphs
"""

import tensorflow as tf

class CustomGraphOps:
    """
    Create custom graph operations.
    """
    
    @staticmethod
    @tf.function
    def custom_dense(x, units, activation=None):
        """
        Custom dense layer operation.
        """
        # Initialize on first call
        if not hasattr(custom_dense, 'kernel'):
            custom_dense.kernel = tf.Variable(
                tf.random.normal([x.shape[-1], units])
            )
        
        output = tf.matmul(x, custom_dense.kernel)
        
        if activation:
            output = activation(output)
        
        return output
    
    @staticmethod
    def using_tf_raw_ops():
        """
        Using raw TensorFlow operations for custom behavior.
        """
        @tf.function
        def custom_batch_norm(x, mean, variance, offset, scale, epsilon=1e-5):
            """
            Custom batch normalization operation.
            """
            # Normalize
            normalized = (x - mean) / tf.sqrt(variance + epsilon)
            
            # Scale and shift
            output = normalized * scale + offset
            
            return output
        
        return custom_batch_norm
```

### Advanced Topic 2: Distributed Graphs

```python
"""
Advanced Topic 2: Distributed Graph Execution
"""

import tensorflow as tf

class DistributedGraphs:
    """
    Distributed computation graphs.
    """
    
    @staticmethod
    def mirrored_strategy():
        """
        MirroredStrategy for multi-GPU.
        """
        strategy = tf.distribute.MirroredStrategy()
        
        print(f"Devices: {strategy.num_replicas_in_sync}")
        
        with strategy.scope():
            model = tf.keras.Sequential([
                tf.keras.layers.Dense(64, activation='relu'),
                tf.keras.layers.Dense(10)
            ])
            model.compile(optimizer='adam', loss='categorial_crossentropy')
        
        return model
    
    @staticmethod
    def multiworker_strategy():
        """
        MultiWorkerMirroredStrategy for multi-machine.
        """
        # Requires cluster specification
        cluster_spec = {
            'worker': ['host1:port', 'host2:port']
        }
        
        strategy = tf.distribute.experimental.MultiWorkerMirroredStrategy(
            cluster_resolver=tf.distribute.cluster_resolver.SimpleClusterResolver(
                cluster_spec
            )
        )
```

### Advanced Topic 3: Graph Serialization

```python
"""
Advanced Topic 3: Saving and Loading Graphs
"""

import tensorflow as tf

class GraphSerialization:
    """
    Save and load computation graphs.
    """
    
    @staticmethod
    def save_concrete_function():
        """
        Save a concrete function.
        """
        @tf.function
        def simple_fn(x):
            return x ** 2
        
        # Get concrete function
        concrete = simple_fn.get_concrete_function(
            tf.TensorSpec(shape=None, dtype=tf.float32)
        )
        
        # Save to SavedModel
        saved_model_path = '/tmp/saved_fn'
        tf.saved_model.save(concrete, saved_model_path)
        
        print(f"Saved to: {saved_model_path}")
        
        return saved_model_path
    
    @staticmethod
    def load_concrete_function(path):
        """
        Load a concrete function.
        """
        loaded = tf.saved_model.load(path)
        return loaded
```

### Common Pitfalls

| Issue | Symptom | Solution |
|-------|---------|----------|
| Too many traces | Slow first calls | Use input_signature to limit traces |
| No gradients | None gradients | Watch variables in GradientTape |
| Memory leak | OOM in tf.function | Use clear_cached_tools() |
| Slow execution | Eager is faster | Add @tf.function decorator |

## VIII. CONCLUSION

### Key Takeaways

1. **Computational Graphs**: TensorFlow builds computation graphs to optimize execution. Use `@tf.function` to enable graph mode.

2. **Input Signatures**: Control tracing with input signatures to avoid excessive recompilations.

3. **Performance**: Graph mode with XLA and mixed precision provides significant speedups.

4. **Debugging**: Start with eager execution, then optimize with graphs.

5. **Serialization**: SavedModel format captures complete graphs for deployment.

### Next Steps

1. Explore TensorFlow Extended (TFX) for production pipelines
2. Learn about TensorFlow Serving for deployment
3. Study TensorFlow Lite for mobile/edge
4. Practice with real-world architectures

### Further Reading

- TensorFlow Graph Guide: https://www.tensorflow.org/guide/function
- tf.function API: https://www.tensorflow.org/api_docs/python/tf/function
- AutoGraph Guide: https://www.tensorflow.org/guide/autograph

End of Computational Graphs and Sessions Tutorial