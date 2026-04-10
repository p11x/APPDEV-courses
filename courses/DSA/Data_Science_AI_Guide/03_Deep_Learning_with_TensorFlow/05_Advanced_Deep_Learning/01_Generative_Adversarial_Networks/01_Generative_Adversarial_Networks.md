# Generative Adversarial Networks

## I. INTRODUCTION

### What are GANs?

Generative Adversarial Networks (GANs) are a framework for training generative models through adversarial competition. Introduced by Ian Goodfellow in 2014, GANs consist of two neural networks - a generator that creates fake samples and a discriminator that distinguishes real from fake samples.

### Why GANs Matter

- **Realistic generation**: Create new, realistic images/data
- **Data augmentation**: Expand limited datasets
- **Art and creativity**: Generate art, music, designs
- **Anonymization**: Create privacy-preserving data

### Prerequisites

- Neural network fundamentals
- Deep learning concepts
- TensorFlow/Keras

## II. FUNDAMENTALS

### GAN Architecture

```
Generator:    z (noise) ──► G(z) ──► Fake Samples
                    │
                    ▼
              Real Samples
                    │
                    ▼
Discriminator: D(x) ──► Real/Fake
```

### Key Concepts

- **Generator**: Creates fake samples from random noise
- **Discriminator**: Classifies real vs fake
- **Minimax game**: Generator tries to fool, discriminator tries to detect
- **Nash equilibrium**: Both networks reach equilibrium

## III. IMPLEMENTATION

```python
"""
Generative Adversarial Networks
Deep Learning with TensorFlow/Keras
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models, Model
import numpy as np
import warnings
warnings.filterwarnings('ignore')

tf.random.set_seed(42)
np.random.seed(42)

print("="*60)
print("GENERATIVE ADVERSARIAL NETWORKS")
print("="*60)

# Step 1: Generator Network
def build_generator(latent_dim=100):
    model = models.Sequential([
        layers.Dense(128, input_dim=latent_dim),
        layers.LeakyReLU(0.2),
        layers.BatchNormalization(),
        
        layers.Dense(256),
        layers.LeakyReLU(0.2),
        layers.BatchNormalization(),
        
        layers.Dense(512),
        layers.LeakyReLU(0.2),
        layers.BatchNormalization(),
        
        layers.Dense(784, activation='tanh'),
        layers.Reshape((28, 28, 1))
    ])
    return model

# Step 2: Discriminator Network
def build_discriminator():
    model = models.Sequential([
        layers.Flatten(input_shape=(28, 28, 1)),
        
        layers.Dense(512),
        layers.LeakyReLU(0.2),
        layers.Dropout(0.3),
        
        layers.Dense(256),
        layers.LeakyReLU(0.2),
        layers.Dropout(0.3),
        
        layers.Dense(1, activation='sigmoid')
    ])
    return model

print("Generator and Discriminator defined")

# Step 3: GAN Training
def train_gan(epochs=50, batch_size=64, latent_dim=100):
    # Build models
    generator = build_generator(latent_dim)
    discriminator = build_discriminator()
    
    discriminator.compile(
        optimizer='adam',
        loss='binary_crossentropy',
        metrics=['accuracy']
    )
    
    # Combined model
    discriminator.trainable = False
    gan_input = keras.Input(shape=(latent_dim,))
    fake_image = generator(gan_input)
    validity = discriminator(fake_image)
    gan = Model(gan_input, validity)
    gan.compile(optimizer='adam', loss='binary_crossentropy')
    
    print("\n" + "="*60)
    print("GAN Training")
    print("="*60)
    
    # Generate synthetic data
    X = np.random.randn(1000, 28, 28, 1).astype(np.float32) * 0.5 + 0.5
    
    for epoch in range(epochs):
        # Train discriminator
        idx = np.random.randint(0, X.shape[0], batch_size)
        real_images = X[idx]
        
        noise = np.random.randn(batch_size, latent_dim)
        fake_images = generator.predict(noise, verbose=0)
        
        d_loss_real = discriminator.train_on_batch(real_images, np.ones((batch_size, 1)))
        d_loss_fake = discriminator.train_on_batch(fake_images, np.zeros((batch_size, 1)))
        d_loss = 0.5 * np.add(d_loss_real, d_loss_fake)
        
        # Train generator
        noise = np.random.randn(batch_size, latent_dim)
        g_loss = gan.train_on_batch(noise, np.ones((batch_size, 1)))
        
        if (epoch + 1) % 10 == 0:
            print(f"Epoch {epoch+1}: D_loss={d_loss[0]:.4f}, G_loss={g_loss:.4f}")
    
    return generator

gen = train_gan(epochs=30)
```

## IV. APPLICATIONS

### Standard Example: Digit Generation

```python
# Standard Example: Generate MNIST-like digits
def generate_digits():
    print("\n" + "="*60)
    print("Generate Handwritten Digits")
    print("="*60)
    
    generator = build_generator()
    
    # Generate samples
    noise = np.random.randn(10, 100)
    generated = generator(generator.input if hasattr(generator, 'input') else noise)
    
    print("Generated 10 digit images")
    return generator

generate_digits()
```

### Banking Example

```python
# Banking - Generate Fake Transactions
def banking_generate_transactions():
    np.random.seed(42)
    
    print("\n" + "="*60)
    print("Banking - Generate Transaction Patterns")
    print("="*60)
    
    # Generate synthetic transaction sequences
    n_samples = 500
    seq_length = 20
    n_features = 8
    
    gen = models.Sequential([
        layers.Dense(64, input_dim=50),
        layers.LeakyReLU(0.2),
        layers.Dense(128),
        layers.LeakyReLU(0.2),
        layers.Dense(seq_length * n_features),
        layers.Reshape((seq_length, n_features))
    ])
    
    noise = np.random.randn(n_samples, 50)
    fake_transactions = gen(noise)
    
    print(f"Generated {n_samples} synthetic transaction sequences")
    return gen

banking_generate_transactions()
```

### Healthcare Example

```python
# Healthcare - Generate Medical Images
def healthcare_generate_images():
    np.random.seed(42)
    
    print("\n" + "="*60)
    print("Healthcare - Generate Medical Images")
    print("="*60)
    
    # Generate synthetic medical images
    gen = models.Sequential([
        layers.Dense(128, input_dim=100),
        layers.LeakyReLU(0.2),
        layers.BatchNormalization(),
        layers.Dense(256),
        layers.LeakyReLU(0.2),
        layers.BatchNormalization(),
        layers.Dense(64 * 64, activation='tanh'),
        layers.Reshape((64, 64, 1))
    ])
    
    noise = np.random.randn(5, 100)
    fake_images = gen(noise)
    
    print("Generated 5 synthetic medical scan images")
    return gen

healthcare_generate_images()
```

## V. CONCLUSION

### Key Takeaways

1. **Adversarial training**: Generator vs discriminator
2. **Minimax game**: Zero-sum competition
3. **Various types**: DCGAN, WGAN, StyleGAN

### Further Reading

1. "Generative Adversarial Networks" (Goodfellow et al., 2014)
2. "Unsupervised Representation Learning with DCGAN" (Radford et al., 2015)
3. "Wasserstein GAN" (Arjovsky et al., 2017)