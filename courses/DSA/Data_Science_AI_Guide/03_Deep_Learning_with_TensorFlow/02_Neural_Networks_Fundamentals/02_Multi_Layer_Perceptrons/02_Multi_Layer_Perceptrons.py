# Topic: Multi-Layer Perceptrons
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Multi-Layer Perceptrons

I. INTRODUCTION
   - MLP consists of input, hidden, and output layers
   - Can learn non-linear decision boundaries
   - Universal function approximators

II. CORE_CONCEPTS
   - Network architecture design
   - Forward propagation
   - Hidden layer operations
   - Output layer transformations

III. IMPLEMENTATION
   - MLP model creation
   - Training pipeline
   - Evaluation metrics
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models, callbacks
import numpy as np


def simple_mlp():
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Simple MLP:")
    model.summary()
    return model


def deep_mlp():
    model = models.Sequential([
        layers.Dense(256, activation='relu', input_shape=(20,)),
        layers.Dropout(0.3),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(64, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Deep MLP:")
    model.summary()
    return model


def functional_mlp():
    inputs = keras.Input(shape=(10,))
    x = layers.Dense(64, activation='relu')(inputs)
    x = layers.Dense(32, activation='relu')(x)
    outputs = layers.Dense(1, activation='sigmoid')(x)
    model = models.Model(inputs=inputs, outputs=outputs)
    print("Functional MLP:")
    model.summary()
    return model


def multi_class_mlp():
    model = models.Sequential([
        layers.Dense(128, activation='relu', input_shape=(10,)),
        layers.Dense(64, activation='relu'),
        layers.Dense(32, activation='relu'),
        layers.Dense(5, activation='softmax')
    ])
    print("Multi-class MLP:")
    model.summary()
    return model


def regression_mlp():
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(1)
    ])
    print("Regression MLP:")
    model.summary()
    return model


def custom_mlp_class(models.Model):
    def __init__(self, input_dim, hidden_units, output_dim):
        super().__init__()
        self.hidden_layers = []
        for units in hidden_units:
            self.hidden_layers.append(layers.Dense(units, activation='relu'))
        self.output_layer = layers.Dense(output_dim, activation='sigmoid')
    
    def call(self, inputs):
        x = inputs
        for layer in self.hidden_layers:
            x = layer(x)
        return self.output_layer(x)


def manual_forward_pass():
    weights1 = tf.Variable(tf.random.normal([10, 32], stddev=0.3))
    bias1 = tf.Variable(tf.zeros([32]))
    weights2 = tf.Variable(tf.random.normal([32, 1], stddev=0.3))
    bias2 = tf.Variable(tf.zeros([1]))
    
    x = tf.random.normal([1, 10])
    
    hidden = tf.nn.relu(tf.matmul(x, weights1) + bias1)
    output = tf.nn.sigmoid(tf.matmul(hidden, weights2) + bias2)
    
    print(f"Input shape: {x.shape}")
    print(f"Hidden shape: {hidden.shape}")
    print(f"Output shape: {output.shape}")
    return output


def train_mlp():
    X = tf.random.normal([1000, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(16, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    history = model.fit(X, y, epochs=10, verbose=0)
    
    print(f"Training loss: {history.history['loss'][-1]:.4f}")
    print(f"Training accuracy: {history.history['accuracy'][-1]:.4f}")
    return model, history


def train_with_validation():
    X = tf.random.normal([1000, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(10,)),
        layers.Dropout(0.2),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    
    early_stop = callbacks.EarlyStopping(monitor='val_loss', patience=5)
    history = model.fit(X, y, epochs=20, validation_split=0.2, 
                       callbacks=[early_stop], verbose=0)
    
    print(f"Epochs trained: {len(history.history['loss'])}")
    print(f"Final val_loss: {history.history['val_loss'][-1]:.4f}")
    return model, history


def compare_optimizers():
    X = tf.random.normal([500, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    optimizers = ['adam', 'sgd', 'rmsprop']
    results = {}
    
    for opt in optimizers:
        model = models.Sequential([
            layers.Dense(32, activation='relu', input_shape=(10,)),
            layers.Dense(1, activation='sigmoid')
        ])
        model.compile(optimizer=opt, loss='binary_crossentropy')
        history = model.fit(X, y, epochs=10, verbose=0)
        results[opt] = history.history['loss'][-1]
        print(f"{opt}: final loss = {results[opt]:.4f}")
    
    return results


def batch_training():
    X = tf.random.normal([1000, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy')
    history = model.fit(X, y, epochs=5, batch_size=32, verbose=0)
    
    print(f"Final loss with batch_size=32: {history.history['loss'][-1]:.4f}")
    return model


def predict_with_mlp():
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    _ = model(tf.random.normal([1, 10]))
    
    test_data = tf.random.normal([10, 10])
    predictions = model.predict(test_data, verbose=0)
    
    print(f"Predictions shape: {predictions.shape}")
    print(f"Sample predictions: {predictions[:5].flatten()}")
    return predictions


def evaluate_mlp():
    X = tf.random.normal([200, 10])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
    _ = model.fit(X, y, epochs=5, verbose=0)
    
    test_X = tf.random.normal([50, 10])
    test_y = tf.cast(tf.reduce_mean(test_X, axis=1) > 0, tf.float32)
    test_y = tf.expand_dims(test_y, axis=1)
    
    results = model.evaluate(test_X, test_y, verbose=0)
    print(f"Test loss: {results[0]:.4f}")
    print(f"Test accuracy: {results[1]:.4f}")
    return results


def save_load_mlp():
    model = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(10,)),
        layers.Dense(1, activation='sigmoid')
    ])
    model.compile(optimizer='adam', loss='binary_crossentropy')
    
    model.save('mlp_model.keras')
    loaded = models.load_model('mlp_model.keras')
    
    test = tf.random.normal([1, 10])
    orig_pred = model.predict(test, verbose=0)
    loaded_pred = loaded.predict(test, verbose=0)
    
    print(f"Predictions match: {np.allclose(orig_pred, loaded_pred)}")
    return model


def core_implementation():
    print("Simple MLP:")
    simple_mlp()
    print("\nDeep MLP:")
    deep_mlp()
    print("\nFunctional MLP:")
    functional_mlp()
    print("\nManual Forward Pass:")
    manual_forward_pass()
    print("\nTrain MLP:")
    train_mlp()
    return True


def banking_example():
    def create_fraud_model():
        model = models.Sequential([
            layers.Dense(128, activation='relu', input_shape=(20,)),
            layers.Dropout(0.3),
            layers.Dense(64, activation='relu'),
            layers.Dropout(0.3),
            layers.Dense(32, activation='relu'),
            layers.Dense(1, activation='sigmoid')
        ])
        model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])
        return model
    
    X = tf.random.normal([2000, 20])
    y = tf.cast(tf.reduce_mean(X, axis=1) > 0.5, tf.float32)
    y = tf.expand_dims(y, axis=1)
    
    model = create_fraud_model()
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    
    print(f"Fraud detection - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def healthcare_example():
    def create_diagnosis_model():
        model = models.Sequential([
            layers.Dense(256, activation='relu', input_shape=(50,)),
            layers.BatchNormalization(),
            layers.Dropout(0.3),
            layers.Dense(128, activation='relu'),
            layers.BatchNormalization(),
            layers.Dropout(0.3),
            layers.Dense(64, activation='relu'),
            layers.Dense(5, activation='softmax')
        ])
        model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
        return model
    
    X = tf.random.normal([1000, 50])
    y = tf.random.uniform([1000]) * 4
    y = tf.cast(y, tf.int32)
    y = tf.one_hot(y, depth=5)
    
    model = create_diagnosis_model()
    history = model.fit(X, y, epochs=10, validation_split=0.2, verbose=0)
    
    print(f"Diagnosis model - Val accuracy: {history.history['val_accuracy'][-1]:.4f}")
    return model


def main():
    print("Executing Multi-Layer Perceptrons implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()