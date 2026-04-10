# Topic: Autoencoders and Variational Inference
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Autoencoders and Variational Inference

I. INTRODUCTION
   - Autoencoders learn compressed representations
   - Variational autoencoders (VAE) add probabilistic modeling
   - Used for dimensionality reduction, anomaly detection, generation

II. CORE_CONCEPTS
   - Encoder/decoder architecture
   - Latent space
   - VAE loss (reconstruction + KL divergence)
   - Sampling in latent space

III. IMPLEMENTATION
   - Simple autoencoder
   - Variational autoencoder
   - Convolutional autoencoder
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def simple_autoencoder():
    encoder = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(784,)),
        layers.Dense(32, activation='relu')
    ])
    
    decoder = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(32,)),
        layers.Dense(784, activation='sigmoid')
    ])
    
    autoencoder = models.Sequential([encoder, decoder])
    print("Simple Autoencoder:")
    autoencoder.summary()
    return autoencoder


def convolutional_autoencoder():
    encoder = models.Sequential([
        layers.Conv2D(32, (3, 3), activation='relu', input_shape=(28, 28, 1)),
        layers.MaxPooling2D((2, 2)),
        layers.Conv2D(64, (3, 3), activation='relu'),
        layers.MaxPooling2D((2, 2))
    ])
    
    decoder = models.Sequential([
        layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
        layers.UpSampling2D((2, 2)),
        layers.Conv2D(32, (3, 3), activation='relu', padding='same'),
        layers.UpSampling2D((2, 2)),
        layers.Conv2D(1, (3, 3), activation='sigmoid', padding='same')
    ])
    
    autoencoder = models.Sequential([encoder, decoder])
    print("Convolutional Autoencoder:")
    autoencoder.summary()
    return autoencoder


def variational_encoder():
    class VAE(keras.Model):
        def __init__(self):
            super().__init__()
            self.encoder = models.Sequential([
                layers.Dense(64, activation='relu', input_shape=(784,)),
                layers.Dense(32, activation='relu')
            ])
            self.z_mean = layers.Dense(2)
            self.z_log_var = layers.Dense(2)
            self.decoder = models.Sequential([
                layers.Dense(64, activation='relu', input_shape=(2,)),
                layers.Dense(784, activation='sigmoid')
            ])
        
        def encode(self, x):
            h = self.encoder(x)
            return self.z_mean(h), self.z_log_var(h)
        
        def reparameterize(self, mean, logvar):
            eps = tf.random.normal(tf.shape(mean))
            return mean + eps * tf.exp(0.5 * logvar)
        
        def decode(self, z):
            return self.decoder(z)
        
        def call(self, x):
            mean, logvar = self.encode(x)
            z = self.reparameterize(mean, logvar)
            return self.decode(z)
    
    print("Variational Autoencoder:")
    return VAE()


def denoising_autoencoder():
    encoder = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(784,)),
        layers.Dense(32, activation='relu')
    ])
    
    decoder = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(32,)),
        layers.Dense(784, activation='sigmoid')
    ])
    
    autoencoder = models.Sequential([encoder, decoder])
    print("Denoising Autoencoder:")
    return autoencoder


def sparse_autoencoder():
    encoder = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(784,)),
        layers.Dense(32, activation='relu'),
        layers.ActivityRegularization(l1=1e-5)
    ])
    
    decoder = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(32,)),
        layers.Dense(784, activation='sigmoid')
    ])
    
    autoencoder = models.Sequential([encoder, decoder])
    print("Sparse Autoencoder:")
    return autoencoder


def train_autoencoder():
    X = tf.random.normal([100, 784])
    
    autoencoder = simple_autoencoder()
    autoencoder.compile(optimizer='adam', loss='mse')
    history = autoencoder.fit(X, X, epochs=5, verbose=0)
    print(f"Autoencoder training - Loss: {history.history['loss'][-1]:.4f}")
    return autoencoder


def core_implementation():
    print("Simple Autoencoder:")
    simple_autoencoder()
    print("\nConvolutional Autoencoder:")
    convolutional_autoencoder()
    print("\nVariational Autoencoder:")
    variational_encoder()
    print("\nSparse Autoencoder:")
    sparse_autoencoder()
    return True


def banking_example():
    encoder = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(20,)),
        layers.Dense(16, activation='relu'),
        layers.Dense(2)
    ])
    
    decoder = models.Sequential([
        layers.Dense(16, activation='relu', input_shape=(2,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(20, activation='sigmoid')
    ])
    
    autoencoder = models.Sequential([encoder, decoder])
    print(f"Banking Autoencoder - Total params: {autoencoder.count_params()}")
    return autoencoder


def healthcare_example():
    encoder = models.Sequential([
        layers.Dense(128, activation='relu', input_shape=(50,)),
        layers.Dense(64, activation='relu'),
        layers.Dense(32)
    ])
    
    decoder = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(32,)),
        layers.Dense(128, activation='relu'),
        layers.Dense(50, activation='sigmoid')
    ])
    
    autoencoder = models.Sequential([encoder, decoder])
    autoencoder.compile(optimizer='adam', loss='mse')
    print(f"Healthcare Autoencoder - Total params: {autoencoder.count_params()}")
    return autoencoder


def main():
    print("Executing Autoencoders and Variational Inference implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    print("\nTraining Autoencoder:")
    train_autoencoder()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()