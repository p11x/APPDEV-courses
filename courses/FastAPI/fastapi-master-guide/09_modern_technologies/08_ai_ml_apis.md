# AI/ML APIs

## Overview

FastAPI excels at serving machine learning models as APIs.

## Model Serving

### Complete ML API

```python
# Example 1: ML model serving
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
from typing import List, Dict
import joblib
import numpy as np

app = FastAPI(title="ML Prediction API")

class PredictionRequest(BaseModel):
    features: List[float] = Field(..., min_length=1)

class PredictionResponse(BaseModel):
    prediction: float
    confidence: float
    model_version: str

class ModelRegistry:
    """Manage ML models"""

    def __init__(self):
        self.models: Dict[str, object] = {}
        self.versions: Dict[str, str] = {}

    def load_model(self, name: str, path: str, version: str):
        """Load model from file"""
        self.models[name] = joblib.load(path)
        self.versions[name] = version

    def predict(self, name: str, features: np.ndarray):
        """Make prediction"""
        if name not in self.models:
            raise KeyError(f"Model {name} not found")
        return self.models[name].predict(features)

registry = ModelRegistry()

@app.on_event("startup")
async def load_models():
    """Load models on startup"""
    registry.load_model("classifier", "models/classifier.pkl", "1.0")
    registry.load_model("regressor", "models/regressor.pkl", "1.0")

@app.post("/predict/{model_name}", response_model=PredictionResponse)
async def predict(model_name: str, request: PredictionRequest):
    """Make prediction"""
    try:
        features = np.array(request.features).reshape(1, -1)
        prediction = registry.predict(model_name, features)

        return PredictionResponse(
            prediction=float(prediction[0]),
            confidence=0.95,
            model_version=registry.versions[model_name]
        )
    except KeyError:
        raise HTTPException(404, f"Model {model_name} not found")
```

## Summary

FastAPI provides excellent support for ML model serving.

## Next Steps

Continue learning about:
- [Real-Time Analytics](./09_real_time_analytics.md)
- [Streaming APIs](./10_streaming_apis.md)
