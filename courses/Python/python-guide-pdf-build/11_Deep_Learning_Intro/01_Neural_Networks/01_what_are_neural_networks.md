# 🧠 What Are Neural Networks?

## 🎯 What You'll Learn

- The neuron analogy (input × weight + bias)
- Layers: input, hidden, output
- Activation functions: ReLU, Sigmoid, Softmax
- Forward pass and backpropagation basics

## 📦 Prerequisites

- Complete the Machine Learning section (folder 10)

## Neural Networks Demystified

**Neural networks** are inspired by the brain — they have "neurons" that receive signals, process them, and pass them on!

## The Neuron: A Simple Analogy

Think of a neuron like a **decision-making machine**:

```
        Inputs (x)
           │
           ▼
    ┌──────────────┐
    │   Multiply   │  x1 × w1 + x2 × w2 + ... + bias
    │  by weights  │
    └──────────────┘
           │
           ▼
    ┌──────────────┐
    │  Activation │  ReLU, Sigmoid, etc.
    │  Function   │
    └──────────────┘
           │
           ▼
       Output (y)
```

### Simple Math

```
output = activation(sum(inputs × weights) + bias)
```

## Network Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    NEURAL NETWORK                       │
├─────────────────────────────────────────────────────────┤
│                                                          │
│   Input Layer          Hidden Layers        Output Layer│
│   (Features)           (Computation)        (Prediction)│
│                                                          │
│      ○                      ○                   ○       │
│      │                     ╱ ╲                  │       │
│      ○───────────────○───╱   ╲──────○───────────○       │
│      │             ╱ ╲ ╱     ╲╲ ╲          │       │
│      ○───────────○╱   ╳       ╳╲ ╳────○────○       │
│      │          ╱         ╱ ╲╱          │       │
│      ○─────────○───────○─╱   ╲─────○─────○       │
│      │         │      ╱ ╲     ╱ ╲        │       │
│      ○─────────○─────○──╲   ╱─────○─────○       │
│                            ╲ ╱                  │       │
│                             ○───────────────────○       │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

- **Input Layer**: Your features (pixels, words, numbers)
- **Hidden Layers**: Where the "learning" happens
- **Output Layer**: Predictions (class probabilities, values)

## Activation Functions

### ReLU (Rectified Linear Unit) — Most Common!

```
output = max(0, input)

    │
  5 ┤    ╱
    │   ╱
  0 ┤──╱───────
    │
    └──────────
        0    5
```

Used in hidden layers — simple and effective!

### Sigmoid (For Binary Classification)

```
output = 1 / (1 + e^(-x))

    │
1.0 ┤╲
    │ ╲
0.5 ┤  ╲
    │   ╲
0.0 ┤────╲────────
    │
    └───────────
        0
```

Outputs between 0 and 1 — good for probability!

### Softmax (For Multi-Class)

Outputs probabilities that sum to 1:

```
scores = [2.0, 1.0, 0.1]
softmax = [0.7, 0.2, 0.1]  # First class is most likely!
```

## Forward Pass

Data flows **left to right** through the network:

```
Input: [x1, x2, x3]
         │
         ▼
    Layer 1: [h1, h2]
         │
         ▼
    Layer 2: [h3, h4, h5]
         │
         ▼
   Output: [y1, y2]
```

## Backpropagation

**The magic that makes learning possible!**

1. Make a prediction (forward pass)
2. Calculate error (how wrong were we?)
3. Propagate error **backward** through the network
4. Adjust weights to reduce error

```
Forward:  Input → Output → Error
Backward: Error → Gradients → Update Weights
```

Think of it like:
- Playing darts: throw → see miss → adjust aim → throw again
- Eventually, you get better!

## Loss Functions

**Loss = How wrong was the prediction?**

| Problem Type | Loss Function | What it Measures |
|--------------|---------------|------------------|
| Regression | MSE | Mean Squared Error |
| Binary Classification | Binary Cross-Entropy | Log loss |
| Multi-class | Categorical Cross-Entropy | Log loss |

## Optimizers

**How do we minimize the loss?**

- **Gradient Descent**: Take small steps downhill
- **Adam**: Adaptive learning — most popular!
- **SGD**: Stochastic Gradient Descent

### The Hiking Analogy

```
Loss
  │
  │          You're here (current loss)
  │              ↓
  │             ╱╲
  │            ╱  ╲     Gradient = slope direction
  │           ╱    ╲    Step size = learning rate
  │          ╱      ╲
  │         ╱        ╲
  │        ╱          ╲
  │       ╱            ╲
  │      ╱              ╲
  │     ╱                ╲
  │    ╱                  ╲
  │   ╱                    ╲
  │  ╱                      ╲
  │╱                         ╲
  └──────────────────────────
              Weights →
```

- **Learning rate too high**: Overshoot the minimum!
- **Learning rate too low**: Takes forever!

## What Makes It "Deep"?

- **Shallow**: 1-2 hidden layers
- **Deep**: Many hidden layers (hence "Deep Learning")

More layers = more complex patterns can be learned!

## ✅ Summary

- **Neuron**: input × weight + bias → activation
- **Layers**: Input → Hidden → Output
- **Activation**: ReLU (hidden), Sigmoid/Softmax (output)
- **Forward pass**: Data flows left to right
- **Backpropagation**: Error flows right to left, updates weights
- **Loss**: How wrong the prediction is
- **Optimizer**: Adam is the go-to choice
- **Deep Learning**: Many hidden layers

## ➡️ Next Steps

Ready to see activations in code? Head to **[02_activation_and_loss.md](./02_activation_and_loss.md)**!

## 🔗 Further Reading

- [Neural Networks Explained](https://www.ibm.com/topics/neural-networks)
- [Deep Learning Book (Free)](https://www.deeplearningbook.org/)
- [3Blue1Brown Neural Networks](https://www.youtube.com/playlist?list=PLZHQObOWTQDNU6R1_67000Dx_ZCJB-3p3)
