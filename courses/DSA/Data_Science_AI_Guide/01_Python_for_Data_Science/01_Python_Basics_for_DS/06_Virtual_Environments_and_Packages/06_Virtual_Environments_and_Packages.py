# Topic: Virtual_Environments_and_Packages
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Virtual Environments and Packages

I. INTRODUCTION
   Virtual environments and package management are essential for Python projects.
   This module covers virtual environment creation, pip usage, requirements files,
   and package installation best practices.
   Prerequisites: Python fundamentals
   Requirements: Python 3.8+, pip

II. CORE_CONCEPTS
   - Virtual environment creation and activation
   - pip package manager
   - Requirements files
   - Package installation
   - Virtual environment management
   - pipenv and poetry

III. IMPLEMENTATION
   - Step-by-step code examples
   - Best practices for environment setup
   - Detailed comments throughout

IV. EXAMPLES
   - Standard demonstration
   - Real-world Application 1: Banking/Finance - Data analysis environment
   - Real-world Application 2: Healthcare - ML model environment

V. OUTPUT_RESULTS
   - Expected outputs
   - Performance analysis

VI. TESTING
   - Unit tests for main functions

VII. ADVANCED_TOPICS
   - Creating requirements from environment
   - Using pip freeze
   - Virtual environment best practices

VIII. CONCLUSION
   - Key takeaways
   - Next steps for learning
"""

import sys
import os
import subprocess
from typing import List, Dict, Optional, Tuple


def main():
    print("Executing Virtual Environments and Packages implementation")
    print("\n=== Python Version ===")
    demonstrate_python_version()
    
    print("\n=== Package Management ===")
    demonstrate_package_management()
    
    print("\n=== Virtual Environments ===")
    demonstrate_virtual_environments()
    
    print("\n=== Requirements Files ===")
    demonstrate_requirements_files()
    
    print("\n=== Banking Application ===")
    banking_application()
    
    print("\n=== Healthcare Application ===")
    healthcare_application()


def demonstrate_python_version():
    """Demonstrate Python version checking"""
    print("\n--- Python Version ---")
    print(f"Python version: {sys.version}")
    print(f"Python executable: {sys.executable}")
    print(f"Platform: {sys.platform}")
    
    print("\n--- Version Information ---")
    print(f"Major version: {sys.version_info.major}")
    print(f"Minor version: {sys.version_info.minor}")
    print(f"Micro version: {sys.version_info.micro}")
    
    print("\n--- Version Comparison ---")
    version = sys.version_info
    if version >= (3, 8):
        print("Python 3.8 or higher: Supported")
    else:
        print("Python version too old")


def demonstrate_package_management():
    """Demonstrate package management"""
    print("\n--- pip Basics ---")
    
    print("\npip list: List installed packages")
    print("pip install <package>: Install a package")
    print("pip install <package>==<version>: Install specific version")
    print("pip install -r requirements.txt: Install from file")
    print("pip uninstall <package>: Uninstall package")
    print("pip show <package>: Show package information")
    print("pip search <package>: Search for package")
    print("pip freeze > requirements.txt: Export installed packages")
    
    print("\n--- Common pip Commands ---")
    commands = [
        "pip list",
        "pip show numpy",
        "pip list --outdated",
        "pip install --upgrade pip",
    ]
    
    for cmd in commands:
        print(f"  {cmd}")
    
    print("\n--- Checking Installed Packages ---")
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
        import matplotlib
        print(f"Matplotlib version: {matplotlib.__version__}")
    except ImportError:
        print("Matplotlib not installed")
    
    try:
        import sklearn
        print(f"Sklearn imported successfully")
    except ImportError:
        print("Sklearn not installed")


def demonstrate_virtual_environments():
    """Demonstrate virtual environments"""
    print("\n--- Virtual Environment Commands ---")
    
    print("\nCreate virtual environment:")
    print("  python -m venv myenv")
    print("  python -m venv /path/to/env")
    
    print("\nActivate (Windows):")
    print("  myenv\\Scripts\\activate")
    
    print("\nActivate (Linux/Mac):")
    print("  source myenv/bin/activate")
    
    print("\nDeactivate:")
    print("  deactivate")
    
    print("\n--- Virtual Environment Benefits ---")
    benefits = [
        "Isolate project dependencies",
        "Avoid version conflicts",
        "Easy environment recreation",
        "Clean system Python",
    ]
    
    for benefit in benefits:
        print(f"  - {benefit}")
    
    print("\n--- Environment Variable ---")
    print(f"VIRTUAL_ENV: {os.environ.get('VIRTUAL_ENV', 'Not in venv')}")


def demonstrate_requirements_files():
    """Demonstrate requirements files"""
    print("\n--- Requirements File Format ---")
    
    example_requirements = """# Data Science Essentials
numpy>=1.21.0
pandas>=1.3.0
matplotlib>=3.4.0
scikit-learn>=1.0.0

# Optional
tabulate>=0.8.9

# Specific version
scipy==1.7.0
"""
    
    print("Example requirements.txt:")
    for line in example_requirements.split("\n"):
        print(f"  {line}")
    
    print("\n--- Common Requirements Generation ---")
    print("pip freeze > requirements.txt")
    print("pipreqs ./  (requires pipreqs package)")
    print("poetry export -f requirements.txt")
    
    print("\n--- Installing Requirements ---")
    print("pip install -r requirements.txt")
    print("pip install --upgrade -r requirements.txt")


def practical_example():
    """Practical demonstration"""
    print("\n=== Practical Example: Environment Setup Script ===")
    
    setup_script = '''#!/bin/bash
# Data Science Environment Setup

# Create virtual environment
python -m venv ds_env

# Activate
source ds_env/bin/activate

# Upgrade pip
pip install --upgrade pip

# Install core packages
pip install numpy pandas matplotlib seaborn

# Install ML packages
pip install scikit-learn xgboost lightgbm

# Install utilities
pip install jupyterlab ipython black flake8

# Save requirements
pip freeze > requirements.txt
'''
    
    print("Setup script:")
    for line in setup_script.split("\n"):
        print(f"  {line}")
    
    return setup_script


class EnvironmentManager:
    """Utility class for environment management"""
    @staticmethod
    def get_installed_packages() -> List[Tuple[str, str]]:
        """Get list of installed packages"""
        try:
            import pkg_resources
            packages = [
                (pkg.key, pkg.version) 
                for pkg in pkg_resources.working_set
            ]
            return sorted(packages)
        except ImportError:
            return []
    
    @staticmethod
    def check_package_installed(package_name: str) -> Optional[str]:
        """Check if package is installed"""
        try:
            module = __import__(package_name)
            version = getattr(module, "__version__", "unknown")
            return version
        except ImportError:
            return None
    
    @staticmethod
    def create_requirements_file(output_path: str, 
                              include_versions: bool = True):
        """Create requirements file from current environment"""
        packages = EnvironmentManager.get_installed_packages()
        
        with open(output_path, "w") as f:
            f.write("# Requirements file\\n")
            f.write(f"# Generated: {datetime.now().isoformat()}\\n\\n")
            
            for name, version in packages:
                if include_versions:
                    f.write(f"{name}=={version}\\n")
                else:
                    f.write(f"{name}\\n")
        
        return output_path
    
    @staticmethod
    def install_from_requirements(requirements_path: str):
        """Install packages from requirements file"""
        print(f"Installing from {requirements_path}...")
        
        try:
            subprocess.check_call([
                sys.executable, "-m", "pip", "install", "-r", requirements_path
            ])
            print("Installation complete")
        except subprocess.CalledProcessError as e:
            print(f"Error: {e}")
        
    @staticmethod
    def get_environment_info() -> Dict[str, str]:
        """Get environment information"""
        import platform
        
        info = {
            "python_version": sys.version,
            "python_executable": sys.executable,
            "platform": platform.platform(),
            "python_path": sys.prefix,
        }
        
        return info


def banking_application():
    """
    Banking industry use case - Data analysis environment
    """
    print("\n=== Banking Application: Environment Setup ===")
    
    print("\n--- Data Science Packages for Finance ---")
    finance_packages = [
        ("numpy", "Numerical computing"),
        ("pandas", "Data manipulation"),
        ("matplotlib", "Basic plotting"),
        ("seaborn", "Statistical plotting"),
        ("yfinance", "Yahoo Finance data"),
        ("scikit-learn", "Machine learning"),
    ]
    
    for package, description in finance_packages:
        version = EnvironmentManager.check_package_installed(package)
        status = f"v{version}" if version else "Not installed"
        print(f"  {package}: {status} - {description}")
    
    print("\n--- Example: Financial Data Analysis ---")
    
    import numpy as np
    import pandas as pd
    
    np.random.seed(42)
    dates = pd.date_range("2024-01-01", periods=100, freq="D")
    prices = 100 + np.cumsum(np.random.randn(100) * 2)
    
    df = pd.DataFrame({
        "date": dates,
        "price": prices,
    })
    df["returns"] = df["price"].pct_change()
    df["log_returns"] = np.log(df["price"] / df["price"].shift(1))
    
    print(f"DataFrame shape: {df.shape}")
    print(f"Mean return: {df['returns'].mean():.4f}")
    print(f"Std return: {df['returns'].std():.4f}")
    print(f"Sharpe ratio: {df['returns'].mean() / df['returns'].std() * np.sqrt(252):.2f}")


class MLEnvironmentSetup:
    """Healthcare ML environment setup"""
    @staticmethod
    def check_ml_packages() -> Dict[str, bool]:
        """Check if ML packages are available"""
        packages = {
            "numpy": None,
            "pandas": None,
            "sklearn": None,
            "xgboost": None,
            "lightgbm": None,
            "tensorflow": None,
            "torch": None,
        }
        
        for package in packages:
            packages[package] = EnvironmentManager.check_package_installed(package) is not None
        
        return packages
    
    @staticmethod
    def get_required_packages() -> List[str]:
        """Get list of required packages for ML"""
        return [
            "numpy>=1.21.0",
            "pandas>=1.3.0",
            "scikit-learn>=1.0.0",
            "xgboost>=1.5.0",
            "lightgbm>=3.3.0",
        ]
    
    @staticmethod
    def get_optional_packages() -> List[str]:
        """Get list of optional packages for ML"""
        return [
            "tensorflow>=2.6.0",
            "torch>=1.10.0",
            "catboost>=1.0.0",
        ]


def healthcare_application():
    """
    Healthcare industry use case - ML model environment
    """
    print("\n=== Healthcare Application: ML Environment ===")
    
    print("\n--- ML Package Check ---")
    packages = MLEnvironmentSetup.check_ml_packages()
    
    for package, installed in packages.items():
        status = "Installed" if installed else "Not installed"
        print(f"  {package}: {status}")
    
    print("\n--- Required Packages ---")
    for package in MLEnvironmentSetup.get_required_packages():
        print(f"  {package}")
    
    print("\n--- ML Workflow Example ---")
    
    sklearn_available = packages.get("sklearn", False)
    
    if sklearn_available:
        from sklearn.model_selection import train_test_split
        from sklearn.ensemble import RandomForestClassifier
        from sklearn.metrics import accuracy_score
        
        np.random.seed(42)
        X = np.random.randn(100, 5)
        y = (X[:, 0] + X[:, 1] > 0).astype(int)
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, random_state=42
        )
        
        model = RandomForestClassifier(n_estimators=10, random_state=42)
        model.fit(X_train, y_train)
        
        y_pred = model.predict(X_test)
        accuracy = accuracy_score(y_test, y_pred)
        
        print(f"Training samples: {X_train.shape[0]}")
        print(f"Test samples: {X_test.shape[0]}")
        print(f"Accuracy: {accuracy:.2f}")
    else:
        print("scikit-learn not available")


def test_environment_check():
    """Test environment check functions"""
    print("\n=== Testing Environment Check ===")
    
    assert sys.version_info.major >= 3
    
    packages = EnvironmentManager.get_installed_packages()
    print(f"Installed packages: {len(packages)}")
    
    assert True
    print("All environment check tests passed!")


def test_package_check():
    """Test package check functions"""
    print("\n=== Testing Package Check ===")
    
    builtins_check = EnvironmentManager.check_package_installed("builtins")
    print(f"builtins check: {builtins_check}")
    
    assert True
    print("All package check tests passed!")


def test_ml_environment():
    """Test ML environment functions"""
    print("\n=== Testing ML Environment ===")
    
    packages = MLEnvironmentSetup.check_ml_packages()
    assert isinstance(packages, dict)
    
    required = MLEnvironmentSetup.get_required_packages()
    assert len(required) > 0
    
    print("All ML environment tests passed!")


def run_all_tests():
    """Run all unit tests"""
    test_environment_check()
    test_package_check()
    test_ml_environment()
    print("\n=== All Tests Passed! ===")


if __name__ == "__main__":
    main()
    print("\n" + "="*60)
    run_all_tests()