# ML Model Integration

## Overview

FastAPI is ideal for serving machine learning models as APIs. This guide covers model integration, prediction endpoints, and production patterns.

## Basic ML API

### Simple Prediction Endpoint

```python
# Example 1: Basic ML model serving
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
from typing import List
import pickle
import numpy as np

app = FastAPI(title="ML Prediction API")

# Load model at startup
model = None

@app.on_event("startup")
async def load_model():
    global model
    with open("model.pkl", "rb") as f:
        model = pickle.load(f)

class PredictionRequest(BaseModel):
    features: List[float] = Field(..., min_length=1)

class PredictionResponse(BaseModel):
    prediction: float
    confidence: float
    model_version: str

@app.post("/predict", response_model=PredictionResponse)
async def predict(request: PredictionRequest):
    """Make prediction"""
    if model is None:
        raise HTTPException(500, "Model not loaded")

    features = np.array(request.features).reshape(1, -1)
    prediction = model.predict(features)[0]

    # Get confidence if available
    if hasattr(model, "predict_proba"):
        confidence = float(model.predict_proba(features).max())
    else:
        confidence = 1.0

    return PredictionResponse(
        prediction=float(prediction),
        confidence=confidence,
        model_version="1.0.0"
    )
```

## Model Versioning

### Multi-Model Serving

```python
# Example 2: Model versioning
from fastapi import FastAPI, Path
from typing import Dict
import joblib
from pathlib import Path as FilePath

app = FastAPI()

class ModelRegistry:
    """Manage multiple model versions"""

    def __init__(self, models_dir: str = "models"):
        self.models_dir = FilePath(models_dir)
        self.models: Dict[str, any] = {}
        self.default_version = "latest"

    def load_models(self):
        """Load all model versions"""
        for model_file in self.models_dir.glob("*.pkl"):
            version = model_file.stem
            self.models[version] = joblib.load(model_file)

        # Set latest version
        if self.models:
            self.default_version = sorted(self.models.keys())[-1]

    def get_model(self, version: str = None):
        """Get model by version"""
        version = version or self.default_version
        return self.models.get(version)

    def list_versions(self):
        """List available model versions"""
        return list(self.models.keys())

registry = ModelRegistry()

@app.on_event("startup")
async def startup():
    registry.load_models()

@app.get("/models/versions")
async def list_versions():
    """List available model versions"""
    return {
        "versions": registry.list_versions(),
        "default": registry.default_version
    }

@app.post("/predict/{version}")
async def predict_version(
    version: str,
    features: List[float]
):
    """Predict with specific model version"""
    model = registry.get_model(version)
    if not model:
        raise HTTPException(404, f"Model version {version} not found")

    prediction = model.predict([features])[0]
    return {"version": version, "prediction": float(prediction)}

@app.post("/predict")
async def predict_default(features: List[float]):
    """Predict with default model version"""
    model = registry.get_model()
    prediction = model.predict([features])[0]
    return {"version": registry.default_version, "prediction": float(prediction)}
```

## Batch Predictions

### Batch Processing

```python
# Example 3: Batch prediction endpoint
from fastapi import FastAPI, BackgroundTasks
from pydantic import BaseModel
from typing import List
import asyncio
import uuid

app = FastAPI()

class BatchRequest(BaseModel):
    items: List[List[float]]

class BatchResponse(BaseModel):
    batch_id: str
    predictions: List[float]
    total: int

class BatchJob(BaseModel):
    batch_id: str
    status: str
    total: int
    processed: int
    results: List[float] = []

# Store batch job status
batch_jobs: dict = {}

@app.post("/predict/batch", response_model=BatchResponse)
async def batch_predict(request: BatchRequest):
    """Synchronous batch prediction"""
    predictions = []
    for features in request.items:
        prediction = model.predict([features])[0]
        predictions.append(float(prediction))

    return BatchResponse(
        batch_id=str(uuid.uuid4()),
        predictions=predictions,
        total=len(predictions)
    )

@app.post("/predict/batch/async")
async def async_batch_predict(
    request: BatchRequest,
    background_tasks: BackgroundTasks
):
    """Asynchronous batch prediction"""
    batch_id = str(uuid.uuid4())

    batch_jobs[batch_id] = BatchJob(
        batch_id=batch_id,
        status="processing",
        total=len(request.items),
        processed=0
    )

    background_tasks.add_task(process_batch, batch_id, request.items)

    return {"batch_id": batch_id, "status": "processing"}

async def process_batch(batch_id: str, items: List[List[float]]):
    """Process batch in background"""
    job = batch_jobs[batch_id]

    for i, features in enumerate(items):
        prediction = model.predict([features])[0]
        job.results.append(float(prediction))
        job.processed = i + 1
        await asyncio.sleep(0)  # Yield to event loop

    job.status = "completed"

@app.get("/predict/batch/{batch_id}")
async def get_batch_status(batch_id: str):
    """Get batch job status"""
    if batch_id not in batch_jobs:
        raise HTTPException(404, "Batch job not found")

    return batch_jobs[batch_id]
```

## Model Health Monitoring

### Model Metrics

```python
# Example 4: Model monitoring
from fastapi import FastAPI
from prometheus_client import Counter, Histogram
import time

app = FastAPI()

# Metrics
PREDICTION_COUNT = Counter(
    'ml_predictions_total',
    'Total predictions',
    ['model_version', 'status']
)

PREDICTION_DURATION = Histogram(
    'ml_prediction_duration_seconds',
    'Prediction duration',
    ['model_version']
)

class ModelMonitor:
    """Monitor model performance"""

    def __init__(self):
        self.predictions = []
        self.drift_detector = None

    def record_prediction(
        self,
        features: list,
        prediction: float,
        actual: float = None
    ):
        """Record prediction for monitoring"""
        self.predictions.append({
            "features": features,
            "prediction": prediction,
            "actual": actual,
            "timestamp": time.time()
        })

    def get_metrics(self):
        """Get model metrics"""
        return {
            "total_predictions": len(self.predictions),
            "avg_prediction": sum(p["prediction"] for p in self.predictions) / len(self.predictions) if self.predictions else 0
        }

monitor = ModelMonitor()

@app.post("/predict")
async def predict(features: List[float]):
    """Make prediction with monitoring"""
    start = time.time()

    prediction = model.predict([features])[0]

    # Record metrics
    duration = time.time() - start
    PREDICTION_COUNT.labels(model_version="1.0.0", status="success").inc()
    PREDICTION_DURATION.labels(model_version="1.0.0").observe(duration)

    monitor.record_prediction(features, float(prediction))

    return {"prediction": float(prediction)}

@app.get("/metrics/model")
async def model_metrics():
    """Get model metrics"""
    return monitor.get_metrics()
```

## Feature Store Integration

### Feature Management

```python
# Example 5: Feature store integration
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Dict, List, Optional
import redis

app = FastAPI()

class FeatureStore:
    """Simple feature store using Redis"""

    def __init__(self):
        self.redis = redis.Redis(host='localhost', port=6379, decode_responses=True)

    def get_features(self, entity_id: str, feature_names: List[str]) -> Dict:
        """Get features for entity"""
        pipe = self.redis.pipeline()
        for name in feature_names:
            pipe.hget(f"features:{entity_id}", name)
        values = pipe.execute()

        return dict(zip(feature_names, values))

    def set_features(self, entity_id: str, features: Dict):
        """Set features for entity"""
        self.redis.hset(f"features:{entity_id}", mapping=features)

feature_store = FeatureStore()

class PredictionRequest(BaseModel):
    entity_id: str
    feature_names: List[str]

@app.post("/predict/entity")
async def predict_entity(request: PredictionRequest):
    """Predict using features from feature store"""
    features = feature_store.get_features(
        request.entity_id,
        request.feature_names
    )

    # Convert to model input
    feature_values = [float(v) for v in features.values()]
    prediction = model.predict([feature_values])[0]

    return {
        "entity_id": request.entity_id,
        "prediction": float(prediction),
        "features_used": features
    }
```

## Async Model Inference

### Async Predictions

```python
# Example 6: Async model inference
from fastapi import FastAPI
import asyncio
from concurrent.futures import ThreadPoolExecutor
import numpy as np

app = FastAPI()

# Thread pool for CPU-bound tasks
executor = ThreadPoolExecutor(max_workers=4)

async def async_predict(features):
    """Run prediction in thread pool"""
    loop = asyncio.get_event_loop()
    return await loop.run_in_executor(
        executor,
        model.predict,
        np.array(features).reshape(1, -1)
    )

@app.post("/predict/async")
async def async_prediction(features: List[float]):
    """Async prediction endpoint"""
    prediction = await async_predict(features)
    return {"prediction": float(prediction[0])}

@app.post("/predict/batch/async")
async def async_batch_prediction(batch: List[List[float]]):
    """Async batch prediction"""
    tasks = [async_predict(features) for features in batch]
    predictions = await asyncio.gather(*tasks)

    return {"predictions": [float(p[0]) for p in predictions]}
```

## Best Practices

### ML API Guidelines

```python
# Example 7: ML API best practices
"""
ML API Best Practices:

1. Model Management
   - Version your models
   - Store model metadata
   - Enable A/B testing

2. Performance
   - Use async inference
   - Batch predictions when possible
   - Cache frequent predictions

3. Monitoring
   - Track prediction latency
   - Monitor prediction distribution
   - Detect data drift

4. Error Handling
   - Validate input features
   - Handle model loading failures
   - Provide fallback predictions

5. Security
   - Validate input sizes
   - Rate limit requests
   - Monitor for adversarial inputs
"""

# Input validation for ML
from pydantic import validator

class MLPredictionRequest(BaseModel):
    features: List[float]

    @validator('features')
    def validate_features(cls, v):
        if len(v) > 1000:  # Limit feature count
            raise ValueError("Too many features")
        if any(not np.isfinite(x) for x in v):
            raise ValueError("Features must be finite")
        return v
```

## Summary

| Feature | Implementation | Purpose |
|---------|----------------|---------|
| Model serving | Pickle/joblib | Load models |
| Versioning | Model registry | A/B testing |
| Batch | Background tasks | Bulk processing |
| Monitoring | Prometheus | Track performance |
| Features | Feature store | Feature management |

## Next Steps

Continue learning about:
- [Prediction APIs](./02_prediction_apis.md) - Detailed prediction
- [Model Monitoring](./05_ml_model_monitoring.md) - Production monitoring
- [Caching Strategies](../02_performance_optimization/01_caching_strategies.md)
