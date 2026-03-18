# ML Model Serving

## What You'll Learn
- Serving ML models via API
- Using pickle/joblib for models
- Input validation for ML
- Batch prediction

## Prerequisites
- Basic ML knowledge

## Installation

```bash
pip install scikit-learn joblib numpy
```

## Creating Model API

```python
from fastapi import FastAPI
from pydantic import BaseModel
import joblib
import numpy as np

app = FastAPI()

# Load model at startup
model = joblib.load("model.joblib")

class PredictionRequest(BaseModel):
    features: list[float]

class PredictionResponse(BaseModel):
    prediction: float
    probability: float | None = None

@app.post("/predict", response_model=PredictionResponse)
async def predict(request: PredictionRequest):
    """Make prediction on input features."""
    
    features = np.array(request.features).reshape(1, -1)
    prediction = model.predict(features)[0]
    
    # Get probability if available
    prob = None
    if hasattr(model, "predict_proba"):
        prob = model.predict_proba(features)[0].max()
    
    return PredictionResponse(
        prediction=float(prediction),
        probability=float(prob) if prob else None
    )

@app.post("/predict/batch")
async def predict_batch(requests: list[PredictionRequest]):
    """Batch predictions."""
    
    features = np.array([r.features for r in requests])
    predictions = model.predict(features)
    
    return {"predictions": predictions.tolist()}
```

## Summary

- Serve ML models via FastAPI endpoints
- Use Pydantic for input validation
- Load models at startup for performance
- Support batch predictions for efficiency
