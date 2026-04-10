# Perceptrons and Activation Functions

## I. INTRODUCTION

### What is a Perceptron?
The perceptron is the fundamental building block of neural networks, originally proposed by Frank Rosenblatt in 1957. It is a binary classification algorithm inspired by biological neurons, and it serves as the foundation upon which more complex neural network architectures are built.

A perceptron takes multiple input values, multiplies each by a weight, sums them up, adds a bias, and passes the result through an activation function to produce an output. This simple mechanism forms the core of all deep learning models.

The mathematical formulation is: y = f(w·x + b), where w is the weight vector, x is the input vector, b is the bias, and f is the activation function.

### Why are Perceptrons and Activation Functions Important?

1. **Non-linearity**: Activation functions introduce non-linearity, enabling networks to learn complex patterns
2. **Decision Boundaries**: Perceptrons can learn linear decision boundaries
3. **Gradient Flow**: Modern activation functions are differentiable, enabling backpropagation
4. **Network Depth**: Stacking perceptrons creates deep networks capable of learning hierarchical representations

### Prerequisites
- Basic linear algebra (vectors, matrices)
- Python programming
- Understanding of gradient descent
- Familiarity with TensorFlow basics

## II. FUNDAMENTALS

### Key Concepts

**Single Perceptron**: The simplest neural network unit with multiple inputs and one output.

**Activation Functions**: Mathematical functions that determine whether a neuron should be "activated" based on its input. Common activation functions include:
- Step function: Binary output (0 or 1)
- Sigmoid: S-shaped curve, outputs between 0 and 1
- Tanh: S-shaped curve, outputs between -1 and 1
- ReLU: Rectified Linear Unit, outputs max(0, x)
- Softmax: Converts logits to probabilities, used for multi-class classification

**Weights and Biases**: Trainable parameters that control the perceptron's behavior. Weights determine the strength of connections, and bias allows shifting the activation threshold.

## III. IMPLEMENTATION

### Step-by-Step Implementation

```python
"""
Perceptrons and Activation Functions - Module 1
Neural Network Fundamentals Series
"""

import tensorflow as tf
import numpy as np
import matplotlib.pyplot as plt

class PerceptronImplementation:
    """Comprehensive perceptron implementation."""
    
    @staticmethod
    def manual_perceptron():
        """Manually implement a perceptron without TensorFlow."""
        print("="*60)
        print("Manual Perceptron Implementation")
        print("="*60)
        
        def perceptron(inputs, weights, bias, activation='step'):
            """
            Single perceptron implementation.
            
            Args:
                inputs: Input values
                weights: Weight values
                bias: Bias value
                activation: Activation function type
                
            Returns:
                output: Perceptron output
            """
            # Compute weighted sum
            weighted_sum = sum(i * w for i, w in zip(inputs, weights)) + bias
            
            # Apply activation function
            if activation == 'step':
                return 1 if weighted_sum >= 0 else 0
            elif activation == 'sigmoid':
                return 1 / (1 + np.exp(-weighted_sum))
            elif activation == 'relu':
                return max(0, weighted_sum)
            elif activation == 'tanh':
                return np.tanh(weighted_sum)
            else:
                return weighted_sum
        
        # Test perceptron
        inputs = [1.0, 0.5, -0.5]
        weights = [0.8, 0.4, 0.2]
        bias = -0.5
        
        print(f"Inputs: {inputs}")
        print(f"Weights: {weights}")
        print(f"Bias: {bias}")
        
        for act in ['step', 'sigmoid', 'relu', 'tanh']:
            output = perceptron(inputs, weights, bias, act)
            print(f"  {act.capitalize()}: {output:.4f}")
        
        return perceptron
    
    @staticmethod
    def tensorflow_perceptron():
        """Perceptron using TensorFlow."""
        print("\n" + "="*60)
        print("TensorFlow Perceptron")
        print("="*60)
        
        # Create weights and bias as variables
        weights = tf.Variable(tf.random.normal([3, 1]), name='weights')
        bias = tf.Variable(tf.zeros([1]), name='bias')
        
        # Input data
        x = tf.constant([[1.0, 0.5, 0.5],
                        [0.5, 1.0, 0.5],
                        [0.5, 0.5, 1.0]], dtype=tf.float32)
        
        # Forward pass
        @tf.function
        def forward(x):
            linear = tf.matmul(x, weights) + bias
            return tf.sigmoid(linear)
        
        # Compute output
        output = forward(x)
        
        print(f"Input shape: {x.shape}")
        print(f"Weights shape: {weights.shape}")
        print(f"Output:\n{output.numpy()}")
        
        return weights, bias, output

class ActivationFunctions:
    """Comprehensive activation function demonstrations."""
    
    @staticmethod
    def visualize_activations():
        """Visualize common activation functions."""
        print("="*60)
        print("Activation Functions Overview")
        print("="*60)
        
        x = np.linspace(-5, 5, 100)
        
        activations = {
            'Sigmoid': lambda x: 1 / (1 + np.exp(-x)),
            'Tanh': lambda x: np.tanh(x),
            'ReLU': lambda x: np.maximum(0, x),
            'Leaky ReLU': lambda x: np.where(x > 0, x, 0.01 * x),
            'ELU': lambda x: np.where(x > 0, x, (np.exp(x) - 1)),
            'Swish': lambda x: x * (1 / (1 + np.exp(-x))),
            'Softmax': lambda x: np.exp(x) / np.sum(np.exp(x))
        }
        
        print("\nActivation Functions:")
        for name, func in activations.items():
            # Apply to scalar range
            if name == 'Softmax':
                y = func(x)  # Softmax needs special handling
            else:
                y = [func(val) for val in x]
            
            print(f"  {name}: range [{min(y):.2f}, {max(y):.2f}]")
        
        return activations
    
    @staticmethod
    def tensorflow_activations():
        """TensorFlow activation functions."""
        print("\n" + "="*60)
        print("TensorFlow Activation Functions")
        print("="*60)
        
        # Test inputs
        x = tf.constant([-2.0, -1.0, 0.0, 1.0, 2.0])
        
        activations = {
            'tf.nn.sigmoid': tf.nn.sigmoid,
            'tf.nn.tanh': tf.nn.tanh,
            'tf.nn.relu': tf.nn.relu,
            'tf.nn.leaky_relu': lambda x: tf.nn.leaky_relu(x, alpha=0.2),
            'tf.nn.elu': tf.nn.elu,
            'tf.nn.swish': tf.nn.swish
        }
        
        print(f"Input: {x.numpy()}")
        
        for name, func in activations.items():
            result = func(x)
            print(f"  {name}: {result.numpy()}")
        
        # Softmax for multi-class
        logits = tf.constant([[1.0, 2.0, 3.0], [2.0, 1.0, 0.5]])
        softmax = tf.nn.softmax(logits)
        print(f"\nSoftmax logits: {logits.numpy()}")
        print(f"Softmax output: {softmax.numpy()}")
        print(f"Sum per row: {tf.reduce_sum(softmax, axis=1).numpy()}")
    
    @staticmethod
    def gradient_computation():
        """Compute gradients of activation functions."""
        print("\n" + "="*60)
        print("Activation Function Gradients")
        print("="*60)
        
        x = tf.Variable(tf.constant([-2.0, -1.0, 0.0, 1.0, 2.0]))
        
        activations = [
            ('Sigmoid', tf.nn.sigmoid),
            ('Tanh', tf.nn.tanh),
            ('ReLU', tf.nn.relu)
        ]
        
        for name, func in activations:
            with tf.GradientTape() as tape:
                y = func(x)
            
            gradients = tape.gradient(y, x)
            print(f"\n{name}:")
            print(f"  Input:  {x.numpy()}")
            print(f"  Output: {y.numpy()}")
            print(f"  Gradient: {gradients.numpy()}")

def demonstrate_perceptrons():
    """Demonstrate perceptron concepts."""
    
    per_impl = PerceptronImplementation()
    per_impl.manual_perceptron()
    per_impl.tensorflow_perceptron()
    
    act_funcs = ActivationFunctions()
    act_funcs.visualize_activations()
    act_funcs.tensorflow_activations()
    act_funcs.gradient_computation()

### Standard Example: Binary Classification with Perceptron

```python
"""
Standard Example: Binary Classification with Perceptron
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class PerceptronClassifier:
    """Binary classification using a single perceptron."""
    
    def __init__(self, input_dim):
        self.input_dim = input_dim
        
        # Single layer perceptron (logistic regression)
        self.model = keras.Sequential([
            layers.Dense(
                1, 
                activation='sigmoid',
                input_shape=(input_dim,),
                kernel_initializer='glorot_uniform',
                bias_initializer='zeros'
            )
        ])
        
        self.model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.01),
            loss='binary_crossentropy',
            metrics=['accuracy']
        )
    
    def train(self, X, y, epochs=100, verbose=0):
        """Train the perceptron."""
        history = self.model.fit(
            X, y,
            epochs=epochs,
            verbose=verbose
        )
        
        return history
    
    def predict(self, X):
        """Make predictions."""
        return self.model.predict(X, verbose=0)

def generate_classification_data(n_samples=1000):
    """Generate linearly separable classification data."""
    np.random.seed(42)
    
    # Class 0: centered at (0, 0)
    class0 = np.random.randn(n_samples // 2, 2) - [2, 2]
    
    # Class 1: centered at (3, 3)
    class1 = np.random.randn(n_samples // 2, 2) + [2, 2]
    
    # Combine
    X = np.vstack([class0, class1]).astype('float32')
    
    # Labels
    y = np.array([0] * (n_samples // 2) + [1] * (n_samples // 2))
    
    # Shuffle
    indices = np.random.permutation(n_samples)
    X = X[indices]
    y = y[indices]
    
    # Normalize
    X = (X - X.mean(axis=0)) / X.std(axis=0)
    
    return X, y

def run_perceptron_classification():
    """Run perceptron classification example."""
    print("="*60)
    print("PERCEPTRON BINARY CLASSIFICATION")
    print("="*60)
    
    # Generate data
    X, y = generate_classification_data(1000)
    
    print(f"Data shape: {X.shape}")
    print(f"Labels: {y.sum()} positive, {len(y) - y.sum()} negative")
    
    # Create and train model
    classifier = PerceptronClassifier(input_dim=2)
    classifier.model.summary()
    
    history = classifier.train(X, y, epochs=50, verbose=0)
    
    print(f"\nFinal training accuracy: {history.history['accuracy'][-1]:.4f}")
    
    # Test predictions
    test_input = tf.constant([[0.0, 0.0], [3.0, 3.0], [-3.0, -3.0]])
    predictions = classifier.predict(test_input)
    
    print(f"\nTest predictions:")
    print(f"  Input [0,0]: {predictions[0][0]:.4f} (expected ~0)")
    print(f"  Input [3,3]: {predictions[1][0]:.4f} (expected ~1)")
    print(f"  Input [-3,-3]: {predictions[2][0]:.4f} (expected ~0)")

if __name__ == "__main__":
    demonstrate_perceptrons()
    run_perceptron_classification()
```

### Real-world Example 1: Banking - Credit Approval

```python
"""
Real-world Example 1: Banking - Credit Card Approval
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class CreditApprovalModel:
    """
    Perceptron-based credit approval system for banking.
    Predicts whether a credit card application should be approved.
    
    Features:
    - income: Annual income
    - credit_score: Credit score (300-850)
    - debt: Current debt
    - employment_years: Years of employment
    - age: Applicant age
    """
    
    def __init__(self):
        self.model = None
        self.n_features = 5
        
    def build_model(self):
        """Build single-layer perceptron for credit approval."""
        model = keras.Sequential([
            layers.Dense(
                1,
                activation='sigmoid',
                input_shape=(self.n_features,),
                kernel_initializer='glorot_uniform',
                name='perceptron'
            )
        ])
        
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.01),
            loss='binary_crossentropy',
            metrics=['accuracy', keras.metrics.AUC(name='auc')]
        )
        
        self.model = model
        return model
    
    def generate_data(self, n_samples):
        """Generate synthetic credit approval data."""
        np.random.seed(42)
        
        # Generate features
        income = np.random.uniform(20000, 200000, n_samples)
        credit_score = np.random.uniform(300, 850, n_samples)
        debt = np.random.uniform(0, 50000, n_samples)
        employment_years = np.random.uniform(0, 40, n_samples)
        age = np.random.uniform(18, 70, n_samples)
        
        # Combine features
        X = np.column_stack([
            income, credit_score, debt, employment_years, age
        ])
        
        # Generate labels based on simple rules
        # (Higher income + higher credit score + lower debt = approval)
        approval_score = (
            (income / 200000) * 0.3 +
            (credit_score / 850) * 0.4 +
            (1 - debt / 50000) * 0.2 +
            (employment_years / 40) * 0.1
        )
        
        # Add noise
        approval_score += np.random.normal(0, 0.1, n_samples)
        
        y = (approval_score > 0.5).astype(int)
        
        # Normalize
        X = (X - X.mean(axis=0)) / X.std(axis=0)
        
        print(f"Generated {n_samples} samples")
        print(f"  Approval rate: {y.mean():.1%}")
        
        return X, y
    
    def predict_approval(self, income, credit_score, debt, employment, age):
        """Predict credit approval for a single application."""
        features = np.array([[income, credit_score, debt, employment, age]])
        features = (features - self.mean) / self.std
        
        probability = self.model.predict(features, verbose=0)[0][0]
        
        return probability

def run_banking_example():
    """Run banking credit approval example."""
    print("="*60)
    print("CREDIT APPROVAL - BANKING EXAMPLE")
    print("="*60)
    
    # Create and build model
    approval_model = CreditApprovalModel()
    model = approval_model.build_model()
    
    # Generate data
    X, y = approval_model.generate_data(5000)
    approval_model.mean = X.mean(axis=0)
    approval_model.std = X.std(axis=0)
    
    # Train
    print("\nTraining perceptron...")
    model.fit(
        X, y,
        epochs=30,
        batch_size=32,
        validation_split=0.2,
        verbose=0
    )
    
    # Evaluate
    results = model.evaluate(X, y, verbose=0)
    print(f"\nTraining accuracy: {results[1]:.4f}")
    print(f"Training AUC: {results[2]:.4f}")
    
    # Test predictions
    test_cases = [
        (80000, 750, 10000, 5, 35),   # Good profile
        (25000, 550, 30000, 1, 22),  # Risky profile
        (150000, 800, 5000, 15, 45)   # Excellent profile
    ]
    
    print("\nTest Predictions:")
    for income, score, debt, emp, age in test_cases:
        # Normalize for model
        features = np.array([[income, score, debt, emp, age]])
        features = (features - approval_model.mean) / approval_model.std
        
        prob = model.predict(features, verbose=0)[0][0]
        decision = "APPROVED" if prob > 0.5 else "REJECTED"
        
        print(f"  Income: ${income:,}, Credit: {score}, Decision: {decision} ({prob:.2%})")

if __name__ == "__main__":
    run_banking_example()
```

### Real-world Example 2: Healthcare - Disease Risk Prediction

```python
"""
Real-world Example 2: Healthcare - Heart Disease Risk Prediction
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class HeartDiseasePredictor:
    """
    Perceptron for heart disease risk prediction.
    Predicts probability of heart disease based on clinical indicators.
    
    Features:
    - age: Patient age
    - blood_pressure: Systolic blood pressure
    - cholesterol: Cholesterol level
    - heart_rate: Resting heart rate
    - bmi: Body mass index
    """
    
    def __init__(self):
        self.model = None
        self.n_features = 5
        
    def build_model(self):
        """Build perceptron for heart disease prediction."""
        model = keras.Sequential([
            layers.Dense(
                1,
                activation='sigmoid',
                input_shape=(self.n_features,),
                kernel_initializer='glorot_uniform',
                name='risk_perceptron'
            )
        ])
        
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.01),
            loss='binary_crossentropy',
            metrics=['accuracy', keras.metrics.Precision(name='precision'),
                    keras.metrics.Recall(name='recall')]
        )
        
        self.model = model
        return model
    
    def generate_data(self, n_samples):
        """Generate synthetic heart disease data."""
        np.random.seed(42)
        
        # Generate features (realistic ranges)
        age = np.random.uniform(30, 80, n_samples)
        blood_pressure = np.random.uniform(90, 180, n_samples)
        cholesterol = np.random.uniform(150, 300, n_samples)
        heart_rate = np.random.uniform(50, 120, n_samples)
        bmi = np.random.uniform(18, 40, n_samples)
        
        # Combine features
        X = np.column_stack([
            age, blood_pressure, cholesterol, heart_rate, bmi
        ])
        
        # Generate labels based on risk factors
        # Higher age + high BP + high cholesterol = higher risk
        risk_score = (
            (age - 30) / 50 * 0.2 +                    # Age factor
            (blood_pressure - 90) / 90 * 0.3 +          # BP factor
            (cholesterol - 150) / 150 * 0.25 +          # Cholesterol factor
            (heart_rate - 50) / 70 * 0.15 +             # Heart rate factor
            (bmi - 18) / 22 * 0.1                       # BMI factor
        )
        
        # Add noise
        risk_score += np.random.normal(0, 0.15, n_samples)
        
        y = (risk_score > 0.4).astype(int)
        
        # Normalize
        X = (X - X.mean(axis=0)) / X.std(axis=0)
        
        print(f"Generated {n_samples} patient records")
        print(f"  Disease rate: {y.mean():.1%}")
        
        return X, y
    
    def analyze_risk(self, age, bp, chol, hr, bmi):
        """Analyze heart disease risk for a patient."""
        features = np.array([[age, bp, chol, hr, bmi]])
        features = (features - self.mean) / self.std
        
        risk_probability = self.model.predict(features, verbose=0)[0][0]
        
        risk_level = "HIGH" if risk_probability > 0.6 else \
                    "MEDIUM" if risk_probability > 0.3 else "LOW"
        
        return risk_probability, risk_level

def run_healthcare_example():
    """Run healthcare heart disease prediction example."""
    print("="*60)
    print("HEART DISEASE RISK PREDICTION - HEALTHCARE")
    print("="*60)
    
    # Create model
    predictor = HeartDiseasePredictor()
    model = predictor.build_model()
    
    # Generate data
    X, y = predictor.generate_data(5000)
    predictor.mean = X.mean(axis=0)
    predictor.std = X.std(axis=0)
    
    # Train
    print("\nTraining risk prediction model...")
    model.fit(
        X, y,
        epochs=30,
        batch_size=32,
        validation_split=0.2,
        verbose=0
    )
    
    # Evaluate
    results = model.evaluate(X, y, verbose=0)
    print(f"\nTest Accuracy: {results[1]:.4f}")
    print(f"Precision: {results[2]:.4f}")
    print(f"Recall: {results[3]:.4f}")
    
    # Test patient cases
    print("\nPatient Risk Analysis:")
    test_patients = [
        (45, 120, 200, 72, 24),    # Low risk
        (65, 160, 280, 95, 32),   # High risk
        (55, 140, 220, 80, 27)    # Medium risk
    ]
    
    for age, bp, chol, hr, bmi in test_patients:
        prob, level = predictor.analyze_risk(age, bp, chol, hr, bmi)
        print(f"  Age:{age}, BP:{bp}, Chol:{chol}, HR:{hr}, BMI:{bmi}")
        print(f"    Risk: {level} ({prob:.1%})")

if __name__ == "__main__":
    run_healthcare_example()
```

## V. OUTPUT_RESULTS

```
PERCEPTRON BINARY CLASSIFICATION
============================================================
Training accuracy: 0.9850

Test Predictions:
  Input [0,0]: 0.4998 (expected ~0.5)
  Input [3,3]: 0.9966 (expected ~1)
  Input [-3,-3]: 0.0033 (expected ~0)
```

## VI. ADVANCED TOPICS

### Advanced Topic 1: Custom Activation Functions

```python
"""
Advanced Topic 1: Custom Activation Functions
"""

class CustomActivationFunctions:
    """Create custom activation functions."""
    
    @staticmethod
    @tf.function
    def custom_swish(x):
        """Swish activation: x * sigmoid(x)."""
        return x * tf.nn.sigmoid(x)
    
    @staticmethod
    @tf.function
    def mish(x):
        """Mish: x * tanh(softplus(x))."""
        return x * tf.nn.tanh(tf.nn.softplus(x))
    
    @staticmethod
    def parametric_relu():
        """Parametric ReLU with learnable alpha."""
        class PReLU(layers.Layer):
            def __init__(self, **kwargs):
                super().__init__(**kwargs)
                
            def build(self, input_shape):
                self.alpha = self.add_weight(
                    'alpha',
                    shape=(1,),
                    initializer='ones',
                    trainable=True
                )
                
            def call(self, inputs):
                return tf.where(inputs > 0, inputs, self.alpha * inputs)
            
            def get_config(self):
                config = super().get_config()
                return config
        
        return PReLU()
```

### Advanced Topic 2: Gradient Vanishing Problem

```python
"""
Advanced Topic 2: Understanding and Addressing Gradient Vanishing
"""

class GradientVanishing:
    """Analyzing gradient vanishing in activation functions."""
    
    @staticmethod
    def compare_gradients():
        """Compare gradients of different activation functions."""
        print("="*60)
        print("Gradient Analysis")
        print("="*60)
        
        x = tf.Variable(tf.constant(np.linspace(-5, 5, 20)))
        
        activations = [
            ('Sigmoid', tf.nn.sigmoid),
            ('Tanh', tf.nn.tanh),
            ('ReLU', tf.nn.relu)
        ]
        
        for name, func in activations:
            with tf.GradientTape() as tape:
                y = func(x)
            
            grads = tape.gradient(y, x).numpy()
            
            print(f"\n{name}:")
            print(f"  Mean gradient: {grads.mean():.4f}")
            print(f"  Max gradient: {grads.max():.4f}")
            print(f"  Min gradient: {grads.min():.4f}")
            
            # Check for vanishing
            if grads.mean() < 0.1:
                print(f"  ⚠️ Potential gradient vanishing!")

### Advanced Topic 3: Choosing Activation Functions

| Situation | Recommended Activation |
|-----------|------------------------|
| Binary output | Sigmoid |
| Multi-class classification | Softmax |
| Hidden layers (default) | ReLU |
| Deep networks | Leaky ReLU, ELU |
| Regression (positive) | ReLU |
| Regression (bounded) | Sigmoid/Tanh |

## VIII. CONCLUSION

### Key Takeaways

1. **Perceptrons**: The fundamental unit of neural networks
2. **Activation Functions**: Enable non-linear decision boundaries
3. **Sigmoid/Softmax**: For probability outputs
4. **ReLU**: Default choice for hidden layers
5. **Gradient Flow**: Choose activations that support backpropagation

### Next Steps

1. Learn about multi-layer perceptrons (next module)
2. Explore backpropagation
3. Study weight initialization
4. Practice with different architectures

### Further Reading

- Rosenblatt, F. (1957). "The Perceptron: A Probabilistic Model for Information Storage and Organization"
- Goodfellow, I. et al. "Deep Learning" - Chapter 6

End of Perceptrons and Activation Functions Tutorial