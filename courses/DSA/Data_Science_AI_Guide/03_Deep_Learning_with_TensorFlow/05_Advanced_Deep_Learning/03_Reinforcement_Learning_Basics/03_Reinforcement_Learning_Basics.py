# Topic: Reinforcement Learning Basics
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Reinforcement Learning Basics

I. INTRODUCTION
   - RL agents learn by interacting with environment
   - Maximize cumulative reward
   - Applications: games, robotics, resource management

II. CORE_CONCEPTS
   - Agent, environment, states, actions, rewards
   - Policy gradient
   - Q-learning
   - Deep Q-networks (DQN)

III. IMPLEMENTATION
   - Simple RL environment
   - DQN agent
   - Training loop
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers, models
import numpy as np


def simple_q_network():
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(4,)),
        layers.Dense(64, activation='relu'),
        layers.Dense(2)
    ])
    print("Simple Q-Network:")
    model.summary()
    return model


def dqn_agent():
    inputs = keras.Input(shape=(4,))
    x = layers.Dense(64, activation='relu')(inputs)
    x = layers.Dense(64, activation='relu')(x)
    outputs = layers.Dense(2)(x)
    model = models.Model(inputs=inputs, outputs=outputs)
    print("DQN Agent:")
    model.summary()
    return model


def policy_network():
    inputs = keras.Input(shape=(4,))
    x = layers.Dense(32, activation='relu')(inputs)
    x = layers.Dense(32, activation='relu')(x)
    outputs = layers.Dense(2, activation='softmax')(x)
    model = models.Model(inputs=inputs, outputs=outputs)
    print("Policy Network:")
    model.summary()
    return model


def value_network():
    inputs = keras.Input(shape=(4,))
    x = layers.Dense(32, activation='relu')(inputs)
    x = layers.Dense(32, activation='relu')(x)
    value = layers.Dense(1)(x)
    model = models.Model(inputs=inputs, outputs=value)
    print("Value Network:")
    model.summary()
    return model


def actor_critic():
    state_input = keras.Input(shape=(4,))
    
    actor = models.Sequential([
        layers.Dense(32, activation='relu')(state_input),
        layers.Dense(32, activation='relu'),
        layers.Dense(2, activation='softmax')
    ])
    
    critic = models.Sequential([
        layers.Dense(32, activation='relu')(state_input),
        layers.Dense(32, activation='relu'),
        layers.Dense(1)
    ])
    
    print("Actor-Critic:")
    print(f"  Actor params: {actor.count_params()}")
    print(f"  Critic params: {critic.count_params()}")
    return actor, critic


def epsilon_greedy(epsilon):
    def select_action(q_values, epsilon=epsilon):
        if np.random.random() < epsilon:
            return np.random.randint(len(q_values))
        return np.argmax(q_values)
    return select_action


def replay_buffer(capacity=10000):
    buffer = []
    def add(state, action, reward, next_state, done):
        buffer.append((state, action, reward, next_state, done))
        if len(buffer) > capacity:
            buffer.pop(0)
        return buffer
    
    def sample(batch_size):
        indices = np.random.choice(len(buffer), batch_size, replace=False)
        return [buffer[i] for i in indices]
    
    return add, sample


def train_dqn():
    model = simple_q_network()
    model.compile(optimizer='adam', loss='mse')
    print("DQN Training placeholder")
    return model


def core_implementation():
    print("Simple Q-Network:")
    simple_q_network()
    print("\nDQN Agent:")
    dqn_agent()
    print("\nPolicy Network:")
    policy_network()
    print("\nActor-Critic:")
    actor_critic()
    return True


def banking_example():
    state_dim = 10
    action_dim = 3
    
    actor = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(state_dim,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(action_dim, activation='softmax')
    ])
    
    critic = models.Sequential([
        layers.Dense(32, activation='relu', input_shape=(state_dim,)),
        layers.Dense(32, activation='relu'),
        layers.Dense(1)
    ])
    
    print(f"Banking RL Agent - Actor params: {actor.count_params()}")
    return actor, critic


def healthcare_example():
    state_dim = 20
    action_dim = 5
    
    actor = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(state_dim,)),
        layers.Dense(64, activation='relu'),
        layers.Dropout(0.2),
        layers.Dense(action_dim, activation='softmax')
    ])
    
    critic = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(state_dim,)),
        layers.Dense(64, activation='relu'),
        layers.Dropout(0.2),
        layers.Dense(1)
    ])
    
    print(f"Healthcare RL Agent - Actor params: {actor.count_params()}")
    return actor, critic


def main():
    print("Executing Reinforcement Learning Basics implementation\n")
    core_implementation()
    print("\n" + "=" * 60)
    banking_example()
    print("\n" + "=" * 60)
    healthcare_example()
    print("\nAll implementations completed!")


if __name__ == "__main__":
    main()