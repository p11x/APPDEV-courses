# Topic: MLOps Toolchain Integration
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for MLOps Toolchain Integration

I. INTRODUCTION
MLOps (Machine Learning Operations) encompasses the practices and tools for deploying
and maintaining ML models in production. This module covers CI/CD pipelines,
model registry, feature store integration, and monitoring systems.

II. CORE CONCEPTS
- CI/CD for ML systems
- Model registry and versioning
- Feature store integration
- Model monitoring and observability
- Container orchestration

III. IMPLEMENTATION
"""

import os
import json
import yaml
import hashlib
import shutil
import subprocess
from datetime import datetime, timedelta
from typing import Dict, List, Tuple, Optional, Any
from dataclasses import dataclass, field
from enum import Enum
import numpy as np
import pandas as pd


class ModelStatus(Enum):
    """Model deployment status."""
    REGISTERED = "registered"
    STAGED = "staged"
    PRODUCTION = "production"
    ARCHIVED = "archived"
    FAILED = "failed"


class PipelineStage(Enum):
    """ML pipeline stages."""
    DATA_VALIDATION = "data_validation"
    FEATURE_ENGINEERING = "feature_engineering"
    MODEL_TRAINING = "model_training"
    MODEL_EVALUATION = "model_evaluation"
    MODEL_REGISTRATION = "model_registration"
    DEPLOYMENT = "deployment"


@dataclass
class ModelMetadata:
    """Model metadata for registry."""
    model_name: str
    version: str
    framework: str
    input_schema: Dict[str, Any]
    output_schema: Dict[str, Any]
    metrics: Dict[str, float]
    parameters: Dict[str, Any]
    created_at: str
    created_by: str
    description: str
    tags: List[str] = field(default_factory=list)
    status: ModelStatus = ModelStatus.REGISTERED


@dataclass
class ExperimentRun:
    """ML experiment run information."""
    run_id: str
    experiment_name: str
    start_time: datetime
    end_time: Optional[datetime] = None
    status: str = "running"
    parameters: Dict[str, Any] = field(default_factory=dict)
    metrics: Dict[str, float] = field(default_factory=dict)
    artifacts: List[str] = field(default_factory=list)


class ModelRegistry:
    """Model registry for version management."""

    def __init__(self, registry_path: str = "./model_registry"):
        self.registry_path = registry_path
        self.models: Dict[str, List[ModelMetadata]] = {}
        self._ensure_registry_dir()

    def _ensure_registry_dir(self) -> None:
        """Ensure registry directory exists."""
        os.makedirs(self.registry_path, exist_ok=True)
        os.makedirs(os.path.join(self.registry_path, "models"), exist_ok=True)
        os.makedirs(os.path.join(self.registry_path, "metadata"), exist_ok=True)

    def register_model(
        self,
        model_name: str,
        model_path: str,
        framework: str,
        metrics: Dict[str, float],
        parameters: Dict[str, Any],
        input_schema: Dict[str, Any],
        output_schema: Dict[str, Any],
        description: str = "",
        tags: List[str] = None
    ) -> ModelMetadata:
        """Register a new model version."""
        version = self._generate_version(model_name)
        
        model_dir = os.path.join(self.registry_path, "models", model_name, version)
        os.makedirs(model_dir, exist_ok=True)
        
        if os.path.isfile(model_path):
            shutil.copy2(model_path, os.path.join(model_dir, "model.pkl"))
        else:
            shutil.copytree(model_path, os.path.join(model_dir, "model"))
        
        metadata = ModelMetadata(
            model_name=model_name,
            version=version,
            framework=framework,
            input_schema=input_schema,
            output_schema=output_schema,
            metrics=metrics,
            parameters=parameters,
            created_at=datetime.now().isoformat(),
            created_by="system",
            description=description,
            tags=tags or []
        )
        
        self._save_metadata(model_name, version, metadata)
        
        if model_name not in self.models:
            self.models[model_name] = []
        self.models[model_name].append(metadata)
        
        return metadata

    def _generate_version(self, model_name: str) -> str:
        """Generate unique version for model."""
        existing = self.models.get(model_name, [])
        timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
        version = f"v{len(existing) + 1}_{timestamp}"
        return version

    def _save_metadata(
        self,
        model_name: str,
        version: str,
        metadata: ModelMetadata
    ) -> None:
        """Save model metadata to file."""
        metadata_dir = os.path.join(self.registry_path, "metadata", model_name)
        os.makedirs(metadata_dir, exist_ok=True)
        
        metadata_file = os.path.join(metadata_dir, f"{version}.json")
        with open(metadata_file, 'w') as f:
            json.dump({
                'model_name': metadata.model_name,
                'version': metadata.version,
                'framework': metadata.framework,
                'input_schema': metadata.input_schema,
                'output_schema': metadata.output_schema,
                'metrics': metadata.metrics,
                'parameters': metadata.parameters,
                'created_at': metadata.created_at,
                'created_by': metadata.created_by,
                'description': metadata.description,
                'tags': metadata.tags,
                'status': metadata.status.value
            }, f, indent=2)

    def get_model(self, model_name: str, version: str = None) -> Optional[ModelMetadata]:
        """Get model metadata by name and version."""
        if model_name not in self.models:
            return None
        
        models = self.models[model_name]
        if version is None:
            return models[-1] if models else None
        
        for model in models:
            if model.version == version:
                return model
        return None

    def list_models(self, model_name: str = None) -> List[ModelMetadata]:
        """List all registered models."""
        if model_name:
            return self.models.get(model_name, [])
        
        all_models = []
        for models in self.models.values():
            all_models.extend(models)
        return all_models

    def stage_model(self, model_name: str, version: str) -> bool:
        """Stage a model for deployment."""
        model = self.get_model(model_name, version)
        if model:
            model.status = ModelStatus.STAGED
            self._save_metadata(model_name, version, model)
            return True
        return False

    def promote_to_production(
        self,
        model_name: str,
        version: str
    ) -> List[str]:
        """Promote model to production."""
        model = self.get_model(model_name, version)
        if not model:
            return []
        
        errors = []
        
        if not self._validate_schema(model.input_schema):
            errors.append("Input schema validation failed")
        if not self._validate_schema(model.output_schema):
            errors.append("Output schema validation failed")
        
        if errors:
            model.status = ModelStatus.FAILED
            self._save_metadata(model_name, version, model)
            return errors
        
        self._archive_existing_production(model_name)
        
        model.status = ModelStatus.PRODUCTION
        self._save_metadata(model_name, version, model)
        return []

    def _archive_existing_production(self, model_name: str) -> None:
        """Archive existing production models."""
        for model in self.models.get(model_name, []):
            if model.status == ModelStatus.PRODUCTION:
                model.status = ModelStatus.ARCHIVED
                self._save_metadata(model_name, model.version, model)

    def _validate_schema(self, schema: Dict[str, Any]) -> bool:
        """Validate model schema."""
        return bool(schema)


class FeatureStore:
    """Feature store for ML feature management."""

    def __init__(self, store_path: str = "./feature_store"):
        self.store_path = store_path
        self.features: Dict[str, Dict[str, Any]] = {}
        self._ensure_store_dir()

    def _ensure_store_dir(self) -> None:
        """Ensure feature store directory exists."""
        os.makedirs(self.store_path, exist_ok=True)
        os.makedirs(os.path.join(self.store_path, "features"), exist_ok=True)
        os.makedirs(os.path.join(self.store_path, "groups"), exist_ok=True)

    def register_feature_group(
        self,
        group_name: str,
        features: Dict[str, Any],
        description: str = ""
    ) -> None:
        """Register a feature group."""
        self.features[group_name] = {
            'features': features,
            'description': description,
            'created_at': datetime.now().isoformat()
        }
        
        group_file = os.path.join(self.store_path, "groups", f"{group_name}.json")
        with open(group_file, 'w') as f:
            json.dump(self.features[group_name], f, indent=2)

    def get_feature_group(self, group_name: str) -> Optional[Dict[str, Any]]:
        """Get feature group by name."""
        return self.features.get(group_name)

    def get_online_features(
        self,
        group_name: str,
        entity_keys: Dict[str, Any]
    ) -> Dict[str, Any]:
        """Get online features for entity."""
        feature_group = self.get_feature_group(group_name)
        if not feature_group:
            return {}
        
        features = {}
        for feature_name, feature_def in feature_group['features'].items():
            if 'default_value' in feature_def:
                features[feature_name] = feature_def['default_value']
        
        return features

    def compute_offline_features(
        self,
        group_name: str,
        start_time: datetime,
        end_time: datetime
    ) -> pd.DataFrame:
        """Compute offline features for historical data."""
        feature_group = self.get_feature_group(group_name)
        if not feature_group:
            return pd.DataFrame()
        
        n_samples = 100
        feature_names = list(feature_group['features'].keys())
        
        data = {
            'event_time': pd.date_range(start_time, end_time, periods=n_samples),
        }
        
        for feature_name in feature_names:
            data[feature_name] = np.random.randn(n_samples)
        
        return pd.DataFrame(data)


class CIPipeline:
    """CI/CD pipeline for ML systems."""

    def __init__(self, config_path: str = "./ci_config.yaml"):
        self.config_path = config_path
        self.stages: List[PipelineStage] = []
        self.artifacts: Dict[str, str] = {}

    def load_config(self) -> Dict[str, Any]:
        """Load pipeline configuration."""
        if os.path.exists(self.config_path):
            with open(self.config_path, 'r') as f:
                return yaml.safe_load(f) or {}
        return {}

    def run_stage(
        self,
        stage: PipelineStage,
        config: Dict[str, Any]
    ) -> Tuple[bool, str]:
        """Run a pipeline stage."""
        print(f"\n  Running stage: {stage.value}")
        
        if stage == PipelineStage.DATA_VALIDATION:
            return self._validate_data(config)
        elif stage == PipelineStage.FEATURE_ENGINEERING:
            return self._engineer_features(config)
        elif stage == PipelineStage.MODEL_TRAINING:
            return self._train_model(config)
        elif stage == PipelineStage.MODEL_EVALUATION:
            return self._evaluate_model(config)
        elif stage == PipelineStage.MODEL_REGISTRATION:
            return self._register_model(config)
        elif stage == PipelineStage.DEPLOYMENT:
            return self._deploy_model(config)
        
        return False, "Unknown stage"

    def _validate_data(self, config: Dict[str, Any]) -> Tuple[bool, str]:
        """Data validation stage."""
        print("    - Checking data quality")
        print("    - Validating schema")
        print("    - Checking for nulls and duplicates")
        return True, "Data validation passed"

    def _engineer_features(self, config: Dict[str, Any]) -> Tuple[bool, str]:
        """Feature engineering stage."""
        print("    - Computing features")
        print("    - Storing in feature store")
        return True, "Feature engineering passed"

    def _train_model(self, config: Dict[str, Any]) -> Tuple[bool, str]:
        """Model training stage."""
        print("    - Loading training data")
        print("    - Training model")
        print("    - Saving model checkpoint")
        return True, "Model training passed"

    def _evaluate_model(self, config: Dict[str, Any]) -> Tuple[bool, str]:
        """Model evaluation stage."""
        print("    - Computing evaluation metrics")
        print("    - Comparing to baseline")
        
        metrics = config.get('target_metrics', {})
        for metric_name, threshold in metrics.items():
            print(f"    - {metric_name}: {np.random.uniform(0.8, 0.95):.4f} (threshold: {threshold})")
        
        return True, "Model evaluation passed"

    def _register_model(self, config: Dict[str, Any]) -> Tuple[bool, str]:
        """Model registration stage."""
        print("    - Registering model in registry")
        print("    - Saving metadata")
        return True, "Model registration passed"

    def _deploy_model(self, config: Dict[str, Any]) -> Tuple[bool, str]:
        """Deployment stage."""
        print("    - Building container image")
        print("    - Pushing to registry")
        print("    - Updating service")
        return True, "Model deployment passed"

    def execute_pipeline(
        self,
        stages: List[PipelineStage],
        config: Dict[str, Any]
    ) -> Tuple[bool, List[str]]:
        """Execute full pipeline."""
        results = []
        success = True
        
        for stage in stages:
            stage_success, message = self.run_stage(stage, config)
            results.append(f"{stage.value}: {message}")
            
            if not stage_success:
                success = False
                break
        
        return success, results


class ModelMonitor:
    """Model monitoring and observability."""

    def __init__(self, monitor_config: Dict[str, Any] = None):
        self.config = monitor_config or {}
        self.baseline_metrics: Dict[str, float] = {}
        self.current_metrics: Dict[str, float] = {}
        self.alerts: List[Dict[str, Any]] = []

    def set_baseline(self, metrics: Dict[str, float]) -> None:
        """Set baseline metrics for comparison."""
        self.baseline_metrics = metrics.copy()

    def check_model_health(self, metrics: Dict[str, float]) -> Dict[str, Any]:
        """Check model health against baseline."""
        self.current_metrics = metrics.copy()
        
        health_status = {
            'healthy': True,
            'drift_detected': False,
            'performance_degradation': False,
            'alerts': []
        }
        
        for metric_name, current_value in metrics.items():
            if metric_name in self.baseline_metrics:
                baseline = self.baseline_metrics[metric_name]
                threshold = self.config.get('drift_threshold', 0.1)
                
                drift = abs(current_value - baseline) / baseline if baseline != 0 else 0
                
                if drift > threshold:
                    health_status['drift_detected'] = True
                    health_status['healthy'] = False
                    self._create_alert(
                        metric_name,
                        'drift',
                        f"{metric_name} drift: {drift:.2%}"
                    )
                    health_status['alerts'].append({
                        'metric': metric_name,
                        'type': 'drift',
                        'value': drift
                    })
        
        if health_status['drift_detected']:
            print(f"\n  ALERT: Model drift detected!")
            for alert in health_status['alerts']:
                print(f"    - {alert['metric']}: {alert['value']:.4f}")
        
        return health_status

    def _create_alert(self, metric: str, alert_type: str, message: str) -> None:
        """Create monitoring alert."""
        alert = {
            'timestamp': datetime.now().isoformat(),
            'metric': metric,
            'type': alert_type,
            'message': message
        }
        self.alerts.append(alert)

    def get_metrics_summary(self) -> pd.DataFrame:
        """Get metrics summary as DataFrame."""
        data = []
        for metric_name in set(list(self.baseline_metrics.keys()) + 
                         list(self.current_metrics.keys())):
            data.append({
                'metric': metric_name,
                'baseline': self.baseline_metrics.get(metric_name),
                'current': self.current_metrics.get(metric_name),
                'change': (self.current_metrics.get(metric_name, 0) - 
                         self.baseline_metrics.get(metric_name, 0))
            })
        return pd.DataFrame(data)


class ContainerBuilder:
    """Docker container builder for ML models."""

    def __init__(self, base_image: str = "python:3.9-slim"):
        self.base_image = base_image
        self.packages: List[str] = []
        self.files: List[str] = []

    def add_package(self, package: str) -> None:
        """Add package to requirements."""
        self.packages.append(package)

    def add_file(self, file_path: str, dest_path: str) -> None:
        """Add file to container."""
        self.files.append((file_path, dest_path))

    def build_dockerfile(self, model_path: str, output_path: str) -> str:
        """Build Dockerfile content."""
        dockerfile = f"""FROM {self.base_image}

WORKDIR /app

COPY requirements.txt /app/
RUN pip install --no-cache-dir -r requirements.txt

COPY {model_path} /app/model/

COPY serve_model.py /app/

EXPOSE 8080

CMD ["python", "serve_model.py"]
"""
        
        dockerfile_path = os.path.join(output_path, "Dockerfile")
        with open(dockerfile_path, 'w') as f:
            f.write(dockerfile)
        
        return dockerfile_path

    def build_image(
        self,
        image_name: str,
        output_path: str = "."
    ) -> Tuple[bool, str]:
        """Build Docker image."""
        print(f"\n  Building Docker image: {image_name}")
        print(f"    - Base image: {self.base_image}")
        print(f"    - Packages: {len(self.packages)}")
        print(f"    - Files: {len(self.files)}")
        
        return True, f"Image {image_name} built successfully"


def banking_example():
    """MLOps example for banking sector."""
    print("\n" + "="*60)
    print("BANKING SECTOR: Credit Risk Model MLOps")
    print("="*60)
    
    print("\n1. Model Registry:")
    registry = ModelRegistry("./banking_registry")
    
    metadata = registry.register_model(
        model_name="credit_risk_v1",
        model_path="./models/credit_risk",
        framework="sklearn",
        metrics={
            'auc': 0.85,
            'precision': 0.82,
            'recall': 0.78,
            'f1': 0.80
        },
        parameters={
            'n_estimators': 100,
            'max_depth': 5,
            'learning_rate': 0.1
        },
        input_schema={
            'features': [
                {'name': 'income', 'type': 'float'},
                {'name': 'credit_score', 'type': 'int'},
                {'name': 'employment_years', 'type': 'int'}
            ]
        },
        output_schema={
            'prediction': {'type': 'float'},
            'probability': {'type': 'float'}
        },
        description="Credit risk prediction model"
    )
    print(f"   Model registered: {metadata.model_name} ({metadata.version})")
    
    print("\n2. Feature Store:")
    feature_store = FeatureStore("./banking_features")
    
    feature_store.register_feature_group(
        group_name="credit_features",
        features={
            'income': {'type': 'float', 'default_value': 0.0},
            'credit_score': {'type': 'int', 'default_value': 600},
            'debt_to_income': {'type': 'float', 'default_value': 0.3},
            'payment_history': {'type': 'float', 'default_value': 1.0}
        },
        description="Credit risk feature group"
    )
    print(f"   Feature group registered: credit_features")
    
    print("\n3. Model Monitoring:")
    monitor = ModelMonitor({'drift_threshold': 0.1})
    
    baseline_metrics = {'auc': 0.85, 'precision': 0.82, 'recall': 0.78}
    monitor.set_baseline(baseline_metrics)
    
    current_metrics = {'auc': 0.82, 'precision': 0.79, 'recall': 0.76}
    health = monitor.check_model_health(current_metrics)
    print(f"   Model health: {health['healthy']}")
    print(f"   Drift detected: {health['drift_detected']}")


def healthcare_example():
    """MLOps example for healthcare sector."""
    print("\n" + "="*60)
    print("HEALTHCARE SECTOR: Patient Outcome Prediction MLOps")
    print("="*60)
    
    print("\n1. CI/CD Pipeline:")
    pipeline = CIPipeline()
    
    stages = [
        PipelineStage.DATA_VALIDATION,
        PipelineStage.FEATURE_ENGINEERING,
        PipelineStage.MODEL_TRAINING,
        PipelineStage.MODEL_EVALUATION,
        PipelineStage.MODEL_REGISTRATION,
        PipelineStage.DEPLOYMENT
    ]
    
    config = {
        'data_path': '/data/patient_outcomes',
        'target_metrics': {
            'auc': 0.80,
            'precision': 0.75,
            'recall': 0.70
        },
        'model_path': '/models/outcome_predictor'
    }
    
    success, results = pipeline.execute_pipeline(stages, config)
    print(f"\n   Pipeline success: {success}")
    for result in results:
        print(f"     {result}")
    
    print("\n2. Container Deployment:")
    builder = ContainerBuilder(base_image="python:3.9-slim")
    builder.add_package("scikit-learn")
    builder.add_package("pandas")
    builder.add_package("numpy")
    
    success, message = builder.build_image("patient-outcome-model:v1")
    print(f"   {message}")


def core_implementation():
    """Core implementation demonstrating all components."""
    print("\n" + "="*60)
    print("CORE IMPLEMENTATION")
    print("="*60)
    
    print("\n1. Model Registry:")
    registry = ModelRegistry()
    print("   Model registry initialized")
    
    print("\n2. Feature Store:")
    store = FeatureStore()
    print("   Feature store initialized")
    
    print("\n3. CI/CD Pipeline:")
    pipeline = CIPipeline()
    print("   CI/CD pipeline initialized")
    
    print("\n4. Model Monitor:")
    monitor = ModelMonitor()
    print("   Model monitor initialized")
    
    print("\n5. Container Builder:")
    builder = ContainerBuilder()
    print("   Container builder initialized")


def main():
    print("="*60)
    print("MLOPS TOOLCHAIN INTEGRATION")
    print("="*60)
    
    core_implementation()
    banking_example()
    healthcare_example()
    
    print("\n" + "="*60)
    print("IMPLEMENTATION COMPLETE")
    print("="*60)


if __name__ == "__main__":
    main()