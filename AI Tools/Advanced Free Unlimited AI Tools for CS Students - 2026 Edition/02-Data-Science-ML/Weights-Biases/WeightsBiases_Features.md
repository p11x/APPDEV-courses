# Weights & Biases Features

## Core Features

### 1. Experiment Tracking

| Feature | Description |
|---------|-------------|
| Metrics Logging | Log any metric during training |
| Hyperparameter Tracking | Track all hyperparameters automatically |
| System Metrics | Monitor CPU, GPU, memory usage |
| Training Logs | Capture stdout/stderr from training |
| Git Integration | Automatically track code versions |

### 2. Visualizations

W&B provides automatic visualizations:

- **Line Charts**: Track metrics over time
- **Scatter Plots**: Analyze correlations
- **Histograms**: View parameter distributions
- **Parallel Coordinates**: Multi-dimensional analysis
- **3D Scatter Plots**: Visualize embeddings
- **Custom Charts**: Build tailored visualizations

### 3. Integrations

| Framework | Support Level | Installation |
|-----------|---------------|--------------|
| PyTorch | Full | `pip install wandb` |
| TensorFlow | Full | `pip install wandb` |
| Keras | Full | Built-in |
| Scikit-learn | Full | `pip install wandb` |
| JAX | Full | `pip install wandb` |
| Hugging Face | Full | `pip install wandb` |
| fastai | Full | Built-in |
| PyTorch Lightning | Full | `pip install wandb` |
| Keras | Full | `wandb.keras.WandbCallback()` |

### 4. Sweeps (Hyperparameter Optimization)

W&B provides automated hyperparameter search:

- **Grid Search**: Exhaustive parameter search
- **Random Search**: Random parameter combinations
- **Bayesian Optimization**: Smart search with wandb SDK
- **Hyperband**: Early stopping optimization
- **Parallel Runs**: Run multiple experiments simultaneously

### 5. Artifacts

Model and dataset management:

- **Model Versioning**: Track model evolution
- **Dataset Tracking**: Version datasets
- **Pipeline Tracking**: Track end-to-end workflows
- **Model Registry**: Organize models for deployment
- **Dependencies**: Track model dependencies

### 6. Reports and Dashboards

Create shareable content:

- **Reports**: Generate markdown reports
- **Dashboards**: Custom visualization panels
- **Tables**: Interactive data tables
- **Panels**: Reusable visualization components

### 7. Collaboration Features

Team collaboration tools:

- **Team Workspaces**: Shared workspaces
- **Comments**: Discuss experiments
- **Sharing**: Public or private sharing
- **Reports**: Collaborative documents

## Advanced Features

### 8. Tables

Create interactive tables:

- Sort and filter data
- Add custom columns
- Link to experiments
- Export to CSV/Excel

### 9. Alerts

Set up notifications:

- Experiment completion alerts
- Metric threshold alerts
- Resource usage alerts
- Custom webhook alerts

### 10. API Access

Programmatic access:

- REST API for custom integrations
- Python SDK for automation
- CLI for command-line usage
- Webhooks for CI/CD integration

## Feature Comparison

| Feature | Free Tier | Paid Tier |
|---------|-----------|-----------|
| Experiments | 100/month | Unlimited |
| Artifact Storage | 5GB | 100GB+ |
| Sweep Runs | 3 concurrent | Unlimited |
| Team Members | 1 | Unlimited |
| Private Projects | Limited | Unlimited |
| Reports | Basic | Advanced |

---

*Back to [Weights & Biases README](./README.md)*
*Back to [Data Science README](../README.md)*
*Back to [Main README](../../README.md)*