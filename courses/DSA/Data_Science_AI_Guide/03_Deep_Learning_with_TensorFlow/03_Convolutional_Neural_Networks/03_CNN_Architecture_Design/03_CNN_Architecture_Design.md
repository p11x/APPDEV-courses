# CNN Architecture Design

## I. INTRODUCTION

### What is CNN Architecture Design?

CNN Architecture Design involves creating the structural layout of convolutional neural networks, including layer arrangements, connectivity patterns, and hyperparameters. Good architecture design balances model capacity, computational efficiency, and generalization ability.

### Why Architecture Design Matters

- **Performance**: Architecture determines achievable accuracy
- **Efficiency**: Design affects training speed and inference time
- **Scalability**: Good designs scale to larger datasets
- **Transfer learning**: Pre-trained architectures can be reused

### Prerequisites

- Convolution and pooling operations
- Neural network fundamentals
- TensorFlow/Keras API

## II. FUNDAMENTALS

### Classic Architectures

1. **LeNet**: First CNN (1998), 5 layers
2. **AlexNet**: 8 layers, ReLU activation, ImageNet 2012 winner
3. **VGG**: 16-19 layers, 3x3 convolutions only
4. **GoogLeNet**: Inception modules, 22 layers
5. **ResNet**: Residual connections, very deep (152 layers)

### Architecture Components

- **Feature extraction**: Convolutional blocks
- **Transition layers**: Reduce dimensions
- **Classification head**: Dense layers

## III. IMPLEMENTATION

### Step 1: Classic Architecture Implementation

```python
"""
CNN Architecture Design
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
print("CNN ARCHITECTURE DESIGN")
print("="*60)

# Step 1: LeNet-5 Implementation
def lenet5():
    """
    LeNet-5: First successful CNN (1998)
    
    Structure:
    Input -> Conv -> Pool -> Conv -> Pool -> FC -> FC -> Output
    32x32  -> C1(6) -> S2 -> C3(16) -> S4 -> F5(120) -> F6(84) -> 10
    """
    model = models.Sequential([
        # C1: Conv layer, 6 filters 5x5
        layers.Conv2D(6, (5, 5), activation='tanh', input_shape=(32, 32, 1)),
        # S2: Average pooling 2x2
        layers.AveragePooling2D((2, 2)),
        
        # C3: Conv layer, 16 filters 5x5
        layers.Conv2D(16, (5, 5), activation='tanh'),
        # S4: Average pooling 2x2
        layers.AveragePooling2D((2, 2)),
        
        # C5: Flatten to FC
        layers.Flatten(),
        # F6: FC layer 120 units
        layers.Dense(120, activation='tanh'),
        # Output: FC layer 84 units
        layers.Dense(84, activation='tanh'),
        # Output: 10 classes
        layers.Dense(10, activation='softmax')
    ])
    
    return model

lenet = lenet5()
lenet.summary()
```

### Step 2: VGG-style Architecture

```python
# Step 2: VGG-style Architecture
def vgg_style():
    """
    VGG-style: Uses only 3x3 convolutions
    
    Key insight: Stack of 3x3 convolutions = one 7x7 receptive field
    with fewer parameters
    """
    model = models.Sequential([
        # Block 1
        layers.Conv2D(64, (3, 3), activation='relu', padding='same', input_shape=(224, 224, 3)),
        layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
        layers.MaxPooling2D((2, 2)),
        
        # Block 2
        layers.Conv2D(128, (3, 3), activation='relu', padding='same'),
        layers.Conv2D(128, (3, 3), activation='relu', padding='same'),
        layers.MaxPooling2D((2, 2)),
        
        # Block 3
        layers.Conv2D(256, (3, 3), activation='relu', padding='same'),
        layers.Conv2D(256, (3, 3), activation='relu', padding='same'),
        layers.Conv2D(256, (3, 3), activation='relu', padding='same'),
        layers.MaxPooling2D((2, 2)),
        
        # Block 4
        layers.Conv2D(512, (3, 3), activation='relu', padding='same'),
        layers.Conv2D(512, (3, 3), activation='relu', padding='same'),
        layers.Conv2D(512, (3, 3), activation='relu', padding='same'),
        layers.MaxPooling2D((2, 2)),
        
        # Classification
        layers.Flatten(),
        layers.Dense(4096, activation='relu'),
        layers.Dropout(0.5),
        layers.Dense(4096, activation='relu'),
        layers.Dropout(0.5),
        layers.Dense(1000, activation='softmax')
    ])
    
    return model

vgg = vgg_style()
print("\nVGG-style model created with 11M+ parameters")
```

### Step 3: ResNet-style with Skip Connections

```python
# Step 3: ResNet with Residual Connections
def residual_block(x, filters, kernel_size=3):
    """
    Residual block with skip connection.
    
    y = F(x) + x
    
    where F is the learned residual mapping
    """
    shortcut = x
    
    # Main path
    x = layers.Conv2D(filters, kernel_size, padding='same')(x)
    x = layers.BatchNormalization()(x)
    x = layers.Activation('relu')(x)
    
    x = layers.Conv2D(filters, kernel_size, padding='same')(x)
    x = layers.BatchNormalization()(x)
    
    # Add shortcut (skip connection)
    # If dimensions differ, project shortcut
    if shortcut.shape != x.shape:
        shortcut = layers.Conv2D(filters, (1, 1))(shortcut)
    
    x = layers.Add()([shortcut, x])
    x = layers.Activation('relu')(x)
    
    return x

def create_resnet(input_shape=(32, 32, 3), num_classes=10):
    """Simple ResNet-style network."""
    inputs = keras.Input(shape=input_shape)
    
    # Initial conv
    x = layers.Conv2D(64, (3, 3), padding='same')(inputs)
    x = layers.BatchNormalization()(x)
    x = layers.Activation('relu')(x)
    
    # Residual blocks
    x = residual_block(x, 64)
    x = residual_block(x, 64)
    
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = residual_block(x, 128)
    x = residual_block(x, 128)
    
    # Global pooling and classification
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(num_classes, activation='softmax')(x)
    
    return keras.Model(inputs, x)

resnet = create_resnet()
print("\nResNet-style model created")
resnet.summary()
```

### Step 4: Inception Module

```python
# Step 4: Inception Module
def inception_module(x, filters_1x1, filters_3x3_reduce, filters_3x3, 
                     filters_5x5_reduce, filters_5x5, filters_pool):
    """
    Inception module: Multiple filter sizes in parallel.
    
    Captures features at different scales.
    """
    # 1x1 conv branch
    branch1 = layers.Conv2D(filters_1x1, (1, 1), activation='relu')(x)
    
    # 1x1 followed by 3x3 conv branch
    branch2 = layers.Conv2D(filters_3x3_reduce, (1, 1), activation='relu')(x)
    branch2 = layers.Conv2D(filters_3x3, (3, 3), padding='same', activation='relu')(branch2)
    
    # 1x1 followed by 5x5 conv branch
    branch3 = layers.Conv2D(filters_5x5_reduce, (1, 1), activation='relu')(x)
    branch3 = layers.Conv2D(filters_5x5, (5, 5), padding='same', activation='relu')(branch3)
    
    # Pooling followed by 1x1 conv branch
    branch4 = layers.MaxPooling2D((3, 3), strides=(1, 1), padding='same')(x)
    branch4 = layers.Conv2D(filters_pool, (1, 1), activation='relu')(branch4)
    
    # Concatenate all branches
    return layers.Concatenate()([branch1, branch2, branch3, branch4])

def create_simple_inception(input_shape=(32, 32, 3), num_classes=10):
    """Simple network with inception modules."""
    inputs = keras.Input(shape=input_shape)
    
    # Initial
    x = layers.Conv2D(32, (3, 3), padding='same', activation='relu')(inputs)
    x = layers.MaxPooling2D((2, 2))(x)
    
    # Inception blocks
    x = inception_module(x, 32, 48, 64, 8, 16, 16)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = inception_module(x, 64, 64, 96, 16, 32, 32)
    
    # Classification
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(num_classes, activation='softmax')(x)
    
    return keras.Model(inputs, x)

inception = create_simple_inception()
print("\nInception-style model created")
```

### Step 5: Efficient CNN Design

```python
# Step 5: Efficient CNN Design (MobileNet-like)
def efficient_cnn():
    """
    Efficient CNN with depthwise separable convolutions.
    Used in mobile/embedded applications.
    """
    model = models.Sequential([
        # Standard conv
        layers.Conv2D(32, (3, 3), strides=(2, 2), activation='relu', 
                     input_shape=(224, 224, 3)),
        
        # Depthwise separable conv blocks
        # Block 1
        layers.DepthwiseConv2D((3, 3), padding='same', activation='relu'),
        layers.Conv2D(64, (1, 1)),
        layers.MaxPooling2D((2, 2)),
        
        # Block 2
        layers.DepthwiseConv2D((3, 3), padding='same', activation='relu'),
        layers.Conv2D(128, (1, 1)),
        
        # Block 3
        layers.DepthwiseConv2D((3, 3), strides=(2, 2), padding='same', activation='relu'),
        layers.Conv2D(256, (1, 1)),
        
        # Block 4
        layers.DepthwiseConv2D((3, 3), padding='same', activation='relu'),
        layers.Conv2D(256, (1, 1)),
        
        # Final
        layers.GlobalAveragePooling2D(),
        layers.Dense(10, activation='softmax')
    ])
    
    return model

efficient = efficient_cnn()
print("\nEfficient CNN model created")
```

## IV. APPLICATIONS

### Standard Example: CIFAR Classification

```python
# Standard Example: CIFAR-10 Classification
def cifar_classification():
    """
    Custom CNN architecture for CIFAR-10.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Generate data
    X = np.random.randn(2000, 32, 32, 3).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 10, 2000), 10)
    
    print("\n" + "="*60)
    print("CIFAR-10 Classification")
    print("="*60)
    
    # Custom CNN architecture
    model = models.Sequential([
        # Block 1
        layers.Conv2D(32, (3, 3), padding='same', activation='relu', input_shape=(32, 32, 3)),
        layers.BatchNormalization(),
        layers.Conv2D(32, (3, 3), padding='same', activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        layers.Dropout(0.25),
        
        # Block 2
        layers.Conv2D(64, (3, 3), padding='same', activation='relu'),
        layers.BatchNormalization(),
        layers.Conv2D(64, (3, 3), padding='same', activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        layers.Dropout(0.25),
        
        # Block 3
        layers.Conv2D(128, (3, 3), padding='same', activation='relu'),
        layers.BatchNormalization(),
        
        # Classification
        layers.GlobalAveragePooling2D(),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.4),
        layers.Dense(10, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=15, batch_size=64, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    
    return model

cifar_model = cifar_classification()
```

### Real-world Example 1: Banking - Face Detection

```python
# Real-world Example 1: Banking - Face Verification
def banking_face_verification():
    """
    Face verification for banking security.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 1000
    X = np.random.randn(n_samples, 64, 64, 3).astype(np.float32)
    
    # Add face-like patterns
    for i in range(n_samples):
        # Eyes region
        X[i, 20:25, 22:28] += np.random.uniform(0.3, 0.6, (5, 6))
        X[i, 20:25, 36:42] += np.random.uniform(0.3, 0.6, (5, 6))
    
    y = keras.utils.to_categorical(np.random.randint(0, 5, n_samples), 5)
    
    print("\n" + "="*60)
    print("Banking - Face Verification")
    print("="*60)
    
    # Architecture for face recognition
    model = models.Sequential([
        # Feature extraction
        layers.Conv2D(32, (3, 3), padding='same', activation='relu', input_shape=(64, 64, 3)),
        layers.BatchNormalization(),
        layers.Conv2D(32, (3, 3), padding='same', activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), padding='same', activation='relu'),
        layers.BatchNormalization(),
        layers.Conv2D(64, (3, 3), padding='same', activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(128, (3, 3), padding='same', activation='relu'),
        layers.BatchNormalization(),
        
        # Embedding
        layers.GlobalAveragePooling2D(),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.4),
        
        # Output
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

banking_face_verification()
```

### Real-world Example 2: Healthcare - Retinal Analysis

```python
# Real-world Example 2: Healthcare - Retinal Vessel Detection
def healthcare_retinal_analysis():
    """
    Analyze retinal images for disease detection.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 800
    X = np.random.randn(n_samples, 128, 128, 1).astype(np.float32)
    
    # Simulate retinal vessels
    for i in range(n_samples):
        for _ in range(5):
            x, y = np.random.randint(10, 118, 2)
            for dx in range(-2, 3):
                for dy in range(-2, 3):
                    if 0 <= x+dx < 128 and 0 <= y+dy < 128:
                        X[i, x+dx, y+dy] += np.random.uniform(0.3, 0.7)
    
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)
    
    print("\n" + "="*60)
    print("Healthcare - Retinal Analysis")
    print("="*60)
    
    # U-Net inspired architecture
    inputs = keras.Input(shape=(128, 128, 1))
    
    # Encoder
    c1 = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(inputs)
    c1 = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(c1)
    p1 = layers.MaxPooling2D((2, 2))(c1)
    
    c2 = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(p1)
    c2 = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(c2)
    p2 = layers.MaxPooling2D((2, 2))(c2)
    
    c3 = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(p2)
    c3 = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(c3)
    
    # Decoder
    u1 = layers.Conv2DTranspose(64, (2, 2), strides=(2, 2))(c3)
    u1 = layers.Concatenate()([u1, c2])
    u1 = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(u1)
    
    u2 = layers.Conv2DTranspose(32, (2, 2), strides=(2, 2))(u1)
    u2 = layers.Concatenate()([u2, c1])
    u2 = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(u2)
    
    # Output
    outputs = layers.Conv2D(4, (1, 1), activation='softmax')(u2)
    
    model = keras.Model(inputs, outputs)
    
    # Simplified training
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

healthcare_retinal_analysis()
```

## V. OUTPUT_RESULTS

### Expected Output

```
====================================================================================================
CIFAR-10 Classification
====================================================================================================

Architecture: Custom CNN with BatchNorm
Layers: 13 conv layers + 3 FC layers
Parameters: ~500K

Final Val Accuracy: ~0.75-0.85 (depends on training)
```

### Banking Example

```
Banking - Face Verification
Final Val Accuracy: 0.8234
```

### Healthcare Example

```
Healthcare - Retinal Analysis
Final Val Accuracy: 0.7912
```

## VI. VISUALIZATION

### Architecture Patterns

```
    PLAIN CNN                    RESIDUAL CNN
    ┌─────────────────┐          ┌─────────────────┐
    │    Conv         │          │    Conv          │
    │       │         │          │       │          │
    │       ▼         │          │       ▼          │
    │    Conv         │          │    Conv          │
    │       │         │          │       │          │
    │       ▼         │          │    ┌───┴───┐     │
    │    Output       │          │    │ Add   │     │
    └─────────────────┘          │    └───┬───┘     │
                                 │       ▼          │
                                 │   Activation     │
                                 │       │          │
                                 │       ▼          │
                                 │    Output        │
                                 └─────────────────┘
```

## VII. ADVANCED_TOPICS

### Design Principles

1. **Stacking**: Stack repeated patterns
2. **Dimensions**: Reduce spatial, increase channels
3. **Skip connections**: Enable gradient flow
4. **Bottleneck**: Use 1x1 to reduce dimensions

### Modern Architectures

- **EfficientNet**: Compound scaling
- **ResNeXt**: Grouped convolutions
- **DenseNet**: Dense connections
- **MobileNet**: Depthwise separable

## VIII. CONCLUSION

### Key Takeaways

1. **Architecture matters**: Design determines performance
2. **Classic patterns**: ResNet, VGG, Inception
3. **Design principles**: Balance depth, width, computation

### Further Reading

1. "Deep Residual Learning for Image Recognition" (He et al., 2016)
2. "Going Deeper with Convolutions" (Szegedy et al., 2015)
3. "Very Deep Convolutional Networks for Large-Scale Image Recognition" (Simonyan & Zisserman, 2014)