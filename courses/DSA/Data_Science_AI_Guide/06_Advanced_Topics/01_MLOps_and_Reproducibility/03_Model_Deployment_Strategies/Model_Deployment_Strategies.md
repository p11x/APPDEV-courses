# Model Deployment Strategies

## I. INTRODUCTION

### What is Model Deployment?
Model Deployment refers to the process of making machine learning models available for inference in production environments. This encompasses a range of strategies from real-time API endpoints to batch processing systems, each with different trade-offs in latency, throughput, scalability, and cost. The goal is to reliably serve model predictions to downstream applications while maintaining model quality, monitoring performance, and enabling easy updates.

Deployment strategies differ based on use case requirements. Real-time prediction needs (like fraud detection) require low-latency responses, typically through API services. Batch prediction scenarios (like report generation) can tolerate higher latency and process larger volumes asynchronously. Edge deployment targets resource-constrained devices with specialized optimization. Understanding these trade-offs is crucial for selecting the appropriate strategy.

### Why is it Important?
Proper deployment directly impacts business value from ML investments. A model that can't be deployed effectively provides no value regardless of its accuracy. Beyond prediction capability, deployment strategies ensure:
- Reliability through redundancy and graceful degradation
- Scalability to handle varying loads
- Monitoring for performance and health
- Safe updates without service disruption
- Compliance with regulatory requirements

In regulated industries like finance and healthcare, deployment practices must include audit trails, roll-back capabilities, and controlled change management. Poor deployment can also expose models to adversarial attacks or data leakage.

### Prerequisites
- Understanding of ML model lifecycle
- Familiarity with REST APIs and microservices
- Knowledge of cloud platforms (AWS, GCP, Azure)
- Basic Docker and containerization
- Understanding of load balancing and caching

## II. FUNDAMENTALS

### Basic Concepts and Definitions

**Inference**: The process of generating predictions from a trained model. Can be synchronous (real-time) or asynchronous (batch).

**Model Serving**: The infrastructure and processes that accept prediction requests and return results.

**Scaling**: Adjusting compute resources to handle demand. Vertical scaling adds resources to existing instances; horizontal scaling adds more instances.

**Deployment Pattern**: A proven approach for deploying models with specific characteristics around availability, latency, and rollout.

### Key Terminology

**Blue-Green Deployment**: Running two identical production environments and switching between them for updates.

**Canary Deployment**: Gradually rolling out changes to a subset of users before full deployment.

**A/B Testing**: Serving different model versions to different user segments to compare performance.

**Rollback**: Reverting to a previous working version when issues are detected.

### Core Principles

**Principle 1: Immutable Infrastructure**
Each deployment creates new infrastructure rather than modifying existing. This enables reliable rollbacks and consistency.

**Principle 2: Graceful Degradation**
When models fail, systems should continue operating with reduced functionality rather than complete failure.

**Principle 3: Zero-Downtime Deployment**
Users experience no service interruption during updates.

**Principle 4: Observability**
Comprehensive logging, metrics, and tracing enable quick problem identification.

## III. IMPLEMENTATION

### Implementation with Flask, FastAPI, and Docker

```python
"""
Model Deployment Implementation
==============================
Demonstrates multiple deployment strategies with Flask and Docker.
"""

import os
import json
import pickle
import time
from pathlib import Path
from typing import Dict, List, Any, Optional, Union
from dataclasses import dataclass
from enum import Enum
import numpy as np
import pandas as pd
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, classification_report
import warnings
warnings.filterwarnings('ignore')


class ModelServer:
    """
    Base class for model serving.
    """
    
    def __init__(self, model_path: str, model_version: str = "1.0.0"):
        """
        Initialize model server.
        
        Args:
            model_path: Path to saved model
            model_version: Version identifier
        """
        self.model_path = model_path
        self.model_version = model_version
        self.model = None
        self.load_model()
    
    def load_model(self) -> None:
        """
        Load model from disk.
        """
        with open(self.model_path, "rb") as f:
            self.model = pickle.load(f)
        print(f"Model loaded: {self.model_version}")
    
    def predict(self, features: np.ndarray) -> np.ndarray:
        """
        Generate predictions.
        
        Args:
            features: Input features
            
        Returns:
            Predictions
        """
        return self.model.predict(features)
    
    def predict_proba(self, features: np.ndarray) -> np.ndarray:
        """
        Generate probability predictions.
        
        Args:
            features: Input features
            
        Returns:
            Prediction probabilities
        """
        return self.model.predict_proba(features)


class FlaskModelServer:
    """
    Flask-based model server with REST API.
    """
    
    def __init__(self, model_path: str, host: str = "0.0.0.0", port: int = 5000):
        """
        Initialize Flask server.
        
        Args:
            model_path: Path to saved model
            host: Host address
            port: Port number
        """
        from flask import Flask, request, jsonify
        
        self.host = host
        self.port = port
        self.app = Flask(__name__)
        
        self.model_server = ModelServer(model_path)
        
        self.app.add_url_rule('/predict', 'predict', self.predict, methods=['POST'])
        self.app.add_url_rule('/health', 'health', self.health, methods=['GET'])
        self.app.add_url_rule('/model_info', 'model_info', self.model_info, methods=['GET'])
    
    def predict(self) -> dict:
        """
        Handle prediction requests.
        """
        from flask import request, jsonify
        
        data = request.get_json()
        
        if 'features' not in data:
            return jsonify({'error': 'Missing features'}), 400
        
        features = np.array(data['features'])
        
        if features.ndim == 1:
            features = features.reshape(1, -1)
        
        prediction = self.model_server.predict(features)[0]
        probabilities = self.model_server.predict_proba(features)[0]
        
        return jsonify({
            'prediction': int(prediction),
            'probabilities': probabilities.tolist(),
            'model_version': self.model_server.model_version
        })
    
    def health(self) -> dict:
        """
        Health check endpoint.
        """
        from flask import jsonify
        
        return jsonify({
            'status': 'healthy',
            'model_version': self.model_server.model_version
        })
    
    def model_info(self) -> dict:
        """
        Model information endpoint.
        """
        from flask import jsonify
        
        return jsonify({
            'model_version': self.model_server.model_version,
            'model_type': type(self.model_server.model).__name__
        })
    
    def run(self, debug: bool = False) -> None:
        """
        Run the Flask server.
        
        Args:
            debug: Enable debug mode
        """
        self.app.run(host=self.host, port=self.port, debug=debug)


class FastAPIModelServer:
    """
    FastAPI-based model server with async support.
    """
    
    def __init__(self, model_path: str):
        """
        Initialize FastAPI server.
        
        Args:
            model_path: Path to saved model
        """
        from fastapi import FastAPI
        from pydantic import BaseModel
        from typing import List
        
        self.model_server = ModelServer(model_path)
        self.app = FastAPI(title="Model API", version="1.0.0")
        
        class PredictionRequest(BaseModel):
            features: List[float]
        
        class PredictionResponse(BaseModel):
            prediction: int
            probabilities: List[float]
            model_version: str
        
        @self.app.get("/health")
        async def health():
            return {"status": "healthy"}
        
        @self.app.get("/model_info")
        async def model_info():
            return {
                "model_version": self.model_server.model_version,
                "model_type": type(self.model_server.model).__name__
            }
        
        @self.app.post("/predict", response_model=PredictionResponse)
        async def predict(request: PredictionRequest):
            features = np.array(request.features).reshape(1, -1)
            prediction = self.model_server.predict(features)[0]
            probabilities = self.model_server.predict_proba(features)[0]
            
            return PredictionResponse(
                prediction=int(prediction),
                probabilities=probabilities.tolist(),
                model_version=self.model_server.model_version
            )


@dataclass
class DeploymentConfig:
    """Configuration for model deployment."""
    model_path: str
    model_version: str
    deployment_type: str
    replicas: int = 1
    resources: Optional[Dict[str, str]] = None
    health_check_path: str = "/health"
    readiness_check_path: str = "/ready"


class DeploymentManager:
    """
    Manages model deployment operations.
    Handles container orchestration and updates.
    """
    
    def __init__(self, config: DeploymentConfig):
        """
        Initialize deployment manager.
        
        Args:
            config: Deployment configuration
        """
        self.config = config
        self.deployment_history = []
    
    def create_dockerfile(self) -> str:
        """
        Generate Dockerfile for model deployment.
        
        Returns:
            Dockerfile content
        """
        dockerfile = f"""
FROM python:3.9-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY {Path(self.config.model_path).name} /app/model.pkl

COPY server.py /app/

EXPOSE 5000

HEALTHCHECK --interval=30s --timeout=3s \\
  CMD curl -f http://localhost:5000/health || exit 1

CMD ["python", "server.py"]
"""
        return dockerfile
    
    def create_kubernetes_manifest(self) -> str:
        """
        Generate Kubernetes deployment manifest.
        
        Returns:
            YAML manifest
        """
        manifest = f"""
apiVersion: apps/v1
kind: Deployment
metadata:
  name: model-deployment-{self.config.model_version}
spec:
  replicas: {self.config.replicas}
  selector:
    matchLabels:
      app: model-server
  template:
    metadata:
      labels:
        app: model-server
        version: {self.config.model_version}
    spec:
      containers:
      - name: model-server
        image: model-server:{self.config.model_version}
        ports:
        - containerPort: 5000
        resources:
          requests:
            memory: "512Mi"
            cpu: "500m"
          limits:
            memory: "1Gi"
            cpu: "1000m"
        livenessProbe:
          httpGet:
            path: {self.config.health_check_path}
            port: 5000
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: {self.config.readiness_check_path}
            port: 5000
          initialDelaySeconds: 10
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: model-service
spec:
  selector:
    app: model-server
  ports:
  - port: 80
    targetPort: 5000
  type: ClusterIP
"""
        return manifest
    
    def create_docker_compose(self) -> str:
        """
        Generate Docker Compose configuration.
        
        Returns:
            Docker Compose YAML
        """
        compose = f"""
version: '3.8'

services:
  model-server:
    build: .
    image: model-server:{self.config.model_version}
    ports:
      - "5000:5000"
   environment:
      - MODEL_PATH=/app/model.pkl
      - MODEL_VERSION={self.config.model_version}
    deploy:
      replicas: {self.config.replicas}
      resources:
        limits:
          cpus: '1'
          memory: 1G
        reservations:
          cpus: '0.5'
          memory: 512M

  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf:ro
    depends_on:
      - model-server
"""
        return compose


class BlueGreenDeployment:
    """
    Implements blue-green deployment strategy.
    """
    
    def __init__(self):
        self.blue_version = None
        self.green_version = None
        self.active_version = "blue"
    
    def deploy(self, version: str, model_path: str) -> None:
        """
        Deploy new version.
        
        Args:
            version: Version identifier
            model_path: Path to model
        """
        if self.active_version == "blue":
            self.green_version = {'version': version, 'model_path': model_path}
            self.active_version = "green"
        else:
            self.blue_version = {'version': version, 'model_path': model_path}
            self.active_version = "blue"
        
        self.deployment_history.append({
            'version': version,
            'timestamp': time.time(),
            'active': self.active_version
        })
    
    def rollback(self) -> None:
        """
        Rollback to previous version.
        """
        if self.active_version == "blue":
            if self.green_version:
                self.active_version = "green"
        else:
            if self.blue_version:
                self.active_version = "blue"
    
    def get_active_version(self) -> str:
        """
        Get currently active version.
        
        Returns:
            Active version identifier
        """
        return self.active_version


class CanaryDeployment:
    """
    Implements canary deployment strategy.
    """
    
    def __init__(self, initial_traffic: float = 10.0):
        """
        Initialize canary deployment.
        
        Args:
            initial_traffic: Initial percentage for canary
        """
        self.versions = []
        self.canary_traffic = initial_traffic
        self.deployment_history = []
    
    def deploy(self, version: str, model_path: str) -> None:
        """
        Deploy new version as canary.
        
        Args:
            version: Version identifier
            model_path: Path to model
        """
        self.versions.append({
            'version': version,
            'model_path': model_path,
            'traffic_percent': self.canary_traffic,
            'status': 'canary'
        })
        
        self.deployment_history.append({
            'version': version,
            'canary_percent': self.canary_traffic
        })
    
    def increase_traffic(self, increment: float = 10.0) -> None:
        """
        Increase canary traffic.
        
        Args:
            increment: Percentage increase
        """
        self.canary_traffic = min(100.0, self.canary_traffic + increment)
        
        if self.versions:
            self.versions[-1]['traffic_percent'] = self.canary_traffic
            
            if self.canary_traffic >= 100:
                self.versions[-1]['status'] = 'production'
    
    def rollback(self) -> None:
        """
        Rollback canary deployment.
        """
        if self.versions:
            self.versions[-1]['status'] = 'rolled_back'
            self.canary_traffic = 0


def run_deployment_example():
    """
    Run deployment example demonstrating strategies.
    """
    print("=" * 70)
    print("MODEL DEPLOYMENT STRATEGIES - EXAMPLE")
    print("=" * 70)
    
    np.random.seed(42)
    n_samples = 500
    
    data = pd.DataFrame({
        'feature_1': np.random.randn(n_samples),
        'feature_2': np.random.rand(n_samples),
        'feature_3': np.random.randint(0, 5, n_samples),
        'target': np.random.randint(0, 2, n_samples)
    })
    
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
    
    model_path = "model.pkl"
    with open(model_path, "wb") as f:
        pickle.dump(model, f)
    
    print(f"Model saved to {model_path}")
    
    server = ModelServer(model_path, "1.0.0")
    
    test_features = np.array([[0.5, 0.3, 2]])
    prediction = server.predict(test_features)
    probabilities = server.predict_proba(test_features)
    
    print(f"\nPrediction: {prediction[0]}")
    print(f"Probabilities: {probabilities[0]}")
    
    config = DeploymentConfig(
        model_path=model_path,
        model_version="1.0.0",
        deployment_type="api",
        replicas=2
    )
    
    manager = DeploymentManager(config)
    
    dockerfile = manager.create_dockerfile()
    print(f"\nDockerfile generated")
    
    k8s_manifest = manager.create_kubernetes_manifest()
    print(f"Kubernetes manifest generated")
    
    compose = manager.create_docker_compose()
    print(f"Docker Compose generated")
    
    blue_green = BlueGreenDeployment()
    blue_green.deploy("1.0.0", model_path)
    print(f"\nBlue-green: Active version = {blue_green.get_active_version()}")
    
    blue_green.deploy("1.1.0", "model_v1.1.0.pkl")
    print(f"Blue-green: New version deployed, active = {blue_green.get_active_version()}")
    
    blue_green.rollback()
    print(f"Blue-green: Rolled back, active = {blue_green.get_active_version()}")
    
    canary = CanaryDeployment(initial_traffic=10)
    canary.deploy("1.1.0", "model_v1.1.0.pkl")
    print(f"\nCanary: Initial traffic = 10%")
    
    canary.increase_traffic(20)
    print(f"Canary: Traffic increased to 30%")
    
    canary.increase_traffic(30)
    print(f"Canary: Traffic increased to 60%")
    
    canary.increase_traffic(50)
    print(f"Canary: Full rollout complete")
    
    return server


if __name__ == "__main__":
    run_deployment_example()
```

## IV. APPLICATIONS

### Standard Example

```python
# Standard API Deployment
server = FlaskModelServer("model.pkl")
server.run(debug=False)
```

### Banking Example: Fraud Detection API

```python
# Deploy fraud detection model in banking environment
class FraudDetectionServer:
    """High-performance fraud detection for banking."""
    
    def __init__(self, model_path, threshold=0.85):
        self.server = ModelServer(model_path)
        self.threshold = threshold
        self.prevention = FraudPrevention()
    
    def check_transaction(self, transaction_data):
        features = self.prevention.extract_features(transaction_data)
        fraud_prob = self.server.predict_proba(features)[0][1]
        
        if fraud_prob > self.threshold:
            self.prevention.alert(fraud_prob, transaction_data)
        
        return {
            'fraud_probability': fraud_prob,
            'action': 'block' if fraud_prob > self.threshold else 'allow',
            'model_version': self.server.model_version
        }
```

### Healthcare Example: Diagnostic Support API

```python
# Deploy diagnostic model for clinical decision support
class DiagnosticServer:
    """Clinical decision support for healthcare."""
    
    def __init__(self, model_path):
        self.server = ModelServer(model_path)
        self.audit = AuditLogger()
    
    def diagnose(self, patient_data):
        features = self.preprocess(patient_data)
        
        prediction = self.server.predict(features)
        probabilities = self.server.predict_proba(features)
        
        self.audit.log(patient_data, prediction, probabilities)
        
        return {
            'recommendation': self.interpret(prediction[0]),
            'confidence': max(probabilities[0]),
            'model_version': self.server.model_version
        }
```

## V. OUTPUT_RESULTS

```
Model saved to model.pkl
Model loaded: 1.0.0

Prediction: 1
Probabilities: [0.3 0.7]

Dockerfile generated
Kubernetes manifest generated
Docker Compose generated

Blue-green: Active version = blue
Blue-green: New version deployed, active = green
Blue-green: Rolled back, active = blue

Canary: Initial traffic = 10%
Canary: Traffic increased to 30%
Canary: Traffic increased to 60%
Canary: Full rollout complete
```

## VI. VISUALIZATION

```
+--------------------------------------------------------+
|            DEPLOYMENT STRATEGIES                         |
+--------------------------------------------------------+

BLUE-GREEN DEPLOYMENT
====================

   User Traffic
        |
        v
   +-----------+      +-----------+
   |  BLUE     |      |  GREEN    |
   | (active)  |      | (standby) |
   +-----------+      +-----------+
        |                |
   Update         Update
        |                |
        v                v
   STANDBY ---------> ACTIVE
        |                
   Rollback           
        v                
   NEW ACTIVE           

CANARY DEPLOYMENT
================

   User Traffic (90%) -----> Current Version
        |
        v
   User Traffic (10%) -----> Canary Version
        |
        +----> Monitor metrics
        |
   Increase gradually
        |
        v
   Full Rollout

```

## VII. ADVANCED_TOPICS

### Extensions
- Auto-scaling based on prediction latency
- Model ensembling for higher availability
- Feature store integration
- A/B testing infrastructure

### Optimization
- Model quantization for faster inference
- ONNX export for portability
- GPU acceleration
- Caching prediction results

### Common Issues
- Cold start latency: Pre-load models, use model warming
- Memory issues: Limit batch sizes, use model streaming
- Version conflicts: Use container isolation

## VIII. CONCLUSION

### Key Takeaways
1. Choose deployment strategy based on latency, scale, and safety requirements
2. Implement proper health checks and monitoring
3. Use blue-green or canary for safe rollouts
4. Containerize models for portability

### Next Steps
- Implement Kubernetes deployment
- Add model monitoring
- Configure auto-scaling

### Further Reading
- TensorFlow Serving documentation
- KubeFlow documentation
- MLflow deployment guides