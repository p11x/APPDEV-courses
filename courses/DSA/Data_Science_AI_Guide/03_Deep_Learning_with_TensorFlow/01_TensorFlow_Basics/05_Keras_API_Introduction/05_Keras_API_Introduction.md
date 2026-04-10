# Keras API Introduction

## I. INTRODUCTION

### What is Keras?
Keras is the official high-level API of TensorFlow, designed to enable fast experimentation with deep neural networks. It provides a clean, modular, and extensible API for building and training models, making it accessible to both beginners and advanced researchers.

Keras was created by François Chollet in 2015 as an interface to multiple deep learning backends (TensorFlow, Theano, CNTK). In TensorFlow 2.x, Keras became the central API, deeply integrated into TensorFlow's ecosystem. The name "Keras" (κέρας) means "horn" in Greek, referring to a Greek word meaning "to create."

### Why is Keras Important?

1. **Simplicity**: Keras provides an intuitive interface that abstracts away low-level details
2. **Flexibility**: Easy to extend with custom layers, losses, and metrics
3. **Production-Ready**: Models built with Keras can be exported and deployed
4. **Integration**: Part of TensorFlow, benefiting from its ecosystem
5. **Research-Friendly**: Widely used in research papers and industry

### Prerequisites
- Python programming
- Basic understanding of tensors
- Neural network fundamentals
- TensorFlow basics (modules 1-4)

## II. FUNDAMENTALS

### Keras Architecture

Keras provides several ways to build models:

1. **Sequential API**: Simple linear stack of layers
2. **Functional API**: Complex architectures with multiple inputs/outputs
3. **Subclassing API**: Full customization with forward pass definition

### Core Components

**Layers**: The building blocks of neural networks. Layers accept inputs, perform operations, and produce outputs.

**Models**: The complete neural network that holds layers together.

**Optimizers**: Algorithms for updating weights (Adam, SGD, RMSprop)

**Losses**: Functions to minimize during training

**Metrics**: Evaluation measures (accuracy, precision, recall)

## III. IMPLEMENTATION

### Sequential API

```python
"""
Keras API Introduction - Module 5
This module demonstrates Keras API for building neural networks.
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class KerasSequential:
    """Sequential API demonstrations."""
    
    @staticmethod
    def basic_sequential():
        """Basic Sequential model."""
        print("Basic Sequential Model:")
        print("="*50)
        
        model = keras.Sequential([
            layers.Dense(64, activation='relu', input_shape=(784,)),
            layers.Dense(64, activation='relu'),
            layers.Dense(10, activation='softmax')
        ])
        
        model.compile(
            optimizer='adam',
            loss='categorical_crossentropy',
            metrics=['accuracy']
        )
        
        model.summary()
        
        return model
    
    @staticmethod
    def adding_layers():
        """Adding layers to Sequential model."""
        print("\n" + "="*50)
        print("Adding Layers Dynamically:")
        print("="*50)
        
        model = keras.Sequential(name='my_model')
        
        # Add layers one by one
        model.add(layers.Dense(32, activation='relu', input_shape=(10,)))
        model.add(layers.Dense(16, activation='relu'))
        model.add(layers.Dense(1, activation='sigmoid'))
        
        print(f"Total layers: {len(model.layers)}")
        
        # Summary shows architecture
        model.summary()
        
        return model
    
    @staticmethod
    def custom_sequential():
        """Sequential with custom configuration."""
        print("\n" + "="*50)
        print("Custom Sequential Model:")
        print("="*50)
        
        # Pre-configured models
        from tensorflow.keras.regularizers import l2
        
        model = keras.Sequential([
            layers.Dense(128, activation='relu', 
                        kernel_regularizer=l2(0.001),
                        input_shape=(20,)),
            layers.Dropout(0.3),
            layers.Dense(64, activation='relu',
                        kernel_regularizer=l2(0.001)),
            layers.Dropout(0.3),
            layers.Dense(10, activation='softmax')
        ])
        
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.001),
            loss='categorical_crossentropy',
            metrics=['accuracy', 'mae']
        )
        
        return model

class KerasFunctional:
    """Functional API demonstrations."""
    
    @staticmethod
    def basic_functional():
        """Basic Functional model."""
        print("="*50)
        print("Basic Functional API:")
        print("="*50)
        
        # Input layer
        inputs = keras.Input(shape=(784,), name='input')
        
        # First dense block
        x = layers.Dense(64, activation='relu', name='dense1')(inputs)
        
        # Second dense block
        x = layers.Dense(64, activation='relu', name='dense2')(x)
        
        # Output layer
        outputs = layers.Dense(10, activation='softmax', name='output')(x)
        
        # Create model
        model = keras.Model(inputs=inputs, outputs=outputs, name='functional_model')
        
        model.compile(
            optimizer='adam',
            loss='categorical_crossentropy',
            metrics=['accuracy']
        )
        
        model.summary()
        
        return model
    
    @staticmethod
    def multi_input_output():
        """Multiple inputs and outputs."""
        print("\n" + "="*50)
        print("Multi-Input Multi-Output:")
        print("="*50)
        
        # Image input branch
        image_input = keras.Input(shape=(224, 224, 3), name='image')
        x1 = layers.Conv2D(64, 3, activation='relu')(image_input)
        x1 = layers.Flatten()(x1)
        x1 = layers.Dense(64, activation='relu')(x1)
        
        # Text input branch
        text_input = keras.Input(shape=(100,), name='text')
        x2 = layers.Dense(64, activation='relu')(text_input)
        x2 = layers.Dense(64, activation='relu')(x2)
        
        # Combine branches
        combined = layers.Concatenate()([x1, x2])
        
        # Outputs
        classification = layers.Dense(10, activation='softmax', name='class')(combined)
        regression = layers.Dense(1, name='regress')(combined)
        
        # Create model with 2 inputs, 2 outputs
        model = keras.Model(
            inputs=[image_input, text_input],
            outputs=[classification, regression]
        )
        
        model.compile(
            optimizer='adam',
            loss=['categorical_crossentropy', 'mse'],
            metrics=['accuracy', 'mae']
        )
        
        model.summary()
        
        return model
    
    @staticmethod
    def shared_layers():
        """Shared layers in Functional API."""
        print("\n" + "="*50)
        print("Shared Layers:")
        print("="*50)
        
        # Shared embedding layer
        shared_dense = layers.Dense(64, activation='relu')
        
        # Two inputs
        input_a = keras.Input(shape=(100,), name='input_a')
        input_b = keras.Input(shape=(100,), name='input_b')
        
        # Use shared layer
        output_a = shared_dense(input_a)
        output_b = shared_dense(input_b)
        
        # Combine
        combined = layers.Concatenate()([output_a, output_b])
        output = layers.Dense(10, activation='softmax')(combined)
        
        model = keras.Model(inputs=[input_a, input_b], outputs=output)
        
        return model

class KerasSubclassing:
    """Subclassing API demonstrations."""
    
    @staticmethod
    def custom_model():
        """Custom Model class."""
        print("\n" + "="*50)
        print("Subclassing API:")
        print("="*50)
        
        class SimpleModel(keras.Model):
            """Simple custom model."""
            
            def __init__(self, input_dim, output_dim):
                super().__init__()
                self.dense1 = layers.Dense(64, activation='relu')
                self.dense2 = layers.Dense(32, activation='relu')
                self.output_layer = layers.Dense(output_dim, activation='softmax')
            
            def call(self, inputs, training=None):
                """Forward pass."""
                x = self.dense1(inputs)
                x = self.dense2(x)
                return self.output_layer(x)
        
        # Create model
        model = SimpleModel(input_dim=784, output_dim=10)
        
        # Build (creates weights)
        model.build(input_shape=(None, 784))
        
        model.compile(
            optimizer='adam',
            loss='categorical_crossentropy',
            metrics=['accuracy']
        )
        
        model.summary()
        
        return model
    
    @staticmethod
    def custom_layer():
        """Custom layer implementation."""
        print("\n" + "="*50)
        print("Custom Layer:")
        print("="*50)
        
        class CustomDense(layers.Layer):
            """Custom dense layer with residual connection."""
            
            def __init__(self, units, **kwargs):
                super().__init__(**kwargs)
                self.units = units
            
            def build(self, input_shape):
                self.kernel = self.add_weight(
                    'kernel',
                    shape=(input_shape[-1], self.units),
                    initializer='glorot_uniform',
                    trainable=True
                )
                self.bias = self.add_weight(
                    'bias',
                    shape=(self.units,),
                    initializer='zeros',
                    trainable=True
                )
            
            def call(self, inputs):
                output = tf.matmul(inputs, self.kernel) + self.bias
                # Residual connection
                if inputs.shape[-1] == self.units:
                    output = output + inputs
                return tf.nn.relu(output)
            
            def get_config(self):
                config = super().get_config()
                config.update({'units': self.units})
                return config
        
        # Use custom layer
        model = keras.Sequential([
            layers.Input(shape=(10,)),
            CustomDense(10),
            CustomDense(10),
            layers.Dense(1, activation='sigmoid')
        ])
        
        return model

def demonstrate_keras_api():
    """Demonstrate all Keras API methods."""
    print("="*60)
    print("KERAS API DEMONSTRATIONS")
    print("="*60)
    
    seq = KerasSequential()
    seq.basic_sequential()
    seq.adding_layers()
    seq.custom_sequential()
    
    func = KerasFunctional()
    func.basic_functional()
    func.multi_input_output()
    func.shared_layers()
    
    sub = KerasSubclassing()
    sub.custom_model()
    sub.custom_layer()

### Standard Example: Image Classification

```python
"""
Standard Example: MNIST Image Classification with Keras
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class MNISTClassifier:
    """MNIST digit classifier with Keras."""
    
    def __init__(self):
        self.model = None
        self.history = None
    
    def load_data(self):
        """Load and preprocess MNIST data."""
        (x_train, y_train), (x_test, y_test) = keras.datasets.mnist.load_data()
        
        # Normalize
        x_train = x_train.astype('float32') / 255.0
        x_test = x_test.astype('float32') / 255.0
        
        # One-hot encode
        num_classes = 10
        y_train = keras.utils.to_categorical(y_train, num_classes)
        y_test = keras.utils.to_categorical(y_test, num_classes)
        
        print(f"Training: {x_train.shape}, Labels: {y_train.shape}")
        print(f"Test: {x_test.shape}, Labels: {y_test.shape}")
        
        return (x_train, y_train), (x_test, y_test)
    
    def build_model(self):
        """Build CNN model."""
        model = keras.Sequential([
            layers.Conv2D(32, (3, 3), activation='relu', 
                         input_shape=(28, 28, 1), padding='same'),
            layers.BatchNormalization(),
            layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
            layers.MaxPooling2D((2, 2)),
            layers.Dropout(0.25),
            
            layers.Conv2D(128, (3, 3), activation='relu', padding='same'),
            layers.BatchNormalization(),
            layers.Dropout(0.25),
            
            layers.Flatten(),
            layers.Dense(256, activation='relu'),
            layers.BatchNormalization(),
            layers.Dropout(0.5),
            layers.Dense(10, activation='softmax')
        ])
        
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.001),
            loss='categorical_crossentropy',
            metrics=['accuracy']
        )
        
        self.model = model
        return model
    
    def train(self, x_train, y_train, epochs=5, batch_size=32):
        """Train the model."""
        self.history = self.model.fit(
            x_train, y_train,
            epochs=epochs,
            batch_size=batch_size,
            validation_split=0.1,
            verbose=1
        )
        
        return self.history
    
    def evaluate(self, x_test, y_test):
        """Evaluate model."""
        results = self.model.evaluate(x_test, y_test, verbose=1)
        
        print(f"\nTest Loss: {results[0]:.4f}")
        print(f"Test Accuracy: {results[1]:.4f}")
        
        return results
    
    def predict(self, x_test):
        """Make predictions."""
        predictions = self.model.predict(x_test)
        predicted_classes = np.argmax(predictions, axis=1)
        
        return predicted_classes

def run_mnist_example():
    """Run MNIST classification example."""
    print("="*60)
    print("MNIST IMAGE CLASSIFICATION WITH KERAS")
    print("="*60)
    
    classifier = MNISTClassifier()
    
    # Load data
    (x_train, y_train), (x_test, y_test) = classifier.load_data()
    
    # Build model
    classifier.build_model()
    
    # Train
    classifier.train(x_train, y_train, epochs=3)
    
    # Evaluate
    classifier.evaluate(x_test, y_test)
    
    print("\nMNIST Example Complete!")

if __name__ == "__main__":
    run_mnist_example()
```

### Real-world Example 1: Banking - Credit Risk Classification

```python
"""
Real-world Example 1: Banking - Credit Risk Classification
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class CreditRiskModel:
    """
    Credit risk classification model for banking.
    Predicts loan default probability.
    """
    
    def __init__(self, n_features):
        self.n_features = n_features
        self.model = None
    
    def build_model(self):
        """Build credit risk model."""
        # Input layer
        inputs = keras.Input(shape=(self.n_features,), name='features')
        
        # Feature engineering
        x = layers.Dense(128, activation='relu', name='dense1')(inputs)
        x = layers.BatchNormalization(name='bn1')(x)
        x = layers.Dropout(0.3, name='dropout1')(x)
        
        x = layers.Dense(64, activation='relu', name='dense2')(x)
        x = layers.BatchNormalization(name='bn2')(x)
        x = layers.Dropout(0.3, name='dropout2')(x)
        
        x = layers.Dense(32, activation='relu', name='dense3')(x)
        
        # Binary output (default/no default)
        outputs = layers.Dense(1, activation='sigmoid', name='output')(x)
        
        model = keras.Model(inputs=inputs, outputs=outputs, name='credit_risk')
        
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.001),
            loss='binary_crossentropy',
            metrics=[
                'accuracy',
                keras.metrics.AUC(name='auc'),
                keras.metrics.Precision(name='precision'),
                keras.metrics.Recall(name='recall')
            ]
        )
        
        self.model = model
        return model
    
    def generate_data(self, n_samples):
        """Generate synthetic credit data."""
        np.random.seed(42)
        
        # Features: income, debt, credit_score, employment_years, etc.
        features = np.random.randn(n_samples, 15).astype('float32')
        
        # Add noise to make realistic
        features[:, 0] = np.random.uniform(20000, 200000, n_samples)  # income
        features[:, 1] = np.random.uniform(0, 50000, n_samples)  # debt
        features[:, 2] = np.random.uniform(300, 850, n_samples)  # credit score
        features[:, 3] = np.random.uniform(0, 30, n_samples)  # employment
        features[:, 4:] = np.random.randn(n_samples, 11)
        
        # Normalize
        features = (features - features.mean(axis=0)) / features.std(axis=0)
        
        # Labels (15% default rate)
        labels = np.random.binomial(1, 0.15, n_samples).astype('float32')
        
        return features, labels

def run_banking_example():
    """Run banking example."""
    print("="*60)
    print("CREDIT RISK CLASSIFICATION - BANKING")
    print("="*60)
    
    # Create model
    credit_model = CreditRiskModel(n_features=15)
    model = credit_model.build_model()
    
    # Generate data
    features, labels = credit_model.generate_data(10000)
    
    print(f"Data shape: {features.shape}")
    print(f"Default rate: {labels.mean():.2%}")
    
    # Train
    model.fit(
        features, labels,
        epochs=10,
        batch_size=64,
        validation_split=0.2,
        verbose=1
    )
    
    # Evaluate
    test_features, test_labels = credit_model.generate_data(1000)
    results = model.evaluate(test_features, test_labels, verbose=1)
    
    print(f"\nTest AUC: {results[3]:.4f}")
    print(f"Test Precision: {results[4]:.4f}")
    print(f"Test Recall: {results[5]:.4f}")
    
    print("\nCredit Risk Model Complete!")

if __name__ == "__main__":
    run_banking_example()
```

### Real-world Example 2: Healthcare - Medical Image Analysis

```python
"""
Real-world Example 2: Healthcare - Medical Image Classification
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class MedicalImageClassifier:
    """
    Medical image classification for disease detection.
    Classifies chest X-rays as normal or abnormal.
    """
    
    def __init__(self, img_size):
        self.img_size = img_size
        self.model = None
    
    def build_model(self):
        """Build medical image classifier."""
        inputs = keras.Input(
            shape=(self.img_size, self.img_size, 3), 
            name='xray_image'
        )
        
        # Data augmentation (for medical images)
        x = layers.RandomFlip(mode='horizontal')(inputs)
        x = layers.RandomRotation(0.1)(x)
        x = layers.RandomZoom(0.1)(x)
        
        # Feature extraction
        x = layers.Conv2D(32, 3, activation='relu', padding='same')(x)
        x = layers.BatchNormalization()(x)
        x = layers.MaxPooling2D(2)(x)
        
        x = layers.Conv2D(64, 3, activation='relu', padding='same')(x)
        x = layers.BatchNormalization()(x)
        x = layers.MaxPooling2D(2)(x)
        
        x = layers.Conv2D(128, 3, activation='relu', padding='same')(x)
        x = layers.BatchNormalization()(x)
        x = layers.MaxPooling2D(2)(x)
        
        x = layers.Conv2D(256, 3, activation='relu', padding='same')(x)
        x = layers.GlobalAveragePooling2D()(x)
        
        # Classification head
        x = layers.Dense(128, activation='relu')(x)
        x = layers.Dropout(0.5)(x)
        
        # Binary classification
        outputs = layers.Dense(1, activation='sigmoid', name='diagnosis')(x)
        
        model = keras.Model(inputs=inputs, outputs=outputs, name='xray_classifier')
        
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.0001),
            loss='binary_crossentropy',
            metrics=['accuracy', 'auc', 'precision', 'recall']
        )
        
        self.model = model
        return model
    
    def generate_data(self, n_samples):
        """Generate synthetic medical images."""
        np.random.seed(42)
        
        images = []
        labels = []
        
        for _ in range(n_samples):
            # Create synthetic image (normal or abnormal)
            label = np.random.binomial(1, 0.3)  # 30% abnormal
            
            # Base image
            img = np.random.randn(
                self.img_size, self.img_size, 3
            ).astype('float32')
            
            # Add patterns for abnormal
            if label == 1:
                for _ in range(5):
                    x, y = np.random.randint(0, self.img_size, 2)
                    r = np.random.randint(5, 15)
                    for i in range(self.img_size):
                        for j in range(self.img_size):
                            if (i-x)**2 + (j-y)**2 < r**2:
                                img[i, j] -= 0.5
            
            # Normalize
            img = (img - img.min()) / (img.max() - img.min() + 1e-8)
            
            images.append(img)
            labels.append(label)
        
        return np.array(images), np.array(labels)

def run_healthcare_example():
    """Run healthcare example."""
    print("="*60)
    print("MEDICAL IMAGE CLASSIFICATION - HEALTHCARE")
    print("="*60)
    
    classifier = MedicalImageClassifier(img_size=128)
    model = classifier.build_model()
    
    # Generate data
    images, labels = classifier.generate_data(5000)
    
    print(f"Images: {images.shape}, Labels: {labels.shape}")
    print(f"Abnormal rate: {labels.mean():.2%}")
    
    # Train
    model.fit(
        images, labels,
        epochs=10,
        batch_size=32,
        validation_split=0.2,
        verbose=1
    )
    
    # Evaluate
    test_images, test_labels = classifier.generate_data(1000)
    results = model.evaluate(test_images, test_labels, verbose=1)
    
    print(f"\nTest AUC: {results[2]:.4f}")
    print("\nMedical Image Classification Complete!")

if __name__ == "__main__":
    run_healthcare_example()
```

## V. OUTPUT_RESULTS

```
KERAS API DEMONSTRATIONS
============================================================
Model: "sequential"
Layer (type)                 Output Shape              Param #   
================================================================
dense (Dense)                (None, 64)                50240     
...
Total params: 1,346,762
Trainable params: 1,346,762
Non-trainable params: 0
```

## VI. ADVANCED TOPICS

### Callbacks

```python
"""
Advanced Topic: Keras Callbacks
"""

class CustomCallbacks:
    """Callback demonstrations."""
    
    @staticmethod
    def standard_callbacks():
        """Standard Keras callbacks."""
        callbacks = [
            # Stop if no improvement
            keras.callbacks.EarlyStopping(
                monitor='val_loss',
                patience=5,
                restore_best_weights=True
            ),
            
            # Save best model
            keras.callbacks.ModelCheckpoint(
                'best_model.h5',
                monitor='val_accuracy',
                save_best_only=True
            ),
            
            # Learning rate scheduling
            keras.callbacks.ReduceLROnPlateau(
                monitor='val_loss',
                factor=0.5,
                patience=3
            ),
            
            # TensorBoard
            keras.callbacks.TensorBoard(
                log_dir='./logs',
                histogram_freq=1
            )
        ]
        
        return callbacks
    
    @staticmethod
    def custom_callback():
        """Custom callback."""
        class TrainingMonitor(keras.callbacks.Callback):
            def on_epoch_end(self, epoch, logs=None):
                print(f"\nEpoch {epoch+1}: loss={logs['loss']:.4f}")
        
        return TrainingMonitor()
```

### Model Saving and Loading

```python
"""
Advanced Topic: Model Saving/Loading
"""

class ModelPersistence:
    """Model saving and loading."""
    
    @staticmethod
    def save_whole_model():
        """Save entire model."""
        model = keras.Sequential([
            layers.Dense(10, activation='relu'),
            layers.Dense(1, activation='sigmoid')
        ])
        
        # Save in H5 format
        model.save('model.h5')
        
        # Load back
        loaded = keras.models.load_model('model.h5')
        
        return loaded
    
    @staticmethod
    def save_weights():
        """Save only weights."""
        # Save weights
        model.save_weights('weights.h5')
        
        # Load weights
        model.load_weights('weights.h5')
    
    @staticmethod
    def save_savedmodel():
        """Save in SavedModel format."""
        model.save('saved_model')
        
        # Load back
        loaded = keras.models.load_model('saved_model')
        
        return loaded

### Best Practices

1. Use functional API for complex architectures
2. Add batch normalization after convolutions
3. Use dropout appropriately
4. Monitor validation metrics during training
5. Use callbacks for early stopping and checkpoints

## VIII. CONCLUSION

### Key Takeaways

1. **Sequential API**: Simple linear architectures
2. **Functional API**: Complex multi-input/output models
3. **Subclassing API**: Full customization
4. **Callbacks**: Training automation
5. **Model Persistence**: Save/load for deployment

### Next Steps

1. Explore pre-trained models (transfer learning)
2. Learn about custom training loops
3. Study model deployment options
4. Practice with real datasets

### Further Reading

- Keras Guide: https://keras.io/getting_started/
- TensorFlow Keras: https://www.tensorflow.org/guide/keras
- "Deep Learning with Python" by François Chollet

End of Keras API Introduction Tutorial