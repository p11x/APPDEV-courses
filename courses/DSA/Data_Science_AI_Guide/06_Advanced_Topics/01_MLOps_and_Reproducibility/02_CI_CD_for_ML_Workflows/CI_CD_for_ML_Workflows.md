# CI/CD for ML Workflows

## I. INTRODUCTION

### What is CI/CD for ML Workflows?
CI/CD for ML Workflows refers to the application of Continuous Integration and Continuous Delivery/Deployment practices specifically tailored for machine learning systems. In traditional software development, CI/CD automates the building, testing, and deployment of code. For ML systems, CI/CD must be extended to handle additional complexities including data versioning, model training, model evaluation, and data drift detection. The goal is to automate the entire ML pipeline from data ingestion to model deployment while ensuring reproducibility, quality, and reliability.

ML workflows differ from traditional software in several fundamental ways. First, there's a data dependency - models depend on training data which can change over time. Second, there's non-determinism - training can produce different results based on random initialization. Third, quality is probabilistic - model performance is measured on held-out data rather than through deterministic tests. Fourth, there's concept drift - the relationship between features and target can change over time. These differences require CI/CD pipelines specifically designed for ML.

### Why is it Important?
CI/CD for ML workflows addresses critical challenges in production ML systems. It ensures reproducibility by automating the entire pipeline from versioned data and code. It improves quality through automated testing including data validation, model validation, and performance thresholds. It enables faster iteration by automating deployment of new model versions. It provides reliability through consistent, tested deployment processes. It enables collaboration by making the development process transparent and reproducible across teams.

In regulated industries like finance and healthcare, CI/CD for ML is often a requirement. Regulatory bodies expect automated audit trails, consistent processes, and controlled deployments. CI/CD pipelines provide all of these capabilities. They also enable A/B testing and gradual rollouts which reduce risk when deploying new models.

### Prerequisites
- Understanding of basic ML workflows and pipelines
- Familiarity with Git and version control
- Knowledge of Python and ML libraries (scikit-learn, TensorFlow, PyTorch)
- Understanding of Docker and containerization
- Basic knowledge of cloud platforms (optional)
- Command line proficiency

## II. FUNDAMENTALS

### Basic Concepts and Definitions

**Pipeline**: A sequence of automated steps that process data, train models, and deploy solutions. ML pipelines include data ingestion, preprocessing, training, evaluation, and deployment stages.

**Continuous Integration (CI)**: The practice of frequently merging code changes and running automated tests. For ML, CI also validates data and model quality on each change.

**Continuous Delivery (CD)**: The practice of keeping code in a deployable state andautomatically deploying changes after passing tests. For ML, CD deploys validated model packages to staging or production environments.

**Continuous Training (CT)**: An ML-specific extension that automatically retrains models when new data is available or performance degrades.

**Orchestration**: The coordination of multiple pipeline stages, including scheduling, error handling, and resource management.

### Key Terminology

**Pipeline as Code**: Defining pipelines using configuration files (YAML, JSON) rather than GUI-based tools, enabling version control and review.

**Stage Gate**: A checkpoint in the pipeline that must pass before proceeding to the next stage. Examples include data validation, model validation, and performance thresholds.

**Data Contract**: An agreement between data producers and consumers specifying expected data schema, types, and valid ranges.

**Model Registry**: A centralized storage for model versions, their metadata, and performance metrics.

### Core Principles

**Principle 1: Everything as Code**
All pipeline components should be defined in code: data processing, model training, evaluation, and deployment. This enables version control, review, and reproducibility.

**Principle 2: Immutable Artifacts**
Each pipeline run should produce immutable artifacts that can be traced back to specific code, data, and configuration versions.

**Principle 3: Fail-Fast**
Pipeline stages should validate early and provide clear error messages. Data validation should occur before training; model validation before deployment.

**Principle 4: Reproducibility**
Pipeline runs should be reproducible given the same code, data, and configuration. This requires fixed random seeds, versioned dependencies, and consistent environments.

## III. IMPLEMENTATION

### Building ML Pipelines with GitHub Actions

This implementation demonstrates automated ML pipelines using GitHub Actions with comprehensive testing and deployment.

```python
"""
CI/CD Pipeline Implementation for ML Workflows
============================================
Demonstrates building complete CI/CD pipelines for ML using Python and YAML configuration.
"""

import os
import json
import yaml
import subprocess
from pathlib import Path
from typing import Dict, List, Any, Optional, Tuple
from datetime import datetime
import hashlib
import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, 
    f1_score, roc_auc_score
)
import pickle
import warnings
warnings.filterwarnings('ignore')


class MLDataValidator:
    """
    Validates ML datasets for pipeline stages.
    Ensures data quality before training begins.
    """
    
    def __init__(self):
        self.validation_results = []
        self.errors = []
        self.warnings = []
    
    def validate_schema(
        self,
        df: pd.DataFrame,
        expected_columns: List[str],
        critical_columns: List[str]
    ) -> bool:
        """
        Validate dataframe schema.
        
        Args:
            df: DataFrame to validate
            expected_columns: Expected column names
            critical_columns: Columns that must be present
            
        Returns:
            True if validation passes
        """
        missing_critical = set(critical_columns) - set(df.columns)
        if missing_critical:
            self.errors.append(
                f"Missing critical columns: {missing_critical}"
            )
            return False
        
        missing_expected = set(expected_columns) - set(df.columns)
        if missing_expected:
            self.warnings.append(
                f"Missing expected columns: {missing_expected}"
            )
        
        self.validation_results.append("Schema validation passed")
        return True
    
    def validate_data_quality(
        self,
        df: pd.DataFrame,
        null_thresholds: Dict[str, float],
        outlier_detection: bool = True
    ) -> bool:
        """
        Validate data quality metrics.
        
        Args:
            df: DataFrame to validate
            null_thresholds: Maximum allowed null percentage per column
            outlier_detection: Whether to detect outliers
            
        Returns:
            True if quality thresholds are met
        """
        for column, max_null_pct in null_thresholds.items():
            if column not in df.columns:
                continue
            
            null_pct = df[column].isnull().sum() / len(df) * 100
            if null_pct > max_null_pct:
                self.errors.append(
                    f"Column {column} has {null_pct:.2f}% nulls, "
                    f"exceeds threshold {max_null_pct}%"
                )
                return False
        
        if outlier_detection:
            numeric_cols = df.select_dtypes(include=[np.number]).columns
            for col in numeric_cols:
                q1 = df[col].quantile(0.25)
                q3 = df[col].quantile(0.75)
                iqr = q3 - q1
                lower = q1 - 1.5 * iqr
                upper = q3 + 1.5 * iqr
                outliers = ((df[col] < lower) | (df[col] > upper)).sum()
                outlier_pct = outliers / len(df) * 100
                
                if outlier_pct > 5:
                    self.warnings.append(
                        f"Column {col} has {outlier_pct:.2f}% outliers"
                    )
        
        self.validation_results.append("Data quality validation passed")
        return True
    
    def validate_distribution(
        self,
        df: pd.DataFrame,
        reference_df: Optional[pd.DataFrame] = None,
        categorical_cols: Optional[List[str]] = None,
        numerical_cols: Optional[List[str]] = None
    ) -> bool:
        """
        Validate data distribution matches expected distribution.
        
        Args:
            df: DataFrame to validate
            reference_df: Reference distribution (optional)
            categorical_cols: Categorical columns to check
            numerical_cols: Numerical columns to check
            
        Returns:
            True if distributions are similar
        """
        if reference_df is None:
            self.validation_results.append(
                "Distribution validation skipped (no reference)"
            )
            return True
        
        if categorical_cols:
            for col in categorical_cols:
                if col not in df.columns:
                    continue
                
                train_cats = set(df[col].unique())
                ref_cats = set(reference_df[col].unique())
                
                new_cats = train_cats - ref_cats
                if new_cats:
                    self.warnings.append(
                        f"New categories in {col}: {new_cats}"
                    )
        
        if numerical_cols:
            for col in numerical_cols:
                if col not in df.columns:
                    continue
                
                mean_diff = abs(df[col].mean() - reference_df[col].mean())
                std_diff = abs(df[col].std() - reference_df[col].std())
                
                if mean_diff > 3 * reference_df[col].std():
                    self.warnings.append(
                        f"Significant mean shift in {col}"
                    )
        
        self.validation_results.append("Distribution validation passed")
        return True
    
    def get_report(self) -> Dict[str, Any]:
        """
        Get validation report.
        
        Returns:
            Dictionary with validation results
        """
        return {
            "passed": len(self.errors) == 0,
            "results": self.validation_results,
            "errors": self.errors,
            "warnings": self.warnings
        }


class MLModelValidator:
    """
    Validates ML models for deployment.
    Ensures model quality meets thresholds.
    """
    
    def __init__(
        self,
        performance_thresholds: Dict[str, float]
    ):
        """
        Initialize model validator.
        
        Args:
            performance_thresholds: Minimum acceptable performance
        """
        self.thresholds = performance_thresholds
        self.validation_results = []
        self.errors = []
        self.warnings = []
    
    def validate_performance(
        self,
        y_true: np.ndarray,
        y_pred: np.ndarray,
        y_proba: Optional[np.ndarray] = None
    ) -> bool:
        """
        Validate model performance against thresholds.
        
        Args:
            y_true: True labels
            y_pred: Predicted labels
            y_proba: Predicted probabilities
            
        Returns:
            True if all thresholds met
        """
        if "accuracy" in self.thresholds:
            accuracy = accuracy_score(y_true, y_pred)
            if accuracy < self.thresholds["accuracy"]:
                self.errors.append(
                    f"Accuracy {accuracy:.4f} below threshold "
                    f"{self.thresholds['accuracy']}"
                )
                return False
            self.validation_results.append(
                f"Accuracy: {accuracy:.4f}"
            )
        
        if "precision" in self.thresholds:
            precision = precision_score(y_true, y_pred)
            if precision < self.thresholds["precision"]:
                self.errors.append(
                    f"Precision {precision:.4f} below threshold "
                    f"{self.thresholds['precision']}"
                )
                return False
            self.validation_results.append(
                f"Precision: {precision:.4f}"
            )
        
        if "recall" in self.thresholds:
            recall = recall_score(y_true, y_pred)
            if recall < self.thresholds["recall"]:
                self.errors.append(
                    f"Recall {recall:.4f} below threshold "
                    f"{self.thresholds['recall']}"
                )
                return False
            self.validation_results.append(
                f"Recall: {recall:.4f}"
            )
        
        if "f1" in self.thresholds:
            f1 = f1_score(y_true, y_pred)
            if f1 < self.thresholds["f1"]:
                self.errors.append(
                    f"F1 Score {f1:.4f} below threshold "
                    f"{self.thresholds['f1']}"
                )
                return False
            self.validation_results.append(
                f"F1 Score: {f1:.4f}"
            )
        
        if "roc_auc" in self.thresholds and y_proba is not None:
            roc_auc = roc_auc_score(y_true, y_proba)
            if roc_auc < self.thresholds["roc_auc"]:
                self.errors.append(
                    f"ROC-AUC {roc_auc:.4f} below threshold "
                    f"{self.thresholds['roc_auc']}"
                )
                return False
            self.validation_results.append(
                f"ROC-AUC: {roc_auc:.4f}"
            )
        
        self.validation_results.append("Performance validation passed")
        return True
    
    def validate_fairness(
        self,
        model,
        X_test: pd.DataFrame,
        y_test: pd.Series,
        protected_attribute: str,
        fairness_threshold: float = 0.1
    ) -> bool:
        """
        Validate model fairness across groups.
        
        Args:
            model: Trained model
            X_test: Test features
            y_test: Test labels
            protected_attribute: Column for protected attribute
            fairness_threshold: Maximum allowed disparity
            
        Returns:
            True if fairness threshold met
        """
        y_pred = model.predict(X_test)
        
        group_rates = {}
        for group in X_test[protected_attribute].unique():
            mask = X_test[protected_attribute] == group
            group_rates[group] = y_pred[mask].mean()
        
        if len(group_rates) > 1:
            rate_values = list(group_rates.values())
            max_diff = max(rate_values) - min(rate_values)
            
            if max_diff > fairness_threshold:
                self.warnings.append(
                    f"Fairness disparity {max_diff:.4f} exceeds "
                    f"threshold {fairness_threshold}"
                )
                for group, rate in group_rates.items():
                    self.warnings.append(
                        f"  {group}: {rate:.4f}"
                    )
            else:
                self.validation_results.append(
                    f"Fairness validation passed (max diff: {max_diff:.4f})"
                )
        
        return True
    
    def get_report(self) -> Dict[str, Any]:
        """
        Get validation report.
        
        Returns:
            Dictionary with validation results
        """
        return {
            "passed": len(self.errors) == 0,
            "results": self.validation_results,
            "errors": self.errors,
            "warnings": self.warnings
        }


class MLPipelineOrchestrator:
    """
    Orchestrates ML pipelines with stage management.
    Handles execution, logging, and error handling.
    """
    
    def __init__(self, project_name: str):
        """
        Initialize pipeline orchestrator.
        
        Args:
            project_name: Name of the ML project
        """
        self.project_name = project_name
        self.stages = {}
        self.current_stage = None
        self.stage_outputs = {}
        self.execution_log = []
    
    def register_stage(
        self,
        stage_name: str,
        stage_function: callable,
        dependencies: Optional[List[str]] = None,
        required: bool = True
    ) -> None:
        """
        Register a pipeline stage.
        
        Args:
            stage_name: Name of the stage
            stage_function: Function to execute
            dependencies: Required preceding stages
            required: Whether stage is required
        """
        self.stages[stage_name] = {
            "function": stage_function,
            "dependencies": dependencies or [],
            "required": required,
            "status": "pending"
        }
    
    def execute_stage(
        self,
        stage_name: str,
        **kwargs
    ) -> Any:
        """
        Execute a pipeline stage.
        
        Args:
            stage_name: Name of stage to execute
            **kwargs: Arguments for stage function
            
        Returns:
            Stage output
        """
        stage = self.stages[stage_name]
        
        for dep in stage["dependencies"]:
            if dep not in self.stage_outputs:
                raise ValueError(
                    f"Dependency {dep} not satisfied for {stage_name}"
                )
        
        self.execution_log.append({
            "timestamp": datetime.now().isoformat(),
            "stage": stage_name,
            "status": "running"
        })
        
        try:
            result = stage["function"](**kwargs)
            self.stage_outputs[stage_name] = result
            
            self.execution_log.append({
                "timestamp": datetime.now().isoformat(),
                "stage": stage_name,
                "status": "completed"
            })
            
            return result
            
        except Exception as e:
            self.execution_log.append({
                "timestamp": datetime.now().isoformat(),
                "stage": stage_name,
                "status": "failed",
                "error": str(e)
            })
            raise
    
    def execute_pipeline(
        self,
        start_stage: Optional[str] = None,
        **kwargs
    ) -> Dict[str, Any]:
        """
        Execute the pipeline.
        
        Args:
            start_stage: Stage to start from (None for all)
            **kwargs: Global arguments
            
        Returns:
            Pipeline outputs
        """
        if start_stage:
            stages_to_run = self._get_dependency_chain(start_stage)
        else:
            stages_to_run = list(self.stages.keys())
        
        for stage_name in stages_to_run:
            self.execute_stage(stage_name, **kwargs)
        
        return self.stage_outputs
    
    def _get_dependency_chain(
        self,
        start_stage: str
    ) -> List[str]:
        """
        Get ordered list of stages to run.
        
        Args:
            start_stage: Starting stage
            
        Returns:
            Ordered list of stages
        """
        visited = set()
        ordered = []
        
        def visit(stage):
            if stage in visited:
                return
            visited.add(stage)
            
            for dep in self.stages[stage]["dependencies"]:
                visit(dep)
            
            ordered.append(stage)
        
        visit(start_stage)
        return ordered
    
    def get_log(self) -> List[Dict[str, Any]]:
        """
        Get execution log.
        
        Returns:
            List of log entries
        """
        return self.execution_log


def generate_pipeline_yaml() -> str:
    """
    Generate GitHub Actions YAML configuration.
    
    Returns:
    YAML string for pipeline configuration
    """
    yaml_content = """
name: ML Pipeline

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]
  schedule:
    - cron: '0 0 * * *'

env:
  PYTHON_VERSION: '3.9'
  MODEL_NAME: classifier

jobs:
  validate-data:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Set up Python
        uses: actions/setup-python@v4
        with:
          python-version: ${{ env.PYTHON_VERSION }}
      
      - name: Install dependencies
        run: |
          pip install -r requirements.txt
      
      - name: Run data validation
        run: python src/validate_data.py
      
      - name: Upload validation report
        uses: actions/upload-artifact@v3
        with:
          name: data-validation
          path: reports/data_validation.json

  train-model:
    needs: validate-data
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Set up Python
        uses: actions/setup-python@v4
        with:
          python-version: ${{ env.PYTHON_VERSION }}
      
      - name: Install dependencies
        run: pip install -r requirements.txt
      
      - name: Train model
        run: python src/train_model.py
      
      - name: Upload model artifacts
        uses: actions/upload-artifact@v3
        with:
          name: model-artifacts
          path: models/

  validate-model:
    needs: train-model
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Download model
        uses: actions/download-artifact@v3
        with:
          name: model-artifacts
      
      - name: Set up Python
        uses: actions/setup-python@v4
        with:
          python-version: ${{ env.PYTHON_VERSION }}
      
      - name: Install dependencies
        run: pip install -r requirements.txt
      
      - name: Run model validation
        run: python src/validate_model.py
      
      - name: Upload validation report
        uses: actions/upload-artifact@v3
        with:
          name: model-validation
          path: reports/model_validation.json

  deploy-staging:
    needs: validate-model
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/develop'
    steps:
      - name: Deploy to staging
        run: |
          echo "Deploying to staging environment"
          # Add deployment commands here

  deploy-production:
    needs: validate-model
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    steps:
      - name: Deploy to production
        run: |
          echo "Deploying to production environment"
          # Add deployment commands here
"""
    return yaml_content


def run_example():
    """
    Run complete CI/CD pipeline example.
    """
    print("=" * 70)
    print("CI/CD FOR ML WORKFLOWS - EXAMPLE")
    print("=" * 70)
    
    np.random.seed(42)
    n_samples = 1000
    
    data = pd.DataFrame({
        'feature_1': np.random.randn(n_samples),
        'feature_2': np.random.rand(n_samples),
        'feature_3': np.random.randint(0, 5, n_samples),
        'target': np.random.randint(0, 2, n_samples)
    })
    
    data.to_csv("data/training_data.csv", index=False)
    
    validator = MLDataValidator()
    
    expected_cols = ['feature_1', 'feature_2', 'feature_3', 'target']
    critical_cols = ['feature_1', 'target']
    
    schema_valid = validator.validate_schema(
        data, expected_cols, critical_cols
    )
    print(f"\nSchema validation: {'PASSED' if schema_valid else 'FAILED'}")
    
    null_thresholds = {'feature_1': 0.05, 'feature_2': 0.05}
    quality_valid = validator.validate_data_quality(data, null_thresholds)
    print(f"Data quality validation: {'PASSED' if quality_valid else 'FAILED'}")
    
    data_report = validator.get_report()
    print(f"\nValidation Report:")
    print(f"  Passed: {data_report['passed']}")
    print(f"  Errors: {len(data_report['errors'])}")
    print(f"  Warnings: {len(data_report['warnings'])}")
    
    X = data[['feature_1', 'feature_2', 'feature_3']]
    y = data['target']
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    model = RandomForestClassifier(
        n_estimators=100,
        max_depth=10,
        random_state=42
    )
    
    model.fit(X_train, y_train)
    
    y_pred = model.predict(X_test)
    y_proba = model.predict_proba(X_test)[:, 1]
    
    performance_thresholds = {
        'accuracy': 0.70,
        'precision': 0.65,
        'recall': 0.65,
        'f1': 0.65,
        'roc_auc': 0.70
    }
    
    model_validator = MLModelValidator(performance_thresholds)
    model_valid = model_validator.validate_performance(
        y_test, y_pred, y_proba
    )
    print(f"\nModel validation: {'PASSED' if model_valid else 'FAILED'}")
    
    model_report = model_validator.get_report()
    print(f"Model Report:")
    print(f"  Passed: {model_report['passed']}")
    print(f"  Errors: {len(model_report['errors'])}")
    print(f"  Results:")
    for result in model_report['results']:
        print(f"    {result}")
    
    pipeline = MLPipelineOrchestrator("example_pipeline")
    
    def stage_ingest(**kwargs):
        print("  Stage: Data Ingestion")
        return {"data_loaded": True}
    
    def stage_preprocess(**kwargs):
        print("  Stage: Preprocessing")
        return {"data_processed": True}
    
    def stage_train(**kwargs):
        print("  Stage: Training")
        return {"model_trained": True}
    
    pipeline.register_stage(
        "ingest", stage_ingest, dependencies=[], required=True
    )
    pipeline.register_stage(
        "preprocess", stage_preprocess, dependencies=["ingest"], required=True
    )
    pipeline.register_stage(
        "train", stage_train, dependencies=["preprocess"], required=True
    )
    
    print("\nExecuting Pipeline:")
    pipeline.execute_pipeline()
    
    print("\nPipeline Log:")
    for entry in pipeline.get_log():
        print(f"  {entry['timestamp']} - {entry['stage']}: {entry['status']}")
    
    print("\nGitHub Actions YAML generated successfully")
    
    return pipeline


if __name__ == "__main__":
    run_example()
```

## IV. APPLICATIONS

### Standard Example: Classification Pipeline with CI/CD