# Topic: Generative Adversarial Networks
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Generative Adversarial Networks

I. INTRODUCTION
   - GANs generate synthetic data that mimics real data
   - Generator creates, discriminator evaluates
   - Used for image generation, data augmentation, style transfer

II. CORE_CONCEPTS
   - Generator network
   - Discriminator network
   - Adversarial loss
   - Training dynamics

III. IMPLEMENTATION
   - Simple GAN
   - Deep GAN
   - Training loop
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def generator_model():
    model = models.Sequential([
        layers.Dense(128, activation='relu', input_shape=(100,)),
        layers.Dense(256, activation='relu'),
        layers.Dense(512, activation='relu'),
        layers.Dense(784, activation='tanh')
    ])
    print("Generator:")
    model.summary()
    return model


def discriminator_model():
    model = models.Sequential([
        layers.Dense(512, activation='relu', input_shape=(784,)),
        layers.Dense(256, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    print("Discriminator:")
    model.summary()
    return model


def simple_gan():
    latent_dim = 100
    generator = models.Sequential([
        layers.Dense(256, activation='relu', input_shape=(latent_dim,)),
        layers.Dense(512, activation='relu'),
        layers.Dense(784, activation='tanh')
    ])
    
    discriminator = models.Sequential([
        layers.Dense(512, activation='relu', input_shape=(784,)),
        layers.Dense(256, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    print("Simple GAN created")
    return generator, discriminator


def dcgan_generator():
    model = models.Sequential([
        layers.Dense(7 * 7 * 128, activation='relu', input_shape=(100,)),
        layers.Reshape((7, 7, 128)),
        
        layers.Conv2DTranspose(128, (4, 4), strides=(2, 2), activation='relu'),
        layers.Conv2DTranspose(64, (4, 4), strides=(2, 2), activation='relu'),
        layers.Conv2D(1, (7, 7), activation='tanh', padding='same')
    ])
    print("DCGAN Generator:")
    model.summary()
    return model


def dcgan_discriminator():
    model = models.Sequential([
        layers.Conv2D(64, (5, 5), strides=(2, 2), padding='same', input_shape=(28, 28, 1)),
        layers.LeakyReLU(0.2),
        layers.Dropout(0.3),
        
        layers.Conv2D(128, (5, 5), strides=(2, 2), padding='same'),
        layers.LeakyReLU(0.2),
        layers.Dropout(0.3),
        
        layers.Flatten(),
        layers.Dense(1, activation='sigmoid')
    ])
    print("DCGAN Discriminator:")
    model.summary()
    return model


def train_gan_steps():
    latent_dim = 100
    generator = generator_model()
    discriminator = discriminator_model()
    
    cross_entropy = keras.losses.BinaryCrossentropy()
    
    @tf.function
    def train_step(real_images):
        batch_size = tf.shape(real_images)[0]
        noise = tf.random.normal([batch_size, latent_dim])
        
        with tf.GradientTape() as gen_tape, tf.GradientTape() as disc_tape:
            generated = generator(noise, training=True)
            real_output = discriminator(real_images, training=True)
            generated_output = discriminator(generated, training=True)
            
            gen_loss = cross_entropy(tf.ones_like(real_output), generated_output)
            disc_loss = cross_entropy(tf.ones_like(real_output), real_output) + \
                      cross_entropy(tf.zeros_like(generated_output), generated_output)
        
        gen_gradients = gen_tape.gradient(gen_loss, generator.trainable_variables)
        disc_gradients = disc_tape.gradient(disc_loss, discriminator.trainable_variables)
        
        return gen_loss, disc_loss
    
    print("GAN training step defined")
    return train_step


def core_implementation():
    print("Generator:")
    generator_model()
    print("\nDiscriminator:")
    discriminator_model()
    print("\nDCGAN Generator:")
    dcgan_generator()
    print("\nDCGAN Discriminator:")
    dcgan_discriminator()
    return True


def banking_example():
    latent_dim = 50
    
    generator = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(latent_dim,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(1, activation='tanh')
    ])
    
    discriminator = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(1,)),
        layers.Dense(16, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])
    
    print("Banking GAN: Generator params:", generator.count_params())
    return generator, discriminator


def healthcare_example():
    latent_dim = 100
    
    generator = models.Sequential([
        layers.Dense(128, activation='relu', input_shape=(latent_dim,)),
        layers.BatchNormalization(),
        layers.Dense(256, activation='relu'),
        layers.BatchNormalization(),
        layers.Dense(10, activation='softmax')
    ])
    
    discriminator = models.Sequential([
        layers.Dense(256, activation='relu', input_shape=(10,)),
        layers.Dropout(0.3),
        layers.Dense(128, activation='relu'),
        layers.Dropout(0.3),
        layers.Dense(1, activation='sigmoid')
    ])
    
    print("Healthcare GAN: Generator params:", generator.count_params())
    return generator, discriminator


def main():
    print("Executing Generative Adversarial Networks implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()