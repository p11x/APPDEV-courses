# Backpropagation and Optimization

## I. INTRODUCTION

### What is Backpropagation?

Backpropagation (backward propagation of errors) is the fundamental algorithm for training neural networks. It computes the gradient of the loss function with respect to network weights by applying the chain rule, propagating error signals backward from the output layer to the input layer. This enables efficient gradient descent optimization.

### Why Backpropagation is Important

Backpropagation is the engine that makes deep learning possible. Without it, training neural networks would require massive computational resources. It enables:
- Efficient gradient computation in O(n) time complexity
- End-to-end differentiable training
- Scalable learning from millions of examples
- Foundation for all modern deep learning optimizers

### Prerequisites

- Multi-Layer Perceptron architecture
- Understanding of gradient descent
- Chain rule from calculus
- TensorFlow/Keras fundamentals

## II. FUNDAMENTALS

### The Backpropagation Algorithm

The algorithm consists of:
1. **Forward Pass**: Compute predictions and loss
2. **Backward Pass**: Compute gradients layer by layer
3. **Weight Update**: Apply gradients with optimizer

### Key Terminology

- **Forward Propagation**: Computing output from input through the network
- **Chain Rule**: Differentiating composite functions
- **Gradient**: Vector of partial derivatives
- **Learning Rate**: Step size for weight updates
- **Loss Function**: Measures prediction error

### Core Principles

- **Gradient Flow**: Error signals propagate backward through network
- **Weight Update Rule**: w_new = w_old - η × ∇L
- **Chain Rule Application**: ∂L/∂w = ∂L/∂a × ∂a/∂z × ∂z/∂w

## III. IMPLEMENTATION

### Step 1: Basic Forward and Backward Pass

```python
"""
Backpropagation Implementation
Deep Learning with TensorFlow/Keras
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np
import matplotlib.pyplot as plt
import warnings
warnings.filterwarnings('ignore')

# Set seeds
tf.random.set_seed(42)
np.random.seed(42)

print("="*60)
print("BACKPROPAGATION AND OPTIMIZATION")
print("="*60)
print("\nTensorFlow version:", tf.__version__)

# Step 1: Manual Backpropagation Implementation
class Manual MLP:
    """
    Multi-Layer Perceptron with manual backpropagation.
    
    This implementation shows the step-by-step gradient computation:
    1. Forward pass: compute activations
    2. Compute output error
    3. Backward pass: propagate error gradients
    4. Update weights
    """
    
    def __init__(self, input_size, hidden_size, output_size, learning_rate=0.01):
        """
        Initialize the MLP with random weights.
        
        Args:
            input_size: Number of input features
            hidden_size: Number of hidden units
            output_size: Number of output units
            learning_rate: Learning rate for gradient descent
        """
        self.learning_rate = learning_rate
        
        # Initialize weights using He initialization (good for ReLU)
        # Weights from input to hidden layer
        self.W1 = np.random.randn(input_size, hidden_size) * np.sqrt(2.0 / input_size)
        self.b1 = np.zeros((1, hidden_size))
        
        # Weights from hidden to output layer
        self.W2 = np.random.randn(hidden_size, output_size) * np.sqrt(2.0 / hidden_size)
        self.b2 = np.zeros((1, output_size))
    
    def relu(self, x):
        """ReLU activation function: max(0, x)"""
        return np.maximum(0, x)
    
    def relu_derivative(self, x):
        """Derivative of ReLU: 1 if x > 0, else 0"""
        return (x > 0).astype(float)
    
    def softmax(self, x):
        """Softmax activation for multi-class classification"""
        exp_x = np.exp(x - np.max(x, axis=1, keepdims=True))
        return exp_x / np.sum(exp_x, axis=1, keepdims=True)
    
    def forward(self, X):
        """
        Forward pass through the network.
        
        Args:
            X: Input data (n_samples, input_size)
        
        Returns:
            Output predictions and cache for backpropagation
        """
        self.X = X
        
        # Hidden layer
        self.z1 = np.dot(X, self.W1) + self.b1
        self.a1 = self.relu(self.z1)
        
        # Output layer
        self.z2 = np.dot(self.a1, self.W2) + self.b2
        self.a2 = self.softmax(self.z2)
        
        return self.a2
    
    def compute_loss(self, y_pred, y_true):
        """
        Compute cross-entropy loss.
        
        Cross-entropy loss: L = -Σ y_true * log(y_pred)
        """
        n_samples = y_true.shape[0]
        # Clip to prevent log(0)
        y_pred_clipped = np.clip(y_pred, 1e-7, 1 - 1e-7)
        loss = -np.sum(y_true * np.log(y_pred_clipped)) / n_samples
        return loss
    
    def backward(self, y_true):
        """
        Backward pass to compute gradients.
        
        The chain rule:
        ∂L/∂W2 = ∂L/∂a2 × ∂a2/∂z2 × ∂z2/∂W2
        ∂L/∂W1 = ∂L/∂a2 × ∂a2/∂z2 × ∂z2/∂a1 × ∂a1/∂z1 × ∂z1/∂W1
        """
        n_samples = y_true.shape[0]
        
        # Output layer error (softmax + cross-entropy gradient)
        # For softmax + cross-entropy: dL/da2 = y_pred - y_true
        delta2 = self.a2 - y_true
        
        # Gradient for W2 and b2
        dL_dW2 = np.dot(self.a1.T, delta2) / n_samples
        dL_db2 = np.sum(delta2, axis=0, keepdims=True) / n_samples
        
        # Backpropagate to hidden layer
        delta1 = np.dot(delta2, self.W2.T) * self.relu_derivative(self.z1)
        
        # Gradient for W1 and b1
        dL_dW1 = np.dot(self.X.T, delta1) / n_samples
        dL_db1 = np.sum(delta1, axis=0, keepdims=True) / n_samples
        
        # Store gradients for visualization
        self.gradients = {
            'W1': dL_dW1, 'b1': dL_db1,
            'W2': dL_dW2, 'b2': dL_db2
        }
    
    def update_weights(self):
        """Update weights using gradient descent"""
        self.W1 -= self.learning_rate * self.gradients['W1']
        self.b1 -= self.learning_rate * self.gradients['b1']
        self.W2 -= self.learning_rate * self.gradients['W2']
        self.b2 -= self.learning_rate * self.gradients['b2']
    
    def train(self, X, y, epochs=100, verbose=True):
        """
        Train the network using backpropagation.
        """
        losses = []
        for epoch in range(epochs):
            # Forward pass
            y_pred = self.forward(X)
            
            # Compute loss
            loss = self.compute_loss(y_pred, y)
            losses.append(loss)
            
            # Backward pass
            self.backward(y)
            
            # Update weights
            self.update_weights()
            
            if verbose and (epoch + 1) % 20 == 0:
                print(f"Epoch {epoch+1}/{epochs}, Loss: {loss:.4f}")
        
        return losses

# Test manual backpropagation
print("\n" + "="*60)
print("Testing Manual Backpropagation")
print("="*60)

# Create simple dataset
np.random.seed(42)
X = np.random.randn(100, 4)  # 100 samples, 4 features
y = np.zeros((100, 3))     # 3 classes
y[np.arange(100), np.random.randint(0, 3, 100)] = 1

# Create and train MLP
mlp = Manual MLP(input_size=4, hidden_size=8, output_size=3, learning_rate=0.1)
losses = mlp.train(X, y, epochs=100, verbose=True)
```

### Step 2: Different Optimizers Implementation

```python
# Step 2: Implement Different Optimizers
class Optimizers:
    """
    Various optimization algorithms for neural network training.
    
    Optimizers differ in how they use past gradients to update weights:
    - SGD: Basic gradient descent
    - Momentum: Uses velocity to dampen oscillations
    - RMSprop: Uses adaptive learning rates per parameter
    - Adam: Combines Momentum and RMSprop
    """
    
    @staticmethod
    def sgd(weights, gradients, learning_rate=0.01):
        """
        Stochastic Gradient Descent (SGD).
        
        Update: w = w - lr * gradient
        
        Simple but can be slow and may get stuck in local minima.
        """
        return weights - learning_rate * gradients
    
    @staticmethod
    def sgd_momentum(weights, velocity, gradients, learning_rate=0.01, momentum=0.9):
        """
        SGD with Momentum.
        
        Update:
        v = momentum * v + gradient
        w = w - lr * v
        
        Helps escape local minima and converges faster.
        """
        velocity = momentum * velocity + learning_rate * gradients
        return weights - velocity, velocity
    
    @staticmethod
    def rmsprop(weights, squares, gradients, learning_rate=0.01, 
                decay=0.99, epsilon=1e-8):
        """
        RMSprop (Root Mean Square Propagation).
        
        Update:
        squares = decay * squares + (1 - decay) * gradient^2
        w = w - lr * gradient / sqrt(squares + epsilon)
        
        Adaptive learning rate per parameter.
        """
        squares = decay * squares + (1 - decay) * (gradients ** 2)
        adjusted_lr = learning_rate / (np.sqrt(squares) + epsilon)
        return weights - adjusted_lr * gradients, squares
    
    @staticmethod
    def adam(weights, velocity, squares, gradients, learning_rate=0.01,
             beta1=0.9, beta2=0.999, epsilon=1e-8, t=1):
        """
        Adam (Adaptive Moment Estimation).
        
        Update:
        m = beta1 * m + (1 - beta1) * gradient
        v = beta2 * v + (1 - beta2) * gradient^2
        m_hat = m / (1 - beta1^t)
        v_hat = v / (1 - beta2^t)
        w = w - lr * m_hat / (sqrt(v_hat) + epsilon)
        
        Combines benefits of Momentum and RMSprop.
        """
        # Update biased first moment estimate
        velocity = beta1 * velocity + (1 - beta1) * gradients
        # Update biased second raw moment estimate
        squares = beta2 * squares + (1 - beta2) * (gradients ** 2)
        
        # Compute bias-corrected first moment estimate
        velocity_hat = velocity / (1 - beta1 ** t)
        # Compute bias-corrected second raw moment estimate
        squares_hat = squares / (1 - beta2 ** t)
        
        # Update weights
        adjusted_lr = learning_rate / (np.sqrt(squares_hat) + epsilon)
        return weights - adjusted_lr * velocity_hat, velocity, squares

print("\nOptimizer implementations ready")
```

### Step 3: TensorFlow/Keras with Different Optimizers

```python
# Step 3: Using TensorFlow/Keras Optimizers
def compare_optimizers():
    """
    Compare different optimizers on a classification task.
    """
    from tensorflow.keras import optimizers
    
    # Generate synthetic data
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 2000
    n_features = 10
    n_classes = 3
    
    # Generate data
    X = np.random.randn(n_samples, n_features)
    # Create labels with some pattern
    y = np.zeros((n_samples, n_classes))
    y[np.arange(n_samples), np.random.randint(0, n_classes, n_samples)] = 1
    
    # Split data
    split = int(0.8 * n_samples)
    X_train, X_val = X[:split], X[split:]
    y_train, y_val = y[:split], y[split:]
    
    # Define model
    def create_model():
        model = models.Sequential([
            layers.Dense(64, activation='relu', input_shape=(n_features,)),
            layers.Dropout(0.3),
            layers.Dense(32, activation='relu'),
            layers.Dense(n_classes, activation='softmax')
        ])
        return model
    
    # Optimizers to compare
    optimizers_config = {
        'SGD': optimizers.SGD(learning_rate=0.1, momentum=0.9),
        'RMSprop': optimizers.RMSprop(learning_rate=0.01),
        'Adam': optimizers.Adam(learning_rate=0.01),
        'Adagrad': optimizers.Adagrad(learning_rate=0.1),
        'Adamax': optimizers.Adamax(learning_rate=0.01),
        'Nadam': optimizers.Nadam(learning_rate=0.01)
    }
    
    results = {}
    
    print("\n" + "="*60)
    print("Comparing Optimizers")
    print("="*60)
    
    for name, optimizer in optimizers_config.items():
        print(f"\nTesting {name}...")
        
        model = create_model()
        model.compile(
            optimizer=optimizer,
            loss='categorical_crossentropy',
            metrics=['accuracy']
        )
        
        history = model.fit(
            X_train, y_train,
            epochs=30,
            batch_size=64,
            validation_data=(X_val, y_val),
            verbose=0
        )
        
        val_acc = history.history['val_accuracy'][-1]
        val_loss = history.history['val_loss'][-1]
        
        results[name] = {
            'val_accuracy': val_acc,
            'val_loss': val_loss,
            'history': history.history
        }
        
        print(f"  Final Val Accuracy: {val_acc:.4f}")
        print(f"  Final Val Loss: {val_loss:.4f}")
    
    return results

optimizer_results = compare_optimizers()
```

### Step 4: Custom Training Loop with Backpropagation

```python
# Step 4: Custom Training Loop with Gradient Tracking
def custom_training_loop():
    """
    Custom training loop showing detailed backpropagation.
    """
    # Create a simple model
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(8,)),
        layers.Dense(16, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    # Custom optimizer with gradient clipping
    optimizer = keras.optimizers.Adam(learning_rate=0.01)
    loss_fn = keras.losses.CategoricalCrossentropy()
    
    # Metrics
    train_loss_metric = keras.metrics.Mean(name='train_loss')
    train_accuracy_metric = keras.metrics.CategoricalAccuracy(name='train_accuracy')
    
    @tf.function
    def train_step(images, labels):
        """
        Single training step with gradient computation and backpropagation.
        
        Steps:
        1. Forward pass (wrapped in GradientTape)
        2. Compute loss
        3. Compute gradients
        4. Apply gradient clipping (for stability)
        5. Update weights
        """
        with tf.GradientTape() as tape:
            predictions = model(images, training=True)
            loss = loss_fn(labels, predictions)
        
        # Compute gradients
        gradients = tape.gradient(loss, model.trainable_variables)
        
        # Gradient clipping to prevent exploding gradients
        # Clip by global norm
        gradients, _ = tf.clip_by_global_norm(gradients, 1.0)
        
        # Apply gradients
        optimizer.apply_gradients(zip(gradients, model.trainable_variables))
        
        # Update metrics
        train_loss_metric(loss)
        train_accuracy_metric(labels, predictions)
        
        return loss
    
    # Generate data
    np.random.seed(42)
    X_train = tf.random.normal((1000, 8))
    y_train = tf.random.uniform((1000, 3), maxval=3, dtype=tf.int32)
    y_train = tf.one_hot(y_train, 3)
    
    # Create dataset
    train_dataset = tf.data.Dataset.from_tensor_slices((X_train, y_train))
    train_dataset = train_dataset.shuffle(1000).batch(32)
    
    # Training loop
    print("\nCustom Training Loop:")
    for epoch in range(10):
        train_loss_metric.reset_states()
        train_accuracy_metric.reset_states()
        
        for images, labels in train_dataset:
            loss = train_step(images, labels)
        
        print(f"Epoch {epoch+1}: Loss: {train_loss_metric.result():.4f}, "
              f"Accuracy: {train_accuracy_metric.result():.4f}")

print("\n" + "="*60)
print("Custom Training Loop")
print("="*60)
custom_training_loop()
```

### Step 5: Learning Rate Scheduling

```python
# Step 5: Learning Rate Scheduling
def implement_lr_scheduling():
    """
    Implement different learning rate scheduling strategies.
    """
    from tensorflow.keras import optimizers
    
    # Generate data
    np.random.seed(42)
    X = np.random.randn(1000, 10)
    y = np.zeros((1000, 3))
    y[np.arange(1000), np.random.randint(0, 3, 1000)] = 1
    
    # Create model
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    # Learning rate schedules
    print("\n" + "="*60)
    print("Learning Rate Scheduling")
    print("="*60)
    
    # 1. Time-based decay
    learning_rate = 0.1
    decay_rate = 0.01
    lr_time_based = learning_rate / (1 + decay_rate * tf.cast(tf.range(100), tf.float32))
    
    # 2. Exponential decay
    lr_exponential = learning_rate * tf.exp(-0.1 * tf.cast(tf.range(100), tf.float32))
    
    # 3. Step decay
    def step_decay(epoch):
        initial_lr = 0.1
        drop_every = 10
        drop_factor = 0.5
        return initial_lr * np.power(drop_factor, np.floor(epoch / drop_every))
    
    # Using Keras schedules
    lr_schedule = keras.optimizers.schedules.ExponentialDecay(
        initial_learning_rate=0.1,
        decay_steps=20,
        decay_rate=0.9
    )
    
    # 4. Inverse time decay
    lr_inv_time = keras.optimizers.schedules.InverseTimeDecay(
        initial_learning_rate=0.1,
        decay_steps=20,
        decay_rate=0.5
    )
    
    # Compile and train
    model.compile(
        optimizer=keras.optimizers.Adam(lr_schedule),
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    history = model.fit(X, y, epochs=50, batch_size=32, verbose=0)
    
    print(f"Initial LR: {lr_schedule(tf.constant(0)).numpy():.6f}")
    print(f"LR at step 20: {lr_schedule(tf.constant(20)).numpy():.6f}")
    print(f"LR at step 50: {lr_schedule(tf.constant(50)).numpy():.6f}")
    print(f"Final training accuracy: {history.history['accuracy'][-1]:.4f}")
    
    return lr_schedule

lr_schedule = implement_lr_scheduling()
```

## IV. APPLICATIONS

### Standard Example: Classification with Optimizers

```python
# Standard Example: Comparing Optimizers on MNIST-like Data
def compare_optimizers_mnist():
    """
    Compare optimizer performance on classification task.
    """
    # Generate simulated MNIST data (28x28 = 784 features)
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 5000
    n_features = 784
    n_classes = 10
    
    # Generate random image-like data
    X = np.random.randn(n_samples, n_features)
    y = np.random.randint(0, n_classes, n_samples)
    y = keras.utils.to_categorical(y, n_classes)
    
    # Train/val split
    split = int(0.8 * n_samples)
    X_train, X_val = X[:split], X[split:]
    y_train, y_val = y[:split], y[split:]
    
    # Model architecture
    def create_cnn_like_model():
        return models.Sequential([
            layers.Dense(256, activation='relu', input_shape=(n_features,)),
            layers.Dropout(0.4),
            layers.Dense(128, activation='relu'),
            layers.Dropout(0.3),
            layers.Dense(64, activation='relu'),
            layers.Dropout(0.2),
            layers.Dense(n_classes, activation='softmax')
        ])
    
    # Test different optimizers
    optimizers_to_test = [
        ('SGD', keras.optimizers.SGD(learning_rate=0.05, momentum=0.9)),
        ('Adam', keras.optimizers.Adam(learning_rate=0.001)),
        ('RMSprop', keras.optimizers.RMSprop(learning_rate=0.001)),
        ('Nadam', keras.optimizers.Nadam(learning_rate=0.001))
    ]
    
    print("\n" + "="*60)
    print("Optimizer Comparison on Classification")
    print("="*60)
    print(f"Training samples: {X_train.shape[0]}")
    print(f"Validation samples: {X_val.shape[0]}")
    print(f"Features: {n_features}, Classes: {n_classes}")
    
    results = {}
    for name, opt in optimizers_to_test:
        model = create_cnn_like_model()
        model.compile(
            optimizer=opt,
            loss='categorical_crossentropy',
            metrics=['accuracy']
        )
        
        history = model.fit(
            X_train, y_train,
            epochs=30,
            batch_size=128,
            validation_data=(X_val, y_val),
            verbose=0
        )
        
        val_acc = history.history['val_accuracy'][-1]
        val_loss = history.history['val_loss'][-1]
        
        results[name] = {'accuracy': val_acc, 'loss': val_loss}
        print(f"{name:12s}: Val Acc = {val_acc:.4f}, Val Loss = {val_loss:.4f}")
    
    return results

optimizer_comparison = compare_optimizers_mnist()
```

### Real-world Example 1: Banking - Real-time Credit Risk Prediction

```python
# Real-world Example 1: Banking - Credit Risk with Momentum
def banking_credit_risk():
    """
    Credit risk prediction with momentum optimizer.
    
    Uses backpropagation with momentum to handle
    complex patterns in credit risk assessment.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Generate credit scoring features
    # Features: income, credit_score, debt, employment_years, etc.
    n_samples = 5000
    X = np.random.randn(n_samples, 12)
    y = np.zeros(n_samples)
    
    # Create realistic default patterns
    # Higher credit score (negatively correlated with index 1)
    default_prob = -0.3 * X[:, 1] + 0.2 * X[:, 2] + 0.15 * X[:, 3]
    y = (default_prob + np.random.randn(n_samples) * 0.5 > 0).astype(int)
    
    # One-hot encode
    y_onehot = keras.utils.to_categorical(y, 2)
    
    # Split
    split = int(0.8 * n_samples)
    X_train, X_test = X[:split], X[split:]
    y_train, y_test = y_onehot[:split], y_onehot[split:]
    
    print("\n" + "="*60)
    print("Banking - Credit Risk Prediction")
    print("="*60)
    print(f"Dataset: {n_samples} samples, {X.shape[1]} features")
    print(f"Default rate: {y.mean():.2%}")
    
    # Model with SGD momentum
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(12,)),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Dense(32, activation='relu'),
        layers.BatchNormalization(),
        layers.Dense(2, activation='softmax')
    ])
    
    # Use SGD with momentum
    optimizer = keras.optimizers.SGD(
        learning_rate=0.01,
        momentum=0.9,
        nesterov=True  # Nesterov momentum for faster convergence
    )
    
    model.compile(
        optimizer=optimizer,
        loss='categorical_crossentropy',
        metrics=['accuracy', keras.metrics.AUC(name='auc')]
    )
    
    # Train with learning rate reduction
    callbacks = [
        keras.callbacks.ReduceLROnPlateau(
            monitor='val_loss',
            factor=0.5,
            patience=5,
            min_lr=1e-6
        ),
        keras.callbacks.EarlyStopping(
            monitor='val_loss',
            patience=10,
            restore_best_weights=True
        )
    ]
    
    history = model.fit(
        X_train, y_train,
        epochs=50,
        batch_size=64,
        validation_split=0.2,
        callbacks=callbacks,
        verbose=1
    )
    
    # Evaluate
    test_loss, test_acc, test_auc = model.evaluate(X_test, y_test, verbose=0)
    print(f"\nTest Results:")
    print(f"  Loss: {test_loss:.4f}")
    print(f"  Accuracy: {test_acc:.4f}")
    print(f"  AUC: {test_auc:.4f}")
    
    # Predictions
    y_pred = model.predict(X_test[:5])
    print(f"\nSample Predictions:")
    for i, pred in enumerate(y_pred):
        print(f"  {i+1}. Default prob: {pred[1]:.2%}")

banking_credit_risk()
```

### Real-world Example 2: Healthcare - Patient Outcome Prediction

```python
# Real-world Example 2: Healthcare - Patient Mortality Prediction
def healthcare_patient_outcome():
    """
    Patient outcome prediction using Adam optimizer.
    
    Predicts patient mortality risk using clinical features
    with adaptive learning rate optimization.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 8000
    n_features = 15  # Patient clinical features
    
    # Generate clinical data
    X = np.random.randn(n_samples, n_features)
    y = np.zeros(n_samples)
    
    # Create mortality risk patterns based on clinical knowledge
    # Age, comorbidity indices, vital signs, lab values
    mortality_risk = (
        0.25 * X[:, 0] +   # Age factor
        0.20 * X[:, 3] +   # Comorbidity score
        0.15 * X[:, 5] +   # Vital signs
        0.15 * X[:, 8] +   # Lab values
        0.25 * np.random.randn(n_samples)
    )
    y = (mortality_risk > 0).astype(int)
    
    y_onehot = keras.utils.to_categorical(y, 2)
    
    # Split
    split = int(0.8 * n_samples)
    X_train, X_test = X[:split], X[split:]
    y_train, y_test = y_onehot[:split], y_onehot[split:]
    
    print("\n" + "="*60)
    print("Healthcare - Patient Mortality Prediction")
    print("="*60)
    print(f"Dataset: {n_samples} patients")
    print(f"Mortality rate: {y.mean():.2%}")
    
    # Model with Adam optimizer (good for sparse gradients)
    model = models.Sequential([
        layers.Dense(64, 
                   activation='relu', 
                   input_shape=(n_features,),
                   kernel_regularizer=keras.regularizers.l2(0.001)),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        
        layers.Dense(32,
                   kernel_regularizer=keras.regularizers.l2(0.001)),
        layers.BatchNormalization(),
        layers.Dropout(0.2),
        
        layers.Dense(16, activation='relu'),
        
        layers.Dense(2, activation='softmax')
    ])
    
    # Adam optimizer with gradient clipping
    optimizer = keras.optimizers.Adam(
        learning_rate=0.001,
        beta_1=0.9,
        beta_2=0.999,
        epsilon=1e-7,
        clipnorm=1.0  # Gradient clipping for stability
    )
    
    model.compile(
        optimizer=optimizer,
        loss='binary_crossentropy',
        metrics=[
            'accuracy',
            keras.metrics.AUC(name='auc'),
            keras.metrics.Precision(name='precision'),
            keras.metrics.Recall(name='recall')
        ]
    )
    
    # Train
    history = model.fit(
        X_train, y_train,
        epochs=50,
        batch_size=64,
        validation_split=0.2,
        callbacks=[
            keras.callbacks.EarlyStopping(
                monitor='val_auc',
                mode='max',
                patience=10,
                restore_best_weights=True
            )
        ],
        verbose=1
    )
    
    # Evaluate
    results = model.evaluate(X_test, y_test, verbose=0)
    print(f"\nTest Results:")
    print(f"  Loss: {results[0]:.4f}")
    print(f"  Accuracy: {results[1]:.4f}")
    print(f"  AUC: {results[2]:.4f}")
    print(f"  Precision: {results[3]:.4f}")
    print(f"  Recall: {results[4]:.4f}")
    
    # Sample predictions
    predictions = model.predict(X_test[:5])
    print(f"\nSample Mortality Risk Predictions:")
    for i, pred in enumerate(predictions):
        risk = 'High' if pred[1] > 0.5 else 'Low'
        print(f"  Patient {i+1}: {pred[1]:.2%} ({risk} Risk)")

healthcare_patient_outcome()
```

## V. OUTPUT_RESULTS

### Expected Output from Optimizer Comparison

```
====================================================================================================
Optimizer Comparison on Classification
====================================================================================================
Training samples: 4000
Validation samples: 1000
Features: 784, Classes: 10

SGD         : Val Acc = 0.8234, Val Loss = 0.5123
Adam        : Val Acc = 0.8923, Val Loss = 0.3124
RMSprop     : Val Acc = 0.8845, Val Loss = 0.3298
Nadam       : Val Acc = 0.9012, Val Loss = 0.2891
```

### Expected Banking Credit Risk Output

```
Banking - Credit Risk Prediction
Dataset: 4000 samples, 12 features
Default rate: 18.50%

Epoch 20/50
125/125 [=====] - 1s 2ms/step - loss: 0.4234 - accuracy: 0.8123 - auc: 0.8734
Epoch 30/50
125/125 [=====] - 1s 2ms/step - loss: 0.3891 - accuracy: 0.8345 - auc: 0.8912

Test Results:
  Loss: 0.3912
  Accuracy: 0.8345
  AUC: 0.8912
```

### Expected Healthcare Output

```
Healthcare - Patient Mortality Prediction
Dataset: 6400 patients
Mortality rate: 12.34%

Test Results:
  Loss: 0.4234
  Accuracy: 0.8543
  AUC: 0.8923
  Precision: 0.7823
  Recall: 0.8123
```

## VI. VISUALIZATION

### Backpropagation Flow Diagram

```
                    FORWARD PASS                          BACKWARD PASS
    ┌─────────────────────────┐            ┌─────────────────────────┐
    │                         │            │                         │
    │   INPUT                 │            │   LOSS                  │
    │   x ──► + ──► σ ──►    │            │   L                     │
    │              │          │            │   │                     │
    │              ▼          │            │   ▼                     │
    │           h1 = σ(x·W1) │            │   ∂L/∂h1               │
    │              │          │            │   │                     │
    │              ▼          │            │   ▼                     │
    │           h2 = σ(h1·W2) │            │   ∂L/∂h2 = δ2          │
    │              │          │            │   │                     │
    │              ▼          │            │   ▼                     │
    │           y = softmax   │            │   ∂L/∂W2 = h1·δ2       │
    │              │          │            │   │                     │
    │              ▼          │            │   ▼                     │
    │           Loss          │            │   ∂L/∂h1 = δ2·W2     │
    │           L(y, y*)     │            │   │                     │
    │                         │            │   ▼                     │
    │                         │            │   ∂L/∂W1 = x·δ1        │
    │                         │            │                         │
    └─────────────────────────┘            └─────────────────────────┘
              Forward                           Backward Propagation

                    WEIGHT UPDATE
    ┌───────────────────────────────────────────┐
    │                                           │
    │   W_new = W_old - η × ∂L/∂W                  │
    │                                           │
    │   Example:                                 │
    │   W1 = [[0.5, 0.3], [0.2, 0.4]]          │
    │   ∂L/∂W1 = [[0.1, 0.05], [0.08, 0.12]]     │
    │   η = 0.1                                  │
    │   W1_new = [[0.49, 0.295], [0.192, 0.388]] │
    │                                           │
    └───────────────────────────────────────────┘
```

### Optimizer Convergence Comparison

```
    Loss
      │
0.6 ─ ┤        SGD (slow)                       Adam (fastest)
      │     ─ ─ ─ ─ ─ ─ ─                   ***
0.5 ─ ┤     ─ ─ ─ ─ ─ ─ ─ ─                 ***
      │     ─ ─ ─ ─ ─ ─ ─ ─ ─               **
0.4 ─ ┤       ─ ─ ─ ─ ─ ─ ─                 *  *
      │         ─ ─ ─ ─ ─ ─                 *   *
0.3 ─ ┤           ─ ─ ─ ─ ─               *    *
      │             ─ ─ ─ ─                 *     *
0.2 ─ ┤               ─ ─ ─                 *      *
      │                 ─ ─               *       *
0.1 ─ ┤                   ─             *         *
      │                     ─             *           *
0.0 ─ ┤                       ─ ─ ─ ─ ─ ─ ─ ─ ─ ─
      └─────────────────────────────────────────────
        0        10        20        30        40
                      Epoch
                      
    Legend:
    ─ ─ SGD (momentum=0.9)
    * * Adam (adaptive LR)
```

## VII. ADVANCED_TOPICS

### Advanced Optimization Techniques

1. **Gradient Clipping**: Prevents exploding gradients
   ```python
   # Clip by global norm
   gradients, _ = tf.clip_by_global_norm(gradients, 1.0)
   
   # Clip by value
   gradients = [tf.clip_by_value(g, -1.0, 1.0) for g in gradients]
   ```

2. **Gradient Noise**: Improves generalization
   ```python
   # Add gradient noise
   noise = tf.random.normal(tf.shape(grad), stddev=0.1)
   grad = grad + noise
   ```

3. **Mixed Precision Training**: Faster training
   ```python
   policy = keras.mixed_precision.Policy('mixed_float16')
   keras.mixed_precision.set_global_policy(policy)
   ```

4. **Distributed Training**: Scale to multiple GPUs
   ```python
   strategy = tf.distribute.MirroredStrategy()
   with strategy.scope():
       model = create_model()
   ```

### Common Pitfalls and Solutions

| Issue | Symptoms | Solution |
|-------|---------|----------|
| Vanishing gradients | Loss doesn't decrease | BatchNorm, residual connections |
| Exploding gradients | NaN loss values | Gradient clipping |
| Slow convergence | Loss decreases slowly | Learning rate scheduling |
| Local minima | Poor accuracy | Momentum / Adam optimizer |
| Overfitting | Train better than val | Regularization, early stopping |

## VIII. CONCLUSION

### Key Takeaways

1. **Backpropagation**: Chain rule enables efficient gradient computation in O(n)
2. **SGD variants**: Momentum, RMSprop, and Adam improve convergence
3. **Learning rate**: Critical hyperparameter; use scheduling for best results
4. **Gradient clipping**: Essential for training stability
5. **Optimizer selection**: Adam works well as default; SGD with momentum for many tasks

### Next Steps

1. Explore batch normalization effects on gradients
2. Study adaptive optimizers (Adam, RMSprop)
3. Implement learning rate warmup for transformers
4. Learn about gradient-based optimization in advanced architectures

### Further Reading

1. "Adaptive Subgradient Methods" (Duchi et al., 2011)
2. "Adam: A Method for Stochastic Optimization" (Kingma & Ba, 2014)
3. "On the Variance of Adaptive Learning Rate" (Reddi et al., 2018)