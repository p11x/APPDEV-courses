# SKLearn Pipeline

## What You'll Learn
- Chain steps
- make_pipeline

## Prerequisites
- Read 03_model_evaluation.md first

## Overview
ML pipelines.

## Pipeline
Chain

```python
from sklearn.pipeline import Pipeline
pipe = Pipeline([("scaler", StandardScaler()), ("clf", LogisticRegression())])
```

## Common Mistakes
- wrong order
- not using

## Summary
- list of steps
- order matters
- clean code

## Next Steps
Continue to **[](./)**
