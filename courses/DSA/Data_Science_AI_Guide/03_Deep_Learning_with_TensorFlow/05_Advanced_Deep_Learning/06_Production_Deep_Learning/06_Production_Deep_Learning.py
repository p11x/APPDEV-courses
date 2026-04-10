# Topic: Production Deep Learning
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Production Deep Learning

I. INTRODUCTION
   - Production ML involves deploying and maintaining models at scale
   - Includes serving, monitoring, and continuous training
   - Best practices for enterprise deployment

II. CORE_CONCEPTS
   - Model serving
   - Model versioning
   - Monitoring and logging
   - Performance optimization

III. IMPLEMENTATION
   - Model export
   - TensorFlow Serving
   - Optimization techniques
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def create_sample_model():
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dropout(0.3),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    return model


def save_model_keras():
    model = create_sample_model()
    model.compile(optimizer='adam', loss='binary_crossentropy')
    model.save('model.keras')
    print("Model saved in Keras format")
    return model


def save_model_savedmodel():
    model = create_sample_model()
    model.compile(optimizer='adam', loss='binary_crossentropy')
    model.save('saved_model/model')
    print("Model saved in SavedModel format")
    return model


def save_model_tflite():
    model = create_sample_model()
    model.compile(optimizer='adam', loss='binary_crossentropy')
    
    converter = tf.lite.TFLiteConverter.from_keras_model(model)
    tflite_model = converter.convert()
    
    with open('model.tflite', 'wb') as f:
        f.write(tflite_model)
    
    print("Model saved in TensorFlow Lite format")
    return tflite_model


def load_model():
    model = models.load_model('model.keras')
    print("Model loaded from Keras format")
    return model


def quantize_model():
    model = create_sample_model()
    model.compile(optimizer='adam', loss='binary_crossentropy')
    
    converter = tf.lite.TFLiteConverter.from_keras_model(model)
    converter.optimizations = [tf.lite.Optimize.DEFAULT]
    converter.target_spec.supported_types = [tf.float16]
    
    quantized_model = converter.convert()
    print(f"Quantized model size: {len(quantized_model)} bytes")
    return quantized_model


def model_versioning():
    version_info = {
        'version': '1.0.0',
        'timestamp': '2024-01-01',
        'metrics': {'accuracy': 0.95, 'loss': 0.05}
    }
    print(f"Model version info: {version_info}")
    return version_info


def batch_inference():
    model = create_sample_model()
    _ = model(tf.ones([32, 10]))
    
    batch_data = tf.random.normal([100, 10])
    predictions = model.predict(batch_data, batch_size=32, verbose=0)
    print(f"Batch inference - shape: {predictions.shape}")
    return predictions


def core_implementation():
    print("Save Model (Keras):")
    save_model_keras()
    print("\nQuantize Model:")
    quantize_model()
    print("\nBatch Inference:")
    batch_inference()
    return True


def banking_example():
    model = models.Sequential([
        layers.Dense(128, activation='relu', input_shape=(20,)),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Dense(64, activation='relu'),
        layers.BatchNormalization(),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    model.save('banking_model.keras')
    print(f"Banking model saved - params: {model.count_params()}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.Dense(256, activation='relu', input_shape=(50,)),
        layers.BatchNormalization(),
        layers.Dropout(0.4),
        layers.Dense(128, activation='relu'),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Dense(64, activation='relu'),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    model.save('healthcare_model.keras')
    print(f"Healthcare model saved - params: {model.count_params()}")
    return model


def main():
    print("Executing Production Deep Learning implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()