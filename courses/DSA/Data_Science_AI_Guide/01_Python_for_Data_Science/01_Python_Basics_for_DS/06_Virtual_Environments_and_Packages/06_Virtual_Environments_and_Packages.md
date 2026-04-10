# Virtual Environments and Packages

## Introduction

Virtual environments and package management are essential skills for data science projects. They ensure reproducibility, prevent conflicts between project dependencies, and make it easy to share projects with others. This guide covers creating and managing virtual environments, installing packages from PyPI, and best practices for data science development.

Python packages are pre-written libraries that provide additional functionality. The Python Package Index (PyPI) hosts thousands of packages including pandas, numpy, scikit-learn, and TensorFlow. However, different projects may require different package versions, which can lead to conflicts.

Virtual environments solve this problem by creating isolated Python environments for each project. This allows you to have different package versions for different projects without conflicts.

This guide covers virtual environment creation using venv and conda, package installation using pip and conda, and best practices for managing dependencies.

## Fundamentals

### Virtual Environments with venv

Python's built-in venv module provides virtual environment support. It's simple to use and doesn't require additional installation.

```bash
# Create virtual environment
python -m venv myenv

# Activate (Windows)
myenv\Scripts\activate

# Activate (Linux/Mac)
source myenv/bin/activate

# Install packages
pip install numpy pandas

# Freeze requirements
pip freeze > requirements.txt

# Deactivate
deactivate
```

### Virtual Environments with Conda

Conda is a powerful package and environment manager that comes with Anaconda. It's popular in data science for its ease of use and robust dependency resolution.

```bash
# Create environment
conda create -n myenv python=3.10

# Activate
conda activate myenv

# Install packages
conda install numpy pandas scikit-learn

# Install from conda-forge
conda install -c conda-forge xgboost

# List environments
conda env list

# Export environment
conda env export > environment.yml
```

### Package Installation with pip

pip is Python's package installer. It downloads packages from PyPI and handles dependencies.

```bash
# Install package
pip install numpy

# Install specific version
pip install numpy==1.24.0

# Install from requirements
pip install -r requirements.txt

# Upgrade package
pip install --upgrade numpy

# Show installed packages
pip list

# Check for outdated
pip list --outdated
```

### Requirements Files

Requirements files list all packages and their versions for a project. They enable reproducible environments.

```text
# requirements.txt
numpy==1.24.0
pandas==2.0.0
scikit-learn==1.2.0
matplotlib==3.7.0
seaborn==0.12.0
```

## Implementation

### Banking: Setting Up Data Science Environment

```bash
# Create project directory
mkdir banking_project
cd banking_project

# Create virtual environment
python -m venv venv

# Activate
# Windows: venv\Scripts\activate
# Linux/Mac: source venv/bin/activate

# Install core data science packages
pip install numpy pandas matplotlib seaborn

# Install banking-specific packages
pip install pandas-datareader yfinance

# Install ML libraries
pip install scikit-learn xgboost lightgbm

# Install for feature engineering
pip install category-encoders sklearn-pandas

# Save requirements
pip freeze > requirements.txt
```

### Banking: Package Imports and Verification

```python
# Verify installed packages
try:
    import numpy
    print(f"NumPy version: {numpy.__version__}")
except ImportError:
    print("NumPy not installed")

try:
    import pandas
    print(f"Pandas version: {pandas.__version__}")
except ImportError:
    print("Pandas not installed")

try:
    import sklearn
    print(f"Scikit-learn version: {sklearn.__version__}")
except ImportError:
    print("Scikit-learn not installed")

try:
    import yfinance
    print("yFinance installed")
except ImportError:
    print("yFinance not installed")
```

### Healthcare: Setting Up ML Environment

```bash
# Create healthcare ML environment
conda create -n healthcare_ml python=3.10 -y

# Activate
conda activate healthcare_ml

# Install core ML packages
conda install numpy pandas scikit-learn matplotlib -y

# Install medical imaging packages
conda install pydicom SimpleITK -y

# Install deep learning
conda install tensorflow keras -y

# Install visualization
conda install seaborn plotly -y

# Create requirements file
conda env export > environment.yml
```

### Healthcare: Package Configuration

```python
# Healthcare ML package configuration

# Core packages required
REQUIRED_PACKAGES = {
    "numpy": ">=1.20",
    "pandas": ">=1.3",
    "scikit-learn": ">=1.0",
    "matplotlib": ">=3.5",
    "seaborn": ">=0.11",
}

# Optional packages
OPTIONAL_PACKAGES = {
    "tensorflow": ">=2.6",
    "pydicom": ">=2.0",
    "SimpleITK": ">=2.0",
    "plotly": ">=5.0",
}

def check_requirements():
    """Check if required packages are installed"""
    missing = []
    
    for package, version in REQUIRED_PACKAGES.items():
        try:
            __import__(package)
        except ImportError:
            missing.append(f"{package}{version}")
    
    if missing:
        print(f"Missing packages: {', '.join(missing)}")
        return False
    
    print("All required packages installed")
    return True

def get_package_versions():
    """Get versions of installed packages"""
    packages = [
        "numpy", "pandas", "sklearn",
        "matplotlib", "seaborn", "scipy"
    ]
    
    versions = {}
    for package in packages:
        try:
            mod = __import__(package)
            versions[package] = mod.__version__
        except ImportError:
            versions[package] = "Not installed"
    
    return versions
```

### Data Science: Package Management

```bash
# Upgrade all packages
pip freeze > current_requirements.txt
pip install -r current_requirements.txt --upgrade

# Check for package conflicts
pip check

# Uninstall packages
pip uninstall package_name

# Clean up unused packages
pip autoclean
```

## Applications in Banking

### Banking: Environment Setup Script

```bash
#!/bin/bash
# setup_banking_env.sh

# Create environment
python -m venv banking_env

# Activate
source banking_env/bin/activate

# Update pip
pip install --upgrade pip

# Install data processing
pip install numpy pandas matplotlib seaborn

# Install financial analysis
pip install yfinance pandas-datareader

# Install ML
pip install scikit-learn xgboost lightgbm

# Install feature engineering
pip install category-encoders tsfresh

# Install visualization
pip install plotly kaleido

# Save requirements
pip freeze > requirements.txt

echo "Environment setup complete"
```

### Banking: Requirements Configuration

```python
# requirements.txt for banking project

# Core data science
numpy==1.24.0
pandas==2.0.0
matplotlib==3.7.0
seaborn==0.12.0

# Financial data
yfinance==0.2.28
pandas-datareader==0.10.0

# Machine learning
scikit-learn==1.2.0
xgboost==1.7.0
lightgbm==3.3.0

# Feature engineering
category-encoders==2.5.0
tsfresh==1.4.0

# Visualization
plotly==5.13.0
kaleido==0.2.1
```

### Banking: Conda Environment

```yaml
# environment.yml
name: banking_ml
channels:
  - defaults
  - conda-forge
dependencies:
  - python=3.10
  - numpy=1.24
  - pandas=2.0
  - matplotlib=3.7
  - scikit-learn=1.2
  - xgboost=1.7
  - pip
  - pip:
    - yfinance==0.2.28
    - pandas-datareader==0.10.0
```

## Applications in Healthcare

### Healthcare: Environment Setup

```bash
#!/bin/bash
# setup_healthcare_env.sh

# Create conda environment
conda create -n healthcare_ml python=3.10 -y

# Activate
conda activate healthcare_ml

# Install core packages
conda install -c conda-forge \
    numpy \
    pandas \
    scikit-learn \
    matplotlib \
    seaborn \
    scipy \
    -y

# Install medical imaging
conda install -c conda-forge \
    pydicom \
    SimpleITK \
    -y

# Install deep learning
conda install -c conda-forge \
    tensorflow \
    pytorch \
    -y

# Install visualization
conda install -c conda-forge \
    plotly \
    -y

# Export environment
conda env export > environment.yml
```

### Healthcare: Package Requirements

```python
# requirements_healthcare.txt

# Medical imaging
SimpleITK==2.2.0
pydicom==2.3.0

# Deep learning
tensorflow==2.12.0
torch==2.0.0

# Data processing
numpy==1.24.0
pandas==2.0.0
scipy==1.10.0

# ML
scikit-learn==1.2.0
xgboost==1.7.0

# Visualization
matplotlib==3.7.0
seaborn==0.12.0
plotly==5.13.0

# Utilities
ipython==8.12.0
jupyter==1.0.0
```

### Healthcare: Conda Configuration

```yaml
# environment_healthcare.yml
name: healthcare_ml
channels:
  - defaults
  - conda-forge
  - pytorch
dependencies:
  - python=3.10
  - numpy=1.24
  - pandas=2.0
  - scipy=1.10
  - scikit-learn=1.2
  - matplotlib=3.7
  - seaborn=0.12
  - pytorch::pytorch=2.0.0
  - pydicom=2.3
  - SimpleITK=2.2
```

## Output Results

### Sample Output: Package Installation

```
# Creating virtual environment
$ python -m venv myenv

# Activating
$ source myenv/bin/activate
(myenv) $ pip install numpy pandas

# Installation output
Collecting numpy
  Using cached numpy-1.24.0-cp310-cp310-manylinux_2_17_x86_64.whl
Collecting pandas
  Using cached pandas-2.0.0-cp310-cp310-cp310-linux_x86_64.whl
Installing collected packages: numpy, pandas
Successfully installed numpy-1.24.0 pandas-2.0.0

# Verifying installation
(myenv) $ pip list
Package    Version
---------- -------
numpy     1.24.0
pandas    2.0.0
pip       23.0
setuptools 67.0

# Freezing requirements
(myenv) $ pip freeze > requirements.txt
```

### Sample Output: Conda Environment

```
# Creating conda environment
$ conda create -n data_science python=3.10 -y

# Installing packages
$ conda install -c conda-forge numpy pandas scikit-learn -y

# Environment info
$ conda info
     active environment : data_science
    active config file :
           conda version : 4.14.0

# List packages
$ conda list
# packages in environment at /opt/anaconda3/envs/data_science:
numpy                1.24.0         numpy
pandas              2.0.0          conda-forge
scikit-learn        1.2.0          conda-forge
```

## Visualization

### ASCII: Virtual Environment Architecture

```
+====================================================================+
|                VIRTUAL ENVIRONMENT ARCHITECTURE                        |
+====================================================================+
|                                                                      |
|  SYSTEM PYTHON                                                     |
|  ==============                                                  |
|  /usr/bin/python3.10                                             |
|  /usr/lib/python3.10/site-packages                             |
|       |                                                           |
|       v                                                           |
|  +--------------------------------------------------------+      |
|  |  PROJECT A ENVIRONMENT                                  |      |
|  |  /home/user/projects/project_a/venv                   |      |
|  |  +----------------------------------------------+    |      |
|  |  | project_a/                                   |    |      |
|  |  | lib/python3.10/site-packages/               |    |      |
|  |  |   numpy-1.24.0/                          |    |      |
|  |  |   pandas-2.0.0/                           |    |      |
|  |  |   sklearn-1.2.0/                          |    |      |
|  |  +----------------------------------------------+    |      |
|  +--------------------------------------------------------+      |
|       |                                                           |
|       v                                                           |
|  +--------------------------------------------------------+      |
|  |  PROJECT B ENVIRONMENT                                  |      |
|  |  /home/user/projects/project_b/venv                   |      |
|  |  +----------------------------------------------+    |      |
|  |  | project_b/                                   |    |      |
|  |  | lib/python3.10/site-packages/               |    |      |
|  |  |   numpy-1.23.0/                          |    |      |
|  |  |   pandas-1.5.0/                           |    |      |
|  |  |   tensorflow-2.11.0/                      |    |      |
|  |  +----------------------------------------------+    |      |
|  +--------------------------------------------------------+      |
|                                                                      |
|  BENEFITS:                                                        |
|  - Different package versions per project                          |
|  - No conflicts between projects                              |
|  - Reproducible environments                                  |
|  - Easy to share and deploy                                |
|                                                                      |
+====================================================================+
```

### ASCII: Package Dependency Flow

```
+====================================================================+
|                  PACKAGE DEPENDENCY FLOW                           |
+====================================================================+
|                                                                      |
|  PYPI (Python Package Index)                                        |
|  =============================                                      |
|                                                                      |
|       +---------+                                                  |
|       | Package |                                                  |
|       |Request |                                                  |
|       +----+--+                                                  |
|            |                                                     |
|            v                                                     |
|  +----------------+          +----------------+                    |
|  | pip install   |-------->│  Download     |                    |
|  | package_name |          │  package.tar  |                    |
|  +----------------+          +-------+------+                    |
|                                      |                            |
|                                      v                            |
|                              +----------------+                    |
|                              │ Extract tar   |                    |
|                              +-------+------+                    |
|                                      |                            |
|                                      v                            |
|                              +----------------+                    |
|                              │  Install to   |                    |
|                              │ venv/site-   |                    |
|                              │ packages/   |                    |
|                              +----------------+                    |
|                                                                      |
|  DEPENDENCY RESOLUTION                                             |
|  =====================                                            |
|                                                                      |
|  numpy==1.24.0                                                  |
|       |                                                           |
|       +--> scipy>=1.8                                            |
|       +--> pandas>=1.3                                            |
|       +--> matplotlib>=3.5                                        |
|                                                                      |
|  INSTALLATION ORDER:                                              |
|  1. scipy (installed first)                                       |
|  2. pandas (installed second)                                     |
|  3. matplotlib (installed third)                                  |
|  4. numpy (installed last)                                        |
|                                                                      |
+====================================================================+
```

### ASCII: Conda vs Pip Comparison

```
+====================================================================+
|              CONDA VS PIP COMPARISON                               |
+====================================================================+
|                                                                      |
|  CONDA                                                            |
|  =====                                                            |
|  + Binary packages (faster)                                        |
|  + Dependency resolver (C++)                                      |
|  + Works with non-Python packages                                   |
|  + Environment management                                         |
|  + Anaconda distribution                                           |
|  - Larger installer                                                |
|  - Some packages not available                                     |
|                                                                      |
|  PIP                                                              |
|  ===                                                             |
|  + PyPI has more packages                                          |
|  + Smaller installer                                             |
|  + Easy to use                                                  |
|  - Pure Python packages only                                       |
|  - Slower dependency resolution                                  |
|                                                                      |
|  RECOMMENDATION:                                                  |
|  =============                                                   |
|                    USE BOTH                                       |
|                                                                      |
|  +--------------------------------------------------------+      |
|  |  conda create -n myenv python=3.10                       |      |
|  |  conda activate myenv                                   |      |
|  |  conda install numpy pandas ...                        |      |
|  +--------------------------------------------------------+      |
|       |                                                           |
|       v                                                           |
|  +--------------------------------------------------------+      |
|  |  pip install special-package-not-in-conda              |      |
|  +--------------------------------------------------------+      |
|                                                                      |
+====================================================================+
```

## Best Practices

### Project Setup Best Practices

```bash
# 1. Create project directory
mkdir my_project
cd my_project

# 2. Initialize git
git init

# 3. Create .gitignore
echo "venv/" > .gitignore
echo "__pycache__/" >> .gitignore
echo "*.pyc" >> .gitignore
echo ".env" >> .gitignore

# 4. Create README
echo "# My Project" > README.md

# 5. Create virtual environment
python -m venv venv

# 6. Activate and install packages
source venv/bin/activate
pip install numpy pandas

# 7. Create requirements.txt
pip freeze > requirements.txt
```

### Environment Management

```bash
# Check for outdated packages
pip list --outdated

# Upgrade packages
pip install --upgrade pip setuptools wheel

# Upgrade specific package
pip install --upgrade package_name

# Create clean environment
python -m venv new_env
source new_env/bin/activate
pip install -r requirements.txt

# Use pip-tools for better dependency management
pip install pip-tools
pip-compile requirements.in
```

### Sharing Environments

```bash
# Export with pip
pip freeze > requirements.txt

# Export with conda
conda env export > environment.yml

# Import from requirements.txt
pip install -r requirements.txt

# Import from conda
conda env create -f environment.yml
```

## Advanced Topics

### Advanced: Docker for Data Science

```dockerfile
# Dockerfile for data science environment
FROM python:3.10-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

CMD ["python", "main.py"]
```

```python
# requirements.txt for Docker
numpy==1.24.0
pandas==2.0.0
scikit-learn==1.2.0
flask==2.3.0
```

### Advanced: pip-tools

```bash
# requirements.in (user's input)
numpy
pandas>=1.3
scikit-learn

# Compile to requirements.txt
pip-compile requirements.in

# Or use pip-tools with constraints
pip-compile requirements.in --output-file=requirements.txt
```

### Advanced: Poetry

```bash
# Install poetry
curl -sSL https://install.python-poetry.org | python3 -

# Initialize project
poetry new banking_project

# Add dependencies
poetry add numpy pandas scikit-learn

# Install
poetry install

# Export requirements
poetry export -f requirements.txt --output requirements.txt
```

```toml
# pyproject.toml
[tool.poetry]
name = "banking-project"
version = "0.1.0"
description = "Banking ML project"
authors = ["Developer"]

[tool.poetry.dependencies]
python = "^3.10"
numpy = "^1.24"
pandas = "^2.0"

[tool.poetry.dev-dependencies]
pytest = "^7.0"
```

## Conclusion

Virtual environments and package management are fundamental skills for data science projects. This guide covered creating and managing virtual environments with venv and conda, installing packages, and best practices for data science development.

Key takeaways include:
- **Virtual environments** - Create isolated Python environments for each project
- **Package installation** - Use pip or conda to install packages
- **Requirements files** - Share environments using requirements.txt or environment.yml
- **Best practices** - Use .gitignore, clean setups, and proper sharing methods

The banking and healthcare applications demonstrated setting up data science and machine learning environments with appropriate packages. Docker and pip-tools provide advanced options for containerization and dependency management.

You have now completed the Python Basics for Data Science module. Continue to explore more advanced topics in data science including machine learning, deep learning, and specialized domains.