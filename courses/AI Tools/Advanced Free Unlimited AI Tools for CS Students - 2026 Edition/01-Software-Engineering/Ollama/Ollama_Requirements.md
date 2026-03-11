# Ollama Requirements

## System Requirements

### Minimum Requirements

| Component | Requirement |
|-----------|-------------|
| Operating System | macOS 11+, Linux (Ubuntu 20.04+), Windows 10+ |
| RAM | 8 GB minimum |
| Storage | 10 GB free (more for models) |
| Internet | Required for initial model download |

### Recommended Requirements

| Component | Recommendation |
|-----------|----------------|
| RAM | 16 GB or more |
| GPU | NVIDIA GPU with CUDA support |
| VRAM | 8+ GB VRAM for large models |
| Storage | 50+ GB SSD |

## Hardware Requirements by Model

### CPU-Only Operation

| Model | RAM Needed | Storage | Performance |
|-------|------------|---------|-------------|
| Phi 3 | 8 GB | 4 GB | Slow |
| Mistral | 16 GB | 4 GB | Very slow |
| Llama 3.2 1B | 8 GB | 2 GB | Acceptable |

### GPU-Accelerated Operation

| Model | GPU VRAM | RAM | Storage |
|-------|----------|-----|---------|
| Llama 3.2 1B | 2 GB | 8 GB | 2 GB |
| Llama 3.2 3B | 4 GB | 8 GB | 4 GB |
| Llama 3.2 7B | 8 GB | 8 GB | 8 GB |
| Llama 3.2 70B | 24 GB | 16 GB | 70 GB |

## Software Requirements

### Required Software

1. **Operating System**
   - macOS 11 (Big Sur) or later
   - Ubuntu 20.04 or later
   - Windows 10/11 with WSL2

2. **For GPU Acceleration (NVIDIA)**
   - NVIDIA GPU drivers
   - CUDA Toolkit 12.1+
   - cuDNN 8+

### Checking GPU Support

```bash
# Check NVIDIA GPU
nvidia-smi

# Expected output shows GPU info, VRAM, CUDA version
```

## Installation Prerequisites

### macOS

No additional prerequisites - just download the app.

### Linux

```bash
# Check for CUDA
nvcc --version

# Update package list
sudo apt update
```

### Windows (WSL2)

```bash
# Install WSL2
wsl --install

# Update to latest Ubuntu
wsl --update
```

## Disk Space Planning

### Model Sizes

| Model | Disk Space |
|-------|------------|
| Phi 3 (3.8B) | ~2 GB |
| Llama 3.2 (1B) | ~1.3 GB |
| Llama 3.2 (3B) | ~2 GB |
| Mistral (7B) | ~4 GB |
| Llama 3.2 (7B) | ~4 GB |
| Llama 3.2 (70B) | ~40 GB |
| Codellama (7B) | ~4 GB |

### Recommended Storage

- Minimum: 20 GB free
- Recommended: 50+ GB free
- For all models: 100+ GB free

## Network Requirements

- **Initial Download**: 2-50 GB (one-time per model)
- **Updates**: Occasional small updates
- **Offline Use**: Fully offline after installation

## Environment Considerations

### For Students

- Can run on most modern laptops
- Start with smaller models (Phi, Llama 3.2 1B)
- Upgrade to larger models as needed

### For Campus Networks

- Download models on fast connection
- Use offline once installed
- Consider external SSD for portability

### For Limited Storage

- Delete unused models: `ollama rm modelname`
- Check model sizes before downloading
- Use smaller quantized models