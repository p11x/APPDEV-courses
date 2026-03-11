# LM Studio Requirements

## System Requirements

### Minimum Requirements

| Component | Requirement |
|-----------|-------------|
| Operating System | macOS 10.15+, Windows 10+, Linux (Ubuntu 20.04+) |
| RAM | 8 GB minimum |
| Storage | 5 GB free (plus models) |
| Internet | Required for initial model downloads |

### Recommended Requirements

| Component | Recommendation |
|-----------|----------------|
| RAM | 16 GB |
| GPU | NVIDIA GPU with CUDA OR Apple Silicon (M1/M2/M3) |
| Storage | 30+ GB free |
| Display | 1280x720 minimum |

## Hardware Requirements by Use Case

### Basic Use (CPU Only)

| Scenario | RAM | Storage |
|----------|-----|---------|
| Small models (Phi 3) | 8 GB | 10 GB |
| Medium models (Mistral) | 16 GB | 15 GB |

### GPU Accelerated

| Model Size | GPU VRAM | RAM | Storage |
|------------|----------|-----|---------|
| 3B parameters | 4 GB | 8 GB | 5 GB |
| 7B parameters | 8 GB | 8 GB | 8 GB |
| 13B parameters | 10 GB | 12 GB | 15 GB |
| 70B parameters | 24 GB | 16 GB | 70 GB |

## Platform-Specific Requirements

### macOS

- Apple Silicon (M1, M2, M3) recommended for best performance
- Intel Mac supported but slower
- No additional software needed

### Windows

- Windows 10 version 1903 or later
- Windows 11 recommended
- Optional: NVIDIA drivers for GPU acceleration

### Linux

- Ubuntu 20.04 or later
- Debian 10 or later
- Optional: NVIDIA drivers + CUDA for GPU

## GPU Requirements

### NVIDIA GPUs

| GPU Series | VRAM | Support |
|------------|------|---------|
| GTX 16xx | 4-6 GB | Basic |
| RTX 20xx | 6-8 GB | Good |
| RTX 30xx | 8-24 GB | Excellent |
| RTX 40xx | 12-24 GB | Best |

### Apple Silicon

| Chip | Neural Engine | Support |
|------|---------------|---------|
| M1 | 11 TOPS | Good |
| M2 | 15.8 TOPS | Very Good |
| M3 | 18 TOPS | Excellent |

## Software Requirements

### Required

1. Modern web browser (for downloads)
2. Internet connection (for initial setup)

### Optional

- NVIDIA CUDA Toolkit 12.1+ (for GPU acceleration)
- cuDNN 8+ (for optimal GPU performance)

## Disk Space Planning

### Model Sizes

| Model | Disk Space |
|-------|------------|
| Phi 3 Mini | ~2 GB |
| Llama 3.2 1B | ~1.3 GB |
| Llama 3.2 3B | ~2 GB |
| Mistral 7B | ~4 GB |
| Llama 3.2 70B | ~40 GB |

### Recommendations

- Minimum: 15 GB free
- Recommended: 50 GB free
- For large models: 100+ GB free

## Network Requirements

- **Initial Setup**: ~100 MB
- **Model Downloads**: 2-50 GB per model
- **Updates**: Occasional small updates
- **Offline Use**: Supported after download

## Checking Your System

### Check RAM (Windows)

```
Task Manager > Performance tab
```

### Check RAM (macOS)

```
About This Mac > Memory
```

### Check GPU (Windows)

```
Device Manager > Display adapters
```

### Check GPU (macOS)

```
System Report > Graphics/Displays
```