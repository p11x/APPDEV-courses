# Autoencoders and Variational Inference

## I. INTRODUCTION

### What are Autoencoders?

Autoencoders are neural networks that learn to compress and reconstruct data. They consist of an encoder that maps input to a latent representation and a decoder that reconstructs the original input from the latent space.

### Why Autoencoders Matter

- **Dimensionality reduction**: Compress data efficiently
- **Anomaly detection**: Identify unusual patterns
- **Data generation**: Create new samples from latent space
- **Representation learning**: Learn useful features

### Prerequisites

- Neural network fundamentals
- TensorFlow/Keras
- Basic probability concepts

## II. FUNDAMENTALS

### Types of Autoencoders

1. **Vanilla autoencoder**: Basic encoder-decoder
2. **Denoising autoencoder**: Learn from noisy inputs
3. **Variational autoencoder (VAE)**: Probabilistic model

### Variational Inference

- Learn distribution over latent space
- Generate new samples by sampling from distribution
- Enables probabilistic generation

## III. IMPLEMENTATION

```python
"""
Autoencoders and Variational Inference
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
print("AUTOENCODERS AND VARIATIONAL INFERENCE")
print("="*60)

# Step 1: Basic Autoencoder
def build_autoencoder():
    input_img = keras.Input(shape=(28, 28, 1))
    
    # Encoder
    x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(input_img)
    x = layers.MaxPooling2D((2, 2))(x)
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    x = layers.MaxPooling2D((2, 2))(x)
    encoded = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
    
    # Decoder
    x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(encoded)
    x = layers.UpSampling2D((2, 2))(x)
    x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(x)
    x = layers.UpSampling2D((2, 2))(x)
    decoded = layers.Conv2D(1, (3, 3), activation='sigmoid', padding='same')(x)
    
    return keras.Model(input_img, decoded)

# Step 2: Variational Autoencoder
class VAE(keras.Model):
    def __init__(self, latent_dim=2):
        super().__init__()
        self.latent_dim = latent_dim
        self.encoder = self.build_encoder()
        self.decoder = self.build_decoder()
        self.total_loss_tracker = keras.metrics.Mean(name="total_loss")
        self.reconstruction_loss_tracker = keras.metrics.Mean(name="reconstruction_loss")
        self.kl_loss_tracker = keras.metrics.Mean(name="kl_loss")
    
    def build_encoder(self):
        encoder_input = keras.Input(shape=(28, 28, 1))
        x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(encoder_input)
        x = layers.MaxPooling2D((2, 2))(x)
        x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
        x = layers.Flatten()(x)
        
        z_mean = layers.Dense(self.latent_dim, name="z_mean")(x)
        z_log_var = layers.Dense(self.latent_dim, name="z_log_var")(x)
        
        return keras.Model(encoder_input, [z_mean, z_log_var])
    
    def build_decoder(self):
        decoder_input = keras.Input(shape=(self.latent_dim,))
        x = layers.Dense(7 * 7 * 64, activation='relu')(decoder_input)
        x = layers.Reshape((7, 7, 64))(x)
        x = layers.Conv2D(64, (3, 3), activation='relu', padding='same')(x)
        x = layers.UpSampling2D((2, 2))(x)
        x = layers.Conv2D(32, (3, 3), activation='relu', padding='same')(x)
        x = layers.UpSampling2D((2, 2))(x)
        decoded = layers.Conv2D(1, (3, 3), activation='sigmoid', padding='same')(x)
        return keras.Model(decoder_input, decoded)
    
    def call(self, inputs):
        z_mean, z_log_var = self.encoder(inputs)
        z = z_mean + tf.exp(0.5 * z_log_var) * tf.random.normal(tf.shape(z_mean))
        return self.decoder(z)

print("Autoencoder and VAE defined")

# Training
def train_vae():
    print("\n" + "="*60)
    print("Training VAE")
    print("="*60)
    
    # Generate data
    X = np.random.randn(500, 28, 28, 1).astype(np.float32)
    
    vae = VAE(latent_dim=2)
    vae.compile(optimizer='adam')
    
    # Simplified training
    print("VAE model created")
    return vae

train_vae()
```

## IV. APPLICATIONS

### Banking - Anomaly Detection

```python
# Banking - Detect Anomalous Transactions
def banking_anomaly_detection():
    np.random.seed(42)
    
    print("\n" + "="*60)
    print("Banking - Transaction Anomaly Detection")
    print("="*60)
    
    # Autoencoder for anomaly detection
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(20,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(64, activation='relu'),
        layers.Dense(20)
    ])
    
    # Train on normal transactions
    X_normal = np.random.randn(300, 20).astype(np.float32)
    model.compile(optimizer='adam', loss='mse')
    model.fit(X_normal, X_normal, epochs=10, verbose=0)
    
    # Detect anomalies by reconstruction error
    X_test = np.random.randn(50, 20).astype(np.float32)
    X_test[0] += 5  # Add anomaly
    
    reconstructions = model.predict(X_test, verbose=0)
    errors = np.mean(np.abs(X_test - reconstructions), axis=1)
    
    print(f"Reconstruction errors computed")
    return model

banking_anomaly_detection()
```

### Healthcare - Image Denoising

```python
# Healthcare - Denoise Medical Images
def healthcare_image_denoising():
    np.random.seed(42)
    
    print("\n" + "="*60)
    print("Healthcare - Medical Image Denoising")
    print("="*60)
    
    # Denoising autoencoder
    encoder = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', padding='same', input_shape=(64, 64, 1)),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
    ])
    
    decoder = models.Sequential([
        layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
        layers.UpSampling2D((2, 2)),
        layers.Conv2D(1, (3, 3), activation='sigmoid', padding='same'),
    ])
    
    # Clean images
    clean_images = np.random.randn(200, 64, 64, 1).astype(np.float32) * 0.5 + 0.5
    # Noisy images
    noisy_images = clean_images + np.random.randn(200, 64, 64, 1).astype(np.float32) * 0.2
    
    print("Denoising autoencoder created")
    return encoder, decoder

healthcare_image_denoising()
```

## V. CONCLUSION

### Key Takeaways

1. **Autoencoders**: Compress and reconstruct data
2. **VAE**: Probabilistic latent space for generation
3. **Denoising**: Learn robust representations

### Further Reading

1. "Auto-Encoding Variational Bayes" (Kingma & Welling, 2013)
2. "Learning Deep Generative Models" (Salakhutdinov, 2015)