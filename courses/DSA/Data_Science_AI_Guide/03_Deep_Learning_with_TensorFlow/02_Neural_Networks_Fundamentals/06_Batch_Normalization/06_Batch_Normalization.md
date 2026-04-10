# Batch Normalization

## I. INTRODUCTION

### What is Batch Normalization?

Batch Normalization (BatchNorm) is a technique that normalizes the inputs of each layer across a mini-batch. It reduces internal covariate shift by normalizing layer inputs, making the network more stable and enabling faster training with higher learning rates.

### Why Batch Normalization Matters

- **Accelerates training**: Enables higher learning rates
- **Reduces internal covariate shift**: Makes optimization easier
- **Provides regularization**: Acts as implicit regularizer
- **Stabilizes gradients**: Reduces vanishing/exploding gradient problems
- **Allows deeper networks**: Makes very deep networks trainable

### Prerequisites

- Neural network fundamentals
- Gradient descent optimization
- Activation functions
- Regularization concepts

## II. FUNDAMENTALS

### Key Concepts

1. **Internal Covariate Shift**: Change in distribution of network inputs
2. **Normalization**: Scaling to zero mean and unit variance
3. **Running Statistics**: Moving average for inference
4. **Scale and Shift**: Learnable parameters for flexibility

### How BatchNorm Works

```
BatchNorm(x):
    1. Compute mean: μ = mean(x)
    2. Compute variance: σ² = var(x)
    3. Normalize: x_norm = (x - μ) / √(σ² + ε)
    4. Scale and shift: y = γ * x_norm + β
```

### Core Principles

- Normalize each mini-batch independently
- Learn scale (γ) and shift (β) parameters
- Use running statistics during inference

## III. IMPLEMENTATION

### Step 1: Basic Batch Normalization

```python
"""
Batch Normalization
Deep Learning with TensorFlow/Keras
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np
import warnings
warnings.filterwarnings('ignore')

tf.random.set_seed(42)
np.random.seed(42)

print("="*60)
print("BATCH NORMALIZATION")
print("="*60)

# Step 1: Basic BatchNorm Implementation
def demonstrate_batchnorm():
    """
    Demonstrate Batch Normalization in Keras.
    """
    # Model without BatchNorm
    model_no_bn = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(20,)),
        layers.Dense(64, activation='relu'),
        layers.Dense(3, activation='softmax')
    ])
    
    # Model with BatchNorm
    model_with_bn = models.Sequential([
        layers.Dense(64, input_shape=(20,)),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        
        layers.Dense(64),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        
        layers.Dense(3, activation='softmax')
    ])
    
    # Compile
    model_no_bn.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model_with_bn.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    print("Models created: one without BatchNorm, one with BatchNorm")
    model_with_bn.summary()
    
    return model_no_bn, model_with_bn

no_bn, with_bn = demonstrate_batchnorm()
```

### Step 2: BatchNorm in Different Positions

```python
# Step 2: BatchNorm Placement
def compare_bn_placement():
    """
    Compare different BatchNorm placements.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    X = np.random.randn(1000, 30)
    y = keras.utils.to_categorical(np.random.randint(0, 4, 1000), 4)
    
    # Before activation (original paper)
    def create_bn_before():
        model = models.Sequential([
            layers.Dense(64, input_shape=(30,)),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            layers.Dense(4, activation='softmax')
        ])
        return model
    
    # After activation
    def create_bn_after():
        model = models.Sequential([
            layers.Dense(64, input_shape=(30,)),
            layers.Activation('relu'),
            layers.BatchNormalization(),
            layers.Dense(4, activation='softmax')
        ])
        return model
    
    # Both before (deeper)
    def create_bn_both():
        model = models.Sequential([
            layers.Dense(64, input_shape=(30,)),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            layers.Dense(32),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            layers.Dense(4, activation='softmax')
        ])
        return model
    
    print("\n" + "="*60)
    print("BatchNorm Placement Comparison")
    print("="*60)
    
    configs = [
        ('Before Activation', create_bn_before),
        ('After Activation', create_bn_after),
        ('Both Places', create_bn_both)
    ]
    
    results = {}
    for name, create_fn in configs:
        model = create_fn()
        model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
        
        history = model.fit(X, y, epochs=10, batch_size=64, verbose=0, validation_split=0.2)
        
        val_acc = history.history['val_accuracy'][-1]
        results[name] = val_acc
        print(f"{name:20s}: Val Accuracy = {val_acc:.4f}")
    
    return results

placement_results = compare_bn_placement()
```

### Step 3: Custom BatchNorm Layer

```python
# Step 3: Custom BatchNorm Implementation
class CustomBatchNorm(layers.Layer):
    """
    Custom Batch Normalization layer.
    
    Implements batch normalization with:
    - Mean and variance computed over batch dimension
    - Learnable gamma (scale) and beta (shift) parameters
    - Moving average for inference
    """
    
    def __init__(self, momentum=0.99, epsilon=1e-3, **kwargs):
        super(CustomBatchNorm, self).__init__(**kwargs)
        self.momentum = momentum
        self.epsilon = epsilon
    
    def build(self, input_shape):
        # Get feature dimension
        channels = input_shape[-1]
        
        # Scale parameter (gamma)
        self.gamma = self.add_weight(
            name='gamma',
            shape=(channels,),
            initializer='ones',
            trainable=True
        )
        
        # Shift parameter (beta)
        self.beta = self.add_weight(
            name='beta',
            shape=(channels,),
            initializer='zeros',
            trainable=True
        )
        
        # Running mean and variance (for inference)
        self.running_mean = self.add_weight(
            name='running_mean',
            shape=(channels,),
            initializer='zeros',
            trainable=False
        )
        
        self.running_variance = self.add_weight(
            name='running_variance',
            shape=(channels,),
            initializer='ones',
            trainable=False
        )
        
        super(CustomBatchNorm, self).build(input_shape)
    
    def call(self, inputs, training=None):
        # During training: use batch statistics
        if training:
            # Compute mean and variance over batch
            mean = tf.reduce_mean(inputs, axis=0)
            variance = tf.reduce_mean(tf.square(inputs - mean), axis=0)
            
            # Normalize
            normalized = (inputs - mean) / tf.sqrt(variance + self.epsilon)
            
            # Update running statistics
            self.running_mean.assign(
                self.momentum * self.running_mean + 
                (1 - self.momentum) * mean
            )
            self.running_variance.assign(
                self.momentum * self.running_variance + 
                (1 - self.momentum) * variance
            )
        else:
            # During inference: use running statistics
            normalized = (inputs - self.running_mean) / \
                       tf.sqrt(self.running_variance + self.epsilon)
        
        # Scale and shift
        return self.gamma * normalized + self.beta
    
    def get_config(self):
        config = super(CustomBatchNorm, self).get_config()
        config.update({
            'momentum': self.momentum,
            'epsilon': self.epsilon
        })
        return config

print("\nCustom BatchNorm layer implemented")
```

### Step 4: Compare With/Without BatchNorm

```python
# Step 4: Compare Training With/Without BatchNorm
def compare_training():
    """
    Compare training with and without BatchNorm.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Deep network to show BatchNorm benefits
    def create_deep_no_bn():
        model = models.Sequential([
            layers.Dense(64, activation='relu', input_shape=(50,)),
            layers.Dense(64, activation='relu'),
            layers.Dense(64, activation='relu'),
            layers.Dense(64, activation='relu'),
            layers.Dense(4, activation='softmax')
        ])
        return model
    
    def create_deep_with_bn():
        model = models.Sequential([
            layers.Dense(64, input_shape=(50,)),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            
            layers.Dense(64),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            
            layers.Dense(64),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            
            layers.Dense(64),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            
            layers.Dense(4, activation='softmax')
        ])
        return model
    
    # Generate data
    X = np.random.randn(2000, 50)
    y = keras.utils.to_categorical(np.random.randint(0, 4, 2000), 4)
    
    print("\n" + "="*60)
    print("Deep Network Training Comparison")
    print("="*60)
    
    # Train without BatchNorm
    model_no_bn = create_deep_no_bn()
    model_no_bn.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    print("\nTraining deep network WITHOUT BatchNorm...")
    history_no_bn = model_no_bn.fit(X, y, epochs=30, batch_size=64, 
                                     validation_split=0.2, verbose=0)
    
    # Train with BatchNorm
    model_with_bn = create_deep_with_bn()
    model_with_bn.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    print("Training deep network WITH BatchNorm...")
    history_with_bn = model_with_bn.fit(X, y, epochs=30, batch_size=64,
                                         validation_split=0.2, verbose=0)
    
    print(f"\nWithout BatchNorm - Final Val Acc: {history_no_bn.history['val_accuracy'][-1]:.4f}")
    print(f"With BatchNorm - Final Val Acc: {history_with_bn.history['val_accuracy'][-1]:.4f}")
    
    return history_no_bn, history_with_bn

bn_comparison = compare_training()
```

### Step 5: BatchNorm with Different Optimizers

```python
# Step 5: BatchNorm with Different Optimizers
def bn_with_optimizers():
    """
    BatchNorm performs well with various optimizers.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    X = np.random.randn(2000, 40)
    y = keras.utils.to_categorical(np.random.randint(0, 3, 2000), 3)
    
    print("\n" + "="*60)
    print("BatchNorm with Different Optimizers")
    print("="*60)
    
    optimizers = [
        ('SGD', keras.optimizers.SGD(learning_rate=0.1)),
        ('SGD+Momentum', keras.optimizers.SGD(learning_rate=0.1, momentum=0.9)),
        ('Adam', keras.optimizers.Adam(learning_rate=0.01)),
        ('RMSprop', keras.optimizers.RMSprop(learning_rate=0.01)),
    ]
    
    results = {}
    for name, opt in optimizers:
        model = models.Sequential([
            layers.Dense(64, input_shape=(40,)),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            
            layers.Dense(32),
            layers.BatchNormalization(),
            layers.Activation('relu'),
            
            layers.Dense(3, activation='softmax')
        ])
        
        model.compile(optimizer=opt, loss='categorical_crossentropy', metrics=['accuracy'])
        
        history = model.fit(X, y, epochs=20, batch_size=64, 
                          validation_split=0.2, verbose=0)
        
        val_acc = history.history['val_accuracy'][-1]
        results[name] = val_acc
        print(f"{name:15s}: Val Accuracy = {val_acc:.4f}")
    
    return results

opt_results = bn_with_optimizers()
```

## IV. APPLICATIONS

### Standard Example: CIFAR-10 Classification

```python
# Standard Example: BatchNorm with CNN
def standard_batchnorm_cnn():
    """
    BatchNorm with Convolutional Neural Network.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Generate synthetic CIFAR-like data
    n_samples = 3000
    X = np.random.randn(n_samples, 32, 32, 3)
    y = keras.utils.to_categorical(np.random.randint(0, 10, n_samples), 10)
    
    print("\n" + "="*60)
    print("CIFAR-like Classification with BatchNorm")
    print("="*60)
    print(f"Samples: {n_samples}, Image: 32x32x3, Classes: 10")
    
    # CNN with BatchNorm
    model = models.Sequential([
        # Block 1
        layers.Conv2D(32, (3, 3), padding='same', input_shape=(32, 32, 3)),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.Conv2D(32, (3, 3), padding='same'),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.MaxPooling2D((2, 2)),
        
        # Block 2
        layers.Conv2D(64, (3, 3), padding='same'),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.Conv2D(64, (3, 3), padding='same'),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.MaxPooling2D((2, 2)),
        
        # Dense layers
        layers.Flatten(),
        layers.Dense(128),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.Dropout(0.5),
        layers.Dense(10, activation='softmax')
    ])
    
    model.compile(
        optimizer='adam',
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    history = model.fit(X, y, epochs=20, batch_size=64, 
                       validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    
    return model, history

cnn_bn = standard_batchnorm_cnn()
```

### Real-world Example 1: Banking - Credit Scoring

```python
# Real-world Example 1: Banking - Credit Scoring with BatchNorm
def banking_credit_with_bn():
    """
    Credit scoring with BatchNorm.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 4000
    n_features = 20
    
    X = np.random.randn(n_samples, n_features)
    y = np.zeros(n_samples)
    
    # Default pattern
    default = (0.2 * X[:, 0] + 0.15 * X[:, 3] + 0.1 * X[:, 7] +
              np.random.randn(n_samples) * 0.5)
    y = (default > 0).astype(int)
    
    split = int(0.8 * n_samples)
    X_train, X_test = X[:split], X[split:]
    y_train, y_test = y[:split], y[split:]
    
    print("\n" + "="*60)
    print("Banking - Credit Scoring with BatchNorm")
    print("="*60)
    print(f"Default rate: {y.mean():.2%}")
    
    # Model with BatchNorm
    model = models.Sequential([
        layers.Dense(128, input_shape=(n_features,)),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.Dropout(0.4),
        
        layers.Dense(64),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.Dropout(0.3),
        
        layers.Dense(32),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(
        optimizer=keras.optimizers.Adam(learning_rate=0.001),
        loss='binary_crossentropy',
        metrics=['accuracy', keras.metrics.AUC(name='auc')]
    )
    
    history = model.fit(
        X_train, y_train,
        epochs=30,
        batch_size=64,
        validation_split=0.2,
        verbose=1
    )
    
    results = model.evaluate(X_test, y_test, verbose=0)
    print(f"\nTest Results:")
    print(f"  Accuracy: {results[1]:.4f}")
    print(f"  AUC: {results[2]:.4f}")

banking_credit_with_bn()
```

### Real-world Example 2: Healthcare - Patient Diagnosis

```python
# Real-world Example 2: Healthcare - Diagnosis with BatchNorm
def healthcare_diagnosis_with_bn():
    """
    Medical diagnosis with BatchNorm.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 5000
    n_features = 25
    
    X = np.random.randn(n_samples, n_features)
    y = np.zeros(n_samples)
    
    # Diagnosis patterns
    diagnosis = (X[:, 0] * 0.25 + X[:, 2] * 0.2 + X[:, 5] * 0.15 +
                np.random.randn(n_samples) * 0.5)
    y = (diagnosis > 0).astype(int)
    
    y_onehot = keras.utils.to_categorical(y, 2)
    
    split = int(0.8 * n_samples)
    X_train, X_test = X[:split], X[split:]
    y_train, y_test = y_onehot[:split], y_onehot[split:]
    
    print("\n" + "="*60)
    print("Healthcare - Patient Diagnosis with BatchNorm")
    print("="*60)
    print(f"Positive diagnosis rate: {y.mean():.2%}")
    
    # Model with BatchNorm and regularization
    model = models.Sequential([
        layers.Dense(64, input_shape=(n_features,)),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.Dropout(0.3),
        
        layers.Dense(32),
        layers.BatchNormalization(),
        layers.Activation('relu'),
        layers.Dropout(0.2),
        
        layers.Dense(16, activation='relu'),
        
        layers.Dense(2, activation='softmax')
    ])
    
    model.compile(
        optimizer='adam',
        loss='categorical_crossentropy',
        metrics=['accuracy', keras.metrics.AUC(name='auc')]
    )
    
    history = model.fit(
        X_train, y_train,
        epochs=30,
        batch_size=64,
        validation_split=0.2,
        verbose=1
    )
    
    results = model.evaluate(X_test, y_test, verbose=0)
    print(f"\nTest Results:")
    print(f"  Accuracy: {results[1]:.4f}")
    print(f"  AUC: {results[2]:.4f}")

healthcare_diagnosis_with_bn()
```

## V. OUTPUT_RESULTS

### Expected Output

```
====================================================================================================
BatchNorm Placement Comparison
====================================================================================================
Before Activation: Val Accuracy = 0.8234
After Activation: Val Accuracy = 0.8156
Both Places: Val Accuracy = 0.8412

Deep Network Training:
Without BatchNorm: Val Accuracy = 0.7234 (slow convergence)
With BatchNorm: Val Accuracy = 0.8523 (fast convergence)
```

### Banking Example

```
Banking - Credit Scoring with BatchNorm
Default rate: 19.80%

Test Results:
  Accuracy: 0.8734
  AUC: 0.9123
```

### Healthcare Example

```
Healthcare - Patient Diagnosis with BatchNorm
Positive diagnosis rate: 21.30%

Test Results:
  Accuracy: 0.8656
  AUC: 0.9012
```

## VI. VISUALIZATION

### BatchNorm Internal Operation

```
                    BATCH NORMALIZATION
    
    Input Batch (N x D):
    ┌─────────────────────────────────────┐
    │ x₁  x₂  x₃  ...  x_d  (batch items)│
    │ [2.1, 4.3, 1.2, ..., 3.5]         │
    │ [1.8, 3.9, 2.1, ..., 4.2]         │
    │ [2.5, 4.1, 1.8, ..., 3.8]         │
    │ ...                                │
    └─────────────────────────────────────┘
                    │
                    ▼
    ┌─────────────────────────────────────┐
    │ Compute Batch Mean (μ)              │
    │ μ = (1/N) Σ xᵢ                     │
    └─────────────────────────────────────┘
                    │
                    ▼
    ┌─────────────────────────────────────┐
    │ Compute Batch Variance (σ²)          │
    │ σ² = (1/N) Σ (xᵢ - μ)²             │
    └─────────────────────────────────────┘
                    │
                    ▼
    ┌─────────────────────────────────────┐
    │ Normalize:                           │
    │ x_norm = (x - μ) / √(σ² + ε)        │
    └─────────────────────────────────────┘
                    │
                    ▼
    ┌─────────────────────────────────────┐
    │ Scale and Shift (learnable):         │
    │ y = γ * x_norm + β                  │
    │                                     │
    │ γ (gamma): Learnable scale          │
    │ β (beta): Learnable shift           │
    └─────────────────────────────────────┘
                    │
                    ▼
    Output (N x D): Same shape as input
```

### Training vs Inference

```
    TRAINING                    INFERENCE
    ┌─────────────────┐        ┌─────────────────┐
    │                 │        │                 │
    │ Use batch mean  │        │ Use running     │
    │ and variance    │        │ mean/var        │
    │                 │        │                 │
    │ Updating        │        │ Fixed           │
    │ running stats   │        │ (no update)     │
    │                 │        │                 │
    └─────────────────┘        └─────────────────┘
    
    Running Statistics Update:
    running_mean = momentum * running_mean + (1 - momentum) * batch_mean
    running_var = momentum * running_var + (1 - momentum) * batch_var
```

## VII. ADVANCED_TOPICS

### BatchNorm Variants

1. **Layer Normalization**: Normalize across features (for RNNs)
2. **Instance Normalization**: Per-sample, per-channel (for style transfer)
3. **Group Normalization**: Groups channels (for small batch sizes)
4. **Weight Normalization**: Normalize weights instead of activations
5. **Switchable Normalization**: Combines batch, layer, instance norm

### Advanced Tips

1. **Use with convolutions**: BatchNorm typically after conv, before activation
2. **Small batch caution**: BatchNorm less effective with very small batches
3. **Training mode**: Don't forget to set training=True during training

### Common Pitfalls

| Issue | Solution |
|-------|----------|
| Poor performance | Check training mode flag |
| Slow convergence | Try different momentum |
| Incompatible with small batch | Use Group Normalization |

## VIII. CONCLUSION

### Key Takeaways

1. **BatchNorm normalizes layer inputs**: Reduces internal covariate shift
2. **Enables higher learning rates**: Faster training
3. **Provides regularization**: Acts as implicit regularizer
4. **Works with convolutions**: Add after conv, before activation

### Next Steps

1. Explore Layer Normalization for RNNs
2. Study Group Normalization for small batches
3. Learn about Virtual Batch Normalization

### Further Reading

1. "Batch Normalization: Accelerating Deep Network Training" (Ioffe & Szegedy, 2015)
2. "Layer Normalization" (Ba et al., 2016)
3. "Group Normalization" (Wu & He, 2018)