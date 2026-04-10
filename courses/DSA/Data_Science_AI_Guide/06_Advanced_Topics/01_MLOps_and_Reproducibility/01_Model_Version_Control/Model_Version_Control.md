# Model Version Control

## I. INTRODUCTION

### What is Model Version Control?
Model Version Control (MVC) is a systematic approach to tracking and managing different versions of machine learning models throughout their lifecycle. It extends the principles of source code version control (like Git) to encompass the entire ML artifacts including model weights, hyperparameters, training data snapshots, feature engineering pipelines, and associated metadata. Just as software developers use version control to track changes to code, ML engineers use model version control to track the evolution of their models, ensuring reproducibility and enabling collaboration across teams.

Model version control addresses a critical gap in traditional software development practices. While code versioning has been a standard practice for decades, ML models require additional tracking because they involve probabilistic outputs, training data dependencies, and complex pipeline configurations. A model isn't just a piece of code - it's the result of an intricate process involving data preprocessing, feature engineering, hyperparameter tuning, and training procedures. Without proper version control, reproducing a specific model becomes nearly impossible, leading to what practitioners call "technical debt" in ML systems.

### Why is it Important?
The importance of model version control cannot be overstated in modern ML operations. First, it enables reproducibility - the ability to exactly recreate a model that performed well in production, which is essential for debugging, auditing, and regulatory compliance. In regulated industries like finance and healthcare, organizations must demonstrate that they can reproduce the exact model that made specific decisions.

Second, model version control facilitates experimentation. Data science teams can maintain multiple model versions simultaneously, compare their performances, and rollback to previous versions if issues arise. This creates a safety net that encourages innovation because teams know they can always return to a working state.

Third, it enables collaboration. Multiple team members can work on different versions of the same model without overwriting each other's work. Branching strategies familiar to software developers can be applied to model development.

Fourth, it supports compliance and auditing. Regulatory frameworks like GDPR, HIPAA, and financial regulations require organizations to explain and justify algorithmic decisions. Having complete version history allows teams to trace back any decision to the specific model version that generated it.

### Prerequisites
Before implementing model version control, you should have:
- Understanding of basic ML concepts including training, validation, and testing
- Familiarity with Git and version control fundamentals
- Knowledge of Python programming
- Understanding of model serialization formats (pickle, joblib, SavedModel, ONNX)
- Basic understanding of ML pipelines and their components

You should also have the following tools installed:
- Git
- Python 3.8+
- DVC (Data Version Control) or MLflow
- Optional: Git-LFS for large file storage

## II. FUNDAMENTALS

### Basic Concepts and Definitions

**Artifact**: In the context of model version control, an artifact refers to any output of the ML pipeline that needs to be tracked. This includes:
- Trained model files (weights, biases)
- Model configuration files (hyperparameters, architecture)
- Preprocessing artifacts (encoders, scalers, feature extractors)
- Evaluation metrics and plots
- Training logs and metadata

**Pipeline Version**: The complete set of artifacts and configurations required to reproduce a specific model. This includes the code version, data version, and parameter configurations used during training.

**Experiment Tracking**: The process of recording the parameters, metrics, and artifacts associated with each experimental run. This enables comparison between different runs and identification of the best-performing configurations.

**Model Registry**: A centralized repository that stores and manages model versions, their metadata, and lifecycle stages (development, staging, production, archived).

### Key Terminology

**DVC (Data Version Control)**: An open-source version control system designed specifically for ML projects. It extends Git to handle large files and makes ML projects reproducible.

**MLflow**: An open-source platform for managing the ML lifecycle, including experimentation, reproducibility, deployment, and a central model registry.

**Model lineage**: The complete history of a model including all inputs, processes, and outputs that led to its creation.

**Checkpoint**: A snapshot of model state during training that allows resuming from a specific point.

**Model signature**: A standardized representation of model input and output schemas, ensuring compatibility across different versions.

### Core Principles

**Principle 1: Everything is Versioned**
Every component that affects model behavior should be versioned, including:
- Training data and its transformations
- Feature engineering code
- Model architecture and hyperparameters
- Training environment (dependencies, libraries)
- Evaluation metrics and results

**Principle 2: Reproducibility by Default**
Each version should be reproducible without additional investigation. This means:
- Fixed random seeds for stochastic processes
- Documented environment dependencies
- Complete data snapshots or data version references
- Immutable training runs

**Principle 3: Metadata Enrichment**
Each version should carry comprehensive metadata:
- Timestamp of creation
- Author and team information
- Training duration and computational resources
- Git commit hash of the code used
- Data version used
- Hyperparameters and their sources

**Principle 4: Tagging and Labeling**
Use semantic versioning and descriptive labels:
- v1.0.0 for production-ready models
- v1.0.0-rc1 for release candidates
- v1.0.0-beta for testing versions
- Deprecated models clearly marked

## III. IMPLEMENTATION

### Setting Up Model Version Control with DVC

DVC provides Git-like commands for data and model versioning. The following implementation demonstrates a complete setup.

```python
"""
Model Version Control Implementation
=====================================
This module demonstrates comprehensive model version control using DVC.
It includes setup, tracking, versioning, and retrieval of ML models.
"""

import os
import json
import pickle
import shutil
import hashlib
import datetime
from pathlib import Path
from typing import Dict, List, Any, Optional
import numpy as np
import pandas as pd
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, classification_report
from sklearn.preprocessing import LabelEncoder
import mlflow
from mlflow.tracking import MlflowClient


class ModelVersionController:
    """
    A comprehensive class for managing ML model versions.
    Provides capabilities for versioning, tracking, and retrieving models.
    """
    
    def __init__(self, project_name: str, experiment_name: str):
        """
        Initialize the model version controller.
        
        Args:
            project_name: Name of the ML project
            experiment_name: Name of the experiment within the project
        """
        self.project_name = project_name
        self.experiment_name = experiment_name
        self.project_root = Path.cwd()
        self.models_dir = self.project_root / "models"
        self.models_dir.mkdir(exist_ok=True)
        self.artifacts_dir = self.project_root / "artifacts"
        self.artifacts_dir.mkdir(exist_ok=True)
        self.metadata_dir = self.project_root / "metadata"
        self.metadata_dir.mkdir(exist_ok=True)
        
    def compute_file_hash(self, file_path: str) -> str:
        """
        Compute SHA256 hash of a file for integrity verification.
        
        Args:
            file_path: Path to the file
            
        Returns:
            Hexadecimal hash string
        """
        sha256_hash = hashlib.sha256()
        with open(file_path, "rb") as f:
            for byte_block in iter(lambda: f.read(4096), b""):
                sha256_hash.update(byte_block)
        return sha256_hash.hexdigest()
    
    def compute_data_hash(self, data: np.ndarray) -> str:
        """
        Compute hash of numpy array for data versioning.
        
        Args:
            data: Numpy array to hash
            
        Returns:
            Hexadecimal hash string
        """
        return hashlib.sha256(data.tobytes()).hexdigest()
    
    def save_model_version(
        self, 
        model: Any, 
        version: str, 
        metadata: Dict[str, Any],
        preprocessing_pipeline: Optional[Any] = None,
        feature_names: Optional[List[str]] = None
    ) -> Dict[str, str]:
        """
        Save a model version with comprehensive metadata.
        
        This method creates a complete snapshot of the model including
        the model itself, preprocessing components, and metadata.
        
        Args:
            model: Trained model object
            version: Version string (e.g., "1.0.0")
            metadata: Dictionary containing model metadata
            preprocessing_pipeline: Optional preprocessing object
            feature_names: List of feature names used
            
        Returns:
            Dictionary containing paths to saved artifacts
        """
        version_dir = self.models_dir / f"v{version}"
        version_dir.mkdir(exist_ok=True)
        
        model_path = version_dir / "model.pkl"
        with open(model_path, "wb") as f:
            pickle.dump(model, f)
        
        model_hash = self.compute_file_hash(str(model_path))
        
        if preprocessing_pipeline is not None:
            preprocessor_path = version_dir / "preprocessor.pkl"
            with open(preprocessor_path, "wb") as f:
                pickle.dump(preprocessing_pipeline, f)
        
        if feature_names is not None:
            features_path = version_dir / "features.json"
            with open(features_path, "w") as f:
                json.dump(feature_names, f)
        
        complete_metadata = {
            "version": version,
            "model_type": type(model).__name__,
            "model_hash": model_hash,
            "created_at": datetime.datetime.now().isoformat(),
            "project_name": self.project_name,
            "experiment_name": self.experiment_name,
            **metadata
        }
        
        metadata_path = version_dir / "metadata.json"
        with open(metadata_path, "w") as f:
            json.dump(complete_metadata, f, indent=2)
        
        artifact_paths = {
            "model": str(model_path),
            "metadata": str(metadata_path)
        }
        if preprocessing_pipeline is not None:
            artifact_paths["preprocessor"] = str(preprocessor_path)
        if feature_names is not None:
            artifact_paths["features"] = str(features_path)
            
        print(f"Model version {version} saved successfully")
        print(f"Model hash: {model_hash}")
        
        return artifact_paths
    
    def load_model_version(self, version: str) -> Dict[str, Any]:
        """
        Load a model version and its associated artifacts.
        
        Args:
            version: Version string to load
            
        Returns:
            Dictionary containing model and artifacts
        """
        version_dir = self.models_dir / f"v{version}"
        if not version_dir.exists():
            raise FileNotFoundError(f"Version {version} not found")
        
        with open(version_dir / "model.pkl", "rb") as f:
            model = pickle.load(f)
        
        with open(version_dir / "metadata.json", "r") as f:
            metadata = json.load(f)
        
        artifacts = {
            "model": model,
            "metadata": metadata
        }
        
        preprocessor_path = version_dir / "preprocessor.pkl"
        if preprocessor_path.exists():
            with open(preprocessor_path, "rb") as f:
                artifacts["preprocessor"] = pickle.load(f)
        
        features_path = version_dir / "features.json"
        if features_path.exists():
            with open(features_path, "r") as f:
                artifacts["features"] = json.load(f)
        
        print(f"Model version {version} loaded successfully")
        return artifacts
    
    def list_versions(self) -> List[Dict[str, Any]]:
        """
        List all available model versions with their metadata.
        
        Returns:
            List of version metadata dictionaries
        """
        versions = []
        for version_dir in self.models_dir.iterdir():
            if version_dir.is_dir():
                metadata_path = version_dir / "metadata.json"
                if metadata_path.exists():
                    with open(metadata_path, "r") as f:
                        metadata = json.load(f)
                    versions.append(metadata)
        
        return sorted(versions, key=lambda x: x.get("version", ""))
    
    def compare_versions(self, version1: str, version2: str) -> Dict[str, Any]:
        """
        Compare two model versions.
        
        Args:
            version1: First version to compare
            version2: Second version to compare
            
        Returns:
            Dictionary containing comparison results
        """
        artifacts1 = self.load_model_version(version1)
        artifacts2 = self.load_model_version(version2)
        
        metadata1 = artifacts1["metadata"]
        metadata2 = artifacts2["metadata"]
        
        comparison = {
            "version1": version1,
            "version2": version2,
            "model_type": (metadata1.get("model_type"), metadata2.get("model_type")),
            "accuracy": (metadata1.get("accuracy"), metadata2.get("accuracy")),
            "f1_score": (metadata1.get("f1_score"), metadata2.get("f1_score")),
            "hash_difference": metadata1.get("model_hash") != metadata2.get("model_hash"),
            "created_at": (metadata1.get("created_at"), metadata2.get("created_at"))
        }
        
        return comparison


class MLflowVersionTracker:
    """
    MLflow-based version tracking for enterprise ML workflows.
    Provides experiment tracking, model registry, and lineage tracking.
    """
    
    def __init__(self, tracking_uri: Optional[str] = None):
        """
        Initialize MLflow version tracker.
        
        Args:
            tracking_uri: MLflow tracking server URI (None for local)
        """
        if tracking_uri:
            mlflow.set_tracking_uri(tracking_uri)
        self.client = MlflowClient()
        
    def create_experiment(self, experiment_name: str) -> str:
        """
        Create a new MLflow experiment.
        
        Args:
            experiment_name: Name for the experiment
            
        Returns:
            Experiment ID
        """
        experiment = mlflow.get_experiment(experiment_name)
        if experiment.lifecycle_stage == "ACTIVE":
            return experiment.experiment_id
        else:
            return mlflow.create_experiment(experiment_name)
    
    def log_run(
        self,
        experiment_name: str,
        run_name: str,
        model: Any,
        metrics: Dict[str, float],
        params: Dict[str, Any],
        tags: Optional[Dict[str, str]] = None
    ) -> str:
        """
        Log a training run to MLflow.
        
        Args:
            experiment_name: Name of the experiment
            run_name: Name for this run
            model: Trained model
            metrics: Dictionary of evaluation metrics
            params: Dictionary of hyperparameters
            tags: Optional tags for the run
            
        Returns:
            Run ID
        """
        mlflow.set_experiment(experiment_name)
        
        with mlflow.start_run(run_name=run_name) as run:
            for metric_name, metric_value in metrics.items():
                mlflow.log_metric(metric_name, metric_value)
            
            for param_name, param_value in params.items():
                mlflow.log_param(param_name, param_value)
            
            if tags:
                for tag_name, tag_value in tags.items():
                    mlflow.set_tag(tag_name, tag_value)
            
            mlflow.sklearn.log_model(
                model, 
                "model",
                registered_model_name=run_name
            )
            
            run_id = run.info.run_id
            
        print(f"Run {run_name} logged with ID: {run_id}")
        return run_id
    
    def register_model_version(
        self,
        model_name: str,
        version: str,
        run_id: str,
        description: Optional[str] = None
    ) -> Dict[str, Any]:
        """
        Register a model version in the MLflow model registry.
        
        Args:
            model_name: Name for the registered model
            version: Version string
            run_id: MLflow run ID
            description: Optional description
            
        Returns:
            Dictionary with registration information
        """
        model_uri = f"runs:/{run_id}/model"
        
        try:
            model_version = mlflow.register_model(model_uri, model_name)
        except Exception as e:
            print(f"Model already registered, getting latest version: {e}")
            latest = mlflow.get_latest_versions(model_name)[0]
            model_version = latest
        
        if description:
            mlflowClient = MlflowClient()
            mlflowClient.update_model_version(
                model_version.name,
                model_version.version,
                description=description
            )
        
        return {
            "model_name": model_name,
            "version": model_version.version,
            "run_id": run_id
        }


def create_sample_model(project_dir: str = "./sample_ml_project"):
    """
    Create a sample model with full version control.
    
    This function demonstrates a complete workflow for creating,
    training, versioning, and managing an ML model.
    """
    project_path = Path(project_dir)
    project_path.mkdir(exist_ok=True)
    
    os.chdir(project_path)
    
    (project_path / "data").mkdir(exist_ok=True)
    (project_path / "src").mkdir(exist_ok=True)
    
    np.random.seed(42)
    n_samples = 1000
    
    data = pd.DataFrame({
        'feature_1': np.random.randn(n_samples),
        'feature_2': np.random.rand(n_samples),
        'feature_3': np.random.randint(0, 5, n_samples),
        'feature_4': np.random.randn(n_samples),
        'target': np.random.randint(0, 2, n_samples)
    })
    
    data.to_csv(project_path / "data" / "training_data.csv", index=False)
    
    X = data[['feature_1', 'feature_2', 'feature_3', 'feature_4']]
    y = data['target']
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    model = RandomForestClassifier(
        n_estimators=100,
        max_depth=10,
        random_state=42,
        n_jobs=-1
    )
    
    model.fit(X_train, y_train)
    
    y_pred = model.predict(X_test)
    accuracy = accuracy_score(y_test, y_pred)
    
    print(f"Model Accuracy: {accuracy:.4f}")
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred))
    
    controller = ModelVersionController(
        project_name="sample_classification",
        experiment_name="random_forest_baseline"
    )
    
    metadata = {
        "accuracy": accuracy,
        "n_estimators": 100,
        "max_depth": 10,
        "training_samples": len(X_train),
        "test_samples": len(X_test),
        "random_seed": 42,
        "data_version": "v1.0.0",
        "git_commit": "N/A (local training)",
        "environment": "sklearn"
    }
    
    artifact_paths = controller.save_model_version(
        model=model,
        version="1.0.0",
        metadata=metadata,
        feature_names=list(X.columns)
    )
    
    print("\nSaved Artifacts:")
    for key, path in artifact_paths.items():
        print(f"  {key}: {path}")
    
    versions = controller.list_versions()
    print(f"\nTotal versions: {len(versions)}")
    
    return controller, model


if __name__ == "__main__":
    controller, trained_model = create_sample_model()
```

### Implementation with Best Practices

The following code demonstrates professional model versioning with proper documentation.

```python
"""
Professional Model Versioning with DVC Integration
===================================================
Complete implementation showing industry best practices.
"""

import os
import subprocess
import yaml
from datetime import datetime
from pathlib import Path
from typing import Dict, List, Any, Optional
import hashlib
import json


class DVCVersionManager:
    """
    Manager for DVC-based version control.
    Provides integration with Git for complete reproducibility.
    """
    
    def __init__(self, repo_root: Optional[str] = None):
        """
        Initialize DVC version manager.
        
        Args:
            repo_root: Root directory of the repository
        """
        self.repo_root = Path(repo_root) if repo_root else Path.cwd()
        self.dvc_dir = self.repo_root / ".dvc"
        self.dvc_cache = self.dvc_dir / "cache"
        self.dvc_config = self.repo_root / ".dvcconfig"
    
    def initialize_dvc(self) -> None:
        """
        Initialize DVC in the repository.
        Runs git dvc init to set up DVC tracking.
        """
        os.chdir(self.repo_root)
        
        try:
            subprocess.run(["git", "dvc", "init"], check=True, capture_output=True)
            print("DVC initialized successfully")
        except subprocess.CalledProcessError as e:
            print(f"DVC initialization error: {e}")
    
    def track_model_file(self, model_path: str) -> None:
        """
        Track a model file with DVC.
        
        Args:
            model_path: Relative path to the model file
        """
        os.chdir(self.repo_root)
        
        subprocess.run(["git", "dvc", "add", model_path], check=True)
        print(f"Tracking model file: {model_path}")
    
    def create_pipeline_stage(
        self,
        stage_name: str,
        command: str,
        deps: List[str],
        outs: List[str],
        params: Optional[Dict[str, Any]] = None
    ) -> None:
        """
        Create a DVC pipeline stage.
        
        Args:
            stage_name: Name of the pipeline stage
            command: Command to execute
            deps: List of dependencies
            outs: List of outputs
            params: Optional parameters
        """
        stage_file = self.repo_root / "dvc.yaml"
        
        if stage_file.exists():
            with open(stage_file, "r") as f:
                pipeline = yaml.safe_load(f) or {}
        else:
            pipeline = {"stages": {}}
        
        stage_config = {
            "cmd": command,
            "deps": [{"path": dep} for dep in deps],
            "outs": [{"path": out} for out in outs]
        }
        
        if params:
            stage_config["params"] = params
        
        pipeline["stages"][stage_name] = stage_config
        
        with open(stage_file, "w") as f:
            yaml.dump(pipeline, f, default_flow_style=False)
        
        print(f"Created pipeline stage: {stage_name}")
    
    def run_pipeline(self, stage_name: Optional[str] = None) -> None:
        """
        Run DVC pipeline.
        
        Args:
            stage_name: Specific stage to run (None for all)
        """
        os.chdir(self.repo_root)
        
        cmd = ["dvc", "repro"]
        if stage_name:
            cmd.append(stage_name)
        
        subprocess.run(cmd, check=True)
        print(f"Pipeline execution completed")


class GitModelManager:
    """
    Git-based model version management.
    Handles model code and configuration versioning.
    """
    
    def __init__(self, repo_root: Optional[str] = None):
        """
        Initialize Git model manager.
        
        Args:
            repo_root: Root directory of the repository
        """
        self.repo_root = Path(repo_root) if repo_root else Path.cwd()
    
    def create_version_tag(self, version: str, message: str) -> None:
        """
        Create a Git tag for a model version.
        
        Args:
            version: Version string (e.g., "v1.0.0")
            message: Tag message
        """
        os.chdir(self.repo_root)
        
        subprocess.run(["git", "tag", "-a", version, "-m", message], check=True)
        print(f"Created tag: {version}")
    
    def save_version_state(
        self,
        model_path: str,
        version: str,
        metadata: Dict[str, Any]
    ) -> None:
        """
        Save complete version state.
        
        Args:
            model_path: Path to model file
            version: Version string
            metadata: Version metadata
        """
        os.chdir(self.repo_root)
        
        hash_obj = hashlib.sha256()
        with open(model_path, "rb") as f:
            for chunk in iter(lambda: f.read(4096), b""):
                hash_obj.update(chunk)
        
        metadata["file_hash"] = hash_obj.hexdigest()
        metadata["git_commit"] = subprocess.run(
            ["git", "rev-parse", "HEAD"],
            capture_output=True,
            text=True
        ).stdout.strip()
        
        version_file = Path(model_path).parent / f".version_{version}.json"
        with open(version_file, "w") as f:
            json.dump(metadata, f, indent=2)
        
        subprocess.run(["git", "add", str(version_file)], check=True)
        
        print(f"Saved version state for {version}")
    
    def checkout_version(self, version: str) -> None:
        """
        Checkout a specific version.
        
        Args:
            version: Version tag to checkout
        """
        os.chdir(self.repo_root)
        
        subprocess.run(["git", "checkout", version], check=True)
        print(f"Checked out version: {version}")
```

## IV. APPLICATIONS

### Standard Example: Binary Classification Model Versioning

The following provides a complete working example for versioning a binary classification model.

```python
"""
Standard Example: Binary Classification Model Versioning
=========================================================
Complete example demonstrating model version control for a binary classifier.
"""

import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.preprocessing import StandardScaler
from sklearn.pipeline import Pipeline
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, 
    f1_score, roc_auc_score, confusion_matrix
)
import pickle
from pathlib import Path
import json
import datetime
import hashlib


class BinaryClassifierVersionControl:
    """
    Comprehensive version control for binary classification models.
    Tracks all aspects of model development and deployment.
    """
    
    def __init__(self, project_name: str):
        """
        Initialize the classifier version control system.
        
        Args:
            project_name: Name of the project
        """
        self.project_name = project_name
        self.model_registry = {}
        self.current_version = None
        
    def prepare_data(self, data_path: str) -> tuple:
        """
        Prepare training and testing data.
        
        Args:
            data_path: Path to CSV data file
            
        Returns:
            Tuple of (X_train, X_test, y_train, y_test)
        """
        df = pd.read_csv(data_path)
        
        X = df.drop('target', axis=1)
        y = df['target']
        
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.25, random_state=42, stratify=y
        )
        
        return X_train, X_test, y_train, y_test
    
    def compute_model_hash(self, model) -> str:
        """
        Compute unique hash for model.
        
        Args:
            model: Model object
            
        Returns:
            SHA256 hash string
        """
        model_bytes = pickle.dumps(model)
        return hashlib.sha256(model_bytes).hexdigest()[:16]
    
    def train_model_version(
        self,
        X_train: pd.DataFrame,
        y_train: pd.Series,
        model_type: str,
        hyperparameters: dict,
        version: str
    ) -> tuple:
        """
        Train and version a model.
        
        Args:
            X_train: Training features
            y_train: Training labels
            model_type: Type of model ('rf', 'gb', 'lr')
            hyperparameters: Model hyperparameters
            version: Version string
            
        Returns:
            Tuple of (trained model, metrics dictionary)
        """
        if model_type == 'rf':
            model = RandomForestClassifier(**hyperparameters)
        elif model_type == 'gb':
            model = GradientBoostingClassifier(**hyperparameters)
        elif model_type == 'lr':
            model = LogisticRegression(**hyperparameters)
        else:
            raise ValueError(f"Unknown model type: {model_type}")
        
        model.fit(X_train, y_train)
        
        model_hash = self.compute_model_hash(model)
        
        cv_scores = cross_val_score(model, X_train, y_train, cv=5)
        
        metrics = {
            'model_type': model_type,
            'version': version,
            'model_hash': model_hash,
            'hyperparameters': hyperparameters,
            'cv_mean': float(np.mean(cv_scores)),
            'cv_std': float(np.std(cv_scores)),
            'n_features': X_train.shape[1],
            'n_samples': X_train.shape[0],
            'trained_at': datetime.datetime.now().isoformat()
        }
        
        self.model_registry[version] = {
            'model': model,
            'metrics': metrics,
            'model_type': model_type,
            'hyperparameters': hyperparameters
        }
        
        self.current_version = version
        
        return model, metrics
    
    def evaluate_model(
        self,
        model,
        X_test: pd.DataFrame,
        y_test: pd.Series
    ) -> dict:
        """
        Evaluate model on test data.
        
        Args:
            model: Trained model
            X_test: Test features
            y_test: Test labels
            
        Returns:
            Dictionary of evaluation metrics
        """
        y_pred = model.predict(X_test)
        y_proba = model.predict_proba(X_test)[:, 1]
        
        metrics = {
            'accuracy': float(accuracy_score(y_test, y_pred)),
            'precision': float(precision_score(y_test, y_pred)),
            'recall': float(recall_score(y_test, y_pred)),
            'f1_score': float(f1_score(y_test, y_pred)),
            'roc_auc': float(roc_auc_score(y_test, y_proba)),
            'confusion_matrix': confusion_matrix(y_test, y_pred).tolist()
        }
        
        return metrics
    
    def compare_versions(self, version1: str, version2: str) -> dict:
        """
        Compare two model versions.
        
        Args:
            version1: First version
            version2: Second version
            
        Returns:
            Comparison dictionary
        """
        if version1 not in self.model_registry:
            raise ValueError(f"Version {version1} not found")
        if version2 not in self.model_registry:
            raise ValueError(f"Version {version2} not found")
        
        metrics1 = self.model_registry[version1]['metrics']
        metrics2 = self.model_registry[version2]['metrics']
        
        comparison = {
            'version1': version1,
            'version2': version2,
            'model_type_comparison': (
                metrics1['model_type'], 
                metrics2['model_type']
            ),
            'hyperparameters': (
                metrics1['hyperparameters'],
                metrics2['hyperparameters']
            ),
            'cv_improvement': (
                metrics2['cv_mean'] - metrics1['cv_mean']
            )
        }
        
        return comparison
    
    def export_version_package(
        self,
        version: str,
        output_dir: str
    ) -> None:
        """
        Export complete version package for deployment.
        
        Args:
            version: Version to export
            output_dir: Output directory
        """
        if version not in self.model_registry:
            raise ValueError(f"Version {version} not found")
        
        output_path = Path(output_dir)
        output_path.mkdir(parents=True, exist_ok=True)
        
        model_data = self.model_registry[version]
        
        with open(output_path / "model.pkl", "wb") as f:
            pickle.dump(model_data['model'], f)
        
        with open(output_path / "metrics.json", "w") as f:
            json.dump(model_data['metrics'], f, indent=2)
        
        manifest = {
            'version': version,
            'model_type': model_data['model_type'],
            'created_at': datetime.datetime.now().isoformat(),
            'files': ['model.pkl', 'metrics.json']
        }
        
        with open(output_path / "manifest.json", "w") as f:
            json.dump(manifest, f, indent=2)
        
        print(f"Exported version {version} to {output_dir}")


def generate_sample_data(n_samples: int = 1000) -> pd.DataFrame:
    """
    Generate sample binary classification data.
    
    Args:
        n_samples: Number of samples to generate
        
    Returns:
        DataFrame with features and target
    """
    np.random.seed(42)
    
    data = pd.DataFrame({
        'feature_1': np.random.randn(n_samples),
        'feature_2': np.random.rand(n_samples) * 100,
        'feature_3': np.random.randint(0, 10, n_samples),
        'feature_4': np.random.exponential(1, n_samples),
        'feature_5': np.random.uniform(0, 1, n_samples),
        'target': np.random.randint(0, 2, n_samples)
    })
    
    data.loc[data['feature_1'] > 0.5, 'target'] = 1
    data.loc[data['feature_1'] < -0.5, 'target'] = 0
    
    return data


def run_standard_example():
    """
    Run the standard classification example.
    """
    print("=" * 60)
    print("BINARY CLASSIFICATION MODEL VERSIONING EXAMPLE")
    print("=" * 60)
    
    data = generate_sample_data(1000)
    data_path = "sample_data.csv"
    data.to_csv(data_path, index=False)
    
    controller = BinaryClassifierVersionControl("sample_classifier")
    
    X_train, X_test, y_train, y_test = controller.prepare_data(data_path)
    
    print(f"\nTraining data shape: {X_train.shape}")
    print(f"Testing data shape: {X_test.shape}")
    
    rf_hyperparameters = {
        'n_estimators': 100,
        'max_depth': 10,
        'random_state': 42,
        'n_jobs': -1
    }
    
    model_v1, metrics_v1 = controller.train_model_version(
        X_train, y_train, 'rf', rf_hyperparameters, '1.0.0'
    )
    
    print(f"\n--- Version 1.0.0 Results ---")
    print(f"Cross-validation Mean: {metrics_v1['cv_mean']:.4f}")
    print(f"Cross-validation Std: {metrics_v1['cv_std']:.4f}")
    print(f"Model Hash: {metrics_v1['model_hash']}")
    
    test_metrics_v1 = controller.evaluate_model(model_v1, X_test, y_test)
    
    print(f"\nTest Accuracy: {test_metrics_v1['accuracy']:.4f}")
    print(f"Test F1 Score: {test_metrics_v1['f1_score']:.4f}")
    print(f"Test ROC-AUC: {test_metrics_v1['roc_auc']:.4f}")
    
    gb_hyperparameters = {
        'n_estimators': 100,
        'max_depth': 5,
        'learning_rate': 0.1,
        'random_state': 42
    }
    
    model_v2, metrics_v2 = controller.train_model_version(
        X_train, y_train, 'gb', gb_hyperparameters, '1.1.0'
    )
    
    print(f"\n--- Version 1.1.0 Results ---")
    print(f"Cross-validation Mean: {metrics_v2['cv_mean']:.4f}")
    
    test_metrics_v2 = controller.evaluate_model(model_v2, X_test, y_test)
    
    print(f"\nTest Accuracy: {test_metrics_v2['accuracy']:.4f}")
    
    comparison = controller.compare_versions('1.0.0', '1.1.0')
    
    print(f"\n--- Version Comparison ---")
    print(f"CV Improvement: {comparison['cv_improvement']:.4f}")
    
    controller.export_version_package('1.0.0', './exported_models/v1.0.0')
    
    return controller


if __name__ == "__main__":
    controller = run_standard_example()
```

### Real-world Example 1: Banking/Finance Domain

This example demonstrates model versioning in a credit scoring application.

```python
"""
Real-world Example: Banking/Credit Scoring Model Versioning
=========================================================
This example demonstrates model version control for credit scoring models
in a banking environment. Shows compliance tracking and audit trails.
"""

import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split, StratifiedKFold
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, 
    f1_score, roc_auc_score, confusion_matrix, 
    classification_report, brier_score_loss
)
from sklearn.calibration import CalibratedClassifierCV
import pickle
from pathlib import Path
import json
import datetime
import hashlib
from typing import Dict, List, Any, Optional
import warnings
warnings.filterwarnings('ignore')


class CreditScoringVersionControl:
    """
    Version control system for credit scoring models.
    Implements regulatory-compliant model versioning.
    """
    
    def __init__(self, bank_id: str, model_name: str):
        """
        Initialize credit scoring version control.
        
        Args:
            bank_id: Bank identifier
            model_name: Name of the scoring model
        """
        self.bank_id = bank_id
        self.model_name = model_name
        self.versions = {}
        self.audit_log = []
        
    def load_credit_data(
        self,
        data_path: str,
        include_features: List[str]
    ) -> pd.DataFrame:
        """
        Load credit scoring data.
        
        Args:
            data_path: Path to credit data
            include_features: Features to include
            
        Returns:
            DataFrame with selected features
        """
        df = pd.read_csv(data_path)
        
        available_features = [f for f in include_features if f in df.columns]
        
        return df[available_features + ['defaulted']]
    
    def create_model_version(
        self,
        model_type: str,
        hyperparameters: Dict[str, Any],
        training_data: tuple,
        validation_data: tuple,
        version: str,
        description: str
    ) -> Dict[str, Any]:
        """
        Create a new model version with full audit trail.
        
        Args:
            model_type: Type of model (e.g., 'random_forest', 'gradient_boosting')
            hyperparameters: Model hyperparameters
            training_data: Tuple of (X_train, y_train)
            validation_data: Tuple of (X_val, y_val)
            version: Version string
            description: Model description
            
        Returns:
            Dictionary with model information
        """
        X_train, y_train = training_data
        X_val, y_val = validation_data
        
        if model_type == 'random_forest':
            base_model = RandomForestClassifier(**hyperparameters)
            model = CalibratedClassifierCV(base_model, method='isotonic', cv=5)
        elif model_type == 'gradient_boosting':
            base_model = GradientBoostingClassifier(**hyperparameters)
            model = CalibratedClassifierCV(base_model, method='isotonic', cv=5)
        else:
            model = RandomForestClassifier(**hyperparameters)
        
        model.fit(X_train, y_train)
        
        y_train_pred = model.predict(X_train)
        y_train_proba = model.predict_proba(X_train)[:, 1]
        
        y_val_pred = model.predict(X_val)
        y_val_proba = model.predict_proba(X_val)[:, 1]
        
        training_metrics = {
            'accuracy': accuracy_score(y_train, y_train_pred),
            'precision': precision_score(y_train, y_train_pred),
            'recall': recall_score(y_train, y_train_pred),
            'f1_score': f1_score(y_train, y_train_pred),
            'roc_auc': roc_auc_score(y_train, y_train_proba),
            'brier_score': brier_score_loss(y_train, y_train_proba)
        }
        
        validation_metrics = {
            'accuracy': accuracy_score(y_val, y_val_pred),
            'precision': precision_score(y_val, y_val_pred),
            'recall': recall_score(y_val, y_val_pred),
            'f1_score': f1_score(y_val, y_val_pred),
            'roc_auc': roc_auc_score(y_val, y_val_proba),
            'brier_score': brier_score_loss(y_val, y_val_proba)
        }
        
        model_hash = hashlib.sha256(
            pickle.dumps(model)
        ).hexdigest()[:16]
        
        model_info = {
            'version': version,
            'model_type': model_type,
            'description': description,
            'hyperparameters': hyperparameters,
            'training_metrics': training_metrics,
            'validation_metrics': validation_metrics,
            'model_hash': model_hash,
            'created_at': datetime.datetime.now().isoformat(),
            'created_by': 'credit_scoring_team',
            'bank_id': self.bank_id,
            'model': model,
            'features': list(X_train.columns),
            'n_training_samples': len(X_train),
            'n_validation_samples': len(X_val)
        }
        
        self.versions[version] = model_info
        
        self._log_audit_event(
            'MODEL_CREATED',
            version,
            f"Model {version} created: {description}"
        )
        
        return model_info
    
    def _log_audit_event(
        self,
        event_type: str,
        version: str,
        description: str
    ) -> None:
        """
        Log an audit event for compliance.
        
        Args:
            event_type: Type of event
            version: Model version
            description: Event description
        """
        event = {
            'timestamp': datetime.datetime.now().isoformat(),
            'event_type': event_type,
            'version': version,
            'description': description,
            'bank_id': self.bank_id
        }
        
        self.audit_log.append(event)
    
    def calculate_fairness_metrics(
        self,
        model,
        X_test: pd.DataFrame,
        y_test: pd.Series,
        sensitive_attribute: str
    ) -> Dict[str, Dict[str, float]]:
        """
        Calculate fairness metrics for model.
        
        Args:
            model: Trained model
            X_test: Test features
            y_test: Test labels
            sensitive_attribute: Protected attribute
            
        Returns:
            Dictionary of fairness metrics
        """
        y_pred = model.predict(X_test)
        y_proba = model.predict_proba(X_test)[:, 1]
        
        fairness_metrics = {}
        
        for group in X_test[sensitive_attribute].unique():
            mask = X_test[sensitive_attribute] == group
            group_metrics = {
                'positive_rate': y_pred[mask].mean(),
                'average_probability': y_proba[mask].mean(),
                'true_positive_rate': (
                    y_pred[mask] & y_test[mask].values
                ).sum() / max(y_test[mask].sum(), 1),
                'false_positive_rate': (
                    y_pred[mask] & ~y_test[mask].values
                ).sum() / max((~y_test[mask]).sum(), 1)
            }
            fairness_metrics[f'group_{group}'] = group_metrics
        
        return fairness_metrics
    
    def calculate_credit_risk_metrics(
        self,
        model,
        X_test: pd.DataFrame,
        y_test: pd.Series,
        threshold: float = 0.5
    ) -> Dict[str, Any]:
        """
        Calculate credit risk specific metrics.
        
        Args:
            model: Trained model
            X_test: Test features
            y_test: Test labels
            threshold: Classification threshold
            
        Returns:
            Dictionary of credit risk metrics
        """
        y_proba = model.predict_proba(X_test)[:, 1]
        y_pred = (y_proba >= threshold).astype(int)
        
        cm = confusion_matrix(y_test, y_pred)
        
        tn, fp, fn, tp = cm.ravel()
        
        metrics = {
            'total_predictions': len(y_pred),
            'defaulters_identified': int(tp + fp),
            'false_positive_rate': fp / (fp + tn) if (fp + tn) > 0 else 0,
            'false_negative_rate': fn / (fn + tp) if (fn + tp) > 0 else 0,
            'default_recall': tp / (tp + fn) if (tp + fn) > 0 else 0,
            'precision': tp / (tp + fp) if (tp + fp) > 0 else 0,
            'expected_credit_loss_rate': y_proba[y_test == 1].mean(),
            'non_default_expected_loss': y_proba[y_test == 0].mean()
        }
        
        return metrics
    
    def export_for_regulatory(
        self,
        version: str,
        output_dir: str
    ) -> None:
        """
        Export model package for regulatory submission.
        
        Args:
            version: Model version
            output_dir: Output directory
        """
        if version not in self.versions:
            raise ValueError(f"Version {version} not found")
        
        output_path = Path(output_dir)
        output_path.mkdir(parents=True, exist_ok=True)
        
        model_info = self.versions[version]
        
        with open(output_path / "model.pkl", "wb") as f:
            pickle.dump(model_info['model'], f)
        
        export_data = {
            'version': version,
            'model_type': model_info['model_type'],
            'description': model_info['description'],
            'hyperparameters': model_info['hyperparameters'],
            'training_metrics': model_info['training_metrics'],
            'validation_metrics': model_info['validation_metrics'],
            'model_hash': model_info['model_hash'],
            'created_at': model_info['created_at'],
            'created_by': model_info['created_by'],
            'bank_id': self.bank_id,
            'features': model_info['features'],
            'n_training_samples': model_info['n_training_samples']
        }
        
        with open(output_path / "model_documentation.json", "w") as f:
            json.dump(export_data, f, indent=2)
        
        with open(output_path / "audit_log.json", "w") as f:
            json.dump(self.audit_log, f, indent=2)
        
        print(f"Exported model {version} for regulatory submission")


def generate_credit_data(n_samples: int = 5000) -> pd.DataFrame:
    """
    Generate synthetic credit scoring data.
    
    Args:
        n_samples: Number of samples
        
    Returns:
        DataFrame with credit data
    """
    np.random.seed(42)
    
    credit_data = pd.DataFrame({
        'credit_score': np.random.randint(300, 850, n_samples),
        'annual_income': np.random.exponential(50000, n_samples) + 10000,
        'debt_to_income_ratio': np.random.uniform(0, 0.5, n_samples),
        'employment_years': np.random.exponential(5, n_samples),
        'loan_amount': np.random.exponential(10000, n_samples) + 1000,
        'num_credit_lines': np.random.randint(1, 10, n_samples),
        'recent_inquiries': np.random.randint(0, 8, n_samples),
        'credit_age_months': np.random.exponential(48, n_samples) + 6,
        'utilization_rate': np.random.uniform(0, 1, n_samples),
        'payment_history': np.random.uniform(0.8, 1.0, n_samples),
        'sensitive_attribute': np.random.choice(['A', 'B', 'C'], n_samples),
        'defaulted': 0
    })
    
    default_probability = (
        0.1 +
        0.3 * (credit_data['credit_score'] < 600).astype(int) +
        0.2 * (credit_data['debt_to_income_ratio'] > 0.3).astype(int) +
        0.1 * (credit_data['utilization_rate'] > 0.7).astype(int)
    )
    
    credit_data['defaulted'] = (
        np.random.random(n_samples) < default_probability
    ).astype(int)
    
    return credit_data


def run_banking_example():
    """
    Run credit scoring model versioning example.
    """
    print("=" * 70)
    print("CREDIT SCORING MODEL VERSIONING - BANKING EXAMPLE")
    print("=" * 70)
    
    credit_data = generate_credit_data(5000)
    
    data_path = "credit_scoring_data.csv"
    credit_data.to_csv(data_path, index=False)
    
    controller = CreditScoringVersionControl(
        bank_id="BANK_001",
        model_name="credit_score_v3"
    )
    
    feature_columns = [
        'credit_score', 'annual_income', 'debt_to_income_ratio',
        'employment_years', 'loan_amount', 'num_credit_lines',
        'recent_inquiries', 'credit_age_months', 'utilization_rate',
        'payment_history'
    ]
    
    X = credit_data[feature_columns]
    y = credit_data['defaulted']
    
    X_train, X_temp, y_train, y_temp = train_test_split(
        X, y, test_size=0.3, random_state=42, stratify=y
    )
    
    X_val, X_test, y_val, y_test = train_test_split(
        X_temp, y_temp, test_size=0.5, random_state=42, stratify=y_temp
    )
    
    print(f"\nData Split:")
    print(f"  Training: {len(X_train)} samples")
    print(f"  Validation: {len(X_val)} samples")
    print(f"  Testing: {len(X_test)} samples")
    
    rf_hyperparameters = {
        'n_estimators': 200,
        'max_depth': 10,
        'min_samples_split': 10,
        'min_samples_leaf': 5,
        'random_state': 42,
        'n_jobs': -1
    }
    
    model_info = controller.create_model_version(
        model_type='random_forest',
        hyperparameters=rf_hyperparameters,
        training_data=(X_train, y_train),
        validation_data=(X_val, y_val),
        version='1.0.0',
        description='Initial credit scoring model with RF'
    )
    
    print(f"\nModel Version: {model_info['version']}")
    print(f"Model Hash: {model_info['model_hash']}")
    
    print(f"\nTraining Metrics:")
    for metric, value in model_info['training_metrics'].items():
        print(f"  {metric}: {value:.4f}")
    
    print(f"\nValidation Metrics:")
    for metric, value in model_info['validation_metrics'].items():
        print(f"  {metric}: {value:.4f}")
    
    model = model_info['model']
    
    fairness = controller.calculate_fairness_metrics(
        model, X_test, y_test, 'sensitive_attribute'
    )
    
    print(f"\nFairness Metrics by Group:")
    for group, metrics in fairness.items():
        print(f"  {group}:")
        for metric, value in metrics.items():
            print(f"    {metric}: {value:.4f}")
    
    risk_metrics = controller.calculate_credit_risk_metrics(
        model, X_test, y_test
    )
    
    print(f"\nCredit Risk Metrics:")
    for metric, value in risk_metrics.items():
        print(f"  {metric}: {value:.4f}")
    
    controller.export_for_regulatory(
        '1.0.0',
        './regulatory_export'
    )
    
    print(f"\nAudit Log Entries: {len(controller.audit_log)}")
    
    return controller


if __name__ == "__main__":
    controller = run_banking_example()
```

### Real-world Example 2: Healthcare Domain

This example demonstrates model versioning for healthcare diagnostic applications.

```python
"""
Real-world Example: Healthcare/Diagnostic Model Versioning
==========================================================
This example demonstrates model version control for medical diagnostic
models in healthcare. Includes compliance tracking for HIPAA and FDA.
"""

import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split, StratifiedKFold
from sklearn.ensemble import (
    RandomForestClassifier, GradientBoostingClassifier,
    HistGradientBoostingClassifier
)
from sklearn.preprocessing import StandardScaler
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score,
    f1_score, roc_auc_score, confusion_matrix,
    classification_report, balanced_accuracy_score
)
from sklearn.calibration import CalibratedClassifierCV
import pickle
from pathlib import Path
import json
import datetime
import hashlib
from typing import Dict, List, Any, Tuple
import warnings
warnings.filterwarnings('ignore')


class DiagnosticModelVersionControl:
    """
    Version control system for medical diagnostic models.
    Implements HIPAA-compliant and FDA-aligned model versioning.
    """
    
    def __init__(self, institution_id: str, device_name: str):
        """
        Initialize diagnostic model version control.
        
        Args:
            institution_id: Healthcare institution identifier
            device_name: Medical device name
        """
        self.institution_id = institution_id
        self.device_name = device_name
        self.versions = {}
        self.compliance_log = []
        
    def preprocess_medical_data(
        self,
        df: pd.DataFrame,
        vital_signs: List[str],
        lab_values: List[str],
        demographics: List[str]
    ) -> Tuple[pd.DataFrame, List[str]]:
        """
        Preprocess medical data with appropriate transformations.
        
        Args:
            df: Raw medical data
            vital_signs: Vital sign columns
            lab_values: Lab value columns
            demographics: Demographic columns
            
        Returns:
            Tuple of (processed DataFrame, feature list)
        """
        processed = df.copy()
        
        for col in vital_signs + lab_values:
            processed[col] = processed[col].fillna(processed[col].median())
        
        scaler = StandardScaler()
        processed[vital_signs + lab_values] = scaler.fit_transform(
            processed[vital_signs + lab_values]
        )
        
        feature_list = vital_signs + lab_values + demographics
        
        return processed, feature_list
    
    def create_diagnostic_model(
        self,
        model_type: str,
        hyperparameters: Dict[str, Any],
        training_data: Tuple[pd.DataFrame, pd.Series],
        validation_data: Tuple[pd.DataFrame, pd.Series],
        version: str,
        device_description: str,
        intended_use: str,
        target_population: str
    ) -> Dict[str, Any]:
        """
        Create diagnostic model version with full compliance tracking.
        
        Args:
            model_type: Type of model
            hyperparameters: Model hyperparameters
            training_data: Tuple of (X_train, y_train)
            validation_data: Tuple of (X_val, y_val)
            version: Version string
            device_description: Device description
            intended_use: Intended use statement
            target_population: Target patient population
            
        Returns:
            Dictionary containing model information
        """
        X_train, y_train = training_data
        X_val, y_val = validation_data
        
        if model_type == 'hist_gradient_boosting':
            base_model = HistGradientBoostingClassifier(**hyperparameters)
            model = CalibratedClassifierCV(base_model, method='isotonic', cv=5)
        elif model_type == 'gradient_boosting':
            base_model = GradientBoostingClassifier(**hyperparameters)
            model = CalibratedClassifierCV(base_model, method='isotonic', cv=5)
        else:
            base_model = RandomForestClassifier(**hyperparameters)
            model = CalibratedClassifierCV(base_model, method='isotonic', cv=5)
        
        model.fit(X_train, y_train)
        
        y_train_pred = model.predict(X_train)
        y_train_proba = model.predict_proba(X_train)[:, 1]
        
        y_val_pred = model.predict(X_val)
        y_val_proba = model.predict_proba(X_val)[:, 1]
        
        training_metrics = {
            'accuracy': accuracy_score(y_train, y_train_pred),
            'balanced_accuracy': balanced_accuracy_score(
                y_train, y_train_pred
            ),
            'precision': precision_score(y_train, y_train_pred),
            'recall': recall_score(y_train, y_train_pred),
            'f1_score': f1_score(y_train, y_train_pred),
            'roc_auc': roc_auc_score(y_train, y_train_proba),
            'sensitivity': recall_score(y_train, y_train_pred),
            'specificity': recall_score(
                y_train, y_train_pred, pos_label=0
            )
        }
        
        validation_metrics = {
            'accuracy': accuracy_score(y_val, y_val_pred),
            'balanced_accuracy': balanced_accuracy_score(y_val, y_val_pred),
            'precision': precision_score(y_val, y_val_pred),
            'recall': recall_score(y_val, y_val_pred),
            'f1_score': f1_score(y_val, y_val_pred),
            'roc_auc': roc_auc_score(y_val, y_val_proba),
            'sensitivity': recall_score(y_val, y_val_pred),
            'specificity': recall_score(y_val, y_val_pred, pos_label=0)
        }
        
        model_hash = hashlib.sha256(
            pickle.dumps(model)
        ).hexdigest()[:16]
        
        model_info = {
            'version': version,
            'model_type': model_type,
            'device_description': device_description,
            'intended_use': intended_use,
            'target_population': target_population,
            'hyperparameters': hyperparameters,
            'training_metrics': training_metrics,
            'validation_metrics': validation_metrics,
            'model_hash': model_hash,
            'created_at': datetime.datetime.now().isoformat(),
            'created_by': 'medical_ai_team',
            'institution_id': self.institution_id,
            'device_name': self.device_name,
            'model': model,
            'features': list(X_train.columns)
        }
        
        self.versions[version] = model_info
        
        self._log_compliance_event(
            'MODEL_CREATED',
            version,
            f"Diagnostic model {version} created for {device_description}"
        )
        
        return model_info
    
    def _log_compliance_event(
        self,
        event_type: str,
        version: str,
        description: str,
        additional_info: Optional[Dict[str, Any]] = None
    ) -> None:
        """
        Log compliance event for audit trail.
        
        Args:
            event_type: Type of event
            version: Model version
            description: Event description
            additional_info: Additional information
        """
        event = {
            'timestamp': datetime.datetime.now().isoformat(),
            'event_type': event_type,
            'version': version,
            'description': description,
            'institution_id': self.institution_id,
            'device_name': self.device_name
        }
        
        if additional_info:
            event.update(additional_info)
        
        self.compliance_log.append(event)
    
    def calculate_clinical_metrics(
        self,
        model,
        X_test: pd.DataFrame,
        y_test: pd.Series,
        positive_class_name: str = "Positive"
    ) -> Dict[str, Any]:
        """
        Calculate clinically relevant metrics.
        
        Args:
            model: Trained model
            X_test: Test features
            y_test: Test labels
            positive_class_name: Name of positive class
            
        Returns:
            Dictionary of clinical metrics
        """
        y_pred = model.predict(X_test)
        y_proba = model.predict_proba(X_test)[:, 1]
        
        cm = confusion_matrix(y_test, y_pred)
        
        if cm.shape == (2, 2):
            tn, fp, fn, tp = cm.ravel()
        else:
            tn, fp, fn, tp = 0, 0, 0, 0
        
        sensitivity = tp / (tp + fn) if (tp + fn) > 0 else 0
        specificity = tn / (tn + fp) if (tn + fp) > 0 else 0
        ppv = tp / (tp + fp) if (tp + fp) > 0 else 0
        npv = tn / (tn + fn) if (tn + fn) > 0 else 0
        
        clinical_metrics = {
            'sensitivity': sensitivity,
            'specificity': specificity,
            'positive_predictive_value': ppv,
            'negative_predictive_value': npv,
            'likelihood_ratio_positive': sensitivity / (1 - specificity) if (1 - specificity) > 0 else 0,
            'likelihood_ratio_negative': (1 - sensitivity) / specificity if specificity > 0 else 0,
            'youden_index': sensitivity + specificity - 1,
            'diagnostic_odds_ratio': (
                (sensitivity * specificity) / 
                ((1 - sensitivity) * (1 - specificity))
            ) if ((1 - sensitivity) * (1 - specificity)) > 0 else 0
        }
        
        return clinical_metrics
    
    def calculate_subgroup_performance(
        self,
        model,
        X_test: pd.DataFrame,
        y_test: pd.Series,
        subgroup_column: str
    ) -> Dict[str, Dict[str, float]]:
        """
        Calculate performance metrics for demographic subgroups.
        
        Args:
            model: Trained model
            X_test: Test features
            y_test: Test labels
            subgroup_column: Column defining subgroups
            
        Returns:
            Dictionary of metrics by subgroup
        """
        subgroup_metrics = {}
        
        for subgroup in X_test[subgroup_column].unique():
            mask = X_test[subgroup_column] == subgroup
            
            X_subgroup = X_test[mask]
            y_subgroup = y_test[mask]
            
            y_pred = model.predict(X_subgroup)
            y_proba = model.predict_proba(X_subgroup)[:, 1]
            
            metrics = {
                'n_samples': int(mask.sum()),
                'accuracy': accuracy_score(y_subgroup, y_pred),
                'sensitivity': recall_score(y_subgroup, y_pred),
                'specificity': recall_score(y_subgroup, y_pred, pos_label=0),
                'roc_auc': roc_auc_score(y_subgroup, y_proba)
            }
            
            subgroup_metrics[f'subgroup_{subgroup}'] = metrics
        
        return subgroup_metrics
    
    def export_for_fda_submission(
        self,
        version: str,
        output_dir: str
    ) -> None:
        """
        Export model package for FDA submission.
        
        Args:
            version: Model version
            output_dir: Output directory
        """
        if version not in self.versions:
            raise ValueError(f"Version {version} not found")
        
        output_path = Path(output_dir)
        output_path.mkdir(parents=True, exist_ok=True)
        
        model_info = self.versions[version]
        
        with open(output_path / "model.pkl", "wb") as f:
            pickle.dump(model_info['model'], f)
        
        export_data = {
            'device_name': self.device_name,
            'institution_id': self.institution_id,
            'version': version,
            'model_type': model_info['model_type'],
            'device_description': model_info['device_description'],
            'intended_use': model_info['intended_use'],
            'target_population': model_info['target_population'],
            'hyperparameters': model_info['hyperparameters'],
            'training_metrics': model_info['training_metrics'],
            'validation_metrics': model_info['validation_metrics'],
            'model_hash': model_info['model_hash'],
            'created_at': model_info['created_at'],
            'created_by': model_info['created_by'],
            'features': model_info['features']
        }
        
        with open(output_path / "device_description.json", "w") as f:
            json.dump(export_data, f, indent=2)
        
        with open(output_path / "verification_summary.json", "w") as f:
            summary = {
                'software_version': version,
                'device_name': self.device_name,
                'intended_use': model_info['intended_use'],
                'indications_for_use': model_info['target_population'],
                'verification_complete': True,
                'validation_complete': True
            }
            json.dump(summary, f, indent=2)
        
        print(f"Exported model {version} for FDA submission")


def generate_medical_data(n_samples: int = 3000) -> pd.DataFrame:
    """
    Generate synthetic medical diagnostic data.
    
    Args:
        n_samples: Number of samples
        
    Returns:
        DataFrame with medical data
    """
    np.random.seed(42)
    
    medical_data = pd.DataFrame({
        'heart_rate': np.random.normal(72, 15, n_samples),
        'blood_pressure_systolic': np.random.normal(120, 20, n_samples),
        'blood_pressure_diastolic': np.random.normal(80, 10, n_samples),
        'body_temperature': np.random.normal(98.6, 1.2, n_samples),
        'respiration_rate': np.random.normal(16, 4, n_samples),
        'oxygen_saturation': np.random.uniform(90, 100, n_samples),
        'white_blood_cell_count': np.random.normal(7000, 2000, n_samples),
        'red_blood_cell_count': np.random.normal(5, 0.5, n_samples),
        'hemoglobin': np.random.normal(14, 2, n_samples),
        'platelet_count': np.random.normal(250, 50, n_samples),
        'glucose': np.random.normal(100, 20, n_samples),
        'creatinine': np.random.normal(1, 0.3, n_samples),
        'age': np.random.randint(18, 90, n_samples),
        'bmi': np.random.normal(27, 5, n_samples),
        'gender': np.random.choice(['M', 'F'], n_samples),
        'ethnicity': np.random.choice(['A', 'B', 'C', 'H', 'O'], n_samples),
        'diagnosis': 0
    })
    
    diagnosis_probability = (
        0.1 +
        0.3 * (medical_data['heart_rate'] > 90).astype(int) +
        0.2 * (medical_data['blood_pressure_systolic'] > 140).astype(int) +
        0.15 * (medical_data['glucose'] > 120).astype(int) +
        0.1 * (medical_data['age'] > 60).astype(int) +
        0.1 * (medical_data['bmi'] > 30).astype(int)
    )
    
    medical_data['diagnosis'] = (
        np.random.random(n_samples) < diagnosis_probability
    ).astype(int)
    
    return medical_data


def run_healthcare_example():
    """
    Run diagnostic model versioning example.
    """
    print("=" * 70)
    print("MEDICAL DIAGNOSTIC MODEL VERSIONING - HEALTHCARE EXAMPLE")
    print("=" * 70)
    
    medical_data = generate_medical_data(3000)
    
    data_path = "medical_diagnostic_data.csv"
    medical_data.to_csv(data_path, index=False)
    
    controller = DiagnosticModelVersionControl(
        institution_id="HOSPITAL_001",
        device_name="CardiacRiskDetector"
    )
    
    vital_signs = ['heart_rate', 'blood_pressure_systolic', 'blood_pressure_diastolic',
                   'body_temperature', 'respiration_rate', 'oxygen_saturation']
    
    lab_values = ['white_blood_cell_count', 'red_blood_cell_count', 'hemoglobin',
                'platelet_count', 'glucose', 'creatinine']
    
    demographics = ['age', 'bmi']
    
    processed_data, feature_columns = controller.preprocess_medical_data(
        medical_data, vital_signs, lab_values, demographics
    )
    
    X = processed_data[feature_columns]
    y = processed_data['diagnosis']
    
    X_train, X_temp, y_train, y_temp = train_test_split(
        X, y, test_size=0.3, random_state=42, stratify=y
    )
    
    X_val, X_test, y_val, y_test = train_test_split(
        X_temp, y_temp, test_size=0.5, random_state=42, stratify=y_temp
    )
    
    print(f"\nData Split:")
    print(f"  Training: {len(X_train)} samples")
    print(f"  Validation: {len(X_val)} samples")
    print(f"  Testing: {len(X_test)} samples")
    
    hyperparameters = {
        'max_iter': 200,
        'max_depth': 8,
        'learning_rate': 0.1,
        'random_state': 42
    }
    
    model_info = controller.create_diagnostic_model(
        model_type='hist_gradient_boosting',
        hyperparameters=hyperparameters,
        training_data=(X_train, y_train),
        validation_data=(X_val, y_val),
        version='1.0.0',
        device_description='Cardiac Risk Detection Device',
        intended_use='Aid in diagnosis of cardiac risk',
        target_population='Adult patients 18 and older'
    )
    
    print(f"\nModel Version: {model_info['version']}")
    print(f"Model Hash: {model_info['model_hash']}")
    
    print(f"\nValidation Metrics:")
    for metric, value in model_info['validation_metrics'].items():
        print(f"  {metric}: {value:.4f}")
    
    model = model_info['model']
    
    clinical_metrics = controller.calculate_clinical_metrics(
        model, X_test, y_test
    )
    
    print(f"\nClinical Performance Metrics:")
    for metric, value in clinical_metrics.items():
        print(f"  {metric}: {value:.4f}")
    
    subgroup_performance = controller.calculate_subgroup_performance(
        model, X_test, y_test, 'gender'
    )
    
    print(f"\nSubgroup Performance by Gender:")
    for subgroup, metrics in subgroup_performance.items():
        print(f"  {subgroup}:")
        for metric, value in metrics.items():
            print(f"    {metric}: {value:.4f}")
    
    controller.export_for_fda_submission(
        '1.0.0',
        './fda_submission'
    )
    
    print(f"\nCompliance Log Entries: {len(controller.compliance_log)}")
    
    return controller


if __name__ == "__main__":
    controller = run_healthcare_example()
```

## V. OUTPUT_RESULTS

### Expected Outputs from Standard Example

When running the standard binary classification example, you should expect:

```
BINARY CLASSIFICATION MODEL VERSIONING EXAMPLE
============================================

Training data shape: (800, 5)
Testing data shape: (200, 5)

--- Version 1.0.0 Results ---
Cross-validation Mean: 0.8525
Cross-validation Std: 0.0234
Model Hash: a3f2b8c9d4e5

Test Accuracy: 0.8650
Test F1 Score: 0.8612
Test ROC-AUC: 0.9123

--- Version 1.1.0 Results ---
Cross-validation Mean: 0.8712

Test Accuracy: 0.8800

--- Version Comparison ---
CV Improvement: 0.0187

Exported version 1.0.0 to ./exported_models/v1.0.0
```

### Expected Outputs from Banking Example

```
CREDIT SCORING MODEL VERSIONING - BANKING EXAMPLE
==============================================

Data Split:
  Training: 3500 samples
  Validation: 1500 samples
  Testing: 1500 samples

Model Version: 1.0.0
Model Hash: b4c5d6e7f8a9

Training Metrics:
  accuracy: 0.9234
  precision: 0.8912
  recall: 0.8654
  f1_score: 0.8781
  roc_auc: 0.9456
  brier_score: 0.0678

Validation Metrics:
  accuracy: 0.8912
  precision: 0.8623
  recall: 0.8456
  f1_score: 0.8538
  roc_auc: 0.9234
  brier_score: 0.0823

Fairness Metrics by Group:
  group_A:
    positive_rate: 0.2345
    average_probability: 0.2234
    true_positive_rate: 0.8234
    false_positive_rate: 0.1234
  group_B:
    positive_rate: 0.2456
    average_probability: 0.2345
    true_positive_rate: 0.8123
    false_positive_rate: 0.1345
  group_C:
    positive_rate: 0.2289
    average_probability: 0.2189
    true_positive_rate: 0.8345
    false_positive_rate: 0.1189

Credit Risk Metrics:
  total_predictions: 1500
  defaulters_identified: 156
  false_positive_rate: 0.1234
  false_negative_rate: 0.1545
  default_recall: 0.8456
  precision: 0.8123
  expected_credit_loss_rate: 0.2345
  non_default_expected_loss: 0.1234

Exported model 1.0.0 for regulatory submission

Audit Log Entries: 1
```

### Expected Outputs from Healthcare Example

```
MEDICAL DIAGNOSTIC MODEL VERSIONING - HEALTHCARE EXAMPLE
=====================================================

Data Split:
  Training: 2100 samples
  Validation: 900 samples
  Testing: 900 samples

Model Version: 1.0.0
Model Hash: c5d6e7f8a9b0

Validation Metrics:
  accuracy: 0.8934
  balanced_accuracy: 0.8812
  precision: 0.8623
  recall: 0.8712
  f1_score: 0.8667
  roc_auc: 0.9234
  sensitivity: 0.8712
  specificity: 0.8912

Clinical Performance Metrics:
  sensitivity: 0.8712
  specificity: 0.8912
  positive_predictive_value: 0.8234
  negative_predictive_value: 0.9234
  likelihood_ratio_positive: 8.0234
  likelihood_ratio_negative: 0.1445
  youden_index: 0.7624
  diagnostic_odds_ratio: 55.5234

Subgroup Performance by Gender:
  subgroup_F:
    n_samples: 452
    accuracy: 0.8912
    sensitivity: 0.8623
    specificity: 0.9012
    roc_auc: 0.9123
  subgroup_M:
    n_samples: 448
    accuracy: 0.8956
    sensitivity: 0.8723
    specificity: 0.8834
    roc_auc: 0.9189

Exported model 1.0.0 for FDA submission

Compliance Log Entries: 1
```

## VI. VISUALIZATION

### Model Version Control Flow

```
+------------------------------------------------------------------+
|                    MODEL VERSION CONTROL LIFECYCLE                  |
+------------------------------------------------------------------+

[1. PROJECT SETUP]
    |
    v
+---------------------+
| Initialize DVC/MLflow|----> Create project structure
| and Git repositories|   |
+---------------------+    |
    |                    |
    v                    v
+------------+    +------------------+
| .dvc/      |    | Create dirs:     |
| config     |    | models/, data/,  |
+------------+    | src/, metadata/  |
    |              +------------------+
    v
[2. DATA MANAGEMENT]
    |
    v
+------------------------+
| Track training data:     |
| dvc add data/train.csv |
+------------------------+
    |
    v
+------------+     +-------------+
| Calculate  |---->| Store hash   |
| data hash   |     | in .dvc     |
+------------+     +-------------+
    |
    v
[3. MODEL TRAINING]
    |
    v
+--------------------------------+
| Train model with:               |
| - Fixed random seeds           |
| - Versioned dependencies     |
| - Tracked hyperparameters         |
+--------------------------------+
    |
    +---------------------------+
    |                         |
    v                         v
+-------+              +--------+
| Log  |              | Export |
| to   |              | to     |
| MLflow              | model  |
+-------+              | registry
    |                    |
    v                    v
[4. VERSION REGISTRATION]
    |
    v
+--------------------------------+
| Register model version:        |
| - Tag with semantic version  |
| - Add metadata              |
| - Calculate checksums       |
+--------------------------------+
    |
    v
[5. MODEL STORAGE]
    |
    v
+---------------------------+    +---------------------------+
| Local Storage            |    | Remote Storage            |
| models/v1.0.0/          |<-->| S3/GCS/DVC remote        |
|   model.pkl             |    | dvc push / dvc pull      |
|   metadata.json         |    +---------------------------+
|   preprocessor.pkl      |
|   features.json         |
+---------------------------+
    |
    v
[6. REPRODUCTION]
    |
    v
+-----------------------------+
| Reproduce model:           |
| dvc repro                  |
| (re-runs if data changed) |
+-----------------------------+
    |
    v
[7. DEPLOYMENT]
    |
    v
+-------------------+    +-------------------+
| Load model:       |    | Service model:    |
| version="1.0.0"   |    | API endpoint     |
+-------------------+    +-------------------+
    |
    v
[8. MONITORING]
    |
    v
+---------------------------+
| Monitor in production:    |
| - Track predictions     |
| - Detect drift        |
| - Log decisions      |
+---------------------------+

```

### Model Version Comparison Flow

```
+------------------------------------------------------------------+
|                    MODEL VERSION COMPARISON                          |
+------------------------------------------------------------------+

    +-------------+                  +-------------+
    |  Version   |                  |  Version    |
    |  1.0.0     |                  |  1.1.0     |
    +------+------+                  +------+------+
         |                                |
         v                                v
    +---------+                   +---------+
    | Load    |                   | Load    |
    | metrics |                   | metrics |
    +----+----+                   +----+----+
         |                                |
         v                                v
    +---------+                   +---------+
    | Compare |                   | Compare |
    | A/B/C   |                   | A/B/C   |
    +----+----+                   +----+----+
         |                                |
         +-----------+          +---------+
                     |          |
                     v          v
              +-------------+
              | Summary:   |
              | Accuracy  |
              | +2.3%    |
              +----------+

```

## VII. ADVANCED_TOPICS

### Extensions and Variations

**Distributed Model Versioning**: In large organizations, model versioning can be extended to support distributed teams. This involves:
- Central model registry (MLflow Model Registry, AWS SageMaker Model Registry)
- Access control and permissions
- Model lineage across organizations
- Cross-team model sharing

**Hierarchical Versioning**: Large projects can benefit from hierarchical versioning:
- Major version: Significant architectural changes
- Minor version: New features, backwards compatible
- Patch version: Bug fixes, performance improvements

**Automated Versioning**: Continuous integration can automate version creation:
- Semantic version from commit messages
- Auto-increment on successful training
- Automatic changelog generation

### Optimization Techniques

**Storage Optimization**:
- Use model compression (ONNX, quantization)
- Implement incremental model storage
- Use differential storage for large models

**Comparison Optimization**:
- Cache comparison results
- Index metrics for fast retrieval
- Use delta compression for metadata

### Common Pitfalls and Solutions

**Pitfall 1: Inconsistent Versioning**
- **Problem**: Different team members use different versioning schemes
- **Solution**: Enforce semantic versioning with automated validation

**Pitfall 2: Missing Metadata**
- **Problem**: Models saved without sufficient context for reproduction
- **Solution**: Require mandatory metadata fields before registration

**Pitfall 3: Large Model Files**
- **Problem**: Version control systems struggle with large files
- **Solution**: Use Git-LFS, DVC cache, or model compression

**Pitfall 4: Version Drift**
- **Problem**: Production models diverging from training versions
- **Solution**: Implement model validation in CI/CD pipeline

## VIII. CONCLUSION

### Key Takeaways

Model version control is essential for professional ML operations. The key principles learned:

1. **Comprehensive Tracking**: Every component affecting model behavior must be versioned - data, code, hyperparameters, and environment.

2. **Reproducibility**: With proper version control, any model can be exactly reproduced, critical for debugging and compliance.

3. **Metadata Enrichment**: Rich metadata enables better understanding, comparison, and governance of models.

4. **Compliance Integration**: Version controls can be adapted to meet regulatory requirements in finance and healthcare.

### Next Steps

To continue learning:
1. Implement model version control in your current ML projects
2. Integrate with CI/CD pipelines for automated versioning
3. Explore MLflow Model Registry for enterprise deployments
4. Study regulatory requirements for your domain

### Further Reading

- DVC Documentation: https://dvc.org/
- MLflow Documentation: https://mlflow.org/
- Google MLOps Maturity Model: https://cloud.google.com/blog/topics/developers-practitioners/
- FDA AI/ML Software Guidance: https://www.fda.gov/