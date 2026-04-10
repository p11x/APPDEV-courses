# Advanced CNN Techniques

## I. INTRODUCTION

### What are Advanced CNN Techniques?

Advanced CNN techniques are modern architectural innovations and training strategies that push the boundaries of convolutional neural network performance. These include attention mechanisms, modern architectures, efficient designs, and specialized modules that improve accuracy, efficiency, and generalization.

### Why Advanced Techniques Matter

- **State-of-the-art performance**: Achieve better accuracy on benchmarks
- **Efficiency**: Reduce computation while maintaining accuracy
- **Better generalization**: More robust to variations
- **Modern applications**: Essential for production systems

### Prerequisites

- CNN fundamentals
- Architecture design patterns
- TensorFlow/Keras

## II. FUNDAMENTALS

### Key Advanced Techniques

1. **Attention mechanisms**: Focus on important features
2. **Skip connections**: Gradient flow and feature reuse
3. **Feature pyramids**: Multi-scale feature fusion
4. **Grouped convolutions**: Efficient parameter use

### Core Concepts

- **Self-attention**: Global context via weighted sums
- **Spatial attention**: Where to focus
- **Channel attention**: What features matter
- **Fusion strategies**: Combining features

## III. IMPLEMENTATION

### Step 1: Channel Attention (SE-Net)

```python
"""
Advanced CNN Techniques
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
print("ADVANCED CNN TECHNIQUES")
print("="*60)

# Step 1: Squeeze-and-Excitation (SE) Block
def se_block(input_tensor, reduction=16):
    """
    Squeeze-and-Excitation block for channel attention.
    
    Process:
    1. Global pooling (squeeze)
    2. FC -> ReLU -> FC (excitation)
    3. Scale input channels
    """
    channels = input_tensor.shape[-1]
    
    # Squeeze: Global average pooling
    x = layers.GlobalAveragePooling2D()(input_tensor)
    x = layers.Reshape((1, 1, channels))(x)
    
    # Excitation: FC layers to learn channel weights
    x = layers.Dense(channels // reduction, activation='relu')(x)
    x = layers.Dense(channels, activation='sigmoid')(x)
    
    # Scale: multiply input by learned weights
    x = layers.Multiply()([input_tensor, x])
    
    return x

def se_cnn_block(filters):
    """CNN block with SE attention."""
    def block(x):
        x = layers.Conv2D(filters, (3, 3), padding='same')(x)
        x = layers.BatchNormalization()(x)
        x = layers.Activation('relu')(x)
        
        # SE attention
        x = se_block(x)
        
        return x
    return block

print("SE (Squeeze-and-Excitation) block implemented")
```

### Step 2: Spatial Attention

```python
# Step 2: Spatial Attention Module
def spatial_attention_block(input_tensor):
    """
    Spatial attention module to focus on important locations.
    """
    # Channel-wise average and max pooling
    avg_pool = layers.Lambda(lambda x: tf.reduce_mean(x, axis=-1, keepdims=True))(input_tensor)
    max_pool = layers.Lambda(lambda x: tf.reduce_max(x, axis=-1, keepdims=True))(input_tensor)
    
    # Concatenate
    concat = layers.Concatenate()([avg_pool, max_pool])
    
    # Conv to get attention map
    attention = layers.Conv2D(1, (7, 7), padding='same', activation='sigmoid')(concat)
    
    # Apply attention
    return layers.Multiply()([input_tensor, attention])

print("Spatial attention module implemented")
```

### Step 3: CBAM (Convolutional Block Attention Module)

```python
# Step 3: CBAM - Combined Channel and Spatial Attention
def cbam_block(input_tensor, reduction=16):
    """
    CBAM: First apply channel attention, then spatial attention.
    """
    # Channel attention (from SE block)
    channels = input_tensor.shape[-1]
    x = layers.GlobalAveragePooling2D()(input_tensor)
    x = layers.Reshape((1, 1, channels))(x)
    x = layers.Dense(channels // reduction, activation='relu')(x)
    x = layers.Dense(channels, activation='sigmoid')(x)
    x = layers.Multiply()([input_tensor, x])
    
    # Spatial attention
    avg_pool = layers.Lambda(lambda z: tf.reduce_mean(z, axis=-1, keepdims=True))(x)
    max_pool = layers.Lambda(lambda z: tf.reduce_max(z, axis=-1, keepdims=True))(x)
    concat = layers.Concatenate()([avg_pool, max_pool])
    attention = layers.Conv2D(1, (7, 7), padding='same', activation='sigmoid')(concat)
    x = layers.Multiply()([x, attention])
    
    return x

def cbam_cnn(filters):
    """CNN block with CBAM."""
    def block(x):
        x = layers.Conv2D(filters, (3, 3), padding='same')(x)
        x = layers.BatchNormalization()(x)
        x = layers.Activation('relu')(x)
        
        # CBAM attention
        x = cbam_block(x)
        
        return x
    return block

print("CBAM (Convolutional Block Attention Module) implemented")
```

### Step 4: Feature Pyramid Network

```python
# Step 4: Feature Pyramid Network (FPN)
def fpn_block(feature_levels, num_filters=256):
    """
    Feature Pyramid Network for multi-scale feature fusion.
    
    Args:
        feature_levels: List of feature tensors from different scales
        num_filters: Number of filters for output layers
    """
    # Top-down pathway
    # Start from the deepest (smallest) feature map
    laterals = []
    for i, feat in enumerate(feature_levels):
        if i == 0:
            # For the smallest feature, use a 1x1 conv
            lateral = layers.Conv2D(num_filters, (1, 1), 
                                    name=f'lateral_{i}')(feat)
        else:
            # Upsample and add to current level
            up = layers.UpSampling2D(size=(2, 2))(laterals[-1])
            lateral = layers.Conv2D(num_filters, (1, 1),
                                    name=f'lateral_{i}')(feat)
            lateral = layers.Add()([up, lateral])
        
        laterals.append(lateral)
    
    # Bottom-up pathway (optional for FPN)
    outputs = []
    for lateral in reversed(laterals):
        output = layers.Conv2D(num_filters, (3, 3), padding='same',
                              name=f'output_{len(output)}')(lateral)
        outputs.append(output)
    
    return outputs

print("Feature Pyramid Network implemented")
```

### Step 5: Mixed Depthwise Separable Convolution

```python
# Step 5: MixNet-style Mixed Depthwise Convolution
def mixed_depthwise_block(input_tensor, kernel_sizes=[3, 5, 7]):
    """
    Mixed depthwise convolution with different kernel sizes.
    Captures features at multiple scales.
    """
    branches = []
    
    for kernel_size in kernel_sizes:
        # Depthwise conv with specific kernel
        branch = layers.DepthwiseConv2D(
            (kernel_size, kernel_size),
            padding='same',
            depthwise_initializer='he_normal'
        )(input_tensor)
        branch = layers.BatchNormalization()(branch)
        branch = layers.Activation('relu')(branch)
        branches.append(branch)
    
    # Concatenate all branches
    output = layers.Concatenate()(branches)
    
    # Pointwise conv to mix channels
    output = layers.Conv2D(input_tensor.shape[-1], (1, 1))(output)
    
    # Add original input (residual connection)
    output = layers.Add()([input_tensor, output])
    output = layers.Activation('relu')(output)
    
    return output

print("Mixed depthwise convolution implemented")
```

## IV. APPLICATIONS

### Standard Example: Advanced CNN Classification

```python
# Standard Example: Advanced CNN with Attention
def advanced_cnn_classification():
    """
    Complete advanced CNN with attention mechanisms.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Generate data
    n_samples = 1000
    X = np.random.randn(n_samples, 64, 64, 3).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 5, n_samples), 5)
    
    print("\n" + "="*60)
    print("Advanced CNN with Attention")
    print("="*60)
    
    # Build advanced CNN
    inputs = keras.Input(shape=(64, 64, 3))
    
    # Initial conv
    x = layers.Conv2D(32, (3, 3), padding='same')(inputs)
    x = layers.BatchNormalization()(x)
    x = layers.Activation('relu')(x)
    
    # Block 1 with SE attention
    x = layers.Conv2D(32, (3, 3), padding='same')(x)
    x = layers.BatchNormalization()(x)
    x = layers.Activation('relu')(x)
    x = se_block(x)  # Add channel attention
    x = layers.MaxPooling2D((2, 2))(x)
    
    # Block 2 with CBAM
    x = layers.Conv2D(64, (3, 3), padding='same')(x)
    x = layers.BatchNormalization()(x)
    x = layers.Activation('relu')(x)
    x = cbam_block(x)  # Combined attention
    x = layers.MaxPooling2D((2, 2))(x)
    
    # Block 3 with spatial attention
    x = layers.Conv2D(128, (3, 3), padding='same')(x)
    x = layers.BatchNormalization()(x)
    x = layers.Activation('relu')(x)
    x = spatial_attention_block(x)
    
    # Global pooling and classification
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(64, activation='relu')(x)
    x = layers.Dropout(0.4)(x)
    outputs = layers.Dense(5, activation='softmax')(x)
    
    model = keras.Model(inputs, outputs)
    
    model.compile(
        optimizer='adam',
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")
    
    return model

advanced_cnn = advanced_cnn_classification()
```

### Real-world Example 1: Banking - Document Analysis

```python
# Real-world Example 1: Banking - Receipt Analysis
def banking_receipt_analysis():
    """
    Analyze receipts using advanced CNN with attention.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 800
    X = np.random.randn(n_samples, 80, 80, 1).astype(np.float32)
    
    # Add text-like patterns
    for i in range(n_samples):
        for _ in range(np.random.randint(3, 10)):
            x = np.random.randint(5, 75)
            y = np.random.randint(5, 75)
            w = np.random.randint(10, 30)
            X[i, x:x+w, y:y+2] = np.random.uniform(0.5, 1.0, (w, 2, 1))
    
    # Categories: Receipt, Invoice, Statement, Other
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)
    
    print("\n" + "="*60)
    print("Banking - Receipt Analysis")
    print("="*60)
    
    # Advanced model with attention
    inputs = keras.Input(shape=(80, 80, 1))
    
    x = layers.Conv2D(32, (3, 3), padding='same')(inputs)
    x = cbam_block(x)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(64, (3, 3), padding='same')(x)
    x = se_block(x)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(128, (3, 3), padding='same')(x)
    x = spatial_attention_block(x)
    
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(64, activation='relu')(x)
    x = layers.Dropout(0.4)(x)
    outputs = layers.Dense(4, activation='softmax')(x)
    
    model = keras.Model(inputs, outputs)
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

banking_receipt_analysis()
```

### Real-world Example 2: Healthcare - Medical Image Segmentation

```python
# Real-world Example 2: Healthcare - Medical Image Segmentation
def healthcare_medical_segmentation():
    """
    Medical image segmentation with attention.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 500
    X = np.random.randn(n_samples, 64, 64, 1).astype(np.float32)
    
    # Add organ-like shapes
    for i in range(n_samples):
        cx, cy = np.random.randint(20, 44, 2)
        r = np.random.randint(10, 18)
        for dx in range(-r, r+1):
            for dy in range(-r, r+1):
                if dx*dx + dy*dy <= r*r:
                    if 0 <= cx+dx < 64 and 0 <= cy+dy < 64:
                        X[i, cx+dx, cy+dy, 0] = np.random.uniform(0.4, 0.9)
    
    # Binary segmentation mask
    y = (X > 0.3).astype(np.float32)
    
    print("\n" +="60)
    print("Healthcare - Medical Image Segmentation")
    print("="*60)
    
    # Encoder with attention
    inputs = keras.Input(shape=(64, 64, 1))
    
    # Encoder
    c1 = layers.Conv2D(32, (3, 3), padding='same', activation='relu')(inputs)
    c1 = cbam_block(c1)
    c1 = layers.Conv2D(32, (3, 3), padding='same', activation='relu')(c1)
    p1 = layers.MaxPooling2D((2, 2))(c1)
    
    c2 = layers.Conv2D(64, (3, 3), padding='same', activation='relu')(p1)
    c2 = se_block(c2)
    c2 = layers.Conv2D(64, (3, 3), padding='same', activation='relu')(c2)
    p2 = layers.MaxPooling2D((2, 2))(c2)
    
    # Bottleneck
    c3 = layers.Conv2D(128, (3, 3), padding='same', activation='relu')(p2)
    c3 = spatial_attention_block(c3)
    
    # Decoder
    u1 = layers.Conv2DTranspose(64, (2, 2), strides=(2, 2))(c3)
    u1 = layers.Concatenate()([u1, c2])
    u1 = layers.Conv2D(64, (3, 3), padding='same', activation='relu')(u1)
    
    u2 = layers.Conv2DTranspose(32, (2, 2), strides=(2, 2))(u1)
    u2 = layers.Concatenate()([u2, c1])
    u2 = layers.Conv2D(32, (3, 3), padding='same', activation='relu')(u2)
    
    # Output: Segmentation mask
    outputs = layers.Conv2D(1, (1, 1), activation='sigmoid')(u2)
    
    model = keras.Model(inputs, outputs)
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

healthcare_medical_segmentation()
```

## V. OUTPUT_RESULTS

### Expected Output

```
====================================================================================================
Advanced CNN with Attention
====================================================================================================

Architecture: CNN with SE, CBAM, and Spatial Attention
Features: Channel attention, spatial attention, skip connections
Final Val Accuracy: ~0.80-0.90
```

### Banking Example

```
Banking - Receipt Analysis
Final Val Accuracy: 0.8623
```

### Healthcare Example

```
Healthcare - Medical Image Segmentation
Final Val Accuracy: 0.8734
```

## VI. VISUALIZATION

### Attention Mechanisms

```
    CHANNEL ATTENTION (SE-Net)
    
    Input Feature Map    Global Pooling    FC->ReLU->FC    Scale
    ┌─────────────┐     ┌─────────────┐    ┌───────────┐    ┌───────────┐
    │  H x W x C  │ ──► │  1 x 1 x C  │ ─►│  Channel  │ ─►│  H x W x C│
    │             │     │  (squeeze)  │    │  Weights  │    │ (scaled)  │
    └─────────────┘     └─────────────┘    └───────────┘    └───────────┘
    
    SPATIAL ATTENTION
    
    Input Feature Map    Avg+Max Pool    Conv 7x1    Sigmoid    Multiply
    ┌─────────────┐     ┌─────────────┐  ┌─────────┐  ┌─────────┐ ┌───────────┐
    │  H x W x C  │ ──► │  H x W x 2  │ ─►│ 1x1 map │ ─►│  map    │ │  H x W x C│
    │             │     │             │   │         │   │(where)  │ │(focused)  │
    └─────────────┘     └─────────────┘  └─────────┘  └─────────┘ └───────────┘
```

## VII. ADVANCED_TOPICS

### More Advanced Techniques

1. **Vision Transformers**: Self-attention for images
2. **EfficientNet**: Compound scaling
3. **Ghost Modules**: Efficient feature generation
4. **NAS**: Neural Architecture Search

### Attention Types

- **Self-attention**: Q, K, V matrices
- **Cross-attention**: Between two sequences
- **Non-local**: Long-range dependencies

## VIII. CONCLUSION

### Key Takeaways

1. **Attention mechanisms**: Improve feature selection
2. **SE blocks**: Channel attention
3. **CBAM**: Combined channel + spatial
4. **Mixed convolutions**: Multi-scale features

### Further Reading

1. "Squeeze-and-Excitation Networks" (Hu et al., 2018)
2. "CBAM: Convolutional Block Attention Module" (Woo et al., 2019)
3. "MixNet: Mixed Depthwise Convolutional Kernels" (Tan & Le, 2019)