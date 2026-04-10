# Reinforcement Learning Basics

## I. INTRODUCTION

### What is Reinforcement Learning?

Reinforcement Learning (RL) is a type of machine learning where an agent learns to make decisions by interacting with an environment. The agent learns from the consequences of its actions, receiving rewards or penalties, rather than from explicit instructions.

### Why RL Matters

- **Autonomous decision-making**: Robots, games, self-driving
- **Optimize long-term goals**: Strategic planning
- **Complex environments**: Where manual programming is impossible
- **Real-world applications**: Finance, healthcare, robotics

### Prerequisites

- Neural network fundamentals
- Basic probability
- Optimization concepts

## II. FUNDAMENTALS

### Key Components

1. **Agent**: The learner
2. **Environment**: What the agent interacts with
3. **State**: Current situation
4. **Action**: What the agent can do
5. **Reward**: Feedback from environment

### RL Algorithms

- **Q-learning**: Value-based method
- **Policy gradients**: Direct policy optimization
- **Actor-critic**: Combines value and policy methods

## III. IMPLEMENTATION

```python
"""
Reinforcement Learning Basics
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
print("REINFORCEMENT LEARNING BASICS")
print("="*60)

# Step 1: Simple Environment (Grid World)
class SimpleEnvironment:
    """
    Simple grid world environment for RL.
    """
    def __init__(self, size=4):
        self.size = size
        self.state = (0, 0)
        self.goal = (size-1, size-1)
    
    def reset(self):
        self.state = (0, 0)
        return self.state
    
    def step(self, action):
        x, y = self.state
        if action == 0:  # Up
            x = max(0, x - 1)
        elif action == 1:  # Down
            x = min(self.size - 1, x + 1)
        elif action == 2:  # Left
            y = max(0, y - 1)
        elif action == 3:  # Right
            y = min(self.size - 1, y + 1)
        
        self.state = (x, y)
        
        if self.state == self.goal:
            return self.state, 1.0, True  # Reward, done
        return self.state, -0.1, False
    
    def render(self):
        grid = [['.' for _ in range(self.size)] for _ in range(self.size)]
        grid[self.state[0]][self.state[1]] = 'A'
        grid[self.goal[0]][self.goal[1]] = 'G'
        return '\n'.join([' '.join(row) for row in grid])

print("Environment defined")

# Step 2: Q-Network
def build_q_network(state_size, action_size):
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=(state_size,)),
        layers.Dense(64, activation='relu'),
        layers.Dense(action_size, activation='linear')
    ])
    return model

# Step 3: Q-Learning Agent
class QLearningAgent:
    def __init__(self, state_size, action_size, learning_rate=0.1, gamma=0.95):
        self.state_size = state_size
        self.action_size = action_size
        self.learning_rate = learning_rate
        self.gamma = gamma
        self.epsilon = 1.0  # Exploration rate
        self.q_network = build_q_network(state_size, action_size)
        self.optimizer = keras.optimizers.Adam(learning_rate=0.001)
    
    def act(self, state):
        if np.random.random() < self.epsilon:
            return np.random.randint(self.action_size)
        q_values = self.q_network(tf.expand_dims(state, 0))
        return tf.argmax(q_values, axis=1).numpy()[0]
    
    def train(self, state, action, reward, next_state, done):
        with tf.GradientTape() as tape:
            q_values = self.q_network(tf.expand_dims(state, 0))
            next_q = self.q_network(tf.expand_dims(next_state, 0))
            
            target = reward + self.gamma * tf.reduce_max(next_q) * (1 - int(done))
            current_q = q_values[0][action]
            loss = (target - current_q) ** 2
        
        gradients = tape.gradient(loss, self.q_network.trainable_variables)
        self.optimizer.apply_gradients(zip(gradients, self.q_network.trainable_variables))
        
        # Decay epsilon
        self.epsilon = max(0.01, self.epsilon * 0.99)

print("Q-Learning agent defined")

# Training
def train_rl():
    print("\n" + "="*60)
    print("Training RL Agent")
    print("="*60)
    
    env = SimpleEnvironment()
    agent = QLearningAgent(state_size=2, action_size=4)
    
    episodes = 100
    for episode in range(episodes):
        state = env.reset()
        total_reward = 0
        
        while True:
            action = agent.act(state)
            next_state, reward, done = env.step(action)
            
            agent.train(state, action, reward, next_state, done)
            total_reward += reward
            state = next_state
            
            if done:
                break
        
        if (episode + 1) % 20 == 0:
            print(f"Episode {episode+1}: Total Reward = {total_reward:.2f}")
    
    return agent

agent = train_rl()
```

## IV. APPLICATIONS

### Banking - Portfolio Optimization

```python
# Banking - Portfolio Selection
def banking_portfolio():
    np.random.seed(42)
    
    print("\n" + "="*60)
    print("Banking - Portfolio Optimization")
    print("="*60)
    
    # Simple portfolio environment
    env = SimpleEnvironment(size=5)
    
    # Train agent
    agent = QLearningAgent(state_size=2, action_size=4)
    
    for episode in range(50):
        state = env.reset()
        total_reward = 0
        
        for _ in range(20):
            action = agent.act(state)
            next_state, reward, done = env.step(action)
            agent.train(state, action, reward, next_state, done)
            total_reward += reward
            state = next_state
            if done:
                break
    
    print("Portfolio agent trained")
    return agent

banking_portfolio()
```

### Healthcare - Treatment Planning

```python
# Healthcare - Treatment Optimization
def healthcare_treatment():
    np.random.seed(42)
    
    print("\n" + "="*60)
    print("Healthcare - Treatment Planning")
    print("="*60)
    
    # Patient state: (severity, time_elapsed)
    # Actions: treatment levels 0-3
    
    agent = QLearningAgent(state_size=2, action_size=4)
    
    # Simulated patient data
    for episode in range(50):
        state = (np.random.randint(1, 5), 0)
        total_reward = 0
        
        for step in range(5):
            action = agent.act(state)
            # Simulate treatment outcome
            next_severity = max(1, state[0] - action + np.random.randint(-1, 2))
            reward = -next_severity * 0.5 - step * 0.1
            next_state = (next_severity, step + 1)
            
            agent.train(state, action, reward, next_state, step == 4)
            total_reward += reward
            state = next_state
    
    print("Treatment planning agent trained")
    return agent

healthcare_treatment()
```

## V. CONCLUSION

### Key Takeaways

1. **Agent-environment interaction**: Learn through trial and error
2. **Reward signals**: Guide learning
3. **Value functions**: Estimate future rewards

### Further Reading

1. "Reinforcement Learning: An Introduction" (Sutton & Barto, 2018)
2. "Deep Q-Learning" (Mnih et al., 2015)