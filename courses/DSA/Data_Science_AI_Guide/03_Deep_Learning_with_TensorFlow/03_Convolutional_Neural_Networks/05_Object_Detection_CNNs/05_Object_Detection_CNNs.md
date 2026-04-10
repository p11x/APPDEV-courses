# Object Detection CNNs

## I. INTRODUCTION

### What is Object Detection?

Object detection is a computer vision task that combines classification and localization - not only identifying what objects are in an image but also determining where they are by predicting bounding boxes. It's more complex than image classification (single label) or semantic segmentation (pixel-level).

### Why Object Detection Matters

- **Autonomous vehicles**: Detect pedestrians, other cars, road signs
- **Surveillance**: Security monitoring, face detection
- **Retail**: Inventory tracking, customer analytics
- **Medical imaging**: Detect tumors, abnormalities
- **Agriculture**: Crop monitoring, pest detection

### Prerequisites

- CNN fundamentals
- Convolutional and pooling operations
- Classification architectures

## II. FUNDAMENTALS

### Object Detection Approaches

1. **Two-stage detectors**: R-CNN, Fast R-CNN, Faster R-CNN
2. **One-stage detectors**: YOLO, SSD
3. **Anchor-based**: Using predefined box shapes
4. **Anchor-free**: Keypoint-based (CenterNet)

### Key Terminology

- **Bounding box**: Rectangle containing detected object
- **IoU (Intersection over Union)**: Overlap metric
- **Non-maximum suppression (NMS)**: Remove duplicate detections
- **Anchor boxes**: Predefined region proposals

## III. IMPLEMENTATION

### Step 1: Simple Detection Architecture

```python
"""
Object Detection with CNNs
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
print("OBJECT DETECTION CNNS")
print("="*60)

# Step 1: Simple Detection Model (Single Object)
def simple_object_detector():
    """
    Simple single-object detector.
    Returns bounding box coordinates + class.
    """
    model = models.Sequential([
        # Backbone: Feature extraction
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(224, 224, 3)),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(128, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),
        
        # Detection head
        layers.Flatten(),
        layers.Dense(256, activation='relu'),
        
        # Output: [class_prob, x, y, w, h]
        layers.Dense(5, activation='softmax')  # class
    ])
    
    # Better: Use separate outputs
    # Class prediction + Bounding box regression
    
    print("Simple detection model created")
    return model

simple_detector = simple_object_detector()
```

### Step 2: Multi-Scale Detection (SSD-like)

```python
# Step 2: SSD-style Multi-scale Detection
def ssd_style_detector():
    """
    Single Shot MultiBox Detector style architecture.
    Detects objects at multiple scales.
    """
    # Input
    inputs = keras.Input(shape=(224, 224, 3))
    
    # Backbone: Feature pyramid
    # Feature scale 1: 56x56
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(inputs)
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    scale1 = layers.MaxPooling2D((2, 2))(x)
    
    # Feature scale 2: 28x28
    x = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(scale1)
    x = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(x)
    scale2 = layers.MaxPooling2D((2, 2))(x)
    
    # Feature scale 3: 14x14
    x = layers.Conv2D(256, (3, 3), activation='relu', padding='same')(scale2)
    x = layers.Conv2D(256, (3, 3), activation='relu', padding='same')(x)
    scale3 = layers.MaxPooling2D((2, 2))(x)
    
    # Feature scale 4: 7x7
    x = layers.Conv2D(512, (3, 3), activation='relu', padding='same')(scale3)
    x = layers.Conv2D(512, (3, 3), activation='relu', padding='same')(x)
    scale4 = layers.MaxPooling2D((2, 2))(x)
    
    # Detection heads at each scale
    # Scale 1: Detect small objects
    det1 = layers.Conv2D(36, (3, 3), activation='relu')(scale1)  # 4*9 + 4 = 40? Simplified
    
    # Scale 2: Medium objects
    det2 = layers.Conv2D(36, (3, 3), activation='relu')(scale2)
    
    # Scale 3: Large objects
    det3 = layers.Conv2D(36, (3, 3), activation='relu')(scale3)
    
    # Scale 4: Largest objects
    det4 = layers.Conv2D(36, (3, 3), activation='relu')(scale4)
    
    print("\nSSD-style multi-scale detector created")
    
    return keras.Model(inputs, [det1, det2, det3, det4])

ssd_model = ssd_style_detector()
```

### Step 3: YOLO-style Detection

```python
# Step 3: YOLO-style Detection
def yolo_style_detector():
    """
    YOLO-style single-shot detector.
    Grid-based detection at multiple scales.
    """
    inputs = keras.Input(shape=(416, 416, 3))
    
    # Backbone (darknet-like)
    x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(inputs)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(64, (1, 1), activation='relu', padding='same')(x)
    x = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(x)
    x = layers.MaxPooling2D((2, 2))(x)
    
    # Continue building...
    x = layers.Conv2D(256, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(128, (1, 1), activation='relu', padding='same')(x)
    x = layers.Conv2D(256, (3, 3), activation='relu', padding='same')(x)
    skip1 = x
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(512, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(256, (1, 1), activation='relu', padding='same')(x)
    x = layers.Conv2D(512, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(256, (1, 1), activation='relu', padding='same')(x)
    x = layers.Conv2D(512, (3, 3), activation='relu', padding='same')(x)
    skip2 = x
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(1024, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(512, (1, 1), activation='relu', padding='same')(x)
    x = layers.Conv2D(1024, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(512, (1, 1), activation='relu', padding='same')(x)
    x = layers.Conv2D(1024, (3, 3), activation='relu', padding='same')(x)
    
    # Detection at two scales (like YOLOv3)
    # For each location: [objectness, class_probs, bbox]
    
    print("\nYOLO-style detector created")
    
    return keras.Model(inputs, x)

yolo_model = yolo_style_detector()
```

### Step 4: R-CNN Style Detection

```python
# Step 4: Two-stage Detector (Faster R-CNN style)
def faster_rcnn_style():
    """
    Two-stage detector:
    1. Region Proposal Network (RPN)
    2. Classification and bbox refinement
    """
    inputs = keras.Input(shape=(224, 224, 3))
    
    # Backbone (shared feature extractor)
    # Using ResNet-style blocks
    x = layers.Conv2D(64, (7, 7), strides=2, activation='relu', padding='same')(inputs)
    x = layers.MaxPooling2D((3, 3), strides=2)(x)
    
    # Res blocks
    x = layers.Conv2D(64, (1, 1), activation='relu')(x)
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(256, (1, 1), activation='relu')(x)
    
    # Feature pyramid
    c2 = x
    x = layers.Conv2D(128, (3, 3), strides=2, activation='relu', padding='same')(x)
    c3 = x
    x = layers.Conv2D(256, (3, 3), strides=2, activation='relu', padding='same')(x)
    c4 = x
    
    # Region Proposal Network
    rpn_conv = layers.Conv2D(512, (3, 3), padding='same', activation='relu')(c4)
    
    # Classification: objectness
    rpn_cls = layers.Conv2D(9, (1, 1), activation='sigmoid')(rpn_conv)  # 3 anchors * 3 scales
    
    # Regression: bounding box delta
    rpn_reg = layers.Conv2D(36, (1, 1), activation='linear')(rpn_conv)  # 9 * 4
    
    print("\nFaster R-CNN style detector created")
    
    return keras.Model(inputs, {'rpn_cls': rpn_cls, 'rpn_reg': rpn_reg})

rcnn_model = faster_rcnn_style()
```

### Step 5: Non-Maximum Suppression

```python
# Step 5: NMS Implementation
def non_max_suppression(boxes, scores, iou_threshold=0.5):
    """
    Apply Non-Maximum Suppression to remove duplicate detections.
    
    Args:
        boxes: [N, 4] (x1, y1, x2, y2)
        scores: [N] confidence scores
        iou_threshold: IoU threshold for suppression
    
    Returns:
        keep: indices to keep
    """
    # Sort by score
    order = scores.argsort()[::-1]
    
    keep = []
    while order.size > 0:
        i = order[0]
        keep.append(i)
        
        if order.size == 1:
            break
        
        # Compute IoU with remaining boxes
        ious = np.array([compute_iou(boxes[i], boxes[j]) for j in order[1:]])
        
        # Keep boxes with IoU < threshold
        mask = ious <= iou_threshold
        order = order[1:][mask]
    
    return keep

def compute_iou(box1, box2):
    """Compute IoU between two boxes."""
    x1 = max(box1[0], box2[0])
    y1 = max(box1[1], box2[1])
    x2 = min(box1[2], box2[2])
    y2 = min(box1[3], box2[3])
    
    inter = max(0, x2 - x1) * max(0, y2 - y1)
    area1 = (box1[2] - box1[0]) * (box1[3] - box1[1])
    area2 = (box2[2] - box2[0]) * (box2[3] - box2[1])
    
    union = area1 + area2 - inter
    return inter / union if union > 0 else 0

print("\nNMS implementation complete")
```

## IV. APPLICATIONS

### Standard Example: Object Detection Pipeline

```python
# Standard Example: End-to-End Detection
def object_detection_pipeline():
    """
    Complete object detection pipeline.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Generate synthetic images with objects
    n_samples = 500
    X = np.random.randn(n_samples, 64, 64, 3).astype(np.float32)
    
    # Add simple objects (colored rectangles)
    for i in range(n_samples):
        for _ in range(np.random.randint(1, 4)):
            x = np.random.randint(5, 50)
            y = np.random.randint(5, 50)
            w = np.random.randint(8, 20)
            h = np.random.randint(8, 20)
            color = np.random.uniform(0.5, 1.0, 3)
            X[i, x:x+w, y:y+h] = color
    
    print("\n" + "="*60)
    print("Object Detection Pipeline")
    print("="*60)
    
    # Simple CNN for detection
    # Output: class + bbox coordinates
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(64, 64, 3)),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(128, (3, 3), activation='relu'),
        
        layers.GlobalAveragePooling2D(),
        
        # Output: class + bbox (x, y, w, h)
        layers.Dense(64, activation='relu'),
        layers.Dense(5, activation='linear')  # 1 class + 4 coords
    ])
    
    # Simplified training (would need proper data format in real use)
    model.compile(optimizer='adam', loss='mse')
    model.fit(X, X[:, 0, 0, :5], epochs=5, verbose=0)
    
    print("Detection model trained")
    
    return model

detection_model = object_detection_pipeline()
```

### Real-world Example 1: Banking - Document Scanner

```python
# Real-world Example 1: Banking - Document Detection
def banking_document_detection():
    """
    Detect document boundaries in scanned images.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 600
    X = np.random.randn(n_samples, 96, 96, 1).astype(np.float32)
    
    # Add document-like rectangles
    for i in range(n_samples):
        x, y = np.random.randint(10, 30, 2)
        w, h = np.random.randint(30, 60, 2)
        X[i, y:y+h, x:x+w] = np.random.uniform(0.6, 1.0, (h, w, 1))
    
    # Output: [x, y, w, h] normalized coordinates
    y_coords = np.array([
        [np.random.uniform(0.1, 0.3), np.random.uniform(0.1, 0.3),
         np.random.uniform(0.3, 0.5), np.random.uniform(0.3, 0.5)]
        for _ in range(n_samples)
    ])
    
    print("\n" + "="*60)
    print("Banking - Document Detection")
    print("="*60)
    
    # Detection model
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(96, 96, 1)),
        layers.BatchNormalization(),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        
        layers.GlobalAveragePooling2D(),
        layers.Dense(128, activation='relu'),
        layers.Dense(4, activation='sigmoid')  # x, y, w, h
    ])
    
    model.compile(optimizer='adam', loss='mse')
    model.fit(X, y_coords, epochs=15, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"Model trained for document detection")

banking_document_detection()
```

### Real-world Example 2: Healthcare - Cell Detection

```python
# Real-world Example 2: Healthcare - Cell Detection
def healthcare_cell_detection():
    """
    Detect cells in microscopy images.
    """
    np.random.seed(42)
    tf.random.set_seed(42)
    
    n_samples = 800
    X = np.random.randn(n_samples, 64, 64, 1).astype(np.float32)
    
    # Add cell-like patterns (circular blobs)
    for i in range(n_samples):
        num_cells = np.random.randint(2, 8)
        for _ in range(num_cells):
            cx = np.random.randint(8, 56)
            cy = np.random.randint(8, 56)
            r = np.random.randint(3, 8)
            for dx in range(-r, r+1):
                for dy in range(-r, r+1):
                    if dx*dx + dy*dy <= r*r:
                        if 0 <= cx+dx < 64 and 0 <= cy+dy < 64:
                            X[i, cx+dx, cy+dy, 0] = np.random.uniform(0.4, 0.9)
    
    # Labels: [x, y, radius] for each image
    y = np.zeros((n_samples, 3))
    for i in range(n_samples):
        y[i] = [np.random.uniform(0.2, 0.8), np.random.uniform(0.2, 0.8), np.random.uniform(0.1, 0.3)]
    
    print("\n" + "="*60)
    print("Healthcare - Cell Detection")
    print("="*60)
    
    # Cell detection model
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(64, 64, 1)),
        layers.BatchNormalization(),
        layers.Conv2D(32, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2)),
        
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        layers.Conv2D(64, (3, 3), activation='relu'),
        
        layers.GlobalAveragePooling2D(),
        layers.Dense(64, activation='relu'),
        layers.Dense(3, activation='sigmoid')  # center_x, center_y, radius
    ])
    
    model.compile(optimizer='adam', loss='mse')
    model.fit(X, y, epochs=20, batch_size=32, validation_split=0.2, verbose=1)
    
    print(f"Cell detection complete")

healthcare_cell_detection()
```

## V. OUTPUT_RESULTS

### Expected Output

```
====================================================================================================
Object Detection Pipeline
====================================================================================================

Architecture: CNN-based single-shot detector
Output: Class predictions + Bounding boxes
Components: Backbone + Detection head + NMS
```

### Banking Example

```
Banking - Document Detection
Trained to detect document boundaries in scanned images
```

### Healthcare Example

```
Healthcare - Cell Detection
Trained to detect cell centers and sizes in microscopy images
```

## VI. VISUALIZATION

### Detection Pipeline

```
    INPUT IMAGE         FEATURE EXTRACTION        DETECTION HEAD
    ┌─────────┐         ┌─────────────────┐       ┌──────────────┐
    │         │         │                 │       │              │
    │  Image  │ ──────► │  CNN Backbone   │ ──►  │ Class + BBox │
    │         │         │ (ResNet/VGG)    │       │              │
    └─────────┘         └─────────────────┘       └──────────────┘
                                                       │
                                                       ▼
                                               ┌──────────────┐
                                               │    NMS       │ ──► Final boxes
                                               │ (filtering)  │
                                               └──────────────┘
```

## VII. ADVANCED_TOPICS

### Modern Detectors

1. **YOLOv8**: Latest YOLO with better accuracy
2. **Faster R-CNN**: Two-stage, highest accuracy
3. **DETR**: Transformer-based detection
4. **EfficientDet**: Efficient, scalable

### Key Components

- **Feature Pyramid Network (FPN)**: Multi-scale features
- **Anchor Boxes**: Prior region shapes
- **Loss Functions**: Classification + Regression

## VIII. CONCLUSION

### Key Takeaways

1. **Object detection**: Localization + Classification
2. **Two-stage vs one-stage**: Speed/accuracy trade-off
3. **NMS**: Remove duplicate detections

### Further Reading

1. "Faster R-CNN" (Ren et al., 2015)
2. "YOLO9000" (Redmon & Farhadi, 2017)
3. "SSD: Single Shot MultiBox Detector" (Liu et al., 2016)