# Ollama Requirements

## System Requirements

### Minimum Requirements
| Component | Requirement |
|-----------|-------------|
| OS | macOS 10.15+, Linux, or Windows 10+ |
| RAM | 8GB (for smaller models) |
| Storage | 4GB+ for models |
| Internet | Required for initial download |

### Recommended Requirements
| Component | Requirement |
|-----------|-------------|
| OS | macOS 12+ / Ubuntu 20.04+ / Windows 11 |
| RAM | 16GB+ (for larger models) |
| Storage | 20GB+ (for multiple models) |
| GPU | NVIDIA GPU with CUDA (optional, for faster inference) |

## Hardware Recommendations by Use Case

### For Code Generation
- Minimum: 8GB RAM, integrated graphics
- Recommended: 16GB RAM, dedicated GPU
- Models: CodeLlama, StarCoder

### For Learning and General Use
- Minimum: 8GB RAM
- Recommended: 16GB RAM
- Models: Llama 2, Mistral

### For Heavy Development
- Minimum: 16GB RAM
- Recommended: 32GB RAM, NVIDIA GPU
- Multiple models running simultaneously

## Software Dependencies

### For macOS
- No additional dependencies required
- Homebrew recommended for easy installation

### For Linux
- cURL for downloading
- Optional: NVIDIA drivers for GPU acceleration

### For Windows
- Windows 10 version 1903 or later
- Optional: NVIDIA drivers for GPU acceleration

## Network Requirements
- Internet connection required for initial model download
- After installation, can run completely offline
- Models downloaded once and stored locally

## Special Considerations

### For Students
- 8GB RAM laptops can run smaller models (Mistral, Phi-2)
- Consider starting with smaller models if hardware is limited
- Models can be deleted and re-downloaded as needed

### For Privacy-Conscious Users
- No account required
- No data sent to external servers
- Complete offline capability after setup