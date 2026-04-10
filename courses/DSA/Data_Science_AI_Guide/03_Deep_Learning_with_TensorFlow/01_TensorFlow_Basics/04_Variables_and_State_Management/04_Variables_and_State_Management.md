# Variables and State Management

## I. INTRODUCTION

### What are Variables and State?
In TensorFlow, variables are the primary mechanism for storing and updating model parameters during training. Unlike regular tensors which are immutable, variables maintain state across operations and can be updated through gradient descent or other optimization algorithms.

State management encompasses:
- Variable creation and initialization
- Saving and loading model weights
- Checkpointing during training
- Managing shared state across operations

### Why are Variables Important?

1. **Model Parameters**: Neural network weights and biases are stored as variables
2. **Training**: Gradients are applied to variables to minimize loss
3. **State Persistence**: Variables persist across operations
4. **Checkpointing**: Variables can be saved and restored

### Prerequisites
- Understanding of tensors
- Familiarity with computational graphs
- Basic neural network concepts

## II. FUNDAMENTALS

### Key Concepts

**tf.Variable**: A tensor whose value can be changed by ops like `assign`, `assign_add`, `assign_sub`

**Stateful vs Stateless**: Variables maintain state vs operations which compute and are discarded

## III. IMPLEMENTATION

```python
"""
Variables and State Management - Module 4
"""

import tensorflow as tf
import numpy as np

class VariableBasics:
    """Basic variable operations."""
    
    @staticmethod
    def create_variable():
        """Creating TensorFlow variables."""
        print("Creating Variables:")
        print("="*50)
        
        # Scalar variable
        scalar = tf.Variable(5.0, name='scalar')
        print(f"Scalar: {scalar.numpy()}")
        
        # Vector variable
        vector = tf.Variable([1.0, 2.0, 3.0], name='vector')
        print(f"Vector: {vector.numpy()}")
        
        # Matrix variable
        matrix = tf.Variable([[1.0, 2.0], [3.0, 4.0]], name='matrix')
        print(f"Matrix:\n{matrix.numpy()}")
        
        return scalar, vector, matrix
    
    @staticmethod
    def assign_operations():
        """Variable assignment operations."""
        print("\nAssign Operations:")
        print("="*50)
        
        var = tf.Variable(10.0)
        print(f"Initial: {var.numpy()}")
        
        # assign
        var.assign(20.0)
        print(f"After assign(20): {var.numpy()}")
        
        # assign_add
        var.assign_add(5.0)
        print(f"After assign_add(5): {var.numpy()}")
        
        # assign_sub
        var.assign_sub(3.0)
        print(f"After assign_sub(3): {var.numpy()}")
        
        return var
    
    @staticmethod
    def variable_scopes():
        """Variable scopes and reuse."""
        print("\nVariable Scopes:")
        print("="*50)
        
        # Define variable scope
        with tf.variable_scope('layer1', reuse=tf.AUTO_REUSE):
            weights = tf.get_variable(
                'weights', 
                shape=[10, 10],
                initializer=tf.random_normal_initializer()
            )
            bias = tf.get_variable(
                'bias', 
                shape=[10],
                initializer=tf.zeros_initializer()
            )
        
        print(f"Weights shape: {weights.shape}")
        print(f"Bias shape: {bias.shape}")
        
        # Reuse variables
        with tf.variable_scope('layer1', reuse=tf.AUTO_REUSE):
            weights2 = tf.get_variable('weights')
        
        print(f"Reused: {weights is weights2}")
        
        return weights, bias

class TrainingState:
    """Managing state during training."""
    
    def __init__(self):
        self.global_step = tf.Variable(0, trainable=False)
        self.epoch = tf.Variable(0, trainable=False)
    
    @tf.function
    def increment_step(self):
        """Increment training step."""
        self.global_step.assign_add(1)
        return self.global_step
    
    def save_checkpoint(self, path):
        """Save checkpoint."""
        checkpoint = tf.train.Checkpoint(
            step=self.global_step,
            epoch=self.epoch,
            optimizer=tf.Variable(tf.keras.optimizers.Adam()),
            model=tf.Variable(tf.random.normal([10, 10]))
        )
        return checkpoint
    
def demonstrate_variables():
    """Demonstrate variable operations."""
    basics = VariableBasics()
    basics.create_variable()
    basics.assign_operations()
    basics.variable_scopes()
```

### Standard Example: Training Loop with State

```python
"""
Standard Example: Training with Variables
"""

import tensorflow as tf
from tensorflow import keras

class TrainingLoop:
    """Complete training loop with state management."""
    
    def __init__(self, input_dim, output_dim):
        self.input_dim = input_dim
        self.output_dim = output_dim
        
        # Model variables
        self.weights = tf.Variable(
            tf.random.normal([input_dim, output_dim])
        )
        self.bias = tf.Variable(tf.zeros([output_dim]))
        
        # Training state
        self.loss_history = tf.Variable(
            tf.constant([]), 
            trainable=False
        )
    
    @tf.function
    def forward(self, x):
        """Forward pass."""
        return tf.matmul(x, self.weights) + self.bias
    
    @tf.function  
    def train_step(self, x, y, optimizer):
        """Single training step."""
        with tf.GradientTape() as tape:
            pred = self.forward(x)
            loss = tf.reduce_mean(
                keras.losses.mse(y, pred)
            )
        
        gradients = tape.gradient(
            loss, 
            [self.weights, self.bias]
        )
        optimizer.apply_gradients(zip(gradients, [self.weights, self.bias]))
        
        return loss

def run_training_example():
    """Run complete training example."""
    print("="*60)
    print("TRAINING WITH VARIABLES")
    print("="*60)
    
    model = TrainingLoop(input_dim=20, output_dim=10)
    optimizer = keras.optimizers.Adam(0.01)
    
    # Generate data
    x = tf.random.normal([100, 20])
    y = tf.random.normal([100, 10])
    
    # Training
    for epoch in range(100):
        loss = model.train_step(x, y, optimizer)
        
        if (epoch + 1) % 20 == 0:
            print(f"Epoch {epoch+1}: Loss = {loss.numpy():.4f}")
    
    print("Training complete!")

if __name__ == "__main__":
    run_training_example()
```

### Real-world Example 1: Banking - Account Balance Tracker

```python
"""
Real-world Example 1: Banking - Account Balance Tracking
"""

import tensorflow as tf
import numpy as np

class AccountManager:
    """
    Manages account balances with variable state.
    """
    
    def __init__(self, account_id):
        self.account_id = account_id
        self.balance = tf.Variable(0.0, name='balance')
        self.transaction_count = tf.Variable(
            0, 
            trainable=False, 
            dtype=tf.int32
        )
        self.total_deposits = tf.Variable(
            0.0, 
            trainable=False
        )
        self.total_withdrawals = tf.Variable(
            0.0, 
            trainable=False
        )
    
    def deposit(self, amount):
        """Process deposit."""
        if amount <= 0:
            raise ValueError("Deposit amount must be positive")
        
        self.balance.assign_add(amount)
        self.total_deposits.assign_add(amount)
        self.transaction_count.assign_add(1)
        
        return self.balance.numpy()
    
    def withdraw(self, amount):
        """Process withdrawal."""
        if amount <= 0:
            raise ValueError("Withdrawal amount must be positive")
        
        if amount > self.balance.numpy():
            raise ValueError("Insufficient funds")
        
        self.balance.assign_sub(amount)
        self.total_withdrawals.assign_add(amount)
        self.transaction_count.assign_add(1)
        
        return self.balance.numpy()
    
    def get_statement(self):
        """Get account statement."""
        return {
            'account_id': self.account_id,
            'current_balance': self.balance.numpy(),
            'total_deposits': self.total_deposits.numpy(),
            'total_withdrawals': self.total_withdrawals.numpy(),
            'transaction_count': self.transaction_count.numpy()
        }

def run_banking_example():
    """Run banking example."""
    print("="*60)
    print("ACCOUNT BALANCE TRACKING - BANKING")
    print("="*60)
    
    account = AccountManager("ACC-001")
    
    # Process transactions
    account.deposit(1000.0)
    account.deposit(500.0)
    account.withdraw(200.0)
    account.deposit(300.0)
    
    # Get statement
    statement = account.get_statement()
    
    print(f"\nAccount: {statement['account_id']}")
    print(f"Current Balance: ${statement['current_balance']:.2f}")
    print(f"Total Deposits: ${statement['total_deposits']:.2f}")
    print(f"Total Withdrawals: ${statement['total_withdrawals']:.2f}")
    print(f"Transactions: {statement['transaction_count']}")

if __name__ == "__main__":
    run_banking_example()
```

### Real-world Example 2: Healthcare - Patient Vitals Monitor

```python
"""
Real-world Example 2: Healthcare - Patient Vitals Monitoring
"""

import tensorflow as tf
import numpy as np

class VitalsMonitor:
    """
    Monitor patient vital signs with state tracking.
    """
    
    def __init__(self, patient_id):
        self.patient_id = patient_id
        
        # Vital tracking variables
        self.heart_rate = tf.Variable(
            70.0, 
            trainable=False,
            name='heart_rate'
        )
        self.blood_pressure_sys = tf.Variable(
            120.0, 
            trainable=False,
            name='bp_sys'
        )
        self.blood_pressure_dia = tf.Variable(
            80.0, 
            trainable=False,
            name='bp_dia'
        )
        self.spo2 = tf.Variable(
            98.0, 
            trainable=False,
            name='spo2'
        )
        
        # Statistics
        self.reading_count = tf.Variable(
            0, 
            trainable=False,
            dtype=tf.int32
        )
        self.alert_triggered = tf.Variable(
            False, 
            trainable=False,
            dtype=tf.bool
        )
        
    def update_vitals(self, hr, bp_sys, bp_dia, spo2):
        """Update vital signs."""
        self.heart_rate.assign(hr)
        self.blood_pressure_sys.assign(bp_sys)
        self.blood_pressure_dia.assign(bp_dia)
        self.spo2.assign(spo2)
        
        self.reading_count.assign_add(1)
        
        # Check for alerts
        self._check_alerts()
        
        return self.get_status()
    
    def _check_alerts(self):
        """Check vital sign thresholds."""
        alert = False
        
        # Heart rate alerts
        if self.heart_rate.numpy() < 50 or self.heart_rate.numpy() > 150:
            alert = True
        
        # Blood pressure alerts
        if self.blood_pressure_sys.numpy() > 180 or self.blood_pressure_dia.numpy() > 120:
            alert = True
        if self.blood_pressure_sys.numpy() < 90 or self.blood_pressure_dia.numpy() < 60:
            alert = True
        
        # SpO2 alerts
        if self.spo2.numpy() < 92:
            alert = True
        
        self.alert_triggered.assign(alert)
    
    def get_status(self):
        """Get current status."""
        return {
            'patient_id': self.patient_id,
            'heart_rate': self.heart_rate.numpy(),
            'bp': f"{self.blood_pressure_sys.numpy()}/{self.blood_pressure_dia.numpy()}",
            'spo2': self.spo2.numpy(),
            'alert': self.alert_triggered.numpy()
        }

def run_healthcare_example():
    """Run healthcare example."""
    print("="*60)
    print("PATIENT VITALS MONITORING - HEALTHCARE")
    print("="*60)
    
    monitor = VitalsMonitor("P-12345")
    
    # Simulate readings
    readings = [
        (72, 118, 78, 98),
        (68, 122, 82, 97),
        (75, 125, 80, 98),
        (145, 160, 95, 96),  # Abnormal HR
    ]
    
    for hr, bp_sys, bp_dia, spo2 in readings:
        status = monitor.update_vitals(hr, bp_sys, bp_dia, spo2)
        print(f"\nReading #{status['reading_count']}:")
        print(f"  HR: {status['heart_rate']}, BP: {status['bp']}, SpO2: {status['spo2']}")
        print(f"  Alert: {status['alert']}")
    
    print("\nMonitoring complete!")

if __name__ == "__main__":
    run_healthcare_example()
```

## V. OUTPUT_RESULTS

```
TRAINING WITH VARIABLES
============================================================
Epoch 20: Loss = 0.8934
Epoch 40: Loss = 0.8123
...
```

## VI. ADVANCED TOPICS

### Advanced Topic 1: Checkpointing

```python
"""
Advanced Topic 1: Model Checkpointing
"""

class ModelCheckpoint:
    """Saving and loading model state."""
    
    def __init__(self, model_dir):
        self.checkpoint = tf.train.Checkpoint(
            optimizer=optimizer,
            model=model
        )
        self.manager = tf.train.CheckpointManager(
            self.checkpoint, 
            model_dir, 
            max_to_keep=3
        )
    
    def save(self):
        """Save checkpoint."""
        return self.manager.save()
    
    def restore(self, path=None):
        """Restore checkpoint."""
        status = self.checkpoint.restore(
            path or self.manager.latest_checkpoint
        )
        status.expect_partial()
```

End of Variables and State Management Tutorial