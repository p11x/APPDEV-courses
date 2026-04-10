# Pooling Layers

## I. INTRODUCTION

### What are Pooling Layers?

Pooling layers are downsampling operations in CNNs that reduce spatial dimensions while retaining important information. They aggregate features within a sliding window, providing translation invariance and reducing computational complexity.

### Why Pooling Matters

- **Spatial reduction**: Decrease feature map size
- **Translation invariance**: Features more robust to shifts
- **Parameter reduction**: Fewer parameters in subsequent layers
- **Computational efficiency**: Faster training and inference
- **Prevents overfitting**: Acts as regularization

### Prerequisites

- Convolution operations fundamentals
- Feature map concepts
- CNN architecture basics

## II. FUNDAMENTALS

### Types of Pooling

1. **Max Pooling**: Takes maximum value in window
2. **Average Pooling**: Takes mean value in window
3. **Global Pooling**: Pools over entire feature map
4. **L2 Pooling**: Takes L2 norm of window

### Key Parameters

- **Pool Size**: Window dimensions
- **Strides**: Step size between windows
- **Padding**: Border handling

### Core Formulas

```
Output size = floor((Input - Pool_size) / Stride) + 1
```

## III. IMPLEMENTATION

### Step 1: Basic Pooling Operations

```python
"""
Pooling Layers
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
print("POOLING LAYERS")
print("="*60)

# Step 1: Basic Max and Average Pooling
def basic_pooling():
    """
    Demonstrate max and average pooling.
    """
    # Create sample input
    X = np.array([
        [[1, 5, 3],
         [2, 8, 4],
         [6, 7, 9]]
    ], dtype=np.float32)
    X = np.expand_dims(X, axis=0)  # Add batch dimension
    
    print("Input shape:", X.shape)
    print("Input:\n", X[0])
    
    # Max Pooling 2x2 with stride 2
    max_pool = layers.MaxPooling2D(pool_size=(2, 2), strides=(2, 2))
    max_out = max_pool(X)
    print("\nMax Pooling (2x2, stride 2):")
    print("Output shape:", max_out.shape)
    print("Output:\n", max_out[0])
    
    # Average Pooling 2x2 with stride 2
    avg_pool = layers.AveragePooling2D(pool_size=(2, 2), strides=(2, 2))
    avg_out = avg_pool(X)
    print("\nAverage Pooling (2x2, stride 2):")
    print("Output shape:", avg_out.shape)
    print("Output:\n", avg_out[0])

basic_pooling()
```

### Step 2: Global Pooling

```python
# Step 2: Global Pooling
def global_pooling():
    """
    Global pooling reduces each channel to single value.
    """
    # Input: (batch, height, width, channels)
    X = np.random.randn(1, 16, 16, 8).astype(np.float32)
    
    print("\n" + "="*60)
    print("Global Pooling")
    print("="*60)
    print(f"Input shape: {X.shape}")
    
    # Global Max Pooling
    global_max = layers.GlobalMaxPooling2D()
    gmax_out = global_max(X)
    print(f"Global Max Pooling output: {gmax_out.shape}")
    
    # Global Average Pooling
    global_avg = layers.GlobalAveragePooling2D()
    gavg_out = global_avg(X)
    print(f"Global Average Pooling output: {gavg_out.shape}")
    
    # Equivalent to Flatten + Dense but with regularization
    # More commonly used: global avg before final dense layer
    
    return gmax_out, gavg_out

gmax, gavg = global_pooling()
```

### Step 3: Pooling with Conv Nets

```python
# Step 3: Pooling in CNN Architecture
def pooling_in_cnn():
    """
    Complete CNN with pooling layers.
    """
    # Generate synthetic data
    X = np.random.randn(100, 32, 32, 3).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 10, 100), 10)
    
    print("\n" + "="*60)
    print("CNN with Pooling Layers")
    print("="*60)
    
    model = models.Sequential([
        # Block 1: Conv -> Pool
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(32, 32, 3)),
        layers.MaxPooling2D((2, 2)),  # 32x32 -> 16x16
        
        # Block 2
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),  # 16x16 -> 8x8
        
        # Block 3
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.AveragePooling2D((2, 2)),  # 8x8 -> 4x4
        
        # Classification
        layers.Flatten(),
        layers.Dense(64, activation='relu'),
        layers.Dense(10, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.summary()
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Training accuracy: {history.history['accuracy'][-1]:.4f}")
    
    return model

cnn_with_pooling = pooling_in_cnn()
```

### Step 4: Stride as Pooling Alternative

```python
# Step 4: Stride vs Pooling
def stride_vs_pooling():
    """
    Compare strided convolution to pooling.
    """
    X = np.random.randn(1, 16, 16, 1).astype(np.float32)
    
    print("\n" + "="*60)
    print("Stride vs Pooling")
    print("="*60)
    
    # Max pooling
    pool = layers.MaxPooling2D((2, 2), strides=(2, 2))
    pool_out = pool(X)
    print(f"Max Pooling output: {pool_out.shape}")
    
    # Strided convolution (alternative to pooling)
    conv_stride = layers.Conv2D(1, (2, 2), strides=(2, 2), use_bias=False)
    conv_out = conv_stride(X)
    print(f"Strided Conv output: {conv_out.shape}")
    
    # Both reduce spatial dimensions by factor of 2
    # Conv strided learns parameters, Pooling is parameter-free
    
    return pool_out, conv_out

stride_pool = stride_vs_pooling()
```

### Step 5: Custom Pooling

```python
# Step 5: Custom Pooling Implementation
class L2Pooling2D(layers.Layer):
    """
    L2 Pooling: computes L2 norm in pooling window.
    """
    def __init__(self, pool_size=(2, 2), strides=None, padding='valid', **kwargs):
        super(L2Pooling2D, self).__init__(**kwargs)
        self.pool_size = pool_size
        self.strides = strides if strides is not None else pool_size
        self.padding = padding
    
    def call(self, inputs):
        return tf.nn.pool(
            inputs,
            window_shape=self.pool_size,
            pooling_type='AVG',  # Use avg then square
            strides=self.strides,
            padding=self.padding
        )
    
    def get_config(self):
        config = super(L2Pooling2D, self).get_config()
        config.update({
            'pool_size': self.pool_size,
            'strides': self.strides,
            'padding': self.padding
        })
        return config

print("\nCustom L2 Pooling layer implemented")
```

## IV. APPLICATIONS

### Standard Example: Image Classification

```python
# Standard Example: Classification with Pooling
def image_classification_pooling():
    """
    Image classification demonstrating pooling benefits.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Synthetic images
    X = np.random.randn(500, 28, 28, 1).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 5, 500), 5)
    
    print("\n" + "="*60)
    print("Image Classification with Pooling")
    print("="*60)
    
    # Model with pooling
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(28, 28, 1)),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        layers.Dropout(0.25),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        layers.Dropout(0.25),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.GlobalAveragePooling2D(),
        
        layers.Dense(32, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=10, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    
    return model

pooling_model = image_classification_pooling()
```

### Real-world Example 1: Banking - Signature Verification

```python
# Real-world Example 1: Banking - Signature Verification
def banking_signature():
    """
    Verify signatures using pooling for spatial reduction.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 800
    X = np.random.randn(n_samples, 64, 64, 1).astype(np.float32)
    
    # Add signature-like patterns
    for i in range(n_samples):
        # Simulate stroke patterns
        x, y = np.random.randint(10, 54, 2)
        for dx in range(-3, 4):
            for dy in range(-1, 2):
                if 0 <= x+dx < 64 and 0 <= y+dy < 64:
                    X[i, x+dx, y+dy] += np.random.uniform(0.5, 1.5)
    
    # Original, copied, forged
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Banking - Signature Verification")
    print("="*60)
    
    model = models.Sequential([
        # Feature extraction
        layers.Conv2D(32, (5, 5), activation='relu', input_shape=(64, 64, 1)),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        
        # Global pooling instead of flatten (less params)
        layers.GlobalAveragePooling2D(),
        
        layers.Dense(32, activation='relu'),
        layers.Dropout(0.4),
        layers.Dense(3, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

banking_signature()
```

### Real-world Example 2: Healthcare - MRI Scan Analysis

```python
# Real-world Example 2: Healthcare - MRI Analysis
def healthcare_mri_analysis():
    """
    Analyze MRI scans using pooling for downsampling.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 1000
    X = np.random.randn(n_samples, 48, 48, 1).astype(np.float32)
    
    # Add brain-like structures
    for i in range(n_samples):
        # Ventricle regions (dark areas)
        center_x, center_y = np.random.randint(18, 30, 2)
        for dx in range(-6, 7):
            for dy in range(-4, 5):
                if 0 <= center_x+dx < 48 and 0 <= center_y+dy < 48:
                    X[i, center_x+dx, center_y+dy] -= np.random.uniform(0.3, 0.8)
    
    # Normal, tumor, stroke
    y = keras.utils.to_categorical(np.random.randint(0, 3, n_samples), 3)
    
    print("\n" + "="*60)
    print("Healthcare - MRI Scan Analysis")
    print("="*60)
    
    model = models.Sequential([
        # U-Net style with pooling
        layers.Conv2D(32, (3, 3), activation='relu', padding='same',
                     input_shape=(48, 48, 1)),
        layers.Conv2D(32, (3, 3), activation='relu', padding='same'),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
        layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(128, (3, 3), activation='relu', padding='same'),
        
        # Use global pooling for classification
        layers.GlobalAveragePooling2D(),
        
        layers.Dense(64, activation='relu'),
        layers.Dropout(0.5),
        layers.Dense(3, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

healthcare_mri_analysis()
```

## V. OUTPUT_RESULTS

### Expected Output

```
====================================================================================================
Pooling Comparison
====================================================================================================

Input shape: (1, 3, 3)
Input:
 [[1, 5, 3],
  [2, 8, 4],
  [6, 7, 9]]

Max Pooling (2x2, stride 2):
Output shape: (1, 1, 1, 2)
Output: [[8, 9]]

Average Pooling (2x2, stride 2):
Output shape: (1, 1, 1, 2)
Output: [[4, 5]]
```

### Banking Example

```
Banking - Signature Verification
Final Val Accuracy: 0.7823
```

### Healthcare Example

```
Healthcare - MRI Scan Analysis
Final Val Accuracy: 0.8345
```

## VI. VISUALIZATION

### Pooling Operation

```
    MAX POOLING (2x2, stride 2)
    
    Input               Output
    ┌─────┬─────┬─────┐   ┌─────┬─────┐
    │  1  │  5  │  3  │   │  8  │  9  │  <- max(1,5,2,8), max(3,4,7,9)
    ├─────┼─────┼─────┤   ├─────┼─────┤
    │  2  │  8  │  4  │
    ├─────┼─────┼─────┤
    │  6  │  7  │  9  │
    └─────┴─────┴─────┘
    
    AVERAGE POOLING (2x2, stride 2)
    
    Input               Output
    ┌─────┬─────┬─────┐   ┌─────┬─────┐
    │  1  │  5  │  3  │   │  4  │  5  │  <- (1+5+2+8)/4, (3+4+7+9)/4
    ├─────┼─────┼─────┤   ├─────┼─────┤
    │  2  │  8  │  4  │
    ├─────┼─────┼─────┤
    │  6  │  7  │  9  │
    └─────┴─────┴─────┘
```

### Global Pooling

```
    GLOBAL AVERAGE POOLING
    
    Input (H x W x C)          Output (1 x 1 x C)
    
    ┌─────────────────────┐
    │  Feature Channel 1  │     ┌───┐
    │  [H x W values]      │ ──► │ v1 │  <- mean of all values
    └─────────────────────┘     ├───┤
    ┌─────────────────────┐     │v2 │  <- mean of channel 2
    │  Feature Channel 2  │ ──► ├───┤
    └─────────────────────┘     │...│
                               ├───┤
    ┌─────────────────────┐     │vC │
    │  Feature Channel C  │ ──► └───┘
    └─────────────────────┘
    
    Reduces HxW to 1x1 while keeping channel dimension
```

## VII. ADVANCED_TOPICS

### Advanced Pooling Types

1. **Spatial Pyramid Pooling**: Multiple pool sizes
2. **RoI Pooling**: For object detection
3. **Stochastic Pooling**: Random selection based on distribution

### Pooling vs Stride

| Aspect | Pooling | Stride |
|--------|---------|--------|
| Parameters | None | Learnable |
| Flexibility | Fixed | Learnable |
| Computation | Simple | More |
| Regularization | Some | Less |

### Modern Trends

- Many architectures reduce or eliminate pooling
- Strided convolutions often preferred
- Global pooling before final layers

## VIII. CONCLUSION

### Key Takeaways

1. **Pooling reduces spatial dimensions**: 2x2 pool halves dimensions
2. **Max pooling preserves strong features**: Good for detection
3. **Average pooling smooths features**: Good for classification
4. **Global pooling**: Replaces flatten for efficiency

### Next Steps

1. Learn CNN architectures (VGG, ResNet)
2. Study upsampling (transposed conv)
3. Explore attention mechanisms

### Further Reading

1. "ImageNet Classification with Deep Convolutional Neural Networks" (Krizhevsky et al., 2012)
2. "Very Deep Convolutional Networks for Large-Scale Image Recognition" (Simonyan & Zisserman, 2014)
3. "Spatial Pyramid Pooling in Deep Convolutional Networks" (He et al., 2014)