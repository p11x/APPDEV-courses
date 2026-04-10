# Topic: Transfer Learning with CNNs
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Transfer Learning with CNNs

I. INTRODUCTION
   - Transfer learning uses pre-trained models on new tasks
   - Reduces training time and data requirements
   - Common models: VGG, ResNet, MobileNet, EfficientNet

II. CORE_CONCEPTS
   - Feature extraction mode
   - Fine-tuning mode
   - Freezing layers
   - Custom classifier training

III. IMPLEMENTATION
   - Using pre-trained models
   - Feature extraction
   - Fine-tuning strategies
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models, applications
import numpy as np


def mobilenet_feature_extraction():
    base_model = applications.MobileNetV2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dense(1, activation='sigmoid')
    ])
    
    print("MobileNetV2 Feature Extraction:")
    print(f"  Trainable parameters: {model.count_params()}")
    return model


def resnet_feature_extraction():
    base_model = applications.ResNet50V2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dense(10, activation='softmax')
    ])
    
    print("ResNet50V2 Feature Extraction:")
    print(f"  Trainable parameters: {model.count_params()}")
    return model


def vgg_feature_extraction():
    base_model = applications.VGG16(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.Flatten(),
        layers.Dense(256, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    print("VGG16 Feature Extraction:")
    model.summary()
    return model


def efficientnet_feature_extraction():
    base_model = applications.EfficientNetB0(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dropout(0.3),
        layers.Dense(10, activation='softmax')
    ])
    
    print("EfficientNetB0:")
    print(f"  Total parameters: {model.count_params()}")
    return model


def partial_fine_tuning():
    base_model = applications.MobileNetV2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    for layer in base_model.layers[-30:]:
        layer.trainable = True
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dense(1, activation='sigmoid')
    ])
    
    print("Partial Fine-tuning:")
    trainable_count = sum([tf.size(w).numpy() for w in model.trainable_weights])
    print(f"  Trainable weights: {trainable_count}")
    return model


def full_fine_tuning():
    base_model = applications.MobileNetV2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = True
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dropout(0.5),
        layers.Dense(1, activation='sigmoid')
    ])
    
    print("Full Fine-tuning:")
    print(f"  Total parameters: {model.count_params()}")
    return model


def custom_classifier():
    base_model = applications.MobileNetV2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.BatchNormalization(),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.3),
        layers.BatchNormalization(),
        layers.Dense(1, activation='sigmoid')
    ])
    
    print("Custom Classifier:")
    model.summary()
    return model


def multi_output_model():
    base_model = applications.MobileNetV2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    x = base_model.output
    x = layers.GlobalAveragePooling2D()(x)
    
    class_output = layers.Dense(10, activation='softmax', name='class')(x)
    bbox_output = layers.Dense(4, name='bbox')(x)
    
    model = models.Model(inputs=base_model.input, outputs=[class_output, bbox_output])
    
    print("Multi-output Model:")
    print(f"  Outputs: {len(model.outputs)}")
    return model


def train_with_tfdata():
    base_model = applications.MobileNetV2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([50, 224, 224, 3])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=3, verbose=0)
    print(f"Training - Loss: {history.history['loss'][-1]:.4f}")
    return model


def compare_transfer_methods():
    X = tf.random.normal([30, 224, 224, 3])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    results = {}
    
    base = applications.MobileNetV2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base.trainable = False
    model1 = models.Sequential([base, layers.GlobalAveragePooling2D(), layers.Dense(1, 'sigmoid')])
    model1.compile(optimizer='adam', loss='binary_crossentropy')
    history1 = model1.fit(X, y, epochs=3, verbose=0)
    results['Feature Extraction'] = history1.history['loss'][-1]
    
    base2 = applications.MobileNetV2(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base2.trainable = True
    model2 = models.Sequential([base2, layers.GlobalAveragePooling2D(), layers.Dense(1, 'sigmoid')])
    model2.compile(optimizer='adam', loss='binary_crossentropy')
    history2 = model2.fit(X, y, epochs=3, verbose=0)
    results['Fine-tuning'] = history2.history['loss'][-1]
    
    print("Transfer Learning Comparison:")
    for method, loss in results.items():
        print(f"  {method}: {loss:.4f}")
    return results


def core_implementation():
    print("MobileNetV2 Feature Extraction:")
    mobilenet_feature_extraction()
    print("\nResNet50V2:")
    resnet_feature_extraction()
    print("\nCustom Classifier:")
    custom_classifier()
    print("\nCompare Transfer Methods:")
    compare_transfer_methods()
    return True


def banking_example():
    base_model = applications.MobileNetV2(input_shape=(224, 224, 1), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.BatchNormalization(),
        layers.Dense(64, activation='relu'),
        layers.Dropout(0.4),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([50, 224, 224, 1])
    y = tf.cast(tf.reduce_mean(X) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Banking - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    base_model = applications.EfficientNetB0(input_shape=(224, 224, 3), include_top=False, weights='imagenet')
    base_model.trainable = False
    
    model = models.Sequential([
        base_model,
        layers.GlobalAveragePooling2D(),
        layers.BatchNormalization(),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.4),
        layers.BatchNormalization(),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([50, 224, 224, 3])
    y = tf.random.uniform([50], minval=0, maxval=5, dtype=tf.int32)
    y = tf.one_hot(y, depth=5)
    
    history = model.fit(X, y, epochs=5, verbose=0)
    print(f"Healthcare - Accuracy: {history.history['accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Transfer Learning with CNNs implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()