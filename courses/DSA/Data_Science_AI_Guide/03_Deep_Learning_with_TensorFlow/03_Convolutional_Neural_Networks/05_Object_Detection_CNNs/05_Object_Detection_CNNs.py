# Topic: Object Detection CNNs
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Object Detection CNNs

I. INTRODUCTION
   - Object detection localizes and classifies objects in images
   - Applications: self-driving cars, surveillance, medical imaging
   - Frameworks: YOLO, SSD, Faster R-CNN

II. CORE_CONCEPTS
   - Bounding box prediction
   - Anchor boxes
   - Non-maximum suppression
   - Intersection over Union (IoU)

III. IMPLEMENTATION
   - Simple detection model
   - Anchor box generation
   - NMS implementation
   - Detection training
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def simple_detection_head():
    inputs = keras.Input(shape=(224, 224, 3))
    x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(inputs)
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(x)
    
    bbox_regression = layers.Conv2D(4, (1, 1))(x)
    classification = layers.Conv2D(10, (1, 1), activation='sigmoid')(x)
    
    model = models.Model(inputs=inputs, outputs=[bbox_regression, classification])
    print("Simple Detection Head:")
    model.summary()
    return model


def yolo_style_detection():
    inputs = keras.Input(shape=(416, 416, 3))
    
    x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(inputs)
    x = layers.MaxPooling2D((2, 2))(x)
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    x = layers.MaxPooling2D((2, 2))(x)
    x = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(256, (3, 3), activation='relu', padding='same')(x)
    x = layers.MaxPooling2D((2, 2))(x)
    x = layers.Conv2D(512, (3, 3), activation='relu', padding='same')(x)
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(1024, activation='relu')(x)
    x = layers.Dense(30, activation='linear')(x)
    
    model = models.Model(inputs=inputs, outputs=x)
    print("YOLO-style Detection:")
    model.summary()
    return model


def anchor_boxes():
    feature_size = 13
    anchor_sizes = [(30, 30), (60, 60), (100, 100)]
    anchor_ratios = [[1, 1], [1, 2], [2, 1]]
    
    anchors = []
    for size in anchor_sizes:
        for ratio in anchor_ratios:
            width = size[0] * np.sqrt(ratio[0])
            height = size[1] * np.sqrt(ratio[1])
            anchors.append((width, height))
    
    print(f"Generated {len(anchors)} anchor boxes:")
    for i, anchor in enumerate(anchors):
        print(f"  Anchor {i+1}: {anchor[0]:.1f}x{anchor[1]:.1f}")
    return anchors


def iou_calculation():
    box1 = [10, 10, 50, 50]
    box2 = [30, 30, 70, 70]
    
    x1 = max(box1[0], box2[0])
    y1 = max(box1[1], box2[1])
    x2 = min(box1[2], box2[2])
    y2 = min(box1[3], box2[3])
    
    intersection = max(0, x2 - x1) * max(0, y2 - y1)
    
    area1 = (box1[2] - box1[0]) * (box1[3] - box1[1])
    area2 = (box2[2] - box2[0]) * (box2[3] - box2[1])
    union = area1 + area2 - intersection
    
    iou = intersection / union if union > 0 else 0
    print(f"IoU calculation: {iou:.4f}")
    return iou


def non_max_suppression():
    boxes = tf.constant([[10, 10, 50, 50], [15, 15, 55, 55], [80, 80, 120, 120]])
    scores = tf.constant([0.9, 0.85, 0.7])
    
    selected_indices = tf.image.non_max_suppression(boxes, scores, max_output_size=2, iou_threshold=0.5)
    print(f"NMS selected indices: {selected_indices.numpy()}")
    return selected_indices


def ssd_style_detection():
    inputs = keras.Input(shape=(300, 300, 3))
    
    base = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(inputs)
    base = layers.MaxPooling2D((2, 2))(base)
    base = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(base)
    base = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(base)
    
    conv4_3 = layers.Conv2D(512, (3, 3), activation='relu', padding='same')(base)
    pool = layers.MaxPooling2D((2, 2))(conv4_3)
    conv5 = layers.Conv2D(256, (3, 3), activation='relu', padding='same')(pool)
    
    output = layers.GlobalAveragePooling2D()(conv5)
    model = models.Model(inputs=inputs, outputs=output)
    print("SSD-style Detection:")
    model.summary()
    return model


def bounding_box_to_corners():
    center_x, center_y, width, height = 50, 50, 40, 40
    
    x1 = center_x - width / 2
    y1 = center_y - height / 2
    x2 = center_x + width / 2
    y2 = center_y + height / 2
    
    print(f"Center: ({center_x}, {center_y}), Size: {width}x{height}")
    print(f"Corners: ({x1}, {y1}, {x2}, {y2})")
    return [x1, y1, x2, y2]


def decode_detection_output():
    model = keras.Sequential([
        layers.Conv2D(256, (3, 3), activation='relu', input_shape=(13, 13, 1024)),
        layers.Conv2D(4 * 5, (1, 1))
    ])
    
    test_input = tf.ones([1, 13, 13, 1024])
    output = model(test_input)
    
    print(f"Detection output shape: {output.shape}")
    print(f"Output spatial shape: {output.shape[1:3]}")
    return output


def multi_scale_detection():
    inputs = keras.Input(shape=(224, 224, 3))
    
    scale_32 = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(inputs)
    scale_32 = layers.Conv2D(64, (1, 1))(scale_32)
    
    scale_16 = layers.MaxPooling2D((2, 2))(scale_32)
    scale_16 = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(scale_16)
    scale_16 = layers.Conv2D(64, (1, 1))(scale_16)
    
    scale_8 = layers.MaxPooling2D((2, 2))(scale_16)
    scale_8 = layers.Conv2D(256, (3, 3), activation='relu', padding='same')(scale_8)
    scale_8 = layers.Conv2D(64, (1, 1))(scale_8)
    
    model = models.Model(inputs=inputs, outputs=[scale_32, scale_16, scale_8])
    print("Multi-scale Detection:")
    model.summary()
    return model


def core_implementation():
    print("Simple Detection Head:")
    simple_detection_head()
    print("\nAnchor Boxes:")
    anchor_boxes()
    print("\nIoU Calculation:")
    iou_calculation()
    print("\nNon-Max Suppression:")
    non_max_suppression()
    return True


def banking_example():
    inputs = keras.Input(shape=(64, 64, 1))
    
    x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(inputs)
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    x = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(x)
    
    bbox = layers.Conv2D(4, (1, 1))(x)
    class_out = layers.Conv2D(2, (1, 1), activation='sigmoid')(x)
    
    model = models.Model(inputs=inputs, outputs=[bbox, class_out])
    model.compile(optimizer='adam', loss='mse')
    
    X = tf.random.normal([10, 64, 64, 1])
    bboxes = tf.random.normal([10, 64, 64, 4])
    classes = tf.random.uniform([10, 64, 64, 2])
    
    print("Banking Document Detection Model")
    return model


def healthcare_example():
    inputs = keras.Input(shape=(128, 128, 3))
    
    x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(inputs)
    x = layers.BatchNormalization()(x)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    x = layers.BatchNormalization()(x)
    x = layers.MaxPooling2D((2, 2))(x)
    
    x = layers.Conv2D(128, (3, 3), activation='relu', padding='same')(x)
    x = layers.BatchNormalization()(x)
    
    bbox = layers.Conv2D(4, (1, 1))(x)
    class_out = layers.Conv2D(5, (1, 1), activation='sigmoid')(x)
    
    model = models.Model(inputs=inputs, outputs=[bbox, class_out])
    model.compile(optimizer='adam', loss='mse')
    
    X = tf.random.normal([10, 128, 128, 3])
    print("Healthcare Medical Object Detection Model")
    return model


def main():
    print("Executing Object Detection CNNs implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()