# Topic: Keras API Introduction
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Keras API Introduction

I. INTRODUCTION
   - Keras is the official high-level API for TensorFlow
   - Provides modular and extensible building blocks
   - Simplifies deep learning model creation

II. CORE_CONCEPTS
   - Sequential API
   - Functional API
   - Model subclassing
   - Layers and callbacks

III. IMPLEMENTATION
   - Model creation
   - Training流程
   - Evaluation
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models, callbacks
import numpy as np


def sequential_api_example():
    model = keras.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Sequential API Model:")
    model.summary()
    return model


def functional_api_example():
    inputs = keras.Input(shape=(10,))
    x = layers.Dense(64, activation='relu')(inputs)
    x = layers.Dense(32, activation='relu')(x)
    outputs = layers.Dense(1, activation='sigmoid')(x)
    
    model = keras.Model(inputs=inputs, outputs=outputs)
    print("\nFunctional API Model:")
    model.summary()
    return model


def model_subclassing_example():
    class MyModel(keras.Model):
        def __init__(self):
            super().__init__()
            self.dense1 = layers.Dense(64, activation='relu')
            self.dense2 = layers.Dense(32, activation='relu')
            self.output_layer = layers.Dense(1, activation='sigmoid')
        
        def call(self, inputs):
            x = self.dense1(inputs)
            x = self.dense2(x)
            return self.output_layer(x)
    
    model = MyModel()
    _ = model(tf.ones((1, 10)))
    print("\nModel Subclassing:")
    model.summary()
    return model


def compile_and_fit():
    model = keras.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1)
    ])
    
    model.compile(
        optimizer='adam',
        loss='mse',
        metrics=['mae']
    )
    
    x_train = np.random.randn(1000, 10)
    y_train = np.random.randn(1000, 1)
    
    history = model.fit(
        x_train, y_train,
        epochs=10,
        batch_size=32,
        validation_split=0.2,
        verbose=0
    )
    print(f"\nTraining - Loss: {history.history['loss'][-1]:.4f}")
    return model, history


def custom_layer():
    class CustomDense(layers.Layer):
        def __init__(self, units, **kwargs):
            super().__init__(**kwargs)
            self.units = units
        
        def build(self, input_shape):
            self.kernel = self.add_weight(
                name='kernel',
                shape=(input_shape[-1], self.units),
                initializer='glorot_uniform',
                trainable=True
            )
            self.bias = self.add_weight(
                name='bias',
                shape=(self.units,),
                initializer='zeros',
                trainable=True
            )
        
        def call(self, inputs):
            return tf.matmul(inputs, self.kernel) + self.bias
        
        def get_config(self):
            config = super().get_config()
            config.update({'units': self.units})
            return config
    
    layer = CustomDense(32)
    print(f"\nCustom layer output shape: {layer(tf.ones((1, 10))).shape}")
    return layer


def callbacks_example():
    early_stop = callbacks.EarlyStopping(
        monitor='val_loss',
        patience=3,
        restore_best_weights=True
    )
    
    reduce_lr = callbacks.ReduceLROnPlateau(
        monitor='val_loss',
        factor=0.5,
        patience=2
    )
    
    model = keras.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dense(1)
    ])
    
    model.compile(optimizer='adam', loss='mse')
    
    x_train = np.random.randn(500, 10)
    y_train = np.random.randn(500, 1)
    
    history = model.fit(
        x_train, y_train,
        epochs=20,
        batch_size=32,
        validation_split=0.2,
        callbacks=[early_stop, reduce_lr],
        verbose=0
    )
    print(f"\nEarly stopping at epoch: {len(history.history['loss'])}")
    return model


def save_and_load():
    model = keras.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1)
    ])
    model.compile(optimizer='adam', loss='mse')
    
    model.save('my_model.keras')
    loaded_model = keras.models.load_model('my_model.keras')
    
    predictions = model.predict(tf.ones((1, 10)), verbose=0)
    loaded_predictions = loaded_model.predict(tf.ones((1, 10)), verbose=0)
    print(f"\nPredictions match: {np.allclose(predictions, loaded_predictions)}")
    return model


def multi_input_output():
    input1 = keras.Input(shape=(5,), name='input1')
    input2 = keras.Input(shape=(5,), name='input2')
    
    x1 = layers.Dense(16, activation='relu')(input1)
    x2 = layers.Dense(16, activation='relu')(input2)
    
    concatenated = layers.Concatenate()([x1, x2])
    output = layers.Dense(1)(concatenated)
    
    model = keras.Model(inputs=[input1, input2], outputs=output)
    print("\nMulti-input model:")
    model.summary()
    return model


def shared_layers():
    shared_dense = layers.Dense(32, activation='relu')
    
    input1 = keras.Input(shape=(10,))
    input2 = keras.Input(shape=(10,))
    
    output1 = shared_dense(input1)
    output2 = shared_dense(input2)
    
    output = layers.Add()([output1, output2])
    model = keras.Model(inputs=[input1, input2], outputs=output)
    print("\nShared layers model:")
    model.summary()
    return model


def batch_normalization_layer():
    model = keras.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Dense(1)
    ])
    print("\nBatch normalization model:")
    model.summary()
    return model


def dropout_layer():
    model = keras.Sequential([
        layers.Dense(64, input_shape=(10,)),
        layers.Dropout(0.3),
        layers.Dense(32),
        layers.Dropout(0.3),
        layers.Dense(1)
    ])
    print("\nDropout model:")
    model.summary()
    return model


def conv_layer():
    model = keras.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(28, 28, 1)),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.Flatten(),
        layers.Dense(10)
    ])
    print("\nConv model:")
    model.summary()
    return model


def recurrent_layer():
    model = keras.Sequential([
        layers.LSTM(32, input_shape=(10, 1)),
        layers.Dense(1)
    ])
    print("\nLSTM model:")
    model.summary()
    return model


def core_implementation():
    sequential_api_example()
    functional_api_example()
    model_subclassing_example()
    print("\nCompile and Fit:")
    compile_and_fit()
    print("\nCustom Layer:")
    custom_layer()
    return True


def banking_example():
    fraud_model = keras.Sequential([
        layers.Dense(128, activation='relu', input_shape=(20,)),
        layers.Dropout(0.3),
        layers.Dense(64, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    fraud_model.compile(
        optimizer='adam',
        loss='binary_crossentropy',
        metrics=['accuracy', 'precision', 'recall']
    )
    
    print("\nBanking Fraud Detection Model:")
    fraud_model.summary()
    return fraud_model


def healthcare_example():
    diagnosis_model = keras.Sequential([
        layers.Dense(256, activation='relu', input_shape=(50,)),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Dropout(0.3),
        layers.Dense(128, activation='relu'),
        layers.BatchNormalization(),
        layers.ReLU(),
        layers.Dropout(0.3),
        layers.Dense(64, activation='relu'),
        layers.Dense(5, activation='softmax')
    ])
    
    diagnosis_model.compile(
        optimizer='adam',
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    print("\nHealthcare Diagnosis Model:")
    diagnosis_model.summary()
    return diagnosis_model


def main():
    print("Executing Keras API Introduction implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()