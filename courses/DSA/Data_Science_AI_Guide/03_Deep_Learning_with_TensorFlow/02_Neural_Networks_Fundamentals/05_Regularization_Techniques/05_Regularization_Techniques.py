# Topic: Regularization Techniques
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Regularization Techniques

I. INTRODUCTION
   - Regularization prevents overfitting
   - Various techniques to improve generalization
   - Essential for production models

II. CORE_CONCEPTS
   - L1/L2 regularization
   - Dropout
   - Early stopping
   - Data augmentation

III. IMPLEMENTATION
   - Keras regularization API
   - Custom regularization
   - Training strategies
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models, regularizers, callbacks
import numpy as np


def l1_regularization():
    model = models.Sequential([
        layers.Dense(32, kernel_regularizer=regularizers.l1(0.01), 
                   input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    print("L1 Regularization:")
    for layer in model.layers:
        if hasattr(layer, 'kernel_regularizer'):
            print(f"  Regularizer: {layer.kernel_regularizer}")
    return model


def l2_regularization():
    model = models.Sequential([
        layers.Dense(32, kernel_regularizer=regularizers.l2(0.01), 
                   input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    print("L2 Regularization:")
    for layer in model.layers:
        if hasattr(layer, 'kernel_regularizer'):
            print(f"  Regularizer: {layer.kernel_regularizer}")
    return model


def l1_l2_regularization():
    model = models.Sequential([
        layers.Dense(32, kernel_regularizer=regularizers.l1_l2(l1=0.01, l2=0.01), 
                   input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    print("L1+L2 Regularization:")
    for layer in model.layers:
        if hasattr(layer, 'kernel_regularizer'):
            print(f"  Regularizer: {layer.kernel_regularizer}")
    return model


def dropout_layer():
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dropout(0.5),
        layers.Dense(32, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Dropout Layers:")
    model.summary()
    return model


def spatial_dropout():
    model = models.Sequential([
        layers.Conv2D(32, (3, 3), input_shape=(28, 28, 1)),
        layers.SpatialDropout2D(0.3),
        layers.Conv2D(64, (3, 3)),
        layers.Flatten(),
        layers.Dense(10)
    ])
    print("Spatial Dropout:")
    model.summary()
    return model


def alpha_dropout():
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.AlphaDropout(0.5),
        layers.Dense(32, activation='relu'),
        layers.AlphaDropout(0.3),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Alpha Dropout:")
    return model


def early_stopping():
    early_stop = callbacks.EarlyStopping(
        monitor='val_loss',
        patience=5,
        restore_best_weights=True,
        verbose=1
    )
    print("Early Stopping Config:")
    print(f"  monitor: {early_stop.monitor}")
    print(f"  patience: {early_stop.patience}")
    return early_stop


def model_checkpoint():
    checkpoint = callbacks.ModelCheckpoint(
        'best_model.keras',
        monitor='val_loss',
        save_best_only=True,
        verbose=1
    )
    print("Model Checkpoint Config:")
    print(f"  filepath: {checkpoint.filepath}")
    print(f"  monitor: {checkpoint.monitor}")
    return checkpoint


def reduce_lr_on_plateau():
    reduce_lr = callbacks.ReduceLROnPlateau(
        monitor='val_loss',
        factor=0.5,
        patience=3,
        min_lr=0.0001
    )
    print("Reduce LR Config:")
    print(f"  monitor: {reduce_lr.monitor}")
    print(f"  factor: {reduce_lr.factor}")
    return reduce_lr


def compare_regularization():
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    configs = [
        ('No Regularization', None, None),
        ('L2 (0.01)', regularizers.l2(0.01), None),
        ('Dropout (0.3)', None, 0.3),
        ('L2 + Dropout', regularizers.l2(0.01), 0.3)
    ]
    
    results = {}
    for name, reg, drop in configs:
        layers_list = [layers.Dense(32, kernel_regularizer=reg, input_shape=(10,))]
        if drop:
            layers_list.append(layers.Dropout(drop))
        layers_list.append(layers.Dense(1, activation='sigmoid'))
        
        model = models.Sequential(layers_list)
        model.compile(optimizer='adam', loss='binary_crossentropy')
        
        history = model.fit(X, y, epochs=10, verbose=0)
        results[name] = history.history['loss'][-1]
        print(f"{name}: {results[name]:.4f}")
    
    return results


def training_with_regularization():
    X = tf.random.normal([1000, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = models.Sequential([
        layers.Dense(64, kernel_regularizer=regularizers.l2(0.001), 
                   activation='relu', input_shape=(10,)),
        layers.Dropout(0.4),
        layers.Dense(32, kernel_regularizer=regularizers.l2(0.001), 
                   activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy')
    
    early_stop = callbacks.EarlyStopping(monitor='val_loss', patience=5)
    reduce_lr = callbacks.ReduceLROnPlateau(monitor='val_loss', factor=0.5, patience=3)
    
    history = model.fit(X, y, epochs=20, validation_split=0.2,
                       callbacks=[early_stop, reduce_lr], verbose=0)
    
    print(f"Training completed - Epochs: {len(history.history['loss'])}")
    print(f"Final val_loss: {history.history['val_loss'][-1]:.4f}")
    return model


def activity_regularization():
    model = models.Sequential([
        layers.Dense(32, activity_regularizer=regularizers.l2(0.01), 
                   input_shape=(10,)),
        layers.Dense(1)
    ])
    print("Activity Regularization:")
    return model


def core_implementation():
    print("L1 Regularization:")
    l1_regularization()
    print("\nL2 Regularization:")
    l2_regularization()
    print("\nDropout:")
    dropout_layer()
    print("\nCompare Regularization:")
    compare_regularization()
    print("\nTraining with Regularization:")
    training_with_regularization()
    return True


def banking_example():
    model = models.Sequential([
        layers.Dense(128, kernel_regularizer=regularizers.l1_l2(l1=0.001, l2=0.001), 
                   activation='relu', input_shape=(20,)),
        layers.BatchNormalization(),
        layers.Dropout(0.4),
        layers.Dense(64, kernel_regularizer=regularizers.l1_l2(l1=0.001, l2=0.001), 
                   activation='relu'),
        layers.BatchNormalization(),
        layers.Dropout(0.3),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([2000, 20])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0.3, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    early_stop = callbacks.EarlyStopping(monitor='val_loss', patience=10, restore_best_weights=True)
    
    history = model.fit(X, y, epochs=30, validation_split=0.2, callbacks=[early_stop], verbose=0)
    print(f"Banking - Best val_loss: {min(history.history['val_loss']):.4f}")
    return model


def healthcare_example():
    model = models.Sequential([
        layers.Dense(256, kernel_regularizer=regularizers.l2(0.01), 
                   activation='relu', input_shape=(50,)),
        layers.BatchNormalization(),
        layers.Dropout(0.5),
        layers.Dense(128, kernel_regularizer=regularizers.l2(0.01), 
                   activation='relu'),
        layers.BatchNormalization(),
        layers.Dropout(0.4),
        layers.Dense(64, kernel_regularizer=regularizers.l2(0.01), 
                   activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(5, activation='softmax')
    ])
    
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    
    X = tf.random.normal([1000, 50])
    y = tf.cast(tf.reduce_mean(X, axis=1) * 4, tf.int32)
    y = tf.one_hot(y, depth=5)
    
    reduce_lr = callbacks.ReduceLROnPlateau(monitor='val_loss', factor=0.5, patience=5)
    
    history = model.fit(X, y, epochs=30, validation_split=0.2, callbacks=[reduce_lr], verbose=0)
    print(f"Healthcare - Best val_accuracy: {max(history.history['val_accuracy']):.4f}")
    return model


def main():
    print("Executing Regularization Techniques implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()