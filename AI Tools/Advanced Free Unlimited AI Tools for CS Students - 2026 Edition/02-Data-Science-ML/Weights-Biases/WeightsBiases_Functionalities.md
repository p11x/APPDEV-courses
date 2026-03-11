# Weights & Biases Functionalities

## What You Can Do with Weights & Biases

### 1. Logging Metrics

Track any metric during training:

```python
import wandb

# Initialize a run
wandb.init(project="my-project")

# Log a single metric
wandb.log({"loss": 0.5})

# Log multiple metrics
wandb.log({
    "loss": 0.5,
    "accuracy": 0.9,
    "learning_rate": 0.01
})

# Log at specific step
wandb.log({"loss": 0.3}, step=100)
```

### 2. Configuration Management

Track hyperparameters and configurations:

```python
# Method 1: Using config dictionary
wandb.init(config={
    "learning_rate": 0.01,
    "batch_size": 32,
    "epochs": 100,
    "optimizer": "adam"
})

# Method 2: Using wandb.config
wandb.init(project="my-project")
wandb.config.learning_rate = 0.01
wandb.config.batch_size = 32
wandb.config.epochs = 100
```

### 3. Artifacts Management

Version and track models and datasets:

```python
# Create an artifact
artifact = wandb.Artifact("my-model", type="model")
artifact.add_file("model.pt")
artifact.add_reference("s3://bucket/model.pt")
wandb.log_artifact(artifact)

# Use an artifact in another run
artifact = wandb.use_artifact("my-model:latest")
artifact.download()
```

### 4. Visualizations

W&B automatically creates visualizations:

| Visualization | Use Case |
|---------------|----------|
| Line Charts | Training curves |
| Scatter Plots | Correlation analysis |
| Histograms | Distribution analysis |
| Parallel Coordinates | Multi-parameter search |
| 3D Scatter | Embedding visualization |

### 5. Framework-Specific Integrations

#### PyTorch

```python
import wandb
import torch

wandb.init(project="pytorch-project")

for epoch in range(10):
    # Training loop
    for batch in dataloader:
        loss = train(batch)
        wandb.log({"loss": loss})
```

#### TensorFlow/Keras

```python
import wandb
from wandb.keras import WandbCallback

wandb.init(project="keras-project")

model.fit(
    x_train, y_train,
    callbacks=[WandbCallback()]
)
```

#### PyTorch Lightning

```python
from pytorch_lightning.loggers import WandbLogger

wandb_logger = WandbLogger(project="lightning-project")

trainer = Trainer(logger=wandb_logger)
```

### 6. Hyperparameter Sweeps

Automated hyperparameter optimization:

```yaml
# sweep.yaml
program: train.py
method: random
metric:
  name: val_loss
  goal: minimize
parameters:
  learning_rate:
    values: [0.01, 0.001, 0.0001]
  batch_size:
    values: [16, 32, 64]
```

Run sweep:

```bash
wandb sweep sweep.yaml
wandb agent <sweep-id>
```

### 7. Reports

Create shareable reports:

- Add markdown content
- Include visualization panels
- Link to experiments
- Share with team or make public

### 8. Tables

Create interactive data tables:

```python
wandb.log({
    "predictions": wandb.Table(
        columns=["image", "prediction", "confidence"],
        data=[[img, pred, conf] for img, pred, conf in results]
    )
})
```

## Common Use Cases

| Use Case | Code Example |
|----------|--------------|
| Basic Logging | `wandb.log({"loss": loss})` |
| Config Tracking | `wandb.config.update(params)` |
| Model Saving | `wandb.log_artifact(model)` |
| Image Logging | `wandb.log({"image": wandb.Image(img)})` |
| Video Logging | `wandb.log({"video": wandb.Video(video)})` |
| Histogram Logging | `wandb.log({"weights": wandb.Histogram(weights)})` |

---

*Back to [Weights & Biases README](./README.md)*
*Back to [Data Science README](../README.md)*
*Back to [Main README](../../README.md)*