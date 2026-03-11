# Weights & Biases Installation Guide

## Prerequisites

Before installing W&B, ensure you have:

| Requirement | Description |
|-------------|-------------|
| Python | 3.6 or higher |
| pip | Latest version recommended |
| Account | Free W&B account |
| Internet | Required for logging |

## Installation Steps

### Step 1: Install W&B

```bash
# Using pip
pip install wandb

# Using conda
conda install wandb -c conda-forge
```

### Step 2: Verify Installation

```bash
# Check version
wandb --version

# Should output: wandb version x.x.x
```

### Step 3: Login to W&B

```bash
# Login with API key
wandb login

# Or use environment variable
export WANDB_API_KEY=your-api-key
```

### Step 4: Initialize in Your Code

```python
import wandb

# Initialize a run
wandb.init(project="my-first-project")

# Log a metric
wandb.log({"accuracy": 0.95})

# Finish the run
wandb.finish()
```

## Framework-Specific Setup

### PyTorch

```python
import wandb
import torch
import torch.nn as nn

# Initialize
wandb.init(project="pytorch-project")

# Watch model (automatic logging)
wandb.watch(model)

# Training loop
for epoch in range(10):
    for batch in dataloader:
        loss = train(batch)
        wandb.log({"loss": loss})
```

### TensorFlow/Keras

```python
import wandb
from wandb.keras import WandbCallback

# Initialize
wandb.init(project="keras-project")

# Add callback to model.fit
model.fit(
    x_train, y_train,
    epochs=10,
    callbacks=[WandbCallback()]
)
```

### PyTorch Lightning

```python
from pytorch_lightning.loggers import WandbLogger

# Create logger
wandb_logger = WandbLogger(project="lightning-project")

# Use in trainer
trainer = Trainer(logger=wandb_logger)
```

### Hugging Face Transformers

```python
from transformers import Trainer, TrainingArguments

training_args = TrainingArguments(
    output_dir="output",
    report_to="wandb",
    run_name="my-run"
)

trainer = Trainer(
    model=model,
    args=training_args,
    train_dataset=train_dataset
)

trainer.train()
```

## Configuration

### Environment Variables

| Variable | Description |
|----------|-------------|
| WANDB_API_KEY | Your API key |
| WANDB_PROJECT | Default project name |
| WANDB_ENTITY | Team or username |
| WANDB_MODE | "online", "offline", or "disabled" |

### Using .env File

Create a `.env` file:

```bash
WANDB_API_KEY=your-api-key-here
WANDB_PROJECT=my-project
```

Load in Python:

```python
from dotenv import load_dotenv
load_dotenv()
```

## Offline Mode

Use offline mode when you don't have internet:

```python
# Initialize offline
wandb.init(mode="offline")

# Sync later when online
wandb.sync()
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Login failed | Run `wandb login` again |
| Import error | Reinstall: `pip install --upgrade wandb` |
| Network issues | Use offline mode |
| Permission denied | Check API key permissions |

---

*Back to [Weights & Biases README](./README.md)*
*Back to [Data Science README](../README.md)*
*Back to [Main README](../../README.md)*