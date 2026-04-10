---
Category: Machine Learning
Subcategory: ML Fundamentals
Concept: Machine Learning Basics
Purpose: Understanding machine learning fundamentals for cloud applications
Difficulty: beginner
Prerequisites: Python, Statistics
RelatedFiles: 02_Advanced_ML.md
UseCase: Building ML-powered applications
CertificationExam: AWS Machine Learning Specialty
LastUpdated: 2025
---

## WHY

ML is increasingly integrated into cloud applications for intelligent features.

## WHAT

### ML Categories

- **Supervised Learning**: Labeled data training
- **Unsupervised Learning**: Pattern discovery
- **Reinforcement Learning**: Agent-based learning

### Common Algorithms

- Linear Regression
- Decision Trees
- Neural Networks

## HOW

### SageMaker Example

```python
import boto3
import sagemaker
from sagemaker.estimator import Estimator

# Create estimator
estimator = Estimator(
    role='arn:aws:iam::123456789:role/sagemaker-role',
    instance_count=1,
    instance_type='ml.m5.large',
    image_name='linear-learner'
)

# Fit model
estimator.fit({'train': 's3://bucket/train'})

# Deploy endpoint
predictor = estimator.deploy(initial_instance_count=1)
```

## CROSS-REFERENCES

### Related Services

- SageMaker: AWS ML platform
- Rekognition: Image analysis
- Comprehend: NLP