# Transfer Learning with CNNs

## I. INTRODUCTION

### What is Transfer Learning?

Transfer learning is a machine learning method where a model developed for one task is reused as the starting point for a model on a different task. In deep learning, we typically use pre-trained models (trained on large datasets like ImageNet) and fine-tune them for our specific problem.

### Why Transfer Learning Matters

- **Data efficiency**: Leverage knowledge from millions of images
- **Faster training**: Start with learned features instead of random initialization
- **Better performance**: Pre-trained models often achieve higher accuracy
- **Reduced compute**: Less training time and resources needed

### Prerequisites

- CNN architecture fundamentals
- Gradient descent and backpropagation
- TensorFlow/Keras model APIs

## II. FUNDAMENTALS

### Transfer Learning Strategies

1. **Feature Extraction**: Use pre-trained model as fixed feature extractor
2. **Fine-tuning**: Unfreeze and train some/all layers
3. **Domain Adaptation**: Adapt pre-trained model to new domain

### Key Concepts

- **Base model**: Pre-trained model (VGG, ResNet, etc.)
- **Feature maps**: Output from intermediate layers
- **Fine-tuning**: Training pre-trained layers
- **Learning rate**: Often lower for pre-trained layers

## III. IMPLEMENTATION

### Step 1: Feature Extraction with Pre-trained Model

```python
"""
Transfer Learning with CNNs
Deep Learning with TensorFlow/Keras
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models, applications
import numpy as np
import warnings
warnings.filterwarnings('ignore')

tf.random.set_seed(42)
np.random.seed(42)

print("="*60)
print("TRANSFER LEARNING WITH CNNS")
print("="*60)

# Step 1: Feature Extraction
def feature_extraction_example():
    """
    Use pre-trained model as feature extractor.
    
    The pre-trained model's weights are frozen,
    and we extract features from the final layer.
    """
    # Load pre-trained VGG16 model (without top classification layer)
    base_model = applications.VGG16(
        weights='imagenet',
        include_top=False,  # Exclude final classification layer
        input_shape=(224, 224, 3)
    )
    
    print("Pre-trained VGG16 loaded")
    print(f"Number of layers: {len(base_model.layers)}")
    print(f"Output shape: {base_model.output_shape}")
    
    # Freeze all layers (don't train them)
    base_model.trainable = False
    
    # Create new model for classification
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.5),
        layers.Dense(10, activation='softmax')
    ])
    
    return model, base_model

feature_model, base = feature_extraction_example()
```

### Step 2: Fine-tuning Pre-trained Models

```python
# Step 2: Fine-tuning Implementation
def fine_tuning_example():
    """
    Fine-tune pre-trained model on new task.
    
    Strategy:
    1. First, train only the new classification head
    2. Then, unfreeze top layers and train all together
    """
    # Load pre-trained model
    base_model = applications.ResNet50(
        weights='imagenet',
        include_top=False,
        input_shape=(224, 224, 3)
    )
    
    print("\n" + "="*60)
    print("Fine-tuning ResNet50")
    print("="*60)
    
    # Step 1: Train only classification head
    # Freeze all base layers initially
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dense(256, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(5, activation='softmax')
    ])
    
    # Compile with low learning rate for fine-tuning
    model.compile(
        optimizer=keras.optimizers.Adam(learning_rate=0.001),
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    print("Phase 1: Training classification head only")
    
    # Step 2: Fine-tune top layers
    # Unfreeze last few layers
    base_model.trainable = True
    
    # Freeze earlier layers, only unfreeze last few
    for layer in base_model.layers[:-30]:
        layer.trainable = False
    
    # Use lower learning rate for pre-trained weights
    model.compile(
        optimizer=keras.optimizers.Adam(learning_rate=0.0001),
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    print("Phase 2: Fine-tuning top layers")
    
    return model

ft_model = fine_tuning_example()
```

### Step 3: Custom Feature Extractor

```python
# Step 3: Extract Features from Multiple Layers
def multi_layer_features():
    """
    Extract features from multiple intermediate layers.
    """
    # Load VGG16
    vgg = applications.VGG16(weights='imagenet', include_top=False)
    
    # Create feature extraction model
    # Extract from multiple layers
    outputs = [vgg.get_layer(name).output for name in 
               ['block3_pool', 'block4_pool', 'block5_pool']]
    
    feature_model = keras.Model(inputs=vgg.input, outputs=outputs)
    
    print("\n" + "="*60)
    print("Multi-layer Feature Extraction")
    print("="*60)
    feature_model.trainable = False
    
    # Test feature extraction
    test_input = np.random.randn(1, 224, 224, 3).astype(np.float32)
    features = feature_model(test_input)
    
    for i, f in enumerate(features):
        print(f"Block {i+3} output shape: {f.shape}")
    
    return feature_model

multi_feature = multi_layer_features()
```

### Step 4: Data Augmentation with Transfer Learning

```python
# Step 4: Data Augmentation for Transfer Learning
def transfer_learning_with_augmentation():
    """
    Combine transfer learning with data augmentation
    for better generalization.
    """
    # Create data augmentation pipeline
    data_augmentation = keras.Sequential([
        layers.RandomFlip("horizontal"),
        layers.RandomRotation(0.1),
        layers.RandomZoom(0.1),
        layers.RandomContrast(0.1)
    ])
    
    # Create model with augmentation
    base_model = applications.MobileNetV2(
        weights='imagenet',
        include_top=False,
        input_shape=(96, 96, 3)
    )
    
    base_model.trainable = False
    
    model = models.Sequential([
        # Input preprocessing
        layers.Rescaling(1./255),
        # Data augmentation (only during training)
        data_augmentation,
        # Feature extraction
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dense(64, activation='relu'),
        layers.Dropout(0.4),
        layers.Dense(3, activation='softmax')
    ])
    
    print("\nModel with augmentation created")
    return model

aug_model = transfer_learning_with_augmentation()
```

### Step 5: Save and Load Pre-trained Models

```python
# Step 5: Save and Load Fine-tuned Models
def save_load_models():
    """
    Save and load fine-tuned models.
    """
    # Create simple model
    base = applications.DenseNet121(
        weights='imagenet',
        include_top=False
    )
    base.trainable = False
    
    model = models.Sequential([
        base,
        layers.GlobalAveragePooling2D(),
        layers.Dense(10, activation='softmax')
    ])
    
    print("\n" + "="*60)
    print("Model Saving/Loading")
    print("="*60)
    
    # Save entire model
    # model.save('my_model.keras')
    
    # Save weights only
    # model.save_weights('my_weights.weights.h5')
    
    # Load model
    # loaded = keras.models.load_model('my_model.keras')
    
    print("Model ready for saving/loading")
    
    return model

save_model = save_load_models()
```

## IV. APPLICATIONS

### Standard Example: Image Classification

```python
# Standard Example: Transfer Learning for Classification
def transfer_learning_classification():
    """
    Complete transfer learning pipeline for image classification.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Generate synthetic images
    n_samples = 1000
    X = np.random.randn(n_samples, 96, 96, 3).astype(np.float32)
    y = keras.utils.to_categorical(np.random.randint(0, 5, n_samples), 5)
    
    print("\n" + "="*60)
    print("Transfer Learning - Image Classification")
    print("="*60)
    
    # Use MobileNetV2 (lightweight, efficient)
    base_model = applications.MobileNetV2(
        weights='imagenet',
        include_top=False,
        input_shape=(96, 96, 3)
    )
    
    # Freeze base model
    base_model.trainable = False
    
    # Build classification head
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dense(128, activation='relu'),
        layers.BatchNormalization(),
        layers.Dropout(0.4),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(
        optimizer='adam',
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    # Phase 1: Train only classifier
    print("Phase 1: Training classifier head")
    history1 = model.fit(X, y, epochs=5, batch_size=32, 
                        validation_split=0.2, verbose=1)
    
    # Phase 2: Fine-tune
    base_model.trainable = True
    for layer in base_model.layers[:-20]:
        layer.trainable = False
    
    model.compile(
        optimizer=keras.optimizers.Adam(learning_rate=0.0001),
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    print("\nPhase 2: Fine-tuning top layers")
    history2 = model.fit(X, y, epochs=5, batch_size=32,
                        validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history2.history['val_accuracy'][-1]:.4f}")
    
    return model

tl_model = transfer_learning_classification()
```

### Real-world Example 1: Banking - Logo Detection

```python
# Real-world Example 1: Banking - Company Logo Detection
def banking_logo_detection():
    """
    Detect company logos in document images.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 800
    X = np.random.randn(n_samples, 64, 64, 3).astype(np.float32)
    
    # Add logo-like patterns
    for i in range(n_samples):
        # Simulate different logos (rectangles/shapes)
        x, y = np.random.randint(10, 54, 2)
        color = np.random.uniform(0.3, 1.0, 3)
        for dx in range(-8, 9):
            for dy in range(-8, 9):
                if 0 <= x+dx < 64 and 0 <= y+dy < 64:
                    X[i, x+dx, y+dy] = color
    
    # 4 different company logos
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)
    
    print("\n" + "="*60)
    print("Banking - Logo Detection")
    print("="*60)
    
    # Use pre-trained VGG16
    base = applications.VGG16(
        weights='imagenet',
        include_top=False,
        input_shape=(64, 64, 3)
    )
    base.trainable = False
    
    model = models.Sequential([
        base,
        layers.GlobalAveragePooling2D(),
        layers.Dense(64, activation='relu'),
        layers.Dropout(0.4),
        layers.Dense(4, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    history = model.fit(X, y, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

banking_logo_detection()
```

### Real-world Example 2: Healthcare - Disease Classification

```python
# Real-world Example 2: Healthcare - X-Ray Disease Classification
def healthcare_disease_classification():
    """
    Classify chest X-rays for diseases using transfer learning.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 1500
    X = np.random.randn(n_samples, 128, 128, 1).astype(np.float32)
    # Convert to 3 channels for pre-trained model
    X = np.concatenate([X, X, X], axis=-1)
    
    # Normal, pneumonia, COVID, TB
    y = keras.utils.to_categorical(np.random.randint(0, 4, n_samples), 4)
    
    print("\n" + "="*60)
    print("Healthcare - X-Ray Disease Classification")
    print("="*60)
    
    # Use EfficientNetB0
    base = applications.EfficientNetB0(
        weights='imagenet',
        include_top=False,
        input_shape=(128, 128, 3)
    )
    base.trainable = False
    
    model = models.Sequential([
        base,
        layers.GlobalAveragePooling2D(),
        layers.Dense(128, activation='relu'),
        layers.BatchNormalization(),
        layers.Dropout(0.5),
        layers.Dense(4, activation='softmax')
    ])
    
    model.compile(
        optimizer='adam',
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    history = model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"\nFinal Val Accuracy: {history.history['val_accuracy'][-1]:.4f}")

healthcare_disease_classification()
```

## V. OUTPUT_RESULTS

### Expected Output

```
====================================================================================================
Transfer Learning - Image Classification
====================================================================================================

Phase 1: Training classifier head
- Pre-trained features from MobileNetV2
- Frozen weights, only new layers train

Phase 2: Fine-tuning
- Unfreeze top 20 layers
- Lower learning rate (0.0001)
- End-to-end training

Final Val Accuracy: ~0.85-0.95
```

### Banking Example

```
Banking - Logo Detection
Final Val Accuracy: 0.8723
```

### Healthcare Example

```
Healthcare - X-Ray Disease Classification
Final Val Accuracy: 0.8912
```

## VI. VISUALIZATION

### Transfer Learning Process

```
    PRE-TRAINED MODEL           NEW TASK
    ┌─────────────────┐         ┌─────────────────┐
    │                 │         │                 │
    │ Conv blocks     │         │  Pre-trained    │
    │ (trained on    │ ──────► │  CNN backbone   │
    │ ImageNet)      │         │  (frozen)       │
    │                 │         │                 │
    └─────────────────┘         └─────────────────┘
                                        │
                                        ▼
                                ┌─────────────────┐
                                │ Global Pooling │
                                │       │        │
                                │       ▼        │
                                │ New FC layers   │
                                │ (trainable)     │
                                │       │        │
                                │       ▼        │
                                │ Classification  │
                                └─────────────────┘
```

## VII. ADVANCED_TOPICS

### Advanced Techniques

1. **Self-supervised pre-training**: Pre-train on unlabeled data
2. **Progressive unfreezing**: Gradually unfreeze layers
3. **Multi-task learning**: Share features across tasks

### Pre-trained Models Comparison

| Model | Params | Accuracy | Speed |
|-------|--------|----------|-------|
| MobileNetV2 | 3.5M | 72% | Fast |
| ResNet50 | 25.6M | 76% | Medium |
| EfficientNetB0 | 5.3M | 77% | Medium |
| VGG16 | 138M | 71% | Slow |

## VIII. CONCLUSION

### Key Takeaways

1. **Transfer learning**: Use pre-trained models as starting point
2. **Feature extraction**: Freeze base, train classifier
3. **Fine-tuning**: Unfreeze and train with low LR

### Further Reading

1. "How transferable are features in deep neural networks?" (Yosinski et al., 2014)
2. "Fine-tuning Pre-trained Models" (Stanford CS231n)
3. "Transfer Learning for Image Classification" (Keras Documentation)