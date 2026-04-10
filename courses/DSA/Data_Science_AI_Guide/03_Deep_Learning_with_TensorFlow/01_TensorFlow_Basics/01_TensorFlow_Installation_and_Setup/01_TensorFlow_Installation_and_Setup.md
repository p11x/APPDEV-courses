# TensorFlow Installation and Setup

## I. INTRODUCTION

### What is TensorFlow?
TensorFlow is an open-source machine learning framework developed by Google Brain team. It provides a comprehensive ecosystem of tools, libraries, and community resources that enables researchers to push the state-of-the-art in machine learning, and developers to easily build and deploy ML-powered applications. TensorFlow supports both deep learning and traditional machine learning, making it one of the most versatile frameworks in the field.

TensorFlow's name comes from the computational nature of the framework - data flows through a graph of operations (tensors), where each node represents a mathematical operation and each edge represents the data (tensors) flowing between them. This declarative approach allows TensorFlow to optimize computation for various hardware platforms, from mobile devices to distributed GPU clusters.

### Why is it Important in Deep Learning?
TensorFlow has become the backbone of modern deep learning research and industry applications for several compelling reasons:

1. **Production-Ready**: TensorFlow offers robust deployment capabilities across multiple platforms (server, mobile, embedded, browser). TensorFlow Serving, TensorFlow Lite, and TensorFlow.js enable seamless transition from research to production.

2. **Scalability**: TensorFlow supports distributed training across multiple GPUs and machines, making it suitable for large-scale deep learning projects.

3. **Ecosystem**: The TensorFlow ecosystem includes TensorBoard for visualization, TensorFlow Hub for pre-trained models, TensorFlow Extended (TFX) for ML pipelines, and Keras as the official high-level API.

4. **Community and Support**: Backed by Google and a large community, TensorFlow has extensive documentation, tutorials, and third-party resources.

5. **Flexibility**: Support for both low-level operations (for custom architectures) and high-level APIs (like Keras) caters to different user needs.

### Prerequisites
Before installing TensorFlow, ensure your system meets these requirements:

- **Python**: Version 3.8 to 3.12 (TensorFlow 2.x supports Python 3.8+)
- **Operating System**: Windows, macOS, or Linux
- **Package Manager**: pip (Python's package installer)
- **Optional for GPU support**: NVIDIA GPU with CUDA Compute Capability 3.5+ and cuDNN

Basic understanding of Python programming, virtual environments, and command-line operations is recommended.

## II. FUNDAMENTALS

### Key Terminology

1. **Tensor**: The core data structure in TensorFlow. A tensor is a multi-dimensional array of numbers. Tensors have three properties:
   - rank: number of dimensions
   - shape: size of each dimension
   - data type: type of elements (float32, int32, string, etc.)

2. **Computational Graph**: A directed graph where nodes represent operations and edges represent data flow. TensorFlow 2.x uses eager execution by default, where operations execute immediately.

3. **Keras**: The official high-level API for TensorFlow. Keras provides modular building blocks for rapid model development.

4. **Session (TF1) vs Eager Execution (TF2)**: In TensorFlow 1.x, sessions were required to execute the graph. TensorFlow 2.x uses eager execution by default, making debugging easier.

5. **SavedModel**: TensorFlow's standard format for exporting trained models. Includes model architecture, weights, and training configuration.

6. **Checkpoint**: Binary files containing model weights, used for resuming training or inference.

### Core Installation Methods

#### Method 1: pip Install (Recommended)
```bash
# Create virtual environment (recommended)
python -m venv tf_env
source tf_env/bin/activate  # On Windows: tf_env\Scripts\activate

# Install TensorFlow with pip
pip install tensorflow

# Verify installation
python -c "import tensorflow as tf; print(tf.__version__)"
```

#### Method 2: GPU Support
```bash
# Install TensorFlow with GPU support
pip install tensorflow[gpu]

# Verify GPU availability
python -c "import tensorflow as tf; print('GPU Available:', tf.config.list_physical_devices('GPU'))"
```

#### Method 3: Using Anaconda
```bash
# Create conda environment
conda create -n tf_env python=3.10
conda activate tf_env

# Install TensorFlow
conda install tensorflow

# Or use pip within conda
pip install tensorflow
```

### Version Compatibility Matrix

| TensorFlow Version | Python Version | CUDA Version | Release Date |
|-------------------|----------------|---------------|--------------|
| 2.16.x | 3.9-3.12 | 12.3 | 2024 |
| 2.15.x | 3.9-3.11 | 12.2 | 2023 |
| 2.14.x | 3.9-3.11 | 12.2 | 2023 |
| 2.13.x | 3.8-3.11 | 11.8 | 2023 |
| 2.12.x | 3.7-3.10 | 11.8 | 2023 |

## III. IMPLEMENTATION

### Step 1: System Check and Environment Setup

```python
"""
TensorFlow Installation and Setup - Module 1
This script demonstrates setting up TensorFlow and verifying the installation.
"""

# Import required libraries
import sys
import subprocess
import platform
import os

def check_python_version():
    """Check if Python version meets TensorFlow requirements."""
    version = sys.version_info
    print(f"Python Version: {version.major}.{version.minor}.{version.micro}")
    
    if version.major < 3 or (version.major == 3 and version.minor < 8):
        print("WARNING: TensorFlow requires Python 3.8 or higher")
        return False
    print("✓ Python version is compatible")
    return True

def check_system_info():
    """Display system information."""
    print("\n" + "="*50)
    print("SYSTEM INFORMATION")
    print("="*50)
    print(f"Platform: {platform.platform()}")
    print(f"Processor: {platform.processor()}")
    print(f"Architecture: {platform.machine()}")
    print(f"Python Executable: {sys.executable}")

def check_system_dependencies():
    """Check system-level dependencies."""
    print("\n" + "="*50)
    print("SYSTEM DEPENDENCIES")
    print("="*50)
    
    # Check for pip
    try:
        import pip
        print(f"✓ pip installed: {pip.__version__}")
    except ImportError:
        print("✗ pip not found")
    
    # Check for virtualenv
    try:
        import virtualenv
        print(f"✓ virtualenv installed")
    except ImportError:
        print("- virtualenv not installed (optional)")

def check_nvidia_gpu():
    """Check for NVIDIA GPU availability (Windows-specific)."""
    print("\n" + "="*50)
    print("GPU CONFIGURATION")
    print("="*50)
    
    try:
        import subprocess
        result = subprocess.run(['nvidia-smi'], capture_output=True, text=True)
        if result.returncode == 0:
            print("✓ NVIDIA GPU detected")
            print(result.stdout[:500])
        else:
            print("- No NVIDIA GPU or nvidia-smi not available")
    except FileNotFoundError:
        print("- nvidia-smi not found (GPU monitoring not available)")

def create_virtual_environment():
    """Create a virtual environment for TensorFlow projects."""
    env_name = "tf_venv"
    
    print(f"\nCreating virtual environment: {env_name}")
    
    try:
        import venv
        subprocess.run([sys.executable, "-m", "venv", env_name], check=True)
        print(f"✓ Virtual environment created: {env_name}")
    except Exception as e:
        print(f"Error creating environment: {e}")

if __name__ == "__main__":
    check_system_info()
    check_system_dependencies()
    check_python_version()
    check_nvidia_gpu()
```

### Step 2: TensorFlow Installation and Verification

```python
"""
Step 2: Install and verify TensorFlow installation
"""

import subprocess
import sys
import os

def install_tensorflow():
    """
    Install TensorFlow using pip. This function demonstrates the installation process.
    Note: In production, use a virtual environment.
    """
    print("Installing TensorFlow (this may take several minutes)...")
    
    # Upgrade pip first for better installation experience
    subprocess.run([sys.executable, "-m", "pip", "install", "--upgrade", "pip"],
                 capture_output=True)
    
    # Install TensorFlow
    result = subprocess.run([sys.executable, "-m", "pip", "install", "tensorflow"],
                           capture_output=True, text=True)
    
    if result.returncode == 0:
        print("✓ TensorFlow installed successfully")
    else:
        print(f"Installation output: {result.stdout}")
        print(f"Error: {result.stderr}")

def verify_tensorflow_installation():
    """
    Verify TensorFlow installation and display configuration details.
    """
    try:
        import tensorflow as tf
        
        print("\n" + "="*50)
        print("TENSORFLOW VERIFICATION")
        print("="*50)
        
        # Version information
        print(f"TensorFlow Version: {tf.__version__}")
        
        # Build information
        print(f"\nBuild Info:")
        print(f"  - Compiled with CUDA: {tf.test.is_built_with_cuda()}")
        print(f"  - GPU Support: {tf.test.is_gpu_available()}")
        
        # Available devices
        print(f"\nAvailable Devices:")
        gpus = tf.config.list_physical_devices('GPU')
        if gpus:
            print(f"  - GPUs: {len(gpus)}")
            for gpu in gpus:
                print(f"    * {gpu}")
        else:
            print("  - No GPU devices available")
        
        cpus = tf.config.list_physical_devices('CPU')
        print(f"  - CPUs: {len(cpus)}")
        
        return True
        
    except ImportError:
        print("✗ TensorFlow not installed")
        return False
    except Exception as e:
        print(f"Error verifying TensorFlow: {e}")
        return False

def test_basic_tensorflow_operations():
    """
    Test basic TensorFlow operations to verify the installation works correctly.
    """
    import tensorflow as tf
    
    print("\n" + "="*50)
    print("BASIC TENSORFLOW OPERATIONS TEST")
    print("="*50)
    
    # Test 1: Create a tensor
    tensor = tf.constant([[1, 2, 3], [4, 5, 6]])
    print(f"Test 1 - Create Tensor:")
    print(f"  Input: tf.constant([[1, 2, 3], [4, 5, 6]])")
    print(f"  Output:\n  {tensor.numpy()}")
    print(f"  ✓ Tensor created successfully")
    
    # Test 2: Basic math operations
    a = tf.constant([1.0, 2.0, 3.0])
    b = tf.constant([4.0, 5.0, 6.0])
    c = tf.add(a, b)
    print(f"\nTest 2 - Add Operations:")
    print(f"  Input: tf.add([1,2,3], [4,5,6])")
    print(f"  Output: {c.numpy()}")
    print(f"  ✓ Math operations work")
    
    # Test 3: Matrix multiplication
    matrix_a = tf.constant([[1, 2], [3, 4]])
    matrix_b = tf.constant([[5, 6], [7, 8]])
    result = tf.matmul(matrix_a, matrix_b)
    print(f"\nTest 3 - Matrix Multiplication:")
    print(f"  Input: matmul([[1,2],[3,4]], [[5,6],[7,8]])")
    print(f"  Output:\n  {result.numpy()}")
    print(f"  ✓ Matrix operations work")
    
    # Test 4: Automatic differentiation (Gradients)
    x = tf.Variable(3.0)
    with tf.GradientTape() as tape:
        y = x ** 2
    grad = tape.gradient(y, x)
    print(f"\nTest 4 - Gradient Computation:")
    print(f"  Input: y = x^2, x = 3.0")
    print(f"  Gradient: dy/dx = {grad.numpy()}")
    print(f"  ✓ Gradient tape works")
    
    return True

def configure_gpu_memory_growth():
    """
    Configure GPU memory growth to avoid OOM errors.
    This is important for training large models.
    """
    import tensorflow as tf
    
    print("\n" + "="*50)
    print("GPU MEMORY CONFIGURATION")
    print("="*50)
    
    gpus = tf.config.list_physical_devices('GPU')
    
    if gpus:
        try:
            # Enable memory growth for all GPUs
            for gpu in gpus:
                tf.config.experimental.set_memory_growth(gpu, True)
            print("✓ GPU memory growth enabled")
            
            # Log device placement
            print("\nLogical devices after configuration:")
            logical_gpus = tf.config.list_logical_devices('GPU')
            print(f"  - Logical GPUs: {len(logical_gpus)}")
            
        except RuntimeError as e:
            print(f"Error setting memory growth: {e}")
    else:
        print("No GPUs available for configuration")

def set_random_seed():
    """
    Set random seeds for reproducibility in deep learning experiments.
    """
    import tensorflow as tf
    import numpy as np
    import random
    
    # Set seeds
    tf.random.set_seed(42)
    np.random.seed(42)
    random.seed(42)
    
    # Configure TensorFlow to use deterministic operations
    tf.config.experimental.enable_op_determinism()
    
    print("\n" + "="*50)
    print("RANDOM SEED CONFIGURATION")
    print("="*50)
    print("✓ Random seeds set for reproducibility")
    print("  - TensorFlow: 42")
    print("  - NumPy: 42")
    print("  - Python random: 42")
    print("  - Deterministic operations: enabled")

if __name__ == "__main__":
    verify_tensorflow_installation()
    test_basic_tensorflow_operations()
    configure_gpu_memory_growth()
    set_random_seed()
```

### Step 3: Project Structure Setup

```python
"""
Step 3: Create a standard project structure for TensorFlow projects
"""

import os
import shutil
from pathlib import Path

class TensorFlowProjectSetup:
    """
    Creates a professional project structure for TensorFlow projects.
    """
    
    def __init__(self, project_name):
        self.project_name = project_name
        self.root_dir = Path(project_name)
        
    def create_directory_structure(self):
        """Create the project directory structure."""
        
        directories = {
            "src/data": "Data loading and preprocessing",
            "src/models": "Model definitions",
            "src/training": "Training loop and utilities",
            "src/evaluation": "Evaluation and metrics",
            "src/utils": "Utility functions",
            "configs": "Configuration files",
            "notebooks": "Jupyter notebooks",
            "tests": "Unit tests",
            "logs": "Training logs",
            "checkpoints": "Model checkpoints",
            "saved_models": "Exported models",
            "results": "Output results"
        }
        
        print("Creating project structure:")
        
        for dir_path, description in directories.items():
            full_path = self.root_dir / dir_path
            full_path.mkdir(parents=True, exist_ok=True)
            print(f"  ✓ {dir_path}/ - {description}")
        
        return True
    
    def create_config_files(self):
        """Create configuration files."""
        
        # Create __init__.py files
        src_dirs = ["data", "models", "training", "evaluation", "utils"]
        
        for dir_name in src_dirs:
            init_file = self.root_dir / "src" / dir_name / "__init__.py"
            init_file.touch()
        
        print("✓ Created __init__.py files")
        
        # Create config.yaml
        config_content = """# TensorFlow Project Configuration
project:
  name: tf_project
  version: 1.0.0
  
model:
  type: classifier
  input_shape: [28, 28, 1]
  num_classes: 10
  
training:
  batch_size: 32
  epochs: 100
  learning_rate: 0.001
  optimizer: adam
  
data:
  dataset: mnist
  validation_split: 0.2
  shuffle_buffer: 10000
  
paths:
  data_dir: ./data
  checkpoint_dir: ./checkpoints
  log_dir: ./logs
  model_dir: ./saved_models
"""
        
        config_file = self.root_dir / "configs" / "config.yaml"
        config_file.write_text(config_content)
        print("✓ Created config.yaml")
        
        return True
    
    def create_requirements_txt(self):
        """Create requirements.txt file."""
        
        requirements = """# TensorFlow Requirements
tensorflow>=2.15.0
numpy>=1.24.0
pandas>=2.0.0
scikit-learn>=1.3.0
matplotlib>=3.7.0
seaborn>=0.12.0
tensorboard>=2.15.0
tensorflow-datasets>=4.9.0
"""
        
        req_file = self.root_dir / "requirements.txt"
        req_file.write_text(requirements)
        print("✓ Created requirements.txt")
        
        return True
    
    def create_readme(self):
        """Create README.md file."""
        
        readme_content = f"""# {self.project_name}

A TensorFlow-based deep learning project.

## Project Structure

```
{self.project_name}/
├── src/
│   ├── data/          # Data loading
│   ├── models/        # Model definitions
│   ├── training/      # Training utilities
│   ├── evaluation/    # Evaluation metrics
│   └── utils/         # Utility functions
├── configs/          # Configuration files
├── notebooks/        # Jupyter notebooks
├── tests/            # Unit tests
├── logs/              # Training logs
├── checkpoints/      # Model checkpoints
├── saved_models/     # Exported models
└── results/          # Output results
```

## Installation

```bash
pip install -r requirements.txt
```

## Usage

```bash
# Train model
python src/training/train.py --config configs/config.yaml

# Evaluate model
python src/evaluation/evaluate.py --checkpoint checkpoints/model.h5
```
"""
        
        readme_file = self.root_dir / "README.md"
        readme_file.write_text(readme_content)
        print("✓ Created README.md")
        
        return True
    
    def create_gitignore(self):
        """Create .gitignore file."""
        
        gitignore_content = """# Python
__pycache__/
*.py[cod]
*$py.class
*.so
.Python
build/
develop-eggs/
dist/
downloads/
eggs/
.eggs/
lib/
lib64/
parts/
sdist/
var/
wheels/
*.egg-info/
.installed.cfg
*.egg

# Virtual Environment
venv/
env/
ENV/

# TensorFlow
logs/
checkpoints/
*.ckpt
*.h5
saved_models/

# Jupyter
.ipynb_checkpoints
*.ipynb

# IDE
.vscode/
.idea/
*.swp
*.swo

# OS
.DS_Store
Thumbs.db
"""
        
        gitignore_file = self.root_dir / ".gitignore"
        gitignore_file.write_text(gitignore_content)
        print("✓ Created .gitignore")
        
        return True

def setup_project(project_name="my_tf_project"):
    """Setup a complete TensorFlow project."""
    setup = TensorFlowProjectSetup(project_name)
    setup.create_directory_structure()
    setup.create_config_files()
    setup.create_requirements_txt()
    setup.create_readme()
    setup.create_gitignore()
    
    print("\n" + "="*50)
    print(f"Project '{project_name}' setup complete!")
    print("="*50)
    
    return setup

if __name__ == "__main__":
    setup_project("my_tf_project")
```

### Best Practices

```python
"""
Best Practices for TensorFlow Development
"""

import tensorflow as tf

def best_practice_1_use_keras():
    """
    Best Practice 1: Use Keras API for rapid development.
    Keras provides modular, composable, and extensible building blocks.
    """
    # Instead of low-level TensorFlow operations,
    # use Keras layers and models
    
    from tensorflow import keras
    from tensorflow.keras import layers
    
    # Define a simple model using Keras
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
    
    return model

def best_practice_2_use_tf_data():
    """
    Best Practice 2: Use tf.data for efficient data pipelines.
    tf.data provides optimized data loading and preprocessing.
    """
    # Instead of manual data loading,
    # use tf.data datasets
    
    # Create a dataset from numpy arrays
    import numpy as np
    
    # Sample data
    x = np.random.rand(1000, 784).astype('float32')
    y = np.random.randint(0, 10, 1000)
    
    # Create tf.data dataset
    dataset = tf.data.Dataset.from_tensor_slices((x, y))
    
    # Apply transformations
    dataset = dataset.shuffle(buffer_size=1000)
    dataset = dataset.batch(32)
    dataset = dataset.prefetch(tf.data.AUTOTUNE)
    
    return dataset

def best_practice_3_use_tensorboard():
    """
    Best Practice 3: Use TensorBoard for visualization.
    TensorBoard provides insights into training metrics and model graphs.
    """
    from tensorflow.keras.callbacks import TensorBoard
    
    # Create TensorBoard callback
    tensorboard_callback = TensorBoard(
        log_dir='./logs',
        histogram_freq=1,
        write_graph=True,
        update_freq='epoch'
    )
    
    return tensorboard_callback

def best_practice_4_use_callbacks():
    """
    Best Practice 4: Use callbacks for training control.
    Callbacks provide hooks for training lifecycle events.
    """
    from tensorflow.keras.callbacks import (
        EarlyStopping,
        ModelCheckpoint,
        LearningRateScheduler,
        ReduceLROnPlateau
    )
    
    callbacks = [
        # Stop training when validation loss stops improving
        EarlyStopping(
            monitor='val_loss',
            patience=10,
            restore_best_weights=True
        ),
        
        # Save best model during training
        ModelCheckpoint(
            filepath='./checkpoints/best_model.h5',
            monitor='val_loss',
            save_best_only=True
        ),
        
        # Reduce learning rate when training plateaus
        ReduceLROnPlateau(
            monitor='val_loss',
            factor=0.5,
            patience=5
        )
    ]
    
    return callbacks

def best_practice_5_use_gpu_efficiently():
    """
    Best Practice 5: Configure GPU for efficient memory usage.
    """
    import tensorflow as tf
    
    # Allow dynamic memory allocation
    gpus = tf.config.list_physical_devices('GPU')
    if gpus:
        for gpu in gpus:
            tf.config.experimental.set_memory_growth(gpu, True)
    
    # Set memory limit (if needed)
    # tf.config.experimental.set_virtual_device_configuration(
    #     gpus[0],
    #     [tf.config.experimental.VirtualDeviceConfiguration(
    #         memory_limit=4096  # MB
    #     )]
    # )

def best_practice_6_reproducibility():
    """
    Best Practice 6: Set random seeds for reproducibility.
    """
    import random
    import numpy as np
    import tensorflow as tf
    
    # Set all random seeds
    random.seed(42)
    np.random.seed(42)
    tf.random.set_seed(42)
    
    # Enable deterministic operations
    tf.config.experimental.enable_op_determinism()

class TensorFlowBestPractices:
    """Main class demonstrating TensorFlow best practices."""
    
    @staticmethod
    def demonstrate():
        """Demonstrate all best practices."""
        print("="*50)
        print("TENSORFLOW BEST PRACTICES")
        print("="*50)
        
        print("\n1. Using Keras API")
        model = best_practice_1_use_keras()
        print(f"   Created model: {model.name}")
        
        print("\n2. Using tf.data")
        dataset = best_practice_2_use_tf_data()
        print(f"   Dataset element_spec: {dataset.element_spec}")
        
        print("\n3. Using TensorBoard")
        tb_cb = best_practice_3_use_tensorboard()
        print(f"   TensorBoard callback created")
        
        print("\n4. Using Callbacks")
        callbacks = best_practice_4_use_callbacks()
        print(f"   Created {len(callbacks)} callbacks")
        
        print("\n5. GPU Configuration")
        best_practice_5_use_gpu_efficiently()
        print("   GPU memory growth enabled")
        
        print("\n6. Reproducibility")
        best_practice_6_reproducibility()
        print("   Random seeds set to 42")
        
        print("\n" + "="*50)
        
if __name__ == "__main__":
    TensorFlowBestPractices.demonstrate()
```

## IV. APPLICATIONS

### Standard Example: MNIST Classification

```python
"""
Standard Example: MNIST Digit Classification
This example demonstrates a complete TensorFlow workflow for image classification.
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np
import matplotlib.pyplot as plt

class MNISTClassifier:
    """
    MNIST digit classifier using TensorFlow/Keras.
    """
    
    def __init__(self):
        self.model = None
        self.history = None
        
    def load_data(self):
        """Load MNIST dataset."""
        (x_train, y_train), (x_test, y_test) = keras.datasets.mnist.load_data()
        
        # Normalize pixel values to [0, 1]
        x_train = x_train.astype('float32') / 255.0
        x_test = x_test.astype('float32') / 255.0
        
        # Add channel dimension for CNN
        x_train = np.expand_dims(x_train, axis=-1)
        x_test = np.expand_dims(x_test, axis=-1)
        
        # Convert labels to categorical
        y_train = keras.utils.to_categorical(y_train, 10)
        y_test = keras.utils.to_categorical(y_test, 10)
        
        print(f"Training set: {x_train.shape[0]} samples")
        print(f"Test set: {x_test.shape[0]} samples")
        
        return (x_train, y_train), (x_test, y_test)
    
    def build_model(self):
        """Build the CNN model."""
        model = keras.Sequential([
            # Input layer
            layers.Input(shape=(28, 28, 1)),
            
            # First convolutional block
            layers.Conv2D(32, (3, 3), activation='relu', padding='same'),
            layers.MaxPooling2D((2, 2)),
            
            # Second convolutional block
            layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
            layers.MaxPooling2D((2, 2)),
            
            # Flatten and dense layers
            layers.Flatten(),
            layers.Dropout(0.5),
            layers.Dense(128, activation='relu'),
            layers.Dense(10, activation='softmax')
        ])
        
        model.compile(
            optimizer='adam',
            loss='categorical_crossentropy',
            metrics=['accuracy']
        )
        
        model.summary()
        
        self.model = model
        return model
    
    def train(self, x_train, y_train, epochs=5, batch_size=32):
        """Train the model."""
        
        # Split training data for validation
        split = int(0.9 * x_train.shape[0])
        x_val = x_train[split:]
        y_val = y_train[split:]
        x_train_split = x_train[:split]
        y_train_split = y_train[:split]
        
        self.history = self.model.fit(
            x_train_split, y_train_split,
            validation_data=(x_val, y_val),
            epochs=epochs,
            batch_size=batch_size,
            verbose=1
        )
        
        return self.history
    
    def evaluate(self, x_test, y_test):
        """Evaluate the model on test data."""
        
        results = self.model.evaluate(x_test, y_test, verbose=1)
        
        print("\nTest Results:")
        print(f"  Loss: {results[0]:.4f}")
        print(f"  Accuracy: {results[1]:.4f}")
        
        return results
    
    def predict(self, x_test):
        """Make predictions on test data."""
        
        predictions = self.model.predict(x_test)
        
        # Get predicted classes
        predicted_classes = np.argmax(predictions, axis=1)
        
        return predicted_classes

def run_mnist_example():
    """Run the complete MNIST classification example."""
    
    print("="*50)
    print("MNIST Digit Classification Example")
    print("="*50)
    
    # Create classifier
    classifier = MNISTClassifier()
    
    # Load data
    print("\nLoading MNIST dataset...")
    (x_train, y_train), (x_test, y_test) = classifier.load_data()
    
    # Build model
    print("\nBuilding model...")
    classifier.build_model()
    
    # Train model
    print("\nTraining model...")
    classifier.train(x_train, y_train, epochs=5)
    
    # Evaluate model
    print("\nEvaluating model...")
    classifier.evaluate(x_test, y_test)
    
    print("\n" + "="*50)
    print("MNIST Example Complete!")
    print("="*50)

if __name__ == "__main__":
    run_mnist_example()
```

### Real-world Example 1: Banking/Finance - Credit Card Fraud Detection

```python
"""
Real-world Example 1: Banking/Finance - Credit Card Fraud Detection
This example demonstrates fraud detection using a deep learning model.
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class CreditCardFraudDetection:
    """
    Credit card fraud detection model for banking applications.
    
    This model identifies suspicious transactions based on:
    - Transaction amount
    - Transaction location
    - Time of transaction
    - Merchant category
    - Customer history
    """
    
    def __init__(self):
        self.model = None
        self.feature_names = [
            'amount', 'hour', 'day_of_week', 'distance_from_home',
            'distance_from_last_transaction', 'merchant_category',
            'customer_tenure', 'avg_spending', 'transaction_count'
        ]
        
    def generate_synthetic_data(self, n_samples=10000):
        """
        Generate synthetic fraud detection data.
        In production, this would connect to actual transaction data.
        """
        np.random.seed(42)
        
        # Generate normal transactions
        normal_transactions = np.random.randn(n_samples, len(self.feature_names))
        normal_transactions = normal_transactions * np.array([
            100, 12, 6, 50, 20, 5, 365, 500, 30
        ])
        
        # Generate fraudulent transactions (anomalies)
        fraud_transactions = np.random.randn(n_samples // 10, len(self.feature_names))
        fraud_transactions = fraud_transactions * np.array([
            500, 12, 6, 200, 5, 5, 365, 2000, 5
        ]) + np.array([
            200, 2, 3, 150, 2, 0, 30, 1500, 2
        ])
        
        # Combine and create labels
        X = np.vstack([normal_transactions, fraud_transactions])
        y = np.array([0] * n_samples + [1] * (n_samples // 10))
        
        # Shuffle
        indices = np.random.permutation(len(X))
        X = X[indices]
        y = y[indices]
        
        # Normalize features
        mean = X.mean(axis=0)
        std = X.std(axis=0)
        X = (X - mean) / std
        
        # Split into train/test
        split = int(0.8 * len(X))
        X_train, X_test = X[:split], X[split:]
        y_train, y_test = y[:split], y[split:]
        
        print(f"Training samples: {len(X_train)}")
        print(f"Test samples: {len(X_test)}")
        print(f"Fraud rate (train): {y_train.mean():.2%}")
        print(f"Fraud rate (test): {y_test.mean():.2%}")
        
        return (X_train, y_train), (X_test, y_test)
    
    def build_model(self, input_dim):
        """
        Build fraud detection neural network.
        
        Architecture:
        - Input: Transaction features
        - Hidden layers with dropout for regularization
        - Output: Binary classification (fraud/not fraud)
        """
        model = keras.Sequential([
            layers.Input(shape=(input_dim,)),
            
            # First hidden layer
            layers.Dense(64, activation='relu'),
            layers.BatchNormalization(),
            layers.Dropout(0.3),
            
            # Second hidden layer
            layers.Dense(32, activation='relu'),
            layers.BatchNormalization(),
            layers.Dropout(0.3),
            
            # Third hidden layer
            layers.Dense(16, activation='relu'),
            layers.BatchNormalization(),
            
            # Output layer
            layers.Dense(1, activation='sigmoid')
        ])
        
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.001),
            loss='binary_crossentropy',
            metrics=[
                'accuracy',
                keras.metrics.Precision(name='precision'),
                keras.metrics.Recall(name='recall'),
                keras.metrics.AUC(name='auc')
            ]
        )
        
        self.model = model
        return model
    
    def train(self, X_train, y_train, epochs=50, batch_size=32):
        """Train the fraud detection model."""
        
        # Handle class imbalance with class weights
        class_weights = {0: 1.0, 1: 10.0}  # Upweight fraud cases
        
        history = self.model.fit(
            X_train, y_train,
            epochs=epochs,
            batch_size=batch_size,
            validation_split=0.2,
            class_weight=class_weights,
            verbose=1
        )
        
        return history
    
    def evaluate(self, X_test, y_test):
        """Evaluate the fraud detection model."""
        
        results = self.model.evaluate(X_test, y_test, verbose=1)
        
        # Get predictions
        y_pred = self.model.predict(X_test)
        y_pred_class = (y_pred > 0.5).astype(int).flatten()
        
        # Calculate confusion matrix
        from sklearn.metrics import confusion_matrix, classification_report
        
        print("\nConfusion Matrix:")
        print(confusion_matrix(y_test, y_pred_class))
        
        print("\nClassification Report:")
        print(classification_report(y_test, y_pred_class))
        
        return results
    
    def detect_fraud(self, transaction_features):
        """Detect if a transaction is fraudulent."""
        
        prediction = self.model.predict(transaction_features)
        probability = prediction[0][0]
        
        is_fraud = probability > 0.5
        risk_score = probability * 100
        
        return is_fraud, risk_score

def run_fraud_detection_example():
    """Run the complete fraud detection example."""
    
    print("="*50)
    print("Credit Card Fraud Detection - Banking Example")
    print("="*50)
    
    # Create fraud detector
    fraud_detector = CreditCardFraudDetection()
    
    # Generate data
    print("\nGenerating synthetic transaction data...")
    (X_train, y_train), (X_test, y_test) = fraud_detector.generate_synthetic_data()
    
    # Build model
    print("\nBuilding fraud detection model...")
    fraud_detector.build_model(X_train.shape[1])
    
    # Train model
    print("\nTraining model...")
    fraud_detector.train(X_train, y_train, epochs=30)
    
    # Evaluate model
    print("\nEvaluating model...")
    fraud_detector.evaluate(X_test, y_test)
    
    print("\n" + "="*50)
    print("Credit Card Fraud Detection Complete!")
    print("="*50)

if __name__ == "__main__":
    run_fraud_detection_example()
```

### Real-world Example 2: Healthcare - Medical Image Classification

```python
"""
Real-world Example 2: Healthcare - Pneumonia Detection from Chest X-rays
This example demonstrates medical image classification for healthcare applications.
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import numpy as np

class ChestXRayClassifier:
    """
    Pneumonia detection from chest X-ray images.
    
    Use case: Help radiologists identify pneumonia from chest X-rays.
    Output: Normal or Pneumonia classification with confidence score.
    """
    
    def __init__(self):
        self.model = None
        
    def generate_synthetic_data(self, img_size=(128, 128), n_samples=1000):
        """
        Generate synthetic chest X-ray images.
        In production, this would load actual medical images.
        """
        np.random.seed(42)
        
        # Generate synthetic X-ray images
        # Normal: Lighter, more uniform images
        # Pneumonia: Darker regions, more patterns
        
        def generate_images(label, n):
            images = []
            for _ in range(n):
                # Base image
                img = np.random.randn(img_size[0], img_size[1]) * 0.1 + 0.5
                
                if label == 1:  # Pneumonia: Add patterns
                    # Add darker regions
                    for _ in range(5):
                        x, y = np.random.randint(0, img_size[1], 2)
                        r = np.random.randint(5, 20)
                        for i in range(img_size[0]):
                            for j in range(img_size[1]):
                                if (i-x)**2 + (j-y)**2 < r**2:
                                    img[i, j] -= np.random.uniform(0.1, 0.3)
                
                # Add noise
                img += np.random.randn(img_size[0], img_size[1]) * 0.05
                
                # Clip values
                img = np.clip(img, 0, 1)
                
                images.append(img)
            
            return np.array(images)
        
        # Generate normal and pneumonia images
        normal_images = generate_images(0, n_samples)
        pneumonia_images = generate_images(1, n_samples)
        
        # Combine and create labels
        X = np.concatenate([normal_images, pneumonia_images])
        X = X.astype('float32')
        X = np.expand_dims(X, axis=-1)  # Add channel dimension
        
        y = np.array([0] * n_samples + [1] * n_samples)
        
        # Shuffle
        indices = np.random.permutation(len(X))
        X = X[indices]
        y = y[indices]
        
        # Split into train/test
        split = int(0.8 * len(X))
        X_train, X_test = X[:split], X[split:]
        y_train, y_test = y[:split], y[split:]
        
        print(f"Training set: {X_train.shape[0]} images")
        print(f"Test set: {X_test.shape[0]} images")
        print(f"Image shape: {img_size}")
        print(f"Pneumonia rate (train): {y_train.mean():.2%}")
        
        return (X_train, y_train), (X_test, y_test)
    
    def build_model(self, input_shape):
        """
        Build CNN for chest X-ray classification.
        
        Architecture designed for medical image classification:
        - Convolutional layers for feature extraction
        - Batch normalization for training stability
        - Dropout for regularization
        - Binary output for Normal/Pneumonia
        """
        model = keras.Sequential([
            # Input layer
            layers.Input(shape=input_shape),
            
            # First convolutional block
            layers.Conv2D(32, (3, 3), activation='relu', padding='same'),
            layers.BatchNormalization(),
            layers.MaxPooling2D((2, 2)),
            
            # Second convolutional block
            layers.Conv2D(64, (3, 3), activation='relu', padding='same'),
            layers.BatchNormalization(),
            layers.MaxPooling2D((2, 2)),
            
            # Third convolutional block
            layers.Conv2D(128, (3, 3), activation='relu', padding='same'),
            layers.BatchNormalization(),
            layers.MaxPooling2D((2, 2)),
            
            # Flatten and dense layers
            layers.Flatten(),
            layers.Dropout(0.5),
            layers.Dense(64, activation='relu'),
            layers.BatchNormalization(),
            
            # Output layer
            layers.Dense(1, activation='sigmoid')
        ])
        
        model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=0.001),
            loss='binary_crossentropy',
            metrics=[
                'accuracy',
                keras.metrics.Precision(name='precision'),
                keras.metrics.Recall(name='recall'),
                keras.metrics.AUC(name='auc')
            ]
        )
        
        model.summary()
        
        self.model = model
        return model
    
    def train(self, X_train, y_train, epochs=20, batch_size=32):
        """Train the chest X-ray classifier."""
        
        # Handle class imbalance
        class_weights = {0: 1.0, 1: 1.5}  # Upweight pneumonia
        
        history = self.model.fit(
            X_train, y_train,
            epochs=epochs,
            batch_size=batch_size,
            validation_split=0.2,
            class_weight=class_weights,
            verbose=1
        )
        
        return history
    
    def evaluate(self, X_test, y_test):
        """Evaluate the model."""
        
        results = self.model.evaluate(X_test, y_test, verbose=1)
        
        # Get predictions
        y_pred = self.model.predict(X_test)
        y_pred_class = (y_pred > 0.5).astype(int).flatten()
        
        # Calculate metrics
        from sklearn.metrics import confusion_matrix, classification_report
        
        print("\nConfusion Matrix:")
        print(confusion_matrix(y_test, y_pred_class))
        
        print("\nClassification Report:")
        print(classification_report(y_test, y_pred_class, 
                            target_names=['Normal', 'Pneumonia']))
        
        return results
    
    def predict_xray(self, xray_image):
        """
        Predict pneumonia from a chest X-ray.
        
        Args:
            xray_image: Preprocessed X-ray image
            
        Returns:
            classification: 'Normal' or 'Pneumonia'
            confidence: Prediction confidence (0-100%)
        """
        prediction = self.model.predict(xray_image)
        confidence = prediction[0][0]
        
        classification = 'Pneumonia' if confidence > 0.5 else 'Normal'
        
        return classification, float(confidence * 100)

def run_healthcare_example():
    """Run the complete healthcare example."""
    
    print("="*50)
    print("Chest X-Ray Pneumonia Detection - Healthcare Example")
    print("="*50)
    
    # Create classifier
    classifier = ChestXRayClassifier()
    
    # Generate data
    print("\nGenerating synthetic X-ray images...")
    (X_train, y_train), (X_test, y_test) = classifier.generate_synthetic_data()
    
    # Build model
    print("\nBuilding CNN model...")
    classifier.build_model(X_train.shape[1:])
    
    # Train model
    print("\nTraining model...")
    classifier.train(X_train, y_train, epochs=20)
    
    # Evaluate model
    print("\nEvaluating model...")
    classifier.evaluate(X_test, y_test)
    
    print("\n" + "="*50)
    print("Chest X-Ray Classification Complete!")
    print("="*50)

if __name__ == "__main__":
    run_healthcare_example()
```

## V. OUTPUT_RESULTS

### Expected Output Format

When running the TensorFlow installation and setup examples, you should see output similar to:

```
==================================================
SYSTEM INFORMATION
==================================================
Platform: Windows-10-10.0.22631-SP0
Processor: Intel64 Family 6 Model 140 Stepping 1,GenuineIntel
Architecture: AMD64
Python Executable: C:\Users\p11x\...\python.exe

==================================================
SYSTEM DEPENDENCIES
==================================================
✓ pip installed: 24.0
==================================================
TENSORFLOW VERIFICATION
==================================================
TensorFlow Version: 2.16.1

Build Info:
  - Compiled with CUDA: True
  - GPU Support: True

Available Devices:
  - GPUs: 1
    * PhysicalDevice(name='/GPU:0', device_type='GPU')
  - CPUs: 1

==================================================
BASIC TENSORFLOW OPERATIONS TEST
==================================================
Test 1 - Create Tensor:
  Input: tf.constant([[1, 2, 3], [4, 5, 6]])
  Output:
  [[1 2 3]
   [4 5 6]]
  ✓ Tensor created successfully

Test 2 - Add Operations:
  Input: tf.add([1,2,3], [4,5,6])
  Output: [5. 7. 9.]
  ✓ Math operations work
...
```

For the MNIST example:
```
Epoch 1/5
1688/1688 [======] - loss: 0.2345 - accuracy: 0.9234 - val_loss: 0.1234 - val_accuracy: 0.9654
...
Test Results:
  Loss: 0.0891
  Accuracy: 0.9743
```

## VI. VISUALIZATION

### TensorFlow Architecture Flow Chart

```
TENSORFLOW DEEP LEARNING WORKFLOW
================================

┌─────────────────────────────────────────────────────────────────────────┐
│                      DATA PREPARATION PHASE                              │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────┐     ┌─────────────┐     ┌─────────────┐              │
│  │ Raw Data   │────▶│   Load     │────▶│  Preprocess │              │
│  │ (Files,    │     │   Data     │     │  (Normalize,│              │
│  │  DB, etc.) │     │  (tf.data) │     │   Augment)  │              │
│  └─────────────┘     └─────────────┘     └─────────────┘              │
│                                            │                        │
│                                            ▼                        │
│                                    ┌─────────────┐                   │
│                                    │  Dataset   │                   │
│                                    │  (Train/   │                   │
│                                    │   Val/Test)│                   │
│                                    └─────────────┘                   │
└─────────────────────────────────────────────────────────────────────────┘
                                            │
                                            ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                      MODEL BUILDING PHASE                              │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                    MODEL ARCHITECTURE                          │   │
│  │                                                                  │   │
│  │  ┌──────────────────────────────────────────────────────────┐ │   │
│  │  │  Input Layer                                            │ │   │
│  │  │  (shape=(28,28,1))                                      │ │   │
│  │  └──────────────────────────────────────────────────────────┘ │   │
│  │                            │                                  │   │
│  │                            ▼                                  │   │
│  │  ┌──────────────────────────────────────────────────────────┐ │   │
│  │  │  Hidden Layers (Stacked)                                 │ │   │
│  │  │  ┌───────────┐ ┌───────────┐ ┌───────────┐              │ │   │
│  │  │  │ Conv2D    │ │Dense + BN │ │  Dropout  │              │ │   │
│  │  │  │ MaxPool   │ │ + ReLU    │ │   (0.3)   │              │ │   │
│  │  │  └───────────┘ └───────────┘ └───────────┘              │ │   │
│  │  │         │             │             │                  │ │   │
│  │  │         └─────────────┴─────────────┘                  │ │   │
│  │  └──────────────────────────────────────────────────────────┘ │   │
│  │                            │                                  │   │
│  │                            ▼                                  │   │
│  │  ┌──────────────────────────────────────────────────────────┐ │   │
│  │  │  Output Layer                                             │ │   │
│  │  │  Dense(num_classes, activation='softmax')                │ │   │
│  │  └──────────────────────────────────────────────────────────┘ │   │
│  │                                                                  │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└───────────────────────────────────────���─���───────────────────────────────┘
                                            │
                                            ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                      TRAINING PHASE                                   │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                    TRAINING LOOP                                │   │
│  │                                                                  │   │
│  │  for epoch in range(num_epochs):                                │   │
│  │       │                                                         │   │
│  │       ▼                                                         │   │
│  │  ┌──────────────┐                                              │   │
│  │  │ Forward Pass │                                              │   │
│  │  │  predictions │                                              │   │
│  │  └──────────────┘                                              │   │
│  │       │                                                         │   │
│  │       ▼                                                         │   │
│  │  ┌──────────────┐                                              │   │
│  │  │ Compute     │                                              │   │
│  │  │ Loss        │                                              │   │
│  │  └──────────────┘                                              │   │
│  │       │                                                         │   │
│  │       ▼                                                         │   │
│  │  ┌──────────────┐                                              │   │
│  │  │ Backward    │                                              │   │
│  │  │ Pass        │                                              │   │
│  │  │ (Gradients) │                                              │   │
│  │  └──────────────┘                                              │   │
│  │       │                                                         │   │
│  │       ▼                                                         │   │
│  │  ┌──────────────┐                                              │   │
│  │  │ Update      │                                              │   │
│  │  │ Weights     │                                              │   │
│  │  └──────────────┘                                              │   │
│  │       │                                                         │   │
│  │       ▼                                                         │   │
│  │  ┌──────────────┐                                              │   │
│  │  │ Evaluate    │                                              │   │
│  │  │ Metrics     │                                              │   │
│  │  └──────────────┘                                              │   │
│  │                                                                  │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
                                            │
                                            ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                      INFERENCE PHASE                                  │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────┐     ┌─────────────┐     ┌─────────────┐            │
│  │ New Input   │────▶│ Load Saved  │────▶│   Forward   │            │
│  │ Data       │     │   Model     │     │   Pass       │            │
│  └─────────────┘     └─────────────┘     └─────────────┘            │
│                                             │                         │
│                                             ▼                         │
│                                    ┌─────────────┐                   │
│                                    │ Prediction │                   │
│                                    │ (Class/    │                   │
│                                    │  Prob)     │                   │
│                                    └─────────────┘                   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

## VII. ADVANCED_TOPICS

### Advanced Topic 1: Custom Training Loop

```python
"""
Advanced Topic 1: Custom Training Loop
For fine-grained control over the training process.
"""

import tensorflow as tf
from tensorflow import keras
import numpy as np

def custom_training_loop():
    """
    Implement a custom training loop with:
    - Gradient computation with GradientTape
    - Custom optimizers
    - Learning rate scheduling
    - Gradient clipping
    """
    
    # Create simple model
    model = keras.Sequential([
        keras.layers.Dense(64, activation='relu', input_shape=(784,)),
        keras.layers.Dense(64, activation='relu'),
        keras.layers.Dense(10)
    ])
    
    # Custom optimizer with gradient clipping
    optimizer = keras.optimizers.Adam(learning_rate=0.001)
    
    # Loss function
    loss_fn = keras.losses.CategoricalCrossentropy(from_logits=True)
    
    # Metrics
    train_loss = keras.metrics.Mean(name='train_loss')
    train_accuracy = keras.metrics.CategoricalAccuracy(name='train_accuracy')
    
    # Generate sample data
    x_train = np.random.randn(1000, 784).astype('float32')
    y_train = np.random.randint(0, 10, 1000)
    y_train = keras.utils.to_categorical(y_train, 10)
    
    # Custom training step
    @tf.function
    def train_step(inputs, targets):
        """Single training step."""
        with tf.GradientTape() as tape:
            predictions = model(inputs, training=True)
            loss = loss_fn(targets, predictions)
        
        gradients = tape.gradient(loss, model.trainable_variables)
        
        # Gradient clipping (prevent exploding gradients)
        gradients, _ = tf.clip_by_global_norm(gradients, norm=1.0)
        
        optimizer.apply_gradients(zip(gradients, model.trainable_variables))
        
        train_loss(loss)
        train_accuracy(targets, predictions)
        
        return loss
    
    # Training loop
    print("Custom Training Loop Example:")
    print("="*50)
    
    batch_size = 32
    num_epochs = 3
    
    for epoch in range(num_epochs):
        # Reset metrics
        train_loss.reset_state()
        train_accuracy.reset_state()
        
        # Train over batches
        num_batches = len(x_train) // batch_size
        
        for batch in range(num_batches):
            start = batch * batch_size
            end = start + batch_size
            
            x_batch = x_train[start:end]
            y_batch = y_train[start:end]
            
            loss = train_step(x_batch, y_batch)
        
        print(f"Epoch {epoch+1}: Loss={train_loss.result():.4f}, "
              f"Accuracy={train_accuracy.result():.4f}")

def distributed_training():
    """
    Distributed training across multiple GPUs.
    """
    
    # Strategy for multi-GPU training
    strategy = tf.distribute.MirroredStrategy()
    
    print(f"Number of devices: {strategy.num_replicas_in_sync}")
    
    with strategy.scope():
        # Model created within strategy scope
        model = keras.Sequential([
            keras.layers.Dense(64, activation='relu', input_shape=(784,)),
            keras.layers.Dense(10, activation='softmax')
        ])
        
        model.compile(
            optimizer='adam',
            loss='categorical_crossentropy',
            metrics=['accuracy']
        )

if __name__ == "__main__":
    custom_training_loop()
```

### Advanced Topic 2: TensorFlow Lite for Mobile

```python
"""
Advanced Topic 2: TensorFlow Lite for Mobile Deployment
Convert models for mobile and edge devices.
"""

import tensorflow as tf

def convert_to_tflite():
    """
    Convert Keras model to TensorFlow Lite format.
    """
    # Create and train a simple model
    model = tf.keras.Sequential([
        tf.keras.layers.Dense(64, activation='relu', input_shape=(784,)),
        tf.keras.layers.Dense(10, activation='softmax')
    ])
    
    model.compile(
        optimizer='adam',
        loss='categorical_crossentropy',
        metrics=['accuracy']
    )
    
    # Convert to TFLite
    converter = tf.lite.TFLiteConverter.from_keras_model(model)
    
    # Optimize for size
    converter.optimizations = [tf.lite.Optimize.DEFAULT]
    
    # Quantize weights
    converter.target_spec.supported_types = [tf.float16]
    
    tflite_model = converter.convert()
    
    print(f"TFLite model size: {len(tflite_model)} bytes")
    
    return tflite_model

def tflite_inference():
    """
    Run inference with TensorFlow Lite interpreter.
    """
    # Create interpreter
    interpreter = tf.lite.Interpreter(model_content=convert_to_tflite())
    interpreter.allocate_tensors()
    
    # Get input/output details
    input_details = interpreter.get_input_details()
    output_details = interpreter.get_output_details()
    
    # Run inference
    test_input = tf.random.normal([1, 784])
    interpreter.set_tensor(
        input_details[0]['index'], 
        test_input.numpy()
    )
    interpreter.invoke()
    
    output = interpreter.get_tensor(output_details[0]['index'])
    print(f"Prediction shape: {output.shape}")

if __name__ == "__main__":
    convert_to_tflite()
    tflite_inference()
```

### Advanced Topic 3: Custom Layers and Models

```python
"""
Advanced Topic 3: Custom Layers and Models
Create reusable custom components.
"""

import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers

class CustomDenseLayer(layers.Layer):
    """
    Custom dense layer with optional residual connection.
    """
    
    def __init__(self, units, activation='relu', use_residual=False, **kwargs):
        super().__init__(**kwargs)
        self.units = units
        self.activation = activation
        self.use_residual = use_residual
        
    def build(self, input_shape):
        self.kernel = self.add_weight(
            name='kernel',
            shape=(input_shape[-1], self.units),
            initializer='glorot_uniform',
            trainable=True
        )
        self.bias = self.add_weight(
            name='bias',
            shape=(self.units,),
            initializer='zeros',
            trainable=True
        )
        
    def call(self, inputs, training=None):
        output = tf.matmul(inputs, self.kernel) + self.bias
        
        if self.activation:
            output = tf.keras.activations.get(self.activation)(output)
        
        # Optional residual connection
        if self.use_residual and inputs.shape[-1] == self.units:
            output = output + inputs
        
        return output
    
    def get_config(self):
        config = super().get_config()
        config.update({
            'units': self.units,
            'activation': self.activation,
            'use_residual': self.use_residual
        })
        return config

class CustomModel(keras.Model):
    """
    Custom model with built-in training logic.
    """
    
    def __init__(self, num_classes, **kwargs):
        super().__init__(**kwargs)
        self.dense1 = CustomDenseLayer(64, activation='relu')
        self.dense2 = CustomDenseLayer(64, activation='relu')
        self.classifier = CustomDenseLayer(num_classes, activation='softmax')
        
    def call(self, inputs, training=None):
        x = self.dense1(inputs)
        x = self.dense2(x)
        return self.classifier(x)
    
    def train_step(self, data):
        """Custom training step."""
        inputs, targets = data
        
        with tf.GradientTape() as tape:
            predictions = self(inputs, training=True)
            loss = self.compiled_loss(targets, predictions)
        
        gradients = tape.gradient(loss, self.trainable_variables)
        self.optimizer.apply_gradients(
            zip(gradients, self.trainable_variables)
        )
        
        return {'loss': loss}

if __name__ == "__main__":
    print("Custom Layers and Models Example")
    print("="*50)
    
    # Test custom layer
    custom_layer = CustomDenseLayer(32)
    print(f"Created custom layer: {custom_layer}")
    
    # Test custom model
    custom_model = CustomModel(10)
    print(f"Created custom model: {custom_model}")
```

### Common Pitfalls and Solutions

| Pitfall | Cause | Solution |
|--------|-------|----------|
| OOM Errors | GPU memory insufficient | Use `tf.config.experimental.set_memory_growth()`, reduce batch size |
| Non-deterministic results | Random seeds not set | Set all random seeds, enable determinism |
| Slow training | CPU bottleneck | Use `tf.data.AUTOTUNE`, enable GPU acceleration |
| Poor generalization | Overfitting | Use dropout, regularization, more data |
| NaN losses | Exploding gradients | Gradient clipping, reduce learning rate |
| Model not saving | File permission issues | Check directory permissions |

## VIII. CONCLUSION

### Key Takeaways

1. **TensorFlow Installation**: Use pip in a virtual environment for clean setup
   ```bash
   python -m venv tf_env
   pip install tensorflow
   ```

2. **Core Concepts**: Understand tensors, computational graphs, and the Keras API

3. **Best Practices**:
   - Use Keras for rapid development
   - Use tf.data for efficient data pipelines
   - Use TensorBoard for visualization
   - Set random seeds for reproducibility

4. **Production Readiness**: 
   - Convert models to TensorFlow Lite for deployment
   - Use SavedModel format for export
   - Implement proper error handling

5. **GPU Configuration**: Enable memory growth to avoid OOM errors

### Next Steps

1. Explore TensorBoard for training visualization
2. Learn about transfer learning with pre-trained models
3. Study advanced architectures (CNNs, RNNs, Transformers)
4. Practice with real-world datasets
5. Learn model deployment with TensorFlow Serving

### Further Reading

- TensorFlow Official Documentation: https://www.tensorflow.org/guide
- Keras Documentation: https://keras.io/getting_started/
- "Hands-On Machine Learning with Scikit-Learn, Keras, and TensorFlow" by Aurélien Géron
- TensorFlow Blog: https://blog.tensorflow.org
- Machine Learning Crash Course: https://developers.google.com/machine-learning/crash-course

End of TensorFlow Installation and Setup Tutorial