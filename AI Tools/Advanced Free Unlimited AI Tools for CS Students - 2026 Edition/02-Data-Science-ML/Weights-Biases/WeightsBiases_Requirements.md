# Weights & Biases Requirements

## Account Requirements

| Requirement | Description |
|-------------|-------------|
| Email | Valid email address |
| Account | Free W&B account |
| Verification | Email verification required |
| Age | 13+ years old |

## System Requirements

### Operating System

| OS | Support |
|----|---------|
| Windows | Full support |
| macOS | Full support |
| Linux | Full support |
| Cloud | Works on all platforms |

### Python Requirements

| Component | Requirement |
|-----------|-------------|
| Python Version | 3.6+ |
| pip | Latest version |
| Internet | Required for online logging |

### Hardware Requirements

| Resource | Minimum | Recommended |
|----------|---------|--------------|
| RAM | 2GB | 4GB+ |
| Disk Space | 1GB | 5GB+ |
| GPU | Optional | CUDA-capable |

## Framework Support

### Supported ML Frameworks

| Framework | Version | Support |
|-----------|---------|---------|
| PyTorch | 1.0+ | Full |
| TensorFlow | 2.0+ | Full |
| Keras | 2.2.4+ | Full |
| Scikit-learn | 0.20+ | Full |
| JAX | 0.3+ | Full |
| fastai | 1.0+ | Full |
| Hugging Face | 4.0+ | Full |
| PyTorch Lightning | 1.0+ | Full |

## Network Requirements

| Requirement | Description |
|-------------|-------------|
| Internet | Required for logging |
| Firewall | Allow outbound to wandb.ai |
| Proxy | Configure if needed |

## Getting Started Checklist

- [ ] Create free account at [wandb.ai](https://wandb.ai)
- [ ] Verify email address
- [ ] Install Python 3.6+
- [ ] Install wandb: `pip install wandb`
- [ ] Run `wandb login` to authenticate
- [ ] Start logging experiments

## API Key Setup

### Getting Your API Key

1. Go to [wandb.ai/authorize](https://wandb.ai/authorize)
2. Click "Create an API Key"
3. Copy the key
4. Use `wandb login` to paste the key

### Environment Variable Setup

```bash
# Windows
setx WANDB_API_KEY your-api-key

# Linux/Mac
export WANDB_API_KEY=your-api-key
```

---

*Back to [Weights & Biases README](./README.md)*
*Back to [Data Science README](../README.md)*
*Back to [Main README](../../README.md)*