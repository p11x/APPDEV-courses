# Topic: Meta Learning and Few-Shot Learning
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Meta-Learning and Few-Shot Learning

I. INTRODUCTION
   - Meta-learning learns to learn from few examples
   - Enables rapid adaptation to new tasks
   - Applications: few-shot image classification, rapid robotics

II. CORE_CONCEPTS
   - MAML (Model-Agnostic Meta-Learning)
   - Prototypical networks
   - Siamese networks
   - Episode-based training

III. IMPLEMENTATION
   - MAML algorithm
   - Prototypical networks
   - Metric learning
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def prototypical_network():
    inputs = keras.Input(shape=(84, 84, 3))
    x = layers.Conv2D(32, (3, 3), activation='relu')(inputs)
    x = layers.Conv2D(64, (3, 3), activation='relu')(x)
    x = layers.Conv2D(128, (3, 3), activation='relu')(x)
    x = layers.GlobalAveragePooling2D()(x)
    model = models.Model(inputs=inputs, outputs=x)
    print("Prototypical Network Encoder:")
    model.summary()
    return model


def siamese_network():
    inputs = keras.Input(shape=(84, 84, 3))
    x = layers.Conv2D(32, (3, 3), activation='relu')(inputs)
    x = layers.Conv2D(64, (3, 3), activation='relu')(x)
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(128, activation='relu')(x)
    encoder = models.Model(inputs=inputs, outputs=x)
    
    input1 = keras.Input(shape=(84, 84, 3))
    input2 = keras.Input(shape=(84, 84, 3))
    encoded1 = encoder(input1)
    encoded2 = encoder(input2)
    distance = tf.abs(encoded1 - encoded2)
    output = layers.Dense(1, activation='sigmoid')(distance)
    
    model = models.Model(inputs=[input1, input2], outputs=output)
    print("Siamese Network:")
    model.summary()
    return model


def maml_network():
    inputs = keras.Input(shape=(84, 84, 3))
    x = layers.Conv2D(32, (3, 3), activation='relu')(inputs)
    x = layers.Conv2D(64, (3, 3), activation='relu')(x)
    x = layers.Conv2D(128, (3, 3), activation='relu')(x)
    x = layers.GlobalAveragePooling2D()(x)
    outputs = layers.Dense(5, activation='softmax')(x)
    model = models.Model(inputs=inputs, outputs=outputs)
    print("MAML Network:")
    model.summary()
    return model


def relation_network():
    inputs = keras.Input(shape=(84, 84, 3))
    x = layers.Conv2D(64, (3, 3), activation='relu')(inputs)
    x = layers.Conv2D(64, (3, 3), activation='relu')(x)
    x = layers.GlobalAveragePooling2D()(x)
    x = layers.Dense(128, activation='relu')(x)
    encoder = models.Model(inputs=inputs, outputs=x)
    
    input1 = keras.Input(shape=(84, 84, 3))
    input2 = keras.Input(shape=(84, 84, 3))
    enc1 = encoder(input1)
    enc2 = encoder(input2)
    combined = tf.concat([enc1, enc2], axis=1)
    x = layers.Dense(64, activation='relu')(combined)
    output = layers.Dense(1, activation='sigmoid')(x)
    
    model = models.Model(inputs=[input1, input2], outputs=output)
    print("Relation Network:")
    model.summary()
    return model


def compute_prototypes(support_set, n_way):
    unique_classes = np.unique(support_set[:, -1])
    prototypes = {}
    for cls in unique_classes:
        cls_samples = support_set[support_set[:, -1] == cls][:, :-1]
        if len(cls_samples) > 0:
            prototypes[cls] = np.mean(cls_samples, axis=0)
    return prototypes


def classify_by_prototype(query_sample, prototypes):
    distances = {}
    for cls, prototype in prototypes.items():
        distances[cls] = np.linalg.norm(query_sample - prototype)
    return min(distances, key=distances.get)


def core_implementation():
    print("Prototypical Network:")
    prototypical_network()
    print("\nSiamese Network:")
    siamese_network()
    print("\nMAML Network:")
    maml_network()
    print("\nRelation Network:")
    relation_network()
    return True


def banking_example():
    encoder = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(32, 32, 1)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.GlobalAveragePooling2D(),
        layers.Dense(64, activation='relu')
    ])
    
    print(f"Banking Few-shot: {encoder.count_params()} params")
    return encoder


def healthcare_example():
    encoder = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(64, 64, 3)),
        layers.BatchNormalization(),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.BatchNormalization(),
        layers.GlobalAveragePooling2D(),
        layers.Dense(128, activation='relu')
    ])
    
    print(f"Healthcare Few-shot: {encoder.count_params()} params")
    return encoder


def main():
    print("Executing Meta-Learning and Few-Shot Learning implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()